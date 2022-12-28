using MRaabta.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;

namespace MRaabta.Files
{
    public class ReceiptTableModel
    {
        //customer copy

        public string lbl_cn { get; set; }
        public string lbl_user { get; set; }
        public string lbl_ref { get; set; }
        public string lbl_booking { get; set; }
        public string lbl_origin { get; set; }
        public string lbl_dst { get; set; }
        public string lbl_consignee { get; set; }
        public string lbl_consigneeaddress { get; set; }
        public string lbl_consigneephone { get; set; }
        public string lbl_shipper { get; set; }
        public string lbl_shipperaddress { get; set; }
        public string lbl_shippercontact { get; set; }
        public string lbl_Piece { get; set; }
        public string lbl_weight { get; set; }
        public string lbl_w { get; set; }
        public string lbl_b { get; set; }
        public string lbl_h { get; set; }
        public string lbl_val { get; set; }
        public string lbl_insurance { get; set; }
        public string lbl_amount { get; set; }
        public string lbl_discount { get; set; }
        public string lbl_gst { get; set; }
        public string lbl_chargedamount { get; set; }

        //consignee copy
        public string lbl_cn2 { get; set; }
        public string lbl_user2 { get; set; }
        public string lbl_ref2 { get; set; }
        public string lbl_booking2 { get; set; }
        public string lbl_origin2 { get; set; }
        public string lbl_dst2 { get; set; }
        public string lbl_consignee2 { get; set; }
        public string lbl_consigneeaddress2 { get; set; }
        public string lbl_consigneephone2 { get; set; }
        public string lbl_shipper2 { get; set; }
        public string lbl_shipperaddress2 { get; set; }
        public string lbl_shippercontact2 { get; set; }
        public string lbl_Piece2 { get; set; }
        public string lbl_weight2 { get; set; }
        public string lbl_w2 { get; set; }
        public string lbl_b2 { get; set; }
        public string lbl_h2 { get; set; }
        public string lbl_val2 { get; set; }
        public string lbl_insurance2 { get; set; }
        public string lbl_amount2 { get; set; }
        public string lbl_discount2 { get; set; }
        public string lbl_gst2 { get; set; }
        public string lbl_chargedamount2 { get; set; }
        public string lbl_supplementary { get; set; }
        public string lbl_supplementary2 { get; set; }
        public string lblService { get; set; }
        public string lblService2 { get; set; }
        public string lblSpecialInstructions { get; set; }
        public string lblSpecialInstructions2 { get; set; }
        public string lblPackageContents { get; set; }
        public string lblPackageContents2 { get; set; }

        public string ImageUrlQR { get; set; }
        public string ImageUrlQR2 { get; set; }
        public string lbl_gstpercentage1 { get; set; }
        public string lbl_gstpercentage2 { get; set; }
        public string lbl_ntn1 { get; set; }
        public string lbl_ntn2 { get; set; }
        public string lbl_cnic_ntn1 { get; set; }
        public string lbl_cnic_ntn2 { get; set; }
        public string lbl_ridercode1 { get; set; }
        public string lbl_ridercode2 { get; set; }
        
    }

    public partial class RetailBookingReceipt : System.Web.UI.Page
    {
        Cl_Variables clvar = new Cl_Variables();
        Consignemnts con = new Consignemnts();

