using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using iTextSharp.text;
using iTextSharp.text.pdf;
using MRaabta.App_Code;

namespace MRaabta.Files
{
    public partial class CODPayments : System.Web.UI.Page
    {
        Variable clvar = new Variable();
        Cl_Variables clvar_ = new Cl_Variables();
        bayer_Function b_fun = new bayer_Function();
        MISReport mis = new MISReport();

        int count = 0;
        Boolean flag = false;
        int page = 1;



        string  query, paidsts, reportid;


        protected void Page_Load(object sender, EventArgs e)
        {

            error_msg.Text = "";
            Errorid.Text = "";
            if (!IsPostBack)
            {

                //UpdatePanel mainPanel = Page.Master.FindControl("mainPanel") as UpdatePanel;
                //UpdatePanelControlTrigger trigger = new PostBackTrigger();
                //trigger.ControlID = btn_print.UniqueID;
                //mainPanel.Triggers.Add(trigger);

                //trigger = new PostBackTrigger();
                //trigger.ControlID = Button4.UniqueID;
                //mainPanel.Triggers.Add(trigger);
                string script = String.Format("javascript:return DeleteGridView('{0}');", GridView2.ClientID);
                //txtPercentage.Attributes.Add("onkeydown", script);
                Get_CreditClient();


            }

        }

        public void Get_CreditClient()
        {
            DataSet ds_zone = Get_CreditClientAccounts(clvar);

            if (ds_zone.Tables[0].Rows.Count != 0)
            {
                dd_customer.DataTextField = "name";
                dd_customer.DataValueField = "accountno";
                dd_customer.DataSource = ds_zone.Tables[0].DefaultView;
                dd_customer.DataBind();
            }
        }



        protected void Btn_Search_Click(object sender, EventArgs e)
        {
            if (type.SelectedValue == "html")
            {

                clvar = new Variable();
                string[] split_number = dd_customer.SelectedValue.Split('_');
                clvar.ACNumber = split_number[0];
                if (paidstatus.Items.FindByValue("PAID").Selected == true)
                {
                    #region Marking Paid
                    cnNumber.Style.Add("display", "none");
                    ChequeCriteria.Style.Add("display", "none");
                    //ChequeNo.Style.Add("display", "none");
                    paidsts = "b.PaidStatus = 'UNPAID' AND b.CHEQUENO IS NULL";

                    if (split_number[1] == "OLD")
                    {
                        query = " INNER JOIN CODCONSIGNMENTDETAIL CD\n" +
                                "    ON CD.CONSIGNMENTNUMBER = C.CONSIGNMENTNUMBER\n";
                    }

                    if (split_number[1] == "NEW")
                    {
                        query = " INNER JOIN CODCONSIGNMENTDETAIL_NEW CD\n" +
                                "    ON CD.CONSIGNMENTNUMBER = C.CONSIGNMENTNUMBER\n";
                    }
                    #endregion
                }
                #region unpaid
                //else if (paidstatus.Items.FindByValue("UNPAID").Selected == true)
                //{
                //    #region Marking Unpaid and Editing
                //    cnNumber.Style.Add("display", "block");
                //    ChequeCriteria.Style.Add("display", "none");
                //    //ChequeNo.Style.Add("display", "block");
                //    if (txt_cnNumber.Text.Trim() == "")
                //    {
                //        Alert("Enter Consignment Number", "Red");
                //        GridView2.DataSource = null;
                //        GridView2.DataBind();
                //        return;
                //    }

                //    paidsts = "b.PaidStatus = 'PAID'";

                //    if (split_number[1] == "OLD")
                //    {
                //        query = " INNER JOIN CODCONSIGNMENTDETAIL CD\n" +
                //                "    ON CD.CONSIGNMENTNUMBER = C.CONSIGNMENTNUMBER\n";
                //    }

                //    if (split_number[1] == "NEW")
                //    {
                //        query = " INNER JOIN CODCONSIGNMENTDETAIL_NEW CD\n" +
                //                "    ON CD.CONSIGNMENTNUMBER = C.CONSIGNMENTNUMBER\n";
                //    }
                //    paidsts += " AND b.ConsignmentNumber = '" + txt_cnNumber.Text + "'";
                //    #endregion
                //} 
                #endregion
                #region Edit Paid
                //else if (paidstatus.Items.FindByValue("EDIT").Selected == true)
                //{
                //    #region Editing Cheque Number
                //    cnNumber.Style.Add("display", "none");
                //    ChequeCriteria.Style.Add("display", "block");
                //    //ChequeNo.Style.Add("display", "none");
                //    paidsts = "b.PaidStatus = 'PAID'";
                //    if (rbtn_ChequeCriteria.SelectedValue == "0")
                //    {
                //        //if (dd_start_date.Text.Trim() == "" || dd_end_date.Text.Trim() == "")
                //        //{
                //        //    Alert("Please Provide Dates in case of Without Cheque # Selection", "Red");
                //        //    return;
                //        //}
                //        //clvar.StartDate += " AND c.BOOKINGDATE BETWEEN '" + dd_start_date.Text + "' AND '" + dd_end_date.Text + "'";
                //        paidsts += " AND (b.ChequeNo is null OR b.ChequeNo = '' )";
                //    }
                //    else
                //    {
                //        paidsts += " AND b.ChequeNo = '" + txt_chequeNo.Text + "'";
                //    }

                //    if (split_number[1] == "OLD")
                //    {
                //        query = " INNER JOIN CODCONSIGNMENTDETAIL CD\n" +
                //                "    ON CD.CONSIGNMENTNUMBER = C.CONSIGNMENTNUMBER\n";
                //    }

                //    if (split_number[1] == "NEW")
                //    {
                //        query = " INNER JOIN CODCONSIGNMENTDETAIL_NEW CD\n" +
                //                "    ON CD.CONSIGNMENTNUMBER = C.CONSIGNMENTNUMBER\n";
                //    }
                //    #endregion
                //} 
                #endregion
                else if (paidstatus.Items.FindByValue("PRINT").Selected == true)
                {
                    #region Printing
                    paidsts = "b.PaidStatus = 'PAID'";
                    cnNumber.Style.Add("display", "none");
                    ChequeCriteria.Style.Add("display", "none");
                    ChequeNo.Style.Add("display", "block");

                    if (txt_chequeNo.Text.Trim() == "")
                    {
                        Alert("Enter Payment Number", "Red");
                        return;
                    }
                    if (split_number[1] == "OLD")
                    {
                        query = " INNER JOIN CODCONSIGNMENTDETAIL CD\n" +
                                "    ON CD.CONSIGNMENTNUMBER = C.CONSIGNMENTNUMBER\n";
                    }

                    if (split_number[1] == "NEW")
                    {
                        query = " INNER JOIN CODCONSIGNMENTDETAIL_NEW CD\n" +
                                "    ON CD.CONSIGNMENTNUMBER = C.CONSIGNMENTNUMBER\n";
                    }

                    //if (rbtn_ChequeCriteria.SelectedValue == "0")
                    //{
                    //    //ChequeNo.Style.Add("display", "none");
                    //    //paidsts += " and ( b.ChequeNo is NULL OR b.ChequeNo = '')";
                    //    paidsts += " and b.ChequeNo = '" + txt_chequeNo.Text + "'";
                    //}
                    //else
                    //{
                    //    //ChequeNo.Style.Add("display", "block");
                    //    if (txt_chequeNo.Text.Trim() == "")
                    //    {
                    //        Alert("Enter Cheque Number", "Red");
                    //        return;
                    //    }
                    //    paidsts += " and b.ChequeNo = '" + txt_chequeNo.Text + "'";
                    //}

                    paidsts += " and b.ChequeNo = '" + txt_chequeNo.Text + "'";
                    #endregion
                }


                //if (paidstatus.Items.FindByValue("UNPAID").Selected == true)
                //{
                //    paidsts = "b.PaidStatus = 'UNPAID'";
                //}



                //clvar.StartDate = date;
                clvar.Status = query;
                clvar.Seal = paidsts;

                double invoice = 0;
                double codPayable = 0;
                if (clvar.StartDate != null && clvar.Status != "" || 1 == 1)
                {
                    DataSet header = Get_CODPaymentReport(clvar);
                    Cl_Variables cvar = new Cl_Variables();
                    cvar.AccountNo = split_number[0];
                    DataTable creditClient = Get_CreditClientID(cvar);
                    cvar.CreditClientID = creditClient.Rows[0][0].ToString();
                    DataTable invoiceDetails = GetInvoicesForVoucherNonCentralized(cvar);
                    if (invoiceDetails != null)
                    {
                        if (invoiceDetails.Rows.Count > 0)
                        {
                            gv_invoiceDetail.DataSource = invoiceDetails;
                            gv_invoiceDetail.DataBind();

                            foreach (GridViewRow row in gv_invoiceDetail.Rows)
                            {
                                string tempInvoiceAmount = (row.FindControl("lbl_outstanding") as Label).Text;
                                double temp = 0;
                                double.TryParse(tempInvoiceAmount, out temp);
                                invoice += temp;
                            }
                        }
                        else
                        {
                            gv_invoiceDetail.DataSource = null;
                            gv_invoiceDetail.DataBind();
                            invoiceDetails.Rows.Add(invoiceDetails.NewRow());
                            gv_invoiceDetail.DataSource = invoiceDetails;
                            gv_invoiceDetail.DataBind();
                        }
                    }
                    if (header.Tables[0].Rows.Count != 0)
                    {
                        reportid = "103";


                        lbl_branchName.Text = header.Tables[0].Rows[0]["ORGZONE"].ToString();
                        lbl_date.Text = header.Tables[0].Rows[0]["BOOKINGDATE"].ToString();
                        lbl_accountName.Text = header.Tables[0].Rows[0]["CUSTOMERNAME"].ToString();
                        lbl_accountNumber.Text = header.Tables[0].Rows[0]["ACCOUNTNO"].ToString();
                        error_msg.Text = "";
                        #region MyRegion
                        //#region MyRegion
                        ////#region MyRegion


                        //Literal lt_Main = new Literal();
                        ////lt_Main.Text += "</b>";
                        ////lt_Main.Text += "</tr></table>";
                        //lt_Main.Text += "<table class='input-form' style='Width:100% !important;'>";
                        //lt_Main.Text += "<tr>"; //phela table ha jisma se variabble uthane ha or grid ke sab se uper aien
                        //lt_Main.Text += "<td style=\"width:16% !important; float:left;\"><b> Orign:</b></td>";
                        //lt_Main.Text += "<td style=\"width:45% !important; float:left;\">" + header.Tables[0].Rows[0]["ORGZONE"].ToString() + "</td>";
                        //lt_Main.Text += "<td style=\"width:10% !important; float:left;\"><b>Date</b></font></td>";
                        //lt_Main.Text += "<td style=\"width:25% !important; float:left;\">" + header.Tables[0].Rows[0]["BOOKINGDATE"].ToString() + "</td>";
                        //lbl_branchName.Text = header.Tables[0].Rows[0]["ORGZONE"].ToString();
                        //lbl_date.Text = header.Tables[0].Rows[0]["BOOKINGDATE"].ToString();
                        ////Store your database DateTime value into a variable
                        ////   DateTime dt = DateTime.ParseExact(dd_start_date.Text, "yyyyMMdd", null);
                        ////   DateTime txtMyDate = DateTime.Parse(dd_start_date.Text);


                        ////   DateTime dateValue = DateTime.ParseExact(dd_start_date.Text, "M/d/yyyy", CultureInfo.InvariantCulture);


                        ////    lt_Main.Text += "<td width=\"36%\">" + txtMyDate + " TO " + dd_end_date.Text + "</td>";
                        //lt_Main.Text += "</tr>";

                        //lt_Main.Text += "<tr>";

                        //lt_Main.Text += "<td width=\"16% !important; float:left;\"><b> Customer:</b></td>";//phela table ha jisma se variabble uthane ha or grid ke sab se uper aien total 4 varaible hain
                        //lt_Main.Text += "<td width=\"47% !important; float:left;\">" + header.Tables[0].Rows[0]["CUSTOMERNAME"].ToString() + "</td>";
                        //lt_Main.Text += "<td width=\"10% !important; float:left;\"><b>Account</b></font></td>";
                        //lt_Main.Text += "<td width=\"16% !important; float:left;\">" + header.Tables[0].Rows[0]["ACCOUNTNO"].ToString() + "</td>";
                        //lt_Main.Text += "</tr>";
                        //lbl_accountName.Text = header.Tables[0].Rows[0]["CUSTOMERNAME"].ToString();
                        //lbl_accountNumber.Text = header.Tables[0].Rows[0]["ACCOUNTNO"].ToString();
                        //lt_Main.Text += "</table>";
                        ////myHolder1.Dispose();
                        ////myHolder1.Controls.Clear();
                        ////myHolder1.Controls.Add(lt_Main);



                        ////Literal lt_Main1 = new Literal();
                        ////lt_Main1.Text += "<table class='detail' cellspacing='0'>";
                        ////lt_Main1.Text += "<tr>";
                        ////lt_Main1.Text += "<th width=\"3%\"><b> S.NO</b></th>";
                        ////lt_Main1.Text += "<th width=\"14%\"><b> CONSIGNMENT NUMBER</b></th>";
                        ////lt_Main1.Text += "<th width=\"14%\"><b>BOOKING DATE</b></font></th>";
                        ////lt_Main1.Text += "<th width=\"14%\"><b>STATUS</b></font></th>";
                        ////lt_Main1.Text += "<th width=\"14%\"><b>RR NUMBER</b></font></th>";
                        ////lt_Main1.Text += "<th width=\"8%\"><b>COD AMOUNT</b></font></th>";
                        ////lt_Main1.Text += "<th width=\"8%\"><b>PAID STATUS</b></font></th>";
                        ////lt_Main1.Text += "<th width=\"8%\"><b>INVOICE NO</b></font></th>";
                        ////lt_Main1.Text += "</tr>";

                        ////for (int j = 0; j < header.Tables[0].Rows.Count; j++)
                        ////{
                        ////    lt_Main1.Text += "<tr>";
                        ////    lt_Main1.Text += "<td>" + (j + 1).ToString() + "</td>";
                        ////    lt_Main1.Text += "<td>" + header.Tables[0].Rows[j]["CONSIGNMENT NUMBER"].ToString() + "</td>";
                        ////    lt_Main1.Text += "<td>" + header.Tables[0].Rows[j]["BOOKING DATE"].ToString() + "</td>";
                        ////    lt_Main1.Text += "<td>" + header.Tables[0].Rows[j]["RR STATUS"].ToString() + "</td>";
                        ////    lt_Main1.Text += "<td>" + header.Tables[0].Rows[j]["Payment Voucher ID"].ToString() + "</td>";
                        ////    lt_Main1.Text += "<td>" + string.Format("{0:N0}", header.Tables[0].Rows[j]["COD AMOUNT"].ToString()) + "</td>";
                        ////    lt_Main1.Text += "<td>" + header.Tables[0].Rows[j]["PaidStatus"].ToString() + "</td>";
                        ////    lt_Main1.Text += "<td>" + header.Tables[0].Rows[j]["invoicenumber"].ToString() + "</td>";

                        ////    CODTotalAmount += double.Parse(header.Tables[0].Rows[j]["COD AMOUNT"].ToString());
                        ////    //total payable ammount
                        ////    int i = 1;
                        ////    int x = 1;
                        ////    int a = 1;
                        ////    // if (header.Tables[0].Rows[j]["RR STATUS"].ToString() == "DELIVERED")
                        ////    if (header.Tables[0].Rows[j]["RR STATUS"].ToString() == "D-DELIVERED")
                        ////    {
                        ////        Delivered += i++;
                        ////        DeliveredAmount += double.Parse(header.Tables[0].Rows[j]["COD AMOUNT"].ToString());
                        ////        //shipment diliver

                        ////        if (header.Tables[0].Rows[j]["DeliveredCN"].ToString() == "1")
                        ////        {
                        ////            DeliveredCN += x++;
                        ////            DeliveredwithRRAmount += double.Parse(header.Tables[0].Rows[j]["COD AMOUNT"].ToString());
                        ////        }

                        ////        if (header.Tables[0].Rows[j]["DeliveredCN"].ToString() == "0")
                        ////        {
                        ////            DeliveredCN2 += x++;
                        ////            DeliveredwithoutRRAmount += double.Parse(header.Tables[0].Rows[j]["COD AMOUNT"].ToString());
                        ////        }
                        ////    }

                        ////    lt_Main1.Text += "</tr>";
                        ////}

                        ////myHolder2.Dispose();
                        ////myHolder2.Controls.Clear();
                        ////myHolder2.Controls.Add(lt_Main1);
                        ////lt_Main1.Text += "</table>";



                        ////Literal lt_Main2 = new Literal();
                        ////lt_Main2.Text += "<table class='summary'>";
                        ////lt_Main2.Text += "<tr>";
                        ////lt_Main2.Text += "<td width=\"20%\"><b>SUMMARY</b></td>";
                        ////lt_Main2.Text += "<td width=\"5%\"><b>QTY</b></td>";
                        ////lt_Main2.Text += "<td width=\"8%\"><b>AMOUNT</b></font></td>";
                        ////lt_Main2.Text += "</tr>";
                        //////y
                        ////lt_Main2.Text += "<tr>";
                        ////lt_Main2.Text += "<td>Shipment Booked </td>";
                        ////lt_Main2.Text += "<td>" + header.Tables[0].Rows.Count + "</td>"; //shipment booked
                        ////lt_Main2.Text += "<td>" + string.Format("{0:N0}", CODTotalAmount) + "</td>";
                        ////lt_Main2.Text += "</tr>";

                        ////lt_Main2.Text += "<tr>";
                        ////lt_Main2.Text += "<td>Shipment Delivered </td>";
                        ////lt_Main2.Text += "<td>" + Delivered + "</td>";
                        ////lt_Main2.Text += "<td>" + string.Format("{0:N0}", DeliveredAmount) + "</td>";
                        ////lt_Main2.Text += "</tr>";

                        ////lt_Main2.Text += "<tr>";
                        ////lt_Main2.Text += "<td style='padding: 0px 0px 0px 25px;'>Delivered with RR </td>";
                        ////lt_Main2.Text += "<td>" + DeliveredCN + "</td>"; //diliver rr
                        ////lt_Main2.Text += "<td>" + string.Format("{0:N0}", DeliveredwithRRAmount) + "</td>";
                        ////lt_Main2.Text += "</tr>";

                        ////lt_Main2.Text += "<tr>";
                        ////lt_Main2.Text += "<td style='padding: 0px 0px 0px 25px;'>Delivered without RR </td>"; //
                        ////lt_Main2.Text += "<td>" + DeliveredCN2 + "</td>"; //diliver withOutrr
                        ////lt_Main2.Text += "<td>" + string.Format("{0:N0}", DeliveredwithoutRRAmount) + "</td>";
                        ////lt_Main2.Text += "</tr>";

                        ////lt_Main2.Text += "<tr>";
                        ////lt_Main2.Text += "<td><b>Total Payable (with & without RR) </b></td>";
                        ////lt_Main2.Text += "<td></td>";
                        ////lt_Main2.Text += "<td><b>" + string.Format("{0:N0}", DeliveredAmount) + "</b></td>";
                        ////lt_Main2.Text += "</tr>";

                        ////myHolder3.Dispose();
                        ////myHolder3.Controls.Clear();
                        ////myHolder3.Controls.Add(lt_Main2);
                        ////lt_Main2.Text += "</table>";




                        ////// ye last ma 2 lines chaye hain          
                        ////Literal lt_Main3 = new Literal();
                        ////lt_Main3.Text += "<table class='note'>";
                        ////lt_Main3.Text += "<tr>";
                        ////lt_Main3.Text += "<td width=\"20%\">*Approval of Director 3PL will be required for payment of those shipments whose RR not available.</td>";
                        ////lt_Main3.Text += "</tr>";
                        ////lt_Main3.Text += "<tr>";
                        ////lt_Main3.Text += "<td width=\"5%\">**No service charges while payment will be deducted, rather monthly invoice to Customer will be submitted for payment as per routine business.</td>";
                        ////lt_Main3.Text += "</tr>";

                        ////myHolder4.Dispose();
                        ////myHolder4.Controls.Clear();
                        ////myHolder4.Controls.Add(lt_Main3);
                        ////lt_Main3.Text += "</table>";
                        //////ye necha signature wala part
                        ////Literal lt_Main4 = new Literal();
                        ////lt_Main4.Text += "<table class='signature'>";
                        ////lt_Main4.Text += "<tr>";
                        ////lt_Main4.Text += "<td width=\"20%\" style='position: relative; border-top: 3px solid rgb(0, 0, 0);'><b>Head of Operations</b></td>";
                        ////lt_Main4.Text += "</tr>";
                        ////lt_Main4.Text += "<tr>";
                        ////lt_Main4.Text += "<td width=\"5%\">COD Business</td>";
                        ////lt_Main4.Text += "</tr>";
                        ////lt_Main4.Text += "<tr><td style='height: 42px;'></td></tr>";
                        ////lt_Main4.Text += "<tr><td style='border-top: 3px solid rgb(0, 0, 0);'><b>Director 3PL & Special Projects</b></td></tr>";

                        ////myHolder5.Dispose();
                        ////myHolder5.Controls.Clear();
                        ////myHolder5.Controls.Add(lt_Main4);
                        ////lt_Main4.Text += "</table>";




                        ////#endregion
                        //#endregion 
                        #endregion

                        if (header.Tables[0].Rows.Count > 0)
                        {
                            GridView2.DataSource = header.Tables[0];
                            GridView2.DataBind();


                            if (paidstatus.SelectedValue == "PAID" || paidstatus.SelectedValue == "PRINT")
                            {
                                (GridView2.HeaderRow.FindControl("chk_paidAll") as CheckBox).Enabled = false;
                            }
                            else
                            {
                                (GridView2.HeaderRow.FindControl("chk_paidAll") as CheckBox).Enabled = true;
                            }

                            foreach (GridViewRow row in GridView2.Rows)
                            {
                                string tempVoucherAmount = row.Cells[8].Text;
                                double temp = 0;

                                HiddenField deliveryStatus = row.FindControl("hd_DeliveryStatus") as HiddenField;
                                CheckBox chk = row.FindControl("chk_paid") as CheckBox;
                                if (paidstatus.SelectedValue.ToUpper() == "PAID")
                                {

                                    if (row.Cells[4].Text.Trim() == "-" || row.Cells[4].Text.Trim() == "" || row.Cells[4].Text.Trim() == "&nbsp;")
                                    {
                                        chk.Checked = false;
                                        chk.Enabled = false;
                                    }
                                    else
                                    {
                                        chk.Enabled = false;
                                        if (deliveryStatus.Value == "55")
                                        {
                                            chk.Checked = true;

                                            double.TryParse(tempVoucherAmount, out temp);
                                            codPayable += temp;
                                        }
                                    }
                                }
                                else if (paidstatus.SelectedValue.ToUpper() == "PRINT")
                                {
                                    chk.Checked = true;
                                    chk.Enabled = false;
                                }

                            }

                            Label lbl_footerInvoice = gv_invoiceDetail.FooterRow.FindControl("lbl_fTotalInvAmount") as Label;
                            Label lbl_footerCodPayable = gv_invoiceDetail.FooterRow.FindControl("lbl_fTotalCODPayable") as Label;
                            Label lbl_footerBalance = gv_invoiceDetail.FooterRow.FindControl("lbl_fBalance") as Label;
                            lbl_footerInvoice.Text = String.Format("{0:N2}", invoice);
                            lbl_footerCodPayable.Text = String.Format("{0:N2}", codPayable);
                            lbl_footerBalance.Text = String.Format("{0:N2}", codPayable - invoice);
                            if (invoice > codPayable)
                            {
                                lbl_footerCodPayable.BackColor = System.Drawing.Color.Pink;
                                lbl_footerInvoice.BackColor = System.Drawing.Color.Pink;
                                gv_invoiceDetail.FooterRow.BackColor = System.Drawing.Color.Pink;
                            }
                            else
                            {
                                lbl_footerCodPayable.BackColor = System.Drawing.Color.LightGreen;
                                lbl_footerInvoice.BackColor = System.Drawing.Color.LightGreen;
                                gv_invoiceDetail.FooterRow.BackColor = System.Drawing.Color.LightGreen;
                            }
                        }
                        else
                        {
                            GridView2.DataSource = null;
                            GridView2.DataBind();
                        }
                    }
                    else
                    {
                        GridView2.DataSource = null;
                        GridView2.DataBind();
                        lbl_branchName.Text = "";
                        lbl_date.Text = "";
                        lbl_accountName.Text = "";
                        lbl_accountNumber.Text = "";
                        error_msg.Text = "No Record Found...";

                    }
                }
                else
                {

                    error_msg.Text = "Select Filter";
                }



            }
        }

        #region PDF WALA KAAM
        //public void ExporttopdfSummary(string origin, string date, string account, string customer, double sbqty, double sbamt, double sdqty, double sdamt, double drrqty, double drramt, double dwrrqty, double dwrramt, double totalwrr)
        //{
        //    Document document = new Document(PageSize.A4, 88f, 88f, 10f, 10f);
        //    iTextSharp.text.Font NormalFont = FontFactory.GetFont("Courier New", 6, iTextSharp.text.Font.NORMAL); //FontFactory.GetFont("Courier New", 6, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
        //    using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
        //    {
        //        PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
        //        Phrase phrase = null;
        //        PdfPCell cell = null;
        //        PdfPTable table_up = null;
        //        PdfPTable table = null;
        //        PdfPTable table_down = null;
        //        PdfPTable table_pageNum = null;
        //        //iTextSharp.text.BaseColor color = null;

        //        document.Open();

        //        table_up = new PdfPTable(2);
        //        table_up.TotalWidth = 550f;
        //        table_up.LockedWidth = true;
        //        //table.SetWidths(new float[] { 0.7f, 0.7f, 0.7f, 0.7f });             

        //        cell = PhraseCell(new Phrase("Customer's Payment Summary", FontFactory.GetFont("Courier New", 16, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_CENTER);
        //        cell.Colspan = 2;
        //        cell.PaddingBottom = 20f;
        //        table_up.AddCell(cell);

        //        cell = PhraseCell(new Phrase("Origin:            " + origin, FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_LEFT);
        //        cell.Colspan = 1;
        //        //cell.BorderColorTop = BaseColor.BLACK;
        //        //cell.BorderColorLeft = BaseColor.BLACK;
        //        cell.Padding = 5f;
        //        cell.UseVariableBorders = true;
        //        table_up.AddCell(cell);

        //        cell = PhraseCell(new Phrase("Date:            " + date, FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_LEFT);
        //        cell.Colspan = 1;
        //        //cell.BorderColorTop = BaseColor.BLACK;
        //        //cell.BorderColorRight = BaseColor.BLACK;
        //        cell.Padding = 5f;
        //        cell.UseVariableBorders = true;
        //        table_up.AddCell(cell);

        //        cell = PhraseCell(new Phrase("Customer:        " + customer, FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_LEFT);
        //        cell.Colspan = 1;
        //        //cell.BorderColorBottom = BaseColor.BLACK;
        //        //cell.BorderColorLeft = BaseColor.BLACK;
        //        cell.Padding = 5f;
        //        cell.UseVariableBorders = true;
        //        table_up.AddCell(cell);

        //        cell = PhraseCell(new Phrase("Account #:     " + account, FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_LEFT);
        //        cell.Colspan = 1;
        //        //cell.BorderColorBottom = BaseColor.BLACK;
        //        //cell.BorderColorRight = BaseColor.BLACK;
        //        cell.Padding = 5f;
        //        cell.UseVariableBorders = true;
        //        table_up.AddCell(cell);


        //        //Center table

        //        int c = 1;

        //        if (type.SelectedValue == "html")
        //        {
        //            clvar = new Variable();
        //            string[] split_number = dd_customer.SelectedValue.Split('_');
        //            clvar.ACNumber = split_number[0];
        //            if (paidstatus.Items.FindByValue("PAID").Selected == true)
        //            {
        //                #region Marking Paid
        //                cnNumber.Style.Add("display", "none");
        //                ChequeCriteria.Style.Add("display", "none");
        //                ChequeNo.Style.Add("display", "none");
        //                paidsts = "b.PaidStatus = 'UNPAID'";

