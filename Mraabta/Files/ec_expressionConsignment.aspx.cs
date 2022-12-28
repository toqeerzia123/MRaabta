using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using MRaabta.App_Code;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
using System.Net;
using System.Text;
using System.IO;

namespace MRaabta.Files
{
    public partial class ec_expressionConsignment : System.Web.UI.Page
    {
        Cl_Variables clvar = new Cl_Variables();
        Variable clvar1 = new Variable();
        Consignemnts con = new Consignemnts();
        CommonFunction CF = new CommonFunction();
        ExpressionConsignment EC = new ExpressionConsignment();

        protected void Page_Load(object sender, EventArgs e)
        {
            Errorid.Text = "";
            if (!IsPostBack)
            {
                //btn_save.Visible = false;
                //btn_update.Visible = false;


                gv_items.DataSource = null;
                gv_items.DataBind();
                //BindCities();
                Get_Destination();
                Get_Items();

                DataTable dt_items = new DataTable();
                //   dt_items.Columns.Add("ID", typeof(string));
                dt_items.Columns.Add("Description");
                dt_items.Columns.Add("id");
                dt_items.Columns.Add("consignementNo", typeof(string));
                dt_items.Columns.Add("itemId", typeof(Int64));
                dt_items.Columns.Add("Qty", typeof(float));
                dt_items.Columns.Add("amount", typeof(float));
                dt_items.Columns.Add("message", typeof(string));
                dt_items.Columns.Add("status", typeof(string));




                dt_items.Columns.Add("gst", typeof(float));
                dt_items.Columns.Add("serviceCharges", typeof(float));

                ViewState["PM"] = dt_items;

                dt_DeliveryDatetime.SelectedDate = DateTime.Now.Date;
                //txt_accountReceivingDate.Text = DateTime.Now.Date.ToString("yyyy-MM-dd");
                booking_date.Text = DateTime.Now.Date.ToString("yyyy-MM-dd");
                //dt_DeliveryDatetime.MinDate = DateTime.Now.Date;



                clvar.origin = HttpContext.Current.Session["BranchCode"].ToString();
                DataSet BranchGSTInformation = con.BranchGSTInformation(clvar);
                ViewState["Bgst"] = BranchGSTInformation.Tables[0].Rows[0]["Gst"].ToString();

                //ProductsPricesClientSide();

                //gv_items.DataSource = dt_items;
                //gv_items.DataBind();

                //RepeaterPopulate();
            }
        }

        public void RepeaterPopulate()
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[] {
        new DataColumn("itemID"),
        new DataColumn("Description"),
        new DataColumn("itemQty"),
        new DataColumn("itemAmount"),
        new DataColumn("itemGST")
        });

