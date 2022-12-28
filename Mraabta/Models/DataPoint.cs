﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace MRaabta.Models
{
    [DataContract]
    public class DataPoint
    {
        //DataContract for Serializing Data - required to serve in JSON format

        //public DataPoint(double y, string label)
        //{
        //    //this.X = x;
        //    this.Y = y;
        //    this.Label = label;
        //}

        //Explicitly setting the name to be used while serializing to JSON.
        //[DataMember(Name = "x")]
        //public Nullable<double> X = null;

        //Explicitly setting the name to be used while serializing to JSON.
        [DataMember(Name = "y")]
        public Nullable<double> Y = null;

        [DataMember(Name = "x")]
        public string X = null;

        [DataMember(Name = "Date")]
        public string Label = "";
    }
}
