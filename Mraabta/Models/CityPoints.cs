using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRaabta.Models
{
    public class CityPoints
    {
        public int IslamabadTotalDelivered { get; set; }
        public int KarachiTotalDelivered { get; set; }
        public int LahoreTotalDelivered { get; set; }
        public int RawalpindiTotalDelivered { get; set; }

        public int KarachiTCNDownloaded { get; set; }
        public int IslamabadTCNDownloaded { get; set; }

        public int LahoreTCNDownloaded { get; set; }
        public int RawalpindiTCNDownloaded { get; set; }

    }
}