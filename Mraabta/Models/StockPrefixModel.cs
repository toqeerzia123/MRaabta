using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRaabta.Models
{

    public class StockPrefixModel
    {
        public int PrefixId { get; set; }
        public string ProductName { get; set; }
        public string ProductId { get; set; }
        public string TypeName { get; set; }
        public int PrefixLength{ get; set; }
        public String ZoneCode { get; set; }
        public String CreatedBy { get; set; }
        public string ZoneName { get; set; }
       

    }
    public class StockPrefixList
    {
        public List<StockPrefixModel> PrefixDetailList { get; set; }

    }


}