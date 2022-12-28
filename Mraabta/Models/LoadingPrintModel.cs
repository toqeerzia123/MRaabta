using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRaabta.Models
{
    public class LoadingPrintModel
    {
        public long Id { get; set; }
        public string TransportType { get; set; }
        public string VehicleRegNo { get; set; }
        public string CourierName { get; set; }
        public string Description { get; set; }
        public string VehicleType { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public string Route { get; set; }
        public string SealNo { get; set; }
        public string IsAirport { get; set; }
        public DateTime? DepartureFlightDate { get; set; }
        public string FlightNo { get; set; }
        public DateTime Date { get; set; }
        public List<LoadingPrintBagModel> LoadingPrintBags { get; set; }
        public List<LoadingPrintCNModel> LoadingPrintCNs { get; set; }
    }

    public class LoadingPrintBagModel
    {
        public string BagNo { get; set; }
        public float BagWeight { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public string BagSeal { get; set; }
        public string Remarks { get; set; }
        public string CreatedBy { get; set; }
        public string Product { get; set; }
        public string Service { get; set; }
        public string Type { get; set; }
        public int SHS { get; set; }
    }
    public class LoadingPrintCNModel
    {
        public string CN { get; set; }
        public int Pcs { get; set; }
        public float Weight { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public string Consigner { get; set; }
        public string Remarks { get; set; }
        public string CreatedBy { get; set; }
    }
}