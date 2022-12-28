using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRaabta.Repo
{
    public class Points
    {


        //public string ADDRESS_NOT_FOUND { get; set; }
        //public string FSC_FridaySaturdayClosed { get; set; }
        //public string CS_ConsigneeShifted { get; set; }
        //public string ICA_IncompleteAddress { get; set; }
        //public string M_R_REASON { get; set; }
        //public string BAD_ADDRESS { get; set; }
        //public string UL_UnabletoLocate { get; set; }
        //public string MIS_ROUTED { get; set; }
        //public string SRT_ShortContents_Missing { get; set; }
        //public string CLA_ClosedonArrival { get; set; }
        //public string CAN_ConsigneenotAvailable { get; set; }
        //public string NSA_NonServiceArea { get; set; }
        //public string RD_Refused_to_Received { get; set; }
        //public string NSC_No_Such_Consignee { get; set; }
        //public string GT_Given_To { get; set; }
        //public string UAT_Unattempted { get; set; }
        //public string HPO_Hold_Pending { get; set; }
        //public string RTR_Ready_to_Return { get; set; }
        //public string DED_DeductionCase { get; set; }
        //public string Other { get; set; }

        public string Reason { get; set; }

        public int Maximum_Count { get; set; }

        public int TotalRunsheet { get; set;}
        
         public int TotalCN { get; set; }

    }
}