<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Receipt_Print.aspx.cs" Inherits="MRaabta.Files.Receipt_Print" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        .outpiece {
            width: 100%;
            font-family: Calibri;
            font-size: small;
            border-collapse: collapse;
        }

            .outpiece th {
                border-bottom: 2px Solid Black;
                border-top: 2px Solid Black;
            }

            .outpiece td {
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
                            <h2>Receipt Voucher</h2>
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
                            <b>Voucher Information</b>
                        </td>
                    </tr>
                    <tr>
                        <td style="border-right-style: solid; border-right-width: 1px; border-right-color: #000000; border-bottom-style: solid; border-bottom-width: 1px; border-bottom-color: #000000; border-left-width: 2px; border-left-style: Solid; width: 20%;">
                            <b>VoucherNo:</b><asp:Label ID="lbl_Voucher_1" runat="server"></asp:Label>
                        </td>
                        <td style="border-right-style: solid; border-right-width: 1px; border-right-color: #000000; border-bottom-style: solid; border-bottom-width: 1px; border-bottom-color: #000000; width: 20%;">
                            <b>Branch: </b>
                            <asp:Label ID="lbl_Branch" runat="server"></asp:Label>
                        </td>
                        <td style="border-right-style: solid; border-right-width: 1px; border-right-color: #000000; border-bottom-style: solid; border-bottom-width: 1px; border-bottom-color: #000000; width: 20%;">
                            <b>Zone: </b>
                            <asp:Label ID="lbl_Zone" runat="server"></asp:Label>
                        </td>
                        <td style="border-right-style: solid; border-right-width: 1px; border-right-color: #000000; border-bottom-style: solid; border-bottom-width: 1px; border-bottom-color: #000000; width: 20%;">
                            <b>ReceiptNo: </b>
                            <asp:Label ID="lbl_ReceiptNo" runat="server"></asp:Label>
                        </td>
                        <td style="border-right-style: solid; border-right-width: 2px; border-right-color: #000000; border-bottom-style: solid; border-bottom-width: 1px; border-bottom-color: #000000; width: 20%;">
                            <b>Date: </b>
                            <asp:Label ID="lbl_ReceiptDate" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="border-right-style: solid; border-right-width: 1px; border-right-color: #000000; border-bottom-style: solid; border-bottom-width: 2px; border-bottom-color: #000000; border-left-width: 2px; border-left-style: Solid;">
                            <b>Customer Account: </b>
                            <asp:Label ID="lbl_AccountNo" runat="server"></asp:Label>
                        </td>
                        <td colspan="4" style="border-right-style: solid; border-right-width: 2px; border-right-color: #000000; border-bottom-style: solid; border-bottom-width: 2px; border-bottom-color: #000000;">
                            <b>Customre Name: </b>
                            <asp:Label ID="lbl_CustomeName" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="border-right-style: solid; border-right-width: 1px; border-right-color: #000000; border-bottom-style: solid; border-bottom-width: 2px; border-bottom-color: #000000; border-left-width: 2px; border-left-style: Solid; width: 20%;">
                            <b>Customer Type: </b>
                            <asp:Label ID="lbl_CustomerType" runat="server"></asp:Label>
                        </td>
                        <td colspan="2" style="border-right-style: solid; border-right-width: 2px; border-right-color: #000000; border-bottom-style: solid; border-bottom-width: 2px; border-bottom-color: #000000; width: 20%;">
                            <b>Payment Source: </b>
                            <asp:Label ID="lbl_PaymentSource" runat="server"></asp:Label>
                        </td>
                        <td colspan="2" style="border-right-style: solid; border-right-width: 2px; border-right-color: #000000; border-bottom-style: solid; border-bottom-width: 2px; border-bottom-color: #000000; width: 20%;">
                            <b>Payment Type: </b>
                            <asp:Label ID="lbl_PaymentType" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="border-right-style: solid; border-right-width: 1px; border-right-color: #000000; border-bottom-style: solid; border-bottom-width: 2px; border-bottom-color: #000000; border-left-width: 2px; border-left-style: Solid; width: 20%;">
                            <b>CN No: </b>
                            <asp:Label ID="lbl_ConsignmentNo" runat="server"></asp:Label>
                        </td>
                        <td colspan="2" style="border-right-style: solid; border-right-width: 2px; border-right-color: #000000; border-bottom-style: solid; border-bottom-width: 2px; border-bottom-color: #000000; width: 20%;">
                            <b>Rider Name: </b>
                            <asp:Label ID="lbl_RiderName" runat="server"></asp:Label>
                        </td>
                        <td colspan="2" style="border-right-style: solid; border-right-width: 2px; border-right-color: #000000; border-bottom-style: solid; border-bottom-width: 2px; border-bottom-color: #000000; width: 20%;">
                            <b>Express Center Name: </b>
                            <asp:Label ID="lbl_ExpressCenter" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="border-right-style: solid; border-right-width: 1px; border-right-color: #000000; border-bottom-style: solid; border-bottom-width: 2px; border-bottom-color: #000000; border-left-width: 2px; border-left-style: Solid; width: 20%;"
                            colspan="2">
                            <b>Deposit Slip No. :</b>
                            <asp:Label ID="lbl_dslipNo" Text="" runat="server"></asp:Label>
                        </td>
                        <td colspan="3" style="border-right-style: solid; border-right-width: 2px; border-right-color: #000000; border-bottom-style: solid; border-bottom-width: 2px; border-bottom-color: #000000; width: 20%;">
                            <b>Deposit Slip Bank :</b>
                            <asp:Label ID="lbl_dslipBank" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="border-right-style: solid; border-right-width: 1px; border-right-color: #000000; border-bottom-style: solid; border-bottom-width: 2px; border-bottom-color: #000000; border-left-width: 2px; border-left-style: Solid; width: 20%;"
                            colspan="2">
                            <b>CPR Number :</b>
                            <asp:Label ID="lbl_cprNo" Text="" runat="server"></asp:Label>
                        </td>
                        <td colspan="3" style="border-right-style: solid; border-right-width: 2px; border-right-color: #000000; border-bottom-style: solid; border-bottom-width: 2px; border-bottom-color: #000000; width: 20%;">
                            <b>STW Number :</b>
                            <asp:Label ID="lbl_stwNo" runat="server"></asp:Label>
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
                        <td colspan="5" style="text-align: center; font-size: medium; border: 2px
                Solid Black; background-color: #C0C0C0">
                            <b>Payment Information</b>
                        </td>
                    </tr>
                    <tr>
                        <th width='20%' style="background-color: #C0C0C0">Voucher No
                        </th>
                        <th width='20%' style="background-color: #C0C0C0">Account
                        </th>
                        <th width='20%' style="background-color: #C0C0C0">Amount
                        </th>
                        <th width='20%' style="background-color: #C0C0C0">Balance
                        </th>
                        <th width='20%' style="background-color: #C0C0C0">Date
                        </th>
                    </tr>
                    <tr>
                        <td width='20%' style="text-align: center;">
                            <b>
                                <asp:Label ID="lbl_Voucher2" runat="server"></asp:Label></b>
                        </td>
                        <td width='20%' style="text-align: center;">
                            <b>
                                <asp:Label ID="lbl_AcountNo" runat="server"></asp:Label></b>
                        </td>
                        <td width='20%' style="text-align: center;">
                            <b>
                                <asp:Label ID="lbl_Amount" runat="server"></asp:Label></b>
                        </td>
                        <td width='20%' style="text-align: center;">
                            <b>
                                <asp:Label ID="lbl_Balance" runat="server"></asp:Label></b>
                        </td>
                        <td width='20%' style="text-align: center;">
                            <b>
                                <asp:Label ID="lbl_Date" runat="server"></asp:Label></b>
                        </td>
                    </tr>
                </table>
                <br />
                <br />
                <br />
                <div style="width: 100%; text-align: center; font-family: Calibri; font-size: small;">
                    <asp:GridView ID="gv_productWiseAmount" runat="server" AutoGenerateColumns="false"
                        Width="100%">
                        <HeaderStyle BackColor="#c0c0c0" BorderStyle="Solid" BorderColor="Black" BorderWidth="2px"
                            Font-Size="Medium" HorizontalAlign="Center" />
                        <Columns>
                            <asp:BoundField HeaderText="Product" DataField="Product" HeaderStyle-Width="50%" />
                            <asp:BoundField HeaderText="Amount" DataField="Amount" HeaderStyle-Width="50%" />
                        </Columns>
                    </asp:GridView>
                </div>
                <br />
                <br />
                <br />
                <table style="border: thin solid
                #003366; width: 100%; font-family: Calibri; font-size: small;">
                    <tr>
                        <td colspan="1" style="text-align: center; font-size: medium;"></td>
                        <td colspan="2" style="border-style: none none solid none; border-width: 2px; border-color: inherit; text-align: center; border-bottom-style: solid; border-bottom-width: 2px;"></td>
                        <td colspan="1" style="text-align: center; font-size: medium;"></td>
                        <td colspan="2" style="border-color: Black; text-align: center; font-size: medium; border-bottom-style: solid; border-bottom-width: 2px;"></td>
                    </tr>
                    <tr>
                        <td colspan="1" style="text-align: center; font-size: medium;"></td>
                        <td colspan="2" style="border-style: none; border-width: 2px; border-color: inherit; text-align: center; border-bottom-width: 2px;">Approved By
                        </td>
                        <td colspan="1" style="text-align: center; font-size: medium;"></td>
                        <td colspan="2" style="border-color: Black; text-align: center; font-size: medium; border-bottom-width: 2px;">Received By
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </form>
</body>
</html>