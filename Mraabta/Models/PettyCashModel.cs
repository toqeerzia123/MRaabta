using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MRaabta.Models
{
    public class PettyCashModel
    {
        public string Status { get; set; }
        public DateTime StartDate { get; set; }
        public string EncryptedStartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string EncryptedEndDate { get; set; }
        public String[] Zone{ get; set; }
        public String[] Branch{ get; set; }
        public String BranchName{ get; set; }
     
        public String OpeningBalance{ get; set; }
        public String ClosingBalance{ get; set; }
        public String CreatedUser { get; set; }
        public String TotalCredit { get; set; }
        public String TotalDebit { get; set; }
        //public IEnumerable<SelectListItem> Branch1 { get; set; }
        public List<PettyCashData> DataList { get; set; }
    }

    public class PettyCashData{

        public string ID { get; set; }
        public string Date { get; set; }
        public string description { get; set; }
        public string narrate { get; set; }
        public string chque_no { get; set; }
        public string comp1 { get; set; }
        public string YEAR { get; set; }
        public string MONTH { get; set; }
        public string zone { get; set; }
        public string zoneCode { get; set; }
        public string EncryptedZoneCode { get; set; }
        public string branch { get; set; }
        public string EncryptedBranch { get; set; }
        public string branchCode { get; set; }
        public string EncryptedBranchCode { get; set; }
        public long cnote { get; set; }
        public long dnote { get; set; }
        public string cnoteComma{ get; set; }
        public string dnoteComma{ get; set; }
        public string Balance { get; set; }
        public string Company { get; set; }
    }

    public class PettyCashDropdown
    {
        public string Value { get; set; }
        public string Text { get; set; }
    } 
}