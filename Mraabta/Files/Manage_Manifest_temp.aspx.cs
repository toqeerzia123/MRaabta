using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Services;
using MRaabta.App_Code;
using Dapper;

namespace MRaabta.Files
{
    public partial class Manage_Manifest_temp : System.Web.UI.Page
    {
        Cl_Variables clvar = new Cl_Variables();
        Consignemnts con = new Consignemnts();
        CommonFunction fun = new CommonFunction();
        cl_Encryption enc = new cl_Encryption();
        Cl_Variables clvar_ = new Cl_Variables();

        public class ManifestModel
        {
            public string ManifestNumber { get; set; }       //
            public string origin { get; set; }           //
            public string Destination { get; set; }             //
            public string ServiceType { get; set; }             //
            public string Type { get; set; }             //
            public string serverResponse { get; set; }
            public float Weight { get; set; }             //
            public int Pieces { get; set; }
        }

        public class CNtModel
        {
            public string ConsignmentNumber { get; set; }       //
            public string ManifestNumber { get; set; }       //
            public string StatusCode { get; set; }           //
            public string weight { get; set; }           //
            public string Pieces { get; set; }           //
            public string serverResponse { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            txt_date.Enabled = false;
            ErrorID.Text = "";
            if (!IsPostBack)
            {

                string rb_1 = rbtn_search.ClientID;
                hd_1.Value = rbtn_search.SelectedValue;
                GetOrigin();
                GetDestination();
                GetServiceTypes();
                Get_PrefixCheck();

                txt_date.Text = DateTime.Now.ToString("yyyy-MM-dd");

                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[]
                {
                    new DataColumn("ConsignmentNumber"),
                    new DataColumn("Orgin"),
                    new DataColumn("OriginName"),
                    new DataColumn("Destination"),
                    new DataColumn("DestinationName"),
                    new DataColumn("ConType"),
                    new DataColumn("ServiceTypeName"),
                    new DataColumn("Weight"),
                    new DataColumn("Pieces"),
                    new DataColumn("ISMODIFIED"),
                    new DataColumn("Order")

                });
                dt.AcceptChanges();
                ViewState["temp"] = null;
                ViewState["temp"] = dt;
                ViewState["types"] = fun.ConsignmentType().Tables[0];
            }
        }

