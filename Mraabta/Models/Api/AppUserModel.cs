using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRaabta.Models.Api
{
    public class AppUserModel
    {
        public string RiderCode { get; set; }
        public string Pass { get; set; }
        public string NewPass { get; set; }
    }
}