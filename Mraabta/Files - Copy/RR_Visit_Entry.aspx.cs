using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using MRaabta.App_Code;

namespace MRaabta.Files
{
    public partial class RR_Visit_Entry : System.Web.UI.Page
    {
        CommonFunction fun = new CommonFunction();
        Cl_Variables clvar = new Cl_Variables();
        Consignemnts con = new Consignemnts();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Complevel1();
                Complevel2();
                Complevel3();
                Get_Years();

                dd_Year.SelectedValue = DateTime.Now.Year.ToString();
                dd_Month.SelectedValue = DateTime.Now.ToString("MM");

                txt_date.Text = DateTime.Today.ToString("yyyy-MM-dd");
                txt_date.Enabled = false;
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
                    //   dt.Clear();// = null;
                    dd_Complevel3.Items.Clear();
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
            string sql = "SELECT cl.comp1 HeadID,upper(cl.Name) HeadName FROM Comp_Level1 cl WHERE cl.[Status]='1' and id ='1' order by cl.Name asc";

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
        public void get_year()
        {
            //string query = "select zoneCode, name from Zones where status = '1' order by name";

            string sql = "SELECT cl.year, cl.FromDate + '||' + cl.ToDate Y1 FROM Fiscal_year cl WHERE cl.[Status]='1' order by year desc ";

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

            if (dt.Rows.Count != 0)
            {
                dd_Year.DataTextField = "year";
                dd_Year.DataValueField = "Y1";
                dd_Year.DataSource = dt.DefaultView;
                dd_Year.DataBind();
            }

        }
        public void Get_Years()
        {
            var currentYear = DateTime.Today.Year;
            for (int i = 3; i >= 0; i--)
            {
                // Now just add an entry that's the current year minus the counter
                dd_Year.Items.Add((currentYear - i).ToString());
            }
        }
        public DataTable get_Complevel2()
        {
            string sql = "";
            if (Session["U_ID"].ToString() == "5280")
            {
                sql = "SELECT cl.comp2 ManagerID,upper(cl.Name) ManagerName FROM Comp_Level2 cl WHERE cl.[Status]='1' and comp1='" + dd_Complevel1.SelectedValue + "' ORDER BY 2 ASC";
            }
            else
            {
                sql = @"SELECT cl.comp2 ManagerID,upper(cl.Name) ManagerName 
                    FROM Comp_Level2 cl 
                    WHERE cl.[Status]='1' 
                    and comp1='" + dd_Complevel1.SelectedValue + @"' 
                    and addtional_Fields2 = '" + Session["U_ID"].ToString() + @"'
                    ORDER BY 2 ASC";
            }

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
            string sql = "";

            if (dd_Complevel2.SelectedValue == "" || dd_Complevel2.SelectedValue == "0")
            {
                sql = "SELECT upper(cl.Name)+' ('+u.employeeCode+')' officerName,cl.comp3 OfficerID, branch \n" +
                            "FROM Comp_Level3 cl \n" +
                            "INNER JOIN UserAssociation u ON cl.Name = u.username \n" +
                            "WHERE cl.[Status]='1' \n" +
                            "and addtional_Fields4 = '" + Session["U_ID"].ToString() + "'  order by cl.Name asc";
            }
            else
            {
                sql = "SELECT upper(cl.Name)+' ('+u.employeeCode+')' officerName,cl.comp3 OfficerID, branch \n" +
                           "FROM Comp_Level3 cl \n" +
                           "INNER JOIN UserAssociation u ON cl.Name = u.username \n" +
                           "WHERE cl.[Status]='1' \n" +
                           "and comp1='" + dd_Complevel1.SelectedValue + "' \n" +
                           "and cl.comp2='" + dd_Complevel2.SelectedValue + "'  order by cl.Name asc";
            }
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
        protected void btn_showTariff_Click(object sender, EventArgs e)
        {
            //if (txt_accountNo.Text.Length == 0)
            //{
            //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Account No is Mandatory')", true);
            //    //   err_msg.Text = "Cannot Update Cash Tariff";
            //    return;
            //}
            string Day = "";
            if (dd_Complevel2.SelectedValue == "0004")
            {
                Day = " and DayCount > 90 \n";
            }
            else
            {
                //Day = " and DayCount <=90 \n";
            }
            string accountno = "", clientgroup = "", InvoiceNumber = "";
            if (txt_accountNo.Text != "")
            {
                accountno = " and cc.accountno='" + txt_accountNo.Text + "' \n";
            }
            if (this.txt_groupID.Text != "")
            {
                clientgroup = " and cc.clientGrpId='" + txt_groupID.Text + "' \n";
            }
            /*
            string fromdate = "", todate = "";
            if (this.dd_Year.SelectedValue != "")
            {
                string[] year = dd_Year.SelectedValue.Split('|');
                fromdate = year[0].ToString();
                todate = year[2].ToString();
            }
            */
            string invoiceYear = "", invoiceMonth = "";
            if (chk_year_all.Checked)
            {
                invoiceYear = "";
            }
            else
            {
                invoiceYear = "AND YEAR(I.startDate) = '" + dd_Year.SelectedValue + "' \n";
            }

            if (chk_month_all.Checked)
            {
                invoiceMonth = "";
            }
            else
            {
                invoiceMonth = "AND MONTH(I.startDate) = '" + dd_Month.SelectedValue + "' \n";
            }

            if (txt_invoiceNumber.Text != "")
            {
                InvoiceNumber = " and i.invoiceNumber ='" + txt_invoiceNumber.Text + "' \n";
            }

            #region sql__23092020
            string sql__23092020 = "SELECT p.* , \n"
               + "CASE                 \n"
               + "   WHEN p.OUTSTANDING != 0 AND p.BILLAMOUNT != 0 THEN ROUND((p.OUTSTANDING / p.BILLAMOUNT)   \n"
               + "        * 100,2)        \n"
               + "   ELSE '0'            \n"
               + " END     per2         \n"
               + "FROM   ( \n"
               + "           SELECT z.name                   ZoneName,z.zonecode, \n"
               + "                  BB.name                  BranchName, bb.branchcode, \n"
               + "                  CC.name                  ClientName,cc.id clientid, \n"
               + "                  Cc.accountNo             ClientAccountno, \n"
               + "                  MONTH(I.startDate)     BILLMONTH, \n"
               + "                  YEAR(I.startDate)      BILLYEAR, \n"
               + "                  LEFT(DATENAME(MONTH, I.startDate), 3) + '-' + RIGHT(YEAR(I.startDate), 2)  \n"
               + "                  BILLDATE, \n"
               + "                  DATEDIFF(DAY, i.endDate, GETDATE()) DayCount, \n"
               + "                  i.invoiceNumber, \n"
               + "                  SUM( \n"
               + "                      CASE  \n"
               + "                           WHEN I.TOTALAMOUNT IS NULL THEN '0' \n"
               + "                           ELSE I.TOTALAMOUNT \n"
               + "                      END \n"
               + "                  )                        BILLAMOUNT, \n"
               + "                  SUM( \n"
               + "                      ROUND( \n"
               + "                          CASE  \n"
               + "                               WHEN I.TOTALAMOUNT IS NULL THEN '0' \n"
               + "                               ELSE I.TOTALAMOUNT \n"
               + "                          END -( \n"
               + "                              CASE  \n"
               + "                                   WHEN RR2.RR IS NULL THEN '0' \n"
               + "                                   ELSE RR2.RR \n"
               + "                              END + CASE  \n"
               + "                                         WHEN JV2.JV IS NULL THEN '0' \n"
               + "                                         ELSE JV2.JV \n"
               + "                                    END \n"
               + "                          ), \n"
               + "                          2 \n"
               + "                      ) \n"
               + "                  )                        OUTSTANDING, \n"
               + "                  ROUND( \n"
               + "                      ( \n"
               + "                          SUM( \n"
               + "                              ROUND( \n"
               + "                                  CASE  \n"
               + "                                       WHEN I.TOTALAMOUNT IS NULL THEN '0' \n"
               + "                                       ELSE I.TOTALAMOUNT \n"
               + "                                  END -( \n"
               + "                                      CASE  \n"
               + "                                           WHEN RR2.RR IS NULL THEN '0' \n"
               + "                                           ELSE RR2.RR \n"
               + "                                      END + CASE  \n"
               + "                                                 WHEN JV2.JV IS NULL THEN '0' \n"
               + "                                                 ELSE JV2.JV \n"
               + "                                            END \n"
               + "                                  ), \n"
               + "                                  2 \n"
               + "                              ) \n"
               + "                          ) \n"
               + "                          / \n"
               + "                          SUM( \n"
               + "                              CASE  \n"
               + "                                   WHEN I.TOTALAMOUNT IS NULL THEN '0' \n"
               + "                                   ELSE I.TOTALAMOUNT \n"
               + "                              END \n"
               + "                          ) \n"
               + "                      ) * 100, \n"
               + "                      2 \n"
               + "                  )                        PERCENTAGE \n"
               + "           FROM   INVOICE I \n"
               + "                  LEFT JOIN ( \n"
               + "                           SELECT I1.INVOICENUMBER, \n"
               + "                                  SUM(G.AMOUNT)JV \n"
               + "                           FROM   INVOICE I1 \n"
               + "                                  INNER JOIN GENERALVOUCHER G \n"
               + "                                       ON  I1.INVOICENUMBER = G.INVOICENO \n"
               + "                           GROUP BY \n"
               + "                                  I1.INVOICENUMBER \n"
               + "                       ) JV2 \n"
               + "                       ON  I.INVOICENUMBER = JV2.INVOICENUMBER \n"
               + "                  LEFT JOIN ( \n"
               + "                           SELECT I2.INVOICENUMBER, \n"
               + "                                  SUM(IR.AMOUNT)RR \n"
               + "                           FROM   INVOICE I2 \n"
               + "                                  INNER JOIN INVOICEREDEEM IR \n"
               + "                                       ON  I2.INVOICENUMBER = IR.INVOICENO \n"
               + "                                  LEFT JOIN PAYMENTVOUCHERS A \n"
               + "                                       ON  IR.PAYMENTVOUCHERID = A.ID \n"
               + "                                  LEFT JOIN PAYMENTSOURCE B \n"
               + "                                       ON  A.PAYMENTSOURCEID = B.ID \n"
               + "                                  LEFT JOIN PAYMENTTYPES F \n"
               + "                                       ON  A.PAYMENTTYPEID = F.ID \n"
               + "                                  LEFT JOIN CHEQUESTATUS CS1 \n"
               + "                                       ON  A.ID = CS1.PAYMENTVOUCHERID \n"
               + "                                  LEFT JOIN CHEQUESTATE CS2 \n"
               + "                                       ON  CS1.CHEQUESTATEID = CS2.ID \n"
               + "                                       AND CS1.ISCURRENTSTATE = '1' \n"
               + "                           GROUP BY \n"
               + "                                  I2.INVOICENUMBER \n"
               + "                       ) RR2 \n"
               + "                       ON  I.INVOICENUMBER = RR2.INVOICENUMBER \n"
               + "                  INNER JOIN CREDITCLIENTS CC \n"
               + "                       ON  I.CLIENTID = CC.ID \n"
               + "                       AND (i.IsInvoiceCanceled IS NULL OR i.IsInvoiceCanceled = '0') \n"
               + "                  INNER JOIN BRANCHES BB \n"
               + "                       ON  CC.BRANCHCODE = BB.BRANCHCODE \n"
               + "                  INNER JOIN ZONES Z \n"
               + "                       ON  BB.ZONECODE = Z.ZONECODE \n"
               + "                  INNER JOIN ClientStaff_temp cs \n"
               + "                       ON  cs.ClientId = i.clientId \n"
               + "                  INNER JOIN Comp_Level3 cl \n"
               + "                       ON  cl.comp3 = cs.comp3 \n"
               + "           WHERE  i.IsInvoiceCanceled = '0' \n"
               + "                  AND cs.[status] = '1' \n"
               + "                  AND cl.[Status] = '1' \n"
               + invoiceYear + invoiceMonth
               //+ "                  and cast(I.INVOICEDATE as date) >='" + fromdate + "' \n"
               //+ "                  and cast(I.INVOICEDATE as date) <='" + todate + "' \n"

               + "                  AND cl.comp3 ='" + dd_Complevel3.SelectedValue + "'\n";
            sql__23092020 = sql__23092020 + accountno;
            sql__23092020 = sql__23092020 + clientgroup;
            sql__23092020 = sql__23092020 + InvoiceNumber;
            sql__23092020 += "           GROUP BY \n"
             + "                  MONTH(I.startDate), \n"
             + "                  YEAR(I.startDate), \n"
             + "                  LEFT(DATENAME(MONTH, I.startDate), 3) + '-' + RIGHT(YEAR(I.startDate), 2) , \n"
             + "                  i.invoiceNumber,I.startDate, \n"
             + "                  I.INVOICEDATE, \n"
             + "                  z.name, \n"
             + "                  BB.name, \n"
             + "                  CC.name,startDate, \n"
             + "                  Cc.accountNo,cc.id ,z.Zonecode,bb.branchcode,i.endDate \n"
             + "          \n"
             + "       ) p \n"
             + "WHERE  OUTSTANDING >10 " +
             " \n";
            sql__23092020 = sql__23092020 + Day;
            sql__23092020 = sql__23092020 + " \n"
             + "ORDER BY p.BILLYEAR DESC ,p.BILLMONTH, p.ClientName";

            #endregion

            string sql = "SELECT p.* , \n"
               + "CASE                 \n"
               + "   WHEN p.OUTSTANDING != 0 AND p.BILLAMOUNT != 0 THEN ROUND((p.OUTSTANDING / p.BILLAMOUNT)   \n"
               + "        * 100,2)        \n"
               + "   ELSE '0'            \n"
               + " END     per2         \n"
               + "FROM   ( \n"
               + "           SELECT z.name                   ZoneName,z.zonecode, \n"
               + "                  BB.name                  BranchName, bb.branchcode, \n"
               + "                  CC.name                  ClientName,cc.id clientid, \n"
               + "                  Cc.accountNo             ClientAccountno, \n"
               + "                  MONTH(I.startDate)     BILLMONTH, \n"
               + "                  YEAR(I.startDate)      BILLYEAR, \n"
               + "                  LEFT(DATENAME(MONTH, I.startDate), 3) + '-' + RIGHT(YEAR(I.startDate), 2)  \n"
               + "                  BILLDATE, \n"
               + "                  DATEDIFF(DAY, i.endDate, GETDATE()) DayCount, \n"
               + "                  i.invoiceNumber, \n"
               + "                  SUM( \n"
               + "                      CASE  \n"
               + "                           WHEN I.TOTALAMOUNT IS NULL THEN '0' \n"
               + "                           ELSE I.TOTALAMOUNT \n"
               + "                      END \n"
               + "                  )                        BILLAMOUNT, \n"
               + "                  SUM( \n"
               + "                      ROUND( \n"
               + "                          CASE  \n"
               + "                               WHEN I.TOTALAMOUNT IS NULL THEN '0' \n"
               + "                               ELSE I.TOTALAMOUNT \n"
               + "                          END -( \n"
               + "                              CASE  \n"
               + "                                   WHEN RR2.RR IS NULL THEN '0' \n"
               + "                                   ELSE RR2.RR \n"
               + "                              END + CASE  \n"
               + "                                         WHEN JV2.JV IS NULL THEN '0' \n"
               + "                                         ELSE JV2.JV \n"
               + "                                    END \n"
               + "                          ), \n"
               + "                          2 \n"
               + "                      ) \n"
               + "                  )                        OUTSTANDING, \n"
               + "                  ROUND( \n"
               + "                      ( \n"
               + "                          SUM( \n"
               + "                              ROUND( \n"
               + "                                  CASE  \n"
               + "                                       WHEN I.TOTALAMOUNT IS NULL THEN '0' \n"
               + "                                       ELSE I.TOTALAMOUNT \n"
               + "                                  END -( \n"
               + "                                      CASE  \n"
               + "                                           WHEN RR2.RR IS NULL THEN '0' \n"
               + "                                           ELSE RR2.RR \n"
               + "                                      END + CASE  \n"
               + "                                                 WHEN JV2.JV IS NULL THEN '0' \n"
               + "                                                 ELSE JV2.JV \n"
               + "                                            END \n"
               + "                                  ), \n"
               + "                                  2 \n"
               + "                              ) \n"
               + "                          ) \n"
               + "                          / \n"
               + "                          SUM( \n"
               + "                              CASE  \n"
               + "                                   WHEN I.TOTALAMOUNT IS NULL THEN '0' \n"
               + "                                   ELSE I.TOTALAMOUNT \n"
               + "                              END \n"
               + "                          ) \n"
               + "                      ) * 100, \n"
               + "                      2 \n"
               + "                  )                        PERCENTAGE \n"
               + "           FROM   INVOICE I \n"
               + "                  LEFT JOIN ( \n"
               + "                           SELECT I1.INVOICENUMBER, \n"
               + "                                  SUM(G.AMOUNT)JV \n"
               + "                           FROM   INVOICE I1 \n"
               + "                                  INNER JOIN GENERALVOUCHER G \n"
               + "                                       ON  I1.INVOICENUMBER = G.INVOICENO \n"
               + "                           GROUP BY \n"
               + "                                  I1.INVOICENUMBER \n"
               + "                       ) JV2 \n"
               + "                       ON  I.INVOICENUMBER = JV2.INVOICENUMBER \n"
               + "                  LEFT JOIN ( \n"
               + "                           SELECT I2.INVOICENUMBER, \n"
               + "                                  SUM(IR.AMOUNT)RR \n"
               + "                           FROM   INVOICE I2 \n"
               + "                                  INNER JOIN INVOICEREDEEM IR \n"
               + "                                       ON  I2.INVOICENUMBER = IR.INVOICENO \n"
               + "                                  LEFT JOIN PAYMENTVOUCHERS A \n"
               + "                                       ON  IR.PAYMENTVOUCHERID = A.ID \n"
               + "                                  LEFT JOIN PAYMENTSOURCE B \n"
               + "                                       ON  A.PAYMENTSOURCEID = B.ID \n"
               + "                                  LEFT JOIN PAYMENTTYPES F \n"
               + "                                       ON  A.PAYMENTTYPEID = F.ID \n"
               + "                                  LEFT JOIN CHEQUESTATUS CS1 \n"
               + "                                       ON  A.ID = CS1.PAYMENTVOUCHERID \n"
               + "                                  LEFT JOIN CHEQUESTATE CS2 \n"
               + "                                       ON  CS1.CHEQUESTATEID = CS2.ID \n"
               + "                                       AND CS1.ISCURRENTSTATE = '1' \n"
               + "                           GROUP BY \n"
               + "                                  I2.INVOICENUMBER \n"
               + "                       ) RR2 \n"
               + "                       ON  I.INVOICENUMBER = RR2.INVOICENUMBER \n"
               + "                  INNER JOIN CREDITCLIENTS CC \n"
               + "                       ON  I.CLIENTID = CC.ID \n"
               + "                       AND (i.IsInvoiceCanceled IS NULL OR i.IsInvoiceCanceled = '0') \n"
               + "                  INNER JOIN BRANCHES BB \n"
               + "                       ON  CC.BRANCHCODE = BB.BRANCHCODE \n"
               + "                  INNER JOIN ZONES Z \n"
               + "                       ON  BB.ZONECODE = Z.ZONECODE \n"
               + "                  INNER JOIN CLIENTSTAFF_TEMP cs ON  cs.ClientId = i.clientId \n"
               + "                  INNER JOIN USERASSOCIATION U ON CS.USERNAME = U.USERNAME \n"
               + "                  INNER JOIN Comp_Level3 cl ON u.username = cl.Name \n"
               + "           WHERE  i.IsInvoiceCanceled = '0' \n"
               + "                  AND cs.StaffTypeId = '215' \n"
               + "                  AND cl.[Status] = '1' AND cs.[Status] = '1' \n"
               + invoiceYear + invoiceMonth
               + "                  AND cl.comp3 ='" + dd_Complevel3.SelectedValue + "'\n";
            sql = sql + accountno;
            sql = sql + clientgroup;
            sql = sql + InvoiceNumber;
            sql += "           GROUP BY \n"
             + "                  MONTH(I.startDate), \n"
             + "                  YEAR(I.startDate), \n"
             + "                  LEFT(DATENAME(MONTH, I.startDate), 3) + '-' + RIGHT(YEAR(I.startDate), 2) , \n"
             + "                  i.invoiceNumber,I.startDate, \n"
             + "                  I.INVOICEDATE, \n"
             + "                  z.name, \n"
             + "                  BB.name, \n"
             + "                  CC.name,startDate, \n"
             + "                  Cc.accountNo,cc.id ,z.Zonecode,bb.branchcode,i.endDate \n"
             + "          \n"
             + "       ) p \n"
             + "WHERE  OUTSTANDING >10 " +
             " \n";
            sql = sql + Day;
            sql = sql + " \n"
             + "ORDER BY p.BILLYEAR DESC ,p.BILLMONTH, p.ClientName";


            DataTable Ds_1 = new DataTable();

            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sql, orcl);
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
                gv_tariff.DataSource = Ds_1.DefaultView;
                gv_tariff.DataBind();
            }
            else
            {
                gv_tariff.DataSource = null;
                gv_tariff.DataBind();

            }
        }
        protected void gv_tariff_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList dp = (e.Row.FindControl("dd_MajoreCat") as DropDownList);
                DataTable dt = MajoreCategory();

