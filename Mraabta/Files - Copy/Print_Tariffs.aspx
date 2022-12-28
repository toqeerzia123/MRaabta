<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Print_Tariffs.aspx.cs" Inherits="MRaabta.Files.Print_Tariffs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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
            <table style="width: 100%; text-align: center; font-family: Calibri; font-size: small;">
                <tr>
                    <td style="width: 20%">
                        <img src="../images/OCS_Transparent.png" height="60px" alt="logo" width="157px" />
                    </td>
                    <td style="width: 60%; text-align: center;">
                        <h2>
                            Customer Tariffs</h2>
                    </td>
                    <td style="width: 20%; vertical-align: top; text-align: right">
                        <b>Print Time</b><asp:Label ID="lbl_Dd" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
            <table style="border: thin solid
                #003366; width: 100%; font-family: Calibri; font-size: small; border-collapse: collapse;">
                <tr>
                    <td colspan="5" style="text-align: center; font-size: medium; border: 2px
                Solid Black; background-color: #C0C0C0">
                        <b>Customer Information</b>
                    </td>
                </tr>
                <tr>
                    <td style="border-right-style: solid; border-right-width: 1px; border-right-color: #000000;
                        border-bottom-style: solid; border-bottom-width: 1px; border-bottom-color: #000000;
                        width: 20%;">
                        <b>Branch: </b>
                        <asp:Label ID="lbl_Branch" runat="server"></asp:Label>
                    </td>
                    <td style="border-right-style: solid; border-right-width: 1px; border-right-color: #000000;
                        border-bottom-style: solid; border-bottom-width: 1px; border-bottom-color: #000000;
                        width: 20%;">
                        <b>Zone: </b>
                        <asp:Label ID="lbl_Zone" runat="server"></asp:Label>
                    </td>
                    <td style="border-right-style: solid; border-right-width: 1px; border-right-color: #000000;
                        border-bottom-style: solid; border-bottom-width: 1px; border-bottom-color: #000000;
                        border-left-width: 2px; border-left-style: Solid; width: 20%;">
                    </td>
                    <td style="border-right-style: solid; border-right-width: 1px; border-right-color: #000000;
                        border-bottom-style: solid; border-bottom-width: 1px; border-bottom-color: #000000;
                        width: 20%;">
                    </td>
                    <td style="border-right-style: solid; border-right-width: 2px; border-right-color: #000000;
                        border-bottom-style: solid; border-bottom-width: 1px; border-bottom-color: #000000;
                        width: 20%;">
                        <b>Date: </b>
                        <asp:Label ID="lbl_ReceiptDate" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="1" style="border-right-style: solid; border-right-width: 1px; border-right-color: #000000;
                        border-bottom-style: solid; border-bottom-width: 2px; border-bottom-color: #000000;
                        border-left-width: 2px; border-left-style: Solid;">
                        <b>Customer ID: </b>
                        <asp:Label ID="lbl_id" runat="server"></asp:Label>
                    </td>
                    <td colspan="1" style="border-right-style: solid; border-right-width: 1px; border-right-color: #000000;
                        border-bottom-style: solid; border-bottom-width: 2px; border-bottom-color: #000000;
                        border-left-width: 2px; border-left-style: Solid;">
                        <b>Customer Account: </b>
                        <asp:Label ID="lbl_AccountNo" runat="server"></asp:Label>
                    </td>
                    <td colspan="3" style="border-right-style: solid; border-right-width: 2px; border-right-color: #000000;
                        border-bottom-style: solid; border-bottom-width: 2px; border-bottom-color: #000000;">
                        <b>Customer Name: </b>
                        <asp:Label ID="lbl_CustomeName" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="border-right-style: solid; border-right-width: 1px; border-right-color: #000000;
                        border-bottom-style: solid; border-bottom-width: 2px; border-bottom-color: #000000;
                        border-left-width: 2px; border-left-style: Solid; width: 20%;">
                        <b>Customer Type: </b>
                        <asp:Label ID="lbl_CustomerType" runat="server"></asp:Label>
                    </td>
                    <td colspan="2" style="border-right-style: solid; border-right-width: 2px; border-right-color: #000000;
                        border-bottom-style: solid; border-bottom-width: 2px; border-bottom-color: #000000;
                        width: 20%;">
                        <b>Industry ID: </b>
                        <asp:Label ID="lbl_Industry" runat="server"></asp:Label>
                    </td>
                    <td colspan="1" style="border-right-style: solid; border-right-width: 2px; border-right-color: #000000;
                        border-bottom-style: solid; border-bottom-width: 2px; border-bottom-color: #000000;
                        width: 20%;">
                        <b>Centralized Client: </b>
                        <asp:Label ID="lb_CentralizedClient" runat="server"></asp:Label>
                    </td>
                    <td colspan="1" style="border-right-style: solid; border-right-width: 2px; border-right-color: #000000;
                        border-bottom-style: solid; border-bottom-width: 2px; border-bottom-color: #000000;
                        width: 20%;">
                        <b>Client Group: </b>
                        <asp:Label ID="lb_ClientGroup" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
            <br />
            <br />
            <br />
            <table style="border: thin solid
                #003366; width: 100%; font-family: Calibri; font-size: small; border-collapse: collapse;"
                align="center">
                <tr>
                    <td style="text-align: center; font-size: medium; border: 2px
                Solid Black; background-color: #C0C0C0">
                        <b>Tariff Information</b>
                    </td>
                </tr>
                <tr>
                    <asp:GridView ID="gv_tariff_" runat="server" AutoGenerateColumns="false" CssClass="mGrid"
                        AlternatingRowStyle-CssClass="alt" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px"
                        EmptyDataText="No Tariff Available" Width="100%">
                        <Columns>
                            <asp:BoundField HeaderText="Destination" DataField="Destination" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField HeaderText="Service Type" DataField="ServiceID" />
                            <asp:BoundField HeaderText="From Weight" DataField="FromWeight" />
                            <asp:BoundField HeaderText="To Weight" DataField="ToWeight" />
                            <asp:BoundField HeaderText="Additional Factor" DataField="addFactor" />
                            <asp:BoundField HeaderText="Price" DataField="Price" />
                        </Columns>
                    </asp:GridView>
                </tr>
            </table>
            <br />
            <br />
            <br />
            <table style="border: thin solid
                #003366; width: 50%; font-family: Calibri; font-size: small;">
                <tr>
                    <td colspan="1" style="text-align: center; font-size: medium;">
                    </td>
                    <td colspan="2" style="border-style: none none solid none; border-width: 2px; border-color: inherit;
                        text-align: center; border-bottom-style: solid; border-bottom-width: 2px;">
                    </td>
                    <td colspan="1" style="text-align: center; font-size: medium;">
                    </td>
                    <td colspan="2" style="border-color: Black; text-align: center; font-size: medium;
                        border-bottom-style: solid; border-bottom-width: 2px;">
                    </td>
                </tr>
                <tr>
                    <td colspan="1" style="text-align: center; font-size: medium;">
                    </td>
                    <td colspan="2" style="border-style: none; border-width: 2px; border-color: inherit;
                        text-align: center; border-bottom-width: 2px;">
                        Approved By
                    </td>
                    <td colspan="1" style="text-align: center; font-size: medium;">
                    </td>
                    <td colspan="2" style="border-color: Black; text-align: center; font-size: medium;
                        border-bottom-width: 2px;">
                        Received By
                    </td>
                </tr>
            </table>
        </div>
    </div>
    </form>
</html>
