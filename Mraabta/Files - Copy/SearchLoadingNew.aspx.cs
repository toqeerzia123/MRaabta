using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using MRaabta.App_Code;
using System.Globalization;

namespace MRaabta.Files
{
    public partial class SearchLoadingNew : System.Web.UI.Page
    {
        Variable clvar = new Variable();
        LoadingPrintReport b_fun = new LoadingPrintReport();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txt_date.Text = DateTime.Now.ToString("dd-MM-yyyy");
                Get_MasterTransportType();
                GetDestinations();
            }
        }

        public void Get_MasterTransportType()
        {
            DataSet ds_transporttype = b_fun.Get_MasterTransportType(clvar);

            if (ds_transporttype.Tables[0].Rows.Count != 0)
            {
                dd_transportType.DataTextField = "AttributeDesc";
                dd_transportType.DataValueField = "id";
                dd_transportType.DataSource = ds_transporttype.Tables[0].DefaultView;
                dd_transportType.DataBind();
            }
            //dd_transportType.Items.Insert(0, new ListItem("Select Transport Type ", ""));
        }
        public void GetDestinations()
        {
            DataTable ds_destination = Cities_();// b_fun.Get_MasterDestination(clvar);
            if (ds_destination.Rows.Count != 0)
            {
                dd_destination.DataTextField = "BranchName";
                dd_destination.DataValueField = "branchCode";
                dd_destination.DataSource = ds_destination;
                dd_destination.DataBind();
                //ViewState["destinations"] = ds_destination;

            }
        }
        protected void Btn_Save_Click(object sender, EventArgs e)
        {
            if (txt_loading.Text.Trim() != "" || txt_date.Text.Trim() != "" || txt_sealNo.Text.Trim() != "" || dd_destination.SelectedValue != "0" || dd_transportType.SelectedValue != "0")
            {
                clvar.LoadingId = txt_loading.Text;
                clvar._StartDate = txt_date.Text;
                if (txt_date.Text.Trim() != "")
                {
                    clvar.Check_Condition += " AND CAST(l.date as DATE) = '" + DateTime.ParseExact(txt_date.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd") + "'\n";
                }
                if (txt_loading.Text != "")
                {
                    clvar.Check_Condition += " AND l.ID = '" + txt_loading.Text.Trim() + "'\n";
                }
                if (txt_sealNo.Text.Trim() != "")
                {
                    if (txt_date.Text.Trim() == "")
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Select Date')", true);
                        return;
                    }
                    clvar.Check_Condition += " AND l.sealno = '" + txt_sealNo.Text + "'\n";
                }
                if (dd_destination.SelectedValue != "0")
                {
                    if (txt_date.Text.Trim() == "")
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Select Date')", true);
                        return;
                    }
                    clvar.Check_Condition += " AND l.destination = '" + dd_destination.SelectedValue + "'\n";
                }
                if (dd_transportType.SelectedValue != "0")
                {
                    if (txt_date.Text.Trim() == "")
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Select Date')", true);
                        return;
                    }
                    clvar.Check_Condition += " AND l.TransportationType = '" + dd_transportType.SelectedValue + "'\n";
                }
                if (cb_Select.Checked == true)
                {
                    Session["Destination"] = dd_destination.SelectedValue;
                }

                DataSet ds = Get_SearchLoading(clvar);

                if (ds.Tables[0].Rows.Count != 0)
                {
                    GridView.DataSource = ds.Tables[0].DefaultView;
                    GridView.DataBind();
                }
                else
                {
                    GridView.DataSource = null;
                    GridView.DataBind();
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('No Records Found')", true);
                    return;
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter/Select at least one criteria')", true);

                return;
            }
        }

        public DataSet Get_SearchLoading(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                var sql = "select l.id,\n" +
                  "       lu.AttributeDesc TransportType,\n" +
                  "       CONVERT(VARCHAR(10), l.date, 105) date,\n" +
                  "       v.MakeModel + ' (' + v.Description + ')' VehicleName,\n" +
                  "       l.courierName,\n" +
                  "       b1.name OrgName,\n" +
                  "       b2.name DestName,\n" +
                  "       l.description,\n" +
                  "       l.flightNo,\n" +
                  "       l.sealno,\n" +
                  "       l.departureflightdate,\n" +
                  "       ((SELECT ISNULL(SUM(CAST(lb.BagWeight AS FLOAT)),0) FROM MnP_LoadingBag lb WHERE lb.loadingId  = l.id) + \n" +
                  "       (SELECT ISNULL(SUM(CAST(lb.CNWeight AS FLOAT)),0) FROM mnp_loadingConsignment lb WHERE lb.loadingId = l.id)) TotalWeight,\n" +
                  "       Case when isairport ='1' then 'Yes' else 'No' end AtAirport, \n" +
                  "       u.Name as CreatedBy" +
                  "  from mnp_Loading l\n" +
                  "  left outer join rvdbo.Lookup lu\n" +
                  "    on lu.id = l.transportationType\n" +
                  "   and lu.AttributeGroup = 'TRANSPORT_TYPE'\n" +
                  " left outer join rvdbo.Vehicle v\n" +
                  "    on v.VehicleCode = CAST(l.vehicleId AS VARCHAR)\n" +
                  " inner join Branches b1\n" +
                  "    on b1.branchCode = l.origin\n" +
                  " inner join Branches b2\n" +
                  "    on b2.branchCode = l.destination\n" +
                  " inner join ZNI_USER1 u on u.u_id = l.createdBy\n" +
                  " WHERE l.branchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "' " + clvar.Check_Condition + "\n";

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

        public DataTable Cities_()
        {

            string sqlString = "SELECT CAST(A.bid as Int) bid, A.description NAME, A.expressCenterCode, A.CategoryID\n" +
            "  FROM (SELECT e.expresscentercode, e.bid, e.description, e.CategoryId\n" +
            "          FROM ServiceArea a\n" +
            "         right outer JOIN ExpressCenters e\n" +
            "            ON e.expressCenterCode = a.Ec_code where e.bid <> '17' and e.status = '1') A\n" +
            " GROUP BY A.bid, A.description, A.expressCenterCode, A.CategoryID\n" +
            " ORDER BY 2";

            sqlString = "";

            sqlString = "select b.branchCode , b.sname + ' - ' + b.name BranchName, b.branchCode\n" +
            "  from branches b\n" +
            " where b.status = '1'\n" +
            " order by 2";

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sqlString, con);
                sda.Fill(dt);

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return dt;
        }
    }
}