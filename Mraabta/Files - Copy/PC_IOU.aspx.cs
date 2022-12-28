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
    public partial class PC_IOU : System.Web.UI.Page
    {

        #region Variables

        Cl_Variables clvar = new Cl_Variables();
        CommonFunction func = new CommonFunction();
        Consignemnts con = new Consignemnts();

        DataTable ConsignmentDetails_ = null;
        DataTable WrappingProcess_ = null;
        DataTable WrappingProcessArchive_ = null;

        bool DataComeFromWrappingProcess = false;
        bool DataComeFromConsignment = false;


        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Populate_IOUNumber();

                //PopulateBranches(Get_Branches());
                DataTable dt = Get_PC_IOU();
                BindDataTableToRepeater(dt);

                PopulateZones(Get_Zones());
                PopulateStatus(Get_Flag());

                dd_Status.SelectedValue = "1";
                if (dd_Status.SelectedValue == "1")
                {
                    lbl_IOUNO_.Visible = false;
                    txt_IOUNO_.Visible = false;
                }

            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }

        public void Populate_IOUNumber()
        {
            DataTable Get_Max_IONO = Get_MAX_IOUNumber();

            string Max_IONO = Get_Max_IONO.Rows[0][0].ToString();

            if (string.IsNullOrEmpty(Max_IONO))
            {
                Max_IONO = string.Concat(DateTime.Now.Year.ToString(), "0000001");
            }
            else
            {

                double Temp_Max_IOUNO = double.Parse(Max_IONO);

                Temp_Max_IOUNO += 1.0;

                Max_IONO = Temp_Max_IOUNO.ToString();

            }

            txt_IOUNO.Text = Max_IONO;

            hf_IOUNumber.Value = Max_IONO;
        }

        private void PopulateBranches(DataTable dt)
        {
            DataTable dt_ = dt;
            dd_Branch.Items.Clear();

            dd_Branch.Items.Add(new ListItem { Text = "Select Branch", Value = "0" });

            dd_Branch.DataSource = dt_;
            dd_Branch.DataTextField = "name";
            dd_Branch.DataValueField = "branchcode";

            dd_Branch.DataBind();
        }

        public void PopulateZones(DataTable dt)
        {
            DataTable dt_ = dt;
            dd_Zone.Items.Clear();

            dd_Zone.Items.Add(new ListItem { Text = "Select Zone", Value = "0" });

            if (dt_.Rows.Count > 0)
            {
                dd_Zone.DataSource = dt_;
                dd_Zone.DataTextField = "name";
                dd_Zone.DataValueField = "zonecode";
                dd_Zone.DataBind();
            }
        }

        public void PopulateStatus(DataTable dt)
        {
            DataTable dt_ = dt;
            dd_Status.Items.Clear();

            if (dt_.Rows.Count > 0)
            {
                dd_Status.DataSource = dt_;
                dd_Status.DataTextField = "Flag";
                dd_Status.DataValueField = "Id";
                dd_Status.DataBind();
            }
        }

        public DataTable Get_Branches()
        {
            string Session_Branch = Session["BRANCHCODE"].ToString();
            string Branch_Query = string.Empty;

            if (Session_Branch.Equals("ALL") && !Session_Branch.Contains(','))
            {
                Branch_Query = " ";
            }

            else if (!Session_Branch.Equals("ALL") && Session_Branch.Contains(','))
            {
                List<string> Branch = new List<string>();

                var elements = Session_Branch.Split(new[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);

                foreach (var item in elements)
                {
                    Branch.Add(item);
                }

                string In_Clause_Branch = string.Empty;

                foreach (var item in Branch)
                {
                    In_Clause_Branch += "'" + item + "',";
                }
                if (In_Clause_Branch.EndsWith(","))
                {
                    In_Clause_Branch = In_Clause_Branch.Remove(In_Clause_Branch.Length - 1, 1);
                }

                Branch_Query = " branchcode IN (" + In_Clause_Branch + ") AND";
            }

            else if (!Session_Branch.Equals("ALL") && !Session_Branch.Contains(','))
            {
                Branch_Query = " branchcode =" + Session_Branch + " AND";
            }

            string query = " Select branchcode, name from branches where" + Branch_Query + " status ='1'";

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

        public DataTable Get_Zone_Related_Branches()
        {
            string Selected_Zone = dd_Zone.SelectedValue.ToString();

            string Session_Branch = Session["BRANCHCODE"].ToString();
            string Zone_Query = string.Empty;

            if (Session_Branch.Equals("ALL") && !Session_Branch.Contains(','))
            {
                Zone_Query = " ";
            }

            else if (!Session_Branch.Equals("ALL") && Session_Branch.Contains(','))
            {
                List<string> Branch = new List<string>();

                var elements = Session_Branch.Split(new[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);

                foreach (var item in elements)
                {
                    Branch.Add(item);
                }

                string In_Clause_Branch = string.Empty;

                foreach (var item in Branch)
                {
                    In_Clause_Branch += "'" + item + "',";
                }
                if (In_Clause_Branch.EndsWith(","))
                {
                    In_Clause_Branch = In_Clause_Branch.Remove(In_Clause_Branch.Length - 1, 1);
                }

                Zone_Query = " branchcode IN (" + In_Clause_Branch + ") AND";
            }

            else if (!Session_Branch.Equals("ALL") && !Session_Branch.Contains(','))
            {
                Zone_Query = " branchcode =" + Session_Branch + " AND";
            }

            Zone_Query += " zonecode =" + Selected_Zone + " AND";


            string query = " Select branchcode, name from branches where" + Zone_Query + " status ='1'";

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

        public DataTable Get_Zones()
        {
            string Session_Zone = Session["ZONECODE"].ToString();
            string Zone_Query = string.Empty;

            if (Session_Zone.Equals("ALL") && !Session_Zone.Contains(','))
            {
                Zone_Query = " ";
            }

            else if (!Session_Zone.Equals("ALL") && Session_Zone.Contains(','))
            {
                List<string> Zone = new List<string>();

                var elements = Session_Zone.Split(new[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);

                foreach (var item in elements)
                {
                    Zone.Add(item);
                }

                string In_Clause_Zone = string.Empty;

                foreach (var item in Zone)
                {
                    In_Clause_Zone += "'" + item + "',";
                }
                if (In_Clause_Zone.EndsWith(","))
                {
                    In_Clause_Zone = In_Clause_Zone.Remove(In_Clause_Zone.Length - 1, 1);
                }

                Zone_Query = " zonecode IN (" + In_Clause_Zone + ") AND";
            }

            else if (!Session_Zone.Equals("ALL") && !Session_Zone.Contains(','))
            {
                Zone_Query = " zonecode = " + Session_Zone + " AND";
            }

            string query = " Select zonecode, name from Zones where" + Zone_Query + " status ='1' AND Region IS NOT NULL";

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

        public DataTable Get_Flag()
        {
            string query = " Select Id, UPPER(Flag) 'Flag' from PC_IOU_Flag Where status ='1'";

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

        public void BindDataTableToRepeater(DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                rp_PC_IOU_DBT.DataSource = dt;
                rp_PC_IOU_DBT.DataBind();
            }
        }

        protected void rp_PC_IOU_DBT_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Edit")
            {

                Label ID = e.Item.FindControl("Id") as Label;
                string id = ID.Text.ToString();
                Label IOUNumber = e.Item.FindControl("lbl_IOUNumber") as Label;
                string Text_IOUNumber = IOUNumber.Text.ToString();
                Label AID = e.Item.FindControl("lblRowNumber") as Label;
                string aid = AID.Text.ToString();
                HiddenField Flag = e.Item.FindControl("hf_Flag_ID") as HiddenField;
                string Text_Flag_ID = Flag.Value.ToString();
                Label Employee_Code = e.Item.FindControl("lbl_Employee_Code") as Label;
                string Text_Employee_Code = Employee_Code.Text.ToString();
                Label Employee_Name = e.Item.FindControl("lbl_Employee_Name") as Label;
                string Text_Employee_Name = Employee_Name.Text.ToString();
                Label DateIOU = e.Item.FindControl("lbl_Date_IOU") as Label;
                string Text_IOUDate = DateIOU.Text.ToString();
                HiddenField Zone = e.Item.FindControl("hf_Zone_ID") as HiddenField;
                string Text_Zone_ID = Zone.Value.ToString();
                HiddenField Branch = e.Item.FindControl("hf_Branch_ID") as HiddenField;
                string Text_Branch_ID = Branch.Value.ToString();
                Label Amount = e.Item.FindControl("lbl_Amount") as Label;
                string Text_Amount = Amount.Text.ToString();
                Label Reason = e.Item.FindControl("lbl_Reason") as Label;
                string Text_Reason = Reason.Text.ToString();
                HiddenField Long_Reason = e.Item.FindControl("hf_Long_Reason") as HiddenField;
                string Text_Long_Reason = Long_Reason.Value.ToString();
                Label Remarks = e.Item.FindControl("lbl_Remarks") as Label;
                string Text_Remarks = Remarks.Text.ToString();
                HiddenField Long_Remarks = e.Item.FindControl("hf_Long_Remarks") as HiddenField;
                string Text_Long_Remarks = Long_Remarks.Value.ToString();

                Label CreateOn = e.Item.FindControl("lbl_CreateOn") as Label;
                string Text_CreateOn = CreateOn.Text.ToString();

                DateTime Check_Date = DateTime.Parse(Text_CreateOn);

                bool IsCurrentDay = false;

                hf_Create_Date.Value = Text_CreateOn;
                hf_ID.Value = id;
                hf_EditClickCheck.Value = "1";
                hf_FlagID.Value = Text_Flag_ID;
                //hf_IOUNumber.Value = Text_IOUNumber;

                if (Check_Date.Day.Equals(DateTime.Now.Day)
                    && Check_Date.Month.Equals(DateTime.Now.Month)
                    && Check_Date.Year.Equals(DateTime.Now.Year))
                {
                    IsCurrentDay = true;
                }

                if (IsCurrentDay == false)
                {
                    dd_Status.Enabled = false;
                    txt_IOUNO.Enabled = false;

                    dd_Status.SelectedValue = Text_Flag_ID;
                    txt_IOUNO.Text = Text_IOUNumber;
                    txt_Employee_Code.Text = Text_Employee_Code;
                    txt_Employee_Name.Text = Text_Employee_Name;
                    txt_IOUDate.Text = Text_IOUDate;
                    dd_Zone.SelectedValue = Text_Zone_ID;
                    PopulateBranches(Get_Zone_Related_Branches());
                    dd_Branch.SelectedValue = Text_Branch_ID;
                    txt_Amount.Text = Text_Amount;
                    txt_Reason.Value = Text_Long_Reason;
                    txt_Remarks.Value = Text_Long_Remarks;

                    decimal Total_SettledAmount = 0M;
                    DataTable dt = Get_IOU_Number_Related_Data();

                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (dt.Rows[i][8].ToString() == "1")
                            {
                                hf_OutstandingAmount.Value = dt.Rows[0][5].ToString();
                                if (dt.Rows[i][8].ToString() != "2")
                                {
                                    hf_SettledAmount.Value = "0";
                                }
                            }
                            else if (dt.Rows[i][8].ToString() == "2")
                            {
                                if (string.IsNullOrEmpty(dt.Rows[i][8].ToString()))
                                {
                                    hf_SettledAmount.Value = "0";
                                }
                                else if (!string.IsNullOrEmpty(dt.Rows[i][8].ToString()))
                                {
                                    decimal Temp_SettledAmount = decimal.Parse(dt.Rows[i][5].ToString());
                                    Total_SettledAmount += Temp_SettledAmount;
                                    hf_SettledAmount.Value = Total_SettledAmount.ToString();
                                }
                            }
                        }
                    }

                    if (Text_Flag_ID == "1")
                    {
                        lbl_IOUNO_.Visible = true;
                        txt_IOUNO_.Visible = true;
                        txt_Employee_Code.Enabled = true;
                        txt_Employee_Name.Enabled = true;
                        txt_IOUDate.Enabled = true;
                        Popup_Button2.Visible = true;
                    }
                    else if (Text_Flag_ID == "2")
                    {
                        lbl_IOUNO_.Visible = true;
                        txt_IOUNO_.Visible = true;
                        txt_Employee_Code.Enabled = true;
                        txt_Employee_Name.Enabled = true;
                        txt_IOUDate.Enabled = true;
                        Popup_Button2.Visible = true;

                        decimal Remaining_Amount = decimal.Parse(hf_SettledAmount.Value) - decimal.Parse(hf_OutstandingAmount.Value);
                        if (Remaining_Amount < 0)
                        {
                            Remaining_Amount = Remaining_Amount * -1;
                        }
                        lbl_RemainAmount.Text = "Remaining Amount: " + Remaining_Amount.ToString();
                        lbl_RemainAmount.ForeColor = System.Drawing.Color.Red;
                    }

                    lbl_Message.Text = "Unable To Edit Record!";
                    lbl_Message.ForeColor = System.Drawing.Color.Red;
                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Unable To Edit Record!')", true);

                    return;
                }

                else if (IsCurrentDay == true)
                {
                    dd_Status.Enabled = false;
                    txt_IOUNO.Enabled = false;

                    dd_Status.SelectedValue = Text_Flag_ID;
                    txt_IOUNO.Text = Text_IOUNumber;
                    txt_Employee_Code.Text = Text_Employee_Code;
                    txt_Employee_Name.Text = Text_Employee_Name;
                    txt_IOUDate.Text = Text_IOUDate;
                    dd_Zone.SelectedValue = Text_Zone_ID;
                    PopulateBranches(Get_Zone_Related_Branches());
                    dd_Branch.SelectedValue = Text_Branch_ID;
                    txt_Amount.Text = Text_Amount;
                    txt_Reason.Value = Text_Reason;
                    txt_Remarks.Value = Text_Remarks;

                    decimal Total_SettledAmount = 0M;
                    DataTable dt = Get_IOU_Number_Related_Data();

                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (dt.Rows[i][8].ToString() == "1")
                            {
                                hf_OutstandingAmount.Value = dt.Rows[0][5].ToString();
                                if (dt.Rows[i][8].ToString() != "2")
                                {
                                    hf_SettledAmount.Value = "0";
                                }
                            }
                            else if (dt.Rows[i][8].ToString() == "2")
                            {
                                if (string.IsNullOrEmpty(dt.Rows[i][8].ToString()))
                                {
                                    hf_SettledAmount.Value = "0";
                                }
                                else if (!string.IsNullOrEmpty(dt.Rows[i][8].ToString()))
                                {
                                    decimal Temp_SettledAmount = decimal.Parse(dt.Rows[i][5].ToString());
                                    Total_SettledAmount += Temp_SettledAmount;
                                    hf_SettledAmount.Value = Total_SettledAmount.ToString();
                                }
                            }
                        }
                    }

                    if (Text_Flag_ID == "1")
                    {
                        lbl_IOUNO_.Visible = true;
                        txt_IOUNO_.Visible = true;
                        txt_Employee_Code.Enabled = true;
                        txt_Employee_Name.Enabled = true;
                        txt_IOUDate.Enabled = true;
                        Popup_Button2.Visible = true;
                    }
                    else if (Text_Flag_ID == "2")
                    {
                        lbl_IOUNO_.Visible = true;
                        txt_IOUNO_.Visible = true;
                        txt_Employee_Code.Enabled = true;
                        txt_Employee_Name.Enabled = true;
                        txt_IOUDate.Enabled = true;
                        Popup_Button2.Visible = true;

                        decimal Remaining_Amount = decimal.Parse(hf_SettledAmount.Value) - decimal.Parse(hf_OutstandingAmount.Value);
                        if (Remaining_Amount < 0)
                        {
                            Remaining_Amount = Remaining_Amount * -1;
                        }
                        lbl_RemainAmount.Text = "Remaining Amount: " + Remaining_Amount.ToString();
                        lbl_RemainAmount.ForeColor = System.Drawing.Color.Red;
                    }

                    return;
                }
            }
        }

        protected void rp_PC_IOU_DBT_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                (e.Item.FindControl("lblRowNumber") as Label).Text = (e.Item.ItemIndex + 1).ToString();

                //DataTable dt = Get_PC_IOU();
                //if (dt.Rows.Count > 0)
                //{
                //    DataRow dr = dt.Rows[dt.Rows.Count - 1];
                //    dr["ActualID"] = (e.Item.FindControl("lblRowNumber") as Label).Text.ToString();
                //    dt.AcceptChanges();
                //}
            }
        }

        protected void btn_save_Click(object sender, EventArgs e)
        {

            string Employee_Code = txt_Employee_Code.Text.ToString();
            string Employee_Name = txt_Employee_Name.Text.ToString();
            string IOU_Number = txt_IOUNO.Text.ToString();
            string IOU_Date = txt_IOUDate.Text.ToString();
            string Amount = txt_Amount.Text.ToString();
            string Status_Code = dd_Status.SelectedValue.ToString();
            string Status_Name = dd_Status.SelectedItem.Text.ToString();
            string Zone_Code = dd_Zone.SelectedValue.ToString();
            string Zone_Name = dd_Zone.SelectedItem.Text.ToString();
            string Branch_Code = dd_Branch.SelectedValue.ToString();
            string Branch_Name = dd_Branch.SelectedItem.Text.ToString();
            string Reason = txt_Reason.Value.ToString();
            string Remarks = txt_Remarks.Value.ToString();

            #region Validation

            if (Employee_Code.Length == 0)
            {
                lbl_Message.Text = "Enter Employee Code";
                lbl_Message.ForeColor = System.Drawing.Color.Red;

                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter Employee Code')", true);
                txt_Employee_Code.Focus();

                if (dd_Status.SelectedValue == "1")
                {
                    txt_IOUNO.Enabled = false;
                    lbl_IOUNO_.Visible = false;
                    txt_IOUNO_.Visible = false;
                    //lbl_IOUNO_.Style.Add(HtmlTextWriterStyle.Visibility, "visible");
                    //txt_IOUNO_.Style.Add(HtmlTextWriterStyle.Visibility, "visible");
                }
                else if (dd_Status.SelectedValue == "2")
                {
                    txt_IOUNO.Enabled = true;
                    lbl_IOUNO_.Visible = true;
                    txt_IOUNO_.Visible = true;
                    //lbl_IOUNO_.Style.Add(HtmlTextWriterStyle.Visibility, "visible");
                    //txt_IOUNO_.Style.Add(HtmlTextWriterStyle.Visibility, "visible");
                }

                return;
            }

            if (Employee_Name.Length == 0)
            {
                lbl_Message.Text = "Enter Employee Name";
                lbl_Message.ForeColor = System.Drawing.Color.Red;

                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter Employee Code')", true);
                txt_Employee_Code.Focus();

                if (dd_Status.SelectedValue == "1")
                {
                    txt_IOUNO.Enabled = false;
                    lbl_IOUNO_.Visible = false;
                    txt_IOUNO_.Visible = false;
                    //lbl_IOUNO_.Style.Add(HtmlTextWriterStyle.Visibility, "visible");
                    //txt_IOUNO_.Style.Add(HtmlTextWriterStyle.Visibility, "visible");
                }
                else if (dd_Status.SelectedValue == "2")
                {
                    txt_IOUNO.Enabled = true;
                    lbl_IOUNO_.Visible = true;
                    txt_IOUNO_.Visible = true;
                    //lbl_IOUNO_.Style.Add(HtmlTextWriterStyle.Visibility, "visible");
                    //txt_IOUNO_.Style.Add(HtmlTextWriterStyle.Visibility, "visible");
                }

                return;
            }

            if (Status_Code == "2")
            {
                if (IOU_Number.Length == 0)
                {
                    lbl_Message.Text = "Enter IOU Number";
                    lbl_Message.ForeColor = System.Drawing.Color.Red;

                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter IOU Number')", true);
                    txt_IOUNO.Focus();

                    if (dd_Status.SelectedValue == "1")
                    {
                        txt_IOUNO.Enabled = false;
                        lbl_IOUNO_.Visible = false;
                        txt_IOUNO_.Visible = false;
                        //lbl_IOUNO_.Style.Add(HtmlTextWriterStyle.Visibility, "visible");
                        //txt_IOUNO_.Style.Add(HtmlTextWriterStyle.Visibility, "visible");
                    }
                    else if (dd_Status.SelectedValue == "2")
                    {
                        txt_IOUNO.Enabled = true;
                        lbl_IOUNO_.Visible = true;
                        txt_IOUNO_.Visible = true;
                        //lbl_IOUNO_.Style.Add(HtmlTextWriterStyle.Visibility, "visible");
                        //txt_IOUNO_.Style.Add(HtmlTextWriterStyle.Visibility, "visible");
                    }

                    return;
                }
            }

            if (IOU_Date.Length == 0)
            {
                lbl_Message.Text = "Enter Date";
                lbl_Message.ForeColor = System.Drawing.Color.Red;

                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter Date')", true);
                txt_IOUDate.Focus();

                if (dd_Status.SelectedValue == "1")
                {
                    txt_IOUNO.Enabled = false;
                    lbl_IOUNO_.Visible = false;
                    txt_IOUNO_.Visible = false;
                    //lbl_IOUNO_.Style.Add(HtmlTextWriterStyle.Visibility, "visible");
                    //txt_IOUNO_.Style.Add(HtmlTextWriterStyle.Visibility, "visible");
                }
                else if (dd_Status.SelectedValue == "2")
                {
                    txt_IOUNO.Enabled = true;
                    lbl_IOUNO_.Visible = true;
                    txt_IOUNO_.Visible = true;
                    //lbl_IOUNO_.Style.Add(HtmlTextWriterStyle.Visibility, "visible");
                    //txt_IOUNO_.Style.Add(HtmlTextWriterStyle.Visibility, "visible");
                }

                return;
            }

            if (Zone_Code == "0")
            {
                lbl_Message.Text = "Select Zone";
                lbl_Message.ForeColor = System.Drawing.Color.Red;

                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Select Zone')", true);
                dd_Zone.Focus();

                if (dd_Status.SelectedValue == "1")
                {
                    txt_IOUNO.Enabled = false;
                    lbl_IOUNO_.Visible = false;
                    txt_IOUNO_.Visible = false;
                    //lbl_IOUNO_.Style.Add(HtmlTextWriterStyle.Visibility, "visible");
                    //txt_IOUNO_.Style.Add(HtmlTextWriterStyle.Visibility, "visible");
                }
                else if (dd_Status.SelectedValue == "2")
                {
                    txt_IOUNO.Enabled = true;
                    lbl_IOUNO_.Visible = true;
                    txt_IOUNO_.Visible = true;
                    //lbl_IOUNO_.Style.Add(HtmlTextWriterStyle.Visibility, "visible");
                    //txt_IOUNO_.Style.Add(HtmlTextWriterStyle.Visibility, "visible");
                }

                return;
            }

            if (Branch_Code == "0")
            {
                lbl_Message.Text = "Select Branch";
                lbl_Message.ForeColor = System.Drawing.Color.Red;

                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Select Branch')", true);
                dd_Branch.Focus();

                if (dd_Status.SelectedValue == "1")
                {
                    txt_IOUNO.Enabled = false;
                    lbl_IOUNO_.Visible = false;
                    txt_IOUNO_.Visible = false;
                    //lbl_IOUNO_.Style.Add(HtmlTextWriterStyle.Visibility, "visible");
                    //txt_IOUNO_.Style.Add(HtmlTextWriterStyle.Visibility, "visible");
                }
                else if (dd_Status.SelectedValue == "2")
                {
                    txt_IOUNO.Enabled = true;
                    lbl_IOUNO_.Visible = true;
                    txt_IOUNO_.Visible = true;
                    //lbl_IOUNO_.Style.Add(HtmlTextWriterStyle.Visibility, "visible");
                    //txt_IOUNO_.Style.Add(HtmlTextWriterStyle.Visibility, "visible");
                }

                return;
            }

            if (Amount.Length == 0)
            {
                lbl_Message.Text = "Enter Amount";
                lbl_Message.ForeColor = System.Drawing.Color.Red;

                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter Amount')", true);
                txt_Amount.Focus();

                if (dd_Status.SelectedValue == "1")
                {
                    txt_IOUNO.Enabled = false;
                    lbl_IOUNO_.Visible = false;
                    txt_IOUNO_.Visible = false;
                    //lbl_IOUNO_.Style.Add(HtmlTextWriterStyle.Visibility, "visible");
                    //txt_IOUNO_.Style.Add(HtmlTextWriterStyle.Visibility, "visible");
                }
                else if (dd_Status.SelectedValue == "2")
                {
                    txt_IOUNO.Enabled = true;
                    lbl_IOUNO_.Visible = true;
                    txt_IOUNO_.Visible = true;
                    //lbl_IOUNO_.Style.Add(HtmlTextWriterStyle.Visibility, "visible");
                    //txt_IOUNO_.Style.Add(HtmlTextWriterStyle.Visibility, "visible");
                }

                return;
            }

            #endregion

            string ID = string.Empty;
            string CreateOn = string.Empty;
            bool Check_Edit = false;
            DateTime Check_Date = DateTime.MinValue;

            if (hf_EditClickCheck != null)
            {
                Check_Edit = ReturnBoolean(hf_EditClickCheck.Value);
            }

            SqlConnection con = new SqlConnection(clvar.Strcon());

            #region When User Do Not Click on Edit Button

            /*
            if (Check_Edit == false)
            {
                if (ID == "" || ID == null || string.IsNullOrEmpty(ID))
                {
                    try
                    {
                        Populate_IOUNumber();

                        con.Open();

                        string sql = "INSERT INTO [dbo].[PC_IOU] \n"
                   + "           ([IOUNumber] \n"
                   + "           ,[EmployeeCode] \n"
                   + "           ,[DateIOU] \n"
                   + "           ,[Reason] \n"
                   + "           ,[Amount] \n"
                   + "           ,[Flag] \n"
                   + "           ,[Remarks] \n"
                   + "           ,[Zone] \n"
                   + "           ,[Branch] \n"
                   + "           ,[CreateOn] \n"
                   + "           ,[CreateBy]) \n"
                   + "     VALUES \n"
                   + "           ('" + hf_IOUNumber.Value.ToString() + "' \n"
                   + "           ,'" + Employee_Code + "' \n"
                   + "           ,'" + IOU_Date + "' \n"
                   + "           ,'" + Reason + "' \n"
                   + "           ,'" + Amount + "' \n"
                   + "           ,'" + Status_Code + "' \n"
                   + "           ,'" + Remarks + "' \n"
                   + "           ,'" + Zone_Code + "' \n"
                   + "           ,'" + Branch_Code + "' \n"
                   + "           ,GETDATE() \n"
                   + "           ,'" + HttpContext.Current.Session["U_ID"].ToString() + "' \n)";

                        SqlCommand orcd = new SqlCommand(sql, con);
                        orcd.CommandType = CommandType.Text;
                        orcd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        lbl_Message.Text = ex.Message;
                        lbl_Message.ForeColor = System.Drawing.Color.Red;
                        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
                    }
                    finally
                    {
                        con.Close();
                        lbl_Message.Text = "Record has been saved successfully!<br />IOU Number: " + hf_IOUNumber.Value.ToString();
                        lbl_Message.ForeColor = System.Drawing.Color.Red;
                    }
                }
            }
            */

            #endregion

            #region When User Click on Edit Button

            /*
            if (Check_Edit == true)
            {
                if (hf_ID != null)
                {
                    ID = hf_ID.Value.ToString();
                }
                if (hf_Create_Date != null)
                {
                    CreateOn = hf_Create_Date.Value.ToString();
                    Check_Date = DateTime.Parse(CreateOn);
                }

                bool IsCurrentDay = false;

                if (Check_Date.Day.Equals(DateTime.Now.Day)
                    && Check_Date.Month.Equals(DateTime.Now.Month)
                    && Check_Date.Year.Equals(DateTime.Now.Year))
                {
                    IsCurrentDay = true;
                }

                if (IsCurrentDay == false)
                {
                    //hf_Create_Date.Value = "";
                    //hf_ID.Value = "";
                    //hf_EditClickCheck.Value = "0";
                    //hf_IOUNumber.Value = "";

                    //dd_Status.Enabled = false;

                    //txt_Employee_Code.Enabled = false;
                    //txt_IOUDate.Enabled = false;

                    //txt_Employee_Code.Text = null;
                    //txt_IOUDate.Text = null;
                    //txt_Amount.Text = null;
                    //txt_Remarks.Value = null;
                    //txt_Reason.Value = null;
                    //dd_Zone.SelectedValue = "0";
                    //dd_Branch.SelectedValue = "0";

                    lbl_Message.Text = "Unable To Edit Record!";
                    lbl_Message.ForeColor = System.Drawing.Color.Red;

                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Unable To Edit Record!')", true);
                    return;
                }

                else if (IsCurrentDay == true)
                {
                    if (ID != "" || ID != null || !string.IsNullOrEmpty(ID))
                    {
                        try
                        {
                            con.Open();
                            string sql = "UPDATE PC_IOU \n"
                                           + "SET    EmployeeCode = '" + Employee_Code + "', \n"
                                           + "       DateIOU = '" + txt_IOUDate.Text.ToString() + "', \n"
                                           + "       Reason = '" + Reason + "', \n"
                                           + "       Amount = '" + Amount + "', \n"
                                           + "       Flag = '" + Status_Code + "', \n"
                                           + "       Remarks = '" + Remarks + "', \n"
                                           + "       Zone = '" + Zone_Code + "', \n"
                                           + "       Branch = '" + Branch_Code + "', \n"
                                           + "       ModifyOn = GETDATE(), \n"
                                           + "       Modifyby = '" + HttpContext.Current.Session["U_ID"].ToString() + "' \n"
                                           + "       WHERE Id = '" + ID + "'";

                            SqlCommand orcd = new SqlCommand(sql, con);
                            orcd.CommandType = CommandType.Text;
                            orcd.ExecuteNonQuery();
                        }

                        catch (Exception ex)
                        {
                            lbl_Message.Text = ex.Message;
                            lbl_Message.ForeColor = System.Drawing.Color.Red;
                            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
                        }
                        finally
                        {
                            con.Close();
                            lbl_Message.Text = "Record has been saved successfully!";
                            lbl_Message.ForeColor = System.Drawing.Color.Red;
                        }
                    }
                }
            }
            */

            #endregion

            #region When User Do Not Click on Edit Button

            if (Check_Edit == false)
            {
                #region Outstanding


                if (txt_Employee_Code.Enabled == true)
                {
                    if (ID == "" || ID == null || string.IsNullOrEmpty(ID))
                    {
                        try
                        {
                            Populate_IOUNumber();

                            con.Open();

                            string sql = "INSERT INTO [dbo].[PC_IOU] \n"
                       + "           ([IOUNumber] \n"
                       + "           ,[EmployeeCode] \n"
                       + "           ,[EmployeeName] \n"
                       + "           ,[DateIOU] \n"
                       + "           ,[Reason] \n"
                       + "           ,[Amount] \n"
                       + "           ,[Flag] \n"
                       + "           ,[Remarks] \n"
                       + "           ,[Zone] \n"
                       + "           ,[Branch] \n"
                       + "           ,[CreateOn] \n"
                       + "           ,[CreateBy]) \n"
                       + "     VALUES \n"
                       + "           ('" + hf_IOUNumber.Value.ToString() + "' \n"
                       + "           ,'" + Employee_Code + "' \n"
                       + "           ,'" + Employee_Name + "' \n"
                       + "           ,'" + IOU_Date + "' \n"
                       + "           ,'" + Reason + "' \n"
                       + "           ,'" + Amount + "' \n"
                       + "           ,'" + Status_Code + "' \n"
                       + "           ,'" + Remarks + "' \n"
                       + "           ,'" + Zone_Code + "' \n"
                       + "           ,'" + Branch_Code + "' \n"
                       + "           ,GETDATE() \n"
                       + "           ,'" + HttpContext.Current.Session["U_ID"].ToString() + "' \n)";

                            SqlCommand orcd = new SqlCommand(sql, con);
                            orcd.CommandType = CommandType.Text;
                            orcd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            lbl_Message.Text = ex.Message;
                            lbl_Message.ForeColor = System.Drawing.Color.Red;
                            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
                        }
                        finally
                        {
                            con.Close();
                            lbl_Message.Text = "Record has been saved successfully!<br />IOU Number: " + hf_IOUNumber.Value.ToString();
                            lbl_Message.ForeColor = System.Drawing.Color.Red;
                        }
                    }
                }


                #endregion

                #region Settled


                else if (txt_Employee_Code.Enabled == false)
                {
                    if (ID == "" || ID == null || string.IsNullOrEmpty(ID))
                    {
                        if (hf_ID != null)
                        {
                            ID = hf_ID.Value.ToString();
                        }
                        if (hf_Create_Date != null)
                        {
                            CreateOn = hf_Create_Date.Value.ToString();
                            Check_Date = DateTime.Parse(CreateOn);
                        }

                        bool IsCurrentDay = false;

                        if (Check_Date.Day.Equals(DateTime.Now.Day)
                            && Check_Date.Month.Equals(DateTime.Now.Month)
                            && Check_Date.Year.Equals(DateTime.Now.Year))
                        {
                            IsCurrentDay = true;
                        }

                        if (IsCurrentDay == false)
                        {
                            lbl_Message.Text = "Unable To Edit Record!";
                            lbl_Message.ForeColor = System.Drawing.Color.Red;

                            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Unable To Edit Record!')", true);
                            return;
                        }

                        else if (IsCurrentDay == true)
                        {
                            decimal Text_Amount = decimal.Parse(txt_Amount.Text);
                            Text_Amount += decimal.Parse(hf_SettledAmount.Value);

                            if (Text_Amount > decimal.Parse(hf_OutstandingAmount.Value))
                            {
                                lbl_Message.Text = "Settling Amount can not be greater than Outstanding Amount!";
                                lbl_Message.ForeColor = System.Drawing.Color.Red;
                                return;
                            }
                            else
                                try
                                {
                                    con.Open();

                                    string sql = "INSERT INTO [dbo].[PC_IOU] \n"
                                                   + "           ([IOUNumber] \n"
                                                   + "           ,[EmployeeCode] \n"
                                                   + "           ,[EmployeeName] \n"
                                                   + "           ,[DateIOU] \n"
                                                   + "           ,[Reason] \n"
                                                   + "           ,[Amount] \n"
                                                   + "           ,[Flag] \n"
                                                   + "           ,[Remarks] \n"
                                                   + "           ,[Zone] \n"
                                                   + "           ,[Branch] \n"
                                                   + "           ,[CreateOn] \n"
                                                   + "           ,[CreateBy]) \n"
                                                   + "     VALUES \n"
                                                   + "           ('" + IOU_Number + "' \n"
                                                   + "           ,'" + Employee_Code + "' \n"
                                                   + "           ,'" + Employee_Name + "' \n"
                                                   + "           ,'" + IOU_Date + "' \n"
                                                   + "           ,'" + Reason + "' \n"
                                                   + "           ,'" + Amount + "' \n"
                                                   + "           ,'" + Status_Code + "' \n"
                                                   + "           ,'" + Remarks + "' \n"
                                                   + "           ,'" + Zone_Code + "' \n"
                                                   + "           ,'" + Branch_Code + "' \n"
                                                   + "           ,GETDATE() \n"
                                                   + "           ,'" + HttpContext.Current.Session["U_ID"].ToString() + "' \n)";

                                    SqlCommand orcd = new SqlCommand(sql, con);
                                    orcd.CommandType = CommandType.Text;
                                    orcd.ExecuteNonQuery();
                                }

                                catch (Exception ex)
                                {
                                    lbl_Message.Text = ex.Message;
                                    lbl_Message.ForeColor = System.Drawing.Color.Red;
                                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
                                }
                                finally
                                {
                                    con.Close();
                                    lbl_Message.Text = "Record has been saved successfully!";
                                    lbl_Message.ForeColor = System.Drawing.Color.Red;
                                }
                        }
                    }
                }


                #endregion
            }


            #endregion

            #region When User Click on Edit Button

            if (Check_Edit == true)
            {
                if (hf_ID != null)
                {
                    ID = hf_ID.Value.ToString();
                }
                if (hf_Create_Date != null)
                {
                    CreateOn = hf_Create_Date.Value.ToString();
                    Check_Date = DateTime.Parse(CreateOn);
                }

                bool IsCurrentDay = false;

                if (Check_Date.Day.Equals(DateTime.Now.Day)
                    && Check_Date.Month.Equals(DateTime.Now.Month)
                    && Check_Date.Year.Equals(DateTime.Now.Year))
                {
                    IsCurrentDay = true;
                }

                if (IsCurrentDay == false)
                {
                    //hf_Create_Date.Value = "";
                    //hf_ID.Value = "";
                    //hf_EditClickCheck.Value = "0";
                    //hf_IOUNumber.Value = "";

                    //dd_Status.Enabled = false;

                    //txt_Employee_Code.Enabled = false;
                    //txt_IOUDate.Enabled = false;

                    //txt_Employee_Code.Text = null;
                    //txt_IOUDate.Text = null;
                    //txt_Amount.Text = null;
                    //txt_Remarks.Value = null;
                    //txt_Reason.Value = null;
                    //dd_Zone.SelectedValue = "0";
                    //dd_Branch.SelectedValue = "0";

                    lbl_Message.Text = "Unable To Edit Record!";
                    lbl_Message.ForeColor = System.Drawing.Color.Red;

                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Unable To Edit Record!')", true);
                    return;
                }

                else if (IsCurrentDay == true)
                {
                    if (ID != "" || ID != null || !string.IsNullOrEmpty(ID))
                    {
                        if (hf_FlagID.Value == "2")
                        {
                            decimal Text_Amount = decimal.Parse(txt_Amount.Text);
                            decimal TotalSettledAmount = decimal.Parse(hf_SettledAmount.Value);
                            decimal TotalAdjustedAmount = TotalSettledAmount - Text_Amount;

                            if (TotalAdjustedAmount > decimal.Parse(hf_OutstandingAmount.Value))
                            {
                                lbl_Message.Text = "Settling Amount can not be greater than Outstanding Amount!";
                                lbl_Message.ForeColor = System.Drawing.Color.Red;
                                return;
                            }
                        }
                        try
                        {
                            con.Open();
                            string sql = "UPDATE PC_IOU \n"
                                           + "SET    EmployeeCode = '" + Employee_Code + "', \n"
                                           + "       EmployeeName = '" + Employee_Name + "', \n"
                                           + "       DateIOU = '" + txt_IOUDate.Text.ToString() + "', \n"
                                           + "       Reason = '" + Reason + "', \n"
                                           + "       Amount = '" + Amount + "', \n"
                                           + "       Flag = '" + Status_Code + "', \n"
                                           + "       Remarks = '" + Remarks + "', \n"
                                           + "       Zone = '" + Zone_Code + "', \n"
                                           + "       Branch = '" + Branch_Code + "', \n"
                                           + "       ModifyOn = GETDATE(), \n"
                                           + "       Modifyby = '" + HttpContext.Current.Session["U_ID"].ToString() + "' \n"
                                           + "       WHERE Id = '" + ID + "'";

                            SqlCommand orcd = new SqlCommand(sql, con);
                            orcd.CommandType = CommandType.Text;
                            orcd.ExecuteNonQuery();
                        }

                        catch (Exception ex)
                        {
                            lbl_Message.Text = ex.Message;
                            lbl_Message.ForeColor = System.Drawing.Color.Red;
                            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
                        }
                        finally
                        {
                            con.Close();
                            lbl_Message.Text = "Record has been updated successfully!";
                            lbl_Message.ForeColor = System.Drawing.Color.Red;
                        }
                    }
                }
            }


            #endregion


            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record has been saved')", true);

            BindDataTableToRepeater(Get_PC_IOU());

            //Clear Fields

            lbl_RemainAmount.Text = "";
            lbl_RemainAmount.ForeColor = System.Drawing.Color.Black;

            hf_Create_Date.Value = "";
            hf_ID.Value = "";
            hf_EditClickCheck.Value = "0";
            hf_IOUNumber.Value = "";
            hf_SettledAmount.Value = "";
            hf_OutstandingAmount.Value = "";
            hf_FlagID.Value = "";

            txt_Employee_Code.Enabled = true;
            txt_IOUDate.Enabled = true;

            txt_Employee_Code.Text = null;
            txt_Employee_Name.Text = null;
            txt_IOUDate.Text = null;
            txt_Amount.Text = null;
            txt_Remarks.Value = null;
            txt_Reason.Value = null;
            dd_Zone.SelectedValue = "0";
            dd_Branch.SelectedValue = "0";

            dd_Status.SelectedValue = "1";
            dd_Status.Enabled = true;


            //if (dd_Status.SelectedValue == "1")
            //{
            //    lbl_IOUNO_.Style.Add(HtmlTextWriterStyle.Visibility, "hidden");
            //    txt_IOUNO_.Style.Add(HtmlTextWriterStyle.Visibility, "hidden");
            //}

            if (dd_Status.SelectedValue == "1")
            {
                lbl_IOUNO_.Visible = false;
                txt_IOUNO_.Visible = false;
                dd_Zone.Enabled = true;
                dd_Branch.Enabled = true;
                txt_IOUNO_.Disabled = true;
            }
            else if (dd_Status.SelectedValue == "2")
            {
                lbl_IOUNO_.Visible = true;
                txt_IOUNO_.Visible = true;
                txt_IOUNO_.Disabled = false;
            }

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

        public DataTable Get_PC_IOU()
        {
            string UserID = string.Empty;

            if (Session["U_ID"] != null)
            {
                UserID = Session["U_ID"].ToString();
            }

            DataTable dt = new DataTable();

            if (UserID == "")
            {
                return dt;
            }

            string query = string.Empty;

            if (UserID != "")
            {
                query = "SELECT  \n"
                           + "       PC.[Id] 'Id', \n"
                           + "       PC.[IOUNumber] 'IOUNumber', \n"
                           + "       PC.[EmployeeCode] 'EmployeeCode', \n"
                           + "       PC.[EmployeeName] 'EmployeeName', \n"
                           + "       LEFT(CONVERT(VARCHAR(30), PC.[DateIOU], 120), 10) 'DateIOU', \n"
                           + "       LEFT(PC.[Reason], 20) 'Reason', \n"
                           + "       PC.[Reason] 'Long_Reason', \n"
                           + "       PC.[Amount] 'Amount', \n"
                           + "       UPPER(PC_Flag.[Flag]) 'Flag', \n"
                           + "       PC_Flag.[Id] 'Flag_ID', \n"
                           + "       LEFT(PC.[Remarks], 20) 'Remarks', \n"
                           + "       PC.[Remarks] 'Long_Remarks', \n"
                           + "       Z.name 'Zone', \n"
                           + "       Z.zonecode 'Zone_ID', \n"
                           + "       B.name 'Branch', \n"
                           + "       B.branchcode 'Branch_ID', \n"
                           + "       LEFT(CONVERT(VARCHAR(30), PC.[CreateOn], 120), 16) 'CreateOn' \n"
                           + " FROM [dbo].[PC_IOU] PC \n"
                           + " INNER JOIN BRANCHES B \n"
                           + " ON B.branchcode = PC.Branch \n"
                           + " INNER JOIN ZONES Z \n"
                           + " ON Z.zonecode = PC.Zone \n"
                           + " INNER JOIN PC_IOU_Flag PC_Flag \n"
                           + " ON PC_Flag.Id = PC.Flag \n"
                           + " WHERE  \n"
                           + " PC.CreateBy = '" + UserID + "' \n"
                           + " AND CAST(PC.Createon AS DATE) = CAST(GETDATE() AS DATE)  \n"
                           + " ORDER BY PC.CreateOn Desc";
            }
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                sda.Fill(dt);
            }
            catch (Exception ex)
            {
                lbl_Message.Text = ex.Message;
                lbl_Message.ForeColor = System.Drawing.Color.Red;
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
            }
            finally { con.Close(); }
            return dt;
        }

        public DataTable Get_MAX_IOUNumber()
        {
            DataTable dt = new DataTable();

            string query = string.Empty;


            query = "SELECT  \n"
                       + "       MAX(PC.[IOUNumber]) 'IOUNumber' \n"
                       + " FROM [dbo].[PC_IOU] PC ";

            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                sda.Fill(dt);
            }
            catch (Exception ex)
            {
                lbl_Message.Text = ex.Message;
                lbl_Message.ForeColor = System.Drawing.Color.Red;
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
            }
            finally { con.Close(); }
            return dt;
        }

        public DataTable Get_MAX_ID()
        {
            DataTable dt = new DataTable();

            string query = string.Empty;


            query = "SELECT  \n"
                       + "       MAX(PC.[Id]) 'Id' \n"
                       + " FROM [dbo].[PC_IOU] PC ";

            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                sda.Fill(dt);
            }
            catch (Exception ex)
            {
                lbl_Message.Text = ex.Message;
                lbl_Message.ForeColor = System.Drawing.Color.Red;
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
            }
            finally { con.Close(); }
            return dt;
        }

        public DataTable Get_IOU_Number_Related_Data()
        {
            string UserID = string.Empty;

            if (Session["U_ID"] != null)
            {
                UserID = Session["U_ID"].ToString();
            }

            DataTable dt = new DataTable();

            string query = string.Empty;


            query = "SELECT  \n"
                        + "       PC.[Id] 'Id', \n"
                        + "       PC.[EmployeeCode] 'EmployeeCode', \n"
                        + "       PC.[EmployeeName] 'EmployeeName', \n"
                        + "       LEFT(CONVERT(VARCHAR(30), PC.[DateIOU], 120), 10) 'DateIOU', \n"
                        + "       PC.[Reason] 'Reason', \n"
                        + "       PC.[Amount] 'Amount', \n"
                        + "       PC.[Remarks] 'Remarks', \n"
                        + "       PC_Flag.[Flag] 'Flag', \n"
                        + "       PC_Flag.[Id] 'Flag_ID', \n"
                        + "       Z.name 'Zone', \n"
                        + "       Z.zonecode 'Zone_ID', \n"
                        + "       B.name 'Branch', \n"
                        + "       B.branchcode 'Branch_ID', \n"
                        + "       LEFT(CONVERT(VARCHAR(30), PC.[CreateOn], 120), 16) 'CreateOn' \n"
                        + " FROM [dbo].[PC_IOU] PC \n"
                        + " INNER JOIN BRANCHES B \n"
                        + " ON B.branchcode = PC.Branch \n"
                        + " INNER JOIN ZONES Z \n"
                        + " ON Z.zonecode = PC.Zone \n"
                        + " INNER JOIN PC_IOU_Flag PC_Flag \n"
                        + " ON PC_Flag.Id = PC.Flag \n"
                        + " WHERE  \n"
                        + " PC.CreateBy = '" + UserID + "' \n"
                        + " AND PC.IOUNumber = '" + txt_IOUNO.Text.ToString() + "'";

            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                sda.Fill(dt);
            }
            catch (Exception ex)
            {
                lbl_Message.Text = ex.Message;
                lbl_Message.ForeColor = System.Drawing.Color.Red;
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
            }
            finally { con.Close(); }
            return dt;
        }

        private bool ReturnBoolean(string Name)
        {
            if (Name == "1")
            {
                return true;
            }
            else if (Name == "0")
            {
                return false;
            }
            return false;
        }

        protected void dd_Zone_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateBranches(Get_Zone_Related_Branches());

            lbl_RemainAmount.Text = "";
            lbl_RemainAmount.ForeColor = System.Drawing.Color.Black;

            lbl_Message.Text = "";
            lbl_Message.ForeColor = System.Drawing.Color.Black;

            if (dd_Status.SelectedValue == "1")
            {
                lbl_IOUNO_.Visible = false;
                txt_IOUNO_.Visible = false;
                txt_IOUNO_.Disabled = true;
            }
            else if (dd_Status.SelectedValue == "2")
            {
                lbl_IOUNO_.Visible = true;
                txt_IOUNO_.Visible = true;
                txt_IOUNO_.Disabled = false;
            }

            //lbl_IOUNO_.Style.Add(HtmlTextWriterStyle.Visibility, "hidden");
            //txt_IOUNO_.Style.Add(HtmlTextWriterStyle.Visibility, "hidden");
        }

        protected void txt_IOUNO_TextChanged(object sender, EventArgs e)
        {
            DataTable dt = Get_IOU_Number_Related_Data();
            decimal Total_SettledAmount = 0M;

            if (dt.Rows.Count > 0)
            {
                lbl_Message.Text = "";
                lbl_Message.ForeColor = System.Drawing.Color.Black;

                hf_ID.Value = dt.Rows[0][0].ToString();
                txt_Employee_Code.Text = dt.Rows[0][1].ToString();
                txt_Employee_Name.Text = dt.Rows[0][2].ToString();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i][8].ToString() == "1")
                    {
                        hf_OutstandingAmount.Value = dt.Rows[0][5].ToString();
                        if (dt.Rows[i][8].ToString() != "2")
                        {
                            hf_SettledAmount.Value = "0";
                        }
                    }
                    else if (dt.Rows[i][8].ToString() == "2")
                    {
                        if (string.IsNullOrEmpty(dt.Rows[i][8].ToString()))
                        {
                            hf_SettledAmount.Value = "0";
                        }
                        else if (!string.IsNullOrEmpty(dt.Rows[i][8].ToString()))
                        {
                            decimal Temp_SettledAmount = decimal.Parse(dt.Rows[i][5].ToString());
                            Total_SettledAmount += Temp_SettledAmount;
                            hf_SettledAmount.Value = Total_SettledAmount.ToString();
                        }
                    }
                }

                txt_IOUDate.Text = dt.Rows[0][3].ToString();
                dd_Zone.SelectedValue = dt.Rows[0][10].ToString();
                PopulateBranches(Get_Zone_Related_Branches());
                dd_Branch.SelectedValue = dt.Rows[0][12].ToString();
                hf_Create_Date.Value = dt.Rows[0][13].ToString();

                decimal Remaining_Amount = decimal.Parse(hf_SettledAmount.Value) - decimal.Parse(hf_OutstandingAmount.Value);
                if (Remaining_Amount < 0)
                {
                    Remaining_Amount = Remaining_Amount * -1;
                }
                lbl_RemainAmount.Text = "Remaining Amount: " + Remaining_Amount.ToString();
                lbl_RemainAmount.ForeColor = System.Drawing.Color.Red;

                txt_Amount.Focus();

                dd_Status.Enabled = false;
                txt_IOUNO.Enabled = false;
                dd_Zone.Enabled = false;
                dd_Branch.Enabled = false;
                txt_Employee_Code.Enabled = false;
                txt_IOUDate.Enabled = false;
                Popup_Button2.Visible = false;
            }
            else
            {
                lbl_Message.Text = "Unable to find related data!";
                lbl_Message.ForeColor = System.Drawing.Color.Red;
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Unable to find related data!')", true);
                return;
            }
        }

        protected void dd_Status_SelectedIndexChanged(object sender, EventArgs e)
        {
            TextBox Employee_Code = txt_Employee_Code;
            TextBox Employee_Name = txt_Employee_Name;
            TextBox IOUDate = txt_IOUDate;
            TextBox Amount = txt_Amount;
            DropDownList Status = dd_Status;
            DropDownList Zone = dd_Zone;
            DropDownList Branch = dd_Branch;

            ImageButton Button_Calender = Popup_Button2;

            TextBox IOUNO = txt_IOUNO;
            HiddenField HF_IOUNO = hf_IOUNumber;

            if (Status.SelectedValue == "1")
            {
                //Populate_IOUNumber();

                //IOUNO.Enabled = false;
                //IOUNO.Text = HF_IOUNO.Value;

                lbl_RemainAmount.Text = "";
                lbl_RemainAmount.ForeColor = System.Drawing.Color.Black;

                lbl_Message.Text = "";
                lbl_Message.ForeColor = System.Drawing.Color.Black;

                lbl_IOUNO_.Visible = false;
                txt_IOUNO_.Visible = false;

                Button_Calender.Visible = true;
                Button_Calender.Width = Unit.Pixel(20);

                Employee_Code.Enabled = true;
                Employee_Name.Enabled = true;
                IOUDate.Enabled = true;
                Zone.Enabled = true;
                Branch.Enabled = true;
                Employee_Code.Text = "";
                Employee_Name.Text = "";
                IOUDate.Text = "";
                Amount.Text = "";
                Zone.SelectedValue = "0";
                Branch.SelectedValue = "0";
                txt_Reason.Value = "";
                txt_Remarks.Value = "";

            }
            else if (Status.SelectedValue == "2")
            {
                IOUNO.Enabled = true;
                IOUNO.Text = "";

                lbl_RemainAmount.Text = "";
                lbl_RemainAmount.ForeColor = System.Drawing.Color.Black;

                lbl_Message.Text = "";
                lbl_Message.ForeColor = System.Drawing.Color.Black;

                lbl_IOUNO_.Visible = true;
                txt_IOUNO_.Visible = true;

                Button_Calender.Visible = false;

                Employee_Code.Enabled = false;
                Employee_Name.Enabled = false;
                IOUDate.Enabled = false;
                Zone.Enabled = false;
                Branch.Enabled = false;
                Employee_Code.Text = "";
                Employee_Name.Text = "";
                IOUDate.Text = "";
                Amount.Text = "";
                Zone.SelectedValue = "0";
                Branch.SelectedValue = "0";
                txt_Reason.Value = "";
                txt_Remarks.Value = "";
            }
        }

        protected void btn_reset_Click(object sender, EventArgs e)
        {
            TextBox Employee_Code = txt_Employee_Code;
            TextBox Employee_Name = txt_Employee_Name;
            TextBox IOUDate = txt_IOUDate;
            TextBox Amount = txt_Amount;
            DropDownList Status = dd_Status;
            DropDownList Zone = dd_Zone;
            DropDownList Branch = dd_Branch;

            ImageButton Button_Calender = Popup_Button2;

            TextBox IOUNO = txt_IOUNO;
            HiddenField HF_IOUNO = hf_IOUNumber;

            hf_Create_Date.Value = "";
            hf_ID.Value = "";
            hf_EditClickCheck.Value = "0";
            HF_IOUNO.Value = "";
            hf_SettledAmount.Value = "";
            hf_OutstandingAmount.Value = "";
            hf_FlagID.Value = "";

            Employee_Code.Text = "";
            Employee_Name.Text = "";
            IOUDate.Text = "";
            Amount.Text = "";
            Zone.Enabled = true;
            Zone.SelectedValue = "0";
            Branch.Enabled = true;
            Branch.SelectedValue = "0";
            txt_Reason.Value = "";
            txt_Remarks.Value = "";
            Status.SelectedValue = "1";

            if (Status.SelectedValue == "1")
            {
                lbl_RemainAmount.Text = "";
                lbl_RemainAmount.ForeColor = System.Drawing.Color.Black;

                lbl_Message.Text = "";
                lbl_Message.ForeColor = System.Drawing.Color.Black;

                lbl_IOUNO_.Visible = false;
                txt_IOUNO_.Visible = false;

                Employee_Code.Enabled = true;
                Employee_Name.Enabled = true;
                IOUDate.Enabled = true;
                //Populate_IOUNumber();
                Status.Enabled = true;
                Button_Calender.Visible = true;
                Button_Calender.Width = Unit.Pixel(20);
                IOUNO.Enabled = false;
                IOUNO.Text = HF_IOUNO.Value;
            }
        }
    }
}