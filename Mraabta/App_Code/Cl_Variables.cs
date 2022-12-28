using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// Summary description for Cl_Variables
/// </summary>
/// 

namespace MRaabta.App_Code
{
    public class Cl_Variables
    {
        #region  Variables

        private string _PaymentMode;
        private string _PaymentTransactionID;
        private string _NoteNumber;
        private string _NoteType;
        private string _consignmentNo;
        private string _AccountNo;
        private string _RiderCode;
        private int _Con_Type;
        private string _Destination;
        private string _ServiceType;
        private float _Weight;
        private int _Unit;
        private int _Discount;
        private int _pieces;
        private string _Consignee;
        private string _Consigner;
        private string _ConsignerCell;
        private string _ConsignerCNIC;
        private string _ConsignerPhone;
        private string _ConsigneeCell;
        private string _ConsigneeCNIC;
        private string _CouponNo;
        private string _FlyerType;
        private string _IsCodIsInsurance;
        private string _Remarks;
        private string _ConsigneeAddress;
        private string _ConsignerAddress;
        private float _Declaredvalue;
        private DateTime _Bookingdate;
        private string _origin;
        private string _Insurance;
        private float _Othercharges;
        private string _Day;
        private string _Branch;
        private string _Zone;
        private string _expresscenter;
        private string _destinationExpressCenter;
        private string _Flyer;
        private string _RefNo;
        private string _destination;
        private string _clientID;
        private double _chargeAmount;
        private double _totalAmount;
        private double _gst;
        private double _tariff;
        private int _customertype;
        private string _ServiceTypeName;
        private Boolean _isCod;
        private string _createdBy;
        private int _status;
        private Boolean _isCreatedFromMMUs;
        private int _deliveryType;
        private string _remarks;
        private string _shipperAddress;
        private string _riderCode;
        private double _width;
        private double _breadth;
        private double _height;
        private string _PakageContents;
        private DateTime _expressionDeliveryDateTime;   //Expressions
        private Boolean _expressionGreetingCard;        //Expressions
        private string _expressionMessage;              //Expressions
        private string _expressionconsignmentRefNumber; //Expressions
        private string _routeCode;
        private string _docPouchNo;
        private string _consignerAccountNo;
        private string _consignerEmail;
        private Boolean _docIsHomeDelivery;
        private DateTime _cutOffTime;
        private string _destinationCountryCode;
        private float _insuarancePercentage;
        private Boolean _isInsured;
        private Boolean _isReturned;
        private DateTime _cutOffTimeShift;
        private int _cnClientType;
        private string _destinationExpressCenterCode;
        private int _dayType;
        private Boolean _receivedFromRider;
        private string _cnState;
        private string _cardRefNo;
        private string _manifestNo;
        private int _manifestId;
        private string _originBrName;
        private string _orderRefNo;
        private string _customerName;
        private int _productTypeId;
        private string _productDescription;
        private Boolean _chargeCODAmount;
        private float _codAmount;
        private float _calculatedCodAmount;
        private Boolean _isPM;
        private DataTable _LstModifiersCNt;
        private string _serviceTypeId;
        private string _originId;
        private string _destId;
        private string _cnTypeId;
        private string _cnOperationalType;
        private string _cnScreenId;
        private string _cnStatus;
        private string _flagSendConsigneeMsg;
        private string _isExpUser;
        private DataTable _expresssion;

        private int _carier;
        private int _mode;
        private int _term;
        private int _subServicetype;
        private string _subdetai;
        private int _Currency;
        private double _amount;
        private string _instruction;
        private string _destinationCity;
        private int _postalCode;
        private string _consigneeAttention;
        private int _destinationCitycode;
        private DateTime _deliveryDate;
        private string _ToZoneCode;
        private string _FromZoneCode;
        private string _BaseCurrency;
        private string _ToCurrency;
        private string _Error;
        public string _CityCode;

        private string _Company;
        private string _ServiceTypeCategory;
        private Boolean _isInternational;
        private string _commissionID;
        private Boolean _isFranchised;

        private string _currencyID;


        private string _currencyName;
        private string _currencyCode;
        private string _currencySymbol;

        private string _FromWeight;
        private string _ToWeight;
        private string _AdditionalFactor;

        private string _tariffID;

        private string _FromCat;
        private string _ToCat;
        private string _OriginExpressCenterCode;
        private string _BagNumber;
        private Boolean _VoidConsignment;
        private string _BranchName;
        private string _SealNumber;
        private string _RemarksID;
        private string _OriginCountry;
        private Boolean _IsImport;

        public string _RunsheetNumber;

        string _LoadingID;
        string _UnloadingID;
        string _LoadingDate;
        string _UnloadingDate;


        List<string> _NormalBags;
        List<string> _ShortReceivedBags;

        List<string> _ReceivedManifests;
        List<string> _ShortManifests;
        List<string> _ShortManifestRemarks;
        List<string> _BagManifestID;



        private string _CheckCondition;


        private string _RunSheet;


        private List<string> _ClvarListStr;
        private List<Int64> _ClvarListInt;
        private string _RunSheetDate;
        private string _RouteDesc;

        private string _RunSheetType;
        private string _RunSheetTypeID;

        private string _CreditClientID;
        private string _RefNumber;
        private string _StateID;


        private DateTime _FromDate;
        private DateTime _ToDate;


