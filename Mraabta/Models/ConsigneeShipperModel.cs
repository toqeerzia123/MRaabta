using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MRaabta.Models
{
    public class ConsigneeShipperModel
    {
        //Shipper

        [Required]
        public int PickUpId { get; set; }
        public string CNNumber { get; set; }
        public string ShipperName { get; set; }
        public string ShipperContact { get; set; }
        public string ShipperAddress { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsEntered { get; set; }
        public bool IsFaulty { get; set; }
        //Conginee
        public string ConsigneeName { get; set; }
        public string ConsigneeContact { get; set; }
        public string ConsigneeAddress { get; set; }
        public DateTime EnteredOn { get; set; }
        public int EnteredBy { get; set; }
        public string Town { get; set; }
        public string City { get; set; }
        public string FeedBy { get; set; }
        public DateTime FeedOn { get; set; }
    }
}