using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MRaabta.Files
{
    public partial class PackingMaterialRequest : System.Web.UI.Page
    {
      //  string constr = ConfigurationManager.ConnectionStrings["ConnectionString_LIVE_SERVER_new"].ToString();
        string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        string Mode = "A";

        public string getMSG(string msg)
        {
            string javas = "<script type=\"text/javascript\"> swal(' " + msg + "') </script>";
            return javas;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["PR_MSG"] != null)
            {
                string msg = Session["PR_MSG"].ToString();
                Session["PR_MSG"] = null;

                Page.ClientScript.RegisterStartupScript(this.GetType(), "alertMessage", getMSG(msg));
            }

            if (!IsPostBack)
            {

                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[10] { new DataColumn("SNO"), new DataColumn("Name"), new DataColumn("ItemID"), new DataColumn("Size"), new DataColumn("SizeID"), new DataColumn("RequestQuantity"), new DataColumn("ApprovedQuantity"), new DataColumn("IssuedQuantity"), new DataColumn("TariffItem"), new DataColumn("CompanyCost") });//
                ViewState["PackingMaterialRequest"] = dt;
                this.BindGrid();
                int PID = Convert.ToInt32(Request.QueryString["ID"]);
                loadCombo();
                if (Request.QueryString["Mode"] != null)
                {
                    Mode = Request.QueryString["Mode"].ToString();
                }

                if (Mode == "E")
                {

                  //  grdPR.Columns[8].Visible = false;
                    grdPR.Columns[2].Visible = false;
                    grdPR.Columns[4].Visible = false;
                    txtAccountNumber.Enabled = false;
                    ddlLocation.Enabled = false;
                    ddlNeedPackingMaterialID.Enabled = false;
                    txtQuantity.Enabled = false;
                    btnAddItem.Enabled = false;
                    btnSubmit.Enabled = false;
                    btnReset.Enabled = false;
                    using (SqlConnection con = new SqlConnection(constr))
                    {
                        using (SqlCommand cmd = new SqlCommand(@"select ROW_NUMBER() OVER (ORDER BY PackingRequestDetailID) AS SNO, pm.Name ,
                                                                pms.Size,RequestQuantity,pr.LocationID,pr.CustomerID,pr.Address,
                                                                ISNULL(ApprovedQuantity,'0') ApprovedQuantity,ISNULL(IssuedQuantity,'0') IssuedQuantity ,accountNo,
                                                                R.Rate TariffItem, R.Gst, PRC.COST AS COMPANYCOST
                                                                from PR_PackingRequestDetail as prd
                                                                inner join PR_packing_material as pm on prd.ItemID = pm.ID
                                                                inner join PR_PackingMaterialSize as pms on prd.ItemSize = pms.ID
                                                                inner join PR_PackingRequest as pr on prd.PackingRequestID = pr.PackingRequestID
                                                                inner join  CreditClients C on C.id=CustomerID
                                                                left JOIN PR_PackingMaterialRate R ON PR.CustomerID = R.ClientId
                                                                INNER JOIN PR_COMPANYCOST PRC ON PRC.SIZEID=PMS.ID AND PRC.MATERIALID=PM.ID
                                                                where pr.PackingRequestID = @PackingRequestID"))
                        {
                            using (SqlDataAdapter sda = new SqlDataAdapter())
                            {
                                cmd.Connection = con;
                                cmd.Parameters.AddWithValue("@PackingRequestID", PID);
                                sda.SelectCommand = cmd;
                                //adpt1.SelectCommand.Parameters.AddWithValue("@PackingRequestID", PID.Trim());
                                using (DataTable dt1 = new DataTable())
                                {
                                    sda.Fill(dt1);
                                    grdPR.DataSource = dt1;
                                    grdPR.DataBind();
                                    txtAccountNumber.Text = dt1.Rows[0]["accountNo"].ToString();
                                    CustomerID.Value = dt1.Rows[0]["CustomerID"].ToString();
                                    loadLocation();
                                    txtAddress.Text = dt1.Rows[0]["Address"].ToString();
                                    ddlLocation.SelectedValue = dt1.Rows[0]["LocationID"].ToString();
                                }
                            }
                        }
                    }
                }


                //loadLocation();

            }

        }
        private void loadCombo()
        {
            try
            {


                SqlConnection con = new SqlConnection(constr);
                con.Open();
                string com1 = "select ID, Name from PR_packing_material where id > 1 and rec_STATUS = '1'";
                SqlDataAdapter adpt1 = new SqlDataAdapter(com1, con);
                DataTable dt1 = new DataTable();
                adpt1.Fill(dt1);
                con.Close();
                ddlNeedPackingMaterialID.DataSource = dt1;
                ddlNeedPackingMaterialID.DataBind();
                ddlNeedPackingMaterialID.DataTextField = "Name";
                ddlNeedPackingMaterialID.DataValueField = "ID";
                ddlNeedPackingMaterialID.DataBind();
                ListItem item1 = new ListItem("Select Packing Material", "-1");
                ddlNeedPackingMaterialID.Items.Insert(0, item1);
                dt1.Dispose();
                dt1.Clear();
            }
            catch (Exception)
            {

            }
        }
        private void BindGrid()
        {
            try

            {
                grdPR.DataSource = (DataTable)ViewState["PackingMaterialRequest"];
                grdPR.DataBind();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private string GeneratePackingRequestNo()
        {
            string PackingRequestNo = "";
            DataTable temp = new DataTable();
            SqlConnection orcl = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString());

            string sqlString = @"declare @BatachNo varchar(10)
                                set @BatachNo='0000001'
                                declare @Year varchar(2)
                                set @Year=SUBSTRING( cast(year(GETDATE()) as varchar(10)),3,4)
                                select @BatachNo= (right('0000000'+convert(nvarchar(10),isnull(max(cast(PackingRequestNo as numeric(18,0))),0) +1),7))   
                                from  PR_PackingRequestNoTable where PackingRequestcode = 'MR'
                                group by PackingRequestcode

                                update  PR_PackingRequestNoTable set  PackingRequestNo=@BatachNo ,[Year]=@Year where PackingRequestcode = 'MR'
                                select @Year+'-'+ PackingRequestcode +PackingRequestNo  as PackingRequestGenNo
                                from  PR_PackingRequestNoTable where PackingRequestcode = 'MR'";

            orcl.Open();
            try
            {

                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(temp);
                for (int i = 0; i < temp.Rows.Count; i++)
                {
                    PackingRequestNo = temp.Rows[i]["PackingRequestGenNo"].ToString();
                }

            }
            catch (Exception Err)
            {

            }
            finally
            {
                orcl.Close();
            }
            return PackingRequestNo;
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {

                if (CustomerID.Value.Trim() == "")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "swal('Please select Account NO')", true);
                    return;

                }
                if (ddlLocation.SelectedValue == "")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "swal('Please select Location')", true);
                    return;
                }
                else
                {
                    DataTable dt = (DataTable)ViewState["PackingMaterialRequest"];
                    if (dt.Rows.Count > 0)
                    {
                        int PackingRequestID;
                        SqlConnection con = new SqlConnection(constr);
                        string query = @"INSERT INTO PR_PackingRequest(CustomerID,LocationID,branchcode,Address,RequsestBy,AppStatus,RequestStatus,RequestType,RequestLabel,RequestDate) 
                            VALUES(@CustomerID,@LocationID,@branchcode,@Address,@RequsestBy,@AppStatus,@RequestStatus,@RequestType,@RequestLabel,getdate())
                            SELECT SCOPE_IDENTITY()";
                        SqlCommand cmd = new SqlCommand(query);

                        string PackingRequestNo = GeneratePackingRequestNo();

                        cmd.Parameters.AddWithValue("@CustomerID", CustomerID.Value);
                        cmd.Parameters.AddWithValue("@LocationID", ddlLocation.SelectedValue);
                        cmd.Parameters.AddWithValue("@branchCode", Session["BranchCode"].ToString());
                        cmd.Parameters.AddWithValue("@Address", txtAddress.Text);
                        cmd.Parameters.AddWithValue("@RequsestBy", Convert.ToInt32(Session["u_ID"].ToString()));
                        cmd.Parameters.AddWithValue("@AppStatus", "Waiting For Approval");
                        cmd.Parameters.AddWithValue("@RequestStatus", 1);
                        cmd.Parameters.AddWithValue("@RequestType", "MR");
                        cmd.Parameters.AddWithValue("@RequestLabel", PackingRequestNo);

                        cmd.Connection = con;
                        con.Open();
                        PackingRequestID = Convert.ToInt32(cmd.ExecuteScalar());
                        con.Close();


                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            string query2 = @"insert into PR_PackingRequestDetail (PackingRequestID,ItemID,ItemSize,RequestQuantity) values
                                      (@PackingRequestID,@ItemID,@SizeID,@RequestQuantity ) ";

                            SqlCommand cmd2 = new SqlCommand(query2);

                            cmd2.Parameters.AddWithValue("@PackingRequestID", PackingRequestID);
                            cmd2.Parameters.AddWithValue("@ItemID", dt.Rows[i]["ItemID"].ToString());
                            cmd2.Parameters.AddWithValue("@SizeID", dt.Rows[i]["SizeID"].ToString());
                            cmd2.Parameters.AddWithValue("@RequestQuantity", dt.Rows[i]["RequestQuantity"].ToString());

                            cmd2.Connection = con;
                            con.Open();
                            cmd2.ExecuteNonQuery();
                            con.Close();
                        }
                        Response.Redirect("PackingMaterialRequestlist.aspx?MSG=Your Packing Request ID: " + PackingRequestID + " is Generated for this Customer ID: " + CustomerID.Value + "", true);
                        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "swal('Your Packing Request ID: " + PackingRequestID + " is Generated for this Customer ID: "+ ddlAccountNo.SelectedValue + "')", true);
                        dt.Clear();
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "swal('Please Add Packing Material Item')", true);
                    }
                }
                grdPR.DataBind();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        protected void grdPR_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }
        protected void grdPR_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string item = e.Row.Cells[0].Text;
                foreach (Button button in e.Row.Cells[5].Controls.OfType<Button>())
                {
                    if (button.CommandName == "Delete")
                    {
                        button.Attributes["onclick"] = "if(!confirm('Do you want to delete " + item + "?')){ return false; };";
                    }
                }
            }
        }
        protected void grdPR_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdPR.PageIndex = e.NewPageIndex;
            BindGrid();
        }
        protected void ddlNeedPackingMaterialID_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlNeedPackingMaterialID.SelectedValue != "0")
                {
                    ddlSize.Enabled = true;
                    txtQuantity.Enabled = true;
                    SqlConnection con = new SqlConnection(constr);
                    con.Open();
                    string com = @"select distinct pms.ID,pms.Size from PR_PackingMaterialRate as pr
                                 inner join PR_PackingMaterialSize as pms on pr.SizeID = pms.ID
                                 where pr.rec_status=1 AND pms.rec_status = 1 and MaterialID = @MaterialID";
                    SqlDataAdapter adpt = new SqlDataAdapter(com, con);
                    adpt.SelectCommand.Parameters.AddWithValue("@MaterialID", ddlNeedPackingMaterialID.SelectedValue);

                    DataTable dt = new DataTable();
                    adpt.Fill(dt);
                    con.Close();
                    ddlSize.DataSource = dt;
                    ddlSize.DataBind();
                    ddlSize.DataTextField = "Size";
                    ddlSize.DataValueField = "ID";
                    ddlSize.DataBind();
                    dt.Dispose();
                    dt.Clear();
                }
                else
                {
                    ddlSize.Items.Clear();
                    ddlSize.Enabled = false;
                    txtQuantity.Enabled = false;
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        protected void btnAddItem_Click(object sender, EventArgs e)
        {
            try
            {
                string AccountNo = "", LocationName = "", MaterialID = "", SizeID = "";

                if (ddlNeedPackingMaterialID.SelectedValue.Trim() != "")
                {
                    MaterialID = ddlNeedPackingMaterialID.SelectedValue.Trim();
                }

                if (ddlSize.SelectedValue.Trim() != "")
                {
                    SizeID = ddlSize.SelectedValue.Trim();
                }

                if (ddlLocation.SelectedValue != "")
                {
                    LocationName = ddlLocation.SelectedValue.Trim();
                }

                if (txtAccountNumber.Text.Trim() != "")
                {
                    AccountNo = txtAccountNumber.Text.Trim();
                }

                string[] RateData = GetAccountTariff(AccountNo, LocationName, MaterialID, SizeID);

                DataTable dt = (DataTable)ViewState["PackingMaterialRequest"];

                DataRow[] result = dt.Select("Name='" + ddlNeedPackingMaterialID.SelectedItem.Text + "' and Size='" + ddlSize.SelectedItem.Text + "'");
                foreach (DataRow item in result)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "swal('Please Add another items')", true);
                    return;
                }

                if (txtQuantity.Text != "")
                {
                    dt.Rows.Add(dt.Rows.Count + 1, ddlNeedPackingMaterialID.SelectedItem.Text.Trim(), ddlNeedPackingMaterialID.SelectedItem.Value.Trim(), ddlSize.SelectedItem.Text.Trim(), ddlSize.SelectedItem.Value.Trim(), txtQuantity.Text.Trim(), "","",RateData[0],RateData[1]);
                    grdPR.Columns[2].Visible = false;
                    grdPR.Columns[4].Visible = false;
                    grdPR.Columns[6].Visible = false;
                    grdPR.Columns[7].Visible = false;
                    ViewState["PackingMaterialRequest"] = dt;
                    this.BindGrid();
                    txtQuantity.Text = string.Empty;
                    ddlNeedPackingMaterialID.SelectedValue = "-1";
                    ddlSize.Items.Clear();
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "swal('Please Add Packing Material Quantity')", true);
                }
            }
            catch (Exception EX)
            {
                throw;
            }
        }
        protected void grdPR_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

            int index = Convert.ToInt32(e.RowIndex);
            DataTable dt = ViewState["PackingMaterialRequest"] as DataTable;
            dt.Rows[index].Delete();
            ViewState["PackingMaterialRequest"] = dt;
            BindGrid();
        }
        protected void btnReset_Click(object sender, EventArgs e)
        {
            Response.Redirect("PackingMaterialRequest.aspx", true);
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("PackingMaterialRequestList.aspx", true);
        }
        protected void ddlAccountNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(constr);
            con.Open();
            try
            {
                string com1 = @"select * from CreditClients WHERE accountNo != '' 
                and accountNo=@accountNo and branchCode=@branchCode";
                SqlDataAdapter adpt1 = new SqlDataAdapter(com1, con);
                adpt1.SelectCommand.Parameters.AddWithValue("@accountNo", txtAccountNumber.Text.Trim());
                adpt1.SelectCommand.Parameters.AddWithValue("@branchCode", Session["BranchCode"].ToString());

                DataTable dt1 = new DataTable();
                adpt1.Fill(dt1);
                con.Close();
                if (dt1.Rows.Count > 0)
                {
                    CustomerID.Value = dt1.Rows[0]["id"].ToString();
                    txtAddress.Text = dt1.Rows[0]["address"].ToString();
                    dt1.Dispose();
                    dt1.Clear();
                    if (CustomerID.Value != "")
                    {
                        loadLocation();
                    }
                }
                else
                {
                    Session["PR_MSG"] = "Account does not Exist"; ;
                    Response.Redirect("PackingMaterialRequest.aspx", true);
                }
            }
            catch (Exception ex)
            { }
            finally
            {
                con.Close();
            }

        }
        protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(constr);
            con.Open();
            try
            {
                string com1 = @"SELECT * FROM COD_CustomerLocations c 
                                WHERE CreditClientID = @accountNo
                                AND c.locationid = @locationId
                                AND c.locationName != 'Default'";

                SqlDataAdapter adpt1 = new SqlDataAdapter(com1, con);
                adpt1.SelectCommand.Parameters.AddWithValue("@accountNo", CustomerID.Value);
                adpt1.SelectCommand.Parameters.AddWithValue("@locationId", ddlLocation.SelectedValue);
                DataTable dt1 = new DataTable();
                adpt1.Fill(dt1);
                con.Close();
                if (dt1.Rows.Count > 0)
                {
                    txtAddress.Text = dt1.Rows[0]["locationaddress"].ToString();
                    dt1.Dispose();
                    dt1.Clear();
                }
                else
                {
                    Session["PR_MSG"] = "Account does not Exist";
                    Response.Redirect("PackingMaterialRequest.aspx", true);
                }
            }
            catch (Exception ex)
            { }
            finally
            {
                con.Close();
            }
        }
        void loadLocation()
        {
            SqlConnection con = new SqlConnection(constr);
            con.Open();
            string com_loc = @"select locationName,locationID 
                               from COD_CustomerLocations 
                               where brancahCode =@branchCode 
                               and CreditClientID = @accountNo 
                               AND locationName != 'Default' 
                               AND [status] = '1'";

            SqlDataAdapter adpt_loc = new SqlDataAdapter(com_loc, con);
            adpt_loc.SelectCommand.Parameters.AddWithValue("@accountNo", CustomerID.Value);
            adpt_loc.SelectCommand.Parameters.AddWithValue("@branchCode", Session["BranchCode"].ToString());

            DataTable dt_loc = new DataTable();
            adpt_loc.Fill(dt_loc);
            con.Close();
            if (dt_loc.Rows.Count > 0)
            {
                ddlLocation.DataSource = dt_loc;
                ddlLocation.DataBind();
                ddlLocation.DataTextField = "locationName";
                ddlLocation.DataValueField = "locationID";
                ddlLocation.DataBind();
                dt_loc.Dispose();
                dt_loc.Clear();
            }

            else
            {
                ddlLocation.Items.Clear();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "swal('This Account No Location not available')", true);
            }
        }
        static string[] GetAccountTariff(string AccountNo, string LocationName, string MaterialID, string SizeID)
        {
            string Rate = "", CompanyCost = "";
            string[] arrays = new string[2];

            DataTable temp = new DataTable();
            SqlConnection orcl = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString());

            string sqlString = @"SELECT 
                                    R.Rate, R.Gst, CL.locationName, R.CompanyCost
                                FROM 
                                    COD_CustomerLocations CL
                                    INNER JOIN PR_PackingMaterialRate R ON CL.CreditClientID = R.ClientId
                                WHERE 
                                    CL.accountNo = '" + AccountNo + @"'
                                    AND CL.locationID = '" + LocationName + @"'
                                    AND R.MaterialID = '" + MaterialID + @"'
                                    AND R.SizeID = '" + SizeID + @"'
                                    AND r.rec_status = '1'";

            orcl.Open();
            try
            {
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(temp);

                if (temp.Rows.Count != 0)
                {
                    Rate = temp.Rows[0]["Rate"].ToString();
                }
                else Rate = "";

                sqlString = @" SELECT Cost as CompanyCost FROM pr_companycost WHERE sizeid=" + SizeID + " AND materialID=" + MaterialID + " and STATUS=1";
                orcd = new SqlCommand(sqlString, orcl);
                oda = new SqlDataAdapter(orcd);
                oda.Fill(temp);

                CompanyCost = temp.Rows[0]["CompanyCost"].ToString();

                arrays = new string[2] { Rate, CompanyCost };
            }
            catch (Exception Err)
            {

            }
            finally
            {
                orcl.Close();
            }
            return arrays;
        }
    }
}