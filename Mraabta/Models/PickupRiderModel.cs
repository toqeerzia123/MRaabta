using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRaabta.Models
{
    public class PickupRiderModel
    {
    }


    public class Response_PickupRiderNotification
    {
        public bool isSuccess { get; set; }
        public String message { get; set; }
    }

    public class PickupRiderNotification
    {
        public string TicketNumber { get; set; }
        public string CallStatus { get; set; }
        public string U_ID { get; set; }  
    }
}