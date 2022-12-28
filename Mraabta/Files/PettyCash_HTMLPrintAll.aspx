<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PettyCash_HTMLPrintAll.aspx.cs" Inherits="MRaabta.Files.PettyCash_HTMLPrintAll" %>

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
            <asp:Button ID="btn" runat="server" Text="Edit" Visible="false"
                Width="130" CssClass="button1" OnClick="btn_Click" />
            <asp:HiddenField runat="server" ID="hf_id" />
            <asp:PlaceHolder ID="myHolder" runat="server"></asp:PlaceHolder>
            <asp:Literal ID="lt_Main" runat="server"></asp:Literal>
        </div>
    </form>
</body>
</html>
