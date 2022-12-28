using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRaabta.Models
{
    public class PickupDashboard
    {

            // GET: PointsDashboard
            public int ActiveUsers { get; set; }
            public int RegisteredUsers { get; set; }
            public int TotalPending { get; set; }
            public int TotalRequest { get; set; }
            public int TodayRequest { get; set; }
            public int TodayPending { get; set; }
            public int TodayProcess { get; set; }

            public int TodayCancel { get; set; }
            public int TodayPerformed { get; set; }
            public int Request { get; set; }
            public int Pending { get; set; }
            public int Process { get; set; }
            public int Cancel { get; set; }
            public int Booked { get; set; }

            public int RoutecodeCount { get; set; }

            public int TotalRunsheet { get; set; }
            public int TotalBookings { get; set; }
            public int TotalCancelled { get; set; }
            public int DownloadedRunsheet { get; set; }   
    }
}