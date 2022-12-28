using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRaabta.Models
{
    public class RecoveryLetterModel
    {
        public string ClientID { get; set; }
        public string ClientAccount { get; set; }
        public string ClientName { get; set; }
        public string PhoneNo { get; set; }
        public string Invoice { get; set; }
        public decimal InvoiceAmount { get; set; }
        public decimal Outstanding { get; set; }
        public string TargetBucket { get; set; }
        public int TotalDays { get; set; }
    }

    public class RecoveryLetterDetailModel
    {
        public string ClientID { get; set; }
        public string ClientAccount { get; set; }
        public string ClientName { get; set; }
        public string PhoneNo { get; set; }
        public string Invoice { get; set; }
        public decimal InvoiceAmount { get; set; }
        public decimal Outstanding { get; set; }
        public string ClientAddress { get; set; }
        public string InvoiceMonth { get; set; }
        public string InvoiceZone { get; set; }
        public string AccountNumber { get; set; }
        public string InvoiceBranch { get; set; }
        public int TotalDays { get; set; }
    }
}