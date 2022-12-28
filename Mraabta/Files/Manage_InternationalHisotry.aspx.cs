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
using Telerik.Web.UI;
using System.Collections;

namespace MRaabta.Files
{
    public partial class Manage_InternationalHisotry : System.Web.UI.Page
    {
        Variable c = new Variable();
        Cl_Variables clvar = new Cl_Variables();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Get_Couries();
            }
        }

        public void Get_Couries()
        {
            DataSet ds_couries = Get_AllCouries();

            if (ds_couries.Tables[0].Rows.Count != 0)
            {
                dd_couries.DataTextField = "Name";
                dd_couries.DataValueField = "ID";
                dd_couries.DataSource = ds_couries.Tables[0].DefaultView;
                dd_couries.DataBind();
            }
            dd_couries.Items.Insert(0, new ListItem("Select Couries", ""));
        }

        public DataSet Get_AllCouries()
        {
            DataSet ds = new DataSet();
            string query;
            try
            {
                query = "SELECT * FROM MNP_InternationalCourier where status = '1'";

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

        protected void RadGrid1_PreRender(object sender, System.EventArgs e)
        {
        }

        private static DataTable GetDataTable(string queryString)
        {
            String ConnString = ConfigurationManager.ConnectionStrings["Connectionstring"].ConnectionString;
            SqlConnection MySqlConnection = new SqlConnection(ConnString);
            SqlDataAdapter MySqlDataAdapter = new SqlDataAdapter();
            MySqlDataAdapter.SelectCommand = new SqlCommand(queryString, MySqlConnection);

            DataTable myDataTable = new DataTable();
            MySqlConnection.Open();
            try
            {
                MySqlDataAdapter.Fill(myDataTable);
            }
            finally
            {
                MySqlConnection.Close();
            }

            return myDataTable;
        }

        private DataTable Employees
        {
            get
            {
                object obj = this.Session["Employees"];
                if ((!(obj == null)))
                {
                    return ((DataTable)(obj));
                }
                DataTable myDataTable = new DataTable();
                myDataTable = GetDataTable("SELECT * FROM InternationalTrackinghistory");
                this.Session["Employees"] = myDataTable;
                return myDataTable;
            }
        }

        protected void RadGrid1_UpdateCommand(object source, GridCommandEventArgs e)
        {
            GridEditableItem editedItem = e.Item as GridEditableItem;
            UserControl userControl = (UserControl)e.Item.FindControl(GridEditFormItem.EditFormUserControlID);

            //Prepare new row to add it in the DataSource
            DataRow[] changedRows = this.Employees.Select("id = " + editedItem.OwnerTableView.DataKeyValues[editedItem.ItemIndex]["ID"]);
            string ID_ = editedItem.OwnerTableView.DataKeyValues[editedItem.ItemIndex]["ID"].ToString();

            if (changedRows.Length != 1)
            {
            }

            //Update new values
            Hashtable newValues = new Hashtable();

            newValues["Consignmentnumber"] = (userControl.FindControl("Consignmentnumber") as TextBox).Text;
            newValues["ReferenceNumber"] = (userControl.FindControl("ReferenceNumber") as TextBox).Text;

            newValues["TransactionDate"] = (userControl.FindControl("TransactionDate") as RadDatePicker).SelectedDate.ToString();
            //    newValues["Transactiontime"] = (userControl.FindControl("Transactiontime") as RadDateTimePicker).SelectedDate.ToString();
            newValues["CurrentLocation"] = (userControl.FindControl("txt_CurrentLocation") as TextBox).Text;
            newValues["Details"] = (userControl.FindControl("Details") as TextBox).Text;

            changedRows[0].BeginEdit();
            try
            {
            }
            catch (Exception ex)
            {
                changedRows[0].CancelEdit();


                e.Canceled = true;
            }
        }
        protected void RadGrid1_InsertCommand(object source, GridCommandEventArgs e)
        {

        }
        protected void RadGrid1_DeleteCommand(object source, GridCommandEventArgs e)
        {
            string ID = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["id"].ToString();
            DataTable employeeTable = this.Employees;
            if (employeeTable.Rows.Find(ID) != null)
            {
                employeeTable.Rows.Find(ID).Delete();
                employeeTable.AcceptChanges();
            }
        }
        protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //    RadGrid1.MasterTableView.EditMode = (GridEditMode)Enum.Parse(typeof(GridEditMode), RadioButtonList1.SelectedValue);
            //    RadGrid1.Rebind();
        }
        protected void Btn_Load_Click(object sender, EventArgs e)
        {

            String ConnString = ConfigurationManager.ConnectionStrings["Connectionstring"].ConnectionString;
            SqlConnection MySqlConnection = new SqlConnection(ConnString);
            SqlDataAdapter MySqlDataAdapter = new SqlDataAdapter();
            MySqlDataAdapter.SelectCommand = new SqlCommand("Select c.* from ServiceTypes_New sn inner join consignment c on sn.serviceTypeName = c.serviceTypeName where Products ='International' and consignmentnumber ='" + txt_consignmentno.Text + "'", MySqlConnection);

            DataTable myDataTable = new DataTable();
            MySqlConnection.Open();
            try
            {
                MySqlDataAdapter.Fill(myDataTable);
            }
            finally
            {
                MySqlConnection.Close();
            }

            if (myDataTable.Rows.Count > 0)
            { }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Consignment Number Doesnot Exist')", true);
                txt_consignmentno.Text = "";
                return;
            }

            // Getting CN Detail based on Tracking 
            Get_Details();
        }
        public void Get_Details()
        {
            String ConnString = ConfigurationManager.ConnectionStrings["Connectionstring"].ConnectionString;
            SqlConnection MySqlConnection = new SqlConnection(ConnString);
            SqlDataAdapter MySqlDataAdapter = new SqlDataAdapter();
            MySqlDataAdapter.SelectCommand = new SqlCommand("SELECT * FROM InternationalTrackinghistory where consignmentnumber ='" + txt_consignmentno.Text + "'", MySqlConnection);

            DataTable myDataTable = new DataTable();
            MySqlConnection.Open();
            try
            {
                MySqlDataAdapter.Fill(myDataTable);
            }
            finally
            {
                MySqlConnection.Close();
            }

            Repeater1.DataSource = myDataTable.DefaultView;
            Repeater1.DataBind();

        }

        public DataTable Get_Details_2(string id)
        {
            String ConnString = ConfigurationManager.ConnectionStrings["Connectionstring"].ConnectionString;
            SqlConnection MySqlConnection = new SqlConnection(ConnString);
            SqlDataAdapter MySqlDataAdapter = new SqlDataAdapter();
            MySqlDataAdapter.SelectCommand = new SqlCommand("SELECT * FROM InternationalTrackinghistory where id='" + id + "'", MySqlConnection);

            DataTable myDataTable = new DataTable();
            MySqlConnection.Open();
            try
            {
                MySqlDataAdapter.Fill(myDataTable);
            }
            finally
            {
                MySqlConnection.Close();
            }

            return myDataTable;
        }

        public DataTable Get_Details_3(string consignmentnumber, string referencenumber, string Currenttime, string currentlocation, string details)
        {
            String ConnString = ConfigurationManager.ConnectionStrings["Connectionstring"].ConnectionString;
            SqlConnection MySqlConnection = new SqlConnection(ConnString);
            SqlDataAdapter MySqlDataAdapter = new SqlDataAdapter();
            MySqlDataAdapter.SelectCommand = new SqlCommand("SELECT * FROM InternationalTrackinghistory where consignmentnumber='" + consignmentnumber + "' and referencenumber ='" + referencenumber + "' and currentlocation='" + currentlocation + "' and cast(details as varchar) ='" + details + "'", MySqlConnection);

            DataTable myDataTable = new DataTable();
            MySqlConnection.Open();
            try
            {
                MySqlDataAdapter.Fill(myDataTable);
            }
            finally
            {
                MySqlConnection.Close();
            }

            return myDataTable;
        }

        protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

        }

        protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Edit")
            {
                HiddenField CityID = e.Item.FindControl("hd_id") as HiddenField;
                hd_rd1.Value = CityID.Value;
                DataTable dt = Get_Details_2(CityID.Value);

                if (dt.Rows.Count > 0)
                {
                    txt_1.Text = dt.Rows[0]["consignmentnumber"].ToString();
                    txt_referenceno.Text = dt.Rows[0]["ReferenceNumber"].ToString();
                    this.txt_CurrentLocation.Text = dt.Rows[0]["CurrentLocation"].ToString();
                    this.txt_TrackingDetails.Text = dt.Rows[0]["details"].ToString();
                    this.txt_Carier.Text = dt.Rows[0]["carrier"].ToString();
                    if (dt.Rows[0]["Courier"].ToString() != "")
                    {
                        this.dd_couries.SelectedValue = dt.Rows[0]["Courier"].ToString();
                    }
                    btn_Insert.Enabled = false;
                    btn_update.Enabled = true;

                }

            }
            if (e.CommandName == "Delete")
            {
                HiddenField CityID = e.Item.FindControl("hd_id") as HiddenField;
                hd_rd1.Value = CityID.Value;
                DataTable dt = Get_Details_2(CityID.Value);
                try
                {
                    string Sql = "delete from [dbo].[InternationalTrackingHistory] \n" +
                                 "  WHERE id ='" + hd_rd1.Value + "'";

                    using (var connection = new SqlConnection(clvar.Strcon()))
                    {
                        using (var command = new SqlCommand(Sql, connection))
                        {
                            connection.Open();
                            command.ExecuteNonQuery();

                            Get_Details();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Label lblError = new Label();
                    lblError.Text = "Unable to insert Employees. Reason: " + ex.Message;
                    lblError.ForeColor = System.Drawing.Color.Red;
                }
            }
        }

        protected void btn_Insert_Click(object sender, EventArgs e)
        {

            if (txt_referenceno.Text.Length == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Reference Number not Present')", true);
                return;
            }
            if (this.txt_CurrentLocation.Text.Length == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Current Location not Present')", true);
                return;
            }
            if (this.txt_TrackingDetails.Text.Length == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Tracking Details not Present')", true);
                return;
            }
            if (this.txt_Carier.Text.Length == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Carrier not Present')", true);
                return;
            }
            if ((rd_1).SelectedDate.ToString().Length == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Current Time Not Present')", true);
                return;
            }

            try
            {
                DataTable Dt1 = Get_Details_3(txt_consignmentno.Text, txt_referenceno.Text, "", txt_CurrentLocation.Text, txt_TrackingDetails.Text);
                if (Dt1.Rows.Count != 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('This Record is already Present')", true);
                    return;
                }

                String ConnString = ConfigurationManager.ConnectionStrings["Connectionstring"].ConnectionString;
                SqlConnection MySqlConnection = new SqlConnection(ConnString);
                SqlDataAdapter MySqlDataAdapter = new SqlDataAdapter();
                MySqlDataAdapter.SelectCommand = new SqlCommand("Select c.* from ServiceTypes_New sn inner join consignment c on sn.serviceTypeName = c.serviceTypeName where Products ='International' and consignmentnumber ='" + txt_consignmentno.Text + "'", MySqlConnection);

                DataTable myDataTable = new DataTable();
                MySqlConnection.Open();
                try
                {
                    MySqlDataAdapter.Fill(myDataTable);
                }
                finally
                {
                    MySqlConnection.Close();
                }

                if (myDataTable.Rows.Count > 0)
                {

                    DateTime d = DateTime.Parse((rd_1).SelectedDate.ToString());

                    string Sql = "insert into [dbo].[InternationalTrackingHistory] \n" +
                                " Select '" + txt_consignmentno.Text + "'\n" +
                                "  ,'" + txt_referenceno.Text + "'\n" +
                                "   ,'" + txt_CurrentLocation.Text + "' \n" +
                                "   ,'" + txt_TrackingDetails.Text + "' \n" +
                                "   ,'" + HttpContext.Current.Session["U_ID"].ToString() + "' \n" +
                                "   ,getdate() \n" +
                                "  ,'" + d.ToString("yyyy-MM-dd HH:mm:ss") + "' \n" +
                                "  ,'" + this.txt_Carier.Text + "' \n" +
                                "  ,'" + this.dd_couries.SelectedValue + "' \n";


                    using (var connection = new SqlConnection(clvar.Strcon()))
                    {
                        using (var command = new SqlCommand(Sql, connection))
                        {
                            connection.Open();
                            command.ExecuteNonQuery();

                            Get_Details();
                        }
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Consignment Number Doesnot Exist')", true);

                    return;
                }
            }
            catch (Exception ex)
            {
                Label lblError = new Label();
                lblError.Text = "Unable to insert Employees. Reason: " + ex.Message;
                lblError.ForeColor = System.Drawing.Color.Red;

            }
            btn_Cancel_Click(sender, e);
        }

        protected void btn_update_Click(object sender, EventArgs e)
        {

            if (txt_referenceno.Text.Length == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Reference Number not Present')", true);
                return;
            }
            if (this.txt_CurrentLocation.Text.Length == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Current Location not Present')", true);
                return;
            }
            if (this.txt_TrackingDetails.Text.Length == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Tracking Details not Present')", true);
                return;
            }
            if ((rd_1).SelectedDate.ToString().Length == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Current Time Not Present')", true);
                return;
            }

            try
            {
                DataTable Dt1 = Get_Details_3(txt_consignmentno.Text, txt_referenceno.Text, "", txt_CurrentLocation.Text, txt_TrackingDetails.Text);
                if (Dt1.Rows.Count != 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('This Record is already Present')", true);
                    return;
                }

                DateTime d = DateTime.Parse((rd_1).SelectedDate.ToString());

                string Sql = "UPDATE [dbo].[InternationalTrackingHistory] \n" +
                            " SET [ReferenceNumber] = '" + txt_referenceno.Text + "'\n" +
                            "  ,[TransactionDate] = '" + d.ToString("yyyy-MM-dd HH:mm:ss") + "' \n" +
                            "  ,[CurrentLocation] = '" + txt_CurrentLocation.Text + "' \n" +
                            "  ,[Details] = '" + txt_TrackingDetails.Text + "' \n" +
                            "  ,[carrier] = '" + this.txt_Carier.Text + "' \n" +
                            "  ,[Courier] = '" + this.dd_couries.SelectedValue + "' \n" +
                            "  WHERE id ='" + hd_rd1.Value + "'";

                using (var connection = new SqlConnection(clvar.Strcon()))
                {
                    using (var command = new SqlCommand(Sql, connection))
                    {
                        connection.Open();
                        command.ExecuteNonQuery();

                        Get_Details();
                    }
                }
                btn_Insert.Enabled = true;
                btn_update.Enabled = false;
            }
            catch (Exception ex)
            {
                Label lblError = new Label();
                lblError.Text = "Unable to insert Employees. Reason: " + ex.Message;
                lblError.ForeColor = System.Drawing.Color.Red;
            }
            btn_Cancel_Click(sender, e);
        }

        protected void btn_Cancel_Click(object sender, EventArgs e)
        {
            txt_referenceno.Text = ""; //dt.Rows[0]["ReferenceNumber"].ToString();
            this.txt_CurrentLocation.Text = ""; //dt.Rows[0]["CurrentLocation"].ToString();
            this.txt_TrackingDetails.Text = ""; //dt.Rows[0]["details"].ToString();
            this.txt_Carier.Text = "";
        }

        protected void txt_referenceno_TextChanged(object sender, EventArgs e)
        {
            String ConnString = ConfigurationManager.ConnectionStrings["Connectionstring"].ConnectionString;
            SqlConnection MySqlConnection = new SqlConnection(ConnString);
            SqlDataAdapter MySqlDataAdapter = new SqlDataAdapter();
            MySqlDataAdapter.SelectCommand = new SqlCommand("Select c.* from ServiceTypes_New sn inner join consignment c on sn.serviceTypeName = c.serviceTypeName where Products ='International' and consignmentnumber ='" + txt_consignmentno.Text + "'", MySqlConnection);

            DataTable myDataTable = new DataTable();
            MySqlConnection.Open();
            try
            {
                MySqlDataAdapter.Fill(myDataTable);
            }
            finally
            {
                MySqlConnection.Close();
            }
        }
        protected void btn_update_Click1(object sender, EventArgs e)
        {

        }
    }


}