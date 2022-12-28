using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace MRaabta.Models.Api
{
    public class RidersLocationModel
    {
        [Key]
        [DataMember(Name = "USER_ID")]
        public int USER_ID { get; set; }

        [DataMember(Name = "logTime")]
        public DateTime logTime { get; set; }
        [DataMember(Name = "latitude")]
        public double latitude { get; set; }
        [DataMember(Name = "longitude")]
        public double longitude { get; set; }

        [NotMapped]
        [DataMember(Name = "sysTime")]
        public DateTime sysTime { get; set; }

    }
    public class RidersLocationInsertionMultiple
    {
        public List<RidersLocationModel> ridersLocationList { get; set; }

    }
    public class RidersLocationInsertionResponse
    {
        public bool isSuccess { get; set; }
        public List<string> message { get; set; }
    }

    public class ridersRouteTrackingModel
    {
        [Key]
        [DataMember(Name = "deviceName")]
        public string deviceName { get; set; }

        [DataMember(Name = "logDateTime")]
        public DateTime logDateTime { get; set; }
        [DataMember(Name = "latitude")]
        public double latitude { get; set; }
        [DataMember(Name = "longitude")]
        public double longitude { get; set; }

        [DataMember(Name = "isStart")]
        public bool isStart { get; set; }

        [DataMember(Name = "isReach")]
        public bool isReach { get; set; }
        [DataMember(Name = "isEnd")]
        public bool isEnd { get; set; }
        [DataMember(Name = "isPickup")]
        public bool isPickup { get; set; }
        [NotMapped]
        [DataMember(Name = "rtID")]
        public int rtID { get; set; }

    }

    public class RidersRouteTrackingModelMultiple
    {
        public List<ridersRouteTrackingModel> ridersRouteTrackingList { get; set; }

    }
    public class RidersRouteTrackingInsertionResponse
    {
        public bool isSuccess { get; set; }
        public List<string> message { get; set; }
    }

    public class RiderDataList
    {
        public string riderID { get; set; }
        public string riderCode { get; set; }
        public string phoneNo { get; set; }
        public string RiderName { get; set; }
        public string RiderAttendance { get; set; }

    }
    public class RiderDataBySupervisorResponse
    {
        public bool isSuccess { get; set; }
        public string message { get; set; }
        public List<RiderDataList> riderList { get; set; }
    }

}