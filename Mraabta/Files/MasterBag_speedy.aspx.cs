using MRaabta.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dapper;

namespace MRaabta.Files
{
    public partial class MasterBag_speedy : System.Web.UI.Page
    {
        Variable clvar = new Variable();
        bayer_Function b_fun = new bayer_Function();
        DataSet ds_grid = new DataSet();
        cl_Encryption enc = new cl_Encryption();
        CommonFunction CF = new CommonFunction();
        Consignemnts con = new Consignemnts();

        string manifestNumber, manifestType, origin, destination, OrignName, DestinationName, consignmentNumber;
        string BagNumber, BagId, BagDate, BagWeight, BagSeal, ConsignmentNo, serviceTypeName, consignmentTypeId, weight, consignmentTypeName, CNStatus;
        string pieces, remarks;

        public class BagModel
        {
            public string BagNumber { get; set; }       //
            public string TotalWeight { get; set; }       //
            public string Seal { get; set; }       //
            public string Origin { get; set; }       //
            public string Destination { get; set; }       //
            public string Type { get; set; }       //
            public string bagDate { get; set; }       //
            public string serverResponse { get; set; }
        }

        public class ManifestModel
        {
            public string BagNumber { get; set; }       //
            public string ManifestNumber { get; set; }
            public float weight { get; set; }           //
            public int Pieces { get; set; }           //
            public string serverResponse { get; set; }
        }

        public class CNtModel
        {
            public string ConsignmentNumber { get; set; }       //
            public string BagNumber { get; set; }       //
            public string ServiceType { get; set; }           //
            public string weight { get; set; }           //
            public string Pieces { get; set; }           //
            public string serverResponse { get; set; }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            error_msg.Text = "";
            Get_PrefixCheck();

            dd_start_date.Text = DateTime.Now.ToString("yyyy-MM-dd");
            if (Edit.Checked)
            {
                hd_1.Value = "2";
            }
            if (New.Checked)
            {
                hd_1.Value = "1";
            }

            if (!IsPostBack)
            {
                Get_Orign();
                Get_Destination();
                DataTable dt_head = new DataTable();
                dt_head.Columns.Add("manifestNumber", typeof(string));
                dt_head.Columns.Add("manifestType", typeof(string));
                dt_head.Columns.Add("origin", typeof(string));
                dt_head.Columns.Add("destination", typeof(string));
                dt_head.Columns.Add("OrignName", typeof(string));
                dt_head.Columns.Add("DestinationName", typeof(string));
                dt_head.Columns.Add("isnew");
                dt_head.Columns.Add("pieces");
                dt_head.Columns.Add("remarks");
                ViewState["handleManifest"] = dt_head;

                DataTable dt_head2 = new DataTable();
                dt_head2.Columns.Add("consignmentNumber", typeof(string));
                dt_head2.Columns.Add("serviceTypeName", typeof(string));
                dt_head2.Columns.Add("consignmentTypeId", typeof(string));
                dt_head2.Columns.Add("consignmentTypeName", typeof(string));
                dt_head2.Columns.Add("origin", typeof(string));
                dt_head2.Columns.Add("destination", typeof(string));
                dt_head2.Columns.Add("OrignName", typeof(string));
                dt_head2.Columns.Add("DestinationName", typeof(string));
                dt_head2.Columns.Add("Weight", typeof(string));
                dt_head2.Columns.Add("CNStatus", typeof(string));
                dt_head2.Columns.Add("isnew");
                dt_head2.Columns.Add("pieces");
                dt_head2.Columns.Add("remarks");
                ViewState["handleConsignment"] = dt_head2;
                GetCNLengths();
            }
        }

        protected void AltuFaltu()
        {
            DataTable dt_head = ViewState["handleManifest"] as DataTable;
            DataTable dt_head2 = ViewState["handleConsignment"] as DataTable;
        }
        public void GetCNLengths()
        {

        }

        public DataSet Get_MasterOrign(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sql = "SELECT b.branchCode, \n"
                               + "       b.name   BranchName, b.sname +' - '+ b.name SNAME \n"
                               + "FROM   Branches  b                         \n"
                               + "where b.status ='1' \n"
                               + " and b.branchCode = '" + HttpContext.Current.Session["branchcode"].ToString() + "' \n"
                               + "GROUP BY \n"
                               + "       b.branchCode, \n"
                               + "       b.name, b.sname  \n"
                               + "ORDER BY BranchName ASC";

                SqlConnection orcl = new SqlConnection(clvar.Strcon2());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sql, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                orcl.Close();
            }
            catch (Exception Err)
            {

            }
            finally
            { }
            return ds;
        }

        public void Get_Orign()
        {
            DataSet ds_orign = Get_MasterOrign(clvar);

            if (ds_orign.Tables[0].Rows.Count != 0)
            {
                dd_origin.DataTextField = "SNAME";
                dd_origin.DataValueField = "branchCode";
                dd_origin.DataSource = ds_orign.Tables[0].DefaultView;
                dd_origin.DataBind();
            }
        }

        public void Get_Destination()
        {
            DataTable ds_destination = Cities_();

            if (ds_destination.Rows.Count != 0)
            {
                dd_destination.DataTextField = "SNAME";
                dd_destination.DataValueField = "branchCode";
                dd_destination.DataSource = ds_destination.DefaultView;
                dd_destination.DataBind();
            }
            dd_destination.Items.Insert(0, new ListItem("Select Destination ", ""));
        }

        protected void txt_bagno_TextChanged(object sender, EventArgs e)
        {
            error_msg.Text = "";
            clvar._BagNumber = txt_bagno.Text.Trim();

            DataSet ds = b_fun.Get_SingleBagNumberRecord(clvar);
            if (ds.Tables[0].Rows.Count != 0)
            {
                error_msg.Text = "Bag Number Already in the System...";
                txt_bagno.Text = "";
                if (New.Checked)
                {
                    error_msg.Text = "Bag Number Already in the System...";
                    txt_bagno.Text = "";
                }
                else
                {
                    clvar.BagNumber = clvar._BagNumber;
                    ds = GetBagDetails(clvar);
                    if (ds.Tables["BagManifests"].Rows.Count > 0)
                    {
                        ViewState["handleManifest"] = ds.Tables["BagManifest"];
                    }
                    if (ds.Tables["OutPieces"].Rows.Count > 0)
                    {
                        ViewState["handleConsignment"] = ds.Tables["OutPieces"];
                    }
                }
            }
            else
            {

            }
        }

        protected void txt_seal_TextChanged(object sender, EventArgs e)
        {
            error_msg.Text = "";
            clvar._Seal = txt_seal.Text.Trim();

            DataSet ds = b_fun.Get_SingleSealNumberRecord(clvar);
            if (ds.Tables[0].Rows.Count != 0)
            {
                error_msg.Text = "Seal Number Already in the System...";
                txt_seal.Text = "";
            }
        }

        protected void txt_manifestno_TextChanged(object sender, EventArgs e)
        {

        }

