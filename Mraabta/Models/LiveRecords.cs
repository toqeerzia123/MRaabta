using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MRaabta.Models
{
    public class LiveRecords
    {
        public string ConsignmentNumber { get; set; }
        public string userName { get; set; }
        //public string Date { get; set; }
        public string Time { get; set; }
        public DateTime PerformedOn { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }

    }
}