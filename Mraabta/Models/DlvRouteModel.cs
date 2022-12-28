using System;
using System.Collections.Generic;

namespace MRaabta.Models
{
    public class DlvRouteModel
    {
        public int Id { get; set; }
        public int RiderId { get; set; }
        public decimal Lat { get; set; }
        public decimal Long { get; set; }
        public bool IsMarker { get; set; }
        public string Details { get; set; }
        public DateTime CreationDate { get; set; }
        public string name { get; set; }
        public String ConsignmentNumber { get; set; }

        public String Reason { get; set; }

        public string pickupDetail { get; set; }

    }


    public class DLVRoute_LogTrackingModel
    {
        public String id { get; set; }
        public String USER_ID { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public DateTime logTime { get; set; }

    }

    public class DLVRoute_ListOfTrackingNConsignmentDrop
    {
        public List<DlvRouteModel> consignment { get; set; }
        public List<DLVRoute_LogTrackingModel> tracking { get; set; }
    }

}