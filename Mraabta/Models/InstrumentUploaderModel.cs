using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRaabta.Models
{
    public class InstrumentUploaderModel
    {
        public string PaymentId { get; set; }
        public string AccountNo { get; set; }
        public string PaymentMode { get; set; }
        public string PaymentModeNo { get; set; }
        public bool isUpdated { get; set; }
    }

    public class PaymentInstrumentModel
    {
        public List<InstrumentUploaderModel> InstrumentModelList{ get; set; }
        public string FileName{ get; set; }
    }
}