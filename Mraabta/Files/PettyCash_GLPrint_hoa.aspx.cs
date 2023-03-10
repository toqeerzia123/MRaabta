using MRaabta.App_Code;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI.WebControls;

namespace MRaabta.Files
{
    public partial class PettyCash_GLPrint_hoa : System.Web.UI.Page
    {
        Variable clvar = new Variable();
        bayer_Function bfunc = new bayer_Function();
        CommonFunction cfunc = new CommonFunction();
        clsVariables clsvar = new clsVariables();
        Cl_Variables cl_var = new Cl_Variables();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["from"] != null)
                {
                    hf_ft.Value = Decrypt(HttpUtility.UrlDecode(Request.QueryString["ft"]));
                    hf_hoa.Value = Decrypt(HttpUtility.UrlDecode(Request.QueryString["hoa"]));
                    hf_branch.Value = Decrypt(HttpUtility.UrlDecode(Request.QueryString["branch"]));
                    hf_from.Value = Decrypt(HttpUtility.UrlDecode(Request.QueryString["from"]));
                    hf_to.Value = Decrypt(HttpUtility.UrlDecode(Request.QueryString["to"]));
                    hf_ec.Value = Decrypt(HttpUtility.UrlDecode(Request.QueryString["ec"]));
                    hf_status.Value = Decrypt(HttpUtility.UrlDecode(Request.QueryString["status"]));
                    hf_company.Value = Decrypt(HttpUtility.UrlDecode(Request.QueryString["company"]));
                    lbl_from.Text = hf_from.Value.ToString();
                    lbl_to.Text = hf_to.Value.ToString();
                    Data();

                }
            }
        }
        public void Data()
        {
            DataSet ds = new DataSet();
            ds = GetData();
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow[] dr_comp = ds.Tables[0].Select("comp1 <> ''");
                if (dr_comp.Count() > 0)
                {
                    lbl_company.Text = dr_comp[0]["comp1"].ToString();
                }

                lbl_branch.Text = Decrypt(HttpUtility.UrlDecode(Request.QueryString["bname"]));
                // lbl_created_by.Text = ds.Tables[0].Rows[0]["create_by"].ToString();
                lbl_currentdate.Text = DateTime.Now.ToString();

                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                //debit credit balance
                dt.Columns.Add("balance", typeof(string));
                int debit = 0;
                int credit = 0;
                int balance = 0;

                double credit_total = 0;
                double debit_total = 0;

                foreach (DataRow dr in dt.Rows)
                {
                    if (!string.IsNullOrEmpty(dr["dnote"].ToString()))
                    {
                        debit += int.Parse(dr["dnote"].ToString());
                    }
                    if (!string.IsNullOrEmpty(dr["cnote"].ToString()))
                    {
                        credit += int.Parse(dr["cnote"].ToString());
                    }
                    balance = debit - credit;
                    dr["balance"] = int.Parse(balance.ToString()).ToString("N0");

                }
                foreach (DataRow dr in dt.Rows)
                {
                    credit_total += double.Parse(dr["cnote"].ToString());
                    debit_total += double.Parse(dr["dnote"].ToString());

                    dr["dnote"] = int.Parse(dr["dnote"].ToString());
                    dr["cnote"] = int.Parse(dr["cnote"].ToString());
                }
                //  lbl_tot.Text = amount.ToString("N0");
                lbl_user.Text = HttpContext.Current.Session["U_NAME"].ToString();

                DataRow total_row = dt.NewRow();
                total_row["dnote"] = debit_total;
                total_row["cnote"] = credit_total;
                dt.Rows.Add(total_row);
                dt.AcceptChanges();

                GridView.DataSource = dt;
                GridView.DataBind();
                if (hf_ec.Value.ToString() != "")
                {
                    GridView.Columns[GridView.Columns.Count - 1].Visible = false;

                }
                if (hf_ec.Value.ToString() == "" || hf_ec.Value.ToString() == "'0'")
                {
                    GridView.Columns[3].Visible = false;
                    GridView.Rows[GridView.Rows.Count - 1].Cells[0].ColumnSpan = 4;
                }
                if (hf_branch.Value.Contains("','") == true)
                {
                    GridView.Columns[3].Visible = true;
                    GridView.Rows[GridView.Rows.Count - 1].Cells[0].ColumnSpan = 5;
                }
                GridView.Rows[GridView.Rows.Count - 1].Cells[0].Text = "Total";
                GridView.Rows[GridView.Rows.Count - 1].Cells[1].Visible = false;
                GridView.Rows[GridView.Rows.Count - 1].Cells[2].Visible = false;
                GridView.Rows[GridView.Rows.Count - 1].Cells[3].Visible = false;
                GridView.Rows[GridView.Rows.Count - 1].Cells[4].Visible = false;

                GridView.Rows[GridView.Rows.Count - 1].Font.Bold = true;

            }
        }
        public DataSet GetData()
        {
            DataSet Ds_1 = new DataSet();
            try
            {
                string ft = "";
                string hoa = "";
                string branch = "";
                string ec = "";

                if (hf_ft.Value.ToString() != "0")
                {
                    ft = " and pcd.expense='" + hf_ft.Value.ToString() + "'";
                }
                if (hf_hoa.Value.ToString() != "0")
                {
                    hoa = " and pcd.[desc]='" + hf_hoa.Value.ToString() + "' ";
                }

                if (hf_branch.Value.ToString() != "")
                {
                    branch = " and pch.BRANCH in  (" + hf_branch.Value.ToString() + ")\n";
                }
                if (hf_ec.Value.ToString() != "")
                {
                    ec = " and pch.express_center in (" + hf_ec.Value.ToString() + ") ";
                }
                string status = "";
                if (hf_status.Value.ToString() != "")
                {
                    status = " and pcd.status='" + hf_status.Value.ToString() + "' ";
                }



                string sqlString = "SELECT a.rank,\n" +
                "       a.COMPANY,\n" +
                "       a.YEAR,\n" +
                "       a.MONTH,\n" +
                "       a.Date,\n" +
                "       a.Debit dnote,\n" +
                "       a.Credit cnote,\n" +
                "       b.name branch,\n" +
                "       case e.name\n" +
                "         when '0' then\n" +
                "          'ALL'\n" +
                "         ELSE\n" +
                "          ISNULL(e.name, 'ALL')\n" +
                "       END ec,\n" +
                "       a.expense,\n" +
                "       a.[description],\n" +
                "       a.narrate,\n" +
                "       a.ID,\n" +
                "       a.comp1\n" +
                "  FROM (SELECT '1' rank,\n" +
                "               pch.COMPANY,\n" +
                "               pch.BRANCH,\n" +
                "               pch.[YEAR],\n" +
                "               pch.[MONTH],\n" +
                "               CONVERT(VARCHAR, pcd.DATE, 103) \"Date\",\n" +
                "               pch.express_center,\n" +
                "               0 AS Credit,\n" +
                "               pcd.AMOUNT AS Debit,\n" +
                "               'Petty Cash' expense,\n" +
                "               'AMOUNT TRANSFERRED FROM CIH TO PETTY CASH' + '(' + pmode.[desc]+ ')' description,\n" +
                "               '' narrate,\n" +
                "               pch.ID,\n" +
                "               cm.sdesc_OF comp1\n" +
                "          FROM PC_CIH_head AS pch\n" +
                "         INNER JOIN PC_CIH_detail AS pcd\n" +
                "            ON pcd.head_id = pch.ID\n" +
                "          left outer join PC_cash_mode pmode\n" +
                "            on pmode.ID = pcd.cash_type\n" +
                "         inner join COMPANY_OF cm\n" +
                "            on cm.code_OF = pch.COMPANY\n" +
                "           and pch.COMPANY = '" + hf_company.Value.ToString() + "'\n" +
               " where pcd.[DATE] BETWEEN CONVERT(DATETIME, '" + hf_from.Value.ToString() + "', 103) AND CONVERT(DATETIME, '" + hf_to.Value.ToString() + "', 103) ";
                sqlString +=
                " \n" +
                " " + branch + " " + ec + "  ";
                sqlString +=
              //"   and pch.CREATED_BY = '" + HttpContext.Current.Session["U_ID"].ToString() + "'\n" +
              "\n" +
                "        UNION ALL\n" +
                "\n" +
                "        SELECT '2' rank,\n" +
                "               pch.COMPANY,\n" +
                "               pch.BRANCH,\n" +
                "               pch.[YEAR],\n" +
                "               pch.[MONTH],\n" +
                "               CONVERT(VARCHAR, pcd.DATE, 103) \"Date\",\n" +
                "               pch.express_center,\n" +
                "               pcd.AMOUNT AS Credit,\n" +
                "               0 AS Debit,\n" +
                "               m.description expense,\n" +
                "               s.sub_desc description,\n" +
                "               pcd.NARRATE,\n" +
                "               pch.ID,\n" +
                "               cm.sdesc_OF comp1\n" +
                "          FROM PC_head AS pch\n" +
                "         INNER JOIN PC_detail AS pcd\n" +
                "            ON pcd.head_id = pch.ID --and pch.CREATED_BY=pcd.CREATED_BY\n" +
                "          left outer join pc_mainHead m\n" +
                "            on pcd.expense = m.code\n" +
                "          left outer join pc_subHead s\n" +
                "            on pcd.[desc]= s.subcode\n" +
                "           and pcd.EXPENSE = s.headcode\n" +
                "         inner join COMPANY_OF cm\n" +
                "            on cm.code_OF = pch.COMPANY\n" +
                "           and pch.COMPANY = '" + hf_company.Value.ToString() + "'\n" +
               " where pcd.[DATE] BETWEEN CONVERT(DATETIME, '" + hf_from.Value.ToString() + "', 103) AND CONVERT(DATETIME, '" + hf_to.Value.ToString() + "', 103) ";
                sqlString +=
                " \n" +
                " " + branch + " " + ec + " " + status + " " + ft + " " + hoa + " ";
                sqlString +=
                //"   and pch.CREATED_BY = '" + HttpContext.Current.Session["U_ID"].ToString() + "'\n" +
                ") a\n" +
              " inner join Branches b\n" +
                "    on a.BRANCH = b.branchCode\n" +
                "  left outer join ExpressCenters e\n" +
                "    on a.express_center = e.expressCenterCode\n" +
                " ORDER BY 5, 1";


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

        #region converting amount to words
        private static String ones(String Number)
        {
            int _Number = Convert.ToInt32(Number);
            String name = "";
            switch (_Number)
            {
                case 1:
                    name = "One";
                    break;
                case 2:
                    name = "Two";
                    break;
                case 3:
                    name = "Three";
                    break;
                case 4:
                    name = "Four";
                    break;
                case 5:
                    name = "Five";
                    break;
                case 6:
                    name = "Six";
                    break;
                case 7:
                    name = "Seven";
                    break;
                case 8:
                    name = "Eight";
                    break;
                case 9:
                    name = "Nine";
                    break;
            }
            return name;
        }
        private static String tens(String Number)
        {
            int _Number = Convert.ToInt32(Number);
            String name = null;
            switch (_Number)
            {
                case 10:
                    name = "Ten";
                    break;
                case 11:
                    name = "Eleven";
                    break;
                case 12:
                    name = "Twelve";
                    break;
                case 13:
                    name = "Thirteen";
                    break;
                case 14:
                    name = "Fourteen";
                    break;
                case 15:
                    name = "Fifteen";
                    break;
                case 16:
                    name = "Sixteen";
                    break;
                case 17:
                    name = "Seventeen";
                    break;
                case 18:
                    name = "Eighteen";
                    break;
                case 19:
                    name = "Nineteen";
                    break;
                case 20:
                    name = "Twenty";
                    break;
                case 30:
                    name = "Thirty";
                    break;
                case 40:
                    name = "Fourty";
                    break;
                case 50:
                    name = "Fifty";
                    break;
                case 60:
                    name = "Sixty";
                    break;
                case 70:
                    name = "Seventy";
                    break;
                case 80:
                    name = "Eighty";
                    break;
                case 90:
                    name = "Ninety";
                    break;
                default:
                    if (_Number > 0)
                    {
                        name = tens(Number.Substring(0, 1) + "0") + " " + ones(Number.Substring(1));
                    }
                    break;
            }
            return name;
        }
        private static String ConvertWholeNumber(String Number)
        {
            string word = "";
            try
            {
                bool beginsZero = false;//tests for 0XX  
                bool isDone = false;//test if already translated  
                double dblAmt = (Convert.ToDouble(Number));
                //if ((dblAmt > 0) && number.StartsWith("0"))  
                if (dblAmt > 0)
                {//test for zero or digit zero in a nuemric  
                    beginsZero = Number.StartsWith("0");

                    int numDigits = Number.Length;
                    int pos = 0;//store digit grouping  
                    String place = "";//digit grouping name:hundres,thousand,etc...  
                    switch (numDigits)
                    {
                        case 1://ones' range  

                            word = ones(Number);
                            isDone = true;
                            break;
                        case 2://tens' range  
                            word = tens(Number);
                            isDone = true;
                            break;
                        case 3://hundreds' range  
                            pos = (numDigits % 3) + 1;
                            place = " Hundred ";
                            break;
                        case 4://thousands' range  
                        case 5:
                        case 6:
                            pos = (numDigits % 4) + 1;
                            place = " Thousand ";
                            break;
                        case 7://millions' range  
                        case 8:
                        case 9:
                            pos = (numDigits % 7) + 1;
                            place = " Million ";
                            break;
                        case 10://Billions's range  
                        case 11:
                        case 12:

                            pos = (numDigits % 10) + 1;
                            place = " Billion ";
                            break;
                        //add extra case options for anything above Billion...  
                        default:
                            isDone = true;
                            break;
                    }
                    if (!isDone)
                    {//if transalation is not done, continue...(Recursion comes in now!!)  
                        if (Number.Substring(0, pos) != "0" && Number.Substring(pos) != "0")
                        {
                            try
                            {
                                word = ConvertWholeNumber(Number.Substring(0, pos)) + place + ConvertWholeNumber(Number.Substring(pos));
                            }
                            catch { }
                        }
                        else
                        {
                            word = ConvertWholeNumber(Number.Substring(0, pos)) + ConvertWholeNumber(Number.Substring(pos));
                        }

                        //check for trailing zeros  
                        //if (beginsZero) word = " and " + word.Trim();  
                    }
                    //ignore digit grouping names  
                    if (word.Trim().Equals(place.Trim())) word = "";
                }
            }
            catch { }
            return word.Trim();
        }
        private static String ConvertToWords(String numb)
        {
            String val = "", wholeNo = numb, points = "", andStr = "", pointStr = "";
            String endStr = "Only";
            try
            {
                int decimalPlace = numb.IndexOf(".");
                if (decimalPlace > 0)
                {
                    wholeNo = numb.Substring(0, decimalPlace);
                    points = numb.Substring(decimalPlace + 1);
                    if (Convert.ToInt32(points) > 0)
                    {
                        andStr = "and";// just to separate whole numbers from points/cents  
                        endStr = "Paisa " + endStr;//Cents  
                        pointStr = ConvertDecimals(points);
                    }
                }
                val = String.Format("{0} {1}{2} {3}", ConvertWholeNumber(wholeNo).Trim(), andStr, pointStr, endStr);
            }
            catch { }
            return val;
        }
        private static String ConvertDecimals(String number)
        {
            String cd = "", digit = "", engOne = "";
            for (int i = 0; i < number.Length; i++)
            {
                digit = number[i].ToString();
                if (digit.Equals("0"))
                {
                    engOne = "Zero";
                }
                else
                {
                    engOne = ones(digit);
                }
                cd += " " + engOne;
            }
            return cd;
        }
        #endregion
        #region Encryption & decryption
        private string Encrypt(string clearText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = System.Text.Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }
        private string Decrypt(string cipherText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = System.Text.Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
        #endregion
    }
}