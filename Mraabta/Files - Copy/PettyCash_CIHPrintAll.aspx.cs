using MRaabta.App_Code;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace MRaabta.Files
{
    public partial class PettyCash_CIHPrintAll : System.Web.UI.Page
    {
        Variable clvar = new Variable();
        bayer_Function bfunc = new bayer_Function();
        CommonFunction cfunc = new CommonFunction();
        clsVariables clsvar = new clsVariables();
        Cl_Variables cl_var = new Cl_Variables();

        int page = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["ID"] != null)
                {
                    if (Request.QueryString["ID"].ToString() != "")
                    {
                        Data();

                    }
                }
            }
        }
        public void Data()
        {
            int month = 0;
            string month1 = "";
            int index = 1;
            DataSet ds = new DataSet();
            ds = GetData();
            if (ds.Tables[0].Rows.Count > 0)
            {
                #region total
                double amount = 0;

                #endregion


                //Getting Distinct ID
                DataView viewID = new DataView(ds.Tables[0]);
                viewID.Sort = "ID asc";
                DataTable distinctID = viewID.ToTable(true, "ID");

                foreach (DataRow dr_ID in distinctID.Rows)
                {
                    DataRow[] dr_data = ds.Tables[0].Select("ID = " + dr_ID["ID"].ToString() + "");

                    string company = "";
                    if (dr_data[0]["company_desc"].ToString() != "")
                    {
                        company = dr_data[0]["company_desc"].ToString();
                    }
                    else
                    {
                        company = "Muller & Phipps Express And Logistics";
                    }
                    Header(company);
                    month = int.Parse(ds.Tables[0].Rows[0]["month"].ToString());
                    month1 = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);
                    #region labels
                    lt_Main.Text += "  <table border=\"0\" width=\"100%\"> " +
                " <tr> " +
                    " <td width=\"25%\" class=\"style2\"> " +
                       "  <font color=\"#000000\" face=\"verdana\" size=\"1\" class=\"style2\"><b>Transaction Number : </b> " +
                          "   " + dr_data[0]["ID"].ToString() + "</font> " +
                   "  </td> " +
                   "  <td align=\"center\" colspan=\"2\" class=\"style5\"> " +
                      "   &nbsp; " +
                   "  </td> " +
                   "  <td align=\"right\" width=\"25%\"> " +
                       "  <font color=\"#000000\" face=\"verdana\" size=\"1\" class=\"style2\"><b>Company : </b> " +
                           "  " + company + "</asp:Label></font> " +
                   "  </td> " +
               "  </tr> " +
               "  <tr> " +
                 "    <td width=\"25%\" class=\"style2\"> " +
                   "      <font color=\"#000000\" face=\"verdana\" size=\"1\" class=\"style2\"><b>Branch : </b> " +
                     "        " + dr_data[0]["branch"].ToString() + "</font> " +
                   "    </td> " +
                    "   <td align=\"center\" colspan=\"2\" class=\"style5\"> " +
                     "      &nbsp; " +
                   "    </td> " +
                   "    <td align=\"right\" width=\"25%\"> " +
                      "     <font color=\"#000000\" face=\"verdana\" size=\"1\" class=\"style2\"><b>Status : </b> " +
                  "            " + dr_data[0]["status"].ToString() + " </font> " +
                  "     </td> " +
                "   </tr> " +
                "   <tr> " +
                 "      <td width=\"25%\" class=\"style2\"> " +
                     "      <font color=\"#000000\" face=\"verdana\" size=\"1\" class=\"style2\"><b>Year : </b> " +
                      "        " + dr_data[0]["year"].ToString() + "</font> " +
                  "     </td> " +
                   "    <td align=\"center\" colspan=\"2\" class=\"style5\"> " +
                    "       &nbsp; " +
                  "     </td> " +
                   "    <td align=\"right\" width=\"25%\"> " +
                      "     <font color=\"#000000\" face=\"verdana\" size=\"1\" class=\"style2\"><b>Month : </b> " +
                      "        " + month1 + "</font> " +
                  "     </td> " +
               "    </tr> " +
                "   <tr> " +
                   "    <td width=\"25%\" class=\"style2\"> " +
                   "        <font color=\"#000000\" face=\"verdana\" size=\"1\" class=\"style2\"><b>Entered By : </b> " +
                      "     " + dr_data[0]["create_by"].ToString() + "</font> " +
                   "    </td> " +
                   "    <td align=\"center\" colspan=\"2\" class=\"style5\"> " +
                     "      &nbsp; " +
                   "    </td> " +
                    "   <td align=\"right\" width=\"25%\"> " +
                      "     <font color=\"#000000\" face=\"verdana\" size=\"1\" class=\"style2\"><b>Entered On : </b> " +
                       "      " + dr_data[0]["created_on"].ToString() + "</asp:Label></font> " +
                    "   </td> " +
                 "  </tr> " +
            "   </table> " +
           "    <br /> " +
           " <br /> " +
           " <hr style=\"color: Black; background-color: Black;\" /> ";
                    #endregion
                    #region Table Header
                    lt_Main.Text += "<table width=\"100%\">\n" +
           "                    <tr style=\"background-color:#a8a8a8;color:black; border-color: black; border-width: 1px; border-style: Solid;\">\n" +
           //"                        <th align=\"center\">S.No</th>\n" +
           "                        <th align=\"center\">Date</th>\n" +
             "                        <th align=\"center\">Cash Type</th>\n" +
           "                        <th align=\"center\">Cheque No.</th>\n" +
            "                        <th align=\"center\">Amount</th>\n" +
           //"                        <th align=\"center\">Remaining Amount</th>\n" +
           "                    </tr>\n";

                    #endregion
                    foreach (DataRow dr in dr_data)
                    {
                        amount += int.Parse(dr["amount"].ToString());
                        //   dr["amount"] = dr["amount"].ToString();
                        //     string amount_total = ;
                        lt_Main.Text += "\n" +
                   "                    <tr style=\"background-color:white;color:black; border-color: black; border-width: 1px; border-style: Solid;font-face:verdana;font-size:1\">\n" +
                   //"                        <td align=\"center\"> <font color=\"#000000\" face=\"verdana\" size=\"1\" class=\"style2\">" + index.ToString() + "</font></td>\n" +
                   "                        <td align=\"center\"> <font color=\"#000000\" face=\"verdana\" size=\"1\" class=\"style2\">" + dr["Date"].ToString() + "</font></td>\n" +
                     "                        <td align=\"left\"> <font color=\"#000000\" face=\"verdana\" size=\"1\" class=\"style2\">" + dr["desc"].ToString() + "</font></td>\n" +
                   "                        <td align=\"left\"> <font color=\"#000000\" face=\"verdana\" size=\"1\" class=\"style2\">" + dr["chque_no"].ToString() + "</font></td>\n" +
                   "                        <td align=\"right\"> <font color=\"#000000\" face=\"verdana\" size=\"1\" class=\"style2\">" + double.Parse(dr["amount"].ToString()).ToString("N0") + "</font></td>\n" +
                   //" <td align=\"right\"> <font color=\"#000000\" face=\"verdana\" size=\"1\" class=\"style2\">" + double.Parse(dr["amt_rmain"].ToString()).ToString("N0") + "</font></td>\n" +
                   "                    </tr>\n";
                        index++;
                    }
                    lt_Main.Text += "  <table border=\"0\" width=\"100%\">\n" +
                    "  <tr>\n" +
                       "   <td width=\"25%\" class=\"style2\">\n" +
                           "   <font color=\"#000000\" face=\"verdana\" size=\"1\" class=\"style2\"></font>\n" +
                      "    </td>\n" +
                        "  <td align=\"center\" colspan=\"2\" class=\"style3\">\n" +
                         "     &nbsp;\n" +
                       "   </td>\n" +
                       "   <td align=\"right\" width=\"25%\">\n" +
                         "     <font color=\"#000000\" face=\"verdana\" size=\"1\" class=\"style2\"><b>\n" +

                         "     </b>____________<br />\n" +
                          "        <asp:Label Style=\"font-size: x-small\">" + amount.ToString("N0") + "</asp:Label></font><br />\n" +
                         "     <font color=\"#000000\" face=\"verdana\" size=\"1\" class=\"style2\"><b>\n" +

                          "    </b>____________<br />\n" +
                       "   </td>\n" +
                   "   </tr>\n" +
                  "    <tr>\n" +
                      "    <td width=\"25%\" class=\"style2\" colspan=\"2\">\n" +

                      "    </td>\n" +
                      "    <td >\n" +
                      "        <b></b>\n" +
                    "      </td>\n" +
                   "   </tr>\n" +
                "  </table>\n" +
               "   <table border=\"0\" width=\"100%\">\n" +
                   "   <tr>\n" +
                    "      <td>\n" +
                       "       <br />\n" +
                       "       <br />\n" +
                       "       <br />\n" +
                       "       <br />\n" +
                      "    </td>\n" +
                  "    </tr>\n" +
                   "   <tr>\n" +
                       "   <td width=\"30%\" class=\"style2\">\n" +
                           "   <font color=\"#000000\" face=\"verdana\" size=\"1\" class=\"style2\"><b style=\"text-align: left\">\n" +
                             "     Prepared By:________________</b></font>\n" +
                     "     </td>\n" +
                        "   <td width=\"40%\" class=\"style2\">\n" +
                           "   <font color=\"#000000\" face=\"verdana\" size=\"1\" class=\"style2\"><b style=\"text-align: left\">\n" +
                             "    Checked By:________________</b></font>\n" +
                     "     </td>\n" +
                       "   <td align=\"right\" width=\"30%\">\n" +
                          "    <font color=\"#000000\" face=\"verdana\" size=\"1\" class=\"style2\"><b>Approved By:________________\n" +
                       "       </b></font>\n" +
                      "    </td>\n" +
                  "    </tr>\n" +
               "   </table><br/><br/>\n";

                    lt_Main.Text += "   <div class=\"page\" style=\"page-break-before: always;\"> " +
               " </div>";
                    amount = 0;
                }


                //   GridView.DataSource = ds.Tables[0];
                //  GridView.DataBind();
            }
        }
        public DataSet GetData()
        {
            DataSet Ds_1 = new DataSet();
            try
            {
                string company = "";
                if (Request.QueryString["company"].ToString() != "")
                {
                    company = Request.QueryString["company"].ToString();
                }
                string sqlString = "select h.ID ID,\n" +
                "       CONVERT(varchar, d.DATE, 103) date,\n" +
                "       d.AMOUNT,\n" +
                "       CONVERT(varchar, h.Created_ON, 103) Created_ON,\n" +
                "       h.MONTH,\n" +
                "       h.YEAR,\n" +
                "       b.name branch,h.COMPANY,\n" +
                "       z.Name create_by,\n" +
                "     cm.[desc],\n" +
                "     d.chque_no,d.amt_rmain,\n" +
                "  case d.[status] when '1' then 'Approved' when '2' then 'Rejected' else 'Unapproved' end status,cp.sdesc_OF company_desc  \n" +
                "  from PC_CIH_detail d\n" +
                " inner join PC_CIH_head h\n" +
                "    on h.ID = d.HEAD_ID\n" +
                " inner join Branches b\n" +
                "    on b.branchCode = h.BRANCH\n" +
                " inner join ZNI_USER1 z\n" +
                "    on h.CREATED_BY = z.U_ID\n" +
                "  inner join PC_cash_mode cm\n" +
                "  on cm.ID=d.cash_type inner join COMPANY_OF cp  on cp.code_OF=h.COMPANY\n" +
               " where \n" +
                "    head_id in (" + Request.QueryString["ID"].ToString() + ") and h.company='" + company + "'\n" +
                " order by D.ID";



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

        protected void Header(string company)
        {
            string pageBreak = "";
            pageBreak = "page-break-before:always";
            lt_Main.Text += " <table border=\"0\" width=\"100%\" " + pageBreak + "> " +
                " <tr> " +
                  "   <td width=\"25%\">" +
                    "     &nbsp;" +
                   "  </td>" +
                  "   <td align=\"center\" colspan=\"2\" width=\"50%\">" +
                     "    <font color=\"#000000\" face=\"verdana\" size=\"1\"><b style=\"font-size: small\">" +
                          "   " + company + "</b></font>" +
                  "   </td>" +
                   "  <td align=\"right\" width=\"25%\">" +
                      "   <font color=\"#000000\" face=\"verdana\" size=\"1\"></font>" +
                  "   </td>" +
              "   </tr>" +
              "   <tr>" +
                    " <td width=\"25%\">" +
                     "    &nbsp;" +
                  "   </td>" +
                   "  <td align=\"center\" colspan=\"2\" width=\"50%\">" +
                      "   <font color=\"#000000\" face=\"verdana\"><span class=\"style1\"> Receipt Voucher Print</span></font>" +
                  "   </td>" +
                 "    <td align=\"right\" width=\"25%\">" +
                   "      &nbsp;" +
                  "   </td>" +
              "   </tr>" +
          "   </table>";

            lt_Main.Text += " <table border=\"0\" width=\"100%\"> " +
               "  <tr> " +
                  "   <td width=\"25%\"> " +
                    "     &nbsp; " +
                   "  </td> " +
                   "  <td align=\"center\" colspan=\"2\" class=\"style4\"> " +
                    "     &nbsp; " +
                   "  </td> " +
                   "  <td align=\"right\" width=\"25%\"> " +
                      "   <font color=\"#000000\" face=\"verdana\" size=\"1\">Print Date &amp; Time: " +
                        "     " + DateTime.Now.ToString() + "</font> " +
                   "  </td> " +
               "  </tr> " +
               "  <tr> " +
                  "   <td width=\"25%\"> " +
                   "      &nbsp; " +
                  "   </td> " +
                  "   <td align=\"center\" colspan=\"2\" class=\"style4\"> " +
                    "     &nbsp; " +
                 "    </td> " +
                  "   <td align=\"right\" width=\"25%\"> " +
                     "    &nbsp; " +
                   "  </td> " +
              "   </tr> " +
          "   </table> ";

        }


        //#region converting amount to words
        //private static String ones(String Number)
        //{
        //    int _Number = Convert.ToInt32(Number);
        //    String name = "";
        //    switch (_Number)
        //    {
        //        case 1:
        //            name = "One";
        //            break;
        //        case 2:
        //            name = "Two";
        //            break;
        //        case 3:
        //            name = "Three";
        //            break;
        //        case 4:
        //            name = "Four";
        //            break;
        //        case 5:
        //            name = "Five";
        //            break;
        //        case 6:
        //            name = "Six";
        //            break;
        //        case 7:
        //            name = "Seven";
        //            break;
        //        case 8:
        //            name = "Eight";
        //            break;
        //        case 9:
        //            name = "Nine";
        //            break;
        //    }
        //    return name;
        //}
        //private static String tens(String Number)
        //{
        //    int _Number = Convert.ToInt32(Number);
        //    String name = null;
        //    switch (_Number)
        //    {
        //        case 10:
        //            name = "Ten";
        //            break;
        //        case 11:
        //            name = "Eleven";
        //            break;
        //        case 12:
        //            name = "Twelve";
        //            break;
        //        case 13:
        //            name = "Thirteen";
        //            break;
        //        case 14:
        //            name = "Fourteen";
        //            break;
        //        case 15:
        //            name = "Fifteen";
        //            break;
        //        case 16:
        //            name = "Sixteen";
        //            break;
        //        case 17:
        //            name = "Seventeen";
        //            break;
        //        case 18:
        //            name = "Eighteen";
        //            break;
        //        case 19:
        //            name = "Nineteen";
        //            break;
        //        case 20:
        //            name = "Twenty";
        //            break;
        //        case 30:
        //            name = "Thirty";
        //            break;
        //        case 40:
        //            name = "Fourty";
        //            break;
        //        case 50:
        //            name = "Fifty";
        //            break;
        //        case 60:
        //            name = "Sixty";
        //            break;
        //        case 70:
        //            name = "Seventy";
        //            break;
        //        case 80:
        //            name = "Eighty";
        //            break;
        //        case 90:
        //            name = "Ninety";
        //            break;
        //        default:
        //            if (_Number > 0)
        //            {
        //                name = tens(Number.Substring(0, 1) + "0") + " " + ones(Number.Substring(1));
        //            }
        //            break;
        //    }
        //    return name;
        //}   
        //private static String ConvertWholeNumber(String Number)  
        //  {  
        //      string word = "";  
        //      try  
        //      {  
        //          bool beginsZero = false;//tests for 0XX  
        //          bool isDone = false;//test if already translated  
        //          double dblAmt = (Convert.ToDouble(Number));  
        //          //if ((dblAmt > 0) && number.StartsWith("0"))  
        //          if (dblAmt > 0)  
        //          {//test for zero or digit zero in a nuemric  
        //              beginsZero = Number.StartsWith("0");  

        //              int numDigits = Number.Length;  
        //              int pos = 0;//store digit grouping  
        //              String place = "";//digit grouping name:hundres,thousand,etc...  
        //              switch (numDigits)  
        //              {  
        //                  case 1://ones' range  

        //                      word = ones(Number);  
        //                      isDone = true;  
        //                      break;  
        //                  case 2://tens' range  
        //                      word = tens(Number);  
        //                      isDone = true;  
        //                      break;  
        //                  case 3://hundreds' range  
        //                      pos = (numDigits % 3) + 1;  
        //                      place = " Hundred ";  
        //                      break;  
        //                  case 4://thousands' range  
        //                  case 5:  
        //                  case 6:  
        //                      pos = (numDigits % 4) + 1;  
        //                      place = " Thousand ";  
        //                      break;  
        //                  case 7://millions' range  
        //                  case 8:  
        //                  case 9:  
        //                      pos = (numDigits % 7) + 1;  
        //                      place = " Million ";  
        //                      break;  
        //                  case 10://Billions's range  
        //                  case 11:  
        //                  case 12:  

        //                      pos = (numDigits % 10) + 1;  
        //                      place = " Billion ";  
        //                      break;  
        //                  //add extra case options for anything above Billion...  
        //                  default:  
        //                      isDone = true;  
        //                      break;  
        //              }                     
        //              if (!isDone)  
        //              {//if transalation is not done, continue...(Recursion comes in now!!)  
        //                  if (Number.Substring(0, pos) != "0" && Number.Substring(pos) != "0")  
        //                  {  
        //                      try  
        //                      {  
        //                          word = ConvertWholeNumber(Number.Substring(0, pos)) + place + ConvertWholeNumber(Number.Substring(pos));  
        //                      }  
        //                      catch { }  
        //                  }  
        //                  else  
        //                  {  
        //                      word = ConvertWholeNumber(Number.Substring(0, pos))  + ConvertWholeNumber(Number.Substring(pos));  
        //                  }  

        //                  //check for trailing zeros  
        //                 //if (beginsZero) word = " and " + word.Trim();  
        //              }  
        //              //ignore digit grouping names  
        //              if (word.Trim().Equals(place.Trim())) word = "";  
        //          }  
        //      }  
        //      catch { }  
        //      return word.Trim();  
        //  }
        //private static String ConvertToWords(String numb)
        //{
        //    String val = "", wholeNo = numb, points = "", andStr = "", pointStr = "";
        //    String endStr = "Only";
        //    try
        //    {
        //        int decimalPlace = numb.IndexOf(".");
        //        if (decimalPlace > 0)
        //        {
        //            wholeNo = numb.Substring(0, decimalPlace);
        //            points = numb.Substring(decimalPlace + 1);
        //            if (Convert.ToInt32(points) > 0)
        //            {
        //                andStr = "and";// just to separate whole numbers from points/cents  
        //                endStr = "Paisa " + endStr;//Cents  
        //                pointStr = ConvertDecimals(points);
        //            }
        //        }
        //        val = String.Format("{0} {1}{2} {3}", ConvertWholeNumber(wholeNo).Trim(), andStr, pointStr, endStr);
        //    }
        //    catch { }
        //    return val;
        //}
        //private static String ConvertDecimals(String number)
        //{
        //    String cd = "", digit = "", engOne = "";
        //    for (int i = 0; i < number.Length; i++)
        //    {
        //        digit = number[i].ToString();
        //        if (digit.Equals("0"))
        //        {
        //            engOne = "Zero";
        //        }
        //        else
        //        {
        //            engOne = ones(digit);
        //        }
        //        cd += " " + engOne;
        //    }
        //    return cd;
        //}  
        //#endregion

    }
}