using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace MRaabta.Models
{
    public class CXMComplaintConsignmentDetails
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string RequestNature { get; set; }
        public string RequestType { get; set; }
        public string ConsignmentNumber { get; set; }
        public string InquirerType { get; set; }
        public string InquirerName { get; set; }
        public string EmailId { get; set; }
        public string PhoneNumber { get; set; }
        public string CellNumber { get; set; }
        public string AccountNo { get; set; }
        public string Weight { get; set; }
        public string Pieces { get; set; }
        public string ShipperName { get; set; }
        public string ShipperCell { get; set; }
        public string ShipperAddress { get; set; }
        public string ConsigneeName { get; set; }
        public string ConsigneeCell { get; set; }
        public string ConsigneeAddress { get; set; }
        public string SourceMedia { get; set; }
        public string Department { get; set; }
        public string AllocationBy { get; set; }
        public string StandardNotes { get; set; }
        public string Description { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
    }

    public class CXMComplaintRequest
    {
        public bool isSuccess { get; set; }
        public string message { get; set; }
        public DataTable RequestType { get; set; }
    }
    public class Response_CXMComplaintSave
    {
        public bool isSuccess { get; set; }
        public string message { get; set; }
    }
    public class CXMComplaintStandardNotes
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string RequestTypeValue { get; set; }
    }
    public class CXMConsignment
    {
        public bool isSuccess { get; set; }
        public string InquirerName { get; set; }
        public string InquirerEmail { get; set; }
        public string InquirerPhoneNumber { get; set; }
        public string InquirerCellNumber { get; set; }
        public string AccountNo { get; set; }
        public string Weight { get; set; }
        public string Pieces { get; set; }
        public string ShipperName { get; set; }
        public string ShipperCell { get; set; }
        public string ShipperAddress { get; set; }
        public string Consignee { get; set; }
        public string ConsigneeCell { get; set; }
        public string ConsigneeAddress { get; set; }
        public string AllocationZoneName { get; set; }
        public string AllocationZoneCode { get; set; }
        public string OriginName { get; set; }
        public string OriginCode { get; set; }
        public string DestinationName { get; set; }
        public string DestinationCode { get; set; }
    }
}