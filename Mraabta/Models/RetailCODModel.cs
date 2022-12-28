using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRaabta.Models
{
    public class RetailCODConsignmentModel
    {
        public string ConsignmentNumber { get; set; }
        public string BookingDate { get; set; }
        public string Service { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public string LocationName { get; set; }
        public string PrintDateTime { get; set; }
        public string Shipper { get; set; }
        public string ShipperAddress { get; set; }
        public string ShipperContact { get; set; }
        public string ConsigneeAddress { get; set; }
        public string ConsigneeContact { get; set; }
        public string Consignee { get; set; }
        public string SuppService { get; set; }
        public string SuppCharges { get; set; }
        public string calculatedGST { get; set; }
        public string DiscountValueType { get; set; }
        public string DiscountValue { get; set; }
        public string SpecialInstructions { get; set; }
        public string DimensionWidth { get; set; }
        public string DimensionBreadth { get; set; }
        public string DimensionHeight { get; set; }
        public string ValDeclared { get; set; }
        public string Pieces { get; set; }
        public string Weight { get; set; }
        public string Fragile { get; set; }
        public string DeclaredInsuranceValue{ get; set; }
        public string BookingStaff { get; set; }
        public string Discount { get; set; }
        public string GST { get; set; }
        public string NetAmount { get; set; }
        public string totalAmount { get; set; }
        public string PackageContents { get; set; }
        public string CODAmount{ get; set; }
        public string ProductDetail{ get; set; }
        public string Remarks{ get; set; }
        public string CustomerRef{ get; set; }
        public string ChargedAmount { get; set; }
        public string DiscountID { get; set; }
        public string couponNumber { get; set; }
        public string PriceModifierId { get; set; }
        public string BookingCode { get; set; }
        public string Province { get; set; }
        public string cnic { get; set; }
    }

    public class RetailCODCRFModel {
        public string AccountNumber { get; set; }
        public string RegistrationDate { get; set; }
        public string Zone { get; set; }
        public string Branch { get; set; }
        public string CustomerName { get; set; }
        public string NTN { get; set; }
        public string CNIC { get; set; }
        public string RegisteredAddress { get; set; }
        public string BankContactNo { get; set; }
        public string BankAccountTitle { get; set; }
        public string BankName { get; set; }
        public string BankAccNo { get; set; }
        public string BranchName{ get; set; }
        public string BankBranchCode{ get; set; }
        public string RSEName{ get; set; }
        public string CourierCenterName{ get; set; }

    }
}