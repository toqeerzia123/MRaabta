<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="ReceiptVoucherAdj.aspx.cs" Inherits="MRaabta.Files.ReceiptVoucherAdj" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="text-align: center; font-size: medium; font-weight: bold; width: 100%;
        padding-left: 20px;">
        <asp:Label ID="Errorid" runat="server"></asp:Label>
        <asp:HiddenField ID="hd_paysource" runat="server" />
        <asp:HiddenField ID="hd_VoucherDate" runat="server" />
    </div>
    <table style="font-family: Calibri; font-size: medium; padding-bottom: 0px !important;
        padding-top: 10px !important; width: 97%" class="input-form">
        <tr style="margin: 0px 0px 0px 0px !important;">
            <td class="field" style="width: 15% !important;">
                Form Mode
            </td>
            <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;">
            </td>
            <td class="field" style="width: 15% !important;">
            </td>
            <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;">
            </td>
        </tr>
        <tr>
            <td class="input-field" style="width: 15% !important;">
                <asp:RadioButtonList ID="rbtn_formMode" runat="server" RepeatDirection="Horizontal"
                    AutoPostBack="true" RepeatColumns="2" OnSelectedIndexChanged="rbtn_formMode_SelectedIndexChanged">
                    <asp:ListItem Selected="True" Value="1">New</asp:ListItem>
                    <asp:ListItem Value="2">View</asp:ListItem>
                </asp:RadioButtonList>
            </td>
            <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;">
            </td>
            <td class="input-field" style="width: 15% !important;">
            </td>
            <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;">
            </td>
            <td class="field" style="width: 7% !important;">
            </td>
            <td class="input-field" style="width: 15% !important;">
            </td>
            <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;">
            </td>
            <td class="field" style="width: 5% !important;">
            </td>
            <td class="input-field" style="width: 15% !important;">
            </td>
            <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;">
            </td>
        </tr>
    </table>
    <table style="font-family: Calibri; font-size: medium; padding-bottom: 0px !important;
        padding-top: 0px !important; width: 97%" class="input-form">
        <tr style="float: none !important;">
            <td style="float: none !important; font-variant: small-caps !important; width: 200px;
                padding-bottom: 5px !important; font-size: large; text-align: center;">
                <b>Information</b>
            </td>
        </tr>
        <tr>
            <td class="field" style="width: 15% !important; text-align: right !important; padding-right: 5px!important;">
                Voucher#
            </td>
            <td class="input-field" style="width: 15% !important;">
                <asp:TextBox ID="txt_voucherNo" runat="server" OnTextChanged="txt_voucherNo_TextChanged"
                    AutoPostBack="true"></asp:TextBox>
            </td>
            <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;">
            </td>
            <td class="field" style="width: 10% !important; text-align: right !important; padding-right: 5px!important;">
                Payment Mode
            </td>
            <td class="input-field" style="width: 15% !important;">
                <asp:TextBox ID="txt_paymentMode" runat="server" Text="" Enabled="false"></asp:TextBox>
            </td>
            <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;">
            </td>
            <td class="field" style="width: 10% !important; text-align: right !important; padding-right: 5px!important;">
                Express Center
            </td>
            <td class="input-field" style="width: 15% !important;">
                <asp:TextBox ID="txt_expressCenter" runat="server" Enabled="false"></asp:TextBox>
            </td>
            <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;">
            </td>
        </tr>
        <tr>
            <td class="field" style="width: 15% !important; text-align: right !important; padding-right: 5px!important;">
                Rider
            </td>
            <td class="input-field" style="width: 15% !important;">
                <asp:TextBox ID="txt_rider" runat="server" Enabled="false"></asp:TextBox>
            </td>
            <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;">
            </td>
            <td class="field" style="width: 10% !important; text-align: right !important; padding-right: 5px!important;">
                Pay Source
            </td>
            <td class="input-field" style="width: 15% !important;">
                <asp:TextBox ID="txt_paySource" runat="server" Enabled="false"></asp:TextBox>
                
                
            </td>
            <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;">
            </td>
            <td class="field" style="width: 10% !important; text-align: right !important; padding-right: 5px!important;">
                Client Name
            </td>
            <td class="input-field" style="width: 15% !important;">
                <asp:TextBox ID="txt_clientName" runat="server" Enabled="false"></asp:TextBox>
            </td>
            <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;">
            </td>
        </tr>
        <tr>
            <td class="field" style="width: 15% !important; text-align: right !important; padding-right: 5px!important;">
                Bank
            </td>
            <td class="input-field" style="width: 15% !important;">
                <asp:TextBox ID="txt_bank" runat="server" Enabled="false"></asp:TextBox>
            </td>
            <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;">
            </td>
            <td class="field" style="width: 10% !important; text-align: right !important; padding-right: 5px!important;">
                Cheque#
            </td>
            <td class="input-field" style="width: 15% !important;">
                <asp:TextBox ID="txt_chequeNo" runat="server" Enabled="false"></asp:TextBox>
            </td>
            <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;">
            </td>
            <td class="field" style="width: 10% !important; text-align: right !important; padding-right: 5px!important;">
                Cheque Date
            </td>
            <td class="input-field" style="width: 15% !important;">
                <asp:TextBox ID="txt_chequeDate" runat="server" Enabled="false"></asp:TextBox>
            </td>
            <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;">
            </td>
        </tr>
        <tr>
            <td class="field" style="width: 15% !important; text-align: right !important; padding-right: 5px!important;">
                Voucher Amount
            </td>
            <td class="input-field" style="width: 15% !important;">
                <asp:TextBox ID="txt_VoucherAmount" runat="server" Enabled="false"></asp:TextBox>
            </td>
            <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;">
            </td>
            <td class="field" style="width: 10% !important; text-align: right !important; padding-right: 5px!important;">
                Used Amount
            </td>
            <td class="input-field" style="width: 15% !important;">
                <asp:TextBox ID="txt_usedAmount" runat="server" Enabled="false"></asp:TextBox>
            </td>
            <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;">
            </td>
            <td class="field" style="width: 10% !important; text-align: right !important; padding-right: 5px!important;">
                Adj Amount
            </td>
            <td class="input-field" style="width: 15% !important;">
                <asp:TextBox ID="txt_adjAmount" runat="server" Enabled="true"></asp:TextBox>
            </td>
            <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;">
            </td>
        </tr>
    </table>
    <div class="tbl-large">
        <asp:GridView ID="gv_productWiseAmount" runat="server" AutoGenerateColumns="false"
            CssClass="mGrid">
            <Columns>
                <asp:BoundField HeaderText="Product" DataField="ProductDisplay" />
                <asp:TemplateField HeaderText="Amount">
                    <ItemTemplate>
                        <asp:TextBox ID="txt_amount" runat="server" onchange="checkAmounts();" onkeypress="return isNumberKey(event);"></asp:TextBox>
                        <asp:HiddenField ID="hd_product" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "Products") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <div style="width: 100%; text-align: center">
        <asp:Button ID="btn_reset" runat="server" Text="Reset" CssClass="button" OnClick="btn_reset_Click" />
        &nbsp;
        <asp:Button ID="btn_save" runat="server" Text="Save" CssClass="button" CommandName="first"
            OnClick="btn_save_Click" />
        &nbsp;
    </div>
    <asp:HiddenField ID="hd_paymentType" runat="server" />
    <asp:HiddenField ID="hd_closed" runat="server" />
    <asp:HiddenField ID="hd_paid" runat="server" />
</asp:Content>
