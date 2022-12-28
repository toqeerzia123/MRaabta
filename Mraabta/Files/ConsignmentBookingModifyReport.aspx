<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="ConsignmentBookingModifyReport.aspx.cs" Inherits="MRaabta.Files.ConsignmentBookingModifyReport" %>

<%@ Register TagPrefix="Ajax1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <table style="float:left;margin:40px;">
        <tr>
            <td style="width:30%">
                <b>Search Criteria</b>
                <asp:RadioButton ID="rbAll" GroupName ="criteria" Checked="true" Text ="All" runat="server" />
                <asp:RadioButton ID="rbCash" GroupName ="criteria" Text="Cash" runat="server" />
                <asp:RadioButton ID="rbCredit" GroupName ="criteria" Text="Credit" runat="server" />
            </td>
            <td style="width:30%">
                Report Type
                <asp:RadioButton ID="rbDP" GroupName ="dpacc" Checked="true" Text ="DP Report" runat="server" />
                <asp:RadioButton ID="rbAcc" GroupName ="dpacc" Text="Account Report" runat="server" />
            </td>
            <td style="width:40%">
                <b>Edit Date</b>    
                <asp:TextBox ID="txt_dateFrom" runat="server">
                </asp:TextBox>
                <Ajax1:CalendarExtender ID="calendar1" runat="server" TargetControlID="txt_dateFrom"
                    Format="yyyy-MM-dd">
                </Ajax1:CalendarExtender>
                <asp:Button ID="btn_generateReport" runat="server" CssClass="button" Text="Generate Report"
                    OnClick="btn_generateReport_Click" />
            </td>
        
        </tr>
    </table>
</asp:Content>
