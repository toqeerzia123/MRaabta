using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRaabta.Models
{
    public class RiderFormModel
    {
        public int? UserId { get; set; }
        public string RiderName { get; set; }
        public string RiderCode { get; set; }
        public string Password { get; set; }
        public string CPassword { get; set; }
        public string Imei1 { get; set; }
        public string Imei2 { get; set; }
        public string SimNO { get; set; }
        public string BranchCode { get; set; }
        public long HubId { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public int CreatedBy { get; set; }
    }
}