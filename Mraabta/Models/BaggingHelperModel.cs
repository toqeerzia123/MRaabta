using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRaabta.Models
{
    public class BagModel
    {
        public bool Edit { get; set; }
        public string BagNo { get; set; }
        public int Destination { get; set; }
        public int Product { get; set; }
        public int Service { get; set; }
        public string BagType { get; set; }
        public int SHS { get; set; }
        public int TotalWeight { get; set; }
        public string SealNo { get; set; }
        public List<BagDetailModel> BagDetails { get; set; }
    }

    public class BagDetailModel
    {
        public string ManCN { get; set; }
        public string Weight { get; set; }
        public int Pcs { get; set; }
        public bool IsMan { get; set; }
        public string Remarks { get; set; }
        public string AccountNo { get; set; }
        public int DestinationId { get; set; }
    }


    public class BagPrintModel
    {
        public long Id { get; set; }
        public string BagNo { get; set; }
        public string TotalWeight { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public string SealNo { get; set; }
        public string Product { get; set; }
        public string Service { get; set; }
        public string Type { get; set; }
        public int SHS { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public IEnumerable<BagPrintManifestModel> Manifests { get; set; }
        public IEnumerable<BagPrintCNModel> CNs { get; set; }
    }

    public class BagPrintManifestModel
    {
        public string ManNo { get; set; }
        public int Pcs { get; set; }
        public string Weight { get; set; }
        public string Remarks { get; set; }
        public string Destination { get; set; }
    }

    public class BagPrintCNModel
    {
        public string CN { get; set; }
        public string Consigner { get; set; }
        public string Consignee { get; set; }
        public int Pcs { get; set; }
        public string Weight { get; set; }
        public string Remarks { get; set; }
        public string Destination { get; set; }
    }
}