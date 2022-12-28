<%@ Page Language="C#" AutoEventWireup="true" Title="International Screen" MasterPageFile="~/BtsMasterPage.master" CodeBehind="ec_internationalConsignment.aspx.cs" Inherits="MRaabta.Files.ec_internationalConsignment" %>

<%@ Register TagPrefix="Ajax1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:HiddenField ID="ecCode" runat="server" />
    <asp:HiddenField ID="hd_unApproveable" runat="server" />
    <script type="text/javascript" language="javascript">
        debugger
        function handleEnter(event) {
            var keyCode = event.keyCode ? event.keyCode : event.which ? event.which : event.charCode;
            if (keyCode == 13 || keyCode == 189 || keyCode == 109) {
                document.getElementById(obj).click();
                return false;
            }
            else {
                return true;
            }
        }

        function isNumberKey(evt) {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            return true;
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
        function CNLenChange() {

            DisableApprove();
            var d = document.getElementById('<%= dd_origin.ClientID %>');
            var dd = d.options[d.selectedIndex];
            var rlist = document.getElementById('<%= rbtn_consignmentSender.ClientID %>');
            for (var i = 0; i < rlist.rows[0].cells.length; i++) {

                if (rlist.rows[0].cells[i].firstChild.checked) {
                    var sender = rlist.rows[0].cells[i].firstChild.value;
                    var cn = document.getElementById('<%= txt_consignmentNo.ClientID %>');
                    if (sender == "1") {
                        cn.value = "";
                        cn.maxLength = 11;
                    }
                    else if (sender == "4") {
                        cn.value = "";
                        cn.maxLength = 12;
                    }
                }
            }
        }
        function DisableApprove() {

            var btn_save = document.getElementById('<%= btn_save.ClientID %>');
            btn_save.disabled = true;
        }
        function CNTypeHandle(dd) {

            var lbl = document.getElementById('<%= lbl_cupon.ClientID %>');
            var txt = document.getElementById('<%= txt_cupon.ClientID %>');
            var cnType = dd.options[dd.selectedIndex].value;
            DisableApprove();
            if (cnType == "13") {
                lbl.innerText = "Flyer No.";
                txt.disabled = false;
            }
            else {
                lbl.value = "Coupon No.";
                txt.disabled = false;
            }
        }
        function AccountChange(accNo) {

            var chAmount = document.getElementById('<%= txt_chargedamount.ClientID %>');
            if (accNo.value == "0") {
                chAmount.disabled = false;
            }
            else {
                chAmount.disabled = true;
            }

        }

        function InsuranceCheck(chk) {
            var declaredValue = document.getElementById('<%= txt_declaredValue.ClientID %>');
            if (chk.checked) {

                declaredValue.disabled = false;

            }
            else {
                // declaredValue.disabled = true;
            }
        }

        function RiderFreeze(chk) {
            var rider = document.getElementById('<%= txt_riderCode.ClientID %>');
            if (chk.checked) {
                if (rider.value == "") {
                    alert('Enter Rider Code');
                    chk.checked = false;
                    return;
                }
                rider.disabled = true;
            }
            else {
                chk.checked = false;
            }
        }
        function SaveChecks() {

            var btn_Save = document.getElementById('<%= btn_save.ClientID %>');
            if (btn_Save.disabled == true) {
                alert('Please Validate First');
                return false;
            }
            else {
                return true;
            }
        }

        function PrintReceipt() {
            var cn = document.getElementById('<%= txt_consignmentNo.ClientID %>');
            var win = window.open('InternationalPrint.aspx?Xcode=' + cn.value, '_blank');
            if (win) {
                win.focus();
            }
        }

        function PaymentModeChange() {
            var saveButton = document.getElementById('<%= dd_PaymentMode.ClientID %>');
            val_ = saveButton.options[saveButton.selectedIndex].value
            if (val_ == "6") {
                document.getElementById('<%= txt_TransactionID.ClientID %>').maxLength = "6";
                document.getElementById('<%= txt_TransactionID.ClientID %>').disabled = false;
                document.getElementById('<%= txt_TransactionID.ClientID %>').value = "0";
                
            }
            else if (val_ == "7") {
                document.getElementById('<%= txt_TransactionID.ClientID %>').maxLength = "10";
                document.getElementById('<%= txt_TransactionID.ClientID %>').disabled = false;
                document.getElementById('<%= txt_TransactionID.ClientID %>').value = "0";
                
            }
            else {
                document.getElementById('<%= txt_TransactionID.ClientID %>').Text = "0";
                document.getElementById('<%= txt_TransactionID.ClientID %>').disabled = true;
                document.getElementById('<%= txt_TransactionID.ClientID %>').value = "0";

            }


        }

    </script>
    <script src="javascripts/jquery.js">
    jQuery.extend(jQuery.expr[':'], {
    focusable: function (el, index, selector) {
        return $(el).is('a, button, :input, [tabindex]');
    }
});

$(document).on('keypress', 'input,select', function (e) {
    if (e.which == 13) {
        e.preventDefault();
        // Get all focusable elements on the page
        var $canfocus = $(':focusable');
        var index = $canfocus.index(document.activeElement) + 1;
        if (index >= $canfocus.length) index = 0;
        $canfocus.eq(index).focus();
    }
    </script>
    <style>
        .input-field > input
        {
            width: auto;
        }
        
        
        #main_table tr td
        {
            padding-bottom: 10px;
        }
        
        #ContentPlaceHolder1_CalendarExtender_daysBody td
        {
            padding: 0 !important;
        }
        
        #main_table tr td
        {
            padding-bottom: 10px;
        }
    </style>
    <div>
        <table cellpadding="0" cellspacing="0" width='100%' class='mGrid_Table'>
            <tr>
                <td colspan="12" align="center" class="head_column">
                    <h3>
                        International Manage Consignment</h3>
                </td>
            </tr>
        </table>
        <fieldset class="fieldsetSmall" style="font-size: medium;">
            <asp:Label ID="lbl_error" runat="server" ForeColor="Red"></asp:Label>
            <table id="main_table">
                <tr>
                    <td class="cellNormal">
                        User Type
                    </td>
                    <td class="cellTextField">
                        <asp:Label ID="lbl_userType" runat="server" Text="Express Center User" Font-Bold="true"></asp:Label>
                    </td>
                    <td colspan="5">
                        <asp:RadioButtonList ID="rbtn_consignmentSender" runat="server" AutoPostBack="true"
                            onclick="CNLenChange();" RepeatDirection="Horizontal" RepeatColumns="2" OnSelectedIndexChanged="rbtn_consignmentSender_SelectedIndexChanged">
                        </asp:RadioButtonList>
                    </td>
                    <td class="cellTextField">
                        <asp:CheckBox ID="chk_riderCode" runat="server" Text="Save Rider Code" OnCheckedChanged="chk_riderCode_CheckedChanged"
                            AutoPostBack="false" onclick="RiderFreeze(this);" />
                    </td>
                </tr>
                <tr>
                    <td class="cellNormal">
                        Consignment No.
                    </td>
                    <td class="cellTextField">
                        <asp:TextBox ID="txt_consignmentNo" runat="server" onkeypress="return isNumberKey(event);"
                            Width="160px" OnTextChanged="txt_consignmentNo_TextChanged" onchange="DisableApprove();"
                            onblur="validateElement('Configuration', 'testSuiteConfigurationform','totalRetryCount')">
                        </asp:TextBox>
                    </td>
                    <td class="cellNormal">
                        Account No.
                    </td>
                    <td class="cellTextField">
                        <asp:TextBox ID="txt_accountNo" runat="server" onkeydown="return (event.keyCode!=13);"
                            Width="160px" OnTextChanged="txt_accountNo_TextChanged" AutoPostBack="false"
                            onchange="AccountChange(this);">
                        </asp:TextBox>
                        <asp:HiddenField ID="hd_customerType" runat="server" />
                        <%--<asp:TextBox ID="txt_accountNo" runat="server" CssClass="textBox" OnTextChanged="txt_accountNo_TextChanged"
                            AutoPostBack="true"></asp:TextBox>--%>
                    </td>
                    <td class="cellNormal">
                        Consigner
                    </td>
                    <td class="cellTextField">
                        <asp:TextBox ID="txt_consigner" runat="server" onkeydown="return (event.keyCode!=13);"
                            Width="160px">
                        </asp:TextBox>
                        <%--<asp:TextBox ID="txt_consigner" runat="server" CssClass="textBox"></asp:TextBox>--%>
                        <asp:HiddenField ID="hd_creditClientID" runat="server" />
                    </td>
                    <td class="cellNormal">
                        <asp:Label ID="lbl_cupon" runat="server" Text="Cupon No."></asp:Label>
                    </td>
                    <td class="cellTextField">
                        <asp:TextBox ID="txt_cupon" runat="server" onkeydown="return (event.keyCode!=13);"
                            Width="160px">
                        </asp:TextBox>
                        <%--<asp:TextBox ID="txt_cupon" runat="server" CssClass="textBox" Text="0" Enabled="false"></asp:TextBox>--%>
                    </td>
                </tr>
                <tr>
                    <td class="cellNormal">
                        Consigner Cell No.
                    </td>
                    <td class="cellTextField">
                        <asp:TextBox ID="txt_consignerCellNo" runat="server" onkeydown="return (event.keyCode!=13);"
                            onkeypress="return isNumberKey(event);" Width="160px">
                        </asp:TextBox>
                        <%--<asp:TextBox ID="txt_consignerCellNo" runat="server" CssClass="textBox"></asp:TextBox>--%>
                    </td>
                    <td class="cellNormal">
                        Consigner CNIC
                    </td>
                    <td class="cellTextField">
                        <asp:TextBox ID="txt_consignerCNIC" runat="server" skin="Web20" MaxLength="13" Width="160px"
                            onkeypress="return isNumberKey(event);">
                        </asp:TextBox>
                        <%-- <asp:TextBox ID="txt_consignerCNIC" runat="server" CssClass="textBox"></asp:TextBox>--%>
                    </td>
                    <td class="cellNormal">
                        Consignee
                    </td>
                    <td class="cellTextField">
                        <asp:TextBox ID="txt_consignee" runat="server" MaxLength="40" onkeydown="return (event.keyCode!=13);"
                            Width="160px">
                        </asp:TextBox>
                        <%--<asp:TextBox ID="txt_consignee" runat="server" CssClass="textBox"></asp:TextBox>--%>
                    </td>
                    <td class="cellNormal">
                        Cell No.
                    </td>
                    <td class="cellTextField">
                        <asp:TextBox ID="txt_cellNo" runat="server" MaxLength="12" onkeydown="return (event.keyCode!=13);"
                            onkeypress="return isNumberKey(event);" Width="160px">
                        </asp:TextBox>
                        <%--<asp:TextBox ID="txt_cellNo" runat="server" CssClass="textBox"></asp:TextBox>--%>
                    </td>
                </tr>
                <tr>
                    <td class="cellNormal">
                        Consignee Attn.
                    </td>
                    <td class="cellTextField">
                        <asp:TextBox ID="txt_consigneeAttn" runat="server" onkeydown="return (event.keyCode!=13);"
                            Width="160px">
                        </asp:TextBox>
                        <%--<asp:TextBox ID="txt_consigneeAttn" runat="server" CssClass="textBox"></asp:TextBox>--%>
                    </td>
                    <td class="cellNormal">
                        Con. Type
                    </td>
                    <td class="cellTextField">
                        <asp:DropDownList ID="dd_conType" runat="server" CssClass="dropdown" OnSelectedIndexChanged="dd_conType_SelectedIndexChanged"
                            onchange="CNTypeHandle(this);" AutoPostBack="false">
                        </asp:DropDownList>
                    </td>
                    <td class="cellNormal">
                        Origin
                    </td>
                    <td class="cellTextField">
                        <asp:DropDownList ID="dd_origin" runat="server" CssClass="dropdown">
                        </asp:DropDownList>
                    </td>
                    <td class="cellNormal">
                        Forwarding Station
                    </td>
                    <td class="cellTextField">
                        <asp:DropDownList ID="dd_destination" runat="server" CssClass="dropdown" OnSelectedIndexChanged="dd_destination_SelectedIndexChanged"
                            AppendDataBoundItems="true" AutoPostBack="false">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="cellNormal">
                        Dest. Country
                    </td>
                    <td class="cellTextField">
                        <asp:DropDownList ID="dd_destCountry" runat="server" CssClass="dropdown" AutoPostBack="false"
                            AppendDataBoundItems="true" OnSelectedIndexChanged="dd_destCountry_SelectedIndexChanged"
                            onchange="DisableApprove();">
                            <asp:ListItem Value="0">Select Dest Country</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td class="cellNormal">
                        Service Type
                    </td>
                    <td class="cellTextField">
                        <asp:DropDownList ID="dd_serviceType" runat="server" CssClass="dropdown" OnSelectedIndexChanged="dd_serviceType_SelectedIndexChanged"
                            onchange="DisableApprove();" AutoPostBack="false" AppendDataBoundItems="true">
                            <asp:ListItem Value="0">Select Service Type</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td class="cellNormal">
                        W
                    </td>
                    <td class="cellTextField">
                        <asp:TextBox ID="txt_w" runat="server" MaxLength="12" Width="30px" Enabled="false"
                            onkeydown="return (event.keyCode!=13);">
                        </asp:TextBox>
                        <%--<asp:TextBox ID="txt_w" runat="server" CssClass="textBox" Width="30px" Style="text-align: right;
                            font-size: small; font-family: Calibri;" Text="0" Enabled="false"></asp:TextBox>--%>
                        &nbsp; H
                        <asp:TextBox ID="txt_h" runat="server" MaxLength="12" Width="30px" Enabled="false"
                            onkeydown="return (event.keyCode!=13);">
                        </asp:TextBox>
                        <%-- <asp:TextBox ID="txt_h" runat="server" CssClass="textBox" Width="30px" Style="text-align: right;
                            font-size: small; font-family: Calibri;" Text="0" Enabled="false"></asp:TextBox>--%>
                        &nbsp; B
                        <asp:TextBox ID="txt_b" runat="server" MaxLength="12" Width="30px" Enabled="false"
                            onkeydown="return (event.keyCode!=13);">
                        </asp:TextBox>
                        <%--<asp:TextBox ID="txt_b" runat="server" CssClass="textBox" Width="30px" Style="text-align: right;
                            font-size: small; font-family: Calibri;" Text="0" Enabled="false"></asp:TextBox>--%>
                    </td>
                    <td class="cellNormal">
                        Weight
                    </td>
                    <td class="cellTextField">
                        <asp:TextBox ID="txt_weight" runat="server" Text="0.5" onkeypress="return isNumberKeydouble(event);"
                            onchange="DisableApprove();" MaxLength="4" Width="58px">
                            
                        </asp:TextBox>
                        <%--<asp:TextBox ID="txt_weight" runat="server" CssClass="textBox" Width="60px" Text="0"
                            Style="text-align: right;"></asp:TextBox>--%>
                        &nbsp; Disc.
                        <asp:TextBox ID="txt_discount" runat="server" MaxLength="12" Enabled="false" onkeydown="return (event.keyCode!=13);"
                            Width="58px">
                        </asp:TextBox>
                        <%-- <asp:TextBox ID="txt_discount" runat="server" CssClass="textBox" Width="58px" Text="0"
                            Style="text-align: right;" Enabled="false"></asp:TextBox>--%>
                    </td>
                </tr>
                <tr>
                    <td class="cellNormal">
                        Rider Code
                    </td>
                    <td class="cellTextField">
                        <asp:TextBox ID="txt_riderCode" runat="server" MaxLength="12" AutoPostBack="false"
                            onkeydown="return (event.keyCode!=13);" Width="160px" OnTextChanged="txt_riderCode_TextChanged"
                            onchange="DisableApprove();">
                        </asp:TextBox>
                        <%--<asp:TextBox ID="txt_riderCode" runat="server" CssClass="textBox" OnTextChanged="txt_riderCode_TextChanged"
                            AutoPostBack="true"></asp:TextBox>--%>
                    </td>
                    <td class="cellNormal">
                        Pieces
                    </td>
                    <td class="cellTextField">
                        <asp:TextBox ID="txt_pieces" runat="server" Text="1" MaxLength="12" Width="80px"
                            onkeydown="return (event.keyCode != 109 && event.keyCode != 189 && event.keyCode != 13)"
                            onchange="DisableApprove();">            
                        </asp:TextBox>
                        <%--<asp:TextBox ID="txt_pieces" runat="server" CssClass="textBox" Width="80px" Text="0"
                            Style="text-align: right;"></asp:TextBox>--%>
                        &nbsp; Insurance
                        <asp:CheckBox ID="chk_insurance" runat="server" CssClass="checkBox" Style="float: right"
                            OnCheckedChanged="chk_insurance_CheckedChanged" AutoPostBack="false" onclick="InsuranceCheck(this);" />
                    </td>
                    <td class="cellNormal">
                        Insurance%
                    </td>
                    <td class="cellTextField">
                        <asp:TextBox ID="txt_insurance" runat="server" MaxLength="12" Text="2.5%" Width="160px"
                            Enabled="false">
                            
                        </asp:TextBox>
                        <%--<asp:TextBox ID="txt_insurance" runat="server" CssClass="textBox" Style="text-align: right;"
                            Text="2.5%"></asp:TextBox>--%>
                    </td>
                    <td class="cellNormal">
                        Declared Value
                    </td>
                    <td class="cellTextField">
                        <asp:TextBox ID="txt_declaredValue" runat="server" MaxLength="7" Width="160px" Enabled="true"
                            onchange="DisableApprove();">
                            
                        </asp:TextBox>
                        <%--<asp:TextBox ID="txt_declaredValue" runat="server" CssClass="textBox" Text="0" Enabled="false"
                            Style="text-align: right;"></asp:TextBox>--%>
                    </td>
                </tr>
                <tr>
                    <td class="cellNormal">
                        Booking Date
                    </td>
                    <td class="cellTextField">
                        <%-- <telerik:RadTextBox ID="txt_bookingDate" runat="server" Skin="Web20" LabelWidth="64px"
                            Resize="None" ResolvedRenderMode="Classic" Width="160px" Enabled="false">
                        </telerik:RadTextBox>--%>
                        <%--<asp:TextBox ID="txt_bookingDate" runat="server" CssClass="textBox" Enabled="false"></asp:TextBox>--%>
                        <asp:TextBox ID="txt_bookingDate" runat="server" CssClass="med-field" MaxLength="10"></asp:TextBox>
                        <Ajax1:CalendarExtender ID="CalendarExtender" TargetControlID="txt_bookingDate" runat="server"
                            Format="yyyy-MM-dd" PopupButtonID="Image1">
                        </Ajax1:CalendarExtender>
                    </td>
                    <td>
                    </td>
                    <td class="cellNormal">
                        Package Content
                    </td>
                    <td class="cellTextField" colspan="2" style="padding-right: 3px;">
                        <asp:TextBox ID="txt_packageContent" runat="server" Width="98%">
                        </asp:TextBox>
                        <%--<asp:TextBox ID="txt_packageContent" runat="server" CssClass="textBox" Width="98%"></asp:TextBox>--%>
                    </td>
                </tr>
                <tr>
                    <td class="cellNormal" style="width: 155px">
                        Total Amount
                    </td>
                    <td class="cellTextField">
                        <asp:TextBox ID="txt_totalAmount" runat="server" MaxLength="7" Enabled="false" onkeypress="return isNumberKeydouble(event);">
                            
                        </asp:TextBox>
                    </td>
                    <td class="cellNormal" style="width: 155px">
                        Payment Mode
                    </td>
                    <td>
                        <asp:DropDownList ID="dd_PaymentMode" runat="server" AppendDataBoundItems="true"
                            onchange="PaymentModeChange();">
                            <asp:ListItem Text="Select PaymentMode" Value="0" />
                        </asp:DropDownList>
                    </td>
                    <td class="cellNormal" style="width: 155px">
                        Payment Transaction ID
                    </td>
                    <td>
                        <asp:TextBox ID="txt_TransactionID" value="0" Enabled="false" runat="server" Width="100px"
                            onkeypress="return isNumberKey(event);"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="cellNormal" style="width: 155px">
                        GST
                    </td>
                    <td class="cellTextField">
                        <asp:TextBox ID="txt_gst" runat="server" MaxLength="7" Enabled="false" onkeypress="return isNumberKeydouble(event);">
                            
                        </asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="cellNormal" style="width: 155px">
                        Charged Amount
                    </td>
                    <td class="cellTextField">
                        <asp:TextBox ID="txt_chargedamount" runat="server" MaxLength="7" Enabled="false"
                            onkeypress="return isNumberKeydouble(event);">
                            
                        </asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="cellNormal">
                        Consignee Address
                    </td>
                    <td class="cellTextField" colspan="2">
                        <asp:TextBox ID="txt_consigneeAddress" runat="server" Width="100%" TextMode="MultiLine"
                            Rows="3">
                        </asp:TextBox>
                        <%--<asp:TextBox ID="txt_consigneeAddress" runat="server" CssClass="textBox" Width="100%"
                            TextMode="MultiLine" Rows="3"></asp:TextBox>--%>
                    </td>
                    <td class="cellNormal">
                        Consigner Address
                    </td>
                    <td class="cellTextField" colspan="2">
                        <asp:TextBox ID="txt_consignerAddress" runat="server" TextMode="MultiLine" Rows="3"
                            Width="98%">
                        </asp:TextBox>
                        <%--<asp:TextBox ID="txt_consignerAddress" runat="server" CssClass="textBox" Rows="3"
                            TextMode="MultiLine" Width="98%"></asp:TextBox>--%>
                    </td>
                </tr>
                <tr>
                    <td class="cellNormal">
                        Reporting Date:
                    </td>
                    <td class="cellTextField">
                        <asp:TextBox ID="txt_reporting_date" runat="server" CssClass="med-field" MaxLength="10"
                            Enabled="false"></asp:TextBox>
                        <Ajax1:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_reporting_date"
                            runat="server" Format="yyyy-MM-dd" PopupButtonID="Image1">
                        </Ajax1:CalendarExtender>
                    </td>
                    <td class="cellNormal" style="width: 155px">
                        Approve Status:
                    </td>
                    <td class="cellTextField">
                        <asp:TextBox ID="txt_approve_status" runat="server" CssClass="med-field"></asp:TextBox>
                    </td>
                    <td class="cellNormal">
                        Inv Status:
                    </td>
                    <td class="cellTextField">
                        <asp:TextBox ID="txt_inv_status" runat="server" CssClass="med-field"></asp:TextBox>
                    </td>
                    <td class="cellNormal">
                        Invoice No:
                    </td>
                    <td class="cellTextField">
                        <asp:TextBox ID="txt_invoice" runat="server" CssClass="med-field"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <br />
            <fieldset class="fieldsetSmall" style="display:none;">
                <legend><b>Price Modifiers</b></legend>
                <table>
                    <tr>
                        <td class="cellNormal">
                            Name
                        </td>
                        <td class="cellTextField">
                            <asp:DropDownList ID="dd_name" runat="server" CssClass="dropdown" OnSelectedIndexChanged="dd_name_SelectedIndexChanged"
                                AutoPostBack="true" AppendDataBoundItems="true">
                                <asp:ListItem Value="0">Fuel Surcharges</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td class="cellNormal">
                            Calculation Base
                        </td>
                        <td class="cellTextField">
                            <asp:RadioButtonList ID="rbtn_calculationBase" runat="server" RepeatDirection="Horizontal"
                                Enabled="false" RepeatColumns="2">
                                <asp:ListItem Value="1">Flat</asp:ListItem>
                                <asp:ListItem Value="2">Percentage</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td class="cellNormal">
                            Value
                        </td>
                        <td class="cellTextField" colspan="2">
                            <%--<telerik:RadMaskedTextBox ID="txt_value" Mask="#,###,###.##" runat="server" Width="100%"
                                CssClass="textBox" Style="text-align: right;">
                            </telerik:RadMaskedTextBox>--%>
                            <asp:TextBox ID="txt_value" runat="server" MaxLength="7" CssClass="med-field-right"
                                Enabled="false">
                            </asp:TextBox>
                            <%--<asp:TextBox ID="txt_value" runat="server" CssClass="textBox" Width="100%" Style="text-align: right;"></asp:TextBox>--%>
                        </td>
                    </tr>
                    <tr>
                        <td class="cellNormal">
                            Description
                        </td>
                        <td class="cellTextField" colspan="3">
                            <asp:TextBox ID="txt_description" runat="server" TextMode="MultiLine" Rows="3" Width="100%">
                            </asp:TextBox>
                            <%--<asp:TextBox ID="txt_description" runat="server" CssClass="textBox" TextMode="MultiLine"
                                Rows="2" Width="100%"></asp:TextBox>--%>
                        </td>
                        <td class="cellNormal" colspan="2">
                            <asp:Button ID="btn_add" runat="server" Text="Add" CssClass="button" Style="float: right;"
                                UseSubmitBehavior="false" OnClick="btn_add_Click" OnClientClick="DisableApprove();" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <asp:GridView ID="gv_surcharges" runat="server" AutoGenerateColumns="false" BackColor="Transparent"
                                Width="100%" OnRowCommand="gv_surcharges_RowCommand">
                                <HeaderStyle BackColor="#5f5a8d" HorizontalAlign="Left" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Delete" HeaderStyle-Width="5%">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbtn_delete" Text="Delete" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") %>'
                                                CommandName="Del"></asp:LinkButton>
                                            <asp:HiddenField ID="hd_id" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "ID") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Price Modifier Name" DataField="Pname" HeaderStyle-Width="20%" />
                                    <asp:TemplateField HeaderStyle-Width="10%">
                                        <ItemTemplate>
                                            <%--<asp:Label ID="lbl_value" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "PValue") %>'></asp:Label>--%>
                                            <%--<asp:TextBox ID="txt_value" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "PValue") %>'></asp:TextBox>--%>
                                            <asp:TextBox ID="txt_value" runat="server" MaxLength="7" onchange="DisableApprove();"
                                                Enabled="true" Text='<%# DataBinder.Eval(Container.DataItem, "PValue") %>'>
                                                
                                            </asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="5%">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbtn_edit" Text="Edit" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") %>'
                                                CommandName="ed"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:BoundField HeaderText="Value" DataField="PValue" HeaderStyle-Width="10%" />--%>
                                    <asp:BoundField HeaderText="Calculation Base" DataField="Base" HeaderStyle-Width="15%" />
                                    <asp:BoundField HeaderText="Description" DataField="Description" />
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </fieldset>
            <div style="float: right; margin: 10px 0px 0px;">
                <asp:Button ID="btn_reset" runat="server" CssClass="button" Text="RESET" UseSubmitBehavior="false"
                    OnClick="btn_reset_Click" />
                &nbsp; &nbsp;
                <asp:Button ID="btn_validate" runat="server" CssClass="button" Text="Validate" OnClick="btn_validate_Click" />
                &nbsp; &nbsp;
                <asp:Button ID="btn_save" runat="server" CssClass="button" Text="APPROVE" CommandName="Save"
                    UseSubmitBehavior="false" OnClick="btn_save_Click" />
                &nbsp; &nbsp;
                <asp:Button ID="btn_print" runat="server" CssClass="button" Text="PRINT RECEIPT"
                    UseSubmitBehavior="false" OnClientClick="PrintReceipt();" />
                <asp:HiddenField ID="hd_totalGst" runat="server" />
                <asp:HiddenField ID="hd_totalAmount" runat="server" />
                <asp:HiddenField ID="hd_grandTotalAmount" runat="server" />
            </div>
        </fieldset>
    </div>
    </table> </table> </table> </table> </table>
</asp:Content>
