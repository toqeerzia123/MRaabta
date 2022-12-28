using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using MRaabta.App_Code;
using System.Security.Cryptography;
using System.IO;
using System.Text;

namespace MRaabta.Files
{
    public partial class PettyCash_IOU : System.Web.UI.Page
    {
        #region Variable

        Variable clvar = new Variable();
        bayer_Function b_fun = new bayer_Function();

        string year = "";
        string month = "";
        string day = "";
        string report, zone, account, catch_error_msg;
        string start_date = "", consignment_number = "";
        string reportid = "312";

        //decimal totalamount = 0M;

        string query;

        SqlCommand sqlcommand = new SqlCommand();
        DataSet dataset;

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                if (RBL_ReportCategory.SelectedValue == "Date")    //  When User Select Date Option
                {
                    txtStartDate.Text = "";
                    txtEndDate.Text = "";

                    ListBoxZones.SelectedValue = null;
                    ListBoxBranches.SelectedValue = null;

                    Td_IOU_EC.Visible = false;
                    Td_Date.Visible = true;
                    Td_Zone.Visible = true;
                    //Td_Branch.Visible = true;
                    //Td_FlagType.Visible = true;

                    lblIOUNO.Visible = false;
                    txtIOUONO.Visible = false;

                    lblEC.Visible = false;
                    txtEC.Visible = false;
                }

                string Session_Zone = HttpContext.Current.Session["ZONECODE"].ToString();

                string Session_Branch = HttpContext.Current.Session["BRANCHCODE"].ToString();

                //if (Session_Zone == "ALL")
                //{
                //    CheckBoxZonesAll.Visible = true;
                //    LabelCheckZone.Visible = true;
                //}
                //else if (Session_Zone != "ALL" && Session_Zone.Contains(','))
                //{
                //    int TotalZoneCount = GetAllZoneCount(clvar);

                //    string[] Zones = Session_Zone.Split(new[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);

                //    if (Zones.Length - 1 == TotalZoneCount)
                //    {
                //        CheckBoxZonesAll.Visible = true;
                //        LabelCheckZone.Visible = true;
                //    }
                //    else
                //    {
                //        CheckBoxZonesAll.Visible = false;
                //        LabelCheckZone.Visible = false;
                //    }
                //}
                //else if (Session_Zone != "ALL" && !Session_Zone.Contains(','))
                //{
                //    CheckBoxZonesAll.Visible = false;
                //    LabelCheckZone.Visible = false;
                //}

                //if (Session_Branch == "ALL")
                //{
                //    CheckBoxBranchesAll.Visible = true;
                //    LabelCheckBranch.Visible = true;
                //}
                //else if (Session_Branch != "ALL" && Session_Branch.Contains(','))
                //{
                //    int TotalBranchCount = GetAllBranchCount(clvar);

                //    string[] Branch = Session_Zone.Split(new[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);

                //    if (Branch.Length - 1 == TotalBranchCount)
                //    {
                //        CheckBoxBranchesAll.Visible = true;
                //        CheckBoxBranchesAll.Visible = true;
                //    }
                //    else
                //    {
                //        CheckBoxBranchesAll.Visible = false;
                //        LabelCheckBranch.Visible = false;
                //    }
                //}
                //else if (Session_Zone != "ALL" && !Session_Zone.Contains(','))
                //{
                //    CheckBoxBranchesAll.Visible = false;
                //    LabelCheckBranch.Visible = false;
                //}