        protected void txt_bagno1_TextChanged(object sender, EventArgs e)
        {
            //clvar._BagNumber = txt_bagno1.Text.Trim();

            //DataSet ds = b_fun.Get_SingleBagNumberRecord(clvar);
            //if (ds.Tables[0].Rows.Count != 0)
            //{
            //    dd_start_date.Text = ds.Tables[0].Rows[0]["date"].ToString();
            //    txt_weight.Text = ds.Tables[0].Rows[0]["totalWeight"].ToString();
            //    txt_seal.Text = ds.Tables[0].Rows[0]["sealNo"].ToString();
            //    dd_destination.SelectedIndex = dd_destination.Items.IndexOf(dd_destination.Items.FindByValue(ds.Tables[0].Rows[0]["destination"].ToString()));
            //    dd_orign.SelectedIndex = dd_orign.Items.IndexOf(dd_orign.Items.FindByValue(ds.Tables[0].Rows[0]["origin"].ToString()));

            //    DataSet ds2 = b_fun.Get_BagManifestRecord(clvar);

            //    if (ds2.Tables[0].Rows.Count != 0)
            //    {
            //        GridView1.DataSource = ds2.Tables[0].DefaultView;
            //        GridView1.DataBind();
            //    }
            //}

            error_msg.Text = "";
            //     clvar._BagNumber = txt_bagno1.Text.Trim();

            DataSet ds = b_fun.Get_SingleBagNumberRecord(clvar);
            if (ds.Tables[0].Rows.Count != 0)
            {

                if (New.Checked)
                {
                    error_msg.Text = "Bag Number Already in the System...";
                    txt_bagno.Text = "";
                }
                else
                {
                    dd_start_date.Text = ds.Tables[0].Rows[0]["date"].ToString();
                    txt_weight.Text = ds.Tables[0].Rows[0]["totalWeight"].ToString();
                    txt_seal.Text = ds.Tables[0].Rows[0]["sealNo"].ToString();
                    dd_destination.SelectedIndex = dd_destination.Items.IndexOf(dd_destination.Items.FindByValue(ds.Tables[0].Rows[0]["destination"].ToString()));
                    dd_origin.SelectedIndex = dd_origin.Items.IndexOf(dd_origin.Items.FindByValue(ds.Tables[0].Rows[0]["origin"].ToString()));
                    clvar.BagNumber = clvar._BagNumber;
                    ds = GetBagDetails(clvar);
                    if (ds.Tables["BagManifests"].Rows.Count > 0)
                    {
                        ViewState["handleManifest"] = ds.Tables["BagManifests"];
                    }
                    if (ds.Tables["OutPieces"].Rows.Count > 0)
                    {
                        ViewState["handleConsignment"] = ds.Tables["OutPieces"];
                    }
                }
            }
            else
            {

            }

        }

        protected void txt_consignment_TextChanged(object sender, EventArgs e)
        {


        }

        protected void new_Consignment()
        {

        }

        protected void Btn_Save_Click(object sender, EventArgs e)
        {

            //   System.Threading.Thread.Sleep(10);

            DataTable dt_head = ViewState["handleManifest"] as DataTable;
            DataTable dt_head2 = ViewState["handleConsignment"] as DataTable;

            if (txt_bagno.Text != "" && dd_start_date.Text != "" && txt_weight.Text != "" && txt_seal.Text != "")
            {
                clvar._BagNumber = txt_bagno.Text.Trim();
                clvar._StartDate = dd_start_date.Text;
                clvar._Weight = txt_weight.Text;
                clvar._Seal = txt_seal.Text;
                clvar._Orign = dd_origin.SelectedValue;
                clvar._Destination = dd_destination.SelectedValue;
                clvar._status = "New";
                clvar._StateId = "3";

                b_fun.Insert_Bag(clvar);
                b_fun.Insert_BagStatus(clvar);

                for (int i = 0; i < dt_head.Rows.Count; i++)
                {
                    clvar._BagNumber = txt_bagno.Text.Trim();
                    clvar._Manifest = dt_head.Rows[i]["manifestNumber"].ToString();
                    string pieces = dt_head.Rows[i]["pieces"].ToString();
                    string remarks = dt_head.Rows[i]["Remarks"].ToString();
                    Insert_BagManifest(clvar, pieces, remarks);

                    clvar._Id = clvar._Orign;
                    DataSet ds_origin = b_fun.Get_BranchDetail(clvar);
                    clvar._CityCode = ds_origin.Tables[0].Rows[0]["name"].ToString();
                    clvar.Seal = txt_seal.Text;
                    Cl_Variables clvar_ = new Cl_Variables();
                    clvar_.manifestNo = clvar._Manifest;
                    //con.InsertTrackingFromBagging(clvar, clvar_);
                    //b_fun.Insert_ConsignmentTrackingHistory(clvar);
                    b_fun.Insert_TrackingFromBagManifest(clvar);
                }

                //b_fun.Insert_RVDBOBag(clvar);

                for (int i = 0; i < dt_head2.Rows.Count; i++)
                {
                    clvar._Manifest = "";
                    clvar._BagNumber = txt_bagno.Text.Trim();
                    clvar._ConsignmentNo = dt_head2.Rows[i]["consignmentNumber"].ToString();
                    clvar._RiderCode = "";
                    clvar._RiderName = "";
                    clvar._Zone = dt_head2.Rows[i]["origin"].ToString();
                    clvar._TownCode = dt_head2.Rows[i]["destination"].ToString();
                    clvar._Weight = dt_head2.Rows[i]["Weight"].ToString();
                    clvar._Services = dt_head2.Rows[i]["serviceTypeName"].ToString();
                    clvar._ConsignmentType = dt_head2.Rows[i]["consignmentTypeId"].ToString();
                    pieces = dt_head2.Rows[i]["pieces"].ToString();
                    remarks = dt_head2.Rows[i]["remarks"].ToString();
                    b_fun.Insert_Consignment(clvar);
                    BagConsignment(clvar, pieces, remarks, clvar._Weight.ToString());

                    clvar._Id = clvar._Orign;
                    DataSet ds_origin = b_fun.Get_BranchDetail(clvar);
                    clvar._CityCode = ds_origin.Tables[0].Rows[0]["name"].ToString();

                    if (dt_head2.Rows[i]["CNSTATUS"].ToString() == "NEW")
                    {
                        clvar._StateId = "1";
                        clvar._BagNumber = "";
                        b_fun.Insert_ConsignmentTrackingHistory(clvar);
                    }

                    clvar._StateId = "3";

                    clvar.Seal = txt_seal.Text;
                    b_fun.Insert_ConsignmentTrackingHistory(clvar);
                }

                //    clvar._CityCode = b_fun.Get_Branches(clvar).Tables[0].Rows[0]["name"].ToString();
                //    b_fun.Insert_ConsignmentTrackingHistory(clvar);

                // string temp_ = clvar.ArrivalID.ToString();
                string temp_ = txt_bagno.Text.Trim();

                string script_ = @"window.open(""{0}"", ""{1}"", ""{2}"");";
                string script = String.Format(script_, "Bag_Print.aspx?Xcode=" + temp_, "_blank", "");
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Redirect", script, true);
                ResetAll();
                //   Response.Redirect("Files/MasterBag.aspx");
                /*
                for (int i = 0; i < dt_head2.Rows.Count; i++)
                {
                    clvar._BagNumber = txt_bagno.Text.Trim();
                    clvar._ConsignmentNo = dt_head.Rows[i]["consignmentNumber"].ToString();

                    b_fun.Insert_BagOutpieceAssociation(clvar);
                }
                */
            }
            else
            {

            }
        }

        protected void Btn_Update_Click(object sender, EventArgs e)
        {
            clvar._StartDate = dd_start_date.Text;
            clvar._Weight = txt_weight.Text;
            clvar._Seal = txt_seal.Text;
            clvar._Orign = dd_origin.SelectedValue;
            clvar._Destination = dd_destination.SelectedValue;

            clvar._status = "Modify";

            DataTable dt_head = ViewState["handleManifest"] as DataTable;
            DataTable dt_head2 = ViewState["handleConsignment"] as DataTable;

            //if (txt_bagno1.Text != "" && txt_weight.Text != "" && txt_seal.Text != "")
            //{
            //    b_fun.Update_Bag(clvar);
            //    for (int i = 0; i < dt_head.Rows.Count; i++)
            //    {
            //        clvar._Manifest = dt_head.Rows[i]["manifestNumber"].ToString();


            //        //b_fun.Insert_BagStatus(clvar);
            //        b_fun.Insert_BagManifest(clvar);
            //    }
            //}



            string error = UpdateBag(clvar, dt_head, dt_head2);
            if (error == "")
            {

                string script_ = @"window.open(""{0}"", ""{1}"", ""{2}"");";
                //   string script = String.Format(script_, "Bag_Print.aspx?Xcode=" + temp_, "_blank", "");
                //   ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Redirect", script, true);
                ResetAll();
            }
            else
            {
                error_msg.Text = error;
                return;
            }
        }

        protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string item = e.Row.Cells[0].Text;
                foreach (Button button in e.Row.Cells[2].Controls.OfType<Button>())
                {
                    if (button.CommandName == "Delete")
                    {
                        button.Attributes["onclick"] = "if(!confirm('Do you want to delete " + item + "?')){ return false; };";
                    }
                }
            }
        }

        protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int index = Convert.ToInt32(e.RowIndex);
            DataTable dt = ViewState["handleManifest"] as DataTable;
            dt.Rows[index].Delete();
            int numberOfRecords = dt.Select().Length;
            lbl_count.Text = "Count: " + numberOfRecords.ToString();
            ViewState["handleManifest"] = dt;
        }

        protected void OnRowDataBound_2(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string item = e.Row.Cells[0].Text;
                foreach (Button button in e.Row.Cells[2].Controls.OfType<Button>())
                {
                    if (button.CommandName == "Delete")
                    {
                        button.Attributes["onclick"] = "if(!confirm('Do you want to delete " + item + "?')){ return false; };";
                    }
                }
            }
        }

        protected void OnRowDeleting_2(object sender, GridViewDeleteEventArgs e)
        {
            int index = Convert.ToInt32(e.RowIndex);
            DataTable dt = ViewState["handleConsignment"] as DataTable;
            dt.Rows[index].Delete();
            int numberOfRecords = dt.Select().Length;
            lbl_count.Text = "Count: " + numberOfRecords.ToString();
            ViewState["handleConsignment"] = dt;
        }

        public DataSet GetBagDetails(Variable clvar)
        {



            string sqlString = "select m.manifestNumber,\n" +
            "       m.manifestType,\n" +
            "       m.origin,\n" +
            "       m.destination,\n" +
            "       b1.name OrignName,\n" +
            "       b2.name DestinationName,\n" +
            "       '' isnew, bm.pieces, bm.remarks\n" +
            "  from MNP_Manifest m\n" +
            " inner join BagManifest bm\n" +
            "    on bm.manifestNumber = m.manifestNumber\n" +
            " inner join Branches b1\n" +
            "    on m.origin = b1.branchCode\n" +
            " inner join Branches b2\n" +
            "    on m.destination = b2.branchCode" +
            "   and bm.bagNumber = '" + clvar.BagNumber + "'";


            string sqlString1 = "select c.consignmentNumber,\n" +
            "       c.serviceTypeName,\n" +
            "       c.consignmentTypeId,\n" +
            "       ct.name ConsignmentTypeName,\n" +
            "       c.orgin origin,\n" +
            "       c.destination,\n" +
            "       b1.name OrignName,\n" +
            "       b2.name DestinationName,\n" +
            "       ba.weight,\n" +
            "       '' CNStatus,\n" +
            "       '' isnew, ba.pieces, ba.remarks\n" +
            "  from Consignment c\n" +
            " inner join BagOutpieceAssociation ba\n" +
            "    on ba.outpieceNumber = c.consignmentNumber\n" +
            " inner join ServiceTypes st\n" +
            "    on st.serviceTypeName = c.serviceTypeName\n" +
            " inner join ConsignmentType ct\n" +
            "    on ct.id = c.consignmentTypeId\n" +
            " inner join branches b1\n" +
            "    on b1.branchCode = c.orgin\n" +
            " inner join branches b2\n" +
            "    on b2.branchCode = c.destination\n" +
            " where ba.bagNumber = '" + clvar.BagNumber + "'";

            DataSet ds = new DataSet();

            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sqlString, con);
                sda.Fill(ds, "BagManifests");
                sda = new SqlDataAdapter(sqlString1, con);
                sda.Fill(ds, "OutPieces");
            }
            catch (Exception ex)
            { }
            finally { con.Close(); }


            return ds;
        }

        public string UpdateBag(Variable clvar, DataTable manifest, DataTable outpiece)
        {
            clvar._StateId = "3";
            List<string> queries = new List<string>();
            string query = "update bag set totalWeight = '" + clvar._Weight + "', SealNo = '" + clvar._Seal + "' where bagNumber = '" + clvar._BagNumber + "'";
            queries.Add(query);

            foreach (DataRow row in manifest.Rows)
            {
                if (row["isnew"].ToString() == "1")
                {
                    query = "insert into BagManifest \n" +
                                "  (b.bagNumber, b.manifestNumber, b.createdBy, b.createdOn, pieces, remarks)\n" +
                                "values\n" +
                                "  ( \n" +
                                "   '" + clvar._BagNumber + "',\n" +
                                "   '" + row["manifestNumber"].ToString() + "',\n" +
                                "   '" + Session["U_ID"].ToString() + "', \n" +
                                "   GETDATE(), '" + row["pieces"].ToString() + "', '" + row["remarks"].ToString() + "'\n" +
                                " ) ";
                    queries.Add(query);

                    query = "insert into ConsignmentsTrackingHistory (consignmentNumber, stateID, currentLocation, manifestNumber, bagNumber, transactionTime, sealNo) \n" +
                                "select consignmentNumber, \n" +
                                " '" + clvar._StateId + "' StateID,\n" +
                                " '" + HttpContext.Current.Session["LocationName"].ToString() + "' CurrentLocation, \n" +
                                " manifestNumber, \n" +
                                " '" + clvar._BagNumber + "' BagNumber, \n" +
                                " GETDATE(), \n" +
                                " '" + clvar.Seal + "' SealNo \n" +
                                " from Mnp_ConsignmentManifest \n" +
                                " where manifestNumber = '" + row["manifestNumber"].ToString() + "'";
                    queries.Add(query);
                }
                else
                {
                    query = "UPDATE BagManifest set pieces = '" + row["pieces"].ToString() + "', remarks = '" + row["remarks"].ToString() + "' where bagNumber = '" + clvar._BagNumber + "' and manifestNumber = '" + row["manifestNumber"].ToString() + "'";
                    queries.Add(query);
                }
            }

            foreach (DataRow row in outpiece.Rows)
            {
                if (row["isnew"].ToString() == "1")
                {
                    if (row["CNSTATUS"].ToString().ToUpper() == "NEW")
                    {
                        query = "insert into Consignment \n" +
                                "  (consignmentNumber, serviceTypeName,  consignmentTypeId, weight, orgin, destination, createdon, createdby, customerType, pieces, \n" +
                                "   creditClientId, weightUnit, discount, cod, totalAmount, zoneCode, branchCode, gst, consignerAccountNo, bookingDate, isApproved, \n" +
                                "   deliveryStatus, dayType, isPriceComputed, isNormalTariffApplied, receivedFromRider, originExpressCenter)\n" +
                                "values\n" +
                                "  ( \n" +
                                "   '" + row["ConsignmentNumber"].ToString() + "',\n" +
                                "   '" + row["serviceTypeName"].ToString() + "',\n" +

                                "   '" + row["ConsignmentTypeID"].ToString() + "',\n" +
                                "   '" + row["weight"].ToString() + "',\n" +
                                "   '" + Session["BranchCode"].ToString() + "',\n" +
                                "   '" + clvar._Destination + "',\n" +
                                "   GETDATE(),\n" +
                                "   '" + Session["U_ID"].ToString() + "',\n" +
                                "   '1',\n" +
                                "   '" + row["pieces"].ToString() + "',\n" +
                                "   '330140',\n" +
                                "   '1',\n" +
                                "   '0',\n" +
                                "   '0',\n" +
                                "   '0',\n" +
                                "   '" + Session["zonecode"].ToString() + "',\n" +
                                "   '" + Session["BranchCode"].ToString() + "',\n" +
                                "   '0',\n" +
                                "   '4D1',\n" +
                                "   GETDATE(),\n" +
                                "   '0',\n" +
                                "   '4',\n" +
                                "   '0',\n" +
                                "   '0',\n" +
                                "   '0',\n" +
                                "   '1', '" + clvar.Expresscentercode + "'\n" +
                                " ) ";
                        queries.Add(query);
                        query = "insert into ConsignmentsTrackingHistory \n" +
                                "  (consignmentNumber, stateID, currentLocation, transactionTime, sealNo)\n" +
                                "values\n" +
                                "  ( \n" +
                                "   '" + row["consignmentNumber"].ToString() + "',\n" +
                                "   '1',\n" +
                                "   '" + HttpContext.Current.Session["LocationName"].ToString() + "',\n" +
                                "   GETDATE(),\n" +
                                "   '" + clvar.Seal + "'\n" +
                                " ) ";
                        queries.Add(query);
                    }

                    query = "insert into BagOutpieceAssociation \n" +
                                    " (bagNumber, outpieceNumber, pieces, remarks, weight)\n" +
                                    "values\n" +
                                    "  ( \n" +
                                    "   '" + clvar._BagNumber + "',\n" +
                                    "   '" + row["consignmentNumber"].ToString() + "' , '" + row["pieces"].ToString() + "', '" + row["remarks"].ToString() + "', '" + row["Weight"].ToString() + "'\n" +
                                    " ) ";
                    queries.Add(query);
                    query = "insert into ConsignmentsTrackingHistory \n" +
                                "  (consignmentNumber, stateID, currentLocation,  bagNumber,   transactionTime, sealNo)\n" +
                                "values\n" +
                                "  ( \n" +
                                "   '" + row["consignmentNumber"].ToString() + "',\n" +
                                "   '3',\n" +
                                "   '" + HttpContext.Current.Session["LocationName"].ToString() + "',\n" +
                                "   '" + clvar._BagNumber + "',\n" +
                                "   GETDATE(),\n" +
                                "   '" + clvar.Seal + "'\n" +
                                " ) ";
                    queries.Add(query);
                }
                else
                {
                    query = "UPDATE bagOutpieceAssociation set pieces = '" + row["pieces"].ToString() + "', remarks = '" + row["remarks"].ToString() + "', weight='" + row["weight"].ToString() + "' where bagnumber = '" + clvar._BagNumber + "' and outpieceNumber = '" + row["consignmentNumber"].ToString() + "'";
                    queries.Add(query);
                }
            }




            string Error = "";
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                foreach (string str in queries)
                {
                    cmd.CommandText = str;
                    cmd.ExecuteNonQuery();
                }

            }
            catch (Exception ex)
            { Error = ex.Message; }
            finally { con.Close(); }
            return Error;
        }

        protected void ResetAll()
        {
            txt_bagno.Text = "";
            //   txt_consignmentno.Text = "";
            txt_seal.Text = "";
            txt_weight.Text = "";
            dd_origin.ClearSelection();
            dd_destination.ClearSelection();
            dd_start_date.Text = DateTime.Now.ToString("yyyy-MM-dd");


            DataTable dt_head = new DataTable();
            dt_head.Columns.Add("manifestNumber", typeof(string));
            dt_head.Columns.Add("manifestType", typeof(string));
            dt_head.Columns.Add("origin", typeof(string));
            dt_head.Columns.Add("destination", typeof(string));
            dt_head.Columns.Add("OrignName", typeof(string));
            dt_head.Columns.Add("DestinationName", typeof(string));
            dt_head.Columns.Add("isnew");
            ViewState["handleManifest"] = dt_head;

            DataTable dt_head2 = new DataTable();
            dt_head2.Columns.Add("consignmentNumber", typeof(string));
            dt_head2.Columns.Add("serviceTypeName", typeof(string));
            dt_head2.Columns.Add("consignmentTypeId", typeof(string));
            dt_head2.Columns.Add("consignmentTypeName", typeof(string));
            dt_head2.Columns.Add("origin", typeof(string));
            dt_head2.Columns.Add("destination", typeof(string));
            dt_head2.Columns.Add("OrignName", typeof(string));
            dt_head2.Columns.Add("DestinationName", typeof(string));
            dt_head2.Columns.Add("Weight", typeof(string));
            dt_head2.Columns.Add("CNStatus", typeof(string));
            dt_head2.Columns.Add("isnew");
            ViewState["handleConsignment"] = dt_head2;

        }

        public DataSet Get_SingleManifestRecord(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                // string sql = "select * from Manifest m where m.manifestNumber = '" + clvar._Manifest + "' ";

                //string sql = "select m.manifestNumber, m.manifestType, b1.name OrignName, b2.name DestinationName, m.origin, m.destination \n" +
                //             "from Manifest m, Branches b1, Branches b2 \n" +
                //             "where m.origin = b1.branchCode \n" +
                //             "and m.destination = b2.branchCode \n" +
                //             "and m.manifestNumber = '" + clvar._Manifest + "' ";

                string sql = "select m.manifestNumber, m.manifestType, 0 IsDeManifested, b1.name OrignName, b2.name DestinationName, m.origin, m.destination, '1' pieces, '' remarks \n" +
                           "from MNP_Manifest m, Branches b1, Branches b2  \n" +
                           "where m.origin = b1.branchCode \n" +
                           "and m.destination = b2.branchCode \n" +
                           "and m.manifestNumber = '" + clvar._Manifest + "' \n";
                // "and m.destination = '" + clvar._Designation + "' ";

                SqlConnection orcl = new SqlConnection(clvar.Strcon2());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sql, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                orcl.Close();
            }
            catch (Exception Err)
            {

            }
            finally
            { }
            return ds;
        }

        public DataSet Get_BagConsignment(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                //  string query = "select * from Consignment c where c.consignmentNumber = '" + clvar._ConsignmentNo + "' AND c.destination = '" + clvar._Designation + "' ";
                string query = "select * \n" +
                               "  from Consignment c\n" +
                               " where c.consignmentNumber = '" + clvar._ConsignmentNo + "' ";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                orcl.Close();
            }
            catch (Exception Err)
            {

            }
            finally
            { }
            return ds;
        }

        public void Insert_BagManifest(Variable clvar, string pieces, string remarks)
        {
            try
            {
                string query = "insert into BagManifest \n" +
                                "  (b.bagNumber, b.manifestNumber, b.createdBy, b.createdOn, pieces, remarks)\n" +
                                "values\n" +
                                "  ( \n" +
                                "   '" + clvar.BagNumber + "',\n" +
                                "   '" + clvar._Manifest + "',\n" +
                                "   '" + Session["U_ID"].ToString() + "', \n" +
                                "   GETDATE(), '" + pieces + "', '" + remarks + "'\n" +
                                " ) ";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                orcd.ExecuteNonQuery();
                orcl.Close();
            }
            catch (Exception Err)
            {

            }
            finally
            { }
        }

        public void BagConsignment(Variable clvar, string pieces, string remarks, string weight)
        {
            try
            {
                string query = "insert into BagOutpieceAssociation \n" +
                                " (bagNumber, outpieceNumber, pieces, remarks, weight)\n" +
                                "values\n" +
                                "  ( \n" +
                                "   '" + clvar._BagNumber + "',\n" +
                                "   '" + clvar._ConsignmentNo + "', '" + pieces + "', '" + remarks + "', '" + weight.ToString() + "' \n" +
                                " ) ";

                SqlConnection orcl = new SqlConnection(clvar.Strcon2());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                orcd.ExecuteNonQuery();
                orcl.Close();
            }
            catch (Exception Err)
            {

            }
            finally
            { }
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

            sqlString = "select b.branchCode ,B.sname +' - '+ b.name SNAME, b.branchCode\n" +
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

        public bool IsNumeric(string text)
        {
            char[] arr = text.ToCharArray();
            foreach (char ch in arr)
            {
                if (!char.IsDigit(ch))
                {
                    return false;
                }
            }

            return true;
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
        public static string[][] Get_ManifefstInformation(string ManifestNo)
        {
            List<string[]> resp = new List<string[]>();


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
                            string[] Manifest = { "", "", "", "", "", "", "" };
                            Manifest[0] = ManifestNo;
                            Manifest[1] = ds.Tables[0].Rows[0][1].ToString();
                            Manifest[2] = ds.Tables[0].Rows[0][4].ToString();
                            Manifest[3] = ds.Tables[0].Rows[0][5].ToString();
                            Manifest[4] = DateTime.Parse(ds.Tables[0].Rows[0][2].ToString()).ToString("yyyy-MM-dd");
                            Manifest[5] = ds.Tables[0].Rows[0][23].ToString();
                            Manifest[6] = ds.Tables[0].Rows[0][24].ToString();
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

            return resp.ToArray();
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
        public static string[][] Get_BagInformation(string BagNo, string Selection)
        {
            List<string[]> resp = new List<string[]>();

            if (Selection == "1")
            {
                string ManifestNo_ = BagNo;
                DataSet ds = GetBagCheck(ManifestNo_);

                if (ds != null)
                {
                    if (ds.Tables.Count != 0)
                    {
                        if (ds.Tables[0].Rows.Count != 0)
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                string[] Bag = { "", "", "", "", "" };
                                Bag[0] = dr[0].ToString();
                                Bag[1] = dr[19].ToString();
                                Bag[2] = dr[9].ToString();
                                Bag[3] = dr[10].ToString();
                                Bag[4] = dr[5].ToString();


                                resp.Add(Bag);
                            }
                        }
                        else
                        {
                            string[] Bag = { "" };
                            Bag[0] = "N/A";
                            resp.Add(Bag);
                        }
                    }
                    else
                    {
                        string[] Bag = { "" };
                        Bag[0] = "N/A";
                        resp.Add(Bag);
                    }
                }
            }
            if (Selection == "2")
            {
                Cl_Variables clvar = new Cl_Variables();
                clvar.BagNumber = BagNo;
                String origin = HttpContext.Current.Session["BRANCHCODE"].ToString();
                String CheckFinalStatus = GetFinalStatus(clvar);
                String checkOrigin = GetOriginBag(clvar, origin);
                DataTable dt = GetConsignmentDetailByBagNumber(clvar);
                DataTable dt_ = GetManifestDetailByBagNumber(clvar);

                if (CheckFinalStatus.Length > 0)
                {
                    string[] status = { "DEBAGGED" };
                    resp.Add(status);
                    return resp.ToArray();
                }
                if (checkOrigin == "")
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
                        foreach (DataRow dr in dt.Rows)
                        {
                            string[] Consignment = { "", "", "", "", "", "", "", "", "", "", "", "", "" };
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
                            resp.Add(Consignment);
                        }
                    }

                    DataSet ds = GetBagCheck(BagNo);

                    if (ds != null)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            string[] Bag = { "", "", "", "", "" };
                            Bag[0] = dr[0].ToString();
                            Bag[1] = dr[19].ToString();
                            Bag[2] = dr[9].ToString();
                            Bag[3] = dr[10].ToString();
                            Bag[4] = dr[5].ToString();

                            resp.Add(Bag);
                        }
                    }
                    foreach (DataRow dr in dt_.Rows)
                    {
                        string[] Manifest = { "", "", "", "", "", "", "", "", "", "" };
                        Manifest[0] = dr[0].ToString();
                        Manifest[1] = dr[1].ToString();
                        Manifest[2] = dr[2].ToString();
                        Manifest[3] = dr[3].ToString();
                        Manifest[4] = dr[4].ToString();
                        Manifest[5] = dr[5].ToString();
                        Manifest[6] = dr[6].ToString();
                        Manifest[7] = dr[7].ToString();
                        Manifest[8] = dr[8].ToString();
                        Manifest[9] = dr[9].ToString();
                        resp.Add(Manifest);
                    }
                    //Consignment


                }


            }
            if (Selection == "3")
            {
                Cl_Variables clvar = new Cl_Variables();
                clvar.manifestNo = BagNo;
                DataTable dt = GetConsignmentDetailByBagNumber(clvar);
                DataTable dt_ = GetManifestDetailByBagNumber(clvar);

                if (dt != null)
                {
                    if (dt.Rows.Count != 0)
                    {
                        DataSet ds = GetBagCheck(BagNo);

                        if (ds != null)
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                string[] Bag = { "", "", "", "", "" };
                                Bag[0] = dr[0].ToString();
                                Bag[1] = dr[19].ToString();
                                Bag[2] = dr[9].ToString();
                                Bag[3] = dr[10].ToString();
                                Bag[4] = dr[5].ToString();

                                resp.Add(Bag);
                            }
                        }
                    }
                    //Manifest Number
                    foreach (DataRow dr in dt_.Rows)
                    {
                        string[] Manifest = { "", "", "", "", "", "", "", "", "" };
                        Manifest[0] = dr[0].ToString();
                        Manifest[1] = dr[1].ToString();
                        Manifest[2] = dr[2].ToString();
                        Manifest[3] = dr[3].ToString();
                        Manifest[4] = dr[4].ToString();
                        Manifest[5] = dr[5].ToString();
                        Manifest[6] = dr[6].ToString();
                        Manifest[7] = dr[7].ToString();
                        Manifest[8] = dr[8].ToString();
                        resp.Add(Manifest);

                    }


                    //Consignment

                    foreach (DataRow dr in dt.Rows)
                    {
                        string[] Consignment = { "", "", "", "", "", "", "", "", "", "", "", "", "" };
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
                        resp.Add(Consignment);
                    }

                }
            }
            return resp.ToArray();
        }

        private static string GetOriginBag(Cl_Variables clvar, String origin)
        {
            String status = "";
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT origin FROM bag WHERE bagNumber = '" + clvar.BagNumber + "' AND origin = '" + origin + "'";

                SqlDataReader sda = cmd.ExecuteReader();
                if (sda.HasRows)
                {
                    while (sda.Read())
                    {
                        status = sda.GetString(0);
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

        public static String GetFinalStatus(Cl_Variables clvar)
        {
            String status = "";
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "Select isFinal from MnP_Debag p where p.BagNumber ='" + clvar.BagNumber + "'";

                SqlDataReader sda = cmd.ExecuteReader();
                while (sda.Read())
                {
                    status = sda.GetValue(0).ToString();
                }
            }
            catch (Exception ex)
            {
            }
            finally { con.Close(); }

            return status;
        }
        public static DataSet GetBagCheck(string ManifestNo)
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
                cmd.CommandText = "Select  * from bag p where p.bagnumber ='" + ManifestNo + "'";

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
        public static DataTable GetConsignmentDetailByBagNumber(Cl_Variables clvar)
        {
            string sqlString1 = "select ba.outpiecenumber consignmentNumber,\n" +
                    "       'overnight' serviceTypeName,\n" +
                    "       '12' consignmentTypeId,\n" +
                    "       '' ConsignmentTypeName,\n" +
                    "       b.origin origin,\n" +
                    "       b.destination,\n" +
                    "       b1.sname +'-'+ b1.name OrignName,\n" +
                    "       b1.sname +'-'+ b2.name DestinationName,\n" +
                    "       ba.weight,\n" +
                    "       '' CNStatus,\n" +
                    "       '' isnew, ba.pieces, ba.remarks\n" +
                    " from bag b \n" +
                    " inner join BagOutpieceAssociation ba\n" +
                    " on b.bagnumber = ba.bagnumber\n" +
                    " inner join branches b1\n" +
                    "    on b1.branchCode = b.origin\n" +
                    " inner join branches b2\n" +
                    "    on b2.branchCode = b.destination\n" +
                    " where ba.bagNumber = '" + clvar.BagNumber + "'";

            DataTable dt = new DataTable();

            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString1, orcl);
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
        public static DataTable GetManifestDetailByBagNumber(Cl_Variables clvar)
        {
            string sqlString = "select m.manifestNumber,\n" +
            "       m.manifestType,\n" +
            "       m.origin,\n" +
            "       m.destination,\n" +
            "       b1.sname +'-'+ b1.name OrignName,\n" +
            "       b2.sname +'-'+ b2.name DestinationName,\n" +
            "       '' isnew, bm.pieces, bm.remarks,bm.weight\n" +
            "  from MNP_Manifest m\n" +
            " inner join BagManifest bm\n" +
            "    on bm.manifestNumber = m.manifestNumber\n" +
            " inner join Branches b1\n" +
            "    on m.origin = b1.branchCode\n" +
            " inner join Branches b2\n" +
            "    on m.destination = b2.branchCode" +
            "   and bm.bagNumber = '" + clvar.BagNumber + "'";

            DataTable dt = new DataTable();

            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
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
        public static string InsertBag(ManifestModel[] manifests, CNtModel[] consignments, BagModel bag)
        {
            string status = "";
            if (bag.Type == "1")
            {
                DataTable dt_consignment = new DataTable();
                dt_consignment.Columns.AddRange(new DataColumn[] {
            new DataColumn("ConsignmentNumber", typeof(string)),
            new DataColumn("BagNo", typeof(string)),
            new DataColumn("ServiceType", typeof(string)),
            new DataColumn("Weight", typeof(string)),
            new DataColumn("Pieces", typeof(string))
        });

                DataTable dt_manifest = new DataTable();
                dt_manifest.Columns.AddRange(new DataColumn[] {
            new DataColumn("ManifestNumber", typeof(string)),
            new DataColumn("BagNo", typeof(string)),
            new DataColumn("Weight", typeof(float)),
            new DataColumn("Pieces", typeof(int))
        });

                Cl_Variables clvar = new Cl_Variables();
                clvar.BagNumber = bag.BagNumber;
                clvar.ToWeight = bag.TotalWeight;
                clvar.SealNumber = bag.Seal;
                clvar.origin = bag.Origin;
                clvar.destination = bag.Destination;
                clvar.BookingDate = bag.bagDate;

                // Consignment Logic;
                if (consignments.Length > 0)
                {
                    foreach (CNtModel cn in consignments)
                    {
                        DataRow dr = dt_consignment.NewRow();
                        dr["ConsignmentNumber"] = cn.ConsignmentNumber;
                        dr["BagNo"] = cn.BagNumber;
                        dr["ServiceType"] = cn.ServiceType;
                        dr["Weight"] = cn.weight;
                        dr["Pieces"] = cn.Pieces;
                        dt_consignment.Rows.Add(dr);
                    }
                }
                // Manifestlogic
                if (manifests.Length > 0)
                {
                    foreach (ManifestModel cn in manifests)
                    {

                        DataRow dr = dt_manifest.NewRow();
                        dr["ManifestNumber"] = cn.ManifestNumber;
                        dr["BagNo"] = cn.BagNumber;
                        dr["Weight"] = cn.weight;
                        dr["Pieces"] = cn.Pieces;
                        dt_manifest.Rows.Add(dr);

                    }
                }

                //Insert Bag
                status = GenerateBag(clvar, dt_consignment, dt_manifest);

            }
            if (bag.Type == "2")
            {
                DataTable dt_consignment = new DataTable();
                dt_consignment.Columns.AddRange(new DataColumn[] {
            new DataColumn("ConsignmentNumber", typeof(string)),
            new DataColumn("BagNo", typeof(string)),
            new DataColumn("ServiceType", typeof(string)),
            new DataColumn("Weight", typeof(string)),
            new DataColumn("Pieces", typeof(string))
        });

                DataTable dt_manifest = new DataTable();
                dt_manifest.Columns.AddRange(new DataColumn[] {
            new DataColumn("ManifestNumber", typeof(string)),
            new DataColumn("BagNo", typeof(string)),
            new DataColumn("Weight", typeof(float)),
            new DataColumn("Pieces", typeof(int))
            });

                Cl_Variables clvar = new Cl_Variables();
                clvar.BagNumber = bag.BagNumber;
                string roundWeight = "";
                clvar.ToWeight = bag.TotalWeight;
                if (clvar.ToWeight == "0.5")
                {
                    roundWeight = "1";
                }
                else
                {
                    roundWeight = Math.Round(double.Parse(clvar.ToWeight)).ToString();
                }

                clvar.ToWeight = roundWeight;
                clvar.SealNumber = bag.Seal;
                clvar.origin = bag.Origin;
                clvar.destination = bag.Destination;
                clvar.BookingDate = bag.bagDate;

                // Consignment Logic;
                if (consignments.Length > 0)
                {
                    foreach (CNtModel cn in consignments)
                    {
                        DataRow dr = dt_consignment.NewRow();
                        dr["ConsignmentNumber"] = cn.ConsignmentNumber;
                        dr["BagNo"] = cn.BagNumber;
                        dr["ServiceType"] = cn.ServiceType;
                        dr["Weight"] = cn.weight;
                        dr["Pieces"] = cn.Pieces;
                        dt_consignment.Rows.Add(dr);
                    }
                }
                // Manifestlogic
                if (manifests.Length > 0)
                {
                    foreach (ManifestModel cn in manifests)
                    {

                        DataRow dr = dt_manifest.NewRow();
                        dr["ManifestNumber"] = cn.ManifestNumber;
                        dr["BagNo"] = cn.BagNumber;
                        dr["Weight"] = cn.weight;
                        dr["Pieces"] = cn.Pieces;
                        dt_manifest.Rows.Add(dr);

                    }
                }
                GenerateBag_Archeive(clvar);
                //Delete_Bag(clvar);
                //Insert Manifest
                status = EditBag(clvar, dt_consignment, dt_manifest);


            }
            return status;
        }

        public static void Delete_Bag(Cl_Variables clvar)
        {
            SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            sqlcon.Open();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcon;
            sqlcmd.CommandType = CommandType.Text;
            int count = 0;
            try
            {
                //Manifest
                string query = " Delete p from bagmanifest p where p.bagnumber = '" + clvar.BagNumber + "'";
                sqlcmd.CommandText = query;
                count = sqlcmd.ExecuteNonQuery();

                //Manifest
                query = " Delete p from BagOutpieceAssociation p where p.bagnumber = '" + clvar.BagNumber + "'";
                sqlcmd.CommandText = query;
                count = sqlcmd.ExecuteNonQuery();

                //Manifest
                query = " Delete p from bag p where p.bagnumber = '" + clvar.BagNumber + "'";
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

        public static string GenerateBag(Cl_Variables clvar, DataTable dt, DataTable dt1)
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


                string roundWeight = "";
                if (clvar.ToWeight == "0.5")
                {
                    roundWeight = "1";
                }
                else
                {
                    roundWeight = Math.Round(double.Parse(clvar.ToWeight)).ToString();
                }

                string query = "insert into Bag ([bagNumber],[createdBy],[createdOn],[totalWeight],[origin],[destination],[date],[branchCode],[zoneCode],[sealNo],LocationID)\n" +
                               " VALUES ( \n" +
                               "'" + clvar.BagNumber + "',\n" +
                               "'" + HttpContext.Current.Session["U_ID"].ToString() + "',\n" +
                               "'" + DateTime.Now.ToString("yyyy-MM-dd") + "',\n" +
                               "'" + roundWeight + "',\n" +
                               "'" + clvar.origin + "',\n" +
                               "'" + clvar.destination + "',\n" +
                               "'" + clvar.BookingDate + "',\n" +
                               "'" + HttpContext.Current.Session["BranchCode"].ToString() + "',\n" +
                               "'" + HttpContext.Current.Session["ZONECODE"].ToString() + "',\n" +
                               "'" + clvar.SealNumber + "',\n" +
                                "'" + HttpContext.Current.Session["LocationID"].ToString() + "'\n" +
                               ")";
                // Bag Out Pieces
                string query1_ = "";
                if (dt.Rows.Count != 0)
                {
                    query1_ = "INSERT INTO BagOutpieceAssociation (outpieceNumber,bagNumber,weight,pieces,createdon,ismerged ) select A.* FROM ( \n";
                    for (int i = 0; i < dt.Rows.Count - 1; i++)
                    {
                        query1_ += "SELECT '" + dt.Rows[i][0].ToString() + "' outpieceNumber,'" + dt.Rows[i][1].ToString() + "' bagNumber, '" + dt.Rows[i]["weight"].ToString() + "' weight, '" + dt.Rows[i]["pieces"].ToString() + "' pieces, getdate() createdon, '0' ismerged \n UNION ALL \n";
                    }
                    int j = dt.Rows.Count - 1;
                    query1_ += "SELECT '" + dt.Rows[j][0].ToString() + "' outpieceNumber, '" + dt.Rows[j][1].ToString() + "' bagNumber, '" + dt.Rows[j]["weight"].ToString() + "' weight, '" + dt.Rows[j]["pieces"].ToString() + "' pieces, getdate() createdon, '0' ismerged ) A where A.outpieceNumber not in (select outpieceNumber from BagOutpieceAssociation where bagNumber = '" + clvar.BagNumber + "')";
                }
                // Bag Manifest

                string query2_ = "";
                if (dt1.Rows.Count != 0)
                {
                    query2_ = "INSERT INTO BagManifest (bagNumber,ManifestNumber,pieces,createdon,createdby,weight,ismerged ) SELECT A.* FROM (\n";
                    for (int i = 0; i < dt1.Rows.Count - 1; i++)
                    {
                        query2_ += "SELECT '" + dt1.Rows[i][1].ToString() + "' bagNumber,'" + dt1.Rows[i][0].ToString() + "' ManifestNumber, '" + dt1.Rows[i]["Pieces"].ToString() + "' pieces, getdate() createdon,'" + HttpContext.Current.Session["U_ID"].ToString() + "' createdby,'" + dt1.Rows[i]["weight"].ToString() + "' weight, '0' ismerged \n UNION ALL \n";
                    }
                    int j = dt1.Rows.Count - 1;
                    query2_ += "SELECT '" + dt1.Rows[j][1].ToString() + "' bagNumber,'" + dt1.Rows[j][0].ToString() + "' ManifestNumber, '" + dt1.Rows[j]["Pieces"].ToString() + "' pieces, getdate() createdon,'" + HttpContext.Current.Session["U_ID"].ToString() + "' createdby, '" + dt1.Rows[j]["weight"].ToString() + "' weight, '0' ismerged) A where A.ManifestNumber not in (select ManifestNumber from BagManifest where bagnumber = '" + clvar.BagNumber + "')";
                }



                string trackingCmd = "" +
                "INSERT INTO CONSIGNMENTSTRACKINGHISTORY\n" +
                "  (\n" +
                "   -- ID -- THIS COLUMN VALUE IS AUTO-GENERATED\n" +
                "   CONSIGNMENTNUMBER,\n" +
                "   STATEID,\n" +
                "   CURRENTLOCATION,\n" +
                "   BAGNUMBER,\n" +
                "   MANIFESTNUMBER,\n" +
                "   TRANSACTIONTIME,\n" +
                "   STATUSTIME)\n" +
                "  SELECT MCM.CONSIGNMENTNUMBER CNNUMBER,\n" +
                "         '3' STATEID,\n" +
                "         '" + HttpContext.Current.Session["LocationName"].ToString() + "' CURRENTLOCATION,\n" +
                "         BM.BAGNUMBER,\n" +
                "         BM.MANIFESTNUMBER,\n" +
                "         GETDATE(),\n" +
                "         GETDATE()\n" +
                "    FROM BAGMANIFEST BM\n" +
                "   INNER JOIN MNP_CONSIGNMENTMANIFEST MCM\n" +
                "      ON MCM.MANIFESTNUMBER = BM.MANIFESTNUMBER\n" +
                "   INNER JOIN BRANCHES B\n" +
                "      ON B.BRANCHCODE = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'\n" +
                "   WHERE BM.BAGNUMBER = '" + clvar.BagNumber + "'\n" +
                "  UNION ALL\n" +
                "  SELECT BA.OUTPIECENUMBER CNNUMBER,\n" +
                "         '3' STATEID,\n" +
                "         '" + HttpContext.Current.Session["LocationName"].ToString() + "' CURRENTLOCATION,\n" +
                "         BA.BAGNUMBER,\n" +
                "         NULL MANIFESTNUMBER,\n" +
                "         GETDATE(),\n" +
                "         GETDATE()\n" +
                "    FROM BAGOUTPIECEASSOCIATION BA\n" +
                "   INNER JOIN BRANCHES B\n" +
                "      ON B.BRANCHCODE = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'\n" +
                "   WHERE BA.BAGNUMBER = '" + clvar.BagNumber + "'";




                sqlcmd.CommandText = query;
                count = sqlcmd.ExecuteNonQuery();
                if (count == 0)
                {
                    trans.Rollback();
                    count = 0;
                    return "NOT OK";
                }

                if (query2_.Length != 0)
                {
                    sqlcmd.CommandText = query2_;
                    count = sqlcmd.ExecuteNonQuery();
                    if (count == 0)
                    {
                        trans.Rollback();
                        count = 0;
                        return "NOT OK";
                    }
                }

                if (query1_.Length != 0)
                {
                    sqlcmd.CommandText = query1_;
                    count = sqlcmd.ExecuteNonQuery();
                    if (count == 0)
                    {
                        trans.Rollback();
                        count = 0;
                        return "NOT OK";
                    }
                }

                sqlcmd.CommandText = trackingCmd;
                count = sqlcmd.ExecuteNonQuery();
                if (count <= 0)
                {
                    trans.Rollback();
                    count = 0;
                    return "Could Not Insert Tracking";
                }

                trans.Commit();
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
        public static string EditBag(Cl_Variables clvar, DataTable dt, DataTable dt1)
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


                string query = "UPDATE Bag\n" +
                "   SET modifiedBy = '" + HttpContext.Current.Session["U_ID"].ToString() + "', modifiedOn = GETDATE(), totalWeight = '" + clvar.ToWeight + "', destination = '" + clvar.destination + "'\n" +
                " WHERE bagNumber = '" + clvar.BagNumber + "'";

                // Bag Out Pieces
                string query1_ = "";
                if (dt.Rows.Count != 0)
                {
                    query1_ = "INSERT INTO BagOutpieceAssociation (outpieceNumber,bagNumber,weight,pieces,createdon,ismerged ) select A.* FROM ( \n";
                    for (int i = 0; i < dt.Rows.Count - 1; i++)
                    {
                        query1_ += "SELECT '" + dt.Rows[i][0].ToString() + "' outpieceNumber,'" + dt.Rows[i][1].ToString() + "' bagNumber, '" + dt.Rows[i]["weight"].ToString() + "' weight, '" + dt.Rows[i]["pieces"].ToString() + "' pieces, getdate() createdon, '0' ismerged \n UNION ALL \n";
                    }
                    int j = dt.Rows.Count - 1;
                    query1_ += "SELECT '" + dt.Rows[j][0].ToString() + "' outpieceNumber, '" + dt.Rows[j][1].ToString() + "' bagNumber, '" + dt.Rows[j]["weight"].ToString() + "' weight, '" + dt.Rows[j]["pieces"].ToString() + "' pieces, getdate() createdon, '0' ismerged ) A where A.outpieceNumber not in (select outpieceNumber from BagOutpieceAssociation where bagNumber = '" + clvar.BagNumber + "')";
                }
                // Bag Manifest

                string query2_ = "";
                if (dt1.Rows.Count != 0)
                {
                    query2_ = "INSERT INTO BagManifest (bagNumber,ManifestNumber,pieces,createdon,createdby,weight,ismerged ) SELECT A.* FROM (\n";
                    for (int i = 0; i < dt1.Rows.Count - 1; i++)
                    {
                        query2_ += "SELECT '" + dt1.Rows[i][1].ToString() + "' bagNumber,'" + dt1.Rows[i][0].ToString() + "' ManifestNumber, '1' pieces, getdate() createdon,'" + HttpContext.Current.Session["U_ID"].ToString() + "' createdby,'" + dt1.Rows[i]["weight"].ToString() + "' weight, '0' ismerged \n UNION ALL \n";
                    }
                    int j = dt1.Rows.Count - 1;
                    query2_ += "SELECT '" + dt1.Rows[j][1].ToString() + "' bagNumber,'" + dt1.Rows[j][0].ToString() + "' ManifestNumber, '1' pieces, getdate() createdon,'" + HttpContext.Current.Session["U_ID"].ToString() + "' createdby,'" + dt1.Rows[j]["weight"].ToString() + "' weight, '0' ismerged) A where A.ManifestNumber not in (select ManifestNumber from BagManifest where bagnumber = '" + clvar.BagNumber + "')";
                }


                string trackingCmd = "INSERT INTO CONSIGNMENTSTRACKINGHISTORY\n" +
                "  (\n" +
                "   -- ID -- THIS COLUMN VALUE IS AUTO-GENERATED\n" +
                "   CONSIGNMENTNUMBER,\n" +
                "   STATEID,\n" +
                "   CURRENTLOCATION,\n" +
                "   BAGNUMBER,\n" +
                "   MANIFESTNUMBER,\n" +
                "   TRANSACTIONTIME,\n" +
                "   STATUSTIME)\n" +
                "  SELECT *\n" +
                "    FROM (SELECT MCM.CONSIGNMENTNUMBER CNNUMBER,\n" +
                "                 '3' STATEID,\n" +
                "                 B.NAME CURRENTLOCATION,\n" +
                "                 BM.BAGNUMBER,\n" +
                "                 BM.MANIFESTNUMBER,\n" +
                "                 GETDATE() TRANSACTIONTIME,\n" +
                "                 GETDATE() STATUSTIME\n" +
                "            FROM BAGMANIFEST BM\n" +
                "           INNER JOIN MNP_CONSIGNMENTMANIFEST MCM\n" +
                "              ON MCM.MANIFESTNUMBER = BM.MANIFESTNUMBER\n" +
                "           INNER JOIN BRANCHES B\n" +
                "              ON B.BRANCHCODE = '" + clvar.Branch + "'\n" +
                "           WHERE BM.BAGNUMBER = '" + clvar.BagNumber + "'\n" +
                "\n" +
                "          UNION ALL\n" +
                "          SELECT BA.OUTPIECENUMBER CNNUMBER,\n" +
                "                 '3' STATEID,\n" +
                "                 B.NAME CURRENTLOCATION,\n" +
                "                 BA.BAGNUMBER,\n" +
                "                 NULL MANIFESTNUMBER,\n" +
                "                 GETDATE() TRANSACTIONTIME,\n" +
                "                 GETDATE() STATUSTIME\n" +
                "            FROM BAGOUTPIECEASSOCIATION BA\n" +
                "           INNER JOIN BRANCHES B\n" +
                "              ON B.BRANCHCODE = '" + clvar.Branch + "'\n" +
                "           WHERE BA.BAGNUMBER = '" + clvar.BagNumber + "') A\n" +
                "   WHERE A.CNNUMBER NOT IN\n" +
                "         (SELECT CTH.CONSIGNMENTNUMBER\n" +
                "            FROM CONSIGNMENTSTRACKINGHISTORY CTH\n" +
                "           WHERE CTH.CONSIGNMENTNUMBER IN\n" +
                "                 (SELECT BOA.OUTPIECENUMBER\n" +
                "                    FROM BAGOUTPIECEASSOCIATION BOA\n" +
                "                   WHERE BOA.BAGNUMBER = '" + clvar.BagNumber + "'\n" +
                "                  UNION ALL\n" +
                "                  SELECT MCM.CONSIGNMENTNUMBER\n" +
                "                    FROM BAGMANIFEST BM\n" +
                "                   INNER JOIN MNP_CONSIGNMENTMANIFEST MCM\n" +
                "                      ON MCM.MANIFESTNUMBER = BM.MANIFESTNUMBER\n" +
                "                   WHERE BM.BAGNUMBER = '" + clvar.BagNumber + "')\n" +
                "             AND CTH.BAGNUMBER = '" + clvar.BagNumber + "')";



                sqlcmd.CommandText = query;
                count = sqlcmd.ExecuteNonQuery();
                if (count == 0)
                {
                    trans.Rollback();
                    count = 0;
                    return "NOT OK";
                }

                if (query2_.Length != 0)
                {
                    sqlcmd.CommandText = query2_;
                    count = sqlcmd.ExecuteNonQuery();
                    //if (count == 0)
                    //{
                    //    trans.Rollback();
                    //    count = 0;
                    //    return "NOT OK";
                    //}
                }

                if (query1_.Length != 0)
                {
                    sqlcmd.CommandText = query1_;
                    count = sqlcmd.ExecuteNonQuery();
                    //if (count == 0)
                    //{
                    //    trans.Rollback();
                    //    count = 0;
                    //    return "NOT OK";
                    //}
                }

                sqlcmd.CommandText = trackingCmd;
                sqlcmd.ExecuteNonQuery();
                trans.Commit();
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
        public static string GenerateBag_Archeive(Cl_Variables clvar)
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
                DataTable dt_cn = getOutpiece(clvar);
                DataTable dt_Manifest = getManifest(clvar);

                CommonFunction func = new CommonFunction();
                clvar.Branch = HttpContext.Current.Session["BranchCode"].ToString();
                string branchName = func.Branch(clvar).Tables[0].Rows[0]["BranchName"].ToString();

                string query = "insert into Bag_Archeive \n";
                query += "SELECT bagNumber, \n"
                   + "	createdBy, \n"
                   + "	createdOn, \n"
                   + "	modifiedBy, \n"
                   + "	modifiedOn, \n"
                   + "	totalWeight, \n"
                   + "	transportationType, \n"
                   + "	transportNumber, \n"
                   + "	[description], \n"
                   + "	origin, \n"
                   + "	destination, \n"
                   + "	transportDescription, \n"
                   + "	date, \n"
                   + "	[status], \n"
                   + "	expressCenterCode, \n"
                   + "	branchCode, \n"
                   + "	zoneCode, \n"
                   + "	destinationHash, \n"
                   + "	transportationNature, \n"
                   + "	sealNo, \n"
                   + "	bagScreenId FROM bag where bagnumber='" + clvar.BagNumber + "'";


                // Bag Out Pieces
                string query1_ = "INSERT INTO BagOutpieceAssociation_Archeive \n";
                query1_ += "SELECT  * from BagOutpieceAssociation where bagnumber='" + clvar.BagNumber + "'";

                // Bag Manifest
                string query2_ = "INSERT INTO BagManifest_Archeive \n";
                query2_ += "SELECT  * from BagManifest where bagnumber='" + clvar.BagNumber + "' ";

                sqlcmd.CommandText = query;
                count = sqlcmd.ExecuteNonQuery();
                if (count == 0)
                {
                    trans.Rollback();
                    count = 0;
                    return "NOT OK";
                }
                if (dt_Manifest.Rows.Count > 0)
                {
                    if (query2_.Length != 0)
                    {
                        sqlcmd.CommandText = query2_;
                        count = sqlcmd.ExecuteNonQuery();
                        if (count == 0)
                        {
                            trans.Rollback();
                            count = 0;
                            return "NOT OK";
                        }
                    }
                }
                if (dt_cn.Rows.Count > 0)
                {
                    if (query1_.Length != 0)
                    {

                        sqlcmd.CommandText = query1_;
                        count = sqlcmd.ExecuteNonQuery();
                        if (count == 0)
                        {
                            trans.Rollback();
                            count = 0;
                            return "NOT OK";
                        }
                    }
                }

                trans.Commit();
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

        private static DataTable getManifest(Cl_Variables clvar)
        {
            string query = "select * from BagManifest bm where  bm.bagNumber = '" + clvar.BagNumber + "'";
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

        private static DataTable getOutpiece(Cl_Variables clvar)
        {
            string query = "select * from BagOutpieceAssociation bm   WHERE bm.bagNumber = '" + clvar.BagNumber + "'";
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
            Variable clvar = new Variable();
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
        public static string[][] CheckControls(string cn)
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
                    Response[1] = "Error: no booking found for this COD CN";
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
            string query = @"SELECT * ,(select count(*) from tbl_CODControlBypass where isactive = 1 and consignmentNumber = '" + Consignment + @"') bypass FROM Consignment c 
            inner join (select consignmentnumber, sum(AtDest) as AtDest, sum(allowRTN) as allowRTN from (
            select '" + Consignment + @"' consignmentnumber, 0 AtDest, 0 allowRTN union
            select consignmentnumber, case when Reason in ('70', '58') then 0 else 1 end AtDest, 0 as allowRTN from RunsheetConsignment where consignmentNumber = '" + Consignment + @"' 
            and createdOn = (select max(createdon) from RunsheetConsignment where consignmentNumber = '" + Consignment + @"' ) union
            select consignmentnumber, 0 as AtDest, case when COUNT(*) > 0 then 1 else 0 end as allowRTN 
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