using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using MRaabta.App_Code;
using System.Configuration;

namespace MRaabta.Files
{
    public partial class VMMDpoExpenseLog : System.Web.UI.Page
    {
        bool edit = false;
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        bool check = true;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (HttpContext.Current.Session["U_NAME"] == null)
            {

                Response.Redirect("~/login");

            }

            if (!IsPostBack)
            {

                logdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                deprtdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                ArvlDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                RadTimePicker1.SelectedTime = TimeSpan.Parse((DateTime.Now.ToString("hh:mm:ss")));
                RadTimePicker2.SelectedTime = TimeSpan.Parse((DateTime.Now.ToString("hh:mm:ss")));
                //RadTimePicker1.Text = DateTime.Now.ToString("hh:mm:ss");
                //RadTimePicker2.Text = DateTime.Now.ToString("hh:mm:ss");

                if (check)
                {

                    GetProducts();
                    // GetVehicle_type();
                    //GetRiders();
                    //GetDriver(); //Alternate Riders
                    // GetVehicle();
                    Showedit();
                    GetZone();
                    GetRoutes();
                    Get_Operlocation();
                    btn_Submit.Visible = true;
                    btn_update.Visible = false;
                }

            }

            if (HttpContext.Current.Session["EDIT"] == "T")
            {

                string log_no = Session["Logno"].ToString();
                //  GetRiders();
                GetVehicle_type();
                GetDriver(); //Alternate Riders

                GetZone();

                //   GetRoutes();
                Get_Operlocation();
                GetProducts();
                //ddl_zone_SelectedIndexChanged(this, e);
                //dd_br_SelectedIndexChanged(this, e);
                Edit();

                btn_Submit.Visible = false;
                btn_update.Visible = true;
                gv_edit.Visible = false;
                Session["EDIT"] = null;
            }



        }


        protected void GetProducts()
        {

            string query = "select distinct(products) from servicetypes_new";

            DataSet ds = new DataSet();
            try
            {

                con.Open();
                SqlCommand orcd = new SqlCommand(query, con);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {

                    ddl_products.DataTextField = "products";
                    ddl_products.DataValueField = "products";
                    ddl_products.DataSource = ds.Tables[0].DefaultView;
                    ddl_products.DataBind();
                }
                ddl_products.Items.Insert(0, new ListItem("SELECT PRODUCT", "0"));
            }
            catch (Exception Err)
            { }
            finally
            { con.Close(); }


        }


        protected void GetVehicle_type()
        {

            string query = "SELECT typeid,typedesc FROM Vehicle_Type";

            DataSet ds = new DataSet();
            try
            {

                con.Open();
                SqlCommand orcd = new SqlCommand(query, con);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {

                    dd_vtype.DataTextField = "typedesc";
                    dd_vtype.DataValueField = "typeid";
                    dd_vtype.DataSource = ds.Tables[0].DefaultView;
                    dd_vtype.DataBind();
                }
                dd_vtype.Items.Insert(0, new ListItem("SELECT VEHICLE TYPE", "0"));
            }
            catch (Exception Err)
            { }
            finally
            { con.Close(); }


        }


