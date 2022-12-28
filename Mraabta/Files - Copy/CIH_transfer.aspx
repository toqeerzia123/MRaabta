<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="CIH_transfer.aspx.cs" Inherits="MRaabta.Files.CIH_transfer" %>

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
                    <h3>Cash In Hand Transfer
                    </h3>
                </td>
            </tr>
        </table>
        <style>
            .input-form tr {
                float: none;
                margin: 0 0 10px;
                width: 100%;
            }

            .outer_box {
                background: #444 none repeat scroll 0 0;
                height: 101%;
                left: 0;
                opacity: 0.9;
                position: absolute;
                top: -1%;
                width: 100%;
            }


            .pop_div {
                background: #eee none repeat scroll 0 0;
                border-radius: 10px;
                height: 100px;
                left: 48%;
                position: relative;
                top: 40%;
                width: 257px;
            }

            .btn_ok {
                background: #000 none repeat scroll 0 0;
                border: 0 none;
                color: #fff;
                left: -18px;
                padding: 1px 14px;
                position: relative;
                top: 67%;
            }

            .btn_cancel {
                background: #000 none repeat scroll 0 0;
                border: 0 none;
                color: #fff;
                left: 22%;
                padding: 1px 14px;
                position: relative;
                top: 42%;
            }

            .pop_div > span {
                float: left;
                line-height: 40px;
                text-align: center;
                width: 100%;
            }

            .tbl-large div {
                position: static;
            }

            .outer_box img {
                left: 42%;
                position: relative;
                top: 40%;
            }
        </style>
        <style>
            .search {
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

                .search a {
                    color: #fff;
                    text-decoration: none;
                }

            .width {
            }
        </style>
        <table class="input-form" style="width: 95%;">
            <tr>
                <td class="field">From Company:
                </td>
                <td class="input-field">
                    <asp:DropDownList ID="dd_from_company" runat="server" CssClass="dropdown width" Width="190px">
                    </asp:DropDownList>
                </td>
                <td class="space"></td>
                <td class="field">To Company:
                </td>
                <td class="input-field">
                    <asp:DropDownList ID="dd_to_company" runat="server" CssClass="dropdown width" Width="190px">
                    </asp:DropDownList>
                </td>
                <td class="space"></td>
                <td class="field"></td>
                <td class="input-field"></td>
            </tr>
            <tr>
                <td class="field">Year:
                </td>
                <td class="input-field">
                    <asp:DropDownList ID="dd_year" runat="server" CssClass="dropdown width" OnSelectedIndexChanged="dd_year_SelectedIndexChanged"
                        AutoPostBack="true" Width="190px">
                        <asp:ListItem Text="2018" Value="18"></asp:ListItem>
                        <asp:ListItem Text="2017" Value="17"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td class="space"></td>
                <td class="field">Month:
                </td>
                <td class="input-field">
                    <asp:DropDownList ID="dd_month" Width="190px" runat="server" CssClass="dropdown width"
                        OnSelectedIndexChanged="dd_month_SelectedIndexChanged" AutoPostBack="true">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="field">Zone:
                </td>
                <td class="input-field">
                    <asp:DropDownList ID="dd_zone" runat="server" CssClass="dropdown width" Width="190px"
                        AutoPostBack="true" OnSelectedIndexChanged="dd_zone_Changed">
                    </asp:DropDownList>
                </td>
                <td class="space"></td>
                <td class="field">Branch:
                </td>
                <td class="input-field">
                    <asp:DropDownList ID="dd_branch" runat="server" CssClass="dropdown width" Width="190px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="field">Date:
                </td>
                <td class="input-field">
                    <asp:TextBox ID="txt_date" runat="server" CssClass="med-field" MaxLength="10" Width="180px" onkeypress="return false;"></asp:TextBox>
                    <Ajax1:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_date" runat="server"
                        Format="dd/MM/yyyy" PopupButtonID="Image1"></Ajax1:CalendarExtender>
                </td>
                <td class="space"></td>
                <td class="field">Amount:
                </td>
                <td class="input-field">
                    <asp:TextBox ID="txt_amount" Width="180px" runat="server" onkeypress="return isNumberKey(event);"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="field">Remarks:
                </td>
                <td class="input-field">
                    <asp:TextBox ID="txt_remarks" Width="185px" Rows="3" runat="server" TextMode="MultiLine"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:Button ID="btn_submit" runat="server" Text="SUBMIT" Width="130" CssClass="button1"
                        OnClick="btn_submit_click" />
                    <br />
                    <asp:Label runat="server" ID="lbl_error" Style="color: #FF0000; font-size: medium"></asp:Label>
                    <asp:Label runat="server" ID="lbl_message"
                        Style="color: #006600; font-size: medium; font-weight: 700;"></asp:Label>
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="hf_status" runat="server" />
        <asp:HiddenField ID="hf_headID" runat="server" />
        <asp:HiddenField ID="hf_subheadID" runat="server" />
        <asp:HiddenField ID="hf_remaining_val" runat="server" />
        <asp:HiddenField ID="hf_remaining_pettyCash" runat="server" />
        <asp:HiddenField ID="hf_diff" runat="server" />
        <asp:HiddenField ID="hf_petty_amount" runat="server" />
    </div>
</asp:Content>
