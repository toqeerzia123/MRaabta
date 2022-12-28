using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MRaabta.Models
{
    [Table("RiderData")]
    public class RiderData
    {
        [Key]
        public string riderCode { get; set; }
        public string riderName { get; set; }

        [NotMapped]
        public SelectList RiderList { get; set; }

    }
}