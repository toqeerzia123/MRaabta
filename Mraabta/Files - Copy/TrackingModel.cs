using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRaabta.Files
{
    internal class TrackingModel
    {
        public class result_batch_updated
        {
            public string isSuccess { get; set; }
            public string message { get; set; }
            public List<string> orderReferenceIdList { get; set; }
            public OrderStatus_New_updated[] tracking_Details { get; set; }
        }
        public class OrderStatus_New_updated
        {
            public string BookingDate { get; set; }
            public string Destination { get; set; }
            public string Origin { get; set; }
            public string Shipper { get; set; }
            public string Consignee { get; set; }
            public string ServiceType { get; set; }
            public string CNStatus { get; set; }
            public string CNStatusID { get; set; }
            public string pieces { get; set; }
            public string weight { get; set; }
            public string reason { get; set; }
            public ItemStatus_new[] Details { get; set; }
        }
        public class ItemStatus_new
        {
            public string DateTime { get; set; }
            public string Status { get; set; }
            public string Location { get; set; }
            public string Detail { get; set; }
        }
    }
}