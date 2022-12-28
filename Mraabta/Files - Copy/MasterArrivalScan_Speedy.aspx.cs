using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Data.SqlClient;
using System.Web.Services;
using MRaabta.App_Code;

namespace MRaabta.Files
{
    public partial class MasterArrivalScan_Speedy : System.Web.UI.Page
    {
        public static Variable clvar = new Variable();
        bayer_Function b_fun = new bayer_Function();

        public class MasterModel
        {
            public string ArrivalID { get; set; }
            public string RiderCode { get; set; }
            public string RiderName { get; set; }
            public string ServiceType { get; set; }
            public string ConType { get; set; }
            public string ConTypeName { get; set; }
            public string OriginExpressCenterCode { get; set; }
            public string TotalWeight { get; set; }
            public string Mode { get; set; }
            public string hd_ExpressCenter_Session { get; set; }
            public string hd_BranchCode { get; set; }
            public string hd_zoneCode { get; set; }
            public string hd_U_ID { get; set; }
            public string hd_LocationID { get; set; }
            public string hd_LocationName { get; set; }

        }
        public class DetailModel
        {
            public string ConsignmentNumber { get; set; }
            public string Weight { get; set; }
            public string Pieces { get; set; }
            public string ServiceType { get; set; }
            public string ConTypeName { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            error_msg.Text = "";
            if (!IsPostBack)
            {
                if (rbtn_mode.SelectedValue == "1")
                {
                    txt_arrivalID.Enabled = true;
                }
                else
                {
                    txt_arrivalID.Enabled = false;
                }


                txt_ridername.ReadOnly = true;
                Get_ServiceType();
                Get_MasterConsignmentType();

                hd_ExpressCenter_Session.Value = Session["ExpressCenter"].ToString();
                hd_BranchCode.Value = Session["BranchCode"].ToString();
                hd_zoneCode.Value = Session["ZoneCode"].ToString();
                hd_U_ID.Value = Session["U_ID"].ToString();
                hd_LocationID.Value = Session["LocationID"].ToString();
                hd_LocationName.Value = Session["LocationName"].ToString();

                //DataTable dt_riders = Get_RIDERS(hd_BranchCode.Value);
                //Cache["dt_riders"] = dt_riders;


                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[] {
            new DataColumn("ConsignmentNumber", typeof(string)),
            new DataColumn("ServiceTypeName", typeof(string)),
            new DataColumn("ConsignmentTypeID", typeof(string)),
            new DataColumn("WEIGHT", typeof(string)),
            new DataColumn("Pieces", typeof(string)),
            new DataColumn("Origin", typeof(string)),
            new DataColumn("Destination", typeof(string)),
            new DataColumn("CreditClientID", typeof(Int64)),
            new DataColumn("ZoneCode", typeof(string)),
            new DataColumn("BranchCode", typeof(string)),
            new DataColumn("ConsignerAccountNo", typeof(string)),
            new DataColumn("RiderCode", typeof(string)),
            new DataColumn("CnInsert", typeof(bool)),
            new DataColumn("isNew", typeof(bool)),
            new DataColumn("Arrived")
            });
                ViewState["tbl_detail"] = dt;
                GetCNLengths();
            }
        }

        public void GetCNLengths()
        {
            string query = "SELECT * FROM MNP_ConsignmentLengths where status = '1'";
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                sda.Fill(dt); ;
                ViewState["cnLengths"] = dt;
            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            cnControls.DataSource = dt;
            cnControls.DataBind();
        }
        public void Get_ServiceType()
        {
            DataSet ds_servicetype = b_fun.Get_ServiceType(clvar);

            if (ds_servicetype.Tables[0].Rows.Count != 0)
            {
                dd_servicetype.DataTextField = "serviceTypeName";
                dd_servicetype.DataValueField = "serviceTypeName";
                dd_servicetype.DataSource = ds_servicetype.Tables[0].DefaultView;
                dd_servicetype.DataBind();
            }
            // dd_servicetype.Items.Insert(0, new ListItem("overnight"));
            dd_servicetype.SelectedValue = "overnight";
        }
        public void Get_MasterConsignmentType()
        {
            DataSet ds_contype = Get_MasterConsignmentType(clvar);

            if (ds_contype.Tables[0].Rows.Count != 0)
            {
                dd_contype.DataTextField = "name";
                dd_contype.DataValueField = "id";
                dd_contype.DataSource = ds_contype.Tables[0].DefaultView;
                dd_contype.DataBind();
            }
            dd_contype.SelectedValue = "12";
        }

