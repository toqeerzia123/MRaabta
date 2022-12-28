using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Configuration;
using MRaabta.App_Code;
using System.Data.SqlClient;
using System.Drawing;

namespace MRaabta.Files
{
    public partial class MegaDestCorrection : System.Web.UI.Page
    {
        Cl_Variables clvar = new Cl_Variables();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
        }
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile)
            {
                string FileName = Path.GetFileName(FileUpload1.PostedFile.FileName);
                string Extension = Path.GetExtension(FileUpload1.PostedFile.FileName);
                string FolderPath = ConfigurationManager.AppSettings["FolderPath"];

                string FilePath = Server.MapPath(FolderPath + FileName);
                FileUpload1.SaveAs(FilePath);
                Import_To_Grid(FilePath, Extension, rbHDR.SelectedItem.Text);
            }
        }
        private void Import_To_Grid(string FilePath, string Extension, string isHDR)
        {
            string conStr = "";
            switch (Extension)
            {
                case ".xls": //Excel 97-03
                    conStr = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                    break;
                case ".xlsx": //Excel 07
                    conStr = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                    break;
            }
            conStr = String.Format(conStr, FilePath, isHDR);
            OleDbConnection connExcel = new OleDbConnection(conStr);
            OleDbCommand cmdExcel = new OleDbCommand();
            OleDbDataAdapter oda = new OleDbDataAdapter();
            DataTable dt = new DataTable();
            cmdExcel.Connection = connExcel;
            try
            {

                //Get the name of First Sheet
                connExcel.Open();
                DataTable dtExcelSchema;
                dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                connExcel.Close();

                //Read Data from First Sheet
                connExcel.Open();
                cmdExcel.CommandText = "SELECT distinct left(ConsignmentNo, 4) From [" + SheetName + "] where left(ConsignmentNo, 4) not in ('8600','8601','8602','8610','8611','8612','8650','8655','1310')";
                oda.SelectCommand = cmdExcel;
                oda.Fill(dt);
                connExcel.Close();

                connExcel.Open();
                cmdExcel.CommandText = "SELECT * From [" + SheetName + "]";
                oda.SelectCommand = cmdExcel;
                oda.Fill(dt);
                connExcel.Close();
                int i = 0, j = 0;
                foreach (var row in dt.Rows)
                {
                    string sqlinsert = @"update consignment set destination = (select branchCode from Branches where sname = '" + ((System.Data.DataRow)row).ItemArray[1].ToString() + "' and status = 1) where consignmentNumber = '" + ((System.Data.DataRow)row).ItemArray[0].ToString() + @"' 
and consignerAccountNo in ('4H618','4H619','4H620','4A1013','4A1014','4A1015','4A1018','4A1019','4A1020','4H623','4H624','4H625','4B483') 
and (select count(*) from MnP_ConsignmentLengths where Product = 'Mega' and status = 1 and Prefix = left('" + ((System.Data.DataRow)row).ItemArray[0].ToString() + @"', 4)) = 1
and (select count(branchCode) from Branches where sname = '" + ((System.Data.DataRow)row).ItemArray[1].ToString() + "' and status = 1) = 1";
                    try
                    {
                        i++; j++;
                        SqlConnection con1 = new SqlConnection(clvar.Strcon());
                        SqlCommand cmd1 = new SqlCommand();
                        cmd1.Connection = con1;
                        cmd1.CommandText = sqlinsert;
                        cmd1.CommandType = CommandType.Text;
                        con1.Open();
                        cmd1.ExecuteNonQuery();
                        con1.Close();
                    }
                    catch (Exception ex)
                    {
                        i--;
                        lb_result.Text = "Error in Row: " + j.ToString() + " " + ex.Message;
                        lb_result.ForeColor = Color.Red;
                    }
                }

                lb_result.Text = i.ToString() + "Rows Updated Successfully";
                lb_result.ForeColor = Color.Green;

            }
            catch (Exception)
            {

                throw;
            }
            //Bind Data to GridView
            //GridView1.Caption = Path.GetFileName(FilePath);
            //GridView1.DataSource = dt;
            //GridView1.DataBind();
        }
        protected void PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            string FolderPath = ConfigurationManager.AppSettings["FolderPath"];
            string FileName = GridView1.Caption;
            string Extension = Path.GetExtension(FileName);
            string FilePath = Server.MapPath(FolderPath + FileName);

            Import_To_Grid(FilePath, Extension, rbHDR.SelectedItem.Text);
            GridView1.PageIndex = e.NewPageIndex;
            GridView1.DataBind();
        }
        //protected void getmcat()
        //{
        //    string query = "SELECT MMC_ID, MMC_SDESC FROM VMM_MAINTENANCEMCATEGORY ORDER BY MMC_SDESC";
        //    SqlConnection con = new SqlConnection(clvar.Strcon());
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.Connection = con;
        //    cmd.CommandText = query;
        //    cmd.CommandType = CommandType.Text;
        //    DataSet ds = new DataSet();
        //    SqlDataAdapter adp = new SqlDataAdapter(cmd);
        //    con.Open();
        //    adp.Fill(ds);
        //    con.Close();
        //    if (ds.Tables[0].Rows.Count > 0)
        //    {
        //        dd_cat.DataSource = ds.Tables[0];

        //        dd_cat.DataTextField = "MMC_SDESC";
        //        dd_cat.DataValueField = "MMC_ID";
        //        dd_cat.DataBind();
        //        dd_cat.Items.Insert(0, new ListItem("Select Category", ""));
        //    }

        //}
        //protected void getamcat()
        //{
        //    string query = "SELECT MMC_ID, MMC_SDESC FROM VMM_MAINTENANCEMCATEGORY ORDER BY MMC_SDESC";
        //    SqlConnection con = new SqlConnection(clvar.Strcon());
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.Connection = con;
        //    cmd.CommandText = query;
        //    cmd.CommandType = CommandType.Text;
        //    DataSet ds = new DataSet();
        //    SqlDataAdapter adp = new SqlDataAdapter(cmd);
        //    con.Open();
        //    adp.Fill(ds);
        //    con.Close();
        //    if (ds.Tables[0].Rows.Count > 0)
        //    {
        //        dd_asgmcat.DataSource = ds.Tables[0];

        //        dd_asgmcat.DataTextField = "MMC_SDESC";
        //        dd_asgmcat.DataValueField = "MMC_ID";
        //        dd_asgmcat.DataBind();
        //        dd_asgmcat.Items.Insert(0, new ListItem("Select Category", ""));
        //    }

        //}
        //protected void getmscat()
        //{
        //    string query = "select * from vmm_maintenancescategory order by msc_sdesc;";
        //    SqlConnection con = new SqlConnection(clvar.Strcon());
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.Connection = con;
        //    cmd.CommandText = query;
        //    cmd.CommandType = CommandType.Text;
        //    DataSet ds = new DataSet();
        //    SqlDataAdapter adp = new SqlDataAdapter(cmd);
        //    con.Open();
        //    adp.Fill(ds);
        //    con.Close();
        //    if (ds.Tables[0].Rows.Count > 0)
        //    {
        //        dd_asgscat.DataSource = ds.Tables[0];

        //        dd_asgscat.DataTextField = "MsC_SDESC";
        //        dd_asgscat.DataValueField = "MsC_ID";
        //        dd_asgscat.DataBind();
        //        dd_asgscat.Items.Insert(0, new ListItem("Select Sub Category", ""));
        //    }

        //}
        //protected void rb_newcat_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (rb_newcat.Checked == true)
        //    {
        //        tr1.Visible = true;
        //        tr2.Visible = false;
        //        tr3.Visible = false;
        //        tb1.Visible = false;
        //    }
        //    else if (rb_newscat.Checked == true)
        //    {
        //        getmcat();
        //        tr2.Visible = true;
        //        tr3.Visible = true;
        //        tr1.Visible = false;
        //        tb1.Visible = false;
        //    }
        //    else
        //    {
        //        getamcat();
        //        getmscat();
        //        tr2.Visible = false;
        //        tr3.Visible = false;
        //        tr1.Visible = false;
        //        tb1.Visible = true;
        //    }
        //}
        //protected void rb_newscat_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (rb_newcat.Checked == true)
        //    {
        //        tr1.Visible = true;
        //        tr2.Visible = false;
        //        tr3.Visible = false;
        //        tb1.Visible = false;
        //    }
        //    else if (rb_newscat.Checked == true)
        //    {
        //        getmcat();
        //        tr2.Visible = true;
        //        tr3.Visible = true;
        //        tr1.Visible = false;
        //        tb1.Visible = false;
        //    }
        //    else
        //    {
        //        getamcat();
        //        getmscat();
        //        tr2.Visible = false;
        //        tr3.Visible = false;
        //        tr1.Visible = false;
        //        tb1.Visible = true;
        //    }
        //}
        //protected void btn_save_Click(object sender, EventArgs e)
        //{
        //    string sql = "";
        //    bool same = false;

        //    if (rb_newcat.Checked == true)
        //    {
        //        sql = "select max(CAST (mmc_id AS int)) from vmm_maintenancemcategory";
        //    }
        //    else
        //    {
        //        sql = "select max(CAST (msc_id AS int)) from vmm_maintenancescategory";
        //    }
        //    int id = 0;
        //    if (rb_assgn.Checked == false)
        //    {
        //        SqlConnection con = new SqlConnection(clvar.Strcon());
        //        SqlCommand cmd = new SqlCommand();
        //        cmd.Connection = con;
        //        cmd.CommandText = sql;
        //        cmd.CommandType = CommandType.Text;
        //        SqlDataAdapter adp = new SqlDataAdapter(cmd);
        //        DataSet ds = new DataSet();
        //        con.Open();
        //        adp.Fill(ds);
        //        con.Close();

        //        id = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());
        //        id++;
        //    }



        //    string sqlinsert = "";
        //    if (rb_newcat.Checked == true)
        //    {
        //        sqlinsert = "insert into vmm_maintenancemcategory (mmc_id,mmc_sdesc,mmc_ldesc,createdby,Createdon,modifyBy,modifyOn)\n" +
        //                     "values('" + id + "','" + txt_cname.Text + "','" + txt_cname.Text + "','" + Session["User_Info"].ToString() + "',getdate(),'','')";
        //    }
        //    else if (rb_newscat.Checked == true)
        //    {
        //        sqlinsert = "insert into vmm_maintenancescategory (mmc_id,msc_id,msc_sdesc,msc_ldesc,createdby,Createdon,modifyBy,modifyOn)\n" +
        //                     "values('" + dd_cat.SelectedValue + "','" + id + "','" + txt_scname.Text + "','" + txt_scname.Text + "','" + Session["User_Info"].ToString() + "',getdate(),'','')";
        //    }
        //    else
        //    {
        //        sqlinsert = "insert into vmm_maintenancescategory (mmc_id,msc_id,msc_sdesc,msc_ldesc,createdby,Createdon,modifyBy,modifyOn)\n" +
        //                    "values('" + dd_asgmcat.SelectedValue + "','" + dd_asgscat.SelectedValue + "','" + dd_asgscat.SelectedItem.ToString() + "','" + dd_asgscat.SelectedItem.ToString() + "','" + Session["User_Info"].ToString() + "',getdate(),'','')";

        //        string sqchk = "select * from vmm_maintenancescategory where mmc_id='" + dd_asgmcat.SelectedValue + "' and msc_id='" + dd_asgscat.SelectedValue + "'";

        //        SqlConnection con2 = new SqlConnection(clvar.Strcon());
        //        SqlCommand cmd2 = new SqlCommand();
        //        cmd2.Connection = con2;
        //        cmd2.CommandText = sqchk;
        //        cmd2.CommandType = CommandType.Text;
        //        SqlDataAdapter adp2 = new SqlDataAdapter(cmd2);
        //        DataSet ds2 = new DataSet();
        //        con2.Open();
        //        adp2.Fill(ds2);
        //        con2.Close();
        //        if (ds2.Tables[0].Rows.Count > 0)
        //        {
        //            same = true;
        //        }
        //    }

        //    if (same)
        //    {
        //        lb_result.Text = "Assignment already Exists";
        //        lb_result.ForeColor = Color.Red;
        //    }
        //    else
        //    {
        //        try
        //        {
        //            SqlConnection con1 = new SqlConnection(clvar.Strcon());
        //            SqlCommand cmd1 = new SqlCommand();
        //            cmd1.Connection = con1;
        //            cmd1.CommandText = sqlinsert;
        //            cmd1.CommandType = CommandType.Text;
        //            con1.Open();
        //            cmd1.ExecuteNonQuery();
        //            con1.Close();
        //            if (rb_newcat.Checked == true)
        //            {
        //                lb_result.Text = "Category Added";
        //                lb_result.ForeColor = Color.Red;
        //            }
        //            else
        //            {
        //                lb_result.Text = "Subcategory Added in Category:" + dd_cat.SelectedItem.ToString() + "";
        //                lb_result.ForeColor = Color.Red;
        //            }
        //        }
        //        catch (Exception ex)
        //        {

        //        }
        //    }


        //}
        //protected void rb_assgn_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (rb_newcat.Checked == true)
        //    {
        //        tr1.Visible = true;
        //        tr2.Visible = false;
        //        tr3.Visible = false;
        //        tb1.Visible = false;
        //    }
        //    else if (rb_newscat.Checked == true)
        //    {
        //        getmcat();
        //        tr2.Visible = true;
        //        tr3.Visible = true;
        //        tr1.Visible = false;
        //        tb1.Visible = false;
        //    }
        //    else
        //    {
        //        getmscat();
        //        getamcat();
        //        tr2.Visible = false;
        //        tr3.Visible = false;
        //        tr1.Visible = false;
        //        tb1.Visible = true;
        //    }
        //}
    }
}