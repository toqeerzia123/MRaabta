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

namespace MRaabta.Files
{
    public partial class Demanifest_speedy : System.Web.UI.Page
    {
        // public static Variable clvar3 = new Variable();
        bayer_Function b_fun = new bayer_Function();
        DataSet ds_grid = new DataSet();
        cl_Encryption enc = new cl_Encryption();
        public static CommonFunction CF = new CommonFunction();
        static Cl_Variables clvar2 = new Cl_Variables();
        public static Cl_Variables clvar = new Cl_Variables();
        public static Consignemnts con = new Consignemnts();

        static string user = "";
        public static string DestinationName = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            //if(!IsPostBack){
            //    txt_cnNumber.Attributes.Add("onchange", "javascript:Chkgrid("+gv_consignments+")");
            //}
            user = Session["User_Info"].ToString();
            GetCNLengths();
            BindDestinations();

        }
        public void BindDestinations()
        {
            DataSet ds = Branch();
            if (ds != null)
            {
                if (ds.Tables[0] != null)
                {
                    dd_origin.DataSource = ds.Tables[0];
                    dd_origin.DataTextField = "BranchName";
                    dd_origin.DataValueField = "branchCode";
                    dd_origin.DataBind();

                    DestinationName = ds.Tables[0].Select("BranchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'").FirstOrDefault()["BranchName"].ToString();
                }
            }
        }

