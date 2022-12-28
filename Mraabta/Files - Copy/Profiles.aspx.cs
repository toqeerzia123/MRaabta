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
    public partial class Profiles : System.Web.UI.Page
    {
        Variable clvar = new Variable();
        bayer_Function b_fun = new bayer_Function();

        String id;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (id != null)
                {
                    Get_SingleProfile();
                    Get_MainMenu();
                    Get_Zone();
                }
                else
                {
                    Get_MainMenu();
                    //   Get_Reports();
                    Get_Zone();

                    GridView.Visible = false;
                    DataTable dt_head = new DataTable();
                    dt_head.Columns.Add("Profile_Name", typeof(string));
                    dt_head.Columns.Add("ZoneCode", typeof(string));
                    dt_head.Columns.Add("ZoneName", typeof(string));
                    dt_head.Columns.Add("BranchCode", typeof(string));
                    dt_head.Columns.Add("BranchName", typeof(string));
                    dt_head.Columns.Add("ExpressCenterCode", typeof(string));
                    dt_head.Columns.Add("ExpressCenterName", typeof(string));
                    ViewState["dthead"] = dt_head;

                    DataTable dt = new DataTable();
                    dt.Columns.Add("Profile_Name", typeof(string));
                    //dt.Columns.Add("ZoneCode", typeof(int));
                    //dt.Columns.Add("Zonename");
                    //dt.Columns.Add("BranchCode", typeof(int));
                    //dt.Columns.Add("BrancHName");
                    //dt.Columns.Add("ExpressCenterCode", typeof(int));
                    //dt.Columns.Add("ExpressCenter");
                    dt.Columns.Add("EntryDateTime", typeof(DateTime));
                    dt.Columns.Add("Status", typeof(int));
                    dt.Columns.Add("MainMenu_Id", typeof(int));
                    dt.Columns.Add("MainMenu");
                    dt.Columns.Add("ChildMenu_Id", typeof(int));
                    dt.Columns.Add("ChildMenu");
                    // dt.AcceptChanges();
                    ViewState["dt"] = dt;

                    saveall.Visible = false;
                }
            }
        }

        public void Get_MainMenu()
        {
            DataSet ds_mainmenu = b_fun.Get_MainMenu(clvar);

            if (ds_mainmenu.Tables[0].Rows.Count != 0)
            {
                dd_mainmenu.DataTextField = "MENU_NAME";
                dd_mainmenu.DataValueField = "MENU_ID";
                dd_mainmenu.DataSource = ds_mainmenu.Tables[0].DefaultView;
                dd_mainmenu.DataBind();
            }
        }

        public void Get_Zone()
        {
            DataSet ds_zone = b_fun.Get_AllZones1(clvar);

            dd_branch.Items.Clear();

            if (ds_zone.Tables[0].Rows.Count != 0)
            {
                dd_zone.DataTextField = "Name";
                dd_zone.DataValueField = "zoneCode";
                dd_zone.DataSource = ds_zone.Tables[0].DefaultView;
                dd_zone.DataBind();
            }
            //     dd_zone.Items.Insert(0, new ListItem("Select Zone", ""));
        }

        protected void branch_SelectedIndexChanged(object sender, EventArgs e)
        {
            String ZoneID = "";
            for (int i = 0; i < dd_zone.Items.Count; i++)
            {
                if (dd_zone.Items[i].Selected)
                {
                    ZoneID += dd_zone.Items[i].Value + ',';
                }
            }

            if (ZoneID != "")
            {
                ZoneID = ZoneID.Remove(ZoneID.Length - 1);
                ZoneID.ToString();

                clvar._Zone = ZoneID;
                // clvar._Zone = dd_zone.SelectedValue;
            }

            DataSet ds_branch = b_fun.Get_ZonebyBranches(clvar);

            dd_branch.Items.Clear();
            if (ds_branch.Tables.Count != 0)
            {
                if (ds_branch.Tables[0].Rows.Count != 0)
                {
                    dd_branch.DataTextField = "name";
                    dd_branch.DataValueField = "branchCode";
                    dd_branch.DataSource = ds_branch.Tables[0].DefaultView;
                    dd_branch.DataBind();
                }
            }
        }

        protected void expresscenter_SelectedIndexChanged(object sender, EventArgs e)
        {
            String BrandID = "";
            for (int i = 0; i < dd_branch.Items.Count; i++)
            {
                if (dd_branch.Items[i].Selected)
                {
                    BrandID += dd_branch.Items[i].Value + ',';
                }
            }
            BrandID = BrandID.Remove(BrandID.Length - 1);
            BrandID.ToString();

            clvar._brand = BrandID;

            DataSet ds_expresscenter = b_fun.Get_ExpressCenter(clvar);

            dd_expresscenter.Items.Clear();

            if (ds_expresscenter.Tables[0].Rows.Count != 0)
            {
                //    dd_expresscenter.Items.Add(new ListItem("Select Express Center", "0"));
                dd_expresscenter.DataTextField = "name";
                dd_expresscenter.DataValueField = "expressCenterCode";
                dd_expresscenter.DataSource = ds_expresscenter.Tables[0].DefaultView;
                dd_expresscenter.DataBind();
            }
        }

        protected void childmenu_SelectedIndexChanged(object sender, EventArgs e)
        {
            clvar._menuname = dd_mainmenu.SelectedValue;

            DataSet ds_childmenu = b_fun.Get_ChildMenubyMainMenu(clvar);

            dd_childmenu.Items.Clear();
            if (ds_childmenu.Tables[0].Rows.Count != 0)
            {
                dd_childmenu.Items.Add(new ListItem("Select Child Name", "0"));
                dd_childmenu.DataTextField = "sub_menu_name";
                dd_childmenu.DataValueField = "child_menuid";
                dd_childmenu.DataSource = ds_childmenu.Tables[0].DefaultView;
                dd_childmenu.DataBind();
            }
        }

        protected void Btn_header_Click(object sender, EventArgs e)
        {
            clvar._ProfileName = txt_profile_name.Text.Trim();
            clvar._Zone = dd_zone.SelectedValue;
            clvar._TownCode = dd_branch.SelectedValue;

            String ZoneID = "", ZoneName = "";
            if (chkAll_zone.Checked)
            {
                ZoneName = "All";
                ZoneID = "All";
            }
            else
            {
                for (int i = 0; i < dd_zone.Items.Count; i++)
                {
                    if (dd_zone.Items[i].Selected)
                    {
                        ZoneName += dd_zone.Items[i].Text + ",";
                        ZoneID += dd_zone.Items[i].Value + ",";
                    }
                }

                ZoneName = ZoneName.Remove(ZoneName.Length - 1);
                ZoneName.ToString();

                ZoneID = ZoneID.Remove(ZoneID.Length - 1);
                ZoneID.ToString();
            }

            String BranchID = "", BranchName = "";

            //if (dd_branch.SelectedIndex != -1)
            //{
            if (chkAll_branch.Checked)
            {
                BranchID = "All";
                BranchName = "All";
            }
            else
            {
                for (int i = 0; i < dd_branch.Items.Count; i++)
                {
                    if (dd_branch.Items[i].Selected)
                    {
                        BranchName += dd_branch.Items[i].Text + ",";
                        BranchID += dd_branch.Items[i].Value + ",";
                    }
                }
                BranchName = BranchName.Remove(BranchName.Length - 1);
                BranchName.ToString();

                BranchID = BranchID.Remove(BranchID.Length - 1);
                BranchID.ToString();
            }
            //}
            //else
            //{
            //    BranchName = "";
            //    BranchID = "";
            //}

            String ExpressId = "", ExpressName = "";

            //if (dd_expresscenter.SelectedIndex != -1)
            //{
            for (int i = 0; i < dd_expresscenter.Items.Count; i++)
            {
                if (dd_expresscenter.Items[i].Selected)
                {
                    ExpressName += dd_expresscenter.Items[i].Text + ",";
                    ExpressId += dd_expresscenter.Items[i].Value + ",";
                }
            }
            ExpressName = ExpressName.Remove(ExpressName.Length - 1);
            ExpressName.ToString();

            ExpressId = ExpressId.Remove(ExpressId.Length - 1);
            ExpressId.ToString();
            //}
            //else
            //{
            //    ExpressName = "";
            //    ExpressId = "";
            //}

            GridView.Visible = true;
            txt_profile_name.ReadOnly = true;
            dd_zone.Enabled = false;
            dd_branch.Enabled = false;
            dd_expresscenter.Enabled = false;

            DataTable dt_head = ViewState["dthead"] as DataTable;

            dt_head.Rows.Add(clvar._ProfileName, ZoneID, ZoneName, BranchID, BranchName, ExpressId, ExpressName);

            dt_head.AcceptChanges();
            ViewState["dthead"] = dt_head;
            GridView.DataSource = dt_head;
            GridView.DataBind();

            Button1.Visible = false;
            saveall.Visible = false;
        }

        protected void Btn_detail_Click(object sender, EventArgs e)
        {
            clvar._menuname = dd_mainmenu.SelectedItem.Text;
            string menuid = dd_mainmenu.SelectedValue;

            clvar._childmenu = dd_childmenu.SelectedItem.Text;
            string childid = dd_childmenu.SelectedValue;


            GridView2.Visible = true;

            DataTable dt_head = ViewState["dthead"] as DataTable;
            DataTable dt = ViewState["dt"] as DataTable;

            clvar._ProfileName = dt_head.Rows[0]["profile_name"].ToString();

            dt.Rows.Add(clvar._ProfileName, DateTime.Now, '1', menuid, clvar._menuname, childid, clvar._childmenu);

            dt.AcceptChanges();
            ViewState["dt"] = dt;
            GridView2.DataSource = dt;
            GridView2.DataBind();

            saveall.Visible = true;
        }

        protected void Btn_Save_All_Click(object sender, EventArgs e)
        {
            DataTable dt_head = ViewState["dthead"] as DataTable;
            DataTable dt = ViewState["dt"] as DataTable;

            clvar._ProfileName = dt_head.Rows[0]["Profile_Name"].ToString();
            clvar._Zone = dt_head.Rows[0]["ZoneCode"].ToString();
            clvar._TownCode = dt_head.Rows[0]["BranchCode"].ToString();
            clvar._Expresscentercode = dt_head.Rows[0]["ExpressCenterCode"].ToString();

            b_fun.Insert_Profile_Head(clvar);

            DataSet ds = b_fun.Get_Profile_Max_Id(clvar);
            clvar._Id = ds.Tables[0].Rows[0]["Profile_Id"].ToString();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                clvar._menuname = dt.Rows[i]["MainMenu_Id"].ToString();
                clvar._childmenu = dt.Rows[i]["ChildMenu_Id"].ToString();
                b_fun.Insert_Profile_Detail(clvar);
            }

            GridView.Visible = false;
            dd_zone.Enabled = true;
            txt_profile_name.Text = "";
            dd_childmenu.Items.Clear();
            dd_branch.Items.Clear();
            dd_expresscenter.Items.Clear();
        }


        public void Get_SingleProfile()
        {
            clvar._Id = Request.QueryString["id"];

            DataSet ds = new DataSet();

            ds = b_fun.Get_SingleRecord(clvar);


            if (ds.Tables[0].Rows.Count != 0)
            {

                txt_profile_name.Text = ds.Tables[0].Rows[0]["PROFILE_NAME"].ToString();
                /*
                txt_Vehicle_code.Text = ds.Tables[0].Rows[0]["VEHICLE"].ToString();

                dd_VehicleType.SelectedValue = ds.Tables[0].Rows[0]["VEHICLE_TYPE"].ToString();
                dd_nature.SelectedValue = ds.Tables[0].Rows[0]["NATURE"].ToString();
                txt_RegNo.Text = ds.Tables[0].Rows[0]["REG_NO"].ToString();
                txt_ChassisNo.Text = ds.Tables[0].Rows[0]["CHASSIS_NO"].ToString();
                txt_EngineNo.Text = ds.Tables[0].Rows[0]["ENGIN_NO"].ToString();
                txt_Color.Text = ds.Tables[0].Rows[0]["COLOR"].ToString();
                txt_Make.Text = ds.Tables[0].Rows[0]["MAKE"].ToString();
                txt_Model.Text = ds.Tables[0].Rows[0]["YEAR_MFG"].ToString();
                //  txt_OwnerName1.Text = ds.Tables[0].Rows[0]["OWNER_NAME"].ToString();
                //  txt_OwnerNIC.Text = ds.Tables[0].Rows[0]["OWNER_NIC"].ToString();
                zone.SelectedValue = ds.Tables[0].Rows[0]["ZONE"].ToString();
                type.SelectedValue = ds.Tables[0].Rows[0]["VT"].ToString();
                invdate.Text = ds.Tables[0].Rows[0]["INV_DATE"].ToString();
                cost.Text = ds.Tables[0].Rows[0]["VEHICLE_COST"].ToString();
                regcost.Text = ds.Tables[0].Rows[0]["REG_COST"].ToString();
                routediv.Text = ds.Tables[0].Rows[0]["ROUTE_DIV"].ToString();
                //  grade.Text = ds.Tables[0].Rows[0]["GRADE"].ToString();
                //  grade.SelectedValue = ds.Tables[0].Rows[0]["GRADE"].ToString();
                lessor.SelectedValue = ds.Tables[0].Rows[0]["LESSOR"].ToString();
                policy_no.Text = ds.Tables[0].Rows[0]["POLICY_NO"].ToString();
                maturity.Text = ds.Tables[0].Rows[0]["MATURITY"].ToString();
                reg_book.Text = ds.Tables[0].Rows[0]["REG_BOOK"].ToString();
                tax_due_date.Text = ds.Tables[0].Rows[0]["TAX_DUE_DATE"].ToString();
                rout_permit.Text = ds.Tables[0].Rows[0]["ROUT_PERMIT"].ToString();
                rout_permit_exp.Text = ds.Tables[0].Rows[0]["ROUT_PERMIT_DATE_EXP"].ToString();
                 * */
            }





        }


        protected void btn_Uncheck_Click(object sender, EventArgs e)
        {
            //for (int i = 0; i < dd_zone.Items.Count; i++)
            //{
            //    if (dd_zone.Items[i].Selected == true)
            //    {
            //       dd_zone.Items[i].Selected == true;
            //    }
            //    else
            //    {
            //      //  dd_zone.Items[i].Selected == true;
            //    }
            //}

            foreach (ListItem li in dd_zone.Items)
            {
                li.Selected = false;
            }
        }
        protected void btn_check_Click(object sender, EventArgs e)
        {
            //for (int i = 0; i < dd_zone.Items.Count; i++)
            //{
            //    if (dd_zone.Items[i].Selected == true)
            //    {
            //        dd_zone.Items[i].Selected == true;
            //    }
            //    else
            //    {
            //        //  dd_zone.Items[i].Selected == true;
            //    }
            //}

            //foreach (ListItem li in dd_zone.Items)
            //{
            //    if (li.Selected = true)
            //    {
            //        li.Selected = true;
            //    }
            //    else
            //    {
            //        li.Selected = false;
            //    }

            //}
        }
    }
}