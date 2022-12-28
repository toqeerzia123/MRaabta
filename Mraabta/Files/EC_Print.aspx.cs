using MRaabta.App_Code;
using System;
using System.Data;
using System.Data.SqlClient;

namespace MRaabta.Files
{
    public partial class EC_Print : System.Web.UI.Page
    {

        Cl_Variables clvar = new Cl_Variables();


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string RiderCode = Request.QueryString["EC_Code"];
                string BranchCode = Request.QueryString["BCode"];

                string sql = " \n"
                + "SELECT ec.*, b.name BranchName, z.name ZoneName, mect.ExpressCenter Ec_Type,case when ec.[status] = '1' THEN 'Active' ELSE 'InActive' END Ec_Status, mect.EC_Sname \n"
                + "FROM   ExpressCenters ec \n"
                + "       INNER JOIN Branches b ON b.branchCode = ec.bid INNER JOIN Mnp_ExpressCenterType mect ON ec.Center_type = mect.ExpressCenter_TypeID \n"
                + "        INNER JOIN Zones z ON z.zoneCode = b.zoneCode \n"
                + "WHERE  b.[status]='1' \n"
                + "AND ec.expressCenterCode ='" + RiderCode + "' \n"
                + " \n"
                + "ORDER BY \n"
                + "       ec.createdOn DESC ";


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
                    this.lbl_FName.Text = ds.Tables[0].Rows[0]["name"].ToString();
                    this.lbl_Address.Text = ds.Tables[0].Rows[0]["Address"].ToString();
                    this.lbl_CNIC.Text = ds.Tables[0].Rows[0]["fax"].ToString();
                    this.lbl_Phone.Text = ds.Tables[0].Rows[0]["phone"].ToString();
                    lbl_RiderCode.Text = ds.Tables[0].Rows[0]["EC_Sname"].ToString() + "-" + ds.Tables[0].Rows[0]["expressCenterCode"].ToString();
                    lbl_RiderType.Text = ds.Tables[0].Rows[0]["email"].ToString();
                    lbl_RShiftType.Text = ds.Tables[0].Rows[0]["Ec_Type"].ToString();
                    lbl_Ec.Text = "";// ds.Tables[0].Rows[0]["ECName"].ToString();
                    lbl_BranchName.Text = ds.Tables[0].Rows[0]["BranchName"].ToString();
                    lbl_ZoneName.Text = ds.Tables[0].Rows[0]["ZoneName"].ToString();

                    //  lbl_sep.Text = ds.Tables[0].Rows[0]["SeparationType"].ToString();
                    lbl_sepDate.Text = ds.Tables[0].Rows[0]["DateOfLeaving"].ToString();
                    lbl_remarks.Text = ds.Tables[0].Rows[0]["deActivateReamarks"].ToString();
                    lbl_EDate.Text = ds.Tables[0].Rows[0]["createdOn"].ToString();
                    lbl_Status.Text = ds.Tables[0].Rows[0]["Ec_Status"].ToString();

                }

            }
        }
    }
}