        private string _ReceiptNo;
        private string _VoucherNo;
        private string _VoucherDate;
        private string _Bank;
        private string _PaymentSource;
        private string _PaymentType;
        private string _ChequeNo;
        private string _ChequeDate;
        private string _ClientGroupID;
        private string _StaffType;

        private Boolean _isCentralized;
        private Boolean _isByCreditClientID;

        private string _BookingDate;
        public string _InvoiceNo;
        private string _CatID;
        private string _startsequence;
        private string _endsequence;
        private string _expresscentername;
        private string _expresscentertype;
        private bool _isdistributionCenter;
        private bool _ismainEc;
        private string _Dayoff;
        private string _shortName;
        private string _fax;
        private string _pruchaseorder;
        private string _pruchasedate;

        public string pruchasedate
        {
            set { _pruchasedate = value; }
            get
            {
                return _pruchasedate;
            }
        }

        public string pruchaseorder
        {
            set { _pruchaseorder = value; }
            get
            {
                return _pruchaseorder;
            }
        }






        public string CatID
        {
            get { return _CatID; }
            set { _CatID = value; }
        }
        public Boolean IsImport
        {
            get { return _IsImport; }
            set { _IsImport = value; }
        }

        public string InvoiceNo
        {
            get
            {
                return _InvoiceNo;
            }
            set
            {
                if (_InvoiceNo == value)
                    return;
                _InvoiceNo = value;
            }
        }

        public Boolean IsByCreditClientID
        {
            get { return _isByCreditClientID; }
            set { _isByCreditClientID = value; }
        }

        public Boolean IsCentralized
        {
            get { return _isCentralized; }
            set { _isCentralized = value; }
        }

        List<string> _ExcessBags;

        public string startsequence
        {
            get { return _startsequence; }
            set { _startsequence = value; }
        }

        public string endsequence
        {
            get { return _endsequence; }
            set { _endsequence = value; }
        }

        #endregion
        public string Fax
        {
            get
            {
                return _fax;
            }
            set
            {
                if (_fax == value)
                    return;
                _fax = value;
            }
        }

        public string shortName
        {
            get
            {
                return _shortName;
            }
            set
            {
                if (_shortName == value)
                    return;
                _shortName = value;
            }
        }

        public string expresscentername
        {
            get
            {
                return _expresscentername;
            }
            set
            {
                if (_expresscentername == value)
                    return;
                _expresscentername = value;
            }
        }

        public string PaymentMode
        {
            get
            {
                return _PaymentMode;
            }
            set
            {
                if (_PaymentMode == value)
                    return;
                _PaymentMode = value;
            }
        }

        public string PaymentTransactionID
        {
            get
            {
                return _PaymentTransactionID;
            }
            set
            {
                if (_PaymentTransactionID == value)
                    return;
                _PaymentTransactionID = value;
            }
        }

        public string OriginCountry
        {
            get
            {
                return _OriginCountry;
            }
            set
            {
                if (_OriginCountry == value)
                    return;
                _OriginCountry = value;
            }
        }

