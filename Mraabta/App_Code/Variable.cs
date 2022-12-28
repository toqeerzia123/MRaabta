using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Bayer_Variable
/// </summary>
/// 
namespace MRaabta.App_Code
{
    public class Variable
    {
        public string _Year;
        public string _Principal;
        public string _Zone;
        public string _Detail;
        public string _division;
        public string _TableName;
        public string _Depot;
        public string _Price_Type;
        public string _check_condition;
        public string _check_condition1;
        public string _check_condtion2;
        public string _check_condition3;
        public string _check_condition4;
        public string _session_div;
        public string _session_prn;
        public string _session_zone;
        public string _session_depot;
        public int[] Zone_Value = new int[] { 1, 2, 3, 4, 5 };
        public string _Zone_Type;
        public string _Month;
        public string _Value_Type;
        public string _prdcd;
        public string _daymon;
        public string _division_id;
        public string _invoiceNo;
        public string _customerNo;
        public string _salegroup;
        public string _AreaManagerID;
        public string _AreaManager;
        public string _FMO;
        public string _BranchManagerID;
        public string _BranchManager;
        public string _status;
        public string _BayerFmobyTeam;
        public string _TownCode;
        public string _UserName;
        public string _password;
        public string _SalesManager;
        public string _BusinessHead;
        public string _Team;
        public string _previousyearmonth;
        public string _previousyear;
        public DateTime _Date_time;
        public DateTime _End_Date;
        public string _FromMonth;
        public string _ToMonth;
        public string _SalesGroup;
        public int _previous_year;
        public string _brand;
        public string _menuname;
        public string _childmenu;
        public string _hyperlink;
        public string _reportname;
        public string _reportid;
        public string _Expresscentercode;
        public string _ProfileName;
        public string _Id;
        public string _Type;
        public string _DesignationCode;
        public string _ExcelPermission;
        public string _UserId;
        public string _MACAddress;
        public string _Department;
        public string _Designation;
        public string _Responsible;
        public string _ReportId;
        public string _PaymentSource;
        public string _StartDate;
        public string _EndDate;
        public string _Query;
        public string _CNNumber;
        public string _ACNumber;
        public string _Remarks;
        public string _Version;
        public string _Request;
        public string _enddate2;
        public string _Zone2;
        public string _CityCode;
        public string _Amount;
        public string _Services;
        private string _SalesGroupID;
        public string _ShiftTime;

        /*
        // BTS 
        public string _ModifierName;
        public string _CalculationValue;
        public string _CalculationBase;
        public string _BillingModifier;
        public string _Code;
        public string _Category;
        public string _Rate;
        public string _GST;
        public string _ServiceCharges;
        public string _Cost;
        */

        // BTS 
        public string _ModifierName;
        public string _CalculationValue;
        public string _CalculationBase;
        public string _BillingModifier;
        public string _Code;
        public string _Category;
        public string _Rate;
        public string _GST;
        public string _ServiceCharges;
        public string _Cost;
        public string _RiderCode;
        public string _ConsignmentNo;
        public string _RiderName;
        public string _Weight;
        public string _ConsignmentType;
        public string _Manifest;
        public string _ManifestType;
        public string _Destination;
        public string _Orign;
        public string _BagNumber;
        public string _Seal;
        public string _Route;
        public string _TouchPoint;
        public string _TransportType;
        public string _Vehicle;
        public string _RegNo;
        public string _CourierName;
        public string _VehicleId;
        public string _Description;
        public string _LoadingId;

        private Int64 _ArrivalID;

        public string _StateId;
        public string _MawbNumber;
        public string _RunsheetNumber;

        public string _OldUserName;
        public string _FlightNo;
        public string _FlightDepartureDate;
        public string _VehicleNo;



        //  private string _SalesGroupID;


