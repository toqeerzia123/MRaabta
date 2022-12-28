using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using MRaabta.App_Code;
using System.Text.RegularExpressions;

namespace MRaabta.Files
{
    public partial class Manage_ConsignmentApproval_Import : System.Web.UI.Page
    {
        Cl_Variables clvar = new Cl_Variables();
        CommonFunction func = new CommonFunction();
        Consignemnts con = new Consignemnts();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                picker_reportingDate.SelectedDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
                picker_bookingDate.SelectedDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
                Get_origin();
                Get_Service_Type();
                Get_ConType();
                CODSequence();
                Get_OriginExpressCenter();
                Get_PriceModifiers();
                GetFuelSurcharges();
                Get_originCountry();
                txt_chargedAmount.Text = "0";
                txt_totalAmt.Text = "0";
                txt_gst.Text = "0";

                DataTable pm = new DataTable();
                pm.Columns.Add("priceModifierId");
                pm.Columns.Add("priceModifierName");
                pm.Columns.Add("calculatedValue");
                pm.Columns.Add("ModifiedCalculatedValue");
                pm.Columns.Add("calculationBase");
                pm.Columns.Add("isTaxable");
                pm.Columns.Add("description");
                pm.Columns.Add("SortOrder");
                pm.Columns.Add("NEW");
                pm.Columns.Add("AlternateValue");
                pm.AcceptChanges();
                ViewState["pm"] = pm;
                DataTable profiles = GetAccessProfiles();
                if (profiles != null)
                {
                    if (profiles.Rows.Count > 0)
                    {

                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "alert('You are not authorized to view this page.'); window.location='" + Request.ApplicationPath + "/Login';", true);
                        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('You are not authorized to view this page.')", true);
                        //Response.Redirect("~/Login");
                        return;
                    }
                }
                else
                {

                }
            }
        }

        protected DataTable GetAccessProfiles()
        {
            string sql = "SELECT pd.Profile_Id \n"
               + "FROM   Profile_Detail pd \n"
               + "       INNER JOIN Profile_Head ph \n"
               + "            ON  ph.profile_id = pd.Profile_Id \n"
               + "WHERE  pd.ChildMenu_Id in ('67', '60', '260','625') and pd.profile_id = '" + HttpContext.Current.Session["Profile"].ToString() + "'";

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sql, con);
                sda.Fill(dt);
            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return dt;
        }

        public void Get_ProductType()
        {

            dd_productType.Items.Clear();
            DataSet ds = con.ProductTypeInfo(clvar);
            if (ds.Tables[0].Rows.Count != 0)
            {
                dd_productType.DataTextField = "productTypeName";
                dd_productType.DataValueField = "productTypeCode";
                dd_productType.DataSource = ds.Tables[0].DefaultView;
                dd_productType.DataBind();
            }
            dd_productType.Items.Insert(0, new ListItem("Select Product Type", "0"));
        }
        public void Get_origin()
        {
            DataSet ds = func.Branch();
            if (ds.Tables.Count != 0)
            {
                //  cb_Destination
                this.dd_origin.DataTextField = "BranchName";
                this.dd_origin.DataValueField = "branchCode";
                this.dd_origin.DataSource = ds.Tables[0].DefaultView;
                this.dd_origin.DataBind();
                try
                {
                    dd_origin.SelectedValue = HttpContext.Current.Session["BranchCode"].ToString();
                    //dd_origin.Enabled = false;
                }
                catch (Exception ex)
                {

                    Response.Redirect("~/Login");
                }
                DataTable dt = Cities_();
                this.dd_destination.DataTextField = "sname";
                this.dd_destination.DataValueField = "branchCode";
                this.dd_destination.DataSource = dt;//ds.Tables[0].DefaultView;
                this.dd_destination.DataBind();
                ViewState["cities"] = dt;
            }
        }

        public void Get_originCountry()
        {
            DataSet ds = OriginCountry();
            if (ds.Tables.Count != 0)
            {
                //  cb_Destination
                this.Or_Country.DataTextField = "CountryName";
                this.Or_Country.DataValueField = "Sno";
                this.Or_Country.DataSource = ds.Tables[0].DefaultView;
                this.Or_Country.DataBind();

            }
        }

        public DataSet OriginCountry()
        {
            DataSet Ds_1 = new DataSet();

            try
            {
                string query = "Select num Sno, Name CountryName from Country where status ='1'";

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

        public DataSet ServiceTypeName()
        {
            DataSet Ds_1 = new DataSet();

            try
            {
                string query = "SELECT st.name          ServiceTypeName, isintl \n"
                + "FROM   ServiceTypes_new     st \n"
                + "WHERE  \n"
                + "       st.[status] = '1' \n"
                + "       And st.products in ('Import') \n"
                + "GROUP BY \n"
                + "       st.name,isintl  \n"
                + "ORDER BY \n"
                + "       st.name";

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

        public void Get_Service_Type()
        {
            DataSet ds = ServiceTypeName();
            if (ds.Tables.Count != 0)
            {
                //  cb_Destination
                //       this.cb_ServiceType.Items.Add(new RadComboBoxItem("Select Service Type Name", "0"));
                this.dd_serviceType.DataTextField = "ServiceTypeName";
                this.dd_serviceType.DataValueField = "ServiceTypeName";
                this.dd_serviceType.DataSource = ds.Tables[0].DefaultView;
                this.dd_serviceType.DataBind();

                //dd_serviceType.SelectedValue = "overnight";
            }
        }

        public DataSet ConsignmentType()
        {
            DataSet Ds_1 = new DataSet();

            try
            {
                string query = " SELECT ct.id, ct.name ConsignmentType FROM ConsignmentType ct  \n"
                + "       WHERE ct.[status]='1' and name in ('DDP','DDU','Normal') \n"
                + "       GROUP BY ct.name,ct.id";

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

        public void Get_ConType()
        {
            DataSet ds = ConsignmentType();
            if (ds.Tables.Count != 0)
            {
                //  cb_Destination
                //this.dd_consignmentType.Items.Add(new RadComboBoxItem("Select Consignment Type", "0"));
                this.dd_consignmentType.DataTextField = "ConsignmentType";
                this.dd_consignmentType.DataValueField = "id";
                this.dd_consignmentType.DataSource = ds.Tables[0].DefaultView;
                this.dd_consignmentType.DataBind();
                //cb_ConType.SelectedValue = "12";
            }
        }

        public DataSet PriceModifiers1()
        {
            DataSet Ds_1 = new DataSet();

            try
            {
                string query = "SELECT id, \n"
               + "       pm.name PriceModifier, \n"
               + "       pm.calculationValue, \n"
               + "       pm.[description], calculationbase, isgst \n"
               + "FROM   PriceModifiers pm \n"
               + "WHERE  pm.[status] = '1' \n"
               + "AND pm.chkBillingModifier ='0' and import ='1' \n"
               + "GROUP BY \n"
               + "       id, \n"
               + "       pm.name, \n"
               + "       pm.calculationValue, \n"
               + "       pm.[description], pm.calculationbase, isgst \n"
               + "ORDER BY pm.name";

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

        public void Get_PriceModifiers()
        {
            DataSet ds = PriceModifiers1();
            if (ds.Tables.Count != 0)
            {
                ////  cb_Destination
                ////this.dd_priceModifier.Items.Add(new RadComboBoxItem("Select Price Modifier", "0"));
                //this.dd_priceModifier.DataTextField = "PriceModifier";
                //this.dd_priceModifier.DataValueField = "id";
                //this.dd_priceModifier.DataSource = ds.Tables[0].DefaultView;
                //this.dd_priceModifier.DataBind();

                ViewState["PM_"] = ds.Tables[0];
            }
        }
        //public void Get_PriceModifiers()
        //{
        //    DataSet ds = func.PriceModifiers();
        //    if (ds.Tables.Count != 0)
        //    {
        //        //  cb_Destination
        //        this.cb_PriceModifier.Items.Add(new RadComboBoxItem("Select Price Modifier", "0"));
        //        this.cb_PriceModifier.DataTextField = "PriceModifier";
        //        this.cb_PriceModifier.DataValueField = "id";
        //        this.cb_PriceModifier.DataSource = ds.Tables[0].DefaultView;
        //        this.cb_PriceModifier.DataBind();

        //        ViewState["PM_"] = ds.Tables[0];
        //    }
        //}
        public void Get_OriginExpressCenter()
        {
            try
            {
                clvar.origin = HttpContext.Current.Session["BranchCode"].ToString();
                //dd_origin.Enabled = false;
            }
            catch (Exception ex)
            {

                Response.Redirect("~/Login");
            }
            DataSet ds = func.ExpressCenterOrigin(clvar);
            if (ds.Tables.Count != 0)
            {
                //  cb_Destination
                //this.cb_Destination.Items.Add(new DropDownListItem("Select Destination", "0"));
                //this.dd_originExpressCenter.Items.Clear();
                this.dd_originExpressCenter.DataTextField = "Name";
                this.dd_originExpressCenter.DataValueField = "expresscentercode";
                this.dd_originExpressCenter.DataSource = ds.Tables[0].DefaultView;
                this.dd_originExpressCenter.DataBind();
                //this.dd_originExpressCenter.Enabled = false;

                ViewState["Ex_origin"] = ds.Tables[0];
                // hd_oecCatid.Value = ds.Tables[0].Rows[0]["CategoryID"].toString(); 
            }
        }
        protected void GetFuelSurcharges()
        {

            DataTable dt = new DataTable();


            try
            {

                DataSet ds = PriceModifiers1();
                ViewState["PriceModifiers"] = ds.Tables[0];
                dt = new DataTable();
                dt.Columns.Add("ID", typeof(string));
                dt.Columns.Add("NAME", typeof(string));
                dt.Columns.Add("base", typeof(string));
                dt.Columns.Add("isgst", typeof(string));
                dt.AcceptChanges();
                int count = 0;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    DataRow dr_ = dt.NewRow();
                    dr_["ID"] = dr["ID"].ToString();
                    dr_["NAME"] = dr["PriceModifier"].ToString();
                    dr_["BASE"] = dr["ID"].ToString() + "-" + dr["calculationBase"].ToString() + "-" + dr["calculationValue"].ToString() + "-" + dr["isgst"].ToString();
                    dr_["isgst"] = dr["isgst"].ToString();
                    dt.Rows.Add(dr_);
                    dt.AcceptChanges();
                }
                if (dt.Rows.Count > 0)
                {
                    dd_priceModifier.DataSource = dt;
                    dd_priceModifier.DataTextField = "name";
                    dd_priceModifier.DataValueField = "base";
                    dd_priceModifier.DataBind();
                }
            }
            catch (Exception ex)
            { }
            finally
            {
            }

        }

        public DataSet PriceModifiers()
        {
            DataSet Ds_1 = new DataSet();

            try
            {
                string query = "SELECT id, \n"
               + "       pm.name PriceModifier, \n"
               + "       pm.calculationValue, \n"
               + "       pm.[description], calculationbase, '' AlternateValue,isgst \n"
               + "FROM   PriceModifiers pm \n"
               + "WHERE  pm.[status] = '1' \n"
               + "AND pm.chkBillingModifier ='0' and import ='1' \n"
               + "GROUP BY \n"
               + "       id, \n"
               + "       pm.name, \n"
               + "       pm.calculationValue, \n"
               + "       pm.[description], pm.calculationbase \n"
               + "ORDER BY pm.name";

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


            DataTable pm_ = new DataTable();
            pm_.Columns.Add("priceModifierId");
            pm_.Columns.Add("priceModifierName");
            pm_.Columns.Add("calculatedValue");
            pm_.Columns.Add("ModifiedCalculatedValue");
            pm_.Columns.Add("calculationBase");
            pm_.Columns.Add("isTaxable");
            pm_.Columns.Add("description");
            pm_.Columns.Add("SortOrder");
            pm_.Columns.Add("NEW");
            pm_.Columns.Add("AlternateValue");
            pm_.AcceptChanges();
            ViewState["pm"] = pm_;

            gv_CNModifiers.DataSource = pm_;
            gv_CNModifiers.DataBind();

            DataTable cities = ViewState["cities"] as DataTable;
            dd_origin.Enabled = false;
            txt_cnNumber.Text = txt_cnNumber.Text.Trim(' ');
            clvar.consignmentNo = txt_cnNumber.Text;

            //DataTable codSequence = ViewState["codSequence"] as DataTable;

            DataTable dt = GetConsignmentForApproval(clvar);
            //DataTable dt_ = GetCODConsignmentForApproval(clvar);

            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    hd_BookingDate.Value = dt.Rows[0]["BookingDate"].ToString();
                    hd_AccountReceivingDate.Value = dt.Rows[0]["AccountReceivingDate"].ToString();
                    txt_remarks.Text = dt.Rows[0]["remarks"].ToString();
                    txt_l.Text = dt.Rows[0]["Breadth"].ToString();
                    txt_w.Text = dt.Rows[0]["Width"].ToString();
                    txt_h.Text = dt.Rows[0]["Height"].ToString();
                    txt_pieces.Text = dt.Rows[0]["Pieces"].ToString();

                    txt_aWeight.Text = dt.Rows[0]["DenseWeight"].ToString();
                    float temp = 0;
                    float.TryParse(txt_aWeight.Text, out temp);
                    if (temp != 0)
                    {
                        txt_aWeight.Text = temp.ToString();
                    }

                    double breadth = 0;
                    double width = 0;
                    double height = 0;
                    int pieces = 1;

                    double.TryParse(txt_l.Text.Trim(), out breadth);
                    double.TryParse(txt_w.Text.Trim(), out width);
                    double.TryParse(txt_h.Text.Trim(), out height);
                    int.TryParse(txt_pieces.Text.Trim(), out pieces);
                    double volumetricWeight = ((breadth * width * height) / 5000) * pieces;

                    txt_vWeight.Text = volumetricWeight.ToString();

                    hd_portalConsignment.Value = dt.Rows[0]["PortalCN"].ToString();
                    if (dt.Rows[0]["orgin"].ToString() != HttpContext.Current.Session["BranchCode"].ToString())
                    {
                        //hd_unApproveable.Value = "0";
                    }
                    else
                    {
                        hd_unApproveable.Value = "1";
                    }
                    // dd_origin.Enabled = true;
                    txt_coupon.Text = dt.Rows[0]["CouponNumber"].ToString();
                    txt_weight.Text = dt.Rows[0]["Weight"].ToString();
                    txt_totalAmt.Text = dt.Rows[0]["totalAmount"].ToString();
                    txt_gst.Text = dt.Rows[0]["GST"].ToString();
                    ViewState["CNState"] = "OLD";

                    txt_consigneeCell.Text = dt.Rows[0]["ConsigneePhoneNo"].ToString();
                    txt_consignerCell.Text = dt.Rows[0]["ConsignerCellNo"].ToString();
                    txt_Address.Text = dt.Rows[0]["Address"].ToString();
                    txt_pieces.Text = dt.Rows[0]["pieces"].ToString();


                    if (dt.Rows[0]["accountNo"].ToString() == "0")
                    {
                        txt_chargedAmount.Enabled = true;
                    }
                    else
                    {
                        txt_chargedAmount.Enabled = false;
                    }
                    if (txt_accountNo.Text == "0")
                    {
                        txt_chargedAmount.Enabled = true;
                    }
                    else
                    {
                        txt_chargedAmount.Enabled = false;
                    }


                    hd_CreditClientID.Value = dt.Rows[0]["creditClientId"].ToString();

                    if (chk_accountNoFreeze.Checked == true)
                    {
                    }
                    else
                    {
                        txt_accountNo.Text = dt.Rows[0]["accountNo"].ToString();
                        txt_accountNo_TextChanged(sender, e);
                        txt_consigner.Text = dt.Rows[0]["Consigner"].ToString();

                    }

                    txt_chargedAmount.Text = dt.Rows[0]["chargedAmount"].ToString();
                    txt_consignee.Text = dt.Rows[0]["Consignee"].ToString();
                    txt_invoiceNumber.Text = dt.Rows[0]["invoiceNumber_"].ToString();
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dr["invoiceNumber_"].ToString() != "")
                        {
                            txt_invoiceNumber.Text = dr["invoiceNumber_"].ToString();
                            hd_unApproveable.Value = "0";
                            break;
                        }
                    }
                    txt_invoiceStatus.Text = dt.Rows[0]["deliveryStatus"].ToString();
                    hd_COD.Value = dt.Rows[0]["COD"].ToString();

                    txt_riderCode.Text = dt.Rows[0]["RiderCode"].ToString();
                    txt_riderCode_TextChanged(this, e);
                    if (chk_bulkUpdate.Checked == true)
                    {

                    }
                    else
                    {
                        txt_weight.Text = dt.Rows[0]["weight"].ToString();
                    }
                    hd_customerType.Value = dt.Rows[0]["CustomerType"].ToString();
                    if (dt.Rows[0]["AccountReceivingDate"].ToString() != "")
                    {
                        picker_reportingDate.SelectedDate = DateTime.Parse(dt.Rows[0]["AccountReceivingDate"].ToString());
                    }
                    if (dt.Rows[0]["BookingDate"].ToString().Trim() != "")
                    {
                        picker_bookingDate.SelectedDate = DateTime.Parse(dt.Rows[0]["BookingDate"].ToString());
                    }
                    try
                    {
                        dd_approvalStatus.SelectedValue = dt.Rows[0]["isApproved"].ToString();
                    }
                    catch (Exception ex)
                    { dd_approvalStatus.SelectedValue = "0"; }

                    if (dt.Rows[0]["ConsignmentTypeID"].ToString() != "")
                    {
                        //if (dt.Rows[0]["ConsignmentTypeID"].ToString() != "4")
                        //{
                        // dd_consignmentType.SelectedItem.Text.fi //= dt.Rows[0]["consignmentTypeId"].ToString();
                        dd_consignmentType.SelectedIndex = dd_consignmentType.Items.IndexOf(dd_consignmentType.Items.FindByValue(dt.Rows[0]["consignmentTypeId"].ToString()));
                        //}
                    }
                    else
                    {
                        dd_consignmentType.ClearSelection();
                    }

                    if (dt.Rows[0]["origin_country"].ToString() != "")
                    {
                        //if (dt.Rows[0]["ConsignmentTypeID"].ToString() != "4")
                        //{
                        // dd_consignmentType.SelectedItem.Text.fi //= dt.Rows[0]["consignmentTypeId"].ToString();
                        //  dd_consignmentType.SelectedIndex = dd_consignmentType.Items.IndexOf(dd_consignmentType.Items.FindByValue(dt.Rows[0]["consignmentTypeId"].ToString()));
                        Or_Country.SelectedIndex = Or_Country.Items.IndexOf(Or_Country.Items.FindByValue(dt.Rows[0]["origin_country"].ToString()));
                        //}
                    }
                    else
                    {
                        Or_Country.ClearSelection();
                    }


                    try
                    {
                        dd_destination.SelectedValue = dt.Rows[0]["destination"].ToString();
                    }
                    catch (Exception ex)
                    {
                        dd_destination.ClearSelection();
                    }

                    //dd_origin.SelectedValue = dt.Rows[0]["Orgin"].ToString();

                    if (dt.Rows[0]["originExpressCenter"].ToString() != "")
                    {
                        try
                        {
                            //dd_originExpressCenter.SelectedValue = dt.Rows[0]["originExpressCenter"].ToString();
                        }
                        catch (Exception ex)
                        { }

                    }
                    try
                    {
                        picker_reportingDate.SelectedDate = DateTime.Parse(dt.Rows[0]["accountReceivingDate"].ToString());
                    }
                    catch (Exception ex)
                    { }
                    if (dt.Rows[0]["ServiceTypeName"].ToString().Trim() != "")
                    {
                        bool found = false;
                        foreach (ListItem item in dd_serviceType.Items)
                        {
                            if (item.Text == dt.Rows[0]["ServiceTypeName"].ToString().Trim())
                            {
                                found = true;
                                break;
                            }
                        }
                        if (found)
                        {
                            dd_serviceType.SelectedValue = dt.Rows[0]["ServiceTypeName"].ToString();
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Invalid Service Type. " + dt.Rows[0]["serviceTypeName"].ToString() + "')", true);
                        }

                    }
                    else
                    {
                        dd_serviceType.ClearSelection();
                    }

                    DataTable pm = ViewState["pm"] as DataTable;
                    pm.Clear();
                    foreach (DataRow row in dt.Select("priceModifierId is not null", ""))
                    {
                        DataRow dr = pm.NewRow();
                        dr["priceModifierId"] = row["priceModifierId"];
                        dr["priceModifierName"] = row["priceModifierName"];
                        dr["calculatedValue"] = row["ModifiedCalculatedValue"];
                        dr["ModifiedCalculatedValue"] = row["ModifiedCalculatedValue"];
                        dr["calculationBase"] = row["calculationBase"];
                        dr["isTaxable"] = row["isTaxable"];
                        dr["description"] = row["description"];
                        dr["SortOrder"] = row["SortOrder"];
                        dr["AlternateValue"] = row["AlternateValue"];
                        pm.Rows.Add(dr);
                        pm.AcceptChanges();
                    }
                    gv_CNModifiers.DataSource = pm;
                    gv_CNModifiers.DataBind();

                    if (pm.Rows.Count > 0)
                    {
                        cb_checkbox.Checked = true;
                        codTable.Visible = true;
                    }
                    else
                    {
                        cb_checkbox.Checked = false;
                        codTable.Visible = false;
                    }

                    txt_riderCode_TextChanged(sender, e);
                    txt_cnNumber.Focus();
                    if (txt_invoiceNumber.Text.Trim() != "")
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage()", "alert('Consignment Already Invoiced. Cannot Approve')", true);
                        return;
                    }

                    clvar.consignmentNo = txt_cnNumber.Text;



                    DataSet chk_cod = Check_CODConsignmentDetail(clvar);

                    if (chk_cod.Tables[0].Rows.Count != 0)
                    {
                        clvar.status = 0;

                        txt_orderRefNo.Text = chk_cod.Tables[0].Rows[0]["orderRefNo"].ToString();
                        Cb_CODAmount.Checked = true;
                        txt_descriptionCOD.Text = chk_cod.Tables[0].Rows[0]["ProductDescription"].ToString();
                        txt_codAmount.Text = chk_cod.Tables[0].Rows[0]["codAmount"].ToString();


                    }

                    clvar.Branch = dd_origin.SelectedValue;
                    DataTable dates = MinimumDate(clvar);
                    if (dates.Rows[0][0].ToString() == "")
                    {
                        AlertMessage("You are not Authorized to Use Approval.");
                        btn_reset_Click(this, e);
                        return;
                    }
                    DateTime minAllowedDate = DateTime.Parse(dates.Rows[0]["DateAllowed"].ToString()).AddDays(1);
                    DateTime maxAllowedDate = DateTime.Now;
                    if (picker_bookingDate.SelectedDate < minAllowedDate || picker_bookingDate.SelectedDate > maxAllowedDate || picker_reportingDate.SelectedDate < minAllowedDate || picker_reportingDate.SelectedDate > maxAllowedDate)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Day End has already been performed for selected Dates. Cannot UnApprove CN')", true);
                        return;
                    }
                }
                else
                {


                    //foreach (DataRow dr in codSequence.Rows)
                    //{
                    //    Int64 start = Int64.Parse(dr["sequenceStart"].ToString());
                    //    Int64 end = Int64.Parse(dr["sequenceEnd"].ToString());
                    //    Int64 num = Int64.Parse(txt_cnNumber.Text);
                    //    if (start <= num && end >= num)
                    //    {
                    //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('CN Number Already Researved. Cannot Use this Sequence')", true);
                    //        txt_cnNumber.Text = "";
                    //        txt_cnNumber.Focus();
                    //        return;
                    //    }
                    //}
                    hd_portalConsignment.Value = "0";
                    string specialCondition = "\n ";
                    dd_approvalStatus.SelectedValue = "0";
                    clvar.Zone = HttpContext.Current.Session["ZoneCode"].ToString();
                    clvar.consignmentNo = txt_cnNumber.Text.Trim();
                    DataTable allowedCN = SequenceCheck(clvar, specialCondition);
                    if (allowedCN.Rows.Count == 0)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('This CN Number cannot be used in this Zone')", true);
                        txt_cnNumber.Text = "";
                        return;
                    }
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('New Consignment')", true);
                    ViewState["CNState"] = "NEW";

                    txt_cnNumber.Focus();



                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Invalid Consignment Number')", true);
            }
        }
        protected void btn_reset_Click(object sender, EventArgs e)
        {
            if (chk_bulkUpdate.Checked == true)
            {
                txt_remarks.Text = "";
                txt_consigner.Text = "";
                txt_cnNumber.Text = "";
                txt_chargedAmount.Text = "";
                txt_consignee.Text = "";
                txt_invoiceNumber.Text = "";
                txt_invoiceStatus.Text = "";
                txt_riderCode.Text = "";
                txt_weight.Text = "";
                txt_gst.Text = "0";
                txt_totalAmt.Text = "0";
                txt_consigneeCell.Text = "";
                txt_consignerCell.Text = "";
                try
                {
                    //   dd_origin.SelectedValue = HttpContext.Current.Session["BranchCode"].ToString();
                }
                catch (Exception ex)
                {
                }
                dd_approvalStatus.ClearSelection();
                dd_consignmentType.ClearSelection();
                dd_destination.ClearSelection();
                //dd_origin.ClearSelection();
                dd_originExpressCenter.ClearSelection();
                txt_pieces.Text = "1";
                txt_Address.Text = "";
                dd_approvalStatus.SelectedValue = "0";
                txt_coupon.Text = "";
                txt_cnNumber.Focus();
            }
            else
            {
                txt_remarks.Text = "";
                txt_consigner.Text = "";
                txt_cnNumber.Text = "";
                txt_accountNo.Text = "";
                txt_chargedAmount.Text = "";
                txt_consignee.Text = "";
                txt_invoiceNumber.Text = "";
                txt_invoiceStatus.Text = "";
                txt_riderCode.Text = "";
                txt_weight.Text = "";
                txt_rangeFrom.Text = "";
                txt_rangeTo.Text = "";
                chk_bulkUpdate.Checked = false;
                txt_gst.Text = "0";
                txt_totalAmt.Text = "0";
                txt_coupon.Text = "";
                try
                {
                    //  dd_origin.SelectedValue = HttpContext.Current.Session["BranchCode"].ToString();
                }
                catch (Exception ex)
                {
                }
                dd_approvalStatus.ClearSelection();
                dd_consignmentType.ClearSelection();
                dd_destination.ClearSelection();
                //dd_origin.ClearSelection();
                dd_originExpressCenter.ClearSelection();
                dd_serviceType.ClearSelection();
                txt_consigneeCell.Text = "";
                txt_consignerCell.Text = "";
                txt_pieces.Text = "1";
                txt_Address.Text = "";
                dd_approvalStatus.SelectedValue = "0";
                txt_cnNumber.Focus();

                txt_codAmount.Text = "";
                dd_productType.SelectedValue = "0";
                txt_orderRefNo.Text = "";
                txt_descriptionCOD.Text = "";
                Cb_CODAmount.Checked = false;
                gv_CNModifiers.DataSource = null;
                gv_CNModifiers.DataBind();
                dd_priceModifier.ClearSelection();
                txt_declaredValue.Text = "";
                txt_remarks.Text = "";
                txt_l.Text = txt_w.Text = txt_h.Text = txt_vWeight.Text = txt_aWeight.Text = "";
            }

            DataTable pm = new DataTable();
            pm.Columns.Add("priceModifierId");
            pm.Columns.Add("priceModifierName");
            pm.Columns.Add("calculatedValue");
            pm.Columns.Add("ModifiedCalculatedValue");
            pm.Columns.Add("calculationBase");
            pm.Columns.Add("isTaxable");
            pm.Columns.Add("description");
            pm.Columns.Add("SortOrder");
            pm.Columns.Add("NEW");
            pm.AcceptChanges();
            ViewState["pm"] = pm;
            gv_CNModifiers.DataSource = pm;
            gv_CNModifiers.DataBind();


            CheckFrozenThings();

        }
        protected void btn_save_Click(object sender, EventArgs e)
        {
            clvar.Branch = dd_origin.SelectedValue;
            DataTable dates = MinimumDate(clvar);
            DateTime minAllowedDate = DateTime.Parse(dates.Rows[0]["DateAllowed"].ToString()).AddDays(1);
            DateTime maxAllowedDate = DateTime.Now;
            if (picker_bookingDate.SelectedDate < minAllowedDate || picker_bookingDate.SelectedDate > maxAllowedDate || picker_reportingDate.SelectedDate < minAllowedDate || picker_reportingDate.SelectedDate > maxAllowedDate)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Day End has already been performed for selected Dates. Cannot Approve CN')", true);
                return;
            }

            txt_accountNo_TextChanged(sender, e);
            if (dd_approvalStatus.SelectedValue == "1")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('CN Already Approved. Cannot Edit. Go To Unapproval Screen to Approve this CN First')", true);
                //Errorid.Text = "Go To Unapproval Screen to Unapprove this CN First";
                lt1.Text = "CN Already Approved. Cannot Edit.<a href='unapproveConsignment.aspx'> Go To Unapproval Screen</a> to Unapprove this CN First";
                //btn_save.Enabled = false;
                return;
            }
            if (dd_serviceType.SelectedValue == "0")
            {
                Errorid.Text = "Select Service Type";
                Errorid.ForeColor = System.Drawing.Color.Red;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage()", "alert('Select Service Type')", true);
                return;
            }
            if (dd_consignmentType.SelectedValue == "0")
            {
                Errorid.Text = "Select Consignment Type";
                Errorid.ForeColor = System.Drawing.Color.Red;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage()", "alert('Select Consignment Type')", true);
                return;
            }
            if (dd_originExpressCenter.SelectedValue == "0")
            {

                Errorid.Text = "Select Origin Express Center";
                Errorid.ForeColor = System.Drawing.Color.Red;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage()", "alert('Select Origin Express Center')", true);
                return;
            }
            if (Or_Country.SelectedValue == "0")
            {
                Errorid.Text = "Select Origin Country";
                Errorid.ForeColor = System.Drawing.Color.Red;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage()", "alert('Select Origin Country')", true);
                return;
            }


            Errorid.Text = "";
            DataTable cities = ViewState["cities"] as DataTable;
            try
            {
                if (txt_accountNo.Text == "0")
                {
                    if (txt_chargedAmount.Text.Trim() == "" || txt_chargedAmount.Text.Trim() == "0")
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage()", "alert('Charged Amount Cannot be Zero or Empty')", true);
                        return;
                    }
                }
                string destination = cities.Select("expresscentercode = '" + dd_destination.SelectedValue + "'")[0]["bid"].ToString();
                if (txt_riderCode.Text.Trim() == "")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage()", "alert('Provide Rider Code')", true);
                    return;
                }

                //if (txt_accountNo.Text.Trim() != "")
                //{
                //    txt_accountNo_TextChanged(sender, e);
                //}
                if (txt_invoiceNumber.Text.Trim() != "")
                {
                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage()", "alert('Consignment Already Invoiced. Cannot Approve')", true);
                    //return;
                }
                if (txt_accountNo.Text.Trim() == "")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage()", "alert('Provide Account No.')", true);
                    return;
                }
                if (txt_weight.Text.Trim() == "0" || txt_weight.Text.Trim() == "")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Invalid Weight')", true);
                    return;
                    //picker_bookingDate.SelectedDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
                }
                clvar.Zone = HttpContext.Current.Session["ZoneCode"].ToString();

                clvar.Branch = dd_origin.SelectedValue;
                clvar.BranchName = destination;

                DataTable zonedetail = func.GetZoneByBranch(clvar);

                string FromZoneColorID = zonedetail.Select("BranchCode = '" + dd_origin.SelectedValue + "'")[0]["ColorID"].ToString();
                string ToZoneColorID = zonedetail.Select("BranchCode = '" + destination + "'")[0]["ColorID"].ToString();

                clvar.CustomerClientID = hd_CreditClientID.Value;
                clvar.ServiceTypeName = dd_serviceType.SelectedItem.Text;
                DataTable dt = new DataTable();
                DataTable mainTariff = func.GetClientTariffForConsignmentApproval(clvar);
                if (clvar.ServiceTypeName != "Road n Rail" && clvar.ServiceTypeName != "Express Cargo")
                {

                    if (destination == dd_origin.SelectedValue)
                    {
                        try
                        {
                            dt = mainTariff.Select("TOZONECODE = '17' AND CHKDeleted = 'False'").CopyToDataTable();
                        }
                        catch (Exception ex)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage()", "alert('No Tariff available for Selected Customer')", true);
                            return;
                        }

                    }
                    else if (dd_origin.SelectedValue != destination)
                    {
                        try
                        {
                            dt = mainTariff.Select("TOZONECODE = '" + zonedetail.Select("BranchCode = '" + destination + "'")[0]["ZoneCode"].ToString() + "' AND CHKDeleted = 'False'").CopyToDataTable();
                        }
                        catch (Exception ex)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage()", "alert('No Tariff available for Selected Customer')", true);
                            return;
                        }
                    }
                }
                else
                {
                    dt = func.RNRTarrif(clvar);
                }


                if (dt != null)
                {
                    double amount = 0;
                    double gst = 0;

                    if (dt.Rows.Count > 0)
                    {
                        if (clvar.ServiceTypeName != "Road n Rail")
                        {
                            #region OTHER THAN ROADnRAIL CHAY PAI

                            double additionalFactor = 0;// double.Parse(dt.Rows[0]["AdditionalFactor"].ToString());
                            double additionalPrice_ = 0;
                            double weight = double.Parse(txt_weight.Text);
                            double maxWeight = double.Parse(dt.Compute("MAX(toWeight)", "").ToString());
                            double maxPrice = double.Parse(dt.Compute("MAX(Price)", "").ToString());
                            if (weight > maxWeight)
                            {
                                if (dd_origin.SelectedValue == destination)
                                {
                                    try
                                    {
                                        double.TryParse(mainTariff.Select("TOZONECODE = 'LOCAL' AND CHKDeleted = 'False'").CopyToDataTable().Rows[0]["AdditionalFactor"].ToString(), out additionalFactor);
                                        double.TryParse(mainTariff.Select("TOZONECODE = 'LOCAL' AND CHKDeleted = 'False'").CopyToDataTable().Rows[0]["PRICE"].ToString(), out additionalPrice_);
                                    }
                                    catch (Exception ex)
                                    {
                                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Local Zone Tariff Not Available')", true);
                                        return;
                                    }
                                }
                                else
                                {
                                    if (FromZoneColorID == ToZoneColorID)
                                    {
                                        try
                                        {
                                            double.TryParse(mainTariff.Select("TOZONECODE = 'SAME' AND CHKDeleted = 'False'").CopyToDataTable().Rows[0]["AdditionalFactor"].ToString(), out additionalFactor);
                                            double.TryParse(mainTariff.Select("TOZONECODE = 'SAME' AND CHKDeleted = 'False'").CopyToDataTable().Rows[0]["PRICE"].ToString(), out additionalPrice_);
                                        }
                                        catch (Exception ex)
                                        {
                                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('SAME Zone Tariff Not Available')", true);
                                            return;
                                        }

                                    }
                                    else
                                    {
                                        try
                                        {
                                            double.TryParse(mainTariff.Select("TOZONECODE = 'DIFF' AND CHKDeleted = 'False'").CopyToDataTable().Rows[0]["AdditionalFactor"].ToString(), out additionalFactor);
                                            double.TryParse(mainTariff.Select("TOZONECODE = 'DIFF' AND CHKDeleted = 'False'").CopyToDataTable().Rows[0]["PRICE"].ToString(), out additionalPrice_);
                                        }
                                        catch (Exception ex)
                                        {
                                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Diff Zone Tariff Not Available')", true);
                                            return;
                                        }
                                    }
                                }
                            }

                            double remainingWeight = 0;

                            if (weight > maxWeight)
                            {
                                remainingWeight = weight - maxWeight;
                                amount = maxPrice;
                                int count = 0;
                                try
                                {
                                    count = Convert.ToInt16(remainingWeight / additionalFactor);
                                }
                                catch (Exception ex)
                                {
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage()", "alert('No Tariff available for Selected Customer')", true);
                                    return;
                                }
                                amount += Math.Round(remainingWeight / additionalFactor) * additionalPrice_;
                                //amount += count * additionalFactor;//needs to be changed after discussion
                            }
                            else if (weight == maxWeight)
                            {
                                amount = maxPrice;
                            }
                            else
                            {
                                DataRow[] dr = dt.Select("FromWeight < '" + weight + "' AND ToWeight >= '" + weight + "'");
                                amount = double.Parse(dr[0]["Price"].ToString());
                            }
                            #endregion

                            #region HAND CARRY SCENARIO
                            if (dd_consignmentType.SelectedValue == "13" && clvar.ServiceTypeName != "Road n Rail")
                            {
                                clvar.CustomerClientID = hd_CreditClientID.Value;
                                clvar.ServiceTypeName = "hand carry";
                                dt = new DataTable();
                                mainTariff = func.GetClientTariffForConsignmentApproval(clvar);

                                if (destination == dd_origin.SelectedValue)
                                {
                                    try
                                    {
                                        dt = mainTariff.Select("TOZONECODE = '17'").CopyToDataTable();
                                    }
                                    catch (Exception ex)
                                    {
                                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage()", "alert('No Tariff available for Selected Customer')", true);
                                        return;
                                    }

                                }
                                else if (dd_origin.SelectedValue != destination)
                                {
                                    try
                                    {
                                        dt = mainTariff.Select("TOZONECODE = '" + zonedetail.Select("BranchCode = '" + destination + "'")[0]["ZoneCode"].ToString() + "' AND CHKDeleted = 'False'").CopyToDataTable();
                                    }
                                    catch (Exception ex)
                                    {
                                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage()", "alert('No Tariff available for Selected Customer')", true);
                                        return;
                                    }
                                }
                                if (dt.Rows.Count > 0)
                                {
                                    weight = double.Parse(txt_weight.Text);
                                    maxWeight = double.Parse(dt.Compute("MAX(toWeight)", "").ToString());
                                    double additionalPrice = 0;
                                    if (weight > maxWeight)
                                    {
                                        if (dd_origin.SelectedValue == destination)
                                        {
                                            double.TryParse(mainTariff.Select("TOZONECODE = 'LOCAL' AND CHKDeleted = 'False'").CopyToDataTable().Rows[0]["AdditionalFactor"].ToString(), out additionalFactor);
                                            double.TryParse(mainTariff.Select("TOZONECODE = 'LOCAL' AND CHKDeleted = 'False'").CopyToDataTable().Rows[0]["Price"].ToString(), out additionalPrice);
                                        }
                                        else
                                        {
                                            if (FromZoneColorID == ToZoneColorID)
                                            {
                                                double.TryParse(mainTariff.Select("TOZONECODE = 'SAME' AND CHKDeleted = 'False'").CopyToDataTable().Rows[0]["AdditionalFactor"].ToString(), out additionalFactor);
                                                double.TryParse(mainTariff.Select("TOZONECODE = 'SAME' AND CHKDeleted = 'False'").CopyToDataTable().Rows[0]["Price"].ToString(), out additionalPrice);
                                            }
                                            else
                                            {
                                                double.TryParse(mainTariff.Select("TOZONECODE = 'DIFF' AND CHKDeleted = 'False'").CopyToDataTable().Rows[0]["AdditionalFactor"].ToString(), out additionalFactor);
                                                double.TryParse(mainTariff.Select("TOZONECODE = 'DIFF' AND CHKDeleted = 'False'").CopyToDataTable().Rows[0]["Price"].ToString(), out additionalPrice);
                                            }
                                        }


                                        maxPrice = double.Parse(dt.Compute("MAX(Price)", "").ToString());
                                    }

                                    //additionalFactor = 0;// double.Parse(dt.Rows[0]["AdditionalFactor"].ToString());



                                    remainingWeight = 0;
                                    gst = 0;
                                    if (weight > maxWeight)
                                    {
                                        remainingWeight = weight - maxWeight;
                                        amount += maxPrice;
                                        int count = 0;
                                        try
                                        {
                                            count = Convert.ToInt16(remainingWeight / additionalFactor);
                                        }
                                        catch (Exception ex)
                                        {
                                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage()", "alert('No Tariff available for Selected Customer')", true);
                                            return;
                                        }
                                        amount += Math.Round(remainingWeight / additionalFactor) * additionalPrice;
                                        //amount += count * additionalFactor;//needs to be changed after discussion
                                    }
                                    else if (weight == maxWeight)
                                    {
                                        amount += maxPrice;
                                    }
                                    else
                                    {
                                        DataRow[] dr = dt.Select("FromWeight < '" + weight + "' AND ToWeight >= '" + weight + "'");
                                        amount += double.Parse(dr[0]["Price"].ToString());
                                    }
                                }
                                else
                                {
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage()", "alert('No Tariff available for Selected Customer')", true);
                                    return;
                                }
                            }
                            #endregion

                        }
                        else
                        {
                            #region ROADnRAIL CHAI PAI

                            string fromCat = "";
                            string toCat = "";
                            string destEC = "";
                            DataTable tempCities = func.Cities_();
                            //                               tempCities.Select("bid = '" + dd_origin.SelectedValue + "'").AsEnumerable().Max(row => row["column_Name"]) ;
                            fromCat = tempCities.Select("expressCenterCode = '" + dd_originExpressCenter.SelectedValue + "'", "CategoryID DESC")[0]["CategoryID"].ToString();
                            toCat = tempCities.Select("expressCenterCode = '" + dd_destination.SelectedValue + "'")[0]["CategoryID"].ToString();


                            DataRow[] dr = dt.Select("FromCatID='" + fromCat + "' and ToCatID='" + toCat + "'");

                            if (dr.Length > 0)
                            {
                                Double w = float.Parse(txt_weight.Text);
                                if (w < 5)
                                {
                                    amount = 5 * float.Parse(dr[0]["value"].ToString());
                                }
                                else
                                {
                                    amount = Math.Ceiling(float.Parse(txt_weight.Text)) * float.Parse(dr[0]["value"].ToString());
                                }
                            }
                            else
                            {
                                dt = func.RNRTarrifDefault(clvar);
                                dr = dt.Select("FromCatID='" + fromCat + "' and ToCatID='" + toCat + "'");
                                if (dr.Length > 0)
                                {
                                    Double w = float.Parse(txt_weight.Text);
                                    if (w < 5)
                                    {
                                        amount = 5 * float.Parse(dr[0]["value"].ToString());
                                    }
                                    else
                                    {
                                        amount = Math.Ceiling(float.Parse(txt_weight.Text)) * float.Parse(dr[0]["value"].ToString());
                                    }
                                }
                                else
                                {
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('RnR Tariff Not Available')", true);
                                    return;
                                }

                            }


                            #endregion
                        }

                        clvar.origin = dd_origin.SelectedValue;

                        DataTable branchGst = con.BranchGSTInformation(clvar).Tables[0];
                        double.TryParse(branchGst.Rows[0]["GST"].ToString(), out gst);
                        ViewState["totalAmount"] = amount;
                        foreach (GridViewRow row in gv_CNModifiers.Rows)
                        {
                            if (row.Cells[3].Text == "1")
                            {
                                amount += double.Parse((row.FindControl("txt_gValue") as TextBox).Text);
                            }
                            else
                            {
                                amount += amount * (double.Parse((row.FindControl("txt_gValue") as TextBox).Text) / 100);
                            }
                        }
                        ViewState["totalGST"] = amount * (double.Parse(branchGst.Rows[0]["GST"].ToString()) / 100);
                        amount += amount * (double.Parse(branchGst.Rows[0]["GST"].ToString()) / 100);

                        //txt_chargedAmount.Text = Math.Round(amount, 2).ToString();

                        ViewState["chargedAmount"] = txt_chargedAmount.Text;

                        ViewState["gst"] = branchGst.Rows[0]["GST"].ToString();
                        //lbl_error.Text = "Total Amount: " + Math.Round(amount, 2).ToString() + "\nPress OK to Continue or Cancel to Cancel";
                        //divDialogue.Style.Add("display", "block");

                        // WAQAS BHAI K KEHNAY PER KIA HAI YEH SARA
                        clvar.Bookingdate = DateTime.Parse(picker_bookingDate.SelectedDate.ToString()); //.t;
                        clvar.consignmentNo = txt_cnNumber.Text;
                        clvar.AccountNo = txt_accountNo.Text;
                        clvar.origin = HttpContext.Current.Session["BranchCode"].ToString();//dd_origin.SelectedValue;
                        clvar.serviceTypeId = dd_serviceType.SelectedValue;
                        clvar.ServiceTypeName = dd_serviceType.SelectedItem.Text;
                        clvar.Consigner = txt_consigner.Text;

                        clvar.Consignee = txt_consignee.Text;
                        //clvar.destination = dd_destination.SelectedValue;
                        clvar.Weight = (float)Math.Round(float.Parse(txt_weight.Text), 1);
                        clvar.riderCode = txt_riderCode.Text;
                        clvar.OriginExpressCenterCode = dd_originExpressCenter.SelectedValue;
                        clvar.destination = cities.Select("expresscenterCode = '" + dd_destination.SelectedValue + "'")[0]["bid"].ToString();
                        clvar.destinationExpressCenterCode = dd_destination.SelectedValue;
                        clvar.cnTypeId = dd_consignmentType.SelectedValue;
                        clvar.pieces = 1;
                        clvar.CouponNo = txt_coupon.Text;
                        if (txt_accountNo.Text == "0")
                        {
                            clvar.Customertype = 1;
                        }
                        else
                        {
                            clvar.Customertype = 2;//int.Parse(hd_customerType.Value);

                        }
                        double tempTotalAmount = 0;
                        double.TryParse(txt_chargedAmount.Text, out tempTotalAmount);
                        clvar.TotalAmount = amount;
                        clvar.consignerAccountNo = txt_accountNo.Text;
                        //clvar.TotalAmount = amount;
                        clvar.gst = double.Parse(ViewState["totalGST"].ToString());
                        if (hd_customerType.Value == "1")
                        {
                            clvar.ChargeAmount = double.Parse(txt_chargedAmount.Text);
                        }
                        else
                        {
                            clvar.ChargeAmount = clvar.TotalAmount + clvar.gst;
                        }


                        clvar.status = 1;// int.Parse(dd_approvalStatus.SelectedValue);
                        clvar.CheckCondition = ViewState["gst"].ToString();
                        clvar.CustomerClientID = hd_CreditClientID.Value;
                        clvar.LoadingDate = picker_reportingDate.SelectedDate.Value.ToString("yyyy-MM-dd");
                        DataTable pm = new DataTable();
                        pm.Columns.Add("priceModifierId");
                        pm.Columns.Add("priceModifierName");
                        pm.Columns.Add("calculatedValue");
                        pm.Columns.Add("calculationBase");
                        pm.Columns.Add("isTaxable");
                        pm.Columns.Add("description");
                        pm.Columns.Add("NEW");
                        pm.Columns.Add("SortOrder");
                        pm.AcceptChanges();
                        if (gv_CNModifiers.Rows.Count > 0)
                        {
                            foreach (GridViewRow row in gv_CNModifiers.Rows)
                            {
                                DataRow dr = pm.NewRow();
                                dr["priceModifierId"] = (row.FindControl("btn_gRemove") as Button).CommandArgument.ToString();
                                dr["priceModifierName"] = row.Cells[1].Text;
                                dr["calculatedValue"] = (row.FindControl("txt_gValue") as TextBox).Text;
                                dr["calculationBase"] = row.Cells[3].Text;
                                dr["isTaxable"] = row.Cells[4].Text;
                                dr["description"] = row.Cells[5].Text;
                                dr["NEW"] = (row.FindControl("hd_new") as HiddenField).Value;
                                dr["SortOrder"] = (row.FindControl("hd_sortOrder") as HiddenField).Value;
                                pm.Rows.Add(dr);
                                pm.AcceptChanges();
                            }
                        }


                        clvar.CatID = ViewState["isAPIClient"].ToString();
                        clvar.Consigner = clvar.Consigner.Replace("'", "''");

                        if (ViewState["CNState"].ToString() == "NEW")
                        {



                            clvar.Bookingdate = picker_bookingDate.SelectedDate.Value;
                            clvar.isCod = bool.Parse(ViewState["ISCOD"].ToString());
                            bool cod = false;
                            if (ViewState["ISCOD"].ToString().ToLower() == "true")
                            {

                                if (txt_orderRefNo.Text.Trim() != "" || clvar.CatID == "1")
                                {
                                    string f = "";
                                    cod = true;
                                    clvar.orderRefNo = txt_orderRefNo.Text;
                                    clvar.chargeCODAmount = true;

                                    clvar.productTypeId = int.Parse(dd_productType.SelectedValue);
                                    float tempCodAmount = 0;
                                    float.TryParse(txt_codAmount.Text, out tempCodAmount);
                                    if (tempCodAmount == 0 && clvar.CatID != "1")
                                    {
                                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('COD Amount Cannot be 0')", true);
                                        return;
                                    }
                                    clvar.codAmount = tempCodAmount; //float.Parse(txt_codAmount.Text);
                                    clvar.cnScreenId = "0";
                                    clvar.status = 1;
                                    f = InsertConsignmentFromApprovalScreen(clvar, pm, cod);
                                    //if (clvar.ServiceTypeName != "Road n Rail")
                                    //{
                                    //    con.AddApprovalComputation(clvar);
                                    //}
                                    //else
                                    //{
                                    //    con.AddApprovalRNRComputation(clvar);
                                    //}

                                    if (f == "OK")
                                    {
                                        //btn_reset_Click(sender, e);
                                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Consignment Saved')", true);
                                        if (chk_bulkUpdate.Checked)
                                        {
                                            clvar.ClvarListStr = ViewState["bulkRange"] as List<string>;
                                            clvar.ClvarListStr.RemoveAt(0);
                                            if (clvar.ClvarListStr.Count > 0)
                                            {
                                                txt_cnNumber.Text = clvar.ClvarListStr[0].ToString();
                                                txt_cnNumber_TextChanged(sender, e);
                                            }
                                            else
                                            {
                                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Bulk Consignment Saved')", true);
                                            }

                                        }
                                        else
                                        {
                                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Consignment Saved')", true);
                                            btn_reset_Click(sender, e);
                                        }


                                        return;
                                    }
                                    else
                                    {
                                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Could not save Consignment\\nError: " + f + "')", true);
                                        return;
                                    }
                                }
                                else
                                {
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Provide COD Order Number')", true);
                                    return;
                                }


                            }
                            else
                            {
                                string f = "";
                                cod = false;
                                clvar.cnScreenId = "0";
                                clvar.status = 1;
                                f = InsertConsignmentFromApprovalScreen(clvar, pm, cod);
                                if (f == "OK")
                                {
                                    //if (clvar.ServiceTypeName != "Road n Rail")
                                    //{
                                    //    con.AddApprovalComputation(clvar);
                                    //}
                                    //else
                                    //{
                                    //    con.AddApprovalRNRComputation(clvar);
                                    //}
                                    if (chk_bulkUpdate.Checked)
                                    {
                                        clvar.ClvarListStr = ViewState["bulkRange"] as List<string>;
                                        clvar.ClvarListStr.RemoveAt(0);
                                        if (clvar.ClvarListStr.Count > 0)
                                        {
                                            txt_cnNumber.Text = clvar.ClvarListStr[0].ToString();
                                            txt_cnNumber_TextChanged(sender, e);
                                        }
                                        else
                                        {
                                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Bulk Consignment Saved')", true);
                                            btn_reset_Click(sender, e);
                                        }
                                    }
                                    else
                                    {
                                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Consignment Saved')", true);
                                        btn_reset_Click(sender, e);
                                    }

                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Consignment Saved')", true);
                                    txt_cnNumber.Focus();
                                    return;
                                }
                                else
                                {
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Could not save Consignment\\nError: " + f + "')", true);
                                    return;
                                }
                            }
                        }
                        else
                        {
                            bool cod;

                            //if (txt_codAmount.Text.Trim() != "")
                            //{
                            //    cod = true;
                            //    clvar.isCod = true;

                            //    clvar.orderRefNo = txt_orderRefNo.Text;

                            //    //clvar.productTypeId = int.Parse(dd_productType.SelectedValue);
                            //    clvar.productDescription = txt_descriptionCOD.Text;
                            //    float tempCodAmount = 0;
                            //    float.TryParse(txt_codAmount.Text, out tempCodAmount);
                            //    if (tempCodAmount == 0)
                            //    {
                            //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('COD Amount Cannot be 0')", true);
                            //        return;
                            //    }
                            //    clvar.codAmount = float.Parse(txt_codAmount.Text);

                            //    //if (clvar.status == 0)
                            //    //{

                            //    //}
                            //}
                            //else
                            //{
                            //    cod = false;
                            //    clvar.isCod = false;
                            //}


                            //clvar.isCod = bool.Parse(ViewState["ISCOD"].ToString());
                            if (hd_COD.Value == "1" || hd_COD.Value.ToUpper() == "TRUE")
                            {
                                cod = true;
                                clvar.isCod = true;
                                if (clvar.CatID != "1")
                                {


                                    clvar.orderRefNo = txt_orderRefNo.Text;

                                    //clvar.productTypeId = int.Parse(dd_productType.SelectedValue);
                                    clvar.productDescription = txt_descriptionCOD.Text;
                                    float tempCodAmount = 0;
                                    float.TryParse(txt_codAmount.Text, out tempCodAmount);
                                    if (tempCodAmount == 0)
                                    {
                                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('COD Amount Cannot be 0')", true);
                                        return;
                                    }
                                    clvar.codAmount = float.Parse(txt_codAmount.Text);
                                }

                            }
                            else
                            {
                                cod = false;
                                clvar.isCod = false;
                            }
                            clvar.status = 1;
                            string f = ApproveDomesticConsignment(clvar, pm, cod);
                            if (f == "OK")
                            {
                                //if (clvar.ServiceTypeName != "Road n Rail")
                                //{
                                //    con.AddApprovalComputation(clvar);
                                //}
                                //else
                                //{
                                //    con.AddApprovalRNRComputation(clvar);
                                //}
                                if (chk_bulkUpdate.Checked)
                                {
                                    clvar.ClvarListStr = ViewState["bulkRange"] as List<string>;
                                    clvar.ClvarListStr.RemoveAt(0);
                                    if (clvar.ClvarListStr.Count > 0)
                                    {
                                        txt_cnNumber.Text = clvar.ClvarListStr[0].ToString();
                                        txt_cnNumber_TextChanged(sender, e);
                                    }
                                    else
                                    {
                                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Bulk Consignment Saved')", true);
                                        btn_reset_Click(sender, e);
                                        txt_cnNumber.Focus();
                                    }
                                }
                                else
                                {
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Consignment Saved')", true);
                                    btn_reset_Click(sender, e);
                                    txt_cnNumber.Focus();
                                }
                                return;
                            }
                            else
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Consignment Not Saved\\nError = " + f + "')", true);
                                //btn_reset_Click(sender, e);
                            }







                        }


                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage()", "alert('No Tariff available for Selected Customer')", true);
                        return;
                    }

                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage()", "alert('No Tariff available for Selected Customer')", true);
                    return;
                }


            }
            catch (Exception ex)
            {

                Errorid.Text = ex.Message;
            }

        }
        protected void btn_add_Click_(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)ViewState["PM"];
            string PM = dd_priceModifier.SelectedValue;
            DataRow[] dr = dt.Select(" priceModifierId ='" + dd_priceModifier.SelectedItem.Value + "'");
            clvar.origin = dd_origin.SelectedValue;
            DataSet BranchGSTInformation = con.BranchGSTInformation(clvar);
            string gst = "";
            if (BranchGSTInformation.Tables[0].Rows.Count != 0)
            {
                gst = BranchGSTInformation.Tables[0].Rows[0]["gst"].ToString();
            }
            if (dr.Length == 0)
            {
                if (dd_priceModifier.SelectedItem.Text.ToUpper() != "EXTRA CHARGES")
                {

                    DataTable dt_ = (DataTable)ViewState["PM_"];
                    //   DataRow   dr = dt.NewRow()
                    dr = dt_.Select(" id ='" + dd_priceModifier.SelectedValue.Split('-')[0] + "'");

                    DataRow dr_ = dt.NewRow();

                    dr_["pricemodifierid"] = int.Parse(dd_priceModifier.SelectedItem.Value.Split('-')[0]);
                    dr_["ConsignmentNumber"] = Int64.Parse(txt_cnNumber.Text);
                    dr_["CreatedBy"] = "TEST";
                    dr_["CreatedOn"] = DateTime.Now;
                    dr_["ModifiedBy"] = "";
                    dr_["ModifiedOn"] = DBNull.Value;
                    dr_["CalculatedValue"] = float.Parse(dr[0]["calculationValue"].ToString()) * (double.Parse(txt_chargedAmount.Text) / 100); //float.Parse(pm.Select("id = '" + dr["id"].ToString() + "'")[0]["ActualValue"].ToString());
                    dr_["ModifiedCalculationValue"] = float.Parse(txt_priceModifierValue.Text);//float.Parse(pm.Select("id = '" + dr["id"].ToString() + "'")[0]["CalculationValue"].ToString());
                    dr_["CalculatedGst"] = ((float.Parse(dr[0]["calculationValue"].ToString()) * (double.Parse(txt_chargedAmount.Text) / 100)) / 100) * float.Parse(gst);
                    dr_["CalculationBase"] = dr[0]["calculationbase"].ToString();//int.Parse("1");
                    dr_["isTaxable"] = 1;
                    dr_["SortOrder"] = dt.Rows.Count + 1;

                    dt.Rows.Add(dr_);
                    dt.AcceptChanges();

                    ViewState["PM"] = dt;
                }
                else
                {
                    DataTable dt_ = (DataTable)ViewState["PM_"];
                    //   DataRow   dr = dt.NewRow()
                    dr = dt_.Select(" id ='" + dd_priceModifier.SelectedValue.Split('-')[0] + "'");

                    DataRow dr_ = dt.NewRow();

                    dr_["pricemodifierid"] = int.Parse(dd_priceModifier.SelectedItem.Value.Split('-')[0]);
                    dr_["ConsignmentNumber"] = Int64.Parse(txt_cnNumber.Text);
                    dr_["CreatedBy"] = "TEST";
                    dr_["CreatedOn"] = DateTime.Now;
                    dr_["ModifiedBy"] = "";
                    dr_["ModifiedOn"] = DBNull.Value;
                    dr_["CalculatedValue"] = float.Parse(dr[0]["calculationValue"].ToString()) + (double.Parse(txt_chargedAmount.Text)); //float.Parse(pm.Select("id = '" + dr["id"].ToString() + "'")[0]["ActualValue"].ToString());
                    dr_["ModifiedCalculationValue"] = float.Parse(txt_priceModifierValue.Text);//float.Parse(pm.Select("id = '" + dr["id"].ToString() + "'")[0]["CalculationValue"].ToString());
                    dr_["CalculatedGst"] = ((float.Parse(dr[0]["calculationValue"].ToString()) + (double.Parse(txt_chargedAmount.Text) / 100)) / 100) * float.Parse(gst);
                    dr_["CalculationBase"] = dr[0]["calculationbase"].ToString();//int.Parse("1");
                    dr_["isTaxable"] = 1;
                    dr_["SortOrder"] = dt.Rows.Count + 1;

                    dt.Rows.Add(dr_);
                    dt.AcceptChanges();

                    ViewState["PM"] = dt;

                }
                LoadGrid();
                //Total();
            }

        }
        protected void btn_add_Click(object sender, EventArgs e)
        {
            Int64 tempDeclaredValue = 0;
            Int64.TryParse(txt_declaredValue.Text.Trim(), out tempDeclaredValue);
            if (dd_calculationBase.SelectedValue == "3" && (txt_declaredValue.Text.Trim() == "" || txt_declaredValue.Text.Trim() == "0" || tempDeclaredValue <= 0))
            {
                AlertMessage("Please Provide Proper Declared Value.");

                return;
            }
            else if (dd_calculationBase.SelectedValue == "3" && tempDeclaredValue < 2000)
            {
                AlertMessage("Declared Value must be greater than 2,000");
                return;
            }
            else if (dd_calculationBase.SelectedValue == "3" && tempDeclaredValue > 200000)
            {
                AlertMessage("Declared Value must be lesser than 200,000");
                return;
            }
            DataTable pm = ViewState["PriceModifiers"] as DataTable;
            DataTable dtSurcharges = ViewState["pm"] as DataTable;

            if (dtSurcharges.Select("priceModifierId='" + dd_priceModifier.SelectedValue.Split('-')[0] + "' and new <> 'REMOVED'").Count() > 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cannot Add Double Surcharges')", true);
                return;
            }
            //pm.Columns.Add("priceModifierId");
            //pm.Columns.Add("priceModifierName");
            //pm.Columns.Add("calculatedValue");
            //pm.Columns.Add("calculationBase");
            //pm.Columns.Add("isTaxable");
            //pm.Columns.Add("description");

            int sortOrder = 0;
            int.TryParse(dtSurcharges.Compute("MAX(SORTORDER)", "").ToString(), out sortOrder);
            DataRow dr = dtSurcharges.NewRow();
            dr["priceModifierId"] = dd_priceModifier.SelectedValue.Split('-')[0];
            dr["calculationBase"] = dd_priceModifier.SelectedValue.Split('-')[1];
            dr["priceModifierName"] = dd_priceModifier.SelectedItem.Text;
            dr["calculatedValue"] = dd_priceModifier.SelectedValue.Split('-')[2];
            dr["ModifiedCalculatedValue"] = dd_priceModifier.SelectedValue.Split('-')[2];
            dr["isTaxable"] = dd_priceModifier.SelectedValue.Split('-')[3];
            //dr["calculatedValue"] = dd_priceModifier.SelectedValue.Split('-')[2];
            dr["description"] = (pm.Select("id = '" + dd_priceModifier.SelectedValue.Split('-')[0] + "'"))[0]["Description"].ToString();//txt_description.Text;
            dr["NEW"] = "1";
            dr["SortOrder"] = sortOrder + 1; //(int.TryParse(dtSurcharges.Compute("MAX(SORTORDER)", "").ToString()) + 1).ToString();
            dr["AlternateValue"] = txt_declaredValue.Text;
            dtSurcharges.Rows.Add(dr);

            gv_CNModifiers.DataSource = dtSurcharges;
            gv_CNModifiers.DataBind();
            ViewState["pm"] = dtSurcharges;
        }
        public void LoadGrid()
        {
            DataTable dt = (DataTable)ViewState["PM"];
            if (dt.Rows.Count > 0)
            {
                gv_CNModifiers.DataSource = dt.DefaultView;
                gv_CNModifiers.DataBind();
            }
            else
            {
                gv_CNModifiers.DataSource = null;
            }
            //Total();
        }
        protected void dd_priceModifier_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable fuelSurcharges = ViewState["PriceModifiers"] as DataTable;
            if (dd_priceModifier.SelectedValue != "0")
            {
                dd_calculationBase.SelectedValue = dd_priceModifier.SelectedValue.Split('-')[1];
                txt_priceModifierValue.Text = dd_priceModifier.SelectedValue.Split('-')[2];
                if (dd_calculationBase.SelectedValue == "3")
                {
                    txt_declaredValue.Enabled = true;
                }
                else
                {
                    txt_declaredValue.Enabled = false;
                    txt_declaredValue.Text = "";
                }
            }
            else
            {
                txt_declaredValue.Text = "";
                txt_declaredValue.Enabled = false;
            }
            dd_priceModifier.Focus();

        }
        protected void txt_accountNo_TextChanged(object sender, EventArgs e)
        {
            if (txt_accountNo.Text != "")
            {
                if (txt_accountNo.Text != "")
                {
                    clvar.Branch = dd_origin.SelectedValue;
                    clvar.AccountNo = txt_accountNo.Text;
                    DataSet ds = CustomerInformation(clvar);
                    if (ds.Tables.Count != 0)
                    {
                        if (ds.Tables[0].Rows.Count != 0)
                        {
                            if (txt_accountNo.Text != "0")
                            {
                                txt_consigner.Text = ds.Tables[0].Rows[0]["Name"].ToString();

                                clvar.CustomerClientID = ds.Tables[0].Rows[0]["id"].ToString();
                                hd_CreditClientID.Value = ds.Tables[0].Rows[0]["id"].ToString();
                                if (clvar.CustomerClientID != "")
                                {
                                    Get_ProductType();
                                }
                                //   clvar.COD = ds.Tables[0].Rows[0]["iscod"].ToString();
                                txt_chargedAmount.Enabled = false;
                            }
                            else
                            {
                                txt_chargedAmount.Enabled = true;
                            }

                            ViewState["ISCOD"] = "1";
                            hd_CreditClientID.Value = ds.Tables[0].Rows[0]["id"].ToString();
                            clvar.CatID = ds.Tables[0].Rows[0]["isAPIClient"].ToString();
                            ViewState["isAPIClient"] = ds.Tables[0].Rows[0]["isAPIClient"].ToString();
                            if (clvar.CatID == "1")
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('This COD Account Cannot be used.')", true);
                                hd_unApproveable.Value = "1";
                                txt_accountNo.Text = "";
                                return;
                            }
                            else
                            {

                            }

                            hd_COD.Value = ds.Tables[0].Rows[0]["ISCOD"].ToString();

                            hd_CreditClientID.Value = ds.Tables[0].Rows[0]["id"].ToString();

                            ViewState["CODType"] = ds.Tables[0].Rows[0]["CODTYPE"].ToString();
                            if (ds.Tables[0].Rows[0]["ISCOD"].ToString().ToLower() == "true")
                            {
                                Cb_CODAmount.Checked = true;
                                cb_checkbox.Enabled = true;

                            }
                            else
                            {
                                Cb_CODAmount.Checked = false;
                                //codTable.Visible = false;
                                cb_checkbox.Enabled = false;

                            }
                            if (ds.Tables[0].Rows[0]["accountNo"].ToString() == "0")
                            {
                                hd_customerType.Value = "1";
                            }
                            else
                            {
                                hd_customerType.Value = "2";
                            }

                            if (txt_riderCode.Text.Trim() != "")
                            {
                                txt_riderCode_TextChanged(this, e);
                            }

                            txt_accountNo.Focus();

                        }
                        else
                        {
                            ViewState["isAPIClient"] = "0";
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Invalid Account Number')", true);
                            txt_accountNo.Text = "";
                            txt_accountNo.Focus();
                        }
                    }
                    else
                    {
                        ViewState["isAPIClient"] = "0";
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Invalid Account Number')", true);
                        txt_accountNo.Text = "";
                        txt_accountNo.Focus();
                    }
                }
            }

            if (txt_accountNo.Text.Trim(' ') == "0")
            {
                ViewState["isAPIClient"] = "0";
                //txt_consigner.Text = "";//ds.Tables[0].Rows[0]["Name"].ToString
            }
        }
        protected void txt_accountNo_TextChanged_Save(object sender, EventArgs e)
        {
            if (txt_accountNo.Text != "")
            {
                if (txt_accountNo.Text != "")
                {
                    //DataTable codSequence = ViewState["codSequence"] as DataTable;
                    ////DataRow[] dr = codSequence.Select("accountNo = '" + txt_accountNo.Text.Trim() + "'");


                    ////codSequence.Rows.Remove(dr[0]);
                    //foreach (DataRow dr in codSequence.Rows)
                    //{
                    //    Int64 start = Int64.Parse(dr["sequenceStart"].ToString());
                    //    Int64 end = Int64.Parse(dr["sequenceEnd"].ToString());
                    //    Int64 num = Int64.Parse(txt_cnNumber.Text);
                    //    if (start <= num && end >= num && dr["accountNo"].ToString().ToUpper() != txt_accountNo.Text.Trim().ToUpper())
                    //    {
                    //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('This Account Cannot be used with this CN Number.')", true);
                    //        txt_accountNo.Text = "";
                    //        txt_accountNo.Focus();
                    //        return;
                    //    }
                    //}

                    clvar.Branch = dd_origin.SelectedValue;
                    clvar.AccountNo = txt_accountNo.Text;
                    DataSet ds = CustomerInformation(clvar);
                    if (ds.Tables.Count != 0)
                    {
                        if (ds.Tables[0].Rows.Count != 0)
                        {
                            if (txt_accountNo.Text != "0")
                            {
                                txt_consigner.Text = ds.Tables[0].Rows[0]["Name"].ToString();

                                clvar.CustomerClientID = ds.Tables[0].Rows[0]["id"].ToString();
                                hd_CreditClientID.Value = ds.Tables[0].Rows[0]["id"].ToString();
                                if (clvar.CustomerClientID != "")
                                {
                                    Get_ProductType();
                                }
                                //   clvar.COD = ds.Tables[0].Rows[0]["iscod"].ToString();
                                txt_chargedAmount.Enabled = false;
                            }
                            else
                            {
                                txt_chargedAmount.Enabled = true;
                            }

                            ViewState["ISCOD"] = ds.Tables[0].Rows[0]["ISCOD"].ToString();
                            hd_CreditClientID.Value = ds.Tables[0].Rows[0]["id"].ToString();
                            clvar.CatID = ds.Tables[0].Rows[0]["isAPIClient"].ToString();
                            ViewState["isAPIClient"] = ds.Tables[0].Rows[0]["isAPIClient"].ToString();
                            if (clvar.CatID == "1")
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('This COD Account Cannot be used.')", true);
                                hd_unApproveable.Value = "1";
                                txt_accountNo.Text = "";
                                return;
                            }
                            else
                            {

                            }

                            hd_COD.Value = ds.Tables[0].Rows[0]["ISCOD"].ToString();

                            hd_CreditClientID.Value = ds.Tables[0].Rows[0]["id"].ToString();

                            ViewState["CODType"] = ds.Tables[0].Rows[0]["CODTYPE"].ToString();
                            if (ds.Tables[0].Rows[0]["ISCOD"].ToString().ToLower() == "true")
                            {
                                Cb_CODAmount.Checked = true;
                                //codTable.Visible = true;
                                //clvar.CustomerClientID = ds.Tables[0].Rows[0]["id"].ToString();
                                //DataSet ds_Product = con.ProductTypeInfo(clvar);
                                //if (ds_Product.Tables[0].Rows.Count != 0)
                                //{
                                //    //dd_productType.Items.Add(new RadComboBoxItem("Select Product", "0"));
                                //    dd_productType.Items.Add(new ListItem("SELECT PRODUCT", "0"));
                                //    dd_productType.DataTextField = "ProductTypeName";
                                //    dd_productType.DataValueField = "Producttypecode";
                                //    dd_productType.DataSource = ds_Product.Tables[0].DefaultView;
                                //    dd_productType.DataBind();
                                //}
                                //else
                                //{
                                //    dd_productType.Items.Clear();
                                //}
                            }
                            else
                            {
                                Cb_CODAmount.Checked = false;
                                //codTable.Visible = false;
                            }
                            if (ds.Tables[0].Rows[0]["accountNo"].ToString() == "0")
                            {
                                hd_customerType.Value = "1";
                            }
                            else
                            {
                                hd_customerType.Value = "2";
                            }

                            if (txt_riderCode.Text.Trim() != "")
                            {
                                //txt_riderCode_TextChanged(this, e);
                            }

                            txt_accountNo.Focus();

                        }
                        else
                        {
                            ViewState["isAPIClient"] = "0";
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Invalid Account Number')", true);
                            txt_accountNo.Text = "";
                            txt_accountNo.Focus();
                        }
                    }
                    else
                    {
                        ViewState["isAPIClient"] = "0";
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Invalid Account Number')", true);
                        txt_accountNo.Text = "";
                        txt_accountNo.Focus();
                    }
                }
            }

            if (txt_accountNo.Text.Trim(' ') == "0")
            {
                ViewState["isAPIClient"] = "0";
                //txt_consigner.Text = "";//ds.Tables[0].Rows[0]["Name"].ToString
            }
        }
        protected void btn_cancelDialogue_Click(object sender, EventArgs e)
        {
            divDialogue.Style.Add("Display", "none");
            chk_bulkUpdate.Checked = false;
        }
        protected void btn_okDialogue_Click(object sender, EventArgs e)
        {
            #region MyRegion
            //clvar.consignmentNo = txt_cnNumber.Text;
            //clvar.AccountNo = txt_accountNo.Text;
            //clvar.origin = dd_origin.SelectedValue;
            //clvar.serviceTypeId = dd_serviceType.SelectedValue;
            //clvar.ServiceTypeName = dd_serviceType.SelectedItem.Text;
            //clvar.Consigner = txt_consigner.Text;
            //clvar.Consignee = txt_consignee.Text;
            //clvar.destination = dd_destination.SelectedValue;
            //clvar.Weight = float.Parse(txt_weight.Text);
            //clvar.riderCode = txt_riderCode.Text;
            //clvar.OriginExpressCenterCode = dd_originExpressCenter.SelectedValue;
            //clvar.cnTypeId = dd_consignmentType.SelectedValue;
            //clvar.ChargeAmount = double.Parse(ViewState["totalAmount"].ToString());
            //clvar.gst = double.Parse(ViewState["totalGST"].ToString());
            //clvar.status = 1;// int.Parse(dd_approvalStatus.SelectedValue);
            //clvar.CheckCondition = ViewState["gst"].ToString();
            //clvar.CustomerClientID = hd_CreditClientID.Value;
            //clvar.LoadingDate = picker_reportingDate.SelectedDate.Value.ToString("yyyy-MM-dd");
            //DataTable pm = new DataTable();
            //pm.Columns.Add("priceModifierId");
            //pm.Columns.Add("priceModifierName");
            //pm.Columns.Add("calculatedValue");
            //pm.Columns.Add("calculationBase");
            //pm.Columns.Add("isTaxable");
            //pm.Columns.Add("description");
            //pm.Columns.Add("NEW");
            //pm.Columns.Add("SortOrder");
            //pm.AcceptChanges();
            //if (gv_CNModifiers.Rows.Count > 0)
            //{
            //    foreach (GridViewRow row in gv_CNModifiers.Rows)
            //    {
            //        DataRow dr = pm.NewRow();
            //        dr["priceModifierId"] = (row.FindControl("btn_gRemove") as Button).CommandArgument.ToString();
            //        dr["priceModifierName"] = row.Cells[1].Text;
            //        dr["calculatedValue"] = (row.FindControl("txt_gValue") as TextBox).Text;
            //        dr["calculationBase"] = row.Cells[3].Text;
            //        dr["isTaxable"] = row.Cells[4].Text;
            //        dr["description"] = row.Cells[5].Text;
            //        dr["NEW"] = (row.FindControl("hd_new") as HiddenField).Value;
            //        dr["SortOrder"] = (row.FindControl("hd_sortOrder") as HiddenField).Value;
            //        pm.Rows.Add(dr);
            //        pm.AcceptChanges();
            //    }
            //}

            //if (con.ApproveDomesticConsignment(clvar, pm) == "OK")
            //{
            //    divDialogue.Style.Add("display", "none");
            //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Consignment Saved')", true);
            //    btn_reset_Click(sender, e);
            //    return;
            //} 
            #endregion

            divDialogue.Style.Add("display", "none");

            Int64 rangeFrom = Int64.Parse(txt_rangeFrom.Text);
            Int64 rangeTo = Int64.Parse(txt_rangeTo.Text);
            Int64 consignmentCount = rangeTo - rangeFrom;
            clvar.ClvarListStr.Add(txt_rangeFrom.Text);
            for (int i = 0; i < consignmentCount; i++)
            {
                clvar.ClvarListStr.Add((rangeFrom + 1).ToString());
                rangeFrom++;
            }
            ViewState["bulkRange"] = clvar.ClvarListStr;
            txt_cnNumber.Text = clvar.ClvarListStr[0].ToString();
            txt_cnNumber_TextChanged(sender, e);


        }
        protected void gv_CNModifiers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Remove")
            {
                DataTable dt = ViewState["pm"] as DataTable;

                string id = e.CommandArgument.ToString();

                DataRow dr = dt.Select("priceModifierID = '" + id + "'")[0];
                dr["NEW"] = "REMOVED";
                ViewState["pm"] = dt;
                gv_CNModifiers.DataSource = dt;
                gv_CNModifiers.DataBind();
            }
        }
        protected void txt_riderCode_TextChanged(object sender, EventArgs e)
        {
            if (txt_riderCode.Text == txt_weight.Text)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Weight And Rider Code Cannot be Equal.')", true);
                txt_riderCode.Text = "";
                return;
            }
            if (txt_riderCode.Text != "")
            {
                clvar = new Cl_Variables();
                clvar.RiderCode = txt_riderCode.Text;
                clvar.origin = dd_origin.SelectedValue;

                DataSet Rider = con.RiderInformation(clvar);
                try
                {
                    if (Rider.Tables[0].Rows.Count != 0)
                    {
                        bool found = false;
                        //if (txt_accountNo.Text.Trim() != "0" && txt_accountNo.Text.Trim() != "")
                        //{
                        //hd_OriginExpressCenter.Value = Rider.Tables[0].Rows[0]["expresscenterid"].ToString();
                        foreach (ListItem item in dd_originExpressCenter.Items)
                        {
                            if (item.Value == Rider.Tables[0].Rows[0]["ExpressCenterID"].ToString())
                            {
                                found = true;

                                break;
                            }
                        }
                        if (txt_accountNo.Text != "0")
                        {
                            if (found)
                            {
                                this.dd_originExpressCenter.SelectedValue = Rider.Tables[0].Rows[0]["expresscenterid"].ToString();
                                dd_originExpressCenter.Enabled = false;
                            }
                            else
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Origin Express Center Not Found for Selected Rider. Please Select Other Rider')", true);
                                txt_riderCode.Text = "";
                                dd_originExpressCenter.ClearSelection();
                                return;
                            }
                        }
                        else
                        {
                            string consignmentNumber = txt_cnNumber.Text;
                            DataTable seqTbl = GetECBySequence(consignmentNumber);
                            if (seqTbl != null)
                            {
                                if (seqTbl.Rows.Count > 0)
                                {
                                    if (seqTbl.Rows.Count > 1)
                                    {
                                        AlertMessage("Consignment Sequence Issued to Multiple Express Centers. Contact your Zonal Accountant.");
                                        dd_originExpressCenter.ClearSelection();
                                        dd_originExpressCenter.Enabled = false;
                                        return;
                                    }
                                    ListItem item = dd_originExpressCenter.Items.FindByValue(seqTbl.Rows[0]["ExpressCenter"].ToString());
                                    if (item != null)
                                    {
                                        dd_originExpressCenter.SelectedValue = seqTbl.Rows[0]["ExpressCenter"].ToString();
                                        dd_originExpressCenter.Enabled = false;
                                    }
                                    else
                                    {
                                        if (seqTbl.Rows[0]["ExpressCenter"].ToString().Trim() == "")
                                        {
                                            AlertMessage("No Express Center Defined for this sequence. Select Express Center.");
                                            dd_originExpressCenter.ClearSelection();
                                            dd_originExpressCenter.Enabled = true;

                                            return;
                                        }
                                        dd_originExpressCenter.Enabled = false;
                                        dd_originExpressCenter.Items.Add(new ListItem { Value = seqTbl.Rows[0]["ExpressCenter"].ToString(), Text = seqTbl.Rows[0]["ECName"].ToString() });
                                        dd_originExpressCenter.SelectedValue = seqTbl.Rows[0]["ExpressCEnter"].ToString();
                                    }

                                }
                                else
                                {

                                    AlertMessage("Express Center not found for Entered consignment. Select Express Center.");
                                    dd_originExpressCenter.Enabled = true;
                                    dd_originExpressCenter.ClearSelection();
                                    return;
                                }
                            }
                            else
                            {
                                AlertMessage("Express Center not found for Entered consignment. ");
                                dd_originExpressCenter.ClearSelection();
                                return;
                            }
                        }
                        //}
                        // hd_OriginExpressCenter.Value = Rider.Tables[0].Rows[0]["expresscenterid"].ToString();
                        //foreach (ListItem item in dd_originExpressCenter.Items)
                        //{
                        //    if (item.Value == Rider.Tables[0].Rows[0]["ExpressCenterID"].ToString())
                        //    {
                        //        found = true;

                        //        break;
                        //    }
                        //}
                        //if (found)
                        //{
                        //    this.dd_originExpressCenter.SelectedValue = Rider.Tables[0].Rows[0]["expresscenterid"].ToString();

                        //}
                        //else
                        //{
                        //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Origin Express Center Not Found for Selected Rider. Please Select Other Rider')", true);
                        //    txt_riderCode.Text = "";
                        //    dd_originExpressCenter.ClearSelection();
                        //    return;
                        //}

                        txt_l.Focus();
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Invalid Rider Code')", true);
                        dd_originExpressCenter.ClearSelection();
                        txt_riderCode.Text = "";
                        txt_riderCode.Focus();
                    }

                }
                catch (Exception ex)
                { }

            }
        }
        protected void txt_riderCode_TextChanged_Save(object sender, EventArgs e)
        {
            if (txt_riderCode.Text == txt_weight.Text)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Weight And Rider Code Cannot be Equal.')", true);
                txt_riderCode.Text = "";
                return;
            }
            if (txt_riderCode.Text != "")
            {
                clvar = new Cl_Variables();
                clvar.RiderCode = txt_riderCode.Text;
                clvar.origin = dd_origin.SelectedValue;

                DataSet Rider = con.RiderInformation(clvar);
                try
                {
                    if (Rider.Tables[0].Rows.Count != 0)
                    {
                        bool found = false;
                        //if (txt_accountNo.Text.Trim() != "0" && txt_accountNo.Text.Trim() != "")
                        //{
                        //hd_OriginExpressCenter.Value = Rider.Tables[0].Rows[0]["expresscenterid"].ToString();
                        foreach (ListItem item in dd_originExpressCenter.Items)
                        {
                            if (item.Value == Rider.Tables[0].Rows[0]["ExpressCenterID"].ToString())
                            {
                                found = true;

                                break;
                            }
                        }
                        if (txt_accountNo.Text != "0")
                        {
                            if (found)
                            {
                                this.dd_originExpressCenter.SelectedValue = Rider.Tables[0].Rows[0]["expresscenterid"].ToString();
                                dd_originExpressCenter.Enabled = false;
                            }
                            else
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Origin Express Center Not Found for Selected Rider. Please Select Other Rider')", true);
                                txt_riderCode.Text = "";
                                dd_originExpressCenter.ClearSelection();
                                return;
                            }
                        }

                        //}
                        // hd_OriginExpressCenter.Value = Rider.Tables[0].Rows[0]["expresscenterid"].ToString();
                        //foreach (ListItem item in dd_originExpressCenter.Items)
                        //{
                        //    if (item.Value == Rider.Tables[0].Rows[0]["ExpressCenterID"].ToString())
                        //    {
                        //        found = true;

                        //        break;
                        //    }
                        //}
                        //if (found)
                        //{
                        //    this.dd_originExpressCenter.SelectedValue = Rider.Tables[0].Rows[0]["expresscenterid"].ToString();

                        //}
                        //else
                        //{
                        //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Origin Express Center Not Found for Selected Rider. Please Select Other Rider')", true);
                        //    txt_riderCode.Text = "";
                        //    dd_originExpressCenter.ClearSelection();
                        //    return;
                        //}

                        txt_l.Focus();
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Invalid Rider Code')", true);
                        dd_originExpressCenter.ClearSelection();
                        txt_riderCode.Text = "";
                        txt_riderCode.Focus();
                    }

                }
                catch (Exception ex)
                { }

            }
        }
        protected void gv_CNModifiers_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if ((e.Row.FindControl("hd_new") as HiddenField).Value == "REMOVED")
                {
                    e.Row.Visible = false;
                }
            }
        }
        protected void chk_bulkUpdate_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_bulkUpdate.Checked)
            {
                divDialogue.Style.Add("display", "block");
            }
            else
            {
                divDialogue.Style.Add("display", "none");
            }
        }
        protected void picker_bookingDate_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            if (picker_bookingDate.SelectedDate > DateTime.Now)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Invalid Date')", true);
                picker_bookingDate.SelectedDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
                return;
            }
            int currentMonth = DateTime.Now.Month;
            //if (picker_bookingDate.SelectedDate.Value.Month < currentMonth)
            //{
            //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Invalid Date.')", true);
            //    picker_bookingDate.SelectedDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
            //}
        }

        protected void CheckFrozenThings()
        {
            if (chk_BookingDateFreeze.Checked)
            {
                picker_bookingDate.SelectedDate = DateTime.Parse(ViewState["FrozenBookingDate"].ToString());
            }
            else
            {

            }

            if (chk_accountNoFreeze.Checked)
            {
                txt_accountNo.Text = ViewState["FrozenAccountNo"].ToString();
            }
            else
            {

            }

            if (chk_riderCodeFreeze.Checked)
            {
                txt_riderCode.Text = ViewState["FrozenRiderCode"].ToString();
            }
            else
            {

            }

            if (chk_reportingDateFreeze.Checked)
            {
                picker_reportingDate.SelectedDate = DateTime.Parse(ViewState["FrozenReportingDate"].ToString());
            }
            else
            {

            }
        }

        protected void chk_BookingDateFreeze_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_BookingDateFreeze.Checked)
            {
                ViewState["FrozenBookingDate"] = picker_bookingDate.SelectedDate.Value;
                picker_bookingDate.Enabled = false;
            }
            else
            {
                picker_bookingDate.Enabled = true;
            }
        }
        protected void chk_accountNoFreeze_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_accountNoFreeze.Checked)
            {
                ViewState["FrozenAccountNo"] = txt_accountNo.Text;
                txt_accountNo.Enabled = false;
            }
            else
            {
                txt_accountNo.Enabled = true;
            }
        }
        protected void chk_riderCodeFreeze_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_riderCodeFreeze.Checked)
            {
                ViewState["FrozenRiderCode"] = txt_riderCode.Text;
                txt_riderCode.Enabled = false;
            }
            else
            {
                txt_riderCode.Enabled = true;
            }
        }
        protected void chk_reportingDateFreeze_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_reportingDateFreeze.Checked)
            {
                ViewState["FrozenReportingDate"] = picker_reportingDate.SelectedDate.Value;
                picker_reportingDate.Enabled = false;
            }
            else
            {
                picker_reportingDate.Enabled = true;
            }
        }
        protected void txt_weight_TextChanged(object sender, EventArgs e)
        {
            if (txt_weight.Text == txt_riderCode.Text)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Weight And Rider Code Cannot be Equal.')", true);
                txt_weight.Text = "";
                return;
            }
            txt_riderCode.Focus();
        }

        protected void CODSequence()
        {
            DataTable dt = new DataTable();




            //string sqlString = "select cod.*, \n" +
            //                   "       cc.accountNo \n" +
            //                   "  from CODUserCNSequence cod \n" +
            //                   " inner join CreditClients cc \n" +
            //                   "    on cc.id = cod.CreditClientID";
            //SqlConnection con = new SqlConnection(clvar.Strcon());

            //try
            //{
            //    con.Open();
            //    SqlDataAdapter sda = new SqlDataAdapter(sqlString, con);
            //    sda.Fill(dt);

            //}
            //catch (Exception ex)
            //{ }
            //finally { con.Close(); }

            ViewState["codSequence"] = dt;

        }

        protected DataTable MinimumDate(Cl_Variables clvar)
        {
            string sqlString = "select MAX(DateTime) DateAllowed from Mnp_Account_DayEnd where Branch = '" + clvar.Branch + "' and doc_type = 'A'";
            string sql = "SELECT MAX(d.DateTime)        DateAllowed \n"
               + "FROM   Mnp_Account_DayEnd     d \n"
               + "WHERE  d.Doc_Type = ( \n"
               + "           SELECT CASE  \n"
               + "                       WHEN zu.[Profile] IN ('2', '5', '9', '12', '38') THEN 'O' \n"
               + "                       WHEN zu.[Profile] IN ('6', '16', '33', '37', '39', '44', '53', '52', '108') THEN  \n"
               + "                            'A' \n"
               + "                  END           Doc_type \n"
               + "           FROM   ZNI_USER1     zu \n"
               + "           WHERE  zu.U_ID = '" + HttpContext.Current.Session["U_ID"].ToString() + "' \n"
               + "       ) \n"
               + "       AND d.Branch = '" + clvar.Branch + "'";


            sqlString = "SELECT CASE\n" +
            "         WHEN ZU.PROFILE IN ('2', '5', '9', '12', '38', '113','126','137') AND\n" +
            "              MAX(A.OPSDATEALLOWED) > MAX(A.ACCDATEALLOWED) THEN\n" +
            "          MAX(A.OPSDATEALLOWED)\n" +
            "         WHEN ZU.PROFILE IN ('2', '5', '9', '12', '38', '113','126','137') AND\n" +
            "              MAX(A.OPSDATEALLOWED) <= MAX(A.ACCDATEALLOWED) THEN\n" +
            "          MAX(A.ACCDATEALLOWED)\n" +
            "         WHEN ZU.PROFILE IN\n" +
            "              ('6', '16', '33', '37', '39', '44', '53', '52', '108','126','137') THEN\n" +
            "          MAX(A.ACCDATEALLOWED)\n" +
            "       END DateAllowed\n" +
            "  FROM (SELECT MAX(D.DATETIME) ACCDATEALLOWED, 0 OPSDATEALLOWED\n" +
            "          FROM MNP_ACCOUNT_DAYEND D\n" +
            "         WHERE D.BRANCH = '" + clvar.Branch + "'\n" +
            "           AND D.DOC_TYPE = 'A'\n" +
            "        UNION ALL\n" +
            "        SELECT 0 ACCDATEALLOWED, MAX(D.DATETIME) OPSDATEALLOWED\n" +
            "          FROM MNP_ACCOUNT_DAYEND D\n" +
            "         WHERE D.BRANCH = '" + clvar.Branch + "'\n" +
            "           AND D.DOC_TYPE = 'O') A\n" +
            " INNER JOIN ZNI_USER1 ZU\n" +
            "    ON ZU.U_ID = '" + HttpContext.Current.Session["U_ID"].ToString() + "'\n" +
            " GROUP BY ZU.PROFILE";

            SqlConnection con = new SqlConnection(clvar.Strcon());
            DataTable dt = new DataTable();
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

        protected void btn_unapprove_Click(object sender, EventArgs e)
        {
            if (hd_portalConsignment.Value == "1")
            {
                AlertMessage("This COD Portal Consignment cannot be used.");
                return;
            }
            if (ViewState["isAPIClient"].ToString() == "1")
            {
                AlertMessage("This COD Account Cannot be used.");
                return;
            }

            clvar.Branch = dd_origin.SelectedValue;
            DataTable dates = MinimumDate(clvar);
            DateTime minAllowedDate = DateTime.Parse(dates.Rows[0]["DateAllowed"].ToString()).AddDays(1);
            DateTime maxAllowedDate = DateTime.Now;
            string[] operationProfiles = { "2", "5", "9", "12", "38", "113" };
            if (picker_bookingDate.SelectedDate < minAllowedDate || picker_bookingDate.SelectedDate > maxAllowedDate || picker_reportingDate.SelectedDate < minAllowedDate || picker_reportingDate.SelectedDate > maxAllowedDate)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Day End has already been performed for selected Dates. Cannot UnApprove CN')", true);
                return;
            }
            else if (operationProfiles.Contains(HttpContext.Current.Session["Profile"].ToString()))
            {
                if (dd_approvalStatus.SelectedValue == "1")
                {
                    DateTime bd = DateTime.Parse(hd_BookingDate.Value).Date;
                    DateTime ad = DateTime.Parse(hd_AccountReceivingDate.Value).Date;
                    DateTime dateToCheck = DateTime.Now.AddDays(-1).Date;

                    if ((bd <= DateTime.Now.Date || ad <= DateTime.Now.Date) && !(bd < dateToCheck || ad < dateToCheck))
                    {
                        string dayEndTime = GetDayEndTime();
                        if (dayEndTime == "")
                        {
                            AlertMessage("Cannot Unapprove. Day End Time not defined. Please Contact I.T. Support.");
                            return;
                        }
                        else
                        {
                            DateTime t1 = DateTime.Now;
                            DateTime t2 = Convert.ToDateTime(dayEndTime);
                            int i = DateTime.Compare(t2, t1);
                            if (i <= 0 && (ad.Date != DateTime.Today.Date || bd.Date != DateTime.Today.Date))
                            {
                                AlertMessage("Consignment Cannot be unapproved. Consignment\\'s Date is Closed.");
                                return;
                            }
                        }
                    }
                    else
                    {
                        AlertMessage("Consignment Cannot be unapproved. Consignment\\'s Date is Closed.");
                        return;
                    }
                }
            }

            if (hd_unApproveable.Value == "0" || txt_invoiceNumber.Text.Trim() != "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('This Consignment Cannot Be Unapproved')", true);
                return;
            }
            //txt_cnNumber_TextChanged(this, e);
            clvar.consignmentNo = txt_cnNumber.Text;
            DataTable dt = GetConsignmentForApproval(clvar);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    picker_bookingDate.SelectedDate = DateTime.Parse(dt.Rows[0]["BookingDate"].ToString());
                    picker_reportingDate.SelectedDate = DateTime.Parse(dt.Rows[0]["AccountReceivingDate"].ToString());
                    if (picker_bookingDate.SelectedDate < minAllowedDate || picker_bookingDate.SelectedDate > maxAllowedDate || picker_reportingDate.SelectedDate < minAllowedDate || picker_reportingDate.SelectedDate > maxAllowedDate)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Day End has already been performed for selected Dates. Cannot UnApprove CN')", true);
                        return;
                    }
                    else if (operationProfiles.Contains(HttpContext.Current.Session["Profile"].ToString()))
                    {
                        if (dd_approvalStatus.SelectedValue == "1")
                        {
                            DateTime bd = DateTime.Parse(dt.Rows[0]["BookingDate"].ToString()).Date;
                            DateTime ad = DateTime.Parse(dt.Rows[0]["AccountReceivingDate"].ToString()).Date;
                            DateTime dateToCheck = DateTime.Now.AddDays(-1).Date;

                            if ((bd <= DateTime.Now.Date || ad <= DateTime.Now.Date) && !(bd < dateToCheck || ad < dateToCheck))
                            {
                                string dayEndTime = GetDayEndTime();
                                if (dayEndTime == "")
                                {
                                    AlertMessage("Cannot Unapprove. Day End Time not defined. Please Contact I.T. Support.");
                                    return;
                                }
                                else
                                {
                                    DateTime t1 = DateTime.Now;
                                    DateTime t2 = Convert.ToDateTime(dayEndTime);
                                    int i = DateTime.Compare(t2, t1);
                                    if (i <= 0 && (ad.Date != DateTime.Today.Date || bd.Date != DateTime.Today.Date))
                                    {
                                        AlertMessage("Consignment Cannot be unapproved. Consignment\\'s Date is Closed.");
                                        return;
                                    }
                                }
                            }
                            else
                            {
                                AlertMessage("Consignment Cannot be unapproved. Consignment\\'s Date is Closed.");
                                return;
                            }
                        }
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cannot UnApprove CN')", true);
                    return;
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cannot UnApprove CN')", true);
                return;
            }
            if (picker_bookingDate.SelectedDate < minAllowedDate || picker_bookingDate.SelectedDate > maxAllowedDate || picker_reportingDate.SelectedDate < minAllowedDate || picker_reportingDate.SelectedDate > maxAllowedDate)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Day End has already been performed for selected Dates. Cannot UnApprove CN')", true);
                return;
            }
            if (hd_unApproveable.Value == "0")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('This Consignment Cannot Be Unapproved')", true);
                return;
            }
            if (txt_accountNo.Text == "")
            {
                return;
            }

            if (dd_approvalStatus.SelectedItem.Text.ToUpper() == "UNAPPROVED")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Consignment has already been unapproved.')", true);
                return;
            }



            clvar.consignmentNo = txt_cnNumber.Text;

            string error = func.UnapproveConsignment(clvar);
            if (error != "OK")
            {
                Errorid.Text = "Consignment Could not be unapproved";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Consignment Could not be unapproved')", true);
                return;
            }
            else
            {
                Errorid.Text = "Consignment unapproved";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Consignment unapproved')", true);
                txt_cnNumber_TextChanged(sender, e);
                //txt_consignmentNo_TextChanged(this, e);
                return;
            }

        }
        public DataTable GetConsignmentForApproval(Cl_Variables clvar)
        {

            string sqlString = "select c.bookingDate,\n" +
            "       c.consignmentNumber,\n" +
            "       c.customerType,\n" +
            "       c.creditClientId,\n" +
            "       c.orgin,\n" +
            "       c.serviceTypeName,\n" +
            "       c.consigner,\n" +
            "       c.consignee,\n" +
            "       c.destination,\n" +
            "       c.weight,\n" +
            "       c.riderCode,\n" +
            "       c.originExpressCenter,\n" +
            "       c.consignmentTypeId,\n" +
            "       c.chargedAmount,\n" +
            "       c.isApproved,\n" +
            "       ic.invoiceNumber,\n" +
            "       i.startDate ReportingDate,\n" +
            "       i.deliveryStatus,\n" +
            "       cm.priceModifierId,\n" +
            "       p.name,\n" +
            "       cm.calculatedValue,\n" +
            "       cm.calculationBase,\n" +
            "       cm.isTaxable,\n" +
            "       cm.SortOrder,\n" +
            "       p.description, c.destinationExpressCenterCode, c.couponNumber, c.remarks, cm.AlternateValue, c.origin_country\n" +
            "\n" +
            "  from consignment c\n" +
            " inner join InvoiceConsignment ic\n" +
            "    on c.consignmentNumber = ic.consignmentNumber\n" +
            " inner join Invoice i\n" +
            "    on i.invoiceNumber = ic.invoiceNumber\n" +
            " inner join ServiceTypes_New i1\n" +
            "    on i1.ServiceTypeName = c.serviceTypeName\n" +
            "  left outer join ConsignmentModifier cm\n" +
            "    on c.consignmentNumber = cm.consignmentNumber\n" +
            "  left outer join PriceModifiers p\n" +
            "    on cm.priceModifierId = p.id\n" +
            " where c.orgin = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'\n" +
            "   and c.consignmentTypeId <> '10'\n" +
            "   and cm.priceModifierId is not null\n" +
            "   and IsNull( i.IsInvoiceCanceled , 0 ) ='0'\n" +
            "   and c.consignmentNumber = '" + clvar.consignmentNo + "'\n" +
            " order by consignmentNumber, SortOrder";

            // sqlString = "select c.bookingDate,\n" +
            //"       c.consignmentNumber,\n" +
            //"       c.customerType,\n" +
            //"       c.creditClientId,\n" +
            //"       c.orgin,\n" +
            //"       c.serviceTypeName,\n" +
            //"       c.consigner,\n" +
            //"       c.consignee,\n" +
            //"       c.destination,\n" +
            //"       c.weight,\n" +
            //"       c.riderCode,\n" +
            //"       c.originExpressCenter,\n" +
            //"       c.consignmentTypeId,\n" +
            //"       c.chargedAmount,\n" +
            //"       CAST(c.isApproved as varchar) isApproved,\n" +
            //"       c.consignerAccountNo accountNo,\n" +
            //"       ic.invoiceNumber,\n" +
            //"       i.startDate ReportingDate,\n" +
            //"       i.deliveryStatus,\n" +
            //"       cm.priceModifierId,\n" +
            //"       p.name priceModifierName,\n" +
            //"       cm.calculatedValue,\n" +
            //"       cm.calculationBase,\n" +
            //"       cm.isTaxable,\n" +
            //"       cm.SortOrder,\n" +
            //"       p.description, c.destinationExpressCenterCode, c.accountReceivingDate, c.bookingDate, c.COD\n" +
            //"\n" +
            //"  from consignment c\n" +
            //" left outer join InvoiceConsignment ic\n" +
            //"    on c.consignmentNumber = ic.consignmentNumber\n" +
            //" left outer join Invoice i\n" +
            //"    on i.invoiceNumber = ic.invoiceNumber\n" +
            //"  left outer join ConsignmentModifier cm\n" +
            //"    on c.consignmentNumber = cm.consignmentNumber\n" +
            //"  left outer join PriceModifiers p\n" +
            //"    on cm.priceModifierId = p.id\n" +
            //" inner join CreditClients cc\n" +
            //"    on c.creditClientId = cc.id\n" +
            //" where /*c.orgin = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'\n" +
            //"   and c.consignmentTypeId <> '10'\n" +
            //"   and */c.consignmentNumber = '" + clvar.consignmentNo.Trim() + "' and ISNULL(i.IsInvoiceCanceled,0) = '0' \n" +
            //" order by consignmentNumber, SortOrder";

            string sql = "SELECT A.*, i.deliveryStatus, CASE  \n"
               + "WHEN i.IsInvoiceCanceled = '1' THEN ''\n"
               + " ELSE i.invoiceNumber      \n"
               + "END invoiceNumber_ FROM ( \n"
               + "SELECT --c.bookingDate, \n"
               + "       c.consignmentNumber, \n"
               + "       c.customerType, \n"
               + "       c.creditClientId, \n"
               + "       c.orgin, \n"
               + "       c.serviceTypeName, \n"
               + "       c.consigner, \n"
               + "       c.consignee, \n"
               + "       c.destination, \n"
               + "       c.weight, \n"
               + "       c.riderCode, \n"
               + "       c.originExpressCenter, \n"
               + "       c.consignmentTypeId, \n"
               + "       c.chargedAmount, \n"
               + "       CAST(c.isApproved AS VARCHAR)     isApproved, \n"
               + "       c.consignerAccountNo              accountNo, \n"
               + "       --ic.invoiceNumber, \n"
               + "   --    i.startDate                       ReportingDate, \n"
               + "   --    i.deliveryStatus, \n"
               + "       cm.priceModifierId, \n"
               + "       p.name                            priceModifierName, \n"
               + "       cm.calculatedValue, \n"
               + "       cm.modifiedCalculationValue modifiedCalculatedValue,\n"
               + "       cm.calculationBase, \n"
               + "       cm.isTaxable, \n"
               + "       cm.SortOrder, \n"
               + "       p.description, \n"
               + "       c.destinationExpressCenterCode, \n"
               + "       c.accountReceivingDate, \n"
               + "       c.bookingDate, \n"
               + "       c.COD, c.gst, c.totalAmount, \n"
               + "       ic.invoiceNumber, c.consignerCellNo, c.ConsigneePhoneNo, c.address, c.pieces, c.couponNumber \n"
               + "    --   i.IsInvoiceCanceled \n"
               + "FROM   consignment c \n"
               + "       LEFT OUTER JOIN InvoiceConsignment ic \n"
               + "            ON  c.consignmentNumber = ic.consignmentNumber \n"
               + "       --LEFT OUTER JOIN Invoice i \n"
               + "       --     ON  i.invoiceNumber = ic.invoiceNumber \n"
               + "       LEFT OUTER JOIN ConsignmentModifier cm \n"
               + "            ON  c.consignmentNumber = cm.consignmentNumber \n"
               + "       LEFT OUTER JOIN PriceModifiers p \n"
               + "            ON  cm.priceModifierId = p.id \n"
               + "       INNER JOIN CreditClients cc \n"
               + "            ON  c.creditClientId = cc.id \n"
               + "WHERE  /*c.orgin = '4' \n"
               + "       and c.consignmentTypeId <> '10' \n"
               + "       and */c.consignmentNumber = '" + clvar.consignmentNo.Trim() + "' \n"
               + "  --     AND ISNULL(i.IsInvoiceCanceled, 0) = '0' \n"
               + ") A \n"
               + "LEFT OUTER JOIN  \n"
               + "Invoice i  \n"
               + "ON A.invoiceNumber = i.invoiceNumber \n"
               + "  ORDER BY i.createdOn desc";


            sqlString = "SELECT A.*,\n" +
            "       i.deliveryStatus,\n" +
            "       CASE\n" +
            "         WHEN i.IsInvoiceCanceled = '1' THEN\n" +
            "          ''\n" +
            "         ELSE\n" +
            "          i.invoiceNumber\n" +
            "       END invoiceNumber_\n" +
            "  FROM (SELECT --c.bookingDate,\n" +
            "         c.consignmentNumber,\n" +
            "         c.customerType,\n" +
            "         c.creditClientId,\n" +
            "         c.orgin,\n" +
            "         c.serviceTypeName,\n" +
            "         c.consigner,\n" +
            "         c.consignee,\n" +
            "         c.destination,\n" +
            "         c.weight,\n" +
            "         c.riderCode,\n" +
            "         c.originExpressCenter,\n" +
            "         c.consignmentTypeId,\n" +
            "         c.chargedAmount,\n" +
            "         CAST(c.isApproved AS VARCHAR) isApproved,\n" +
            "         c.consignerAccountNo accountNo,\n" +
            "         --ic.invoiceNumber,\n" +
            "         --    i.startDate                       ReportingDate,\n" +
            "         --    i.deliveryStatus,\n" +
            "         cm.priceModifierId,\n" +
            "         p.name priceModifierName,\n" +
            "         cm.calculatedValue,\n" +
            "         cm.modifiedCalculationValue modifiedCalculatedValue,\n" +
            "         cm.calculationBase,\n" +
            "         p.Isgst isTaxable,\n" +
            "         cm.SortOrder,\n" +
            "         p.description,\n" +
            "         c.destinationExpressCenterCode,\n" +
            "         c.accountReceivingDate,\n" +
            "         c.bookingDate,\n" +
            "         c.COD,\n" +
            "         c.gst,\n" +
            "         c.totalAmount,\n" +
            "         ic.invoiceNumber,\n" +
            "         c.consignerCellNo,\n" +
            "         c.ConsigneePhoneNo,\n" +
            "         c.address,\n" +
            "         c.pieces,\n" +
            "         c.couponNumber,\n" +
            "         CASE\n" +
            "           when cns.CreditClientID is null then\n" +
            "            '0'\n" +
            "           else\n" +
            "            '1'\n" +
            "         end PortalCN, c.width, c.breadth, c.height, c.denseWeight, c.remarks, cm.AlternateValue,c.origin_country\n" +
            "        --   i.IsInvoiceCanceled\n" +
            "          FROM consignment c\n" +
            " inner join ServiceTypes_New i1\n" +
            "    on i1.ServiceTypeName = c.serviceTypeName\n" +
            "          LEFT OUTER JOIN InvoiceConsignment ic\n" +
            "            ON c.consignmentNumber = ic.consignmentNumber\n" +
            "        --LEFT OUTER JOIN Invoice i\n" +
            "        --     ON  i.invoiceNumber = ic.invoiceNumber\n" +
            "          LEFT OUTER JOIN ConsignmentModifier cm\n" +
            "            ON c.consignmentNumber = cm.consignmentNumber\n" +
            "          LEFT OUTER JOIN PriceModifiers p\n" +
            "            ON cm.priceModifierId = p.id\n" +
            "         INNER JOIN CreditClients cc\n" +
            "            ON c.creditClientId = cc.id\n" +
            "          LEFT OUTER JOIN CODUSERS cns\n" +
            "            on cns.CreditClientID = cc.id and cns.isCOD = '1'\n" +
            "         WHERE i1.products ='Import'\n" +
            "         and c.consignmentNumber = '" + clvar.consignmentNo.Trim() + "'\n" +
            "        ) A\n" +
            "  LEFT OUTER JOIN Invoice i\n" +
            "    ON A.invoiceNumber = i.invoiceNumber\n" +
            " ORDER BY i.createdOn desc";






            DataTable dt = new DataTable();
            SqlConnection con_ = new SqlConnection(clvar.Strcon());
            try
            {
                con_.Open();
                SqlDataAdapter sda_ = new SqlDataAdapter(sqlString, con_);
                sda_.Fill(dt);

            }
            catch (Exception ex)
            { }
            finally { con_.Close(); }
            return dt;
        }

        public string ApproveDomesticConsignment(Cl_Variables clvar, DataTable pm, bool cod)
        {
            string auditQuery = "insert into MNP_consignmentunapproval (ConsignmentNumber, TransactionTime, status, USERID) VALUES (\n" +
                "'" + clvar.consignmentNo + "', GETDATE(), '1', '" + HttpContext.Current.Session["U_ID"].ToString() + "')";

            string query = "  update Consignment set\n" +
                           "            creditClientId = '" + clvar.CustomerClientID + "',\n" +
                           "        consignerAccountNo = '" + clvar.consignerAccountNo + "',\n" +
                           "               destination = '" + clvar.destination + "',\n" +
                           "           serviceTypeName = '" + clvar.ServiceTypeName + "',\n" +
                           "                 consigner = '" + clvar.Consigner + "',\n" +
                           "                 consignee = '" + clvar.Consignee + "',\n" +
                           "                    weight = '" + clvar.Weight.ToString() + "',\n" +
                           "      accountReceivingDate = '" + DateTime.Parse(clvar.LoadingDate).ToString("yyyy-MM-dd") + "', \n    " +
                           "                     orgin ='" + clvar.origin + "',\n" +
                           "                BranchCode = '" + clvar.origin + "',\n" +
                           "                  ZoneCode = '" + clvar.Zone + "',\n" +
                           "                 riderCode = '" + clvar.riderCode + "',\n";
            if (clvar.OriginExpressCenterCode.ToString() != "")
            {
                query += "       originExpressCenter = '" + clvar.OriginExpressCenterCode + "',\n";
            }
            query += "            consignmentTypeId = '" + clvar.cnTypeId + "',\n" +
                     "                chargedAmount = '" + clvar.ChargeAmount + "',\n" +
                     "                   isApproved = '" + clvar.status + "',\n" +
                     "                  totalAmount = '" + clvar.TotalAmount + "',\n" +
                     "                          Gst = '" + clvar.gst + "',\n" +
                     "              ispriceComputed = '1',\n" +
                     "                 CustomerType = '" + clvar.Customertype + "',\n" +
                     "                   modifiedBy = '" + HttpContext.Current.Session["U_ID"].ToString() + "',\n" +
                     "                   ModifiedON = GETDATE(),\n" +
                     //"                   pieces = '" + clvar.pieces.ToString() + "',\n" +
                     "            ExpressCenterCode = '" + HttpContext.Current.Session["ExpressCenter"].ToString() + "',\n" +
                     "            destinationExpressCenterCode = '" + clvar.destinationExpressCenterCode + "',\n";
            //"            Address = '" + clvar.ConsigneeAddress + "',\n";

            if (clvar.isCod == false)
            {
                query += "                        cod = '0',\n";
            }

            if (clvar.isCod == true)
            {
                query += "                        cod = '1',\n";
            }
            query += " Bookingdate =CONVERT(datetime,'" + clvar.Bookingdate + "',105), ";
            query += " ispayable = '0', isinvoiced = '0', ConsignmentScreen = '0', DestinationCountryCode='PAK'\n";
            query += "    where consignmentNumber = '" + clvar.consignmentNo + "'";
            string newPMQuery = "";
            string updatePMQuery = "";
            string removePMQuery = "";
            if (pm.Rows.Count > 0)
            {
                DataTable newPM = new DataTable();
                try
                {
                    newPM = pm.Select("NEW = '1'").CopyToDataTable();
                }
                catch (Exception ex)
                { }

                DataTable updatePM = new DataTable();
                try
                {
                    updatePM = pm.Select("NEW = ''").CopyToDataTable();
                }
                catch (Exception ex)
                { }
                DataTable delPM = new DataTable();
                try
                {
                    delPM = pm.Select("NEW = 'REMOVED'").CopyToDataTable();
                }
                catch (Exception ex)
                { }
                if (newPM.Rows.Count > 0)
                {
                    string temp = "CASE pm.ID ";
                    string temp1 = "CASE pm.ID ";
                    string temp2 = "CASE PM.ID ";
                    string temp4 = "CASE PM.ID ";
                    string temp3 = "";

                    foreach (DataRow dr in newPM.Rows)
                    {
                        temp += " WHEN '" + dr[0].ToString() + "' THEN '" + dr["calculatedValue"].ToString() + "'\n";
                        temp1 += " WHEN '" + dr[0].ToString() + "' then '" + ((double.Parse(clvar.CheckCondition) / 100) * double.Parse(dr["calculatedValue"].ToString())) + "'\n";
                        temp2 += " WHEN '" + dr[0].ToString() + "' then '" + dr["SortOrder"].ToString() + "'\n";
                        temp4 += " WHEN '" + dr[0].ToString() + "' then '" + dr["calculationBase"].ToString() + "'\n";
                        temp3 += "'" + dr[0].ToString() + "',";
                    }
                    temp += "END CalculatedValue,";
                    temp1 += "END CalculatedGST, ";
                    temp2 += "END SORTORDER ";
                    temp4 += "END CALCULATIONBASE,";
                    temp3 = temp3.TrimEnd(',');
                    newPMQuery = "INSERT INTO CONSIGNMENTMODIFIER (priceModifierID, ConsignmentNumber, createdBy, CreatedON, modifiedCalculationValue, calculatedValue, CalculatedGSt, CalculationBase, SortOrder)\n" +
                                 " select pm.id, '" + clvar.consignmentNo + "' ConsignmentNumber, '" + HttpContext.Current.Session["U_ID"].ToString() + "' CreatedBy, GETDATE() CreatedON, pm.calculationValue, " + temp + temp1 + temp4 + temp2 + " from PriceModifiers pm WHERE pm.ID in (" + temp3 + ")";
                }
                if (updatePM.Rows.Count > 0)
                {
                    string temp = "CASE priceModifierID ";
                    string temp1 = "CASE priceModifierID ";
                    string temp2 = "CASE priceModifierID ";
                    string temp3 = "";
                    foreach (DataRow dr in updatePM.Rows)
                    {
                        temp += " WHEN '" + dr[0].ToString() + "' THEN '" + dr["calculatedValue"].ToString() + "'\n";
                        temp1 += " WHEN '" + dr[0].ToString() + "' then '" + ((double.Parse(clvar.CheckCondition) / 100) * double.Parse(dr["calculatedValue"].ToString())) + "'\n";
                        temp2 += " WHEN '" + dr[0].ToString() + "' then '" + dr["SortOrder"].ToString() + "'\n";
                        temp3 += "'" + dr[0].ToString() + "',";
                    }
                    temp += "END ";
                    temp1 += "END  ";
                    temp2 += "END ";
                    temp3 = temp3.TrimEnd(',');
                    updatePMQuery = "UPDATE CONSIGNMENTMODIFIER SET CalculatedValue = " + temp + ", CalculatedGST = " + temp1 + " \n" +
                                 "  WHERE priceModifierID in (" + temp3 + ")";

                }
                if (delPM.Rows.Count > 0)
                {
                    removePMQuery += "DELETE FROM CONSIGNMENTMODIFIER where pricemodifierID in (";
                    foreach (DataRow dr in delPM.Rows)
                    {
                        removePMQuery += "'" + dr[0].ToString() + "',";
                    }
                    removePMQuery = removePMQuery.TrimEnd(',') + ") AND ConsignmentNumber = '" + clvar.consignmentNo + "'";
                }
            }


            string codQuery = "";
            if (cod & clvar.CatID != "1")
            {
                CommonFunction cf = new CommonFunction();
                DataTable dt_ = cf.GetCODConsignmentForApproval(clvar);
                if (dt_.Rows.Count == 0)
                {
                    codQuery = "Insert into CODConsignmentDetail (ConsignmentNumber, OrderRefNo, ProductTypeId, ProductDescription, ChargeCodAmount, CodAmount, CalculatedAmount ) Values \n" +
                        "(\n" +
                        "'" + clvar.consignmentNo.Trim() + "',\n" +
                        "'" + clvar.orderRefNo + "',\n" +
                        "'" + clvar.productTypeId + "',\n" +
                        "'" + clvar.productDescription + "',\n" +
                        "'" + clvar.chargeCODAmount + "',\n" +
                        "'" + clvar.codAmount + "',\n" +
                        "'" + clvar.calculatedCodAmount + "'\n" +
                        ")";
                }
                else
                {
                    codQuery = "UPDATE CODConsignmentDetail   \n" +
                               " SET    orderRefNo             = '" + clvar.orderRefNo + "', \n" +
                               "        productDescription     = '" + clvar.productDescription + "', \n" +
                               "        codAmount              = " + clvar.codAmount + ", \n" +
                               "        calculatedAmount       = " + clvar.calculatedCodAmount + " \n" +
                               " WHERE  consignmentNumber      = '" + clvar.consignmentNo.Trim() + "' ";
                }
            }


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
                sqlcmd.CommandText = query;
                sqlcmd.ExecuteNonQuery();

                if (newPMQuery != "")
                {
                    sqlcmd.CommandText = newPMQuery;
                    sqlcmd.ExecuteNonQuery();
                }
                if (updatePMQuery != "")
                {
                    sqlcmd.CommandText = updatePMQuery;
                    sqlcmd.ExecuteNonQuery();
                }
                if (removePMQuery != "")
                {
                    sqlcmd.CommandText = removePMQuery;
                    sqlcmd.ExecuteNonQuery();
                }


                if (codQuery != "")
                {
                    sqlcmd.CommandText = codQuery;
                    sqlcmd.ExecuteNonQuery();
                }

                sqlcmd.CommandText = auditQuery;
                sqlcmd.ExecuteNonQuery();

                trans.Commit();
                sqlcon.Close();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return ex.Message;
            }
            return "OK";
        }

        public string InsertConsignmentFromApprovalScreen(Cl_Variables clvar, DataTable pm, bool cod)
        {
            string auditQuery = "insert into MNP_consignmentunapproval (ConsignmentNumber, TransactionTime, status, USERID) VALUES (\n" +
     "'" + clvar.consignmentNo + "', GETDATE(), '1', '" + HttpContext.Current.Session["U_ID"].ToString() + "')";


            string query = "insert into consignment (\n" +
                "consignmentNumber,\n" +
            "\t   orgin,\n" +
            "\t   consigner,\n" +
            "\t   consignee,\n" +
            "\t   destination,\n" +
            "\t   weight,\n" +
            "\t   riderCode,\n" +
            "\t   originExpressCenter,\n" +
            "\t   consignmentTypeId,\n" +
            "\t   chargedAmount,\n" +
            "\t   gst,\n" +
            "\t   status,\n" +
            "\t   isApproved,\n" +
            "\t   cod,\n" +
            "\t   creditClientId,\n" +
            "\t   createdBy,\n" +
            "\t   createdOn,\n" +
            "\t   customerType,\n" +
            "     zoneCode,BranchCode,\n" +
            "     serviceTypeName, totalAmount, syncID, AccountReceivingDate, ispriceComputed, expressCenterCode,destinationExpressCenterCode, consignerAccountNo,BookingDate,pieces,address, ispayable, isinvoiced, consignmentScreen, destinationCountryCode) VALUES (" +
            "'" + clvar.consignmentNo + "'\n," +
            "'" + clvar.origin + "'\n," +
            "'" + clvar.Consigner + "'\n," +
            "'" + clvar.Consignee + "'\n," +
            "'" + clvar.destination + "'\n," +
            "'" + clvar.Weight + "'\n," +
            "'" + clvar.riderCode + "'\n," +
            "'" + clvar.OriginExpressCenterCode + "'\n," +
            "'" + clvar.cnTypeId + "'\n," +
            "'" + clvar.ChargeAmount + "'\n," +
            "'" + clvar.gst + "'\n," +
            "'" + clvar.status + "'\n," +
            "'" + int.Parse(clvar.status.ToString()) + "'\n,";
            if (cod)
            {
                query += "'1'\n,";
            }
            else
            {
                query += "'0'\n,";
            }

            query += "'" + clvar.CustomerClientID + "'\n," +
            "'" + HttpContext.Current.Session["U_ID"].ToString() + "'\n," +
            " GETDATE()\n," +
            "'" + clvar.Customertype + "'\n," +
            "'" + HttpContext.Current.Session["ZoneCode"].ToString() + "'\n," +
            "'" + HttpContext.Current.Session["BranchCode"].ToString() + "'\n," +
            "'" + clvar.ServiceTypeName + "'\n" +
            ", '" + clvar.TotalAmount + "', NEWID(),'" + DateTime.Parse(clvar.LoadingDate).ToString("yyyy-MM-dd") + "','1',\n" +
            "'" + HttpContext.Current.Session["ExpressCenter"].ToString() + "', '" + clvar.destinationExpressCenterCode + "', '" + clvar.consignerAccountNo + "',CONVERT(datetime,'" + clvar.Bookingdate + "',105)," + clvar.pieces.ToString() + ",'" + clvar.ConsigneeAddress + "','0','0','0','PAK')";

            string stateQuery = "Insert into consignmentStates (ConsignmentNumber, state, isInvoiced, CreatedBy, CreatedOn, UniqueID) VALUES (" +
                "'" + clvar.consignmentNo + "',\n" +
                "'NEW',\n" +
                "'0',\n" +
                "'" + HttpContext.Current.Session["U_ID"].ToString() + "'\n," +
                " GETDATE()\n," +
                " NEWID()\n" +
                ")";

            string pmQuery = "";
            if (pm.Rows.Count > 0)
            {
                string temp = "CASE pm.ID ";
                string temp1 = "CASE pm.ID ";
                string temp2 = "CASE PM.ID ";
                string temp4 = "CASE PM.ID ";
                string temp3 = "";

                foreach (DataRow dr in pm.Rows)
                {
                    temp += " WHEN '" + dr[0].ToString() + "' THEN '" + dr["calculatedValue"].ToString() + "'\n";
                    temp1 += " WHEN '" + dr[0].ToString() + "' then '" + ((double.Parse(clvar.CheckCondition) / 100) * double.Parse(dr["calculatedValue"].ToString())) + "'\n";
                    temp2 += " WHEN '" + dr[0].ToString() + "' then '" + dr["SortOrder"].ToString() + "'\n";
                    temp4 += " WHEN '" + dr[0].ToString() + "' then '" + dr["calculationBase"].ToString() + "'\n";
                    temp3 += "'" + dr[0].ToString() + "',";
                }
                temp += "END CalculatedValue,";
                temp1 += "END CalculatedGST, ";
                temp2 += "END SORTORDER ";
                temp4 += "END CALCULATIONBASE,";
                temp3 = temp3.TrimEnd(',');
                pmQuery = "INSERT INTO CONSIGNMENTMODIFIER (priceModifierID, ConsignmentNumber, createdBy, CreatedON, modifiedCalculationValue, calculatedValue, CalculatedGSt, CalculationBase, SortOrder)\n" +
                             " select pm.id, '" + clvar.consignmentNo + "' ConsignmentNumber, '" + HttpContext.Current.Session["U_ID"].ToString() + "' CreatedBy, GETDATE() CreatedON, pm.calculationValue, " + temp + temp1 + temp4 + temp2 + " from PriceModifiers pm WHERE pm.ID in (" + temp3 + ")";
            }
            CommonFunction func = new CommonFunction();
            clvar.Branch = HttpContext.Current.Session["BranchCode"].ToString();
            string trackQuery = "insert into ConsignmentsTrackingHistory (ConsignmentNumber, stateID, currentLocation, TransactionTime) VALUES \n" +
                "('" + clvar.consignmentNo + "', '1', '" + func.Branch(clvar).Tables[0].Rows[0]["BranchName"].ToString() + "', GETDATE())";

            string codQuery = "";
            if (cod & clvar.CatID != "1")
            {
                codQuery = "Insert into CODConsignmentDetail (ConsignmentNumber, OrderRefNo, ProductTypeId, ProductDescription, ChargeCodAmount, CodAmount, CalculatedAmount ) Values \n" +
                    "(\n" +
                    "'" + clvar.consignmentNo + "',\n" +
                    "'" + clvar.orderRefNo + "',\n" +
                    "'" + clvar.productTypeId + "',\n" +
                    "'" + clvar.productDescription + "',\n" +
                    "'" + clvar.chargeCODAmount + "',\n" +
                    "'" + clvar.codAmount + "',\n" +
                    "'" + clvar.calculatedCodAmount + "'\n" +
                    ")";
            }
            int count = 0;
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

                sqlcmd.CommandText = query;
                sqlcmd.ExecuteNonQuery();

                sqlcmd.CommandText = stateQuery;
                sqlcmd.ExecuteNonQuery();

                if (pmQuery.ToString() != "")
                {
                    sqlcmd.CommandText = pmQuery;
                    sqlcmd.ExecuteNonQuery();
                }

                if (codQuery.ToString() != "")
                {
                    sqlcmd.CommandText = codQuery;
                    sqlcmd.ExecuteNonQuery();
                }



                sqlcmd.CommandText = trackQuery;
                sqlcmd.ExecuteNonQuery();

                sqlcmd.CommandText = auditQuery;
                sqlcmd.ExecuteNonQuery();

                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return ex.Message;

            }
            finally { sqlcon.Close(); }
            return "OK";
        }

        //public string GetProfileType()
        //{
        //    Uri uri = new Uri(HttpContext.Current.Request.Url.AbsoluteUri);
        //    string pageGetURL = uri.Segments.Last();
        //    string resp = "";

        //    string sqlString = "SELECT CASE\n" +
        //    "         WHEN CM.CHILD_MENUID IN ('67', '260') THEN\n" +
        //    "          'A'\n" +
        //    "         ELSE\n" +
        //    "          'O'\n" +
        //    "       END PROFILETYPE\n" +
        //    "  FROM ZNI_USER1 ZU\n" +
        //    " INNER JOIN PROFILE_DETAIL PD\n" +
        //    "    ON PD.PROFILE_ID = ZU.PROFILE\n" +
        //    " INNER JOIN CHILD_MENU CM\n" +
        //    "    ON CM.MAIN_MENU_ID = PD.MAINMENU_ID\n" +
        //    "   AND CM.CHILD_MENUID = PD.CHILDMENU_ID\n" +
        //    " WHERE ZU.U_ID = '" + HttpContext.Current.Session["U_ID"].ToString() + "'\n" +
        //    "   AND CM.HYPERLINK = '" + pageGetURL + "'";
        //    DataTable dt = new DataTable();
        //    SqlConnection con_ = new SqlConnection(clvar.Strcon());
        //    try
        //    {
        //        con_.Open();
        //        SqlDataAdapter sda_ = new SqlDataAdapter(sqlString, con_);
        //        sda_.Fill(dt);
        //        if (dt.Rows.Count > 0)
        //        {   
        //            resp = dt.Rows[0]["ProfileType"].ToString();
        //        }
        //        else
        //        {
        //            resp = "0";
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        resp = ex.Message;
        //    }
        //    finally { con_.Close(); }
        //    return resp;
        //}
        protected void btn_SaveConsignment1_Click(object sender, EventArgs e)
        {


            string isPriceComputed = "";
            string isApproved = "";
            #region Validations
            if (hd_portalConsignment.Value == "1")
            {
                AlertMessage("This COD Portal Consignment Cannot be used.");
                return;
            }
            if (txt_cnNumber.Text.Trim() == "")
            {
                AlertMessage("Enter Consignment Number");
                return;
            }

            if (ViewState["isAPIClient"].ToString() == "1")
            {
                AlertMessage("This COD Account Cannot be used.");
                return;
            }

            if (txt_accountNo.Text.Trim() == "")
            {
                AlertMessage("Enter Account Number");
                return;
            }
            if (dd_serviceType.SelectedValue == "0")
            {
                AlertMessage("Select Service Type");
                return;
            }

            if (dd_destination.SelectedValue == "0")
            {
                AlertMessage("Select Destination");
                return;
            }
            if (txt_pieces.Text.Trim() == "" || txt_pieces.Text.Trim() == "0")
            {
                AlertMessage("Pieces cannot be empty or 0");
                return;
            }
            if (txt_weight.Text.Trim() == "" || txt_weight.Text.Trim() == "0")
            {
                AlertMessage("Weight Cannot be Empty or 0");
                return;
            }
            if (txt_riderCode.Text.Trim() == "")
            {
                AlertMessage("Enter Rider Code");
                return;
            }
            if (dd_originExpressCenter.SelectedValue == "0")
            {
                AlertMessage("Select Origin EC");
                return;
            }
            if (txt_riderCode.Text.Trim() == txt_weight.Text)
            {
                AlertMessage("Rider Code Cannot be equal to Weight");
                return;
            }
            float tempDenseWeight = 0;
            float tempWeight = 0;
            float.TryParse(txt_weight.Text, out tempWeight);
            float.TryParse(txt_aWeight.Text, out tempDenseWeight);

            if (tempDenseWeight < 0.1f)
            {
                AlertMessage("Invalid Dense Weight");
                return;
            }
            if (tempWeight < 0.1f)
            {
                AlertMessage("Invalid Weight");
                return;
            }

            #endregion
            // ServiceType_2();
            // Total();
            if (ViewState["CNState"].ToString() == "NEW")
            {
                string specialCondition = "\n  AND mzc.Product = (select distinct products from serviceTypes_new where serviceTypeName = '" + dd_serviceType.SelectedValue + "')";
                clvar.Zone = HttpContext.Current.Session["ZoneCode"].ToString();
                clvar.consignmentNo = txt_cnNumber.Text.Trim();
                DataTable allowedCN = SequenceCheck(clvar, specialCondition);
                if (allowedCN.Rows.Count == 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('This CN Number cannot be used in for selected Service')", true);
                    return;
                }
            }

            double tempchamount = 0;
            double.TryParse(txt_chargedAmount.Text, out tempchamount);
            if (txt_accountNo.Text.Trim() == "0" && (tempchamount < 0 || tempchamount == 0))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Charge Amount cannot be zero or empty')", true);
                Errorid.Text = "Charge Amount cannot be zero or empty";
                return;
            }
            if (hd_unApproveable.Value == "0")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('This Consignment Cannot Be Unapproved')", true);
                return;
            }
            clvar.Branch = dd_origin.SelectedValue;
            DataTable dates = MinimumDate(clvar);
            DateTime minAllowedDate = DateTime.Parse(dates.Rows[0]["DateAllowed"].ToString()).AddDays(1);
            DateTime maxAllowedDate = DateTime.Now;
            if (picker_bookingDate.SelectedDate < minAllowedDate || picker_bookingDate.SelectedDate > maxAllowedDate || picker_reportingDate.SelectedDate < minAllowedDate || picker_reportingDate.SelectedDate > maxAllowedDate)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Day End has already been performed for selected Dates. Cannot Approve CN')", true);
                return;
            }
            else
            {

            }

            txt_accountNo_TextChanged_Save(sender, e);
            if (dd_approvalStatus.SelectedValue == "1")
            {
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('CN Already Approved. Cannot Edit. Go To Unapproval Screen to Approve this CN First')", true);
                //Errorid.Text = "Go To Unapproval Screen to Unapprove this CN First";
                //lt1.Text = "CN Already Approved. Cannot Edit.<a href='unapproveConsignment.aspx'> Go To Unapproval Screen</a> to Unapprove this CN First";
                //btn_save.Enabled = false;
                return;
            }
            if (dd_serviceType.SelectedValue == "0")
            {
                Errorid.Text = "Select Service Type";
                Errorid.ForeColor = System.Drawing.Color.Red;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage()", "alert('Select Service Type')", true);
                return;
            }
            if (dd_consignmentType.SelectedValue == "0")
            {
                Errorid.Text = "Select Consignment Type";
                Errorid.ForeColor = System.Drawing.Color.Red;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage()", "alert('Select Consignment Type')", true);
                return;
            }
            if (dd_originExpressCenter.SelectedValue == "0")
            {

                Errorid.Text = "Select Origin Express Center";
                Errorid.ForeColor = System.Drawing.Color.Red;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage()", "alert('Select Origin Express Center')", true);
                return;
            }
            if (Or_Country.SelectedValue == "0")
            {

                Errorid.Text = "Select Origin Country";
                Errorid.ForeColor = System.Drawing.Color.Red;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage()", "alert('Select Origin Country')", true);
                return;
            }


            if (txt_pieces.Text.Trim() == "" || txt_pieces.Text.Trim() == "0")
            {
                Errorid.Text = "Enter Pieces";
                Errorid.ForeColor = System.Drawing.Color.Red;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage()", "alert('Enter Pieces')", true);
                return;
            }
            double tempPieces = 0;
            double.TryParse(txt_pieces.Text, out tempPieces);
            if (tempPieces <= 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Pieces Cannot be equal or less than 0')", true);
                Errorid.Text = "Pieces Cannot be equal or less than 0";
                return;
            }
            Errorid.Text = "";
            Errorid.Text = "";
            try
            {
                txt_riderCode_TextChanged_Save(sender, e);
                DataTable cities = ViewState["cities"] as DataTable;
                clvar = new Cl_Variables();
                clvar.consignmentNo = txt_cnNumber.Text;
                clvar.AccountNo = txt_accountNo.Text;
                clvar.RiderCode = txt_riderCode.Text;
                clvar.Destination = dd_destination.SelectedValue;
                clvar.ServiceTypeName = dd_serviceType.SelectedValue;
                clvar.ServiceType = dd_serviceType.SelectedValue;
                double tempw = 0;
                double.TryParse(txt_weight.Text, out tempw);
                if (tempw <= 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Weight Cannot be equal or less than 0')", true);
                    Errorid.Text = "Weight Cannot be equal or less than 0";
                    return;
                }
                clvar.Weight = float.Parse(txt_weight.Text);
                clvar.Unit = 1;// cb_Unit.SelectedValue;
                clvar.pieces = int.Parse(txt_pieces.Text);// int.Parse(txt_Piecies.Text);// txt_Piecies.Text;
                clvar.Zone = Session["ZONECODE"].ToString();
                //clvar.PakageContents = txt_Package_Handcarry.Text;

                //Consignee and Consigner Information
                clvar.Consignee = txt_consignee.Text.ToUpper();
                clvar.ConsigneeCell = txt_consigneeCell.Text;
                //clvar.ConsigneeCNIC = txt_ConsigneeCNIC.Text;
                clvar.ConsigneeAddress = txt_Address.Text.ToUpper();
                clvar.Consigner = txt_consigner.Text.ToUpper();
                clvar.ConsignerCell = txt_consignerCell.Text;
                //clvar.ConsignerCNIC = Txt_ConsignerCNIC.Text;
                //clvar.ConsignerAddress = txt_ShipperAddress.Text.ToUpper();
                clvar.consignerAccountNo = txt_accountNo.Text;
                try
                {
                    clvar.Bookingdate = picker_bookingDate.SelectedDate.Value; //DateTime.Now; // DateTime.Parse(Session["WorkingDate"].ToString());//DateTime.Parse(dt_Picker.SelectedDate.ToString());
                }
                catch (Exception ez)
                {
                    Response.Redirect("~/Login");
                }

                clvar.origin = dd_origin.SelectedValue; //HttpContext.Current.Session["BranchCode"].ToString(); //cb_Origin.SelectedValue;
                                                        //clvar.Insurance = txt_insurance.Text;
                clvar.Othercharges = 0;// txt_Othercharges.Text;
                clvar.Day = "02"; //rb_1.SelectedValue;
                clvar.expresscenter = HttpContext.Current.Session["ExpressCenter"].ToString(); //cb_originExpresscenter.SelectedValue;//hd_OriginExpressCenter.Value;
                clvar.destinationExpressCenterCode = cities.Select("BranchCode = '" + dd_destination.SelectedValue + "'")[0]["ECCOde"].ToString();// hd_Destination_Ec.Value;// cb_Destination.SelectedValue;
                clvar.CustomerClientID = hd_CreditClientID.Value;
                clvar.OriginExpressCenterCode = dd_originExpressCenter.SelectedValue;


                clvar.Con_Type = int.Parse(dd_consignmentType.SelectedValue);
                //clvar.CouponNo = txt_Type.Text;

                //Branch Information
                DataSet BranchGSTInformation = con.BranchGSTInformation(clvar);
                string gst = "";
                if (BranchGSTInformation.Tables[0].Rows.Count != 0)
                {
                    gst = BranchGSTInformation.Tables[0].Rows[0]["gst"].ToString();
                }
                //Calculating Modifier
                double Modfier = 0;

                //foreach (GridViewRow row in gv_CNModifiers.Rows)
                //{
                //    TextBox tx = ((TextBox)row.FindControl("txt_gValue"));
                //    Modfier += double.Parse(tx.Text);
                //}

                //Now Getting Tarrif
                double DV = 0;

                double TotalAmount = (DV + 0);

                // Calculating GST
                double gst_ = ((TotalAmount) / 100) * double.Parse(gst);


                clvar.TotalAmount = TotalAmount;
                clvar.gst = gst_;
                clvar.ChargeAmount = 0;
                DataTable dt = (DataTable)ViewState["PM"];
                //if (dt.Rows.Count != 0)
                //{
                //    clvar.isPM = true;
                //    clvar.LstModifiersCNt = dt;
                //}
                //else
                //{
                //    clvar.isPM = false;
                //    clvar.LstModifiersCNt = null;
                //}
                clvar.destinationCountryCode = "PAK";
                clvar.OriginCountry = Or_Country.SelectedValue;
                clvar.IsImport = true;



                clvar.Customertype = int.Parse(hd_customerType.Value);
                if (Session["U_ID"] != null)
                {
                    clvar.createdBy = Session["U_ID"].ToString();
                }

                double tempDimesion = 0;
                double.TryParse(txt_l.Text, out tempDimesion);
                clvar.breadth = tempDimesion;

                tempDimesion = 0;
                double.TryParse(txt_w.Text, out tempDimesion);
                clvar.width = tempDimesion;

                tempDimesion = 0;
                double.TryParse(txt_h.Text, out tempDimesion);
                clvar.height = tempDimesion;

                float tempDimensionf = 0;
                float.TryParse(txt_aWeight.Text, out tempDimensionf);
                float denseWeight = tempDimensionf;

                tempDimensionf = 0;
                float volumetricWeight = 0;
                float.TryParse(txt_vWeight.Text, out tempDimensionf);
                volumetricWeight = tempDimensionf;
                clvar.Weight = (denseWeight > volumetricWeight) ? denseWeight : volumetricWeight;
                if (clvar.Weight < 0.1f)
                {
                    AlertMessage("Invalid Weight");
                    return;
                }


                string Error = Add_Consignment_Validation(clvar);
                DataTable dt_ = new DataTable();
                if (dd_serviceType.SelectedValue != "Road n Rail" && dd_serviceType.SelectedValue != "Express Cargo")
                {
                    dt_ = con.Add_OcsValidation(clvar);

                    if (dt_.Rows.Count > 0)
                    {
                        if (dt_.Rows[0]["Amount"].ToString().Trim() == "0" || dt_.Rows[0]["Amount"].ToString().Trim() == "")
                        {
                            txt_totalAmt.Text = double.Parse(dt_.Rows[0]["Amount"].ToString()) + double.Parse(dt_.Rows[0]["gst"].ToString()).ToString();
                            //clvar.gst = gst_;
                            txt_gst.Text = double.Parse(dt_.Rows[0]["gst"].ToString()).ToString();
                            //txt_chargedAmount.Text = double.Parse(dt_.Rows[0]["amount"].ToString()).ToString();
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Tariff Not Found')", true);
                            return;
                        }
                        else
                        {
                            txt_totalAmt.Text = (double.Parse(dt_.Rows[0]["Amount"].ToString())).ToString();
                            //clvar.gst = gst_;
                            TotalAmount = float.Parse(txt_totalAmt.Text);
                            txt_gst.Text = double.Parse(dt_.Rows[0]["gst"].ToString()).ToString();
                            gst_ = float.Parse(txt_gst.Text);
                            isPriceComputed = "1";
                            isApproved = "1";

                            //txt_chargedAmount.Text = double.Parse(dt_.Rows[0]["amount"].ToString()).ToString();
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Tariff Not Found')", true);
                        return;
                    }


                    //  clvar.ChargeAmount = 0;

                }
                else
                {
                    dt_ = con.Add_RNRValidation(clvar);
                    if (dt_.Rows.Count > 0)
                    {
                        if (dt_.Rows[0]["totalamount"].ToString().Trim() == "0" || dt_.Rows[0]["totalamount"].ToString().Trim() == "")
                        {
                            txt_totalAmt.Text = double.Parse(dt_.Rows[0]["totalamount"].ToString()) + double.Parse(dt_.Rows[0]["gst"].ToString()).ToString();
                            TotalAmount = float.Parse(txt_totalAmt.Text);
                            //clvar.gst = gst_;
                            txt_gst.Text = double.Parse(dt_.Rows[0]["gst"].ToString()).ToString();
                            gst_ = float.Parse(txt_gst.Text);

                            //txt_chargedAmount.Text = double.Parse(dt_.Rows[0]["amount"].ToString()).ToString();

                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Tariff Not Found')", true);
                            return;
                        }
                        else
                        {
                            txt_totalAmt.Text = double.Parse(dt_.Rows[0]["totalamount"].ToString()).ToString();
                            //clvar.gst = gst_;
                            TotalAmount = float.Parse(txt_totalAmt.Text);
                            txt_gst.Text = double.Parse(dt_.Rows[0]["gst"].ToString()).ToString();
                            gst_ = float.Parse(txt_gst.Text);
                            if (dt_.Rows[0]["IsPriceComputed"].ToString().ToUpper() == "FALSE" || dt_.Rows[0]["IsPriceComputed"].ToString().ToUpper() == "0")
                            {
                                isPriceComputed = "0";
                            }
                            else
                            {
                                isPriceComputed = "1";
                            }

                            if (dt_.Rows[0]["isApproved"].ToString().ToUpper() == "FALSE" || dt_.Rows[0]["isApproved"].ToString().ToUpper() == "0")
                            {
                                isApproved = "0";
                            }
                            else
                            {
                                isApproved = "1";
                            }
                            //txt_chargedAmount.Text = double.Parse(dt_.Rows[0]["amount"].ToString()).ToString();
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Tariff Not Found')", true);
                        return;
                    }
                    //txt_totalAmt.Text = double.Parse(dt_.Rows[0]["totalamount"].ToString()) + double.Parse(dt_.Rows[0]["gst"].ToString()).ToString();
                    ////clvar.gst = gst_;
                    //txt_gst.Text = double.Parse(dt_.Rows[0]["gst"].ToString()).ToString();
                    //txt_chargedAmount.Text = double.Parse(dt_.Rows[0]["totalamount"].ToString()).ToString();
                }


                //Confirmation Message  
                //btn_SaveConsignment0.Enabled = true;
                clvar.TotalAmount = Math.Round(TotalAmount, 2);
                clvar.gst = Math.Round(gst_, 2);
                clvar.dayType = 2;
                if (hd_customerType.Value == "1")
                {
                    //clvar.ChargeAmount = double.Parse(txt_chargedAmount.Text);
                }
                else
                {
                    //clvar.ChargeAmount = TotalAmount + gst_;
                }

                //DataTable dt = (DataTable)ViewState["PM"];
                //if (dt.Rows.Count != 0)
                //{
                //    clvar.isPM = true;
                //    clvar.LstModifiersCNt = dt;
                //}
                //else
                //{
                //    clvar.isPM = false;
                //    clvar.LstModifiersCNt = null;
                //}
                clvar.destinationCountryCode = "PAK";

                //COD
                if (hd_COD.Value == "True" || hd_COD.Value == "1")
                {
                    clvar.isCod = bool.Parse(hd_COD.Value);

                    if (ViewState["CODType"].ToString() == "1")
                    {
                        if (txt_codAmount.Text == "0" || txt_codAmount.Text.Trim() == "")
                        {
                            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Provide COD Amount')", true);
                            //return;
                        }
                        clvar.codAmount = float.Parse(txt_codAmount.Text);
                        clvar.calculatedCodAmount = float.Parse(txt_codAmount.Text);
                    }
                    else if (ViewState["CODType"].ToString() == "2")
                    {
                        if (txt_totalAmt.Text == "0" || txt_totalAmt.Text.Trim() == "")
                        {
                            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Provide COD Amount')", true);
                            //return;
                        }

                        clvar.codAmount = float.Parse(txt_totalAmt.Text);
                        clvar.calculatedCodAmount = float.Parse(txt_totalAmt.Text);
                    }
                    else if (ViewState["CODType"].ToString() == "3")
                    {
                        if ((txt_codAmount.Text == "0" || txt_codAmount.Text.Trim() == "") && (txt_totalAmt.Text == "0" || txt_totalAmt.Text.Trim() == ""))
                        {
                            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Provide COD Amount')", true);
                            //return;
                        }
                        clvar.codAmount = float.Parse(txt_totalAmt.Text) + float.Parse(txt_codAmount.Text);
                        clvar.calculatedCodAmount = float.Parse(txt_totalAmt.Text) + float.Parse(txt_codAmount.Text);
                    }

                    clvar.chargeCODAmount = Cb_CODAmount.Checked;
                    clvar.productDescription = txt_descriptionCOD.Text;
                    if (dd_productType.Items.Count > 0)
                    {
                        clvar.productTypeId = int.Parse(dd_productType.SelectedValue);
                    }
                    else
                    {
                        clvar.productTypeId = 0;
                    }
                    clvar.orderRefNo = txt_orderRefNo.Text;

                }


                //Confirmation Message

                //   String Message = "Are you sure you want to save the consignment.\n Gst : " + String.Format("{0:N0}", gst) + ".\n Total Amount : " + String.Format("{0:N0}", TotalAmount) +".";

                //   ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "confirm('" + Message + "');", true);
                //   return;
                clvar.Customertype = int.Parse(hd_customerType.Value);
                if (Session["U_ID"] != null)
                {
                    clvar.createdBy = Session["U_ID"].ToString();
                }
                clvar.CouponNo = txt_coupon.Text;

                DataTable modifiers = new DataTable();

                modifiers.Columns.AddRange(new DataColumn[] {
                new DataColumn("priceModifierId", typeof(Int64)),
                new DataColumn("ConsignmentNumber",typeof(string)),
                new DataColumn("CreatedBy", typeof(string)),
                new DataColumn("CreatedOn", typeof(DateTime)),
                new DataColumn("ModifiedBy", typeof(string)),
                new DataColumn("ModifiedOn", typeof(DateTime)),
                new DataColumn("modifiedCalculationValue", typeof(int)),
                new DataColumn("calculatedValue",typeof(float)),
                new DataColumn("calculatedGST",typeof(float)),
                new DataColumn("calculationBase",typeof(int)),
                new DataColumn("isTaxable",typeof(bool)),
                new DataColumn("SortOrder", typeof(int)),
                new DataColumn("AlternateValue", typeof(string))
            });
                bool isInsured = false;
                foreach (GridViewRow row in gv_CNModifiers.Rows)
                {
                    if (row.Visible == false)
                    {
                        continue;
                    }
                    DataRow dr = modifiers.NewRow();
                    int calBase = int.Parse(row.Cells[3].Text);
                    string isGST = row.Cells[4].Text;
                    dr["priceModifierId"] = Int64.Parse((row.FindControl("btn_gRemove") as Button).CommandArgument.ToString());
                    dr["ConsignmentNumber"] = txt_cnNumber.Text;
                    dr["CreatedBy"] = HttpContext.Current.Session["U_ID"].ToString();
                    dr["CreatedOn"] = DateTime.Now;
                    dr["ModifiedBy"] = DBNull.Value;
                    dr["ModifiedOn"] = DBNull.Value;
                    dr["modifiedCalculationValue"] = float.Parse((row.FindControl("txt_gValue") as TextBox).Text);
                    if (calBase == 1)
                    {
                        dr["calculatedValue"] = Math.Round(float.Parse((row.FindControl("txt_gValue") as TextBox).Text), 2);
                        if (isGST.ToString().ToUpper() == "FALSE")
                        {
                            dr["CalculatedGST"] = "0";
                        }
                        else
                        {
                            dr["calculatedGST"] = Math.Round((double.Parse(gst) / 100) * float.Parse((row.FindControl("txt_gValue") as TextBox).Text), 2);
                        }

                    }
                    else if (calBase == 2)
                    {
                        dr["CalculatedValue"] = Math.Round(((float.Parse((row.FindControl("txt_gValue") as TextBox).Text) / 100) * TotalAmount), 2);
                        if (isGST.ToString().ToUpper() == "FALSE")
                        {
                            dr["CalculatedGST"] = "0";
                        }
                        else
                        {
                            dr["calculatedGST"] = Math.Round((double.Parse(gst) / 100) * ((float.Parse((row.FindControl("txt_gValue") as TextBox).Text) / 100) * TotalAmount), 2);
                        }
                    }
                    else if (calBase == 3)
                    {
                        float insuranceValue = float.Parse((row.FindControl("txt_gValue") as TextBox).Text);
                        float DeclaredValue = float.Parse((row.FindControl("txt_gDeclaredValue") as TextBox).Text);
                        dr["CalculatedValue"] = Math.Round((insuranceValue / 100) * DeclaredValue);
                        dr["CalculatedGST"] = 0;
                        dr["AlternateValue"] = DeclaredValue;
                        isInsured = true;
                    }
                    dr["calculationBase"] = calBase;
                    dr["isTaxable"] = DBNull.Value;
                    dr["SortOrder"] = int.Parse((row.FindControl("hd_sortOrder") as HiddenField).Value);
                    modifiers.Rows.Add(dr);
                }
                if (modifiers.Rows.Count != 0)
                {
                    clvar.isPM = true;
                    clvar.LstModifiersCNt = modifiers;
                    clvar.codAmount = float.Parse(modifiers.Compute("Sum(CalculatedGST)", string.Empty).ToString()) + float.Parse(modifiers.Compute("Sum(CalculatedValue)", string.Empty).ToString());
                }
                else
                {
                    clvar.isPM = false;
                    clvar.LstModifiersCNt = null;
                }

                clvar.isInsured = isInsured;


                string error = Add_Consignment(clvar, denseWeight, txt_remarks.Text, isPriceComputed, isApproved);
                if (error != "")
                {
                    Errorid.Text = error;
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Could not Save Consignment.')", true);
                    return;
                }

                if (txt_accountNo.Text.Trim() == "0")
                {
                    clvar.ChargeAmount = float.Parse(txt_chargedAmount.Text);
                    clvar.consignmentNo = txt_cnNumber.Text;
                    updateChargedAmount(clvar);
                }


                DataTable dtc = (DataTable)ViewState["PM"];
                error = "";


                error = Add_ConsignmentModifier(modifiers, clvar);

                if (error == "")
                {
                    if (ViewState["CNState"].ToString() == "NEW")
                    {
                        Post_BrandedSMS(txt_consignerCell.Text, txt_cnNumber.Text, txt_consigner.Text.ToUpper(), dd_destination.SelectedItem.Text);
                        //if (txt_SmsConsignment.Checked == true)
                        //{
                        Post_BrandedSMS_(txt_consigneeCell.Text, txt_cnNumber.Text, txt_consignee.Text.ToUpper(), dd_destination.SelectedItem.Text);
                        //}
                    }
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Consignment Has Been Saved.')", true);
                    //   return;

                    #region

                    //string script_ = @"window.open(""{0}"", ""{1}"", ""{2}"");";

                    //string script = String.Format(script_, "invoicepdf-2.aspx?id=" + txt_ConNo.Text, "_blank", "");

                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Redirect", script, true);

                    #endregion

                    if (chk_bulkUpdate.Checked)
                    {
                        clvar.ClvarListStr = ViewState["bulkRange"] as List<string>;
                        clvar.ClvarListStr.RemoveAt(0);
                        if (clvar.ClvarListStr.Count > 0)
                        {
                            txt_cnNumber.Text = clvar.ClvarListStr[0].ToString();
                            txt_cnNumber_TextChanged(sender, e);
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Bulk Consignment Saved')", true);
                            btn_reset_Click(sender, e);
                            txt_cnNumber.Focus();

                            if (cb_checkbox.Checked == true)
                            {
                                ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "window.open('CN_Import_Print.aspx?Param=" + txt_cnNumber.Text + "');", true);
                            }
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Consignment Saved')", true);
                        if (cb_checkbox.Checked == true)
                        {
                            ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "window.open('CN_Import_Print.aspx?Param=" + txt_cnNumber.Text + "');", true);
                        }
                        btn_reset_Click(sender, e);
                        txt_cnNumber.Focus();
                        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Bulk Consignment Saved')", true);
                    }
                }
                else
                {
                    Errorid.Text = error;
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Consignment Could not Save. Please Contact IT Support. Error: " + error + "')", true);
                    return;
                }
                return;
            }
            catch (Exception Err)
            {
                Errorid.Text = Err.Message.ToString();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cannot Save Consignment. Error: " + Err.Message + "')", true);
            }
            finally
            {

            }


        }

        public string Add_Consignment_Validation(Cl_Variables obj)
        {
            clvar.Error = "";
            int IsUnique = 0;
            SqlConnection sqlcon = new SqlConnection(obj.Strcon());
            //    obj.Call_Log_ID = int.Parse(Get_maxValue()) + 1;
            try
            {
                sqlcon.Open();
                SqlCommand sqlcmd = new SqlCommand("MNP_InsertConsignment_import_Temp_Validation", sqlcon);
                // SqlCommand sqlcmd = new SqlCommand("MNP_InsertConsignment_Temp", sqlcon);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                // SqlParameter SParam = new SqlParameter("ReturnValue", SqlDbType.Int);
                // SParam.Direction = ParameterDirection.ReturnValue;
                //
                sqlcmd.Parameters.AddWithValue("@consignmentNumber", obj.consignmentNo);
                sqlcmd.Parameters.AddWithValue("@consigner", obj.Consigner);
                sqlcmd.Parameters.AddWithValue("@consignee", obj.Consignee);
                sqlcmd.Parameters.AddWithValue("@couponNumber", obj.CouponNo);
                sqlcmd.Parameters.AddWithValue("@customerType", obj.Customertype);
                sqlcmd.Parameters.AddWithValue("@orgin", obj.origin);
                sqlcmd.Parameters.AddWithValue("@destination", obj.Destination);
                sqlcmd.Parameters.AddWithValue("@pieces", obj.pieces);
                sqlcmd.Parameters.AddWithValue("@serviceTypeName", obj.ServiceTypeName);
                sqlcmd.Parameters.AddWithValue("@creditClientId", obj.CustomerClientID);
                sqlcmd.Parameters.AddWithValue("@consignmentTypeId", obj.Con_Type);
                sqlcmd.Parameters.AddWithValue("@weight", obj.Weight);
                sqlcmd.Parameters.AddWithValue("@weightUnit", obj.Unit);
                sqlcmd.Parameters.AddWithValue("@discount", obj.Discount);
                sqlcmd.Parameters.AddWithValue("@cod", obj.isCod);
                sqlcmd.Parameters.AddWithValue("@address", obj.ConsigneeAddress);
                sqlcmd.Parameters.AddWithValue("@createdBy", obj.createdBy);
                sqlcmd.Parameters.AddWithValue("@status", obj.status);
                sqlcmd.Parameters.AddWithValue("@totalAmount", obj.TotalAmount);
                sqlcmd.Parameters.AddWithValue("@zoneCode", obj.Zone);
                sqlcmd.Parameters.AddWithValue("@branchCode", obj.origin);
                sqlcmd.Parameters.AddWithValue("@expressCenterCode", obj.expresscenter);
                sqlcmd.Parameters.AddWithValue("@isCreatedFromMMU", obj.isCreatedFromMMUs);
                sqlcmd.Parameters.AddWithValue("@deliveryType", obj.deliveryType);
                sqlcmd.Parameters.AddWithValue("@remarks", obj.Remarks);
                sqlcmd.Parameters.AddWithValue("@shipperAddress", obj.ConsignerAddress);
                sqlcmd.Parameters.AddWithValue("@riderCode", obj.RiderCode);
                sqlcmd.Parameters.AddWithValue("@gst", obj.gst);
                sqlcmd.Parameters.AddWithValue("@width", obj.width);
                sqlcmd.Parameters.AddWithValue("@breadth", obj.breadth);
                sqlcmd.Parameters.AddWithValue("@height", obj.height);
                sqlcmd.Parameters.AddWithValue("@PakageContents", obj.PakageContents);
                sqlcmd.Parameters.AddWithValue("@expressionDeliveryDateTime", obj.expressionDeliveryDateTime);
                sqlcmd.Parameters.AddWithValue("@expressionGreetingCard", obj.expressionGreetingCard);
                sqlcmd.Parameters.AddWithValue("@expressionMessage", obj.expressionMessage);
                sqlcmd.Parameters.AddWithValue("@consigneePhoneNo", obj.ConsigneeCell);
                sqlcmd.Parameters.AddWithValue("@expressionconsignmentRefNumber", obj.expressionconsignmentRefNumber);
                sqlcmd.Parameters.AddWithValue("@otherCharges", obj.Othercharges);
                sqlcmd.Parameters.AddWithValue("@routeCode", obj.routeCode);
                sqlcmd.Parameters.AddWithValue("@docPouchNo", obj.docPouchNo);
                sqlcmd.Parameters.AddWithValue("@consignerPhoneNo", obj.ConsignerPhone);
                sqlcmd.Parameters.AddWithValue("@consignerCellNo", obj.ConsignerCell);
                sqlcmd.Parameters.AddWithValue("@consignerCNICNo", obj.ConsignerCNIC);
                sqlcmd.Parameters.AddWithValue("@consignerAccountNo", obj.consignerAccountNo);
                sqlcmd.Parameters.AddWithValue("@consignerEmail", obj.consignerEmail);
                sqlcmd.Parameters.AddWithValue("@docIsHomeDelivery", obj.docIsHomeDelivery);
                sqlcmd.Parameters.AddWithValue("@cutOffTime", obj.cutOffTime);
                sqlcmd.Parameters.AddWithValue("@destinationCountryCode", obj.destinationCountryCode);
                sqlcmd.Parameters.AddWithValue("@decalaredValue", obj.Declaredvalue);
                sqlcmd.Parameters.AddWithValue("@insuarancePercentage", obj.insuarancePercentage);
                sqlcmd.Parameters.AddWithValue("@consignmentScreen", "0");
                sqlcmd.Parameters.AddWithValue("@isInsured", obj.isInsured);
                sqlcmd.Parameters.AddWithValue("@isReturned", obj.isReturned);
                sqlcmd.Parameters.AddWithValue("@consigneeCNICNo", obj.ConsigneeCNIC);
                sqlcmd.Parameters.AddWithValue("@cutOffTimeShift", obj.cutOffTimeShift);
                sqlcmd.Parameters.AddWithValue("@bookingDate", obj.Bookingdate);
                sqlcmd.Parameters.AddWithValue("@cnClientType", obj.cnClientType);
                sqlcmd.Parameters.AddWithValue("@destinationExpressCenterCode", obj.destinationExpressCenterCode);
                sqlcmd.Parameters.AddWithValue("@dayType", obj.dayType);
                sqlcmd.Parameters.AddWithValue("@originExpressCenter", obj.expresscenter);
                sqlcmd.Parameters.AddWithValue("@receivedFromRider", obj.receivedFromRider);
                sqlcmd.Parameters.AddWithValue("@chargedAmount", obj.ChargeAmount);
                sqlcmd.Parameters.AddWithValue("@cnState", obj.cnState);
                sqlcmd.Parameters.AddWithValue("@cardRefNo", obj.cardRefNo);
                sqlcmd.Parameters.AddWithValue("@manifestNo", obj.manifestNo);
                sqlcmd.Parameters.AddWithValue("@manifestId", obj.manifestId);
                sqlcmd.Parameters.AddWithValue("@originBrName", obj.originBrName);
                sqlcmd.Parameters.AddWithValue("@orderRefNo", obj.orderRefNo);
                sqlcmd.Parameters.AddWithValue("@customerName", obj.customerName);
                sqlcmd.Parameters.AddWithValue("@productTypeId", obj.productTypeId);
                sqlcmd.Parameters.AddWithValue("@productDescription", obj.productDescription);
                sqlcmd.Parameters.AddWithValue("@chargeCODAmount", obj.chargeCODAmount);
                sqlcmd.Parameters.AddWithValue("@codAmount", obj.codAmount);
                //sqlcmd.Parameters.AddWithValue("@isPM", obj.isPM);
                //sqlcmd.Parameters.AddWithValue("@LstModifiersCN", obj.LstModifiersCNt);
                sqlcmd.Parameters.AddWithValue("@serviceTypeId", obj.serviceTypeId);
                sqlcmd.Parameters.AddWithValue("@originId", obj.originId);
                sqlcmd.Parameters.AddWithValue("@destId", obj.destId);
                sqlcmd.Parameters.AddWithValue("@cnTypeId", obj.cnTypeId);
                sqlcmd.Parameters.AddWithValue("@cnOperationalType", obj.cnOperationalType);
                sqlcmd.Parameters.AddWithValue("@cnScreenId", obj.cnScreenId);
                sqlcmd.Parameters.AddWithValue("@cnStatus", obj.cnStatus);
                sqlcmd.Parameters.AddWithValue("@flagSendConsigneeMsg", obj.flagSendConsigneeMsg);
                sqlcmd.Parameters.AddWithValue("@isExpUser", obj.isExpUser);
                sqlcmd.Parameters.AddWithValue("@isInt", obj.isExpUser);
                //     sqlcmd.Parameters.AddWithValue("@OriginCountry", obj.OriginCountry);
                //     sqlcmd.Parameters.AddWithValue("@IsImport", obj.IsImport);

                sqlcmd.ExecuteNonQuery();
                sqlcon.Close();
            }
            catch (Exception Err)
            {
                clvar.Error = Err.Message.ToString();
                /// IsUnique = 1;
            }
            finally
            { }
            //return IsUnique;
            return clvar.Error;
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

            sqlString = "select b.branchCode , b.sname + ' - ' + b.name SNAME, ec.ExpressCenterCode ECCode\n" +
            "  from branches b\n" +
            "  left outer join ExpressCenters ec\n" +
            "    on ec.bid = b.branchCode\n" +
            "   and ec.Main_EC = '1'\n" +
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

        public string Add_ConsignmentModifier(DataTable dt, Cl_Variables clvar)
        {
            clvar.Error = "";



            using (SqlConnection con = new SqlConnection(clvar.Strcon()))
            {
                using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                {
                    try
                    {


                        //Set the database table name
                        SqlCommand cmd = new SqlCommand("DElete from ConsignmentModifier where consignmentNumber = '" + clvar.consignmentNo + "'", con);
                        cmd.Connection = con;

                        sqlBulkCopy.DestinationTableName = "dbo.ConsignmentModifier";

                        //[OPTIONAL]: Map the DataTable columns with that of the database table
                        //sqlBulkCopy.ColumnMappings.Add("PriceModifierID", "");
                        //sqlBulkCopy.ColumnMappings.Add("Name", "Name");
                        //sqlBulkCopy.ColumnMappings.Add("Country", "Country");
                        con.Open();
                        cmd.ExecuteNonQuery();
                        if (dt.Rows.Count > 0)
                        {
                            sqlBulkCopy.WriteToServer(dt);
                        }

                        con.Close();
                    }
                    catch (Exception ex)
                    {
                        clvar.Error = ex.Message;
                    }
                }

            }
            return clvar.Error;
        }

        public string Add_Consignment(Cl_Variables obj, float denseWeight, string remarks, string isPriceComputed, string isApproved)
        {
            clvar.Error = "";
            int IsUnique = 0;
            SqlConnection sqlcon = new SqlConnection(obj.Strcon());
            //    obj.Call_Log_ID = int.Parse(Get_maxValue()) + 1;
            try
            {
                sqlcon.Open();
                SqlCommand sqlcmd = new SqlCommand("MNP_InsertConsignment_Import_Temp2", sqlcon);
                // SqlCommand sqlcmd = new SqlCommand("MNP_InsertConsignment_Temp", sqlcon);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                // SqlParameter SParam = new SqlParameter("ReturnValue", SqlDbType.Int);
                // SParam.Direction = ParameterDirection.ReturnValue;
                //
                sqlcmd.Parameters.AddWithValue("@consignmentNumber", obj.consignmentNo);
                sqlcmd.Parameters.AddWithValue("@consigner", obj.Consigner);
                sqlcmd.Parameters.AddWithValue("@consignee", obj.Consignee);
                sqlcmd.Parameters.AddWithValue("@couponNumber", obj.CouponNo);
                sqlcmd.Parameters.AddWithValue("@customerType", obj.Customertype);
                sqlcmd.Parameters.AddWithValue("@orgin", obj.origin);
                sqlcmd.Parameters.AddWithValue("@destination", obj.Destination);
                sqlcmd.Parameters.AddWithValue("@pieces", obj.pieces);
                sqlcmd.Parameters.AddWithValue("@serviceTypeName", obj.ServiceTypeName);
                sqlcmd.Parameters.AddWithValue("@creditClientId", obj.CustomerClientID);
                sqlcmd.Parameters.AddWithValue("@consignmentTypeId", obj.Con_Type);
                sqlcmd.Parameters.AddWithValue("@weight", obj.Weight);
                sqlcmd.Parameters.AddWithValue("@weightUnit", obj.Unit);
                sqlcmd.Parameters.AddWithValue("@discount", obj.Discount);
                sqlcmd.Parameters.AddWithValue("@cod", obj.isCod);
                sqlcmd.Parameters.AddWithValue("@address", obj.ConsigneeAddress);
                sqlcmd.Parameters.AddWithValue("@createdBy", obj.createdBy);
                sqlcmd.Parameters.AddWithValue("@status", obj.status);
                sqlcmd.Parameters.AddWithValue("@totalAmount", obj.TotalAmount);
                sqlcmd.Parameters.AddWithValue("@zoneCode", obj.Zone);
                sqlcmd.Parameters.AddWithValue("@branchCode", obj.origin);
                sqlcmd.Parameters.AddWithValue("@expressCenterCode", obj.OriginExpressCenterCode);
                sqlcmd.Parameters.AddWithValue("@isCreatedFromMMU", obj.isCreatedFromMMUs);
                sqlcmd.Parameters.AddWithValue("@deliveryType", obj.deliveryType);
                sqlcmd.Parameters.AddWithValue("@remarks", remarks/* obj.Remarks*/);
                sqlcmd.Parameters.AddWithValue("@shipperAddress", obj.ConsignerAddress);
                sqlcmd.Parameters.AddWithValue("@riderCode", obj.RiderCode);
                sqlcmd.Parameters.AddWithValue("@gst", obj.gst);
                sqlcmd.Parameters.AddWithValue("@width", obj.width);
                sqlcmd.Parameters.AddWithValue("@breadth", obj.breadth);
                sqlcmd.Parameters.AddWithValue("@height", obj.height);
                sqlcmd.Parameters.AddWithValue("@PakageContents", obj.PakageContents);
                sqlcmd.Parameters.AddWithValue("@expressionDeliveryDateTime", obj.expressionDeliveryDateTime);
                sqlcmd.Parameters.AddWithValue("@expressionGreetingCard", obj.expressionGreetingCard);
                sqlcmd.Parameters.AddWithValue("@expressionMessage", obj.expressionMessage);
                sqlcmd.Parameters.AddWithValue("@consigneePhoneNo", obj.ConsigneeCell);
                sqlcmd.Parameters.AddWithValue("@expressionconsignmentRefNumber", obj.expressionconsignmentRefNumber);
                sqlcmd.Parameters.AddWithValue("@otherCharges", obj.Othercharges);
                sqlcmd.Parameters.AddWithValue("@routeCode", obj.routeCode);
                sqlcmd.Parameters.AddWithValue("@docPouchNo", obj.docPouchNo);
                sqlcmd.Parameters.AddWithValue("@consignerPhoneNo", obj.ConsignerPhone);
                sqlcmd.Parameters.AddWithValue("@consignerCellNo", obj.ConsignerCell);
                sqlcmd.Parameters.AddWithValue("@consignerCNICNo", obj.ConsignerCNIC);
                sqlcmd.Parameters.AddWithValue("@consignerAccountNo", obj.consignerAccountNo);
                sqlcmd.Parameters.AddWithValue("@consignerEmail", obj.consignerEmail);
                sqlcmd.Parameters.AddWithValue("@docIsHomeDelivery", obj.docIsHomeDelivery);
                sqlcmd.Parameters.AddWithValue("@cutOffTime", obj.cutOffTime);
                sqlcmd.Parameters.AddWithValue("@destinationCountryCode", obj.destinationCountryCode);
                sqlcmd.Parameters.AddWithValue("@decalaredValue", obj.Declaredvalue);
                sqlcmd.Parameters.AddWithValue("@insuarancePercentage", obj.insuarancePercentage);
                sqlcmd.Parameters.AddWithValue("@consignmentScreen", "9");
                sqlcmd.Parameters.AddWithValue("@isInsured", obj.isInsured);
                sqlcmd.Parameters.AddWithValue("@isReturned", obj.isReturned);
                sqlcmd.Parameters.AddWithValue("@consigneeCNICNo", obj.ConsigneeCNIC);
                sqlcmd.Parameters.AddWithValue("@cutOffTimeShift", obj.cutOffTimeShift);
                sqlcmd.Parameters.AddWithValue("@bookingDate", obj.Bookingdate);
                sqlcmd.Parameters.AddWithValue("@cnClientType", obj.cnClientType);
                sqlcmd.Parameters.AddWithValue("@destinationExpressCenterCode", obj.destinationExpressCenterCode);
                sqlcmd.Parameters.AddWithValue("@dayType", obj.dayType);
                sqlcmd.Parameters.AddWithValue("@originExpressCenter", obj.OriginExpressCenterCode);
                sqlcmd.Parameters.AddWithValue("@receivedFromRider", obj.receivedFromRider);
                sqlcmd.Parameters.AddWithValue("@chargedAmount", obj.ChargeAmount);
                sqlcmd.Parameters.AddWithValue("@cnState", obj.cnState);
                sqlcmd.Parameters.AddWithValue("@cardRefNo", obj.cardRefNo);
                sqlcmd.Parameters.AddWithValue("@manifestNo", obj.manifestNo);
                sqlcmd.Parameters.AddWithValue("@manifestId", obj.manifestId);
                sqlcmd.Parameters.AddWithValue("@originBrName", obj.originBrName);
                sqlcmd.Parameters.AddWithValue("@orderRefNo", obj.orderRefNo);
                sqlcmd.Parameters.AddWithValue("@customerName", obj.customerName);
                sqlcmd.Parameters.AddWithValue("@productTypeId", obj.productTypeId);
                sqlcmd.Parameters.AddWithValue("@productDescription", obj.productDescription);
                sqlcmd.Parameters.AddWithValue("@chargeCODAmount", obj.chargeCODAmount);
                sqlcmd.Parameters.AddWithValue("@codAmount", obj.codAmount);
                sqlcmd.Parameters.AddWithValue("@serviceTypeId", obj.serviceTypeId);
                sqlcmd.Parameters.AddWithValue("@originId", obj.originId);
                sqlcmd.Parameters.AddWithValue("@destId", obj.destId);
                sqlcmd.Parameters.AddWithValue("@cnTypeId", obj.cnTypeId);
                sqlcmd.Parameters.AddWithValue("@cnOperationalType", obj.cnOperationalType);
                sqlcmd.Parameters.AddWithValue("@cnScreenId", obj.cnScreenId);
                sqlcmd.Parameters.AddWithValue("@cnStatus", obj.cnStatus);
                sqlcmd.Parameters.AddWithValue("@flagSendConsigneeMsg", obj.flagSendConsigneeMsg);
                sqlcmd.Parameters.AddWithValue("@isExpUser", obj.isExpUser);
                sqlcmd.Parameters.AddWithValue("@isInt", obj.isExpUser);
                sqlcmd.Parameters.AddWithValue("@accountReceivingDate", picker_reportingDate.SelectedDate.Value.ToString("yyyy-MM-dd"));
                sqlcmd.Parameters.AddWithValue("@denseWeight", denseWeight);
                sqlcmd.Parameters.AddWithValue("@isApproved", isApproved);
                sqlcmd.Parameters.AddWithValue("@ispriceComputed", isPriceComputed);
                sqlcmd.Parameters.AddWithValue("@InsertType", 1);
                sqlcmd.Parameters.AddWithValue("@OriginCountry", obj.OriginCountry);
                sqlcmd.Parameters.AddWithValue("@IsImport", obj.IsImport);

                sqlcmd.ExecuteNonQuery();
                sqlcon.Close();
            }
            catch (Exception Err)
            {
                clvar.Error = Err.Message.ToString();
            }
            finally
            { }
            //return IsUnique;
            return clvar.Error;
        }

        public DataSet CustomerInformation(Cl_Variables clvar)
        {
            DataSet Ds_1 = new DataSet();

            try
            {
                string query = "SELECT * FROM CreditClients cc WHERE cc.accountNo = '" + clvar.AccountNo + "' and cc.branchCode='" + clvar.Branch + "' ";

                string sqlString = "SELECT cc.*,\n" +
                "       case\n" +
                "         when cu.CreditClientID is not null then\n" +
                "          '1'\n" +
                "         else\n" +
                "          '0'\n" +
                "       end isAPIClient\n" +
                "  FROM CreditClients cc\n" +
                "  left outer join CODUsers cu\n" +
                "    --on cu.accountno = cc.accountno\n" +
                "   on cu.creditCLientID = cc.id and cu.isCod = '1'\n" +
                " where cc.accountNo = '" + clvar.AccountNo + "'\n" +
                "   and cc.branchCode = '" + clvar.Branch + "'\n" +
                "   and cc.isActive = '1'  and import <> '0'";


                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
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

        public string updateChargedAmount(Cl_Variables clvar)
        {
            string query = "UPDATE consignment set chargedAmount = '" + txt_chargedAmount.Text + "' where consignmentNumber = '" + clvar.consignmentNo + "'";

            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally { con.Close(); }
            return "";
        }

        public DataTable SequenceCheck(Cl_Variables clvar, string specialCondition)
        {
            string sql = "SELECT * \n"
                + "FROM Mnp_ZoneCNSquence mzc \n"
                + "WHERE   mzc.ZoneCode='" + clvar.Zone + "' and Product ='Import' AND \n"
                + "       '" + clvar.consignmentNo + "' BETWEEN mzc.SequenceStart AND mzc.EndSequence " + specialCondition + "";

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sql, con);
                sda.SelectCommand.CommandTimeout = 120;
                sda.Fill(dt);

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return dt;

        }

        public DataTable GetCODConsignmentForApproval(Cl_Variables clvar)
        {

            string sqlString = "select c.bookingDate,\n" +
            "       c.consignmentNumber,\n" +
            "       c.customerType,\n" +
            "       c.creditClientId,\n" +
            "       c.orgin,\n" +
            "       c.serviceTypeName,\n" +
            "       c.consigner,\n" +
            "       c.consignee,\n" +
            "       c.destination,\n" +
            "       c.weight,\n" +
            "       c.riderCode,\n" +
            "       c.originExpressCenter,\n" +
            "       c.consignmentTypeId,\n" +
            "       c.chargedAmount,\n" +
            "       c.isApproved,\n" +
            "       ic.invoiceNumber,\n" +
            "       i.startDate ReportingDate,\n" +
            "       i.deliveryStatus,\n" +
            "       cm.priceModifierId,\n" +
            "       p.name,\n" +
            "       cm.calculatedValue,\n" +
            "       cm.calculationBase,\n" +
            "       cm.isTaxable,\n" +
            "       cm.SortOrder,\n" +
            "       p.description, c.destinationExpressCenterCode\n" +
            "\n" +
            "  from consignment c\n" +
            " inner join InvoiceConsignment ic\n" +
            "    on c.consignmentNumber = ic.consignmentNumber\n" +
            " inner join Invoice i\n" +
            "    on i.invoiceNumber = ic.invoiceNumber\n" +
            "  left outer join ConsignmentModifier cm\n" +
            "    on c.consignmentNumber = cm.consignmentNumber\n" +
            "  left outer join PriceModifiers p\n" +
            "    on cm.priceModifierId = p.id\n" +
            " where c.orgin = '4'\n" +
            "   and c.consignmentTypeId <> '10'\n" +
            "   and cm.priceModifierId is not null\n" +
            "   and c.consignmentNumber = '" + clvar.consignmentNo + "'\n" +
            " order by consignmentNumber, SortOrder";

            sqlString = "SELECT * FROM CODConsignmentDetail cd WHERE cd.consignmentNumber='" + clvar.consignmentNo + "'";

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

        public DataSet Check_CODConsignmentDetail(Cl_Variables clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string query = "select * from CODConsignmentDetail_new where consignmentNumber = '" + clvar.consignmentNo + "' ";

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

            }
            finally
            { }
            return ds;
        }

        public void AlertMessage(string message)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + message + "')", true);
            return;
        }

        private string IsMobileNumberValid(string mobileNumber)
        {
            string _mobileNumber = "";
            // remove all non-numeric characters
            _mobileNumber = CleanNumber(mobileNumber);

            // trim any leading zeros
            _mobileNumber = _mobileNumber.TrimStart(new char[] { '0' });

            // check for this in case they've entered 44 (0)xxxxxxxxx or similar
            if (_mobileNumber.StartsWith("920"))
            {
                _mobileNumber = _mobileNumber.Remove(2, 1);
            }

            // add country code if they haven't entered it
            if (!_mobileNumber.StartsWith("92"))
            {
                _mobileNumber = "92" + _mobileNumber;
            }

            // check if it's the right length
            if (_mobileNumber.Length != 12)
            {
                return "0";
            }

            return _mobileNumber;
        }
        private string CleanNumber(string phone)
        {
            Regex digitsOnly = new Regex(@"[^\d]");
            return digitsOnly.Replace(phone, "");
        }
        public void Post_BrandedSMS(string mobile, string resp, string Consignee, string destination)
        {
            try
            {
                // Create a request using a URL that can receive a post. 
                //   WebRequest request = WebRequest.Create("https://services.converget.com/MTEntryDyn/?");
                // Set the Method property of the request to POST.

                // Create POST data and convert it to a byte array.
                if (txt_cnNumber.Text != string.Empty)
                {
                    //  DataTable dt = clvar.GetMaxOrderNo();
                    string newResp = "Dear Customer, A shipment CN " + resp + " is booked successfully. You can visit www.mulphilog.com or call us on 111-202-202 to track delivery status. Thank you";
                    newResp = "Thank you for choosing M&P Courier. Your shipment with CN #: " + resp + " is booked successfully. Track it at www.mulphilog.com or call 111 202 202.";
                    newResp = newResp.Replace("&", "%26");
                    newResp = newResp.Replace("#", "%23");
                    //string resp_ = "Dear Valued Customer, We have received your shipment under CN:" + resp + "for " + Consignee + " - " + destination + " Amount :" + string.Format("{0:N0}", Double.Parse(txt_TotalAmount.Text)) + ". Please visit www.mulphilog.com or call us on 021-111-202-202 to track delivery status. Thank you";
                    String Mobile = HttpUtility.UrlEncode(IsMobileNumberValid(mobile));
                    String Response = HttpUtility.UrlEncode(resp);

                    string postData = "";//"to=" + Mobile + "&text=" + Response + "&from=OCS&username=sales&password=salestest8225";//"PhoneNumber=" + Mobile + "&Text=" + Response;
                    string smsContent = newResp;//"Dear Customer, M&P Courier visited your address to deliver " + hd_consigner.Value + " shipment under CN #" + row.Cells[1].Text + " but it could not deliver. Pls contact M&P Customer Services 111-202-202";
                    string query2 = "insert into mnp_smsStatus (ConsignmentNumber, Recepient, MessageContent, Status, Createdon, CreatedBy, RunsheetNumber) \n" +
                             "values ('" + resp + "', '" + mobile + "', '" + smsContent + "', '0', GETDATE(), '" + HttpContext.Current.Session["U_ID"].ToString() + "', 'N/A/A')";

                    int count = 0;
                    string error = "";
                    SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
                    try
                    {
                        sqlcon.Open();
                        SqlCommand sqlcmd = new SqlCommand(query2, sqlcon);
                        sqlcmd.CommandType = CommandType.Text;

                        count = sqlcmd.ExecuteNonQuery();
                        //IsUnique = Int32.Parse(SParam.Value.ToString());
                        // obj.XCode = obj.consignmentNo;
                        sqlcon.Close();
                    }
                    catch (Exception ex)
                    {
                        error = ex.Message;
                    }

                }
            }
            catch (Exception Err)
            {
                //listBox1.Items.Add("Failed " + Err + "Mobile Number " + clvar.MobileNumber + " and Response " + resp);
            }

        }
        public void Post_BrandedSMS_(string mobile, string resp, string Consignee, string destination)
        {
            try
            {
                // Create a request using a URL that can receive a post. 
                //   WebRequest request = WebRequest.Create("https://services.converget.com/MTEntryDyn/?");
                // Set the Method property of the request to POST.

                // Create POST data and convert it to a byte array.
                if (txt_cnNumber.Text != string.Empty)
                {
                    //  DataTable dt = clvar.GetMaxOrderNo();
                    string newResp = "Dear Customer, Your shipment CN " + resp + " is received on " + DateTime.Now.Date.ToString("yyyy-MM-dd") + ". Thank You. For further details, contact us on 111-202-202";
                    newResp = "Dear Customer, M&P Courier just received a shipment with CN #: " + resp + " for delivery to you. Track it at www.mulphilog.com or call 111 202 202. ";
                    newResp = newResp.Replace("&", "%26");
                    newResp = newResp.Replace("#", "%23");
                    string resp_ = "Dear Customer, A shipment has been booked under CN:" + resp + "for " + Consignee + " - " + destination + ".You can visit www.ocs.com.pk or call us on 021-111 202 202 to track delivery status. Thank you";
                    String Mobile = HttpUtility.UrlEncode(IsMobileNumberValid(mobile));

                    string smsContent = resp_;//"Dear Customer, M&P Courier visited your address to deliver " + hd_consigner.Value + " shipment under CN #" + row.Cells[1].Text + " but it could not deliver. Pls contact M&P Customer Services 111-202-202";
                    string query2 = "insert into mnp_smsStatus (ConsignmentNumber, Recepient, MessageContent, Status, Createdon, CreatedBy, RunsheetNumber) \n" +
                             "values ('" + resp + "', '" + mobile + "', '" + smsContent + "', '0', GETDATE(), '" + HttpContext.Current.Session["U_ID"].ToString() + "', 'N/A/A')";

                    int count = 0;
                    string error = "";
                    SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
                    try
                    {
                        sqlcon.Open();
                        SqlCommand sqlcmd = new SqlCommand(query2, sqlcon);
                        sqlcmd.CommandType = CommandType.Text;

                        count = sqlcmd.ExecuteNonQuery();
                        //IsUnique = Int32.Parse(SParam.Value.ToString());
                        // obj.XCode = obj.consignmentNo;
                        sqlcon.Close();
                    }
                    catch (Exception ex)
                    {
                        error = ex.Message;
                    }

                }
            }
            catch (Exception Err)
            {
                //listBox1.Items.Add("Failed " + Err + "Mobile Number " + clvar.MobileNumber + " and Response " + resp);
            }

        }

        public string GetDayEndTime()
        {
            DataTable dt = new DataTable();
            string time = "";
            string query = "select * from MnP_DayEnd_Timings mdt \n" +
                           " where mdt.ZoneCode = '" + HttpContext.Current.Session["ZoneCode"].ToString() + "' \n" +
                           "   AND mdt.BranchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "' \n" +
                           "   AND mdt.status = '1'\n" +
                           "   AND mdt.doc_type = 'O'";
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    time = dt.Rows[0]["DayEndTime"].ToString();
                }
                else
                {
                    time = "";
                }
            }
            catch (Exception ex)
            { time = ""; }
            finally { con.Close(); }
            return time;
        }

        public DataTable Add_OcsValidation(Cl_Variables clvar)
        {
            DataSet ds = new DataSet();

            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand("SP_UpdateCNPrice_OCS_Validation", orcl);
                orcd.Parameters.AddWithValue("@ConsignmentNo", clvar.consignmentNo);
                orcd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                orcl.Close();
            }
            catch (Exception)
            { }

            return ds.Tables[0];
        }

        public DataTable GetECBySequence(string consignmentNumber)
        {

            string sqlString = "SELECT *\n" +
            "  FROM Mnp_RiderCNSequence mrc\n" +
            " WHERE '" + consignmentNumber + "' BETWEEN mrc.SequenceStart AND mrc.SequenceEnd\n" +
            "   AND mrc.Branch = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'";

            sqlString = "SELECT mrc.*, ec.name ECName\n" +
            "  FROM Mnp_RiderCNSequence mrc\n" +
            " INNER JOIN ExpressCenters ec\n" +
            "    ON ec.expressCenterCode = mrc.ExpressCenter\n" +
            "   AND ec.bid = mrc.Branch\n" +
            " WHERE '" + consignmentNumber + "' BETWEEN mrc.SequenceStart AND mrc.SequenceEnd";

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

        protected void cb_checkbox_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_checkbox.Checked == true)
            {
                codTable.Visible = true;
            }
            else
            {
                codTable.Visible = false;

                txt_codAmount.Text = "0";
                dd_productType.SelectedValue = "0";
                txt_orderRefNo.Text = "";
                txt_descriptionCOD.Text = "";
                gv_CNModifiers.DataSource = null;
                gv_CNModifiers.DataBind();
                dd_priceModifier.ClearSelection();

                DataTable pm = new DataTable();
                pm.Columns.Add("priceModifierId");
                pm.Columns.Add("priceModifierName");
                pm.Columns.Add("calculatedValue");
                pm.Columns.Add("ModifiedCalculatedValue");
                pm.Columns.Add("calculationBase");
                pm.Columns.Add("isTaxable");
                pm.Columns.Add("description");
                pm.Columns.Add("SortOrder");
                pm.Columns.Add("NEW");
                pm.AcceptChanges();
                ViewState["pm"] = pm;
                gv_CNModifiers.DataSource = pm;
                gv_CNModifiers.DataBind();

            }
        }
    }
}