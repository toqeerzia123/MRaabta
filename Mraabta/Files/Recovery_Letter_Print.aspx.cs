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
    public partial class Recovery_Letter_Print : System.Web.UI.Page
    {
        CommonFunction fun = new CommonFunction();
        Cl_Variables clvar = new Cl_Variables();
        Consignemnts con = new Consignemnts();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Complevel1();
            }
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
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {

                    dd_Complevel2.Items.Clear();
                    dd_Complevel2.Items.Add(new ListItem("Select Manager", "0"));
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
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    dd_Complevel3.DataSource = null;
                    dd_Complevel3.DataBind();

                    dd_Complevel3.Items.Add(new ListItem("Select Officer", "0"));

                    dd_Complevel3.DataSource = dt;
                    dd_Complevel3.DataTextField = "officerName";
                    dd_Complevel3.DataValueField = "OfficerID";
                    dd_Complevel3.DataBind();

                }
            }
        }

        public DataTable get_Complevel1()
        {
            //string query = "select zoneCode, name from Zones where status = '1' order by name";

            string sql = "SELECT cl.comp1 HeadID,cl.Name HeadName FROM Comp_Level1 cl WHERE cl.[Status]='1'";

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

            //string query = "select zoneCode, name from Zones where status = '1' order by name";

            string sql = "SELECT cl.comp2 ManagerID,cl.Name ManagerName FROM Comp_Level2 cl WHERE cl.[Status]='1' and comp1='" + dd_Complevel1.SelectedValue + "'";

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

            //string query = "select zoneCode, name from Zones where status = '1' order by name";

            string sql = "SELECT cl.comp3 OfficerID,cl.Name officerName, branch FROM Comp_Level3 cl WHERE cl.[Status]='1' and comp1='" + dd_Complevel1.SelectedValue + "' and cl.comp2='" + dd_Complevel2.SelectedValue + "'";

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
            ViewState["Comp3"] = dt;
            return dt;
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

        protected void dd_Complevel3_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)ViewState["Comp3"];
            DataRow[] dr = dt.Select("OfficerID='" + dd_Complevel3.SelectedValue + "'");

            ViewState["branch"] = dr[0]["branch"].ToString();


        }

        protected void btn_showTariff_Click(object sender, EventArgs e)
        {
            string clientgroup = "", days = "";

            if (this.txt_groupID.Text != "")
            {
                clientgroup = " and cc.clientGrpId='" + txt_groupID.Text + "' \n";
            }
            string fromdate = "", todate = "";
            if (this.dd_Days.SelectedValue != "")
            {

                if (dd_Days.SelectedValue == "60")
                {
                    days = "a.DayCount >='60' and a.DayCount <='90'";
                }
                if (dd_Days.SelectedValue == "90")
                {
                    days = "a.DayCount >='90' and a.DayCount <='120'";
                }
                if (dd_Days.SelectedValue == "120")
                {
                    days = "a.DayCount >='120' and a.DayCount <='150'";
                }
            }

            string sql = "SELECT mrli.*, \n"
               + "       case when cg.name is not null then cg.name else (Select Top(1) Name from creditclients where accountno=mrli.accountno) end   GroupName, case when days ='60' then 'N/A' else Ref_LetterNo end Ref_Letter,l.AttributeDesc Lettertype,(Select Top(1) Name from creditclients where accountno=mrli.accountno) \n"
               + "FROM   Mnp_Recovery_Letter_info mrli \n"
               + "       left outer JOIN ClientGroups cg \n"
               + "            ON  cg.id = mrli.ClientGroupID \n"
               + "       INNER JOIN rvdbo.Lookup l ON l.Id = mrli.Letter_Type \n"
               + "WHERE  mrli.EntryDate = '" + txt_date.Text + "'";

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

            if (Ds_1.Rows.Count != 0)
            {
                gv_tariff.DataSource = Ds_1.DefaultView;
                gv_tariff.DataBind();
            }
            else
            {
                gv_tariff.DataSource = null;
                gv_tariff.DataBind();

            }
        }
        protected void gv_tariff_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }
        protected void gv_tariff_DataBound(object sender, EventArgs e)
        {

        }
        protected void gv_tariff_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string days = (e.Row.FindControl("txt_Days") as Label).Text;
                string Letter_Ref_No = (e.Row.FindControl("txt_LetterRefno") as Label).Text;

                if (int.Parse(days) < 90)
                {
                    (e.Row.FindControl("hy_1") as HyperLink).NavigateUrl = "Recovery_Letter_Format.aspx?Letter_Ref_No=" + Letter_Ref_No;
                }
                else
                {
                    (e.Row.FindControl("hy_1") as HyperLink).NavigateUrl = "Recovery_Letter_Format2.aspx?Letter_Ref_No=" + Letter_Ref_No;
                }
            }
        }

        protected void btn_save_Click(object sender, EventArgs e)
        {
            if (txt_date.Text.Length == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Date Selection is Compulsory')", true);
                //   err_msg.Text = "Cannot Update Cash Tariff";
                return;
            }


            int count = 0;
            string sql = "INSERT INTO Mnp_Recovery_Letter_info \n"
               + "( \n"
               + "	-- Id -- this column value is auto-generated \n"
               + "	ClientGroupID, \n"
               + "	Days, \n"
               + "	Letter_Type, \n"
               + "	invoiceAmount, \n"
               + "	OutStandingAmount, \n"
               + "	Remarks, \n"
               + "	Letter_RefNo, \n"
               + "	EntryDate, \n"
               + "	CreatedBy,CreatedOn,Ref_LetterNo \n"
               + "	 \n"
               + ")";

            int count_ = 0;
            string sql_ = "";
            foreach (GridViewRow gr in gv_tariff.Rows)
            {
                CheckBox cb = (CheckBox)gr.FindControl("cb_Status");
                if (cb.Checked == true)
                {
                    count_++;
                    string clientid = ((HiddenField)gr.FindControl("hd_clientid")).Value;
                    string InvoiceAmount = ((Label)gr.FindControl("txt_InvoiceAmount")).Text;
                    string Outstanding = ((Label)gr.FindControl("txt_Outstanding")).Text;
                    string comments = ((TextBox)gr.FindControl("txt_comments")).Text;
                    string Visitdate = txt_date.Text;
                    string DayCount = dd_Days.SelectedValue;
                    string Letter = "";
                    string Letter_NO = "";
                    string Letter_Refno = "";


                    if (DayCount == "60")
                    {
                        DataSet ds = LetterNumber("1");
                        Letter_NO = ds.Tables[0].Rows[0][0].ToString();
                        Letter = "224";
                    }
                    else if (DayCount == "90")
                    {
                        DataSet ds = LetterNumber("2");
                        Letter_NO = ds.Tables[0].Rows[0][0].ToString();
                        Letter = "225";
                        DataSet ds_ = post_letter_Refno(clientid);
                        Letter_Refno = ds_.Tables[0].Rows[0]["Letter_RefNo"].ToString();

                    }

                    sql_ = sql_ + "SELECT \n"
                               + "	'" + clientid + "', \n"
                               + "  '" + DayCount + "', \n"
                               + "	'" + Letter + "', \n"
                               + "	'" + InvoiceAmount + "', \n"
                               + "	'" + Outstanding + "', \n"
                               + "	'" + comments + "', \n"
                               + "	'" + Letter_NO + "', \n"
                               + "	'" + Visitdate + "', \n"
                               + "	'" + Session["U_ID"].ToString() + "', \n"
                               + "	getdate(), \n"
                               + " '" + Letter_Refno + "' \n"
                               + " UNion All \n";
                    count++;
                }
            }
            if (count != 0)
            {
                sql_ = sql_.Remove(sql_.Length - 11);
                sql = sql + sql_;
            }
            if (count_ == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert(' was not Selected')", true);
                //   err_msg.Text = "Cannot Update Cash Tariff";
                return;

            }

            SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            try
            {
                sqlcon.Open();
                SqlCommand sqlcmd = new SqlCommand(sql, sqlcon);
                sqlcmd.CommandType = CommandType.Text;

                sqlcmd.ExecuteNonQuery();
                ////IsUnique = Int32.Parse(SParam.Value.ToString());
                //// obj.XCode = obj.consignmentNo;
                //sqlcon.Close();

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Records have been added')", true);

                gv_tariff.DataSource = null;
                gv_tariff.DataBind();

            }
            catch (Exception ex)
            {
                //break;
                err_msg.Text = ex.Message.ToString();// "Zone Must be selected";


            }
        }

        public DataSet LetterNumber(string identifier)
        {
            DataSet ds = new DataSet();
            SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            try
            {
                sqlcon.Open();
                SqlCommand sqlcmd = new SqlCommand("sp_LetterRefNO", sqlcon);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.AddWithValue("identifier", identifier);
                SqlDataAdapter sda = new SqlDataAdapter(sqlcmd);
                sda.Fill(ds);

                //IsUnique = Int32.Parse(SParam.Value.ToString());
                // obj.XCode = obj.consignmentNo;
                sqlcon.Close();
            }
            catch (Exception Err)
            {

            }
            return ds;
        }


        public DataSet post_letter_Refno(string identifier)
        {
            DataSet ds = new DataSet();
            SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            try
            {
                string sql = "SELECT * FROM Mnp_Recovery_Letter_info mrli WHERE mrli.ClientGroupID ='" + identifier + "' AND mrli.Days <'90' AND mrli.Letter_Type='224'  ORDER BY mrli.CreatedOn DESC  \n";
                sqlcon.Open();
                SqlCommand sqlcmd = new SqlCommand("sp_LetterRefNO", sqlcon);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.AddWithValue("identifier", identifier);
                SqlDataAdapter sda = new SqlDataAdapter(sqlcmd);
                sda.Fill(ds);

                //IsUnique = Int32.Parse(SParam.Value.ToString());
                // obj.XCode = obj.consignmentNo;
                sqlcon.Close();
            }
            catch (Exception Err)
            {

            }
            return ds;
        }

        protected void btn_cancel_Click(object sender, EventArgs e)
        {

        }
        protected void up_1_DataBinding(object sender, EventArgs e)
        {

        }
    }
}