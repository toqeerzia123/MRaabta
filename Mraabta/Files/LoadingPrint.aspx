<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoadingPrint.aspx.cs" Inherits="MRaabta.Files.LoadingPrint" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

</head>
<body>

    <style>
        .error {
            color: red;
            float: left;
            font-size: 2em;
            text-align: center;
            width: 100%;
        }
    </style>

    <form id="form1" runat="server">
        <asp:Label ID="lbl_error" runat="server" CssClass="error"></asp:Label>
        <div style="width: 750px;">


            <asp:PlaceHolder ID="ph1" runat="server"></asp:PlaceHolder>
            <br />

        </div>
    </form>
</body>
</html>