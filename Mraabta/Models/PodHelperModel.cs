using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRaabta.Models
{
    public class Pod
    {
        public string RS { get; set; }
        public string Route { get; set; }
        public string RiderCode { get; set; }
        public string Rider { get; set; }
        public string RSDate { get; set; }
        public string Vehicle { get; set; }
        public string VehicleType { get; set; }
        public string MeterStart { get; set; }
        public string MeterEnd { get; set; }
        public IEnumerable<PodDetails> CNs { get; set; }
    }
    public class PodDetails
    {
        public string CN { get; set; }
        public string Consignee { get; set; }
        public string OriginId { get; set; }
        public string Origin { get; set; }
        public string DestinationId { get; set; }
        public string Destination { get; set; }
        public string GivenToRider { get; set; }
        public string Time { get; set; }
        public string ReceivedBy { get; set; }
        public string Reason { get; set; }
        public string ReceiverCNIC { get; set; }
        public string ReceiverRelation { get; set; }
        public string Comments { get; set; }
        //public bool IsPOD { get; set; }
        public bool IsBypass { get; set; }
        public bool Update { get; set; }
        public bool ReadOnly { get; set; }
    }
}