<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="PC_Closing_QA.aspx.cs" Inherits="MRaabta.Files.PC_Closing_QA" %>

<%@ Register TagPrefix="Ajax1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        function ConfirmClose() {
            var dd_year = document.getElementById('<%= dd_year.ClientID %>');
            var dd_month = document.getElementById('<%= dd_month.ClientID %>');
            var dd_dataType = document.getElementById('<%= dd_dataType.ClientID %>');

            var year = dd_year.options[dd_year.options.selectedIndex].value;
            var month = dd_month.options[dd_month.options.selectedIndex].value;
            var dataType = dd_dataType.options[dd_dataType.options.selectedIndex].value;

            if (year == 0) {
                alert('Select Year');
                return false;
            }
            if (parseInt(month) < 1 || parseInt(month) > 12) {
                alert('Select Proper Month');
                return false;
            }
            if (dataType == "0") {
                alert('Select Data Type');
                return false;
            }

            window.open('DownloadPCClosing_QA.aspx?Year=' + year + '&Month=' + month + '&Type=' + dataType, '_blank');
            return false;
        }
    </script>
    <table cellpadding="0" cellspacing="0" width='100%' class='mGrid_Table'>
        <tr>
            <td colspan="12" align="center" class="head_column">
                <h3>
                    Petty Cash Closing
                </h3>
            </td>
        </tr>
    </table>
    <table class="input-form" style="width: 95%;">
        <tr>
            <td class="field">
                <b>Year</b>
            </td>
            <td class="input-field">
                <asp:DropDownList ID="dd_year" CssClass="dropdown" runat="server" AppendDataBoundItems="true">
                    <asp:ListItem Value="0">Select Year</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td class="space">
            </td>
            <td class="field">
                <b>Month</b>
            </td>
            <td class="input-field">
                <asp:DropDownList ID="dd_month" runat="server" AppendDataBoundItems="true" CssClass="dropdown">
                    <asp:ListItem Value="0">Select Month</asp:ListItem>
                    <asp:ListItem Value="1">January</asp:ListItem>
                    <asp:ListItem Value="2">February</asp:ListItem>
                    <asp:ListItem Value="3">March</asp:ListItem>
                    <asp:ListItem Value="4">April</asp:ListItem>
                    <asp:ListItem Value="5">May</asp:ListItem>
                    <asp:ListItem Value="6">June</asp:ListItem>
                    <asp:ListItem Value="7">July</asp:ListItem>
                    <asp:ListItem Value="8">August</asp:ListItem>
                    <asp:ListItem Value="9">September</asp:ListItem>
                    <asp:ListItem Value="10">October</asp:ListItem>
                    <asp:ListItem Value="11">November</asp:ListItem>
                    <asp:ListItem Value="12">December</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="field">
                Data Type
            </td>
            <td class="input-field">
                <asp:DropDownList ID="dd_dataType" runat="server" AppendDataBoundItems="true" CssClass="dropdown">
                    <asp:ListItem Value="0">Select Data Type</asp:ListItem>
                    <asp:ListItem Value="1">Petty Cash</asp:ListItem>
                    <asp:ListItem Value="2">Cash In Hand</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td colspan="5" class="input-field" style="text-align: center !important; width: 100% !important;">
                <asp:Button ID="btn_search" runat="server" CssClass="button" Text="Get Data" OnClientClick="if(!ConfirmClose()) return false;"
                    Width="25%" OnClick="btn_search_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
