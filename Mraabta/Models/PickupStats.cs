using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRaabta.Models
{
    public class PickupStats
    {

        public int Request { get; set; }
        public int Pending { get; set; }
        public int Process { get; set; }
        public string Cancel { get; set; }
        public string Performed { get; set; }


    }
}