        public DataSet Get_MasterConsignmentType(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sqlString = "select ct.id,ct.name,ct.description, CONVERT(VARCHAR(10), ct.createdOn, 105) createdOn, ct.createdBy, ct.modifiedBy, \n" +
                                    "CONVERT(VARCHAR(10), ct.modifiedOn, 105) modifiedOn, \n" +
                                    "CASE WHEN status = '1' THEN 'ACTIVE' ELSE 'INACTIVE' END status \n" +
                                    "from ConsignmentType ct where status ='1'  \n";// +clvar._status;

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
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

        private DataTable Get_RIDERS(string BranchID)
        {
            string query = "SelecT r.firstName + ' ' + r.lastName RiderName, r.* from Riders r where  r.BranchID = '" + BranchID + "' and r.status = '1'";

            SqlConnection con = new SqlConnection(clvar.Strcon());
            DataTable dt = new DataTable();
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                sda.Fill(dt);

            }
            catch (Exception ex)
            {

            }
            finally { con.Close(); }
            return dt;
        }

        [WebMethod]
        public static string[][] ChkRunsheet(string cn)
        {
            List<string[]> resp = new List<string[]>();

            string ConsignmentNo = cn.Trim();
            string reason = "", arrivalid = "", void_Status = "";
            DataSet ds = chkRunsheet(ConsignmentNo);

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string[] consignment = { "", "", "", "", "" };
                    reason = ds.Tables[0].Rows[0]["Reason"].ToString();
                    if (ds.Tables.Count > 1)
                    {
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            arrivalid = ds.Tables[1].Rows[0]["arrivalid"].ToString();
                        }
                        //if (ds.Tables[2].Rows.Count > 0)
                        //{
                        //    void_Status = ds.Tables[2].Rows[0]["consignmentnumber"].ToString();
                        //}
                    }
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
                    else if (arrivalid != "")
                    {
                        consignment[0] = "A";
                        consignment[1] = "Arrived";
                        resp.Add(consignment);

                    }
                    //else if (void_Status != "")
                    //{
                    //    consignment[0] = "V";
                    //    consignment[1] = "Void";
                    //    resp.Add(consignment);

                    //}

