using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRaabta.Models
{
    public class PrintArrivalModel
    {
        public string ArrivalId { get; set; }
        public string RiderCode { get; set; }
        public string RiderName { get; set; }
        public DateTime ArrivaDate { get; set; }
        public string CN { get; set; }
        public string ServiceType { get; set; }
        public double Weight { get; set; }
        public int Pieces { get; set; }
    }
}