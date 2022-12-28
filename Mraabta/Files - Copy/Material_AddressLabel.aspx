<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Material_AddressLabel.aspx.cs" Inherits="MRaabta.Files.Material_AddressLabel" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        * {
            padding: 0;
            margin: 0;
        }

        td {
            text-align: left;
            padding-left: 10px;
            border-color: Black;
        }

        .alignment {
            text-align: left;
        }
    </style>

    <style type="text/css" media="print">
        /*@media print {

            a[href]:after {
                content: none !important;
            }

            @page {
                margin-top: 5px;
           margin-bottom: 80px; 
                /*margin: 10px 37px 10px 37px;
            }

            body {
                padding-top: 10px;
                padding-bottom: 72px;
            }
        }*/
        @page {
            margin-top: 4mm;
            margin-bottom: 1mm;
        }

        body {
            padding-top: 10px;
            padding-bottom: 72px;
        }
    </style>
    <%--    <style type="text/css" media="print">
        @page {
            size: auto; /* auto is the initial value */
            margin:10px 37px 10px 37px; /* this affects the margin in the printer settings */
             
        }
    </style>--%>
</head>
<body>

    <form id="form1" runat="server">
        <div>
            <asp:Literal ID="lt_main" runat="server"></asp:Literal>
        </div>
    </form>
</body>
</html>