                    else
                    {
                        consignment[0] = "";
                        consignment[1] = "";
                        consignment[2] = ds.Tables[0].Rows[0]["weight"].ToString();
                        consignment[3] = ds.Tables[0].Rows[0]["pieces"].ToString();
                        consignment[4] = ds.Tables[0].Rows[0]["codAmount"].ToString();
                        resp.Add(consignment);
                    }
                }
                else
                {
                    string[] consignment = { "" };
                    consignment[0] = "N/A";
                    resp.Add(consignment);
                }

            }
            else
            {
                string[] consignment = { "" };
                string cod = "";

                consignment[0] = "N/A";
                resp.Add(consignment);

            }


            return resp.ToArray();
        }

        private static DataSet chkRunsheet(string Consignment)
        {
            string query = "select * from RunsheetConsignment where Reason in ('59','123') and consignmentnumber = '" + Consignment + "'";

            //string sql = "SELECT consignmentnumber, weight,pieces \n"
            //              + "FROM   Consignment inner join c \n"
            //              + "where consignmentnumber = '" + Consignment + "'";
            string sql = "SELECT c.consignmentNumber, \n"
               + "       c.[weight], \n"
               + "       c.pieces, \n"
               + "       cdn.codAmount \n"
               + "FROM   Consignment c \n"
               + "       LEFT JOIN CODConsignmentDetail_New cdn \n"
               + "            ON  cdn.consignmentNumber = c.consignmentNumber \n"
               + "WHERE  cdn.consignmentNumber = '" + Consignment + "'";

            String sql1 = "Select top(1) arrivalid from ArrivalScan_Detail where consignmentnumber ='" + Consignment + "' order by createdon desc ";

            // Void Check removal
            //string sql2 = "   Select * from consignment c \n" +
            //              "     inner join CODConsignmentDetail_New c2 on c2.consignmentNumber = c.consignmentNumber \n" +
            //              "     where c.cod ='1'    and c2.status ='08' and c.consignmentnumber ='" + Consignment + "' ";
            DataSet dt = new DataSet();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                sda.Fill(dt);

                SqlDataAdapter sda1 = new SqlDataAdapter(sql, con);
                sda1.Fill(dt);

                SqlDataAdapter sda2 = new SqlDataAdapter(sql1, con);
                sda2.Fill(dt, "Arrival");

                //// Void Check removal
                //SqlDataAdapter sda3 = new SqlDataAdapter(sql2, con);
                //sda3.Fill(dt, "Void");
            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return dt;
        }


        protected void btn_saveArrival_Click(object sender, EventArgs e)
        {
            clvar.Services = dd_servicetype.SelectedValue.ToString();
            clvar.ConsignmentType = dd_contype.SelectedValue.ToString();
            clvar.RiderCode = txt_ridercode.Text;
            txt_ridername.Text = Session["ridername"].ToString();

            if (clvar.RiderCode == "" || clvar.RiderCode == null)
            {
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "SaveArrivalMaster('" + false + "','" + "Kindly Enter Rider Code." + "');", true);
                return;
            }
            else if (txt_ridername.Text == "" || txt_ridername.Text == null)
            {
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "SaveArrivalMaster('" + false + "','" + "Kindly Enter Correct Rider Code." + "');", true);
                return;
            }


            Tuple<bool, string> result = InsertMasterData(clvar);
            if (result.Item1)
            {
                txt_arrivalID.Text = result.Item2.ToString();
                txt_ridercode.ReadOnly = true;
                dd_contype.Enabled = false;
                dd_servicetype.Enabled = false;
                btn_saveArrival.Enabled = false;
            }
            else
            {
                txt_arrivalID.Text = "";
                txt_ridercode.ReadOnly = false;
                dd_contype.Enabled = true;
                dd_servicetype.Enabled = true;
                btn_saveArrival.Enabled = true;
            }

            //Page.ClientScript.RegisterStartupScript( this.GetType(), "text", "SaveArrivalMaster()", true);
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "SaveArrivalMaster('" + result.Item1 + "','" + result.Item2 + "');", true);
            //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "SaveArrivalMaster();", true);

            //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "tmp", "<script type='text/javascript'>SaveArrivalMaster();</script>", true);
        }

        public Tuple<bool, string> InsertMasterData(Variable clvar)
        {
            Tuple<bool, string> resp = new Tuple<bool, string>(false, "");


            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "MnP_GenerateArrival_2_parent";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BranchCode", hd_BranchCode.Value);
                cmd.Parameters.AddWithValue("@ZoneCode", hd_zoneCode.Value);
                cmd.Parameters.AddWithValue("@ExpressCenterCode", hd_ExpressCenter_Session.Value);
                cmd.Parameters.AddWithValue("@RiderCode", clvar.RiderCode);
                cmd.Parameters.AddWithValue("@OriginExpressCenterCode", hd_ExpressCenter.Value);
                cmd.Parameters.AddWithValue("@UserID", hd_U_ID.Value);
                cmd.Parameters.AddWithValue("@locationID", hd_LocationID.Value);

                cmd.Parameters.Add("@result", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@ARRIVAL_ID", SqlDbType.BigInt).Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();


                Int64 arrivalID = 0;

                if (cmd.Parameters["@result"].SqlValue.ToString().ToUpper() == "OK")
                {
                    object obj = cmd.Parameters["@ARRIVAL_ID"].SqlValue.ToString();
                    Int64.TryParse(obj.ToString(), out arrivalID);
                    if (arrivalID != 0)
                    {
                        resp = new Tuple<bool, string>(true, arrivalID.ToString());
                    }
                    else
                    {
                        resp = new Tuple<bool, string>(false, "Could Not Generate Arrival");
                    }
                }
                else
                {
                    resp = new Tuple<bool, string>(false, cmd.Parameters["@result"].SqlValue.ToString());
                }

            }
            catch (Exception ex)
            {
                resp = new Tuple<bool, string>(false, ex.Message);
            }
            finally { con.Close(); }



            return resp;
        }

        [WebMethod]
        public static string[] ValidateRider(string RiderCode)
        {
            string[] resp = { "", "", "" };

            try
            {
                DataTable dt_riders = (DataTable)HttpContext.Current.Session["dt_riders"];

                DataRow[] dtRows = dt_riders.Select("riderCode = '" + RiderCode + "'");
                if (dtRows.Count() > 0)
                {
                    resp[0] = dtRows[0]["ridername"].ToString();
                    resp[1] = dtRows[0]["ExpressCenterID"].ToString();
                    resp[2] = "1";
                    HttpContext.Current.Session["ridername"] = resp[0];
                }
                else
                {
                    resp[0] = "";
                    resp[1] = "";
                    resp[2] = "0";
                    HttpContext.Current.Session["ridername"] = "";
                }
            }
            catch (Exception ex)
            {
                resp[0] = "";
                resp[1] = "";
                resp[2] = "0";
            }
            finally { }
            return resp;
        }

        [WebMethod]
        public static string[][] InsertArrival(DetailModel[] consignments, MasterModel MasterParameters)
        {
            //string[] resp = { "", "" };
            List<string[]> resp = new List<string[]>();

            DataTable details = new DataTable();
            details.Columns.AddRange(new DataColumn[] {
            new DataColumn("ConsignmentNumber", typeof(string)),
            new DataColumn("Weight", typeof(float)),
            new DataColumn("Pieces", typeof(int)),
            new DataColumn("ServiceType", typeof(string)),
            new DataColumn("ConsignmentType", typeof(int)),
            new DataColumn("SortOrder", typeof(int))
        });
            long tempArrivalID = 0;
            long.TryParse(MasterParameters.ArrivalID, out tempArrivalID);
            clvar.ArrivalID = tempArrivalID;
            int sortOrder = 1;
            foreach (DetailModel cn in consignments)
            {
                float tempWeight = 0;
                int tempPieces = 0;

                float.TryParse(cn.Weight, out tempWeight);
                int.TryParse(cn.Pieces, out tempPieces);

                if (tempWeight <= 0)
                {
                    string[] temp = { "", "", "" };
                    temp[0] = "0";
                    temp[1] = cn.ConsignmentNumber.Trim();
                    temp[2] = "Invalid Weight";
                    resp.Add(temp);
                    continue;
                }
                else if (tempPieces <= 0)
                {
                    string[] temp = { "", "", "" };
                    temp[0] = "0";
                    temp[1] = cn.ConsignmentNumber.Trim();
                    temp[2] = "Invalid Pieces";
                    resp.Add(temp);
                    continue;
                }

                DataRow dr = details.NewRow();
                dr["ConsignmentNumber"] = cn.ConsignmentNumber.Trim();
                dr["Weight"] = tempWeight;
                dr["Pieces"] = tempPieces;
                dr["ServiceType"] = MasterParameters.ServiceType;
                dr["ConsignmentType"] = MasterParameters.ConType;
                dr["SortOrder"] = sortOrder.ToString();
                sortOrder++;
                string[] temp_ = { "", "", "" };
                details.Rows.Add(dr);
                temp_[0] = "1";
                temp_[1] = cn.ConsignmentNumber;
                temp_[2] = "";
                resp.Add(temp_);
            }

            if (details.Rows.Count == consignments.Count())
            {
                Tuple<bool, string> result = null;
                if (MasterParameters.Mode == "0")
                {
                    result = InsertData(details, MasterParameters);
                }
                else
                {
                    result = EditData(details, MasterParameters);
                }

                if (result.Item1)
                {
                    foreach (string[] item in resp)
                    {
                        if (result.Item2 == "OK")
                        {
                            item[2] = MasterParameters.ArrivalID;
                        }
                    }
                }
                else
                {
                    foreach (string[] item in resp)
                    {
                        item[0] = "0";
                        item[2] = result.Item2;
                    }
                }
            }
            else
            {
                return resp.ToArray();
            }

            return resp.ToArray();
        }

        public static Tuple<bool, string> InsertData(DataTable dt, MasterModel MasterParameters)
        {
            Tuple<bool, string> resp = new Tuple<bool, string>(false, "");

            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "MnP_GenerateArrival_2_child";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@tblDetails", dt);
                cmd.Parameters.AddWithValue("@UserID", MasterParameters.hd_U_ID);
                cmd.Parameters.AddWithValue("@locationName", MasterParameters.hd_LocationName);
                cmd.Parameters.AddWithValue("@ArrivalID", MasterParameters.ArrivalID);
                cmd.Parameters.AddWithValue("@TotalWeight", MasterParameters.TotalWeight);

                cmd.Parameters.Add("@result", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();

                if (cmd.Parameters["@result"].SqlValue.ToString().ToUpper() != "OK")
                {
                    resp = new Tuple<bool, string>(false, MasterParameters.ArrivalID);
                }
                else
                {
                    resp = new Tuple<bool, string>(true, cmd.Parameters["@result"].SqlValue.ToString());
                }
            }
            catch (Exception ex)
            {
                resp = new Tuple<bool, string>(false, ex.Message);
            }
            finally { con.Close(); }



            return resp;
        }

        public static Tuple<bool, string> EditData(DataTable dt, MasterModel MasterParameters)
        {
            Tuple<bool, string> resp = new Tuple<bool, string>(false, "");

            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "MnP_EditArrival_2";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@tblDetails", dt);
                cmd.Parameters.AddWithValue("@ArrivalID", MasterParameters.ArrivalID);
                cmd.Parameters.AddWithValue("@BranchCode", MasterParameters.hd_BranchCode);
                cmd.Parameters.AddWithValue("@ZoneCode", MasterParameters.hd_zoneCode);
                cmd.Parameters.AddWithValue("@ExpressCenterCode", MasterParameters.hd_ExpressCenter_Session);
                cmd.Parameters.AddWithValue("@RiderCode", MasterParameters.RiderCode);
                cmd.Parameters.AddWithValue("@ServiceType", MasterParameters.ServiceType);
                cmd.Parameters.AddWithValue("@ConsignmentType", MasterParameters.ConType);
                cmd.Parameters.AddWithValue("@UserID", MasterParameters.hd_U_ID);
                cmd.Parameters.AddWithValue("@locationName", MasterParameters.hd_LocationName);

                cmd.Parameters.Add("@result", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();

                if (cmd.Parameters["@result"].SqlValue.ToString().ToUpper() == "OK")
                {
                    resp = new Tuple<bool, string>(true, "OK");
                }
                else
                {
                    resp = new Tuple<bool, string>(false, cmd.Parameters["@result"].SqlValue.ToString());
                }

            }
            catch (Exception ex)
            {
                resp = new Tuple<bool, string>(false, ex.Message);
            }
            finally { con.Close(); }
            return resp;
        }
        protected void btn_reset2_Click(object sender, EventArgs e)
        {
            Response.Redirect("MasterArrivalScan_Speedy.aspx");
        }

        public class ReturnEditClass
        {
            public string status { get; set; }
            public string reason { get; set; }
            public MasterModel masterParameters { get; set; }
            public DetailModel[] consignments { get; set; }
        }


        [WebMethod]
        public static ReturnEditClass GetArrivalDetails(string arrivalID, string BranchCode)
        {
            ReturnEditClass resp = new ReturnEditClass();
            List<DetailModel> cns = new List<DetailModel>();

            MasterModel master = new MasterModel();

            DataTable dt = GetArrivalDetailData(arrivalID, BranchCode);
            try
            {


                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        master.ArrivalID = arrivalID.ToString();
                        master.ConType = dt.Rows[0]["ConsignmentType"].ToString();
                        master.ConTypeName = dt.Rows[0]["ConTypeName"].ToString();
                        master.ServiceType = dt.Rows[0]["ServiceType"].ToString();
                        master.RiderCode = dt.Rows[0]["RiderCode"].ToString();
                        master.RiderName = dt.Rows[0]["RiderName"].ToString();
                        master.OriginExpressCenterCode = dt.Rows[0]["OriginExpressCenterCode"].ToString();
                        resp.masterParameters = master;
                        bool error = false;
                        foreach (DataRow dr in dt.Rows)
                        {
                            DetailModel cn = new DetailModel();
                            cn.ConsignmentNumber = dr["ConsignmentNumber"].ToString();
                            cn.Pieces = dr["cnPieces"].ToString();
                            cn.Weight = dr["cnWeight"].ToString();
                            cn.ServiceType = dr["ServiceType"].ToString();
                            cn.ConTypeName = dr["ConTypeName"].ToString();
                            if (dr["ServiceType"].ToString().Trim() == "")
                            {
                                error = true;

                                break;
                            }
                            cns.Add(cn);
                        }
                        if (error)
                        {
                            resp.status = "0";
                            resp.reason = "Use the Old Screen to Edit this Arrival";
                        }
                        else
                        {
                            resp.status = "1";
                            resp.reason = "";
                            resp.consignments = cns.ToArray();
                        }


                        return resp;
                    }
                }
                else
                {

                }

            }
            catch (Exception ex)
            {
                resp.status = "0";
                resp.reason = ex.Message;
            }

            return resp;
        }
        public static DataTable GetArrivalDetailData(string arrivalID, string BranchCode)
        {
            DataTable dt = new DataTable();


            string sqlString = "SELECT a.Id arrivalID,\n" +
                "       a.BranchCode,\n" +
                "       a.RiderCode,\n" +
                "       r.firstName + ' ' + r.lastName RiderName,\n" +
                "       a.OriginExpressCenterCode,\n" +
                "       asd.consignmentNumber,\n" +
                "       asd.cnWeight,\n" +
                "       asd.cnPieces,\n" +
                "       asd.ServiceType,\n" +
                "       asd.ConsignmentType,\n" +
                "       ct.name ConTypeName, asd.sortOrder\n" +
                "  FROM ArrivalScan a\n" +
                " INNER JOIN ArrivalScan_Detail asd\n" +
                "    ON asd.ArrivalID = a.Id\n" +
                " INNER JOIN Riders r\n" +
                "    ON r.riderCode = a.RiderCode\n" +
                "   AND r.branchId = a.BranchCode\n" +
                " INNER JOIN ConsignmentType ct\n" +
                "    ON ct.id = asd.ConsignmentType\n" +
                "WHERE  a.BranchCode = '" + BranchCode + "'\n" +
                "       AND a.Id = '" + arrivalID + "' order by asd.sortOrder desc";


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
    }
}