        protected void Page_Load(object sender, EventArgs e)
        {
            PrintReport();
        }
        public string DecryptString(string encrString)
        {
            byte[] b;
            string decrypted;
            try
            {
                b = Convert.FromBase64String(encrString);
                decrypted = System.Text.ASCIIEncoding.ASCII.GetString(b);
            }
            catch (FormatException fe)
            {
                decrypted = "";
            }
            return decrypted;
        }
        public string EnryptString(string strEncrypted)
        {
            byte[] b = System.Text.ASCIIEncoding.ASCII.GetBytes(strEncrypted);
            string encrypted = Convert.ToBase64String(b);
            return encrypted;
        }
        protected void PrintReport()
        {
            double chargedAmount = 0, totalAmount = 0, gst = 0;
            String encrypted = Request.QueryString["id"].ToString();

            string decrypted = DecryptString(encrypted);
            string[] numbers = decrypted.Split(',');
            String consignments = "";
            foreach (var single in numbers)
            {
                consignments += "'" + single + "',";
            }
            if (numbers.Length > 0)
            {
                consignments = consignments.Remove(consignments.Length - 1, 1);
            }
            DataTable dt_resp = GetConsignmentDetail(clvar, consignments);
            List<ReceiptTableModel> list = new List<ReceiptTableModel>();
            if (dt_resp != null)
            {
                if (dt_resp.Rows.Count > 0)//check ds length
                {
                    for (int i = 0; i < dt_resp.Rows.Count; i++)
                    {
                        ReceiptTableModel entry = new ReceiptTableModel();
                        chargedAmount = double.Parse(dt_resp.Rows[i]["chargedAmount"].ToString());
                        totalAmount = double.Parse(dt_resp.Rows[i]["totalAmount"].ToString());
                        gst = double.Parse(dt_resp.Rows[i]["gst"].ToString());

                        // --------- If Discount Applied ---------
                        if (dt_resp.Rows[i]["DiscountID"].ToString() != "")
                        {
                            // FOR PERCENTAGE DISCOUNT
                            if (dt_resp.Rows[i]["DiscountValueType"].ToString() == "1")
                            {
                                entry.lbl_discount = Math.Round(Math.Abs(chargedAmount - (totalAmount + gst)), 2).ToString();
                                entry.lbl_discount2 = Math.Round(Math.Abs(chargedAmount - (totalAmount + gst)), 2).ToString();
                                entry.lbl_chargedamount = Math.Round(Math.Abs(chargedAmount), 2).ToString("N0");
                                entry.lbl_chargedamount2 = Math.Round(Math.Abs(chargedAmount), 2).ToString("N0");

                                

                                string branch = dt_resp.Rows[i]["origin"].ToString();
                               
                                DataTable ProvinceName = Province(branch);

                                if(ProvinceName.Rows[0]["Name"].ToString() == "Balochistan")
                                {
                                    entry.lbl_gstpercentage1 = "BST: @15%";
                                    entry.lbl_gstpercentage2 = "BST: @15%";
                                    entry.lbl_ntn1 = "BTN: ";
                                    entry.lbl_ntn2 = "BTN: ";
                                }

                                if (ProvinceName.Rows[0]["Name"].ToString() == "Khyber Pakhtunkhwa")
                                {
                                    entry.lbl_gstpercentage1 = "KPST: @15%";
                                    entry.lbl_gstpercentage2 = "KPST: @15%";
                                    entry.lbl_ntn1 = "KNTN: ";
                                    entry.lbl_ntn2 = "KNTN: ";
                                }

                                if (ProvinceName.Rows[0]["Name"].ToString() == "Punjab")
                                {
                                    entry.lbl_gstpercentage1 = "PST: @16%";
                                    entry.lbl_gstpercentage2 = "PST: @16%";
                                    entry.lbl_ntn1 = "PNTN: ";
                                    entry.lbl_ntn2 = "PNTN: ";
                                }

                                if (ProvinceName.Rows[0]["Name"].ToString() == "Sindh")
                                {
                                    entry.lbl_gstpercentage1 = "SST: @13%";
                                    entry.lbl_gstpercentage2 = "SST: @13%";
                                    entry.lbl_ntn1 = "SNTN: ";
                                    entry.lbl_ntn2 = "SNTN: ";
                                }

                                if (dt_resp.Rows[i]["origin"].ToString() == "ISLAMABAD")
                                {
                                    entry.lbl_gstpercentage1 = "GST: @16%";
                                    entry.lbl_gstpercentage2 = "GST: @16%";
                                    entry.lbl_ntn1 = "NTN: ";
                                    entry.lbl_ntn2 = "NTN: ";
                                }


                                entry.lbl_gst = Math.Round(double.Parse(dt_resp.Rows[i]["gst"].ToString()), 2).ToString();
                                entry.lbl_gst2 = Math.Round(double.Parse(dt_resp.Rows[i]["gst"].ToString()), 2).ToString();
                                entry.lbl_amount = (Math.Round(double.Parse(dt_resp.Rows[i]["totalAmount"].ToString()), 2)).ToString();
                                entry.lbl_amount2 = (Math.Round(double.Parse(dt_resp.Rows[i]["totalAmount"].ToString()), 2)).ToString();

                                if (dt_resp.Rows[i]["priceModifierId"].ToString() == "98" || dt_resp.Rows[i]["priceModifierId"].ToString() == "100")
                                {
                                    entry.lbl_val2 = (Math.Round(double.Parse(dt_resp.Rows[i]["AlternateValue"].ToString()), 2)).ToString();
                                    entry.lbl_insurance2 = (Math.Round(double.Parse(dt_resp.Rows[i]["modifiedCalculationValue"].ToString()), 2)).ToString() + "%";

                                    entry.lbl_val = (Math.Round(double.Parse(dt_resp.Rows[i]["AlternateValue"].ToString()), 2)).ToString();
                                    entry.lbl_insurance = (Math.Round(double.Parse(dt_resp.Rows[i]["modifiedCalculationValue"].ToString()), 2)).ToString() + "%";
                                }

                                if (dt_resp.Rows[i]["SuppCharges"].ToString() != "")
                                {
                                    /*
                                    entry.lbl_amount = (Math.Round(double.Parse(dt_resp.Rows[i]["totalAmount"].ToString()), 2) + Math.Round(double.Parse(dt_resp.Rows[i]["SuppCharges"].ToString()), 2)).ToString();
                                    entry.lbl_amount2 = (Math.Round(double.Parse(dt_resp.Rows[i]["totalAmount"].ToString()), 2) + Math.Round(double.Parse(dt_resp.Rows[i]["SuppCharges"].ToString()), 2)).ToString();

                                    entry.lbl_gst = (Math.Round(double.Parse(dt_resp.Rows[i]["gst"].ToString()), 2) + Math.Round(double.Parse(dt_resp.Rows[i]["calculatedGST"].ToString()), 2)).ToString();
                                    entry.lbl_gst2 = (Math.Round(double.Parse(dt_resp.Rows[i]["gst"].ToString()), 2) + Math.Round(double.Parse(dt_resp.Rows[i]["calculatedGST"].ToString()), 2)).ToString();
                                    //entry.lbl_chargedamount = (Math.Round(Math.Abs(chargedAmount), 2).ToString() + Math.Round(double.Parse(dt_resp.Rows[i]["SuppCharges"].ToString()), 2) + Math.Round(double.Parse(dt_resp.Rows[i]["calculatedGST"].ToString()), 2)).ToString(); //double.Parse(dt_resp.Rows[i]["SuppCharges"].ToString())), 2).ToString();
                                    //entry.lbl_chargedamount2 = (Math.Round(Math.Abs(chargedAmount), 2).ToString() + Math.Round(double.Parse(dt_resp.Rows[i]["SuppCharges"].ToString()), 2) + Math.Round(double.Parse(dt_resp.Rows[i]["calculatedGST"].ToString()), 2)).ToString(); //double.Parse(dt_resp.Rows[i]["SuppCharges"].ToString())), 2).ToString();

                                    entry.lbl_chargedamount = (Math.Round(double.Parse(dt_resp.Rows[i]["chargedAmount"].ToString()), 2) + Math.Round(double.Parse(dt_resp.Rows[i]["SuppCharges"].ToString()), 2) + Math.Round(double.Parse(dt_resp.Rows[i]["calculatedGST"].ToString()), 2)).ToString("N0"); //double.Parse(dt_resp.Rows[i]["SuppCharges"].ToString())), 2).ToString();
                                    entry.lbl_chargedamount2 = (Math.Round(double.Parse(dt_resp.Rows[i]["chargedAmount"].ToString()), 2) + Math.Round(double.Parse(dt_resp.Rows[i]["SuppCharges"].ToString()), 2) + Math.Round(double.Parse(dt_resp.Rows[i]["calculatedGST"].ToString()), 2)).ToString("N0"); //double.Parse(dt_resp.Rows[i]["SuppCharges"].ToString())), 2).ToString();
                                */
                                }
                            }

                            // FOR AMOUNT DISCOUNT
                            if (dt_resp.Rows[i]["DiscountValueType"].ToString() == "2")
                            {
                                entry.lbl_discount = Math.Round(double.Parse(dt_resp.Rows[i]["DiscountValue"].ToString()), 2).ToString();
                                entry.lbl_discount2 = Math.Round(double.Parse(dt_resp.Rows[i]["DiscountValue"].ToString()), 2).ToString();
                                entry.lbl_chargedamount = Math.Round(Math.Abs(chargedAmount), 2).ToString("N0");
                                entry.lbl_chargedamount2 = Math.Round(Math.Abs(chargedAmount), 2).ToString("N0");
                                entry.lbl_gst = Math.Round(double.Parse(dt_resp.Rows[i]["gst"].ToString()), 2).ToString();
                                entry.lbl_gst2 = Math.Round(double.Parse(dt_resp.Rows[i]["gst"].ToString()), 2).ToString();

                                if (dt_resp.Rows[i]["SuppCharges"].ToString() != "")
                                {
                                    entry.lbl_gst = (Math.Round(double.Parse(dt_resp.Rows[i]["gst"].ToString()), 2) + Math.Round(double.Parse(dt_resp.Rows[i]["calculatedGST"].ToString()), 2)).ToString();
                                    entry.lbl_gst2 = (Math.Round(double.Parse(dt_resp.Rows[i]["gst"].ToString()), 2) + Math.Round(double.Parse(dt_resp.Rows[i]["calculatedGST"].ToString()), 2)).ToString();
                                    entry.lbl_chargedamount = (Math.Round(double.Parse(dt_resp.Rows[i]["totalAmount"].ToString()), 2) + Math.Round(double.Parse(dt_resp.Rows[i]["gst"].ToString()), 2) + Math.Round(double.Parse(dt_resp.Rows[i]["SuppCharges"].ToString()), 2) + Math.Round(double.Parse(dt_resp.Rows[i]["calculatedGST"].ToString()), 2)).ToString("N0"); //double.Parse(dt_resp.Rows[i]["SuppCharges"].ToString())), 2).ToString();
                                    entry.lbl_chargedamount2 = (Math.Round(double.Parse(dt_resp.Rows[i]["totalAmount"].ToString()), 2) + Math.Round(double.Parse(dt_resp.Rows[i]["gst"].ToString()), 2) + Math.Round(double.Parse(dt_resp.Rows[i]["SuppCharges"].ToString()), 2) + Math.Round(double.Parse(dt_resp.Rows[i]["calculatedGST"].ToString()), 2)).ToString("N0"); //double.Parse(dt_resp.Rows[i]["SuppCharges"].ToString())), 2).ToString();
                                }
                            }
                        }
                        // --------- If Discount Not Applied ---------
                        else
                        {
                            entry.lbl_discount = "N/A";
                            entry.lbl_discount2 = "N/A";
                            entry.lbl_gst = Math.Round(double.Parse(dt_resp.Rows[i]["gst"].ToString()), 2).ToString();
                            entry.lbl_gst2 = Math.Round(double.Parse(dt_resp.Rows[i]["gst"].ToString()), 2).ToString();
                            entry.lbl_chargedamount = (Math.Round(double.Parse(dt_resp.Rows[i]["totalAmount"].ToString()), 2) + Math.Round(double.Parse(dt_resp.Rows[i]["gst"].ToString()), 2)).ToString("N0");
                            entry.lbl_chargedamount2 = (Math.Round(double.Parse(dt_resp.Rows[i]["totalAmount"].ToString()), 2) + Math.Round(double.Parse(dt_resp.Rows[i]["gst"].ToString()), 2)).ToString("N0"); //double.Parse(dt_resp.Rows[i]["SuppCharges"].ToString())), 2).ToString();

                            entry.lbl_amount = (Math.Round(double.Parse(dt_resp.Rows[i]["totalAmount"].ToString()), 2)).ToString();
                            entry.lbl_amount2 = (Math.Round(double.Parse(dt_resp.Rows[i]["totalAmount"].ToString()), 2)).ToString();

                            string branch = dt_resp.Rows[i]["origin"].ToString();

                            DataTable ProvinceName = Province(branch);

                            if (ProvinceName.Rows[0]["Name"].ToString() == "Balochistan")
                            {
                                entry.lbl_gstpercentage1 = "BST: @15%";
                                entry.lbl_gstpercentage2 = "BST: @15%";
                                entry.lbl_ntn1 = "BTN: ";
                                entry.lbl_ntn2 = "BTN: ";
                            }

                            if (ProvinceName.Rows[0]["Name"].ToString() == "Khyber Pakhtunkhwa")
                            {
                                entry.lbl_gstpercentage1 = "KPST: @15%";
                                entry.lbl_gstpercentage2 = "KPST: @15%";
                                entry.lbl_ntn1 = "KNTN: ";
                                entry.lbl_ntn2 = "KNTN: ";
                            }

                            if (ProvinceName.Rows[0]["Name"].ToString() == "Punjab")
                            {
                                entry.lbl_gstpercentage1 = "PST: @16%";
                                entry.lbl_gstpercentage2 = "PST: @16%";
                                entry.lbl_ntn1 = "PNTN: ";
                                entry.lbl_ntn2 = "PNTN: ";
                            }

                            if (ProvinceName.Rows[0]["Name"].ToString() == "Sindh")
                            {
                                entry.lbl_gstpercentage1 = "SST: @13%";
                                entry.lbl_gstpercentage2 = "SST: @13%";
                                entry.lbl_ntn1 = "SNTN: ";
                                entry.lbl_ntn2 = "SNTN: ";
                            }

                            if(dt_resp.Rows[i]["origin"].ToString() == "ISLAMABAD")
                            {
                                entry.lbl_gstpercentage1 = "GST: @16%";
                                entry.lbl_gstpercentage2 = "GST: @16%";
                                entry.lbl_ntn1 = "NTN: ";
                                entry.lbl_ntn2 = "NTN: ";
                            }

                            if (dt_resp.Rows[i]["priceModifierId"].ToString() == "98" || dt_resp.Rows[i]["priceModifierId"].ToString() == "100")
                            {
                                entry.lbl_val2 = (Math.Round(double.Parse(dt_resp.Rows[i]["AlternateValue"].ToString()), 2)).ToString();
                                entry.lbl_insurance2 = (Math.Round(double.Parse(dt_resp.Rows[i]["modifiedCalculationValue"].ToString()), 2)).ToString() + "%";

                                entry.lbl_val = (Math.Round(double.Parse(dt_resp.Rows[i]["AlternateValue"].ToString()), 2)).ToString();
                                entry.lbl_insurance = (Math.Round(double.Parse(dt_resp.Rows[i]["modifiedCalculationValue"].ToString()), 2)).ToString() + "%";
                            }

                            if (dt_resp.Rows[i]["SuppCharges"].ToString() != "")
                            {
                                /*
                                entry.lbl_amount = (Math.Round(double.Parse(dt_resp.Rows[i]["totalAmount"].ToString()), 2) + Math.Round(double.Parse(dt_resp.Rows[i]["SuppCharges"].ToString()), 2)).ToString();
                                entry.lbl_amount2 = (Math.Round(double.Parse(dt_resp.Rows[i]["totalAmount"].ToString()), 2) + Math.Round(double.Parse(dt_resp.Rows[i]["SuppCharges"].ToString()), 2)).ToString();

                                entry.lbl_gst = (Math.Round(double.Parse(dt_resp.Rows[i]["gst"].ToString()), 2) + Math.Round(double.Parse(dt_resp.Rows[i]["calculatedGST"].ToString()), 2)).ToString();
                                entry.lbl_gst2 = (Math.Round(double.Parse(dt_resp.Rows[i]["gst"].ToString()), 2) + Math.Round(double.Parse(dt_resp.Rows[i]["calculatedGST"].ToString()), 2)).ToString();

                                entry.lbl_chargedamount = (Math.Round(double.Parse(dt_resp.Rows[i]["totalAmount"].ToString()), 2) + Math.Round(double.Parse(dt_resp.Rows[i]["gst"].ToString()), 2) + Math.Round(double.Parse(dt_resp.Rows[i]["SuppCharges"].ToString()), 2) + Math.Round(double.Parse(dt_resp.Rows[i]["calculatedGST"].ToString()), 2)).ToString("N0"); //double.Parse(dt_resp.Rows[i]["SuppCharges"].ToString())), 2).ToString();
                                entry.lbl_chargedamount2 = (Math.Round(double.Parse(dt_resp.Rows[i]["totalAmount"].ToString()), 2) + Math.Round(double.Parse(dt_resp.Rows[i]["gst"].ToString()), 2) + Math.Round(double.Parse(dt_resp.Rows[i]["SuppCharges"].ToString()), 2) + Math.Round(double.Parse(dt_resp.Rows[i]["calculatedGST"].ToString()), 2)).ToString("N0"); //double.Parse(dt_resp.Rows[i]["SuppCharges"].ToString())), 2).ToString();
                            */
                            }
                        }

                        entry.lbl_cnic_ntn1 = dt_resp.Rows[i]["consignerCNICNo"].ToString();
                        entry.lbl_cnic_ntn2 = dt_resp.Rows[i]["consignerCNICNo"].ToString();

                        entry.lbl_ridercode1 = "Booking Code: " + dt_resp.Rows[i]["ridercode"].ToString();
                        entry.lbl_ridercode2 = "Booking Code: " + dt_resp.Rows[i]["ridercode"].ToString();
                        
                        entry.lbl_b = dt_resp.Rows[i]["breadth"].ToString();
                        entry.lbl_b2 = dt_resp.Rows[i]["breadth"].ToString();
                        entry.lbl_booking = dt_resp.Rows[i]["bookingDate"].ToString();
                        entry.lbl_booking2 = dt_resp.Rows[i]["bookingDate"].ToString();
                        entry.lbl_cn = dt_resp.Rows[i]["consignmentNumber"].ToString();
                        entry.lbl_cn2 = dt_resp.Rows[i]["consignmentNumber"].ToString();
                        entry.lbl_consignee = dt_resp.Rows[i]["consignee"].ToString();
                        entry.lbl_consignee2 = dt_resp.Rows[i]["consignee"].ToString();
                        entry.lbl_consigneeaddress = dt_resp.Rows[i]["address"].ToString();
                        entry.lbl_consigneeaddress2 = dt_resp.Rows[i]["address"].ToString();
                        entry.lbl_consigneephone = dt_resp.Rows[i]["consigneePhoneNo"].ToString();
                        entry.lbl_consigneephone2 = dt_resp.Rows[i]["consigneePhoneNo"].ToString();
                        entry.lbl_dst = dt_resp.Rows[i]["destination"].ToString();
                        entry.lbl_dst2 = dt_resp.Rows[i]["destination"].ToString();
                        entry.lbl_h = dt_resp.Rows[i]["height"].ToString();
                        entry.lbl_h2 = dt_resp.Rows[i]["height"].ToString();
                        //entry.lbl_insurance = "";
                        //entry.lbl_insurance2 = "";
                        entry.lbl_origin = dt_resp.Rows[i]["origin"].ToString();
                        entry.lbl_origin2 = dt_resp.Rows[i]["origin"].ToString();
                        entry.lbl_Piece = dt_resp.Rows[i]["pieces"].ToString();
                        entry.lbl_Piece2 = dt_resp.Rows[i]["pieces"].ToString();
                        entry.lbl_ref = dt_resp.Rows[i]["ref"].ToString();
                        entry.lbl_ref2 = dt_resp.Rows[i]["ref"].ToString();
                        entry.lbl_shipper = dt_resp.Rows[i]["consigner"].ToString();
                        entry.lbl_shipper2 = dt_resp.Rows[i]["consigner"].ToString();
                        entry.lbl_shipperaddress = dt_resp.Rows[i]["shipperAddress"].ToString();
                        entry.lbl_shipperaddress2 = dt_resp.Rows[i]["shipperAddress"].ToString();
                        entry.lbl_shippercontact = dt_resp.Rows[i]["consignerPhoneNo"].ToString();
                        entry.lbl_shippercontact2 = dt_resp.Rows[i]["consignerPhoneNo"].ToString();
                        entry.lbl_user = dt_resp.Rows[i]["U_NAME"].ToString();
                        entry.lbl_user2 = dt_resp.Rows[i]["U_NAME"].ToString();
                        //entry.lbl_val = "";
                        //entry.lbl_val2 = "";
                        entry.lbl_w = dt_resp.Rows[i]["width"].ToString();
                        entry.lbl_w2 = dt_resp.Rows[i]["width"].ToString();
                        entry.lbl_weight = dt_resp.Rows[i]["weight"].ToString();
                        entry.lbl_weight2 = dt_resp.Rows[i]["weight"].ToString();
                        entry.lbl_supplementary2 = dt_resp.Rows[i]["SuppService"].ToString();
                        entry.lbl_supplementary = dt_resp.Rows[i]["SuppService"].ToString();
                        entry.lblService = dt_resp.Rows[i]["serviceTypeName"].ToString();
                        entry.lblService2 = dt_resp.Rows[i]["serviceTypeName"].ToString();
                        entry.lblPackageContents = dt_resp.Rows[i]["PakageContents"].ToString();
                        entry.lblPackageContents2 = dt_resp.Rows[i]["PakageContents"].ToString();
                        entry.lblSpecialInstructions = dt_resp.Rows[i]["remarks"].ToString();
                        entry.lblSpecialInstructions2 = dt_resp.Rows[i]["remarks"].ToString();
                        string cnNumber = dt_resp.Rows[i]["consignmentNumber"].ToString();

                        string location = CreateQRCode(cnNumber);
                        entry.ImageUrlQR = location;
                        entry.ImageUrlQR2 = location;
                        list.Add(entry);
                    }
                }
            }

            RepterDetails.DataSource = list;
            RepterDetails.DataBind();


        }
        public string CreateQRCode(string consignmentNumber)
        {
            try
            {
                //var url = string.Format("http://chart.apis.google.com/chart?cht=qr&chs={1}x{2}&chl={0}", "http://www.mulphilog.com/track-shipment.php?ConsignmentNumber=" + consignmentNumber, "350", "350");
                //WebResponse response = default(WebResponse);
                //Stream remoteStream = default(Stream);
                //StreamReader readStream = default(StreamReader);
                //WebRequest request = WebRequest.Create(url);
                //response = request.GetResponse();
                //remoteStream = response.GetResponseStream();
                //readStream = new StreamReader(remoteStream);
                //System.Drawing.Image img = System.Drawing.Image.FromStream(remoteStream);
                //string location = Server.MapPath("~/QRCodes/" + consignmentNumber + "_QR.png");
                //img.Save(location);
                //response.Close();
                //remoteStream.Close();
                //readStream.Close();
                return "~\\QRcodes\\QRImageNotFound.png";
            }
            catch (Exception er)
            {
                return "~\\QRcodes\\QRImageNotFound.png";
            }
        }
        public DataTable GetConsignmentDetail(Cl_Variables clvar, string consignmentNumbers)
        {
            string sql = "SELECT distinct \n"
            + "c.consignmentNumber, '' username, c.couponNumber ref,c.serviceTypeName, convert(varchar,c.bookingDate,13) bookingDate,b.name origin, b1.name destination,  \n"
            + "c.consignee, c.[address], c.consigneePhoneNo, c.consigner, c.shipperAddress,  c.consignerCellNo consignerPhoneNo, c.pieces, c.[weight], c.width, c.breadth, c.height, \n"
            + "c.totalAmount, c.gst, c.chargedAmount, c.remarks,c.PakageContents,zu.U_NAME, c.DiscountID ,pm.name [SuppService],cm.calculatedValue [SuppCharges], calculatedGST, \n"
            + "isnull(dm.DiscountValueType,'0') DiscountValueType, dm.DiscountValue, isnull(cm.AlternateValue,0)AlternateValue, isnull(modifiedCalculationValue,0) modifiedCalculationValue," 
            + "cm.priceModifierId, c.consignerCNICNo, C.RIDERCODE \n"
            + "FROM Consignment c  \n"
            + "INNER JOIN Branches b ON b.branchCode = c.orgin \n"
            + "INNER JOIN Branches b1 ON b1.branchCode = c.destination \n"
            + "INNER JOIN ZNI_USER1 zu ON c.createdby = zu.U_ID  \n"
            + "LEFT JOIN ConsignmentModifier cm ON cm.consignmentNumber=c.consignmentNumber \n "
            + " LEFT JOIN PriceModifiers pm  ON pm.id = cm.priceModifierId  \n"
            + "LEFT JOIN MNP_DiscountConsignment dc ON  c.consignmentNumber = dc.ConsignmentNumber \n"
            + "LEFT JOIN MnP_MasterDiscount dm ON  dc.DiscountID = dm.DiscountID \n"
            + "WHERE C.consignmentNumber in (" + consignmentNumbers + ")  \n "
            + "ORDER BY c.consignmentNumber  ";

            DataTable dt = new DataTable();
            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sql, orcl);
                orcd.CommandType = CommandType.Text;
                orcd.CommandTimeout = 300;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dt);
                orcl.Close();

