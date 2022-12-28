using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace MRaabtaApis.Models.Api
{
    public class Accounts
    {
        public string RiderCode { get; set; }

        public int userID { get; set; }


        public string branchCode { get; set; }

        public int RiderContact { get; set; }

        public int RoleId { get; set; }

        public int Status { get; set; }
        public string username { get; set; }
        public string password { get; set; }
    }
    public class Accounts_v5: Accounts
    {
        public Decimal Longitude { get; set; }
        public Decimal Latitude { get; set; }
    }
    public class userDetailResponse
    {
        public bool isSuccess { get; set; }
        public string message { get; set; }

        public string token { get; set; }

        public Accounts accountObj { get; set; }

        public HttpResponseMessage responseMsg { get; set; }

        public string Version { get; set; }

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
    public class userDetailLogout
    {
        public bool isSuccess { get; set; }
        public string message { get; set; }

        public string logout { get; set; }
        public string Version { get; set; }
    }


}