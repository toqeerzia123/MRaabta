using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using MRaabta.App_Code;
using System.Configuration;

namespace MRaabta.Files
{
    public partial class SwitchRetailStaff : System.Web.UI.Page
    {
        String zone = "";
        String branch = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                String retailDesktop = "";
                //if (Session["RetailDesktop"] != null)
                //{
                //    retailDesktop = Session["RetailDesktop"].ToString();
                //    if (retailDesktop != "123456")
                //    {
                //        Response.Redirect("~/login");
                //    }
                //}
                //else
                //{
                //    Response.Redirect("~/login");
                //}
                zone = Session["ZONECODE"].ToString();
                branch = Session["BRANCHCODE"].ToString();
                if (!IsPostBack)
                {

                    Zone();
                    Branch();
                    ExpressCenter(branch);
                    LoadEmpFromZNI();
                    LoadDSGcodeFromZNI();
                    LoadStaff();

                }
            }
            catch (Exception er)
            {
                Response.Redirect("~/login");
            }
        }

        private void Branch()
        {
            DataTable Ds_1 = new DataTable();

            string query = "";
            try
            {
                if (zone == "ALL")
                {
                    query = "  SELECT b.branchCode,  \n"
              + "       b.name     BranchName FROM   Branches b  \n"
              + " where b.[status] ='1'  \n"
              + "order by b.name ASC";

                }
                else if (branch == "ALL")
                {

                    query = "  SELECT b.branchCode,  \n"
              + "       b.name     BranchName FROM   Branches b  \n"
              + " where b.[status] ='1' and b.zoneCode in (" + ddl_zoneId.SelectedValue.ToString() + ")   \n"
              + " order by b.name ASC";
                }
                else
                {

                    query = "  SELECT b.branchCode,  \n"
              + "       b.name     BranchName FROM   Branches b  \n"
              + " where b.[status] ='1' and b.branchcode in (" + branch + ")   \n"
              + " order by b.name ASC";
                }


                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(Ds_1);
                orcl.Close();


                if (Ds_1.Rows.Count != 0)
                {
                    dd_Branch.DataTextField = "BranchName";
                    dd_Branch.DataValueField = "branchCode";
                    dd_Branch.DataSource = Ds_1.DefaultView;
                    dd_Branch.DataBind();
                }
            }
            catch (Exception Err)
            {
            }
            finally
            { }

        }

        private void LoadDSGcodeFromZNI()
        {
            EmpCodeTxtBox.Text = UsernameDropdown.SelectedValue;
        }
        private void LoadEmpFromZNI()
        {
            DataTable Ds_1 = new DataTable();
            string query = "";
            try
            {
                if (zone == "ALL")
                {
                    query = "SELECT U_NAME,DSG_CODE FROM ZNI_USER1 WHERE bts_User='1' AND PROFILE='4'  AND STATUS=1 ORDER BY U_NAME";
                }
                else if (branch == "ALL")
                {
                    query = "SELECT U_NAME,DSG_CODE FROM ZNI_USER1 WHERE bts_User='1' AND PROFILE='4' AND ZoneCode in (" + zone + ")  AND STATUS=1 ORDER BY U_NAME";
                }
                else
                {
                    query = "SELECT U_NAME,DSG_CODE FROM ZNI_USER1 WHERE bts_User='1' AND PROFILE='4' AND ZoneCode in (" + zone + ") and branchcode in (" + branch + ") AND STATUS=1 ORDER BY U_NAME";
                }
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(Ds_1);
                orcl.Close();


                if (Ds_1.Rows.Count != 0)
                {
                    this.UsernameDropdown.DataTextField = "U_NAME";
                    this.UsernameDropdown.DataValueField = "DSG_CODE";
                    this.UsernameDropdown.DataSource = Ds_1.DefaultView;
                    this.UsernameDropdown.DataBind();
                }
            }
            catch (Exception Err)
            {
            }
            finally
            { }
        }

        public void LoadEmpFromZNIBranchChange()
        {
            DataTable Ds_1 = new DataTable();
            string query = "";
            try
            {
                query = "SELECT U_NAME,DSG_CODE FROM ZNI_USER1 WHERE bts_User='1' AND PROFILE='4' and branchcode=" + dd_Branch.SelectedValue.ToString() + "  AND STATUS=1 ORDER BY U_NAME";
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(Ds_1);
                orcl.Close();

                if (Ds_1.Rows.Count != 0)
                {
                    this.UsernameDropdown.DataTextField = "U_NAME";
                    this.UsernameDropdown.DataValueField = "DSG_CODE";
                    this.UsernameDropdown.DataSource = Ds_1.DefaultView;
                    this.UsernameDropdown.DataBind();
                }
                else
                {
                    UsernameDropdown.Items.Clear();
                }
            }
            catch (Exception Err)
            {
            }
            finally
            { }
        }
        Cl_Variables clvar = new Cl_Variables();

        public void LoadStaff()
        {
            String zone = Session["ZONECODE"].ToString();
            string query = "";
            DataTable Ds_1 = new DataTable();

            try
            {
                if (zone == "ALL")
                {
                    query = "SELECT mprs.BookingStaff UserId, z.name Zone, b.name Branch, ec.name+' ('+ec.expressCenterCode+')' ExpressCenterCode, mprs.BookingStaff,mprs.shift,mprs.employeeUsername   FROM MnP_Retail_Staff mprs " +
                    " INNER JOIN ZNI_USER1 zni ON zni.U_ID=mprs.UserId " +
                    " INNER JOIN Zones z ON z.zoneCode = mprs.Zone " +
                    " INNER JOIN Branches b ON b.branchCode = mprs.Branch " +
                    " INNER JOIN ExpressCenters ec ON ec.expressCenterCode = mprs.ExpressCenterCode " +
                    " where mprs.status = 1 ORDER BY mprs.employeeUsername,mprs.Zone,mprs.BookingStaff";

                }
                else if (branch == "ALL")
                {
                    query = "SELECT mprs.BookingStaff UserId, z.name Zone, b.name Branch, ec.name+' ('+ec.expressCenterCode+')' ExpressCenterCode, mprs.BookingStaff,mprs.shift,mprs.employeeUsername   FROM MnP_Retail_Staff mprs " +
                    " INNER JOIN ZNI_USER1 zni ON zni.U_ID=mprs.UserId " +
                    "INNER JOIN Zones z ON z.zoneCode = mprs.Zone " +
                    "INNER JOIN Branches b ON b.branchCode = mprs.Branch " +
                    "INNER JOIN ExpressCenters ec ON ec.expressCenterCode = mprs.ExpressCenterCode " +
                    "where mprs.status = 1 and mprs.Zone in (" + zone + ") ORDER BY mprs.employeeUsername,mprs.Zone,mprs.BookingStaff";
                }
                else
                {
                    query = "SELECT mprs.BookingStaff UserId, z.name Zone, b.name Branch, ec.name+' ('+ec.expressCenterCode+')' ExpressCenterCode, mprs.BookingStaff,mprs.shift,mprs.employeeUsername   FROM MnP_Retail_Staff mprs " +
                    " INNER JOIN ZNI_USER1 zni ON zni.U_ID=mprs.UserId " +
                   "INNER JOIN Zones z ON z.zoneCode = mprs.Zone " +
                   "INNER JOIN Branches b ON b.branchCode = mprs.Branch " +
                   "INNER JOIN ExpressCenters ec ON ec.expressCenterCode = mprs.ExpressCenterCode " +
                   "where mprs.status = 1 and mprs.Branch in (" + branch + ") ORDER BY mprs.employeeUsername,mprs.Zone,mprs.BookingStaff";
                }

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(Ds_1);
                orcl.Close();


                if (Ds_1.Rows.Count != 0)
                {
                    RetailStaffGrid.DataSource = Ds_1.DefaultView;
                    RetailStaffGrid.DataBind();

                }
            }
            catch (Exception Err)
            {

            }
            finally
            { }
        }

        protected void GridView2_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            RetailStaffGrid.PageIndex = e.NewPageIndex;

        }
        public void Zone()
        {
            String zone = Session["ZONECODE"].ToString();

            DataTable Ds_1 = new DataTable();
            string query = "";
            try
            {
                if (zone == "ALL")
                {
                    query = "SELECT z.ZoneCode, z.name    ZoneName" +
                    " FROM" +
                    " zones z " +
                    " where z.[status] = 1 " +
                    "  AND z.Region IS NOT NULL " +
                    " order by z.name ASC";
                }
                else
                {
                    query = "SELECT z.ZoneCode, z.name    ZoneName" +
                    " FROM" +
                    " zones z " +
                    " where z.[status] = 1 and z.zoneCode in (" + zone +
                    ")  AND z.Region IS NOT NULL " +
                    " order by z.name ASC";
                }


                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(Ds_1);
                orcl.Close();


                if (Ds_1.Rows.Count != 0)
                {
                    //   DataTable dt = Cities_();
                    this.ddl_zoneId.DataTextField = "ZoneName";
                    this.ddl_zoneId.DataValueField = "ZoneCode";
                    this.ddl_zoneId.DataSource = Ds_1.DefaultView;
                    this.ddl_zoneId.DataBind();

                }
            }
            catch (Exception Err)
            {
            }
            finally
            { }

        }


        protected void ddl_zoneId_SelectedIndexChanged(object sender, EventArgs e)
        {
            Branch(ddl_zoneId.SelectedValue.ToString());
            ExpressCenter(dd_Branch.SelectedValue.ToString());
            LoadEmpFromZNIBranchChange();

        }
        protected void ddl_Username_SelectedIndexChanged(object sender, EventArgs e)
        {
            EmpCodeTxtBox.Text = UsernameDropdown.SelectedValue.ToString();
        }
        protected void ddl_branchId_SelectedIndexChanged(object sender, EventArgs e)
        {
            String branch = dd_Branch.SelectedValue.ToString();
            ExpressCenter(branch);
            LoadEmpFromZNIBranchChange();

        }

        public void ExpressCenter(String branchCode)
        {
            DataTable Ds_1 = new DataTable();
            String branch = dd_Branch.SelectedValue.ToString();

            try
            {

                string query = "SELECT ec.name+' ('+ec.expressCenterCode+')' name,ec.expressCenterCode FROM ExpressCenters ec WHERE " +
                    "ec.bid = '" + branch + "' AND ec.[status]=1 ORDER BY ec.name";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(Ds_1);
                orcl.Close();

                if (Ds_1.Rows.Count != 0)
                {
                    //   DataTable dt = Cities_();
                    this.dd_eclist.DataTextField = "name";
                    this.dd_eclist.DataValueField = "expressCenterCode";
                    this.dd_eclist.DataSource = Ds_1.DefaultView;
                    this.dd_eclist.DataBind();

                }
                else
                {
                    dd_eclist.Items.Clear();
                }

            }
            catch (Exception Err)
            {
            }
            finally
            { }

        }
        public void Branch(String branch)
        {
            DataTable Ds_1 = new DataTable();
            String ZoneId = ddl_zoneId.SelectedValue.ToString();
            try
            {
                string query = "";

                query = "SELECT b.branchCode, \n"
              + "       b.name     BranchName, sname BName \n"
              + " FROM   Branches                          b \n"
              + " where b.[status] ='1' and b.zoneCode in (" + ZoneId + ")  \n"
              + " GROUP BY \n"
              + "       b.branchCode, \n"
              + "       b.name,sname order by b.name ASC";


                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(Ds_1);
                orcl.Close();
                //dd_Branch.Items.Clear();

                if (Ds_1.Rows.Count != 0)
                {
                    //   DataTable dt = Cities_();
                    this.dd_Branch.DataTextField = "BranchName";
                    this.dd_Branch.DataValueField = "branchCode";
                    this.dd_Branch.DataSource = Ds_1.DefaultView;
                    this.dd_Branch.DataBind();
                }

            }
            catch (Exception Err)
            {
            }
            finally
            { }

        }

        protected void btn_Save_Staff_Click(object sender, EventArgs e)
        {
            if (ddl_zoneId.SelectedValue == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "aletMessage", "alert('Please select a zone')", true);
                return;

            }
            if (dd_Branch.SelectedValue == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "aletMessage", "alert('Please select a branch')", true);
                return;
            }
            if (dd_eclist.SelectedValue == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "aletMessage", "alert('Please select an express center')", true);
                return;
            }
            if (EmpCodeTxtBox.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "aletMessage", "alert('Please provde employee code')", true);
                return;

            }


            if (UsernameDropdown.SelectedItem.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "aletMessage", "alert('Please provide username')", true);
                return;
            }
            AddStaffFromZNI(ddl_zoneId.SelectedValue, dd_Branch.SelectedValue, dd_eclist.SelectedValue, EmpCodeTxtBox.Text, /*BookingCodeTxtbox.Text,*/ UsernameDropdown.SelectedItem.Text, shiftDropdown.SelectedValue.ToString());

            Zone();
            Branch();
            ExpressCenter(branch);
            LoadEmpFromZNI();
            LoadDSGcodeFromZNI();
            LoadStaff();

        }


        private void AddStaffFromZNI(string zone, string branch, string ec, string empCode,/* String bookingCode, */ string Username, string shift)
        {
            String U_ID = "";
            Username = Username.Trim();

            DataTable Dt = new DataTable();
            try
            {
                string query = "SELECT zu.U_ID  " +
                    "FROM ZNI_USER1 zu WHERE bts_User='1' AND PROFILE='4' AND STATUS=1 and /*zu.DSG_CODE='" + empCode + "' AND*/ zu.U_NAME='" + Username + "'";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataReader rdr = orcd.ExecuteReader();
                if (rdr.Read())
                {
                    U_ID = rdr.GetInt32(0).ToString();
                    orcl.Close();

                    String status = AddFromZNIToRetailTableUpdateOld(zone, branch, ec, U_ID, empCode,/* bookingCode, */Username, shift);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "aletMessage", "alert('" + status + "')", true);
                    LoadStaff();
                }
                else
                {
                    orcl.Close();

                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "aletMessage", "alert('No employee found.')", true);

                }


            }
            catch (Exception Err)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "aletMessage", "alert('No employee found.')", true);
            }
            finally
            { }

        }

        private String AddFromZNIToRetailTableUpdateOld(string zone, string branch, string expressCenter, string U_ID, string empCode,/*string bookingCode,*/ string username, string shift)
        {
            String status = "";
            using (SqlConnection sqlcon = new SqlConnection((ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString)))
            {
                sqlcon.Open();
                SqlCommand command = sqlcon.CreateCommand();
                SqlTransaction transaction;

                transaction = sqlcon.BeginTransaction("Updating Retail");
                command.Connection = sqlcon;
                command.Transaction = transaction;
                try
                {
                    //////////updating commission entry old to status 0 and inserting new

                    command.CommandText = "  UPDATE MnP_Retail_Staff   SET[Status] = 0,  ModifiedBy = " + Session["U_ID"].ToString() + ",   	ModifiedOn = GETDATE()  WHERE status=1 and EmployeeUsername='" + username + "' and UserId ='" + U_ID + "'";
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();

                    string sqlInsert = " \n"
               + "  INSERT INTO MnP_Retail_Staff \n"
               + "  ( 	UserId,  	Zone,  	Branch,  	ExpressCenterCode, \n"
               + "  	BookingStaff, \n"
               + "  	[Status], \n"
               + "  	CreatedBy," +
               "        EmployeeUsername, \n"
               + "  	CreatedOn, \n"
               + "      shift      "
               + "  ) \n"
               + "  VALUES \n"
               + "  ( \n"
               + "  	@EmpCode, \n"
               + "  	@zone, \n"
               + "  	@branch, \n"
               + "  	@ExpressC, \n"
               + "  	@bookingStaff, \n"
               + "  	1, \n"
               + "  	@CreatedBy,@EmployeeUsername, \n"
               + "  	GETDATE(), \n"
               + "      @shift   "
               + "  )";
                    command.CommandText = sqlInsert;
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@EmpCode", U_ID);
                    command.Parameters.AddWithValue("@zone", zone);
                    command.Parameters.AddWithValue("@branch", branch);
                    command.Parameters.AddWithValue("@ExpressC", expressCenter);
                    command.Parameters.AddWithValue("@bookingStaff", empCode);
                    command.Parameters.AddWithValue("@CreatedBy", Session["U_ID"].ToString());
                    command.Parameters.AddWithValue("@shift", shift);
                    command.Parameters.AddWithValue("@EmployeeUsername", username);

                    command.ExecuteNonQuery();
                    transaction.Commit();
                    status = "Record Added Successfully";
                }
                catch (Exception ex)
                {
                    try
                    {
                        transaction.Rollback();
                        clvar.Error = "Error inserting retail staff record, " + ex.Message.ToString();
                        status = clvar.Error;
                    }
                    catch (Exception ex2)
                    {
                        clvar.Error = "Error inserting retail staff record, rollback transaction failed " + ex2.Message.ToString();
                        status = clvar.Error;
                    }
                }
                finally
                { sqlcon.Close(); }
                return status;
            }
        }

        protected void GridView1_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        {
            try
            {

                string query = "";
                //NewEditIndex property used to determine the index of the row being edited.  
                RetailStaffGrid.EditIndex = e.NewEditIndex;
                Label zoneIdPrev = RetailStaffGrid.Rows[e.NewEditIndex].FindControl("lbl_Zone") as Label;
                Label branchIdPrev = RetailStaffGrid.Rows[e.NewEditIndex].FindControl("lbl_Branch") as Label;
                Label ECIdPrev = RetailStaffGrid.Rows[e.NewEditIndex].FindControl("lbl_ExpressCenterCode") as Label;

                LoadStaff();
                DataTable Ds_1 = new DataTable();
                if (zone == "ALL")
                {
                    query = "SELECT z.ZoneCode, z.name    ZoneName " +
                       " FROM " +
                       " zones z " +
                       " where z.[status] = 1  " +
                       " AND z.Region IS NOT NULL " +
                       " order by z.name ASC";
                }
                else
                {
                    query = "SELECT z.ZoneCode, z.name    ZoneName" +
                    " FROM" +
                    " zones z " +
                    " where z.[status] = 1 and z.zoneCode in (" + zone +
                    ")  AND z.Region IS NOT NULL " +
                    " order by z.name ASC";
                }


                DropDownList DropDownListZone = (DropDownList)RetailStaffGrid.Rows[e.NewEditIndex].FindControl("Grid_Zone_Dropdown");

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(Ds_1);
                orcl.Close();

                if (Ds_1.Rows.Count != 0)
                {
                    DropDownListZone.DataTextField = "ZoneName";
                    DropDownListZone.DataValueField = "ZoneCode";
                    DropDownListZone.DataSource = Ds_1.DefaultView;
                    DropDownListZone.DataBind();
                    DropDownListZone.SelectedValue = DropDownListZone.Items.FindByText(zoneIdPrev.Text).Value;
                }
                /////////////////branch dropdownlist
                ///

                DataTable Dsbranch = new DataTable();
                string querybranch = "";

                if (branch == "ALL")
                {
                    querybranch = "SELECT b.branchCode, \n"
              + "       b.name     BranchName, sname BName \n"
              + "FROM   Branches                          b \n"
              + "where b.[status] ='1' \n"
              + "GROUP BY \n"
              + "       b.branchCode, \n"
              + "       b.name,sname order by b.name ASC";
                }
                else
                {
                    querybranch = "SELECT b.branchCode, \n"
              + "       b.name     BranchName, sname BName \n"
              + "FROM   Branches                          b \n"
              + "where b.[status] ='1' and b.branchcode in (" + branch + ")  \n"
              + "GROUP BY \n"
              + "       b.branchCode, \n"
              + "       b.name,sname order by b.name ASC";
                }


                DropDownList DropDownListBranch = (DropDownList)RetailStaffGrid.Rows[e.NewEditIndex].FindControl("Grid_Branch_Dropdown");

                SqlConnection orcl2 = new SqlConnection(clvar.Strcon());
                orcl2.Open();
                SqlCommand orcd2 = new SqlCommand(querybranch, orcl2);
                orcd2.CommandType = CommandType.Text;
                SqlDataAdapter oda2 = new SqlDataAdapter(orcd2);
                oda2.Fill(Dsbranch);
                orcl2.Close();

                if (Dsbranch.Rows.Count != 0)
                {
                    DropDownListBranch.DataTextField = "BranchName";
                    DropDownListBranch.DataValueField = "branchCode";
                    DropDownListBranch.DataSource = Dsbranch.DefaultView;
                    DropDownListBranch.DataBind();
                    DropDownListBranch.SelectedValue = DropDownListBranch.Items.FindByText(branchIdPrev.Text).Value;
                }

                /////////////////EC dropdownlist
                ///
                DataTable DsEC = new DataTable();

                string queryEC = "SELECT ec.name+' ('+ec.expressCenterCode+')' name,ec.expressCenterCode FROM ExpressCenters ec WHERE " +
                "ec.bid = '" + DropDownListBranch.SelectedValue.ToString() + "' AND ec.[status]=1 ORDER BY ec.name";

                DropDownList DropDownListEC = (DropDownList)RetailStaffGrid.Rows[e.NewEditIndex].FindControl("Grid_EC_Dropdown");

                SqlConnection orcl3 = new SqlConnection(clvar.Strcon());
                orcl3.Open();
                SqlCommand orcd3 = new SqlCommand(queryEC, orcl3);
                orcd3.CommandType = CommandType.Text;
                SqlDataAdapter oda3 = new SqlDataAdapter(orcd3);
                oda3.Fill(DsEC);
                orcl3.Close();

                if (DsEC.Rows.Count != 0)
                {
                    DropDownListEC.DataTextField = "name";
                    DropDownListEC.DataValueField = "expressCenterCode";
                    DropDownListEC.DataSource = DsEC.DefaultView;
                    DropDownListEC.DataBind();
                    DropDownListEC.SelectedValue = DropDownListEC.Items.FindByText(ECIdPrev.Text).Value;
                }
                else
                {
                    DropDownListEC.Items.Clear();
                }
            }
            catch (Exception er)
            {

            }
        }

        protected void ddl_zone_grid_IndexChange(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            GridViewRow gvr = (GridViewRow)ddl.NamingContainer;
            int rowindex = gvr.RowIndex;

            DropDownList comboBox = (DropDownList)sender;
            string ZoneId = (string)comboBox.SelectedValue;

            DataTable Ds_1 = new DataTable();
            try
            {
                DropDownList DropDownListBranchGrid = (DropDownList)RetailStaffGrid.Rows[rowindex].FindControl("Grid_Branch_Dropdown");

                string query = "SELECT b.branchCode, \n"
               + "       b.name     BranchName, sname BName \n"
               + "FROM   Branches                          b \n"
               + "where b.[status] ='1' and b.zoneCode='" + ZoneId + "'  \n"
               + "GROUP BY \n"
               + "       b.branchCode, \n"
               + "       b.name,sname order by b.name ASC";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(Ds_1);
                orcl.Close();
                //dd_Branch.Items.Clear();

                if (Ds_1.Rows.Count != 0)
                {
                    //   DataTable dt = Cities_();
                    DropDownListBranchGrid.DataTextField = "BranchName";
                    DropDownListBranchGrid.DataValueField = "branchCode";
                    DropDownListBranchGrid.DataSource = Ds_1.DefaultView;
                    DropDownListBranchGrid.DataBind();
                }
                ///get EC
                ///

                DataTable Ds_EC = new DataTable();

                string queryEC = "SELECT ec.name+' ('+ec.expressCenterCode+')' name,ec.expressCenterCode FROM ExpressCenters ec WHERE " +
                "ec.bid = '" + DropDownListBranchGrid.SelectedValue.ToString() + "' AND ec.[status]=1 ORDER BY ec.name";

                DropDownList DropDownListEC = (DropDownList)RetailStaffGrid.Rows[rowindex].FindControl("Grid_EC_Dropdown");
                SqlConnection orcl3 = new SqlConnection(clvar.Strcon());
                orcl3.Open();
                SqlCommand orcd3 = new SqlCommand(queryEC, orcl3);
                orcd3.CommandType = CommandType.Text;
                SqlDataAdapter oda3 = new SqlDataAdapter(orcd3);
                oda3.Fill(Ds_EC);
                orcl3.Close();

                if (Ds_EC.Rows.Count != 0)
                {
                    DropDownListEC.DataTextField = "name";
                    DropDownListEC.DataValueField = "expressCenterCode";
                    DropDownListEC.DataSource = Ds_EC.DefaultView;
                    DropDownListEC.DataBind();
                }
                else
                {
                    DropDownListEC.Items.Clear();

                }

            }
            catch (Exception Err)
            {
            }
            finally
            { }
        }

        protected void ddl_branch_grid_IndexChange(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            GridViewRow gvr = (GridViewRow)ddl.NamingContainer;
            int rowindex = gvr.RowIndex;

            DropDownList comboBox = (DropDownList)sender;
            string branchId = (string)comboBox.SelectedValue;

            DataTable Ds_1 = new DataTable();
            try
            {
                DropDownList DropDownListECGrid = (DropDownList)RetailStaffGrid.Rows[rowindex].FindControl("Grid_EC_Dropdown");

                string query = "SELECT ec.name+' ('+ec.expressCenterCode+')' name,ec.expressCenterCode FROM ExpressCenters ec WHERE " +
                "ec.bid = '" + branchId + "' AND ec.[status]=1 ORDER BY ec.name";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(Ds_1);
                orcl.Close();
                //dd_Branch.Items.Clear();

                if (Ds_1.Rows.Count != 0)
                {
                    //   DataTable dt = Cities_();
                    DropDownListECGrid.DataTextField = "name";
                    DropDownListECGrid.DataValueField = "expressCenterCode";
                    DropDownListECGrid.DataSource = Ds_1.DefaultView;
                    DropDownListECGrid.DataBind();
                }
                else
                {
                    DropDownListECGrid.Items.Clear();
                }

            }
            catch (Exception Err)
            {
            }
            finally
            { }
        }
        protected void GridView1_RowUpdating(object sender, System.Web.UI.WebControls.GridViewUpdateEventArgs e)
        {
            //Finding the controls from Gridview for the row which is going to update  
            Label id = RetailStaffGrid.Rows[e.RowIndex].FindControl("lblRowNumber") as Label;
            DropDownList zone = RetailStaffGrid.Rows[e.RowIndex].FindControl("Grid_Zone_Dropdown") as DropDownList;
            DropDownList branch = RetailStaffGrid.Rows[e.RowIndex].FindControl("Grid_Branch_Dropdown") as DropDownList;
            DropDownList expressCenterCode = RetailStaffGrid.Rows[e.RowIndex].FindControl("Grid_EC_Dropdown") as DropDownList;
            DropDownList shiftddl = RetailStaffGrid.Rows[e.RowIndex].FindControl("Grid_shift_Dropdown") as DropDownList;

            // TextBox bookingStaff = RetailStaffGrid.Rows[e.RowIndex].FindControl("txt_BookingStaff") as TextBox;

            //if (bookingStaff.Text=="")
            //{
            //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "aletMessage", "alert('please provide booking staff code')", true);
            //    return;
            //}
            if (expressCenterCode.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "aletMessage", "alert('please select Express center')", true);
                return;
            }

            String Employee_DSGCODE = RetailStaffGrid.Rows[e.RowIndex].Cells[3].Text;
            String Employee_Username = RetailStaffGrid.Rows[e.RowIndex].Cells[4].Text;
            String Employee_U_ID = "";


            String status = "";
            using (SqlConnection sqlcon = new SqlConnection((ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString)))
            {
                sqlcon.Open();
                SqlCommand command = sqlcon.CreateCommand();
                SqlTransaction transaction;

                transaction = sqlcon.BeginTransaction("Updating staff");
                command.Connection = sqlcon;
                command.Transaction = transaction;
                try
                {
                    ////////updating commission entry old to status 0 and inserting new

                    command.CommandText = "SELECT U_ID FROM ZNI_USER1 zu WHERE zu.U_NAME='" + Employee_Username + "' AND STATUS=1";
                    command.CommandType = CommandType.Text;
                    SqlDataReader rdr1 = command.ExecuteReader();
                    if (rdr1.Read())
                    {
                        Employee_U_ID = rdr1.GetInt32(0).ToString();
                        rdr1.Close();

                    }
                    else
                    {
                        rdr1.Close();

                        throw new Exception();

                    }


                    command.CommandText = "  UPDATE MnP_Retail_Staff   SET[Status] = 0,  ModifiedBy = " + Session["U_ID"].ToString() + ",   	ModifiedOn = GETDATE()  WHERE status=1 and EmployeeUsername='" + Employee_Username + "'";
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();

                    string sqlInsert = " \n"
               + "  INSERT INTO MnP_Retail_Staff \n"
               + "  ( 	UserId,  	Zone,  	Branch,  	ExpressCenterCode, \n"
               + "  	BookingStaff, \n"
               + "  	[Status], \n"
               + "  	CreatedBy," +
               "        EmployeeUsername, \n"
               + "  	CreatedOn,shift \n"
               + "  ) \n"
               + "  VALUES \n"
               + "  ( \n"
               + "  	@EmpCode, \n"
               + "  	@zone, \n"
               + "  	@branch, \n"
               + "  	@ExpressC, \n"
               + "  	@bookingStaff, \n"
               + "  	1, \n"
               + "  	@CreatedBy,@EmployeeUsername, \n"
               + "  	GETDATE(),@shiftGrid \n"
               + "  )";
                    command.CommandText = sqlInsert;
                    command.CommandType = CommandType.Text;
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@EmpCode", Employee_U_ID);
                    command.Parameters.AddWithValue("@zone", zone.SelectedValue.ToString());
                    command.Parameters.AddWithValue("@branch", branch.SelectedValue.ToString());
                    command.Parameters.AddWithValue("@ExpressC", expressCenterCode.SelectedValue.ToString());
                    command.Parameters.AddWithValue("@shiftGrid", shiftddl.SelectedValue.ToString());
                    command.Parameters.AddWithValue("@bookingStaff", Employee_DSGCODE);
                    command.Parameters.AddWithValue("@CreatedBy", Session["U_ID"].ToString());
                    command.Parameters.AddWithValue("@EmployeeUsername", Employee_Username);

                    command.ExecuteNonQuery();
                    transaction.Commit();
                    status = "Record Added Successfully";
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "aletMessage", "alert('Staff Updated Successfully')", true);

                }
                catch (Exception ex)
                {
                    try
                    {
                        transaction.Rollback();
                        clvar.Error = "Error updating retail staff record: " + ex.Message.ToString();
                        status = clvar.Error;
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "aletMessage", "alert('" + status + "')", true);

                    }
                    catch (Exception ex2)
                    {
                        clvar.Error = "Error updating retail staff record, rollback transaction failed " + ex2.Message.ToString();
                        status = clvar.Error;
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "aletMessage", "alert('" + status + "')", true);

                    }
                }
                finally
                { sqlcon.Close(); }

            }
            //Setting the EditIndex property to -1 to cancel the Edit mode in Gridview  
            RetailStaffGrid.EditIndex = -1;
            //Call ShowData method for displaying updated data  
            LoadStaff();

        }

        protected void GridView1_RowCancelingEdit(object sender, System.Web.UI.WebControls.GridViewCancelEditEventArgs e)
        {
            RetailStaffGrid.EditIndex = -1;
            LoadStaff();
        }


        protected void btn_Cancel_Click(object sender, EventArgs e)
        {
            EmpCodeTxtBox.Text = "";
            //BookingCodeTxtbox.Text = "";
            UsernameDropdown.SelectedIndex = 0;
            ddl_zoneId.SelectedIndex = 0;
            dd_Branch.SelectedIndex = 0;
            dd_eclist.SelectedIndex = 0;

        }

    }
}