using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRaabta.Models.Api
{
    public class CODCNCashConfirmationModel
    {
        public string ID { get; set; }
        public string RiderCode { get; set; }
        public string Date { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public List<CODCNCashConfirmationDetailModel> Detail { get; set; }
    }
    public class CODCNCashConfirmationDetailModel
    {
        public string RunsheetNumber { get; set; }
        public string ConsignmentNumber { get; set; }
        public decimal CNAmount { get; set; }
    }
    public class CODCNCashConfirmationDetailReturnModel
    {
        public string Id { get; set; }
        public string ConsignmentNumber { get; set; }
        public bool IsUpdated { get; set; }
    }
}