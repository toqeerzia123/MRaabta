using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRaabta.Models
{
    public class DebreifingAllModel
    {
        public int ConsignId { get; set; }
        public string Comments { get; set; }
        public int SortOrder { get; set; }
        public string CN { get; set; }
        public string Remarks { get; set; }
        public int Pcs { get; set; }
        public double Weight { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public string ConsigneeName { get; set; }
        public string ConsigneePhone { get; set; }
        public string ConsigneeAddress { get; set; }
        public decimal CodAmount { get; set; }
        public string RiderComments { get; set; }
        public string PickerPhoneNo { get; set; }
        public string Imei { get; set; }
        public string RiderCode { get; set; }
        public string RiderFName { get; set; }
        public string RiderLName { get; set; }
        public string Receiver { get; set; }
        public string Relation { get; set; }
        public string Reason { get; set; }
        public int IsPending { get; set; }
        public int IsDelivered { get; set; }
        public string NicNo { get; set; }
        public DateTime? PerformedOn { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }
        public string RunsheetNo { get; set; }
        public bool Verify { get; set; }
        public DateTime RunsheetDate { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public string DBUserName { get; set; }
        public DateTime? DBOn { get; set; }
        public string DBOnStr { get; set; }
    }
}