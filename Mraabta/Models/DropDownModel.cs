using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRaabta.Models
{
    public class DropDownModel
    {
        public string Value { get; set; }
        public string Text { get; set; }

        public string brancahCode { get; set; }

        public string routecode { get; set; }
        //public int _Value
        //{
        //    get
        //    {
        //        return Convert.ToInt32(Value);
        //    }
        //}
    }

    public class DDLModel
    {
        public string Value { get; set; }
        public string Text { get; set; }
        public int _Value
        {
            get
            {
                int i = 0;
                int.TryParse(Value, out i);
                return i;
            }
        }
    }
}