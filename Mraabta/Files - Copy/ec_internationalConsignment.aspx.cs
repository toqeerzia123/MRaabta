using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Telerik.Web.UI;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Text;
using MRaabta.App_Code;

namespace MRaabta.Files
{
    public partial class ec_internationalConsignment : System.Web.UI.Page
    {
        CommonFunction com = new CommonFunction();
        Cl_Variables clvar = new Cl_Variables();
        Consignemnts con = new Consignemnts();
        bayer_Function b_fun = new bayer_Function();
        Function Fun = new Function();
        Variable Vclvar = new Variable();
        bool isValid = true;
        string message = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["BranchCode"] == null)
            {
                Response.Redirect("~/login");
            }
            //   txt_bookingDate.Text = DateTime.Now.ToShortDateString();//.ToString("yyyy-MM-dd");

            txt_approve_status.Enabled = false;
            txt_inv_status.Enabled = false;
            txt_invoice.Enabled = false;
            //   btn_update.Enabled = false;
            lbl_error.Text = "";
            if (!IsPostBack)
            {
                txt_bookingDate.Enabled = false;
                btn_save.Enabled = false;
                txt_consignmentNo.MaxLength = 11;
                txt_bookingDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                txt_reporting_date.Text = DateTime.Now.ToString("yyyy-MM-dd");
                GetCompanies();
                dd_origin.Items.Add(new ListItem("KARACHI", "4"));
                dd_origin.Enabled = false;
                if (ViewState["riderCode"] != null)
                {
                    txt_riderCode.Text = ViewState["riderCode"] as string;
                }

                DataTable dtSurcharges = new DataTable();
                DataColumn dc = new DataColumn("ID", typeof(int));
                dtSurcharges.Columns.Add(dc);
                dc = new DataColumn("Pname", typeof(string));
                dtSurcharges.Columns.Add(dc);
                dc = new DataColumn("PValue", typeof(double));
                dtSurcharges.Columns.Add(dc);
                dc = new DataColumn("ActualValue", typeof(double));
                dtSurcharges.Columns.Add(dc);
                dc = new DataColumn("Base", typeof(string));
                dtSurcharges.Columns.Add(dc);
                dc = new DataColumn("Description", typeof(string));
                dtSurcharges.Columns.Add(dc);
                dtSurcharges.AcceptChanges();
                ViewState["dt"] = dtSurcharges;
                GetCountries();
                GetOrigin();
                GetDestination();
                GetConsignmentType();
                GetServiceType();
                GetFuelSurcharges();
                hd_creditClientID.Value = "0";
                Get_Paymentsource();
            }
        }

        public DataSet PaymentSource()
        {
            DataSet Ds_1 = new DataSet();

            try
            {
                string query = "Select id,Name from Paymentsource where booking ='1' and status ='1'";
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
        public void Get_Paymentsource()
        {
            DataSet ds = PaymentSource();
            if (ds.Tables.Count != 0)
            {
                //  cb_Destination

                //   this.dd_PaymentMode.Items.Add(new ListItem { Text = "Select Payment Source", Value = "0" });
                this.dd_PaymentMode.DataTextField = "Name";
                this.dd_PaymentMode.DataValueField = "id";
                this.dd_PaymentMode.DataSource = ds.Tables[0].DefaultView;
                this.dd_PaymentMode.DataBind();

                this.dd_PaymentMode.SelectedValue = "0";

            }

        }
        protected void GetCompanies()
        {
            DataTable dt = com.Companies();
            rbtn_consignmentSender.DataSource = dt;
            rbtn_consignmentSender.DataTextField = "COMPANYNAME";
            rbtn_consignmentSender.DataValueField = "ID";
            rbtn_consignmentSender.DataBind();
            rbtn_consignmentSender.SelectedValue = "1";
        }

        protected void GetCountries()
        {
            string query = "SELECT * FROM country order by name";
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString_"].ConnectionString);
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    dd_destCountry.DataSource = dt;
                    dd_destCountry.DataTextField = "NAME";
                    dd_destCountry.DataValueField = "CODE";
                    dd_destCountry.DataBind();
                    dd_destCountry.SelectedValue = "PK";
                }
            }
            catch (Exception ex)
            { }
            finally { con.Close(); }

        }
        protected void GetOrigin()
        {
            string query = "SELECT CAST(BRANCHCODE as varchar) + '-' + Cast(ZONECODE as varchar) as branchcode, NAME FROM BRANCHES WHERE STATUS = '1'";
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString_"].ConnectionString);
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    dd_origin.DataSource = dt;
                    dd_origin.DataTextField = "NAME";
                    dd_origin.DataValueField = "BRANCHCODE";
                    dd_origin.DataBind();
                    dd_origin.SelectedValue = Session["BranchCode"].ToString() + "-" + Session["ZoneCode"].ToString();
                    dd_origin.Enabled = false;
                }
            }
            catch (Exception ex)
            { }
            finally { con.Close(); }

        }
        protected void GetDestination()
        {
            string query = "SELECT BRANCHCODE, NAME FROM BRANCHES WHERE STATUS = '1'";
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString_"].ConnectionString);
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    dd_destination.DataSource = dt;
                    dd_destination.DataTextField = "NAME";
                    dd_destination.DataValueField = "BRANCHCODE";
                    dd_destination.DataBind();
                    dd_destination.SelectedValue = "4";
                    dd_destination.Enabled = false;
                }
            }
            catch (Exception ex)
            { }
            finally { con.Close(); }

        }
        protected void GetConsignmentType()
        {
            string sql = " SELECT ct.id, \n"
               + "        ct.name             ConsignmentType \n"
               + " FROM   ConsignmentType     ct \n"
               + " WHERE  ct.[status] = '1' \n"
               + "        AND ct.id NOT IN ('18', '19') \n"
               + " GROUP BY \n"
               + "        ct.name, \n"
               + "        ct.id";
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString_"].ConnectionString);
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sql, con);
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    dd_conType.DataSource = dt;
                    dd_conType.DataTextField = "ConsignmentType";
                    dd_conType.DataValueField = "id";
                    dd_conType.DataBind();
                }
            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
        }
        protected void GetServiceType()
        {
            string service = "";
            if (rbtn_consignmentSender.SelectedValue == "4")
            {
                service = "fe";
            }
            else if (rbtn_consignmentSender.SelectedValue == "1")
            {
                service = "int";
            }
            //string query = "select name, serviceTypeName FROM ServiceTypes where status = '1' and companyId = '1' and isfranchised = '1' and serviceTypeName like '" + service + "%' and IsIntl = '1'";
            // string query = "select name, serviceTypeName FROM ServiceTypes where status = '1' and companyId = '1' and name like '" + service + "%' and IsIntl = '1'";

            string query = @"SELECT 
                            upper(s.serviceTypeName) serviceTypeName 
                            FROM MnP_Customer_ServiceMap m
                            INNER JOIN ServiceTypes_New s ON m.ServiceTypeName = s.serviceTypeName 
                            INNER JOIN CreditClients cc ON cc.id = m.CreditClientID
                            WHERE cc.accountNo = '0' AND cc.branchCode = '"+ Session["BRANCHCODE"].ToString() + @"'
                            and s.serviceTypeName like '" + service + @"%' and isnull(IsIntl,'0') = '1'
                            ORDER BY 1 ASC
                            ";

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString_"].ConnectionString);
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    dd_serviceType.Items.Clear();
                    dd_serviceType.Items.Add(new ListItem { Value = "0", Text = "Select Service Type" });
                    dd_serviceType.DataSource = dt;
                    dd_serviceType.DataTextField = "serviceTypeName";
                    dd_serviceType.DataValueField = "serviceTypeName";
                    dd_serviceType.DataBind();
                }
                else
                {
                    dd_serviceType.Items.Clear();
                    dd_serviceType.Items.Add(new ListItem { Value = "0", Text = "Select Service Type" });
                }
            }
            catch (Exception ex)
            {
                dd_serviceType.Items.Clear();
                dd_serviceType.Items.Add(new ListItem { Value = "0", Text = "Select Service Type" });
            }
            finally { con.Close(); }
        }
        protected void GetFuelSurcharges()
        {
            //string query = "select id, name,  cast(id as varchar)+'-'+cast(calculationBase as varchar)+'-'+ CAST(calculationValue as varchar) as base FROM PriceModifiers where status = '1'";
            DataTable dt = new DataTable();

            //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            try
            {
                //con.Open();
                //SqlDataAdapter sda = new SqlDataAdapter(query, con);
                //sda.Fill(dt);
                DataSet ds = com.PriceModifiers();
                ViewState["PriceModifiers"] = ds.Tables[0];
                dt = new DataTable();
                dt.Columns.Add("ID", typeof(string));
                dt.Columns.Add("NAME", typeof(string));
                dt.Columns.Add("base", typeof(string));
                dt.AcceptChanges();
                int count = 0;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    DataRow dr_ = dt.NewRow();
                    dr_["ID"] = dr["ID"].ToString();
                    dr_["NAME"] = dr["PriceModifier"].ToString();
                    dr_["BASE"] = dr["ID"].ToString() + "-" + dr["calculationBase"].ToString() + "-" + dr["calculationValue"].ToString();
                    dt.Rows.Add(dr_);
                    dt.AcceptChanges();
                }
                if (dt.Rows.Count > 0)
                {
                    //dd_name.Items.Add(new ListItem { Text = "Select Modifier", Value = "0" });
                    dd_name.DataSource = dt;
                    dd_name.DataTextField = "name";
                    dd_name.DataValueField = "base";
                    dd_name.DataBind();

                }
            }
            catch (Exception ex)
            { }
            finally
            { //con.Close(); 
            }

        }

        protected void txt_consignmentNo_TextChanged(object sender, EventArgs e)
        {
            //btn_save.Visible = false;
            ////btn_Validate.Visible = true;
            ////clvar.Weight = float.Parse(txt_weight.Text);
            //if (txt_consignmentNo.Text.Trim(' ') != "")
            //{
            //    if (txt_consignmentNo.Text.ToCharArray().Count() != 11)
            //    {
            //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Invalid Consignment Number.')", true);
            //        txt_consignmentNo.Focus();

            //    }
            //}
            #region MyRegion
            //txt_accountNo.Text = "";
            //txt_b.Text = "0";
            ////  txt_bookingDate.Text = "";
            //txt_cellNo.Text = "";
            //txt_consignee.Text = "";
            //txt_consigneeAddress.Text = "";
            //txt_consigneeAttn.Text = "";
            //txt_consigner.Text = "";
            //txt_consignerAddress.Text = "";
            //txt_consignerCellNo.Text = "";
            //txt_consignerCNIC.Text = "";
            ////txt_consignmentNo.Text = "";
            //txt_cupon.Text = "0";
            //txt_declaredValue.Text = "0";
            //txt_description.Text = "";
            //txt_discount.Text = "0";
            //txt_h.Text = "0";
            //txt_insurance.Text = "";
            //txt_packageContent.Text = "";
            //txt_pieces.Text = "1";
            //txt_riderCode.Text = ""; chk_riderCode.Checked = false;
            //txt_value.Text = "0";
            //txt_w.Text = "0";
            //txt_weight.Text = "0.5"; 
            #endregion

            Vclvar.ConsignmentNo = txt_consignmentNo.Text;
            DataTable dt = GetConsignmentDetail(txt_consignmentNo.Text);
            DataSet ds2 = Fun.Get_InternationConsignmentInvoiceStatus(Vclvar);

            clvar.consignmentNo = txt_consignmentNo.Text;
            DataTable pm = GetConsignmentModifiers(clvar);

            if (dt.Rows.Count > 0)
            {
                isValid = false;
                message = "CN Already Exists";
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Consignment Already Booked.')", true);

                //   btn_save.Enabled = false;
                //btn_update.Enabled = true;
                //btn_update.Visible = true;
                //btn_Validate.Visible = false;

                txt_consignmentNo.Text = dt.Rows[0]["consignmentnumber"].ToString();
                if (dt.Rows[0]["CONSIGNMENTTYPEID"].ToString() != "")
                {
                    dd_conType.SelectedValue = dt.Rows[0]["CONSIGNMENTTYPEID"].ToString();
                }

                dd_destination.SelectedValue = dt.Rows[0]["Destination"].ToString();
                //dd_origin.SelectedValue = dt.Rows[0]["ORGIN"].ToString() + "-" + dt.Rows[0]["ZONECODE"].ToString();
                //  rbtn_customerType.SelectedValue = dt.Rows[0]["CUSTOMERTYPE"].ToString();
                txt_accountNo.Text = dt.Rows[0]["consigneraccountno"].ToString();

                if (dt.Rows[0]["consigneraccountno"].ToString() != "4D1")
                {
                    txt_accountNo_TextChanged(sender, e);
                }
                if (dt.Rows[0]["consigneraccountno"].ToString() == "0")
                {
                    //txt_chargedamount.Enabled = true;
                    txt_chargedamount.Text = dt.Rows[0]["chargedAmount"].ToString();
                }
                else
                {
                    txt_chargedamount.Enabled = false;
                    txt_chargedamount.Text = "0.00";
                }

                if (pm != null)
                {
                    if (pm.Rows.Count > 0)
                    {
                        DataTable dtSurcharges = new DataTable();
                        DataColumn dc = new DataColumn("ID", typeof(int));
                        dtSurcharges.Columns.Add(dc);
                        dc = new DataColumn("Pname", typeof(string));
                        dtSurcharges.Columns.Add(dc);
                        dc = new DataColumn("PValue", typeof(double));
                        dtSurcharges.Columns.Add(dc);
                        dc = new DataColumn("ActualValue", typeof(double));
                        dtSurcharges.Columns.Add(dc);
                        dc = new DataColumn("Base", typeof(string));
                        dtSurcharges.Columns.Add(dc);
                        dc = new DataColumn("Description", typeof(string));
                        dtSurcharges.Columns.Add(dc);
                        dtSurcharges.AcceptChanges();

                        foreach (DataRow dr in pm.Rows)
                        {
                            DataRow row = dtSurcharges.NewRow();
                            row["id"] = dr["PriceModifierID"].ToString();
                            row["Pname"] = dr["name"].ToString();
                            row["PValue"] = dr["ModifiedCalculationValue"].ToString();
                            row["ActualValue"] = dr["CalculatedValue"].ToString();
                            row["Base"] = dr["CalculationBase"].ToString();
                            row["Description"] = dr["Description"].ToString();
                            dtSurcharges.Rows.Add(row);
                        }

                        ViewState["dt"] = dtSurcharges;

                        gv_surcharges.DataSource = dtSurcharges;
                        gv_surcharges.DataBind();
                    }
                }


                txt_b.Text = dt.Rows[0]["breadth"].ToString();
                txt_bookingDate.Text = dt.Rows[0]["BOOKINGDATE"].ToString();
                txt_cellNo.Text = dt.Rows[0]["CONSIGNEEPHONENO"].ToString();
                txt_consignee.Text = dt.Rows[0]["CONSIGNEE"].ToString();
                txt_consigneeAddress.Text = dt.Rows[0]["ADDRESS"].ToString();
                txt_consigneeAttn.Text = "";
                txt_consigner.Text = dt.Rows[0]["CONSIGNER"].ToString();
                txt_consignerAddress.Text = dt.Rows[0]["SHIPPERADDRESS"].ToString();
                txt_consignerCellNo.Text = dt.Rows[0]["ConsignerCellNo"].ToString();
                txt_consignerCNIC.Text = dt.Rows[0]["CONSIGNERCNICNO"].ToString();
                //txt_consignmentNo.Text = "";
                txt_cupon.Text = dt.Rows[0]["COUPONNUMBER"].ToString();
                txt_consignerAddress.Text = dt.Rows[0]["shipperaddress"].ToString();

                txt_declaredValue.Text = "";
                //  dd_destination.SelectedIndex = dd_destination.Items.IndexOf(dd_destination.Items.FindByValue(dt.Rows[0]["destinationCountryCode"].ToString()));
                if (dt.Rows[0]["destinationCountryCode"].ToString() != "")
                {
                    dd_destCountry.SelectedIndex = dd_destCountry.Items.IndexOf(dd_destCountry.Items.FindByValue(dt.Rows[0]["destinationCountryCode"].ToString()));
                }
                dd_serviceType.SelectedIndex = dd_serviceType.Items.IndexOf(dd_serviceType.Items.FindByValue(dt.Rows[0]["serviceTypeName"].ToString()));
                dd_conType.SelectedIndex = dd_conType.Items.IndexOf(dd_conType.Items.FindByValue(dt.Rows[0]["consignmentTypeId"].ToString()));

                dd_serviceType_SelectedIndexChanged(sender, e);

                txt_discount.Text = dt.Rows[0]["DISCOUNT"].ToString();
                txt_h.Text = dt.Rows[0]["HEIGHT"].ToString();
                txt_insurance.Text = "";
                txt_packageContent.Text = dt.Rows[0]["PAKAGECONTENTs"].ToString();
                txt_pieces.Text = dt.Rows[0]["pieces"].ToString();

                txt_riderCode.Text = dt.Rows[0]["RIDERCODE"].ToString();
                txt_riderCode_TextChanged(sender, e);

                txt_value.Text = dt.Rows[0]["totalAmount"].ToString();
                txt_w.Text = dt.Rows[0]["Width"].ToString();
                txt_weight.Text = dt.Rows[0]["Weight"].ToString();
                //   txt_chargedamount.Text = dt.Rows[0]["chargedAmount"].ToString();
                txt_reporting_date.Text = dt.Rows[0]["accountReceivingDate"].ToString();

                if (dt.Rows[0]["isApproved"].ToString() == "False")
                {
                    txt_approve_status.Text = "UNAPPROVED";
                }
                if (dt.Rows[0]["isApproved"].ToString() == "True")
                {
                    txt_approve_status.Text = "Approved";
                }

                if (ds2.Tables[0].Rows.Count != 0)
                {
                    txt_inv_status.Text = ds2.Tables[0].Rows[0]["consignmentState"].ToString();
                    txt_invoice.Text = ds2.Tables[0].Rows[0]["invoiceNumber"].ToString();
                }

            }
            else
            {
                isValid = true;
                message = "";
                txt_approve_status.Text = "UNAPPROVED";
                ViewState["CNSTATE"] = "NEW";
                string specialCondition = "\n AND mzc.Product = 'International'\n";
                clvar.Zone = HttpContext.Current.Session["ZoneCode"].ToString();
                clvar.consignmentNo = txt_consignmentNo.Text.Trim();
                //DataTable allowedCN = SequenceCheck(clvar, specialCondition);
                //if (allowedCN.Rows.Count == 0)
                //{
                //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('This CN Number cannot be used in this Zone')", true);
                //    txt_consignmentNo.Text = "";
                //    return;
                //}



            }
            txt_accountNo.Focus();
        }

        protected DataTable GetConsignmentDetail(string conNumber)
        {
            string query = "SELECT CONVERT(VARCHAR(10), bookingDate, 105) bookingDate, CONVERT(VARCHAR(10), accountReceivingDate, 105)accountReceivingDate, *\n" +
                            "FROM Consignment WHERE CONSIGNMENTNUMBER = '" + conNumber + "'";
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
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
        protected void txt_accountNo_TextChanged(object sender, EventArgs e)
        {
            #region MyRegion
            //if (txt_accountNo.Text != "0")
            //{
            //    #region MyRegion
            //    string query = "SELECT * FROM CREDITCLIENTS WHERE ACCOUNTNO = '" + txt_accountNo.Text + "' and branchcode = '" + Session["BRANCHCODE"].ToString() + "'";
            //    DataTable dt = new DataTable();
            //    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            //    try
            //    {
            //        con.Open();
            //        SqlDataAdapter sda = new SqlDataAdapter(query, con);
            //        sda.Fill(dt);

            //    }
            //    catch (Exception ex)
            //    { }
            //    finally { con.Close(); }
            //    #endregion

            //    txt_chargedamount.Enabled = false;
            //    txt_chargedamount.Text = "0";
            //    if (dt.Rows.Count > 0)
            //    {
            //        hd_creditClientID.Value = dt.Rows[0]["ID"].ToString();
            //        txt_consigner.Text = dt.Rows[0]["NAME"].ToString();
            //        txt_consigner.Enabled = false;
            //        txt_consignerCellNo.Text = dt.Rows[0]["PHONENO"].ToString();
            //        //txt_consignerCNIC.Text = dt.Rows[0][""].ToString();
            //        txt_consignerAddress.Text = dt.Rows[0]["MAILINGADDRESS"].ToString();
            //        if (txt_accountNo.Text.Trim() == "0")
            //        {
            //            hd_customerType.Value = "1";
            //        }
            //        else
            //        {
            //            hd_customerType.Value = "2";
            //        }
            //        if (dt.Rows[0]["ISCOD"].ToString().ToLower() == "true")
            //        {
            //            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Selected Customer is COD. Cannot Create International Consignment')", true);
            //            //  btn_save.Enabled = false;
            //            txt_accountNo.Focus();
            //        }
            //        else
            //        {
            //            txt_cupon.Focus();
            //            // btn_save.Enabled = true;
            //        }

            //    }
            //    else
            //    {
            //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Invalid Account Number')", true);
            //        txt_accountNo.Text = "";
            //        txt_accountNo.Focus();

            //        return;
            //    }
            //    // rbtn_customerType.SelectedValue = "1";


            //}
            //else
            //{
            //    //    rbtn_customerType.SelectedValue = "2";

            //    #region MyRegion
            //    string query = "SELECT * FROM CREDITCLIENTS WHERE ACCOUNTNO = '" + txt_accountNo.Text + "' and branchcode = '" + Session["BRANCHCODE"].ToString() + "'";
            //    DataTable dt = new DataTable();
            //    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            //    try
            //    {
            //        con.Open();
            //        SqlDataAdapter sda = new SqlDataAdapter(query, con);
            //        sda.Fill(dt);

            //    }
            //    catch (Exception ex)
            //    { }
            //    finally { con.Close(); }
            //    #endregion

            //    if (dt.Rows.Count > 0)
            //    {
            //        txt_chargedamount.Text = "0";
            //        txt_chargedamount.Enabled = true;

            //        hd_creditClientID.Value = dt.Rows[0]["ID"].ToString();
            //        txt_consigner.Text = "Cash";
            //        txt_consigner.Enabled = false;
            //        txt_consignerCellNo.Text = "";
            //        //txt_consignerCNIC.Text = dt.Rows[0][""].ToString();
            //        txt_consignerAddress.Text = "";
            //        if (dt.Rows[0]["ISCOD"].ToString().ToLower() == "true")
            //        {
            //            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Selected Customer is COD. Cannot Create International Consignment')", true);
            //            //  btn_save.Enabled = false;
            //            txt_accountNo.Focus();
            //        }
            //        else
            //        {
            //            txt_cupon.Focus();
            //            // btn_save.Enabled = true;
            //        }
            //        txt_consigner.Focus();

            //    }
            //    else
            //    {
            //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Invalid Account No.')", true);
            //        txt_accountNo.Focus();
            //        hd_creditClientID.Value = "";
            //        txt_consigner.Text = "";
            //        txt_consigner.Enabled = true;
            //        txt_consignerCellNo.Text = "";
            //        //txt_consignerCNIC.Text = dt.Rows[0][""].ToString();
            //        txt_consignerAddress.Text = "";
            //        txt_accountNo.Focus();
            //    }
            //} 
            #endregion

            if (txt_accountNo.Text != "")
            {
                clvar.Branch = HttpContext.Current.Session["BranchCode"].ToString();
                clvar.AccountNo = txt_accountNo.Text;
                DataSet ds = CustomerInformation(clvar);

                if (ds.Tables.Count != 0)
                {
                    if (ds.Tables[0].Rows.Count != 0)
                    {
                        if (txt_accountNo.Text != "0")
                        {
                            txt_consigner.Text = ds.Tables[0].Rows[0]["Name"].ToString();
                            txt_consignerCellNo.Text = ds.Tables[0].Rows[0]["PhoneNo"].ToString();
                            txt_consignerAddress.Text = ds.Tables[0].Rows[0]["address"].ToString();
                            txt_chargedamount.Enabled = false;
                        }
                        else
                        {
                            txt_chargedamount.Enabled = true;
                        }
                        hd_creditClientID.Value = ds.Tables[0].Rows[0]["id"].ToString();


                        if (ds.Tables[0].Rows[0]["accountNo"].ToString() == "0")
                        {
                            //this.rbtn_customerType.SelectedValue = "1";
                        }
                        else
                        {
                            //this.rbtn_customerType.SelectedValue = "2";
                        }
                        if (ds.Tables[0].Rows[0]["isAPIClient"].ToString() == "1")
                        {
                            hd_unApproveable.Value = "0";
                            txt_accountNo.Text = "";
                            txt_consigner.Text = "";
                            txt_consignerCellNo.Text = "";
                            txt_consignerAddress.Text = "";
                            txt_accountNo.Focus();
                            Alert("This Account is Reserved only for COD Portal.");
                            isValid = false;
                            return;
                        }
                        //cb_Destination.Focus();
                    }
                    else
                    {
                        Alert("Invalid Account No");
                        txt_accountNo.Text = "";
                        txt_accountNo.Focus();
                        isValid = false;
                        return;
                    }
                }
                else
                {
                    Alert("Account Not Found");
                    txt_accountNo.Text = "";
                    txt_accountNo.Focus();
                    isValid = false;
                    return;
                }
            }
            else
            {
                txt_accountNo.Focus();
                return;
            }
            txt_consigner.Focus();
        }

        protected void txt_riderCode_TextChanged(object sender, EventArgs e)
        {
            #region MyRegion
            string query = "SELECT * FROM RIDERS WHERE BRANCHID = '" + HttpContext.Current.Session["BranchCode"].ToString() + "' and ridercode = '" + txt_riderCode.Text + "' and status = '1'";
            DataTable dt = new DataTable();

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString_"].ConnectionString);
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                sda.Fill(dt);

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            #endregion
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    txt_pieces.Focus();
                    ecCode.Value = dt.Rows[0]["ExpressCenterID"].ToString();
                    //     txt_chargedamount.Enabled = false;
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Invalid Rider Code')", true);
                    isValid = false;
                    txt_riderCode.Text = ""; chk_riderCode.Checked = false;
                    txt_riderCode.Focus();
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Invalid Rider Code')", true);
                isValid = false;
                txt_riderCode.Text = ""; chk_riderCode.Checked = false;
                txt_riderCode.Focus();
            }

        }
        protected void dd_name_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dd_name.SelectedValue != "0")
            {
                rbtn_calculationBase.SelectedValue = dd_name.SelectedValue.Split('-')[1];
                txt_value.Text = dd_name.SelectedValue.Split('-')[2];
            }
            dd_name.Focus();


        }
        protected void btn_add_Click(object sender, EventArgs e)
        {
            if (dd_name.SelectedValue != "0")
            {
                DataTable pm = ViewState["PriceModifiers"] as DataTable;
                DataTable dtSurcharges = ViewState["dt"] as DataTable;

                if (dtSurcharges.Select("id='" + dd_name.SelectedValue.Split('-')[0] + "'").Count() > 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cannot Add Double Surcharges')", true);
                    return;
                }

                DataRow dr = dtSurcharges.NewRow();
                dr["ID"] = dd_name.SelectedValue.Split('-')[0];
                dr["BASE"] = dd_name.SelectedValue.Split('-')[1];
                dr["PNAME"] = dd_name.SelectedItem.Text;
                dr["PValue"] = dd_name.SelectedValue.Split('-')[2];
                dr["ActualValue"] = dd_name.SelectedValue.Split('-')[2];
                dr["DESCRIPTION"] = (pm.Select("id = '" + dd_name.SelectedValue.Split('-')[0] + "'"))[0]["Description"].ToString();//txt_description.Text;
                dtSurcharges.Rows.Add(dr);
                gv_surcharges.DataSource = dtSurcharges;
                gv_surcharges.DataBind();
                ViewState["dt"] = dtSurcharges;
                btn_save.Enabled = false;
            }
        }

        protected void btn_save_Click(object sender, EventArgs e)
        {
            //if (ViewState["CNSTATE"].ToString() == "NEW")
            //{
            //    string specialCondition = "\n  AND mzc.Product = (select products from serviceTypes_new where serviceTypeName = '" + dd_serviceType.SelectedValue + "')";
            //    clvar.Zone = HttpContext.Current.Session["ZoneCode"].ToString();
            //    clvar.consignmentNo = txt_consignmentNo.Text.Trim();
            //    DataTable allowedCN = SequenceCheck(clvar, specialCondition);
            //    if (allowedCN.Rows.Count == 0)
            //    {
            //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('This CN Number cannot be used in for selected Service')", true);
            //        return;
            //    }
            //}
            double tempchamount = 0;
            double.TryParse(txt_chargedamount.Text, out tempchamount);
            if (txt_accountNo.Text.Trim() == "0" && (tempchamount < 0 || tempchamount == 0))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Charge Amount cannot be zero or empty')", true);
                lbl_error.Text = "Charge Amount cannot be zero or empty";
                return;
            }
            if (txt_approve_status.Text.ToUpper() == "APPROVED")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('CN Already Approved')", true);
                return;
            }
            DateTime thisDate = DateTime.Parse(txt_bookingDate.Text);
            DateTime reportingDate = DateTime.Parse(txt_reporting_date.Text);
            clvar.Branch = HttpContext.Current.Session["BranchCode"].ToString();
            DataTable dates = MinimumDate(clvar);
            DateTime minAllowedDate = DateTime.Parse(dates.Rows[0]["DateAllowed"].ToString()).AddDays(1);
            DateTime maxAllowedDate = DateTime.Now;
            if (thisDate < minAllowedDate || thisDate > maxAllowedDate || reportingDate < minAllowedDate || reportingDate > maxAllowedDate)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Invalid Date.')", true);
                return;
            }

            #region Validations

            if (txt_consignmentNo.Text.Trim(' ') == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter Consignment Number')", true);
                txt_consignmentNo.Focus();
                return;
            }
            else if (txt_consignmentNo.Text.ToCharArray()[0] != '9' && rbtn_consignmentSender.SelectedValue != "4")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Invalid Consignment Number')", true);
                txt_consignmentNo.Focus();
                return;
            }
            if (txt_accountNo.Text.Trim(' ') == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter Account No.')", true);
                txt_accountNo.Focus();
                return;
            }
            if (txt_consignerCellNo.Text.Trim(' ') == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter Consigner Cell Number')", true);
                txt_consignerCellNo.Focus();
                return;
            }
            if (txt_consigner.Text.Trim(' ') == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter Consigner')", true);
                txt_consigner.Focus();
                return;
            }
            if (txt_consignerCNIC.Text.Trim(' ') == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter Consignmener CNIC')", true);
                txt_consignerCNIC.Focus();
                return;
            }
            if (txt_consignee.Text.Trim(' ') == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter Consignee')", true);
                txt_consignee.Focus();
                return;
            }
            if (txt_cellNo.Text.Trim(' ') == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter Consignee Cell Number')", true);
                txt_cellNo.Focus();
                return;
            }

            if (txt_accountNo.Text != "")
            {
                if (txt_chargedamount.Text.Trim(' ') == "")
                {
                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter Charge Amount')", true);
                    //txt_cellNo.Focus();
                    //return;
                }
            }
            if (txt_reporting_date.Text.Trim(' ') == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter Reporting Date')", true);
                txt_cellNo.Focus();
                return;
            }

            if (dd_PaymentMode.SelectedValue == "0")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Select Payment Mode')", true);
                txt_cellNo.Focus();
                return;
            }



            if (txt_riderCode.Text.Trim(' ') != "")
            {
                clvar.riderCode = txt_riderCode.Text;
                DataTable rider = ValidateRiderCode(clvar);
                if (rider != null)
                {
                    if (rider.Rows.Count > 0)
                    {
                        ecCode.Value = rider.Rows[0]["ExpressCenterID"].ToString();
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Invalid Rider Code')", true);
                        txt_riderCode.Text = ""; chk_riderCode.Checked = false;
                        txt_riderCode.Focus();
                        return;
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Invalid Rider Code')", true);
                    txt_riderCode.Text = ""; chk_riderCode.Checked = false;
                    txt_riderCode.Focus();
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter Rider Code')", true);
                //txt_riderCode.Text = ""; chk_riderCode.Checked = false;
                txt_riderCode.Focus();
            }
            if (dd_destCountry.SelectedValue.Trim(' ') == "0")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Select Destination Country')", true);
                dd_destCountry.Focus();
                return;
            }
            if (dd_serviceType.SelectedValue.Trim(' ') == "0")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Select Service Type')", true);
                dd_serviceType.Focus();
                return;
            }
            if (txt_weight.Text.Trim(' ') == "" || txt_weight.Text.Trim(' ') == "0")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter Proper Weight')", true);
                txt_weight.Focus();
                return;
            }
            if (txt_consignerAddress.Text.Trim(' ') == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter Consigner Address')", true);
                txt_consignerAddress.Focus();
                return;
            }
            if (txt_consigneeAddress.Text.Trim(' ') == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter Consignee Address')", true);
                txt_consigneeAddress.Focus();
                return;
            }
            if (txt_pieces.Text.Trim() == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter Pieces')", true);
                txt_consigneeAddress.Focus();
                return;
            }
            else if (int.Parse(txt_pieces.Text.Trim()) <= 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Pieces cannot be zero')", true);
                txt_consigneeAddress.Focus();
                return;
            }

            double tempPieces = 0;
            double.TryParse(txt_pieces.Text, out tempPieces);
            if (tempPieces <= 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Pieces Cannot be equal or less than 0')", true);
                lbl_error.Text = "Pieces Cannot be equal or less than 0";
                return;
            }

            #endregion

            double totalAmount = 0;
            int cod = 0;
            try
            {
                clvar.Zone = Session["zonecode"].ToString(); //"2";//dd_origin.SelectedValue.Split('-')[1];
                clvar.expresscenter = Session["ExpressCenter"].ToString();
                clvar.destinationExpressCenterCode = "0111";
                clvar.FromZoneCode = Session["zonecode"].ToString();//dd_origin.SelectedValue.Split('-')[1];
            }
            catch (Exception ex)
            {
                Response.Redirect("~/login");
            }

            clvar.destinationCountryCode = dd_destCountry.SelectedValue;
            clvar.CustomerClientID = hd_creditClientID.Value;
            if (ViewState["serviceType"] != null)
            {
                clvar.ToZoneCode = (ViewState["serviceType"] as DataTable).Rows[0][0].ToString();
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cannot Add Consignment')", true);
                return;
            }

            clvar.ServiceTypeName = dd_serviceType.SelectedValue;
            clvar.Branch = dd_origin.SelectedValue.Split('-')[0];
            //DataTable dt = GetClientTarrifForInternationalConsignment(clvar).Tables[0];
            double tempw = 0;

            double.TryParse(txt_weight.Text, out tempw);
            if (tempw <= 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Weight Cannot be equal or less than 0')", true);
                lbl_error.Text = "Weight Cannot be equal or less than 0";
                return;
            }
            clvar.Weight = float.Parse(txt_weight.Text);

            if (clvar.Weight == 0 || clvar.Weight == null)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Enter Weight.')", true);
                return;
            }
            #region MyRegion
            //if (dt != null)
            //{
            //    if (dt.Rows.Count <= 0)
            //    {
            //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('No Tarrif Available. Cannot Create Consignment')", true);
            //        return;
            //    }
            //    else
            //    {
            //        double temp = 0;
            //        double temp_ = 0;
            //        double amount = 0;
            //        string currency = "";
            //        DataRow[] dr_ = dt.Select("ClIENT_ID = '" + int.Parse(hd_creditClientID.Value) + "'");
            //        //DataTable tempdt = dr_.CopyToDataTable();
            //        //double maxWeight = double.Parse(tempdt.Compute("MAX(TOWeight)", "").ToString());
            //        //double actualWeight = double.Parse(txt_weight.Text);
            //        if (dr_.Count() > 0)
            //        {
            //            DataTable tempdt = dr_.CopyToDataTable();
            //            double maxWeight = double.Parse(tempdt.Compute("MAX(TOWeight)", "").ToString());
            //            double actualWeight = double.Parse(txt_weight.Text);
            //            if (actualWeight <= maxWeight)
            //            {
            //                foreach (DataRow dr in dr_)
            //                {
            //                    temp = double.Parse(dr["FromWeight"].ToString());
            //                    temp_ = double.Parse(dr["ToWeight"].ToString());
            //                    if (clvar.Weight >= temp && clvar.Weight <= temp_)
            //                    {
            //                        amount = double.Parse(dr["Price"].ToString());
            //                        currency = dr["currencyCodeID"].ToString();
            //                        break;
            //                    }

            //                }
            //            }
            //            else
            //            {
            //                //foreach (DataRow dr in dr_)
            //                //{
            //                //    temp = double.Parse(dr["FromWeight"].ToString());
            //                //    temp_ = double.Parse(dr["ToWeight"].ToString());
            //                //    if (clvar.Weight > temp && clvar.Weight <= temp_)
            //                //    {
            //                //        amount = double.Parse(dr["Price"].ToString());
            //                //        currency = dr["currencyCodeID"].ToString();
            //                //        break;
            //                //    }

            //                //}
            //                if (tempdt == null)
            //                {
            //                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Additional Factor Not Found')", true);
            //                    return;
            //                }
            //                if (tempdt.Rows.Count == 0)
            //                {
            //                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Additional Factor Not Found')", true);
            //                    return;
            //                }
            //                amount = double.Parse(dr_.CopyToDataTable().Select("TOWEIGHT = '" + maxWeight + "'")[0]["PRICE"].ToString());
            //                currency = dr_.CopyToDataTable().Select("TOWEIGHT = '" + maxWeight + "'")[0]["currencyCodeID"].ToString();
            //                double remainingWeight = actualWeight - maxWeight;
            //                double additionalFactor = double.Parse(tempdt.Select("TOWEIGHT = '" + maxWeight + "'", "")[0]["additionalFactor"].ToString());
            //                double price = double.Parse(tempdt.Select("TOWEIGHT = '" + maxWeight + "'", "")[0]["addtionalFactorDZ"].ToString());
            //                if (additionalFactor == 0)
            //                {
            //                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Additional Factor Not Found')", true);
            //                    return;
            //                }
            //                double value = Math.Ceiling(remainingWeight / additionalFactor) * price;
            //                amount += value;
            //            }


            //        }
            //        else
            //        {

            //            dr_ = dt.Select("CLIENT_ID = '0'");
            //            DataTable tempdt = GetClientTarrifForInternationalConsignmentForZero(clvar).Tables[0];
            //            if (tempdt.Rows.Count == 0)
            //            {
            //                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Tariff Not Available')", true);
            //                return;
            //            }
            //            double maxWeight = double.Parse(tempdt.Compute("MAX(TOWeight)", "").ToString());
            //            double actualWeight = double.Parse(txt_weight.Text);
            //            if (dr_.Count() > 0)
            //            {
            //                int count_ = 0;
            //                if (actualWeight <= maxWeight)
            //                {
            //                    foreach (DataRow dr in dr_)
            //                    {
            //                        temp = double.Parse(dr["FromWeight"].ToString());
            //                        temp_ = double.Parse(dr["ToWeight"].ToString());
            //                        if (clvar.Weight >= temp && clvar.Weight <= temp_)
            //                        {
            //                            amount = double.Parse(dr["Price"].ToString());
            //                            currency = dr["currencyCodeID"].ToString();
            //                            count_++;
            //                            break;
            //                        }

            //                    }
            //                    if (count_ == 0)
            //                    {
            //                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Could not Find Weight Bucket')", true);
            //                        return;
            //                    }
            //                }
            //                else
            //                {
            //                    //foreach (DataRow dr in dr_)
            //                    //{
            //                    //    temp = double.Parse(dr["FromWeight"].ToString());
            //                    //    temp_ = double.Parse(dr["ToWeight"].ToString());
            //                    //    if (clvar.Weight >= temp && clvar.Weight <= temp_)
            //                    //    {
            //                    //        amount = double.Parse(dr["Price"].ToString());
            //                    //        currency = dr["currencyCodeID"].ToString();
            //                    //        count_++;
            //                    //        break;
            //                    //    }

            //                    //}
            //                    if (tempdt == null)
            //                    {
            //                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Additional Factor Not Found')", true);
            //                        return;
            //                    }
            //                    if (tempdt.Rows.Count == 0)
            //                    {
            //                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Additional Factor Not Found')", true);
            //                        return;
            //                    }
            //                    amount = double.Parse(dr_.CopyToDataTable().Select("TOWEIGHT = '" + maxWeight + "'")[0]["PRICE"].ToString());
            //                    currency = dr_.CopyToDataTable().Select("TOWEIGHT = '" + maxWeight + "'")[0]["currencyCodeID"].ToString();
            //                    double remainingWeight = actualWeight - maxWeight;
            //                    double additionalFactor = double.Parse(tempdt.Select("TOWEIGHT = '" + maxWeight + "'", "")[0]["additionalFactor"].ToString());
            //                    double price = double.Parse(tempdt.Select("TOWEIGHT = '" + maxWeight + "'", "")[0]["addtionalFactorDZ"].ToString());
            //                    double value = Math.Ceiling(remainingWeight / additionalFactor) * price;
            //                    if (additionalFactor == 0)
            //                    {
            //                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Additional Factor Not Found')", true);
            //                        return;
            //                    }
            //                    amount += value;
            //                }


            //            }
            //            else
            //            {
            //                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('No Tarrif Available. Cannot Create Consignment')", true);
            //                return;
            //            }
            //        }
            //        clvar.BaseCurrency = currency;
            //        if (clvar.BaseCurrency != "2" && clvar.BaseCurrency != "")
            //        {
            //            DataTable dtCurrencyRates = com.GetCurrencyConversionRates(clvar).Tables[0];
            //            if (dtCurrencyRates != null)
            //            {
            //                if (dtCurrencyRates.Rows.Count > 0)
            //                {
            //                    totalAmount = double.Parse(dtCurrencyRates.Rows[0]["Rate"].ToString()) * amount;
            //                    clvar.TotalAmount = totalAmount;
            //                }
            //                else
            //                {
            //                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Currency Rates Not Available. Cannot Create Consignment.')", true);
            //                    return;
            //                }
            //            }
            //            else
            //            {
            //                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Currency Rates Not Available. Cannot Create Consignment.')", true);
            //                return;
            //            }
            //        }
            //        else
            //        {
            //            totalAmount = amount;
            //            clvar.TotalAmount = totalAmount;
            //        }

            //    }
            //}
            //else
            //{
            //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('No Tarrif Available. Cannot Create Consignment')", true);
            //    return;
            //} 
            #endregion

            clvar.consignmentNo = txt_consignmentNo.Text;
            clvar.Consigner = txt_consigner.Text;
            clvar.Consignee = txt_consignee.Text;
            clvar.CouponNo = txt_cupon.Text;
            //   clvar.Customertype = int.Parse(rbtn_customerType.SelectedValue);
            clvar.origin = dd_origin.SelectedValue.Split('-')[0];
            clvar.Destination = dd_destination.SelectedValue;
            clvar.pieces = int.Parse(txt_pieces.Text);
            clvar.ServiceTypeName = dd_serviceType.SelectedValue;
            clvar.CustomerClientID = hd_creditClientID.Value;
            clvar.Con_Type = int.Parse(dd_conType.SelectedValue);
            clvar.Unit = 0;
            //  clvar.ChargeAmount = int.Parse(txt_chargedamount.Text);

            //clvar.Discount = int.Parse(txt_discount.Text);
            int d = 0;
            int.TryParse(txt_discount.Text, out d);
            clvar.Discount = d;
            //clvar.isCod = false;

            clvar.ConsigneeAddress = txt_consigneeAddress.Text;
            clvar.createdBy = "99";
            clvar.status = 1;


            clvar.ConsignerAddress = txt_consignerAddress.Text;
            clvar.RiderCode = txt_riderCode.Text;
            double w = 0;
            double.TryParse(txt_w.Text, out w);
            clvar.width = w;//double.Parse(txt_w.Text);
            double b = 0;
            double.TryParse(txt_b.Text, out b);
            clvar.breadth = b;// double.Parse(txt_b.Text);
            double h = 0;
            double.TryParse(txt_h.Text, out h);

            clvar.height = h;// double.Parse(txt_h.Text);
            clvar.PakageContents = txt_packageContent.Text;
            clvar.ConsigneeCell = txt_cellNo.Text;
            clvar.ConsignerCell = txt_consignerCellNo.Text;
            clvar.ConsignerCNIC = txt_consignerCNIC.Text;
            clvar.AccountNo = txt_accountNo.Text;
            clvar.destinationCountryCode = dd_destCountry.SelectedValue;
            float declaredValue = 0;
            if (chk_insurance.Checked)
            {
                clvar.Declaredvalue = declaredValue = float.Parse(txt_declaredValue.Text);
                clvar.insuarancePercentage = 2.5f;
                clvar.isInsured = true;

                if (clvar.Declaredvalue <= 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Enter Declared Value')", true);
                    return;
                }

            }
            else
            {
                clvar.Declaredvalue = 0;
                clvar.insuarancePercentage = 0;
                clvar.isInsured = false;
            }
            clvar.Bookingdate = DateTime.Parse(txt_bookingDate.Text);

            clvar.originBrName = dd_origin.SelectedItem.Text;
            clvar.customerName = txt_consigner.Text;
            //clvar.serviceTypeId = dd_serviceType.SelectedValue;
            clvar.originId = dd_origin.SelectedValue.Split('-')[0];
            clvar.isExpUser = "1";

            DataSet BranchGSTInformation = con.BranchGSTInformation(clvar);
            string gst = "";
            if (BranchGSTInformation.Tables[0].Rows.Count != 0)
            {
                gst = BranchGSTInformation.Tables[0].Rows[0]["gst"].ToString();
            }
            clvar.gst = (double.Parse(gst) / 100) * totalAmount;
            clvar.consignerAccountNo = txt_accountNo.Text;
            clvar.PaymentMode = dd_PaymentMode.SelectedValue;
            clvar.PaymentTransactionID = txt_TransactionID.Text;


            DataTable dtc = new DataTable();
            dtc.Columns.AddRange(new DataColumn[12]
            {
            new DataColumn("PriceModifierID", typeof(int)),
            new DataColumn("ConsignmentNumber", typeof(Int64)),
            new DataColumn("CreatedBy",typeof(string)),
            new DataColumn("CreatedOn", typeof(DateTime)),
            new DataColumn("ModifiedBy", typeof(string)),
            new DataColumn("ModifiedOn", typeof(DateTime)),
            new DataColumn("ModifiedCalculationValue", typeof(int)),
            new DataColumn("CalculatedValue", typeof(float)),
            new DataColumn("CalculatedGST", typeof(float)),
            new DataColumn("CalculationBase", typeof(int)),
            new DataColumn("isTaxable", typeof(bool)),
            new DataColumn("SortOrder", typeof(int))

            });
            dtc.AcceptChanges();
            DataTable pm = ViewState["PriceModifiers"] as DataTable;
            DataTable surcharges = ViewState["dt"] as DataTable;


            int count = 1;
            double tempTotalAmount = totalAmount;
            double tempTotalGst = (double.Parse(gst) / 100) * totalAmount;
            double tempGrandTotalAmount = 0;
            double insurance = 0;
            insurance = declaredValue * 0.025;
            tempTotalGst += insurance * (double.Parse(gst) / 100);
            tempTotalAmount += insurance;
            double tempt = totalAmount;


            tempGrandTotalAmount = tempTotalAmount + tempTotalGst;


            //if (sender.Equals(btn_Validate))
            //{
            //    btn_save.Visible = true;
            //    //btn_Validate.Visible = false;
            //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Total Amount      " + tempTotalAmount + ". \\nTotalGst          " + tempTotalGst + ".\\nGrand Total       " + tempGrandTotalAmount + "')", true);
            //    return;
            //}


            try
            {
                clvar.expresscenter = HttpContext.Current.Session["ExpressCenter"].ToString();
                clvar.OriginExpressCenterCode = ecCode.Value;
                clvar.destinationExpressCenterCode = "0111";
            }
            catch (Exception)
            {

                Response.Redirect("~/login");
            }


            if (clvar.isCod == false)
            {
                clvar.StateID = "0";
            }

            if (clvar.isCod == true)
            {
                clvar.StateID = "1";
            }


            clvar.gst = Math.Round(tempTotalGst, 2);
            //clvar.ChargeAmount = Double.Parse(txt_chargedamount.Text.ToString());

            //clvar.StateID = codcheck.ToString();
            clvar._CityCode = dd_destination.SelectedItem.Text;

            clvar.Day = txt_reporting_date.Text;
            clvar.Customertype = 0;

            string Error = Add_Consignment_Validation(clvar);
            if (txt_accountNo.Text.Trim() == "0")
            {
                clvar.Customertype = 1;
            }
            else
            {
                clvar.Customertype = 2;
            }

            if (Error == "")
            {
                DataSet pricing = Add_OcsInternationalValidation(clvar);
                if (pricing != null)
                {
                    if (pricing.Tables[0].Rows.Count > 0)
                    {
                        if (pricing.Tables[1].Rows[0]["amount"].ToString().Trim() == "0" || pricing.Tables[1].Rows[0]["amount"].ToString().Trim() == "")
                        {
                            AlertMessage("Could Not Compute Prices");
                            return;
                        }
                        else
                        {
                            clvar.TotalAmount = double.Parse(pricing.Tables[1].Rows[0]["amount"].ToString());
                            clvar.gst = double.Parse(pricing.Tables[1].Rows[0]["gst"].ToString());

                            clvar.cnScreenId = "2";
                            clvar.deliveryDate = DateTime.Parse(txt_reporting_date.Text);
                            clvar.createdBy = HttpContext.Current.Session["U_ID"].ToString();
                            Error = Add_Consignment(clvar);

                            if (Error == "")
                            {
                                if (txt_accountNo.Text.Trim() == "0")
                                {
                                    updateChargedAmount(clvar);
                                }
                            }
                            else
                            {
                                AlertMessage("Could not Add Consignment");
                                return;
                            }
                            foreach (DataRow dr in surcharges.Rows)
                            {
                                DataRow dr_ = dtc.NewRow();
                                dr_["pricemodifierid"] = int.Parse(pm.Select("id = '" + dr["id"].ToString() + "'")[0]["ID"].ToString());
                                dr_["ConsignmentNumber"] = Int64.Parse(txt_consignmentNo.Text);
                                dr_["CreatedBy"] = "TEST";
                                dr_["CreatedOn"] = DateTime.Now;
                                dr_["ModifiedBy"] = DBNull.Value;
                                dr_["ModifiedOn"] = DBNull.Value;
                                if (dr["BASE"].ToString() == "2")
                                {
                                    dr_["CalculatedValue"] = (float.Parse(dr["PValue"].ToString()) / 100) * clvar.TotalAmount;
                                    dr_["CalculatedGst"] = ((float.Parse(dr["PValue"].ToString()) / 100) * clvar.TotalAmount) * (float.Parse(BranchGSTInformation.Tables[0].Rows[0]["GST"].ToString()) / 100);
                                    tempTotalAmount += (float.Parse(dr["PValue"].ToString()) / 100) * clvar.TotalAmount;
                                    tempTotalGst += ((float.Parse(dr["PValue"].ToString()) / 100) * clvar.TotalAmount) * (float.Parse(BranchGSTInformation.Tables[0].Rows[0]["GST"].ToString()) / 100);
                                    tempt = tempt + (float.Parse(dr["PValue"].ToString()) / 100) * clvar.TotalAmount;
                                    //dr_["CalculatedGst"] = (float.Parse(BranchGSTInformation.Tables[0].Rows[0]["GST"].ToString()) / 100) * float.Parse(dr["Pvalue"].ToString());
                                }
                                else
                                {
                                    dr_["CalculatedValue"] = float.Parse(dr["Pvalue"].ToString()); //float.Parse(pm.Select("id = '" + dr["id"].ToString() + "'")[0]["ActualValue"].ToString());

                                    dr_["CalculatedGst"] = (float.Parse(BranchGSTInformation.Tables[0].Rows[0]["GST"].ToString()) / 100) * float.Parse(dr["Pvalue"].ToString());
                                    tempTotalAmount += float.Parse(dr["Pvalue"].ToString());
                                    tempTotalGst += (float.Parse(BranchGSTInformation.Tables[0].Rows[0]["GST"].ToString()) / 100) * float.Parse(dr["Pvalue"].ToString());
                                    tempt = tempt + float.Parse(dr["Pvalue"].ToString());
                                }

                                dr_["ModifiedCalculationValue"] = float.Parse(dr["Pvalue"].ToString());//float.Parse(pm.Select("id = '" + dr["id"].ToString() + "'")[0]["CalculationValue"].ToString());
                                                                                                       //dr_["CalculatedGst"] = (float.Parse(BranchGSTInformation.Tables[0].Rows[0]["GST"].ToString()) / 100) * float.Parse(dr["Pvalue"].ToString());
                                dr_["CalculationBase"] = int.Parse(dr["BASE"].ToString());
                                dr_["isTaxable"] = 1;
                                dr_["SortOrder"] = count++;
                                dtc.Rows.Add(dr_);
                                dtc.AcceptChanges();
                            }

                            Error = Add_ConsignmentModifier(dtc, clvar);
                            if (Error == "")
                            {
                                AlertMessage("Consignment Saved");

                                Post_BrandedSMS(txt_consignerCellNo.Text, txt_consignmentNo.Text, txt_consigner.Text.ToUpper(), dd_destination.SelectedItem.Text);
                                //if (txt_SmsConsignment.Checked == true)
                                //{
                                Post_BrandedSMS_(txt_cellNo.Text, txt_consignmentNo.Text, txt_consignee.Text.ToUpper(), dd_destination.SelectedItem.Text);

                                //txt_consignmentNo_TextChanged(this, e);
                                string script_ = @"window.open(""{0}"", ""{1}"", ""{2}"");";

                                string script = String.Format(script_, "InternationalPrint.aspx?XCode=" + txt_consignmentNo.Text, "_blank", "");

                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Redirect", script, true);
                                btn_reset_Click(this, e);

                                return;

                            }
                        }
                    }
                    else
                    {
                        AlertMessage("Could Not Compute Prices");
                        return;
                    }
                }
                else
                {
                    AlertMessage("Could Not Compute Prices");
                    return;
                }
            }
            else
            {
                //Error Phenk 
            }
            clvar.cnScreenId = "8";
            return;
            #region MyRegion
            //string error = con.Add_Consignment_(clvar);
            ////  con.CardConsignmentApprovalStatus(clvar);

            //clvar.StateID = "1";

            ////clvar.customerName = Session["branchcode"].ToString();
            ////     //   DataSet ds_origin = BranchGSTInformation.Tables[0].Rows[0]["GST"].ToString()
            ////clvar._CityCode = BranchGSTInformation.Tables[0].Rows[0]["name"].ToString();


            ////    DataSet ds_origin = b_fun.Get_BranchDetail(clvar);




            //clvar.consignmentNo = txt_consignmentNo.Text;
            //con.Insert_ConsignmentTrackingHistory(clvar);

            //if (error != "" && error != null)
            //{
            //    lbl_error.Text = error;
            //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cannot Create Consignment. ')", true);
            //    return;
            //}
            //clvar.consignmentNo = txt_consignmentNo.Text;
            //clvar.Company = rbtn_consignmentSender.SelectedValue;
            //error = con.AddConsignmentSender(clvar);
            //if (error != "" && error != null)
            //{
            //    lbl_error.Text = error;
            //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cannot Create Consignment. ')", true);
            //    return;
            //}


            //string error_ = con.Add_ConsignmentModifier(dtc, clvar);
            //if (error_ != "")
            //{
            //    error = "";
            //    error = con.DeleteConsignment(clvar);
            //    if (error != "")
            //    {

            //    }
            //    else
            //    {
            //        lbl_error.Text = error;
            //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Consignment Could not Save. Please Contact IT Support.')", true);
            //        return;
            //    }
            //    lbl_error.Text = error;
            //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Could Not Add Consignment Modifier. \n  Please Contact I.T. Support')", true);
            //    return;
            //}
            //int i = con.Add_InternationalConsignmentDetail(clvar);
            //if (i == 0)
            //{
            //    error = "";
            //    error = con.DeleteConsignment(clvar);
            //    if (error != "")
            //    {

            //    }
            //    else
            //    {
            //        lbl_error.Text = error;
            //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Consignment Could not Save. Please Contact IT Support.')", true);
            //        return;
            //    }
            //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Could Not Add Consignment Details. Please Contact I.T. Support')", true);
            //    return;
            //}

            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Consignment Successfully Created.')", true);
            //ResetAll();

            //return; 
            #endregion





            #region commented
            //double totalAmount = 0;
            //double insurancePercentage = 0;
            //if (chk_insurance.Checked)
            //{
            //    insurancePercentage = 2.5;
            //}
            //string query = "INSERT INTO CONSIGNMENT (CONSIGNMENTNUMBER,\n" +
            //    " CONSIGNER,\n" +
            //    " CONSIGNEE,\n" +
            //    " COUPONNUMBER, \n" +
            //    " CUSTOMERTYPE, \n" +
            //    " ORGIN, \n" +
            //    " DESTINATION, \n" +
            //    " PIECES, \n" +
            //    " SERVICETYPENAME, \n" +
            //    " CREDITCLIENTID, \n" +
            //    " WEIGHT, \n" +
            //    " WEIGHTUNIT, \n" +
            //    " DISCOUNT, \n" +
            //    " COD, \n" +
            //    " CREATEDBY, \n" +
            //    " CREATEDON, \n" +
            //    " TOTALAMOUNT, \n" +
            //    " ZONECODE, \n" +
            //    " BRANCHCODE, \n" +
            //    " CONSIGNMENTTYPEID, \n" +
            //    " RIDERCODE, \n" +
            //    " CONSIGNERACCOUNTNO, \n" +
            //    " DESTINATIONCOUNTRYCODE, \n" +
            //    " INSUaRANCEPERCENTAGE, \n" +
            //    " BOOKINGDATE, \n" +
            //    " ISPRICECOMPUTED, CONSIGNERCNICNO, shipperAddress, consignercellno, decalaredvalue, \n" +
            //    "isapproved, consigneephoneno, pakageContents, width, breadth, height)\n" +
            //    "VALUES\n" +
            //    "(\n" +
            //    "'" + txt_consignmentNo.Text + "',\n" +
            //    "'" + txt_consigner.Text + "'," +
            //    "'" + txt_consignee.Text + "'," +
            //    "'" + txt_cupon.Text + "'," +
            //    "" + rbtn_customerType.SelectedValue + "," +
            //    "'" + dd_origin.SelectedValue.Split('-')[0] + "'," +
            //    "'" + dd_destination.SelectedValue + "'," +
            //    "" + txt_pieces.Text + "," +
            //    "'" + dd_serviceType.SelectedValue + "'," +
            //    "'" + hd_creditClientID.Value + "'," +
            //    "" + txt_weight.Text + "," +
            //    "0.5," +
            //    "" + txt_discount.Text + "," +
            //    "'0'," +
            //    "'Rabi'," +
            //    "'" + DateTime.Now.ToString("yyyy/MM/dd HH:m:ss") + "'," +
            //    "" + totalAmount + "," +
            //    "'" + dd_origin.SelectedValue.Split('-')[1] + "',\n" +
            //    "'" + dd_origin.SelectedValue.Split('-')[0] + "',\n" +
            //    "'" + dd_conType.SelectedValue + "'," +
            //    "'" + txt_riderCode.Text + "'," +
            //    "'" + txt_accountNo.Text + "'," +
            //    "'" + dd_destCountry.SelectedValue + "'," +
            //    "'" + insurancePercentage.ToString() + "'," +
            //    "'" + DateTime.Parse(txt_bookingDate.Text).ToString("yyyy/MM/dd HH:m:ss") + "'," +
            //    "'1', '" + txt_consignerCNIC.Text + "', '" + txt_consignerAddress.Text + "', '" + txt_consignerCellNo.Text + "', '" + txt_declaredValue.Text + "',\n" +
            //    "'0', '" + txt_cellNo.Text + "', '" + txt_packageContent.Text + "', " + txt_w.Text + ", " + txt_b.Text + ", " + txt_h.Text + "" +
            //    ")";


            //string query1 = "INSERT INTO CONSIGNMENTMODIFIER ( PRICEMODIFIERID, CONSIGNMENTNUMBER, CreatedBy, CreatedON, CalculatedValue, CALCULATIONBASE)  \n";
            //for (int i = 0; i < (ViewState["dt"] as DataTable).Rows.Count - 1; i++)
            //{
            //    query1 += "SELECT " +
            //            " " + (gv_surcharges.Rows[i].Cells[0].FindControl("hd_id") as HiddenField).Value + "," +
            //            " '" + txt_consignmentNo.Text + "'," +
            //            " 'Rabi'," +
            //            " '" + DateTime.Now.ToString("yyyy/MM/dd HH:m:ss") + "'," +
            //            " '" + gv_surcharges.Rows[i].Cells[2].Text + "', '" + gv_surcharges.Rows[i].Cells[3].Text + "' \n" +
            //            " UNION ALL";

            //}
            //int count = gv_surcharges.Rows.Count;
            //query1 += " SELECT " +
            //            " " + (gv_surcharges.Rows[count - 1].Cells[0].FindControl("hd_id") as HiddenField).Value + "," +
            //            " '" + txt_consignmentNo.Text + "'," +
            //            " 'Rabi'," +
            //            " '" + DateTime.Now.ToString("yyyy/MM/dd HH:m:ss") + "'," +
            //            " " + gv_surcharges.Rows[count - 1].Cells[2].Text + ", '" + gv_surcharges.Rows[count - 1].Cells[3].Text + "'\n" +
            //            " ";
            //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            //try
            //{
            //    con.Open();
            //    SqlCommand cmd = new SqlCommand(query, con);
            //    cmd.ExecuteNonQuery();
            //    cmd = new SqlCommand(query1, con);
            //    cmd.ExecuteNonQuery();
            //    con.Close();
            //}
            //catch (Exception ex)
            //{ }

            ////query1 += " SELECT * FROM DUAL"; 
            #endregion


        }
        protected void btn_update_Click(object sender, EventArgs e)
        {
            #region Validations

            if (txt_consignmentNo.Text.Trim(' ') == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter Consignment Number')", true);
                txt_consignmentNo.Focus();
                return;
            }
            else if (txt_consignmentNo.Text.ToCharArray()[0] != '9' && rbtn_consignmentSender.SelectedValue != "4" && rbtn_consignmentSender.SelectedValue != "1")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Invalid Consignment Number')", true);
                txt_consignmentNo.Focus();
                return;
            }
            if (txt_accountNo.Text.Trim(' ') == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter Account No.')", true);
                txt_accountNo.Focus();
                return;
            }
            if (txt_consignerCellNo.Text.Trim(' ') == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter Consigner Cell Number')", true);
                txt_consignerCellNo.Focus();
                return;
            }
            if (txt_consigner.Text.Trim(' ') == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter Consigner')", true);
                txt_consigner.Focus();
                return;
            }
            if (txt_consignerCNIC.Text.Trim(' ') == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter Consignmener CNIC')", true);
                txt_consignerCNIC.Focus();
                return;
            }
            if (txt_consignee.Text.Trim(' ') == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter Consignee')", true);
                txt_consignee.Focus();
                return;
            }
            if (txt_cellNo.Text.Trim(' ') == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter Consignee Cell Number')", true);
                txt_cellNo.Focus();
                return;
            }

            if (txt_accountNo.Text == "")
            {
                if (txt_chargedamount.Text.Trim(' ') == "")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter Charge Amount')", true);
                    txt_cellNo.Focus();
                    return;
                }
            }
            if (txt_reporting_date.Text.Trim(' ') == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter Reporting Date')", true);
                txt_cellNo.Focus();
                return;
            }



            if (txt_riderCode.Text.Trim(' ') != "")
            {
                clvar.riderCode = txt_riderCode.Text;
                DataTable rider = com.ValidateRiderCode(clvar);
                if (rider != null)
                {
                    if (rider.Rows.Count > 0)
                    {

                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Invalid Rider Code')", true);
                        txt_riderCode.Text = "";
                        txt_riderCode.Focus();
                        return;
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Invalid Rider Code')", true);
                    txt_riderCode.Text = ""; chk_riderCode.Checked = false;
                    txt_riderCode.Focus();
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter Rider Code')", true);
                //txt_riderCode.Text = ""; chk_riderCode.Checked = false;
                txt_riderCode.Focus();
            }
            if (dd_destCountry.SelectedValue.Trim(' ') == "0")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Select Destination Country')", true);
                dd_destCountry.Focus();
                return;
            }
            if (dd_serviceType.SelectedValue.Trim(' ') == "0")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Select Service Type')", true);
                dd_serviceType.Focus();
                return;
            }
            if (txt_weight.Text.Trim(' ') == "" || txt_weight.Text.Trim(' ') == "0")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter Proper Weight')", true);
                txt_weight.Focus();
                return;
            }
            if (txt_consignerAddress.Text.Trim(' ') == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter Consigner Address')", true);
                txt_consignerAddress.Focus();
                return;
            }
            if (txt_consigneeAddress.Text.Trim(' ') == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter Consignee Address')", true);
                txt_consigneeAddress.Focus();
                return;
            }
            #endregion

            double totalAmount = 0;
            int cod = 0;
            try
            {
                clvar.Zone = Session["zonecode"].ToString(); //"2";//dd_origin.SelectedValue.Split('-')[1];
                clvar.expresscenter = Session["ExpressCenter"].ToString();
                clvar.destinationExpressCenterCode = "0111";
                clvar.FromZoneCode = Session["zonecode"].ToString();//dd_origin.SelectedValue.Split('-')[1];
            }
            catch (Exception ex)
            {
                Response.Redirect("~/login");
            }

            clvar.destinationCountryCode = dd_destCountry.SelectedValue;
            clvar.CustomerClientID = hd_creditClientID.Value;
            if (ViewState["serviceType"] != null)
            {
                clvar.ToZoneCode = (ViewState["serviceType"] as DataTable).Rows[0][0].ToString();
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cannot Add Consignment')", true);
                return;
            }

            clvar.ServiceTypeName = dd_serviceType.SelectedValue;
            clvar.Branch = dd_origin.SelectedValue.Split('-')[0];
            DataTable dt = com.GetClientTarrifForInternationalConsignment(clvar).Tables[0];
            double tempw = 0;
            double.TryParse(txt_weight.Text, out tempw);
            if (tempw <= 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Weight Cannot be equal or less than 0')", true);
                lbl_error.Text = "Weight Cannot be equal or less than 0";
                return;
            }
            clvar.Weight = float.Parse(txt_weight.Text);

            if (clvar.Weight == 0 || clvar.Weight == null)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Enter Weight.')", true);
                return;
            }
            if (dt != null)
            {
                if (dt.Rows.Count <= 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('No Tarrif Available. Cannot Create Consignment')", true);
                    return;
                }
                else
                {
                    double temp = 0;
                    double temp_ = 0;
                    double amount = 0;
                    string currency = "";
                    DataRow[] dr_ = dt.Select("ClIENT_ID = '" + int.Parse(hd_creditClientID.Value) + "'");
                    //DataTable tempdt = dr_.CopyToDataTable();
                    //double maxWeight = double.Parse(tempdt.Compute("MAX(TOWeight)", "").ToString());
                    //double actualWeight = double.Parse(txt_weight.Text);
                    if (dr_.Count() > 0)
                    {
                        DataTable tempdt = dr_.CopyToDataTable();
                        double maxWeight = double.Parse(tempdt.Compute("MAX(TOWeight)", "").ToString());
                        double actualWeight = double.Parse(txt_weight.Text);
                        if (actualWeight <= maxWeight)
                        {
                            foreach (DataRow dr in dr_)
                            {
                                temp = double.Parse(dr["FromWeight"].ToString());
                                temp_ = double.Parse(dr["ToWeight"].ToString());
                                if (clvar.Weight >= temp && clvar.Weight <= temp_)
                                {
                                    amount = double.Parse(dr["Price"].ToString());
                                    currency = dr["currencyCodeID"].ToString();
                                    break;
                                }

                            }
                        }
                        else
                        {
                            //foreach (DataRow dr in dr_)
                            //{
                            //    temp = double.Parse(dr["FromWeight"].ToString());
                            //    temp_ = double.Parse(dr["ToWeight"].ToString());
                            //    if (clvar.Weight > temp && clvar.Weight <= temp_)
                            //    {
                            //        amount = double.Parse(dr["Price"].ToString());
                            //        currency = dr["currencyCodeID"].ToString();
                            //        break;
                            //    }

                            //}
                            if (tempdt == null)
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Additional Factor Not Found')", true);
                                return;
                            }
                            if (tempdt.Rows.Count == 0)
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Additional Factor Not Found')", true);
                                return;
                            }
                            amount = double.Parse(dr_.CopyToDataTable().Select("TOWEIGHT = '" + maxWeight + "'")[0]["PRICE"].ToString());
                            currency = dr_.CopyToDataTable().Select("TOWEIGHT = '" + maxWeight + "'")[0]["currencyCodeID"].ToString();
                            double remainingWeight = actualWeight - maxWeight;
                            double additionalFactor = double.Parse(tempdt.Select("TOWEIGHT = '" + maxWeight + "'", "")[0]["additionalFactor"].ToString());
                            double price = double.Parse(tempdt.Select("TOWEIGHT = '" + maxWeight + "'", "")[0]["addtionalFactorDZ"].ToString());
                            if (additionalFactor == 0)
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Additional Factor Not Found')", true);
                                return;
                            }
                            double value = Math.Ceiling(remainingWeight / additionalFactor) * price;
                            amount += value;
                        }


                    }
                    else
                    {

                        dr_ = dt.Select("CLIENT_ID = '0'");
                        DataTable tempdt = dr_.CopyToDataTable();
                        double maxWeight = double.Parse(tempdt.Compute("MAX(TOWeight)", "").ToString());
                        double actualWeight = double.Parse(txt_weight.Text);
                        if (dr_.Count() > 0)
                        {
                            int count_ = 0;
                            if (actualWeight <= maxWeight)
                            {
                                foreach (DataRow dr in dr_)
                                {
                                    temp = double.Parse(dr["FromWeight"].ToString());
                                    temp_ = double.Parse(dr["ToWeight"].ToString());
                                    if (clvar.Weight >= temp && clvar.Weight <= temp_)
                                    {
                                        amount = double.Parse(dr["Price"].ToString());
                                        currency = dr["currencyCodeID"].ToString();
                                        count_++;
                                        break;
                                    }

                                }
                                if (count_ == 0)
                                {
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Could not Find Weight Bucket')", true);
                                    return;
                                }
                            }
                            else
                            {
                                //foreach (DataRow dr in dr_)
                                //{
                                //    temp = double.Parse(dr["FromWeight"].ToString());
                                //    temp_ = double.Parse(dr["ToWeight"].ToString());
                                //    if (clvar.Weight >= temp && clvar.Weight <= temp_)
                                //    {
                                //        amount = double.Parse(dr["Price"].ToString());
                                //        currency = dr["currencyCodeID"].ToString();
                                //        count_++;
                                //        break;
                                //    }

                                //}
                                if (tempdt == null)
                                {
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Additional Factor Not Found')", true);
                                    return;
                                }
                                if (tempdt.Rows.Count == 0)
                                {
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Additional Factor Not Found')", true);
                                    return;
                                }
                                amount = double.Parse(dr_.CopyToDataTable().Select("TOWEIGHT = '" + maxWeight + "'")[0]["PRICE"].ToString());
                                currency = dr_.CopyToDataTable().Select("TOWEIGHT = '" + maxWeight + "'")[0]["currencyCodeID"].ToString();
                                double remainingWeight = actualWeight - maxWeight;
                                double additionalFactor = double.Parse(tempdt.Select("TOWEIGHT = '" + maxWeight + "'", "")[0]["additionalFactor"].ToString());
                                double price = double.Parse(tempdt.Select("TOWEIGHT = '" + maxWeight + "'", "")[0]["addtionalFactorDZ"].ToString());
                                double value = Math.Ceiling(remainingWeight / additionalFactor) * price;
                                if (additionalFactor == 0)
                                {
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Additional Factor Not Found')", true);
                                    return;
                                }
                                amount += value;
                            }


                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('No Tarrif Available. Cannot Create Consignment')", true);
                            return;
                        }
                    }
                    clvar.BaseCurrency = currency;
                    if (clvar.BaseCurrency != "2" && clvar.BaseCurrency != "")
                    {
                        DataTable dtCurrencyRates = com.GetCurrencyConversionRates(clvar).Tables[0];
                        if (dtCurrencyRates != null)
                        {
                            if (dtCurrencyRates.Rows.Count > 0)
                            {
                                totalAmount = double.Parse(dtCurrencyRates.Rows[0]["Rate"].ToString()) * amount;
                                clvar.TotalAmount = totalAmount;
                            }
                            else
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Currency Rates Not Available. Cannot Create Consignment.')", true);
                                return;
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Currency Rates Not Available. Cannot Create Consignment.')", true);
                            return;
                        }
                    }
                    else
                    {
                        totalAmount = amount;
                        clvar.TotalAmount = totalAmount;
                    }

                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('No Tarrif Available. Cannot Create Consignment')", true);
                return;
            }

            clvar.consignmentNo = txt_consignmentNo.Text;
            clvar.Consigner = txt_consigner.Text;
            clvar.Consignee = txt_consignee.Text;
            clvar.CouponNo = txt_cupon.Text;
            //   clvar.Customertype = int.Parse(rbtn_customerType.SelectedValue);
            clvar.origin = dd_origin.SelectedValue.Split('-')[0];
            clvar.Destination = dd_destination.SelectedValue;
            clvar.pieces = int.Parse(txt_pieces.Text);
            clvar.ServiceTypeName = dd_serviceType.SelectedValue;
            clvar.CustomerClientID = hd_creditClientID.Value;
            clvar.Con_Type = int.Parse(dd_conType.SelectedValue);
            clvar.Unit = 0;
            //  clvar.ChargeAmount = int.Parse(txt_chargedamount.Text);

            //clvar.Discount = int.Parse(txt_discount.Text);
            int d = 0;
            int.TryParse(txt_discount.Text, out d);
            clvar.Discount = d;
            //clvar.isCod = false;

            clvar.ConsigneeAddress = txt_consigneeAddress.Text;
            clvar.createdBy = "99";
            clvar.status = 1;


            clvar.ConsignerAddress = txt_consignerAddress.Text;
            clvar.RiderCode = txt_riderCode.Text;
            double w = 0;
            double.TryParse(txt_w.Text, out w);
            clvar.width = w;//double.Parse(txt_w.Text);
            double b = 0;
            double.TryParse(txt_b.Text, out b);
            clvar.breadth = b;// double.Parse(txt_b.Text);
            double h = 0;
            double.TryParse(txt_h.Text, out h);

            clvar.height = h;// double.Parse(txt_h.Text);
            clvar.PakageContents = txt_packageContent.Text;
            clvar.ConsigneeCell = txt_cellNo.Text;
            clvar.ConsignerCell = txt_consignerCellNo.Text;
            clvar.ConsignerCNIC = txt_consignerCNIC.Text;
            clvar.AccountNo = txt_accountNo.Text;
            clvar.destinationCountryCode = dd_destCountry.SelectedValue;
            float declaredValue = 0;
            if (chk_insurance.Checked)
            {
                clvar.Declaredvalue = declaredValue = float.Parse(txt_declaredValue.Text);
                clvar.insuarancePercentage = 2.5f;
                clvar.isInsured = true;

                if (clvar.Declaredvalue <= 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Enter Declared Value')", true);
                    return;
                }

            }
            else
            {
                clvar.Declaredvalue = 0;
                clvar.insuarancePercentage = 0;
                clvar.isInsured = false;
            }
            clvar.Bookingdate = DateTime.Parse(txt_bookingDate.Text);

            clvar.originBrName = dd_origin.SelectedItem.Text;
            clvar.customerName = txt_consigner.Text;
            //clvar.serviceTypeId = dd_serviceType.SelectedValue;
            clvar.originId = dd_origin.SelectedValue.Split('-')[0];
            clvar.isExpUser = "1";

            DataSet BranchGSTInformation = con.BranchGSTInformation(clvar);
            string gst = "";
            if (BranchGSTInformation.Tables[0].Rows.Count != 0)
            {
                gst = BranchGSTInformation.Tables[0].Rows[0]["gst"].ToString();
            }
            clvar.gst = (double.Parse(gst) / 100) * totalAmount;
            clvar.consignerAccountNo = txt_accountNo.Text;



            DataTable dtc = new DataTable();
            dtc.Columns.AddRange(new DataColumn[12]
            {
            new DataColumn("PriceModifierID", typeof(int)),
            new DataColumn("ConsignmentNumber", typeof(Int64)),
            new DataColumn("CreatedBy",typeof(string)),
            new DataColumn("CreatedOn", typeof(DateTime)),
            new DataColumn("ModifiedBy", typeof(string)),
            new DataColumn("ModifiedOn", typeof(DateTime)),
            new DataColumn("ModifiedCalculationValue", typeof(int)),
            new DataColumn("CalculatedValue", typeof(float)),
            new DataColumn("CalculatedGST", typeof(float)),
            new DataColumn("CalculationBase", typeof(int)),
            new DataColumn("isTaxable", typeof(bool)),
            new DataColumn("SortOrder", typeof(int))

            });
            dtc.AcceptChanges();
            DataTable pm = ViewState["PriceModifiers"] as DataTable;
            DataTable surcharges = ViewState["dt"] as DataTable;


            int count = 1;
            double tempTotalAmount = totalAmount;
            double tempTotalGst = (double.Parse(gst) / 100) * totalAmount;
            double tempGrandTotalAmount = 0;
            double insurance = 0;
            insurance = declaredValue * 0.025;
            tempTotalGst += insurance * (double.Parse(gst) / 100);
            tempTotalAmount += insurance;
            double tempt = totalAmount;
            foreach (DataRow dr in surcharges.Rows)
            {
                DataRow dr_ = dtc.NewRow();
                dr_["pricemodifierid"] = int.Parse(pm.Select("id = '" + dr["id"].ToString() + "'")[0]["ID"].ToString());
                dr_["ConsignmentNumber"] = Int64.Parse(txt_consignmentNo.Text);
                dr_["CreatedBy"] = "TEST";
                dr_["CreatedOn"] = DateTime.Now;
                dr_["ModifiedBy"] = "";
                dr_["ModifiedOn"] = DBNull.Value;
                if (dr["BASE"].ToString() == "2")
                {
                    dr_["CalculatedValue"] = (float.Parse(dr["PValue"].ToString()) / 100) * tempt;
                    dr_["CalculatedGst"] = ((float.Parse(dr["PValue"].ToString()) / 100) * tempt) * (float.Parse(BranchGSTInformation.Tables[0].Rows[0]["GST"].ToString()) / 100);
                    tempTotalAmount += (float.Parse(dr["PValue"].ToString()) / 100) * tempt;
                    tempTotalGst += ((float.Parse(dr["PValue"].ToString()) / 100) * tempt) * (float.Parse(BranchGSTInformation.Tables[0].Rows[0]["GST"].ToString()) / 100);
                    tempt = tempt + (float.Parse(dr["PValue"].ToString()) / 100) * tempt;
                    //dr_["CalculatedGst"] = (float.Parse(BranchGSTInformation.Tables[0].Rows[0]["GST"].ToString()) / 100) * float.Parse(dr["Pvalue"].ToString());
                }
                else
                {
                    dr_["CalculatedValue"] = float.Parse(dr["Pvalue"].ToString()); //float.Parse(pm.Select("id = '" + dr["id"].ToString() + "'")[0]["ActualValue"].ToString());

                    dr_["CalculatedGst"] = (float.Parse(BranchGSTInformation.Tables[0].Rows[0]["GST"].ToString()) / 100) * float.Parse(dr["Pvalue"].ToString());
                    tempTotalAmount += float.Parse(dr["Pvalue"].ToString());
                    tempTotalGst += (float.Parse(BranchGSTInformation.Tables[0].Rows[0]["GST"].ToString()) / 100) * float.Parse(dr["Pvalue"].ToString());
                    tempt = tempt + float.Parse(dr["Pvalue"].ToString());
                }

                dr_["ModifiedCalculationValue"] = float.Parse(dr["Pvalue"].ToString());//float.Parse(pm.Select("id = '" + dr["id"].ToString() + "'")[0]["CalculationValue"].ToString());
                                                                                       //dr_["CalculatedGst"] = (float.Parse(BranchGSTInformation.Tables[0].Rows[0]["GST"].ToString()) / 100) * float.Parse(dr["Pvalue"].ToString());
                dr_["CalculationBase"] = int.Parse(dr["BASE"].ToString());
                dr_["isTaxable"] = 1;
                dr_["SortOrder"] = count++;
                dtc.Rows.Add(dr_);
                dtc.AcceptChanges();
            }

            tempGrandTotalAmount = tempTotalAmount + tempTotalGst;


            //if (sender.Equals(btn_Validate))
            //{
            //    btn_save.Visible = true;
            //    //btn_Validate.Visible = false;
            //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Total Amount      " + tempTotalAmount + ". \\nTotalGst          " + tempTotalGst + ".\\nGrand Total       " + tempGrandTotalAmount + "')", true);
            //    return;
            //}


            try
            {
                clvar.expresscenter = HttpContext.Current.Session["ExpressCenter"].ToString();
                clvar.OriginExpressCenterCode = ecCode.Value;
                clvar.destinationExpressCenterCode = "0111";
            }
            catch (Exception)
            {

                Response.Redirect("~/login");
            }


            if (clvar.isCod == false)
            {
                clvar.StateID = "0";
            }

            if (clvar.isCod == true)
            {
                clvar.StateID = "1";
            }


            clvar.gst = Math.Round(tempTotalGst, 2);

            if (txt_chargedamount.Text == "")
            {
                clvar.ChargeAmount = 0;
            }
            else
            {
                clvar.ChargeAmount = Double.Parse(txt_chargedamount.Text.ToString());
            }
            //  clvar.Day = txt_reporting_date.Text;

            DateTime myDate = DateTime.Parse(txt_reporting_date.Text);
            clvar.expressionDeliveryDateTime = myDate;

            clvar._CityCode = dd_destination.SelectedItem.Text;
            clvar.cnScreenId = "8";
            string error = con.Update_Consignment_(clvar);
            if (error != "" && error != null)
            {
                lbl_error.Text = error;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cannot Create Consignment. ')", true);
                return;
            }
            clvar.consignmentNo = txt_consignmentNo.Text;

            clvar.Company = rbtn_consignmentSender.SelectedValue;
            error = con.AddConsignmentSender(clvar);
            if (error != "" && error != null)
            {
                lbl_error.Text = error;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cannot Create Consignment. ')", true);
                return;
            }


            string error_ = con.Add_ConsignmentModifier(dtc, clvar);
            if (error_ != "")
            {
                error = "";
                error = con.DeleteConsignment(clvar);
                if (error != "")
                {

                }
                else
                {
                    lbl_error.Text = error;
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Consignment Could not Save. Please Contact IT Support.')", true);
                    return;
                }
                lbl_error.Text = error;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Could Not Add Consignment Modifier. \n  Please Contact I.T. Support')", true);
                return;
            }
            int i = con.Add_InternationalConsignmentDetail(clvar);
            if (i == 0)
            {
                error = "";
                error = con.DeleteConsignment(clvar);
                if (error != "")
                {

                }
                else
                {
                    lbl_error.Text = error;
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Consignment Could not Save. Please Contact IT Support.')", true);
                    return;
                }
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Could Not Add Consignment Details. Please Contact I.T. Support')", true);
                return;
            }

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Consignment Successfully Updated.')", true);
            ResetAll();
            return;





            #region commented
            //double totalAmount = 0;
            //double insurancePercentage = 0;
            //if (chk_insurance.Checked)
            //{
            //    insurancePercentage = 2.5;
            //}
            //string query = "INSERT INTO CONSIGNMENT (CONSIGNMENTNUMBER,\n" +
            //    " CONSIGNER,\n" +
            //    " CONSIGNEE,\n" +
            //    " COUPONNUMBER, \n" +
            //    " CUSTOMERTYPE, \n" +
            //    " ORGIN, \n" +
            //    " DESTINATION, \n" +
            //    " PIECES, \n" +
            //    " SERVICETYPENAME, \n" +
            //    " CREDITCLIENTID, \n" +
            //    " WEIGHT, \n" +
            //    " WEIGHTUNIT, \n" +
            //    " DISCOUNT, \n" +
            //    " COD, \n" +
            //    " CREATEDBY, \n" +
            //    " CREATEDON, \n" +
            //    " TOTALAMOUNT, \n" +
            //    " ZONECODE, \n" +
            //    " BRANCHCODE, \n" +
            //    " CONSIGNMENTTYPEID, \n" +
            //    " RIDERCODE, \n" +
            //    " CONSIGNERACCOUNTNO, \n" +
            //    " DESTINATIONCOUNTRYCODE, \n" +
            //    " INSUaRANCEPERCENTAGE, \n" +
            //    " BOOKINGDATE, \n" +
            //    " ISPRICECOMPUTED, CONSIGNERCNICNO, shipperAddress, consignercellno, decalaredvalue, \n" +
            //    "isapproved, consigneephoneno, pakageContents, width, breadth, height)\n" +
            //    "VALUES\n" +
            //    "(\n" +
            //    "'" + txt_consignmentNo.Text + "',\n" +
            //    "'" + txt_consigner.Text + "'," +
            //    "'" + txt_consignee.Text + "'," +
            //    "'" + txt_cupon.Text + "'," +
            //    "" + rbtn_customerType.SelectedValue + "," +
            //    "'" + dd_origin.SelectedValue.Split('-')[0] + "'," +
            //    "'" + dd_destination.SelectedValue + "'," +
            //    "" + txt_pieces.Text + "," +
            //    "'" + dd_serviceType.SelectedValue + "'," +
            //    "'" + hd_creditClientID.Value + "'," +
            //    "" + txt_weight.Text + "," +
            //    "0.5," +
            //    "" + txt_discount.Text + "," +
            //    "'0'," +
            //    "'Rabi'," +
            //    "'" + DateTime.Now.ToString("yyyy/MM/dd HH:m:ss") + "'," +
            //    "" + totalAmount + "," +
            //    "'" + dd_origin.SelectedValue.Split('-')[1] + "',\n" +
            //    "'" + dd_origin.SelectedValue.Split('-')[0] + "',\n" +
            //    "'" + dd_conType.SelectedValue + "'," +
            //    "'" + txt_riderCode.Text + "'," +
            //    "'" + txt_accountNo.Text + "'," +
            //    "'" + dd_destCountry.SelectedValue + "'," +
            //    "'" + insurancePercentage.ToString() + "'," +
            //    "'" + DateTime.Parse(txt_bookingDate.Text).ToString("yyyy/MM/dd HH:m:ss") + "'," +
            //    "'1', '" + txt_consignerCNIC.Text + "', '" + txt_consignerAddress.Text + "', '" + txt_consignerCellNo.Text + "', '" + txt_declaredValue.Text + "',\n" +
            //    "'0', '" + txt_cellNo.Text + "', '" + txt_packageContent.Text + "', " + txt_w.Text + ", " + txt_b.Text + ", " + txt_h.Text + "" +
            //    ")";


            //string query1 = "INSERT INTO CONSIGNMENTMODIFIER ( PRICEMODIFIERID, CONSIGNMENTNUMBER, CreatedBy, CreatedON, CalculatedValue, CALCULATIONBASE)  \n";
            //for (int i = 0; i < (ViewState["dt"] as DataTable).Rows.Count - 1; i++)
            //{
            //    query1 += "SELECT " +
            //            " " + (gv_surcharges.Rows[i].Cells[0].FindControl("hd_id") as HiddenField).Value + "," +
            //            " '" + txt_consignmentNo.Text + "'," +
            //            " 'Rabi'," +
            //            " '" + DateTime.Now.ToString("yyyy/MM/dd HH:m:ss") + "'," +
            //            " '" + gv_surcharges.Rows[i].Cells[2].Text + "', '" + gv_surcharges.Rows[i].Cells[3].Text + "' \n" +
            //            " UNION ALL";

            //}
            //int count = gv_surcharges.Rows.Count;
            //query1 += " SELECT " +
            //            " " + (gv_surcharges.Rows[count - 1].Cells[0].FindControl("hd_id") as HiddenField).Value + "," +
            //            " '" + txt_consignmentNo.Text + "'," +
            //            " 'Rabi'," +
            //            " '" + DateTime.Now.ToString("yyyy/MM/dd HH:m:ss") + "'," +
            //            " " + gv_surcharges.Rows[count - 1].Cells[2].Text + ", '" + gv_surcharges.Rows[count - 1].Cells[3].Text + "'\n" +
            //            " ";
            //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            //try
            //{
            //    con.Open();
            //    SqlCommand cmd = new SqlCommand(query, con);
            //    cmd.ExecuteNonQuery();
            //    cmd = new SqlCommand(query1, con);
            //    cmd.ExecuteNonQuery();
            //    con.Close();
            //}
            //catch (Exception ex)
            //{ }

            ////query1 += " SELECT * FROM DUAL"; 
            #endregion


        }


        protected void gv_surcharges_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Del")
            {
                string id = e.CommandArgument.ToString();
                DataTable dt = ViewState["dt"] as DataTable;

                DataRow[] dr = dt.Select("ID = '" + id + "'");
                dt.Rows.Remove(dr[0]);

                dt.AcceptChanges();
                gv_surcharges.DataSource = dt;
                gv_surcharges.DataBind();
                ViewState["dt"] = dt;
            }

            if (e.CommandName == "ed")
            {
                string id = e.CommandArgument.ToString();
                DataTable dt = ViewState["dt"] as DataTable;
                DataRow[] dr = dt.Select("ID= '" + id + "'");
                foreach (GridViewRow row in gv_surcharges.Rows)
                {
                    if ((row.Cells[0].FindControl("hd_id") as HiddenField).Value == id)
                    {
                        dr[0]["PValue"] = double.Parse((row.FindControl("txt_value") as RadNumericTextBox).Value.ToString());
                        dt.AcceptChanges();
                    }
                }
                gv_surcharges.DataSource = dt;
                gv_surcharges.DataBind();
                ViewState["dt"] = dt;
            }
        }
        protected void dd_conType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dd_conType.SelectedValue == "13")
            {
                lbl_cupon.Text = "Flyer No.";
                txt_cupon.Enabled = true;
            }
            else
            {
                lbl_cupon.Text = "Cupon No.";
                txt_cupon.Enabled = false;
            }
            dd_destCountry.Focus();
        }
        protected void chk_insurance_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_insurance.Checked)
            {
                txt_declaredValue.Enabled = true;
            }
            else
            {
                txt_declaredValue.Enabled = false;
            }
        }
        protected void dd_serviceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dd_serviceType.SelectedValue != "0")
            {


                clvar.destinationCountryCode = dd_destCountry.SelectedValue;
                clvar.ServiceTypeName = dd_serviceType.SelectedValue;
                DataTable dt = com.CheckServiceAvailability(clvar).Tables[0];
                //clvar.serviceTypeId = "0";
                if (dt != null)
                {
                    if (dt.Rows.Count <= 0)
                    {
                        ViewState["serviceType"] = null;
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "confirm('This Service is not Available in selected Country')", true);
                        dd_serviceType.Focus();
                        isValid = false;
                        return;
                    }
                    else
                    {
                        ViewState["serviceType"] = dt;
                        txt_weight.Focus();
                    }
                }
            }
            else
            {
                isValid = false;
                Alert("Select Service Type");
            }
        }
        protected void dd_destCountry_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (dd_serviceType.SelectedValue == "0")
            {
                dd_serviceType.Focus();
                return;
            }
            if (dd_destCountry.SelectedValue == "0")
            {
                isValid = false;
                Alert("Select Destination Country");
                return;
            }
            clvar.destinationCountryCode = dd_destCountry.SelectedValue;
            clvar.ServiceTypeName = dd_serviceType.SelectedValue;
            DataTable dt = com.CheckServiceAvailability(clvar).Tables[0];

            if (dt != null)
            {
                if (dt.Rows.Count <= 0)
                {
                    ViewState["serviceType"] = null;
                    dd_serviceType.Focus();
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('This Service is not Available in selected Country')", true);
                    isValid = false;
                    return;
                }
                else
                {
                    ViewState["serviceType"] = dt;
                }
            }
            dd_serviceType.Focus();
        }
        protected void dd_destination_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        protected void chk_riderCode_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_riderCode.Checked)
            {
                if (txt_riderCode.Text.Trim(' ') == "")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter a valid Rider Code First')", true);
                    return;
                }
                else
                {
                    ViewState["riderCode"] = txt_riderCode.Text;
                }
            }
            else
            {
                ViewState["riderCode"] = null;
                txt_riderCode.Text = ""; chk_riderCode.Checked = false;
            }
        }
        protected void ResetAll()
        {
            txt_accountNo.Text = "";
            txt_consignmentNo.Text = "";
            txt_consigner.Text = "";
            txt_cellNo.Text = "";
            txt_consignee.Text = "";
            txt_consigneeAddress.Text = "";
            txt_consigneeAttn.Text = "";
            txt_consigner.Text = "";
            txt_consignerAddress.Text = "";
            txt_consignerCellNo.Text = "";
            txt_consignerCNIC.Text = "";
            txt_cupon.Text = "0";
            txt_description.Text = "";
            txt_discount.Text = "";
            txt_h.Text = "0";
            txt_weight.Text = "0.5";
            txt_w.Text = "0";
            txt_insurance.Text = "2.5";
            txt_packageContent.Text = "";
            txt_pieces.Text = "1";
            //dd_origin.SelectedValue = "";
            if (!(chk_riderCode.Checked))
            {
                txt_riderCode.Text = "";
            }
            DataTable dt = ViewState["dt"] as DataTable;
            dt.Clear();
            ViewState["dt"] = dt;
            gv_surcharges.DataSource = null;
            gv_surcharges.DataBind();
            dd_destCountry.ClearSelection();
            dd_serviceType.ClearSelection();
            btn_save.Visible = false;
            txt_bookingDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            txt_reporting_date.Text = DateTime.Now.ToString("yyyy-MM-dd");
            //btn_Validate.Visible = true;
            txt_chargedamount.Text = "";
            txt_totalAmount.Text = "";
            txt_gst.Text = "";
            txt_consignmentNo.Focus();
            dd_name.ClearSelection();

        }
        protected void btn_reset_Click(object sender, EventArgs e)
        {
            ResetAll();
        }
        protected void rbtn_consignmentSender_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetServiceType();
            if (rbtn_consignmentSender.SelectedValue == "1")
            {
                txt_consignmentNo.MaxLength = 11;
                //txt_consignmentNo.Mask = "###########";
            }
            else
            {
                txt_consignmentNo.MaxLength = 12;
                //txt_consignmentNo.Mask = "############";
            }
        }

        protected DataTable MinimumDate(Cl_Variables clvar)
        {
            string sqlString = "select MAX(DateTime) DateAllowed from Mnp_Account_DayEnd where Branch = '" + clvar.Branch + "' and doc_type = 'A'";
            SqlConnection con = new SqlConnection(clvar.Strcon());
            DataTable dt = new DataTable();
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


        public DataSet GetClientTarrifForInternationalConsignment(Cl_Variables clvar)
        {
            string query = "select * FROM tempClientTariff where ISINTLTARIFf = '1' AND ToZoneCode = '" + clvar.ToZoneCode + "' and BranchCode = '" + clvar.Branch + "' and ServiceID = '" + clvar.ServiceTypeName + "' and chkdeleted = 'False' --and client_id = '" + clvar.CustomerClientID + "'";
            DataSet ds = new DataSet();
            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                orcl.Close();
            }
            catch (Exception ex)
            { }

            return ds;
        }
        public DataSet GetClientTarrifForInternationalConsignmentForZero(Cl_Variables clvar)
        {
            string query = "select * FROM tempClientTariff where ISINTLTARIFf = '1' AND ToZoneCode = '" + clvar.ToZoneCode + "' and branchCode = '" + clvar.Branch + "' and client_Id = '0' and ServiceID = '" + clvar.ServiceTypeName + "' and chkdeleted = 'False' --and client_id = '" + clvar.CustomerClientID + "'";
            DataSet ds = new DataSet();
            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                orcl.Close();
            }
            catch (Exception ex)
            { }

            return ds;
        }


        public DataTable ValidateRiderCode(Cl_Variables clvar)
        {
            string query = "SELECT * FROM RIDERS WHERE BRANCHID = '" + HttpContext.Current.Session["BranchCode"].ToString() + "' and expressCenterId = '0111' AND STATUS = '1'";
            query = "SELECT * FROM RIDERS r where r.riderCode = '" + txt_riderCode.Text.Trim() + "' and r.branchID = '" + HttpContext.Current.Session["BranchCode"].ToString() + "' and r.status = '1'";
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


        public string Add_Consignment_Validation(Cl_Variables obj)
        {
            clvar.Error = "";
            int IsUnique = 0;
            SqlConnection sqlcon = new SqlConnection(obj.Strcon());
            //    obj.Call_Log_ID = int.Parse(Get_maxValue()) + 1;
            try
            {
                sqlcon.Open();
                SqlCommand sqlcmd = new SqlCommand("MNP_InsertConsignment_Temp_Validation", sqlcon);
                // SqlCommand sqlcmd = new SqlCommand("MNP_InsertConsignment_Temp", sqlcon);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                // SqlParameter SParam = new SqlParameter("ReturnValue", SqlDbType.Int);
                // SParam.Direction = ParameterDirection.ReturnValue;
                //
                sqlcmd.Parameters.AddWithValue("@consignmentNumber", obj.consignmentNo);
                sqlcmd.Parameters.AddWithValue("@consigner", obj.Consigner);
                sqlcmd.Parameters.AddWithValue("@consignee", obj.Consignee);
                sqlcmd.Parameters.AddWithValue("@couponNumber", obj.CouponNo);
                sqlcmd.Parameters.AddWithValue("@customerType", obj.Customertype);
                sqlcmd.Parameters.AddWithValue("@orgin", obj.origin);
                sqlcmd.Parameters.AddWithValue("@destination", obj.Destination);
                sqlcmd.Parameters.AddWithValue("@pieces", obj.pieces);
                sqlcmd.Parameters.AddWithValue("@serviceTypeName", obj.ServiceTypeName);
                sqlcmd.Parameters.AddWithValue("@creditClientId", obj.CustomerClientID);
                sqlcmd.Parameters.AddWithValue("@consignmentTypeId", obj.Con_Type);
                sqlcmd.Parameters.AddWithValue("@weight", obj.Weight);
                sqlcmd.Parameters.AddWithValue("@weightUnit", obj.Unit);
                sqlcmd.Parameters.AddWithValue("@discount", obj.Discount);
                sqlcmd.Parameters.AddWithValue("@cod", obj.isCod);
                sqlcmd.Parameters.AddWithValue("@address", obj.ConsigneeAddress);
                sqlcmd.Parameters.AddWithValue("@createdBy", obj.createdBy);
                sqlcmd.Parameters.AddWithValue("@status", obj.status);
                sqlcmd.Parameters.AddWithValue("@totalAmount", obj.TotalAmount);
                sqlcmd.Parameters.AddWithValue("@zoneCode", obj.Zone);
                sqlcmd.Parameters.AddWithValue("@branchCode", obj.origin);
                sqlcmd.Parameters.AddWithValue("@expressCenterCode", obj.expresscenter);
                sqlcmd.Parameters.AddWithValue("@isCreatedFromMMU", obj.isCreatedFromMMUs);
                sqlcmd.Parameters.AddWithValue("@deliveryType", obj.deliveryType);
                sqlcmd.Parameters.AddWithValue("@remarks", obj.Remarks);
                sqlcmd.Parameters.AddWithValue("@shipperAddress", obj.ConsignerAddress);
                sqlcmd.Parameters.AddWithValue("@riderCode", obj.RiderCode);
                sqlcmd.Parameters.AddWithValue("@gst", obj.gst);
                sqlcmd.Parameters.AddWithValue("@width", obj.width);
                sqlcmd.Parameters.AddWithValue("@breadth", obj.breadth);
                sqlcmd.Parameters.AddWithValue("@height", obj.height);
                sqlcmd.Parameters.AddWithValue("@PakageContents", obj.PakageContents);
                sqlcmd.Parameters.AddWithValue("@expressionDeliveryDateTime", obj.expressionDeliveryDateTime);
                sqlcmd.Parameters.AddWithValue("@expressionGreetingCard", obj.expressionGreetingCard);
                sqlcmd.Parameters.AddWithValue("@expressionMessage", obj.expressionMessage);
                sqlcmd.Parameters.AddWithValue("@consigneePhoneNo", obj.ConsigneeCell);
                sqlcmd.Parameters.AddWithValue("@expressionconsignmentRefNumber", obj.expressionconsignmentRefNumber);
                sqlcmd.Parameters.AddWithValue("@otherCharges", obj.Othercharges);
                sqlcmd.Parameters.AddWithValue("@routeCode", obj.routeCode);
                sqlcmd.Parameters.AddWithValue("@docPouchNo", obj.docPouchNo);
                sqlcmd.Parameters.AddWithValue("@consignerPhoneNo", obj.ConsignerPhone);
                sqlcmd.Parameters.AddWithValue("@consignerCellNo", obj.ConsignerCell);
                sqlcmd.Parameters.AddWithValue("@consignerCNICNo", obj.ConsignerCNIC);
                sqlcmd.Parameters.AddWithValue("@consignerAccountNo", obj.consignerAccountNo);
                sqlcmd.Parameters.AddWithValue("@consignerEmail", obj.consignerEmail);
                sqlcmd.Parameters.AddWithValue("@docIsHomeDelivery", obj.docIsHomeDelivery);
                sqlcmd.Parameters.AddWithValue("@cutOffTime", obj.cutOffTime);
                sqlcmd.Parameters.AddWithValue("@destinationCountryCode", obj.destinationCountryCode);
                sqlcmd.Parameters.AddWithValue("@decalaredValue", obj.Declaredvalue);
                sqlcmd.Parameters.AddWithValue("@insuarancePercentage", obj.insuarancePercentage);
                sqlcmd.Parameters.AddWithValue("@consignmentScreen", 8);
                sqlcmd.Parameters.AddWithValue("@isInsured", obj.isInsured);
                sqlcmd.Parameters.AddWithValue("@isReturned", obj.isReturned);
                sqlcmd.Parameters.AddWithValue("@consigneeCNICNo", obj.ConsigneeCNIC);
                sqlcmd.Parameters.AddWithValue("@cutOffTimeShift", obj.cutOffTimeShift);
                sqlcmd.Parameters.AddWithValue("@bookingDate", obj.Bookingdate);
                sqlcmd.Parameters.AddWithValue("@cnClientType", obj.cnClientType);
                sqlcmd.Parameters.AddWithValue("@destinationExpressCenterCode", obj.destinationExpressCenterCode);
                sqlcmd.Parameters.AddWithValue("@dayType", 2);
                sqlcmd.Parameters.AddWithValue("@originExpressCenter", obj.expresscenter);
                sqlcmd.Parameters.AddWithValue("@receivedFromRider", obj.receivedFromRider);
                sqlcmd.Parameters.AddWithValue("@chargedAmount", obj.ChargeAmount);
                sqlcmd.Parameters.AddWithValue("@cnState", obj.cnState);
                sqlcmd.Parameters.AddWithValue("@cardRefNo", obj.cardRefNo);
                sqlcmd.Parameters.AddWithValue("@manifestNo", obj.manifestNo);
                sqlcmd.Parameters.AddWithValue("@manifestId", obj.manifestId);
                sqlcmd.Parameters.AddWithValue("@originBrName", obj.originBrName);
                sqlcmd.Parameters.AddWithValue("@orderRefNo", obj.orderRefNo);
                sqlcmd.Parameters.AddWithValue("@customerName", obj.customerName);
                sqlcmd.Parameters.AddWithValue("@productTypeId", obj.productTypeId);
                sqlcmd.Parameters.AddWithValue("@productDescription", obj.productDescription);
                sqlcmd.Parameters.AddWithValue("@chargeCODAmount", obj.chargeCODAmount);
                sqlcmd.Parameters.AddWithValue("@codAmount", obj.codAmount);
                //sqlcmd.Parameters.AddWithValue("@isPM", obj.isPM);
                //sqlcmd.Parameters.AddWithValue("@LstModifiersCN", obj.LstModifiersCNt);
                sqlcmd.Parameters.AddWithValue("@serviceTypeId", obj.serviceTypeId);
                sqlcmd.Parameters.AddWithValue("@originId", obj.originId);
                sqlcmd.Parameters.AddWithValue("@destId", obj.destId);
                sqlcmd.Parameters.AddWithValue("@cnTypeId", obj.cnTypeId);
                sqlcmd.Parameters.AddWithValue("@cnOperationalType", obj.cnOperationalType);
                sqlcmd.Parameters.AddWithValue("@cnScreenId", obj.cnScreenId);
                sqlcmd.Parameters.AddWithValue("@cnStatus", obj.cnStatus);
                sqlcmd.Parameters.AddWithValue("@flagSendConsigneeMsg", obj.flagSendConsigneeMsg);
                sqlcmd.Parameters.AddWithValue("@isExpUser", obj.isExpUser);
                sqlcmd.Parameters.AddWithValue("@isInt", obj.isExpUser);

                //SqlParameter P_XCode = new SqlParameter("@XCode", SqlDbType.VarChar, 256);
                //P_XCode.Direction = ParameterDirection.Output;

                //  sqlcmd.Parameters.Add(P_XCode);
                // sqlcmd.Parameters.Add(SParam);
                sqlcmd.ExecuteNonQuery();
                //IsUnique = Int32.Parse(SParam.Value.ToString());
                // obj.XCode = obj.consignmentNo;
                sqlcon.Close();
            }
            catch (Exception Err)
            {
                clvar.Error = Err.Message.ToString();
                /// IsUnique = 1;
            }
            finally
            { }
            //return IsUnique;
            return clvar.Error;
        }

        public DataSet Add_OcsInternationalValidation(Cl_Variables clvar)
        {
            DataSet ds = new DataSet();

            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand("SP_UpdateCNPrice_Int_Validation", orcl);
                orcd.Parameters.AddWithValue("@ConsignmentNo", clvar.consignmentNo);
                orcd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                orcl.Close();
            }
            catch (Exception)
            { }

            return ds;
        }

        public void AlertMessage(string message)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + message + "')", true);
            lbl_error.Text = message;
        }
        public string Add_Consignment(Cl_Variables obj)
        {
            clvar.Error = "";
            int IsUnique = 0;
            SqlConnection sqlcon = new SqlConnection(obj.Strcon());
            //    obj.Call_Log_ID = int.Parse(Get_maxValue()) + 1;
            try
            {
                sqlcon.Open();
                SqlCommand sqlcmd = new SqlCommand("MNP_InsertConsignment_Temp2", sqlcon);
                // SqlCommand sqlcmd = new SqlCommand("MNP_InsertConsignment_Temp", sqlcon);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                // SqlParameter SParam = new SqlParameter("ReturnValue", SqlDbType.Int);
                // SParam.Direction = ParameterDirection.ReturnValue;
                //
                sqlcmd.Parameters.AddWithValue("@consignmentNumber", obj.consignmentNo);
                sqlcmd.Parameters.AddWithValue("@consigner", obj.Consigner);
                sqlcmd.Parameters.AddWithValue("@consignee", obj.Consignee);
                sqlcmd.Parameters.AddWithValue("@couponNumber", obj.CouponNo);
                sqlcmd.Parameters.AddWithValue("@customerType", obj.Customertype);
                sqlcmd.Parameters.AddWithValue("@orgin", obj.origin);
                sqlcmd.Parameters.AddWithValue("@destination", obj.Destination);
                sqlcmd.Parameters.AddWithValue("@pieces", obj.pieces);
                sqlcmd.Parameters.AddWithValue("@serviceTypeName", obj.ServiceTypeName);
                sqlcmd.Parameters.AddWithValue("@creditClientId", obj.CustomerClientID);
                sqlcmd.Parameters.AddWithValue("@consignmentTypeId", obj.Con_Type);
                sqlcmd.Parameters.AddWithValue("@weight", obj.Weight);
                sqlcmd.Parameters.AddWithValue("@weightUnit", obj.Unit);
                sqlcmd.Parameters.AddWithValue("@discount", obj.Discount);
                sqlcmd.Parameters.AddWithValue("@cod", obj.isCod);
                sqlcmd.Parameters.AddWithValue("@address", obj.ConsigneeAddress);
                sqlcmd.Parameters.AddWithValue("@createdBy", obj.createdBy);
                sqlcmd.Parameters.AddWithValue("@status", obj.status);
                sqlcmd.Parameters.AddWithValue("@totalAmount", obj.TotalAmount);
                sqlcmd.Parameters.AddWithValue("@zoneCode", obj.Zone);
                sqlcmd.Parameters.AddWithValue("@branchCode", obj.origin);
                sqlcmd.Parameters.AddWithValue("@expressCenterCode", obj.expresscenter);
                sqlcmd.Parameters.AddWithValue("@isCreatedFromMMU", obj.isCreatedFromMMUs);
                sqlcmd.Parameters.AddWithValue("@deliveryType", obj.deliveryType);
                sqlcmd.Parameters.AddWithValue("@remarks", obj.Remarks);
                sqlcmd.Parameters.AddWithValue("@shipperAddress", obj.ConsignerAddress);
                sqlcmd.Parameters.AddWithValue("@riderCode", obj.RiderCode);
                sqlcmd.Parameters.AddWithValue("@gst", obj.gst);
                sqlcmd.Parameters.AddWithValue("@width", obj.width);
                sqlcmd.Parameters.AddWithValue("@breadth", obj.breadth);
                sqlcmd.Parameters.AddWithValue("@height", obj.height);
                sqlcmd.Parameters.AddWithValue("@PakageContents", obj.PakageContents);
                sqlcmd.Parameters.AddWithValue("@expressionDeliveryDateTime", obj.expressionDeliveryDateTime);
                sqlcmd.Parameters.AddWithValue("@expressionGreetingCard", obj.expressionGreetingCard);
                sqlcmd.Parameters.AddWithValue("@expressionMessage", obj.expressionMessage);
                sqlcmd.Parameters.AddWithValue("@consigneePhoneNo", obj.ConsigneeCell);
                sqlcmd.Parameters.AddWithValue("@expressionconsignmentRefNumber", obj.expressionconsignmentRefNumber);
                sqlcmd.Parameters.AddWithValue("@otherCharges", obj.Othercharges);
                sqlcmd.Parameters.AddWithValue("@routeCode", obj.routeCode);
                sqlcmd.Parameters.AddWithValue("@docPouchNo", obj.docPouchNo);
                sqlcmd.Parameters.AddWithValue("@consignerPhoneNo", obj.ConsignerPhone);
                sqlcmd.Parameters.AddWithValue("@consignerCellNo", obj.ConsignerCell);
                sqlcmd.Parameters.AddWithValue("@consignerCNICNo", obj.ConsignerCNIC);
                sqlcmd.Parameters.AddWithValue("@consignerAccountNo", obj.consignerAccountNo);
                sqlcmd.Parameters.AddWithValue("@consignerEmail", obj.consignerEmail);
                sqlcmd.Parameters.AddWithValue("@docIsHomeDelivery", obj.docIsHomeDelivery);
                sqlcmd.Parameters.AddWithValue("@cutOffTime", obj.cutOffTime);
                sqlcmd.Parameters.AddWithValue("@destinationCountryCode", obj.destinationCountryCode);
                sqlcmd.Parameters.AddWithValue("@decalaredValue", obj.Declaredvalue);
                sqlcmd.Parameters.AddWithValue("@insuarancePercentage", obj.insuarancePercentage);
                sqlcmd.Parameters.AddWithValue("@consignmentScreen", "8");
                sqlcmd.Parameters.AddWithValue("@isInsured", obj.isInsured);
                sqlcmd.Parameters.AddWithValue("@isReturned", obj.isReturned);
                sqlcmd.Parameters.AddWithValue("@consigneeCNICNo", obj.ConsigneeCNIC);
                sqlcmd.Parameters.AddWithValue("@cutOffTimeShift", obj.cutOffTimeShift);
                sqlcmd.Parameters.AddWithValue("@bookingDate", obj.Bookingdate);
                sqlcmd.Parameters.AddWithValue("@cnClientType", obj.cnClientType);
                sqlcmd.Parameters.AddWithValue("@destinationExpressCenterCode", obj.destinationExpressCenterCode);
                sqlcmd.Parameters.AddWithValue("@dayType", 2);
                sqlcmd.Parameters.AddWithValue("@originExpressCenter", obj.expresscenter);
                sqlcmd.Parameters.AddWithValue("@receivedFromRider", obj.receivedFromRider);
                sqlcmd.Parameters.AddWithValue("@chargedAmount", obj.ChargeAmount);
                sqlcmd.Parameters.AddWithValue("@cnState", obj.cnState);
                sqlcmd.Parameters.AddWithValue("@cardRefNo", obj.cardRefNo);
                sqlcmd.Parameters.AddWithValue("@manifestNo", obj.manifestNo);
                sqlcmd.Parameters.AddWithValue("@manifestId", obj.manifestId);
                sqlcmd.Parameters.AddWithValue("@originBrName", obj.originBrName);
                sqlcmd.Parameters.AddWithValue("@orderRefNo", obj.orderRefNo);
                sqlcmd.Parameters.AddWithValue("@customerName", obj.customerName);
                sqlcmd.Parameters.AddWithValue("@productTypeId", obj.productTypeId);
                sqlcmd.Parameters.AddWithValue("@productDescription", obj.productDescription);
                sqlcmd.Parameters.AddWithValue("@chargeCODAmount", obj.chargeCODAmount);
                sqlcmd.Parameters.AddWithValue("@codAmount", obj.codAmount);
                //sqlcmd.Parameters.AddWithValue("@isPM", obj.isPM);
                //sqlcmd.Parameters.AddWithValue("@LstModifiersCN", obj.LstModifiersCNt);
                sqlcmd.Parameters.AddWithValue("@serviceTypeId", obj.serviceTypeId);
                sqlcmd.Parameters.AddWithValue("@originId", obj.originId);
                sqlcmd.Parameters.AddWithValue("@destId", obj.destId);
                sqlcmd.Parameters.AddWithValue("@cnTypeId", obj.cnTypeId);
                sqlcmd.Parameters.AddWithValue("@cnOperationalType", obj.cnOperationalType);
                sqlcmd.Parameters.AddWithValue("@cnScreenId", obj.cnScreenId);
                sqlcmd.Parameters.AddWithValue("@cnStatus", obj.cnStatus);
                sqlcmd.Parameters.AddWithValue("@flagSendConsigneeMsg", obj.flagSendConsigneeMsg);
                sqlcmd.Parameters.AddWithValue("@isExpUser", obj.isExpUser);
                sqlcmd.Parameters.AddWithValue("@isInt", obj.isExpUser);
                sqlcmd.Parameters.AddWithValue("@accountReceivingDate", clvar.deliveryDate);
                sqlcmd.Parameters.AddWithValue("@denseWeight", "0");

                if (Session["BookingUserStatus"].ToString() == "1")
                {
                    sqlcmd.Parameters.AddWithValue("@isApproved", "0");
                    sqlcmd.Parameters.AddWithValue("@InsertType", 2);
                }
                else
                {
                    sqlcmd.Parameters.AddWithValue("@isApproved", "1");
                    sqlcmd.Parameters.AddWithValue("@InsertType", 1);
                }
                sqlcmd.Parameters.AddWithValue("@ispriceComputed", "1");
                //sqlcmd.Parameters.AddWithValue("@InsertType", 1);
                //if (Session["BookingUserStatus"].ToString() == "1")
                //{
                //    sqlcmd.Parameters.AddWithValue("@InsertType", 2);
                //}
                //else
                //{
                //    sqlcmd.Parameters.AddWithValue("@InsertType", 1);
                //}
                sqlcmd.Parameters.AddWithValue("@PaymentMode", obj.PaymentMode);
                sqlcmd.Parameters.AddWithValue("@PaymentTransactionID", obj.PaymentTransactionID);

                //SqlParameter P_XCode = new SqlParameter("@XCode", SqlDbType.VarChar, 256);
                //P_XCode.Direction = ParameterDirection.Output;

                //  sqlcmd.Parameters.Add(P_XCode);
                // sqlcmd.Parameters.Add(SParam);
                sqlcmd.ExecuteNonQuery();
                //IsUnique = Int32.Parse(SParam.Value.ToString());
                // obj.XCode = obj.consignmentNo;
                sqlcon.Close();
            }
            catch (Exception Err)
            {
                clvar.Error = Err.Message.ToString();
                /// IsUnique = 1;
            }
            finally
            { }
            //return IsUnique;
            return clvar.Error;
        }
        public string Add_ConsignmentModifier(DataTable dt, Cl_Variables clvar)
        {
            clvar.Error = "";



            using (SqlConnection con = new SqlConnection(clvar.Strcon()))
            {
                using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                {
                    try
                    {


                        //Set the database table name
                        SqlCommand cmd = new SqlCommand("DElete from ConsignmentModifier where consignmentNumber = '" + clvar.consignmentNo + "'", con);
                        cmd.Connection = con;

                        sqlBulkCopy.DestinationTableName = "dbo.ConsignmentModifier";

                        //[OPTIONAL]: Map the DataTable columns with that of the database table
                        //sqlBulkCopy.ColumnMappings.Add("PriceModifierID", "");
                        //sqlBulkCopy.ColumnMappings.Add("Name", "Name");
                        //sqlBulkCopy.ColumnMappings.Add("Country", "Country");
                        con.Open();
                        cmd.ExecuteNonQuery();
                        if (dt.Rows.Count > 0)
                        {
                            sqlBulkCopy.WriteToServer(dt);
                        }

                        con.Close();
                    }
                    catch (Exception ex)
                    {
                        clvar.Error = ex.Message;
                    }
                }

            }
            return clvar.Error;
        }



        public DataTable GetConsignmentForApproval(Cl_Variables clvar)
        {

            string sqlString = "select c.bookingDate,\n" +
            "       c.consignmentNumber,\n" +
            "       c.customerType,\n" +
            "       c.creditClientId,\n" +
            "       c.orgin,\n" +
            "       c.serviceTypeName,\n" +
            "       c.consigner,\n" +
            "       c.consignee,\n" +
            "       c.destination,\n" +
            "       c.weight,\n" +
            "       c.riderCode,\n" +
            "       c.originExpressCenter,\n" +
            "       c.consignmentTypeId,\n" +
            "       c.chargedAmount,\n" +
            "       c.isApproved,\n" +
            "       ic.invoiceNumber,\n" +
            "       i.startDate ReportingDate,\n" +
            "       i.deliveryStatus,\n" +
            "       cm.priceModifierId,\n" +
            "       p.name,\n" +
            "       cm.calculatedValue,\n" +
            "       cm.calculationBase,\n" +
            "       cm.isTaxable,\n" +
            "       cm.SortOrder,\n" +
            "       p.description, c.destinationExpressCenterCode\n" +
            "\n" +
            "  from Consignment c\n" +
            " inner join InvoiceConsignment ic\n" +
            "    on c.consignmentNumber = ic.consignmentNumber\n" +
            " inner join Invoice i\n" +
            "    on i.invoiceNumber = ic.invoiceNumber\n" +
            "  left outer join ConsignmentModifier cm\n" +
            "    on c.consignmentNumber = cm.consignmentNumber\n" +
            "  left outer join PriceModifiers p\n" +
            "    on cm.priceModifierId = p.id\n" +
            " where c.orgin = '4'\n" +
            "   and c.consignmentTypeId <> '10'\n" +
            "   and cm.priceModifierId is not null\n" +
            "   and IsNull( i.IsInvoiceCanceled , 0 ) ='0'\n" +
            "   and c.consignmentNumber = '" + clvar.consignmentNo + "'\n" +
            " order by consignmentNumber, SortOrder";

            // sqlString = "select c.bookingDate,\n" +
            //"       c.consignmentNumber,\n" +
            //"       c.customerType,\n" +
            //"       c.creditClientId,\n" +
            //"       c.orgin,\n" +
            //"       c.serviceTypeName,\n" +
            //"       c.consigner,\n" +
            //"       c.consignee,\n" +
            //"       c.destination,\n" +
            //"       c.weight,\n" +
            //"       c.riderCode,\n" +
            //"       c.originExpressCenter,\n" +
            //"       c.consignmentTypeId,\n" +
            //"       c.chargedAmount,\n" +
            //"       CAST(c.isApproved as varchar) isApproved,\n" +
            //"       c.consignerAccountNo accountNo,\n" +
            //"       ic.invoiceNumber,\n" +
            //"       i.startDate ReportingDate,\n" +
            //"       i.deliveryStatus,\n" +
            //"       cm.priceModifierId,\n" +
            //"       p.name priceModifierName,\n" +
            //"       cm.calculatedValue,\n" +
            //"       cm.calculationBase,\n" +
            //"       cm.isTaxable,\n" +
            //"       cm.SortOrder,\n" +
            //"       p.description, c.destinationExpressCenterCode, c.accountReceivingDate, c.bookingDate, c.COD\n" +
            //"\n" +
            //"  from consignment c\n" +
            //" left outer join InvoiceConsignment ic\n" +
            //"    on c.consignmentNumber = ic.consignmentNumber\n" +
            //" left outer join Invoice i\n" +
            //"    on i.invoiceNumber = ic.invoiceNumber\n" +
            //"  left outer join ConsignmentModifier cm\n" +
            //"    on c.consignmentNumber = cm.consignmentNumber\n" +
            //"  left outer join PriceModifiers p\n" +
            //"    on cm.priceModifierId = p.id\n" +
            //" inner join CreditClients cc\n" +
            //"    on c.creditClientId = cc.id\n" +
            //" where /*c.orgin = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'\n" +
            //"   and c.consignmentTypeId <> '10'\n" +
            //"   and */c.consignmentNumber = '" + clvar.consignmentNo.Trim() + "' and ISNULL(i.IsInvoiceCanceled,0) = '0' \n" +
            //" order by consignmentNumber, SortOrder";

            string sql = "SELECT A.*, i.deliveryStatus, CASE  \n"
               + "WHEN i.IsInvoiceCanceled = '1' THEN ''\n"
               + " ELSE i.invoiceNumber      \n"
               + "END invoiceNumber_ FROM ( \n"
               + "SELECT --c.bookingDate, \n"
               + "       c.consignmentNumber, \n"
               + "       c.customerType, \n"
               + "       c.creditClientId, \n"
               + "       c.orgin, \n"
               + "       c.serviceTypeName, \n"
               + "       c.consigner, \n"
               + "       c.consignee, \n"
               + "       c.destination, \n"
               + "       c.weight, \n"
               + "       c.riderCode, \n"
               + "       c.originExpressCenter, \n"
               + "       c.consignmentTypeId, \n"
               + "       c.chargedAmount, \n"
               + "       CAST(c.isApproved AS VARCHAR)     isApproved, \n"
               + "       c.consignerAccountNo              accountNo, \n"
               + "       --ic.invoiceNumber, \n"
               + "   --    i.startDate                       ReportingDate, \n"
               + "   --    i.deliveryStatus, \n"
               + "       cm.priceModifierId, \n"
               + "       p.name                            priceModifierName, \n"
               + "       cm.calculatedValue, \n"
               + "       cm.modifiedCalculationValue modifiedCalculatedValue,\n"
               + "       cm.calculationBase, \n"
               + "       cm.isTaxable, \n"
               + "       cm.SortOrder, \n"
               + "       p.description, \n"
               + "       c.destinationExpressCenterCode, \n"
               + "       c.accountReceivingDate, \n"
               + "       c.bookingDate, \n"
               + "       c.COD, c.gst, c.totalAmount, \n"
               + "       ic.invoiceNumber, c.consignerCellNo, c.ConsigneePhoneNo, c.address, c.pieces \n"
               + "    --   i.IsInvoiceCanceled \n"
               + "FROM   Consignment c \n"
               + "       LEFT OUTER JOIN InvoiceConsignment ic \n"
               + "            ON  c.consignmentNumber = ic.consignmentNumber \n"
               + "       --LEFT OUTER JOIN Invoice i \n"
               + "       --     ON  i.invoiceNumber = ic.invoiceNumber \n"
               + "       LEFT OUTER JOIN ConsignmentModifier cm \n"
               + "            ON  c.consignmentNumber = cm.consignmentNumber \n"
               + "       LEFT OUTER JOIN PriceModifiers p \n"
               + "            ON  cm.priceModifierId = p.id \n"
               + "       INNER JOIN CreditClients cc \n"
               + "            ON  c.creditClientId = cc.id \n"
               + "WHERE  /*c.orgin = '4' \n"
               + "       and c.consignmentTypeId <> '10' \n"
               + "       and */c.consignmentNumber = '" + clvar.consignmentNo.Trim() + "' \n"
               + "  --     AND ISNULL(i.IsInvoiceCanceled, 0) = '0' \n"
               + ") A \n"
               + "LEFT OUTER JOIN  \n"
               + "Invoice i  \n"
               + "ON A.invoiceNumber = i.invoiceNumber \n"
               + "  ORDER BY i.createdOn desc";

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

        public DataSet CustomerInformation(Cl_Variables clvar)
        {
            DataSet Ds_1 = new DataSet();

            try
            {
                string query = "SELECT * FROM CreditClients cc WHERE cc.accountNo = '" + clvar.AccountNo + "' and cc.branchCode='" + clvar.Branch + "'";

                string sqlString = "SELECT cc.*,\n" +
                "       case\n" +
                "         when cu.CreditClientID is not null then\n" +
                "          '1'\n" +
                "         else\n" +
                "          '0'\n" +
                "       end isAPIClient\n" +
                "  FROM CreditClients cc\n" +
                "  left outer join CODUserCNSequence cu\n" +
                "    --on cu.accountno = cc.accountno\n" +
                "   on cu.creditCLientID = cc.id\n" +
                "\n" +
                " where cc.accountNo = '" + clvar.AccountNo + "'\n" +
                "   and cc.branchCode = '" + clvar.Branch + "'\n" +
                "   and cc.isActive = '1'";


                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
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



        //public void Alert(string message)
        //{
        //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + message + "')", true);
        //    lbl_error.Text = message;
        //}


        DataTable GetConsignmentModifiers(Cl_Variables clvar)
        {
            string query = "select * from ec_consignmentModifier cm where cm.consignmentNumber = '" + clvar.consignmentNo + "'";
            query = "select cm.*, pm.name, pm.description\n" +
                    "  from ConsignmentModifier cm\n" +
                    " inner join priceModifiers pm \n" +
                    "    on pm.id = cm.priceModifierId\n" +
                    " where cm.consignmentNumber = '" + clvar.consignmentNo + "'";

            //DataTable dt = new DataTable();

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


        public string updateChargedAmount(Cl_Variables clvar)
        {
            string query = "UPDATE Consignment set chargedAmount = '" + txt_chargedamount.Text + "' where consignmentNumber = '" + clvar.consignmentNo + "'";

            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally { con.Close(); }
            return "";
        }

        public DataTable SequenceCheck(Cl_Variables clvar, string specialCondition)
        {
            string sql = "SELECT * \n"
                + "FROM Mnp_ZoneCNSquence mzc \n"
                + "WHERE   mzc.ZoneCode='" + clvar.Zone + "' AND \n"
                + "       '" + clvar.consignmentNo + "' BETWEEN mzc.SequenceStart AND mzc.EndSequence " + specialCondition + "";

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sql, con);
                sda.SelectCommand.CommandTimeout = 120;
                sda.Fill(dt);

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return dt;

        }

        protected void btn_validate_Click(object sender, EventArgs e)
        {
            txt_consignmentNo_TextChanged(this, e);
            if (!isValid)
            {
                Alert(message);
                return;
            }
            txt_accountNo_TextChanged(this, e);
            if (!isValid)
            {
                return;
            }

            dd_destCountry_SelectedIndexChanged(this, DropDownListEventArgs.Empty);
            if (!isValid)
            {
                return;
            }
            double tempchamount = 0;
            double.TryParse(txt_chargedamount.Text, out tempchamount);
            if (txt_accountNo.Text.Trim() == "0" && (tempchamount < 0 || tempchamount == 0))
            {
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Charge Amount cannot be zero or empty')", true);
                //lbl_error.Text = "Charge Amount cannot be zero or empty";
                //return;
            }
            if (txt_approve_status.Text.ToUpper() == "APPROVED")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('CN Already Approved')", true);
                return;
            }
            DateTime thisDate = DateTime.Parse(txt_bookingDate.Text);
            DateTime reportingDate = DateTime.Parse(txt_reporting_date.Text);
            clvar.Branch = HttpContext.Current.Session["BranchCode"].ToString();
            DataTable dates = MinimumDate(clvar);
            DateTime minAllowedDate = DateTime.Parse(dates.Rows[0]["DateAllowed"].ToString()).AddDays(1);
            DateTime maxAllowedDate = DateTime.Now;
            if (thisDate < minAllowedDate || thisDate > maxAllowedDate || reportingDate < minAllowedDate || reportingDate > maxAllowedDate)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Invalid Date.')", true);
                return;
            }

            #region Validations

            if (txt_consignmentNo.Text.Trim(' ') == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter Consignment Number')", true);
                txt_consignmentNo.Focus();
                return;
            }
            else if (txt_consignmentNo.Text.ToCharArray()[0] != '9' && rbtn_consignmentSender.SelectedValue != "4")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Invalid Consignment Number')", true);
                txt_consignmentNo.Focus();
                return;
            }
            if (txt_accountNo.Text.Trim(' ') == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter Account No.')", true);
                txt_accountNo.Focus();
                return;
            }
            //if (txt_consignerCellNo.Text.Trim(' ') == "")
            //{
            //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter Consigner Cell Number')", true);
            //    txt_consignerCellNo.Focus();
            //    return;
            //}
            //if (txt_consigner.Text.Trim(' ') == "")
            //{
            //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter Consigner')", true);
            //    txt_consigner.Focus();
            //    return;
            //}
            //if (txt_consignerCNIC.Text.Trim(' ') == "")
            //{
            //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter Consignmener CNIC')", true);
            //    txt_consignerCNIC.Focus();
            //    return;
            //}
            //if (txt_consignee.Text.Trim(' ') == "")
            //{
            //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter Consignee')", true);
            //    txt_consignee.Focus();
            //    return;
            //}
            //if (txt_cellNo.Text.Trim(' ') == "")
            //{
            //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter Consignee Cell Number')", true);
            //    txt_cellNo.Focus();
            //    return;
            //}

            if (txt_accountNo.Text != "")
            {
                if (txt_chargedamount.Text.Trim(' ') == "")
                {
                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter Charge Amount')", true);
                    //txt_cellNo.Focus();
                    //return;
                }
            }
            if (txt_reporting_date.Text.Trim(' ') == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter Reporting Date')", true);
                txt_cellNo.Focus();
                return;
            }



            if (txt_riderCode.Text.Trim(' ') != "")
            {
                clvar.riderCode = txt_riderCode.Text;
                DataTable rider = ValidateRiderCode(clvar);
                if (rider != null)
                {
                    if (rider.Rows.Count > 0)
                    {
                        ecCode.Value = rider.Rows[0]["ExpressCenterID"].ToString();
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Invalid Rider Code')", true);
                        txt_riderCode.Text = ""; chk_riderCode.Checked = false;
                        txt_riderCode.Focus();
                        return;
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Invalid Rider Code')", true);
                    txt_riderCode.Text = ""; chk_riderCode.Checked = false;
                    txt_riderCode.Focus();
                    return;
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter Rider Code')", true);
                //txt_riderCode.Text = ""; chk_riderCode.Checked = false;
                txt_riderCode.Focus();
                return;
            }
            if (dd_destCountry.SelectedValue.Trim(' ') == "0")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Select Destination Country')", true);
                dd_destCountry.Focus();
                return;
            }
            if (dd_serviceType.SelectedValue.Trim(' ') == "0")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Select Service Type')", true);
                dd_serviceType.Focus();
                return;
            }
            if (txt_weight.Text.Trim(' ') == "" || txt_weight.Text.Trim(' ') == "0")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter Proper Weight')", true);
                txt_weight.Focus();
                return;
            }
            //if (txt_consignerAddress.Text.Trim(' ') == "")
            //{
            //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter Consigner Address')", true);
            //    txt_consignerAddress.Focus();
            //    return;
            //}
            //if (txt_consigneeAddress.Text.Trim(' ') == "")
            //{
            //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter Consignee Address')", true);
            //    txt_consigneeAddress.Focus();
            //    return;
            //}
            if (txt_pieces.Text.Trim() == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter Pieces')", true);
                txt_consigneeAddress.Focus();
                return;
            }
            else if (int.Parse(txt_pieces.Text.Trim()) <= 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Pieces cannot be zero')", true);
                txt_consigneeAddress.Focus();
                return;
            }

            double tempPieces = 0;
            double.TryParse(txt_pieces.Text, out tempPieces);
            if (tempPieces <= 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Pieces Cannot be equal or less than 0')", true);
                lbl_error.Text = "Pieces Cannot be equal or less than 0";
                return;
            }

            #endregion

            double totalAmount = 0;
            int cod = 0;
            try
            {
                clvar.Zone = Session["zonecode"].ToString(); //"2";//dd_origin.SelectedValue.Split('-')[1];
                clvar.expresscenter = Session["ExpressCenter"].ToString();
                clvar.destinationExpressCenterCode = "0111";
                clvar.FromZoneCode = Session["zonecode"].ToString();//dd_origin.SelectedValue.Split('-')[1];
            }
            catch (Exception ex)
            {
                Response.Redirect("~/login");
            }

            clvar.destinationCountryCode = dd_destCountry.SelectedValue;
            clvar.CustomerClientID = hd_creditClientID.Value;
            if (ViewState["serviceType"] != null)
            {
                clvar.ToZoneCode = (ViewState["serviceType"] as DataTable).Rows[0][0].ToString();
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cannot Add Consignment')", true);
                return;
            }

            clvar.ServiceTypeName = dd_serviceType.SelectedValue;
            clvar.Branch = dd_origin.SelectedValue.Split('-')[0];
            //DataTable dt = GetClientTarrifForInternationalConsignment(clvar).Tables[0];
            double tempw = 0;

            double.TryParse(txt_weight.Text, out tempw);
            if (tempw <= 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Weight Cannot be equal or less than 0')", true);
                lbl_error.Text = "Weight Cannot be equal or less than 0";
                return;
            }
            clvar.Weight = float.Parse(txt_weight.Text);

            if (clvar.Weight == 0 || clvar.Weight == null)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Enter Weight.')", true);
                return;
            }


            clvar.consignmentNo = txt_consignmentNo.Text;
            clvar.Consigner = txt_consigner.Text;
            clvar.Consignee = txt_consignee.Text;
            clvar.CouponNo = txt_cupon.Text;
            //   clvar.Customertype = int.Parse(rbtn_customerType.SelectedValue);
            clvar.origin = dd_origin.SelectedValue.Split('-')[0];
            clvar.Destination = dd_destination.SelectedValue;
            clvar.pieces = int.Parse(txt_pieces.Text);
            clvar.ServiceTypeName = dd_serviceType.SelectedValue;
            clvar.CustomerClientID = hd_creditClientID.Value;
            clvar.Con_Type = int.Parse(dd_conType.SelectedValue);
            clvar.Unit = 0;
            //  clvar.ChargeAmount = int.Parse(txt_chargedamount.Text);

            //clvar.Discount = int.Parse(txt_discount.Text);
            int d = 0;
            int.TryParse(txt_discount.Text, out d);
            clvar.Discount = d;
            //clvar.isCod = false;

            clvar.ConsigneeAddress = txt_consigneeAddress.Text;
            clvar.createdBy = "99";
            clvar.status = 1;


            clvar.ConsignerAddress = txt_consignerAddress.Text;
            clvar.RiderCode = txt_riderCode.Text;
            double w = 0;
            double.TryParse(txt_w.Text, out w);
            clvar.width = w;//double.Parse(txt_w.Text);
            double b = 0;
            double.TryParse(txt_b.Text, out b);
            clvar.breadth = b;// double.Parse(txt_b.Text);
            double h = 0;
            double.TryParse(txt_h.Text, out h);

            clvar.height = h;// double.Parse(txt_h.Text);
            clvar.PakageContents = txt_packageContent.Text;
            clvar.ConsigneeCell = txt_cellNo.Text;
            clvar.ConsignerCell = txt_consignerCellNo.Text;
            clvar.ConsignerCNIC = txt_consignerCNIC.Text;
            clvar.AccountNo = txt_accountNo.Text;
            clvar.destinationCountryCode = dd_destCountry.SelectedValue;
            float declaredValue = 0;
            if (chk_insurance.Checked)
            {
                clvar.Declaredvalue = declaredValue = float.Parse(txt_declaredValue.Text);
                clvar.insuarancePercentage = 2.5f;
                clvar.isInsured = true;

                if (clvar.Declaredvalue <= 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Enter Declared Value')", true);
                    return;
                }

            }
            else
            {
                clvar.Declaredvalue = 0;
                clvar.insuarancePercentage = 0;
                clvar.isInsured = false;
            }
            clvar.Bookingdate = DateTime.Parse(txt_bookingDate.Text);

            clvar.originBrName = dd_origin.SelectedItem.Text;
            clvar.customerName = txt_consigner.Text;
            //clvar.serviceTypeId = dd_serviceType.SelectedValue;
            clvar.originId = dd_origin.SelectedValue.Split('-')[0];
            clvar.isExpUser = "1";

            DataSet BranchGSTInformation = con.BranchGSTInformation(clvar);
            string gst = "";
            if (BranchGSTInformation.Tables[0].Rows.Count != 0)
            {
                gst = BranchGSTInformation.Tables[0].Rows[0]["gst"].ToString();
            }
            clvar.gst = (double.Parse(gst) / 100) * totalAmount;
            clvar.consignerAccountNo = txt_accountNo.Text;


            DataTable dtc = new DataTable();
            dtc.Columns.AddRange(new DataColumn[12]
            {
            new DataColumn("PriceModifierID", typeof(int)),
            new DataColumn("ConsignmentNumber", typeof(Int64)),
            new DataColumn("CreatedBy",typeof(string)),
            new DataColumn("CreatedOn", typeof(DateTime)),
            new DataColumn("ModifiedBy", typeof(string)),
            new DataColumn("ModifiedOn", typeof(DateTime)),
            new DataColumn("ModifiedCalculationValue", typeof(int)),
            new DataColumn("CalculatedValue", typeof(float)),
            new DataColumn("CalculatedGST", typeof(float)),
            new DataColumn("CalculationBase", typeof(int)),
            new DataColumn("isTaxable", typeof(bool)),
            new DataColumn("SortOrder", typeof(int))

            });
            dtc.AcceptChanges();
            DataTable pm = ViewState["PriceModifiers"] as DataTable;
            DataTable surcharges = ViewState["dt"] as DataTable;


            int count = 1;
            double tempTotalAmount = totalAmount;
            double tempTotalGst = (double.Parse(gst) / 100) * totalAmount;
            double tempGrandTotalAmount = 0;
            double insurance = 0;
            insurance = declaredValue * 0.025;
            tempTotalGst += insurance * (double.Parse(gst) / 100);
            tempTotalAmount += insurance;
            double tempt = totalAmount;


            tempGrandTotalAmount = tempTotalAmount + tempTotalGst;


            //if (sender.Equals(btn_Validate))
            //{
            //    btn_save.Visible = true;
            //    //btn_Validate.Visible = false;
            //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Total Amount      " + tempTotalAmount + ". \\nTotalGst          " + tempTotalGst + ".\\nGrand Total       " + tempGrandTotalAmount + "')", true);
            //    return;
            //}


            try
            {
                clvar.expresscenter = HttpContext.Current.Session["ExpressCenter"].ToString();
                clvar.OriginExpressCenterCode = ecCode.Value;
                clvar.destinationExpressCenterCode = "0111";
            }
            catch (Exception)
            {

                Response.Redirect("~/login");
            }


            if (clvar.isCod == false)
            {
                clvar.StateID = "0";
            }

            if (clvar.isCod == true)
            {
                clvar.StateID = "1";
            }


            clvar.gst = Math.Round(tempTotalGst, 2);
            //clvar.ChargeAmount = Double.Parse(txt_chargedamount.Text.ToString());

            //clvar.StateID = codcheck.ToString();
            clvar._CityCode = dd_destination.SelectedItem.Text;

            clvar.Day = txt_reporting_date.Text;
            clvar.Customertype = 0;
            clvar.PaymentMode = dd_PaymentMode.SelectedValue;
            clvar.PaymentTransactionID = txt_TransactionID.Text;

            string Error = Add_Consignment_Validation(clvar);
            if (txt_accountNo.Text.Trim() == "0")
            {
                clvar.Customertype = 1;
            }
            else
            {
                clvar.Customertype = 2;
            }

            if (Error == "")
            {
                DataSet pricing = Add_OcsInternationalValidation(clvar);
                if (pricing != null)
                {
                    if (pricing.Tables[0].Rows.Count > 0)
                    {
                        if (pricing.Tables[1].Rows[0]["amount"].ToString().Trim() == "0" || pricing.Tables[1].Rows[0]["amount"].ToString().Trim() == "")
                        {
                            AlertMessage("Could Not Compute Prices");
                            return;
                        }
                        else
                        {
                            clvar.TotalAmount = double.Parse(pricing.Tables[1].Rows[0]["amount"].ToString());
                            clvar.gst = double.Parse(pricing.Tables[1].Rows[0]["gst"].ToString());
                            txt_totalAmount.Text = Math.Round(clvar.TotalAmount, 2).ToString();
                            txt_gst.Text = Math.Round(clvar.gst, 2).ToString();
                            clvar.cnScreenId = "8";
                            clvar.deliveryDate = DateTime.Parse(txt_reporting_date.Text);
                            clvar.createdBy = HttpContext.Current.Session["U_ID"].ToString();
                            //Error = Add_Consignment(clvar);

                            if (Error == "")
                            {
                                if (txt_accountNo.Text.Trim() == "0")
                                {
                                    //updateChargedAmount(clvar);
                                }
                            }
                            else
                            {
                                AlertMessage("Could not Add Consignment");
                                return;
                            }
                            foreach (DataRow dr in surcharges.Rows)
                            {
                                DataRow dr_ = dtc.NewRow();
                                dr_["pricemodifierid"] = int.Parse(pm.Select("id = '" + dr["id"].ToString() + "'")[0]["ID"].ToString());
                                dr_["ConsignmentNumber"] = Int64.Parse(txt_consignmentNo.Text);
                                dr_["CreatedBy"] = "TEST";
                                dr_["CreatedOn"] = DateTime.Now;
                                dr_["ModifiedBy"] = DBNull.Value;
                                dr_["ModifiedOn"] = DBNull.Value;
                                if (dr["BASE"].ToString() == "2")
                                {
                                    dr_["CalculatedValue"] = (float.Parse(dr["PValue"].ToString()) / 100) * clvar.TotalAmount;
                                    dr_["CalculatedGst"] = ((float.Parse(dr["PValue"].ToString()) / 100) * clvar.TotalAmount) * (float.Parse(BranchGSTInformation.Tables[0].Rows[0]["GST"].ToString()) / 100);
                                    tempTotalAmount += (float.Parse(dr["PValue"].ToString()) / 100) * clvar.TotalAmount;
                                    tempTotalGst += ((float.Parse(dr["PValue"].ToString()) / 100) * clvar.TotalAmount) * (float.Parse(BranchGSTInformation.Tables[0].Rows[0]["GST"].ToString()) / 100);
                                    tempt = tempt + (float.Parse(dr["PValue"].ToString()) / 100) * clvar.TotalAmount;
                                    //dr_["CalculatedGst"] = (float.Parse(BranchGSTInformation.Tables[0].Rows[0]["GST"].ToString()) / 100) * float.Parse(dr["Pvalue"].ToString());
                                }
                                else
                                {
                                    dr_["CalculatedValue"] = float.Parse(dr["Pvalue"].ToString()); //float.Parse(pm.Select("id = '" + dr["id"].ToString() + "'")[0]["ActualValue"].ToString());

                                    dr_["CalculatedGst"] = (float.Parse(BranchGSTInformation.Tables[0].Rows[0]["GST"].ToString()) / 100) * float.Parse(dr["Pvalue"].ToString());
                                    tempTotalAmount += float.Parse(dr["Pvalue"].ToString());
                                    tempTotalGst += (float.Parse(BranchGSTInformation.Tables[0].Rows[0]["GST"].ToString()) / 100) * float.Parse(dr["Pvalue"].ToString());
                                    tempt = tempt + float.Parse(dr["Pvalue"].ToString());
                                }

                                dr_["ModifiedCalculationValue"] = float.Parse(dr["Pvalue"].ToString());//float.Parse(pm.Select("id = '" + dr["id"].ToString() + "'")[0]["CalculationValue"].ToString());
                                                                                                       //dr_["CalculatedGst"] = (float.Parse(BranchGSTInformation.Tables[0].Rows[0]["GST"].ToString()) / 100) * float.Parse(dr["Pvalue"].ToString());
                                dr_["CalculationBase"] = int.Parse(dr["BASE"].ToString());
                                dr_["isTaxable"] = 1;
                                dr_["SortOrder"] = count++;
                                dtc.Rows.Add(dr_);
                                dtc.AcceptChanges();
                            }
                            txt_totalAmount.Text = Math.Round(tempTotalAmount + clvar.TotalAmount, 2).ToString();
                            txt_gst.Text = Math.Round(tempTotalGst + clvar.gst, 2).ToString();
                            //Error = Add_ConsignmentModifier(dtc, clvar);
                            //if (Error == "")
                            //{
                            //    AlertMessage("Consignment Saved");
                            //    txt_consignmentNo_TextChanged(this, e);
                            //    return;
                            //}
                        }
                    }
                    else
                    {
                        AlertMessage("Could Not Compute Prices");
                        return;
                    }
                }
                else
                {
                    AlertMessage("Could Not Compute Prices");
                    return;
                }
            }
            else
            {
                //Error Phenk 
            }
            clvar.cnScreenId = "8";
            btn_save.Enabled = true;
            return;


        }

        protected void Alert(string msg)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + message + "')", true);
            lbl_error.Text = msg;
            return;
        }
        protected void btn_print_Click(object sender, EventArgs e)
        {

        }

        private string IsMobileNumberValid(string mobileNumber)
        {
            string _mobileNumber = "";
            // remove all non-numeric characters
            _mobileNumber = CleanNumber(mobileNumber);

            // trim any leading zeros
            _mobileNumber = _mobileNumber.TrimStart(new char[] { '0' });

            // check for this in case they've entered 44 (0)xxxxxxxxx or similar
            if (_mobileNumber.StartsWith("920"))
            {
                _mobileNumber = _mobileNumber.Remove(2, 1);
            }

            // add country code if they haven't entered it
            if (!_mobileNumber.StartsWith("92"))
            {
                _mobileNumber = "92" + _mobileNumber;
            }

            // check if it's the right length
            if (_mobileNumber.Length != 12)
            {
                return "0";
            }

            return _mobileNumber;
        }
        private string CleanNumber(string phone)
        {
            Regex digitsOnly = new Regex(@"[^\d]");
            return digitsOnly.Replace(phone, "");
        }
        public void Post_BrandedSMS(string mobile, string resp, string Consignee, string destination)
        {
            try
            {
                if (txt_consignmentNo.Text != string.Empty)
                {
                    string smsContent = "Dear Customer, A shipment CN " + resp + " is booked successfully. You can visit www.mulphilog.com or call us on 111-202-202 to track delivery status. Thank you";
                    string query2 = "insert into mnp_smsStatus (ConsignmentNumber, Recepient, MessageContent, Status, Createdon, CreatedBy, RunsheetNumber,smsformtype) \n" +
                         "values ('" + resp + "', '" + mobile + "', '" + smsContent + "', '0', GETDATE(), '" + HttpContext.Current.Session["U_ID"].ToString() + "', 'N/A','3')";

                    int count = 0;
                    string error = "";
                    SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
                    try
                    {
                        sqlcon.Open();
                        SqlCommand sqlcmd = new SqlCommand(query2, sqlcon);
                        sqlcmd.CommandType = CommandType.Text;

                        count = sqlcmd.ExecuteNonQuery();
                        sqlcon.Close();
                    }
                    catch (Exception ex)
                    {
                        error = ex.Message;
                    }
                }

                #region OLD
                /*
                // Create a request using a URL that can receive a post. 
                //   WebRequest request = WebRequest.Create("https://services.converget.com/MTEntryDyn/?");
                // Set the Method property of the request to POST.

                // Create POST data and convert it to a byte array.
                if (txt_consignmentNo.Text != string.Empty)
                {
                    //  DataTable dt = clvar.GetMaxOrderNo();
                    string newResp = "Dear Customer, A shipment CN " + resp + " is booked successfully. You can visit www.mulphilog.com or call us on 111-202-202 to track delivery status. Thank you";
                    //newResp = "Dear Customer, A shipment CN " + resp + " is booked for you. Please visit www.mulphilog.com or call us on 111 202 202 to track delivery status. Thank you";
                    //string resp_ = "Dear Valued Customer, We have received your shipment under CN:" + resp + "for " + Consignee + " - " + destination + " Amount :" + string.Format("{0:N0}", Double.Parse(txt_TotalAmount.Text)) + ". Please visit www.mulphilog.com or call us on 021-111-202-202 to track delivery status. Thank you";
                    String Mobile = HttpUtility.UrlEncode(IsMobileNumberValid(mobile));
                    String Response = HttpUtility.UrlEncode(resp);
                    string postData = "";//"to=" + Mobile + "&text=" + Response + "&from=OCS&username=sales&password=salestest8225";//"PhoneNumber=" + Mobile + "&Text=" + Response;

                    WebRequest request = WebRequest.Create("http://180.178.135.203:24555/api?action=sendmessage&username=OCS_SC&password=o@cs!82660&recipient=" + Mobile + "&originator=82660&messagedata=" + newResp);
                    request.Method = "POST";
                    byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                    ////// Set the ContentType property of the WebRequest.
                    request.ContentType = "application/x-www-form-urlencoded";
                    ////// Set the ContentLength property of the WebRequest.
                    request.ContentLength = byteArray.Length;
                    // Get the request stream.
                    Stream dataStream = request.GetRequestStream();
                    // Write the data to the request stream.
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    // Close the Stream object.
                    dataStream.Close();
                    // Get the response.
                    WebResponse response = request.GetResponse();
                    // Display the status.
                    Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                    // Get the stream containing content returned by the server.
                    dataStream = response.GetResponseStream();
                    // Open the stream using a StreamReader for easy access.
                    StreamReader reader = new StreamReader(dataStream);
                    // Read the content.
                    string responseFromServer = reader.ReadToEnd();
                    // Display the content.
                    Console.WriteLine(responseFromServer);
                    // Clean up the streams.
                    reader.Close();
                    dataStream.Close();
                    response.Close();
                }
                */

                #endregion
            }
            catch (Exception Err)
            {
                //listBox1.Items.Add("Failed " + Err + "Mobile Number " + clvar.MobileNumber + " and Response " + resp);
            }

        }
        public void Post_BrandedSMS_(string mobile, string resp, string Consignee, string destination)
        {
            try
            {
                if (txt_consignmentNo.Text != string.Empty)
                {
                    string smsContent = "Dear Customer, A shipment CN " + resp + " is booked for you. Please visit www.mulphilog.com or call us on 111 202 202 to track delivery status. Thank you";
                    string query2 = "insert into mnp_smsStatus (ConsignmentNumber, Recepient, MessageContent, Status, Createdon, CreatedBy, RunsheetNumber) \n" +
                             "values ('" + resp + "', '" + mobile + "', '" + smsContent + "', '0', GETDATE(), '" + HttpContext.Current.Session["U_ID"].ToString() + "', 'N/A')";

                    int count = 0;
                    string error = "";
                    SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
                    try
                    {
                        sqlcon.Open();
                        SqlCommand sqlcmd = new SqlCommand(query2, sqlcon);
                        sqlcmd.CommandType = CommandType.Text;

                        count = sqlcmd.ExecuteNonQuery();
                        sqlcon.Close();
                    }
                    catch (Exception ex)
                    {
                        error = ex.Message;
                    }

                    #region OLD
                    /*
                    //  DataTable dt = clvar.GetMaxOrderNo();
                    string newResp = "Dear Customer, Your shipment CN " + resp + " is received on " + DateTime.Now.Date.ToString("yyyy-MM-dd") + ". Thank You. For further details, contact us on 111-202-202";
                    newResp = "Dear Customer, A shipment CN " + resp + " is booked for you. Please visit www.mulphilog.com or call us on 111 202 202 to track delivery status. Thank you";
                    string resp_ = "Dear Customer, A shipment has been booked under CN:" + resp + "for " + Consignee + " - " + destination + ".You can visit www.ocs.com.pk or call us on 021-111 202 202 to track delivery status. Thank you";
                    String Mobile = HttpUtility.UrlEncode(IsMobileNumberValid(mobile));
                    String Response = HttpUtility.UrlEncode(resp);
                    string postData = "";//"to=" + Mobile + "&text=" + Response + "&from=OCS&username=sales&password=salestest8225";//"PhoneNumber=" + Mobile + "&Text=" + Response;

                    WebRequest request = WebRequest.Create("http://180.178.135.203:24555/api?action=sendmessage&username=OCS_SC&password=o@cs!82660&recipient=" + Mobile + "&originator=82660&messagedata=" + newResp);
                    request.Method = "POST";
                    byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                    ////// Set the ContentType property of the WebRequest.
                    request.ContentType = "application/x-www-form-urlencoded";
                    ////// Set the ContentLength property of the WebRequest.
                    request.ContentLength = byteArray.Length;
                    // Get the request stream.
                    Stream dataStream = request.GetRequestStream();
                    // Write the data to the request stream.
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    // Close the Stream object.
                    dataStream.Close();
                    // Get the response.
                    WebResponse response = request.GetResponse();
                    // Display the status.
                    Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                    // Get the stream containing content returned by the server.
                    dataStream = response.GetResponseStream();
                    // Open the stream using a StreamReader for easy access.
                    StreamReader reader = new StreamReader(dataStream);
                    // Read the content.
                    string responseFromServer = reader.ReadToEnd();
                    // Display the content.
                    Console.WriteLine(responseFromServer);
                    // Clean up the streams.
                    reader.Close();
                    dataStream.Close();
                    response.Close();
                    */
                    #endregion
                }
            }
            catch (Exception Err)
            {
                //listBox1.Items.Add("Failed " + Err + "Mobile Number " + clvar.MobileNumber + " and Response " + resp);
            }

        }
        protected void btn_print_Click1(object sender, EventArgs e)
        {

        }
    }
}