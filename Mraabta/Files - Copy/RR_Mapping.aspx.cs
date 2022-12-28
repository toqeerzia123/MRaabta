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
    public partial class RR_Mapping : System.Web.UI.Page
    {
        CommonFunction fun = new CommonFunction();
        Cl_Variables clvar = new Cl_Variables();
        Consignemnts con = new Consignemnts();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Zones();
                Industry();
                GetZonesForDomesticTariff();
                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[]
                {
                new DataColumn("ID"),
                new DataColumn("zonecode"),
                new DataColumn("ZoneName"),
                new DataColumn("branchCode"),
                new DataColumn("BranchName"),
                new DataColumn("Name"),
                new DataColumn("AccountNo"),
                new DataColumn("Groupid"),
                new DataColumn("GroupName"),
                new DataColumn("industry"),
                new DataColumn("RecoveryOfficer"),
                new DataColumn("industryName"),
                new DataColumn("TRO"),
                new DataColumn("RO"),
                });
                dt.AcceptChanges();
                ViewState["dt"] = dt;
                ViewState["dt_view"] = dt;


                Complevel1();
                Complevel2();
                Complevel3();
            }
        }

        public void Industry()
        {
            string sql = "SELECT id, tai.Name industry FROM tblAdminIndustry tai WHERE tai.IsActvie='1' \n";

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

            dd_Industry.Items.Clear();

            dd_Industry.DataTextField = "industry";
            dd_Industry.DataValueField = "id";
            dd_Industry.DataSource = dt.DefaultView;
            dd_Industry.DataBind();

        }

        public void Complevel1()
        {
            DataTable dt = get_Complevel1();
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    dd_Complevel1.DataSource = dt;
                    dd_Complevel1.DataTextField = "HeadName";
                    dd_Complevel1.DataValueField = "HeadID";
                    dd_Complevel1.DataBind();

                }
            }
        }

        public void Complevel2()
        {
            DataTable dt = get_Complevel2();

            dd_Complevel2.ClearSelection();
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {

                    dd_Complevel2.DataSource = null;
                    dd_Complevel2.DataBind();

                    //  dd_Complevel2.Items.Add(new ListItem("Select Manager", "0"));

                    dd_Complevel2.DataSource = dt;
                    dd_Complevel2.DataTextField = "ManagerName";
                    dd_Complevel2.DataValueField = "ManagerID";
                    dd_Complevel2.DataBind();

                }
            }
        }

        public void Complevel3()
        {
            DataTable dt = get_Complevel3();
            // dd_Complevel2.ClearSelection();
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    dd_Complevel3.DataSource = null;
                    dd_Complevel3.DataBind();

                    //dd_Complevel3.Items.Add(new ListItem("Select Officer", "0"));


                    dd_Complevel3.DataSource = dt;
                    dd_Complevel3.DataTextField = "officerName";
                    dd_Complevel3.DataValueField = "OfficerID";
                    dd_Complevel3.DataBind();

                }
            }
        }

        protected void Zones()
        {
            DataTable dt = GetZonesForDomesticTariff();
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    dd_Zone.DataSource = dt;
                    dd_Zone.DataTextField = "name";
                    dd_Zone.DataValueField = "zoneCode";
                    dd_Zone.DataBind();

                    //dd_toZone.DataSource = dt;
                    //dd_toZone.DataTextField = "name";
                    //dd_toZone.DataValueField = "zoneCode";
                    //dd_toZone.DataBind();
                    //dd_serviceType.SelectedValue = Session["BranchCode"].ToString();
                }
            }
        }

        protected void btn_showTariff_Click(object sender, EventArgs e)
        {
            #region Validations
            if (dd_Zone.SelectedValue == "0")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Select Zone')", true);
                return;
            }
            #endregion
            string account = "", groupid = "", branch = "", industry = "";
            if (txt_accountNo.Text != "")
            {
                account = " and accountno='" + txt_accountNo.Text + "' \n";
            }
            if (this.txt_groupID.Text != "")
            {
                groupid = " and clientGrpId='" + txt_groupID.Text + "' \n";
            }
            if (this.dd_Branch.SelectedValue != "0")
            {
                branch = " and cc.branchCode='" + dd_Branch.SelectedValue + "' \n";
            }
            if (this.dd_Industry.SelectedValue != "")
            {
                industry = " and  cc.IndustryId='" + dd_Industry.SelectedValue + "'";
            }

            string sql = "SELECT cc.id, \n"
                + "       cc.zonecode, \n"
                + "       z.name     ZoneName, \n"
                + "       cc.branchCode, \n"
                + "       b.name     BranchName, \n"
                + "       cc.name, \n"
                + "       cc.accountNo, \n"
                + "       cc.clientGrpId, \n"
                + "       ua.username RecoveryOfficer, \n"
                + "       ( \n"
                + "           SELECT cg.name \n"
                + "           FROM   ClientGroups cg \n"
                + "           WHERE  cg.id = cc.clientGrpId \n"
                + "                  AND cg.[status] = '1' \n"
                + "       )          GroupName, \n"
                + "       cc.IndustryId, \n"
                + "       tai.Name IndustryName, \n"
                + "       cc.TRO, \n"
                + "       cc.RO \n"
                + "FROM   CreditClients cc \n"
                + "       INNER JOIN tblAdminIndustry tai \n"
                + "            ON  tai.id = cc.IndustryId \n"
                + "       INNER JOIN Zones z \n"
                + "            ON  z.zoneCode = cc.zonecode \n"
                + "       INNER JOIN Branches b \n"
                + "            ON  b.branchCode = cc.branchCode \n"
                + "       LEFT JOIN ClientStaff_temp cs ON cs.ClientId = cc.id AND cs.StaffTypeId = '215' \n"
                + "       LEFT join UserAssociation ua ON cs.comp3 = ua.employeeCode \n"
                + "WHERE  tai.IsActvie = '1' and b.status='1' and  cc.zonecode ='" + dd_Zone.SelectedValue + "' \n";
            sql = sql + branch;
            sql = sql + account;
            sql = sql + groupid;
            sql = sql + industry;


            DataTable Ds_1 = new DataTable();

            try
            {
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



            DataTable dt_ = (DataTable)ViewState["dt"];
            dt_.Clear();
            if (Ds_1.Rows.Count > 0)
            {
                foreach (DataRow row in Ds_1.Rows)
                {

                    DataRow dr = dt_.NewRow();
                    dr["ID"] = row["ID"].ToString();
                    dr["zonecode"] = row["zonecode"].ToString();
                    dr["ZoneName"] = row["ZONENAME"].ToString();
                    dr["branchCode"] = row["branchCode"].ToString();
                    dr["BranchName"] = row["BranchName"].ToString();
                    dr["Name"] = row["Name"].ToString();
                    dr["AccountNo"] = row["AccountNo"].ToString();
                    dr["Groupid"] = row["clientGrpId"].ToString();
                    dr["GroupName"] = row["GroupName"].ToString();
                    dr["industry"] = row["IndustryId"].ToString();
                    dr["industryName"] = row["industryName"].ToString();
                    dr["RecoveryOfficer"] = row["RecoveryOfficer"].ToString();
                    dr["TRO"] = row["TRO"].ToString();
                    dr["RO"] = row["RO"].ToString();

                    dt_.Rows.Add(dr);
                    dt_.AcceptChanges();

                }

                if (dt_.Rows.Count != 0)
                {
                    gv_tariff.DataSource = dt_.DefaultView;
                    gv_tariff.DataBind();
                }
                else
                {
                    gv_tariff.DataSource = null;
                    gv_tariff.DataBind();

                }

            }

            //btn_addPrice.Enabled = false;




            ViewState["Tr"] = dt_;
        }

        public DataTable get_Complevel1()
        {
            string sql = "";
            sql = "SELECT cl.comp1 HeadID,cl.Name HeadName FROM Comp_Level1 cl WHERE cl.[Status]='1'";
            /*
            if (Session["U_ID"].ToString() == "5280")
            {
                sql = "SELECT cl.comp1 HeadID,cl.Name HeadName FROM Comp_Level1 cl WHERE cl.[Status]='1'";
            }
            else
            {
                sql = "";
            }
            */
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

        public DataTable get_Complevel2()
        {
            string sql = "";

            if (Session["U_ID"].ToString() == "5280")
            {
                sql = "SELECT cl.comp2 ManagerID,upper(cl.Name) ManagerName FROM Comp_Level2 cl WHERE cl.[Status]='1' and comp1='" + dd_Complevel1.SelectedValue + "'";
            }
            else
            {
                sql = @"SELECT cl.comp2 ManagerID,upper(cl.Name) ManagerName 
                    FROM Comp_Level2 cl 
                    WHERE cl.[Status]='1' 
                    and comp1='" + dd_Complevel1.SelectedValue + @"' 
                    and addtional_Fields2 = '" + Session["U_ID"].ToString() + @"'
                    ORDER BY 2 ASC";
            }

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

        public DataTable get_Complevel3()
        {
            string sql = "";

            //sql = "SELECT cl.comp3 OfficerID,cl.Name officerName FROM Comp_Level3 cl WHERE cl.[Status]='1' and comp1='" + dd_Complevel1.SelectedValue + "' and cl.comp2='" + dd_Complevel2.SelectedValue + "'";


            if (dd_Complevel2.SelectedValue == "" || dd_Complevel2.SelectedValue == "0")
            {
                sql = "SELECT upper(cl.Name)+' ('+u.employeeCode+')' officerName,cl.comp3 OfficerID, branch \n" +
                            "FROM Comp_Level3 cl \n" +
                            "INNER JOIN UserAssociation u ON cl.Name = u.username \n" +
                            "WHERE cl.[Status]='1' \n" +
                            "and addtional_Fields4 = '" + Session["U_ID"].ToString() + "'  order by cl.Name asc";
            }
            else
            {
                sql = "SELECT upper(cl.Name)+' ('+u.employeeCode+')' officerName,cl.comp3 OfficerID, branch \n" +
                           "FROM Comp_Level3 cl \n" +
                           "INNER JOIN UserAssociation u ON cl.Name = u.username \n" +
                           "WHERE cl.[Status]='1' \n" +
                           // "and comp1='" + dd_Complevel1.SelectedValue + "' \n" +
                           "and cl.comp2='" + dd_Complevel2.SelectedValue + "'  order by cl.Name asc";
            }

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

        public DataTable GetZonesForDomesticTariff()
        {
            //string query = "select zoneCode, name from Zones where status = '1' order by name";

            //string query = "select z.zoneCode, z.name\n" +
            //"  from Zones z\n" +
            //" inner join Branches b\n" +
            //"    on b.zoneCode = z.zoneCode\n" +
            //" where z.status = '1'\n" +
            //"   and b.branchCode <> '17'\n" +
            //" group by z.colorId,\n" +
            //"          z.createdBy,\n" +
            //"          z.createdOn,\n" +
            //"          z.description,\n" +
            //"          z.email,\n" +
            //"          z.faxNo,\n" +
            //"          z.hasStore,\n" +
            //"          z.modifiedBy,\n" +
            //"          z.modifiedOn,\n" +
            //"          z.name,\n" +
            //"          z.phoneNo,\n" +
            //"          z.status,\n" +
            //"          z.type,\n" +
            //"          z.zoneCode\n" +
            //"union all \n" +
            //" select z.zoneCode, z.name from Zones z where z.zoneCode in ('14','16','17','DIFF','LOCAL','SAME')\n" +
            //"order by name";

            string query = "select z.zoneCode, z.name from Zones z where region is not null ORDER BY z.name ASC";

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

        protected void Branches()
        {
            clvar.Zone = dd_Zone.SelectedValue;
            DataTable dt = Branch_(clvar).Tables[0];
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    dd_Branch.Items.Clear();
                    dd_Branch.Items.Add(new ListItem("Select Branch", "0"));
                    dd_Branch.DataSource = dt;
                    dd_Branch.DataTextField = "BranchName";
                    dd_Branch.DataValueField = "branchCode";
                    dd_Branch.DataBind();
                    //dd_branch.SelectedValue = Session["BranchCode"].ToString();
                }
            }
        }

        public DataSet Branch_(Cl_Variables clvar)
        {
            DataSet Ds_1 = new DataSet();

            try
            {
                string query = "SELECT b.branchCode, \n"
               + "       b.name     BranchName, sname BName \n"
               + "FROM   Branches                          b \n"
               + "where b.[status] ='1'  \n"
               + " and zonecode ='" + clvar.Zone + "'\n"
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

        protected void dd_Zone_SelectedIndexChanged(object sender, EventArgs e)
        {
            Branches();
        }
        protected void txt_accountNo_TextChanged(object sender, EventArgs e)
        {

        }

        protected void gv_tariff_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "del")
            {
                clvar.TariffID = e.CommandArgument.ToString();
                if (clvar.TariffID != "")
                {

                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Tariff Deleted')", true);
                    DataTable dt = ViewState["Tr"] as DataTable;

                    DataRow[] dr = dt.Select("ID = '" + clvar.TariffID + "'", "");
                    dt.Rows.Remove(dr[0]);
                    dt.AcceptChanges();
                    ViewState["Tr"] = dt;
                    gv_tariff.DataSource = null;
                    gv_tariff.DataBind();
                    gv_tariff.DataSource = dt;
                    gv_tariff.DataBind();
                    return;
                }

            }
        }

        protected void gv_tariff_RowDeleted(object sender, GridViewDeletedEventArgs e)
        {

        }

        protected void gv_tariff_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void gv_tariff_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
            }
        }
        protected void gv_tariff_DataBound(object sender, EventArgs e)
        {

        }
        protected void dd_Complevel1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dd_Complevel1.SelectedValue != "0")
            {
                dd_Complevel2.DataSource = null;
                dd_Complevel2.DataBind();
                Complevel2();
                dd_Complevel3.DataSource = null;
                dd_Complevel3.DataBind();
            }
            else
            { }
        }
        protected void dd_Complevel2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dd_Complevel2.SelectedValue != "0")
            {
                Complevel3();
            }
        }
        protected void btn_save_Click(object sender, EventArgs e)
        {
            if (txt_accountNo.Text == "0")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cannot Assign Cash Account')", true);
                err_msg.Text = "Cannot Assign Cash Account";
                return;
            }
            if (dd_Zone.SelectedValue == "0")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Zone Must be selected')", true);
                err_msg.Text = "Zone Must be selected";
                return;
            }
            //if (dd_Complevel1.SelectedValue == "0")
            //{
            //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Head of Dept Must be selected')", true);
            //    err_msg.Text = "Head of Dept Must be selected";
            //    return;
            //}
            //if (dd_Complevel2.SelectedValue == "0")
            //{
            //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Manager Must be selected')", true);
            //    err_msg.Text = "Manager Must be selected";
            //    return;
            //}
            if (dd_Complevel3.SelectedValue == "0")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Officer Must be selected')", true);
                err_msg.Text = "Officer Must be selected";
                return;
            }

            int count = 0;
            string sql_r = "";
            foreach (GridViewRow gr in gv_tariff.Rows)
            {
                CheckBox cb = (CheckBox)gr.FindControl("cb_Status");
                if (cb.Checked == true)
                {
                    string clientid = ((HiddenField)gr.FindControl("hd_clientid")).Value;
                    sql_r = sql_r + "DELETE FROM ClientStaff_temp WHERE ClientId = '" + clientid + "' \n";

                }
            }


            string sql_u = "";
            foreach (GridViewRow gr in gv_tariff.Rows)
            {
                CheckBox cb = (CheckBox)gr.FindControl("cb_Status");
                if (cb.Checked == true)
                {
                    string clientid = ((HiddenField)gr.FindControl("hd_clientid")).Value;
                    sql_u = sql_u + "UPDATE ClientStaff SET UserName = '" + dd_Complevel3.SelectedItem + "' WHERE ClientId = '" + clientid + "' \n";

                }
            }

            string sql = "INSERT INTO ClientStaff_temp \n"
                      + "( \n"
                      + "	-- Id -- this column value is auto-generated \n"
                      + "	ClientId, \n"
                      + "	UserName, \n"
                      + "	StaffTypeId, \n"
                      + "	comp1, \n"
                      + "	comp2, \n"
                      + "	comp3, \n"
                      + "	[status], \n"
                      + "	createdon, \n"
                      + "	createdby \n"
                      + ") \n";
            string sql_ = "";
            foreach (GridViewRow gr in gv_tariff.Rows)
            {
                CheckBox cb = (CheckBox)gr.FindControl("cb_Status");
                if (cb.Checked == true)
                {
                    string clientid = ((HiddenField)gr.FindControl("hd_clientid")).Value;
                    string name = dd_Complevel3.SelectedItem.ToString();
                    string username = name.Substring(0, name.LastIndexOf("(") -1);
                    string comp1 = dd_Complevel1.SelectedValue;
                    string comp2 = dd_Complevel2.SelectedValue;
                    string comp3 = dd_Complevel3.SelectedValue;


                    sql_ = sql_ + "SELECT \n"
                               + "  '" + clientid + "', \n"
                               + "	'"+ username + "', \n"
                               + "	(Select top(1) addtional_Fields2 from comp_level3 cl where comp1 ='" + comp1 + "' and comp2='" + comp2 + "' and comp3 ='" + comp3 + "'), \n"
                               + "	'" + comp1 + "', \n"
                               + "	'" + comp2 + "', \n"
                               + "	'" + comp3 + "', \n"
                               + "	'1', \n"
                               + "	getdate(), \n"
                               + "	'" + Session["U_ID"].ToString() + "' \n"
                               + " \n"
                               + " UNion All \n";
                    count++;
                }
            }
            if (count != 0)
            {
                sql_ = sql_.Remove(sql_.Length - 11);
                sql = sql_r + sql_u + sql + sql_;
            }
            SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            try
            {
                sqlcon.Open();
                SqlCommand sqlcmd = new SqlCommand(sql, sqlcon);
                sqlcmd.CommandType = CommandType.Text;

                sqlcmd.ExecuteNonQuery();
                //IsUnique = Int32.Parse(SParam.Value.ToString());
                // obj.XCode = obj.consignmentNo;
                sqlcon.Close();

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Clients(s) Added')", true);

                //string script_ = @"window.open(""{0}"", ""{1}"", ""{2}"");";
                ////string script = String.Format(script_, "RunsheetInvoice.aspx?Xcode=" + temp_, "_blank", "");
                //string script = String.Format(script_, "Print_Tariffs.aspx?ClientID=" + creditclientid.Value + "&ServiceType=" + dd_serviceType.SelectedValue, "_blank", "");
                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Redirect", script, true);


                gv_tariff.DataSource = null;
                gv_tariff.DataBind();
                dd_Complevel3_SelectedIndexChanged(sender, e);

            }
            catch (Exception ex)
            {
                //break;
                err_msg.Text = ex.Message.ToString();// "Zone Must be selected";


            }


        }

        protected void btn_cancel_Click(object sender, EventArgs e)
        {

        }

        protected void dd_Complevel3_SelectedIndexChanged(object sender, EventArgs e)
        {
            string OfficerInformation = dd_Complevel3.SelectedValue;

            string sql = "SELECT cc.id, \n"
         + "       cc.zonecode, \n"
         + "       z.name     ZoneName, \n"
         + "       cc.branchCode, \n"
         + "       b.name     BranchName, \n"
         + "       cc.name, \n"
         + "       cc.accountNo, \n"
         + "       cc.clientGrpId, \n"
         + "       ( \n"
         + "           SELECT cg.name \n"
         + "           FROM   ClientGroups cg \n"
         + "           WHERE  cg.id = cc.clientGrpId \n"
         + "                  AND cg.[status] = '1' \n"
         + "       )          GroupName, \n"
         + "       cc.IndustryId, \n"
         + "       tai.Name IndustryName, \n"
         + "       cc.TRO, \n"
         + "       cc.RO \n"
         + "FROM   CreditClients cc \n"
         + "       INNER JOIN tblAdminIndustry tai \n"
         + "            ON  tai.id = cc.IndustryId \n"
         + "       INNER JOIN Zones z \n"
         + "            ON  z.zoneCode = cc.zonecode \n"
         + "       INNER JOIN Branches b \n"
         + "            ON  b.branchCode = cc.branchCode \n"
         + "        inner join ClientStaff_temp cl  \n"
         + "           on cc.id = cl.clientid \n"
         + "WHERE  tai.IsActvie = '1' and b.status='1' and  cl.comp3='" + dd_Complevel3.SelectedValue + "' and cl.status='1' \n";


            DataTable Ds_1 = new DataTable();

            try
            {
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

            DataTable dt_ = (DataTable)ViewState["dt_view"];
            dt_.Clear();
            if (Ds_1.Rows.Count > 0)
            {
                foreach (DataRow row in Ds_1.Rows)
                {

                    DataRow dr = dt_.NewRow();
                    dr["ID"] = row["ID"].ToString();
                    dr["zonecode"] = row["zonecode"].ToString();
                    dr["ZoneName"] = row["ZONENAME"].ToString();
                    dr["branchCode"] = row["branchCode"].ToString();
                    dr["BranchName"] = row["BranchName"].ToString();
                    dr["Name"] = row["Name"].ToString();
                    dr["AccountNo"] = row["AccountNo"].ToString();
                    dr["Groupid"] = row["clientGrpId"].ToString();
                    dr["GroupName"] = row["GroupName"].ToString();
                    dr["industry"] = row["IndustryId"].ToString();
                    dr["industryName"] = row["industryName"].ToString();
                    dr["TRO"] = row["TRO"].ToString();
                    dr["RO"] = row["RO"].ToString();

                    dt_.Rows.Add(dr);
                    dt_.AcceptChanges();

                }

                if (dt_.Rows.Count != 0)
                {
                    GridView1.DataSource = dt_.DefaultView;
                    GridView1.DataBind();
                }
                else
                {
                    GridView1.DataSource = null;
                    GridView1.DataBind();
                }
            }

        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "del")
            {
                clvar.CreditClientID = e.CommandArgument.ToString();

                //DataRow[] dr = dt.Select("ID = '" + clvar.TariffID + "'", "");
                int count = DeleteTariff(clvar);

                if (count != 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Client has been removed.')", true);
                    dd_Complevel3_SelectedIndexChanged(sender, e);
                }

            }
        }

        public int DeleteTariff(Cl_Variables clvar)
        {
            int count = 0;
            string error = "";
            string query = "UPDATE ClientStaff_temp SET status = '0' where clientid = '" + clvar.CreditClientID + "' and comp3='" + dd_Complevel3.SelectedValue + "'";
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
        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}