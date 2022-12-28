<%@ Page Language="C#" Title="CTS CN Print" AutoEventWireup="true" EnableEventValidation="False" MasterPageFile="~/BtsMasterPage.master" CodeBehind="SerachCTSCN.aspx.cs" Inherits="MRaabta.Files.SerachCTSCN" %>

<%@ Register TagPrefix="Ajax1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <table width="100%">
        <tr>
            <td colspan="12" align="center" class="head_column">
                <h3>Search CTS CNs</h3>
            </td>
        </tr>
    </table>
    <table style="padding: 20px;">
        <tr>
            <td style="float: left; width: 90px; font-weight: bold">Search CTS CN: 
            </td>
            <td style="float: left; width: 200px;">
                <asp:TextBox ID="txt_cn" runat="server"></asp:TextBox>
            </td>
            <td style="float: left; width: 100px;">
                <asp:Button ID="btn_Search" runat="server" Text="Search" CssClass="button" OnClick="btn_SearchCn"></asp:Button>
            </td>
        </tr>
    </table>
</asp:Content>
