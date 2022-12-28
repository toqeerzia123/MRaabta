using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using MRaabta.App_Code;

namespace MRaabta.Files
{
    public partial class MasterArrivalScan : System.Web.UI.Page
    {
        Variable clvar = new Variable();
        bayer_Function b_fun = new bayer_Function();
        DataSet ds_grid = new DataSet();
        cl_Encryption enc = new cl_Encryption();
        CommonFunction CF = new CommonFunction();
        Cl_Variables clvar2 = new Cl_Variables();

        string zone, sts, chksts, id, ConsignmentTypeName, Expresscentercode;

        protected void Page_Load_(object sender, EventArgs e)
        {
            txt_ridername.Enabled = false;

            if (!IsPostBack)
            {
                txt_ridername.Enabled = false;
                Get_ServiceType();
                Get_MasterConsignmentType();

                DataTable dt_head = new DataTable();
                dt_head.Columns.Add("RiderCode", typeof(string));
                dt_head.Columns.Add("RiderName", typeof(string));
                dt_head.Columns.Add("OrignCode", typeof(string));
                dt_head.Columns.Add("BranchCode", typeof(string));
                dt_head.Columns.Add("Weight", typeof(string));
                dt_head.Columns.Add("ServiceType", typeof(string));
                dt_head.Columns.Add("ConsignmentType", typeof(string));
                dt_head.Columns.Add("ConsignmentNo", typeof(string));
                dt_head.Columns.Add("ConsignmentTypeName", typeof(string));
                dt_head.Columns.Add("OriginExpressCenterCode", typeof(string));
                dt_head.Columns.Add("Pieces", typeof(string));
                DataTable dt_head_ = new DataTable();
                dt_head_.Columns.Add("ArrivalID", typeof(Int64));
                dt_head_.Columns.Add("consignmentNumber", typeof(string));
                dt_head_.Columns.Add("CreatedOn", typeof(DateTime));
                dt_head_.Columns.Add("CreatedBy", typeof(string));

                //Consignment Tracking

                DataTable dt_ConsignmentTracking = new DataTable();
                dt_ConsignmentTracking.Columns.Add("consignmentNumber", typeof(string));
                dt_ConsignmentTracking.Columns.Add("stateID", typeof(int));
                dt_ConsignmentTracking.Columns.Add("currentLocation", typeof(string));
                dt_ConsignmentTracking.Columns.Add("manifestNumber", typeof(string));
                dt_ConsignmentTracking.Columns.Add("bagNumber", typeof(string));
                dt_ConsignmentTracking.Columns.Add("loadingNumber", typeof(string));
                dt_ConsignmentTracking.Columns.Add("mawbNumber", typeof(string));
                dt_ConsignmentTracking.Columns.Add("runsheetNumber", typeof(string));
                dt_ConsignmentTracking.Columns.Add("riderName", typeof(string));
                dt_ConsignmentTracking.Columns.Add("transactionTime", typeof(DateTime));
                dt_ConsignmentTracking.Columns.Add("reason", typeof(string));
                dt_ConsignmentTracking.Columns.Add("statusTime", typeof(DateTime));
                dt_ConsignmentTracking.Columns.Add("mappingNo", typeof(string));
                dt_ConsignmentTracking.Columns.Add("isDeleted", typeof(string));
                dt_ConsignmentTracking.Columns.Add("internationalRemarks", typeof(string));
                dt_ConsignmentTracking.Columns.Add("SealNo", typeof(string));
                dt_ConsignmentTracking.Columns.Add("VanNo", typeof(string));
                dt_ConsignmentTracking.Columns.Add("MaterialArrival", typeof(string));
                dt_ConsignmentTracking.Columns.Add("ArrivalID", typeof(string));

                //Consignment
                DataTable dt_Consignment = new DataTable();
                dt_Consignment.Columns.Add("consignmentNumber", typeof(string));
                dt_Consignment.Columns.Add("serviceTypeName", typeof(string));
                dt_Consignment.Columns.Add("consignmentTypeId", typeof(string));
                dt_Consignment.Columns.Add("WEIGHT", typeof(string));
                dt_Consignment.Columns.Add("orgin", typeof(string));
                dt_Consignment.Columns.Add("destination", typeof(string));
                dt_Consignment.Columns.Add("createdon", typeof(DateTime));
                dt_Consignment.Columns.Add("createdby", typeof(string));
                dt_Consignment.Columns.Add("pieces", typeof(string));
                dt_Consignment.Columns.Add("creditClientId", typeof(string));
                dt_Consignment.Columns.Add("zoneCode", typeof(string));
                dt_Consignment.Columns.Add("branchCode", typeof(string));
                dt_Consignment.Columns.Add("consignerAccountNo", typeof(string));
                dt_Consignment.Columns.Add("bookingDate", typeof(DateTime));
                dt_Consignment.Columns.Add("RiderCode", typeof(string));


                ViewState["dthead"] = dt_head;
                ViewState["dthead_"] = dt_head_;
                ViewState["dtConsignmentTracking"] = dt_ConsignmentTracking;
                ViewState["dtConsignment"] = dt_Consignment;

                GetCNLengths();

            }
            SetGridTextFields();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (rbtn_mode.SelectedValue == "1")
            {
                txt_arrivalID.Enabled = true;
            }
            else
            {
                txt_arrivalID.Enabled = false;
            }
            error_msg.Text = "";
            txt_ridername.Enabled = false;

            if (!IsPostBack)
            {
                txt_ridername.Enabled = false;
                Get_ServiceType();
                Get_MasterConsignmentType();


                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[] {
            new DataColumn("ConsignmentNumber", typeof(string)),
            new DataColumn("ServiceTypeName", typeof(string)),
            new DataColumn("ConsignmentTypeID", typeof(string)),
            new DataColumn("WEIGHT", typeof(string)),
            new DataColumn("Pieces", typeof(string)),
            new DataColumn("Origin", typeof(string)),
            new DataColumn("Destination", typeof(string)),
            new DataColumn("CreditClientID", typeof(Int64)),
            new DataColumn("ZoneCode", typeof(string)),
            new DataColumn("BranchCode", typeof(string)),
            new DataColumn("ConsignerAccountNo", typeof(string)),
            new DataColumn("RiderCode", typeof(string)),
            new DataColumn("CnInsert", typeof(bool)),
            new DataColumn("isNew", typeof(bool)),
            new DataColumn("Arrived")
            });
                ViewState["tbl_detail"] = dt;
                GetCNLengths();

            }
            SetGridTextFields();
        }

