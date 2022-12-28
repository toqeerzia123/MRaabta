using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using MRaabta.App_Code;
using System.Security.Cryptography;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;

namespace MRaabta.Files
{
    public partial class WrappingProcess : System.Web.UI.Page
    {

        #region Variables

        Cl_Variables clvar = new Cl_Variables();
        CommonFunction func = new CommonFunction();
        Consignemnts con = new Consignemnts();

        DataTable ConsignmentDetails_ = null;
        DataTable WrappingProcess_ = null;
        DataTable WrappingProcessArchive_ = null;

        bool DataComeFromWrappingProcess = false;
        bool DataComeFromConsignment = false;


        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('BasicInfo');", true);

                txt_ConsignmentNo.Focus();

                PopulateBranches(Get_Branches());
                PopulateServiceType(Get_ServiceTypes());

                //string BranchCode = Session["BRANCHCODE"].ToString();
                //dd_Origin.SelectedIndex = dd_Origin.Items.IndexOf(dd_Origin.Items.FindByValue(BranchCode));
                //dd_Origin.SelectedItem.Value = BranchCode;
            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }

        private void PopulateBranches(DataTable dt)
        {
            DataTable dt_ = dt;
            dd_Origin.Items.Clear();
            dd_Destination.Items.Clear();

            dd_Origin.Items.Add(new ListItem { Text = "Select Origin", Value = "0" });
            dd_Destination.Items.Add(new ListItem { Text = "Select Destination", Value = "0" });

            dd_Origin.DataSource = dt_;
            dd_Destination.DataSource = dt_;
            dd_Origin.DataValueField = "branchcode";
            dd_Destination.DataValueField = "branchcode";
            dd_Origin.DataTextField = "name";
            dd_Destination.DataTextField = "name";

            dd_Origin.DataBind();
            dd_Destination.DataBind();
        }

        public void PopulateServiceType(DataTable dt)
        {
            DataTable dt_ = dt;
            dd_ServiceType.Items.Clear();
            //dd_ServiceType.Items.Add(new ListItem { Text = "Select Service Type", Value = "0" });

            if (dt_.Rows.Count > 0)
            {
                dd_ServiceType.DataSource = dt_;
                dd_ServiceType.DataTextField = "servicetypename";
                dd_ServiceType.DataValueField = "servicetypename";
                dd_ServiceType.DataBind();
            }
        }

