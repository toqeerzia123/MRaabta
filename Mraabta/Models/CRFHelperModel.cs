using System;
using System.Collections.Generic;

namespace MRaabta.Models
{
    public enum CRFUserType
    {
        None = 0,
        SalesPerson = 1,
        AreaManager = 2,
        GeneralManager = 3,
        Director = 4,
        ZA = 5
    }
    public class CustomerStatusModel
    {
        public CRFUserType Type { get; set; }
        public bool Approve { get; set; }
        public int CustomerId { get; set; }
        public string Remarks { get; set; }
        public int Level { get; set; }
    }

    public class CustomerListModel
    {
        public int CustomerId { get; set; }
        public string BusinessName { get; set; }
        public string RequestedBy { get; set; }
        public int Id { get; set; }
        public int UserId { get; set; }
        public string User { get; set; }
        public CRFUserType UserStaffLevelId { get; set; }
        public DateTime CreatedOn { get; set; }
        public string DateTime { get; set; }
        public string UpdatedDateTime { get; set; }
        public int Status { get; set; }
        public string StatusName { get; set; }
        public string Remarks { get; set; }
        public bool IsLast { get; set; }
        public bool IsActive { get; set; }
        public bool Edit { get; set; }
        public bool Print { get; set; }
        public bool Approve { get; set; }
        public bool FinalStatus { get; set; }
        public string NextApprover { get; set; }
        public int Level { get; set; }
        public bool IsFAC { get; set; }
        public decimal FuelFactor { get; set; }
        public string BranchCode { get; set; }
        public IEnumerable<CustomerListDetailModel> Details { get; set; }
        public IEnumerable<Rate> Rates { get; set; }
    }

    public class CustomerListDetailModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string User { get; set; }
        public CRFUserType UserStaffLevelId { get; set; }
        public DateTime CreatedOn { get; set; }
        public string DateTime { get; set; }
        public string UpdatedDateTime { get; set; }
        public int Status { get; set; }
        public string StatusName { get; set; }
        public string Remarks { get; set; }
        public bool IsLast { get; set; }
        public bool IsActive { get; set; }
    }


    public class Customer
    {
        public int Id { get; set; }
        public string BusinessName { get; set; }
        public int IsSingle { get; set; }
        public int GroupId { get; set; }
        public string Group { get; set; }
        public int IndustryId { get; set; }
        public string Industry { get; set; }
        public string ContactPerson { get; set; }
        public string Designation { get; set; }
        public string PhoneNo { get; set; }
        public string FaxNo { get; set; }
        public string CNIC { get; set; }
        public string Email { get; set; }
        public bool NTNRegistered { get; set; }
        public string NTNNumber { get; set; }
        public bool GSTRegistered { get; set; }
        public string GSTNumber { get; set; }
        public int CityId { get; set; }
        public string City { get; set; }
        public string Zone { get; set; }
        public string PostalCode { get; set; }
        public string Area { get; set; }
        public string SectorOrZone { get; set; }
        public string Street { get; set; }
        public string PlotNo { get; set; }
        public string BuildingName { get; set; }
        public string FloorNo { get; set; }
        public string HouseOrOfficeNo { get; set; }
        public string BeneficiaryName { get; set; }
        public string BeneficiaryRelation { get; set; }
        public string IBAN { get; set; }
        public int BankId { get; set; }
        public string BankName { get; set; }
        public string BankBranchCode { get; set; }
        public int BankBranchCity { get; set; }
        public string BankBranchCityName { get; set; }
        public decimal ExpectedRevenue { get; set; }
        public bool IsFAF { get; set; }
        public bool IsFAC { get; set; }
        public decimal FuelFactor { get; set; }
        public string InvoicingScheduler { get; set; }
        public bool IsAutoRecovery { get; set; }
        public int CreditTermsOrDays { get; set; }
        public int SalesDecision { get; set; }
        public int BillingInstruction { get; set; }
        public decimal AnnualRateRevision { get; set; }
        public bool EInvoicing { get; set; }
        public bool PrintInvoice { get; set; }
        public decimal MinimumBilling { get; set; }
        public int RecoveryMode { get; set; }
        public bool TaxExcemption { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByEmail { get; set; }
        public List<CustomerContactPerson> ContactPersons { get; set; }
        public List<CustomerPickupLocation> PickupLocations { get; set; }
        public List<Rate> Rates { get; set; }
    }
    public class CustomerContactPerson
    {
        public string ContactPersonName { get; set; }
        public string Designation { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
    }
    public class CustomerPickupLocation
    {
        public string LocationName { get; set; }
        public string ContactPersonName { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public int CityId { get; set; }
        public string Area { get; set; }
        public string SectorORZone { get; set; }
        public string Street { get; set; }
        public string PlotNumber { get; set; }
        public string BuildingName { get; set; }
        public string FloorNO { get; set; }
        public string HouseOrOfficeNo { get; set; }
    }
    public class Rate
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public bool Overnight { get; set; }
        public bool SecondDay { get; set; }
        public bool ECargo { get; set; }
        public bool Flyer { get; set; }
        public bool FlyerWind { get; set; }
        public decimal AddFactor { get; set; }
        public decimal MinWeight { get; set; }
        public decimal ExpectedRev { get; set; }
        public decimal ZeroToPoint5KGWC { get; set; }
        public decimal ZeroToPoint5KGSZ { get; set; }
        public decimal ZeroToPoint5KGDZ { get; set; }
        public decimal Point5To1KGWC { get; set; }
        public decimal Point5To1KGSZ { get; set; }
        public decimal Point5To1KGDZ { get; set; }
        public decimal AddKGWC { get; set; }
        public decimal AddKGSZ { get; set; }
        public decimal AddKGDZ { get; set; }
        public decimal AddKGWC2 { get; set; }
        public decimal AddKGSZ2 { get; set; }
        public decimal AddKGDZ2 { get; set; }
        public decimal SecDayMin { get; set; }
        public decimal SecDayAdd { get; set; }
        public decimal SecDayMin2 { get; set; }
        public decimal SecDayAdd2 { get; set; }
        public decimal ZoneAMin { get; set; }
        public decimal ZoneAAdd { get; set; }
        public decimal ZoneBMin { get; set; }
        public decimal ZoneBAdd { get; set; }
        public decimal ZoneAMin2 { get; set; }
        public decimal ZoneAAdd2 { get; set; }
        public decimal ZoneBMin2 { get; set; }
        public decimal ZoneBAdd2 { get; set; }
        public decimal FlyerS { get; set; }
        public decimal FlyerM { get; set; }
        public decimal FlyerL { get; set; }
        public decimal FlyerXL { get; set; }
        public decimal FlyerWinS { get; set; }
        public decimal FlyerWinM { get; set; }
        public decimal FlyerWinL { get; set; }
        public decimal FlyerWinXL { get; set; }
        public int Level { get; set; }
        public decimal FACFuelFactor { get; set; }
        public string AccountNo { get; set; }
    }
}