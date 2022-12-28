using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRaabta.Models
{
    public class ArrivalModel
    {
        public int Id { get; set; }
        public string BranchCode { get; set; }
        public string OriginExpressCenterCode { get; set; }
        public string RiderCode { get; set; }
        public string Weight { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public string ZoneCode { get; set; }
        public string ExpressCenterCode { get; set; }
        public string ServiceType { get; set; }
        public int CNType { get; set; }
        public string LocationName { get; set; }
        public List<ArrivalDetailModel> ArrivalDetails { get; set; }
    }
}