        //                if (split_number[1] == "OLD")
        //                {
        //                    query = " INNER JOIN CODCONSIGNMENTDETAIL CD\n" +
        //                            "    ON CD.CONSIGNMENTNUMBER = C.CONSIGNMENTNUMBER\n";
        //                }

        //                if (split_number[1] == "NEW")
        //                {
        //                    query = " INNER JOIN CODCONSIGNMENTDETAIL_NEW CD\n" +
        //                            "    ON CD.CONSIGNMENTNUMBER = C.CONSIGNMENTNUMBER\n";
        //                }
        //                #endregion
        //            }
        //            else if (paidstatus.Items.FindByValue("UNPAID").Selected == true)
        //            {
        //                #region Marking Unpaid and Editing
        //                cnNumber.Style.Add("display", "block");
        //                ChequeCriteria.Style.Add("display", "none");
        //                ChequeNo.Style.Add("display", "block");
        //                if (txt_cnNumber.Text.Trim() == "")
        //                {
        //                    Alert("Enter Consignment Number", "Red");
        //                    return;
        //                }

        //                paidsts = "b.PaidStatus = 'PAID'";

        //                if (split_number[1] == "OLD")
        //                {
        //                    query = " INNER JOIN CODCONSIGNMENTDETAIL CD\n" +
        //                            "    ON CD.CONSIGNMENTNUMBER = C.CONSIGNMENTNUMBER\n";
        //                }

        //                if (split_number[1] == "NEW")
        //                {
        //                    query = " INNER JOIN CODCONSIGNMENTDETAIL_NEW CD\n" +
        //                            "    ON CD.CONSIGNMENTNUMBER = C.CONSIGNMENTNUMBER\n";
        //                }
        //                paidsts += " AND b.ConsignmentNumber = '" + txt_cnNumber.Text + "'";
        //                #endregion
        //            }
        //            else if (paidstatus.Items.FindByValue("PRINT").Selected == true)
        //            {
        //                #region Printing
        //                paidsts = "b.PaidStatus = 'PAID'";
        //                cnNumber.Style.Add("display", "none");
        //                ChequeCriteria.Style.Add("display", "block");

        //                if (split_number[1] == "OLD")
        //                {
        //                    query = " INNER JOIN CODCONSIGNMENTDETAIL CD\n" +
        //                            "    ON CD.CONSIGNMENTNUMBER = C.CONSIGNMENTNUMBER\n";
        //                }

        //                if (split_number[1] == "NEW")
        //                {
        //                    query = " INNER JOIN CODCONSIGNMENTDETAIL_NEW CD\n" +
        //                            "    ON CD.CONSIGNMENTNUMBER = C.CONSIGNMENTNUMBER\n";
        //                }

        //                if (rbtn_ChequeCriteria.SelectedValue == "0")
        //                {
        //                    ChequeNo.Style.Add("display", "none");
        //                    paidsts += " and ( b.ChequeNo is NULL OR b.ChequeNo = '')";
        //                }
        //                else
        //                {
        //                    ChequeNo.Style.Add("display", "block");
        //                    if (txt_chequeNo.Text.Trim() == "")
        //                    {
        //                        Alert("Enter Cheque Number", "Red");
        //                        return;
        //                    }
        //                    paidsts += " and b.ChequeNo = '" + txt_chequeNo.Text + "'";
        //                }
        //                #endregion
        //            }


        //            //if (paidstatus.Items.FindByValue("UNPAID").Selected == true)
        //            //{
        //            //    paidsts = "b.PaidStatus = 'UNPAID'";
        //            //}



        //            //clvar.StartDate = date;
        //            clvar.Status = query;
        //            clvar.Seal = paidsts;

        //            if (clvar.StartDate != null && clvar.Status != "" || 1==1)
        //            {
        //                DataSet header = Get_CODPaymentReport(clvar);//------------------------------------
        //                header.Tables[0].Columns.Add("Sr.", typeof(int));
        //                GridView1.DataSource = header;
        //                GridView1.DataBind();
        //                int colcount = GridView1.Rows[0].Cells.Count;


        //                table = new PdfPTable(colcount);
        //                table.TotalWidth = 550f;

        //                table.SetWidths(new float[] { 35f, 75f, 70f, 80f, 70f, 60f, 60f, 80f });
        //                table.LockedWidth = true;

        //                for (int i = 0; i < colcount; i++)
        //                {
        //                    cell = PhraseCell(new Phrase(GridView1.HeaderRow.Cells[i].Text, FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_CENTER);
        //                    cell.Colspan = 1;
        //                    //cell.BorderColor = BaseColor.BLACK;
        //                    //cell.BackgroundColor = BaseColor.LIGHT_GRAY;
        //                    table.AddCell(cell);
        //                }
        //                int sr = 1;
        //                int z = 1;
        //                foreach (GridViewRow row in GridView1.Rows)
        //                {
        //                    cell = PhraseCell(new Phrase(sr.ToString(), FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_CENTER);
        //                    cell.Colspan = 1;
        //                    //cell.BorderColor = BaseColor.BLACK;

        //                    table.AddCell(cell);
        //                    for (int i = 0; i < colcount; i++)
        //                    {
        //                        if (i == 0)
        //                        {

        //                        }
        //                        else
        //                        {
        //                            if (row.Cells[i].Text == "&nbsp;" || row.Cells[i].Text == null || row.Cells[i].Text == "" || String.IsNullOrWhiteSpace(row.Cells[i].Text))
        //                            {
        //                                cell = PhraseCell(new Phrase("", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_CENTER);
        //                            }
        //                            else
        //                            {
        //                                cell = PhraseCell(new Phrase(row.Cells[i].Text, FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_CENTER);

        //                            }

        //                            cell.Colspan = 1;
        //                            //cell.BorderColor = BaseColor.BLACK;

        //                            table.AddCell(cell);
        //                        }
        //                    }
        //                    z++;
        //                    sr++;
        //                    if (c == 1 && z == 73)
        //                    {
        //                        for (int a = 1; a < 17; a++)//z is row number
        //                        {
        //                            string pageNum;
        //                            if (a == 16) //a cell spaces that are left then pagenum will print curently 8cells to give one line gap its 16
        //                            {
        //                                pageNum = "Page Num  " + c.ToString();
        //                            }
        //                            else
        //                            {
        //                                pageNum = "";
        //                            }
        //                            cell = PhraseCell(new Phrase(pageNum, FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.BOLD)), PdfPCell.ALIGN_RIGHT);
        //                            cell.Colspan = 1;
        //                            cell.Padding = 5f;
        //                            table.AddCell(cell);
        //                        }
        //                        //string pageNum;
        //                        //for (int a = 0; a < 5; a++)
        //                        //{
        //                        //    if (a == 4)
        //                        //    {
        //                        //        pageNum = "Page Num  " + c.ToString();
        //                        //    }
        //                        //    else
        //                        //    {
        //                        //        pageNum = "";
        //                        //    }
        //                        //    cell = PhraseCell(new Phrase(pageNum, FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_RIGHT);
        //                        //    cell.Colspan = 1;
        //                        //    table.AddCell(cell);
        //                        //}

        //                        for (int j = 0; j < colcount; j++)
        //                        {
        //                            cell = PhraseCell(new Phrase(GridView1.HeaderRow.Cells[j].Text, FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_CENTER);
        //                            cell.Colspan = 1;
        //                            //cell.BorderColor = BaseColor.BLACK;
        //                            //cell.BackgroundColor = BaseColor.LIGHT_GRAY;
        //                            table.AddCell(cell);

        //                        }
        //                        z = 1;

        //                        c++;
        //                    }

        //                    else if (c > 1 && z == 80)
        //                    {
        //                        string pageNum;

        //                        for (int a = 1; a < 17; a++)//z is row number
        //                        {

        //                            if (a == 16)//a cell spaces that are left then pagenum will print
        //                            {
        //                                pageNum = "Page Num  " + c.ToString();
        //                            }
        //                            else
        //                            {
        //                                pageNum = "";
        //                            }
        //                            cell = PhraseCell(new Phrase(pageNum, FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.BOLD)), PdfPCell.ALIGN_RIGHT);
        //                            cell.Colspan = 1;
        //                            cell.Padding = 5f;
        //                            table.AddCell(cell);
        //                        }

        //                        for (int j = 0; j < colcount; j++)
        //                        {
        //                            cell = PhraseCell(new Phrase(GridView1.HeaderRow.Cells[j].Text, FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_CENTER);
        //                            cell.Colspan = 1;
        //                            //cell.BorderColor = BaseColor.BLACK;
        //                            //cell.BackgroundColor = BaseColor.LIGHT_GRAY;
        //                            table.AddCell(cell);


        //                        }
        //                        //cell = PhraseCell(c, FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_RIGHT);
        //                        // cell.Colspan = 1;
        //                        //cell.BorderColor = BaseColor.BLACK;

        //                        //table.AddCell(cell);

        //                        z = 1;
        //                        c++;
        //                    }
        //                }
        //            }
        //        }

        //        //Footer Table

        //        table_down = new PdfPTable(7);
        //        table_down.TotalWidth = 550f;
        //        table_down.LockedWidth = true;
        //        table_down.SpacingBefore = 30f;
        //        table_down.SetWidths(new float[] { 35f, 10f, 20f, 10f, 5f, 30f, 5f });


        //        cell = PhraseCell(new Phrase("Summary:", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_LEFT);
        //        cell.Colspan = 1;
        //        cell.Padding = 5f;
        //        table_down.AddCell(cell);

        //        cell = PhraseCell(new Phrase("Qty", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_LEFT);
        //        cell.Colspan = 1;
        //        cell.Padding = 5f;
        //        table_down.AddCell(cell);

        //        cell = PhraseCell(new Phrase("Amount", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_LEFT);
        //        cell.Colspan = 1;
        //        cell.Padding = 5f;
        //        table_down.AddCell(cell);

        //        cell = PhraseCell(new Phrase("✔", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_LEFT);
        //        cell.Colspan = 1;
        //        cell.Padding = 5f;
        //        table_down.AddCell(cell);

        //        cell = PhraseCell(new Phrase("", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_LEFT);
        //        cell.Colspan = 1;
        //        cell.Padding = 5f;
        //        cell.UseVariableBorders = true;
        //        //cell.BorderColorTop = BaseColor.BLACK;
        //        //cell.BorderColorLeft = BaseColor.BLACK;
        //        table_down.AddCell(cell);

        //        cell = PhraseCell(new Phrase("____________________", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_CENTER);
        //        cell.Colspan = 1;
        //        cell.Padding = 5f;
        //        cell.UseVariableBorders = true;
        //        table_down.AddCell(cell);

        //        cell = PhraseCell(new Phrase("", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_LEFT);
        //        cell.Colspan = 1;
        //        cell.Padding = 5f;
        //        cell.UseVariableBorders = true;
        //        //cell.BorderColorTop = BaseColor.BLACK;
        //        //cell.BorderColorRight = BaseColor.BLACK;
        //        table_down.AddCell(cell);

        //        cell = PhraseCell(new Phrase("Shipments Booked", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_LEFT);
        //        cell.Colspan = 1;
        //        cell.Padding = 5f;
        //        table_down.AddCell(cell);

        //        cell = PhraseCell(new Phrase(sbqty.ToString(), FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_LEFT);
        //        cell.Colspan = 1;
        //        cell.Padding = 5f;
        //        table_down.AddCell(cell);

        //        cell = PhraseCell(new Phrase(sbamt.ToString(), FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_LEFT);
        //        cell.Colspan = 1;
        //        cell.Padding = 5f;
        //        table_down.AddCell(cell);

        //        cell = PhraseCell(new Phrase("[ ]", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_LEFT);
        //        cell.Colspan = 1;
        //        cell.Padding = 5f;
        //        table_down.AddCell(cell);

        //        cell = PhraseCell(new Phrase("", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_LEFT);
        //        cell.Colspan = 1;
        //        cell.Padding = 5f;
        //        cell.UseVariableBorders = true;
        //        //cell.BorderColorLeft = BaseColor.BLACK;
        //        table_down.AddCell(cell);

        //        cell = PhraseCell(new Phrase("Head Of Operations", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.BOLD)), PdfPCell.ALIGN_CENTER);
        //        cell.Colspan = 1;
        //        cell.Padding = 5f;
        //        table_down.AddCell(cell);

        //        cell = PhraseCell(new Phrase("", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_LEFT);
        //        cell.Colspan = 1;
        //        cell.Padding = 5f;
        //        cell.UseVariableBorders = true;
        //        //cell.BorderColorRight = BaseColor.BLACK;
        //        table_down.AddCell(cell);



        //        cell = PhraseCell(new Phrase("Shipments Delivered", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_LEFT);
        //        cell.Colspan = 1;
        //        cell.Padding = 5;
        //        table_down.AddCell(cell);

        //        cell = PhraseCell(new Phrase(Delivered.ToString(), FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_LEFT);
        //        cell.Colspan = 1;
        //        cell.Padding = 5f;
        //        table_down.AddCell(cell);

        //        cell = PhraseCell(new Phrase(DeliveredAmount.ToString(), FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_LEFT);
        //        cell.Colspan = 1;
        //        cell.Padding = 5f;
        //        table_down.AddCell(cell);


        //        cell = PhraseCell(new Phrase("[ ]", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_LEFT);
        //        cell.Colspan = 1;
        //        cell.Padding = 5f;
        //        table_down.AddCell(cell);

        //        cell = PhraseCell(new Phrase("", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_LEFT);
        //        cell.Colspan = 1;
        //        cell.Padding = 5f;
        //        cell.UseVariableBorders = true;
        //        //cell.BorderColorLeft = BaseColor.BLACK;
        //        table_down.AddCell(cell);


        //        cell = PhraseCell(new Phrase("COD Buisness", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_CENTER);
        //        cell.Colspan = 1;
        //        cell.Padding = 5f;
        //        table_down.AddCell(cell);

        //        //cell = PhraseCell(new Phrase("", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_LEFT);
        //        //cell.Colspan = 1;
        //        //cell.Padding = 5f;
        //        //cell.UseVariableBorders = true;
        //        //cell.BorderColorRight = BaseColor.BLACK;
        //        //table_down.AddCell(cell);
        //        cell = PhraseCell(new Phrase("", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_LEFT);
        //        cell.Colspan = 1;
        //        cell.Padding = 5f;
        //        cell.UseVariableBorders = true;
        //        //cell.BorderColorRight = BaseColor.BLACK;
        //        table_down.AddCell(cell);



        //        /////////////////////



        //        cell = PhraseCell(new Phrase("Dilevered with RR", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_LEFT);
        //        cell.Colspan = 1;
        //        cell.Padding = 5;
        //        table_down.AddCell(cell);


        //        cell = PhraseCell(new Phrase(DeliveredCN.ToString(), FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_LEFT);
        //        cell.Colspan = 1;
        //        cell.Padding = 5f;
        //        table_down.AddCell(cell);

        //        cell = PhraseCell(new Phrase(DeliveredwithRRAmount.ToString(), FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_LEFT);
        //        cell.Colspan = 1;
        //        cell.Padding = 5f;
        //        table_down.AddCell(cell);

        //        cell = PhraseCell(new Phrase("[ ]", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_LEFT);
        //        cell.Colspan = 1;
        //        cell.Padding = 5f;
        //        table_down.AddCell(cell);

        //        cell = PhraseCell(new Phrase("", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_LEFT);
        //        cell.Colspan = 1;
        //        cell.Padding = 5f;
        //        cell.UseVariableBorders = true;
        //        //cell.BorderColorLeft = BaseColor.BLACK;
        //        table_down.AddCell(cell);

        //        cell = PhraseCell(new Phrase("", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_CENTER);
        //        cell.Colspan = 1;
        //        cell.Padding = 5f;
        //        table_down.AddCell(cell);

        //        cell = PhraseCell(new Phrase("", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_LEFT);
        //        cell.Colspan = 7;
        //        cell.Padding = 5f;
        //        cell.UseVariableBorders = true;
        //        //cell.BorderColorRight = BaseColor.BLACK;
        //        table_down.AddCell(cell);

        //        //cell = PhraseCell(new Phrase("", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_LEFT);
        //        //cell.Colspan = 1;
        //        //cell.Padding = 5f;
        //        //cell.UseVariableBorders = true;
        //        //cell.BorderColorRight = BaseColor.BLACK;
        //        //table_down.AddCell(cell);

        //        cell = PhraseCell(new Phrase("Delivered without RR", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_LEFT);
        //        cell.Colspan = 1;
        //        cell.Padding = 5;
        //        table_down.AddCell(cell);

        //        cell = PhraseCell(new Phrase(DeliveredCN2.ToString(), FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_LEFT);
        //        cell.Colspan = 1;
        //        cell.Padding = 5f;
        //        table_down.AddCell(cell);

        //        cell = PhraseCell(new Phrase(DeliveredwithoutRRAmount.ToString(), FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_LEFT);
        //        cell.Colspan = 1;
        //        cell.Padding = 5f;
        //        table_down.AddCell(cell);


        //        cell = PhraseCell(new Phrase("[ ]", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_LEFT);
        //        cell.Colspan = 1;
        //        cell.Padding = 5f;
        //        table_down.AddCell(cell);

        //        cell = PhraseCell(new Phrase("", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_LEFT);
        //        cell.Colspan = 1;
        //        cell.Padding = 5f;
        //        cell.UseVariableBorders = true;
        //        //cell.BorderColorLeft = BaseColor.BLACK;
        //        table_down.AddCell(cell);

        //        cell = PhraseCell(new Phrase("____________________", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_CENTER);
        //        cell.Colspan = 1;
        //        cell.Padding = 5f;
        //        table_down.AddCell(cell);

        //        cell = PhraseCell(new Phrase("", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_LEFT);
        //        cell.Colspan = 1;
        //        cell.Padding = 5f;
        //        cell.UseVariableBorders = true;
        //        //cell.BorderColorRight = BaseColor.BLACK;
        //        table_down.AddCell(cell);


        //        cell = PhraseCell(new Phrase("Total Payable(with & without RR)", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_LEFT);
        //        cell.Colspan = 1;
        //        cell.Padding = 5f;
        //        table_down.AddCell(cell);

        //        cell = PhraseCell(new Phrase("", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_LEFT);
        //        cell.Colspan = 1;
        //        cell.Padding = 5f;
        //        table_down.AddCell(cell);

        //        cell = PhraseCell(new Phrase(totalwrr.ToString(), FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_LEFT);
        //        cell.Colspan = 1;
        //        cell.Padding = 5f;
        //        table_down.AddCell(cell);

        //        cell = PhraseCell(new Phrase("", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_LEFT);
        //        cell.Colspan = 1;
        //        cell.Padding = 5f;
        //        table_down.AddCell(cell);

        //        cell = PhraseCell(new Phrase("", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_LEFT);
        //        cell.Colspan = 1;
        //        cell.Padding = 5f;
        //        cell.UseVariableBorders = true;
        //       // cell.BorderColorBottom = BaseColor.BLACK;
        //        //cell.BorderColorLeft = BaseColor.BLACK;
        //        table_down.AddCell(cell);

        //        cell = PhraseCell(new Phrase("Director 3PL & Special Projects", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.BOLD)), PdfPCell.ALIGN_CENTER);
        //        cell.Colspan = 1;
        //        cell.Padding = 5f;
        //        table_down.AddCell(cell);

        //        cell = PhraseCell(new Phrase("", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_LEFT);
        //        cell.Colspan = 1;
        //        cell.Padding = 5f;
        //       // cell.BorderColorBottom = BaseColor.BLACK;
        //        cell.UseVariableBorders = true;
        //       // cell.BorderColorRight = BaseColor.BLACK;
        //        table_down.AddCell(cell);


        //        table_pageNum = new PdfPTable(2);
        //        table_pageNum.TotalWidth = 550f;
        //        table_pageNum.LockedWidth = true;
        //        table_pageNum.SpacingBefore = 30f;
        //        table_pageNum.SetWidths(new float[] { 35f, 40f });


        //        for (int a = 1; a < 5; a++)
        //        {
        //            string pageNum;
        //            if (a == 4)
        //            {
        //                pageNum = "Page Num  " + c.ToString();
        //            }
        //            else
        //            {
        //                pageNum = "";
        //            }
        //            cell = PhraseCell(new Phrase(pageNum, FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.BOLD)), PdfPCell.ALIGN_RIGHT);
        //            cell.Colspan = 1;
        //            cell.Padding = 5f;
        //            table_pageNum.AddCell(cell);
        //        }




        //        document.Add(table_up);
        //        document.Add(table);
        //        document.Add(table_down);
        //        document.Add(table_pageNum);


        //        document.Close();
        //        byte[] bytes = memoryStream.ToArray();
        //        memoryStream.Close();
        //        Response.Clear();
        //        Response.ContentType = "application/pdf";
        //        string fileName = "ExportToPdf_" + DateTime.Now.ToShortDateString();
        //        Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName + ".pdf");
        //        Response.ContentType = "application/pdf";

        //        Response.Buffer = true;
        //        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //        Response.BinaryWrite(bytes);
        //        Response.End();
        //        Response.Close();




        //        ////using (MemoryStream ms = new MemoryStream())
        //        ////using (Document document = new Document(PageSize.A4, 25, 25, 30, 30))
        //        //using (PdfWriter writer = PdfWriter.GetInstance(document, ms))
        //        //{
        //        //    document.Open();
        //        //    document.Add(new Paragraph("Hello World"));
        //        //    document.Close();
        //        //    writer.Close();
        //        //    ms.Close();
        //        //    Response.ContentType = "pdf/application";
        //        //    Response.AddHeader("content-disposition", "attachment;filename=First_PDF_document.pdf");
        //        //    Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
        //        //}
        //    }
        //} 


        //private static PdfPCell PhraseCell(Phrase phrase, int align)
        //{
        //    PdfPCell cell = new PdfPCell(phrase);
        //    //cell.BorderColor = iTextSharp.text.BaseColor.WHITE;
        //    cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
        //    cell.HorizontalAlignment = align;
        //    cell.PaddingBottom = 2f;
        //    cell.PaddingTop = 0f;
        //    return cell;
        //}

        //protected void ExportToPDF(object sender, ImageClickEventArgs e)
        //{
        //    if (type.SelectedValue == "html")
        //    {
        //        clvar = new Variable();
        //        string[] split_number = dd_customer.SelectedValue.Split('_');
        //        clvar.ACNumber = split_number[0];
        //        if (paidstatus.Items.FindByValue("PAID").Selected == true)
        //        {
        //            #region Marking Paid
        //            cnNumber.Style.Add("display", "none");
        //            ChequeCriteria.Style.Add("display", "none");
        //            ChequeNo.Style.Add("display", "none");
        //            paidsts = "b.PaidStatus = 'UNPAID'";

        //            if (split_number[1] == "OLD")
        //            {
        //                query = " INNER JOIN CODCONSIGNMENTDETAIL CD\n" +
        //                        "    ON CD.CONSIGNMENTNUMBER = C.CONSIGNMENTNUMBER\n";
        //            }

        //            if (split_number[1] == "NEW")
        //            {
        //                query = " INNER JOIN CODCONSIGNMENTDETAIL_NEW CD\n" +
        //                        "    ON CD.CONSIGNMENTNUMBER = C.CONSIGNMENTNUMBER\n";
        //            }
        //            #endregion
        //        }
        //        else if (paidstatus.Items.FindByValue("UNPAID").Selected == true)
        //        {
        //            #region Marking Unpaid and Editing
        //            cnNumber.Style.Add("display", "block");
        //            ChequeCriteria.Style.Add("display", "none");
        //            ChequeNo.Style.Add("display", "block");
        //            if (txt_cnNumber.Text.Trim() == "")
        //            {
        //                Alert("Enter Consignment Number", "Red");
        //                return;
        //            }

        //            paidsts = "b.PaidStatus = 'PAID'";

        //            if (split_number[1] == "OLD")
        //            {
        //                query = " INNER JOIN CODCONSIGNMENTDETAIL CD\n" +
        //                        "    ON CD.CONSIGNMENTNUMBER = C.CONSIGNMENTNUMBER\n";
        //            }

        //            if (split_number[1] == "NEW")
        //            {
        //                query = " INNER JOIN CODCONSIGNMENTDETAIL_NEW CD\n" +
        //                        "    ON CD.CONSIGNMENTNUMBER = C.CONSIGNMENTNUMBER\n";
        //            }
        //            paidsts += " AND b.ConsignmentNumber = '" + txt_cnNumber.Text + "'";
        //            #endregion
        //        }
        //        else if (paidstatus.Items.FindByValue("PRINT").Selected == true)
        //        {
        //            #region Printing
        //            paidsts = "b.PaidStatus = 'PAID'";
        //            cnNumber.Style.Add("display", "none");
        //            ChequeCriteria.Style.Add("display", "block");

        //            if (split_number[1] == "OLD")
        //            {
        //                query = " INNER JOIN CODCONSIGNMENTDETAIL CD\n" +
        //                        "    ON CD.CONSIGNMENTNUMBER = C.CONSIGNMENTNUMBER\n";
        //            }

        //            if (split_number[1] == "NEW")
        //            {
        //                query = " INNER JOIN CODCONSIGNMENTDETAIL_NEW CD\n" +
        //                        "    ON CD.CONSIGNMENTNUMBER = C.CONSIGNMENTNUMBER\n";
        //            }

        //            if (rbtn_ChequeCriteria.SelectedValue == "0")
        //            {
        //                ChequeNo.Style.Add("display", "none");
        //                paidsts += " and ( b.ChequeNo is NULL OR b.ChequeNo = '')";
        //            }
        //            else
        //            {
        //                ChequeNo.Style.Add("display", "block");
        //                if (txt_chequeNo.Text.Trim() == "")
        //                {
        //                    Alert("Enter Cheque Number", "Red");
        //                    return;
        //                }
        //                paidsts += " and b.ChequeNo = '" + txt_chequeNo.Text + "'";
        //            }
        //            #endregion
        //        }


        //        //if (paidstatus.Items.FindByValue("UNPAID").Selected == true)
        //        //{
        //        //    paidsts = "b.PaidStatus = 'UNPAID'";
        //        //}



        //        clvar.StartDate = date;
        //        clvar.Status = query;
        //        clvar.Seal = paidsts;

        //        if (clvar.StartDate != null && clvar.Status != "" || 1 == 1)
        //        {
        //            DataSet header = Get_CODPaymentReport(clvar);

        //            if (header.Tables[0].Rows.Count != 0)
        //            {
        //                error_msg.Text = "";
        //                #region MyRegion


        //                Literal lt_Main = new Literal();
        //                lt_Main.Text += "</b";
        //                lt_Main.Text += "</tr></table>";
        //                lt_Main.Text += "<table class='header'>";
        //                lt_Main.Text += "<tr>"; //phela table ha jisma se variabble uthane ha or grid ke sab se uper aien
        //                lt_Main.Text += "<td width=\"14%\"><b> Orign:</b></td>";
        //                lt_Main.Text += "<td width=\"36%\">" + header.Tables[0].Rows[0]["ORGZONE"].ToString() + "</td>";
        //                lt_Main.Text += "<td width=\"14%\"><b>Date</b></font></td>";
        //                lt_Main.Text += "<td width=\"36%\">" + header.Tables[0].Rows[0]["BOOKING DATE"].ToString() + "</td>";

        //                //Store your database DateTime value into a variable
        //                //   DateTime dt = DateTime.ParseExact(dd_start_date.Text, "yyyyMMdd", null);
        //                //   DateTime txtMyDate = DateTime.Parse(dd_start_date.Text);


        //                //   DateTime dateValue = DateTime.ParseExact(dd_start_date.Text, "M/d/yyyy", CultureInfo.InvariantCulture);


        //                //    lt_Main.Text += "<td width=\"36%\">" + txtMyDate + " TO " + dd_end_date.Text + "</td>";
        //                lt_Main.Text += "</tr>";

        //                lt_Main.Text += "<tr>";

