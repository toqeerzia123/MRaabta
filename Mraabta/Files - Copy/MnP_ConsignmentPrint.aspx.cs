using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using MRaabta.App_Code;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace MRaabta.Files
{
    public partial class MnP_ConsignmentPrint : System.Web.UI.Page
    {
        private DataTable _Consignments;
        public DataTable Consignments
        {
            get { return _Consignments; }
            set { _Consignments = value; }
        }
        string originExpressCenter = "";

        public DataTable CurrentCity
        {
            get
            {
                return Consignments;
            }
        }

        Cl_Variables clvar = new Cl_Variables();
        protected void Page_Load(object sender, EventArgs e)
        {
            Errorid.Text = "";
            err_msg.Text = "";



        }
        public DataTable Riders()
        {
            string query = "SELECT * FROM RIDERS r where r.branchID = '" + HttpContext.Current.Session["BranchCode"].ToString() + "' and r.status = '1'";
            DataTable dt = new DataTable();

            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                sda.Fill(dt);
                ViewState["Riders"] = dt;
            }
            catch (Exception ex)
            {
            }
            finally { con.Close(); }
            return dt;
        }
        protected void btn_search_Click(object sender, EventArgs e)
        {
            if (txt_bookingDate.Text == "" && txt_gatepass.Text.Trim() == "")
            {
                return;
            }

            clvar.BookingDate = txt_bookingDate.Text;
            clvar.AccountNo = txt_accountNumber.Text;
            clvar.docPouchNo = txt_gatepass.Text;
            DataTable dt = new DataTable();
            if (txt_gatepass.Text == "")
            {
                dt = GetConsignments(clvar);
            }
            else
            {
                dt = GetConsignmentsByGatepass(clvar);
            }
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    gv_consignments.DataSource = dt;
                    gv_consignments.DataBind();
                }
            }

        }
        public DataTable GetConsignmentsByGatepass(Cl_Variables clvar)
        {
                BridgeConnector.Connector connector = new BridgeConnector.Connector();
                DataTable dt = new DataTable();

            string sqlString = @"SELECT DISTINCT D.GATEPASS_NO, D.CONSIGNMENT_NO
  FROM OFUSION.OF_SHIPMENT_DETAILS D
INNER JOIN SM_DISTRIBUTOR_CNSEQUENCE C
    ON C.COMPANY = D.COMPANY
   AND C.DISTRIBUTOR = D.FROMLOCATION
WHERE D.COMPANY = '01'
   AND TRIM(UPPER(C.ACCOUNT1)) = UPPER('" + clvar.AccountNo + "')   AND D.GATEPASS_NO = '" + clvar.docPouchNo + "'";
            //    string sqlString = "" +
            //    "SELECT *\n" +
            //    "  FROM gatepass_consignment gc\n" +
            //    " INNER JOIN sm_distributor_cnsequence sdc\n" +
            //    "    ON sdc.company = gc.company\n" +
            //    "   AND sdc.distributor = gc.distributor\n" +
            //    " WHERE TRIM(UPPER(sdc.account1)) = UPPER('" + clvar.AccountNo + "')\n" +
            //    "   AND gc.doc_no = '" + clvar.docPouchNo + "'";

            dt = connector.Get_datatable(Encrypt(sqlString));
            string cns = "";
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    cns += "'" + dr["Consignment_NO"].ToString() + "'";
                }

                cns = cns.Replace("''", "','");
                dt = GetConsignmentsByConsignmentNumbers(clvar, cns);
            }


            return dt;
        }
        public DataTable GetConsignmentsByConsignmentNumbers(Cl_Variables clvar, string cns)
        {

            string sqlString = @"selecT c.consignmentNumber,
                    c.consigner,                    c.consignee,                     CAST(CAST(c.bookingDate as date) as varchar) bookingDate,                    serviceTypeName,
                    b.name ORIGIN,                    b1.name Destination,                    c.riderCode,                    c.weight,
                    c.pieces, dbo.splitstring(CAST(c.couponNumber AS VARCHAR), ';', 1) AS  couponNumber, dbo.splitstring(CAST(c.couponNumber AS VARCHAR), ';', 2) AS GP
               from consignment c
              inner join creditclients cc
                 on cc.id = c.creditClientId
              inner join branches b
                 on b.branchCode = c.orgin
              inner join branches b1
                 on b1.branchCode = c.destination
              where c.consignerAccountNo = '" + clvar.AccountNo + "'   and c.branchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "' and c.consignmentNumber in (" + cns + ") order by c.consignmentNumber";
            DataTable dt = new DataTable();

            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sqlString, con);
                sda.Fill(dt);
            }
            catch (Exception ex)
            {
            }
            finally { con.Close(); }
            return dt;
        }
        public DataTable GetConsignments(Cl_Variables clvar)
        {

            string sqlString = @"selecT c.consignmentNumber,
                    c.consigner,                    c.consignee,                    CAST(CAST(c.bookingDate as date) as varchar) bookingDate,                    serviceTypeName,
                    b.name ORIGIN,                    b1.name Destination,                    c.riderCode,                    c.weight,
                    c.pieces, dbo.splitstring(CAST(c.couponNumber AS VARCHAR), ';', 1) AS  couponNumber, dbo.splitstring(CAST(c.couponNumber AS VARCHAR), ';', 2) AS GP
               from consignment c
              inner join creditclients cc
                 on cc.id = c.creditClientId
              inner join branches b
                 on b.branchCode = c.orgin
              inner join branches b1
                 on b1.branchCode = c.destination  where CAST(c.bookingDate as date) = '" + clvar.BookingDate + "'    and c.consignerAccountNo = '" + clvar.AccountNo + "'   and c.branchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "' order by c.consignmentNumber";
            DataTable dt = new DataTable();

            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sqlString, con);
                sda.Fill(dt);
            }
            catch (Exception ex)
            {
            }
            finally { con.Close(); }
            return dt;
        }
        protected void btn_reset_Click(object sender, EventArgs e)
        {

        }
        protected void btn_save_Click(object sender, EventArgs e)
        {
            DataTable riders = Riders();
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[] {
        new DataColumn("ConsignmentNumber"),
        new DataColumn("RiderCode"),
        new DataColumn("Weight"),
        new DataColumn("Pieces")
        });
            foreach (GridViewRow row in gv_consignments.Rows)
            {
                double tempweight = 0;
                int temppieces = 0;
                TextBox txt_rider = row.FindControl("txt_gRider") as TextBox;
                TextBox weight = row.FindControl("txt_gWeight") as TextBox;
                TextBox pieces = row.FindControl("txt_gPieces") as TextBox;
                CheckBox chk = row.FindControl("chk_print") as CheckBox;
                if (chk.Checked)
                {
                    if (txt_rider.Text == "")
                    {
                        txt_rider.Focus();
                        row.BackColor = Color.Red;
                        Alert("Enter Rider Code", "Red");
                        return;
                    }
                    else
                    {
                        row.BackColor = Color.White;
                    }

                    double.TryParse(weight.Text.ToString(), out tempweight);
                    int.TryParse(pieces.Text, out temppieces);
                    if (tempweight <= 0)
                    {
                        weight.Focus();
                        row.BackColor = Color.Red;
                        Alert("Enter Weight", "Red");
                        return;
                    }
                    else
                    {
                        row.BackColor = Color.White;
                    }
                    if (temppieces <= 0)
                    {
                        pieces.Focus();
                        row.BackColor = Color.Red;
                        Alert("Enter Pieces", "Red");
                        return;
                    }
                    else
                    {

                    }

                    if (riders.Select("riderCode = '" + txt_rider.Text + "'").Count() == 0)
                    {
                        txt_rider.Focus();
                        row.BackColor = Color.Red;
                        Alert("Invalid Rider Code", "Red");
                        txt_rider.Text = "";

                        return;
                    }
                    else
                    {
                        row.BackColor = Color.White;
                    }
                }


                if (chk.Checked)
                {

                    DataRow dr = dt.NewRow();
                    dr["ConsignmentNumber"] = row.Cells[1].Text.ToString();
                    dr["RiderCode"] = txt_rider.Text;
                    dr["Weight"] = tempweight.ToString();
                    dr["Pieces"] = temppieces.ToString();
                    dt.Rows.Add(dr);
                }



            }


            if (dt.Rows.Count > 0)
            {
                List<string> cn = new List<string>();
                foreach (DataRow dr in dt.Rows)
                {
                    cn.Add(dr["ConsignmentNumber"].ToString());

                }
                List<string> error = UpdateUnupdatedConsignments(cn);

                if (error[0] == "OK")
                {
                    cn = new List<string>();
                    foreach (DataRow dr in dt.Rows)
                    {
                        cn.Add(dr["ConsignmentNumber"].ToString());

                    }
                    Session["GPList"] = cn;
                    string script_ = @"window.open(""{0}"", ""{1}"", ""{2}"");";
                    string script = String.Format(script_, "BarcodePrinter.aspx", "_blank", "");
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Redirect", script, true);
                }
                else
                {
                    Alert(error[1].ToString(), "Red");
                }
            }
        }

        public void Alert(string message, string color)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + message + "')", true);
            Errorid.Text = message;
            err_msg.Text = message;
            Errorid.ForeColor = Color.FromName(color);
            err_msg.ForeColor = Color.FromName(color);
        }
        public List<string> UpdateConsignments(DataTable cns)
        {
            List<string> resp = new List<string>();
            string updateCommand = "";

            List<string> commands = new List<string>();
            foreach (DataRow dr in cns.Rows)
            {
                updateCommand = " UPDATE Consignment set RiderCode = '" + dr["Ridercode"].ToString() + "', Weight='" + dr["weight"].ToString() + "', pieces = '" + dr["Pieces"].ToString() + "', originExpressCenter = '" + dr["OriginEC"].ToString() + "', createdBy = '" + HttpContext.Current.Session["U_ID"].ToString() + "' \n";
                if (dr["ispriceComputed"].ToString() == "0")
                {
                    updateCommand += ", ispriceComputed = '" + dr["ispriceComputed"].ToString() + "'";
                }
                updateCommand += "  where consignmentNumber = '" + dr["ConsignmentNumber"].ToString() + "'";
                commands.Add(updateCommand);
            }


            SqlConnection con = new SqlConnection(clvar.Strcon());
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.Connection = con;
            try
            {
                con.Open();
                foreach (string command in commands)
                {
                    cmd.CommandText = command;
                    cmd.ExecuteNonQuery();
                }

                resp.Add("OK");

            }
            catch (Exception ex)
            {
                resp.Add("NOT OK");
                resp.Add(ex.Message);
            }
            finally { con.Close(); }

            return resp;
        }
        protected void btn_print_Click(object sender, EventArgs e)
        {
            DataTable riders = Riders();
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[] {
        new DataColumn("ConsignmentNumber"),
        new DataColumn("RiderCode"),
        new DataColumn("Weight"),
        new DataColumn("Pieces")
        });

            foreach (GridViewRow row in gv_consignments.Rows)
            {
                double tempweight = 0;
                int temppieces = 0;
                TextBox txt_rider = row.FindControl("txt_gRider") as TextBox;
                TextBox weight = row.FindControl("txt_gWeight") as TextBox;
                TextBox pieces = row.FindControl("txt_gPieces") as TextBox;
                CheckBox chk = row.FindControl("chk_print") as CheckBox;
                if (chk.Checked)
                {
                    if (txt_rider.Text == "")
                    {
                        txt_rider.Focus();
                        row.BackColor = Color.Red;
                        Alert("Enter Rider Code", "Red");
                        return;
                    }
                    else
                    {
                        row.BackColor = Color.White;
                    }
                    tempweight = 0;
                    temppieces = 0;
                    double.TryParse(weight.Text.ToString(), out tempweight);
                    int.TryParse(pieces.Text, out temppieces);
                    if (tempweight <= 0)
                    {
                        weight.Focus();
                        row.BackColor = Color.Red;
                        Alert("Enter Weight", "Red");
                        return;
                    }
                    else
                    {
                        row.BackColor = Color.White;
                    }
                    if (temppieces <= 0)
                    {
                        pieces.Focus();
                        row.BackColor = Color.Red;
                        Alert("Enter Pieces", "Red");
                        return;
                    }
                    else
                    {

                    }

                    if (riders.Select("riderCode = '" + txt_rider.Text + "'").Count() == 0)
                    {
                        txt_rider.Focus();
                        row.BackColor = Color.Red;
                        Alert("Invalid Rider Code", "Red");
                        txt_rider.Text = "";

                        return;
                    }
                    else
                    {
                        row.BackColor = Color.White;
                    }
                }


                if (chk.Checked)
                {

                    DataRow dr = dt.NewRow();
                    dr["ConsignmentNumber"] = row.Cells[1].Text.ToString();
                    dr["RiderCode"] = txt_rider.Text;
                    dr["Weight"] = tempweight.ToString();
                    dr["Pieces"] = temppieces.ToString();
                    dt.Rows.Add(dr);
                }



            }


            if (dt.Rows.Count > 0)
            {

                List<string> cn = new List<string>();

                foreach (DataRow dr in dt.Rows)
                {
                    cn.Add(dr["ConsignmentNumber"].ToString());

                }
                Session["GPList"] = cn;
                string script_ = @"window.open(""{0}"", ""{1}"", ""{2}"");";
                string script = String.Format(script_, "BarcodePrinter.aspx", "_blank", "");
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Redirect", script, true);

            }
        }
        protected void btn_printLoadSheet_Click(object sender, EventArgs e)
        {
            //Consignments = new DataTable();
            //Consignments.Columns.Add("CNNumber");
            //DataTable dt = new DataTable();

            //foreach (GridViewRow row in gv_consignments.Rows)
            //{
            //    CheckBox chkprint = row.FindControl("chk_print") as CheckBox;

            //    if (chkprint.Checked)
            //    {
            //        DataRow dr = Consignments.NewRow();

            //        dr[0] = row.Cells[1].Text;

            //        Consignments.Rows.Add(dr);

            //    }
            //}

            //Server.Transfer("LoadSheetGenerator.aspx", true);





            List<string> cns = new List<string>();
            DataTable riders = Riders();



            foreach (GridViewRow row in gv_consignments.Rows)
            {
                TextBox txt_rider = row.FindControl("txt_gRider") as TextBox;
                TextBox weight = row.FindControl("txt_gWeight") as TextBox;
                TextBox pieces = row.FindControl("txt_gPieces") as TextBox;
                CheckBox chk = row.FindControl("chk_print") as CheckBox;
                if (chk.Checked)
                {
                    if (txt_rider.Text == "")
                    {
                        txt_riderCode.Focus();
                        //row.BackColor = Color.Red;
                        Alert("Enter Rider Code", "Red");
                        return;
                    }
                    else
                    {
                        row.BackColor = Color.White;
                    }
                    double tempweight = 0;
                    int temppieces = 0;
                    double.TryParse(weight.Text.ToString(), out tempweight);
                    int.TryParse(pieces.Text, out temppieces);
                    if (tempweight <= 0)
                    {
                        weight.Focus();
                        row.BackColor = Color.Red;
                        Alert("Enter Weight", "Red");
                        return;
                    }
                    else
                    {
                        row.BackColor = Color.White;
                    }
                    if (temppieces <= 0)
                    {
                        pieces.Focus();
                        row.BackColor = Color.Red;
                        Alert("Enter Pieces", "Red");
                        return;
                    }
                    else
                    {

                    }

                    if (riders.Select("riderCode = '" + txt_rider.Text + "'").Count() == 0)
                    {
                        //txt_rider.Focus();
                        //row.BackColor = Color.Red;
                        txt_riderCode.Text = "";
                        txt_riderCode.Focus();
                        Alert("Invalid Rider Code", "Red");
                        txt_rider.Text = "";

                        return;
                    }
                    else
                    {
                        row.BackColor = Color.White;
                    }
                }


                if (chk.Checked)
                {
                    cns.Add(row.Cells[1].Text.ToString());

                }



            }

            if (cns.Count > 0)
            {
                List<string> error = UpdateUnupdatedConsignments(cns);
                if (error[0] != "OK")
                {
                    Alert(error[1], "Red");
                    return;
                }
            }

            if (cns.Count > 0)
            {
                Session["cns"] = cns;
                string script_ = @"window.open(""{0}"", ""{1}"", ""{2}"");";
                string script = String.Format(script_, "CNTI_ReconcilePrint.aspx", "_blank", "");
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Redirect", script, true);
            }
        }
        protected void btn_printReconciliation_Click(object sender, EventArgs e)
        {


        }

        protected void btn_HTML_Click(object sender, EventArgs e)
        {
            //Consignments = new DataTable();
            //Consignments.Columns.Add("CNNumber");
            //DataTable dt = new DataTable();

            //foreach (GridViewRow row in gv_consignments.Rows)
            //{
            //    CheckBox chkprint = row.FindControl("chk_print") as CheckBox;

            //    if (chkprint.Checked)
            //    {
            //        DataRow dr = Consignments.NewRow();

            //        dr[0] = row.Cells[1].Text;

            //        Consignments.Rows.Add(dr);

            //    }
            //}

            //Server.Transfer("LoadSheetGenerator.aspx", true);





            List<string> cns = new List<string>();
            DataTable riders = Riders();



            foreach (GridViewRow row in gv_consignments.Rows)
            {
                TextBox txt_rider = row.FindControl("txt_gRider") as TextBox;
                TextBox weight = row.FindControl("txt_gWeight") as TextBox;
                TextBox pieces = row.FindControl("txt_gPieces") as TextBox;
                CheckBox chk = row.FindControl("chk_print") as CheckBox;
                if (chk.Checked)
                {
                    if (txt_rider.Text == "")
                    {
                        txt_riderCode.Focus();
                        //row.BackColor = Color.Red;
                        Alert("Enter Rider Code", "Red");
                        return;
                    }
                    else
                    {
                        row.BackColor = Color.White;
                    }
                    double tempweight = 0;
                    int temppieces = 0;
                    double.TryParse(weight.Text.ToString(), out tempweight);
                    int.TryParse(pieces.Text, out temppieces);
                    if (tempweight <= 0)
                    {
                        weight.Focus();
                        row.BackColor = Color.Red;
                        Alert("Enter Weight", "Red");
                        return;
                    }
                    else
                    {
                        row.BackColor = Color.White;
                    }
                    if (temppieces <= 0)
                    {
                        pieces.Focus();
                        row.BackColor = Color.Red;
                        Alert("Enter Pieces", "Red");
                        return;
                    }
                    else
                    {

                    }

                    if (riders.Select("riderCode = '" + txt_rider.Text + "'").Count() == 0)
                    {
                        //txt_rider.Focus();
                        //row.BackColor = Color.Red;
                        txt_riderCode.Text = "";
                        txt_riderCode.Focus();
                        Alert("Invalid Rider Code", "Red");
                        txt_rider.Text = "";

                        return;
                    }
                    else
                    {
                        row.BackColor = Color.White;
                    }
                }


                if (chk.Checked)
                {
                    cns.Add(row.Cells[1].Text.ToString());

                }



            }

            if (cns.Count > 0)
            {
                List<string> error = UpdateUnupdatedConsignments(cns);
                if (error[0] != "OK")
                {
                    Alert(error[1], "Red");
                    return;
                }
            }

            if (cns.Count > 0)
            {
                Session["cns"] = cns;
                string script_ = @"window.open(""{0}"", ""{1}"", ""{2}"");";
                string script = String.Format(script_, "HTMLPrint.aspx", "_blank", "");
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Redirect", script, true);
            }
        }


        protected List<string> UpdateUnupdatedConsignments(List<string> cns)
        {
            List<string> resp = new List<string>();
            DataTable dt = new DataTable();

            DataTable cnToBeUpdated = new DataTable();
            cnToBeUpdated.Columns.Add(new DataColumn("ConsignmentNumber"));
            cnToBeUpdated.Columns.Add(new DataColumn("RiderCode"));
            cnToBeUpdated.Columns.Add(new DataColumn("Weight"));
            cnToBeUpdated.Columns.Add(new DataColumn("Pieces"));
            cnToBeUpdated.Columns.Add(new DataColumn("OriginEC"));
            cnToBeUpdated.Columns.Add(new DataColumn("ispriceComputed"));
            DataTable riders = Riders();




            string cn = "";
            #region MyRegion
            foreach (string str in cns)
            {
                cn += "'" + str + "'";
            }
            cn = cn.Replace("''", "','");
            #endregion
            string query = "selecT consignmentNumber, ridercode, weight, pieces from consignment where consignmentnumber in (" + cn + ")";

            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    foreach (GridViewRow row in gv_consignments.Rows)
                    {
                        TextBox rider = row.FindControl("txt_gRider") as TextBox;
                        TextBox weight = row.FindControl("txt_gWeight") as TextBox;
                        TextBox pieces = row.FindControl("txt_gPieces") as TextBox;

                        foreach (DataRow existing in dt.Rows)
                        {
                            if (existing["Consignmentnumber"].ToString() == row.Cells[1].Text.ToString())
                            {
                                DataRow dr = cnToBeUpdated.NewRow();
                                if (rider.Text != existing["RiderCode"].ToString() || weight.Text != existing["Weight"].ToString() || pieces.Text != existing["Pieces"].ToString())
                                {
                                    dr["ConsignmentNumber"] = existing["ConsignmentNumber"].ToString();
                                    dr["RiderCode"] = rider.Text;
                                    dr["weight"] = weight.Text;
                                    dr["pieces"] = pieces.Text;
                                    if (riders.Rows.Count > 0)
                                    {
                                        DataRow[] dr_ = riders.Select("RiderCode = '" + rider.Text + "'");
                                        originExpressCenter = dr_[0]["ExpressCenterID"].ToString();

                                    }
                                    dr["OriginEC"] = originExpressCenter;
                                    if (rider.Text != existing["RiderCode"].ToString() || weight.Text != existing["Weight"].ToString())
                                    {
                                        dr["ispriceComputed"] = "0";
                                    }
                                    else
                                    {
                                        dr["ispriceComputed"] = "1";
                                    }


                                    cnToBeUpdated.Rows.Add(dr);
                                }
                            }
                        }


                    }


                }

                if (cnToBeUpdated.Rows.Count > 0)
                {
                    resp = UpdateConsignments(cnToBeUpdated);
                }
                else
                {
                    resp.Add("OK");
                }
            }
            catch (Exception ex)
            { resp.Add("NOT OK"); resp.Add(ex.Message); }
            finally
            { con.Close(); }

            return resp;
        }


        protected void sssss_Click(object sender, EventArgs e)
        {

        }



        private static string sKey = "UJYHCX783her*&5@$%#(MJCX**38n*#6835ncv56tvbry(&#MX98cn342cn4*&X#&";
        protected static string EncryptString(string InputText, string Password)
        {

            // "Password" string variable is nothing but the key(your secret key) value which is sent from the front end.

            // "InputText" string variable is the actual password sent from the login page.

            // We are now going to create an instance of the

            // Rihndael class.

            RijndaelManaged RijndaelCipher = new RijndaelManaged();

            // First we need to turn the input strings into a byte array.

            byte[] PlainText = System.Text.Encoding.Unicode.GetBytes(InputText);

            // We are using Salt to make it harder to guess our key

            // using a dictionary attack.

            byte[] Salt = Encoding.ASCII.GetBytes(Password.Length.ToString());

            // The (Secret Key) will be generated from the specified

            // password and Salt.

            //PasswordDeriveBytes -- It Derives a key from a password

            PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(Password, Salt);

            // Create a encryptor from the existing SecretKey bytes.

            // We use 32 bytes for the secret key

            // (the default Rijndael key length is 256 bit = 32 bytes) and

            // then 16 bytes for the IV (initialization vector),

            // (the default Rijndael IV length is 128 bit = 16 bytes)

            ICryptoTransform Encryptor = RijndaelCipher.CreateEncryptor(SecretKey.GetBytes(16), SecretKey.GetBytes(16));

            // Create a MemoryStream that is going to hold the encrypted bytes

            MemoryStream memoryStream = new MemoryStream();

            // Create a CryptoStream through which we are going to be processing our data.

            // CryptoStreamMode.Write means that we are going to be writing data

            // to the stream and the output will be written in the MemoryStream

            // we have provided. (always use write mode for encryption)

            CryptoStream cryptoStream = new CryptoStream(memoryStream, Encryptor, CryptoStreamMode.Write);

            // Start the encryption process.

            cryptoStream.Write(PlainText, 0, PlainText.Length);

            // Finish encrypting.

            cryptoStream.FlushFinalBlock();

            // Convert our encrypted data from a memoryStream into a byte array.

            byte[] CipherBytes = memoryStream.ToArray();

            // Close both streams.

            memoryStream.Close();

            cryptoStream.Close();

            // Convert encrypted data into a base64-encoded string.

            // A common mistake would be to use an Encoding class for that.

            // It does not work, because not all byte values can be

            // represented by characters. We are going to be using Base64 encoding

            // That is designed exactly for what we are trying to do.

            string EncryptedData = Convert.ToBase64String(CipherBytes);

            // Return encrypted string.

            return EncryptedData;

        }



        protected static string DecryptString(string InputText, string Password)
        {

            try
            {

                RijndaelManaged RijndaelCipher = new RijndaelManaged();

                byte[] EncryptedData = Convert.FromBase64String(InputText);

                byte[] Salt = Encoding.ASCII.GetBytes(Password.Length.ToString());

                PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(Password, Salt);

                // Create a decryptor from the existing SecretKey bytes.

                ICryptoTransform Decryptor = RijndaelCipher.CreateDecryptor(SecretKey.GetBytes(16), SecretKey.GetBytes(16));

                MemoryStream memoryStream = new MemoryStream(EncryptedData);

                // Create a CryptoStream. (always use Read mode for decryption).

                CryptoStream cryptoStream = new CryptoStream(memoryStream, Decryptor, CryptoStreamMode.Read);

                // Since at this point we don't know what the size of decrypted data

                // will be, allocate the buffer long enough to hold EncryptedData;

                // DecryptedData is never longer than EncryptedData.

                byte[] PlainText = new byte[EncryptedData.Length];

                // Start decrypting.

                int DecryptedCount = cryptoStream.Read(PlainText, 0, PlainText.Length);

                memoryStream.Close();

                cryptoStream.Close();

                // Convert decrypted data into a string.

                string DecryptedData = Encoding.Unicode.GetString(PlainText, 0, DecryptedCount);

                // Return decrypted string.

                return DecryptedData;

            }

            catch (Exception exception)
            {

                return (exception.Message);

            }

        }



        public static string Encrypt(string sPainText)
        {

            if (sPainText.Length == 0)

                return (sPainText);

            return (EncryptString(sPainText, sKey));

        }



        public static string Decrypt(string sEncryptText)
        {

            if (sEncryptText.Length == 0)

                return (sEncryptText);

            return (DecryptString(sEncryptText, sKey));

        }


        protected void btn_ExportToCSV_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("CONSIGNMENT", typeof(string));
            dt.Columns.Add("CONSIGNER", typeof(string));
            dt.Columns.Add("CONSIGNEE", typeof(string));
            dt.Columns.Add("BOOKING DATE", typeof(string));
            dt.Columns.Add("SERVICE TYPE", typeof(string));
            dt.Columns.Add("ORIGIN", typeof(string));
            dt.Columns.Add("DESTINATION", typeof(string));
            dt.Columns.Add("TI", typeof(string));
            dt.Columns.Add("RIDER", typeof(string));
            dt.Columns.Add("PIECES", typeof(string));
            dt.Columns.Add("WEIGHT", typeof(string));

            foreach (GridViewRow row in gv_consignments.Rows)
            {
                TextBox txt_rider = row.FindControl("txt_gRider") as TextBox;
                TextBox weight = row.FindControl("txt_gWeight") as TextBox;
                TextBox pieces = row.FindControl("txt_gPieces") as TextBox;
                CheckBox chk = row.FindControl("chk_print") as CheckBox;
                if (chk.Checked)
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = String.Format("'{0}", row.Cells[1].Text.ToString());
                    dr[1] = row.Cells[2].Text.ToString();
                    dr[2] = row.Cells[3].Text.ToString();
                    dr[3] = row.Cells[4].Text.ToString();
                    dr[4] = row.Cells[5].Text.ToString();
                    dr[5] = row.Cells[6].Text.ToString();
                    dr[6] = row.Cells[7].Text.ToString();
                    dr[7] = String.Format("'{0}", row.Cells[8].Text.ToString());
                    dr[8] = txt_rider.Text;
                    dr[9] = pieces.Text;
                    dr[10] = weight.Text;
                    dt.Rows.Add(dr);
                }
            }
            Session["ExtractDtToCSV"] = dt;
            Response.Redirect("~/DownloadCSV/Index");
        }
    }
}