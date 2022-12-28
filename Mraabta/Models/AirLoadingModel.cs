using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRaabta.Models
{
    public class AirLoadingModel
    {
        public long Id { get; set; }
        public int RouteId { get; set; }
        public string Route { get; set; }
        public int Origin { get; set; }
        public int Touchpoint { get; set; }
        public int Destination { get; set; }
        public int TransportType { get; set; }
        public int VehicleType { get; set; }
        public int VehicleId { get; set; }
        public bool IsRented { get; set; }
        public string VehicleRegNo { get; set; }
        public string Description { get; set; }
        public string CourierName { get; set; }
        public string LoadingSealNo { get; set; }
        public string ECCode { get; set; }
        public int BranchCode { get; set; }
        public int ZoneCode { get; set; }
        public int CreatedBy { get; set; }
        public int LocationId { get; set; }
        public string Location { get; set; }
        public List<UnloadingDetail> UnloadingDetail { get; set; }

    }
}