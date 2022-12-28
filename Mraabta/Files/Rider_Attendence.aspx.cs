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
    public partial class Rider_Attendence : System.Web.UI.Page
    {
        Cl_Variables clvar = new Cl_Variables();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                dd_riders.Text = System.DateTime.Now.ToString("yyyy-MM-dd");

                DataSet ds = Get_Origin();

                if (ds.Tables[0].Rows.Count > 0)
                {
                    dd_origin.DataTextField = "name";
                    dd_origin.DataValueField = "branchCode";
                    dd_origin.DataSource = ds;
                    dd_origin.DataBind();
                }

            }
        }

        public DataSet Get_Origin()
        {
            string sqlString = "select b.branchCode, b.name\n" +
            "  from Branches b\n" +
            " order by b.name";


            DataSet ds = new DataSet();
            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                orcl.Close();
            }
            catch (Exception Err)
            { }
            finally
            { }
            return ds;
        }


        public int insert_attendence(string riderCode, string origin, string date)
        {
            int un = 0;
            SqlConnection orcl = new SqlConnection(clvar.Strcon());


            string sqlString = "insert into Pickup_Rider_attendence(AttendenceDate,origin,riderCode) values ('" + date + "','" + origin + "','" + riderCode + "')";



            try
            {
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandType = CommandType.Text;
                orcd.ExecuteNonQuery();
            }
            catch (Exception Err)
            {
                un = 1;

            }
            finally
            {
                orcl.Close();
            }
            return un;
        }

        protected void btn_Mark_Click(object sender, EventArgs e)
        {
            foreach (RepeaterItem item in rp_Pickups.Items)
            {
                CheckBox chk_absent = (CheckBox)item.FindControl("chk_absent");
                if (chk_absent.Checked == true)
                {
                    HiddenField hd_riderCode = (HiddenField)item.FindControl("hd_riderCode");
                    string date = dd_riders.Text;
                    string origin = dd_origin.SelectedValue;

                    int inserter = insert_attendence(hd_riderCode.Value, origin, date);

                }
            }


            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Riders marked as ABSENT for date: " + dd_riders.Text + "')", true);

            Errorid.Text = "Riders marked as ABSENT for date: " + dd_riders.Text;

            dd_origin.ClearSelection();
            rp_Pickups.DataSource = null;
            rp_Pickups.DataBind();

        }
        protected void btn_Search_Click(object sender, EventArgs e)
        {
            string origin = dd_origin.SelectedValue;

            DataSet ds = Get_Riders(origin);

            if (ds.Tables[0].Rows.Count > 0)
            {
                rp_Pickups.DataSource = ds;
                rp_Pickups.DataBind();

            }
            else
            {
                Errorid.Text = "No rider found for selected Origin!";

            }
        }


        public DataSet Get_Riders(string origin)
        {
            string sqlString = "select r.phoneNo ContactNo,\n" +
            "       r.riderCode,\n" +
            "       r.firstName + ' - ' + r.lastname RiderName,\n" +
            "       b.branchCode,\n" +
            "       b.name origin\n" +
            "  from Riders r, Branches b\n" +
            " where b.branchCode = r.branchId\n" +
            "   and b.branchCode = '" + origin + "' order by r.firstName + ' - ' + r.lastname";

            DataSet ds = new DataSet();
            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                orcl.Close();
            }
            catch (Exception Err)
            { }
            finally
            { }
            return ds;
        }

    }
}