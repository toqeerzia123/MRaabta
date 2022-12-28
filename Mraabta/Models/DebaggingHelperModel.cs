using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRaabta.Models
{
    public class BagInfoModel
    {
        public string BagNo { get; set; }
        public int OriginId { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public string TotalWeight { get; set; }
        public string SealNo { get; set; }
        public string Date { get; set; }
        public IEnumerable<BagManifestInfoModel> Details { get; set; }
    }

    public class BagManifestInfoModel
    {
        public string ManCN { get; set; }
        public string Reason { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public string TotalWeight { get; set; }
        public int Pieces { get; set; }
        public bool IsMan { get; set; }
        public int Status { get; set; }
    }

    public class DebagPrintModel
    {
        public long Id { get; set; }
        public string BagNo { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public string CreatedOn { get; set; }
        public string TotalWeight { get; set; }
        public IEnumerable<DebagManPrintModel> Manifests { get; set; }
        public IEnumerable<DebagCNPrintModel> CNs { get; set; }

    }
    public class DebagManPrintModel
    {
        public string Manifest { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public string Status { get; set; }
        public string Weight { get; set; }
        public int Pcs { get; set; }
        public string Remarks { get; set; }
    }
    public class DebagCNPrintModel
    {
        public string CN { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public string Status { get; set; }
        public string Weight { get; set; }
        public int Pcs { get; set; }
        public string Remarks { get; set; }
    }
}