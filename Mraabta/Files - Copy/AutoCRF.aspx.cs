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
    public partial class AutoCRF : System.Web.UI.Page
    {
        CL_Customer clvar = new CL_Customer();
        Cl_Variables cl_var = new Cl_Variables();
        CustomerMethods cm = new CustomerMethods();
        //SqlConnection con = new SqlConnection(clvar.Strcon());
        DataTable Categories = new DataTable();
        CommonFunction func = new CommonFunction();

        public bool isMinBilling = false;
        public float minBillingAmount = 0;
        public string BeneficiaryName = "";
        public string BeneficiaryBankAccNo = "";
        public string BeneficiaryBankCode = "";
        public string ispiece_only = "";
        public string IsDestination = "";
        public string rnrWeight = "";
        public string ContactPersonDesignation = "";
        public string ParentID = "";
        //DataTable dt = new DataTable();

        string ServiceId;
        DataTable dt_service = new DataTable();
        DataTable dt_tariff2 = new DataTable();
        DataTable addTariff_dt = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            //  ViewState["service_dt"] = null;

            // SelectCheckBoxList(chkl_services.SelectedValue);

            dt_service.Columns.Add(new DataColumn("Service", typeof(string)));

            DataTable dt_tariff = new DataTable();
            dt_tariff.Columns.AddRange(new DataColumn[]
                {
                new DataColumn("ID"),
                new DataColumn("DestinationID"),
                new DataColumn("Destination"),
                new DataColumn("ServiceID"),
                new DataColumn("FromWeight"),
                new DataColumn("ToWeight"),
                new DataColumn("Price"),
                new DataColumn("AddFactor"),
                new DataColumn("isUpdated")
                });
            dt_tariff.AcceptChanges();
            ViewState["dt_tariff"] = dt_tariff;

            Errorid.Text = "";
            //if (HttpContext.Current.Session["U_ID"].ToString() != "1208")
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "alert('You are not authorized to view this page.'); window.location='" + Request.ApplicationPath + "/Files/BTSDashoard.aspx';", true);
            //}
            if (HttpContext.Current.Session["StaffLevel"].ToString() == "5")
            {
                div11.Visible = true;
                txt_acc_no.Visible = true;
                btn_save.Visible = true;
            }

            if (HttpContext.Current.Session["PROFILE"].ToString() != "16")
            {
                dd_zone.Enabled = false;
                chk_nationWide.Enabled = false;
                ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:BasicInfo.style.display = 'block'; nationwideTab.style.display = 'none'; ", true);
            }
            if (!IsPostBack)
            {
                ViewState["dt_tariff2"] = null;

                GetServices();
                if (HttpContext.Current.Session["PROFILE"].ToString() != "16")
                {
                    dd_zone.SelectedValue = HttpContext.Current.Session["ZoneCode"].ToString();
                    dd_zone_SelectedIndexChanged(this, e);
                }
                GetClientParents();
                txt_nationWideAccountNo.Enabled = false;
                GetZones();
                ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc( 'BasicInfo');", true);
                CODTypes();
                DataTable addedPriceModifiers = new DataTable();
                addedPriceModifiers.Columns.Add("ID");
                addedPriceModifiers.Columns.Add("Name");
                addedPriceModifiers.Columns.Add("CalculationValue");
                addedPriceModifiers.Columns.Add("CalculationBase");
                addedPriceModifiers.Columns.Add("Description");
                addedPriceModifiers.AcceptChanges();
                ViewState["addedPriceModifiers"] = addedPriceModifiers;

                //btn_SelectGroup.Visible = false;

                //btn_Search.Visible = false;
                //Search_Client_div.Visible = false;
                //Search_Group_DIV.Visible = false;


                //  Search_Group_DIV.Visible = false;
                //Search_Client_div.Visible = false;

                //SqlDataAdapter adapter = new SqlDataAdapter("select id ,name  from CreditClientCategories where status='1' ", con);
                //adapter.Fill(Categories);
                //ddl_Cat.DataSource = Categories;
                //ddl_Cat.DataTextField = "name";
                //ddl_Cat.DataValueField = "id";
                //ddl_Cat.DataBind();

                DataTable Staffs = new DataTable();
                Staffs.Columns.Add("StaffTypeID", typeof(string));
                Staffs.Columns.Add("StaffType", typeof(string));
                Staffs.Columns.Add("StaffMember", typeof(string));
                Staffs.Columns.Add("StaffID", typeof(string));
                Staffs.AcceptChanges();

                GetClientCategories();
                GetClientStatusCodes();
                GetClientSectors();
                GetClientIndusties();
                GetClientStaffTypes();
                GetSalesRoutes();
                GetRecoveryExpressCenter();
                GetOriginExpressCenters();
                GetPriceModifiers();
                //GetClientGroups();
                GetAllBranches();
                GetBanks();
                foreach (ListItem item in ddly_stafType.Items)
                {
                    if (item.Value == "0")
                    {
                        continue;
                    }
                    DataRow dr = Staffs.NewRow();
                    dr["StaffTypeID"] = item.Value;
                    dr["StaffType"] = item.Text;

                    Staffs.Rows.Add(dr);
                    Staffs.AcceptChanges();
                }
                gv_Staffs.DataSource = Staffs;
                gv_Staffs.DataBind();
                foreach (GridViewRow row in gv_Staffs.Rows)
                {
                    clvar.StaffType = (row.FindControl("hd_StaffTypeID") as HiddenField).Value;
                    DataTable dt = GetClientStaff_(clvar);
                    DropDownList dd = row.FindControl("dd_gStaffMembers") as DropDownList;
                    DataView dv = dt.AsDataView();
                    dd.Items.Add(new ListItem { Text = "Select Staff", Value = "0" });
                    dv.Sort = "username";
                    dd.DataSource = dv;
                    dd.DataTextField = "UserName";
                    dd.DataValueField = "ID";
                    dd.DataBind();

                }
                ViewState["Staffs"] = Staffs;
                ViewState["UPDATE"] = "0";

                txt_RegDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                //txt_RegDate.Enabled = false;
                txt_RegEndDate.Text = DateTime.Now.AddYears(1).AddDays(-1).ToString("dd/MM/yyyy");
                //txt_RegEndDate.Enabled = false;
            }
        }
        protected void GetServices()
        {
            string sqlString__15112019 = "SELECT * FROM ServiceTypes st where (st.serviceTypeName NOT LIKE '%EXPRESS%' OR st.serviceTypeName = 'EXPRESS CARGO') AND st.serviceTypeName NOT LIKE '%FE%' AND st.[status] = '1' AND st.IsIntl = '0'";

            string sqlString = "SELECT * \n"
               + "FROM   ServiceTypes st \n"
               + "WHERE  ( \n"
               + "           st.serviceTypeName NOT LIKE '%EXPRESS%' \n"
               + "           OR st.serviceTypeName = 'EXPRESS CARGO' \n"
               + "       ) \n"
               + "       AND st.serviceTypeName NOT LIKE '%FE%' \n"
               + "       AND st.serviceTypeName NOT IN ('Aviation Sale', 'Jazz Cash',  \n"
               + "                                     'Jazz Scratch Card 100',  \n"
               + "                                     'Jazz Scratch Card 1000',  \n"
               + "                                     'Jazz Scratch Card 300',  \n"
               + "                                     'Jazz Scratch Card 600',  \n"
               + "                                     'Jazz Scratch Card 625', 'Laptop',  \n"
               + "                                     'Mango Paiti Campaign', 'MB10', 'MB2',  \n"
               + "                                     'MB20', 'MB30', 'MB5', 'My Air Cargo',  \n"
               + "                                     'NTS') \n"
               + "       AND st.[status] = '1' \n"
               + "       AND st.IsIntl = '0'";

            SqlConnection con = new SqlConnection(clvar.Strcon());
            DataTable dt = new DataTable();
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sqlString, con);
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    chkl_services.DataSource = dt;
                    chkl_services.DataTextField = "ServiceTypeName";
                    chkl_services.DataValueField = "ServiceTypeName";
                    chkl_services.DataBind();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        protected void GetZones()
        {
            DataTable dt = new DataTable();

            string sqlString = "selecT z.name, z.zoneCode\n" +
            "  from zones z\n" +
            " where z.status = '1'\n" +
            "   and z.zoneCode in\n" +
            "       (select distinct b.zoneCode from branches b where b.status = '1')\n" +
            " order by 1";
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sqlString, con);
                sda.Fill(dt);
                dd_zone.DataSource = dt;
                dd_zone.DataTextField = "NAME";
                dd_zone.DataValueField = "ZoneCode";
                dd_zone.DataBind();
                foreach (ListItem item in dd_zone.Items)
                {
                    if (item.Value == HttpContext.Current.Session["ZoneCode"].ToString())
                    {
                        item.Selected = true;
                        break;
                    }
                }
                //dd_zone.Enabled = false;
                dd_zone_SelectedIndexChanged(this, EventArgs.Empty);
            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
        }
        protected void GetClientParents()
        {
            DataTable dt = new DataTable();

            string sqlString = "SELECT * FROM Clients_ParentGroup cpg WHERE cpg.[Status] = '1'";
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sqlString, con);
                sda.Fill(dt);
                dd_parent.DataSource = dt;
                dd_parent.DataTextField = "ParentGroupNAME";
                dd_parent.DataValueField = "id";
                dd_parent.DataBind();

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
        }
        protected void GetBanks()
        {
            DataTable dt = new DataTable();

            string sqlString = "SELECT * FROM Banks b WHERE b.isMNPBank = '0'";
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sqlString, con);
                sda.Fill(dt);
                dd_benBank.DataSource = dt;
                dd_benBank.DataTextField = "Name";
                dd_benBank.DataValueField = "id";
                dd_benBank.DataBind();

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
        }
        protected void GetClientCategories()
        {
            //DataTable dt = cm.GetClientCategories();
            //if (dt.Rows.Count > 0)
            //{
            //    //dd_CustomerCategory.Items.Clear();
            //    //dd_CustomerCategory.Items.Add(new ListItem { Text = ".::Select Category::.", Value = "0" });
            //    //dd_CustomerCategory.DataSource = dt;
            //    //dd_CustomerCategory.DataTextField = "name";
            //    //dd_CustomerCategory.DataValueField = "id";
            //    //dd_CustomerCategory.DataBind();
            //}
        }
        protected void GetClientStatusCodes()
        {
            DataTable dt = cm.GetClientStatusCodes();
            DataView dv = dt.AsDataView();
            dv.Sort = "Description";
            DataTable dt_ = dv.ToTable();
            if (dt.Rows.Count > 0)
            {
                dd_StatusCode.Items.Clear();
                dd_StatusCode.Items.Add(new ListItem { Text = ".::Status Code::.", Value = "0" });
                dd_StatusCode.DataSource = dt_;

                dd_StatusCode.DataTextField = "Description";
                dd_StatusCode.DataValueField = "Code";
                dd_StatusCode.DataBind();
                dd_StatusCode.SelectedValue = "AC";
                dd_StatusCode.Enabled = false;
            }
        }
        protected void GetClientSectors()
        {
            //DataTable dt = cm.GetClientSectors();
            //DataView dv = dt.AsDataView();
            //dv.Sort = "Name";
            //DataTable dt_ = dv.ToTable();
            //if (dt.Rows.Count > 0)
            //{
            //    dd_Sector.Items.Clear();
            //    dd_Sector.Items.Add(new ListItem { Text = ".::Select Sector::.", Value = "0" });
            //    dd_Sector.DataSource = dt_;
            //    dd_Sector.DataTextField = "Name";
            //    dd_Sector.DataValueField = "ID";
            //    dd_Sector.DataBind();
            //}
        }
        protected void GetClientIndusties()
        {
            DataTable dt = cm.GetClientIndustries();
            DataView dv = dt.AsDataView();
            dv.Sort = "Name";
            DataTable dt_ = dv.ToTable();
            if (dt.Rows.Count > 0)
            {
                dd_Industry.Items.Clear();
                dd_Industry.Items.Add(new ListItem { Text = ".::Select Industry::.", Value = "0" });
                dd_Industry.DataSource = dt_;
                dd_Industry.DataTextField = "Name";
                dd_Industry.DataValueField = "ID";
                dd_Industry.DataBind();
            }
        }
        protected void GetClientStaffTypes()
        {
            DataTable dt = cm.GetClientStaffTypes();
            DataView dv = dt.AsDataView();
            dv.Sort = "Code";

            DataTable dt_ = dv.ToTable();
            if (dt.Rows.Count > 0)
            {
                ddly_stafType.Items.Clear();
                ddly_stafType.Items.Add(new ListItem { Text = ".::Staff Type::.", Value = "0" });
                ddly_stafType.DataSource = dt_;
                ddly_stafType.DataTextField = "CODE";
                ddly_stafType.DataValueField = "ID";
                ddly_stafType.DataBind();
            }
        }

        protected void GetClientStaff()
        {
            ddl_staff_member.Items.Clear();
            clvar.StaffType = ddly_stafType.SelectedValue;
            DataTable dt =
            GetClientStaff(clvar);
            ddl_staff_member.Items.Add(new ListItem(".::Select Staff::.", "0"));

            if (dt.Rows.Count > 0)
            {
                DataView dv = dt.AsDataView();
                ddl_staff_member.Items.Clear();
                dv.Sort = "username";
                ddl_staff_member.DataSource = dv;
                ddl_staff_member.DataTextField = "UserName";
                ddl_staff_member.DataValueField = "ID";
                ddl_staff_member.DataBind();
            }
        }
        protected DataTable GetClientStaff_(CL_Customer clvar)
        {

            DataTable dt =
            GetClientStaff(clvar);
            return dt;
        }
        protected void GetSalesRoutes()
        {
            //DataTable dt = cm.GetSalesRoutes();

            //if (dt.Rows.Count > 0)
            //{
            //    dd_SalesRoute.Items.Clear();
            //    dd_SalesRoute.Items.Add(new ListItem { Text = ".::Select Route::.", Value = "0" });
            //    dd_SalesRoute.DataSource = dt;
            //    dd_SalesRoute.DataTextField = "name";
            //    dd_SalesRoute.DataValueField = "RouteCode";
            //    dd_SalesRoute.DataBind();
            //}
        }
        protected void GetRecoveryExpressCenter()
        {
            Cl_Variables clvar = new Cl_Variables();
            clvar.origin = dd_Branch.SelectedValue;// HttpContext.Current.Session["BranchCode"].ToString();
            DataTable dt = func.ExpressCenterOrigin(clvar).Tables[0];
            if (dt.Rows.Count > 0)
            {
                dd_recoveryExpID.Items.Clear();
                dd_recoveryExpID.Items.Add(new ListItem { Text = ".::Select Recovery EC::.", Value = "0" });
                dd_recoveryExpID.DataSource = dt;
                dd_recoveryExpID.DataTextField = "NAME";
                dd_recoveryExpID.DataValueField = "ExpressCenterCode";
                dd_recoveryExpID.DataBind();
            }
        }
        protected void GetOriginExpressCenters()
        {
            Cl_Variables clvar = new Cl_Variables();
            clvar.origin = dd_Branch.SelectedValue;// HttpContext.Current.Session["BranchCode"].ToString();
            DataTable dt = func.ExpressCenterOrigin(clvar).Tables[0];
            if (dt.Rows.Count > 0)
            {
                dd_originEC.Items.Clear();
                dd_originEC.Items.Add(new ListItem { Text = ".::Select Origin EC::.", Value = "0" });
                dd_originEC.DataSource = dt;
                dd_originEC.DataTextField = "NAME";
                dd_originEC.DataValueField = "ExpressCenterCode";
                dd_originEC.DataBind();
            }
        }
        protected void GetPriceModifiers()
        {
            DataTable dt = cm.GetPriceModifiers();
            if (dt.Rows.Count > 0)
            {
                ddl_tab4_name.Items.Clear();
                ddl_tab4_name.Items.Add(new ListItem { Text = ".::Select Price Modifier::.", Value = "0" });
                ddl_tab4_name.DataTextField = "name";
                ddl_tab4_name.DataValueField = "id";
                ddl_tab4_name.DataSource = dt;
                ddl_tab4_name.DataBind();
            }
            ViewState["priceModifiers"] = dt;
        }
        protected void GetClientGroups()
        {

            DataTable dt = GetClientGroups_();
            if (dt.Rows.Count > 0)
            {
                //dd_clientGroups.Items.Clear();
                //dd_clientGroups.Items.Add(new ListItem { Text = ".::Select Group::.", Value = "0" });
                //dd_clientGroups.DataSource = dt;
                //dd_clientGroups.DataTextField = "name";
                //dd_clientGroups.DataValueField = "ID";
                //dd_clientGroups.DataBind();


                dd_nationWideGroup.Items.Clear();
                dd_nationWideGroup.Items.Add(new ListItem { Text = ".::Select Group::.", Value = "0" });
                dd_nationWideGroup.DataSource = dt;
                dd_nationWideGroup.DataTextField = "name";
                dd_nationWideGroup.DataValueField = "ID";
                dd_nationWideGroup.DataBind();
            }
        }
        protected void CODTypes()
        {
            DataTable dt = GetCODTypes();

            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    dd_codType.Items.Clear();
                    dd_codType.Items.Add(new ListItem { Text = "Select COD Type", Value = "0" });
                    dd_codType.DataSource = dt;
                    dd_codType.DataTextField = "TypeName";
                    dd_codType.DataValueField = "TypeID";
                    dd_codType.DataBind();
                }
            }
        }
        public DataTable GetCODTypes()
        {
            string query = "select * from mnp_cod_types";
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

        public void GetAllBranches()
        {
            DataSet dt = Branch();
            if (dt != null)
            {
                if (dt.Tables[0].Rows.Count > 0)
                {
                    dd_parentBranch.DataSource = dt;
                    dd_parentBranch.DataTextField = "BranchName";
                    dd_parentBranch.DataValueField = "BranchCode";
                    dd_parentBranch.DataBind();

                    chkList_Branches.DataSource = dt;
                    chkList_Branches.DataTextField = "BranchName";
                    chkList_Branches.DataValueField = "BranchCode";
                    chkList_Branches.DataBind();

                    ViewState["Branches"] = dt;
                }

            }

        }

        #region -----TARRIF-----

        protected void btn_updatetariffPrice_Click(object sender, EventArgs e)
        {
            if (ViewState["dt_tariff2"] != null)
            {
                dt_tariff2 = (DataTable)ViewState["dt_tariff2"];
                if (dt_tariff2.Rows.Count > 0)
                {
                    foreach (GridViewRow row in gv_tariff.Rows)
                    {
                        if (dt_tariff2.Rows[row.RowIndex][5].ToString() == "0")
                        {
                            row.Cells[0].Enabled = false;
                        }
                        else
                        {
                            row.Cells[0].Enabled = true;
                            // CheckBox check = row.Cells[0].Controls[row.RowIndex] as CheckBox;
                            // check.Checked = true;
                        }

                        string Service = row.Cells[1].Text;
                        string FromWeight = row.Cells[2].Text;
                        string ToWeight = row.Cells[3].Text;
                        string AdditionalWeight = row.Cells[4].Text;

                        TextBox localzone = (TextBox)row.FindControl("txt_Local");
                        TextBox samezone = (TextBox)row.FindControl("txt_Same");
                        TextBox diffzone = (TextBox)row.FindControl("txt_Diff");
                        CheckBox chkRow = (row.Cells[0].FindControl("cb_pr") as CheckBox);

                        if (row.Cells[4].Text == "1" && !chkRow.Checked)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('For Additional Weight Check PR Flag')", true);
                            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "active", "openCity(event,'Tariff')", true);
                            return;
                        }

                        gv_tariff.Rows[row.RowIndex].Cells[5].BackColor = System.Drawing.Color.White;
                        gv_tariff.Rows[row.RowIndex].Cells[6].BackColor = System.Drawing.Color.White;
                        gv_tariff.Rows[row.RowIndex].Cells[7].BackColor = System.Drawing.Color.White;

                        if (localzone.Text == "")
                        {
                            gv_tariff.Rows[row.RowIndex].Cells[5].BackColor = System.Drawing.Color.Red;
                            gv_tariff.Rows[row.RowIndex].Cells[5].ForeColor = System.Drawing.Color.White;

                            localzone.Focus();
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Add Local Zone Amount')", true);
                            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "active", "openCity(event,'Tariff')", true);
                            return;
                        }
                        if (samezone.Text == "")
                        {
                            gv_tariff.Rows[row.RowIndex].Cells[6].BackColor = System.Drawing.Color.Red;
                            gv_tariff.Rows[row.RowIndex].Cells[6].ForeColor = System.Drawing.Color.White;
                            samezone.Focus();
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Add Same Zone Amount')", true);
                            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "active", "openCity(event,'Tariff')", true);
                            return;
                        }
                        if (diffzone.Text == "")
                        {
                            gv_tariff.Rows[row.RowIndex].Cells[7].BackColor = System.Drawing.Color.Red;
                            gv_tariff.Rows[row.RowIndex].Cells[7].ForeColor = System.Drawing.Color.White;
                            diffzone.Focus();
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Add Diff Zone Amount')", true);
                            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "active", "openCity(event,'Tariff')", true);
                            return;
                        }

                        DataRow dr_Tariff = dt_tariff2.Select("ServiceTypeName='" + Service + "' and FromWeight='" + FromWeight + "' and ToWeight='" + ToWeight + "' ").FirstOrDefault();
                        if (dr_Tariff != null)
                        {
                            if (chkRow.Checked)
                            {
                                dr_Tariff["cb_pr"] = "true";
                            }
                            else
                            {
                                dr_Tariff["cb_pr"] = "false";
                            }

                            dr_Tariff["LocalZone"] = localzone.Text;
                            dr_Tariff["SameZone"] = samezone.Text;
                            dr_Tariff["DiffZone"] = diffzone.Text;

                            DataView tempview = dt_tariff2.DefaultView;
                            tempview.Sort = "ServiceTypeName,FromWeight,ToWeight";

                            dt_tariff2 = tempview.ToTable();
                            Session["dt_"] = dt_tariff2;
                            ViewState["dt_tariff2"] = dt_tariff2;

                            //gv_tariff.DataSource = null;
                            //gv_tariff.DataBind();

                            //gv_tariff.DataSource = dt_tariff2;
                            //gv_tariff.DataBind();
                        }
                    }

                    gv_tariff.DataSource = null;
                    gv_tariff.DataBind();

                    gv_tariff.DataSource = dt_tariff2;
                    gv_tariff.DataBind();

                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Tariff Updated')", true);
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "active", "openCity(event,'Tariff')", true);

                    //string javascriptCode = @"var element = document.getElementById(""pendingTask""); element.classList.add(""active"");";
                    //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "MyScript", javascriptCode, true);
                }
            }
        }

        protected void btn_addPrice_Click(object sender, EventArgs e)
        {
            if (txt_fromWeight.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter From Weight')", true);
                return;
            }
            if (txt_toWeight.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter To Weight')", true);
                return;
            }

            #region MyRegion

            //DataTable tariff = Session["dt_tariff2"] as DataTable;

            // Checking Duplicate entry

            /*
            dt_tariff2 = (DataTable)ViewState["dt_tariff2"];
            if (dt_tariff2.Rows.Count == 0 && dt_tariff2 != null)
            {
                dt_tariff2.Columns.Remove("id");
                dt_tariff2.Columns.Remove("TempClientId");
                dt_tariff2.Columns.Remove("ServiceTypeName");
                dt_tariff2.Columns.Remove("FromWeight");
                dt_tariff2.Columns.Remove("ToWeight");
                dt_tariff2.Columns.Remove("AddFactor");
                dt_tariff2.Columns.Remove("LocalZone");
                dt_tariff2.Columns.Remove("SameZone");
                dt_tariff2.Columns.Remove("DiffZone");
                dt_tariff2.Columns.Remove("isUpdated");

                ViewState["dt_tariff2"] = null;
            }
            */

            //   dt_tariff2 = (DataTable)ViewState["dt_tariff2"];



            //if (ViewState["dt_tariff2"] != null)
            //{

            //}
            #endregion

            if (ViewState["dt_tariff2"] != null)
            {
                dt_tariff2 = (DataTable)ViewState["dt_tariff2"];
                if (dt_tariff2.Rows.Count > 0)
                {
                    cl_var.ToWeight = txt_toWeight.Text;
                    cl_var.FromWeight = txt_fromWeight.Text;

                    DataRow[] dr_ = dt_tariff2.Select("FromWeight='" + txt_fromWeight.Text + "' and ToWeight='" + txt_toWeight.Text + "' and ServiceTypeName='" + TariffService.SelectedValue + "'");
                    if (dr_.Length > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('From Weight Already Present')", true);
                        txt_fromWeight.Text = "";
                        txt_fromWeight.Focus();

                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "active", "openCity(event,'Tariff')", true);

                        return;
                    }
                    dr_ = null;


                    for (int i = 0; i < dt_tariff2.Rows.Count; i++)
                    {
                        DataRow[] dr_check = dt_tariff2.Select(txt_fromWeight.Text + "< " + dt_tariff2.Rows[i]["ToWeight"] + " and ServiceTypeName='" + TariffService.SelectedValue + "'");
                        if (dr_check.Length > 0)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('Weight Range Already Present')", true);
                            txt_toWeight.Text = "";
                            txt_fromWeight.Text = "";
                            txt_fromWeight.Focus();

                            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "active", "openCity(event,'Tariff')", true);

                            return;
                        }
                        dr_check = null;
                    }


                    #region Remove
                    /*
                    dr_ = dt_tariff2.Select("ToWeight='" + txt_toWeight.Text + "'");
                    dr_ = dt_tariff2.Select("FromWeight='" + txt_fromWeight.Text + "' and ToWeight='" + txt_toWeight.Text + "' and ServiceTypeName='" + TariffService.SelectedValue + "'");
                    if (dr_.Length > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('To Weight Already Present')", true);
                        txt_toWeight.Text = "";
                        txt_toWeight.Focus();
                        return;

                    }

                    if (dt_tariff2.Rows.Count > 0)
                    {
                        cl_var.AdditionalFactor = dt_tariff2.Rows[0]["addFactor"].ToString();
                    }
                    else
                    {
                        cl_var.AdditionalFactor = txt_additionalWeight.Text;
                    }
                    */

                    #endregion

                    DataRow dr = dt_tariff2.NewRow();
                    bool flag = false;

                    dr["ID"] = (dt_tariff2.Rows.Count + 1).ToString();
                    dr["FromWeight"] = cl_var.FromWeight;
                    dr["ToWeight"] = cl_var.ToWeight;
                    dr["ServiceTypeName"] = TariffService.SelectedValue;
                    dr["AddFactor"] = txt_additionalWeight.Text;
                    dr["cb_pr"] = "false";
                    #region REMOVE
                    //dr["LocalZone"] = dt_tariff2.Rows[0]["LocalZone"].ToString();
                    //dr["SameZone"] = dt_tariff2.Rows[0]["SameZone"].ToString();
                    //dr["DiffZone"] = dt_tariff2.Rows[0]["DiffZone"].ToString();
                    /*
                    if (dt_tariff2.Rows.Count > 0)
                    {
                        dr["addFactor"] = dt_tariff2.Rows[0]["addFactor"].ToString();
                    }
                    else
                    {
                        dr["addFactor"] = txt_additionalWeight.Text;
                    }
                    */
                    //   dr["isUpdated"] = "NEW";
                    //dr["client"] = creditclientid.Value; // -------------------- USED

                    #endregion

                    dt_tariff2.Rows.Add(dr);
                    DataView tempview = dt_tariff2.DefaultView;
                    tempview.Sort = "ServiceTypeName,FromWeight,ToWeight";

                    dt_tariff2 = tempview.ToTable();
                    Session["dt_"] = dt_tariff2;

                    gv_tariff.DataSource = null;
                    gv_tariff.DataBind();

                    //dt_tariff2.SetColumnsOrder("Qty", "Unit", "Id");
                    gv_tariff.DataSource = dt_tariff2;
                    gv_tariff.DataBind();

                    foreach (GridViewRow row in gv_tariff.Rows)
                    {
                        if (dt_tariff2.Rows[row.RowIndex][5].ToString() == "0")
                        {
                            row.Cells[0].Enabled = false;
                        }
                        else
                        {
                            row.Cells[0].Enabled = true;
                            // CheckBox check = row.Cells[0].Controls[row.RowIndex] as CheckBox;
                            // check.Checked = true;
                        }
                    }

                    btn_tariffupdate.Visible = true;
                    txt_fromWeight.Text = "";
                    this.txt_toWeight.Text = "";

                    //err_msg.Text = "Tariff has been saved Temporarily";
                    err_msg.Visible = false;
                }
            }
            else
            {
                // dt_tariff2 = null;
                dt_tariff2.Columns.Add(new DataColumn("ID", typeof(string)));
                dt_tariff2.Columns.Add(new DataColumn("TempClientId", typeof(string)));
                dt_tariff2.Columns.Add(new DataColumn("ServiceTypeName", typeof(string)));
                dt_tariff2.Columns.Add(new DataColumn("FromWeight", typeof(string)));
                dt_tariff2.Columns.Add(new DataColumn("ToWeight", typeof(string)));
                dt_tariff2.Columns.Add(new DataColumn("AddFactor", typeof(string)));
                dt_tariff2.Columns.Add(new DataColumn("LocalZone", typeof(string)));
                dt_tariff2.Columns.Add(new DataColumn("SameZone", typeof(string)));
                dt_tariff2.Columns.Add(new DataColumn("DiffZone", typeof(string)));
                dt_tariff2.Columns.Add(new DataColumn("isUpdated", typeof(string)));
                dt_tariff2.Columns.Add(new DataColumn("cb_pr", typeof(string)));
                //DataRow dr1 = dt_tariff2.NewRow();
                //dt_tariff2.Rows.Add(dr1);
                ViewState["dt_tariff2"] = dt_tariff2;

                cl_var.ToWeight = txt_toWeight.Text;
                cl_var.FromWeight = txt_fromWeight.Text;

                DataRow[] dr_ = dt_tariff2.Select("FromWeight='" + txt_fromWeight.Text + "'");
                if (dr_.Length > 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('From Weight Already Present')", true);
                    txt_fromWeight.Text = "";
                    txt_fromWeight.Focus();
                    return;

                }
                dr_ = null;
                dr_ = dt_tariff2.Select("ToWeight='" + txt_toWeight.Text + "'");
                if (dr_.Length > 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('To Weight Already Present')", true);
                    txt_toWeight.Text = "";
                    txt_toWeight.Focus();
                    return;

                }

                if (dt_tariff2.Rows.Count > 0)
                {
                    cl_var.AdditionalFactor = dt_tariff2.Rows[0]["addFactor"].ToString();
                }
                else
                {
                    cl_var.AdditionalFactor = txt_additionalWeight.Text;
                }
                DataRow dr = dt_tariff2.NewRow();
                bool flag = false;

                dr["ID"] = (dt_tariff2.Rows.Count + 1).ToString();
                dr["FromWeight"] = cl_var.FromWeight;
                dr["ToWeight"] = cl_var.ToWeight;
                dr["ServiceTypeName"] = TariffService.SelectedValue;
                dr["cb_pr"] = "false";

                if (dt_tariff2.Rows.Count > 0)
                {
                    dr["addFactor"] = dt_tariff2.Rows[0]["addFactor"].ToString();
                }
                else
                {
                    dr["addFactor"] = txt_additionalWeight.Text;
                }
                //   dr["isUpdated"] = "NEW";
                //dr["client"] = creditclientid.Value; // -------------------- USED

                dt_tariff2.Rows.Add(dr);
                DataView tempview = dt_tariff2.DefaultView;
                tempview.Sort = "FromWeight";

                dt_tariff2 = tempview.ToTable();
                Session["dt_"] = dt_tariff2;

                gv_tariff.DataSource = null;
                gv_tariff.DataBind();

                gv_tariff.DataSource = dt_tariff2;
                gv_tariff.DataBind();

                foreach (GridViewRow row in gv_tariff.Rows)
                {
                    CheckBox check = row.Cells[0].Controls[0] as CheckBox;

                    if (txt_additionalWeight.Text.ToString() == "0")
                    {
                        row.Cells[0].Enabled = false;
                    }
                    else
                    {
                        row.Cells[0].Enabled = true;
                    }

                    if (check != null)
                    {
                        check.Enabled = true;
                    }
                    else
                    {
                        //check.Enabled = false;
                    }
                }

                btn_tariffupdate.Visible = true;
                txt_fromWeight.Text = "";
                this.txt_toWeight.Text = "";

                //err_msg.Text = "Tariff has been saved Temporarily";
                err_msg.Visible = false;

            }
            txt_additionalWeight.Text = "0";
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "openCity(event,'Tariff')", true);

            /*
            cl_var.ToWeight = txt_toWeight.Text;
            cl_var.FromWeight = txt_fromWeight.Text;

            DataRow[] dr_ = tariff2.Select("FromWeight='" + txt_fromWeight.Text + "'");
            if (dr_.Length > 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('From Weight Already Present')", true);
                txt_fromWeight.Text = "";
                txt_fromWeight.Focus();
                return;

            }
            dr_ = null;
            dr_ = tariff2.Select("ToWeight='" + txt_toWeight.Text + "'");
            if (dr_.Length > 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('To Weight Already Present')", true);
                txt_toWeight.Text = "";
                txt_toWeight.Focus();
                return;

            }

            if (tariff.Rows.Count > 0)
            {
                cl_var.AdditionalFactor = tariff.Rows[0]["addFactor"].ToString();
            }
            else
            {
                cl_var.AdditionalFactor = txt_additionalWeight.Text;
            }
            DataRow dr = tariff.NewRow();
            bool flag = false;

            dr["ID"] = (tariff.Rows.Count + 1).ToString();
            dr["FromWeight"] = cl_var.FromWeight;
            dr["ToWeight"] = cl_var.ToWeight;

            if (tariff.Rows.Count > 0)
            {
                dr["addFactor"] = tariff.Rows[0]["addFactor"].ToString();
            }
            else
            {
                dr["addFactor"] = txt_additionalWeight.Text;
            }
            //   dr["isUpdated"] = "NEW";
            //dr["client"] = creditclientid.Value; // -------------------- USED

            tariff.Rows.Add(dr);
            DataView tempview = tariff.DefaultView;
            tempview.Sort = "FromWeight";

            tariff = tempview.ToTable();
            Session["dt_"] = tariff;

            gv_tariff.DataSource = null;
            gv_tariff.DataBind();

            gv_tariff.DataSource = tariff;
            gv_tariff.DataBind();


            txt_fromWeight.Text = "";
            this.txt_toWeight.Text = "";

            //err_msg.Text = "Tariff has been saved Temporarily";
            err_msg.Visible = false;

            */
        }

        protected void cb_pr_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox c = (CheckBox)sender;
            int count = 0;
            foreach (GridViewRow gr in gv_tariff.Rows)
            {
                CheckBox cb = (CheckBox)gr.FindControl("cb_pr");
                if (cb.Checked == true)
                {
                    count++;

                    if (count > 1)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('PR can only be selected once')", true);
                        if (cb.ClientID == c.ClientID)
                        {
                            cb.Checked = false;

                            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "active", "openCity(event,'Tariff')", true);
                            return;
                        }
                    }
                    else
                    {
                        //  (gr.FindControl("txt_Local") as TextBox).Enabled = true;

                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "active", "openCity(event,'Tariff')", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "active", "openCity(event,'Tariff')", true);
                }
            }
        }

        protected void btn_tariff_service(object sender, EventArgs e)
        {
            int lastSelectedIndex = 0;
            string lastSelectedValue = string.Empty;

            foreach (ListItem listitem in chkl_services.Items)
            {
                if (listitem.Selected)
                {
                    int thisIndex = chkl_services.Items.IndexOf(listitem);

                    if (lastSelectedIndex < thisIndex)
                    {
                        lastSelectedIndex = thisIndex;
                        lastSelectedValue = listitem.Value;

                        ViewState["service_dt"] = null;

                        DataRow dr = dt_service.NewRow();
                        dr["Service"] = lastSelectedValue;
                        dt_service.Rows.Add(dr);
                        ViewState["service_dt"] = dt_service;
                    }
                }
            }

            if (dt_service.Rows.Count > 0)
            {
                TariffService.DataTextField = "Service";
                TariffService.DataValueField = "Service";
                TariffService.DataSource = dt_service;
                TariffService.DataBind();

                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "openCity(event,'Services')", true);
            }




        }

        protected void gv_tariff_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //if (txt_accountNo.Text == "0")
            //{
            //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cannot Update Cash Tariff')", true);
            //    err_msg.Text = "Cannot Update Cash Tariff";
            //    return;
            //}
            if (e.CommandName == "del")
            {
                cl_var.TariffID = e.CommandArgument.ToString();
                DataTable dt = ViewState["dt_tariff2"] as DataTable;
                //DataRow dr = dt.Select("DSlipNo = '" + con + "'")[0];
                DataRow dr = dt.Select("ID = '" + cl_var.TariffID + "'", "")[0];

                DataView tempview = dt.DefaultView;
                tempview.Sort = "ServiceTypeName,FromWeight,ToWeight";

                dt.Rows.Remove(dr);
                dt.AcceptChanges();
                ViewState["dt_tariff2"] = dt;

                if (dt.Rows.Count > 0)
                {
                    gv_tariff.DataSource = null;
                    gv_tariff.DataBind();
                    gv_tariff.DataSource = dt;
                    gv_tariff.DataBind();
                }
                else
                {
                    dt.Rows.Clear();
                    ViewState["dt_tariff2"] = null;
                    gv_tariff.DataSource = null;
                    gv_tariff.DataBind();
                    gv_tariff.DataSource = dt;
                    gv_tariff.DataBind();
                }

                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "openCity(event,'Tariff')", true);
                /*
                cl_var.TariffID = e.CommandArgument.ToString();
                if (cl_var.TariffID == "")
                {
                    GridViewRow gr = (GridViewRow)sender;
                    gv_tariff.DeleteRow(gr.RowIndex);
                }

                int count = DeleteTariff(cl_var);

                if (count != 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Tariff Deleted')", true);
                    DataTable dt = Session["dt_"] as DataTable;

                    DataRow[] dr = dt.Select("ID = '" + cl_var.TariffID + "'", "");
                    dt.Rows.Remove(dr[0]);
                    dt.AcceptChanges();
                    Session["dt_"] = dt;
                    gv_tariff.DataSource = null;
                    gv_tariff.DataBind();
                    gv_tariff.DataSource = dt;
                    gv_tariff.DataBind();
                    return;
                }

                err_msg.Text = "Tariff has been Removed";
                err_msg.Visible = true;
                */
            }
            if (e.CommandName == "Update")
            {

                cl_var.TariffID = e.CommandArgument.ToString();
                GridViewRow gr = null;
                GridView grid = sender as GridView;
                int index = 0;

                index = Convert.ToInt32(e.CommandArgument);
                gr = grid.Rows[index - 1];
                ((Button)gv_tariff.Rows[index - 1].FindControl("btn_Update")).Enabled = false;

                //Now Updating The back Table

                string Same = ((TextBox)gr.FindControl("txt_Same")).Text;
                string Diff = ((TextBox)gr.FindControl("txt_Diff")).Text;
                string Local = ((TextBox)gr.FindControl("txt_Local")).Text;
                bool pr = ((CheckBox)gr.FindControl("cb_pr")).Checked;

                if (Same == "" || Same.Length == 0)
                {
                    Same = "0";
                }
                if (Diff == "" || Diff.Length == 0)
                {
                    Diff = "0";
                }
                if (Local == "" || Local.Length == 0)
                {
                    Local = "0";
                }

                DataTable dt = Session["dt_"] as DataTable;
                DataRow[] dr = dt.Select("ID = '" + cl_var.TariffID + "'", "");

                dr[0]["Local"] = Local;
                dr[0]["Same"] = Same;
                dr[0]["Diff"] = Diff;
                dr[0]["Pr"] = pr;
                dr[0]["isUpdated"] = "NEW";


                dt.AcceptChanges();
                DataView tempview = dt.DefaultView;
                tempview.Sort = "FromWeight";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('Tariff has been saved Temporarily')", false);
                //  TextBox txt = (TextBox)gr.RowIndex[index].FindControl("TextBox4");


                //   ((HiddenField)gr.FindControl("hd_isupdated")).Value = "1";

                dt = tempview.ToTable();
                Session["dt_"] = dt;
                Session["dt_1"] = dt;
                gv_tariff.DataSource = dt;
                gv_tariff.DataBind();

                // Now Disabling the Textbox


                //   ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Selected Document is Credit Note.')", true);

                //err_msg.Text = "Tariff has been saved Temporarily";
                //err_msg.Visible = true;

            }
        }

        protected void gv_tariff_DataBound(object sender, EventArgs e)
        {

        }

        protected void gv_tariff_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string id = ((HiddenField)e.Row.FindControl("hd_id")).Value;// Id

                DataTable dt = Session["dt_"] as DataTable;
                DataRow[] dr = dt.Select("ID = '" + id + "'", "");
                if (dr.Length > 0)
                {
                    ((TextBox)e.Row.FindControl("txt_Same")).Text = dr[0]["SameZone"].ToString();
                    ((TextBox)e.Row.FindControl("txt_Diff")).Text = dr[0]["DiffZone"].ToString();
                    ((TextBox)e.Row.FindControl("txt_Local")).Text = dr[0]["LocalZone"].ToString();

                    if (dt_tariff2.Rows[e.Row.RowIndex][5].ToString() == "0")
                    {
                        e.Row.Cells[0].Enabled = false;
                    }
                    else
                    {
                        e.Row.Cells[0].Enabled = true;
                        // CheckBox check = row.Cells[0].Controls[row.RowIndex] as CheckBox;
                        // check.Checked = true;
                    }

                    if (((HiddenField)e.Row.FindControl("hd_isupdated")).Value == "NEW")
                    {
                        //((TextBox)e.Row.FindControl("txt_Diff")).Enabled = false;

                        //((TextBox)e.Row.FindControl("txt_Local")).Enabled = false;
                        //((TextBox)e.Row.FindControl("txt_Same")).Enabled = false;
                        //((Button)e.Row.FindControl("btn_Update")).Enabled = false;
                        //(e.Row.FindControl("btn_Update") as Button).Enabled = false;

                        System.Web.UI.WebControls.Button btn = (System.Web.UI.WebControls.Button)e.Row.Cells[7].Controls[1];
                        btn.Enabled = false;
                        //   e.Row.Cells[7].Enabled = false;
                    }
                }
            }
        }

        protected void gv_tariff_SelectedIndexChanged(object sender, EventArgs e)
        {

        }



        #endregion

        protected void btn_save_Click_Temp(object sender, EventArgs e)
        {
            #region Validations

            if (dd_zone.SelectedValue == "0")
            {
                Alert("Select Zone");
                dd_zone.Focus();
                return;
            }
            if (dd_Branch.SelectedValue == "0")
            {
                Alert("Select Branch");
                dd_Branch.Focus();
                return;
            }

            if (txt_name.Text.Trim() == "")
            {
                Alert("Name is Mendatory");
                txt_name.Focus();
                return;
            }

            if (txt_contactPerson.Text.Trim() == "")
            {
                Alert("Contact Person is Mendatory");
                txt_contactPerson.Focus();
                return;
            }
            if (txt_designation.Text.Trim() == "")
            {
                Alert("Contact Person Designation is mandatory.");
                txt_designation.Focus();
                return;
            }
            //if (dd_CustomerCategory.SelectedValue == "0")
            //{
            //    Alert("Category is Mendatory");
            //    dd_CustomerCategory.Focus();
            //    return;
            //}
            if (txt_PhoneNo.Text.Trim() == "")
            {
                Alert("Phone No is Mendatory");
                txt_PhoneNo.Focus();
                return;
            }
            if (txt_email.Text.Trim() == "")
            {
                txt_email.Focus();
                Alert("Email is Mendatory");
                return;

            }
            if (txt_pickupInstruction.Text.Trim() == "")
            {
                txt_pickupInstruction.Focus(); Alert("Pickup Instructions is Mendatory");
                return;

            }
            if (txt_RegDate.Text.Trim() == "")
            {
                txt_RegDate.Focus();
                Alert("Reg Date is Mendatory");
                return;

            }
            if (txt_RegEndDate.Text.Trim() == "")
            {
                txt_RegEndDate.Focus();
                Alert("Reg End is Mendatory");
                return;
            }
            if (txt_OfficialAddress.Text.Trim() == "")
            {
                txt_OfficialAddress.Focus();
                Alert("Official Address is Mendatory");
                return;

            }
            if (txt_MailingAddress.Text.Trim() == "")
            {
                txt_MailingAddress.Focus();
                Alert("Mailing Address is Mendatory");
                return;

            }
            if (dd_StatusCode.SelectedValue == "0")
            {
                dd_StatusCode.Focus();
                Alert("Status Code is Mendatory");
                return;

            }
            //if (dd_Sector.SelectedValue == "0")
            //{
            //    dd_Sector.Focus();
            //    Alert("Sector is Mendatory");
            //    return;

            //}
            if (dd_Industry.SelectedValue == "0")
            {
                dd_Industry.Focus();
                Alert("Industry is Mendatory");
                return;

            }
            if (dd_prepareBill.SelectedValue == "0" && HttpContext.Current.Session["PROFILE"].ToString() != "16")
            {
                dd_prepareBill.Focus();
                Alert("Prepare Bill selection is Mendatory");
                return;

            }
            if (dd_originEC.SelectedValue == "0")
            {
                dd_originEC.Focus();
                Alert("Origin EC is Mendatory");
                return;

            }
            if (dd_recoveryExpID.SelectedValue == "0")
            {
                dd_recoveryExpID.Focus();
                Alert("Recovery EC is Mendatory");
                return;

            }
            if (txt_SalesTaxNo.Text.Trim() == "")
            {
                txt_SalesTaxNo.Focus();
                Alert("Sales Tax No is Mendatory");
                return;

            }
            if (txt_ntnNo.Text.Trim() == "")
            {
                txt_ntnNo.Focus();
                Alert("NTN No is Mendatory");
                return;

            }

            if (dd_fafType.SelectedValue == "")
            {
                dd_fafType.Focus();
                Alert("Select FAF Type");
                return;
            }

            if (rbtn_codMonthlyBillingAmount.SelectedValue == "1")
            {
                float tempMinAmount = 0;
                float.TryParse(txt_codMonthlyAmount.Text, out tempMinAmount);

                if (tempMinAmount <= 0)
                {
                    Alert("Enter Proper Min Billing Amount");
                    return;
                }
                else
                {
                    isMinBilling = true;
                    minBillingAmount = tempMinAmount;
                }
            }
            else
            {
                minBillingAmount = 0;
                isMinBilling = false;
            }
            if (txt_RnRWeight.Text != "")
            {
                float tempRNRWeight = 0;
                float.TryParse(txt_RnRWeight.Text, out tempRNRWeight);
                if (tempRNRWeight <= 0)
                {
                    Alert("Enter Proper RNR Weight");
                    return;
                }
                else
                {
                    rnrWeight = tempRNRWeight.ToString();
                }
            }
            else
            {
                Alert("Enter RNR Weight");
                return;
            }
            #endregion

            if (ViewState["dt_tariff2"] == null)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('First Add Tariff')", true);
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "active", "openCity(event,'Tariff')", true);
                return;
            }

            if (ViewState["dt_tariff2"] != null)
            {
                dt_tariff2 = (DataTable)ViewState["dt_tariff2"];
            }


            if (chkl_services.Items.Cast<ListItem>().Where(c => c.Selected).Count() == 0)
            {
                AlertMessage("No Service Selected For Client");
                return;
            }
            try
            {

                //--- For Add Tariff Start
                ViewState["dt_tariff2"] = dt_tariff2;
                //--- For Add Tariff Start

                DateTime regDate = DateTime.Parse(txt_RegDate.Text);
                DateTime regEndDate = DateTime.Parse(txt_RegEndDate.Text);
                if (regEndDate <= regDate)
                {
                    AlertMessage("Reg Date cannot be less than Reg End Date");
                    return;
                }

                char[] charArray = txt_name.Text.ToCharArray();
                string tempConsigner = "";
                for (int i = 0; i < charArray.Length; i++)
                {
                    if (!char.IsDigit(charArray[i]))
                    {
                        cl_var.Consigner += charArray[i].ToString();
                        tempConsigner += charArray[i].ToString();
                    }
                }

                DataTable TempClientId = GetTempClientId(cl_var);
                cl_var.Consigner = txt_name.Text;
                if (TempClientId != null)
                {
                    if (TempClientId.Rows.Count > 0)
                    {
                        clvar.AccountNo = dd_Branch.SelectedValue.ToString() + tempConsigner.ToCharArray()[0] + TempClientId.Rows[0][0].ToString();
                        /*
                        if (TempClientId.Rows[0][0].ToString() == "10000000")
                        {
                            clvar.AccountNo = dd_Branch.SelectedValue.ToString() + tempConsigner.ToCharArray()[0] + TempClientId.Rows[0][0].ToString();
                        }
                        else
                        {
                            clvar.AccountNo = dd_Branch.SelectedValue.ToString() + tempConsigner.ToCharArray()[0] + TempClientId.Rows[0][0].ToString();
                        }
                        */
                    }
                    else
                    {
                        Alert("Account Could not be Generated. Contact IT Support.");
                        return;
                    }
                }



                //BASIC INFO STARTS
                BeneficiaryBankAccNo = txt_benAccNo.Text;
                BeneficiaryBankCode = dd_benBank.SelectedValue;
                BeneficiaryName = txt_benName.Text;
                ContactPersonDesignation = txt_designation.Text;
                clvar.Name = txt_name.Text;
                clvar.Name = clvar.Name.Replace("'", "");
                clvar.ContactPerson = txt_contactPerson.Text;
                //clvar.AccountNo = txt_acc_no.Text;
                clvar.IsSpecial = chk_special_cust.Checked.ToString();
                //clvar.Category = dd_CustomerCategory.SelectedValue;
                clvar.FaxNo = txt_FaxNo.Text;
                clvar.PhoneNo = txt_PhoneNo.Text;
                clvar.OfficialAddress = txt_OfficialAddress.Text;
                clvar.MailingAddress = txt_MailingAddress.Text;
                clvar.PickupInstructions = txt_pickupInstruction.Text;
                clvar.RegDate = txt_RegDate.Text;
                clvar.RegEndDate = txt_RegEndDate.Text;
                clvar.CustomerEmail = txt_email.Text;
                clvar.CreditClientType = rbtn_customerType.SelectedValue;
                clvar.IsCentralized = chk_centralisedClient.Checked.ToString();
                clvar.StatusCode = dd_StatusCode.Text;
                clvar.IsCOD = chk_NotCod.Checked.ToString();
                //clvar.IsSmsServiceActive = chk_active_sms_service.Checked.ToString();
                clvar.CODType = dd_codType.SelectedValue;
                //BASIC INFO ENDS

                //CONFIGURATION STARTS
                //clvar.Sector = dd_Sector.SelectedValue;
                clvar.Industry = dd_Industry.SelectedValue;
                //clvar.ClientGroupID = txt_clientGroups.Text; //dd_clientGroups.SelectedValue;
                clvar.Status = rbtn_Status.SelectedValue;
                clvar.PrintingStatus = rbtn_printStatus.SelectedValue;
                //clvar.RedeemWindow = txt_redeemWindow.Text;
                clvar.DomesticAmonTo = txt_DomesticTurnOver.Text;
                clvar.InternationalAmonTo = txt_InternationalTurnOver.Text;
                clvar.DomesticPackets = txt_DomesticPackets.Text;
                clvar.InternationalPackets = txt_InternationalPackets.Text;
                clvar.DomesticAmount = txt_DomesticAmount.Text;
                clvar.InternationalAmount = txt_InternationalAmount.Text;
                //CONFIGURATION ENDS

                //BILLING AND SALES START
                ParentID = dd_parent.SelectedValue;
                ispiece_only = dd_pieces.SelectedValue;
                IsDestination = dd_destination.SelectedValue;
                clvar.PrepareBillType = dd_prepareBill.SelectedValue;
                clvar.BillingMode = rbtn_billingMode.SelectedValue;
                clvar.DiscountOnDocument = txt_DiscountOnDocument.Text;
                clvar.DiscountOnDomestic = txt_DiscountOnDomestic.Text;
                clvar.DiscountOnSample = txt_DiscountOnSample.Text;
                clvar.CreditLimit = txt_CreditLimit.Text;
                clvar.CODMonthlyBillingAmount = txt_codMonthlyAmount.Text;
                //clvar.RnRCharges = txt_RnRCharges.Text;
                clvar.BillTaxType = rbtn_BillTaxType.SelectedValue;
                //clvar.SalesRoute = dd_SalesRoute.SelectedValue;
                clvar.RecoveryExpID = dd_recoveryExpID.SelectedValue;
                clvar.SalesTaxNo = txt_SalesTaxNo.Text;
                //clvar.Memo = txt_memo.Text;
                clvar.NTNNo = txt_ntnNo.Text;
                clvar.OriginEc = dd_originEC.SelectedValue;

                //BILLING AND SALES END

                DataTable addedPriceModifiers = ViewState["addedPriceModifiers"] as DataTable;
                DataTable Staffs = ViewState["Staffs"] as DataTable;

                foreach (GridViewRow row in gv_Staffs.Rows)
                {
                    string staffType = row.Cells[0].Text;
                    string staffTypeID = (row.FindControl("hd_StaffTypeID") as HiddenField).Value;
                    DropDownList ddstaff = row.FindControl("dd_gStaffMembers") as DropDownList;
                    DataRow dr = Staffs.Select("stafftypeid='" + staffTypeID + "'")[0];
                    if (ddstaff.SelectedItem.Text.ToUpper() != "SELECT STAFF")
                    {
                        dr["staffMember"] = ddstaff.SelectedItem.Text;
                        dr["StaffID"] = ddstaff.SelectedValue;

                    }
                    else
                    {
                        if (staffTypeID == "214" || staffTypeID == "215")
                        {
                            Alert("Select " + staffType);
                            ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc( 'billingNsales');", true);
                            return;
                        }
                        else
                        {
                            continue;
                        }

                    }
                }


                DataTable dtService = new DataTable();
                dtService.Columns.AddRange(new DataColumn[] {
                new DataColumn("TempClientID", typeof(Int64)),
                new DataColumn("ServiceTypeName", typeof(string)),
                new DataColumn("Status", typeof(int))
            });

                string error = "";
                if (ViewState["UPDATE"].ToString() == "1")
                {
                    foreach (ListItem item in chkl_services.Items)
                    {
                        DataRow sRow = dtService.Rows.Add(Int64.Parse(hd_tempClientID.Value), item.Value, 0);
                        if (item.Selected)
                        {
                            sRow["Status"] = 1;
                        }
                        else
                        {
                            sRow["Status"] = 0;
                        }
                    }
                    DataTable pmTableType = new DataTable();
                    pmTableType.Columns.Add("TempClientID", typeof(string));
                    pmTableType.Columns.Add("PriceModifierID", typeof(string));
                    pmTableType.Columns.Add("SortOrder", typeof(string));
                    pmTableType.Columns.Add("ModifiedCalculationValue", typeof(string));
                    pmTableType.Columns.Add("CalculationBase", typeof(string));

                    DataTable staffTableType = new DataTable();
                    staffTableType.Columns.Add("id", typeof(Int64));
                    staffTableType.Columns.Add("TempClientID", typeof(Int64));
                    staffTableType.Columns.Add("UserName", typeof(string));
                    staffTableType.Columns.Add("StaffTypeID", typeof(int));

                    int i = 1;
                    foreach (GridViewRow row in gridview_tab4.Rows)
                    {
                        DataRow dr = pmTableType.NewRow();
                        dr["TempClientID"] = hd_tempClientID.Value;
                        dr["priceModifierID"] = (row.FindControl("del") as LinkButton).CommandArgument.ToString();
                        dr["ModifiedCalculationValue"] = (row.FindControl("txt_gValue") as TextBox).Text;
                        dr["CalculationBase"] = row.Cells[3].Text.ToString();
                        dr["SortOrder"] = "0";
                        pmTableType.Rows.Add(dr);
                    }


                    foreach (DataRow dr in Staffs.Rows)
                    {
                        if (dr["StaffMember"].ToString() == "")
                        {
                            continue;
                        }
                        DataRow row = staffTableType.NewRow();
                        row["id"] = 0;
                        row["TempClientID"] = hd_tempClientID.Value;
                        row["UserName"] = dr["StaffMember"].ToString();
                        row["StaffTypeID"] = dr["StaffTypeID"].ToString();
                        staffTableType.Rows.Add(row);
                    }

                    if (!chk_nationWide.Checked)
                    {
                        clvar.ClientGroupID = txt_nationWideGroup.Text;
                        clvar.RnRCharges = hd_tempClientID.Value;
                        if (cl_var.Remarks == "1")
                        {
                            cl_var.Remarks = "1";
                        }
                        else
                        {
                            cl_var.Remarks = "0";
                        }
                        error = UpdateTempCustomer(clvar, pmTableType, staffTableType, hd_tempClientID.Value, dtService);
                        error = DeleteCustomerTariff(clvar);
                    }
                    else
                    {
                        /*
                        DataTable branchesType = new DataTable();
                        branchesType.Columns.AddRange(new DataColumn[]{
                         new DataColumn("BranchCode", typeof(string)),
                         new DataColumn("Status", typeof(string)),
                         new DataColumn("Parent", typeof(string))
                        });

                        bool parentFound = false;
                        foreach (ListItem item in chkList_Branches.Items)
                        {
                            string parent = "0";
                            DataRow dr = branchesType.NewRow();

                            string status = "0";
                            if (item.Value == dd_parentBranch.SelectedValue && item.Selected)
                            {
                                parent = "1";
                                parentFound = true;
                            }
                            if (item.Selected)
                            {
                                status = "1";
                            }
                            else
                            {
                                status = "0";
                            }

                            branchesType.Rows.Add(item.Value, status, parent);

                        }
                        clvar.AccountNo = txt_tempclient_no.Text;
                        if (!parentFound)
                        {
                            Alert("Select Parent Branch");
                            return;
                        }

                        clvar.ClientGroupID = txt_nationWideGroup.Text;
                        error = UpdateNationWiseCustomer(clvar, pmTableType, staffTableType, hd_creditClientID.Value, branchesType, dtService);
                        */
                    }


                    if (error != "")
                    {
                        Alert("Account Could not be Updated. \r\n Error: " + error);
                        Errorid.Text = "Account Could not be Updated. \r\n Error: " + error;
                        Errorid.ForeColor = System.Drawing.Color.Red;
                        div2.Style.Add("display", "none");
                        //ScriptManager.RegisterStartupScript(Page, GetType(), "CallMyFunction", "openCity(event, 'BasicInfo')", false);
                        //ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc( 'BasicInfo');", true);
                        return;
                    }
                    else
                    {
                        Alert("Account " + txt_acc_no.Text + " Updated Successfully");
                        Errorid.Text = "Account " + txt_acc_no.Text + " Updated Successfully";
                        Errorid.ForeColor = System.Drawing.Color.Green;
                        ResetAll();
                    }
                }
                else if (ViewState["UPDATE"].ToString() == "0")
                {

                    foreach (ListItem item in chkl_services.Items)
                    {
                        if (item.Selected)
                        {
                            DataRow sRow = dtService.Rows.Add(0, item.Value, "1");
                        }
                    }

                    if (chk_nationWide.Checked)
                    {
                        if (txt_nationWideAccountNo.Text.Trim() == "")
                        {
                            Alert("Provide Nationwide Account Number");
                            return;
                        }
                        if (dd_parentBranch.SelectedValue == "0")
                        {
                            Alert("Provide Parent Branch");
                            return;
                        }
                        if (txt_nationWideGroup.Text.Trim() == "")
                        {
                            Alert("Provide Nationwide Group");
                            return;
                        }

                        string parentBranch = dd_parentBranch.SelectedValue;
                        clvar.AccountNo = txt_nationWideAccountNo.Text;
                        clvar.ClientGroupID = txt_nationWideGroup.Text; //dd_nationWideGroup.SelectedValue;
                        DataTable dt = new DataTable();
                        dt.Columns.Add("BranchCode");
                        dt.Columns.Add("ZoneCode");
                        dt.Columns.Add("ExpressCenterCode");

                        DataTable zones = (ViewState["Branches"] as DataSet).Tables[0];
                        foreach (ListItem item in chkList_Branches.Items)
                        {
                            if (item.Selected)
                            {
                                DataRow dr = dt.NewRow();
                                dr["BranchCode"] = item.Value;
                                dr["ZoneCode"] = zones.Select("BranchCode = '" + item.Value + "'")[0]["ZoneCode"].ToString();
                                dr["ExpressCenterCode"] = zones.Select("BranchCode = '" + item.Value + "'")[0]["expressCenterCode"].ToString();
                                dt.Rows.Add(dr);
                            }
                        }
                        if (dt.Rows.Count == 0)
                        {
                            Alert("Select Nation wide Branches");
                            return;
                        }
                        DataTable tempModifiers = addedPriceModifiers.Clone();
                        tempModifiers.Clear();
                        foreach (DataRow item in dt.Rows)
                        {
                            tempModifiers.Merge(addedPriceModifiers);

                        }
                        error = AddNationWideCustomer_Temp(clvar, addedPriceModifiers, Staffs, dt, parentBranch, dtService);

                    }
                    else if (chk_centralisedClient.Checked)
                    {

                        string parentBranch = dd_parentBranch.SelectedValue;
                        clvar.AccountNo = txt_nationWideAccountNo.Text;
                        clvar.ClientGroupID = txt_nationWideGroup.Text; //dd_nationWideGroup.SelectedValue;
                        DataTable dt = new DataTable();
                        dt.Columns.Add("BranchCode");
                        dt.Columns.Add("ZoneCode");
                        dt.Columns.Add("ExpressCenterCode");

                        DataTable zones = (ViewState["Branches"] as DataSet).Tables[0];
                        //foreach (ListItem item in chkList_Branches.Items)
                        //{
                        //if (item.Selected)
                        //{
                        DataRow dr = dt.NewRow();
                        dr["BranchCode"] = dd_Branch.SelectedValue;
                        dr["ZoneCode"] = zones.Select("BranchCode = '" + dd_Branch.SelectedValue + "'")[0]["ZoneCode"].ToString();
                        dr["ExpressCenterCode"] = zones.Select("BranchCode = '" + dd_Branch.SelectedValue + "'")[0]["expressCenterCode"].ToString();
                        dt.Rows.Add(dr);
                        //}
                        //}
                        if (dt.Rows.Count == 0)
                        {
                            Alert("Select Nation wide Branches");
                            return;
                        }
                        DataTable tempModifiers = addedPriceModifiers.Clone();
                        tempModifiers.Clear();
                        foreach (DataRow item in dt.Rows)
                        {
                            tempModifiers.Merge(addedPriceModifiers);

                        }
                        //------  error = AddCentralizedCustomer(clvar, addedPriceModifiers, Staffs, dt, parentBranch);
                    }
                    else
                    {
                        // error = AddCustomer(clvar, addedPriceModifiers, Staffs, dtService);
                        error = Temp_AddCustomer(clvar, addedPriceModifiers, Staffs, dtService);

                    }
                    if (error != "")
                    {

                        Alert("Account Could not be Created. \r\n Error: " + error);
                        Errorid.Text = "Account Could not be Created. " + error;
                        Errorid.ForeColor = System.Drawing.Color.Red;
                        div2.Style.Add("display", "none");
                        //ScriptManager.RegisterStartupScript(Page, GetType(), "CallMyFunction", "openCity(event, 'BasicInfo')", false);
                        //ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc( 'billingModifier');", true);
                        return;
                    }
                    else
                    {
                        Alert("Account " + clvar.AccountNo + " Generated Successfully");
                        Errorid.Text = "Account " + clvar.AccountNo + " Generated Successfully";
                        Errorid.ForeColor = System.Drawing.Color.Green;
                        ResetAll();
                    }

                }


            }
            catch (Exception ex)
            {

                Alert(ex.Message);
                Errorid.Text = ex.Message;
                div2.Style.Add("display", "none");
                //ScriptManager.RegisterStartupScript(Page, GetType(), "CallMyFunction", "openCity(event, 'BasicInfo')", false);
                //ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc( 'billingModifier');", true);
            }

            div2.Style.Add("display", "none");



        }

        protected void btn_save_Click_Approved(object sender, EventArgs e)
        {
            clvar.Memo = HttpContext.Current.Session["StaffLevel"].ToString();
            DataTable staff_tariff_dt = Get_Staff_Tariff(clvar);

            dt_tariff2 = (DataTable)ViewState["dt_tariff2"];

            if (dt_tariff2 != null)
            {
                for (int i = 0; i < dt_tariff2.Rows.Count; i++)
                {
                    if (dt_tariff2.Rows[i]["ServiceTypeName"].ToString().ToUpper() != staff_tariff_dt.Rows[i]["ServiceTypeName"].ToString().ToUpper())
                    {
                        gv_tariff.Rows[i].Cells[1].BackColor = System.Drawing.Color.Red;
                        gv_tariff.Rows[i].Cells[1].ForeColor = System.Drawing.Color.White;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('You have Enter Wrong Service')", true);

                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "openCity(event,'Tariff')", true);
                        return;
                    }

                    if (dt_tariff2.Rows[i]["FromWeight"].ToString() != staff_tariff_dt.Rows[i]["FromWeight"].ToString())
                    {
                        gv_tariff.Rows[i].Cells[2].BackColor = System.Drawing.Color.Red;
                        gv_tariff.Rows[i].Cells[2].ForeColor = System.Drawing.Color.White;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('You have Enter Wrong From Weight')", true);

                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "openCity(event,'Tariff')", true);
                        return;
                    }

                    if (dt_tariff2.Rows[i]["ToWeight"].ToString() != staff_tariff_dt.Rows[i]["ToWeight"].ToString())
                    {
                        gv_tariff.Rows[i].Cells[3].BackColor = System.Drawing.Color.Red;
                        gv_tariff.Rows[i].Cells[3].ForeColor = System.Drawing.Color.White;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('You have Enter Wrong To Weight')", true);

                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "openCity(event,'Tariff')", true);
                        return;
                    }

                    if (dt_tariff2.Rows[i]["AddFactor"].ToString() != staff_tariff_dt.Rows[i]["AdditionalWeight"].ToString())
                    {
                        gv_tariff.Rows[i].Cells[4].BackColor = System.Drawing.Color.Red;
                        gv_tariff.Rows[i].Cells[4].ForeColor = System.Drawing.Color.White;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('You have Enter Wrong Additional Weight')", true);

                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "openCity(event,'Tariff')", true);
                        return;
                    }

                    DataRow app_row = staff_tariff_dt.Select("ServiceTypeName ='" + dt_tariff2.Rows[i]["ServiceTypeName"].ToString().ToUpper() + "' and FromWeight='" + dt_tariff2.Rows[i]["FromWeight"].ToString().ToUpper() + "' and ToWeight='" + dt_tariff2.Rows[i]["ToWeight"].ToString().ToUpper() + "' and AdditionalWeight='" + dt_tariff2.Rows[i]["AddFactor"].ToString().ToUpper() + "'")[0];

                    btn_tariffupdate.Visible = true;

                    if (app_row != null)
                    {
                        string serviceValue = app_row["ServiceTypeName"].ToString().ToUpper();
                        string FromWeight = app_row["FromWeight"].ToString().ToUpper();
                        string ToWeight = app_row["ToWeight"].ToString().ToUpper();
                        string AdditionalWeight = app_row["AdditionalWeight"].ToString().ToUpper();
                        string WithinCity = app_row["WithinCity"].ToString().ToUpper();
                        string SameZone = app_row["SameZone"].ToString().ToUpper();
                        string DiffZone = app_row["DiffZone"].ToString().ToUpper();

                        if (!Equals(serviceValue, dt_tariff2.Rows[i]["ServiceTypeName"].ToString().ToUpper())
                                  || !Equals(FromWeight, dt_tariff2.Rows[i]["FromWeight"].ToString())
                                  || !Equals(ToWeight, dt_tariff2.Rows[i]["ToWeight"].ToString())
                                  || !Equals(AdditionalWeight, dt_tariff2.Rows[i]["AddFactor"].ToString())
                                  || (double.Parse(dt_tariff2.Rows[i]["LocalZone"].ToString()) < double.Parse(WithinCity))
                              )
                        {
                            TextBox textbox = gv_tariff.Rows[i].Cells[6].FindControl("txt_Local") as TextBox;
                            textbox.ForeColor = System.Drawing.Color.White;
                            textbox.BackColor = System.Drawing.Color.Red;
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('You are Not Authorize for Local Rate')", true);
                            return;
                        }
                        if (!Equals(serviceValue, dt_tariff2.Rows[i]["ServiceTypeName"].ToString().ToUpper())
                                || !Equals(FromWeight, dt_tariff2.Rows[i]["FromWeight"].ToString())
                            || !Equals(ToWeight, dt_tariff2.Rows[i]["ToWeight"].ToString())
                                || !Equals(AdditionalWeight, dt_tariff2.Rows[i]["AddFactor"].ToString())
                                || (double.Parse(dt_tariff2.Rows[i]["SameZone"].ToString()) < double.Parse(SameZone))
                            )
                        {
                            TextBox textbox = gv_tariff.Rows[i].Cells[7].FindControl("txt_Same") as TextBox;
                            textbox.BackColor = System.Drawing.Color.Red;
                            textbox.ForeColor = System.Drawing.Color.White;

                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('You are Not Authorize for Same Rate')", true);
                            return;
                        }

                        if (!Equals(serviceValue, dt_tariff2.Rows[i]["ServiceTypeName"].ToString().ToUpper())
                                || !Equals(FromWeight, dt_tariff2.Rows[i]["FromWeight"].ToString())
                            || !Equals(ToWeight, dt_tariff2.Rows[i]["ToWeight"].ToString())
                                || !Equals(AdditionalWeight, dt_tariff2.Rows[i]["AddFactor"].ToString())
                                || (double.Parse(dt_tariff2.Rows[i]["DiffZone"].ToString()) < double.Parse(DiffZone))
                            )
                        {
                            TextBox textbox = gv_tariff.Rows[i].Cells[8].FindControl("txt_Diff") as TextBox;
                            textbox.BackColor = System.Drawing.Color.Red;
                            textbox.ForeColor = System.Drawing.Color.White;
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('You are Not Authorize for Diff Rate')", true);
                            return;
                        }
                    }
                }

                // After Approval Code Here

                Session["dt_"] = dt_tariff2;
                ViewState["dt_tariff2"] = dt_tariff2;
                cl_var.Remarks = "1";

                btn_save_Click_Temp(sender, e);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "openCity(event,'Tariff')", true);
            }
            #region REMOVE
            //if (ViewState["dt_tariff2"] != null)
            //{
            //    dt_tariff2 = (DataTable)ViewState["dt_tariff2"];
            //    if (dt_tariff2.Rows.Count > 0)
            //    {
            //        cl_var.ToWeight = txt_toWeight.Text;
            //        cl_var.FromWeight = txt_fromWeight.Text;

            //        foreach (DataRow row in dt_tariff2.Rows)
            //        {
            //            foreach (DataColumn column in dt_tariff2.Columns)
            //            {
            //                for (int i = 0; i < dt_tariff2.Columns.Count; i++)
            //                {
            //                    String cellText = row[i].ToString();
            //                }
            //            }
            //        }
            /*
            foreach (var row in gv_tariff.Rows)
            {
                for (int i = 0; i < gv_tariff.Columns.Count; i++)
                {
                    String header = gv_tariff.Columns[i].HeaderText;
                    //String cellText = Convert.ToString(row.Cells[i].Value);
                    String cellText = row[i].ToString();
                }

                DataRow[] dr_ = dt_tariff2.Select("FromWeight='" + txt_fromWeight.Text + "' and ToWeight='" + txt_toWeight.Text + "' and ServiceTypeName='" + TariffService.SelectedValue + "'");
            }
            */
            //DataRow[] dr_ = dt_tariff2.Select("FromWeight='" + txt_fromWeight.Text + "' and ToWeight='" + txt_toWeight.Text + "' and ServiceTypeName='" + TariffService.SelectedValue + "'");
            //if (dr_.Length > 0)
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('From Weight Already Present')", true);
            //    txt_fromWeight.Text = "";
            //    txt_fromWeight.Focus();
            //    return;

            //}
            //    }

            //}
            //else
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('First Add Tariff')", true);
            //    return;
            //}
            /*
            if (HttpContext.Current.Session["StaffLevel"].ToString() == "1")
            {
                string staffLevel = HttpContext.Current.Session["StaffLevel"].ToString();

                DataSet ds_tariff = Get_Staff_Tariff(staffLevel);

                for (int i = 0; i < ds_tariff.Tables[0].Rows.Count; i++)
                {
                    double frowweight = double.Parse(ds_tariff.Tables[0].Rows[i]["FromWeight"].ToString());
                    double toweight = double.Parse(ds_tariff.Tables[0].Rows[i]["ToWeight"].ToString());

                    #region FOR 0 to 0.5 Weight

                    if (frowweight == 0 && toweight == 0.5)
                    {
                        if (ds_tariff.Tables[0].Rows[i]["Product"].ToString() == "Domestic")
                        {
                            double withincity = double.Parse(ds_tariff.Tables[0].Rows[i]["withincity"].ToString());
                            double samezone = double.Parse(ds_tariff.Tables[0].Rows[i]["samezone"].ToString());
                            double diffzone = double.Parse(ds_tariff.Tables[0].Rows[i]["diffzone"].ToString());

                            if (double.Parse(t21.Text) >= withincity)
                            {
                                t21.ForeColor = System.Drawing.Color.Black;
                            }
                            else
                            {
                                t21.ForeColor = System.Drawing.Color.Red;
                            }

                            if (double.Parse(t22.Text) >= samezone)
                            {
                                t22.ForeColor = System.Drawing.Color.Black;
                            }
                            else
                            {
                                t22.ForeColor = System.Drawing.Color.Red;
                            }

                            if (double.Parse(t23.Text) >= diffzone)
                            {
                                t23.ForeColor = System.Drawing.Color.Black;
                            }
                            else
                            {
                                t23.ForeColor = System.Drawing.Color.Red;
                            }

                        }
                    }


                    #endregion

                    #region FOR 0.51 to 1 Weight

                    if (frowweight == 0.51 && toweight == 1)
                    {
                        if (ds_tariff.Tables[0].Rows[i]["Product"].ToString() == "Domestic")
                        {
                            double withincity = double.Parse(ds_tariff.Tables[0].Rows[i]["withincity"].ToString());
                            double samezone = double.Parse(ds_tariff.Tables[0].Rows[i]["samezone"].ToString());
                            double diffzone = double.Parse(ds_tariff.Tables[0].Rows[i]["diffzone"].ToString());

                            if (double.Parse(t21.Text) >= withincity)
                            {
                                t21.ForeColor = System.Drawing.Color.Black;
                            }
                            else
                            {
                                t21.ForeColor = System.Drawing.Color.Red;
                            }

                            if (double.Parse(t22.Text) >= samezone)
                            {
                                t22.ForeColor = System.Drawing.Color.Black;
                            }
                            else
                            {
                                t22.ForeColor = System.Drawing.Color.Red;
                            }

                            if (double.Parse(t23.Text) >= diffzone)
                            {
                                t23.ForeColor = System.Drawing.Color.Black;
                            }
                            else
                            {
                                t23.ForeColor = System.Drawing.Color.Red;
                            }

                        }
                    }


                    #endregion
                }


            }
             */

            #endregion
        }

        protected void btn_Search_Click(object sender, EventArgs e)
        {
            //Search_Client_div.Visible = true;

        }

        protected void btn_srch_group_Click(object sender, EventArgs e)
        {
            //Search_Group_DIV.Visible = true;
            //gridview_search_group.DataSource = dt;
            //gridview_search_group.DataBind();
        }


        protected void chk_NotCod_CheckedChanged(object sender, EventArgs e)
        {



        }

        protected void ddl_staff_member_SelectedIndexChanged(object sender, EventArgs e)
        {

            DataTable dt = ViewState["Staffs"] as DataTable;
            foreach (DataRow row in dt.Rows)
            {
                if (row["StaffTypeID"].ToString() == ddly_stafType.SelectedValue)
                {
                    if (ddl_staff_member.SelectedValue != "0")
                    {
                        if (row["StaffMember"].ToString() != "" && ViewState["UPDATE"].ToString() == "1")
                        {
                            string clientID = hd_creditClientID.Value;
                            string staff = row["staffMember"].ToString();
                            DeleteStaffMember(clientID, staff);
                        }
                        row["StaffMember"] = ddl_staff_member.SelectedItem.Text;
                        row["StaffID"] = ddl_staff_member.SelectedValue;
                    }
                    else
                    {
                        if (row["StaffMember"].ToString() != "" && ViewState["UPDATE"].ToString() == "1")
                        {
                            string clientID = hd_creditClientID.Value;
                            string staff = row["staffMember"].ToString();
                            DeleteStaffMember(clientID, staff);
                        }
                        row["StaffMember"] = "";
                        row["StaffID"] = "";
                    }
                    gv_Staffs.DataSource = dt;
                    gv_Staffs.DataBind();
                    break;
                }
            }
            ViewState["Staffs"] = dt;

        }

        protected void ddly_stafType_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetClientStaff();
        }
        protected void ddl_tab4_name_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddl_tab4_name.SelectedValue != "0")
            {
                DataTable dt = ViewState["priceModifiers"] as DataTable;
                DataRow[] dr = dt.Select("ID = '" + ddl_tab4_name.SelectedValue + "'");
                if (dr.Count() > 0)
                {
                    rbtn_PriceModifierBase.SelectedValue = dr[0]["CalculationBase"].ToString();
                    txt_BM_desc.Text = dr[0]["Description"].ToString();
                    txt_pmValue.Text = dr[0]["CalculationValue"].ToString();

                }
            }
        }
        protected void btn_BM_Add_Click(object sender, EventArgs e)
        {
            if (gridview_tab4.Rows.Count > 0)
            {
                Alert("Only one Billing Modifier Allowed");
                return;
            }
            ddl_tab4_name_SelectedIndexChanged(sender, e);
            DataTable allPriceModifiers = ViewState["priceModifiers"] as DataTable;
            DataTable addedPriceModifiers = ViewState["addedPriceModifiers"] as DataTable;

            DataRow[] dr = allPriceModifiers.Select("ID = '" + ddl_tab4_name.SelectedValue + "'");
            if (dr.Count() > 0)
            {
                if (addedPriceModifiers.Select("ID = '" + ddl_tab4_name.SelectedValue + "'").Count() > 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Already Added')", true);
                    return;
                }
                DataRow newRow = addedPriceModifiers.NewRow();
                newRow["ID"] = dr[0]["ID"].ToString();
                newRow["Name"] = dr[0]["Name"].ToString();
                newRow["CalculationValue"] = dr[0]["CalculationValue"].ToString();
                newRow["CalculationBase"] = dr[0]["CalculationBase"].ToString();
                newRow["Description"] = dr[0]["Description"].ToString();
                addedPriceModifiers.Rows.Add(newRow);
                addedPriceModifiers.AcceptChanges();
            }
            ViewState["addedPriceModifiers"] = addedPriceModifiers;
            if (addedPriceModifiers.Rows.Count > 0)
            {
                gridview_tab4.DataSource = addedPriceModifiers;
                gridview_tab4.DataBind();
            }
            ddl_tab4_name.ClearSelection();
            rbtn_PriceModifierBase.ClearSelection();
            txt_BM_desc.Text = "";
            txt_pmValue.Text = "";


            div2.Style.Add("display", "none");
            //ScriptManager.RegisterStartupScript(Page, GetType(), "CallMyFunction", "openCity(event, 'BasicInfo')", false);
            ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc( 'billingModifier');", true);
            //*

        }
        protected void gridview_tab4_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "del")
            {
                string id = e.CommandArgument.ToString();
                DataTable dt = ViewState["addedPriceModifiers"] as DataTable;
                DataRow[] dr = dt.Select("ID = '" + id + "'");
                if (ViewState["UPDATE"].ToString() == "1")
                {
                    string clientID = hd_creditClientID.Value;
                    DeletePriceModifier(clientID, id);
                }
                if (dr.Count() > 0)
                {
                    dt.Rows.Remove(dr[0]);
                    dt.AcceptChanges();

                    gridview_tab4.DataSource = dt;
                    gridview_tab4.DataBind();
                }

                ViewState["addedPriceModifiers"] = dt;
            }
        }



        protected void btn_save_Click(object sender, EventArgs e)
        {
            #region Validations

            if (dd_zone.SelectedValue == "0")
            {
                Alert("Select Zone");
                dd_zone.Focus();
                return;
            }
            if (dd_Branch.SelectedValue == "0")
            {
                Alert("Select Branch");
                dd_Branch.Focus();
                return;
            }

            if (txt_name.Text.Trim() == "")
            {
                Alert("Name is Mendatory");
                txt_name.Focus();
                return;
            }

            if (txt_contactPerson.Text.Trim() == "")
            {
                Alert("Contact Person is Mendatory");
                txt_contactPerson.Focus();
                return;
            }
            if (txt_designation.Text.Trim() == "")
            {
                Alert("Contact Person Designation is mandatory.");
                txt_designation.Focus();
                return;
            }
            //if (dd_CustomerCategory.SelectedValue == "0")
            //{
            //    Alert("Category is Mendatory");
            //    dd_CustomerCategory.Focus();
            //    return;
            //}
            if (txt_PhoneNo.Text.Trim() == "")
            {
                Alert("Phone No is Mendatory");
                txt_PhoneNo.Focus();
                return;
            }
            if (txt_email.Text.Trim() == "")
            {
                txt_email.Focus();
                Alert("Email is Mendatory");
                return;

            }
            if (txt_pickupInstruction.Text.Trim() == "")
            {
                txt_pickupInstruction.Focus(); Alert("Pickup Instructions is Mendatory");
                return;

            }
            if (txt_RegDate.Text.Trim() == "")
            {
                txt_RegDate.Focus();
                Alert("Reg Date is Mendatory");
                return;

            }
            if (txt_RegEndDate.Text.Trim() == "")
            {
                txt_RegEndDate.Focus();
                Alert("Reg End is Mendatory");
                return;
            }
            if (txt_OfficialAddress.Text.Trim() == "")
            {
                txt_OfficialAddress.Focus();
                Alert("Official Address is Mendatory");
                return;

            }
            if (txt_MailingAddress.Text.Trim() == "")
            {
                txt_MailingAddress.Focus();
                Alert("Mailing Address is Mendatory");
                return;

            }
            if (dd_StatusCode.SelectedValue == "0")
            {
                dd_StatusCode.Focus();
                Alert("Status Code is Mendatory");
                return;

            }
            //if (dd_Sector.SelectedValue == "0")
            //{
            //    dd_Sector.Focus();
            //    Alert("Sector is Mendatory");
            //    return;

            //}
            if (dd_Industry.SelectedValue == "0")
            {
                dd_Industry.Focus();
                Alert("Industry is Mendatory");
                return;

            }
            if (dd_prepareBill.SelectedValue == "0" && HttpContext.Current.Session["PROFILE"].ToString() != "16")
            {
                dd_prepareBill.Focus();
                Alert("Prepare Bill selection is Mendatory");
                return;

            }
            if (dd_originEC.SelectedValue == "0")
            {
                dd_originEC.Focus();
                Alert("Origin EC is Mendatory");
                return;

            }
            if (dd_recoveryExpID.SelectedValue == "0")
            {
                dd_recoveryExpID.Focus();
                Alert("Recovery EC is Mendatory");
                return;

            }
            if (txt_SalesTaxNo.Text.Trim() == "")
            {
                txt_SalesTaxNo.Focus();
                Alert("Sales Tax No is Mendatory");
                return;

            }
            if (txt_ntnNo.Text.Trim() == "")
            {
                txt_ntnNo.Focus();
                Alert("NTN No is Mendatory");
                return;

            }

            if (dd_fafType.SelectedValue == "")
            {
                dd_fafType.Focus();
                Alert("Select FAF Type");
                return;
            }

            if (rbtn_codMonthlyBillingAmount.SelectedValue == "1")
            {
                float tempMinAmount = 0;
                float.TryParse(txt_codMonthlyAmount.Text, out tempMinAmount);

                if (tempMinAmount <= 0)
                {
                    Alert("Enter Proper Min Billing Amount");
                    return;
                }
                else
                {
                    isMinBilling = true;
                    minBillingAmount = tempMinAmount;
                }
            }
            else
            {
                minBillingAmount = 0;
                isMinBilling = false;
            }
            if (txt_RnRWeight.Text != "")
            {
                float tempRNRWeight = 0;
                float.TryParse(txt_RnRWeight.Text, out tempRNRWeight);
                if (tempRNRWeight <= 0)
                {
                    Alert("Enter Proper RNR Weight");
                    return;
                }
                else
                {
                    rnrWeight = tempRNRWeight.ToString();
                }
            }
            else
            {
                Alert("Enter RNR Weight");
                return;
            }
            #endregion
            /*
            if (chkl_services.Items.Cast<ListItem>().Where(c => c.Selected).Count() == 0)
            {
                AlertMessage("No Service Selected For Client");
                return;
            }
            try
            {
                DateTime regDate = DateTime.Parse(txt_RegDate.Text);
                DateTime regEndDate = DateTime.Parse(txt_RegEndDate.Text);
                if (regEndDate <= regDate)
                {
                    AlertMessage("Reg Date cannot be less than Reg End Date");
                    return;
                }

                char[] charArray = txt_name.Text.ToCharArray();
                string tempConsigner = "";
                for (int i = 0; i < charArray.Length; i++)
                {
                    if (!char.IsDigit(charArray[i]))
                    {
                        cl_var.Consigner += charArray[i].ToString();
                        tempConsigner += charArray[i].ToString();
                    }
                }

                //cl_var.Consigner = txt_name.Text;

                DataTable accountNo = GetAccountNumber(cl_var);
                cl_var.Consigner = txt_name.Text;
                if (accountNo != null)
                {
                    if (accountNo.Rows.Count > 0)
                    {
                        //clvar.AccountNo = dd_Branch.SelectedValue.ToString() + txt_name.Text.ToCharArray()[0] + accountNo.Rows[0][0].ToString();
                        clvar.AccountNo = dd_Branch.SelectedValue.ToString() + tempConsigner.ToCharArray()[0] + accountNo.Rows[0][0].ToString();
                    }
                    else
                    {
                        Alert("Account Could not be Generated. Contact IT Support.");
                        return;
                    }
                }



                //BASIC INFO STARTS
                BeneficiaryBankAccNo = txt_benAccNo.Text;
                BeneficiaryBankCode = dd_benBank.SelectedValue;
                BeneficiaryName = txt_benName.Text;
                ContactPersonDesignation = txt_designation.Text;
                clvar.Name = txt_name.Text;
                clvar.Name = clvar.Name.Replace("'", "");
                clvar.ContactPerson = txt_contactPerson.Text;
                //clvar.AccountNo = txt_acc_no.Text;
                clvar.IsSpecial = chk_special_cust.Checked.ToString();
                //clvar.Category = dd_CustomerCategory.SelectedValue;
                clvar.FaxNo = txt_FaxNo.Text;
                clvar.PhoneNo = txt_PhoneNo.Text;
                clvar.OfficialAddress = txt_OfficialAddress.Text;
                clvar.MailingAddress = txt_MailingAddress.Text;
                clvar.PickupInstructions = txt_pickupInstruction.Text;
                clvar.RegDate = txt_RegDate.Text;
                clvar.RegEndDate = txt_RegEndDate.Text;
                clvar.CustomerEmail = txt_email.Text;
                clvar.CreditClientType = rbtn_customerType.SelectedValue;
                clvar.IsCentralized = chk_centralisedClient.Checked.ToString();
                clvar.StatusCode = dd_StatusCode.Text;
                clvar.IsCOD = chk_NotCod.Checked.ToString();
                //clvar.IsSmsServiceActive = chk_active_sms_service.Checked.ToString();
                clvar.CODType = dd_codType.SelectedValue;
                //BASIC INFO ENDS

                //CONFIGURATION STARTS
                //clvar.Sector = dd_Sector.SelectedValue;
                clvar.Industry = dd_Industry.SelectedValue;
                //clvar.ClientGroupID = txt_clientGroups.Text; //dd_clientGroups.SelectedValue;
                clvar.Status = rbtn_Status.SelectedValue;
                clvar.PrintingStatus = rbtn_printStatus.SelectedValue;
                //clvar.RedeemWindow = txt_redeemWindow.Text;
                clvar.DomesticAmonTo = txt_DomesticTurnOver.Text;
                clvar.InternationalAmonTo = txt_InternationalTurnOver.Text;
                clvar.DomesticPackets = txt_DomesticPackets.Text;
                clvar.InternationalPackets = txt_InternationalPackets.Text;
                clvar.DomesticAmount = txt_DomesticAmount.Text;
                clvar.InternationalAmount = txt_InternationalAmount.Text;
                //CONFIGURATION ENDS

                //BILLING AND SALES START
                ParentID = dd_parent.SelectedValue;
                ispiece_only = dd_pieces.SelectedValue;
                IsDestination = dd_destination.SelectedValue;
                clvar.PrepareBillType = dd_prepareBill.SelectedValue;
                clvar.BillingMode = rbtn_billingMode.SelectedValue;
                clvar.DiscountOnDocument = txt_DiscountOnDocument.Text;
                clvar.DiscountOnDomestic = txt_DiscountOnDomestic.Text;
                clvar.DiscountOnSample = txt_DiscountOnSample.Text;
                clvar.CreditLimit = txt_CreditLimit.Text;
                clvar.CODMonthlyBillingAmount = txt_codMonthlyAmount.Text;
                //clvar.RnRCharges = txt_RnRCharges.Text;
                clvar.BillTaxType = rbtn_BillTaxType.SelectedValue;
                //clvar.SalesRoute = dd_SalesRoute.SelectedValue;
                clvar.RecoveryExpID = dd_recoveryExpID.SelectedValue;
                clvar.SalesTaxNo = txt_SalesTaxNo.Text;
                //clvar.Memo = txt_memo.Text;
                clvar.NTNNo = txt_ntnNo.Text;
                clvar.OriginEc = dd_originEC.SelectedValue;

                //BILLING AND SALES END

                DataTable addedPriceModifiers = ViewState["addedPriceModifiers"] as DataTable;
                DataTable Staffs = ViewState["Staffs"] as DataTable;

                foreach (GridViewRow row in gv_Staffs.Rows)
                {
                    string staffType = row.Cells[0].Text;
                    string staffTypeID = (row.FindControl("hd_StaffTypeID") as HiddenField).Value;
                    DropDownList ddstaff = row.FindControl("dd_gStaffMembers") as DropDownList;
                    DataRow dr = Staffs.Select("stafftypeid='" + staffTypeID + "'")[0];
                    if (ddstaff.SelectedItem.Text.ToUpper() != "SELECT STAFF")
                    {
                        dr["staffMember"] = ddstaff.SelectedItem.Text;
                        dr["StaffID"] = ddstaff.SelectedValue;

                    }
                    else
                    {
                        if (staffTypeID == "214" || staffTypeID == "215")
                        {
                            Alert("Select " + staffType);
                            ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc( 'billingNsales');", true);
                            return;
                        }
                        else
                        {
                            continue;
                        }

                    }
                }


                DataTable dtService = new DataTable();
                dtService.Columns.AddRange(new DataColumn[] { 
                    new DataColumn("CreditClientID", typeof(Int64)),
                    new DataColumn("ServiceTypeName", typeof(string)),
                    new DataColumn("Status", typeof(int))
                });


                string error = "";
                if (ViewState["UPDATE"].ToString() == "1")
                {
                    foreach (ListItem item in chkl_services.Items)
                    {
                        DataRow sRow = dtService.Rows.Add(Int64.Parse(hd_creditClientID.Value), item.Value, 0);
                        if (item.Selected)
                        {
                            sRow["Status"] = 1;
                        }
                        else
                        {
                            sRow["Status"] = 0;
                        }
                    }
                    DataTable pmTableType = new DataTable();
                    pmTableType.Columns.Add("CreditClientID", typeof(string));
                    pmTableType.Columns.Add("PriceModifierID", typeof(string));
                    pmTableType.Columns.Add("SortOrder", typeof(string));
                    pmTableType.Columns.Add("ModifiedCalculationValue", typeof(string));
                    pmTableType.Columns.Add("CalculationBase", typeof(string));

                    DataTable staffTableType = new DataTable();
                    staffTableType.Columns.Add("id", typeof(Int64));
                    staffTableType.Columns.Add("ClientID", typeof(Int64));
                    staffTableType.Columns.Add("UserName", typeof(string));
                    staffTableType.Columns.Add("StaffTypeID", typeof(int));

                    int i = 1;
                    foreach (GridViewRow row in gridview_tab4.Rows)
                    {
                        DataRow dr = pmTableType.NewRow();
                        dr["CreditClientID"] = hd_creditClientID.Value;
                        dr["priceModifierID"] = (row.FindControl("del") as LinkButton).CommandArgument.ToString();
                        dr["ModifiedCalculationValue"] = (row.FindControl("txt_gValue") as TextBox).Text;
                        dr["CalculationBase"] = row.Cells[3].Text.ToString();
                        dr["SortOrder"] = "0";
                        pmTableType.Rows.Add(dr);
                    }

                    foreach (DataRow dr in Staffs.Rows)
                    {
                        if (dr["StaffMember"].ToString() == "")
                        {
                            continue;
                        }
                        DataRow row = staffTableType.NewRow();
                        row["id"] = 0;
                        row["ClientID"] = hd_creditClientID.Value;
                        row["UserName"] = dr["StaffMember"].ToString();
                        row["StaffTypeID"] = dr["StaffTypeID"].ToString();
                        staffTableType.Rows.Add(row);
                    }

                    if (!chk_nationWide.Checked)
                    {
                        clvar.ClientGroupID = txt_nationWideGroup.Text;
                        error = UpdateCustomer(clvar, pmTableType, staffTableType, hd_creditClientID.Value, dtService);
                    }
                    else
                    {
                        DataTable branchesType = new DataTable();
                        branchesType.Columns.AddRange(new DataColumn[]{
                         new DataColumn("BranchCode", typeof(string)),
                         new DataColumn("Status", typeof(string)),
                         new DataColumn("Parent", typeof(string))
                        });

                        bool parentFound = false;
                        foreach (ListItem item in chkList_Branches.Items)
                        {
                            string parent = "0";
                            DataRow dr = branchesType.NewRow();

                            string status = "0";
                            if (item.Value == dd_parentBranch.SelectedValue && item.Selected)
                            {
                                parent = "1";
                                parentFound = true;
                            }
                            if (item.Selected)
                            {
                                status = "1";
                            }
                            else
                            {
                                status = "0";
                            }

                            branchesType.Rows.Add(item.Value, status, parent);

                        }
                        clvar.AccountNo = txt_acc_no.Text;
                        if (!parentFound)
                        {
                            Alert("Select Parent Branch");
                            return;
                        }
                        clvar.ClientGroupID = txt_nationWideGroup.Text;
                        error = UpdateNationWiseCustomer(clvar, pmTableType, staffTableType, hd_creditClientID.Value, branchesType, dtService);
                    }


                    if (error != "")
                    {
                        Alert("Account Could not be Updated. \r\n Error: " + error);
                        Errorid.Text = "Account Could not be Updated. \r\n Error: " + error;
                        Errorid.ForeColor = System.Drawing.Color.Red;
                        div2.Style.Add("display", "none");
                        //ScriptManager.RegisterStartupScript(Page, GetType(), "CallMyFunction", "openCity(event, 'BasicInfo')", false);
                        //ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc( 'BasicInfo');", true);
                        return;
                    }
                    else
                    {
                        Alert("Account " + txt_acc_no.Text + " Updated Successfully");
                        Errorid.Text = "Account " + txt_acc_no.Text + " Updated Successfully";
                        Errorid.ForeColor = System.Drawing.Color.Green;
                        ResetAll();
                    }
                }
                else if (ViewState["UPDATE"].ToString() == "0")
                {
                    foreach (ListItem item in chkl_services.Items)
                    {
                        if (item.Selected)
                        {
                            DataRow sRow = dtService.Rows.Add(0, item.Value, "1");
                        }


                    }
                    if (chk_nationWide.Checked)
                    {
                        if (txt_nationWideAccountNo.Text.Trim() == "")
                        {
                            Alert("Provide Nationwide Account Number");
                            return;
                        }
                        if (dd_parentBranch.SelectedValue == "0")
                        {
                            Alert("Provide Parent Branch");
                            return;
                        }
                        if (txt_nationWideGroup.Text.Trim() == "")
                        {
                            Alert("Provide Nationwide Group");
                            return;
                        }

                        string parentBranch = dd_parentBranch.SelectedValue;
                        clvar.AccountNo = txt_nationWideAccountNo.Text;
                        clvar.ClientGroupID = txt_nationWideGroup.Text; //dd_nationWideGroup.SelectedValue;
                        DataTable dt = new DataTable();
                        dt.Columns.Add("BranchCode");
                        dt.Columns.Add("ZoneCode");
                        dt.Columns.Add("ExpressCenterCode");

                        DataTable zones = (ViewState["Branches"] as DataSet).Tables[0];
                        foreach (ListItem item in chkList_Branches.Items)
                        {
                            if (item.Selected)
                            {
                                DataRow dr = dt.NewRow();
                                dr["BranchCode"] = item.Value;
                                dr["ZoneCode"] = zones.Select("BranchCode = '" + item.Value + "'")[0]["ZoneCode"].ToString();
                                dr["ExpressCenterCode"] = zones.Select("BranchCode = '" + item.Value + "'")[0]["expressCenterCode"].ToString();
                                dt.Rows.Add(dr);
                            }
                        }
                        if (dt.Rows.Count == 0)
                        {
                            Alert("Select Nation wide Branches");
                            return;
                        }
                        DataTable tempModifiers = addedPriceModifiers.Clone();
                        tempModifiers.Clear();
                        foreach (DataRow item in dt.Rows)
                        {
                            tempModifiers.Merge(addedPriceModifiers);

                        }
                        error = AddNationWideCustomer(clvar, addedPriceModifiers, Staffs, dt, parentBranch, dtService);

                    }
                    else if (chk_centralisedClient.Checked)
                    {
                        string parentBranch = dd_parentBranch.SelectedValue;
                        clvar.AccountNo = txt_nationWideAccountNo.Text;
                        clvar.ClientGroupID = txt_nationWideGroup.Text; //dd_nationWideGroup.SelectedValue;
                        DataTable dt = new DataTable();
                        dt.Columns.Add("BranchCode");
                        dt.Columns.Add("ZoneCode");
                        dt.Columns.Add("ExpressCenterCode");

                        DataTable zones = (ViewState["Branches"] as DataSet).Tables[0];
                        //foreach (ListItem item in chkList_Branches.Items)
                        //{
                        //if (item.Selected)
                        //{
                        DataRow dr = dt.NewRow();
                        dr["BranchCode"] = dd_Branch.SelectedValue;
                        dr["ZoneCode"] = zones.Select("BranchCode = '" + dd_Branch.SelectedValue + "'")[0]["ZoneCode"].ToString();
                        dr["ExpressCenterCode"] = zones.Select("BranchCode = '" + dd_Branch.SelectedValue + "'")[0]["expressCenterCode"].ToString();
                        dt.Rows.Add(dr);
                        //}
                        //}
                        if (dt.Rows.Count == 0)
                        {
                            Alert("Select Nation wide Branches");
                            return;
                        }
                        DataTable tempModifiers = addedPriceModifiers.Clone();
                        tempModifiers.Clear();
                        foreach (DataRow item in dt.Rows)
                        {
                            tempModifiers.Merge(addedPriceModifiers);

                        }
                        error = AddCentralizedCustomer(clvar, addedPriceModifiers, Staffs, dt, parentBranch);
                    }
                    else
                    {
                        error = AddCustomer(clvar, addedPriceModifiers, Staffs, dtService);
                    }
                    if (error != "")
                    {

                        Alert("Account Could not be Created. \r\n Error: " + error);
                        Errorid.Text = "Account Could not be Created. " + error;
                        Errorid.ForeColor = System.Drawing.Color.Red;
                        div2.Style.Add("display", "none");
                        //ScriptManager.RegisterStartupScript(Page, GetType(), "CallMyFunction", "openCity(event, 'BasicInfo')", false);
                        //ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc( 'billingModifier');", true);
                        return;
                    }
                    else
                    {
                        Alert("Account " + clvar.AccountNo + " Generated Successfully");
                        Errorid.Text = "Account " + clvar.AccountNo + " Generated Successfully";
                        Errorid.ForeColor = System.Drawing.Color.Green;
                        ResetAll();
                    }
                }


            }
            catch (Exception ex)
            {

                Alert(ex.Message);
                Errorid.Text = ex.Message;
                div2.Style.Add("display", "none");
                //ScriptManager.RegisterStartupScript(Page, GetType(), "CallMyFunction", "openCity(event, 'BasicInfo')", false);
                //ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc( 'billingModifier');", true);
            }

            div2.Style.Add("display", "none");

            */

        }

        public string Temp_AddCustomer(CL_Customer clvar, DataTable modifiers, DataTable staff, DataTable dtService)
        {
            string query = "";
            List<String> queries = new List<string>();
            string sqlString = "insert into Mnp_TempCreditClients \n" +
            "  (name,\n" +
            "   contactPerson,\n" +
            "   phoneNo,\n" +
            "   faxNo,\n" +
            "   email,\n" +////////////////
            "   address,\n" +
            "   centralizedClient,\n" +
            "   regDate,\n" +
            "   regEndDate,\n" +///////////
            "   pickUpInstruction,\n" +
            "   domesticAMonTo,\n" +
            "   internationalAMonTo,\n" +//////////////
            "   domesticPackets,\n" +
            "   internationalPackets,\n" +
            "   domesticAmount,\n" +
            "   internationalAmount,\n" +
            "   status,\n" +
            "   printingStatus,\n" +
            "   billingMode,\n" +
            "   discountOnDomestic,\n" +
            "   discountOnSample,\n" +
            "   discountOnDocument,\n" +
            "   prepareBillType,\n" +
            "   creditLimit,\n" +
            "   salesTaxNo,\n" +
            "   memo,\n" +
            "   billTaxType,\n" +
            "   catId,\n" +
            "   clientGrpId,\n" +
            "   recoveryExpCenId,\n" +
            "   salesRouteId,\n" +
            "   recoveryOfficer,\n" +
            "   salesExecutive,\n" +
            "   redeemWindow,\n" +
            "   overdueCalBase,\n" +
            "   overdueValue,\n" +
            "   createdBy,\n" +
            "   createdOn,\n" +
            "   SectorId,\n" +
            "   IndustryId,\n" +
            "   tempclientid,\n" +
            "   creditClientType,\n" +////////////////////////////////////////////////
            "   zoneCode,\n" +
            "   branchCode,\n" +
            "   expressCenterCode,\n" +/////////////////////
            "   ntnNo,\n" +
            "   IsCOD,\n" +
            "   isActive,\n" +///////////////////
            "   IsSpecial,\n" +
            "   isFranchisee,\n" +
            "   recoveryOfficerName,\n" +
            "   recoveryOfficer_id,\n" +
            "   isNationWide,\n" +
            "   isParent,\n" +
            "   MailingAddress,\n" +
            "   OriginEC,\n" +
            "   StatusCode,\n" +
            "   CODType,\n" +
            "   isMinBilling,\n" +
            "   MonthlyFixCharges,\n" +
            "   IsSmsServiceActive,\n" +
            //"   RNRCNCharges,\n" +
            "   RNR_Weight,\n" +
            "   HSCNCharges, BeneficiaryName, BeneficiaryBankAccNo, BeneficiaryBankCode, ispiece_only, IsDestination, ContactPersonDesignation, ParentID, Faftype\n" +
            "  )\n output inserted.id \n" +
            "Values\n" +
            "  (\n" +
            "'" + clvar.Name + "',\n" +
            "'" + clvar.ContactPerson + "',\n" +
            "'" + clvar.PhoneNo + "',\n" +
            "'" + clvar.FaxNo + "',\n" +
            "'" + clvar.CustomerEmail + "',\n" +/////////////////
            "'" + clvar.OfficialAddress + "',\n" +
            "'" + clvar.IsCentralized + "',\n" +
            "'" + DateTime.Parse(clvar.RegDate).ToString("yyyy-MM-dd") + "',\n" +
            "'" + DateTime.Parse(clvar.RegEndDate).ToString("yyyy-MM-dd") + "',\n" +/////////////////////////
            "'" + clvar.PickupInstructions + "',\n" +
            "'" + clvar.DomesticAmonTo + "',\n" +
            "'" + clvar.InternationalAmonTo + "',\n" +
            "'" + clvar.DomesticPackets + "',\n" +
            "'" + clvar.InternationalPackets + "',\n" +
            "'" + clvar.DomesticAmount + "',\n" +
            "'" + clvar.InternationalAmount + "',\n" +
            "'" + clvar.Status + "',\n" +
            "'" + clvar.PrintingStatus + "',\n" +
            "'" + clvar.BillingMode + "',\n" +
            "'" + clvar.DiscountOnDomestic + "',\n" +
            "'" + clvar.DiscountOnSample + "',\n" +
            "'" + clvar.DiscountOnDocument + "',\n" +
            "'" + clvar.PrepareBillType + "',\n" +
            "'" + clvar.CreditLimit + "',\n" +
            "'" + clvar.SalesTaxNo + "',\n" +
            "'" + clvar.Memo + "',\n" +
            "'" + clvar.BillTaxType + "',\n" +
            "'" + clvar.Category + "',\n" +
            "'" + clvar.ClientGroupID + "',\n" +
            "'" + clvar.RecoveryExpID + "',\n" +
            "'" + clvar.SalesRoute + "',\n" +
            "'" + clvar.RecoveryOfficer + "',\n" +
            "'" + clvar.SalesExecutive + "',\n" +
            "'" + clvar.RedeemWindow + "',\n" +
            "'" + clvar.OverdueCalculationBase + "',\n" +
            "'" + clvar.OverdueValue + "',\n" +
            "'" + HttpContext.Current.Session["U_ID"].ToString() + "',\n" +
            " GETDATE(),\n" +
            "'1',--sectorID\n" +
            "'" + clvar.Industry + "',\n" +
            "'" + clvar.AccountNo + "',\n" +
            "'" + clvar.CreditClientType + "',\n" +
            "'" + dd_zone.SelectedValue + "',\n" +
            "'" + dd_Branch.SelectedValue + "',\n" +
            "'" + HttpContext.Current.Session["ExpressCenter"].ToString() + "',\n" +
            "'" + clvar.NTNNo + "',\n" +
            "'" + clvar.IsCOD + "',\n" +
            "'1',\n" +
            "'" + clvar.IsSpecial + "',\n" +
            "'" + clvar.IsFranchise + "',\n" +
            "'" + clvar.RecoveryOfficerName + "',\n" +
             "'" + clvar.RecoveryOfficerID + "',\n" +
            "'" + clvar.IsNationWide + "',\n" +
            "'" + clvar.IsParent + "',\n" +
            "'" + clvar.MailingAddress + "',\n" +
            "'" + clvar.OriginEc + "',\n" +
            "'" + clvar.StatusCode + "',\n" +
            "'" + clvar.CODType + "',\n" +
            "'" + isMinBilling + "',\n" +
            "'" + minBillingAmount.ToString() + "',\n" +
            "'" + clvar.IsSmsServiceActive + "',\n" +
            "'" + rnrWeight + "',\n" +
            "'" + clvar.HSCNCharges + "', \n" +
            "'" + BeneficiaryName + "', \n" +
            "'" + BeneficiaryBankAccNo + "', \n" +
            "'" + BeneficiaryBankCode + "',\n" +
            "'" + ispiece_only + "', \n" +
            "'" + IsDestination + "', \n" +
            "'" + ContactPersonDesignation + "', \n" +
            "'" + ParentID + "', '" + dd_fafType.SelectedValue + "'\n" +
            "   )";

            string staffQuery = "";
            string error = "";

            SqlConnection con = new SqlConnection(clvar.Strcon());
            con.Open();
            try
            {

                SqlCommand cmd = new SqlCommand(sqlString, con);
                //cmd.ExecuteNonQuery();

                object clientID = cmd.ExecuteScalar();
                Int64 id = Int64.Parse(clientID.ToString());
                foreach (DataRow row in staff.Rows)
                {
                    if (row["STAFFID"].ToString().Trim() != "")
                    {
                        staffQuery = "insert into Mnp_TempClientStaff (TempClientId, UserName, StaffTypeId, createdby, createdon)\n" +
                        "            values\n" +
                        "            (\n" +
                        "               '" + id.ToString() + "',\n" +
                        "               '" + row["staffMember"].ToString() + "',\n" +
                        "               '" + row["StaffTypeID"].ToString() + "', \n" +
                        "               '" + HttpContext.Current.Session["U_ID"].ToString() + "',\n" +
                        "               GETDATE() \n" +
                        "            )";
                        cmd.CommandText = staffQuery;
                        cmd.ExecuteNonQuery();
                        queries.Add(staffQuery);
                    }

                }
                int sortOrder = 0;
                foreach (DataRow row in modifiers.Rows)
                {
                    sortOrder = 0;
                    string priceModifierQuery = "insert into Mnp_TempClientPriceModifierAssociation \n" +
                                                "(\n" +
                                                "		TempClientId,\n" +
                                                "		priceModifierId,\n" +
                                                "		sortOrder, \n" +
                                                "		modifiedCalculationValue, \n" +
                                                "		calculationBase, \n" +
                                                "		createdBy, \n" +
                                                "		createdOn\n" +
                                                "	)\n" +
                                                "	Values\n" +
                                                "	(\n" +
                                                "       '" + id.ToString() + "',\n" +
                                                "       '" + row["id"].ToString() + "',\n" +
                                                "       '" + sortOrder.ToString() + "',\n" +
                                                "       '" + row["CalculationValue"].ToString() + "',\n" +
                                                "       '" + row["CalculationBase"].ToString() + "',\n" +
                                                "       '" + HttpContext.Current.Session["U_ID"].ToString() + "',\n" +
                                                "GETDATE()\n" +
                                                ")";
                    cmd.CommandText = priceModifierQuery;
                    cmd.ExecuteNonQuery();
                }

                foreach (DataRow dr in dtService.Rows)
                {
                    if (dr["Status"].ToString() == "1")
                    {
                        string serviceInsertCommand = "INSERT INTO MnP_TempCustomerServiceMap\n" +
                        "  (\n" +
                        "   -- ID -- THIS COLUMN VALUE IS AUTO-GENERATED\n" +
                        "   TempCLIENTID,\n" +
                        "   SERVICETYPENAME,\n" +
                        "   STATUS,\n" +
                        "   CREATEDON,\n" +
                        "   CREATEDBY,\n" +
                        "   MODIFIEDON,\n" +
                        "   MODIFIEDBY)\n" +
                        "VALUES\n" +
                        "  ('" + id.ToString() + "', '" + dr["ServiceTypeName"].ToString() + "', '1', GETDATE(), '" + HttpContext.Current.Session["U_ID"].ToString() + "', NULL, NULL)";
                        cmd.CommandText = serviceInsertCommand;
                        cmd.ExecuteNonQuery();
                    }
                }

                foreach (DataRow dr_tariff in dt_tariff2.Rows)
                {
                    string sql_tariff = "INSERT INTO MnP_TempTariff \n"
                                       + "( \n"
                                       + "	TempClientId,ServiceTypeName,FromWeight,ToWeight,AdditionalWeight,LocalZone,SameZone,DiffZone,isPR, \n"
                                       + "	[Status],CreatedBy,CreatedOn \n"
                                       + ") \n"
                                       + "VALUES \n"
                                       + "( \n"
                                       + "  '" + id.ToString() + "', \n"
                                       + "	'" + dr_tariff["ServiceTypeName"].ToString() + "', \n"
                                       + "	'" + dr_tariff["FromWeight"].ToString() + "', \n"
                                       + "	'" + dr_tariff["ToWeight"].ToString() + "', \n"
                                       + "	'" + dr_tariff["AddFactor"].ToString() + "', \n"
                                       + "	'" + dr_tariff["LocalZone"].ToString() + "', \n"
                                       + "	'" + dr_tariff["SameZone"].ToString() + "', \n"
                                       + "	'" + dr_tariff["DiffZone"].ToString() + "', \n";
                    if (dr_tariff["cb_pr"].ToString().ToUpper() == "TRUE")
                    {
                        sql_tariff += "	'1', \n";
                    }
                    else
                    {
                        sql_tariff += "	'0', \n";
                    }
                    sql_tariff += "	'1', \n"
                                + "	'" + HttpContext.Current.Session["U_ID"].ToString() + "', \n"
                                + "	GETDATE() \n"
                                + ")";

                    cmd.CommandText = sql_tariff;
                    cmd.ExecuteNonQuery();
                }
                //error = id.ToString();

            }
            catch (Exception ex)
            { error = ex.Message; }
            finally { con.Close(); }

            return error;
        }

        public string DeleteCustomerTariff(CL_Customer clvar)
        {
            string error = "";
            int count = 0;
            string sql_tariff = "UPDATE MnP_TempTariff SET \n" +
                                  "Status = '0', \n" +
                                  "ModifiedBy = '" + HttpContext.Current.Session["U_ID"].ToString() + "', \n" +
                                  "ModifiedOn = GETDATE() \n" +
                                  "WHERE TempClientId ='" + clvar.RnRCharges + "' \n";

            SqlConnection con = new SqlConnection(clvar.Strcon());
            con.Open();
            try
            {

                SqlCommand cmd = new SqlCommand(sql_tariff, con);
                cmd.ExecuteNonQuery();

                foreach (DataRow dr_tariff in dt_tariff2.Rows)
                {
                    string sql_addtariff = "INSERT INTO MnP_TempTariff \n"
                                       + "( \n"
                                       + "	TempClientId,ServiceTypeName,FromWeight,ToWeight,AdditionalWeight,LocalZone,SameZone,DiffZone, \n"
                                       + "	[Status],CreatedBy,CreatedOn \n"
                                       + ") \n"
                                       + "VALUES \n"
                                       + "( \n"
                                       + "  '" + clvar.RnRCharges + "', \n"
                                       + "	'" + dr_tariff["ServiceTypeName"].ToString() + "', \n"
                                       + "	'" + dr_tariff["FromWeight"].ToString() + "', \n"
                                       + "	'" + dr_tariff["ToWeight"].ToString() + "', \n"
                                       + "	'" + dr_tariff["AddFactor"].ToString() + "', \n"
                                       + "	'" + dr_tariff["LocalZone"].ToString() + "', \n"
                                       + "	'" + dr_tariff["SameZone"].ToString() + "', \n"
                                       + "	'" + dr_tariff["DiffZone"].ToString() + "', \n"
                                       + "	'1', \n"
                                       + "	'" + HttpContext.Current.Session["U_ID"].ToString() + "', \n"
                                       + "	GETDATE() \n"
                                       + ")";

                    cmd.CommandText = sql_addtariff;
                    cmd.ExecuteNonQuery();
                }

            }
            /*
            SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            try
            {
                sqlcon.Open();
                SqlCommand sqlcmd = new SqlCommand(sql_tariff, sqlcon);
                sqlcmd.CommandType = CommandType.Text;

                count = sqlcmd.ExecuteNonQuery();
                sqlcon.Close();
            }
             */
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return error;
        }

        /*
        public string UpdateCustomerTariff(Cl_Variables clvar)
        {
            foreach (DataRow dr_tariff in dt_tariff2.Rows)
            {
                "INSERT INTO MnP_TempTariff \n"
                        + "( \n"
                        + "	TempClientId,ServiceTypeName,FromWeight,ToWeight,AdditionalWeight,LocalZone,SameZone,DiffZone, \n"
                        + "	[Status],CreatedBy,CreatedOn \n"
                        + ") \n"
                        + "VALUES \n"
                        + "( \n"
                        + "  '" + id.ToString() + "', \n"
                        + "	'" + dr_tariff["ServiceTypeName"].ToString() + "', \n"
                        + "	'" + dr_tariff["FromWeight"].ToString() + "', \n"
                        + "	'" + dr_tariff["ToWeight"].ToString() + "', \n"
                        + "	'" + dr_tariff["AddFactor"].ToString() + "', \n"
                        + "	'" + dr_tariff["LocalZone"].ToString() + "', \n"
                        + "	'" + dr_tariff["SameZone"].ToString() + "', \n"
                        + "	'" + dr_tariff["DiffZone"].ToString() + "', \n"
                        + "	'1', \n"
                        + "	'" + HttpContext.Current.Session["U_ID"].ToString() + "', \n"
                        + "	GETDATE() \n" +
                        + ")";
            }

            SqlConnection con = new SqlConnection(clvar.Strcon());
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand(sql_tariff, con);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            { error = ex.Message; }
            finally { con.Close(); }

            return error;
        }
        */
        public string AddCustomer(CL_Customer clvar, DataTable modifiers, DataTable staff, DataTable dtService)
        {
            string query = "";
            List<String> queries = new List<string>();
            string sqlString = "insert into CreditClients\n" +
            "  (name,\n" +
            "   contactPerson,\n" +
            "   phoneNo,\n" +
            "   faxNo,\n" +
            "   email,\n" +////////////////
            "   address,\n" +
            "   centralizedClient,\n" +
            "   regDate,\n" +
            "   regEndDate,\n" +///////////
            "   pickUpInstruction,\n" +
            "   domesticAMonTo,\n" +
            "   internationalAMonTo,\n" +//////////////
            "   domesticPackets,\n" +
            "   internationalPackets,\n" +
            "   domesticAmount,\n" +
            "   internationalAmount,\n" +
            "   status,\n" +
            "   printingStatus,\n" +
            "   billingMode,\n" +
            "   discountOnDomestic,\n" +
            "   discountOnSample,\n" +
            "   discountOnDocument,\n" +
            "   prepareBillType,\n" +
            "   creditLimit,\n" +
            "   salesTaxNo,\n" +
            "   memo,\n" +
            "   billTaxType,\n" +
            "   catId,\n" +
            "   clientGrpId,\n" +
            "   recoveryExpCenId,\n" +
            "   salesRouteId,\n" +
            "   recoveryOfficer,\n" +
            "   salesExecutive,\n" +
            "   redeemWindow,\n" +
            "   overdueCalBase,\n" +
            "   overdueValue,\n" +
            "   createdBy,\n" +
            "   createdOn,\n" +
            "   SectorId,\n" +
            "   IndustryId,\n" +
            "   accountNo,\n" +
            "   creditClientType,\n" +////////////////////////////////////////////////
            "   zoneCode,\n" +
            "   branchCode,\n" +
            "   expressCenterCode,\n" +/////////////////////
            "   ntnNo,\n" +
            "   IsCOD,\n" +
            "   isActive,\n" +///////////////////
            "   IsSpecial,\n" +
            "   isFranchisee,\n" +
            "   recoveryOfficerName,\n" +
            "   recoveryOfficer_id,\n" +
            "   isNationWide,\n" +
            "   isParent,\n" +
            "   MailingAddress,\n" +
            "   OriginEC,\n" +
            "   StatusCode,\n" +
            "   CODType,\n" +
            "   isMinBilling,\n" +
            "   MonthlyFixCharges,\n" +
            "   IsSmsServiceActive,\n" +
            //"   RNRCNCharges,\n" +
            "   RNR_Weight,\n" +
            "   HSCNCharges, BeneficiaryName, BeneficiaryBankAccNo, BeneficiaryBankCode, ispiece_only, IsDestination, ContactPersonDesignation, ParentID, Faftype\n" +
            "  )\n output inserted.id \n" +
            "Values\n" +
            "  (\n" +
            "'" + clvar.Name + "',\n" +
            "'" + clvar.ContactPerson + "',\n" +
            "'" + clvar.PhoneNo + "',\n" +
            "'" + clvar.FaxNo + "',\n" +
            "'" + clvar.CustomerEmail + "',\n" +/////////////////
            "'" + clvar.OfficialAddress + "',\n" +
            "'" + clvar.IsCentralized + "',\n" +
            "'" + DateTime.Parse(clvar.RegDate).ToString("yyyy-MM-dd") + "',\n" +
            "'" + DateTime.Parse(clvar.RegEndDate).ToString("yyyy-MM-dd") + "',\n" +/////////////////////////
            "'" + clvar.PickupInstructions + "',\n" +
            "'" + clvar.DomesticAmonTo + "',\n" +
            "'" + clvar.InternationalAmonTo + "',\n" +
            "'" + clvar.DomesticPackets + "',\n" +
            "'" + clvar.InternationalPackets + "',\n" +
            "'" + clvar.DomesticAmount + "',\n" +
            "'" + clvar.InternationalAmount + "',\n" +
            "'" + clvar.Status + "',\n" +
            "'" + clvar.PrintingStatus + "',\n" +
            "'" + clvar.BillingMode + "',\n" +
            "'" + clvar.DiscountOnDomestic + "',\n" +
            "'" + clvar.DiscountOnSample + "',\n" +
            "'" + clvar.DiscountOnDocument + "',\n" +
            "'" + clvar.PrepareBillType + "',\n" +
            "'" + clvar.CreditLimit + "',\n" +
            "'" + clvar.SalesTaxNo + "',\n" +
            "'" + clvar.Memo + "',\n" +
            "'" + clvar.BillTaxType + "',\n" +
            "'" + clvar.Category + "',\n" +
            "'" + clvar.ClientGroupID + "',\n" +
            "'" + clvar.RecoveryExpID + "',\n" +
            "'" + clvar.SalesRoute + "',\n" +
            "'" + clvar.RecoveryOfficer + "',\n" +
            "'" + clvar.SalesExecutive + "',\n" +
            "'" + clvar.RedeemWindow + "',\n" +
            "'" + clvar.OverdueCalculationBase + "',\n" +
            "'" + clvar.OverdueValue + "',\n" +
            "'" + HttpContext.Current.Session["U_ID"].ToString() + "',\n" +
            " GETDATE(),\n" +
            "'1',--sectorID\n" +
            "'" + clvar.Industry + "',\n" +
            "'" + clvar.AccountNo + "',\n" +
            "'" + clvar.CreditClientType + "',\n" +
            "'" + dd_zone.SelectedValue + "',\n" +
            "'" + dd_Branch.SelectedValue + "',\n" +
            "'" + HttpContext.Current.Session["ExpressCenter"].ToString() + "',\n" +
            "'" + clvar.NTNNo + "',\n" +
            "'" + clvar.IsCOD + "',\n" +
            "'1',\n" +
            "'" + clvar.IsSpecial + "',\n" +
            "'" + clvar.IsFranchise + "',\n" +
            "'" + clvar.RecoveryOfficerName + "',\n" +
             "'" + clvar.RecoveryOfficerID + "',\n" +
            "'" + clvar.IsNationWide + "',\n" +
            "'" + clvar.IsParent + "',\n" +
            "'" + clvar.MailingAddress + "',\n" +
            "'" + clvar.OriginEc + "',\n" +
            "'" + clvar.StatusCode + "',\n" +
            "'" + clvar.CODType + "',\n" +
            "'" + isMinBilling + "',\n" +
            "'" + minBillingAmount.ToString() + "',\n" +
            "'" + clvar.IsSmsServiceActive + "',\n" +
            "'" + rnrWeight + "',\n" +
            "'" + clvar.HSCNCharges + "', \n" +
            "'" + BeneficiaryName + "', \n" +
            "'" + BeneficiaryBankAccNo + "', \n" +
            "'" + BeneficiaryBankCode + "',\n" +
            "'" + ispiece_only + "', \n" +
            "'" + IsDestination + "', \n" +
            "'" + ContactPersonDesignation + "', \n" +
            "'" + ParentID + "', '" + dd_fafType.SelectedValue + "'\n" +
            "   )";

            string staffQuery = "";
            string error = "";

            SqlConnection con = new SqlConnection(clvar.Strcon());
            con.Open();
            try
            {

                SqlCommand cmd = new SqlCommand(sqlString, con);
                //cmd.ExecuteNonQuery();

                object clientID = cmd.ExecuteScalar();
                Int64 id = Int64.Parse(clientID.ToString());
                foreach (DataRow row in staff.Rows)
                {
                    if (row["STAFFID"].ToString().Trim() != "")
                    {
                        staffQuery = "insert into ClientStaff (ClientId, UserName, StaffTypeId)\n" +
                        "            values\n" +
                        "            (\n" +
                        "               '" + id.ToString() + "',\n" +
                        "               '" + row["staffMember"].ToString() + "',\n" +
                        "               '" + row["StaffTypeID"].ToString() + "'\n" +
                        "            )";
                        cmd.CommandText = staffQuery;
                        cmd.ExecuteNonQuery();
                        queries.Add(staffQuery);
                    }

                }
                int sortOrder = 0;
                foreach (DataRow row in modifiers.Rows)
                {
                    sortOrder = 0;
                    string priceModifierQuery = "insert into ClientPriceModifierAssociation \n" +
                                                "(\n" +
                                                "		creditClientId,\n" +
                                                "		priceModifierId,\n" +
                                                "		sortOrder, \n" +
                                                "		modifiedCalculationValue, \n" +
                                                "		calculationBase, \n" +
                                                "		createdBy, \n" +
                                                "		createdOn\n" +
                                                "	)\n" +
                                                "	Values\n" +
                                                "	(\n" +
                                                "       '" + id.ToString() + "',\n" +
                                                "       '" + row["id"].ToString() + "',\n" +
                                                "       '" + sortOrder.ToString() + "',\n" +
                                                "       '" + row["CalculationValue"].ToString() + "',\n" +
                                                "       '" + row["CalculationBase"].ToString() + "',\n" +
                                                "       '" + HttpContext.Current.Session["U_ID"].ToString() + "',\n" +
                                                "GETDATE()\n" +
                                                ")";
                    cmd.CommandText = priceModifierQuery;
                    cmd.ExecuteNonQuery();
                }

                foreach (DataRow dr in dtService.Rows)
                {
                    if (dr["Status"].ToString() == "1")
                    {
                        string serviceInsertCommand = "INSERT INTO MNP_CUSTOMER_SERVICEMAP\n" +
                        "  (\n" +
                        "   -- ID -- THIS COLUMN VALUE IS AUTO-GENERATED\n" +
                        "   CREDITCLIENTID,\n" +
                        "   SERVICETYPENAME,\n" +
                        "   STATUS,\n" +
                        "   CREATEDON,\n" +
                        "   CREATEDBY,\n" +
                        "   MODIFIEDON,\n" +
                        "   MODIFIEDBY)\n" +
                        "VALUES\n" +
                        "  ('" + id.ToString() + "', '" + dr["ServiceTypeName"].ToString() + "', '1', GETDATE(), '" + HttpContext.Current.Session["U_ID"].ToString() + "', NULL, NULL)";
                        cmd.CommandText = serviceInsertCommand;
                        cmd.ExecuteNonQuery();
                    }
                }
                //error = id.ToString();

            }
            catch (Exception ex)
            { error = ex.Message; }
            finally { con.Close(); }

            return error;
        }
        public string AddNationWideCustomer(CL_Customer clvar, DataTable modifiers, DataTable staff, DataTable branches, string parentBranch, DataTable dtService)
        {
            string errorMessage = "";
            SqlConnection con = new SqlConnection(clvar.Strcon());
            SqlConnection con_ = new SqlConnection(clvar.Strcon());

            SqlTransaction trans;
            con.Open();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            trans = con.BeginTransaction(IsolationLevel.ReadUncommitted);
            cmd.Transaction = trans;
            string query = "";
            List<String> queries = new List<string>();

            try
            {
                foreach (DataRow dr in branches.Rows)
                {



                    string sqlString = "insert into CreditClients\n" +
                    "  (name,\n" +//
                    "   contactPerson,\n" +//
                    "   phoneNo,\n" +//
                    "   faxNo,\n" +//
                    "   email,\n" +//
                    "   address,\n" +//
                    "   centralizedClient,\n" +//
                    "   regDate,\n" +//
                    "   regEndDate,\n" +//
                    "   pickUpInstruction,\n" +//
                    "   domesticAMonTo,\n" +
                    "   internationalAMonTo,\n" +
                    "   domesticPackets,\n" +
                    "   internationalPackets,\n" +
                    "   domesticAmount,\n" +
                    "   internationalAmount,\n" +
                    "   status,\n" +
                    "   printingStatus,\n" +
                    "   billingMode,\n" +
                    "   discountOnDomestic,\n" +
                    "   discountOnSample,\n" +
                    "   discountOnDocument,\n" +
                    "   prepareBillType,\n" +
                    "   creditLimit,\n" +
                    "   salesTaxNo,\n" +
                    "   memo,\n" +
                    "   billTaxType,\n" +
                    "   catId,\n" +
                    "   clientGrpId,\n" +
                    "   recoveryExpCenId,\n" +
                    "   salesRouteId,\n" +
                    "   recoveryOfficer,\n" +
                    "   salesExecutive,\n" +
                    "   redeemWindow,\n" +
                    "   overdueCalBase,\n" +
                    "   overdueValue,\n" +
                    "   createdBy,\n" +
                    "   createdOn,\n" +
                    "   SectorId,\n" +
                    "   IndustryId,\n" +
                    "   accountNo,\n" +
                    "   creditClientType,\n" +
                    "   zoneCode,\n" +
                    "   branchCode,\n" +
                    "   expressCenterCode,\n" +
                    "   ntnNo,\n" +
                    "   IsCOD,\n" +
                    "   isActive,\n" +
                    "   IsSpecial,\n" +
                    "   isFranchisee,\n" +
                    "   recoveryOfficerName,\n" +
                    "   recoveryOfficer_id,\n" +
                    "   isNationWide,\n" +
                    "   isParent,\n" +
                    "   MailingAddress,\n" +
                    "   OriginEC,\n" +
                    "   StatusCode,\n" +
                    "   CODType,\n" +
                    "   isMinBilling,\n" +
                    "   MonthlyFixCharges,\n" +
                    "   IsSmsServiceActive,\n" +
                    "   RNR_WEight,\n" +
                    "   HSCNCharges,\n" +
                    "   BeneficiaryName, \n" +
                    "   BeneficiaryBankAccNo, \n" +
                    "   BeneficiaryBankCode, \n" +
                    "   ispiece_only, \n" +
                    "   IsDestination, \n" +
                    "   ContactPersonDesignation, \n" +
                    "   parentID, faftype\n" +
                    "  )\n output inserted.id \n" +
                    "Values\n" +
                    "  (\n" +
                    "'" + clvar.Name + "' ,\n" +
                    "'" + clvar.ContactPerson + "',\n" +
                    "'" + clvar.PhoneNo + "',\n" +
                    "'" + clvar.FaxNo + "',\n" +
                    "'" + clvar.CustomerEmail + "',\n";/////////////////
                    if (dd_Branch.SelectedValue == dr["BranchCODE"].ToString())
                    {
                        sqlString += "'" + clvar.OfficialAddress + "',\n";
                    }
                    else
                    {
                        sqlString += "'',\n";
                    }

                    sqlString += "'1',\n" + //nationWide ki soorat main centralized laazmi hai
                    "'" + DateTime.Parse(clvar.RegDate).ToString("yyyy-MM-dd") + "',\n" +
                    "'" + DateTime.Parse(clvar.RegEndDate).ToString("yyyy-MM-dd") + "',\n" +/////////////////////////
                    "'" + clvar.PickupInstructions + "',\n" +
                    "'" + clvar.DomesticAmonTo + "',\n" +
                    "'" + clvar.InternationalAmonTo + "',\n" +
                    "'" + clvar.DomesticPackets + "',\n" +
                    "'" + clvar.InternationalPackets + "',\n" +
                    "'" + clvar.DomesticAmount + "',\n" +
                    "'" + clvar.InternationalAmount + "',\n" +
                    "'" + clvar.Status + "',\n" +
                    "'" + clvar.PrintingStatus + "',\n" +
                    "'" + clvar.BillingMode + "',\n" +
                    "'" + clvar.DiscountOnDomestic + "',\n" +
                    "'" + clvar.DiscountOnSample + "',\n" +
                    "'" + clvar.DiscountOnDocument + "',\n" +
                    "'" + clvar.PrepareBillType + "',\n" +
                    "'" + clvar.CreditLimit + "',\n" +
                    "'" + clvar.SalesTaxNo + "',\n" +
                    "'" + clvar.Memo + "',\n" +
                    "'" + clvar.BillTaxType + "',\n" +
                    "'" + clvar.Category + "',\n" +
                    "'" + clvar.ClientGroupID + "',\n" +
                    "'" + clvar.RecoveryExpID + "',\n" +
                    "'" + clvar.SalesRoute + "',\n" +
                    "'" + clvar.RecoveryOfficer + "',\n" +
                    "'" + clvar.SalesExecutive + "',\n" +
                    "'" + clvar.RedeemWindow + "',\n" +
                    "'" + clvar.OverdueCalculationBase + "',\n" +
                    "'" + clvar.OverdueValue + "',\n" +
                    "'" + HttpContext.Current.Session["U_ID"].ToString() + "',\n" +
                    " GETDATE(),\n" +
                    "'1',--SectorID\n" +
                    "'" + clvar.Industry + "',\n" +
                    "'" + clvar.AccountNo + "',\n" +
                    "'" + clvar.CreditClientType + "',\n" +
                    "'" + dr["ZoneCode"].ToString() + "',\n" +
                    "'" + dr["BranchCode"].ToString() + "',\n" +
                    "'" + dr["expressCenterCode"].ToString() + "',\n" +
                    "'" + clvar.NTNNo + "',\n" +
                    "'" + clvar.IsCOD + "',\n" +
                    "'1',\n" +
                    "'" + clvar.IsSpecial + "',\n" +
                    "'" + clvar.IsFranchise + "',\n" +
                    "'" + clvar.RecoveryOfficerName + "',\n" +
                     "'" + clvar.RecoveryOfficerID + "',\n" +
                    "'1',\n";
                    if (parentBranch == dr["BranchCode"].ToString())
                    {
                        sqlString += "'1',\n";
                    }
                    else
                    {
                        sqlString += "'0',\n";
                    }
                    if (dd_Branch.SelectedValue == dr["BranchCode"].ToString())
                    {
                        sqlString += "'" + clvar.MailingAddress + "',\n";
                    }
                    else
                    {
                        sqlString += "'',\n";
                    }
                    sqlString +=
                    //"'" + clvar.MailingAddress + "',\n" +
                    "'" + clvar.OriginEc + "',\n" +
                    "'" + clvar.StatusCode + "',\n" +
                    "'" + clvar.CODType + "',\n" +
                    "'" + isMinBilling + "',\n" +
                    "'" + minBillingAmount.ToString() + "',\n" +
                    "'" + clvar.IsSmsServiceActive + "',\n" +
                    "'" + rnrWeight + "',\n" +
                    "'" + clvar.HSCNCharges + "', \n" +
                    "'" + BeneficiaryName + "', \n" +
                    "'" + BeneficiaryBankAccNo + "', \n" +
                    "'" + BeneficiaryBankCode + "', \n" +
                    "'" + ispiece_only + "', \n" +
                    "'" + IsDestination + "', \n" +
                    "'" + ContactPersonDesignation + "', \n" +
                    "'" + ParentID + "', '" + dd_fafType.SelectedValue + "'\n" +
                    "   )";

                    string staffQuery = "";
                    string error = "";




                    cmd.CommandText = sqlString;
                    //cmd.ExecuteNonQuery();

                    object clientID = cmd.ExecuteScalar();
                    Int64 id = Int64.Parse(clientID.ToString());
                    if (dr["BranchCode"].ToString() == dd_Branch.SelectedValue)
                    {

                        foreach (DataRow row in staff.Rows)
                        {
                            if (row["STAFFID"].ToString().Trim() != "")
                            {
                                staffQuery = "insert into ClientStaff (ClientId, UserName, StaffTypeId)\n" +
                                "            values\n" +
                                "            (\n" +
                                "               '" + id.ToString() + "',\n" +
                                "               '" + row["staffMember"].ToString() + "',\n" +
                                "               '" + row["StaffTypeID"].ToString() + "'\n" +
                                "            )";
                                cmd.CommandText = staffQuery;
                                cmd.ExecuteNonQuery();
                                queries.Add(staffQuery);
                            }

                        }
                    }
                    int sortOrder = 0;
                    foreach (DataRow row in modifiers.Rows)
                    {
                        sortOrder = sortOrder++;// +1;
                        string priceModifierQuery = "insert into ClientPriceModifierAssociation \n" +
                                                    "(\n" +
                                                    "		creditClientId,\n" +
                                                    "		priceModifierId,\n" +
                                                    "		sortOrder, \n" +
                                                    "		modifiedCalculationValue, \n" +
                                                    "		calculationBase, \n" +
                                                    "		createdBy, \n" +
                                                    "		createdOn\n" +
                                                    "	)\n" +
                                                    "	Values\n" +
                                                    "	(\n" +
                                                    "       '" + id.ToString() + "',\n" +
                                                    "       '" + row["id"].ToString() + "',\n" +
                                                    "       '" + sortOrder.ToString() + "',\n" +
                                                    "       '" + row["CalculationValue"].ToString() + "',\n" +
                                                    "       '" + row["CalculationBase"].ToString() + "',\n" +
                                                    "       '" + HttpContext.Current.Session["U_ID"].ToString() + "',\n" +
                                                    "GETDATE()\n" +
                                                    ")";
                        cmd.CommandText = priceModifierQuery;
                        cmd.ExecuteNonQuery();
                    }
                    foreach (DataRow dr_ in dtService.Rows)
                    {
                        if (dr_["Status"].ToString() == "1")
                        {
                            string serviceInsertCommand = "INSERT INTO MNP_CUSTOMER_SERVICEMAP\n" +
                            "  (\n" +
                            "   -- ID -- THIS COLUMN VALUE IS AUTO-GENERATED\n" +
                            "   CREDITCLIENTID,\n" +
                            "   SERVICETYPENAME,\n" +
                            "   STATUS,\n" +
                            "   CREATEDON,\n" +
                            "   CREATEDBY,\n" +
                            "   MODIFIEDON,\n" +
                            "   MODIFIEDBY)\n" +
                            "VALUES\n" +
                            "  ('" + id.ToString() + "', '" + dr_["ServiceTypeName"].ToString() + "', '1', GETDATE(), '" + HttpContext.Current.Session["U_ID"].ToString() + "', NULL, NULL)";
                            cmd.CommandText = serviceInsertCommand;
                            cmd.ExecuteNonQuery();
                        }
                    }

                }
                trans.Commit();

            }
            catch (Exception ex)
            {
                trans.Rollback();
                errorMessage = ex.Message;
            }

            return errorMessage;
        }
        public string AddNationWideCustomer_Temp(CL_Customer clvar, DataTable modifiers, DataTable staff, DataTable branches, string parentBranch, DataTable dtService)
        {
            string errorMessage = "";
            SqlConnection con = new SqlConnection(clvar.Strcon());
            SqlConnection con_ = new SqlConnection(clvar.Strcon());

            SqlTransaction trans;
            con.Open();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            trans = con.BeginTransaction(IsolationLevel.ReadUncommitted);
            cmd.Transaction = trans;
            string query = "";
            List<String> queries = new List<string>();

            try
            {
                foreach (DataRow dr in branches.Rows)
                {
                    string sqlString = "insert into Mnp_TempCreditClients \n" +
                    "  (name,\n" +//
                    "   contactPerson,\n" +//
                    "   phoneNo,\n" +//
                    "   faxNo,\n" +//
                    "   email,\n" +//
                    "   address,\n" +//
                    "   centralizedClient,\n" +//
                    "   regDate,\n" +//
                    "   regEndDate,\n" +//
                    "   pickUpInstruction,\n" +//
                    "   domesticAMonTo,\n" +
                    "   internationalAMonTo,\n" +
                    "   domesticPackets,\n" +
                    "   internationalPackets,\n" +
                    "   domesticAmount,\n" +
                    "   internationalAmount,\n" +
                    "   status,\n" +
                    "   printingStatus,\n" +
                    "   billingMode,\n" +
                    "   discountOnDomestic,\n" +
                    "   discountOnSample,\n" +
                    "   discountOnDocument,\n" +
                    "   prepareBillType,\n" +
                    "   creditLimit,\n" +
                    "   salesTaxNo,\n" +
                    "   memo,\n" +
                    "   billTaxType,\n" +
                    "   catId,\n" +
                    "   clientGrpId,\n" +
                    "   recoveryExpCenId,\n" +
                    "   salesRouteId,\n" +
                    "   recoveryOfficer,\n" +
                    "   salesExecutive,\n" +
                    "   redeemWindow,\n" +
                    "   overdueCalBase,\n" +
                    "   overdueValue,\n" +
                    "   createdBy,\n" +
                    "   createdOn,\n" +
                    "   SectorId,\n" +
                    "   IndustryId,\n" +
                    "   accountNo,\n" +
                    "   creditClientType,\n" +
                    "   zoneCode,\n" +
                    "   branchCode,\n" +
                    "   expressCenterCode,\n" +
                    "   ntnNo,\n" +
                    "   IsCOD,\n" +
                    "   isActive,\n" +
                    "   IsSpecial,\n" +
                    "   isFranchisee,\n" +
                    "   recoveryOfficerName,\n" +
                    "   recoveryOfficer_id,\n" +
                    "   isNationWide,\n" +
                    "   isParent,\n" +
                    "   MailingAddress,\n" +
                    "   OriginEC,\n" +
                    "   StatusCode,\n" +
                    "   CODType,\n" +
                    "   isMinBilling,\n" +
                    "   MonthlyFixCharges,\n" +
                    "   IsSmsServiceActive,\n" +
                    "   RNR_WEight,\n" +
                    "   HSCNCharges,\n" +
                    "   BeneficiaryName, \n" +
                    "   BeneficiaryBankAccNo, \n" +
                    "   BeneficiaryBankCode, \n" +
                    "   ispiece_only, \n" +
                    "   IsDestination, \n" +
                    "   ContactPersonDesignation, \n" +
                    "   parentID, faftype\n" +
                    "  )\n output inserted.id \n" +
                    "Values\n" +
                    "  (\n" +
                    "'" + clvar.Name + "' ,\n" +
                    "'" + clvar.ContactPerson + "',\n" +
                    "'" + clvar.PhoneNo + "',\n" +
                    "'" + clvar.FaxNo + "',\n" +
                    "'" + clvar.CustomerEmail + "',\n";/////////////////
                    if (dd_Branch.SelectedValue == dr["BranchCODE"].ToString())
                    {
                        sqlString += "'" + clvar.OfficialAddress + "',\n";
                    }
                    else
                    {
                        sqlString += "'',\n";
                    }

                    sqlString += "'1',\n" + //nationWide ki soorat main centralized laazmi hai
                    "'" + DateTime.Parse(clvar.RegDate).ToString("yyyy-MM-dd") + "',\n" +
                    "'" + DateTime.Parse(clvar.RegEndDate).ToString("yyyy-MM-dd") + "',\n" +/////////////////////////
                    "'" + clvar.PickupInstructions + "',\n" +
                    "'" + clvar.DomesticAmonTo + "',\n" +
                    "'" + clvar.InternationalAmonTo + "',\n" +
                    "'" + clvar.DomesticPackets + "',\n" +
                    "'" + clvar.InternationalPackets + "',\n" +
                    "'" + clvar.DomesticAmount + "',\n" +
                    "'" + clvar.InternationalAmount + "',\n" +
                    "'" + clvar.Status + "',\n" +
                    "'" + clvar.PrintingStatus + "',\n" +
                    "'" + clvar.BillingMode + "',\n" +
                    "'" + clvar.DiscountOnDomestic + "',\n" +
                    "'" + clvar.DiscountOnSample + "',\n" +
                    "'" + clvar.DiscountOnDocument + "',\n" +
                    "'" + clvar.PrepareBillType + "',\n" +
                    "'" + clvar.CreditLimit + "',\n" +
                    "'" + clvar.SalesTaxNo + "',\n" +
                    "'" + clvar.Memo + "',\n" +
                    "'" + clvar.BillTaxType + "',\n" +
                    "'" + clvar.Category + "',\n" +
                    "'" + clvar.ClientGroupID + "',\n" +
                    "'" + clvar.RecoveryExpID + "',\n" +
                    "'" + clvar.SalesRoute + "',\n" +
                    "'" + clvar.RecoveryOfficer + "',\n" +
                    "'" + clvar.SalesExecutive + "',\n" +
                    "'" + clvar.RedeemWindow + "',\n" +
                    "'" + clvar.OverdueCalculationBase + "',\n" +
                    "'" + clvar.OverdueValue + "',\n" +
                    "'" + HttpContext.Current.Session["U_ID"].ToString() + "',\n" +
                    " GETDATE(),\n" +
                    "'1',--SectorID\n" +
                    "'" + clvar.Industry + "',\n" +
                    "'" + clvar.AccountNo + "',\n" +
                    "'" + clvar.CreditClientType + "',\n" +
                    "'" + dr["ZoneCode"].ToString() + "',\n" +
                    "'" + dr["BranchCode"].ToString() + "',\n" +
                    "'" + dr["expressCenterCode"].ToString() + "',\n" +
                    "'" + clvar.NTNNo + "',\n" +
                    "'" + clvar.IsCOD + "',\n" +
                    "'1',\n" +
                    "'" + clvar.IsSpecial + "',\n" +
                    "'" + clvar.IsFranchise + "',\n" +
                    "'" + clvar.RecoveryOfficerName + "',\n" +
                     "'" + clvar.RecoveryOfficerID + "',\n" +
                    "'1',\n";
                    if (parentBranch == dr["BranchCode"].ToString())
                    {
                        sqlString += "'1',\n";
                    }
                    else
                    {
                        sqlString += "'0',\n";
                    }
                    if (dd_Branch.SelectedValue == dr["BranchCode"].ToString())
                    {
                        sqlString += "'" + clvar.MailingAddress + "',\n";
                    }
                    else
                    {
                        sqlString += "'',\n";
                    }
                    sqlString +=
                    //"'" + clvar.MailingAddress + "',\n" +
                    "'" + clvar.OriginEc + "',\n" +
                    "'" + clvar.StatusCode + "',\n" +
                    "'" + clvar.CODType + "',\n" +
                    "'" + isMinBilling + "',\n" +
                    "'" + minBillingAmount.ToString() + "',\n" +
                    "'" + clvar.IsSmsServiceActive + "',\n" +
                    "'" + rnrWeight + "',\n" +
                    "'" + clvar.HSCNCharges + "', \n" +
                    "'" + BeneficiaryName + "', \n" +
                    "'" + BeneficiaryBankAccNo + "', \n" +
                    "'" + BeneficiaryBankCode + "', \n" +
                    "'" + ispiece_only + "', \n" +
                    "'" + IsDestination + "', \n" +
                    "'" + ContactPersonDesignation + "', \n" +
                    "'" + ParentID + "', '" + dd_fafType.SelectedValue + "'\n" +
                    "   )";

                    string staffQuery = "";
                    string error = "";




                    cmd.CommandText = sqlString;
                    //cmd.ExecuteNonQuery();

                    object clientID = cmd.ExecuteScalar();
                    Int64 id = Int64.Parse(clientID.ToString());
                    if (dr["BranchCode"].ToString() == dd_Branch.SelectedValue)
                    {

                        foreach (DataRow row in staff.Rows)
                        {
                            if (row["STAFFID"].ToString().Trim() != "")
                            {
                                staffQuery = "insert into Mnp_TempClientStaff (TempClientId, UserName, StaffTypeId)\n" +
                                "            values\n" +
                                "            (\n" +
                                "               '" + id.ToString() + "',\n" +
                                "               '" + row["staffMember"].ToString() + "',\n" +
                                "               '" + row["StaffTypeID"].ToString() + "'\n" +
                                "            )";
                                cmd.CommandText = staffQuery;
                                cmd.ExecuteNonQuery();
                                queries.Add(staffQuery);
                            }

                        }
                    }
                    int sortOrder = 0;
                    foreach (DataRow row in modifiers.Rows)
                    {
                        sortOrder = sortOrder++;// +1;
                        string priceModifierQuery = "insert into Mnp_TempClientPriceModifierAssociation \n" +
                                                    "(\n" +
                                                    "		TempClientId,\n" +
                                                    "		priceModifierId,\n" +
                                                    "		sortOrder, \n" +
                                                    "		modifiedCalculationValue, \n" +
                                                    "		calculationBase, \n" +
                                                    "		createdBy, \n" +
                                                    "		createdOn\n" +
                                                    "	)\n" +
                                                    "	Values\n" +
                                                    "	(\n" +
                                                    "       '" + id.ToString() + "',\n" +
                                                    "       '" + row["id"].ToString() + "',\n" +
                                                    "       '" + sortOrder.ToString() + "',\n" +
                                                    "       '" + row["CalculationValue"].ToString() + "',\n" +
                                                    "       '" + row["CalculationBase"].ToString() + "',\n" +
                                                    "       '" + HttpContext.Current.Session["U_ID"].ToString() + "',\n" +
                                                    "GETDATE()\n" +
                                                    ")";
                        cmd.CommandText = priceModifierQuery;
                        cmd.ExecuteNonQuery();
                    }
                    foreach (DataRow dr_ in dtService.Rows)
                    {
                        if (dr_["Status"].ToString() == "1")
                        {
                            string serviceInsertCommand = "INSERT INTO MnP_TempCustomerServiceMap  \n" +
                            "  (\n" +
                            "   -- ID -- THIS COLUMN VALUE IS AUTO-GENERATED\n" +
                            "   TempClientId,\n" +
                            "   SERVICETYPENAME,\n" +
                            "   STATUS,\n" +
                            "   CREATEDON,\n" +
                            "   CREATEDBY,\n" +
                            "   MODIFIEDON,\n" +
                            "   MODIFIEDBY)\n" +
                            "VALUES\n" +
                            "  ('" + id.ToString() + "', '" + dr_["ServiceTypeName"].ToString() + "', '1', GETDATE(), '" + HttpContext.Current.Session["U_ID"].ToString() + "', NULL, NULL)";
                            cmd.CommandText = serviceInsertCommand;
                            cmd.ExecuteNonQuery();
                        }
                    }

                }
                trans.Commit();

            }
            catch (Exception ex)
            {
                trans.Rollback();
                errorMessage = ex.Message;
            }

            return errorMessage;
        }
        public string AddCentralizedCustomer(CL_Customer clvar, DataTable modifiers, DataTable staff, DataTable branches, string parentBranch)
        {
            string errorMessage = "";
            SqlConnection con = new SqlConnection(clvar.Strcon());
            SqlConnection con_ = new SqlConnection(clvar.Strcon());

            SqlTransaction trans;
            con.Open();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            trans = con.BeginTransaction(IsolationLevel.ReadUncommitted);
            cmd.Transaction = trans;
            string query = "";
            List<String> queries = new List<string>();

            try
            {
                foreach (DataRow dr in branches.Rows)
                {



                    string sqlString = "insert into CreditClients\n" +
                    "  (name,\n" +
                    "   contactPerson,\n" +
                    "   phoneNo,\n" +
                    "   faxNo,\n" +
                    "   email,\n" +////////////////
                    "   address,\n" +
                    "   centralizedClient,\n" +
                    "   regDate,\n" +
                    "   regEndDate,\n" +///////////
                    "   pickUpInstruction,\n" +
                    "   domesticAMonTo,\n" +
                    "   internationalAMonTo,\n" +//////////////
                    "   domesticPackets,\n" +
                    "   internationalPackets,\n" +
                    "   domesticAmount,\n" +
                    "   internationalAmount,\n" +
                    "   status,\n" +
                    "   printingStatus,\n" +
                    "   billingMode,\n" +
                    "   discountOnDomestic,\n" +
                    "   discountOnSample,\n" +
                    "   discountOnDocument,\n" +
                    "   prepareBillType,\n" +
                    "   creditLimit,\n" +
                    "   salesTaxNo,\n" +
                    "   memo,\n" +
                    "   billTaxType,\n" +
                    "   catId,\n" +
                    "   clientGrpId,\n" +
                    "   recoveryExpCenId,\n" +
                    "   salesRouteId,\n" +
                    "   recoveryOfficer,\n" +
                    "   salesExecutive,\n" +
                    "   redeemWindow,\n" +
                    "   overdueCalBase,\n" +
                    "   overdueValue,\n" +
                    "   createdBy,\n" +
                    "   createdOn,\n" +
                    "   SectorId,\n" +
                    "   IndustryId,\n" +
                    "   accountNo,\n" +
                    "   creditClientType,\n" +////////////////////////////////////////////////
                    "   zoneCode,\n" +
                    "   branchCode,\n" +
                    "   expressCenterCode,\n" +/////////////////////
                    "   ntnNo,\n" +
                    "   IsCOD,\n" +
                    "   isActive,\n" +///////////////////
                    "   IsSpecial,\n" +
                    "   isFranchisee,\n" +
                    "   recoveryOfficerName,\n" +
                    "   recoveryOfficer_id,\n" +
                    "   isNationWide,\n" +
                    "   isParent,\n" +
                    "   MailingAddress,\n" +
                    "   OriginEC,\n" +
                    "   StatusCode,\n" +
                    "   CODType,\n" +
                    "   IsMinBilling,\n" +
                    "   MonthlyFixCharges,\n" +
                    "   IsSmsServiceActive,\n" +
                    "   RNR_WEIGHT,\n" +
                    "   HSCNCharges, BeneficiaryName, BeneficiaryBankAccNo, BeneficiaryBankCode, ispiece_only, IsDestination, contactPersonDesignation, ParentID, faftype\n" +
                    "  )\n output inserted.id \n" +
                    "Values\n" +
                    "  (\n" +
                    "'" + clvar.Name + "',\n" +
                    "'" + clvar.ContactPerson + "',\n" +
                    "'" + clvar.PhoneNo + "',\n" +
                    "'" + clvar.FaxNo + "',\n" +
                    "'" + clvar.CustomerEmail + "',\n" +/////////////////
                    "'" + clvar.OfficialAddress + "',\n" +
                    "'1',\n" +
                    "'" + DateTime.Parse(clvar.RegDate).ToString("yyyy-MM-dd") + "',\n" +
                    "'" + DateTime.Parse(clvar.RegEndDate).ToString("yyyy-MM-dd") + "',\n" +/////////////////////////
                    "'" + clvar.PickupInstructions + "',\n" +
                    "'" + clvar.DomesticAmonTo + "',\n" +
                    "'" + clvar.InternationalAmonTo + "',\n" +
                    "'" + clvar.DomesticPackets + "',\n" +
                    "'" + clvar.InternationalPackets + "',\n" +
                    "'" + clvar.DomesticAmount + "',\n" +
                    "'" + clvar.InternationalAmount + "',\n" +
                    "'" + clvar.Status + "',\n" +
                    "'" + clvar.PrintingStatus + "',\n" +
                    "'" + clvar.BillingMode + "',\n" +
                    "'" + clvar.DiscountOnDomestic + "',\n" +
                    "'" + clvar.DiscountOnSample + "',\n" +
                    "'" + clvar.DiscountOnDocument + "',\n" +
                    "'" + clvar.PrepareBillType + "',\n" +
                    "'" + clvar.CreditLimit + "',\n" +
                    "'" + clvar.SalesTaxNo + "',\n" +
                    "'" + clvar.Memo + "',\n" +
                    "'" + clvar.BillTaxType + "',\n" +
                    "'" + clvar.Category + "',\n" +
                    "'" + clvar.ClientGroupID + "',\n" +
                    "'" + clvar.RecoveryExpID + "',\n" +
                    "'" + clvar.SalesRoute + "',\n" +
                    "'" + clvar.RecoveryOfficer + "',\n" +
                    "'" + clvar.SalesExecutive + "',\n" +
                    "'" + clvar.RedeemWindow + "',\n" +
                    "'" + clvar.OverdueCalculationBase + "',\n" +
                    "'" + clvar.OverdueValue + "',\n" +
                    "'" + HttpContext.Current.Session["U_ID"].ToString() + "',\n" +
                    " GETDATE(),\n" +
                    "'1',--SectorID\n" +
                    "'" + clvar.Industry + "',\n" +
                    "dbo.GenerateAccountNumber('" + dr["BranchCode"].ToString() + "', '" + clvar.Name + "'),\n" +
                    //"'" + clvar.AccountNo + "',\n" +
                    "'" + clvar.CreditClientType + "',\n" +
                    "'" + dr["ZoneCode"].ToString() + "',\n" +
                    "'" + dr["BranchCode"].ToString() + "',\n" +
                    "'" + dr["expressCenterCode"].ToString() + "',\n" +
                    "'" + clvar.NTNNo + "',\n" +
                    "'" + clvar.IsCOD + "',\n" +
                    "'1',\n" +
                    "'" + clvar.IsSpecial + "',\n" +
                    "'" + clvar.IsFranchise + "',\n" +
                    "'" + clvar.RecoveryOfficerName + "',\n" +
                     "'" + clvar.RecoveryOfficerID + "',\n" +
                    "'0',\n";
                    if (parentBranch == dr["BranchCode"].ToString())
                    {
                        sqlString += "'1',\n";
                    }
                    else
                    {
                        sqlString += "'0',\n";
                    }
                    sqlString +=
                    "'" + clvar.MailingAddress + "',\n" +
                    "'" + clvar.OriginEc + "',\n" +
                    "'" + clvar.StatusCode + "',\n" +
                    "'" + clvar.CODType + "',\n" +
                    "'" + isMinBilling + "',\n" +
                    "'" + minBillingAmount + "',\n" +
                    "'" + clvar.IsSmsServiceActive + "',\n" +
                    "'" + rnrWeight + "',\n" +
                    "'" + clvar.HSCNCharges + "', '" + BeneficiaryName + "', '" + BeneficiaryBankAccNo + "', '" + BeneficiaryBankCode + "', '" + ispiece_only + "', '" + IsDestination + "', '" + ContactPersonDesignation + "', '" + ParentID + "','" + dd_fafType.SelectedValue + "'\n" +
                    "   )";

                    string staffQuery = "";
                    string error = "";




                    cmd.CommandText = sqlString;
                    //cmd.ExecuteNonQuery();

                    object clientID = cmd.ExecuteScalar();
                    Int64 id = Int64.Parse(clientID.ToString());
                    foreach (DataRow row in staff.Rows)
                    {
                        if (row["STAFFID"].ToString().Trim() != "")
                        {
                            staffQuery = "insert into ClientStaff (ClientId, UserName, StaffTypeId)\n" +
                            "            values\n" +
                            "            (\n" +
                            "               '" + id.ToString() + "',\n" +
                            "               '" + row["staffMember"].ToString() + "',\n" +
                            "               '" + row["StaffTypeID"].ToString() + "'\n" +
                            "            )";
                            cmd.CommandText = staffQuery;
                            cmd.ExecuteNonQuery();
                            queries.Add(staffQuery);
                        }

                    }
                    int sortOrder = 0;
                    foreach (DataRow row in modifiers.Rows)
                    {
                        //  sortOrder = sortOrder + 1;
                        sortOrder = sortOrder++;// +1;
                        string priceModifierQuery = "insert into ClientPriceModifierAssociation \n" +
                                                    "(\n" +
                                                    "		creditClientId,\n" +
                                                    "		priceModifierId,\n" +
                                                    "		sortOrder, \n" +
                                                    "		modifiedCalculationValue, \n" +
                                                    "		calculationBase, \n" +
                                                    "		createdBy, \n" +
                                                    "		createdOn\n" +
                                                    "	)\n" +
                                                    "	Values\n" +
                                                    "	(\n" +
                                                    "       '" + id.ToString() + "',\n" +
                                                    "       '" + row["id"].ToString() + "',\n" +
                                                    "       '" + sortOrder.ToString() + "',\n" +
                                                    "       '" + row["CalculationValue"].ToString() + "',\n" +
                                                    "       '" + row["CalculationBase"].ToString() + "',\n" +
                                                    "       '" + HttpContext.Current.Session["U_ID"].ToString() + "',\n" +
                                                    "GETDATE()\n" +
                                                    ")";
                        cmd.CommandText = priceModifierQuery;
                        cmd.ExecuteNonQuery();
                    }

                }
                trans.Commit();

            }
            catch (Exception ex)
            {
                trans.Rollback();
                errorMessage = ex.Message;
            }

            return errorMessage;
        }
        public string UpdateCustomer(CL_Customer clvar, DataTable modifiers, DataTable staff, string clientID, DataTable dtServices)
        {
            string errorMessage = "";
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "UPDATE_CLIENT_NEW";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@tblService", dtServices);
                cmd.Parameters.AddWithValue("@tblPriceModifiers", modifiers);
                cmd.Parameters.AddWithValue("@tblStaff", staff);
                cmd.Parameters.AddWithValue("@clientID", clientID);
                cmd.Parameters.AddWithValue("@name", clvar.Name);
                cmd.Parameters.AddWithValue("@contactPerson", clvar.ContactPerson);
                cmd.Parameters.AddWithValue("@phoneNo", clvar.PhoneNo);
                cmd.Parameters.AddWithValue("@faxNo", clvar.FaxNo);
                cmd.Parameters.AddWithValue("@email", clvar.CustomerEmail);
                cmd.Parameters.AddWithValue("@address", clvar.OfficialAddress);
                cmd.Parameters.AddWithValue("@centralizedClient", clvar.IsCentralized);
                cmd.Parameters.AddWithValue("@regDate", DateTime.Parse(clvar.RegDate).ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@regEndDate", DateTime.Parse(clvar.RegEndDate).ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@pickUpInstruction", clvar.PickupInstructions);
                cmd.Parameters.AddWithValue("@domesticAMonTo", clvar.DomesticAmonTo);
                cmd.Parameters.AddWithValue("@internationalAMonTo", clvar.InternationalAmonTo);
                cmd.Parameters.AddWithValue("@domesticPackets", clvar.DomesticPackets);
                cmd.Parameters.AddWithValue("@internationalPackets", clvar.InternationalPackets);
                cmd.Parameters.AddWithValue("@domesticAmount", clvar.DomesticAmount);
                cmd.Parameters.AddWithValue("@internationalAmount", clvar.InternationalAmount);
                cmd.Parameters.AddWithValue("@status", clvar.Status);
                cmd.Parameters.AddWithValue("@printingStatus", clvar.PrintingStatus);
                cmd.Parameters.AddWithValue("@billingMode", clvar.BillingMode);
                cmd.Parameters.AddWithValue("@discountOnDomestic", clvar.DiscountOnDomestic);
                cmd.Parameters.AddWithValue("@discountOnSample", clvar.DiscountOnSample);
                cmd.Parameters.AddWithValue("@discountOnDocument", clvar.DiscountOnDocument);
                cmd.Parameters.AddWithValue("@prepareBillType", clvar.PrepareBillType);
                cmd.Parameters.AddWithValue("@creditLimit", clvar.CreditLimit);
                cmd.Parameters.AddWithValue("@salesTaxNo", clvar.SalesTaxNo);
                cmd.Parameters.AddWithValue("@memo", clvar.Memo);
                cmd.Parameters.AddWithValue("@billTaxType", clvar.BillTaxType);
                cmd.Parameters.AddWithValue("@catID", clvar.Category);
                cmd.Parameters.AddWithValue("@clientGrpId", clvar.ClientGroupID);
                cmd.Parameters.AddWithValue("@recoveryExpCenId", clvar.RecoveryExpID);
                cmd.Parameters.AddWithValue("@salesRouteId", clvar.SalesRoute);
                cmd.Parameters.AddWithValue("@redeemWindow", clvar.RedeemWindow);
                cmd.Parameters.AddWithValue("@modifiedBy", HttpContext.Current.Session["U_ID"].ToString());
                cmd.Parameters.AddWithValue("@SectorId", clvar.Sector);
                cmd.Parameters.AddWithValue("@IndustryId", clvar.Industry);
                cmd.Parameters.AddWithValue("@creditClientType", clvar.CreditClientType);
                cmd.Parameters.AddWithValue("@ntnNo", clvar.NTNNo);
                cmd.Parameters.AddWithValue("@IsSpecial", clvar.IsSpecial);
                if (clvar.IsParent == null)
                {
                    cmd.Parameters.AddWithValue("@isParent", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@isParent", clvar.IsParent);
                }
                cmd.Parameters.AddWithValue("@MailingAddress", clvar.MailingAddress);
                cmd.Parameters.AddWithValue("@OriginEC", clvar.OriginEc);
                cmd.Parameters.AddWithValue("@StatusCode", clvar.StatusCode);
                cmd.Parameters.AddWithValue("@CODType", clvar.CODType);
                cmd.Parameters.AddWithValue("@MonthlyFixCharges", clvar.MonthlyFixedCharges);
                cmd.Parameters.AddWithValue("@IsSmsServiceActive", clvar.IsSmsServiceActive);
                //cmd.Parameters.AddWithValue("@RNRCNCharges", clvar.RnRCharges);
                cmd.Parameters.AddWithValue("@UserID", HttpContext.Current.Session["U_ID"].ToString());
                cmd.Parameters.AddWithValue("@RnRWeight", rnrWeight);
                cmd.Parameters.AddWithValue("@ContactPersonDesignation", ContactPersonDesignation);

                if (rbtn_codMonthlyBillingAmount.SelectedValue == "0")
                {
                    cmd.Parameters.AddWithValue("@isMinBilling", false);
                    cmd.Parameters.AddWithValue("@MinBillingAmount", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@isMinBilling", true);
                    float tempMinBillingAmount = 0;
                    float.TryParse(txt_codMonthlyAmount.Text, out tempMinBillingAmount);
                    if (tempMinBillingAmount <= 0)
                    {
                        Alert("Invalid Monthly Billing amount");
                        return "Invalid Monthly Billing amount";
                    }
                    cmd.Parameters.AddWithValue("@MinBillingAmount", tempMinBillingAmount);
                }
                if (dd_fafType.SelectedValue == "")
                {
                    return "Select Faf Type";
                }
                cmd.Parameters.AddWithValue("@FafType", dd_fafType.SelectedValue);
                cmd.Parameters.AddWithValue("@BeneficiaryName", BeneficiaryName);
                cmd.Parameters.AddWithValue("@BeneficiaryBankAccNo", BeneficiaryBankAccNo);
                cmd.Parameters.AddWithValue("@BeneficiaryBankCode", BeneficiaryBankCode);



                cmd.Parameters.AddWithValue("@isCOD", clvar.IsCOD);
                cmd.Parameters.Add("@error_message", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;


                cmd.ExecuteNonQuery();

                string ReturnValue = cmd.Parameters["@error_message"].Value.ToString();

                if (ReturnValue == "1")
                {
                    errorMessage = "";
                }
                else
                {
                    errorMessage = ReturnValue;
                }








            }
            catch (Exception ex)
            {

                errorMessage = ex.Message;
            }


            return errorMessage;
        }
        public string UpdateTempCustomer(CL_Customer clvar, DataTable modifiers, DataTable staff, string clientID, DataTable dtServices)
        {
            string errorMessage = "";
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "UPDATE_TEMP_CLIENT_NEW";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@tblService", dtServices);
                cmd.Parameters.AddWithValue("@tblPriceModifiers", modifiers);
                cmd.Parameters.AddWithValue("@tblStaff", staff);
                cmd.Parameters.AddWithValue("@clientID", clientID);
                cmd.Parameters.AddWithValue("@name", clvar.Name);
                cmd.Parameters.AddWithValue("@contactPerson", clvar.ContactPerson);
                cmd.Parameters.AddWithValue("@phoneNo", clvar.PhoneNo);
                cmd.Parameters.AddWithValue("@faxNo", clvar.FaxNo);
                cmd.Parameters.AddWithValue("@email", clvar.CustomerEmail);
                cmd.Parameters.AddWithValue("@address", clvar.OfficialAddress);
                cmd.Parameters.AddWithValue("@centralizedClient", clvar.IsCentralized);
                cmd.Parameters.AddWithValue("@regDate", DateTime.Parse(clvar.RegDate).ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@regEndDate", DateTime.Parse(clvar.RegEndDate).ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@pickUpInstruction", clvar.PickupInstructions);
                cmd.Parameters.AddWithValue("@domesticAMonTo", clvar.DomesticAmonTo);
                cmd.Parameters.AddWithValue("@internationalAMonTo", clvar.InternationalAmonTo);
                cmd.Parameters.AddWithValue("@domesticPackets", clvar.DomesticPackets);
                cmd.Parameters.AddWithValue("@internationalPackets", clvar.InternationalPackets);
                cmd.Parameters.AddWithValue("@domesticAmount", clvar.DomesticAmount);
                cmd.Parameters.AddWithValue("@internationalAmount", clvar.InternationalAmount);
                cmd.Parameters.AddWithValue("@status", clvar.Status);
                cmd.Parameters.AddWithValue("@printingStatus", clvar.PrintingStatus);
                cmd.Parameters.AddWithValue("@billingMode", clvar.BillingMode);
                cmd.Parameters.AddWithValue("@discountOnDomestic", clvar.DiscountOnDomestic);
                cmd.Parameters.AddWithValue("@discountOnSample", clvar.DiscountOnSample);
                cmd.Parameters.AddWithValue("@discountOnDocument", clvar.DiscountOnDocument);
                cmd.Parameters.AddWithValue("@prepareBillType", clvar.PrepareBillType);
                cmd.Parameters.AddWithValue("@creditLimit", clvar.CreditLimit);
                cmd.Parameters.AddWithValue("@salesTaxNo", clvar.SalesTaxNo);
                cmd.Parameters.AddWithValue("@memo", clvar.Memo);
                cmd.Parameters.AddWithValue("@billTaxType", clvar.BillTaxType);
                cmd.Parameters.AddWithValue("@catID", clvar.Category);
                cmd.Parameters.AddWithValue("@clientGrpId", clvar.ClientGroupID);
                cmd.Parameters.AddWithValue("@recoveryExpCenId", clvar.RecoveryExpID);
                cmd.Parameters.AddWithValue("@salesRouteId", clvar.SalesRoute);
                cmd.Parameters.AddWithValue("@redeemWindow", clvar.RedeemWindow);
                cmd.Parameters.AddWithValue("@modifiedBy", HttpContext.Current.Session["U_ID"].ToString());
                cmd.Parameters.AddWithValue("@SectorId", clvar.Sector);
                cmd.Parameters.AddWithValue("@IndustryId", clvar.Industry);
                cmd.Parameters.AddWithValue("@creditClientType", clvar.CreditClientType);
                cmd.Parameters.AddWithValue("@ntnNo", clvar.NTNNo);
                cmd.Parameters.AddWithValue("@IsSpecial", clvar.IsSpecial);
                if (clvar.IsParent == null)
                {
                    cmd.Parameters.AddWithValue("@isParent", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@isParent", clvar.IsParent);
                }
                cmd.Parameters.AddWithValue("@MailingAddress", clvar.MailingAddress);
                cmd.Parameters.AddWithValue("@OriginEC", clvar.OriginEc);
                cmd.Parameters.AddWithValue("@StatusCode", clvar.StatusCode);
                cmd.Parameters.AddWithValue("@CODType", clvar.CODType);
                cmd.Parameters.AddWithValue("@MonthlyFixCharges", clvar.MonthlyFixedCharges);
                cmd.Parameters.AddWithValue("@IsSmsServiceActive", clvar.IsSmsServiceActive);
                //cmd.Parameters.AddWithValue("@RNRCNCharges", clvar.RnRCharges);
                cmd.Parameters.AddWithValue("@UserID", HttpContext.Current.Session["U_ID"].ToString());
                cmd.Parameters.AddWithValue("@RnRWeight", rnrWeight);
                cmd.Parameters.AddWithValue("@ContactPersonDesignation", ContactPersonDesignation);

                if (rbtn_codMonthlyBillingAmount.SelectedValue == "0")
                {
                    cmd.Parameters.AddWithValue("@isMinBilling", false);
                    cmd.Parameters.AddWithValue("@MinBillingAmount", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@isMinBilling", true);
                    float tempMinBillingAmount = 0;
                    float.TryParse(txt_codMonthlyAmount.Text, out tempMinBillingAmount);
                    if (tempMinBillingAmount <= 0)
                    {
                        Alert("Invalid Monthly Billing amount");
                        return "Invalid Monthly Billing amount";
                    }
                    cmd.Parameters.AddWithValue("@MinBillingAmount", tempMinBillingAmount);
                }
                if (dd_fafType.SelectedValue == "")
                {
                    return "Select Faf Type";
                }
                cmd.Parameters.AddWithValue("@FafType", dd_fafType.SelectedValue);
                cmd.Parameters.AddWithValue("@BeneficiaryName", BeneficiaryName);
                cmd.Parameters.AddWithValue("@BeneficiaryBankAccNo", BeneficiaryBankAccNo);
                cmd.Parameters.AddWithValue("@BeneficiaryBankCode", BeneficiaryBankCode);
                cmd.Parameters.AddWithValue("@ApprovedStatus", cl_var.Remarks);






                cmd.Parameters.AddWithValue("@isCOD", clvar.IsCOD);
                cmd.Parameters.Add("@error_message", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;


                cmd.ExecuteNonQuery();

                string ReturnValue = cmd.Parameters["@error_message"].Value.ToString();

                if (ReturnValue == "1")
                {
                    errorMessage = "";
                }
                else
                {
                    errorMessage = ReturnValue;
                }








            }
            catch (Exception ex)
            {

                errorMessage = ex.Message;
            }


            return errorMessage;
        }

        public string UpdateNationWiseCustomer(CL_Customer clvar, DataTable modifiers, DataTable staff, string clientID, DataTable branchTable, DataTable dtServices)
        {
            string errorMessage = "";
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "UPDATE_NationWideCLIENT_NEW1";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@AccountNo", clvar.AccountNo);
                cmd.Parameters.AddWithValue("@SelectedBranch", dd_Branch.SelectedValue);
                cmd.Parameters.AddWithValue("@tblService", dtServices);
                cmd.Parameters.AddWithValue("@tblPriceModifiers", modifiers);
                cmd.Parameters.AddWithValue("@tblStaff", staff);
                cmd.Parameters.AddWithValue("@tblBranches", branchTable);
                cmd.Parameters.AddWithValue("@clientID", clientID);
                cmd.Parameters.AddWithValue("@name", clvar.Name);
                cmd.Parameters.AddWithValue("@contactPerson", clvar.ContactPerson);
                cmd.Parameters.AddWithValue("@phoneNo", clvar.PhoneNo);
                cmd.Parameters.AddWithValue("@faxNo", clvar.FaxNo);
                cmd.Parameters.AddWithValue("@email", clvar.CustomerEmail);
                cmd.Parameters.AddWithValue("@address", clvar.OfficialAddress);
                cmd.Parameters.AddWithValue("@centralizedClient", clvar.IsCentralized);
                cmd.Parameters.AddWithValue("@regDate", DateTime.Parse(clvar.RegDate).ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@regEndDate", DateTime.Parse(clvar.RegEndDate).ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@pickUpInstruction", clvar.PickupInstructions.Replace("\n", "."));
                cmd.Parameters.AddWithValue("@domesticAMonTo", clvar.DomesticAmonTo);
                cmd.Parameters.AddWithValue("@internationalAMonTo", clvar.InternationalAmonTo);
                cmd.Parameters.AddWithValue("@domesticPackets", clvar.DomesticPackets);
                cmd.Parameters.AddWithValue("@internationalPackets", clvar.InternationalPackets);
                cmd.Parameters.AddWithValue("@domesticAmount", clvar.DomesticAmount);
                cmd.Parameters.AddWithValue("@internationalAmount", clvar.InternationalAmount);
                cmd.Parameters.AddWithValue("@status", clvar.Status);
                cmd.Parameters.AddWithValue("@printingStatus", clvar.PrintingStatus);
                cmd.Parameters.AddWithValue("@billingMode", clvar.BillingMode);
                cmd.Parameters.AddWithValue("@discountOnDomestic", clvar.DiscountOnDomestic);
                cmd.Parameters.AddWithValue("@discountOnSample", clvar.DiscountOnSample);
                cmd.Parameters.AddWithValue("@discountOnDocument", clvar.DiscountOnDocument);
                cmd.Parameters.AddWithValue("@prepareBillType", clvar.PrepareBillType);
                cmd.Parameters.AddWithValue("@creditLimit", clvar.CreditLimit);
                cmd.Parameters.AddWithValue("@salesTaxNo", clvar.SalesTaxNo);
                cmd.Parameters.AddWithValue("@memo", clvar.Memo);
                cmd.Parameters.AddWithValue("@billTaxType", clvar.BillTaxType);
                cmd.Parameters.AddWithValue("@catID", clvar.Category);
                cmd.Parameters.AddWithValue("@clientGrpId", clvar.ClientGroupID);
                cmd.Parameters.AddWithValue("@recoveryExpCenId", clvar.RecoveryExpID);
                cmd.Parameters.AddWithValue("@salesRouteId", clvar.SalesRoute);
                cmd.Parameters.AddWithValue("@redeemWindow", clvar.RedeemWindow);
                cmd.Parameters.AddWithValue("@modifiedBy", HttpContext.Current.Session["U_ID"].ToString());
                cmd.Parameters.AddWithValue("@SectorId", clvar.Sector);
                cmd.Parameters.AddWithValue("@IndustryId", clvar.Industry);
                cmd.Parameters.AddWithValue("@creditClientType", clvar.CreditClientType);
                cmd.Parameters.AddWithValue("@ntnNo", clvar.NTNNo);
                cmd.Parameters.AddWithValue("@IsSpecial", clvar.IsSpecial);
                if (clvar.IsParent == null)
                {
                    cmd.Parameters.AddWithValue("@isParent", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@isParent", clvar.IsParent);
                }
                cmd.Parameters.AddWithValue("@MailingAddress", clvar.MailingAddress);
                cmd.Parameters.AddWithValue("@OriginEC", clvar.OriginEc);
                cmd.Parameters.AddWithValue("@StatusCode", clvar.StatusCode);
                cmd.Parameters.AddWithValue("@CODType", clvar.CODType);
                cmd.Parameters.AddWithValue("@MonthlyFixCharges", clvar.MonthlyFixedCharges);
                cmd.Parameters.AddWithValue("@IsSmsServiceActive", clvar.IsSmsServiceActive);
                //cmd.Parameters.AddWithValue("@RNRCNCharges", clvar.RnRCharges);
                cmd.Parameters.AddWithValue("@UserID", HttpContext.Current.Session["U_ID"].ToString());
                cmd.Parameters.AddWithValue("@RnRWeight", rnrWeight);
                cmd.Parameters.AddWithValue("@ContactPersonDesignation", ContactPersonDesignation);

                if (rbtn_codMonthlyBillingAmount.SelectedValue == "0")
                {
                    cmd.Parameters.AddWithValue("@isMinBilling", false);
                    cmd.Parameters.AddWithValue("@MinBillingAmount", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@isMinBilling", true);
                    float tempMinBillingAmount = 0;
                    float.TryParse(txt_codMonthlyAmount.Text, out tempMinBillingAmount);
                    if (tempMinBillingAmount <= 0)
                    {
                        Alert("Invalid Monthly Billing amount");
                        return "Invalid Monthly Billing amount";
                    }
                    cmd.Parameters.AddWithValue("@MinBillingAmount", tempMinBillingAmount);
                }
                if (dd_fafType.SelectedValue == "")
                {
                    return "Select Faf Type";
                }
                cmd.Parameters.AddWithValue("@FafType", dd_fafType.SelectedValue);
                cmd.Parameters.AddWithValue("@BeneficiaryName", BeneficiaryName);
                cmd.Parameters.AddWithValue("@BeneficiaryBankAccNo", BeneficiaryBankAccNo);
                cmd.Parameters.AddWithValue("@BeneficiaryBankCode", BeneficiaryBankCode);
                cmd.Parameters.AddWithValue("@isCOD", clvar.IsCOD);
                cmd.Parameters.Add("@error_message", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;


                cmd.ExecuteNonQuery();

                string ReturnValue = cmd.Parameters["@error_message"].Value.ToString();

                if (ReturnValue == "1")
                {
                    errorMessage = "";
                }
                else
                {
                    errorMessage = ReturnValue;
                }

            }
            catch (Exception ex)
            {

                errorMessage = ex.Message;
            }


            return errorMessage;
        }
        public DataTable GetAccountNumber(Cl_Variables clvar)
        {
            char[] arr = clvar.Consigner.ToCharArray();

            string query = "SELECT cc.AccountNo FROM CREDITCLIENTS cc where cc.accountNo like '" + dd_Branch.SelectedValue + arr[0] + "%' and cc.branchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'";
            int br_length = dd_Branch.SelectedValue.ToString().Length;
            query = "selecT ISNULL(MAX(CAST(SUBSTRING(cc.accountNo," + (br_length + 2) + ",50) as bigint)),0) + 1\n" +
                " from CreditClients cc where LEFT(cc.accountNo, " + (br_length + 1) + ") = '" + dd_Branch.SelectedValue + arr[0] + "' and cc.branchCode = '" + dd_Branch.SelectedValue.ToString() + "'";

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
            return dt;
        }

        public DataTable GetTempClientId(Cl_Variables clvar)
        {
            char[] arr = clvar.Consigner.ToCharArray();

            string query;

            int br_length = dd_Branch.SelectedValue.ToString().Length;
            query = "selecT ISNULL(MAX(CAST(SUBSTRING(cc.TempClientId," + (br_length + 2) + ",50) as bigint)),0) + 10000000 \n" +
                    "from Mnp_TempCreditClients cc \n" +
                    "where LEFT(cc.TempClientId, " + (br_length + 1) + ") = '" + dd_Branch.SelectedValue + arr[0] + "' \n" +
                    "and cc.branchCode = '" + dd_Branch.SelectedValue.ToString() + "'";

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
            return dt;
        }

        public DataTable GetBranchesOfCenteralizedAccount(string accountNo)
        {
            string query = "select * from creditClients cc where cc.accountNo = '" + accountNo + "'";
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

        public void Alert(string message)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + message + "')", true);
            Errorid.Text = message;
        }
        protected void DeleteStaffMember(string clientID, string staff)
        {
            string query = "delete from ClientStaff where ClientId = '" + clientID + "' and UserName = '" + staff + "'";
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
            }
            finally { con.Close(); }
        }
        public void DeletePriceModifier(string clientID, string pid)
        {
            string query = "delete from ClientPriceModifierAssociation where creditclientID = '" + clientID + "' and priceModifierID = '" + pid + "'";
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
            }
            finally { con.Close(); }
        }
        protected void txt_acc_no_TextChanged(object sender, EventArgs e)
        {
            if (dd_zone.SelectedValue == "0")
            {
                Alert("Select Zone");
                ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc( 'BasicInfo');", true);
                return;
            }
            if (dd_Branch.SelectedValue == "0")
            {
                Alert("Select Branch");
                ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc( 'BasicInfo');", true);
                return;
            }

            if (txt_acc_no.Text.Trim() == "")
            {
                Alert("Enter Account Number");
            }
            clvar.AccountNo = txt_acc_no.Text;

            DataSet ds = GetAccountDetail(clvar);


            DataTable addedModifiers = ViewState["addedPriceModifiers"] as DataTable;
            addedModifiers.Clear();

            DataTable addedStaff = ViewState["Staffs"] as DataTable;



            foreach (DataRow row in ds.Tables["AssociatedStaff"].Rows)
            {
                string staffType = row["StaffTypeID"].ToString();
                DataRow[] dr = addedStaff.Select("StaffTypeID = '" + staffType + "'");
                if (dr.Count() > 0)
                {
                    dr[0]["StaffMember"] = row["UserName"].ToString();
                }
            }
            gv_Staffs.DataSource = addedStaff;
            gv_Staffs.DataBind();

            foreach (GridViewRow row in gv_Staffs.Rows)
            {
                clvar.StaffType = (row.FindControl("hd_StaffTypeID") as HiddenField).Value;
                DataTable dt = GetClientStaff_(clvar);
                DropDownList dd = row.FindControl("dd_gStaffMembers") as DropDownList;
                DataView dv = dt.AsDataView();
                dd.Items.Add(new ListItem { Text = "Select Staff", Value = "0" });
                dv.Sort = "username";
                dd.DataSource = dv;
                dd.DataTextField = "UserName";
                dd.DataValueField = "ID";
                dd.DataBind();
                //DataRow[] dr = addedStaff.Select("StaffTypeID = '" + staffType + "'");
                DataRow[] dr = addedStaff.Select("StaffTypeID = '" + clvar.StaffType + "'");
                if (dr.Count() > 0)
                {
                    if (dr[0]["StaffMember"].ToString().Trim() != "")
                    {
                        dd.SelectedItem.Text = dr[0]["StaffMember"].ToString();
                    }

                }


            }

            foreach (DataRow row in ds.Tables[2].Rows)
            {
                DataRow dr = addedModifiers.NewRow();
                dr["ID"] = row["priceModifierID"].ToString();
                dr["NAme"] = row["name"].ToString();
                dr["CalculationValue"] = row["ModifiedCalculationValue"].ToString();
                dr["CalculationBase"] = row["CalculationBase"].ToString();
                dr["Description"] = "";

                addedModifiers.Rows.Add(dr);
            }

            gridview_tab4.DataSource = addedModifiers;
            gridview_tab4.DataBind();

            if (ds.Tables[0].Rows.Count > 0)
            {
                chk_NotCod.Enabled = false;
                dd_codType.Enabled = false;
                rbtn_printStatus.Enabled = false;
                txt_RegDate.Enabled = false;
                chk_special_cust.Enabled = false;



                DataRow dr = ds.Tables[0].Rows[0];

                if (dr["isMinBilling"].ToString() == "1" || dr["isMinBilling"].ToString().ToUpper() == "TRUE")
                {
                    rbtn_codMonthlyBillingAmount.SelectedValue = "1";
                }
                else
                {
                    rbtn_codMonthlyBillingAmount.SelectedValue = "0";
                }

                if (dr["FAFType"].ToString().Trim() == "")
                {
                    dd_fafType.SelectedIndex = 0;
                    dd_fafType.Enabled = true;
                }
                else if (dr["FAFType"].ToString() == "1" || dr["FAFType"].ToString().ToUpper() == "TRUE")
                {
                    dd_fafType.SelectedValue = "1";
                    dd_fafType.Enabled = false;
                }
                else
                {
                    dd_fafType.SelectedValue = "0";
                    dd_fafType.Enabled = false;
                }

                txt_benName.Text = dr["BeneficiaryName"].ToString();
                txt_benAccNo.Text = dr["BeneficiaryBankAccNo"].ToString();

                ListItem bank = dd_benBank.Items.FindByValue(dr["BeneficiaryBankCode"].ToString());
                if (bank != null)
                {
                    dd_benBank.ClearSelection();
                    bank.Selected = true;
                }


                hd_creditClientID.Value = dr["id"].ToString();
                txt_name.Text = dr["name"].ToString();
                txt_contactPerson.Text = dr["ContactPerson"].ToString();
                txt_PhoneNo.Text = dr["PhoneNo"].ToString();
                txt_FaxNo.Text = dr["faxNo"].ToString();
                txt_email.Text = dr["email"].ToString();
                txt_OfficialAddress.Text = dr["address"].ToString();
                if (dr["isNationWide"].ToString() == "1" || dr["isNationWide"].ToString().ToUpper() == "TRUE")
                {
                    chk_centralisedClient.Checked = true;
                    chk_nationWide.Checked = true;

                    DataTable centeralizedAccBrs = GetBranchesOfCenteralizedAccount(clvar.AccountNo);
                    txt_nationWideAccountNo.Text = clvar.AccountNo;
                    txt_nationWideGroup.Text = dr["ClientGrpID"].ToString();
                    if (centeralizedAccBrs != null)
                    {
                        if (centeralizedAccBrs.Rows.Count > 0)
                        {
                            foreach (DataRow row in centeralizedAccBrs.Rows)
                            {
                                if (chkList_Branches.Items.FindByValue(row["BranchCode"].ToString()) != null)
                                {
                                    chkList_Branches.Items.FindByValue(row["BranchCode"].ToString()).Selected = true;

                                }
                                if (row["isParent"].ToString() == "1" || row["isParent"].ToString().ToUpper() == "TRUE")
                                {
                                    dd_parentBranch.SelectedValue = row["BranchCode"].ToString();
                                }
                            }
                        }
                        else
                        {
                            AlertMessage("Branches Not found for this Account");
                            return;
                        }
                    }
                    else
                    {
                        AlertMessage("Branches Not found for this Account");
                        return;
                    }

                }
                else if (ds.Tables[0].Rows[0]["CentralizedClient"].ToString() == "1" || ds.Tables[0].Rows[0]["CentralizedClient"].ToString().ToUpper() == "TRUE")
                {
                    chk_centralisedClient.Checked = true;

                    txt_nationWideGroup.Text = ds.Tables[0].Rows[0]["clientGrpId"].ToString();
                    txt_nationWideGroup.Enabled = true;
                    txt_nationWideGroup.Attributes.Add("disabled", "false");
                }
                else
                {
                    chk_centralisedClient.Checked = false;
                    txt_nationWideGroup.Enabled = false;
                    txt_nationWideGroup.Attributes.Add("disabled", "true");
                }

                txt_RegDate.Text = dr["regDate"].ToString();
                txt_RegEndDate.Text = dr["regEndDate"].ToString();
                txt_pickupInstruction.Text = dr["pickUpInstruction"].ToString();
                txt_DomesticTurnOver.Text = dr["domesticAMonTo"].ToString();
                txt_InternationalTurnOver.Text = dr["internationalAMonTo"].ToString();
                txt_DomesticPackets.Text = dr["domesticPackets"].ToString();
                txt_InternationalPackets.Text = dr["internationalPackets"].ToString();
                txt_DomesticAmount.Text = dr["domesticAmount"].ToString();
                txt_InternationalAmount.Text = dr["internationalAmount"].ToString();
                rbtn_Status.SelectedValue = dr["isactive_"].ToString();
                rbtn_printStatus.SelectedValue = dr["printingStatus"].ToString();
                rbtn_billingMode.SelectedValue = dr["billingMode"].ToString();
                txt_DiscountOnDomestic.Text = dr["discountOnDomestic"].ToString();
                txt_DiscountOnSample.Text = dr["discountOnSample"].ToString();
                txt_DiscountOnDocument.Text = dr["discountOnDocument"].ToString();
                dd_prepareBill.SelectedValue = dr["prepareBillType"].ToString();
                txt_CreditLimit.Text = dr["creditLimit"].ToString();
                txt_SalesTaxNo.Text = dr["salesTaxNo"].ToString();
                //txt_memo.Text = dr["memo"].ToString();
                rbtn_BillTaxType.SelectedValue = dr["billTaxType"].ToString();
                //dd_CustomerCategory.SelectedValue = dr["catId"].ToString();
                txt_designation.Text = dr["ContactPersonDesignation"].ToString();


                if (dr["ClientGrpID"].ToString() != "")
                {
                    //dd_clientGroups.SelectedValue = dr["clientGrpId"].ToString();

                    //lbl_groupName.Text = dr["GroupName"].ToString();
                    //txt_clientGroups.Text = dr["clientGrpID"].ToString();
                }
                dd_recoveryExpID.ClearSelection();
                foreach (ListItem item in dd_recoveryExpID.Items)
                {
                    if (dr["recoveryexpcenid"].ToString() == item.Value)
                    {
                        item.Selected = true;
                        break;
                    }
                }
                //dd_recoveryExpID.SelectedValue = dr["recoveryExpCenId"].ToString();
                //dd_SalesRoute.ClearSelection();
                //foreach (ListItem item in dd_SalesRoute.Items)
                //{
                //    if (dr["salesRouteId"].ToString() == item.Value)
                //    {
                //        item.Selected = true;
                //        break;
                //    }
                //}
                //dd_SalesRoute.SelectedValue = dr["salesRouteId"].ToString();
                //            txt_redeemWindow.Text = dr["redeemWindow"].ToString();
                //dd_Sector.ClearSelection();
                //foreach (ListItem item in dd_Sector.Items)
                //{
                //    if (dr["SectorId"].ToString() == item.Value)
                //    {
                //        item.Selected = true;
                //        break;
                //    }
                //}
                //dd_Sector.SelectedValue = dr["SectorId"].ToString();
                dd_Industry.ClearSelection();
                foreach (ListItem item in dd_Industry.Items)
                {
                    if (dr["IndustryId"].ToString() == item.Value)
                    {
                        item.Selected = true;
                        break;
                    }
                }
                //dd_Industry.SelectedValue = dr["IndustryId"].ToString();
                txt_acc_no.Text = dr["accountNo"].ToString();
                rbtn_customerType.ClearSelection();
                foreach (ListItem item in rbtn_customerType.Items)
                {
                    if (dr["creditClientType"].ToString() == item.Value)
                    {
                        item.Selected = true;
                        break;
                    }
                }
                //rbtn_customerType.SelectedValue = dr["creditClientType"].ToString();
                txt_ntnNo.Text = dr["ntnNo"].ToString();
                if (dr["IsCOD"].ToString().ToLower() == "true")
                {
                    chk_NotCod.Checked = true;
                }
                else
                {
                    chk_NotCod.Checked = false;
                }
                if (dr["IsSpecial"].ToString() == "1")
                {
                    chk_special_cust.Checked = true;
                }
                else
                {
                    chk_special_cust.Checked = false;
                }
                if (dr["isFranchisee"].ToString() == "1")
                {

                }
                txt_MailingAddress.Text = dr["MailingAddress"].ToString();
                dd_originEC.ClearSelection();
                foreach (ListItem item in dd_originEC.Items)
                {
                    if (dr["OriginEC"].ToString() == item.Value)
                    {
                        item.Selected = true;
                        break;
                    }
                }
                //dd_originEC.SelectedValue = dr["OriginEC"].ToString();
                dd_StatusCode.ClearSelection();
                foreach (ListItem item in dd_StatusCode.Items)
                {
                    if (dr["StatusCode"].ToString().Trim() == item.Value)
                    {
                        item.Selected = true;
                        hd_statusCode.Value = item.Value;
                        //dd_StatusCode.Enabled = false;
                        break;
                    }
                }
                //dd_StatusCode.SelectedValue = dr["StatusCode"].ToString();
                dd_codType.ClearSelection();
                foreach (ListItem item in dd_codType.Items)
                {
                    if (dr["CODType"].ToString() == item.Value)
                    {
                        item.Selected = true;
                        break;
                    }
                }
                //dd_codType.SelectedValue = dr["CODType"].ToString();
                txt_codMonthlyAmount.Text = dr["MonthlyFixCharges"].ToString();
                if (dr["IsSmsServiceActive"].ToString() == "1")
                {
                    //chk_active_sms_service.Checked = true;
                }
                else
                {
                    //chk_active_sms_service.Checked = false;
                }
                txt_RnRWeight.Text = dr["RNR_weight"].ToString();
                if (txt_RnRWeight.Text != "")
                {
                    txt_RnRWeight.Enabled = false;
                }
                else
                {
                    txt_RnRWeight.Text = "5";
                    txt_RnRWeight.Enabled = true;
                }

                if (dd_parent.SelectedValue == "0")
                {
                    dd_parent.Enabled = true;
                }
                else
                {
                    dd_parent.Enabled = false;
                }

                if (ds.Tables[0].Rows[0]["ISCOD"].ToString().ToUpper() == "TRUE")
                {
                    chk_NotCod.Checked = true;
                }
                else
                {
                    chk_NotCod.Checked = false;
                }
                if (ds.Tables[0].Rows[0]["CentralizedClient"].ToString().ToUpper() == "TRUE")
                {
                    chk_centralisedClient.Checked = true;
                }
                else
                {
                    chk_centralisedClient.Checked = false;
                }

                if (ds.Tables[0].Rows[0]["CreditClientType"].ToString().ToUpper() == "TRUE")
                {
                    rbtn_customerType.SelectedValue = "1";
                }
                else
                {
                    rbtn_customerType.SelectedValue = "0";
                }

                if (HttpContext.Current.Session["Profile"].ToString() == "16")
                {
                    dd_StatusCode.Enabled = true;
                    //rbtn_Status.Enabled = true;
                    //rbtn_Status.Items.FindByValue("2").Enabled = false;
                    chk_NotCod.Enabled = true;
                    if (chk_NotCod.Checked)
                    {
                        dd_codType.Enabled = true;
                    }
                    else
                    {
                        dd_codType.Enabled = false;
                    }
                    dd_fafType.Enabled = true;
                    txt_RnRWeight.Enabled = true;
                    //rbtn_codMonthlyBillingAmount.Enabled = true;
                    //txt_codMonthlyAmount.Enabled = true;
                }
                else
                {
                    dd_StatusCode.Enabled = false;
                    //rbtn_Status.Enabled = false;
                    //rbtn_Status.Items.FindByValue("2").Enabled = false;

                    chk_NotCod.Enabled = true;
                    dd_codType.Enabled = false;
                    if (dd_fafType.SelectedValue == "0")
                    {
                        dd_fafType.Enabled = true;
                    }
                    else
                    {
                        dd_fafType.Enabled = false;
                    }
                    float temp = 0;
                    float.TryParse(txt_RnRWeight.Text, out temp);
                    if (txt_RnRWeight.Text.Trim() == "" || temp <= 0)
                    {
                        txt_RnRWeight.Enabled = true;
                    }
                    else
                    {
                        txt_RnRWeight.Enabled = false;
                    }
                }

                if (ds.Tables["AllocatedServices"].Rows.Count > 0)
                {
                    bool servicesFound = false;
                    chkl_services.ClearSelection();
                    foreach (DataRow sRow in ds.Tables["AllocatedServices"].Rows)
                    {

                        ListItem service = chkl_services.Items.Cast<ListItem>().First(r => String.Equals(r.Text, sRow["ServiceTypeName"].ToString(), StringComparison.InvariantCultureIgnoreCase));
                        if (service != null)
                        {
                            servicesFound = true;
                            service.Selected = true;
                        }
                    }
                    if (servicesFound && HttpContext.Current.Session["Profile"].ToString() != "16")
                    {
                        chkl_services.Enabled = false;
                    }
                }
                try
                {
                    #region Please Consult before Changing these lines of codes. I am not proud to have written these

                    /*
                    DataSet tariff = GetClientTariffTemp(hd_creditClientID.Value);

                    if (tariff.Tables.Count > 0)
                    {
                        if (tariff.Tables["DomesticTariff"].Rows.Count > 0)
                        {
                            DataTable dtt = tariff.Tables["DomesticTariff"];

                            for (int i = 2; i < tbl_domesticTariff.Rows.Count; i++)
                            {
                                string weightSlab = tbl_domesticTariff.Rows[i].Cells[0].InnerHtml;
                                weightSlab = weightSlab.Replace("\r\n", string.Empty);
                                weightSlab = weightSlab.Trim(' ');
                                DataRow dtr = dtt.Select("WeightSlab = '" + weightSlab + "'").FirstOrDefault();


                                for (int j = 1; j < tbl_domesticTariff.Rows[2].Cells.Count; j++)
                                {
                                    string controlID = i.ToString() + j.ToString();
                                    if (dtr != null)
                                    {
                                        (tbl_domesticTariff.Rows[i].Cells[j].FindControl("t" + controlID) as TextBox).Text = dtr[j + 1].ToString();
                                    }

                                }


                            }
                        }
                        else
                        {
                            for (int i = 2; i < tbl_domesticTariff.Rows.Count; i++)
                            {
                                string weightSlab = tbl_domesticTariff.Rows[i].Cells[0].InnerHtml;
                                weightSlab = weightSlab.Replace("\r\n", string.Empty);
                                weightSlab = weightSlab.Trim(' ');
                                //DataRow dtr = dtt.Select("WeightSlab = '" + weightSlab + "'").FirstOrDefault();


                                for (int j = 1; j < tbl_domesticTariff.Rows[2].Cells.Count; j++)
                                {
                                    string controlID = i.ToString() + j.ToString();

                                    (tbl_domesticTariff.Rows[i].Cells[j].FindControl("t" + controlID) as TextBox).Text = "";
                                }


                            }
                        }

                        if (tariff.Tables["InternationalTariff"].Rows.Count > 0)
                        {
                            DataTable dit = tariff.Tables["InternationalTariff"];
                            for (int i = 4; i < tbl_international.Rows.Count; i++)
                            {

                                string weightSlab = tbl_international.Rows[i].Cells[0].InnerText;
                                weightSlab = weightSlab.Replace("\r\n", string.Empty);
                                weightSlab = weightSlab.Trim(' ');
                                string category = "";
                                if (i >= 4 && i <= 6)
                                {
                                    category = tbl_international.Rows[4].Cells[1].InnerText;
                                    category = category.Replace("\r\n", string.Empty);
                                    category = category.Trim(' ');

                                }
                                else
                                {
                                    category = tbl_international.Rows[7].Cells[1].InnerText;
                                    category = category.Replace("\r\n", string.Empty);
                                    category = category.Trim(' ');

                                }

                                DataRow dir = dit.Select("WeightSlab = '" + weightSlab + "' and Category = '" + category + "'").FirstOrDefault();
                                for (int j = 2; j < tbl_international.Rows[2].Cells.Count; j++)
                                {
                                    string controlID = i.ToString() + j.ToString();
                                    if (dir != null)
                                    {
                                        (tbl_international.Rows[i].Cells[j].FindControl("i_" + controlID) as TextBox).Text = dir[j + 1].ToString();
                                    }
                                }

                                //dit.Rows.Add(dr);
                            }
                        }
                        else
                        {
                            for (int i = 4; i < tbl_international.Rows.Count; i++)
                            {

                                string weightSlab = tbl_international.Rows[i].Cells[0].InnerText;
                                weightSlab = weightSlab.Replace("\r\n", string.Empty);
                                weightSlab = weightSlab.Trim(' ');
                                string category = "";
                                if (i >= 4 && i <= 6)
                                {
                                    category = tbl_international.Rows[4].Cells[1].InnerText;
                                    category = category.Replace("\r\n", string.Empty);
                                    category = category.Trim(' ');

                                }
                                else
                                {
                                    category = tbl_international.Rows[7].Cells[1].InnerText;
                                    category = category.Replace("\r\n", string.Empty);
                                    category = category.Trim(' ');

                                }

                                //DataRow dir = dit.Select("WeightSlab = '" + weightSlab + "' and Category = '" + category + "'").FirstOrDefault();
                                for (int j = 2; j < tbl_international.Rows[2].Cells.Count; j++)
                                {
                                    string controlID = i.ToString() + j.ToString();

                                    (tbl_international.Rows[i].Cells[j].FindControl("i_" + controlID) as TextBox).Text = "";

                                }

                                //dit.Rows.Add(dr);
                            }
                        }

                    }
                     */
                    #endregion
                }
                catch (Exception)
                {

                    throw;
                }
                //chk_centralisedClient.Enabled = false;
                chk_nationWide.Enabled = false;
                ViewState["UPDATE"] = "1";
            }
            else
            {
                chk_centralisedClient.Enabled = true;
                chk_nationWide.Enabled = true;

                ViewState["UPDATE"] = "0";
                txt_acc_no.Text = "";
                hd_creditClientID.Value = "0";

                #region MyRegion
                /*

                for (int i = 2; i < tbl_domesticTariff.Rows.Count; i++)
                {
                    string weightSlab = tbl_domesticTariff.Rows[i].Cells[0].InnerHtml;
                    weightSlab = weightSlab.Replace("\r\n", string.Empty);
                    weightSlab = weightSlab.Trim(' ');
                    //DataRow dtr = dtt.Select("WeightSlab = '" + weightSlab + "'").FirstOrDefault();


                    for (int j = 1; j < tbl_domesticTariff.Rows[2].Cells.Count; j++)
                    {
                        string controlID = i.ToString() + j.ToString();

                        (tbl_domesticTariff.Rows[i].Cells[j].FindControl("t" + controlID) as TextBox).Text = "";
                    }


                }

                for (int i = 4; i < tbl_international.Rows.Count; i++)
                {

                    string weightSlab = tbl_international.Rows[i].Cells[0].InnerText;
                    weightSlab = weightSlab.Replace("\r\n", string.Empty);
                    weightSlab = weightSlab.Trim(' ');
                    string category = "";
                    if (i >= 4 && i <= 6)
                    {
                        category = tbl_international.Rows[4].Cells[1].InnerText;
                        category = category.Replace("\r\n", string.Empty);
                        category = category.Trim(' ');

                    }
                    else
                    {
                        category = tbl_international.Rows[7].Cells[1].InnerText;
                        category = category.Replace("\r\n", string.Empty);
                        category = category.Trim(' ');

                    }

                    //DataRow dir = dit.Select("WeightSlab = '" + weightSlab + "' and Category = '" + category + "'").FirstOrDefault();
                    for (int j = 2; j < tbl_international.Rows[2].Cells.Count; j++)
                    {
                        string controlID = i.ToString() + j.ToString();

                        (tbl_international.Rows[i].Cells[j].FindControl("i_" + controlID) as TextBox).Text = "";

                    }

                    //dit.Rows.Add(dr);
                }

                */
                #endregion
            }
            ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc( 'BasicInfo');", true);
        }

        public DataSet GetAccountDetail(CL_Customer clvar)
        {

            string sqlString = "selecT CAST(cc.isactive as varchar) isactive_, cc.*, cg.name GroupName from CreditClients cc\n" +
                               "  left outer join ClientGroups cg\n" +
                               "    on cg.id = cc.clientGrpId\n" +
                               " where cc.accountNo = '" + clvar.AccountNo + "' and cc.branchCode = '" + dd_Branch.SelectedValue + "'";


            string sqlString1 = "select cc.name, cc.accountNo, cs.UserName, cs.StaffTypeId\n" +
            "  from CreditClients cc\n" +
            " inner join ClientStaff cs\n" +
            "    on cs.ClientId = cc.id\n" +
            " where cc.accountNo = '" + clvar.AccountNo + "'\n" +
            "   and cc.branchCode = '" + dd_Branch.SelectedValue + "'\n" +
            " order by 1";



            string sqlString2 = "select cpa.*, pm.name\n" +
            "  from ClientPriceModifierAssociation cpa\n" +
            " inner join CreditClients cc\n" +
            "    on cc.id = cpa.creditClientId\n" +
            " inner join PriceModifiers pm\n" +
            "    on pm.id = cpa.priceModifierId\n" +
            "   and cc.branchCode = '" + dd_Branch.SelectedValue + "'\n" +
            "   and cc.accountNo = '" + clvar.AccountNo + "'\n" +
            " order by sortOrder";

            string sqlString3 = "SELECT mp.* \n"
               + "FROM   MnP_Customer_ServiceMap mp \n"
               + "       INNER JOIN CreditClients cc \n"
               + "            ON  cc.id = mp.CreditClientID \n"
               + "WHERE  cc.branchCode = '" + dd_Branch.SelectedValue + "'\n"
               + "   and cc.accountNo = '" + clvar.AccountNo + "'\n"
               + "   and mp.status = '1'\n";

            DataSet ds = new DataSet();
            SqlConnection con = new SqlConnection(clvar.Strcon());

            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sqlString, con);
                sda.Fill(ds, "AccountDetail");
                sda = new SqlDataAdapter(sqlString1, con);
                sda.Fill(ds, "AssociatedStaff");
                sda = new SqlDataAdapter(sqlString2, con);
                sda.Fill(ds, "AssociatedPriceModifiers");
                sda = new SqlDataAdapter(sqlString3, con);
                sda.Fill(ds, "AllocatedServices");
            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return ds;

        }

        public DataSet GetTempAccountDetail(CL_Customer clvar)
        {

            string sqlString = "selecT CAST(cc.isactive as varchar) isactive_, cc.*, cg.name GroupName from Mnp_TempCreditClients cc\n" +
                               "  left outer join ClientGroups cg\n" +
                               "    on cg.id = cc.clientGrpId\n" +
                               " where cc.TempClientId = '" + clvar.AccountNo + "' and cc.branchCode = '" + dd_Branch.SelectedValue + "'";


            string sqlString1 = "select cc.name, cc.accountNo, cs.UserName, cs.StaffTypeId\n" +
            "  from Mnp_TempCreditClients cc\n" +
            " inner join Mnp_TempClientStaff cs\n" +
            "    on cs.TempClientId = cc.id\n" +
            " where cc.TempClientId = '" + clvar.AccountNo + "'\n" +
            "   and cc.branchCode = '" + dd_Branch.SelectedValue + "'\n" +
            " order by 1";



            string sqlString2 = "select cpa.*, pm.name\n" +
            "  from Mnp_TempClientPriceModifierAssociation cpa\n" +
            " inner join Mnp_TempCreditClients cc\n" +
            "    on cc.id = cpa.TempClientId\n" +
            " inner join PriceModifiers pm\n" + //----------------------
            "    on pm.id = cpa.priceModifierId\n" +
            "   and cc.branchCode = '" + dd_Branch.SelectedValue + "'\n" +
            "   and cc.TempClientId = '" + clvar.AccountNo + "'\n" +
            " order by sortOrder";

            string sqlString3 = "SELECT mp.* \n"
               + "FROM   MnP_TempCustomerServiceMap mp \n"
               + "       INNER JOIN Mnp_TempCreditClients cc \n"
               + "            ON  cc.id = mp.TempClientId \n"
               + "WHERE  cc.branchCode = '" + dd_Branch.SelectedValue + "'\n"
               + "   and cc.TempClientId = '" + clvar.AccountNo + "'\n"
               + "   and mp.status = '1'\n";

            DataSet ds = new DataSet();
            SqlConnection con = new SqlConnection(clvar.Strcon());

            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sqlString, con);
                sda.Fill(ds, "AccountDetail");
                sda = new SqlDataAdapter(sqlString1, con);
                sda.Fill(ds, "AssociatedStaff");
                sda = new SqlDataAdapter(sqlString2, con);
                sda.Fill(ds, "AssociatedPriceModifiers");
                sda = new SqlDataAdapter(sqlString3, con);
                sda.Fill(ds, "AllocatedServices");
            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return ds;

        }

        public DataTable GetClientGroups_()
        {
            string query = "select * from ClientGroups cg where cg.collectionCenter='" + dd_Branch.SelectedValue + "'";
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

        protected void ResetAll()
        {
            txt_designation.Text = "";
            txt_benAccNo.Text = "";
            txt_benName.Text = "";
            dd_benBank.ClearSelection();
            dd_fafType.ClearSelection();
            rbtn_codMonthlyBillingAmount.SelectedValue = "0";
            txt_acc_no.Text = "";
            txt_BM_desc.Text = "";
            //txt_clname_acNo.Text = "";
            txt_codMonthlyAmount.Text = "";
            txt_contactPerson.Text = "";
            txt_CreditLimit.Text = "0";
            txt_DiscountOnDocument.Text = "0";
            txt_DiscountOnDomestic.Text = "0";
            txt_DiscountOnSample.Text = "0";
            txt_DomesticAmount.Text = "0";
            txt_DomesticPackets.Text = "0";
            txt_DomesticTurnOver.Text = "0";
            txt_email.Text = "";
            txt_FaxNo.Text = "";
            //txt_Group_seach_div.Text = "";
            //txt_grp_name.Text = "";
            txt_InternationalAmount.Text = "0";
            txt_InternationalPackets.Text = "0";
            txt_InternationalTurnOver.Text = "0";
            txt_MailingAddress.Text = "";
            //txt_memo.Text = "";
            txt_name.Text = "";
            txt_ntnNo.Text = "";
            txt_OfficialAddress.Text = "";
            txt_PhoneNo.Text = "";
            txt_pickupInstruction.Text = "";
            txt_pmValue.Text = "";
            //txt_redeemWindow.Text = "0";
            txt_RegDate.Text = DateTime.Now.ToShortDateString();
            txt_RegEndDate.Text = "";
            txt_RnRWeight.Text = "5";
            txt_SalesTaxNo.Text = "";
            CODTypes();
            DataTable addedPriceModifiers = new DataTable();
            addedPriceModifiers.Columns.Add("ID");
            addedPriceModifiers.Columns.Add("Name");
            addedPriceModifiers.Columns.Add("CalculationValue");
            addedPriceModifiers.Columns.Add("CalculationBase");
            addedPriceModifiers.Columns.Add("Description");
            addedPriceModifiers.AcceptChanges();
            ViewState["addedPriceModifiers"] = addedPriceModifiers;
            gridview_tab4.DataSource = addedPriceModifiers;
            gridview_tab4.DataBind();

            //btn_SelectGroup.Visible = false;

            //btn_Search.Visible = false;
            //Search_Client_div.Visible = false;
            //Search_Group_DIV.Visible = false;


            //  Search_Group_DIV.Visible = false;
            //Search_Client_div.Visible = false;

            //SqlDataAdapter adapter = new SqlDataAdapter("select id ,name  from CreditClientCategories where status='1' ", con);
            //adapter.Fill(Categories);
            //ddl_Cat.DataSource = Categories;
            //ddl_Cat.DataTextField = "name";
            //ddl_Cat.DataValueField = "id";
            //ddl_Cat.DataBind();

            DataTable Staffs = new DataTable();
            Staffs.Columns.Add("StaffTypeID", typeof(string));
            Staffs.Columns.Add("StaffType", typeof(string));
            Staffs.Columns.Add("StaffMember", typeof(string));
            Staffs.Columns.Add("StaffID", typeof(string));
            Staffs.AcceptChanges();

            GetClientCategories();
            GetClientStatusCodes();
            GetClientSectors();
            GetClientIndusties();
            GetClientStaffTypes();
            GetSalesRoutes();
            GetRecoveryExpressCenter();
            GetOriginExpressCenters();
            GetPriceModifiers();
            //GetClientGroups();

            foreach (ListItem item in ddly_stafType.Items)
            {
                if (item.Value == "0")
                {
                    continue;
                }
                DataRow dr = Staffs.NewRow();
                dr["StaffTypeID"] = item.Value;
                dr["StaffType"] = item.Text;

                Staffs.Rows.Add(dr);
                Staffs.AcceptChanges();
            }
            gv_Staffs.DataSource = Staffs;
            gv_Staffs.DataBind();
            foreach (GridViewRow row in gv_Staffs.Rows)
            {
                clvar.StaffType = (row.FindControl("hd_StaffTypeID") as HiddenField).Value;
                DataTable dt = GetClientStaff_(clvar);
                DropDownList dd = row.FindControl("dd_gStaffMembers") as DropDownList;
                DataView dv = dt.AsDataView();
                dd.Items.Add(new ListItem { Text = "Select Staff", Value = "0" });
                dv.Sort = "username";
                dd.DataSource = dv;
                dd.DataTextField = "UserName";
                dd.DataValueField = "ID";
                dd.DataBind();

            }
            ViewState["Staffs"] = Staffs;
            ViewState["UPDATE"] = "0";

            dd_nationWideGroup.ClearSelection();
            dd_parentBranch.ClearSelection();
            chkList_Branches.ClearSelection();





            dd_StatusCode.SelectedValue = "AC";





            chk_nationWide.Checked = false;
            div2.Style.Add("display", "none");
            //div2.Style.Add("display", "none");
            //ScriptManager.RegisterStartupScript(Page, GetType(), "CallMyFunction", "openCity(event, 'BasicInfo')", false);
            txt_nationWideGroup.Text = "";
            chk_centralisedClient.Checked = false;
            ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc( 'BasicInfo');", true);
        }

        public void AlertMessage(string message)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + message + "')", true);
            Errorid.Text = message;
        }

        protected void btn_cancel_Click(object sender, EventArgs e)
        {
            ResetAll();
        }

        public DataSet Branch()
        {
            DataSet Ds_1 = new DataSet();

            try
            {
                string query = "SELECT b.branchCode, \n"
               + "       b.name     BranchName, b.ZoneCode \n"
               + "FROM   Branches                          b \n"
               + "where b.[status] ='1' \n"
               + "GROUP BY \n"
               + "       b.branchCode, \n"
               + "       b.name, b.ZoneCode  order by b.name ASC";



                string sqlString = "SELECT b.branchCode, b.name BranchName, b.ZoneCode, ec.expressCenterCode\n" +
                "  FROM Branches b\n" +
                " left outer join ExpressCenters ec\n" +
                "    on b.branchCode = ec.bid\n" +
                "   and ec.Main_EC = '1'\n" +
                " where b.[status] = '1'\n" +
                " order by 1 asc";



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
        public DataSet Branch(string zone)
        {
            DataSet Ds_1 = new DataSet();

            try
            {
                string query = "SELECT b.branchCode, \n"
               + "       b.name     BranchName, b.ZoneCode \n"
               + "FROM   Branches                          b \n"
               + "where b.[status] ='1' \n"
               + "GROUP BY \n"
               + "       b.branchCode, \n"
               + "       b.name, b.ZoneCode  order by b.name ASC";



                string sqlString = "SELECT b.branchCode, b.sname + ' - ' + b.name BranchName\n" +
                "  FROM Branches b\n" +
                " WHERE b.zoneCode = '" + zone + "' and b.status = '1' \n" +
                " ORDER BY 2 ASC \n";




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
        protected void btn_updateTariff_Click(object sender, EventArgs e)
        {
            //if (dd_zone.SelectedValue == "0")
            //{
            //    Alert("Select Zone");
            //    return;
            //}
            //if (dd_Branch.SelectedValue == "0")
            //{
            //    Alert("Select Branch");
            //    return;
            //}
            if (txt_acc_no.Text.Trim() == "" || hd_creditClientID.Value == "0")
            {
                Alert("Invalid Account Number");
                return;
            }
            else
            {
                string errorMessage = UpdateTariff(hd_creditClientID.Value);
                if (errorMessage == "OK")
                {
                    Alert("Tariff Saved");
                }
            }
        }
        protected string UpdateTariff(string clientID)
        {
            #region MyRegion
            string error = "";
            /*
            #region Please Consult before Changing these lines of codes. I am not proud to have written these
            DataTable dtt = new DataTable();
            dtt.Columns.Add("ClientID", typeof(Int64));
            dtt.Columns.Add("WeightSlab", typeof(string));
            dtt.Columns.Add("DomesticWithinCity", typeof(string));
            dtt.Columns.Add("DomesticSame", typeof(string));
            dtt.Columns.Add("DomesticDiff", typeof(string));
            dtt.Columns.Add("SpWithinCity", typeof(string));
            dtt.Columns.Add("SpCity2City", typeof(string));
            dtt.Columns.Add("secondDayCity2City", typeof(string));
            dtt.Columns.Add("RnRA", typeof(string));
            dtt.Columns.Add("RnRB", typeof(string));
            dtt.Columns.Add("RnRC", typeof(string));
            dtt.Columns.Add("RnRD", typeof(string));

            for (int i = 2; i < tbl_domesticTariff.Rows.Count; i++)
            {
                DataRow dtr = dtt.NewRow();
                string weightSlab = tbl_domesticTariff.Rows[i].Cells[0].InnerHtml;
                weightSlab = weightSlab.Replace("\r\n", string.Empty);
                weightSlab = weightSlab.Trim(' ');
                dtr[0] = hd_creditClientID.Value;
                dtr[1] = weightSlab;

                for (int j = 1; j < tbl_domesticTariff.Rows[2].Cells.Count; j++)
                {
                    string controlID = i.ToString() + j.ToString();
                    if ((tbl_domesticTariff.Rows[i].Cells[j].FindControl("t" + controlID) as TextBox).Text == "")
                    {
                        dtr[j + 1] = "0";
                    }
                    else
                    {
                        dtr[j + 1] = ((tbl_domesticTariff.Rows[i].Cells[j].FindControl("t" + controlID) as TextBox).Text);
                    }

                }

                dtt.Rows.Add(dtr);
            }


            DataTable dit = new DataTable();
            dit.Columns.AddRange(
                new DataColumn[]
                {
                    new DataColumn("ClientID"),
                    new DataColumn("weightSlab"),
                    new DataColumn("Category"),
                    new DataColumn("UAE"),
                    new DataColumn("MiddleEast"),
                    new DataColumn("UK"),
                    new DataColumn("SouthAsia"),
                    new DataColumn("WesternEurope"),
                    new DataColumn("AsiaPacific"),
                    new DataColumn("USA_CANADA"),
                    new DataColumn("EasternEurope"),
                    new DataColumn("Africa"),
                    new DataColumn("LatinAmerica")

                }
                );

            for (int i = 4; i < tbl_international.Rows.Count; i++)
            {
                DataRow dr = dit.NewRow();
                dr[0] = hd_creditClientID.Value;
                string weightSlab = tbl_international.Rows[i].Cells[0].InnerText;
                weightSlab = weightSlab.Replace("\r\n", string.Empty);
                weightSlab = weightSlab.Trim(' ');
                dr[1] = weightSlab;
                if (i >= 4 && i <= 6)
                {
                    string category = tbl_international.Rows[4].Cells[1].InnerText;
                    category = category.Replace("\r\n", string.Empty);
                    category = category.Trim(' ');
                    dr[2] = category;
                }
                else
                {
                    string category = tbl_international.Rows[7].Cells[1].InnerText;
                    category = category.Replace("\r\n", string.Empty);
                    category = category.Trim(' ');
                    dr[2] = category;
                }
                for (int j = 2; j < tbl_international.Rows[2].Cells.Count; j++)
                {
                    string controlID = i.ToString() + j.ToString();
                    if ((tbl_international.Rows[i].Cells[j].FindControl("i_" + controlID) as TextBox).Text == "")
                    {
                        dr[j + 1] = "0";
                    }
                    else
                    {
                        dr[j + 1] = ((tbl_international.Rows[i].Cells[j].FindControl("i_" + controlID) as TextBox).Text);
                    }
                }

                dit.Rows.Add(dr);
            }
            #endregion

            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "UPDATE_DOMESTICTARIFFTEMP";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@tariff", dtt);
                cmd.Parameters.AddWithValue("@intTariff", dit);
                cmd.Parameters.Add("@result", SqlDbType.VarChar, 300).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                object obj = cmd.Parameters["@result"].Value.ToString();
                error = obj.ToString();

            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            finally { con.Close(); }



             */
            return error;

            #endregion
        }
        public DataSet GetClientTariffTemp(string clientID)
        {
            string query = "SELECT * FROM DomesticTariffTemp where clientID = '" + clientID + "'";
            string query2 = "SELECT * FROM InternationalTariffTemp where clientID = '" + clientID + "'";

            DataSet ds = new DataSet();

            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                sda.Fill(ds, "DomesticTariff");
                sda = new SqlDataAdapter(query2, con);
                sda.Fill(ds, "InternationalTariff");
            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return ds;
        }
        protected void dd_zone_SelectedIndexChanged(object sender, EventArgs e)
        {
            dd_Branch.Items.Clear();
            dd_Branch.Items.Add(new ListItem { Text = "Select Branch", Value = "0" });
            DataSet branch = Branch(dd_zone.SelectedValue);
            if (branch.Tables[0].Rows.Count > 0)
            {

                dd_Branch.DataSource = branch.Tables[0];
                dd_Branch.DataTextField = "BranchName";
                dd_Branch.DataValueField = "BranchCode";
                dd_Branch.DataBind();

                dd_Branch.SelectedValue = "0";

                ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc( 'BasicInfo');", true);
            }
        }
        protected void dd_Branch_SelectedIndexChanged(object sender, EventArgs e)
        {
            //GetClientGroups();
            GetRecoveryExpressCenter();
            GetOriginExpressCenters();
            foreach (GridViewRow row in gv_Staffs.Rows)
            {
                clvar.StaffType = (row.FindControl("hd_StaffTypeID") as HiddenField).Value;
                DataTable dt = GetClientStaff_(clvar);
                DropDownList dd = row.FindControl("dd_gStaffMembers") as DropDownList;
                DataView dv = dt.AsDataView();
                dd.Items.Clear();
                dd.Items.Add(new ListItem { Text = "Select Staff", Value = "0" });
                dv.Sort = "username";
                dd.DataSource = dv;
                dd.DataTextField = "UserName";
                dd.DataValueField = "ID";
                dd.DataBind();
            }

            ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc( 'BasicInfo');", true);

        }
        public DataTable GetClientStaff(CL_Customer clvar)
        {

            string query = "select ua.username, ua.id\n" +
           "  from UserStaffType u\n" +
           " inner join UserAssociation ua\n" +
           "    on u.username = ua.username\n" +
           " where u.staffTypeId = '" + clvar.StaffType + "'\n" +
           "   and ua.branchCode = '" + dd_Branch.SelectedValue + "'";


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

        protected void txt_tempclient_no_TextChanged(object sender, EventArgs e)
        {
            if (dd_zone.SelectedValue == "0")
            {
                Alert("Select Zone");
                ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc( 'BasicInfo');", true);
                return;
            }
            if (dd_Branch.SelectedValue == "0")
            {
                Alert("Select Branch");
                ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc( 'BasicInfo');", true);
                return;
            }

            clvar.AccountNo = txt_tempclient_no.Text;

            DataSet ds = GetTempAccountDetail(clvar);

            clvar.RnRCharges = ds.Tables[0].Rows[0]["id"].ToString();

            DataTable addedModifiers = ViewState["addedPriceModifiers"] as DataTable;
            addedModifiers.Clear();

            DataTable addedStaff = ViewState["Staffs"] as DataTable;

            foreach (DataRow row in ds.Tables["AssociatedStaff"].Rows)
            {
                string staffType = row["StaffTypeID"].ToString();
                DataRow[] dr = addedStaff.Select("StaffTypeID = '" + staffType + "'");
                if (dr.Count() > 0)
                {
                    dr[0]["StaffMember"] = row["UserName"].ToString();
                }
            }
            gv_Staffs.DataSource = addedStaff;
            gv_Staffs.DataBind();

            foreach (GridViewRow row in gv_Staffs.Rows)
            {
                clvar.StaffType = (row.FindControl("hd_StaffTypeID") as HiddenField).Value;
                DataTable dt = GetClientStaff_(clvar);
                DropDownList dd = row.FindControl("dd_gStaffMembers") as DropDownList;
                DataView dv = dt.AsDataView();
                dd.Items.Add(new ListItem { Text = "Select Staff", Value = "0" });
                dv.Sort = "username";
                dd.DataSource = dv;
                dd.DataTextField = "UserName";
                dd.DataValueField = "ID";
                dd.DataBind();
                //DataRow[] dr = addedStaff.Select("StaffTypeID = '" + staffType + "'");
                DataRow[] dr = addedStaff.Select("StaffTypeID = '" + clvar.StaffType + "'");
                if (dr.Count() > 0)
                {
                    if (dr[0]["StaffMember"].ToString().Trim() != "")
                    {
                        dd.SelectedItem.Text = dr[0]["StaffMember"].ToString();
                    }
                }
            }

            // ---------- For Tariff Start ---------- 
            addTariff_dt = ViewState["dt_tariff2"] as DataTable;
            //  addTariff_dt.Clear();

            clvar.Memo = ds.Tables[0].Rows[0]["id"].ToString();
            DataSet get_tariff_dt = Get_Tariff(clvar);

            if (get_tariff_dt.Tables[0].Rows.Count > 0)
            {
                // ServiceTypeName = get_tariff_dt.Tables[0].Rows[0]["id"].ToString();

                Session["dt_"] = get_tariff_dt.Tables[0];
                ViewState["dt_tariff2"] = get_tariff_dt.Tables[0];
                gv_tariff.DataSource = get_tariff_dt.Tables[0].DefaultView;
                gv_tariff.DataBind();

                //foreach (GridViewRow row in gv_tariff.Rows)
                //{
                //    //check box is the first control on the last cell.
                //    CheckBox check = get_tariff_dt.Tables[0].Rows[0]["isPR"] as CheckBox;//row.Cells[0].Controls[0] as CheckBox;


                //            CheckBox chkRow = (row.Cells[0].FindControl("cb_pr") as CheckBox);
                //    if (check != null)
                //    {
                //        check.Enabled = true;
                //        //row.Cells[0].FindControl("cb_pr") = true;
                //    }
                //    else
                //    {
                //        //check.Enabled = false;
                //    }


                //    /*
                //    CheckBox chkRow = (row.Cells[0].FindControl("cb_pr") as CheckBox);

                //    if (chkRow.Checked)
                //    {
                //        chkRow.Enabled = true;
                //    }
                //    else
                //    {

                //    }
                //     */ 
                //}

                btn_tariffupdate.Visible = true;
            }
            // ---------- For Tariff Start ---------- 



            foreach (DataRow row in ds.Tables[2].Rows)
            {
                DataRow dr = addedModifiers.NewRow();
                dr["ID"] = row["priceModifierID"].ToString();
                dr["NAme"] = row["name"].ToString();
                dr["CalculationValue"] = row["ModifiedCalculationValue"].ToString();
                dr["CalculationBase"] = row["CalculationBase"].ToString();
                dr["Description"] = "";

                addedModifiers.Rows.Add(dr);
            }

            gridview_tab4.DataSource = addedModifiers;
            gridview_tab4.DataBind();

            if (ds.Tables[0].Rows.Count > 0)
            {
                chk_NotCod.Enabled = false;
                dd_codType.Enabled = false;
                rbtn_printStatus.Enabled = false;
                txt_RegDate.Enabled = false;
                chk_special_cust.Enabled = false;

                DataRow dr = ds.Tables[0].Rows[0];

                if (dr["isMinBilling"].ToString() == "1" || dr["isMinBilling"].ToString().ToUpper() == "TRUE")
                {
                    rbtn_codMonthlyBillingAmount.SelectedValue = "1";
                }
                else
                {
                    rbtn_codMonthlyBillingAmount.SelectedValue = "0";
                }

                if (dr["FAFType"].ToString().Trim() == "")
                {
                    dd_fafType.SelectedIndex = 0;
                    dd_fafType.Enabled = true;
                }
                else if (dr["FAFType"].ToString() == "1" || dr["FAFType"].ToString().ToUpper() == "TRUE")
                {
                    dd_fafType.SelectedValue = "1";
                    dd_fafType.Enabled = false;
                }
                else
                {
                    dd_fafType.SelectedValue = "0";
                    dd_fafType.Enabled = false;
                }

                txt_benName.Text = dr["BeneficiaryName"].ToString();
                txt_benAccNo.Text = dr["BeneficiaryBankAccNo"].ToString();

                ListItem bank = dd_benBank.Items.FindByValue(dr["BeneficiaryBankCode"].ToString());
                if (bank != null)
                {
                    dd_benBank.ClearSelection();
                    bank.Selected = true;
                }

                hd_creditClientID.Value = dr["id"].ToString();
                hd_tempClientID.Value = dr["id"].ToString();

                txt_name.Text = dr["name"].ToString();
                txt_contactPerson.Text = dr["ContactPerson"].ToString();
                txt_PhoneNo.Text = dr["PhoneNo"].ToString();
                txt_FaxNo.Text = dr["faxNo"].ToString();
                txt_email.Text = dr["email"].ToString();
                txt_OfficialAddress.Text = dr["address"].ToString();
                if (dr["isNationWide"].ToString() == "1" || dr["isNationWide"].ToString().ToUpper() == "TRUE")
                {
                    chk_centralisedClient.Checked = true;
                    chk_nationWide.Checked = true;

                    DataTable centeralizedAccBrs = GetBranchesOfCenteralizedAccount(clvar.AccountNo);
                    txt_nationWideAccountNo.Text = clvar.AccountNo;
                    txt_nationWideGroup.Text = dr["ClientGrpID"].ToString();
                    if (centeralizedAccBrs != null)
                    {
                        if (centeralizedAccBrs.Rows.Count > 0)
                        {
                            foreach (DataRow row in centeralizedAccBrs.Rows)
                            {
                                if (chkList_Branches.Items.FindByValue(row["BranchCode"].ToString()) != null)
                                {
                                    chkList_Branches.Items.FindByValue(row["BranchCode"].ToString()).Selected = true;

                                }
                                if (row["isParent"].ToString() == "1" || row["isParent"].ToString().ToUpper() == "TRUE")
                                {
                                    dd_parentBranch.SelectedValue = row["BranchCode"].ToString();
                                }
                            }
                        }
                        else
                        {
                            AlertMessage("Branches Not found for this Account");
                            return;
                        }
                    }
                    else
                    {
                        AlertMessage("Branches Not found for this Account");
                        return;
                    }

                }
                else if (ds.Tables[0].Rows[0]["CentralizedClient"].ToString() == "1" || ds.Tables[0].Rows[0]["CentralizedClient"].ToString().ToUpper() == "TRUE")
                {
                    chk_centralisedClient.Checked = true;

                    txt_nationWideGroup.Text = ds.Tables[0].Rows[0]["clientGrpId"].ToString();
                    txt_nationWideGroup.Enabled = true;
                    txt_nationWideGroup.Attributes.Add("disabled", "false");
                }
                else
                {
                    chk_centralisedClient.Checked = false;
                    txt_nationWideGroup.Enabled = false;
                    txt_nationWideGroup.Attributes.Add("disabled", "true");
                }

                txt_RegDate.Text = dr["regDate"].ToString();
                txt_RegEndDate.Text = dr["regEndDate"].ToString();
                txt_pickupInstruction.Text = dr["pickUpInstruction"].ToString();
                txt_DomesticTurnOver.Text = dr["domesticAMonTo"].ToString();
                txt_InternationalTurnOver.Text = dr["internationalAMonTo"].ToString();
                txt_DomesticPackets.Text = dr["domesticPackets"].ToString();
                txt_InternationalPackets.Text = dr["internationalPackets"].ToString();
                txt_DomesticAmount.Text = dr["domesticAmount"].ToString();
                txt_InternationalAmount.Text = dr["internationalAmount"].ToString();
                rbtn_Status.SelectedValue = dr["isactive_"].ToString();
                rbtn_printStatus.SelectedValue = dr["printingStatus"].ToString();
                rbtn_billingMode.SelectedValue = dr["billingMode"].ToString();
                txt_DiscountOnDomestic.Text = dr["discountOnDomestic"].ToString();
                txt_DiscountOnSample.Text = dr["discountOnSample"].ToString();
                txt_DiscountOnDocument.Text = dr["discountOnDocument"].ToString();
                dd_prepareBill.SelectedValue = dr["prepareBillType"].ToString();
                txt_CreditLimit.Text = dr["creditLimit"].ToString();
                txt_SalesTaxNo.Text = dr["salesTaxNo"].ToString();
                //txt_memo.Text = dr["memo"].ToString();
                rbtn_BillTaxType.SelectedValue = dr["billTaxType"].ToString();
                //dd_CustomerCategory.SelectedValue = dr["catId"].ToString();
                txt_designation.Text = dr["ContactPersonDesignation"].ToString();


                if (dr["ClientGrpID"].ToString() != "")
                {
                    //dd_clientGroups.SelectedValue = dr["clientGrpId"].ToString();

                    //lbl_groupName.Text = dr["GroupName"].ToString();
                    //txt_clientGroups.Text = dr["clientGrpID"].ToString();
                }
                dd_recoveryExpID.ClearSelection();
                foreach (ListItem item in dd_recoveryExpID.Items)
                {
                    if (dr["recoveryexpcenid"].ToString() == item.Value)
                    {
                        item.Selected = true;
                        break;
                    }
                }
                //dd_recoveryExpID.SelectedValue = dr["recoveryExpCenId"].ToString();
                //dd_SalesRoute.ClearSelection();
                //foreach (ListItem item in dd_SalesRoute.Items)
                //{
                //    if (dr["salesRouteId"].ToString() == item.Value)
                //    {
                //        item.Selected = true;
                //        break;
                //    }
                //}
                //dd_SalesRoute.SelectedValue = dr["salesRouteId"].ToString();
                //            txt_redeemWindow.Text = dr["redeemWindow"].ToString();
                //dd_Sector.ClearSelection();
                //foreach (ListItem item in dd_Sector.Items)
                //{
                //    if (dr["SectorId"].ToString() == item.Value)
                //    {
                //        item.Selected = true;
                //        break;
                //    }
                //}
                //dd_Sector.SelectedValue = dr["SectorId"].ToString();
                dd_Industry.ClearSelection();
                foreach (ListItem item in dd_Industry.Items)
                {
                    if (dr["IndustryId"].ToString() == item.Value)
                    {
                        item.Selected = true;
                        break;
                    }
                }
                //dd_Industry.SelectedValue = dr["IndustryId"].ToString();
                txt_acc_no.Text = dr["accountNo"].ToString();
                rbtn_customerType.ClearSelection();
                foreach (ListItem item in rbtn_customerType.Items)
                {
                    if (dr["creditClientType"].ToString() == item.Value)
                    {
                        item.Selected = true;
                        break;
                    }
                }
                //rbtn_customerType.SelectedValue = dr["creditClientType"].ToString();
                txt_ntnNo.Text = dr["ntnNo"].ToString();
                if (dr["IsCOD"].ToString().ToLower() == "true")
                {
                    chk_NotCod.Checked = true;
                }
                else
                {
                    chk_NotCod.Checked = false;
                }
                if (dr["IsSpecial"].ToString() == "1")
                {
                    chk_special_cust.Checked = true;
                }
                else
                {
                    chk_special_cust.Checked = false;
                }
                if (dr["isFranchisee"].ToString() == "1")
                {

                }
                txt_MailingAddress.Text = dr["MailingAddress"].ToString();
                dd_originEC.ClearSelection();
                foreach (ListItem item in dd_originEC.Items)
                {
                    if (dr["OriginEC"].ToString() == item.Value)
                    {
                        item.Selected = true;
                        break;
                    }
                }
                //dd_originEC.SelectedValue = dr["OriginEC"].ToString();
                dd_StatusCode.ClearSelection();
                foreach (ListItem item in dd_StatusCode.Items)
                {
                    if (dr["StatusCode"].ToString().Trim() == item.Value)
                    {
                        item.Selected = true;
                        hd_statusCode.Value = item.Value;
                        //dd_StatusCode.Enabled = false;
                        break;
                    }
                }
                //dd_StatusCode.SelectedValue = dr["StatusCode"].ToString();
                dd_codType.ClearSelection();
                foreach (ListItem item in dd_codType.Items)
                {
                    if (dr["CODType"].ToString() == item.Value)
                    {
                        item.Selected = true;
                        break;
                    }
                }
                //dd_codType.SelectedValue = dr["CODType"].ToString();
                txt_codMonthlyAmount.Text = dr["MonthlyFixCharges"].ToString();
                if (dr["IsSmsServiceActive"].ToString() == "1")
                {
                    //chk_active_sms_service.Checked = true;
                }
                else
                {
                    //chk_active_sms_service.Checked = false;
                }
                txt_RnRWeight.Text = dr["RNR_weight"].ToString();
                if (txt_RnRWeight.Text != "")
                {
                    txt_RnRWeight.Enabled = false;
                }
                else
                {
                    txt_RnRWeight.Text = "5";
                    txt_RnRWeight.Enabled = true;
                }

                if (dd_parent.SelectedValue == "0")
                {
                    dd_parent.Enabled = true;
                }
                else
                {
                    dd_parent.Enabled = false;
                }

                if (ds.Tables[0].Rows[0]["ISCOD"].ToString().ToUpper() == "TRUE")
                {
                    chk_NotCod.Checked = true;
                }
                else
                {
                    chk_NotCod.Checked = false;
                }
                if (ds.Tables[0].Rows[0]["CentralizedClient"].ToString().ToUpper() == "TRUE")
                {
                    chk_centralisedClient.Checked = true;
                }
                else
                {
                    chk_centralisedClient.Checked = false;
                }

                if (ds.Tables[0].Rows[0]["CreditClientType"].ToString().ToUpper() == "TRUE")
                {
                    rbtn_customerType.SelectedValue = "1";
                }
                else
                {
                    rbtn_customerType.SelectedValue = "0";
                }

                if (HttpContext.Current.Session["Profile"].ToString() == "16")
                {
                    dd_StatusCode.Enabled = true;
                    //rbtn_Status.Enabled = true;
                    //rbtn_Status.Items.FindByValue("2").Enabled = false;
                    chk_NotCod.Enabled = true;
                    if (chk_NotCod.Checked)
                    {
                        dd_codType.Enabled = true;
                    }
                    else
                    {
                        dd_codType.Enabled = false;
                    }
                    dd_fafType.Enabled = true;
                    txt_RnRWeight.Enabled = true;
                    //rbtn_codMonthlyBillingAmount.Enabled = true;
                    //txt_codMonthlyAmount.Enabled = true;
                }
                else
                {
                    dd_StatusCode.Enabled = false;
                    //rbtn_Status.Enabled = false;
                    //rbtn_Status.Items.FindByValue("2").Enabled = false;

                    chk_NotCod.Enabled = true;
                    dd_codType.Enabled = false;
                    if (dd_fafType.SelectedValue == "0")
                    {
                        dd_fafType.Enabled = true;
                    }
                    else
                    {
                        dd_fafType.Enabled = false;
                    }
                    float temp = 0;
                    float.TryParse(txt_RnRWeight.Text, out temp);
                    if (txt_RnRWeight.Text.Trim() == "" || temp <= 0)
                    {
                        txt_RnRWeight.Enabled = true;
                    }
                    else
                    {
                        txt_RnRWeight.Enabled = false;
                    }
                }

                if (ds.Tables["AllocatedServices"].Rows.Count > 0)
                {
                    bool servicesFound = false;
                    chkl_services.ClearSelection();
                    foreach (DataRow sRow in ds.Tables["AllocatedServices"].Rows)
                    {

                        ListItem service = chkl_services.Items.Cast<ListItem>().First(r => String.Equals(r.Text, sRow["ServiceTypeName"].ToString(), StringComparison.InvariantCultureIgnoreCase));
                        if (service != null)
                        {
                            servicesFound = true;
                            service.Selected = true;
                        }
                    }
                    if (servicesFound && HttpContext.Current.Session["Profile"].ToString() != "16")
                    {
                        // chkl_services.Enabled = false;
                    }
                }
                try
                {
                    #region Please Consult before Changing these lines of codes. I am not proud to have written these

                    /*
                    DataSet tariff = GetClientTariffTemp(hd_creditClientID.Value);

                    if (tariff.Tables.Count > 0)
                    {
                        if (tariff.Tables["DomesticTariff"].Rows.Count > 0)
                        {
                            DataTable dtt = tariff.Tables["DomesticTariff"];

                            for (int i = 2; i < tbl_domesticTariff.Rows.Count; i++)
                            {
                                string weightSlab = tbl_domesticTariff.Rows[i].Cells[0].InnerHtml;
                                weightSlab = weightSlab.Replace("\r\n", string.Empty);
                                weightSlab = weightSlab.Trim(' ');
                                DataRow dtr = dtt.Select("WeightSlab = '" + weightSlab + "'").FirstOrDefault();


                                for (int j = 1; j < tbl_domesticTariff.Rows[2].Cells.Count; j++)
                                {
                                    string controlID = i.ToString() + j.ToString();
                                    if (dtr != null)
                                    {
                                        (tbl_domesticTariff.Rows[i].Cells[j].FindControl("t" + controlID) as TextBox).Text = dtr[j + 1].ToString();
                                    }

                                }


                            }
                        }
                        else
                        {
                            for (int i = 2; i < tbl_domesticTariff.Rows.Count; i++)
                            {
                                string weightSlab = tbl_domesticTariff.Rows[i].Cells[0].InnerHtml;
                                weightSlab = weightSlab.Replace("\r\n", string.Empty);
                                weightSlab = weightSlab.Trim(' ');
                                //DataRow dtr = dtt.Select("WeightSlab = '" + weightSlab + "'").FirstOrDefault();


                                for (int j = 1; j < tbl_domesticTariff.Rows[2].Cells.Count; j++)
                                {
                                    string controlID = i.ToString() + j.ToString();

                                    (tbl_domesticTariff.Rows[i].Cells[j].FindControl("t" + controlID) as TextBox).Text = "";
                                }


                            }
                        }

                        if (tariff.Tables["InternationalTariff"].Rows.Count > 0)
                        {
                            DataTable dit = tariff.Tables["InternationalTariff"];
                            for (int i = 4; i < tbl_international.Rows.Count; i++)
                            {

                                string weightSlab = tbl_international.Rows[i].Cells[0].InnerText;
                                weightSlab = weightSlab.Replace("\r\n", string.Empty);
                                weightSlab = weightSlab.Trim(' ');
                                string category = "";
                                if (i >= 4 && i <= 6)
                                {
                                    category = tbl_international.Rows[4].Cells[1].InnerText;
                                    category = category.Replace("\r\n", string.Empty);
                                    category = category.Trim(' ');

                                }
                                else
                                {
                                    category = tbl_international.Rows[7].Cells[1].InnerText;
                                    category = category.Replace("\r\n", string.Empty);
                                    category = category.Trim(' ');

                                }

                                DataRow dir = dit.Select("WeightSlab = '" + weightSlab + "' and Category = '" + category + "'").FirstOrDefault();
                                for (int j = 2; j < tbl_international.Rows[2].Cells.Count; j++)
                                {
                                    string controlID = i.ToString() + j.ToString();
                                    if (dir != null)
                                    {
                                        (tbl_international.Rows[i].Cells[j].FindControl("i_" + controlID) as TextBox).Text = dir[j + 1].ToString();
                                    }
                                }

                                //dit.Rows.Add(dr);
                            }
                        }
                        else
                        {
                            for (int i = 4; i < tbl_international.Rows.Count; i++)
                            {

                                string weightSlab = tbl_international.Rows[i].Cells[0].InnerText;
                                weightSlab = weightSlab.Replace("\r\n", string.Empty);
                                weightSlab = weightSlab.Trim(' ');
                                string category = "";
                                if (i >= 4 && i <= 6)
                                {
                                    category = tbl_international.Rows[4].Cells[1].InnerText;
                                    category = category.Replace("\r\n", string.Empty);
                                    category = category.Trim(' ');

                                }
                                else
                                {
                                    category = tbl_international.Rows[7].Cells[1].InnerText;
                                    category = category.Replace("\r\n", string.Empty);
                                    category = category.Trim(' ');

                                }

                                //DataRow dir = dit.Select("WeightSlab = '" + weightSlab + "' and Category = '" + category + "'").FirstOrDefault();
                                for (int j = 2; j < tbl_international.Rows[2].Cells.Count; j++)
                                {
                                    string controlID = i.ToString() + j.ToString();

                                    (tbl_international.Rows[i].Cells[j].FindControl("i_" + controlID) as TextBox).Text = "";

                                }

                                //dit.Rows.Add(dr);
                            }
                        }

                    }
                     */
                    #endregion
                }
                catch (Exception)
                {

                    throw;
                }
                //chk_centralisedClient.Enabled = false;
                chk_nationWide.Enabled = false;
                ViewState["UPDATE"] = "1";
            }
            else
            {
                chk_centralisedClient.Enabled = true;
                chk_nationWide.Enabled = true;

                ViewState["UPDATE"] = "0";
                txt_acc_no.Text = "";
                hd_creditClientID.Value = "0";

                #region MyRegion
                /*

                for (int i = 2; i < tbl_domesticTariff.Rows.Count; i++)
                {
                    string weightSlab = tbl_domesticTariff.Rows[i].Cells[0].InnerHtml;
                    weightSlab = weightSlab.Replace("\r\n", string.Empty);
                    weightSlab = weightSlab.Trim(' ');
                    //DataRow dtr = dtt.Select("WeightSlab = '" + weightSlab + "'").FirstOrDefault();


                    for (int j = 1; j < tbl_domesticTariff.Rows[2].Cells.Count; j++)
                    {
                        string controlID = i.ToString() + j.ToString();

                        (tbl_domesticTariff.Rows[i].Cells[j].FindControl("t" + controlID) as TextBox).Text = "";
                    }


                }

                for (int i = 4; i < tbl_international.Rows.Count; i++)
                {

                    string weightSlab = tbl_international.Rows[i].Cells[0].InnerText;
                    weightSlab = weightSlab.Replace("\r\n", string.Empty);
                    weightSlab = weightSlab.Trim(' ');
                    string category = "";
                    if (i >= 4 && i <= 6)
                    {
                        category = tbl_international.Rows[4].Cells[1].InnerText;
                        category = category.Replace("\r\n", string.Empty);
                        category = category.Trim(' ');

                    }
                    else
                    {
                        category = tbl_international.Rows[7].Cells[1].InnerText;
                        category = category.Replace("\r\n", string.Empty);
                        category = category.Trim(' ');

                    }

                    //DataRow dir = dit.Select("WeightSlab = '" + weightSlab + "' and Category = '" + category + "'").FirstOrDefault();
                    for (int j = 2; j < tbl_international.Rows[2].Cells.Count; j++)
                    {
                        string controlID = i.ToString() + j.ToString();

                        (tbl_international.Rows[i].Cells[j].FindControl("i_" + controlID) as TextBox).Text = "";

                    }

                    //dit.Rows.Add(dr);
                }

                */
                #endregion
            }
            ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc( 'BasicInfo');", true);
        }

        public DataTable Get_Staff_Tariff(CL_Customer clvar)
        {
            string sqlString = "SELECT * FROM MNP_Matrix_Tariff t where t.StaffLevelId = '" + clvar.Memo + "' and status ='1' ";

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

        public DataSet Get_Tariff(CL_Customer clvar)
        {
            DataSet Ds_1 = new DataSet();

            try
            {
                string query = "SELECT 0 isUPDATED, t.AdditionalWeight addFactor, \n" +
                               "case when t.ispr = '1' then 'true' else 'false' end cb_pr, * \n" +
                               "FROM  MnP_TempTariff t WHERE t.TempClientId = '" + clvar.Memo + "' AND t.[Status] = '1'";

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

        protected void txtPhoneNo_TextChanged(object sender, EventArgs e)
        {

            string s = txt_PhoneNo.Text;
            string s_ = s.Substring(0, 1);

            if (s.Length == 11)
            {
                string value;
                value = new System.Text.RegularExpressions.Regex(@"\D").Replace(txt_PhoneNo.Text, string.Empty);

                if (s_.ToString() == "0")
                {
                    txt_PhoneNo.Text = Convert.ToInt64(value).ToString("0###-#######");
                }
                else
                {
                    txt_PhoneNo.Text = Convert.ToInt64(value).ToString("###-#######");
                }

            }
            txt_designation.Focus();
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "active", "openCity(event,'BasicInfo')", true);
        }

        protected void txt_ntnNo_TextChanged(object sender, EventArgs e)
        {
            string s = txt_ntnNo.Text;

            if (s.Length == 8)
            {
                string value;
                value = new System.Text.RegularExpressions.Regex(@"\D").Replace(s, string.Empty);

                txt_ntnNo.Text = Convert.ToInt64(value).ToString("#######-#");
            }
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "active", "openCity(event,'billingNsales')", true);
        }
    }
}