        //                lt_Main.Text += "<td width=\"14%\"><b> Customer:</b></td>";//phela table ha jisma se variabble uthane ha or grid ke sab se uper aien total 4 varaible hain
        //                lt_Main.Text += "<td width=\"36%\">" + header.Tables[0].Rows[0]["CUSTOMERNAME"].ToString() + "</td>";
        //                lt_Main.Text += "<td width=\"14%\"><b>Account</b></font></td>";
        //                lt_Main.Text += "<td width=\"36%\">" + header.Tables[0].Rows[0]["ACCOUNTNO"].ToString() + "</td>";
        //                lt_Main.Text += "</tr>";


        //                myHolder1.Dispose();
        //                myHolder1.Controls.Clear();
        //                myHolder1.Controls.Add(lt_Main);
        //                lt_Main.Text += "</table>";


        //                Literal lt_Main1 = new Literal();
        //                lt_Main1.Text += "<table class='detail' cellspacing='0'>";
        //                lt_Main1.Text += "<tr>";
        //                lt_Main1.Text += "<th width=\"3%\"><b> S.NO</b></th>";
        //                lt_Main1.Text += "<th width=\"14%\"><b> CONSIGNMENT NUMBER</b></th>";
        //                lt_Main1.Text += "<th width=\"14%\"><b>BOOKING DATE</b></font></th>";
        //                lt_Main1.Text += "<th width=\"14%\"><b>STATUS</b></font></th>";
        //                lt_Main1.Text += "<th width=\"14%\"><b>RR NUMBER</b></font></th>";
        //                lt_Main1.Text += "<th width=\"8%\"><b>COD AMOUNT</b></font></th>";
        //                lt_Main1.Text += "</tr>";

        //                for (int j = 0; j < header.Tables[0].Rows.Count; j++)
        //                {
        //                    lt_Main1.Text += "<tr>";
        //                    lt_Main1.Text += "<td>" + (j + 1).ToString() + "</td>";
        //                    lt_Main1.Text += "<td>" + header.Tables[0].Rows[j]["CONSIGNMENT NUMBER"].ToString() + "</td>";
        //                    lt_Main1.Text += "<td>" + header.Tables[0].Rows[j]["BOOKING DATE"].ToString() + "</td>";
        //                    lt_Main1.Text += "<td>" + header.Tables[0].Rows[j]["RR STATUS"].ToString() + "</td>";
        //                    lt_Main1.Text += "<td>" + header.Tables[0].Rows[j]["Payment Voucher ID"].ToString() + "</td>";
        //                    lt_Main1.Text += "<td>" + string.Format("{0:N0}", header.Tables[0].Rows[j]["COD AMOUNT"].ToString()) + "</td>";

        //                    CODTotalAmount += double.Parse(header.Tables[0].Rows[j]["COD AMOUNT"].ToString());
        //                    //total payable ammount
        //                    int i = 1;
        //                    int x = 1;
        //                    int a = 1;
        //                    //  if (header.Tables[0].Rows[j]["RR STATUS"].ToString() == "DELIVERED")
        //                    if (header.Tables[0].Rows[j]["RR STATUS"].ToString() == "D-DELIVERED")
        //                    {
        //                        Delivered += i++;
        //                        DeliveredAmount += double.Parse(header.Tables[0].Rows[j]["COD AMOUNT"].ToString());
        //                        //shipment diliver

        //                        if (header.Tables[0].Rows[j]["DeliveredCN"].ToString() == "1")
        //                        {
        //                            DeliveredCN += x++;
        //                            DeliveredwithRRAmount += double.Parse(header.Tables[0].Rows[j]["COD AMOUNT"].ToString());
        //                        }

        //                        if (header.Tables[0].Rows[j]["DeliveredCN"].ToString() == "0")
        //                        {
        //                            DeliveredCN2 += x++;
        //                            DeliveredwithoutRRAmount += double.Parse(header.Tables[0].Rows[j]["COD AMOUNT"].ToString());
        //                        }
        //                    }

        //                    lt_Main1.Text += "</tr>";
        //                }

        //                myHolder2.Dispose();
        //                myHolder2.Controls.Clear();
        //                myHolder2.Controls.Add(lt_Main1);
        //                lt_Main1.Text += "</table>";



        //                Literal lt_Main2 = new Literal();
        //                lt_Main2.Text += "<table class='summary'>";
        //                lt_Main2.Text += "<tr>";
        //                lt_Main2.Text += "<td width=\"20%\"><b>SUMMARY</b></td>";
        //                lt_Main2.Text += "<td width=\"5%\"><b>QTY</b></td>";
        //                lt_Main2.Text += "<td width=\"8%\"><b>AMOUNT</b></font></td>";
        //                lt_Main2.Text += "</tr>";
        //                //y
        //                lt_Main2.Text += "<tr>";
        //                lt_Main2.Text += "<td>Shipment Booked </td>";
        //                lt_Main2.Text += "<td>" + header.Tables[0].Rows.Count + "</td>"; //shipment booked
        //                lt_Main2.Text += "<td>" + string.Format("{0:N0}", CODTotalAmount) + "</td>";
        //                lt_Main2.Text += "</tr>";

        //                lt_Main2.Text += "<tr>";
        //                lt_Main2.Text += "<td>Shipment Delivered </td>";
        //                lt_Main2.Text += "<td>" + Delivered + "</td>";
        //                lt_Main2.Text += "<td>" + string.Format("{0:N0}", DeliveredAmount) + "</td>";
        //                lt_Main2.Text += "</tr>";

        //                lt_Main2.Text += "<tr>";
        //                lt_Main2.Text += "<td style='padding: 0px 0px 0px 25px;'>Delivered with RR </td>";
        //                lt_Main2.Text += "<td>" + DeliveredCN + "</td>"; //diliver rr
        //                lt_Main2.Text += "<td>" + string.Format("{0:N0}", DeliveredwithRRAmount) + "</td>";
        //                lt_Main2.Text += "</tr>";

        //                lt_Main2.Text += "<tr>";
        //                lt_Main2.Text += "<td style='padding: 0px 0px 0px 25px;'>Delivered without RR </td>"; //
        //                lt_Main2.Text += "<td>" + DeliveredCN2 + "</td>"; //diliver withOutrr
        //                lt_Main2.Text += "<td>" + string.Format("{0:N0}", DeliveredwithoutRRAmount) + "</td>";
        //                lt_Main2.Text += "</tr>";

        //                lt_Main2.Text += "<tr>";
        //                lt_Main2.Text += "<td><b>Total Payable (with & without RR) </b></td>";
        //                lt_Main2.Text += "<td></td>";
        //                lt_Main2.Text += "<td><b>" + string.Format("{0:N0}", DeliveredAmount) + "</b></td>";
        //                lt_Main2.Text += "</tr>";

        //                myHolder3.Dispose();
        //                myHolder3.Controls.Clear();
        //                myHolder3.Controls.Add(lt_Main2);
        //                lt_Main2.Text += "</table>";




        //                // ye last ma 2 lines chaye hain          
        //                Literal lt_Main3 = new Literal();
        //                lt_Main3.Text += "<table class='note'>";
        //                lt_Main3.Text += "<tr>";
        //                lt_Main3.Text += "<td width=\"20%\">*Approval of Director 3PL will be required for payment of those shipments whose RR not available.</td>";
        //                lt_Main3.Text += "</tr>";
        //                lt_Main3.Text += "<tr>";
        //                lt_Main3.Text += "<td width=\"5%\">**No service charges while payment will be deducted, rather monthly invoice to Customer will be submitted for payment as per routine business.</td>";
        //                lt_Main3.Text += "</tr>";

        //                myHolder4.Dispose();
        //                myHolder4.Controls.Clear();
        //                myHolder4.Controls.Add(lt_Main3);
        //                lt_Main3.Text += "</table>";
        //                //ye necha signature wala part
        //                Literal lt_Main4 = new Literal();
        //                lt_Main4.Text += "<table class='signature'>";
        //                lt_Main4.Text += "<tr>";
        //                lt_Main4.Text += "<td width=\"20%\" style='position: relative; border-top: 3px solid rgb(0, 0, 0);'><b>Head of Operations</b></td>";
        //                lt_Main4.Text += "</tr>";
        //                lt_Main4.Text += "<tr>";
        //                lt_Main4.Text += "<td width=\"5%\">COD Business</td>";
        //                lt_Main4.Text += "</tr>";
        //                lt_Main4.Text += "<tr><td style='height: 42px;'></td></tr>";
        //                lt_Main4.Text += "<tr><td style='border-top: 3px solid rgb(0, 0, 0);'><b>Director 3PL & Special Projects</b></td></tr>";

        //                myHolder5.Dispose();
        //                myHolder5.Controls.Clear();
        //                myHolder5.Controls.Add(lt_Main4);
        //                lt_Main4.Text += "</table>";



        //                //?????????????????? kia hua bhai is buttonh pe aie or bechma ye table aie phir ek phela uper phir table phir nechay wala
        //                ExporttopdfSummary(header.Tables[0].Rows[0]["ORGZONE"].ToString(), header.Tables[0].Rows[0]["BOOKING DATE"].ToString(), header.Tables[0].Rows[0]["ACCOUNTNO"].ToString(), header.Tables[0].Rows[0]["CUSTOMERNAME"].ToString(), header.Tables[0].Rows.Count, CODTotalAmount, Delivered, DeliveredAmount, DeliveredCN, DeliveredwithRRAmount, DeliveredCN2, DeliveredwithoutRRAmount, DeliveredAmount);

        //                #endregion
        //            }
        //            else
        //            {
        //                error_msg.Text = "No Record Found...";
        //            }
        //        }
        //        else
        //        {
        //            error_msg.Text = "Select Filter";
        //        }



        //    }







        //}
        //public void ExportToPDF()
        //{
        //    if (type.SelectedValue == "html")
        //    {
        //        clvar = new Variable();
        //        string[] split_number = dd_customer.SelectedValue.Split('_');
        //        clvar.ACNumber = split_number[0];
        //        if (paidstatus.Items.FindByValue("PAID").Selected == true)
        //        {
        //            #region Marking Paid
        //            cnNumber.Style.Add("display", "none");
        //            ChequeCriteria.Style.Add("display", "none");
        //            ChequeNo.Style.Add("display", "none");
        //            paidsts = "b.PaidStatus = 'UNPAID'";

        //            if (split_number[1] == "OLD")
        //            {
        //                query = " INNER JOIN CODCONSIGNMENTDETAIL CD\n" +
        //                        "    ON CD.CONSIGNMENTNUMBER = C.CONSIGNMENTNUMBER\n";
        //            }

        //            if (split_number[1] == "NEW")
        //            {
        //                query = " INNER JOIN CODCONSIGNMENTDETAIL_NEW CD\n" +
        //                        "    ON CD.CONSIGNMENTNUMBER = C.CONSIGNMENTNUMBER\n";
        //            }
        //            #endregion
        //        }
        //        else if (paidstatus.Items.FindByValue("UNPAID").Selected == true)
        //        {
        //            #region Marking Unpaid and Editing
        //            cnNumber.Style.Add("display", "block");
        //            ChequeCriteria.Style.Add("display", "none");
        //            ChequeNo.Style.Add("display", "block");
        //            if (txt_cnNumber.Text.Trim() == "")
        //            {
        //                Alert("Enter Consignment Number", "Red");
        //                return;
        //            }

        //            paidsts = "b.PaidStatus = 'PAID'";

        //            if (split_number[1] == "OLD")
        //            {
        //                query = " INNER JOIN CODCONSIGNMENTDETAIL CD\n" +
        //                        "    ON CD.CONSIGNMENTNUMBER = C.CONSIGNMENTNUMBER\n";
        //            }

        //            if (split_number[1] == "NEW")
        //            {
        //                query = " INNER JOIN CODCONSIGNMENTDETAIL_NEW CD\n" +
        //                        "    ON CD.CONSIGNMENTNUMBER = C.CONSIGNMENTNUMBER\n";
        //            }
        //            paidsts += " AND b.ConsignmentNumber = '" + txt_cnNumber.Text + "'";
        //            #endregion
        //        }
        //        else if (paidstatus.Items.FindByValue("PRINT").Selected == true)
        //        {
        //            #region Printing
        //            paidsts = "b.PaidStatus = 'PAID'";
        //            cnNumber.Style.Add("display", "none");
        //            ChequeCriteria.Style.Add("display", "block");

        //            if (split_number[1] == "OLD")
        //            {
        //                query = " INNER JOIN CODCONSIGNMENTDETAIL CD\n" +
        //                        "    ON CD.CONSIGNMENTNUMBER = C.CONSIGNMENTNUMBER\n";
        //            }

        //            if (split_number[1] == "NEW")
        //            {
        //                query = " INNER JOIN CODCONSIGNMENTDETAIL_NEW CD\n" +
        //                        "    ON CD.CONSIGNMENTNUMBER = C.CONSIGNMENTNUMBER\n";
        //            }

        //            if (rbtn_ChequeCriteria.SelectedValue == "0")
        //            {
        //                ChequeNo.Style.Add("display", "none");
        //                paidsts += " and ( b.ChequeNo is NULL OR b.ChequeNo = '')";
        //            }
        //            else
        //            {
        //                ChequeNo.Style.Add("display", "block");
        //                if (txt_chequeNo.Text.Trim() == "")
        //                {
        //                    Alert("Enter Cheque Number", "Red");
        //                    return;
        //                }
        //                paidsts += " and b.ChequeNo = '" + txt_chequeNo.Text + "'";
        //            }
        //            #endregion
        //        }


        //        //if (paidstatus.Items.FindByValue("UNPAID").Selected == true)
        //        //{
        //        //    paidsts = "b.PaidStatus = 'UNPAID'";
        //        //}



        //        clvar.StartDate = date;
        //        clvar.Status = query;
        //        clvar.Seal = paidsts;

        //        if (clvar.StartDate != null && clvar.Status != "" || 1 == 1)
        //        {
        //            DataSet header = Get_CODPaymentReport(clvar);

        //            if (header.Tables[0].Rows.Count != 0)
        //            {
        //                error_msg.Text = "";
        //                #region MyRegion


        //                Literal lt_Main = new Literal();
        //                lt_Main.Text += "</b";
        //                lt_Main.Text += "</tr></table>";
        //                lt_Main.Text += "<table class='header'>";
        //                lt_Main.Text += "<tr>"; //phela table ha jisma se variabble uthane ha or grid ke sab se uper aien
        //                lt_Main.Text += "<td width=\"14%\"><b> Orign:</b></td>";
        //                lt_Main.Text += "<td width=\"36%\">" + header.Tables[0].Rows[0]["ORGZONE"].ToString() + "</td>";
        //                lt_Main.Text += "<td width=\"14%\"><b>Date</b></font></td>";
        //                lt_Main.Text += "<td width=\"36%\">" + header.Tables[0].Rows[0]["BOOKINGDATE"].ToString() + "</td>";

        //                //Store your database DateTime value into a variable
        //                //   DateTime dt = DateTime.ParseExact(dd_start_date.Text, "yyyyMMdd", null);
        //                //   DateTime txtMyDate = DateTime.Parse(dd_start_date.Text);


        //                //   DateTime dateValue = DateTime.ParseExact(dd_start_date.Text, "M/d/yyyy", CultureInfo.InvariantCulture);


        //                //    lt_Main.Text += "<td width=\"36%\">" + txtMyDate + " TO " + dd_end_date.Text + "</td>";
        //                lt_Main.Text += "</tr>";

        //                lt_Main.Text += "<tr>";

        //                lt_Main.Text += "<td width=\"14%\"><b> Customer:</b></td>";//phela table ha jisma se variabble uthane ha or grid ke sab se uper aien total 4 varaible hain
        //                lt_Main.Text += "<td width=\"36%\">" + header.Tables[0].Rows[0]["CUSTOMERNAME"].ToString() + "</td>";
        //                lt_Main.Text += "<td width=\"14%\"><b>Account</b></font></td>";
        //                lt_Main.Text += "<td width=\"36%\">" + header.Tables[0].Rows[0]["ACCOUNTNO"].ToString() + "</td>";
        //                lt_Main.Text += "</tr>";


        //                myHolder1.Dispose();
        //                myHolder1.Controls.Clear();
        //                myHolder1.Controls.Add(lt_Main);
        //                lt_Main.Text += "</table>";


        //                Literal lt_Main1 = new Literal();
        //                lt_Main1.Text += "<table class='detail' cellspacing='0'>";
        //                lt_Main1.Text += "<tr>";
        //                lt_Main1.Text += "<th width=\"3%\"><b> S.NO</b></th>";
        //                lt_Main1.Text += "<th width=\"14%\"><b> CONSIGNMENT NUMBER</b></th>";
        //                lt_Main1.Text += "<th width=\"14%\"><b>BOOKING DATE</b></font></th>";
        //                lt_Main1.Text += "<th width=\"14%\"><b>STATUS</b></font></th>";
        //                lt_Main1.Text += "<th width=\"14%\"><b>RR NUMBER</b></font></th>";
        //                lt_Main1.Text += "<th width=\"8%\"><b>COD AMOUNT</b></font></th>";
        //                lt_Main1.Text += "</tr>";

        //                for (int j = 0; j < header.Tables[0].Rows.Count; j++)
        //                {
        //                    lt_Main1.Text += "<tr>";
        //                    lt_Main1.Text += "<td>" + (j + 1).ToString() + "</td>";
        //                    lt_Main1.Text += "<td>" + header.Tables[0].Rows[j]["CONSIGNMENTNUMBER"].ToString() + "</td>";
        //                    lt_Main1.Text += "<td>" + header.Tables[0].Rows[j]["BOOKINGDATE"].ToString() + "</td>";
        //                    lt_Main1.Text += "<td>" + header.Tables[0].Rows[j]["RRSTATUS"].ToString() + "</td>";
        //                    lt_Main1.Text += "<td>" + header.Tables[0].Rows[j]["PaymentVoucherID"].ToString() + "</td>";
        //                    lt_Main1.Text += "<td>" + string.Format("{0:N0}", header.Tables[0].Rows[j]["CODAMOUNT"].ToString()) + "</td>";

        //                    CODTotalAmount += double.Parse(header.Tables[0].Rows[j]["CODAMOUNT"].ToString());
        //                    //total payable ammount
        //                    int i = 1;
        //                    int x = 1;
        //                    int a = 1;
        //                    //  if (header.Tables[0].Rows[j]["RR STATUS"].ToString() == "DELIVERED")
        //                    if (header.Tables[0].Rows[j]["RRSTATUS"].ToString() == "D-DELIVERED")
        //                    {
        //                        Delivered += i++;
        //                        DeliveredAmount += double.Parse(header.Tables[0].Rows[j]["CODAMOUNT"].ToString());
        //                        //shipment diliver

        //                        if (header.Tables[0].Rows[j]["DeliveredCN"].ToString() == "1")
        //                        {
        //                            DeliveredCN += x++;
        //                            DeliveredwithRRAmount += double.Parse(header.Tables[0].Rows[j]["CODAMOUNT"].ToString());
        //                        }

        //                        if (header.Tables[0].Rows[j]["DeliveredCN"].ToString() == "0")
        //                        {
        //                            DeliveredCN2 += x++;
        //                            DeliveredwithoutRRAmount += double.Parse(header.Tables[0].Rows[j]["CODAMOUNT"].ToString());
        //                        }
        //                    }

        //                    lt_Main1.Text += "</tr>";
        //                }

        //                myHolder2.Dispose();
        //                myHolder2.Controls.Clear();
        //                myHolder2.Controls.Add(lt_Main1);
        //                lt_Main1.Text += "</table>";



        //                Literal lt_Main2 = new Literal();
        //                lt_Main2.Text += "<table class='summary'>";
        //                lt_Main2.Text += "<tr>";
        //                lt_Main2.Text += "<td width=\"20%\"><b>SUMMARY</b></td>";
        //                lt_Main2.Text += "<td width=\"5%\"><b>QTY</b></td>";
        //                lt_Main2.Text += "<td width=\"8%\"><b>AMOUNT</b></font></td>";
        //                lt_Main2.Text += "</tr>";
        //                //y
        //                lt_Main2.Text += "<tr>";
        //                lt_Main2.Text += "<td>Shipment Booked </td>";
        //                lt_Main2.Text += "<td>" + header.Tables[0].Rows.Count + "</td>"; //shipment booked
        //                lt_Main2.Text += "<td>" + string.Format("{0:N0}", CODTotalAmount) + "</td>";
        //                lt_Main2.Text += "</tr>";

        //                lt_Main2.Text += "<tr>";
        //                lt_Main2.Text += "<td>Shipment Delivered </td>";
        //                lt_Main2.Text += "<td>" + Delivered + "</td>";
        //                lt_Main2.Text += "<td>" + string.Format("{0:N0}", DeliveredAmount) + "</td>";
        //                lt_Main2.Text += "</tr>";

        //                lt_Main2.Text += "<tr>";
        //                lt_Main2.Text += "<td style='padding: 0px 0px 0px 25px;'>Delivered with RR </td>";
        //                lt_Main2.Text += "<td>" + DeliveredCN + "</td>"; //diliver rr
        //                lt_Main2.Text += "<td>" + string.Format("{0:N0}", DeliveredwithRRAmount) + "</td>";
        //                lt_Main2.Text += "</tr>";

        //                lt_Main2.Text += "<tr>";
        //                lt_Main2.Text += "<td style='padding: 0px 0px 0px 25px;'>Delivered without RR </td>"; //
        //                lt_Main2.Text += "<td>" + DeliveredCN2 + "</td>"; //diliver withOutrr
        //                lt_Main2.Text += "<td>" + string.Format("{0:N0}", DeliveredwithoutRRAmount) + "</td>";
        //                lt_Main2.Text += "</tr>";

        //                lt_Main2.Text += "<tr>";
        //                lt_Main2.Text += "<td><b>Total Payable (with & without RR) </b></td>";
        //                lt_Main2.Text += "<td></td>";
        //                lt_Main2.Text += "<td><b>" + string.Format("{0:N0}", DeliveredAmount) + "</b></td>";
        //                lt_Main2.Text += "</tr>";

        //                myHolder3.Dispose();
        //                myHolder3.Controls.Clear();
        //                myHolder3.Controls.Add(lt_Main2);
        //                lt_Main2.Text += "</table>";




        //                // ye last ma 2 lines chaye hain          
        //                Literal lt_Main3 = new Literal();
        //                lt_Main3.Text += "<table class='note'>";
        //                lt_Main3.Text += "<tr>";
        //                lt_Main3.Text += "<td width=\"20%\">*Approval of Director 3PL will be required for payment of those shipments whose RR not available.</td>";
        //                lt_Main3.Text += "</tr>";
        //                lt_Main3.Text += "<tr>";
        //                lt_Main3.Text += "<td width=\"5%\">**No service charges while payment will be deducted, rather monthly invoice to Customer will be submitted for payment as per routine business.</td>";
        //                lt_Main3.Text += "</tr>";

        //                myHolder4.Dispose();
        //                myHolder4.Controls.Clear();
        //                myHolder4.Controls.Add(lt_Main3);
        //                lt_Main3.Text += "</table>";
        //                //ye necha signature wala part
        //                Literal lt_Main4 = new Literal();
        //                lt_Main4.Text += "<table class='signature'>";
        //                lt_Main4.Text += "<tr>";
        //                lt_Main4.Text += "<td width=\"20%\" style='position: relative; border-top: 3px solid rgb(0, 0, 0);'><b>Head of Operations</b></td>";
        //                lt_Main4.Text += "</tr>";
        //                lt_Main4.Text += "<tr>";
        //                lt_Main4.Text += "<td width=\"5%\">COD Business</td>";
        //                lt_Main4.Text += "</tr>";
        //                lt_Main4.Text += "<tr><td style='height: 42px;'></td></tr>";
        //                lt_Main4.Text += "<tr><td style='border-top: 3px solid rgb(0, 0, 0);'><b>Director 3PL & Special Projects</b></td></tr>";

        //                myHolder5.Dispose();
        //                myHolder5.Controls.Clear();
        //                myHolder5.Controls.Add(lt_Main4);
        //                lt_Main4.Text += "</table>";



        //                //?????????????????? kia hua bhai is buttonh pe aie or bechma ye table aie phir ek phela uper phir table phir nechay wala
        //                ExporttopdfSummary(header.Tables[0].Rows[0]["ORGZONE"].ToString(), header.Tables[0].Rows[0]["BOOKINGDATE"].ToString(), header.Tables[0].Rows[0]["ACCOUNTNO"].ToString(), header.Tables[0].Rows[0]["CUSTOMERNAME"].ToString(), header.Tables[0].Rows.Count, CODTotalAmount, Delivered, DeliveredAmount, DeliveredCN, DeliveredwithRRAmount, DeliveredCN2, DeliveredwithoutRRAmount, DeliveredAmount);

        //                #endregion
        //            }
        //            else
        //            {
        //                GridView2.DataSource = null;
        //                GridView2.DataBind();
        //                error_msg.Text = "No Record Found...";
        //            }
        //        }
        //        else
        //        {
        //            error_msg.Text = "Select Filter";
        //        }



        //    }
        //}
        #endregion

        public override void VerifyRenderingInServerForm(Control control)
        {

        }

        /*
         protected void ExportToPDF(object sender, EventArgs e)
         {
            Response.Clear();
            Response.Buffer = true;        
            Response.AddHeader("content-disposition", "attachment;filename=CreditConsignmentSummary.pdf");
            Response.Charset = "";
            Response.ContentType = "application/pdf";
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);

              //  GridView.AllowPaging = false;
               // Result();
                Table_1.RenderControl(hw);

                //style to format numbers to string
                string style = @"<style> .textmode { } </style>";
                Response.Write(style);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
        }
        */


        /// <summary>

        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>






        protected void paidstatus_chk_CheckedChanged(object sender, EventArgs e)
        {

        }




