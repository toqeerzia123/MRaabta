using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRaabta.Models
{
    public class CreditNotesModel
    {
        public string invoiceNumber { get; set; }
        public string type { get; set; }
        public string creditnoteid { get; set; }
        public string creditclientid { get; set; }
        public string accountNo { get; set; }
        public string branch { get; set; }
        public string customername { get; set; }
        public decimal totalAmount { get; set; }
        public long paymenttypeid { get; set; }
        public string VoucherDate { get; set; }
        public DateTime invoiceDate { get; set; }
        public string createdBy { get; set; }
        public DateTime createdOn { get; set; }
        public int isCOD { get; set; }
    }

    public class PaymentTypesModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }

    public class ExcelErrorModel
    {
        public string Invoice { get; set; }
        public string Error { get; set; }
    }
}