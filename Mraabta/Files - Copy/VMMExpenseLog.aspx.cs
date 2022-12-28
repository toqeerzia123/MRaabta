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
    public partial class VMMExpenseLog : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        bool check = true;
        protected void Page_Load(object sender, EventArgs e)
        {


            if (HttpContext.Current.Session["U_NAME"] == null)
            {

                Response.Redirect("~/login");

            }


            if (!IsPostBack)
            {

                expensedate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                billdate.Text = DateTime.Now.ToString("dd/MM/yyyy");

                if (check)
                {
                    GetVehicle_type();
                    // GetRiders();
                    // GetVehicle();
                    getstaff();
                    Get_MainCategory();
                    GetProducts();
                    getzone();
                    Showedit();
                    btn_Submit.Visible = true;
                    btn_update.Visible = false;//note./
                }
            }

            if (HttpContext.Current.Session["EDIT"] == "T")
            {

                string exp_no = Session["Expno"].ToString();
                GetVehicle_type();
                GetRiders();
                getstaff();
                // GetVehicle();
                getzone();
                // dd_zone_SelectedIndexChanged(sender, e);
                GetProducts();
                Get_MainCategory();
                Edit();

                SubCategory_SelectedIndexChanged(sender, e);
                btn_Submit.Visible = false;
                btn_update.Visible = true;
                dv_edit.Visible = false;
                Session["EDIT"] = null;
            }

        }

        protected void GetProducts()
        {

            string query = "select distinct(products) from servicetypes_new";

            DataSet ds = new DataSet();
            try
            {

                con.Open();
                SqlCommand orcd = new SqlCommand(query, con);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {

                    dd_ItemLevel1.DataTextField = "products";
                    dd_ItemLevel1.DataValueField = "products";
                    dd_ItemLevel1.DataSource = ds.Tables[0].DefaultView;
                    dd_ItemLevel1.DataBind();
                }
                dd_ItemLevel1.Items.Insert(0, new ListItem("SELECT PRODUCT", "0"));
            }
            catch (Exception Err)
            { }
            finally
            { con.Close(); }


        }
        protected void getzone()
        {
            string sql = "select zonecode,name from zones where region is not null order by name asc";
            // SqlConnection con = new SqlConnection(clvar.Strcon());
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            con.Open();
            adp.Fill(ds);
            con.Close();
            if (ds.Tables[0].Rows.Count > 0)
            {
                dd_zone.DataSource = ds.Tables[0];
                dd_zone.DataTextField = "name";
                dd_zone.DataValueField = "zonecode";
                dd_zone.DataBind();


                dd_zone.Items.Insert(0, new ListItem("Select Zone", ""));
            }
        }
        protected void GetRiders()//First One
        {
            Errorid.Text = "";
            DataSet ds = new DataSet();
            ds = Get_RidersListByID(dd_zone.SelectedValue);

            if (ds.Tables[0].Rows.Count != 0)
            {
                dd_DSR.DataTextField = "NAME";
                dd_DSR.DataValueField = "riderCode";
                dd_DSR.DataSource = ds.Tables[0].DefaultView;
                dd_DSR.DataBind();
            }
            else
            {
                ds = null;
                Errorid.Text = "No Rider Present.";
            }
            dd_DSR.Items.Insert(0, new ListItem("SELECT DRIVER", "0"));
        }

        protected void GetVehicle(string zone, string vt)
        {
            Errorid.Text = "";
            DataSet ds = new DataSet();
            ds = Get_VehicleListByID(zone, vt);

            if (ds.Tables[0].Rows.Count != 0)
            {
                dd_van.DataTextField = "NAME";
                dd_van.DataValueField = "id";
                dd_van.DataSource = ds.Tables[0].DefaultView;
                dd_van.DataBind();
            }
            else
            {
                ds = null;
                dd_van.DataSource = ds;
                dd_van.DataBind();
                Errorid.Text = "No Vehicle Present.";
                dd_van.Items.Clear();
            }

            dd_van.Items.Insert(0, new ListItem("SELECT VEHICLE", "0"));
        }
        public DataSet Get_VehicleListByID(string zone, string vt)
        {

            //string query = "SELECT v.id, v.NAME FROM Vehicle v";

            string query = "select * from vehicle where zonecode='" + zone + "' and vehiclemaintain='" + vt + "' ORDER BY NAME ASC";


            DataSet ds = new DataSet();
            try
            {
                con.Open();
                SqlCommand orcd = new SqlCommand(query, con);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                con.Close();
            }
            catch (Exception Err)
            { }
            finally
            { con.Close(); }
            return ds;
        }
        protected void getstaff()
        {

            string sql = "select staffcode,staffname from MNP_VEHICLE_STAFF";
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            con.Open();
            adp.Fill(ds);
            con.Close();
            if (ds.Tables[0].Rows.Count > 0)
            {
                dd_staff.DataSource = ds.Tables[0];
                dd_staff.DataTextField = "staffname";
                dd_staff.DataValueField = "staffcode";
                dd_staff.DataBind();


                dd_staff.Items.Insert(0, new ListItem("Select Staff", ""));
            }
        }
        public DataSet Get_RidersListByID(string zone)
        {
            string query = "";

            query = "select r.riderCode,(r.riderCode+'-'+r.firstName) AS NAME\n" +
       "FROM Riders r WHERE r.branchId='" + HttpContext.Current.Session["BRANCHCODE"].ToString() + "' and status = '1' and zoneid='" + zone + "'  ORDER BY r.riderCode";

            DataSet ds = new DataSet();
            try
            {
                con.Open();
                SqlCommand orcd = new SqlCommand(query, con);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                con.Close();
            }
            catch (Exception Err)
            { }
            finally
            { con.Close(); }
            return ds;
        }

        public void Edit()
        {

            string query = "select vmm.edate,vmm.vehicle,vmm.vehicle_type,vmm.mmc_id,vmm.msc_id,vmm.detail,vmm.bill_no,\n" +
                           "vmm.bill_date,vmm.bill_amount,vmm.m_reading,vmm.vendor_info,vmm.remarks,vmm.qty,vmm.driver,vmm.products,vmm.workorder_amount,vmm.zone,vmm.Staff_workshop,vmm.vehiclemaintain\n" +
                           "from VMM_EXPENSE_SHEET vmm where vmm.eid = '" + HttpContext.Current.Session["Expno"].ToString() + "'";

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(query, con);
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                dd_ItemLevel1.SelectedValue = dt.Rows[0]["products"].ToString();
                meterreading.Text = dt.Rows[0]["m_reading"].ToString();
                remarks.Text = dt.Rows[0]["remarks"].ToString();
                dd_DSR.SelectedValue = dt.Rows[0]["driver"].ToString().TrimStart();
                V_type.SelectedValue = dt.Rows[0]["Vehiclemaintain"].ToString();
                dd_vtype.SelectedValue = dt.Rows[0]["vehicle_type"].ToString();
                expensedate.Text = DateTime.Parse(dt.Rows[0]["edate"].ToString()).ToString("dd/MM/yyyy");
                txt_vendor.Text = dt.Rows[0]["vendor_info"].ToString();
                billamount.Text = dt.Rows[0]["bill_amount"].ToString();
                billno.Text = dt.Rows[0]["bill_no"].ToString();
                billdate.Text = DateTime.Parse(dt.Rows[0]["bill_date"].ToString()).ToString("dd/MM/yyyy");
                detail.Text = dt.Rows[0]["detail"].ToString();
                CategoryDropDownList.SelectedValue = dt.Rows[0]["mmc_id"].ToString();
                SubCategory.SelectedValue = hd_subcat.Value = dt.Rows[0]["msc_id"].ToString();
                qtytxtbox.Text = dt.Rows[0]["qty"].ToString();
                dd_zone.SelectedValue = dt.Rows[0]["zone"].ToString();
                GetVehicle(dd_zone.SelectedValue, V_type.SelectedValue);
                GetRiders();
                dd_van.SelectedValue = dt.Rows[0]["vehicle"].ToString();
                dd_staff.SelectedValue = dt.Rows[0]["Staff_workshop"].ToString();
                txt_workorder_amt.Text = dt.Rows[0]["workorder_amount"].ToString();
                check = false;
            }
        }

        protected void btn_Submit_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> queries = new List<string>();
                string vehicletype = "01";
                string vehiclename = "";
                string eDate = expensedate.Text.Trim();
                string products = "";
                if (dd_ItemLevel1.SelectedValue != "0")
                {
                    products = dd_ItemLevel1.SelectedItem.ToString();
                }

                string es_dsr = dd_DSR.SelectedValue.Trim();
                string mc = CategoryDropDownList.SelectedValue.Trim();
                string msc = SubCategory.SelectedValue.Trim();
                int qty = 0;
                if (qtytxtbox.Enabled)
                {
                    if (!string.IsNullOrEmpty(qtytxtbox.Text))
                    {
                        qty = int.Parse(qtytxtbox.Text);
                    }
                    else { qty = 0; }
                }
                string bilno = billno.Text.Trim();
                string bildt = billdate.Text.Trim();
                int bilamt = int.Parse(billamount.Text);
                int mtrRd = int.Parse(meterreading.Text);
                string vnd = txt_vendor.Text;
                string dt = detail.Text.Trim();
                string rem = remarks.Text.Trim();

                if (dd_van.Enabled && dd_van.Visible)
                {
                    vehiclename = dd_van.SelectedValue.Trim();
                }
                else
                {
                    vehiclename = OutsVan.Text.Trim();
                }


                string expenseId = "";
                string no = Get_Expense_Id();
                if (string.IsNullOrEmpty(no))
                {
                    no = "1";
                }
                expenseId = no.ToString();
                bool status = false;

                string query = "INSERT INTO VMM_EXPENSE_SHEET\n" +
                "   (EID, EDATE,\n" +
                "   VEHICLE_TYPE, VEHICLE, MMC_ID,\n" +
                "    MSC_ID, DETAIL, BILL_NO, BILL_DATE,\n" +
                "    BILL_AMOUNT, M_READING,\n" +
                "     VENDOR_INFO, REMARKS,\n" +
                "     Qty, DRIVER,\n" +
                "      PRODUCTS,workorder_amount,zone,staff_workshop,vehiclemaintain)\n" +
                " VALUES (\n" +
                "'" + expenseId + "',\n" +
                "'" + DateFormat(eDate) + "',\n" +
                " '" + dd_vtype.SelectedValue + "',\n" +
                " '" + vehiclename + "',\n" +
                "  '" + mc + "',\n" +
                "  '" + msc + "',\n" +
                "  '" + dt + "',\n" +
                "  '" + bilno + "',\n" +
                "'" + DateFormat(bildt) + "',\n" +
                "  " + bilamt + ",\n" +
                "  " + mtrRd + ",\n" +
                "  '" + vnd + "',\n" +
                "  '" + rem + "',\n" +
                "   " + qty + ",\n" +
                "   '" + es_dsr + "',\n" +
                "   '" + products + "',\n" +
                "   '" + txt_workorder_amt.Text + "','" + dd_zone.SelectedValue + "','" + dd_staff.SelectedValue + "','" + V_type.SelectedValue + "')";

                queries.Add(query);

                int j = PostCollectionNew(queries);
                clearall();
            }
            catch (Exception Err)
            {
                expensedate.ToString();
            }
            finally
            {
                con.Close();
            }


        }
        public string Get_Expense_Id()
        {
            string query = "select distinct max(eid + 1) as max_num from vmm_expense_sheet";
            //string query = "select max(eid) from vmm_expense_sheet";
            string TotalSize = "";
            DataSet ds = new DataSet();
            try
            {
                con.Open();
                SqlCommand orcd = new SqlCommand(query, con);
                TotalSize = Convert.ToString(orcd.ExecuteScalar());
                con.Close();

            }
            catch (Exception Err)
            { }
            finally
            { con.Close(); }
            return TotalSize;
        }
        private string DateFormat(string methDate)
        {
            string mdate = DateTime.ParseExact(methDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture);
            string[] mmarr = mdate.Split('/', ' ');
            return string.Concat(mmarr);
        }

        protected void GetVehicle_type()
        {

            string query = "SELECT typeid,typedesc FROM Vehicle_Type";

            DataSet ds = new DataSet();
            try
            {

                con.Open();
                SqlCommand orcd = new SqlCommand(query, con);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {

                    dd_vtype.DataTextField = "typedesc";
                    dd_vtype.DataValueField = "typeid";
                    dd_vtype.DataSource = ds.Tables[0].DefaultView;
                    dd_vtype.DataBind();
                }
                dd_vtype.Items.Insert(0, new ListItem("SELECT VEHICLE TYPE", "0"));
            }
            catch (Exception Err)
            { }
            finally
            { con.Close(); }


        }

        public void clearall()
        {
            txt_vendor.Text = txt_workorder_amt.Text = expensedate.Text = detail.Text = meterreading.Text = remarks.Text = billamount.Text = txt_workorder_amt.Text = "";
        }
        public void Showedit()
        {

            string sqlString = "SELECT V.EID,convert(varchar,convert(datetime,V.EDATE,105),103) TODAY_DATE, MMC.MMC_LDESC MMC, MSC.MSC_LDESC MSC\n" +
            "                                  FROM VMM_EXPENSE_SHEET V, VMM_MAINTENANCEMCATEGORY MMC, VMM_MAINTENANCESCATEGORY MSC\n" +
            "                                  WHERE V.EDATE = '" + DateFormat(DateTime.Now.ToString("dd/MM/yyyy")) + "'\n" +
            "                                  AND V.MMC_ID = MMC.MMC_ID\n" +
            "                                  AND V.MMC_ID = MSC.MMC_ID\n" +
            "                                  AND V.MSC_ID = MSC.MSC_ID order by V.EID asc";


            DataSet dvt = new DataSet();
            try
            {
                con.Open();
                SqlCommand orcd = new SqlCommand(sqlString, con);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dvt);
                con.Close();
                if (dvt.Tables[0].Rows.Count > 0)
                {
                    dv_edit.Visible = true;
                    gv_edit.DataSource = dvt;
                    gv_edit.DataBind();

                }
                else
                {
                    dvt.Tables[0].Rows.Add(dvt.Tables[0].NewRow());
                    dv_edit.Visible = true;
                    gv_edit.DataSource = dvt;
                    gv_edit.DataBind();
                    int columncount = gv_edit.Rows[0].Cells.Count;
                    gv_edit.Rows[0].Cells.Clear();
                    gv_edit.Rows[0].Cells.Add(new TableCell());
                    gv_edit.Rows[0].Cells[0].ColumnSpan = columncount;
                    gv_edit.Rows[0].Cells[0].Text = "No Records Found";


                }

                con.Close();
            }
            catch (Exception Err)
            {
                // clvar.Error_Check = Err.Message.ToString();
            }
            finally
            { con.Close(); }
        }
        protected void btn_update_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> queries = new List<string>();
                DateTime exp_d = DateTime.ParseExact(expensedate.Text, "dd/MM/yyyy", null);
                string expdate = exp_d.ToString("yyyy-MM-dd");
                DateTime bill_d = DateTime.ParseExact(billdate.Text, "dd/MM/yyyy", null);
                string Billdate = bill_d.ToString("yyyy-MM-dd");

                string van = "";

                if (V_type.SelectedValue.ToString() == "03")
                {

                    van = OutsVan.Text.Replace("'", "");
                }
                else
                {

                    van = dd_van.SelectedValue;

                }
                string product = "";
                if (dd_ItemLevel1.SelectedValue != "0")
                {
                    product = dd_ItemLevel1.SelectedItem.ToString();
                }

                string update = "UPDATE VMM_EXPENSE_SHEET SET\n" +
                                "edate = '" + expdate + "',\n" +
                                "vehicle = '" + van + "',\n" +
                                "vehicle_type = '" + dd_vtype.SelectedValue + "',\n" +
                                 "vehiclemaintain = '" + V_type.SelectedValue + "',\n" +
                                "mmc_id = '" + CategoryDropDownList.SelectedValue + "',\n" +
                                "msc_id = '" + SubCategory.SelectedValue + "',\n" +
                                "detail = '" + detail.Text.Replace("'", "") + "',\n" +
                                "bill_no = '" + billno.Text.Replace("'", "") + "',\n" +
                                "bill_date = '" + Billdate + "',\n" +
                                "bill_amount = '" + billamount.Text.Replace("'", "") + "',\n" +
                                "m_reading = '" + meterreading.Text.Replace("'", "") + "',\n" +
                                "vendor_info = '" + txt_vendor.Text.Replace("'", "") + "',\n" +
                                "remarks = '" + remarks.Text.Replace("'", "") + "',\n" +
                                "qty = '" + qtytxtbox.Text.Replace("'", "") + "',\n" +
                                "driver = '" + dd_DSR.SelectedValue + "',\n" +
                                "products = '" + product + "',\n" +
                                "workorder_amount = '" + txt_workorder_amt.Text + "',\n" +
                                "zone = '" + dd_zone.SelectedValue + "',\n" +
                                "staff_workshop = '" + dd_staff.SelectedValue + "'\n" +
                                "where eid = '" + HttpContext.Current.Session["Expno"].ToString() + "'";


                queries.Add(update);


                int j = PostCollectionNew(queries);
                if (j == 1)
                {
                    check = true;
                    Session["EDIT"] = null;
                    Session["Expno"] = null;
                    clearall();
                    Response.Redirect("VMMDpoExpenseLog.aspx");

                }
            }
            catch (Exception ex)
            {

            }
        }


        protected int PostCollectionNew(List<string> queries)
        {
            Errorid.Text = "";
            int status = 0;
            string query = "";
            int check_ = 0;

            con.Open();
            SqlCommand command = con.CreateCommand();
            SqlTransaction transaction;
            transaction = con.BeginTransaction();
            command.Transaction = transaction;
            Boolean flag = false;
            try
            {
                int statusFinal = 0;
                foreach (string list in queries)
                {
                    command.CommandText = list;
                    statusFinal = command.ExecuteNonQuery();
                }

                if (statusFinal > 0)
                {

                    flag = true;

                }
                else
                {

                    Errorid.Text = "Error In Vehicle log creation";
                }

                if (flag == true)
                {

                    transaction.Commit();
                    status = 1;
                    btn_Submit.Enabled = false;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Messagebox", "alert('Expense log has been Updated!'); window.location('VMMExpenseLog.aspx');", true);

                }
                else
                {
                    transaction.Rollback();
                    Errorid.Text = "Error In Vehicle log creation";
                    status = 0;
                }
            }
            catch (Exception ex)
            {
                Errorid.Text = "" + ex.ToString();
            }
            finally
            {
                con.Close();
            }

            return status;
            //  }

        }

        protected void V_type_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetVehicle(dd_zone.SelectedValue, V_type.SelectedValue);
        }
        public DataSet Get_MainCategoryList()
        {
            string query = "SELECT MMC_ID, MMC_SDESC FROM VMM_MAINTENANCEMCATEGORY ORDER BY MMC_ID";

            DataSet ds = new DataSet();
            try
            {

                con.Open();
                SqlCommand orcd = new SqlCommand(query, con);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                con.Close();
            }
            catch (Exception Err)
            { }
            finally
            { con.Close(); }
            return ds;
        }
        public DataSet Get_SubCategoryList(string subcat)
        {
            string query = "select msc_id, msc_sdesc from VMM_MAINTENANCESCATEGORY where mmc_id = '" + subcat + "' order by mmc_id";

            DataSet ds = new DataSet();
            try
            {
                con.Open();
                SqlCommand orcd = new SqlCommand(query, con);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                con.Close();
            }
            catch (Exception Err)
            { }
            finally
            { con.Close(); }
            return ds;
        }
        protected void Get_MainCategory()
        {
            Errorid.Text = "";
            DataSet ds = new DataSet();
            ds = Get_MainCategoryList();

            if (ds.Tables[0].Rows.Count != 0)
            {
                CategoryDropDownList.DataTextField = "mmc_sdesc";
                CategoryDropDownList.DataValueField = "mmc_id";
                CategoryDropDownList.DataSource = ds.Tables[0].DefaultView;
                CategoryDropDownList.DataBind();
            }
            else
            {
                Errorid.Text = "No Main Category present.";
            }
            CategoryDropDownList.Items.Insert(0, new ListItem("SELECT MAIN CATEGORY", "0"));


        }

        protected void SubCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            //clearing other items
            SubCategory.Items.Clear();
            SubCategory.Items.Insert(0, new ListItem("SELECT SUB CATEGORY", "0"));

            if (CategoryDropDownList.SelectedValue.ToString() != "0")
            {
                //SELECTING SUBCATEGORY
                DataSet ds = new DataSet();
                ds = Get_SubCategoryList(CategoryDropDownList.SelectedValue);
                if (ds.Tables[0].Rows.Count != 0)
                {
                    SubCategory.Items.Clear();
                    SubCategory.DataTextField = "msc_sdesc";
                    SubCategory.DataValueField = "msc_id";
                    SubCategory.DataSource = ds.Tables[0].DefaultView;
                    SubCategory.DataBind();
                    SubCategory.Items.Insert(0, new ListItem("SELECT SUB CATEGORY", "0"));
                }


            }

            //Errorid.Text = "";
            //SubCategory.ClearSelection();
            //SubCategory.Items.Clear();
            //SubCategory.Items.Add("SELECT SUB CATEGORY");
            //if (!CategoryDropDownList.SelectedValue.Contains("SELECT MAIN CATEGORY"))
            //{
            //    DataSet ds = new DataSet();
            //    ds = Get_SubCategoryList(CategoryDropDownList.SelectedValue);

            //    if (ds.Tables[0].Rows.Count != 0)
            //    {
            //        SubCategory.DataTextField = "msc_sdesc";
            //        SubCategory.DataValueField = "msc_id";
            //        SubCategory.DataSource = ds.Tables[0].DefaultView;
            //        SubCategory.DataBind();
            //    }

            //    else
            //    {
            //        Errorid.Text = "No Sub Category present.";

            //    }

            //    SubCategory.Items.Insert(0, new ListItem("SELECT SUB CATEGORY", "0"));

            //    SubCategory_SelectedIndexChanged1(sender, e);
            //}
            //else
            //{
            //    SubCategory.Items.Clear();
            //    SubCategory.ClearSelection();
            //    SubCategory.Items.Add("SELECT SUB CATEGORY");

            //}
        }

        protected void SubCategory_SelectedIndexChanged1(object sender, EventArgs e)
        {
            if (SubCategory.SelectedItem.Text == "PATROL" || SubCategory.SelectedItem.Text == "CNG" || SubCategory.SelectedItem.Text == "DIESEL")
            {
                qtytxtbox.Enabled = true;
            }
            else
            {
                qtytxtbox.Enabled = false;
            }
        }
        protected void gv_data_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EDIT")
            {
                string exp_no = e.CommandArgument.ToString();
                if (!string.IsNullOrEmpty(exp_no))
                {

                    Session["Expno"] = exp_no;
                    Session["EDIT"] = "T";

                    Response.Redirect("VMMExpenseLog.aspx");

                }
            }
        }
        protected void dd_zone_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetVehicle(dd_zone.SelectedValue, V_type.SelectedValue);
        }
    }
}