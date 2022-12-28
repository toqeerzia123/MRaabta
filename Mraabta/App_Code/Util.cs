using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for Util
/// </summary>
/// 

namespace MRaabta.App_Code
{

    namespace Utilities
    {
        public class Util
        {
            private string[] arr_FCColors;
            private int FC_ColorCounter;
            public Util()
            {
                /*    
                 * This page contains an array of colors to be used as default set of colors for FusionCharts
                 * arr_FCColors is the array that would contain the hex code of colors 
                 * ALL COLORS HEX CODES TO BE USED WITHOUT #
                 * 
                 * We also initiate a counter variable to help us cyclically rotate through
                 * the array of colors.
                 */

                FC_ColorCounter = 0;
                arr_FCColors = new string[70];
                arr_FCColors[0] = "1941A5"; //Dark Blue
                arr_FCColors[1] = "AFD8F8";
                arr_FCColors[2] = "F6BD0F";
                arr_FCColors[3] = "8BBA00";
                arr_FCColors[4] = "A66EDD";
                arr_FCColors[5] = "F984A1";
                arr_FCColors[6] = "CCCC00"; //Chrome Yellow+Green
                arr_FCColors[7] = "999999"; //Grey
                arr_FCColors[8] = "0099CC"; //Blue Shade
                arr_FCColors[9] = "FF0000"; //Bright Red 
                arr_FCColors[10] = "006F00"; //Dark Green
                arr_FCColors[11] = "0099FF"; //Blue (Light)
                arr_FCColors[12] = "FF66CC"; //Dark Pink
                arr_FCColors[13] = "669966"; //Dirty green
                arr_FCColors[14] = "7C7CB4"; //Violet shade of blue
                arr_FCColors[15] = "FF9933"; //Orange
                arr_FCColors[16] = "9900FF"; //Violet
                arr_FCColors[17] = "99FFCC"; //Blue+Green Light
                arr_FCColors[18] = "CCCCFF"; //Light violet
                arr_FCColors[19] = "669900"; //Shade of green
                arr_FCColors[20] = "1941A3"; //Dark Blue
                arr_FCColors[21] = "AFD8E8";
                arr_FCColors[22] = "F6BD59";
                arr_FCColors[23] = "8BBA20";
                arr_FCColors[24] = "A66EEE";
                arr_FCColors[25] = "F984BB";
                arr_FCColors[26] = "CCCCDD"; //Chrome Yellow+Green
                arr_FCColors[27] = "999900"; //Grey
                arr_FCColors[28] = "0099EE"; //Blue Shade
                arr_FCColors[29] = "FF1111"; //Bright Red 
                arr_FCColors[30] = "116F00"; //Dark Green
                arr_FCColors[31] = "2299FF"; //Blue (Light)
                arr_FCColors[32] = "FF66EE"; //Dark Pink
                arr_FCColors[33] = "779966"; //Dirty green
                arr_FCColors[34] = "7C7CGG"; //Violet shade of blue
                arr_FCColors[35] = "FF9900"; //Orange
                arr_FCColors[36] = "FF0002"; //Violet
                arr_FCColors[37] = "11FFCC"; //Blue+Green Light
                arr_FCColors[38] = "CCDDFF"; //Light violet
                arr_FCColors[39] = "116600"; //Shade of green
                arr_FCColors[40] = "990088";
                arr_FCColors[41] = "1A5982"; //Dark Blue
                arr_FCColors[42] = "AFD987";
                arr_FCColors[43] = "F6EG0F";
                arr_FCColors[44] = "8BB00A";
                arr_FCColors[45] = "A66DDE";
                arr_FCColors[46] = "F9814A";
                arr_FCColors[47] = "CC00CC"; //Chrome Yellow+Green
                arr_FCColors[48] = "9AA999"; //Grey
                arr_FCColors[49] = "99CC00"; //Blue Shade
                arr_FCColors[50] = "0000FF"; //Bright Red 
                arr_FCColors[51] = "9900FF"; //Blue (Light)
                arr_FCColors[52] = "66CCFF"; //Dark Pink
                arr_FCColors[53] = "669966"; //Dirty green
                arr_FCColors[54] = "7C7CB4"; //Violet shade of blue
                arr_FCColors[55] = "FF9933"; //Orange
                arr_FCColors[56] = "9900FF"; //Violet
                arr_FCColors[57] = "99FFCC"; //Blue+Green Light
                arr_FCColors[58] = "CCCCFF"; //Light violet
                arr_FCColors[59] = "669900"; //Shade of green
                arr_FCColors[60] = "1A5982";
                arr_FCColors[61] = "1A5945"; //Dark Blue
                arr_FCColors[62] = "AFD987";
                arr_FCColors[63] = "F6EG0F";
                arr_FCColors[64] = "8BB00A";
                arr_FCColors[65] = "A66DDE";
                arr_FCColors[66] = "F9814A";
            }
            //getFCColor method helps return a color from arr_FCColors array. It uses
            //cyclic iteration to return a color from a given index. The index value is
            //maintained in FC_ColorCounter

            public string getFCColor()
            {

                //Update index
                FC_ColorCounter++;
                //Return color
                return arr_FCColors[FC_ColorCounter % arr_FCColors.Length];
            }
        }
    }
}