        public DataTable Get_Branches()
        {
            string query = " Select branchcode, name from branches where status ='1' order by name asc";

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

        public DataTable Get_ServiceTypes()
        {
            string query = " Select UPPER(servicetypename) AS 'servicetypename' from ServiceTypes_New where status ='1' order by servicetypename asc";

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

        protected void btn_save_Click(object sender, EventArgs e)
        {
            string ConsignmentNo = txt_ConsignmentNo.Text.ToString();
            //string Consignee = txt_Consignee.Text.ToString();
            string Consignee = txt_Consignee.Value.ToString();
            //string Consigner = txt_Consigner.Text.ToString();
            string Consigner = txt_Consigner.Value.ToString();
            string ConsigneeAddress = txt_ConsigneeAddress.Value.ToString();
            string ConsignerAddress = txt_ConsignerAddress.Value.ToString();
            string BookingDate = txt_BookingDate.Text.ToString();
            string OriginValue = dd_Origin.SelectedValue.ToString();
            string OriginName = dd_Origin.SelectedItem.Text.ToString();
            string DestinationValue = dd_Destination.SelectedValue.ToString();
            string DestinationName = dd_Destination.SelectedItem.Text.ToString();
            string Pieces = txt_Pieces.Text.ToString();
            string Weight = txt_Weight.Text.ToString();
            string ServiceType = dd_ServiceType.SelectedItem.Text.ToString();
            string PackageContent = txt_PackageContent.Value.ToString();
            string AccountNo = txt_AccountNo.Text.ToString();
            string WrappingCharges = txt_WrappingAmount.Text.ToString();
            string Comment = txt_Comment.Value.ToString();
            string ConsigneeMobile = txt_ConsigneeMobile.Text.ToString();
            string ConsigneePhoneNo = txt_ConsigneePhoneNo.Text.ToString();
            bool FromWrappingProcess = false;
            bool FromConsignment = false;
            DataTable TempWrappingProcess = null;
            int Record = 0;
            string TempConsignmentNo = string.Empty;

            if (Session["FromWrappingProcess"] != null && Session["FromConsignment"] != null)
            {
                FromWrappingProcess = (bool)Session["FromWrappingProcess"];
                FromConsignment = (bool)Session["FromConsignment"];
            }

            if (ViewState["WrappingProcess_"] != null)
            {
                TempWrappingProcess = (DataTable)ViewState["WrappingProcess_"];

                TempConsignmentNo = TempWrappingProcess.Rows[0]["ConsignmentNumber"].ToString();
            }

            else if (ViewState["WrappingProcess_"] == null)
            {
                TempConsignmentNo = ConsignmentNo;
            }

            if (ConsignmentNo.Length == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter Consignment Number')", true);
                txt_ConsignmentNo.Focus();
                ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('BasicInfo');", true);
                return;
            }

            #region Validation


            if (Consignee.Length <= 120)
            {
                if (Consignee.Contains("~") || Consignee.Contains("!") || Consignee.Contains("@") || Consignee.Contains("$") || Consignee.Contains("%")
                    || Consignee.Contains("^") || Consignee.Contains("\\") || Consignee.Contains("\"") || Consignee.Contains("\'") || Consignee.Contains(";")
                    || Consignee.Contains(":") || Consignee.Contains("?") || Consignee.Contains(">") || Consignee.Contains("<"))
                {
                    lbl_Message.InnerHtml = "Alert: These Characters are found in Consignee Name!<b>   ~   !   @   $   %   ^   \\   \'   \"  ;   :   ?   >   <</b>";
                    txt_Consignee.Focus();
                    ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('BasicInfo');", true);
                    return;
                }
            }
            else if (Consignee.Length > 120)
            {
                lbl_Message.InnerHtml = "Alert: Consignee Name Must Be Within 120 Character!";
                txt_Consignee.Focus();
                ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('BasicInfo');", true);
                return;
            }

            if (Consigner.Length <= 120)
            {
                if (Consigner.Contains("~") || Consigner.Contains("!") || Consigner.Contains("@") || Consigner.Contains("$") || Consigner.Contains("%")
                    || Consigner.Contains("^") || Consigner.Contains("\\") || Consigner.Contains("\"") || Consigner.Contains("\'") || Consigner.Contains(";")
                    || Consigner.Contains(":") || Consigner.Contains("?") || Consigner.Contains(">") || Consigner.Contains("<"))
                {
                    lbl_Message.InnerHtml = "Alert: These Characters are found in Consigner Name!<b>   ~   !   @   $   %   ^   \\   \'   \"  ;   :   ?   >   <</b>";
                    txt_Consigner.Focus();
                    ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('BasicInfo');", true);
                    return;
                }
            }
            else if (Consigner.Length > 120)
            {
                lbl_Message.InnerHtml = "Alert: Consigner Name Must Be Within 120 Character!";
                txt_Consigner.Focus();
                ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('BasicInfo');", true);
                return;
            }

            if (ConsigneeAddress.Length <= 120)
            {
                if (ConsigneeAddress.Contains("~") || ConsigneeAddress.Contains("!") || ConsigneeAddress.Contains("@") || ConsigneeAddress.Contains("$") || ConsigneeAddress.Contains("%")
                    || ConsigneeAddress.Contains("^") || ConsigneeAddress.Contains("\\") || ConsigneeAddress.Contains("\"") || ConsigneeAddress.Contains("\'") || ConsigneeAddress.Contains(";")
                    || ConsigneeAddress.Contains(":") || ConsigneeAddress.Contains("?") || ConsigneeAddress.Contains(">") || ConsigneeAddress.Contains("<"))
                {
                    lbl_Message.InnerHtml = "Alert: These Characters are found in Consignee Address!<b>   ~   !   @   $   %   ^   \\   \'   \"  ;   :   ?   >   <</b>";
                    txt_ConsigneeAddress.Focus();
                    ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('BasicInfo');", true);
                    return;
                }
            }
            else if (ConsigneeAddress.Length > 120)
            {
                lbl_Message.InnerHtml = "Alert: Consignee Address Must Be Within 120 Character!";
                txt_ConsigneeAddress.Focus();
                ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('BasicInfo');", true);
                return;
            }

            if (ConsignerAddress.Length <= 120)
            {
                if (ConsignerAddress.Contains("~") || ConsignerAddress.Contains("!") || ConsignerAddress.Contains("@") || ConsignerAddress.Contains("$") || ConsignerAddress.Contains("%")
                    || ConsignerAddress.Contains("^") || ConsignerAddress.Contains("\\") || ConsignerAddress.Contains("\"") || ConsignerAddress.Contains("\'") || ConsignerAddress.Contains(";")
                    || ConsignerAddress.Contains(":") || ConsignerAddress.Contains("?") || ConsignerAddress.Contains(">") || ConsignerAddress.Contains("<"))
                {
                    lbl_Message.InnerHtml = "Alert: These Characters are found in Consigner Address!<b>   ~   !   @   $   %   ^   \\   \'   \"  ;   :   ?   >   <</b>";
                    txt_ConsignerAddress.Focus();
                    ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('BasicInfo');", true);
                    return;
                }
            }
            else if (ConsignerAddress.Length > 120)
            {
                lbl_Message.InnerHtml = "Alert: Consigner Address Must Be Within 120 Character!";
                txt_ConsignerAddress.Focus();
                ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('BasicInfo');", true);
                return;
            }

            if (PackageContent.Length <= 120)
            {
                if (PackageContent.Contains("~") || PackageContent.Contains("!") || PackageContent.Contains("@") || PackageContent.Contains("$") || PackageContent.Contains("%")
                    || PackageContent.Contains("^") || PackageContent.Contains("\\") || PackageContent.Contains("\"") || PackageContent.Contains("\'") || PackageContent.Contains(";")
                    || PackageContent.Contains(":") || PackageContent.Contains("?") || PackageContent.Contains(">") || PackageContent.Contains("<"))
                {
                    lbl_Message.InnerHtml = "Alert: These Characters are found in Package Content!<b>   ~   !   @   $   %   ^   \\   \'   \"  ;   :   ?   >   <</b>";
                    txt_PackageContent.Focus();
                    ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('BasicInfo');", true);
                    return;
                }
            }
            else if (PackageContent.Length > 120)
            {
                lbl_Message.InnerHtml = "Alert: Package Content Must Be Within 120 Character!";
                txt_PackageContent.Focus();
                ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('BasicInfo');", true);
                return;
            }

            if (Comment.Length <= 120)
            {
                if (Comment.Contains("~") || Comment.Contains("!") || Comment.Contains("@") || Comment.Contains("$") || Comment.Contains("%")
                    || Comment.Contains("^") || Comment.Contains("\\") || Comment.Contains("\"") || Comment.Contains("\'") || Comment.Contains(";")
                    || Comment.Contains(":") || Comment.Contains("?") || Comment.Contains(">") || Comment.Contains("<"))
                {
                    lbl_Message.InnerHtml = "Alert: These Characters are found in Comment!<b>   ~   !   @   $   %   ^   \\   \'   \"  ;   :   ?   >   <</b>";
                    txt_Comment.Focus();
                    ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('BasicInfo');", true);
                    return;
                }
            }
            else if (Comment.Length > 120)
            {
                lbl_Message.InnerHtml = "Alert: Comment Must Be Within 120 Character!";
                txt_Comment.Focus();
                ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('BasicInfo');", true);
                return;
            }

            if (Pieces.Length > 0)
            {
                if (!Pieces.Contains("0") && !Pieces.Contains("1") && !Pieces.Contains("2") && !Pieces.Contains("3") && !Pieces.Contains("4")
                    && !Pieces.Contains("5") && !Pieces.Contains("6") && !Pieces.Contains("7") && !Pieces.Contains("8") && !Pieces.Contains("9"))
                {
                    lbl_Message.InnerHtml = "Alert: Only Numbers Allowed!";
                    txt_Pieces.Focus();
                    ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('BasicInfo');", true);
                    return;
                }
            }

            if (Weight.Length > 0)
            {
                if (!Weight.Contains("0") && !Weight.Contains("1") && !Weight.Contains("2") && !Weight.Contains("3") && !Weight.Contains("4")
                    && !Weight.Contains("5") && !Weight.Contains("6") && !Weight.Contains("7") && !Weight.Contains("8") && !Weight.Contains("9"))
                {
                    if (Weight.Contains("."))
                    {
                    }
                    else if (!Weight.Contains("."))
                    {
                        if (!Weight.Contains("0") && !Weight.Contains("1") && !Weight.Contains("2") && !Weight.Contains("3") && !Weight.Contains("4")
                            && !Weight.Contains("5") && !Weight.Contains("6") && !Weight.Contains("7") && !Weight.Contains("8") && !Weight.Contains("9"))
                        {
                            lbl_Message.InnerHtml = "Alert: Only Numbers Allowed!";
                            txt_Weight.Focus();
                            ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('BasicInfo');", true);
                            return;
                        }
                    }
                }
                if (Weight.EndsWith("."))
                {
                    lbl_Message.InnerHtml = "Alert: Invalid Weight Entered!";
                    txt_Weight.Focus();
                    ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('BasicInfo');", true);
                    return;
                }
            }

            if (WrappingCharges.Length > 0)
            {
                if (!WrappingCharges.Contains("0") && !WrappingCharges.Contains("1") && !WrappingCharges.Contains("2") && !WrappingCharges.Contains("3")
                    && !WrappingCharges.Contains("4") && !WrappingCharges.Contains("5") && !WrappingCharges.Contains("6") && !WrappingCharges.Contains("7")
                    && !WrappingCharges.Contains("8") && !WrappingCharges.Contains("9"))
                {
                    if (WrappingCharges.Contains("."))
                    {
                    }
                    else if (!WrappingCharges.Contains("."))
                    {
                        if (!WrappingCharges.Contains("0") && !WrappingCharges.Contains("1") && !WrappingCharges.Contains("2") && !WrappingCharges.Contains("3")
                            && !WrappingCharges.Contains("4") && !WrappingCharges.Contains("5") && !WrappingCharges.Contains("6") && !WrappingCharges.Contains("7")
                            && !WrappingCharges.Contains("8") && !WrappingCharges.Contains("9"))
                        {
                            lbl_Message.InnerHtml = "Alert: Only Numbers Allowed!";
                            txt_WrappingAmount.Focus();
                            ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('BasicInfo');", true);
                            return;
                        }
                    }
                }

                if (WrappingCharges.EndsWith("."))
                {
                    lbl_Message.InnerHtml = "Alert: Invalid Wrapping Amount Entered!";
                    txt_WrappingAmount.Focus();
                    ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('BasicInfo');", true);
                    return;
                }
            }

            if (BookingDate == "" || BookingDate == null)
            {
                lbl_Message.InnerHtml = "Alert: Booking Date Not Found!";
                txt_BookingDate.Focus();
                ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('BasicInfo');", true);
                return;
            }

            #region Previous Logic


            /*

            if (ConsignmentNo.Length == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter Consignment Number')", true);
                txt_ConsignmentNo.Focus();
                ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('BasicInfo');", true);
                return;
            }

            if (Consignee.Length == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter Consignee')", true);
                txt_Consignee.Focus();
                ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('BasicInfo');", true);
                return;
            }

            if (Consigner.Length == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter Consigner')", true);
                txt_Consigner.Focus();
                ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('BasicInfo');", true);
                return;
            }

            if (BookingDate.Length == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Select Booking Date')", true);
                txt_BookingDate.Focus();
                ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('BasicInfo');", true);
                return;
            }

            if (OriginValue.Equals(0))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Select Origin')", true);
                dd_Origin.Focus();
                ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('BasicInfo');", true);
                return;
            }

            if (DestinationValue.Equals(0))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Select Origin')", true);
                dd_Destination.Focus();
                ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('BasicInfo');", true);
                return;
            }

            if (Pieces.Length == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter Pieces')", true);
                txt_Pieces.Focus();
                ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('BasicInfo');", true);
                return;
            }

            if (Weight.Length == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter Weight')", true);
                txt_Weight.Focus();
                ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('BasicInfo');", true);
                return;
            }

            if (dd_ServiceType.SelectedValue.Equals(0))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Select Service Type')", true);
                dd_ServiceType.Focus();
                ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('BasicInfo');", true);
                return;
            }

            if (PackageContent.Length == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter Package Content')", true);
                txt_PackageContent.Focus();
                ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('BasicInfo');", true);
                return;

            }

            if (AccountNo.Length == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter Account No')", true);
                txt_AccountNo.Focus();
                ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('BasicInfo');", true);
                return;

            }
            */


            #endregion

            #endregion

            if (FromWrappingProcess.Equals(false) && FromConsignment.Equals(true))
            {

                SqlConnection con = new SqlConnection(clvar.Strcon());
                try
                {
                    con.Open();

                    string sql = "INSERT INTO [dbo].[MNP_Wrapping_Process] \n"
               + "           ([ConsignmentNumber] \n"
               + "           ,[Consigner] \n"
               + "           ,[Consignee] \n"
               + "           ,[Orgin] \n"
               + "           ,[Destination] \n"
               + "           ,[Pieces] \n"
               + "           ,[ServiceTypeName] \n"
               + "           ,[Weight] \n"
               + "           ,[PackageContent2] \n"
               + "           ,[ConsignerAccountNo] \n"
               + "           ,[BookingDate] \n"
               + "           ,[WrappingCharges] \n"
               + "           ,[CreatedOn] \n"
               + "           ,[CreatedBy] \n"
               + "           ,[consignee_address] \n"
               + "           ,[shipperaddress] \n"
               + "           ,[usercomment] \n"
               + "           ,[consigneePhoneNo] \n"
               + "           ,[consigneeMobileNo]) \n"
               + "     VALUES \n"
               + "           ('" + TempConsignmentNo + "' \n"
               + "           ,'" + Consigner + "' \n"
               + "           ,'" + Consignee + "' \n"
               + "           ,'" + OriginValue + "' \n"
               + "           ,'" + DestinationValue + "' \n"
               + "           ,'" + Pieces + "' \n"
               + "           ,'" + ServiceType + "' \n"
               + "           ,'" + Weight + "' \n"
               + "           ,'" + PackageContent + "' \n"
               + "           ,'" + AccountNo + "' \n"
               + "           ,'" + BookingDate + "' \n"
               + "           ,'" + WrappingCharges + "' \n"
               + "           ,GETDATE() \n"
               + "           ,'" + HttpContext.Current.Session["U_ID"].ToString() + "' \n"
               + "           ,'" + ConsigneeAddress + "' \n"
               + "           ,'" + ConsignerAddress + "' \n"
               + "           ,'" + Comment + "' \n"
               + "           ,'" + ConsigneePhoneNo + "' \n"
               + "           ,'" + ConsigneeMobile + "' \n)";

                    SqlCommand orcd = new SqlCommand(sql, con);
                    orcd.CommandType = CommandType.Text;
                    Record = orcd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
                    ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('BasicInfo');", true);

                }
                finally { con.Close(); }
            }


            else if (FromWrappingProcess.Equals(true) && FromConsignment.Equals(false))
            {

                SqlConnection con = new SqlConnection(clvar.Strcon());

                // Insert Data First in MNP_Wrapping_Process_Archive for Logging

                try
                {
                    con.Open();

                    string sql = "INSERT INTO [dbo].[MNP_Wrapping_Process_Archive] \n"
               + "           ([ConsignmentNumber] \n"
               + "           ,[Consigner] \n"
               + "           ,[Consignee] \n"
               + "           ,[Orgin] \n"
               + "           ,[Destination] \n"
               + "           ,[Pieces] \n"
               + "           ,[ServiceTypeName] \n"
               + "           ,[Weight] \n"
               + "           ,[PackageContent2] \n"
               + "           ,[ConsignerAccountNo] \n"
               + "           ,[BookingDate] \n"
               + "           ,[WrappingCharges] \n"
               + "           ,[CreatedOn] \n"
               + "           ,[CreatedBy] \n"
               + "           ,[consignee_address] \n"
               + "           ,[shipperaddress] \n"
               + "           ,[usercomment] \n"
               + "           ,[consigneePhoneNo] \n"
               + "           ,[consigneeMobileNo]) \n"
               + "     VALUES \n"
               + "           ('" + TempConsignmentNo + "' \n"
               + "           ,'" + TempWrappingProcess.Rows[0]["Consigner"].ToString() + "' \n"
               + "           ,'" + TempWrappingProcess.Rows[0]["Consignee"].ToString() + "' \n"
               + "           ,'" + TempWrappingProcess.Rows[0]["Origin"] + "' \n"
               + "           ,'" + TempWrappingProcess.Rows[0]["Destination"] + "' \n"
               + "           ,'" + TempWrappingProcess.Rows[0]["Pieces"] + "' \n"
               + "           ,'" + TempWrappingProcess.Rows[0]["ServiceTypeName"] + "' \n"
               + "           ,'" + TempWrappingProcess.Rows[0]["Weight"] + "' \n"
               + "           ,'" + TempWrappingProcess.Rows[0]["PackageContent"] + "' \n"
               + "           ,'" + TempWrappingProcess.Rows[0]["AccountNo"] + "' \n"
               + "           ,'" + TempWrappingProcess.Rows[0]["BookingDate"] + "' \n"
               + "           ,'" + TempWrappingProcess.Rows[0]["WrappingCharges"] + "' \n"
               + "           ,GETDATE() \n"
               + "           ,'" + HttpContext.Current.Session["U_ID"].ToString() + "' \n"
               + "           ,'" + ConsigneeAddress + "' \n"
               + "           ,'" + ConsignerAddress + "' \n"
               + "           ,'" + Comment + "' \n"
               + "           ,'" + ConsigneePhoneNo + "' \n"
               + "           ,'" + ConsigneeMobile + "' \n)";

                    SqlCommand orcd = new SqlCommand(sql, con);
                    orcd.CommandType = CommandType.Text;
                    Record = orcd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
                    ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('BasicInfo');", true);

                }
                finally { con.Close(); }

                // Now Update Data of MNP_Wrapping_Process

                try
                {
                    con.Open();

                    string sql = "UPDATE [dbo].[MNP_Wrapping_Process] \n"
                               + "   SET [ConsignmentNumber] = '" + ConsignmentNo + "' \n"
                               + "      ,[Consigner] = '" + Consigner + "' \n"
                               + "      ,[Consignee] = '" + Consignee + "' \n"
                               + "      ,[Orgin] = '" + OriginValue + "' \n"
                               + "      ,[Destination] = '" + DestinationValue + "' \n"
                               + "      ,[Pieces] = '" + Pieces + "' \n"
                               + "      ,[ServiceTypeName] = '" + ServiceType + "' \n"
                               + "      ,[Weight] = '" + Weight + "' \n"
                               + "      ,[PackageContent2] = '" + PackageContent + "' \n"
                               + "      ,[ConsignerAccountNo] = '" + AccountNo + "' \n"
                               + "      ,[BookingDate] = '" + BookingDate + "' \n"
                               + "      ,[WrappingCharges] = '" + WrappingCharges + "' \n"
                               + "      ,[CreatedOn] = GETDATE() \n"
                               + "      ,[CreatedBy] = '" + HttpContext.Current.Session["U_ID"].ToString() + "' \n"
                               + "      ,[consignee_address] = '" + ConsigneeAddress + "' \n"
                               + "      ,[shipperaddress] = '" + ConsignerAddress + "' \n"
                               + "      ,[usercomment] = '" + Comment + "' \n"
                               + "      ,[consigneePhoneNo] = '" + ConsigneePhoneNo + "' \n"
                               + "      ,[consigneeMobileNo] = '" + ConsigneeMobile + "' \n"
                               + " WHERE [ConsignmentNumber] = '" + TempConsignmentNo + "'";

                    SqlCommand orcd = new SqlCommand(sql, con);
                    orcd.CommandType = CommandType.Text;
                    Record = orcd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
                    ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('BasicInfo');", true);

                }
                finally
                {
                    con.Close();
                }

            }


            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record have been saved')", true);

            #region Previous Logic


            /*
            if (Record > 0)
            {
                //string CN = Encrypt_QueryString(ConsignmentNo);
                string CN = Encrypt_QueryString(TempConsignmentNo);
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "OpenWindow", "window.open('WrappingProcess_Print.aspx?ConsignmentNo=" + CN + "','Wrapping Process Print','menubar=1,resizable=1,width=900,height=600');", true);
                //Response.Redirect("WrappingProcess_Print.aspx?ConsignmentNo=" + CN + "", false);
            }
            */


            #endregion

            //Clear Fields

            lbl_Message.InnerHtml = "";

            ViewState["WrappingProcess_"] = null;

            ConsignmentDetails_ = null;
            WrappingProcess_ = null;

            txt_ConsignmentNo.Text = null;
            //txt_Consignee.Text = null;
            //txt_Consigner.Text = null;
            txt_Consignee.Value = null;
            txt_Consigner.Value = null;
            txt_ConsigneeAddress.Value = null;
            txt_ConsignerAddress.Value = null;
            txt_BookingDate.Text = null;
            dd_Origin.SelectedValue = "1";
            dd_Destination.SelectedValue = "1";
            txt_Pieces.Text = null;
            txt_Weight.Text = null;
            dd_ServiceType.SelectedValue = "LAPTOP";
            txt_PackageContent.Value = null;
            txt_AccountNo.Text = null;
            txt_WrappingAmount.Text = null;
            txt_Comment.Value = null;
            txt_ConsigneeMobile.Text = null;
            txt_ConsigneePhoneNo.Text = null;

            DataComeFromWrappingProcess = false;
            Session["FromWrappingProcess"] = null;

            DataComeFromConsignment = false;
            Session["FromConsignment"] = null;

            btn_reset.Visible = false;
            btn_save.Visible = false;
            btn_print.Visible = false;

            txt_ConsignmentNo.Enabled = true;

            Record = 0;
            TempConsignmentNo = string.Empty;

            ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('BasicInfo');", true);

        }

        public static string Encrypt_QueryString(string str)
        {
            string EncrptKey = "2013;[pnuLIT)WebCodeExpert";
            byte[] byKey = { };
            byte[] IV = { 18, 52, 86, 120, 144, 171, 205, 239 };
            byKey = System.Text.Encoding.UTF8.GetBytes(EncrptKey.Substring(0, 8));
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = Encoding.UTF8.GetBytes(str);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(byKey, IV), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return Convert.ToBase64String(ms.ToArray());
        }

        public string Conditional_Query_WrappingProcess()
        {
            string query = "";

            string ConsignmentNo = txt_ConsignmentNo.Text.ToString();

            if (ConsignmentNo == "")
            {
                query += " \n";
                return query;
            }

            if (ConsignmentNo != "")
            {
                query += "ConsignmentNumber = '" + ConsignmentNo + "' and \n";
                return query;
            }

            return query;

        }

        public DataTable Get_ConsignmentDetails()
        {

            string ConsignmentNo = txt_ConsignmentNo.Text.ToString();
            DataTable dt = new DataTable();

            if (ConsignmentNo == "")
            {
                return dt;
            }

            string query = string.Empty;

            if (ConsignmentNo != "")
            {
                query = "SELECT c.ConsignmentNumber AS 'ConsignmentNumber', \n"
                 + "       c.consignee           AS 'Consignee', \n"
                 + "       [consigneePhoneNo] AS 'ConsigneePhoneNo', \n"
                 + "       c.consigner           AS 'Consigner', \n"
                 + "       c.[address]           AS 'ConsigneeAddress', \n"
                 + "       c.shipperAddress      AS 'ConsignerAddress', \n"
                 + "       LEFT(CONVERT(VARCHAR(30), c.bookingDate, 120), 10) AS 'BookingDate', \n"
                 + "       c.orgin               AS 'Origin', \n"
                 + "       c.destination         AS 'Destination', \n"
                 + "       c.pieces              AS 'Pieces', \n"
                 + "       c.[weight]            AS 'Weight', \n"
                 + "       UPPER(c.serviceTypeName)     AS 'ServiceTypeName', \n"
                 + "       c.PakageContents      AS 'PackageContent', \n"
                 + "       c.consignerAccountNo  AS 'AccountNo', \n"
                 + "       LEFT(CONVERT(VARCHAR(30), c.createdOn, 120), 10) AS 'CreatedOn', \n"
                 + "       c.createdBy           AS 'CreatedBy', \n"
                 + "       LEFT(CONVERT(VARCHAR(30), c.modifiedOn, 120), 10) AS 'ModifiedOn', \n"
                 + "       c.modifiedBy          AS 'ModifiedBy' \n"
                 + "FROM   Consignment              c \n"
                 + "WHERE \n"
                 + "c.ConsignmentNumber = '" + ConsignmentNo + "'";
                //+ "AND c.Orgin = '" + Session["BRANCHCODE"].ToString() + "'";
            }

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

        public DataTable Get_WrappingProcess()
        {

            string ConsignmentNo = txt_ConsignmentNo.Text.ToString();
            DataTable dt = new DataTable();

            if (ConsignmentNo == "")
            {
                return dt;
            }

            string query = string.Empty;
            if (ConsignmentNo != "")
            {

                query = "SELECT [ConsignmentNumber] AS 'ConsignmentNumber', \n"
                   + "       [Consigner] AS 'Consigner', \n"
                   + "       [Consignee] AS 'Consignee', \n"
                   + "       [consigneePhoneNo] AS 'ConsigneePhoneNo', \n"
                   + "       [consigneeMobileNo] AS 'ConsigneeMobile', \n"
                   + "       [consignee_address]           AS 'ConsigneeAddress', \n"
                   + "       [shipperAddress]      AS 'ConsignerAddress', \n"
                   + "       [Orgin] AS 'Origin', \n"
                   + "       [Destination] AS 'Destination', \n"
                   + "       [Pieces] AS 'Pieces', \n"
                   + "       UPPER([ServiceTypeName]) AS 'ServiceTypeName', \n"
                   + "       [Weight] AS 'Weight', \n"
                   + "       [PackageContent2] AS 'PackageContent', \n"
                   + "       [ConsignerAccountNo] AS 'AccountNo', \n"
                   + "       LEFT(CONVERT(VARCHAR(30), [BookingDate], 120), 10) AS 'BookingDate', \n"
                   + "       [WrappingCharges] AS 'WrappingCharges', \n"
                   + "       LEFT(CONVERT(VARCHAR(30), [CreatedOn], 120), 10) AS 'CreatedOn', \n"
                   + "       [CreatedBy] AS 'CreatedBy', \n"
                   + "       LEFT(CONVERT(VARCHAR(30), [ModifyOn], 120), 10) AS 'ModifyOn', \n"
                   + "       [ModifyBy] AS 'ModifyBy', \n"
                   + "       [usercomment]      AS 'Comment' \n"
                   + "FROM   [APL_BTS].[dbo].[MNP_Wrapping_Process] \n"
                   + "WHERE \n"
                   + "ConsignmentNumber = '" + ConsignmentNo + "'";
                //+ "AND Orgin = '" + Session["BRANCHCODE"].ToString() + "'";
            }
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

        protected void txt_ConsignmentNo_TextChanged(object sender, EventArgs e)
        {
            TextBox ConsignmentNo = txt_ConsignmentNo;
            //TextBox Consignee = txt_Consignee;
            //TextBox Consigner = txt_Consigner;
            TextBox BookingDate = txt_BookingDate;
            DropDownList Origin = dd_Origin;
            DropDownList Destination = dd_Destination;
            TextBox Pieces = txt_Pieces;
            TextBox Weight = txt_Weight;
            DropDownList ServiceType = dd_ServiceType;
            TextBox AccountNo = txt_AccountNo;
            TextBox WrappingCharges = txt_WrappingAmount;
            TextBox ConsigneeMobile = txt_ConsigneeMobile;
            TextBox ConsigneePhoneNo = txt_ConsigneePhoneNo;

            Button Reset = btn_reset;
            Button Save = btn_save;
            Button Print = btn_print;

            #region When Consignment Number is NOT NULL

            if (ConsignmentNo.Text != "")
            {
                #region If CN Contain Only Numbers Or NOT


                Regex Number_Regex = new Regex("^[0-9]*$");

                bool Consignment_IsMatched = Number_Regex.IsMatch(ConsignmentNo.Text.ToString());

                if (Consignment_IsMatched)
                {
                    #region Check For CN in Wrapping Process

                    WrappingProcess_ = Get_WrappingProcess();

                    if (WrappingProcess_.Rows.Count > 0)
                    {

                        #region Previous Code

                        //WrappingProcess_.Rows[0]["ConsignmentNo"] = Get_WrappingProcess().Rows[0]["ConsignmentNumber"];
                        //WrappingProcess_.Rows[0]["Consignee"] = Get_WrappingProcess().Rows[0]["Consigner"];
                        //WrappingProcess_.Rows[0]["Consigner"] = Get_WrappingProcess().Rows[0]["Consignee"];
                        //WrappingProcess_.Rows[0]["Origin"] = Get_WrappingProcess().Rows[0]["Orgin"];
                        //WrappingProcess_.Rows[0]["Destination"] = Get_WrappingProcess().Rows[0]["Destination"];
                        //WrappingProcess_.Rows[0]["Pieces"] = Get_WrappingProcess().Rows[0]["Pieces"];
                        //WrappingProcess_.Rows[0]["ServiceType"] = Get_WrappingProcess().Rows[0]["ServiceTypeName"];
                        //WrappingProcess_.Rows[0]["Weight"] = Get_WrappingProcess().Rows[0]["Weight"];
                        //WrappingProcess_.Rows[0]["PackageContent"] = Get_WrappingProcess().Rows[0]["PackageContent"];
                        //WrappingProcess_.Rows[0]["AccountNo"] = Get_WrappingProcess().Rows[0]["AccountNo"];
                        //WrappingProcess_.Rows[0]["BookingDate"] = Get_WrappingProcess().Rows[0]["BookingDate"];
                        //WrappingProcess_.Rows[0]["WrappingChanges"] = Get_WrappingProcess().Rows[0]["WrappingCharges"];
                        //WrappingProcess_.Rows[0]["CreatedOn"] = Get_WrappingProcess().Rows[0]["CreatedOn"];
                        //WrappingProcess_.Rows[0]["CreatedBy"] = Get_WrappingProcess().Rows[0]["CreatedBy"];
                        //WrappingProcess_.Rows[0]["ModifiedOn"] = Get_WrappingProcess().Rows[0]["ModifyOn"];
                        //WrappingProcess_.Rows[0]["ModifiedBy"] = Get_WrappingProcess().Rows[0]["ModifyBy"];

                        #endregion

                        //Consigner.Text = WrappingProcess_.Rows[0]["Consigner"].ToString();
                        //Consignee.Text = WrappingProcess_.Rows[0]["Consignee"].ToString();
                        txt_Consigner.Value = WrappingProcess_.Rows[0]["Consigner"].ToString();
                        txt_Consignee.Value = WrappingProcess_.Rows[0]["Consignee"].ToString();
                        ConsigneeMobile.Text = WrappingProcess_.Rows[0]["ConsigneeMobile"].ToString();
                        ConsigneePhoneNo.Text = WrappingProcess_.Rows[0]["ConsigneePhoneNo"].ToString();
                        txt_ConsigneeAddress.Value = WrappingProcess_.Rows[0]["ConsigneeAddress"].ToString();
                        txt_ConsignerAddress.Value = WrappingProcess_.Rows[0]["ConsignerAddress"].ToString();
                        Origin.SelectedValue = WrappingProcess_.Rows[0]["Origin"].ToString();
                        Destination.SelectedValue = WrappingProcess_.Rows[0]["Destination"].ToString();
                        Pieces.Text = WrappingProcess_.Rows[0]["Pieces"].ToString();
                        ServiceType.SelectedValue = WrappingProcess_.Rows[0]["ServiceTypeName"].ToString();
                        Weight.Text = WrappingProcess_.Rows[0]["Weight"].ToString();
                        txt_PackageContent.Value = WrappingProcess_.Rows[0]["PackageContent"].ToString();
                        AccountNo.Text = WrappingProcess_.Rows[0]["AccountNo"].ToString();
                        BookingDate.Text = WrappingProcess_.Rows[0]["BookingDate"].ToString();
                        WrappingCharges.Text = WrappingProcess_.Rows[0]["WrappingCharges"].ToString();
                        txt_Comment.Value = WrappingProcess_.Rows[0]["Comment"].ToString();

                        ViewState["WrappingProcess_"] = WrappingProcess_;

                        DataComeFromWrappingProcess = true;
                        Session["FromWrappingProcess"] = DataComeFromWrappingProcess;

                        DataComeFromConsignment = false;
                        Session["FromConsignment"] = DataComeFromConsignment;

                        Reset.Visible = true;
                        Save.Visible = true;
                        Print.Visible = true;

                        txt_ConsignmentNo.Enabled = false;

                        return;
                    }

                    #region Previous Code

                    //if (WrappingProcess_.Rows.Count > 0)
                    //{
                    //    Consignee.Text = WrappingProcess_.Rows[0]["Consignee"].ToString();
                    //    Consigner.Text = WrappingProcess_.Rows[0]["Consigner"].ToString();
                    //    BookingDate.Text = WrappingProcess_.Rows[0]["BookingDate"].ToString();
                    //    Origin.SelectedValue = WrappingProcess_.Rows[0]["Origin"].ToString();
                    //    Destination.SelectedValue = WrappingProcess_.Rows[0]["Destination"].ToString();
                    //    Pieces.Text = WrappingProcess_.Rows[0]["Pieces"].ToString();
                    //    Weight.Text = WrappingProcess_.Rows[0]["Weight"].ToString();
                    //    ServiceType.SelectedValue = WrappingProcess_.Rows[0]["ServiceType"].ToString();
                    //    txt_PackageContent.Value = WrappingProcess_.Rows[0]["PackageContent"].ToString();
                    //    AccountNo.Text = WrappingProcess_.Rows[0]["AccountNo"].ToString();
                    //    WrappingCharges.Text = WrappingProcess_.Rows[0]["WrappingChanges"].ToString();
                    //    DataComeFromWrappingProcess = true;
                    //    DataComeFromConsignment = false;
                    //    return;
                    //}

                    #endregion

                    #endregion

                    #region Check For CN in Consignment

                    ConsignmentDetails_ = Get_ConsignmentDetails();

                    if (ConsignmentDetails_.Rows.Count > 0)
                    {
                        #region Previous Code

                        //ConsignmentDetails_.Rows[0]["ConsignmentNo"] = Get_WrappingProcess().Rows[0]["ConsignmentNumber"];
                        //ConsignmentDetails_.Rows[0]["Consignee"] = Get_WrappingProcess().Rows[0]["Consignee"];
                        //ConsignmentDetails_.Rows[0]["Consigner"] = Get_WrappingProcess().Rows[0]["Consigner"];
                        //ConsignmentDetails_.Rows[0]["Origin"] = Get_WrappingProcess().Rows[0]["Orgin"];
                        //ConsignmentDetails_.Rows[0]["Destination"] = Get_WrappingProcess().Rows[0]["Destination"];
                        //ConsignmentDetails_.Rows[0]["Pieces"] = Get_WrappingProcess().Rows[0]["Pieces"];
                        //ConsignmentDetails_.Rows[0]["ServiceType"] = Get_WrappingProcess().Rows[0]["ServiceTypeName"];
                        //ConsignmentDetails_.Rows[0]["Weight"] = Get_WrappingProcess().Rows[0]["Weight"];
                        //ConsignmentDetails_.Rows[0]["PackageContent"] = Get_WrappingProcess().Rows[0]["PackageContent"];
                        //ConsignmentDetails_.Rows[0]["AccountNo"] = Get_WrappingProcess().Rows[0]["AccountNo"];
                        //ConsignmentDetails_.Rows[0]["BookingDate"] = Get_WrappingProcess().Rows[0]["BookingDate"];
                        //ConsignmentDetails_.Rows[0]["WrappingChanges"] = Get_WrappingProcess().Rows[0]["WrappingCharges"];
                        //ConsignmentDetails_.Rows[0]["CreatedOn"] = Get_WrappingProcess().Rows[0]["CreatedOn"];
                        //ConsignmentDetails_.Rows[0]["CreatedBy"] = Get_WrappingProcess().Rows[0]["CreatedBy"];
                        //ConsignmentDetails_.Rows[0]["ModifiedOn"] = Get_WrappingProcess().Rows[0]["ModifiedOn"];
                        //ConsignmentDetails_.Rows[0]["ModifiedBy"] = Get_WrappingProcess().Rows[0]["ModifiedBy"];


                        #endregion


                        //Consignee.Text = ConsignmentDetails_.Rows[0]["Consignee"].ToString();
                        txt_Consignee.Value = ConsignmentDetails_.Rows[0]["Consignee"].ToString();
                        ConsigneePhoneNo.Text = ConsignmentDetails_.Rows[0]["ConsigneePhoneNo"].ToString();
                        //Consigner.Text = ConsignmentDetails_.Rows[0]["Consigner"].ToString();
                        txt_Consigner.Value = ConsignmentDetails_.Rows[0]["Consigner"].ToString();
                        txt_ConsigneeAddress.Value = ConsignmentDetails_.Rows[0]["ConsigneeAddress"].ToString();
                        txt_ConsignerAddress.Value = ConsignmentDetails_.Rows[0]["ConsignerAddress"].ToString();
                        Origin.SelectedValue = ConsignmentDetails_.Rows[0]["Origin"].ToString();
                        Destination.SelectedValue = ConsignmentDetails_.Rows[0]["Destination"].ToString();
                        Pieces.Text = ConsignmentDetails_.Rows[0]["Pieces"].ToString();
                        ServiceType.SelectedValue = ConsignmentDetails_.Rows[0]["ServiceTypeName"].ToString();
                        Weight.Text = ConsignmentDetails_.Rows[0]["Weight"].ToString();
                        txt_PackageContent.Value = ConsignmentDetails_.Rows[0]["PackageContent"].ToString();
                        AccountNo.Text = ConsignmentDetails_.Rows[0]["AccountNo"].ToString();
                        BookingDate.Text = ConsignmentDetails_.Rows[0]["BookingDate"].ToString();
                        txt_Comment.Value = null;

                        DataComeFromWrappingProcess = false;
                        Session["FromWrappingProcess"] = DataComeFromWrappingProcess;

                        DataComeFromConsignment = true;
                        Session["FromConsignment"] = DataComeFromConsignment;

                        Reset.Visible = true;
                        Save.Visible = true;
                        Print.Visible = true;

                        txt_ConsignmentNo.Enabled = false;

                        return;
                    }

                    #region Previous Code

                    //if (ConsignmentDetails_.Rows.Count > 0)
                    //{
                    //    Consignee.Text = ConsignmentDetails_.Rows[0]["Consignee"].ToString();
                    //    Consigner.Text = ConsignmentDetails_.Rows[0]["Consigner"].ToString();
                    //    BookingDate.Text = ConsignmentDetails_.Rows[0]["BookingDate"].ToString();
                    //    Origin.SelectedValue = ConsignmentDetails_.Rows[0]["Origin"].ToString();
                    //    Destination.SelectedValue = ConsignmentDetails_.Rows[0]["Destination"].ToString();
                    //    Pieces.Text = ConsignmentDetails_.Rows[0]["Pieces"].ToString();
                    //    Weight.Text = ConsignmentDetails_.Rows[0]["Weight"].ToString();
                    //    ServiceType.SelectedValue = ConsignmentDetails_.Rows[0]["ServiceType"].ToString();
                    //    txt_PackageContent.Value = WrappingProcess_.Rows[0]["PackageContent"].ToString();
                    //    AccountNo.Text = ConsignmentDetails_.Rows[0]["AccountNo"].ToString();
                    //    DataComeFromWrappingProcess = false;
                    //    DataComeFromConsignment = true;
                    //    return;
                    //}

                    #endregion

                    #endregion

                    #region If CN is in both Tables But Invalid

                    /*

                if (WrappingProcess_.Rows.Count > 0 || ConsignmentDetails_.Rows.Count > 0)
                {
                    if (WrappingProcess_.Rows[0]["ConsignmentNumber"].ToString() != "" || ConsignmentDetails_.Rows[0]["ConsignmentNumber"].ToString() != "")
                    {
                        if ((ConsignmentNo.Text != WrappingProcess_.Rows[0]["ConsignmentNumber"].ToString())
                            || (ConsignmentNo.Text != ConsignmentDetails_.Rows[0]["ConsignmentNumber"].ToString()))
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('Consignment Number is NOT Valid!')", true);

                            ViewState["WrappingProcess_"] = null;

                            ConsignmentDetails_ = null;
                            WrappingProcess_ = null;

                            DataComeFromWrappingProcess = false;
                            Session["FromWrappingProcess"] = null;

                            DataComeFromConsignment = false;
                            Session["FromConsignment"] = null;

                            txt_ConsignmentNo.Enabled = true;

                            return;
                        }
                    }
                }

                */

                    #endregion

                    #region If CN is NOT in both Tables



                    if (WrappingProcess_.Rows.Count == 0 && ConsignmentDetails_.Rows.Count == 0)
                    {
                        //  Show Error Here

                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('No Record Found; Please Enter New Record!')", true);

                        ViewState["WrappingProcess_"] = null;

                        ConsignmentDetails_ = null;
                        WrappingProcess_ = null;

                        //Consignee.Text = null;
                        //Consigner.Text = null;
                        txt_Consignee.Value = null;
                        txt_Consigner.Value = null;
                        txt_ConsigneeAddress.Value = null;
                        txt_ConsignerAddress.Value = null;
                        BookingDate.Text = null;
                        Origin.SelectedValue = "0";
                        Destination.SelectedValue = "0";
                        Pieces.Text = null;
                        Weight.Text = null;
                        ServiceType.SelectedValue = "AVIATION SALE";
                        txt_PackageContent.Value = null;
                        AccountNo.Text = null;
                        WrappingCharges.Text = null;
                        txt_Comment.Value = null;
                        ConsigneeMobile.Text = null;
                        ConsigneePhoneNo.Text = null;


                        DataComeFromWrappingProcess = false;
                        Session["FromWrappingProcess"] = null;

                        DataComeFromConsignment = false;
                        Session["FromConsignment"] = null;

                        Reset.Visible = true;
                        Save.Visible = true;
                        Print.Visible = true;

                        ConsignmentNo.Enabled = true;

                        ConsignmentNo.Focus();

                        return;
                    }



                    #endregion
                }

                else if (!Consignment_IsMatched)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Consignment Number Must Contain Numbers Only!')", true);

                    ViewState["WrappingProcess_"] = null;

                    ConsignmentDetails_ = null;
                    WrappingProcess_ = null;

                    ConsignmentNo.Text = null;
                    //Consignee.Text = null;
                    //Consigner.Text = null;
                    txt_Consignee.Value = null;
                    txt_Consigner.Value = null;
                    txt_ConsigneeAddress.Value = null;
                    txt_ConsignerAddress.Value = null;
                    BookingDate.Text = null;
                    Origin.SelectedValue = "0";
                    Destination.SelectedValue = "0";
                    Pieces.Text = null;
                    Weight.Text = null;
                    ServiceType.SelectedValue = "AVIATION SALE";
                    txt_PackageContent.Value = null;
                    AccountNo.Text = null;
                    WrappingCharges.Text = null;
                    txt_Comment.Value = null;
                    ConsigneeMobile.Text = null;
                    ConsigneePhoneNo.Text = null;


                    DataComeFromWrappingProcess = false;
                    Session["FromWrappingProcess"] = null;

                    DataComeFromConsignment = false;
                    Session["FromConsignment"] = null;

                    Reset.Visible = false;
                    Save.Visible = false;
                    Print.Visible = false;

                    ConsignmentNo.Enabled = true;

                    ConsignmentNo.Focus();

                    return;
                }


                #endregion
            }

            #endregion

            #region When Consignment Number is NULL

            else if (ConsignmentNo.Text == "")
            {
                //  Show Error Here

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Consignment Number Required!')", true);

                ViewState["WrappingProcess_"] = null;

                ConsignmentDetails_ = null;
                WrappingProcess_ = null;

                ConsignmentNo.Text = null;
                //Consignee.Text = null;
                //Consigner.Text = null;
                txt_Consignee.Value = null;
                txt_Consigner.Value = null;
                txt_ConsigneeAddress.Value = null;
                txt_ConsignerAddress.Value = null;
                BookingDate.Text = null;
                Origin.SelectedValue = "0";
                Destination.SelectedValue = "0";
                Pieces.Text = null;
                Weight.Text = null;
                ServiceType.SelectedValue = "AVIATION SALE";
                txt_PackageContent.Value = null;
                AccountNo.Text = null;
                WrappingCharges.Text = null;
                txt_Comment.Value = null;
                ConsigneeMobile.Text = null;

                DataComeFromWrappingProcess = false;
                Session["FromWrappingProcess"] = null;

                DataComeFromConsignment = false;
                Session["FromConsignment"] = null;

                Reset.Visible = false;
                Save.Visible = false;
                Print.Visible = false;

                ConsignmentNo.Enabled = true;

                ConsignmentNo.Focus();

                return;
            }

            #endregion

        }

        protected void btn_print_Click(object sender, EventArgs e)
        {
            string ConsignmentNo = txt_ConsignmentNo.Text.ToString();
            //string Consignee = txt_Consignee.Text.ToString();
            string Consignee = txt_Consignee.Value.ToString();
            //string Consigner = txt_Consigner.Text.ToString();
            string Consigner = txt_Consigner.Value.ToString();
            string ConsigneeAddress = txt_ConsigneeAddress.Value.ToString();
            string ConsignerAddress = txt_ConsignerAddress.Value.ToString();
            string BookingDate = txt_BookingDate.Text.ToString();
            string OriginName = dd_Origin.SelectedItem.Text.ToString();
            string DestinationName = dd_Destination.SelectedItem.Text.ToString();
            string Pieces = txt_Pieces.Text.ToString();
            string Weight = txt_Weight.Text.ToString();
            string ServiceType = dd_ServiceType.SelectedItem.Text.ToString();
            string PackageContent = txt_PackageContent.Value.ToString();
            //string AccountNo = txt_AccountNo.Text.ToString();
            string WrappingCharges = txt_WrappingAmount.Text.ToString();
            string Comment = txt_Comment.Value.ToString();
            string ConsigneeMobile = txt_ConsigneeMobile.Text.ToString();
            string ConsigneePhoneNo = txt_ConsigneePhoneNo.Text.ToString();

            #region Validation


            if (ConsignmentNo.Length == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter Consignment Number')", true);
                txt_ConsignmentNo.Focus();
                ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('BasicInfo');", true);
                return;
            }

            if (Consignee.Length <= 120)
            {
                if (Consignee.Contains("~") || Consignee.Contains("!") || Consignee.Contains("@") || Consignee.Contains("$") || Consignee.Contains("%")
                    || Consignee.Contains("^") || Consignee.Contains("\\") || Consignee.Contains("\"") || Consignee.Contains("\'") || Consignee.Contains(";")
                    || Consignee.Contains(":") || Consignee.Contains("?") || Consignee.Contains(">") || Consignee.Contains("<"))
                {
                    lbl_Message.InnerHtml = "Alert: These Characters are found in Consignee Name!<b>   ~   !   @   $   %   ^   \\   \'   \"  ;   :   ?   >   <</b>";
                    txt_Consignee.Focus();
                    ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('BasicInfo');", true);
                    return;
                }
            }
            else if (Consignee.Length > 120)
            {
                lbl_Message.InnerHtml = "Alert: Consignee Name Must Be Within 120 Character!";
                txt_Consignee.Focus();
                ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('BasicInfo');", true);
                return;
            }

            if (Consigner.Length <= 120)
            {
                if (Consigner.Contains("~") || Consigner.Contains("!") || Consigner.Contains("@") || Consigner.Contains("$") || Consigner.Contains("%")
                    || Consigner.Contains("^") || Consigner.Contains("\\") || Consigner.Contains("\"") || Consigner.Contains("\'") || Consigner.Contains(";")
                    || Consigner.Contains(":") || Consigner.Contains("?") || Consigner.Contains(">") || Consigner.Contains("<"))
                {
                    lbl_Message.InnerHtml = "Alert: These Characters are found in Consigner Name!<b>   ~   !   @   $   %   ^   \\   \'   \"  ;   :   ?   >   <</b>";
                    txt_Consigner.Focus();
                    ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('BasicInfo');", true);
                    return;
                }
            }
            else if (Consigner.Length > 120)
            {
                lbl_Message.InnerHtml = "Alert: Consigner Name Must Be Within 120 Character!";
                txt_Consigner.Focus();
                ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('BasicInfo');", true);
                return;
            }

            if (ConsigneeAddress.Length <= 120)
            {
                if (ConsigneeAddress.Contains("~") || ConsigneeAddress.Contains("!") || ConsigneeAddress.Contains("@") || ConsigneeAddress.Contains("$") || ConsigneeAddress.Contains("%")
                    || ConsigneeAddress.Contains("^") || ConsigneeAddress.Contains("\\") || ConsigneeAddress.Contains("\"") || ConsigneeAddress.Contains("\'") || ConsigneeAddress.Contains(";")
                    || ConsigneeAddress.Contains(":") || ConsigneeAddress.Contains("?") || ConsigneeAddress.Contains(">") || ConsigneeAddress.Contains("<"))
                {
                    lbl_Message.InnerHtml = "Alert: These Characters are found in Consignee Address!<b>   ~   !   @   $   %   ^   \\   \'   \"  ;   :   ?   >   <</b>";
                    txt_ConsigneeAddress.Focus();
                    ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('BasicInfo');", true);
                    return;
                }
            }
            else if (ConsigneeAddress.Length > 120)
            {
                lbl_Message.InnerHtml = "Alert: Consignee Address Must Be Within 120 Character!";
                txt_ConsigneeAddress.Focus();
                ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('BasicInfo');", true);
                return;
            }

            if (ConsignerAddress.Length <= 120)
            {
                if (ConsignerAddress.Contains("~") || ConsignerAddress.Contains("!") || ConsignerAddress.Contains("@") || ConsignerAddress.Contains("$") || ConsignerAddress.Contains("%")
                    || ConsignerAddress.Contains("^") || ConsignerAddress.Contains("\\") || ConsignerAddress.Contains("\"") || ConsignerAddress.Contains("\'") || ConsignerAddress.Contains(";")
                    || ConsignerAddress.Contains(":") || ConsignerAddress.Contains("?") || ConsignerAddress.Contains(">") || ConsignerAddress.Contains("<"))
                {
                    lbl_Message.InnerHtml = "Alert: These Characters are found in Consigner Address!<b>   ~   !   @   $   %   ^   \\   \'   \"  ;   :   ?   >   <</b>";
                    txt_ConsignerAddress.Focus();
                    ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('BasicInfo');", true);
                    return;
                }
            }
            else if (ConsignerAddress.Length > 120)
            {
                lbl_Message.InnerHtml = "Alert: Consigner Address Must Be Within 120 Character!";
                txt_ConsignerAddress.Focus();
                ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('BasicInfo');", true);
                return;
            }

            if (PackageContent.Length <= 120)
            {
                if (PackageContent.Contains("~") || PackageContent.Contains("!") || PackageContent.Contains("@") || PackageContent.Contains("$") || PackageContent.Contains("%")
                    || PackageContent.Contains("^") || PackageContent.Contains("\\") || PackageContent.Contains("\"") || PackageContent.Contains("\'") || PackageContent.Contains(";")
                    || PackageContent.Contains(":") || PackageContent.Contains("?") || PackageContent.Contains(">") || PackageContent.Contains("<"))
                {
                    lbl_Message.InnerHtml = "Alert: These Characters are found in Package Content!<b>   ~   !   @   $   %   ^   \\   \'   \"  ;   :   ?   >   <</b>";
                    txt_PackageContent.Focus();
                    ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('BasicInfo');", true);
                    return;
                }
            }
            else if (PackageContent.Length > 120)
            {
                lbl_Message.InnerHtml = "Alert: Package Content Must Be Within 120 Character!";
                txt_PackageContent.Focus();
                ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('BasicInfo');", true);
                return;
            }

            if (Comment.Length <= 120)
            {
                if (Comment.Contains("~") || Comment.Contains("!") || Comment.Contains("@") || Comment.Contains("$") || Comment.Contains("%")
                    || Comment.Contains("^") || Comment.Contains("\\") || Comment.Contains("\"") || Comment.Contains("\'") || Comment.Contains(";")
                    || Comment.Contains(":") || Comment.Contains("?") || Comment.Contains(">") || Comment.Contains("<"))
                {
                    lbl_Message.InnerHtml = "Alert: These Characters are found in Comment!<b>   ~   !   @   $   %   ^   \\   \'   \"  ;   :   ?   >   <</b>";
                    txt_Comment.Focus();
                    ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('BasicInfo');", true);
                    return;
                }
            }
            else if (Comment.Length > 120)
            {
                lbl_Message.InnerHtml = "Alert: Comment Must Be Within 120 Character!";
                txt_Comment.Focus();
                ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('BasicInfo');", true);
                return;
            }

            if (Pieces.Length > 0)
            {
                if (!Pieces.Contains("0") && !Pieces.Contains("1") && !Pieces.Contains("2") && !Pieces.Contains("3") && !Pieces.Contains("4")
                    && !Pieces.Contains("5") && !Pieces.Contains("6") && !Pieces.Contains("7") && !Pieces.Contains("8") && !Pieces.Contains("9"))
                {
                    lbl_Message.InnerHtml = "Alert: Only Numbers Allowed!";
                    txt_Pieces.Focus();
                    ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('BasicInfo');", true);
                    return;
                }
            }

            if (Weight.Length > 0)
            {
                if (!Weight.Contains("0") && !Weight.Contains("1") && !Weight.Contains("2") && !Weight.Contains("3") && !Weight.Contains("4")
                    && !Weight.Contains("5") && !Weight.Contains("6") && !Weight.Contains("7") && !Weight.Contains("8") && !Weight.Contains("9"))
                {
                    if (Weight.Contains("."))
                    {
                    }
                    else if (!Weight.Contains("."))
                    {
                        if (!Weight.Contains("0") && !Weight.Contains("1") && !Weight.Contains("2") && !Weight.Contains("3") && !Weight.Contains("4")
                            && !Weight.Contains("5") && !Weight.Contains("6") && !Weight.Contains("7") && !Weight.Contains("8") && !Weight.Contains("9"))
                        {
                            lbl_Message.InnerHtml = "Alert: Only Numbers Allowed!";
                            txt_Weight.Focus();
                            ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('BasicInfo');", true);
                            return;
                        }
                    }
                }
                if (Weight.EndsWith("."))
                {
                    lbl_Message.InnerHtml = "Alert: Invalid Weight Entered!";
                    txt_Weight.Focus();
                    ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('BasicInfo');", true);
                    return;
                }
            }

            if (WrappingCharges.Length > 0)
            {
                if (!WrappingCharges.Contains("0") && !WrappingCharges.Contains("1") && !WrappingCharges.Contains("2") && !WrappingCharges.Contains("3")
                    && !WrappingCharges.Contains("4") && !WrappingCharges.Contains("5") && !WrappingCharges.Contains("6") && !WrappingCharges.Contains("7")
                    && !WrappingCharges.Contains("8") && !WrappingCharges.Contains("9"))
                {
                    if (WrappingCharges.Contains("."))
                    {
                    }
                    else if (!WrappingCharges.Contains("."))
                    {
                        if (!WrappingCharges.Contains("0") && !WrappingCharges.Contains("1") && !WrappingCharges.Contains("2") && !WrappingCharges.Contains("3")
                            && !WrappingCharges.Contains("4") && !WrappingCharges.Contains("5") && !WrappingCharges.Contains("6") && !WrappingCharges.Contains("7")
                            && !WrappingCharges.Contains("8") && !WrappingCharges.Contains("9"))
                        {
                            lbl_Message.InnerHtml = "Alert: Only Numbers Allowed!";
                            txt_WrappingAmount.Focus();
                            ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('BasicInfo');", true);
                            return;
                        }
                    }
                }

                if (WrappingCharges.EndsWith("."))
                {
                    lbl_Message.InnerHtml = "Alert: Invalid Wrapping Amount Entered!";
                    txt_WrappingAmount.Focus();
                    ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('BasicInfo');", true);
                    return;
                }
            }

            if (BookingDate == "" || BookingDate == null)
            {
                lbl_Message.InnerHtml = "Alert: Booking Date Not Found!";
                txt_BookingDate.Focus();
                ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('BasicInfo');", true);
                return;
            }


            #endregion

            string CN_ = Encrypt_QueryString(ConsignmentNo);
            string Consignee_ = Encrypt_QueryString(Consignee);
            string Consigner_ = Encrypt_QueryString(Consigner);
            string ConsigneeAddress_ = Encrypt_QueryString(ConsigneeAddress);
            string ConsignerAddress_ = Encrypt_QueryString(ConsignerAddress);
            string BookingDate_ = Encrypt_QueryString(BookingDate);
            string OriginName_ = Encrypt_QueryString(OriginName);
            string DestinationName_ = Encrypt_QueryString(DestinationName);
            string Pieces_ = Encrypt_QueryString(Pieces);
            string Weight_ = Encrypt_QueryString(Weight);
            string ServiceType_ = Encrypt_QueryString(ServiceType);
            string PackageContent_ = Encrypt_QueryString(PackageContent);
            //string AccountNo_ = Encrypt_QueryString(AccountNo);
            //string WrappingCharges_ = Encrypt_QueryString(WrappingCharges);
            string Comment_ = Encrypt_QueryString(Comment);
            string ConsigneeMobile_ = Encrypt_QueryString(ConsigneeMobile);
            string ConsigneePhoneNo_ = Encrypt_QueryString(ConsigneePhoneNo);

            //string URL = "WrappingProcess_Print.aspx?ConsignmentNo=" + CN_ + "&Consigner=" + Consigner_ + "&ServiceType=" + ServiceType_ + "&Pieces=" + Pieces_ + "&Origin=" + OriginName_ + "&Destination=" + DestinationName_ + "&Consignee=" + Consignee_ + "&Weight=" + Weight_ + "&PackageContent=" + PackageContent_ + "&Comment=" + Comment_ + "&ConsigneeAddress=" + ConsigneeAddress_ + "&ConsignerAddress=" + ConsignerAddress_ + "&ConsigneePhoneNo=" + ConsigneePhoneNo_ + "&ConsigneeMobile=" + ConsigneeMobile_ + "&BookingDate=" + BookingDate_ + "WrappingCharges=" + WrappingCharges_ + "";
            string URL = "WrappingProcess_Print.aspx?ConsignmentNo=" + CN_ + "&Consigner=" + Consigner_ + "&ServiceType=" + ServiceType_ + "&Pieces=" + Pieces_ + "&Origin=" + OriginName_ + "&Destination=" + DestinationName_ + "&Consignee=" + Consignee_ + "&Weight=" + Weight_ + "&PackageContent=" + PackageContent_ + "&Comment=" + Comment_ + "&ConsigneeAddress=" + ConsigneeAddress_ + "&ConsignerAddress=" + ConsignerAddress_ + "&ConsigneePhoneNo=" + ConsigneePhoneNo_ + "&ConsigneeMobile=" + ConsigneeMobile_ + "&BookingDate=" + BookingDate_ + "";

            ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "OpenWindow", "window.open('" + URL + "','Wrapping Process Print','menubar=1,resizable=1,width=900,height=600');", true);

            lbl_Message.InnerHtml = "";
        }
    }
}