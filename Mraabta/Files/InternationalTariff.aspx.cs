using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using MRaabta.App_Code;

namespace MRaabta.Files
{
    public partial class InternationalTariff : System.Web.UI.Page
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
                Currencies();
                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[]
                {
                new DataColumn("ID"),
                new DataColumn("DestinationID"),
                new DataColumn("Destination"),
                new DataColumn("FromWeight", typeof(double)),
                new DataColumn("ToWeight", typeof(double)),
                new DataColumn("Price"),
                new DataColumn("AddFactor"),
                new DataColumn("isUpdated"),
                new DataColumn("addFactorPrice")
                });
                dt.AcceptChanges();
                ViewState["dt"] = dt;
            }
        }


        protected void Branches()
        {
            DataTable dt = fun.Branch().Tables[0];


            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    DataRow[] dr = new DataRow[1];
                    if (Session["BranchCode"].ToString().ToUpper() != "ALL")
                    {
                        dr = dt.Select("BranchCode='" + Session["BranchCode"].ToString() + "'", "");
                        dd_branch.DataSource = dr.CopyToDataTable();
                    }
                    else
                    {
                        dd_branch.DataSource = dt;
                    }


                    dd_branch.DataTextField = "BranchName";
                    dd_branch.DataValueField = "branchCode";
                    dd_branch.DataBind();


                }
            }
        }

        protected void ServiceTypes()
        {
            DataTable dt = fun.InternationalServiceTypes();
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    dd_serviceType.DataSource = dt;
                    dd_serviceType.DataTextField = "ServiceTypeName";
                    dd_serviceType.DataValueField = "ServiceTypeName";
                    dd_serviceType.DataBind();
                    //dd_serviceType.SelectedValue = Session["BranchCode"].ToString();
                }
            }
        }

        protected void Zones()
        {
            DataTable dt = fun.GetInternationalZones();
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    dd_zone.DataSource = dt;
                    dd_zone.DataTextField = "name";
                    dd_zone.DataValueField = "zoneCode";
                    dd_zone.DataBind();
                    //dd_serviceType.SelectedValue = Session["BranchCode"].ToString();
                }
            }
        }

        protected void Currencies()
        {
            DataTable dt = fun.GetCurrencies();
            ViewState["currencies"] = dt;
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    dd_currency.DataSource = dt;
                    dd_currency.DataTextField = "CODE";
                    dd_currency.DataValueField = "id";
                    dd_currency.DataBind();
                    txt_currency.Text = dt.Rows[0]["name"].ToString();
                    //dd_serviceType.SelectedValue = Session["BranchCode"].ToString();
                }
            }


        }

        protected void txt_accountNo_TextChanged(object sender, EventArgs e)
        {
            clvar.Branch = dd_branch.SelectedValue;
            clvar.AccountNo = txt_accountNo.Text;
            DataTable dt = fun.GetAccountDetailByAccountNumber(clvar);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    txt_fromZone.Text = dt.Rows[0]["ZoneNAME"].ToString();
                    txt_clientName.Text = dt.Rows[0]["NAME"].ToString();
                    creditclientid.Value = dt.Rows[0]["ID"].ToString();
                }
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

            //if (dd_zone.SelectedValue == "0")
            //{
            //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Select To Zone')", true);

            //    return;
            //}
            #endregion


            clvar.CustomerClientID = creditclientid.Value;
            clvar.ServiceTypeName = dd_serviceType.SelectedValue;
            //    clvar.ToZoneCode = dd_zone.SelectedValue;
            DataTable tariff = ViewState["dt"] as DataTable;
            tariff.Clear();
            DataTable dt = fun.GetInternationalTarrifForEdit_1(clvar);
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
                        dr["Price"] = row["Price"].ToString();
                        dr["addFactorPrice"] = row["AddtionalFactorDZ"].ToString();
                        tariff.Rows.Add(dr);
                        tariff.AcceptChanges();
                    }
                    btn_Default.Enabled = false;
                }
                else
                {
                    //clvar.CustomerClientID = "0";
                    //dt = fun.GetTarrifForEdit(clvar);
                    //if (dt.Rows.Count > 0)
                    //{
                    //    foreach (DataRow row in dt.Rows)
                    //    {
                    //        DataRow dr = tariff.NewRow();
                    //        dr["ID"] = "";
                    //        dr["DestinationID"] = row["TOZONECODE"].ToString();
                    //        dr["Destination"] = row["ZONENAME"].ToString();
                    //        dr["FromWeight"] = row["FromWeight"].ToString();
                    //        dr["ToWeight"] = row["toWeight"].ToString();
                    //        dr["addFactor"] = row["AdditionalFactor"].ToString();
                    //        dr["Price"] = row["Price"].ToString();
                    //        tariff.Rows.Add(dr);
                    //        tariff.AcceptChanges();
                    //    }
                    //}
                    btn_Default.Enabled = true;
                }
            }

            if (tariff != null)
            {
                if (tariff.Rows.Count > 0)
                {
                    gv_tariff.DataSource = tariff;
                    gv_tariff.DataBind();
                }
            }

            ViewState["dt"] = tariff;
        }
        protected void btn_addWeight_Click(object sender, EventArgs e)
        {
            if (txt_accountNo.Text.Trim() == "0")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cannot Update Cash Tariff')", true);
                err_msg.Text = "Cannot Update Cash Tariff";
                return;
            }
            DataTable tariff = ViewState["dt"] as DataTable;

            clvar.AdditionalFactor = txt_addWeight.Text;

            foreach (DataRow row in tariff.Rows)
            {
                row["addFactor"] = clvar.AdditionalFactor;

                if (row["isUpdated"].ToString() != "NEW")
                {
                    row["isUpdated"] = "YES";
                }
            }
            tariff.AcceptChanges();
            gv_tariff.DataSource = null;
            gv_tariff.DataBind();
            gv_tariff.DataSource = tariff;
            gv_tariff.DataBind();
        }
        protected void btn_addPrice_Click(object sender, EventArgs e)
        {
            if (txt_accountNo.Text.Trim() == "0")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cannot Update Cash Tariff')", true);
                err_msg.Text = "Cannot Update Cash Tariff";
                return;
            }
            #region validations
            if (txt_fromWeight.Text.Trim() == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter From Weight')", true);
                return;
            }
            if (txt_toWeight.Text.Trim() == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter To Weight')", true);
                return;
            }
            if (txt_price.Text.Trim() == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter Price')", true);
                return;
            }
            #endregion
            DataTable tariff = ViewState["dt"] as DataTable;
            clvar.ToWeight = txt_toWeight.Text;
            clvar.FromWeight = txt_fromWeight.Text;
            clvar.ToZoneCode = dd_zone.SelectedValue;
            clvar.amount = double.Parse(txt_price.Text);
            //clvar.AdditionalFactor = tariff.Rows[0]["addFactor"].ToString();
            DataRow dr = tariff.NewRow();
            bool flag = false;
            DataRow[] drs = tariff.Select("DestinationID = '" + clvar.ToZoneCode + "'", "");
            if (drs.Count() != 0)
            {
                DataTable temp = drs.CopyToDataTable();
                //DataRow[] tempRows = temp.Select("FromWeight <= " + clvar.FromWeight + " AND ToWeight > " + clvar.FromWeight + "", "");
                //if (tempRows.Count() != 0)
                //{
                //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cannot Add Selected Slab')", true);
                //    return;
                //}
                //tempRows = temp.Select("ToWeight > " + clvar.ToWeight + "", "");
                //if (tempRows.Count() != 0)
                //{
                //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cannot Add Selected Slab')", true);
                //    return;
                //}

                dr["ID"] = "";
                dr["DestinationID"] = dd_zone.SelectedValue;
                dr["Destination"] = dd_zone.SelectedItem.Text;
                dr["FromWeight"] = clvar.FromWeight;
                dr["ToWeight"] = clvar.ToWeight;
                dr["Price"] = txt_price.Text;
                if (txt_addWeight.Text.Trim() != "")
                {

                    dr["addFactor"] = txt_addWeight.Text;
                    dr["addFactorPrice"] = txt_addPrice.Text;
                }
                else
                {
                    dr["addFactor"] = tariff.Rows[0]["addFactor"].ToString();
                    dr["addFactorPrice"] = tariff.Rows[0]["addFactorPrice"].ToString();
                }


                dr["isUpdated"] = "NEW";
                tariff.Rows.Add(dr);
                tariff.AcceptChanges();
                //tariff = tariff.Select("", "order by 1, 3").CopyToDataTable();
                DataView tempview = tariff.DefaultView;
                tempview.Sort = "Destination, DestinationID, FromWeight";

                tariff = tempview.ToTable();
                ViewState["dt"] = tariff;
                gv_tariff.DataSource = null;
                gv_tariff.DataBind();
                gv_tariff.DataSource = tariff;
                gv_tariff.DataBind();
            }
            else
            {
                clvar.FromWeight = txt_fromWeight.Text;
                clvar.ToWeight = txt_toWeight.Text;
                dr["ID"] = "";
                dr["DestinationID"] = dd_zone.SelectedValue;
                dr["Destination"] = dd_zone.SelectedItem.Text;
                dr["FromWeight"] = clvar.FromWeight;
                dr["ToWeight"] = clvar.ToWeight;
                dr["Price"] = txt_price.Text;
                if (txt_addWeight.Text.Trim() != "")
                {

                    dr["addFactor"] = txt_addWeight.Text;
                    dr["addFactorPrice"] = txt_addPrice.Text;
                }
                else
                {
                    dr["addFactor"] = "0";
                    dr["addFactorPrice"] = "0";
                }
                dr["isUpdated"] = "NEW";
                tariff.Rows.Add(dr);
                tariff.AcceptChanges();
                //tariff = tariff.Select("", "order by 1, 3").CopyToDataTable();
                DataView tempview = tariff.DefaultView;
                tempview.Sort = "Destination, DestinationID, FromWeight";

                tariff = tempview.ToTable();
                ViewState["dt"] = tariff;
                gv_tariff.DataSource = null;
                gv_tariff.DataBind();
                gv_tariff.DataSource = tariff;
                gv_tariff.DataBind();
            }
            txt_fromWeight.Text = "";
            txt_toWeight.Text = "";
            txt_price.Text = "";
            txt_addPrice.Text = "";
            txt_addWeight.Text = "";
            txt_fromWeight.Focus();


        }
        protected void gv_tariff_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "del")
            {
                if (txt_accountNo.Text.Trim() == "0")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cannot Update Cash Tariff')", true);
                    err_msg.Text = "Cannot Update Cash Tariff";
                    return;
                }
                clvar.TariffID = e.CommandArgument.ToString();
                int count = con.DeleteTariff(clvar);
                if (clvar.TariffID.Trim() == "")
                {
                    DataTable dt = ViewState["dt"] as DataTable;
                    GridViewRow row = (GridViewRow)(((Button)e.CommandSource).NamingContainer);

                    //new DataColumn("DestinationID"),
                    //new DataColumn("Destination"),
                    //new DataColumn("FromWeight", typeof(double)),
                    //new DataColumn("ToWeight", typeof(double)),
                    //new DataColumn("Price"),
                    //new DataColumn("AddFactor"),
                    //new DataColumn("isUpdated"),
                    //new DataColumn("addFactorPrice")

                    string criteria = "";
                    criteria = "ID = '" + clvar.TariffID + "' ";
                    criteria += "AND DestinationID = '" + (row.FindControl("hd_destinationID") as HiddenField).Value + "' ";
                    criteria += "AND FromWeight = '" + double.Parse(row.Cells[1].Text) + "' ";
                    criteria += "AND ToWeight = '" + double.Parse(row.Cells[2].Text) + "' ";
                    criteria += "AND Price = '" + row.Cells[3].Text.ToString() + "' ";
                    criteria += "AND AddFactor = '" + row.Cells[4].Text.ToString() + "' ";
                    criteria += "AND addFactorPrice = '" + row.Cells[5].Text + "'";

                    DataRow[] dr = dt.Select(criteria, "");
                    dt.Rows.Remove(dr[0]);
                    dt.AcceptChanges();
                    gv_tariff.DataSource = null;
                    gv_tariff.DataBind();
                    gv_tariff.DataSource = dt;
                    gv_tariff.DataBind();
                    return;
                }
                else if (count != 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Tariff Deleted')", true);
                    DataTable dt = ViewState["dt"] as DataTable;

                    DataRow[] dr = dt.Select("ID = '" + clvar.TariffID + "'", "");
                    dt.Rows.Remove(dr[0]);
                    dt.AcceptChanges();
                    gv_tariff.DataSource = null;
                    gv_tariff.DataBind();
                    gv_tariff.DataSource = dt;
                    gv_tariff.DataBind();
                    return;
                }


            }
        }
        protected void btn_reset_Click(object sender, EventArgs e)
        {
            ResetAll();
        }
        protected void btn_save_Click(object sender, EventArgs e)
        {
            if (txt_accountNo.Text.Trim() == "0")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cannot Update Cash Tariff')", true);
                err_msg.Text = "Cannot Update Cash Tariff";
                return;
            }
            clvar.CustomerClientID = creditclientid.Value;
            clvar.ServiceTypeName = dd_serviceType.SelectedValue;
            clvar.Branch = dd_branch.SelectedValue;
            //clvar.FromZoneCode = dd_zone.SelectedValue;
            clvar.CurrencyCode = dd_currency.SelectedValue;
            DataTable tarrif = ViewState["dt"] as DataTable;
            List<DataRow> dr = new List<DataRow>();
            int count = 0;
            foreach (DataRow item in tarrif.Rows)
            {
                if (item["ID"].ToString().Trim(' ') == "")
                {
                    dr.Add(item);
                    //count++;
                }
            }
            List<DataRow> drToBeUpdated = new List<DataRow>();
            foreach (DataRow row in tarrif.Rows)
            {
                if (row["isUpdated"].ToString() == "YES")
                {
                    drToBeUpdated.Add(row);
                }
            }
            if (dr.Count > 0)
            {
                count = con.InsertInternationalTariff(clvar, dr);
                if (count == 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Could Not Save Transaction. Please try Again')", true);
                    return;
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Tariff(s) Added')", true);


                }
            }

            if (drToBeUpdated.Count > 0)
            {
                count = 0;
                count = con.UpdateInternationalTarrif(clvar, drToBeUpdated);
                if (count == 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Could Not Save Transaction. Please try Again')", true);
                    return;
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Tariff(s) Added')", true);

                }
            }
            btn_showTariff_Click(sender, e);

            string script_ = @"window.open(""{0}"", ""{1}"", ""{2}"");";
            //string script = String.Format(script_, "RunsheetInvoice.aspx?Xcode=" + temp_, "_blank", "");
            string script = String.Format(script_, "Print_Tariffs.aspx?ClientID=" + creditclientid.Value + "&ServiceType=" + dd_serviceType.SelectedValue, "_blank", "");
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Redirect", script, true);


        }
        protected void btn_cancel_Click(object sender, EventArgs e)
        {

        }

        protected void ResetAll()
        {
            txt_accountNo.Text = "";
            //txt_additionalWeight.Text = "";
            txt_clientName.Text = "";
            txt_fromWeight.Text = "";
            txt_fromZone.Text = "";
            txt_price.Text = "";
            txt_toWeight.Text = "";
            dd_branch.SelectedValue = "0";
            dd_serviceType.SelectedValue = "0";
            dd_zone.SelectedValue = "0";
            gv_tariff.DataSource = null;
            gv_tariff.DataBind();

        }
        protected void dd_currency_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = ViewState["currencies"] as DataTable;

            DataRow[] dr = dt.Select("id = '" + dd_currency.SelectedValue + "'");

            txt_currency.Text = dr[0]["name"].ToString();
        }

        protected void btn_Default_Click(object sender, EventArgs e)
        {
            #region

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

            //if (dd_zone.SelectedValue == "0")
            //{
            //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Select To Zone')", true);

            //    return;
            //}
            #endregion

            clvar.Branch = dd_branch.SelectedValue;
            clvar.AccountNo = "0";// creditclientid.Value;
            clvar.ServiceTypeName = dd_serviceType.SelectedValue;
            clvar.ToZoneCode = dd_zone.SelectedValue;
            DataTable tariff = ViewState["dt"] as DataTable;
            tariff.Clear();
            DataTable dt = fun.GetInternationalTarrifForEdit_2(clvar);
            gv_tariff.DataSource = null;
            gv_tariff.DataBind();
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        DataRow dr = tariff.NewRow();
                        dr["ID"] = "";// row["ID"].ToString();
                        dr["DestinationID"] = row["TOZONECODE"].ToString();
                        dr["Destination"] = row["ZONENAME"].ToString();
                        dr["FromWeight"] = row["FromWeight"].ToString();
                        dr["ToWeight"] = row["toWeight"].ToString();
                        dr["addFactor"] = row["AdditionalFactor"].ToString();
                        dr["Price"] = row["Price"].ToString();
                        dr["addFactorPrice"] = row["AddtionalFactorDZ"].ToString();
                        tariff.Rows.Add(dr);
                        tariff.AcceptChanges();
                    }
                }
                else
                {
                    //clvar.CustomerClientID = "0";
                    //dt = fun.GetTarrifForEdit(clvar);
                    //if (dt.Rows.Count > 0)
                    //{
                    //    foreach (DataRow row in dt.Rows)
                    //    {
                    //        DataRow dr = tariff.NewRow();
                    //        dr["ID"] = "";
                    //        dr["DestinationID"] = row["TOZONECODE"].ToString();
                    //        dr["Destination"] = row["ZONENAME"].ToString();
                    //        dr["FromWeight"] = row["FromWeight"].ToString();
                    //        dr["ToWeight"] = row["toWeight"].ToString();
                    //        dr["addFactor"] = row["AdditionalFactor"].ToString();
                    //        dr["Price"] = row["Price"].ToString();
                    //        tariff.Rows.Add(dr);
                    //        tariff.AcceptChanges();
                    //    }
                    //}
                }
            }

            if (tariff != null)
            {
                if (tariff.Rows.Count > 0)
                {
                    gv_tariff.DataSource = tariff;
                    gv_tariff.DataBind();
                }
            }

            ViewState["dt"] = tariff;

        }
    }
}