using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using MRaabta.App_Code;
using System.Net;
using System.IO;
using System.Text;
using OfficeOpenXml;
using System.Configuration;
using System.Threading.Tasks;
using Dapper;
using System.Globalization;

namespace MRaabta.Files
{
    public partial class Pickup_Rider : System.Web.UI.Page
    {
        Cl_Variables clvar = new Cl_Variables();
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                txt_pickDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

                DateTime dt = DateTime.Parse(DateTime.Now.ToString("HH:m tt"));
                MKB.TimePicker.TimeSelector.AmPmSpec am_pm;
                if (dt.ToString("tt") == "AM")
                {
                    am_pm = MKB.TimePicker.TimeSelector.AmPmSpec.AM;
                }
                else
                {
                    am_pm = MKB.TimePicker.TimeSelector.AmPmSpec.PM;
                }
                TimeSelector1.SetTime(dt.Hour, dt.Minute, am_pm);

            }
        }

        public DataSet Get_Origin(string AccountNo)
        {
            string sqlString = "select distinct c.branchCode, b.name, c.name AccountName, c.address\n" +
            "  from CreditClients c, Branches b\n" +
            " where c.accountNo = '" + AccountNo + "'\n" +
            "   and c.branchCode = b.branchCode and c.branchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'\n" +
            " order by b.name";


            DataSet ds = new DataSet();
            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                orcl.Close();
            }
            catch (Exception Err)
            { }
            finally
            { }
            return ds;
        }
        protected void btn_AddBooking_Click(object sender, EventArgs e)
        {
            string accountNo = txt_AccountNo.Text;
            string origin = dd_Origin.SelectedValue;
            string rider = dd_presentRiders.SelectedValue;// dd_riders.Text;
            string pickDate = txt_pickDate.Text;
            string t = type.SelectedValue;


            string accountName = lbl_AccountName.Text;
            string accountAddress = lbl_AccountAddress.Text;

            if (txt_alternateAddress.Text != "")
            {
                accountAddress = txt_alternateAddress.Text;
            }


            DateTime time = DateTime.Parse(string.Format("{0}:{1}:{2} {3}", TimeSelector1.Hour, TimeSelector1.Minute, TimeSelector1.Second, TimeSelector1.AmPm));

            string pickTime = time.ToString("hh:mm tt");
            string weight = txt_weight.Text;
            string pieces = txt_pieces.Text;

            double weightt = 0;//double.Parse(weight);

            try
            {
                Errorid.Text = "";
                weightt = double.Parse(weight);
            }
            catch (Exception dsdf)
            {
                Errorid.Text = "Invalid Weight!";
                return;
            }

            int piecess = 0;

            try
            {
                Errorid.Text = "";
                piecess = int.Parse(pieces);
            }
            catch (Exception dsdf)
            {
                Errorid.Text = "Invalid Pieces!";
                return;
            }


            // DataSet ds = new DataSet();

            //ds = Get_RiderPhone(origin, rider);

            //if (ds.Tables[0].Rows.Count > 0)
            //{

            string Cellphone = txt_RiderPhone.Text;
            var bc = Session["BRANCHCODE"].ToString();
            var rs = UpdateRiderPhoneNo(rider, Cellphone, bc);

            string number = SendMobileNumName(Cellphone);


            string SMSContent = "Dear Rider, \n Pickup Account : " + accountName + "\n Pickup Address: " + accountAddress + " \n Pickup Date: " + pickDate + "\n Pickup Time: " + pickTime + "\n Weight: " + weight + "\n Pieces: " + pieces;

            int inserter = 0;

            pickDate = pickDate + " " + pickTime;

            inserter = Insert_PickupDetails(accountNo, origin, rider, weightt, pickDate, Cellphone, pieces, accountAddress, t);

            if (inserter == 0)
            {
                int smsStatus = InsertSMSDetail(SMSContent, number);

                //Post_BrandedSMS(SMSContent, number);

                Errorid.Text = "Pick up details successfully saved!";
                txt_AccountNo.Text = "";
                lbl_riderName.Text = "";
                txt_alternateAddress.Text = "";
                txt_RiderPhone.Text = "";
                txt_pickDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                DateTime dt = DateTime.Parse(DateTime.Now.ToString("HH:m tt"));
                MKB.TimePicker.TimeSelector.AmPmSpec am_pm;
                if (dt.ToString("tt") == "AM")
                {
                    am_pm = MKB.TimePicker.TimeSelector.AmPmSpec.AM;
                }
                else
                {
                    am_pm = MKB.TimePicker.TimeSelector.AmPmSpec.PM;
                }
                TimeSelector1.SetTime(dt.Hour, dt.Minute, am_pm);
                txt_weight.Text = "";
                txt_pieces.Text = "";
                lbl_AccountAddress.Text = "";
                lbl_AccountName.Text = "";
                dd_Origin.Items.Clear();
                dd_riders.Text = "";
            }
            else
            {
                Errorid.Text = "Cannot Save Pick up Detail!";
            }


            //}




        }
        protected void txt_AccountNo_TextChanged(object sender, EventArgs e)
        {
            string accountNo = txt_AccountNo.Text;

            DataSet ds = new DataSet();

            ds = Get_Origin(accountNo);


            if (ds.Tables[0].Rows.Count > 0)
            {
                lbl_AccountAddress.Text = ds.Tables[0].Rows[0]["address"].ToString();
                lbl_AccountName.Text = ds.Tables[0].Rows[0]["AccountName"].ToString();

                txt_AccountNo.BackColor = System.Drawing.Color.LightGreen;
                dd_Origin.DataTextField = "name";
                dd_Origin.DataValueField = "branchcode";
                dd_Origin.DataSource = ds;
                dd_Origin.DataBind();

                dd_Origin_SelectedIndexChanged(sender, e);

            }
            else
            {
                lbl_AccountAddress.Text = "";
                lbl_AccountName.Text = "";
                dd_Origin.Items.Clear();
                txt_AccountNo.BackColor = System.Drawing.Color.Pink;
                return;
            }
        }
        protected void dd_Origin_SelectedIndexChanged(object sender, EventArgs e)
        {
            string accountNo = txt_AccountNo.Text;
            string origin = dd_Origin.SelectedValue;

            DataTable ds = GetPresentRiders(origin, txt_pickDate.Text);

            if (ds.Rows.Count > 0)
            {
                dd_presentRiders.Items.Clear();
                dd_presentRiders.DataTextField = "firstname";
                dd_presentRiders.DataValueField = "ridercode";
                dd_presentRiders.DataSource = ds;
                dd_presentRiders.DataBind();
                //dd_presentRiders.SelectedValue = ds.Rows[0]["RiderCode"].ToString();


                dd_riders_SelectedIndexChanged(sender, e);


            }
            else
            {
                ViewState["PR"] = ds;
                dd_presentRiders.Items.Clear();
                return;
            }

        }
        protected void PresentRiders()
        {



        }

        public DataSet Get_Riders(string origin)
        {
            string sqlString = "select r.riderCode + ' - ' + r.firstName + ' ' + r.lastname FirstName, r.riderCode\n" +
            "  from Riders r\n" +
            " where r.branchId = '" + origin + "'\n" +
            " order by r.firstName";

            DataSet ds = new DataSet();
            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                orcl.Close();
            }
            catch (Exception Err)
            { }
            finally
            { }
            return ds;
        }

        public void Post_BrandedSMS(string resp, string mobile)
        {
            try
            {
                // Create a request using a URL that can receive a post. 
                WebRequest request = WebRequest.Create("https://services.converget.com/MTEntryDyn/?");
                // Set the Method property of the request to POST.
                request.Method = "POST";
                // Create POST data and convert it to a byte array.

                //String Mobile = HttpUtility.UrlEncode(ClsVar.MobileNumber);
                String Mobile = HttpUtility.UrlEncode(mobile);
                String Response = HttpUtility.UrlEncode(resp);
                string postData = "MSISDN=" + Mobile + "&msgdata=" + Response + "&uid=MullerPhippsSMS&pwd=MullerPhipps2014@Admin";//"PhoneNumber=" + Mobile + "&Text=" + Response;

                //////string postData = "PhoneNumber=" + clvar.MobileNumber + "&Text=" + resp;
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                ////// Set the ContentType property of the WebRequest.
                request.ContentType = "application/x-www-form-urlencoded";
                ////// Set the ContentLength property of the WebRequest.
                request.ContentLength = byteArray.Length;
                // Get the request stream.
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();
                // Get the response.
                WebResponse response = request.GetResponse();
                // Display the status.
                Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                // Get the stream containing content returned by the server.
                dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();
                // Display the content.
                Console.WriteLine(responseFromServer);
                // Clean up the streams.
                reader.Close();
                dataStream.Close();
                response.Close();




            }
            catch (Exception Err)
            {

            }

        }


        public string SendMobileNumName(string consigneeNumber)
        {
            string Number = consigneeNumber.Replace("-", "");

            int numLength = Number.Length;
            string n2 = Number;

            if (numLength != 12)
            {
                if (numLength == 13)
                {
                    n2 = Number.Remove(0, 1);
                }
                else if (numLength == 11)
                {

                    string code = "92";
                    n2 = code + Number.Remove(0, 1);

                }
                else if (numLength == 10)
                {
                    string code = "92";
                    n2 = code + Number;
                }
                else
                {
                    string code = "92";
                    n2 = code + Number;
                }

            }
            return n2;
        }

        public DataSet Get_RiderPhone(string origin, string riderCode)
        {
            string sqlString = "select r.phoneNo, r.firstName + ' - ' + r.lastname firstName\n" +
            "  from Riders r\n" +
            " where r.branchId = '" + origin + "'\n" +
            "   and r.riderCode = '" + riderCode + "'\n" +
            " order by r.firstName";

            DataSet ds = new DataSet();
            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                orcl.Close();
            }
            catch (Exception Err)
            { }
            finally
            { }
            return ds;
        }

        public DataSet Check_riderForAbsent(string origin, string riderCode, string date)
        {
            string sqlString = "select *\n" +
            "  from Pickup_Rider_attendence r\n" +
            " where r.origin = '" + origin + "'\n" +
            "   and r.riderCode = '" + riderCode + "' and cast(r.attendenceDate as DATE)  = '" + date + "'";

            DataSet ds = new DataSet();
            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                orcl.Close();
            }
            catch (Exception Err)
            { }
            finally
            { }
            return ds;
        }

        public int UpdateRiderPhoneNo(string ridercode, string phoneno, string bc)
        {
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
            {
                con.Open();
                var rs = con.Execute(@"update riders set phoneNo = @phoneno where riderCode = @ridercode and branchId = @bc and status = 1;", new { phoneno, ridercode, bc });
                con.Close();
                return rs;
            }
        }

        public int Insert_PickupDetails(string accountNo, string origin, string rider, double weight, string pickupDate, string riderPhone, string pieces, string alternate_address, string type)
        {
            int un = 0;
            SqlConnection orcl = new SqlConnection(clvar.Strcon());


            string sqlString = "INSERT INTO Pickup_Details\n" +
            "  (AccountNo,\n" +
            "   origin,\n" +
            "   riderCode,\n" +
            "   weight,\n" +
            "   pickup_Date, riderPhone, pieces, createdOn, pickup_status, alternate_address,type)\n" +
            "VALUES\n" +
            "  ('" + accountNo + "','" + origin + "','" + rider + "'," + weight + ",'" + pickupDate + "','" + riderPhone + "'," + pieces + ",GETDATE(),0,'" + alternate_address + "','" + type + "')";



            try
            {
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandType = CommandType.Text;
                orcd.ExecuteNonQuery();
            }
            catch (Exception Err)
            {
                un = 1;

            }
            finally
            {
                orcl.Close();
            }
            return un;
        }

        protected void dd_riders_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = ViewState["PR"] as DataTable;
            if (dt.Rows.Count > 0)
            {
                txt_RiderPhone.Text = dt.Select("RIDERCODE = '" + dd_presentRiders.SelectedValue + "'")[0]["PhoneNo"].ToString().Replace("-", "");
            }


        }
        protected void dd_riders_TextChanged(object sender, EventArgs e)
        {
            string accountNo = txt_AccountNo.Text;
            string origin = dd_Origin.SelectedValue;
            string rider = dd_riders.Text;
            string pickDate = txt_pickDate.Text;


            DataSet absentChecker = new DataSet();

            string attendenceDate = DateTime.Now.ToString("yyyy-MM-dd");

            absentChecker = Check_riderForAbsent(origin, rider, attendenceDate);

            if (absentChecker.Tables[0].Rows.Count > 0)
            {
                Errorid.Text = "Rider absent for Date: " + DateTime.Now.ToString("yyyy-MM-dd");
                dd_riders.BackColor = System.Drawing.Color.Pink;
                return;
            }
            else
            {
                Errorid.Text = "";
                DataSet ds = new DataSet();

                ds = Get_RiderPhone(origin, rider);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    dd_riders.BackColor = System.Drawing.Color.LightGreen;
                    string Cellphone = ds.Tables[0].Rows[0][0].ToString();

                    string firstName = ds.Tables[0].Rows[0][1].ToString();
                    txt_RiderPhone.Text = Cellphone;
                    lbl_riderName.Text = firstName;
                }
                else
                {
                    txt_RiderPhone.Text = "";
                    lbl_riderName.Text = "";
                    dd_riders.BackColor = System.Drawing.Color.Pink;
                    return;
                }
            }
        }

        protected DataTable GetPresentRiders(string origin, string date)
        {
            string sqlString = "select r.riderCode + '-' + r.firstName + ' ' + r.lastName FirstName, r.firstName + ' ' + r.lastName RiderName,\n" +
            "       r.riderCode,\n" +
            "       r.phoneNo,\n" +
            "       case\n" +
            "         when pr.riderCode is null then\n" +
            "          'PRESENT'\n" +
            "         else\n" +
            "          'ABSENT'\n" +
            "       END ATTENDANCE\n" +
            "  from Riders r\n" +
            "  left outer join Pickup_Rider_attendence pr\n" +
            "    on pr.riderCode = r.riderCode\n" +
            "   and pr.origin = r.branchId\n" +
            "   and pr.origin = '" + origin + "'\n" +
            "   and pr.AttendenceDate = '" + date + "'\n" +
            " where r.branchId = '" + origin + "'\n" +
            "   and r.status = '1'\n" +
            " order by 1";

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sqlString, con);
                sda.Fill(dt);
            }
            catch (Exception ex)
            {

            }
            finally { con.Close(); }
            ViewState["PR"] = dt;
            return dt;
        }


        public int InsertSMSDetail(string messageContent, string Recepient)
        {
            SqlConnection con = new SqlConnection(clvar.Strcon());
            int returnCount = 0;
            string command = "INSERT INTO MnP_SmsStatus \n"
               + "  ( \n"
               + "    -- MessageID -- this column value is auto-generated \n"
               + "     \n"
               + "    Recepient, \n"
               + "    MessageContent, \n"
               + "    [STATUS], \n"
               + "    CreatedOn, \n"
               + "    CreatedBy,smsformtype \n"
               + "  ) \n"
               + "VALUES \n"
               + "  ( \n"
               + "    '" + Recepient + "', \n"
               + "    '" + messageContent + "', \n"
               + "    '0', \n"
               + "    GETDATE(), \n"
               + "    '" + HttpContext.Current.Session["U_ID"].ToString() + "','9' \n"
               + "  )";
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(command, con);
                returnCount = cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return returnCount;
        }
    }
}