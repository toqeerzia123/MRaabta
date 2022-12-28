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

namespace MRaabta.Files
{
    public partial class PettyCash_GLReport : System.Web.UI.Page
    {
        Variable clvar = new Variable();
        bayer_Function bfunc = new bayer_Function();
        CommonFunction cfunc = new CommonFunction();
        clsVariables clsvar = new clsVariables();
        Cl_Variables cl_var = new Cl_Variables();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //  Get_Branches();
                Get_Zones();
                Get_Companies();
            }
        }
        protected void btn_add_Click(object sender, EventArgs e)
        {
            lbl_error.Text = "";
            DataSet ds = new DataSet();
            try
            {
                ds = Mainhead();

                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = new DataTable();
                    dt = ds.Tables[0];
                    ViewState["dt"] = dt;
                    GridView.DataSource = dt;
                    GridView.DataBind();
                }
                else
                {
                    lbl_error.Text = "No Data Found!!";
                    GridView.DataSource = null;
                    GridView.DataBind();
                }
            }
            catch
            {
                lbl_error.Text = "The report cannot be generated due to error!!";
                GridView.DataSource = null;
                GridView.DataBind();
                return;
            }
            finally
            {
            }
        }
        public DataSet Mainhead()
        {
            DataSet Ds_1 = new DataSet();
            string branch = "";
            string ec = "";

            if (dd_ec.SelectedValue.ToString() != "")
            {
            }
            string ec_val = "";
            int dd_ecSelectedItemCount = dd_ec.Items.Cast<System.Web.UI.WebControls.ListItem>().Count(li => li.Selected);
            if (dd_ecSelectedItemCount > 0)
            {
                if (dd_ec.Items[0].Selected)
                {
                }
                else
                {
                    for (int i = 0; i < dd_ec.Items.Count; i++)
                    {
                        if (dd_ec.Items[i].Selected)
                        {

                            ec_val += "'" + dd_ec.Items[i].Value + "'" + ",";
                        }
                    }
                }
            }
            if (!String.IsNullOrEmpty(ec_val))
            {

                ec_val = ec_val.Substring(0, ec_val.Length - 1);
                ec = " and h.express_center in (" + ec_val + ") ";
            }
            string branch_val = "";
            int dd_branchSelectedItemCount = dd_branch.Items.Cast<System.Web.UI.WebControls.ListItem>().Count(li => li.Selected);
            if (dd_branchSelectedItemCount > 0)
            {
                for (int i = 0; i < dd_branch.Items.Count; i++)
                {
                    if (dd_branch.Items[i].Selected)
                    {

                        branch_val += "'" + dd_branch.Items[i].Value + "'" + ",";
                    }
                }
            }
            if (!String.IsNullOrEmpty(branch_val))
            {

                branch_val = branch_val.Substring(0, branch_val.Length - 1);
                branch = " and h.BRANCH in (" + branch_val + ") ";

            }
            string status = "";
            if (dd_status.SelectedValue.ToString() != "")
            {
                status = " and pd.status='" + dd_status.SelectedValue.ToString() + "' ";
            }


            string sqlString = "select ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS SerialNo,\n" +
            //"m.branchCode,m.branch,\n" +
            "count(m.ID) entries\n" +
            "from\n" +
            "(\n" +
            "(select h.MONTH,pd.ID,\n" +
            "b.branchCode,\n" +
            "e.expressCenterCode,\n" +
            "       b.name branch,\n" +
            "       e.name ec,\n" +
            "       CONVERT(varchar, h.Created_ON, 106) created_on\n" +
            "  from pc_head h\n" +
            "  inner join Branches b\n" +
            "    on b.branchCode = h.BRANCH\n" +
            " left outer join ExpressCenters e\n" +
            "    on e.expressCenterCode = h.express_center\n" +
            "\t inner join PC_DETAIL pd on pd.HEAD_ID=h.ID\n" +
            " where \n" +
            " pd.[DATE] BETWEEN CONVERT(DATETIME, '" + txt_frmdate.Text + "', 103) AND CONVERT(DATETIME, '" + txt_todate.Text + "', 103) ";
            sqlString +=
            " and h.company='" + dd_company.SelectedValue.ToString() + "'  \n" +
            " " + branch + " " + ec + " " + status + "  ";
            sqlString +=
            "   \n" +
            "-- order by h.ID desc, h.Created_ON\n" +
            " )\n" +
            " union all\n" +
            " (\n" +
            " select h.MONTH,pd.ID,\n" +
            " b.branchCode,\n" +
            "e.expressCenterCode,\n" +
            "       b.name branch,\n" +
            "       e.name ec,\n" +
            "       CONVERT(varchar, h.Created_ON, 106) created_on\n" +
            "  from PC_CIH_head h\n" +
            "  inner join Branches b\n" +
            "    on b.branchCode = h.BRANCH\n" +
            " left outer  join ExpressCenters e\n" +
            "    on e.expressCenterCode = h.express_center\n" +
            "\tinner join PC_CIH_detail pd\n" +
            "\ton pd.head_id=h.ID\n" +
            " where \n" +
            " pd.[DATE] BETWEEN CONVERT(DATETIME, '" + txt_frmdate.Text + "', 103) AND CONVERT(DATETIME, '" + txt_todate.Text + "', 103) ";
            sqlString +=
            " \n" +
              " and h.company='" + dd_company.SelectedValue.ToString() + "'  \n" +
            " " + branch + " " + ec + "   ";
            sqlString +=
            "  \n" +
            "-- order by h.ID desc, h.Created_ON\n" +
            " )\n" +
            " ) m\n";
            //" group by  m.branchCode,m.branch";


            SqlConnection orcl = new SqlConnection(clvar.Strcon());
            orcl.Open();
            SqlCommand orcd = new SqlCommand(sqlString, orcl);
            orcd.CommandType = CommandType.Text;
            SqlDataAdapter oda = new SqlDataAdapter(orcd);
            oda.Fill(Ds_1);
            orcl.Close();

            return Ds_1;
        }
        public void Get_Companies()
        {
            DataSet ds = new DataSet();
            ds = ds_companies();
            if (ds.Tables[0].Rows.Count != 0)
            {
                dd_company.DataTextField = "sdesc_of";
                dd_company.DataValueField = "code_of";
                dd_company.DataSource = ds.Tables[0].DefaultView;
                dd_company.DataBind();
            }
        }
        public DataSet ds_companies()
        {
            DataSet Ds_1 = new DataSet();
            try
            {

                string query = "select * from COMPANY_OF order by code_of";
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
        #region ZONE AND BRANCHES
        public void Get_Zones()
        {

            DataSet ds = new DataSet();
            ds = ds_zones();
            if (ds.Tables[0].Rows.Count != 0)
            {
                dd_zone.DataTextField = "name";
                dd_zone.DataValueField = "code";
                dd_zone.DataSource = ds.Tables[0].DefaultView;
                dd_zone.DataBind();
            }
            Get_Branches();
            dd_ec.Items.Clear();
            dd_ec.Items.Insert(0, new ListItem("ALL", ""));
            //Get_ExpressCenter();
        }
        public DataSet ds_zones()
        {
            DataSet Ds_1 = new DataSet();
            try
            {
                string zone = "";
                if (!string.IsNullOrEmpty(HttpContext.Current.Session["ZONECODE"].ToString()))
                {
                    zone = "'" + HttpContext.Current.Session["ZONECODE"].ToString().Replace(",", "','") + "'";
                }
                string sqlString = "SELECT Z.zoneCode CODE,Z.name FROM ZONES Z\n" +
                "WHERE Z.zoneCode in  (" + zone + ")";
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
        protected void dd_zone_Changed(object sender, EventArgs e)
        {
            Get_Branches();
            dd_ec.Items.Clear();
            dd_ec.Items.Insert(0, new ListItem("ALL", ""));
            //Get_ExpressCenter();
        }
        public DataSet Get_Branches(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            string query;
            try
            {
                string extraCondition = "";
                if (HttpContext.Current.Session["BranchCode"].ToString().ToUpper() == "ALL")
                {
                    extraCondition = "";
                }
                else
                {
                    extraCondition = "AND BranchCode in (" + HttpContext.Current.Session["BranchCode"].ToString() + ")";
                }
                string sql = "SELECT NAME, \n"
               + "       branchCode \n"
               + "FROM   Branches \n"
               + "WHERE  STATUS = '1' \n"
               + "       AND zonecode = '" + dd_zone.SelectedValue.ToString() + "'  " + extraCondition + "\n"
               + "                             UNION ALL \n"
               + "SELECT NAME, \n"
               + "       branchCode \n"
               + "FROM   Branches \n"
               + "WHERE  branchCode = '331' \n"
               + "       AND zonecode = '" + dd_zone.SelectedValue.ToString() + "' \n"
               + "ORDER BY \n"
               + "       NAME";

                query = "select NAME, branchCode from Branches where status = '1' \n" +
                                " AND zonecode='" + dd_zone.SelectedValue.ToString() + "'  ORDER BY NAME";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sql, orcl);
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


        public void Get_Branches()
        {
            DataSet ds = new DataSet();
            Variable var = new Variable();
            ds = Get_Branches(var);
            dd_branch.Items.Clear();
            if (ds.Tables[0].Rows.Count != 0)
            {
                dd_branch.DataTextField = "Name";
                dd_branch.DataValueField = "branchCode";
                dd_branch.DataSource = ds.Tables[0].DefaultView;
                dd_branch.DataBind();
                dd_branch.Items[0].Selected = true;
            }
        }
        #endregion
        public void Get_ExpressCenter()
        {
            DataSet ds = new DataSet();


            string branch_val = "";
            int dd_branchSelectedItemCount = dd_branch.Items.Cast<System.Web.UI.WebControls.ListItem>().Count(li => li.Selected);
            if (dd_branchSelectedItemCount > 0)
            {
                for (int i = 0; i < dd_branch.Items.Count; i++)
                {
                    if (dd_branch.Items[i].Selected)
                    {

                        branch_val += "'" + dd_branch.Items[i].Value + "'" + ",";
                    }
                }
            }
            if (!String.IsNullOrEmpty(branch_val))
            {

                branch_val = branch_val.Substring(0, branch_val.Length - 1);

            }
            cl_var.origin = branch_val;
            ds = ExpressCenterOrigin(cl_var);
            if (ds.Tables[0].Rows.Count != 0)
            {
                dd_ec.DataTextField = "Name";
                dd_ec.DataValueField = "expressCenterCode";
                dd_ec.DataSource = ds.Tables[0].DefaultView;
                dd_ec.DataBind();
                dd_ec.Items.Insert(0, new ListItem("Branch", "0"));
                dd_ec.Items.Insert(0, new ListItem("ALL", ""));
                dd_ec.Items[0].Selected = true;

            }
        }
        public DataSet ExpressCenterOrigin(Cl_Variables clvar)
        {
            DataSet Ds_1 = new DataSet();

            try
            {

                string query = "SELECT * FROM ExpressCenters ec where bid IN  (" + clvar.origin + ") and status='1' order by NAME";


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
        protected void dd_branch_SelectedIndexChanged(object sender, EventArgs e)
        {
            Get_ExpressCenter();
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "view")
            {
                //Determine the RowIndex of the Row whose Button was clicked.
                int rowIndex = Convert.ToInt32(e.CommandArgument);

                //Reference the GridView Row.
                GridViewRow row = GridView.Rows[rowIndex];

                //Fetch value of ID.
                string id = row.Cells[0].Text;

                DataTable dt = (DataTable)ViewState["dt"];
                DataRow[] dr = dt.Select("SerialNo='" + id + "'");
                string branchcode = "";
                string ec = "";
                string month = "";
                //foreach (DataRow drow in dr)
                //{
                //    branchcode = drow["branchCode"].ToString();
                //    //  ec = drow["expressCenterCode"].ToString();
                //    //  month = drow["month"].ToString();
                //}
                //branchcode = HttpUtility.UrlEncode(Encrypt(branchcode.ToString().Trim()));
                //  ec = HttpUtility.UrlEncode(Encrypt(dd_ec.SelectedValue.ToString().Trim()));

                string ec_val = "";
                int dd_ecSelectedItemCount = dd_ec.Items.Cast<System.Web.UI.WebControls.ListItem>().Count(li => li.Selected);
                if (dd_ecSelectedItemCount > 0)
                {

                    if (dd_ec.Items[0].Selected)
                    {
                    }
                    else
                    {
                        for (int i = 0; i < dd_ec.Items.Count; i++)
                        {
                            if (dd_ec.Items[i].Selected)
                            {

                                ec_val += "'" + dd_ec.Items[i].Value + "'" + ",";
                            }
                        }
                    }
                }
                if (!String.IsNullOrEmpty(ec_val))
                {

                    ec_val = ec_val.Substring(0, ec_val.Length - 1);

                }
                string branch_val = "";
                string branch_name = "";
                int dd_branchSelectedItemCount = dd_branch.Items.Cast<System.Web.UI.WebControls.ListItem>().Count(li => li.Selected);
                if (dd_branchSelectedItemCount > 0)
                {
                    for (int i = 0; i < dd_branch.Items.Count; i++)
                    {
                        if (dd_branch.Items[i].Selected)
                        {
                            string update = UpdatePC_remaining(dd_branch.Items[i].Value);
                            branch_val += "'" + dd_branch.Items[i].Value + "'" + ",";
                            branch_name += "" + dd_branch.Items[i].Text + "" + ",";
                        }
                    }
                }
                if (!String.IsNullOrEmpty(branch_val))
                {

                    branch_val = branch_val.Substring(0, branch_val.Length - 1);
                    branch_name = branch_name.Substring(0, branch_name.Length - 1);
                }

                branchcode = HttpUtility.UrlEncode(Encrypt(branch_val.ToString().Trim()));
                branch_name = HttpUtility.UrlEncode(Encrypt(branch_name.ToString().Trim()));
                ec = HttpUtility.UrlEncode(Encrypt(ec_val.Trim()));


                string frm_date = HttpUtility.UrlEncode(Encrypt(txt_frmdate.Text.Trim()));
                string to_date = HttpUtility.UrlEncode(Encrypt(txt_todate.Text.Trim()));
                string status = HttpUtility.UrlEncode(Encrypt(dd_status.SelectedValue.ToString().Trim()));
                string company = HttpUtility.UrlEncode(Encrypt(dd_company.SelectedValue.ToString()));
                //Redirect to the print view page
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "window.open('PettyCash_GLprint.aspx?from=" + frm_date + "&to=" + to_date + "&branch=" + branchcode + "&ec=" + ec + "&status=" + status + "&company=" + company + "&bname=" + branch_name + "');", true);
            }
        }
        #region Encryption & decryption
        private string Encrypt(string clearText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = System.Text.Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }
        private string Decrypt(string cipherText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = System.Text.Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
        #endregion

        public string UpdatePC_remaining(string branch)
        {
            SqlConnection sqlcon = new SqlConnection(clvar.Strcon2());
            string result = "";
            DataSet ds = new DataSet();
            try
            {
                DateTime first_day = new DateTime(int.Parse(DateTime.Parse(txt_todate.Text).Year.ToString()), int.Parse(DateTime.Parse(txt_todate.Text).Month.ToString()), 1);
                DateTime last_day = first_day.AddMonths(1).AddSeconds(-1);
                sqlcon.Open();
                SqlCommand sqlcmd = new SqlCommand("UpdatePettyCashBalance", sqlcon);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                SqlParameter SParam = new SqlParameter("Result", SqlDbType.VarChar);
                SParam.Direction = ParameterDirection.Output;
                SParam.Size = 500;
                sqlcmd.Parameters.AddWithValue("@BranchCode", branch);
                sqlcmd.Parameters.AddWithValue("@StartDate", first_day.ToString("yyyy-MM-dd"));
                sqlcmd.Parameters.AddWithValue("@EndDate", last_day.ToString("yyyy-MM-dd"));
                sqlcmd.Parameters.AddWithValue("@Month", DateTime.Parse(txt_todate.Text).Month.ToString());
                sqlcmd.Parameters.AddWithValue("@Year", DateTime.Parse(txt_todate.Text).Year.ToString());
                sqlcmd.Parameters.Add(SParam);
                sqlcmd.ExecuteNonQuery();
                result = SParam.Value.ToString();
                sqlcon.Close();
            }
            catch (Exception Err)
            {
                result = "Error in CIH Update!!";
            }
            finally
            { }
            return result;
        }
    }
}