using MRaabta.App_Code;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MRaabta.Files
{
    public partial class CNTI_ReconcilePrint : System.Web.UI.Page
    {
        Cl_Variables clvar = new Cl_Variables();
        int Sno = 0;
        bool first = true;
        string AccountNumber = "";
        string RiderCode = "";
        string AccountName = "";
        string RiderName = "";
        string services = "";
        string bookingDate = "";
        string bagnumber = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            PrintLoadSheet();
        }
        public void PrintLoadSheet()
        {
            string cns = "";
            List<string> cn = Session["cns"] as List<string>;
            if (cn != null)
            {
                foreach (string item in cn)
                {
                    cns += "'" + item + "'";
                }

                cns = cns.Replace("''", "','");
                //SyncerService.SyncerService_SM1 sz = new SyncerService.SyncerService_SM1();
                DataTable Dt = GetSalezmanTIs(cns);
                DataTable mrDt = GetMRaabtaConsignments(cns);

                #region MyRegion
                if (Dt == null)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Could Not connect to M&P. Please Try again or Contact IT Support.')", true);
                    return;
                }
                if (Dt.Rows.Count == 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('NO Transfer Invoices Found. Please Try again or Contact IT Support.')", true);
                    return;
                }
                if (mrDt == null)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('No Data Found. Please Contact IT Support.')", true);
                    return;
                }

                if (mrDt.Rows.Count == 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('No Consignments Found. Please Contact IT Support.')", true);
                    return;
                }
                #endregion


                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[] {
                new DataColumn("consignmentNumber"),
                new DataColumn("bagNumber"),
                new DataColumn("ServiceType"),
                new DataColumn("cnweight"),
                new DataColumn("cnPieces"),
                new DataColumn("ORIGIN"),
                new DataColumn("Destination"),
                new DataColumn("TransferInvoice"),
                new DataColumn("weight"),
                new DataColumn("CRTN"),
                new DataColumn("Loose"),
                new DataColumn("BookingDate"),
                new DataColumn("AllocationDateTime"),
                new DataColumn("QTY3")
            });
                AccountName = mrDt.Rows[0]["AccountName"].ToString();
                AccountNumber = mrDt.Rows[0]["AccountNo"].ToString();
                RiderCode = mrDt.Rows[0]["RiderCode"].ToString();
                RiderName = mrDt.Rows[0]["RiderName"].ToString();
                DataView view = new DataView(mrDt);
                DataTable distinctValues = view.ToTable(true, "ServiceTypeName");
                foreach (DataRow dr in distinctValues.Rows)
                {
                    services += dr[0].ToString() + ", ";
                }
                services = services.TrimEnd(' ').TrimEnd(',');
                foreach (DataRow row in mrDt.Rows)
                {
                    DataRow dr = dt.NewRow();
                    dr["consignmentNumber"] = row["consignmentNumber"].ToString();
                    dr["ServiceType"] = row["ServiceTypeName"].ToString();
                    dr["BookingDate"] = row["BookingDate"].ToString();
                    dr["cnweight"] = row["weight"].ToString();
                    dr["cnPieces"] = row["pieces"].ToString();
                    dr["ORIGIN"] = row["org"].ToString();

                    dr["AllocationDateTime"] = row["CreatedON"].ToString();
                    dr["bagNumber"] = row["bagNumber"].ToString();
                    DataRow drr = Dt.Select("consignment_no = '" + row["consignmentNumber"].ToString() + "'").FirstOrDefault();
                    if (drr != null)
                    {
                        dr["TransferInvoice"] = drr["ref_doc_no"].ToString();
                        dr["weight"] = drr["weight"].ToString();
                        dr["CRTN"] = drr["Carton"].ToString();
                        dr["LOOSE"] = drr["LOOSE"].ToString();
                        dr["QTY3"] = drr["QTY3"].ToString();
                        dr["Destination"] = row["dest"].ToString() + '-' + drr["LOCATION"].ToString();
                    }
                    dt.Rows.Add(dr);
                }

                bookingDate = dt.Rows[0]["BookingDate"].ToString();

                DataView dv = dt.AsDataView();
                dv.Sort = "Destination, ConsignmentNumber";
                PrintReport(dv.ToTable());


            }
        }
        public DataTable GetSalezmanTIs(string cns)
        {

            //string sqlString = "SELECT A.DISTRIBUTOR,\n" +
            //"       A.REF_DOC_NO,\n" +
            //"       A.CONSIGNMENT_NO,\n" +
            //"       A.WEIGHT,\n" +
            //"       SUM(A.CARTON) CARTON,\n" +
            //"       A.LOOSE,\n" +
            //"       SUM(A.QTY3) QTY3,\n" +
            //"       A.LOCATION\n" +
            //"  FROM (SELECT GC.DISTRIBUTOR,\n" +
            //"               GC.REF_DOC_NO,\n" +
            //"               GC.CONSIGNMENT_NO,\n" +
            //"               GC.WEIGHT,\n" +
            //"               FLOOR(SUM(GD.QTY3) / GD.SELL_FACTOR1) CARTON,\n" +
            //"               GR.LOOSE_CARTON LOOSE,\n" +
            //"               SUM(GD.QTY3) QTY3,\n" +
            //"               GC.\"LOCATION\"\n" +
            //"          FROM GATEPASS_CONSIGNMENT GC\n" +
            //"         INNER JOIN GATEPASS_DETAIL2 GD\n" +
            //"            ON GD.COMPANY = GC.COMPANY\n" +
            //"           AND GD.DISTRIBUTOR = GC.DISTRIBUTOR\n" +
            //"           AND GD.DOCUMENT = GC.DOCUMENT\n" +
            //"           AND GD.DOC_NO = GC.DOC_NO\n" +
            //"           AND GD.SUB_DOCUMENT = GC.SUB_DOCUMENT\n" +
            //"           AND GD.REF_DOCUMENT = GC.REF_DOCUMENT\n" +
            //"           AND GD.REF_SUB_DOCUMENT = GC.REF_SUB_DOCUMENT\n" +
            //"           AND GD.REF_DOC_NO = GC.REF_DOC_NO\n" +
            //"         INNER JOIN GATEPASS_RA GR\n" +
            //"            ON GR.COMPANY = GD.COMPANY\n" +
            //"           AND GR.DISTRIBUTOR = GD.DISTRIBUTOR\n" +
            //"           AND GR.DOCUMENT = GD.DOCUMENT\n" +
            //"           AND GR.SUB_DOCUMENT = GD.SUB_DOCUMENT\n" +
            //"           AND GR.DOC_NO = GD.DOC_NO\n" +
            //"           AND GR.REF_DOCUMENT = GD.REF_DOCUMENT\n" +
            //"           AND GR.REF_SUB_DOCUMENT = GD.REF_SUB_DOCUMENT\n" +
            //"           AND GR.REF_DOC_NO = GD.REF_DOC_NO\n" +
            //"         WHERE GC.CONSIGNMENT_NO IN\n" +
            //"               (" + cns + ")\n" +
            //"         GROUP BY GC.DISTRIBUTOR,\n" +
            //"                  GC.REF_DOC_NO,\n" +
            //"                  GC.CONSIGNMENT_NO,\n" +
            //"                  GC.WEIGHT,\n" +
            //"                  GD.SKU,\n" +
            //"                  GD.SELL_FACTOR1,\n" +
            //"                  GR.LOOSE_CARTON,\n" +
            //"                  GC.LOCATION) A\n" +
            //" GROUP BY A.DISTRIBUTOR,\n" +
            //"          A.REF_DOC_NO,\n" +
            //"          A.CONSIGNMENT_NO,\n" +
            //"          A.WEIGHT,\n" +
            //"          A.LOOSE,\n" +
            //"          A.\"LOCATION\"";

            string sqlString = @"SELECT D.FROMLOCATION DISTRIBUTOR, D.TRANSFERINVOICE_NO REF_DOC_NO, D.CONSIGNMENT_NO, 
                                  D.TI_WEIGHT WEIGHT, D.TI_CRTN CARTON, D.TI_LOOSE LOOSE, 0 QTY3,D.TOLOCATION LOCATION
                                  FROM OFUSION.OF_SHIPMENT_DETAILS D
                                  WHERE D.COMPANY = '01' AND D.CONSIGNMENT_NO IN (" + cns + ")";

            //SalezmanConnection.Connector szConStr = new SalezmanConnection.Connector();
            BridgeConnector.Connector con = new BridgeConnector.Connector();

            //OracleConnection con = new OracleConnection("Data Source=salesman_new; User ID=salezman;password=ak100zelas;Persist Security Info=False"); //new OracleConnection((szConStr.Get_ConnectionString()));
            DataTable dt = new DataTable();
            sqlString = Encrypt(sqlString);
            dt = con.Get_datatable(sqlString);
            try
            {
                //con.Open();
                //OracleDataAdapter oda = new OracleDataAdapter(sqlString, con);
                //oda.Fill(dt);

            }
            catch (Exception ex)
            { } //con.Close(); 
            return dt;

        }

        public DataTable GetMRaabtaConsignments(string cns)
        {

            string sqlString = "select c.consignmentNumber,\n" +
            "       c.serviceTypeName,\n" +
            "       c.weight,\n" +
            "       c.pieces,\n" +
            "       b.sname             org,\n" +
            "       b1.sname            dest\n" +
            "  from consignment c\n" +
            " inner join branches b\n" +
            "    on b.branchCode = c.orgin\n" +
            " inner join branches b1\n" +
            "    on b1.branchCode = c.destination\n" +
            " where c.consignmentNumber in (" + cns + ")";


            sqlString = "select c.consignmentNumber,\n" +
            "       c.serviceTypeName,\n" +
            "       c.weight,\n" +
            "       c.pieces,\n" +
            "       b.sname org,\n" +
            "       b1.sname dest,\n" +
            "       r.firstName + ' ' + r.lastName RiderName\n" +
            "  from consignment c\n" +
            " inner join branches b\n" +
            "    on b.branchCode = c.orgin\n" +
            " inner join branches b1\n" +
            "    on b1.branchCode = c.destination\n" +
            " inner join Riders r\n" +
            "    on r.riderCode = c.riderCode\n" +
            "   and r.branchId = c.branchCode\n" +
            " where c.consignmentNumber in (" + cns + ")";

            sqlString = "select c.consignmentNumber,\n" +
            "       c.serviceTypeName,\n" +
            "       c.weight,\n" +
            "       c.pieces,\n" +
            "       b.sname org,\n" +
            "       b1.sname dest,\n" +
            "       r.firstName + ' ' + r.lastName RiderName, r.RiderCode,\n" +
            "       cc.accountNo,\n" +
            "       cc.name AccountName, convert(varchar, c.bookingdate, 103) bookingDate, c.createdon\n" +
            "  from consignment c\n" +
            " inner join branches b\n" +
            "    on b.branchCode = c.orgin\n" +
            " inner join branches b1\n" +
            "    on b1.branchCode = c.destination\n" +
            " inner join Riders r\n" +
            "    on r.riderCode = c.riderCode\n" +
            "   and r.branchId = c.branchCode\n" +
            " inner join creditclients cc\n" +
            "    on cc.id = c.creditClientId\n" +
            "\n" +
            " where c.consignmentNumber in (" + cns + ")\n" +
            "   and cc.isActive = '1'";



            sqlString = "select c.consignmentNumber,\n" +
            "       c.serviceTypeName,\n" +
            "       c.weight,\n" +
            "       c.pieces,\n" +
            "       bo.bagNumber,\n" +
            "       b.sname org,\n" +
            "       b1.sname dest,\n" +
            "       r.firstName + ' ' + r.lastName RiderName,\n" +
            "       r.RiderCode,\n" +
            "       cc.accountNo,\n" +
            "       cc.name AccountName,\n" +
            "       convert(varchar, c.bookingdate, 103) bookingDate,\n" +
            "       c.createdon\n" +
            "  from consignment c\n" +
            " inner join branches b\n" +
            "    on b.branchCode = c.orgin\n" +
            " inner join branches b1\n" +
            "    on b1.branchCode = c.destination\n" +
            " inner join Riders r\n" +
            "    on r.riderCode = c.riderCode\n" +
            "   and r.branchId = c.branchCode\n" +
            " inner join creditclients cc\n" +
            "    on cc.id = c.creditClientId\n" +
            "  LEFT OUTER JOIN BagOutpieceAssociation bo\n" +
            "    ON bo.outpieceNumber = c.consignmentNumber\n" +
            "   AND bo.createdOn =\n" +
            "       (SELECT MAX(bo2.createdon)\n" +
            "          FROM BagOutpieceAssociation bo2\n" +
            "         WHERE bo2.outpieceNumber = c.consignmentNumber)\n" +
            " where c.consignmentNumber in (" + cns + ")\n" +
            "   and cc.isActive = '1'";



            SqlConnection con = new SqlConnection(clvar.Strcon());
            DataTable dt = new DataTable();
            try
            {
                con.Open();
                SqlDataAdapter oda = new SqlDataAdapter(sqlString, con);
                oda.Fill(dt);
            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return dt;
        }


        public void PrintReport(DataTable dt)
        {

            StringBuilder html = new StringBuilder();
            html.Append("<div style=\"height: 950px; width:780px;\">");
            html.Append(HeaderTable());
            html.Append(HeaderRow());

            first = false;

            double cnWeight = 0;
            double tiWeight = 0;

            int cnPieces = 0;
            int tiCarton = 0;
            int tiLoose = 0;
            int tiQty = 0;

            int additionalRows = 0;
            int depotRowCount = 0;
            int totalDepotRows = 0;
            int rowCount = 0;


            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i > 0)
                {
                    if (dt.Rows[i - 1]["Destination"].ToString().ToUpper() != dt.Rows[i]["Destination"].ToString().ToUpper())
                    {
                        if (depotRowCount > 1)
                        {
                            html.Append(SubTotalRow(cnWeight, tiWeight, cnPieces, tiCarton, tiLoose, tiQty));
                            additionalRows++;
                        }

                        cnWeight = 0;
                        tiWeight = 0;
                        cnPieces = 0;
                        tiCarton = 0;
                        tiLoose = 0;
                        tiQty = 0;
                        depotRowCount = 0;
                    }
                }

                if ((rowCount + additionalRows) >= 33 && i > 0)
                {
                    html.Append("</table>");
                    html.Append("</div>");
                    //html.Append(SignatureRows());
                    html.Append("<div style=\"height: 950px; width:780px;\">");
                    html.Append(HeaderTable());
                    html.Append(HeaderRow());
                    additionalRows = 0;
                    rowCount = 0;
                }

                rowCount++;
                depotRowCount++;
                totalDepotRows = dt.Select("Destination = '" + dt.Rows[i]["Destination"].ToString() + "'").Count();
                string rowStyle = "";
                if (depotRowCount == 1 && totalDepotRows > 1)
                {
                    rowStyle = "border-top: 2px Solid Black;";
                }
                //else if (depotRowCount == totalDepotRows && totalDepotRows > 1)
                //{
                //    rowStyle = "border-bottom: 2px Solid Black;";
                //}

                html.Append(DataRow(dt.Rows[i], rowStyle));

                double dtemp = 0;

                double.TryParse(dt.Rows[i]["cnWeight"].ToString(), out dtemp);
                cnWeight += dtemp;
                dtemp = 0;

                double.TryParse(dt.Rows[i]["Weight"].ToString(), out dtemp);
                tiWeight += dtemp;
                dtemp = 0;

                int itemp = 0;

                int.TryParse(dt.Rows[i]["cnPieces"].ToString(), out itemp);
                cnPieces += itemp;
                itemp = 0;

                int.TryParse(dt.Rows[i]["CRTN"].ToString(), out itemp);
                tiCarton += itemp;
                itemp = 0;

                int.TryParse(dt.Rows[i]["LOOSE"].ToString(), out itemp);
                tiLoose += itemp;
                itemp = 0;

                int.TryParse(dt.Rows[i]["QTY3"].ToString(), out itemp);
                tiQty += itemp;
                itemp = 0;
            }
            if (depotRowCount > 1)
            {
                html.Append(SubTotalRow(cnWeight, tiWeight, cnPieces, tiCarton, tiLoose, tiQty));
            }

            html.Append(FooterRow(dt));
            html.Append("</table>");
            html.Append("<br />");
            html.Append(SignatureRows());
            html.Append("</div>");



            ph1.Controls.Add(new Literal { Text = html.ToString() });
        }

        public string HeaderTable()
        {
            string pageBreak = "";
            if (first)
            {
                pageBreak = "";
            }
            else
            {
                pageBreak = "page-break-before: always;";
            }
            string sqlString = "";

            sqlString = "<table style=\"width: 100%; font-size: medium; font-family: Calibri; " + pageBreak + "\">\n" +
            "           <tr>\n" +
            "               <td style=\"width: 35%\" rowspan='1'>\n" +
            "                   <img src=\"../images/OCS_Transparent.png\" style=\"height: 50px; width: 140px\" />\n" +
            "               </td>\n" +
            "               <td style=\"width: 36%; text-align: center; font-size: large; font-weight: bold;\">\n" +
            "                   LOAD SHEET&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;\n" +
            "               </td>\n" +
            "               <td style=\"width: 35%; text-align: left;\">\n" +
            "                   Print Date: " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + "\n" +
            "                   <br />\n" +
            "                   Booking Date: <span style='font-size:small; font-weight:bold;'>" + bookingDate + "</span>\n" +
            "                   <br />\n" +
            "                   Service(s): <span style='font-size:small; font-weight:bold;'>" + services + "</span>\n" +
            "               </td>\n" +
            "           </tr>\n" +
            "           <tr>\n" +
            "               <td style=\"width: 35%\">\n" +
            "                   <b>Account No:</b> " + AccountNumber.ToString() + "\n" +
            "               </td>\n" +
            "               <td style=\"width: 30%\">\n" +
            "               </td>\n" +
            "               <td style=\"width: 35%; text-align:left;\">\n" +
            "                   <b>Rider Code: </b>" + RiderCode.ToString() + "\n" +
            "               </td>\n" +
            "           </tr>\n" +
            "           <tr>\n" +
            "               <td style=\"width: 65%\" colspan='2'>\n" +
            "                   <b>Account Name: </b>" + AccountName + "\n" +
            "               </td>\n" +
            //"               <td style=\"width: 30%\">\n" +
            //"               </td>\n" +
            "               <td style=\"width: 35%; text-align:left;\">\n" +
            "                   <b>Rider Name: </b>" + RiderName + "\n" +
            "               </td>\n" +
            "           </tr>\n" +
            "       </table>";



            return sqlString;
        }
        public string HeaderRow()
        {
            string sqlString = "";

            sqlString = "<table style=\"width: 100%; font-size: medium; font-family: Calibri; border-collapse: collapse;\n" +
            "            border: 2px Solid Black;\">\n" +
            "            <tr>\n" +
            "                <td style=\"width: 5%; text-align: center;\">\n" +
            "                </td>\n" +
            "                <td style=\"width: 57%; border: 2px Solid Black; text-align: center;\" colspan=\"7\">\n" +
            "                    <span style='font-size:small; font-weight:bold;'><b>M&P Express Logistics</b></span>\n" +
            "                </td>\n" +
            "                <td style=\"width: 38%; border: 2px Solid Black; text-align: center;\" colspan=\"5\">\n" +
            "                    <span style='font-size:small; font-weight:bold;'><b>M&P Pakistan Pvt. Ltd.</b></span>\n" +
            "                </td>\n" +
            "            </tr>\n" +
            "            <tr>\n" +
            "                <td style=\"width: 4%; border: 2px Solid Black; text-align:center;\">\n" +
            "                    <span style='font-size:small; font-weight:bold;'><b>SNo.</b></span>\n" +
            "                </td>\n" +
            "                <td style=\"width: 13%; border: 2px Solid Black; text-align:center;\">\n" +
            "                    <span style='font-size:small; font-weight:bold;'><b>CN Number</b></span>\n" +
            "                </td>\n" +
            "                <td style=\"width: 13%; border: 2px Solid Black; text-align:center;\">\n" +
            "                    <span style='font-size:small; font-weight:bold;'><b>Bag Number</b></span>\n" +
            "                </td>\n" +
            "                <td style=\"width: 13%; border: 2px Solid Black; text-align:center;\">\n" +
            "                    <span style='font-size:small; font-weight:bold;'><b>Allocation DateTime</b></span>\n" +
            "                </td>\n" +
            "                <td style=\"width: 8%; border: 2px Solid Black; text-align:center;\">\n" +
            "                    <span style='font-size:small; font-weight:bold;'><b>CN. Weight</b></span>\n" +
            "                </td>\n" +
            "                <td style=\"width: 6%; border: 2px Solid Black; text-align:center;\">\n" +
            "                    <span style='font-size:small; font-weight:bold;'><b>Pieces</b></span>\n" +
            "                </td>\n" +
            "                <td style=\"width: 4%; border: 2px Solid Black; text-align:center;\">\n" +
            "                    <span style='font-size:small; font-weight:bold;'><b>ORGN.</b></span>\n" +
            "                </td>\n" +
            "                <td style=\"width: 8%; border: 2px Solid Black; text-align:center; font-size: smaller;\">\n" +
            "                    <span style='font-size:small; font-weight:bold;'><b>DEST.</b></span>\n" +
            "                </td>\n" +
            "                <td style=\"width: 10%; border: 2px Solid Black; text-align:center;\">\n" +
            "                    <span style='font-size:small; font-weight:bold;'><b>TI #</b></span>\n" +
            "                </td>\n" +
            "                <td style=\"width: 8%; border: 2px Solid Black; text-align:center;\">\n" +
            "                    <span style='font-size:small; font-weight:bold;'><b>TI Weight</b></span>\n" +
            "                </td>\n" +
            "                <td style=\"width: 4%; border: 2px Solid Black; text-align:center;\">\n" +
            "                    <span style='font-size:small; font-weight:bold;'><b>CRTN.</b></span>\n" +
            "                </td>\n" +
            "                <td style=\"width: 4%; border: 2px Solid Black; text-align:center;\">\n" +
            "                    <span style='font-size:small; font-weight:bold;'><b>Loose</b></span>\n" +
            "                </td>\n" +
            "                <td style=\"width: 5%; border: 2px Solid Black; text-align:center;\">\n" +
            "                    <span style='font-size:small; font-weight:bold;'><b>QTY</b></span>\n" +
            "                </td>\n" +
            "            </tr>";



            return sqlString;
        }
        public string DataRow(DataRow dr, string rowStyle)
        {
            Sno++;
            string sqlString =
            "            <tr style='" + rowStyle + "'>\n" +
            "                <td style=\"width: 4%; border: 1px Solid Black; text-align:center;\">\n" +
            "                    <span style='font-size:small'> " + Sno.ToString() + "</span>\n" +
            "                </td>\n" +
            "                <td style=\"width: 13%; border: 1px Solid Black; text-align:center;\">\n" +
            "                    <span style='font-size:small'> " + dr["consignmentNumber"].ToString() + "</span>\n" +
            "                </td>\n" +
            "                <td style=\"width: 13%; border: 1px Solid Black; text-align:center;\">\n" +
            "                    <span style='font-size:small'> " + dr["bagNumber"].ToString() + "</span>\n" +
            "                </td>\n" +
            "                <td style=\"width: 13%; border: 1px Solid Black; text-align:center;\">\n" +
            "                   <span style='font-size:x-small'> " + dr["AllocationDateTime"].ToString() + "</span>\n" +
            "                </td>\n" +
            "                <td style=\"width: 8%; border: 1px Solid Black; text-align:center;\">\n" +
            "                    <span style='font-size:small'> " + dr["cnweight"].ToString() + "</span>\n" +
            "                </td>\n" +
            "                <td style=\"width: 6%; border: 1px Solid Black; text-align:center;\">\n" +
            "                    <span style='font-size:small'> " + dr["cnpieces"].ToString() + "</span>\n" +
            "                </td>\n" +
            "                <td style=\"width: 4%; border: 1px Solid Black; text-align:center;\">\n" +
            "                    <span style='font-size:small'> " + dr["Origin"].ToString() + "</span>\n" +
            "                </td>\n" +
            "                <td style=\"width: 8%; border: 1px Solid Black; text-align:left;\">\n" +
            "                    <span style='font-size:small'> " + dr["Destination"].ToString() + "</span>\n" +
            "                </td>\n" +
            "                <td style=\"width: 10%; border: 1px Solid Black; text-align:center;\">\n" +
            "                    <span style='font-size:small'> " + dr["TransferInvoice"].ToString() + "</span>\n" +
            "                </td>\n" +
            "                <td style=\"width: 8%; border: 1px Solid Black; text-align:center;\">\n" +
            "                    <span style='font-size:small'> " + dr["Weight"].ToString() + "</span>\n" +
            "                </td>\n" +
            "                <td style=\"width: 4%; border: 1px Solid Black; text-align:center;\">\n" +
            "                    <span style='font-size:small'> " + dr["CRTN"].ToString() + "</span>\n" +
            "                </td>\n" +
            "                <td style=\"width: 4%; border: 1px Solid Black; text-align:center;\">\n" +
            "                    <span style='font-size:small'> " + dr["LOOSE"].ToString() + "</span>\n" +
            "                </td>\n" +
             "                <td style=\"width: 5%; border: 1px Solid Black; text-align:center;\">\n" +
            "                   <span style='font-size:small'> " + dr["QTY3"].ToString() + "</span>\n" +
            "                </td>\n" +
            "            </tr>";
            return sqlString;
        }
        public string SubTotalRow(double cnWeight, double tiWeight, int cnPieces, int tiCarton, int tiLoose, int tiQty)
        {

            string sqlString =
            "            <tr style='border: 2px solid black;'>\n" +
            "                <td style=\"width: 4%; border: 1px Solid Black; text-align:center;\">\n" +
            "                    <span style='font-size:small'></span>\n" +
            "                </td>\n" +
            "                <td style=\"width: 13%; border: 1px Solid Black; text-align:center;\">\n" +
            "                    <span style='font-size:small'></span>\n" +
            "                </td>\n" +
            "                <td style=\"width: 13%; border: 1px Solid Black; text-align:center;\">\n" +
            "                    <span style='font-size:small'> </span>\n" +
            "                </td>\n" +
            "                <td style=\"width: 13%; border: 1px Solid Black; text-align:center;\">\n" +
            "                   <span style='font-size:x-small'> </span>\n" +
            "                </td>\n" +
            "                <td style=\"width: 8%; border: 1px Solid Black; text-align:center;\">\n" +
            "                    <span style='font-size:small; font-weight: bold;'>" + cnWeight.ToString() + "</span>\n" +
            "                </td>\n" +
            "                <td style=\"width: 6%; border: 1px Solid Black; text-align:center;\">\n" +
            "                    <span style='font-size:small; font-weight: bold;'>" + cnPieces.ToString() + "</span>\n" +
            "                </td>\n" +
            "                <td style=\"width: 4%; border: 1px Solid Black; text-align:center;\">\n" +
            "                    <span style='font-size:small'> </span>\n" +
            "                </td>\n" +
            "                <td style=\"width: 8%; border: 1px Solid Black; text-align:center;\">\n" +
            "                    <span style='font-size:small'></span>\n" +
            "                </td>\n" +
            "                <td style=\"width: 10%; border: 1px Solid Black; text-align:center;\">\n" +
            "                    <span style='font-size:small'></span>\n" +
            "                </td>\n" +
            "                <td style=\"width: 8%; border: 1px Solid Black; text-align:center;\">\n" +
            "                    <span style='font-size:small; font-weight: bold;'>" + tiWeight.ToString() + "</span>\n" +
            "                </td>\n" +
            "                <td style=\"width: 4%; border: 1px Solid Black; text-align:center;\">\n" +
            "                    <span style='font-size:small; font-weight: bold;'>" + tiCarton.ToString() + "</span>\n" +
            "                </td>\n" +
            "                <td style=\"width: 4%; border: 1px Solid Black; text-align:center;\">\n" +
            "                    <span style='font-size:small; font-weight: bold;'>" + tiLoose.ToString() + "</span>\n" +
            "                </td>\n" +
             "                <td style=\"width: 5%; border: 1px Solid Black; text-align:center;\">\n" +
            "                   <span style='font-size:small; font-weight: bold;'>" + tiQty.ToString() + "</span>\n" +
            "                </td>\n" +
            "            </tr>";
            return sqlString;
        }
        public string FooterRow(DataTable dt)
        {
            int pieces = 0;
            int TIPieces = 0;
            int TILoose = 0;
            double weight = 0;
            double TIWeight = 0;
            int qty3 = 0;

            foreach (DataRow dr in dt.Rows)
            {
                int temp = 0;

                double temp1 = 0;

                int.TryParse(dr["cnpieces"].ToString(), out temp);
                double.TryParse(dr["cnweight"].ToString(), out temp1);
                pieces += temp;
                weight += temp1;

                temp = 0;
                temp1 = 0;

                int.TryParse(dr["CRTN"].ToString(), out temp);
                double.TryParse(dr["Weight"].ToString(), out temp1);
                TIPieces += temp;
                TIWeight += temp1;

                temp = 0;
                int.TryParse(dr["LOOSE"].ToString(), out temp);
                TILoose += temp;

                temp = 0;
                int.TryParse(dr["QTY3"].ToString(), out temp);

                qty3 += temp;
            }
            Sno++;
            string sqlString =
            "            <tr>\n" +
            "                <td style=\"width: 4%; border: 2px Solid Black; text-align:center;\">\n" +
            "                    \n" +
            "                </td>\n" +
            "                <td style=\"width: 13%; border: 2px Solid Black; text-align:center;\">\n" +
            "                    <b><span style='font-size:small; font-weight:bold;'>Total</span></b>\n" +
            "                </td>\n" +
            "                <td style=\"width: 13%; border: 2px Solid Black; text-align:center;\">\n" +
            //"                    " + dr["ServiceType"].ToString() + "\n" +
            "                </td>\n" +
            "                <td style=\"width: 13%; border: 2px Solid Black; text-align:center;\">\n" +
            //"                    " + dr["ServiceType"].ToString() + "\n" +
            "                </td>\n" +
            "                <td style=\"width: 8%; border: 2px Solid Black; text-align:center;\">\n" +
            "                    <span style='font-size:small; font-weight:bold;'><b>" + String.Format("{0:N2}", weight) + "</b></span>\n" +
            "                </td>\n" +
            "                <td style=\"width: 6%; border: 2px Solid Black; text-align:center;\">\n" +
            "                    <span style='font-size:small; font-weight:bold;'><b>" + pieces.ToString() + "</b></span>\n" +
            "                </td>\n" +
            "                <td style=\"width: 4%; border: 2px Solid Black; text-align:center;\">\n" +
            //"                    " + dr["Origin"].ToString() + "\n" +
            "                </td>\n" +
            "                <td style=\"width: 6%; border: 2px Solid Black; text-align:center;\">\n" +
            //"                    " + dr["Destination"].ToString() + "\n" +
            "                </td>\n" +
            "                <td style=\"width: 12%; border: 2px Solid Black; text-align:center;\">\n" +
            //"                    " + dr["TransferInvoice"].ToString() + "\n" +
            "                </td>\n" +
            "                <td style=\"width: 8%; border: 2px Solid Black; text-align:center;\">\n" +
            "                    <span style='font-size:small; font-weight:bold;'><b>" + String.Format("{0:N2}", TIWeight) + "</b></span>\n" +
            "                </td>\n" +
            "                <td style=\"width: 4%; border: 2px Solid Black; text-align:center;\">\n" +
            "                   <span style='font-size:small; font-weight:bold;'><b>" + TIPieces.ToString() + "</b></span>\n" +
            "                </td>\n" +
            "                <td style=\"width: 4%; border: 2px Solid Black; text-align:center;\">\n" +
            "                    <span style='font-size:small; font-weight:bold;'><b>" + TILoose.ToString() + "</b></span>\n" +
            "                </td>\n" +
            "                <td style=\"width: 5%; border: 2px Solid Black; text-align:center;\">\n" +
            "                    <span style='font-size:small; font-weight:bold;'><b>" + String.Format("{0:N0}", qty3) + "</b></span>\n" +
            "                </td>\n" +
            "            </tr>";
            return sqlString;
        }

        public string SignatureRows()
        {


            return "<div style=\" width: 780px;\">\n" +
            "            <table style=\"width: 100%; border-collapse: collapse; font-family:Calibri;\">\n" +
            "                <tr>\n" +
            "                    <td style=\"width: 5%;\">\n" +
            "                    </td>\n" +
            "                    <td colspan=\"2\" style=\"width: 40%; font-variant: small-caps; text-align:center;\">\n" +
            "                        <b>M&P Personnel</b>\n" +
            "                    </td>\n" +
            "                    <td style=\"width: 10%;\">\n" +
            "                    </td>\n" +
            "                    <td colspan=\"2\" style=\"width: 40%; font-variant: small-caps; text-align:center;\">\n" +
            "                        <b>M&P Express Logistics Personnel</b>\n" +
            "                    </td>\n" +
            "                    <td style=\"width: 5%;\">\n" +
            "                    </td>\n" +
            "                </tr>\n" +
                    "                <tr>\n" +
            "                    <td style=\"width: 5%;\">\n" +
            "                    </td>\n" +
            "                    <td style=\"width: 10%; font-variant: small-caps;\">\n" +
            "\n" +
            "                    </td>\n" +
            "                    <td style=\"width: 30%; font-variant: small-caps;\">\n" +
            "                    </td>\n" +
            "                    <td style=\"width: 10%;\">\n" +
            "                    </td>\n" +
            "                    <td style=\"width: 10%; font-variant: small-caps;\">\n" +
            "                       &nbsp;\n" +
            "                    </td>\n" +
            "                    <td style=\"width: 30%; font-variant: small-caps;\">\n" +
            "                    </td>\n" +
            "                    <td style=\"width: 5%;\">\n" +
            "                    </td>\n" +
            "                </tr>\n" +

            "                <tr>\n" +
            "                    <td style=\"width: 5%;\">\n" +
            "                    </td>\n" +
            "                    <td style=\"width: 10%; font-variant: small-caps;\">\n" +
            "                        Name\n" +
            "                        <br />\n" +
            "                    </td>\n" +
            "                    <td style=\"width: 30%; font-variant: small-caps; border-bottom: 2px Solid Black;\">\n" +
            "                    </td>\n" +
            "                    <td style=\"width: 10%;\">\n" +
            "                    </td>\n" +
            "                    <td style=\"width: 10%; font-variant: small-caps;\">\n" +
            "                        Name\n" +
            "                    </td>\n" +
            "                    <td style=\"width: 30%; font-variant: small-caps; border-bottom: 2px Solid Black;\">\n" +
            "                    </td>\n" +
            "                    <td style=\"width: 5%;\">\n" +
            "                    </td>\n" +
            "                </tr>\n" +
                    "                <tr>\n" +
            "                    <td style=\"width: 5%;\">\n" +
            "                    </td>\n" +
            "                    <td style=\"width: 10%; font-variant: small-caps;\">\n" +
            "\n" +
            "                    </td>\n" +
            "                    <td style=\"width: 30%; font-variant: small-caps;\">\n" +
            "                    </td>\n" +
            "                    <td style=\"width: 10%;\">\n" +
            "                    </td>\n" +
            "                    <td style=\"width: 10%; font-variant: small-caps;\">\n" +
            "                       &nbsp;\n" +
            "                    </td>\n" +
            "                    <td style=\"width: 30%; font-variant: small-caps;\">\n" +
            "                    </td>\n" +
            "                    <td style=\"width: 5%;\">\n" +
            "                    </td>\n" +
            "                </tr>\n" +

            "                <tr>\n" +
            "                    <td style=\"width: 5%;\">\n" +
            "                    </td>\n" +
            "                    <td style=\"width: 10%; font-variant: small-caps;\">\n" +
            "                        Signature\n" +
            "                        <br />\n" +
            "                    </td>\n" +
            "                    <td style=\"width: 30%; font-variant: small-caps; border-bottom: 2px Solid Black;\">\n" +
            "                    </td>\n" +
            "                    <td style=\"width: 10%;\">\n" +
            "                    </td>\n" +
            "                    <td style=\"width: 10%; font-variant: small-caps;\">\n" +
            "                        Signature\n" +
            "                    </td>\n" +
            "                    <td style=\"width: 30%; font-variant: small-caps; border-bottom: 2px Solid Black;\">\n" +
            "                    </td>\n" +
            "                    <td style=\"width: 5%;\">\n" +
            "                    </td>\n" +
            "                </tr>\n" +
            "            </table>\n" +
            "        </div>";

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

    }
}