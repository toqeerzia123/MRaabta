using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using MRaabta.App_Code;
using System.IO;
using Microsoft.VisualBasic.FileIO;

namespace MRaabta.Files
{
    public partial class PrebookingDataLoader : System.Web.UI.Page
    {
        Cl_Variables clvar = new Cl_Variables();
        string originName = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            err_msg.Text = "";
            Errorid.Text = "";
            UpdatePanel mainPanel = Page.Master.FindControl("mainPanel") as UpdatePanel;
            UpdatePanelControlTrigger trigger = new PostBackTrigger();

            trigger.ControlID = btn_upload.UniqueID;
            mainPanel.Triggers.Add(trigger);
            trigger = new PostBackTrigger();
            trigger.ControlID = txt_AccountNumber.UniqueID;
            mainPanel.Triggers.Add(trigger);

            trigger = new PostBackTrigger();
            trigger.ControlID = gv_consignments.UniqueID;
            mainPanel.Triggers.Add(trigger);

            if (!IsPostBack)
            {

                //originName = 
                txt_date.Text = DateTime.Today.ToString("yyyy-MM-dd");
            }
        }


        protected void txt_AccountNumber_TextChanged(object sender, EventArgs e)
        {
            if (txt_AccountNumber.Text.Trim() == "")
            {
                txt_accountName.Text = "";
            }
            clvar.AccountNo = txt_AccountNumber.Text.Trim();
            clvar.Branch = HttpContext.Current.Session["BranchCode"].ToString();

            DataTable dt = CustomerInformation(clvar).Tables[0];

            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["isAPIClient"].ToString().ToUpper() == "TRUE" || dt.Rows[0]["isAPIClient"].ToString().ToUpper() == "1")
                    {
                        txt_accountName.Text = "";
                        txt_AccountNumber.Text = "";
                        Alert("This Account is reserved for COD Portal Only", "Red");
                        return;
                    }
                    else
                    {
                        txt_accountName.Text = dt.Rows[0]["Name"].ToString();
                        hd_clientID.Value = dt.Rows[0]["id"].ToString();
                    }
                }
                else
                {
                    txt_accountName.Text = "";
                    txt_AccountNumber.Text = "";
                    Alert("Account Not Found", "Red");
                    return;
                }
            }
            else
            {
                txt_accountName.Text = "";
                txt_AccountNumber.Text = "";
                Alert("Account Not Found", "Red");
                return;
            }

        }

        public DataSet CustomerInformation(Cl_Variables clvar)
        {
            DataSet Ds_1 = new DataSet();

            try
            {
                string query = "SELECT * FROM CreditClients cc WHERE cc.accountNo = '" + clvar.AccountNo + "' and cc.branchCode='" + clvar.Branch + "'";

                string sqlString = "SELECT cc.*,\n" +
                "       case\n" +
                "         when cu.CreditClientID is not null then\n" +
                "          '1'\n" +
                "         else\n" +
                "          '0'\n" +
                "       end isAPIClient\n" +
                "  FROM CreditClients cc\n" +
                "  left outer join CODUsers cu\n" +
                "    --on cu.accountno = cc.accountno\n" +
                "   on cu.creditCLientID = cc.id and cu.isCod = '1'\n" +
                "\n" +
                " where cc.accountNo = '" + clvar.AccountNo + "'\n" +
                "   and cc.branchCode = '" + clvar.Branch + "'\n" +
                "   and cc.isActive = '1'";


                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(Ds_1);
                orcl.Close();
            }
            catch (Exception Err)
            { }
            finally
            { }

            return Ds_1;
        }

        public void Alert(string message, string color)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + message + "')", true);
            Errorid.Text = message;
            Errorid.ForeColor = System.Drawing.Color.FromName(color);
        }
        protected void btn_upload_Click(object sender, EventArgs e)
        {
            if (txt_AccountNumber.Text.Trim() == "")
            {
                Alert("Enter Account Number Then Select File", "Red");
                return;
            }
            DataTable branches = Cities_();
            DataTable dt = new DataTable();
            if (upload1.HasFile)
            {

                dt.Columns.AddRange(new DataColumn[] {
            new DataColumn("RefNo"),
            new DataColumn("CreditClientID"),
            new DataColumn("AccountNo"),
            new DataColumn("Orgin"),
            new DataColumn("OriginName"),
            new DataColumn("Destination"),
            new DataColumn("Consignee"),
            new DataColumn("Address"),
            new DataColumn("ZoneCode"),
            new DataColumn("BranchCode"),
            new DataColumn("CreatedBy"),
            new DataColumn("Consigner"),
            new DataColumn("PhoneNumber"),
            new DataColumn("RefDate")

            });

                upload1.SaveAs(Server.MapPath(upload1.FileName));
                string filename = upload1.FileName;
                string ext = Path.GetExtension(upload1.FileName);
                if (ext.ToLower() != ".csv")
                {
                    Alert("File Must be a CSV", "Red");
                    return;
                }


                //DataTable dt_ = new DataTable();



                //string csvData = ""; new StringBuilder();
                //csvData = File.ReadAllText(Server.MapPath(upload1.FileName));
                Tuple<string, DataTable> resp = GetDataFromCSV(Server.MapPath(upload1.FileName));
                File.Delete(Server.MapPath(upload1.FileName));
                DataTable csvTable = resp.Item2;


                if (resp.Item1 != "OK")
                {
                    Alert(resp.Item1, "Red");
                    return;
                }
                else
                {
                    #region MyRegion
                    //string totalRows = (csvData.Split('\n').Length - 2).ToString();
                    //int i = 0;
                    //string[] rows = csvData.Split('\n');
                    //for (int i = 0; i < csvTable.Rows.Count; i++)
                    //{

                    //    if (csvTable.Rows[i][0].ToString().Length > 50)
                    //    {

                    //        Alert("Error in Row Number " + (i + 1).ToString() + ". Length of Reference Number exceeds Max Allowed Length.", "Red");
                    //        return;
                    //    }
                    //    else if (csvTable.Rows[i][0].ToString().Length > 70)
                    //    {
                    //        Alert("Error in Row Number " + (i + 1).ToString() + ". Length of Consignee exceeds Max Allowed Length.", "Red");
                    //        return;
                    //    }
                    //    else if (csvTable.Rows[i][0].ToString().Length > 200)
                    //    {
                    //        Alert("Error in Row Number " + (i + 1).ToString() + ". Length of Address exceeds Max Allowed Length.", "Red");
                    //        return;
                    //    }

                    //    DataRow dr = dt.NewRow();

                    //    dr["RefNo"] = csvTable.Rows[i][0].ToString();
                    //    dr["CreditClientID"] = hd_clientID.Value;
                    //    dr["AccountNo"] = txt_AccountNumber.Text;
                    //    dr["Orgin"] = HttpContext.Current.Session["BranchCode"].ToString();
                    //    dr["originName"] = branches.Select("branchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'")[0]["Sname"].ToString();
                    //    dr["Consignee"] = csvTable.Rows[i][1].ToString();
                    //    dr["Address"] = csvTable.Rows[i][2].ToString();
                    //    dr["ZoneCode"] = HttpContext.Current.Session["ZoneCode"].ToString();
                    //    dr["BranchCode"] = HttpContext.Current.Session["BranchCode"].ToString();
                    //    dr["CreatedBy"] = HttpContext.Current.Session["U_ID"].ToString();
                    //    dr["Consigner"] = txt_accountName.Text;
                    //    dr["RefDate"] = csvTable.Rows[i][0].ToString() + ";" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff tt");

                    //    dt.Rows.Add(dr);
                    //    dt.AcceptChanges();

                    //} 
                    #endregion

                    #region MyRegion
                    //for (int i = 0; i < dt_.Rows.Count; i++)
                    //{

                    //    if (dt_.Rows[i][0].ToString().Length > 50)
                    //    {

                    //        Alert("Error in Row Number " + (i + 2).ToString() + ". Length of Reference Number exceeds Max Allowed Length.", "Red");
                    //        return;
                    //    }
                    //    else if (dt_.Rows[i][1].ToString().Length > 70)
                    //    {
                    //        Alert("Error in Row Number " + (i + 2).ToString() + ". Length of Consignee exceeds Max Allowed Length.", "Red");
                    //        return;
                    //    }
                    //    else if (dt_.Rows[i][2].ToString().Length > 200)
                    //    {
                    //        Alert("Error in Row Number " + (i + 2).ToString() + ". Length of Address exceeds Max Allowed Length.", "Red");
                    //        return;
                    //    }



                    //    DataRow dr = dt.NewRow();

                    //    dr["RefNo"] = dt_.Rows[i][0].ToString();
                    //    dr["CreditClientID"] = hd_clientID.Value;
                    //    dr["AccountNo"] = txt_AccountNumber.Text;
                    //    dr["Orgin"] = HttpContext.Current.Session["BranchCode"].ToString();
                    //    dr["originName"] = branches.Select("branchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'")[0]["Sname"].ToString();
                    //    dr["Consignee"] = dt_.Rows[i][1].ToString();
                    //    dr["Address"] = dt_.Rows[i][2].ToString();
                    //    dr["ZoneCode"] = HttpContext.Current.Session["ZoneCode"].ToString();
                    //    dr["BranchCode"] = HttpContext.Current.Session["BranchCode"].ToString();
                    //    dr["CreatedBy"] = HttpContext.Current.Session["U_ID"].ToString();
                    //    dr["Consigner"] = txt_accountName.Text;
                    //    dr["RefDate"] = dt_.Rows[i][0].ToString() + ";" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff tt");

                    //    dt.Rows.Add(dr);
                    //    dt.AcceptChanges();


                    //} 
                    #endregion
                    var query = from row in csvTable.AsEnumerable()
                                group row by row.Field<string>("RefNo") into RefNos
                                orderby RefNos.Key
                                select new
                                {
                                    RefNo = RefNos.Key,
                                    Count = RefNos.Count()
                                };

                    DataTable duplicate = csvTable.Clone();
                    foreach (var refNo in query)
                    {
                        if (refNo.Count > 1)
                        {
                            DataRow[] dr = csvTable.Select("RefNo = '" + refNo.RefNo.ToString() + "'");
                            foreach (DataRow r in dr)
                            {
                                duplicate.Rows.Add(r.ItemArray);
                            }
                        }
                    }

                    if (duplicate.Rows.Count > 0)
                    {
                        Alert("Duplicate Records found in CSV. Please Correct and Upload File again", "Red");
                        gv_consignments.DataSource = duplicate;
                        gv_consignments.DataBind();
                        return;
                    }
                    //gv_consignments.DataSource = dt;
                    //gv_consignments.DataBind();
                    clvar.CreditClientID = hd_clientID.Value;

                    DataTable existing = GetExistingPrebooking(clvar, csvTable);
                    DataTable final = csvTable;


                    #region MyRegion
                    ////DataTable duplicates = dt.Clone();
                    //bool error = false;
                    //foreach (DataRow row in dt.Rows)
                    //{
                    //    if (existing.Select("refrenceNo = '" + row["RefNo"].ToString() + "'").Count() > 0)
                    //    {
                    //        duplicates.Rows.Add(row.ItemArray);
                    //        //row.BackColor = System.Drawing.Color.FromName("#e82e31");
                    //        //row.ForeColor = System.Drawing.Color.White;
                    //        //(row.FindControl("hd_duplicate") as HiddenField).Value = "1";
                    //        error = true;
                    //    }
                    //    else
                    //    {
                    //        final.Rows.Add(row.ItemArray);
                    //        //row.BackColor = System.Drawing.Color.White;
                    //        //row.ForeColor = System.Drawing.Color.Black;
                    //        //(row.FindControl("hd_duplicate") as HiddenField).Value = "0";
                    //    }
                    //} 
                    #endregion
                    ViewState["finalDt"] = final;
                    gv_consignments.DataSource = null;
                    gv_consignments.DataBind();

                    if (existing.Rows.Count > 0)
                    {
                        Alert("Some Records have already been uploaded and will not be updated. Please Check your file and upload again.", "Red");
                        gv_consignments.DataSource = existing;
                        gv_consignments.DataBind();
                        return;
                    }


                    //if (/*dt.Rows.Count == 0 ||*/ gv_consignments.Rows.Count > 0)
                    //{
                    //    Alert("Cannot Upload Data. Data Already Present.", "Red");
                    //    return;
                    //}
                    Tuple<int, string> error = UploadPreCardConsignmentPreBooking(clvar, final);
                    if (error.Item1 == 0)
                    {
                        Alert("Could Not Load Data", "Red");
                        return;

                    }
                    else
                    {
                        Alert("Data Loaded", "Green");
                        final.Clear();
                        ViewState["finalDt"] = final;
                        return;
                    }

                }





            }
        }
        protected void btn_Save_Click(object sender, EventArgs e)
        {
            if (txt_AccountNumber.Text.Trim() == "" || txt_accountName.Text.Trim() == "")
            {
                Alert("Enter Account Number", "Red");
                return;
            }
            DataTable final = ViewState["finalDt"] as DataTable;

            #region MyRegion
            //DataTable dt = new DataTable();
            //dt.Columns.AddRange(new DataColumn[] {
            //new DataColumn("RefNo"),
            //new DataColumn("CreditClientID"),
            //new DataColumn("accountNo"),
            //new DataColumn("orgin"),
            //new DataColumn("destination"),
            //new DataColumn("consignee"),
            //new DataColumn("address"),
            //new DataColumn("createdBy"),
            //new DataColumn("consigner"),
            //new DataColumn("referenceDate")
            //}); 


            //foreach (DataRow row in final.Rows)
            //{
            //    //if ((row.FindControl("hd_duplicate") as HiddenField).Value == "1")
            //    //{
            //    //    continue;
            //    //}
            //    DataRow dr = dt.NewRow();
            //    dr["RefNo"] = row["RefNo"];
            //    dr["CreditClientID"] = row["CreditClientID"];
            //    dr["accountNo"] = row["accountNo"];
            //    dr["orgin"] = row["orgin"];
            //    //dr["destination"] = row.Cells[0].Text;
            //    dr["consignee"] = row["consignee"];
            //    dr["address"] = row["address"];
            //    dr["createdBy"] = row["createdBy"];
            //    dr["consigner"] = row["consigner"];
            //    dr["referenceDate"] = row[0].ToString() + ";" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff tt");
            //    dt.Rows.Add(dr);
            //} 
            #endregion
            #region MyRegion
            //foreach (GridViewRow row in gv_consignments.Rows)
            //{
            //    if ((row.FindControl("hd_duplicate") as HiddenField).Value == "1")
            //    {
            //        continue;
            //    }
            //    DataRow dr = dt.NewRow();
            //    dr["RefNo"] = row.Cells[0].Text;
            //    dr["CreditClientID"] = (row.FindControl("hd_creditClientID") as HiddenField).Value;
            //    dr["accountNo"] = row.Cells[1].Text;
            //    dr["orgin"] = (row.FindControl("hd_origin") as HiddenField).Value;
            //    //dr["destination"] = row.Cells[0].Text;
            //    dr["consignee"] = row.Cells[3].Text;
            //    dr["address"] = (row.FindControl("lbl_gAddress") as Label).Text;
            //    dr["createdBy"] = HttpContext.Current.Session["U_ID"].ToString();
            //    dr["consigner"] = row.Cells[5].Text;
            //    dr["referenceDate"] = (row.FindControl("hd_refDate") as HiddenField).Value;
            //    dt.Rows.Add(dr);
            //} 
            #endregion

            if (/*dt.Rows.Count == 0 ||*/ gv_consignments.Rows.Count > 0)
            {
                Alert("Cannot Upload Data. Data Already Present.", "Red");
                return;
            }
            Tuple<int, string> error = UploadPreCardConsignmentPreBooking(clvar, final);
            if (error.Item1 == 0)
            {
                Alert("Could Not Load Data", "Red");
                return;

            }
            else
            {
                Alert("Data Loaded", "Green");
                final.Clear();
                ViewState["finalDt"] = final;
                return;
            }

            Response.Redirect("PreBookingDataLoader.aspx");
        }
        protected void btn_reset_Click(object sender, EventArgs e)
        {

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

            sqlString = "select b.branchCode , b.sname + ' - ' + b.name SNAME, ec.ExpressCenterCode ECCode\n" +
            "  from branches b\n" +
            "  left outer join ExpressCenters ec\n" +
            "    on ec.bid = b.branchCode\n" +
            "   and ec.Main_EC = '1'\n" +
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

        public Tuple<int, string> UploadPreCardConsignmentPreBooking(Cl_Variables clvar, DataTable dt)
        {
            Tuple<int, string> resp = new Tuple<int, string>(0, "");
            string error = "";
            SqlConnection con = new SqlConnection(clvar.Strcon());
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "MnP_LoadCardConsignmentPreBooking";

                cmd.Parameters.AddWithValue("@loaderTable", dt);
                cmd.Parameters.AddWithValue("@ZoneCode", HttpContext.Current.Session["ZoneCode"].ToString());
                cmd.Parameters.AddWithValue("@BranchCode", HttpContext.Current.Session["BranchCode"].ToString());
                cmd.Parameters.Add("@error_message", SqlDbType.NVarChar, 500).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@error", SqlDbType.Int).Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                error = cmd.Parameters["@error_message"].Value.ToString();
                resp = new Tuple<int, string>(int.Parse(cmd.Parameters["@error"].SqlValue.ToString()), cmd.Parameters["@error_message"].Value.ToString());

            }
            catch (Exception ex)
            {
                resp = new Tuple<int, string>(0, ex.Message);
            }
            finally { con.Close(); }

            return resp;
        }
        public DataTable GetExistingPrebooking(Cl_Variables clvar, DataTable dt_)
        {
            DataTable dt = new DataTable();
            dt_.Columns.Remove("OriginName");
            dt_.Columns.Remove("ZoneCode");
            dt_.Columns.Remove("BranchCode");
            string refNos = "";

            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                SqlCommand cmd = new SqlCommand();
                con.Open();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "MnP_Get_ExistingCardPreBooking";
                cmd.Parameters.AddWithValue("@CreditClientID", clvar.CreditClientID);
                cmd.Parameters.AddWithValue("@zoneCode", HttpContext.Current.Session["ZoneCode"].ToString());
                cmd.Parameters.AddWithValue("@branchCode", HttpContext.Current.Session["BranchCode"].ToString());
                cmd.Parameters.AddWithValue("@loaderTable", dt_);

                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
            }
            catch (Exception ex)
            {
            }
            finally { con.Close(); }
            //foreach (DataRow dr in dt_.Rows)
            //{
            //    refNos += "'" + dr["RefNo"].ToString() + "'";

            //}

            //refNos = refNos.Replace("''", "','");

            //string sqlString = "select *\n" +
            //"  from CardConsignment cc\n" +
            //" where cc.creditClientId = '" + clvar.CreditClientID + "'\n" +
            //"   and cc.isCNGenerated = '0' \n" +
            //"   and cc.refrenceNo in (" + refNos + ")";

            //SqlConnection con = new SqlConnection(clvar.Strcon());
            //con.Open();
            //try
            //{
            //    SqlDataAdapter sda = new SqlDataAdapter(sqlString, con);
            //    sda.Fill(dt);
            //}
            //catch (Exception ex)
            //{

            //}
            //finally { con.Close(); }

            return dt;
        }

        public Tuple<string, DataTable> GetDataFromCSV(string csv_file_path)
        {
            Tuple<string, DataTable> resp = new Tuple<string, DataTable>("", null);
            DataTable branches = Cities_();
            DataTable csvData = new DataTable();
            csvData.Columns.AddRange(new DataColumn[] {
            new DataColumn("RefNo"),
            new DataColumn("CreditClientID"),
            new DataColumn("AccountNo"),
            new DataColumn("Orgin"),
            new DataColumn("OriginName"),
            new DataColumn("Destination"),
            new DataColumn("Consignee"),
            new DataColumn("Address"),
            new DataColumn("ZoneCode"),
            new DataColumn("BranchCode"),
            new DataColumn("CreatedBy"),
            new DataColumn("Consigner"),
            new DataColumn("PhoneNumber"),
            new DataColumn("RefDate")

            });
            try
            {
                using (TextFieldParser csvReader = new TextFieldParser(csv_file_path))
                {
                    csvReader.SetDelimiters(new string[]
                {
                ","
                });
                    csvReader.HasFieldsEnclosedInQuotes = true;
                    string[] colFields = csvReader.ReadFields();
                    foreach (string column in colFields)
                    {
                        DataColumn datecolumn = new DataColumn(column);
                        datecolumn.AllowDBNull = true;
                        //csvData.Columns.Add(datecolumn);
                    }
                    bool error = false;
                    while (!csvReader.EndOfData)
                    {
                        string[] fieldData = csvReader.ReadFields();
                        //Making empty value as null
                        for (int i = 0; i < fieldData.Length; i++)
                        {
                            if (fieldData[i] == string.Empty)
                            {
                                fieldData[i] = string.Empty; //fieldData[i] = null
                            }
                            //Skip rows that have any csv header information or blank rows in them
                            if (fieldData[0].Contains("Disclaimer") || string.IsNullOrEmpty(fieldData[0]))
                            {
                                continue;
                            }
                            if (fieldData[0].ToString().Length > 50)
                            {
                                error = true;
                                //Alert("Error in Row Number " + (i + 1).ToString() + ". Length of Reference Number exceeds Max Allowed Length.", "Red");
                                resp = new Tuple<string, DataTable>("Error in Row Number " + (i + 1).ToString() + ". Length of Reference Number exceeds Max Allowed Length.", null);
                                break;
                                //return;
                            }
                            else if (fieldData[1].ToString().Length > 70)
                            {
                                error = true;
                                //Alert("Error in Row Number " + (i + 1).ToString() + ". Length of Consignee exceeds Max Allowed Length.", "Red");
                                resp = new Tuple<string, DataTable>("Error in Row Number " + (i + 1).ToString() + ". Length of Consignee exceeds Max Allowed Length.", null);
                                break;
                                //return;
                            }
                            else if (fieldData[2].ToString().Length > 200)
                            {
                                error = true;
                                Alert("Error in Row Number " + (i + 1).ToString() + ". Length of Address exceeds Max Allowed Length.", "Red");
                                resp = new Tuple<string, DataTable>("Error in Row Number " + (i + 1).ToString() + ". Length of Address exceeds Max Allowed Length.", null);
                                break;

                                //eturn;
                            }
                            else if (fieldData[3].ToString().Length > 15)
                            {
                                error = true;
                                Alert("Error in Row Number " + (i + 1).ToString() + ". Length of Phone Number exceeds Max Allowed Length.", "Red");
                                resp = new Tuple<string, DataTable>("Error in Row Number " + (i + 1).ToString() + ". Length of Phone Number exceeds Max Allowed Length.", null);
                                break;

                                //eturn;
                            }
                        }
                        if (error)
                        {
                            break;
                        }
                        csvData.Rows.Add(fieldData[0].ToString(), hd_clientID.Value, txt_AccountNumber.Text, HttpContext.Current.Session["BranchCode"].ToString(), branches.Select("branchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'")[0]["Sname"].ToString(), "", fieldData[1].ToString(), fieldData[2].ToString(), HttpContext.Current.Session["ZoneCode"].ToString(), HttpContext.Current.Session["BranchCode"].ToString(), HttpContext.Current.Session["U_ID"].ToString(), txt_accountName.Text , fieldData[3].ToString(), fieldData[0].ToString() + ";" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff tt"));
                    }
                    if (error)
                    {
                        return resp;
                    }
                    else
                    {
                        resp = new Tuple<string, DataTable>("OK", csvData);
                    }
                }
            }
            catch (Exception ex)
            {
                resp = new Tuple<string, DataTable>(ex.Message, null);
            }
            return resp;
        }
    }
}