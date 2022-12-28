using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRaabta.Models
{
    public class RiderRouteModel
    {
        public int Id { get; set; }
       // public int RiderId { get; set; }
        public decimal Lat { get; set; }
        public decimal Long { get; set; }
        public bool IsMarker { get; set; }
        public string Details { get; set; }
        public DateTime CreationDate { get; set; }
        public string name { get; set; }
        public string userName { get; set; }
        public DateTime Date { get; set; }
        public string locationName { get; set; }
        public string pickupDetail { get; set; }

        public string Runsheet { get; set; }

        public string Reason { get; set; }

        public string ConsignmentNumber { get; set; }

        public string cod_amount { get; set; }


    }

    public class LogTrackingModel
    {
        public String id { get; set; }
        public String USER_ID { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public DateTime logTime { get; set; }

    }

    public class ListOfTrackingNConsignmentDrop
    {
        public List<RiderRouteModel> consignment { get; set; }
        public List<LogTrackingModel> tracking { get; set; }
    }

}