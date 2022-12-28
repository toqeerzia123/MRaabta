using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// Summary description for CL_Customer
/// </summary>
/// 
namespace MRaabta.App_Code
{
    public class CL_Customer
    {
        private string _Name;
        private string _ContactPerson;
        private string _Category;
        private string _PhoneNo;


        private string _FaxNo;
        private string _OfficialAddress;
        private string _MailingAddress;
        private string _PickupInstructions;

        private string _RegDate;
        private string _RegEndDate;
        private string _Email;
        private int _type;
        private string _IsCOD;
        private string _Status;
        private string _StatusCode;
        private string _IsCentralized;

        private string _PrintingStatus;
        private string _RedeemWindow;
        private string _Sector;
        private string _Industry;
        private string _ClientGroupID;

        private string _DomesticTurnOver;
        private string _DomesticPackets;
        private string _DomesticAmount;

        private string _InternationalTurnOver;
        private string _InternationalPackets;
        private string _InternationalAmount;

        private string _DiscountOnDocument;
        private string _DiscountOnDomestic;
        private string _DiscountOnSample;

        private string _DomesticAmonTo;
        private string _InternationalAmonTo;


        private string _CreditLimit;
        private string _RnRCharges;
        private string _CODMonthlyBillingAmount;
        private string _BillTaxType;
        private string _Memo;
        private string _NTNNo;
        private string _SalesRoute;
        private string _SalesTaxNo;
        private string _StaffType;
        private string _BillingMode;
        private string _PrepareBillType;
        private string _RecoveryExpID;
        private string _RecoveryOfficer;
        private string _SalesExecutive;
        private string _OverdueCalculationBase;
        private string _OverdueValue;
        private string _AccountNo;
        private string _CreditClientType;
        private string _IsActive;
        private string _IsSpecial;
        private string _IsFranchise;
        private string _RecoveryOfficerName;
        private string _IsNationWide;
        private string _IsParent;

        private string _OriginEc;

        private string _CODType;
        private string _MonthlyFixedCharges;
        private string _IsSmsServiceActive;
        private string _HSCNCharges;

        private string _RecoveryOfficerID;










