using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRaabta.Models
{
    public class StockModel
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string TypeName { get; set; }
        public string BarcodeFrm { get; set; }
        public string BarcodeTo { get; set; }
        public int Qty { get; set; }
        public string Qty_ { get; set; }
        public int Year { get; set; }
        public string Zone { get; set; }
        public string BranchName { get; set; }
        public string ZoneCode { get; set; }
        public string OracleCode { get; set; }

    }

    public class PreviousIssuanceModel
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public string ZoneCode { get; set; }
        public int TotalReqQty { get; set; }
        public int TotalIssueQty { get; set; }
    }

    public class StockReceivingDetailModel
    {
        public string ProductName { get; set; }
        public string TypeName { get; set; }
        public int IssuanceID { get; set; }
        public int RequestID { get; set; }
        public int ReceivingID { get; set; }
        public int IssuanceDetailID { get; set; }
        public int RecievingQty { get; set; }
        public string RecievingQty_ { get; set; }
        public int RequestQty { get; set; }
        public string RequestQty_ { get; set; }
        public string BarcodeFrom { get; set; }
        public string BarcodeTo { get; set; }
        public bool isActive { get; set; }
        public int CreatedBy { get; set; }
        public string FOCNumber { get; set; }
        public DateTime CreatedOn { get; set; }
        public int LastModifiedBy { get; set; }
        public DateTime LastModifiedOn { get; set; }
        public string OracleCode { get; set; }
    }

    public class StockReceivingModel
    {
        public int ID { get; set; }
        public string ZoneName { get; set; }

        public int RequestID { get; set; }
        public int ReceivingID { get; set; }
        public int IssuanceID { get; set; }

        public int Year { get; set; }

        public String CreatedOn { get; set; }
        public int ZoneCode { get; set; }
        public int branchCode { get; set; }
        public int ECRiderCode { get; set; }
        public int UserTypeID { get; set; }
        public int TotalReqQty { get; set; }
        public string TotalReqQty_ { get; set; }
        public int TotalIssueQty { get; set; }
        public string TotalIssueQty_ { get; set; }
        public string OracleCode { get; set; }
        public string DepartmentName { get; set; }
        public int TotalRecieveQty { get; set; }
        public bool isActive { get; set; }
        public String CreatedBy { get; set; }
        public string LocationName { get; set; }
        public string Unit { get; set; }
        //public DateTime CreatedOn { get; set; }
        public int LastModifiedBy { get; set; }
        public DateTime LastModifiedOn { get; set; }


        public List<StockReceivingDetailModel> StockReceivingDetails { get; set; }
    }

    public class CNIssuanceDropDownModel
    {
        public string Value { get; set; }
        public string Text { get; set; }

    }

    public class StockConsumptionModel
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string TypeName { get; set; }
        public string BarcodeFrm { get; set; }
        public string BarcodeTo { get; set; }
        public string Qty { get; set; }
        public string Qty_ { get; set; }
        public int Year { get; set; }
        public string Zone { get; set; }
        public string BranchName { get; set; }
        public string ZoneCode { get; set; }
        public string OracleCode { get; set; }
    }
}