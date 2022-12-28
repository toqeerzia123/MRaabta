using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MRaabta.Models.Api
{
    public class ConsginmentModel
    {
        public String ConsignmentNumber { get; set; }
        public String riderCode { get; set; }
        public String runsheet { get; set; }

        public String name { get; set; }

        public DateTime mobileSyncTime { get; set; }

        //[Range(1,12), ErrorMessage = "Phone Number should be equal to 12.")]
        //[Range(1, 12)]
        //[Range(1,2)]
        public string phone_Number { get; set; }

        public bool is_nic { get; set; }

        public bool is_self { get; set; }

        public bool is_cod { get; set; }

        public float cod_amount { get; set; }

        public int  createdBy{ get; set; }        
        public DateTime performedOn{ get; set; }        

        public int? reasonId { get; set; }
        public string reason { get; set; }

        public int? relationId { get; set; }
        public string relation { get; set; }

        public string pickerName { get; set; }

        public double longitude { get; set; }

        public double latitude { get; set; }

        public Int64 deilvery_id { get; set; }

        public string nic_number { get; set; }

        public int isMobilePerformed { get; set; }

        public string pickerPhone_No { get; set; }
        
        public string rider_comments { get; set; }

        public string rider_iemi { get; set; }

        public string rider_amount_entered { get; set; }
        public int statusId { get; set; }
        public int battery { get; set; }

    }
    public class ConsginmentList{
        public List<ConsginmentModel> listOfconsignments { get; set; }
}
    public class PickupInsertion_Image
    {
        public List<Pickup_Image> Pickup_Child_List { get; set; }
    }

    public class PickupInsertionResponse
    {
        public bool isSuccess { get; set; }
        public string message { get; set; }

        public int PickUp_ID { get; set; }

    }
    public class ConsignmentNumberImage {
        public Pickup_Image image { get; set; }
    }

    public class Pickup_Image
    {
        public string TypeOfImage{ get; set; }
        public string consignmentNumber { get; set; }
        public string imageCode { get; set; }
    }
    


    public class PickupInsertionUrlResponse
    {
        public bool isSuccess { get; set; }
        public string message { get; set; }

        public List<Images> ListImages { get; set; }
    }
    public class Images {

        public bool isSuccess { get; set; }
        public String Message { get; set; }
        public string   ConsignmentNumber{ get; set; }
      
    }


    public class ConsignmentStatus
    {
        public bool isSuccess { get; set; }
        public string Message { get; set; }
        public string consignmentNumber { get; set; }

    }

    public class ConsginmentStatusResponse
    {
        public bool isSuccess { get; set; }
        public string Message { get; set; }
        public List<ConsignmentStatus> listConsignment { get; set; }
    }

    public class ImageDaata
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ImageID { get; set; }
        public string Name { get; set; }
        public byte[] ImageData { get; set; }
    }

    //public class ImageContext : DbContext
    //{
    //    public DbSet<Image> images { get; set; }
    //}

}