<%@ Page Title="" Language="C#" MasterPageFile="~/BtsMasterPage.Master" AutoEventWireup="true" CodeBehind="ConsignmentBookingDetailReport.aspx.cs" Inherits="MRaabta.Files.ConsignmentBookingDetailReport" %>
<%@ Register TagPrefix="Ajax1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        .class
        {
            float: left;
            margin: 15px;
        }
        .class td
        {
            float: left;
            margin-bottom: 5px;
        }
        .class td b
        {
            float: left;
            width: 100px;
        }
        .class input
        {
            margin-right: 17px;
        }
        .class input
        {
            margin-right: 17px;
        }
        .class select
        {
            width: 145px;
            margin-right: 14px;
        }
    </style>
    <script>
        function changeCriteria() {
            var dd = document.getElementById('<%= dd_criteria.ClientID %>');
            var val = dd.options[dd.selectedIndex].value;
            var lb = document.getElementById('<%= lbl_Criteria.ClientID %>');

            if (val == "ConsignerAccountNo") {
                lb.innerText = "Account No";
            }
            else if (val == "riderCode") {
                lb.innerText = "Rider Code";
            }
        }

        function calc() {
            if (document.getElementById('<%= chk_all.ClientID %>').checked) {
                document.getElementById('<%= txt_accountNo.ClientID %>').disabled = true;
            } else {
                document.getElementById('<%= txt_accountNo.ClientID %>').disabled = false;
            }
        }
    </script>
    <table class="class">
        <tr>
            <td>
                <b>Search Criteria</b>
            </td>
            <td>
                <asp:DropDownList ID="dd_criteria" runat="server" onchange="changeCriteria()">
                    <%--<asp:ListItem Value="ConsignerAccountNo">Account No</asp:ListItem>--%>
                    <asp:ListItem Value="riderCode">Rider Code</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
                <b>
                    <asp:Label ID="lbl_Criteria" runat="server" Text="Rider Code"></asp:Label></b>
            </td>
            <td>
                <asp:TextBox ID="txt_accountNo" runat="server"></asp:TextBox>
            </td>
            <td>
                ALL<input type="checkbox" id="chk_all" title="ALL" onclick="calc();" runat="server" />
                <%--<asp:CheckBox ID="chk_all" runat="server" Text="ALL" />--%>
            </td>
        </tr>
        <tr>
            <td>
                <b>Date From</b>
            </td>
            <td>
                <asp:TextBox ID="txt_dateFrom" runat="server">
                </asp:TextBox>
                <Ajax1:CalendarExtender ID="calendar1" runat="server" TargetControlID="txt_dateFrom"
                    Format="yyyy-MM-dd">
                </Ajax1:CalendarExtender>
            </td>
            <td>
                <b>Date To</b>
            </td>
            <td>
                <asp:TextBox ID="txt_dateTo" runat="server">
                </asp:TextBox>
                <Ajax1:CalendarExtender ID="calendar2" runat="server" TargetControlID="txt_dateTo"
                    Format="yyyy-MM-dd">
                </Ajax1:CalendarExtender>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td>
                <b>Sort </b>
            </td>
            <td>
                <asp:DropDownList ID="dd_sort" runat="server">
                    <asp:ListItem Value="BookingDate">Booking Date</asp:ListItem>
                    <asp:ListItem Value="weight">Weight</asp:ListItem>
                    <asp:ListItem Value="destination">Destination</asp:ListItem>
                    <asp:ListItem Value="createdBy">User</asp:ListItem>
                    <asp:ListItem Value="ConsignmentNumber">CN Number</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
            </td>
            <td style="margin-left: 110px;">
                <asp:Button ID="btn_generateReport" runat="server" CssClass="button" Text="Generate Report"
                    OnClick="btn_generateReport_Click" />
            </td>
            <td>
            </td>
        </tr>
    </table>
</asp:Content>
