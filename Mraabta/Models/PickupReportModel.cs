namespace MRaabta.Models
{
    public class PickupReportModel
    {

        public string value { get; set; }
        public int Amount { get; set; }
        public string ticketNumber { get; set; }
        public string consignmentNumber { get; set; }
        public string ServiceType { get; set; }
        public string rider { get; set; }
        public string Status { get; set; }
        public string origin { get; set; }
        public string destination { get; set; }
    }
}