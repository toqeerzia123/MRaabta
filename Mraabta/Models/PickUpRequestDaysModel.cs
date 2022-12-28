using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRaabta.Models
{
    public class PickUpRequestDaysModel
    {
        public int Id { get; set; }
        public int PickupRequestId { get; set; }
        public string Day { get; set; }
        public TimeSpan Time { get; set; }
    }
}