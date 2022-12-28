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
    public partial class FAF_Tax : System.Web.UI.Page
    {

        #region Variables

        Cl_Variables clvar = new Cl_Variables();
        CommonFunction func = new CommonFunction();
        Consignemnts con = new Consignemnts();

        public static int year_from_Date_DBT, year_from_Date_Form, year_to_Date_DBT, year_to_Date_Form;

        public static int month_from_Date_DBT, month_from_Date_Form, month_to_Date_DBT, month_to_Date_Form;

        public static int day_from_Date_DBT, day_from_Date_Form, day_to_Date_DBT, day_to_Date_Form;

        public static string date_from_Date_DBT, date_from_Date_Form, date_to_Date_DBT, date_to_Date_Form;

        //public static string[] CompanyID, BranchCode, GST_Amount, Effective_From_DBT, Effective_To_DBT;

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["U_ID"].ToString() != "1786")
            {
                Response.Redirect("~/login");
            }

            if (!IsPostBack)
            {
                ServiceTypes();
                BindDataToRepeater();
                hf_Status.Value = RBL_Status.SelectedItem.Value;
            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }

        public void BindDataToRepeater()
        {
            DataTable dt = Get_FAF_Tax();
            if (dt.Rows.Count > 0)
            {
                RBL_Status.Visible = true;
                FAF_Tax_DBT_Details.Visible = true;
                CSV_btn.Visible = true;
                rp_FAF_Tax_DBT.DataSource = dt;
                rp_FAF_Tax_DBT.DataBind();
                CC_Panel.Height = Unit.Pixel(250);
            }
            else
            {
                RBL_Status.Visible = false;
                FAF_Tax_DBT_Details.Visible = false;
                CSV_btn.Visible = false;
                rp_FAF_Tax_DBT.DataSource = null;
                rp_FAF_Tax_DBT.DataBind();
                CC_Panel.Height = Unit.Pixel(0);
            }
        }

        protected void ServiceTypes()
        {
            string[] Services = new string[3];
            Services[0] = "Domestic";
            Services[1] = "International";
            Services[2] = "Road N Rail";

            DataTable ServiceType = new DataTable();
            ServiceType.Columns.AddRange(new DataColumn[2] {
            new DataColumn("ID", typeof(int)),
            new DataColumn("ServiceType", typeof(string))
        });
            ServiceType.AcceptChanges();

            for (int i = 0; i <= Services.Length - 1; i++)
            {
                DataRow Row = ServiceType.NewRow();
                int Temp_Number = i;
                Temp_Number++;
                Row[0] = Temp_Number;
                Row[1] = Services[i].ToUpper();
                ServiceType.Rows.Add(Row);
            }
            ServiceType.AcceptChanges();

            dd_ServiceType.Items.Clear();
            dd_ServiceType.Items.Add(new ListItem { Text = "SELECT PRODUCT", Value = "0" });
            if (ServiceType != null)
            {
                if (ServiceType.Rows.Count > 0)
                {
                    dd_ServiceType.DataSource = ServiceType;
                    dd_ServiceType.DataTextField = "ServiceType";
                    dd_ServiceType.DataValueField = "ID";
                    dd_ServiceType.DataBind();
                }
            }
        }

        protected void btn_save_Click(object sender, EventArgs e)
        {

            string ServiceTypeID = dd_ServiceType.SelectedValue.ToString();
            string ServiceTypeName = string.Empty;
            if (dd_ServiceType.SelectedItem != null)
            {
                ServiceTypeName = dd_ServiceType.SelectedItem.Text.ToString();
            }
            string Tax = txt_Tax.Text.ToString();
            string StartDate = dd_From_Date.Text.ToString();
            string EndDate = dd_To_Date.Text.ToString();

            hf_Status.Value = RBL_Status.SelectedItem.Value;

            #region Validation


            if (ServiceTypeID == "0")
            {
                lbl_Message.Text = "Select Service Type!";
                lbl_Message.ForeColor = System.Drawing.Color.Red;
                dd_ServiceType.Focus();
                return;
            }

            if (Tax.Length == 0)
            {
                lbl_Message.Text = "Enter Tax!";
                lbl_Message.ForeColor = System.Drawing.Color.Red;
                txt_Tax.Focus();
                return;

            }
            if (Tax.Length > 0)
            {
                if (Tax.StartsWith("."))
                {
                    lbl_Message.Text = "Invalid Tax Amount!";
                    lbl_Message.ForeColor = System.Drawing.Color.Red;
                    txt_Tax.Focus();
                    return;
                }

            }

            if (StartDate.Length == 0)
            {
                lbl_Message.Text = "Select Effective From Date!";
                lbl_Message.ForeColor = System.Drawing.Color.Red;
                dd_From_Date.Focus();
                return;

            }
            if (StartDate.Length > 0)
            {
                if (StartDate.StartsWith("-"))
                {
                    lbl_Message.Text = "Invalid Date!";
                    lbl_Message.ForeColor = System.Drawing.Color.Red;
                    dd_From_Date.Focus();
                    return;
                }
            }

            if (EndDate.Length == 0)
            {
                lbl_Message.Text = "Select Effective To Date!";
                lbl_Message.ForeColor = System.Drawing.Color.Red;
                dd_To_Date.Focus();
                return;
            }
            if (EndDate.Length > 0)
            {
                if (EndDate.StartsWith("-"))
                {
                    lbl_Message.Text = "Invalid Date!";
                    lbl_Message.ForeColor = System.Drawing.Color.Red;
                    dd_To_Date.Focus();
                    return;
                }
            }

            TimeSpan Difference = TimeSpan.Zero;

            if (StartDate.Length > 0 && EndDate.Length > 0)
            {

                DateTime _StartDate = Convert.ToDateTime(StartDate);
                DateTime _EndDate = Convert.ToDateTime(EndDate);

                Difference = _EndDate.Subtract(_StartDate);

                if (Difference.Days < 0)
                {
                    lbl_Message.Text = "Error: Effective To Date is selected before Effective From Date!";
                    lbl_Message.ForeColor = System.Drawing.Color.Red;
                    dd_To_Date.Focus();
                    return;
                }

                if (_EndDate < _StartDate)
                {
                    lbl_Message.Text = "Error: Effective From Date cannot be selected after Effective To Date!";
                    lbl_Message.ForeColor = System.Drawing.Color.Red;
                    dd_From_Date.Focus();
                    return;
                }

                if (_StartDate == _EndDate)
                {
                    lbl_Message.Text = "Error: Effective From Date and Effective To Date cannot be same!";
                    lbl_Message.ForeColor = System.Drawing.Color.Red;
                    dd_From_Date.Focus();
                    return;
                }

                if ((_StartDate.Month != _EndDate.Month)
                    || (_StartDate.Year != _EndDate.Year))
                {
                    lbl_Message.Text = "Error: Effective From Date and Effective To Date both are not in the same month!";
                    lbl_Message.ForeColor = System.Drawing.Color.Red;
                    dd_To_Date.Focus();
                    return;
                }
            }


            #endregion

            rp_FAF_Tax_DBT.DataSource = null;
            rp_FAF_Tax_DBT.DataBind();
            CSV_btn.Visible = false;

            CC_Panel.Height = Unit.Pixel(0);

            DataTable dt = Get_Last_FAF_Tax_ID_According_To_Service_Type();

            #region Update

            if (dt.Rows.Count > 0)
            {
                string FAF_Tax_ID = dt.Rows[0][0].ToString();
                if (!string.IsNullOrEmpty(FAF_Tax_ID))
                {
                    SqlConnection con = new SqlConnection(clvar.Strcon());

                    try
                    {
                        con.Open();

                        String sql = "UPDATE FAF_Tax \n" +
                                     "SET \n" +
                                     "Status = '0', \n" +
                                     "ModifiedOn = GETDATE(), \n" +
                                     "ModifiedBy = '" + HttpContext.Current.Session["U_ID"].ToString() + "' \n" +
                                     "WHERE FAF_Tax_ID = '" + FAF_Tax_ID + "'";

                        SqlCommand orcd = new SqlCommand(sql, con);
                        orcd.CommandType = CommandType.Text;
                        orcd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        lbl_Message.Text = ex.Message;
                        lbl_Message.ForeColor = System.Drawing.Color.Red;

                    }
                    finally { con.Close(); }
                }

            }


            #endregion

            #region Insert


            SqlConnection con_ = new SqlConnection(clvar.Strcon());
            try
            {
                con_.Open();

                String sql = "INSERT INTO FAF_Tax \n" +
                           "([ServiceTypeID]\n" +
                           ",[ServiceType]\n" +
                           ",[Tax]\n" +
                           ",[Status]\n" +
                           ",[EffectiveFrom]\n" +
                           ",[EffectiveTo]\n" +
                           ",[CreatedOn]\n" +
                           ",[CreatedBy])\n" +
                           " VALUES" +
                           "('" + ServiceTypeID + "', \n" +
                            "'" + ServiceTypeName + "', \n" +
                            "'" + Tax + "', \n" +
                            "'1', \n" +
                            "'" + DateTime.Parse(StartDate).ToString("yyyy-MM-dd") + "', \n" +
                            "'" + DateTime.Parse(EndDate).ToString("yyyy-MM-dd") + "', \n" +
                            "GETDATE(), \n" +
                            "'" + HttpContext.Current.Session["U_ID"].ToString() + "')";
                SqlCommand orcd = new SqlCommand(sql, con_);
                orcd.CommandType = CommandType.Text;
                orcd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                lbl_Message.Text = ex.Message;
                lbl_Message.ForeColor = System.Drawing.Color.Red;
            }
            finally { con_.Close(); }

            #endregion

            lbl_Message.Text = "Record has been saved successfully!";
            lbl_Message.ForeColor = System.Drawing.Color.Red;

            CC_Panel.Height = Unit.Pixel(250);
            RBL_Status.Visible = true;
            FAF_Tax_DBT_Details.Visible = true;
            DataTable MainDataTable = Get_FAF_Tax();
            rp_FAF_Tax_DBT.DataSource = MainDataTable.DefaultView;
            rp_FAF_Tax_DBT.DataBind();
            CSV_btn.Visible = true;

            //Clear Fields

            dd_ServiceType.SelectedValue = "0";
            txt_Tax.Text = "";
            dd_From_Date.Text = "";
            dd_To_Date.Text = "";

        }

        public DataTable Get_FAF_Tax()
        {
            string Session_User_ID = Session["U_ID"].ToString();
            string query = "";

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                query = "SELECT \n" +
                            "FT.FAF_Tax_ID 'FAF_Tax_ID', \n" +
                            "FT.ServiceTypeID 'ServiceTypeID', \n" +
                            "FT.ServiceType 'ServiceType', \n" +
                            "CONVERT(VARCHAR(11), FT.EffectiveFrom, 106) 'EffectiveFrom', \n" +
                            "CONVERT(VARCHAR(11), FT.EffectiveTo, 106) 'EffectiveTo', \n" +
                            "FT.Tax 'Tax', \n" +
                            "CASE WHEN FT.Status = '1' THEN 'Active' ELSE 'In Active' END 'Status', \n" +
                            "ZU.Name 'CreatedBy', \n" +
                            "CONVERT(VARCHAR(11), FT.CreatedOn, 106) 'CreatedOn', \n" +
                            "ZU1.Name 'ModifiedBy', \n" +
                            "CONVERT(VARCHAR(11), FT.ModifiedOn, 106) 'ModifiedOn' \n" +
                            "FROM FAF_Tax FT \n" +
                            "INNER JOIN ZNI_USER1 ZU \n" +
                            "ON ZU.U_ID = FT.CreatedBy \n" +
                            "LEFT JOIN ZNI_USER1 ZU1 \n" +
                            "ON ZU1.U_ID = FT.ModifiedBy \n" +
                            "WHERE  \n";

                if (RBL_Status.SelectedItem.Text == "Active")
                {
                    query += "FT.Status = '" + RBL_Status.SelectedItem.Value + "' \n";
                }
                else if (RBL_Status.SelectedItem.Text == "In Active")
                {
                    query += "FT.Status = '" + RBL_Status.SelectedItem.Value + "' \n";
                }

                query += "AND\nFT.CreatedBy = '" + Session_User_ID + "' \n"
                         + "ORDER BY FT.CreatedOn DESC";
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                sda.Fill(dt);
            }
            catch (Exception ex)
            {
                lbl_Message.Text = "Unable To Fetch Data!";
                lbl_Message.ForeColor = System.Drawing.Color.Red;
            }
            finally { con.Close(); }
            return dt;
        }

        public DataTable Get_Last_FAF_Tax_ID_According_To_Service_Type()
        {

            string query = "";

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                query = query = "SELECT \n" +
                                "MAX(FT.FAF_Tax_ID) 'FAF_Tax_ID' \n" +
                                "FROM FAF_Tax FT \n" +
                                "WHERE \n" +
                                "FT.Status = '1' \n" +
                                "AND\nFT.ServiceTypeID = '" + dd_ServiceType.SelectedValue.ToString() + "'";

                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                sda.Fill(dt);
            }
            catch (Exception ex)
            {
                lbl_Message.Text = "Unable To Fetch Data!";
                lbl_Message.ForeColor = System.Drawing.Color.Red;
            }
            finally { con.Close(); }
            return dt;
        }

        //  Return the Column Data as Array of string
        public string[] Get_FAF_Tax_Column_Data(string Column_Name)
        {
            DataTable dt = null;

            dt = Get_FAF_Tax();

            string[] Column_Data = new string[dt.Rows.Count];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string col_data = string.Empty;

                col_data = dt.Rows[i][Column_Name].ToString();

                if (col_data != null)
                {
                    if (col_data.Contains("/") || col_data.Contains(":"))
                    {
                        DateTime _date = Convert.ToDateTime(col_data);

                        string date_format = "yyyy-MM-dd";

                        string Formatted_Date = _date.ToString(date_format);

                        Column_Data[i] = Formatted_Date;

                    }
                    else
                    {
                        Column_Data[i] = col_data;
                    }
                }
            }

            return Column_Data;
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

        protected void CSV_btn_onclick(object sender, EventArgs e)
        {
            #region For New Window Print Format

            string Session_User_ID = Session["U_ID"].ToString();
            string Query_Status = Encrypt_QueryString(hf_Status.Value.ToString());
            string Query_User_ID = Encrypt_QueryString(Session_User_ID);

            ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "OpenWindow", "window.open('FAF_Tax_Print.aspx?status=" + Query_Status + "&user_id=" + Query_User_ID + "','FAF Tax Print','menubar=1,resizable=1,width=900,height=600');", true);


            #endregion
        }

        protected void rp_FAF_Tax_DBT_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                (e.Item.FindControl("lblRowNumber") as Label).Text = (e.Item.ItemIndex + 1).ToString();
            }
        }

        protected void RBL_Status_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = Get_FAF_Tax();
            if (dt.Rows.Count > 0)
            {
                RBL_Status.Visible = true;
                FAF_Tax_DBT_Details.Visible = true;
                CSV_btn.Visible = true;
                rp_FAF_Tax_DBT.DataSource = dt;
                rp_FAF_Tax_DBT.DataBind();
                CC_Panel.Height = Unit.Pixel(250);
            }
            else
            {
                RBL_Status.Visible = true;
                FAF_Tax_DBT_Details.Visible = true;
                CSV_btn.Visible = false;
                rp_FAF_Tax_DBT.DataSource = null;
                rp_FAF_Tax_DBT.DataBind();
                CC_Panel.Height = Unit.Pixel(0);
            }
            hf_Status.Value = RBL_Status.SelectedItem.Value;
        }
    }
}