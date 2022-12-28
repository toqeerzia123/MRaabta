using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRaabta.Models
{
    public class RetailCODCRFNewModel
    {
        public string Zone { get; set; }
        public string Branch { get; set; }
        public string ECName { get; set; }
        public string RSEName { get; set; }
        public string CustomerName { get; set; }
        public string AccountNumber { get; set; }
        public string Address { get; set; }
        public string IBFT { get; set; }
        public string BankBranchName { get; set; }
        public long BankId { get; set; }
        public string BankContactNo { get; set; }
        public string BankBranchCode { get; set; }
        public string CNIC { get; set; }
        public string Email { get; set; }
        public string AccountTitle { get; set; }
        public DateTime DateOfOpening { get; set; }
        public DateTime DateOfApproval { get; set; }
        public bool isApproved { get; set; }
    }
    public class ZoneModel
    {
        public string ZoneName { get; set; }
        public long zoneCode { get; set; }
    }
    public class BranchModel
    {
        public string BranchName { get; set; }
        public long branchCode { get; set; }
    }
    public class BankModel
    {
        public string BankName { get; set; }
        public long BankId { get; set; }
    }

}