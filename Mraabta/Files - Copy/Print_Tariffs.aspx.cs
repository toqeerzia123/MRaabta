using MRaabta.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MRaabta.Files
{
    public partial class Print_Tariffs : System.Web.UI.Page
    {
        CommonFunction fun = new CommonFunction();
        Cl_Variables clvar = new Cl_Variables();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[]
                {
                new DataColumn("ID"),
                new DataColumn("DestinationID"),
                new DataColumn("Destination"),
                new DataColumn("ServiceID",typeof(string)),
                new DataColumn("FromWeight",typeof(string)),
                new DataColumn("ToWeight"),
                new DataColumn("Price"),
                new DataColumn("AddFactor"),
                new DataColumn("isUpdated")
                });
                dt.AcceptChanges();
                ViewState["dt"] = dt;

                string ClientiD = Request.QueryString["ClientID"].ToString();
                string Servicetype = Request.QueryString["ServiceType"].ToString();
                CustomerHeader(ClientiD);
                Tariffinformation(ClientiD, Servicetype);
            }
        }

        public void CustomerHeader(string ClientiD)
        {
            string query = "SELECT cc.id       clientid, \n"
               + "       cc.accountno, \n"
               + "       cc.name     customername, \n"
               + "       z.name      zone, \n"
               + "       b.name      branch, \n"
               + "       a.name      industry, \n"
               + "       CASE  \n"
               + "            WHEN cc.creditClientType = '0' THEN 'Credit' \n"
               + "            ELSE 'Cash' \n"
               + "       END         CType, \n"
               + "       CASE  \n"
               + "            WHEN cc.centralizedClient = '0' THEN 'No' \n"
               + "            ELSE 'Yes' \n"
               + "       END         Iscentralized, isnull(CG.name,'N/A') ClientGroup \n"
               + " FROM   CreditClients cc \n"
               + "       INNER JOIN tblAdminIndustry A \n"
               + "            ON  cc.IndustryId = a.Id \n"
               + "       INNER JOIN Branches b \n"
               + "            ON  cc.branchCode = b.branchCode \n"
               + "       INNER JOIN zones z \n"
               + "            ON  b.zoneCode = z.zoneCode \n"
               + "       LEFT OUTER JOIN ClientGroups cg \n"
               + "            ON  cG.id= CC.clientGrpId \n"
               + " WHERE cc.id='" + ClientiD + "'";


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

            if (dt.Rows.Count != 0)
            {
                lbl_Zone.Text = dt.Rows[0]["Zone"].ToString();
                lbl_Branch.Text = dt.Rows[0]["branch"].ToString();
                lbl_id.Text = dt.Rows[0]["clientid"].ToString();
                lbl_AccountNo.Text = dt.Rows[0]["accountNo"].ToString();
                this.lbl_CustomeName.Text = dt.Rows[0]["customername"].ToString();
                lbl_CustomerType.Text = dt.Rows[0]["CType"].ToString();
                lbl_Industry.Text = dt.Rows[0]["Industry"].ToString();
                lbl_ReceiptDate.Text = DateTime.Now.Date.ToShortDateString();
                lbl_Dd.Text = DateTime.Now.Date.ToString("dd/MM/yyyy");
                lb_CentralizedClient.Text = dt.Rows[0]["Iscentralized"].ToString();
                lb_ClientGroup.Text = dt.Rows[0]["ClientGroup"].ToString();
            }
        }

        public void Tariffinformation(string client, string servicetype)
        {
            clvar.CustomerClientID = client;// creditclientid.Value;
            clvar.ServiceTypeName = servicetype;//dd_serviceType.SelectedValue;
            clvar.Branch = Session["BRANCHCODE"].ToString(); //dd_branch.SelectedValue;
            DataTable tariff = ViewState["dt"] as DataTable;
            tariff.Clear();
            DataTable dt = GetTarrifForEdit_1(clvar);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        DataRow dr = tariff.NewRow();
                        dr["ID"] = row["ID"].ToString();
                        dr["DestinationID"] = row["TOZONECODE"].ToString();
                        dr["Destination"] = row["ZONENAME"].ToString();
                        dr["FromWeight"] = row["FromWeight"].ToString();
                        dr["ToWeight"] = row["toWeight"].ToString();
                        dr["addFactor"] = row["AdditionalFactor"].ToString();
                        dr["ServiceID"] = row["ServiceID"].ToString();
                        dr["Price"] = row["Price"].ToString();
                        tariff.Rows.Add(dr);
                        tariff.AcceptChanges();

                    }
                    //btn_addPrice.Enabled = false;

                }
                else
                {
                    gv_tariff_.DataSource = null;
                    gv_tariff_.DataBind();
                    // btn_addPrice.Enabled = true;

                }
            }

            if (tariff != null)
            {
                if (tariff.Rows.Count > 0)
                {
                    var newDataTable = tariff.AsEnumerable()
                       .OrderBy(r => r.Field<string>("ServiceID"))
                       .ThenBy(r => r.Field<string>("Destination"))
                       .ThenBy(r => r.Field<string>("FromWeight"))
                       .CopyToDataTable();

                    tariff = newDataTable;// newDataTable.CopyToDataTable();
                    gv_tariff_.DataSource = tariff;
                    gv_tariff_.DataBind();
                }
            }

        }



        public DataTable GetTarrifForEdit_1(Cl_Variables clvar)
        {
            string sqlString = " select z.name ZoneName, t.* From tempClientTariff t \n" +
                "inner join Zones z\n" +
             "on z.zoneCode = t.ToZoneCode\n" +
             "where t.Client_Id = '" + clvar.CustomerClientID + "'\n" +
             " and t.ServiceID = '" + clvar.ServiceTypeName + "' \n" +
             "--and isintltariff = '0' \n" +
             "and chkdeleted = '0' \n" +
             "and t.branchCode = '" + clvar.Branch + "'\n" +
             "order by ZoneName, ToZoneCode, FromWeight";
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
    }
}