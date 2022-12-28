using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRaabta.Models
{

    public class StockRequestModel
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public string ZoneCode { get; set; }
        public int TotalReqQty { get; set; }
        public string TotalReqQty_ { get; set; }
        public string CreatedOn { get; set; }
        public string LocationName { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentID { get; set; }
        public bool IsUpdated { get; set; }
        public String CreatedBy { get; set; }
        public String BranchCode { get; set; }
        public String BranchCo { get; set; }
        public String ECRiderCode { get; set; }
        public string ECName { get; set; }
        public String UserTypeId { get; set; }

        public List<StockRequestDetailModel> StockRequestDetails { get; set; }
        public string BranchName { get; set; }
        public string ZoneName { get; set; }
        public string OracleCode { get; set; }
        public string Unit { get; set; }
    }
    public class Response_StockRequestModel {
        public List<StockRequestModel> Data{ get; set; }
        public bool status { get; set; }
        public string statusMessage { get; set; }
    }


    
    public class StockRequestDetailModel
    {
        public int Inv_ID { get; set; }
        public int StockID { get; set; }
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Qty { get; set; }
        public string Qty_ { get; set; }
        public int IssueQty { get; set; }
        public String BarcodeIssueFrom { get; set; }
        public int QtyIssueFrom { get; set; }
        public String BarcodeFrom { get; set; }
        public String BarcodeTo { get; set; }
        public String TypeName { get; set; }
        public int TypeId { get; set; }
        public bool isupdated { get; set; }
        public string ZoneCode { get; set; }
        public string name { get; set; }
        public int TotalSequence { get; set; }
        public int DetailId { get; set; }
        public string locationId { get; set; }
        public string DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public string BarcodeFrm { get; set; }

        public int ReqId { get; set; }
        public string Product { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Unit { get; set; }
        public string OracleCode { get; set; }
        public string PONo { get; set; }
        public string DCNo { get; set; }
        public string LocationName { get; set; }
        public string CreatedOn { get; set; }

    }



    public class Response_StockRequestDetailModel
    {
        public bool status { get; set; }
        public string statusMessage { get; set; }
        public List<StockRequestDetailModel> Data { get; set; }
    }
    public class StockIssuanceModel
    {
        public int ID { get; set; }
        public int StockRequestId { get; set; }
        public int Year { get; set; }
        public string ZoneCode { get; set; }
        public string BranchCode { get; set; }
        public string ECRiderCode { get; set; }
        public int TotalIssueQty { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int TotalReqQty { get; set; }
        public bool IsUpdated { get; set; }
        public string FOCNumber { get; set; }
        public string Remarks { get; set; }
        public string DepartmentID { get; set; }

        public List<StockIssuanceDetailModel> StockIssuanceDetails { get; set; }


    }


    public class StockIssuanceDetailModel
    {

        public int StockIssuanceId { get; set; }
        public int stockIssuanceDetailID { get; set; }
        public int StockRequestDetailId { get; set; }
        public int ProductId { get; set; }
        public int RequestedQty { get; set; }
        public int IssueQty { get; set; }
        public string BarcodeFrom { get; set; }
        public string BarcodeTo { get; set; }
        public int QtyIssueFrom { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public String ProductName { get; set; }
    }

}