        protected void Get_Operlocation()
        {
            string query = "select DISTINCT op.Shortname as location,op.locationid  from mnp_operationlocations op";

            DataSet ds = new DataSet();
            try
            {

                con.Open();
                SqlCommand orcd = new SqlCommand(query, con);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddl_oper_loc.DataTextField = "location";
                    ddl_oper_loc.DataValueField = "locationid";
                    ddl_oper_loc.DataSource = ds.Tables[0].DefaultView;
                    ddl_oper_loc.DataBind();
                }
                ddl_oper_loc.SelectedValue = "10002";
                // ddl_oper_loc.Items.Insert(0, new ListItem("SELECT OPER LOCATION", "0"));
            }
            catch (Exception Err)
            { }
            finally
            { con.Close(); }



        }

        protected void GetZone()
        {

            string query = "SELECT DISTINCT z.zoneCode,\n" +
                      "z.name as zonename\n" +
               "FROM   Zones z\n" +
                      "INNER JOIN Branches b\n" +
                           "ON b.zoneCode = z.zoneCode\n" +
                           "AND b.[status] = '1'\n" +
               "WHERE z.[status] = '1' \n" +
               "ORDER BY z.name asc \n";

            DataSet ds = new DataSet();



            try
            {
                con.Open();
                SqlCommand orcd = new SqlCommand(query, con);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddl_zone.Items.Insert(0, new ListItem(" Select Zone", "0"));
                    ddl_zone.DataTextField = "zonename";
                    ddl_zone.DataValueField = "zoneCode";
                    ddl_zone.DataSource = ds.Tables[0].DefaultView;
                    ddl_zone.DataBind();
                }

                ddl_zone.Items.Insert(0, new ListItem("SELECT ZONE", "0"));
            }
            catch (Exception Err)
            { }
            finally
            { con.Close(); }


        }

        protected void GetRoutes()
        {
            //string query = "select DISTINCT name as RouteName,movementrouteid\n" +
            //               "from rvdbo.movementroute where originbranchid = '" + HttpContext.Current.Session["BRANCHCODE"].ToString() + "'\n" +
            //               "and isactive = '1' and isdeleted = '0' ";
            string query = "select DISTINCT name as RouteName,movementrouteid\n" +
                           "from rvdbo.movementroute where originbranchid = '" + dd_br.SelectedValue + "'\n" +
                           "and isactive = '1' and isdeleted = '0' ORDER BY name asc ";

            //   string query = "select r.routecode + ' - '+ r.name as routedesc,routecode  from rvdbo.route r";

            DataSet ds = new DataSet();


            try
            {
                con.Open();
                SqlCommand orcd = new SqlCommand(query, con);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddl_routes.DataTextField = "RouteName";
                    ddl_routes.DataValueField = "movementrouteid";
                    ddl_routes.DataSource = ds.Tables[0].DefaultView;
                    ddl_routes.DataBind();

                }
                else
                {
                    ddl_routes.Items.Clear();
                    ddl_routes.Items.Insert(0, new ListItem("SELECT ROUTE", "0"));
                }
                // ddl_routes.Items.Insert(0, new ListItem("SELECT ROUTE", "0"));

            }
            catch (Exception Err)
            { }
            finally
            { con.Close(); }


        }

        protected void GetRiders()//First One
        {
            Errorid.Text = "";
            DataSet ds = new DataSet();

            dd_DSR.Items.Clear();

            ds = Get_RidersListByID(ddl_zone.SelectedValue);

            if (ds.Tables[0].Rows.Count != 0)
            {
                dd_DSR.DataTextField = "NAME";
                dd_DSR.DataValueField = "riderCode";
                dd_DSR.DataSource = ds.Tables[0].DefaultView;
                dd_DSR.DataBind();
            }
            else
            {
                ds = null;
                Errorid.Text = "No Driver Available.";
            }
            dd_DSR.Items.Insert(0, new ListItem("SELECT DRIVER", "0"));
        }
        protected void GetDriver() //Second One
        {
            Errorid.Text = "";
            DataSet ds = new DataSet();
            ds = Get_RidersListByID(ddl_zone.SelectedValue);

            if (ds.Tables[0].Rows.Count != 0)
            {
                dd_driver.DataTextField = "NAME";
                dd_driver.DataValueField = "riderCode";
                dd_driver.DataSource = ds.Tables[0].DefaultView;
                dd_driver.DataBind();
            }
            else
            {
                ds = null;
                Errorid.Text = "No Rider Present.";
            }
            dd_driver.Items.Insert(0, new ListItem("SELECT DRIVER", "0"));
        }
        protected void GetVehicle(string zone, string vt)
        {
            Errorid.Text = "";
            DataSet ds = new DataSet();
            ds = Get_VehicleListByID(zone, vt);

            if (ds.Tables[0].Rows.Count != 0)
            {
                dd_van.DataTextField = "NAME";
                dd_van.DataValueField = "id";
                dd_van.DataSource = ds.Tables[0].DefaultView;
                dd_van.DataBind();
            }
            else
            {
                ds = null;
                dd_van.DataSource = ds;
                dd_van.DataBind();
                Errorid.Text = "No Vehicle Present.";
                dd_van.Items.Clear();
            }
            dd_van.Items.Insert(0, new ListItem("SELECT VEHICLE", "0"));
        }

        public DataSet Get_VehicleListByID(string zone, string vt)
        {

            // string query = "SELECT v.id, v.NAME FROM Vehicle v where v.status = '1'";
            string query = "select * from vehicle where zonecode='" + zone + "' and vehiclemaintain='" + vt + "' and status='1' ORDER BY NAME ASC";
            DataSet ds = new DataSet();
            try
            {
                con.Open();
                SqlCommand orcd = new SqlCommand(query, con);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                con.Close();
            }
            catch (Exception Err)
            { }
            finally
            { con.Close(); }
            return ds;
        }
        public DataSet Get_RidersListByID(string zone)
        {
            string query = "";

            string query__old = "select r.riderCode,(r.riderCode+'-'+r.firstName) AS NAME\n" +
       "FROM Riders r WHERE r.branchId='" + HttpContext.Current.Session["BRANCHCODE"].ToString() + "'  and status = '1' and zoneid='" + zone + "'  ORDER BY r.riderCode";//and usertypeid ='230'

            query = "select r.riderCode,(r.riderCode+'-'+r.firstName) AS NAME\n" +
       "FROM Riders r WHERE status = '1' and RIDER_TYPE = '230' and zoneid='" + zone + "'  ORDER BY r.riderCode";//and usertypeid ='230'


            DataSet ds = new DataSet();
            try
            {
                con.Open();
                SqlCommand orcd = new SqlCommand(query, con);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                con.Close();
            }
            catch (Exception Err)
            { }
            finally
            { con.Close(); }
            return ds;
        }

        public void Edit()
        {

            string query = "select vmm.log_date,vmm.driver1,vmm.driver2,vmm.vehicle_type,vmm.vehicle,\n" +
            "vmm.opn_reading,vmm.cur_reading,vmm.ddate,vmm.dtime,vmm.adate,vmm.atime,vmm.remarks,\n" +
            "vmm.helper1,vmm.helper2,vmm.enter_user,vmm.zone,vmm.gc,vmm.routes,vmm.operation_location,vmm.natureofduty,vmm.products,vmm.fuel_type,vmm.fuel_amount,vmm.branch,vmm.kilometer\n" +
            "from vmm_vehicle_log_master vmm where log_no = '" + HttpContext.Current.Session["Logno"].ToString() + "'";

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(query, con);
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                ddl_products.SelectedValue = dt.Rows[0]["products"].ToString();
                txt_helper_1.Text = dt.Rows[0]["helper1"].ToString();
                txt_helper_2.Text = dt.Rows[0]["helper2"].ToString();
                txt_mtr.Text = dt.Rows[0]["opn_reading"].ToString();
                txt_mtr2.Text = dt.Rows[0]["cur_reading"].ToString();
                txt_remrks.Text = dt.Rows[0]["remarks"].ToString();
                dd_DSR.SelectedValue = dt.Rows[0]["driver1"].ToString().TrimStart();

                dd_vtype.SelectedValue = dt.Rows[0]["vehicle_type"].ToString();
                dd_driver.SelectedValue = dt.Rows[0]["driver2"].ToString().TrimStart();
                //dd_ItemLevel1.SelectedValue = dt.Rows[0]["opn_reading"].ToString();
                RadTimePicker2.SelectedTime = TimeSpan.Parse(dt.Rows[0]["atime"].ToString());
                RadTimePicker1.SelectedTime = TimeSpan.Parse(dt.Rows[0]["dtime"].ToString());
                //RadTimePicker2.Text = dt.Rows[0]["atime"].ToString();
                //RadTimePicker1.Text = dt.Rows[0]["dtime"].ToString();
                deprtdate.Text = DateTime.Parse(dt.Rows[0]["ddate"].ToString()).ToString("dd/MM/yyyy");
                ArvlDate.Text = DateTime.Parse(dt.Rows[0]["adate"].ToString()).ToString("dd/MM/yyyy");
                logdate.Text = DateTime.Parse(dt.Rows[0]["log_date"].ToString()).ToString("dd/MM/yyyy");
                ddl_zone.SelectedValue = dt.Rows[0]["zone"].ToString();
                txt_gc.Text = dt.Rows[0]["GC"].ToString();
                txt_km.Text = dt.Rows[0]["kilometer"].ToString();
                ddl_oper_loc.SelectedValue = dt.Rows[0]["operation_location"].ToString();
                ddl_natureofduty.SelectedValue = dt.Rows[0]["natureofduty"].ToString();
                ddl_fueltype.SelectedValue = dt.Rows[0]["fuel_type"].ToString();
                txt_fuel_amt.Text = dt.Rows[0]["fuel_amount"].ToString();
                GetVehicle(ddl_zone.SelectedValue, V_type.SelectedValue);
                dd_van.SelectedValue = dt.Rows[0]["vehicle"].ToString();
                getbranch(ddl_zone.SelectedValue);
                GetRiders();
                dd_br.SelectedValue = dt.Rows[0]["branch"].ToString();
                GetRoutes();
                ddl_routes.SelectedValue = dt.Rows[0]["routes"].ToString();

                check = false;
            }

        }

        public void clearall()
        {
            txt_fuel_amt.Text = txt_helper_1.Text = txt_mtr.Text = txt_helper_2.Text = txt_mtr2.Text = txt_remrks.Text = "";
        }

        protected void btn_Submit_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> queries = new List<string>();

                // DateTime time1 = DateTime.Parse(string.Format("{0}:{1}:{2} {3}", TimeSelector1.Hour, TimeSelector1.Minute, TimeSelector1.Second, TimeSelector1.AmPm));
                //DateTime time2 = DateTime.Parse(string.Format("{0}:{1}:{2} {3}", TimeSelector2.Hour, TimeSelector2.Minute, TimeSelector2.Second, TimeSelector2.AmPm));

                string time1 = RadTimePicker1.SelectedTime.ToString();
                string time2 = RadTimePicker2.SelectedTime.ToString();
                //string time1 = RadTimePicker1.Text.ToString();
                //string time2 = RadTimePicker2.Text.ToString();
                DataSet ds = new DataSet();
                ds = dsslog();
                string logNo = "";
                if (ds.Tables[0].Rows[0][0] == null)
                {
                    logNo = "1";
                }
                else
                {
                    string no = Convert.ToString(ds.Tables[0].Rows[0][0]);
                    if (no == "")
                    {
                        no = "1";
                    }
                    //   int num = int.Parse(no) + 1;
                    logNo = no.ToString();
                }

                string Remarx = txt_remrks.Text.Replace("'", "");

                DateTime log_d = DateTime.ParseExact(logdate.Text, "dd/MM/yyyy", null);
                DateTime Dep_d = DateTime.ParseExact(deprtdate.Text, "dd/MM/yyyy", null);
                DateTime Arr_d = DateTime.ParseExact(ArvlDate.Text, "dd/MM/yyyy", null);
                //DateTime log_d = DateTime.Parse(logdate.Text);
                string Logdate = log_d.ToString("yyyy-MM-dd");
                string Depdate = Dep_d.ToString("yyyy-MM-dd");
                string Arrdate = Arr_d.ToString("yyyy-MM-dd");

                string insert = "insert into vmm_vehicle_log_master\n" +
                "(log_no,\n" +
                "working_date,\n" +
                "log_date,\n" +
                "driver1,\n" +
                "driver2,\n" +
                "vehicle_type,\n" +
                "vehicle,\n" +
                  "vehiclemaintain,\n" +
                "opn_reading,\n" +
                "cur_reading,\n" +
                "ddate,\n" +
                "dtime,\n" +
                "adate,\n" +
                "atime,\n" +
                "remarks,\n" +
                "zone,\n" +
                "GC,\n" +
                "routes,\n" +
                // "division,\n" +
                //   "alt_dsr,\n" +
                "helper1,\n" +
                "helper2,\n" +
                "enter_user,\n" +
                "branch,\n" +
                     "products,\n" +
                "operation_location,\n" +
                "natureofduty,\n" +
                            "fuel_type,\n" +
                "fuel_amount,kilometer\n" +
                         ")\n" +
                "values\n" +
                " ('" + logNo + "',\n" +
                "'" + DateTime.Now.ToString("yyyy-MM-dd") + "',\n" +
                "'" + Logdate + "',\n" +
                "'" + dd_DSR.SelectedValue + "',\n" +
                "'" + dd_driver.SelectedValue + "',\n" +
                "'" + dd_vtype.SelectedValue + "',\n" +
                "'" + dd_van.SelectedValue + "',\n" +
                "'" + V_type.SelectedValue + "',\n" +
                 //if (V_type.SelectedValue.ToString() == "03")
                 //{

                 //    insert += "'" + OutsVan.Text.Replace("'", "") + "',\n";
                 //}
                 //else
                 //{

                 //    insert += "'" + dd_van.SelectedValue + "',\n";

                 //}

                 "'" + txt_mtr.Text.Replace("'", "") + "',\n" +
                        "'" + txt_mtr2.Text.Replace("'", "") + "',\n" +
                        "'" + Depdate + "',\n" +
                        "'" + time1.ToString() + "',\n" +
                        "'" + Arrdate + "',\n" +
                        "'" + time2.ToString() + "',\n" +
                        "'" + Remarx + "',\n" +
                        "'" + ddl_zone.SelectedValue + "',\n" +
                        "'" + txt_gc.Text + "',\n" +
                        "'" + ddl_routes.SelectedValue + "',\n" +
                        "'" + txt_helper_1.Text.Replace("'", "") + "',\n" +
                        "'" + txt_helper_2.Text.Replace("'", "") + "',\n" +
                        "'" + HttpContext.Current.Session["U_NAME"].ToString() + "',\n" +
                        "" + dd_br.SelectedValue + ",\n" +
                        "'" + ddl_products.SelectedValue + "',\n" +
                        "" + ddl_oper_loc.SelectedValue + ",\n" +
                        "" + ddl_natureofduty.SelectedValue + ",\n" +
                        "" + ddl_fueltype.SelectedValue + ",\n" +
                        "" + txt_fuel_amt.Text + ",\n" +
                         "" + txt_km.Text + "\n" +
                        ")";

                queries.Add(insert);

                //string query = "insert into vmm_vehicle_log_detail\n" +
                //                "(company,\n" +
                //                "distributor,\n" +
                //                "log_no,\n" +
                //                "document,\n" +
                //                "sub_document,\n" +
                //                "doc_no,\n" +
                //                "doc_date,\n" +
                //                "net_amount\n" +
                //                ")\n" +
                //                "values (\n" +
                //                "'01','1',\n" +
                //                "'" + logNo + "',\n" +
                //                "'GN',\n" +
                //                "'01',\n" +
                //                "'9999',\n" +
                //                "'" + DateTime.Now.ToString("yyyy-MM-dd") + "',\n" +
                //                "'9000'\n" +
                //                ")";
                //queries.Add(query);


                int j = PostCollectionNew(queries);
                clearall();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
        public DataSet dsslog()
        {
            string logn0 = "select distinct max(log_no + 1) as max_num from vmm_vehicle_log_master";
            DataSet ds = new DataSet();
            try
            {
                con.Open();
                SqlCommand orcd = new SqlCommand(logn0, con);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                con.Close();
            }
            catch (Exception Err)
            {

            }
            finally
            { con.Close(); }
            return ds;
        }
        protected int PostCollectionNew(List<string> queries)
        {
            Errorid.Text = "";
            int status = 0;
            string query = "";
            int check_ = 0;

            con.Open();
            SqlCommand command = con.CreateCommand();
            SqlTransaction transaction;
            transaction = con.BeginTransaction();
            command.Transaction = transaction;
            Boolean flag = false;
            try
            {
                int statusFinal = 0;
                foreach (string list in queries)
                {
                    command.CommandText = list;
                    statusFinal = command.ExecuteNonQuery();
                }

                if (statusFinal > 0)
                {

                    flag = true;

                }
                else
                {

                    Errorid.Text = "Error In Vehicle log creation";
                }

                if (flag == true)
                {

                    transaction.Commit();
                    status = 1;
                    btn_Submit.Enabled = false;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Messagebox", "alert('Vehicle log has been created!'); window.location('VMMdpoExpenseLog.aspx');", true);

                }
                else
                {
                    transaction.Rollback();
                    Errorid.Text = "Error In Vehicle log creation";
                    status = 0;
                }
            }
            catch (Exception ex)
            {
                Errorid.Text = "" + ex.ToString();
            }
            finally
            {
                con.Close();
            }

            return status;
            //  }

        }

        private string DateFormat(string methDate)
        {
            string mdate = DateTime.ParseExact(methDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture);
            string[] mmarr = mdate.Split('/', ' ');
            return string.Concat(mmarr);
        }

        public void Showedit()
        {
            string sql = "SELECT convert(varchar,convert(datetime,vvlm.working_date,105),103) TODAY_DATE,\n" +
                            "vvlm.log_no LOG_NO, convert(varchar,convert(datetime,vvlm.log_date,105),103)LOG_DATE,r.firstName driver1,r.firstName driver2\n" +
                            "FROM vmm_vehicle_log_master vvlm\n" +
                            "JOIN Riders r\n" +
                            "ON vvlm.driver1 = r.riderCode\n" +
                            "AND vvlm.branch = r.branchid\n" +
                            "--JOIN Riders rr\n" +
                            "--ON vvlm.driver2 = rr.riderCode\n" +
                            "--AND vvlm.branch = rr.branchid\n" +
                            "WHERE vvlm.working_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'\n" +
                            "order by vvlm.log_no";


            DataTable dt = new DataTable();
            try
            {
                con.Open();
                SqlCommand orcd = new SqlCommand(sql, con);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    dv_edit.Visible = true;
                    gv_edit.DataSource = dt;
                    gv_edit.DataBind();

                }
                else
                {
                    dv_edit.Visible = false;
                    ViewState["dtTableInternal"] = null;
                    gv_edit.DataSource = ViewState["dtTableInternal"] as DataTable;
                    gv_edit.DataBind();


                }

                con.Close();
            }
            catch (Exception Err)
            {

            }
            finally
            { con.Close(); }
        }

        protected void btn_update_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> queries = new List<string>();

                string time1 = RadTimePicker1.SelectedTime.ToString();
                string time2 = RadTimePicker2.SelectedTime.ToString();
                //string time1 = RadTimePicker1.Text.ToString();
                //string time2 = RadTimePicker2.Text.ToString();

                DateTime log_d = DateTime.ParseExact(logdate.Text, "dd/MM/yyyy", null);
                DateTime Dep_d = DateTime.ParseExact(deprtdate.Text, "dd/MM/yyyy", null);
                DateTime Arr_d = DateTime.ParseExact(ArvlDate.Text, "dd/MM/yyyy", null);
                //DateTime log_d = DateTime.Parse(logdate.Text);
                string Logdate = log_d.ToString("yyyy-MM-dd");
                string Depdate = Dep_d.ToString("yyyy-MM-dd");
                string Arrdate = Arr_d.ToString("yyyy-MM-dd");

                string Remarx = txt_remrks.Text.Replace("'", "");
                string van = "";

                //if (V_type.SelectedValue.ToString() == "03")
                //{

                //    van = OutsVan.Text.Replace("'", "");
                //}
                //else
                //{

                //    van = dd_van.SelectedValue;

                //}


                string update = "update vmm_vehicle_log_master set\n" +
                "Log_Date = '" + Logdate + "',\n" +
                "driver1='" + dd_DSR.SelectedValue + "',\n" +
                "driver2='" + dd_driver.SelectedValue + "',\n" +
                "vehicle_type='" + dd_vtype.SelectedValue + "',\n" +
                "vehicle='" + van + "',\n" +
                 "vehiclemaintain='" + V_type.SelectedValue + "',\n" +
                "opn_reading='" + txt_mtr.Text + "',\n" +
                "cur_reading='" + txt_mtr2.Text + "',\n" +
                "ddate='" + Depdate + "',\n" +
                "dtime='" + time1 + "',\n" +
                "adate='" + Arrdate + "',\n" +
                "atime='" + time2 + "',\n" +
                "remarks='" + Remarx + "',\n" +
                "helper1='" + txt_helper_1.Text + "',\n" +
                "helper2='" + txt_helper_2.Text + "',\n" +
                "enter_user='" + HttpContext.Current.Session["U_NAME"].ToString() + "',\n" +
                "zone='" + ddl_zone.SelectedValue + "',\n" +
                "gc='" + txt_gc.Text + "',\n" +
                "branch='" + dd_br.SelectedValue + "',\n" +
                "routes='" + ddl_routes.SelectedValue + "',\n" +
                "products='" + ddl_products.SelectedValue + "',\n" +
                "operation_location='" + ddl_oper_loc.SelectedValue + "',\n" +
                "natureofduty='" + ddl_natureofduty.SelectedValue + "',\n" +
                "fuel_type='" + ddl_fueltype.SelectedValue + "',\n" +
                "fuel_amount='" + txt_fuel_amt.Text + "',\n" +
                "kilometer='" + txt_km.Text + "'\n" +
                "where log_no= '" + HttpContext.Current.Session["Logno"].ToString() + "'";

                queries.Add(update);


                int j = PostCollectionNew(queries);
                if (j == 1)
                {
                    check = true;
                    Session["EDIT"] = null;
                    Session["Logno"] = null;
                    clearall();
                    Response.Redirect("VMMDpoExpenseLog.aspx");

                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void gv_data_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EDIT")
            {
                string log_no = e.CommandArgument.ToString();
                if (!string.IsNullOrEmpty(log_no))
                {

                    Session["Logno"] = log_no;
                    Session["EDIT"] = "T";
                    Response.Redirect("VMMDpoExpenseLog.aspx");

                }
            }
        }
        protected void getbranch(string zone)
        {
            string sql = "select * from branches  where zonecode='" + zone + "' and   status='1' order by name";
            //SqlConnection con = new SqlConnection(clvar.Strcon());
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
                dd_br.DataValueField = "branchcode";
                dd_br.DataBind();
                dd_br.Items.Insert(0, new ListItem("Select Branches", ""));
            }
        }
        protected void ddl_zone_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sql = "select * from branches  where zonecode='" + ddl_zone.SelectedValue + "' and status='1' order by name";
            //SqlConnection con = new SqlConnection(clvar.Strcon());
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
                dd_br.DataValueField = "branchcode";
                dd_br.DataBind();
                dd_br.Items.Insert(0, new ListItem("Select Branches", ""));
            }

            GetVehicle(ddl_zone.SelectedValue, V_type.SelectedValue);


            GetRiders();
        }
        protected void dd_br_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetRoutes();
        }
        protected void V_type_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetVehicle(ddl_zone.SelectedValue, V_type.SelectedValue);
        }

        protected void Vechicle_type_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetVehicle_type();

            string sql = "select VehicleType from vehicle WHERE [status] = '1' AND zoneCode = '" + ddl_zone.SelectedValue + "' and id = '" + dd_van.SelectedValue + "' ";
            //SqlConnection con = new SqlConnection(clvar.Strcon());
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            con.Open();
            adp.Fill(ds);
            con.Close();
            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    dd_br.DataSource = ds.Tables[0];
            //    dd_br.DataTextField = "VehicleType";
            //    dd_br.DataValueField = "VehicleType";
            //    dd_br.DataBind();
            //}


            ListItem selectedListItem = dd_vtype.Items.FindByValue(ds.Tables[0].Rows[0][0].ToString());

            if (selectedListItem != null)
            {
                selectedListItem.Selected = true;
            }
        }
    }
}