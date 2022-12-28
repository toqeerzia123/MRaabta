using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRaabta.Models
{
    public class UserModel
    {
        public string Code { get; set; }
        public int Uid { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Profile { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int Status { get; set; }
        public DateTime ActivityDate { get; set; }
        public DateTime InactiveDate { get; set; }
        public string ChangePassFlag { get; set; }
        public string UserMacAddress { get; set; }
        public string ExcelPermission { get; set; }
        public string DsgCode { get; set; }
        public string ZoneCode { get; set; }
        public int BTSUser { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public string ExpressCenter { get; set; }
        public string WorkingDate { get; set; }
        public string ExpressCenterName { get; set; }
        public int LocationId { get; set; }
        public string LocationName { get; set; }
    }
}