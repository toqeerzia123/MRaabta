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
    public partial class Invoice_Adjustment : System.Web.UI.Page
    {
        Cl_Variables clvar = new Cl_Variables();
        CommonFunction func = new CommonFunction();
        Consignemnts con = new Consignemnts();
        cl_InvoiceCancelation cli = new cl_InvoiceCancelation();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Get_invoiceAdjStatus();
                Get_origin();
                Get_Service_Type();
                Get_ConType();
                Get_PriceModifier();
                DataTable dt_Ic = new DataTable();


                dt_Ic.Columns.Add("BookingDate", typeof(string));
                dt_Ic.Columns.Add("invoiceNumber", typeof(string));
                dt_Ic.Columns.Add("consignmentNumber", typeof(string));
                dt_Ic.Columns.Add("Account_No", typeof(string));
                dt_Ic.Columns.Add("Origin", typeof(string));
                dt_Ic.Columns.Add("Destination", typeof(string));
                dt_Ic.Columns.Add("Service_Type", typeof(string));
                dt_Ic.Columns.Add("ConsignmentType_id", typeof(string));
                dt_Ic.Columns.Add("Weight", typeof(string));
                dt_Ic.Columns.Add("TotalAmount", typeof(string));
                dt_Ic.Columns.Add("Gst", typeof(string));
                dt_Ic.Columns.Add("createdBy", typeof(string));
                dt_Ic.Columns.Add("createdOn", typeof(string));
                dt_Ic.Columns.Add("modifiedBy", typeof(string));
                dt_Ic.Columns.Add("modifiedOn", typeof(string));
                dt_Ic.Columns.Add("IsIntl", typeof(string));
                dt_Ic.Columns.Add("NoteType", typeof(string));
                dt_Ic.Columns.Add("NewAmount", typeof(string));
                dt_Ic.Columns.Add("NewGst", typeof(string));
                dt_Ic.Columns.Add("priceModifierId", typeof(string));
                dt_Ic.Columns.Add("cm_value", typeof(string));
                dt_Ic.Columns.Add("cm_gst", typeof(string));

                ViewState["InvoiceConsingnment"] = dt_Ic;
            }
        }

        public void Get_invoiceAdjStatus()
        {
            DataTable ds = Get_invoiceAdjStatus_();
            if (ds.Rows.Count != 0)
            {
                dd_Reason.DataTextField = "Status";
                dd_Reason.DataValueField = "ID";
                dd_Reason.DataSource = ds.DefaultView;
                dd_Reason.DataBind();
            }
        }

        public DataTable Get_invoiceAdjStatus_()
        {
            string query = "select * from Mnp_InvoiceAdjustment_Status l where l.Active = '1'";
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

        public DataTable Get_PriceModifier_()
        {
            string query = "SELECT* FROM PriceModifiers pm WHERE pm.[status] ='1'";
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

        public void Get_PriceModifier()
        {
            DataTable ds = Get_PriceModifier_();
            if (ds.Rows.Count != 0)
            {
                ViewState["PriceModifier"] = ds;
            }
        }

        public void Get_origin()
        {
            DataSet ds = Branch_();
            if (ds.Tables.Count != 0)
            {
                ViewState["Branches"] = ds.Tables[0];
            }
        }

        public DataSet Branch_()
        {
            DataSet Ds_1 = new DataSet();

            try
            {
                string query = "SELECT b.branchCode, \n"
               + "       b.name     BranchName, sname BName \n"
               + "FROM   Branches                          b \n"
               + "where b.[status] ='1'  \n"
               + "GROUP BY \n"
               + "       b.branchCode, \n"
               + "       b.name,sname order by b.name ASC";

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

        public DataSet ServiceTypeName_()
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
            DataSet ds = ServiceTypeName_();
            if (ds.Tables.Count != 0)
            {
                ViewState["ServiceType"] = ds.Tables[0];
            }
        }

        public DataSet ConsignmentType_()
        {
            DataSet Ds_1 = new DataSet();

            try
            {
                string query = " SELECT ct.id, ct.name ConsignmentType FROM ConsignmentType ct  \n"
                + "       WHERE ct.[status]='1' \n"
                + "       GROUP BY ct.name,ct.id";

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

        public void Get_ConType()
        {
            DataSet ds = ConsignmentType_();
            if (ds.Tables.Count != 0)
            {
                ViewState["ConType"] = ds.Tables[0];
            }
        }

        public DataTable GetInvoiceHeader(Cl_Variables clvar)
        {

            string sqlString = "selecT i.companyId,cc.branchCode, cc.accountNo, cc.name, i.startDate, i.endDate, i.invoiceDate,isnull(cpa.calculationBase,0) calculationBase, isnull(cpa.modifiedCalculationValue,0) modifiedCalculationValue, isnull(cc.DIscountOnDomestic,0) DIscountOnDomestic,isnull(cc.discountOnSample,0) discountOnSample, isnull(cc.discountOnDocument,0) discountOnDocument  \n"
               + "  from Invoice i \n"
               + " inner join CreditClients cc \n"
               + "    on cc.id = i.clientId \n"
               + " Left OUTER JOIN ClientPriceModifierAssociation Cpa \n"
               + " ON cpa.creditClientId = cc.id"
               + " where i.invoiceNumber = '" + clvar.RefNumber + "'";

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

        protected void btn_getConsignment_Click(object sender, EventArgs e)
        {
            clvar = new Cl_Variables();
            string invoiceNo = txt_invoiceNo.Text;
            clvar.RefNumber = invoiceNo;
            //   DataTable dt = func.GetConsignmentsInInvoice(clvar);
            DataTable header = GetInvoiceHeader(clvar);

            if (header.Rows.Count > 0)
            {
                Errorid.Text = "";
                lbl_InvoiceNo.Text = txt_invoiceNo.Text;
                txt_AC_Info.Text = header.Rows[0]["AccountNo"].ToString();
                txt_ClientName.Text = header.Rows[0]["name"].ToString();
                txt_EndDate.Text = DateTime.Parse(header.Rows[0]["EndDate"].ToString()).ToShortDateString();
                txt_StartDate.Text = DateTime.Parse(header.Rows[0]["startDate"].ToString()).ToShortDateString();
                txt_InvoiceDate.Text = header.Rows[0]["invoiceDate"].ToString().Substring(0, 10);

                if (header.Rows[0]["calculationBase"].ToString() == "1")
                {
                    lbl_Base.Text = "Value";
                }
                else if (header.Rows[0]["calculationBase"].ToString() == "2")
                {
                    lbl_Base.Text = "Percentage";
                }
                lbl_ModifiedValue.Text = header.Rows[0]["modifiedCalculationValue"].ToString();
                if (header.Rows[0]["DIscountOnDomestic"].ToString() != "")
                {
                    lbl_DiscountOnDomestic.Text = header.Rows[0]["DIscountOnDomestic"].ToString();
                }
                else
                {
                    lbl_DiscountOnDomestic.Text = "0";// dt.Rows[0]["DIscountOnDomestic"].ToString();
                }

                if (header.Rows[0]["discountOnDocument"].ToString() != "")
                {
                    lbl_DomesticDocument.Text = header.Rows[0]["discountOnDocument"].ToString();
                }
                else
                {
                    lbl_DomesticDocument.Text = "0";
                }

                if (header.Rows[0]["discountOnSample"].ToString() != "")
                {
                    lbl_SampleDocument.Text = header.Rows[0]["discountOnSample"].ToString();
                }
                else
                {
                    lbl_SampleDocument.Text = "0";
                }
                ViewState["BranchCode"] = header.Rows[0]["branchCode"].ToString();

            }
            else
            {
                gv_cns.DataSource = null;
                gv_cns.DataBind();
                Errorid.Text = "No Record Found";
                Errorid.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void gv_cns_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataTable dt_Branch = (DataTable)ViewState["Branches"];
                DataTable dt_ConType = (DataTable)ViewState["ConType"];
                DataTable dt_ServiceType = (DataTable)ViewState["ServiceType"];
                DataTable dt_PriceModifier = (DataTable)ViewState["PriceModifier"];

                DropDownList origin_ = (e.Row.FindControl("dd_origin") as DropDownList);
                DropDownList Destination_ = (e.Row.FindControl("dd_Destination") as DropDownList);
                DropDownList ConsignmentType = (e.Row.FindControl("dd_ConsignmentType") as DropDownList);
                DropDownList ServiceType = (e.Row.FindControl("dd_ServiceType") as DropDownList);
                DropDownList priceModifier = (e.Row.FindControl("Dd_priceModifier") as DropDownList);

                HiddenField hd_origin = (e.Row.FindControl("hd_origin") as HiddenField);
                HiddenField hd_Destination_ = (e.Row.FindControl("hd_destination") as HiddenField);
                HiddenField hd_ConsignmentType = (e.Row.FindControl("hd_ConsignmentTypeID") as HiddenField);
                HiddenField hd_ServiceType = (e.Row.FindControl("hd_servicetypeName") as HiddenField);
                HiddenField hd_consignmentNo = (e.Row.FindControl("hd_Consignment") as HiddenField);
                HiddenField hd_PriceModifier = (e.Row.FindControl("hd_PriceModifier") as HiddenField);


                DataTable dt_1 = (DataTable)ViewState["InvoiceConsingnment"];

                if (dt_Branch.Rows.Count != 0)
                {
                    origin_.DataTextField = "BName";
                    origin_.DataValueField = "branchCode";
                    origin_.DataSource = dt_Branch.DefaultView;
                    origin_.DataBind();

                    origin_.SelectedValue = hd_origin.Value;

                    Destination_.DataTextField = "BName";
                    Destination_.DataValueField = "branchCode";
                    Destination_.DataSource = dt_Branch.DefaultView;
                    Destination_.DataBind();

                    Destination_.SelectedValue = hd_Destination_.Value;
                }

                if (dt_ConType.Rows.Count != 0)
                {
                    ConsignmentType.DataTextField = "ConsignmentType";
                    ConsignmentType.DataValueField = "id";
                    ConsignmentType.DataSource = dt_ConType.DefaultView;
                    ConsignmentType.DataBind();

                    ConsignmentType.SelectedValue = hd_ConsignmentType.Value;

                }

                if (dt_ServiceType.Rows.Count != 0)
                {
                    ServiceType.DataTextField = "ServiceTypeName";
                    ServiceType.DataValueField = "ServiceTypeName";
                    ServiceType.DataSource = dt_ServiceType.DefaultView;
                    ServiceType.DataBind();

                    ServiceType.SelectedValue = hd_ServiceType.Value;
                }

                if (dt_PriceModifier.Rows.Count != 0)
                {
                    priceModifier.DataTextField = "name";
                    priceModifier.DataValueField = "id";
                    priceModifier.DataSource = dt_PriceModifier.DefaultView;
                    priceModifier.DataBind();

                    if (hd_PriceModifier.Value != "")
                    {
                        priceModifier.SelectedValue = hd_PriceModifier.Value;
                    }

                }

                DataRow[] dr = dt_1.Select("consignmentNumber ='" + hd_consignmentNo.Value + "'");
                if (dr.Length != 0)
                {
                    (e.Row.FindControl("txt_Amount") as Label).Text = dr[0]["NewAmount"].ToString();
                    (e.Row.FindControl("txt_Gst") as Label).Text = dr[0]["NewGst"].ToString();
                    (e.Row.FindControl("dd_ServiceType") as DropDownList).SelectedValue = dr[0]["Service_Type"].ToString();
                }
            }
        }

        public DataTable Add_OcsValidation_Actual(Cl_Variables clvar)
        {
            DataSet ds = new DataSet();

            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand("SP_UpdateCNPrice_OCS_Validation_Actual", orcl);
                orcd.Parameters.AddWithValue("@ConsignmentNo", clvar.consignmentNo);
                orcd.Parameters.AddWithValue("@weight", clvar.Weight);
                orcd.Parameters.AddWithValue("@origin", clvar.origin);
                orcd.Parameters.AddWithValue("@Destination", clvar.destination);
                orcd.Parameters.AddWithValue("@Con_type", clvar.Con_Type);
                orcd.Parameters.AddWithValue("@ServiceTypeName", clvar.ServiceTypeName);
                orcd.Parameters.AddWithValue("@AccountNo", clvar.AccountNo);
                //orcd.Parameters.AddWithValue("@AccountNo", clvar.AccountNo);


                orcd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                orcl.Close();
            }
            catch (Exception)
            { }

            return ds.Tables[1];
        }

        public DataTable Add_RNRValidation_Actual(Cl_Variables clvar)
        {
            DataSet ds = new DataSet();

            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand("SP_UpdateCNPrice_RNR_Validation_Actual", orcl);
                orcd.Parameters.AddWithValue("@ConsignmentNo", clvar.consignmentNo);
                orcd.Parameters.AddWithValue("@weight", clvar.Weight);
                orcd.Parameters.AddWithValue("@origin", clvar.origin);
                orcd.Parameters.AddWithValue("@Destination", clvar.destination);
                orcd.Parameters.AddWithValue("@Con_type", clvar.Con_Type);
                orcd.Parameters.AddWithValue("@ServiceTypeName", clvar.ServiceTypeName);
                orcd.Parameters.AddWithValue("@AccountNo", clvar.AccountNo);
                orcd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                orcl.Close();
            }
            catch (Exception)
            { }

            return ds.Tables[0];
        }

        public DataTable Add_intValidation_Actual(Cl_Variables clvar)
        {
            DataSet ds = new DataSet();

            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand("SP_UpdateCNPrice_Int_Validation_Actual", orcl);
                orcd.Parameters.AddWithValue("@ConsignmentNo", clvar.consignmentNo);
                orcd.Parameters.AddWithValue("@weight", clvar.Weight);
                orcd.Parameters.AddWithValue("@origin", clvar.origin);
                orcd.Parameters.AddWithValue("@Destination", clvar.destination);
                orcd.Parameters.AddWithValue("@Con_type", clvar.Con_Type);
                orcd.Parameters.AddWithValue("@ServiceTypeName", clvar.ServiceTypeName);
                orcd.Parameters.AddWithValue("@AccountNo", clvar.AccountNo);

                orcd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                orcl.Close();
            }
            catch (Exception)
            { }

            return ds.Tables[0];
        }

        protected void gv_cns_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Re_compute")
            {
                GridViewRow row = (GridViewRow)(((Button)e.CommandSource).NamingContainer);
                DataTable dt_1 = (DataTable)ViewState["InvoiceConsingnment"];


                string consignmnet = (row.FindControl("hd_Consignment") as HiddenField).Value;
                string Origin = (row.FindControl("dd_origin") as DropDownList).SelectedValue;
                Double TotalAmount = double.Parse((row.FindControl("hd_Amount") as HiddenField).Value) + double.Parse((row.FindControl("hd_gst") as HiddenField).Value);
                string Destination = (row.FindControl("dd_Destination") as DropDownList).SelectedValue;
                string ConsignmentTypeid = (row.FindControl("dd_ConsignmentType") as DropDownList).SelectedValue;
                TextBox Hd_Ac = (row.FindControl("txt_Accountno") as TextBox);

                string Hidden_1 = (row.FindControl("hd_Wieght") as HiddenField).Value;
                string Hidden_2 = (row.FindControl("txt_Weight") as TextBox).Text;
                string ServiceTypeName = (row.FindControl("dd_ServiceType") as DropDownList).SelectedValue;

                DataTable dt_ser = (DataTable)ViewState["ServiceType"];
                DataRow[] dr_Se = dt_ser.Select("ServiceTypeName ='" + ServiceTypeName + "'");
                string Int = (row.FindControl("hd_type") as HiddenField).Value;

                clvar.origin = ViewState["BranchCode"].ToString();
                // DataTable BranchGst = func.GetBranchGst(clvar);
                DataSet BranchGst = BranchGSTInformation(clvar);
                double gst = double.Parse(BranchGst.Tables[0].Rows[0]["gst"].ToString());

                if (dr_Se[0]["isintl"].ToString() == "False")
                {
                    DataTable dt_ = new DataTable();
                    if (ServiceTypeName == "Road n Rail")
                    {
                        clvar.consignmentNo = consignmnet;
                        clvar.origin = Origin;
                        clvar.destination = Destination;
                        clvar.Con_Type = int.Parse(ConsignmentTypeid);
                        clvar.ServiceTypeName = ServiceTypeName;
                        clvar.Weight = float.Parse(Hidden_2);
                        clvar.AccountNo = Hd_Ac.Text;

                        dt_ = Add_RNRValidation_Actual(clvar);
                        (row.FindControl("txt_Amount") as Label).Text = string.Format("{0:N0}", double.Parse(dt_.Rows[0]["totalAmount"].ToString()));
                        (row.FindControl("txt_Gst") as Label).Text = string.Format("{0:N0}", double.Parse(dt_.Rows[0]["Gst"].ToString()));

                        Double TotalAmount_ = double.Parse(dt_.Rows[0]["totalAmount"].ToString()) + double.Parse(dt_.Rows[0]["Gst"].ToString());

                        if (dd_Reason.SelectedValue != "3")
                        {
                            if (TotalAmount_ > TotalAmount)
                            {
                                double dif = TotalAmount_ - TotalAmount;

                                (row.FindControl("lbl_NoteType") as HiddenField).Value = "D";

                            }
                            else
                            {
                                double dif = TotalAmount - TotalAmount_;
                                (row.FindControl("lbl_NoteType") as HiddenField).Value = "C";

                            }
                        }
                        else
                        {
                            if (rb_1.SelectedValue == "D")
                            {
                                (row.FindControl("lbl_NoteType") as HiddenField).Value = "D";
                            }
                            else
                            {
                                (row.FindControl("lbl_NoteType") as HiddenField).Value = "C";

                            }
                        }

                        DataRow[] dr = dt_1.Select("consignmentNumber=" + consignmnet);
                        dr[0]["NoteType"] = (row.FindControl("lbl_NoteType") as HiddenField).Value;
                        dr[0]["NewAmount"] = (row.FindControl("txt_Amount") as Label).Text;
                        dr[0]["NewGst"] = (row.FindControl("txt_Gst") as Label).Text;

                        if (rb_1.SelectedValue == "D" && dr[0]["NoteType"] == "C")
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Selected Document is now Debit Note.')", true);
                            dr[0]["NewAmount"] = "0"; //(row.FindControl("txt_Amount") as Label).Text;
                            dr[0]["NewGst"] = "0";  //(row.FindControl("txt_Gst") as Label).Text;

                            string consignmnet_ = (row.FindControl("hd_Consignment") as HiddenField).Value;
                            DataRow[] dr_ = dt_1.Select("consignmentNumber=" + consignmnet_);
                            //if (dr_.Length != 0)
                            //{
                            //    dt_1.Rows.Remove(dr_[0]);
                            //    dt_1.AcceptChanges();
                            //}

                            //dt_1.AcceptChanges();
                            //ViewState["InvoiceConsingnment"] = dt_1;
                            //gv_cns.DataSource = dt_1;
                            //gv_cns.DataBind();

                            return;
                        }
                        else if (rb_1.SelectedValue == "C" && dr[0]["NoteType"] == "D")
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Selected Document is now Credit Note.')", true);
                            dr[0]["NewAmount"] = "0"; //(row.FindControl("txt_Amount") as Label).Text;
                            dr[0]["NewGst"] = "0";  //(row.FindControl("txt_Gst") as Label).Text;

                            string consignmnet_ = (row.FindControl("hd_Consignment") as HiddenField).Value;
                            DataRow[] dr_ = dt_1.Select("consignmentNumber=" + consignmnet_);
                            //if (dr_.Length != 0)
                            //{
                            //    dt_1.Rows.Remove(dr_[0]);
                            //    dt_1.AcceptChanges();
                            //}

                            //ViewState["InvoiceConsingnment"] = dt_1;
                            //dt_1.AcceptChanges();

                            //gv_cns.DataSource = dt_1;
                            //gv_cns.DataBind();


                            return;
                        }

                        else
                        {
                            ViewState["InvoiceConsingnment"] = dt_1;
                            dt_1.AcceptChanges();

                        }

                    }
                    else
                    {
                        clvar.consignmentNo = consignmnet;
                        clvar.origin = Origin;
                        clvar.destination = Destination;
                        clvar.Con_Type = int.Parse(ConsignmentTypeid);
                        clvar.ServiceTypeName = ServiceTypeName;
                        clvar.Weight = float.Parse(Hidden_2);
                        clvar.AccountNo = Hd_Ac.Text;

                        dt_ = Add_OcsValidation_Actual(clvar);
                        Double TotalAmount_ = double.Parse(dt_.Rows[0]["Amount"].ToString()) + double.Parse(dt_.Rows[0]["Gst"].ToString());

                        //if (TotalAmount_ >= TotalAmount)
                        //{
                        //    double dif = TotalAmount_ - TotalAmount;
                        //    (row.FindControl("lbl_NoteType") as HiddenField).Value = "D";
                        //}
                        //else
                        //{
                        //    double dif = TotalAmount - TotalAmount_;
                        //    (row.FindControl("lbl_NoteType") as HiddenField).Value = "C";

                        //}

                        if (dd_Reason.SelectedValue != "3")
                        {
                            if (TotalAmount_ > TotalAmount)
                            {
                                double dif = TotalAmount_ - TotalAmount;

                                (row.FindControl("lbl_NoteType") as HiddenField).Value = "D";

                            }
                            else
                            {
                                double dif = TotalAmount - TotalAmount_;
                                (row.FindControl("lbl_NoteType") as HiddenField).Value = "C";

                            }
                        }
                        else
                        {
                            if (rb_1.SelectedValue == "D")
                            {
                                (row.FindControl("lbl_NoteType") as HiddenField).Value = "D";
                            }
                            else
                            {
                                (row.FindControl("lbl_NoteType") as HiddenField).Value = "C";

                            }
                        }


                        (row.FindControl("txt_Amount") as Label).Text = string.Format("{0:N0}", double.Parse(dt_.Rows[0]["Amount"].ToString()));
                        (row.FindControl("txt_Gst") as Label).Text = string.Format("{0:N0}", double.Parse(dt_.Rows[0]["Gst"].ToString()));

                        DataRow[] dr = dt_1.Select("consignmentNumber=" + consignmnet);
                        dr[0]["NoteType"] = (row.FindControl("lbl_NoteType") as HiddenField).Value;
                        dr[0]["NewAmount"] = (row.FindControl("txt_Amount") as Label).Text;
                        dr[0]["NewGst"] = (row.FindControl("txt_Gst") as Label).Text;

                        if (rb_1.SelectedValue == "D" && dr[0]["NoteType"] == "C")
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Selected Document is Debit Note.')", true);
                            dr[0]["NewAmount"] = "0"; //(row.FindControl("txt_Amount") as Label).Text;
                            dr[0]["NewGst"] = "0";  //(row.FindControl("txt_Gst") as Label).Text;

                            string consignmnet_ = (row.FindControl("hd_Consignment") as HiddenField).Value;
                            DataRow[] dr_ = dt_1.Select("consignmentNumber=" + consignmnet_);
                            //if (dr_.Length != 0)
                            //{
                            //    dt_1.Rows.Remove(dr_[0]);
                            //    dt_1.AcceptChanges();
                            //}

                            //dt_1.AcceptChanges();
                            //ViewState["InvoiceConsingnment"] = dt_1;
                            //gv_cns.DataSource = dt_1;
                            //gv_cns.DataBind();

                            return;
                        }
                        else if (rb_1.SelectedValue == "C" && dr[0]["NoteType"] == "D")
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Selected Document is Credit Note.')", true);
                            dr[0]["NewAmount"] = "0"; //(row.FindControl("txt_Amount") as Label).Text;
                            dr[0]["NewGst"] = "0";  //(row.FindControl("txt_Gst") as Label).Text;

                            string consignmnet_ = (row.FindControl("hd_Consignment") as HiddenField).Value;
                            DataRow[] dr_ = dt_1.Select("consignmentNumber=" + consignmnet_);
                            //if (dr_.Length != 0)
                            //{
                            //    dt_1.Rows.Remove(dr_[0]);
                            //    dt_1.AcceptChanges();
                            //}

                            //dt_1.AcceptChanges();
                            //ViewState["InvoiceConsingnment"] = dt_1;
                            //gv_cns.DataSource = dt_1;
                            //gv_cns.DataBind();


                            return;
                        }
                        else
                        {
                            dt_1.AcceptChanges();
                            ViewState["InvoiceConsingnment"] = dt_1;
                        }
                    }
                }
                else
                {
                    DataTable dt_ = new DataTable();
                    clvar.consignmentNo = consignmnet;
                    clvar.origin = Origin;
                    clvar.destination = Destination;
                    clvar.Con_Type = int.Parse(ConsignmentTypeid);
                    clvar.ServiceTypeName = ServiceTypeName;
                    clvar.Weight = float.Parse(Hidden_2);
                    clvar.AccountNo = Hd_Ac.Text;

                    dt_ = Add_intValidation_Actual(clvar);
                    (row.FindControl("txt_Amount") as Label).Text = string.Format("{0:N0}", double.Parse(dt_.Rows[0]["totalAmount"].ToString()));
                    (row.FindControl("txt_Gst") as Label).Text = string.Format("{0:N0}", double.Parse(dt_.Rows[0]["Gst"].ToString()));

                    Double TotalAmount_ = double.Parse(dt_.Rows[0]["totalAmount"].ToString()) + double.Parse(dt_.Rows[0]["Gst"].ToString());
                    if (dd_Reason.SelectedValue != "3")
                    {
                        if (TotalAmount_ > TotalAmount)
                        {
                            double dif = TotalAmount_ - TotalAmount;

                            (row.FindControl("lbl_NoteType") as HiddenField).Value = "D";

                        }
                        else
                        {
                            double dif = TotalAmount - TotalAmount_;
                            (row.FindControl("lbl_NoteType") as HiddenField).Value = "C";

                        }
                    }
                    else
                    {
                        if (rb_1.SelectedValue == "D")
                        {
                            (row.FindControl("lbl_NoteType") as HiddenField).Value = "D";
                        }
                        else
                        {
                            (row.FindControl("lbl_NoteType") as HiddenField).Value = "C";

                        }
                    }


                    DataRow[] dr = dt_1.Select("consignmentNumber=" + consignmnet);
                    dr[0]["NoteType"] = (row.FindControl("lbl_NoteType") as HiddenField).Value;
                    dr[0]["NewAmount"] = (row.FindControl("txt_Amount") as Label).Text;
                    dr[0]["NewGst"] = (row.FindControl("txt_Gst") as Label).Text;

                    if (rb_1.SelectedValue == "D" && dr[0]["NoteType"] == "C")
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Selected Document is Debit Note.')", true);
                        dr[0]["NewAmount"] = "0"; //(row.FindControl("txt_Amount") as Label).Text;
                        dr[0]["NewGst"] = "0";  //(row.FindControl("txt_Gst") as Label).Text;

                        string consignmnet_ = (row.FindControl("hd_Consignment") as HiddenField).Value;
                        DataRow[] dr_ = dt_1.Select("consignmentNumber=" + consignmnet_);
                        //if (dr_.Length != 0)
                        //{
                        //    dt_1.Rows.Remove(dr_[0]);
                        //    dt_1.AcceptChanges();
                        //}

                        //dt_1.AcceptChanges();
                        //ViewState["InvoiceConsingnment"] = dt_1;
                        //gv_cns.DataSource = dt_1;
                        //gv_cns.DataBind();

                        return;
                    }
                    else if (rb_1.SelectedValue == "C" && dr[0]["NoteType"] == "D")
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Selected Document is Credit Note.')", true);
                        dr[0]["NewAmount"] = "0"; //(row.FindControl("txt_Amount") as Label).Text;
                        dr[0]["NewGst"] = "0";  //(row.FindControl("txt_Gst") as Label).Text;

                        string consignmnet_ = (row.FindControl("hd_Consignment") as HiddenField).Value;
                        DataRow[] dr_ = dt_1.Select("consignmentNumber=" + consignmnet_);
                        //if (dr_.Length != 0)
                        //{
                        //    dt_1.Rows.Remove(dr_[0]);
                        //    dt_1.AcceptChanges();
                        //}

                        //dt_1.AcceptChanges();
                        //ViewState["InvoiceConsingnment"] = dt_1;
                        //gv_cns.DataSource = dt_1;
                        //gv_cns.DataBind();

                        return;
                    }

                    else
                    {
                        dt_1.AcceptChanges();
                        ViewState["InvoiceConsingnment"] = dt_1;
                    }


                }
                ViewState["InvoiceConsingnment"] = dt_1;

                GetDetail();
            }
            if (e.CommandName == "Remove")
            {
                GridViewRow row = (GridViewRow)(((Button)e.CommandSource).NamingContainer);
                DataTable dt_1 = (DataTable)ViewState["InvoiceConsingnment"];


                string consignmnet = (row.FindControl("hd_Consignment") as HiddenField).Value;
                DataRow[] dr = dt_1.Select("consignmentNumber=" + consignmnet);
                if (dr.Length != 0)
                {
                    dt_1.Rows.Remove(dr[0]);
                    dt_1.AcceptChanges();
                }

                ViewState["InvoiceConsingnment"] = dt_1;
                gv_cns.DataSource = dt_1;
                gv_cns.DataBind();
                GetDetail();

            }
        }

        public DataTable GetConsignmentsInInvoice_(Cl_Variables clvar)
        {

            //string sqlString = "selecT  ROW_NUMBER() Over (Order by ic.consignmentNumber) As [Sno], cast(c.bookingDate as date) BookingDate,\n" +
            //"       ic.consignmentNumber,\n" +
            //"       c.pieces,\n" +
            //"       c.serviceTypeName,c.orgin,c.destination destination_,c.consignmentTypeId,\n" +
            //"       b.name destination,\n" +
            //"       c.weight,\n" +
            //"       c.totalAmount totalAmount, c.gst,stn.IsIntl\n" +
            //"  from InvoiceConsignment ic\n" +
            //" inner join Consignment c\n" +
            //"    on c.consignmentNumber = ic.consignmentNumber\n" +
            //" inner join Branches b\n" +
            //"    on b.branchCode = c.destination\n" +
            //" INNER JOIN ServiceTypes_New stn      \n" +
            //" ON c.serviceTypeName = stn.serviceTypeName    \n" +
            //" where ic.invoiceNumber = '" + clvar.RefNumber + "'";

            string sql = ""
               + "SELECT  \n"
               + "       CAST(c.bookingDate AS date)     BookingDate, \n"
               + "       ic.invoiceNumber, \n"
               + "       ic.consignmentNumber, \n"
               + "       c.consignerAccountNo Accountno, \n"
               + "       c.orgin, \n"
               + "       c.destination                   destination, \n"
               + "       c.serviceTypeName, \n"
               + "       c.consignmentTypeId, \n"
               + "       c.weight, \n"
               + "       c.totalAmount                   totalAmount, \n"
               + "       c.gst, \n"
               + "       stn.IsIntl, calculatedValue CM_value,calculatedGST CM_gst, priceModifierId \n"
               + "FROM   InvoiceConsignment ic \n"
               + "       INNER JOIN Consignment c \n"
               + "            ON  c.consignmentNumber = ic.consignmentNumber \n"
               + "       INNER JOIN Branches b \n"
               + "            ON  b.branchCode = c.destination \n"
               + "       INNER JOIN ServiceTypes_New stn \n"
               + "            ON  c.serviceTypeName = stn.serviceTypeName "
               //    + "       inner join Invoice I "
               //    + "            ON  i.InvoiceNumber = ic.InvoiceNumber"
               + "       LEFT OUTER JOIN ConsignmentModifier CM ON "
               + "            ON CM.consignmentNumber = C.consignmentNumber "
               + " where ic.InvoiceNumber ='" + clvar.InvoiceNo + "' and ic.consignmentNumber ='" + clvar.consignmentNo + "'  and isnull(i.IsInvoiceCanceled,'0') ='0'";

            sql = ""
              + "SELECT  \n"
              + "       CAST(c.bookingDate AS date)     BookingDate, \n"
              + "       ic.invoiceNumber, \n"
              + "       ic.consignmentNumber, \n"
              + "       c.consignerAccountNo Accountno, \n"
              + "       c.orgin, \n"
              + "       c.destination                   destination, \n"
              + "       c.serviceTypeName, \n"
              + "       c.consignmentTypeId, \n"
              + "       c.weight, \n"
              + "       c.totalAmount                   totalAmount, \n"
              + "       c.gst, \n"
              + "       stn.IsIntl, isnull(round(calculatedValue,2),'0') CM_value,isnull(round(calculatedGST,2),0) CM_gst, priceModifierId \n"
              + " "
              + "FROM   InvoiceConsignment ic \n"
              + "       INNER JOIN Consignment c \n"
              + "            ON  c.consignmentNumber = ic.consignmentNumber \n"
              + "       INNER JOIN Branches b \n"
              + "            ON  b.branchCode = c.destination \n"
              + "       INNER JOIN ServiceTypes_New stn \n"
              + "            ON  c.serviceTypeName = stn.serviceTypeName "
              + "       LEFT OUTER JOIN ConsignmentModifier CM "
              + "            ON CM.consignmentNumber = C.consignmentNumber "
              + " where  c.consignmentNumber ='" + clvar.consignmentNo + "' and c.orgin='" + HttpContext.Current.Session["BRANCHCODE"].ToString() + "'";



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

        protected void btn_getConsignment_Click_(object sender, EventArgs e)
        {
            clvar = new Cl_Variables();
            string invoiceNo = txt_invoiceNo.Text;
            clvar.consignmentNo = TextBox1.Text;
            clvar.InvoiceNo = txt_invoiceNo.Text;
            DataTable dt = GetConsignmentsInInvoice_(clvar);

            if (dt.Rows.Count > 0)
            {
                ViewState["InvoiceList"] = dt;

                DataTable dt_con = (DataTable)ViewState["InvoiceConsingnment"];
                DataRow[] dr = dt_con.Select("consignmentNumber =" + TextBox1.Text);
                if (dr.Length == 0)
                {
                    if (dt.Rows.Count > 0)
                    {
                        dt_con.Rows.Add(dt.Rows[0][0], dt.Rows[0][1], dt.Rows[0][2], dt.Rows[0][3], dt.Rows[0][4], dt.Rows[0][5], dt.Rows[0][6], dt.Rows[0][7], dt.Rows[0][8], dt.Rows[0]["TotalAmount"], dt.Rows[0]["gst"], "0", "0", "0", "0", dt.Rows[0]["IsIntl"].ToString(), "N", "0", "0", dt.Rows[0]["priceModifierId"].ToString(), dt.Rows[0]["CM_value"].ToString(), dt.Rows[0]["CM_gst"].ToString());
                    }
                    dt_con.AcceptChanges();
                    gv_cns.DataSource = dt_con;
                    gv_cns.DataBind();
                    GetDetail();
                    TextBox1.Text = "";
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Consignment Already Present')", true);
                    return;
                }
            }
            else
            {
                gv_cns.DataSource = null;
                gv_cns.DataBind();
                Errorid.Text = "No Record Found";
                Errorid.ForeColor = System.Drawing.Color.Red;
            }
        }

        public void GetDetail()
        {
            double ConsingmnetAmount = 0;
            double ConsingmnetGst = 0;
            double iConsingmnetAmount = 0;
            double iConsingmnetGst = 0;


            foreach (GridViewRow gr in gv_cns.Rows)
            {
                double Local_ConsingmnetAmount = 0;
                double Local_ConsingmnetGst = 0;
                double int_ConsingmnetAmount = 0;
                double int_ConsingmnetGst = 0;

                double NewAmount = double.Parse((gr.FindControl("txt_Amount") as Label).Text);
                double Newgst = double.Parse((gr.FindControl("txt_Gst") as Label).Text);
                double CM_NewAmount = double.Parse((gr.FindControl("txt_CM_Amount") as TextBox).Text);
                double CM_Newgst = double.Parse((gr.FindControl("txt_CM_Gst") as TextBox).Text);


                double OldAmount = double.Parse((gr.FindControl("hd_Amount") as HiddenField).Value);
                double Oldgst = double.Parse((gr.FindControl("hd_gst") as HiddenField).Value);
                string IsIntl = (gr.FindControl("hd_type") as HiddenField).Value;

                string ServiceTypeName = (gr.FindControl("dd_ServiceType") as DropDownList).SelectedValue;

                DataTable dt_ser = (DataTable)ViewState["ServiceType"];
                DataRow[] dr_Se = dt_ser.Select("ServiceTypeName ='" + ServiceTypeName + "'");
                string Int = (gr.FindControl("hd_type") as HiddenField).Value;

                if (Int == "False")
                {
                    if (dd_Reason.SelectedValue != "3")
                    {
                        if (NewAmount > 0)
                        {
                            if (NewAmount > OldAmount)
                            {
                                Local_ConsingmnetAmount = NewAmount - OldAmount + CM_NewAmount;
                            }
                            else if (NewAmount < OldAmount)
                            {
                                Local_ConsingmnetAmount = OldAmount - NewAmount + CM_NewAmount;
                            }
                            else
                            {
                                Local_ConsingmnetAmount = NewAmount + CM_NewAmount;// -OldAmount;
                            }
                        }
                        else
                        {
                            Local_ConsingmnetAmount = OldAmount + CM_NewAmount;

                        }
                        if (Newgst > 0)
                        {
                            if (Newgst > Oldgst)
                            {
                                Local_ConsingmnetGst = Newgst - Oldgst + CM_Newgst;
                            }
                            else if (NewAmount < Oldgst)
                            {
                                Local_ConsingmnetGst = Oldgst - Newgst + CM_Newgst;
                            }
                            else
                            {
                                Local_ConsingmnetGst = Newgst + CM_Newgst;// -OldAmount;
                            }

                            //- Oldgst;
                        }
                        else
                        {
                            Local_ConsingmnetGst = Oldgst;

                        }
                    }
                    else
                    {
                        Local_ConsingmnetAmount = NewAmount;
                        Local_ConsingmnetGst = Newgst;
                    }
                    ConsingmnetAmount += Local_ConsingmnetAmount;
                    ConsingmnetGst += Local_ConsingmnetGst;

                    if (lbl_DiscountOnDomestic.Text != "0" && lbl_DiscountOnDomestic.Text != "")
                    {
                        // if(lbl_DiscountOnDomestic.Text == ")
                        Double Fuel = (ConsingmnetAmount / 100) * double.Parse(lbl_DiscountOnDomestic.Text);
                        lbl_domesticDiscount2.Text = string.Format("{0:N0}", Fuel);
                    }
                    else
                    {
                        lbl_domesticDiscount2.Text = "0";
                    }

                }
                else
                {
                    if (dd_Reason.SelectedValue != "3")
                    {
                        if (NewAmount > 0)
                        {
                            if (NewAmount > OldAmount)
                            {
                                int_ConsingmnetAmount = NewAmount - OldAmount + CM_NewAmount;// -OldAmount;
                            }
                            else if (NewAmount < OldAmount)
                            {
                                int_ConsingmnetAmount = OldAmount - NewAmount + CM_NewAmount;
                            }
                            else
                            {
                                int_ConsingmnetAmount = NewAmount + CM_NewAmount;// -OldAmount;
                            }
                        }
                        else
                        {
                            int_ConsingmnetAmount = OldAmount + CM_NewAmount;

                        }
                        if (Newgst > 0)
                        {
                            if (Newgst > Oldgst)
                            {
                                int_ConsingmnetGst = Newgst - Oldgst + CM_Newgst;
                            }
                            else if (NewAmount < Oldgst)
                            {
                                int_ConsingmnetGst = Oldgst - Newgst + CM_Newgst;
                            }
                            else
                            {
                                int_ConsingmnetGst = Newgst + CM_Newgst;// -OldAmount;
                            }
                        }
                        else
                        {
                            int_ConsingmnetGst = Oldgst + CM_Newgst;

                        }
                    }
                    else
                    {
                        int_ConsingmnetAmount = NewAmount;
                        int_ConsingmnetGst = Newgst;
                    }

                    iConsingmnetAmount += int_ConsingmnetAmount;
                    iConsingmnetGst += int_ConsingmnetGst;

                    //discount on document
                    lbl_discountdocument.Text = "0";
                    lbl_domesticDiscount2.Text = "0";
                    lbl_SampleDiscount2.Text = "0";
                    //Discount Document
                    if (ServiceTypeName == "International_Doc")
                    {
                        if (this.lbl_DomesticDocument.Text != "0" && lbl_DomesticDocument.Text != "")
                        {
                            Double Fuel = (iConsingmnetAmount / (100 + double.Parse(lbl_DomesticDocument.Text))) * double.Parse(lbl_DomesticDocument.Text);
                            lbl_discountdocument.Text = string.Format("{0:N0}", Fuel);
                        }
                        else
                        {
                            lbl_discountdocument.Text = "0";
                        }
                    }

                    if (ServiceTypeName == "International_Box" || ServiceTypeName == "International Cargo" || ServiceTypeName == "International_Non-Doc")
                    {
                        if (this.lbl_SampleDocument.Text != "0" && lbl_SampleDocument.Text != "")
                        {
                            Double Fuel = (iConsingmnetAmount / (100 + double.Parse(lbl_SampleDocument.Text))) * double.Parse(lbl_SampleDocument.Text);
                            lbl_SampleDiscount2.Text = string.Format("{0:N0}", Fuel);
                        }
                        else
                        {
                            lbl_SampleDiscount2.Text = "0";
                        }
                    }


                }
            }

            //Fuel Surcharge
            lbl_InvoiceAmount.Text = string.Format("{0:N0}", ConsingmnetAmount);
            lbl_IcnAmount.Text = string.Format("{0:N0}", iConsingmnetAmount);
            // lbl_gst.Text = string.Format("{0:N0}", ConsingmnetGst);

            if (lbl_ModifiedValue.Text != "" && lbl_ModifiedValue.Text != "0")
            {
                Double Fuel = (ConsingmnetAmount / (100)) * double.Parse(lbl_ModifiedValue.Text);
                lbl_FuelSurcharge.Text = string.Format("{0:N0}", Fuel);
            }
            else
            {
                lbl_FuelSurcharge.Text = "0";
            }
            //Domestic Discount

            clvar.origin = ViewState["BranchCode"].ToString();
            // DataTable BranchGst = func.GetBranchGst(clvar);
            DataSet BranchGst = BranchGSTInformation(clvar);

            if (BranchGst.Tables[0].Rows.Count != 0)
            {
                double g = 0;
                double g1 = 0;
                if (lbl_DiscountOnDomestic.Text != "0")
                {
                    g = ConsingmnetAmount - ((ConsingmnetAmount / (100 + double.Parse(lbl_DiscountOnDomestic.Text))) * double.Parse(lbl_DiscountOnDomestic.Text));
                    g1 = (g / (100)) * double.Parse(BranchGst.Tables[0].Rows[0]["gst"].ToString());
                    lbl_gst.Text = string.Format("{0:N0}", g1);
                }
                else
                {
                    g = ConsingmnetAmount;// ((ConsingmnetAmount / 100) * double.Parse(lbl_DiscountOnDomestic.Text));
                    g1 = (g / (100)) * double.Parse(BranchGst.Tables[0].Rows[0]["gst"].ToString());
                    lbl_gst.Text = string.Format("{0:N0}", g1);

                }

                g = double.Parse(lbl_FuelSurcharge.Text);
                g1 = (g / (100 + double.Parse(BranchGst.Tables[0].Rows[0]["gst"].ToString()))) * double.Parse(BranchGst.Tables[0].Rows[0]["gst"].ToString());
                lbl_FuelSurchargeGst.Text = string.Format("{0:N0}", g1);

                if (lbl_DomesticDocument.Text != "0")
                {
                    g = iConsingmnetAmount - ((iConsingmnetAmount / (100 + double.Parse(lbl_DomesticDocument.Text)))) * double.Parse(lbl_DomesticDocument.Text);
                    g1 = (g / (100 + double.Parse(BranchGst.Tables[0].Rows[0]["gst"].ToString()))) * double.Parse(BranchGst.Tables[0].Rows[0]["gst"].ToString());
                    this.lbl_IcnGst.Text = string.Format("{0:N0}", g1);
                }
                else
                {
                    g = iConsingmnetAmount; //- ((iConsingmnetAmount / 100) * double.Parse(lbl_DomesticDocument.Text));
                    g1 = (g / (100 + double.Parse(BranchGst.Tables[0].Rows[0]["gst"].ToString()))) * double.Parse(BranchGst.Tables[0].Rows[0]["gst"].ToString());
                    this.lbl_IcnGst.Text = string.Format("{0:N0}", g1);
                }

                if (lbl_SampleDiscount2.Text != "0" && lbl_SampleDiscount2.Text != "")
                {
                    g = iConsingmnetAmount - ((iConsingmnetAmount / 100) * double.Parse(lbl_SampleDiscount2.Text));
                    g1 = (g / (100 + double.Parse(BranchGst.Tables[0].Rows[0]["gst"].ToString()))) * double.Parse(BranchGst.Tables[0].Rows[0]["gst"].ToString());
                    this.lbl_IcnGst.Text = string.Format("{0:N0}", g1);
                }
                else
                {
                    this.lbl_SampleDiscount2.Text = "0";
                    g = iConsingmnetAmount; //- ((iConsingmnetAmount / 100) * double.Parse(lbl_DomesticDocument.Text));
                    g1 = (g / (100 + double.Parse(BranchGst.Tables[0].Rows[0]["gst"].ToString()))) * double.Parse(BranchGst.Tables[0].Rows[0]["gst"].ToString());
                    this.lbl_IcnGst.Text = string.Format("{0:N0}", g1);
                }
            }

            double totalAmount = iConsingmnetAmount + ConsingmnetAmount + double.Parse(lbl_FuelSurcharge.Text) - double.Parse(this.lbl_DiscountOnDomestic.Text) - double.Parse(this.lbl_DomesticDocument.Text) - double.Parse(this.lbl_SampleDiscount2.Text);
            double totalgst = double.Parse(this.lbl_gst.Text) + double.Parse(this.lbl_IcnGst.Text) + double.Parse(lbl_FuelSurchargeGst.Text);

            this.lbl_TotalAmount.Text = string.Format("{0:N0}", totalAmount);
            this.lbl_TotalGst.Text = string.Format("{0:N0}", totalgst);

        }
        public DataTable Invoice_AdjustmentNumber(Cl_Variables clvar)
        {
            string sql = "  SELECT isnull(codeValue,'0') codeValue \n"
               + "		                FROM   SystemCodes \n"
               + "		                WHERE  codeType = 'INVOICE AD' \n"
               + "		                       AND identifier = '1' \n"
               + "		                       AND [year] = CONVERT(VARCHAR, YEAR(getdate()))";

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

        public void updateInvoice_AdjustmentNumber(Cl_Variables clvar)
        {
            string sql = "  update  SystemCodes set codeValue= '" + clvar.RefNo + "' \n"
               + "		                FROM   SystemCodes \n"
               + "		                WHERE  codeType = 'INVOICE AD' \n"
               + "		                       AND identifier = '1' \n"
               + "		                       AND [year] = CONVERT(VARCHAR, YEAR(getdate()))";

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sql, con);
                sda.SelectCommand.ExecuteNonQuery();

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            //  return dt;
        }

        public void Insert_InvoiceNote(Cl_Variables clvar)
        {
            string sql = "INSERT INTO Invoice_Note \n"
               + "( \n"
               + "	invoiceNumber, \n"
               + "	companyId, \n"
               + "	clientId, \n"
               + "	chkIsAuto, \n"
               + "	startDate, \n"
               + "	endDate, \n"
               + "	invoiceDate, \n"
               + "	createdBy, \n"
               + "	createdOn, \n"
               + "	modifiedBy, \n"
               + "	modifiedOn, \n"
               + "	totalAmount, \n"
               + "	overdueDate, \n"
               + "	deliveryStatus, \n"
               + "	BillNo, \n"
               + "	DiscountOnDomestic, \n"
               + "	DiscountOnDocument, \n"
               + "	DiscountOnSample, \n"
               + "	MonthlyFixCharges, \n"
               + "	IsInvoiceCanceled,Approved,Gst,Discount,Note_type,Note_Number,Fuel,Fuelgst,Reason \n"
               + ") \n"
               + "SELECT  \n"
               + "	invoiceNumber, \n"
               + "	companyId, \n"
               + "	clientId, \n"
               + "	chkIsAuto, \n"
               + "	startDate, \n"
               + "	endDate, \n"
               + "	invoiceDate, \n"
               + "	'" + HttpContext.Current.Session["U_ID"].ToString() + "', \n"
               + "	getdate(), \n"
               + "	modifiedBy, \n"
               + "	modifiedOn, \n"
               + "	'" + clvar.TotalAmount.ToString() + "', \n"
               + "	overdueDate, \n"
               + "	deliveryStatus, \n"
               + "	BillNo, \n"
               + "	DiscountOnDomestic, \n"
               + "	DiscountOnDocument, \n"
               + "	DiscountOnSample, \n"
               + "	MonthlyFixCharges, \n"
               + "	IsInvoiceCanceled, 0,'" + clvar.gst + "','" + clvar.Insurance + "','" + clvar.docPouchNo + "','" + clvar.RefNo + "','" + clvar.Flyer + "','" + clvar.expressionMessage + "', '" + clvar.RemarksID + "' \n"
               + "	 FROM Invoice i WHERE i.invoiceNumber ='" + clvar.InvoiceNo + "' \n"
               + "";


            SqlConnection con = new SqlConnection(clvar.Strcon());
            SqlTransaction transaction;
            SqlCommand cmd1 = new SqlCommand();
            cmd1.Connection = con;

            con.Open();
            transaction = con.BeginTransaction();

            try
            {
                cmd1.Transaction = transaction;
                cmd1.CommandType = CommandType.Text;
                cmd1.CommandText = sql;

                cmd1.ExecuteNonQuery();

                transaction.Commit();
                con.Close();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                con.Close();
            }
        }

        protected void btn_save_Click(object sender, EventArgs e)
        {
            DataTable dt_con = (DataTable)ViewState["InvoiceConsingnment"];
            Cl_Invocie cl_1 = new Cl_Invocie();
            DataTable dt_invad = Invoice_AdjustmentNumber(clvar);
            string Number = "", Number_ = "";
            if (dt_invad.Rows.Count != 0)
            {
                Number_ = dt_invad.Rows[0]["codeValue"].ToString();
                clvar.RefNo = Number_;
                Number = (double.Parse(Number_) + +1).ToString();
                clvar.RefNo = Number;
                updateInvoice_AdjustmentNumber(clvar);
            }
            clvar.InvoiceNo = txt_invoiceNo.Text;
            clvar.TotalAmount = double.Parse(lbl_TotalAmount.Text);
            clvar.gst = double.Parse(lbl_TotalGst.Text);
            if (lbl_discountdocument.Text == "")
            {
                lbl_discountdocument.Text = "0";
            }
            clvar.Insurance = (double.Parse(lbl_discountdocument.Text) + double.Parse(lbl_domesticDiscount2.Text)).ToString();
            clvar.docPouchNo = rb_1.SelectedValue;
            clvar.Flyer = lbl_FuelSurcharge.Text;
            clvar.expressionMessage = lbl_FuelSurchargeGst.Text;
            clvar.RefNo = Number_;
            clvar.RemarksID = dd_Reason.SelectedValue;
            Insert_InvoiceNote(clvar);

            // Invoice Consignment 
            DataTable dt = ViewState["InvoiceConsingnment"] as DataTable;
            DataTable invoiceConsignment = dt.Copy();
            dt.Clear();

            foreach (GridViewRow row in gv_cns.Rows)
            {
                string consignmentNumber = row.Cells[2].Text;
                string invoiceNumber = row.Cells[2].Text;
                string bookingDate = DateTime.Parse(row.Cells[0].Text).ToString("yyyy-MM-dd");
                string accountNo = (row.FindControl("txt_Accountno") as TextBox).Text;
                string origin = (row.FindControl("dd_origin") as DropDownList).SelectedValue;
                string destination = (row.FindControl("dd_Destination") as DropDownList).SelectedValue;
                string serviceType = (row.FindControl("dd_ServiceType") as DropDownList).SelectedValue;
                string weight = (row.FindControl("txt_Weight") as TextBox).Text;
                string consignmentType = (row.FindControl("dd_ConsignmentType") as DropDownList).SelectedValue;
                string amount = row.Cells[9].Text;
                string gst = row.Cells[10].Text;
                string newAmount = (row.FindControl("txt_Amount") as Label).Text;
                string newGst = (row.FindControl("txt_Gst") as Label).Text;
                string PriceModifier = (row.FindControl("Dd_priceModifier") as DropDownList).SelectedValue;
                string CM_Value = (row.FindControl("txt_CM_Amount") as TextBox).Text;
                string CM_Gst = (row.FindControl("txt_CM_Gst") as TextBox).Text;


                DataRow dr = dt.NewRow();
                DataRow[] existingRows = invoiceConsignment.Select("ConsignmentNumber = '" + consignmentNumber + "'");
                dr.ItemArray = existingRows[0].ItemArray;
                dr["BookingDate"] = bookingDate.ToString();
                dr["Account_No"] = accountNo.ToString();
                dr["origin"] = origin.ToString();
                dr["destination"] = destination.ToString();
                dr["Service_Type"] = serviceType.ToString();
                dr["ConsignmentType_ID"] = consignmentType.ToString();
                dr["weight"] = weight.ToString();
                dr["totalAmount"] = amount.ToString();
                dr["gst"] = gst.ToString();
                dr["NewAmount"] = newAmount.ToString();
                dr["NewGST"] = newGst.ToString();
                dr["PriceModifierid"] = PriceModifier.ToString();
                dr["CM_Value"] = CM_Value.ToString();
                dr["CM_Gst"] = CM_Gst.ToString();

                dt.Rows.Add(dr);
            }
            string error_message = SaveTransaction(dt, Number_);
            if (error_message.ToUpper() == "OK")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Transaction Saved Successfully')", true);

                if (rb_1.SelectedValue == "C")
                {
                    string script_ = @"window.open(""{0}"", ""{1}"", ""{2}"");";
                    //string script = String.Format(script_, "RunsheetInvoice.aspx?Xcode=" + temp_, "_blank", "");
                    string script = String.Format(script_, "CreditNote_Report.aspx?XCODE=" + Number_, "_blank", "");
                    ScriptManager.RegisterClientScriptBlock(this.Page, typeof(Page), "Redirect", script, true);
                }
                else
                {
                    string script_ = @"window.open(""{0}"", ""{1}"", ""{2}"");";
                    //string script = String.Format(script_, "RunsheetInvoice.aspx?Xcode=" + temp_, "_blank", "");
                    string script = String.Format(script_, "DebitNote_Report.aspx?XCODE=" + Number_, "_blank", "");
                    ScriptManager.RegisterClientScriptBlock(this.Page, typeof(Page), "Redirect", script, true);
                }

                //Clearing the text
                txt_invoiceNo.Text = "";
                txt_ClientName.Text = "";
                txt_EndDate.Text = "";
                txt_InvoiceDate.Text = "";
                txt_AC_Info.Text = "";
                txt_StartDate.Text = "";
                TextBox1.Text = "";

                // Now adding in General Voucher
                // Now Refreshing all the controls

                lbl_InvoiceNo.Text = "";
                txt_AC_Info.Text = "";
                txt_ClientName.Text = "";
                txt_EndDate.Text = "";
                txt_StartDate.Text = "";

                lbl_Base.Text = "";
                lbl_ModifiedValue.Text = "";
                lbl_DiscountOnDomestic.Text = "";
                lbl_DomesticDocument.Text = "";
                lbl_SampleDocument.Text = "";

                lbl_InvoiceAmount.Text = "";
                lbl_IcnAmount.Text = "";
                lbl_FuelSurcharge.Text = "";
                lbl_domesticDiscount2.Text = "";
                lbl_discountdocument.Text = "";
                lbl_discountdocument.Text = "";

                lbl_gst.Text = "";

                lbl_FuelSurchargeGst.Text = "";
                this.lbl_IcnGst.Text = "";

                this.lbl_TotalAmount.Text = "";
                this.lbl_TotalGst.Text = "";

                ViewState["InvoiceConsingnment"] = null;
                gv_cns.DataSource = null;
                gv_cns.DataBind();
                return;
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert(' Transaction Could Not Be Saved. \r\n Error: " + error_message + "')", true);
                return;
            }
        }

        protected string SaveTransaction(DataTable dt, string number)
        {
            List<string> queries = new List<string>();
            foreach (DataRow dr in dt.Rows)
            {
                string query = "insert into invoiceconsignment_note ( \n" +
                                "ConisgnmentDate, \n" +
                                "InvoiceNumber,  \n" +
                                "ConsignmentNumber,  \n" +
                                "Account_No,  \n" +
                                "Origin,  \n" +
                                "Destination,  \n" +
                                "Service_Type,  \n" +
                                "consignmentType_ID,  \n" +
                                "Weight,  \n" +
                                "TotalAmount,  \n" +
                                "GST,  \n" +
                                "CreatedBy,  \n" +
                                "CreatedOn,  \n" +
                                "NewAmount,  \n" +
                                "NewGst,Note_number,Note, PriceModifier, CM_Value, CM_gst \n"
                                + ")\n" +
                                " VALUES\n" +
                                "(\n" +
                                "'" + dr["BookingDate"].ToString() + "',\n" +
                                "'" + dr["InvoiceNumber"].ToString() + "',\n" +
                                "'" + dr["ConsignmentNumber"].ToString() + "',\n" +
                                "'" + dr["Account_No"].ToString() + "',\n" +
                                "'" + dr["Origin"].ToString() + "',\n" +
                                "'" + dr["Destination"].ToString() + "',\n" +
                                "'" + dr["Service_Type"].ToString() + "',\n" +
                                "'" + dr["ConsignmentType_id"].ToString() + "',\n" +
                                "'" + dr["Weight"].ToString() + "',\n" +
                                "'" + dr["TotalAmount"].ToString() + "',\n" +
                                "'" + dr["Gst"].ToString() + "',\n" +
                                "'" + HttpContext.Current.Session["U_ID"].ToString() + "',\n" +
                                "GETDATE(),\n" +
                                "'" + dr["NewAmount"].ToString() + "',\n" +
                                "'" + dr["NewGST"].ToString() + "', '" + number + "','" + rb_1.SelectedValue + "',\n" +
                                "'" + dr["PriceModifierid"].ToString() + "',\n" +
                                "'" + dr["CM_value"].ToString() + "',\n" +
                                "'" + dr["CM_GST"].ToString() + "'\n" +
                                ")";
                queries.Add(query);
            }

            SqlConnection con = new SqlConnection(clvar.Strcon());
            SqlTransaction tran;
            con.Open();
            tran = con.BeginTransaction();

            try
            {

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.Transaction = tran;
                cmd.CommandType = CommandType.Text;
                foreach (string str in queries)
                {
                    cmd.CommandText = str;
                    cmd.ExecuteNonQuery();
                }

                tran.Commit();
            }
            catch (Exception ex)
            {
                con.Close();

                tran.Rollback();
                //Deleting from Consignment 
                try
                {
                    string query_ = "Delete from Invoice_Note where InvoiceNumber='" + txt_invoiceNo.Text + "' and Note_Number='" + number + "'";
                    SqlCommand cmd = new SqlCommand(query_, con);
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
                catch (Exception Err)
                { }


                return ex.Message;
            }
            finally
            { con.Close(); }
            return "OK";
        }

        public DataTable Get_CustomerHeader(Cl_Variables clvar)
        {

            string sqlString = "selecT cc.branchCode, cc.accountNo, cc.name, isnull(cpa.calculationBase,0) calculationBase, isnull(cpa.modifiedCalculationValue,0) modifiedCalculationValue, isnull(cc.DIscountOnDomestic,0) DIscountOnDomestic,isnull(cc.discountOnSample,0) discountOnSample, isnull(cc.discountOnDocument,0) discountOnDocument  \n"
               + "  From CreditClients cc \n"
               + " Left OUTER JOIN ClientPriceModifierAssociation Cpa \n"
               + " ON cpa.creditClientId = cc.id"
               + " where cc.accountNo = '" + clvar.AccountNo + "' and cc.branchCode ='" + HttpContext.Current.Session["BRANCHCODE"].ToString() + "'";

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

        protected void txt_cnNumber_TextChanged(object sender, EventArgs e)
        {
            TextBox cb = (TextBox)sender;
            foreach (GridViewRow gr in gv_cns.Rows)
            {
                if (cb.ClientID == ((TextBox)gr.FindControl("txt_Accountno")).ClientID.ToString())
                {
                    string AccountNo = cb.Text;
                    clvar.AccountNo = AccountNo;
                    DataTable header = Get_CustomerHeader(clvar);
                    if (header.Rows.Count != 0)
                    {
                        txt_AC_Info.Text = header.Rows[0]["AccountNo"].ToString();
                        txt_ClientName.Text = header.Rows[0]["name"].ToString();
                        if (header.Rows[0]["calculationBase"].ToString() == "1")
                        {
                            lbl_Base.Text = "Value";
                        }
                        else if (header.Rows[0]["calculationBase"].ToString() == "2")
                        {
                            lbl_Base.Text = "Percentage";
                        }
                        lbl_ModifiedValue.Text = header.Rows[0]["modifiedCalculationValue"].ToString();
                        lbl_DiscountOnDomestic.Text = header.Rows[0]["DIscountOnDomestic"].ToString();
                        lbl_DomesticDocument.Text = header.Rows[0]["discountOnDocument"].ToString();
                        lbl_SampleDocument.Text = header.Rows[0]["discountOnSample"].ToString();
                        ViewState["BranchCode"] = header.Rows[0]["branchCode"].ToString();

                    }

                }
            }
        }

        protected void gv_cns_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        public DataTable GetAccountDetailByAccountNumber_(Cl_Variables clvar)
        {

            string query = "SELECT   * FROM  CreditClients cc \n"
               + "   \n"
               + " Left OUTER JOIN ClientPriceModifierAssociation Cpa \n"
               + " ON cpa.creditClientId = cc.id \n"
               + " where cc.ACCOUNTNO = '" + clvar.AccountNo + "' and cc.branchcode = '" + clvar.Branch + "'";

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

        protected void txt_AccountNo_TextChanged(object sender, EventArgs e)
        {
            clvar = new Cl_Variables();
            clvar.AccountNo = txt_AccountNo.Text;
            clvar.Branch = ViewState["BranchCode"].ToString();

            DataTable dt = GetAccountDetailByAccountNumber_(clvar);
            if (dt.Rows.Count != 0)
            {
                txt_AC_Info.Text = dt.Rows[0]["AccountNo"].ToString();
                txt_ClientName.Text = dt.Rows[0]["name"].ToString();
                if (dt.Rows[0]["calculationBase"].ToString() == "1")
                {
                    lbl_Base.Text = "Value";
                }
                else if (dt.Rows[0]["calculationBase"].ToString() == "2")
                {
                    lbl_Base.Text = "Percentage";
                }
                lbl_ModifiedValue.Text = dt.Rows[0]["modifiedCalculationValue"].ToString();
                if (dt.Rows[0]["DIscountOnDomestic"].ToString() != "")
                {
                    lbl_DiscountOnDomestic.Text = dt.Rows[0]["DIscountOnDomestic"].ToString();
                }
                else
                {
                    lbl_DiscountOnDomestic.Text = "0";// dt.Rows[0]["DIscountOnDomestic"].ToString();
                }

                if (dt.Rows[0]["discountOnDocument"].ToString() != "")
                {
                    lbl_DomesticDocument.Text = dt.Rows[0]["discountOnDocument"].ToString();
                }
                else
                {
                    lbl_DomesticDocument.Text = "0";
                }

                if (dt.Rows[0]["discountOnSample"].ToString() != "")
                {
                    lbl_SampleDocument.Text = dt.Rows[0]["discountOnSample"].ToString();
                }
                else
                {
                    lbl_SampleDocument.Text = "0";
                }

                DataTable dt_1 = (DataTable)ViewState["InvoiceConsingnment"];
                foreach (GridViewRow gr in gv_cns.Rows)
                {
                    (gr.FindControl("txt_Accountno") as TextBox).Text = txt_AccountNo.Text;
                    string consignmnet_ = (gr.FindControl("hd_Consignment") as HiddenField).Value;
                    DataRow[] dr_ = dt_1.Select("consignmentNumber=" + consignmnet_);
                    if (dr_.Length != 0)
                    {
                        dr_[0]["Account_no"] = txt_AccountNo.Text;

                    }


                }
                dt_1.AcceptChanges();
                ViewState["InvoiceConsingnment"] = dt_1;
                gv_cns.DataSource = dt_1.DefaultView;
                gv_cns.DataBind();

            }
            else
            {

            }

        }

        protected void rb_1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rb_1.SelectedValue == "C")
            {
                //txt_AccountNo.Enabled = false;
                //txt_AccountNo.Text = "";
            }
            else
            {
                //txt_AccountNo.Enabled = true;
                //txt_AccountNo.Text = "";
            }
        }

        public DataSet BranchGSTInformation(Cl_Variables clvar)
        {
            DataSet Ds_1 = new DataSet();

            try
            {
                string query = "SELECT * \n"
               + "FROM   BranchGST bg \n"
               + "WHERE  bg.branchCode = '" + clvar.origin + "' \n"
               + "       AND bg.createdOn = ( \n"
               + "               SELECT MAX(bg2.createdOn) \n"
               + "               FROM   BranchGST bg2 \n"
               + "               WHERE  bg2.branchCode = '" + clvar.origin + "' \n"
               + "           )";

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

        protected void Dd_priceModifier_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList dr = (DropDownList)sender;

            clvar.origin = ViewState["BranchCode"].ToString();
            // DataTable BranchGst = func.GetBranchGst(clvar);
            DataSet BranchGst = BranchGSTInformation(clvar);

            foreach (GridViewRow GridR in gv_cns.Rows)
            {
                if (dr.ClientID == (GridR.FindControl("Dd_priceModifier") as DropDownList).ClientID)
                {
                    if ((GridR.FindControl("txt_Amount") as Label).Text == "0" && (GridR.FindControl("txt_Gst") as Label).Text == "0")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('Recompute the Price')", true);

                        (GridR.FindControl("Dd_priceModifier") as DropDownList).SelectedValue = "0";
                    }
                    else
                    {
                        string Extracharges = (GridR.FindControl("Dd_priceModifier") as DropDownList).SelectedValue;
                        DataTable dt_PM = (DataTable)ViewState["PriceModifier"];
                        if (dt_PM.Rows.Count != 0)
                        {
                            DataRow[] dr_ = dt_PM.Select("id='" + Extracharges + "'");
                            if (dr_.Length != 0)
                            {
                                if (dr_[0]["calculationBase"].ToString() == "1")
                                {
                                    (GridR.FindControl("txt_CM_Amount") as TextBox).Text = dr_[0]["CalculationValue"].ToString();
                                    double gst = (double.Parse(dr_[0]["CalculationValue"].ToString()) / (100 + double.Parse(BranchGst.Tables[0].Rows[0]["gst"].ToString()))) * double.Parse(BranchGst.Tables[0].Rows[0]["gst"].ToString());

                                    (GridR.FindControl("txt_CM_Gst") as TextBox).Text = Math.Round(gst, 2).ToString();// dr_[0]["calculationBase"].ToString();
                                    (GridR.FindControl("txt_CM_Amount") as TextBox).Enabled = true;
                                    //  (GridR.FindControl("txt_CM_Gst") as TextBox).Enabled = true;

                                }
                                else if (dr_[0]["calculationBase"].ToString() == "2")
                                {
                                    string totalAmount = (GridR.FindControl("txt_Amount") as Label).Text;
                                    double A = (double.Parse(totalAmount) / (100 + double.Parse(dr_[0]["CalculationValue"].ToString()))) * double.Parse(dr_[0]["CalculationValue"].ToString());

                                    (GridR.FindControl("txt_CM_Amount") as TextBox).Text = Math.Round(A, 2).ToString(); //dr_[0]["CalculationValue"].ToString();
                                    double gst = (A / (100 + double.Parse(BranchGst.Tables[0].Rows[0]["gst"].ToString()))) * double.Parse(BranchGst.Tables[0].Rows[0]["gst"].ToString());

                                    (GridR.FindControl("txt_CM_Gst") as TextBox).Text = Math.Round(gst, 2).ToString();// dr_[0]["calculationBase"].ToString();
                                    (GridR.FindControl("txt_CM_Amount") as TextBox).Enabled = false;
                                    (GridR.FindControl("txt_CM_Gst") as TextBox).Enabled = false;

                                }
                            }
                        }

                    }
                    GetDetail();
                }
            }
        }

        protected void txt_CM_Amount_TextChanged(object sender, EventArgs e)
        {
            TextBox dr = (TextBox)sender;

            clvar.origin = ViewState["BranchCode"].ToString();
            // DataTable BranchGst = func.GetBranchGst(clvar);
            DataSet BranchGst = BranchGSTInformation(clvar);

            foreach (GridViewRow GridR in gv_cns.Rows)
            {
                if (dr.ClientID == (GridR.FindControl("txt_CM_Amount") as TextBox).ClientID)
                {
                    if ((GridR.FindControl("txt_Amount") as Label).Text == "0" && (GridR.FindControl("txt_Gst") as Label).Text == "0")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('Recompute the Price')", true);

                        (GridR.FindControl("Dd_priceModifier") as DropDownList).SelectedValue = "0";
                    }
                    else
                    {
                        string Extracharges = (GridR.FindControl("Dd_priceModifier") as DropDownList).SelectedValue;
                        DataTable dt_PM = (DataTable)ViewState["PriceModifier"];
                        if (dt_PM.Rows.Count != 0)
                        {
                            DataRow[] dr_ = dt_PM.Select("id='" + Extracharges + "'");
                            if (dr_.Length != 0)
                            {
                                if (dr_[0]["calculationBase"].ToString() == "1")
                                {
                                    // (GridR.FindControl("txt_CM_Amount") as TextBox).Text = dr_[0]["CalculationValue"].ToString();
                                    double gst = ((double.Parse((GridR.FindControl("txt_CM_Amount") as TextBox).Text)) / (100 + double.Parse(BranchGst.Tables[0].Rows[0]["gst"].ToString()))) * double.Parse(BranchGst.Tables[0].Rows[0]["gst"].ToString());

                                    (GridR.FindControl("txt_CM_Gst") as TextBox).Text = Math.Round(gst, 2).ToString();// dr_[0]["calculationBase"].ToString();
                                }
                                else if (dr_[0]["calculationBase"].ToString() == "2")
                                {
                                    string totalAmount = (GridR.FindControl("txt_Amount") as Label).Text;
                                    double A = (double.Parse(totalAmount) / (100 + double.Parse(dr_[0]["CalculationValue"].ToString()))) * double.Parse(dr_[0]["CalculationValue"].ToString());

                                    (GridR.FindControl("txt_CM_Amount") as TextBox).Text = Math.Round(A, 2).ToString(); //dr_[0]["CalculationValue"].ToString();
                                    double gst = (A / (100 + double.Parse(BranchGst.Tables[0].Rows[0]["gst"].ToString()))) * double.Parse(BranchGst.Tables[0].Rows[0]["gst"].ToString());

                                    (GridR.FindControl("txt_CM_Gst") as TextBox).Text = Math.Round(gst, 2).ToString();// dr_[0]["calculationBase"].ToString();
                                }
                            }
                        }

                    }
                }
            }
            GetDetail();

        }
    }
}