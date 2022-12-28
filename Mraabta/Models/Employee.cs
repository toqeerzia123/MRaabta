using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRaabta.Models
{
    public class Employee
    {
        public string id { get; set; }
        public string runsheet_number { get; set; }
        public string cn_number { get; set; }
        public string name { get; set; }

        public string phone_no { get; set; }
        public string is_nic { get; set; }
        public string is_self { get; set; }
        public string is_cod { get; set; }
        public string cod_amount { get; set; }
        public string created_by { get; set; }
        public DateTime created_on { get; set; }
        public string status { get; set; }
        public string reason { get; set; }
        public string relation { get; set; }
        public string picker_name { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }

        public string user_id { get; set; }
        public string is_transfer_data { get; set; }

        public string is_transfer_image { get; set; }
        public string delivery_id { get; set; }
        public string nic_cumber { get; set; }
        public string big_photo { get; set; }

        public string is_transfer_image_big { get; set; }
    }
}