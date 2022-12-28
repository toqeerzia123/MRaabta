using MRaabta.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MRaabta.Files
{
    public partial class Rider_Print : System.Web.UI.Page
    {

        Cl_Variables clvar = new Cl_Variables();


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string RiderCode = Request.QueryString["RiderCode"];
                string BranchCode = Request.QueryString["BCode"];

                string sql = "SELECT r.firstName, \n"
                  + "       r.MiddleName, \n"
                  + "       r.lastName, \n"
                  + "       r.[address], \n"
                  + "       r.phoneNo, \n"
                  + "       r.CNIC, \n"
                  + "       r.expressCenterId, \n"
                  + "       ec.name     ECName, \n"
                  + "       r.branchId, \n"
                  + "       b.name      BName, \n"
                  + "       r.zoneId, \n"
                  + "       z.name      ZName, \n"
                  + "       ( \n"
                  + "           SELECT AttributeValue \n"
                  + "           FROM   rvdbo.Lookup l \n"
                  + "           WHERE  CAST(l.Id AS VARCHAR) = r.cid \n"
                  + "       )           RiderType, \n"
                  + "       ( \n"
                  + "           SELECT AttributeValue \n"
                  + "           FROM   rvdbo.Lookup l \n"
                  + "           WHERE  CAST(l.Id AS VARCHAR) = r.dutyTypeId \n"
                  + "       )           DutyType, \n"
                  + "       ( \n"
                  + "           SELECT AttributeValue \n"
                  + "           FROM   rvdbo.Lookup l \n"
                  + "           WHERE  l.Id = r.Shift \n"
                  + "       )           RiderShift, \n"
                  + "       r.Hrs_code, \n"
                  + "       r.DOB, \n"
                  + "       r.DOJ, \n"
                  + "       r.routeCode, \n"
                  + "       ( \n"
                  + "           SELECT r2.name \n"
                  + "           FROM   Routes r2 \n"
                  + "                  INNER JOIN Cities c \n"
                  + "                       ON  c.id = r2.cityId \n"
                  + "           WHERE  c.id = ( \n"
                  + "                      SELECT b.cityId \n"
                  + "                      FROM   Branches b \n"
                  + "                      WHERE  b.branchCode = r.branchId \n"
                  + "           ) AND r2.routeCode =r.routeCode \n"
                  + "             AND r2.RiderCode =r.riderCode \n"
                  + "             AND R2.BID = R.branchId \n"
                  + "       )           routename, \n"
                  + "       r.riderCode, \n"
                  + "       ( \n"
                  + "           SELECT AttributeValue \n"
                  + "           FROM   rvdbo.Lookup l \n"
                  + "           WHERE  CAST(l.Id AS VARCHAR) = r.department \n"
                  + "       )           Department, \n"
                  + "       r.[SeparationType], \n"
                  + "       r.[DateOfLeaving], \n"
                  + "       r.[deActivateBy], \n"
                  + "       r.[deActivateReamarks], \n"
                  + "       r.[deactivationDate],r.createdOn,CASE WHEN r.[status] ='1' THEN 'Active' ELSE 'InActive' END Status \n"
                  + "FROM   Riders r \n"
                  + "       INNER JOIN Branches b \n"
                  + "            ON  b.branchCode = r.branchId \n"
                  + "       INNER JOIN Zones z \n"
                  + "            ON  r.zoneId = z.zoneCode \n"
                  + "       INNER JOIN ExpressCenters ec \n"
                  + "            ON  r.expressCenterId = ec.expressCenterCode "
                  + "WHERE  r.riderCode = '" + RiderCode + "' \n"
                  + "       AND r.branchId = '" + BranchCode + "'";

                SqlConnection con = new SqlConnection(clvar.Strcon());
                DataSet ds = new DataSet();
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(sql, con);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(ds);

                }
                catch (Exception ex)
                { }
                finally { con.Close(); }

                if (ds.Tables[0].Rows.Count != 0)
                {
                    this.lbl_FName.Text = ds.Tables[0].Rows[0]["FirstName"].ToString();
                    this.lbl_MName.Text = ds.Tables[0].Rows[0]["MIddleName"].ToString();
                    this.lbl_LName.Text = ds.Tables[0].Rows[0]["LastName"].ToString();
                    this.lbl_Address.Text = ds.Tables[0].Rows[0]["Address"].ToString();
                    this.lbl_CNIC.Text = ds.Tables[0].Rows[0]["CNIC"].ToString();
                    this.lbl_Phone.Text = ds.Tables[0].Rows[0]["phoneNo"].ToString();
                    lbl_RiderCode.Text = ds.Tables[0].Rows[0]["riderCode"].ToString();
                    lbl_RiderDutyType.Text = ds.Tables[0].Rows[0]["DutyType"].ToString();
                    lbl_RiderType.Text = ds.Tables[0].Rows[0]["RiderType"].ToString();
                    lbl_RShiftType.Text = ds.Tables[0].Rows[0]["RiderShift"].ToString();
                    lbl_Ec.Text = ds.Tables[0].Rows[0]["ECName"].ToString();
                    lbl_BranchName.Text = ds.Tables[0].Rows[0]["BName"].ToString();
                    lbl_ZoneName.Text = ds.Tables[0].Rows[0]["ZName"].ToString();
                    lbl_RouteCode.Text = ds.Tables[0].Rows[0]["routeCode"].ToString();
                    lbl_RouteName.Text = ds.Tables[0].Rows[0]["routename"].ToString();
                    lbl_HrCode.Text = ds.Tables[0].Rows[0]["Hrs_code"].ToString();
                    lbl_DOB.Text = ds.Tables[0].Rows[0]["DOB"].ToString();
                    lbl_DOJ.Text = ds.Tables[0].Rows[0]["DOJ"].ToString();
                    lbl_dept.Text = ds.Tables[0].Rows[0]["department"].ToString();
                    lbl_sep.Text = ds.Tables[0].Rows[0]["SeparationType"].ToString();
                    lbl_sepDate.Text = ds.Tables[0].Rows[0]["DateOfLeaving"].ToString();
                    lbl_remarks.Text = ds.Tables[0].Rows[0]["deActivateReamarks"].ToString();
                    lbl_EDate.Text = ds.Tables[0].Rows[0]["createdOn"].ToString();
                    lbl_Status.Text = ds.Tables[0].Rows[0]["Status"].ToString();

                }

            }



        }


    }
}