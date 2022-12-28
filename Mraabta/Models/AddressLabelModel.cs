using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRaabta.Models
{
    public class AddressLabelModel
    {
        public string ConsignmentNumber { get; set; }
        public string consignee { get; set; }
        public string consigner { get; set; }
        public long Pieces { get; set; }
        public long Weight { get; set; }
        public long totalamount { get; set; }
        public string orderrefno { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Destination { get; set; }
    }
}
