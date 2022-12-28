using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRaabta.Models
{
    public class ComplainModel
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Zone { get; set; }
        public List<DropDownModel> Zones { get; set; }
        public string Branch { get; set; }
        public List<DropDownModel> Branches { get; set; }
        public int Weight { get; set; }
        public string Consignment { get; set; }
        public int Escalation { get; set; }
        public int Status { get; set; }        
        public string ConsignmentNo { get; set; }
        public int OPSStatus { get; set; }

    }
    public class ComplainDisplayModel
    {
        public string TicketID { get; set; }
        public string ConsignmentNum { get; set; }
        public string RequestNature { get; set; }
        public string RequestType { get; set; }
        public string Description { get; set; }
        public string RequestStatus { get; set; }
        public string LaunchBy { get; set; }
        public string LaunchDate { get; set; }
        public string Zone { get; set; }
        public string Branch { get; set; }
        public int EscLevel { get; set; }
        public string ColorStatus { get; set; }
        public string AssignedTo { get; set; }        
    }
}
