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
    public partial class RiderEntryForm_ : System.Web.UI.Page
    {
        Consignemnts con = new Consignemnts();
        Cl_Variables clvar = new Cl_Variables();
        CommonFunction func = new CommonFunction();

        //shaheer 25/05/2017                         
        public string Strcon()
        {
            string QueryString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString_Test"].ToString();
            return QueryString;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                getDept();
                getCID();
                getDutyType();
                getZones();
                getCities();
                Get_ShiftInformation();
            }
            lbl_error.Visible = false;
        }

        private void getDept()
        {
            DataTable dt = Get_depts();
            if (dt.Rows.Count != 0)
            {
                ddl_dept.DataSource = dt;
                ddl_dept.DataTextField = "AttributeValue";
                ddl_dept.DataValueField = "Id";

                ddl_dept.DataBind();
                ddl_dept.Items.Insert(0, "Select Dept");
            }
        }

        public void Get_ShiftInformation()
        {
            DataTable dt = Get_shifts().Tables[0];
            if (dt.Rows.Count != 0)
            {
                ddl_Shift.DataSource = dt;
                ddl_Shift.DataTextField = "AttributeValue";
                ddl_Shift.DataValueField = "Id";

                ddl_Shift.DataBind();
                ddl_Shift.Items.Insert(0, "Select Shift");
            }

        }

        private void getZones()
        {
            DataTable dt = func.GetAllZones();
            if (dt.Rows.Count != 0)
            {
                ddl_zoneId.DataSource = dt;
                ddl_zoneId.DataTextField = "name";
                ddl_zoneId.DataValueField = "zoneCode";

                ddl_zoneId.DataBind();
                ddl_zoneId.Items.Insert(0, "Select Zone");
            }
        }

        private void getBracnhes()
        {
            clvar.Zone = ddl_zoneId.SelectedValue.ToString();
            DataSet dt = Get_Branches(clvar);
            if (dt.Tables[0].Rows.Count != 0)
            {
                ddl_branchId.DataSource = dt.Tables[0];
                ddl_branchId.DataTextField = "BName";
                ddl_branchId.DataValueField = "branchCode";

                ddl_branchId.DataBind();
                ddl_branchId.Items.Insert(0, "Select Branch");
            }
            else
            {
                ddl_branchId.Items.Clear();
                ddl_branchId.Items.Insert(0, "Select Branch");

                ddl_expressCenterId.Items.Clear();
                ddl_expressCenterId.Items.Insert(0, "Select EXPRESS CENTER");
            }

        }

        public DataSet Get_Branches(Cl_Variables clvar)
        {
            DataSet Ds_1 = new DataSet();

            try
            {
                string query = "SELECT b.branchCode, \n"
               + "       b.name     BranchName, sname BName \n"
               + "FROM   Branches                          b \n"
               + "where b.[status] ='1' and zoneCode ='" + clvar.Zone + "' \n"
               + "GROUP BY \n"
               + "       b.branchCode, \n"
               + "       b.name,sname order by b.sname ASC";

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

        private DataTable Get_depts()
        {
            DataTable Ds_1 = new DataTable();

            try
            {
                string query = "select * from RVDBO.Lookup l where l.AttributeGroup = 'Rider_Department' order by 1";

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

        public DataSet Get_shifts()
        {
            DataSet Ds_1 = new DataSet();

            try
            {
                string sql = "SELECT l.Id,l.AttributeValue \n"
                 + "  FROM rvdbo.Lookup l WHERE l.AttributeGroup ='RIDER_SHIFT'";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sql, orcl);
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

        private void getDutyType()
        {
            DataTable dt = GetDutyType();
            if (dt.Rows.Count != 0)
            {
                ddl_dutyType.DataSource = dt;
                ddl_dutyType.DataTextField = "AttributeValue";
                ddl_dutyType.DataValueField = "id";

                ddl_dutyType.DataBind();
                ddl_dutyType.Items.Insert(0, "Select CID");
            }
        }

        public DataTable GetDutyType()
        {
            DataTable Ds_1 = new DataTable();

            try
            {
                string query = "select l.id,AttributeValue from rvdbo.Lookup l where l.AttributeGroup = 'RIDER_DUTY_TYPE' ";

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

        private void getCities()
        {
            Cl_Variables clvar = new Cl_Variables();
            clvar.Branch = ddl_branchId.SelectedValue;
            DataTable dt = Get_Cities(clvar);
            if (dt.Rows.Count != 0)
            {
                ddl_city.DataSource = dt;
                ddl_city.DataTextField = "CityName";
                ddl_city.DataValueField = "ID";

                ddl_city.DataBind();
                ddl_city.Items.Insert(0, "Select City");
            }
        }

        public DataTable Get_Cities(Cl_Variables clvar)
        {
            string query = "SELECT c.* FROM Branches b INNER JOIN Cities c ON c.id = b.cityId AND b.branchCode='" + clvar.Branch + "'";
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

        private void getCID()
        {
            DataTable dt = GetCID();
            if (dt.Rows.Count != 0)
            {
                ddl_cid.DataSource = dt;
                ddl_cid.DataTextField = "AttributeValue";
                ddl_cid.DataValueField = "id";

                ddl_cid.DataBind();
                ddl_cid.Items.Insert(0, "Select Rider Type");
            }
        }


        public DataTable GetCID()
        {
            DataTable Ds_1 = new DataTable();

            try
            {
                string query = "select l.id,AttributeValue from rvdbo.Lookup l where l.AttributeGroup = 'RIDER_TYPE' ";

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

        private void getExpresscCenter()
        {
            string branch = ddl_branchId.SelectedValue;
            DataTable dt = ExpressCenter_(branch);

            if (dt.Rows.Count != 0)
            {
                ddl_expressCenterId.DataSource = dt;
                ddl_expressCenterId.DataTextField = "name";
                ddl_expressCenterId.DataValueField = "expressCenterCode";

                ddl_expressCenterId.DataBind();
                ddl_expressCenterId.Items.Insert(0, "Select EXPRESS CENTER");
            }
            else
            {
                ddl_expressCenterId.Items.Clear();
                ddl_expressCenterId.Items.Insert(0, "Select EXPRESS CENTER");
            }
        }

        public DataTable ExpressCenter_(string branch)
        {
            DataTable Ds_1 = new DataTable();

            try
            {
                string query = "SELECT * FROM ExpressCenters ec WHERE ec.bid  = '" + branch + "' ";

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

        protected void btn_save_Click(object sender, EventArgs e)
        {
            if (txt_FirstName.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter First Name')", true);
                return;
            }
            if (txt_LastName.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter Last Name')", true);
                return;
            }
            if (txt_MiddleName.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter Middle Name')", true);
                return;
            }
            if (txt_CNIC.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter CNIC')", true);
                return;
            }

            if (this.ddl_cid.SelectedValue == "0")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Select Rider Type')", true);
                return;
            }
            if (txt_address.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter Rider Address')", true);
                return;
            }
            if (this.txt_phoneNo.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter Rider Phone No')", true);
                return;
            }
            if (this.txt_phoneNo.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter Rider Phone No')", true);
                return;
            }
            if (ddl_dutyType.SelectedValue == "0")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Select Duty Type')", true);
                return;
            }
            if (this.ddl_Shift.SelectedValue == "0")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Select Shift')", true);
                return;
            }
            if (this.txt_routeCode.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter Route Code')", true);
                return;
            }
            if (this.ddl_city.SelectedValue == "0")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Select City')", true);
                return;
            }
            if (this.txt_routeCode.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter Route Code')", true);
                return;
            }
            if (this.txt_routeName.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter Route Name')", true);
                return;
            }
            if (this.txt_hrs_Code.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter HRS')", true);
                return;
            }
            if (this.txt_DOB.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter Date of Birth')", true);
                return;
            }
            if (this.txt_DOj.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter Date of Joining')", true);
                return;
            }
            if (this.ddl_dept.SelectedValue == "Select Dept")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Select Dept')", true);
                return;
            }


            string riderCode = txt_riderCode.Text;
            string Branch = ddl_branchId.SelectedValue.ToString();

            DataTable dt = getRider(riderCode, Branch);
            if (dt.Rows.Count > 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Rider Already Exist')", true);
                return;
            }
            else
            {

            }

            int checker = insertRider();

            if (checker == 0)
            {
                if (routecheck == false)
                {
                    int checker_ = insertRoute();
                    if (checker_ == 0)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "redirect", "alert('Rider Inserted!!'); window.location='" + Request.FilePath + "';", true);
                        string script_ = @"window.open(""{0}"", ""{1}"", ""{2}"");";
                        //string script = String.Format(script_, "RunsheetInvoice.aspx?Xcode=" + temp_, "_blank", "");
                        string script = String.Format(script_, "Rider_print.aspx?RiderCode=" + txt_riderCode.Text + "&Bcode=" + ddl_branchId.SelectedValue, "_blank", "");
                        ScriptManager.RegisterClientScriptBlock(this.Page, typeof(Page), "Redirect", script, true);

                    }
                }


            }
            else
            {
                lbl_error.Text = "Insertion Failed";
                lbl_error.Visible = true;
            }

            //Refresh
            txt_FirstName.Text = "";
            txt_LastName.Text = "";
            txt_MiddleName.Text = "";
            txt_CNIC.Text = "";
            this.ddl_cid.SelectedValue.Equals("");
            txt_address.Text = "";
            this.txt_phoneNo.Text = "";
            this.txt_phoneNo.Text = "";
            ddl_dutyType.SelectedValue.Equals("");
            this.ddl_Shift.SelectedValue.Equals("");
            this.txt_routeCode.Text = "";
            this.ddl_city.SelectedValue.Equals("");
            this.txt_routeName.Text = "";
            this.txt_hrs_Code.Text = "";
            this.txt_DOB.Text = "";
            this.txt_DOj.Text = "";
            this.ddl_dept.SelectedValue.Equals("");


        }

        private DataTable getRider(string riderCode, string Branch)
        {
            DataTable dt = new DataTable();

            string sql = "Select * from Riders where riderCode = '" + riderCode + "' and  branchID = '" + Branch + "'";
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sql, con);
                sda.Fill(dt);

            }
            catch (Exception)
            {

                throw;
            }

            return dt;

        }

        private int insertRoute()
        {
            int i = 0;

            string query = "Insert into Routes (routeCode,name,status,description,createdBy,createdOn,cityId,RiderCode,BID) values ('" + txt_routeCode.Text + "','" + txt_routeName.Text + "','1','" + ddl_branchId.SelectedItem.ToString() + "','" + Session["U_ID"].ToString() + "',GetDate(),'" + ddl_city.SelectedValue.ToString() + "','" + txt_riderCode.Text + "','" + ddl_branchId.SelectedValue + "') ";


            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            { i = 1; }
            finally { con.Close(); }

            return i;
        }

        private string getMaxRider()
        {
            string query = "SELECT Max(r.riderCode) FROM Riders r ";
            string getValue = "";
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                getValue = cmd.ExecuteScalar().ToString();

            }
            catch (Exception ex)
            { getValue = ""; }
            finally { con.Close(); }

            return getValue;
        }

        private int insertRider()
        {
            int i = 0;


            string query = "Insert into Riders (firstName,lastName,CNIC,cid,address,phoneNo,dutyTypeId,userTypeId,status,email,riderCode,routeCode,lastrouteCode,expressCenterId,zoneId,branchId,Shift,Hrs_code,DOB,DOJ,MiddleName,department,createdOn,createdby) values ('" + txt_FirstName.Text + "','" + txt_LastName.Text + "','" + txt_CNIC.Text + "','" + ddl_cid.SelectedValue.ToString() + "','" + txt_address.Text + "','" + txt_phoneNo.Text + "','" + ddl_dutyType.SelectedValue.ToString() + "','" + ddl_cid.SelectedValue.ToString() + "','1',\n" +
            " '" + txt_email.Text + "','" + txt_riderCode.Text + "','" + txt_routeCode.Text + "','" + txt_lastrouteCode.Text + "','" + ddl_expressCenterId.SelectedValue.ToString() + "','" + ddl_zoneId.SelectedValue.ToString() + "','" + ddl_branchId.SelectedValue.ToString() + "','" + ddl_Shift.SelectedValue.ToString() + "','" + txt_hrs_Code.Text + "','" + txt_DOB.Text + "','" + txt_DOj.Text + "','" + txt_MiddleName.Text + "','" + ddl_dept.SelectedValue.ToString() + "',getdate(),'" + Session["U_ID"].ToString() + "') ";

            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            { i = 1; }
            finally { con.Close(); }

            return i;
        }
        protected void ddl_zoneId_SelectedIndexChanged(object sender, EventArgs e)
        {
            getBracnhes();
        }
        protected void ddl_branchId_SelectedIndexChanged(object sender, EventArgs e)
        {
            getCities();
            getExpresscCenter();
        }
        protected void ddl_expressCenterId_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        protected void txt_riderCode_TextChanged(object sender, EventArgs e)
        {
            clvar = new Cl_Variables();
            clvar.riderCode = txt_riderCode.Text;
            clvar.Branch = ddl_branchId.SelectedValue;

            DataSet ds = new DataSet();
            string sql = "Select * from Riders where riderCode = '" + txt_riderCode.Text + "' and  branchID = '" + ddl_branchId.SelectedValue + "'";
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                SqlDataAdapter sda = new SqlDataAdapter(sql, con);
                sda.Fill(ds);
            }
            catch (Exception Err)
            {

            }

            if (ds.Tables[0].Rows.Count != 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Rider Code Already Present')", true);

                lb_riderlink.Text = "View Report";
                lb_riderlink.NavigateUrl = "Rider_Print.aspx?RiderCode=" + txt_riderCode.Text + "&BCode=" + ddl_branchId.SelectedValue;
            }
            else
            {
                lb_riderlink.Text = "";
                lb_riderlink.NavigateUrl = "";
            }
        }

        bool routecheck = false;
        protected void txt_routeCode_TextChanged(object sender, EventArgs e)
        {

            DataSet ds = new DataSet();
            string sql = "SELECT * \n"
                  + "FROM   Routes r \n"
                  + "       INNER JOIN Cities c \n"
                  + "            ON  r.cityId = c.id \n"
                  + "       INNER JOIN Branches b  \n"
                  + "		    ON b.cityId = c.id \n"
                  + "WHERE r.routeCode ='" + txt_routeCode.Text + "' AND b.branchCode='" + ddl_branchId.SelectedValue + "' and r.status ='1'";
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                SqlDataAdapter sda = new SqlDataAdapter(sql, con);
                sda.Fill(ds);
            }
            catch (Exception Err)
            {

            }

            if (ds.Tables[0].Rows.Count != 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Route Already Present')", true);
                routecheck = true;
            }
            else
            {
                routecheck = false;
            }
        }
    }
}