            dt.Rows.Add(dt.NewRow());


        }
        public void ProductsPricesClientSide()
        {
            DataTable dt = (DataTable)ViewState["Expressions"];

            string gst = ViewState["Bgst"].ToString();
            foreach (DataRow row in dt.Rows)
            {
                string itemID = row["ID"].ToString();
                string rate = row["rate"].ToString();
                double itemRate = 0;
                double Gst = 0;

                double.TryParse(rate, out itemRate);
                double.TryParse(gst, out Gst);

                double itemAmount = Math.Round(itemRate - ((itemRate / (Gst + 100)) * Gst), 2);
                //hd_itemGst.Value = (((itemAmount / (gst + 100)) * gst) * qty).ToString();

                double itemGst = Math.Round((((itemRate / (Gst + 100)) * Gst)), 2);

                HtmlTableRow htmlRow = new HtmlTableRow();
                HtmlTableCell cell = new HtmlTableCell();
                cell.InnerText = itemID;
                htmlRow.Cells.Add(cell);

                cell = new HtmlTableCell();
                cell.InnerText = itemAmount.ToString();
                htmlRow.Cells.Add(cell);

                cell = new HtmlTableCell();
                cell.InnerText = itemGst.ToString();
                htmlRow.Cells.Add(cell);

                cell = new HtmlTableCell();
                cell.InnerText = "0";
                htmlRow.Cells.Add(cell);

                tbl_productPrices.Rows.Add(htmlRow);

            }

            HtmlTableRow htrow = new HtmlTableRow();
            HtmlTableCell htcell = new HtmlTableCell();
            htrow.Cells.Add(htcell);
            htrow.Cells.Add(htcell);
            htrow.Cells.Add(htcell);
            htrow.Cells.Add(htcell);
            htrow.Cells.Add(htcell);
            htrow.Cells.Add(htcell);
            //tbl_items.Rows.Add(htrow);

        }

        public DataTable Cities_()
        {

            string sqlString = "SELECT CAST(A.bid as Int) bid, A.description NAME, A.expressCenterCode, A.CategoryID\n" +
            "  FROM (SELECT e.expresscentercode, e.bid, e.name description, e.CategoryId\n" +
            "          FROM ServiceArea a\n" +
            "         right outer JOIN ExpressCenters e\n" +
            "            ON e.expressCenterCode = a.Ec_code where e.bid <> '17' and e.status = '1') A\n" +
            " GROUP BY A.bid, A.description, A.expressCenterCode, A.CategoryID\n" +
            " ORDER BY 2";

            sqlString = "select b.branchCode , b.sname + ' - ' + b.name Sname , ec.ExpressCenterCode ECCode\n" +
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
        public void Get_Destination()
        {
            DataTable ds = Cities_(); //CF.Branch();
            if (ds != null)
            {
                //  cb_Destination
                //  this.cb_Destination.Items.Add(new DropDownListItem("Select Destination", "0"));
                this.cb_Destination.DataTextField = "SNAME";
                this.cb_Destination.DataValueField = "BranchCode";
                this.cb_Destination.DataSource = ds.DefaultView;
                this.cb_Destination.DataBind();
                ViewState["Cities"] = ds;
            }
        }

        public void Get_Items()
        {
            DataSet ds = CF.ExpressionItems();
            if (ds.Tables.Count != 0)
            {
                //  cb_Destination
                //this.dd_item.Items.Add(new DropDownListItem("Select Item", "0"));
                dd_item.DataSource = ds.Tables[0];
                dd_item.DataTextField = "NAME";
                dd_item.DataValueField = "ID";
                dd_item.DataBind();

                ViewState["Expressions"] = ds.Tables[0];
            }
        }

        //protected void BindCities()
        //{
        //    string query = "SELECT * FROM CITIES WHERE isActive = '1'";
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
        //    if (dt == null)
        //    {
        //        return;
        //    }
        //    if (dt.Rows.Count > 0)
        //    {
        //        //dd_destination.DataSource = dt;
        //        //dd_destination.DataTextField = "CITYNAME";
        //        //dd_destination.DataValueField = "ID";
        //        //dd_destination.DataBind();
        //    }
        //}
        //protected void BindItems()
        //{
        //    string query = "select CODE, code + ' -- ' + name as NAME FROM ExpressionProduct where status = '1'";
        //    DataTable dt = new DataTable();
        //    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        //    try
        //    {
        //        con.Open();
        //        SqlDataAdapter sda = new SqlDataAdapter(query, con);
        //        sda.Fill(dt);

        //        ViewState["items"] = dt;
        //    }
        //    catch (Exception ex)
        //    { }
        //    finally { con.Close(); }
        //    if (dt == null)
        //    {
        //        return;
        //    }
        //    if (dt.Rows.Count > 0)
        //    {
        //        dd_item.DataSource = dt;
        //        dd_item.DataTextField = "NAME";
        //        dd_item.DataValueField = "CODE";
        //        dd_item.DataBind();
        //    }
        //}

        protected void btn_add_Click(object sender, EventArgs e)
        {

            //Branch Information
            clvar.origin = HttpContext.Current.Session["BranchCode"].ToString();
            DataSet BranchGSTInformation = con.BranchGSTInformation(clvar);
            string gst = "";
            if (BranchGSTInformation.Tables[0].Rows.Count != 0)
            {
                gst = BranchGSTInformation.Tables[0].Rows[0]["gst"].ToString();
            }

            DataTable dt_item = (DataTable)ViewState["PM"];
            DataTable dt = (DataTable)ViewState["Expressions"];
            string Itemid = dd_item.SelectedValue;
            DataRow[] dr_ = dt_item.Select("itemid ='" + HiddenField2.Value + "'");
            if (dr_.Length == 0)
            {
                DataRow[] itemRow = dt.Select("id = '" + Itemid + "'");
                DataRow dr = dt_item.NewRow();

                dr[0] = Int64.Parse("0");
                dr["Description"] = itemRow[0]["Name"].ToString();
                dr["consignementNo"] = txt_ConNo.Text;
                dr["itemId"] = Int64.Parse(Itemid);
                dr["Qty"] = float.Parse(txt_qty.Text);
                dr["amount"] = float.Parse(txt_amount.Text);
                dr["message"] = txt_message.Text;
                dr["status"] = "2";
                dr["gst"] = Math.Round(double.Parse(hd_itemGst.Value), 2).ToString(); //((float.Parse(gst) / 100) * float.Parse(itemRow[0]["serviceCharges"].ToString())) * float.Parse(txt_qty.Text);// gst;
                dr["serviceCharges"] = float.Parse(itemRow[0]["serviceCharges"].ToString()) * float.Parse(txt_qty.Text);// "100";

                dt_item.Rows.Add(dr);
                dt_item.AcceptChanges();
                ViewState["PM"] = dt_item;
                //LoadGrid();
            }


            if (dt_item.Rows.Count > 0)
            {
                gv_items.DataSource = dt_item;///////////////
                gv_items.DataBind();/////////////////////////
            }
            double Modfier = 0;
            double TotalGstAmount = 0;

            double TotalAmount = 0;
            double TotalServiceCharges = 0;



            foreach (DataRow dr in dt_item.Rows)
            {
                TotalAmount += float.Parse(dr["Amount"].ToString());
                TotalGstAmount += float.Parse(dr["gst"].ToString());
                TotalServiceCharges += float.Parse(dr["serviceCharges"].ToString());
            }





            txt_chargedRate.Text = TotalAmount.ToString();
            txt_chargesWalaOtherCharges.Text = TotalServiceCharges.ToString();
            txt_gst.Text = TotalGstAmount.ToString();
            txt_totalAmount.Text = (TotalAmount + TotalGstAmount).ToString();
            txt_qty.Text = "";
            txt_amount.Text = "";

            //foreach (GridViewRow rp in gv_items.Rows)
            //{
            //    if (rp.Visible == true)
            //    {
            //        double Amount = 0;

            //        double ServiceCharges = 0;
            //        double GstAmount = 0;

            //        Label tx = ((Label)rp.FindControl("lbl_Amount"));
            //        Modfier = double.Parse(tx.Text);

            //        ServiceCharges = 100;
            //        GstAmount = (ServiceCharges / 100) * double.Parse(gst);
            //        Amount = (Modfier) + ServiceCharges + GstAmount;

            //        TotalServiceCharges += ServiceCharges;
            //        TotalGstAmount += GstAmount;
            //        TotalAmount += Amount;
            //    }


            //}

            //txt_chargedRate.Text = string.Format("{0:N2}", TotalAmount - TotalGstAmount - TotalServiceCharges);
            //txt_chargesWalaOtherCharges.Text = string.Format("{0:N2}", TotalServiceCharges);
            //txt_gst.Text = string.Format("{0:N2}", TotalGstAmount);
            //txt_totalAmount.Text = string.Format("{0:N2}", TotalAmount);

        }

        protected void txt_ConNo_TextChanged(object sender, EventArgs e)
        {
            DataTable dt_items = new DataTable();
            //   dt_items.Columns.Add("ID", typeof(string));
            dt_items.Columns.Add("Description");
            dt_items.Columns.Add("id");
            dt_items.Columns.Add("consignementNo", typeof(string));
            dt_items.Columns.Add("itemId", typeof(Int64));
            dt_items.Columns.Add("Qty", typeof(float));
            dt_items.Columns.Add("amount", typeof(float));
            dt_items.Columns.Add("message", typeof(string));
            dt_items.Columns.Add("status", typeof(string));




            dt_items.Columns.Add("gst", typeof(float));
            dt_items.Columns.Add("serviceCharges", typeof(float));

            ViewState["PM"] = dt_items;

            clvar = new Cl_Variables();
            clvar.consignmentNo = txt_ConNo.Text;

            DataSet ds = GetConsignmentForApproval(clvar);
            DataTable dt = EC.ConsignmentExpressionDetail(clvar);

            if (ds.Tables[0].Rows.Count != 0)
            {
                hd_portalCn.Value = ds.Tables[0].Rows[0]["PortalCN"].ToString();
                if (hd_portalCn.Value == "1" || hd_portalCn.Value.ToUpper() == "TRUE")
                {
                    Alert("This Consignment is Reserved for COD Portal Only");
                    return;

                }




                if (ds.Tables[0].Rows[0]["ExpressionDeliveryDateTime"].ToString() == null || ds.Tables[0].Rows[0]["ExpressionDeliveryDateTime"].ToString().Trim() == "")
                {
                    dt_DeliveryDatetime.SelectedDate = DateTime.Now;
                }
                else
                {
                    dt_DeliveryDatetime.SelectedDate = DateTime.Parse(ds.Tables[0].Rows[0]["ExpressionDeliveryDateTime"].ToString());
                }

                txt_ConNo.Text = ds.Tables[0].Rows[0]["consignmentNumber"].ToString();
                txt_AccNo.Text = ds.Tables[0].Rows[0]["consignerAccountNo"].ToString();
                hd_OriginExpressCenter.Value = ds.Tables[0].Rows[0]["originExpressCenter"].ToString();
                if (ds.Tables[0].Rows[0]["customerType"].ToString() == "1")
                {
                    rbtn_customerType.SelectedValue = "1";
                }

                if (ds.Tables[0].Rows[0]["customerType"].ToString() == "2")
                {
                    rbtn_customerType.SelectedValue = "2";
                }

                if (ds.Tables[0].Rows[0]["destinationExpressCenterCode"].ToString() != "")
                {
                    cb_Destination.SelectedValue = ds.Tables[0].Rows[0]["destination"].ToString();
                }
                txt_RiderCode.Text = ds.Tables[0].Rows[0]["riderCode"].ToString();
                txt_pieces.Text = ds.Tables[0].Rows[0]["pieces"].ToString();
                txt_otherCharges.Text = ds.Tables[0].Rows[0]["otherCharges"].ToString();
                txt_packageContents.Text = ds.Tables[0].Rows[0]["PakageContents"].ToString();

                txt_ConsignerCellNo.Text = ds.Tables[0].Rows[0]["consignerCellNo"].ToString();
                Txt_ConsignerCNIC.Text = ds.Tables[0].Rows[0]["consignerCNICNo"].ToString();
                txt_Consigner.Text = ds.Tables[0].Rows[0]["consigner"].ToString();
                txt_ShipperAddress.Text = ds.Tables[0].Rows[0]["shipperAddress"].ToString();

                txt_Consignee.Text = ds.Tables[0].Rows[0]["consignee"].ToString();
                txt_Address.Text = ds.Tables[0].Rows[0]["address"].ToString();
                txt_ConsigneeCellno.Text = ds.Tables[0].Rows[0]["consigneePhoneNo"].ToString();

                txt_chargedRate.Text = ds.Tables[0].Rows[0]["totalAmount"].ToString();
                txt_gst.Text = ds.Tables[0].Rows[0]["gst"].ToString();
                double temptamt = 0;
                double tempgst = 0;
                double.TryParse(ds.Tables[0].Rows[0]["totalAmount"].ToString(), out temptamt);
                double.TryParse(ds.Tables[0].Rows[0]["gst"].ToString(), out tempgst);
                txt_totalAmount.Text = Math.Round((temptamt + tempgst), 2).ToString();
                txt_message.Text = ds.Tables[0].Rows[0]["expressionMessage"].ToString();

                booking_date.Text = DateTime.Parse(ds.Tables[0].Rows[0]["bookingDate"].ToString()).ToString("yyyy-MM-dd");
                if (ds.Tables[0].Rows[0]["AccountReceivingDate"].ToString() != "")
                {
                    //txt_accountReceivingDate.Text = DateTime.Parse(ds.Tables[0].Rows[0]["AccountReceivingDate"].ToString()).ToString("yyyy-MM-dd");
                }
                else
                {
                    //txt_accountReceivingDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                }

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr["invoiceNumber_"].ToString() != "")
                    {
                        //txt_invoiceNumber.Text = dr["invoiceNumber_"].ToString();
                        //hd_unApproveable.Value = "0";
                        break;
                    }
                }
                //booking_date.Enabled = false;

                //txt_message.Text = ds.Tables[0].Rows[0]["expressionDeliveryDateTime"].ToString();
                //dt_DeliveryDatetime

                //for (int i = 0; i <= dt.Rows.Count; i++)
                //{
                //    gv_items.DataSource = dt.DefaultView;
                //    gv_items.DataBind();
                //}

                DataTable dt_item = (DataTable)ViewState["PM"];
                dt_item.Clear();


                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt_item.NewRow();

                    //dr[0] = dt.Rows[i]["id"].ToString();
                    //dr[1] = dt.Rows[i]["consignementno"].ToString();
                    //dr[2] = dt.Rows[i]["itemid"].ToString();
                    //dr[3] = float.Parse(dt.Rows[i]["itemqty"].ToString());
                    //dr[4] = float.Parse(dt.Rows[i]["amount"].ToString());
                    //dr[5] = dt.Rows[i]["message"].ToString();
                    //dr[6] = dt.Rows[i]["status"].ToString();

                    dr["id"] = dt.Rows[i]["id"].ToString();
                    dr["Description"] = dt.Rows[i]["itemCode"].ToString();// itemRow[0]["Name"].ToString();
                    dr["consignementNo"] = dt.Rows[i]["consignementno"].ToString();
                    dr["itemId"] = Int64.Parse(dt.Rows[i]["itemid"].ToString());
                    dr["Qty"] = float.Parse(dt.Rows[i]["itemqty"].ToString()); ;
                    dr["amount"] = float.Parse(dt.Rows[i]["amount"].ToString());
                    dr["message"] = dt.Rows[i]["message"].ToString();
                    dr["status"] = "2";
                    dr["gst"] = dt.Rows[i]["gst"].ToString();// gst;
                    dr["serviceCharges"] = float.Parse(dt.Rows[i]["serviceCharges"].ToString());// float.Parse(itemRow[0]["serviceCharges"].ToString()) * float.Parse(txt_qty.Text);// "100";


                    //// dr[9] = dt.Rows[i]["modifiedon"].ToString();
                    //if (dt.Rows[i]["modifiedon"].ToString() != "")
                    //{
                    //    dr[9] = dt.Rows[i]["modifiedon"].ToString();
                    //}
                    //else
                    //{
                    //    dr[9] = DBNull.Value;
                    //}
                    //dr[10] = dt.Rows[i]["modifiedby"].ToString();
                    //dr[11] = float.Parse(dt.Rows[i]["gst"].ToString());// gst;
                    //dr[12] = float.Parse("100");// "100";


                    dt_item.Rows.Add(dr);
                    dt.AcceptChanges();
                    ViewState["PM"] = dt_item;

                }

                txt_AccNo_TextChanged(this, e);

                if (txt_AccNo.Text.Trim() == "")
                {
                    //hd_unApproveable.Value = "0";
                    txt_AccNo.Text = "";
                    txt_AccNo.Focus();
                    Alert("This Account is Reserved only for COD Portal.");
                    return;
                }

                gv_items.DataSource = dt_item;// dt.DefaultView;
                gv_items.DataBind();
                txt_RiderCode_TextChanged(this, e);
                //btn_save.Visible = false;
                //btn_update.Visible = true;

            }
            else
            {
                //btn_save.Visible = true;
                //btn_update.Visible = false;

                //    txt_ConNo.Text = "";
                txt_AccNo.Text = "";

                rbtn_customerType.ClearSelection();

                cb_Destination.SelectedValue = "0";
                //    txt_consignmentRefNo.Text = ds.Tables[0].Rows[0][""].ToString();
                txt_RiderCode.Text = "";
                //  hd_OriginExpressCenter = ds.Tables[0].Rows[0][""].ToString();
                txt_pieces.Text = "";
                txt_otherCharges.Text = "";
                txt_packageContents.Text = "";

                txt_ConsignerCellNo.Text = "";
                Txt_ConsignerCNIC.Text = "";
                txt_Consigner.Text = "";
                txt_ShipperAddress.Text = "";

                txt_Consignee.Text = "";
                txt_Address.Text = "";
                // dd_consigneeArea.Text = ds.Tables[0].Rows[0][""].ToString();
                txt_ConsigneeCellno.Text = "";

                txt_chargedRate.Text = "";
                //   txt_chargesWalaOtherCharges.Text = ds.Tables[0].Rows[0][""].ToString();
                txt_gst.Text = "";
                txt_totalAmount.Text = "";
                txt_message.Text = "";
                booking_date.Text = DateTime.Now.ToString("yyyy-MM-dd");
                //ViewState["PM"] = null;
                gv_items.DataSource = null;
                gv_items.DataBind();

                txt_AccNo.Focus();
            }
        }

        /*
        protected void RadGrid1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            DataTable dt = EC.ConsignmentExpressionDetail(clvar);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                (e.Item.FindControl("lbl_qty") as Literal).Text = dt.Rows[i]["itemQty"].ToString();
            }
        }
        */
        protected void txt_AccNo_TextChanged(object sender, EventArgs e)
        {
            if (txt_AccNo.Text != "")
            {
                clvar.Branch = HttpContext.Current.Session["BranchCode"].ToString();
                clvar.AccountNo = txt_AccNo.Text;
                DataSet ds = CustomerInformation(clvar);
                if (ds.Tables.Count != 0)
                {
                    if (ds.Tables[0].Rows.Count != 0)
                    {
                        if (txt_AccNo.Text != "0")
                        {
                            txt_Consigner.Text = ds.Tables[0].Rows[0]["Name"].ToString();
                            txt_ConsignerCellNo.Text = ds.Tables[0].Rows[0]["PhoneNo"].ToString();
                            txt_ShipperAddress.Text = ds.Tables[0].Rows[0]["address"].ToString();
                        }
                        hd_CreditClientID.Value = ds.Tables[0].Rows[0]["id"].ToString();


                        if (ds.Tables[0].Rows[0]["accountNo"].ToString() == "0")
                        {
                            this.rbtn_customerType.SelectedValue = "1";
                        }
                        else
                        {
                            this.rbtn_customerType.SelectedValue = "2";
                        }
                        if (ds.Tables[0].Rows[0]["isAPIClient"].ToString() == "1")
                        {
                            //hd_unApproveable.Value = "0";
                            txt_AccNo.Text = "";
                            txt_AccNo.Focus();
                            Alert("This Account is Reserved only for COD Portal.");
                            return;
                        }
                        cb_Destination.Focus();
                    }
                    else
                    {
                        Alert("Invalid Account No");
                        return;
                    }
                }
                else
                {
                    Alert("Account Not Found");
                    return;
                }
            }
            else
            {
                txt_AccNo.Focus();
            }
        }

        protected void cb_Account_CheckedChanged(object sender, EventArgs e)
        {
            if (this.cb_Account.Checked == true)
            {
                txt_AccNo.Enabled = false;
            }
            else
            {
                txt_AccNo.Enabled = true;
            }
        }

        protected void cb_Rider_CheckedChanged(object sender, EventArgs e)
        {
            if (this.cb_Rider.Checked == true)
            {
                txt_RiderCode.Enabled = false;
            }
            else
            {
                txt_RiderCode.Enabled = true;
            }
        }

        protected void cb_Destination_ItemSelected(object sender, EventArgs e)
        {

        }

        protected void txt_RiderCode_TextChanged(object sender, EventArgs e)
        {
            //clvar = new Cl_Variables();
            //clvar.RiderCode = txt_RiderCode.Text;
            //clvar.origin = HttpContext.Current.Session["BranchCode"].ToString();// cb_Origin.SelectedValue;

            //DataSet Rider = con.RiderInformation(clvar);
            //if (Rider.Tables[0].Rows.Count != 0)
            //{
            //    hd_OriginExpressCenter.Value = Rider.Tables[0].Rows[0]["expresscenterid"].ToString();
            //}
            //else
            //{
            //    Alert("Invalid Rider Code");
            //    txt_RiderCode.Text = "";
            //    txt_RiderCode.Focus();

            //}
            GetRiderDetail();
        }

        protected void btn_save_Click(object sender, EventArgs e)
        {
            if (hd_portalCn.Value == "1" || hd_portalCn.Value.ToUpper() == "TRUE")
            {
                Alert("this is a COD Portal Consignment. Cannot Save/Update");
                Errorid.ForeColor = System.Drawing.Color.Red;
                return;
            }
            DateTime thisDate = DateTime.Parse(booking_date.Text);
            clvar.Branch = HttpContext.Current.Session["BranchCode"].ToString();
            DataTable dates = MinimumDate(clvar);
            DateTime minAllowedDate = DateTime.Parse(dates.Rows[0]["DateAllowed"].ToString()).AddDays(1);
            DateTime maxAllowedDate = DateTime.Now;
            if (thisDate < minAllowedDate || thisDate > maxAllowedDate)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Invalid Date.')", true);
                Errorid.ForeColor = System.Drawing.Color.Red;
                return;
            }

            if (DateTime.Parse(booking_date.Text) > DateTime.Now)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Invalid Date')", true);
                Errorid.ForeColor = System.Drawing.Color.Red;
                booking_date.Text = DateTime.Now.ToString("yyyy-MM-dd");
                return;
            }
            //if (DateTime.Parse(dt_DeliveryDatetime.SelectedDate.ToString()) > DateTime.Now)
            //{
            //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Invalid Delivery Date')", true);
            //    dt_DeliveryDatetime.SelectedDate = DateTime.Now;
            //    return;
            //}

            if (cb_Destination.SelectedValue == "0")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Select Destination')", true);
                Errorid.ForeColor = System.Drawing.Color.Red;
                return;
            }
            DataTable cities = ViewState["Cities"] as DataTable;
            txt_AccNo_TextChanged(this, e);
            if (txt_AccNo.Text.Trim() == "")
            {
                //hd_unApproveable.Value = "0";
                txt_AccNo.Text = "";
                txt_AccNo.Focus();
                Alert("This Account is Reserved only for COD Portal.");
                Errorid.ForeColor = System.Drawing.Color.Red;
                return;
            }

            txt_RiderCode_TextChanged(this, e);
            Errorid.Text = "";
            clvar = new Cl_Variables();
            clvar.consignmentNo = txt_ConNo.Text;
            clvar.AccountNo = txt_AccNo.Text;
            clvar.RiderCode = txt_RiderCode.Text;
            clvar.Destination = cb_Destination.SelectedValue; //cities.Select("expressCenterCode = '" + cb_Destination.SelectedValue + "'")[0]["bid"].ToString();//cb_Destination.SelectedValue;// hd_Destination.Value;
            clvar.ServiceTypeName = "Expressions";//cb_ServiceType.SelectedValue;
            clvar.Weight = 0;//float.Parse(txt_Weight.Text);
            clvar.Unit = 1;// cb_Unit.SelectedValue;
                           //clvar.pieces = 1;// txt_Piecies.Text;
            clvar.Zone = HttpContext.Current.Session["ZoneCode"].ToString();

            //Consignee and Consigner Information
            clvar.Consignee = txt_Consignee.Text;
            clvar.ConsigneeCell = txt_ConsigneeCellno.Text;
            clvar.ConsigneeCNIC = "";// txt_ConsigneeCNIC.Text;
            clvar.ConsigneeAddress = txt_Address.Text;
            clvar.Consigner = txt_Consigner.Text;
            clvar.ConsignerCell = txt_ConsignerCellNo.Text;
            clvar.ConsignerCNIC = Txt_ConsignerCNIC.Text;
            clvar.ConsignerAddress = txt_ShipperAddress.Text;
            clvar.consignerAccountNo = txt_AccNo.Text;

            // clvar.Bookingdate = DateTime.Parse(booking_date.Text.ToString());  //DateTime.Parse(dt_DeliveryDatetime.SelectedDate.ToString());
            clvar.BookingDate = booking_date.Text.ToString();


            //  clvar.Bookingdate = DateTime.Parse(booking_date.DbSelectedDate.ToString());  //DateTime.Parse(dt_DeliveryDatetime.SelectedDate.ToString());
            clvar.origin = HttpContext.Current.Session["BranchCode"].ToString();// cb_Origin.SelectedValue;
            clvar.Insurance = "";//txt_insurance.Text;
            clvar.Othercharges = 0;// txt_Othercharges.Text;
            clvar.Day = "";// rb_1.SelectedValue;
            clvar.expresscenter = HttpContext.Current.Session["ExpressCenter"].ToString(); // hd_OriginExpressCenter.Value;
            clvar.destinationExpressCenterCode = cities.Select("BranchCode = '" + cb_Destination.SelectedValue + "'")[0]["ECCode"].ToString(); //cb_Destination.SelectedValue;
            clvar.CustomerClientID = hd_CreditClientID.Value;
            clvar.Con_Type = 10;

            clvar.expressionDeliveryDateTime = DateTime.Parse(dt_DeliveryDatetime.DbSelectedDate.ToString());
            clvar.expressionGreetingCard = chk_greetingCard.Checked;
            clvar.expressionMessage = txt_message.Text;
            clvar.expressionconsignmentRefNumber = txt_consignmentRefNo.Text;
            //clvar.routeCode = dd_consigneeArea.Text;

            //Branch Information
            DataSet BranchGSTInformation = con.BranchGSTInformation(clvar);
            string gst = "";
            if (BranchGSTInformation.Tables[0].Rows.Count != 0)
            {
                gst = BranchGSTInformation.Tables[0].Rows[0]["gst"].ToString();
            }
            //Calculating Modifier

            double Modfier = 0;
            double TotalGstAmount = 0;

            double TotalAmount = 0;
            double TotalServiceCharges = 0;
            //Calculating InsuranceAmount
            double insper = 0;
            //foreach (GridViewRow rp in gv_items.Rows)
            //{
            //    if (rp.Visible == true)
            //    {
            //        double Amount = 0;

            //        double ServiceCharges = 0;
            //        double GstAmount = 0;

            //        Label tx = ((Label)rp.FindControl("lbl_Amount"));
            //        Modfier = double.Parse(tx.Text);

            //        ServiceCharges = 100;
            //        GstAmount = (ServiceCharges / 100) * double.Parse(gst);
            //        Amount = (Modfier) + ServiceCharges + GstAmount;

            //        TotalServiceCharges += ServiceCharges;
            //        TotalGstAmount += GstAmount;
            //        TotalAmount += Amount;
            //    }


            //}

            txt_chargedRate.Text = string.Format("{0:N2}", TotalAmount - TotalGstAmount - TotalServiceCharges);
            txt_chargesWalaOtherCharges.Text = string.Format("{0:N2}", TotalServiceCharges);
            txt_gst.Text = string.Format("{0:N2}", TotalGstAmount);
            txt_totalAmount.Text = string.Format("{0:N2}", TotalAmount);

            clvar.TotalAmount = float.Parse(txt_totalAmount.Text) - float.Parse(txt_gst.Text); ;// TotalAmount;
            clvar.gst = float.Parse(txt_gst.Text);
            clvar.ChargeAmount = 0;
            clvar.Customertype = int.Parse(rbtn_customerType.SelectedValue);
            DataTable dt = (DataTable)ViewState["PM"];
            if (dt.Rows.Count != 0)
            {
                clvar.isPM = true;
                clvar.expresssion = dt;
            }
            else
            {
                clvar.isPM = false;
                clvar.expresssion = null;
            }



            clvar.destinationCountryCode = "PAK";

            //con.Add_Consignment(clvar);
            clvar.cnScreenId = "11";
            clvar.RiderCode = txt_RiderCode.Text;
            clvar.createdBy = HttpContext.Current.Session["U_ID"].ToString();
            if (dt.Rows.Count > 0)
            {

            }
            clvar.OriginExpressCenterCode = hd_OriginExpressCenter.Value;
            clvar.pieces = int.Parse(dt.Compute("SUM(Qty)", "").ToString());
            clvar.PakageContents = txt_packageContents.Text;

            string error = con.Add_Consignment_Validation(clvar);


            //EC.Add_Consignment(clvar);
            if (error != "")
            {
                Errorid.Text = error;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cannot Create Consignment.')", true);
                Errorid.ForeColor = System.Drawing.Color.Red;
                return;
            }

            //string error1 = EC.Update_ExpressionConsignmentApproval2(clvar);

            error = "";
            error = WriteToDatabase_(clvar);
            if (error != "")
            {
                Errorid.Text = error;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cannot Create Consignment.')", true);
                Errorid.ForeColor = System.Drawing.Color.Red;
                return;
            }
            DataTable pricing = GetExpressionPricing(clvar).Tables[0];
            if (pricing != null)
            {
                if (pricing.Rows.Count > 0)
                {
                    double tAmount = 0;
                    double tGst = 0;
                    double tServiceCharges = 0;

                    object oAmount = pricing.Compute("SUM(amount)", "");
                    object oGst = pricing.Compute("SUM(gst)", "");
                    object oServiceCharges = pricing.Compute("SUM(ServiceCharges)", "");

                    double.TryParse(oAmount.ToString(), out tAmount);
                    double.TryParse(oGst.ToString(), out tGst);
                    double.TryParse(oServiceCharges.ToString(), out tServiceCharges);
                    clvar.TotalAmount = tAmount;
                    clvar.gst = tGst;


                }
                else
                {
                    Errorid.Text = "Tariff Not Available.";
                    Errorid.ForeColor = System.Drawing.Color.Red;
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Tariff Not Available.')", true);
                    return;
                }
                clvar.dayType = 2;
                error = Add_Consignment(clvar);

                if (error != "")
                {
                    Alert("Consignment Could Not be Saved. Error: " + error);
                    Errorid.ForeColor = System.Drawing.Color.Red;
                    return;
                }
                else
                {
                    error = WriteToDatabase_Actual(clvar);// InsertExpressionDetailActual(clvar);
                    if (error != "")
                    {
                        Alert("Consignment Could Not be Saved. Error: " + error);
                        Errorid.ForeColor = System.Drawing.Color.Red;
                        return;
                    }
                }


                //error = con.DeleteConsignment(clvar);
                //if (error != "")
                //{

                //}
                //else
                //{
                //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Consignment Could not Save. Please Contact IT Support. Error: " + error + "')", true);
                //    return;
                //}
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cannot Create Consignment. Error = '" + error + "'.')", true);
                //return;
            }
            else
            {
                Errorid.Text = "Tariff Not Available.";
                Errorid.ForeColor = System.Drawing.Color.Red;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Tariff Not Available.')", true);
                return;
            }

            Post_BrandedSMS(txt_ConsignerCellNo.Text, txt_ConNo.Text, txt_Consigner.Text.ToUpper(), cb_Destination.SelectedItem.Text);
            //if (txt_SmsConsignment.Checked == true)
            //{
            Post_BrandedSMS_(txt_ConsigneeCellno.Text, txt_ConNo.Text, txt_Consignee.Text.ToUpper(), cb_Destination.SelectedItem.Text);

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Consignment Saved Successfully')", true);
            string script_ = @"window.open(""{0}"", ""{1}"", ""{2}"");";
            string script = String.Format(script_, "ExpressionPrint.aspx?Xcode=" + clvar.consignmentNo, "_blank", "");
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Redirect", script, true);

            ResetAll();
        }

        #region MyRegion
        //protected void btn_update_Click(object sender, EventArgs e)
        //{

        //    DataTable cities = ViewState["Cities"] as DataTable;
        //    if (DateTime.Parse(booking_date.Text) > DateTime.Now)
        //    {
        //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Invalid Date')", true);
        //        booking_date.Text = DateTime.Now.ToString("yyyy-MM-dd");
        //        return;
        //    }

        //    if (cb_Destination.SelectedItem == null)
        //    {
        //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Select Destination')", true);
        //        return;
        //    }
        //    txt_AccNo_TextChanged(this, e);
        //    Errorid.Text = "";
        //    clvar = new Cl_Variables();
        //    clvar.consignmentNo = txt_ConNo.Text;
        //    clvar.AccountNo = txt_AccNo.Text;
        //    clvar.RiderCode = txt_RiderCode.Text;
        //    clvar.Destination = cities.Select("expressCenterCode = '" + cb_Destination.SelectedValue + "'")[0]["bid"].ToString();// cb_Destination.SelectedValue;// hd_Destination.Value;
        //    clvar.ServiceTypeName = "Expressions";//cb_ServiceType.SelectedValue;
        //    clvar.Weight = 0;//float.Parse(txt_Weight.Text);
        //    clvar.Unit = 1;// cb_Unit.SelectedValue;
        //    //   clvar.pieces = 1;// txt_Piecies.Text;
        //    clvar.Zone = HttpContext.Current.Session["ZoneCode"].ToString();

        //    //  string qty = txt_qty.Text;




        //    //for (int i = 0; i <= txt_qty.Text.Count;   )

        //    //Consignee and Consigner Information
        //    clvar.Consignee = txt_Consignee.Text;
        //    clvar.ConsigneeCell = txt_ConsigneeCellno.Text;
        //    clvar.ConsigneeCNIC = "";// txt_ConsigneeCNIC.Text;
        //    clvar.ConsigneeAddress = txt_Address.Text.Replace("'", "").Replace("`", ""); ;
        //    clvar.Consigner = txt_Consigner.Text;
        //    clvar.ConsignerCell = txt_ConsignerCellNo.Text;
        //    clvar.ConsignerCNIC = Txt_ConsignerCNIC.Text;
        //    clvar.ConsignerAddress = txt_ShipperAddress.Text.Replace("'", "").Replace("`", "");
        //    clvar.consignerAccountNo = txt_AccNo.Text;

        //    //    clvar.Bookingdate = DateTime.Parse(dt_DeliveryDatetime.SelectedDate.ToString());

        //    clvar.BookingDate = booking_date.Text.ToString();


        //    // clvar.origin = "4";// cb_Origin.SelectedValue;
        //    clvar.origin = Session["BranchCode"].ToString();
        //    clvar.Insurance = "";//txt_insurance.Text;
        //    clvar.Othercharges = 0;// txt_Othercharges.Text;
        //    clvar.Day = "";// rb_1.SelectedValue;
        //    clvar.expresscenter = hd_OriginExpressCenter.Value;
        //    clvar.destinationExpressCenterCode = cb_Destination.SelectedValue;
        //    clvar.CustomerClientID = hd_CreditClientID.Value;
        //    clvar.Con_Type = 10;

        //    clvar.expressionDeliveryDateTime = DateTime.Parse(dt_DeliveryDatetime.DbSelectedDate.ToString());
        //    clvar.expressionGreetingCard = chk_greetingCard.Checked;
        //    clvar.expressionMessage = txt_message.Text;
        //    clvar.expressionconsignmentRefNumber = txt_consignmentRefNo.Text;
        //    //clvar.routeCode = dd_consigneeArea.Text;

        //    //Branch Information
        //    DataSet BranchGSTInformation = con.BranchGSTInformation(clvar);
        //    string gst = "";
        //    if (BranchGSTInformation.Tables[0].Rows.Count != 0)
        //    {
        //        gst = BranchGSTInformation.Tables[0].Rows[0]["gst"].ToString();
        //    }
        //    //Calculating Modifier
        //    double Modfier = 0;
        //    double TotalGstAmount = 0;

        //    double TotalAmount = 0;
        //    double TotalServiceCharges = 0;


        //    foreach (RepeaterItem rp in gv_items.Items)
        //    {
        //        if (rp.Visible == true)
        //        {
        //            double Amount = 0;

        //            double ServiceCharges = 0;
        //            double GstAmount = 0;

        //            Label tx = ((Label)rp.FindControl("lbl_Amount"));
        //            Modfier = double.Parse(tx.Text);

        //            ServiceCharges = 100;
        //            GstAmount = (ServiceCharges / 100) * double.Parse(gst);
        //            Amount = (Modfier) + ServiceCharges + GstAmount;

        //            TotalServiceCharges += ServiceCharges;
        //            TotalGstAmount += GstAmount;
        //            TotalAmount += Amount;
        //        }


        //    }
        //    txt_chargedRate.Text = string.Format("{0:N2}", TotalAmount - TotalGstAmount - TotalServiceCharges);
        //    txt_chargesWalaOtherCharges.Text = string.Format("{0:N2}", TotalServiceCharges);
        //    txt_gst.Text = string.Format("{0:N2}", TotalGstAmount);
        //    txt_totalAmount.Text = string.Format("{0:N2}", TotalAmount);
        //    //Calculating InsuranceAmount
        //    double insper = 0;
        //    clvar.TotalAmount = float.Parse(txt_totalAmount.Text) - float.Parse(txt_gst.Text); ;// TotalAmount;
        //    clvar.gst = float.Parse(txt_gst.Text);
        //    clvar.ChargeAmount = 0;
        //    DataTable dt = (DataTable)ViewState["PM"];
        //    if (dt.Rows.Count != 0)
        //    {
        //        /*
        //        for (int i = 0; i <= dt.Rows[0]["itemqty"].ToString().Count(); i++)
        //        {
        //            clvar.pieces += Int32.Parse(dt.Rows[i]["itemqty"].ToString());
        //        }
        //            */
        //        clvar.isPM = true;
        //        clvar.expresssion = dt;
        //    }
        //    else
        //    {

        //        clvar.isPM = false;
        //        clvar.expresssion = null;
        //    }

        //    clvar.destinationCountryCode = "PAK";
        //    clvar.Customertype = int.Parse(rbtn_customerType.SelectedValue);

        //    //con.Add_Consignment(clvar);
        //    clvar.CustomerClientID = hd_CreditClientID.Value;
        //    string error = EC.Update_ExpressionConsignmentApproval(clvar);
        //    if (error != "")
        //    {
        //        Errorid.Text = error;
        //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cannot Create Consignment.')", true);

        //        return;
        //    }
        //    error = "";
        //    error = con.WriteToDatabase_(clvar);
        //    if (error != "")
        //    {
        //        //error = con.DeleteConsignment(clvar);
        //        if (error != "")
        //        {

        //        }
        //        else
        //        {
        //            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Consignment Could not Save. Please Contact IT Support. Error: " + error + "')", true);
        //            return;
        //        }
        //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cannot Create Consignment. Error = '" + error + "'.')", true);
        //        return;
        //    }
        //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Consignment Update Successfully')", true);

        //} 
        #endregion

        protected void dd_item_ItemSelected(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            DataTable dt = (DataTable)ViewState["Expressions"];
            if (dd_item.SelectedValue != "0")
            {
                DataRow[] Dr = dt.Select("code='" + dd_item.SelectedValue + "'");
                if (Dr.Length != 0)
                {
                    txt_qty.Text = "";
                    txt_amount.Text = Dr[0]["rate"].ToString();
                    HiddenField2.Value = Dr[0]["id"].ToString();
                }
            }
        }

        public void LoadGrid()
        {
            //clvar.origin = HttpContext.Current.Session["BranchCode"].ToString();
            //DataSet BranchGSTInformation = con.BranchGSTInformation(clvar);
            //string gst = "";
            //if (BranchGSTInformation.Tables[0].Rows.Count != 0)
            //{
            //    gst = BranchGSTInformation.Tables[0].Rows[0]["gst"].ToString();
            //}
            //double Modfier = 0;
            //double TotalGstAmount = 0;

            //double TotalAmount = 0;
            //double TotalServiceCharges = 0;
            //DataTable dt = (DataTable)ViewState["PM"];
            //if (dt.Rows.Count > 0)
            //{
            //    gv_items.DataSource = dt.DefaultView;
            //    gv_items.DataBind();


            //    foreach (GridViewRow rp in gv_items.Rows)
            //    {
            //        if (rp.Visible == true)
            //        {
            //            double Amount = 0;

            //            double ServiceCharges = 0;
            //            double GstAmount = 0;

            //            Label tx = ((Label)rp.FindControl("lbl_Amount"));
            //            Modfier = double.Parse(tx.Text);

            //            ServiceCharges = 100;
            //            GstAmount = (ServiceCharges / 100) * double.Parse(gst);
            //            Amount = (Modfier) + ServiceCharges + GstAmount;

            //            TotalServiceCharges += ServiceCharges;
            //            TotalGstAmount += GstAmount;
            //            TotalAmount += Amount;
            //        }


            //    }

            //    txt_chargedRate.Text = string.Format("{0:N2}", TotalAmount - TotalGstAmount - TotalServiceCharges);
            //    txt_chargesWalaOtherCharges.Text = string.Format("{0:N2}", TotalServiceCharges);
            //    txt_gst.Text = string.Format("{0:N2}", TotalGstAmount);
            //    txt_totalAmount.Text = string.Format("{0:N2}", TotalAmount);
            //}
            //else
            //{
            //    gv_items.DataSource = null;
            //    gv_items.DataBind();
            //}
        }

        protected void RadGrid1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataTable Expressions = (DataTable)ViewState["Expressions"];
                string Itemid = (e.Item.FindControl("Hd_ID") as HiddenField).Value;
                HiddenField hd_status = (e.Item.FindControl("hd_status") as HiddenField);
                DataRow[] dr = Expressions.Select("id ='" + Itemid + "'");
                if (dr.Length != 0)
                {
                    (e.Item.FindControl("lbl_ItemDescription") as Label).Text = dr[0]["name"].ToString();
                }
                if (hd_status.Value == "0")
                {
                    e.Item.Visible = false;
                }
            }
            else
            {
                /*
                clvar.consignmentNo = txt_ConNo.Text;
                DataTable dt = EC.ConsignmentExpressionDetail(clvar);

                for (int i = 1; i < dt.Rows.Count; i++)
                {
                    (e.Item.FindControl("lbl_qty") as Label).Text = dt.Rows[i]["itemQty"].ToString();

                    DataRow dr = dt.NewRow();
                    dr[0] = Int64.Parse("0");
                    dr[1] = txt_ConNo.Text;
                    dr[2] = Int64.Parse(HiddenField2.Value);
                    dr[3] = float.Parse(txt_qty.Text);
                    dr[4] = float.Parse(txt_amount.Text) - 100 - float.Parse(dt.Rows[i]["gst"].ToString());
                    dr[5] = DBNull.Value;
                    dr[6] = DBNull.Value;
                    dr[7] = DateTime.Now.ToString();
                    dr[8] = DBNull.Value;
                    dr[9] = DateTime.Now.ToString();
                    dr[10] = DBNull.Value;
                    dr[11] = float.Parse(dt.Rows[i]["gst"].ToString());// gst;
                    dr[12] = float.Parse("100");// "100";

                    dt.Rows.Add(dr);
                    dt.AcceptChanges();
                    ViewState["PM"] = dt;


                }
                 */
            }
        }

        protected void RadGrid1_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                string Itemid = (e.Item.FindControl("Hd_ID") as HiddenField).Value;

                DataTable dt = (DataTable)ViewState["PM"];

                DataRow dr = dt.Select("itemid ='" + Itemid + "' and status <> '0'").FirstOrDefault(); // finds all rows with id==2 and selects first or null if haven't found any
                if (dr != null)
                {
                    if (dr["status"].ToString() == "1")
                    {
                        dr["Status"] = "0";
                    }
                    else
                    {
                        dr.Delete();//changes the Product_name
                    }

                }
                dt.AcceptChanges();
                ViewState["PM"] = dt;

                LoadGrid();
            }
        }
        protected void dt_DeliveryDatetime_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            //DateTime dt = dt_DeliveryDatetime.SelectedDate.Value;
            //if (DateTime.Parse(dt_DeliveryDatetime.SelectedDate.ToString()) > DateTime.Now)
            //{
            //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Invalid Delivery Date')", true);
            //    dt_DeliveryDatetime.SelectedDate = DateTime.Now;
            //    return;
            //}
        }

        protected DataTable MinimumDate(Cl_Variables clvar)
        {
            string sqlString = "select MAX(DateTime) DateAllowed from Mnp_Account_DayEnd where Branch = '" + clvar.Branch + "' and doc_type = 'A'";
            sqlString = "SELECT CASE\n" +
           "       --WHEN MAX(A.DATEALLOWED_ACC) > MAX(A.DATEALLOWED_OP) AND (SELECT DEPARTMENT FROM ZNI_USER1 Z WHERE Z.U_ID = '" + HttpContext.Current.Session["U_ID"].ToString() + "') = '19' THEN\n" +
           "       -- MAX(A.DATEALLOWED_ACC)\n" +
           "         WHEN (SELECT DEPARTMENT FROM ZNI_USER1 Z WHERE Z.U_ID = '" + HttpContext.Current.Session["U_ID"].ToString() + "') = '19' THEN\n" +
           "          MAX(A.DATEALLOWED_OP)\n" +
           "         when (SELECT DEPARTMENT FROM ZNI_USER1 Z WHERE Z.U_ID = '" + HttpContext.Current.Session["U_ID"].ToString() + "') = '1' THEN\n" +
           "          MAX(A.DATEALLOWED_ACC)\n" +
           "         else\n" +
           "          null\n" +
           "       END DATEALLOWED\n" +
           "  FROM (SELECT MAX(DATETIME) DATEALLOWED_OP, NULL DATEALLOWED_ACC\n" +
           "          FROM MNP_ACCOUNT_DAYEND\n" +
           "         WHERE BRANCH = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'\n" +
           "           AND DOC_TYPE = 'O'\n" +
           "        UNION ALL\n" +
           "        SELECT NULL DATEALLOWED_OP, MAX(DATETIME) DATEALLOWED_ACC\n" +
           "          FROM MNP_ACCOUNT_DAYEND\n" +
           "         WHERE BRANCH = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'\n" +
           "           AND DOC_TYPE = 'A') A";
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

        public DataSet GetExpressionPricing(Cl_Variables clvar)
        {
            DataSet ds = new DataSet();

            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand("SP_UpdateCNPrice_EXP_Validation", orcl);
                orcd.Parameters.AddWithValue("@Consignment", clvar.consignmentNo);
                orcd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                orcl.Close();
            }
            catch (Exception)
            { }

            return ds;
        }

        public string WriteToDatabase_(Cl_Variables clvar)
        {
            string error = "";
            try
            {
                List<string> queries = new List<string>();
                queries.Add("DELETE FROM ConsignmentExpressionDetail_temp where ConsignementNo = '" + clvar.consignmentNo + "'");
                foreach (DataRow row in clvar.expresssion.Rows)
                {

                    error = "insert into ConsignmentExpressionDetail_temp (ConsignementNo, itemID, itemqty, amount, message, Status, CreatedOn, CreatedBy, gst, serviceCharges) \n" +
                            " VALUES (\n" +
                            "'" + clvar.consignmentNo + "',\n" +
                            "'" + row["itemId"].ToString() + "',\n" +
                            "'" + row["Qty"].ToString() + "',\n" +
                            "'" + row["amount"].ToString() + "',\n" +
                            "'" + row["message"].ToString() + "',\n" +
                            "'" + DBNull.Value + "',\n" +
                            "GETDATE(),\n" +
                            "'" + HttpContext.Current.Session["U_ID"].ToString() + "',\n" +
                            "'" + row["gst"].ToString() + "',\n" +
                            "'" + row["serviceCharges"].ToString() + "'\n" +

                            ")";
                    queries.Add(error);

                }

                SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
                SqlTransaction trans;
                sqlcon.Open();
                SqlCommand sqlcmd = new SqlCommand();
                sqlcmd.Connection = sqlcon;
                trans = sqlcon.BeginTransaction();
                sqlcmd.Transaction = trans;
                sqlcmd.CommandType = CommandType.Text;
                error = "";
                try
                {
                    foreach (string sql in queries)
                    {
                        sqlcmd.CommandText = sql;
                        int count = sqlcmd.ExecuteNonQuery();
                    }


                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                }
                finally
                {
                    sqlcon.Close();
                }


            }
            catch (Exception er)
            {

                error = er.Message;
            }
            // reset

            return error;

        }
        public string WriteToDatabase_Actual(Cl_Variables clvar)
        {
            string error = "";
            try
            {
                List<string> queries = new List<string>();
                queries.Add("DELETE FROM ConsignmentExpressionDetail where ConsignementNo = '" + clvar.consignmentNo + "'");
                foreach (DataRow row in clvar.expresssion.Rows)
                {

                    error = "insert into ConsignmentExpressionDetail (ConsignementNo, itemID, itemqty, amount, message, Status, CreatedOn, CreatedBy, gst, serviceCharges) \n" +
                            " VALUES (\n" +
                            "'" + clvar.consignmentNo + "',\n" +
                            "'" + row["itemId"].ToString() + "',\n" +
                            "'" + row["Qty"].ToString() + "',\n" +
                            "'" + row["amount"].ToString() + "',\n" +
                            "'" + row["message"].ToString() + "',\n" +
                            "'" + DBNull.Value + "',\n" +
                            "GETDATE(),\n" +
                            "'" + HttpContext.Current.Session["U_ID"].ToString() + "',\n" +
                            "'" + row["gst"].ToString() + "',\n" +
                            "'" + row["serviceCharges"].ToString() + "'\n" +

                            ")";
                    queries.Add(error);

                }

                SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
                SqlTransaction trans;
                sqlcon.Open();
                SqlCommand sqlcmd = new SqlCommand();
                sqlcmd.Connection = sqlcon;
                trans = sqlcon.BeginTransaction();
                sqlcmd.Transaction = trans;
                sqlcmd.CommandType = CommandType.Text;
                error = "";
                try
                {
                    foreach (string sql in queries)
                    {
                        sqlcmd.CommandText = sql;
                        int count = sqlcmd.ExecuteNonQuery();
                    }


                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                }
                finally
                {
                    sqlcon.Close();
                }


            }
            catch (Exception er)
            {

                error = er.Message;
            }
            // reset

            return error;

        }


        protected void txt_riderCode_TextChanged(object sender, EventArgs e)
        {
            GetRiderDetail();
        }
        protected DataTable GetRiderDetail()
        {
            string query = "SELECT * FROM RIDERS r where r.riderCode = '" + txt_RiderCode.Text.Trim() + "' and r.branchID = '" + HttpContext.Current.Session["BranchCode"].ToString() + "' and r.status = '1'";
            DataTable dt = new DataTable();
            try
            {
                SqlConnection con = new SqlConnection(clvar.Strcon());
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                sda.Fill(dt);
            }
            catch (Exception ex)
            { }
            if (dt.Rows.Count > 0)
            {
                hd_OriginExpressCenter.Value = dt.Rows[0]["ExpressCenterID"].ToString();
                //txt_weight.Focus();
                booking_date.Focus();
            }
            else
            {
                hd_OriginExpressCenter.Value = "0";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Invalid Rider Code')", true);
                txt_RiderCode.Text = "";
                txt_RiderCode.Focus();

            }
            return dt;
        }
        public void Alert(string message)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + message + "')", true);
            Errorid.Text = message;
        }

        public string InsertExpressionDetailActual(Cl_Variables clvar)
        {
            string error = "";
            string query = "insert into consignmentExpressionDetail  select ct.consignementNo, ct.itemId, ct.itemQty, ct.amount, ct.message, ct.status, GETDATE(), '" + HttpContext.Current.Session["U_ID"].ToString() + "', NULL, NULL, ct.gst, ct.serviceCharges from consignmentExpressionDetail_temp ct where ct.consignementNo = '" + clvar.consignmentNo + "'";
            string query_ = "delete from consignmentExpressionDetail where consignementNo = '" + clvar.consignmentNo + "'";
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(query_, con);
                cmd.ExecuteNonQuery();

                cmd.CommandText = query;
                int count = cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                error = ex.Message;
                SqlCommand cmd_ = new SqlCommand("DELETE FROM CONSIGNMENT WHERE CONSIGNMENTNUMBER = '" + clvar.consignmentNo + "'", con);
                cmd_.ExecuteNonQuery();
            }
            finally { con.Close(); }
            return error;
        }

        public void ResetAll()
        {
            //gv_items.DataSource = null;
            //gv_items.DataBind();
            //BindCities();
            Get_Destination();
            Get_Items();

            DataTable dt_items = new DataTable();
            //   dt_items.Columns.Add("ID", typeof(string));
            dt_items.Columns.Add("Description");
            dt_items.Columns.Add("id");
            dt_items.Columns.Add("consignementNo", typeof(string));
            dt_items.Columns.Add("itemId", typeof(Int64));
            dt_items.Columns.Add("Qty", typeof(float));
            dt_items.Columns.Add("amount", typeof(float));
            dt_items.Columns.Add("message", typeof(string));
            dt_items.Columns.Add("status", typeof(string));




            dt_items.Columns.Add("gst", typeof(float));
            dt_items.Columns.Add("serviceCharges", typeof(float));

            ViewState["PM"] = dt_items;

            dt_DeliveryDatetime.SelectedDate = DateTime.Now.Date;

            txt_AccNo.Text = "";
            txt_Address.Text = "";
            txt_amount.Text = "";
            txt_chargedRate.Text = "";
            txt_chargesWalaOtherCharges.Text = "";
            txt_ConNo.Text = "";
            txt_Consignee.Text = "";
            txt_ConsigneeCellno.Text = "";
            txt_Consigner.Text = "";
            txt_ConsignerCellNo.Text = "";
            Txt_ConsignerCNIC.Text = "";
            txt_consignmentRefNo.Text = "";
            txt_gst.Text = "";
            txt_message.Text = "";
            txt_otherCharges.Text = "";
            txt_packageContents.Text = "";
            txt_pieces.Text = "1";
            txt_qty.Text = "";
            txt_RiderCode.Text = "";
            txt_ShipperAddress.Text = "";
            txt_totalAmount.Text = "";

            gv_items.DataSource = null;
            gv_items.DataBind(); ;
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
                SqlCommand sqlcmd = new SqlCommand("MNP_InsertConsignment_Temp", sqlcon);
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
                sqlcmd.Parameters.AddWithValue("@consignmentScreen", "11");
                sqlcmd.Parameters.AddWithValue("@isInsured", obj.isInsured);
                sqlcmd.Parameters.AddWithValue("@isReturned", obj.isReturned);
                sqlcmd.Parameters.AddWithValue("@consigneeCNICNo", obj.ConsigneeCNIC);
                sqlcmd.Parameters.AddWithValue("@cutOffTimeShift", obj.cutOffTimeShift);
                sqlcmd.Parameters.AddWithValue("@bookingDate", obj.BookingDate);
                sqlcmd.Parameters.AddWithValue("@cnClientType", obj.cnClientType);
                sqlcmd.Parameters.AddWithValue("@destinationExpressCenterCode", obj.destinationExpressCenterCode);
                sqlcmd.Parameters.AddWithValue("@dayType", obj.dayType);
                sqlcmd.Parameters.AddWithValue("@originExpressCenter", obj.OriginExpressCenterCode);
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
                sqlcmd.Parameters.AddWithValue("@accountReceivingDate", obj.BookingDate);

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




        public DataSet GetConsignmentForApproval(Cl_Variables clvar)
        {

            #region MyRegion
            //string sqlString = "select c.bookingDate,\n" +
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
            //"       c.isApproved,\n" +
            //"       ic.invoiceNumber,\n" +
            //"       i.startDate ReportingDate,\n" +
            //"       i.deliveryStatus,\n" +
            //"       cm.priceModifierId,\n" +
            //"       p.name,\n" +
            //"       cm.calculatedValue,\n" +
            //"       cm.calculationBase,\n" +
            //"       cm.isTaxable,\n" +
            //"       cm.SortOrder,\n" +
            //"       p.description, c.destinationExpressCenterCode, c.couponNumber\n" +
            //"\n" +
            //"  from consignment c\n" +
            //" inner join InvoiceConsignment ic\n" +
            //"    on c.consignmentNumber = ic.consignmentNumber\n" +
            //" inner join Invoice i\n" +
            //"    on i.invoiceNumber = ic.invoiceNumber\n" +
            //"  left outer join ConsignmentModifier cm\n" +
            //"    on c.consignmentNumber = cm.consignmentNumber\n" +
            //"  left outer join PriceModifiers p\n" +
            //"    on cm.priceModifierId = p.id\n" +
            //" where c.orgin = '4'\n" +
            //"   and c.consignmentTypeId <> '10'\n" +
            //"   and cm.priceModifierId is not null\n" +
            //"   and IsNull( i.IsInvoiceCanceled , 0 ) ='0'\n" +
            //"   and c.consignmentNumber = '" + clvar.consignmentNo + "'\n" +
            //" order by consignmentNumber, SortOrder";

            //// sqlString = "select c.bookingDate,\n" +
            ////"       c.consignmentNumber,\n" +
            ////"       c.customerType,\n" +
            ////"       c.creditClientId,\n" +
            ////"       c.orgin,\n" +
            ////"       c.serviceTypeName,\n" +
            ////"       c.consigner,\n" +
            ////"       c.consignee,\n" +
            ////"       c.destination,\n" +
            ////"       c.weight,\n" +
            ////"       c.riderCode,\n" +
            ////"       c.originExpressCenter,\n" +
            ////"       c.consignmentTypeId,\n" +
            ////"       c.chargedAmount,\n" +
            ////"       CAST(c.isApproved as varchar) isApproved,\n" +
            ////"       c.consignerAccountNo accountNo,\n" +
            ////"       ic.invoiceNumber,\n" +
            ////"       i.startDate ReportingDate,\n" +
            ////"       i.deliveryStatus,\n" +
            ////"       cm.priceModifierId,\n" +
            ////"       p.name priceModifierName,\n" +
            ////"       cm.calculatedValue,\n" +
            ////"       cm.calculationBase,\n" +
            ////"       cm.isTaxable,\n" +
            ////"       cm.SortOrder,\n" +
            ////"       p.description, c.destinationExpressCenterCode, c.accountReceivingDate, c.bookingDate, c.COD\n" +
            ////"\n" +
            ////"  from consignment c\n" +
            ////" left outer join InvoiceConsignment ic\n" +
            ////"    on c.consignmentNumber = ic.consignmentNumber\n" +
            ////" left outer join Invoice i\n" +
            ////"    on i.invoiceNumber = ic.invoiceNumber\n" +
            ////"  left outer join ConsignmentModifier cm\n" +
            ////"    on c.consignmentNumber = cm.consignmentNumber\n" +
            ////"  left outer join PriceModifiers p\n" +
            ////"    on cm.priceModifierId = p.id\n" +
            ////" inner join CreditClients cc\n" +
            ////"    on c.creditClientId = cc.id\n" +
            ////" where /*c.orgin = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'\n" +
            ////"   and c.consignmentTypeId <> '10'\n" +
            ////"   and */c.consignmentNumber = '" + clvar.consignmentNo.Trim() + "' and ISNULL(i.IsInvoiceCanceled,0) = '0' \n" +
            ////" order by consignmentNumber, SortOrder"; 
            #endregion

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
               + "       c.consignerAccountNo              , \n"
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
               + "       ic.invoiceNumber, c.consignerCellNo, c.ConsigneePhoneNo, c.address, c.pieces, c.couponNumber, c.ExpressionDeliveryDateTime, c.otherCharges\n"
               + "    --   i.IsInvoiceCanceled \n"
               + "FROM   consignment c \n"
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




            sql = "SELECT A.*,\n" +
           "       i.deliveryStatus,\n" +
           "       CASE\n" +
           "         WHEN i.IsInvoiceCanceled = '1' THEN\n" +
           "          ''\n" +
           "         ELSE\n" +
           "          i.invoiceNumber\n" +
           "       END invoiceNumber_\n" +
           "  FROM (SELECT c.*, ic.invoiceNumber,\n" +
           "               CASE\n" +
           "                 when cns.CreditClientID is null then\n" +
           "                  '0'\n" +
           "                 else\n" +
           "                  '1'\n" +
           "               end PortalCN\n" +
           "          FROM consignment c\n" +
           "          LEFT OUTER JOIN InvoiceConsignment ic\n" +
           "            ON c.consignmentNumber = ic.consignmentNumber\n" +
           "         INNER JOIN CreditClients cc\n" +
           "            ON c.creditClientId = cc.id\n" +
           "          LEFT OUTER JOIN CODUSERS cns\n" +
           "            on cns.CreditClientID = cc.id\n" +
           "           and cns.isCOD = '1'\n" +
           "         WHERE c.consignmentNumber = '" + clvar.consignmentNo.Trim() + "'\n" +
           "        ) A\n" +
           "  LEFT OUTER JOIN Invoice i\n" +
           "    ON A.invoiceNumber = i.invoiceNumber\n" +
           " ORDER BY i.createdOn desc";




            DataSet dt = new DataSet();
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



        public string UnapproveConsignment(Cl_Variables clvar)
        {
            string query = "update consignment Set isApproved = '0' where consignmentnumber = '" + clvar.consignmentNo + "'";
            string query2 = "insert into MNP_ConsignmentUnapproval (ConsignmentNumber, USERID, TransactionTime, STATUS) VALUES (\n" +
                "'" + clvar.consignmentNo + "', '" + HttpContext.Current.Session["U_ID"].ToString() + "',GETDATE(), '0')";
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand(query2, con);
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                //Consignemnts con = new Consignemnts();
                CommonFunction cfunc = new CommonFunction();
                con.Close();
                cfunc.InsertErrorLog(clvar.consignmentNo, "", "", "", "", "", "UNAPPROVE CONSIGNMENT", ex.Message);
                return ex.Message;
            }
            finally { con.Close(); }

            return "OK";
        }





        protected void txt_qty_TextChanged(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)ViewState["Expressions"];
            float gst = float.Parse(ViewState["Bgst"].ToString());// as float;
            if (dd_item.SelectedValue != "0" && txt_qty.Text != "0")
            {
                DataRow[] Dr = dt.Select("id='" + dd_item.SelectedValue + "'");
                if (Dr.Length != 0)
                {
                    float sCharges = float.Parse(Dr[0]["serviceCharges"].ToString());
                    float itemAmount = float.Parse(Dr[0]["rate"].ToString());
                    float qty = float.Parse(txt_qty.Text);
                    double totalAmount = Math.Round(itemAmount - ((itemAmount / (gst + 100)) * gst), 2);
                    hd_itemGst.Value = (((itemAmount / (gst + 100)) * gst) * qty).ToString();
                    txt_amount.Text = (totalAmount * qty).ToString();
                    //txt_qty.Text = "1";
                    //txt_amount.Text = ((float.Parse(Dr[0]["ItemAmount"].ToString()) + float.Parse(Dr[0]["serviceCharges"].ToString()) + (float.Parse(Dr[0]["serviceCharges"].ToString()))) * int.Parse(txt_qty.Text)).ToString();
                    HiddenField2.Value = Dr[0]["id"].ToString();


                    //     ROUND(

                    //a.itemamount -(a.itemamount / ((100 + CAST(a.gst AS INT))) * a.gst),

                    //2





                }
            }
        }
        protected void gv_items_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Remove")
            {
                //GridViewRow row = (GridViewRow)((Control)e.CommandSource).NamingContainer;
                string Itemid = e.CommandArgument.ToString();// (row.FindControl("Hd_ID") as HiddenField).Value;
                                                             //row.Visible = false;
                DataTable dt = (DataTable)ViewState["PM"];

                DataRow dr = dt.Select("itemid ='" + Itemid + "'").FirstOrDefault(); // finds all rows with id==2 and selects first or null if haven't found any
                if (dr != null)
                {
                    if (dr["status"].ToString() == "1")
                    {
                        dr["Status"] = "0";
                    }
                    else
                    {
                        dt.Rows.Remove(dr);//changes the Product_name
                    }

                }
                dt.AcceptChanges();
                ViewState["PM"] = dt;
                double TotalGstAmount = 0;

                double TotalAmount = 0;
                double TotalServiceCharges = 0;



                foreach (DataRow dr_ in dt.Rows)
                {
                    TotalAmount += float.Parse(dr_["Amount"].ToString());
                    TotalGstAmount += float.Parse(dr_["gst"].ToString());
                    TotalServiceCharges += float.Parse(dr_["serviceCharges"].ToString());
                }





                txt_chargedRate.Text = TotalAmount.ToString();
                txt_chargesWalaOtherCharges.Text = TotalServiceCharges.ToString();
                txt_gst.Text = TotalGstAmount.ToString();
                txt_totalAmount.Text = (TotalAmount + TotalGstAmount).ToString();
                LoadGrid2();
            }

        }
        public void LoadGrid2()
        {
            DataTable dt = ViewState["PM"] as DataTable;
            //gv_items.DataSource = null;
            //gv_items.DataBind();

            //gv_items.DataSource = dt;
            //gv_items.DataBind();
        }
        protected void btn_validate_Click(object sender, EventArgs e)
        {
            Tuple<bool, string> cnResponse = CnCheck();
            if (!cnResponse.Item1)
            {
                Alert(cnResponse.Item2);
                return;
            }

            Tuple<bool, string> accResponse = AccCheck();
            if (!accResponse.Item1)
            {
                Alert(accResponse.Item2);
                return;
            }

            Tuple<bool, string> riderResponse = RiderCheck();
            if (!riderResponse.Item1)
            {
                Alert(riderResponse.Item2);
                return;
            }


            DateTime thisDate = DateTime.Parse(booking_date.Text);
            DateTime delvDate = dt_DeliveryDatetime.SelectedDate.Value;

            if (thisDate < DateTime.Today || thisDate > DateTime.Today)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Invalid Date.')", true);
                Errorid.ForeColor = System.Drawing.Color.Red;
                return;
            }

            if (delvDate < DateTime.Today)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Invalid Delivery Date.')", true);
                Errorid.ForeColor = System.Drawing.Color.Red;
                return;
            }


            if (cb_Destination.SelectedValue == "0")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Select Destination')", true);
                Errorid.ForeColor = System.Drawing.Color.Red;
                return;
            }

            DataTable cities = ViewState["Cities"] as DataTable;



            Errorid.Text = "";
            clvar = new Cl_Variables();
            clvar.consignmentNo = txt_ConNo.Text;
            clvar.AccountNo = txt_AccNo.Text;
            clvar.RiderCode = txt_RiderCode.Text;
            clvar.Destination = cb_Destination.SelectedValue; //cities.Select("expressCenterCode = '" + cb_Destination.SelectedValue + "'")[0]["bid"].ToString();//cb_Destination.SelectedValue;// hd_Destination.Value;
            clvar.ServiceTypeName = "Expressions";//cb_ServiceType.SelectedValue;
            clvar.Weight = 0;//float.Parse(txt_Weight.Text);
            clvar.Unit = 1;// cb_Unit.SelectedValue;
                           //clvar.pieces = 1;// txt_Piecies.Text;
            clvar.Zone = HttpContext.Current.Session["ZoneCode"].ToString();

            //Consignee and Consigner Information
            clvar.Consignee = txt_Consignee.Text;
            clvar.ConsigneeCell = txt_ConsigneeCellno.Text;
            clvar.ConsigneeCNIC = "";// txt_ConsigneeCNIC.Text;
            clvar.ConsigneeAddress = txt_Address.Text;
            clvar.Consigner = txt_Consigner.Text;
            clvar.ConsignerCell = txt_ConsignerCellNo.Text;
            clvar.ConsignerCNIC = Txt_ConsignerCNIC.Text;
            clvar.ConsignerAddress = txt_ShipperAddress.Text;
            clvar.consignerAccountNo = txt_AccNo.Text;

            // clvar.Bookingdate = DateTime.Parse(booking_date.Text.ToString());  //DateTime.Parse(dt_DeliveryDatetime.SelectedDate.ToString());
            clvar.BookingDate = booking_date.Text.ToString();


            //  clvar.Bookingdate = DateTime.Parse(booking_date.DbSelectedDate.ToString());  //DateTime.Parse(dt_DeliveryDatetime.SelectedDate.ToString());
            clvar.origin = HttpContext.Current.Session["BranchCode"].ToString();// cb_Origin.SelectedValue;
            clvar.Insurance = "";//txt_insurance.Text;
            clvar.Othercharges = 0;// txt_Othercharges.Text;
            clvar.Day = "";// rb_1.SelectedValue;
            clvar.expresscenter = HttpContext.Current.Session["ExpressCenter"].ToString(); // hd_OriginExpressCenter.Value;
            clvar.destinationExpressCenterCode = cities.Select("BranchCode = '" + cb_Destination.SelectedValue + "'")[0]["ECCode"].ToString(); //cb_Destination.SelectedValue;
            clvar.CustomerClientID = hd_CreditClientID.Value;
            clvar.Con_Type = 10;

            clvar.expressionDeliveryDateTime = DateTime.Parse(dt_DeliveryDatetime.DbSelectedDate.ToString());
            clvar.expressionGreetingCard = chk_greetingCard.Checked;
            clvar.expressionMessage = txt_message.Text;
            clvar.expressionconsignmentRefNumber = txt_consignmentRefNo.Text;
            //clvar.routeCode = dd_consigneeArea.Text;

            //Branch Information
            DataSet BranchGSTInformation = con.BranchGSTInformation(clvar);
            string gst = "";
            if (BranchGSTInformation.Tables[0].Rows.Count != 0)
            {
                gst = BranchGSTInformation.Tables[0].Rows[0]["gst"].ToString();
            }
            //Calculating Modifier

            double Modfier = 0;
            double TotalGstAmount = 0;

            double TotalAmount = 0;
            double TotalServiceCharges = 0;
            //Calculating InsuranceAmount
            double insper = 0;
            //foreach (GridViewRow rp in gv_items.Rows)
            //{
            //    if (rp.Visible == true)
            //    {
            //        double Amount = 0;

            //        double ServiceCharges = 0;
            //        double GstAmount = 0;

            //        Label tx = ((Label)rp.FindControl("lbl_Amount"));
            //        Modfier = double.Parse(tx.Text);

            //        ServiceCharges = 100;
            //        GstAmount = (ServiceCharges / 100) * double.Parse(gst);
            //        Amount = (Modfier) + ServiceCharges + GstAmount;

            //        TotalServiceCharges += ServiceCharges;
            //        TotalGstAmount += GstAmount;
            //        TotalAmount += Amount;
            //    }


            //}

            txt_chargedRate.Text = string.Format("{0:N2}", TotalAmount - TotalGstAmount - TotalServiceCharges);
            txt_chargesWalaOtherCharges.Text = string.Format("{0:N2}", TotalServiceCharges);
            txt_gst.Text = string.Format("{0:N2}", TotalGstAmount);
            txt_totalAmount.Text = string.Format("{0:N2}", TotalAmount);

            clvar.TotalAmount = float.Parse(txt_totalAmount.Text) - float.Parse(txt_gst.Text); ;// TotalAmount;
            clvar.gst = float.Parse(txt_gst.Text);
            clvar.ChargeAmount = 0;
            clvar.Customertype = int.Parse(rbtn_customerType.SelectedValue);
            DataTable dt = (DataTable)ViewState["PM"];
            if (dt.Rows.Count != 0)
            {
                clvar.isPM = true;
                clvar.expresssion = dt;
            }
            else
            {
                clvar.isPM = false;
                clvar.expresssion = null;
                Alert("Select Items");
                Errorid.ForeColor = System.Drawing.Color.Red;
                btn_save.Enabled = false;
                return;
            }



            clvar.destinationCountryCode = "PAK";

            //con.Add_Consignment(clvar);
            clvar.cnScreenId = "11";
            clvar.RiderCode = txt_RiderCode.Text;
            clvar.createdBy = HttpContext.Current.Session["U_ID"].ToString();
            if (dt.Rows.Count > 0)
            {

            }
            clvar.OriginExpressCenterCode = hd_OriginExpressCenter.Value;
            clvar.pieces = int.Parse(dt.Compute("SUM(Qty)", "").ToString());
            clvar.PakageContents = txt_packageContents.Text;

            string error = con.Add_Consignment_Validation(clvar);


            //EC.Add_Consignment(clvar);
            if (error != "")
            {
                Errorid.Text = error;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cannot Create Consignment.')", true);
                Errorid.ForeColor = System.Drawing.Color.Red;
                return;
            }

            //string error1 = EC.Update_ExpressionConsignmentApproval2(clvar);

            error = "";
            error = WriteToDatabase_(clvar);
            if (error != "")
            {
                Errorid.Text = error;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cannot Create Consignment.')", true);
                Errorid.ForeColor = System.Drawing.Color.Red;
                return;
            }
            DataTable pricing = GetExpressionPricing(clvar).Tables[0];
            if (pricing != null)
            {
                if (pricing.Rows.Count > 0)
                {
                    double tAmount = 0;
                    double tGst = 0;
                    double tServiceCharges = 0;

                    object oAmount = pricing.Compute("SUM(amount)", "");
                    object oGst = pricing.Compute("SUM(gst)", "");
                    object oServiceCharges = pricing.Compute("SUM(ServiceCharges)", "");

                    double.TryParse(oAmount.ToString(), out tAmount);
                    double.TryParse(oGst.ToString(), out tGst);
                    double.TryParse(oServiceCharges.ToString(), out tServiceCharges);
                    clvar.TotalAmount = tAmount;
                    txt_chargedRate.Text = Math.Round(tAmount, 2).ToString();

                    txt_gst.Text = Math.Round(tGst, 2).ToString();
                    txt_totalAmount.Text = Math.Round(tAmount + tGst, 2).ToString();
                    clvar.gst = tGst;
                    btn_save.Enabled = true;
                    Alert("Validation Successful. Press Save to Book the Consignment");
                    Errorid.ForeColor = System.Drawing.Color.Green;
                }
                else
                {
                    Errorid.Text = "Tariff Not Available.";
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Tariff Not Available.')", true);
                    Errorid.ForeColor = System.Drawing.Color.Red;
                    btn_save.Enabled = false;
                    return;
                }
                clvar.dayType = 2;
            }
            else
            {
                Errorid.Text = "Tariff Not Available.";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Tariff Not Available.')", true);
                Errorid.ForeColor = System.Drawing.Color.Red;
                btn_save.Enabled = false;
                return;
            }
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Consignment Saved Successfully')", true);

            //ResetAll();
        }

        protected Tuple<bool, string> CnCheck()
        {
            Tuple<bool, string> errTuple;
            //DataTable dt_items = new DataTable();
            ////   dt_items.Columns.Add("ID", typeof(string));
            //dt_items.Columns.Add("Description");
            //dt_items.Columns.Add("id");
            //dt_items.Columns.Add("consignementNo", typeof(string));
            //dt_items.Columns.Add("itemId", typeof(Int64));
            //dt_items.Columns.Add("Qty", typeof(float));
            //dt_items.Columns.Add("amount", typeof(float));
            //dt_items.Columns.Add("message", typeof(string));
            //dt_items.Columns.Add("status", typeof(string));




            //dt_items.Columns.Add("gst", typeof(float));
            //dt_items.Columns.Add("serviceCharges", typeof(float));

            //ViewState["PM"] = dt_items;

            clvar = new Cl_Variables();
            clvar.consignmentNo = txt_ConNo.Text;

            DataSet ds = GetConsignmentForApproval(clvar);
            DataTable dt = EC.ConsignmentExpressionDetail(clvar);
            booking_date.Text = DateTime.Now.ToString("yyyy-MM-dd");
            if (ds.Tables[0].Rows.Count != 0)
            {

                if (ds.Tables[0].Rows[0]["ExpressionDeliveryDateTime"].ToString() == null || ds.Tables[0].Rows[0]["ExpressionDeliveryDateTime"].ToString().Trim() == "")
                {
                    dt_DeliveryDatetime.SelectedDate = DateTime.Now;
                }
                else
                {
                    dt_DeliveryDatetime.SelectedDate = DateTime.Parse(ds.Tables[0].Rows[0]["ExpressionDeliveryDateTime"].ToString());
                }

                txt_ConNo.Text = ds.Tables[0].Rows[0]["consignmentNumber"].ToString();
                txt_AccNo.Text = ds.Tables[0].Rows[0]["consignerAccountNo"].ToString();
                hd_OriginExpressCenter.Value = ds.Tables[0].Rows[0]["originExpressCenter"].ToString();
                if (ds.Tables[0].Rows[0]["customerType"].ToString() == "1")
                {
                    rbtn_customerType.SelectedValue = "1";
                }

                if (ds.Tables[0].Rows[0]["customerType"].ToString() == "2")
                {
                    rbtn_customerType.SelectedValue = "2";
                }

                if (ds.Tables[0].Rows[0]["destinationExpressCenterCode"].ToString() != "")
                {
                    cb_Destination.SelectedValue = ds.Tables[0].Rows[0]["destination"].ToString();
                }
                txt_RiderCode.Text = ds.Tables[0].Rows[0]["riderCode"].ToString();
                txt_pieces.Text = ds.Tables[0].Rows[0]["pieces"].ToString();
                txt_otherCharges.Text = ds.Tables[0].Rows[0]["otherCharges"].ToString();
                txt_packageContents.Text = ds.Tables[0].Rows[0]["PakageContents"].ToString();

                txt_ConsignerCellNo.Text = ds.Tables[0].Rows[0]["consignerCellNo"].ToString();
                Txt_ConsignerCNIC.Text = ds.Tables[0].Rows[0]["consignerCNICNo"].ToString();
                txt_Consigner.Text = ds.Tables[0].Rows[0]["consigner"].ToString();
                txt_ShipperAddress.Text = ds.Tables[0].Rows[0]["shipperAddress"].ToString();

                txt_Consignee.Text = ds.Tables[0].Rows[0]["consignee"].ToString();
                txt_Address.Text = ds.Tables[0].Rows[0]["address"].ToString();
                txt_ConsigneeCellno.Text = ds.Tables[0].Rows[0]["consigneePhoneNo"].ToString();

                txt_chargedRate.Text = ds.Tables[0].Rows[0]["totalAmount"].ToString();
                txt_gst.Text = ds.Tables[0].Rows[0]["gst"].ToString();
                double temptamt = 0;
                double tempgst = 0;
                double.TryParse(ds.Tables[0].Rows[0]["totalAmount"].ToString(), out temptamt);
                double.TryParse(ds.Tables[0].Rows[0]["gst"].ToString(), out tempgst);
                txt_totalAmount.Text = Math.Round((temptamt + tempgst), 2).ToString();
                txt_message.Text = ds.Tables[0].Rows[0]["expressionMessage"].ToString();

                booking_date.Text = DateTime.Now.ToString("yyyy-MM-dd"); //DateTime.Parse(ds.Tables[0].Rows[0]["bookingDate"].ToString()).ToString("yyyy-MM-dd");
                if (ds.Tables[0].Rows[0]["AccountReceivingDate"].ToString() != "")
                {
                    //txt_accountReceivingDate.Text = DateTime.Parse(ds.Tables[0].Rows[0]["AccountReceivingDate"].ToString()).ToString("yyyy-MM-dd");
                }
                else
                {
                    //txt_accountReceivingDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                }

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr["invoiceNumber_"].ToString() != "")
                    {
                        //txt_invoiceNumber.Text = dr["invoiceNumber_"].ToString();
                        //hd_unApproveable.Value = "0";
                        break;
                    }
                }
                //booking_date.Enabled = false;

                //txt_message.Text = ds.Tables[0].Rows[0]["expressionDeliveryDateTime"].ToString();
                //dt_DeliveryDatetime

                //for (int i = 0; i <= dt.Rows.Count; i++)
                //{
                //    gv_items.DataSource = dt.DefaultView;
                //    gv_items.DataBind();
                //}

                DataTable dt_item = (DataTable)ViewState["PM"];
                dt_item.Clear();


                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt_item.NewRow();

                    //dr[0] = dt.Rows[i]["id"].ToString();
                    //dr[1] = dt.Rows[i]["consignementno"].ToString();
                    //dr[2] = dt.Rows[i]["itemid"].ToString();
                    //dr[3] = float.Parse(dt.Rows[i]["itemqty"].ToString());
                    //dr[4] = float.Parse(dt.Rows[i]["amount"].ToString());
                    //dr[5] = dt.Rows[i]["message"].ToString();
                    //dr[6] = dt.Rows[i]["status"].ToString();

                    dr["id"] = dt.Rows[i]["id"].ToString();
                    dr["Description"] = dt.Rows[i]["itemCode"].ToString();// itemRow[0]["Name"].ToString();
                    dr["consignementNo"] = dt.Rows[i]["consignementno"].ToString();
                    dr["itemId"] = Int64.Parse(dt.Rows[i]["itemid"].ToString());
                    dr["Qty"] = float.Parse(dt.Rows[i]["itemqty"].ToString()); ;
                    dr["amount"] = float.Parse(dt.Rows[i]["amount"].ToString());
                    dr["message"] = dt.Rows[i]["message"].ToString();
                    dr["status"] = "2";
                    dr["gst"] = dt.Rows[i]["gst"].ToString();// gst;
                    dr["serviceCharges"] = float.Parse(dt.Rows[i]["serviceCharges"].ToString());// float.Parse(itemRow[0]["serviceCharges"].ToString()) * float.Parse(txt_qty.Text);// "100";


                    //// dr[9] = dt.Rows[i]["modifiedon"].ToString();
                    //if (dt.Rows[i]["modifiedon"].ToString() != "")
                    //{
                    //    dr[9] = dt.Rows[i]["modifiedon"].ToString();
                    //}
                    //else
                    //{
                    //    dr[9] = DBNull.Value;
                    //}
                    //dr[10] = dt.Rows[i]["modifiedby"].ToString();
                    //dr[11] = float.Parse(dt.Rows[i]["gst"].ToString());// gst;
                    //dr[12] = float.Parse("100");// "100";


                    dt_item.Rows.Add(dr);
                    dt.AcceptChanges();
                    ViewState["PM"] = dt_item;

                }

                //txt_AccNo_TextChanged(this, e);

                errTuple = new Tuple<bool, string>(false, "Consignment Number Already Exists");
                //gv_items.DataSource = dt_item;// dt.DefaultView;
                //gv_items.DataBind();
                return errTuple;
                //btn_save.Visible = false;
                //btn_update.Visible = true;

            }
            else
            {
                //btn_save.Visible = true;
                //btn_update.Visible = false;

                //    txt_ConNo.Text = "";
                //txt_AccNo.Text = "";

                //rbtn_customerType.SelectedValue = "";

                //cb_Destination.SelectedValue = "0";
                ////    txt_consignmentRefNo.Text = ds.Tables[0].Rows[0][""].ToString();
                //txt_RiderCode.Text = "";
                ////  hd_OriginExpressCenter = ds.Tables[0].Rows[0][""].ToString();
                //txt_pieces.Text = "";
                //txt_otherCharges.Text = "";
                //txt_packageContents.Text = "";

                //txt_ConsignerCellNo.Text = "";
                //Txt_ConsignerCNIC.Text = "";
                //txt_Consigner.Text = "";
                //txt_ShipperAddress.Text = "";

                //txt_Consignee.Text = "";
                //txt_Address.Text = "";
                //// dd_consigneeArea.Text = ds.Tables[0].Rows[0][""].ToString();
                //txt_ConsigneeCellno.Text = "";

                //txt_chargedRate.Text = "";
                ////   txt_chargesWalaOtherCharges.Text = ds.Tables[0].Rows[0][""].ToString();
                //txt_gst.Text = "";
                //txt_totalAmount.Text = "";
                //txt_message.Text = "";
                //booking_date.Text = DateTime.Now.ToString("yyyy-MM-dd");
                ////ViewState["PM"] = null;


                //txt_AccNo.Focus();
                errTuple = new Tuple<bool, string>(true, "OK");
                return errTuple;
            }
        }

        protected Tuple<bool, string> AccCheck()
        {
            Tuple<bool, string> errTuple;
            if (txt_AccNo.Text != "")
            {
                clvar.Branch = HttpContext.Current.Session["BranchCode"].ToString();
                clvar.AccountNo = txt_AccNo.Text;
                DataSet ds = CustomerInformation(clvar);
                if (ds.Tables.Count != 0)
                {
                    if (ds.Tables[0].Rows.Count != 0)
                    {
                        if (txt_AccNo.Text != "0")
                        {
                            txt_Consigner.Text = ds.Tables[0].Rows[0]["Name"].ToString();
                            txt_ConsignerCellNo.Text = ds.Tables[0].Rows[0]["PhoneNo"].ToString();
                            txt_ShipperAddress.Text = ds.Tables[0].Rows[0]["address"].ToString();
                        }
                        hd_CreditClientID.Value = ds.Tables[0].Rows[0]["id"].ToString();


                        if (ds.Tables[0].Rows[0]["accountNo"].ToString() == "0")
                        {
                            this.rbtn_customerType.SelectedValue = "1";
                        }
                        else
                        {
                            this.rbtn_customerType.SelectedValue = "2";
                        }
                        if (ds.Tables[0].Rows[0]["isAPIClient"].ToString() == "1" || ds.Tables[0].Rows[0]["isAPIClient"].ToString().ToUpper() == "TRUE")
                        {
                            //hd_unApproveable.Value = "0";
                            txt_AccNo.Text = "";
                            txt_AccNo.Focus();
                            //Alert("This Account is Reserved only for COD Portal.");
                            errTuple = new Tuple<bool, string>(false, "This Account is Reserved only for COD Portal.");
                            return errTuple;
                        }
                        errTuple = new Tuple<bool, string>(true, "OK");
                        return errTuple;
                        //cb_Destination.Focus();
                    }
                    else
                    {
                        //Alert("Invalid Account No");
                        errTuple = new Tuple<bool, string>(false, "Invalid Account No.");
                        return errTuple;
                        //return;
                    }
                }
                else
                {
                    errTuple = new Tuple<bool, string>(false, "Account Not Found.");
                    return errTuple;
                }
            }
            else
            {
                errTuple = new Tuple<bool, string>(false, "Enter Account Number");
                txt_AccNo.Focus();
                return errTuple;
            }
        }

        protected Tuple<bool, string> RiderCheck()
        {
            Tuple<bool, string> errTuple;
            string query = "SELECT * FROM RIDERS r where r.riderCode = '" + txt_RiderCode.Text.Trim() + "' and r.branchID = '" + HttpContext.Current.Session["BranchCode"].ToString() + "' and r.status = '1'";
            DataTable dt = new DataTable();
            try
            {
                SqlConnection con = new SqlConnection(clvar.Strcon());
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                sda.Fill(dt);
            }
            catch (Exception ex)
            {
                errTuple = new Tuple<bool, string>(false, ex.Message);
                return errTuple;
            }
            if (dt.Rows.Count > 0)
            {
                hd_OriginExpressCenter.Value = dt.Rows[0]["ExpressCenterID"].ToString();
                //txt_weight.Focus();
                booking_date.Focus();
                errTuple = new Tuple<bool, string>(true, "OK");
                return errTuple;
            }
            else
            {
                hd_OriginExpressCenter.Value = "0";
                errTuple = new Tuple<bool, string>(false, "Invalid Rider Code");
                txt_RiderCode.Text = "";
                txt_RiderCode.Focus();
                return errTuple;

            }
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
                if (txt_ConNo.Text != string.Empty)
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
                if (txt_ConNo.Text != string.Empty)
                {
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
                }
            }
            catch (Exception Err)
            {
                //listBox1.Items.Add("Failed " + Err + "Mobile Number " + clvar.MobileNumber + " and Response " + resp);
            }

        }
    }
}