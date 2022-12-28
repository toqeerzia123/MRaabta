using System;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Data.SqlClient;
using MRaabta.App_Code;
using System.Globalization;

namespace MRaabta.Files
{
    public partial class Accounts_DayEnd : System.Web.UI.Page
    {
        CommonFunction cfunc = new CommonFunction();
        Cl_Variables clvar = new Cl_Variables();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["U_ID"].ToString() != "1786")
            {
                //AlertMessage("Unauthorized User.");
                // ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "alert('You are not authorized to view this page.'); window.location='" //Request.ApplicationPath + "/login';", true);
                //Response.Redirect("~/login");
                // return;
            }
            if (!IsPostBack)
            {
                GetZones();
                GetDocTypes();
                Years();
                DisplayDayend();
                //PopulateCalendar();
            }
        }

        protected void GetZones()
        {
            DataTable dt = GetAllZones();

            if (dt.Rows.Count > 0)
            {
                dd_zone.DataSource = dt;
                dd_zone.DataTextField = "name";
                dd_zone.DataValueField = "zoneCode";
                dd_zone.DataBind();
            }
        }
        protected void GetDocTypes()
        {
            DataTable dt = GetDocumentTypes();
            if (dt.Rows.Count > 0)
            {
                dd_docType.DataSource = dt;
                dd_docType.DataTextField = "Document";
                dd_docType.DataValueField = "Doc_type";
                dd_docType.DataBind();
            }
        }
        protected void Years()
        {
            DataTable dt = GetYears();
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    dd_year.DataSource = dt;
                    dd_year.DataTextField = "YEAR";
                    dd_year.DataValueField = "YEAR";
                    dd_year.DataBind();
                }
            }

        }
        protected void DisplayDayend()
        {
            clvar.Zone = dd_zone.SelectedValue;

            DataTable dt = GetDayEnds(clvar);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    gv_dayends.DataSource = dt;
                    gv_dayends.DataBind();
                }
                else
                {
                    gv_dayends.DataSource = null;
                    gv_dayends.DataBind();
                }
            }

        }
        protected void DisplayDayEndWithParameters()
        {

        }


        protected void dd_zone_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayDayend();
        }
        protected void dd_year_SelectedIndexChanged(object sender, EventArgs e)
        {
            clvar.CheckCondition = "and YEAR(d.DateTime) = '" + dd_year.SelectedValue + "'\n";
            DisplayDayend();
        }
        protected void dd_month_SelectedIndexChanged(object sender, EventArgs e)
        {
            clvar.CheckCondition = "and YEAR(d.DateTime) = '" + dd_year.SelectedValue + "'\n and MONTH(d.DateTime) = '" + dd_month.SelectedValue + "'";
            DisplayDayend();
        }
        protected void dd_docType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }



        public DataTable GetAllZones()
        {
            string query = "selecT z.zoneCode, z.name from Zones z where z.zoneCode in (select zoneCode from Branches b where b.branchCode <> '17') and z.status = '1' order by name";
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

        public DataTable GetDocumentTypes()
        {
            string query = "selecT * from Mnp_DocumentType";
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

        public DataTable GetDayEnds(Cl_Variables clvar)
        {

            string sqlString = "selecT d.Zone, z.name ZoneName, d.Doc_Type, MAX(DateTime) DayEndDate\n" +
            "  from Mnp_Account_DayEnd d\n" +
            " inner join Zones z\n" +
            "    on z.zoneCode = d.Zone\n" +
            " where d.Zone = '" + clvar.Zone + "'\n" +
            " group by d.Zone, z.name, d.Doc_Type";

            sqlString = "selecT d.Zone,\n" +
           "       z.name ZoneName,\n" +
           "       d.Doc_Type,\n" +
           "       dt.Document,\n" +
           "       MAX(DateTime) DayEndDate\n" +
           "  from Mnp_Account_DayEnd d\n" +
           " inner join Zones z\n" +
           "    on z.zoneCode = d.Zone\n" +
           " inner join Mnp_DocumentType dt\n" +
           "    on dt.Doc_type = d.Doc_Type\n" +
           " where d.Zone = '" + clvar.Zone + "'\n" +
           " " + clvar.CheckCondition + " \n" +
           " group by d.Zone, z.name, d.Doc_Type, dt.Document";

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

        public DataTable GetYears()
        {
            string query = "select distinct YEAR from Calendar";
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


        protected void btn_dayEnd_Click(object sender, EventArgs e)
        {


        }

        public string AccountsDayEnd(Cl_Variables clvar)
        {
            string query = "insert into Mnp_Account_DayEnd\n" +
                "selecT '" + clvar.Zone + "' ZoneCode, b.branchCode, '" + DateTime.Parse(clvar.BookingDate).ToString("yyyy-MM-dd") + "' DateTime, '" + clvar.ServiceType + "' Doc_Type, GETDATE() CREATEDON, '" + HttpContext.Current.Session["U_ID"].ToString() + "' CreatedBy from branches b where b.zoneCode = '" + clvar.Zone + "' and b.status = '1'";
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return "OK";
        }


        public void AlertMessage(string message)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + message + "')", true);

        }
        protected void btn_dayEnd_Click1(object sender, EventArgs e)
        {
            clvar.Zone = dd_zone.SelectedValue;
            clvar.BookingDate = DateTime.Parse(txt_date.Text).ToShortDateString();
            clvar.ServiceType = dd_docType.SelectedValue;
            clvar.CheckCondition = " and d.Doc_Type = '" + clvar.ServiceType.ToString() + "' ";
            DateTime date = new DateTime();
            DataTable dt = GetDayEnds(clvar);
            DateTime minDate = new DateTime();
            DateTime maxDate = new DateTime();
            if (dt.Rows.Count > 0)
            {
                date = DateTime.Parse(dt.Rows[0]["DayEndDate"].ToString());

            }
            else
            {
                //date = DateTime.Parse("31/05/2016");
                //date = DateTime.Parse("01/" +  + "/" + DateTime.Now.Year).AddDays(-1);
                if (DateTime.Parse(txt_date.Text) >= DateTime.Today)
                {
                    AlertMessage("Cannot Perform Day End for later than or equal to todays Date.");
                    return;
                }
                div1.Style.Add("display", "block");
                return;

            }
            minDate = date.AddDays(1);
            maxDate = DateTime.Now.AddDays(-1);


            if (DateTime.Parse(txt_date.Text) >= minDate && DateTime.Parse(txt_date.Text) <= maxDate)
            {
                AccountsDayEnd(clvar);
                DisplayDayend();
            }
            else
            {
                AlertMessage("Dayend Can only be performed within " + minDate.ToString("yyyy-MM-dd") + " AND " + maxDate.ToString("yyyy-MM-dd") + ".");
                return;
            }

        }

        protected void btn_ok2_Click(object sender, EventArgs e)
        {
            clvar.BookingDate = txt_date.Text;
            clvar.ServiceType = dd_docType.SelectedValue;
            clvar.Zone = dd_zone.SelectedValue;
            string errorMessage = AccountsDayEnd(clvar);
            if (errorMessage == "OK")
            {
                AlertMessage("Day End Completed.");
                DisplayDayend();
            }
            else
            {
                AlertMessage("Day End Could not be Completed.");
            }

            div1.Style.Add("display", "none");


        }





        protected void PopulateCalendar()
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[]
            {
            new DataColumn("ID"),
            new DataColumn("CompanyID"),
            new DataColumn("Year",typeof(string)),
            new DataColumn("Month",typeof(string)),
            new DataColumn("MonthName",typeof(string)),
            new DataColumn("Week",typeof(string)),
            new DataColumn("Day",typeof(string)),
            new DataColumn("DayDate",typeof(string)),
            new DataColumn("FullDate",typeof(string)),
            new DataColumn("WorkingDate",typeof(DateTime))

            });


            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
            DateTime date1 = new DateTime(2011, 1, 1);
            System.Globalization.Calendar cal = dfi.Calendar;


            int week = 0;

            DateTime date = DateTime.Parse("01/01/2017");
            DateTime lastDate = DateTime.Parse("31/12/2017");
            int id = 1;
            while (date <= lastDate)
            {

                int year = date.Year;
                int month = date.Month;
                int day_ = date.Day;
                string monthName = date.ToString("MMMM");
                string dayName = date.ToString("dddd");
                string fullDate = date.ToString();
                string workingDate = date.ToString("yyyy-MM-dd");
                var day = (int)CultureInfo.CurrentCulture.Calendar.GetDayOfWeek(date);
                week = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(date.AddDays(4 - (day == 0 ? 7 : day)), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
                insertCalendar(id.ToString(), "", year.ToString(), month.ToString(), monthName, week.ToString(), dayName, day_.ToString(), fullDate, workingDate);
                id++;
                date = date.AddDays(1);

            }

        }
        protected void insertCalendar(string id, string companyID, string year, string month, string monthname, string week, string day, string daydate, string fulldate, string workingdate)
        {
            string query = "Insert into Calendar (ID, COMPANYID, YEAR, MONTH, MONTHNAME, WEEK, DAY, DAYDATE, FULLDATE, WORKINGDATE) VALUES \n" +
                            " (\n" +
                            "  '" + id + "', '01', '" + year + "', '" + month + "', '" + monthname + "', '" + week + "', '" + day + "', '" + daydate + "', '" + fulldate + "', '" + workingdate + "') ";



            SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            try
            {
                sqlcon.Open();
                SqlCommand sqlcmd = new SqlCommand(query, sqlcon);
                sqlcmd.CommandType = CommandType.Text;

                sqlcmd.ExecuteNonQuery();
                //IsUnique = Int32.Parse(SParam.Value.ToString());
                // obj.XCode = obj.consignmentNo;
                sqlcon.Close();
            }
            catch (Exception ex)
            {

            }

        }
    }
}