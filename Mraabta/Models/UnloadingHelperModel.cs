using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRaabta.Models
{
    public class LoadingInfo
    {
        public long LoadingNo { get; set; }
        public int RouteId { get; set; }
        public string Route { get; set; }
        public int OriginId { get; set; }
        public string Origin { get; set; }
        public int DestinationId { get; set; }
        public string Destination { get; set; }
        public int BagsCount { get; set; }
        public int CNsCount { get; set; }
        public decimal TotalWeight { get; set; }
        public string Description { get; set; }
        public string Courier { get; set; }
        public string SealNo { get; set; }
        public string Date { get; set; }
        public int TransportTypeId { get; set; }
        public string TransportType { get; set; }
        public int VehicleId { get; set; }
        public string VehicleRegNo { get; set; }
        public string FlightNo { get; set; }
        public bool IsOld { get; set; }
        public List<LoadingInfoDetails> LoadingInfoDetails { get; set; }
    }
    public class LoadingInfoDetails
    {
        public long LoadingNo { get; set; }
        public string BagCN { get; set; }
        public int OriginId { get; set; }
        public string Origin { get; set; }
        public int DestinationId { get; set; }
        public string Destination { get; set; }
        public int Pcs { get; set; }
        public string Weight { get; set; }
        public string ServiceType { get; set; }
        public string CNType { get; set; }
        public string SealNo { get; set; }
        public bool IsBag { get; set; }
        public int Status { get; set; }
        public string Remarks { get; set; }
        public bool IsOld { get; set; }
    }

    public class Unloading
    {
        public long Id { get; set; }
        public int OriginId { get; set; }
        public int DestinationId { get; set; }
        public string BranchCode { get; set; }
        public int CreatedBy { get; set; }
        public int LocationId { get; set; }
        public string Location { get; set; }
        public string ZoneCode { get; set; }
        public List<UnloadingDetail> UnloadingDetail { get; set; }
    }

    public class UnloadingDetail
    {
        public long LoadingNo { get; set; }
        public string BagCN { get; set; }
        public int OriginId { get; set; }
        public string Origin { get; set; }
        public int DestinationId { get; set; }
        public string Destination { get; set; }
        public int Pcs { get; set; }
        public decimal Weight { get; set; }
        public string SealNo { get; set; }
        public bool IsBag { get; set; }
        public int Status { get; set; }
        public string Remarks { get; set; }
        public string SortOrder { get; set; }
    }

    public class UnloadingPrint
    {
        public long Id { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime Date { get; set; }
        public List<LoadingInfo> LoadingsInfo { get; set; }
        public List<LoadingInfoDetails> LoadingInfoDetails { get; set; }
    }
}

