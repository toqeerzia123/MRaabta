using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Session_class
/// </summary>
/// 
namespace MRaabta.App_Code
{
    public class Session_class
    {
        public string _session_div;
        public string _session_prn;
        public string _session_zone;
        public string _session_depot;

        public Session_class()
        {
            //
            // TODO: Add constructor logic here
            //
            _session_div = string.Empty;
            _session_prn = string.Empty;
            _session_zone = string.Empty;
            _session_depot = string.Empty;
        }

        public string Session_div
        {
            set
            {
                _session_div = value;
            }
            get
            {
                return _session_div;
            }
        }

        public string Session_prn
        {
            set
            {
                _session_prn = value;
            }
            get
            {
                return _session_prn;
            }
        }

        public string Session_zone
        {
            set
            {
                _session_zone = value;
            }
            get
            {
                return _session_zone;
            }
        }

        public string Session_Depot
        {
            set
            {
                _session_depot = value;
            }
            get
            {
                return _session_depot;
            }
        }
    }
}