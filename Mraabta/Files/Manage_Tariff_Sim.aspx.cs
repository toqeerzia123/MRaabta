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
    public partial class Manage_Tariff_Sim : System.Web.UI.Page
    {
        CommonFunction fun = new CommonFunction();
        Cl_Variables clvar = new Cl_Variables();
        Consignemnts con = new Consignemnts();



        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Branches();
                ServiceTypes();
                Zones();
                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[]
                {
                new DataColumn("ID"),
                new DataColumn("DestinationID"),
                new DataColumn("Destination"),
                new DataColumn("ServiceID"),
                new DataColumn("FromWeight"),
                new DataColumn("ToWeight"),
                new DataColumn("Price"),
                new DataColumn("AddFactor"),
                new DataColumn("isUpdated")
                });
                dt.AcceptChanges();
                ViewState["dt"] = dt;

                // All Zone Information
                DataTable dt_ = new DataTable();
                dt_.Columns.AddRange(new DataColumn[]
                {
                new DataColumn("ID"),
                new DataColumn("FromWeight"),
                new DataColumn("ToWeight"),
                new DataColumn("AddFactor"),
                new DataColumn("Local"),
                new DataColumn("Same"),
                new DataColumn("Diff"),
                new DataColumn("client"),
                new DataColumn("Pr"),
                new DataColumn("isUpdated")
                });
                dt_.AcceptChanges();
                Session["dt_"] = dt_;
            }
        }

        protected void Branches()
        {
            DataTable dt = Branch().Tables[0];
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    dd_branch.DataSource = dt;
                    dd_branch.DataTextField = "BranchName";
                    dd_branch.DataValueField = "branchCode";
                    dd_branch.DataBind();
                    dd_branch.SelectedValue = Session["BranchCode"].ToString();
                }
            }
        }

        public DataSet ServiceTypeName_()
        {
            DataSet Ds_1 = new DataSet();

            try
            {
                string query = "SELECT st.name          ServiceTypeName \n"
                + "FROM   ServiceTypes     st \n"
                + "WHERE  st.IsIntl = '0' \n"
                + "       AND st.[status] = '1' \n"
                + "       And st.name not in ('Expressions','Road N Rail') \n"
                + "GROUP BY \n"
                + "       st.name \n"
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


        protected void ServiceTypes()
        {
            DataTable dt = ServiceTypeName().Tables[0];
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    dd_serviceType.DataSource = dt;
                    dd_serviceType.DataTextField = "ServiceTypeName";
                    dd_serviceType.DataValueField = "ServiceTypeName";
                    dd_serviceType.DataBind();
                    dd_serviceType.SelectedValue = "overnight";
                    //dd_serviceType.SelectedValue = Session["BranchCode"].ToString();
                }
            }
        }

        protected void Zones()
        {
            DataTable dt = GetZonesForDomesticTariff();
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    //dd_zone.DataSource = dt;
                    //dd_zone.DataTextField = "name";
                    //dd_zone.DataValueField = "zoneCode";
                    //dd_zone.DataBind();

                    //dd_toZone.DataSource = dt;
                    //dd_toZone.DataTextField = "name";
                    //dd_toZone.DataValueField = "zoneCode";
                    //dd_toZone.DataBind();
                    //dd_serviceType.SelectedValue = Session["BranchCode"].ToString();
                }
            }
        }

        protected void btn_cancelupdate_Click(object sender, EventArgs e)
        {

        }

        protected void btn_update_Click(object sender, EventArgs e)
        {

        }

        protected void txt_accountNo_TextChanged(object sender, EventArgs e)
        {
            clvar.Branch = dd_branch.SelectedValue;
            clvar.AccountNo = txt_accountNo.Text;
            DataTable dt = GetAccountDetailByAccountNumber(clvar);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {

                    txt_fromZone.Text = dt.Rows[0]["ZoneNAME"].ToString();
                    txt_clientName.Text = dt.Rows[0]["NAME"].ToString();
                    creditclientid.Value = dt.Rows[0]["ID"].ToString();
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Invalid Account Number')", true);
                    txt_accountNo.Text = "";
                    return;
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Invalid Account Number')", true);
                txt_accountNo.Text = "";
                return;
            }
        }

        protected void btn_showTariff_Click(object sender, EventArgs e)
        {
            #region Validations
            if (dd_branch.SelectedValue == "0")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Select Branch')", true);
                return;
            }

            if (dd_serviceType.SelectedValue == "0")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Select Service Type')", true);
                return;
            }

            if (txt_accountNo.Text.Trim(' ') == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter Account Number')", true);

                return;
            }

            #endregion
            // clvar.ToZoneCode = dd_toZone.SelectedValue;
            clvar.CustomerClientID = creditclientid.Value;
            clvar.ServiceTypeName = dd_serviceType.SelectedValue;
            clvar.Branch = dd_branch.SelectedValue;
            DataTable tariff = ViewState["dt"] as DataTable;
            tariff.Clear();
            DataTable dt = GetTarrifForEdit_1(clvar);
            gv_tariff.DataSource = null;
            gv_tariff.DataBind();
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        DataRow dr = tariff.NewRow();
                        dr["ID"] = row["ID"].ToString();
                        dr["DestinationID"] = row["TOZONECODE"].ToString();
                        dr["Destination"] = row["ZONENAME"].ToString();
                        dr["FromWeight"] = row["FromWeight"].ToString();
                        dr["ToWeight"] = row["toWeight"].ToString();
                        dr["addFactor"] = row["AdditionalFactor"].ToString();
                        dr["ServiceID"] = row["ServiceID"].ToString();
                        dr["Price"] = row["Price"].ToString();
                        tariff.Rows.Add(dr);
                        tariff.AcceptChanges();

                    }
                    //btn_addPrice.Enabled = false;

                }
                else
                {
                    gv_tariff_.DataSource = null;
                    gv_tariff_.DataBind();
                    // btn_addPrice.Enabled = true;

                }
            }

            if (tariff != null)
            {
                if (tariff.Rows.Count > 0)
                {
                    gv_tariff_.DataSource = tariff;
                    gv_tariff_.DataBind();
                }
            }

            ViewState["dt"] = tariff;
            hyp_1.NavigateUrl = "Print_Tariffs.aspx?ClientID=" + creditclientid.Value + "&ServiceType=" + dd_serviceType.SelectedValue + "";
            hyp_1.Text = "View Tariff";
        }

        protected void btn_addWeight_Click(object sender, EventArgs e)
        {
            //if (txt_accountNo.Text == "0")
            //{
            //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cannot Update Cash Tariff')", true);
            //    err_msg.Text = "Cannot Update Cash Tariff";
            //    return;
            //}
            DataTable tariff = Session["dt_"] as DataTable;

            clvar.AdditionalFactor = txt_additionalWeight.Text;

            foreach (DataRow row in tariff.Rows)
            {
                row["addFactor"] = clvar.AdditionalFactor;

                //if (row["isUpdated"].ToString() != "NEW")
                //{
                //    row["isUpdated"] = "YES";
                //}
            }
            tariff.AcceptChanges();
            gv_tariff.DataSource = null;
            gv_tariff.DataBind();
            gv_tariff.DataSource = tariff;
            gv_tariff.DataBind();

        }

        protected void btn_addPrice_Click(object sender, EventArgs e)
        {
            //if (txt_accountNo.Text == "0")
            //{
            //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cannot Update Cash Tariff')", true);
            //    err_msg.Text = "Cannot Update Cash Tariff";
            //    return;
            //}
            if (txt_fromWeight.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter From Weight')", true);
                return;
            }
            if (txt_toWeight.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter To Weight')", true);
                return;
            }


            DataTable tariff = Session["dt_"] as DataTable;

            // Checking Duplicate entry

            DataTable tariff_ = Session["dt_1"] as DataTable;

            clvar.ToWeight = txt_toWeight.Text;
            clvar.FromWeight = txt_fromWeight.Text;

            DataRow[] dr_ = tariff.Select("FromWeight='" + txt_fromWeight.Text + "'");
            if (dr_.Length > 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('From Weight Already Present')", true);
                //            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('From Weight Already Present')", true);
                txt_fromWeight.Text = "";
                txt_fromWeight.Focus();
                return;

            }
            dr_ = null;
            dr_ = tariff.Select("ToWeight='" + txt_toWeight.Text + "'");
            if (dr_.Length > 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('To Weight Already Present')", true);
                //            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('To Weight Already Present')", true);
                txt_toWeight.Text = "";
                txt_toWeight.Focus();
                return;

            }

            if (tariff.Rows.Count > 0)
            {
                clvar.AdditionalFactor = tariff.Rows[0]["addFactor"].ToString();
            }
            else
            {
                clvar.AdditionalFactor = txt_additionalWeight.Text;
            }
            DataRow dr = tariff.NewRow();
            bool flag = false;

            dr["ID"] = (tariff.Rows.Count + 1).ToString();
            dr["FromWeight"] = clvar.FromWeight;
            dr["ToWeight"] = clvar.ToWeight;

            if (tariff.Rows.Count > 0)
            {
                dr["addFactor"] = tariff.Rows[0]["addFactor"].ToString();
            }
            else
            {
                dr["addFactor"] = txt_additionalWeight.Text;
            }
            //   dr["isUpdated"] = "NEW";
            dr["client"] = creditclientid.Value; ;

            tariff.Rows.Add(dr);
            DataView tempview = tariff.DefaultView;
            tempview.Sort = "FromWeight";

            tariff = tempview.ToTable();
            Session["dt_"] = tariff;

            gv_tariff.DataSource = null;
            gv_tariff.DataBind();

            gv_tariff.DataSource = tariff;
            gv_tariff.DataBind();


            txt_fromWeight.Text = "";
            this.txt_toWeight.Text = "";

            //err_msg.Text = "Tariff has been saved Temporarily";
            err_msg.Visible = false;


        }

        protected void btn_reset_Click(object sender, EventArgs e)
        {
            ResetAll();
        }

        protected void btn_save_Click(object sender, EventArgs e)
        {
            //if (txt_accountNo.Text == "0")
            //{
            //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cannot Update Cash Tariff')", true);
            //    err_msg.Text = "Cannot Update Cash Tariff";
            //    return;
            //}
            clvar.CustomerClientID = creditclientid.Value;
            clvar.ServiceTypeName = dd_serviceType.SelectedValue;
            clvar.Branch = dd_branch.SelectedValue;
            clvar.FromZoneCode = "";//dd_zone.SelectedValue;
                                    //   clvar.ToZoneCode = dd_toZone.SelectedValue;
            try
            {
                clvar.FromZoneCode = HttpContext.Current.Session["ZoneCode"].ToString();
            }
            catch (Exception ex)
            { return; }

            DataTable tarrif = Session["dt_"] as DataTable;
            DataTable Tariff_Actual = (ViewState["dt"] as DataTable).Clone();
            Tariff_Actual.Clear();

            //List<DataRow> dr = new List<DataRow>();
            int count = 0;

            // For Local
            //DataRow[] dr_Local = tarrif.Select("Local != '0' and Pr='0'");
            var result = from r in tarrif.AsEnumerable()
                         where r.Field<string>("Pr") == "False" && r.Field<string>("Local") != "0"
                         select r;
            if (result.Count() > 0)
            {
                DataTable dtResult = result.CopyToDataTable();

                DataRow[] dr_Local = dtResult.Select();
                if (dr_Local.Length > 0)
                {
                    for (int i = 0; i < dr_Local.Length; i++)
                    {
                        DataRow dr_arrival = Tariff_Actual.NewRow();

                        dr_arrival["ID"] = "";
                        dr_arrival["DestinationID"] = "17";//HttpContext.Current.Session["ZoneCode"].ToString();
                        dr_arrival["Destination"] = "17";//HttpContext.Current.Session["ZoneCode"].ToString();
                        dr_arrival["FromWeight"] = dr_Local[i]["FromWeight"].ToString();// clvar.FromWeight;
                        dr_arrival["ServiceID"] = dd_serviceType.SelectedValue;  //dr_Same[i]["FromWeight"].ToString();// clvar.FromWeight;
                        dr_arrival["ToWeight"] = dr_Local[i]["ToWeight"].ToString(); //clvar.ToWeight;
                        dr_arrival["Price"] = dr_Local[i]["Local"].ToString(); //txt_price.Text;
                        dr_arrival["addFactor"] = dr_Local[i]["AddFactor"].ToString(); //;
                                                                                       //dr["addFactor"] = tariff.Rows[0]["addFactor"].ToString();
                        dr_arrival["isUpdated"] = "NEW";

                        Tariff_Actual.Rows.Add(dr_arrival);
                        Tariff_Actual.AcceptChanges();
                    }
                }
            }

            DataTable Zone_Infor = ZoneInfo();
            DataTable Zone_Same = ZoneInfo_Same(Zone_Infor.Rows[0]["Colorid"].ToString());

            // For Same
            result = from r in tarrif.AsEnumerable()
                     where r.Field<string>("Pr") == "False" && r.Field<string>("Same") != "0"
                     select r;
            if (result.Count() > 0)
            {
                DataTable dtResult = result.CopyToDataTable();

                DataRow[] dr_Same = dtResult.Select();
                if (dr_Same.Length > 0)
                {
                    for (int i = 0; i < dr_Same.Length; i++)
                    {
                        DataRow dr_arrival = Tariff_Actual.NewRow();

                        dr_arrival["ID"] = "";
                        dr_arrival["DestinationID"] = "14";//HttpContext.Current.Session["ZoneCode"].ToString();
                        dr_arrival["Destination"] = "14";// HttpContext.Current.Session["ZoneCode"].ToString();
                        dr_arrival["FromWeight"] = dr_Same[i]["FromWeight"].ToString();// clvar.FromWeight;
                        dr_arrival["ServiceID"] = dd_serviceType.SelectedValue;  //dr_Same[i]["FromWeight"].ToString();// clvar.FromWeight;
                        dr_arrival["ToWeight"] = dr_Same[i]["ToWeight"].ToString(); //clvar.ToWeight;
                        dr_arrival["Price"] = dr_Same[i]["Same"].ToString(); //txt_price.Text;
                        dr_arrival["addFactor"] = dr_Same[i]["AddFactor"].ToString(); //;
                                                                                      //dr["addFactor"] = tariff.Rows[0]["addFactor"].ToString();
                        dr_arrival["isUpdated"] = "NEW";

                        Tariff_Actual.Rows.Add(dr_arrival);
                        Tariff_Actual.AcceptChanges();
                        if (Zone_Same.Rows.Count != 0)
                        {
                            for (int i1 = 0; i1 < Zone_Same.Rows.Count; i1++)
                            {
                                dr_arrival = Tariff_Actual.NewRow();
                                Tariff_Actual.AcceptChanges();

                                dr_arrival["ID"] = "";
                                dr_arrival["DestinationID"] = Zone_Same.Rows[i1]["Zonecode"].ToString(); //HttpContext.Current.Session["ZoneCode"].ToString();
                                dr_arrival["Destination"] = Zone_Same.Rows[i1]["Zonecode"].ToString(); HttpContext.Current.Session["ZoneCode"].ToString();
                                dr_arrival["FromWeight"] = dr_Same[i]["FromWeight"].ToString();// clvar.FromWeight;
                                dr_arrival["ServiceID"] = dd_serviceType.SelectedValue;  //dr_Same[i]["FromWeight"].ToString();// clvar.FromWeight;
                                dr_arrival["ToWeight"] = dr_Same[i]["ToWeight"].ToString(); //clvar.ToWeight;
                                dr_arrival["Price"] = dr_Same[i]["Same"].ToString(); //txt_price.Text;
                                dr_arrival["addFactor"] = dr_Same[i]["AddFactor"].ToString(); //;
                                                                                              //dr["addFactor"] = tariff.Rows[0]["addFactor"].ToString();
                                dr_arrival["isUpdated"] = "NEW";

                                Tariff_Actual.Rows.Add(dr_arrival);
                                Tariff_Actual.AcceptChanges();
                            }
                        }
                    }
                }
            }
            // For Diff
            result = from r in tarrif.AsEnumerable()
                     where r.Field<string>("Pr") == "False" && r.Field<string>("Diff") != "0"
                     select r;
            if (result.Count() > 0)
            {
                DataTable dtResult = result.CopyToDataTable();

                DataRow[] dr_Diff = dtResult.Select();

                DataTable ZoneType = ZoneInfo(Zone_Infor.Rows[0]["Colorid"].ToString());
                if (dr_Diff.Length > 0)
                {
                    if (ZoneType.Rows.Count != 0)
                    {
                        for (int i = 0; i < dr_Diff.Length; i++)
                        {
                            DataRow dr_arrival_ = Tariff_Actual.NewRow();

                            dr_arrival_["ID"] = "";
                            dr_arrival_["DestinationID"] = "16";//ZoneType.Rows[i1]["Zonecode"].ToString();
                            dr_arrival_["Destination"] = "16";//ZoneType.Rows[i1]["Zonecode"].ToString();// HttpContext.Current.Session["ZoneCode"].ToString();
                            dr_arrival_["ServiceID"] = dd_serviceType.SelectedValue;  //dr_Same[i]["FromWeight"].ToString();// clvar.FromWeight;
                            dr_arrival_["FromWeight"] = dr_Diff[i]["FromWeight"].ToString();// clvar.FromWeight;
                            dr_arrival_["ToWeight"] = dr_Diff[i]["ToWeight"].ToString(); //clvar.ToWeight;
                            dr_arrival_["Price"] = dr_Diff[i]["Diff"].ToString(); //txt_price.Text;
                            dr_arrival_["addFactor"] = dr_Diff[i]["AddFactor"].ToString(); //;
                                                                                           //dr["addFactor"] = tariff.Rows[0]["addFactor"].ToString();
                            dr_arrival_["isUpdated"] = "NEW";
                            Tariff_Actual.Rows.Add(dr_arrival_);
                            Tariff_Actual.AcceptChanges();


                            for (int i1 = 0; i1 < ZoneType.Rows.Count; i1++)
                            {
                                DataRow dr_arrival = Tariff_Actual.NewRow();

                                dr_arrival["ID"] = "";
                                dr_arrival["DestinationID"] = ZoneType.Rows[i1]["Zonecode"].ToString();
                                dr_arrival["Destination"] = ZoneType.Rows[i1]["Zonecode"].ToString();// HttpContext.Current.Session["ZoneCode"].ToString();
                                dr_arrival["ServiceID"] = dd_serviceType.SelectedValue;  //dr_Same[i]["FromWeight"].ToString();// clvar.FromWeight;
                                dr_arrival["FromWeight"] = dr_Diff[i]["FromWeight"].ToString();// clvar.FromWeight;
                                dr_arrival["ToWeight"] = dr_Diff[i]["ToWeight"].ToString(); //clvar.ToWeight;
                                dr_arrival["Price"] = dr_Diff[i]["Diff"].ToString(); //txt_price.Text;
                                dr_arrival["addFactor"] = dr_Diff[i]["AddFactor"].ToString(); //;
                                                                                              //dr["addFactor"] = tariff.Rows[0]["addFactor"].ToString();
                                dr_arrival["isUpdated"] = "NEW";
                                Tariff_Actual.Rows.Add(dr_arrival);
                                Tariff_Actual.AcceptChanges();
                            }




                        }

                    }
                }
            }

            // For Pr
            DataRow[] dr_Pr = tarrif.Select(" Pr='True'");
            DataTable ZoneType_Pr = ZoneInfo_Pr();

            if (dr_Pr.Length > 0)
            {
                if (ZoneType_Pr.Rows.Count != 0)
                {
                    for (int i1 = 0; i1 < ZoneType_Pr.Rows.Count; i1++)
                    {
                        DataRow dr_arrival = Tariff_Actual.NewRow();

                        if (ZoneType_Pr.Rows[i1]["Zonecode"].ToString() == "LOCAL")
                        {
                            if (dr_Pr[0]["Local"].ToString() != "0")
                            {
                                dr_arrival["ID"] = "";
                                dr_arrival["DestinationID"] = ZoneType_Pr.Rows[i1]["Zonecode"].ToString();
                                dr_arrival["Destination"] = ZoneType_Pr.Rows[i1]["Zonecode"].ToString();// HttpContext.Current.Session["ZoneCode"].ToString();
                                dr_arrival["ServiceID"] = dd_serviceType.SelectedValue;  //dr_Same[i]["FromWeight"].ToString();// clvar.FromWeight;
                                dr_arrival["FromWeight"] = dr_Pr[0]["FromWeight"].ToString();// clvar.FromWeight;
                                dr_arrival["ToWeight"] = dr_Pr[0]["ToWeight"].ToString(); //clvar.ToWeight;
                                dr_arrival["Price"] = dr_Pr[0]["Local"].ToString(); //txt_price.Text;
                                dr_arrival["addFactor"] = dr_Pr[0]["AddFactor"].ToString(); //;
                                                                                            //dr["addFactor"] = tariff.Rows[0]["addFactor"].ToString();
                                dr_arrival["isUpdated"] = "NEW";
                                Tariff_Actual.Rows.Add(dr_arrival);
                                Tariff_Actual.AcceptChanges();
                            }
                        }
                        if (ZoneType_Pr.Rows[i1]["Zonecode"].ToString() == "SAME")
                        {
                            if (dr_Pr[0]["Same"].ToString() != "0")
                            {
                                dr_arrival["ID"] = "";
                                dr_arrival["DestinationID"] = ZoneType_Pr.Rows[i1]["Zonecode"].ToString();
                                dr_arrival["Destination"] = ZoneType_Pr.Rows[i1]["Zonecode"].ToString();// HttpContext.Current.Session["ZoneCode"].ToString();
                                dr_arrival["ServiceID"] = dd_serviceType.SelectedValue;  //dr_Same[i]["FromWeight"].ToString();// clvar.FromWeight;
                                dr_arrival["FromWeight"] = dr_Pr[0]["FromWeight"].ToString();// clvar.FromWeight;
                                dr_arrival["ToWeight"] = dr_Pr[0]["ToWeight"].ToString(); //clvar.ToWeight;
                                dr_arrival["Price"] = dr_Pr[0]["same"].ToString(); //txt_price.Text;
                                dr_arrival["addFactor"] = dr_Pr[0]["AddFactor"].ToString(); //;
                                                                                            //dr["addFactor"] = tariff.Rows[0]["addFactor"].ToString();
                                dr_arrival["isUpdated"] = "NEW";
                                Tariff_Actual.Rows.Add(dr_arrival);
                                Tariff_Actual.AcceptChanges();
                            }
                        }
                        if (ZoneType_Pr.Rows[i1]["Zonecode"].ToString() == "DIFF")
                        {
                            if (dr_Pr[0]["Diff"].ToString() != "0")
                            {
                                dr_arrival["ID"] = "";
                                dr_arrival["DestinationID"] = ZoneType_Pr.Rows[i1]["Zonecode"].ToString();
                                dr_arrival["Destination"] = ZoneType_Pr.Rows[i1]["Zonecode"].ToString();// HttpContext.Current.Session["ZoneCode"].ToString();
                                dr_arrival["ServiceID"] = dd_serviceType.SelectedValue;  //dr_Same[i]["FromWeight"].ToString();// clvar.FromWeight;
                                dr_arrival["FromWeight"] = dr_Pr[0]["FromWeight"].ToString();// clvar.FromWeight;
                                dr_arrival["ToWeight"] = dr_Pr[0]["ToWeight"].ToString(); //clvar.ToWeight;
                                dr_arrival["Price"] = dr_Pr[0]["Diff"].ToString(); //txt_price.Text;
                                dr_arrival["addFactor"] = dr_Pr[0]["AddFactor"].ToString(); //;
                                                                                            //dr["addFactor"] = tariff.Rows[0]["addFactor"].ToString();
                                dr_arrival["isUpdated"] = "NEW";
                                Tariff_Actual.Rows.Add(dr_arrival);
                                Tariff_Actual.AcceptChanges();
                            }
                        }
                    }
                }
            }

            //  btn_showTariff_Click(sender, e);
            Tariff_Actual = RemoveDuplicates(Tariff_Actual);// Tariff_Actual.AsEnumerable().Distinct().CopyToDataTable();// Tariff_Actual.DefaultView.ToTable(true, "FromWeight", "ToWeight",);
            DataTable Tariff_Previous = ViewState["dt"] as DataTable;
            DataTable Tariff_Previous_1 = Tariff_Previous.Clone();
            Tariff_Previous_1.Clear();

            if (Tariff_Previous.Rows.Count != 0)
            {
                // ScriptManager.RegisterStartupScript(this, this.GetType(), "showPopup", "if (!confirm('Tariff Already Present, Do you want to continue?')) return false;", true);
                // Removeing Previous entries 
                for (int i = 0; i < Tariff_Actual.Rows.Count; i++)
                {
                    for (int j = 0; j < Tariff_Previous.Rows.Count; j++)
                    {
                        DataRow dr = Tariff_Previous.Rows[j];
                        if (dr["ServiceID"].ToString() == dd_serviceType.SelectedValue && dr["FromWeight"].ToString() == Tariff_Actual.Rows[i]["Fromweight"].ToString() && dr["ToWeight"].ToString() == Tariff_Actual.Rows[i]["ToWeight"].ToString() && dr["DestinationID"].ToString() == Tariff_Actual.Rows[i]["DestinationID"].ToString())
                        {
                            DataRow dr1 = Tariff_Previous_1.NewRow();
                            dr1 = dr;

                            Tariff_Previous_1.ImportRow(dr1);
                        }
                    }
                }


                //Updating

                for (int i = 0; i < Tariff_Previous_1.Rows.Count; i++)
                {
                    clvar.TariffID = Tariff_Previous_1.Rows[i]["ID"].ToString();
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Previously Defined Tariffs will be updated')", true);

                    DeleteTariff(clvar);
                }
                // Removing Duplicates
                Tariff_Actual = Tariff_Actual.DefaultView.ToTable(true, "ID", "DestinationID", "Destination", "FromWeight", "ServiceID", "ToWeight", "Price", "addFactor");

                //Inserting
                count = InsertTariff(clvar, Tariff_Actual);
                if (count == 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Could Not Save Transaction. Please try Again')", true);
                    return;
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Tariff(s) Added')", true);


                    //string temp_ = error.ToString();

                    //string script_ = @"window.open(""{0}"", ""{1}"", ""{2}"");";
                    ////string script = String.Format(script_, "RunsheetInvoice.aspx?Xcode=" + temp_, "_blank", "");
                    //string script = String.Format(script_, "Print_Tariffs.aspx?ClientID=" + creditclientid.Value + "&ServiceType=" + dd_serviceType.SelectedValue, "_blank", "");
                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Redirect", script, true);


                    gv_tariff.DataSource = null;
                    gv_tariff.DataBind();

                    gv_tariff.DataSource = null;
                    gv_tariff.DataBind();


                    ViewState["dt"] = null;
                    Session["dt_"] = null;

                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "location.reload()", true);

                }

            }
            else
            {
                // con.InsertTariff(clvar, Tariff_Actual);

                count = InsertTariff(clvar, Tariff_Actual);
                if (count == 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Could Not Save Transaction. Please try Again')", true);
                    return;
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Tariff(s) Added')", true);

                    //string script_ = @"window.open(""{0}"", ""{1}"", ""{2}"");";
                    ////string script = String.Format(script_, "RunsheetInvoice.aspx?Xcode=" + temp_, "_blank", "");
                    //string script = String.Format(script_, "Print_Tariffs.aspx?ClientID=" + creditclientid.Value + "&ServiceType=" + dd_serviceType.SelectedValue, "_blank", "");
                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Redirect", script, true);


                    gv_tariff.DataSource = null;
                    gv_tariff.DataBind();

                    gv_tariff.DataSource = null;
                    gv_tariff.DataBind();


                    Session["dt"] = "";
                    ViewState["dt_"] = null;
                    Session["dt_"] = "";
                    Session["dt_1"] = "";

                    ViewState.Clear();


                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "location.reload()", true);








                }
            }


        }

        private DataTable RemoveDuplicates(DataTable dt)
        {

            if (dt.Rows.Count > 0)
            {
                for (int i = dt.Rows.Count - 1; i >= 0; i--)
                {
                    if (i == 0)
                    {
                        break;
                    }
                    for (int j = i - 1; j >= 0; j--)
                    {
                        if (dt.Rows[i]["DestinationID"] == dt.Rows[j]["DestinationID"] && dt.Rows[i]["Destination"].ToString() == dt.Rows[j]["Destination"].ToString() && dt.Rows[i]["ServiceID"].ToString() == dt.Rows[j]["ServiceID"].ToString() && dt.Rows[i]["FromWeight"].ToString() == dt.Rows[j]["FromWeight"].ToString())
                        {

                            //          new DataColumn("DestinationID"),
                            //new DataColumn("Destination"),
                            //new DataColumn("ServiceID"),
                            //new DataColumn("FromWeight"),
                            //new DataColumn("ToWeight"),
                            //new DataColumn("Price"),
                            //new DataColumn("AddFactor"),
                            dt.Rows[i].Delete();
                            break;
                        }
                    }
                }
                dt.AcceptChanges();
            }
            return dt;
        }

        protected void btn_cancel_Click(object sender, EventArgs e)
        {

        }

        protected void gv_tariff_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //if (txt_accountNo.Text == "0")
            //{
            //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cannot Update Cash Tariff')", true);
            //    err_msg.Text = "Cannot Update Cash Tariff";
            //    return;
            //}
            if (e.CommandName == "del")
            {
                clvar.TariffID = e.CommandArgument.ToString();
                if (clvar.TariffID == "")
                {
                    GridViewRow gr = (GridViewRow)sender;
                    gv_tariff.DeleteRow(gr.RowIndex);
                }

                int count = DeleteTariff(clvar);

                if (count != 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Tariff Deleted')", true);
                    DataTable dt = Session["dt_"] as DataTable;

                    DataRow[] dr = dt.Select("ID = '" + clvar.TariffID + "'", "");
                    dt.Rows.Remove(dr[0]);
                    dt.AcceptChanges();
                    Session["dt_"] = dt;
                    gv_tariff.DataSource = null;
                    gv_tariff.DataBind();
                    gv_tariff.DataSource = dt;
                    gv_tariff.DataBind();
                    return;
                }

                err_msg.Text = "Tariff has been Removed";
                err_msg.Visible = true;

            }
            if (e.CommandName == "Update")
            {

                clvar.TariffID = e.CommandArgument.ToString();
                GridViewRow gr = null;
                GridView grid = sender as GridView;
                int index = 0;

                index = Convert.ToInt32(e.CommandArgument);
                gr = grid.Rows[index - 1];
                ((Button)gv_tariff.Rows[index - 1].FindControl("btn_Update")).Enabled = false;

                //Now Updating The back Table

                string Same = ((TextBox)gr.FindControl("txt_Same")).Text;
                string Diff = ((TextBox)gr.FindControl("txt_Diff")).Text;
                string Local = ((TextBox)gr.FindControl("txt_Local")).Text;
                bool pr = ((CheckBox)gr.FindControl("cb_pr")).Checked;

                if (Same == "" || Same.Length == 0)
                {
                    Same = "0";
                }
                if (Diff == "" || Diff.Length == 0)
                {
                    Diff = "0";
                }
                if (Local == "" || Local.Length == 0)
                {
                    Local = "0";
                }

                DataTable dt = Session["dt_"] as DataTable;
                DataRow[] dr = dt.Select("ID = '" + clvar.TariffID + "'", "");

                dr[0]["Local"] = Local;
                dr[0]["Same"] = Same;
                dr[0]["Diff"] = Diff;
                dr[0]["Pr"] = pr;
                dr[0]["isUpdated"] = "NEW";


                dt.AcceptChanges();
                DataView tempview = dt.DefaultView;
                tempview.Sort = "FromWeight";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Tariff has been saved Temporarily')", true);
                //            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('Tariff has been saved Temporarily')", false);
                //  TextBox txt = (TextBox)gr.RowIndex[index].FindControl("TextBox4");


                //   ((HiddenField)gr.FindControl("hd_isupdated")).Value = "1";

                dt = tempview.ToTable();
                Session["dt_"] = dt;
                Session["dt_1"] = dt;
                gv_tariff.DataSource = dt;
                gv_tariff.DataBind();

                // Now Disabling the Textbox


                //   ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Selected Document is Credit Note.')", true);

                //err_msg.Text = "Tariff has been saved Temporarily";
                //err_msg.Visible = true;

            }
        }

        protected void ResetAll()
        {
            txt_accountNo.Text = "";
            txt_additionalWeight.Text = "";
            txt_clientName.Text = "";
            txt_fromWeight.Text = "";
            txt_fromZone.Text = "";
            //txt_price.Text = "";
            txt_toWeight.Text = "";
            //dd_branch.SelectedValue = "0";
            dd_serviceType.SelectedValue = "0";
            //     dd_toZone.SelectedValue = "0";
            gv_tariff.DataSource = null;
            gv_tariff.DataBind();

        }
        protected void btn_applyDefaultTariff_Click(object sender, EventArgs e)
        {
            if (txt_accountNo.Text == "0")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cannot Update Cash Tariff')", true);
                err_msg.Text = "Cannot Update Cash Tariff";
                return;
            }
            txt_accountNo_TextChanged(sender, e);
            clvar.CreditClientID = creditclientid.Value;
            clvar.ServiceTypeName = dd_serviceType.SelectedValue;
            clvar.Branch = dd_branch.SelectedValue;
            clvar.FromZoneCode = HttpContext.Current.Session["ZoneCode"].ToString();//dd_zone.SelectedValue;
        }

        public DataTable GetAccountDetailByAccountNumber(Cl_Variables clvar)
        {

            string query = "SELECT  z.name ZoneName, c.* FROM CREDITCLIENTS c inner join Zones z on z.zoneCode = c.zoneCode where c.ACCOUNTNO = '" + clvar.AccountNo + "' and c.ACCOUNTNO !='0'  and c.branchcode = '" + clvar.Branch + "' and c.isActive = '1'";
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

        protected void cb_pr_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox c = (CheckBox)sender;
            int count = 0;
            foreach (GridViewRow gr in gv_tariff.Rows)
            {
                CheckBox cb = (CheckBox)gr.FindControl("cb_pr");
                if (cb.Checked == true)
                {
                    count++;

                    if (count > 1)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('PR can only be selected once')", true);
                        //                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('PR can only be selected once')", true);
                        if (cb.ClientID == c.ClientID)
                        {
                            cb.Checked = false;
                            return;
                        }

                    }
                    else
                    {
                        //  (gr.FindControl("txt_Local") as TextBox).Enabled = true;
                    }
                }
            }


        }
        protected void gv_tariff_DataBound(object sender, EventArgs e)
        {

        }
        protected void gv_tariff_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string id = ((HiddenField)e.Row.FindControl("hd_id")).Value;// Id

                DataTable dt = Session["dt_"] as DataTable;
                DataRow[] dr = dt.Select("ID = '" + id + "'", "");
                if (dr.Length > 0)
                {
                    ((TextBox)e.Row.FindControl("txt_Same")).Text = dr[0]["Same"].ToString();
                    ((TextBox)e.Row.FindControl("txt_Diff")).Text = dr[0]["Diff"].ToString();
                    ((TextBox)e.Row.FindControl("txt_Local")).Text = dr[0]["Local"].ToString();

                    if (((HiddenField)e.Row.FindControl("hd_isupdated")).Value == "NEW")
                    {
                        //((TextBox)e.Row.FindControl("txt_Diff")).Enabled = false;

                        //((TextBox)e.Row.FindControl("txt_Local")).Enabled = false;
                        //((TextBox)e.Row.FindControl("txt_Same")).Enabled = false;
                        //((Button)e.Row.FindControl("btn_Update")).Enabled = false;
                        //(e.Row.FindControl("btn_Update") as Button).Enabled = false;

                        System.Web.UI.WebControls.Button btn = (System.Web.UI.WebControls.Button)e.Row.Cells[7].Controls[1];
                        btn.Enabled = false;
                        //   e.Row.Cells[7].Enabled = false;
                    }
                }
            }
        }

        public DataTable ZoneInfo()
        {
            string query = "SELECT  z.*,c.name ColorName from Zones z inner join Colors c on z.colorid = c.id where zone_type='1' and z.zonecode =  '" + Session["ZONECODE"].ToString() + "'";
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

        public DataTable ZoneInfo(string color)
        {
            string query = "SELECT  z.* from Zones z where zone_type='1' and z.zonecode <> '" + Session["ZONECODE"].ToString() + "' and colorid <> '" + color + "' AND z.zoneCode not IN ('16','14','17')";
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

        public DataTable ZoneInfo_Same(string color)
        {
            string query = "SELECT  z.* from Zones z where zone_type='1' and colorid = '" + color + "' and z.zoneCode not IN ('16','14','17')";
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


        public DataTable ZoneInfo_Pr()
        {
            string query = "SELECT  z.* from Zones z where zone_type='2' ";
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
        protected void gv_tariff_SelectedIndexChanged(object sender, EventArgs e)
        {

        }






        #region Class Methods

        #region Common Function
        public DataTable GetZonesForDomesticTariff()
        {
            //string query = "select zoneCode, name from Zones where status = '1' order by name";

            string query = "select z.zoneCode, z.name\n" +
            "  from Zones z\n" +
            " inner join Branches b\n" +
            "    on b.zoneCode = z.zoneCode\n" +
            " where z.status = '1'\n" +
            "   and b.branchCode <> '17'\n" +
            " group by z.colorId,\n" +
            "          z.createdBy,\n" +
            "          z.createdOn,\n" +
            "          z.description,\n" +
            "          z.email,\n" +
            "          z.faxNo,\n" +
            "          z.hasStore,\n" +
            "          z.modifiedBy,\n" +
            "          z.modifiedOn,\n" +
            "          z.name,\n" +
            "          z.phoneNo,\n" +
            "          z.status,\n" +
            "          z.type,\n" +
            "          z.zoneCode\n" +
            "union all \n" +
            " select z.zoneCode, z.name from Zones z where z.zoneCode in ('14','16','17','DIFF','LOCAL','SAME')\n" +
            "order by zoneCode";


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
        public DataSet Branch()
        {
            DataSet Ds_1 = new DataSet();

            try
            {
                string query = "SELECT b.branchCode, \n"
               + "       b.name     BranchName, sname BName \n"
               + "FROM   Branches                          b \n"
               + "where b.[status] ='1' \n"
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
        public DataSet ServiceTypeName()
        {
            DataSet Ds_1 = new DataSet();

            try
            {
                string query = "SELECT st.name ServiceTypeName \n"
                + "FROM   ServiceTypes     st \n"
                + "WHERE  st.IsIntl = '0' \n"
                + "       AND st.[status] = '1' \n"
                + "       And st.name not in ('Expressions','Road n Rail') \n"
                + "GROUP BY \n"
                + "       st.name \n"
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
        public DataTable GetTarrifForEdit_1(Cl_Variables clvar)
        {
            string sqlString = " select z.name ZoneName, t.* From tempClientTariff t \n" +
                "inner join Zones z\n" +
             "on z.zoneCode = t.ToZoneCode\n" +
             "where t.Client_Id = '" + clvar.CustomerClientID + "'\n" +
             " and t.ServiceID = '" + clvar.ServiceTypeName + "' \n" +
             "--and isintltariff = '0' \n" +
             "and chkdeleted = '0' \n" +
             "and t.branchCode = '" + clvar.Branch + "'\n" +
             "order by ZoneName, ToZoneCode, FromWeight";
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
        #endregion

        #region Consignments
        public DataTable GetZoneOfBranch(Cl_Variables clvar)
        {
            string query = "select * from Branches b where b.branchCode = '" + clvar.Branch + "'";
            DataTable dt = new DataTable();

            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd); orcd.CommandTimeout = 400;
                oda.Fill(dt);

                orcl.Close();
            }
            catch (Exception)
            { }
            return dt;
        }
        public int DeleteTariff(Cl_Variables clvar)
        {
            int count = 0;
            string error = "";
            //string query = "UPDATE TEMPCLIENTTARIFF SET chkDELETED = '1' where id = '" + clvar.TariffID + "'";
            string query = "UPDATE TEMPCLIENTTARIFF SET chkDELETED = '1', modifiedOn = GETDATE(), ModifiedBy = '" + HttpContext.Current.Session["U_ID"].ToString() + "'  where id = '" + clvar.TariffID + "' ";
            SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            try
            {
                sqlcon.Open();
                SqlCommand sqlcmd = new SqlCommand(query, sqlcon);
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
            return count;
        }

        public int InsertTariff(Cl_Variables clvar, DataTable dr)
        {

            clvar.FromZoneCode = GetZoneOfBranch(clvar).Rows[0]["ZoneCode"].ToString();
            string error = "";
            int count = 0;
            string query = "INSERT INTO tempClientTariff (tariffCode, Client_id, serviceID, BranchCode, FromZoneCode, ToZoneCode, FromWeight, ToWeight, Price, AdditionalFactor, AddtionalFactorSZ, AddtionalFactorDZ, chkDefaultTariff, chkDeleted, isIntlTariff,  CreatedOn,CreatedBy)\n";

            for (int i = 0; i < dr.Rows.Count - 1; i++)
            {
                query += "SELECT '0', " + clvar.CustomerClientID + ",'" + clvar.ServiceTypeName + "', '" + clvar.Branch + "', '" + clvar.FromZoneCode + "', '" + dr.Rows[i]["DestinationID"].ToString() + "', '" + dr.Rows[i]["FromWeight"].ToString() + "', '" + dr.Rows[i]["ToWeight"].ToString() + "', \n" +
                        "'" + dr.Rows[i]["Price"].ToString() + "', '" + dr.Rows[i]["AddFactor"].ToString() + "', '0', '0', '0', '0', '0', GETDATE(),'" + HttpContext.Current.Session["U_ID"].ToString() + "'\n" +
                        "UNION ALL \n";
            }
            int Row_ = dr.Rows.Count - 1;
            query += "SELECT '0', " + clvar.CustomerClientID + ",'" + clvar.ServiceTypeName + "', '" + clvar.Branch + "', '" + clvar.FromZoneCode + "', '" + dr.Rows[Row_]["DestinationID"].ToString() + "', '" + dr.Rows[Row_]["FromWeight"].ToString() + "', '" + dr.Rows[Row_]["ToWeight"].ToString() + "', \n" +
                        "'" + dr.Rows[Row_]["Price"].ToString() + "', '" + dr.Rows[Row_]["AddFactor"].ToString() + "', '0', '0', '0', '0', '0', GETDATE(),'" + HttpContext.Current.Session["U_ID"].ToString() + "'; ";
            query += " insert into tbl_TariffChange (accNo, creditclientid, modifiedby, modifiedon, recompute) values ((select accountno from CreditClients where id = " + clvar.CustomerClientID + "), " + clvar.CustomerClientID + ", '" + HttpContext.Current.Session["U_ID"].ToString() + "', getdate(), 0); ";
            SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            try
            {
                sqlcon.Open();
                SqlCommand sqlcmd = new SqlCommand(query, sqlcon);
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
            return count;

        }
        #endregion

        #endregion
        protected void gv_tariff__RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "del")
            {
                clvar.TariffID = e.CommandArgument.ToString();
                if (clvar.TariffID == "")
                {
                    GridViewRow gr = (GridViewRow)sender;
                    gv_tariff.DeleteRow(gr.RowIndex);
                }
                //DataRow[] dr = dt.Select("ID = '" + clvar.TariffID + "'", "");
                int count = con.DeleteTariff(clvar);

                if (count != 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Tariff Deleted')", true);
                    btn_showTariff_Click(sender, e);
                }

            }

        }
    }
}