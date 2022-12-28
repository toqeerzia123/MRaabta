using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRaabta.Models
{
    public class DashboardViewModel
    {
        public int DRS { get; set; }
        public int DCN { get; set; }
        public int Performed { get; set; }
        public int UnPerformed { get; set; }
        public int TouchPoints { get; set; }
        public int TotalRiders { get; set; }
        public int ActiveRiders { get; set; }
        public int PerformingRiders { get; set; }
        public List<MonthlyStatsModel> MonthlyStats { get; set; }
        public List<DashboardRiderReport> DashboardRiderReport { get; set; }
        public int Delivered { get; set; }
        public int Undelivered { get; set; }
        public int DeliveredRTS { get; set; }
    }
    public class DashboardModel
    {
        public List<RunsheetStatsModel> RunsheetStatsModels { get; set; }
        public RiderCountModel RiderCountModel { get; set; }
        public List<MonthlyStatsModel> MonthlyStatsModels { get; set; }
        public List<DashboardRiderReport> DashboardRiderReport { get; set; }
    }
    public class RunsheetStatsModel
    {
        public string RS { get; set; }
        public string CN { get; set; }
        public int StatusId { get; set; }
        public float Lat { get; set; }
        public float Long { get; set; }
        public string Receiver { get; set; }
        public string Reason { get; set; }
    }

    public class RiderCountModel
    {
        public int TotalRiders { get; set; }
        public int ActiveRiders { get; set; }
        public int PerformingRiders { get; set; }
    }

    public class MonthlyStatsModel
    {
        public int Day { get; set; }
        public int DeliveredCNs { get; set; }
    }

    public class DashboardRiderReport
    {
        public int StatusId { get; set; }
        public string CN { get; set; }
        public string RiderName { get; set; }
        public DateTime PerformedOn { get; set; }
        public string PerformedOnStr { get; set; }
    }
}