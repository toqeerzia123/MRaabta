<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master" EnableViewState="true" CodeBehind="PettyCash_EF_reversal.aspx.cs" Inherits="MRaabta.Files.PettyCash_EF_reversal" %>

<%@ Register TagPrefix="Ajax1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .zoom {
            transition: transform .2s;
        }

            .zoom:hover {
                -ms-transform: scale(1.5); /* IE 9 */
                -webkit-transform: scale(1.5); /* Safari 3-8 */
                transform: scale(1.5);
            }

        .mGrid1 {
            background-color: #fff;
            border-collapse: collapse;
            font-family: Tahoma;
            font-size: 11px;
            margin: 1px 5px 0;
            width: 99%;
        }

            .mGrid1 th {
                background: #000 none repeat scroll 0 0;
                border-left: 1px solid #525252;
                color: #fff;
                font-size: 12px;
                padding: 10px 8px;
                text-align: center;
                text-transform: uppercase !important;
                white-space: nowrap;
            }

            .mGrid1 td {
                border: 1px solid #c1c1c1;
                padding: 5px;
                white-space: nowrap;
            }

            .mGrid1 .alt {
                background: #fcfcfc none repeat-x scroll center top;
            }

            .mGrid1 input {
                background-color: #ccc;
                border: 0 none;
                color: #000;
                cursor: pointer;
                font-family: Tahoma;
                font-size: 11px;
                padding: 5px;
                text-align: center;
            }
    </style>
    <script language="javascript" type="text/javascript">

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
        function number_validation(a) {
            debugger;

            var cm_ = a.id
            var cm2 = document.getElementById(cm_).value;
            if (cm2 != Math.floor(cm2)) {
                alert("Select a number");
                document.getElementById(cm_).value = "";
                document.getElementById(cm_).focus();
                return false;
            }
        }

        function checkKey(e) {
            debugger;
            var keycode;
            if (window.event) keycode = window.event.keyCode;
            else if (e) keycode = e.which;
            else return true;

            var selection = document.selection.createRange();
            var selected_text = selection.text;

            if ((keycode >= 48 && keycode <= 57) || (keycode >= 97 && keycode <= 122) || (keycode >= 65 && keycode <= 90)) {

                return false;
            }

            else return true;
        }
        function isNumberKey(evt) {

            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {

                return false;
            }
            return true;
        }
        function submit_validation() {

            if (document.getElementById("<%=dd_subdept.ClientID%>").value == "0") {
                alert("Please Select Department");
                return false;
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div>
        <table cellpadding="0" cellspacing="0" width='100%' class='mGrid_Table'>
            <tr>
                <td colspan="12" align="center" class="head_column">
                    <h3>Expense Voucher Reversal
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
                margin-left: 0px;
            }
        </style>
        <div id="div2" runat="server" class="outer_box" style="display: none; background: #FFFFFF none repeat scroll 0 0;">
            <img src="../images/Loading_Movie-02.gif" />
        </div>
        <%--  <div class="search">
                <a href="SearchBags.aspx">Search Bags</a>
            </div>--%>
        <table class="input-form" style="width: 95%;">
            <tr>
                <td class="field"></td>
                <td class="input-field">
                    <asp:TextBox ID="txt_ID" runat="server" Width="170px" Enabled="false" Visible="false"></asp:TextBox>
                </td>
                <td class="space"></td>
                <td class="field">Company:
                </td>
                <td class="input-field">
                    <asp:DropDownList ID="dd_company" Width="180px" runat="server" CssClass="dropdown width"
                        OnSelectedIndexChanged="dd_company_SelectedIndexChanged" AutoPostBack="true">
                        <%--   <asp:ListItem Text="M&P" Value="01"></asp:ListItem>
                        <asp:ListItem Text="R&R" Value="02"></asp:ListItem>--%>
                    </asp:DropDownList>
                </td>
                <td class="space"></td>
                <td class="field"></td>
                <td class="input-field"></td>
            </tr>
            <tr>
                <td class="field">Zone:
                </td>
                <td class="input-field">
                    <asp:DropDownList ID="dd_zone" runat="server" CssClass="dropdown width" Width="180px"
                        AutoPostBack="true" OnSelectedIndexChanged="dd_zone_Changed">
                    </asp:DropDownList>
                </td>
                <td class="space"></td>
                <td class="field"></td>
                <td class="input-field">
                    <%--<asp:DropDownList ID="DropDownList1" runat="server" CssClass="dropdown width" Width="190px">
                    </asp:DropDownList>--%>
                </td>
            </tr>
            <tr>
                <td class="field">Branch:
                </td>
                <td class="input-field">
                    <asp:DropDownList ID="dd_branch" runat="server" Width="180px" CssClass="dropdown width"
                        OnSelectedIndexChanged="dd_branch_SelectedIndexChanged" AutoPostBack="true">
                    </asp:DropDownList>
                </td>
                <td class="space"></td>
                <td class="field">Express Center:
                </td>
                <td class="input-field">
                    <asp:DropDownList ID="dd_ec" runat="server" Width="180px" CssClass="dropdown width">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="space"></td>
            </tr>
            <tr>
                <td class="field">Year:
                </td>
                <td class="input-field">
                    <asp:DropDownList ID="dd_year" runat="server" Width="180px" CssClass="dropdown width"
                        OnSelectedIndexChanged="dd_year_SelectedIndexChanged" AutoPostBack="true">
                        <asp:ListItem Text="2018" Value="18"></asp:ListItem>
                        <asp:ListItem Text="2017" Value="17"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td class="space"></td>
                <td class="field">Month:
                </td>
                <td class="input-field">
                    <asp:DropDownList ID="dd_month" runat="server" Width="180px" CssClass="dropdown width"
                        OnSelectedIndexChanged="dd_month_SelectedIndexChanged" AutoPostBack="true">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        <table runat="server" id="tbl_form_type" class="input-form" style="width: 95%;">
            <tr>
                <td class="field" style="width: 13%">Form Type:
                </td>
                <td class="input-field">
                    <asp:DropDownList ID="dd_eh" runat="server" Width="180px" CssClass="dropdown width"
                        AutoPostBack="true" OnSelectedIndexChanged="dd_eh_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td class="space"></td>
                <td class="field" style="width: 13%">Head Of Account:
                </td>
                <td class="input-field">
                    <asp:DropDownList ID="dd_sh" runat="server" Width="180px" CssClass="dropdown width">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="field">Date:
                </td>
                <td class="input-field">
                    <asp:TextBox ID="txt_date" runat="server" Width="170px" CssClass="med-field" MaxLength="10"
                        OnTextChanged="txt_date_TextChanged" AutoPostBack="true" onkeypress="return false;"></asp:TextBox>
                    <Ajax1:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_date" runat="server"
                        Format="yyyy-MM-dd" PopupButtonID="Image1" Enabled="true"></Ajax1:CalendarExtender>
                </td>
                <td class="space"></td>
                <td class="field">Division:
                </td>
                <td class="input-field">
                    <asp:DropDownList ID="dd_division" runat="server" Width="180px" CssClass="dropdown width">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="field">Sub Product:
                </td>
                <td class="input-field">
                    <asp:DropDownList ID="dd_sub_prod" runat="server" Width="180px" CssClass="dropdown width">
                    </asp:DropDownList>
                </td>
                <td class="space"></td>
                <td class="field">Sub Department:
                </td>
                <td class="input-field">
                    <asp:DropDownList ID="dd_subdept" runat="server" Width="180px" CssClass="dropdown width">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="field">Project:
                </td>
                <td class="input-field">
                    <asp:DropDownList ID="dd_proj" runat="server" Width="180px" CssClass="dropdown width">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="field">Narrate:
                </td>
                <td class="input-field">
                    <asp:TextBox ID="txt_narrate" runat="server" Columns="8" Width="250px" Rows="4" TextMode="MultiLine"></asp:TextBox>
                </td>
                <td class="space"></td>
                <td class="field">Amount:
                </td>
                <td class="input-field">
                    <asp:TextBox ID="txt_amount" runat="server" Width="250px" onkeypress="return isNumberKey(event);"></asp:TextBox>

                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:Label runat="server" ID="lbl_cih" Style="color: #000000; font-size: large; font-weight: 700;"></asp:Label>
                    <asp:HiddenField runat="server" ID="hf_pamount" />
                    <asp:HiddenField runat="server" ID="hf_value" />
                    <asp:HiddenField runat="server" ID="hf_available_amt" />
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:Button ID="btn_add" Visible="false" runat="server" Text="ADD AND SAVE" Width="130"
                        CssClass="button1" OnClick="btn_add_Click" OnClientClick="return submit_validation();" />
                    <br />
                    <asp:Label runat="server" ID="lbl_error" Style="color: #FF0000; font-size: medium"></asp:Label>

                </td>
            </tr>
        </table>
        <span id="Span1" class="tbl-large">
            <asp:GridView ID="GridView" runat="server" AutoGenerateColumns="false" CssClass="mGrid"
                AlternatingRowStyle-CssClass="alt" BorderColor="#DEDFDE" BorderStyle="None" AllowPaging="false"
                BorderWidth="1px" OnRowDeleting="OnRowDeleting" OnRowDataBound="OnRowDataBound">
                <Columns>
                    <asp:CommandField ShowDeleteButton="True" DeleteText="Remove" ButtonType="Button"
                        ItemStyle-Width="10%" />
                    <asp:BoundField HeaderText="ID" DataField="ID" ItemStyle-HorizontalAlign="Center"
                        ItemStyle-Width="10%"></asp:BoundField>
                    <asp:BoundField HeaderText="Date" DataField="date" ItemStyle-HorizontalAlign="Center"
                        ItemStyle-Width="10%"></asp:BoundField>
                    <asp:BoundField HeaderText="Expense Head" DataField="expense" ItemStyle-HorizontalAlign="left"
                        ItemStyle-Width="20%"></asp:BoundField>
                    <asp:BoundField HeaderText="Description" DataField="descr" ItemStyle-HorizontalAlign="left"
                        ItemStyle-Width="20%"></asp:BoundField>
                    <asp:BoundField HeaderText="Narrate" DataField="narrate" ItemStyle-HorizontalAlign="left"
                        ItemStyle-Width="30%"></asp:BoundField>
                    <asp:BoundField HeaderText="Amount" DataField="amount" ItemStyle-HorizontalAlign="right"
                        DataFormatString="{0:N}" ItemStyle-Width="10%"></asp:BoundField>
                </Columns>
            </asp:GridView>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="btn_submit" runat="server" Text="SUBMIT" Width="130" CssClass="button1"
                UseSubmitBehavior="false" Visible="false" OnClick="btn_submit_Click" />
            &nbsp;&nbsp;<asp:Button ID="btn_clear" runat="server" Text="CLEAR ALL ENTRIES" Width="130"
                CssClass="button1" Visible="false" OnClick="btn_clear_Click"
                OnClientClick="Confirm()" />
            <br />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Label runat="server" ID="lbl_err2" Style="color: #FF0000; font-size: medium"></asp:Label>
        </span>
        <span id="Span2" class="tbl-large">
            <asp:GridView ID="gridview2" runat="server" AutoGenerateColumns="false" CssClass="mGrid"
                AlternatingRowStyle-CssClass="alt" BorderColor="#DEDFDE" BorderStyle="None" AllowPaging="false"
                BorderWidth="1px" OnRowDataBound="OnRowDataBound">
                <Columns>
                    <asp:BoundField HeaderText="ID" DataField="ID" ItemStyle-HorizontalAlign="Center"
                        ItemStyle-Width="10%"></asp:BoundField>
                    <asp:BoundField HeaderText="Date" DataField="date" ItemStyle-HorizontalAlign="Center"
                        ItemStyle-Width="10%"></asp:BoundField>
                    <asp:BoundField HeaderText="Expense Head" DataField="expense" ItemStyle-HorizontalAlign="left"
                        ItemStyle-Width="20%"></asp:BoundField>
                    <asp:BoundField HeaderText="Description" DataField="description" ItemStyle-HorizontalAlign="left"
                        ItemStyle-Width="20%"></asp:BoundField>
                    <asp:BoundField HeaderText="Narrate" DataField="narrate" ItemStyle-HorizontalAlign="left"
                        ItemStyle-Width="30%"></asp:BoundField>
                    <asp:BoundField HeaderText="Amount" DataField="amount" ItemStyle-HorizontalAlign="right"
                        DataFormatString="{0:N}" ItemStyle-Width="10%"></asp:BoundField>
                </Columns>
            </asp:GridView>
        </span>
        <asp:HiddenField ID="hf_status" runat="server" />
        <asp:HiddenField ID="hf_headID" runat="server" />
        <asp:HiddenField ID="hf_subheadID" runat="server" />
</asp:Content>