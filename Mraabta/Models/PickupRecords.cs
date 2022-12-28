using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRaabta.Models
{
    public class PickupRecords
    {
        public string ticketNumber { get; set; }
        public string chargedAmount { get; set; }
        public string consigner { get; set; }
        public string Status { get; set; }
        public string scheduledtime { get; set; }
    }
}