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
    public partial class Post_Pickup : System.Web.UI.Page
    {
        Cl_Variables clvar = new Cl_Variables();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
        }

        public DataSet Get_Origin(string AccountNo)
        {
            string sqlString = "select distinct c.branchCode, b.name, c.name AccountName, c.address\n" +
            "  from CreditClients c, Branches b\n" +
            " where c.accountNo = '" + AccountNo + "'\n" +
            "   and c.branchCode = b.branchCode\n" +
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


        public DataSet Get_Riders(string origin)
        {
            string sqlString = "select r.riderCode + ' - ' + r.firstName + ' ' + r.lastname FirstName, r.riderCode\n" +
            "  from Riders r\n" +
            " where r.branchId = '" + origin + "'\n" +
            " order by r.firstName";

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


        public DataSet Get_RiderPickups(string riderCode)
        {
            string sqlString = "select pd.riderCode,\n" +
            "       (r.firstName + ' ' + r.lastName) RiderName,\n" +
            "       b.name Origin,\n" +
            "       pd.riderPhone,\n" +
            "       pd.weight,\n" +
            "       pd.pieces,\n" +
            "       pd.pickup_Date,\n" +
            "       pd.pickup_status,\n" +
            "       ps.StatusDescription,\n" +
            "       pd.createdOn, pd.id, pd.remarks\n" +
            "  from Pickup_Details pd,\n" +
            "       CreditClients  cc,\n" +
            "       Riders         r,\n" +
            "       Branches       b,\n" +
            "       Pickup_Status  ps\n" +
            " where pd.AccountNo = cc.accountNo\n" +
            "   and pd.origin = cc.branchCode\n" +
            "   and pd.riderCode = r.riderCode\n" +
            "   and pd.origin = r.branchId\n" +
            "   and pd.origin = b.branchCode\n" +
            "   and ISNULL(pd.pickup_status,1) = ps.StatusCode\n" +
            "   and pd.riderCode = '" + riderCode + "'\n" +
            " order by pd.pickup_Date";



            sqlString = "select pd.riderCode,\n" +
            "       (r.firstName + ' ' + r.lastName) RiderName,\n" +
            "       b.name Origin,\n" +
            "       pd.riderPhone,\n" +
            "       pd.weight,\n" +
            "       pd.pieces,\n" +
            "       pd.pickup_Date,\n" +
            "       pd.pickup_status,\n" +
            "       ps.StatusDescription,\n" +
            "       pd.createdOn,\n" +
            "       pd.id,\n" +
            "       pd.remarks,\n" +
            "       cc.accountNo,\n" +
            "       cc.name\n" +
            "  from Pickup_Details pd,\n" +
            "       CreditClients  cc,\n" +
            "       Riders         r,\n" +
            "       Branches       b,\n" +
            "       Pickup_Status  ps\n" +
            " where pd.AccountNo = cc.accountNo\n" +
            "   and pd.origin = cc.branchCode\n" +
            "   and pd.riderCode = r.riderCode\n" +
            "   and pd.origin = r.branchId\n" +
            "   and pd.origin = b.branchCode\n" +
            "   and ISNULL(pd.pickup_status, 1) = ps.StatusCode\n" +
            "   " + clvar.CheckCondition + "--and pd.riderCode = '" + riderCode + "'\n" +
            "   and CAST(pd.pickup_Date as DATE) >= '" + DateTime.Parse(txt_dateFrom.Text).ToString("yyyy-MM-dd") + "'\n" +
            "   and CAST(pd.pickup_Date as DATE) <= '" + DateTime.Parse(txt_dateTo.Text).ToString("yyyy-MM-dd") + "'\n" +
            "\n" +
            " order by pd.pickup_Date";


            sqlString = "SELECT pd.riderCode,\n" +
            "       (r.firstName + ' ' + r.lastName) RiderName,\n" +
            "       b.name Origin,\n" +
            "       pd.riderPhone,\n" +
            "       pd.weight,\n" +
            "       pd.pieces,\n" +
            "       pd.pickup_Date,\n" +
            "       pd.pickup_status,\n" +
            "       ps.StatusDescription,\n" +
            "       pd.createdOn,\n" +
            "       pd.id,\n" +
            "       pd.remarks,\n" +
            "       cc.accountNo,\n" +
            "       cc.name\n" +
            "  FROM Pickup_Details pd\n" +
            " INNER JOIN creditclients cc\n" +
            "    ON cc.accountNo = pd.AccountNo\n" +
            "   AND cc.branchCode = pd.origin\n" +
            " INNER JOIN Riders r\n" +
            "    ON r.riderCode = pd.riderCode\n" +
            "   AND r.branchId = pd.origin\n" +
            "   AND r.status = '1'\n" +
            " INNER JOIN Branches b\n" +
            "    ON b.branchCode = pd.origin\n" +
            " INNER JOIN Pickup_Status ps\n" +
            "    ON ps.StatusCode = ISNULL(pd.pickup_status, 1)\n" +
            " WHERE 1 = 1\n" +
            "   " + clvar.CheckCondition + "--and pd.riderCode = '" + riderCode + "'\n" +
            "   and CAST(pd.pickup_Date as DATE) >= '" + DateTime.Parse(txt_dateFrom.Text).ToString("yyyy-MM-dd") + "'\n" +
            "   and CAST(pd.pickup_Date as DATE) <= '" + DateTime.Parse(txt_dateTo.Text).ToString("yyyy-MM-dd") + "'\n" +
            "   AND pd.origin = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'\n" +
            " ORDER BY pd.pickup_Date";

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


        public DataTable Get_PickupStatus()
        {
            string sqlString = "select * from Pickup_Status";

            DataTable ds = new DataTable();
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



        public int Update_Status(string id, string riderCode, string riderPhone, string status, string remarks)
        {
            int un = 0;
            SqlConnection orcl = new SqlConnection(clvar.Strcon());


            string sqlString = "update pickup_details set pickup_status = '" + status + "', remarks = '" + remarks + "', modifiedOn = GETDATE() where id= " + id + " and riderCode = '" + riderCode + "' and riderPhone = '" + riderPhone + "'";



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

        protected void dd_riders_TextChanged(object sender, EventArgs e)
        {

        }
        protected void btn_Search_Click(object sender, EventArgs e)
        {
            DateTime dt;
            DateTime.TryParse(txt_dateFrom.Text, out dt);
            if (dt == null)
            {
                return;
            }

            if (txt_dateFrom.Text.Trim() == "")
            {
                Errorid.Text = "Select From Date";
                return;
            }
            if (txt_dateTo.Text.Trim() == "")
            {
                Errorid.Text = "Select To Date";
                return;
            }
            string rider = dd_riders.Text;
            if (dd_riders.Text.Trim() != "")
            {
                clvar.CheckCondition = "and pd.RiderCode = '" + dd_riders.Text.Trim() + "'";
            }
            else
            {
                clvar.CheckCondition = "";
            }
            DataSet ds = new DataSet();

            ds = Get_RiderPickups(rider);

            DataTable statuses = Get_PickupStatus();

            if (ds.Tables[0].Rows.Count > 0)
            {
                Errorid.Text = "";
                rp_Pickups.DataSource = ds;
                rp_Pickups.DataBind();

                foreach (RepeaterItem item in rp_Pickups.Items)
                {
                    DropDownList dd_status = (DropDownList)item.FindControl("dd_status");
                    DataTable statusX = statuses.Copy();

                    dd_status.DataTextField = "StatusDescription";
                    dd_status.DataValueField = "StatusCode";
                    dd_status.DataSource = statusX;
                    dd_status.DataBind();

                    HiddenField hd_pickup_status = (HiddenField)item.FindControl("hd_pickup_status");

                    dd_status.SelectedValue = hd_pickup_status.Value;
                }
            }
            else
            {
                rp_Pickups.DataSource = null;
                rp_Pickups.DataBind();
                Errorid.Text = "No Pickups found for entered Rider!";
                return;
            }
        }
        protected void rp_Pickups_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Update")
            {
                HiddenField hd_id = (HiddenField)e.Item.FindControl("hd_id");
                HiddenField hd_riderCode = (HiddenField)e.Item.FindControl("hd_riderCode");
                HiddenField hd_riderPhone = (HiddenField)e.Item.FindControl("hd_riderPhone");
                DropDownList dd_status = (DropDownList)e.Item.FindControl("dd_status");

                TextBox txt_remarks = (TextBox)e.Item.FindControl("txt_remarks");

                string id = hd_id.Value;
                string riderCode = hd_riderCode.Value;
                string riderPhone = hd_riderPhone.Value;
                string statusValue = dd_status.SelectedValue;
                string remarks = txt_remarks.Text;

                int updater = Update_Status(id, riderCode, riderPhone, statusValue, remarks);

                if (updater == 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Notify", "alert('Notification : Status Updated for Rider: " + riderCode + "');", true);
                    //Errorid.Text = "Status Updated for Rider: " + riderCode;

                }
                else
                {
                    Errorid.Text = "Error Updating Status!";
                }


            }
        }
    }
}