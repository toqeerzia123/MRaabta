<%@ Page Title="" Language="C#" MasterPageFile="~/BtsMasterPage.Master" AutoEventWireup="true" CodeBehind="Get_DayCloseReport.aspx.cs" Inherits="MRaabta.Files.Get_DayCloseReport" %>

<%@ Register TagPrefix="Ajax1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<script>
    function NavigateReport() {
        debugger;
        var date = document.getElementById('<%= txt_date.ClientID %>');
        if (date.value == "") {
            alert('Select Date');
            return;

        }
        var win = window.open('DayCloseReport.aspx?date=' + date.value, '_blank');
        if (win) {
            win.focus();
        }
    }
</script>
    <table class="input-form" style="width:100% !important;">
        <tr>
            <td colspan="7" class="field" style="text-align: left; width:100% !important; font-family: Calibri; font-size: large;
                font-variant: small-caps;">
                <b>Day Close Report</b>
            </td>
        </tr>
        <tr>

            <td class="field" style="width:10% !important;">
                Date
            </td>
            <td class="input-field" style="width:15% !important;">
                <asp:TextBox ID="txt_date" runat="server"></asp:TextBox>
                <Ajax1:CalendarExtender ID="calendar1" runat="server" TargetControlID="txt_date"
                    Format="yyyy-MM-dd">
                </Ajax1:CalendarExtender>
            </td>
            <td class="space" style="margin:0px 0px 0px 0px !important; width:2% !important;"></td>
            <td class="field" style="width:10% !important;">
                Rider Code
            </td>
            <td class="input-field" style="width:15% !important;">
                <asp:TextBox ID="txt_riderCode" runat="server"></asp:TextBox>
                
            </td>
            <td class="space" style="margin:0px 0px 0px 0px !important; width:2% !important;"></td>
            <td class="input-field" style="width:15% !important;">
                <asp:Button ID="btn_search" runat="server" Text="S E A R C H" CssClass="button" 
                    onclick="btn_search_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
