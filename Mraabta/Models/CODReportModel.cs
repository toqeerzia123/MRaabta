using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRaabta.Models
{
    public class CODPickupReportModel
    {
        public bool status { get; set; }
        public string Message { get; set; }
        public List<CODPickupResponse> CODTable { get; set; }
    }
    public class CODPickupSearch
    {
        public string startDate { get; set; }
        public string endDate { get; set; }
        public string Zone { get; set; }
        public string Account { get; set; }
        public string Status { get; set; }

    }
    public class CODPickupResponse
    {
        public string serial { get; set; }
        public string consignerAccountNo { get; set; }
        public string consigner { get; set; }
        public string Zone { get; set; }
        public string branch { get; set; }
        public string LocationName { get; set; }
        public string CreatedDate { get; set; }
        public string BookedCount { get; set; }
        public string LoadsheetCount { get; set; }
        public string ArrivalCount { get; set; }
        public string ManifestCount { get; set; }
        public string BaggingCount { get; set; }
        public string LoadingCount { get; set; }
        public string bookingDate { get; set; }
        public string orderRefNo { get; set; }
        public string consignmentNumber { get; set; }
        public string consignee { get; set; }
        public string consigneePhoneNo { get; set; }
        public string ADDRESS { get; set; }
        public string weight { get; set; }
        public string pieces { get; set; }
        public string STATUS { get; set; }
        public string Arrivaldt { get; set; }
        public string deliverydate { get; set; }
        public string ShipperAddress { get; set; }
        public string CustomerPhone { get; set; }
        public string BookingDate { get; set; }
    }

}