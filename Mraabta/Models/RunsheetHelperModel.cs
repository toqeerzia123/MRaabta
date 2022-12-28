using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRaabta.Models
{
    public class RunsheetModel
    {
        public DateTime Date { get; set; }
        public int Type { get; set; }
        public string RouteCode { get; set; }
        public string RiderCode { get; set; }
        public string Rider { get; set; }
        public string Landmark { get; set; }
        public int VehicleType { get; set; }
        public int VehicleId { get; set; }
        public string VehicleNo { get; set; }
        public string MeterStart { get; set; }
        public string MeterEnd { get; set; }
        public string ZoneCode { get; set; }
        public string BranchCode { get; set; }
        public string ECCode { get; set; }
        public int CreatedBy { get; set; }
        public string Location { get; set; }
        public bool IsChief { get; set; }
        public List<RunsheetDetailModel> Details { get; set; }
    }

    public class RunsheetDetailModel
    {
        public string CN { get; set; }
        public int DestinationId { get; set; }
        public bool IsCod { get; set; }
        public int Sort { get; set; }
    }
}