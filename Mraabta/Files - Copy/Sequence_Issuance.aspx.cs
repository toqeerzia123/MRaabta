using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using MRaabta.App_Code;

namespace MRaabta.Files
{
    public partial class Sequence_Issuance : System.Web.UI.Page
    {
        Cl_Invocie in1 = new Cl_Invocie();
        Cl_Variables clv = new Cl_Variables();

        protected void Page_Load(object sender, EventArgs e)
        {
            ZoneError.Text = "";
            BranchError.Text = "";
            ECError.Text = "";
            if (!IsPostBack)
            {
                Zone();
                Product();
                ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('BasicInfo');", true);
                //   Zone_Seq();
                //   Brnach_Seq();
                //   EC_Seq();
            }
        }

        public void Zone_Seq()
        {
            if (dd_Zone.SelectedValue != "0")
            {
                clv.CheckCondition = "\n AND mzc.ZoneCode = '" + dd_Zone.SelectedValue + "'";
            }

            if (dd_Product.SelectedValue != "0")
            {
                clv.CheckCondition += "\n AND mzc.Product = '" + dd_Product.SelectedValue + "'";
            }

            if (txt_startSeq.Text != "")
            {
                clv.CheckCondition += "\n AND '" + txt_startSeq.Text + "' between mzc.SequenceStart and mzc.EndSequence";
            }

            DataTable ds = Zone_Sequence(clv);

            if (ds.Rows.Count != 0)
            {
                gv_Main.DataSource = ds.DefaultView;
                gv_Main.DataBind();
            }
        }
        public void Brnach_Seq()
        {
            Cl_Variables clvar = new Cl_Variables();
            if (DropDownList1.SelectedValue == "0")
            {
                clvar.CheckCondition += "";
            }
            else
            {
                clvar.CheckCondition += " AND mzc.ZoneCode = '" + DropDownList1.SelectedValue + "'\n";
            }
            if (TextBox1.Text != "")
            {
                clvar.CheckCondition += " AND '" + TextBox1.Text + "' between SequenceStart and SequenceEnd \n";
            }
            if (dd_Branch.SelectedValue == "0")
            {
                clvar.CheckCondition += "";
            }
            else
            {
                clvar.CheckCondition += " AND mzc.Branch = '" + dd_Branch.SelectedValue + "'\n";
            }
            if (DropDownList2.SelectedValue == "0")
            {
                clvar.CheckCondition += "";
            }
            else
            {
                clvar.CheckCondition += " AND mzc.product = '" + DropDownList2.SelectedValue + "'";
            }

            DataTable ds = Brancg_Sequence_(clvar.CheckCondition.ToString());
            if (ds.Rows.Count != 0)
            {
                GridView1.DataSource = ds.DefaultView;
                GridView1.DataBind();

                ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('Configuration');", true);
            }
        }
        public void EC_Seq()
        {
            DataTable ds = Express_Sequence(clv);
            if (ds.Rows.Count != 0)
            {

                GridView2.DataSource = ds.DefaultView;
                GridView2.DataBind();
            }

        }
        public void Zone()
        {
            DataTable ds = in1.Active_Zones(clv);
            if (ds.Rows.Count != 0)
            {
                //1
                dd_Zone.DataTextField = "name";
                dd_Zone.DataValueField = "zonecode";
                dd_Zone.DataSource = ds.DefaultView;
                dd_Zone.DataBind();

                //2
                //1
                DropDownList1.DataTextField = "name";
                DropDownList1.DataValueField = "zonecode";
                DropDownList1.DataSource = ds.DefaultView;
                DropDownList1.DataBind();

                //3
                DropDownList3.DataTextField = "name";
                DropDownList3.DataValueField = "zonecode";
                DropDownList3.DataSource = ds.DefaultView;
                DropDownList3.DataBind();


                dd_Zone.SelectedValue = HttpContext.Current.Session["ZoneCode"].ToString();
                DropDownList1.SelectedValue = HttpContext.Current.Session["ZoneCode"].ToString();
                DropDownList3.SelectedValue = HttpContext.Current.Session["ZoneCode"].ToString();

                dd_Zone.Enabled = false;
                DropDownList1.Enabled = false;
                DropDownList3.Enabled = false;

                dd_Zone_SelectedIndexChanged(this, EventArgs.Empty);
                DropDownList1_SelectedIndexChanged(this, EventArgs.Empty);
                DropDownList3_SelectedIndexChanged(this, EventArgs.Empty);

            }
        }
        public void Product()
        {
            DataTable ds = in1.products(clv);
            if (ds.Rows.Count != 0)
            {
                //1
                dd_Product.DataTextField = "Products";
                dd_Product.DataValueField = "Products";
                dd_Product.DataSource = ds.DefaultView;
                dd_Product.DataBind();

                //2
                DropDownList2.DataTextField = "Products";
                DropDownList2.DataValueField = "Products";
                DropDownList2.DataSource = ds.DefaultView;
                DropDownList2.DataBind();

                //3
                //2
                DropDownList5.DataTextField = "Products";
                DropDownList5.DataValueField = "Products";
                DropDownList5.DataSource = ds.DefaultView;
                DropDownList5.DataBind();


            }
        }

        protected void btn_Save_Click(object sender, EventArgs e)
        {
            if (dd_Zone.SelectedValue == "0")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Select Zone')", true);
                return;
            }
            if (dd_Product.SelectedValue == "0")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Select Product.')", true);
                return;
            }
            if (txt_startSeq.Text.Length == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Sequence is invalid.')", true);
                return;
            }
            if (txt_seqend.Text.Length == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Sequence is invalid.')", true);
                return;
            }

            // Information
            Cl_Variables clvar = new Cl_Variables();
            clvar.Zone = dd_Zone.SelectedValue;
            clvar.productDescription = dd_Product.SelectedValue;
            clvar.startsequence = txt_startSeq.Text;
            clvar.endsequence = txt_seqend.Text;

            Int64 start = Int64.Parse(clvar.startsequence);
            Int64 end = Int64.Parse(clvar.endsequence);
            if (clvar.startsequence.Length != clvar.endsequence.Length)
            {
                ZoneAlert("Start Sequence AND End Sequence Must be of the same length", "Red");
                return;
            }
            if (start > end)
            {
                ZoneAlert("Start Sequence Cannot be Greater than End Sequence", "Red");
                return;
            }

            DataTable available = GetZoneSequenceAvailability(clvar);
            for (int i = 0; i < available.Columns.Count; i++)
            {
                if (available.Rows[0][i].ToString().ToUpper() == "NO")
                {
                    ZoneAlert("Invalid Sequence", "Red");
                    return;
                }
            }

            string test = in1.Insert_ZoneSeq(clvar);
            if (test == "1")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Sequence has been Added.')", true);
                ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('BasicInfo');", true);
                Zone_Seq();
                //   dd_Zone.SelectedValue = "0";
                //   dd_Product.SelectedValue = "0";
                txt_startSeq.Text = "";
                txt_seqend.Text = "";

                return;
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cannot add Sequence.')", true);
                ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('BasicInfo');", true);