        public DataSet Get_CreditClientAccounts(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {


                string sqlString = "\n" +
                "SELECT B.ACCOUNTNO, B.NAME\n" +
                "  FROM (SELECT CC.ACCOUNTNO + '_NEW' ACCOUNTNO,\n" +
                "               CC.NAME + ' ( ' + CC.ACCOUNTNO + ' )' NAME\n" +
                "          FROM CODUsers CC\n" +
                "         WHERE CC.STATUS = '1'\n" +
                //"           AND CC.ACCOUNTNO IN ('4G7', '43C8', '4E22')\n" +
                "\n" +
                "        UNION ALL\n" +
                "\n" +
                "        SELECT CC.ACCOUNTNO + '_OLD' ACCOUNTNO,\n" +
                "               CC.NAME + ' ( ' + CC.ACCOUNTNO + ' )' NAME\n" +
                "          FROM CREDITCLIENTS CC\n" +
                "         WHERE CC.ISCOD = '1'\n" +
                "           AND CC.ACCOUNTNO NOT IN (SELECT CU.ACCOUNTNO FROM CODUsers CU WHERE CU.status = '1')) B\n" +
                //  "           AND CC.ACCOUNTNO NOT IN ('4G7', '43C8', '4E22')) B\n" +
                " GROUP BY B.ACCOUNTNO, B.NAME\n" +
                " ORDER BY 2";


                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
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

        public DataSet Get_CODPaymentReport(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {


                #region MyRegion
                //string sqlString = "select ROW_NUMBER()OVER(Order by b.CONSIGNMENTNUMBER) SR, b.CONSIGNMENTNUMBER, b.BOOKINGDATE, b.RRSTATUS, b.PaymentVoucherID, b.CODAMOUNT, b.ORGZONE, b.ORGZONECODE, b.accountNo, b.CUSTOMERNAME, b.DELIVEREDCN, b.PaidStatus  from (  \n" +
                //       "----- COD PAYMENT REPORTS\n" +
                //       " --" + HttpContext.Current.Session["U_NAME"].ToString() + "\n" +
                //       "      SELECT C.CONSIGNMENTNUMBER CONSIGNMENTNUMBER,\n" +
                //       "       CONVERT(VARCHAR(11), C.BOOKINGDATE, 106) BOOKINGDATE ,\n" +
                //       "       l.AttributeValue RRSTATUS, \n" +
                //       "       isnull(PV.ReceiptNo,'-') PaymentVoucherID,\n" +
                //       "       CD.CODAMOUNT CODAMOUNT,\n" +
                //       "       Z.NAME ORGZONE,\n" +
                //       "       Z.ZONECODE ORGZONECODE,\n" +
                //       "       CC.ACCOUNTNO,\n" +
                //       "       CC.NAME CUSTOMERNAME,\n" +
                //       "       COUNT(DISTINCT PV.CONSIGNMENTNO) DELIVEREDCN, \n" +
                //       "       case when c.isPayable = '1' then 'Paid' else 'UnPaid' end PaidStatus, \n" +
                //       "       i.invoiceNumber \n" +
                //       "  FROM CONSIGNMENT C\n" +
                //       " INNER JOIN CODCONSIGNMENTDETAIL CD\n" +
                //       "    ON CD.CONSIGNMENTNUMBER = C.CONSIGNMENTNUMBER\n" +
                //       "  LEFT JOIN RUNSHEETCONSIGNMENT R\n" +
                //       "    ON R.CONSIGNMENTNUMBER = C.CONSIGNMENTNUMBER\n" +
                //       "   AND  R.createdOn =\n" +
                //       "   (select MAX(createdOn)\n" +
                //       "   from RunsheetConsignment rc1\n" +
                //       "   where rc1.consignmentNumber = R.consignmentNumber) \n" +
                //       "  LEFT JOIN PaymentVouchers pv \n " +
                //       "    ON pv.ConsignmentNo = c.consignmentNumber \n" +
                //       " INNER JOIN CREDITCLIENTS CC\n" +
                //       "    ON CC.ID = C.CREDITCLIENTID\n" +
                //       " INNER JOIN BRANCHES B2\n" +
                //       "    ON CC.BRANCHCODE = B2.BRANCHCODE\n" +
                //       " INNER JOIN ZONES Z\n" +
                //       "    ON Z.ZONECODE = B2.ZONECODE\n" +
                //       " INNER JOIN BRANCHES B3\n" +
                //       "    ON C.DESTINATION = B3.BRANCHCODE\n" +
                //       " INNER JOIN ZONES Z2\n" +
                //       "    ON B3.ZONECODE = Z2.ZONECODE\n" +
                //    //" LEFT join InvoiceConsignment ic \n" +
                //    //" on c.consignmentNumber = ic.consignmentNumber\n" +
                //    //" left OUTER join Invoice i\n" +
                //    //" on ic.invoiceNumber = i.invoiceNumber\n" +
                //       "LEFT OUTER JOIN rvdbo.Lookup L \n" +
                //        " ON CAST(R.Reason AS VARCHAR(20)) = CAST(L.ID AS VARCHAR(20))\n" +
                //       "  WHERE C.COD = '1'\n" +
                //       "  AND (C.status <> '9' or C.status is null)\n" +
                //       "    AND isnull(i.IsInvoiceCanceled,0) != '1' \n " +
                //       "   AND CC.ACCOUNTNO = '" + clvar.ACNumber + "'\n" +
                //     clvar.StartDate + "\n" +
                //       "GROUP BY C.CONSIGNMENTNUMBER,\n" +
                //        "         CONVERT(VARCHAR(11), C.BOOKINGDATE, 106),\n" +
                //        "         l.AttributeValue, \n" +
                //        "         CD.CODAMOUNT,\n" +
                //        "         Z.NAME,\n" +
                //        "         Z.ZONECODE,\n" +
                //        "         CC.ACCOUNTNO,\n" +
                //        "         CC.NAME, PV.ReceiptNo, i.invoiceNumber, c.isPayable \n" +
                //        "UNION ALL \n" +
                //        "      SELECT C.CONSIGNMENTNUMBER CONSIGNMENTNUMBER,\n" +
                //       "       CONVERT(VARCHAR(11), C.BOOKINGDATE, 106) BOOKINGDATE ,\n" +
                //       "       l.AttributeValue RRSTATUS, \n" +
                //       "       isnull(PV.ReceiptNo,'-') PaymentVoucherID,\n" +
                //       "       CD.CODAMOUNT CODAMOUNT,\n" +
                //       "       Z.NAME ORGZONE,\n" +
                //       "       Z.ZONECODE ORGZONECODE,\n" +
                //       "       CC.ACCOUNTNO,\n" +
                //       "       CC.NAME CUSTOMERNAME,\n" +
                //       "       COUNT(DISTINCT PV.CONSIGNMENTNO) DELIVEREDCN, \n" +
                //       "       case when c.isPayable = '1' then 'Paid' else 'UnPaid' end PaidStatus, \n" +
                //       "       i.invoiceNumber \n" +
                //       "  FROM CONSIGNMENT C\n" +
                //       " INNER JOIN CODCONSIGNMENTDETAIL_NEW CD\n" +
                //       "    ON CD.CONSIGNMENTNUMBER = C.CONSIGNMENTNUMBER\n" +
                //       "  LEFT JOIN RUNSHEETCONSIGNMENT R\n" +
                //       "    ON R.CONSIGNMENTNUMBER = C.CONSIGNMENTNUMBER\n" +
                //       "   AND  R.createdOn =\n" +
                //       "   (select MAX(createdOn)\n" +
                //       "   from RunsheetConsignment rc1\n" +
                //       "   where rc1.consignmentNumber = R.consignmentNumber) \n" +
                //       "  LEFT JOIN PaymentVouchers pv \n " +
                //       "    ON pv.ConsignmentNo = c.consignmentNumber \n" +
                //       " INNER JOIN CREDITCLIENTS CC\n" +
                //       "    ON CC.ID = C.CREDITCLIENTID\n" +
                //       " INNER JOIN BRANCHES B2\n" +
                //       "    ON CC.BRANCHCODE = B2.BRANCHCODE\n" +
                //       " INNER JOIN ZONES Z\n" +
                //       "    ON Z.ZONECODE = B2.ZONECODE\n" +
                //       " INNER JOIN BRANCHES B3\n" +
                //       "    ON C.DESTINATION = B3.BRANCHCODE\n" +
                //       " INNER JOIN ZONES Z2\n" +
                //       "    ON B3.ZONECODE = Z2.ZONECODE\n" +
                //    //" LEFT join InvoiceConsignment ic \n" +
                //    //" on c.consignmentNumber = ic.consignmentNumber\n" +
                //    //" left OUTER join Invoice i\n" +
                //    //" on ic.invoiceNumber = i.invoiceNumber\n" +
                //       "LEFT OUTER JOIN rvdbo.Lookup L \n" +
                //        " ON CAST(R.Reason AS VARCHAR(20)) = CAST(L.ID AS VARCHAR(20))\n" +
                //       "  WHERE C.COD = '1'\n" +
                //       "  AND (C.status <> '9' or C.status is null)\n" +
                //       "    AND isnull(i.IsInvoiceCanceled,0) != '1' \n " +
                //       "   AND CC.ACCOUNTNO = '" + clvar.ACNumber + "'\n" +
                //     clvar.StartDate + "\n" +
                //       "GROUP BY C.CONSIGNMENTNUMBER,\n" +
                //        "         CONVERT(VARCHAR(11), C.BOOKINGDATE, 106),\n" +
                //        "         l.AttributeValue, \n" +
                //        "         CD.CODAMOUNT,\n" +
                //        "         Z.NAME,\n" +
                //        "         Z.ZONECODE,\n" +
                //        "         CC.ACCOUNTNO,\n" +
                //        "         CC.NAME, PV.ReceiptNo, i.invoiceNumber, c.isPayable  \n" +
                //        ") b \n" +
                //        "   where " + clvar.Seal + " \n" +
                //        "ORDER BY 1"; 

                //  string sqlString = "select ROW_NUMBER() OVER(Order by b.CONSIGNMENTNUMBER) SR,\n" +
                //"       b.CONSIGNMENTNUMBER,\n" +
                //"       b.BOOKINGDATE,\n" +
                //"       b.RRSTATUS,\n" +
                //"       b.PaymentVoucherID,\n" +
                //"       b.CODAMOUNT,\n" +
                //"       b.ORGZONE,\n" +
                //"       b.ORGZONECODE,\n" +
                //"       b.accountNo,\n" +
                //"       b.CUSTOMERNAME,\n" +
                //"       b.DELIVEREDCN,\n" +
                //"       b.PaidStatus, b.ChequeNo\n" +
                //"  from (\n" +
                //"        ----- COD PAYMENT REPORTS\n" +
                //"        --khi.acc@ocs.com.pk\n" +
                //"        SELECT C.CONSIGNMENTNUMBER CONSIGNMENTNUMBER,\n" +
                //"                CONVERT(VARCHAR(11), C.BOOKINGDATE, 106) BOOKINGDATE,\n" +
                //"                l.AttributeValue RRSTATUS,\n" +
                //"                isnull(PV.ReceiptNo, '-') PaymentVoucherID,\n" +
                //"                CD.CODAMOUNT CODAMOUNT,\n" +
                //"                Z.NAME ORGZONE,\n" +
                //"                Z.ZONECODE ORGZONECODE,\n" +
                //"                CC.ACCOUNTNO,\n" +
                //"                CC.NAME CUSTOMERNAME,\n" +
                //"                COUNT(DISTINCT PV.CONSIGNMENTNO) DELIVEREDCN,\n" +
                //"                case\n" +
                //"                  when c.isPayable = '1' then\n" +
                //"                   'Paid'\n" +
                //"                  else\n" +
                //"                   'UnPaid'\n" +
                //"                end PaidStatus, C.transactionNumber ChequeNo\n" +
                //"          FROM CONSIGNMENT C\n" +
                //"         INNER JOIN CODCONSIGNMENTDETAIL CD\n" +
                //"            ON CD.CONSIGNMENTNUMBER = C.CONSIGNMENTNUMBER\n" +
                //"          LEFT JOIN RUNSHEETCONSIGNMENT R\n" +
                //"            ON R.CONSIGNMENTNUMBER = C.CONSIGNMENTNUMBER\n" +
                //"           AND R.createdOn =\n" +
                //"               (select MAX(createdOn)\n" +
                //"                  from RunsheetConsignment rc1\n" +
                //"                 where rc1.consignmentNumber = R.consignmentNumber)\n" +
                //"          LEFT JOIN PaymentVouchers pv\n" +
                //"            ON pv.ConsignmentNo = c.consignmentNumber\n" +
                //"         INNER JOIN CREDITCLIENTS CC\n" +
                //"            ON CC.ID = C.CREDITCLIENTID\n" +
                //"         INNER JOIN BRANCHES B2\n" +
                //"            ON CC.BRANCHCODE = B2.BRANCHCODE\n" +
                //"         INNER JOIN ZONES Z\n" +
                //"            ON Z.ZONECODE = B2.ZONECODE\n" +
                //"         INNER JOIN BRANCHES B3\n" +
                //"            ON C.DESTINATION = B3.BRANCHCODE\n" +
                //"         INNER JOIN ZONES Z2\n" +
                //"            ON B3.ZONECODE = Z2.ZONECODE\n" +
                //"          LEFT OUTER JOIN rvdbo.Lookup L\n" +
                //"            ON CAST(R.Reason AS VARCHAR(20)) = CAST(L.ID AS VARCHAR(20))\n" +
                //"         WHERE C.COD = '1'\n" +
                //"           AND (C.status <> '9' or C.status is null)\n" +
                //"   AND CC.ACCOUNTNO = '" + clvar.ACNumber + "'\n" +
                //clvar.StartDate + "\n" +
                //"         GROUP BY C.CONSIGNMENTNUMBER,\n" +
                //"                   CONVERT(VARCHAR(11), C.BOOKINGDATE, 106),\n" +
                //"                   l.AttributeValue,\n" +
                //"                   CD.CODAMOUNT,\n" +
                //"                   Z.NAME,\n" +
                //"                   Z.ZONECODE,\n" +
                //"                   CC.ACCOUNTNO,\n" +
                //"                   CC.NAME,\n" +
                //"                   PV.ReceiptNo,\n" +
                //"                   c.isPayable, C.transactionNumber\n" +
                //"        UNION ALL\n" +
                //"        SELECT C.CONSIGNMENTNUMBER CONSIGNMENTNUMBER,\n" +
                //"               CONVERT(VARCHAR(11), C.BOOKINGDATE, 106) BOOKINGDATE,\n" +
                //"               l.AttributeValue RRSTATUS,\n" +
                //"               isnull(PV.ReceiptNo, '-') PaymentVoucherID,\n" +
                //"               CD.CODAMOUNT CODAMOUNT,\n" +
                //"               Z.NAME ORGZONE,\n" +
                //"               Z.ZONECODE ORGZONECODE,\n" +
                //"               CC.ACCOUNTNO,\n" +
                //"               CC.NAME CUSTOMERNAME,\n" +
                //"               COUNT(DISTINCT PV.CONSIGNMENTNO) DELIVEREDCN,\n" +
                //"               case\n" +
                //"                 when c.isPayable = '1' then\n" +
                //"                  'Paid'\n" +
                //"                 else\n" +
                //"                  'UnPaid'\n" +
                //"               end PaidStatus, C.transactionNumber ChequeNo\n" +
                //"          FROM CONSIGNMENT C\n" +
                //"         INNER JOIN CODCONSIGNMENTDETAIL_NEW CD\n" +
                //"            ON CD.CONSIGNMENTNUMBER = C.CONSIGNMENTNUMBER\n" +
                //"          LEFT JOIN RUNSHEETCONSIGNMENT R\n" +
                //"            ON R.CONSIGNMENTNUMBER = C.CONSIGNMENTNUMBER\n" +
                //"           AND R.createdOn =\n" +
                //"               (select MAX(createdOn)\n" +
                //"                  from RunsheetConsignment rc1\n" +
                //"                 where rc1.consignmentNumber = R.consignmentNumber)\n" +
                //"          LEFT JOIN PaymentVouchers pv\n" +
                //"            ON pv.ConsignmentNo = c.consignmentNumber\n" +
                //"         INNER JOIN CREDITCLIENTS CC\n" +
                //"            ON CC.ID = C.CREDITCLIENTID\n" +
                //"         INNER JOIN BRANCHES B2\n" +
                //"            ON CC.BRANCHCODE = B2.BRANCHCODE\n" +
                //"         INNER JOIN ZONES Z\n" +
                //"            ON Z.ZONECODE = B2.ZONECODE\n" +
                //"         INNER JOIN BRANCHES B3\n" +
                //"            ON C.DESTINATION = B3.BRANCHCODE\n" +
                //"         INNER JOIN ZONES Z2\n" +
                //"            ON B3.ZONECODE = Z2.ZONECODE\n" +
                //"          LEFT OUTER JOIN rvdbo.Lookup L\n" +
                //"            ON CAST(R.Reason AS VARCHAR(20)) = CAST(L.ID AS VARCHAR(20))\n" +
                //"         WHERE C.COD = '1'\n" +
                //"           AND (C.status <> '9' or C.status is null)\n" +
                //"           AND CC.ACCOUNTNO = '" + clvar.ACNumber + "'\n" +
                //clvar.StartDate + "\n" +
                //"         GROUP BY C.CONSIGNMENTNUMBER,\n" +
                //"                  CONVERT(VARCHAR(11), C.BOOKINGDATE, 106),\n" +
                //"                  l.AttributeValue,\n" +
                //"                  CD.CODAMOUNT,\n" +
                //"                  Z.NAME,\n" +
                //"                  Z.ZONECODE,\n" +
                //"                  CC.ACCOUNTNO,\n" +
                //"                  CC.NAME,\n" +
                //"                  PV.ReceiptNo,\n" +
                //"                  c.isPayable, C.transactionNumber) b\n" +
                //" where " + clvar.Seal + " \n" +
                //" ORDER BY 1";
                #endregion





                string sqlString = "SELECT ROW_NUMBER() OVER(ORDER BY B.CONSIGNMENTNUMBER) SR,\n" +
                "       B.CONSIGNMENTNUMBER,\n" +
                "       B.BOOKINGDATE,\n" +
                "       B.RRSTATUS,\n" +
                "       B.PAYMENTVOUCHERID, B.VoucherID, CAST(B.VOUCHERDATE as VARCHAR) VOUCHERDATE,B.COLLECTIONBR,\n" +
                "       B.CODAMOUNT,\n" +
                "       B.AvailableAmount,\n" +
                "       B.ORGZONE,\n" +
                "       B.ORGZONECODE,\n" +
                "       B.ACCOUNTNO,\n" +
                "       B.CUSTOMERNAME,\n" +
                "       B.DELIVEREDCN,\n" +
                "       B.PAIDSTATUS,\n" +
                "       B.CHEQUENO, B.STATUS DeliveryStatus\n" +
                "  FROM (\n" +
                "        ----- COD PAYMENT REPORTS\n" +
                "        --KHI.ACC@OCS.COM.PK\n" +
                "        SELECT C.CONSIGNMENTNUMBER CONSIGNMENTNUMBER,\n" +
                "                CONVERT(VARCHAR(11), C.BOOKINGDATE, 106) BOOKINGDATE,\n" +
                "                L.ATTRIBUTEVALUE RRSTATUS,\n" +
                "                ISNULL(PV.RECEIPTNO, '-') PAYMENTVOUCHERID, pv.id VoucherID, PV.VOUCHERDATE,B4.SNAME COLLECTIONBR,\n" +
                "                CD.CODAMOUNT CODAMOUNT,\n" +
                "                (PV.Amount - PV.AmountUsed) AvailableAmount,\n" +
                "                Z.NAME ORGZONE,\n" +
                "                Z.ZONECODE ORGZONECODE,\n" +
                "                CC.ACCOUNTNO,\n" +
                "                CC.NAME CUSTOMERNAME,\n" +
                "                COUNT(DISTINCT PV.CONSIGNMENTNO) DELIVEREDCN,\n" +
                "                CASE\n" +
                "                  WHEN C.ISPAYABLE = '1' THEN\n" +
                "                   'PAID'\n" +
                "                  ELSE\n" +
                "                   'UNPAID'\n" +
                "                END PAIDSTATUS,\n" +
                "                C.TRANSACTIONNUMBER CHEQUENO, R.STATUS\n" +
                "          FROM CONSIGNMENT C\n" +
                "         INNER JOIN CODCONSIGNMENTDETAIL CD\n" +
                "            ON CD.CONSIGNMENTNUMBER = C.CONSIGNMENTNUMBER\n" +
                "          LEFT JOIN RUNSHEETCONSIGNMENT R\n" +
                "            ON R.CONSIGNMENTNUMBER = C.CONSIGNMENTNUMBER\n" +
                "           AND R.CREATEDON =\n" +
                "               (SELECT MAX(CREATEDON)\n" +
                "                  FROM RUNSHEETCONSIGNMENT RC1\n" +
                "                 WHERE RC1.CONSIGNMENTNUMBER = R.CONSIGNMENTNUMBER)\n" +
                "          LEFT JOIN PAYMENTVOUCHERS PV\n" +
                "            ON PV.CONSIGNMENTNO = C.CONSIGNMENTNUMBER\n" +
                "           AND PV.CREDITCLIENTID = C.CREDITCLIENTID\n" +
                "         INNER JOIN CREDITCLIENTS CC\n" +
                "            ON CC.ID = C.CREDITCLIENTID\n" +
                "         INNER JOIN BRANCHES B2\n" +
                "            ON CC.BRANCHCODE = B2.BRANCHCODE\n" +
                "         INNER JOIN ZONES Z\n" +
                "            ON Z.ZONECODE = B2.ZONECODE\n" +
                "         INNER JOIN BRANCHES B3\n" +
                "            ON C.DESTINATION = B3.BRANCHCODE\n" +
                "         INNER JOIN ZONES Z2\n" +
                "            ON B3.ZONECODE = Z2.ZONECODE\n" +
                "         INNER JOIN Branches B4\n" +
                "		     ON B4.branchCode = PV.BranchCode" +
                "          LEFT OUTER JOIN RVDBO.LOOKUP L\n" +
                "            ON CAST(R.REASON AS VARCHAR(20)) = CAST(L.ID AS VARCHAR(20))\n" +
                "         WHERE C.COD = '1' and c.isapproved = '1'\n" +
                "           --AND C.transactionNumber IS NULL\n" +
                "           AND (C.STATUS <> '9' OR C.STATUS IS NULL)\n" +
                "           AND CC.ACCOUNTNO = '" + clvar.ACNumber + "'\n" +
               clvar.StartDate + "\n" +
                "\n" +
                "         GROUP BY C.CONSIGNMENTNUMBER,\n" +
                "                   CONVERT(VARCHAR(11), C.BOOKINGDATE, 106),\n" +
                "                   L.ATTRIBUTEVALUE,\n" +
                "                   CD.CODAMOUNT, \n" +
                "                   Z.NAME,\n" +
                "                   Z.ZONECODE,\n" +
                "                   CC.ACCOUNTNO,\n" +
                "                   CC.NAME,\n" +
                "                   PV.RECEIPTNO, pv.id,\n" +
                "                   C.ISPAYABLE,\n" +
                "                   C.TRANSACTIONNUMBER,\n" +
                "                   PV.VOUCHERDATE,B4.SNAME, PV.Amount, PV.Amountused, R.STATUS\n" +
                "        UNION ALL\n" +
                "        SELECT C.CONSIGNMENTNUMBER CONSIGNMENTNUMBER,\n" +
                "               CONVERT(VARCHAR(11), C.BOOKINGDATE, 106) BOOKINGDATE,\n" +
                "               L.ATTRIBUTEVALUE RRSTATUS,\n" +
                "               ISNULL(PV.RECEIPTNO, '-') PAYMENTVOUCHERID, pv.ID VoucherID, PV.VOUCHERDATE,B4.SNAME COLLECTIONBR,\n" +
                "               CD.CODAMOUNT CODAMOUNT,\n" +
                "               (PV.Amount - PV.AmountUsed) AvailableAmount,\n" +
                "               Z.NAME ORGZONE,\n" +
                "               Z.ZONECODE ORGZONECODE,\n" +
                "               CC.ACCOUNTNO,\n" +
                "               CC.NAME CUSTOMERNAME,\n" +
                "               COUNT(DISTINCT PV.CONSIGNMENTNO) DELIVEREDCN,\n" +
                "               CASE\n" +
                "                 WHEN C.ISPAYABLE = '1' THEN\n" +
                "                  'PAID'\n" +
                "                 ELSE\n" +
                "                  'UNPAID'\n" +
                "               END PAIDSTATUS,\n" +
                "               C.TRANSACTIONNUMBER CHEQUENO, R.STATUS\n" +
                "          FROM CONSIGNMENT C\n" +
                "         INNER JOIN CODCONSIGNMENTDETAIL_NEW CD\n" +
                "            ON CD.CONSIGNMENTNUMBER = C.CONSIGNMENTNUMBER\n" +
                "          LEFT JOIN RUNSHEETCONSIGNMENT R\n" +
                "            ON R.CONSIGNMENTNUMBER = C.CONSIGNMENTNUMBER\n" +
                "           AND R.CREATEDON =\n" +
                "               (SELECT MAX(CREATEDON)\n" +
                "                  FROM RUNSHEETCONSIGNMENT RC1\n" +
                "                 WHERE RC1.CONSIGNMENTNUMBER = R.CONSIGNMENTNUMBER)\n" +
                "          LEFT JOIN PAYMENTVOUCHERS PV\n" +
                "            ON PV.CONSIGNMENTNO = C.CONSIGNMENTNUMBER\n" +
                "           AND PV.CREDITCLIENTID = C.CREDITCLIENTID\n" +
                "         INNER JOIN CREDITCLIENTS CC\n" +
                "            ON CC.ID = C.CREDITCLIENTID\n" +
                "         INNER JOIN BRANCHES B2\n" +
                "            ON CC.BRANCHCODE = B2.BRANCHCODE\n" +
                "         INNER JOIN ZONES Z\n" +
                "            ON Z.ZONECODE = B2.ZONECODE\n" +
                "         INNER JOIN BRANCHES B3\n" +
                "            ON C.DESTINATION = B3.BRANCHCODE\n" +
                "         INNER JOIN ZONES Z2\n" +
                "            ON B3.ZONECODE = Z2.ZONECODE\n" +
                "         INNER JOIN Branches B4\n" +
                "		     ON B4.branchCode = PV.BranchCode" +
                "          LEFT OUTER JOIN RVDBO.LOOKUP L\n" +
                "            ON CAST(R.REASON AS VARCHAR(20)) = CAST(L.ID AS VARCHAR(20))\n" +
                "         WHERE C.COD = '1' and c.isapproved = '1'\n" +
                "           --AND C.transactionNumber IS NULL\n" +
                "           AND (C.STATUS <> '9' OR C.STATUS IS NULL)\n" +
                "           AND CC.ACCOUNTNO = '" + clvar.ACNumber + "'\n" +
               clvar.StartDate + "\n" +
                "\n" +
                "         GROUP BY C.CONSIGNMENTNUMBER,\n" +
                "                  CONVERT(VARCHAR(11), C.BOOKINGDATE, 106),\n" +
                "                  L.ATTRIBUTEVALUE,\n" +
                "                  CD.CODAMOUNT,\n" +
                "                  Z.NAME,\n" +
                "                  Z.ZONECODE,\n" +
                "                  CC.ACCOUNTNO,\n" +
                "                  CC.NAME,\n" +
                "                  PV.RECEIPTNO, pv.id,\n" +
                "                  C.ISPAYABLE,\n" +
                "                  C.TRANSACTIONNUMBER,\n" +
                "                  PV.VOUCHERDATE,B4.SNAME, PV.amount, PV.AmountUsed, R.STATUS) B\n" +
                " where " + clvar.Seal + " \n" +
                " ORDER BY 1";



                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandTimeout = 3000;
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
        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chk = e.Row.FindControl("chk_paid") as CheckBox;
                if (e.Row.Cells[4].Text.ToString() == "-" || e.Row.Cells[6].Text.ToUpper() == "PAID")
                {

                    //chk.Enabled = false;
                    if (e.Row.Cells[6].Text.ToUpper() == "PAID")
                    {
                        chk.Checked = true;
                    }
                }
                if ((paidstatus.SelectedValue == "PAID" && e.Row.Cells[4].Text.ToString() == "-"))
                {
                    chk.Enabled = false;
                    if (paidstatus.SelectedValue == "PRINT" && e.Row.Cells[4].Text.ToString() == "-")
                    {
                        chk.Checked = false;
                    }
                }

            }
        }
        protected void btn_save_Click(object sender, EventArgs e)
        {

            if (paidstatus.SelectedValue == "PAID")
            {
                #region Marking Paid
                string CN = "";
                DataTable ConsignmentNumbers = new DataTable();
                ConsignmentNumbers.Columns.Add("ConsignmentNumber", typeof(string));
                bool doSave = false;
                foreach (GridViewRow row in GridView2.Rows)
                {
                    CheckBox chk = row.FindControl("chk_paid") as CheckBox;
                    string chequeNumber = row.Cells[7].Text;
                    if (chk.Checked /*&& chk.Enabled == true*/)
                    {
                        doSave = true;
                        CN += "'" + row.Cells[1].Text + "'";
                        ConsignmentNumbers.Rows.Add(row.Cells[1].Text);
                    }
                }

                CN = CN.Replace("''", "','");

                if (doSave)
                {
                    if (txt_chequeNo.Text.Trim() == "")
                    {
                        clvar_.CheckCondition = "";
                    }
                    else
                    {
                        clvar_.CheckCondition = ", TransactionNumber = '" + txt_chequeNo.Text + "'";
                    }

                    #region AutoRedeem Calculations
                    double balAmount = 0;
                    double.TryParse((gv_invoiceDetail.FooterRow.FindControl("lbl_fBalance") as Label).Text, out balAmount);
                    if (balAmount < 0)
                    {
                        Alert("Payment Not Allowed. Not Enoguh COD Payable.", "Red");
                        return;
                    }
                    DataTable dt = new DataTable();
                    dt.Columns.AddRange(new DataColumn[]{
                    new DataColumn("RR"),
                    new DataColumn("Invoice"),
                    new DataColumn("Amount", typeof(double)),
                    new DataColumn("Finished")
                    });

                    foreach (GridViewRow row in GridView2.Rows)
                    {
                        CheckBox chk = row.FindControl("chk_paid") as CheckBox;
                        if (chk.Checked)
                        {
                            DataRow dr = dt.NewRow();
                            dr["RR"] = (row.FindControl("hd_voucherID") as HiddenField).Value;
                            dr["Amount"] = row.Cells[8].Text;
                            dr["Finished"] = "0";
                            dt.Rows.Add(dr);
                        }
                    }

                    DataTable finalDt = new DataTable();
                    finalDt.Columns.AddRange(new DataColumn[] {
                    new DataColumn("VoucherID", typeof(Int64)),
                    new DataColumn("Invoice", typeof(string)),
                    new DataColumn("Amount", typeof(float))
                });
                    int k = 0;
                    for (int i = 0; i < gv_invoiceDetail.Rows.Count; i++)
                    {
                        string invoiceNumber = (gv_invoiceDetail.Rows[i].FindControl("lbl_invoiceNumber") as Label).Text;
                        string invoiceAmount = (gv_invoiceDetail.Rows[i].FindControl("lbl_outstanding") as Label).Text;

                        float remainingIAmount = 0;
                        float remainingRRAmount = 0;

                        float.TryParse(invoiceAmount, out remainingIAmount);

                        for (int j = k; j < dt.Rows.Count; j++)
                        {
                            Int64 RR = Int64.Parse(dt.Rows[j][0].ToString());
                            string RRa = dt.Rows[j][2].ToString();


                            float.TryParse(RRa, out remainingRRAmount);
                            if (remainingIAmount == 0)
                            {
                                break;
                            }
                            if (remainingRRAmount == 0)
                            {
                                continue;
                            }
                            if (remainingRRAmount <= remainingIAmount)
                            {
                                DataRow dr = finalDt.NewRow();
                                dr[0] = RR;
                                dr[1] = invoiceNumber;
                                dr[2] = remainingRRAmount;
                                finalDt.Rows.Add(dr);
                                remainingIAmount = remainingIAmount - remainingRRAmount;
                                dt.Rows[j][3] = "1";
                                dt.Rows[j][2] = "0";
                                remainingRRAmount = 0;
                                k = k + 1;
                            }
                            else if (remainingRRAmount > remainingIAmount && remainingIAmount > 0)
                            {
                                DataRow dr = finalDt.NewRow();
                                dr[0] = RR;
                                dr[1] = invoiceNumber;
                                dr[2] = remainingIAmount;
                                finalDt.Rows.Add(dr);
                                dt.Rows[j][3] = "0";
                                dt.Rows[j][2] = remainingRRAmount - remainingIAmount;
                                remainingRRAmount = remainingRRAmount - remainingIAmount;
                                //i--;
                                //j--;
                                remainingIAmount = 0;
                            }

                        }

                    }
                    #endregion
                    finalDt.Compute("SUM(amount)", "");
                    //finalDt.Columns.Remove("Finished");

                    string error = MarkPaid(CN, finalDt, ConsignmentNumbers);
                    if (error == "OK")
                    {
                        Alert("Consignment Updated", "Green");
                        Button4_Click(this, e);
                        Btn_Search_Click(this, e);
                    }
                    else
                    {
                        Alert(error, "Red");
                        return;
                    }
                }
                #endregion
            }
            else if (paidstatus.SelectedValue == "UNPAID")
            {
                #region Marking Unpaid
                //string query = "";
                //bool update = false;

                //foreach (GridViewRow row in GridView2.Rows)
                //{
                //    CheckBox chk = row.FindControl("chk_paid") as CheckBox;
                //    string consignmentNumber = row.Cells[1].Text;
                //    clvar_.consignmentNo = consignmentNumber;
                //    string chequeNumber = (row.FindControl("lbl_cheque") as TextBox).Text;
                //    if (chk.Checked)
                //    {
                //        clvar_.CheckCondition = " SET isPayable = '0', TransactionNumber = NULL";
                //    }
                //    else
                //    {
                //        clvar_.CheckCondition = "";
                //    }

                //    update = true;
                //}

                //if (update)
                //{
                //    MarkUnPaid(clvar_);
                //    Btn_Search_Click(this, e);
                //}

                #endregion
            }
            else if (paidstatus.SelectedValue == "EDIT")
            {
                #region Editing Cheque Number

                string CN = "";
                bool doSave = false;
                foreach (GridViewRow row in GridView2.Rows)
                {
                    CheckBox chk = row.FindControl("chk_paid") as CheckBox;
                    string chequeNumber = row.Cells[7].Text;
                    if (chk.Checked && chk.Enabled == true)
                    {
                        doSave = true;
                        CN += "'" + row.Cells[1].Text + "'";
                    }
                }

                CN = CN.Replace("''", "','");

                if (doSave)
                {
                    if (txt_chequeNo.Text.Trim() == "")
                    {
                        Alert("Enter Cheque Number", "Red");
                        return;
                    }
                    else
                    {
                        clvar_.CheckCondition = " TransactionNumber = '" + txt_chequeNo.Text + "'";
                    }
                    string error = EditChequeNumber(CN);
                    if (error == "OK")
                    {
                        Alert("Consignment Updated", "Green");
                        Btn_Search_Click(this, e);
                    }
                    else
                    {
                        Alert(error, "Red");
                        return;
                    }
                }

                #endregion
            }






        }
        protected void btn_reset_Click(object sender, EventArgs e)
        {

        }

        public void Alert(string message, string color)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + message + "')", true);
            Errorid.Text = message;
            Errorid.ForeColor = System.Drawing.Color.FromName(color);
        }

