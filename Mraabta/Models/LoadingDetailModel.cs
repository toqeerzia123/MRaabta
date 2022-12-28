using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRaabta.Models
{
    public class LoadingDetailModel
    {
        public string BagCN { get; set; }
        public int OriginId { get; set; }
        public string OriginName { get; set; }
        public int DestinationId { get; set; }
        public string DestinationName { get; set; }
        public string SealNoPieces { get; set; }
        public double Weight { get; set; }
        public string Remarks { get; set; }
        public bool IsBag { get; set; }
        public string ServiceType { get; set; }
        public int ConsignmentTypeId { get; set; }
        public int SortOrder { get; set; }
        public bool IsOld { get; set; }
    }
}