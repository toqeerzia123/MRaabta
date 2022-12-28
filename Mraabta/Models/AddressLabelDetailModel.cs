using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRaabta.Models
{
    public class AddressLabelDetailModel
    {
        public string Consignment { get; set; }
        public string Service { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public string Shipper { get; set; }
        public string consignee { get; set; }
        public string consigneeAddress { get; set; }
        public long pieces { get; set; }
        public decimal weight { get; set; }
        public long insuranceValue { get; set; }
        public decimal codAmount { get; set; }
        public string ProductDetail { get; set; }
        public DateTime bookingDate { get; set; }
        public string remarks { get; set; }
        public string CustomerRef { get; set; }
        public string consigneePhoneNo { get; set; }
        public string BookingTime { get; set; }
        public string locationName { get; set; }
        public string locationAddress { get; set; }
    }
}