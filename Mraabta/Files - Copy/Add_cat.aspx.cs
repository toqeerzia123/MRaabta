using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using MRaabta.App_Code;
using System.Drawing;

namespace MRaabta.Files
{
    public partial class Add_cat : System.Web.UI.Page
    {
        Cl_Variables clvar = new Cl_Variables();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                getmcat();

            }
        }

        protected void getmcat()
        {
            string query = "SELECT MMC_ID, MMC_SDESC FROM VMM_MAINTENANCEMCATEGORY ORDER BY MMC_SDESC";
            SqlConnection con = new SqlConnection(clvar.Strcon());
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = query;
            cmd.CommandType = CommandType.Text;
            DataSet ds = new DataSet();
            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            con.Open();
            adp.Fill(ds);
            con.Close();
            if (ds.Tables[0].Rows.Count > 0)
            {
                dd_cat.DataSource = ds.Tables[0];

                dd_cat.DataTextField = "MMC_SDESC";
                dd_cat.DataValueField = "MMC_ID";
                dd_cat.DataBind();
                dd_cat.Items.Insert(0, new ListItem("Select Category", ""));
            }

        }
        protected void getamcat()
        {
            string query = "SELECT MMC_ID, MMC_SDESC FROM VMM_MAINTENANCEMCATEGORY ORDER BY MMC_SDESC";
            SqlConnection con = new SqlConnection(clvar.Strcon());
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = query;
            cmd.CommandType = CommandType.Text;
            DataSet ds = new DataSet();
            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            con.Open();
            adp.Fill(ds);
            con.Close();
            if (ds.Tables[0].Rows.Count > 0)
            {
                dd_asgmcat.DataSource = ds.Tables[0];

                dd_asgmcat.DataTextField = "MMC_SDESC";
                dd_asgmcat.DataValueField = "MMC_ID";
                dd_asgmcat.DataBind();
                dd_asgmcat.Items.Insert(0, new ListItem("Select Category", ""));
            }

        }
        protected void getmscat()
        {
            string query = "select * from vmm_maintenancescategory order by msc_sdesc;";
            SqlConnection con = new SqlConnection(clvar.Strcon());
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = query;
            cmd.CommandType = CommandType.Text;
            DataSet ds = new DataSet();
            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            con.Open();
            adp.Fill(ds);
            con.Close();
            if (ds.Tables[0].Rows.Count > 0)
            {
                dd_asgscat.DataSource = ds.Tables[0];

                dd_asgscat.DataTextField = "MsC_SDESC";
                dd_asgscat.DataValueField = "MsC_ID";
                dd_asgscat.DataBind();
                dd_asgscat.Items.Insert(0, new ListItem("Select Sub Category", ""));
            }

        }
        protected void rb_newcat_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_newcat.Checked == true)
            {
                tr1.Visible = true;
                tr2.Visible = false;
                tr3.Visible = false;
                tb1.Visible = false;
            }
            else if (rb_newscat.Checked == true)
            {
                getmcat();
                tr2.Visible = true;
                tr3.Visible = true;
                tr1.Visible = false;
                tb1.Visible = false;
            }
            else
            {
                getamcat();
                getmscat();
                tr2.Visible = false;
                tr3.Visible = false;
                tr1.Visible = false;
                tb1.Visible = true;
            }
        }
        protected void rb_newscat_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_newcat.Checked == true)
            {
                tr1.Visible = true;
                tr2.Visible = false;
                tr3.Visible = false;
                tb1.Visible = false;
            }
            else if (rb_newscat.Checked == true)
            {
                getmcat();
                tr2.Visible = true;
                tr3.Visible = true;
                tr1.Visible = false;
                tb1.Visible = false;
            }
            else
            {
                getamcat();
                getmscat();
                tr2.Visible = false;
                tr3.Visible = false;
                tr1.Visible = false;
                tb1.Visible = true;
            }
        }
        protected void btn_save_Click(object sender, EventArgs e)
        {
            string sql = "";
            bool same = false;

            if (rb_newcat.Checked == true)
            {
                sql = "select max(CAST (mmc_id AS int)) from vmm_maintenancemcategory";
            }
            else
            {
                sql = "select max(CAST (msc_id AS int)) from vmm_maintenancescategory";
            }
            int id = 0;
            if (rb_assgn.Checked == false)
            {
                SqlConnection con = new SqlConnection(clvar.Strcon());
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                con.Open();
                adp.Fill(ds);
                con.Close();

                id = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());
                id++;
            }



            string sqlinsert = "";
            if (rb_newcat.Checked == true)
            {
                sqlinsert = "insert into vmm_maintenancemcategory (mmc_id,mmc_sdesc,mmc_ldesc,createdby,Createdon,modifyBy,modifyOn)\n" +
                             "values('" + id + "','" + txt_cname.Text + "','" + txt_cname.Text + "','" + Session["User_Info"].ToString() + "',getdate(),'','')";
            }
            else if (rb_newscat.Checked == true)
            {
                sqlinsert = "insert into vmm_maintenancescategory (mmc_id,msc_id,msc_sdesc,msc_ldesc,createdby,Createdon,modifyBy,modifyOn)\n" +
                             "values('" + dd_cat.SelectedValue + "','" + id + "','" + txt_scname.Text + "','" + txt_scname.Text + "','" + Session["User_Info"].ToString() + "',getdate(),'','')";
            }
            else
            {
                sqlinsert = "insert into vmm_maintenancescategory (mmc_id,msc_id,msc_sdesc,msc_ldesc,createdby,Createdon,modifyBy,modifyOn)\n" +
                            "values('" + dd_asgmcat.SelectedValue + "','" + dd_asgscat.SelectedValue + "','" + dd_asgscat.SelectedItem.ToString() + "','" + dd_asgscat.SelectedItem.ToString() + "','" + Session["User_Info"].ToString() + "',getdate(),'','')";

                string sqchk = "select * from vmm_maintenancescategory where mmc_id='" + dd_asgmcat.SelectedValue + "' and msc_id='" + dd_asgscat.SelectedValue + "'";

                SqlConnection con2 = new SqlConnection(clvar.Strcon());
                SqlCommand cmd2 = new SqlCommand();
                cmd2.Connection = con2;
                cmd2.CommandText = sqchk;
                cmd2.CommandType = CommandType.Text;
                SqlDataAdapter adp2 = new SqlDataAdapter(cmd2);
                DataSet ds2 = new DataSet();
                con2.Open();
                adp2.Fill(ds2);
                con2.Close();
                if (ds2.Tables[0].Rows.Count > 0)
                {
                    same = true;
                }
            }

            if (same)
            {
                lb_result.Text = "Assignment already Exists";
                lb_result.ForeColor = Color.Red;
            }
            else
            {
                try
                {
                    SqlConnection con1 = new SqlConnection(clvar.Strcon());
                    SqlCommand cmd1 = new SqlCommand();
                    cmd1.Connection = con1;
                    cmd1.CommandText = sqlinsert;
                    cmd1.CommandType = CommandType.Text;
                    con1.Open();
                    cmd1.ExecuteNonQuery();
                    con1.Close();
                    if (rb_newcat.Checked == true)
                    {
                        lb_result.Text = "Category Added";
                        lb_result.ForeColor = Color.Red;
                    }
                    else
                    {
                        lb_result.Text = "Subcategory Added in Category:" + dd_cat.SelectedItem.ToString() + "";
                        lb_result.ForeColor = Color.Red;
                    }
                }
                catch (Exception ex)
                {

                }
            }


        }
        protected void rb_assgn_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_newcat.Checked == true)
            {
                tr1.Visible = true;
                tr2.Visible = false;
                tr3.Visible = false;
                tb1.Visible = false;
            }
            else if (rb_newscat.Checked == true)
            {
                getmcat();
                tr2.Visible = true;
                tr3.Visible = true;
                tr1.Visible = false;
                tb1.Visible = false;
            }
            else
            {
                getmscat();
                getamcat();
                tr2.Visible = false;
                tr3.Visible = false;
                tr1.Visible = false;
                tb1.Visible = true;
            }
        }
    }
}