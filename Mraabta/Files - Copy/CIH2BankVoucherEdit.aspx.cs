using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using MRaabta.App_Code;
using System.Drawing;
using System.Security.Cryptography;
using System.Globalization;

namespace MRaabta.Files
{
    public partial class CIH2BankVoucherEdit : System.Web.UI.Page
    {
        CommonFunction func = new CommonFunction();
        Consignemnts con = new Consignemnts();
        Cl_Variables clvar = new Cl_Variables();
        Cl_Receipts rec = new Cl_Receipts();
        LoadingPrintReport enc_ = new LoadingPrintReport();
        bayer_Function bfunc = new bayer_Function();
        Variable var = new Variable();

        #region values initialization
        double remaining_val = 0;
        double remaining_pettyCash = 0;
        double diff = 0;
        double petty_amount = 0;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                string temp = HttpContext.Current.Session["BranchCode"].ToString();
            }
            catch (Exception ex)
            {

                Errorid.Text = "Session Expired. Please Re-Login";
                Errorid.ForeColor = Color.Red;
                return;
            }


            if (!IsPostBack)
            {
                Errorid.Text = "";
                Banks();
                //Get_Branches();
                Get_Zones();
                Get_Companies();
                if (!string.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    hf_id.Value = Request.QueryString["id"].ToString();
                    //hf_id.Value = Decrypt(HttpUtility.UrlDecode(Request.QueryString["id"]));
                    DataSet ds = Mainhead();
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        txt_amount.Text = ds.Tables[0].Rows[0]["Amount"].ToString();
                        hf_old_val.Value = ds.Tables[0].Rows[0]["Amount"].ToString();
                        txt_chequeDate.Text = ds.Tables[0].Rows[0]["cheque_date"].ToString();
                        if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DepositSlipNo"].ToString()))
                        {
                            txt_dslipNo.Text = ds.Tables[0].Rows[0]["DepositSlipNo"].ToString();
                            txt_dslipNo.Enabled = false;
                        }
                        else
                        {
                            txt_dslipNo.Enabled = true;
                        }
                        dd_company.SelectedValue = ds.Tables[0].Rows[0]["company"].ToString();
                        dd_zone.SelectedValue = ds.Tables[0].Rows[0]["ZONECODE"].ToString();
                        Get_Branches();
                        dd_branch.SelectedValue = ds.Tables[0].Rows[0]["BranchCode"].ToString();

                        dd_depositSlipBank.SelectedValue = ds.Tables[0].Rows[0]["DepositSlipBankID"].ToString();

                    }
                }
            }
        }
        public void Get_Companies()
        {
            DataSet ds = new DataSet();
            ds = ds_companies();
            if (ds.Tables[0].Rows.Count != 0)
            {
                dd_company.DataTextField = "sdesc_of";
                dd_company.DataValueField = "code_of";
                dd_company.DataSource = ds.Tables[0].DefaultView;
                dd_company.DataBind();
            }
        }
        public DataSet ds_companies()
        {
            DataSet Ds_1 = new DataSet();
            try
            {

                string query = "select * from COMPANY_OF order by code_of";
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(Ds_1);
                orcl.Close();
            }
            catch (Exception Err)
            { }
            finally
            { }

            return Ds_1;
        }

        #region ZONE AND BRANCHES
        public void Get_Zones()
        {

            DataSet ds = new DataSet();
            ds = ds_zones();
            if (ds.Tables[0].Rows.Count != 0)
            {
                dd_zone.DataTextField = "name";
                dd_zone.DataValueField = "code";
                dd_zone.DataSource = ds.Tables[0].DefaultView;
                dd_zone.DataBind();
            }
          //  Get_Branches();
        }
        public DataSet ds_zones()
        {
            DataSet Ds_1 = new DataSet();
            try
            {
                string zone = "";
                if (!string.IsNullOrEmpty(HttpContext.Current.Session["ZONECODE"].ToString()))
                {
                    zone = "'" + HttpContext.Current.Session["ZONECODE"].ToString().Replace(",", "','") + "'";
                }
                string sqlString = "SELECT Z.zoneCode CODE,Z.name FROM ZONES Z\n" +
                "WHERE Z.zoneCode in  (" + zone + ")";
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(Ds_1);
                orcl.Close();
            }
            catch (Exception Err)
            { }
            finally
            { }

            return Ds_1;
        }
        protected void dd_zone_Changed(object sender, EventArgs e)
        {
            Get_Branches();
        }
        public DataSet Get_Branches(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            string query;
            try
            {
                string extraCondition = "";
                if (HttpContext.Current.Session["BranchCode"].ToString().ToUpper() == "ALL")
                {
                    extraCondition = "";
                }
                else
                {
                    extraCondition = "AND BranchCode in (" + HttpContext.Current.Session["BranchCode"].ToString() + ")";
                }
                string sql = "SELECT NAME, \n"
               + "       branchCode \n"
               + "FROM   Branches \n"
               + "WHERE  STATUS = '1' \n"
               + "       AND zonecode = '" + dd_zone.SelectedValue.ToString() + "'  " + extraCondition + "\n"
               + "                             UNION ALL \n"
               + "SELECT NAME, \n"
               + "       branchCode \n"
               + "FROM   Branches \n"
               + "WHERE  branchCode = '331' \n"
               + "       AND zonecode = '" + dd_zone.SelectedValue.ToString() + "' \n"
               + "ORDER BY \n"
               + "       NAME";

                query = "select NAME, branchCode from Branches where status = '1' \n" +
                                " AND zonecode='" + dd_zone.SelectedValue.ToString() + "'  ORDER BY NAME";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sql, orcl);
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




        public void Get_Branches()
        {
            DataSet ds = new DataSet();
            ds = Get_Branches(var);
            dd_branch.Items.Clear();
            if (ds.Tables[0].Rows.Count != 0)
            {
                dd_branch.DataTextField = "Name";
                dd_branch.DataValueField = "branchCode";
                dd_branch.DataSource = ds.Tables[0].DefaultView;
                dd_branch.DataBind();

            }
        }
        #endregion


        public DataTable GetBanks()
        {

            string query = "select * from Banks_of";

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
        protected void Banks()
        {
            DataTable dt = GetBanks();
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    // DataTable mnpBanks = dt;//.Select("isMNPBank = TRUE").CopyToDataTable();
                    dd_depositSlipBank.DataSource = dt;
                    dd_depositSlipBank.DataTextField = "bank_name";
                    dd_depositSlipBank.DataValueField = "bank_code";
                    dd_depositSlipBank.DataBind();
                }
            }
        }

        public float Get_cih()
        {
            float amount = 0;
            DataSet ds = new DataSet();
            try
            {
                float remain_amount = 0;
                string remaininig_date = "";
                DataSet ds_remain = Get_Remaings();
                amount = float.Parse(ds_remain.Tables[0].Rows[0]["remaining_amount"].ToString());

                if (ds_remain.Tables[0].Rows.Count > 0)
                {
                    remaininig_date = DateTime.Parse(ds_remain.Tables[0].Rows[0]["chequedate"].ToString()).ToString("yyyy-MM-dd");
                }

                string sqlString = "SELECT isnull(SUM(pv.Amount),0) TotalAmount,'0' ChequeDate\n" +
                "FROM   CIH_balance pv\n" +
                "WHERE  pv.BranchCode = '" + dd_branch.SelectedValue.ToString() + "'\n" +
                " and pv.voucherdate between '" + remaininig_date + "' AND '" + txt_chequeDate.Text + "'  ";

                //Query To Be Run On First Time
                //string sqlString = "SELECT SUM(pv.Amount) TotalAmount, pv.BranchCode\n" +
                //"FROM   PaymentVouchers pv\n" +
                //"WHERE  pv.BranchCode = '" + dd_branch.SelectedValue.ToString() + "'\n" +
                //"       AND (pv.PaymentSourceId IS NULL OR pv.PaymentSourceId = '1')\n" +
                //"          AND pv.CashPaymentSource != '4' AND CAST(pv.VoucherDate AS DATE) <= '2018-03-31'\n" +
                //"GROUP BY pv.BranchCode";


                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                orcl.Close();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["TotalAmount"].ToString()))
                        amount = float.Parse(ds.Tables[0].Rows[0]["TotalAmount"].ToString()) + amount;
                }

            }
            catch (Exception Err)
            {
            }
            finally
            { }
            return amount;
        }
        public int Insert_Entry()
        {
            try
            {

                string sqlString = "update Deposit_to_bank \n" +
                " set DepositSlipNo='" + txt_dslipNo.Text + "',Modifiedby = '" + HttpContext.Current.Session["U_ID"].ToString() + "',\n" +
                 "  ModifiedOn = GETDATE(),DepositSlipBankID='" + dd_depositSlipBank.SelectedValue.ToString() + "',\n" +
                  "  amount = '" + float.Parse(txt_amount.Text) + "' where id='" + hf_id.Value.ToString() + "'\n";


                int mont = DateTime.ParseExact(txt_chequeDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture).Month;  
                int year = DateTime.ParseExact(txt_chequeDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture).Year;
                double diff = double.Parse(hf_old_val.Value.ToString()) - double.Parse(txt_amount.Text);
                string sqlString1 = "update CIH_remainings set  value=value + '" + diff + "'\n" +
                " where\n" +
                " [month]='" + mont + "' and\n" +
                " [year]='" + year + "' and\n" +
                " [COMPANY]='" + dd_company.SelectedValue.ToString() + "' and\n" +
                " [branch]='" + dd_branch.SelectedValue.ToString() + "'";

                SqlConnection orcl = new SqlConnection(clvar.Strcon2());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                orcd.ExecuteNonQuery();

                //update query
                SqlCommand orcd1 = new SqlCommand(sqlString1, orcl);
                orcd1.CommandType = CommandType.Text;
                SqlDataAdapter oda1 = new SqlDataAdapter(orcd1);
                orcd1.ExecuteNonQuery();

                orcl.Close();
                Errorid.Text = "";
                return 1;
                // }
            }
            catch (Exception Err)
            {
                Errorid.Text = "The entry is not saved due to an error!!";
                return 0;
            }
            finally
            {
            }
        }

        protected void btn_save_Click(object sender, EventArgs e)
        {
            lbl_message.Text = "";
            lbl_err2.Text = "";

            if (double.Parse(hf_old_val.Value.ToString()) > double.Parse(txt_amount.Text))
            {
                try
                {
                    int i = Insert_Entry();

                    if (i == 1)
                    {
                        Errorid.Text += UpdateCIH_remaining();
                        Errorid.Text += UpdatePC_remaining();
                        lbl_message.Text = "The Transaction Is Saved Successfully";
                    }
                    else
                    {
                        if (Errorid.Text == "")
                        {
                            Errorid.Text = "The Entry Is Not Saved Due To Error!!";
                        }
                        Errorid.ForeColor = System.Drawing.Color.Red;
                    }
                }
                catch (Exception ex)
                {

                    Errorid.Text = "The Entry Is Not Saved Due To Error!!";
                    Errorid.ForeColor = System.Drawing.Color.Red;
                }
            }
            else
            {
                Errorid.Text = "The New Entry Cannot Exceed The previous Amount!!";
                return;
            }
        }
        protected void btn_reset_Click(object sender, EventArgs e)
        {
            ResetAll();
        }
        protected void ResetAll()
        {
            txt_amount.Text = "";
            txt_dslipNo.Text = "";



        }

        protected void txt_cnNo_TextChanged(object sender, EventArgs e)
        {
            //   clvar.consignmentNo = txt_cnNo.Text;
            DataTable dt = GetConsignmentDetail(clvar);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["PRESENT"].ToString().Trim(' ') != "")
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Voucher No.# " + dt.Rows[0]["VoucherID"].ToString() + " Already Created for CN Number " + clvar.consignmentNo + "')", true);

                        txt_amount.Text = "";
                        return;
                    }

                    //txt_amount.Text = dt.Rows[0]["TotalAmount"].ToString();

                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('CN Not Found')", true);
                    txt_amount.Text = "";
                    return;
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('CN Not Found')", true);
                txt_amount.Text = "";
                return;
            }
        }
        public void AlertMessage(string message, string color)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AlertMessage", "alert('" + message + "')", true);
            Errorid.Text = message;
            Errorid.ForeColor = System.Drawing.Color.FromName(color);
        }

        public DataSet Get_Remaings()
        {
            float amount = 0;
            DataSet ds = new DataSet();
            try
            {


                string sqlString = " select pd.remaining_amount,MAX(pd.ChequeDate) chequedate from deposit_to_bank pd\n" +
                "where  pd.BranchCode = '" + dd_branch.SelectedValue.ToString() + "'\n" +
                " and  pd.ChequeDate <= '" +  DateTime.ParseExact(txt_chequeDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd") + "'   \n" +
                "  group by pd.remaining_amount";



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
        public DataTable CIH_remainingCash()
        {
            DataTable dt = new DataTable();
            try
            {
                int mont = DateTime.ParseExact(txt_chequeDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture).Month;
                int year = DateTime.ParseExact(txt_chequeDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture).Year;
                string sqlString = "select c.[VALUE] remaining_cash,petty_cash from CIH_remainings c\n" +
                "where\n" +
                "c.month='" + mont + "'\n" +
                "and c.year='" + year + "'\n" +
                "and c.company='" + dd_company.SelectedValue.ToString() + "'\n" +
                "and c.branch='" + dd_branch.SelectedValue.ToString() + "'";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dt);
                orcl.Close();
            }
            catch (Exception Err)
            { }
            finally
            { }

            return dt;
        }
        public int check_remainings()
        {

            //:::::::::::Amount Checking::::::::::::::::://
            double remain_amount = 0;
            DataTable dt = new DataTable();
            dt = CIH_remainingCash();
            if (dt.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(dt.Rows[0]["remaining_cash"].ToString()))
                {
                    remaining_val = double.Parse(dt.Rows[0]["remaining_cash"].ToString());

                }
                else
                {
                    remaining_val = 0;
                }
                if (!string.IsNullOrEmpty(dt.Rows[0]["petty_cash"].ToString()))
                {
                    remaining_pettyCash = double.Parse(dt.Rows[0]["petty_cash"].ToString());

                }
                else
                {
                    remaining_pettyCash = 0;

                }
            }
            // hf_remaining_pettyCash.Value = remaining_pettyCash.ToString();
            // hf_remaining_val.Value = remaining_val.ToString();

            diff = remaining_val - double.Parse(txt_amount.Text);
            petty_amount = double.Parse(txt_amount.Text) + remaining_pettyCash;

            hf_diff.Value = diff.ToString();
            ///        hf_petty_amount.Value = petty_amount.ToString();

            if (remaining_val <= double.Parse(txt_amount.Text))
            {
                lbl_err2.Text = "The amount you entered exceeds the available Cash In Hand!!";
                return 0;
            }
            return 1;
        }

        public DataTable GetConsignmentDetail(Cl_Variables clvar)
        {
            DataTable dt = new DataTable();
            string sqlString = "select c.consignmentNumber, cc.accountNo, c.consignerAccountNo, cc.name, c.creditClientId, cc.id\n" +
            "  from consignment c\n" +
            " inner join CreditClients cc\n" +
            "    on cc.accountNo = c.consignerAccountNo\n" +
            "   and cc.branchCode = c.branchCode\n" +
            " where \n" +
            "   c.consignmentNumber = '" + clvar.consignmentNo + "'";


            sqlString = "select c.consignmentNumber,\n" +
            "       cc.accountNo,\n" +
            "       c.consignerAccountNo,\n" +
            "       cc.name,\n" +
            "       c.creditClientId,\n" +
            "       cc.id,\n" +
            "       p.ConsignmentNo Present, p.id VoucherID, c.totalAmount\n" +
            "  from consignment c\n" +
            " inner join CreditClients cc\n" +
            "    on cc.id = c.creditclientID\n" +
            "    --on cc.accountNo = c.consignerAccountNo\n" +
            "   --and cc.branchCode = c.branchCode\n" +
            "  left outer join PaymentVouchers p\n" +
            "    on p.ConsignmentNo = c.consignmentNumber\n" +
            "   and p.CreditClientId = c.creditClientId\n" +
            " where c.consignmentNumber = '" + clvar.consignmentNo + "'";


            sqlString = "\n" +
           "SELECT c.consignmentNumber,\n" +
           "       cc.accountNo,\n" +
           "       c.consignerAccountNo,\n" +
           "       cc.name,\n" +
           "       c.creditClientId,\n" +
           "       cc.id,\n" +
           "       p.ConsignmentNo Present,\n" +
           "       p.id VoucherID,\n" +
           "       c.totalAmount,\n" +
           "       CASE\n" +
           "         WHEN COUNT(c2.Accountno) > 0 THEN\n" +
           "          (SELECT SUM(distinct cdn.codAmount)\n" +
           "             FROM CODConsignmentDetail_New cdn\n" +
           "            WHERE cdn.consignmentNumber = c.consignmentNumber)\n" +
           "         ELSE\n" +
           "          (SELECT SUM(cd.codAmount)\n" +
           "             FROM CODConsignmentDetail cd\n" +
           "            WHERE cd.consignmentNumber = c.consignmentNumber)\n" +
           "       END CODAMOUNT\n" +
           "  FROM consignment c\n" +
           " INNER JOIN CreditClients cc\n" +
           "    ON cc.id = c.creditclientID\n" +
           "--on cc.accountNo = c.consignerAccountNo\n" +
           "--and cc.branchCode = c.branchCode\n" +
           "\n" +
           "  LEFT OUTER JOIN PaymentVouchers p\n" +
           "    ON p.ConsignmentNo = c.consignmentNumber\n" +
           "   AND p.CreditClientId = c.creditClientId\n" +
           "  LEFT OUTER JOIN CODUsers c2\n" +
           "    ON c2.CreditClientID = c.creditClientId\n" +
           "   AND c2.Accountno = c.consignerAccountNo\n" +
           "   AND c2.IsCOD = '1'\n" +
           " WHERE c.consignmentnumber = '" + clvar.consignmentNo + "'\n" +
           " GROUP BY c.consignmentNumber,\n" +
           "          cc.accountNo,\n" +
           "          c.consignerAccountNo,\n" +
           "          cc.name,\n" +
           "          c.creditClientId,\n" +
           "          cc.id,\n" +
           "          p.ConsignmentNo,\n" +
           "          p.id,\n" +
           "          c.totalAmount";

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

        public DataSet Mainhead()
        {
            string id = "";
            DataSet Ds_1 = new DataSet();

            string branch = "";
            string ec = "";


            string sqlString = "SELECT d.Id\n" +
            "      ,d.ChequeNo\n" +
            "      ,CONVERT(varchar, d.ChequeDate, 103) cheque_date\n" +
            "      ,d.Amount\n" +
            "      ,d.remaining_amount\n" +
            "      ,d.BranchCode\n" +
            "      ,CONVERT(varchar, d.CreatedOn, 103) created_on\n" +
            "      ,d.CreatedBy\n" +
            "      ,d.ReceiptNo\n" +
            "      ,d.DepositSlipNo\n" +
            "      ,d.DepositSlipBankID\n" +
            "      ,d.company\n" +
            "      ,d.zonecode\n" +
            "    ,c.sdesc_OF company_name\n" +
            "    ,b.[name] branch_name\n" +
            "    ,z.Name cuser_name\n" +
            "    ,ba.bank_name \n" +
            "  FROM Deposit_to_bank d\n" +
            "  inner join Branches b\n" +
            "  on b.branchCode=d.BranchCode\n" +
            "  inner join COMPANY_OF c\n" +
            "  on c.code_OF=d.company\n" +
            "  inner join ZNI_USER1 z\n" +
            "  on z.U_ID=d.CreatedBy\n" +
            "  left outer join banks_of ba\n" +
            "  on ba.bank_code=d.DepositSlipBankID\n" +
            "  where d.id='" + hf_id.Value.ToString() + "'\n";
            sqlString += " order by d.CreatedOn desc,d.Id desc ";

            SqlConnection orcl = new SqlConnection(clvar.Strcon());
            orcl.Open();
            SqlCommand orcd = new SqlCommand(sqlString, orcl);
            orcd.CommandType = CommandType.Text;
            SqlDataAdapter oda = new SqlDataAdapter(orcd);
            oda.Fill(Ds_1);
            orcl.Close();

            return Ds_1;
        }
        #region update cih remainings
        public string UpdatePC_remaining()
        {
            SqlConnection sqlcon = new SqlConnection(clvar.Strcon2());
            string result = "";
            DataSet ds = new DataSet();
            try
            {
                DateTime dt = DateTime.ParseExact(txt_chequeDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateTime first_day = new DateTime(dt.Year, dt.Month, 1);
                DateTime last_day = first_day.AddMonths(1).AddSeconds(-1);
                sqlcon.Open();
                SqlCommand sqlcmd = new SqlCommand("UpdatePettyCashBalance", sqlcon);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                SqlParameter SParam = new SqlParameter("Result", SqlDbType.VarChar);
                SParam.Direction = ParameterDirection.Output;
                SParam.Size = 500;
                sqlcmd.Parameters.AddWithValue("@BranchCode", dd_branch.SelectedValue.ToString());
                sqlcmd.Parameters.AddWithValue("@StartDate", first_day.ToString("yyyy-MM-dd"));
                sqlcmd.Parameters.AddWithValue("@EndDate", last_day.ToString("yyyy-MM-dd"));
                sqlcmd.Parameters.AddWithValue("@Month", dt.Month.ToString());
                sqlcmd.Parameters.AddWithValue("@Year", dt.Year.ToString());
                sqlcmd.Parameters.Add(SParam);
                sqlcmd.ExecuteNonQuery();
                result = SParam.Value.ToString();
                sqlcon.Close();
            }
            catch (Exception Err)
            {
                result = "Error in CIH Update!!";
            }
            finally
            { }
            return result;
        }
        public string UpdateCIH_remaining()
        {
            SqlConnection sqlcon = new SqlConnection(clvar.Strcon2());
            string result = "";
            DataSet ds = new DataSet();
            try
            {
                DateTime dt = DateTime.ParseExact(txt_chequeDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateTime first_day = new DateTime(dt.Year, dt.Month, 1);
                DateTime last_day = first_day.AddMonths(1).AddSeconds(-1);
                sqlcon.Open();
                SqlCommand sqlcmd = new SqlCommand("UpdateCashInHandBalance", sqlcon);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                SqlParameter SParam = new SqlParameter("Result", SqlDbType.VarChar);
                SParam.Direction = ParameterDirection.Output;
                SParam.Size = 500;
                sqlcmd.Parameters.AddWithValue("@BranchCode", dd_branch.SelectedValue.ToString());
                sqlcmd.Parameters.AddWithValue("@StartDate", first_day.ToString("yyyy-MM-dd"));
                sqlcmd.Parameters.AddWithValue("@EndDate", last_day.ToString("yyyy-MM-dd"));
                sqlcmd.Parameters.AddWithValue("@Month", dt.Month.ToString());
                //  sqlcmd.Parameters.AddWithValue("@Year", dd_year.SelectedItem.ToString());
                sqlcmd.Parameters.Add(SParam);
                sqlcmd.ExecuteNonQuery();
                result = SParam.Value.ToString();
                sqlcon.Close();
            }
            catch (Exception Err)
            {
                result = "Error in CIH Update!!";
            }
            finally
            { }
            return result;
        }
        #endregion
        #region Encryption & decryption
        private string Encrypt(string clearText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = System.Text.Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }
        private string Decrypt(string cipherText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = System.Text.Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
        #endregion
    }
}