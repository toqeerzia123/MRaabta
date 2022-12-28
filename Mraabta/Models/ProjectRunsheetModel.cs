using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRaabta.Models
{
    public class ProjectRunsheetModel
    {
        public DateTime Date { get; set; }
        public string RouteCode { get; set; }
        public string RiderCode { get; set; }
        public int Remaining { get; set; }
        public int Assign { get; set; }
    }
}