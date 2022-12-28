using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRaabta.Models
{
    public class PickupTicketModel
    {

        public string ticketNumber { get; set; }
        public string consignmentNumber { get; set; }
        public string ServiceType { get; set; }
        public string CallStatus { get; set; }
        public string Reason { get; set; }
        public string rider { get; set; }
        public float Amount { get; set; }
        public float totalAmount { get; set; }
        public float gst { get; set; }
        public string discountApplied { get; set; }
        public float discountGST { get; set; }

        public float longitude { get; set; }
        public float latitude { get; set; }

        public string consignee { get; set; }
        public string consigneeAddress { get; set; }
        public string consigneeCellNo { get; set; }
        public string consigner { get; set; }
        public string consigneraddress { get; set; }
        public string consignerCellNo { get; set; }
        public string Status { get; set; }
        public string origin { get; set; }
        public string destination { get; set; }
        public string Weight { get; set; }
        public string PickupTime { get; set; }
        public string PickupDate { get; set; }
        public string pieces { get; set; }
    }
}