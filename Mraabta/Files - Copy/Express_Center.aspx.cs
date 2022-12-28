using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Data;
using System.Data.SqlClient;
using MRaabta.App_Code;

namespace MRaabta.Files
{
    public partial class Express_Center : System.Web.UI.Page
    {
        Cl_Variables clvar = new Cl_Variables();
        CommonFunction func = new CommonFunction();
        Consignemnts con = new Consignemnts();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Get_origin();
                Get_ExpressCenterType();
                Get_DaysOff();
            }
        }

        public void Get_origin()
        {
            DataSet ds = Branch();
            if (ds.Tables.Count != 0)
            {
                //   DataTable dt = Cities_();
                this.dd_Branch.DataTextField = "BranchName";
                this.dd_Branch.DataValueField = "branchCode";
                this.dd_Branch.DataSource = ds.Tables[0].DefaultView;
                this.dd_Branch.DataBind();

            }
        }

        public void Get_Franchise()
        {
            DataSet ds = func.Branch();
            if (ds.Tables.Count != 0)
            {
                //   DataTable dt = Cities_();
                this.dd_Branch.DataTextField = "BranchName";
                this.dd_Branch.DataValueField = "branchCode";
                this.dd_Branch.DataSource = ds.Tables[0].DefaultView;
                this.dd_Branch.DataBind();

            }
        }

        public DataSet Branch()
        {
            DataSet Ds_1 = new DataSet();

            try
            {
                string query = "SELECT b.branchCode, \n"
               + "       b.name     BranchName, sname BName \n"
               + "FROM   Branches                          b \n"
               + "where b.[status] ='1'  \n"
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
            }
            catch (Exception Err)
            { }
            finally
            { }

            return Ds_1;
        }

        public DataSet Franchise(string branch)
        {
            DataSet Ds_1 = new DataSet();

            try
            {
                string sql = "SELECT f.name,f.id FROM Franchisee f WHERE f.isActive ='1' AND f.branchCode ='" + branch + "'";

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

        public void Get_ExpressCenterType()
        {
            DataSet ds = func.ExpressCenterType();
            if (ds.Tables.Count != 0)
            {
                //   DataTable dt = Cities_();
                this.ec_type.DataTextField = "ExpressCenter";
                this.ec_type.DataValueField = "ExpressCenter_TypeID";
                this.ec_type.DataSource = ds.Tables[0].DefaultView;
                this.ec_type.DataBind();

            }
        }

        public void Get_DaysOff()
        {
            DataSet ds = func.ExpressCenterType();

            List<string[]> list = new List<string[]>();
            list.Add(new string[] { "Monday" });
            list.Add(new string[] { "Tuesday" });
            list.Add(new string[] { "Wednesday" });
            list.Add(new string[] { "Thursday" });
            list.Add(new string[] { "Friday" });
            list.Add(new string[] { "Saturday" });
            list.Add(new string[] { "Sunday" });


            DataTable dr = ConvertListToDataTable(list);

            if (dr.Rows.Count != 0)
            {
                //   DataTable dt = Cities_();
                this.dd_Dayoff.DataTextField = "Days";
                this.dd_Dayoff.DataValueField = "Days";
                this.dd_Dayoff.DataSource = dr.DefaultView;
                this.dd_Dayoff.DataBind();

            }
        }

        static DataTable ConvertListToDataTable(List<string[]> list)
        {
            // New table.
            DataTable table = new DataTable();

            // Get max columns.
            int columns = 0;
            foreach (var array in list)
            {
                if (array.Length > columns)
                {
                    columns = array.Length;
                }
            }

            // Add columns.
            for (int i = 0; i < columns; i++)
            {
                table.Columns.Add("Days");
            }

            // Add rows.
            foreach (var array in list)
            {
                table.Rows.Add(array);
            }

            return table;
        }

        protected void btn_SaveConsignment1_Click(object sender, EventArgs e)
        {

            if (ec_type.SelectedValue == "2")
            {
                if (txt_ClientID.Text == "")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "aletMessage", "alert('Enter Client Name')", true);
                    return;
                }
                if (txt_FaxNo.Text == "")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "aletMessage", "alert('Enter Fax')", true);
                    return;
                }
                if (this.txt_Description.Text == "")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "aletMessage", "alert('Enter Description')", true);
                    return;
                }
                if (txt_PhoneNo.Text == "")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "aletMessage", "alert('Enter Phone No')", true);
                    return;
                }
                if (txt_EmailID.Text == "")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "aletMessage", "alert('Enter Email ID')", true);
                    return;
                }

            }

            clvar = new Cl_Variables();
            clvar.expresscenter = txt_Eccode.Text;
            clvar.expresscentername = txt_ExpressCenterName.Text;
            clvar.productDescription = txt_Description.Text;
            clvar.Branch = dd_Branch.SelectedValue;
            clvar.shortName = txt_Sname.Text;
            clvar.expresscentertype = ec_type.SelectedValue;
            clvar.isdistributionCenter = cb_idc.Checked;
            clvar.ismainEc = cb_EC.Checked;
            clvar.Dayoff = dd_Dayoff.SelectedValue;
            clvar.CreditClientID = txt_ClientID.Text;
            clvar.Fax = txt_FaxNo.Text;
            clvar.ConsignerPhone = txt_PhoneNo.Text;
            clvar.consignerEmail = txt_EmailID.Text;


            clvar = Add_ExpressCenter(clvar);
            if (clvar.Error != "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "aletMessage", "alert('" + clvar.Error.ToString() + "')", true);
                return;
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "aletMessage", "alert('Express Center has been Saved')", true);

                txt_Eccode.Text = "";
                txt_ExpressCenterName.Text = "";
                txt_Description.Text = "";
                dd_Branch.SelectedValue = "0";
                txt_Sname.Text = "";
                //    ec_type.SelectedValue = "0";
                cb_idc.Checked = false;
                cb_EC.Checked = false;
                //    dd_Dayoff.SelectedValue = "0";
                //    txt_ClientID.Text = "";
                txt_FaxNo.Text = "";
                txt_PhoneNo.Text = "";
                txt_EmailID.Text = "";

                string script_ = @"window.open(""{0}"", ""{1}"", ""{2}"");";
                //string script = String.Format(script_, "RunsheetInvoice.aspx?Xcode=" + temp_, "_blank", "");
                string script = String.Format(script_, "Ec_print.aspx?EC_Code=" + clvar.expresscenter, "_blank", "");
                ScriptManager.RegisterClientScriptBlock(this.Page, typeof(Page), "Redirect", script, true);


            }
        }

        public Cl_Variables Add_ExpressCenter(Cl_Variables obj)
        {
            clvar.Error = "";
            int IsUnique = 0;
            SqlConnection sqlcon = new SqlConnection(obj.Strcon());
            //    obj.Call_Log_ID = int.Parse(Get_maxValue()) + 1;
            try
            {
                sqlcon.Open();
                SqlCommand sqlcmd = new SqlCommand("spSaveExpCenterDetail", sqlcon);
                // SqlCommand sqlcmd = new SqlCommand("MNP_InsertConsignment_Temp", sqlcon);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                // SqlParameter SParam = new SqlParameter("ReturnValue", SqlDbType.Int);
                // SParam.Direction = ParameterDirection.ReturnValue;
                //
                //   sqlcmd.Parameters.AddWithValue("@ExpCenCode", obj.expresscenter);
                sqlcmd.Parameters.AddWithValue("@Name", obj.expresscentername);
                sqlcmd.Parameters.AddWithValue("@Description", obj.productDescription);
                sqlcmd.Parameters.AddWithValue("@BranchId", obj.Branch);
                sqlcmd.Parameters.AddWithValue("@ShortName", obj.shortName);
                sqlcmd.Parameters.AddWithValue("@Email", obj.consignerEmail);
                sqlcmd.Parameters.AddWithValue("@Phone", obj.ConsignerPhone);
                sqlcmd.Parameters.AddWithValue("@Fax", obj.Fax);
                sqlcmd.Parameters.AddWithValue("@HasStore", "0");
                sqlcmd.Parameters.AddWithValue("@IsDistributionCenter", obj.isdistributionCenter);
                sqlcmd.Parameters.AddWithValue("@ClientId", obj.CreditClientID);
                sqlcmd.Parameters.AddWithValue("@CostCenterId", "0");
                sqlcmd.Parameters.AddWithValue("@IsFranchised", "0");
                sqlcmd.Parameters.AddWithValue("@FranchiseeAccountNo", obj.AccountNo);
                sqlcmd.Parameters.AddWithValue("@IsActive", "1");
                sqlcmd.Parameters.AddWithValue("@UserName", Session["U_ID"].ToString());
                sqlcmd.Parameters.AddWithValue("@ExpressCenterType", obj.expresscentertype);
                sqlcmd.Parameters.AddWithValue("@Dayoff", obj.Dayoff);
                sqlcmd.Parameters.AddWithValue("@MainEC", obj.ismainEc);
                sqlcmd.Parameters.Add("@result", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                sqlcmd.ExecuteNonQuery();
                clvar.expresscenter = sqlcmd.Parameters["@result"].Value.ToString();
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
            return clvar;
        }

        protected void ec_type_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (ec_type.SelectedValue == "2")
            //{
            //    txt_Facc.Enabled = true;
            //    txt_Facc.Text = "";
            //}
            //else
            //{
            //    txt_Facc.Enabled = false;
            //    txt_Facc.Text = "";

            //}
        }

        protected void txt_Facc_TextChanged(object sender, EventArgs e)
        {
            //clvar = new Cl_Variables();
            //clvar.AccountNo = txt_Facc.Text;
            //clvar.Branch = dd_Branch.SelectedValue;

            //if (dd_Branch.SelectedValue == "0")
            //{
            //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "aletMessage", "alert('Branch not Selected')", true);
            //    dd_Branch.Focus();
            //    return;
            //}
            //DataTable ds = func.ClientInfo(clvar);
            //if (ds.Rows.Count != 0)
            //{
            //txt_ClientID.Text = ds.Rows[0]["id"].ToString();
            //txt_PhoneNo.Text = ds.Rows[0]["phoneno"].ToString();
            //txt_FaxNo.Text = ds.Rows[0]["faxno"].ToString();
            //txt_FaxNo.Text = ds.Rows[0]["Email"].ToString();
            //}
            //else
            //{
            //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "aletMessage", "alert('Invalid Account No')", true);
            //    txt_Facc.Text = "";
            //    txt_Facc.Focus();
            //    return;

            //}

        }
        protected void btn_Cancel_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "aletMessage", "alert('Express Center has been Canceled')", true);

            txt_Eccode.Text = "";
            txt_ExpressCenterName.Text = "";
            txt_Description.Text = "";
            dd_Branch.SelectedValue = "0";
            txt_Sname.Text = "";
            ec_type.SelectedValue = "0";
            cb_idc.Checked = false;
            cb_EC.Checked = false;
            dd_Dayoff.SelectedValue = "0";
            txt_ClientID.Text = "";
            txt_FaxNo.Text = "";
            txt_PhoneNo.Text = "";
            txt_EmailID.Text = "";
        }



        protected void dd_Branch_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (dd_Branch.SelectedValue != "0")
            {
                string branch = dd_Branch.SelectedValue;
                DataSet franchise = Franchise(branch);
                if (franchise.Tables[0].Rows.Count != 0)
                {
                    rad_Franchise.DataSource = null;

                    rad_Franchise.DataTextField = "";
                    rad_Franchise.DataValueField = "";

                }
            }
        }
    }
}