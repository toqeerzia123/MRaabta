<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrintDemanifest.aspx.cs" Inherits="MRaabta.Files.PrintDemanifest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="width: 100%; text-align:center;">
        <asp:PlaceHolder ID="ph1" runat="server"></asp:PlaceHolder>
        <%--<table style="width: 100%; border: 2px Solid Black; border-bottom: 1px Solid Black;
            border-collapse: collapse; font-family: Calibri; font-size: medium;">
            <tr>
                <td style="width: 100%; font-size: x-large; font-variant: small-caps; text-align: center;
                    border-bottom: 2px Solid Black" colspan="4">
                    <b>Demanifest Detail</b>
                </td>
            </tr>
            <tr>
                <td style="width: 20%; text-align: center; border: 1px Solid Black; border-bottom: 2px Solid Black">
                    <b>User</b>
                </td>
                <td style="width: 30%; text-align: center; border: 1px Solid Black; border-bottom: 2px Solid Black">
                    khi.operations
                </td>
                <td style="width: 25%; text-align: center; border: 1px Solid Black; border-bottom: 2px Solid Black">
                    <b>Print Date Time</b>
                </td>
                <td style="width: 25%; text-align: center; border: 1px Solid Black; border-bottom: 2px Solid Black">
                    15-12-2016 09:00 AM
                </td>
            </tr>
            <tr>
                <td style="width: 20%; text-align: center; border: 1px Solid Black;">
                    <b>Date</b>
                </td>
                <td style="width: 30%; text-align: center; border: 1px Solid Black;">
                    <b>Demanifest Count</b>
                </td>
                <td style="width: 25%; text-align: center; border: 1px Solid Black;">
                    <b>CN Count</b>
                </td>
                <td style="width: 25%; text-align: center; border: 1px Solid Black;">
                    Page
                </td>
            </tr>
            <tr>
                <td style="width: 20%; text-align: center; border: 1px Solid Black;">
                    15-12-2017
                </td>
                <td style="width: 30%; text-align: center; border: 1px Solid Black;">
                    50
                </td>
                <td style="width: 25%; text-align: center; border: 1px Solid Black;">
                    500
                </td>
                <td style="width: 25%; text-align: center; border: 1px Solid Black;">
                    Page
                </td>
            </tr>
            <tr>
                <td style="width: 20%; text-align: center; border: 1px Solid Black; border-top: 2px Solid Black;
                    border-bottom: 2px Solid Black;">
                    <b>Origin</b>
                </td>
                <td style="width: 30%; text-align: center; border: 1px Solid Black; border-top: 2px Solid Black;
                    border-bottom: 2px Solid Black;">
                    <b>Destination</b>
                </td>
                <td style="width: 25%; text-align: center; border: 1px Solid Black; border-top: 2px Solid Black;
                    border-bottom: 2px Solid Black;">
                    <b>Manifest Number</b>
                </td>
                <td style="width: 25%; text-align: center; border: 1px Solid Black; border-top: 2px Solid Black;
                    border-bottom: 2px Solid Black;">
                    <b>Consignment Count</b>
                </td>
            </tr>
        </table>--%>
    </div>
    </form>
</body>
</html>
