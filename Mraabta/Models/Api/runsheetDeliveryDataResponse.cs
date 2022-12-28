using MRaabtaApis.Models.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace MRaabta.Models.Api
{
    public class runsheetDeliveryDataResponse

    {

        public bool isSuccess { get; set; }



        public string Message { get; set; }



        public List<runsheetDeliveryData> deliverydataList { get; set; }

    }



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



    public class userDetailResponse_v5

    {

        public bool isSuccess { get; set; }

        public string message { get; set; }



        public string token { get; set; }



        public Accounts_v5 accountObj { get; set; }



        public HttpResponseMessage responseMsg { get; set; }



        public string Version { get; set; }



    }



    public class runsheetDeliveryData
    {

        public int OriginId { get; set; }

        public int DestinationId { get; set; }

        public int RTSBranchId { get; set; }

        public String consignmentNumber { get; set; }

        public String remarks { get; set; }

        public String consigneePhoneNo { get; set; }

        public int pieces { get; set; }



        public double weight { get; set; }

        public string origin { get; set; }



        public String destination { get; set; }



        public String consignee { get; set; }



        public String Address { get; set; }

        public bool is_cod { get; set; }



        public int SortOrder { get; set; }



        public int codAmount { get; set; }

        public string Receiver_CNIC { get; set; }



        public String riderCode { get; set; }



        public bool isnic { get; set; }



        public bool isself { get; set; }



        public int isMobilePerformed { get; set; }

        public bool isvas { get; set; }


    }
}