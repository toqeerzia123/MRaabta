using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using MRaabta.App_Code;
using System.Collections;

namespace MRaabta.Files
{
    public partial class Manage_Invoices_COD : System.Web.UI.Page
    {

        Cl_Variables clvar = new Cl_Variables();
        CommonFunction func = new CommonFunction();
        Consignemnts con = new Consignemnts();
        protected void Page_Load(object sender, EventArgs e)
        {

            Errorid.Text = "";
            if (!IsPostBack)
            {
                getZones();
                getBillingCycle();
            }
        }
         
        public DataTable GetAllZones()
        {
            string query = "select zoneCode, name from Zones where status = '1' and region is not null  order by name";
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

        private void getZones()
        {
            DataTable dt = GetAllZones();
            if (dt.Rows.Count != 0)
            {
                ddl_zoneId.DataSource = dt;
                ddl_zoneId.DataTextField = "name";
                ddl_zoneId.DataValueField = "zoneCode";

                ddl_zoneId.DataBind();
                ddl_zoneId.Items.Insert(0, "Select Zone");
            }
        }
        private void getBillingCycle()
        {
            string query = "select id, name from stp_billingtype where isactive = 1";
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                sda.Fill(dt);
                if (dt.Rows.Count != 0)
                {
                    cbBillingCycle.DataTextField = "name";
                    cbBillingCycle.DataValueField = "id";
                    cbBillingCycle.DataSource = dt;
                    cbBillingCycle.DataBind();
                }
            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            
        }

        protected void btn_getCustomers_Click(object sender, EventArgs e)
        {
            Get_customerList();
            SelectAll.Checked = false;
        }
        protected void btn_save_Click(object sender, EventArgs e)
        {

            DataTable dt = Get_InvoiceEndCheck(HttpContext.Current.Session["ZONECODE"].ToString(), new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("yyyy-MM-dd"), DateTime.Now.ToString("yyyy-MM-dd"));
            if (dt.Rows.Count > 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Month Already Closed')", true);
                return;
            }
            ArrayList list = new ArrayList();
            double invoicecount = 0, invoicettl = 0;
            for (int i = 0; i < lb_CustomerList.Items.Count; i++)
            {
                if (lb_CustomerList.Items[i].Selected == true)
                {
                    string branchcode = "";
                    string sql = " select branchCode from CreditClients where id = " + lb_CustomerList.Items[i].Value;
                    SqlConnection con = new SqlConnection(clvar.Strcon());
                    SqlCommand cmd;
                    try
                    {
                        con.Open();
                        cmd = new SqlCommand(sql, con);
                        branchcode = cmd.ExecuteScalar().ToString();
                    }
                    catch (Exception ex)
                    { }
                    finally { con.Close(); }

                    invoicettl++;
                    clvar.Company = "1";
                    clvar.CreditClientID = lb_CustomerList.Items[i].Value;
                    clvar.FromDate = DateTime.Parse(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("yyyy-MM-dd"));
                    clvar.ToDate = DateTime.Now;
                    clvar.LoadingDate = DateTime.Now.ToString("yyyy-MM-dd");
                    clvar.Branch = branchcode;

                    string error = GenerateManualInvoice(clvar);
                    double number = 0;

                    double.TryParse(error, out number);
                    if (number != 0)
                        invoicecount++;
                }
            }

            // Displaying Msg
            string a = "";
            //for (int i = 0; i < list.Count; i++)
            //{
            //    a += (i + 1).ToString() + " " + list[i] + "\\n ";
            //}
            //a = "1 Customer :  8C57-01 CERO UNO (COD) Invoice No: Cannot Create Invoice\n 2 Customer :  841-4 SEASONS CLOTHES (COD) Invoice No: Cannot Create Invoice\n 3 Customer :  871-7 STAR WATCHES (COD) Invoice No: Cannot Create Invoice\n 4 Customer :  8810-8 BAZAR (COD) Invoice No: Cannot Create Invoice\n 5 Customer :  891-99 ONLINE SALES ( COD ) Invoice No: Cannot Create Invoice\n 6 Customer :  8A316-A - ORIGNAL.CO (COD) Invoice No: Cannot Create Invoice\n 7 Customer :  8A318-A 2 COLLECTION (COD) Invoice No: Cannot Create Invoice\n 8 Customer :  8A266-A A ENTERPRISES (COD) Invoice No: Cannot Create Invoice\n 9 Customer :  8A259-A MALL.PK (COD) Invoice No: Cannot Create Invoice\n 10 Customer :  8A312-A S MARKETING (COD) Invoice No: Cannot Create Invoice\n 11 Customer :  8A306-A S TEXTILE (COD) Invoice No: Cannot Create Invoice\n 12 Customer :  8A187-A Z ARTS ( COD ) Invoice No: Cannot Create Invoice\n 13 Customer :  8A315-A Z COLLECTION (COD) Invoice No: Cannot Create Invoice\n 14 Customer :  8A310-A ZAR TEXTILES PVT Ltd (COD) Invoice No: Cannot Create Invoice\n 15 Customer :  8A274-A.H. BAZAR (COD) Invoice No: Cannot Create Invoice\n 16 Customer :  8A277-A.H.F (COD) Invoice No: Cannot Create Invoice\n 17 Customer :  8A272-A.Z CREATIONS (COD) Invoice No: Cannot Create Invoice\n 18 Customer :  8A303-AA MARKETING (COD) Invoice No: Cannot Create Invoice\n 19 Customer :  8A283-AAA INTERNATIONAL (COD) Invoice No: Cannot Create Invoice\n 20 Customer :  8A161-AAHM ENTERPRICES (COD) Invoice No: Cannot Create Invoice\n 21 Customer :  8A258-AAMIR MEHMOOD (COD) Invoice No: Cannot Create Invoice\n 22 Customer :  8A169-AFORDIA BUSINESS SOLUTION ( COD ) Invoice No: Cannot Create Invoice\n 23 Customer :  8A127-AHMED TRADERS(TO PAY) COD FSD Invoice No: Cannot Create Invoice\n 24 Customer :  8A185-AIM ADVERTISER ( COD ) Invoice No: Cannot Create Invoice\n 25 Customer :  8A317-AL KARAMAT COLLECTION (COD) Invoice No: Cannot Create Invoice\n 26 Customer :  8A322-AL MUMTAZ BEDDING HOUSE (COD) Invoice No: Cannot Create Invoice\n 27 Customer :  8A324-AL TAQWEEM ONLINE STORE (COD) Invoice No: Cannot Create Invoice\n 28 Customer :  8A184-AL-HAMRA FABRICS.COM (COO) Invoice No: Cannot Create Invoice\n 29 Customer :  8A262-AL-NOOR BRANDS COLLECTION (COD) Invoice No: Cannot Create Invoice\n 30 Customer :  8A270-AL-REHMAN COLLECTION (COD) Invoice No: Cannot Create Invoice\n 31 Customer :  8A242-ALAYA FASHION STORE ( COD) Invoice No: Cannot Create Invoice\n 32 Customer :  8A231-ALBARKAT COUTURE ( COD ) Invoice No: Cannot Create Invoice\n 33 Customer :  8A308-ALI COLLECTION (COD) Invoice No: Cannot Create Invoice\n 34 Customer :  8A260-ALIF COLLECTIONS (COD) Invoice No: Cannot Create Invoice\n 35 Customer :  8A165-ALLOMORE.COM (COD) Invoice No: Cannot Create Invoice\n 36 Customer :  8A240-ALPHA BLUE (COD) Invoice No: Cannot Create Invoice\n 37 Customer :  8A271-AMERICAN MEDICIN (COD) Invoice No: Cannot Create Invoice\n 38 Customer :  8A252-AMIR SOHAIL (COD) Invoice No: Cannot Create Invoice\n 39 Customer :  8A299-AMMAN CLOTHING (COD) Invoice No: Cannot Create Invoice\n 40 Customer :  8A251-AMMAR COLLECTION (COD) Invoice No: Cannot Create Invoice\n 41 Customer :  8A265-AN REPLICA/EXPERT SALE POINT Invoice No: Cannot Create Invoice\n 42 Customer :  8A300-ANAYA LINEN (COD) Invoice No: Cannot Create Invoice\n 43 Customer :  8A241-ANCERU (COD) Invoice No: Cannot Create Invoice\n 44 Customer :  8A281-ANMOL JEWELERS (COD) Invoice No: Cannot Create Invoice\n 45 Customer :  8A297-ANMOL LIBAS COLLECTION (COD) Invoice No: Cannot Create Invoice\n 46 Customer :  8A246-ANOKHI DUKAN (COD) Invoice No: Cannot Create Invoice\n 47 Customer :  8A250-AREEJ FASHION LINE (COD) Invoice No: Cannot Create Invoice\n 48 Customer :  8A292-ASAAISH (COD) Invoice No: Cannot Create Invoice\n 49 Customer :  8A284-ASKARI CENTER (COD) Invoice No: Cannot Create Invoice\n 50 Customer :  8A146-AT YOUR DOOR STEP (COD) Invoice No: Cannot Create Invoice\n 51 Customer :  8A279-ATM (COD) Invoice No: Cannot Create Invoice\n 52 Customer :  8A245-AZHAR SHAH (COD) Invoice No: Cannot Create Invoice\n 53 Customer :  8B97-BABY SHABY (COD) Invoice No: Cannot Create Invoice\n 54 Customer :  8B86-BAKHIYA.PK (COD) Invoice No: Cannot Create Invoice\n 55 Customer :  8B48-BEE STONE COLLECTION (COD) Invoice No: Cannot Create Invoice\n 56 Customer :  8B92-BELLA STUDIO (COD) Invoice No: Cannot Create Invoice\n 57 Customer :  8B85-BELLEZA FASHION (COD) Invoice No: Cannot Create Invoice\n 58 Customer :  8B58-BEROYAL STORE (COD) Invoice No: Cannot Create Invoice\n 59 Customer :  8B78-BIN ILYAS FABRICS (COD) Invoice No: Cannot Create Invoice\n 60 Customer :  8B82-BISMILLAH COLLECTION (COD) Invoice No: Cannot Create Invoice\n 61 Customer :  8B47-BLUE B ENTERPRISES (COD) Invoice No: Cannot Create Invoice\n 62 Customer :  8B79-BLUE TAG FASHION (COD) Invoice No: Cannot Create Invoice\n 63 Customer :  8B89-BOTANICAL GLOW Invoice No: Cannot Create Invoice\n 64 Customer :  8B60-BRAND CIRCLE.PK (COD) Invoice No: Cannot Create Invoice\n 65 Customer :  8B45-BRAND ON-LINE CITY (COD) Invoice No: Cannot Create Invoice\n 66 Customer :  8B94-BRANDED CHOICE (COD) Invoice No: Cannot Create Invoice\n 67 Customer :  8B71-BRANDED CLOTHS (COD) Invoice No: Cannot Create Invoice\n 68 Customer :  8B93-BRANDS CLOTHING-FACTORY OUTLET (COD) Invoice No: Cannot Create Invoice\n 69 Customer :  8B81-BRANDS EAGLE (COD) Invoice No: Cannot Create Invoice\n 70 Customer :  8B59-BRANDS INN COLLECTION (COD) Invoice No: Cannot Create Invoice\n 71 Customer :  8B39-BRANDS ONLINE (COD) Invoice No: Cannot Create Invoice\n 72 Customer :  8F83-Brands Store (COD) Invoice No: Cannot Create Invoice\n 73 Customer :  8B40-BRANDSEGO.COM (COD) Invoice No: Cannot Create Invoice\n 74 Customer :  8B74-BURHAN FABRICS (COD) Invoice No: Cannot Create Invoice\n 75 Customer :  8B96-BURRAQ COLLECTION (COD) Invoice No: Cannot Create Invoice\n 76 Customer :  8C42-CAMELLIA OFFICIAL (COD) Invoice No: Cannot Create Invoice\n 77 Customer :  8C59-CHAUDHARY FABRICS (COD) Invoice No: Cannot Create Invoice\n 78 Customer :  8C53-CHAWLA FABRICS (COD) Invoice No: Cannot Create Invoice\n 79 Customer :  8C38-CHENAB STUFF (COD) Invoice No: Cannot Create Invoice\n 80 Customer :  8C61-CHERRY COSMATICS (COD) Invoice No: Cannot Create Invoice\n 81 Customer :  8C60-CIELO FABRICS (COD) Invoice No: Cannot Create Invoice\n 82 Customer :  8C51-CITY ONLINE STORE (COD) Invoice No: Cannot Create Invoice\n 83 Customer :  8C48-CITY SHOPPING STORE (COD) Invoice No: Cannot Create Invoice\n 84 Customer :  8C37-COD SHOPPING.PK Invoice No: Cannot Create Invoice\n 85 Customer :  8C56-CODSTORE.PK (COD) Invoice No: Cannot Create Invoice\n 86 Customer :  8C62-CRAFTS HOME (COD) Invoice No: Cannot Create Invoice\n 87 Customer :  8C10-CREATIONS (COD) Invoice No: Cannot Create Invoice\n 88 Customer :  8C46-CROSS CONNECTION RETAIL (Pvt) Ltd (COD) Invoice No: Cannot Create Invoice\n 89 Customer :  8C47-CROSS CONNECTION RETAIL (Pvt) Ltd (COD) Invoice No: Cannot Create Invoice\n 90 Customer :  8C44-CROSS LINE (COD) Invoice No: Cannot Create Invoice\n 91 Customer :  8C45-CUSTOM ZONE (COD) Invoice No: Cannot Create Invoice\n 92 Customer :  8D57-D STOCK (COD) Invoice No: Cannot Create Invoice\n 93 Customer :  8D54-DANIAS BOUTIQUE (COD) Invoice No: Cannot Create Invoice\n 94 Customer :  8D44-DEAL SHAKER.PK (COD) Invoice No: Cannot Create Invoice\n 95 Customer :  8D40-Deeds .PK (COD) Invoice No: Cannot Create Invoice\n 96 Customer :  8D65-DELUXE SUITING (COD) Invoice No: Cannot Create Invoice\n 97 Customer :  8D58-DESIGNER EXPRESS (COD) Invoice No: Cannot Create Invoice\n 98 Customer :  8D46-DESIRE FASHION ( COD ) Invoice No: Cannot Create Invoice\n 99 Customer :  8D63-DHANAK BOUTIQUE (COD) Invoice No: Cannot Create Invoice\n 100 Customer :  8D51-DIGITAL TRADERS (COD) Invoice No: Cannot Create Invoice\n 101 Customer :  8D59-DIVINE COLLECTION (COD) Invoice No: Cannot Create Invoice\n 102 Customer :  8D49-DRESSIFY.PK ( COD ) Invoice No: Cannot Create Invoice\n 103 Customer :  8E46-E BEAUTY STORE (COD) Invoice No: Cannot Create Invoice\n 104 Customer :  8E26-E-TIJARAT.PK Invoice No: Cannot Create Invoice\n 105 Customer :  8E33-EASTERN COSMETICS.COM.PK (COD) Invoice No: Cannot Create Invoice\n 106 Customer :  8E29-EASTROBE.COM (COD) Invoice No: Cannot Create Invoice\n 107 Customer :  8E24-EASY SKY SHOP (COD) Invoice No: Cannot Create Invoice\n 108 Customer :  8E43-ELEGANT SHOP (COD) Invoice No: Cannot Create Invoice\n 109 Customer :  8E30-ELITE STORE (COD) Invoice No: Cannot Create Invoice\n 110 Customer :  8E49-EMAAN BEDSHEET (COD) Invoice No: Cannot Create Invoice\n 111 Customer :  8E10-EMAAN ENTERPRISES (COD)  Invoice No: Cannot Create Invoice\n 112 Customer :  8E25-EMAAN SHAKEEL ONLOINE STORE (COD) Invoice No: Cannot Create Invoice\n 113 Customer :  8E37-EMAAN ZARA (COD) Invoice No: Cannot Create Invoice\n 114 Customer :  8E42-Emaan Zara Clothing (COD) Invoice No: Cannot Create Invoice\n 115 Customer :  8E48-EMAAN ZARAS (COD) Invoice No: Cannot Create Invoice\n 116 Customer :  8E38-EPOSH.PK (COD) Invoice No: Cannot Create Invoice\n 117 Customer :  8E44-ESTATIONERS (COD) Invoice No: Cannot Create Invoice\n 118 Customer :  8E35-EVONIA ATTIRE (COD) Invoice No: Cannot Create Invoice\n 119 Customer :  8E36-EXCLUSIVE BRANDS (COD) Invoice No: Cannot Create Invoice\n 120 Customer :  8E39-EXPORT BRANDS (COD) Invoice No: Cannot Create Invoice\n 121 Customer :  8E40-EXPORT DEN (COD) Invoice No: Cannot Create Invoice\n 122 Customer :  8E45-EXPORT FITTERS (COD) Invoice No: Cannot Create Invoice\n 123 Customer :  8E12-EXPORTS CLUB (COD) Invoice No: Cannot Create Invoice\n 124 Customer :  8E21-EXPORTS HUB Invoice No: Cannot Create Invoice\n 125 Customer :  8E27-EXPRESS BRAND (COD) Invoice No: Cannot Create Invoice\n 126 Customer :  8F105-F & F CLOTHING (COD) Invoice No: Cannot Create Invoice\n 127 Customer :  8F95-FABULOUS COLLECTION (COD) Invoice No: Cannot Create Invoice\n 128 Customer :  8F103-FAISALABAD CLOTH FASHION (COD) Invoice No: Cannot Create Invoice\n 129 Customer :  8F96-FAIZASTORE.COM (COD) Invoice No: Cannot Create Invoice\n 130 Customer :  8F84-FALAH BIO PRODUCT (COD ) Invoice No: Cannot Create Invoice\n 131 Customer :  8F114-FASH INN (COD) Invoice No: Cannot Create Invoice\n 132 Customer :  8F89-FASHION CLOTHES (COD) Invoice No: Cannot Create Invoice\n 133 Customer :  8F101-FASHION COLLECTION (COD) Invoice No: Cannot Create Invoice\n 134 Customer :  8F94-FASHION HARBOUR (COD) Invoice No: Cannot Create Invoice\n 135 Customer :  8F108-FASHION HOUSE (COD) Invoice No: Cannot Create Invoice\n 136 Customer :  8F97-FASHION HUB (COD) Invoice No: Cannot Create Invoice\n 137 Customer :  8F85-FASHION HUB (COD) Invoice No: Cannot Create Invoice\n 138 Customer :  8F87-FASHION LIGHTS ( COD ) FSD Invoice No: Cannot Create Invoice\n 139 Customer :  8F107-FASHION MANIA (COD) Invoice No: Cannot Create Invoice\n 140 Customer :  8F82-FASHION MORE (COD) Invoice No: Cannot Create Invoice\n 141 Customer :  8F78-FASHION REBON (COD) Invoice No: Cannot Create Invoice\n 142 Customer :  8F77-FASHION SQUARE.PK  (COD) Invoice No: Cannot Create Invoice\n 143 Customer :  8F74-FASHION TRENDS (COD) Invoice No: Cannot Create Invoice\n 144 Customer :  8F72-FASHION VALLEY (COD) Invoice No: Cannot Create Invoice\n 145 Customer :  8F111-FASHION VOGUE (COD) Invoice No: Cannot Create Invoice\n 146 Customer :  8F86-FASHION ZONE (COD) Invoice No: Cannot Create Invoice\n 147 Customer :  8F115-FASHIONREVOLUTION.PK (COD) Invoice No: Cannot Create Invoice\n 148 Customer :  8F66-FASION FAIRY ( COD ) Invoice No: Cannot Create Invoice\n 149 Customer :  8F90-FASIONISTA (COD) Invoice No: Cannot Create Invoice\n 150 Customer :  8F112-FATIMA BOUTIQ (COD) Invoice No: Cannot Create Invoice\n 151 Customer :  8F113-FEMALE CHOICE (COD) Invoice No: Cannot Create Invoice\n 152 Customer :  8F67-FEMALE CHOICE (COD) Invoice No: Cannot Create Invoice\n 153 Customer :  8F64-FEMALE DRESS COLLECTION (COD) Invoice No: Cannot Create Invoice\n 154 Customer :  8F100-FK FABRICS (COD) Invoice No: Cannot Create Invoice\n 155 Customer :  8F91-FLAGSHIP COMPANY (COD) Invoice No: Cannot Create Invoice\n 156 Customer :  8F60-FLIP ART (COD) Invoice No: Cannot Create Invoice\n 157 Customer :  300014-FOC Account – INTL (Import) Invoice No: Cannot Create Invoice\n 158 Customer :  8F62-FOSPK (COD) Invoice No: Cannot Create Invoice\n 159 Customer :  8F98-FRILLEE ACCESSORIES  Invoice No: Cannot Create Invoice\n 160 Customer :  8F110-FRS FULFILMENT CENTER ( COD) Invoice No: Cannot Create Invoice\n 161 Customer :  8F75-FSD  BAZAR (COD) Invoice No: Cannot Create Invoice\n 162 Customer :  8F102-FUTURE FASHION FABRICS (COD) Invoice No: Cannot Create Invoice\n 163 Customer :  8G27-GARYALS BOUTIQUE Invoice No: Cannot Create Invoice\n 164 Customer :  8G20-GET 24 SEVEN (COD) Invoice No: Cannot Create Invoice\n 165 Customer :  8G29-GET IN STYLE (COD) Invoice No: Cannot Create Invoice\n 166 Customer :  8G25-GHANI CLOTH HOUSE (COD) Invoice No: Cannot Create Invoice\n 167 Customer :  8G24-GHOUS WARIS ARTS (COD) Invoice No: Cannot Create Invoice\n 168 Customer :  8G28-GLORIOUS FASHION HUB (COD) Invoice No: Cannot Create Invoice\n 169 Customer :  8G30-GODAAM.PK (COD) Invoice No: Cannot Create Invoice\n 170 Customer :  8G22-GRACE STORE.PK ( COD ) Invoice No: Cannot Create Invoice\n 171 Customer :  8G16-GURGABI.PK (COD) Invoice No: Cannot Create Invoice\n 172 Customer :  8H61-H&H COSMTICS (COD) Invoice No: Cannot Create Invoice\n 173 Customer :  8H59-HAFIZ DRY FRUIT (COD) Invoice No: Cannot Create Invoice\n 174 Customer :  8H58-HAPPY BAZAR (COD) Invoice No: Cannot Create Invoice\n 175 Customer :  8H53-HAREEM FATIMA (COD) Invoice No: Cannot Create Invoice\n 176 Customer :  8H49-HARIS ARTS ( COD ) Invoice No: Cannot Create Invoice\n 177 Customer :  8H73-HASEEB PIGEONS RINGS DEALER (COD) Invoice No: Cannot Create Invoice\n 178 Customer :  8H44-HASHTAG ENTERPRISES (COD) Invoice No: Cannot Create Invoice\n 179 Customer :  8H54-HEALTH CARE ONLINE (COD) Invoice No: Cannot Create Invoice\n 180 Customer :  8H63-HK FABRICS (COD) Invoice No: Cannot Create Invoice\n 181 Customer :  8H68-HOME IDEAS (COD) Invoice No: Cannot Create Invoice\n 182 Customer :  8H74-HOMES SHOP 24 (COD) Invoice No: Cannot Create Invoice\n 183 Customer :  8H60-HOPIO.PK (COD) Invoice No: Cannot Create Invoice\n 184 Customer :  8H50-HOUSE OF AYESHA AHMED ( COD ) Invoice No: Cannot Create Invoice\n 185 Customer :  8H55-HOUSE OF REPLICA ( COD ) Invoice No: Cannot Create Invoice\n 186 Customer :  8H62-HQC TRADERS (COD) Invoice No: Cannot Create Invoice\n 187 Customer :  8H67-HUM VIP.COM (COD) Invoice No: Cannot Create Invoice\n 188 Customer :  8H75-HUMAN LIFE SHOPING STORE (COD) Invoice No: Cannot Create Invoice\n 189 Customer :  8H71-HUZZON (COD) Invoice No: Cannot Create Invoice\n 190 Customer :  8I51-ICONIC ARTS (COD) Invoice No: Cannot Create Invoice\n 191 Customer :  8I36-IDEAL GIFT CENTER Invoice No: Cannot Create Invoice\n 192 Customer :  8I56-IDEAL GRAND STORE (COD) Invoice No: Cannot Create Invoice\n 193 Customer :  8I57-IDEAS BY MOON (COD) Invoice No: Cannot Create Invoice\n 194 Customer :  8I39-IMAGE GARMENTS (PVT) LTD (EXPORT LEFTOVERS) COD Invoice No: Cannot Create Invoice\n 195 Customer :  8I50-INVOICE PAY.PK (COD) Invoice No: Cannot Create Invoice\n 196 Customer :  8I47-IRAH COLLECTION (COD) Invoice No: Cannot Create Invoice\n 197 Customer :  8I55-IRAH COLLECTION (COD) Invoice No: Cannot Create Invoice\n 198 Customer :  8I53-ITTEHAD TEXTILE INDUSTRIES (PVT) LTD (COD) Invoice No: Cannot Create Invoice\n 199 Customer :  8J19-JABRONDEALS.COM (COD) Invoice No: Cannot Create Invoice\n 200 Customer :  8J16-JAWAD QASIM ( COD) Invoice No: Cannot Create Invoice\n 201 Customer :  8J22-JBANTOO (COD) Invoice No: Cannot Create Invoice\n 202 Customer :  8J24-JK BY JAVED KHADDAR (COD) Invoice No: Cannot Create Invoice\n 203 Customer :  8J25-JUST DOT OUTFITS (COD) Invoice No: Cannot Create Invoice\n 204 Customer :  8K24-KAINAAT COLLECTION (COD) Invoice No: Cannot Create Invoice\n 205 Customer :  8K34-KAPRAY.PK (COD) Invoice No: Cannot Create Invoice\n 206 Customer :  8K30-KARACHI CLOTH (COD) Invoice No: Cannot Create Invoice\n 207 Customer :  8K21-KARAM CLOTHING ( COD ) Invoice No: Cannot Create Invoice\n 208 Customer :  8K38-KAYRAN (COD) Invoice No: Cannot Create Invoice\n 209 Customer :  8K28-KB TRADERS (COD) Invoice No: Cannot Create Invoice\n 210 Customer :  8K32-KHAN TRADERS (COD) Invoice No: Cannot Create Invoice\n 211 Customer :  8K22-KHAS STORE (COD) Invoice No: Cannot Create Invoice\n 212 Customer :  8K31-KHAWAJA DIAMOND (COD) Invoice No: Cannot Create Invoice\n 213 Customer :  8K26-KIDZANIA (COD) Invoice No: Cannot Create Invoice\n 214 Customer :  8K23-KOHSAAR TRADERS (COD) Invoice No: Cannot Create Invoice\n 215 Customer :  8L33-LADIES BRANDS (COD) Invoice No: Cannot Create Invoice\n 216 Customer :  8L17-LADIES CHOICE (COD) Invoice No: Cannot Create Invoice\n 217 Customer :  8L25-LADIES COLLECTION (COD) Invoice No: Cannot Create Invoice\n 218 Customer :  8L22-LADIES FASHION (COD) Invoice No: Cannot Create Invoice\n 219 Customer :  8L18-LADIES LAND (COD) Invoice No: Cannot Create Invoice\n 220 Customer :  8L21-LADIES OWN STORE (COD) Invoice No: Cannot Create Invoice\n 221 Customer :  8L16-LADIES SHOP (COD)  Invoice No: Cannot Create Invoice\n 222 Customer :  8L28-LADY LIKE (COD) Invoice No: Cannot Create Invoice\n 223 Customer :  8L26-LEATHER HEIGHTS (COD) Invoice No: Cannot Create Invoice\n 224 Customer :  8L24-LIBASS KHAS (COD) Invoice No: Cannot Create Invoice\n 225 Customer :  8L30-LIBASS TANN (CLOTHING BRAND) COD Invoice No: Cannot Create Invoice\n 226 Customer :  8L23-LONDON BRIDGE (COD) Invoice No: Cannot Create Invoice\n 227 Customer :  8L20-LOVE HUNT ( COD ) Invoice No: Cannot Create Invoice\n 228 Customer :  8L29-LUXURY MADE (COD) Invoice No: Cannot Create Invoice\n 229 Customer :  8M145-M & S TEXTILES (COD) Invoice No: Cannot Create Invoice\n 230 Customer :  8M151-M SAEED ASKARI (COD) Invoice No: Cannot Create Invoice\n 231 Customer :  8M102-M. YASIR MEHMOOD (COD) Invoice No: Cannot Create Invoice\n 232 Customer :  8M132-M.F TEXTILE (COD) Invoice No: Cannot Create Invoice\n 233 Customer :  8M137-M/S 99.CHOICE (COD) Invoice No: Cannot Create Invoice\n 234 Customer :  8M114-M/S ARTS ( COD ) Invoice No: Cannot Create Invoice\n 235 Customer :  8M93-M/S MARCA LEFTOVERS (COD) Invoice No: Cannot Create Invoice\n 236 Customer :  8F53-Mahi Clothing Store (COD) Invoice No: Cannot Create Invoice\n 237 Customer :  8M143-MAHI CLOTHING STORE (COD) Invoice No: Cannot Create Invoice\n 238 Customer :  8M135-MAJESTIC COLLECTION (COD) Invoice No: Cannot Create Invoice\n 239 Customer :  8M111-MAKE UP EVER.PK (COD) Invoice No: Cannot Create Invoice\n 240 Customer :  8M95-MALL OF DREAMS (COD) Invoice No: Cannot Create Invoice\n 241 Customer :  8M115-MASTER REPLICA (COD) Invoice No: Cannot Create Invoice\n 242 Customer :  8M138-MASTER REPLICA CLOTHING (COD) Invoice No: Cannot Create Invoice\n 243 Customer :  8M97-MASTER REPLICA STORE (COD) Invoice No: Cannot Create Invoice\n 244 Customer :  8M150-MASTER REPLICA WHOLE SALE (COD) Invoice No: Cannot Create Invoice\n 245 Customer :  8M122-MAYAAR ONLINE (COD) Invoice No: Cannot Create Invoice\n 246 Customer :  8M136-MD ONLINE SHOP (COD) Invoice No: Cannot Create Invoice\n 247 Customer :  8M110-MEDIEVAL REBORN ( COD ) Invoice No: Cannot Create Invoice\n 248 Customer :  8M157-MIR COLLECTION (COD) Invoice No: Cannot Create Invoice\n 249 Customer :  8M105-MISTORE.PK (SMART LINK TECHNOLOGIES) (COD) Invoice No: Cannot Create Invoice\n 250 Customer :  8M112-MR FASHION.PK ( COD ) Invoice No: Cannot Create Invoice\n 251 Customer :  8M119-MS ARTS (COD) Invoice No: Cannot Create Invoice\n 252 Customer :  8M121-MS TRADERS (COD) Invoice No: Cannot Create Invoice\n 253 Customer :  8M128-MUHAMMAD SHAH SB (COD) Invoice No: Cannot Create Invoice\n 254 Customer :  8M124-MUHIBBA COLLECTION (COD) Invoice No: Cannot Create Invoice\n 255 Customer :  8M125-MUHIBBA COLLECTION (COD) Invoice No: Cannot Create Invoice\n 256 Customer :  8M126-MUHIBBA COLLECTION (COD) Invoice No: Cannot Create Invoice\n 257 Customer :  8M127-MUHIBBA COLLECTION (COD) Invoice No: Cannot Create Invoice\n 258 Customer :  8M153-MURAASH (COD) Invoice No: Cannot Create Invoice\n 259 Customer :  8M116-MURAD FABRICS (COD) Invoice No: Cannot Create Invoice\n 260 Customer :  8M118-MUSTAFAI COLLECTION  (COD) Invoice No: Cannot Create Invoice\n 261 Customer :  8M134-MUTAYYAB & CO (COD) Invoice No: Cannot Create Invoice\n 262 Customer :  8M98-MY HOME STORE (COD) Invoice No: Cannot Create Invoice\n 263 Customer :  8M149-MY HOME STYLE (COD) Invoice No: Cannot Create Invoice\n 264 Customer :  8N79-NAFEES BAZAR (COD) Invoice No: Cannot Create Invoice\n 265 Customer :  8N70-NATURES NECTOR CO (COD) Invoice No: Cannot Create Invoice\n 266 Customer :  8N80-NAWAZ COLLECTION (COD) Invoice No: Cannot Create Invoice\n 267 Customer :  8N58-NEW FAROOQ ELECTRONICS (COD) Invoice No: Cannot Create Invoice\n 268 Customer :  8N64-NIZAAM LABORATORIES ( COD ) Invoice No: Cannot Create Invoice\n 269 Customer :  8N69-NMH AMERICA INC (COD) Invoice No: Cannot Create Invoice\n 270 Customer :  8N78-NOMAN TRADER (COD) Invoice No: Cannot Create Invoice\n 271 Customer :  8N60-NUTRI FACTOR HEALTH CARE (COD) Invoice No: Cannot Create Invoice\n 272 Customer :  8O18-ONE STITCH REPLICA (COD) Invoice No: Cannot Create Invoice\n 273 Customer :  8O14-ONIEO.COM (COD) Invoice No: Cannot Create Invoice\n 274 Customer :  8O20-ONLINE AT STORE (COD) Invoice No: Cannot Create Invoice\n 275 Customer :  8O16-ONLINE SHOPPING CENTER (COD) Invoice No: Cannot Create Invoice\n 276 Customer :  8O7-ONLINE TRADERS FSD Invoice No: Cannot Create Invoice\n 277 Customer :  8O23-ORGANIC ESSENTIALS (COD) Invoice No: Cannot Create Invoice\n 278 Customer :  8O12-ORIFLAME PAKISTAN 1 (COD) Invoice No: Cannot Create Invoice\n 279 Customer :  8O13-OUTFIT7.PK ( COD ) Invoice No: Cannot Create Invoice\n 280 Customer :  8O19-OUTFITS.PK (COD) Invoice No: Cannot Create Invoice\n 281 Customer :  8O21-OUTLET FACTORY (COD) Invoice No: Cannot Create Invoice\n 282 Customer :  8P55-PAKISTAN CLOTHING (COD) Invoice No: Cannot Create Invoice\n 283 Customer :  8P67-PAKISTAN FASHION LINE (COD) Invoice No: Cannot Create Invoice\n 284 Customer :  8P53-PAKISTANI CARFT (COD) Invoice No: Cannot Create Invoice\n 285 Customer :  8P70-PAKISTANI REPLICA (COD) Invoice No: Cannot Create Invoice\n 286 Customer :  8P66-PARI CLOTHES (COD) Invoice No: Cannot Create Invoice\n 287 Customer :  8P57-PEARL FABRICS (COD) Invoice No: Cannot Create Invoice\n 288 Customer :  8P50-PEHNAWA (COD) Invoice No: Cannot Create Invoice\n 289 Customer :  8P64-PEHNAWA 69 (COD) Invoice No: Cannot Create Invoice\n 290 Customer :  8O22-PERIDOT OUTFITS (COD) Invoice No: Cannot Create Invoice\n 291 Customer :  8P59-PK VALLY (COD) Invoice No: Cannot Create Invoice\n 292 Customer :  8P68-POSH APPAREL (COD) Invoice No: Cannot Create Invoice\n 293 Customer :  8P61-POSH.PK (COD) Invoice No: Cannot Create Invoice\n 294 Customer :  8P71-POSHAK COLLECTION (COD) Invoice No: Cannot Create Invoice\n 295 Customer :  8P60-POWER MATES (COD) Invoice No: Cannot Create Invoice\n 296 Customer :  8P54-PYARI  COLLECTION (COD) Invoice No: Cannot Create Invoice\n 297 Customer :  8Q8-QBN COLLECTION (COD) Invoice No: Cannot Create Invoice\n 298 Customer :  8Q9-QUEENS CLOTHES (COD) Invoice No: Cannot Create Invoice\n 299 Customer :  8Q11-QUEENS FASHION (COD) Invoice No: Cannot Create Invoice\n 300 Customer :  8R51-R J COLLECTION (COD) Invoice No: Cannot Create Invoice\n 301 Customer :  8R44-RAMEEN COLLECTION (COD) Invoice No: Cannot Create Invoice\n 302 Customer :  8R29-RANG BHAAR COLLECTION (COD) Invoice No: Cannot Create Invoice\n 303 Customer :  8R26-RAPLICA MALL (COD) Invoice No: Cannot Create Invoice\n 304 Customer :  8R31-RAWAJ MAHAL (COD) Invoice No: Cannot Create Invoice\n 305 Customer :  8R10-RCG CLOTHING GALLERY COD ACCOUNT Invoice No: Cannot Create Invoice\n 306 Customer :  8R40-REAL LEATHER (COD) Invoice No: Cannot Create Invoice\n 307 Customer :  8R33-RECTO BIZ (COD) Invoice No: Cannot Create Invoice\n 308 Customer :  8R30-REPLICA BAZAR (COD) Invoice No: Cannot Create Invoice\n 309 Customer :  8R38-REVEUSE SLEEP & LOUNGE WEAR (COD) Invoice No: Cannot Create Invoice\n 310 Customer :  8R39-REVEUSE SLEEP & LOUNGE WEAR (COD) Invoice No: Cannot Create Invoice\n 311 Customer :  8R52-RIVAYAT (COD) Invoice No: Cannot Create Invoice\n 312 Customer :  8R54-RIYAN TRADERS (COD) Invoice No: Cannot Create Invoice\n 313 Customer :  8R37-ROYAL CARS (COD) Invoice No: Cannot Create Invoice\n 314 Customer :  8R46-ROYALS CLOTHING (COD) Invoice No: Cannot Create Invoice\n 315 Customer :  8S95-S & K BOUTIQUE ( COD ) Invoice No: Cannot Create Invoice\n 316 Customer :  8S159-SA WEARS (COD) Invoice No: Cannot Create Invoice\n 317 Customer :  8S86-SAAMAAN.PK (COD) Invoice No: Cannot Create Invoice\n 318 Customer :  8S152-SADDIQUE COLLECTION Invoice No: Cannot Create Invoice\n 319 Customer :  8S106-SAIMA COLLECTION (COD) Invoice No: Cannot Create Invoice\n 320 Customer :  8S148-SAMREEN COLLECTION (COD) Invoice No: Cannot Create Invoice\n 321 Customer :  8S114-SANAAYYAN.PK (COD) Invoice No: Cannot Create Invoice\n 322 Customer :  8S115-SANAS FASHION HUB (COD) Invoice No: Cannot Create Invoice\n 323 Customer :  8S84-SANOOR (PVT) LTD (COD) Invoice No: Cannot Create Invoice\n 324 Customer :  8S167-SAQAFAT OFFICIAL (COD) Invoice No: Cannot Create Invoice\n 325 Customer :  8S128-SARA.B OFFICIAL (COD0 Invoice No: Cannot Create Invoice\n 326 Customer :  8S129-SD ONLINE STORE (COD) Invoice No: Cannot Create Invoice\n 327 Customer :  8S138-SEASON WEAR (COD) Invoice No: Cannot Create Invoice\n 328 Customer :  8S150-SECRET COLLECTIONS (COD) Invoice No: Cannot Create Invoice\n 329 Customer :  8S139-SEEBI FASHION (COD) Invoice No: Cannot Create Invoice\n 330 Customer :  8S101-SHADE 31 (COD) Invoice No: Cannot Create Invoice\n 331 Customer :  8S130-SHAFIQ ARTS (COD) Invoice No: Cannot Create Invoice\n 332 Customer :  8S154-SHAH APPARELS (COD) Invoice No: Cannot Create Invoice\n 333 Customer :  8S160-SHAHEEN STORE (COD) Invoice No: Cannot Create Invoice\n 334 Customer :  8S133-SHAHID IQBAL (COD) Invoice No: Cannot Create Invoice\n 335 Customer :  8S157-SHAM TRADER (COD) Invoice No: Cannot Create Invoice\n 336 Customer :  8S103-SHE GEE FASHION (COD) Invoice No: Cannot Create Invoice\n 337 Customer :  8S142-SHIRT TO SHOE (COD) Invoice No: Cannot Create Invoice\n 338 Customer :  8S127-SHOP AT YOUR HOME (COD) Invoice No: Cannot Create Invoice\n 339 Customer :  8S134-SHOP ONLINE (COD) Invoice No: Cannot Create Invoice\n 340 Customer :  8S75-SHOP ONLINE (COD) Invoice No: Cannot Create Invoice\n 341 Customer :  8S124-SHOPPING CITY (COD) Invoice No: Cannot Create Invoice\n 342 Customer :  8S112-SHOPPING PLANNET (COD) Invoice No: Cannot Create Invoice\n 343 Customer :  8S170-SHOPS BIZ (COD) Invoice No: Cannot Create Invoice\n 344 Customer :  8S161-SIDDIQUE TRADERS (COD) Invoice No: Cannot Create Invoice\n 345 Customer :  8S155-SIDRAZ COLLECTION (COD) Invoice No: Cannot Create Invoice\n 346 Customer :  8S147-SIMPLY PURE (PVT) Ltd (COD) Invoice No: Cannot Create Invoice\n 347 Customer :  8S82-SIMPLY THE GREAT FOOD (COD) Invoice No: Cannot Create Invoice\n 348 Customer :  8S121-SIMPLY THE GREAT FOOD (PVT) LTD (COD) Invoice No: Cannot Create Invoice\n 349 Customer :  8S110-SMART COLLECTION (COD) Invoice No: Cannot Create Invoice\n 350 Customer :  43S151-Smart Fusion Technologies Pvt Ltd Invoice No: Cannot Create Invoice\n 351 Customer :  8S107-SN STORE (COD) Invoice No: Cannot Create Invoice\n 352 Customer :  8S122-SNAP SHOP (COD) Invoice No: Cannot Create Invoice\n 353 Customer :  8S158-SONI SMART COLLECTION (COD) Invoice No: Cannot Create Invoice\n 354 Customer :  8S165-SRC CLOTH (COD) Invoice No: Cannot Create Invoice\n 355 Customer :  8S126-SS WEAR (COD) Invoice No: Cannot Create Invoice\n 356 Customer :  8S163-SSA EXPERIENCE (COD) Invoice No: Cannot Create Invoice\n 357 Customer :  8S145-STAR INTERNATIONAL (COD) Invoice No: Cannot Create Invoice\n 358 Customer :  8S100-STARDOM COLLECTION (COD) Invoice No: Cannot Create Invoice\n 359 Customer :  8S120-STYLE LOFT.PK Invoice No: Cannot Create Invoice\n 360 Customer :  8S164-STYLEINS (COD) Invoice No: Cannot Create Invoice\n 361 Customer :  8S104-STYLISH ENTERPRISES (COD) Invoice No: Cannot Create Invoice\n 362 Customer :  8S89-STYLISH STORE (COD) Invoice No: Cannot Create Invoice\n 363 Customer :  8S168-STYLISH WARDROBE (COD) Invoice No: Cannot Create Invoice\n 364 Customer :  8S151-STYLISH WEARS (COD) Invoice No: Cannot Create Invoice\n 365 Customer :  8S144-SUITING STUDIO (COD) Invoice No: Cannot Create Invoice\n 366 Customer :  8T67-TAB E MAH COLLECTION (COD) Invoice No: Cannot Create Invoice\n 367 Customer :  8T52-TAP N CARRY (COD) Invoice No: Cannot Create Invoice\n 368 Customer :  8T56-TELE BEST SHOP (COD) Invoice No: Cannot Create Invoice\n 369 Customer :  8T40-TELE BRAND (COD) Invoice No: Cannot Create Invoice\n 370 Customer :  8T54-TEXTILE TRENDS (COD) Invoice No: Cannot Create Invoice\n 371 Customer :  8T68-THE BEDDING STUDIO (COD) Invoice No: Cannot Create Invoice\n 372 Customer :  8T69-THE BODY SHOP PK (COD) Invoice No: Cannot Create Invoice\n 373 Customer :  8T65-THE DENIM STUDIO (COD) Invoice No: Cannot Create Invoice\n 374 Customer :  8T78-THE FASHION DESIRE (COD) Invoice No: Cannot Create Invoice\n 375 Customer :  8T62-THE GREAT FOOD HONEY (COD) Invoice No: Cannot Create Invoice\n 376 Customer :  8T73-THE ONLINE SHOP (COD) Invoice No: Cannot Create Invoice\n 377 Customer :  8T71-THE WISH BOX (COD) Invoice No: Cannot Create Invoice\n 378 Customer :  8T74-THREAD ELIGENCE (COD) Invoice No: Cannot Create Invoice\n 379 Customer :  8T58-THREAD UP (COD) Invoice No: Cannot Create Invoice\n 380 Customer :  8T50-TOP MARK (COD) ALI ILYAS Invoice No: Cannot Create Invoice\n 381 Customer :  8T75-TORONTO DIGITS (COD) Invoice No: Cannot Create Invoice\n 382 Customer :  8T51-TREND HOME TEX (COD) Invoice No: Cannot Create Invoice\n 383 Customer :  8T64-TRENDS.PK (COD) Invoice No: Cannot Create Invoice\n 384 Customer :  8T57-TRENDY PAKISTAN (COD) Invoice No: Cannot Create Invoice\n 385 Customer :  8T59-TRENDY TECH (COD ) FSD Invoice No: Cannot Create Invoice\n 386 Customer :  8U27-U BRAND ONLINE (COD) Invoice No: Cannot Create Invoice\n 387 Customer :  8U40-U BRANDS ONLINE.COM (COD) Invoice No: Cannot Create Invoice\n 388 Customer :  8U49-UM TRADERS (COD) Invoice No: Cannot Create Invoice\n 389 Customer :  8U34-UMERS FASHION STORE (COD) Invoice No: Cannot Create Invoice\n 390 Customer :  8U35-UMU COLLECTION (COD) Invoice No: Cannot Create Invoice\n 391 Customer :  8U26-UNITEEZ STORE (COD) Invoice No: Cannot Create Invoice\n 392 Customer :  8U41-URBAN SWATCH (COD) Invoice No: Cannot Create Invoice\n 393 Customer :  8U25-URGE TECH (COD) Invoice No: Cannot Create Invoice\n 394 Customer :  8U52-UZH COLLECTION (COD) Invoice No: Cannot Create Invoice\n 395 Customer :  8V4-VACI WAKI (COD) Invoice No: Cannot Create Invoice\n 396 Customer :  8V3-VESTIRO CLOTHING (COD) Invoice No: Cannot Create Invoice\n 397 Customer :  8V7-VIP CLOTHING GALLERY (COD) Invoice No: Cannot Create Invoice\n 398 Customer :  8V5-VISION MARKETTERS (COD) Invoice No: Cannot Create Invoice\n 399 Customer :  8W27-WANIYA TRADERS (COD) Invoice No: Cannot Create Invoice\n 400 Customer :  8W29-WAQAR TRADERS (COD) Invoice No: Cannot Create Invoice\n 401 Customer :  8W23-WAX DNM (COD) Invoice No: Cannot Create Invoice\n 402 Customer :  8W18-WHOLE SALE COLLECTION (COD) Invoice No: Cannot Create Invoice\n 403 Customer :  8W30-WHOLESALE MASTER REPLICA STORE (COD) Invoice No: Cannot Create Invoice\n 404 Customer :  8W22-WIYA (COD) Invoice No: Cannot Create Invoice\n 405 Customer :  8W25-WOMEN CLOTHING STORE (COD) Invoice No: Cannot Create Invoice\n 406 Customer :  8W28-WOMENS LAND (COD) Invoice No: Cannot Create Invoice\n 407 Customer :  8W24-WORLD TELE SHOP (COD) Invoice No: Cannot Create Invoice\n 408 Customer :  8W20-WOVEN WEAR (COD) Invoice No: Cannot Create Invoice\n 409 Customer :  8W26-WWW.FAIRYCLOTHES.COM.PK (COD) Invoice No: Cannot Create Invoice\n 410 Customer :  8Y5-YOUR STORE (COD) Invoice No: Cannot Create Invoice\n 411 Customer :  8Y6-YOUR STYLE SHOP (COD) Invoice No: Cannot Create Invoice\n 412 Customer :  8Z38-ZABAR (COD) Invoice No: Cannot Create Invoice\n 413 Customer :  8Z15-ZAHRA STORE (COD) Invoice No: Cannot Create Invoice\n 414 Customer :  8Z32-ZAIB TANN (COD) Invoice No: Cannot Create Invoice\n 415 Customer :  8Z29-ZAIN COLLECTION (COD) Invoice No: Cannot Create Invoice\n 416 Customer :  8Z27-ZAQ (COD) Invoice No: Cannot Create Invoice\n 417 Customer :  8Z21-ZEE CHOICE (COD) Invoice No: Cannot Create Invoice\n 418 Customer :  8Z37-ZEE JEE FABRICS (COD) Invoice No: Cannot Create Invoice\n 419 Customer :  8Z39-ZEE NATION (COD) Invoice No: Cannot Create Invoice\n 420 Customer :  8Z20-ZEE TELE MALL (COD) Invoice No: Cannot Create Invoice\n 421 Customer :  8Z30-ZEHAK (COD) Invoice No: Cannot Create Invoice\n 422 Customer :  8Z25-ZIA COLLECTION ( COD ) Invoice No: Cannot Create Invoice\n 423 Customer :  8Z33-ZNH TEX FAISALABAD (COD) Invoice No: Cannot Create Invoice\n 424 Customer :  8Z26-ZNH TEXTILE (COD) Invoice No: Cannot Create Invoice\n ";
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage" + Guid.NewGuid(), "alert('Invoicing Completed: " + invoicecount.ToString() + " Invoices Made out of " + invoicettl.ToString() + ".')", true);

            string script_ = @"window.open(""{0}"", ""{1}"", ""{2}"");";
            Session["List_view"] = list;
            string script = String.Format(script_, "InvoiceSummary.aspx", "_blank", "");

            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Redirect", script, true);

        }

        public string GenerateManualInvoice(Cl_Variables clvar)
        {
            string invoiceNumber = "";
            SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            try
            {
                sqlcon.Open();
                SqlCommand sqlcmd = new SqlCommand("spGenerateAutoInvoices_v2", sqlcon);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandTimeout = 2000;

                sqlcmd.Parameters.AddWithValue("@BranchCode", clvar.Branch);
                sqlcmd.Parameters.AddWithValue("@ZoneCode", HttpContext.Current.Session["ZONECODE"].ToString());
                sqlcmd.Parameters.AddWithValue("@DateFrom", clvar.FromDate.ToString("yyyy-MM-dd"));
                sqlcmd.Parameters.AddWithValue("@DateTo", clvar.ToDate.ToString("yyyy-MM-dd"));
                sqlcmd.Parameters.AddWithValue("@UserName", HttpContext.Current.Session["U_ID"].ToString());
                sqlcmd.Parameters.AddWithValue("@ClientId", clvar.CreditClientID);
                sqlcmd.Parameters.AddWithValue("@CompanyId", clvar.Company);
                sqlcmd.Parameters.AddWithValue("@Autocheck", "0");


                SqlDataAdapter sda = new SqlDataAdapter(sqlcmd);
                DataSet ds = new DataSet();
                sda.Fill(ds);
                sqlcon.Close();
                if (ds.Tables[3].Rows.Count != 0)
                {
                    invoiceNumber = ds.Tables[3].Rows[0][0].ToString();
                }
            }
            catch (Exception ex)
            { return ex.Message; }
            finally { sqlcon.Close(); }

            return invoiceNumber;
        }
        public DataTable GetConsignmentsForInvoice(Cl_Variables clvar)
        {
            DataTable dt = new DataTable();

            string sql = " \n"
               + "SELECT top(100) * \n"
               + "FROM   ( \n"
               + "          SELECT cc.id, \n"
               + "           z.name Zone, \n"
               + "           bb.name Branch, \n"
               + "           cc.accountNo, \n"
               + "           cc.name 'Client Name', \n"
               + "           CAST(c.bookingDate AS DATE) bookingDate, \n"
               + "           c.consignmentNumber ConsignmentNumber, \n"
               + "           c.pieces pieces, \n"
               + "           CASE  \n"
               + "                WHEN c.consignmentTypeId = '13' THEN 'Hand Carry' \n"
               + "                ELSE c.serviceTypeName \n"
               + "           END AS ServiceTypeName, \n"
               + "           b.name Destination, \n"
               + "           c.weight WEIGHT, \n"
               + "           c.totalAmount totalAmount, \n"
               + "           CASE  \n"
               + "                WHEN c.serviceTypeName IN ('Road n Rail', 'Flyer', 'NTS', 'HEC',  \n"
               + "                                          'Bank to Bank', 'Bulk Shipment',  \n"
               + "                                          'overnight', 'Return Service',  \n"
               + "                                          'Same Day', 'Second Day', 'Smart Box',  \n"
               + "                                          'Sunday & Holiday', 'Smart Box',  \n"
               + "                                          'Hand Carry', 'Smart Cargo', 'MB10',  \n"
               + "                                          'MB2', 'MB20', 'MB30', 'MB5',  \n"
               + "                                          'Aviation Sale') THEN 'Domestic' \n"
               + "                WHEN c.serviceTypeName IN ('Expressions',  \n"
               + "                                          'International Expressions', 'Mango',  \n"
               + "                                          'Mango Petty') THEN 'Expression' \n"
               + "                WHEN c.serviceTypeName IN ( \n"
               + "                                          'International 5 Percent Discount Tariff Non Doc',  \n"
               + "                                          'International 10 Percent Discount Tariff Non Doc',  \n"
               + "                                          'International 15 Percent Discount tariff Non Doc',  \n"
               + "                                          'International 20 Percent Discount tariff Non Doc',  \n"
               + "                                          'International 25 Percent Discount Tariff Non Doc',  \n"
               + "                                          'International Cargo',  \n"
               + "                                          'International_Box',  \n"
               + "                                          'International_Non-Doc',  \n"
               + "                                          'International_Non-Doc_Special_Hub_2014',  \n"
               + "                                          'Logex') THEN 'SAMPLE' \n"
               + "                WHEN c.serviceTypeName IN ( \n"
               + "                                          'International Discount Tariff 5 Percent',  \n"
               + "                                          'International 10 Percent Discount tariff',  \n"
               + "                                          'International 15 percent Discount tariff',  \n"
               + "                                          'International 20 Percent Discount tariff',  \n"
               + "                                          'International 25 Percent Discount tariff',  \n"
               + "                                          'International Special Rates from KHI',  \n"
               + "                                          'International Special Rates from Up Country',  \n"
               + "                                          'International Student Package Tariff',  \n"
               + "                                          'International_Doc',  \n"
               + "                                          'International_Doc_Special_Hub') THEN  \n"
               + "                     'DOCUMENT' \n"
               + "           END AS Product, \n"
               + "           c.isPriceComputed \n"
               + "           FROM Consignment c \n"
               + "           INNER JOIN CreditClients cc \n"
               + "           ON c.creditClientId = cc.id \n"
               + "           INNER JOIN Branches bb \n"
               + "           ON cc.branchCode = bb.branchCode \n"
               + "           INNER JOIN Branches AS b \n"
               + "           ON b.branchCode = c.destination \n"
               + "           INNER JOIN Zones z \n"
               + "           ON bb.zoneCode = z.zoneCode \n"
               + "           INNER JOIN ServiceTypes st \n"
               + "           ON c.serviceTypeName = st.serviceTypeName \n"
               + "            \n"
               + "           WHERE cc.id = '" + clvar.CreditClientID + "' \n"
               + "           AND c.status != '9' and c.isapproved = '1' \n"
               + "   and cast(c.bookingDate AS DATE) BETWEEN '" + clvar.FromDate.ToString("yyyy-MM-dd") + "' AND '" + clvar.ToDate.ToString("yyyy-MM-dd") + "'\n"
               + "   and st.companyId = '" + clvar.Company + "'\n"
               + "       ) temp \n"
               + "       LEFT OUTER JOIN InvoiceConsignment ic \n"
               + "            ON  ic.consignmentNumber = temp.consignmentNumber \n"
               + "       LEFT OUTER JOIN Invoice i \n"
               + "            ON  ic.invoiceNumber = i.invoiceNumber \n"
               + "            AND isnull(i.IsInvoiceCanceled,'0') = '0'";


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
        
        public void Get_customerList()
        {
            string k = "";
            for (int i = 0; i < cbBillingCycle.Items.Count; i++)
            {
                if (cbBillingCycle.Items[i].Selected)
                {

                    k = k + cbBillingCycle.Items[i].Value + ", ";
                }
            }
            if (k.Length > 2)
                k = k.Substring(0, k.Length - 2);
            string sql = @"select ' ' +  c.consignerAccountNo + '-' + cc.name + '('+b.sname+') - ('+cast(count(*) as varchar)+' CNs)'+case when cc.isactive = 0 then ' - Inactive' else '' end NAME, cc.id
                            FROM   consignment AS c
                            inner join CODConsignmentDetail_New as cod on c.consignmentNumber=cod.consignmentNumber
                            INNER JOIN CreditClients  AS cc ON  cc.id = c.creditClientId
                            inner join Branches b on b.branchCode = c.orgin
                            WHERE cast(c.bookingDate as date) between '" + new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("yyyy-MM-dd") + "' AND '" + DateTime.Now.ToString("yyyy-MM-dd") + @"'
                            and c.consigneraccountno not in ('0','4D1','300014','4T154','4T67')  and isnull(c.status,'0') != '9'
                            and cc.CODType <> 3 and isnull(c.isinvoiced,'0') != '1' and isnull(c.isapproved,'0') = '1'
                            and cc.BillingID in (" + k + ") and isnull(c.ispricecomputed,'0') = '1' and IsCOD = '1' ";
            if (ddl_zoneId.SelectedIndex > 0)
                sql += " and c.zoneCode = '" + ddl_zoneId.SelectedValue.ToString() + "'";
                            sql += " group by b.sname,c.consignerAccountNo,cc.name, cc.id, cc.isactive order by c.consignerAccountNo desc";

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

            SelectAll.Text = "Select All (" + dt.Rows.Count.ToString() + ")";
            lb_CustomerList.DataSource = null;
            lb_CustomerList.DataSource = dt.DefaultView;
            lb_CustomerList.DataTextField = "Name";
            lb_CustomerList.DataValueField = "id";
            lb_CustomerList.DataBind();



        }
        protected void SelectAll_CheckedChanged(object sender, EventArgs e)
        {
            if (SelectAll.Checked == true)
            {
                if (lb_CustomerList.Items.Count != 0)
                {
                    for (int i = 0; i < lb_CustomerList.Items.Count; i++)
                    {
                        lb_CustomerList.Items[i].Selected = true;
                    }
                }
            }
            else
            {
                if (lb_CustomerList.Items.Count != 0)
                {
                    for (int i = 0; i < lb_CustomerList.Items.Count; i++)
                    {
                        lb_CustomerList.Items[i].Selected = false;
                    }
                }
            }

        }

        public DataTable Get_InvoiceEndCheck(string Zone, string fromdate, string todate)
        {
            string sql = "SELECT * FROM Mnp_Account_DayEnd made WHERE made.Doc_Type='I' AND cast(made.[DateTime] as date) BETWEEN '" + fromdate + "' AND '" + todate + "' and zone='" + Zone + "' ";
            Cl_Variables clvar = new Cl_Variables();
            DataSet dt = new DataSet();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sql, con);
                sda.TableMappings.Add("Table", "tblInvoices");
                sda.Fill(dt);

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return dt.Tables[0];
        }

    }
}