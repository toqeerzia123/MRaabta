using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRaabta.Models
{
    public class RiderCashRecievingModel
    {
        public int Id { get; set; }
        public string RiderCode { get; set; }
        public string RiderName { get; set; }
        public string StaffCode { get; set; }
        public string StaffName { get; set; }
        public string Amount { get; set; }
        public int Date { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedDate { get; set; }
        public string ECCode { get; set; }
        public string ECName { get; set; }
        public string CurrExpAmount { get; set; }
        public string TotalExpAmount { get; set; }
        public string ColAmount { get; set; }
        public string DiffAmount { get; set; }

        public IEnumerable<RiderCODConsignmentModel> CNLineItem { get; set; }
        public IEnumerable<RiderCashDetailModel> paymentLineItem { get; set; }
    }
    public class RiderCashDetailModel
    {
        public int Id { get; set; }
        public string SubmitTime { get; set; }
        public string ExpressCenter { get; set; }
        public string ExpectAmount { get; set; }
        public string SubmittedAmount { get; set; }
        public string ShortAmount { get; set; }
    }

    public class RiderCODConsignmentModel
    {
        public int Id { get; set; }
        public string ConsignmentNumber { get; set; }
        public string RunSheetNumber { get; set; }
        public string CreditClientID { get; set; }
        public string RiderAmount { get; set; }
        public string AmountRcv { get; set; }
        public string PrevAmountRcv { get; set; }
        public bool IsPaid { get; set; }
        public bool IsRecieved { get; set; }
    }
}