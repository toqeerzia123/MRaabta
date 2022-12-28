using System;
using System.Collections.Generic;

namespace MRaabta.Models
{
    public class DeliveryModel
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

    public class Delivery_Child_Model
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

    public class GetDeliveryChildDataDistance
    {
        public List<Delivery_Child_details> list = new List<Delivery_Child_details>();
        public string distanceKm { get; set; }
    }
    public class Delivery_Child_details
    {
        public Int64 Consign_id { get; set; }
        public int SortOrder { get; set; }
        public string consignmentNumber { get; set; }
        public string OriginCity { get; set; }
        public double weight { get; set; }
        public int pieces { get; set; }
        public string Destination { get; set; }
        public string ConsigneeName { get; set; }
        public string codAmount { get; set; }
        public string Rider { get; set; }
        public string Receiver { get; set; }
        public string Relation { get; set; }
        public string Reason { get; set; }
        public bool isPending { get; set; }
        public bool isDelivered { get; set; }
        public string nic_number { get; set; }
        public String DeliveredDate { get; set; }
        public String DeliveredTime { get; set; }
        public bool verify { get; set; }
        public string comments { get; set; }
        public String imei { get; set; }
        public string contactNumber { get; set; }
        public string rider_comments { get; set; }

        public double longitude { get; set; }
        public double latitude { get; set; }
        public double DistancePerRecord { get; set; }
    }
    public class  GetDeliveryDataWithDistance {
       public List<GetDeliveryData> list = new List<GetDeliveryData>();
        public string distanceKm { get; set; }
    }
    public class GetDeliveryData
    {
        public Int64 RunsheetNumber { get; set; }
        public string Sname { get; set; }
        public string Name { get; set; }
        public string ROUTE_MASTER { get; set; }
        public string COURIER { get; set; }
        public String RunsheetDate { get; set; }
        public string RIDERCODE { get; set; }
        public int TotalCN { get; set; }
        public int PODCN { get; set; }
        public int DLVCN { get; set; }
        public String RUNSHEETTIME { get; set; }

        public string Touchpoints { get; set; }

        public string TotalTimeTaken { get; set; }
    }

    public class ViewModelDelivery
    {
        public List<GetPickupData> PickUpData { get; set; }
        public List<LocationModel> LocationData { get; set; }
        //public List<SelectListItem> RiderData { get; set; }
    }


    public class LatLongModel
    {
        public double Lat { get; set; }
        public double Long { get; set; }
        public DateTime CreatedOn { get; set; }
    }

    public class TimeTakenModel
    {
        public string RiderCode { get; set; }
        public DateTime Date { get; set; }
        public DateTime MinDate { get; set; }
        public DateTime MaxDate { get; set; }
    }

    public class CoordinatesDataM {
        public DateTime createdOn { get; set; }
        public string RunSheetNumber { get; set; }
        public double longitude { get; set; }
        public double latitude { get; set; }
    }
}