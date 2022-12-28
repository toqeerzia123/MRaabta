using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MRaabta.Models
{
    public class MonthlyStats
    {
        public int delivered { get; set; }
        public int undelivered { get; set; }

        public int deliveredrts { get; set; }
        public int TCNDownloaded { get; set; }
        public int DownloadedRunsheet { get; set; }

        public string runsheetDate { get; set; }

        public string TotalTimeTaken { get; set; }

        public int Touchpoints { get; set; }
    }

}