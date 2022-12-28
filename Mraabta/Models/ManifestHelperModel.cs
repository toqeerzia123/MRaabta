using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRaabta.Models
{
    public class ManifestModel
    {
        public string ManifestNo { get; set; }
        public int Destination { get; set; }
        public string Service { get; set; }
        public double TotalWeight { get; set; }
        public int TotalPcs { get; set; }
        public string Product { get; set; }
        public int SHS { get; set; }
        public bool IsEdit { get; set; }
        public List<ManifestDetailModel> Details { get; set; }
    }

    public class ManifestDetailModel
    {
        public string CN { get; set; }
        public int StatusCode { get; set; }
        public string Remarks { get; set; }
        public string Weight { get; set; }
        public string Pcs { get; set; }
        public bool IsOld { get; set; }
    }

    public class ManifestDataModel
    {
        public string ManifestNo { get; set; }
        public int ServiceTypeId { get; set; }
        public string ServiceType { get; set; }
        public int ProductId { get; set; }
        public string Product { get; set; }
        public int SHS { get; set; }
        public string Origin { get; set; }
        public string DestinationId { get; set; }
        public string Destination { get; set; }
        public string TotalWeight { get; set; }
        public int TotalPcs { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Date { get; set; }
        public string DemanifestDate { get; set; }
        public bool IsDemanifested { get; set; }
        public List<ManifestDataDetailPrintModel> ManifestDetail { get; set; }
    }

    public class ManifestDataDetailPrintModel
    {
        public string CN { get; set; }
        public string Shipper { get; set; }
        public string Consignee { get; set; }
        public string Weight { get; set; }
        public string Pcs { get; set; }
        public string Remarks { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public string ServiceType { get; set; }
        public string CNType { get; set; }
        public int Status { get; set; }
    }
}
