<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CN_Import_Print.aspx.cs" Inherits="MRaabta.Files.CN_Import_Print" %>


<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

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
                    <td style="width: 55%; text-align: center;">
                        <h2>
                            REIMBURSEMENT STATEMENT</h2>
                    </td>
                    <td style="width: 25%; vertical-align: top; text-align: right">
                        <b>Print Time :</b><asp:Label ID="lbl_Dd" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="1">
                    </td>
                    <td colspan="1" style="width: 55%; text-align: center;">
                        <telerik:RadBarcode ID="t1" runat="server" Height="50px" Width="150px" LineWidth="2"
                            Type="Code128" Font-Size="16px" Style="margin-left: 30px" ShowChecksum="true"
                            ShowText="false">
                        </telerik:RadBarcode>
                    </td>
                    <td colspan="1">
                        <b>Gst No :</b><asp:Label ID="lbl_Gst" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
            <br />
            <table style="border: thin solid
                #003366; width: 100%; font-family: Calibri; font-size: small; border-collapse: collapse;">
                <tr>
                    <td colspan="5" style="text-align: center; font-size: medium; border: 2px
                Solid Black; background-color: #C0C0C0">
                        <b>Consignment Information</b>
                    </td>
                </tr>
                <tr>
                    <td style="border-right-style: solid; border-right-width: 1px; border-right-color: #000000;
                        border-bottom-style: solid; border-bottom-width: 1px; border-bottom-color: #000000;
                        border-left-width: 2px; border-left-style: Solid; width: 20%;">
                        <b>HAWB No:</b> <b>
                            <asp:Label ID="lbl_ConsignmentNumber" runat="server" Font-Size="Medium"></b></asp:Label>
                    </td>
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
                        width: 20%;">
                        &nbsp;
                    </td>
                    <td style="border-right-style: solid; border-right-width: 2px; border-right-color: #000000;
                        border-bottom-style: solid; border-bottom-width: 1px; border-bottom-color: #000000;
                        width: 20%;">
                        <b>BookingDate: </b>
                        <asp:Label ID="lbl_ReceiptDate" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="border-right-style: solid; border-right-width: 1px; border-right-color: #000000;
                        border-bottom-style: solid; border-bottom-width: 2px; border-bottom-color: #000000;
                        border-left-width: 2px; border-left-style: Solid;">
                        <b>Customer Account: </b>
                        <asp:Label ID="lbl_AccountNo" runat="server"></asp:Label>
                    </td>
                    <td colspan="4" style="border-right-style: solid; border-right-width: 2px; border-right-color: #000000;
                        border-bottom-style: solid; border-bottom-width: 2px; border-bottom-color: #000000;">
                        <b>Customre Name: </b>
                        <asp:Label ID="lbl_CustomeName" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="border-right-style: solid; border-right-width: 2px; border-right-color: #000000;
                        border-bottom-style: solid; border-bottom-width: 2px; border-bottom-color: #000000;
                        width: 20%;">
                        <b>Origin Country: </b>
                        <asp:Label ID="lbl_OriginCountry" runat="server"></asp:Label>
                    </td>
                    <td style="border-right-style: solid; border-right-width: 1px; border-right-color: #000000;
                        border-bottom-style: solid; border-bottom-width: 2px; border-bottom-color: #000000;
                        border-left-width: 2px; border-left-style: Solid; width: 20%;">
                        <b>Destination: </b>
                        <asp:Label ID="lbl_Destination" runat="server"></asp:Label>
                    </td>
                    <td colspan="2" style="border-right-style: solid; border-right-width: 2px; border-right-color: #000000;
                        border-bottom-style: solid; border-bottom-width: 2px; border-bottom-color: #000000;
                        width: 20%;">
                        <b>Shipment Type: </b>
                        <asp:Label ID="lbl_ServiceType" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
            <br />
            <br />
            <div style="width: 100%; text-align: center; font-family: Calibri; font-size: small;">
                <asp:GridView ID="gv_productWiseAmount" runat="server" AutoGenerateColumns="false"
                    Width="100%" OnRowDataBound="gv_productWiseAmount_RowDataBound" ShowFooter="true">
                    <HeaderStyle BackColor="#c0c0c0" BorderStyle="Solid" BorderColor="Black" BorderWidth="2px"
                        Font-Size="Medium" />
                    <Columns>
                        <asp:BoundField HeaderText="Particulars" DataField="PriceModifiers" HeaderStyle-Width="70%"
                            ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="Amount" DataField="TotalAmount" HeaderStyle-Width="15%"
                            ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField HeaderText="Gst" DataField="Gst" HeaderStyle-Width="15%" ItemStyle-HorizontalAlign="Right" />
                    </Columns>
                    <FooterStyle />
                </asp:GridView>
            </div>
            <br />
            <table style="border: thin solid
                #003366; width: 100%; font-family: Calibri; font-size: medium; font-weight: bold;">
                <tr>
                    <td style="border-style: none; border-width: 2px; border-color: inherit; text-align: eft;
                        border-bottom-width: 2px; width: 75%">
                        <b>
                            <asp:Label ID="totalAmount" runat="server"></asp:Label>
                        </b>
                    </td>
                    <td style="border-style: none; border-width: 2px; border-color: inherit; text-align: eft;
                        border-bottom-width: 2px; text-align: right">
                        <b>
                            <asp:Label ID="totalAmount_" runat="server"></asp:Label>
                        </b>
                    </td>
                </tr>
            </table>
            <b>*In case of cheque or pay order please prepare in favor of M&P Express Logistics
                (Pvt) Ltd. </b>
            <br />
            <br />
            <br />
            <table style="width: 100%; font-family: Calibri; font-size: small;">
                <tr>
                    <td style="border-style: none; border-width: 2px; border-color: inherit; text-align: eft;
                        border-bottom-width: 2px;">
                        <b>IMPORT TEAM</b><br />
                        <b>CC:- Branch Manager/Incharge&nbsp; </b>
                        <br />
                        <b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; -Manager Account HO, KHI </b>
                        <br />
                        <br />
                        <br />
                        <b>M&P Express Logistics (Private) Limited
                            <br />
                            (Formerly OCS Pakistan (Pvt) Ltd) c-17 Korangi Road, DHA Phase II Ext, Karachi Pakistan.
                            Phone: 111-002-002 Ext : 3046, 3055 </b>
                    </td>
                </tr>
                <tr>
                    <td style="border-style: none; border-width: 2px; border-color: inherit; text-align: eft;
                        border-bottom-width: 2px;">
                        &nbsp;
                    </td>
                </tr>
            </table>
        </div>
    </div>
    </form>
</body>
</html>