        public Variable()
        {
            _Year = string.Empty;
            _Principal = string.Empty;
            _Zone = string.Empty;
            _Detail = string.Empty;
            _division = string.Empty;
            _Depot = string.Empty;
            _TableName = string.Empty;
            _Price_Type = string.Empty;
            _check_condition = string.Empty;
            _check_condition1 = string.Empty;
            _check_condtion2 = string.Empty;
            _check_condition3 = string.Empty;
            _check_condition4 = string.Empty;
            _Zone_Type = string.Empty;
            _Month = string.Empty;
            _Value_Type = string.Empty;
            _prdcd = string.Empty;
            _daymon = string.Empty;
            _invoiceNo = string.Empty;
            _customerNo = string.Empty;
            _AreaManager = string.Empty;
            _AreaManagerID = string.Empty;
            _FMO = string.Empty;
            _BranchManager = string.Empty;
            _BranchManagerID = string.Empty;
            _status = string.Empty;
            _BayerFmobyTeam = string.Empty;
            _TownCode = string.Empty;
            _UserName = string.Empty;
            _password = string.Empty;
            _SalesManager = string.Empty;
            _BusinessHead = string.Empty;
            _Team = string.Empty;
            _previousyearmonth = string.Empty;
            _previousyear = string.Empty;
            _Date_time = DateTime.Now.Date;
            _End_Date = DateTime.Now;
            _FromMonth = string.Empty;
            _ToMonth = string.Empty;
            _SalesGroup = string.Empty;
            _previous_year = 0;
            _brand = string.Empty;
            _SalesGroupID = string.Empty;
            _menuname = string.Empty;
            _hyperlink = string.Empty;
            _childmenu = string.Empty;
            _reportname = string.Empty;
            _reportid = string.Empty;
            _Expresscentercode = string.Empty;
            _ProfileName = string.Empty;
            _Id = string.Empty;
            _Type = string.Empty;
            _DesignationCode = string.Empty;
            _ExcelPermission = string.Empty;
            _UserId = string.Empty;
            _MACAddress = string.Empty;
            _Department = string.Empty;
            _Designation = string.Empty;
            _Responsible = string.Empty;
            _ReportId = string.Empty;
            _PaymentSource = string.Empty;
            _StartDate = string.Empty;
            _EndDate = string.Empty;
            _Query = string.Empty;
            _CNNumber = string.Empty;
            _ACNumber = string.Empty;
            _Remarks = string.Empty;
            _Version = string.Empty;
            _Request = string.Empty;
            _enddate2 = string.Empty;
            _Zone2 = string.Empty;
            _CityCode = string.Empty;
            _Amount = string.Empty;
            _Services = string.Empty;

            /*
            // BTS
            _ModifierName = string.Empty;
            _CalculationValue = string.Empty;
            _CalculationBase = string.Empty;
            _Code = string.Empty;
            _Category = string.Empty;
            _Rate = string.Empty;
            _GST = string.Empty;
            _ServiceCharges = string.Empty;
            _Cost = string.Empty;
            */

            // BTS
            _ModifierName = string.Empty;
            _CalculationValue = string.Empty;
            _CalculationBase = string.Empty;
            _Code = string.Empty;
            _Category = string.Empty;
            _Rate = string.Empty;
            _GST = string.Empty;
            _ServiceCharges = string.Empty;
            _Cost = string.Empty;
            _RiderCode = string.Empty;
            _ConsignmentNo = string.Empty;
            _RiderName = string.Empty;
            _Weight = string.Empty;
            _ConsignmentType = string.Empty;
            _Manifest = string.Empty;
            _ManifestType = string.Empty;
            _Destination = string.Empty;
            _Orign = string.Empty;
            _BagNumber = string.Empty;
            _Seal = string.Empty;
            _Route = string.Empty;
            _TouchPoint = string.Empty;
            _TransportType = string.Empty;
            _Vehicle = string.Empty;
            _RegNo = string.Empty;
            _CourierName = string.Empty;
            _VehicleId = string.Empty;
            _Description = string.Empty;
            _LoadingId = string.Empty;

            _ArrivalID = 0;
            _StateId = string.Empty;
            _MawbNumber = string.Empty;
            _RunsheetNumber = string.Empty;
            _OldUserName = string.Empty;

            _FlightNo = string.Empty;
            _FlightDepartureDate = string.Empty;
            _VehicleNo = string.Empty;



        }

