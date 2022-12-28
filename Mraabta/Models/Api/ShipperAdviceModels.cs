using System;

namespace MRaabta.Models.Api
{

    public class RequestModel
    {
        public int Type { get; set; }
        public string AccountNo { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public string CN { get; set; }
    }
    public class ShipperAdviceDataModel
    {
        public string CN { get; set; }
        public string BookingDate { get; set; }
        public string TicketNo { get; set; }
        public string TicketDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public string DestinationBranch { get; set; }
        public string PendingReason { get; set; }
        public string StandardNote { get; set; }
        public string CallStatus { get; set; }
        public string AdditionalRemarks { get; set; }
        public string KPI { get; set; }
        public string OrderRefNo { get; set; }
        public string Comments { get; set; }
        public string Consignee { get; set; }
        public string ConsigneeContact { get; set; }
        public string ConsigneeAddress { get; set; }
        public int TempAdvice { get; set; }
        public int Advice { get; set; }
        public int ReattemptReason { get; set; }
        public int CreatedBy { get; set; }
        public string CallTrackName { get; set; }
        public double CodAmount { get; set; }
        public bool ShowDetail { get; set; }
    }
}