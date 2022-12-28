<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="InvoiceRedeemAdjTesting.aspx.cs" Inherits="MRaabta.Files.InvoiceRedeemAdjTesting" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script>
        function ReadGrid() {
            debugger;
            var grid = document.getElementById('<%= gv_redemptions.ClientID %>');

            for (var i = 1; i < grid.rows.length; i++) {

                var redeemAmount = grid.rows[i].cells[4].innerHTML;
                var voucherAmount = grid.rows[i].cells[5].innerHTML;
                var voucherUsedAmount = grid.rows[i].cells[6].innerHTML;
                var adjAmount = grid.rows[i].cells[7].childNodes[1].value;

                if (adjAmount != "") {

                    var FredeemAmount = parseFloat(redeemAmount.toString());
                    var FadjAmount = parseFloat(adjAmount.toString());
                    var FvoucherUsedAmount = parseFloat(voucherUsedAmount.toString());
                    if (FadjAmount > FredeemAmount || FadjAmount > FvoucherUsedAmount) {
                        grid.rows[i].cells[7].childNodes[1].value = "";
                        alert('Adj Amount Cannot be Greater than redeem amount or Voucher Used amount');
                        grid.rows[i].focus();
                        grid.rows[i].bgColor = "#f7a0a0";
                        break;
                    }
                    else {
                        grid.rows[i].bgColor = "White";
                    }


                }
            }

        }
    </script>
    <div style="text-align: center; font-size: medium; font-weight: bold; width: 100%; padding-left: 20px;">
        <asp:Label ID="Errorid" runat="server"></asp:Label>
    </div>
    <table style="font-family: Calibri; font-size: medium; padding-bottom: 0px !important; padding-top: 10px !important; width: 97%"
        class="input-form">
        <tr style="margin: 0px 0px 0px 0px !important;">
            <td class="field" style="width: 15% !important;">Search By
            </td>
            <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;"></td>
        </tr>
        <tr>
            <td class="input-field" style="width: 32% !important; text-align: left;">
                <asp:RadioButtonList ID="rbtn_formMode" runat="server" RepeatDirection="Horizontal"
                    RepeatColumns="2" OnSelectedIndexChanged="rbtn_formMode_SelectedIndexChanged">
                    <asp:ListItem Selected="True" Value="1">Invoice No</asp:ListItem>
                    <%--<asp:ListItem Value="2" Enabled="false">Voucher</asp:ListItem>--%>
                </asp:RadioButtonList>
            </td>
            <td class="field" style="width: 7% !important;"></td>
            <td class="field" style="width: 5% !important;"></td>
            <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;"></td>
        </tr>
    </table>
    <table style="font-family: Calibri; font-size: medium; padding-bottom: 0px !important; padding-top: 0px !important; width: 97%"
        class="input-form">
        <tr style="float: none !important;">
            <td style="float: none !important; font-variant: small-caps !important; width: 200px; padding-bottom: 5px !important; font-size: large; text-align: center;"
                colspan="9">
                <b>Information</b>
            </td>
        </tr>
        <tr>
            <td class="field" style="width: 15% !important; text-align: right !important; padding-right: 5px!important;">
                <asp:Label ID="lbl_searchBy" runat="server" Text="Invoice Number"></asp:Label>
            </td>
            <td class="input-field" style="width: 15% !important;">
                <asp:TextBox ID="txt_voucherNo" runat="server" OnTextChanged="txt_voucherNo_TextChanged"
                    AutoPostBack="true"></asp:TextBox>
            </td>
            <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;"></td>
            <td class="field" style="width: 10% !important; text-align: right !important; padding-right: 5px!important;"></td>
            <td class="input-field" style="width: 15% !important;"></td>
            <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;"></td>
            <td class="field" style="width: 10% !important; text-align: right !important; padding-right: 5px!important;"></td>
            <td class="input-field" style="width: 15% !important;"></td>
            <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;"></td>
        </tr>
        <tr>
            <td class="field" style="width: 15% !important; text-align: right !important; padding-right: 5px!important;">Invoice Date
            </td>
            <td class="input-field" style="width: 15% !important;">
                <asp:TextBox ID="txt_invoiceDate" runat="server" Enabled="false"></asp:TextBox>
            </td>
            <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;"></td>
            <td class="field" style="width: 10% !important; text-align: right !important; padding-right: 5px!important;">Account No
            </td>
            <td class="input-field" style="width: 15% !important;">
                <asp:TextBox ID="txt_accountNo" runat="server" Enabled="false"></asp:TextBox>
            </td>
            <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;"></td>
            <td class="field" style="width: 10% !important; text-align: right !important; padding-right: 5px!important;">Client Name
            </td>
            <td class="input-field" style="width: 15% !important;">
                <asp:TextBox ID="txt_clientName" runat="server" Enabled="false"></asp:TextBox>
            </td>
            <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;"></td>
        </tr>
    </table>
    <div class="tbl-large">
        <asp:Label ID="lbl_new" runat="server" CssClass="error_msg"></asp:Label>
        <span id="Table_1" class="tbl-large" style="overflow: scroll; height: 95%; width: 96%; max-height: 464px;">
            <asp:GridView ID="gv_redemptions" runat="server" AutoGenerateColumns="false" CssClass="mGrid"
                BorderColor="#DEDFDE" BorderStyle="None" AllowPaging="false" BorderWidth="1px">
                <Columns>
                    <asp:BoundField HeaderText="Invoice #" DataField="invoiceNo" ItemStyle-HorizontalAlign="Center"
                        ItemStyle-Width="10%"></asp:BoundField>
                    <asp:BoundField HeaderText="Voucher#" DataField="VoucherID" />
                    <asp:BoundField HeaderText="Cheque#" DataField="ChequeNo" />
                    <asp:BoundField HeaderText="Invoice Amt" DataField="InvoiceAmount" />
                    <asp:BoundField HeaderText="Redempted Amt" DataField="RedemptedAmount" />
                    <asp:BoundField HeaderText="Voucher Amt" DataField="VoucherAmount" />
                    <asp:BoundField HeaderText="Voucher Used Amt" DataField="VoucherUsedAmount" />
                    <asp:TemplateField HeaderText="Adjustment Amt">
                        <ItemTemplate>
                            <asp:TextBox ID="txt_adjAmount" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "adjAmount") %>'
                                onchange="ReadGrid();"></asp:TextBox>
                            <asp:HiddenField ID="hd_irID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "InvoiceRedeemID") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </span>
    </div>
    <div style="width: 100%; text-align: center">
        <asp:Button ID="btn_reset" runat="server" Text="Reset" CssClass="button" OnClick="btn_reset_Click" />
        &nbsp;
        <asp:Button ID="btn_save" runat="server" Text="Save" CssClass="button" CommandName="first"
            UseSubmitBehavior="false" OnClick="btn_save_Click" />
        &nbsp;
    </div>
</asp:Content>