        public string Strcon()
        {
            string QueryString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            return QueryString;
        }

        public string StrconLive()
        {
            string QueryString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            return QueryString;
        }

        public string Strcon2()
        {
            string QueryString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            return QueryString;
        }

        public string Strcon_SNDQA()
        {
            string QueryString = System.Configuration.ConfigurationManager.ConnectionStrings["Sndqa_ConnectionString"].ToString();
            return QueryString;
        }

        public string Strcon_Telecom()
        {
            string QueryString = System.Configuration.ConfigurationManager.ConnectionStrings["Telecom_ConnectionString"].ToString();
            return QueryString;
        }

        public string Strcon_Bayer()
        {
            string QueryString = System.Configuration.ConfigurationManager.ConnectionStrings["Bayer_ConnectionString"].ToString();
            return QueryString;
        }

        public string Strcon_SndPTF()
        {
            string QueryString = System.Configuration.ConfigurationManager.ConnectionStrings["Sndptf_ConnectionString"].ToString();
            return QueryString;
        }

        public string Strcon_Zni()
        {
            string QueryString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString_zni"].ToString();
            return QueryString;
        }

        public string Team
        {
            set
            {
                _Team = value;
            }
            get
            {
                return _Team;
            }
        }

        public string Year
        {
            set
            {
                _Year = value;
            }
            get
            {
                return _Year;
            }
        }

        public string Principal
        {
            set
            {
                _Principal = value;
            }
            get
            {
                return _Principal;
            }
        }

        public string Zone
        {
            set
            {
                _Zone = value;
            }
            get
            {
                return _Zone;
            }
        }

        public string Detail
        {
            set
            {
                _Detail = value;
            }
            get
            {
                return _Detail;
            }
        }


        public string Division
        {
            set
            {
                _division = value;
            }
            get
            {
                return _division;
            }
        }

        public string Depot
        {
            set
            {
                _Depot = value;
            }
            get
            {
                return _Depot;
            }
        }

        public string UserName
        {
            set
            {
                _UserName = value;
            }
            get
            {
                return _UserName;
            }
        }

        public string Password
        {
            set
            {
                _password = value;
            }
            get
            {
                return _password;
            }
        }

        public string TableName
        {
            set
            {
                _TableName = value;
            }
            get
            {
                return _TableName;
            }
        }

        public string Price_Type
        {
            set
            {
                _Price_Type = value;
            }
            get
            {
                return _Price_Type;
            }
        }

        public string Check_Condition
        {
            set
            {
                _check_condition = value;
            }
            get
            {
                return _check_condition;
            }
        }

        public string Check_Condition1
        {
            set
            {
                _check_condition1 = value;
            }
            get
            {
                return _check_condition1;
            }
        }

        public string Check_Condition2
        {
            set
            {
                _check_condtion2 = value;
            }
            get
            {
                return _check_condtion2;
            }
        }

        public string Check_Condition3
        {
            set
            {
                _check_condition3 = value;
            }
            get
            {
                return _check_condition3;
            }
        }

        public string Check_Condition4
        {
            set
            {
                _check_condition4 = value;
            }
            get
            {
                return _check_condition4;
            }
        }

        public string Zone_Type
        {
            set
            {
                _Zone_Type = value;
            }
            get
            {
                return _Zone_Type;
            }
        }

        public string Month
        {
            set
            {
                _Month = value;
            }
            get
            {
                return _Month;
            }
        }

        public string Value_Type
        {
            set
            {
                _Value_Type = value;
            }
            get
            {
                return _Value_Type;
            }
        }

        public string Prdcd
        {
            set
            {
                _prdcd = value;
            }
            get
            {
                return _prdcd;
            }
        }

        public string DayMon
        {
            set
            {
                _daymon = value;
            }
            get
            {
                return _daymon;
            }
        }

        public string Division_ID
        {
            set
            {
                _division_id = value;
            }
            get
            {
                return _division_id;
            }
        }

