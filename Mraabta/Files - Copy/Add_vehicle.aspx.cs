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
    public partial class Add_vehicle : System.Web.UI.Page
    {
        Cl_Variables clvar = new Cl_Variables();
        CommonFunction func = new CommonFunction();
        Consignemnts con = new Consignemnts();
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                getvehicle();
                getzone();
                getvm();
                dd_br.Items.Insert(0, new ListItem("Select Branches", ""));
            }
        }

        protected void getvehicle()
        {
            string sql = "select TypeID,TypeDesc from vehicle_type where status='1'";
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
            if (ds.Tables[0].Rows.Count > 0)
            {
                dd_vt.DataSource = ds.Tables[0];
                dd_vt.DataTextField = "TypeDesc";
                dd_vt.DataValueField = "TypeID";
                dd_vt.DataBind();


                dd_vt.Items.Insert(0, new ListItem("Select Vehicle Type", ""));
            }
        }
        protected void getvm()
        {
            string sql = "select * from MnP_VAN_TYPE where status ='1'";

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
            if (ds.Tables[0].Rows.Count > 0)
            {
                dd_vm.DataSource = ds.Tables[0];
                dd_vm.DataTextField = "name";
                dd_vm.DataValueField = "vehicle_type_id";
                dd_vm.DataBind();
                dd_vm.Items.Insert(0, new ListItem("Select Vehicle Manage", ""));

            }
        }
        protected void getzone()
        {
            string sql = "select zonecode,name from zones where region is not null order by name asc";
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
            if (ds.Tables[0].Rows.Count > 0)
            {
                dd_zone.DataSource = ds.Tables[0];
                dd_zone.DataTextField = "name";
                dd_zone.DataValueField = "zonecode";
                dd_zone.DataBind();


                dd_zone.Items.Insert(0, new ListItem("Select Zone", ""));
            }
        }
        protected void dd_zone_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sql = "select * from branches  where zonecode='" + dd_zone.SelectedValue + "' and status='1' order by name";
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
            if (ds.Tables[0].Rows.Count > 0)
            {
                dd_br.DataSource = ds.Tables[0];
                dd_br.DataTextField = "name";
                dd_br.DataValueField = "branchCode";
                dd_br.DataBind();
                dd_br.Items.Insert(0, new ListItem("Select Branches", ""));
            }
        }

        protected void btn_save_Click(object sender, EventArgs e)
        {
            if (txt_name.Text == "")
            {
                errname.Text = "Please Enter Name";
                errname.ForeColor = Color.Red;
                errname.Visible = true;
            }
            else
            {
                errname.Visible = false;
                string sql1 = "select name from vehicle where name=upper('" + txt_name.Text + "')";

                SqlConnection con = new SqlConnection(clvar.Strcon());
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = sql1;
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                con.Open();
                adp.Fill(ds);
                // con.Close();
                if (ds.Tables[0].Rows.Count > 0)
                {

                    result.Text = "Vehicle With name " + txt_name.Text + " already exists";
                    result.ForeColor = Color.Red;
                }
                else
                {

                    string status = "";
                    if (rb_a.Checked == true)
                    {
                        status = "1";
                    }
                    else
                    {
                        status = "0";
                    }
                    string sql = "insert into vehicle (name,description,status,createdBy,createdon,branchcode,zonecode,vehicletype,VehicleMaintain)\n" +
                        "values(Upper('" + txt_name.Text + "'),'" + txt_des.Text + "','" + status + "','" + Session["User_Info"].ToString() + "',getdate(),'" + dd_br.SelectedValue + "','" + dd_zone.SelectedValue + "','" + dd_vt.SelectedValue + "','" + dd_vm.SelectedValue + "')";
                    if (dd_vt.SelectedValue == "")
                    {
                        errvt.Text = "Please Select Vehicle Type";
                        errvt.ForeColor = Color.Red;
                        errvt.Visible = true;

                    }
                    else
                    {
                        errvt.Visible = false;
                    }
                    if (dd_vm.SelectedValue == "")
                    {
                        errvm.Text = "Please Select Vehicle Maintain";
                        errvm.ForeColor = Color.Red;
                        errvm.Visible = true;
                    }
                    else
                    {
                        errvm.Visible = false;
                    }
                    if (dd_br.SelectedValue == "")
                    {
                        errbr.Text = "Please Select Branch";
                        errbr.ForeColor = Color.Red;
                        errbr.Visible = true;
                    }
                    else
                    {
                        errbr.Visible = false;
                    }
                    if (dd_zone.SelectedValue == "")
                    {
                        errzone.Text = "Please Select Zone";
                        errzone.ForeColor = Color.Red;
                        errzone.Visible = true;
                    }
                    else
                    {
                        errzone.Visible = false;
                    }

                    if (txt_name.Text == "")
                    {
                        errname.Text = "Please Enter Name";
                        errname.ForeColor = Color.Red;
                        errname.Visible = true;
                    }
                    else
                    {
                        errname.Visible = false;
                    }

                    if (errname.Visible == true || errbr.Visible == true || errvm.Visible == true || errzone.Visible == true || errvt.Visible == true)
                    {

                    }
                    else
                    {
                        try
                        {
                            SqlConnection con1 = new SqlConnection(clvar.Strcon());
                            SqlCommand cmd1 = new SqlCommand();
                            cmd1.Connection = con;
                            cmd1.CommandText = sql;
                            cmd1.CommandType = CommandType.Text;
                            con1.Open();
                            cmd1.ExecuteNonQuery();
                            con1.Close();

                            result.Visible = true;
                            result.Text = "Vehicle Added";
                            result.ForeColor = Color.Red;
                            // Response.Write("<script>alert('Vehicle Added');</script>");

                            txt_name.Text = txt_des.Text = "";


                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
            }
        }
    }
}