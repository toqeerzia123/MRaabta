using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Web.Services;
using MRaabta.App_Code;
using System.Globalization;

namespace MRaabta.Files
{
    public partial class BulkConsignment_NewSequences : System.Web.UI.Page
    {
        public static Cl_Variables clvar = new Cl_Variables();
        public static CommonFunction func = new CommonFunction();
        public static Consignemnts con = new Consignemnts();

        public string InsertType = "";
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!(HttpContext.Current.Session["U_ID"].ToString() == "2974" || HttpContext.Current.Session["U_ID"].ToString() == "3060"))
            {
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "alert('You are not authorized to view this page.'); window.location='" + Request.ApplicationPath + "/Files/BTSDashoard.aspx';", true);
                //return;
            }
            clvar.Branch = HttpContext.Current.Session["BranchCode"].ToString();
            DataTable allowedDate = MinimumDate(clvar);
            if (allowedDate != null)
            {
                if (allowedDate.Rows.Count > 0)
                {
                    hd_minAllowedDate.Value = ((DateTime)allowedDate.Rows[0][0]).ToString("dd-MM-yyyy");	
                    hd_maxAllowedDate.Value = DateTime.Now.ToString("dd-MM-yyyy");
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "alert('Could Not Fetch Month End Date. Please Contact I.T. Support.'); window.location='" + Request.ApplicationPath + "/login.aspx';", true);
                    return;
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "alert('Could Not Fetch Month End Date. Please Contact I.T. Support.'); window.location='" + Request.ApplicationPath + "/login.aspx';", true);
                return;
            }
            if (!IsPostBack)
            {

                calendar1.SelectedDate = DateTime.Now;	
                txt_bookingDate.Text = DateTime.Now.ToString("dd-MM-yyyy");	
                calendar2.SelectedDate = DateTime.Now;	
                txt_reportingDate.Text = DateTime.Now.ToString("dd-MM-yyyy");	
                //txt_bookingDate.Text = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")).ToShortDateString().ToString();	
                //txt_reportingDate.Text = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")).ToShortDateString().ToString();
	        Get_origin();
                Get_Service_Type();
                Get_ConType();
                dd_consignmentType.SelectedIndex = 1;
                //  Get_ProductType();

                //Get_PriceModifiers();
                Get_OriginExpressCenter();
                Get_PriceModifiers();
                GetFuelSurcharges();
                txt_chargedAmount.Text = "0";
                txt_totalAmt.Text = "0";
                txt_gst.Text = "0";

                DataTable pm = new DataTable();
                pm.Columns.Add("priceModifierId");
                pm.Columns.Add("priceModifierName");
                pm.Columns.Add("calculatedValue");
                pm.Columns.Add("ModifiedCalculatedValue");
                pm.Columns.Add("calculationBase");
                pm.Columns.Add("isTaxable");
                pm.Columns.Add("description");
                pm.Columns.Add("SortOrder");
                pm.Columns.Add("NEW");
                pm.Columns.Add("AlternateValue");
                pm.AcceptChanges();
                ViewState["pm"] = pm;
                DataTable profiles = GetAccessProfiles();
                if (profiles != null)
                {
                    if (profiles.Rows.Count > 0)
                    {

                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "alert('You are not authorized to view this page.'); window.location='" + Request.ApplicationPath + "/login.aspx';", true);
                        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('You are not authorized to view this page.')", true);
                        //Response.Redirect("~/login.aspx");
                        return;
                    }
                }
                else
                {

                }
            }
        }
        protected DataTable GetAccessProfiles()
        {
            string sql = "SELECT pd.Profile_Id \n"
               + "FROM   Profile_Detail pd \n"
               + "       INNER JOIN Profile_Head ph \n"
               + "            ON  ph.profile_id = pd.Profile_Id \n"
               + "WHERE  pd.ChildMenu_Id in ('67', '60', '260', '549', '550', '587') and pd.profile_id = '" + HttpContext.Current.Session["Profile"].ToString() + "'";

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

        public void Get_origin()
        {
            DataSet ds = func.Branch();
            if (ds.Tables.Count != 0)
            {
                //  cb_Destination
                this.dd_origin.DataTextField = "BranchName";
                this.dd_origin.DataValueField = "branchCode";
                this.dd_origin.DataSource = ds.Tables[0].DefaultView;
                this.dd_origin.DataBind();
                try
                {
                    dd_origin.SelectedValue = HttpContext.Current.Session["BranchCode"].ToString();
                    //dd_origin.Enabled = false;
                }
                catch (Exception ex)
                {

                    Response.Redirect("~/Login.aspx");
                }
                DataTable dt = Cities_();
                this.dd_destination.DataTextField = "sname";
                this.dd_destination.DataValueField = "branchCode";
                this.dd_destination.DataSource = dt;//ds.Tables[0].DefaultView;
                this.dd_destination.DataBind();
                ViewState["cities"] = dt;
            }
        }

        public DataSet ServiceTypeName()
        {
            DataSet Ds_1 = new DataSet();

            try
            {
                string query = "SELECT st.name          ServiceTypeName, isintl \n"
                + "FROM   ServiceTypes     st \n"
                + "WHERE  \n"
                + "       st.[status] = '1' \n"
                + "       And st.name not in ('Expressions') \n"
                + "GROUP BY \n"
                + "       st.name,isintl  \n"
                + "ORDER BY \n"
                + "       st.name";

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


        public void Get_Service_Type()
        {
            DataSet ds = ServiceTypeName();
            if (ds.Tables.Count != 0)
            {
                //  cb_Destination
                //       this.cb_ServiceType.Items.Add(new RadComboBoxItem("Select Service Type Name", "0"));
                this.dd_serviceType.DataTextField = "ServiceTypeName";
                this.dd_serviceType.DataValueField = "ServiceTypeName";
                this.dd_serviceType.DataSource = ds.Tables[0].DefaultView;
                this.dd_serviceType.DataBind();

                //dd_serviceType.SelectedValue = "overnight";
            }
        }

        public void Get_ConType()
        {
            DataSet ds = func.ConsignmentType();
            if (ds.Tables.Count != 0)
            {
                //  cb_Destination
                //this.dd_consignmentType.Items.Add(new RadComboBoxItem("Select Consignment Type", "0"));
                this.dd_consignmentType.DataTextField = "ConsignmentType";
                this.dd_consignmentType.DataValueField = "id";
                this.dd_consignmentType.DataSource = ds.Tables[0].DefaultView;
                this.dd_consignmentType.DataBind();
                //cb_ConType.SelectedValue = "12";
            }
        }
        public void Get_PriceModifiers()
        {
            DataSet ds = func.PriceModifiers();
            if (ds.Tables.Count != 0)
            {
                ////  cb_Destination
                ////this.dd_priceModifier.Items.Add(new RadComboBoxItem("Select Price Modifier", "0"));
                //this.dd_priceModifier.DataTextField = "PriceModifier";
                //this.dd_priceModifier.DataValueField = "id";
                //this.dd_priceModifier.DataSource = ds.Tables[0].DefaultView;
                //this.dd_priceModifier.DataBind();

                ViewState["PM_"] = ds.Tables[0];
            }
        }
        //public void Get_PriceModifiers()
        //{
        //    DataSet ds = func.PriceModifiers();
        //    if (ds.Tables.Count != 0)
        //    {
        //        //  cb_Destination
        //        this.cb_PriceModifier.Items.Add(new RadComboBoxItem("Select Price Modifier", "0"));
        //        this.cb_PriceModifier.DataTextField = "PriceModifier";
        //        this.cb_PriceModifier.DataValueField = "id";
        //        this.cb_PriceModifier.DataSource = ds.Tables[0].DefaultView;
        //        this.cb_PriceModifier.DataBind();

        //        ViewState["PM_"] = ds.Tables[0];
        //    }
        //}
        public void Get_OriginExpressCenter()
        {
            try
            {
                clvar.origin = HttpContext.Current.Session["BranchCode"].ToString();
                //dd_origin.Enabled = false;
            }
            catch (Exception ex)
            {

                Response.Redirect("~/Login.aspx");
            }
            DataSet ds = func.ExpressCenterOrigin(clvar);
            if (ds.Tables.Count != 0)
            {
                //  cb_Destination
                //this.cb_Destination.Items.Add(new DropDownListItem("Select Destination", "0"));
                //this.dd_originExpressCenter.Items.Clear();
                this.dd_originExpressCenter.DataTextField = "Name";
                this.dd_originExpressCenter.DataValueField = "expresscentercode";
                this.dd_originExpressCenter.DataSource = ds.Tables[0].DefaultView;
                this.dd_originExpressCenter.DataBind();
                //this.dd_originExpressCenter.Enabled = false;

                ViewState["Ex_origin"] = ds.Tables[0];
                // hd_oecCatid.Value = ds.Tables[0].Rows[0]["CategoryID"].toString(); 
            }
        }


        #region Any Change in Price Modifier Query Must be changed in Both of the enclosed Methods
        protected void GetFuelSurcharges()
        {

            DataTable dt = new DataTable();


            try
            {

                DataTable ds = PriceModifiers();
                ViewState["PriceModifiers"] = ds;
                dt = new DataTable();
                dt.Columns.Add("ID", typeof(string));
                dt.Columns.Add("NAME", typeof(string));
                dt.Columns.Add("base", typeof(string));
                dt.AcceptChanges();
                int count = 0;
                foreach (DataRow dr in ds.Rows)
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
                    dd_priceModifier.DataSource = dt;
                    dd_priceModifier.DataTextField = "name";
                    dd_priceModifier.DataValueField = "base";
                    dd_priceModifier.DataBind();
                }
            }
            catch (Exception ex)
            { }
            finally
            {
            }

        }
        public static DataTable GetFuelSurcharges_()
        {

            DataTable dt = new DataTable();

            try
            {

                DataTable ds = PriceModifiers();

                return ds;
            }
            catch (Exception ex)
            { }
            finally
            {
            }
            return dt;
        }
        #endregion

        public static DataTable PriceModifiers()
        {
            DataTable Ds_1 = new DataTable();

            try
            {
                string query = "SELECT id, \n"
               + "       pm.name PriceModifier, \n"
               + "       pm.calculationValue, \n"
               + "       pm.[description], calculationbase, '' AlternateValue, pm.Isgst \n"
               + "FROM   PriceModifiers pm \n"
               + "WHERE  pm.[status] = '1' \n"
               + "AND pm.chkBillingModifier ='0' \n"
               + "GROUP BY \n"
               + "       id, \n"
               + "       pm.name, \n"
               + "       pm.calculationValue, \n"
               + "       pm.[description], pm.calculationbase, pm.Isgst  \n"
               + "ORDER BY pm.name";

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
        public static DataTable PriceModifiersByService(string ServiceTypeName)
        {
            DataTable Ds_1 = new DataTable();

            try
            {

                string sqlString = "SELECT PM.ID,\n" +
                "       PM.NAME PRICEMODIFIER,\n" +
                "       PM.CALCULATIONVALUE,\n" +
                "       PM.[DESCRIPTION],\n" +
                "       PM.CALCULATIONBASE,\n" +
                "       '' ALTERNATEVALUE,\n" +
                "       PM.ISGST\n" +
                "  FROM MNP_SERVICE_PRICEMODIFIERMAP MSP\n" +
                " INNER JOIN PRICEMODIFIERS PM\n" +
                "    ON PM.ID = MSP.PRICEMODIFIERID\n" +
                " WHERE MSP.SERVICETYPENAME = '" + ServiceTypeName + "'\n" +
                "   AND PM.CHKBILLINGMODIFIER = '0'\n" +
                "   AND PM.[STATUS] = '1'\n" +
                "   AND MSP.[STATUS] = '1'";


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
        public static DataTable MinimumDate(Cl_Variables clvar)
        {
            #region old
            string sqlString___ = "select MAX(DateTime) DateAllowed from Mnp_Account_DayEnd where Branch = '" + clvar.Branch + "' and doc_type = 'A'";
            string sql___ = "SELECT FORMAT(MAX(d.DateTime), 'dd/MM/yyyy')        DateAllowed \n"
               + "FROM   Mnp_Account_DayEnd     d \n"
               + "WHERE  d.Doc_Type = ( \n"
               + "           SELECT CASE  \n"
               + "                       WHEN zu.[Profile] IN ('2', '5', '9', '12', '38', '113') THEN 'O' \n"
               + "                       WHEN zu.[Profile] IN ('6', '16', '33', '37', '39', '44', '53', '52', '108') THEN  \n"
               + "                            'A' \n"
               + "                  END           Doc_type \n"
               + "           FROM   ZNI_USER1     zu \n"
               + "           WHERE  zu.U_ID = '" + HttpContext.Current.Session["U_ID"].ToString() + "' \n"
               + "       ) \n"
               + "       AND d.Branch = '" + clvar.Branch + "'";

            sqlString___ = "SELECT CASE\n" +
            "         WHEN ZU.PROFILE IN ('2', '5', '9', '12', '38', '113') AND\n" +
            "              MAX(A.OPSDATEALLOWED) > MAX(A.ACCDATEALLOWED) THEN\n" +
            "          FORMAT(MAX(A.OPSDATEALLOWED), 'dd/MM/yyyy') \n" +
            "         WHEN ZU.PROFILE IN ('2', '5', '9', '12', '38', '113') AND\n" +
            "              MAX(A.OPSDATEALLOWED) <= MAX(A.ACCDATEALLOWED) THEN\n" +
            "           FORMAT(MAX(A.ACCDATEALLOWED), 'dd/MM/yyyy')\n" +
            "         WHEN ZU.PROFILE IN\n" +
            "              ('6', '16', '33', '37', '39', '44', '53', '52', '108') THEN\n" +
            "          FORMAT(MAX(A.ACCDATEALLOWED), 'dd/MM/yyyy')\n" +
            "       END DateAllowed\n" +
            "  FROM (SELECT MAX(D.DATETIME) ACCDATEALLOWED, 0 OPSDATEALLOWED\n" +
            "          FROM MNP_ACCOUNT_DAYEND D\n" +
            "         WHERE D.BRANCH = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'\n" +
            "           AND D.DOC_TYPE = 'A'\n" +
            "        UNION ALL\n" +
            "        SELECT 0 ACCDATEALLOWED, MAX(D.DATETIME) OPSDATEALLOWED\n" +
            "          FROM MNP_ACCOUNT_DAYEND D\n" +
            "         WHERE D.BRANCH = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'\n" +
            "           AND D.DOC_TYPE = 'O') A\n" +
            " INNER JOIN ZNI_USER1 ZU\n" +
            "    ON ZU.U_ID = '" + HttpContext.Current.Session["U_ID"].ToString() + "'\n" +
            " GROUP BY ZU.PROFILE";

            #endregion

            string sqlString = "SELECT CASE\n" +
            "         WHEN ZU.CNApproval ='1' AND\n" +
            "              MAX(A.OPSDATEALLOWED) > MAX(A.ACCDATEALLOWED) THEN\n" +
            "          MAX(A.OPSDATEALLOWED)\n" +
            "         WHEN ZU.CNApproval ='1' AND\n" +
            "              MAX(A.OPSDATEALLOWED) <= MAX(A.ACCDATEALLOWED) THEN\n" +
            "          MAX(A.ACCDATEALLOWED)\n" +
            "         WHEN ZU.CNApproval ='2' THEN\n" +
            "          MAX(A.ACCDATEALLOWED)\n" +
            "       END DateAllowed\n" +
            "  FROM (SELECT MAX(D.DATETIME) ACCDATEALLOWED, 0 OPSDATEALLOWED\n" +
            "          FROM MNP_ACCOUNT_DAYEND D\n" +
            "         WHERE D.BRANCH = '" + clvar.Branch + "'\n" +
            "           AND D.DOC_TYPE = 'A'\n" +
            "        UNION ALL\n" +
            "        SELECT 0 ACCDATEALLOWED, MAX(D.DATETIME) OPSDATEALLOWED\n" +
            "          FROM MNP_ACCOUNT_DAYEND D\n" +
            "         WHERE D.BRANCH = '" + clvar.Branch + "'\n" +
            "           AND D.DOC_TYPE = 'O') A\n" +
            " INNER JOIN ZNI_USER1 ZU\n" +
            "    ON ZU.U_ID = '" + HttpContext.Current.Session["U_ID"].ToString() + "'\n" +
            " GROUP BY ZU.CNApproval";

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

            sqlString = "select b.branchCode , b.sname + ' - ' + b.name SNAME, ec.ExpressCenterCode ECCode\n" +
            "  from branches b\n" +
            "  left outer join ExpressCenters ec\n" +
            "    on ec.bid = b.branchCode\n" +
            "   and ec.Main_EC = '1'\n" +
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

        public DataSet CustomerInformation(Cl_Variables clvar)
        {
            DataSet Ds_1 = new DataSet();

            try
            {
                string query = "SELECT * FROM CreditClients cc WHERE cc.accountNo = '" + clvar.AccountNo + "' and cc.branchCode='" + clvar.Branch + "' ";

                string sqlString = "SELECT cc.*,\n" +
                "       case\n" +
                "         when cu.CreditClientID is not null AND cc.IsCOD = '1 then\n" +
                "          '1'\n" +
                "         else\n" +
                "          '0'\n" +
                "       end isAPIClient\n" +
                "  FROM CreditClients cc\n" +
                "  left outer join CODUsers cu\n" +
                "    --on cu.accountno = cc.accountno\n" +
                "   on cu.creditCLientID = cc.id and cu.isCod = '1'\n" +
                "\n" +
                " where cc.accountNo = '" + clvar.AccountNo + "'\n" +
                "   and cc.branchCode = '" + clvar.Branch + "'\n" +
                "   and cc.isActive = '1'  ";


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

        public string updateChargedAmount(Cl_Variables clvar)
        {
            string query = "UPDATE consignment set chargedAmount = '" + txt_chargedAmount.Text + "' where consignmentNumber = '" + clvar.consignmentNo + "'";

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

        public DataTable GetCODConsignmentForApproval(Cl_Variables clvar)
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
            "  from consignment c\n" +
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
            "   and c.consignmentNumber = '" + clvar.consignmentNo + "'\n" +
            " order by consignmentNumber, SortOrder";

            sqlString = "SELECT * FROM CODConsignmentDetail cd WHERE cd.consignmentNumber='" + clvar.consignmentNo + "'";

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

        public DataSet Check_CODConsignmentDetail(Cl_Variables clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string isApiClient = ViewState["isAPIClient"].ToString();
                string query = "";
                if (isApiClient == "0")
                {
                    query = "select * from CODConsignmentDetail where consignmentNumber = '" + clvar.consignmentNo + "' ";
                }
                else
                {
                    query = "select * from CODConsignmentDetail_new where consignmentNumber = '" + clvar.consignmentNo + "' ";
                }


                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
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

        public void AlertMessage(string message)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + message + "')", true);
            return;
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
                // Create a request using a URL that can receive a post. 
                //   WebRequest request = WebRequest.Create("https://services.converget.com/MTEntryDyn/?");
                // Set the Method property of the request to POST.

                // Create POST data and convert it to a byte array.
                if (txt_cnNumber.Text != string.Empty)
                {
                    //  DataTable dt = clvar.GetMaxOrderNo();
                    string newResp = "Dear Customer, A shipment CN " + resp + " is booked successfully. You can visit www.mulphilog.com or call us on 111-202-202 to track delivery status. Thank you";
                    newResp = "Thank you for choosing M&P Courier. Your shipment with CN #: " + resp + " is booked successfully. Track it at www.mulphilog.com or call 111 202 202.";
                    newResp = newResp.Replace("&", "%26");
                    newResp = newResp.Replace("#", "%23");
                    //string resp_ = "Dear Valued Customer, We have received your shipment under CN:" + resp + "for " + Consignee + " - " + destination + " Amount :" + string.Format("{0:N0}", Double.Parse(txt_TotalAmount.Text)) + ". Please visit www.mulphilog.com or call us on 021-111-202-202 to track delivery status. Thank you";
                    String Mobile = HttpUtility.UrlEncode(IsMobileNumberValid(mobile));
                    String Response = HttpUtility.UrlEncode(resp);
                    string postData = "";//"to=" + Mobile + "&text=" + Response + "&from=OCS&username=sales&password=salestest8225";//"PhoneNumber=" + Mobile + "&Text=" + Response;
                    string smsContent = newResp;//"Dear Customer, M&P Courier visited your address to deliver " + hd_consigner.Value + " shipment under CN #" + row.Cells[1].Text + " but it could not deliver. Pls contact M&P Customer Services 111-202-202";
                    string query2 = "insert into mnp_smsStatus (ConsignmentNumber, Recepient, MessageContent, Status, Createdon, CreatedBy, RunsheetNumber) \n" +
                             "values ('" + resp + "', '" + mobile + "', '" + smsContent + "', '0', GETDATE(), '" + HttpContext.Current.Session["U_ID"].ToString() + "', 'N/A/A')";

                    int count = 0;
                    string error = "";
                    SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
                    try
                    {
                        sqlcon.Open();
                        SqlCommand sqlcmd = new SqlCommand(query2, sqlcon);
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

                }
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
                // Create a request using a URL that can receive a post. 
                //   WebRequest request = WebRequest.Create("https://services.converget.com/MTEntryDyn/?");
                // Set the Method property of the request to POST.

                // Create POST data and convert it to a byte array.
                if (txt_cnNumber.Text != string.Empty)
                {
                    //  DataTable dt = clvar.GetMaxOrderNo();
                    string newResp = "Dear Customer, Your shipment CN " + resp + " is received on " + DateTime.Now.Date.ToString("yyyy-MM-dd") + ". Thank You. For further details, contact us on 111-202-202";
                    newResp = "Dear Customer, M&P Courier just received a shipment with CN #: " + resp + " for delivery to you. Track it at www.mulphilog.com or call 111 202 202. ";
                    newResp = newResp.Replace("&", "%26");
                    newResp = newResp.Replace("#", "%23");
                    string resp_ = "Dear Customer, A shipment has been booked under CN:" + resp + "for " + Consignee + " - " + destination + ".You can visit www.ocs.com.pk or call us on 021-111 202 202 to track delivery status. Thank you";
                    String Mobile = HttpUtility.UrlEncode(IsMobileNumberValid(mobile));

                    string smsContent = resp_;//"Dear Customer, M&P Courier visited your address to deliver " + hd_consigner.Value + " shipment under CN #" + row.Cells[1].Text + " but it could not deliver. Pls contact M&P Customer Services 111-202-202";
                    string query2 = "insert into mnp_smsStatus (ConsignmentNumber, Recepient, MessageContent, Status, Createdon, CreatedBy, RunsheetNumber) \n" +
                             "values ('" + resp + "', '" + mobile + "', '" + smsContent + "', '0', GETDATE(), '" + HttpContext.Current.Session["U_ID"].ToString() + "', 'N/A/A')";

                    int count = 0;
                    string error = "";
                    SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
                    try
                    {
                        sqlcon.Open();
                        SqlCommand sqlcmd = new SqlCommand(query2, sqlcon);
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

                }
            }
            catch (Exception Err)
            {
                //listBox1.Items.Add("Failed " + Err + "Mobile Number " + clvar.MobileNumber + " and Response " + resp);
            }

        }

        public DataTable Add_OcsValidation(Cl_Variables clvar)
        {
            DataSet ds = new DataSet();

            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand("SP_UpdateCNPrice_OCS_Validation", orcl);
                orcd.Parameters.AddWithValue("@ConsignmentNo", clvar.consignmentNo);
                orcd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                orcl.Close();
            }
            catch (Exception)
            { }

            return ds.Tables[0];
        }

        //public DataTable GetECBySequence(string consignmentNumber)
        //{

        //    string sqlString = "SELECT *\n" +
        //    "  FROM Mnp_RiderCNSequence mrc\n" +
        //    " WHERE '" + consignmentNumber + "' BETWEEN mrc.SequenceStart AND mrc.SequenceEnd\n" +
        //    "   AND mrc.Branch = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'";

        //    sqlString = "SELECT mrc.*, ec.name ECName\n" +
        //    "  FROM Mnp_RiderCNSequence mrc\n" +
        //    " INNER JOIN ExpressCenters ec\n" +
        //    "    ON ec.expressCenterCode = mrc.ExpressCenter\n" +
        //    "   AND ec.bid = mrc.Branch\n" +
        //    " WHERE '" + consignmentNumber + "' BETWEEN mrc.SequenceStart AND mrc.SequenceEnd";

        //    DataTable dt = new DataTable();
        //    SqlConnection con = new SqlConnection(clvar.Strcon());
        //    try
        //    {
        //        con.Open();
        //        SqlDataAdapter sda = new SqlDataAdapter(sqlString, con);
        //        sda.Fill(dt);

        //    }
        //    catch (Exception ex)
        //    { }
        //    finally { con.Close(); }
        //    return dt;

        //}

        [WebMethod]
        public static string RefreshTime(string a)
        {
            return DateTime.Now.ToShortDateString();
        }

        public class ConsignmentClass
        {
            public string CN { get; set; }
            public string BKDate { get; set; }
            public string Acc { get; set; }
            public string CreditClientID { get; set; }
            public string ServiceType { get; set; }
            public string Consigner { get; set; }
            public string Consignee { get; set; }
            public string ConsignerMob { get; set; }
            public string ConsigneeMob { get; set; }
            public string Destination { get; set; }
            public string DestinationName { get; set; }
            public string DestinationEC { get; set; }
            public string Rider { get; set; }
            public string Dimensions { get; set; }
            public string VolWgt { get; set; }
            public string DnsWgt { get; set; }
            public string Weight { get; set; }
            public string Pieces { get; set; }
            public string Address { get; set; }
            public string shipperAddress { get; set; }
            public string PakageContents { get; set; }
            public string OriginEC { get; set; }
            public string OriginECName { get; set; }
            public string CNType { get; set; }
            public string CNTypeName { get; set; }
            public string Coupon { get; set; }
            public string SpecialInstructions { get; set; }
            public string RPDate { get; set; }
            public string Approved { get; set; }
            public string InvStatus { get; set; }
            public string InvNumber { get; set; }
            public string CODRef { get; set; }
            public string CODDesc { get; set; }
            public string CODAmt { get; set; }
            public string AddServ { get; set; }
            public string pmID { get; set; }
            public string calBase { get; set; }
            public string calValue { get; set; }
            public string modCalValue { get; set; }
            public string calGst { get; set; }
            public string isTaxable { get; set; }
            public string AltValue { get; set; }
            public string AddChrg { get; set; }
            public string ChrgAmt { get; set; }
            public string TotalAmt { get; set; }
            public string Gst { get; set; }
            public string ServerResponse { get; set; }
            public string isNew { get; set; }
            public string isCOD { get; set; }
            public string InsertType { get; set; }
        }

        public class CnDimensions
        {
            public string ConsignmentNumber { get; set; }
            public string ItemNumber { get; set; }
            public string Length { get; set; }
            public string Width { get; set; }
            public string Height { get; set; }
            public string Pieces { get; set; }
            public string VolWeight { get; set; }
            public string denseWeight { get; set; }
        }

        public class ReturnConsignmentClass
        {
            public ConsignmentClass[] Consignments { get; set; }
            public CnDimensions[] Dimensions { get; set; }
        }

        [WebMethod]
        public static ReturnConsignmentClass GetConsignmentDetails(string consignmentStart, string numberOfCN)
        {
            ReturnConsignmentClass resp_ = new ReturnConsignmentClass();
            List<ConsignmentClass> resp = new List<ConsignmentClass>();
            ConsignmentClass cn = new ConsignmentClass();
            Int64 start = 0;
            Int64.TryParse(consignmentStart, out start);

            int till = 0;
            int.TryParse(numberOfCN, out till);

            if (start == 0 || till == 0)
            {
                cn.ServerResponse = "Invalid Consignment Parameters";
                resp.Add(cn);
                resp_.Consignments = resp.ToArray();
                return resp_;
                //return resp.ToArray();
            }
            string consignmentnumbers = "";

            DataTable cns = new DataTable();
            cns.Columns.Add("ConsignmentNumber", typeof(string));

            start = start + till;
            for (int i = till; i > 0; i--)
            {
                start--;
                cn = new ConsignmentClass();
                cn.CN = start.ToString();
                resp.Add(cn);
                consignmentnumbers += "'" + start.ToString() + "'";
                cns.Rows.Add(start.ToString());

            }
            consignmentnumbers = consignmentnumbers.Replace("''", "','");


            DataSet available = GetConsignmentAvailability(cns, 0, "");


            string sqlString = "SELECT A.*,\n" +
            "       i.deliveryStatus,\n" +
            "       CASE\n" +
            "         WHEN i.IsInvoiceCanceled = '1' THEN\n" +
            "          ''\n" +
            "         ELSE\n" +
            "          i.invoiceNumber\n" +
            "       END invoiceNumber_\n" +
            "  FROM (SELECT --c.bookingDate,\n" +
            "         c.consignmentNumber,c.InsertType,\n" +
            "         c.customerType,\n" +
            "         c.creditClientId,\n" +
            "         c.orgin,\n" +
            "         c.serviceTypeName,\n" +
            "         c.consigner,\n" +
            "         c.consignee,\n" +
            "         c.destination,\n" +
            "         b.sname destinationName,\n" +
            "         c.weight,\n" +
            "         c.riderCode,\n" +
            "         c.originExpressCenter,\n" +
            "         c.consignmentTypeId,\n" +
            "         c.chargedAmount,\n" +
            "         CAST(c.isApproved AS VARCHAR) isApproved,\n" +
            "         c.consignerAccountNo accountNo,\n" +
            "         --ic.invoiceNumber,\n" +
            "         --    i.startDate                       ReportingDate,\n" +
            "         --    i.deliveryStatus,\n" +
            "         cm.priceModifierId,\n" +
            "         p.name priceModifierName,\n" +
            "         cm.calculatedValue,\n" +
            "         cm.modifiedCalculationValue modifiedCalculatedValue,\n" +
            "         cm.calculationBase,\n" +
            "         cm.isTaxable,\n" +
            "         cm.SortOrder,\n" +
            "         p.description,\n" +
            "         c.destinationExpressCenterCode,\n" +
            "         FORMAT(c.accountReceivingDate, 'dd/MM/yyyy') accountReceivingDate,\n" +
            "         FORMAT(c.bookingDate, 'dd/MM/yyyy') BookingDate,\n" +
            "         c.COD,\n" +
            "         c.gst,\n" +
            "         c.totalAmount,\n" +
            "         ic.invoiceNumber,\n" +
            "         c.consignerCellNo,\n" +
            "         c.ConsigneePhoneNo,\n" +
            "         c.address,\n" +
            "         c.pieces,\n" +
            "         c.couponNumber,\n" +
            "         CASE\n" +
            "           when cns.CreditClientID is null then\n" +
            "            '0'\n" +
            "           else\n" +
            "            '1'\n" +
            "         end PortalCN, ISNULL(c.width, 0) width, ISNULL(c.breadth, 0) breadth, ISNULL(c.height, 0) height, (ISNULL(c.width, 0) * ISNULL(c.breadth, 0) * ISNULL(c.height, 0)) VolWeight,  c.denseWeight, c.remarks, cm.AlternateValue\n" +
            "        --   i.IsInvoiceCanceled\n" +
            "          FROM consignment c\n" +
            "          LEFT OUTER JOIN InvoiceConsignment ic\n" +
            "            ON c.consignmentNumber = ic.consignmentNumber\n" +
            "        --LEFT OUTER JOIN Invoice i\n" +
            "        --     ON  i.invoiceNumber = ic.invoiceNumber\n" +
            "          LEFT OUTER JOIN ConsignmentModifier cm\n" +
            "            ON c.consignmentNumber = cm.consignmentNumber\n" +
            "          LEFT OUTER JOIN PriceModifiers p\n" +
            "            ON cm.priceModifierId = p.id\n" +
            "         INNER JOIN CreditClients cc\n" +
            "            ON c.creditClientId = cc.id\n" +
            "          LEFT OUTER JOIN CODUSERS cns\n" +
            "            on cns.CreditClientID = cc.id and cns.isCOD = '1'\n" +
            "          LEFT OUTER JOIN BRANCHES b on b.branchCode = c.destination\n" +
            "         WHERE /*c.orgin = '4'\n" +
            "               and c.consignmentTypeId <> '10'\n" +
            "               and */\n" +
            "         c.consignmentNumber in (" + consignmentnumbers + ")\n" +
            "        --     AND ISNULL(i.IsInvoiceCanceled, 0) = '0'\n" +
            "        ) A\n" +
            "  LEFT OUTER JOIN Invoice i\n" +
            "    ON A.invoiceNumber = i.invoiceNumber\n" +
            " ORDER BY i.createdOn desc";



            #region MyRegion
            //sqlString = "SELECT A.*,\n" +
            //"       i.deliveryStatus,\n" +
            //"       CASE\n" +
            //"            WHEN i.IsInvoiceCanceled = '1' THEN ''\n" +
            //"            ELSE i.invoiceNumber\n" +
            //"       END             invoiceNumber_\n" +
            //"FROM   (\n" +
            //"           SELECT --c.bookingDate,\n" +
            //"                  c.consignmentNumber,\n" +
            //"                  c.customerType,\n" +
            //"                  c.creditClientId,\n" +
            //"                  c.orgin,\n" +
            //"                  c.serviceTypeName,\n" +
            //"                  c.consigner,\n" +
            //"                  c.consignee,\n" +
            //"                  c.destination,\n" +
            //"                  b.sname                  destinationName,\n" +
            //"                  c.weight,\n" +
            //"                  c.riderCode,\n" +
            //"                  c.originExpressCenter,\n" +
            //"                  c.consignmentTypeId,\n" +
            //"                  c.chargedAmount,\n" +
            //"                  CAST(c.isApproved AS VARCHAR) isApproved,\n" +
            //"                  c.consignerAccountNo     accountNo,\n" +
            //"                  --ic.invoiceNumber,\n" +
            //"                  --    i.startDate                       ReportingDate,\n" +
            //"                  --    i.deliveryStatus,\n" +
            //"                  ISNULL(cm.priceModifierId,0) priceModifierId,\n" +
            //"                  p.name                   priceModifierName,\n" +
            //"                  ISNULL(cm.calculatedValue,0) calculatedValue,\n" +
            //"                  ISNULL(cm.calculatedGST,0) calculatedGST, \n" +
            //"                  ISNULL(cm.modifiedCalculationValue,0) modifiedCalculatedValue,\n" +
            //"                  ISNULL(cm.calculationBase,0) calculationBase,\n" +
            //"                  ISNULL(cm.isTaxable,0) isTaxable,\n" +
            //"                  ISNULL(cm.AlternateValue, 0) alternateValue,\n" +
            //"                  cm.SortOrder,\n" +
            //"                  p.description,\n" +
            //"                  c.destinationExpressCenterCode,\n" +
            //"                  FORMAT(c.accountReceivingDate, 'dd/MM/yyyy')\n" +
            //"                  accountReceivingDate,\n" +
            //"                  FORMAT(c.bookingDate, 'dd/MM/yyyy') BookingDate,\n" +
            //"                  c.COD,\n" +
            //"                  c.gst,\n" +
            //"                  c.totalAmount,\n" +
            //"                  ic.invoiceNumber,\n" +
            //"                  c.consignerCellNo,\n" +
            //"                  c.ConsigneePhoneNo,\n" +
            //"                  c.address,\n" +
            //"                  c.pieces,\n" +
            //"                  c.couponNumber,\n" +
            //"                  CASE\n" +
            //"                       WHEN cns.CreditClientID IS NULL THEN '0'\n" +
            //"                       ELSE '1'\n" +
            //"                  END                      PortalCN,\n" +
            //"                  ISNULL(c.width, 0)       width,\n" +
            //"                  ISNULL(c.breadth, 0)     breadth,\n" +
            //"                  ISNULL(c.height, 0)      height,\n" +
            //"                  (\n" +
            //"                      (ISNULL(c.width, 0) * ISNULL(c.breadth, 0) * ISNULL(c.height, 0))/5000\n" +
            //"                  )                        VolWeight1,\n" +
            //"                  c.denseWeight,\n" +
            //"                  c.remarks,\n" +
            //    //"                  cm.AlternateValue,\n" +
            //"                  ec.expressCenterCode orgEC,\n" +
            //"                  ec.name orgECName,ct.name CNTypeName, c.volWeight\n" +
            //"           FROM   consignment c\n" +
            //"                  LEFT OUTER JOIN InvoiceConsignment ic\n" +
            //"                       ON  c.consignmentNumber = ic.consignmentNumber\n" +
            //"                  LEFT OUTER JOIN ConsignmentModifier cm\n" +
            //"                       ON  c.consignmentNumber = cm.consignmentNumber\n" +
            //"                  LEFT OUTER JOIN PriceModifiers p\n" +
            //"                       ON  cm.priceModifierId = p.id\n" +
            //"                  INNER JOIN CreditClients cc\n" +
            //"                       ON  c.creditClientId = cc.id\n" +
            //"                  LEFT OUTER JOIN CODUSERS cns\n" +
            //"                       ON  cns.CreditClientID = cc.id\n" +
            //"                       AND cns.isCOD = '1'\n" +
            //"                  LEFT OUTER JOIN BRANCHES b\n" +
            //"                       ON  b.branchCode = c.destination\n" +
            //"                  LEFT OUTER JOIN Riders r\n" +
            //"                  ON r.riderCode = c.riderCode\n" +
            //"                  AND r.branchId = c.orgin\n" +
            //"                  AND r.[status] = '1'\n" +
            //"                  LEFT OUTER JOIN ExpressCenters ec\n" +
            //"                  ON ec.expressCenterCode = r.expressCenterId\n" +
            //"                  AND ec.bid = c.orgin\n" +
            //"                  AND ec.[status] = '1'\n" +
            //"                  LEFT OUTER JOIN ConsignmentType ct\n" +
            //"                  ON ct.id = c.consignmentTypeId\n" +
            //"                  AND ct.[status] = '1'" +
            //"           WHERE  c.consignmentNumber IN (" + consignmentnumbers + ")\n" +
            //"       ) A\n" +
            //"       LEFT OUTER JOIN Invoice i\n" +
            //"            ON  A.invoiceNumber = i.invoiceNumber\n" +
            //"ORDER BY\n" +
            //"       i.createdOn     DESC"; 
            #endregion



            sqlString = "SELECT A.*,\n" +
            "       i.deliveryStatus,\n" +
            "       CASE\n" +
            "            WHEN i.IsInvoiceCanceled = '1' THEN ''\n" +
            "            ELSE i.invoiceNumber\n" +
            "       END             invoiceNumber_\n" +
            "FROM   (\n" +
            "           SELECT --c.bookingDate,c.InsertType,\n" +
            "                  c.consignmentNumber,cc.IsCOD creditclientiscod,c.InsertType,\n" +
            "                  c.customerType,\n" +
            "                  c.creditClientId,\n" +
            "                  c.orgin,\n" +
            "                  c.serviceTypeName,\n" +
            "                  c.consigner,\n" +
            "                  c.consignee,\n" +
            "                  c.destination,\n" +
            "                  b.sname                  destinationName,\n" +
            "                  c.weight,\n" +
            "                  c.riderCode,\n" +
            "                  c.originExpressCenter,\n" +
            "                  c.consignmentTypeId,\n" +
            "                  c.chargedAmount,\n" +
            "                  CAST(c.isApproved AS VARCHAR) isApproved,\n" +
            "                  c.consignerAccountNo     accountNo,\n" +
            "                  --ic.invoiceNumber,\n" +
            "                  --    i.startDate                       ReportingDate,\n" +
            "                  --    i.deliveryStatus,\n" +
            "                  ISNULL(cm.priceModifierId,0) priceModifierId,\n" +
            "                  p.name                   priceModifierName,\n" +
            "                  ISNULL(cm.calculatedValue,0) calculatedValue,\n" +
            "                  ISNULL(cm.calculatedGST,0) calculatedGST, \n" +
            "                  ISNULL(cm.modifiedCalculationValue,0) modifiedCalculatedValue,\n" +
            "                  ISNULL(cm.calculationBase,0) calculationBase,\n" +
            "                  ISNULL(cm.isTaxable,0) isTaxable,\n" +
            "                  ISNULL(cm.AlternateValue, 0) alternateValue,\n" +
            "                  cm.SortOrder,\n" +
            "                  p.description,\n" +
            "                  c.destinationExpressCenterCode,\n" +
            "                  FORMAT(c.accountReceivingDate, 'dd/MM/yyyy')\n" +
            "                  accountReceivingDate,\n" +
            "                  FORMAT(c.bookingDate, 'dd/MM/yyyy') BookingDate,\n" +
            "                  c.COD,\n" +
            "                  c.gst,\n" +
            "                  c.totalAmount,\n" +
            "                  ic.invoiceNumber,\n" +
            "                  c.consignerCellNo,\n" +
            "                  c.ConsigneePhoneNo,\n" +
            "                  c.address,c.shipperAddress, c.PakageContents,\n" +
            "                  c.pieces,\n" +
            "                  c.couponNumber,\n" +
            "                  CASE\n" +
            "                       WHEN cns.CreditClientID IS NULL THEN '0'\n" +
            "                       ELSE '1'\n" +
            "                  END                      PortalCN,\n" +
            "                  ISNULL(c.width, 0)       width,\n" +
            "                  ISNULL(c.breadth, 0)     breadth,\n" +
            "                  ISNULL(c.height, 0)      height,\n" +
            "                  (\n" +
            "                      (ISNULL(c.width, 0) * ISNULL(c.breadth, 0) * ISNULL(c.height, 0))/5000\n" +
            "                  )                        VolWeight1,\n" +
            "                  c.denseWeight,\n" +
            "                  c.remarks,\n" +
            //"                  cm.AlternateValue,\n" +
            "                  ec.expressCenterCode orgEC,\n" +
            "                  ec.name orgECName,ct.name CNTypeName, c.volWeight, ec2.name OriginExpressCenterName\n" +
            "           FROM   consignment c\n" +
            "                  LEFT OUTER JOIN InvoiceConsignment ic\n" +
            "                       ON  c.consignmentNumber = ic.consignmentNumber\n" +
            "                  LEFT OUTER JOIN ConsignmentModifier cm\n" +
            "                       ON  c.consignmentNumber = cm.consignmentNumber\n" +
            "                  LEFT OUTER JOIN (\n" +
            "                           SELECT pm.*, mc.CreditClientID\n" +
            "                           FROM   PriceModifiers pm\n" +
            "                                  INNER JOIN MnP_Service_PriceModifierMap mp\n" +
            "                                       ON  mp.PriceModifierID = pm.id\n" +
            "                                  INNER JOIN MnP_Customer_ServiceMap mc\n" +
            "                                       ON  mc.ServiceTypeName = mp.ServiceTypeName\n" +
            "                       ) p\n" +
            "                       ON  cm.priceModifierId = p.id\n" +
            "                       AND c.creditClientId = p.CreditClientID\n" +
            "                  INNER JOIN CreditClients cc\n" +
            "                       ON  c.creditClientId = cc.id\n" +
            "                  LEFT OUTER JOIN CODUSERS cns\n" +
            "                       ON  cns.CreditClientID = cc.id\n" +
            "                       AND cns.isCOD = '1'\n" +
            "                  LEFT OUTER JOIN BRANCHES b\n" +
            "                       ON  b.branchCode = c.destination\n" +
            "                  LEFT OUTER JOIN Riders r\n" +
            "                  ON r.riderCode = c.riderCode\n" +
            "                  AND r.branchId = c.orgin\n" +
            "                  AND r.[status] = '1'\n" +
            "                  LEFT OUTER JOIN ExpressCenters ec\n" +
            "                  ON ec.expressCenterCode = r.expressCenterId\n" +
            "                  AND ec.bid = c.orgin\n" +
            "                  AND ec.[status] = '1'\n" +
            "                  LEFT OUTER JOIN ExpressCenters ec2\n" +
            "                  ON ec2.expressCenterCode = c.originExpressCenter\n" +
            "                  AND ec2.bid = c.orgin\n" +
            "                  AND ec2.[status] = '1'\n" +
            "                  LEFT OUTER JOIN ConsignmentType ct\n" +
            "                  ON ct.id = c.consignmentTypeId\n" +
            "                  AND ct.[status] = '1' \n" +
            "           WHERE  c.consignmentNumber IN (" + consignmentnumbers + ")\n" +
            "       ) A\n" +
            "       LEFT OUTER JOIN Invoice i\n" +
            "            ON  A.invoiceNumber = i.invoiceNumber\n" +
            "ORDER BY\n" +
            "       i.createdOn     DESC";




            SqlConnection con = new SqlConnection(clvar.Strcon());
            DataTable dt = new DataTable();
            DataTable dimDt = new DataTable();
            string dimensionQuery = "SELECT * FROM Consignment_Dimensions cd where cd.consignmentNumber in (" + consignmentnumbers + ") order by 2, 3";
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sqlString, con);
                sda.Fill(dt);
                sda = new SqlDataAdapter(dimensionQuery, con);
                sda.Fill(dimDt);
            }
            catch (Exception ex)
            {
                resp.Clear();
                cn.ServerResponse = ex.Message;
                resp.Add(cn);
                con.Close();
                resp_.Consignments = resp.ToArray();
                return resp_;
                //return resp.ToArray();
            }
            List<string> unfoundCNs = new List<string>();
            foreach (ConsignmentClass item in resp)
            {
                DataRow[] dr = dt.Select("ConsignmentNumber = '" + item.CN + "'");
                DataRow availableRow = available.Tables[0].Select("ConsignmentNumber = '" + item.CN + "'").FirstOrDefault();
                if (availableRow["ZoneAllowed"].ToString() == "N" && availableRow["CODReserved"].ToString() != "Y")
                {
                    item.ServerResponse = "Consignment Not allowed in your Zone.";
                }
                else if (dr == null)
                {
                    //unfoundCNs.Add(item.CN);
                    item.isNew = "Y";
                }
                else if (dr.Count() == 0)
                {
                    item.ServerResponse = "OK";
                    item.isNew = "Y";
                }
                else
                {
                    //if (dr[0]["COD"].ToString() == "1")
                    //{
                    //    cn.ServerResponse = "In Bulk Screen COD Consignment Can't be Used.";
                    //    resp.Add(cn);
                    //    resp_.Consignments = resp.ToArray();
                    //    return resp_;
                    //}
                    if (dr[0]["COD"].ToString() == "1")
                    {
                        if (dr[0]["isApproved"].ToString() == "0" || dr[0]["creditclientiscod"].ToString() == "False")
                        {
                            cn.ServerResponse = "COD Consignment is not Approved.";
                            resp.Add(cn);
                            resp_.Consignments = resp.ToArray();
                            return resp_;
                        }
                    }
                    cn = new ConsignmentClass();
                    item.CN = dr[0]["ConsignmentNumber"].ToString();
                    item.Acc = dr[0]["accountNo"].ToString();
                    item.CreditClientID = dr[0]["CreditClientID"].ToString();
                    if (item.Acc == "0")
                    {
                        if (availableRow["ExpressCenter"].ToString().Trim() != "")
                        {
                            item.OriginEC = dr[0]["OriginExpressCenter"].ToString();
                            item.OriginECName = dr[0]["OriginExpressCenterName"].ToString();
                        }
                        else
                        {
                            item.OriginEC = "0";
                            item.OriginECName = "";
                        }

                    }
                    else
                    {
                        item.OriginEC = dr[0]["OrgEC"].ToString();
                        item.OriginECName = dr[0]["OrgECName"].ToString();
                    }
                    item.InsertType = dr[0]["InsertType"].ToString();
                    item.Address = dr[0]["Address"].ToString();
                    item.shipperAddress = dr[0]["shipperAddress"].ToString();
                    item.PakageContents = dr[0]["PakageContents"].ToString();
                    item.Approved = dr[0]["IsApproved"].ToString();
                    item.BKDate = dr[0]["BookingDate"].ToString();
                    item.ChrgAmt = dr[0]["ChargedAmount"].ToString();
                    item.CNType = dr[0]["ConsignmentTypeID"].ToString();
                    item.CNTypeName = dr[0]["CNTypeName"].ToString();
                    //Consignment Type Name Lana hai
                    item.Consignee = dr[0]["Consignee"].ToString();
                    item.ConsigneeMob = dr[0]["ConsigneePhoneNo"].ToString();
                    item.Consigner = dr[0]["Consigner"].ToString();
                    item.ConsignerMob = dr[0]["ConsignerCellNo"].ToString();
                    item.Coupon = dr[0]["CouponNumber"].ToString();
                    item.CreditClientID = dr[0]["CreditClientID"].ToString();
                    item.Destination = dr[0]["Destination"].ToString();
                    item.DestinationName = dr[0]["DestinationName"].ToString();
                    item.Dimensions = dr[0]["breadth"].ToString() + "X" + dr[0]["Width"].ToString() + "X" + dr[0]["Height"].ToString();
                    item.DnsWgt = dr[0]["DenseWeight"].ToString();
                    item.Gst = dr[0]["GSt"].ToString();
                    item.InvNumber = dr[0]["InvoiceNumber_"].ToString();
                    item.InvStatus = dr[0]["DeliveryStatus"].ToString();

                    //cn.OriginECName
                    item.Pieces = dr[0]["Pieces"].ToString();
                    item.Rider = dr[0]["RiderCode"].ToString();
                    item.RPDate = dr[0]["accountReceivingDate"].ToString();
                    item.ServiceType = dr[0]["ServiceTypeName"].ToString();
                    item.SpecialInstructions = dr[0]["Remarks"].ToString();
                    item.TotalAmt = dr[0]["TotalAmount"].ToString();
                    item.isCOD = dr[0]["COD"].ToString();
                    double tempVolWeight = 0;
                    int tempPieces = 1;

                    double.TryParse(dr[0]["VolWeight"].ToString(), out tempVolWeight);
                    int.TryParse(dr[0]["Pieces"].ToString(), out tempPieces);

                    item.VolWgt = dr[0]["VolWeight"].ToString(); //(tempVolWeight * tempPieces).ToString();
                    item.Weight = dr[0]["Weight"].ToString();
                    item.pmID = dr[0]["priceModifierID"].ToString();
                    float calculatedValue = 0;
                    float calculatedGst = 0;

                    float.TryParse(dr[0]["CalculatedValue"].ToString(), out calculatedValue);
                    float.TryParse(dr[0]["CalculatedGST"].ToString(), out calculatedGst);
                    item.AddServ = dr[0]["PriceModifierName"].ToString();
                    item.AddChrg = (calculatedValue + calculatedGst).ToString();

                    item.calBase = dr[0]["CalculationBase"].ToString();
                    item.calValue = dr[0]["calculatedValue"].ToString();
                    item.calGst = dr[0]["calculatedGST"].ToString();
                    item.isTaxable = dr[0]["isTaxable"].ToString();
                    item.modCalValue = dr[0]["ModifiedcalculatedValue"].ToString();
                    item.AltValue = dr[0]["alternateValue"].ToString();
                    item.ServerResponse = "OK";


                    //resp.Add(cn);
                }
            }

            List<CnDimensions> Dimensions = new List<CnDimensions>();
            foreach (DataRow dr in dimDt.Rows)
            {
                CnDimensions cnd = new CnDimensions();
                cnd.ConsignmentNumber = dr["ConsignmentNumber"].ToString();
                cnd.ItemNumber = dr["DimensionNumber"].ToString();
                cnd.Length = dr["Breadth"].ToString();
                cnd.Width = dr["Width"].ToString();
                cnd.Height = dr["Height"].ToString();
                cnd.VolWeight = dr["VolWeight"].ToString();
                cnd.Pieces = dr["Pieces"].ToString();
                cnd.denseWeight = dr["DenseWeight"].ToString();
                Dimensions.Add(cnd);
            }
            //foreach (ConsignmentClass row in resp)
            //{
            //    DataRow[] dimRow = dimDt.Select("ConsignmentNumber = '" + row.CN.ToString() + "'");
            //    if (dimRow != null)
            //    {
            //        foreach (DataRow dr in dimRow)
            //        {
            //            CnDimensions cnd = new CnDimensions();
            //            cnd.ConsignmentNumber = dr["ConsignmentNumber"].ToString();
            //            cnd.ItemNumber = dr["DimensionNumber"].ToString();
            //            cnd.Length = dr["Breadth"].ToString();
            //            cnd.Width = dr["Width"].ToString();
            //            cnd.Height = dr["Height"].ToString();
            //            cnd.VolWeight = dr["VolWeight"].ToString();
            //            cnd.Pieces = dr["Pieces"].ToString();
            //            cnd.denseWeight = dr["DenseWeight"].ToString();
            //            Dimensions.Add(cnd);
            //        }
            //    }

            //}

            resp_.Consignments = resp.ToArray();
            resp_.Dimensions = Dimensions.ToArray();
            //return resp.ToArray();
            return resp_;
        }

        public static DataSet GetConsignmentAvailability(DataTable dt, float weight, string service)
        {
            //Cl_Variables clvar = new Cl_Variables();
            DataSet ds = new DataSet();

            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "MnP_CheckConsignmentNumbers_Approval_For_NewSequences";
                cmd.Parameters.AddWithValue("@CNS", dt);
                cmd.Parameters.AddWithValue("@ServiceType", service);
                cmd.Parameters.AddWithValue("@ZoneCode", HttpContext.Current.Session["ZoneCode"].ToString());
                cmd.Parameters.AddWithValue("@BranchCode", HttpContext.Current.Session["BranchCode"].ToString());
                cmd.Parameters.AddWithValue("@ExpressCenterCode", HttpContext.Current.Session["ExpressCenter"].ToString());
                cmd.Parameters.AddWithValue("@Weight", weight);
                SqlDataAdapter sda = new SqlDataAdapter();
                sda.SelectCommand = cmd;
                sda.Fill(ds);

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return ds;
        }

        [WebMethod]
        public static string ValidateRider(string riderCode)
        {
            DataTable riderDetails = GetRiderCode(riderCode);
            if (riderDetails != null)
            {
                if (riderDetails.Rows.Count > 0)
                {
                    return "OK";
                }
                else
                {
                    return "Invalid Rider Code";
                }
            }
            else
            {
                return "Invalid Rider Code";
            }
            //return "";
        }

        public static DataTable GetRiderCode(string riderCode)
        {

            DataTable dt = new DataTable();
            string sqlString = "selecT * from riders where riderCode = '" + riderCode + "' and branchID = '" + HttpContext.Current.Session["BranchCode"].ToString() + "' and status = '1'";

            sqlString = "SELECT ec.name ECNAME, ec.expressCenterCode ExpressCenter, r.*\n" +
            "  FROM Riders r\n" +
            " INNER JOIN ExpressCenters ec\n" +
            "    ON ec.bid = r.branchId\n" +
            "   AND ec.expressCenterCode = r.expressCenterId\n" +
            "   And ec.status = '1'\n" +
            " WHERE r.riderCode = '" + riderCode + "'\n" +
            "   AND r.branchId = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'\n" +
            "   AND r.[status] = '1'";

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


        [WebMethod]
        public static string[] ValidateAccount(string AccountNumber)
        {
            string[] resp = { "", "", "", "", "", "" };
            DataTable AccountDetails = GetAccount(AccountNumber);
            if (AccountDetails != null)
            {
                if (AccountDetails.Rows.Count > 0)
                {
                    resp[0] = "1";
                    //if (AccountDetails.Rows[0]["CreditClientID"].ToString() != "")
                    //{
                    //    resp[0] = "0";
                    //    resp[1] = "This COD Account is not Allowed in MRaabta";
                    //    return resp;
                    //}
                    if (AccountDetails.Rows[0]["IsCOD"].ToString() == "1" || AccountDetails.Rows[0]["IsCOD"].ToString().ToUpper() == "TRUE")
                    {
                        //if (AccountDetails.Rows[0]["CODType"].ToString() != "2" || AccountDetails.Rows[0]["CreditClientID"].ToString() != "")
                        //{
                        //    resp[0] = "0";
                        //    resp[1] = "This COD Account is not Allowed in MRaabta";
                        //    return resp;
                        //}
                    }

                    if (AccountNumber != "0")
                    {
                        resp[1] = AccountDetails.Rows[0]["PhoneNo"].ToString();
                        resp[2] = AccountDetails.Rows[0]["Name"].ToString();
                        resp[3] = AccountDetails.Rows[0]["Address"].ToString();
                        //if (AccountDetails.Rows[0]["IsCOD"].ToString() == "1" || AccountDetails.Rows[0]["IsCOD"].ToString().ToUpper() == "TRUE")
                        //{
                        //    resp[4] = "2";
                        //}
                        //else
                        //{
                        //    resp[4] = "0";
                        //}
                        resp[4] = AccountDetails.Rows[0]["isCod"].ToString() + "-" + AccountDetails.Rows[0]["CODType"].ToString();
                        resp[5] = AccountDetails.Rows[0]["id"].ToString();
                    }
                    else
                    {
                        resp[1] = "";
                        resp[2] = "";
                        resp[3] = "";
                        resp[5] = AccountDetails.Rows[0]["id"].ToString();
                    }

                    return resp;
                }
                else
                {
                    resp[0] = "0";
                    resp[1] = "Invalid Account Number";
                    return resp;
                }
            }
            else
            {
                resp[0] = "0";
                resp[1] = "Invalid Account Number";
                return resp;
            }
            //return "";
        }

        public static DataTable GetAccount(string AccountNumber)
        {

            DataTable dt = new DataTable();
            string sqlString = "selecT * from creditClients cc where cc.accountNo = '" + AccountNumber + "' and cc.branchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "' and cc.isactive = '1' and cc.isCod = '0'";

            sqlString = "selecT *\n" +
            "  from creditClients cc\n" +
            "  LEFT OUTER JOIN codusers cc2\n" +
            "    ON cc2.CreditClientID = cc.id and cc2.iscod = '1'\n" +
            " where cc.accountNo = '" + AccountNumber + "'\n" +
            "   and cc.branchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'\n" +
            "   and cc.isactive = '1'\n" +
            "   --AND cc2.CreditClientID IS NULL";

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


        public class ServiceTypes
        {
            public string ServiceTypeName { get; set; }
            public string Product { get; set; }
        }
        public class ServiceTypesReturn
        {
            public string Status { get; set; }
            public string Message { get; set; }
            public ServiceTypes[] Services { get; set; }

        }
        [WebMethod]
        public static ServiceTypesReturn GetServices(string accountNumber)
        {
            ServiceTypesReturn resp = new ServiceTypesReturn();
            List<ServiceTypes> services = new List<ServiceTypes>();


            DataTable dt = GetClientServiceMap(accountNumber);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        ServiceTypes service = new ServiceTypes();
                        service.ServiceTypeName = dr["ServiceTypeName"].ToString();
                        services.Add(service);
                    }
                    resp.Status = "1";
                    resp.Services = services.ToArray();
                }
                else
                {
                    resp.Status = "0";
                    resp.Message = "No Services Allocated To This Client.";
                }
            }
            else
            {
                resp.Status = "0";
                resp.Message = "Could not Find Services.";
            }

            return resp;
        }
        public static DataTable GetClientServiceMap(string AccountNumber)
        {
            string sqlString = "SELECT MSC.SERVICETYPENAME\n" +
            "  FROM MNP_CUSTOMER_SERVICEMAP MSC\n" +
            " INNER JOIN CREDITCLIENTS CC\n" +
            "    ON CC.ID = MSC.CREDITCLIENTID\n" +
            " WHERE CC.ACCOUNTNO = '" + AccountNumber + "'\n" +
            "   AND CC.BRANCHCODE = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'\n" +
            "   AND CC.ISACTIVE = '1'\n" +
            "   AND MSC.[STATUS] = '1'";


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

        public class PriceModifier
        {
            public string ID { get; set; }
            public string Name { get; set; }
            public string Base { get; set; }
            public string Value { get; set; }
        }
        public class PriceModifiersReturn
        {
            public string Status { get; set; }
            public string Message { get; set; }
            public PriceModifier[] PriceModifiers { get; set; }
        }
        [WebMethod]
        public static PriceModifiersReturn GetModifiers(string serviceTypeName)
        {
            PriceModifiersReturn resp = new PriceModifiersReturn();
            List<PriceModifier> modifiers = new List<PriceModifier>();


            try
            {

                DataTable dt = PriceModifiersByService(serviceTypeName);
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            PriceModifier modifier = new PriceModifier();
                            modifier.ID = dr["ID"].ToString();
                            modifier.Name = dr["PriceModifier"].ToString();
                            modifier.Base = dr["calculationBase"].ToString();
                            modifier.Value = dr["calculationValue"].ToString();
                            modifiers.Add(modifier);
                        }
                        resp.PriceModifiers = modifiers.ToArray();
                        resp.Status = "1";
                    }
                    else
                    {
                        resp.Status = "0";
                    }
                }
                else
                {
                    resp.Status = "-1";
                    resp.Message = "Could Not Find Price Modifiers For Selected Service";
                }



            }
            catch (Exception ex)
            {
                resp.Status = "-1";
                resp.Message = ex.Message;
            }
            finally
            {
            }
            return resp;
        }
        [WebMethod]
        public static string[] GetEC(string accountNumber, string riderCode, string CnFrom)
        {
            string[] resp = { "", "", "" };
            if (accountNumber.Trim() == "0")
            {
                DataTable ecBySeq = GetECBySequence(CnFrom);
                if (ecBySeq != null)
                {
                    if (ecBySeq.Rows.Count == 1)
                    {
                        resp[0] = "1";
                        resp[1] = ecBySeq.Rows[0]["ExpressCenter"].ToString();
                        resp[2] = ecBySeq.Rows[0]["ECNAME"].ToString();
                        return resp;
                    }
                    else if (ecBySeq.Rows.Count > 1)
                    {
                        resp[0] = "0";
                        resp[1] = "Sequence defined for multiple Express Centers";
                        return resp;
                    }
                    else
                    {
                        resp[0] = "0";
                        resp[1] = "Express Center Not Found for Entered Consignment";
                        return resp;
                    }
                }
                else
                {
                    resp[0] = "0";
                    resp[1] = "Express Center Not Found for Entered Consignment";
                    return resp;
                }
            }
            else
            {
                DataTable ecByRider = GetRiderCode(riderCode);
                if (ecByRider != null)
                {
                    if (ecByRider.Rows.Count > 0)
                    {
                        resp[0] = "1";
                        resp[1] = ecByRider.Rows[0]["ExpressCenter"].ToString();
                        resp[2] = ecByRider.Rows[0]["ECNAME"].ToString();
                        return resp;
                    }
                    else
                    {
                        resp[0] = "-1";
                        resp[1] = "No Express Center Found for Your Rider";
                        return resp;
                    }
                }
                else
                {
                    resp[0] = "-1";
                    resp[1] = "No Express Center Found for Your Rider";
                    return resp;
                }
            }

        }

        public static DataTable GetECBySequence(string consignmentNumber)
        {

            string sqlString = "SELECT *\n" +
            "  FROM Mnp_RiderCNSequence mrc\n" +
            " WHERE '" + consignmentNumber + "' BETWEEN mrc.SequenceStart AND mrc.SequenceEnd\n" +
            "   AND mrc.Branch = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'";

            sqlString = "SELECT mrc.*, ec.name ECName\n" +
            "  FROM Mnp_RiderCNSequence mrc\n" +
            " INNER JOIN ExpressCenters ec\n" +
            "    ON ec.expressCenterCode = mrc.ExpressCenter\n" +
            "   AND ec.bid = mrc.Branch\n" +
            " WHERE '" + consignmentNumber + "' BETWEEN mrc.SequenceStart AND mrc.SequenceEnd";

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



        [WebMethod]
        public static ConsignmentClass[] SaveToDb(ConsignmentClass[] Consignments, CnDimensions[] cnDimensions)
        {
            string InsertType = "";
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[] {
        new DataColumn("ConsignmentNumber", typeof(string)),
        new DataColumn("AccountNumber", typeof(string)),
        new DataColumn("CreditClientID", typeof(string)),
        new DataColumn("Origin", typeof(string)),
        new DataColumn("Destination", typeof(string)),
        new DataColumn("ServiceTypeName", typeof(string)),
        new DataColumn("length", typeof(string)),
        new DataColumn("width", typeof(string)),
        new DataColumn("height", typeof(string)),
        new DataColumn("DenseWeight", typeof(string)),
        new DataColumn("Weight", typeof(string)),
        new DataColumn("Pieces", typeof(string)),
        new DataColumn("ExpressCenterCode", typeof(string)),
        new DataColumn("OriginExpressCenterCode", typeof(string)),
        new DataColumn("DestinationExpressCenterCode", typeof(string)),
        new DataColumn("cnDiscountAmount", typeof(string)),
        new DataColumn("cnGrossAmount", typeof(string)),
        new DataColumn("cnGst", typeof(string)),
        new DataColumn("cnNetAmount", typeof(string)),
        new DataColumn("shipperAddress", typeof(string)),
        new DataColumn("PakageContents", typeof(string)),
        new DataColumn("cnTotalAmount", typeof(string))
        });
            DataTable unapprove = new DataTable();
            unapprove.Columns.AddRange(new DataColumn[]{
            new DataColumn("ConsignmentNumber", typeof(string))
        });

            clvar.Branch = HttpContext.Current.Session["BranchCode"].ToString();
            DataTable allowedDate = MinimumDate(clvar);

            DataTable cns = new DataTable();
            cns.Columns.Add("ConsignmentNumber");
            for (int i = 0; i < Consignments.Count(); i++)
            {
                cns.Rows.Add(Consignments[i].CN);
                InsertType = Consignments[i].InsertType;
            }
            DataSet availability = GetConsignmentAvailability(cns, 0, "");
            bool errorFound = false;
            #region Availability Searching
            if (availability != null)
            {
                #region Checking if Availability found
                if (availability.Tables[0] == null || availability.Tables[2] == null)
                {
                    foreach (ConsignmentClass item in Consignments)
                    {
                        item.ServerResponse = "Error Connecting to DataBase";
                    }
                    return Consignments;
                }

                if (availability.Tables[0].Rows.Count == 0 || availability.Tables[2].Rows.Count == 0)
                {
                    foreach (ConsignmentClass item in Consignments)
                    {
                        item.ServerResponse = "Error Connecting to DataBase";
                    }
                    return Consignments;
                }
                #endregion
                availability.Tables[2].CaseSensitive = false;
                foreach (ConsignmentClass item in Consignments)
                {
                    DataRow dr = availability.Tables[0].Select("ConsignmentNumber = '" + item.CN + "'").FirstOrDefault();
                    if (dr != null)
                    {

                        #region If Consignment Number allowed in this Zone
                        if (dr["ZoneAllowed"].ToString() != "Y" && (dr["CODRESERVED"].ToString() != "Y" || (dr["CODRESERVED"].ToString() == "Y" && item.isCOD != "1")))
                        {
                            item.ServerResponse = "Consignment Not Allowed in your Zone";
                            errorFound = true;
                            continue;
                        }
                        #endregion

                        #region If Consignment is New
                        if (dr["Available"].ToString() == "Y")
                        {
                            //If Consignment is New Get the Product of The service Type
                            DataRow serviceRow = availability.Tables[2].Select("ServiceTypeName = '" + item.ServiceType.ToUpper() + "'").FirstOrDefault();

                            if (serviceRow != null)
                            {
                                // IF Product of the selected Service type is not equal to Product of the Consignment Sequence consignment is not valid.
                                if (dr["Products"].ToString().ToUpper() != serviceRow["Products"].ToString().ToUpper())
                                {
                                    errorFound = true;
                                    item.ServerResponse = "Invalid Service Type. Consignment Number Reserved for " + dr["Products"].ToString() + " Product(s) only.";
                                }
                            }
                            else
                            {
                                errorFound = true;
                                item.ServerResponse = "Invalid Service Type";
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        item.ServerResponse = "Error in Consignment";
                        errorFound = true;
                    }
                }

                // IF error found any where in the Availability, Return.
                if (errorFound)
                {
                    return Consignments;
                }
            }
            else
            {
                foreach (ConsignmentClass item in Consignments)
                {
                    item.ServerResponse = "Error Connecting to DataBase";
                }
                return Consignments;
            }
            #endregion

            // ========================= Operation Control =========================

            


            DataTable destEc = GetDestinationECS();
            for (int i = 0; i < Consignments.Count(); i++)
            {
                float length = 0;
                float width = 0;
                float height = 0;

                string[] dimesions = Consignments[i].Dimensions.ToUpper().Split('X');


                //DateTime bookingDate = DateTime.Parse(Consignments[i].BKDate);	
                //DateTime reportingDate = DateTime.Parse(Consignments[i].RPDate);	
                DateTime bookingDate = DateTime.ParseExact(Consignments[i].BKDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);	
                DateTime reportingDate = DateTime.ParseExact(Consignments[i].RPDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                DateTime minAllowedDate = DateTime.Parse(allowedDate.Rows[0]["DateAllowed"].ToString()).AddDays(1);
                DateTime maxAllowedDate = DateTime.Now;
                string[] operationProfiles = { "2", "5", "9", "12", "38", "113" };

                if (bookingDate < minAllowedDate || bookingDate > maxAllowedDate || reportingDate < minAllowedDate || reportingDate > maxAllowedDate)
                {
                    Consignments[i].ServerResponse = "Day End has already been performed for this Consignment";
                    continue;
                }
                else if (operationProfiles.Contains(HttpContext.Current.Session["Profile"].ToString()))
                {
                    if (Consignments[i].Approved.ToUpper() == "APPROVED" || Consignments[i].Approved == "1")
                    {
                        DateTime bd = bookingDate.Date;
                        DateTime ad = reportingDate.Date;
                        DateTime dateToCheck = DateTime.Now.AddDays(-1).Date;

                        if ((bd <= DateTime.Now.Date || ad <= DateTime.Now.Date) && !(bd < dateToCheck || ad < dateToCheck))
                        {
                            string dayEndTime = GetDayEndTime();
                            if (dayEndTime == "")
                            {
                                Consignments[i].ServerResponse = "Cannot Unapprove. Day End Time not defined. Please Contact I.T. Support.";
                                continue;
                            }
                            else
                            {
                                DateTime t1 = DateTime.Now;
                                DateTime t2 = Convert.ToDateTime(dayEndTime);
                                int iq = DateTime.Compare(t2, t1);
                                if (iq <= 0 && (ad.Date != DateTime.Today.Date || bd.Date != DateTime.Today.Date))
                                {
                                    Consignments[iq].ServerResponse = "Consignment Cannot be unapproved. Consignment\\'s Date is Closed.";
                                    continue;
                                }
                            }
                        }
                        else
                        {
                            Consignments[i].ServerResponse = "Consignment Cannot be unapproved. Consignment\\'s Date is Closed.";
                            continue;
                        }
                    }
                }

                if (Consignments[i].Approved.ToUpper() == "APPROVED" || Consignments[i].Approved == "1")
                {

                    if (dimesions.Count() == 3)
                    {
                        float.TryParse(dimesions[0], out length);
                        float.TryParse(dimesions[1], out width);
                        float.TryParse(dimesions[2], out height);
                    }

                    DataRow dr = dt.NewRow();
                    dr["ConsignmentNumber"] = Consignments[i].CN;
                    dr["AccountNumber"] = Consignments[i].Acc;
                    DataTable ccid = GetCreditClientID(Consignments[i].Acc, HttpContext.Current.Session["BranchCode"].ToString());
                    string creditClientID = "";
                    if (ccid != null)
                    {
                        if (ccid.Rows.Count > 0)
                        {
                            creditClientID = ccid.Rows[0]["ID"].ToString();
                            Consignments[i].CreditClientID = creditClientID;
                        }
                        else
                        {
                            errorFound = true;
                            Consignments[i].ServerResponse = "Invalid Account Number";
                        }
                    }

                    dr["CreditClientID"] = creditClientID;
                    dr["Origin"] = HttpContext.Current.Session["BranchCode"].ToString();
                    dr["Destination"] = Consignments[i].Destination;
                    dr["ServiceTypeName"] = Consignments[i].ServiceType;
                    dr["length"] = length.ToString();
                    dr["width"] = width.ToString();
                    dr["height"] = height.ToString();
                    dr["DenseWeight"] = Consignments[i].DnsWgt;
                    dr["Weight"] = Consignments[i].Weight;
                    dr["Pieces"] = Consignments[i].Pieces;
                    dr["shipperAddress"] = Consignments[i].shipperAddress;
                    dr["PakageContents"] = Consignments[i].PakageContents;
                    dr["ExpressCenterCode"] = Consignments[i].OriginEC;
                    dr["OriginExpressCenterCode"] = Consignments[i].OriginEC;
                    dr["DestinationExpressCenterCode"] = destEc.Select("BranchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'").FirstOrDefault()["ExpressCenterCode"].ToString();
                    dr["cnDiscountAmount"] = "0";
                    dr["cnGrossAmount"] = "0";
                    dr["cnGst"] = "0";
                    dr["cnNetAmount"] = "0";
                    dr["cnTotalAmount"] = "0";

                    dt.Rows.Add(dr);

                }
                else if (Consignments[i].Approved.ToUpper() == "UNAPPROVED" || Consignments[i].Approved == "0")
                {
                    DataRow dr = unapprove.NewRow();
                    dr["ConsignmentNumber"] = Consignments[i].CN;
                    unapprove.Rows.Add(dr);
                }


            }

            if (errorFound)
            {
                return Consignments;
            }

            dt.CaseSensitive = false;
            DataRow[] temp = dt.Select("serviceTypeName = 'Road N Rail'");

            DataTable rnr = new DataTable();
            DataTable domestic = new DataTable();
            DataTable calculatedRnr = new DataTable();
            DataTable calculatedDomestic = new DataTable();
            if (temp != null)
            {
                if (temp.Count() > 0)
                {
                    rnr = temp.CopyToDataTable();
                }
            }
            temp = dt.Select("serviceTypeName <> 'Road N Rail'");

            if (temp != null)
            {
                if (temp.Count() > 0)
                {
                    domestic = temp.CopyToDataTable();
                }
            }

            if (rnr != null)
            {
                if (rnr.Rows.Count > 0)
                {
                    calculatedRnr = GetConsignmentAmounts(rnr, "", "Road N Rail");
                }
            }
            if (domestic != null)
            {
                if (domestic.Rows.Count > 0)
                {
                    calculatedDomestic = GetConsignmentAmounts(domestic, "", "");
                }
            }

            DataTable finaldt = new DataTable();
            finaldt.Columns.AddRange(new DataColumn[] {
        new DataColumn("ConsignmentNumber", typeof(string)),
        new DataColumn("BookingDate", typeof(string)),
        new DataColumn("ReportingDate", typeof(string)),
        new DataColumn("AccountNumber", typeof(string)),
        new DataColumn("CreditClientID", typeof(string)),
        new DataColumn("Origin", typeof(string)),
        new DataColumn("Coupon", typeof(string)),
        new DataColumn("remarks", typeof(string)),
        new DataColumn("Consignee", typeof(string)),
        new DataColumn("ConsigneePhone", typeof(string)),
        new DataColumn("ConsigneeCNIC", typeof(string)),
        new DataColumn("ConsigneeAddress", typeof(string)),
        new DataColumn("Consigner", typeof(string)),
        new DataColumn("ConsignerPhone", typeof(string)),
        new DataColumn("ConsignerCNIC", typeof(string)),
        new DataColumn("ConsignerAddress", typeof(string)),
        new DataColumn("Destination", typeof(string)),
        new DataColumn("ServiceTypeName", typeof(string)),
        new DataColumn("RiderCode", typeof(string)),
        new DataColumn("length", typeof(string)),
        new DataColumn("width", typeof(string)),
        new DataColumn("height", typeof(string)),
        new DataColumn("DenseWeight", typeof(string)),
        new DataColumn("Weight", typeof(string)),
        new DataColumn("Pieces", typeof(string)),
        new DataColumn("ExpressCenterCode", typeof(string)),
        new DataColumn("OriginExpressCenterCode", typeof(string)),
        new DataColumn("DestinationExpressCenterCode", typeof(string)),
        new DataColumn("cnDiscountAmount", typeof(string)),
        new DataColumn("cnGrossAmount", typeof(string)),
        new DataColumn("cnGst", typeof(string)),
        new DataColumn("ActualGST", typeof(string)),
        new DataColumn("DiscountGST", typeof(string)),
        new DataColumn("cnNetAmount", typeof(string)),
        new DataColumn("cnTotalAmount", typeof(string)),
        new DataColumn("ChargeAmount",typeof(string)),
        new DataColumn("Region", typeof(string)),
        new DataColumn("ZoningCriteria", typeof(string)),
        new DataColumn("Zoning", typeof(string)),
        new DataColumn("Zoning_Criteria_Origin", typeof(string)),
        new DataColumn("Zoning_Origin", typeof(string)),
        new DataColumn("DestinationZone", typeof(string)),
        new DataColumn("isApproved", typeof(string)),
        new DataColumn("isPriceComputed", typeof(string)),
        new DataColumn("isInsured", typeof(string)),
        new DataColumn("VolWeight", typeof(string)),
        new DataColumn("shipperAddress", typeof(string)),
        new DataColumn("PakageContents", typeof(string))
    //    new DataColumn("InsertType", typeof(string))
        });

            DataTable modifiers = new DataTable();
            modifiers.Columns.AddRange(new DataColumn[]{
            new DataColumn("PriceModifierID", typeof(Int64)) ,
            new DataColumn("ConsignmentNumber", typeof(string)) ,
            new DataColumn("ModifiedCalculationValue", typeof(int)) ,
            new DataColumn("CalculatedValue", typeof(float)) ,
            new DataColumn("CalculatedGST", typeof(float)) ,
            new DataColumn("CalculationBase", typeof(int)) ,
            new DataColumn("isTaxable", typeof(int)) ,
            new DataColumn("SortOrder", typeof(int)) ,
            new DataColumn("AlternateValue", typeof(Int64))
        });

            DataTable codDetails = new DataTable();
            codDetails.Columns.AddRange(new DataColumn[]{
            new DataColumn("consignmentNumber", typeof(string)),
            new DataColumn("orderRefNo", typeof(string)),
            new DataColumn("customerName", typeof(string)),
            new DataColumn("productTypeId", typeof(string)),
            new DataColumn("productDescription", typeof(string)),
            new DataColumn("chargeCODAmount", typeof(string)),
            new DataColumn("codAmount", typeof(string)),
            new DataColumn("calculatedAmount", typeof(string))
        });

            foreach (ConsignmentClass item in Consignments)
            {
                float length = 0;
                float width = 0;
                float height = 0;

                DateTime bookingDate = DateTime.ParseExact(item.BKDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);	
                DateTime reportingDate = DateTime.ParseExact(item.RPDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);	
                //DateTime.TryParse(item.BKDate, out bookingDate);	
                //DateTime.TryParse(item.RPDate, out reportingDate);
                if (bookingDate.Date > DateTime.Today.Date)
                {
                    errorFound = true;
                    item.ServerResponse = "Invalid Booking Date";
                    continue;

                }
                if (reportingDate.Date > DateTime.Today.Date)
                {
                    errorFound = true;
                    item.ServerResponse = "Invalid Reporting Date";
                    continue;
                }

                string[] dimesions = item.Dimensions.ToUpper().Split('X');
                if (dimesions.Count() == 3)
                {
                    float.TryParse(dimesions[0], out length);
                    float.TryParse(dimesions[1], out width);
                    float.TryParse(dimesions[2], out height);
                }
                if (rnr != null)
                {
                    if (rnr.Rows.Count > 0)
                    {
                        DataRow row = calculatedRnr.Select("ConsignmentNumber = '" + item.CN + "'").FirstOrDefault();
                        if (row != null)
                        {
                            DataRow dr = finaldt.NewRow();
                            dr["ConsignmentNumber"] = row["ConsignmentNumber"].ToString();
                            dr["BookingDate"] = bookingDate.ToString("yyyy-MM-dd");
                            dr["ReportingDate"] = reportingDate.ToString("yyyy-MM-dd");
                            dr["AccountNumber"] = item.Acc;
                            dr["CreditClientID"] = item.CreditClientID;
                            dr["serviceTypeName"] = item.ServiceType;
                            dr["RiderCode"] = item.Rider;
                            dr["Consignee"] = item.Consignee;
                            dr["ConsigneeAddress"] = item.Address;
                            dr["ConsigneePhone"] = item.ConsigneeMob;
                            dr["ConsigneeCNIC"] = "";
                            dr["Destination"] = item.Destination;
                            dr["Consigner"] = item.Consigner.Replace("&amp;", "&");
                            dr["ConsignerAddress"] = "";
                            dr["ConsignerPhone"] = item.ConsignerMob;
                            dr["ConsignerCNIC"] = "";
                            dr["cnDiscountAmount"] = "0";
                            dr["ActualGST"] = row["GST"].ToString();
                            dr["DiscountGST"] = "0";
                            dr["cnGrossAmount"] = "0";
                            dr["cnGst"] = "0";
                            dr["cnNetAmount"] = "0";
                            dr["cnTotalAmount"] = row["TotalAmount"].ToString();
                            dr["ChargeAmount"] = item.ChrgAmt;
                            dr["Region"] = row["Region"].ToString();
                            dr["ZoningCriteria"] = row["ZoningCriteria"].ToString();
                            dr["Zoning"] = row["Zoning"].ToString();
                            dr["Zoning_Criteria_Origin"] = row["Zoning_Criteria_Origin"].ToString();
                            dr["Zoning_Origin"] = row["Zoning_Origin"].ToString();
                            dr["DestinationZone"] = row["DestinationZone"].ToString();
                            dr["Remarks"] = item.SpecialInstructions;
                            dr["ExpressCenterCode"] = item.OriginEC;
                            dr["OriginExpressCenterCode"] = item.OriginEC;
                            dr["DestinationExpressCenterCode"] = destEc.Select("BranchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'").FirstOrDefault()["ExpressCenterCode"].ToString();
                            dr["Length"] = length.ToString();
                            dr["width"] = width.ToString();
                            dr["height"] = height.ToString();
                            dr["denseWeight"] = item.DnsWgt;
                            dr["weight"] = item.Weight;
                            dr["pieces"] = item.Pieces;
                            dr["Coupon"] = item.Coupon;
                            dr["isApproved"] = row["isApproved"].ToString();
                            dr["isPriceComputed"] = row["isPriceComputed"].ToString();
                            dr["shipperAddress"] = row["shipperAddress"].ToString();
                            dr["PakageContents"] = row["PakageContents"].ToString();
                            dr["IsInsured"] = "0";
                            dr["VolWeight"] = item.VolWgt;
                            // dr["InsertType"] = item.InsertType;
                            finaldt.Rows.Add(dr);
                            DataRow dr_ = codDetails.NewRow();
                            dr_["ConsignmentNumber"] = item.CN;
                            dr_["orderRefNo"] = item.CODRef;
                            dr_["customerName"] = item.Consigner;
                            dr_["productTypeId"] = "0";
                            dr_["productDescription"] = item.CODDesc;
                            dr_["chargeCODAmount"] = "0";
                            dr_["codAmount"] = "0";
                            dr_["calculatedAmount"] = "0";
                            codDetails.Rows.Add(dr_);
                        }

                    }
                }

                if (domestic != null)
                {
                    if (domestic.Rows.Count > 0)
                    {


                        DataRow row = calculatedDomestic.Select("ConsignmentNumber = '" + item.CN + "'").FirstOrDefault();
                        if (row != null)
                        {
                            DataRow dr = finaldt.NewRow();
                            dr["ConsignmentNumber"] = row["ConsignmentNumber"].ToString();
                            dr["BookingDate"] = bookingDate.ToString("yyyy-MM-dd");
                            dr["ReportingDate"] = reportingDate.ToString("yyyy-MM-dd");
                            dr["AccountNumber"] = item.Acc;
                            dr["CreditClientID"] = item.CreditClientID;
                            dr["serviceTypeName"] = item.ServiceType;
                            dr["RiderCode"] = item.Rider;
                            dr["Consignee"] = item.Consignee;
                            dr["ConsigneeAddress"] = item.Address;
                            dr["ConsigneePhone"] = item.ConsigneeMob;
                            dr["ConsigneeCNIC"] = "";
                            dr["Destination"] = item.Destination;
                            dr["Consigner"] = item.Consigner.Replace("&amp;", "&"); ;
                            dr["ConsignerAddress"] = "";
                            dr["ConsignerPhone"] = item.ConsignerMob;
                            dr["ConsignerCNIC"] = "";
                            dr["cnDiscountAmount"] = "0";
                            dr["ActualGST"] = row["GST"].ToString();
                            dr["DiscountGST"] = "0";
                            dr["cnGrossAmount"] = "0";
                            dr["cnGst"] = "0";
                            dr["cnNetAmount"] = "0";
                            dr["cnTotalAmount"] = row["Amount"].ToString();
                            dr["ChargeAmount"] = item.ChrgAmt;
                            dr["Region"] = "";
                            dr["ZoningCriteria"] = "";
                            dr["Zoning"] = "";
                            dr["Zoning_Criteria_Origin"] = "";
                            dr["Zoning_Origin"] = "";
                            dr["DestinationZone"] = "";
                            dr["Remarks"] = item.SpecialInstructions;
                            dr["ExpressCenterCode"] = item.OriginEC;
                            dr["OriginExpressCenterCode"] = item.OriginEC;
                            dr["DestinationExpressCenterCode"] = destEc.Select("BranchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'").FirstOrDefault()["ExpressCenterCode"].ToString();
                            dr["Length"] = length.ToString();
                            dr["width"] = width.ToString();
                            dr["height"] = height.ToString();
                            dr["denseWeight"] = item.DnsWgt;
                            dr["weight"] = item.Weight;
                            dr["pieces"] = item.Pieces;
                            dr["Coupon"] = item.Coupon;
                            dr["shipperAddress"] = item.shipperAddress;
                            dr["PakageContents"] = item.PakageContents;
                            dr["isApproved"] = "1";
                            dr["isPriceComputed"] = "1";
                            dr["IsInsured"] = "0";
                            dr["VolWeight"] = item.VolWgt;
                            //  dr["InsertType"] = item.InsertType;
                            finaldt.Rows.Add(dr);
                            DataRow dr_ = codDetails.NewRow();
                            dr_["ConsignmentNumber"] = item.CN;
                            dr_["orderRefNo"] = item.CODRef;
                            dr_["customerName"] = item.Consigner;
                            dr_["productTypeId"] = "0";
                            dr_["productDescription"] = item.CODDesc;
                            dr_["chargeCODAmount"] = "0";
                            dr_["codAmount"] = "0";
                            dr_["calculatedAmount"] = "0";
                            codDetails.Rows.Add(dr_);
                        }
                    }
                }


            }

            DataTable masterModifiers = PriceModifiers();
            clvar.origin = HttpContext.Current.Session["BranchCode"].ToString();
            DataSet tblBranchGst = con.BranchGSTInformation(clvar);
            foreach (ConsignmentClass item in Consignments)
            {
                DataRow dr = finaldt.Select("ConsignmentNumber = '" + item.CN + "'").FirstOrDefault();
                if (dr != null)
                {
                    if (item.pmID != "" && item.pmID != "0")
                    {
                        #region Modifier Validations
                        if (masterModifiers == null)
                        {
                            item.ServerResponse = "Modifiers Not Found for Computation";
                            continue;
                        }
                        if (masterModifiers == null)
                        {
                            item.ServerResponse = "Modifiers Not Found for Computation";
                            continue;
                        }
                        if (masterModifiers.Rows.Count == 0)
                        {
                            item.ServerResponse = "Modifiers Not Found for Computation";
                            continue;
                        }
                        if (tblBranchGst == null)
                        {
                            item.ServerResponse = "Branch GST not Found for Computation";
                            continue;
                        }
                        if (tblBranchGst.Tables[0] == null)
                        {
                            item.ServerResponse = "Branch GST not Found for Computation";
                            continue;
                        }
                        if (tblBranchGst.Tables[0].Rows.Count == 0)
                        {
                            item.ServerResponse = "Branch GST not Found for Computation";
                            continue;
                        }

                        DataRow pmRow = masterModifiers.Select("id = '" + item.pmID + "'").FirstOrDefault();
                        if (pmRow == null)
                        {
                            item.ServerResponse = "Invalid Price Modifier Selected";
                            continue;
                        }
                        #endregion

                        float branchGst = 0;
                        float.TryParse(tblBranchGst.Tables[0].Rows[0]["GST"].ToString(), out branchGst);

                        DataRow row = modifiers.NewRow();
                        Int64 pmid = 0;
                        int modifiedCalculationValue = 0;
                        float calculatedValue = 0;
                        float calculatedGST = 0;
                        int calculationBase = 0;
                        Int64 alternateValue = 0;
                        int isTaxable = 0;

                        float cnTotalAmount = 0;
                        float.TryParse(dr["cnTotalAmount"].ToString(), out cnTotalAmount);

                        Int64.TryParse(item.pmID, out pmid);
                        if (pmid <= 0)
                        {
                            item.ServerResponse = "Invalid Price Modifier Selected";
                            errorFound = true;
                        }

                        int.TryParse(item.modCalValue, out modifiedCalculationValue);
                        if (modifiedCalculationValue <= 0)
                        {
                            item.ServerResponse = "Invalid Modifier Value";
                            errorFound = true;
                        }

                        int.TryParse(item.calBase, out calculationBase);
                        if (calculationBase <= 0)
                        {
                            item.ServerResponse = "Invalid Calculation Base";
                            errorFound = true;
                        }

                        Int64.TryParse(item.AltValue, out alternateValue);

                        if (calculationBase == 3 && alternateValue <= 0)
                        {
                            item.ServerResponse = "Invalid Declared Value";
                            errorFound = true;
                        }

                        row["PriceModifierID"] = item.pmID;
                        row["ConsignmentNumber"] = item.CN;
                        row["ModifiedCalculationValue"] = modifiedCalculationValue;
                        row["CalculationBase"] = calculationBase;

                        if (calculationBase == 1)
                        {
                            calculatedValue = float.Parse(modifiedCalculationValue.ToString());
                            if (pmRow["isGST"].ToString().ToUpper() == "TRUE" || pmRow["isGST"].ToString().ToUpper() == "1")
                            {
                                calculatedGST = calculatedValue * (branchGst / 100);
                                isTaxable = 1;
                            }
                            else
                            {
                                calculatedGST = 0;
                                isTaxable = 0;
                            }
                        }
                        else if (calculationBase == 2)
                        {
                            calculatedValue = cnTotalAmount * (float.Parse(modifiedCalculationValue.ToString()) / 100);
                            if (pmRow["isGST"].ToString().ToUpper() == "TRUE" || pmRow["isGST"].ToString().ToUpper() == "1")
                            {
                                calculatedGST = calculatedValue * (branchGst / 100);
                                isTaxable = 1;
                            }
                            else
                            {
                                calculatedGST = 0;
                                isTaxable = 0;
                            }
                        }
                        else if (calculationBase == 3)
                        {
                            dr["IsInsured"] = "1";
                            calculatedValue = alternateValue * (float.Parse(modifiedCalculationValue.ToString()) / 100);
                            if (pmRow["isGST"].ToString().ToUpper() == "TRUE" || pmRow["isGST"].ToString().ToUpper() == "1")
                            {
                                calculatedGST = calculatedValue * (branchGst / 100);
                                isTaxable = 1;
                            }
                            else
                            {
                                calculatedGST = 0;
                                isTaxable = 0;
                            }
                        }
                        row["CalculatedValue"] = calculatedValue;
                        row["CalculatedGST"] = calculatedGST;

                        row["isTaxable"] = isTaxable;
                        row["SortOrder"] = "0";
                        row["AlternateValue"] = alternateValue;
                        modifiers.Rows.Add(row);
                        //         new DataColumn("PriceModifierID", typeof(Int64)) ,
                        //new DataColumn("ConsignmentNumber", typeof(string)) ,
                        //new DataColumn("ModifiedCalculationValue", typeof(int)) ,
                        //new DataColumn("CalculatedValue", typeof(float)) ,
                        //new DataColumn("CalculatedGST", typeof(float)) ,
                        //new DataColumn("CalculationBase", typeof(int)) ,
                        //new DataColumn("isTaxable", typeof(int)) ,
                        //new DataColumn("SortOrder", typeof(int)) ,
                        //new DataColumn("AlternateValue", typeof(Int64)) 
                    }
                }
            }

            DataTable dimensions = new DataTable();
            dimensions.Columns.AddRange(new DataColumn[] {
            new DataColumn("ConsignmentNumber", typeof(string)) ,
            new DataColumn("ItemNumber", typeof(string)) ,
            new DataColumn("Width", typeof(string)) ,
            new DataColumn("Breadth", typeof(string)) ,
            new DataColumn("Height", typeof(string)) ,
            new DataColumn("VolWeight", typeof(string)) ,
            new DataColumn("Pieces", typeof(string)),
            new DataColumn("WEIGHT", typeof(string))
        });
            foreach (CnDimensions dimension in cnDimensions)
            {
                dimensions.Rows.Add(dimension.ConsignmentNumber, dimension.ItemNumber, dimension.Width, dimension.Length, dimension.Height, dimension.VolWeight, dimension.Pieces, "");
            }



            if (errorFound)
            {
                return Consignments;
            }
            if (unapprove.Rows.Count > 0)
            {
                string profileType = GetProfileType(); // Getting Profile Type
                #region For Operations Users Checking Day End Time defined in MnP_DayEnd_Timings
                if (profileType.ToUpper() == "O")
                {
                    DataTable cnDates = GetConsignmentDates(unapprove); //Getting Consignments Booking Dates and Reporting Dates

                    if (cnDates != null)
                    {
                        if (cnDates.Rows.Count > 0)
                        {
                            foreach (ConsignmentClass cn in Consignments)
                            {
                                DataRow cnDateRow = cnDates.Select("ConsignmentNumber = '" + cn.CN + "'").FirstOrDefault();
                                if (cnDateRow != null)
                                {
                                    DateTime bd = DateTime.Parse(cnDateRow["BookingDate"].ToString()).Date;
                                    DateTime ad = DateTime.Parse(cnDateRow["ReportingDate"].ToString()).Date;
                                    DateTime dateToCheck = DateTime.Now.AddDays(-1).Date;

                                    if ((bd <= DateTime.Now.Date || ad <= DateTime.Now.Date) && !(bd < dateToCheck || ad < dateToCheck))
                                    {
                                        string dayEndTime = GetDayEndTime();
                                        if (dayEndTime == "")
                                        {
                                            cn.ServerResponse = "Day End Time Not Found. Please Contact I.T. Support";
                                            errorFound = true;
                                        }
                                        else
                                        {
                                            DateTime t1 = DateTime.Now;
                                            DateTime t2 = Convert.ToDateTime(dayEndTime);
                                            int i = DateTime.Compare(t2, t1);
                                            if (i <= 0 && (ad.Date != DateTime.Today.Date || bd.Date != DateTime.Today.Date))
                                            {
                                                cn.ServerResponse = "Consignment has been Closed.";
                                                errorFound = true;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        cn.ServerResponse = "Consignment has been Closed.";
                                        errorFound = true;
                                    }
                                }
                            }
                            if (errorFound)
                            {
                                return Consignments;
                            }
                        }
                        else
                        {
                            errorFound = true;
                            foreach (ConsignmentClass item in Consignments)
                            {
                                DataRow dr = unapprove.Select("ConsignmentNumber = '" + item.CN + "'").FirstOrDefault();
                                if (dr != null)
                                {
                                    item.ServerResponse = "Failed to Unapprove";

                                }
                            }
                        }
                    }
                    else
                    {
                        errorFound = true;
                        foreach (ConsignmentClass item in Consignments)
                        {
                            DataRow dr = unapprove.Select("ConsignmentNumber = '" + item.CN + "'").FirstOrDefault();
                            if (dr != null)
                            {
                                item.ServerResponse = "Failed to Unapprove";
                            }
                        }
                    }
                }
                #endregion
                if (errorFound)
                {
                    return Consignments;
                }
                string response = UnapproveConsignments(unapprove);
                if (response != "OK")
                {
                    foreach (ConsignmentClass item in Consignments)
                    {
                        DataRow dr = unapprove.Select("ConsignmentNumber = '" + item.CN + "'").FirstOrDefault();
                        if (dr != null)
                        {
                            item.ServerResponse = "Failed to Unapprove";
                        }
                    }
                }
                else
                {
                    foreach (ConsignmentClass item in Consignments)
                    {
                        DataRow dr = unapprove.Select("ConsignmentNumber = '" + item.CN + "'").FirstOrDefault();
                        if (dr != null)
                        {
                            item.ServerResponse = "OK";
                        }
                    }
                }
            }


            string resp = SaveConsignments(finaldt, clvar, 0, modifiers, codDetails, dimensions, InsertType);
            if (resp == "OK")
            {
                foreach (ConsignmentClass item in Consignments)
                {
                    DataRow dr = finaldt.Select("ConsignmentNumber = '" + item.CN + "'").FirstOrDefault();
                    if (dr != null)
                    {
                        item.ServerResponse = "OK";
                    }
                }
            }
            else
            {
                foreach (ConsignmentClass item in Consignments)
                {
                    DataRow dr = finaldt.Select("ConsignmentNumber = '" + item.CN + "'").FirstOrDefault();
                    if (dr != null)
                    {
                        item.ServerResponse = "Failed to Approve";
                    }
                }
            }
            return Consignments;
        }

        public static DataTable GetCreditClientID(string accountNumber, string branch)
        {
            string sqlString = "selecT * from creditclients cc where cc.accountNo = '" + accountNumber + "' and cc.branchCode = '" + branch + "' and cc.isactive = '1'";

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
            return dt;
        }

        public static DataTable GetConsignmentAmounts(DataTable dt, string AccountNumber, string service)
        {
            DataTable dt_ = new DataTable();
            //Cl_Variables clvar = new Cl_Variables();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                if (service.ToUpper() != "ROAD N RAIL")
                {
                    cmd.CommandText = "MnP_BulkConsignmentValidation";
                }
                else
                {
                    cmd.CommandText = "MnP_BulkConsignmentValidation_RNR";
                }

                cmd.CommandType = CommandType.StoredProcedure;

                DataTable bilal_dt = dt.Copy();

                bilal_dt.Columns.Remove("PakageContents");
                bilal_dt.Columns.Remove("shipperAddress");

                cmd.Parameters.AddWithValue("@CNS", bilal_dt);
                cmd.Parameters.AddWithValue("@UserID", HttpContext.Current.Session["U_ID"].ToString());
                cmd.Parameters.AddWithValue("@ZoneCode", HttpContext.Current.Session["ZoneCode"].ToString());
                cmd.Parameters.AddWithValue("@BranchCode", HttpContext.Current.Session["BranchCode"].ToString());
                cmd.Parameters.AddWithValue("@ExpressCenter", HttpContext.Current.Session["ExpressCenter"].ToString());
                cmd.Parameters.AddWithValue("@AccountNumber", AccountNumber);
                cmd.Parameters.Add("@Reason", SqlDbType.VarChar, 1000).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@insertCount", SqlDbType.Int).Direction = ParameterDirection.Output;
                SqlDataAdapter sda = new SqlDataAdapter(cmd);

                sda.Fill(dt_);

                string reason = cmd.Parameters["@Reason"].SqlValue.ToString();
                string insert = cmd.Parameters["@insertCount"].SqlValue.ToString();
            }
            catch (Exception ex)
            { }
            finally { }

            return dt_;
        }

        public static DataTable GetDestinationECS()
        {
            DataTable dt = new DataTable();

            string sqlString = "SELECT b.branchCode, ec.expressCenterCode\n" +
            "  FROM Branches b\n" +
            "  LEFT OUTER JOIN ExpressCenters ec\n" +
            "    ON ec.bid = b.branchCode\n" +
            "   AND ec.Main_EC = '1'\n" +
            "   AND ec.[status] = '1'\n" +
            " WHERE b.[status] = '1'";
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

        public static string SaveConsignments(DataTable dt, Cl_Variables clvar, float DiscountApplied, DataTable modifiers, DataTable codDetails, string InsertType)
        {
            string resp = "";
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.CommandText = "MnP_BulkConsignmentApproval";
                cmd.CommandText = "MnP_BulkConsignmentApproval_With_Dimensions";
                cmd.Parameters.AddWithValue("@CNS", dt);
                cmd.Parameters.AddWithValue("@modifiers", modifiers);
                cmd.Parameters.AddWithValue("@CodDetails", codDetails);
                cmd.Parameters.AddWithValue("@UserID", HttpContext.Current.Session["U_ID"].ToString());
                cmd.Parameters.AddWithValue("@ZoneCode", HttpContext.Current.Session["ZoneCode"].ToString());
                cmd.Parameters.AddWithValue("@BranchCode", HttpContext.Current.Session["BranchCode"].ToString());
                cmd.Parameters.AddWithValue("@ExpressCenter", HttpContext.Current.Session["ExpressCenter"].ToString());




                cmd.Parameters.AddWithValue("@DiscountApplied", DiscountApplied);
                //cmd.Parameters.AddWithValue("@InsertType", 4);

                if (dt.Rows[0]["ACCOUNTNUMBER"].ToString() == "0")
                {
                    cmd.Parameters.AddWithValue("@InsertType", 2);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@InsertType", 4);
                }

                cmd.Parameters.Add("@Reason", SqlDbType.VarChar, 1000).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@insertCount", SqlDbType.Int).Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();

                object obj = cmd.Parameters["@insertCount"].SqlValue.ToString();
                string reason = cmd.Parameters["@Reason"].SqlValue.ToString();
                resp = reason.ToString();
            }
            catch (Exception ex)
            { }
            finally { con.Close(); }


            return resp;
        }
        public static string SaveConsignments(DataTable dt, Cl_Variables clvar, float DiscountApplied, DataTable modifiers, DataTable codDetails, DataTable dimensions, string InsertType)
        {
            string resp = "";
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.CommandText = "MnP_BulkConsignmentApproval";
                cmd.CommandText = "MnP_BulkConsignmentApproval_With_Dimensions";
                cmd.Parameters.AddWithValue("@Dimensions", dimensions);
                cmd.Parameters.AddWithValue("@CNS", dt);
                cmd.Parameters.AddWithValue("@modifiers", modifiers);
                cmd.Parameters.AddWithValue("@CodDetails", codDetails);
                cmd.Parameters.AddWithValue("@UserID", HttpContext.Current.Session["U_ID"].ToString());
                cmd.Parameters.AddWithValue("@ZoneCode", HttpContext.Current.Session["ZoneCode"].ToString());
                cmd.Parameters.AddWithValue("@BranchCode", HttpContext.Current.Session["BranchCode"].ToString());
                cmd.Parameters.AddWithValue("@ExpressCenter", HttpContext.Current.Session["ExpressCenter"].ToString());




                cmd.Parameters.AddWithValue("@DiscountApplied", DiscountApplied);

                if (InsertType == "2")
                {
                    cmd.Parameters.AddWithValue("@InsertType", 2);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@InsertType", 4);
                }

                cmd.Parameters.Add("@Reason", SqlDbType.VarChar, 1000).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@insertCount", SqlDbType.Int).Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();

                object obj = cmd.Parameters["@insertCount"].SqlValue.ToString();
                string reason = cmd.Parameters["@Reason"].SqlValue.ToString();
                resp = reason.ToString();
            }
            catch (Exception ex)
            { }
            finally { con.Close(); }


            return resp;
        }

        public static string UnapproveConsignments(DataTable dt)
        {
            string resp = "";
            string consignmentNumbers = "";
            foreach (DataRow dr in dt.Rows)
            {
                consignmentNumbers += "'" + dr["ConsignmentNumber"].ToString() + "'";
            }
            consignmentNumbers = consignmentNumbers.Replace("''", "','");

            string query1 = "INSERT INTO Consignment_Archive SELECT * FROM Consignment AS c WHERE c.consignmentNumber in (" + consignmentNumbers + ")";
            string query2 = "update consignment Set isApproved = '0' where consignmentnumber in (" + consignmentNumbers + ")";
            string query3 = "insert into MNP_ConsignmentUnapproval (ConsignmentNumber, USERID, TransactionTime, STATUS) select c.consignmentNumber, '" + HttpContext.Current.Session["U_ID"].ToString() + "', GETDATE(), '0' from consignment c where c.consignmentNumber in (" + consignmentNumbers + ")\n ";
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(query1, con);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand(query2, con);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand(query3, con);
                cmd.ExecuteNonQuery();
                con.Close();
                resp = "OK";
            }
            catch (Exception ex)
            {
                //Consignemnts con = new Consignemnts();
                con.Close();
                //InsertErrorLog(clvar.consignmentNo, "", "", "", "", "", "UNAPPROVE CONSIGNMENT", ex.Message);
                return ex.Message;
            }
            finally { con.Close(); }
            return resp;
        }

        public static string GetProfileType()
        {
            Uri uri = new Uri(HttpContext.Current.Request.Url.AbsoluteUri);
            string pageGetURL = uri.Segments.Last();
            if (uri.IsFile)
            {
                pageGetURL = uri.Segments.Last();
            }
            else
            {
                pageGetURL = uri.Segments[uri.Segments.Count() - 2].Replace("/", "");
            }

            string resp = "";

            string sqlString = "SELECT CASE\n" +
            "         WHEN CM.CHILD_MENUID IN ('67', '260') THEN\n" +
            "          'A'\n" +
            "         ELSE\n" +
            "          'O'\n" +
            "       END PROFILETYPE\n" +
            "  FROM ZNI_USER1 ZU\n" +
            " INNER JOIN PROFILE_DETAIL PD\n" +
            "    ON PD.PROFILE_ID = ZU.PROFILE\n" +
            " INNER JOIN CHILD_MENU CM\n" +
            "    ON CM.MAIN_MENU_ID = PD.MAINMENU_ID\n" +
            "   AND CM.CHILD_MENUID = PD.CHILDMENU_ID\n" +
            " WHERE ZU.U_ID = '" + HttpContext.Current.Session["U_ID"].ToString() + "'\n" +
            "   AND CM.HYPERLINK = '" + pageGetURL + "'";
            DataTable dt = new DataTable();
            SqlConnection con_ = new SqlConnection(clvar.Strcon());
            try
            {
                con_.Open();
                SqlDataAdapter sda_ = new SqlDataAdapter(sqlString, con_);
                sda_.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    resp = dt.Rows[0]["ProfileType"].ToString();
                }
                else
                {
                    resp = "0";
                }

            }
            catch (Exception ex)
            {
                resp = ex.Message;
            }
            finally { con_.Close(); }
            return resp;
        }

        public static DataTable GetConsignmentDates(DataTable dt)
        {

            string resp = "";
            string consignmentNumbers = "";
            foreach (DataRow dr in dt.Rows)
            {
                consignmentNumbers += "'" + dr["ConsignmentNumber"].ToString() + "'";
            }
            consignmentNumbers = consignmentNumbers.Replace("''", "','");
            string sqlString = "SELECT C.CONSIGNMENTNUMBER,\n" +
            "       FORMAT(C.BOOKINGDATE, 'yyyy/MM/dd') BOOKINGDATE,\n" +
            "       FORMAT(C.ACCOUNTRECEIVINGDATE, 'yyyy/MM/dd') REPORTINGDATE, c.isApproved\n" +
            "  FROM CONSIGNMENT C\n" +
            " WHERE C.CONSIGNMENTNUMBER IN\n" +
            "       (" + consignmentNumbers + ")";


            DataTable dt_ = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sqlString, con);
                sda.Fill(dt_);

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return dt_;
        }

        public static string GetDayEndTime()
        {
            DataTable dt = new DataTable();
            string time = "";
            string query = "select * from MnP_DayEnd_Timings mdt \n" +
                           " where mdt.ZoneCode = '" + HttpContext.Current.Session["ZoneCode"].ToString() + "' \n" +
                           "   AND mdt.BranchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "' \n" +
                           "   AND mdt.status = '1'\n" +
                           "   AND mdt.doc_type = 'O'";
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    time = dt.Rows[0]["DayEndTime"].ToString();
                }
                else
                {
                    time = "";
                }
            }
            catch (Exception ex)
            { time = ""; }
            finally { con.Close(); }
            return time;
        }
    }
}