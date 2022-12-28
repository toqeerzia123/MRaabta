using System;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using MRaabta.App_Code;
using System.Drawing;

namespace MRaabta.Files
{
    public partial class Staff_entry : System.Web.UI.Page
    {
        Cl_Variables clvar = new Cl_Variables();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                getzone();
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

        protected void dd_zone_SelectedIndexChanged1(object sender, EventArgs e)
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
                dd_branch.DataSource = ds.Tables[0];
                dd_branch.DataTextField = "name";
                dd_branch.DataValueField = "branchCode";
                dd_branch.DataBind();
                dd_branch.Items.Insert(0, new ListItem("Select Branches", ""));
            }
        }

        protected void btn_save_Click(object sender, EventArgs e)
        {
            if (txt_stfcode.Text == "")
            {
                l1.Text = "Please Select Staff Code";
                l1.ForeColor = Color.Red;
                return;
            }
            else { l1.Text = ""; }

            if (txt_stfname.Text == "")
            {
                Label1.Text = "Please Enter Staff Name";
                Label1.ForeColor = Color.Red;
                return;
            }
            else { Label1.Text = ""; }

            if (txt_designation.Text == "")
            {
                Label6.Text = "Please Enter Designation";
                Label6.ForeColor = Color.Red;
                return;
            }
            else { Label6.Text = ""; }


            //if (txt_cnic.Text == "")
            //{
            //    Label2.Text = "Please Select CNIC";
            //    Label2.ForeColor = Color.Red;
            //}
            //else { Label2.Text = ""; }

            //if (txt_phone.Text == "")
            //{
            //    Label3.Text = "Please Select Phone";
            //    Label3.ForeColor = Color.Red;
            //}
            //else
            //{
            //    Label3.Text = "";
            //} 
            if (dd_zone.SelectedValue == "")
            {
                Label4.Text = "Please Select Zone";
                Label4.ForeColor = Color.Red;
                return;
            }
            else
            {
                Label4.Text = "";
            }
            if (dd_branch.SelectedValue == "")
            {
                Label5.Text = "Please Select Branch";
                Label5.ForeColor = Color.Red;
                return;
            }
            else
            {
                Label5.Text = "";
            }


            string sqlString = "insert into MNP_VEHICLE_STAFF (staffcode,staffname,designation,CNIC,zone,contactno,Branch,Createdby,CreatedOn,ModifyBy,ModifyOn)\n" +
            "values('" + txt_stfcode.Text + "','" + txt_stfname.Text + "','" + txt_designation.Text + "','" + txt_cnic.Text + "','" + dd_zone.SelectedValue + "','" + txt_phone.Text + "','" + dd_branch.SelectedValue + "','" + Session["User_Info"].ToString() + "',getdate(),'','')";


            try
            {
                SqlConnection con1 = new SqlConnection(clvar.Strcon());
                SqlCommand cmd1 = new SqlCommand();
                cmd1.Connection = con1;
                cmd1.CommandText = sqlString;
                cmd1.CommandType = CommandType.Text;
                con1.Open();
                cmd1.ExecuteNonQuery();
                con1.Close();
                result.Visible = true;
                result.Text = "Staff Added";
                result.ForeColor = Color.Red;

                txt_stfcode.Text = txt_stfname.Text = txt_phone.Text = txt_cnic.Text = txt_designation.Text = "";
            }
            catch (Exception ex)
            {

                result.Text = "Error Creating staff";
            }




        }
    }
}