        public string MarkPaid(string CN, DataTable dt, DataTable cns)
        {
            string error = "";
            string archiveString = "insert into Consignment_Archive select * from consignment c where c.consignmentnumber in (" + CN + ")";
            string updateString = "update consignment set isPayable = '1' " + clvar_.CheckCondition + " where consignmentNumber in (" + CN + ")";
            #region MyRegion
            //SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            //SqlTransaction trans;
            //sqlcon.Open();
            //SqlCommand sqlcmd = new SqlCommand();
            //sqlcmd.Connection = sqlcon;
            //trans = sqlcon.BeginTransaction();
            //try
            //{


            //    sqlcmd.Transaction = trans;
            //    sqlcmd.CommandType = CommandType.Text;
            //    sqlcmd.CommandText = archiveString;

            //    int cnt = sqlcmd.ExecuteNonQuery();
            //    if (cnt == 0)
            //    {
            //        trans.Rollback();
            //        error = "CONSIGNMENTS COULD NOT BE UPDATED";
            //        sqlcon.Close();
            //        return error;
            //    }
            //    cnt = 0;
            //    sqlcmd.CommandText = updateString;
            //    cnt = sqlcmd.ExecuteNonQuery();
            //    if (cnt == 0)
            //    {
            //        trans.Rollback();
            //        error = "CONSIGNMENTS COULD NOT BE UPDATED";
            //        sqlcon.Close();
            //        return error;
            //    }

            //    trans.Commit();
            //    error = "OK";
            //}
            //catch (Exception ex)
            //{
            //    trans.Rollback();
            //    sqlcon.Close();
            //    error = ex.Message;
            //} 
            #endregion

            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "MnP_CODPayment_Paid";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = con;
                cmd.Parameters.AddWithValue("@BranchCode", HttpContext.Current.Session["BranchCode"].ToString());
                cmd.Parameters.AddWithValue("@ZoneCode", HttpContext.Current.Session["ZoneCode"].ToString());
                cmd.Parameters.AddWithValue("@CNNumbers", cns);
                cmd.Parameters.AddWithValue("@tbl_redeem", dt);
                cmd.Parameters.AddWithValue("@CreatedBy", HttpContext.Current.Session["U_ID"].ToString());
                if (txt_chequeNo.Text.Trim() != "")
                {
                    cmd.Parameters.AddWithValue("@ChequeNumber", txt_chequeNo.Text);
                }
                cmd.Parameters.Add("@ResultStatus", SqlDbType.Int).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@result", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();

                if (cmd.Parameters["@ResultStatus"].Value.ToString() == "0")
                {
                    error = cmd.Parameters["@result"].Value.ToString();
                }
                else
                {
                    error = "OK";
                    txt_chequeNo.Text = cmd.Parameters["@result"].Value.ToString();
                }
            }
            catch (Exception ex)
            { error = ex.Message; }
            finally { con.Close(); }
            return error;
        }
        public string EditChequeNumber(string CN)
        {
            string error = "";
            string archiveString = "insert into Consignment_Archive select * from consignment c where c.consignmentnumber in (" + CN + ")";
            string updateString = "update consignment set " + clvar_.CheckCondition + " where consignmentNumber in (" + CN + ")";
            SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            SqlTransaction trans;
            sqlcon.Open();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcon;
            trans = sqlcon.BeginTransaction();
            try
            {


                sqlcmd.Transaction = trans;
                sqlcmd.CommandType = CommandType.Text;
                sqlcmd.CommandText = archiveString;

                int cnt = sqlcmd.ExecuteNonQuery();
                if (cnt == 0)
                {
                    trans.Rollback();
                    error = "CONSIGNMENTS COULD NOT BE UPDATED";
                    sqlcon.Close();
                    return error;
                }
                cnt = 0;
                sqlcmd.CommandText = updateString;
                cnt = sqlcmd.ExecuteNonQuery();
                if (cnt == 0)
                {
                    trans.Rollback();
                    error = "CONSIGNMENTS COULD NOT BE UPDATED";
                    sqlcon.Close();
                    return error;
                }

                trans.Commit();
                error = "OK";
            }
            catch (Exception ex)
            {
                trans.Rollback();
                sqlcon.Close();
                error = ex.Message;
            }
            return error;
        }
        public string MarkUnPaid(Cl_Variables clvar)
        {
            string error = "";
            string archiveCommand = "insert into Consignment_Archive select * from consignment where consignmentNumber = '" + clvar.consignmentNo + "'";
            string updateCommand = "Update Consignment " + clvar.CheckCondition + ", Modifiedon = GETDATE(), ModifiedBy = '" + HttpContext.Current.Session["U_ID"].ToString() + "' where consignmentNumber = '" + clvar.consignmentNo + "'";

            SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            SqlTransaction trans;
            sqlcon.Open();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcon;
            trans = sqlcon.BeginTransaction();
            try
            {
                sqlcmd.Transaction = trans;
                sqlcmd.CommandType = CommandType.Text;
                sqlcmd.CommandText = archiveCommand;
                int count = sqlcmd.ExecuteNonQuery();
                if (count == 0)
                {
                    trans.Rollback();
                    error = "Could Not Update";
                    sqlcon.Close();
                    return error;
                }
                sqlcmd.CommandText = updateCommand;
                count = 0;
                count = sqlcmd.ExecuteNonQuery();
                if (count == 0)
                {
                    trans.Rollback();
                    error = "Could Not Update";
                    sqlcon.Close();
                    return error;
                }
                trans.Commit();
                error = "OK";
                sqlcon.Close();
            }
            catch (Exception ex)
            { trans.Rollback(); sqlcon.Close(); error = ex.Message; }
            return error;
        }
        public string Print()
        {
            return "";
        }
        protected void Button4_Click(object sender, EventArgs e)
        {

            //List<string> paidCNs = new List<string>();
            //foreach (GridViewRow row in GridView2.Rows)
            //{
            //    CheckBox chk = row.FindControl("chk_paid") as CheckBox;
            //    if (chk.Checked)
            //    {
            //        paidCNs.Add(row.Cells[1].Text);
            //    }
            //}

            //if (paidCNs.Count > 0)
            //{
            //    Session["paidCNs"] = paidCNs;
            //    string script_ = @"window.open(""{0}"", ""{1}"", ""{2}"");";
            //    //string script = String.Format(script_, "RunsheetInvoice.aspx?Xcode=" + temp_, "_blank", "");
            //    string script = String.Format(script_, "CODPayments_print.aspx?", "_blank", "");

            //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Redirect", script, true);
            //}
            //else
            //{
            //    Alert("Select CNs first", "Red");
            //    return;
            //}

            foreach (GridViewRow row in GridView2.Rows)
            {
                CheckBox chk = row.FindControl("chk_paid") as CheckBox;
                if (chk.Checked)
                {
                    clvar.Check_Condition1 += "'" + (row.FindControl("hd_voucherID") as HiddenField).Value + "'";
                }
            }
            clvar.Check_Condition1 = clvar.Check_Condition1.Replace("''", "','");

            ExporttopdfSummary();
            Btn_Search_Click(this, e);
        }