        public CL_Customer()
        {
            //
            // TODO: Add constructor logic here
            //

            _Name = string.Empty;
            _ContactPerson = string.Empty;
            _Category = string.Empty;
            _PhoneNo = string.Empty;
            _FaxNo = string.Empty;
            _OfficialAddress = string.Empty;
            _MailingAddress = string.Empty;
            _PickupInstructions = string.Empty;
            _RegDate = string.Empty;
            _RegEndDate = string.Empty;
            _Email = string.Empty;
            _type = 0;
            _IsCOD = string.Empty;
            _Status = string.Empty;
            _StatusCode = string.Empty;
            _IsCentralized = string.Empty;
            _PrintingStatus = string.Empty;
            _RedeemWindow = string.Empty;
            _Sector = string.Empty;
            _Industry = string.Empty;
            _ClientGroupID = string.Empty;
            _DomesticTurnOver = string.Empty;
            _DomesticPackets = string.Empty;
            _DomesticAmount = string.Empty;
            _InternationalTurnOver = string.Empty;
            _InternationalPackets = string.Empty;
            _InternationalAmount = string.Empty;
            _DiscountOnDocument = string.Empty;
            _DiscountOnDomestic = string.Empty;
            _DiscountOnSample = string.Empty;
            _CreditLimit = string.Empty;
            _RnRCharges = string.Empty;
            _CODMonthlyBillingAmount = string.Empty;
            _BillTaxType = string.Empty;
            _Memo = string.Empty;
            _NTNNo = string.Empty;
            _SalesRoute = string.Empty;
            _SalesTaxNo = string.Empty;
            _StaffType = string.Empty;

            _BillingMode = string.Empty;
            _PrepareBillType = string.Empty;
            _RecoveryExpID = string.Empty;
            _RecoveryOfficer = string.Empty;
            _SalesExecutive = string.Empty;
            _OverdueCalculationBase = string.Empty;
            _OverdueValue = string.Empty;
            _AccountNo = string.Empty;
            _CreditClientType = string.Empty;
            _IsActive = string.Empty;
            _IsSpecial = string.Empty;
            _IsFranchise = string.Empty;
            _RecoveryOfficerName = string.Empty;
            _IsNationWide = string.Empty;
            _IsParent = string.Empty;

            _OriginEc = string.Empty;

            _CODType = string.Empty;
            _MonthlyFixedCharges = string.Empty;
            _IsSmsServiceActive = string.Empty;
            _HSCNCharges = string.Empty;

            _RecoveryOfficerID = string.Empty;
        }
        public string Strcon()
        {
            string QueryString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString_"].ToString();
            return QueryString;
        }
        public string Strcon2()
        {
            string QueryString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString_"].ToString();
            return QueryString;
        }
        public string StrconLive()
        {
            string QueryString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            return QueryString;
        }

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        public string ContactPerson
        {
            get { return _ContactPerson; }
            set { _ContactPerson = value; }
        }
        public string Category
        {
            get { return _Category; }
            set { _Category = value; }
        }
        public string PhoneNo
        {
            get { return _PhoneNo; }
            set { _PhoneNo = value; }
        }
        public string FaxNo
        {
            get { return _FaxNo; }
            set { _FaxNo = value; }
        }
        public string OfficialAddress
        {
            get { return _OfficialAddress; }
            set { _OfficialAddress = value; }
        }
        public string MailingAddress
        {
            get { return _MailingAddress; }
            set { _MailingAddress = value; }
        }
        public string PickupInstructions
        {
            get { return _PickupInstructions; }
            set { _PickupInstructions = value; }
        }
        public string RegDate
        {
            get { return _RegDate; }
            set { _RegDate = value; }
        }
        public string RegEndDate
        {
            get { return _RegEndDate; }
            set { _RegEndDate = value; }
        }
        public string CustomerEmail
        {
            get { return _Email; }
            set { _Email = value; }
        }
        public int type
        {
            get { return _type; }
            set { _type = value; }
        }
        public string IsCOD
        {
            get { return _IsCOD; }
            set { _IsCOD = value; }
        }
        public string Status
        {
            get { return _Status; }
            set { _Status = value; }
        }
        public string StatusCode
        {
            get { return _StatusCode; }
            set { _StatusCode = value; }
        }
        public string IsCentralized
        {
            get { return _IsCentralized; }
            set { _IsCentralized = value; }
        }
        public string PrintingStatus
        {
            get { return _PrintingStatus; }
            set { _PrintingStatus = value; }
        }
        public string RedeemWindow
        {
            get { return _RedeemWindow; }
            set { _RedeemWindow = value; }
        }
        public string Sector
        {
            get { return _Sector; }
            set { _Sector = value; }
        }
        public string Industry
        {
            get { return _Industry; }
            set { _Industry = value; }
        }
        public string ClientGroupID
        {
            get { return _ClientGroupID; }
            set { _ClientGroupID = value; }
        }
        public string DomesticTurnOver
        {
            get { return _DomesticTurnOver; }
            set { _DomesticTurnOver = value; }
        }
        public string DomesticPackets
        {
            get { return _DomesticPackets; }
            set { _DomesticPackets = value; }
        }
        public string DomesticAmount
        {
            get { return _DomesticAmount; }
            set { _DomesticAmount = value; }
        }
        public string InternationalTurnOver
        {
            get { return _InternationalTurnOver; }
            set { _InternationalTurnOver = value; }
        }
        public string InternationalPackets
        {
            get { return _InternationalPackets; }
            set { _InternationalPackets = value; }
        }
        public string InternationalAmount
        {
            get { return _InternationalAmount; }
            set { _InternationalAmount = value; }
        }
        public string DiscountOnDocument
        {
            get { return _DiscountOnDocument; }
            set { _DiscountOnDocument = value; }
        }
        public string DiscountOnDomestic
        {
            get { return _DiscountOnDomestic; }
            set { _DiscountOnDomestic = value; }
        }
        public string DiscountOnSample
        {
            get { return _DiscountOnSample; }
            set { _DiscountOnSample = value; }
        }
        public string DomesticAmonTo
        {
            get { return _DomesticAmonTo; }
            set { _DomesticAmonTo = value; }
        }
        public string InternationalAmonTo
        {
            get { return _InternationalAmonTo; }
            set { _InternationalAmonTo = value; }
        }
        public string CreditLimit
        {
            get { return _CreditLimit; }
            set { _CreditLimit = value; }
        }
        public string RnRCharges
        {
            get { return _RnRCharges; }
            set { _RnRCharges = value; }
        }
        public string CODMonthlyBillingAmount
        {
            get { return _CODMonthlyBillingAmount; }
            set { _CODMonthlyBillingAmount = value; }
        }
        public string BillTaxType
        {
            get { return _BillTaxType; }
            set { _BillTaxType = value; }
        }
        public string Memo
        {
            get { return _Memo; }
            set { _Memo = value; }
        }
        public string NTNNo
        {
            get { return _NTNNo; }
            set { _NTNNo = value; }
        }
        public string SalesRoute
        {
            get { return _SalesRoute; }
            set { _SalesRoute = value; }
        }
        public string SalesTaxNo
        {
            get { return _SalesTaxNo; }
            set { _SalesTaxNo = value; }
        }
        public string StaffType
        {
            get { return _StaffType; }
            set { _StaffType = value; }
        }
        public string BillingMode
        {
            get { return _BillingMode; }
            set { _BillingMode = value; }
        }
        public string PrepareBillType
        {
            get { return _PrepareBillType; }
            set { _PrepareBillType = value; }
        }