                return;

            }

            ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('BasicInfo');", true);
        }

        protected void btn_Save1_Click(object sender, EventArgs e)
        {
            if (DropDownList1.SelectedValue == "0")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Select Zone')", true);
                return;
            }
            if (dd_Branch.SelectedValue == "0")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Select Branch')", true);
                BranchAlert("Please Select Branch", "Red");

                return;
            }
            if (DropDownList2.SelectedValue == "0")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Select Product.')", true);
                BranchAlert("Please Select Product", "Red");

                return;
            }
            if (TextBox1.Text.Length == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Sequence is invalid.')", true);
                BranchAlert("Sequence is invalid", "Red");

                return;
            }
            if (TextBox2.Text.Length == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Sequence is invalid.')", true);
                BranchAlert("Sequence is invalid", "Red");

                return;
            }

            // Information
            Cl_Variables clvar = new Cl_Variables();
            clvar.Zone = DropDownList1.SelectedValue;
            clvar.Branch = dd_Branch.SelectedValue;
            clvar.productDescription = DropDownList2.SelectedValue;
            clvar.startsequence = TextBox1.Text;
            clvar.endsequence = TextBox2.Text;

            Int64 start = Int64.Parse(clvar.startsequence);
            Int64 end = Int64.Parse(clvar.endsequence);
            if (clvar.startsequence.Length != clvar.endsequence.Length)
            {
                BranchAlert("Start Sequence AND End Sequence Must be of the same length", "Red");
                return;
            }
            if (start > end)
            {
                BranchAlert("Start Sequence Cannot be Greater than End Sequence", "Red");
                return;
            }

            DataTable available = GetBranchSequenceAvailability(clvar);
            for (int i = 0; i < available.Columns.Count; i++)
            {
                if (available.Rows[0][i].ToString().ToUpper() == "NO")
                {
                    BranchAlert("Invalid Sequence", "Red");
                    return;
                }
            }



            string test = in1.Insert_BranchSeq(clvar);
            if (test == "1")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Sequence has been Added.')", true);
                ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('Configuration');", true);
                Brnach_Seq();

                //  DropDownList1.SelectedValue = "0";
                dd_Branch.SelectedValue = "0";
                DropDownList2.SelectedValue = "0";
                TextBox1.Text = "";
                TextBox2.Text = "";
                return;
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cannot add Sequence.')", true);
                ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('Configuration');", true);
                return;

            }

            ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('Configuration');", true);
        }

        protected void btn_save2_Click(object sender, EventArgs e)
        {
            if (DropDownList3.SelectedValue == "0")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Select Zone')", true);
                ECAlert("Please Select Zone", "Red");

                return;
            }
            if (DropDownList4.SelectedValue == "0")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Select Branch')", true);
                ECAlert("Please Select Branch", "Red");

                return;
            }
            if (dd_ExpressCenter.SelectedValue == "0")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Select Express Center.')", true);
                ECAlert("Please Select Express Center", "Red");

                return;
            }
            if (dd_ridercode.SelectedValue == "0")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Select Rider.')", true);
                ECAlert("Please Select Rider", "Red");

                return;
            }
            if (DropDownList5.SelectedValue == "0")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Select Product.')", true);
                ECAlert("Please Select Product", "Red");

                return;
            }
            if (TextBox3.Text.Length == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Sequence is invalid.')", true);
                ECAlert("Sequence is invalid", "Red");

                return;
            }
            if (TextBox4.Text.Length == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Sequence is invalid.')", true); ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('billingNsales');", true);
                ECAlert("Sequence is invalid", "Red");

                return;
            }

            // Information
            Cl_Variables clvar = new Cl_Variables();
            clvar.Zone = DropDownList3.SelectedValue;
            clvar.Branch = DropDownList4.SelectedValue;
            clvar.expresscenter = dd_ExpressCenter.SelectedValue;
            clvar.RiderCode = dd_ridercode.SelectedValue;
            clvar.productDescription = DropDownList5.SelectedValue;
            clvar.Day = rb_list.SelectedValue;
            clvar.startsequence = TextBox3.Text;// txt_startSeq.Text;
            clvar.endsequence = TextBox4.Text;// txt_seqend.Text;

            Int64 start = Int64.Parse(clvar.startsequence);
            Int64 end = Int64.Parse(clvar.endsequence);

            if (clvar.startsequence.Length != clvar.endsequence.Length)
            {
                ECAlert("Start Sequence AND End Sequence Must be of the same length", "Red");
                ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('billingNsales');", true);

                return;
            }
            if (start > end)
            {
                ECAlert("Start Sequence Cannot be Greater than End Sequence", "Red");
                ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('billingNsales');", true);

                return;
            }

            DataTable available = GetRiderSequenceAvailability(clvar);
            for (int i = 0; i < available.Columns.Count; i++)
            {
                if (available.Rows[0][i].ToString().ToUpper() == "NO")
                {
                    ECAlert("Invalid Sequence", "Red");
                    ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('billingNsales');", true);

                    return;
                }
            }

            string test = in1.Insert_expressSeq(clvar);
            if (test == "1")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Sequence has been Added.')", true);
                ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('billingNsales');", true);
                btn_searchExpressSeq_Click(sender, e);
                dd_ExpressCenter.SelectedValue = "0";
                dd_ExpressCenter.Text = "";
                dd_ridercode.SelectedValue = "0";
                dd_ridercode.Text = "";
                rb_list.SelectedValue = "R";
                TextBox3.Text = "";
                TextBox4.Text = "";
                return;
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cannot add Sequence.')", true);
                ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('billingNsales');", true);
                return;

            }
            ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('billingNsales');", true);
        }

        protected void dd_ExpressCenter_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {

            Cl_Variables clvar = new Cl_Variables();
            clvar.Branch = DropDownList4.SelectedValue;
            clvar.expresscenter = dd_ExpressCenter.SelectedValue;
            clvar.Zone = DropDownList3.SelectedValue;
            //dd_ridercode.Items.Clear();
            //DataTable dt1 = in1.Rider(clvar);
            //dd_ridercode.DataTextField = "Name";
            //dd_ridercode.DataValueField = "riderCode";
            //dd_ridercode.DataSource = dt1.DefaultView;
            //dd_ridercode.DataBind();

            if (DropDownList3.SelectedValue == "0")
            {
                clvar.CheckCondition = "";
            }
            else
            {
                clvar.CheckCondition = " AND mzc.zoneCode = '" + DropDownList3.SelectedValue + "'\n";
            }
            if (DropDownList4.SelectedValue == "0")
            {
                clvar.CheckCondition += "";
            }
            else
            {
                clvar.CheckCondition += " AND mzc.Branch = '" + DropDownList4.SelectedValue + "'\n";
            }
            if (dd_ExpressCenter.SelectedValue == "0")
            {
                clvar.CheckCondition += "";
            }
            else
            {
                clvar.CheckCondition += " AND mzc.ExpressCenter = '" + dd_ExpressCenter.SelectedValue + "'\n";
            }

            //DataTable dt = Express_Sequence(clvar);// new DataTable();
            //if (dt != null)
            //{
            //    if (dt.Rows.Count > 0)
            //    {
            //        GridView2.DataSource = dt;
            //        GridView2.DataBind();
            //    }
            //    else
            //    {
            //        GridView2.DataSource = null;
            //        GridView2.DataBind();
            //    }
            //}

            //EC_Seq();
            // To remain on the same tab
            ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('billingNsales');", true);

        }

        public DataTable Rider(Cl_Variables clvar)
        {
            string sql = "SELECT r.expressCenterId, r.riderCode, \n"
                + "       r.riderCode + ' - ' + r.firstName NAME \n"
                + "FROM   Zones              z, \n"
                + "       Branches           b, \n"
                + "       ExpressCenters     ec,Riders r \n"
                + "WHERE  b.zoneCode = z.zoneCode \n"
                + "       AND ec.bid = b.branchCode \n"
                + "       AND r.branchId = b.branchCode \n"
                + "       AND r.expressCenterId = ec.expressCenterCode \n"
                + "       AND b.zoneCode = '" + clvar.Zone + "' \n"
                + "       AND b.branchCode = '" + clvar.Branch + "' and r.status='1' \n"
                + "ORDER BY \n"
                + "       ec.name \n"
                + " \n"
                + "";

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sql, con);
                sda.Fill(dt);

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return dt;
        }

        public DataTable Rider_(Cl_Variables clvar)
        {
            string sql = "SELECT r.expressCenterId, r.riderCode, \n"
                + "       r.riderCode + ' - ' + r.firstName NAME \n"
                + "FROM   Zones              z, \n"
                + "       Branches           b, \n"
                + "       ExpressCenters     ec,Riders r \n"
                + "WHERE  b.zoneCode = z.zoneCode \n"
                + "       AND ec.bid = b.branchCode \n"
                + "       AND r.branchId = b.branchCode \n"
                + "       AND r.expressCenterId = ec.expressCenterCode \n"
                + "       AND b.zoneCode = '" + clvar.Zone + "' \n"
                + "       AND b.branchCode = '" + clvar.Branch + "' and r.status='1' and Main_Ec='1'\n"
                + "ORDER BY \n"
                + "       ec.name \n"
                + " \n"
                + "";

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sql, con);
                sda.Fill(dt);

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return dt;
        }

        protected void DropDownList4_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Branch
            Cl_Variables clvar = new Cl_Variables();
            clvar.Branch = DropDownList4.SelectedValue;
            clvar.Zone = DropDownList3.SelectedValue;
            dd_ExpressCenter.Items.Clear();
            dd_ridercode.Items.Clear();
            DataTable dt1 = in1.ExpressCenter(clvar);
            dd_ExpressCenter.DataTextField = "Name";
            dd_ExpressCenter.DataValueField = "expressCenterCode";
            dd_ExpressCenter.DataSource = dt1.DefaultView;
            dd_ExpressCenter.DataBind();

            dt1 = null;

            dd_ridercode.Items.Clear();
            dt1 = Rider(clvar);
            dd_ridercode.DataTextField = "Name";
            dd_ridercode.DataValueField = "riderCode";
            dd_ridercode.DataSource = dt1.DefaultView;
            dd_ridercode.DataBind();


            if (DropDownList3.SelectedValue == "0")
            {
                clvar.CheckCondition = "";
            }
            else
            {
                clvar.CheckCondition = " AND mzc.zoneCode = '" + DropDownList3.SelectedValue + "'";
            }
            if (DropDownList4.SelectedValue == "0")
            {
                clvar.CheckCondition += "";
            }
            else
            {
                clvar.CheckCondition += " AND mzc.Branch = '" + DropDownList4.SelectedValue + "'";
            }
            //DataTable dt = Express_Sequence(clvar);// new DataTable();
            //if (dt != null)
            //{
            //    if (dt.Rows.Count > 0)
            //    {
            //        GridView2.DataSource = dt;
            //        GridView2.DataBind();
            //    }
            //    else
            //    {
            //        GridView2.DataSource = null;
            //        GridView2.DataBind();
            //    }
            //}
            rb_list_SelectedIndexChanged(sender, e);

            //EC_Seq();

            // To remain on the same tab
            ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('billingNsales');", true);

        }

        protected void DropDownList3_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Zone
            string zone = DropDownList3.SelectedValue;
            Cl_Variables clvar = new Cl_Variables();
            clvar.Zone = zone;
            DataTable dt1 = in1.branch(clvar);

            DropDownList4.Items.Clear();
            DropDownList4.Items.Add(new ListItem("Select Branch", "0"));
            DropDownList4.DataTextField = "name";
            DropDownList4.DataValueField = "branchCode";
            DropDownList4.DataSource = dt1.DefaultView;
            DropDownList4.DataBind();

            if (DropDownList3.SelectedValue == "0")
            {
                clvar.CheckCondition = "";
            }
            else
            {
                clvar.CheckCondition = " AND mzc.zoneCode = '" + DropDownList3.SelectedValue + "'";
            }

            DataTable dt = new DataTable();

            //dt = Express_Sequence(clvar);
            //if (dt != null)
            //{
            //    if (dt.Rows.Count > 0)
            //    {
            //        GridView2.DataSource = dt;
            //        GridView2.DataBind();
            //    }
            //    else
            //    {
            //        GridView2.DataSource = null;
            //        GridView2.DataBind();
            //    }
            //}
            //else
            //{
            //    GridView2.DataSource = null;
            //    GridView2.DataBind();
            //}

            // To remain on the same tab
            ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('billingNsales');", true);

        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string zone = DropDownList1.SelectedValue;
            Cl_Variables clvar = new Cl_Variables();
            clvar.Zone = zone;
            DataTable dt1 = in1.branch(clvar);

            dd_Branch.Items.Clear();
            dd_Branch.Items.Add(new ListItem("Select Branch", "0"));
            dd_Branch.DataTextField = "name";
            dd_Branch.DataValueField = "branchCode";
            dd_Branch.DataSource = dt1.DefaultView;
            dd_Branch.DataBind();
            clv.CheckCondition = "";
            if (DropDownList1.SelectedValue == "0")
            {
                clv.CheckCondition = "";
            }
            else
            {
                clv.CheckCondition = " AND mzc.ZoneCode = '" + DropDownList1.SelectedValue + "'";


            }
            //   DataTable branchSeq = Brancg_Sequence(clv);
            //   if (branchSeq.Rows.Count > 0)
            //   {
            //       GridView1.DataSource = branchSeq;
            //       GridView1.DataBind();
            //   }
            //   else
            //   {
            //       GridView1.DataSource = null;
            //       GridView1.DataBind();
            //       //BranchAlert("No Sequences Found", "Red");
            //   }

            ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('Configuration');", true);

        }

        protected void dd_Zone_SelectedIndexChanged(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('BasicInfo');", true);

        }

        protected void dd_Branch_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cl_Variables clvar = new Cl_Variables();
            clvar.Branch = dd_Branch.SelectedValue;
            clv.CheckCondition = "";
            if (DropDownList1.SelectedValue == "0")
            {
                clvar.CheckCondition += "";
            }
            else
            {
                clvar.CheckCondition += " AND mzc.ZoneCode = '" + DropDownList1.SelectedValue + "'\n";
            }
            if (dd_Branch.SelectedValue == "0")
            {
                clvar.CheckCondition += "";
            }
            else
            {
                clvar.CheckCondition += " AND mzc.Branch = '" + dd_Branch.SelectedValue + "'";
            }

            //DataTable ds = Brancg_Sequence(clvar);
            //if (ds.Rows.Count != 0)
            //{
            //    GridView1.DataSource = ds.DefaultView;
            //    GridView1.DataBind();
            //}
            //else
            //{
            //    GridView1.DataSource = null;
            //    GridView1.DataBind();

            //}
            ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('Configuration');", true);
        }

        protected void DropDownList2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cl_Variables clvar = new Cl_Variables();
            clvar.Branch = dd_Branch.SelectedValue;
            clvar.productDescription = DropDownList2.SelectedValue;
            if (DropDownList1.SelectedValue == "0")
            {
                clvar.CheckCondition += "";
            }
            else
            {
                clvar.CheckCondition += " AND mzc.ZoneCode = '" + DropDownList1.SelectedValue + "'\n";
            }
            if (dd_Branch.SelectedValue == "0")
            {
                clvar.CheckCondition += "";
            }
            else
            {
                clvar.CheckCondition += " AND mzc.Branch = '" + dd_Branch.SelectedValue + "'\n";
            }
            if (DropDownList2.SelectedValue == "0")
            {
                clvar.CheckCondition += "";
            }
            else
            {
                clvar.CheckCondition += " AND mzc.product = '" + DropDownList2.SelectedValue + "'";
            }

            //DataTable ds = Brancg_Sequence(clvar);
            //if (ds.Rows.Count != 0)
            //{
            //    GridView1.DataSource = ds.DefaultView;
            //    GridView1.DataBind();
            //}
            //else
            //{
            //    GridView1.DataSource = null;
            //    GridView1.DataBind();

            //}
            ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('Configuration');", true);

        }

        protected void dd_ridercode_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            Cl_Variables clvar = new Cl_Variables();
            if (DropDownList3.SelectedValue == "0")
            {
                clvar.CheckCondition = "";
            }
            else
            {
                clvar.CheckCondition = " AND mzc.zoneCode = '" + DropDownList3.SelectedValue + "'\n";
            }
            if (DropDownList4.SelectedValue == "0")
            {
                clvar.CheckCondition += "";
            }
            else
            {
                clvar.CheckCondition += " AND mzc.Branch = '" + DropDownList4.SelectedValue + "'\n";
            }
            if (dd_ExpressCenter.SelectedValue == "0")
            {
                clvar.CheckCondition += "";
            }
            else
            {
                clvar.CheckCondition += " AND mzc.ExpressCenter = '" + dd_ExpressCenter.SelectedValue + "'\n";
            }
            if (dd_ridercode.SelectedValue == "0")
            {
                clvar.CheckCondition += "";
            }
            else
            {
                clvar.CheckCondition += " AND mzc.Rider = '" + dd_ridercode.SelectedValue + "'";
            }

            DataTable dt = Express_Sequence(clvar);// new DataTable();
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    GridView2.DataSource = dt;
                    GridView2.DataBind();
                }
                else
                {
                    GridView2.DataSource = null;
                    GridView2.DataBind();
                }
            }

            //EC_Seq();
            ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('billingNsales');", true);

        }

        public void BranchAlert(string message, string color)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + message + "')", true);
            ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('Configuration');", true);
            BranchError.Text = message;
            BranchError.ForeColor = System.Drawing.Color.FromName(color);
        }
        public void ECAlert(string message, string color)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + message + "')", true);
            ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('billingNsales');", true);
            ECError.Text = message;
            ECError.ForeColor = System.Drawing.Color.FromName(color);
        }

        public DataTable GetBranchSequenceAvailability(Cl_Variables clvar)
        {

            string sqlString = "select MAX(a.ZoneAllowed) ZoneAllowed, MAX(a.BranchAllowed) BranchAllowed\n" +
            "  from (select case\n" +
            "                 when COUNT(*) > 0 then\n" +
            "                  'YES'\n" +
            "                 ELSE\n" +
            "                  'NO'\n" +
            "               END ZoneAllowed,\n" +
            "               '' BranchAllowed\n" +
            "          from Mnp_ZoneCNSquence c\n" +
            "         where c.SequenceStart <= '" + clvar.startsequence + "'\n" +
            "           and c.EndSequence >= '" + clvar.endsequence + "'\n" +
            "           and c.ZoneCode = '" + clvar.Zone + "'\n" +
            "           and c.Product = '" + clvar.productDescription + "'\n" +
            "        union\n" +
            "\n" +
            "        select '' ZoneAllowed,\n" +
            "               Case\n" +
            "                 when COUNT(*) > 0 then\n" +
            "                  'NO'\n" +
            "                 ELSE\n" +
            "                  'YES'\n" +
            "               END BranchAllowed\n" +
            "          from Mnp_BranchCNSequence_ z\n" +
            "         where ((CAST('" + clvar.startsequence + "' as bigint) between z.SequenceStart and\n" +
            "               z.SequenceEnd) OR (CAST('" + clvar.endsequence + "' as bigint) between\n" +
            "               z.SequenceStart and z.SequenceEnd) OR\n" +
            "               (CAST('" + clvar.startsequence + "' as bigint) < z.SequenceStart and\n" +
            "               CAST('" + clvar.endsequence + "' as bigint) > z.SequenceEnd))\n" +
            "\n" +
            "        ) a";
            SqlConnection con = new SqlConnection(clvar.Strcon());
            DataTable dt = new DataTable();
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
        public DataTable GetRiderSequenceAvailability(Cl_Variables clvar)
        {



            string sqlString = "select MAX(a.ZoneAllowed) ZoneAllowed,\n" +
            "       MAX(a.BranchAllowed) BranchAllowed,\n" +
            "       MAX(a.ECALLOWED) BranchAllowed\n" +
            "  from (select case\n" +
            "                 when COUNT(*) > 0 then\n" +
            "                  'YES'\n" +
            "                 ELSE\n" +
            "                  'NO'\n" +
            "               END ZoneAllowed,\n" +
            "               '' BranchAllowed,\n" +
            "               '' ECALLOWED\n" +
            "          from Mnp_ZoneCNSquence c\n" +
            "         where c.SequenceStart <= '" + clvar.startsequence + "'\n" +
            "           and c.EndSequence >= '" + clvar.endsequence + "'\n" +
            "           and c.ZoneCode = '" + clvar.Zone + "'\n" +
            "           and c.Product = '" + clvar.productDescription + "'\n" +
            "        union\n" +
            "        select '' ZoneAllowed,\n" +
            "\n" +
            "               case\n" +
            "                 when COUNT(*) > 0 then\n" +
            "                  'YES'\n" +
            "                 ELSE\n" +
            "                  'NO'\n" +
            "               END BranchAllowed,\n" +
            "               '' ECALLOWED\n" +
            "          from Mnp_BranchCNSequence_ c\n" +
            "         where c.SequenceStart <= '" + clvar.startsequence + "'\n" +
            "           and c.SequenceEnd >= '" + clvar.endsequence + "'\n" +
            "           and c.ZoneCode = '" + clvar.Zone + "'\n" +
            "           and c.Branch = '" + clvar.Branch + "'\n" +
            "           and c.Product = '" + clvar.productDescription + "'\n" +
            "        union\n" +
            "        select '' ZoneAllowed,\n" +
            "               '' BranchAllowed,\n" +
            "               Case\n" +
            "                 when COUNT(*) > 0 then\n" +
            "                  'NO'\n" +
            "                 ELSE\n" +
            "                  'YES'\n" +
            "               END ECAllowed\n" +
            "          from Mnp_RiderCNSequence z\n" +
            "         where ((CAST('" + clvar.startsequence + "' as bigint) between z.SequenceStart and\n" +
            "               z.SequenceEnd) OR (CAST('" + clvar.endsequence + "' as bigint) between\n" +
            "               z.SequenceStart and z.SequenceEnd) OR\n" +
            "               (CAST('" + clvar.startsequence + "' as bigint) < z.SequenceStart and\n" +
            "               CAST('" + clvar.endsequence + "' as bigint) > z.SequenceEnd))\n" +
            "\n" +
            "        ) a";

            SqlConnection con = new SqlConnection(clvar.Strcon());
            DataTable dt = new DataTable();
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
        protected void btn_zoneSearch_Click(object sender, EventArgs e)
        {

            if (dd_Zone.SelectedValue != "0")
            {
                clv.CheckCondition = "\n AND mzc.ZoneCode = '" + dd_Zone.SelectedValue + "'";
            }

            if (dd_Product.SelectedValue != "0")
            {
                clv.CheckCondition += "\n AND mzc.Product = '" + dd_Product.SelectedValue + "'";
            }

            if (txt_startSeq.Text != "")
            {
                clv.CheckCondition += "\n AND '" + txt_startSeq.Text + "' between mzc.SequenceStart and mzc.EndSequence";
            }

            DataTable dt = Zone_Sequence(clv);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    gv_Main.DataSource = dt;
                    gv_Main.DataBind();
                    ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('BasicInfo');", true);
                }
                else
                {
                    gv_Main.DataSource = null;
                    gv_Main.DataBind();
                    ZoneAlert("No Sequences Found", "Red");
                    return;
                }
            }
            else
            {
                ZoneAlert("No Sequences Found", "Red");
                return;

            }
        }

        #region Zone Related Methods

        public DataTable Zone_Sequence(Cl_Variables clvar)
        {
            string sql = " \n"
               + "SELECT mzc.id, z.name Zone, \n"
               + "       mzc.SequenceStart, \n"
               + "       mzc.EndSequence, \n"
               + "       mzc.Product,\n"
               + "       (mzc.EndSequence -  mzc.SequenceStart) + 1 Qty, \n"
               + "       mzc.Created_On,\n"
               + "       zu.U_NAME--, \n"
               //+ "       (             \n"
               //+ "               SELECT COUNT(c.consignmentNumber)   \n"
               //+ "                FROM   Consignment c             \n"
               //+ "                WHERE  CAST(c.consignmentNumber AS VARCHAR(16)) BETWEEN CAST(mzc.SequenceStart AS VARCHAR)   \n"
               //+ "                       AND CAST(mzc.EndSequence AS VARCHAR)  \n"
               //+ "                       AND c.zoneCode = mzc.ZoneCode   \n"
               //+ "            ) usage   \n"
               + "FROM   Mnp_ZoneCNSquence     mzc, \n"
               + "       Zones                 z, \n"
               + "       ZNI_USER1             zu \n"
               + "WHERE  z.zoneCode = mzc.ZoneCode and z.zoneCode = '" + HttpContext.Current.Session["ZoneCode"].ToString() + "' " + clvar.CheckCondition + "\n"
               + "       AND zu.U_ID = mzc.Created_By";

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sql, con);
                sda.SelectCommand.CommandTimeout = 30;
                sda.Fill(dt);

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return dt;
        }

        public DataTable Zone_Sequence_(Cl_Variables clvar)
        {
            string sql = " \n"
               + "SELECT mzc.id, z.name Zone, \n"
               + "       mzc.SequenceStart, \n"
               + "       mzc.EndSequence, \n"
               + "       mzc.Product,\n"
               + "       (mzc.EndSequence -  mzc.SequenceStart) + 1 Qty, \n"
               + "       mzc.Created_On,\n"
               + "       zu.U_NAME--, \n"
               //+ "       (             \n"
               //+ "               SELECT COUNT(c.consignmentNumber)   \n"
               //+ "                FROM   Consignment c             \n"
               //+ "                WHERE  CAST(c.consignmentNumber AS VARCHAR(16)) BETWEEN CAST(mzc.SequenceStart AS VARCHAR)   \n"
               //+ "                       AND CAST(mzc.EndSequence AS VARCHAR)  \n"
               //+ "                       AND c.zoneCode = mzc.ZoneCode   \n"
               //+ "            ) usage   \n"
               + "FROM   Mnp_ZoneCNSquence     mzc, \n"
               + "       Zones                 z, \n"
               + "       ZNI_USER1             zu \n"
               + "WHERE  z.zoneCode = mzc.ZoneCode and z.zoneCode = '" + HttpContext.Current.Session["ZoneCode"].ToString() + "' " + clvar.CheckCondition + "\n"
               + "       AND zu.U_ID = mzc.Created_By";

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sql, con);
                sda.SelectCommand.CommandTimeout = 30;
                sda.Fill(dt);

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return dt;
        }

        public DataTable GetZoneSequenceAvailability(Cl_Variables clvar)
        {

            string sqlString = "select Case\n" +
            "         when COUNT(*) > 0 then\n" +
            "          'NO'\n" +
            "         ELSE\n" +
            "          'YES'\n" +
            "       END ZoneAllowed\n" +
            "  from Mnp_ZoneCNSquence z\n" +
            " where ((CAST('" + clvar.startsequence + "' as bigint) between z.SequenceStart and z.EndSequence) OR\n" +
            "       (CAST('" + clvar.endsequence + "' as bigint) between z.SequenceStart and z.EndSequence) OR\n" +
            "       (CAST('" + clvar.startsequence + "' as bigint) < z.SequenceStart and\n" +
            "       CAST('" + clvar.endsequence + "' as bigint) > z.EndSequence))";

            SqlConnection con = new SqlConnection(clvar.Strcon());
            DataTable dt = new DataTable();
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

        protected void gv_Main_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.ToString() == "Del")
            {
                string ID = e.CommandArgument.ToString();

                List<string> error = DeleteZoneSeq(ID);
                if (error[0] == "0")
                {
                    ZoneAlert(error[1].ToString(), "Red");
                }
                else
                {
                    ZoneAlert(error[1].ToString(), "Green");
                    DataTable dt = Zone_Sequence(clv);
                    if (dt.Rows.Count > 0)
                    {
                        gv_Main.DataSource = dt;
                        gv_Main.DataBind();
                    }
                }

            }
        }
        protected List<string> DeleteZoneSeq(string ID)
        {
            List<string> response = new List<string>();

            string query = "";
            query = "\n" +
            "SELECT CASE\n" +
            "         WHEN MAX(A.BRCOUNT) = 0 then\n" +
            "          'YES'\n" +
            "         ELSE\n" +
            "          'NO'\n" +
            "       END BRALLOWED,\n" +
            "       CASE\n" +
            "         WHEN MAX(A.ECCOUNT) = 0 then\n" +
            "          'YES'\n" +
            "         ELSE\n" +
            "          'NO'\n" +
            "       END BRALLOWED\n" +
            "  FROM (select COUNT(bcs.ID) BRCOUNT, '' ECCOUNT\n" +
            "          from Mnp_BranchCNSequence_ bcs\n" +
            "         where bcs.SequenceStart between\n" +
            "               (select zcs.SequenceStart\n" +
            "                  from Mnp_ZoneCNSquence zcs\n" +
            "                 where zcs.ID = '" + ID + "') and\n" +
            "               (select zcs.EndSequence\n" +
            "                  from Mnp_ZoneCNSquence zcs\n" +
            "                 where zcs.ID = '" + ID + "')\n" +
            "        union\n" +
            "        select '' BRCOUNT, COUNT(bcs.id) ECCOUNT\n" +
            "          from Mnp_RiderCNSequence bcs\n" +
            "         where bcs.SequenceStart between\n" +
            "               (select zcs.SequenceStart\n" +
            "                  from Mnp_ZoneCNSquence zcs\n" +
            "                 where zcs.ID = '" + ID + "') and\n" +
            "               (select zcs.EndSequence\n" +
            "                  from Mnp_ZoneCNSquence zcs\n" +
            "                 where zcs.ID = '" + ID + "')) A";



            string sqlString = "select ISNULL(COUNT(c.consignmentNumber),0)\n" +
                "  from consignment c, Mnp_ZoneCNSquence mzc\n" +
                " where c.consignmentNumber between CAST(mzc.SequenceStart as nvarchar) and\n" +
                "       CAST(mzc.EndSequence as nvarchar)\n" +
                "   and mzc.ID = '" + ID + "'";



            sqlString = "select ISNULL(COUNT(c.consignmentNumber), 0)\n" +
            "  from consignment c, Mnp_ZoneCNSquence mzc\n" +
            " where c.consignmentNumber between CAST(mzc.SequenceStart as nvarchar) and\n" +
            "       CAST(mzc.EndSequence as nvarchar)\n" +
            "   and mzc.ID = '" + ID + "'\n" +
            "   and LEN(mzc.SequenceStart) = LEN(c.consignmentNumber)\n" +
            "   and LEN(mzc.EndSequence) = LEN(c.consignmentNumber)";


            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clv.Strcon());
            SqlCommand cmd = new SqlCommand();
            try
            {
                con.Open();

                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                sda.Fill(dt);
                if (dt != null)
                {
                    if (dt.Rows.Count != 0)
                    {
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            if (dt.Rows[0][i].ToString() == "NO")
                            {
                                response.Add("0");
                                response.Add("Seq Already Issued to Branch or ExpressCenter/Rider. Could Not be Deleted.");
                                con.Close();
                                return response;
                            }
                        }
                    }
                    else
                    {
                        response.Add("0");
                        response.Add("Seq Could Not be Deleted.");
                        con.Close();
                        return response;
                    }
                }
                else
                {
                    response.Add("0");
                    response.Add("Seq Could Not be Deleted.");
                    con.Close();
                    return response;
                }

                sda = new SqlDataAdapter(sqlString, con);
                dt = new DataTable();
                sda.Fill(dt);
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0].ToString() != "0")
                        {
                            response.Add("0");
                            response.Add("This Seq is in use. Cannot Delete");
                        }
                        else
                        {
                            cmd.Connection = con;
                            cmd.CommandText = "insert into Mnp_ZoneCNSquence_archive select mzc.*, GETDATE(), '" + HttpContext.Current.Session["U_ID"].ToString() + "' from Mnp_ZoneCNSquence mzc where mzc.id = '" + ID + "'";
                            object obj = cmd.ExecuteNonQuery();
                            if (obj.ToString() == "0")
                            {
                                response.Add("0");
                                response.Add("Seq Could Not be Deleted.");

                            }
                            else
                            {
                                cmd.CommandText = "Delete from Mnp_ZoneCNSquence where id = '" + ID + "'";
                                obj = new object();
                                obj = cmd.ExecuteNonQuery();
                                if (obj.ToString() != "1")
                                {
                                    response.Add("0");
                                    response.Add("Seq Could Not be Deleted.");
                                }
                                else
                                {
                                    response.Add("1");
                                    response.Add("Seq Deleted.");
                                }
                            }

                        }
                    }
                    else
                    {
                        response.Add("0");
                        response.Add("Seq Could Not be Deleted.");
                    }
                }
                else
                {
                    response.Add("0");
                    response.Add("Seq Could Not be Deleted.");
                }
            }
            catch (Exception ex)
            {
                response.Add("0");
                response.Add(ex.Message);
            }
            finally { con.Close(); }

            return response;
        }
        public void ZoneAlert(string message, string color)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + message + "')", true);
            ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('BasicInfo');", true);
            ZoneError.Text = message;
            ZoneError.ForeColor = System.Drawing.Color.FromName(color);
        }
        #endregion

        #region Branch Related Methods

        public DataTable Brancg_Sequence(Cl_Variables clvar)
        {
            string sql = "SELECT mzc.id, z.name zone, \n"
               + "       b.sname Bname, \n"
               + "       mzc.SequenceStart, \n"
               + "       mzc.SequenceEnd, \n"
               + "       mzc.Product, \n"
               + "       (mzc.SequenceEnd -  mzc.SequenceStart) + 1 Qty, \n"
               + "       zu.U_NAME                CreatedBy, \n"
               + "       mzc.Created_On--,\n"
               //+ "       (             \n"
               //+ "               SELECT COUNT(c.consignmentNumber)   \n"
               //+ "                FROM   Consignment c             \n"
               //+ "                WHERE  CAST(c.consignmentNumber AS VARCHAR(16)) BETWEEN CAST(mzc.SequenceStart AS VARCHAR)   \n"
               //+ "                       AND CAST(mzc.EndSequence AS VARCHAR)  \n"
               //+ "                       AND c.zoneCode = mzc.ZoneCode  and c.orgin=  mzc.Branch \n"
               //+ "            ) usage   \n"
               + "FROM   Mnp_BranchCNSequence_     mzc, \n"
               + "       Zones                    z, \n"
               + "       Branches                 b, \n"
               + "       ZNI_USER1                zu \n"
               + "WHERE  z.zoneCode = mzc.ZoneCode \n"
               + "       AND b.branchCode = mzc.Branch " + clvar.CheckCondition + "\n"
               + "       AND zu.U_ID = mzc.Created_By";

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sql, con);
                sda.Fill(dt);

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return dt;
        }


        public DataTable Brancg_Sequence_(string A)
        {
            Cl_Variables clvar = new Cl_Variables();
            string sql = "SELECT mzc.id, z.name zone, \n"
               + "       b.sname Bname, \n"
               + "       mzc.SequenceStart, \n"
               + "       mzc.SequenceEnd, \n"
               + "       mzc.Product, \n"
               + "       (mzc.SequenceEnd -  mzc.SequenceStart) + 1 Qty, \n"
               + "       zu.U_NAME                CreatedBy, \n"
               + "       mzc.Created_On--,\n"
               //+ "       (             \n"
               //+ "               SELECT COUNT(c.consignmentNumber)   \n"
               //+ "                FROM   Consignment c             \n"
               //+ "                WHERE  CAST(c.consignmentNumber AS VARCHAR(16)) BETWEEN CAST(mzc.SequenceStart AS VARCHAR)   \n"
               //+ "                       AND CAST(mzc.EndSequence AS VARCHAR)  \n"
               //+ "                       AND c.zoneCode = mzc.ZoneCode  and c.orgin=  mzc.Branch \n"
               //+ "            ) usage   \n"
               + "FROM   Mnp_BranchCNSequence_     mzc, \n"
               + "       Zones                    z, \n"
               + "       Branches                 b, \n"
               + "       ZNI_USER1                zu \n"
               + "WHERE  z.zoneCode = mzc.ZoneCode \n"
               + "       AND b.branchCode = mzc.Branch " + A + "\n"
               + "       AND zu.U_ID = mzc.Created_By";

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sql, con);
                sda.Fill(dt);

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return dt;
        }

        protected void btn_searchBranchSeq_Click(object sender, EventArgs e)
        {
            Cl_Variables clvar = new Cl_Variables();
            if (DropDownList1.SelectedValue == "0")
            {
                clvar.CheckCondition = "";
            }
            else
            {
                clvar.CheckCondition += " AND mzc.ZoneCode = '" + DropDownList1.SelectedValue + "'\n";
            }
            if (TextBox1.Text != "")
            {
                clvar.CheckCondition += " AND '" + TextBox1.Text + "' between SequenceStart and SequenceEnd \n";
            }
            if (dd_Branch.SelectedValue == "0")
            {
                clvar.CheckCondition += "";
            }
            else
            {
                clvar.CheckCondition += " AND mzc.Branch = '" + dd_Branch.SelectedValue + "'\n";
            }
            if (DropDownList2.SelectedValue == "0")
            {
                clvar.CheckCondition += "";
            }
            else
            {
                clvar.CheckCondition += " AND mzc.product = '" + DropDownList2.SelectedValue + "'";
            }

            DataTable ds = Brancg_Sequence(clvar);
            if (ds.Rows.Count != 0)
            {
                GridView1.DataSource = ds.DefaultView;
                GridView1.DataBind();

                ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('Configuration');", true);
            }
            else
            {
                GridView1.DataSource = null;
                GridView1.DataBind();
                BranchAlert("No Seq Found", "Red");
            }
            ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('Configuration');", true);
        }
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.ToString() == "Del")
            {
                string ID = e.CommandArgument.ToString();

                List<string> error = DeleteBranchSequence(ID);
                if (error[0] == "0")
                {
                    BranchAlert(error[1].ToString(), "Red");
                }
                else
                {
                    BranchAlert(error[1].ToString(), "Green");
                    if (DropDownList1.SelectedValue == "0")
                    {
                        clv.CheckCondition += "";
                    }
                    else
                    {
                        clv.CheckCondition += " AND mzc.ZoneCode = '" + DropDownList1.SelectedValue + "'\n";
                    }
                    if (dd_Branch.SelectedValue == "0")
                    {
                        clv.CheckCondition += "";
                    }
                    else
                    {
                        clv.CheckCondition += " AND mzc.Branch = '" + dd_Branch.SelectedValue + "'\n";
                    }
                    if (DropDownList2.SelectedValue == "0")
                    {
                        clv.CheckCondition += "";
                    }
                    else
                    {
                        clv.CheckCondition += " AND mzc.product = '" + DropDownList2.SelectedValue + "'";
                    }


                    DataTable dt = Brancg_Sequence(clv);
                    if (dt.Rows.Count > 0)
                    {
                        GridView1.DataSource = dt;
                        GridView1.DataBind();
                    }

                }

            }
        }


        public List<string> DeleteBranchSequence(string ID)
        {
            List<string> response = new List<string>();
            string sqlString = "SELECT CASE\n" +
            "         WHEN COUNT(bcs.id) = 0 then\n" +
            "          'YES'\n" +
            "         ELSE\n" +
            "          'NO'\n" +
            "       END ECAllowed\n" +
            "  from Mnp_RiderCNSequence bcs\n" +
            " where bcs.SequenceStart between\n" +
            "       (select zcs.SequenceStart\n" +
            "          from Mnp_BranchCNSequence_ zcs\n" +
            "         where zcs.ID = '" + ID + "') and\n" +
            "       (select zcs.SequenceEnd\n" +
            "          from Mnp_BranchCNSequence_ zcs\n" +
            "         where zcs.ID = '" + ID + "')";

            string sqlString_ = "select ISNULL(COUNT(c.consignmentNumber), 0)\n" +
            "  from consignment c, Mnp_BranchCNSequence_ mzc\n" +
            " where c.consignmentNumber between CAST(mzc.SequenceStart as nvarchar) and\n" +
            "       CAST(mzc.SequenceEND as nvarchar)\n" +
            "   and mzc.ID = '" + ID + "'\n" +
            "   and LEN(mzc.SequenceStart) = LEN(c.consignmentNumber)\n" +
            "   and LEN(mzc.SequenceEND) = LEN(c.consignmentNumber)";
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clv.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sqlString, con);
                sda.Fill(dt);
                #region Checking For Child Sequence
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0].ToString() == "NO")
                        {
                            con.Close();
                            response.Add("0");
                            response.Add("Sequence Already Issued to ExpressCenter/Rider. Can not be deleted.");
                            return response;
                        }
                    }
                    else
                    {
                        con.Close();
                        response.Add("0");
                        response.Add("Sequence Could not be deleted.");
                        return response;
                    }
                }
                else
                {
                    con.Close();
                    response.Add("0");
                    response.Add("Sequence Could not be deleted.");
                    return response;
                }
                #endregion

                dt = new DataTable();
                sda = new SqlDataAdapter(sqlString_, con);
                sda.Fill(dt);
                #region Checking For Usage
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0].ToString() != "0")
                        {
                            con.Close();
                            response.Add("0");
                            response.Add("Sequence Already in Use. Cannot Delete.");
                            return response;
                        }
                    }
                    else
                    {
                        con.Close();
                        response.Add("0");
                        response.Add("Sequence Could not be deleted.");
                        return response;
                    }
                }
                else
                {
                    con.Close();
                    response.Add("0");
                    response.Add("Sequence Could not be deleted.");
                    return response;
                }
                #endregion

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;

                cmd.CommandText = "insert into Mnp_BranchCNSequence_Archive selecT bcs.*, '" + HttpContext.Current.Session["U_ID"].ToString() + "', GETDATE() from Mnp_BranchCNSequence_ bcs where bcs.ID = '" + ID + "' ";
                object obj = cmd.ExecuteNonQuery();
                if (obj.ToString() == "0")
                {
                    con.Close();
                    response.Add("0");
                    response.Add("Sequence Could not be deleted.");
                    return response;
                }

                cmd.CommandText = "DELETE FROM Mnp_BranchCNSequence_ where id = '" + ID + "'";
                obj = new object();
                obj = cmd.ExecuteNonQuery();
                if (obj.ToString() == "0")
                {
                    con.Close();
                    response.Add("0");
                    response.Add("Sequence Could not be deleted.");
                    return response;
                }
                else
                {
                    response.Add("1");
                    response.Add("Sequence deleted.");
                }
            }
            catch (Exception ex)
            {
                response.Add("0");
                response.Add(ex.Message);
            }
            finally { con.Close(); }
            return response;
        }
        #endregion

        #region Rider Related Methods
        public DataTable Express_Sequence(Cl_Variables clvar)
        {
            string sql = "/************************************************************ \n"
               + " * Code formatted by SoftTree SQL Assistant © v6.3.153 \n"
               + " * Time: 01/12/2016 7:38:13 PM \n"
               + " ************************************************************/ \n"
               + " \n"
               + "SELECT mzc.id, z.name                  zone, \n"
               + "       b.sname                 Bname, \n"
               + "       ec.name              ECName, \n"
               + "       r.firstName +' '+r.lastName RiderName,\n"
               + "       mzc.SequenceStart, \n"
               + "       mzc.SequenceEnd, \n"
               + "       mzc.Product, \n"
               + "       (mzc.SequenceEnd - mzc.SequenceStart) + 1 Qty, \n"
               + "       zu.U_NAME               CreatedBy, \n"
               + "       mzc.Created_On--, \n"
               + "FROM   Mnp_RiderCNSequence     mzc, \n"
               + "       Zones                   z, \n"
               + "       Branches                b,  \n"
               + "       ZNI_USER1               zu, \n"
               + "       ExpressCenters         ec,Riders r \n"
               + "WHERE  z.zoneCode = mzc.ZoneCode \n"
               + "       AND b.branchCode = mzc.Branch \n"
               + "       AND ec.bid = b.branchCode \n"
               + "       AND ec.expresscentercode = mzc.ExpressCenter \n"
               + "       AND zu.U_ID = mzc.Created_By \n"
               + "       AND mzc.Rider = r.riderCode \n"
               + "       AND r.branchId = b.branchCode and r.status = '1' " + clvar.CheckCondition + "";

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sql, con);
                sda.Fill(dt);

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return dt;
        }

        protected void btn_searchExpressSeq_Click(object sender, EventArgs e)
        {
            Cl_Variables clvar = new Cl_Variables();
            if (DropDownList3.SelectedValue == "0")
            {
                clvar.CheckCondition += "";
            }
            else
            {
                clvar.CheckCondition += " AND mzc.ZoneCode = '" + DropDownList1.SelectedValue + "'\n";
            }
            if (DropDownList4.SelectedValue == "0")
            {
                clvar.CheckCondition += "";
            }
            else
            {
                clvar.CheckCondition += " AND mzc.branch = '" + DropDownList4.SelectedValue + "'\n";
            }
            if (dd_ExpressCenter.SelectedValue == "0")
            {
                clvar.CheckCondition += "";
            }
            else
            {
                clvar.CheckCondition += " AND mzc.ExpressCenter = '" + dd_ExpressCenter.SelectedValue + "'\n";
            }
            if (TextBox3.Text != "")
            {
                clvar.CheckCondition += " AND '" + TextBox3.Text + "' between SequenceStart and SequenceEnd \n";
            }

            DataTable ds = Express_Sequence(clvar);
            if (ds.Rows.Count != 0)
            {
                GridView2.DataSource = ds.DefaultView;
                GridView2.DataBind();
                ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('billingNsales');", true);

            }
            else
            {
                GridView2.DataSource = null;
                GridView2.DataBind();
                ECAlert("No Seq Found", "Red");
                ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('billingNsales');", true);

            }
        }

        protected void GridView2_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.ToString() == "Del")
            {
                string ID = e.CommandArgument.ToString();
                List<string> response = DeleteECSequence(ID);
                ECAlert(response[1], "Red");
                if (DropDownList3.SelectedValue == "0")
                {
                    clv.CheckCondition = "";
                }
                else
                {
                    clv.CheckCondition = " AND mzc.zoneCode = '" + DropDownList3.SelectedValue + "'\n";
                }
                if (DropDownList4.SelectedValue == "0")
                {
                    clv.CheckCondition += "";
                }
                else
                {
                    clv.CheckCondition += " AND mzc.Branch = '" + DropDownList4.SelectedValue + "'\n";
                }
                if (dd_ExpressCenter.SelectedValue == "0" || dd_ExpressCenter.SelectedValue.Trim() == "")
                {
                    clv.CheckCondition += "";
                }
                else
                {
                    clv.CheckCondition += " AND mzc.ExpressCenter = '" + dd_ExpressCenter.SelectedValue + "'\n";
                }
                if (dd_ridercode.SelectedValue == "0" || dd_ridercode.SelectedValue.Trim() == "")
                {
                    clv.CheckCondition += "";
                }
                else
                {
                    clv.CheckCondition += " AND mzc.Rider = '" + dd_ridercode.SelectedValue + "'";
                }

                DataTable dt = Express_Sequence(clv);// new DataTable();
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        GridView2.DataSource = dt;
                        GridView2.DataBind();
                        ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('billingNsales');", true);

                    }
                    else
                    {
                        GridView2.DataSource = null;
                        GridView2.DataBind();
                    }
                }
            }
        }

        protected List<string> DeleteECSequence(string ID)
        {
            List<string> response = new List<string>();


            string sqlString = "select ISNULL(COUNT(c.consignmentNumber), 0)\n" +
            "          from consignment c, Mnp_RiderCNSequence mzc\n" +
            "         where c.consignmentNumber between CAST(mzc.SequenceStart as nvarchar) and\n" +
            "               CAST(mzc.SequenceEND as nvarchar)\n" +
            "           and mzc.ID = '" + ID + "'\n" +
            "           and LEN(mzc.SequenceStart) = LEN(c.consignmentNumber)\n" +
            "           and LEN(mzc.SequenceEND) = LEN(c.consignmentNumber)";

            SqlConnection con = new SqlConnection(clv.Strcon());
            DataTable dt = new DataTable();
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sqlString, con);
                sda.Fill(dt);
                #region Usage Check
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0].ToString() != "0")
                        {
                            con.Close();
                            response.Add("0");
                            response.Add("Sequence Already in Use. Cannot Delete.");
                            return response;
                        }
                    }
                    else
                    {
                        con.Close();
                        response.Add("0");
                        response.Add("Sequence Could not be deleted.");
                        return response;
                    }
                }
                else
                {
                    con.Close();
                    response.Add("0");
                    response.Add("Sequence Could not be deleted.");
                    return response;
                }
                #endregion

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "INSERT INTO Mnp_RiderCNSequence_Archive selecT rcs.*, '" + HttpContext.Current.Session["U_ID"].ToString() + "', GETDATE() from Mnp_RiderCNSequence rcs where rcs.ID = '" + ID + "' ";
                object obj = cmd.ExecuteNonQuery();
                if (obj.ToString() == "0")
                {
                    con.Close();
                    response.Add("0");
                    response.Add("Sequence Could not be deleted.");
                    return response;
                }

                cmd.CommandText = "DELETE FROM Mnp_RiderCNSequence where id = '" + ID + "'";
                obj = new object();
                obj = cmd.ExecuteNonQuery();
                if (obj.ToString() == "0")
                {
                    con.Close();
                    response.Add("0");
                    response.Add("Sequence Could not be deleted.");
                    return response;
                }

                response.Add("1");
                response.Add("Sequence Deleted.");
            }
            catch (Exception ex)
            {

                response.Add("0");
                response.Add(ex.Message);
            }
            finally { con.Close(); }
            return response;
        }
        #endregion

        protected void rb_list_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rb_list.SelectedValue == "R")
            {
                Cl_Variables clvar = new Cl_Variables();
                clvar.Branch = DropDownList4.SelectedValue;
                clvar.Zone = DropDownList3.SelectedValue;
                dd_ExpressCenter.Items.Clear();
                dd_ridercode.Items.Clear();
                DataTable dt1 = ExpressCenter(clvar);

                if (dt1.Rows.Count > 0)
                {
                    dd_ExpressCenter.Text = "";
                    dd_ExpressCenter.DataTextField = "Name";
                    dd_ExpressCenter.DataValueField = "expressCenterCode";
                    dd_ExpressCenter.DataSource = dt1.DefaultView;
                    dd_ExpressCenter.DataBind();
                }
                else
                {
                    dd_ExpressCenter.Items.Clear();
                }
                dt1 = null;

                dd_ridercode.Items.Clear();
                dt1 = Rider_(clvar);
                if (dt1.Rows.Count > 0)
                {
                    dd_ridercode.Text = "";

                    dd_ridercode.DataTextField = "Name";
                    dd_ridercode.DataValueField = "riderCode";
                    dd_ridercode.DataSource = dt1.DefaultView;
                    dd_ridercode.DataBind();
                }
                else
                {
                    dd_ridercode.Items.Clear();
                }

                if (DropDownList3.SelectedValue == "0")
                {
                    clvar.CheckCondition = "";
                }
                else
                {
                    clvar.CheckCondition = " AND mzc.zoneCode = '" + DropDownList3.SelectedValue + "'";
                }
                if (DropDownList4.SelectedValue == "0")
                {
                    clvar.CheckCondition += "";
                }
                else
                {
                    clvar.CheckCondition += " AND mzc.Branch = '" + DropDownList4.SelectedValue + "'";
                }

                // To remain on the same tab
                ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('billingNsales');", true);

            }
            else
            {
                Cl_Variables clvar = new Cl_Variables();
                clvar.Branch = DropDownList4.SelectedValue;
                clvar.Zone = DropDownList3.SelectedValue;
                dd_ExpressCenter.Items.Clear();
                dd_ridercode.Items.Clear();
                DataTable dt1 = in1.ExpressCenter(clvar);
                if (dt1.Rows.Count > 0)
                {
                    dd_ExpressCenter.DataTextField = "Name";
                    dd_ExpressCenter.DataValueField = "expressCenterCode";
                    dd_ExpressCenter.DataSource = dt1.DefaultView;
                    dd_ExpressCenter.DataBind();
                }
                else
                {
                    dd_ExpressCenter.Items.Clear();
                }
                dt1 = null;

                dd_ridercode.Items.Clear();
                dt1 = Rider(clvar);
                if (dt1.Rows.Count > 0)
                {
                    dd_ridercode.DataTextField = "Name";
                    dd_ridercode.DataValueField = "riderCode";
                    dd_ridercode.DataSource = dt1.DefaultView;
                    dd_ridercode.DataBind();
                }
                else
                {
                    dd_ridercode.Items.Clear();
                }

                if (DropDownList3.SelectedValue == "0")
                {
                    clvar.CheckCondition = "";
                }
                else
                {
                    clvar.CheckCondition = " AND mzc.zoneCode = '" + DropDownList3.SelectedValue + "'";
                }
                if (DropDownList4.SelectedValue == "0")
                {
                    clvar.CheckCondition += "";
                }
                else
                {
                    clvar.CheckCondition += " AND mzc.Branch = '" + DropDownList4.SelectedValue + "'";
                }

                // To remain on the same tab
                ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:Myfunc('billingNsales');", true);

            }
        }


        public DataTable ExpressCenter(Cl_Variables clvar)
        {
            string sql = "SELECT ec.expressCenterCode, \n"
               + "       ec.expressCenterCode + ' - ' + ec.name NAME \n"
               + "FROM   Zones              z, \n"
               + "       Branches           b, \n"
               + "       ExpressCenters     ec \n"
               + "WHERE  b.zoneCode = z.zoneCode \n"
               + "       AND ec.bid = b.branchCode \n"
               + "       AND b.zoneCode = '" + clvar.Zone + "' \n"
               + "       AND b.branchCode = '" + clvar.Branch + "' and ec.status ='1' and Main_EC ='1'\n"
               + "ORDER BY \n"
               + "       ec.name \n"
               + "";

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sql, con);
                sda.Fill(dt);

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return dt;
        }
    }
}