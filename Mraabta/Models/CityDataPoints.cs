using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRaabta.Models
{
    public class CityDataPoints
    {
        public int KarachiTotalRidersCount { get; set; }
        public int IslamabadTotalRidersCount { get; set; }
        public int RawalpindiTotalRidersCount { get; set; }
        public int LahoreTotalRidersCount { get; set; }
        public int KarachiActiveRiders { get; set; }
        public int IslamabadActiveRiders { get; set; }
        public int RawalpindiActiveRiders { get; set; }
        public int LahoreActiveRiders { get; set; }
        public int KarachiOfflineRiders { get; set; }
        public int IslamabadOfflineRiders { get; set; }
        public int RawalpindiOfflineRiders { get; set; }
        public int LahoreOfflineRiders { get; set; }
        public int KarachiTCNDownloaded { get; set; }
        public int IslamabadTCNDownloaded { get; set; }
        public int RawalpindiTCNDownloaded { get; set; }
        public int LahoreTCNDownloaded { get; set; }
        public int KarachiDownloadedRunsheet { get; set; }
        public int IslamabadDownloadedRunsheet { get; set; }
        public int RawalpindiDownloadedRunsheet { get; set; }
        public int LahoreDownloadedRunsheet { get; set; }
        public int KarachiTotalUndelivered { get; set; }
        public int IslamabadTotalUndelivered { get; set; }
        public int RawalpindiTotalUndelivered { get; set; }
        public int LahoreTotalUndelivered { get; set; }

        public int KarachiTotalDeliveredRTS { get; set; }
        public int IslamabadTotalDeliveredRTS { get; set; }
        public int RawalpindiTotalDeliveredRTS { get; set; }
        public int LahoreTotalDeliveredRTS { get; set; }


    }
}