        public string expresscentertype
        {
            get
            {
                return _expresscentertype;
            }
            set
            {
                if (_expresscentertype == value)
                    return;
                _expresscentertype = value;
            }
        }
        public bool isdistributionCenter
        {
            get
            {
                return _isdistributionCenter;
            }
            set
            {
                if (_isdistributionCenter == value)
                    return;
                _isdistributionCenter = value;
            }
        }
        public bool ismainEc
        {
            get
            {
                return _ismainEc;
            }
            set
            {
                if (_ismainEc == value)
                    return;
                _ismainEc = value;
            }
        }
        public string Dayoff
        {
            get
            {
                return _Dayoff;
            }
            set
            {
                if (_Dayoff == value)
                    return;
                _Dayoff = value;
            }
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

        public Cl_Variables()
        {
            //
            // TODO: Add constructor logic here
            //

            _consignmentNo = string.Empty;
            _AccountNo = string.Empty;
            _RiderCode = string.Empty;
            _Con_Type = 0;
            _Destination = string.Empty;
            _ServiceType = string.Empty;
            _Weight = 0;
            _Unit = 0;
            _Discount = 0;
            _pieces = 0;
            _Consignee = string.Empty;
            _Consigner = string.Empty;
            _ConsignerCell = string.Empty;
            _ConsignerCNIC = string.Empty;
            _ConsignerPhone = string.Empty;
            _ConsigneeCell = string.Empty;
            _ConsigneeCNIC = string.Empty;
            _CouponNo = string.Empty;
            _FlyerType = string.Empty;
            _IsCodIsInsurance = string.Empty;
            _Remarks = string.Empty;
            _RemarksID = string.Empty;
            _ConsigneeAddress = string.Empty;
            _ConsignerAddress = string.Empty;
            _Declaredvalue = 0;
            _Bookingdate = DateTime.Now;
            _origin = string.Empty;
            _Insurance = string.Empty;
            _Othercharges = 0;
            _Day = string.Empty;
            _Branch = string.Empty;
            _Zone = string.Empty;
            _expresscenter = string.Empty;
            _destinationExpressCenter = string.Empty;
            _Flyer = string.Empty;
            _RefNo = string.Empty;
            _destination = string.Empty;
            _clientID = string.Empty;
            _chargeAmount = 0;
            _totalAmount = 0;
            _gst = 0;
            _tariff = 0;
            _customertype = 0;
            _ServiceTypeName = string.Empty;
            _isCod = false;
            _createdBy = string.Empty;
            _status = 0;
            _isCreatedFromMMUs = false;
            _deliveryType = 0;
            _remarks = string.Empty;
            _shipperAddress = string.Empty;
            _riderCode = string.Empty;
            _width = 0;
            _breadth = 0;
            _height = 0;
            _PakageContents = string.Empty;
            _expressionDeliveryDateTime = DateTime.Now;
            _expressionGreetingCard = false;
            _expressionMessage = string.Empty;
            _expressionconsignmentRefNumber = string.Empty;
            _routeCode = string.Empty;
            _docPouchNo = string.Empty;
            _consignerAccountNo = string.Empty;
            _consignerEmail = string.Empty;
            _docIsHomeDelivery = false;
            _cutOffTime = DateTime.Now;
            _destinationCountryCode = string.Empty;
            _insuarancePercentage = 0;
            _isInsured = false;
            _isReturned = false;
            _cutOffTimeShift = DateTime.Now;
            _cnClientType = 1;
            _destinationExpressCenterCode = "0";
            _dayType = 1;
            _receivedFromRider = false;
            _cnState = "0";
            _cardRefNo = "0";
            _manifestNo = string.Empty;
            _manifestId = 0;
            _originBrName = string.Empty;
            _orderRefNo = string.Empty;
            _customerName = string.Empty;
            _productTypeId = 0;
            _productDescription = string.Empty;
            _chargeCODAmount = false;
            _codAmount = 0;
            _calculatedCodAmount = 0;
            _isPM = false;
            _LstModifiersCNt = null;
            _serviceTypeId = "0";
            _originId = "0";
            _destId = "0";
            _cnTypeId = "0";
            _cnOperationalType = "0";
            _cnScreenId = "0";
            _cnStatus = "0";
            _flagSendConsigneeMsg = string.Empty;
            _isExpUser = "0";
            _expresssion = null;
            _CityCode = "";
            _Company = "";
            _ServiceTypeCategory = "";
            _isInternational = false;
            _commissionID = "";
            _isFranchised = false;
            _currencyCode = "";
            _currencyID = "";
            _currencyName = "";
            _currencySymbol = "";
            _FromWeight = "";
            _ToWeight = "";
            _AdditionalFactor = "0";
            _tariffID = "";
            _FromCat = "";
            _ToCat = "";
            _OriginExpressCenterCode = "";
            _VoidConsignment = false;
            _BranchName = "";
            _BagNumber = "";
            _SealNumber = "";

            _LoadingDate = "";
            _LoadingID = "";
            _UnloadingDate = "";
            _UnloadingID = "";

            _NormalBags = new List<string>();
            _ShortReceivedBags = new List<string>();
            _ReceivedManifests = new List<string>();
            _ShortManifests = new List<string>();
            _ShortManifestRemarks = new List<string>();
            _CheckCondition = string.Empty;
            _BagManifestID = new List<string>();
            _RunSheet = "";
            _ClvarListStr = new List<string>();
            _ClvarListInt = new List<Int64>();
            _RunSheetDate = "";
            _RouteDesc = "";
            _RunSheetType = "";
            _RunSheetTypeID = "";
            _CreditClientID = "";
            _RefNumber = "";
            _StateID = "";
            _ToDate = new DateTime();
            _FromDate = new DateTime();
            _ReceiptNo = string.Empty;
            _VoucherNo = string.Empty;
            _VoucherDate = string.Empty;
            _Bank = string.Empty;
            _PaymentSource = string.Empty;

            _ChequeNo = string.Empty;
            _ChequeDate = string.Empty;
            _ClientGroupID = string.Empty;
            _PaymentType = string.Empty;
            _StaffType = string.Empty;


            _BookingDate = string.Empty;
            _CatID = "";
            _ExcessBags = new List<string>();
        }

        public string NoteNumber
        {
            get
            {
                return _NoteNumber;
            }
            set
            {
                if (_NoteNumber == value)
                    return;
                _NoteNumber = value;

            }
        }

        public string NoteType
        {
            get
            {
                return _NoteType;
            }
            set
            {
                if (_NoteType == value)
                    return;
                _NoteType = value;

            }
        }

        public string consignmentNo
        {
            get
            {
                return _consignmentNo;
            }
            set
            {
                if (_consignmentNo == value)
                    return;
                _consignmentNo = value;
            }
        }

        public string AccountNo
        {
            get
            {
                return _AccountNo;
            }
            set
            {
                if (_AccountNo == value)
                    return;
                _AccountNo = value;
            }
        }

        public string RiderCode
        {
            get
            {
                return _RiderCode;
            }
            set
            {
                if (_RiderCode == value)
                    return;
                _RiderCode = value;
            }
        }

        public int Con_Type
        {
            get
            {
                return _Con_Type;
            }
            set
            {
                if (_Con_Type == value)
                    return;
                _Con_Type = value;
            }
        }

        public string Destination
        {
            get
            {
                return _Destination;
            }
            set
            {
                if (_Destination == value)
                    return;
                _Destination = value;
            }
        }

        public string ServiceType
        {
            get
            {
                return _ServiceType;
            }
            set
            {
                if (_ServiceType == value)
                    return;
                _ServiceType = value;
            }
        }

        public float Weight
        {
            get
            {
                return _Weight;
            }
            set
            {
                if (_Weight == value)
                    return;
                _Weight = value;
            }
        }

        public int Unit
        {
            get
            {
                return _Unit;
            }
            set
            {
                if (_Unit == value)
                    return;
                _Unit = value;
            }
        }

        public int Discount
        {
            get
            {
                return _Discount;
            }
            set
            {
                if (_Discount == value)
                    return;
                _Discount = value;
            }
        }

        public int pieces
        {
            get
            {
                return _pieces;
            }
            set
            {
                if (_pieces == value)
                    return;
                _pieces = value;
            }
        }

        public string Consignee
        {
            get
            {
                return _Consignee;
            }
            set
            {
                if (_Consignee == value)
                    return;
                _Consignee = value;
            }
        }

        public string Consigner
        {
            get
            {
                return _Consigner;
            }
            set
            {
                if (_Consigner == value)
                    return;
                _Consigner = value;
            }
        }

        public string ConsignerCell
        {
            get
            {
                return _ConsignerCell;
            }
            set
            {
                if (_ConsignerCell == value)
                    return;
                _ConsignerCell = value;
            }
        }

        public string ConsigneeCell
        {
            get
            {
                return _ConsigneeCell;
            }
            set
            {
                if (_ConsigneeCell == value)
                    return;
                _ConsigneeCell = value;
            }
        }

        public string ConsignerCNIC
        {
            get
            {
                return _ConsignerCNIC;
            }
            set
            {
                if (_ConsignerCNIC == value)
                    return;
                _ConsignerCNIC = value;
            }
        }

        public string ConsigneeCNIC
        {
            get
            {
                return _ConsigneeCNIC;
            }
            set
            {
                if (_ConsigneeCNIC == value)
                    return;
                _ConsigneeCNIC = value;
            }
        }

        public string CouponNo
        {
            get
            {
                return _CouponNo;
            }
            set
            {
                if (_CouponNo == value)
                    return;
                _CouponNo = value;
            }
        }

        public string FlyerType
        {
            get
            {
                return _FlyerType;
            }
            set
            {
                if (_FlyerType == value)
                    return;
                _FlyerType = value;
            }
        }

        public String IsCodIsInsurance
        {
            get
            {
                return _IsCodIsInsurance;
            }
            set
            {
                if (_IsCodIsInsurance == value)
                    return;
                _IsCodIsInsurance = value;
            }
        }

        public string Remarks
        {
            get
            {
                return _Remarks;
            }
            set
            {
                if (_Remarks == value)
                    return;
                _Remarks = value;
            }
        }

        public string ConsigneeAddress
        {
            get
            {
                return _ConsigneeAddress;
            }
            set
            {
                if (_ConsigneeAddress == value)
                    return;
                _ConsigneeAddress = value;
            }
        }

        public string ConsignerAddress
        {
            get
            {
                return _ConsignerAddress;
            }
            set
            {
                if (_ConsignerAddress == value)
                    return;
                _ConsignerAddress = value;
            }
        }

        public DateTime Bookingdate
        {
            get
            {
                return _Bookingdate;
            }
            set
            {
                if (_Bookingdate == value)
                    return;
                _Bookingdate = value;
            }
        }

        public string origin
        {
            get
            {
                return _origin;
            }
            set
            {
                if (_origin == value)
                    return;
                _origin = value;
            }
        }

        public string Insurance
        {
            get
            {
                return _Insurance;
            }
            set
            {
                if (_Insurance == value)
                    return;
                _Insurance = value;
            }
        }

        public float Othercharges
        {
            get
            {
                return _Othercharges;
            }
            set
            {
                if (_Othercharges == value)
                    return;
                _Othercharges = value;
            }
        }

        public string Day
        {
            get
            {
                return _Day;
            }
            set
            {
                if (_Day == value)
                    return;
                _Day = value;
            }
        }

        public float Declaredvalue
        {
            get
            {
                return _Declaredvalue;
            }
            set
            {
                if (_Declaredvalue == value)
                    return;
                _Declaredvalue = value;
            }
        }

        public string Branch
        {
            get
            {
                return _Branch;
            }
            set
            {
                if (_Branch == value)
                    return;
                _Branch = value;
            }
        }

        public string Zone
        {
            get
            {
                return _Zone;
            }
            set
            {
                if (_Zone == value)
                    return;
                _Zone = value;
            }
        }

        public string expresscenter
        {
            get
            {
                return _expresscenter;
            }
            set
            {
                if (_expresscenter == value)
                    return;
                _expresscenter = value;
            }
        }

        public string destinationExpressCenter
        {
            get
            {
                return _destinationExpressCenter;
            }
            set
            {
                if (_destinationExpressCenter == value)
                    return;
                _destinationExpressCenter = value;
            }
        }

        public string Flyer
        {
            get
            {
                return _Flyer;
            }
            set
            {
                if (_Flyer == value)
                    return;
                _Flyer = value;
            }
        }

        public string RefNo
        {
            get
            {
                return _RefNo;
            }
            set
            {
                if (_RefNo == value)
                    return;
                _RefNo = value;
            }
        }

        public string destination
        {
            get
            {
                return _destination;
            }
            set
            {
                if (_destination == value)
                    return;
                _destination = value;
            }
        }

        public string CustomerClientID
        {
            get
            {
                return _clientID;
            }
            set
            {
                if (_clientID == value)
                    return;
                _clientID = value;
            }
        }

        public double TotalAmount
        {
            get
            {
                return _totalAmount;
            }
            set
            {
                if (_totalAmount == value)
                    return;
                _totalAmount = value;
            }
        }

        public double ChargeAmount
        {
            get
            {
                return _chargeAmount;
            }
            set
            {
                if (_chargeAmount == value)
                    return;
                _chargeAmount = value;
            }
        }
        public double gst
        {
            get
            {
                return _gst;
            }
            set
            {
                if (_gst == value)
                    return;
                _gst = value;
            }
        }

        public double tariff
        {
            get
            {
                return _tariff;
            }
            set
            {
                if (_tariff == value)
                    return;
                _tariff = value;
            }
        }

        public int Customertype
        {
            get
            {
                return _customertype;
            }
            set
            {
                if (_customertype == value)
                    return;
                _customertype = value;
            }
        }

        public string ServiceTypeName
        {
            get
            {
                return _ServiceTypeName;
            }
            set
            {
                if (_ServiceTypeName == value)
                    return;
                _ServiceTypeName = value;
            }
        }

        public Boolean isCod
        {
            get
            {
                return _isCod;
            }
            set
            {
                if (_isCod == value)
                    return;
                _isCod = value;
            }
        }

        public string createdBy
        {
            get
            {
                return _createdBy;
            }
            set
            {
                if (_createdBy == value)
                    return;
                _createdBy = value;
            }
        }

        public int status
        {
            get
            {
                return _status;
            }
            set
            {
                if (_status == value)
                    return;
                _status = value;
            }
        }

        public Boolean isCreatedFromMMUs
        {
            get
            {
                return _isCreatedFromMMUs;
            }
            set
            {
                if (_isCreatedFromMMUs == value)
                    return;
                _isCreatedFromMMUs = value;
            }
        }

        public int deliveryType
        {
            get
            {
                return _deliveryType;
            }
            set
            {
                if (_deliveryType == value)
                    return;
                _deliveryType = value;
            }
        }

        public string remarks
        {
            get
            {
                return _remarks;
            }
            set
            {
                if (_remarks == value)
                    return;
                _remarks = value;
            }
        }

        public string shipperAddress
        {
            get
            {
                return _shipperAddress;
            }
            set
            {
                if (_shipperAddress == value)
                    return;
                _shipperAddress = value;
            }
        }

        public string riderCode
        {
            get
            {
                return _riderCode;
            }
            set
            {
                if (_riderCode == value)
                    return;
                _riderCode = value;
            }
        }

        public double width
        {
            get
            {
                return _width;
            }
            set
            {
                if (_width == value)
                    return;
                _width = value;
            }
        }

        public double breadth
        {
            get
            {
                return _breadth;
            }
            set
            {
                if (_breadth == value)
                    return;
                _breadth = value;
            }
        }

        public double height
        {
            get
            {
                return _height;
            }
            set
            {
                if (_height == value)
                    return;
                _height = value;
            }
        }

        public string PakageContents
        {
            get
            {
                return _PakageContents;
            }
            set
            {
                if (_PakageContents == value)
                    return;
                _PakageContents = value;
            }
        }

        public DateTime expressionDeliveryDateTime
        {
            get
            {
                return _expressionDeliveryDateTime;
            }
            set
            {
                if (_expressionDeliveryDateTime == value)
                    return;
                _expressionDeliveryDateTime = value;
            }
        }

        public Boolean expressionGreetingCard
        {
            get
            {
                return _expressionGreetingCard;
            }
            set
            {
                if (_expressionGreetingCard == value)
                    return;
                _expressionGreetingCard = value;
            }
        }

        public string expressionMessage
        {
            get
            {
                return _expressionMessage;
            }
            set
            {
                if (_expressionMessage == value)
                    return;
                _expressionMessage = value;
            }
        }

        public string expressionconsignmentRefNumber
        {
            get
            {
                return _expressionconsignmentRefNumber;
            }
            set
            {
                if (_expressionconsignmentRefNumber == value)
                    return;
                _expressionconsignmentRefNumber = value;
            }
        }

        public string routeCode
        {
            get
            {
                return _routeCode;
            }
            set
            {
                if (_routeCode == value)
                    return;
                _routeCode = value;
            }
        }

        public string docPouchNo
        {
            get
            {
                return _docPouchNo;
            }
            set
            {
                if (_docPouchNo == value)
                    return;
                _docPouchNo = value;
            }
        }

        public string consignerAccountNo
        {
            get
            {
                return _consignerAccountNo;
            }
            set
            {
                if (_consignerAccountNo == value)
                    return;
                _consignerAccountNo = value;
            }
        }

        public string consignerEmail
        {
            get
            {
                return _consignerEmail;
            }
            set
            {
                if (_consignerEmail == value)
                    return;
                _consignerEmail = value;
            }
        }

        public Boolean docIsHomeDelivery
        {
            get
            {
                return _docIsHomeDelivery;
            }
            set
            {
                if (_docIsHomeDelivery == value)
                    return;
                _docIsHomeDelivery = value;
            }
        }

        public DateTime cutOffTime
        {
            get
            {
                return _cutOffTime;
            }
            set
            {
                if (_cutOffTime == value)
                    return;
                _cutOffTime = value;
            }
        }

        public string destinationCountryCode
        {
            get
            {
                return _destinationCountryCode;
            }
            set
            {
                if (_destinationCountryCode == value)
                    return;
                _destinationCountryCode = value;
            }
        }

        public float insuarancePercentage
        {
            get
            {
                return _insuarancePercentage;
            }
            set
            {
                if (_insuarancePercentage == value)
                    return;
                _insuarancePercentage = value;
            }
        }

        public Boolean isInsured
        {
            get
            {
                return _isInsured;
            }
            set
            {
                if (_isInsured == value)
                    return;
                _isInsured = value;
            }
        }

        public Boolean isReturned
        {
            get
            {
                return _isReturned;
            }
            set
            {
                if (_isReturned == value)
                    return;
                _isReturned = value;
            }
        }

        public DateTime cutOffTimeShift
        {
            get
            {
                return _cutOffTimeShift;
            }
            set
            {
                if (_cutOffTimeShift == value)
                    return;
                _cutOffTimeShift = value;
            }
        }

        public int cnClientType
        {
            get
            {
                return _cnClientType;
            }
            set
            {
                if (_cnClientType == value)
                    return;
                _cnClientType = value;
            }
        }

        public string destinationExpressCenterCode
        {
            get
            {
                return _destinationExpressCenterCode;
            }
            set
            {
                if (_destinationExpressCenterCode == value)
                    return;
                _destinationExpressCenterCode = value;
            }
        }

        public int dayType
        {
            get
            {
                return _dayType;
            }
            set
            {
                if (_dayType == value)
                    return;
                _dayType = value;
            }
        }

        public Boolean receivedFromRider
        {
            get
            {
                return _receivedFromRider;
            }
            set
            {
                if (_receivedFromRider == value)
                    return;
                _receivedFromRider = value;
            }
        }

        public string cnState
        {
            get
            {
                return _cnState;
            }
            set
            {
                if (_cnState == value)
                    return;
                _cnState = value;
            }
        }

        public string cardRefNo
        {
            get
            {
                return _cardRefNo;
            }
            set
            {
                if (_cardRefNo == value)
                    return;
                _cardRefNo = value;
            }
        }

        public string manifestNo
        {
            get
            {
                return _manifestNo;
            }
            set
            {
                if (_manifestNo == value)
                    return;
                _manifestNo = value;
            }
        }

        public int manifestId
        {
            get
            {
                return _manifestId;
            }
            set
            {
                if (_manifestId == value)
                    return;
                _manifestId = value;
            }
        }

        public string originBrName
        {
            get
            {
                return _originBrName;
            }
            set
            {
                if (_originBrName == value)
                    return;
                _originBrName = value;
            }
        }

        public string orderRefNo
        {
            get
            {
                return _orderRefNo;
            }
            set
            {
                if (_orderRefNo == value)
                    return;
                _orderRefNo = value;
            }
        }

        public string customerName
        {
            get
            {
                return _customerName;
            }
            set
            {
                if (_customerName == value)
                    return;
                _customerName = value;
            }
        }

        public int productTypeId
        {
            get
            {
                return _productTypeId;
            }
            set
            {
                if (_productTypeId == value)
                    return;
                _productTypeId = value;
            }
        }

        public string productDescription
        {
            get
            {
                return _productDescription;
            }
            set
            {
                if (_productDescription == value)
                    return;
                _productDescription = value;
            }
        }

        public Boolean chargeCODAmount
        {
            get
            {
                return _chargeCODAmount;
            }
            set
            {
                if (_chargeCODAmount == value)
                    return;
                _chargeCODAmount = value;
            }
        }

        public float codAmount
        {
            get
            {
                return _codAmount;
            }
            set
            {
                if (_codAmount == value)
                    return;
                _codAmount = value;
            }
        }

        public float calculatedCodAmount
        {
            get
            {
                return _calculatedCodAmount;
            }
            set
            {
                if (_calculatedCodAmount == value)
                    return;
                _calculatedCodAmount = value;
            }
        }

        public Boolean isPM
        {
            get
            {
                return _isPM;
            }
            set
            {
                if (_isPM == value)
                    return;
                _isPM = value;
            }
        }

        public DataTable LstModifiersCNt
        {
            get
            {
                return _LstModifiersCNt;
            }
            set
            {
                if (_LstModifiersCNt == value)
                    return;
                _LstModifiersCNt = value;
            }
        }

        public string serviceTypeId
        {
            get
            {
                return _serviceTypeId;
            }
            set
            {
                if (_serviceTypeId == value)
                    return;
                _serviceTypeId = value;
            }
        }

        public string originId
        {
            get
            {
                return _originId;
            }
            set
            {
                if (_originId == value)
                    return;
                _originId = value;
            }
        }

        public string destId
        {
            get
            {
                return _destId;
            }
            set
            {
                if (_destId == value)
                    return;
                _destId = value;
            }
        }

        public string cnTypeId
        {
            get
            {
                return _cnTypeId;
            }
            set
            {
                if (_cnTypeId == value)
                    return;
                _cnTypeId = value;
            }
        }

        public string cnOperationalType
        {
            get
            {
                return _cnOperationalType;
            }
            set
            {
                if (_cnOperationalType == value)
                    return;
                _cnOperationalType = value;
            }
        }

        public string cnScreenId
        {
            get
            {
                return _cnScreenId;
            }
            set
            {
                if (_cnScreenId == value)
                    return;
                _cnScreenId = value;
            }
        }

        public string cnStatus
        {
            get
            {
                return _cnStatus;
            }
            set
            {
                if (_cnStatus == value)
                    return;
                _cnStatus = value;
            }
        }

        public string flagSendConsigneeMsg
        {
            get
            {
                return _flagSendConsigneeMsg;
            }
            set
            {
                if (_flagSendConsigneeMsg == value)
                    return;
                _flagSendConsigneeMsg = value;
            }
        }

        public string isExpUser
        {
            get
            {
                return _isExpUser;
            }
            set
            {
                if (_isExpUser == value)
                    return;
                _isExpUser = value;
            }
        }

        public string ConsignerPhone
        {
            get
            {
                return _ConsignerPhone;
            }
            set
            {
                if (_ConsignerPhone == value)
                    return;
                _ConsignerPhone = value;
            }
        }

        public DataTable expresssion
        {
            get
            {
                return _expresssion;
            }
            set
            {
                if (_expresssion == value)
                    return;
                _expresssion = value;
            }
        }

        public int carier
        {
            get
            {
                return _carier;
            }
            set
            {
                if (_carier == value)
                    return;
                _carier = value;
            }
        }

        public int mode
        {
            get
            {
                return _mode;
            }
            set
            {
                if (_mode == value)
                    return;
                _mode = value;
            }
        }

        public int term
        {
            get
            {
                return _term;
            }
            set
            {
                if (_term == value)
                    return;
                _term = value;
            }
        }

        public int subServicetype
        {
            get
            {
                return _subServicetype;
            }
            set
            {
                if (_subServicetype == value)
                    return;
                _subServicetype = value;
            }
        }

        public string subdetai
        {
            get
            {
                return _subdetai;
            }
            set
            {
                if (_subdetai == value)
                    return;
                _subdetai = value;
            }
        }

        public int Currency
        {
            get
            {
                return _Currency;
            }
            set
            {
                if (_Currency == value)
                    return;
                _Currency = value;
            }
        }

        public double amount
        {
            get
            {
                return _amount;
            }
            set
            {
                if (_amount == value)
                    return;
                _amount = value;
            }
        }

        public string instruction
        {
            get
            {
                return _instruction;
            }
            set
            {
                if (_instruction == value)
                    return;
                _instruction = value;
            }
        }

        public string destinationCity
        {
            get
            {
                return _destinationCity;
            }
            set
            {
                if (_destinationCity == value)
                    return;
                _destinationCity = value;
            }
        }

        public int postalCode
        {
            get
            {
                return _postalCode;
            }
            set
            {
                if (_postalCode == value)
                    return;
                _postalCode = value;
            }
        }

        public string consigneeAttention
        {
            get
            {
                return _consigneeAttention;
            }
            set
            {
                if (_consigneeAttention == value)
                    return;
                _consigneeAttention = value;
            }
        }

        public int destinationCitycode
        {
            get
            {
                return _destinationCitycode;
            }
            set
            {
                if (_destinationCitycode == value)
                    return;
                _destinationCitycode = value;
            }
        }

        public DateTime deliveryDate
        {
            get
            {
                return _deliveryDate;
            }
            set
            {
                if (_deliveryDate == value)
                    return;
                _deliveryDate = value;
            }
        }

        public string ToZoneCode
        {
            get
            {
                return _ToZoneCode;
            }
            set
            {
                if (_ToZoneCode == value)
                    return;
                _ToZoneCode = value;
            }
        }

        public string FromZoneCode
        {
            get
            {
                return _FromZoneCode;
            }
            set
            {
                if (_FromZoneCode == value)
                    return;
                _FromZoneCode = value;
            }
        }

        public string BaseCurrency
        {
            get
            {
                return _BaseCurrency;
            }
            set
            {
                if (_BaseCurrency == value)
                    return;
                _BaseCurrency = value;
            }
        }

        public string ToCurrency
        {
            get
            {
                return _ToCurrency;
            }
            set
            {
                if (_ToCurrency == value)
                    return;
                _ToCurrency = value;
            }
        }

        public string Error
        {
            get
            {
                return _Error;
            }
            set
            {
                if (_Error == value)
                    return;
                _Error = value;
            }
        }

        public string CityCode
        {
            get { return _CityCode; }
            set { _CityCode = value; }
        }

        public string Company
        {
            get { return _Company; }
            set { _Company = value; }
        }

        public string ServiceTypeCategory
        {
            get { return _ServiceTypeCategory; }
            set { _ServiceTypeCategory = value; }
        }

        public Boolean IsInternational
        {
            get { return _isInternational; }
            set { _isInternational = value; }
        }

        public string CommissionID
        {
            get { return _commissionID; }
            set { _commissionID = value; }
        }

        public Boolean IsFranchised
        {
            get { return _isFranchised; }
            set { _isFranchised = value; }
        }

        public string CurrencyName
        {
            get { return _currencyName; }
            set { _currencyName = value; }
        }

        public string CurrencyCode
        {
            get { return _currencyCode; }
            set { _currencyCode = value; }
        }

        public string CurrencySymbol
        {
            get { return _currencySymbol; }
            set { _currencySymbol = value; }
        }

        public string CurrencyID
        {
            get { return _currencyID; }
            set { _currencyID = value; }
        }

        public string FromWeight
        {
            get { return _FromWeight; }
            set { _FromWeight = value; }
        }

        public string ToWeight
        {
            get { return _ToWeight; }
            set { _ToWeight = value; }
        }

        public string AdditionalFactor
        {
            get { return _AdditionalFactor; }
            set { _AdditionalFactor = value; }
        }

        public string TariffID
        {
            get { return _tariffID; }
            set { _tariffID = value; }
        }

        public string FromCat
        {
            get { return _FromCat; }
            set { _FromCat = value; }
        }

        public string ToCat
        {
            get { return _ToCat; }
            set { _ToCat = value; }
        }

        public string OriginExpressCenterCode
        {
            get { return _OriginExpressCenterCode; }
            set { _OriginExpressCenterCode = value; }
        }

        public Boolean VoidConsignment
        {
            get { return _VoidConsignment; }
            set { _VoidConsignment = value; }
        }
        public string BranchName
        {
            get { return _BranchName; }
            set { _BranchName = value; }
        }
        public string BagNumber
        {
            get { return _BagNumber; }
            set { _BagNumber = value; }
        }
        public string SealNumber
        {
            get { return _SealNumber; }
            set { _SealNumber = value; }
        }

        public string RemarksID
        {
            get { return _RemarksID; }
            set { _RemarksID = value; }
        }


        public string LoadingID
        {
            get { return _LoadingID; }
            set { _LoadingID = value; }
        }

        public string UnloadingID
        {
            get { return _UnloadingID; }
            set { _UnloadingID = value; }
        }

        public string LoadingDate
        {
            get { return _LoadingDate; }
            set { _LoadingDate = value; }
        }

        public string UnloadingDate
        {
            get { return _UnloadingDate; }
            set { _UnloadingDate = value; }
        }

        public List<string> NormalBags
        {
            get { return _NormalBags; }
            set { _NormalBags = value; }
        }

        public List<string> ShortReceivedBags
        {
            get { return _ShortReceivedBags; }
            set { _ShortReceivedBags = value; }
        }

        public List<string> ReceivedManifests
        {
            get { return _ReceivedManifests; }
            set { _ReceivedManifests = value; }
        }
        public List<string> ShortManifests
        {
            get { return _ShortManifests; }
            set { _ShortManifests = value; }
        }
        public List<string> ShortManifestRemarks
        {
            get { return _ShortManifestRemarks; }
            set { _ShortManifestRemarks = value; }
        }
        public string CheckCondition
        {
            get { return _CheckCondition; }
            set { _CheckCondition = value; }
        }
        public List<string> BagManifestID
        {
            get { return _BagManifestID; }
            set { _BagManifestID = value; }
        }

        public List<string> ClvarListStr
        {
            get { return _ClvarListStr; }
            set { _ClvarListStr = value; }
        }
        public List<Int64> ClvarListInt
        {
            get { return _ClvarListInt; }
            set { _ClvarListInt = value; }
        }

        public string RunSheetDate
        {
            get { return _RunSheetDate; }
            set { _RunSheetDate = value; }
        }
        public string RouteDesc
        {
            get { return _RouteDesc; }
            set { _RouteDesc = value; }
        }

        public string RunSheetType
        {
            get { return _RunSheetType; }
            set { _RunSheetType = value; }
        }

        public string RunSheetTypeID
        {
            get { return _RunSheetTypeID; }
            set { _RunSheetTypeID = value; }
        }

        public string CreditClientID
        {
            get { return _CreditClientID; }
            set { _CreditClientID = value; }
        }

        public string RefNumber
        {
            get { return _RefNumber; }
            set { _RefNumber = value; }
        }

        public string StateID
        {
            get { return _StateID; }
            set { _StateID = value; }
        }

        public DateTime FromDate
        {
            get { return _FromDate; }
            set { _FromDate = value; }
        }

        public DateTime ToDate
        {
            get { return _ToDate; }
            set { _ToDate = value; }
        }

        public string ReceiptNo
        {
            get { return _ReceiptNo; }
            set { _ReceiptNo = value; }
        }

        public string VoucherNo
        {
            get { return _VoucherNo; }
            set { _VoucherNo = value; }
        }

        public string VoucherDate
        {
            get { return _VoucherDate; }
            set { _VoucherDate = value; }
        }


        public string ChequeDate
        {
            get { return _ChequeDate; }
            set { _ChequeDate = value; }
        }

        public string ChequeNo
        {
            get { return _ChequeNo; }
            set { _ChequeNo = value; }
        }

        public string PaymentSource
        {
            get { return _PaymentSource; }
            set { _PaymentSource = value; }
        }

        public string Bank
        {
            get { return _Bank; }
            set { _Bank = value; }
        }

        public string ClientGroupID
        {
            get { return _ClientGroupID; }
            set { _ClientGroupID = value; }
        }
        public string PaymentType
        {
            get { return _PaymentType; }
            set { _PaymentType = value; }
        }
        public string StaffType
        {
            get { return _StaffType; }
            set { _StaffType = value; }
        }


        public string BookingDate
        {
            set
            {
                _BookingDate = value;
            }
            get
            {
                return _BookingDate;
            }
        }

        public string RunsheetNumber
        {
            set
            {
                _RunsheetNumber = value;
            }
            get
            {
                return _RunsheetNumber;
            }
        }
        public string RunSheetNumber
        {
            get { return _RunsheetNumber; }
            set { _RunsheetNumber = value; }
        }

        public List<string> ExcessBags
        {
            get { return _ExcessBags; }
            set { _ExcessBags = value; }
        }
    }
}