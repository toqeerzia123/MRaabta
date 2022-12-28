using System;
using System.Web.UI;
using System.Data;
using System.Data.SqlClient;
using MRaabta.App_Code;

namespace MRaabta.Files
{
    public partial class RiderDeactivate_Form : System.Web.UI.Page
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
                getBracnhes();
                getExpresscCenter();
            }
            lbl_error.Visible = false;

        }

        public DataSet Get_shifts()
        {
            DataSet Ds_1 = new DataSet();

            try
            {
                string sql = "SELECT l.Id,l.AttributeDesc \n"
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
        private void getCities(string Brancg)
        {
            Cl_Variables clvar = new Cl_Variables();
            clvar.Branch = Brancg;// ddl_branchId.SelectedValue;
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

        public void Get_ShiftInformation()
        {
            DataTable dt = Get_shifts().Tables[0];
            if (dt.Rows.Count != 0)
            {
                ddl_Shift.DataSource = dt;
                ddl_Shift.DataTextField = "AttributeDesc";
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

        private void getBracnhes()
        {

            DataSet dt = Get_Branches();
            if (dt.Tables[0].Rows.Count != 0)
            {
                ddl_branchId.DataSource = dt.Tables[0];
                ddl_branchId.DataTextField = "BranchName";
                ddl_branchId.DataValueField = "branchCode";

                ddl_branchId.DataBind();
                ddl_branchId.Items.Insert(0, "Select Branch");


                ddl_branch.DataSource = dt.Tables[0];
                ddl_branch.DataTextField = "BName";
                ddl_branch.DataValueField = "branchCode";

                ddl_branch.DataBind();
                ddl_branch.Items.Insert(0, "Select Branch");
            }


        }

        private void getBracnhes_Zone()
        {
            clvar.Zone = ddl_zoneId.SelectedValue.ToString();
            DataSet dt = Get_Branches_zone(clvar);
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

        public DataSet Get_Branches_zone(Cl_Variables clvar)
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

        public DataSet Get_Branches()
        {
            DataSet Ds_1 = new DataSet();

            try
            {
                string query = "SELECT b.branchCode, \n"
               + "       b.name     BranchName, sname BName \n"
               + "FROM   Branches                          b \n"
               + "where b.[status] ='1' \n"
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

            if (this.ddl_cid.SelectedValue == "Select Rider Type")
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
            if (ddl_dutyType.SelectedValue == "Select CID")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Select Duty Type')", true);
                return;
            }
            if (this.ddl_Shift.SelectedValue == "0")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Select Shift')", true);
                return;
            }

            if (this.ddl_zoneId.SelectedValue == "Select Zone")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Select Zone')", true);
                return;
            }


            if (this.ddl_branch.SelectedValue == "Select Branch")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Select Branch')", true);
                return;
            }
            //if (this.txt_routeCode.Text == "")
            //{
            //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter Route Code')", true);
            //    return;
            //}
            if (this.ddl_city.SelectedValue == "Select City")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Select City')", true);
                return;
            }

            if (this.ddl_Shift.SelectedValue == "Select Shift")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Select Shift')", true);
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


            string riderCode = txt_riderCode.Text;
            string Branch = ddl_branchId.SelectedValue.ToString();



            int checker = updatedRider(riderCode, Branch);

            if (checker == 0)
            {
                //if (routecheck == false)
                //{
                //    //if (checker_ == 0)
                //    //{
                //        string script_ = @"window.open(""{0}"", ""{1}"", ""{2}"");";
                //        //string script = String.Format(script_, "RunsheetInvoice.aspx?Xcode=" + temp_, "_blank", "");
                //        string script = String.Format(script_, "Rider_print.aspx?RiderCode=" + txt_riderCode.Text + "&Bcode=" + ddl_branch.SelectedValue, "_blank", "");
                //        ScriptManager.RegisterClientScriptBlock(this.Page, typeof(Page), "Redirect", script, true);

                //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "redirect", "alert('Rider Inserted!!'); window.location='" + Request.FilePath + "';", true);
                //    //}
                //}
                //else
                //{
                int checker_ = updatedRoute();
                if (checker_ == 0)
                {

                }
                else
                {
                    insertRoute();

                }
                string script_ = @"window.open(""{0}"", ""{1}"", ""{2}"");";
                //string script = String.Format(script_, "RunsheetInvoice.aspx?Xcode=" + temp_, "_blank", "");
                string script = String.Format(script_, "Rider_print.aspx?RiderCode=" + txt_riderCode.Text + "&Bcode=" + ddl_branch.SelectedValue, "_blank", "");
                ScriptManager.RegisterClientScriptBlock(this.Page, typeof(Page), "Redirect", script, true);

                ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "alert('Rider Updated!!'); window.location='" + Request.FilePath + "';", true);

            }
            else
            {
                lbl_error.Text = "Updation Failed";
                lbl_error.Visible = true;
            }

        }

        private int insertRoute()
        {
            int i = 0;

            string query = "Insert into Routes (routeCode,name,status,description,createdBy,createdOn,cityId,RIDECODER,BID) values ('" + txt_routeCode.Text + "','" + txt_routeName.Text + "','1','" + ddl_branchId.SelectedItem.ToString() + "','" + Session["U_ID"].ToString() + "',GetDate(),'" + ddl_city.SelectedValue.ToString() + "','" + txt_riderCode.Text + "','" + ddl_branchId.SelectedValue + "') ";


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

        private DataTable getRider(string riderCode, string Branch)
        {
            DataTable dt = new DataTable();

            string sql = "Select * from Riders where riderCode = '" + riderCode + "' and  branchID = '" + Branch + "'";
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Checking')", true);

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

        private DataTable getROUTES(string riderCode, string Branch)
        {
            DataTable dt = new DataTable();

            string sql = "Select * from Routes where riderCode = '" + riderCode + "' and  BID = '" + Branch + "'";
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Checking')", true);

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

        private int updatedRider(string riderCode, string Branch)
        {
            int i = 0;


            //string query = "Update  Riders set firstName= '" + txt_FirstName.Text + "' ,lastName= '" + txt_LastName.Text + "' ,MiddleName= '" + txt_MiddleName.Text + "' ,  \n" +
            //"  CNIC= '" + txt_CNIC.Text + "' ,cid= '" + ddl_cid.SelectedValue.ToString() + "' ,address= '" + txt_address.Text + "' , phoneNo= '" + txt_phoneNo.Text + "' ,  \n" +
            //"  dutyTypeId = '" + ddl_dutyType.SelectedValue.ToString() + "' , email= '" + txt_email.Text + "' ,lastrouteCode = '" + txt_lastrouteCode.Text + "',zoneId = '" + ddl_zoneId.SelectedValue.ToString() + "' ,  \n" +
            //"  Shift = '" + ddl_Shift.SelectedValue.ToString() + "' , expressCenterId = '" + ddl_expressCenterId.SelectedValue.ToString() + "' ,branchId = '" + ddl_branch.SelectedValue.ToString() + "' ,  \n" +
            //"  routeCode = '" + txt_routeCode.Text + "' ,Hrs_code= '" + txt_hrs_Code.Text + "' ,DOJ= '" + txt_DOj.Text + "' , DOB= '" + txt_DOB.Text + "' ,  \n" +
            //"  SeparationType = '" + ddl_separation.SelectedValue.ToString() + "' , DateOfLeaving = '" + txt_leaving.Text + "' ,deActivateBy = '" + Session["U_ID"].ToString() + "'  \n" +
            //", deActivateReamarks = '" + txt_remark.Text + "', deactivationDate = '" + DateTime.Now.ToString("yyyy-MM-dd HH:MM:ss") + "' where   riderCode = '" + riderCode + "' and  branchID = '" + Branch + "'";



            string query = "";

            if (chk_Deactivate.Checked)
            {
                query = "Update  Riders set firstName= '" + txt_FirstName.Text + "' ,lastName= '" + txt_LastName.Text + "' ,MiddleName= '" + txt_MiddleName.Text + "' ,  \n" +
                "  CNIC= '" + txt_CNIC.Text + "' ,cid= '" + ddl_cid.SelectedValue.ToString() + "' ,address= '" + txt_address.Text + "' , phoneNo= '" + txt_phoneNo.Text + "' ,  \n" +
                "  dutyTypeId = '" + ddl_dutyType.SelectedValue.ToString() + "' , email= '" + txt_email.Text + "' ,lastrouteCode = '" + txt_lastrouteCode.Text + "',zoneId = '" + ddl_zoneId.SelectedValue.ToString() + "' ,  \n" +
                "  Shift = '" + ddl_Shift.SelectedValue.ToString() + "' , expressCenterId = '" + ddl_expressCenterId.SelectedValue.ToString() + "' ,branchId = '" + ddl_branch.SelectedValue.ToString() + "' ,  \n" +
                "  Hrs_code= '" + txt_hrs_Code.Text + "' ,DOJ= '" + txt_DOj.Text + "' , DOB= '" + txt_DOB.Text + "' ,  \n" +
                "  SeparationType = '" + ddl_separation.SelectedValue.ToString() + "' , DateOfLeaving = '" + txt_leaving.Text + "' ,deActivateBy = '" + Session["U_ID"].ToString() + "'  \n" +
                ", deActivateReamarks = '" + txt_remark.Text + "', deactivationDate = '" + DateTime.Now.ToString("yyyy-MM-dd HH:MM:ss") + "', status ='0',routeCode='" + txt_routeCode.Text + "' where   riderCode = '" + riderCode + "' and  branchID = '" + Branch + "'";

            }
            else
            {
                query = "Update  Riders set firstName= '" + txt_FirstName.Text + "' ,lastName= '" + txt_LastName.Text + "' ,MiddleName= '" + txt_MiddleName.Text + "' ,  \n" +
                "  CNIC= '" + txt_CNIC.Text + "' ,cid= '" + ddl_cid.SelectedValue.ToString() + "' ,address= '" + txt_address.Text + "' , phoneNo= '" + txt_phoneNo.Text + "' ,  \n" +
                "  dutyTypeId = '" + ddl_dutyType.SelectedValue.ToString() + "' , email= '" + txt_email.Text + "' ,lastrouteCode = '" + txt_lastrouteCode.Text + "',zoneId = '" + ddl_zoneId.SelectedValue.ToString() + "' ,  \n" +
                "  Shift = '" + ddl_Shift.SelectedValue.ToString() + "' , expressCenterId = '" + ddl_expressCenterId.SelectedValue.ToString() + "' ,branchId = '" + ddl_branch.SelectedValue.ToString() + "' ,  \n" +
                "  Hrs_code= '" + txt_hrs_Code.Text + "' ,DOJ= '" + txt_DOj.Text + "' , DOB= '" + txt_DOB.Text + "',routeCode='" + txt_routeCode.Text + "'  where   riderCode = '" + riderCode + "' and  branchID = '" + Branch + "'";

            }


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

        private int updatedRoute()
        {
            int i = 0;

            DataTable dt = (DataTable)ViewState["dt_"];

            if (dt.Rows.Count >= 1)
            {
                string query = "Update  Routes set name= '" + txt_routeName.Text + "' ,cityId= '" + ddl_city.SelectedValue.ToString() + "',BID='" + ddl_branchId.SelectedValue + "',routecode='" + txt_routeCode.Text + "'  where RIDERCODE='" + txt_riderCode.Text + "' AND BID='" + ddl_branchId.SelectedValue + "'";

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
            }
            else
            {
                i = 1;
            }

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

        protected void ddl_branch_SelectedIndexChanged(object sender, EventArgs e)
        {
            getCities(ddl_branch.SelectedValue);
            getExpresscCenter();
        }

        private void getExpresscCenter()
        {
            string branch = ddl_branch.SelectedValue;
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
                string query = "SELECT * FROM ExpressCenters ec WHERE ec.bid  = '" + branch + "' and status='1' order by Name";

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

        protected void ddl_expressCenterId_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btn_search_Click(object sender, EventArgs e)
        {

            getCID();
            getDutyType();
            getZones();
            getCities();
            Get_ShiftInformation();

            string riderCode = txt_riderCode.Text;
            string Branch = ddl_branchId.SelectedValue.ToString();

            DataTable dt = getRider(riderCode, Branch);
            DataTable dt_1 = getROUTES(riderCode, Branch);
            ViewState["dt_"] = dt_1;

            if (dt.Rows.Count > 0)
            {
                div_main.Visible = true;
                btn_save.Enabled = true;
                txt_FirstName.Text = dt.Rows[0]["firstName"].ToString();
                txt_LastName.Text = dt.Rows[0]["lastName"].ToString();
                txt_MiddleName.Text = dt.Rows[0]["MiddleName"].ToString();
                txt_CNIC.Text = dt.Rows[0]["CNIC"].ToString();
                if (dt.Rows[0]["cid"].ToString() != "")
                {
                    for (int i = 0; i < ddl_cid.Items.Count; i++)
                    {
                        if (ddl_cid.Items[i].Value.ToString() == dt.Rows[0]["cid"].ToString())
                        {
                            ddl_cid.SelectedValue = dt.Rows[0]["cid"].ToString();
                        }
                    }
                }
                txt_address.Text = dt.Rows[0]["address"].ToString();
                txt_phoneNo.Text = dt.Rows[0]["phoneNo"].ToString();
                if (dt.Rows[0]["dutyTypeId"].ToString() != "")
                {
                    for (int i = 0; i < ddl_dutyType.Items.Count; i++)
                    {
                        if (ddl_dutyType.Items[i].Value.ToString() == dt.Rows[0]["dutyTypeId"].ToString())
                        {
                            ddl_dutyType.SelectedValue = dt.Rows[0]["dutyTypeId"].ToString();
                        }
                    }

                }
                txt_email.Text = dt.Rows[0]["email"].ToString();
                txt_lastrouteCode.Text = dt.Rows[0]["lastrouteCode"].ToString();
                if (dt.Rows[0]["zoneId"].ToString() != "")
                {
                    ddl_zoneId.SelectedValue = dt.Rows[0]["zoneId"].ToString();
                }
                if (dt.Rows[0]["Shift"].ToString() != "")
                {
                    for (int i = 0; i < ddl_Shift.Items.Count; i++)
                    {
                        if (ddl_Shift.Items[i].Value.ToString() == dt.Rows[0]["Shift"].ToString())
                        {
                            ddl_Shift.SelectedValue = dt.Rows[0]["Shift"].ToString();
                        }
                    }
                }

                if (dt.Rows[0]["branchId"].ToString() != "")
                {
                    ddl_branch.SelectedValue = dt.Rows[0]["branchId"].ToString();
                }
                getExpresscCenter();

                if (dt.Rows[0]["expressCenterId"].ToString() != "")
                {
                    for (int i = 0; i < ddl_expressCenterId.Items.Count; i++)
                    {
                        if (ddl_expressCenterId.Items[i].Value.ToString() == dt.Rows[0]["expressCenterId"].ToString())
                        {
                            ddl_expressCenterId.SelectedValue = dt.Rows[0]["expressCenterId"].ToString();
                        }
                    }
                }
                // txt_routeCode.Text = dt.Rows[0]["routeCode"].ToString();

                txt_hrs_Code.Text = dt.Rows[0]["Hrs_code"].ToString();
                txt_routeCode.Text = dt.Rows[0]["routeCode"].ToString();

                if (dt.Rows[0]["DOJ"].ToString() != "" || dt.Rows[0]["DOJ"].ToString() != "0" || dt.Rows[0]["DOJ"].ToString() != null)
                {
                    try
                    {
                        DateTime dt_doj = DateTime.Parse(dt.Rows[0]["DOJ"].ToString());
                        txt_DOj.Text = dt_doj.ToString("yyyy-MM-dd");
                    }
                    catch (Exception Err)
                    { }
                }

                if (dt.Rows[0]["DOB"].ToString() != "" || dt.Rows[0]["DOB"].ToString() != "0" || dt.Rows[0]["DOB"].ToString() != null)
                {
                    try
                    {
                        DateTime dt_doj = DateTime.Parse(dt.Rows[0]["DOB"].ToString());
                        txt_DOB.Text = dt_doj.ToString("yyyy-MM-dd");
                    }
                    catch (Exception Err)
                    { }
                }




                DataTable dt_ = getCity();
                if (dt_.Rows.Count != 0)
                {
                    if (dt_.Rows[0]["cityId"].ToString() != "")
                    {
                        ddl_city.SelectedValue = dt_.Rows[0]["cityId"].ToString();
                    }

                    txt_routeName.Text = dt_.Rows[0]["name"].ToString();
                    Cache["routedID"] = dt_.Rows[0]["id"].ToString();
                }
                if (dt.Rows[0]["SeparationType"].ToString() != "" && dt.Rows[0]["SeparationType"] != null)
                {
                    ddl_separation.SelectedValue = dt.Rows[0]["SeparationType"].ToString(); ;
                }
                if (dt.Rows[0]["DateofLeaving"].ToString() != "" && dt.Rows[0]["DateofLeaving"] != null)
                {
                    this.txt_leaving.Text = dt.Rows[0]["DateofLeaving"].ToString();
                }
                if (dt.Rows[0][31].ToString() != "" && dt.Rows[0][31] != null)
                {
                    txt_remark.Text = dt.Rows[0][31].ToString();
                }
                if (dt.Rows[0]["Status"].ToString() == "False")
                {
                    btn_save.Enabled = false;
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Rider Already Deactivated')", true);
                    return;
                }
                else
                {
                    btn_save.Enabled = true;
                }
            }

            else
            {
                div_main.Visible = false;
                btn_save.Enabled = false;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('No Rider Found')", true);
                return;
            }

        }

        private DataTable getCity()
        {
            DataTable Ds_1 = new DataTable();

            try
            {
                string sqlString = "SELECT r2.routeCode,\n" +
                "       r2.name,\n" +
                "       r2.cityId,\n" +
                "       c.cityName,r2.id\n" +
                "FROM   Riders r\n" +
                "       INNER JOIN Branches b\n" +
                "            ON  b.branchCode = r.branchId\n" +
                "       INNER JOIN Cities c\n" +
                "            ON  c.id = b.cityId\n" +
                "       INNER JOIN Routes r2\n" +
                "            ON  r2.routeCode = r.routeCode\n" +
                "            AND r2.cityId = b.cityId\n" +
                "WHERE  r.riderCode = '" + txt_riderCode.Text + "'\n" +
                "       AND r.branchId = '" + ddl_branchId.SelectedValue.ToString() + "'\n" +
                "";



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
        protected void chk_Deactivate_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_Deactivate.Checked == true)
            {
                div_deactivate.Visible = true;
            }
            else
            { div_deactivate.Visible = false; }

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
                  + "WHERE r.routeCode ='" + txt_routeCode.Text + "' AND b.branchCode='" + ddl_branch.SelectedValue + "' and r.status ='1'";

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