using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MRaabta.Models
{
    public class PointsDashboard 
    {
        // GET: PointsDashboard
        public int RidersCount { get; set; }
        public int ActiveRiders { get; set; }
        public int OfflineRiders { get; set; }

        public int RoutecodeCount { get; set; }
        public int CurrentUsers { get; set; }
        public int TotalRunsheet { get; set; }

        public int TCNDownloaded { get; set; }

        public int DownloadedRunsheet { get; set; }
        public int delivered { get; set; }
        public int undelivered { get; set; }
        public int deliveredRts { get; set; }
        public int Touchpoints { get; set; }


    }
}