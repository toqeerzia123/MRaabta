using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRaabta.Models
{
    public class RiderReportModel
    {
        public Int64 DownloadedRunsheet { get; set; }
        public int TotalRunsheet { get; set; }
        public string RiderName { get; set; }

        public int riderCode { get; set; }
        public string Location { get; set; }
        public string RiderRoute { get; set; }
        public int TotalCN { get; set; }
        public int TCNDownloaded { get; set; }
        public int delivered { get; set; }
        public int deliveredRts { get; set; }
        public int undelivered { get; set; }
        public string Route { get; set; }
        public string TotalTimeTaken { get; set; }
        public int Touchpoints { get; set; }
    }
}