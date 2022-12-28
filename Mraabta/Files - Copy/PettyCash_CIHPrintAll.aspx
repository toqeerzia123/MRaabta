<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PettyCash_CIHPrintAll.aspx.cs" Inherits="MRaabta.Files.PettyCash_CIHPrintAll" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .style1 {
            font-size: small;
            font-weight: bold;
        }

        .style2 {
            font-size: x-small;
        }

        .style3 {
            font-size: x-small;
            width: 45%;
        }

        .style4 {
            width: 35%;
        }

        .style5 {
            font-size: x-small;
            width: 28%;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:PlaceHolder ID="myHolder" runat="server"></asp:PlaceHolder>
            <asp:Literal ID="lt_Main" runat="server"></asp:Literal>
        </div>
    </form>
</body>
</html>
