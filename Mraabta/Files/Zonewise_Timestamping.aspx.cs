using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;

using MRaabta.App_Code;
namespace MRaabta.Files
{
    public partial class Zonewise_Timestamping : System.Web.UI.Page
    {
        clsVariables clsv = new clsVariables();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                fillDropdown();
            }
        }
        public void fillDropdown()
        {
            dpd_zone.DataSource = clsv.DataFill("select zoneCode, Name from Zones z where z.type = '1' AND z.zoneCode in ('1', '10', '11', '12', '2', '27', '3','4','5','7','9')order by z.name ASC");
            dpd_zone.DataTextField = "Name";
            dpd_zone.DataValueField = "ZoneCode";
            dpd_zone.DataBind();
            dpd_zone.Items.Insert(0, new ListItem("-- Select --", "0"));


        }
        public void viewReportdata()
        {
            string from = clsv.GetDate(txt_date_from.Text).ToString();
            string to = clsv.GetDate(txt_date_to.Text).ToString();


            string sqlString = "select\n" +
            "a.RiderCode,\n" +
            "a.shift,\n" +
            "a.firstName,\n" +
            "a.lastName,\n" +
            "a.ZoneCode,\n" +
            "a.Zone,\n" +
            "a.branchCode,\n" +
            "a.Branch,\n" +
            "a.Department,\n" +
            "a.Designation,\n" +
            "a.DATE,\n" +
            "a.Designation,\n" +
            "a.userTypeId ,\n" +
            "CONVERT(varchar(15), CAST(a.LASTIN AS TIME), 108) LASTIN,\n" +
            "CONVERT(VARCHAR(10), a.LASTIN, 103) DateIn,\n" +
            "CONVERT(varchar(15), CAST(a.FIRSTOUT AS TIME), 108) FIRSTOUT,\n" +
            "CONVERT(VARCHAR(10), a.FIRSTOUT , 103) DateOut\n" +
            "\n" +
            "\n" +
            " from\n" +
            "(\n" +
            "SELECT B.RiderCode,\n" +
            "       R.firstName,\n" +
            "       R.lastName,\n" +
            "       Z.ZoneCode,\n" +
            "       Z.Name Zone,\n" +
            "       BB.branchCode,\n" +
            "       BB.name Branch,\n" +
            "       'Operations' Department,\n" +
            "       r.cid Designation,\n" +
            "       ec.ShiftName shift,\n" +
            "       r.userTypeId,\n" +
            "       B.DATE,\n" +
            "       MAX(CASE\n" +
            "             WHEN B.REMARKS = 'LASTIN' THEN\n" +
            "              B.TIME\n" +
            "             ELSE\n" +
            "              ' '\n" +
            "           END) LASTIN,\n" +
            "       MAX(CASE\n" +
            "             WHEN B.REMARKS = 'FIRSTOUT' THEN\n" +
            "              B.TIME\n" +
            "             ELSE\n" +
            "              ' '\n" +
            "           END) FIRSTOUT\n" +
            "  FROM (SELECT S.RiderCode,\n" +
            "               CONVERT(VARCHAR(10), S.DATETIME, 103) DATE,\n" +
            "               MIN(S.DATETIME) TIME,\n" +
            "               'FIRSTOUT' REMARKS\n" +
            "          FROM TIMESTAMPING AS S\n" +
            "         --WHERE S.RiderCode = '1039' AND\n" +
            "           where S.[NEW STATE] = 'C/OUT'\n" +
            "         GROUP BY S.RiderCode, CONVERT(VARCHAR(10), S.DATETIME, 103)\n" +
            "\n" +
            "        UNION\n" +
            "\n" +
            "        SELECT S.RiderCode,\n" +
            "               CONVERT(VARCHAR(10), S.DATETIME, 103) DATE,\n" +
            "               MAX(S.DATETIME) TIME,\n" +
            "               'LASTIN' REMARKS\n" +
            "          FROM TIMESTAMPING AS S\n" +
            "         WHERE -- S.RiderCode = '1255' AND\n" +
            "         [NEW STATE] != 'C/OUT'\n" +
            "           --AND CONVERT(VARCHAR(10), S.DATETIME, 103) BETWEEN '01/07/2016' AND '11/07/2016'\n" +
            "         GROUP BY S.RiderCode, CONVERT(VARCHAR(10), S.DATETIME, 103)) B\n" +
            "\n" +
            "\n" +
            " INNER JOIN Riders R\n" +
            " ON R.riderCode = B.RiderCode\n" +
            " and R.riderCode not IN('N/A','na','new')\n" +
            "\n" +
            "-- INNER JOIN Riders R\n" +
            "  --  ON R.riderCode = B.RiderCode\n" +
            " INNER JOIN Branches BB\n" +
            "    ON BB.branchCode = R.branchId\n" +
            "    INNER JOIN ExpressCenterShiftTime ec\n" +
            "    ON ec.Id = R.shift\n" +
            " INNER JOIN rvdbo.Zone Z\n" +
            "    ON Z.ZoneCode = R.zoneId\n" +
            "  --  where b.DATE\n" +
            " GROUP BY B.RiderCode,\n" +
            "       R.firstName,\n" +
            "       R.lastName,\n" +
            "       Z.ZoneCode,\n" +
            "       Z.Name,\n" +
            "       BB.branchCode,\n" +
            "       BB.name,\n" +
            "       r.cid,\n" +
            "       r.userTypeId,\n" +
            "       ec.ShiftName,\n" +
            "       B.DATE\n" +
            "       )a  where /*a.ACNo='' and */ a.ZoneCode='" + dpd_zone.SelectedValue + "' and a.branchCode='" + dpd_branch.SelectedValue + "' and a.DATE between CONVERT(VARCHAR(10), '" + from + "', 103) and CONVERT(VARCHAR(10), '" + to + "', 103)\n" +
            "       GROUP BY\n" +
            "       a.RiderCode,\n" +
            "a.shift,\n" +
            "a.firstName,\n" +
            "a.lastName,\n" +
            "a.ZoneCode,\n" +
            "a.Zone,\n" +
            "a.branchCode,\n" +
            "a.Branch,\n" +
            "a.Department,\n" +
            "a.Designation,\n" +
            "a.DATE,\n" +
            "a.Designation,\n" +
            "a.userTypeId ,\n" +
            "CONVERT(varchar(15), CAST(a.LASTIN AS TIME), 108),\n" +
            "CONVERT(VARCHAR(10), a.LASTIN, 103),\n" +
            "CONVERT(varchar(15), CAST(a.FIRSTOUT AS TIME), 108),\n" +
            "CONVERT(VARCHAR(10), a.FIRSTOUT , 103)";



            TimeSpan TotalHrs = (TimeSpan.Parse("0:00"));

            DataTable data = clsv.DataFill(sqlString);
            data.Columns.Add("WorkHrs", typeof(System.TimeSpan));
            data.Columns.Add("Remarks", typeof(System.String));
            data.Columns.Add("Physical", typeof(System.Int32));
            data.Columns.Add("Missing", typeof(System.Int32));



            foreach (DataRow row in data.Rows)
            {
                //need to set value to NewColumn column
                if (DateTime.Parse(row["FIRSTOUT"].ToString()) != Convert.ToDateTime("00:00:00") || DateTime.Parse(row["LASTIN"].ToString()) != Convert.ToDateTime("00:00:00"))
                {
                    if (DateTime.Parse(row["FIRSTOUT"].ToString()) != Convert.ToDateTime("00:00:00") && DateTime.Parse(row["LASTIN"].ToString()) != Convert.ToDateTime("00:00:00"))
                    {
                        row["WorkHrs"] = DateTime.Parse(row["FIRSTOUT"].ToString()).Subtract(DateTime.Parse(row["LASTIN"].ToString()));

                        TotalHrs.Add(TimeSpan.Parse(row[13].ToString()));

                    }
                    else
                    {

                        row["Remarks"] = "Time Missing";
                        row["Missing"] = 1;
                    }
                    row["Physical"] = 1;
                }
                else
                {
                    TotalHrs.Add(TimeSpan.Parse("0:00"));
                    row["WorkHrs"] = 0;
                    row["Remarks"] = "Time Missing";
                    row["Missing"] = 1;
                    row["Physical"] = 1;

                }

            }


            DateTime d1 = Convert.ToDateTime(from);
            DateTime d2 = Convert.ToDateTime(to);

            if (data.Rows.Count > 0)
            {
                int PhysicalPresent = Convert.ToInt32(data.Compute("Sum(Physical)", ""));
                lbl_department.Text = data.Rows[0]["Department"].ToString();
                lbl_employee_type.Text = data.Rows[0]["userTypeId"].ToString();
                lbl_designation.Text = data.Rows[0]["Designation"].ToString();
                lbl_empname.Text = data.Rows[0]["firstName"].ToString() + " " + data.Rows[0]["lastName"].ToString();
                lbl_empno.Text = data.Rows[0]["RiderCode"].ToString();
                lbl_zone.Text = data.Rows[0]["Zone"].ToString();
                lbl_branch.Text = data.Rows[0]["Branch"].ToString();
                lbl_Physical.Text = data.Compute("Sum(Physical)", "").ToString();
                lbl_missing.Text = data.Compute("Sum(Missing)", "").ToString();
                lbl_Absent.Text = Convert.ToString((d2 - d1).TotalDays - PhysicalPresent);
                lbl_PayDays.Text = (d2 - d1).TotalDays.ToString(); //Convert.ToString(days - 1);
                lbl_from.Text = from;
                lbl_To.Text = to;

                rpt_time.DataSource = data;
                rpt_time.DataBind();
                ViewReport.Visible = true;

            }
            else
            {
                ViewReport.Visible = false;
                lbl_error.Visible = true;
                lbl_error.Text = "NO DATA FOUND. !";

            }


        }

        protected void btn_view_Click(object sender, EventArgs e)
        {
            viewReportdata();
        }

        protected void dpd_branch_SelectedIndexChanged(object sender, EventArgs e)
        {


            dpd_branch.DataSource = clsv.DataFill("select b.branchCode,b.name as name from Branches b where b.status = '1' and b.zoneCode ='" + dpd_zone.SelectedValue + "'");
            dpd_branch.DataTextField = "Name";
            dpd_branch.DataValueField = "branchCode";
            dpd_branch.DataBind();
            dpd_branch.Items.Insert(0, new ListItem("-- Select --", "0"));
        }
    }
}