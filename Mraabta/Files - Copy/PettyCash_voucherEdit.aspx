<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="PettyCash_voucherEdit.aspx.cs" Inherits="MRaabta.Files.PettyCash_voucherEdit" %>

<%@ Register TagPrefix="Ajax1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .mGrid1
        {
            background-color: #fff;
            border-collapse: collapse;
            font-family: Tahoma;
            font-size: 11px;
            margin: 1px 5px 0;
            width: 99%;
        }
        .mGrid1 th
        {
            background: #000 none repeat scroll 0 0;
            border-left: 1px solid #525252;
            color: #fff;
            font-size: 12px;
            padding: 10px 8px;
            text-align: center;
            text-transform: uppercase !important;
            white-space: nowrap;
        }
        
        .mGrid1 td
        {
            border: 1px solid #c1c1c1;
            padding: 5px;
            white-space: nowrap;
        }
        
        .mGrid1 .alt
        {
            background: #fcfcfc none repeat-x scroll center top;
        }
        
        .mGrid1 input
        {
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
    <script language="javascript">
        function isNumberKey(evt) {

            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {

                return false;
            }
            return true;
        }
    </script>
    <%-- <script language="javascript">
        function AddValidation() {

//            if (document.getElementById("<%=txt_date.ClientID%>").value == "") {
//                alert("Please Select Date");
//                return false;
//            }

//            if (document.getElementById("<%=dd_eh.ClientID%>").value == "") {
//                alert("Please Select Expense");
//                return false;

//            }

//            if (document.getElementById("<%=dd_subhead.ClientID%>").value == "") {
//                alert("Please Select Description");
//                return false;
//            }

//            if (document.getElementById("<%=txt_narrate.ClientID%>").value == "") {
//                alert("Please Write The Narrate");
//                return false;
//            }

//            if (document.getElementById("<%=txt_amount.ClientID%>").value == "") {
//                alert("Please enter Amount");
//                return false;
//            }
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

    </script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div>
        <table cellpadding="0" cellspacing="0" width='100%' class='mGrid_Table'>
            <tr>
                <td colspan="12" align="center" class="head_column">
                    <h3>
                        Expense Voucher Edit
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
                <td class="field">
                    ID:
                </td>
                <td class="input-field">
                    <asp:TextBox ID="txt_ID" runat="server" Width="250px" Enabled="false"></asp:TextBox>
                </td>
                <td class="space">
                </td>
                <td class="field">
                    Company:
                </td>
                <td class="input-field">
                    <asp:DropDownList ID="dd_company" Width="180px" runat="server" CssClass="dropdown width"
                        OnSelectedIndexChanged="dd_company_SelectedIndexChanged" AutoPostBack="true">
                        <%--   <asp:ListItem Text="M&P" Value="01"></asp:ListItem>
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
                    Branch:
                </td>
                <td class="input-field">
                    <asp:TextBox ID="dd_branch" runat="server" Width="250px" Enabled="false"></asp:TextBox>
                </td>
                <td class="space">
                </td>
                <td class="field">
                    Express Center:
                </td>
                <td class="input-field">
                    <asp:TextBox ID="dd_ec" runat="server" Width="250px" Enabled="false"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="field">
                    Year:
                </td>
                <td class="input-field">
                    <asp:TextBox ID="dd_year" runat="server" Width="250px" Enabled="false"></asp:TextBox>
                </td>
                <td class="space">
                </td>
                <td class="field">
                    Month:
                </td>
                <td class="input-field">
                    <asp:DropDownList ID="dd_month" runat="server" CssClass="dropdown width" Enabled="false">
                        <asp:ListItem Text="January" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Febuary" Value="2"></asp:ListItem>
                        <asp:ListItem Text="March" Value="3"></asp:ListItem>
                        <asp:ListItem Text="April" Value="4"></asp:ListItem>
                        <asp:ListItem Text="May" Value="5"></asp:ListItem>
                        <asp:ListItem Text="June" Value="6"></asp:ListItem>
                        <asp:ListItem Text="July" Value="7"></asp:ListItem>
                        <asp:ListItem Text="August" Value="8"></asp:ListItem>
                        <asp:ListItem Text="September" Value="9"></asp:ListItem>
                        <asp:ListItem Text="October" Value="10"></asp:ListItem>
                        <asp:ListItem Text="November" Value="11"></asp:ListItem>
                        <asp:ListItem Text="December" Value="12"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        <table class="input-form" style="width: 95%;">
            <tr>
                <td class="field">
                    Date:
                </td>
                <td class="input-field">
                    <asp:TextBox ID="txt_date" runat="server" CssClass="med-field" MaxLength="10" Width="250"></asp:TextBox>
                    <Ajax1:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_date" runat="server"
                        Format="dd/MM/yyyy" PopupButtonID="Image1">
                    </Ajax1:CalendarExtender>
                </td>
                <td class="space">
                </td>
            </tr>
            <tr>
                <td class="field">
                    Form Type:
                </td>
                <td class="input-field">
                    <asp:DropDownList ID="dd_eh" runat="server" CssClass="dropdown width" AutoPostBack="true"
                        OnSelectedIndexChanged="dd_eh_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td class="space">
                </td>
                <td class="field">
                    Head Of Account:
                </td>
                <td class="input-field">
                    <asp:DropDownList ID="dd_subhead" runat="server" CssClass="dropdown width">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="field">
                    Division:
                </td>
                <td class="input-field">
                    <asp:DropDownList ID="dd_division" runat="server" Width="180px" CssClass="dropdown width">
                    </asp:DropDownList>
                </td>
                <td class="space">
                </td>
                <td class="field">
                    Project:
                </td>
                <td class="input-field">
                    <asp:DropDownList ID="dd_proj" runat="server" Width="180px" CssClass="dropdown width">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="field">
                    Sub Product:
                </td>
                <td class="input-field">
                    <asp:DropDownList ID="dd_sub_prod" runat="server" Width="180px" CssClass="dropdown width">
                    </asp:DropDownList>
                </td>
                <td class="space">
                </td>
                <td class="field">
                    Sub sDepartment:
                </td>
                <td class="input-field">
                    <asp:DropDownList ID="dd_subdept" runat="server" Width="180px" CssClass="dropdown width">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="field">
                    Narrate:
                </td>
                <td class="input-field">
                    <asp:TextBox ID="txt_narrate" runat="server" Columns="8" Width="250px" Rows="4" TextMode="MultiLine"></asp:TextBox>
                </td>
                <td class="space">
                </td>
                <td class="field">
                    Amount:
                </td>
                <td class="input-field">
                    <asp:TextBox ID="txt_amount" runat="server" Width="250px" onkeypress="return isNumberKey(event);"></asp:TextBox>
                </td>
                <%--   <tr>
                    <td class="field">
                        Status:
                    </td>
                    <td class="input-field">
                        <asp:DropDownList ID="dd_status" runat="server" CssClass="dropdown width">
                            <asp:ListItem Text="Select Status" Value=""></asp:ListItem>
                            <asp:ListItem Text="Unapproved" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Approved" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Rejected" Value="2"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>--%>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:Label runat="server" ID="lbl_stat" Style="color: #000000; font-size: medium;
                        font-weight: 700;"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                <asp:HiddenField runat="server" ID="hf_pamount" />
                <asp:HiddenField runat="server" ID="hf_bcode" />
                    <asp:Button ID="btn_add" runat="server" Text="SAVE" Width="130" CssClass="button1"
                        OnClick="btn_add_Click" />
                    <asp:Button ID="btn_back" runat="server" Text="BACK" Width="130" CssClass="button1"
                        OnClick="btn_back_Click" />
                    <%--  <a href="javascript:history.go(-1)">Go back</a>--%>
                    <br />
                    <asp:Label runat="server" ID="lbl_error" Style="color: #FF0000; font-size: medium"></asp:Label>
                    <asp:Label runat="server" ID="Label1" Style="color: #009900; font-size: medium"></asp:Label>
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="hf_user" runat="server" />
        <asp:HiddenField ID="hf_status" runat="server" />
        <asp:HiddenField ID="hf_headID" runat="server" />
        <asp:HiddenField ID="hf_ID" runat="server" />
        <asp:HiddenField ID="hf_old_amount" runat="server" />
</asp:Content>
