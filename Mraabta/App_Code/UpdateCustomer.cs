using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
using System.Net;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;

/// <summary>
/// Summary description for UpdateCustomer
/// </summary>
/// 

namespace MRaabta.App_Code
{
    public class UpdateCustomer
    {
        public UpdateCustomer()
        {
            //
            // TODO: Add constructor logic here
            //
        }


        public List<string> Update_OrderStatus(string accountNo, string orderNo, string Itemstatus)
        {

            List<string> responseList = new List<string>();

            DataSet ds = Get_StatusMapping(accountNo, Itemstatus);

            string customerOrderStatus = "";

            string customerItemStatus = "";


            if (ds.Tables[0].Rows.Count > 0)
            {
                customerOrderStatus = ds.Tables[0].Rows[0]["Customer_status"].ToString();
                customerItemStatus = ds.Tables[0].Rows[0]["Customer_item_status"].ToString();
            }
            else
            {
                return responseList;
            }



            string url0 = "https://www.mygerrys.com/api/order_status.php?apiuser=gerry&apipass=gerry@135&order_id=" + orderNo + "&status_id=" + customerOrderStatus + "&remarks=order";


            try
            {

                WebClient c = new WebClient();
                var data = c.DownloadString(url0);
                //Console.WriteLine(data);
                JObject o = JObject.Parse(data);

                string id = o["id"].ToString();
                string message = o["message"].ToString();

                responseList.Add(id);
                responseList.Add(message);

            }
            catch (Exception sdf)
            {

            }

            return responseList;
        }

        public List<string> Update_ItemStatus(string accountNo, string orderNo, string Itemstatus, string itemID)
        {
            List<string> responseList = new List<string>();

            DataSet ds = Get_StatusMapping(accountNo, Itemstatus);

            string customerItemStatus = "";

            if (ds.Tables[0].Rows.Count > 0)
            {
                customerItemStatus = ds.Tables[0].Rows[0]["Customer_item_status"].ToString();
            }
            else
            {
                return responseList;
            }




            //string url0 = "https://www.mygerrys.com/api/order_status.php?apiuser=gerry&apipass=gerry@135&order_id=" + orderNo + "&status_id=" + status + "&remarks=order";

            string url1 = "https://www.mygerrys.com/api/item_status.php?apiuser=gerry&apipass=gerry@135&order_id=" + orderNo + "&status_id=" + customerItemStatus + "&item_id=" + itemID + "&remarks=item";

            try
            {

                WebClient c = new WebClient();
                var data = c.DownloadString(url1);
                //Console.WriteLine(data);
                JObject o = JObject.Parse(data);

                string id = o["id"].ToString();
                string message = o["message"].ToString();

                responseList.Add(id);
                responseList.Add(message);

            }
            catch (Exception sdf)
            {

            }

            return responseList;
        }


        public DataSet Get_StatusMapping(string accountNo, string itemStatus)
        {
            string sqlString = "select * from COD_Customer_Status_Mapping where accountNo = '" + accountNo + "' and ocs_status = '" + itemStatus + "' and customer_status is not null";

            DataSet ds = new DataSet();
            try
            {
                Cl_Variables clvar = new Cl_Variables();
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                orcl.Close();
            }
            catch (Exception Err)
            { }
            finally
            { }
            return ds;
        }
    }
}