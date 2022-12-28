using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRaabta.Models
{
    public class TaxForwarderModel
    {
        public string CPRNo { get; set; }
        public float PaidAmount { get; set; }
        public string Customer { get; set; }
        public string InvoiceNumber { get; set; }
        public string INV { get; set; }
        public float Type { get; set; }
        public float Source { get; set; }
        public double ReceptNo { get; set; }
    }
}