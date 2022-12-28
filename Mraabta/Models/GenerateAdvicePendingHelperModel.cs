using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRaabta.Models
{
    public enum UserType
    {
        None = 0,
        CXM = 1,
        Coordinator = 2,
        Project = 3
    }
    public class TicketModel
    {
        public string CN { get; set; }
        public string TicketNo { get; set; }
        public int Reason { get; set; }
        public int Note { get; set; }
        public int PhoneStatus { get; set; }
        public TimeSpan CallingTime { get; set; }
        public string Consignee { get; set; }
        public string ConsigneeCell { get; set; }
        public string ConsigneeAddress { get; set; }
        public int Status { get; set; }
        public int Reattempt { get; set; }
        public string Remarks { get; set; }
        public string Shippper { get; set; }
        public string AccountNo { get; set; }
        public int Origin { get; set; }
        public int Destination { get; set; }
        public UserType UserType { get; set; }
    }
}