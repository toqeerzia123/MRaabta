using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using MRaabta.App_Code;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Web.UI.HtmlControls;

namespace MRaabta.Files
{
    public partial class InvoicePDF_2 : System.Web.UI.Page
    {
        CommonFunction CF = new CommonFunction();
        Cl_Variables clvar = new Cl_Variables();
        Consignemnts con = new Consignemnts();
        public DataTable GetConsignmentModifier(Cl_Variables clvar)
        {
            DataTable dt = new DataTable();
            string query = " selecT * from ConsignmentModifier cm\n" +
                           "  inner join priceModifier pm\n" +
                           "     on cm.priceModifierID = pm.id\n" +
                           "  where consignmentNumber = '" + clvar.consignmentNo + "'";
            query = " selecT * from ConsignmentModifier cm \n" +
                    "  inner join PriceModifiers pm\n" +
                    "     on pm.id = cm.priceModifierId\n" +
                    "  where cm.consignmentNumber = '" + clvar.consignmentNo + "'";
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
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                clvar.consignmentNo = Request.QueryString["id"]; //"999999999010";//

                DataSet ds = con.Consignment(clvar);
                DataTable priceModifier = GetConsignmentModifier(clvar);
                //   DataSet ds2 = con.br
                //Consignment
                if (ds.Tables.Count != 0)
                {
                    bool isInsured = false;
                    bool insuranceApplied = false;
                    if (ds.Tables[0].Rows[0]["IsInsured"].ToString().ToUpper() == "TRUE" || ds.Tables[0].Rows[0]["IsInsured"].ToString().ToUpper() == "1")
                    {
                        isInsured = true;
                        if (priceModifier.Select("CalculationBase = '3'").Count() > 0)
                        {
                            insuranceApplied = true;
                            object obj = priceModifier.Compute("SUM(AlternateValue)", "CalculationBase = '3'");
                            double declaredValue = 0;
                            double.TryParse(obj.ToString(), out declaredValue);
                            lbl_insuranceMessage.Text = lbl_insuranceMessage2.Text = "Declared Value for Insurance: " + String.Format("{0:N0}", declaredValue);
                        }
                        else
                        {
                            insuranceApplied = false;
                            lbl_insuranceMessage.Text = lbl_insuranceMessage2.Text = "Insurance Offered but Refused.";
                        }
                    }

                    RadBarcode1.Text = ds.Tables[0].Rows[0]["ConsignmentNumber"].ToString();

                    Bitmap bitmap1 = new Bitmap(RadBarcode1.GetImage());
                    MemoryStream stream = new MemoryStream();
                    bitmap1.Save(Server.MapPath("~") + "\\docs\\" + clvar.consignmentNo + ".png", ImageFormat.Png);

                    img_1.Src = "../docs/" + clvar.consignmentNo + ".png";
                    prt_acc.Text = ds.Tables[0].Rows[0]["ConsignerAccountNo"].ToString();// txt_AccNo.Text;

                    // prt_reff.Text = ds.Tables[0].Rows[0]["couponNumber"].ToString();// 

                    if (ds.Tables[0].Rows[0]["couponNumber"].ToString() != "")
                    {
                        prt_reff.Text = ds.Tables[0].Rows[0]["couponNumber"].ToString();// 
                    }
                    else
                    {
                        prt_reff.Text = "&nbsp;";
                    }

                    // consigner
                    if (ds.Tables[0].Rows[0]["Consigner"].ToString().Length <= 15)
                    {
                        prt_consigner.Text = ds.Tables[0].Rows[0]["Consigner"].ToString();// 
                    }
                    else
                    {
                        prt_consigner.Text = ds.Tables[0].Rows[0]["Consigner"].ToString().Substring(0, 15);// 
                    }

                    prt_consigner_add.Text = ds.Tables[0].Rows[0]["shipperAddress"].ToString();// 
                    prt_consigner_ph.Text = ds.Tables[0].Rows[0]["Consignercellno"].ToString();// 
                    prt_consigner_nic.Text = ds.Tables[0].Rows[0]["ConsignerCNICno"].ToString();// 
                    clvar.Branch = ds.Tables[0].Rows[0]["destination"].ToString();// 
                    prt_branch.Text = CF.Branch(clvar).Tables[0].Rows[0]["sname"].ToString();// 
                    clvar.Branch = ds.Tables[0].Rows[0]["orgin"].ToString();// 
                    prt_zone.Text = CF.Branch(clvar).Tables[0].Rows[0]["sname"].ToString();//


                    clvar.createdBy = ds.Tables[0].Rows[0]["createdby"].ToString();
                    DataSet ds1 = con.User(clvar);
                    //     if (ds.Tables[0].Rows[0]["createdby"].ToString() == "")
                    if (ds1.Tables[0].Rows.Count == 0)
                    {
                        prt_username.Text = "";
                        shi_username.Text = "";
                    }
                    else
                    {
                        prt_username.Text = ds1.Tables[0].Rows[0]["name"].ToString();// 
                        shi_username.Text = ds1.Tables[0].Rows[0]["name"].ToString();// 
                    }

                    lbl_hps.Text = "M&P offers Free Home pick-up service so you can stay home stay safe and stay Courier Connected.  For details and pickups call now 021 111 202 202.";
                    lbl_hps1.Text = "M&P offers Free Home pick-up service so you can stay home stay safe and stay Courier Connected.  For details and pickups call now 021 111 202 202.";
                    ridercode.Text = "Rider Code: " + ds.Tables[0].Rows[0]["ridercode"].ToString();// 
                    prt_bookingdate.Text = ds.Tables[0].Rows[0]["createdon"].ToString();//


                    shi_bookingdate.Text = ds.Tables[0].Rows[0]["createdon"].ToString();// 
                                                                                        //    shi_username.Text = ds1.Tables[0].Rows[0]["name"].ToString();// 
                    shi_ridername.Text = "Rider Code: " + ds.Tables[0].Rows[0]["ridercode"].ToString();// 

                    //cb_Origin

                    // consignee
                    prt_consignee_ph.Text = ds.Tables[0].Rows[0]["Consigneephoneno"].ToString();// 
                    prt_consignee_add.Text = ds.Tables[0].Rows[0]["address"].ToString();// 
                    if (ds.Tables[0].Rows[0]["consignee"].ToString().Length <= 15)
                    {
                        prt_consignee.Text = ds.Tables[0].Rows[0]["consignee"].ToString();// 
                    }
                    else
                    {
                        prt_consignee.Text = ds.Tables[0].Rows[0]["consignee"].ToString().Substring(0, 15);//                
                    }


                    prt_consignmentno.Text = ds.Tables[0].Rows[0]["ConsignmentNumber"].ToString();//

                    prt_package.Text = ds.Tables[0].Rows[0]["PakageContents"].ToString();// 
                    /*
                    if (ds.Tables[0].Rows[0]["PakageContents"].ToString().Length <= 30)
                    {
                        prt_package.Text = ds.Tables[0].Rows[0]["PakageContents"].ToString();// 
                    }
                    else
                    {
                        prt_package.Text = ds.Tables[0].Rows[0]["PakageContents"].ToString().Substring(0, 30);//                 
                    }
                    */
                    prt_piecies.Text = ds.Tables[0].Rows[0]["pieces"].ToString();// 
                    prt_weight.Text = ds.Tables[0].Rows[0]["weight"].ToString();// 

                    lbl_specialInstructions.Text = ds.Tables[0].Rows[0]["Remarks"].ToString();
                    lbl_specialInstructions2.Text = ds.Tables[0].Rows[0]["Remarks"].ToString();

                    prt_servicetype.Text = ds.Tables[0].Rows[0]["serviceTypeName"].ToString();// 

                    // prt_paymentmode.Text = ds.Tables[0].Rows[0]["CustomerType"].ToString();

                    if (ds.Tables[0].Rows[0]["CustomerType"].ToString() == "1")
                    {
                        prt_paymentmode.Text = "CASH";

                        prt_servicecharge.Text = ds.Tables[0].Rows[0]["TotalAmount"].ToString();// 
                        prt_discountamount.Text = "0.00";
                        prt_gstamount.Text = ds.Tables[0].Rows[0]["gst"].ToString();// 
                        prt_totalamount.Text = string.Format("{0:N0}", double.Parse(ds.Tables[0].Rows[0]["TotalAmount"].ToString()) + Double.Parse(ds.Tables[0].Rows[0]["gst"].ToString()));// 
                        double tempGST = double.Parse(ds.Tables[0].Rows[0]["gst"].ToString());
                        double tempTotalAmount = double.Parse(ds.Tables[0].Rows[0]["TotalAmount"].ToString());
                        if (priceModifier.Rows.Count > 0)
                        {
                            string extraText = "";


                            foreach (DataRow row in priceModifier.Rows)
                            {
                                extraText += "<div style=\"float: left; width: 51%; text-align: center;\">" + row["name"].ToString() + "</div> " +
                                            "<div style=\"float: right; width: 49%; text-align: center;\">" + row["CalculatedValue"].ToString() + "</div>";
                                tempGST += double.Parse(row["CalculatedGST"].ToString());
                                tempTotalAmount += double.Parse(row["CalculatedValue"].ToString());
                            }
                            extraDiv.InnerHtml = extraText.ToString();
                        }
                        prt_gstamount.Text = String.Format("{0:N2}", tempGST);
                        prt_totalamount.Text = String.Format("{0:N0}", tempTotalAmount + tempGST);
                    }
                    else
                    {
                        prt_paymentmode.Text = "CREDIT";
                        prt_servicecharge.Text = "0.00";
                        prt_discountamount.Text = "0.00";
                        prt_gstamount.Text = "0.00";
                        prt_totalamount.Text = "0.00";
                    }

                    //if (ds.Tables[0].Rows[0]["decalaredValue"].ToString() == "" || ds.Tables[0].Rows[0]["insuarancePercentage"].ToString() == "")
                    //{
                    //    lbl_nsurance.Text = "0";
                    //    prt_insurance.Text = "0";
                    //}
                    //else
                    //{
                    //    double inc = (double.Parse(ds.Tables[0].Rows[0]["decalaredValue"].ToString()) * double.Parse(ds.Tables[0].Rows[0]["insuarancePercentage"].ToString())) / 100;

                    //    lbl_nsurance.Text = inc.ToString();
                    //    prt_insurance.Text = inc.ToString();
                    //}
                    Label1.Text = ds.Tables[0].Rows[0]["ConsignerAccountNo"].ToString();// txt_AccNo.Text;
                                                                                        //  Label2.Text = ds.Tables[0].Rows[0]["couponNumber"].ToString();// 

                    if (ds.Tables[0].Rows[0]["couponNumber"].ToString() != "")
                    {
                        Label2.Text = ds.Tables[0].Rows[0]["couponNumber"].ToString();// 
                    }
                    else
                    {
                        Label2.Text = "&nbsp;";
                    }


                    Image1.Src = "../docs/" + clvar.consignmentNo + ".png";
                    if (ds.Tables[0].Rows[0]["Consigner"].ToString().Length <= 15)
                    {
                        Label3.Text = ds.Tables[0].Rows[0]["Consigner"].ToString();// 
                    }
                    else
                    {
                        Label3.Text = ds.Tables[0].Rows[0]["Consigner"].ToString().Substring(0, 15);//                 
                    }

                    Label4.Text = ds.Tables[0].Rows[0]["shipperAddress"].ToString();// 
                    Label5.Text = ds.Tables[0].Rows[0]["Consignercellno"].ToString();// 
                    Label6.Text = ds.Tables[0].Rows[0]["ConsignerCNICno"].ToString();// 
                    clvar.Branch = ds.Tables[0].Rows[0]["destination"].ToString();// 
                    Label13.Text = CF.Branch(clvar).Tables[0].Rows[0]["sname"].ToString();// 
                    clvar.Branch = ds.Tables[0].Rows[0]["orgin"].ToString();// 
                    Label12.Text = CF.Branch(clvar).Tables[0].Rows[0]["sname"].ToString();//
                                                                                          //cb_Origin

                    // consignee
                    Label7.Text = ds.Tables[0].Rows[0]["Consigneephoneno"].ToString();// 
                    Label8.Text = ds.Tables[0].Rows[0]["address"].ToString();// 

                    if (ds.Tables[0].Rows[0]["consignee"].ToString().Length <= 15)
                    {
                        Label9.Text = ds.Tables[0].Rows[0]["consignee"].ToString();// 
                    }
                    else
                    {
                        Label9.Text = ds.Tables[0].Rows[0]["consignee"].ToString().Substring(0, 15);//
                    }

                    Label10.Text = ds.Tables[0].Rows[0]["ConsignmentNumber"].ToString();// 
                                                                                        // Label11.Text = ds.Tables[0].Rows[0]["PakageContents"].ToString();// 
                    Label11.Text = ds.Tables[0].Rows[0]["PakageContents"].ToString();// 

                    /*
                    if (ds.Tables[0].Rows[0]["PakageContents"].ToString().Length <= 30)
                    {
                        Label11.Text = ds.Tables[0].Rows[0]["PakageContents"].ToString();// 
                    }
                    else
                    {
                        Label11.Text = ds.Tables[0].Rows[0]["PakageContents"].ToString().Substring(0, 30);//                 
                    }
                    */


                    Label14.Text = ds.Tables[0].Rows[0]["pieces"].ToString();// 
                    Label15.Text = ds.Tables[0].Rows[0]["weight"].ToString();// 


                    Label16.Text = ds.Tables[0].Rows[0]["serviceTypeName"].ToString();// 

                    //  Label17.Text = ds.Tables[0].Rows[0]["CustomerType"].ToString();

                    if (ds.Tables[0].Rows[0]["CustomerType"].ToString() == "1")
                    {
                        Label17.Text = "CASH";

                        Label18.Text = ds.Tables[0].Rows[0]["TotalAmount"].ToString();// 
                        Label19.Text = "0.00";
                        Label20.Text = ds.Tables[0].Rows[0]["gst"].ToString();// 
                        Label21.Text = string.Format("{0:N0}", double.Parse(ds.Tables[0].Rows[0]["TotalAmount"].ToString()) + Double.Parse(ds.Tables[0].Rows[0]["gst"].ToString()));// 

                        double tempGST = double.Parse(ds.Tables[0].Rows[0]["gst"].ToString());
                        double tempTotalAmount = double.Parse(ds.Tables[0].Rows[0]["TotalAmount"].ToString());

                        if (priceModifier.Rows.Count > 0)
                        {
                            string extraText = "";

                            foreach (DataRow row in priceModifier.Rows)
                            {
                                extraText += "<div style=\"float: left; width: 51%; text-align: center;\">" + row["name"].ToString() + "</div> " +
                                            "<div style=\"float: right; width: 49%; text-align: center;\">" + row["CalculatedValue"].ToString() + "</div>";
                                tempGST += double.Parse(row["CalculatedGST"].ToString());
                                tempTotalAmount += double.Parse(row["CalculatedValue"].ToString());
                            }
                            Label20.Text = String.Format("{0:N2}", tempGST);
                            Label21.Text = String.Format("{0:N0}", tempTotalAmount + tempGST);
                            extraDiv1.InnerHtml = extraText.ToString();
                        }
                    }
                    else
                    {
                        Label17.Text = "CREDIT";

                        Label18.Text = "0.00";
                        Label19.Text = "0.00";
                        Label20.Text = "0.00";
                        Label21.Text = "0.00";
                    }

                    /*
                    if (ds.Tables[0].Rows[0]["CustomerType"] == "1")
                    {
                        Label18.Text = ds.Tables[0].Rows[0]["TotalAmount"].ToString();// 
                        Label19.Text = "0.00";
                        Label20.Text = ds.Tables[0].Rows[0]["gst"].ToString();// 
                        Label21.Text = string.Format("{0:N0}", double.Parse(ds.Tables[0].Rows[0]["TotalAmount"].ToString()) + Double.Parse(ds.Tables[0].Rows[0]["gst"].ToString()));// 
                    }
                    else
                    {
                        Label18.Text = "0.00"; 
                        Label19.Text = "0.00";
                        Label20.Text = "0.00";
                        Label21.Text = "0.00";
                    }
                     */

                    PrintWebControl(tbl_invoice, string.Empty);
                    // int width = tbl_invoice_.w.Width;
                    //  int height = tbl_invoice_.Size.Height;

                    // bitmap1 = new Bitmap(this.tbl_invoice_.get);

                    //PDF;

                    //      Response.Clear();
                    //      Response.ContentType = "application/pdf";
                    //      Response.AddHeader("content-disposition", "attachment;filename=InvoicePDF.pdf");
                    //      Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    //      StringWriter sw = new StringWriter();
                    //      HtmlTextWriter hw = new HtmlTextWriter(sw);
                    ////      RadBarcode1.RenderControl(hw);
                    //      tbl_invoice.RenderControl(hw);
                    //      StringReader sr = new StringReader(sw.ToString());
                    //      Document pdfDoc = new Document(PageSize.A3, 0, 0, 0, 0);

                    //      HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                    //      PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                    //      pdfDoc.Open();
                    //      htmlparser.Parse(sr);
                    //      pdfDoc.Close();
                    //      Response.Write(pdfDoc);
                    //      Response.End();
                }

            }
        }

