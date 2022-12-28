using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRaabta.Models
{
    public class RiderPerformanceReportModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<RiderPerformanceDetailsModel> DataList { get; set; }
    }

    public class RiderPerformanceDetailsModel
    {
        public string Sno { get; set; }
        public string RiderName { get; set; }
        public string RiderCode { get; set; }
        public DateTime First_RS_Download { get; set; }
        public string First_CN_Performed { get; set; }
        public string Last_CN_Performed { get; set; }
        public string First_RS_DownloadString { get; set; }
        public int TotalCNs { get; set; }
        public int CNDelivered { get; set; }
        public int CNUnDelivered { get; set; }
        public int CN_RTS { get; set; }
        public string TotalTimeOnRoute { get; set; }
        public string RS { get; set; }
        public string CN { get; set; }
        public int StatusId { get; set; }
        public DateTime? PerformedOn { get; set; }

        
    }
}