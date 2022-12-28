using MRaabta.App_Code;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace MRaabta.Files
{
    public partial class CN_Import_Print : System.Web.UI.Page
    {
        Cl_Variables clvar = new Cl_Variables();
        Boolean flag = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                string par = Request.QueryString["Param"].ToString();
                Header(par);
                Detail(par);
                lbl_Dd.Text = DateTime.Now.ToString();
            }
        }


        public void Header(string par)
        {
            string query = " Select c.consignmentNumber, b.name OriginBranchName, b1.name destinationBranchName, z.name ZoneName, c1.Name OriginCountry, case when c.ConsignmentTypeid ='19' then 'DDP' when c.ConsignmentTypeid ='18' then 'DDU' else 'N/A' end  serviceTypeName,c.consignerAccountNo,c.consignee consigner,c.bookingDate, (Select GSTNo from Company where id ='1') GstNo   from consignment c \n" +
                           "     inner join Branches b on c.orgin = b.branchCode   \n" +
                           "     inner join Branches b1 on c.Destination = b1.branchCode   \n" +
                            "    inner join Zones z on c.zoneCode = z.zoneCode  \n" +
                            "    inner join CreditClients cc on cc.id = c.creditClientId  \n" +
                            "    inner join Country c1 on c1.Num = c.origin_country  \n" +
                            "    where c.consignmentNumber = '" + par + "'  \n" +
                            "    and b.status='1'  \n" +
                            "    and b1.status='1'  \n" +
                            "    and cc.isActive='1' ";


            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {


                    t1.Text = dt.Rows[0]["consignmentnumber"].ToString();

                    lbl_ConsignmentNumber.Text = dt.Rows[0]["consignmentnumber"].ToString();
                    lbl_Branch.Text = dt.Rows[0]["OriginBranchName"].ToString();
                    lbl_Zone.Text = dt.Rows[0]["ZoneName"].ToString();
                    lbl_OriginCountry.Text = dt.Rows[0]["OriginCountry"].ToString();
                    lbl_ServiceType.Text = dt.Rows[0]["serviceTypeName"].ToString();
                    lbl_AccountNo.Text = dt.Rows[0]["consignerAccountNo"].ToString();
                    lbl_CustomeName.Text = dt.Rows[0]["consigner"].ToString();
                    lbl_ReceiptDate.Text = dt.Rows[0]["bookingDate"].ToString().Substring(0, 11);
                    lbl_Destination.Text = dt.Rows[0]["destinationBranchName"].ToString();
                    lbl_Gst.Text = dt.Rows[0]["GstNo"].ToString();
                }
            }
            catch (Exception ex)
            {

            }
            finally { con.Close(); }
        }

        public void Detail(string par)
        {
            string query = " Select a.PriceModifiers, round(Sum(a.totalamount),2) TotalAmount, case when max(isTaxable) ='0' then '0' else round(sum(a.gst),2) end Gst from ( \n" +
                           " Select Name PriceModifiers, 0 totalamount, 0 Gst, 0 isTaxable  from PriceModifiers p \n" +
                           " where Import='1' \n" +
                           " union all  \n" +
                           " Select Name PriceModifiers, o.calculatedValue totalamount, calculatedGST Gst, p.isgst isTaxable from PriceModifiers p right outer Join ConsignmentModifier o on p.id = o.priceModifierId \n" +
                           " where Import='1' and o.consignmentNumber = '" + par + "' \n" +
                           " ) a inner join PriceModifiers o on a.PriceModifiers = o.Name \n" +
                           " group by a.PriceModifiers,o.sortorder \n" +
                           " order by sortorder";


            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    gv_productWiseAmount.DataSource = dt;
                    gv_productWiseAmount.DataBind();

                    double total = 0; ;
                    double gst = 0; ;

                    gv_productWiseAmount.FooterRow.Cells[0].Text = " Amount";
                    gv_productWiseAmount.FooterRow.Cells[0].Font.Bold = true;

                    gv_productWiseAmount.FooterRow.Cells[1].Font.Bold = true;
                    gv_productWiseAmount.FooterRow.Cells[1].HorizontalAlign = HorizontalAlign.Right;
                    for (int k = 0; k < dt.Rows.Count; k++)
                    {
                        total += double.Parse(dt.Rows[k]["TotalAmount"].ToString());
                        gst += double.Parse(dt.Rows[k]["gst"].ToString());

                    }
                    gv_productWiseAmount.FooterRow.Cells[1].Text = total.ToString();
                    gv_productWiseAmount.FooterRow.Cells[1].Font.Bold = true;
                    gv_productWiseAmount.FooterRow.BackColor = System.Drawing.Color.Beige;
                    gv_productWiseAmount.FooterRow.Cells[1].HorizontalAlign = HorizontalAlign.Right;

                    gv_productWiseAmount.FooterRow.Cells[2].Text = gst.ToString();
                    gv_productWiseAmount.FooterRow.Cells[2].Font.Bold = true;
                    gv_productWiseAmount.FooterRow.BackColor = System.Drawing.Color.Beige;
                    gv_productWiseAmount.FooterRow.Cells[2].HorizontalAlign = HorizontalAlign.Right;

                    totalAmount.Text = NumWords(total + gst) + " Only ";
                    totalAmount_.Text = String.Format("{0:0.00}", total + gst);
                }
            }
            catch (Exception ex)
            {

            }
            finally { con.Close(); }
        }

        private string NumWords(double n) //converts double to words
        {
            string[] numbersArr = new string[] { "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
            string[] tensArr = new string[] { "Twenty", "Thirty", "Fourty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninty" };
            string[] suffixesArr = new string[] { "Thousand", "Million", "Billion", "Trillion", "Quadrillion", "Quintillion", "Sextillion", "septillion", "octillion", "nonillion", "decillion", "undecillion", "duodecillion", "tredecillion", "Quattuordecillion", "Quindecillion", "Sexdecillion", "Septdecillion", "Octodecillion", "Novemdecillion", "Vigintillion" };
            string words = "";

            bool tens = false;

            if (n < 0)
            {
                words += "negative ";
                n *= -1;
            }

            int power = (suffixesArr.Length + 1) * 3;

            while (power > 3)
            {
                double pow = Math.Pow(10, power);
                if (n >= pow)
                {
                    if (n % pow > 0)
                    {
                        words += NumWords(Math.Floor(n / pow)) + " " + suffixesArr[(power / 3) - 1] + ", ";
                    }
                    else if (n % pow == 0)
                    {
                        words += NumWords(Math.Floor(n / pow)) + " " + suffixesArr[(power / 3) - 1];
                    }
                    n %= pow;
                }
                power -= 3;
            }
            if (n >= 1000)
            {
                if (n % 1000 > 0) words += NumWords(Math.Floor(n / 1000)) + " thousand, ";
                else words += NumWords(Math.Floor(n / 1000)) + " thousand";
                n %= 1000;
            }
            if (0 <= n && n <= 999)
            {
                if ((int)n / 100 > 0)
                {
                    words += NumWords(Math.Floor(n / 100)) + " hundred";
                    n %= 100;
                }
                if ((int)n / 10 > 1)
                {
                    if (words != "")
                        words += " ";
                    words += tensArr[(int)n / 10 - 2];
                    tens = true;
                    n %= 10;
                }

                if (n < 20 && n > 0)
                {
                    if (words != "" && tens == false)
                        words += " ";
                    words += (tens ? "-" + numbersArr[(int)n - 1] : numbersArr[(int)n - 1]);
                    n -= Math.Floor(n);
                }
            }

            return words;

        }



        protected void gv_productWiseAmount_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
    }
}