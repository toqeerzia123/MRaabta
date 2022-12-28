using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRaabta.Models
{
    public class DebrieferCount
    {
        public int TCNDownloaded { get; set; }
        public int VerifiedCount { get; set; }
        public int CommentsCount { get; set; }
        public int Attempted { get; set; }
        public int UnAttempted { get; set; }
        public int TotalDelivered { get; set; }
        public int TotalDeliveredRts { get; set; }
        public int TotalUndelivered { get; set; }
        public string MostOccuringReason { get; set; }
        public int MostOccuringTime { get; set; } 
    }
}