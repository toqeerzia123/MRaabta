using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Text;
using System.Net.Mail;
using System.Collections.Generic;
using MRaabta.App_Code;

namespace MRaabta.Files
{
    public partial class RetailCODCustomer : System.Web.UI.Page
    {
        Cl_Variables clvar = new Cl_Variables();
        CL_Customer clvar_ = new CL_Customer();

        string error, getPassword;
        public string BeneficiaryBankAccNo = "";
        public string defaultAccountNo = "0";

        protected void Page_Load(object sender, EventArgs e)
        {
            Get_Banks();
        }
        public void Get_Banks()
        {
            DataTable dt = Banks();
            if (dt.Rows.Count > 0)
            {
                dd_bank.DataTextField = "name";
                dd_bank.DataValueField = "id";
                dd_bank.DataSource = dt;
                dd_bank.DataBind();
            }
        }
        protected void txt_cnic_TextChanged(object sender, EventArgs e)
        {
            string contactNo = txt_cnic.Text;
            double sAsD = double.Parse(contactNo);
            txt_cnic.Text = string.Format("{0:#-#######-#}", sAsD).ToString();

            DataTable checkCNIC = Get_checkCNIC(txt_cnic.Text);

            if (checkCNIC.Rows.Count > 0)
            {
                txt_cnic.Text = "";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('CNIC already Entered.')", true);
                return;
            }
        }
        protected void txt_mobile_TextChanged(object sender, EventArgs e)
        {
            string contactNo = txt_mobile.Text;
            double sAsD = double.Parse(contactNo);
            txt_mobile.Text = Convert.ToInt64(sAsD).ToString("0###-#######");

            DataTable checkMobileNo = Get_checkMobileNo(txt_mobile.Text);

            if (checkMobileNo.Rows.Count > 0)
            {
                txt_mobile.Text = "";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Mobile Number already Entered.')", true);
                return;
            }
        }
        protected void Btn_Save(object sender, EventArgs e)
        {
            #region Validation

            if (txt_customername.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter Customer Name')", true);
                return;
            }
            if (txt_mobile.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter Mobile Number')", true);
                return;
            }
            if (txt_email.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter Email Address')", true);
                return;
            }
            if (txt_cnic.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter CNIC Number')", true);
                return;
            }
            if (txt_address.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter Address')", true);
                return;
            }
            if (dd_bank.SelectedValue == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Select Bank')", true);
                return;
            }
            if (txt_bankbranchname.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter Bank Branch Name')", true);
                return;
            }
            if (txt_accounttitle.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter Bank Account Title')", true);
                return;
            }

            if (txt_ibft.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter Bank Account IBFT Number')", true);
                return;
            }
            //if (txt_accountno.Text == "")
            //{
            //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter Bank Account Number')", true);
            //    return;
            //}

            #endregion

            clvar.customerName = txt_customername.Text;
            clvar.ConsignerPhone = txt_mobile.Text;
            clvar.consignerEmail = txt_email.Text;
            clvar.ConsignerCNIC = txt_cnic.Text;
            clvar.ConsignerAddress = txt_address.Text;
            clvar.Bank = dd_bank.SelectedValue;
            clvar.BranchName = txt_bankbranchname.Text;
            clvar.CityCode = txt_bankbranchcode.Text;
            clvar.cardRefNo = txt_ibft.Text;
            clvar.Company = txt_accounttitle.Text;
            BeneficiaryBankAccNo = txt_ibft.Text;
            //clvar_.NTNNo = txt_ntn.Text;

            DateTime start = DateTime.Now;
            DateTime end = start.AddYears(1);

            //clvar_.RegDate = start.ToString("dd/MM/yyyy");
            clvar_.RegEndDate = end.ToString("yyyy-MM-dd");

            string str = Session["U_NAME"].ToString();
            string[] array = str.Split('@');
            clvar.createdBy = array[0];

            DataTable accountNo = GetAccountNumber(clvar);
            clvar.Consigner = txt_customername.Text;
            if (accountNo != null)
            {
                if (accountNo.Rows.Count > 0)
                {
                    clvar.AccountNo = Session["BRANCHCODE"].ToString() + "CC" + accountNo.Rows[0][0].ToString();
                }
                else
                {
                    Alert("Account Could not be Generated. Contact IT Support.");
                    return;
                }
            }

            error = Add_Customers(clvar);
            if (error == "")
            {
                getPassword = CreatePassword(11);
                EmailShoot(getPassword);
                Post_BrandedSMS(clvar.ConsignerPhone, clvar.AccountNo, clvar.customerName.ToUpper(), Session["BRANCHCODE"].ToString());
                ResetAll();
            }
        }
        public void Post_BrandedSMS(string mobile, string resp, string Consigner, string destination)
        {
            try
            {
                if (mobile != string.Empty)
                {
                    string smsContent = "Dear " + Consigner + ",<br>Your Account has been created successfully. Below are the Credentials<br>Username: " + clvar.AccountNo + "<br>Password: " + getPassword + "<br>URL: https://mnpcourier.com/mycod <br>" + " You can visit https://mnpcourier.com/mycod or call us on 111-202-202 to track delivery status. Thank you for choosing M&P Courier.";

                    string query2 = "insert into MNP_SMSSTATUS (ConsignmentNumber, Recepient, MessageContent, Status, Createdon, CreatedBy, RunsheetNumber,smsformtype) \n" +
                             "values ('', '" + mobile + "', '" + smsContent + "', '0', GETDATE(), '" + HttpContext.Current.Session["U_ID"].ToString() + "', 'N/A','3')";

                    int count = 0;
                    string error = "";
                    SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
                    try
                    {
                        sqlcon.Open();
                        SqlCommand sqlcmd = new SqlCommand(query2, sqlcon);
                        sqlcmd.CommandType = CommandType.Text;

                        count = sqlcmd.ExecuteNonQuery();
                        sqlcon.Close();
                    }
                    catch (Exception ex)
                    {
                        error = ex.Message;
                    }
                }
            }
            catch (Exception Err)
            {
            }
        }
        public DataTable Banks()
        {
            String sqlString = "SELECT * FROM Banks b WHERE b.isMNPBank = '0' ORDER BY b.Name ASC";

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
        public DataTable Get_checkCNIC(string cnic)
        {
            String sqlString = "SELECT DISTINCT b.CNIC FROM MNP_RETAIL_COD_CUSTOMERS b WHERE b.CNIC = '" + cnic + "'";

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
        public DataTable Get_checkMobileNo(string mobileno)
        {
            String sqlString = "SELECT DISTINCT b.MobileNumber FROM MNP_RETAIL_COD_CUSTOMERS b WHERE b.MobileNumber = '" + mobileno + "'";

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
        public DataTable GetAccountNumber(Cl_Variables clvar)
        {
            string arr = "CC";

            int br_length = Session["BRANCHCODE"].ToString().Length;
            string query = "selecT ISNULL(MAX(CAST(SUBSTRING(cc.accountNo," + (br_length + 3) + ",50) as bigint)),0) + 1\n" +
                    " from CreditClients cc where LEFT(cc.accountNo, " + (br_length + 2) + ") = '" + Session["BRANCHCODE"].ToString() + arr + "' and cc.branchCode = '" + Session["BRANCHCODE"].ToString() + "'";

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
            return dt;
        }
        public string Add_Customers(Cl_Variables clvar)
        {
            string errorMessage = "";
            SqlConnection sqlConnection = new SqlConnection(clvar.Strcon());
            SqlTransaction transaction;

            sqlConnection.Open();
            transaction = sqlConnection.BeginTransaction();
            try
            {
                SqlCommand sql1 = new SqlCommand("INSERT INTO MNP_RETAIL_COD_CUSTOMERS (CUSTOMERNAME, MOBILENUMBER, EMAIL, CNIC, ADDRESS, BANKNAME, BankBranchName, BankBranchCode, " +
                                                 "IBFT, ACCOUNTTITLE, ACCOUNTNUMBER, BRANCH, STATUS, CREATEDBY, CREATEDON) \n" +
                          "VALUES ( \n" +
                          " '" + clvar.customerName + "', \n" +
                          " '" + clvar.ConsignerPhone + "', \n" +
                          " '" + clvar.consignerEmail + "', \n" +
                          " '" + clvar.ConsignerCNIC + "', \n" +
                          " '" + clvar.ConsignerAddress + "', \n" +
                          " '" + clvar.Bank + "', \n" +
                          " '" + clvar.BranchName + "', \n" +
                          " '" + clvar.CityCode + "', \n" +
                          " '" + clvar.cardRefNo + "', \n" +
                          " '" + clvar.Company + "', \n" +
                          " '" + clvar.AccountNo + "', \n" +
                          " '" + Session["BRANCHCODE"].ToString() + "', \n" +
                          " '1', \n" +
                          " '" + Session["U_ID"].ToString() + "', \n" +
                          " GETDATE() \n" +
                          " ) ", sqlConnection, transaction);
                sql1.ExecuteNonQuery();
                
                // For CreditClients
                SqlCommand sql2 = new SqlCommand("insert into CreditClients\n" +
                                "  (name,\n" +
                                "   contactPerson,\n" +
                                "   phoneNo,\n" +
                                "   faxNo,\n" +
                                "   email,\n" +
                                "   address,\n" +
                                "   centralizedClient,\n" +
                                "   regDate,\n" +
                                "   regEndDate,\n" +
                                "   pickUpInstruction,\n" +
                                "   domesticAMonTo,\n" +
                                "   internationalAMonTo,\n" +
                                "   domesticPackets,\n" +
                                "   internationalPackets,\n" +
                                "   domesticAmount,\n" +
                                "   internationalAmount,\n" +
                                "   status,\n" +
                                "   printingStatus,\n" +
                                "   billingMode,\n" +
                                "   discountOnDomestic,\n" +
                                "   discountOnSample,\n" +
                                "   discountOnDocument,\n" +
                                "   prepareBillType,\n" +
                                "   creditLimit,\n" +
                                "   salesTaxNo,\n" +
                                "   memo,\n" +
                                "   billTaxType,\n" +
                                "   catId,\n" +
                                "   clientGrpId,\n" +
                                "   recoveryExpCenId,\n" +
                                "   salesRouteId,\n" +
                                "   recoveryOfficer,\n" +
                                "   salesExecutive,\n" +
                                "   redeemWindow,\n" +
                                "   overdueCalBase,\n" +
                                "   overdueValue,\n" +
                                "   createdBy,\n" +
                                "   createdOn,\n" +
                                "   SectorId,\n" +
                                "   IndustryId,\n" +
                                "   accountNo,\n" +
                                "   creditClientType,\n" +
                                "   zoneCode,\n" +
                                "   branchCode,\n" +
                                "   expressCenterCode,\n" +
                                "   ntnNo,\n" +
                                "   IsCOD,\n" +
                                "   isActive,\n" +
                                "   IsSpecial,\n" +
                                "   isFranchisee,\n" +
                                "   recoveryOfficerName,\n" +
                                "   recoveryOfficer_id,\n" +
                                "   isNationWide,\n" +
                                "   isParent,\n" +
                                "   MailingAddress,\n" +
                                "   OriginEC,\n" +
                                "   StatusCode,\n" +
                                "   CODType,\n" +
                                "   isMinBilling,\n" +
                                "   MonthlyFixCharges,\n" +
                                "   IsSmsServiceActive,\n" +
                                "   RNR_Weight,\n" +
                                "   HSCNCharges, BeneficiaryName, BeneficiaryBankAccNo, BeneficiaryBankCode, ispiece_only, IsDestination, ContactPersonDesignation, ParentID, Faftype\n" +
                                "  )\n output inserted.id \n" +
                                "Values\n" +
                                "  (\n" +
                                "'" + clvar.customerName + "',\n" +
                                "'" + clvar.customerName + "',\n" +
                                "'" + clvar.ConsignerPhone + "',\n" +
                                "'0',\n" +
                                "'" + clvar.consignerEmail + "',\n" +
                                "'" + clvar.ConsignerAddress + "',\n" +
                                "'0',\n" +
                                "'" + DateTime.Now.ToString("yyyy-MM-dd") + "',\n" +
                                "'" + clvar_.RegEndDate + "',\n" +
                                "'0',\n" +
                                "'0',\n" +
                                "'0',\n" +
                                "'0',\n" +
                                "'0',\n" +
                                "'0',\n" +
                                "'0',\n" +
                                "'1',\n" +
                                "'0',\n" +
                                "'0',\n" +
                                "'0',\n" +
                                "'0',\n" +
                                "'0',\n" +
                                "'1',\n" +
                                "'0',\n" +
                                "'0',\n" +
                                "'0',\n" +
                                "'0',\n" +
                                "'0',\n" +
                                "'0',\n" +
                                "'" + HttpContext.Current.Session["ExpressCenter"].ToString() + "',\n" +
                                "NULL,\n" +
                                "NULL,\n" +
                                "NULL,\n" +
                                "'0',\n" +
                                "'0',\n" +
                                "'0',\n" +
                                "'" + HttpContext.Current.Session["U_ID"].ToString() + "',\n" +
                                " GETDATE(),\n" +
                                "'1',\n" +
                                "'25',\n" +
                                "'" + clvar.AccountNo + "',\n" +
                                "'0',\n" +
                                "'" + HttpContext.Current.Session["ZONECODE"].ToString() + "',\n" +
                                "'" + HttpContext.Current.Session["BRANCHCODE"].ToString() + "',\n" +
                                "'" + HttpContext.Current.Session["ExpressCenter"].ToString() + "',\n" +
                                //"'" + clvar_.NTNNo + "',\n" +
                                "'" + clvar.ConsignerCNIC + "',\n" +
                                "'1',\n" +
                                "'1',\n" +
                                "'0',\n" +
                                "'0',\n" +
                                "'0',\n" +
                                "'0',\n" +
                                "'0',\n" +
                                "'0',\n" +
                                "'" + clvar.ConsignerAddress + "',\n" +
                                "'" + HttpContext.Current.Session["ExpressCenter"].ToString() + "',\n" +
                                "'AC',\n" +
                                "'3',\n" + // RETAIL COD (COD TYPE)
                                "'0',\n" +
                                "'0',\n" +
                                "'0',\n" +
                                "'0',\n" +
                                "'0',\n" +
                                "'" + clvar.Company + "', \n" +
                                "'" + BeneficiaryBankAccNo + "', \n" +
                                "'" + clvar.Bank + "',\n" +
                                "'0',\n" +
                                "'0',\n" +
                                "'0',\n" +
                                "'0',\n" +
                                "'0'\n" +
                                "   ) ", sqlConnection, transaction);
                object clientID = sql2.ExecuteScalar();
                Int64 id = Int64.Parse(clientID.ToString());

                // For ClientStaff
                SqlCommand sql3 = new SqlCommand("insert into ClientStaff (ClientId, UserName, StaffTypeId) values (\n" +
                              " '" + id.ToString() + "', \n" +
                              " '" + clvar.createdBy + "', \n" +
                              " '214' \n" +
                              ") ", sqlConnection, transaction);
                sql3.ExecuteNonQuery();


                // For MNP_CUSTOMER_SERVICEMAP
                SqlCommand sql4 = new SqlCommand("INSERT INTO MNP_CUSTOMER_SERVICEMAP \n" +
                               "  SELECT '" + id.ToString() + "', st.name, '1', GETDATE(),'" + HttpContext.Current.Session["U_ID"].ToString() + "',NULL,NULL \n" +
                               "  FROM ServiceTypes_New st \n" +
                               "  WHERE st.IsIntl = '0' \n" +
                               "  AND st.[status] = '1' \n" +
                               "  And st.name not in ('Expressions', 'Road n Rail') \n" +
                               "  AND st.Products = 'Domestic' \n" +
                               "  GROUP BY st.name \n" +
                               "  ORDER BY st.name ", sqlConnection, transaction);
                sql4.ExecuteNonQuery();
                /*
                // For Tariff
                SqlCommand sql5 = new SqlCommand("INSERT INTO tempClientTariff \n" +
                                                 "select \n" +
                                                 "'" + id.ToString() + "' Client_Id, t.TariffCode, t.ServiceID, '" + HttpContext.Current.Session["BRANCHCODE"].ToString() + "' BranchCode, \n" +
                                                 " t.FromZoneCode, t.ToZoneCode, t.FromWeight, t.ToWeight, t.Price, t.additionalFactor, \n" +
                                                 " t.addtionalFactorSZ, t.addtionalFactorDZ, t.chkDefaultTariff, t.chkDeleted, t.isIntlTariff, t.currencyCodeId, getdate(), \n" +
                                                 " null, '23', null" +
                                                 " From tempClientTariff t \n" +
                                                 " inner join Zones z  on z.zoneCode = t.ToZoneCode \n" +
                                                 " inner join Creditclients c on c.id = t.client_id \n" +
                                                 " INNER JOIN ServiceTypes_New stn ON t.ServiceID = stn.serviceTypeName \n" +
                                                 " where c.accountno = '" + defaultAccountNo + "' \n" +
                                                 " AND stn.Products = 'Domestic' \n" +
                                                 " and isintltariff = '0' \n" +
                                                 " and chkDefaultTariff = 1 \n" +
                                                 " and chkdeleted = '0' \n" +
                                                 " and c.branchCode='" + HttpContext.Current.Session["BRANCHCODE"].ToString() + "' ", sqlConnection, transaction);
                sql5.ExecuteNonQuery();
                */
                // For COD Account
                string customerName = clvar.customerName;
                string[] CustomerUserId = customerName.Split(' ');
                string CNIssued = "5" + id.ToString() + "10000001";
                string CNSequenceEnd = "5" + id.ToString() + "99999999";

                SqlCommand sql6 = new SqlCommand("INSERT INTO MNP_Retail_COD_Info (CreditClientId, AccountNo, UserId, SequenceStart,SequenceEnd, LastCNIssued, LastCNIssuedOn, CloudConverted, CreatedBy, CreatedOn) " +
                                                 "Values ( \n" +
                                                  " '" + id.ToString() + "', \n" +
                                                  " '" + clvar.AccountNo + "', \n" +
                                                  " '" + CustomerUserId[0] + "_" + clvar.AccountNo + "',\n" +
                                                  " '" + CNIssued + "', \n" +
                                                  " '" + CNSequenceEnd + "', \n" +
                                                  " '" + CNIssued + "', \n" +
                                                 " GETDATE(), \n" +
                                                  " '0', \n" +
                                                  " '" + HttpContext.Current.Session["U_ID"].ToString() + "',\n" +
                                                  " GETDATE() \n" +
                                                 ")", sqlConnection, transaction);
                sql6.ExecuteNonQuery();

                // For Prefix
                SqlCommand sql7 = new SqlCommand("INSERT INTO MnP_ConsignmentLengths (Prefix, Product, Length, STATUS, Created_on, Created_by) values ( \n" +
                                                 " '5"+ id.ToString() + "', \n" +
                                                 " 'COD', \n"+
                                                 " '15', \n"+
                                                 " '1', \n"+
                                                 " GETDATE(), \n" +
                                                 " '" + HttpContext.Current.Session["U_ID"].ToString() + "'\n" +
                                                 ")", sqlConnection, transaction);
                sql7.ExecuteNonQuery();

                transaction.Commit();
                ErrorLbl.Text = "";
 
                string locationCRFSheet = Request.ApplicationPath + "/RetailCOD/RetailCODCRF?Account=" + clvar.AccountNo;
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "alert('Account Created');var test=window.open('" + locationCRFSheet + "','_blank');", true);
            }
            catch (Exception Error)
            {
                string errorMsg = "Error occured, " + Error.Message.ToString();
                transaction.Rollback();
                ErrorLbl.Text = "Error occured, " + Error.Message.ToString();
            }
            finally
            {
                sqlConnection.Close();
            }

            return errorMessage;
        }
        public string CreatePassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890#!";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }
        private void EmailShoot(string getPassword)
        {
            MailMessage message = new MailMessage();

            message.To.Add(clvar.consignerEmail);
            //message.CC.Add("");
            message.From = new MailAddress("no-reply@mulphilog.com");
            message.Subject = "Shipper Name: " + clvar.Consigner + " & Account No. " + clvar.AccountNo;
            message.IsBodyHtml = true;

            message.Body = "Dear " + clvar.Consigner + ", <br>" +
                           "<br> We are pleased to inform you that you have successfully registered your profile with us as Retail COD Customer. Please note your following credentials for COD portal" +
                           "<br>URL: https://mnpcourier.com/mycod/Login.aspx <br>" + 
                           "<br>Username: " + clvar.AccountNo + "" +
                           "<br>Password: 12345<br>" + //" + getPassword + " < br > " +
                           "<br><br> <strong><i>Reset your password after first login. Your password must have 08 characters(alpha numeric including one upper case letter & special character e.g.Abcd@1234).</i></strong>" +
                           "<br><br> <strong>Queries:</strong>" +
                           "<br>For your queries you can call us on our help line 021 - 111 - 202 - 202 or you can write us on contact @mulphilog.com" +
                           "<br><br> <strong>Payments:</strong>" +
                           "<br>For all your queries regarding payments, you can contact your Account Manager, or you may contact the following Email ID mentioning your account number: Affaq Qamar: affaq.qamar @mulphilog.com" +
                           "<br>In -case of any query feel free to write us. ";

            SmtpClient mail = new SmtpClient();
            mail.Port = int.Parse("587");
            mail.Host = "smtp.office365.com";
            mail.EnableSsl = true;
            mail.UseDefaultCredentials = true;
            mail.Credentials = new System.Net.NetworkCredential("no-reply@mulphilog.com", "Mpl@1234");

            //Add this line to bypass the certificate validation
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object s,
                    System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                    System.Security.Cryptography.X509Certificates.X509Chain chain,
                    System.Net.Security.SslPolicyErrors sslPolicyErrors)
            {
                return true;
            };
            try
            {
                mail.Send(message);
            }
            catch (Exception Err)
            {
                Err.Message.ToString();
            }
        }
        protected void ResetAll()
        {
            txt_customername.Text = "";
            txt_mobile.Text = "";
            txt_email.Text = "";
            txt_cnic.Text = "";
            txt_address.Text = "";
            //  dd_bank.SelectedValue = "0";
            txt_ibft.Text = "";
            txt_accounttitle.Text = "";
            // txt_accountno.Text = "";
        }
        public void Alert(string message)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + message + "')", true);
            // Errorid.Text = message;
        }
    }
}