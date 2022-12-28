using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using MRaabta.App_Code;

namespace MRaabta.Files
{
    public partial class Recovery_Letter_Form : System.Web.UI.Page
    {
        CommonFunction fun = new CommonFunction();
        Cl_Variables clvar = new Cl_Variables();
        Consignemnts con = new Consignemnts();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Complevel1();
            }
        }

        public void Complevel1()
        {
            DataTable dt = get_Complevel1();
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    dd_Complevel1.DataSource = dt;
                    dd_Complevel1.DataTextField = "HeadName";
                    dd_Complevel1.DataValueField = "HeadID";
                    dd_Complevel1.DataBind();

                }
            }
        }

        public void Complevel2()
        {
            DataTable dt = get_Complevel2();
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {

                    dd_Complevel2.Items.Clear();
                    dd_Complevel2.Items.Add(new ListItem("Select Manager", "0"));
                    dd_Complevel2.DataSource = dt;
                    dd_Complevel2.DataTextField = "ManagerName";
                    dd_Complevel2.DataValueField = "ManagerID";
                    dd_Complevel2.DataBind();

                }
            }
        }

        public void Complevel3()
        {
            DataTable dt = get_Complevel3();
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    dd_Complevel3.DataSource = null;
                    dd_Complevel3.DataBind();

                    dd_Complevel3.Items.Add(new ListItem("Select Officer", "0"));

                    dd_Complevel3.DataSource = dt;
                    dd_Complevel3.DataTextField = "officerName";
                    dd_Complevel3.DataValueField = "OfficerID";
                    dd_Complevel3.DataBind();

                }
            }
        }

        public DataTable get_Complevel1()
        {
            //string query = "select zoneCode, name from Zones where status = '1' order by name";

            string sql = "SELECT cl.comp1 HeadID,cl.Name HeadName FROM Comp_Level1 cl WHERE cl.[Status]='1'";

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
            return dt;
        }

        public DataTable get_Complevel2()
        {

            //string query = "select zoneCode, name from Zones where status = '1' order by name";

            string sql = "SELECT cl.comp2 ManagerID,cl.Name ManagerName FROM Comp_Level2 cl WHERE cl.[Status]='1' and comp1='" + dd_Complevel1.SelectedValue + "'";

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
            return dt;
        }

        public DataTable get_Complevel3()
        {

            //string query = "select zoneCode, name from Zones where status = '1' order by name";

            string sql = "SELECT cl.comp3 OfficerID,cl.Name officerName, branch FROM Comp_Level3 cl WHERE cl.[Status]='1' and comp1='" + dd_Complevel1.SelectedValue + "' and cl.comp2='" + dd_Complevel2.SelectedValue + "'";

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
            ViewState["Comp3"] = dt;
            return dt;
        }

        protected void dd_Complevel1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dd_Complevel1.SelectedValue != "0")
            {
                dd_Complevel2.DataSource = null;
                dd_Complevel2.DataBind();
                Complevel2();
                dd_Complevel3.DataSource = null;
                dd_Complevel3.DataBind();
            }
            else
            { }
        }

        protected void dd_Complevel2_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (dd_Complevel2.SelectedValue != "0")
            {
                Complevel3();
            }
        }

        protected void dd_Complevel3_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)ViewState["Comp3"];
            DataRow[] dr = dt.Select("OfficerID='" + dd_Complevel3.SelectedValue + "'");

            ViewState["branch"] = dr[0]["branch"].ToString();


        }

        protected void btn_showTariff_Click(object sender, EventArgs e)
        {
            string clientgroup = "", days = "";

            if (this.txt_groupID.Text != "")
            {
                if (bt_1.SelectedValue == "0")
                {
                    clientgroup = " and cc.clientGrpId='" + txt_groupID.Text + "' \n";
                }
                else
                {
                    clientgroup = " and cc.accountno='" + txt_groupID.Text + "' \n";

                }
            }
            string fromdate = "", todate = "";
            if (this.dd_Days.SelectedValue != "")
            {

                if (dd_Days.SelectedValue == "60")
                {
                    days = "DayCount >='0' and DayCount <'90'";
                }
                if (dd_Days.SelectedValue == "90")
                {
                    days = "DayCount >='60' and DayCount <'120'";
                }
                if (dd_Days.SelectedValue == "120")
                {
                    days = "DayCount >='120' and DayCount <='150'";
                }
            }


            string sql = "SELECT p1.clientGroupID, \n"
               + "       p1.GroupName, \n"
               + "       SUM(InvoiceAmount)         InvoiceAmount, \n"
               + "       SUM(outstandingAmount)     outstandingAmount \n"
               + "FROM   ( \n"
               + "           SELECT a.clientGroupID, \n"
               + "                  a.GroupName, \n"
               + "                  SUM(a.BILLAMOUNT)      InvoiceAmount, \n"
               + "                  SUM(a.OUTSTANDING)     outstandingAmount \n"
               + "           FROM   ( \n"
               + "                      SELECT z.name      ZoneName, \n"
               + "                             z.zonecode, \n"
               + "                             BB.name     BranchName, \n"
               + "                             bb.branchcode, \n"
               + "                             CC.name     ClientName, \n"
               + "                             cc.id       clientid, \n"
               + "                             Cc.accountNo ClientAccountno, \n"
               + "                             cg.id       clientGroupID, \n"
               + "                             cg.name     GroupName, \n"
               + "                             MONTH(I.startDate) BILLMONTH, \n"
               + "                             YEAR(I.startDate) BILLYEAR, \n"
               + "                             LEFT(DATENAME(MONTH, I.startDate), 3) + '-' + RIGHT(YEAR(I.startDate), 2)  \n"
               + "                             BILLDATE, \n"
               + "                             i.invoiceNumber, \n"
               + "                             DATEDIFF(DAY, i.endDate, GETDATE()) DayCount, \n"
               + "                             SUM( \n"
               + "                                 CASE  \n"
               + "                                      WHEN I.TOTALAMOUNT IS NULL THEN '0' \n"
               + "                                      ELSE I.TOTALAMOUNT \n"
               + "                                 END \n"
               + "                             )           BILLAMOUNT, \n"
               + "                             0           OUTSTANDING \n"
               + "                      FROM   INVOICE I \n"
               + "                             LEFT JOIN ( \n"
               + "                                      SELECT I1.INVOICENUMBER, \n"
               + "                                             SUM(G.AMOUNT)JV \n"
               + "                                      FROM   INVOICE I1 \n"
               + "                                             INNER JOIN GENERALVOUCHER G \n"
               + "                                                  ON  I1.INVOICENUMBER = G.INVOICENO \n"
               + "                                      GROUP BY \n"
               + "                                             I1.INVOICENUMBER \n"
               + "                                  ) JV2 \n"
               + "                                  ON  I.INVOICENUMBER = JV2.INVOICENUMBER \n"
               + "                             LEFT JOIN ( \n"
               + "                                      SELECT I2.INVOICENUMBER, \n"
               + "                                             SUM(IR.AMOUNT)RR \n"
               + "                                      FROM   INVOICE I2 \n"
               + "                                             INNER JOIN INVOICEREDEEM IR \n"
               + "                                                  ON  I2.INVOICENUMBER = IR.INVOICENO \n"
               + "                                             LEFT JOIN PAYMENTVOUCHERS A \n"
               + "                                                  ON  IR.PAYMENTVOUCHERID = A.ID \n"
               + "                                             LEFT JOIN PAYMENTSOURCE B \n"
               + "                                                  ON  A.PAYMENTSOURCEID = B.ID \n"
               + "                                             LEFT JOIN PAYMENTTYPES F \n"
               + "                                                  ON  A.PAYMENTTYPEID = F.ID \n"
               + "                                             LEFT JOIN CHEQUESTATUS CS1 \n"
               + "                                                  ON  A.ID = CS1.PAYMENTVOUCHERID \n"
               + "                                             LEFT JOIN CHEQUESTATE CS2 \n"
               + "                                                  ON  CS1.CHEQUESTATEID = CS2.ID \n"
               + "                                                  AND CS1.ISCURRENTSTATE = '1' \n"
               + "                                      GROUP BY \n"
               + "                                             I2.INVOICENUMBER \n"
               + "                                  ) RR2 \n"
               + "                                  ON  I.INVOICENUMBER = RR2.INVOICENUMBER \n"
               + "                             INNER JOIN CREDITCLIENTS CC \n"
               + "                                  ON  I.CLIENTID = CC.ID \n"
               + "                                  AND (i.IsInvoiceCanceled IS NULL OR i.IsInvoiceCanceled = '0') \n"
               + "                             LEFT OUTER JOIN ClientGroups cg \n"
               + "                                  ON  cc.clientGrpId = cg.id \n"
               + "                             INNER JOIN BRANCHES BB \n"
               + "                                  ON  CC.BRANCHCODE = BB.BRANCHCODE \n"
               + "                             INNER JOIN ZONES Z \n"
               + "                                  ON  BB.ZONECODE = Z.ZONECODE \n"
               + "                      WHERE  i.IsInvoiceCanceled = '0' \n"
               + "                             AND CAST(I.INVOICEDATE AS date) <  = CAST(GETDATE() AS date) \n"
               + "                             AND cg.[status] = '1'  \n";
            sql = sql + clientgroup;
            sql = sql + "GROUP BY \n"
               + "                             MONTH(I.startDate), \n"
               + "                             YEAR(I.startDate), \n"
               + "                             LEFT(DATENAME(MONTH, I.INVOICEDATE), 3) + '-' +  \n"
               + "                             RIGHT(YEAR(I.INVOICEDATE), 2), \n"
               + "                             i.invoiceNumber, \n"
               + "                             I.endDate, \n"
               + "                             I.INVOICEDATE, \n"
               + "                             z.name, \n"
               + "                             BB.name, \n"
               + "                             CC.name, \n"
               + "                             startDate, \n"
               + "                             Cc.accountNo, \n"
               + "                             cc.id, \n"
               + "                             z.Zonecode, \n"
               + "                             bb.branchcode, \n"
               + "                             cg.name, \n"
               + "                             cg.id \n"
               + "                  )                      a \n"
               + "           WHERE  " + days + "\n"
               + "           GROUP BY \n"
               + "                  a.clientGroupID, \n"
               + "                  a.GroupName  \n"
               + "           UNION ALL \n"
               + "           SELECT a1.clientGroupID, \n"
               + "                  a1.GroupName, \n"
               + "                  SUM(a1.BILLAMOUNT)      InvoiceAmount, \n"
               + "                  SUM(a1.OUTSTANDING)     outstandingAmount \n"
               + "           FROM   ( \n"
               + "                      SELECT z.name      ZoneName, \n"
               + "                             z.zonecode, \n"
               + "                             BB.name     BranchName, \n"
               + "                             bb.branchcode, \n"
               + "                             CC.name     ClientName, \n"
               + "                             cc.id       clientid, \n"
               + "                             Cc.accountNo ClientAccountno, \n"
               + "                             cg.id       clientGroupID, \n"
               + "                             cg.name     GroupName, \n"
               + "                             MONTH(I.startDate) BILLMONTH, \n"
               + "                             YEAR(I.startDate) BILLYEAR, \n"
               + "                             LEFT(DATENAME(MONTH, I.startDate), 3) + '-' + RIGHT(YEAR(I.startDate), 2)  \n"
               + "                             BILLDATE, \n"
               + "                             i.invoiceNumber, \n"
               + "                             DATEDIFF(DAY, i.endDate, GETDATE()) DayCount, \n"
               + "                             0           BILLAMOUNT, \n"
               + "                           case when   SUM( \n"
               + "                                 ROUND( \n"
               + "                                     CASE  \n"
               + "                                          WHEN I.TOTALAMOUNT IS NULL THEN '0' \n"
               + "                                          ELSE I.TOTALAMOUNT \n"
               + "                                     END -( \n"
               + "                                         CASE  \n"
               + "                                              WHEN RR2.RR IS NULL THEN '0' \n"
               + "                                              ELSE RR2.RR \n"
               + "                                         END + CASE  \n"
               + "                                                    WHEN JV2.JV IS NULL THEN '0' \n"
               + "                                                    ELSE JV2.JV \n"
               + "                                               END \n"
               + "                                     ), \n"
               + "                                     2 \n"
               + "                                 ) \n"
               + "                             )  < 0 then 0 else \n"
               + "                                  SUM( \n"
               + "                                 ROUND( \n"
               + "                                     CASE  \n"
               + "                                          WHEN I.TOTALAMOUNT IS NULL THEN '0' \n"
               + "                                          ELSE I.TOTALAMOUNT \n"
               + "                                     END -( \n"
               + "                                         CASE  \n"
               + "                                              WHEN RR2.RR IS NULL THEN '0' \n"
               + "                                              ELSE RR2.RR \n"
               + "                                         END + CASE  \n"
               + "                                                    WHEN JV2.JV IS NULL THEN '0' \n"
               + "                                                    ELSE JV2.JV \n"
               + "                                               END \n"
               + "                                     ), \n"
               + "                                     2 \n"
               + "                                 ) \n"
               + "                             ) end  OUTSTANDING \n"
               + "                      FROM   INVOICE I \n"
               + "                             LEFT JOIN ( \n"
               + "                                      SELECT I1.INVOICENUMBER, \n"
               + "                                             SUM(G.AMOUNT)JV \n"
               + "                                      FROM   INVOICE I1 \n"
               + "                                             INNER JOIN GENERALVOUCHER G \n"
               + "                                                  ON  I1.INVOICENUMBER = G.INVOICENO \n"
               + "                                      GROUP BY \n"
               + "                                             I1.INVOICENUMBER \n"
               + "                                  ) JV2 \n"
               + "                                  ON  I.INVOICENUMBER = JV2.INVOICENUMBER \n"
               + "                             LEFT JOIN ( \n"
               + "                                      SELECT I2.INVOICENUMBER, \n"
               + "                                             SUM(IR.AMOUNT)RR \n"
               + "                                      FROM   INVOICE I2 \n"
               + "                                             INNER JOIN INVOICEREDEEM IR \n"
               + "                                                  ON  I2.INVOICENUMBER = IR.INVOICENO \n"
               + "                                             LEFT JOIN PAYMENTVOUCHERS A \n"
               + "                                                  ON  IR.PAYMENTVOUCHERID = A.ID \n"
               + "                                             LEFT JOIN PAYMENTSOURCE B \n"
               + "                                                  ON  A.PAYMENTSOURCEID = B.ID \n"
               + "                                             LEFT JOIN PAYMENTTYPES F \n"
               + "                                                  ON  A.PAYMENTTYPEID = F.ID \n"
               + "                                             LEFT JOIN CHEQUESTATUS CS1 \n"
               + "                                                  ON  A.ID = CS1.PAYMENTVOUCHERID \n"
               + "                                             LEFT JOIN CHEQUESTATE CS2 \n"
               + "                                                  ON  CS1.CHEQUESTATEID = CS2.ID \n"
               + "                                                  AND CS1.ISCURRENTSTATE = '1' \n"
               + "                                      GROUP BY \n"
               + "                                             I2.INVOICENUMBER \n"
               + "                                  ) RR2 \n"
               + "                                  ON  I.INVOICENUMBER = RR2.INVOICENUMBER \n"
               + "                             INNER JOIN CREDITCLIENTS CC \n"
               + "                                  ON  I.CLIENTID = CC.ID \n"
               + "                                  AND (i.IsInvoiceCanceled IS NULL OR i.IsInvoiceCanceled = '0') \n"
               + "                             LEFT OUTER JOIN ClientGroups cg \n"
               + "                                  ON  cc.clientGrpId = cg.id \n"
               + "                             INNER JOIN BRANCHES BB \n"
               + "                                  ON  CC.BRANCHCODE = BB.BRANCHCODE \n"
               + "                             INNER JOIN ZONES Z \n"
               + "                                  ON  BB.ZONECODE = Z.ZONECODE \n"
               + "                      WHERE  i.IsInvoiceCanceled = '0' \n"
               + "                             AND CAST(I.INVOICEDATE AS date) <  = CAST(GETDATE() AS date) \n"
               + "                             AND cg.[status] = '1'  \n"
               + "   						   AND cast(i.startDate as date) >='2015-07-01'\n";
            sql = sql + clientgroup;
            sql = sql + "GROUP BY \n"
                + "                             MONTH(I.startDate), \n"
                + "                             YEAR(I.startDate), \n"
                + "                             LEFT(DATENAME(MONTH, I.INVOICEDATE), 3) + '-' +  \n"
                + "                             RIGHT(YEAR(I.INVOICEDATE), 2), \n"
                + "                             i.invoiceNumber, \n"
                + "                             I.endDate, \n"
                + "                             I.INVOICEDATE, \n"
                + "                             z.name, \n"
                + "                             BB.name, \n"
                + "                             CC.name, \n"
                + "                             startDate, \n"
                + "                             Cc.accountNo, \n"
                + "                             cc.id, \n"
                + "                             z.Zonecode, \n"
                + "                             bb.branchcode, \n"
                + "                             cg.name, \n"
                + "                             cg.id \n"
                + "                  )                       a1 \n"
                + "           WHERE  " + days + "\n"
                + "           GROUP BY \n"
                + "                  a1.clientGroupID, \n"
                + "                  a1.GroupName \n"
                + "       )                          p1 \n"
                + "        \n"
                + "GROUP BY \n"
                + "       p1.clientGroupID, \n"
                + "       p1.GroupName \n"
                + "  having sum(outstandingAmount) = sum(InvoiceAmount)  \n";

            string sql_ = "SELECT p1.ClientName,  \n"
               + "       p1.AccountNo,  \n"
               + "       SUM(InvoiceAmount)         InvoiceAmount,  \n"
               + "       SUM(outstandingAmount)     outstandingAmount  \n"
               + "FROM   (  \n"
               + "           SELECT a.ClientName,  \n"
               + "                  a.ClientAccountno AccountNo,  \n"
               + "                  SUM(a.BILLAMOUNT)      InvoiceAmount,  \n"
               + "                  SUM(a.OUTSTANDING)     outstandingAmount  \n"
               + "           FROM   (  \n"
               + "                      SELECT z.name      ZoneName,  \n"
               + "                             z.zonecode,  \n"
               + "                             BB.name     BranchName,  \n"
               + "                             bb.branchcode,  \n"
               + "                             CC.name     ClientName,  \n"
               + "                             cc.id       clientid,  \n"
               + "                             Cc.accountNo ClientAccountno,  \n"
               + "                             MONTH(I.startDate) BILLMONTH,  \n"
               + "                             YEAR(I.startDate) BILLYEAR,  \n"
               + "                             LEFT(DATENAME(MONTH, I.startDate), 3) + '-' + RIGHT(YEAR(I.startDate), 2)   \n"
               + "                             BILLDATE,  \n"
               + "                             i.invoiceNumber,  \n"
               + "                             DATEDIFF(DAY, i.endDate, GETDATE()) DayCount,  \n"
               + "                             SUM(  \n"
               + "                                 CASE   \n"
               + "                                      WHEN I.TOTALAMOUNT IS NULL THEN '0'  \n"
               + "                                      ELSE I.TOTALAMOUNT  \n"
               + "                                 END  \n"
               + "                             )           BILLAMOUNT,  \n"
               + "                             0           OUTSTANDING  \n"
               + "                      FROM   INVOICE I  \n"
               + "                             LEFT JOIN (  \n"
               + "                                      SELECT I1.INVOICENUMBER,  \n"
               + "                                             SUM(G.AMOUNT)JV  \n"
               + "                                      FROM   INVOICE I1  \n"
               + "                                             INNER JOIN GENERALVOUCHER G  \n"
               + "                                                  ON  I1.INVOICENUMBER = G.INVOICENO  \n"
               + "                                      GROUP BY  \n"
               + "                                             I1.INVOICENUMBER  \n"
               + "                                  ) JV2  \n"
               + "                                  ON  I.INVOICENUMBER = JV2.INVOICENUMBER  \n"
               + "                             LEFT JOIN (  \n"
               + "                                      SELECT I2.INVOICENUMBER,  \n"
               + "                                             SUM(IR.AMOUNT)RR  \n"
               + "                                      FROM   INVOICE I2  \n"
               + "                                             INNER JOIN INVOICEREDEEM IR  \n"
               + "                                                  ON  I2.INVOICENUMBER = IR.INVOICENO  \n"
               + "                                             LEFT JOIN PAYMENTVOUCHERS A  \n"
               + "                                                  ON  IR.PAYMENTVOUCHERID = A.ID  \n"
               + "                                             LEFT JOIN PAYMENTSOURCE B  \n"
               + "                                                  ON  A.PAYMENTSOURCEID = B.ID  \n"
               + "                                             LEFT JOIN PAYMENTTYPES F  \n"
               + "                                                  ON  A.PAYMENTTYPEID = F.ID  \n"
               + "                                             LEFT JOIN CHEQUESTATUS CS1  \n"
               + "                                                  ON  A.ID = CS1.PAYMENTVOUCHERID  \n"
               + "                                             LEFT JOIN CHEQUESTATE CS2  \n"
               + "                                                  ON  CS1.CHEQUESTATEID = CS2.ID  \n"
               + "                                                  AND CS1.ISCURRENTSTATE = '1'  \n"
               + "                                      GROUP BY  \n"
               + "                                             I2.INVOICENUMBER  \n"
               + "                                  ) RR2  \n"
               + "                                  ON  I.INVOICENUMBER = RR2.INVOICENUMBER  \n"
               + "                             INNER JOIN CREDITCLIENTS CC  \n"
               + "                                  ON  I.CLIENTID = CC.ID  \n"
               + "                                  AND (i.IsInvoiceCanceled IS NULL OR i.IsInvoiceCanceled = '0')  \n"
               + "                             INNER JOIN BRANCHES BB  \n"
               + "                                  ON  CC.BRANCHCODE = BB.BRANCHCODE  \n"
               + "                             INNER JOIN ZONES Z  \n"
               + "                                  ON  BB.ZONECODE = Z.ZONECODE  \n"
               + "                      WHERE  i.IsInvoiceCanceled = '0'  \n"
               + "                             AND CAST(I.INVOICEDATE AS date) <  = CAST(GETDATE() AS date)  \n"
               + "                             AND ISNULL(cc.clientGrpId,'0') ='0' \n";
            sql_ = sql_ + clientgroup;
            sql_ = sql_ + "GROUP BY \n"
               + "                             MONTH(I.startDate),  \n"
               + "                             YEAR(I.startDate),  \n"
               + "                             LEFT(DATENAME(MONTH, I.INVOICEDATE), 3) + '-' +   \n"
               + "                             RIGHT(YEAR(I.INVOICEDATE), 2),  \n"
               + "                             i.invoiceNumber,  \n"
               + "                             I.endDate,  \n"
               + "                             I.INVOICEDATE,  \n"
               + "                             z.name,  \n"
               + "                             BB.name,  \n"
               + "                             CC.name,  \n"
               + "                             startDate,  \n"
               + "                             Cc.accountNo,  \n"
               + "                             cc.id,  \n"
               + "                             z.Zonecode,  \n"
               + "                             bb.branchcode \n"
               + "                  )                      a  \n"
               + "           WHERE  " + days + "\n"
               + "           GROUP BY  \n"
               + "                 a.ClientName,  \n"
               + "                  a.ClientAccountno \n"
               + "           UNION ALL  \n"
               + "           SELECT  a1.ClientName,  \n"
               + "                  a1.ClientAccountno,  \n"
               + "                  SUM(a1.BILLAMOUNT)      InvoiceAmount,  \n"
               + "                  SUM(a1.OUTSTANDING)     outstandingAmount  \n"
               + "           FROM   (  \n"
               + "                      SELECT z.name      ZoneName,  \n"
               + "                             z.zonecode,  \n"
               + "                             BB.name     BranchName,  \n"
               + "                             bb.branchcode,  \n"
               + "                             CC.name     ClientName,  \n"
               + "                             cc.id       clientid,  \n"
               + "                             Cc.accountNo ClientAccountno,  \n"
               + "                             MONTH(I.startDate) BILLMONTH,  \n"
               + "                             YEAR(I.startDate) BILLYEAR,  \n"
               + "                             LEFT(DATENAME(MONTH, I.startDate), 3) + '-' + RIGHT(YEAR(I.startDate), 2)   \n"
               + "                             BILLDATE,  \n"
               + "                             i.invoiceNumber,  \n"
               + "                             DATEDIFF(DAY, i.endDate, GETDATE()) DayCount,  \n"
               + "                             0           BILLAMOUNT,  \n"
               + "                           case when   SUM( \n"
               + "                                 ROUND( \n"
               + "                                     CASE  \n"
               + "                                          WHEN I.TOTALAMOUNT IS NULL THEN '0' \n"
               + "                                          ELSE I.TOTALAMOUNT \n"
               + "                                     END -( \n"
               + "                                         CASE  \n"
               + "                                              WHEN RR2.RR IS NULL THEN '0' \n"
               + "                                              ELSE RR2.RR \n"
               + "                                         END + CASE  \n"
               + "                                                    WHEN JV2.JV IS NULL THEN '0' \n"
               + "                                                    ELSE JV2.JV \n"
               + "                                               END \n"
               + "                                     ), \n"
               + "                                     2 \n"
               + "                                 ) \n"
               + "                             )  < 0 then 0 else \n"
               + "                                  SUM( \n"
               + "                                 ROUND( \n"
               + "                                     CASE  \n"
               + "                                          WHEN I.TOTALAMOUNT IS NULL THEN '0' \n"
               + "                                          ELSE I.TOTALAMOUNT \n"
               + "                                     END -( \n"
               + "                                         CASE  \n"
               + "                                              WHEN RR2.RR IS NULL THEN '0' \n"
               + "                                              ELSE RR2.RR \n"
               + "                                         END + CASE  \n"
               + "                                                    WHEN JV2.JV IS NULL THEN '0' \n"
               + "                                                    ELSE JV2.JV \n"
               + "                                               END \n"
               + "                                     ), \n"
               + "                                     2 \n"
               + "                                 ) \n"
               + "                             ) end  OUTSTANDING \n"
               + "                      FROM   INVOICE I  \n"
               + "                             LEFT JOIN (  \n"
               + "                                      SELECT I1.INVOICENUMBER,  \n"
               + "                                             SUM(G.AMOUNT)JV  \n"
               + "                                      FROM   INVOICE I1  \n"
               + "                                             INNER JOIN GENERALVOUCHER G  \n"
               + "                                                  ON  I1.INVOICENUMBER = G.INVOICENO  \n"
               + "                                      GROUP BY  \n"
               + "                                             I1.INVOICENUMBER  \n"
               + "                                  ) JV2  \n"
               + "                                  ON  I.INVOICENUMBER = JV2.INVOICENUMBER  \n"
               + "                             LEFT JOIN (  \n"
               + "                                      SELECT I2.INVOICENUMBER,  \n"
               + "                                             SUM(IR.AMOUNT)RR  \n"
               + "                                      FROM   INVOICE I2  \n"
               + "                                             INNER JOIN INVOICEREDEEM IR  \n"
               + "                                                  ON  I2.INVOICENUMBER = IR.INVOICENO  \n"
               + "                                             LEFT JOIN PAYMENTVOUCHERS A  \n"
               + "                                                  ON  IR.PAYMENTVOUCHERID = A.ID  \n"
               + "                                             LEFT JOIN PAYMENTSOURCE B  \n"
               + "                                                  ON  A.PAYMENTSOURCEID = B.ID  \n"
               + "                                             LEFT JOIN PAYMENTTYPES F  \n"
               + "                                                  ON  A.PAYMENTTYPEID = F.ID  \n"
               + "                                             LEFT JOIN CHEQUESTATUS CS1  \n"
               + "                                                  ON  A.ID = CS1.PAYMENTVOUCHERID  \n"
               + "                                             LEFT JOIN CHEQUESTATE CS2  \n"
               + "                                                  ON  CS1.CHEQUESTATEID = CS2.ID  \n"
               + "                                                  AND CS1.ISCURRENTSTATE = '1'  \n"
               + "                                      GROUP BY  \n"
               + "                                             I2.INVOICENUMBER  \n"
               + "                                  ) RR2  \n"
               + "                                  ON  I.INVOICENUMBER = RR2.INVOICENUMBER  \n"
               + "                             INNER JOIN CREDITCLIENTS CC  \n"
               + "                                  ON  I.CLIENTID = CC.ID  \n"
               + "                                  AND (i.IsInvoiceCanceled IS NULL OR i.IsInvoiceCanceled = '0')  \n"
               + "                             INNER JOIN BRANCHES BB  \n"
               + "                                  ON  CC.BRANCHCODE = BB.BRANCHCODE  \n"
               + "                             INNER JOIN ZONES Z  \n"
               + "                                  ON  BB.ZONECODE = Z.ZONECODE  \n"
               + "                      WHERE  i.IsInvoiceCanceled = '0'  \n"
               + "                             AND CAST(I.INVOICEDATE AS date) <  = CAST(GETDATE() AS date)  \n"
               + "                             AND ISNULL(cc.clientGrpId,'0') ='0'\n"
               + "   						   AND cast(i.startDate as date) >='2015-07-01'\n";

            sql_ = sql_ + clientgroup;
            sql_ = sql_ + "GROUP BY \n"
               + "                             MONTH(I.startDate),  \n"
               + "                             YEAR(I.startDate),  \n"
               + "                             LEFT(DATENAME(MONTH, I.INVOICEDATE), 3) + '-' +   \n"
               + "                             RIGHT(YEAR(I.INVOICEDATE), 2),  \n"
               + "                             i.invoiceNumber,  \n"
               + "                             I.endDate,  \n"
               + "                             I.INVOICEDATE,  \n"
               + "                             z.name,  \n"
               + "                             BB.name,  \n"
               + "                             CC.name,  \n"
               + "                             startDate,  \n"
               + "                             Cc.accountNo,  \n"
               + "                             cc.id,  \n"
               + "                             z.Zonecode,  \n"
               + "                             bb.branchcode \n"
               + "                  )                       a1  \n"
               + "           WHERE  " + days + "\n"
               + "           GROUP BY  \n"
               + "                   a1.ClientName,  \n"
               + "                  a1.ClientAccountno \n"
               + "       )                          p1  \n"
               + "         \n"
               + "GROUP BY  \n"
               + "       p1.ClientName,  \n"
               + "       p1.AccountNo  \n"
               + "  having sum(outstandingAmount) = sum(InvoiceAmount)  \n";

            DataTable Ds_1 = new DataTable();

            string sql2 = "";
            if (bt_1.SelectedValue == "0")
            {
                sql2 = sql;
            }
            else
            {
                sql2 = sql_;
            }

            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sql2, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(Ds_1);
                orcl.Close();
            }
            catch (Exception Err)
            { }
            finally
            { }

            if (Ds_1.Rows.Count != 0)
            {
                if (bt_1.SelectedValue == "0")
                {
                    gv_tariff.Visible = true;

                    gv_tariff.DataSource = Ds_1.DefaultView;
                    gv_tariff.DataBind();

                    gv_tariff_.DataSource = null;
                    gv_tariff_.DataBind();
                    gv_tariff_.Visible = false;

                }
                else
                {
                    gv_tariff_.Visible = true;

                    gv_tariff_.DataSource = Ds_1.DefaultView;
                    gv_tariff_.DataBind();

                    gv_tariff.DataSource = null;
                    gv_tariff.DataBind();
                    gv_tariff.Visible = false;

                }
            }
            else
            {
                gv_tariff.DataSource = null;
                gv_tariff.DataBind();

            }
        }
        protected void gv_tariff_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }
        protected void gv_tariff_DataBound(object sender, EventArgs e)
        {

        }
        protected void gv_tariff_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void btn_save_Click(object sender, EventArgs e)
        {
            if (txt_date.Text.Length == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Date Selection is Compulsory')", true);
                //   err_msg.Text = "Cannot Update Cash Tariff";
                return;
            }


            int count = 0;
            string sql = "INSERT INTO Mnp_Recovery_Letter_info \n"
               + "( \n"
               + "	-- Id -- this column value is auto-generated \n"
               + "	ClientGroupID, \n"
               + "	Days, \n"
               + "	Letter_Type, \n"
               + "	invoiceAmount, \n"
               + "	OutStandingAmount, \n"
               + "	Remarks, \n"
               + "	Letter_RefNo, \n"
               + "	EntryDate, \n"
               + "	CreatedBy,CreatedOn,Ref_LetterNo,AccountNo \n"
               + "	 \n"
               + ")";

            int count_ = 0;
            string sql_ = "";
            GridView gg;
            if (bt_1.SelectedValue == "0")
            {
                gg = gv_tariff;
            }
            else
            {
                gg = gv_tariff_;
            }

            foreach (GridViewRow gr in gg.Rows)
            {
                CheckBox cb = (CheckBox)gr.FindControl("cb_Status");
                if (cb.Checked == true)
                {
                    count_++;
                    string clientid = ((HiddenField)gr.FindControl("hd_clientid")).Value;
                    string InvoiceAmount = ((Label)gr.FindControl("txt_InvoiceAmount")).Text;
                    string Outstanding = ((Label)gr.FindControl("txt_Outstanding")).Text;
                    string comments = ((TextBox)gr.FindControl("txt_comments")).Text;
                    string Visitdate = txt_date.Text;
                    string DayCount = dd_Days.SelectedValue;
                    string Letter = "";
                    string Letter_NO = "";
                    string Letter_Refno = "";


                    if (DayCount == "60")
                    {
                        DataSet ds = LetterNumber("1");
                        Letter_NO = ds.Tables[0].Rows[0][0].ToString();
                        Letter = "224";
                    }
                    else if (DayCount == "90")
                    {
                        DataSet ds = LetterNumber("2");
                        Letter_NO = ds.Tables[0].Rows[0][0].ToString();
                        Letter = "225";
                        DataSet ds_ = post_letter_Refno(clientid);
                        if (ds_ != null)
                        {
                            if (ds_.Tables[0].Rows.Count != 0)
                            {
                                Letter_Refno = ds_.Tables[0].Rows[0][0].ToString();
                            }
                        }
                    }

                    if (bt_1.SelectedValue == "0")
                    {
                        sql_ = sql_ + "SELECT \n"
                                   + "	'" + clientid + "', \n"
                                   + "  '" + DayCount + "', \n"
                                   + "	'" + Letter + "', \n"
                                   + "	'" + InvoiceAmount + "', \n"
                                   + "	'" + Outstanding + "', \n"
                                   + "	'" + comments + "', \n"
                                   + "	'" + Letter_NO + "', \n"
                                   + "	'" + Visitdate + "', \n"
                                   + "	'" + Session["U_ID"].ToString() + "', \n"
                                   + "	getdate(), \n"
                                   + " '" + Letter_Refno + "' Letter_Refno, \n"
                                   + " '' \n"

                                   + " UNion All \n";
                    }
                    else
                    {
                        sql_ = sql_ + "SELECT \n"
                                   + "	'', \n"
                                   + "  '" + DayCount + "', \n"
                                   + "	'" + Letter + "', \n"
                                   + "	'" + InvoiceAmount + "', \n"
                                   + "	'" + Outstanding + "', \n"
                                   + "	'" + comments + "', \n"
                                   + "	'" + Letter_NO + "', \n"
                                   + "	'" + Visitdate + "', \n"
                                   + "	'" + Session["U_ID"].ToString() + "', \n"
                                   + "	getdate(), \n"
                                   + " '" + Letter_Refno + "' Letter_Refno, \n"
                                   + "	'" + clientid + "' AccountnNo \n"

                                   + " UNion All \n";

                    }
                    count++;
                }
            }
            if (count != 0)
            {
                sql_ = sql_.Remove(sql_.Length - 11);
                sql = sql + sql_;
            }
            if (count_ == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert(' was not Selected')", true);
                //   err_msg.Text = "Cannot Update Cash Tariff";
                return;

            }

            SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            try
            {
                sqlcon.Open();
                SqlCommand sqlcmd = new SqlCommand(sql, sqlcon);
                sqlcmd.CommandType = CommandType.Text;

                sqlcmd.ExecuteNonQuery();
                ////IsUnique = Int32.Parse(SParam.Value.ToString());
                //// obj.XCode = obj.consignmentNo;
                //sqlcon.Close();

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Records have been added')", true);

                gv_tariff.DataSource = null;
                gv_tariff.DataBind();

            }
            catch (Exception ex)
            {
                //break;
                err_msg.Text = ex.Message.ToString();// "Zone Must be selected";


            }
        }

        public DataSet LetterNumber(string identifier)
        {
            DataSet ds = new DataSet();
            SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            try
            {
                sqlcon.Open();
                SqlCommand sqlcmd = new SqlCommand("sp_LetterRefNO", sqlcon);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.AddWithValue("identifier", identifier);
                SqlDataAdapter sda = new SqlDataAdapter(sqlcmd);
                sda.Fill(ds);

                //IsUnique = Int32.Parse(SParam.Value.ToString());
                // obj.XCode = obj.consignmentNo;
                sqlcon.Close();
            }
            catch (Exception Err)
            {

            }
            return ds;
        }


        public DataSet post_letter_Refno(string identifier)
        {
            DataSet ds = new DataSet();
            SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            try
            {
                string sql = "SELECT * FROM Mnp_Recovery_Letter_info mrli WHERE mrli.ClientGroupID ='" + identifier + "' AND mrli.Days <'90' AND mrli.Letter_Type='224'  ORDER BY mrli.CreatedOn DESC  \n";
                sqlcon.Open();
                SqlCommand sqlcmd = new SqlCommand(sql, sqlcon);
                sqlcmd.CommandType = CommandType.Text;
                sqlcmd.Parameters.AddWithValue("identifier", identifier);
                SqlDataAdapter sda = new SqlDataAdapter(sqlcmd);
                sda.Fill(ds);

                //IsUnique = Int32.Parse(SParam.Value.ToString());
                // obj.XCode = obj.consignmentNo;
                sqlcon.Close();
            }
            catch (Exception Err)
            {

            }
            return ds;
        }

        protected void btn_cancel_Click(object sender, EventArgs e)
        {

        }
        protected void gv_tariff_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        protected void bt_1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (bt_1.SelectedValue == "0")
            {
                lb_1.Text = "Group ID";
            }
            else
            {
                lb_1.Text = "Account No";
            }
        }
    }
}