                dp.DataTextField = "Category";
                dp.DataValueField = "Id";
                dp.DataSource = dt.DefaultView;
                dp.DataBind();
            }
        }
        public DataTable MajoreCategory()
        {
            string sql = "SELECT * FROM Mnp_Major_Category mmc";

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
        protected void gv_tariff_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }
        protected void gv_tariff_DataBound(object sender, EventArgs e)
        {

        }
        protected void btn_save_Click(object sender, EventArgs e)
        {
            err_msg.Text = "";
            if (txt_date.Text.Length == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Date Selection is Compulsory')", true);
                //   err_msg.Text = "Cannot Update Cash Tariff";
                return;
            }
            if (dd_Complevel1.SelectedValue == "0")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Head of Dept Must be selected')", true);
                err_msg.Text = "Head of Dept Must be selected";
                return;
            }
            /* if (dd_Complevel2.SelectedValue == "0")
             {
                 ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Manager Must be selected')", true);
                 err_msg.Text = "Manager Must be selected";
                 return;
             }*/
            if (dd_Complevel3.SelectedValue == "0")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Officer Must be selected')", true);
                err_msg.Text = "Officer Must be selected";
                return;
            }

            int count = 0;
            string sql = "INSERT INTO Mnp_RecoveryVisit \n"
                       + "( \n"
                       + "	-- ID -- this column value is auto-generated \n"
                       + "	ZoneCode, \n"
                       + "  Branch,\n"
                       + "  visitdate,\n"
                       + "	ClientID, \n"
                       + "	ClientAccount,comp3, \n"
                       + "	InvoiceMonth, \n"
                       + "	InvoiceNumber, \n"
                       + "	Outstanding, \n"
                       + "	Major_Category, \n"
                       + "	comments, \n"
                       + "	createdby, \n"
                       + "	Created_On \n"
                       + ") \n";
            int count_ = 0;
            string sql_ = "";
            foreach (GridViewRow gr in gv_tariff.Rows)
            {
                CheckBox cb = (CheckBox)gr.FindControl("cb_Status");
                if (cb.Checked == true)
                {
                    count_++;
                    string clientid = ((HiddenField)gr.FindControl("hd_clientid")).Value;
                    string zoneid = ((HiddenField)gr.FindControl("hd_Zoneid")).Value;
                    string branch = ((HiddenField)gr.FindControl("hd_BranchId")).Value;
                    string AccountNo = ((Label)gr.FindControl("lbl_Account")).Text;
                    string billMonth = ((Label)gr.FindControl("txt_BillMonth")).Text;
                    string InvoiceNumber = ((Label)gr.FindControl("txt_invoiceNumber")).Text;
                    string Outstanding = ((Label)gr.FindControl("txt_Outstanding")).Text;
                    string MajoreCat = ((DropDownList)gr.FindControl("dd_MajoreCat")).SelectedValue;
                    string comments = ((TextBox)gr.FindControl("txt_comments")).Text;
                    string Visitdate = txt_date.Text;

                    string comp1 = dd_Complevel1.SelectedValue;
                    string comp2 = dd_Complevel2.SelectedValue;
                    string comp3 = dd_Complevel3.SelectedValue;


                    sql_ = sql_ + "SELECT \n"
                               + "  '" + zoneid + "', \n"
                               + "	'" + branch + "', \n"
                               + "  '" + Visitdate + "', \n"
                               + "	'" + clientid + "', \n"
                               + "  '" + AccountNo + "', \n"
                               + "	'" + comp3 + "', \n"
                               + "	'" + billMonth + "', \n"
                               + "	'" + InvoiceNumber + "', \n"
                               + "	'" + Outstanding + "', \n"
                               + "	'" + MajoreCat + "', \n"
                               + "	'" + comments + "', \n"
                               + "	'" + Session["U_ID"].ToString() + "', \n"
                               + "	getdate() \n"
                               + " UNion All \n";
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
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Invoice Number was not Selected')", true);
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
                //IsUnique = Int32.Parse(SParam.Value.ToString());
                // obj.XCode = obj.consignmentNo;
                sqlcon.Close();

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Records have been added')", true);

                //string script_ = @"window.open(""{0}"", ""{1}"", ""{2}"");";
                ////string script = String.Format(script_, "RunsheetInvoice.aspx?Xcode=" + temp_, "_blank", "");
                //string script = String.Format(script_, "Print_Tariffs.aspx?ClientID=" + creditclientid.Value + "&ServiceType=" + dd_serviceType.SelectedValue, "_blank", "");
                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Redirect", script, true);


                gv_tariff.DataSource = null;
                gv_tariff.DataBind();
                dd_Complevel3_SelectedIndexChanged(sender, e);

            }
            catch (Exception ex)
            {
                //break;
                err_msg.Text = ex.Message.ToString();// "Zone Must be selected";


            }
        }
        protected void btn_cancel_Click(object sender, EventArgs e)
        {

        }
        protected void year_chk_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_year_all.Checked)
            {
                dd_Year.Visible = false;
                dd_Month.Visible = false;
                chk_month_all.Checked = true;
                chk_month_all.Enabled = false;
            }
            else
            {
                dd_Year.Visible = true;
                dd_Month.Visible = true;
                chk_month_all.Checked = false;
                chk_month_all.Enabled = true;
            }
        }
        protected void month_chk_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_month_all.Checked)
            {
                dd_Month.Visible = false;
            }
            else
            {
                dd_Month.Visible = true;
            }
        }
    }
}