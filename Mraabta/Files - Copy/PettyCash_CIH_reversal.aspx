<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="PettyCash_CIH_reversal.aspx.cs" Inherits="MRaabta.Files.PettyCash_CIH_reversal" %>

<%@ Register TagPrefix="Ajax1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script language="javascript">
        function isNumberKey(evt) {

            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {

                return false;
            }
            return true;
        }

        function AddValidation() {

            if (document.getElementById("<%=txt_date.ClientID%>").value == "") {
                alert("Please Select Date");
                return false;
            }

            if (document.getElementById("<%=txt_amount.ClientID%>").value == "") {
                alert("Please enter Amount");
                return false;
            }
            return true;
        }

        function Confirm() {

            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("Do you want to Delete data?")) {
                confirm_value.value = "Yes";
            } else {
                confirm_value.value = "No";
            }
            document.forms[0].appendChild(confirm_value);
        }
        function NumericValidation() {
            if (event.keyCode < 48 || event.keyCode > 57) {
                event.returnValue = false;
            }
        }
        function isNumberKeydouble(evt) {

            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                if (charCode == 46) {
                    return true;
                }
                return false;
            }
            return true;
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div>
        <table cellpadding="0" cellspacing="0" width='100%' class='mGrid_Table'>
            <tr>
                <td colspan="12" align="center" class="head_column">
                    <h3>
                        Petty Cash Receipt
                    </h3>
                </td>
            </tr>
        </table>
        <style>
            .input-form tr
            {
                float: none;
                margin: 0 0 10px;
                width: 100%;
            }
            .outer_box
            {
                background: #444 none repeat scroll 0 0;
                height: 101%;
                left: 0;
                opacity: 0.9;
                position: absolute;
                top: -1%;
                width: 100%;
            }
            
            
            .pop_div
            {
                background: #eee none repeat scroll 0 0;
                border-radius: 10px;
                height: 100px;
                left: 48%;
                position: relative;
                top: 40%;
                width: 257px;
            }
            
            .btn_ok
            {
                background: #000 none repeat scroll 0 0;
                border: 0 none;
                color: #fff;
                left: -18px;
                padding: 1px 14px;
                position: relative;
                top: 67%;
            }
            
            .btn_cancel
            {
                background: #000 none repeat scroll 0 0;
                border: 0 none;
                color: #fff;
                left: 22%;
                padding: 1px 14px;
                position: relative;
                top: 42%;
            }
            
            .pop_div > span
            {
                float: left;
                line-height: 40px;
                text-align: center;
                width: 100%;
            }
            .tbl-large div
            {
                position: static;
            }
            
            .outer_box img
            {
                left: 42%;
                position: relative;
                top: 40%;
            }
        </style>
        <style>
            .search
            {
                float: right;
                width: 10%;
                background: #5f5a8d;
                padding: 3px;
                position: relative;
                right: 99px;
                margin: 0px 0px 15px;
                top: 7px;
                text-align: center;
            }
            
            .search a
            {
                color: #fff;
                text-decoration: none;
            }
            .width
            {
            }
        </style>
        <table class="input-form" style="width: 95%;">
            <tr>
                <td class="field">
                </td>
                <td class="input-field">
                    <asp:TextBox ID="txt_ID" Visible="false" Width="180px" runat="server" Enabled="false"></asp:TextBox>
                </td>
                <td class="space">
                </td>
                <td class="field">
                    Company:
                </td>
                <td class="input-field">
                    <asp:DropDownList ID="dd_company" runat="server" CssClass="dropdown width" Width="190px">
                        <%--  <asp:ListItem Text="M&P" Value="01"></asp:ListItem>
                        <asp:ListItem Text="R&R" Value="02"></asp:ListItem>--%>
                    </asp:DropDownList>
                </td>
                <td class="space">
                </td>
                <td class="field">
                </td>
                <td class="input-field">
                </td>
            </tr>
            <tr>
                <td class="field">
                    Year:
                </td>
                <td class="input-field">
                    <asp:DropDownList ID="dd_year" runat="server" CssClass="dropdown width" OnSelectedIndexChanged="dd_year_SelectedIndexChanged"
                        AutoPostBack="true" Width="190px">
                        <asp:ListItem Text="2018" Value="18"></asp:ListItem>
                        <asp:ListItem Text="2017" Value="17"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td class="space">
                </td>
                <td class="field">
                    Month:
                </td>
                <td class="input-field">
                    <asp:DropDownList ID="dd_month" Width="190px" runat="server" CssClass="dropdown width"
                        OnSelectedIndexChanged="dd_month_SelectedIndexChanged" AutoPostBack="true">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="field">
                    Zone:
                </td>
                <td class="input-field">
                    <asp:DropDownList ID="dd_zone" runat="server" CssClass="dropdown width" Width="190px"
                        AutoPostBack="true" OnSelectedIndexChanged="dd_zone_Changed">
                    </asp:DropDownList>
                </td>
                <td class="space">
                </td>
                <td class="field">
                    Branch:
                </td>
                <td class="input-field">
                    <asp:DropDownList ID="dd_branch" runat="server" CssClass="dropdown width" Width="190px">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        <table runat="server" id="tb_1" class="input-form" style="width: 95%;">
            <tr>
                <td class="field">
                    Date:
                </td>
                <td class="input-field">
                    <asp:TextBox ID="txt_date" runat="server" CssClass="med-field" onkeypress="return false;"
                        MaxLength="10" Width="180px"></asp:TextBox>
                    <Ajax1:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_date" runat="server"
                        Format="yyyy-MM-dd" PopupButtonID="Image1">
                    </Ajax1:CalendarExtender>
                </td>
                <td class="space">
                </td>
                <td class="field">
                    Amount:
                </td>
                <td class="input-field">
                    <asp:TextBox ID="txt_amount" Width="180px" runat="server" onkeypress="return isNumberKey(event);"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="field">
                    Cash Transfer Mode:
                </td>
                <td class="input-field">
                    <asp:DropDownList ID="dd_cash_mode" Width="190px" runat="server" CssClass="dropdown width"
                        AutoPostBack="true" OnSelectedIndexChanged="dd_cash_mode_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td class="space">
                </td>
                <td class="field">
                    <asp:Label ID="lbl1" runat="server" Text="Checque No:" Visible="false"></asp:Label>
                </td>
                <td class="input-field">
                    <asp:TextBox ID="txt_checque_no" Width="180px" runat="server" Visible="false" onkeypress="return isNumberKeydouble(event);"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:Button ID="btn_add" runat="server" Text="ADD AND SAVE" Width="130" CssClass="button1"
                        OnClick="btn_add_Click" OnClientClick="return AddValidation();" />
                    <br />
                    <asp:Label runat="server" ID="lbl_error" Style="color: #FF0000; font-size: medium"></asp:Label>
                </td>
            </tr>
        </table>
        <span id="Span1" class="tbl-large">
            <asp:GridView ID="GV" runat="server" AutoGenerateColumns="false" CssClass="mGrid"
                AlternatingRowStyle-CssClass="alt" BorderColor="#DEDFDE" BorderStyle="None" AllowPaging="false"
                BorderWidth="1px" OnRowDeleting="OnRowDeleting" OnRowDataBound="OnRowDataBound">
                <Columns>
                    <%--   <asp:CommandField ShowDeleteButton="True" DeleteText="Remove" ButtonType="Button"
                        ItemStyle-Width="10%" />--%>
                    <asp:BoundField HeaderText="ID" DataField="ID" ItemStyle-HorizontalAlign="Center"
                        ItemStyle-Width="10%"></asp:BoundField>
                    <asp:BoundField HeaderText="Date" DataField="date" ItemStyle-HorizontalAlign="Center"
                        ItemStyle-Width="10%"></asp:BoundField>
                    <asp:BoundField HeaderText="Amount" DataField="amount" ItemStyle-HorizontalAlign="right"
                        DataFormatString="{0:N}" ItemStyle-Width="10%"></asp:BoundField>
                    <asp:BoundField HeaderText="Transfer Mode" DataField="cash_type" ItemStyle-HorizontalAlign="left"
                        ItemStyle-Width="10%"></asp:BoundField>
                    <asp:BoundField HeaderText="Checque No." DataField="chque_no" ItemStyle-HorizontalAlign="center"
                        ItemStyle-Width="10%"></asp:BoundField>
                </Columns>
            </asp:GridView>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="btn_submit" runat="server" Text="SUBMIT" Width="130" CssClass="button1"
                UseSubmitBehavior="false" Visible="false" OnClick="btn_submit_Click" />
            &nbsp;&nbsp;<asp:Button ID="btn_clear" runat="server" Text="CLEAR ALL ENTRIES" Width="130"
                CssClass="button1" UseSubmitBehavior="false" Visible="false" OnClick="btn_clear_Click"
                OnClientClick="Confirm()" />
            <br />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Label runat="server" ID="lbl_err2" Style="color: #FF0000; font-size: medium"></asp:Label>
        </span>
        <asp:HiddenField ID="hf_status" runat="server" />
        <asp:HiddenField ID="hf_headID" runat="server" />
        <asp:HiddenField ID="hf_subheadID" runat="server" />
        <asp:HiddenField ID="hf_remaining_val" runat="server" />
        <asp:HiddenField ID="hf_remaining_pettyCash" runat="server" />
        <asp:HiddenField ID="hf_diff" runat="server" />
        <asp:HiddenField ID="hf_petty_amount" runat="server" />
        <asp:HiddenField ID="hf_amount" runat="server" />
    </div>
</asp:Content>
