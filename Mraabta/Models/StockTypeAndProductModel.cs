using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRaabta.Models
{
    public class StockTypeModel
    {
        public string Type { get; set; }
        public string Detail { get; set; }

    }
    public class StockProductModel
    { 
        public string ProductName { get; set; }
        public string ProductId { get; set; }
        public string OracleCode { get; set; }
        public string TypeName { get; set; }
        public string TypeId { get; set; }
        public string Unit { get; set; }
        public string isSerialised { get; set; }
    }
}