        public string InvoiceNo
        {
            set
            {
                _invoiceNo = value;
            }
            get
            {
                return _invoiceNo;
            }
        }

        public string CustomerNo
        {
            set
            {
                _customerNo = value;
            }
            get
            {
                return _customerNo;
            }
        }

        public string SalesGroup
        {
            set
            {
                _salegroup = value;
            }
            get
            {
                return _salegroup;
            }
        }

        public string AreaManager
        {
            set
            {
                _AreaManager = value;
            }
            get
            {
                return _AreaManager;
            }
        }

        public string AreaManagerID
        {
            set
            {
                _AreaManagerID = value;
            }
            get
            {
                return _AreaManagerID;
            }
        }

        public string FMO
        {
            set
            {
                _FMO = value;
            }
            get
            {
                return _FMO;
            }
        }

        public string BranchManager
        {
            set
            {
                _BranchManager = value;
            }
            get
            {
                return _BranchManager;
            }
        }

        public string BranchManagerID
        {
            set
            {
                _BranchManagerID = value;
            }
            get
            {
                return _BranchManagerID;
            }
        }

        public string Status
        {
            set
            {
                _status = value;
            }
            get
            {
                return _status;
            }
        }

        public string BayerFMOByTeam
        {
            set
            {
                _BayerFmobyTeam = value;
            }
            get
            {
                return _BayerFmobyTeam;
            }
        }

        public string TownCode
        {
            set
            {
                _TownCode = value;
            }
            get
            {
                return _TownCode;
            }
        }

        public string SalesManager
        {
            set
            {
                _SalesManager = value;
            }
            get
            {
                return _SalesManager;
            }
        }

        public string BusinessHead
        {
            set
            {
                _BusinessHead = value;
            }
            get
            {
                return _BusinessHead;
            }
        }

        public string PreviousYearMonth
        {
            set
            {
                _previousyearmonth = value;
            }
            get
            {
                return _previousyearmonth;
            }
        }

        public string PreviousYear
        {
            set
            {
                _previousyear = value;

            }
            get
            {
                return _previousyear;
            }
        }

        public DateTime Date_Time
        {
            set
            {
                _Date_time = value;
            }
            get
            {
                return _Date_time;
            }
        }

        public DateTime End_Date
        {
            set
            {
                _End_Date = value;
            }
            get
            {
                return _End_Date;
            }
        }

        public string FromMonth
        {
            set
            {
                _FromMonth = value;
            }
            get
            {
                return _FromMonth;
            }
        }

        public string ToMonth
        {
            set
            {
                _ToMonth = value;
            }
            get
            {
                return _ToMonth;
            }

        }

        public int Previous_Year
        {
            set
            {
                _previous_year = 0;
            }
            get
            {
                return _previous_year;
            }
        }

        public string Brand
        {
            set
            {
                _brand = value;
            }
            get
            {
                return _brand;
            }
        }

        public string SaleGroupID
        {
            set
            {
                _SalesGroupID = value;
            }
            get
            {
                return _SalesGroupID;
            }
        }


        public string MenuName
        {
            set
            {
                _menuname = value;
            }
            get
            {
                return _menuname;
            }
        }

        public string Hyperlink
        {
            set
            {
                _hyperlink = value;
            }
            get
            {
                return _hyperlink;
            }
        }


        public string Childmenu
        {
            set
            {
                _childmenu = value;
            }
            get
            {
                return _childmenu;
            }
        }

        public string Reportname
        {
            set
            {
                _reportname = value;
            }
            get
            {
                return _reportname;
            }
        }

        public string Reportid
        {
            set
            {
                _reportid = value;
            }
            get
            {
                return _reportid;
            }
        }

        public string Expresscentercode
        {
            set
            {
                _Expresscentercode = value;
            }
            get
            {
                return _Expresscentercode;
            }
        }

        public string ProfileName
        {
            set
            {
                _ProfileName = value;
            }
            get
            {
                return _ProfileName;
            }
        }

