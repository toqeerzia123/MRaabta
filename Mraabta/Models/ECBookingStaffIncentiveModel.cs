using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRaabta.Models
{
    public class ECBookingStaffIncentiveModel
    {
        public string AccountReceivingDate {get;set;}
        public string Month { get; set; }
        public string Year { get; set; }
        public long Type { get; set; }
    }
    public class ECBookingStaffIncentiveReportModel
    {
        public DateTime AccountReceivingDate { get; set; }
        public string Zone { get; set; }
        public string Branch { get; set; }
        public string Rider_Code { get; set; }
        public long CNCount { get; set; }
        public long Domestic { get; set; }
        public long International { get; set; }
        public long FedEx { get; set; }
        public long MyAirCargo { get; set; }
        public long RoadNRail { get;set;}
        public long TotalIncentive { get;set;}
        public string RiderCode { get; set; }
}
    public class ECBookingStaffIncentiveDetailReportModel
    {
        public string CN { get; set; }
        public string BookingDate { get; set; }
        public string ServiceTypeName { get; set; }
        public string Zone { get; set; }
        public string Branch { get; set; }
        public string ECName { get; set; }
        public string EC_Code { get;set;}
        public string RiderCode { get;set;}
        public string RiderName { get;set;}
        public string SeparationType { get;set;}
        public string DateOfLeaving { get;set;}
        public string UserTypeID { get;set;}
        public decimal Weight { get;set;}
        public long IncentiveRate { get;set;}
        public long TotalIncentive { get;set;}
}
}