        public DataTable GetInvoicesForVoucherNonCentralized(Cl_Variables clvar)
        {

            string sqlString = "SELECT b.invoiceNumber,\n" +
            "       b.clientId,\n" +
            "       SUM(b.Invoice_Amount) Invoice_Amount,\n" +
            "       SUM(b.Recovery) RECOVERY,\n" +
            "       SUM(b.Adjustment) Adjustment,\n" +
            "       SUM(b.Invoice_Amount) - (SUM(b.Recovery) + SUM(b.Adjustment)) Oustanding\n" +
            "  FROM (SELECT i.invoiceNumber,\n" +
            "               i.clientId,\n" +
            "               SUM(i.totalAmount) Invoice_Amount,\n" +
            "               0 RECOVERY,\n" +
            "               0 Adjustment\n" +
            "          FROM Invoice AS i\n" +
            "           Where  i.IsInvoiceCanceled = '0'\n" +
            "         GROUP BY i.invoiceNumber, i.clientId\n" +
            "\n" +
            "        UNION\n" +
            "\n" +
            "        SELECT ir.InvoiceNo,\n" +
            "               i.clientId,\n" +
            "               0 Invoice_Amount,\n" +
            "               SUM(ir.Amount) RECOVERY,\n" +
            "               0 Adjustment\n" +
            "          FROM InvoiceRedeem AS ir\n" +
            "         INNER JOIN Invoice AS i\n" +
            "            ON i.invoiceNumber = ir.InvoiceNo\n" +
            "         INNER JOIN PaymentVouchers AS pv\n" +
            "            ON pv.Id = ir.PaymentVoucherId\n" +
            "         WHERE pv.PaymentSourceId = '1'\n" +
            "           and  i.IsInvoiceCanceled = '0' \n" +
            "         GROUP BY ir.InvoiceNo, i.clientId\n" +
            "\n" +
            "        UNION\n" +
            "\n" +
            "        SELECT ir.InvoiceNo,\n" +
            "               i.clientId,\n" +
            "               0 Invoice_Amount,\n" +
            "               SUM(ir.Amount) RECOVERY,\n" +
            "               0 Adjustment\n" +
            "          FROM InvoiceRedeem AS ir\n" +
            "         INNER JOIN Invoice AS i\n" +
            "            ON i.invoiceNumber = ir.InvoiceNo\n" +
            "         INNER JOIN PaymentVouchers AS pv\n" +
            "            ON pv.Id = ir.PaymentVoucherId\n" +
            "         INNER JOIN ChequeStatus AS cs\n" +
            "            ON cs.PaymentVoucherId = pv.Id\n" +
            "         WHERE pv.PaymentSourceId = '2'\n" +
            "           AND cs.IsCurrentState = '1'\n" +
            "           AND cs.ChequeStateId IN ('1', '2')\n" +
            "           and  i.IsInvoiceCanceled = '0' \n" +
            "         GROUP BY ir.InvoiceNo, i.clientId\n" +
            "\n" +
            "        UNION\n" +
            "\n" +
            "        SELECT gv.InvoiceNo,\n" +
            "               gv.CreditClientId,\n" +
            "               0 Invoice_Amount,\n" +
            "               0 RECOVERY,\n" +
            "               SUM(gv.Amount) Adjustment\n" +
            "          FROM GeneralVoucher AS gv\n" +
            "         GROUP BY gv.InvoiceNo, gv.CreditClientId) b\n" +
            " WHERE b.clientId = '" + clvar.CreditClientID + "'\n" +
            " GROUP BY b.invoiceNumber, b.clientId\n" +
            "HAVING SUM(b.Invoice_Amount) - (SUM(b.Recovery) + SUM(b.Adjustment)) > 0";



            sqlString = "SELECT b.invoiceNumber,\n" +
            "       b.clientId,\n" +
            "       b2.name Branch,\n" +
            "       datename(MM,i.startDate)+'-'+datename(YY,i.startDate) Month,\n" +
            "       c.companyName,\n" +
            "       SUM(b.Invoice_Amount)     Total_Amount,\n" +
            "--       SUM(b.[Recovery])         RECOVERY,\n" +
            "--       SUM(b.Adjustment)         Adjustment,\n" +
            "--case when SUM(b.Invoice_Amount) -(SUM(b.Recovery) + SUM(b.Adjustment)) > 1 then SUM(b.Invoice_Amount) -(SUM(b.Recovery) + SUM(b.Adjustment)) else 0 end oustanding\n" +
            "       SUM(b.Invoice_Amount) -(SUM(b.Recovery) + SUM(b.Adjustment))Oustanding\n" +
            "FROM   (\n" +
            "           SELECT i.invoiceNumber,\n" +
            "                  i.clientId,\n" +
            "                  SUM(i.totalAmount)     Invoice_Amount,\n" +
            "                  0                      RECOVERY,\n" +
            "                  0                      Adjustment\n" +
            "           FROM   Invoice             AS i\n" +
            "           Where  i.IsInvoiceCanceled = '0' \n" +
            "           GROUP BY\n" +
            "                  i.invoiceNumber,\n" +
            "                  i.clientId\n" +
            "\n" +
            "           UNION\n" +
            "\n" +
            "           SELECT ir.InvoiceNo,\n" +
            "                  i.clientId,\n" +
            "                  0                      Invoice_Amount,\n" +
            "                  SUM(ir.Amount)         RECOVERY,\n" +
            "                  0                      Adjustment\n" +
            "           FROM   InvoiceRedeem       AS ir\n" +
            "                  INNER JOIN Invoice  AS i\n" +
            "                       ON  i.invoiceNumber = ir.InvoiceNo\n" +
            "                  INNER JOIN PaymentVouchers AS pv\n" +
            "                       ON  pv.Id = ir.PaymentVoucherId\n" +
            "           WHERE  ISNULL(pv.PaymentSourceId, 1) in ('1','8')\n" +
            "           and  i.IsInvoiceCanceled = '0' \n" +
            "           GROUP BY\n" +
            "                  ir.InvoiceNo,\n" +
            "                  i.clientId\n" +
            "\n" +
            "           UNION\n" +
            "\n" +
            "           SELECT ir.InvoiceNo,\n" +
            "                  i.clientId,\n" +
            "                  0                      Invoice_Amount,\n" +
            "                  SUM(ir.Amount)         RECOVERY,\n" +
            "                  0                      Adjustment\n" +
            "           FROM   InvoiceRedeem       AS ir\n" +
            "                  INNER JOIN Invoice  AS i\n" +
            "                       ON  i.invoiceNumber = ir.InvoiceNo\n" +
            "                  INNER JOIN PaymentVouchers AS pv\n" +
            "                       ON  pv.Id = ir.PaymentVoucherId\n" +
            "                  INNER JOIN ChequeStatus AS cs\n" +
            "                       ON  cs.PaymentVoucherId = pv.Id\n" +
            "           WHERE  pv.PaymentSourceId in ('2', '3', '4')\n" +
            "           and  i.IsInvoiceCanceled = '0' \n" +
            "                  AND cs.IsCurrentState = '1'\n" +
            "                  AND cs.ChequeStateId IN ('1', '2')\n" +
            "           GROUP BY\n" +
            "                  ir.InvoiceNo,\n" +
            "                  i.clientId\n" +
            "\n" +
            "\n" +
            "           UNION\n" +
            "\n" +
            "           SELECT gv.InvoiceNo,\n" +
            "                  gv.CreditClientId,\n" +
            "                  0                  Invoice_Amount,\n" +
            "                  0                  RECOVERY,\n" +
            "                  SUM(gv.Amount)     Adjustment\n" +
            "           FROM   GeneralVoucher  AS gv\n" +
            "           GROUP BY\n" +
            "                  gv.InvoiceNo,\n" +
            "                  gv.CreditClientId\n" +
            "       )                         b\n" +
            "INNER JOIN Invoice AS i ON i.invoiceNumber=b.invoiceNumber\n" +
            "INNER JOIN CreditClients AS cc ON cc.id=b.clientId\n" +
            "INNER JOIN Branches AS b2 ON b2.branchCode = cc.branchCode\n" +
            "INNER JOIN Company AS c ON c.Id=i.companyId\n" +
            "\n" +
            "WHERE  b.clientId = '" + clvar.CreditClientID + "' and cc.sectorid != '0'\n" +
            "           and  i.IsInvoiceCanceled = '0' and i.startDate >= '2017-06-29' \n" +
           "GROUP BY\n" +
            "       b.invoiceNumber,\n" +
            "       b.clientId,\n" +
            "       b2.name ,\n" +
            "       datename(MM,i.startDate)+'-'+datename(YY,i.startDate) ,\n" +
            "       c.companyName\n" +
            "HAVING SUM(b.Invoice_Amount) -(SUM(b.Recovery) + SUM(b.Adjustment)) >= 1";

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
        public DataTable Get_CreditClientID(Cl_Variables clvar)
        {
            string query = "SELECT * FROM CreditClients cc WHERE cc.accountNo = '" + clvar.AccountNo + "' AND cc.isActive = '1'";
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




        public void ExporttopdfSummary()
        {
            string invNum = "";
            string Invoice_numbers = "";
            DataTable dt_upper = getUpperData();
            DataTable RTS = GetRTSData();
            if (dt_upper.Rows.Count == 0)
            {
                return;
            }


            for (int inv = 0; inv < dt_upper.Rows.Count; inv++)
            {
                if (dt_upper.Rows[inv]["invoicenumber"].ToString() != "")
                {
                    invNum += "'" + dt_upper.Rows[inv]["invoicenumber"].ToString() + "'";

                    if (inv + 1 < dt_upper.Rows.Count)
                    {
                        Invoice_numbers += dt_upper.Rows[inv]["invoicenumber"].ToString() + ",";
                    }

                    else
                    {
                        Invoice_numbers += dt_upper.Rows[inv]["invoicenumber"].ToString();
                    }
                }

            }
            invNum = invNum.Replace("''", "','");
            if (invNum.Trim() == "")
            {
                invNum = "''";
            }
            Document document = new Document(PageSize.A4, 88f, 88f, 10f, 10f);
            iTextSharp.text.Font NormalFont = FontFactory.GetFont("Courier New", 6, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);



            using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
            {
                PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                Phrase phrase = null;
                PdfPCell cell = null;
                PdfPTable table_up = null;
                PdfPTable table = null;
                PdfPTable table_Center = null;
                PdfPTable table_pageNum = null;
                PdfPTable table_header = null;
                iTextSharp.text.BaseColor color = null;


                document.Open();

                table_header = new PdfPTable(3);
                table_header.TotalWidth = 550f;
                table_header.LockedWidth = true;
                table_header.SetWidths(new float[] { 50f, 60f, 370f });



                iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(Server.MapPath("~/images") + "\\new_logo.png");
                jpg.ScaleAbsolute(60f, 30f);
                PdfPCell imageCell = new PdfPCell(jpg);
                imageCell.Colspan = 1;
                table_header.AddCell(imageCell);

                cell = PhraseCell(new Phrase("", FontFactory.GetFont("Courier New", 18, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.PaddingBottom = 0f;
                table_header.AddCell(cell);

                cell = PhraseCell(new Phrase("M&P Payment Detail ", FontFactory.GetFont("Courier New", 18, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.PaddingBottom = 0f;
                table_header.AddCell(cell);


                table_up = new PdfPTable(4);
                table_up.TotalWidth = 550f;
                table_up.LockedWidth = true;
                table_up.SetWidths(new float[] { 20f, 40f, 20f, 40f });
                //UPPER TABLE

                cell = PhraseCell(new Phrase("------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 4;
                cell.Padding = 10f;
                cell.UseVariableBorders = true;
                table_up.AddCell(cell);


                cell = PhraseCell(new Phrase("Account Number:", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.BorderColorTop = BaseColor.BLACK;
                cell.BorderColorLeft = BaseColor.BLACK;

                cell.Padding = 5f;
                cell.UseVariableBorders = true;
                table_up.AddCell(cell);


                cell = PhraseCell(new Phrase(dt_upper.Rows[0]["accountNo"].ToString(), FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.BorderColorTop = BaseColor.BLACK;
                cell.BorderColorRight = BaseColor.BLACK;
                cell.Padding = 5f;
                cell.UseVariableBorders = true;
                table_up.AddCell(cell);

                cell = PhraseCell(new Phrase("Payment ID :", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.BorderColorTop = BaseColor.BLACK;
                cell.BorderColorLeft = BaseColor.BLACK;

                cell.Padding = 5f;
                cell.UseVariableBorders = true;
                table_up.AddCell(cell);


                cell = PhraseCell(new Phrase(txt_chequeNo.Text, FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.BorderColorTop = BaseColor.BLACK;
                cell.BorderColorRight = BaseColor.BLACK;
                cell.Padding = 5f;
                cell.UseVariableBorders = true;
                table_up.AddCell(cell);

                cell = PhraseCell(new Phrase("Customer Name:", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.BorderColorLeft = BaseColor.BLACK;
                cell.Padding = 5f;
                cell.UseVariableBorders = true;
                table_up.AddCell(cell);

                cell = PhraseCell(new Phrase(dt_upper.Rows[0]["clientname"].ToString(), FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.BorderColorRight = BaseColor.BLACK;
                cell.Padding = 5f;
                cell.UseVariableBorders = true;
                table_up.AddCell(cell);

                cell = PhraseCell(new Phrase("Print Date/Time:", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.BorderColorLeft = BaseColor.BLACK;
                cell.Padding = 5f;
                cell.UseVariableBorders = true;
                table_up.AddCell(cell);

                cell = PhraseCell(new Phrase(DateTime.Now.ToString("dd/MM/yyyy hh:mm tt"), FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.BorderColorRight = BaseColor.BLACK;
                cell.Padding = 5f;
                cell.UseVariableBorders = true;
                table_up.AddCell(cell);

                cell = PhraseCell(new Phrase("City:", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.BorderColorLeft = BaseColor.BLACK;
                cell.Padding = 5f;
                cell.UseVariableBorders = true;
                table_up.AddCell(cell);


                cell = PhraseCell(new Phrase(dt_upper.Rows[0]["city"].ToString(), FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.BorderColorRight = BaseColor.BLACK;
                cell.Padding = 5f;
                cell.UseVariableBorders = true;
                table_up.AddCell(cell);
                //cell = PhraseCell(new Phrase("", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                //cell.Colspan = 1;
                //cell.BorderColorLeft = BaseColor.BLACK;
                //cell.Padding = 5f;
                //cell.UseVariableBorders = true;
                //table_up.AddCell(cell);


                cell = PhraseCell(new Phrase("Beneficiary Name:", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.BorderColorLeft = BaseColor.BLACK;
                cell.Padding = 5f;
                cell.UseVariableBorders = true;
                table_up.AddCell(cell);

                cell = PhraseCell(new Phrase(dt_upper.Rows[0]["BeneficiaryName"].ToString(), FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.BorderColorRight = BaseColor.BLACK;
                cell.Padding = 5f;
                cell.UseVariableBorders = true;
                table_up.AddCell(cell);








                cell = PhraseCell(new Phrase("Payment Period:", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.BorderColorLeft = BaseColor.BLACK;
                cell.Padding = 5f;
                cell.UseVariableBorders = true;
                table_up.AddCell(cell);

                DateTime dt_min = DateTime.Parse(dt_upper.Rows[0]["MinPaymentPeriod"].ToString());
                DateTime dt_max = DateTime.Parse(dt_upper.Rows[0]["MaxPaymentPeriod"].ToString());


                cell = PhraseCell(new Phrase(dt_min.ToString("dd/MM/yyyy") + " To " + dt_max.ToString("dd/MM/yyyy"), FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.BorderColorRight = BaseColor.BLACK;
                cell.Padding = 5f;
                cell.UseVariableBorders = true;
                table_up.AddCell(cell);

                cell = PhraseCell(new Phrase("", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.BorderColorLeft = BaseColor.BLACK;
                cell.Padding = 5f;
                cell.UseVariableBorders = true;
                table_up.AddCell(cell);



                cell = PhraseCell(new Phrase("", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 2;
                cell.BorderColorRight = BaseColor.BLACK;
                cell.Padding = 5f;
                cell.UseVariableBorders = true;
                table_up.AddCell(cell);


                cell = PhraseCell(new Phrase("Invoice Numbers:", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.BorderColorLeft = BaseColor.BLACK;
                cell.BorderColorBottom = BaseColor.BLACK;
                cell.Padding = 5f;
                cell.UseVariableBorders = true;
                table_up.AddCell(cell);




                cell = PhraseCell(new Phrase(Invoice_numbers, FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 3;
                cell.BorderColorRight = BaseColor.BLACK;
                cell.BorderColorBottom = BaseColor.BLACK;
                cell.Padding = 5f;
                cell.UseVariableBorders = true;
                table_up.AddCell(cell);



                cell = PhraseCell(new Phrase("------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 4;
                cell.Padding = 10f;
                cell.UseVariableBorders = true;
                table_up.AddCell(cell);

                //cell = PhraseCell(new Phrase("Invoice Number:     " + dt_upper.Rows[0]["city"].ToString(), FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                //cell.Colspan = 2;
                //cell.BorderColorBottom = BaseColor.BLACK;
                //cell.BorderColorRight = BaseColor.BLACK;
                //cell.Padding = 5f;
                //cell.UseVariableBorders = true;
                //table_up.AddCell(cell);

                //cell = PhraseCell(new Phrase("Reporting Date:        " + dt_upper.Rows[0]["city"].ToString(), FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                //cell.Colspan = 2;
                //cell.BorderColorBottom = BaseColor.BLACK;
                //cell.BorderColorLeft = BaseColor.BLACK;
                //cell.Padding = 5f;
                //cell.UseVariableBorders = true;
                //table_up.AddCell(cell);



                //Center table
                DataTable dt_right = CenterTableRight_Data(invNum);
                if (dt_right.Rows.Count == 0)
                {
                    return;
                }

                DataTable dt_left = CenterTableLeft_Data(invNum);
                if (dt_right.Rows.Count == 0)
                {
                    return;
                }


                table_Center = new PdfPTable(4);
                table_Center.TotalWidth = 550f;
                table_Center.LockedWidth = true;

                cell = PhraseCell(new Phrase("INVOICE SUMMARY ", FontFactory.GetFont("Courier New", 10, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_CENTER);
                cell.Colspan = 2;
                cell.PaddingBottom = 10f;
                cell.PaddingTop = 10f;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.BorderColorLeft = BaseColor.BLACK;
                cell.BorderColorRight = BaseColor.BLACK;
                cell.BorderColorTop = BaseColor.BLACK;
                cell.BorderColorBottom = BaseColor.BLACK;
                table_Center.AddCell(cell);

                cell = PhraseCell(new Phrase("PAYMENT SUMMARY", FontFactory.GetFont("Courier New", 10, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_CENTER);
                cell.Colspan = 2;
                cell.PaddingBottom = 10f;
                cell.PaddingTop = 10f;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.BorderColorLeft = BaseColor.BLACK;
                cell.BorderColorRight = BaseColor.BLACK;
                cell.BorderColorTop = BaseColor.BLACK;
                cell.BorderColorBottom = BaseColor.BLACK;
                table_Center.AddCell(cell);

                cell = PhraseCell(new Phrase("Shipping Charges:", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.BorderColorTop = BaseColor.BLACK;
                cell.BorderColorLeft = BaseColor.BLACK;
                cell.Padding = 5f;
                cell.UseVariableBorders = true;
                table_Center.AddCell(cell);

                double shipmentCharge = 0;

                double.TryParse(dt_right.Rows[0]["shipmentCharge"].ToString(), out shipmentCharge);

                cell = PhraseCell(new Phrase(string.Format("{0:N0}", shipmentCharge), FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.Padding = 5f;
                cell.BorderColorTop = BaseColor.BLACK;
                cell.UseVariableBorders = true;
                table_Center.AddCell(cell);


                cell = PhraseCell(new Phrase("Total COD Amount:", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.BorderColorLeft = BaseColor.BLACK;
                cell.BorderColorTop = BaseColor.BLACK;
                cell.Padding = 5f;
                cell.UseVariableBorders = true;
                table_Center.AddCell(cell);

                double TotalCODAmount = 0;
                double.TryParse(dt_left.Rows[0]["TotalCODAmount"].ToString(), out TotalCODAmount);

                cell = PhraseCell(new Phrase(string.Format("{0:N0}", TotalCODAmount), FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.BorderColorTop = BaseColor.BLACK;
                cell.BorderColorRight = BaseColor.BLACK;

                cell.Padding = 5f;
                cell.UseVariableBorders = true;
                table_Center.AddCell(cell);

                cell = PhraseCell(new Phrase("Fuel Surcharge:", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.BorderColorLeft = BaseColor.BLACK;
                cell.Padding = 5f;
                cell.UseVariableBorders = true;
                table_Center.AddCell(cell);

                double FUEL_CHARGE = 0;
                double.TryParse(dt_right.Rows[0]["FUEL_CHARGE"].ToString(), out FUEL_CHARGE);

                cell = PhraseCell(new Phrase(string.Format("{0:N0}", FUEL_CHARGE), FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;

                cell.Padding = 5f;
                cell.UseVariableBorders = true;
                table_Center.AddCell(cell);

                cell = PhraseCell(new Phrase("Total Invoice Amount:", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.BorderColorLeft = BaseColor.BLACK;
                cell.Padding = 5f;
                cell.UseVariableBorders = true;
                table_Center.AddCell(cell);

                double invoiceAmount = 0;
                double.TryParse(dt_left.Rows[0]["invoiceAmount"].ToString(), out invoiceAmount);

                cell = PhraseCell(new Phrase(string.Format("{0:N0}", invoiceAmount), FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.BorderColorRight = BaseColor.BLACK;
                cell.Padding = 5f;
                cell.UseVariableBorders = true;
                table_Center.AddCell(cell);

                cell = PhraseCell(new Phrase("GST:", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.BorderColorLeft = BaseColor.BLACK;
                cell.Padding = 5f;
                cell.UseVariableBorders = true;
                table_Center.AddCell(cell);

                double GST = 0;
                double.TryParse(dt_right.Rows[0]["GST"].ToString(), out GST);


                cell = PhraseCell(new Phrase(string.Format("{0:N0}", GST), FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;

                cell.Padding = 5f;
                cell.UseVariableBorders = true;
                table_Center.AddCell(cell);

                cell = PhraseCell(new Phrase("Net Payable:", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.BorderColorLeft = BaseColor.BLACK;
                cell.Padding = 5f;
                cell.UseVariableBorders = true;
                table_Center.AddCell(cell);

                double NetPayable = 0;
                double.TryParse(dt_left.Rows[0]["NetPayable"].ToString(), out NetPayable);

                cell = PhraseCell(new Phrase(string.Format("{0:N0}", NetPayable), FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.BorderColorRight = BaseColor.BLACK;
                cell.Padding = 5f;
                cell.UseVariableBorders = true;
                table_Center.AddCell(cell);







                cell = PhraseCell(new Phrase("Extra Charge:", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.BorderColorLeft = BaseColor.BLACK;
                cell.BorderColorBottom = BaseColor.BLACK;
                cell.Padding = 5f;
                cell.UseVariableBorders = true;
                table_Center.AddCell(cell);

                double EXTRACHARGES = 0;
                double.TryParse(dt_right.Rows[0]["EXTRACHARGES"].ToString(), out EXTRACHARGES);

                cell = PhraseCell(new Phrase(string.Format("{0:N0}", EXTRACHARGES), FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.BorderColorBottom = BaseColor.BLACK;

                cell.Padding = 5f;
                cell.UseVariableBorders = true;
                table_Center.AddCell(cell);


                cell = PhraseCell(new Phrase("Returned to Shipper", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.BorderColorLeft = BaseColor.BLACK;
                cell.Padding = 5f;
                cell.UseVariableBorders = true;
                table_Center.AddCell(cell);

                cell = PhraseCell(new Phrase(String.Format("{0:N2}", RTS.Rows[0][0]), FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.BorderColorRight = BaseColor.BLACK;
                cell.Padding = 5f;
                cell.UseVariableBorders = true;
                table_Center.AddCell(cell);



                cell = PhraseCell(new Phrase("Total Invoice Amount:", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.BorderColorLeft = BaseColor.BLACK;
                cell.BorderColorBottom = BaseColor.BLACK;
                cell.Padding = 5f;
                cell.UseVariableBorders = true;
                table_Center.AddCell(cell);

                double SUM_AMOUNT = shipmentCharge + FUEL_CHARGE + GST + EXTRACHARGES;


                cell = PhraseCell(new Phrase(string.Format("{0:N0}", SUM_AMOUNT), FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.BorderColorBottom = BaseColor.BLACK;

                cell.Padding = 5f;
                cell.UseVariableBorders = true;
                table_Center.AddCell(cell);


                cell = PhraseCell(new Phrase("", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.BorderColorLeft = BaseColor.BLACK;
                cell.BorderColorBottom = BaseColor.BLACK;
                cell.Padding = 5f;
                cell.UseVariableBorders = true;
                table_Center.AddCell(cell);


                cell = PhraseCell(new Phrase("", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.BorderColorBottom = BaseColor.BLACK;
                cell.BorderColorRight = BaseColor.BLACK;
                cell.Padding = 5f;
                cell.UseVariableBorders = true;
                table_Center.AddCell(cell);



                //space


                table_Center.SpacingAfter = 1000f;
                document.Add(table_header);
                document.Add(table_up);
                document.Add(table_Center);

                //Grid
                int c = 1;



                DataSet header = getData(invNum);//------------------------------------
                header.Tables[0].Columns.Add("Sr.", typeof(int));
                double footerCodAmount = 0;
                double footerShippingCharges = 0;

                foreach (DataRow dr in header.Tables[0].Rows)
                {
                    double temp = 0;
                    double.TryParse(dr["CODAmount"].ToString(), out temp);

                    footerCodAmount += temp;

                    temp = 0;
                    double.TryParse(dr["ShippingCharges"].ToString(), out temp);

                    footerShippingCharges += temp;

                }

                //header.Tables[0].Rows.Add("0", "", "", "", "", String.Format("{0:N2}", footerCodAmount), "", "", "", "", String.Format("{0:N2}", footerShippingCharges), "", "");

                GridView1.DataSource = header;
                GridView1.DataBind();
                int colcount = GridView1.Rows[0].Cells.Count;


                table = new PdfPTable(colcount);
                table.TotalWidth = 550f;

                table.SetWidths(new float[] { 25f, 60, 57f, 45f, 45f, 50f, 24f, 24, 30f, 65f, 50f, 40f });
                table.LockedWidth = true;

                for (int i = 0; i < colcount; i++)
                {
                    if (i == 0)
                    {
                        cell = PhraseCell(new Phrase("Account Number: " + dt_upper.Rows[0]["accountNo"].ToString(), FontFactory.GetFont("Courier New", 6, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_CENTER);
                        cell.Colspan = 2;
                        cell.Padding = 5f;
                        cell.BorderColor = BaseColor.BLACK;
                        table.AddCell(cell);


                        cell = PhraseCell(new Phrase("Customer Name: " + dt_upper.Rows[0]["clientname"].ToString(), FontFactory.GetFont("Courier New", 6, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_CENTER);
                        cell.Colspan = 5;
                        cell.Padding = 5f;
                        cell.BorderColor = BaseColor.BLACK;
                        table.AddCell(cell);


                        cell = PhraseCell(new Phrase("City: " + dt_upper.Rows[0]["city"].ToString(), FontFactory.GetFont("Courier New", 6, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_CENTER);
                        cell.Colspan = 2;
                        cell.Padding = 5f;
                        cell.BorderColor = BaseColor.BLACK;
                        table.AddCell(cell);

                        cell = PhraseCell(new Phrase("Payment Period: " + dt_min.ToString("dd/MM/yyyy") + " To " + dt_max.ToString("dd/MM/yyyy"), FontFactory.GetFont("Courier New", 6, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_CENTER);
                        cell.Colspan = 3;
                        cell.Padding = 5f;
                        cell.BorderColor = BaseColor.BLACK;
                        table.AddCell(cell);



                    }

                    cell = PhraseCell(new Phrase(GridView1.HeaderRow.Cells[i].Text, FontFactory.GetFont("Courier New", 6, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_CENTER);
                    cell.Colspan = 1;
                    cell.Padding = 4f;
                    cell.BorderColor = BaseColor.BLACK;
                    cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    table.AddCell(cell);
                }
                int sr = 1;
                int z = 1;

                int totalPages = 0;

                totalPages = (GridView1.Rows.Count % 45 > 0) ? (GridView1.Rows.Count / 45) + 1 : (GridView1.Rows.Count / 45) + 0;

                foreach (GridViewRow row in GridView1.Rows)
                {
                    cell = PhraseCell(new Phrase(sr.ToString(), FontFactory.GetFont("Courier New", 6, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 1;
                    cell.BorderColor = BaseColor.BLACK;
                    cell.Padding = 5f;
                    //cell.Width = 10f;

                    table.AddCell(cell);
                    for (int i = 0; i < colcount; i++)
                    {
                        if (i == 0)
                        {

                        }
                        else
                        {
                            if (row.Cells[i].Text == "&nbsp;" || row.Cells[i].Text == null || row.Cells[i].Text == "" || String.IsNullOrWhiteSpace(row.Cells[i].Text))
                            {
                                cell = PhraseCell(new Phrase("", FontFactory.GetFont("Courier New", 6, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_CENTER);
                            }
                            else
                            {
                                if (i == 2)
                                {
                                    string refNo = (row.Cells[i].Text.Length >= 15) ? row.Cells[i].Text.Substring(0, 15) : row.Cells[i].Text.ToString();
                                    cell = PhraseCell(new Phrase(refNo, FontFactory.GetFont("Courier New", 6, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                                }

                                else if (i == 5)
                                {
                                    string name = row.Cells[i].Text;
                                    if (name.Length >= 10)
                                    {
                                        cell = PhraseCell(new Phrase(name.Substring(0, 10), FontFactory.GetFont("Courier New", 6, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                                    }
                                    else
                                    {
                                        cell = PhraseCell(new Phrase(row.Cells[i].Text, FontFactory.GetFont("Courier New", 6, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                                    }
                                }

                                else if (i == 10)
                                {
                                    double ammount = 0;
                                    double.TryParse(row.Cells[i].Text, out ammount);

                                    cell = PhraseCell(new Phrase(string.Format("{0:N2}", ammount), FontFactory.GetFont("Courier New", 6, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_RIGHT);
                                }

                                else if (i == 11)
                                {
                                    double ammount = 0;
                                    double.TryParse(row.Cells[i].Text, out ammount);

                                    cell = PhraseCell(new Phrase(string.Format("{0:N2}", ammount), FontFactory.GetFont("Courier New", 6, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_RIGHT);
                                }

                                else
                                {
                                    cell = PhraseCell(new Phrase(row.Cells[i].Text, FontFactory.GetFont("Courier New", 6, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                                }


                            }

                            cell.Colspan = 1;
                            cell.BorderColor = BaseColor.BLACK;
                            cell.Padding = 5f;
                            if (i == 2)
                            {
                                cell.PaddingLeft = 2f;
                                cell.PaddingRight = 2f;
                            }
                            else if (i == 3 || i == 4)
                            {
                                cell.PaddingLeft = 2f;
                                cell.PaddingRight = 2f;
                            }
                            else
                            {
                                cell.PaddingLeft = 5f;
                                cell.PaddingRight = 5f;
                            }

                            //cell.Width = 10f;

                            table.AddCell(cell);
                        }
                    }
                    z++;
                    sr++;
                    if (c == 1 && z == 46)
                    {
                        for (int a = 1; a < 25; a++)//z is row number
                        {
                            string pageNum;
                            if (a == 24) //a cell spaces that are left then pagenum will print curently 8cells to give one line gap its 16
                            {
                                pageNum = "Page No  " + c.ToString() + " of " + totalPages.ToString();
                            }
                            else
                            {
                                pageNum = "";
                            }
                            cell = PhraseCell(new Phrase(pageNum, FontFactory.GetFont("Courier New", 6, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_RIGHT);
                            cell.Colspan = 3;
                            cell.Padding = 5f;
                            table.AddCell(cell);

                        }

                        table.SpacingAfter = 1000f;
                        document.Add(table);
                        table = new PdfPTable(colcount);
                        table.TotalWidth = 550f;

                        table.SetWidths(new float[] { 25f, 60, 57f, 40f, 40f, 50f, 24f, 24, 30f, 65f, 50f, 40f });
                        table.LockedWidth = true;
                        //string pageNum;
                        //for (int a = 0; a < 5; a++)
                        //{
                        //    if (a == 4)
                        //    {
                        //        pageNum = "Page Num  " + c.ToString();
                        //    }
                        //    else
                        //    {
                        //        pageNum = "";
                        //    }
                        //    cell = PhraseCell(new Phrase(pageNum, FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_RIGHT);
                        //    cell.Colspan = 1;
                        //    table.AddCell(cell);
                        //}

                        for (int j = 0; j < colcount; j++)
                        {

                            if (j == 0)
                            {
                                cell = PhraseCell(new Phrase("Account Number: " + dt_upper.Rows[0]["accountNo"].ToString(), FontFactory.GetFont("Courier New", 6, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_CENTER);
                                cell.Colspan = 2;
                                cell.Padding = 5f;
                                cell.BorderColor = BaseColor.BLACK;
                                table.AddCell(cell);


                                cell = PhraseCell(new Phrase("Customer Name: " + dt_upper.Rows[0]["clientname"].ToString(), FontFactory.GetFont("Courier New", 6, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_CENTER);
                                cell.Colspan = 5;
                                cell.Padding = 5f;
                                cell.BorderColor = BaseColor.BLACK;
                                table.AddCell(cell);


                                cell = PhraseCell(new Phrase("City: " + dt_upper.Rows[0]["city"].ToString(), FontFactory.GetFont("Courier New", 6, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_CENTER);
                                cell.Colspan = 2;
                                cell.Padding = 5f;
                                cell.BorderColor = BaseColor.BLACK;
                                table.AddCell(cell);

                                cell = PhraseCell(new Phrase("Payment Period: " + dt_min.ToString("dd/MM/yyyy") + " To " + dt_max.ToString("dd/MM/yyyy"), FontFactory.GetFont("Courier New", 6, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_CENTER);
                                cell.Colspan = 3;
                                cell.Padding = 5f;
                                cell.BorderColor = BaseColor.BLACK;
                                table.AddCell(cell);



                            }

                            cell = PhraseCell(new Phrase(GridView1.HeaderRow.Cells[j].Text, FontFactory.GetFont("Courier New", 6, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_CENTER);
                            cell.Colspan = 1;
                            cell.Padding = 4f;
                            cell.BorderColor = BaseColor.BLACK;
                            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                            table.AddCell(cell);

                        }
                        z = 1;

                        c++;
                    }

                    else if (c > 1 && z == 46)
                    {
                        string pageNum;

                        for (int a = 1; a < 23; a++)//z is row number
                        {

                            if (a == 6)//a cell spaces that are left then pagenum will print
                            {
                                pageNum = "Page No  " + c.ToString() + " of " + totalPages.ToString();
                            }
                            else
                            {
                                pageNum = "";
                            }
                            cell = PhraseCell(new Phrase(pageNum, FontFactory.GetFont("Courier New", 6, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_RIGHT);
                            cell.Colspan = 2;
                            cell.Padding = 5f;
                            table.AddCell(cell);

                        }
                        table.SpacingAfter = 1000f;
                        document.Add(table);
                        table = new PdfPTable(colcount);
                        table.TotalWidth = 550f;

                        table.SetWidths(new float[] { 25f, 60, 57f, 40f, 40f, 50f, 24f, 24, 30f, 65f, 50f, 40f });
                        table.LockedWidth = true;
                        for (int j = 0; j < colcount; j++)
                        {
                            if (j == 0)
                            {
                                cell = PhraseCell(new Phrase("Account Number: " + dt_upper.Rows[0]["accountNo"].ToString(), FontFactory.GetFont("Courier New", 6, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_CENTER);
                                cell.Colspan = 2;
                                cell.Padding = 5f;
                                cell.BorderColor = BaseColor.BLACK;
                                table.AddCell(cell);


                                cell = PhraseCell(new Phrase("Customer Name: " + dt_upper.Rows[0]["clientname"].ToString(), FontFactory.GetFont("Courier New", 6, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_CENTER);
                                cell.Colspan = 5;
                                cell.Padding = 5f;
                                cell.BorderColor = BaseColor.BLACK;
                                table.AddCell(cell);


                                cell = PhraseCell(new Phrase("City: " + dt_upper.Rows[0]["city"].ToString(), FontFactory.GetFont("Courier New", 6, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_CENTER);
                                cell.Colspan = 2;
                                cell.Padding = 5f;
                                cell.BorderColor = BaseColor.BLACK;
                                table.AddCell(cell);

                                cell = PhraseCell(new Phrase("Payment Period: " + dt_min.ToString("dd/MM/yyyy") + " To " + dt_max.ToString("dd/MM/yyyy"), FontFactory.GetFont("Courier New", 6, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_CENTER);
                                cell.Colspan = 3;
                                cell.Padding = 5f;
                                cell.BorderColor = BaseColor.BLACK;
                                table.AddCell(cell);



                            }
                            cell = PhraseCell(new Phrase(GridView1.HeaderRow.Cells[j].Text, FontFactory.GetFont("Courier New", 6, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_CENTER);
                            cell.Colspan = 1;
                            cell.Padding = 4f;
                            cell.BorderColor = BaseColor.BLACK;
                            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                            table.AddCell(cell);


                        }
                        //cell = PhraseCell(c, FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_RIGHT);
                        // cell.Colspan = 1;
                        //cell.BorderColor = BaseColor.BLACK;

                        //table.AddCell(cell);

                        z = 1;
                        c++;
                    }
                }

                cell = PhraseCell(new Phrase("", FontFactory.GetFont("Courier New", 6, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_CENTER);
                cell.Colspan = 1;
                cell.Padding = 5f;
                cell.BorderColor = BaseColor.BLACK;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase("TOTALS:", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 9;
                cell.Padding = 5f;
                cell.BorderColor = BaseColor.BLACK;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(String.Format("{0:N2}", footerCodAmount), FontFactory.GetFont("Courier New", 7, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_RIGHT);
                cell.Colspan = 1;
                cell.Padding = 5f;
                cell.BorderColor = BaseColor.BLACK;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(String.Format("{0:N2}", footerShippingCharges), FontFactory.GetFont("Courier New", 7, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_RIGHT);
                cell.Colspan = 1;
                cell.Padding = 5f;
                cell.BorderColor = BaseColor.BLACK;
                table.AddCell(cell);



                table_pageNum = new PdfPTable(2);
                table_pageNum.TotalWidth = 550f;
                table_pageNum.LockedWidth = true;
                table_pageNum.SpacingBefore = 30f;
                table_pageNum.SetWidths(new float[] { 35f, 40f });


                for (int a = 1; a < 5; a++)
                {
                    string pageNum;
                    if (a == 4)
                    {
                        pageNum = "Page No  " + c.ToString() + " of " + totalPages.ToString();
                    }
                    else
                    {
                        pageNum = "";
                    }
                    cell = PhraseCell(new Phrase(pageNum, FontFactory.GetFont("Courier New", 6, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_RIGHT);
                    cell.Colspan = 1;
                    cell.Padding = 5f;
                    table_pageNum.AddCell(cell);
                }



                document.Add(table);

                document.Add(table_pageNum);


                document.Close();
                byte[] bytes = memoryStream.ToArray();
                memoryStream.Close();
                Response.Clear();
                Response.ContentType = "application/pdf";
                string fileName = "" + dt_upper.Rows[0]["clientname"].ToString() + "_" + txt_chequeNo.Text + "_" + DateTime.Now.ToShortDateString();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName + ".pdf");
                Response.ContentType = "application/pdf";
                Response.Buffer = true;
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.BinaryWrite(bytes);

                //Alert("Message Agya", "Red");
                //Btn_Search_Click(this, EventArgs.Empty);
                Response.End();
                Response.Close();
            }
        }


        #region MyRegion
        //private DataTable getUpperData()
        //{
        //    DataTable ds = new DataTable();

        //    try
        //    {


        //        string sqlString = "SELECT cc.accountNo,\n" +
        //        "       cc.name                 clientname,\n" +
        //        "       cc.[address],\n" +
        //        "       b3.name                 city,\n" +
        //        "       MIN(pv.VoucherDate)     MinPaymentPeriod,\n" +
        //        "       MAX(pv.VoucherDate)     MaxPaymentPeriod, ic.Invoicenumber\n" +
        //        "FROM   Consignment c\n" +
        //        "       INNER JOIN Branches b\n" +
        //        "            ON  b.branchCode = c.orgin\n" +
        //        "       INNER JOIN Branches b2\n" +
        //        "            ON  b2.branchCode = c.destination\n" +
        //        "       INNER JOIN CODConsignmentDetail_New cd\n" +
        //        "            ON  cd.consignmentNumber = c.consignmentNumber\n" +
        //        "       INNER JOIN CreditClients cc\n" +
        //        "            ON  c.creditClientId = cc.id\n" +
        //        "       INNER JOIN Branches b3\n" +
        //        "            ON  cc.branchCode = b3.branchCode\n" +
        //        "       INNER JOIN PaymentVouchers pv\n" +
        //        "            ON  pv.ConsignmentNo = c.consignmentNumber\n" +
        //        "       LEFT OUTER JOIN InvoiceConsignment ic\n" +
        //        "            ON  ic.consignmentNumber = c.consignmentNumber\n" +
        //        "       LEFT OUTER JOIN Invoice i\n" +
        //        "            ON  i.invoiceNumber = ic.invoiceNumber\n" +
        //        "            AND i.IsInvoiceCanceled = '0'\n" +
        //        "       LEFT OUTER JOIN RunsheetConsignment rc\n" +
        //        "            ON  rc.consignmentNumber = c.consignmentNumber\n" +
        //        "            AND rc.createdOn = (\n" +
        //        "                    SELECT MAX(RC2.createdOn) statusdate\n" +
        //        "                    FROM   RunsheetConsignment RC2\n" +
        //        "                    WHERE  RC2.consignmentNumber = rc.consignmentNumber\n" +
        //        "                )\n" +
        //        "       LEFT OUTER JOIN rvdbo.Lookup l\n" +
        //        "            ON  l.AttributeGroup = 'POD_STATUS'\n" +
        //        "            AND CAST(l.Id AS VARCHAR) = rc.[Status]\n" +
        //        "WHERE  pv.id in (" + clvar.Check_Condition1 + ") --c.transactionNumber = '999999'\n" +
        //        "       --AND c.consignerAccountNo = '4G7'\n" +
        //        "GROUP BY\n" +
        //        "       cc.accountNo,\n" +
        //        "       cc.name,\n" +
        //        "       cc.[address],\n" +
        //        "       b3.name,ic.invoicenumber";


        //        SqlConnection con = new SqlConnection(clvar.Strcon());
        //        SqlCommand orcd = new SqlCommand(sqlString, con);
        //        orcd.CommandType = CommandType.Text;
        //        SqlDataAdapter oda = new SqlDataAdapter(orcd);
        //        oda.Fill(ds);
        //        con.Close();
        //    }
        //    catch (Exception Err)
        //    { }
        //    finally
        //    { }
        //    return ds;
        //} 
        #endregion
        #region MyRegion
        //private DataTable getUpperData()
        //{
        //    DataTable ds = new DataTable();

        //    try
        //    {


        //        string sqlString = "SELECT cc.accountNo,\n" +
        //        "       cc.name                 clientname,\n" +
        //        "       cc.[address],\n" +
        //        "       b3.name                 city,\n" +
        //        "       MIN(pv.VoucherDate)     MinPaymentPeriod,\n" +
        //        "       MAX(pv.VoucherDate)     MaxPaymentPeriod\n" +
        //        "FROM   Consignment c\n" +
        //        "       INNER JOIN Branches b\n" +
        //        "            ON  b.branchCode = c.orgin\n" +
        //        "       INNER JOIN Branches b2\n" +
        //        "            ON  b2.branchCode = c.destination\n" +
        //        "       LEFT OUTER  JOIN CODConsignmentDetail_New cd\n" +
        //        "            ON  cd.consignmentNumber = c.consignmentNumber\n" +
        //        "       INNER JOIN CreditClients cc\n" +
        //        "            ON  c.creditClientId = cc.id\n" +
        //        "       INNER JOIN Branches b3\n" +
        //        "            ON  cc.branchCode = b3.branchCode\n" +
        //        "       INNER JOIN PaymentVouchers pv\n" +
        //        "            ON  pv.ConsignmentNo = c.consignmentNumber\n" +
        //        "       LEFT OUTER JOIN InvoiceConsignment ic\n" +
        //        "            ON  ic.consignmentNumber = c.consignmentNumber\n" +
        //        "       LEFT OUTER JOIN Invoice i\n" +
        //        "            ON  i.invoiceNumber = ic.invoiceNumber\n" +
        //        "            AND i.IsInvoiceCanceled = '0'\n" +
        //        "       LEFT OUTER JOIN RunsheetConsignment rc\n" +
        //        "            ON  rc.consignmentNumber = c.consignmentNumber\n" +
        //        "            AND rc.createdOn = (\n" +
        //        "                    SELECT MAX(RC2.createdOn) statusdate\n" +
        //        "                    FROM   RunsheetConsignment RC2\n" +
        //        "                    WHERE  RC2.consignmentNumber = rc.consignmentNumber\n" +
        //        "                )\n" +
        //        "       LEFT OUTER JOIN rvdbo.Lookup l\n" +
        //        "            ON  l.AttributeGroup = 'POD_STATUS'\n" +
        //        "            AND CAST(l.Id AS VARCHAR) = rc.[Status]\n" +
        //        "WHERE  pv.id in (" + clvar.Check_Condition1 + ") --c.transactionNumber = '999999'\n" +
        //        "       --AND c.consignerAccountNo = '4G7'\n" +
        //        "GROUP BY\n" +
        //        "       cc.accountNo,\n" +
        //        "       cc.name,\n" +
        //        "       cc.[address],\n" +
        //        "       b3.name";


        //        SqlConnection con = new SqlConnection(clvar.Strcon());
        //        SqlCommand orcd = new SqlCommand(sqlString, con);
        //        orcd.CommandType = CommandType.Text;
        //        SqlDataAdapter oda = new SqlDataAdapter(orcd);
        //        oda.Fill(ds);
        //        con.Close();
        //    }
        //    catch (Exception Err)
        //    { }
        //    finally
        //    { }
        //    return ds;
        //} 
        #endregion
        private DataTable getUpperData()
        {
            DataTable ds = new DataTable();

            try
            {


                string sqlString = "SELECT cc.accountNo,\n" +
                "       cc.name                 clientname,\n" +
                "       cc.[address],\n" +
                "       b3.name                 city,\n" +
                "       MIN(pv.VoucherDate)     MinPaymentPeriod,\n" +
                "       MAX(pv.VoucherDate)     MaxPaymentPeriod, ic.Invoicenumber\n" +
                "FROM   Consignment c\n" +
                "       INNER JOIN Branches b\n" +
                "            ON  b.branchCode = c.orgin\n" +
                "       INNER JOIN Branches b2\n" +
                "            ON  b2.branchCode = c.destination\n" +
                "       left outer JOIN CODConsignmentDetail_New cd\n" +
                "            ON  cd.consignmentNumber = c.consignmentNumber\n" +
                "       INNER JOIN CreditClients cc\n" +
                "            ON  c.creditClientId = cc.id\n" +
                "       INNER JOIN Branches b3\n" +
                "            ON  cc.branchCode = b3.branchCode\n" +
                "       INNER JOIN PaymentVouchers pv\n" +
                "            ON  pv.ConsignmentNo = c.consignmentNumber\n" +
                "       LEFT OUTER JOIN InvoiceConsignment ic\n" +
                "            ON  ic.consignmentNumber = c.consignmentNumber\n" +
                "       LEFT OUTER JOIN Invoice i\n" +
                "            ON  i.invoiceNumber = ic.invoiceNumber\n" +
                "            AND i.IsInvoiceCanceled = '0'\n" +
                "       LEFT OUTER JOIN RunsheetConsignment rc\n" +
                "            ON  rc.consignmentNumber = c.consignmentNumber\n" +
                "            AND rc.createdOn = (\n" +
                "                    SELECT MAX(RC2.createdOn) statusdate\n" +
                "                    FROM   RunsheetConsignment RC2\n" +
                "                    WHERE  RC2.consignmentNumber = rc.consignmentNumber\n" +
                "                )\n" +
                "       LEFT OUTER JOIN rvdbo.Lookup l\n" +
                "            ON  l.AttributeGroup = 'POD_STATUS'\n" +
                "            AND CAST(l.Id AS VARCHAR) = rc.[Status]\n" +
                "WHERE  pv.id in (" + clvar.Check_Condition1 + ") --c.transactionNumber = '999999'\n" +
                "       --AND c.consignerAccountNo = '4G7'\n" +
                "GROUP BY\n" +
                "       cc.accountNo,\n" +
                "       cc.name,\n" +
                "       cc.[address],\n" +
                "       b3.name,ic.invoicenumber";

                string sql = "SELECT DISTINCT cc.accountNo, \n"
               + "       cc.name clientname, \n"
               + "       b.name                  City, \n"
               + "       ir.InvoiceNo Invoicenumber, \n"
               + "       MIN(pv.VoucherDate)     MinPaymentPeriod, \n"
               + "       MAX(pv.VoucherDate)     MaxPaymentPeriod, cc.BeneficiaryName \n"
               + "FROM   PaymentVouchers pv \n"
               + "       LEFT OUTER JOIN InvoiceRedeem ir \n"
               + "            ON  ir.PaymentVoucherId = pv.Id \n"
               + "       LEFT OUTER JOIN Invoice i \n"
               + "            ON  i.invoiceNumber = ir.InvoiceNo \n"
               + "       INNER JOIN CreditClients cc \n"
               + "            ON  cc.id = pv.creditclientId \n"
               + "       INNER JOIN Branches b \n"
               + "            ON  b.branchCode = cc.branchCode \n"
               + "WHERE  pv.id in (" + clvar.Check_Condition1 + ") \n"
               + "GROUP BY \n"
               + "       cc.accountNo, \n"
               + "       cc.name, \n"
               + "       b.name, cc.BeneficiaryName,\n"
               + "       ir.InvoiceNo \n"
               + "";

                SqlConnection con = new SqlConnection(clvar.Strcon());
                SqlCommand orcd = new SqlCommand(sql, con);
                orcd.CommandTimeout = 30000;
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                con.Close();
            }
            catch (Exception Err)
            { }
            finally
            { }
            return ds;
        }



        private DataSet getData(string invoiceNumbers)
        {
            DataSet ds = new DataSet();

            try
            {

                string sqlString = "SELECT ROW_NUMBER() OVER(order by b.consignmentNumber) Sr, b.consignmentNumber,\n" +
                "       b.orderRefNo,\n" +
                "       convert(varchar(11),b.bookingDate,106) bookingDate,\n" +
                "       b.consignee,\n" +
                "       b.origin,\n" +
                "       b.[weight],\n" +
                "       b.destination,\n" +
                "       SUM(b.CODAMOUNT)           CODAMOUNT,\n" +
                "       b.CNStatus,\n" +
                "       b.transactionNumber        transactionNumber,\n" +
                "       SUM(b.ShippingCharges)     ShippingCharges, convert(varchar(11),b.RRDATE,106) RRDATE\n" +
                "FROM   (\n" +
                "           SELECT c.consignmentNumber,\n" +
                "                  cd.orderRefNo,\n" +
                "                  c.bookingDate,\n" +
                "                  c.consignee,\n" +
                "                  b.sname                  origin,\n" +
                "                  b2.sname                  destination,\n" +
                "                  c.[weight],\n" +
                "                  --SUM(cd.codAmount) / COUNT(cd.consignmentNumber) CODAMOUNT,\n" +
                "                  --cd.codAmount,\n" +
                "                  pv.Amount                CODAMOUNT,\n" +
                "                  l.AttributeValue         CNStatus,\n" +
                "                  ic.consignmentAmount     ShippingCharges,\n" +
                "                  transactionNumber, pv.VoucherDate RRDATE\n" +
                "           FROM   Consignment c\n" +
                "                  INNER JOIN Branches b\n" +
                "                       ON  b.branchCode = c.orgin\n" +
                "                  INNER JOIN Branches b2\n" +
                "                       ON  b2.branchCode = c.destination\n" +
                "                  INNER JOIN CODConsignmentDetail_New cd\n" +
                "                       ON  cd.consignmentNumber = c.consignmentNumber\n" +
                "                  INNER JOIN PaymentVouchers pv\n" +
                "                       ON  pv.ConsignmentNo = c.consignmentNumber\n" +
                "                  LEFT OUTER JOIN InvoiceConsignment ic\n" +
                "                       ON  ic.consignmentNumber = c.consignmentNumber\n" +
                "                  LEFT OUTER JOIN Invoice i\n" +
                "                       ON  i.invoiceNumber = ic.invoiceNumber\n" +
                "                       AND i.IsInvoiceCanceled = '0'\n" +
                "                  LEFT OUTER JOIN RunsheetConsignment rc\n" +
                "                       ON  rc.consignmentNumber = c.consignmentNumber\n" +
                "                       AND rc.createdOn = (\n" +
                "                               SELECT MAX(RC2.createdOn) statusdate\n" +
                "                               FROM   RunsheetConsignment RC2\n" +
                "                               WHERE  RC2.consignmentNumber = rc.consignmentNumber\n" +
                "                           )\n" +
                "                  LEFT OUTER JOIN rvdbo.Lookup l\n" +
                "                       ON  l.AttributeGroup = 'POD_STATUS'\n" +
                "                       AND CAST(l.Id AS VARCHAR) = rc.[Status]\n" +
                "   WHERE pv.id in (" + clvar.Check_Condition1 + ") --c.transactionNumber = '999999'\n" +
                "      --and c.consignerAccountNo = '4G7'\n" +
                "           GROUP BY\n" +
                "                  c.consignmentNumber,\n" +
                "                  cd.orderRefNo,\n" +
                "                  c.bookingDate,\n" +
                "                  c.consignee,\n" +
                "                  b.sname,\n" +
                "                  b2.sname,\n" +
                "                  c.[weight],\n" +
                "                  pv.Amount,\n" +
                "                  l.AttributeValue,\n" +
                "                  ic.consignmentAmount,\n" +
                "                  transactionNumber, pv.VoucherDate\n" +
                "       )                          b\n" +
                "GROUP BY\n" +
                "       b.consignmentNumber,\n" +
                "       b.orderRefNo,\n" +
                "       convert(varchar(11),b.bookingDate,106),\n" +
                "       b.consignee,\n" +
                "       b.origin,\n" +
                "       b.destination,\n" +
                "       b.CNStatus,\n" +
                "       b.[weight],\n" +
                "       b.transactionNumber, b.RRDATE";

                #region MyRegion
                // string sql = "SELECT ROW_NUMBER() OVER(order by b.consignmentNumber) Sr, b.consignmentNumber, \n"
                //+ "       b.orderRefNo, \n"
                //+ "       convert(varchar(11),b.bookingDate,106) bookingDate, \n"
                //+ "       b.consignee, \n"
                //+ "       b.origin, \n"
                //+ "       b.[weight], \n"
                //+ "       b.destination, \n"
                //+ "       SUM(b.CODAMOUNT)           CODAMOUNT, \n"
                //+ "       --b.CNStatus, \n"
                //+ "       --b.CNReason, \n"
                //+ "       CASE WHEN b.cnReasonvalue IN ('58','59','123') THEN dbo.splitstring(b.CNReason,'-',2) ELSE 'INPROCESS' END CNSTATUS, \n"
                //+ "       b.cnReasonvalue, \n"
                //+ "        \n"
                //+ "       b.transactionNumber        transactionNumber, \n"
                //+ "       SUM(b.ShippingCharges)     ShippingCharges \n"
                //+ "FROM   ( \n"
                //+ "           SELECT c.consignmentNumber, \n"
                //+ "                  cd.orderRefNo, \n"
                //+ "                  c.bookingDate, \n"
                //+ "                  c.consignee, \n"
                //+ "                  b.sname                  origin, \n"
                //+ "                  b2.sname                  destination, \n"
                //+ "                  c.[weight], \n"
                //+ "                  --SUM(cd.codAmount) / COUNT(cd.consignmentNumber) CODAMOUNT, \n"
                //+ "                  --cd.codAmount, \n"
                //+ "                  pv.Amount                CODAMOUNT, \n"
                //+ "                  l2.AttributeValue         CNStatus, \n"
                //+ "                  l.AttributeValue CNReason, \n"
                //+ "                  l.id cnReasonvalue, \n"
                //+ "                  ic.consignmentAmount     ShippingCharges, \n"
                //+ "                  transactionNumber \n"
                //+ "           FROM   Consignment c \n"
                //+ "                  INNER JOIN Branches b \n"
                //+ "                       ON  b.branchCode = c.orgin \n"
                //+ "                  INNER JOIN Branches b2 \n"
                //+ "                       ON  b2.branchCode = c.destination \n"
                //+ "                  INNER JOIN CODConsignmentDetail_New cd \n"
                //+ "                       ON  cd.consignmentNumber = c.consignmentNumber \n"
                //+ "                  INNER JOIN PaymentVouchers pv \n"
                //+ "                       ON  pv.ConsignmentNo = c.consignmentNumber \n"
                //+ "                  LEFT OUTER JOIN InvoiceConsignment ic \n"
                //+ "                       ON  ic.consignmentNumber = c.consignmentNumber \n"
                //+ "                  LEFT OUTER JOIN Invoice i \n"
                //+ "                       ON  i.invoiceNumber = ic.invoiceNumber \n"
                //+ "                       AND i.IsInvoiceCanceled = '0' \n"
                //+ "                  LEFT OUTER JOIN RunsheetConsignment rc \n"
                //+ "                       ON  rc.consignmentNumber = c.consignmentNumber \n"
                //+ "                       AND rc.createdOn = ( \n"
                //+ "                               SELECT MAX(RC2.createdOn) statusdate \n"
                //+ "                               FROM   RunsheetConsignment RC2 \n"
                //+ "                               WHERE  RC2.consignmentNumber = rc.consignmentNumber \n"
                //+ "                           ) \n"
                //+ "                  LEFT OUTER JOIN rvdbo.Lookup l \n"
                //+ "                       ON  l.AttributeGroup = 'POD_REASON' \n"
                //+ "                       AND CAST(l.Id AS VARCHAR) = rc.[reason] \n"
                //+ "                  LEFT OUTER JOIN rvdbo.Lookup l2 \n"
                //+ "                  ON l2.AttributeGroup = 'POD_STATUS' \n"
                //+ "                  AND l2.AttributeValue = l.AttributeDesc \n"
                //+ "   WHERE pv.id in (" + clvar.Check_Condition1 + ") --c.transactionNumber = '999999' \n"
                //+ "      --and c.consignerAccountNo = '4G7' \n"
                //+ "           GROUP BY \n"
                //+ "                  c.consignmentNumber, \n"
                //+ "                  cd.orderRefNo, \n"
                //+ "                  c.bookingDate, \n"
                //+ "                  c.consignee, \n"
                //+ "                  b.sname, \n"
                //+ "                  b2.sname, \n"
                //+ "                  c.[weight], \n"
                //+ "                  pv.Amount, \n"
                //+ "                  l.AttributeValue, \n"
                //+ "                  ic.consignmentAmount, \n"
                //+ "                  transactionNumber, \n"
                //+ "                  l2.Attributevalue,l.id  \n"
                //+ "       )                          b \n"
                //+ "GROUP BY \n"
                //+ "       b.consignmentNumber, \n"
                //+ "       b.orderRefNo, \n"
                //+ "       convert(varchar(11),b.bookingDate,106), \n"
                //+ "       b.consignee, \n"
                //+ "       b.origin, \n"
                //+ "       b.destination, \n"
                //+ "       b.CNStatus, \n"
                //+ "       b.[weight], \n"
                //+ "       b.transactionNumber, \n"
                //+ "       b.cnReason, \n"
                //+ "       b.cnReasonvalue"; 
                #endregion
                string sql = "SELECT ROW_NUMBER() OVER(ORDER BY b.consignmentNumber) Sr, \n"
               + "       b.consignmentNumber, \n"
               + "       b.orderRefNo, \n"
               + "       CONVERT(VARCHAR(11), b.bookingDate, 106) bookingDate, \n"
               + "       b.consignee, \n"
               + "       b.origin, \n"
               + "       b.[weight], \n"
               + "       b.destination, \n"
               + "       SUM(b.CODAMOUNT)           CODAMOUNT, \n"
               + "       b.CNStatus, \n"
               + "       b.transactionNumber        transactionNumber, \n"
               + "       SUM(b.ShippingCharges)     ShippingCharges, CASE WHEN p.VoucherDate is null then '' else convert(varchar(11),p.VoucherDate,106) end RRDATE --, CASE WHEN b.RRDATE is not null then convert(varchar(11),b.RRDATE,106) else '' end RRDATE \n"
               + "FROM   ( \n"
               + "           SELECT c.consignmentNumber, \n"
               + "                  cd.orderRefNo, \n"
               + "                  c.bookingDate, \n"
               + "                  c.consignee, \n"
               + "                  b.sname              origin, \n"
               + "                  b2.sname             destination, \n"
               + "                  c.[weight], \n"
               + "                  --SUM(cd.codAmount) / COUNT(cd.consignmentNumber) CODAMOUNT, \n"
               + "                  --cd.codAmount, \n"
               + "                  pv.Amount            CODAMOUNT, \n"
               + "                  l.AttributeValue     CNStatus, \n"
               + "                  0                    ShippingCharges,\n"
               + "                  transactionNumber \n"
               + "           FROM   Consignment c \n"
               + "                  INNER JOIN Branches b \n"
               + "                       ON  b.branchCode = c.orgin \n"
               + "                  INNER JOIN Branches b2 \n"
               + "                       ON  b2.branchCode = c.destination \n"
               + "                  LEFT OUTER JOIN CODConsignmentDetail_New cd \n"
               + "                       ON  cd.consignmentNumber = c.consignmentNumber \n"
               + "                  INNER JOIN PaymentVouchers pv \n"
               + "                       ON  pv.ConsignmentNo = c.consignmentNumber \n"
               + "                  LEFT OUTER JOIN RunsheetConsignment rc \n"
               + "                       ON  rc.consignmentNumber = c.consignmentNumber \n"
               + "                       AND rc.createdOn = ( \n"
               + "                               SELECT MAX(RC2.createdOn) statusdate \n"
               + "                               FROM   RunsheetConsignment RC2 \n"
               + "                               WHERE  RC2.consignmentNumber = rc.consignmentNumber \n"
               + "                           ) \n"
               + "                  LEFT OUTER JOIN rvdbo.Lookup l \n"
               + "                       ON  l.AttributeGroup = 'POD_STATUS' \n"
               + "                       AND CAST(l.Id AS VARCHAR) = rc.[Status] \n"
               + "           WHERE pv.id in (" + clvar.Check_Condition1 + ") --c.transactionNumber = '999999' \n"
               + "                                                                   --and c.consignerAccountNo = '4G7' \n"
               + "           GROUP BY \n"
               + "                  c.consignmentNumber, \n"
               + "                  cd.orderRefNo, \n"
               + "                  c.bookingDate, \n"
               + "                  c.consignee, \n"
               + "                  b.sname, \n"
               + "                  b2.sname, \n"
               + "                  c.[weight], \n"
               + "                  pv.Amount, \n"
               + "                  l.AttributeValue, \n"
               + "                  transactionNumber  \n"
               + "            \n"
               + "            \n"
               + "           UNION ALL \n"
               + "           SELECT c.consignmentNumber, \n"
               + "                  cd.orderRefNo, \n"
               + "                  c.bookingDate, \n"
               + "                  c.consignee, \n"
               + "                  b.sname              origin, \n"
               + "                  b2.sname             destination, \n"
               + "                  c.[weight], \n"
               + "                  --SUM(cd.codAmount) / COUNT(cd.consignmentNumber) CODAMOUNT, \n"
               + "                  --cd.codAmount, \n"
               + "                  0                    CODAMOUNT, \n"
               + "                  l.AttributeValue     CNStatus, \n"
               + "                  c.totalAmount        ShippingCharges,\n"
               + "                  transactionNumber \n"
               + "           FROM   Consignment c \n"
               + "                  INNER JOIN Branches b \n"
               + "                       ON  b.branchCode = c.orgin \n"
               + "                  INNER JOIN Branches b2 \n"
               + "                       ON  b2.branchCode = c.destination \n"
               + "                  LEFT OUTER JOIN CODConsignmentDetail_New cd \n"
               + "                       ON  cd.consignmentNumber = c.consignmentNumber \n"
               + "                  INNER JOIN InvoiceConsignment ic \n"
               + "                       ON  ic.consignmentNumber = c.consignmentNumber \n"
               + "                  LEFT OUTER JOIN RunsheetConsignment rc \n"
               + "                       ON  rc.consignmentNumber = c.consignmentNumber \n"
               + "                       AND rc.createdOn = ( \n"
               + "                               SELECT MAX(RC2.createdOn) statusdate \n"
               + "                               FROM   RunsheetConsignment RC2 \n"
               + "                               WHERE  RC2.consignmentNumber = rc.consignmentNumber \n"
               + "                           ) \n"
               + "                  LEFT OUTER JOIN rvdbo.Lookup l \n"
               + "                       ON  l.AttributeGroup = 'POD_STATUS' \n"
               + "                       AND CAST(l.Id AS VARCHAR) = rc.[Status] \n"
               + "           WHERE  ic.invoiceNumber IN (" + invoiceNumbers + ") \n"
               + "           GROUP BY \n"
               + "                  c.consignmentNumber, \n"
               + "                  cd.orderRefNo, \n"
               + "                  c.bookingDate, \n"
               + "                  c.consignee, \n"
               + "                  b.sname, \n"
               + "                  b2.sname, \n"
               + "                  c.[weight], \n"
               + "                  l.AttributeValue, \n"
               + "                  c.totalAmount, \n"
               + "                  transactionNumber  \n"
               + "       )                          b left outer join paymentvouchers p on p.consignmentNo = b.consignmentNumber \n"
               + "GROUP BY \n"
               + "       b.consignmentNumber, \n"
               + "       b.orderRefNo, \n"
               + "       CONVERT(VARCHAR(11), b.bookingDate, 106), \n"
               + "       b.consignee, \n"
               + "       b.origin, \n"
               + "       b.destination, \n"
               + "       b.CNStatus, \n"
               + "       b.[weight], \n"
               + "       b.transactionNumber, p.VoucherDate";
                SqlConnection con = new SqlConnection(clvar.Strcon());
                SqlCommand orcd = new SqlCommand(sql, con);
                orcd.CommandTimeout = 30000;
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                con.Close();
            }
            catch (Exception Err)
            { }
            finally
            { }
            return ds;
        }
        private DataTable GetRTSData()
        {
            DataTable dt = new DataTable();
            try
            {

                string sqlString = "SELECT ROW_NUMBER() OVER(order by b.consignmentNumber) Sr, b.consignmentNumber,\n" +
                "       b.orderRefNo,\n" +
                "       convert(varchar(11),b.bookingDate,106) bookingDate,\n" +
                "       b.consignee,\n" +
                "       b.origin,\n" +
                "       b.[weight],\n" +
                "       b.destination,\n" +
                "       SUM(b.CODAMOUNT)           CODAMOUNT,\n" +
                "       b.CNStatus,\n" +
                "       b.transactionNumber        transactionNumber,\n" +
                "       SUM(b.ShippingCharges)     ShippingCharges\n" +
                "FROM   (\n" +
                "           SELECT c.consignmentNumber,\n" +
                "                  cd.orderRefNo,\n" +
                "                  c.bookingDate,\n" +
                "                  c.consignee,\n" +
                "                  b.sname                  origin,\n" +
                "                  b2.sname                  destination,\n" +
                "                  c.[weight],\n" +
                "                  --SUM(cd.codAmount) / COUNT(cd.consignmentNumber) CODAMOUNT,\n" +
                "                  --cd.codAmount,\n" +
                "                  pv.Amount                CODAMOUNT,\n" +
                "                  l.AttributeValue         CNStatus,\n" +
                "                  ic.consignmentAmount     ShippingCharges,\n" +
                "                  transactionNumber\n" +
                "           FROM   Consignment c\n" +
                "                  INNER JOIN Branches b\n" +
                "                       ON  b.branchCode = c.orgin\n" +
                "                  INNER JOIN Branches b2\n" +
                "                       ON  b2.branchCode = c.destination\n" +
                "                  INNER JOIN CODConsignmentDetail_New cd\n" +
                "                       ON  cd.consignmentNumber = c.consignmentNumber\n" +
                "                  INNER JOIN PaymentVouchers pv\n" +
                "                       ON  pv.ConsignmentNo = c.consignmentNumber\n" +
                "                  LEFT OUTER JOIN InvoiceConsignment ic\n" +
                "                       ON  ic.consignmentNumber = c.consignmentNumber\n" +
                "                  LEFT OUTER JOIN Invoice i\n" +
                "                       ON  i.invoiceNumber = ic.invoiceNumber\n" +
                "                       AND i.IsInvoiceCanceled = '0'\n" +
                "                  LEFT OUTER JOIN RunsheetConsignment rc\n" +
                "                       ON  rc.consignmentNumber = c.consignmentNumber\n" +
                "                       AND rc.createdOn = (\n" +
                "                               SELECT MAX(RC2.createdOn) statusdate\n" +
                "                               FROM   RunsheetConsignment RC2\n" +
                "                               WHERE  RC2.consignmentNumber = rc.consignmentNumber\n" +
                "                           )\n" +
                "                  LEFT OUTER JOIN rvdbo.Lookup l\n" +
                "                       ON  l.AttributeGroup = 'POD_STATUS'\n" +
                "                       AND CAST(l.Id AS VARCHAR) = rc.[Status]\n" +
                "   WHERE pv.id in (" + clvar.Check_Condition1 + ") --c.transactionNumber = '999999'\n" +
                "      --and c.consignerAccountNo = '4G7'\n" +
                "           GROUP BY\n" +
                "                  c.consignmentNumber,\n" +
                "                  cd.orderRefNo,\n" +
                "                  c.bookingDate,\n" +
                "                  c.consignee,\n" +
                "                  b.sname,\n" +
                "                  b2.sname,\n" +
                "                  c.[weight],\n" +
                "                  pv.Amount,\n" +
                "                  l.AttributeValue,\n" +
                "                  ic.consignmentAmount,\n" +
                "                  transactionNumber\n" +
                "       )                          b\n" +
                "GROUP BY\n" +
                "       b.consignmentNumber,\n" +
                "       b.orderRefNo,\n" +
                "       convert(varchar(11),b.bookingDate,106),\n" +
                "       b.consignee,\n" +
                "       b.origin,\n" +
                "       b.destination,\n" +
                "       b.CNStatus,\n" +
                "       b.[weight],\n" +
                "       b.transactionNumber";

                string sql = "SELECT ISNULL(SUM(ISNULL(cdn.codAmount, 0)),0) RTSAmount FROM CODConsignmentDetail_New cdn \n"
               + "       INNER JOIN PaymentVouchers pv \n"
               + "       ON pv.ConsignmentNo = cdn.consignmentNumber \n"
               + "       INNER JOIN RunsheetConsignment rc \n"
               + "       ON rc.consignmentNumber = cdn.consignmentNumber \n"
               + "       AND rc.createdOn = ( \n"
               + "                               SELECT MAX(RC2.createdOn) statusdate \n"
               + "                               FROM   RunsheetConsignment RC2 \n"
               + "                               WHERE  RC2.consignmentNumber = rc.consignmentNumber \n"
               + "       ) \n"
               + "       WHERE rc.Reason = '59' \n"
               + "       AND pv.id in (" + clvar.Check_Condition1 + ")";

                SqlConnection con = new SqlConnection(clvar.Strcon());
                SqlCommand orcd = new SqlCommand(sql, con);
                orcd.CommandTimeout = 30000;
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dt);
                con.Close();
            }
            catch (Exception Err)
            { }
            finally
            { }

            return dt;
        }
        #region COMMENTED getData
        //private DataSet getData()
        //{
        //    DataSet ds = new DataSet();

        //    try
        //    {

        //        string sqlString = "SELECT ROW_NUMBER() OVER(Order by c.consignmentNumber) SR, c.consignmentNumber,\n" +
        //        "       cd.orderRefNo,\n" +
        //        "       convert(varchar(11),c.bookingDate,106) bookingDate,\n" +
        //        "       c.consignee,\n" +
        //        "       b.sname                  origin,\n" +
        //        "       b2.sname                  destination,\n" +
        //        "       c.[weight],\n" +
        //        "       --SUM(cd.codAmount) / COUNT(cd.consignmentNumber) CODAMOUNT,\n" +
        //        "       --cd.codAmount,\n" +
        //        "       pv.Amount CODAMOUNT,\n" +
        //        "       l.AttributeValue         CNStatus,\n" +
        //        "       ic.consignmentAmount     ShippingCharges,\n" +
        //        "       transactionNumber\n" +
        //        "FROM   Consignment c\n" +
        //        "       INNER JOIN Branches b\n" +
        //        "            ON  b.branchCode = c.orgin\n" +
        //        "       INNER JOIN Branches b2\n" +
        //        "            ON  b2.branchCode = c.destination\n" +
        //        "       left outer JOIN CODConsignmentDetail_New cd\n" +
        //        "            ON  cd.consignmentNumber = c.consignmentNumber\n" +
        //        "       INNER JOIN PaymentVouchers pv\n" +
        //        "            ON  pv.ConsignmentNo = c.consignmentNumber\n" +
        //        "       LEFT OUTER JOIN InvoiceConsignment ic\n" +
        //        "            ON  ic.consignmentNumber = c.consignmentNumber\n" +
        //        "       LEFT OUTER JOIN Invoice i\n" +
        //        "            ON  i.invoiceNumber = ic.invoiceNumber\n" +
        //        "            AND i.IsInvoiceCanceled = '0'\n" +
        //        "       LEFT OUTER JOIN RunsheetConsignment rc\n" +
        //        "            ON  rc.consignmentNumber = c.consignmentNumber\n" +
        //        "            AND rc.createdOn = (\n" +
        //        "                    SELECT MAX(RC2.createdOn) statusdate\n" +
        //        "                    FROM   RunsheetConsignment RC2\n" +
        //        "                    WHERE  RC2.consignmentNumber = rc.consignmentNumber\n" +
        //        "                )\n" +
        //        "       LEFT OUTER JOIN rvdbo.Lookup l\n" +
        //        "            ON  l.AttributeGroup = 'POD_STATUS'\n" +
        //        "            AND CAST(l.Id AS VARCHAR) = rc.[Status]\n" +
        //        "WHERE pv.id in (" + clvar.Check_Condition1 + ") --c.transactionNumber = '999999'\n" +
        //        "      --and c.consignerAccountNo = '4G7'\n" +
        //        "GROUP BY\n" +
        //        "       c.consignmentNumber,\n" +
        //        "       cd.orderRefNo,\n" +
        //        "       c.bookingDate,\n" +
        //        "       c.consignee,\n" +
        //        "       b.sname,\n" +
        //        "       b2.sname,\n" +
        //        "       c.[weight],\n" +
        //        "       pv.Amount,\n" +
        //        "       --cd.codAmount,\n" +
        //        "       l.AttributeValue,\n" +
        //        "       ic.consignmentAmount\n" +
        //        ",transactionNumber";

        //        SqlConnection con = new SqlConnection(clvar.Strcon());
        //        SqlCommand orcd = new SqlCommand(sqlString, con);
        //        orcd.CommandType = CommandType.Text;
        //        SqlDataAdapter oda = new SqlDataAdapter(orcd);
        //        oda.Fill(ds);
        //        con.Close();
        //    }
        //    catch (Exception Err)
        //    { }
        //    finally
        //    { }
        //    return ds;
        //} 
        #endregion

        /// </summary>
        /// <param name="sender"></param>


        private static PdfPCell PhraseCell(Phrase phrase, int align)
        {
            PdfPCell cell = new PdfPCell(phrase);
            cell.BorderColor = iTextSharp.text.BaseColor.WHITE;
            cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
            cell.HorizontalAlignment = align;
            cell.PaddingBottom = 2f;
            cell.PaddingTop = 0f;
            return cell;
        }

        #region MyRegion
        //private DataTable CenterTableRight_Data(string invoiceNumbers)
        //{
        //    DataTable ds = new DataTable();

        //    try
        //    {


        //        string sqlString = "\n" +
        //        "SELECT SUM(shipmentCharge) shipmentCharge,\n" +
        //        "SUM(FUELCHARGE) FUEL_CHARGE,\n" +
        //        "SUM(EXTRACHARGES) EXTRACHARGES,\n" +
        //        "SUM(EXTRACHARGESGST) + SUM(FUELCHARGEGST) GST\n" +
        //        "\n" +
        //        "FROM\n" +
        //        "(\n" +
        //        "SELECT sum(c.totalAmount) shipmentCharge,\n" +
        //        "\n" +
        //        "(SELECT ISNULL(SUM(IPM.CALCULATEDAMOUNT), 0)\n" +
        //        "   FROM INVOICEPRICEMODIFIERASSOCIATION IPM\n" +
        //        "  WHERE IPM.INVOICENO = ic.INVOICENUMBER\n" +
        //        "  AND IPM.MODIFIERTYPE = '1'\n" +
        //        ") FUELCHARGE,\n" +
        //        "\n" +
        //        "(SELECT ISNULL(SUM(IPM.CALCULATEDGST), 0)\n" +
        //        "   FROM INVOICEPRICEMODIFIERASSOCIATION IPM\n" +
        //        "  WHERE IPM.INVOICENO = ic.INVOICENUMBER\n" +
        //        "  AND IPM.MODIFIERTYPE = '1'\n" +
        //        ") FUELCHARGEGST,\n" +
        //        "\n" +
        //        "(SELECT ISNULL(SUM(CM.CALCULATEDVALUE), 0)\n" +
        //        "   FROM CONSIGNMENTMODIFIER AS CM\n" +
        //        "  WHERE CM.CONSIGNMENTNUMBER = C.CONSIGNMENTNUMBER) EXTRACHARGES,\n" +
        //        "(SELECT ISNULL(SUM(CM.CALCULATEDGST), 0)\n" +
        //        "   FROM CONSIGNMENTMODIFIER AS CM\n" +
        //        "  WHERE CM.CONSIGNMENTNUMBER = C.CONSIGNMENTNUMBER) EXTRACHARGESGST\n" +
        //        "\n" +
        //        "\n" +
        //        "FROM   Consignment c\n" +
        //        "       INNER JOIN Branches b\n" +
        //        "            ON  b.branchCode = c.orgin\n" +
        //        "       INNER JOIN Branches b2\n" +
        //        "            ON  b2.branchCode = c.destination\n" +
        //        "       INNER JOIN CODConsignmentDetail_New cd\n" +
        //        "            ON  cd.consignmentNumber = c.consignmentNumber\n" +
        //        "       INNER JOIN CreditClients cc\n" +
        //        "            ON  c.creditClientId = cc.id\n" +
        //        "       INNER JOIN Branches b3\n" +
        //        "            ON  cc.branchCode = b3.branchCode\n" +
        //        "       INNER JOIN PaymentVouchers pv\n" +
        //        "            ON  pv.ConsignmentNo = c.consignmentNumber\n" +
        //        "       LEFT OUTER JOIN InvoiceConsignment ic\n" +
        //        "            ON  ic.consignmentNumber = c.consignmentNumber\n" +
        //        "       LEFT OUTER JOIN Invoice i\n" +
        //        "            ON  i.invoiceNumber = ic.invoiceNumber\n" +
        //        "            AND i.IsInvoiceCanceled = '0'\n" +
        //        "       LEFT OUTER JOIN RunsheetConsignment rc\n" +
        //        "            ON  rc.consignmentNumber = c.consignmentNumber\n" +
        //        "            AND rc.createdOn = (\n" +
        //        "                    SELECT MAX(RC2.createdOn) statusdate\n" +
        //        "                    FROM   RunsheetConsignment RC2\n" +
        //        "                    WHERE  RC2.consignmentNumber = rc.consignmentNumber\n" +
        //        "                )\n" +
        //        "       LEFT OUTER JOIN rvdbo.Lookup l\n" +
        //        "            ON  l.AttributeGroup = 'POD_STATUS'\n" +
        //        "            AND CAST(l.Id AS VARCHAR) = rc.[Status]\n" +
        //        "WHERE  pv.id in (" + clvar.Check_Condition1 + ") --c.transactionNumber = '00000162'\n" +
        //        "       --AND c.consignerAccountNo = '4a260'\n" +
        //        "GROUP BY\n" +
        //        "ic.INVOICENUMBER, C.CONSIGNMENTNUMBER\n" +
        //        ") b";

        //       string sql = "SELECT ISNULL(SUM(A.ShippingCharges), 0)     shipmentCharge, \n"
        //       + "       ISNULL(SUM(A.GST), 0)                 GST, \n"
        //       + "       ISNULL(SUM(A.FuelSurcharge), 0)       FUEL_CHARGE, \n"
        //       + "       ISNULL(SUM(A.ExtraCharges), 0)        EXTRACHARGES \n"
        //       + "FROM   ( \n"
        //       + "           SELECT ROUND(SUM(ISNULL(c.totalAmount, 0)), 2) ShippingCharges, \n"
        //       + "                  ROUND(SUM(ISNULL(c.gst, 0)), 2) GST, \n"
        //       + "                  0     FuelSurcharge, \n"
        //       + "                  0     ExtraCharges \n"
        //       + "           FROM   Invoice i \n"
        //       + "                  INNER JOIN InvoiceConsignment ic \n"
        //       + "                       ON  ic.invoiceNumber = i.invoiceNumber \n"
        //       + "                  INNER JOIN Consignment c \n"
        //       + "                       ON  c.consignmentNumber = ic.consignmentNumber \n"
        //       + "                  LEFT OUTER JOIN InvoicePriceModifierAssociation ip \n"
        //       + "                       ON  ip.InvoiceNo = i.invoiceNumber \n"
        //       + "                       AND ip.modifierType = '1' \n"
        //       + "           WHERE  i.invoiceNumber in (" + invoiceNumbers + ") \n"
        //       + "            \n"
        //       + "           UNION  \n"
        //       + "            \n"
        //       + "           SELECT 0     ShippingCharges, \n"
        //       + "                  ROUND(SUM(ISNULL(ip.calculatedGST, 0)), 2) GST, \n"
        //       + "                  ROUND(SUM(ISNULL(ip.calculatedAmount, 0)), 2) FuelSurcharge, \n"
        //       + "                  0     ExtraCharges \n"
        //       + "           FROM   InvoicePriceModifierAssociation ip \n"
        //       + "           WHERE  ip.InvoiceNo in (" + invoiceNumbers + ") \n"
        //       + "                  AND ip.modifierType = '1' \n"
        //       + "            \n"
        //       + "           UNION \n"
        //       + "            \n"
        //       + "           SELECT 0     ShippingCharges, \n"
        //       + "                  ROUND(SUM(ISNULL(cm.calculatedGST, 0)), 2) GST, \n"
        //       + "                  0     FuelSurcharge, \n"
        //       + "                  ROUND(SUM(ISNULL(cm.calculatedValue, 0)), 2) ExtraCharges \n"
        //       + "           FROM   ConsignmentModifier cm \n"
        //       + "                  INNER JOIN InvoiceConsignment ic \n"
        //       + "                       ON  ic.consignmentNumber = cm.consignmentNumber \n"
        //       + "           WHERE  ic.invoiceNumber in (" + invoiceNumbers + ") \n"
        //       + "       )                          A \n"
        //       + "";
        //        SqlConnection con = new SqlConnection(clvar.Strcon());
        //        SqlCommand orcd = new SqlCommand(sqlString, con);
        //        orcd.CommandType = CommandType.Text;
        //        SqlDataAdapter oda = new SqlDataAdapter(orcd);
        //        oda.Fill(ds);
        //        con.Close();
        //    }
        //    catch (Exception Err)
        //    { }
        //    finally
        //    { }
        //    return ds;
        //}


        //private DataTable CenterTableLeft_Data()
        //{
        //    DataTable ds = new DataTable();

        //    try
        //    {



        //        string sqlString = "\n" +
        //        "SELECT SUM(cd.codAmount) TotalCODAmount, SUM(ic.consignmentAmount) invoiceAmount,\n" +
        //        "SUM(cd.codAmount) - SUM(ic.consignmentAmount) NetPayable\n" +
        //        "\n" +
        //        "FROM   Consignment c\n" +
        //        "       INNER JOIN Branches b\n" +
        //        "            ON  b.branchCode = c.orgin\n" +
        //        "       INNER JOIN Branches b2\n" +
        //        "            ON  b2.branchCode = c.destination\n" +
        //        "       INNER JOIN CODConsignmentDetail_New cd\n" +
        //        "            ON  cd.consignmentNumber = c.consignmentNumber\n" +
        //        "       INNER JOIN CreditClients cc\n" +
        //        "            ON  c.creditClientId = cc.id\n" +
        //        "       INNER JOIN Branches b3\n" +
        //        "            ON  cc.branchCode = b3.branchCode\n" +
        //        "       INNER JOIN PaymentVouchers pv\n" +
        //        "            ON  pv.ConsignmentNo = c.consignmentNumber\n" +
        //        "       LEFT OUTER JOIN InvoiceConsignment ic\n" +
        //        "            ON  ic.consignmentNumber = c.consignmentNumber\n" +
        //        "       LEFT OUTER JOIN Invoice i\n" +
        //        "            ON  i.invoiceNumber = ic.invoiceNumber\n" +
        //        "            AND i.IsInvoiceCanceled = '0'\n" +
        //        "       LEFT OUTER JOIN RunsheetConsignment rc\n" +
        //        "            ON  rc.consignmentNumber = c.consignmentNumber\n" +
        //        "            AND rc.createdOn = (\n" +
        //        "                    SELECT MAX(RC2.createdOn) statusdate\n" +
        //        "                    FROM   RunsheetConsignment RC2\n" +
        //        "                    WHERE  RC2.consignmentNumber = rc.consignmentNumber\n" +
        //        "                )\n" +
        //        "       LEFT OUTER JOIN rvdbo.Lookup l\n" +
        //        "            ON  l.AttributeGroup = 'POD_STATUS'\n" +
        //        "            AND CAST(l.Id AS VARCHAR) = rc.[Status]\n" +
        //        "\n" +
        //        "\n" +
        //        "\n" +
        //        "WHERE  pv.id in (" + clvar.Check_Condition1 + ") --c.transactionNumber = '999999'\n" +
        //        "       --AND c.consignerAccountNo = '4G7'";


        //        SqlConnection con = new SqlConnection(clvar.Strcon());
        //        SqlCommand orcd = new SqlCommand(sqlString, con);
        //        orcd.CommandType = CommandType.Text;
        //        SqlDataAdapter oda = new SqlDataAdapter(orcd);
        //        oda.Fill(ds);
        //        con.Close();
        //    }
        //    catch (Exception Err)
        //    { }
        //    finally
        //    { }
        //    return ds;
        //} 
        #endregion

        private DataTable CenterTableRight_Data(string invoiceNumbers)
        {
            DataTable ds = new DataTable();

            try
            {

                if (invoiceNumbers == "")
                {
                    invoiceNumbers = "''";
                }
                string sqlString = "\n" +
                "SELECT SUM(shipmentCharge) shipmentCharge,\n" +
                "SUM(FUELCHARGE) FUEL_CHARGE,\n" +
                "SUM(EXTRACHARGES) EXTRACHARGES,\n" +
                "SUM(EXTRACHARGESGST) + SUM(FUELCHARGEGST) GST\n" +
                "\n" +
                "FROM\n" +
                "(\n" +
                "SELECT sum(c.totalAmount) shipmentCharge,\n" +
                "\n" +
                "(SELECT ISNULL(SUM(IPM.CALCULATEDAMOUNT), 0)\n" +
                "   FROM INVOICEPRICEMODIFIERASSOCIATION IPM\n" +
                "  WHERE IPM.INVOICENO = ic.INVOICENUMBER\n" +
                "  AND IPM.MODIFIERTYPE = '1'\n" +
                ") FUELCHARGE,\n" +
                "\n" +
                "(SELECT ISNULL(SUM(IPM.CALCULATEDGST), 0)\n" +
                "   FROM INVOICEPRICEMODIFIERASSOCIATION IPM\n" +
                "  WHERE IPM.INVOICENO = ic.INVOICENUMBER\n" +
                "  AND IPM.MODIFIERTYPE = '1'\n" +
                ") FUELCHARGEGST,\n" +
                "\n" +
                "(SELECT ISNULL(SUM(CM.CALCULATEDVALUE), 0)\n" +
                "   FROM CONSIGNMENTMODIFIER AS CM\n" +
                "  WHERE CM.CONSIGNMENTNUMBER = C.CONSIGNMENTNUMBER) EXTRACHARGES,\n" +
                "(SELECT ISNULL(SUM(CM.CALCULATEDGST), 0)\n" +
                "   FROM CONSIGNMENTMODIFIER AS CM\n" +
                "  WHERE CM.CONSIGNMENTNUMBER = C.CONSIGNMENTNUMBER) EXTRACHARGESGST\n" +
                "\n" +
                "\n" +
                "FROM   Consignment c\n" +
                "       INNER JOIN Branches b\n" +
                "            ON  b.branchCode = c.orgin\n" +
                "       INNER JOIN Branches b2\n" +
                "            ON  b2.branchCode = c.destination\n" +
                "       INNER JOIN CODConsignmentDetail_New cd\n" +
                "            ON  cd.consignmentNumber = c.consignmentNumber\n" +
                "       INNER JOIN CreditClients cc\n" +
                "            ON  c.creditClientId = cc.id\n" +
                "       INNER JOIN Branches b3\n" +
                "            ON  cc.branchCode = b3.branchCode\n" +
                "       INNER JOIN PaymentVouchers pv\n" +
                "            ON  pv.ConsignmentNo = c.consignmentNumber\n" +
                "       LEFT OUTER JOIN InvoiceConsignment ic\n" +
                "            ON  ic.consignmentNumber = c.consignmentNumber\n" +
                "       LEFT OUTER JOIN Invoice i\n" +
                "            ON  i.invoiceNumber = ic.invoiceNumber\n" +
                "            AND i.IsInvoiceCanceled = '0'\n" +
                "       LEFT OUTER JOIN RunsheetConsignment rc\n" +
                "            ON  rc.consignmentNumber = c.consignmentNumber\n" +
                "            AND rc.createdOn = (\n" +
                "                    SELECT MAX(RC2.createdOn) statusdate\n" +
                "                    FROM   RunsheetConsignment RC2\n" +
                "                    WHERE  RC2.consignmentNumber = rc.consignmentNumber\n" +
                "                )\n" +
                "       LEFT OUTER JOIN rvdbo.Lookup l\n" +
                "            ON  l.AttributeGroup = 'POD_STATUS'\n" +
                "            AND CAST(l.Id AS VARCHAR) = rc.[Status]\n" +
                "WHERE  pv.id in (" + clvar.Check_Condition1 + ") --c.transactionNumber = '00000162'\n" +
                "       --AND c.consignerAccountNo = '4a260'\n" +
                "GROUP BY\n" +
                "ic.INVOICENUMBER, C.CONSIGNMENTNUMBER\n" +
                ") b";

                string sql = "SELECT SUM(c.totalAmount) shipmentCharge, \n"
               + "       SUM(ISNULL(ipma.calculatedAmount, 0)) FUEL_CHARGE, \n"
               + "       SUM(ISNULL(ipma.calculatedGST, 0)) EXTRACHARGES, \n"
               + "       SUM(ISNULL(ipma.calculatedAmount, 0)) + SUM(ISNULL(ipma.calculatedGST, 0))  \n"
               + "       GST \n"
               + "FROM   PaymentVouchers pv \n"
               + "       left outer JOIN InvoiceRedeem ir \n"
               + "            ON  ir.PaymentVoucherId = pv.Id \n"
               + "       INNER JOIN invoice i \n"
               + "            ON  i.invoiceNumber = ir.InvoiceNo \n"
               + "            AND i.IsInvoiceCanceled = '0' \n"
               + "       INNER JOIN InvoiceConsignment ic \n"
               + "            ON  ic.invoiceNumber = i.invoiceNumber \n"
               + "       INNER JOIN Consignment c \n"
               + "            ON  c.consignmentNumber = ic.consignmentNumber \n"
               + "       LEFT OUTER JOIN InvoicePriceModifierAssociation ipma \n"
               + "            ON  ipma.InvoiceNo = i.invoiceNumber \n"
               + "            AND ipma.modifierType = '1'\n"
               + "Where pv.id in (" + clvar.Check_Condition1 + ")";



                sql = "SELECT ISNULL(SUM(A.ShippingCharges), 0)     shipmentCharge, \n"
               + "       ISNULL(SUM(A.GST), 0)                 GST, \n"
               + "       ISNULL(SUM(A.FuelSurcharge), 0)       FUEL_CHARGE, \n"
               + "       ISNULL(SUM(A.ExtraCharges), 0)        EXTRACHARGES \n"
               + "FROM   ( \n"
               + "           SELECT ROUND(SUM(ISNULL(c.totalAmount, 0)), 2) ShippingCharges, \n"
               + "                  ROUND(SUM(ISNULL(c.gst, 0)), 2) GST, \n"
               + "                  0     FuelSurcharge, \n"
               + "                  0     ExtraCharges \n"
               + "           FROM   Invoice i \n"
               + "                  INNER JOIN InvoiceConsignment ic \n"
               + "                       ON  ic.invoiceNumber = i.invoiceNumber \n"
               + "                  INNER JOIN Consignment c \n"
               + "                       ON  c.consignmentNumber = ic.consignmentNumber \n"
               //+ "                  LEFT OUTER JOIN InvoicePriceModifierAssociation ip \n"
               //+ "                       ON  ip.InvoiceNo = i.invoiceNumber \n"
               //+ "                       AND ip.modifierType = '1' \n"
               //+ "                       AND ip.calculatedAmount > 0 \n"
               + "           WHERE  i.invoiceNumber in (" + invoiceNumbers + ") \n"
               + "            \n"
               + "           UNION  \n"
               + "            \n"
               + "           SELECT 0     ShippingCharges, \n"
               + "                  ROUND(SUM(ISNULL(ip.calculatedGST, 0)), 2) GST, \n"
               + "                  ROUND(SUM(ISNULL(ip.calculatedAmount, 0)), 2) FuelSurcharge, \n"
               + "                  0     ExtraCharges \n"
               + "           FROM   InvoicePriceModifierAssociation ip \n"
               + "           WHERE  ip.InvoiceNo in (" + invoiceNumbers + ") \n"
               //+ "                  AND ip.modifierType = '1' \n"
               + "            \n"
               + "           UNION \n"
               + "            \n"
               + "           SELECT 0     ShippingCharges, \n"
               + "                  ROUND(SUM(ISNULL(cm.calculatedGST, 0)), 2) GST, \n"
               + "                  0     FuelSurcharge, \n"
               + "                  ROUND(SUM(ISNULL(cm.calculatedValue, 0)), 2) ExtraCharges \n"
               + "           FROM   ConsignmentModifier cm \n"
               + "                  INNER JOIN InvoiceConsignment ic \n"
               + "                       ON  ic.consignmentNumber = cm.consignmentNumber \n"
               + "           WHERE  ic.invoiceNumber in (" + invoiceNumbers + ") \n"
               + "       )                          A \n"
               + "";

                SqlConnection con = new SqlConnection(clvar.Strcon());
                SqlCommand orcd = new SqlCommand(sql, con);
                orcd.CommandTimeout = 30000;
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                con.Close();
            }
            catch (Exception Err)
            { }
            finally
            { }
            return ds;
        }


        private DataTable CenterTableLeft_Data(string invoiceNumbers)
        {
            DataTable ds = new DataTable();

            try
            {

                if (invoiceNumbers == "")
                {
                    invoiceNumbers = "''";
                }

                string sqlString = "\n" +
                "SELECT SUM(cd.codAmount) TotalCODAmount, SUM(ic.consignmentAmount) invoiceAmount,\n" +
                "SUM(cd.codAmount) - SUM(ic.consignmentAmount) NetPayable\n" +
                "\n" +
                "FROM   Consignment c\n" +
                "       INNER JOIN Branches b\n" +
                "            ON  b.branchCode = c.orgin\n" +
                "       INNER JOIN Branches b2\n" +
                "            ON  b2.branchCode = c.destination\n" +
                "       INNER JOIN CODConsignmentDetail_New cd\n" +
                "            ON  cd.consignmentNumber = c.consignmentNumber\n" +
                "       INNER JOIN CreditClients cc\n" +
                "            ON  c.creditClientId = cc.id\n" +
                "       INNER JOIN Branches b3\n" +
                "            ON  cc.branchCode = b3.branchCode\n" +
                "       INNER JOIN PaymentVouchers pv\n" +
                "            ON  pv.ConsignmentNo = c.consignmentNumber\n" +
                "       LEFT OUTER JOIN InvoiceConsignment ic\n" +
                "            ON  ic.consignmentNumber = c.consignmentNumber\n" +
                "       LEFT OUTER JOIN Invoice i\n" +
                "            ON  i.invoiceNumber = ic.invoiceNumber\n" +
                "            AND i.IsInvoiceCanceled = '0'\n" +
                "       LEFT OUTER JOIN RunsheetConsignment rc\n" +
                "            ON  rc.consignmentNumber = c.consignmentNumber\n" +
                "            AND rc.createdOn = (\n" +
                "                    SELECT MAX(RC2.createdOn) statusdate\n" +
                "                    FROM   RunsheetConsignment RC2\n" +
                "                    WHERE  RC2.consignmentNumber = rc.consignmentNumber\n" +
                "                )\n" +
                "       LEFT OUTER JOIN rvdbo.Lookup l\n" +
                "            ON  l.AttributeGroup = 'POD_STATUS'\n" +
                "            AND CAST(l.Id AS VARCHAR) = rc.[Status]\n" +
                "\n" +
                "\n" +
                "\n" +
                "WHERE  pv.id in (" + clvar.Check_Condition1 + ") --c.transactionNumber = '999999'\n" +
                "       --AND c.consignerAccountNo = '4G7'";

                string sql = "SELECT SUM(pv.amount)         TotalCODAmount, \n"
               + "       SUM(i.totalAmount)     invoiceAmount, \n"
               + "       (SUM(pv.Amount) - SUM(i.totalAmount)) NetPayable \n"
               + "FROM   PaymentVouchers pv \n"
               + "       left outer JOIN InvoiceRedeem ir \n"
               + "            ON  ir.PaymentVoucherId = pv.Id \n"
               + "       left outer JOIN Invoice i \n"
               + "            ON  i.invoiceNumber = ir.InvoiceNo \n"
               + "WHERE  pv.id in (" + clvar.Check_Condition1 + ")";


                sql = "SELECT SUM(A.CODAmount)         TotalCODAmount, \n"
              + "       SUM(A.InvoiceAmount)     InvoiceAmount, \n"
              + "       (SUM(A.CODAmount) - SUM(A.InvoiceAmount)) NETPayable \n"
              + "FROM   ( \n"
              + "           SELECT SUM(pv.Amount)      CODAmount, \n"
              + "                  0                   InvoiceAmount \n"
              + "           FROM   PaymentVouchers     pv \n"
              + "           WHERE  pv.Id IN (" + clvar.Check_Condition1 + ") \n"
              + "            \n"
              + "           UNION \n"
              + "            \n"
              + "           SELECT 0                      CODAmount, \n"
              + "                  SUM(i.totalAmount)     InvoiceAmount \n"
              + "           FROM   Invoice                i \n"
              + "           WHERE  i.invoiceNumber IN (" + invoiceNumbers + ") \n"
              + "       )                        A";

                SqlConnection con = new SqlConnection(clvar.Strcon());
                SqlCommand orcd = new SqlCommand(sql, con);
                orcd.CommandTimeout = 30000;
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                con.Close();
            }
            catch (Exception Err)
            { }
            finally
            { }
            return ds;
        }
    }
}