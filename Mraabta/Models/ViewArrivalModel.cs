using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRaabta.Models
{
    public class ViewArrivalModel
    {
        public int Id { get; set; }
        public string RiderCode { get; set; }
        public DateTime Date { get; set; }
        public string Branch { get; set; }
        public string ExpressCenter { get; set; }
        public float Weight { get; set; }
        public int Count { get; set; }
    }
}