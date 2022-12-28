using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRaabta.Models
{
    public class WeightModel
    {
        public string From { get; set; }
        public string Zone { get; set; }
        public string Branch { get; set; }
        public double Weight { get; set; }
        public string BookingCode { get; set; }

    }

    public class WeightDisplayModel
    {
        public string CN { get; set; }
        public string consignmentNumber { get; set; }
        public string BOOKINGDATE { get; set; }
        public string orignZone { get; set; }
        public string orignBranch { get; set; }
        public string destinationZone { get; set; }
        public string destinationBranch { get; set; }
        public string serviceTypeName { get; set; }
        public string clientname { get; set; }
        public string accountNo { get; set; }
        public int CNpieces { get; set; }
        public string Ops_Pieces { get; set; }
        public string CNWeight { get; set; }
        public string Ops_weight { get; set; }
        public string Weight_diff { get; set; }
        public string ops { get; set; }
        public string Ops_Number { get; set; }
        public string LOCATION { get; set; }
        public string isApproved { get; set; }
        public string isPriceComputed { get; set; }
        public string IsInvoiced { get; set; }
        public string CODStatus { get; set; }
    }
}