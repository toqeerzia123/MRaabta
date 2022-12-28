using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRaabta.Models
{
    public class PickupRequestModel
    {
        public int Id { get; set; }
        public string Customer { get; set; }
        public int LocationId { get; set; }
        public string Location { get; set; }
        public DateTime PickupDate { get; set; }
        public TimeSpan PickupTime { get; set; }
        public int UserId { get; set; }
        public int CourierId { get; set; }

        public int DefaultCourierId { get; set; }
        public string Courier { get; set; }
        public int Priority { get; set; }
        public string PriorityName { get; set; }
        public bool IsRoutine { get; set; }
        public string Comments { get; set; }
        public int Status { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string PickupType { get; set; }
        public string DateOrDays { get; set; }
        public List<PickUpRequestDaysModel> PickUpRequestDays { get; set; }
    }
}