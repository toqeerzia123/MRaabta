using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRaabta.Models
{
    public class PickupModel
    {
        public int PickUp_ID { get; set; }
        public string riderCode { get; set; }
        public int locationID { get; set; }
        public int createdBy { get; set; }
        public DateTime createdOn { get; set; }
        public int status { get; set; }
        public int isCOD { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
    }

    public class Pickup_Child_Model
    {
        public int pickUpChild_ID { get; set; }
        public int pickUp_ID { get; set; }
        public string consignmentNumber { get; set; }
        public double weight { get; set; }
        public DateTime pickUpDateTime { get; set; }
        public string CN_picURL { get; set; }
        public int pieces { get; set; }
        public int status { get; set; }
    }

    public class Pickup_Child_details
    {
        public string consignmentNumber { get; set; }
        public double weight { get; set; }
        public int pieces { get; set; }
        public string imageURL { get; set; }

    }

    public class GetPickupData
    {
        public int PickUp_ID { get; set; }
        public string ClientName { get; set; }
        public int locationID { get; set; }
        public string locationName { get; set; }
        public string riderCode { get; set; }
        public string riderName { get; set; }
        public DateTime PickUpTime { get; set; }
        public string PickUpUrl { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public string pick_Date { get; set; }
        public string pick_Time { get; set; }
    }

    public class ViewModelPickUp
    {
        public List<GetPickupData> PickUpData { get; set; }
        public List<LocationModel> LocationData { get; set; }
        //public List<SelectListItem> RiderData { get; set; }
    }
}