                GetAllZones(clvar);
            }

        }

        public int GetAllZoneCount(Variable clvar)
        {
            DataTable dt = new DataTable();
            string SQLquery = "";

            SQLquery = "SELECT Z.NAME, Z.ZONECODE FROM ZONES Z WHERE Z.REGION IS NOT NULL AND Z.[STATUS] = '1' ORDER BY 1 ASC";

            SqlConnection con = new SqlConnection(clvar.Strcon());

            if (con.State.Equals(ConnectionState.Closed))
            {
                con.Open();
            }

            SqlDataAdapter sda = new SqlDataAdapter(SQLquery, con);
            sda.Fill(dt);
            return dt.Rows.Count;
        }

        public int GetAllBranchCount(Variable clvar)
        {
            DataTable dt = new DataTable();
            string SQLquery = "";

            SQLquery = "SELECT * FROM Branches b WHERE STATUS = '1'";

            SqlConnection con = new SqlConnection(clvar.Strcon());

            if (con.State.Equals(ConnectionState.Closed))
            {
                con.Open();
            }

            SqlDataAdapter sda = new SqlDataAdapter(SQLquery, con);
            sda.Fill(dt);
            return dt.Rows.Count;
        }

        public override void VerifyRenderingInServerForm(Control control)
        {

        }

        public void GetAllZones(Variable clvar)
        {
            DataSet Zones = b_fun.Get_AllZones1(clvar);

            ListBoxZones.DataSource = Zones;
            ListBoxZones.DataTextField = Zones.Tables[0].Columns["Name"].ToString();
            ListBoxZones.DataValueField = Zones.Tables[0].Columns["ZoneCode"].ToString();
            ListBoxZones.DataBind();
        }

        private void DeSelectAllZones()
        {
            for (int i = 0; i < ListBoxZones.Items.Count; i++)
            {
                ListBoxZones.Items[i].Selected = false;
            }

        }

        private void SelectAllZones()
        {
            for (int i = 0; i < ListBoxZones.Items.Count; i++)
            {
                ListBoxZones.Items[i].Selected = true;
            }
        }

        public void GetAllBranches(Variable clvar)
        {
            DropDownList Zone = ListBoxZones;

            #region When User has ALL Zones Right

            if (HttpContext.Current.Session["ZONECODE"].ToString() == "ALL")
            {
                string InPartQuery_Zone = string.Join(",", Zone.Items
                                         .Cast<System.Web.UI.WebControls.ListItem>()
                                         .Where(t => t.Selected)
                                         .Select(r => "'" + r.Value + "'"));
                InPartQuery_Zone.TrimEnd(',');

                #region When User has ALL Branches Rights

                if (HttpContext.Current.Session["BRANCHCODE"].ToString() == "ALL")
                {
                    string SelectedZones = string.Empty;

                    int SelectedZonesCount = 0;

                    foreach (ListItem item in ListBoxZones.Items)
                    {
                        if (item.Selected)
                        {
                            SelectedZones += "'" + item.Value + "'" + ",";
                            SelectedZonesCount = SelectedZonesCount + 1;
                        }
                    }

                    if (SelectedZones.Length > 3)
                    {
                        SelectedZones = SelectedZones.Remove(SelectedZones.Length - 1, 1);
                    }

                    string Combined_Query = string.Empty;

                    #region For Zone

                    if (ListBoxZones.Items.Count.Equals(SelectedZonesCount))
                    {
                        Combined_Query += " \n";
                    }
                    else if (!ListBoxZones.Items.Count.Equals(SelectedZonesCount))
                    {
                        Combined_Query += " AND z.zonecode IN (" + SelectedZones + ")         \n";
                    }

                    #endregion

                    clvar._status = "AND b.status = '1' \n";
                    clvar._Zone = Combined_Query;

                    DataSet Branch = b_fun.Get_MasterAllBranches(clvar);

                    ListBoxBranches.DataSource = Branch;
                    ListBoxBranches.DataTextField = Branch.Tables[0].Columns["branchname"].ToString();
                    ListBoxBranches.DataValueField = Branch.Tables[0].Columns["branchCode"].ToString();
                    ListBoxBranches.DataBind();

                    DeSelectAllBranches();
                }

                #endregion

                #region When User has Some Branches Rights

                else if (HttpContext.Current.Session["BRANCHCODE"].ToString() != "ALL")
                {
                    string SelectedZones = string.Empty;
                    string SelectedBranch = string.Empty;

                    int SelectedZonesCount = 0;
                    int SelectedBranchesCount = 0;

                    foreach (ListItem item in ListBoxZones.Items)
                    {
                        if (item.Selected)
                        {
                            SelectedZones += "'" + item.Value + "'" + ",";
                            SelectedZonesCount = SelectedZonesCount + 1;
                        }
                    }

                    if (SelectedZones.Length > 3)
                    {
                        SelectedZones = SelectedZones.Remove(SelectedZones.Length - 1, 1);
                    }

                    foreach (ListItem item in ListBoxBranches.Items)
                    {
                        if (item.Selected)
                        {
                            SelectedBranch += "'" + item.Value + "'" + ",";
                            SelectedBranchesCount = SelectedBranchesCount + 1;
                        }
                    }

                    if (SelectedBranch.Length > 3)
                    {
                        SelectedBranch = SelectedBranch.Remove(SelectedBranch.Length - 1, 1);
                    }

                    string Combined_Query = string.Empty;

                    #region For Zone

                    if (ListBoxZones.Items.Count.Equals(SelectedZonesCount))
                    {
                        Combined_Query += " \n";
                    }
                    else if (!ListBoxZones.Items.Count.Equals(SelectedZonesCount))
                    {
                        Combined_Query += " AND z.zonecode IN (" + SelectedZones + ")         \n";
                    }

                    #endregion

                    #region For Branch

                    if (ListBoxBranches.Items.Count.Equals(SelectedBranchesCount))
                    {
                        Combined_Query += " \n";
                    }
                    else if (!ListBoxBranches.Items.Count.Equals(SelectedBranchesCount))
                    {
                        Combined_Query += " AND b.branchcode IN (" + SelectedBranch + ")         \n";
                    }

                    #endregion

                    clvar._status = "AND b.status = '1' \n";
                    clvar._Zone = Combined_Query;

                    DataSet Branch_ = b_fun.Get_MasterAllBranches(clvar);

                    ListBoxBranches.DataSource = Branch_;
                    ListBoxBranches.DataTextField = Branch_.Tables[0].Columns["branchname"].ToString();
                    ListBoxBranches.DataValueField = Branch_.Tables[0].Columns["branchCode"].ToString();
                    ListBoxBranches.DataBind();

                    DeSelectAllBranches();
                }

                #endregion

            }

            #endregion

            #region When User has Some Zones Right

            else if (HttpContext.Current.Session["ZONECODE"].ToString() != "ALL")
            {

                #region When User has ALL Branches Rights

                if (HttpContext.Current.Session["BRANCHCODE"].ToString() == "ALL")
                {
                    string SelectedZones = string.Empty;

                    int SelectedZonesCount = 0;

                    foreach (ListItem item in ListBoxZones.Items)
                    {
                        if (item.Selected)
                        {
                            SelectedZones += "'" + item.Value + "'" + ",";
                            SelectedZonesCount = SelectedZonesCount + 1;
                        }
                    }

                    if (SelectedZones.Length > 3)
                    {
                        SelectedZones = SelectedZones.Remove(SelectedZones.Length - 1, 1);
                    }

                    string Combined_Query = string.Empty;

                    #region For Zone

                    if (ListBoxZones.Items.Count.Equals(SelectedZonesCount))
                    {
                        Combined_Query += " \n";
                    }
                    else if (!ListBoxZones.Items.Count.Equals(SelectedZonesCount))
                    {
                        Combined_Query += " AND z.zonecode IN (" + SelectedZones + ")         \n";
                    }

                    #endregion

                    clvar._status = "AND b.status = '1' \n";
                    clvar._Zone = Combined_Query;

                    DataSet Branch = b_fun.Get_MasterAllBranches(clvar);

                    ListBoxBranches.DataSource = Branch;
                    ListBoxBranches.DataTextField = Branch.Tables[0].Columns["branchname"].ToString();
                    ListBoxBranches.DataValueField = Branch.Tables[0].Columns["branchCode"].ToString();
                    ListBoxBranches.DataBind();

                    DeSelectAllBranches();
                }

                #endregion

                #region When User has Some Branches Rights

                else if (HttpContext.Current.Session["BRANCHCODE"].ToString() != "ALL")
                {
                    string SelectedZones = string.Empty;
                    string SelectedBranch = string.Empty;

                    int SelectedZonesCount = 0;
                    int SelectedBranchesCount = 0;

                    foreach (ListItem item in ListBoxZones.Items)
                    {
                        if (item.Selected)
                        {
                            SelectedZones += "'" + item.Value + "'" + ",";
                            SelectedZonesCount = SelectedZonesCount + 1;
                        }
                    }

                    if (SelectedZones.Length > 3)
                    {
                        SelectedZones = SelectedZones.Remove(SelectedZones.Length - 1, 1);
                    }

                    foreach (ListItem item in ListBoxBranches.Items)
                    {
                        if (item.Selected)
                        {
                            SelectedBranch += "'" + item.Value + "'" + ",";
                            SelectedBranchesCount = SelectedBranchesCount + 1;
                        }
                    }

                    if (SelectedBranch.Length > 3)
                    {
                        SelectedBranch = SelectedBranch.Remove(SelectedBranch.Length - 1, 1);
                    }

                    string Combined_Query = string.Empty;

                    #region For Zone

                    if (ListBoxZones.Items.Count.Equals(SelectedZonesCount))
                    {
                        Combined_Query += " \n";
                    }
                    else if (!ListBoxZones.Items.Count.Equals(SelectedZonesCount))
                    {
                        Combined_Query += " AND z.zonecode IN (" + SelectedZones + ")         \n";
                    }

                    #endregion

                    #region For Branch

                    if (ListBoxBranches.Items.Count.Equals(SelectedBranchesCount))
                    {
                        Combined_Query += " \n";
                    }
                    else if (!ListBoxBranches.Items.Count.Equals(SelectedBranchesCount))
                    {
                        Combined_Query += " AND b.branchcode IN (" + SelectedBranch + ")         \n";
                    }

                    #endregion

                    clvar._status = "AND b.status = '1' \n";
                    clvar._Zone = Combined_Query;

                    DataSet Branch_ = b_fun.Get_MasterAllBranches(clvar);

                    ListBoxBranches.DataSource = Branch_;
                    ListBoxBranches.DataTextField = Branch_.Tables[0].Columns["branchname"].ToString();
                    ListBoxBranches.DataValueField = Branch_.Tables[0].Columns["branchCode"].ToString();
                    ListBoxBranches.DataBind();

                    DeSelectAllBranches();
                }

                #endregion

            }

            #endregion
        }

        private void DeSelectAllBranches()
        {
            for (int i = 0; i < ListBoxBranches.Items.Count; i++)
            {
                ListBoxBranches.Items[i].Selected = false;
            }

        }

        private void SelectAllBranches()
        {
            for (int i = 0; i < ListBoxBranches.Items.Count; i++)
            {
                ListBoxBranches.Items[i].Selected = true;
            }
        }

        private void RemoveAllBranches()
        {

            List<System.Web.UI.WebControls.ListItem> itemsToRemove = new List<System.Web.UI.WebControls.ListItem>();

            foreach (System.Web.UI.WebControls.ListItem listItem in ListBoxBranches.Items)
            {
                if (!listItem.Selected || listItem.Selected)
                {
                    itemsToRemove.Add(listItem);
                }
            }

            foreach (System.Web.UI.WebControls.ListItem listItem in itemsToRemove)
            {
                ListBoxBranches.Items.Remove(listItem);
            }

            ListBoxBranches.DataBind();
        }

        protected void ListBoxZones_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            // Get Only Selected Branches related to Zone

            DropDownList Zone = ListBoxZones;

            #region When User has ALL Zones Right

            if (HttpContext.Current.Session["ZONECODE"].ToString() == "ALL")
            {
                string InPartQuery_Zone = string.Join(",", Zone.Items
                                         .Cast<System.Web.UI.WebControls.ListItem>()
                                         .Where(t => t.Selected)
                                         .Select(r => "'" + r.Value + "'"));
                InPartQuery_Zone.TrimEnd(',');

                #region When User has ALL Branches Rights

                if (HttpContext.Current.Session["BRANCHCODE"].ToString() == "ALL")
                {
                    string SelectedZones = string.Empty;

                    int SelectedZonesCount = 0;

                    foreach (ListItem item in ListBoxZones.Items)
                    {
                        if (item.Selected)
                        {
                            SelectedZones += "'" + item.Value + "'" + ",";
                            SelectedZonesCount = SelectedZonesCount + 1;
                        }
                    }

                    if (SelectedZones.Length > 3)
                    {
                        SelectedZones = SelectedZones.Remove(SelectedZones.Length - 1, 1);
                    }

                    string Combined_Query = string.Empty;

                    #region For Zone

                    if (ListBoxZones.Items.Count.Equals(SelectedZonesCount))
                    {
                        Combined_Query += " \n";
                    }
                    else if (!ListBoxZones.Items.Count.Equals(SelectedZonesCount))
                    {
                        Combined_Query += " AND z.zonecode IN (" + SelectedZones + ")         \n";
                    }

                    #endregion

                    clvar._status = "AND b.status = '1' \n";
                    clvar._Zone = Combined_Query;

                    DataSet Branch = b_fun.Get_MasterAllBranches(clvar);

                    ListBoxBranches.DataSource = Branch;
                    ListBoxBranches.DataTextField = Branch.Tables[0].Columns["branchname"].ToString();
                    ListBoxBranches.DataValueField = Branch.Tables[0].Columns["branchCode"].ToString();
                    ListBoxBranches.DataBind();

                    DeSelectAllBranches();
                }

                #endregion

                #region When User has Some Branches Rights

                else if (HttpContext.Current.Session["BRANCHCODE"].ToString() != "ALL")
                {
                    string SelectedZones = string.Empty;
                    string SelectedBranch = string.Empty;

                    int SelectedZonesCount = 0;
                    int SelectedBranchesCount = 0;

                    foreach (ListItem item in ListBoxZones.Items)
                    {
                        if (item.Selected)
                        {
                            SelectedZones += "'" + item.Value + "'" + ",";
                            SelectedZonesCount = SelectedZonesCount + 1;
                        }
                    }

                    if (SelectedZones.Length > 3)
                    {
                        SelectedZones = SelectedZones.Remove(SelectedZones.Length - 1, 1);
                    }

                    foreach (ListItem item in ListBoxBranches.Items)
                    {
                        if (item.Selected)
                        {
                            SelectedBranch += "'" + item.Value + "'" + ",";
                            SelectedBranchesCount = SelectedBranchesCount + 1;
                        }
                    }

                    if (SelectedBranch.Length > 3)
                    {
                        SelectedBranch = SelectedBranch.Remove(SelectedBranch.Length - 1, 1);
                    }

                    string Combined_Query = string.Empty;

                    #region For Zone

                    if (ListBoxZones.Items.Count.Equals(SelectedZonesCount))
                    {
                        Combined_Query += " \n";
                    }
                    else if (!ListBoxZones.Items.Count.Equals(SelectedZonesCount))
                    {
                        Combined_Query += " AND z.zonecode IN (" + SelectedZones + ")         \n";
                    }

                    #endregion

                    #region For Branch

                    if (ListBoxBranches.Items.Count.Equals(SelectedBranchesCount))
                    {
                        Combined_Query += " \n";
                    }
                    else if (!ListBoxBranches.Items.Count.Equals(SelectedBranchesCount))
                    {
                        Combined_Query += " AND b.branchcode IN (" + SelectedBranch + ")         \n";
                    }

                    #endregion

                    clvar._status = "AND b.status = '1' \n";
                    clvar._Zone = Combined_Query;

                    DataSet Branch_ = b_fun.Get_MasterAllBranches(clvar);

                    ListBoxBranches.DataSource = Branch_;
                    ListBoxBranches.DataTextField = Branch_.Tables[0].Columns["branchname"].ToString();
                    ListBoxBranches.DataValueField = Branch_.Tables[0].Columns["branchCode"].ToString();
                    ListBoxBranches.DataBind();

                    DeSelectAllBranches();
                }

                #endregion

            }

            #endregion

            #region When User has Some Zones Right

            else if (HttpContext.Current.Session["ZONECODE"].ToString() != "ALL")
            {

                #region When User has ALL Branches Rights

                if (HttpContext.Current.Session["BRANCHCODE"].ToString() == "ALL")
                {
                    string SelectedZones = string.Empty;

                    int SelectedZonesCount = 0;

                    foreach (ListItem item in ListBoxZones.Items)
                    {
                        if (item.Selected)
                        {
                            SelectedZones += "'" + item.Value + "'" + ",";
                            SelectedZonesCount = SelectedZonesCount + 1;
                        }
                    }

                    if (SelectedZones.Length > 3)
                    {
                        SelectedZones = SelectedZones.Remove(SelectedZones.Length - 1, 1);
                    }

                    string Combined_Query = string.Empty;

                    #region For Zone

                    if (ListBoxZones.Items.Count.Equals(SelectedZonesCount))
                    {
                        Combined_Query += " \n";
                    }
                    else if (!ListBoxZones.Items.Count.Equals(SelectedZonesCount))
                    {
                        Combined_Query += " AND z.zonecode IN (" + SelectedZones + ")         \n";
                    }

                    #endregion

                    clvar._status = "AND b.status = '1' \n";
                    clvar._Zone = Combined_Query;

                    DataSet Branch = b_fun.Get_MasterAllBranches(clvar);

                    ListBoxBranches.DataSource = Branch;
                    ListBoxBranches.DataTextField = Branch.Tables[0].Columns["branchname"].ToString();
                    ListBoxBranches.DataValueField = Branch.Tables[0].Columns["branchCode"].ToString();
                    ListBoxBranches.DataBind();

                    DeSelectAllBranches();
                }

                #endregion

                #region When User has Some Branches Rights

                else if (HttpContext.Current.Session["BRANCHCODE"].ToString() != "ALL")
                {
                    string SelectedZones = string.Empty;
                    string SelectedBranch = string.Empty;

                    int SelectedZonesCount = 0;
                    int SelectedBranchesCount = 0;

                    foreach (ListItem item in ListBoxZones.Items)
                    {
                        if (item.Selected)
                        {
                            SelectedZones += "'" + item.Value + "'" + ",";
                            SelectedZonesCount = SelectedZonesCount + 1;
                        }
                    }

                    if (SelectedZones.Length > 3)
                    {
                        SelectedZones = SelectedZones.Remove(SelectedZones.Length - 1, 1);
                    }

                    foreach (ListItem item in ListBoxBranches.Items)
                    {
                        if (item.Selected)
                        {
                            SelectedBranch += "'" + item.Value + "'" + ",";
                            SelectedBranchesCount = SelectedBranchesCount + 1;
                        }
                    }

                    if (SelectedBranch.Length > 3)
                    {
                        SelectedBranch = SelectedBranch.Remove(SelectedBranch.Length - 1, 1);
                    }

                    string Combined_Query = string.Empty;

                    #region For Zone

                    if (ListBoxZones.Items.Count.Equals(SelectedZonesCount))
                    {
                        Combined_Query += " \n";
                    }
                    else if (!ListBoxZones.Items.Count.Equals(SelectedZonesCount))
                    {
                        Combined_Query += " AND z.zonecode IN (" + SelectedZones + ")         \n";
                    }

                    #endregion

                    #region For Branch

                    if (ListBoxBranches.Items.Count.Equals(SelectedBranchesCount))
                    {
                        Combined_Query += " \n";
                    }
                    else if (!ListBoxBranches.Items.Count.Equals(SelectedBranchesCount))
                    {
                        Combined_Query += " AND b.branchcode IN (" + SelectedBranch + ")         \n";
                    }

                    #endregion

                    clvar._status = "AND b.status = '1' \n";
                    clvar._Zone = Combined_Query;

                    DataSet Branch_ = b_fun.Get_MasterAllBranches(clvar);

                    ListBoxBranches.DataSource = Branch_;
                    ListBoxBranches.DataTextField = Branch_.Tables[0].Columns["branchname"].ToString();
                    ListBoxBranches.DataValueField = Branch_.Tables[0].Columns["branchCode"].ToString();
                    ListBoxBranches.DataBind();

                    DeSelectAllBranches();
                }

                #endregion

            }

            #endregion


        }

        protected void btnSearchData_Click(object sender, EventArgs e)
        {
            string StartDate = txtStartDate.Text.ToString();
            string EndDate = txtEndDate.Text.ToString();
            string ZoneName = string.Empty;
            string ZoneID = ListBoxZones.SelectedValue.ToString();
            string BranchName = string.Empty;
            string BranchID = ListBoxBranches.SelectedValue.ToString();

            //string Flag = RBL_FlagType.SelectedValue.ToString();
            if (RBL_ReportCategory.SelectedValue == "Date")
            {
                if (ListBoxZones.SelectedItem != null)
                {
                    ZoneName = ListBoxZones.SelectedItem.Text.ToString();
                }
                if (ListBoxBranches.SelectedItem != null)
                {
                    BranchName = ListBoxBranches.SelectedItem.Text.ToString();
                }
            }

            hf_StartDate.Value = StartDate;
            hf_EndDate.Value = EndDate;
            hf_ZoneID.Value = ZoneID;
            hf_BranchID.Value = BranchID;
            //hf_FlagID.Value = Flag;

            hf_IOUNumber.Value = txtIOUNumber.Text.ToString();
            hf_EmployeeCode.Value = txtEmployeeCode.Text.ToString();
            hf_ReportType.Value = RBL_ReportCategory.SelectedItem.Value.ToString();
            hf_UserID.Value = Session["U_ID"].ToString();

            #region Check Before Search

            #region Report Type

            if (RBL_ReportCategory.SelectedValue == "0")
            {
                if (txtIOUNumber.Text.Length.Equals(0))
                {
                    lbl_Message.Text = "Error: Please Enter IOU Number!";
                    lbl_Message.ForeColor = System.Drawing.Color.Red;

                    foreach (ListItem item in RBL_ReportCategory.Items)
                    {
                        item.Attributes.CssStyle.Add("color", "white");
                        item.Attributes.CssStyle.Add("font-weight", "bold");
                        item.Attributes.CssStyle.Add("font-family", "Tahoma");
                        item.Attributes.CssStyle.Add("font-size", "11px");
                    }

                    //foreach (ListItem item in RBL_FlagType.Items)
                    //{
                    //    item.Attributes.CssStyle.Add("color", "white");
                    //    item.Attributes.CssStyle.Add("font-weight", "bold");
                    //    item.Attributes.CssStyle.Add("font-family", "Tahoma");
                    //    item.Attributes.CssStyle.Add("font-size", "11px");
                    //}

                    txtIOUNumber.Focus();
                    return;
                }
            }
            else if (RBL_ReportCategory.SelectedValue == "1")
            {
                if (txtEmployeeCode.Text.Length.Equals(0))
                {
                    lbl_Message.Text = "Error: Please Enter Employee Code!";
                    lbl_Message.ForeColor = System.Drawing.Color.Red;

                    foreach (ListItem item in RBL_ReportCategory.Items)
                    {
                        item.Attributes.CssStyle.Add("color", "white");
                        item.Attributes.CssStyle.Add("font-weight", "bold");
                        item.Attributes.CssStyle.Add("font-family", "Tahoma");
                        item.Attributes.CssStyle.Add("font-size", "11px");
                    }

                    //foreach (ListItem item in RBL_FlagType.Items)
                    //{
                    //    item.Attributes.CssStyle.Add("color", "white");
                    //    item.Attributes.CssStyle.Add("font-weight", "bold");
                    //    item.Attributes.CssStyle.Add("font-family", "Tahoma");
                    //    item.Attributes.CssStyle.Add("font-size", "11px");
                    //}

                    txtEmployeeCode.Focus();
                    return;
                }
            }

            #endregion

            if (RBL_ReportCategory.SelectedValue == "Date")
            {
                if (StartDate.Length.Equals(0))
                {
                    lbl_Message.Text = "Error: Please Select Start Date!";
                    lbl_Message.ForeColor = System.Drawing.Color.Red;

                    foreach (ListItem item in RBL_ReportCategory.Items)
                    {
                        item.Attributes.CssStyle.Add("color", "white");
                        item.Attributes.CssStyle.Add("font-weight", "bold");
                        item.Attributes.CssStyle.Add("font-family", "Tahoma");
                        item.Attributes.CssStyle.Add("font-size", "11px");
                    }

                    //foreach (ListItem item in RBL_FlagType.Items)
                    //{
                    //    item.Attributes.CssStyle.Add("color", "white");
                    //    item.Attributes.CssStyle.Add("font-weight", "bold");
                    //    item.Attributes.CssStyle.Add("font-family", "Tahoma");
                    //    item.Attributes.CssStyle.Add("font-size", "11px");
                    //}

                    txtStartDate.Focus();
                    return;
                }
                if (EndDate.Length.Equals(0))
                {
                    lbl_Message.Text = "Error: Please Select End Date!";
                    lbl_Message.ForeColor = System.Drawing.Color.Red;

                    foreach (ListItem item in RBL_ReportCategory.Items)
                    {
                        item.Attributes.CssStyle.Add("color", "white");
                        item.Attributes.CssStyle.Add("font-weight", "bold");
                        item.Attributes.CssStyle.Add("font-family", "Tahoma");
                        item.Attributes.CssStyle.Add("font-size", "11px");
                    }

                    //foreach (ListItem item in RBL_FlagType.Items)
                    //{
                    //    item.Attributes.CssStyle.Add("color", "white");
                    //    item.Attributes.CssStyle.Add("font-weight", "bold");
                    //    item.Attributes.CssStyle.Add("font-family", "Tahoma");
                    //    item.Attributes.CssStyle.Add("font-size", "11px");
                    //}

                    txtEndDate.Focus();
                    return;
                }
                if (ZoneName.Length.Equals(0))
                {
                    lbl_Message.Text = "Error: Please Select Zone!";
                    lbl_Message.ForeColor = System.Drawing.Color.Red;

                    foreach (ListItem item in RBL_ReportCategory.Items)
                    {
                        item.Attributes.CssStyle.Add("color", "white");
                        item.Attributes.CssStyle.Add("font-weight", "bold");
                        item.Attributes.CssStyle.Add("font-family", "Tahoma");
                        item.Attributes.CssStyle.Add("font-size", "11px");
                    }

                    //foreach (ListItem item in RBL_FlagType.Items)
                    //{
                    //    item.Attributes.CssStyle.Add("color", "white");
                    //    item.Attributes.CssStyle.Add("font-weight", "bold");
                    //    item.Attributes.CssStyle.Add("font-family", "Tahoma");
                    //    item.Attributes.CssStyle.Add("font-size", "11px");
                    //}

                    ListBoxZones.Focus();
                    return;
                }
                if (BranchName.Length.Equals(0))
                {
                    lbl_Message.Text = "Error: Please Select Branch!";
                    lbl_Message.ForeColor = System.Drawing.Color.Red;

                    foreach (ListItem item in RBL_ReportCategory.Items)
                    {
                        item.Attributes.CssStyle.Add("color", "white");
                        item.Attributes.CssStyle.Add("font-weight", "bold");
                        item.Attributes.CssStyle.Add("font-family", "Tahoma");
                        item.Attributes.CssStyle.Add("font-size", "11px");
                    }

                    //foreach (ListItem item in RBL_FlagType.Items)
                    //{
                    //    item.Attributes.CssStyle.Add("color", "white");
                    //    item.Attributes.CssStyle.Add("font-weight", "bold");
                    //    item.Attributes.CssStyle.Add("font-family", "Tahoma");
                    //    item.Attributes.CssStyle.Add("font-size", "11px");
                    //}

                    ListBoxBranches.Focus();
                    return;
                }
            }

            TimeSpan Difference = TimeSpan.Zero;

            if (RBL_ReportCategory.SelectedValue == "Date")
            {
                DateTime _StartDate = Convert.ToDateTime(StartDate);
                DateTime _EndDate = Convert.ToDateTime(EndDate);

                if (StartDate.Length > 0 && EndDate.Length > 0)
                {

                    Difference = _EndDate.Subtract(_StartDate);

                    if (Difference.Days < 0)
                    {
                        lbl_Message.Text = "Error: End Date is selected before Start Date!";
                        lbl_Message.ForeColor = System.Drawing.Color.Red;

                        foreach (ListItem item in RBL_ReportCategory.Items)
                        {
                            item.Attributes.CssStyle.Add("color", "white");
                            item.Attributes.CssStyle.Add("font-weight", "bold");
                            item.Attributes.CssStyle.Add("font-family", "Tahoma");
                            item.Attributes.CssStyle.Add("font-size", "11px");
                        }

                        //foreach (ListItem item in RBL_FlagType.Items)
                        //{
                        //    item.Attributes.CssStyle.Add("color", "white");
                        //    item.Attributes.CssStyle.Add("font-weight", "bold");
                        //    item.Attributes.CssStyle.Add("font-family", "Tahoma");
                        //    item.Attributes.CssStyle.Add("font-size", "11px");
                        //}

                        return;
                    }

                    //else if (!_StartDate.Month.Equals(_EndDate.Month)
                    //        || !_StartDate.Year.Equals(_EndDate.Year))
                    //{
                    //    lbl_Message.Text = "Error: Unable to Select Data of More than 31 Days!";
                    //    lbl_Message.ForeColor = System.Drawing.Color.Red;

                    //    foreach (ListItem item in RBL_ReportCategory.Items)
                    //    {
                    //        item.Attributes.CssStyle.Add("color", "white");
                    //        item.Attributes.CssStyle.Add("font-weight", "bold");
                    //        item.Attributes.CssStyle.Add("font-family", "Tahoma");
                    //        item.Attributes.CssStyle.Add("font-size", "11px");
                    //    }

                    //    //foreach (ListItem item in RBL_FlagType.Items)
                    //    //{
                    //    //    item.Attributes.CssStyle.Add("color", "white");
                    //    //    item.Attributes.CssStyle.Add("font-weight", "bold");
                    //    //    item.Attributes.CssStyle.Add("font-family", "Tahoma");
                    //    //    item.Attributes.CssStyle.Add("font-size", "11px");
                    //    //}

                    //    return;
                    //}

                    else if (Difference.Days > 31)
                    {
                        lbl_Message.Text = "Error: Unable to Select Data of More than 31 Days!";
                        lbl_Message.ForeColor = System.Drawing.Color.Red;

                        foreach (ListItem item in RBL_ReportCategory.Items)
                        {
                            item.Attributes.CssStyle.Add("color", "white");
                            item.Attributes.CssStyle.Add("font-weight", "bold");
                            item.Attributes.CssStyle.Add("font-family", "Tahoma");
                            item.Attributes.CssStyle.Add("font-size", "11px");
                        }

                        //foreach (ListItem item in RBL_FlagType.Items)
                        //{
                        //    item.Attributes.CssStyle.Add("color", "white");
                        //    item.Attributes.CssStyle.Add("font-weight", "bold");
                        //    item.Attributes.CssStyle.Add("font-family", "Tahoma");
                        //    item.Attributes.CssStyle.Add("font-size", "11px");
                        //}

                        return;
                    }

                }

                else
                {
                    GridViewPettyCash_IOUReport_Date.Visible = false;
                    lbl_report_name.Visible = false;
                    lbl_report_version.Visible = false;
                    lbl_total_record.Visible = false;

                    error_msg.Text = "Select Filter";
                }
            }

            #endregion

            #region For HTML Output

            GridViewPettyCash_IOUReport_Date.Visible = false;
            GridViewPettyCash_IOUReport.Visible = false;

            if (RBL_ReportCategory.SelectedValue == "Date")
            {
                if ((StartDate != "" && StartDate != null) && (EndDate != "" && EndDate != null) && (ZoneName != "" && ZoneName != null) && (BranchName != "" && BranchName != null))
                {
                    #region Without VOID Date
                    /*
                    string[] startyear_ = StartDate.Split('-');
                    string startyear = startyear_[0].ToString();

                    clvar.Year = startyear;
                    clvar.StartDate = txtStartDate.Text;

                    if (lbl_report_name.Visible && lbl_report_version.Visible && lbl_total_record.Visible && btn_HTML.Visible)
                    {
                        GridViewPettyCash_IOUReport_Date.Visible = false;
                        GridViewPettyCash_IOUReport.Visible = false;
                        lbl_report_name.Visible = false;
                        lbl_report_version.Visible = false;
                        lbl_total_record.Visible = false;
                        btn_HTML.Visible = false;
                    }

                    DataSet ds = Get_PettyCash_IOU(clvar);

                    lbl_msg.Text = "";
                    lbl_total_record.Text = "";

                    if (ds.Tables[0].Rows.Count != 0)
                    {
                        GridViewPettyCash_IOUReport_Date.Visible = true;
                        GridViewPettyCash_IOUReport.Visible = false;
                        error_msg.Text = "";
                        lbl_total_record.Text = "Total Record: " + ds.Tables[0].Rows.Count.ToString();
                        lbl_Message.Text = "";

                        GridViewDataBind(ds.Tables[0]);

                        clvar._ReportId = reportid;
                        b_fun.Insert_ReportTrackLog(clvar);
                        Get_ReportVersion();
                        lbl_report_name.Text = "Petty Cash IOU Report";

                        lbl_report_name.Visible = true;
                        lbl_report_version.Visible = true;
                        lbl_total_record.Visible = true;
                        btn_HTML.Visible = true;
                    }
                    else
                    {
                        GridViewPettyCash_IOUReport_Date.Visible = false;
                        GridViewPettyCash_IOUReport.Visible = false;
                        lbl_report_name.Visible = false;
                        lbl_report_version.Visible = false;
                        lbl_total_record.Visible = false;
                        btn_HTML.Visible = false;

                        lbl_Message.Text = "No Record Found...";
                        lbl_Message.ForeColor = System.Drawing.Color.Red;

                        //error_msg.Text = "No Record Found...<br> " + catch_error_msg;


                    }
                    */
                    #endregion

                    btn_HTML_Click(sender, e);
                }
            }

            else if (RBL_ReportCategory.SelectedValue == "0")
            {
                if (txtIOUNumber.Text != "" && txtIOUNumber.Text != null)
                {
                    #region Without VOID IOU Number
                    /*
                    string[] startyear_ = StartDate.Split('-');
                    string startyear = startyear_[0].ToString();

                    clvar.Year = startyear;
                    clvar.StartDate = txtStartDate.Text;

                    if (lbl_report_name.Visible && lbl_report_version.Visible && lbl_total_record.Visible && btn_HTML.Visible)
                    {
                        GridViewPettyCash_IOUReport_Date.Visible = false;
                        GridViewPettyCash_IOUReport.Visible = false;
                        lbl_report_name.Visible = false;
                        lbl_report_version.Visible = false;
                        lbl_total_record.Visible = false;
                        btn_HTML.Visible = false;
                    }

                    DataSet ds = Get_PettyCash_IOU(clvar);

                    lbl_msg.Text = "";
                    lbl_total_record.Text = "";

                    if (ds.Tables[0].Rows.Count != 0)
                    {
                        GridViewPettyCash_IOUReport_Date.Visible = false;
                        GridViewPettyCash_IOUReport.Visible = true;
                        error_msg.Text = "";
                        lbl_total_record.Text = "Total Record: " + ds.Tables[0].Rows.Count.ToString();
                        lbl_Message.Text = "";

                        GridViewDataBind(ds.Tables[0]);

                        clvar._ReportId = reportid;
                        b_fun.Insert_ReportTrackLog(clvar);
                        Get_ReportVersion();
                        lbl_report_name.Text = "Petty Cash IOU Report";

                        lbl_report_name.Visible = true;
                        lbl_report_version.Visible = true;
                        lbl_total_record.Visible = true;
                        btn_HTML.Visible = true;
                    }
                    else
                    {
                        GridViewPettyCash_IOUReport_Date.Visible = false;
                        GridViewPettyCash_IOUReport.Visible = false;
                        lbl_report_name.Visible = false;
                        lbl_report_version.Visible = false;
                        lbl_total_record.Visible = false;
                        btn_HTML.Visible = false;

                        lbl_Message.Text = "No Record Found...";
                        lbl_Message.ForeColor = System.Drawing.Color.Red;

                        //error_msg.Text = "No Record Found...<br> " + catch_error_msg;


                    }
                    */
                    #endregion

                    btn_HTML_Click(sender, e);
                }
            }

            else if (RBL_ReportCategory.SelectedValue == "1")
            {
                if (txtEmployeeCode.Text != "" && txtEmployeeCode.Text != null)
                {
                    #region Without VOID Employee Code
                    /*
                    string[] startyear_ = StartDate.Split('-');
                    string startyear = startyear_[0].ToString();

                    clvar.Year = startyear;
                    clvar.StartDate = txtStartDate.Text;

                    if (lbl_report_name.Visible && lbl_report_version.Visible && lbl_total_record.Visible && btn_HTML.Visible)
                    {
                        GridViewPettyCash_IOUReport_Date.Visible = false;
                        GridViewPettyCash_IOUReport.Visible = false;
                        lbl_report_name.Visible = false;
                        lbl_report_version.Visible = false;
                        lbl_total_record.Visible = false;
                        btn_HTML.Visible = false;
                    }

                    DataSet ds = Get_PettyCash_IOU(clvar);

                    lbl_msg.Text = "";
                    lbl_total_record.Text = "";

                    if (ds.Tables[0].Rows.Count != 0)
                    {
                        GridViewPettyCash_IOUReport_Date.Visible = false;
                        GridViewPettyCash_IOUReport.Visible = true;
                        error_msg.Text = "";
                        lbl_total_record.Text = "Total Record: " + ds.Tables[0].Rows.Count.ToString();
                        lbl_Message.Text = "";

                        GridViewDataBind(ds.Tables[0]);

                        clvar._ReportId = reportid;
                        b_fun.Insert_ReportTrackLog(clvar);
                        Get_ReportVersion();
                        lbl_report_name.Text = "Petty Cash IOU Report";

                        lbl_report_name.Visible = true;
                        lbl_report_version.Visible = true;
                        lbl_total_record.Visible = true;
                        btn_HTML.Visible = true;
                    }
                    else
                    {
                        GridViewPettyCash_IOUReport_Date.Visible = false;
                        GridViewPettyCash_IOUReport.Visible = false;
                        lbl_report_name.Visible = false;
                        lbl_report_version.Visible = false;
                        lbl_total_record.Visible = false;
                        btn_HTML.Visible = false;

                        lbl_Message.Text = "No Record Found...";
                        lbl_Message.ForeColor = System.Drawing.Color.Red;

                        //error_msg.Text = "No Record Found...<br> " + catch_error_msg;


                    }
                    */
                    #endregion

                    btn_HTML_Click(sender, e);
                }
            }



            #endregion

        }

        private void GridViewDataBind(DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                btn_HTML.Visible = true;
                int TotalRecords = int.Parse(dt.Rows.Count.ToString());
                lbl_total_record.Text = "Total Records: " + TotalRecords.ToString("N0");

                if (RBL_ReportCategory.SelectedItem.Value == "Date")
                {
                    GridViewPettyCash_IOUReport_Date.DataSource = dt.DefaultView;
                    GridViewPettyCash_IOUReport_Date.DataBind();
                }
                else if (RBL_ReportCategory.SelectedItem.Value == "0" || RBL_ReportCategory.SelectedItem.Value == "1")  //  IOU Number OR Employee Code
                {
                    GridViewPettyCash_IOUReport.DataSource = dt.DefaultView;
                    GridViewPettyCash_IOUReport.DataBind();
                }
            }
            else
            {
                lbl_Message.Text = "No Records are found!";
                lbl_Message.ForeColor = System.Drawing.Color.Red;
                //error_msg.Text = "No Records are found!";
            }
        }

        protected void btn_HTML_Click(object sender, EventArgs e)
        {
            string Query_StartDate = Encrypt_QueryString(hf_StartDate.Value.ToString());
            string Query_EndDate = Encrypt_QueryString(hf_EndDate.Value.ToString());
            string Query_ZoneID = Encrypt_QueryString(hf_ZoneID.Value.ToString());
            string Query_BranchID = Encrypt_QueryString(hf_BranchID.Value.ToString());
            //string Query_FlagID = Encrypt_QueryString(hf_FlagID.Value.ToString());
            string Query_IOUNumber = Encrypt_QueryString(hf_IOUNumber.Value.ToString());
            string Query_EmployeeCode = Encrypt_QueryString(hf_EmployeeCode.Value.ToString());
            string Query_ReportType = Encrypt_QueryString(hf_ReportType.Value.ToString());
            string Query_UserID = Encrypt_QueryString(hf_UserID.Value.ToString());

            //Response.Redirect("PettyCash_IOU_Print.aspx?startdate=" + Query_StartDate + "'&enddate=" + Query_EndDate + "&zoneid=" + Query_ZoneID + "&branchid=" + Query_BranchID + "&flagid=" + Query_FlagID + "&iounumber=" + Query_IOUNumber + "&employeecode=" + Query_EmployeeCode + "&reporttype=" + Query_ReportType + "&userid=" + Query_UserID);
            ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "OpenWindow", "window.open('PettyCash_IOU_Print.aspx?startdate=" + Query_StartDate + "&enddate=" + Query_EndDate + "&zoneid=" + Query_ZoneID + "&branchid=" + Query_BranchID + "&iounumber=" + Query_IOUNumber + "&employeecode=" + Query_EmployeeCode + "&reporttype=" + Query_ReportType + "&userid=" + Query_UserID + "','Petty Cash IOU','menubar=1,scrollbars=yes,resizable=1,width=900,height=600');", true);
            //ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "OpenWindow", "window.open('PettyCash_IOU_Print.aspx?startdate=" + hf_StartDate.Value + "&enddate=" + hf_EndDate.Value + "&zoneid=" + hf_ZoneID.Value + "&branchid=" + hf_BranchID.Value + "&flagid=" + hf_FlagID.Value + "&iounumber=" + hf_IOUNumber.Value + "&employeecode=" + hf_EmployeeCode.Value + "&reporttype=" + hf_ReportType.Value + "&userid=" + hf_UserID.Value + ",'Petty Cash IOU','menubar=1,resizable=1,width=900,height=600');", true);
            return;
        }

        protected void GridViewPettyCash_IOUReport_Date_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewPettyCash_IOUReport_Date.PageIndex = e.NewPageIndex;
            this.GridViewDataBind(Get_PettyCash_IOU(clvar).Tables[0]);
        }

        protected void GridViewPettyCash_IOUReport_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewPettyCash_IOUReport.PageIndex = e.NewPageIndex;
            this.GridViewDataBind(Get_PettyCash_IOU(clvar).Tables[0]);
        }

        public DataSet Get_PettyCash_IOU(Variable clvar)
        {
            DataSet ds = new DataSet();
            string sqlString = "";

            clvar.StartDate = txtStartDate.Text;
            clvar.EndDate = txtEndDate.Text;

            string UserID = string.Empty;

            if (Session["U_ID"] != null)
            {
                UserID = Session["U_ID"].ToString();
            }

            string SelectedZones = string.Empty;
            string SelectedBranch = string.Empty;

            if (RBL_ReportCategory.SelectedValue == "Date")
            {
                SelectedZones = ListBoxZones.SelectedItem.Value;
                SelectedBranch = ListBoxBranches.SelectedItem.Value;
            }

            #region SELECT QUERY OF SQL


            try
            {
                #region For Date

                if (RBL_ReportCategory.SelectedValue == "Date")
                {
                    sqlString = "/************************************************************ \n"
                   + " * Code formatted by SoftTree SQL Assistant © v6.3.153 \n"
                   + " * Time: 4/15/2019 11:40:34 AM \n"
                   + " ************************************************************/ \n"
                   + " \n"
                   + "SELECT A.DateIOU, \n"
                   + "       A.Zone, \n"
                   + "       A.Branch, \n"
                   + "       A.IOUNumber, \n"
                   + "       A.EmployeeCode, \n"
                   + "       A.EmployeeName, \n"
                   + "       ISNULL(A.[OutstandingAmount], 0)     OutstandingAmount, \n"
                   + "       ISNULL(A.SettledAmount, 0)           SettledAmount, \n"
                   + "       ( \n"
                   + "           ( \n"
                   + "               ISNULL(A.SettledAmount, 0) - ISNULL(a.[OutstandingAmount], 0) \n"
                   + "           ) * -1 \n"
                   + "       )                                 AS 'RemainingAmount', \n"
                   + "       A.Reason, \n"
                   + "       A.Remarks, \n"
                   + "       A.CreateOn, \n"
                   + "       A.CreateBy, \n"
                   + "       A.ModifyOn, \n"
                   + "       A.ModifyBy \n"
                   + "FROM   ( \n"
                   + "           SELECT CONVERT(VARCHAR(11), PC.[DateIOU], 106) 'DateIOU', \n"
                   + "                  Z.name 'Zone', \n"
                   + "                  B.name 'Branch', \n"
                   + "                  PC.[IOUNumber] 'IOUNumber', \n"
                   + "                  PC.[EmployeeCode] 'EmployeeCode', \n"
                   + "                  PC.[EmployeeName] 'EmployeeName', \n"
                   + "                  SUM(CASE WHEN pc.Flag = '1' THEN PC.[Amount] END) AS  \n"
                   + "                  'OutstandingAmount', \n"
                   + "                  SUM(CASE WHEN pc.Flag = '2' THEN PC.[Amount] END) AS  \n"
                   + "                  'SettledAmount', \n"
                   + "                  MAX(PC.[Reason]) 'Reason', \n"
                   + "                  MAX(PC.[Remarks]) 'Remarks', \n"
                   + "                  CONVERT(VARCHAR(11), PC.[CreateOn], 106) 'CreateOn', \n"
                   + "                  ZU.[Name] 'CreateBy', \n"
                   + "                  CONVERT(VARCHAR(11), PC.[ModifyOn], 106) 'ModifyOn', \n"
                   + "                  ZU_.[Name] 'ModifyBy' \n"
                   + "           FROM   [dbo].[PC_IOU] PC \n"
                   + "                  INNER JOIN BRANCHES B \n"
                   + "                       ON  B.branchcode = PC.Branch \n"
                   + "                  INNER JOIN ZONES Z \n"
                   + "                       ON  Z.zonecode = PC.Zone \n"
                   + "                  INNER JOIN PC_IOU_Flag PC_Flag \n"
                   + "                       ON  PC_Flag.Id = PC.Flag \n"
                   + "                  INNER JOIN ZNI_USER1 ZU \n"
                   + "                       ON  ZU.U_ID = PC.CreateBy \n"
                   + "                  LEFT JOIN ZNI_USER1 ZU_ \n"
                   + "                       ON  ZU_.U_ID = PC.ModifyBy \n"
                   + "           WHERE  \n";

                    #region Report Type

                    if (RBL_ReportCategory.SelectedValue == "Date")
                    {
                        sqlString += " CAST(PC.CreateOn AS Date) >= '" + clvar.StartDate + "' \n"
                                + " AND CAST(PC.CreateOn AS Date) <= '" + clvar.EndDate + "' \n";
                    }

                    #endregion

                    #region For Zone

                    if (RBL_ReportCategory.SelectedValue == "Date")
                    {
                        sqlString += " AND PC.Zone = '" + SelectedZones + "'         \n";
                    }

                    #endregion

                    #region For Branch

                    if (RBL_ReportCategory.SelectedValue == "Date")
                    {
                        sqlString += " AND PC.Branch = '" + SelectedBranch + "'         \n";
                    }

                    #endregion

                    sqlString += " AND PC.CreateBy = '" + UserID + "' \n"
                   + "                  GROUP BY \n"
                   + "                  CONVERT(VARCHAR(11), PC.[DateIOU], 106), \n"
                   + "                  Z.name, \n"
                   + "                  B.name, \n"
                   + "                  PC.[IOUNumber], \n"
                   + "                  PC.[EmployeeCode], \n"
                   + "                  PC.[EmployeeName], \n"
                   + "                  CONVERT(VARCHAR(11), PC.[CreateOn], 106), \n"
                   + "                  ZU.[Name], \n"
                   + "                  CONVERT(VARCHAR(11), PC.[ModifyOn], 106), \n"
                   + "                  ZU_.[Name] \n"
                   + "       )                                    A \n"
                   + "GROUP BY \n"
                   + "       A.DateIOU, \n"
                   + "       A.Zone, \n"
                   + "       A.Branch, \n"
                   + "       A.IOUNumber, \n"
                   + "       A.EmployeeCode, \n"
                   + "       A.EmployeeName, \n"
                   + "       A.SettledAmount, \n"
                   + "       A.OutstandingAmount, \n"
                   + "       A.Reason, \n"
                   + "       A.Remarks, \n"
                   + "       A.CreateOn, \n"
                   + "       A.CreateBy, \n"
                   + "       A.ModifyOn, \n"
                   + "       A.ModifyBy \n"
                   + "ORDER BY \n"
                   + "       A.CreateOn DESC \n"
                   + "";
                }

                #endregion

                #region For IOU Number & Employee Code

                if (RBL_ReportCategory.SelectedValue == "0" || RBL_ReportCategory.SelectedValue == "1")
                {
                    sqlString = "SELECT  \n"
                               + "       CONVERT(VARCHAR(11), PC.[DateIOU], 106) 'DateIOU', \n"
                               + "       Z.name 'Zone', \n"
                               + "       B.name 'Branch', \n"
                               + "       PC.[IOUNumber] 'IOUNumber', \n"
                               + "       PC.[EmployeeCode] 'EmployeeCode', \n"
                               + "       PC.[EmployeeName] 'EmployeeName', \n"
                               + "       UPPER(PC_Flag.[Flag]) 'Flag', \n"
                               + "       PC.[Amount] 'Amount', \n"
                               + "       PC.[Reason] 'Reason', \n"
                               + "       PC.[Remarks] 'Remarks', \n"
                               + "       CONVERT(VARCHAR(11), PC.[CreateOn], 106) 'CreateOn', \n"
                               + "       ZU.[Name] 'CreateBy', \n"
                               + "       CONVERT(VARCHAR(11), PC.[ModifyOn], 106) 'ModifyOn', \n"
                               + "       ZU_.[Name] 'ModifyBy' \n"
                               + " FROM [dbo].[PC_IOU] PC \n"
                               + " INNER JOIN BRANCHES B \n"
                               + " ON B.branchcode = PC.Branch \n"
                               + " INNER JOIN ZONES Z \n"
                               + " ON Z.zonecode = PC.Zone \n"
                               + " INNER JOIN PC_IOU_Flag PC_Flag \n"
                               + " ON PC_Flag.Id = PC.Flag \n"
                               + " INNER JOIN ZNI_USER1 ZU \n"
                               + " ON ZU.U_ID = PC.CreateBy \n"
                               + " LEFT JOIN ZNI_USER1 ZU_ \n"
                               + " ON ZU_.U_ID = PC.ModifyBy \n"
                               + " WHERE  \n";

                    #region Report Type

                    if (RBL_ReportCategory.SelectedValue == "0")
                    {
                        if (txtIOUNumber.Text.Length > 0)
                        {
                            sqlString += " PC.IOUNumber = '" + txtIOUNumber.Text.ToString() + "' \n";
                        }
                    }
                    else if (RBL_ReportCategory.SelectedValue == "1")
                    {
                        if (txtEmployeeCode.Text.Length > 0)
                        {
                            sqlString += " PC.EmployeeCode = '" + txtEmployeeCode.Text.ToString() + "' \n";
                        }
                    }

                    #endregion

                    sqlString += " AND PC.CreateBy = '" + UserID + "' \n"
                                + " ORDER BY PC.CreateOn Desc";
                }


                #endregion

                #endregion

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandTimeout = 6000;
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);

                dataset = ds;

                orcl.Close();

            }
            catch (Exception Err)
            {
                catch_error_msg = Err.Message.ToString();
            }
            finally
            { }
            return ds;
        }

        public static string Encrypt_QueryString(string str)
        {
            string EncrptKey = "2013;[pnuLIT)WebCodeExpert";
            byte[] byKey = { };
            byte[] IV = { 18, 52, 86, 120, 144, 171, 205, 239 };
            byKey = System.Text.Encoding.UTF8.GetBytes(EncrptKey.Substring(0, 8));
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = Encoding.UTF8.GetBytes(str);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(byKey, IV), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return Convert.ToBase64String(ms.ToArray());
        }

        private decimal GetTotalRowSum(DataTable datatable, string ColumnName)
        {
            decimal Total = 0M;

            if (datatable.Rows.Count < 0)
            {
                return Total;
            }
            else
            {
                for (int i = 0; i < datatable.Rows.Count; i++)
                {
                    if (!datatable.Rows[i][ColumnName].Equals(null))
                    {
                        Total += decimal.Parse(datatable.Rows[i][ColumnName].ToString());
                    }

                }
                return Total;
            }
        }

        protected void Get_ReportVersion()
        {
            lbl_report_version.Text = "";
            clvar._reportid = reportid;
            DataSet ds = b_fun.Get_Report_VersionByReportId(clvar);
            if (ds.Tables[0].Rows.Count != 0)
            {
                lbl_report_version.Text = "Version: " + ds.Tables[0].Rows[0]["version"].ToString();
            }
        }

        protected void RBL_ReportCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            #region Date Option


            if (RBL_ReportCategory.SelectedValue == "Date")    //  When User Select Date Option
            {
                txtStartDate.Text = "";
                txtEndDate.Text = "";

                string Session_Zone = HttpContext.Current.Session["ZONECODE"].ToString();
                string Session_Branch = HttpContext.Current.Session["BRANCHCODE"].ToString();

                if (Session_Zone == "ALL")
                {
                    ListBoxZones.Visible = true;
                    ListBoxZones.SelectedValue = null;
                    GetAllZones(clvar);
                }
                else if (Session_Zone != "ALL" && Session_Zone.Contains(','))
                {
                    int TotalZoneCount = GetAllZoneCount(clvar);

                    string[] Zones = Session_Zone.Split(new[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);

                    if (Zones.Length - 1 == TotalZoneCount)
                    {
                        ListBoxZones.Visible = true;
                        ListBoxZones.SelectedValue = null;
                        GetAllZones(clvar);
                    }
                    else
                    {
                        ListBoxZones.Visible = true;
                        ListBoxZones.SelectedValue = null;
                        GetAllZones(clvar);
                    }
                }
                else if (Session_Zone != "ALL" && !Session_Zone.Contains(','))
                {
                    ListBoxZones.Visible = true;
                    ListBoxZones.SelectedValue = null;
                    GetAllZones(clvar);
                }

                if (Session_Branch == "ALL")
                {
                    RemoveAllBranches();
                    ListBoxBranches.Visible = true;
                }
                else if (Session_Branch != "ALL" && Session_Branch.Contains(','))
                {
                    int TotalBranchCount = GetAllBranchCount(clvar);

                    string[] Branch = Session_Zone.Split(new[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);

                    if (Branch.Length - 1 == TotalBranchCount)
                    {
                        RemoveAllBranches();
                        ListBoxBranches.Visible = true;
                    }
                    else
                    {
                        RemoveAllBranches();
                        ListBoxBranches.Visible = true;
                    }
                }
                else if (Session_Branch != "ALL" && !Session_Branch.Contains(','))
                {
                    RemoveAllBranches();
                    ListBoxBranches.Visible = true;
                }

                Td_IOU_EC.Visible = false;
                Td_Date.Visible = true;
                Td_Zone.Visible = true;
                //Td_Branch.Visible = true;
                //Td_FlagType.Visible = true;

                lblIOUNO.Visible = false;
                txtIOUONO.Visible = false;

                lblEC.Visible = false;
                txtEC.Visible = false;

                GridViewPettyCash_IOUReport_Date.DataSource = null;
                GridViewPettyCash_IOUReport_Date.DataBind();

                GridViewPettyCash_IOUReport.DataSource = null;
                GridViewPettyCash_IOUReport.DataBind();

                btn_HTML.Visible = false;
                lbl_total_record.Visible = false;
                lbl_report_name.Visible = false;
                lbl_report_version.Visible = false;
                error_msg.Visible = false;
                lbl_Message.Text = "";

            }


            #endregion

            #region IOU Number Option


            else if (RBL_ReportCategory.SelectedValue == "0")    //  When User Select IOU Number Option
            {
                txtIOUNumber.Text = "";

                ListBoxZones.Visible = false;
                ListBoxZones.SelectedValue = null;
                RemoveAllBranches();
                ListBoxBranches.Visible = false;

                Td_IOU_EC.Visible = true;
                Td_Date.Visible = false;
                Td_Zone.Visible = false;
                //Td_Branch.Visible = false;
                //Td_FlagType.Visible = false;

                txtIOUNumber.Focus();

                lblIOUNO.Visible = true;
                txtIOUONO.Visible = true;

                lblEC.Visible = false;
                txtEC.Visible = false;

                GridViewPettyCash_IOUReport_Date.DataSource = null;
                GridViewPettyCash_IOUReport_Date.DataBind();

                GridViewPettyCash_IOUReport.DataSource = null;
                GridViewPettyCash_IOUReport.DataBind();

                btn_HTML.Visible = false;
                lbl_total_record.Visible = false;
                lbl_report_name.Visible = false;
                lbl_report_version.Visible = false;
                error_msg.Visible = false;
                lbl_Message.Text = "";

            }


            #endregion

            #region Employee Code Option


            else if (RBL_ReportCategory.SelectedValue == "1")       //  When User Select Employee Code Option
            {
                txtEmployeeCode.Text = "";

                ListBoxZones.Visible = false;
                ListBoxZones.SelectedValue = null;
                RemoveAllBranches();
                ListBoxBranches.Visible = false;

                Td_IOU_EC.Visible = true;
                Td_Date.Visible = false;
                Td_Zone.Visible = false;
                //Td_Branch.Visible = false;
                //Td_FlagType.Visible = false;

                txtEmployeeCode.Focus();

                lblIOUNO.Visible = false;
                txtIOUONO.Visible = false;

                lblEC.Visible = true;
                txtEC.Visible = true;

                GridViewPettyCash_IOUReport_Date.DataSource = null;
                GridViewPettyCash_IOUReport_Date.DataBind();

                GridViewPettyCash_IOUReport.DataSource = null;
                GridViewPettyCash_IOUReport.DataBind();

                btn_HTML.Visible = false;
                lbl_total_record.Visible = false;
                lbl_report_name.Visible = false;
                lbl_report_version.Visible = false;
                error_msg.Visible = false;
                lbl_Message.Text = "";
            }


            #endregion
        }

    }
}