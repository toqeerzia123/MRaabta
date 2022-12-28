using MRaabta.Models.Api;
using MRaabta.Repo.Api;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MRaabta.Controllers.Api
{
    public class RidersLocation_v5Controller : ApiController
    {
        RidersLocationDB_v5 Rider_LocationDB;

        public RidersLocation_v5Controller()
        {
            Rider_LocationDB = new RidersLocationDB_v5();
        }
        /// <summary>
        /// Insert Riders Location.
        /// </summary>
        [HttpPost, ActionName("InsertRidersLocation")]
        public RidersLocationInsertionResponse InsertRidersLocation([FromBody] RidersLocationInsertionMultiple LocationList)
        {
            string message = "";
            bool isSuccess = false;
            RidersLocationInsertionResponse rlir = new RidersLocationInsertionResponse();
            List<string> lt_message = new List<string>();
            List<RidersLocationModel> ridersLocationList = LocationList.ridersLocationList;
            for (int i = 0; i < ridersLocationList.Count; i++)
            {
                if (ridersLocationList[i].USER_ID == 0 || ridersLocationList[i].logTime == null || ridersLocationList[i].longitude == 0 || ridersLocationList[i].latitude == 0)
                {
                    message = "Kindly Send Proper USER_ID, logTime, longitude, latitude on object index: " + (i + 1).ToString() + "";
                    lt_message.Add(message);
                    isSuccess = false;
                    rlir = new RidersLocationInsertionResponse { message = lt_message, isSuccess = isSuccess };
                    return rlir;
                }
            }
            DataTable dt_child = new DataTable();
            dt_child.Columns.AddRange(new DataColumn[] {
            new DataColumn("USER_ID", typeof(int)),
            new DataColumn("logTime", typeof(DateTime)),
            new DataColumn("latitude", typeof(Decimal)),
            new DataColumn("longitude", typeof(Decimal))
            });

            for (int i = 0; i < ridersLocationList.Count; i++)
            {
                DataRow dr = dt_child.NewRow();
                dr[0] = ridersLocationList[i].USER_ID;
                dr[1] = ridersLocationList[i].logTime;
                dr[2] = Convert.ToDecimal(ridersLocationList[i].latitude);
                dr[3] = Convert.ToDecimal(ridersLocationList[i].longitude);
                dt_child.Rows.Add(dr);
                dt_child.AcceptChanges();
            }

            Tuple<bool, string> result = Rider_LocationDB.InsertRiderLocation(dt_child);
            if (result.Item1)
            {
                string[] mes = result.Item2.Split('|');
                if (mes[0] != "Null")
                {
                    for (int i = 0; i < mes.Count() - 1; i++)
                    {
                        lt_message.Add(mes[i]);
                    }
                }

                isSuccess = true;
                rlir = new RidersLocationInsertionResponse { message = lt_message, isSuccess = isSuccess };

            }
            else
            {
                lt_message.Add(result.Item2);
                isSuccess = false;
                rlir = new RidersLocationInsertionResponse { message = lt_message, isSuccess = isSuccess };
            }
            return rlir;
        }

        /// <summary>
        ///Insert Route Tracking
        /// </summary>
        [HttpPost, ActionName("InsertRouteTracking")]
        public RidersRouteTrackingInsertionResponse InsertRouteTracking([FromBody] RidersRouteTrackingModelMultiple LocationList)
        {
            string message = "";
            bool isSuccess = false;
            RidersRouteTrackingInsertionResponse rlir = new RidersRouteTrackingInsertionResponse();
            List<string> lt_message = new List<string>();
            List<ridersRouteTrackingModel> ridersRouteList = LocationList.ridersRouteTrackingList;
            for (int i = 0; i < ridersRouteList.Count; i++)
            {
                if (ridersRouteList[i].logDateTime == null || ridersRouteList[i].longitude == 0 || ridersRouteList[i].latitude == 0)
                {
                    message = "Kindly Send Proper  logTime, longitude, latitude on object index: " + (i + 1).ToString() + "";
                    lt_message.Add(message);
                    isSuccess = false;
                    rlir = new RidersRouteTrackingInsertionResponse { message = lt_message, isSuccess = isSuccess };
                    return rlir;
                }
            }
            DataTable dt_child = new DataTable();
            dt_child.Columns.AddRange(new DataColumn[] {
            new DataColumn("deviceName", typeof(string)),
            new DataColumn("logDateTime", typeof(DateTime)),
            new DataColumn("latitude", typeof(Decimal)),
            new DataColumn("longitude", typeof(Decimal)),
            new DataColumn("isStart", typeof(bool)),
            new DataColumn("isReach", typeof(bool)),
            new DataColumn("isEnd", typeof(bool)),
            new DataColumn("isPickup", typeof(bool))
            });

            for (int i = 0; i < ridersRouteList.Count; i++)
            {
                DataRow dr = dt_child.NewRow();
                dr[0] = ridersRouteList[i].deviceName;
                dr[1] = ridersRouteList[i].logDateTime;
                dr[2] = Convert.ToDecimal(ridersRouteList[i].latitude);
                dr[3] = Convert.ToDecimal(ridersRouteList[i].longitude);
                dr[4] = ridersRouteList[i].isStart;
                dr[5] = ridersRouteList[i].isReach;
                dr[6] = ridersRouteList[i].isEnd;
                dr[7] = ridersRouteList[i].isPickup;
                dt_child.Rows.Add(dr);
                dt_child.AcceptChanges();
            }

            Tuple<bool, string> result = Rider_LocationDB.InsertRouteTracking(dt_child);
            if (result.Item1)
            {
                string[] mes = result.Item2.Split('|');
                if (mes[0] != "Null")
                {
                    for (int i = 0; i < mes.Count() - 1; i++)
                    {
                        lt_message.Add(mes[i]);
                    }
                }

                isSuccess = true;
                rlir = new RidersRouteTrackingInsertionResponse { message = lt_message, isSuccess = isSuccess };

            }
            else
            {
                lt_message.Add(result.Item2);
                isSuccess = false;
                rlir = new RidersRouteTrackingInsertionResponse { message = lt_message, isSuccess = isSuccess };
            }
            return rlir;
        }

        /// <summary>
        /// Get Rider By Supervisor
        /// </summary>
        [HttpGet, ActionName("GetRiderBySupervisor")]
        public RiderDataBySupervisorResponse GetRiderBySupervisor(int SupervisorID)
        {
            var riderDataList = new RiderDataList();
            var riderData_list = new List<RiderDataList>();
            var RiderDataResponse = new RiderDataBySupervisorResponse();
            if (SupervisorID == 0)
            {
                RiderDataResponse = new RiderDataBySupervisorResponse
                {
                    isSuccess = false,
                    message = "Kindly send SupervisorID",
                    riderList = riderData_list
                };

                return RiderDataResponse;
            }
            DataTable dt = Rider_LocationDB.GetRiderData(SupervisorID);

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    riderDataList = new RiderDataList
                    {
                        riderID = dt.Rows[i]["riderID"].ToString(),
                        riderCode = dt.Rows[i]["riderCode"].ToString(),
                        phoneNo = dt.Rows[i]["phoneNo"].ToString(),
                        RiderName = dt.Rows[i]["RiderName"].ToString(),
                        RiderAttendance = dt.Rows[i]["RiderAttendance"].ToString()

                    };
                    riderData_list.Add(riderDataList);
                }

                RiderDataResponse = new RiderDataBySupervisorResponse
                {
                    isSuccess = true,
                    message = "",
                    riderList = riderData_list
                };
            }
            else
            {
                RiderDataResponse = new RiderDataBySupervisorResponse
                {
                    isSuccess = false,
                    message = "No Riders Exists",
                    riderList = null
                };
            }
            return RiderDataResponse;
        }

    }
}