        public string Id
        {
            set
            {
                _Id = value;
            }
            get
            {
                return _Id;
            }
        }

        public string Type
        {
            set
            {
                _Type = value;
            }
            get
            {
                return _Type;
            }
        }

        public string DesignationCode
        {
            set
            {
                _DesignationCode = value;
            }
            get
            {
                return _DesignationCode;
            }
        }

        public string ExcelPermission
        {
            set
            {
                _ExcelPermission = value;
            }
            get
            {
                return _ExcelPermission;
            }
        }

        public string UserId
        {
            set
            {
                _UserId = value;
            }
            get
            {
                return _UserId;
            }
        }

        public string MACAddress
        {
            set
            {
                _MACAddress = value;
            }
            get
            {
                return _MACAddress;
            }
        }

        public string Department
        {
            set
            {
                _Department = value;
            }
            get
            {
                return _Department;
            }
        }

        public string Designation
        {
            set
            {
                _Designation = value;
            }
            get
            {
                return _Designation;
            }
        }

        public string Responsible
        {
            set
            {
                _Responsible = value;
            }
            get
            {
                return _Responsible;
            }
        }

        public string ReportId
        {
            set
            {
                _ReportId = value;
            }
            get
            {
                return _ReportId;
            }
        }

        public string PaymentSource
        {
            set
            {
                _PaymentSource = value;
            }
            get
            {
                return _PaymentSource;
            }
        }

        public string StartDate
        {
            set
            {
                _StartDate = value;
            }
            get
            {
                return _StartDate;
            }
        }

        public string EndDate
        {
            set
            {
                _EndDate = value;
            }
            get
            {
                return _EndDate;
            }
        }

        public string Query
        {
            set
            {
                _Query = value;
            }
            get
            {
                return _Query;
            }
        }

        public string CNNumber
        {
            set
            {
                _CNNumber = value;
            }
            get
            {
                return _CNNumber;
            }
        }

        public string ACNumber
        {
            set
            {
                _ACNumber = value;
            }
            get
            {
                return _ACNumber;
            }
        }

        public string Remarks
        {
            set
            {
                _Remarks = value;
            }
            get
            {
                return _Remarks;
            }
        }

        public string Version
        {
            set
            {
                _Version = value;
            }
            get
            {
                return _Version;
            }
        }

        public string Request
        {
            set
            {
                _Request = value;
            }
            get
            {
                return _Request;
            }
        }

        public string Enddate2
        {
            set
            {
                _enddate2 = value;
            }
            get
            {
                return _enddate2;
            }
        }

        public string Zone2
        {
            set
            {
                _Zone2 = value;
            }
            get
            {
                return _Zone2;
            }
        }

        public string CityCode
        {
            set
            {
                _CityCode = value;
            }
            get
            {
                return _CityCode;
            }
        }

        public string Amount
        {
            set
            {
                _Amount = value;
            }
            get
            {
                return _Amount;
            }
        }

        public string Services
        {
            set
            {
                _Services = value;
            }
            get
            {
                return _Services;
            }
        }









        public string ModifierName
        {
            set
            {
                _ModifierName = value;
            }
            get
            {
                return _ModifierName;
            }
        }

        public string CalculationValue
        {
            set
            {
                _CalculationValue = value;
            }
            get
            {
                return _CalculationValue;
            }
        }

        public string CalculationBase
        {
            set
            {
                _CalculationBase = value;
            }
            get
            {
                return _CalculationBase;
            }
        }

        public string BillingModifier
        {
            set
            {
                _BillingModifier = value;
            }
            get
            {
                return _BillingModifier;
            }
        }

        public string Code
        {
            set
            {
                _Code = value;
            }
            get
            {
                return _Code;
            }
        }

        public string Category
        {
            set
            {
                _Category = value;
            }
            get
            {
                return _Category;
            }
        }

        public string Rate
        {
            set
            {
                _Rate = value;
            }
            get
            {
                return _Rate;
            }
        }

        public string GST
        {
            set
            {
                _GST = value;
            }
            get
            {
                return _GST;
            }
        }

