<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="PC_ClosingBalances_QA.aspx.cs" Inherits="MRaabta.Files.PC_ClosingBalances_QA" %>

<%@ Register TagPrefix="Ajax1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table cellpadding="0" cellspacing="0" width='100%' class='mGrid_Table'>
        <tr>
            <td colspan="12" align="center" class="head_column">
                <h3>Petty Cash Closing
                </h3>
            </td>
        </tr>
    </table>
    <table class="input-form" style="width: 95%;">
        <tr>
            <td class="field" style="width: 100px !important;">
                <b>Date</b>
            </td>
            <td class="input-field" style="width: 150px !important;">
                <asp:TextBox ID="txt_date" runat="server" CssClass="textBox" BorderColor="Black"></asp:TextBox>
                <Ajax1:CalendarExtender ID="calendar1" runat="server" Format="yyyy-MM-dd" TargetControlID="txt_date">
                </Ajax1:CalendarExtender>
            </td>
            <td class="space"></td>
            <td class="input-field" style="width: 150px !important;">
                <asp:Button ID="btn_search" runat="server" CssClass="button" Text="Get Data" OnClientClick="if(!ConfirmClose()) return false;"
                    Width="100%" OnClick="btn_search_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