        protected void GetOrigin()
        {
            if (Session["BranchCode"] != null)
            {
                DataTable dt = new DataTable();



                dt = fun.Branch().Tables[0];

                DataView dv = dt.AsDataView();
                dv.Sort = "BranchName";
                dt = dv.ToTable();
                dd_origin.DataSource = dv;
                dd_origin.DataTextField = "BranchName";
                dd_origin.DataValueField = "branchCode";
                if (Session["BranchCode"].ToString() == "ALL")
                {
                    dd_origin.Enabled = true;
                }
                else
                {
                    dd_origin.Enabled = false;
                    try
                    {
                        dd_origin.SelectedValue = Session["BranchCode"].ToString();
                    }
                    catch (Exception ex)
                    {
                        Response.Redirect("~/login");
                    }
                }
                dd_origin.DataBind();


            }
            else
            {
                Response.Redirect("~/login");
            }
        }
        protected void GetDestination()
        {
            DataTable dt = Cities_();
            //dt = fun.Branch().Tables[0];
            //DataView dv = dt.AsDataView();
            //dt = dv.ToTable();
            //dv.Sort = "BranchName";
            if (dt.Rows.Count > 0)
            {
                dd_destination.DataSource = dt;
                dd_destination.DataTextField = "BRANCHNAME";
                dd_destination.DataValueField = "branchCode";
                dd_destination.DataBind();
            }



            ViewState["destinations"] = dt;
        }
        protected void GetServiceTypes()
        {
            DataTable dt = new DataTable();
            dt = fun.ServiceTypeNameRvdbo();
            dd_serviceType.DataSource = dt;
            dd_serviceType.DataTextField = "ServiceTypeName";
            dd_serviceType.DataValueField = "ServiceTypeID";
            dd_serviceType.DataBind();
            dd_serviceType.SelectedValue = "overnight";
            ViewState["serviceTypes"] = dt;

        }
        protected void btn_reset_Click(object sender, EventArgs e)
        {
            DataTable dt = ViewState["temp"] as DataTable;
            dt.Clear();

            rbtn_search.SelectedValue = "1";

            txt_consignmentNo.Text = "";
            txt_date.Text = DateTime.Now.ToString("yyyy-MM-dd");
            txt_manifestNo.Text = "";
            dd_destination.ClearSelection();
            dd_serviceType.ClearSelection();

            //lbl_count.Text = "";
        }
        protected void btn_cancel_Click(object sender, EventArgs e)
        {

        }
        protected void txt_consignmentNo_TextChanged(object sender, EventArgs e)
        {

        }
        protected void gv_consignments_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {


                string con = e.Row.Cells[1].Text;

                DataTable dt = ViewState["temp"] as DataTable;
                DataRow dr = dt.Select("ConsignmentNumber = '" + con + "'", "")[0];
                DataTable conType = ViewState["types"] as DataTable;
                DataTable dest = ViewState["destinations"] as DataTable;
                DropDownList dd = e.Row.FindControl("dd_gorigin") as DropDownList;
                dd.DataSource = dest;
                dd.DataTextField = "BranchName";
                dd.DataValueField = "branchCode";
                dd.DataBind();
                dd.SelectedValue = dr["Orgin"].ToString();

                DropDownList dd_ = e.Row.FindControl("dd_contype") as DropDownList;
                dd_.DataSource = conType;
                dd_.DataTextField = "ConsignmentType";
                dd_.DataValueField = "id";
                dd_.DataBind();
                dd_.SelectedValue = dr["ConsignmentTypeID"].ToString();

                if ((e.Row.FindControl("hd_isModified") as HiddenField).Value == "DELETE")
                {
                    e.Row.Visible = false;
                }
            }
        }
        protected void gv_consignments_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "remove")
            {
                string con = e.CommandArgument.ToString();
                DataTable dt = ViewState["temp"] as DataTable;
                DataRow dr = dt.Select("ConsignmentNumber = '" + con + "'")[0];
                if (rbtn_search.SelectedValue == "3")
                {
                    dr["ISMODIFIED"] = "DELETE";
                }
                else
                {
                    dt.Rows.Remove(dr);
                }
                dt.AcceptChanges();
                ViewState["temp"] = dt;
                //lbl_count.Text = dt.Rows.Count.ToString();
                //gv_consignments.DataSource = dt;
                //gv_consignments.DataBind();
            }

            if (e.CommandName == "Update")
            {
            }


        }
        protected void rbtn_search_SelectedIndexChanged(object sender, EventArgs e)
        {
            //gv_consignments.DataSource = null;
            //gv_consignments.DataBind();

            if (rbtn_search.SelectedValue == "2")
            {
                btn_print.Visible = true;
                txt_manifestNo.Text = "";

            }
            else if (rbtn_search.SelectedValue == "3")
            {
                txt_manifestNo.Text = "";
                //   btn_save.Visible = true;
                btn_print.Visible = false;
            }
            else
            {
                btn_print.Visible = false;

                txt_manifestNo.Text = "";

            }
        }
        public void Get_PrefixCheck()
        {
            Cl_Variables clvar = new Cl_Variables();
            DataSet ds = new DataSet();

            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "Select  cast(Prefix as varchar) +'-'+ cast(Length as varchar) from MnP_ConsignmentLengths where STATUS ='1'";

                SqlDataAdapter sda = new SqlDataAdapter();
                sda.SelectCommand = cmd;
                sda.Fill(ds);
            }
            catch (Exception ex)
            { }
            finally { con.Close(); }

            string Prefix = "";
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                Prefix += ds.Tables[0].Rows[i][0].ToString() + ",";
            }
            Prefix = Prefix.Remove(Prefix.Length - 1);
            Hd_2.Value = Prefix;
        }


        [WebMethod]
        public static string[][] Get_ManifefstInformation(string ManifestNo, string Selection)
        {
            List<string[]> resp = new List<string[]>();

            if (Selection == "1")
            {
                string ManifestNo_ = ManifestNo;
                DataSet ds = GetManifestCheck(ManifestNo);

                if (ds != null)
                {
                    if (ds.Tables.Count != 0)
                    {
                        if (ds.Tables[0].Rows.Count != 0)
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                string[] Manifest = { "" };
                                Manifest[0] = dr[0].ToString();
                                resp.Add(Manifest);
                            }
                        }
                        else
                        {
                            string[] Manifest = { "" };
                            Manifest[0] = "N/A";
                            resp.Add(Manifest);
                        }
                    }
                    else
                    {
                        string[] Manifest = { "" };
                        Manifest[0] = "N/A";
                        resp.Add(Manifest);
                    }
                }
            }
            if (Selection == "2")
            {
                Cl_Variables clvar = new Cl_Variables();
                clvar.manifestNo = ManifestNo;
                DataTable dt = GetConsignmentDetailByManifestNumber(clvar);
                DataTable dt_ = GetManifestCheck(ManifestNo).Tables[0];

                if (dt != null)
                {
                    if (dt.Rows.Count != 0)
                    {
                        if (dt_.Rows.Count != 0)
                        {
                            string[] Manifest = { "", "", "", "", "" };
                            Manifest[0] = ManifestNo;
                            Manifest[1] = dt_.Rows[0][1].ToString();
                            Manifest[2] = dt_.Rows[0][4].ToString();
                            Manifest[3] = dt_.Rows[0][5].ToString();
                            Manifest[4] = DateTime.Parse(dt_.Rows[0][2].ToString()).ToString("yyyy-MM-dd");
                            resp.Add(Manifest);
                            // dt.Rows[0][0].ToString();
                            //     
                        }
                        foreach (DataRow dr in dt.Rows)
                        {
                            string[] Consignment = { "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
                            Consignment[0] = dr[0].ToString();
                            Consignment[1] = dr[1].ToString();
                            Consignment[2] = dr[2].ToString();
                            Consignment[3] = dr[3].ToString();
                            Consignment[4] = dr[4].ToString();
                            Consignment[5] = dr[5].ToString();
                            Consignment[6] = dr[6].ToString();
                            Consignment[7] = dr[7].ToString();
                            Consignment[8] = dr[8].ToString();
                            Consignment[9] = dr[9].ToString();
                            Consignment[10] = dr[10].ToString();
                            Consignment[11] = dr[11].ToString();
                            Consignment[12] = dr[12].ToString();
                            resp.Add(Consignment);
                        }

                    }
                }
            }
            if (Selection == "3")
            {
                Cl_Variables clvar = new Cl_Variables();
                clvar.manifestNo = ManifestNo;
                String origin = HttpContext.Current.Session["BRANCHCODE"].ToString();
                String status = GetDemanifestCheck(clvar);
                String OriginNotSame = getOriginOfManifest(clvar, origin);
                DataTable dt = GetConsignmentDetailByManifestNumber(clvar);
                DataTable dt_ = GetManifestCheck(ManifestNo).Tables[0];
                if (status != "0")
                {
                    return resp.ToArray();
                }
                if (OriginNotSame == "")
                {
                    string[] Manifest = { "" };
                    Manifest[0] = "Invalid Origin";
                    resp.Add(Manifest);
                    return resp.ToArray();
                }
                if (dt != null)
                {
                    if (dt.Rows.Count != 0)
                    {
                        if (dt_.Rows.Count != 0)
                        {
                            string[] Manifest = { "", "", "", "", "" };
                            Manifest[0] = ManifestNo;
                            Manifest[1] = dt_.Rows[0][1].ToString();
                            Manifest[2] = dt_.Rows[0][4].ToString();
                            Manifest[3] = dt_.Rows[0][5].ToString();
                            Manifest[4] = DateTime.Parse(dt_.Rows[0][2].ToString()).ToString("yyyy-MM-dd");
                            resp.Add(Manifest);
                            // dt.Rows[0][0].ToString();
                            //     
                        }
                        foreach (DataRow dr in dt.Rows)
                        {
                            string[] Consignment = { "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
                            Consignment[0] = dr[0].ToString();
                            Consignment[1] = dr[1].ToString();
                            Consignment[2] = dr[2].ToString();
                            Consignment[3] = dr[3].ToString();
                            Consignment[4] = dr[4].ToString();
                            Consignment[5] = dr[5].ToString();
                            Consignment[6] = dr[6].ToString();
                            Consignment[7] = dr[7].ToString();
                            Consignment[8] = dr[8].ToString();
                            Consignment[9] = dr[9].ToString();
                            Consignment[10] = dr[10].ToString();
                            Consignment[11] = dr[11].ToString();
                            Consignment[12] = dr[12].ToString();
                            resp.Add(Consignment);
                        }

                    }
                }


            }

            return resp.ToArray();
        }

        private static string getOriginOfManifest(Cl_Variables clvar, String origin)
        {

            SqlConnection con = new SqlConnection(clvar.Strcon());
            string status = "";
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT ORIGIN FROM Mnp_Manifest mm WHERE mm.manifestNumber = '" + clvar.manifestNo + "' AND mm.origin='" + origin + "'";

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        status = reader.GetString(0);
                    }
                }
                else
                {
                    status = "";
                }

            }
            catch (Exception ex)
            {
                status = "";
            }
            finally { con.Close(); }
            return status;
        }

        private static string GetDemanifestCheck(Cl_Variables clvar)
        {
            SqlConnection con = new SqlConnection(clvar.Strcon());
            string status = "";
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT Cast(ISNULL(isDemanifested, 0) as varchar) isDemanifested   FROM Mnp_Manifest  WHERE manifestNumber = '" + clvar.manifestNo + "'";

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        status = reader.GetString(0);
                    }
                }
                else
                {
                    status = "0";
                }

            }
            catch (Exception ex)
            {
                status = "1";
            }
            finally { con.Close(); }
            return status;
        }

        public static DataSet GetManifestCheck(string ManifestNo)
        {
            Cl_Variables clvar = new Cl_Variables();
            DataSet ds = new DataSet();

            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "Select  * from mnp_Manifest p where p.manifestNumber ='" + ManifestNo + "'";

                SqlDataAdapter sda = new SqlDataAdapter();
                sda.SelectCommand = cmd;
                sda.Fill(ds);
            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return ds;
        }


        [WebMethod]
        public static string RefreshTime(string a)
        {
            return DateTime.Now.ToString();
        }

        [WebMethod]
        public static DataTable GetConsignmentDetailByManifestNumber(Cl_Variables clvar)
        {

            string sql = " \n"
                   + "SELECT c.consignmentNumber, \n"
                   + "        '' consigner, \n"
                   + "        '' consignee, \n"
                   + "       c.weight, \n"
                   + "       mm.origin orgin, \n"
                   + "       b.name      OriginName, \n"
                   + "       mm.destination, \n"
                   + "       b2.name     DestinationName, \n"
                   + "       '12' consignmentTypeId, \n"
                   + "       mm.manifestType serviceTypeName, \n"
                   + "       c.weight, \n"
                   + "       c.pieces, \n"
                   + "       c.manifestNumber \n"
                   + "FROM MNP_ConsignmentManifest c INNER JOIN Mnp_Manifest mm ON mm.manifestNumber = c.manifestNumber \n"
                   + "       INNER JOIN Branches b \n"
                   + "            ON  mm.origin = b.branchCode \n"
                   + "       INNER JOIN Branches b2 \n"
                   + "            ON  mm.destination = b2.branchCode \n"
                   + "WHERE  c.manifestNumber ='" + clvar.manifestNo + "'";
            DataTable dt = new DataTable();

            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sql, orcl);
                orcd.CommandType = CommandType.Text;
                orcd.CommandTimeout = 300;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dt);
                orcl.Close();
            }
            catch (Exception)
            { }

            return dt;
        }

        [WebMethod]
        public static DataTable GetManifesttDetailByManifestNumber(Cl_Variables clvar)
        {

            string sql = " \n"
                   + "SELECT * from Mnp_Manifest mm \n"
                   + "WHERE  mm.manifestNumber ='" + clvar.manifestNo + "'";
            DataTable dt = new DataTable();

            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sql, orcl);
                orcd.CommandType = CommandType.Text;
                orcd.CommandTimeout = 300;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dt);
                orcl.Close();
            }
            catch (Exception)
            { }

            return dt;
        }


        protected void txt_manifestNo_TextChanged(object sender, EventArgs e)
        {
            if (rbtn_search.SelectedValue == "2")
            {
                clvar.manifestNo = txt_manifestNo.Text;
                DataTable dt = con.GetConsignmentDetailByManifestNumber(clvar);
                ViewState["temp"] = dt;
                dt.Columns.Add("ORDER");
                //gv_consignments.Columns[8].Visible = false;
                //gv_consignments.DataSource = null;
                //gv_consignments.DataBind();
                //gv_consignments.DataSource = dt;
                //gv_consignments.DataBind();

            }
            else if (rbtn_search.SelectedValue == "1" || rbtn_search.SelectedValue == "3")
            {
                clvar.manifestNo = txt_manifestNo.Text;
                DataTable dt = con.GetConsignmentDetailByManifestNumber(clvar);
                ViewState["temp"] = dt;
                if (dt.Rows.Count > 0)
                {
                    //DataTable header = con.GetManifestHeader(clvar);
                    //txt_date.Text = header.Rows[0]["date"].ToString();
                    //dd_destination.SelectedValue = header.Rows[0]["DCODE"].ToString();
                    //dd_origin.SelectedValue = header.Rows[0]["OCODE"].ToString();
                    //foreach (ListItem item in dd_serviceType.Items)
                    //{
                    //    if (item.Text.ToUpper() == header.Rows[0]["manifestType"].ToString().ToUpper())
                    //    {
                    //        item.Selected = true;
                    //        break;
                    //    }
                    //}
                    // dd_serviceType.SelectedValue = header.Rows[0]["manifestType"].ToString().ToUpper();
                    int count = dt.Rows.Count;

                    dt.Columns.Add("ORDER");
                    dt.AcceptChanges();
                    foreach (DataRow dr in dt.Rows)
                    {
                        dr["Order"] = count--;
                    }

                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Manifest Already Exists')", true);

                    dt.AsDataView().Sort = "ORDER desc";
                    //gv_consignments.DataSource = null;
                    //gv_consignments.DataBind();
                    //gv_consignments.DataSource = dt;
                    //gv_consignments.DataBind();
                    //gv_consignments.Columns[0].Visible = false;
                    //gv_consignments.Columns[8].Visible = false;
                    // gv_consignments.Visible = true;
                    DataTable header = con.GetManifestHeader(clvar);
                    txt_date.Text = header.Rows[0]["date"].ToString();
                    dd_destination.SelectedValue = header.Rows[0]["DCODE"].ToString();
                    dd_origin.SelectedValue = header.Rows[0]["OCODE"].ToString();

                    for (int i = 0; i < dd_serviceType.Items.Count; i++)
                    {
                        if (dd_serviceType.Items[i].Text.ToUpper() == header.Rows[0]["manifestType"].ToString().ToUpper())
                        {
                            dd_serviceType.SelectedValue = dd_serviceType.Items[i].Value;
                            break;
                        }
                    }
                    if (rbtn_search.SelectedValue == "3")
                    {
                        //  gv_consignments.Columns[0].Visible = true;
                    }
                    else
                    {
                        // gv_consignments.Columns[0].Visible = false;
                        // gv_consignments.Columns[8].Visible = false;

                    }
                }
                else
                {
                }

            }
        }


        protected void btn_print_Click(object sender, EventArgs e)
        {
            string script_ = @"window.open(""{0}"", ""{1}"", ""{2}"");";
            string script = String.Format(script_, "Manifest_Print.aspx?Xcode=" + txt_manifestNo.Text, "_blank", "");
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Redirect", script, true);
        }
        protected void chk_weight_CheckedChanged(object sender, EventArgs e)
        {

        }
        protected void chk_pieces_CheckedChanged(object sender, EventArgs e)
        {

        }
        protected void dd_gorigin_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList origin = (DropDownList)sender;
            GridViewRow row = (GridViewRow)origin.Parent.Parent;

            DataTable dt = ViewState["temp"] as DataTable;

            string cn = row.Cells[1].Text;
            DataRow dr = dt.Select("ConsignmentNumber = '" + cn + "'", "")[0];
            dr["Orgin"] = origin.SelectedValue;
            dr["OriginName"] = origin.SelectedItem.Text;
            dr.AcceptChanges();
            if (origin.SelectedValue == "0")
            {
                row.Cells[2].BackColor = System.Drawing.Color.DarkGray;
            }
            else
            {
                row.Cells[2].BackColor = System.Drawing.Color.White;
            }
        }
        public DataTable Cities_()
        {

            string sqlString = "SELECT CAST(A.bid as Int) bid, A.description NAME, A.expressCenterCode, A.CategoryID\n" +
            "  FROM (SELECT e.expresscentercode, e.bid, e.description, e.CategoryId\n" +
            "          FROM ServiceArea a\n" +
            "         right outer JOIN ExpressCenters e\n" +
            "            ON e.expressCenterCode = a.Ec_code where e.bid <> '17' and e.status = '1') A\n" +
            " GROUP BY A.bid, A.description, A.expressCenterCode, A.CategoryID\n" +
            " ORDER BY 2";

            sqlString = "";

            sqlString = "select b.branchCode , b.sname + ' - ' + b.name BRANCHNAME, b.branchCode\n" +
            "  from branches b\n" +
            " where b.status = '1'\n" +
            " order by 2";

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
        public void rbtn_search_SelectedIndexChanged1(object sender, EventArgs e)
        {
            hd_1.Value = rbtn_search.SelectedValue;
            txt_manifestNo.Focus();
            if (rbtn_search.SelectedValue == "2")
            {
                btn_print.Visible = true;
            }
            else
            {
                btn_print.Visible = false;
            }


        }

        [WebMethod]
        public static string InsertManifest(ManifestModel manifest, CNtModel[] consignments)
        {
            string status = "";
            if (manifest.Type == "1")
            {
                DataTable dt_consignment = new DataTable();
                dt_consignment.Columns.AddRange(new DataColumn[] {
        new DataColumn("ConsignmentNumber", typeof(string)),
        new DataColumn("Manifest", typeof(string)),
        new DataColumn("Weight", typeof(string)),
        new DataColumn("Pieces", typeof(string))
        });



                Cl_Variables clvar = new Cl_Variables();
                clvar.manifestNo = manifest.ManifestNumber;
                clvar.origin = manifest.origin;
                clvar.destination = manifest.Destination;
                clvar.ServiceTypeName = manifest.ServiceType;
                clvar.deliveryDate = DateTime.Now;
                clvar.pieces = manifest.Pieces;
                clvar.Weight = manifest.Weight;

                // Consignment Logic;
                if (consignments.Length > 0)
                {
                    foreach (CNtModel cn in consignments)
                    {
                        DataRow dr = dt_consignment.NewRow();
                        dr["ConsignmentNumber"] = cn.ConsignmentNumber;
                        dr["Manifest"] = cn.ManifestNumber;
                        dr["Weight"] = cn.weight;
                        dr["Pieces"] = cn.Pieces;

                        dt_consignment.Rows.Add(dr);
                    }
                }
                status = GenerateManifest_New(clvar, dt_consignment);
            }
            if (manifest.Type == "3")
            {
                DataTable dt_consignment = new DataTable();
                dt_consignment.Columns.AddRange(new DataColumn[] {
                new DataColumn("ConsignmentNumber", typeof(string)),
                new DataColumn("Manifest", typeof(string)),
                new DataColumn("Weight", typeof(string)),
                new DataColumn("Pieces", typeof(string))
            });



                Cl_Variables clvar = new Cl_Variables();
                clvar.manifestNo = manifest.ManifestNumber;
                clvar.origin = manifest.origin;
                clvar.destination = manifest.Destination;
                clvar.ServiceTypeName = manifest.ServiceType;
                clvar.deliveryDate = DateTime.Now;
                clvar.pieces = manifest.Pieces;
                clvar.Weight = manifest.Weight;

                // Consignment Logic;
                if (consignments.Length > 0)
                {
                    foreach (CNtModel cn in consignments)
                    {
                        DataRow dr = dt_consignment.NewRow();
                        dr["ConsignmentNumber"] = cn.ConsignmentNumber;
                        dr["Manifest"] = cn.ManifestNumber;
                        dr["Weight"] = cn.weight;
                        dr["Pieces"] = cn.Pieces;
                        dt_consignment.Rows.Add(dr);
                    }
                }
                //Delete Manifest
                //Delete_Manifest(clvar);
                //Insert Manifest
                status = EditManifest_New(clvar, dt_consignment);
            }
            if (status == "OK")
            {
                //Files_Manage_Manifest a = new Files_Manage_Manifest();
                //a.rbtn_search.SelectedValue = "1";
                //a.hd_1.Value = "1";
            }

            return status.ToString();
        }

        public static void Delete_Manifest(Cl_Variables clvar)
        {
            SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            sqlcon.Open();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcon;
            sqlcmd.CommandType = CommandType.Text;
            int count = 0;
            try
            {
                //Consignment Manifest
                string query = " Delete p from mnp_consignmentManifest p where p.manifestNumber = '" + clvar.manifestNo + "'";
                sqlcmd.CommandText = query;
                count = sqlcmd.ExecuteNonQuery();
                //Manifest
                query = " Delete p from mnp_Manifest p where p.manifestNumber = '" + clvar.manifestNo + "'";
                sqlcmd.CommandText = query;
                count = sqlcmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                count = 0;
            }
            finally
            {
                sqlcon.Close();
            }

        }

        public static string GenerateManifest(Cl_Variables clvar, DataTable dt)
        {
            SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            SqlTransaction trans;
            sqlcon.Open();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcon;
            trans = sqlcon.BeginTransaction();
            sqlcmd.Transaction = trans;
            sqlcmd.CommandType = CommandType.Text;
            int count = 0;
            try
            {
                CommonFunction func = new CommonFunction();
                clvar.Branch = HttpContext.Current.Session["BranchCode"].ToString();
                string branchName = func.Branch(clvar).Tables[0].Rows[0]["BranchName"].ToString();

                string query = "insert into MNP_Manifest (manifestNumber, origin, destination, branchCode, zoneCode, manifestType, date, createdBy, createdOn, isTemp)\n" +
                               " VALUES ( \n" +
                               "'" + clvar.manifestNo + "',\n" +
                               "'" + clvar.origin + "',\n" +
                               "'" + clvar.destination + "',\n" +
                               "'" + HttpContext.Current.Session["BranchCode"].ToString() + "',\n" +
                               "'" + HttpContext.Current.Session["ZONECODE"].ToString() + "',\n" +
                               "'" + clvar.ServiceTypeName + "',\n" +
                               "'" + clvar.deliveryDate.ToString("yyyy-MM-dd") + "',\n" +
                               "'" + HttpContext.Current.Session["U_ID"].ToString() + "',\n" +
                               " GETDATE() ,\n" +
                               "'0'\n" +
                               ")";
                string query_ = "Insert into rvdbo.manifest (ManifestID, ManifestNo , ServiceTypeID, ManifestDate, ZoneID, OriginBranchID, DestBranchID,  isTemp,  isDemanifested) " +
                                "Values " +
                                "(" +
                                "'" + clvar.manifestNo.TrimStart('0') + "',\n" +
                                "'" + clvar.manifestNo + "',\n" +
                                "'" + clvar.serviceTypeId + "',\n" +
                                "'" + clvar.deliveryDate.ToString("yyyy-MM-dd") + "',\n" +
                                "'" + HttpContext.Current.Session["ZONECODE"].ToString() + "',\n" +
                                "'" + clvar.origin + "',\n" +
                                "'" + clvar.destination + "', '0', '0')";
                string query1 = "Insert into rvdbo.Manifestconsignment (manifestID, ConsignmentID) ";

                sqlcmd.CommandText = query;
                count = sqlcmd.ExecuteNonQuery();
                if (count == 0)
                {
                    trans.Rollback();
                    count = 0;
                    return "NOT OK";
                }


                string query1_ = "INSERT INTO MNP_ConsignmentManifest (manifestNumber,consignmentNumber,weight,pieces,ismergerd) \n";
                for (int i = 0; i < dt.Rows.Count - 1; i++)
                {
                    query1 += "SELECT '" + clvar.manifestNo + "', '" + dt.Rows[i][0].ToString() + "' \n UNION ALL \n";
                    query1_ += "SELECT '" + clvar.manifestNo + "','" + dt.Rows[i][0].ToString() + "', '" + dt.Rows[i]["weight"].ToString() + "', '" + dt.Rows[i]["pieces"].ToString() + "','0' \n UNION ALL \n";

                }
                int j = dt.Rows.Count - 1;
                query1 += "SELECT '" + clvar.manifestNo + "', '" + dt.Rows[j][0].ToString() + "'";
                query1_ += "SELECT '" + clvar.manifestNo + "', '" + dt.Rows[j][0].ToString() + "', '" + dt.Rows[j]["weight"].ToString() + "', '" + dt.Rows[j]["pieces"].ToString() + "','0'\n";


                //SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
                //SqlTransaction trans;
                //sqlcon.Open();
                //SqlCommand sqlcmd = new SqlCommand();
                //sqlcmd.Connection = sqlcon;
                //trans = sqlcon.BeginTransaction();
                //sqlcmd.Transaction = trans;
                //sqlcmd.CommandType = CommandType.Text;

                //sqlcmd.CommandText = query_;
                //count = sqlcmd.ExecuteNonQuery();
                sqlcmd.CommandText = query1_;
                count = sqlcmd.ExecuteNonQuery();
                if (count == 0)
                {
                    trans.Rollback();
                    count = 0;
                    return "NOT OK";
                }
                //sqlcmd.CommandText = query1_;
                //sqlcmd.ExecuteNonQuery();
                trans.Commit();
                // trans.Rollback();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                count = 0;
                return ex.Message;
            }
            finally
            {
                sqlcon.Close();
            }

            return "OK";
        }

        public static string GenerateManifest_New(Cl_Variables clvar, DataTable dt)
        {
            string resp = "";

            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "MnP_GenerateManifest_1";
                cmd.Parameters.AddWithValue("@tblDetails", dt);
                cmd.Parameters.AddWithValue("@ManifestNumber", clvar.manifestNo);
                cmd.Parameters.AddWithValue("@Origin", HttpContext.Current.Session["BranchCode"].ToString());
                cmd.Parameters.AddWithValue("@destination", clvar.destination);
                cmd.Parameters.AddWithValue("@branchcode", HttpContext.Current.Session["BranchCode"].ToString());
                cmd.Parameters.AddWithValue("@zonecode", HttpContext.Current.Session["ZoneCode"].ToString());
                cmd.Parameters.AddWithValue("@ServiceType", clvar.ServiceTypeName);
                cmd.Parameters.AddWithValue("@UserID", HttpContext.Current.Session["U_ID"].ToString());
                cmd.Parameters.AddWithValue("@locationID", HttpContext.Current.Session["LocationID"].ToString());
                cmd.Parameters.AddWithValue("@locationName", HttpContext.Current.Session["LocationName"].ToString());
                cmd.Parameters.AddWithValue("@Weight", clvar.Weight);
                cmd.Parameters.AddWithValue("@Pieces", clvar.pieces);

                cmd.Parameters.Add("@result", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();

                resp = cmd.Parameters["@result"].SqlValue.ToString();
            }
            catch (Exception ex)
            { resp = ex.Message; }
            finally { con.Close(); }
            return resp;
        }
        public static string EditManifest_New(Cl_Variables clvar, DataTable dt)
        {
            string resp = "";

            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "MnP_EditManifest_1";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@tblDetails", dt);
                cmd.Parameters.AddWithValue("@ManifestNumber", clvar.manifestNo);
                cmd.Parameters.AddWithValue("@Origin", HttpContext.Current.Session["BranchCode"].ToString());
                cmd.Parameters.AddWithValue("@destination", clvar.destination);
                cmd.Parameters.AddWithValue("@branchcode", HttpContext.Current.Session["BranchCode"].ToString());
                cmd.Parameters.AddWithValue("@zonecode", HttpContext.Current.Session["ZoneCode"].ToString());
                cmd.Parameters.AddWithValue("@ServiceType", clvar.ServiceTypeName);
                cmd.Parameters.AddWithValue("@UserID", HttpContext.Current.Session["U_ID"].ToString());
                cmd.Parameters.AddWithValue("@locationName", HttpContext.Current.Session["LocationName"].ToString());
                cmd.Parameters.AddWithValue("@Weight", clvar.Weight);
                cmd.Parameters.AddWithValue("@Pieces", clvar.pieces);

                cmd.Parameters.Add("@result", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();

                resp = cmd.Parameters["@result"].SqlValue.ToString();
            }
            catch (Exception ex)
            { resp = ex.Message; }
            finally { con.Close(); }
            return resp;
        }

        [WebMethod]
        public static string[][] ChkRunsheet(string cn)
        {
            List<string[]> resp = new List<string[]>();

            string ConsignmentNo = cn;
            string reason = "";
            DataTable ds = chkRunsheet(ConsignmentNo);

            if (ds.Rows.Count > 0)
            {
                string[] consignment = { "", "" };
                reason = ds.Rows[0]["Reason"].ToString();
                if (reason == "59")
                {
                    consignment[0] = reason;
                    consignment[1] = "RS-Return to Shipper";
                    resp.Add(consignment);
                }
                else if (reason == "123")
                {
                    consignment[0] = reason;
                    consignment[1] = "D-DELIVERED";
                    resp.Add(consignment);

                }

            }
            else
            {
                string[] consignment = { "" };
                consignment[0] = "N/A";
                resp.Add(consignment);


            }


            return resp.ToArray();
        }

        private static DataTable chkRunsheet(string Consignment)
        {
            Cl_Variables clvar = new Cl_Variables();
            string query = "select * from RunsheetConsignment where Reason in ('59','123') and consignmentnumber = '" + Consignment + "'";
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                sda.Fill(dt); ;
            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return dt;
        }

        [WebMethod]
        public static string[][] ControlsCheck(string cn)
        {
            List<string[]> resp = new List<string[]>();
            string[] Response = { "", "" };

            string ConsignmentNo = cn;
            string Reason = "";
            #region Primary Check By Fahad 12-oct-2020
            var rs = PrimaryCheck(cn);
            if (!string.IsNullOrEmpty(rs))
            {
                Response[0] = "false";
                Response[1] = rs;
                resp.Add(Response);
                return resp.ToArray();
            }
            #endregion

            #region RTS/DLV Check by Talha 23-oct-2020
            var rss = alreadyRTS_DLV(cn);
            if (!string.IsNullOrEmpty(rss))
            {
                Response[0] = "false";
                Response[1] = rss;
                resp.Add(Response);
                return resp.ToArray();
            }
            #endregion

            if (cn.StartsWith("5") && cn.Length == 15)
            {
                DataTable BookingDT = CheckConsignmentBooking(ConsignmentNo);
                DataTable FirstProcessDT = CheckFirstProcessOrigin(ConsignmentNo);
                string status = "true";
                if (BookingDT.Rows.Count > 0)
                {
                    if (BookingDT.Rows[0]["bypass"].ToString() == "0")
                    {
                        //if (HttpContext.Current.Session["BranchCode"].ToString() != BookingDT.Rows[0]["destination"].ToString() && BookingDT.Rows[0]["AtDest"].ToString() == "1" && BookingDT.Rows[0]["allowRTN"].ToString() == "0")
                        if (BookingDT.Rows[0]["AtDest"].ToString() == "1" && BookingDT.Rows[0]["allowRTN"].ToString() == "0")
                        {
                            status = "false";
                            Reason = "Alert: Once reached destination can only move with Return NCI";
                        }
                        else if (FirstProcessDT.Rows.Count > 0 || BookingDT.Rows[0]["isApproved"].ToString() == "True")
                        {
                            if (BookingDT.Rows[0]["status"].ToString() == "9")
                            {
                                status = "false";
                                Reason = "Alert: Consignment is Void perform Arrival";
                            }
                            else
                            {
                                status = "true";
                                Reason = "";
                            }
                        }
                        else
                        {
                            status = "false";
                            Reason = "Alert: First Process Must be at orign";
                        }
                    }
                    Response[0] = status;
                    Response[1] = Reason;
                    resp.Add(Response);
                }
                else
                {
                    Response[0] = "false";
                    Response[1] = "Alert: no booking found for this COD CN";
                    resp.Add(Response);
                }
            }
            else
            {
                Response[0] = "true";
                Response[1] = "";
                resp.Add(Response);
            }
            return resp.ToArray();
        }

        private static DataTable CheckConsignmentBooking(string Consignment)
        {
            Cl_Variables clvar = new Cl_Variables();
            //string query = @"SELECT * FROM Consignment c 
            //inner join (select consignmentnumber, sum(AtDest) as AtDest, sum(allowRTN) as allowRTN from (
            //select '" + Consignment + @"' consignmentnumber, 0 AtDest, 0 allowRTN union
            //select consignmentnumber, case when Reason in ('70', '58') then 0 else 1 end AtDest, 0 as allowRTN from RunsheetConsignment where consignmentNumber = '" + Consignment + @"' 
            //and createdOn = (select max(createdon) from RunsheetConsignment where consignmentNumber = '" + Consignment + @"' ) union
            //select consignmentnumber, 0 as AtDest, case when COUNT(*) > 0 then 1 else 0 end as allowRTN 
            //from MNP_NCI_Request where calltrack = 2 and consignmentnumber ='" + Consignment + @"' group by consignmentnumber) xb
            //group by consignmentnumber) xxb on xxb.consignmentNumber = c.consignmentNumber
            //WHERE c.consignmentNumber = '" + Consignment + "'";
            string query = @"SELECT c.consignmentNumber, (select count(*) from tbl_CODControlBypass where isactive = 1 and consignmentNumber = '" + Consignment + @"') bypass, c.consigner, c.consignee, c.couponNumber, c.customerType, c.orgin, isnull(xxb.branchcode, c.destination) as destination, c.pieces, c.serviceTypeName, c.creditClientId, c.weight, c.cod, c.address, c.status, c.totalAmount, c.zoneCode, c.branchCode, c.shipperAddress, c.transactionNumber, c.otherCharges, c.routeCode, c.docPouchNo, c.consignerPhoneNo, c.consignerCellNo, c.consignerCNICNo, c.consignerAccountNo, c.consignerEmail, c.docIsHomeDelivery, c.cutOffTime, c.destinationCountryCode, c.decalaredValue, c.insuarancePercentage, c.consignmentScreen, c.isInsured, c.isReturned, c.consigneeCNICNo, c.cutOffTimeShift, c.bookingDate, c.cnClientType, c.syncState, c.syncId, c.destinationExpressCenterCode, c.isApproved, c.deliveryStatus, c.dayType, c.originExpressCenter, c.isPriceComputed, c.isNormalTariffApplied, c.receivedFromRider, c.chargedAmount, c.misRouted, c.accountReceivingDate, c.IsInvoiced, c.ispayable, c.CorrectCN, c.cnReason, c.Region, c.DestinationZone, c.ZoningCriteria, c.Zoning, c.DenseWeight, c.Zoning_Criteria_origin, c.Zoning_origin, c.statusSync, c.paidon, c.InstrumentMode, c.InstrumentNumber, c.ConsignerCostCenter, c.Address2, c.Phone2, c.PackageContent2, c.InsertType, c.DiscountID, c.DiscountApplied, c.DiscountGST, c.VolWeight, c.origin_country, c.import, c.locationID, xxb.* FROM Consignment c 
            inner join (select consignmentnumber, max(branchcode) as branchcode, sum(AtDest) as AtDest, sum(allowRTN) as allowRTN from (
            select '" + Consignment + @"' consignmentnumber, '' branchcode, 0 AtDest, 0 allowRTN union
            select consignmentnumber, branchcode, case when Reason in ('70', '58') then 0 else 1 end AtDest, 0 as allowRTN from RunsheetConsignment where consignmentNumber = '" + Consignment + @"' 
            and createdOn = (select max(createdon) from RunsheetConsignment where consignmentNumber = '" + Consignment + @"' ) union
            select consignmentnumber, '' branchcode, 0 as AtDest, case when COUNT(*) > 0 then 1 else 0 end as allowRTN 
            from MNP_NCI_Request where calltrack = 2 and consignmentnumber ='" + Consignment + @"' group by consignmentnumber) xb
            group by consignmentnumber) xxb on xxb.consignmentNumber = c.consignmentNumber
            WHERE c.consignmentNumber = '" + Consignment + "'";
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


        private static DataTable CheckFirstProcessOrigin(string Consignment)
        {
            Cl_Variables clvar = new Cl_Variables();
            string query = " select * from consignment where consignmentNumber = '" + Consignment + "' and orgin = '" + HttpContext.Current.Session["BranchCode"].ToString() + "' ";
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

        public static string PrimaryCheck(string cn)
        {
            Cl_Variables clvar = new Cl_Variables();
            using (SqlConnection con = new SqlConnection(clvar.Strcon()))
            {
                try
                {
                    var query = $"select 'Consignment already exist in archive database' AS Msg from primaryconsignments where isManual = 1 AND consignmentnumber = '{cn}'";
                    con.Open();
                    var rs = con.QueryFirstOrDefault<string>(query);
                    con.Close();
                    return rs;
                }
                catch (SqlException ex)
                {
                    con.Close();
                    return null;
                }
                catch (Exception ex)
                {
                    con.Close();
                    return null;
                }
            }
        }

        public static string alreadyRTS_DLV(string cn)
        {
            Cl_Variables clvar = new Cl_Variables();
            using (SqlConnection con = new SqlConnection(clvar.Strcon()))
            {
                try
                {
                    var query = $"select top(1) 'Consignment already Marked Delivered or Returned' AS Msg from Mnp_ConsignmentOperations where ConsignmentId = '{cn}' and (IsDelivered = 1 or IsReturned = 1) ";
                    con.Open();
                    var rs = con.QueryFirstOrDefault<string>(query);
                    con.Close();
                    return rs;
                }
                catch (SqlException ex)
                {
                    con.Close();
                    return null;
                }
                catch (Exception ex)
                {
                    con.Close();
                    return null;
                }
            }
        }
    }
}