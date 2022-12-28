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
    public partial class Manage_RnRTariff_new : System.Web.UI.Page
    {
        CommonFunction fun = new CommonFunction();
        Consignemnts con = new Consignemnts();
        Cl_Variables clvar = new Cl_Variables();
        bayer_Function b_fun = new bayer_Function();
        Variable clvarrrrrrrrr = new Variable();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ServiceTypes();
                Branches();
                GetCategories();
                //Current
                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[]
                {
                new DataColumn("ID"),
                new DataColumn("ServiceTypeName"),
                new DataColumn("ClientID"),
                new DataColumn("FromZone"),
                new DataColumn("FromZone_"),
                new DataColumn("ToZone"),
                new DataColumn("ToZone_"),
                new DataColumn("Price"),
                new DataColumn("isUpdated")
                });
                dt.AcceptChanges();
                ViewState["dt"] = dt;

                //Previous
                DataTable dt1 = new DataTable();
                dt1.Columns.AddRange(new DataColumn[]
                {
                new DataColumn("ID"),
                new DataColumn("ServiceTypeName"),
                new DataColumn("ClientID"),
                new DataColumn("FromZone"),
                new DataColumn("FromZone_"),
                new DataColumn("ToZone"),
                new DataColumn("ToZone_"),
                new DataColumn("Price"),
                new DataColumn("isUpdated")
                });
                dt1.AcceptChanges();
                ViewState["dt_Previous"] = dt1;
            }
        }

        protected void Branches()
        {
            DataTable dt = b_fun.Get_ZonebyBranches4(clvarrrrrrrrr).Tables[0];//fun.Branch().Tables[0];
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    dd_branch.DataSource = dt;
                    dd_branch.DataTextField = "name";
                    dd_branch.DataValueField = "branchCode";
                    dd_branch.DataBind();
                    dd_branch.SelectedValue = Session["BranchCode"].ToString();
                }
            }
        }

        protected void ServiceTypes()
        {
            DataTable dt = ServiceTypeName().Tables[0];
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    dd_serviceType.DataSource = dt;
                    dd_serviceType.DataTextField = "ServiceTypeName";
                    dd_serviceType.DataValueField = "ServiceTypeName";
                    dd_serviceType.DataBind();
                    dd_serviceType.SelectedValue = "overnight";
                    //dd_serviceType.SelectedValue = Session["BranchCode"].ToString();
                }
            }
        }

        public DataSet ServiceTypeName()
        {
            DataSet Ds_1 = new DataSet();

            try
            {
                string query = "SELECT st.ServiceTypeName, st.ServiceTypeName \n"
                + "FROM   ServiceTypes     st \n"
                + "WHERE  st.IsIntl = '0' \n"
                + "       AND st.[status] = '1' \n"
                + "       And st.companyid= '2' \n"
                + "GROUP BY \n"
                + "       st.ServiceTypeName \n"
                + "ORDER BY \n"
                + "       st.ServiceTypeName";

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

        public DataTable GetRnRCategories()
        {
            string query = "select * From RnR_Categories where status='1' ";
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

        protected void GetCategories()
        {
            DataTable dt = GetRnRCategories();

            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {

                    dd_fromCat.DataSource = dt;
                    dd_fromCat.DataTextField = "Label";
                    dd_fromCat.DataValueField = "ID";
                    dd_fromCat.DataBind();

                    dd_toCat.DataSource = dt;
                    dd_toCat.DataTextField = "Label";
                    dd_toCat.DataValueField = "ID";
                    dd_toCat.DataBind();

                }
            }

        }

        protected void txt_accountNo_TextChanged(object sender, EventArgs e)
        {
            clvar.Branch = dd_branch.SelectedValue;
            clvar.AccountNo = txt_accountNo.Text;
            DataTable dt = fun.GetAccountDetailByAccountNumber(clvar);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    txt_clientName.Text = dt.Rows[0]["NAME"].ToString();
                    creditclientid.Value = dt.Rows[0]["ID"].ToString();
                }
            }
        }

        public DataTable GetRnRTariffForEdit(Cl_Variables clvar)
        {
            //string query = "SELECT T.ID,\n" +
            //"       T.CLIENT_ID,\n" +
            //"       T.FROMCATID,\n" +
            //"       C.LABEL FROMCAT,\n" +
            //"       T.TOCATID,\n" +
            //"       D.LABEL TOCAT,\n" +
            //"       T.ISDEFAULT,\n" +
            //"       T.VALUE\n" +
            //"  FROM RNR_TARRIF T\n" +
            //" INNER JOIN RNR_CATEGORIES C\n" +
            //"    ON T.FROMCATID = C.ID\n" +
            //" INNER JOIN RNR_CATEGORIES D\n" +
            //"    ON T.TOCATID = D.ID\n" +
            //" WHERE T.CLIENT_ID = '" + clvar.CustomerClientID + "' ORDER BY FROMCAT, TOCAT, VALUE";

            string sql = "SELECT T.ID, T.ServiceTypeName, \n"
               + "       T.CLIENT_ID, \n"
               + "       T.FromZone, \n"
               + "       C.LABEL             FROMCAT, \n"
               + "       T.ToZone, \n"
               + "       D.LABEL             TOCAT, \n"
               + "       T.ISDEFAULT, \n"
               + "       T.VALUE \n"
               + "FROM RnR_Tarrif_Zonewise T \n"
               + "       INNER JOIN RNR_CATEGORIES C \n"
               + "            ON  T.FromZone = C.ID \n"
               + "       INNER JOIN RNR_CATEGORIES D \n"
               + "            ON  T.ToZone = D.ID \n"
               + "WHERE  T.CLIENT_ID = '" + clvar.CustomerClientID + "' AND T.ChkDeleted='0' and ServiceTypename='" + dd_serviceType.SelectedValue + "' \n"
               + "ORDER BY \n"
               + "       FROMCAT, \n"
               + "       TOCAT";

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sql, con);
                sda.Fill(dt);

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return dt;

        }

        protected void btn_showTariff_Click(object sender, EventArgs e)
        {

            #region Validations
            if (dd_branch.SelectedValue == "0")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Select Branch')", true);
                return;
            }
            if (txt_accountNo.Text.Trim(' ') == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter Account Number')", true);

                return;
            }
            #endregion


            DataTable tariff = ViewState["dt_Previous"] as DataTable;

            tariff.Clear();
            clvar.CustomerClientID = creditclientid.Value;
            //clvar.ServiceTypeName = dd_serviceType.SelectedValue;
            //DataTable tariff = ViewState["dt"] as DataTable;
            tariff.Clear();
            DataTable dt = GetRnRTariffForEdit(clvar);

            gv_tariff.DataSource = null;
            gv_tariff.DataBind();

            foreach (DataRow dr in dt.Rows)
            {
                DataRow row = tariff.NewRow();
                row["ID"] = dr["ID"].ToString();
                row["ServiceTypeName"] = dr["ServiceTypeName"].ToString();
                row["ClientID"] = dr["Client_ID"].ToString();
                row["FromZone"] = dr["FromZone"].ToString();
                row["FromZone_"] = dr["FromCat"].ToString();
                row["Tozone"] = dr["Tozone"].ToString();
                row["Tozone_"] = dr["ToCat"].ToString();
                row["Price"] = dr["Value"].ToString();
                tariff.Rows.Add(row);
                tariff.AcceptChanges();
            }
            if (tariff.Rows.Count == 0)
            {
                btn_applyDefault.Visible = true;
            }
            else
            {
                btn_applyDefault.Visible = false;
            }
            gv_tariff.DataSource = tariff;
            gv_tariff.DataBind();
            gv_tariff.EmptyDataText = "No Data Available";
            ViewState["dt_Previous"] = tariff;
        }

        protected void btn_add_Click(object sender, EventArgs e)
        {
            //if (txt_accountNo.Text.Trim() == "0")
            //{
            //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cannot Update Cash Tariff')", true);
            //    err_msg.Text = "Cannot Update Cash Tariff";
            //    return;
            //}

            if (dd_serviceType.SelectedValue == "0")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Service Type Not Selected')", true);
                err_msg.Text = "";
                return;
            }
            DataTable tariff = ViewState["dt"] as DataTable;
            clvar.FromCat = dd_fromCat.SelectedValue;
            clvar.ToCat = dd_toCat.SelectedValue;
            clvar.amount = double.Parse(txt_price.Text);
            DataTable tariff_check = ViewState["dt_Previous"] as DataTable;
            DataRow[] dr_ = tariff_check.Select("FromZone = '" + clvar.FromCat + "' AND ToZone = '" + clvar.ToCat + "' AND CLIENTID = '" + creditclientid.Value + "'");


            if (dr_.Count() > 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cannot Add Tariff. Tariff Already Exists')", true);
                return;
            }

            DataRow[] drr = tariff.Select("FromZone = '" + clvar.FromCat + "' AND ToZone = '" + clvar.ToCat + "' AND CLIENTID = '" + creditclientid.Value + "'");
            if (drr.Count() > 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Tarif already present in the list')", true);
                return;
            }

            DataRow dr = tariff.NewRow();
            dr["ID"] = "";
            dr["ServiceTypeName"] = dd_serviceType.SelectedValue;
            dr["ClientID"] = creditclientid.Value;
            dr["FromZone"] = dd_fromCat.SelectedValue;
            dr["FromZone_"] = dd_fromCat.SelectedItem.Text;
            dr["ToZone"] = dd_toCat.SelectedValue;
            dr["ToZone_"] = dd_toCat.SelectedItem.Text;
            dr["Price"] = txt_price.Text;
            dr["isUpdated"] = "INSERT";

            tariff.Rows.Add(dr);
            tariff.AcceptChanges();
            DataView tempView = tariff.DefaultView;
            tempView.Sort = " FromZone_, ToZone_, PRICE";
            tariff = tempView.ToTable();
            gv_tariff_Actual.DataSource = null;
            gv_tariff_Actual.DataBind();
            gv_tariff_Actual.DataSource = tariff;
            gv_tariff_Actual.DataBind();
            gv_tariff_Actual.EmptyDataText = "No Data Available";
            ViewState["dt"] = tariff;
            if (tariff.Rows.Count == 0)
            {
                btn_applyDefault.Visible = true;
            }
            else
            {
                btn_applyDefault.Visible = false;
            }
        }

        protected void gv_tariff_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                if (e.Row.Cells[0].Text.Trim(' ') == "")
                {
                    (e.Row.FindControl("btn_delete") as Button).Visible = false;

                }

                if ((e.Row.FindControl("hd_isupdated") as HiddenField).Value == "DELETE")
                {
                    e.Row.Visible = false;
                }


            }
        }

        protected void gv_tariff_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //if (txt_accountNo.Text.Trim() == "0")
            //{
            //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cannot Update Cash Tariff')", true);
            //    err_msg.Text = "Cannot Update Cash Tariff";
            //    return;
            //}
            DataTable tariff = ViewState["dt_Previous"] as DataTable;
            if (e.CommandName == "del")
            {
                DataRow dr = tariff.Select("ID = '" + e.CommandArgument.ToString() + "'")[0];
                clvar.TariffID = e.CommandArgument.ToString();
                DeleteTariff(clvar);

                btn_showTariff_Click(sender, e);
            }

        }

        protected void btn_reset_Click(object sender, EventArgs e)
        {
            ResetAll();
        }

        protected void ResetAll()
        {
            txt_accountNo.Text = "";
            txt_clientName.Text = "";
            txt_price.Text = "";
            dd_branch.SelectedValue = "0";
            dd_fromCat.SelectedValue = "0";
            dd_toCat.SelectedValue = "0";

            gv_tariff.DataSource = null;
            gv_tariff.DataBind();


        }

        protected void btn_save_Click(object sender, EventArgs e)
        {
            //    if (txt_accountNo.Text.Trim() == "0")
            //    {
            //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cannot Update Cash Tariff')", true);
            //        err_msg.Text = "Cannot Update Cash Tariff";
            //        return;
            //    }
            DataTable tariff = ViewState["dt"] as DataTable;
            int[] count = InsertRnRTariff(clvar, tariff);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Tarrif(s) Inserted: " + count[0].ToString() + "\\n')", true);

            // btn_showTariff_Click(sender, e);

            ResetAll();

        }

        protected void btn_cancel_Click(object sender, EventArgs e)
        {

        }

        public int DeleteTariff(Cl_Variables clvar)
        {
            int count = 0;
            string error = "";
            string query = "UPDATE RnR_Tarrif_Zonewise SET ChkDeleted = '1', modifiedby='" + Session["U_ID"].ToString() + "',modifiedon='" + DateTime.Now.ToString("yyyy-MM-dd") + "'  where id = '" + clvar.TariffID + "'";
            SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            try
            {
                sqlcon.Open();
                SqlCommand sqlcmd = new SqlCommand(query, sqlcon);
                sqlcmd.CommandType = CommandType.Text;

                count = sqlcmd.ExecuteNonQuery();
                //IsUnique = Int32.Parse(SParam.Value.ToString());
                // obj.XCode = obj.consignmentNo;
                sqlcon.Close();
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return count;
        }

        protected void gv_tariff_Actual_RowCommand(object sender, GridViewCommandEventArgs e)
        {   //if (txt_accountNo.Text.Trim() == "0")
            //{
            //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cannot Update Cash Tariff')", true);
            //    err_msg.Text = "Cannot Update Cash Tariff";
            //    return;
            //}



            DataTable tariff = ViewState["dt"] as DataTable;
            if (e.CommandName == "del")
            {
                DataRow dr = tariff.Select("ID = '" + e.CommandArgument.ToString() + "'")[0];
                if (dr["ISUPDATED"].ToString() == "INSERT")
                {
                    tariff.Rows.Remove(dr);
                }
                else
                {
                    dr["isUpdated"] = "DELETE";

                }
                tariff.AcceptChanges();

                gv_tariff_Actual.DataSource = null;
                gv_tariff_Actual.DataBind();

                gv_tariff_Actual.DataSource = tariff;
                gv_tariff_Actual.DataBind();
                ViewState["dt"] = tariff;
            }

        }

        public int[] InsertRnRTariff(Cl_Variables clvar, DataTable dt)
        {
            int[] count = new int[3];
            string query1 = "";
            string query2 = "";
            string query3 = "";
            string query4 = "";
            string query5 = "";
            string query6 = "";
            int insertCount = 0;
            int updateCount = 0;
            int deleteCount = 0;
            DataRow[] drInsert = dt.Select("ISUPDATED = 'INSERT'", "");
            insertCount = drInsert.Count();

            #region Insertion Query For New Records
            if (drInsert.Count() > 0)
            {
                query1 = "INSERT INTO RnR_Tarrif_Zonewise (ServiceTypeName,Client_ID, FromZone, ToZone, IsDefault, Value,CreatedBy,Createdon,ChkDeleted)";
                for (int i = 0; i < drInsert.Count() - 1; i++)
                {
                    query1 += "SELECT  '" + drInsert[i]["ServiceTypeName"].ToString() + "', " +
                                   "'" + drInsert[i]["ClientID"].ToString() + "', " +
                                  "       '" + drInsert[i]["FromZone"].ToString() + "', " +
                                  "       '" + drInsert[i]["ToZone"].ToString() + "', " +
                                  "       '0', " +
                                  "       '" + drInsert[i]["Price"].ToString() + "',\n" +
                                  "       '" + Session["U_ID"].ToString() + "',\n" +
                                  "       '" + DateTime.Now.ToString("yyyy-MM-dd") + "','0'\n" +

                                  "UNION ALL \n";
                    //query1 += " insert into tbl_TariffChange (accNo, creditclientid, modifiedby, modifiedon, recompute) values ((select accountno from CreditClients where id = " + drInsert[i]["ClientID"].ToString() + "), " + drInsert[i]["ClientID"].ToString() + ", '" + HttpContext.Current.Session["U_ID"].ToString() + "', getdate(), 0); ";
                }
                int j = drInsert.Count() - 1;
                query1 += "SELECT  '" + drInsert[j]["ServiceTypeName"].ToString() + "', " +
                          "'" + drInsert[j]["ClientID"].ToString() + "', " +
                         "       '" + drInsert[j]["FromZone"].ToString() + "', " +
                         "       '" + drInsert[j]["ToZone"].ToString() + "', " +
                         "       '0', " +
                         "       '" + drInsert[j]["Price"].ToString() + "',\n" +
                         "       '" + Session["U_ID"].ToString() + "',\n" +
                         "       '" + DateTime.Now.ToString("yyyy-MM-dd") + "','0';\n";
                query1 += " insert into tbl_TariffChange (accNo, creditclientid, modifiedby, modifiedon, recompute) values ((select accountno from CreditClients where id = " + drInsert[j]["ClientID"].ToString() + "), " + drInsert[j]["ClientID"].ToString() + ", '" + HttpContext.Current.Session["U_ID"].ToString() + "', getdate(), 0); ";
            }
            else
            {
                insertCount = 0;
            }
            #endregion




            SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            SqlTransaction trans;
            sqlcon.Open();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcon;
            trans = sqlcon.BeginTransaction();
            sqlcmd.Transaction = trans;
            sqlcmd.CommandType = CommandType.Text;
            try
            {
                if (insertCount != 0)
                {
                    sqlcmd.CommandText = query1;
                    count[0] = sqlcmd.ExecuteNonQuery();
                    if (count[0] > 1)
                    {
                        count[0] = count[0] - 1;
                    }
                }
                else
                {
                    count[0] = 0;
                }

                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                count[0] = 0;
            }
            finally
            {
                sqlcon.Close();
            }

            return count;
        }
    }
}