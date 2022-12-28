namespace MRaabta.Models
{
    public class CNModel
    {
        public string CN { get; set; }
        public string Shipper { get; set; }
        public string Consignee { get; set; }
        public string Receiver { get; set; }
        public string ReceiverNumber { get; set; }
        public string CallStatus { get; set; }
        public bool DeliveryVerified { get; set; }
        public string CustomerRemarks { get; set; }
        public string StaffRemarks { get; set; }
        public int CreatedBy { get; set; }
    }
}