        public string RecoveryExpID
        {
            get { return _RecoveryExpID; }
            set { _RecoveryExpID = value; }
        }
        public string RecoveryOfficer
        {
            get { return _RecoveryOfficer; }
            set { _RecoveryOfficer = value; }
        }
        public string SalesExecutive
        {
            get { return _SalesExecutive; }
            set { _SalesExecutive = value; }
        }
        public string OverdueCalculationBase
        {
            get { return _OverdueCalculationBase; }
            set { _OverdueCalculationBase = value; }
        }
        public string OverdueValue
        {
            get { return _OverdueValue; }
            set { _OverdueValue = value; }
        }
        public string AccountNo
        {
            get { return _AccountNo; }
            set { _AccountNo = value; }
        }
        public string CreditClientType
        {
            get { return _CreditClientType; }
            set { _CreditClientType = value; }
        }
        public string IsActive
        {
            get { return _IsActive; }
            set { _IsActive = value; }
        }
        public string IsSpecial
        {
            get { return _IsSpecial; }
            set { _IsSpecial = value; }
        }
        public string IsFranchise
        {
            get { return _IsFranchise; }
            set { _IsFranchise = value; }
        }
        public string RecoveryOfficerName
        {
            get { return _RecoveryOfficerName; }
            set { _RecoveryOfficerName = value; }
        }
        public string IsNationWide
        {
            get { return _IsNationWide; }
            set { _IsNationWide = value; }
        }

        public string IsParent
        {
            get { return _IsParent; }
            set { _IsParent = value; }
        }

        public string OriginEc
        {
            get { return _OriginEc; }
            set { _OriginEc = value; }
        }
        public string CODType
        {
            get { return _CODType; }
            set { _CODType = value; }
        }
        public string MonthlyFixedCharges
        {
            get { return _MonthlyFixedCharges; }
            set { _MonthlyFixedCharges = value; }
        }
        public string IsSmsServiceActive
        {
            get { return _IsSmsServiceActive; }
            set { _IsSmsServiceActive = value; }
        }
        public string HSCNCharges
        {
            get { return _HSCNCharges; }
            set { _HSCNCharges = value; }
        }
        public string RecoveryOfficerID
        {
            get { return _RecoveryOfficerID; }
            set { _RecoveryOfficerID = value; }
        }
    }
}