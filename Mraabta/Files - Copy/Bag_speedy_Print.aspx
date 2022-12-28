<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Bag_speedy_Print.aspx.cs" Inherits="MRaabta.Files.Bag_speedy_Print" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        .outpiece
        {
            width: 100%;
            font-family: Calibri;
            font-size: small;
            border-collapse: collapse;
            
        }
        .outpiece th
        {
            border-bottom: 2px Solid Black;
            border-top: 2px Solid Black;
            
        }
        .outpiece td
        {
            border-bottom: 1px Solid Black;
            border-top: 1px Solid Black;
            
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div style="width: 700px;">
            <asp:PlaceHolder ID="ph1" runat="server"></asp:PlaceHolder>
            
        </div>
    </div>
    </form>
</body>
</html>
