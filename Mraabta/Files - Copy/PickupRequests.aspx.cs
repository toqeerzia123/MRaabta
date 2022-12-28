using System;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using MRaabta.App_Code;
using System.Web.Services;

namespace MRaabta.Files
{
    public partial class PickupRequests : System.Web.UI.Page
    {
        Cl_Variables clvar = new Cl_Variables();
        String zone = "", branch = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            zone = Session["ZONECODE"].ToString();
            branch = Session["BRANCHCODE"].ToString();
        }
        protected void btn_Save_Staff_Click(object sender, EventArgs e)
        {
            SqlConnection orcl = new SqlConnection(clvar.Strcon());
            DateTime strtDateObj = DateTime.Parse(Fromdate.Text.ToString());
            DateTime endDateObj = DateTime.Parse(Todate.Text.ToString());
            string start = strtDateObj.ToString("yyyy-MM-dd");
            string end = endDateObj.ToString("yyyy-MM-dd");
            String status = "";
            DataTable Ds_1 = new DataTable();
            string query = "";
            try
            {
                query = "SELECT capr.id, \n"
              + "       b.name origin, \n"
              + "       b2.name destination, \n"
              + "       capr.From_Name [ConsignerName], \n"
              + "       capr.To_Name [ConsigneeName], \n"
              + "       capr.From_Address [ConsignerAddress], \n"
              + "       CONVERT(VARCHAR, capr.createdOn, 106) createdOn, \n"
              + "       capr.STATUS \n"
              + "FROM   CustomerApp_PickupRequest capr \n"
              + "INNER JOIN Branches b ON b.branchCode=capr.Origin \n"
              + "INNER JOIN branches b2 ON b2.branchCode=capr.Destination \n"
              + "WHERE  CAST(capr.createdOn AS date) >= CAST('" + start + "' AS date) \n"
              + "       AND CAST(capr.createdOn AS date) <= CAST('" + end + "' AS date)";

                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(Ds_1);
                if (Ds_1.Rows.Count != 0)
                {
                    PickupGrid.DataSource = Ds_1;
                    PickupGrid.DataBind();
                    status_lbl.Text = "";
                }
                else
                {
                    status_lbl.Text = "No record found";
                }
            }
            catch (Exception Err)
            {
                status_lbl.Text = "Error finding record";
            }
            finally
            {
                orcl.Close();
            }
        }

        [WebMethod]
        public static string OpenDetails(string id)
        {
            try
            {

                return "Success";
            }
            catch (Exception er)
            {
                return "";
            }
        }

        protected void ShowDetails_Click(object sender, EventArgs e)
        {
            string PickUpid = ((sender as LinkButton).CommandArgument).ToString();
            DataTable PickUpDetails = new DataTable();
            DataTable Ds_1 = new DataTable();
            SqlConnection orcl = new SqlConnection(clvar.Strcon());
            try
            {
                orcl.Open();
                string query = "SELECT r.firstName+' '+r.lastName+'-'+r.riderCode NAME, r.riderCode " +
                    "  FROM Riders r WHERE r.[status] = 1 AND r.branchId = '" + branch + "'";
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(Ds_1);

                if (Ds_1.Rows.Count != 0)
                {
                    //   DataTable dt = Cities_();
                    this.dd_riderList.DataTextField = "name";
                    this.dd_riderList.DataValueField = "riderCode";
                    this.dd_riderList.DataSource = Ds_1.DefaultView;
                    this.dd_riderList.DataBind();

                }
                else
                {
                    dd_riderList.Items.Clear();
                }

            }
            catch (Exception Err)
            {
            }
            finally
            {
                orcl.Close();
            }

            try
            {
                orcl.Open();
                string query = "SELECT * FROM CustomerApp_PickupRequest capr WHERE capr.Id=" + PickUpid + "";

                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(PickUpDetails);

                if (PickUpDetails.Rows.Count != 0)
                {
                    DetailDiv.Visible = true;
                    lblweight.Text = PickUpDetails.Rows[0]["Weight"].ToString();
                    lblPieces.Text = PickUpDetails.Rows[0]["Pieces"].ToString();
                    lblOrigin.Text = PickUpDetails.Rows[0]["Origin"].ToString();
                    lblDestination.Text = PickUpDetails.Rows[0]["Destination"].ToString();
                    lblServiceType.Text = PickUpDetails.Rows[0]["Service"].ToString();

                    lblFromName.Text = PickUpDetails.Rows[0]["From_Name"].ToString();
                    lblFromPhone.Text = PickUpDetails.Rows[0]["From_Phone_Number"].ToString();
                    lblFromAddress.Text = PickUpDetails.Rows[0]["From_Address"].ToString();

                    lblToName.Text = PickUpDetails.Rows[0]["To_Name"].ToString();
                    lblToPhone.Text = PickUpDetails.Rows[0]["To_Phone_Number"].ToString();
                    lblToAddress.Text = PickUpDetails.Rows[0]["To_Address"].ToString();
                    lblPickUpID.Text = PickUpDetails.Rows[0]["Id"].ToString();

                    Double longitude = Convert.ToDouble(PickUpDetails.Rows[0]["Longitude"]);
                    Double latitude = Convert.ToDouble(PickUpDetails.Rows[0]["Latitude"]);
                    lblLocationCoordinates.Text = "<a href='http://maps.google.com?q=" + latitude + "," + longitude + "' target='_blank' >MAP</a>";
                }
                else
                {
                    status_lbl.Text = "No Details Found";
                }
            }
            catch (Exception Err)
            {
            }
            finally
            {
                orcl.Close();
            }

        }

