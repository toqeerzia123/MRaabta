using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MRaabta.Models
{
    public class ScheduleModel
    {
        public int pickuprequestID { get; set; }
        public int locationID { get; set; }
        public DateTime pickupDate { get; set; }
        public DateTime pickuptime { get; set; }
        public int User_ID { get; set; }

        public int priority { get; set; }
        public int isRoutine { get; set; }
        public string comments { get; set; }

        public int STATUS { get; set; }
        public int createdBy { get; set; }

        public DateTime createdOn { get; set; }
        public int modifiedBy { get; set; }

        public DateTime modifiedOn { get; set; }
    }
    public class CourierTransferModel
    {
        public int oldCourierID { get; set; }
        public int newCourierID { get; set; }
        public int RB_TYPE { get; set; }
        public int status { get; set; }
        public int createddBy { get; set; }
        public DateTime createddOn { get; set; }
        [NotMapped]
        public DateTime Date { get; set; }
        public int modifiedBy { get; set; }
        public DateTime modifiedOn { get; set; }
    }

    public class CourierTransferDateModel
    {
       
        public DateTime scheduleDate { get; set; }
        public int oldCourierID { get; set; }
        public int newCourierID { get; set; }
        public int STATUS { get; set; }
        public int createdBy { get; set; }
        public DateTime createdOn { get; set; }
        public List<int> scheduleId { get; set; }
    }


}