                if (dt.Rows.Count > 0)
                {
                    for (int j = 0; j < dt.Rows.Count - 1; j++)
                    {
                        DataRow Currentdr = dt.Rows[j];
                        DataRow dr = dt.Rows[j + 1];
                        if (Currentdr["consignmentNumber"].ToString() == dr["consignmentNumber"].ToString())
                        {
                            String CurrentdrRowService = Currentdr["SuppService"].ToString();
                            String drService = dr["SuppService"].ToString();
                            float CurrentdrRowServicePrice = float.Parse(Currentdr["SuppCharges"].ToString());
                            float drServicePrice = float.Parse(dr["SuppCharges"].ToString());
                            float drcalculatedGST = float.Parse(dr["calculatedGST"].ToString());
                            float CurrentcalculatedGST = float.Parse(Currentdr["calculatedGST"].ToString());

                            Currentdr["SuppService"] = CurrentdrRowService + ", " + drService;
                            Currentdr["SuppCharges"] = (CurrentdrRowServicePrice + drServicePrice);
                            Currentdr["calculatedGST"] = (drcalculatedGST + CurrentcalculatedGST);

                            dr.Delete();
                            dt.AcceptChanges();
                            --j;
                        }
                    }
                }
            }
            catch (Exception er)
            { }

            return dt;
        }

        public DataTable Province(string branch)
        {

            string sqlString = @"SELECT p.Name FROM Branches AS b 
                                INNER JOIN Province AS p ON b.Province = p.ProvinceId
                                WHERE b.name = '"+ branch + @"'
                                AND b.[status] = '1'";

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
    }
}