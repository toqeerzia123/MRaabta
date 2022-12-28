<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="Search_debag.aspx.cs" Inherits="MRaabta.Files.Search_debag" %>


<%@ Register TagPrefix="Ajax1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Label ID="ErrorID" runat="server" Font-Bold="true" ForeColor="Red"></asp:Label>
    <table>
        <tr>
            <td style="float: left; text-align: right; width: 200px;">
                <b>Debag Date</b>
            </td>
            <td style="float: left; text-align: left; width: 200px;">
                <asp:TextBox ID="txt_date" runat="server"></asp:TextBox>
                <Ajax1:CalendarExtender ID="calendar1" runat="server" TargetControlID="txt_date"
                    Format="yyyy-MM-dd">
                </Ajax1:CalendarExtender>
            </td>
            <td style="float: left; text-align: center; width: 200px;">
                <asp:Button ID="btn_search" runat="server" Text="Search Debag" 
                    CssClass="button" onclick="btn_search_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
