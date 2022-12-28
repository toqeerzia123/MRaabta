using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRaabta.Models
{
    public class ArrivalDetailModel
    {
        public long ArrivalID { get; set; }
        public string ConsignmentNumber { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string Weight { get; set; }
        public string Pieces { get; set; }
        public string ServiceType { get; set; }
        public int ConsignmentType { get; set; }
        public int SortOrder { get; set; }
    }
}