        public void SetGridTextFields()
        {
            DataTable dt = ViewState["tbl_detail"] as DataTable;

            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {


                    foreach (GridViewRow row in GridView.Rows)
                    {
                        string cn = row.Cells[1].Text;
                        DataRow[] dr = dt.Select("ConsignmentNumber = '" + cn + "'");
                        if (dr.Count() > 0)
                        {
                            dr[0]["Weight"] = (row.FindControl("txt_gWeight") as TextBox).Text;
                            dr[0]["Pieces"] = (row.FindControl("txt_gPieces") as TextBox).Text;


                        }
                    }

                    ViewState["tbl_detail"] = dt;

                }
            }
        }

        public void GetCNLengths()
        {
            string query = "SELECT * FROM MNP_ConsignmentLengths where status = '1'";
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                sda.Fill(dt); ;
                ViewState["cnLengths"] = dt;
            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            cnControls.DataSource = dt;
            cnControls.DataBind();
        }
        public void Get_ServiceType()
        {
            DataSet ds_servicetype = b_fun.Get_ServiceType(clvar);

            if (ds_servicetype.Tables[0].Rows.Count != 0)
            {
                dd_servicetype.DataTextField = "serviceTypeName";
                dd_servicetype.DataValueField = "serviceTypeName";
                dd_servicetype.DataSource = ds_servicetype.Tables[0].DefaultView;
                dd_servicetype.DataBind();
            }
            // dd_servicetype.Items.Insert(0, new ListItem("overnight"));
            dd_servicetype.SelectedValue = "overnight";
        }

        public void Get_MasterConsignmentType()
        {
            DataSet ds_contype = b_fun.Get_MasterConsignmentType(clvar);

            if (ds_contype.Tables[0].Rows.Count != 0)
            {
                dd_contype.DataTextField = "name";
                dd_contype.DataValueField = "id";
                dd_contype.DataSource = ds_contype.Tables[0].DefaultView;
                dd_contype.DataBind();
            }
            dd_contype.SelectedValue = "12";
        }

        protected void txt_ridercode_TextChanged(object sender, EventArgs e)
        {
            txt_ridername.Enabled = false;
            clvar._RiderCode = txt_ridercode.Text;
            clvar._Zone = Session["ZONECODE"].ToString();
            clvar._TownCode = Session["BRANCHCODE"].ToString();

            DataSet ds = b_fun.Get_RidersName(clvar);
            if (ds.Tables[0].Rows.Count != 0)
            {
                txt_ridername.Text = ds.Tables[0].Rows[0]["ridername"].ToString();
                hd_ExpressCenter.Value = ds.Tables[0].Rows[0]["ExpressCenterID"].ToString();

                DataTable detail = ViewState["tbl_detail"] as DataTable;

                if (detail.Rows.Count > 0)
                {
                    foreach (DataRow dr in detail.Rows)
                    {
                        dr["RiderCode"] = txt_ridercode.Text;
                    }
                }

                GridView.DataSource = detail;
                GridView.DataBind();
                ViewState["tbl_detail"] = detail;
                txt_weight.Focus();
            }
        }

        protected void txt_consignment_TextChanged_(object sender, EventArgs e)
        {
            clvar.ConsignmentNo = txt_consignment.Text;
            DataTable cnLength = ViewState["cnLengths"] as DataTable;
            bool prefixFound = false;
            if (cnLength != null)
            {
                if (cnLength.Rows.Count > 0)
                {
                    foreach (DataRow d in cnLength.Rows)
                    {
                        if (d[1].ToString().Length > txt_consignment.Text.Length)
                        {
                            continue;
                        }
                        if (d[1].ToString() == txt_consignment.Text.Substring(0, d[1].ToString().Length))
                        {
                            if (d[3].ToString() == txt_consignment.Text.Length.ToString())
                            {
                                prefixFound = true;
                                break;
                            }
                        }
                    }
                }
            }
            if (!prefixFound)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Invalid Consignment Lengnth or Prefix.')", true);
                error_msg.Text = "Invalid Consignment Lengnth or Prefix.";
                return;
            }
            if (!IsNumeric(clvar.ConsignmentNo))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Only Numerics allowed in Consignment Number')", true);
                return;
            }
            if (/*txt_consignment.Text.Length <= 15*/ true)
            {
                if (/*txt_consignment.Text.Length >= 11*/ true)
                {
                    txt_ridername.Enabled = false;
                    clvar._Zone = Session["ZONECODE"].ToString();
                    clvar._TownCode = Session["BRANCHCODE"].ToString();
                    clvar._ConsignmentNo = txt_consignment.Text;

                    DataSet ds1 = Get_ConsignmentNumber(clvar);

                    if (ds1.Tables[0].Rows.Count != 0)
                    {
                        if (/*txt_consignment.Text.Length.ToString() == "12" || txt_consignment.Text.Length.ToString() == "11"*/ true)
                        {
                            //  txt_ridercode.Text = ds1.Tables[0].Rows[0]["riderCode"].ToString();
                            //txt_weight.Text = //ds1.Tables[0].Rows[0]["weight"].ToString();
                            //clvar._Orign = ds1.Tables[0].Rows[0]["originExpressCenter"].ToString();
                            if (txt_ridercode.Text != "" && txt_ridername.Text != "" && txt_consignment.Text != "")
                            {
                                DataTable dt_head = ViewState["dthead"] as DataTable;
                                DataTable dt_head_ = ViewState["dthead_"] as DataTable;
                                DataTable dtConsignmentTracking = ViewState["dtConsignmentTracking"] as DataTable;

                                DataRow[] foundRows = dt_head.Select("ConsignmentNo ='" + clvar._ConsignmentNo + "' ");
                                if (foundRows.Length == 0)
                                {
                                    int numberOfRecords = dt_head.Select().Length + 1;
                                    lbl_count.Text = "Count: " + numberOfRecords.ToString();

                                    DataTable dt = new DataTable();
                                    //dt_head.Rows.Add(txt_ridercode.Text, txt_ridername.Text, clvar._Zone, clvar._TownCode, txt_weight.Text, dd_servicetype.SelectedValue, dd_contype.SelectedValue, txt_consignment.Text, dd_contype.SelectedItem, clvar._Orign);
                                    dt = dt_head.Clone();
                                    dt.Clear();
                                    dt.Rows.Add(txt_ridercode.Text, txt_ridername.Text, clvar._Zone, clvar._TownCode, txt_weight.Text, dd_servicetype.SelectedValue, dd_contype.SelectedValue, txt_consignment.Text, dd_contype.SelectedItem, clvar._Orign, "1");
                                    dt.Merge(dt_head);
                                    dt_head.Clear();
                                    dt_head = dt;

                                    dt.Dispose();
                                    dt = dt_head_.Clone();
                                    dt.Clear();
                                    dt.Rows.Add(0, clvar._ConsignmentNo, DateTime.Now, HttpContext.Current.Session["U_ID"].ToString());
                                    dt.Merge(dt_head_);
                                    dt_head_.Clear();
                                    dt_head_ = dt;



                                    dt.Dispose();
                                    dt = dtConsignmentTracking.Clone();
                                    dt.Clear();
                                    dt.Rows.Add(clvar._ConsignmentNo, "18", b_fun.Get_Branches(clvar).Tables[0].Rows[0]["name"].ToString(), "", "", "", "", "", "", DateTime.Now, "", DateTime.Now, "", "0", "", "", "", "", "");
                                    dt.Merge(dtConsignmentTracking);
                                    dtConsignmentTracking.Clear();
                                    dtConsignmentTracking = dt;
                                    //dtConsignmentTracking.Rows.Add(clvar._ConsignmentNo, "18", b_fun.Get_Branches(clvar).Tables[0].Rows[0]["name"].ToString(), "", "", "", "", "", "", DateTime.Now, "", DateTime.Now, "", "0", "", "", "", "", "");

                                    dt_head.AcceptChanges();
                                    dt_head_.AcceptChanges();
                                    dtConsignmentTracking.AcceptChanges();

                                    ViewState["dthead"] = dt_head;
                                    ViewState["dthead_"] = dt_head_;
                                    ViewState["dtConsignmentTracking"] = dtConsignmentTracking;

                                    GridView.DataSource = dt_head;
                                    GridView.DataBind();


                                    //txt_ridercode.Enabled = false;
                                    txt_ridername.Enabled = false;
                                    txt_consignment.Text = "";
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "showError",
                                    "alert('Consignment Already in List');", true);

                                    txt_consignment.Text = "";
                                    //txt_ridercode.Text = "";
                                    // txt_ridername.Text = "";
                                    // txt_weight.Text = "";
                                }

                            }
                            else
                            {
                                new_Consignment();
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "showError",
                                        "alert('Invalid Consignment Number...');", true);
                            txt_consignment.Text = "";
                        }

                    }
                    else
                    {
                        new_Consignment();
                    }

                    txt_consignment.Focus();
                }
                else
                {
                    txt_consignment.Text = "";
                }
            }
            else
            {
                txt_consignment.Text = "";
            }
        }
        protected void txt_consignment_TextChanged(object sender, EventArgs e)
        {
            clvar.ConsignmentNo = txt_consignment.Text;
            #region Validations
            DataTable cnLength = ViewState["cnLengths"] as DataTable;
            bool prefixFound = false;
            if (cnLength != null)
            {
                if (cnLength.Rows.Count > 0)
                {
                    foreach (DataRow d in cnLength.Rows)
                    {
                        if (d[1].ToString().Length > txt_consignment.Text.Length)
                        {
                            continue;
                        }
                        if (d[1].ToString() == txt_consignment.Text.Substring(0, d[1].ToString().Length))
                        {
                            if (d[3].ToString() == txt_consignment.Text.Length.ToString())
                            {
                                prefixFound = true;
                                break;
                            }
                        }
                    }
                }
            }
            if (!prefixFound)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Invalid Consignment Lengnth or Prefix.')", true);
                error_msg.Text = "Invalid Consignment Lengnth or Prefix.";
                return;
            }
            if (!IsNumeric(clvar.ConsignmentNo))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Only Numerics allowed in Consignment Number')", true);
                return;
            }

            #endregion

            txt_ridername.Enabled = false;

            DataTable detail = ViewState["tbl_detail"] as DataTable;
            if (detail != null)
            {
                if (detail.Rows.Count > 0)
                {
                    if (detail.Select("ConsignmentNumber = '" + txt_consignment.Text.Trim() + "'").Count() > 0)
                    {
                        AlertMessage("Consignment Already Scanned", "Red");
                        return;
                    }
                }
            }
            DataSet ds1 = Get_ConsignmentNumber(clvar);
            if (ds1.Tables.Count == 0)
            {
                AlertMessage("Something Went wrong. Please Contact IT Support", "Red");
                return;
            }

            if (ds1.Tables[0].Rows.Count != 0)
            {
                DataTable dt = ds1.Tables[0];
                DataRow dr = detail.NewRow();

                dr["ConsignmentNumber"] = dt.Rows[0]["ConsignmentNumber"].ToString();
                dr["ServiceTypeName"] = dt.Rows[0]["ServiceTypeName"].ToString();
                dr["ConsignmentTypeID"] = dt.Rows[0]["ConsignmentTypeID"].ToString();
                dr["WEIGHT"] = dt.Rows[0]["WEIGHT"].ToString();
                dr["Pieces"] = dt.Rows[0]["Pieces"].ToString();
                dr["Origin"] = dt.Rows[0]["Orgin"].ToString();
                dr["Destination"] = dt.Rows[0]["Destination"].ToString();
                dr["CreditClientID"] = dt.Rows[0]["CreditClientID"].ToString();
                dr["ZoneCode"] = dt.Rows[0]["ZoneCode"].ToString();
                dr["BranchCode"] = dt.Rows[0]["BranchCode"].ToString();
                dr["ConsignerAccountNo"] = dt.Rows[0]["ConsignerAccountNo"].ToString();
                dr["RiderCode"] = dt.Rows[0]["RiderCode"].ToString();
                dr["CnInsert"] = false;
                dr["isNew"] = true;
                dr["Arrived"] = "0";
                detail.Rows.InsertAt(dr, 0);
                //detail.Rows.Add(dr);
                detail.AcceptChanges();

                ViewState["tbl_detail"] = detail;

                GridView.DataSource = detail;
                GridView.DataBind();
                txt_consignment.Text = "";
            }
            else
            {
                DataRow dr = detail.NewRow();

                dr["ConsignmentNumber"] = txt_consignment.Text.Trim();
                dr["ServiceTypeName"] = dd_servicetype.SelectedValue;
                dr["ConsignmentTypeID"] = dd_contype.SelectedValue;
                dr["WEIGHT"] = "0.5";
                dr["Pieces"] = "1";
                dr["Origin"] = HttpContext.Current.Session["BranchCode"].ToString();
                dr["Destination"] = HttpContext.Current.Session["BranchCode"].ToString();
                dr["CreditClientID"] = "330140";
                dr["ZoneCode"] = HttpContext.Current.Session["ZoneCode"].ToString();
                dr["BranchCode"] = HttpContext.Current.Session["BranchCode"].ToString();
                dr["ConsignerAccountNo"] = "4D1";
                dr["RiderCode"] = txt_ridercode.Text.Trim();
                dr["CnInsert"] = true;
                dr["isNew"] = true;
                dr["Arrived"] = "0";
                detail.Rows.InsertAt(dr, 0);
                //detail.Rows.Add(dr);
                detail.AcceptChanges();

                ViewState["tbl_detail"] = detail;

                GridView.DataSource = detail;
                GridView.DataBind();
                txt_consignment.Text = "";

            }

            lbl_count.Text = "Total CNs: " + detail.Rows.Count.ToString();
            txt_consignment.Focus();


        }

        protected void Btn_Save_Click_(object sender, EventArgs e)
        {
            this.submit.Enabled = false;

            DataTable dt_head = ViewState["dthead"] as DataTable;
            DataTable dt_head_ = ViewState["dthead_"] as DataTable;
            DataTable dtConsignmentTracking = ViewState["dtConsignmentTracking"] as DataTable;
            DataTable dtConsignment = ViewState["dtConsignment"] as DataTable;
            Int64 ArrivalID = 0;
            string Arrival = "";
            if (dt_head.Rows.Count > 0)
            {
                clvar.RiderCode = dt_head.Rows[0]["RiderCode"].ToString();

                if (dt_head.Rows[0]["originExpressCenterCode"].ToString() != "")
                {
                    Expresscentercode = dt_head.Rows[0]["originExpressCenterCode"].ToString();
                }
                else
                {
                    Expresscentercode = hd_ExpressCenter.Value;
                }

                clvar.Expresscentercode = Expresscentercode;

                double totalweight = 0;

                for (int i = 0; i < dt_head.Rows.Count; i++)
                {
                    totalweight += Double.Parse(dt_head.Rows[0]["Weight"].ToString());
                }
                totalweight = Convert.ToDouble(totalweight.ToString("N3"));

                clvar._Weight = totalweight.ToString();
                clvar._StateId = "1";
                try
                {
                    ArrivalID = b_fun.Insert_ArrivalScan(clvar);
                    dt_head_.Columns["ArrivalID"].Expression = "'" + ArrivalID.ToString() + "'";
                    dtConsignmentTracking.Columns["ArrivalID"].Expression = "'" + ArrivalID.ToString() + "'";
                    Arrival = b_fun.BulkInsertTo_ArrivalDetail(dt_head_, dtConsignmentTracking, dtConsignment);
                }
                catch (Exception Err)
                {

                }

            }


            string temp_ = clvar.ArrivalID.ToString();
            if (Arrival == "1")
            {
                string script_ = @"window.open(""{0}"", ""{1}"", ""{2}"");";
                string script = String.Format(script_, "Arrival_print.aspx?Xcode=" + ArrivalID.ToString(), "_blank", "");
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Redirect", script, true);


                dt_head.Rows.Clear();
                dt_head.Columns.Clear();   //warning: All Columns delete
                dt_head_.Rows.Clear();
                dt_head_.Columns.Clear();
                dtConsignmentTracking.Rows.Clear();
                dtConsignmentTracking.Columns.Clear();
                dtConsignment.Rows.Clear();
                dtConsignment.Columns.Clear();
                dt_head.Dispose();
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "showError",
                                "alert('Arrival cannot be created');", true);

            }
            this.submit.Enabled = true;

            this.txt_ridercode.Text = "";
            this.txt_ridername.Text = "";
            this.txt_weight.Text = "";
            this.lbl_count.Text = "";
            this.txt_consignment.Focus();
            this.GridView.DataSource = null;
            this.GridView.DataBind();

            // Response.Redirect("Files/MasterArrivalScan.aspx");             
        }

        protected void Btn_Save_Click(object sender, EventArgs e)
        {
            #region Validations
            if (txt_ridercode.Text.Trim() == "")
            {
                AlertMessage("Enter Rider Code", "Red");
                return;
            }
            double tempWeight_ = 0f;



            #endregion
            if (rbtn_mode.SelectedValue == "0")
            {
                #region New Arrival

                clvar2.expresscenter = HttpContext.Current.Session["ExpressCenter"].ToString();
                clvar2.Branch = HttpContext.Current.Session["BranchCode"].ToString();
                clvar2.Zone = HttpContext.Current.Session["ZoneCode"].ToString();
                clvar2.createdBy = HttpContext.Current.Session["U_ID"].ToString();
                clvar2.riderCode = txt_ridercode.Text.Trim();
                clvar2.OriginExpressCenterCode = hd_ExpressCenter.Value;

                DataTable detail = ViewState["tbl_detail"] as DataTable;
                float tempWeight = 0f;
                if (detail.Rows.Count > 0)
                {
                    foreach (DataRow dr in detail.Rows)
                    {
                        float temp = 0f;
                        float.TryParse(dr["Weight"].ToString(), out temp);
                        tempWeight += temp;
                    }
                }

                clvar2.Weight = tempWeight;

                detail.Columns.Remove("Arrived");
                Tuple<bool, string> resp = CreateArrival(detail, clvar2);
                if (resp.Item1 == false)
                {
                    AlertMessage(resp.Item2, "Red");
                    return;
                }
                else
                {
                    string script_ = @"window.open(""{0}"", ""{1}"", ""{2}"");";
                    string script = String.Format(script_, "Arrival_print.aspx?Xcode=" + resp.Item2, "_blank", "");
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Redirect", script, true);
                    detail.Rows.Clear();
                    ViewState["tbl_detail"] = detail;
                    GridView.DataSource = detail;
                    GridView.DataBind();

                    txt_ridercode.Text = "";
                    txt_ridername.Text = "";
                    txt_weight.Text = "";
                    dd_contype.ClearSelection();
                    dd_servicetype.ClearSelection();
                }
                #endregion
            }
            else
            {
                #region Edit Arrival
                if (txt_arrivalID.Text.Trim() == "")
                {
                    AlertMessage("Enter Arrival ID", "Red");
                    return;
                }
                clvar2.CheckCondition = txt_arrivalID.Text;
                clvar2.expresscenter = HttpContext.Current.Session["ExpressCenter"].ToString();
                clvar2.Branch = HttpContext.Current.Session["BranchCode"].ToString();
                clvar2.Zone = HttpContext.Current.Session["ZoneCode"].ToString();
                clvar2.createdBy = HttpContext.Current.Session["U_ID"].ToString();
                clvar2.riderCode = txt_ridercode.Text.Trim();
                clvar2.OriginExpressCenterCode = hd_ExpressCenter.Value;

                DataTable detail = ViewState["tbl_detail"] as DataTable;
                float tempWeight = 0f;
                if (detail.Rows.Count > 0)
                {
                    foreach (DataRow dr in detail.Rows)
                    {
                        float temp = 0f;
                        float.TryParse(dr["Weight"].ToString(), out temp);
                        tempWeight += temp;
                    }
                }

                clvar2.Weight = tempWeight;

                detail.Columns.Remove("Arrived");
                Tuple<bool, string> resp = EditArrival(detail, clvar2);
                if (resp.Item1 == false)
                {
                    AlertMessage(resp.Item2, "Red");
                    return;
                }
                else
                {
                    string script_ = @"window.open(""{0}"", ""{1}"", ""{2}"");";
                    string script = String.Format(script_, "Arrival_print.aspx?Xcode=" + txt_arrivalID.Text, "_blank", "");
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Redirect", script, true);
                    detail.Rows.Clear();
                    ViewState["tbl_detail"] = detail;
                    GridView.DataSource = detail;
                    GridView.DataBind();

                    txt_ridercode.Text = "";
                    txt_ridername.Text = "";
                    txt_weight.Text = "";
                    txt_arrivalID.Text = "";
                    txt_arrivalID.Enabled = false;
                    rbtn_mode.SelectedValue = "0";
                    dd_contype.ClearSelection();
                    dd_servicetype.ClearSelection();
                }
                #endregion
            }



        }

        public void Reset()
        {

        }
        //  protected void txt_weight_TextChanged1(object sender, EventArgs e)

        protected void new_Consignment()
        {
            if (dd_servicetype.SelectedValue.ToUpper() == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Select ServiceType')", true);
                return;
            }
            txt_ridername.Enabled = false;
            clvar._Zone = Session["ZONECODE"].ToString();
            clvar._TownCode = Session["BRANCHCODE"].ToString();
            clvar._ConsignmentNo = txt_consignment.Text;
            //  ConsignmentTypeName = dd_contype.SelectedItem;

            DataSet ds1 = b_fun.Get_ConsignmentNumber(clvar);
            if (ds1.Tables[0].Rows.Count == 0)
            {
                //   error_msg.Text = "This Consignment Not Valid...";
                dd_contype.Enabled = true;
                txt_ridercode.Enabled = true;
                txt_ridername.Enabled = true;
                txt_weight.Enabled = true;
                dd_servicetype.Enabled = true;
                clvar._Orign = hd_ExpressCenter.Value;

                DataTable dt_head = ViewState["dthead"] as DataTable;
                DataTable dt_head_ = ViewState["dthead_"] as DataTable;
                DataTable dtConsignmentTracking = ViewState["dtConsignmentTracking"] as DataTable;
                DataTable dtConsignment = ViewState["dtConsignment"] as DataTable;

                DataRow[] foundRows = dt_head.Select("ConsignmentNo ='" + clvar._ConsignmentNo + "' ");
                if (foundRows.Length == 0)
                {
                    int numberOfRecords = dt_head.Select().Length + 1;
                    lbl_count.Text = "Count: " + numberOfRecords.ToString();

                    //dt_head.Rows.Add(txt_ridercode.Text, txt_ridername.Text, clvar._Zone, clvar._TownCode, txt_weight.Text, dd_servicetype.SelectedValue, dd_contype.SelectedValue, txt_consignment.Text, dd_contype.SelectedItem, clvar._Orign);
                    DataTable dt = new DataTable();
                    //dt_head.Rows.Add(txt_ridercode.Text, txt_ridername.Text, clvar._Zone, clvar._TownCode, txt_weight.Text, dd_servicetype.SelectedValue, dd_contype.SelectedValue, txt_consignment.Text, dd_contype.SelectedItem, clvar._Orign);
                    dt = dt_head.Clone();
                    dt.Clear();
                    dt.Rows.Add(txt_ridercode.Text, txt_ridername.Text, clvar._Zone, clvar._TownCode, txt_weight.Text, dd_servicetype.SelectedValue, dd_contype.SelectedValue, txt_consignment.Text, dd_contype.SelectedItem, clvar._Orign);
                    dt.Merge(dt_head);
                    dt_head.Clear();
                    dt_head = dt;


                    dt.Dispose();
                    dt = new DataTable();
                    dt = dt_head_.Clone();
                    dt.Clear();
                    //                dt_head_.Rows.Add(0, clvar._ConsignmentNo, DateTime.Now, HttpContext.Current.Session["U_ID"].ToString());
                    dt.Rows.Add(0, clvar._ConsignmentNo, DateTime.Now, HttpContext.Current.Session["U_ID"].ToString());
                    dt.Merge(dt_head_);
                    dt_head_.Clear();
                    dt_head_ = dt;

                    dt.Dispose();
                    dt = new DataTable();
                    dt = dtConsignment.Clone();
                    dt.Clear();
                    //dtConsignment.Rows.Add(clvar._ConsignmentNo, dd_servicetype.SelectedValue, dd_contype.SelectedValue, txt_weight.Text, HttpContext.Current.Session["BranchCode"].ToString(), HttpContext.Current.Session["BranchCode"].ToString(), DateTime.Now, HttpContext.Current.Session["U_ID"].ToString(), "1", "330140", HttpContext.Current.Session["zonecode"].ToString(), HttpContext.Current.Session["BranchCode"].ToString(), "4D1", DateTime.Now, this.txt_ridercode.Text);
                    dt.Rows.Add(clvar._ConsignmentNo, dd_servicetype.SelectedValue, dd_contype.SelectedValue, txt_weight.Text, HttpContext.Current.Session["BranchCode"].ToString(), HttpContext.Current.Session["BranchCode"].ToString(), DateTime.Now, HttpContext.Current.Session["U_ID"].ToString(), "1", "330140", HttpContext.Current.Session["zonecode"].ToString(), HttpContext.Current.Session["BranchCode"].ToString(), "4D1", DateTime.Now, this.txt_ridercode.Text);
                    dt.Merge(dtConsignment);
                    dtConsignment.Clear();
                    dtConsignment = dt;

                    dt.Dispose();
                    dt = new DataTable();
                    dt = dtConsignmentTracking.Clone();
                    dt.Clear();
                    //dtConsignmentTracking.Rows.Add(clvar._ConsignmentNo, "18", b_fun.Get_Branches(clvar).Tables[0].Rows[0]["name"].ToString(), "", "", "", "", "", "", DateTime.Now, "", DateTime.Now, "", "0", "", "", "", "", "");
                    dt.Rows.Add(clvar._ConsignmentNo, "18", b_fun.Get_Branches(clvar).Tables[0].Rows[0]["name"].ToString(), "", "", "", "", "", "", DateTime.Now, "", DateTime.Now, "", "0", "", "", "", "", "");
                    dt.Merge(dtConsignmentTracking);
                    dtConsignmentTracking.Clear();
                    dtConsignmentTracking = dt;
                    dt_head.AcceptChanges();
                    dt_head_.AcceptChanges();
                    dtConsignmentTracking.AcceptChanges();

                    ViewState["dthead"] = dt_head;
                    ViewState["dthead_"] = dt_head_;
                    ViewState["dtConsignmentTracking"] = dtConsignmentTracking;
                    ViewState["dtConsignment"] = dtConsignment;


                    ViewState["dthead"] = dt_head;
                    ViewState["dthead_"] = dt_head_;
                    GridView.DataSource = dt_head;
                    GridView.DataBind();

                    //txt_ridercode.Enabled = false;
                    txt_ridername.Enabled = false;
                    txt_consignment.Text = "";
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "showError",
                            "alert('Consignment Already in List');", true);
                }

            }
            //  txt_weight.Text = "0";
        }

        protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string arrived = (e.Row.FindControl("hd_arrived") as HiddenField).Value;

                if (arrived == "1")
                {


                    (e.Row.FindControl("btn_remove") as Button).Visible = false;
                    (e.Row.FindControl("txt_gWeight") as TextBox).Enabled = false;
                    (e.Row.FindControl("txt_gPieces") as TextBox).Enabled = false;
                }
                else
                {
                    (e.Row.FindControl("btn_remove") as Button).Visible = true;
                    (e.Row.FindControl("txt_gWeight") as TextBox).Enabled = true;
                    (e.Row.FindControl("txt_gPieces") as TextBox).Enabled = true;
                }
            }
        }

        protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int index = Convert.ToInt32(e.RowIndex);
            string cn = GridView.Rows[index].Cells[1].Text;
            DataTable detail = ViewState["tbl_detail"] as DataTable;
            DataRow dr = detail.Select("ConsignmentNumber = '" + cn + "'").First();
            detail.Rows.Remove(dr);
            detail.AcceptChanges();
            //DataTable dt = ViewState["dthead"] as DataTable;
            //dt.Rows[index].Delete();
            //int numberOfRecords = dt.Select().Length;
            //lbl_count.Text = "Count: " + numberOfRecords.ToString();
            //ViewState["dthead"] = dt;
            ViewState["tbl_detail"] = detail;
            GridView.DataSource = detail;
            GridView.DataBind();
            lbl_count.Text = "Total CNs: " + detail.Rows.Count.ToString();
        }







        /*
        protected void txt_weight_TextChanged(object sender, EventArgs e)
        {
            txt_weight.Text = "";
            txt_weight.Focus();

            return;
            txt_ridername.Enabled = false;
            clvar._Zone = Session["ZONECODE"].ToString();
            clvar._TownCode = Session["BRANCHCODE"].ToString();
            clvar._ConsignmentNo = txt_consignment.Text;

            DataSet ds1 = b_fun.Get_ConsignmentNumber(clvar);
            if (ds1.Tables[0].Rows.Count == 0)
            {
                //   error_msg.Text = "This Consignment Not Valid...";
                dd_contype.Enabled = true;
                txt_ridercode.Enabled = true;
                txt_ridername.Enabled = true;
                txt_weight.Enabled = true;
                dd_servicetype.Enabled = true;

                if (txt_ridercode.Text != "" && txt_ridername.Text != "" && txt_weight.Text != "" && txt_consignment.Text != "")
                {
                    DataTable dt_head = ViewState["dthead"] as DataTable;

                    int numberOfRecords = dt_head.Select().Length + 1;
                    lbl_count.Text = "Count: " + numberOfRecords.ToString();

                    dt_head.Rows.Add(txt_ridercode.Text, txt_ridername.Text, clvar._Zone, clvar._TownCode, txt_weight.Text, dd_servicetype.SelectedValue, dd_contype.SelectedValue, txt_consignment.Text);

                    dt_head.AcceptChanges();
                    ViewState["dthead"] = dt_head;
                    GridView.DataSource = dt_head;
                    GridView.DataBind();
                }

                //txt_ridercode.Text = "";
                //txt_ridername.Text = "";
                //txt_weight.Text = "";
            }
            txt_weight.Text = "0";
        }
        */

        protected void GridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "remove")
            {
                string cn = e.CommandArgument.ToString();

                DataTable detail = ViewState["tbl_detail"] as DataTable;
                DataRow dr = detail.Select("ConsignmentNumber = '" + cn + "'").First();
                detail.Rows.Remove(dr);
                detail.AcceptChanges();
                //DataTable dt = ViewState["dthead"] as DataTable;
                //dt.Rows[index].Delete();
                //int numberOfRecords = dt.Select().Length;
                //lbl_count.Text = "Count: " + numberOfRecords.ToString();
                //ViewState["dthead"] = dt;
                ViewState["tbl_detail"] = detail;
                GridView.DataSource = detail;
                GridView.DataBind();
                lbl_count.Text = "Total CNs: " + detail.Rows.Count.ToString();
                #region OLD WORKING FAULTY
                //DataTable dt_head = ViewState["dthead"] as DataTable;
                //DataTable dt_head_ = ViewState["dthead_"] as DataTable;
                //DataTable dtConsignmentTracking = ViewState["dtConsignmentTracking"] as DataTable;
                //DataTable dtConsignment = ViewState["dtConsignment"] as DataTable; //DataRow dr = dt.Select("ConsignmentNumber = '" + con + "'")[0];

                //DataRow dr = dt_head.Select("ConsignmentNo = '" + con + "'")[0];
                //DataRow dr1 = dt_head_.Select("ConsignmentNumber = '" + con + "'")[0];
                //DataRow dr2 = dtConsignmentTracking.Select("ConsignmentNumber = '" + con + "'")[0];
                //DataRow dr3 = dtConsignment.Select("ConsignmentNumber = '" + con + "'")[0];

                //dt_head.Rows.Remove(dr);
                //dt_head_.Rows.Remove(dr1);
                //dtConsignmentTracking.Rows.Remove(dr2);
                //dtConsignment.Rows.Remove(dr3);
                //ViewState["dthead"] = dt_head;
                //ViewState["dthead_"] = dt_head_;
                //ViewState["dtConsignmentTracking"] = dtConsignmentTracking;
                //ViewState["dtConsignment"] = dtConsignment;

                //lbl_count.Text = dt_head.Rows.Count.ToString();
                //GridView.DataSource = dt_head;
                //GridView.DataBind(); 
                #endregion
            }
        }

        public DataSet Get_ConsignmentNumber(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                // string query = "select c.serviceTypeName, c.riderCode, c.consignmentTypeId, c.weight from Consignment c where c.consignmentNumber = '" + clvar._ConsignmentNo + "' ";
                string query = "select c.serviceTypeName, \n" +
                               " c.riderCode,\n" +
                               " c.consignmentTypeId,\n" +
                               " c.weight,\n" +
                               " c.pieces,\n" +
                               " c.originExpressCenter,\n" +
                               " c.ConsignmentNumber,\n" +
                               " c.orgin,\n" +
                               " c.destination,\n" +
                               " c.zoneCode,\n" +
                               " c.BranchCode,\n" +
                               " c.ExpressCenterCode,\n" +
                               " c.creditClientID, c.consignerAccountNo \n" +
                               " from Consignment c where c.consignmentNumber = '" + clvar._ConsignmentNo + "' ";

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

        public bool IsNumeric(string text)
        {
            char[] arr = text.ToCharArray();
            foreach (char ch in arr)
            {
                if (!char.IsDigit(ch))
                {
                    return false;
                }
            }

            return true;
        }

        public void AlertMessage(string Message, string color)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert(" + Message + ")", true);
            error_msg.Text = Message;
            error_msg.ForeColor = System.Drawing.Color.FromName(color);
        }
        protected void GridView_DataBound(object sender, EventArgs e)
        {
            float totalWeight = 0f;
            foreach (GridViewRow row in GridView.Rows)
            {
                Button btn_remove = row.FindControl("btn_remove") as Button;
                btn_remove.Attributes["onclick"] = "if(!confirm('Do you want to delete " + row.Cells[1].Text + "?')){ return false; };";



                row.Cells[3].Text = dd_contype.SelectedItem.Text;
                row.Cells[6].Text = txt_ridercode.Text;
                row.Cells[7].Text = txt_ridername.Text;

                float temp = 0f;
                float.TryParse((row.FindControl("txt_gWeight") as TextBox).Text, out temp);

                totalWeight += temp;

            }
            txt_weight.Text = totalWeight.ToString();
        }

        public Tuple<bool, string> CreateArrival(DataTable dt, Cl_Variables clvar2)
        {
            Tuple<bool, string> resp = new Tuple<bool, string>(false, "");

            SqlConnection con = new SqlConnection(clvar2.Strcon());
            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "MNP_SP_CREATEARRIVAL";

                cmd.Parameters.AddWithValue("@tblDetail", dt);
                cmd.Parameters.AddWithValue("@ExpressCenterCode", clvar2.expresscenter);
                cmd.Parameters.AddWithValue("@BranchCode", clvar2.Branch);
                cmd.Parameters.AddWithValue("@ZoneCode", clvar2.Zone);
                cmd.Parameters.AddWithValue("@CreatedBy", clvar2.createdBy);
                cmd.Parameters.AddWithValue("@RiderCode", clvar2.riderCode);
                cmd.Parameters.AddWithValue("@OriginExpressCenterCode", clvar2.OriginExpressCenterCode);
                cmd.Parameters.AddWithValue("@TotalWeight", clvar2.Weight);

                cmd.Parameters.Add("@ErrorCode", SqlDbType.Int).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@ErrorMessage", SqlDbType.VarChar, 1000).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@ArrivalID", SqlDbType.BigInt).Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();

                if (cmd.Parameters["@ErrorCode"].SqlValue.ToString() == "0")
                {
                    resp = new Tuple<bool, string>(false, cmd.Parameters["@ErrorMessage"].SqlValue.ToString());
                }
                if (cmd.Parameters["@ErrorCode"].SqlValue.ToString() == "1")
                {
                    resp = new Tuple<bool, string>(true, cmd.Parameters["@ArrivalID"].SqlValue.ToString());
                }
            }
            catch (Exception ex)
            { resp = new Tuple<bool, string>(false, ex.Message); }
            finally { con.Close(); }


            return resp;
        }
        public Tuple<bool, string> EditArrival(DataTable dt, Cl_Variables clvar2)
        {
            Tuple<bool, string> resp = new Tuple<bool, string>(false, "");

            SqlConnection con = new SqlConnection(clvar2.Strcon());
            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "MNP_SP_EDITARRIVAL";

                cmd.Parameters.AddWithValue("@ARRIVAL_ID", clvar2.CheckCondition);
                cmd.Parameters.AddWithValue("@tblDetail", dt);
                cmd.Parameters.AddWithValue("@ExpressCenterCode", clvar2.expresscenter);
                cmd.Parameters.AddWithValue("@BranchCode", clvar2.Branch);
                cmd.Parameters.AddWithValue("@ZoneCode", clvar2.Zone);
                cmd.Parameters.AddWithValue("@CreatedBy", clvar2.createdBy);
                cmd.Parameters.AddWithValue("@RiderCode", clvar2.riderCode);
                cmd.Parameters.AddWithValue("@OriginExpressCenterCode", clvar2.OriginExpressCenterCode);
                cmd.Parameters.AddWithValue("@TotalWeight", clvar2.Weight);

                cmd.Parameters.Add("@ErrorCode", SqlDbType.Int).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@ErrorMessage", SqlDbType.VarChar, 1000).Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();

                if (cmd.Parameters["@ErrorCode"].SqlValue.ToString() == "0")
                {
                    resp = new Tuple<bool, string>(false, cmd.Parameters["@ErrorMessage"].SqlValue.ToString());
                }
                if (cmd.Parameters["@ErrorCode"].SqlValue.ToString() == "1")
                {
                    resp = new Tuple<bool, string>(true, cmd.Parameters["@ErrorMessage"].SqlValue.ToString());
                }
            }
            catch (Exception ex)
            { resp = new Tuple<bool, string>(false, ex.Message); }
            finally { con.Close(); }


            return resp;
        }
        protected void txt_arrivalID_TextChanged1(object sender, EventArgs e)
        {
            if (txt_arrivalID.Text.Trim() == "")
            {
                AlertMessage("Enter Arrival ID", "Red");
                return;
            }

            clvar2.CheckCondition = txt_arrivalID.Text.Trim();
            DataSet ds = GetArrivalDetails(clvar2);

            if (ds.Tables.Count > 0)
            {
                DataTable header = ds.Tables["HeaderTable"];
                DataTable detail = ds.Tables["DetailTable"];

                if (header.Rows.Count > 0)
                {
                    txt_ridercode.Text = header.Rows[0]["RiderCode"].ToString();
                    txt_ridername.Text = header.Rows[0]["RiderName"].ToString();
                    txt_weight.Text = header.Rows[0]["Weight"].ToString();
                    hd_ExpressCenter.Value = header.Rows[0]["OriginExpressCenterCode"].ToString();
                }
                else
                {
                    AlertMessage("Invalid Arrival ID", "Red");
                    return;
                }

                if (detail.Rows.Count > 0)
                {
                    DataRow[] dr = detail.Select("serviceType <> ''");
                    if (dr.Count() > 0)
                    {
                        txt_ridercode.Text = "";
                        txt_ridername.Text = "";
                        txt_weight.Text = "";
                        hd_ExpressCenter.Value = "";
                        txt_arrivalID.Text = "";
                        AlertMessage("Please use the new Arrival screen to edit this Arrival", "Red");
                        GridView.DataSource = null;
                        GridView.DataBind();
                        return;
                    }
                    ViewState["tbl_detail"] = detail;
                    GridView.DataSource = detail;
                    GridView.DataBind();
                    lbl_count.Text = "Total CNs: " + detail.Rows.Count.ToString();
                }
                else
                {
                    AlertMessage("Details of This Arrival are not found", "Red");
                    return;
                }
            }
        }


        public DataSet GetArrivalDetails(Cl_Variables clvar)
        {
            DataSet ds = new DataSet();
            string headerQuery = "SELECT a.*, \n"
               + "       r.firstName + ' ' + r.lastName RiderName \n"
               + "FROM   ArrivalScan a \n"
               + "       INNER JOIN Riders r \n"
               + "            ON  r.riderCode = a.RiderCode \n"
               + "            AND r.branchId = a.BranchCode \n"
               + "WHERE  a.Id = '" + clvar.CheckCondition + "' \n"
               + "       AND r.[status] = '1' \n"
               + "       AND a.BranchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'";

            string detailQuery = "SELECT ad.consignmentNumber, \n"
               + "       c.serviceTypeName, \n"
               + "       c.consignmentTypeId, \n"
               + "       ad.cnWeight weight, \n"
               + "       ad.cnPieces pieces, \n"
               + "       c.orgin Origin, \n"
               + "       c.destination, \n"
               + "       c.creditClientId, \n"
               + "       c.zonecode, \n"
               + "       c.branchCode, \n"
               + "       c.consignerAccountNo, \n"
               + "       a.RiderCode, \n"
               + "       CASE WHEN c.consignmentNumber IS NULL THEN 1 ELSE 0 END CnInsert, \n"
               + "       0 isNew, '1' Arrived, ad.serviceType, ad.consignmentType, ad.sortOrder \n"
               + "FROM   ArrivalScan a \n"
               + "INNER JOIN ArrivalScan_Detail Ad \n"
               + "ON ad.ArrivalID = a.Id \n"
               + "       LEFT OUTER JOIN Consignment c \n"
               + "            ON  c.consignmentNumber = ad.consignmentNumber \n"
               + " WHERE  ad.ArrivalID = '" + clvar.CheckCondition + "'\n"
               + " and a.branchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'";

            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(headerQuery, con);
                sda.Fill(ds, "HeaderTable");

                sda = new SqlDataAdapter(detailQuery, con);
                sda.Fill(ds, "DetailTable");

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }

            return ds;
        }

    }
}