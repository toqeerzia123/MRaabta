<%@ Page Language="C#" Title="Packing Material Invoice Print" AutoEventWireup="true" CodeBehind="PackingMaterialInvoice.aspx.cs" Inherits="MRaabta.Files.PackingMaterialInvoice" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        function preventBack() { window.history.forward(); }
        setTimeout("preventBack()", 0);
        window.onunload = function () { null };
    </script>
    <style type="text/css">
        td {
            padding-left: 5px;
        }

        .style1 {
            width: 75%;
            margin: 0 auto;
        }

        .style2 {
            width: 147px;
            font-size: small;
        }

        .style3 {
            width: 148px;
            padding-right: 5px;
        }

        .style4 {
            width: 38px;
        }

        .style5 {
            width: 136px;
        }

        .style6 {
            width: 110px;
        }

        .style7 {
            width: 119px;
        }

        .style8 {
            width: 80px;
        }

        .style9 {
            width: 83px;
        }

        .style10 {
            width: 116px;
        }

        .style11 {
            width: 404px;
        }

        .style17 {
            width: 182px;
            height: 50px;
        }

        .style18 {
            height: 50px;
            width: 579px;
        }

        .style19 {
            width: 404px;
            height: 30px;
        }

        .style20 {
            height: 30px;
        }

        .style21 {
            height: 8px;
        }

        .style22 {
            height: 7px;
        }

        .mnp {
            text-align: center;
            background-color: Black;
            color: White;
        }

        .style23 {
            font-weight: normal;
        }

        .label {
            font-weight: 700;
            font-size: small;
        }

        @page {
            margin-top: 4mm;
            margin-bottom: 1mm;
        }

        body {
            padding-top: 10px;
            padding-bottom: 72px;
        }

        .auto-style1 {
            width: 144px;
            font-size: small;
        }

        .auto-style2 {
            width: 183px;
        }

        .auto-style6 {
            width: 144px;
        }

        .auto-style9 {
            width: 148px;
            font-size: small;
        }

        .auto-style10 {
            width: 215px;
        }

        .auto-style11 {
            width: 106px;
            font-size: small;
        }

        .auto-style12 {
            width: 193px;
        }

        .auto-style14 {
            width: 127px;
        }

        .auto-style15 {
            width: 1064px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div id="page-wrap">
            <table class="style1">
                <tr>
                    <td colspan="2" style="text-align: left;" class="style22">
                        <h5>
                            <asp:Label ID="lbl_Print_Date" Text="" runat="server" /></h5>
                    </td>
                </tr>
                <tr>
                    <td colspan="1">
                        <img src="../images/mnpLogo.png" alt="M&P" width="100" />
                    </td>
                    <td colspan="6" style="text-align: center; font-size: larger; font-weight: 300;">
                        <h5>M&P Express Logistics (Private) Limited</h5>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align: center;">&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="2"><b>Sales Tax Registration No: </b></td>
                    <td colspan="2"class="auto-style2">
                       12-00-9808-003-55
                    </td>
                    <td colspan="2"><b>Packing Sheet No:</b></td>
                    <td colspan="2" rowspan="2" style="text-align: center;">
                        <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
                    </td>
                </tr>
                <tr>

                    <td colspan="2" style="text-align: center;">&nbsp;</td>
                </tr>
                <tr>

                    <td colspan="6" style="text-align: center;">&nbsp;</td>
                    <td colspan="2" style="text-align: center;">
                        <asp:Label ID="lblPackingSheetNo" Font-Bold="true" runat="server" /></td>
                </tr>
                <tr>
                    <td class="auto-style1">
                        <b>Customer Code:</b>
                    </td>
                    <td class="auto-style14">
                        <asp:Label ID="lbl_customerCode" runat="server" />
                    </td>
                    <td class="auto-style9">
                        <b>Name:</b>
                    </td>
                    <td class="auto-style10 " style="font-size: small;">
                        <asp:Label ID="lbl_customerName" runat="server" />
                    </td>
                    <td class="auto-style11">
                        <b>Address:</b>
                    </td>
                    <td colspan="3" style="font-size: small;">
                        <asp:Label ID="lbl_Address" runat="server" />
                    </td>
                </tr>

                <tr>
                    <td class="auto-style1">&nbsp;</td>
                    <td class="auto-style14">&nbsp;</td>
                    <td class="auto-style9">&nbsp;</td>
                    <td class="auto-style10">&nbsp;</td>
                    <td class="auto-style11">&nbsp;</td>
                    <td class="auto-style12">&nbsp;</td>
                    <td class="auto-style9">Invoice Number :</td>
                    <td class="auto-style10">
                        <asp:Label ID="lbl_Invoice_NO" Text="" runat="server" /></td>
                </tr>

                <tr>
                    <td class="auto-style1">NAME :</td>
                    <td colspan="2" style="font-size: small;">MULLER AND PHIPPS PAKISTAN PVT LTD</td>

                    <td class="auto-style10">&nbsp;</td>
                    <td class="auto-style11">&nbsp;</td>
                    <td class="auto-style12">&nbsp;</td>
                    <td class="auto-style9">Invoice Date :</td>
                    <td class="auto-style10">
                        <asp:Label ID="lbl_Invoice_Date" Text="" runat="server" /></td>
                </tr>
                <tr>
                    <td class="auto-style1">ADDRESS :</td>
                    <td colspan="4" style="font-size: small;">SECOND FLOOR, DEAN ARCADE BLOCK - 8, KEHKASHAN, CLIFTON, KARACHI.</td>
                    <td class="auto-style12">&nbsp;</td>
                    <td class="auto-style9">Invoice Month :</td>
                    <td class="auto-style10">
                        <asp:Label ID="lbl_Invoice_Month" Text="" runat="server" /></td>
                </tr>
                <tr>
                    <td class="auto-style6">&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style6">&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="8" style="text-align: center;">Description</td>
                </tr>
                <tr>
                    <td colspan="8">&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="8" style="text-align: center;">FLYER BILL</td>
                </tr>
            </table>
            <table border="1px" cellpadding="0" cellspacing="0" class="style1">
                <%--<tr>
                    <th class="auto-style13">
                    </th>
                    <th>
                    </th>
                </tr>--%>
                <asp:Repeater ID="rpt_goods_details" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td class="style4">
                                <%#Eval("Details")%>
                            </td>
                            <td class="style3" style="text-align: right;">
                                <%#Eval("TotalPrice")%>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
                <tr>
                    <td>&nbsp; &nbsp;</td>
                    <td>&nbsp; &nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style15">NET TOTAL
                    </td>
                    <td style="text-align: right; padding-right: 5px;">
                        <asp:Label ID="lbl_NET_AMOUNT" Text="" runat="server" /></td>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style15">GST AMOUNT</td>
                    <td style="text-align: right; padding-right: 5px;">
                        <asp:Label ID="lbl_GST_AMOUNT" Text="" runat="server" /></td>
                </tr>
                <tr>
                    <td class="auto-style15">
                        <b>TOTAL AMOUNT</b> </td>
                    <td style="text-align: right; padding-right: 5px;">
                        <asp:Label ID="lbl_Total_Amount" Text="" runat="server" Font-Bold="true" /></td>
                </tr>
            </table>
            <table border="0" cellpadding="0" cellspacing="0" class="style1">
                <tr>
                    <td colspan="2">&nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="2">&nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <b>Amount in words: &nbsp;</b>    <b>
                            <asp:Label ID="lbl_Amount_Words" runat="server" CssClass="label" /></b>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
