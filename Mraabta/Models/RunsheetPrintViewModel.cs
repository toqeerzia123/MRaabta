using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRaabta.Models
{
    public class RunsheetPrintViewModel
    {
        public string RS { get; set; }
        public string CN { get; set; }
        public int Sort { get; set; }
        public DateTime RunsheetDate { get; set; }
        public string Branch { get; set; }
        public string RiderCode { get; set; }
        public string RiderName { get; set; }
        public string RouteCode { get; set; }
        public string Route { get; set; }
        public string RouteTerritoryCode { get; set; }
        public string RouteTerritory { get; set; }
        public string Consignee { get; set; }
        public string ConsigneeAddress { get; set; }
        public string ConsigneeCnicNo { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public double Weight { get; set; }
        public int Pieces { get; set; }
        public int StatusId { get; set; }
        public DateTime? PerformedOn { get; set; }
        public string Reason { get; set; }
        public string Receiver { get; set; }
        public string Relation { get; set; }
        public string MeterStart { get; set; }
        public string MeterEnd { get; set; }
        public string VehicleType { get; set; }
        public string VehicleNumber { get; set; }
        public string RSType { get; set; }
        public int? CodAmount { get; set; }
        public float? Lat { get; set; }
        public float? Long { get; set; }
        public string RiderComments { get; set; }
        public string Comments { get; set; }
        public string Phone { get; set; }
    }
}

