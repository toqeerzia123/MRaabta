using System;
using System.Web.UI;
using MRaabta.App_Code;
using System.Data;
using System.Data.SqlClient;

namespace MRaabta.Files
{
    public partial class codsearchconsignment : System.Web.UI.Page
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
            dt = GetConsignmentDetail(clvar);
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

                lbl_orderRef.Text = dt.Rows[0]["orderrefNo"].ToString();
                // lbl_productType.Text = dt.Rows[0]["PRODUCTTYPEID"].ToString();
                //if (dt.Rows[0]["ChargeCodAmount"].ToString() != "0")
                //{
                //    Cb_CODAmount.Checked = true;
                //}
                //else
                //{
                //    Cb_CODAmount.Checked = false;
                //}

                lbl_descriptionCOD.Text = dt.Rows[0]["ProductDescription"].ToString();
                lbl_CODAmount.Text = dt.Rows[0]["CODAMount"].ToString();
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

                lbl_totalCharges.Text = (double.Parse(lbl_totalCharges.Text) + value).ToString();
                lbl_gstCharges.Text = (double.Parse(lbl_gstCharges.Text) + gst).ToString();
                lbl_totalAmount.Text = (double.Parse(dt.Rows[0]["TotalAmount"].ToString()) + double.Parse(dt.Rows[0]["gst"].ToString()) + value + gst + double.Parse(lbl_insurance.Text)).ToString();
            }
        }
        protected void txt_consignmentNumber_TextChanged(object sender, EventArgs e)
        {
            ConsignmentDetails();
        }
        protected void btn_PrintConsignment_Click(object sender, EventArgs e)
        {
            string script_ = @"window.open(""{0}"", ""{1}"", ""{2}"");";

            string ecnryptedConsignment = EnryptString(txt_consignmentNumber.Text);
            string script = String.Format(script_, "../RetailCOD/addresslabel?CN=" + ecnryptedConsignment, "_blank", "");
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Redirect", script, true);
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
        public DataTable GetConsignmentDetail(Cl_Variables clvar)
        {
            string sqlString = @"select c.consignmentNumber ConNo,
                    c.consignerAccountNo AccountNo,
                    c.riderCode,
                    ct.name ConType,
                    cc.cityName City,
                    b2.name Branch,
                    c.weight,
                    c.serviceTypeName,
                    c.discount,
                    c.pieces,
                    c.consignee,
                    c.consigneePhoneNo ConsigneeCell,
                    c.consigneeCNICNo ConsigneeCNIC,
                    c.consigner,
                    c.consignerCellNo ConsignerCell,
                    c.consignerCNICNo ConsignerCNIC,
                    c.couponNumber Coupon,
                    c.decalaredValue DeclaredValue,
                    c.PakageContents PackageContents,
                    c.address Address,
                    c.shipperAddress,
                    c.bookingDate,
                    b.name ORIGIN,
                    c.originExpressCenter,
                    ec.name,
                    c.insuarancePercentage,
                    c.totalAmount,
                    c.chargedAmount,
                    c.isInsured,
                    c.dayType,c.gst, ccc.CODTYPE,
                    cdn.codAmount, cdn.orderRefNo, cdn.productDescription
                from 
                    Consignment c
                    inner join Zones z on z.zoneCode = c.zoneCode
                    inner join Branches b on b.branchCode = c.Orgin
                    inner join Branches b2 on b2.branchCode = c.destination
                    inner join ConsignmentType ct on ct.id = c.consignmentTypeId
                    inner join Cities cc on cc.id = b2.cityId
                    left outer join ExpressCenters ec on ec.expressCenterCode = c.originExpressCenter
                    inner join CreditClients ccc on ccc.id = c.creditClientId
                    INNER JOIN CODConsignmentDetail_New cdn ON c.consignmentNumber = cdn.consignmentNumber
                where 
                    c.consignmentNumber = '" + clvar.consignmentNo + @"' ";

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