        protected void ConfirmRequest_Click(object sender, EventArgs e)
        {
            DataTable Ds_1 = new DataTable();
            String PickupId = lblPickUpID.Text;
            SqlConnection orcl = new SqlConnection(clvar.Strcon());
            SqlCommand command = new SqlCommand();
            command.Connection = orcl;

            try
            {
                orcl.Open();
                string sqlInsert = "update CustomerApp_PickupRequest set rider=@rider, status=1 where Id=@pickupID";
                command.CommandText = sqlInsert;
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@rider", dd_riderList.SelectedValue.ToString());
                command.Parameters.AddWithValue("@pickupID", PickupId);
                command.ExecuteNonQuery();
            }
            catch (Exception Err)
            {
            }
            finally
            {
                orcl.Close();
            }
            DetailDiv.Visible = false;
            btn_Save_Staff_Click(sender, e);
        }

        protected void Cancel_Click(object sender, EventArgs e)
        {
            DataTable Ds_1 = new DataTable();
            String PickupId = lblPickUpID.Text;
            SqlConnection orcl = new SqlConnection(clvar.Strcon());
            SqlCommand command = new SqlCommand();
            command.Connection = orcl;

            try
            {
                orcl.Open();
                string sqlInsert = "update CustomerApp_PickupRequest set status=0 where Id=@pickupID";
                command.CommandText = sqlInsert;
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@pickupID", PickupId);
                command.ExecuteNonQuery();
            }
            catch (Exception Err)
            {
            }
            finally
            {
                orcl.Close();
            }
            DetailDiv.Visible = false;
            btn_Save_Staff_Click(sender, e);

        }

        protected void PickupGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                System.Data.DataRow row = ((System.Data.DataRowView)e.Row.DataItem).Row;
                if (row["status"].ToString() == "1")
                {
                    e.Row.BackColor = System.Drawing.Color.Green;
                    e.Row.ForeColor = System.Drawing.Color.White;
                    ((LinkButton)e.Row.FindControl("LinkBtnDetails")).Visible = false;

                }
                else if (row["status"].ToString() == "0")
                {
                    e.Row.BackColor = System.Drawing.Color.Red;
                    e.Row.ForeColor = System.Drawing.Color.White;
                    ((LinkButton)e.Row.FindControl("LinkBtnDetails")).Visible = false;


                }

                //if (row["status"].ToString() == "1"|| row["status"].ToString() == "0")
                //{
                //    ((LinkButton)e.Row.FindControl("LinkBtnDetails")).Visible = false;
                //}

            }
        }
    }
}