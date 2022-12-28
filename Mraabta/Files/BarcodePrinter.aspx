<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BarcodePrinter.aspx.cs" Inherits="MRaabta.Files.BarcodePrinter" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="font-family:Calibri;">
        <asp:PlaceHolder ID="ph1" runat="server"></asp:PlaceHolder>
    </div>
    </form>
</body>
</html>