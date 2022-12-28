using System;
using System.Web.UI;
using MRaabta.App_Code;
using System.Data;
using System.Data.SqlClient;

namespace MRaabta.Files
{
    public partial class SearchConsignment : System.Web.UI.Page
    {
        Cl_Variables clvar = new Cl_Variables();
        Consignemnts con = new Consignemnts();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString.Keys.Count != 0)
                {
                    txt_consignmentNumber.Text = Request.QueryString["XCode"];
                    txt_consignmentNumber.Enabled = false;
                    btn_print.Visible = true;
                    ConsignmentDetails();

                }
            }
        }

        protected void ConsignmentDetails()
        {
            DataTable dt = new DataTable();
            clvar.consignmentNo = txt_consignmentNumber.Text;
            dt = con.GetConsignmentDetail(clvar);
            if (dt.Rows.Count > 0)
            {
                txt_consignmentNumber.Text = dt.Rows[0]["ConNo"].ToString();
                lbl_accountNo.Text = dt.Rows[0]["AccountNo"].ToString();
                lbl_bookingDate.Text = dt.Rows[0]["BookingDate"].ToString();
                lbl_city.Text = dt.Rows[0]["City"].ToString();
                lbl_destination.Text = dt.Rows[0]["Branch"].ToString();
                lbl_discount.Text = dt.Rows[0]["Discount"].ToString();
                lbl_gstCharges.Text = dt.Rows[0]["gst"].ToString();
                if (dt.Rows[0]["IsInsured"].ToString() == "1")
                {
                    lbl_insurance.Text = (double.Parse(dt.Rows[0]["DeclaredValue"].ToString()) * 0.025).ToString();
                }
                else
                {
                    lbl_insurance.Text = "0";
                }
                //lbl_insurance.Text = "00000";
                lbl_origin.Text = dt.Rows[0]["ORigin"].ToString();
                lbl_originExpressCenter.Text = dt.Rows[0]["name"].ToString();
                lbl_packageContents.Text = dt.Rows[0]["PackageContents"].ToString();
                lbl_pieces.Text = dt.Rows[0]["Pieces"].ToString();
                lbl_riderCode.Text = dt.Rows[0]["RiderCode"].ToString();
                lbl_serviceType.Text = dt.Rows[0]["ServiceTypeName"].ToString();
                lbl_totalCharges.Text = dt.Rows[0]["TotalAmount"].ToString();

                lbl_totalAmount.Text = (double.Parse(dt.Rows[0]["TotalAmount"].ToString()) + double.Parse(dt.Rows[0]["GST"].ToString())).ToString();
                lbl_weight.Text = dt.Rows[0]["Weight"].ToString();
                lbl_consignee.Text = dt.Rows[0]["Consignee"].ToString();
                lbl_consigneeAddress.Text = dt.Rows[0]["Address"].ToString();
                lbl_consigneeCell.Text = dt.Rows[0]["ConsigneeCell"].ToString();
                lbl_consigneeCNIC.Text = dt.Rows[0]["ConsigneeCNIC"].ToString();
                lbl_consigner.Text = dt.Rows[0]["Consigner"].ToString();
                lbl_consignerAddress.Text = dt.Rows[0]["ShipperAddress"].ToString();
                lbl_consignerCell.Text = dt.Rows[0]["ConsignerCell"].ToString();
                lbl_consignerCNIC.Text = dt.Rows[0]["ConsignerCNIC"].ToString();
                lbl_consignmentType.Text = dt.Rows[0]["ConType"].ToString();
                if (dt.Rows[0]["CODTYPE"].ToString() != "0")
                {
                    cb_COD.Checked = true;
                }
                else
                {
                    cb_COD.Checked = false;
                }
            }

            DataTable cod = new DataTable();
            cod = con.GetCodDetailByConsignmentNumber(clvar);
            if (cod.Rows.Count > 0)
            {
                lbl_orderRef.Text = cod.Rows[0]["orderrefNo"].ToString();
                lbl_productType.Text = cod.Rows[0]["PRODUCTTYPEID"].ToString();
                if (cod.Rows[0]["ChargeCodAmount"].ToString() != "0")
                {
                    Cb_CODAmount.Checked = true;
                }
                else
                {
                    Cb_CODAmount.Checked = false;
                }

                lbl_descriptionCOD.Text = cod.Rows[0]["ProductDescription"].ToString();
                lbl_CODAmount.Text = cod.Rows[0]["CODAMount"].ToString();
            }

            DataTable priceModifier = GetConsignmentModifier(clvar);
            if (priceModifier.Rows.Count > 0)
            {
                RadGrid1.DataSource = priceModifier;
                RadGrid1.DataBind();
                double value = 0;
                double gst = 0;
                foreach (DataRow row in priceModifier.Rows)
                {
                    value += double.Parse(row["calculatedValue"].ToString());
                    gst += double.Parse(row["calculatedGST"].ToString());
                }

                lbl_totalCharges.Text = (double.Parse(lbl_totalCharges.Text)).ToString();
                lbl_gstCharges.Text = (double.Parse(lbl_gstCharges.Text)).ToString();
                lbl_totalAmount.Text = (double.Parse(dt.Rows[0]["TotalAmount"].ToString()) + double.Parse(dt.Rows[0]["gst"].ToString())).ToString();
                //lbl_totalAmount.Text = (double.Parse(dt.Rows[0]["TotalAmount"].ToString()) + double.Parse(dt.Rows[0]["gst"].ToString()) + value + gst + double.Parse(lbl_insurance.Text)).ToString();
            }


        }

        protected void txt_consignmentNumber_TextChanged(object sender, EventArgs e)
        {
            ConsignmentDetails();
        }
        protected void btn_PrintConsignment_Click(object sender, EventArgs e)
        {
            if (Session["BookingUserStatus"].ToString() == "1")
            {
                #region FOR PRINT CN

                string script_ = @"window.open(""{0}"", ""{1}"", ""{2}"");";

                string ecnryptedConsignment = EnryptString(txt_consignmentNumber.Text);
                string script = String.Format(script_, "RetailBookingReceipt.aspx?id=" + ecnryptedConsignment, "_blank", "");

                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Redirect", script, true);

                #endregion
            }
            else
            {
                string script_ = @"window.open(""{0}"", ""{1}"", ""{2}"");";

                string script = String.Format(script_, "invoicepdf_2.aspx?id=" + txt_consignmentNumber.Text, "_blank", "");

                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Redirect", script, true);
            }


            //   Response.Redirect("~/Files/invoicepdf.aspx?id=" + txt_consignmentNumber.Text);


            /*
            var htmlToPdf = new NReco.PdfGenerator.HtmlToPdfConverter();

            iTextSharp.text.PageSize p = new iTextSharp.text.PageSize();
            VerifyRenderingInServerForm(tbl_invoice);
            String Consignment = "999999999011";
            lbl_Error.Text = "";

            //   clvar = new Cl_Variables();
            // PDF OUTPUT
            tbl_invoice.Visible = true;

            prt_acc.Text = txt_AccNo.Text;
            prt_reff.Text = txt_Type.Text.ToUpper();

            // consigner
            prt_consigner.Text = txt_Consigner.Text.ToUpper();
            prt_consigner_add.Text = txt_Address.Text.ToUpper();
            prt_consigner_ph.Text = txt_ConsignerCellNo.Text;
            prt_consigner_nic.Text = Txt_ConsignerCNIC.Text;

            // consignee
            prt_consignee_ph.Text = txt_ConsigneeCellno.Text;
            prt_consignee_add.Text = txt_ShipperAddress.Text.ToUpper();
            prt_consignee.Text = txt_Consignee.Text.ToUpper();


            prt_consignmentno.Text = txt_ConNo.Text;
            prt_package.Text = txt_Package_Handcarry.Text;

            prt_piecies.Text = txt_Piecies.Text.Trim();
            prt_weight.Text = txt_Weight.Text;


            prt_servicetype.Text = cb_ServiceType.SelectedValue;
            prt_paymentmode.Text = Rb_CustomerType.SelectedValue;

            prt_servicecharge.Text = txt_TotalCharges.Text;
            prt_discountamount.Text = "0.00";
            prt_gstamount.Text = txt_Othercharges.Text;
            prt_totalamount.Text = txt_TotalAmount.Text;


            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            var strWr = new StringWriter();
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.AddHeader("content-disposition", "attachment;filename=InvoicePDF.pdf");
            Response.ContentType = "application/pdf";

            tbl_invoice.RenderControl(hw);
            base.Render(hw);
            htmlToPdf.GeneratePdf(strWr.ToString(), null, Response.OutputStream);
            Response.End();

            //Response.ContentType = "application/pdf";
            //Response.AddHeader("content-disposition", "attachment;filename=InvoicePDF.pdf");
            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //StringWriter sw = new StringWriter();
            //HtmlTextWriter hw = new HtmlTextWriter(sw);
            ////  GridView1.AllowPaging = false;
            ////  GridView1.DataBind();
            //tbl_invoice.RenderControl(hw);
            //StringReader sr = new StringReader(sw.ToString());
            //iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 1f, 1f, 1f, 1f);

            //HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            //PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            //pdfDoc.Open();
            //htmlparser.Parse(sr);
            //pdfDoc.Close();
            //Response.Write(pdfDoc);
            //Response.End();
            //htmlToPdf.GeneratePdfFromFile(tbl_invoice.InnerHtml, null, "export.pdf");
             * 
             */
        }

        public string EnryptString(string strEncrypted)
        {
            byte[] b = System.Text.ASCIIEncoding.ASCII.GetBytes(strEncrypted);
            string encrypted = Convert.ToBase64String(b);
            return encrypted;
        }

        public DataTable GetConsignmentModifier(Cl_Variables clvar)
        {

            string sqlString = "select p.name,\n" +
            "         p.description,\n" +
            "         c.calculatedValue,\n" +
            "         c.calculationBase,\n" +
            "         c.modifiedCalculationValue, calculatedGST \n" +
            "    from ConsignmentModifier c\n" +
            "   inner join PriceModifiers p\n" +
            "      on p.id = c.priceModifierId\n" +
            "   where c.consignmentNumber = '" + clvar.consignmentNo + "'";
            DataTable dt = new DataTable();

            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dt);
                orcl.Close();
            }
            catch (Exception)
            { }

            return dt;
        }
    }

}