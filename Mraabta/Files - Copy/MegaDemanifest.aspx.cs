using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using MRaabta.App_Code;
using System.Web.Services;

namespace MRaabta.Files
{
    public partial class MegaDemanifest : System.Web.UI.Page
    {
        bayer_Function b_fun = new bayer_Function();
        DataSet ds_grid = new DataSet();
        cl_Encryption enc = new cl_Encryption();
        public static CommonFunction CF = new CommonFunction();
        static Cl_Variables clvar2 = new Cl_Variables();
        public static Cl_Variables clvar = new Cl_Variables();
        public static Consignemnts con = new Consignemnts();

        static string user = "";
        public static string DestinationName = "", DestinationCode = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            user = Session["User_Info"].ToString();
            GetCNLengths();
            BindDestinations();

            txt_destination.Text = DestinationName;
            hd_destinationCode.Value = DestinationCode;

            DateTime dateNow = DateTime.Now;
            string LoadingIDLogic = DateTime.Now.Year.ToString().Substring(2, 2) + dateNow.Month.ToString("d2") + dateNow.Day.ToString("d2") + dateNow.Hour.ToString("D2") + dateNow.Minute.ToString("D2") + dateNow.Second.ToString("D2") + dateNow.Millisecond.ToString("D3");
            txt_manifestNumber.Text = LoadingIDLogic;
            hd_manifestNumber2.Value = LoadingIDLogic;
        }

        protected void Btn_Search_Click(object sender, EventArgs e)
        {

            Response.Redirect("MegaDemanifest_Print.aspx?Xcode=" + clvar.manifestNo);
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
                    DestinationCode = ds.Tables[0].Select("BranchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'").FirstOrDefault()["BranchCode"].ToString();
                }
            }
        }

        public static DataSet Branch()
        {
            DataSet Ds_1 = new DataSet();

            try
            {
                string query = "SELECT b.branchCode, b.sname + '-' + b.name BranchName FROM Branches b GROUP BY b.branchCode, b.name, b.sname order by b.sname ASC";
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
            string query = "SELECT * FROM MNP_ConsignmentLengths where status = '1' AND Product = 'Mega' ";
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

        [WebMethod]
        public static string SaveDemanifest(MasterModel Master, DetailModel[] Consignments)
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[] {
            new DataColumn("consignmentNumber", typeof(string)),
            new DataColumn("manifestNumber", typeof(string)),
            //new DataColumn("statusCode", typeof(string)),
            //new DataColumn("reason", typeof(string)),
            new DataColumn("DeManifestStateID", typeof(string)),
            new DataColumn("Remarks", typeof(string)),
            new DataColumn("Weight", typeof(float)),
            new DataColumn("Pieces", typeof(int))
        });
            clvar.manifestNo = Master.Manifest;
            clvar.destination = Master.DestinationCode;
            // DateTime manifestDate = DateTime.Now;

            clvar.BookingDate = DateTime.Today.ToString("yyyy-MM-dd");
            clvar.ServiceType = "Overnight";

            foreach (DetailModel cn in Consignments)
            {
                float tempWeight = 0;
                float.TryParse(cn.Weight, out tempWeight);
                if (tempWeight <= 0)
                {
                    tempWeight = 0.5f;
                }
                DataRow dr = dt.NewRow();
                dr["consignmentNumber"] = cn.ConsignmentNumber;
                dr["manifestNumber"] = Master.Manifest;
                dr["DeManifestStateID"] = "67";
                dr["Weight"] = tempWeight;
                dr["Pieces"] = "1";

                dt.Rows.Add(dr);
            }

            string resp = SaveDemanifestToDataBase(dt, clvar);

            return resp;
        }

        public static string SaveDemanifestToDataBase(DataTable dt, Cl_Variables clvar)
        {
            string resp = "";


            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = con;
                cmd.CommandText = "MnP_Mega_Demanifest";
                cmd.Parameters.AddWithValue("@Details", dt);
                cmd.Parameters.AddWithValue("@ManifestNumber", clvar.manifestNo);
                cmd.Parameters.AddWithValue("@UserID", HttpContext.Current.Session["U_ID"].ToString());
                cmd.Parameters.AddWithValue("@BranchCode", HttpContext.Current.Session["BranchCode"].ToString());
                cmd.Parameters.AddWithValue("@ZoneCode", HttpContext.Current.Session["ZoneCode"].ToString());
                cmd.Parameters.AddWithValue("@ExpressCenterCode", HttpContext.Current.Session["ExpressCenter"].ToString());
                cmd.Parameters.AddWithValue("@Destination", clvar.destination);
                cmd.Parameters.AddWithValue("@ManifestType", clvar.ServiceType);
                cmd.Parameters.AddWithValue("@ManifestDate", clvar.BookingDate);
                //  cmd.Parameters.AddWithValue("@WoManifest", woManifest);

                cmd.Parameters.Add("@result", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                //cmd.Parameters.Add("@ResultStatus", SqlDbType.Int).Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                resp = cmd.Parameters["@result"].SqlValue.ToString();
            }
            catch (Exception ex)
            { 
                resp = ex.Message;
                throw;
            }
            finally { con.Close(); }

            return resp;
        }

    }
}