using MRaabta.Models.Api;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MRaabta.Repo.Api
{
    public class RunsheetDB_v5
    {

        SqlConnection conenctionString = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

        //public List<RunsheetListOnlyNumber> GetRunsheetFromRouteCode(String Route, String branchcode)
        //{
        //    List<RunsheetListOnlyNumber> runsheetlist = new List<RunsheetListOnlyNumber>();
        //    SqlDataReader rdr = null;
        //    try
        //    {
        //        conenctionString.Open();
        //        SqlCommand cmd = new SqlCommand();
        //        cmd.Connection = conenctionString;
        //        cmd.CommandText = "App_dlv_GetRunsheetFromRouteCode";
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@RouteCode", Route);
        //        cmd.Parameters.AddWithValue("@branchcode", branchcode);
        //        rdr = cmd.ExecuteReader();
        //        if (!rdr.HasRows)
        //        {
        //        }
        //        else
        //        {
        //            while (rdr.Read())
        //            {
        //                runsheetlist.Add(new RunsheetListOnlyNumber { RunsheetNumber = rdr["runsheetNumber"].ToString() });

        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception();
        //    }
        //    finally { conenctionString.Close(); }

        //    return runsheetlist;
        //}


        //public List<runsheetDeliveryData> GetDeliveryDataFromRunsheetNumber(string RunsheetNumber)
        //{
        //    List<runsheetDeliveryData> runsheetlist = new List<runsheetDeliveryData>();
        //    SqlDataReader rdr = null;
        //    DataTable ds = new DataTable();
        //    try
        //    {
        //        conenctionString.Open();
        //        SqlCommand cmd = new SqlCommand();
        //        cmd.Connection = conenctionString;
        //        cmd.CommandText = "App_dlv_GetRunsheetDT";
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@RunsheetNumber", RunsheetNumber);
        //        rdr = cmd.ExecuteReader();

        //        if (!rdr.HasRows)
        //        {
        //            return runsheetlist;
        //        }
        //        else
        //        {
        //            while (rdr.Read())
        //            {
        //                runsheetlist.Add(new runsheetDeliveryData
        //                {
        //                    Address = rdr["Address"].ToString(),
        //                    codAmount = Convert.ToInt32(rdr["codAmount"].ToString()),
        //                    consignee = rdr["consignee"].ToString(),
        //                    consigneePhoneNo = rdr["consigneePhoneNo"].ToString(),
        //                    consignmentNumber = rdr["consignmentNumber"].ToString(),
        //                    destination = rdr["destination"].ToString(),
        //                    is_cod = Convert.ToBoolean(rdr["is_cod"]),
        //                    origin = rdr["origin"].ToString(),
        //                    pieces = Convert.ToInt32(rdr["pieces"].ToString()),
        //                    Receiver_CNIC = rdr["Receiver_CNIC"].ToString(),
        //                    remarks = rdr["remarks"].ToString(),
        //                    riderCode = rdr["riderCode"].ToString(),
        //                    SortOrder = Convert.ToInt32(rdr["SortOrder"]),
        //                    weight = Convert.ToDouble(rdr["weight"]),
        //                    isMobilePerformed = Convert.ToInt32(rdr["isMobilePerformed"]),
        //                    OriginId = Convert.ToInt32(rdr["OriginId"]),
        //                    DestinationId = Convert.ToInt32(rdr["DestinationId"]),
        //                    RTSBranchId = Convert.ToInt32(rdr["RTSBranchId"])
        //                });
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message.ToLower());
        //    }
        //    finally { conenctionString.Close(); }

        //    return runsheetlist;
        //}
    }

}