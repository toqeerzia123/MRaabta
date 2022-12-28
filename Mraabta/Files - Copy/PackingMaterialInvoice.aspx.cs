using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MRaabta.Files
{
    public partial class PackingMaterialInvoice : System.Web.UI.Page
    {
        string constr = ConfigurationManager.ConnectionStrings["ConnectionString_"].ToString();
        string Mode = "A";
        int PID;
        string LID;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    PID = Convert.ToInt32(Request.QueryString["ID"]);
                    LID = Request.QueryString["LID"];
                    if (Request.QueryString["Mode"] != null)
                    {
                        Mode = Request.QueryString["Mode"].ToString();
                    }
                    if (Mode == "V")
                    {
                        CustomerDetail();
                        InvoiceDetails();
                        GST();
                        NetAmount();
                        TotalAmount();
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        void CustomerDetail()
        {
            SqlConnection con = new SqlConnection(constr);
            con.Open();
            string com1 = @"select CONCAT( cc.contactPerson ,'-',cc.name) as name,cc.address,cc.accountNo, l.locationName,pr.RequestLabel,pr.InvoiceDate, pr.PackingRequestID from PR_PackingRequest as pr
                                    inner join CreditClients as cc on pr.CustomerID = cc.id
                                    inner join COD_CustomerLocations as l on pr.LocationID = l.locationID
                                    where pr.PackingRequestID=" + PID + @" AND cc.accountNo = '" + LID + @"'";
            SqlDataAdapter adpt1 = new SqlDataAdapter(com1, con);
            DataTable dt2 = new DataTable();
            adpt1.Fill(dt2);
            con.Close();
            if (dt2.Rows.Count > 0)
            {
                lbl_customerCode.Text = dt2.Rows[0]["accountNo"].ToString();
                lbl_customerName.Text = dt2.Rows[0]["name"].ToString();
                lbl_Address.Text = dt2.Rows[0]["address"].ToString();
                lblPackingSheetNo.Text = dt2.Rows[0]["RequestLabel"].ToString();
                if (dt2.Rows[0]["InvoiceDate"].ToString() != "" && dt2.Rows[0]["InvoiceDate"].ToString() != null)
                {
                    lbl_Invoice_Date.Text = Convert.ToDateTime(dt2.Rows[0]["InvoiceDate"]).ToString("MMMM dd,yyyy");
                    lbl_Invoice_Month.Text = Convert.ToDateTime(dt2.Rows[0]["InvoiceDate"]).ToString("MMMM-yyyy");
                }
                //lbl_Invoice_Date.Text = DateTime.Now.ToString("MMMM dd,yyyy");
                //lbl_Invoice_Month.Text = DateTime.Now.ToString("MMMM-yyyy");
                //http://localhost/MRaabta/Files/PackingMaterialInvoice.aspx?ID=3&Mode=E
            }
        }
        void InvoiceDetails()
        {
            try
            {
                SqlConnection con = new SqlConnection(constr);
                SqlCommand cmd = new SqlCommand(@"select ROW_NUMBER() OVER(ORDER BY prd.PackingRequestDetailID) AS SNO,R.Rate,R.GST,R.Rate*IssuedQuantity as TotalPrice
                                                ,R.GST*(R.Rate*IssuedQuantity) as GSTAmount
                                                ,(R.Rate*IssuedQuantity)+(R.GST*(R.Rate*IssuedQuantity)) as TotalAmount
                                                ,Req.CustomerID,Req.Address,Req.LocationID,Req.RequestLabel	,C.name as Cusname,L.Locationname
                                                ,CONCAT(pm.Name,' - ', pms.Size,' (',IssuedQuantity,'*',R.Rate,')') as Details
                                                from PR_PackingRequestDetail as prd
                                                inner join  PR_PackingRequest Req on prd.PackingRequestID=Req.PackingRequestID
                                                inner join PR_packing_material as pm on prd.ItemID = pm.ID
                                                inner join PR_PackingMaterialSize as pms on prd.ItemSize = pms.ID
                                                inner join CreditClients as C on Req.CustomerID = C.id									 
                                                left join COD_CustomerLocations as L on Req.LocationID = L.locationID and C.id=L.CreditClientID	
                                                inner join PR_PackingMaterialRate as R on R.MaterialID = pm.ID and R.SizeID=pms.ID	AND R.ClientId = c.id
                                                where prd.PackingRequestID  =" + PID + @" AND c.accountNo = '" + LID + @"'");
                SqlDataAdapter sda = new SqlDataAdapter();
                cmd.Connection = con;
                sda.SelectCommand = cmd;
                using (DataTable dt1 = new DataTable())
                {
                    sda.Fill(dt1);
                    rpt_goods_details.DataSource = dt1;
                    rpt_goods_details.DataBind();
                }
                con.Close();
            }
            catch (Exception)
            {

                throw;
            }

        }
        void GST()
        {
            try
            {
                SqlConnection con = new SqlConnection(constr);
                SqlCommand cmd1 = new SqlCommand(@"select sum(R.GST*(R.Rate*IssuedQuantity)) as GstAmount 
									from PR_PackingRequestDetail as prd
										inner join  PR_PackingRequest Req on prd.PackingRequestID=Req.PackingRequestID
                                        inner join PR_packing_material as pm on prd.ItemID = pm.ID
                                         inner join PR_PackingMaterialSize as pms on prd.ItemSize = pms.ID
                                         inner join PR_PackingMaterialRate as R on R.MaterialID = pm.ID and R.SizeID=pms.ID	
                                         inner join CreditClients as C on Req.CustomerID = C.id									 
                                         left join COD_CustomerLocations as L on Req.LocationID = L.locationID and C.id=L.CreditClientID									
                                         where prd.PackingRequestID =" + PID + @" AND c.accountNo = '" + LID + @"'");

                SqlDataAdapter sda1 = new SqlDataAdapter();

                cmd1.Connection = con;
                sda1.SelectCommand = cmd1;
                using (DataTable dt2 = new DataTable())
                {
                    sda1.Fill(dt2);
                    if (dt2.Rows.Count > 0)
                    {
                        // txtRequestNumber.Text = dt1.Rows[0]["RequestLabel"].ToString();
                        lbl_GST_AMOUNT.Text = dt2.Rows[0]["GstAmount"].ToString();
                        ;
                    }

                }
                con.Close();
            }
            catch (Exception)
            {

                throw;
            }

        }
        void NetAmount()
        {
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd2 = new SqlCommand(@"select sum((R.Rate*IssuedQuantity)) as NetAmount 
									from PR_PackingRequestDetail as prd
										inner join  PR_PackingRequest Req on prd.PackingRequestID=Req.PackingRequestID
                                        inner join PR_packing_material as pm on prd.ItemID = pm.ID
                                         inner join PR_PackingMaterialSize as pms on prd.ItemSize = pms.ID
                                         inner join PR_PackingMaterialRate as R on R.MaterialID = pm.ID and R.SizeID=pms.ID	
                                         inner join CreditClients as C on Req.CustomerID = C.id									 
                                         left join COD_CustomerLocations as L on Req.LocationID = L.locationID and C.id=L.CreditClientID									
                                         where prd.PackingRequestID =" + PID);

            SqlDataAdapter sda2 = new SqlDataAdapter();

            cmd2.Connection = con;
            sda2.SelectCommand = cmd2;
            using (DataTable dt3 = new DataTable())
            {
                sda2.Fill(dt3);
                if (dt3.Rows.Count > 0)
                {
                    // txtRequestNumber.Text = dt1.Rows[0]["RequestLabel"].ToString();
                    lbl_NET_AMOUNT.Text = dt3.Rows[0]["NetAmount"].ToString();

                    ;
                }
            }
            con.Close();


        }
        String[] units = { "Zero", "One", "Two", "Three",
    "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven",
    "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen",
    "Seventeen", "Eighteen", "Nineteen" };
        String[] tens = { "", "", "Twenty", "Thirty", "Forty",
    "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

        public String ConvertAmount(double amount)
        {
            try
            {
                Int64 amount_int = (Int64)amount;
                Int64 amount_dec = (Int64)Math.Round((amount - (double)(amount_int)) * 100);
                if (amount_dec == 0)
                {
                    return Convert1(amount_int) + " Only.";
                }
                else
                {
                    return Convert1(amount_int) + " Point " + Convert1(amount_dec) + " Only.";
                }
            }
            catch (Exception e)
            {
                // TODO: handle exception  
            }
            return "";
        }

        public String Convert1(Int64 i)
        {
            if (i < 20)
            {
                return units[i];
            }
            if (i < 100)
            {
                return tens[i / 10] + ((i % 10 > 0) ? " " + Convert1(i % 10) : "");
            }
            if (i < 1000)
            {
                return units[i / 100] + " Hundred"
                        + ((i % 100 > 0) ? " And " + Convert1(i % 100) : "");
            }
            if (i < 100000)
            {
                return Convert1(i / 1000) + " Thousand "
                + ((i % 1000 > 0) ? " " + Convert1(i % 1000) : "");
            }
            if (i < 10000000)
            {
                return Convert1(i / 100000) + " Lac "
                        + ((i % 100000 > 0) ? " " + Convert1(i % 100000) : "");
            }
            if (i < 1000000000)
            {
                return Convert1(i / 10000000) + " Million  "
                        + ((i % 10000000 > 0) ? " " + Convert1(i % 10000000) : "");
            }
            return Convert1(i / 1000000000) + " Billion "
                    + ((i % 1000000000 > 0) ? " " + Convert1(i % 1000000000) : "");
        }
        void TotalAmount()
        {
            try
            {
                SqlConnection con = new SqlConnection(constr);
                SqlCommand cmd2 = new SqlCommand(@"select sum((R.Rate*IssuedQuantity)+R.GST*(R.Rate*IssuedQuantity)) as TotalAmount 
									from PR_PackingRequestDetail as prd
										inner join  PR_PackingRequest Req on prd.PackingRequestID=Req.PackingRequestID
                                        inner join PR_packing_material as pm on prd.ItemID = pm.ID
                                         inner join PR_PackingMaterialSize as pms on prd.ItemSize = pms.ID
                                         inner join PR_PackingMaterialRate as R on R.MaterialID = pm.ID and R.SizeID=pms.ID	
                                         inner join CreditClients as C on Req.CustomerID = C.id									 
                                         left join COD_CustomerLocations as L on Req.LocationID = L.locationID and C.id=L.CreditClientID									
                                         where prd.PackingRequestID =" + PID);

                SqlDataAdapter sda2 = new SqlDataAdapter();

                cmd2.Connection = con;
                sda2.SelectCommand = cmd2;
                using (DataTable dt3 = new DataTable())
                {
                    sda2.Fill(dt3);
                    if (dt3.Rows.Count > 0)
                    {
                        // txtRequestNumber.Text = dt1.Rows[0]["RequestLabel"].ToString();
                        lbl_Total_Amount.Text = Convert.ToInt64(dt3.Rows[0]["TotalAmount"]).ToString("N0");
                        lbl_Amount_Words.Text = ConvertAmount(Convert.ToDouble(Convert.ToInt64(dt3.Rows[0]["TotalAmount"]))).ToString();
                    }
                }
                con.Close();
            }
            catch (Exception)
            {

                throw;
            }



        }
    }

}