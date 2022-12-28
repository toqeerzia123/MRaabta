using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRaabta.Models
{
    public class RidersActivity
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string RiderCode { get; set; }
        public bool IsActive { get; set; }
        public string Password { get; set; }
        public int Battery { get; set; }
        public bool Status { get; set; }
    }
}