        public string ServiceCharges
        {
            set
            {
                _ServiceCharges = value;
            }
            get
            {
                return _ServiceCharges;
            }
        }

        public string Cost
        {
            set
            {
                _Cost = value;
            }
            get
            {
                return _Cost;
            }
        }

        public string ShiftTime
        {
            set
            {
                _ShiftTime = value;
            }
            get
            {
                return _ShiftTime;
            }
        }




        public string RiderCode
        {
            set
            {
                _RiderCode = value;
            }
            get
            {
                return _RiderCode;
            }
        }

        public string ConsignmentNo
        {
            set
            {
                _ConsignmentNo = value;
            }
            get
            {
                return _ConsignmentNo;
            }
        }

        public string RiderName
        {
            set
            {
                _RiderName = value;
            }
            get
            {
                return _RiderName;
            }
        }

        public string ConsignmentType
        {
            set
            {
                _ConsignmentType = value;
            }
            get
            {
                return _ConsignmentType;
            }
        }

        public string Weight
        {
            set
            {
                _Weight = value;
            }
            get
            {
                return _Weight;
            }
        }

        public string Manifest
        {
            set
            {
                _Manifest = value;
            }
            get
            {
                return _Manifest;
            }
        }

        public string ManifestType
        {
            set
            {
                _ManifestType = value;
            }
            get
            {
                return _ManifestType;
            }
        }

        public string Destination
        {
            set
            {
                _Destination = value;
            }
            get
            {
                return _Destination;
            }
        }

        public string Orign
        {
            set
            {
                _Orign = value;
            }
            get
            {
                return _Orign;
            }
        }

        public string BagNumber
        {
            set
            {
                _BagNumber = value;
            }
            get
            {
                return _BagNumber;
            }
        }

        public string Seal
        {
            set
            {
                _Seal = value;
            }
            get
            {
                return _Seal;
            }
        }

        public string Route
        {
            set
            {
                _Route = value;
            }
            get
            {
                return _Route;
            }
        }

        public string TouchPoint
        {
            set
            {
                _TouchPoint = value;
            }
            get
            {
                return _TouchPoint;
            }
        }

        public string TransportType
        {
            set
            {
                _TransportType = value;
            }
            get
            {
                return _TransportType;
            }
        }

        public string Vehicle
        {
            set
            {
                _Vehicle = value;
            }
            get
            {
                return _Vehicle;
            }
        }

        public string RegNo
        {
            set
            {
                _RegNo = value;
            }
            get
            {
                return _RegNo;
            }
        }

        public string CourierName
        {
            set
            {
                _CourierName = value;
            }
            get
            {
                return _CourierName;
            }
        }

        public string VehicleId
        {
            set
            {
                _VehicleId = value;
            }
            get
            {
                return _VehicleId;
            }
        }

        public string Description
        {
            set
            {
                _Description = value;
            }
            get
            {
                return _Description;
            }
        }

        public string LoadingId
        {
            set
            {
                _LoadingId = value;
            }
            get
            {
                return _LoadingId;
            }
        }


        public Int64 ArrivalID
        {
            get { return _ArrivalID; }
            set { _ArrivalID = value; }
        }

        public string StateId
        {
            set
            {
                _StateId = value;
            }
            get
            {
                return _StateId;
            }
        }

        public string MawbNumber
        {
            set
            {
                _MawbNumber = value;
            }
            get
            {
                return _MawbNumber;
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


        public string OldUserName
        {
            set
            {
                _OldUserName = value;
            }
            get
            {
                return _OldUserName;
            }
        }


        public string FlightNo
        {
            set
            {
                _FlightNo = value;
            }
            get
            {
                return _FlightNo;
            }
        }

        public string FlightDepartureDate
        {
            set
            {
                _FlightDepartureDate = value;
            }
            get
            {
                return _FlightDepartureDate;
            }
        }

        public string VehicleNo
        {
            set
            {
                _VehicleNo = value;
            }
            get
            {
                return _VehicleNo;
            }
        }
    }
}