        public static void PrintWebControl(Control ctrl, string Script)
        {
            Random r = new Random();
            int a = r.Next(0, 100);
            StringWriter stringWrite = new StringWriter();
            System.Web.UI.HtmlTextWriter htmlWrite = new System.Web.UI.HtmlTextWriter(stringWrite);
            if (ctrl is WebControl)
            {
                Unit w = new Unit(100, UnitType.Percentage); ((WebControl)ctrl).Width = w;
            }
            Page pg = new Page();
            pg.EnableEventValidation = false;
            if (Script != string.Empty)
            {
                pg.ClientScript.RegisterStartupScript(pg.GetType(), "PrintJavaScript", Script);
            }
            HtmlForm frm = new HtmlForm();
            pg.Controls.Add(frm);
            frm.Attributes.Add("runat", "server");
            frm.Controls.Add(ctrl);
            pg.DesignerInitialize();
            pg.RenderControl(htmlWrite);
            string strHTML = stringWrite.ToString();
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Write(strHTML);

            HttpContext.Current.Response.Write("<script>window.print();</script>");
            HttpContext.Current.Response.End();
        }


        protected void ExportToPDF2(object sender, EventArgs e)
        {

        }

        protected void RadBarcode1_PreRender(object sender, EventArgs e)
        {

        }
    }
}