        public static DataSet Branch()
        {
            DataSet Ds_1 = new DataSet();

            try
            {
                string query = "SELECT b.branchCode, \n"
               + "       b.sname + '-' + b.name     BranchName \n"
               + "FROM   Branches                          b \n"
               + "--where b.[status] ='1' \n"
               + "GROUP BY \n"
               + "       b.branchCode, \n"
               + "       b.name, b.sname order by b.sname ASC";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
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

        public class DetailModel
        {
            public string ConsignmentNumber { get; set; }
            public string status { get; set; }
            public string Reason { get; set; }
            public string Origin { get; set; }
            public string OriginCode { get; set; }
            public string Destination { get; set; }
            public string DestinationCode { get; set; }
            public string ConsignmentType { get; set; }
            public string ServiceType { get; set; }
            public string Weight { get; set; }
            public string DemanifestStateID { get; set; }
        }
        public class MasterModel
        {
            public string Manifest { get; set; }
            public string Type { get; set; }
            public string Date { get; set; }
            public string Origin { get; set; }
            public string OriginCode { get; set; }
            public string Destination { get; set; }
            public string DestinationCode { get; set; }
            public string IsDemanifested { get; set; }
            public string WoManifest { get; set; }
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
                sda.Fill(dt);
                ViewState["cnLengths"] = dt;
            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            cnControls.DataSource = dt;
            cnControls.DataBind();
        }

        //protected void txt_manifestNumber_TextChanged(object sender, EventArgs e)
        //{
        //    clvar.manifestNo = txt_manifestNumber.Text.Trim();
        //    DataSet chk = checkdemanifest(clvar.manifestNo);
        //    DataTable dt = con.GetConsignmentsInManifests(clvar);
        //    DataTable header = con.GetManifestHeader(clvar);
        //    if (chk.Tables[0].Rows.Count > 0)
        //    {
        //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Manifest Already Demanifested')", true);
        //        //btn_save.Enabled = false;
        //    }
        //    else
        //    {

        //        if (dt != null)
        //        {
        //            if (dt.Rows.Count > 0)
        //            {
        //                gv_consignments.DataSource = dt;
        //                gv_consignments.DataBind();
        //                bool demanifested = false;
        //                //foreach (DataRow dr in dt.Rows)
        //                //{
        //                //    if (dr["DemanifestStateID"].ToString().Trim() != "")
        //                //    {
        //                //        demanifested = true;
        //                //    }
        //                //}

        //                //if (demanifested)
        //                //{
        //                //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Manifest Already Demanifested')", true);
        //                //    btn_save.Enabled = false;
        //                //}
        //                if (header.Rows.Count > 0)
        //                {
        //                    txt_manifestNumber.Text = header.Rows[0]["manifestNumber"].ToString();
        //                    txt_destination.Text = header.Rows[0]["Destination"].ToString();
        //                    txt_date.Text = header.Rows[0]["date"].ToString();
        //                    txt_origin.Text = header.Rows[0]["Origin"].ToString();
        //                }
        //            }
        //            else
        //            {
        //                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Invalid Manifest Number')", true);

        //            }
        //        }
        //        else
        //        {

        //        }
        //    }



        //}
        protected void chk_received_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            GridViewRow gr = (GridViewRow)chk.Parent.Parent;
            if (gr.Cells[0].Enabled != true)
            {
                return;
            }
            if (chk.Checked)
            {
                gr.Cells[2].Text = "Received";

            }
            else
            {
                gr.Cells[2].Text = "Short Received";
            }
        }
        protected void btn_reset_Click(object sender, EventArgs e)
        {
            txt_cnNumber.Text = "";
            txt_date.Text = "";
            txt_destination.Text = "";
            txt_manifestNumber.Text = "";
            txt_origin.Text = "";
            //gv_consignments.DataSource = null;
            //gv_consignments.DataBind();
        }
        [WebMethod]
        protected void Exe(bool flag)
        {


            #region Manifest Wala Scene
            //foreach (GridViewRow row in gv_consignments.Rows)
            //{
            //    if (!((row.FindControl("chk_received") as CheckBox).Checked))
            //    {
            //        row.Cells[2].Text = "Short Received";
            //        //(row.FindControl("txt_reason") as TextBox).Text = "Short Received";
            //        flag = true;
            //    }
            //}
            if (flag)
            {
                //divDialogue.Visible = true;
                //divDialogue.Style.Add("display", "block");
            }
            else
            {
                //  btn_okDialogue_Click(sender, e);
            }

            #endregion

        }

        protected void btn_cancel_Click(object sender, EventArgs e)
        {

        }
        protected void btn_print_Click(object sender, EventArgs e)
        {

        }


        protected void gv_consignments_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[2].Text.Trim() != "")
                {
                    e.Row.Cells[0].Enabled = false;
                    if (e.Row.Cells[2].Text.Trim() == "5")
                    {
                        e.Row.Cells[2].Text = "Received";
                    }
                    else if (e.Row.Cells[2].Text.Trim() == "6")
                    {
                        e.Row.Cells[2].Text = "Short Received";
                    }
                    else if (e.Row.Cells[2].Text.Trim() == "7")
                    {
                        e.Row.Cells[2].Text = "Excess Received";
                    }
                }
            }
        }
        protected void btn_cancel2Dialogue_Click(object sender, EventArgs e)
        {

        }
        protected void btn_ok2Dialogue_Click(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static string Demanifest(DetailModel[] consignments)
        {
            //string[] resp = { "", "" };
            List<string[]> resp = new List<string[]>();

            DataTable details = new DataTable();
            details.Columns.AddRange(new DataColumn[] {
            new DataColumn("Manifest", typeof(string)),
            new DataColumn("ConsignmentNumber", typeof(string)),
            new DataColumn("statuscode", typeof(int)),
            new DataColumn("status", typeof(string)),
            new DataColumn("DeManifestStateID", typeof(string)),
            new DataColumn("Reason", typeof(string)),
            new DataColumn("Origin", typeof(string)),
            new DataColumn("Destination", typeof(string)),
            new DataColumn("ConsignmentType", typeof(string)),
               new DataColumn("ServiceType", typeof(string)),
            new DataColumn("Weight", typeof(float))

        });
            //clvar.Services = MasterParameters.ServiceType;
            //clvar.ConsignmentType = MasterParameters.ConType;
            //clvar.RiderCode = MasterParameters.RiderCode;
            //clvar.Expresscentercode = MasterParameters.OriginExpressCenterCode;
            //long tempArrivalID = 0;
            //long.TryParse(MasterParameters.ArrivalID, out tempArrivalID);
            //clvar.ArrivalID = tempArrivalID;
            int sortOrder = 1;
            foreach (DetailModel cn in consignments)
            {
                float tempWeight = 0;
                int tempPieces = 0;

                float.TryParse(cn.Weight, out tempWeight);
                // int.TryParse(cn.Pieces, out tempPieces);

                //if (tempWeight <= 0)
                //{
                //    //temp = {"0", cn.ConsignmentNumber, "Invalid Weight"};
                //    string[] temp = { "", "", "" };
                //    temp[0] = "0";
                //    temp[1] = cn.ConsignmentNumber;
                //    temp[2] = "Invalid Weight";
                //    resp.Add(temp);
                //    continue;
                //}
                //else if (tempPieces <= 0)
                //{
                //    string[] temp = { "", "", "" };
                //    temp[0] = "0";
                //    temp[1] = cn.ConsignmentNumber;
                //    temp[2] = "Invalid Pieces";
                //    resp.Add(temp);
                //    continue;
                //}

                DataRow dr = details.NewRow();

                dr["ConsignmentNumber"] = cn.ConsignmentNumber;

                dr["status"] = cn.status;
                if (dr["status"].ToString() == "Recieved")
                {
                    dr["statuscode"] = "5";
                }
                else if (dr["status"].ToString() == "Short Recieved")
                {
                    dr["statuscode"] = "6";
                }
                else if (dr["status"].ToString() == "Excess Recieved")
                {
                    dr["statuscode"] = "7";
                }
                dr["Reason"] = cn.Reason;
                dr["Origin"] = cn.Origin;
                dr["Destination"] = cn.Destination;
                dr["ConsignmentType"] = cn.ConsignmentType;
                dr["ServiceType"] = cn.ServiceType;
                dr["Weight"] = tempWeight;
                sortOrder++;
                string[] temp_ = { "", "", "" };
                details.Rows.Add(dr);
                temp_[0] = "1";
                temp_[1] = cn.ConsignmentNumber;
                temp_[2] = "";
                resp.Add(temp_);
            }

            string query = "";
            string recv = "";
            string srecv = "";
            string casef = "";
            string casef2 = "";
            bool isrec = false;
            bool issrec = false;
            bool isexcess = false;
            List<String> casestr = new List<string>();
            List<String> casestr2 = new List<string>();
            List<string> queries = new List<string>();
            string manifestnumber = "";


            string q1 = "insert into Mnp_De_Manifest\n" +
            "Select * from mnp_Manifest where manifestnumber ='" + details.Rows[0]["Manifest"].ToString() + "'";



            queries.Add(q1);

            foreach (DataRow row in details.Rows)
            {
                string q2 = "insert into Mnp_ConsignmentDeManifest (consignmentnumber,manifestnumber,statuscode,Reason,DeManifestStateID,Remarks,ModifiedOn,ModifiedBy,weight,pieces,ismergerd)\n" +
                              "values('" + row["ConsignmentNumber"].ToString() + "','" + row["Manifest"].ToString() + "','" + row["statuscode"].ToString() + "','" + row["Reason"].ToString() + "','" + row["statuscode"].ToString() + "','" + row["status"].ToString() + "',getdate(),'" + user + "','" + row["Weight"].ToString() + "','','')";


                //int j = clvar.NormalBags.Count - 1;
                //  clvar.Branch = HttpContext.Current.Session["BranchCode"].ToString();
                //for (int i = 0; i < clvar.NormalBags.Count - 1; i++)
                //{

                //}

                queries.Add(q2);
            }

            #region OLD
            //    foreach(DataRow row in details.Rows){
            //        manifestnumber = row["Manifest"].ToString();
            //        if(row["Status"].ToString()=="Recieved"){


            //            recv += "'" + row["ConsignmentNumber"].ToString() + "'";

            //            isrec = true;
            //        }else if(row["Status"].ToString()=="Short Recieved"){
            //            srecv += "'" + row["ConsignmentNumber"].ToString() + "'";
            //            casestr.Add("WHEN '" + row["ConsignmentNumber"].ToString() + "' THEN '" + row["Reason"].ToString() + "'\n");
            //            casestr2.Add("WHEN '" + row["ConsignmentNumber"].ToString() + "' THEN '" + row["Status"].ToString() + "'\n");
            //            issrec = true;
            //        }
            //        else if(row["Status"].ToString()=="Excess Recieved")
            //        {

            //            query = "insert into MNP_ConsignmentManifest (consignmentNumber,manifestnumber,statuscode,Remarks,DeManifestStateID,Reason)\n" +
            //            "values ('" + row["ConsignmentNumber"] + "','" + row["Manifest"].ToString() + "','7','" + row["Status"].ToString() + "','7','" + row["Reason"].ToString() + "')";
            //            isexcess = true;
            //             //query = " Insert into  MNP_ConsignmentManifest set statusCode = '7',DemanifestStateID = '7' where consignmentNumber in (" + row["ConsignmentNumber"].ToString() + ")";
            //             queries.Add(query);
            //        }
            //    }


            //if(isrec){
            //    recv = recv.Replace("''", "','");
            //    string query1 = " update MNP_ConsignmentManifest set statusCode = '5',DemanifestStateID = '5',Remarks='Recieved' where consignmentNumber in (" + recv + ")";

            //    queries.Add(query1);
            //}
            //    if(issrec==true){
            //        srecv = srecv.Replace("''", "','");
            //        foreach (String casest in casestr)
            //        {
            //            casef += casest;
            //        }
            //        foreach (String casest in casestr2)
            //        {
            //            casef2 += casef2;
            //        }

            //        string query2 = "update MNP_ConsignmentManifest set statusCode = '6', reason = CASE consignmentNumber " + casef + " end where consignmentNumber in (" + srecv + ")";
            //        queries.Add(query2);
            //        string query3 = "update MNP_ConsignmentManifest set DemanifestStateID = '6' , Remarks = Case consignmentNumber " + casef2 + " end where consignmentNumber in (" + srecv + ")";
            //        queries.Add(query3);
            //    }

            #endregion


            string q3 = "INSERT into ConsignmentsTrackingHistory (ConsignmentNumber, StateId, CurrentLocation, manifestNumber, TransactionTime) \n";
            for (int i = 0; i < details.Rows.Count - 1; i++)
            {

                q3 += "  SELECT '" + details.Rows[i]["ConsignmentNumber"].ToString() + "', '7', '', '" + details.Rows[i]["Manifest"].ToString() + "', GETDATE()\n" +
                                "UNION ALL";
            }

            q3 += "  SELECT '" + details.Rows[details.Rows.Count - 1]["ConsignmentNumber"].ToString() + "', '7', '', '" + details.Rows[details.Rows.Count - 1]["Manifest"].ToString() + "', GETDATE()\n";

            queries.Add(q3);

            string q4 = "UPDATE Mnp_De_Manifest \n" +
                        "SET isDemanifested = '1', DemanifestDate = GETDATE(), DemanifestBy = '" + HttpContext.Current.Session["U_ID"].ToString() + "'   " +
                        "WHERE manifestNumber ='" + details.Rows[0]["Manifest"].ToString() + "'";

            queries.Add(q4);

            // query = " update MNP_ConsignmentManifest set statusCode = '6',DemanifestStateID = '6' where consignmentNumber in ('" + row["Reason"].ToString() + "')";

            string err = "";// save(queries); 



            return err;
        }


        public static DataSet checkdemanifest(string manifestnumber)
        {
            DataSet ds = new DataSet();
            //string sql = "  select * from mnp_de_manifest where manifestnumber='" + manifestnumber + "' and isDeManifested='1'";
            //SqlConnection con = new SqlConnection(clvar.Strcon());
            //SqlCommand cmd = new SqlCommand();
            //cmd.Connection = con;
            //cmd.CommandText = sql;
            //cmd.CommandType = CommandType.Text;
            //SqlDataAdapter adp = new SqlDataAdapter(cmd);

            //con.Open();
            //adp.Fill(ds);
            //con.Close();

            return ds;

        }
        private static string save(List<string> queries)
        {
            SqlConnection sqlcon = new SqlConnection(clvar2.Strcon());
            SqlTransaction trans;
            sqlcon.Open();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcon;
            trans = sqlcon.BeginTransaction();
            sqlcmd.Transaction = trans;
            sqlcmd.CommandType = CommandType.Text;

            try
            {
                for (int i = 0; i < queries.Count; i++)
                {
                    int count = 0;
                    sqlcmd.CommandText = queries[i].ToString();
                    count = sqlcmd.ExecuteNonQuery();
                    if (count == 0)
                    {
                        trans.Rollback();
                        sqlcon.Close();
                        return "NOT OK";
                    }
                }

                //count = 0;




                // sqlcmd.CommandText = query3;
                // count = sqlcmd.ExecuteNonQuery();
                // if (count == 0)
                // {
                //     trans.Rollback();
                //     return "NOT OK";
                // }

                trans.Commit();
                sqlcon.Close();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return ex.Message;
                sqlcon.Close();
            }
            return "OK";
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



        public class ReturnCombined
        {
            public MasterModel Master { get; set; }
            public DetailModel[] Details { get; set; }
            public string ServerResponse { get; set; }
        }

        [WebMethod]
        public static ReturnCombined GetManifestDetails(string ManifestNumber)
        {

            ReturnCombined resp = new ReturnCombined();
            resp.ServerResponse = "";
            MasterModel master = new MasterModel();
            List<DetailModel> details = new List<DetailModel>();


            clvar.manifestNo = ManifestNumber.Trim();
            DataSet chk = checkdemanifest(clvar.manifestNo);
            DataTable dt = GetConsignmentsInManifests(clvar);
            DataTable header = GetManifestHeader(clvar);
            if (/*chk.Tables[0].Rows.Count > 0*/ false)
            {
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Manifest Already Demanifested')", true);
                //btn_save.Enabled = false;

                resp.ServerResponse = "Manifest Already Demanifested.";
                return resp;
            }
            else
            {
                if (header != null)
                {
                    if (header.Rows.Count > 0)
                    {
                        master.Manifest = header.Rows[0]["manifestNumber"].ToString();
                        master.Destination = header.Rows[0]["Destination"].ToString();
                        master.Date = header.Rows[0]["date"].ToString();
                        master.Origin = header.Rows[0]["Origin"].ToString();
                        master.OriginCode = header.Rows[0]["OCODE"].ToString();
                        master.DestinationCode = header.Rows[0]["DCODE"].ToString();
                        master.Type = header.Rows[0]["manifestType"].ToString();
                        master.IsDemanifested = header.Rows[0]["isDemanifested"].ToString();
                        master.WoManifest = "0";
                        resp.Master = master;
                    }
                    else
                    {
                        master.Manifest = ManifestNumber;
                        master.Destination = DestinationName;
                        master.DestinationCode = HttpContext.Current.Session["BranchCode"].ToString();
                        master.Date = DateTime.Now.ToShortDateString();
                        master.Origin = "";
                        master.OriginCode = "";
                        master.Type = "Overnight";
                        master.IsDemanifested = "";
                        master.WoManifest = "1";
                        resp.Master = master;
                        resp.Details = details.ToArray();
                        return resp;
                    }
                }
                else
                {
                    resp.ServerResponse = "Error Fetching Records from DataBase";
                    return resp;
                }
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            DetailModel cn = new DetailModel();
                            cn.ConsignmentNumber = dr["consignmentNumber"].ToString();
                            cn.status = dr["Remarks"].ToString();
                            cn.Reason = dr["REASON"].ToString();
                            cn.Origin = dr["Origin"].ToString();
                            cn.Destination = dr["Dest"].ToString();
                            cn.ConsignmentType = dr["ConType"].ToString();
                            cn.ServiceType = dr["serviceTypeName"].ToString();
                            cn.Weight = dr["Weight"].ToString();
                            cn.OriginCode = dr["Orgin"].ToString();
                            cn.DestinationCode = dr["Destination"].ToString();
                            cn.DemanifestStateID = dr["DeManifestStateID"].ToString();

                            details.Add(cn);
                        }
                        resp.Details = details.ToArray();
                    }
                    else
                    {
                        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Invalid Manifest Number')", true
                        resp.ServerResponse = "No Consignments found for This Manifest Number";
                        return resp;
                    }
                }
                else
                {
                    resp.ServerResponse = "Error Fetching Records from DataBase";
                    return resp;
                }
            }


            return resp;
        }

        public static DataTable GetConsignmentsInManifests(Cl_Variables clvar)
        {
            string sqlString = "select cm.manifestNumber ManifestId, \n"
                + "	   m.manifestNumber ManifestNo, \n"
                + "	   cm.DeManifestStateID, \n"
                + "	   cm.consignmentNumber, \n"
                + "	   cm.Remarks, \n"
                + "	   CASE When c.orgin is null then m.origin else c.orgin end orgin, \n"
                + "	   b1.Sname Origin, \n"
                + "	   b2.Sname Dest, \n"
                + "	   CASE When c.destination is null then m.destination else c.destination end destination , \n"
                + "	   c.consignmentTypeId, \n"
                + "	   CASE WHEN ct.name is null then 'NORMAL' else ct.name end ConType, \n"
                + "	   CASE WHEN c.serviceTypeName is null then m.manifestType else c.serviceTypeName end ServiceTypeName, \n"
                + "	   cm.weight, cm.reason \n"
                + " \n"
                + "FROM MNP_ConsignmentManifest cm \n"
                + " \n"
                + " left outer join Consignment c \n"
                + "    on c.consignmentNumber = cast (cm.consignmentNumber as varchar) \n"
                + " \n"
                + " inner join MNP_Manifest m \n"
                + "	on m.manifestNumber = cm.manifestNumber \n"
                + " \n"
                + " inner join Branches b1 \n"
                + "	on b1.branchCode = CASE WHEN c.orgin is null then m.origin else c.orgin end \n"
                + " \n"
                + " inner join Branches b2 \n"
                + "	on b2.branchCode = CASE WHEN c.destination is null then m.destination else c.destination end \n"
                + " \n"
                + " left outer join ConsignmentType ct \n"
                + "	on ct.id = c.consignmentTypeId"
                + " where m.manifestNumber = '" + clvar.manifestNo + "'\n"
                + "   --and m.destination = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'";

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

        public static DataTable GetManifestHeader(Cl_Variables clvar)
        {

            string sqlString = "\n" +
            "select b3.name          Branch,\n" +
            "       m.date,\n" +
            "       m.manifestNumber,\n" +
            "       b.name           Origin,\n" +
            "       b2.name          Destination\n" +
            "  from MNP_Manifest m\n" +
            " inner join Branches b\n" +
            "    on b.branchCode = m.origin\n" +
            " inner join Branches b2\n" +
            "    on b2.branchCode = m.destination\n" +
            " inner join Branches b3\n" +
            "    on b3.branchCode = m.branchCode\n" +
            " where m.manifestNumber = '" + clvar.consignmentNo + "'";

            sqlString = "\n" +
           "select b3.name          Branch,\n" +
           "       FORMAT(m.date, 'dd/MM/yyyy') date, m.origin OCODE, m.Destination DCODE,\n" +
           "       m.manifestNumber,\n" +
           "       b.sname + '-' + b.name           Origin,\n" +
           "       b2.sname + '-' + b2.name          Destination, m.manifestType, m.date, m.isDemanifested\n" +
           "  from MNP_Manifest m\n" +
           " inner join Branches b\n" +
           "    on b.branchCode = m.origin\n" +
           " inner join Branches b2\n" +
           "    on b2.branchCode = m.destination\n" +
           " inner join Branches b3\n" +
           "    on b3.branchCode = m.branchCode\n" +
           " where m.manifestNumber = '" + clvar.manifestNo + "'";
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
        public static string SaveDemanifest(MasterModel Master, DetailModel[] Consignments)
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[] {
            new DataColumn("consignmentNumber", typeof(string)),
            new DataColumn("manifestNumber", typeof(string)),
            new DataColumn("statusCode", typeof(string)),
            new DataColumn("reason", typeof(string)),
            new DataColumn("DeManifestStateID", typeof(string)),
            new DataColumn("Remarks", typeof(string)),
            new DataColumn("Weight", typeof(float)),
            new DataColumn("Pieces", typeof(int))
        });
            clvar.manifestNo = Master.Manifest;
            clvar.destination = Master.DestinationCode;
            DateTime manifestDate = new DateTime(1990, 01, 01);
            DateTime.TryParse(Master.Date, out manifestDate);
            if (manifestDate.ToShortDateString() == new DateTime(1990, 01, 01).ToShortDateString())
            {
                return "Invalid Date";
            }
            else
            {
                clvar.BookingDate = manifestDate.ToString("yyyy-MM-dd");
            }

            string woManifest = Master.WoManifest;
            if (woManifest == "0")
            {
                clvar.ServiceType = Master.Type;
            }
            else
            {
                clvar.ServiceType = "Overnight";
            }

            DataTable recStatus = CF.GetReceivingStatus();

            foreach (DetailModel cn in Consignments)
            {
                float tempWeight = 0;
                float.TryParse(cn.Weight, out tempWeight);
                if (tempWeight <= 0)
                {
                    tempWeight = 0.5f;
                }
                DataRow dr = dt.NewRow();
                DataRow recRow = recStatus.Select("id = '" + cn.status + "'").FirstOrDefault();
                dr["consignmentNumber"] = cn.ConsignmentNumber;
                dr["manifestNumber"] = Master.Manifest;

                dr["statusCode"] = recRow["id"].ToString();
                dr["DeManifestStateID"] = recRow["id"].ToString();
                dr["Remarks"] = recRow["AttributeDesc"].ToString().ToUpper();


                dr["reason"] = cn.Reason;
                dr["Weight"] = tempWeight;
                dr["Pieces"] = "1";

                dt.Rows.Add(dr);
            }

            string resp = SaveDemanifestToDataBase(dt, clvar, woManifest);

            return resp;
        }

        public static string SaveDemanifestToDataBase(DataTable dt, Cl_Variables clvar, string woManifest)
        {
            string resp = "";


            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = con;
                cmd.CommandText = "MnP_SaveDemanifest";
                cmd.Parameters.AddWithValue("@Details", dt);
                cmd.Parameters.AddWithValue("@ManifestNumber", clvar.manifestNo);
                cmd.Parameters.AddWithValue("@UserID", HttpContext.Current.Session["U_ID"].ToString());
                cmd.Parameters.AddWithValue("@BranchCode", HttpContext.Current.Session["BranchCode"].ToString());
                cmd.Parameters.AddWithValue("@ZoneCode", HttpContext.Current.Session["ZoneCode"].ToString());
                cmd.Parameters.AddWithValue("@ExpressCenterCode", HttpContext.Current.Session["ExpressCenter"].ToString());
                cmd.Parameters.AddWithValue("@Destination", clvar.destination);
                cmd.Parameters.AddWithValue("@ManifestType", clvar.ServiceType);
                cmd.Parameters.AddWithValue("@ManifestDate", clvar.BookingDate);
                cmd.Parameters.AddWithValue("@WoManifest", woManifest);

                cmd.Parameters.Add("@result", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                //cmd.Parameters.Add("@ResultStatus", SqlDbType.Int).Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                resp = cmd.Parameters["@result"].SqlValue.ToString();
            }
            catch (Exception ex)
            { resp = ex.Message; }
            finally { con.Close(); }

            return resp;
        }

        public class RecStatus
        {
            public string Code { get; set; }
            public string Name { get; set; }
        }

        [WebMethod]
        public static RecStatus[] GetRecStatus()
        {

            List<RecStatus> statuses = new List<RecStatus>();


            DataTable dt = CF.GetReceivingStatus();

            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        RecStatus recst = new RecStatus();
                        recst.Code = dr["id"].ToString();
                        recst.Name = dr["AttributeDesc"].ToString();
                        statuses.Add(recst);
                    }
                }
            }





            return statuses.ToArray();
        }
    }
}