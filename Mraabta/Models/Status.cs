using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRaabta.Models
{
    public class Status
    {
        public int delivered { get; set; }
        public int undelivered { get; set; }
        public int deliveredRts { get; set; }
        public int Pending { get; set; }
        public int TotalCN { get; set; }
        public int TotalRunsheet { get; set; }
        public int TCNDownloaded { get; set; }
        public int DownloadedRunsheet { get; set; }
        public int Touchpoints { get; set; }
        public string City { get; set; }
        public string Rider_IEMI { get; set; }
        public string Rider_CNIC_No { get; set; }
        public string Courier { get; set; }
        public string TotalTimeTaken { get; set; }
        public string Route { get; set; }
        //public String Reason { get; set; }
    }
}