using Dapper;
using MRaabta.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace MRaabta.Files
{
    public partial class CardConsignmentWithoutData : System.Web.UI.Page
    {
        CommonFunction CF = new CommonFunction();
        Consignemnts con = new Consignemnts();
        Cl_Variables clvar = new Cl_Variables();
        bayer_Function b_fun = new bayer_Function();
        Variable Vclvar = new Variable();

        string consignmentNo, referenceNo, Consignee, OrgName, address, orgin, ridercode, CreditClientID, Destination, ServiceType, Consigner;
        string accountNo, Weight, COD, ExpressCenterCode, ZoneCode, BranchCode, BookingDate, ManifestNo, consignee_new, address_new, Con_Length;

        protected void Page_Load(object sender, EventArgs e)
        {

            //IsNumeric("123123");
            //Button1.Visible = false;
            if (!IsPostBack)
            {
                clvar.Branch = HttpContext.Current.Session["BranchCode"].ToString();
                DataTable dates = MinimumDate(clvar);

                ViewState["dates"] = dates;

                GetServiceTypes();
                Get_Cities();
                Get_Branches();


                dd_start_date.Text = DateTime.Now.ToString("yyyy-MM-dd");

                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[]
                {
                new DataColumn("ConsignmentNo"),
                new DataColumn("ReferenceNo"),
                new DataColumn("AccountNo"),
                new DataColumn("CreditClientID"),
                new DataColumn("Origin"),
                new DataColumn("Destination"),
                new DataColumn("ServiceType"),
                new DataColumn("RiderCode"),
                new DataColumn("Consigner"),
                new DataColumn("Consignee"),
                new DataColumn("Address"),
                new DataColumn("Weight"),
                new DataColumn("COD"),
                new DataColumn("ExpressCenterCode"),
                new DataColumn("ZoneCode"),
                new DataColumn("BranchCode"),
                new DataColumn("ManifestNo"),
                new DataColumn("BookingDate") ,
                new DataColumn("DestinationExpressCenter"),
                new DataColumn("OriginExpressCenter")
                    });
                dt.AcceptChanges();
                ViewState["dt"] = dt;

                DataTable dt_ConLength = new DataTable();
                dt_ConLength.Columns.AddRange(new DataColumn[]
                {
                new DataColumn("ConLen"),
                });
                dt_ConLength.AcceptChanges();
                ViewState["dt_ConLength"] = dt_ConLength;

                //Consignment Tracking

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
                dt_Consignment.Columns.Add("ReferenceNo", typeof(string));
                dt_Consignment.Columns.Add("ConsignerAccountNo", typeof(string));
                dt_Consignment.Columns.Add("CreditClientID", typeof(Int64));
                dt_Consignment.Columns.Add("Orgin", typeof(string));
                dt_Consignment.Columns.Add("Destination", typeof(string));
                dt_Consignment.Columns.Add("ServiceTypeName", typeof(string));
                dt_Consignment.Columns.Add("RiderCode", typeof(string));
                dt_Consignment.Columns.Add("Consigner", typeof(string));
                dt_Consignment.Columns.Add("Consignee", typeof(string));
                dt_Consignment.Columns.Add("Address", typeof(string));
                dt_Consignment.Columns.Add("weight", typeof(float));
                dt_Consignment.Columns.Add("COD", typeof(bool));
                dt_Consignment.Columns.Add("ExpressCenterCode", typeof(string));
                dt_Consignment.Columns.Add("ZoneCode", typeof(string));
                dt_Consignment.Columns.Add("BranchCode", typeof(string));
                dt_Consignment.Columns.Add("bookingDate", typeof(DateTime));
                dt_Consignment.Columns.Add("DestinationExpressCenterCode", typeof(string));
                dt_Consignment.Columns.Add("CreatedBy", typeof(string));
                dt_Consignment.Columns.Add("originExpressCenter", typeof(string));

                DataTable mnp_consignmentManifest = new DataTable();
                mnp_consignmentManifest.Columns.Add("ConsignmentNumber", typeof(string));
                mnp_consignmentManifest.Columns.Add("ManifestNumber", typeof(string));
                mnp_consignmentManifest.Columns.Add("StatusCode", typeof(string));
                mnp_consignmentManifest.Columns.Add("Reason", typeof(string));
                mnp_consignmentManifest.Columns.Add("DemanifestStateID", typeof(string));
                mnp_consignmentManifest.Columns.Add("Remarks", typeof(string));
                ViewState["dtConsignmentTracking"] = dt_ConsignmentTracking;
                ViewState["dtConsignment"] = dt_Consignment;
                ViewState["dtMnP_consignmentManifest"] = mnp_consignmentManifest;
            }
        }

        /*
        protected void GetDestinations()
        {
            DataTable dt = func.Branch().Tables[0];
            DataView dv = dt.AsDataView();
            dv.Sort = "BranchName";
            if (dt == null)
            {

            }
            else
            {
                if (dt.Rows.Count > 0)
                {
                    dd_destination.DataSource = dv;
                    dd_destination.DataTextField = "BranchName";
                    dd_destination.DataValueField = "branchCode";
                    dd_destination.DataBind();
                }
            }
        }
         * */

        public void Get_Branches()
        {
            clvar.CityCode = dd_city.SelectedValue;
            DataSet ds = CF.ExpressCenterLocal(clvar);
            ViewState["city_dest"] = ds;
        }

        public void Get_Destinations()
        {
            clvar.expresscenter = dd_city.SelectedValue;
            DataSet ds = (DataSet)ViewState["city_dest"];

            if (ds.Tables.Count != 0)
            {
                if (ds.Tables[0].Rows.Count != 0)
                {
                    string expresscenter = "";
                    DataRow[] dr = ds.Tables[0].Select("ServiceID='" + dd_city.SelectedValue + "'");
                    if (dr.Length != 0)
                    {
                        DataTable dt = dr.CopyToDataTable();
                        expresscenter = dr[0]["expresscentercode"].ToString();

                        ViewState["Ex_Info"] = dt;// ds.Tables[0];
                        DataTable ds_ = (DataTable)ViewState["Ex_Info"];
                        if (ds_.Rows.Count != 0)
                        {
                            DataRow[] dr_ = ds_.Select("expressCenterCode='" + expresscenter + "'");

                            if (dr.Length != 0)
                            {
                                hd_Destination.Value = dr[0]["bid"].ToString();
                                hd_DestinationZone.Value = dr[0]["ZoneCode"].ToString();
                                hd_Destination_Ec.Value = dr[0]["expressCenterCode"].ToString();
                            }
                        }
                        else
                        {
                            //   cb_Destination.SelectedValue = "0";
                        }

                    }
                    else
                    {
                        //this.cb_Destination.Items.Clear();
                        //this.cb_Destination.Text = "";

                    }
                }
            }
        }

        public void Get_Cities()
        {
            DataTable dt = Cities_();

            dd_city.DataSource = dt;
            dd_city.DataTextField = "Sname";
            dd_city.DataValueField = "BranchCode";
            dd_city.DataBind();
            ViewState["cities"] = dt;
        }

        protected void GetServiceTypes()
        {
            DataTable dt = ServiceTypeNameRvdbo();
            if (dt != null)
            {

                dd_serviceType.DataSource = dt;
                dd_serviceType.DataTextField = "ServiceTypeName";
                dd_serviceType.DataValueField = "ServiceTypeName";
                dd_serviceType.DataBind();
                dd_serviceType.SelectedValue = "overnight";
            }
        }

        protected void btn_add_Click(object sender, EventArgs e)
        {
            DataTable dt = ViewState["dt"] as DataTable;

            DataRow dr = dt.NewRow();
            dr["ConsignmentNo"] = txt_referenceNo.Text;
            dr["AccountNo"] = txt_accountNo.Text;
            dr["CreditClientID"] = hd_creditClientID.Value;
            //  dr["Origin"] = hd_origin.Value;
            //   dr["Destination"] = dd_destination.SelectedValue;
            dr["serviceType"] = dd_serviceType.SelectedValue;
            dr["RiderCode"] = txt_riderCode.Text;


        }

        protected void txt_consignment_TextChanged(object sender, EventArgs e)
        {
            if (txt_riderCode.Text.Trim() == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter Rider Code')", true);
                txt_riderCode.Focus();
                txt_consignment.Text = "";
                //txt_consignment.Focus();
                return;
            }
            clvar.consignmentNo = txt_consignment.Text;
            if (!IsNumeric(clvar.consignmentNo))
            {
                txt_consignment.Text = "";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Only Numerics allowed in Consignment Number')", true);
                return;
            }

            #region Primary CN Check
            var rs = PrimaryCheck(clvar.consignmentNo);

            if (!string.IsNullOrEmpty(rs))
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "showError", $"alert('{rs}');", true);
                return;
            }
            #endregion

            DataTable dt = Get_CardConsignment(clvar);

            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["COD"].ToString().ToUpper() == "1")
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "showError",
                               "alert('This is COD CN.');", true);
                    txt_consignment.Text = "";
                    txt_consignment.Focus();
                    return;
                }
            }


            DataTable dt_ConLength = ViewState["dt_ConLength"] as DataTable;
            Con_Length = dt_ConLength.Rows[0]["ConLen"].ToString();

            if (dt.Rows.Count == 0)
            {
                /* if (dt.Rows[0]["COD"].ToString().ToUpper() == "TRUE")
                 {
                     ScriptManager.RegisterStartupScript(Page, Page.GetType(), "showError",
                                "alert('This is COD CN.');", true);
                     txt_consignment.Text = "";
                     txt_consignment.Focus();
                     return;
                 }
                 else
                 {*/
                if (!(txt_consignment.Text.Length < 12 || txt_consignment.Text.Length > 20))
                {
                    if (chk_refFreeze.Checked)
                    {
                        txt_consignee.Focus();
                    }
                    else
                    {
                        txt_referenceNo.Focus();
                    }

                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "showError",
                                "alert('Consignment Number Must be between 12 and 15 digits');", true);
                    txt_consignment.Text = "";
                }
                //}
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "showError",
                            "alert('Consignment Number Already in the System. Please Enter another Consignment Number');", true);
                txt_consignment.Text = "";
            }
        }

        protected void txt_manifest_TextChanged(object sender, EventArgs e)
        {
            if (txt_riderCode.Text.Trim() == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter Rider Code')", true);
                txt_manifest.Text = "";
                txt_manifest.Enabled = true;
                //txt_manifest.Focus();
                txt_riderCode.Focus();
                return;
            }
            clvar.manifestNo = txt_manifest.Text;
            DataTable dt = con.Get_CardManifest(clvar);

            if (dt.Rows.Count == 0)
            {
                if (txt_manifest.Text.Length.ToString() == "10" || txt_manifest.Text.Length.ToString() == "11")
                {
                    txt_consignment.Focus();
                    txt_manifest.Enabled = false;
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "showError",
                                "alert('Manifest Number Must be 10 Or 11 Characters');", true);
                    txt_manifest.Text = "";
                    txt_manifest.Enabled = true;
                    txt_manifest.Focus();
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "showError",
                            "alert('Manifest Number Already in the System. Please Enter another Manifest Number');", true);
                txt_manifest.Text = "";
                txt_manifest.Enabled = true;
                txt_manifest.Focus();
            }
        }

        protected void txt_address_TextChanged(object sender, EventArgs e)
        {
            if (!IsNumeric(clvar.consignmentNo))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Only Numerics allowed in Consignment Number')", true);
                return;
            }
            if (txt_consignment.Text.Length < 11 || txt_consignment.Text.Length > 20)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Consignment Number must be between 11 and 20 digits.')", true);
                return;
            }
            if (txt_address.Text.Contains("'"))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Single Quotes not allowed in Address')", true);
                return;
            }
            if (dd_city.SelectedValue.ToString() == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Kindly select proper Destination')", true);
                dd_city.Focus();
                return;
            }

            double w = 0;
            double.TryParse(txt_weight.Text, out w);
            if (w <= 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Weight Cannot be 0')", true);
                txt_referenceNo.Text = "";
                return;
            }
            DataTable dates = ViewState["dates"] as DataTable;

            DateTime minAllowedDate = DateTime.Parse(dates.Rows[0]["DateAllowed"].ToString()).AddDays(1);
            DateTime maxAllowedDate = DateTime.Today;
            if (DateTime.Parse(dd_start_date.Text) < minAllowedDate || DateTime.Parse(dd_start_date.Text) > maxAllowedDate)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Day End has already been performed for selected Date.  Select Different Date')", true);
                txt_address.Text = "";
                txt_address.Focus();
                return;
            }
            if (txt_riderCode.Text.Trim() == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter Rider Code')", true);
                txt_address.Text = "";
                //txt_address.Focus();
                txt_riderCode.Focus();
                return;
            }

            consignmentNo = txt_consignment.Text;
            referenceNo = txt_referenceNo.Text;
            DataTable cities = ViewState["cities"] as DataTable;
            Destination = dd_city.SelectedValue; //hd_Destination.Value;
            ServiceType = dd_serviceType.SelectedValue;
            Weight = "0.5";
            ridercode = txt_riderCode.Text;
            ManifestNo = txt_manifest.Text;
            BookingDate = dd_start_date.Text;

            Consignee = txt_consignee.Text;
            address = txt_address.Text;

            clvar.expresscenter = hd_originEC.Value;//cities.Select("BranchCode = '" + dd_city.SelectedValue + "'")[0]["ECCode"].ToString(); // hd_Destination_Ec.Value;
            ExpressCenterCode = cities.Select("BranchCode = '" + dd_city.SelectedValue + "'")[0]["ECCode"].ToString();
            BranchCode = HttpContext.Current.Session["BranchCode"].ToString(); //hd_Destination.Value;
            ZoneCode = HttpContext.Current.Session["ZoneCode"].ToString();


            clvar.AccountNo = txt_accountNo.Text;
            DataTable cr_dt = con.Get_CardCreditClients(clvar);
            Consigner = cr_dt.Rows[0]["name"].ToString();
            CreditClientID = cr_dt.Rows[0]["id"].ToString();


            orgin = Session["branchcode"].ToString();

            if (Consignee != "" && address != "")
            {
                DataTable dt1 = ViewState["dt"] as DataTable;

                DataRow[] foundRows = dt1.Select("consignmentNo ='" + consignmentNo + "'  ");

                if (foundRows.Length == 0)
                {

                    int numberOfRecords = dt1.Select().Length + 1;
                    lbl_count.Text = "Consignmnet Session Count = " + numberOfRecords.ToString();
                    if (numberOfRecords.ToString() != "1")
                    {
                        double weight = double.Parse(txt_weight.Text);
                        weight += 0.5;
                        txt_weight.Text = weight.ToString();
                    }

                    //dt1.Rows.Add(consignmentNo, referenceNo, clvar.AccountNo, CreditClientID, orgin, Destination, ServiceType, ridercode, Consigner, Consignee, 
                    //    address, Weight, COD, ExpressCenterCode, ZoneCode, BranchCode, ManifestNo, BookingDate, ExpressCenterCode, clvar.expresscenter);

                    DataRow dr = dt1.NewRow(); //Create New Row
                    dr[0] = consignmentNo;
                    dr[1] = referenceNo;
                    dr[2] = clvar.AccountNo;
                    dr[3] = CreditClientID;
                    dr[4] = orgin;
                    dr[5] = Destination;
                    dr[6] = ServiceType;
                    dr[7] = ridercode;
                    dr[8] = Consigner;
                    dr[9] = Consignee;
                    dr[10] = address;
                    dr[11] = Weight;
                    dr[12] = COD;
                    dr[13] = ExpressCenterCode;
                    dr[14] = ZoneCode;
                    dr[15] = BranchCode;
                    dr[16] = ManifestNo;
                    dr[17] = BookingDate;
                    dr[18] = ExpressCenterCode;
                    dr[19] = clvar.expresscenter;

                    dt1.Rows.InsertAt(dr, 0); // InsertAt specified position
                    dt1.AcceptChanges();
                    ViewState["dt"] = dt1;
                    GridView.DataSource = dt1;
                    GridView.DataBind();
                    if (!chk_refFreeze.Checked)
                    {
                        txt_referenceNo.Text = "";
                    }

                    txt_referenceNo.Focus();

                    Button1.Visible = true;
                    txt_consignment.ReadOnly = false;
                    txt_consignment.Text = "";
                    txt_consignee.Text = "";
                    txt_address.Text = "";
                    txt_consignment.Focus();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "showError",
                    "alert('Consignment Already in List');", true);
                    txt_referenceNo.Text = "";
                    txt_referenceNo.Focus();
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "showError",
                "alert('Insert Detail Form');", true);
            }

        }

        protected void txt_accountNo_TextChanged(object sender, EventArgs e)
        {
            clvar.AccountNo = txt_accountNo.Text;
            DataTable dt = con.Get_CardCreditClients(clvar);

            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["IsCOD"].ToString().ToUpper() == "TRUE")
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "showError",
                               "alert('This is COD Account Number.');", true);
                    txt_accountNo.Text = "";
                    txt_accountNo.Focus();
                    return;
                }
                else
                {
                    Consigner = dt.Rows[0]["name"].ToString();
                    Con_Length = dt.Rows[0]["Con_Length"].ToString();

                    DataTable dt_ConLength = ViewState["dt_ConLength"] as DataTable;
                    dt_ConLength.Rows.Add(Con_Length);

                    dt_ConLength.AcceptChanges();
                    ViewState["dt_ConLength"] = dt_ConLength;

                    txt_accountNo.Enabled = false;
                    txt_riderCode.Focus();
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "showError",
                            "alert('Account Number Dective / Invalid Account Number');", true);
                txt_accountNo.Text = "";
                txt_accountNo.Focus();
            }
        }

        protected void txt_referenceNo_TextChanged(object sender, EventArgs e)
        {
            /*
            clvar.consignmentNo = txt_consignment.Text;
            clvar.RefNumber     = txt_referenceNo.Text;
            clvar.AccountNo     = txt_accountNo.Text;
            clvar.RiderCode     = txt_riderCode.Text;
            clvar.manifestNo    = txt_manifest.Text;

            if (clvar.consignmentNo != "" && clvar.AccountNo != "" && clvar.RefNumber != "" && clvar.manifestNo != "" && dd_destination.SelectedValue != "" && dd_serviceType.SelectedValue != "" &&
                dd_start_date.Text != "" && clvar.RiderCode != "" && txt_weight.Text != "")
            {
                DataTable dt2 = con.GetCreditClientByAccountNo(clvar);
                DataTable dt = con.GetCardConsignmentByRefNumber(clvar);
                DataTable dt4 = con.GetRiderExpressCenterCode(clvar);
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {

                        consignmentNo = clvar.consignmentNo;
                        referenceNo     = dt.Rows[0]["refrenceNo"].ToString().Replace("*", "");
                        Consignee       = dt.Rows[0]["Consignee"].ToString();
                        OrgName         = dt.Rows[0]["OrgName"].ToString();
                        address         = dt.Rows[0]["address"].ToString();
                        orgin           = dt.Rows[0]["orgin"].ToString();
                        ridercode       = txt_riderCode.Text;                    
                        Destination     = dd_destination.SelectedValue;
                        ServiceType     = dd_serviceType.SelectedValue;
                        Weight          = txt_weight.Text;
                        CreditClientID  = dt2.Rows[0]["id"].ToString();
                        Consigner       = dt2.Rows[0]["name"].ToString();
                        ExpressCenterCode = dt4.Rows[0]["expressCenterId"].ToString();
                        ZoneCode        = dt.Rows[0]["ZoneCode"].ToString();
                        BranchCode      = dt.Rows[0]["BranchCode"].ToString();
                        ManifestNo      = clvar.manifestNo;
                        BookingDate     = dd_start_date.Text;

                        if (dt2.Rows[0]["iscod"].ToString() == "False")
                        {
                            COD = "0";
                        }
                        else
                        {
                            COD = "1";
                        }

                        DataTable dt1 = ViewState["dt"] as DataTable;

                        DataRow[] foundRows = dt1.Select("consignmentNo ='" + consignmentNo + "' OR ReferenceNo ='" + referenceNo + "' ");

                        if (foundRows.Length == 0)
                        {
                            int numberOfRecords = dt1.Select().Length + 1;
                            lbl_count.Text = "Consignmnet Session Count = " + numberOfRecords.ToString();

                            dt1.Rows.Add(consignmentNo, referenceNo, clvar.AccountNo, CreditClientID, orgin, Destination, ServiceType, ridercode, Consigner, Consignee, address, Weight, COD, ExpressCenterCode, ZoneCode, BranchCode, ManifestNo, BookingDate);

                            dt1.AcceptChanges();
                            ViewState["dt"] = dt1;
                            GridView.DataSource = dt1;
                            GridView.DataBind();
                            txt_referenceNo.Text = "";
                            txt_referenceNo.Focus();

                            Button1.Visible = true;

                            txt_consignment.Text = "";
                            txt_consignment.Focus();
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "showError",
                            "alert('Consignment Already in List');", true);
                            txt_referenceNo.Text = "";
                            txt_referenceNo.Focus();
                        }
                    }
                    else
                    {
                        //txt_consignee.Text = "";
                        //txt_consignmentNo.Text = "";
                        //txt_origin.Text = "";
                        //txt_address.Text = "";
                        //hd_origin.Value = "";

                        //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "showError",
                        //   "alert('Reference Number Does not Belong to Account Number');", true);

                        consignmentNo = clvar.consignmentNo;
                        referenceNo = txt_referenceNo.Text;
                        Destination = dd_destination.SelectedValue;
                        ServiceType = dd_serviceType.SelectedValue;
                        Weight = txt_weight.Text;
                        ridercode = txt_riderCode.Text;
                        ManifestNo = clvar.manifestNo;
                        BookingDate = dd_start_date.Text;

                        Consignee = txt_consignee.Text;
                        address = txt_address.Text;


                        clvar.AccountNo = txt_accountNo.Text;
                        DataTable cr_dt = con.Get_CardCreditClients(clvar);
                        CreditClientID = cr_dt.Rows[0]["id"].ToString();
                        Consigner = cr_dt.Rows[0]["name"].ToString();

                        orgin = Session["ZONECODE"].ToString();

                        if (Consignee != "" && address != "")
                        {
                            DataTable dt1 = ViewState["dt"] as DataTable;

                            DataRow[] foundRows = dt1.Select("consignmentNo ='" + consignmentNo + "' OR ReferenceNo ='" + referenceNo + "' ");

                            if (foundRows.Length == 0)
                            {
                                int numberOfRecords = dt1.Select().Length + 1;
                                lbl_count.Text = "Consignmnet Session Count = " + numberOfRecords.ToString();

                                dt1.Rows.Add(consignmentNo, referenceNo, clvar.AccountNo, CreditClientID, orgin, Destination, ServiceType, ridercode, Consigner, Consignee, address, Weight, COD, ExpressCenterCode, ZoneCode, BranchCode, ManifestNo, BookingDate);

                                dt1.AcceptChanges();
                                ViewState["dt"] = dt1;
                                GridView.DataSource = dt1;
                                GridView.DataBind();
                                txt_referenceNo.Text = "";
                                txt_referenceNo.Focus();

                                Button1.Visible = true;

                                txt_consignment.Text = "";
                                txt_consignee.Text = "";
                                txt_address.Text = "";

                                txt_consignment.Focus();
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "showError",
                                "alert('Consignment Already in List');", true);
                                txt_referenceNo.Text = "";
                                txt_referenceNo.Focus();
                            }
                        }
                        else
                        {
                            //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "showError",
                            //"alert('Insert Detail Form');", true);
                        }
                        txt_consignee.Focus();


                    }
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "showError",
                        "alert('Insert Full Form');", true);
                txt_referenceNo.Text = "";
            }
            */
        }

        protected void Btn_Save_Click(object sender, EventArgs e)
        {
            DateTime thisDate = DateTime.Parse(dd_start_date.Text);
            clvar.Branch = HttpContext.Current.Session["BranchCode"].ToString();

            DataTable dates = MinimumDate(clvar);
            DateTime minAllowedDate = DateTime.Parse(dates.Rows[0]["DateAllowed"].ToString()).AddDays(1);
            DateTime maxAllowedDate = DateTime.Now;
            if (thisDate < minAllowedDate || thisDate > maxAllowedDate)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Invalid Date.')", true);
                return;
            }

            if (txt_riderCode.Text.Trim(' ') == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter Rider Code')", true);
                return;
            }
            DataTable dt1 = ViewState["dt"] as DataTable;
            DataTable dtCNs = ViewState["dtConsignment"] as DataTable;
            DataTable mnp_ConsignmentManifest = ViewState["dtMnP_consignmentManifest"] as DataTable;
            DataTable tracking = ViewState["dtConsignmentTracking"] as DataTable;
            DataTable mtracking = ViewState["dtConsignmentTracking"] as DataTable;
            dtCNs.Clear();
            mnp_ConsignmentManifest.Clear();
            tracking.Clear();

            if (dt1.Rows.Count > 0)
            {
                var location = Session["LocationName"].ToString();
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    DataRow dr = dtCNs.NewRow();
                    DataRow drm = mnp_ConsignmentManifest.NewRow();
                    DataRow tr = tracking.NewRow();
                    DataRow trm = tracking.NewRow();
                    dr["consignmentNumber"] = dt1.Rows[i]["consignmentNo"].ToString();
                    dr["referenceNo"] = dt1.Rows[i]["ReferenceNo"].ToString();
                    dr["ConsignerAccountNo"] = dt1.Rows[i]["AccountNo"].ToString();
                    dr["CreditClientID"] = dt1.Rows[i]["CreditClientID"].ToString();
                    dr["Orgin"] = dt1.Rows[i]["Origin"].ToString();
                    dr["Destination"] = dt1.Rows[i]["Destination"].ToString();
                    dr["ServiceTypeName"] = "overnight";
                    dr["RiderCode"] = dt1.Rows[i]["RiderCode"].ToString();
                    dr["Consigner"] = dt1.Rows[i]["Consigner"].ToString();
                    dr["Consignee"] = dt1.Rows[i]["Consignee"].ToString();
                    dr["Address"] = dt1.Rows[i]["address"].ToString();
                    dr["weight"] = dt1.Rows[i]["Weight"].ToString();
                    if (dt1.Rows[i]["COD"].ToString() == "1")
                    {
                        dr["COD"] = true;
                    }
                    else
                    {
                        dr["COD"] = false;
                    }
                    //dr["COD"] = dt1.Rows[i]["COD"];
                    dr["ExpressCenterCode"] = HttpContext.Current.Session["ExpressCenter"].ToString();
                    dr["ZoneCode"] = dt1.Rows[i]["ZoneCode"].ToString();
                    dr["BranchCode"] = dt1.Rows[i]["BranchCode"].ToString();
                    dr["bookingDate"] = dt1.Rows[i]["BookingDate"].ToString();
                    dr["DestinationExpressCenterCode"] = dt1.Rows[i]["DestinationExpressCenter"].ToString();
                    dr["CreatedBy"] = HttpContext.Current.Session["U_ID"].ToString();
                    dr["originExpressCenter"] = dt1.Rows[i]["OriginExpressCenter"].ToString();
                    dtCNs.Rows.Add(dr);

                    drm["consignmentNumber"] = dt1.Rows[i]["consignmentNo"].ToString();
                    drm["manifestNumber"] = dt1.Rows[0]["ManifestNo"].ToString();
                    drm["statusCode"] = "";
                    drm["reason"] = "";
                    drm["DemanifestStateID"] = "";
                    drm["Remarks"] = "";
                    mnp_ConsignmentManifest.Rows.Add(drm);


                    //clvar.consignmentNo = dt1.Rows[i]["consignmentNo"].ToString();
                    //clvar.AccountNo = dt1.Rows[i]["AccountNo"].ToString();
                    //clvar.CreditClientID = dt1.Rows[i]["CreditClientID"].ToString();
                    clvar.origin = dt1.Rows[i]["Origin"].ToString();
                    clvar.Destination = dt1.Rows[i]["Destination"].ToString();
                    clvar.ServiceType = dt1.Rows[i]["ServiceType"].ToString();
                    clvar.RiderCode = dt1.Rows[i]["RiderCode"].ToString();
                    clvar.Consigner = dt1.Rows[i]["Consigner"].ToString();
                    clvar.Consignee = dt1.Rows[i]["Consignee"].ToString();
                    clvar.shipperAddress = dt1.Rows[i]["Address"].ToString();
                    clvar.ToWeight = dt1.Rows[i]["Weight"].ToString();
                    clvar.Insurance = dt1.Rows[i]["COD"].ToString();
                    clvar.expresscenter = dt1.Rows[i]["ExpressCenterCode"].ToString();
                    clvar.Zone = dt1.Rows[i]["ZoneCode"].ToString();
                    clvar.Branch = dt1.Rows[i]["BranchCode"].ToString();
                    clvar.manifestNo = dt1.Rows[i]["ManifestNo"].ToString();
                    clvar.Day = dt1.Rows[i]["BookingDate"].ToString();
                    clvar.RefNo = dt1.Rows[i]["ReferenceNo"].ToString();

                    clvar.destinationExpressCenterCode = dt1.Rows[i]["DestinationExpressCenter"].ToString();

                    Vclvar._Id = Session["BRANCHCODE"].ToString();
                    DataSet ds_origin = b_fun.Get_BranchDetail(Vclvar);
                    clvar.destinationCity = ds_origin.Tables[0].Rows[0]["name"].ToString();
                    tr["ConsignmentNumber"] = dt1.Rows[i]["consignmentNo"].ToString();
                    tr["stateID"] = "1";
                    //tr["CurrentLocation"] = clvar.destinationCity.ToString();
                    tr["CurrentLocation"] = location; //Changed by Fahad Ali 09-oct-2020
                    tr["transactionTime"] = DateTime.Now;

                    trm["ConsignmentNumber"] = dt1.Rows[i]["consignmentNo"].ToString();
                    trm["stateID"] = "2"; // Changed on 30-10-2019 by Bilal
                    //trm["CurrentLocation"] = clvar.destinationCity.ToString();
                    trm["CurrentLocation"] = location; //Changed by Fahad Ali 09-oct-2020
                    trm["transactionTime"] = DateTime.Now;
                    trm["manifestNumber"] = dt1.Rows[i]["ManifestNo"].ToString();

                    tracking.Rows.Add(tr);
                    tracking.Rows.Add(trm);
                }

                //clvar.manifestNo = dt1.Rows[0]["ManifestNo"].ToString();
                //clvar.origin = dt1.Rows[0]["Origin"].ToString();
                //clvar.Destination = dt1.Rows[0]["Destination"].ToString();
                //clvar.Day = dt1.Rows[0]["BookingDate"].ToString();
                //clvar.ServiceType = dt1.Rows[0]["ServiceType"].ToString();

                //con.Insert_CardManifest(clvar, dt1);
                clvar.manifestNo = dt1.Rows[0]["ManifestNo"].ToString();
                clvar.origin = dt1.Rows[0]["Origin"].ToString();
                clvar.destination = dt1.Rows[0]["Destination"].ToString();
                clvar.Day = dt1.Rows[0]["BookingDate"].ToString();
                clvar.ServiceType = dt1.Rows[0]["ServiceType"].ToString();
                clvar.Weight = float.Parse(txt_weight.Text);
                clvar.pieces = dt1.Select().Length;

                //string p = con.Insert_CardManifest_(clvar, mnp_ConsignmentManifest);
                //b_fun.BulkInsertTrackingtoDatabase(tracking);
                string error = Insert_CardConsignment(clvar, dtCNs, mnp_ConsignmentManifest, tracking);

                lbl_count.Text = "";

                if (error == "0")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert(Could not Complete Transaction.)", true);
                    return;
                }
                lbl_count.Text = "";

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "showError",
                            "alert('Card Consignemt Saved');", true);

                string script_ = @"window.open(""{0}"", ""{1}"", ""{2}"");";
                string script = String.Format(script_, "CardManifest_Print.aspx?Xcode=" + clvar.manifestNo, "_blank", "");
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Redirect", script, true);
                btn_reset_Click(sender, e);
            }
        }

        protected void btn_reset_Click(object sender, EventArgs e)
        {
            if (!chk_refFreeze.Checked)
            {
                txt_referenceNo.Text = "";
            }

            txt_accountNo.Text = "";
            txt_riderCode.Text = "";
            dd_city.SelectedValue = "";
            txt_manifest.Enabled = true;
            txt_referenceNo.Text = "";
            txt_referenceNo.Enabled = true;
            txt_weight.Text = "0.5";
            chk_refFreeze.Checked = false;
            // txt_weight.Text = "";
            txt_manifest.Text = "";
            dd_city.ClearSelection();
            dd_serviceType.ClearSelection();

            GridView.DataSource = null;
            GridView.DataBind();

            //DataTable dt = new DataTable();
            //dt.Columns.Add("ConsignmentNo", typeof(string));
            //dt.Columns.Add("AccountNo", typeof(string));
            //dt.Columns.Add("CreditClientID", typeof(string));
            //dt.Columns.Add("Origin", typeof(string));
            //dt.Columns.Add("Destination", typeof(string));
            //dt.Columns.Add("ServiceType", typeof(string));
            //dt.Columns.Add("RiderCode", typeof(string));
            //dt.Columns.Add("Consigner", typeof(string));
            //dt.Columns.Add("Consignee", typeof(string));
            //dt.Columns.Add("Address", typeof(string));
            //dt.Columns.Add("Weight", typeof(string));
            //dt.Columns.Add("COD", typeof(string));
            //dt.Columns.Add("ExpressCenterCode", typeof(string));
            //dt.Columns.Add("ZoneCode", typeof(string));
            //dt.Columns.Add("BranchCode", typeof(string));
            //dt.AcceptChanges();
            //ViewState["dt"] = dt;


            GetServiceTypes();
            Get_Cities();
            Get_Branches();


            dd_start_date.Text = DateTime.Now.ToString("yyyy-MM-dd");

            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[]
                {
                new DataColumn("ConsignmentNo"),
                new DataColumn("ReferenceNo"),
                new DataColumn("AccountNo"),
                new DataColumn("CreditClientID"),
                new DataColumn("Origin"),
                new DataColumn("Destination"),
                new DataColumn("ServiceType"),
                new DataColumn("RiderCode"),
                new DataColumn("Consigner"),
                new DataColumn("Consignee"),
                new DataColumn("Address"),
                new DataColumn("Weight"),
                new DataColumn("COD"),
                new DataColumn("ExpressCenterCode"),
                new DataColumn("ZoneCode"),
                new DataColumn("BranchCode"),
                new DataColumn("ManifestNo"),
                new DataColumn("BookingDate") ,
                new DataColumn("DestinationExpressCenter"),
                new DataColumn("OriginExpressCenter")
                    });
            dt.AcceptChanges();
            ViewState["dt"] = dt;

            DataTable dt_ConLength = new DataTable();
            dt_ConLength.Columns.AddRange(new DataColumn[]
                {
                new DataColumn("ConLen"),
                });
            dt_ConLength.AcceptChanges();
            ViewState["dt_ConLength"] = dt_ConLength;

            //Consignment Tracking

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
            dt_Consignment.Columns.Add("ReferenceNo", typeof(string));
            dt_Consignment.Columns.Add("ConsignerAccountNo", typeof(string));
            dt_Consignment.Columns.Add("CreditClientID", typeof(Int64));
            dt_Consignment.Columns.Add("Orgin", typeof(string));
            dt_Consignment.Columns.Add("Destination", typeof(string));
            dt_Consignment.Columns.Add("ServiceTypeName", typeof(string));
            dt_Consignment.Columns.Add("RiderCode", typeof(string));
            dt_Consignment.Columns.Add("Consigner", typeof(string));
            dt_Consignment.Columns.Add("Consignee", typeof(string));
            dt_Consignment.Columns.Add("Address", typeof(string));
            dt_Consignment.Columns.Add("weight", typeof(float));
            dt_Consignment.Columns.Add("COD", typeof(bool));
            dt_Consignment.Columns.Add("ExpressCenterCode", typeof(string));
            dt_Consignment.Columns.Add("ZoneCode", typeof(string));
            dt_Consignment.Columns.Add("BranchCode", typeof(string));
            dt_Consignment.Columns.Add("bookingDate", typeof(DateTime));
            dt_Consignment.Columns.Add("DestinationExpressCenterCode", typeof(string));
            dt_Consignment.Columns.Add("CreatedBy", typeof(string));
            dt_Consignment.Columns.Add("originExpressCenter", typeof(string));

            DataTable mnp_consignmentManifest = new DataTable();
            mnp_consignmentManifest.Columns.Add("ConsignmentNumber", typeof(string));
            mnp_consignmentManifest.Columns.Add("ManifestNumber", typeof(string));
            mnp_consignmentManifest.Columns.Add("StatusCode", typeof(string));
            mnp_consignmentManifest.Columns.Add("Reason", typeof(string));
            mnp_consignmentManifest.Columns.Add("DemanifestStateID", typeof(string));
            mnp_consignmentManifest.Columns.Add("Remarks", typeof(string));
            ViewState["dtConsignmentTracking"] = dt_ConsignmentTracking;
            ViewState["dtConsignment"] = dt_Consignment;
            ViewState["dtMnP_consignmentManifest"] = mnp_consignmentManifest;

        }

        protected void dd_cities_selectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (dd_city.SelectedValue != "0")
            {
                Get_Destinations(); // Get_Branches();
            }
            //dd_city.Focus();
            //cb_Destination.Focus();

        }

        protected void chk_refFreeze_CheckedChanged(object sender, EventArgs e)
        {
            if (txt_referenceNo.Text.Trim(' ') == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Enter Reference Number')", true);
                chk_refFreeze.Checked = false;
                return;
            }
            else
            {
                if (chk_refFreeze.Checked)
                {
                    txt_referenceNo.Enabled = false;
                    txt_consignee.Focus();

                }
                else
                {
                    txt_referenceNo.Enabled = true; txt_referenceNo.Focus();
                }
            }
        }

        protected DataTable MinimumDate(Cl_Variables clvar)
        {
            string sqlString = "select MAX(DateTime) DateAllowed from Mnp_Account_DayEnd where Branch = '" + clvar.Branch + "' and doc_type = 'A'";
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

        public string Insert_CardConsignment(Cl_Variables clvar, DataTable cn, DataTable man, DataTable track)
        {
            SqlConnection con = new SqlConnection(clvar.Strcon());

            SqlCommand cmd = new SqlCommand();

            cmd.Connection = con;

            con.Open();


            try
            {

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Bulk_Card_CNInsert";
                cmd.Parameters.AddWithValue("@tblCustomers", cn);
                cmd.Parameters.AddWithValue("@tblCustomers1", man);
                cmd.Parameters.AddWithValue("@tblCustomers2", track);
                cmd.Parameters.AddWithValue("@ManifestNumber", clvar.manifestNo);
                cmd.Parameters.AddWithValue("@manifestType", "overnight");
                cmd.Parameters.AddWithValue("@manifestDate", clvar.Bookingdate);
                cmd.Parameters.AddWithValue("@origin", clvar.origin);
                cmd.Parameters.AddWithValue("@destination", clvar.destination);
                cmd.Parameters.AddWithValue("@createdBy", HttpContext.Current.Session["U_ID"].ToString());
                cmd.Parameters.AddWithValue("@BranchCode", HttpContext.Current.Session["BranchCode"].ToString());
                cmd.Parameters.AddWithValue("@zoneCode", HttpContext.Current.Session["ZoneCode"].ToString());
                cmd.Parameters.AddWithValue("@Weight", clvar.Weight);
                cmd.Parameters.AddWithValue("@Pieces", clvar.pieces);

                cmd.Parameters.Add("@error_message", SqlDbType.NVarChar, 500).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                if (cmd.Parameters["@error_message"].Value.ToString() == "0")
                {
                    con.Close();
                    con.Dispose();
                    return "0";
                }

            }

            catch (SqlException sqlEx)
            {

            }

            finally
            {
                con.Close();
                con.Dispose();
            }
            return "1";
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

            sqlString = "select b.branchCode , b.sname + ' - ' + b.name SNAME , ec.ExpressCenterCode ECCode\n" +
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

        protected void txt_riderCode_TextChanged(object sender, EventArgs e)
        {
            GetRiderDetail();
            txt_manifest.Focus();
        }
        protected DataTable GetRiderDetail()
        {
            string query = "SELECT * FROM RIDERS r where r.riderCode = '" + txt_riderCode.Text.Trim() + "' and r.branchID = '" + HttpContext.Current.Session["BranchCode"].ToString() + "' and r.status = '1'";
            DataTable dt = new DataTable();
            try
            {
                SqlConnection con = new SqlConnection(clvar.Strcon());
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                sda.Fill(dt);
            }
            catch (Exception ex)
            { }
            if (dt.Rows.Count > 0)
            {
                hd_originEC.Value = dt.Rows[0]["ExpressCenterID"].ToString();
                txt_weight.Focus();
            }
            else
            {
                hd_originEC.Value = "0";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Invalid Rider Code')", true);
                txt_riderCode.Text = "";
                txt_riderCode.Focus();
            }
            return dt;
        }

        public DataTable Get_CardConsignment(Cl_Variables clvar)
        {
            string sqlString = "select * from (Select * from cardConsignment_temp UNION Select * from Consignment) c where c.ConsignmentNumber = '" + clvar.consignmentNo + "' ";

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


        public DataTable ServiceTypeNameRvdbo()
        {
            DataSet Ds_1 = new DataSet();

            try
            {
                string query = "SELECT st.name          ServiceTypeName \n"
                + "FROM   ServiceTypes     st \n"
                + "WHERE  st.isintl = '0' \n"
                + "       AND st.status = '1' \n"
                + "GROUP BY \n"
                + "       st.name \n"
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
            return Ds_1.Tables[0];
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

        protected void btn_removeConsignment_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            double weight = double.Parse(txt_weight.Text);
            //Get the row that contains this button
            Button btnEdit = (Button)sender;
            GridViewRow row = (GridViewRow)btnEdit.NamingContainer;
            HiddenField hd_ConsignmentNo = (HiddenField)row.FindControl("hd_ConsignmentNo");

            DataTable dt1 = ViewState["dt"] as DataTable;
            for (int i = dt1.Rows.Count - 1; i >= 0; i--)
            {
                DataRow dr = dt1.Rows[i];
                if (dr["ConsignmentNo"].ToString() == hd_ConsignmentNo.Value.ToString())
                {
                    dr.Delete();
                    weight -= 0.5;
                    break;
                }
            }
            txt_weight.Text = weight.ToString();
            dt1.AcceptChanges();
            ViewState["dt"] = dt1;
            GridView.DataSource = dt1;
            GridView.DataBind();
            lbl_count.Text = "Consignmnet Session Count = " + dt1.Rows.Count.ToString();
        }

        public string PrimaryCheck(string cn)
        {
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
    }
}