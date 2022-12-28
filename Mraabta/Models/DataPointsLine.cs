using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace MRaabta.Models
{
    public class DataPointsLine 
    {
        [DataMember(Name = "y")]
        public Nullable<double> Y = null;

        [DataMember(Name = "label")]
        public string Label = "";




    }
}