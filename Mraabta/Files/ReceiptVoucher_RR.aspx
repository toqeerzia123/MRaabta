<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="ReceiptVoucher_RR.aspx.cs" Inherits="MRaabta.Files.ReceiptVoucher_RR" EnableEventValidation="false" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="Ajax1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script>
        function isNumberKey(evt) {
            debugger;
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                if (charCode == 0) {
                    return true;
                }
                return false;
            }
            return true;
        }

        function checkAmountsOnSave() {
            debugger;
            var btnDiv = document.getElementById('<%= btnDiv.ClientID %>');
            btnDiv.style.display = 'none';
            var loaderdiv = document.getElementById('<%= loader.ClientID %>');
            var paymenttype = document.getElementById('<%= dd_type.ClientID %>');
            loaderdiv.style.display = 'block';
            var grid = document.getElementById('<%= gv_productWiseAmount.ClientID %>');
            var voucherAmount = document.getElementById('<%= txt_amount.ClientID %>').value;
            var vAmount = 0;
            var pAmount = 0;
            var tpAmount = 0;
            if (voucherAmount.trim() != "") {
                vAmount = parseFloat(voucherAmount);
            }
            for (var i = 1; i < grid.rows.length; i++) {
                var row = grid.rows[i];
                var rowText = row.cells[1].childNodes[1].value;

                if (rowText.trim() != "") {
                    pAmount = parseFloat(rowText);
                    tpAmount += pAmount;

                }
            }
            var rbtn_paymentMode = document.getElementById('<%= rbtn_paymentMode.ClientID %>');
            var selectedValue = "";
            for (var i = 0; i < rbtn_paymentMode.rows[0].cells.length; i++) {
                if (rbtn_paymentMode.rows[0].cells[i].children[0].checked) {
                    selectedValue = rbtn_paymentMode.rows[0].cells[i].children[0].defaultValue;
                }
            }
            if (tpAmount != vAmount && paymenttype.options[paymenttype.selectedIndex].value == "4") {

                alert('Product Amounts and voucher Amount Do Not Match. Please ReCalculate');
                loaderdiv.style.display = 'none';
                btnDiv.style.display = 'block';
                return false;
            }

            return true;

        }
        function checkAmounts() {

            var grid = document.getElementById('<%= gv_productWiseAmount.ClientID %>');
            var voucherAmount = document.getElementById('<%= txt_amount.ClientID %>').value;
            var vAmount = 0;
            var pAmount = 0;
            var tpAmount = 0;
            if (voucherAmount.trim() != "") {
                vAmount = parseFloat(voucherAmount);
            }
            for (var i = 1; i < grid.rows.length; i++) {
                var row = grid.rows[i];
                var rowText = row.cells[1].childNodes[1].value;

                if (rowText.trim() != "") {
                    pAmount = parseFloat(rowText);
                    tpAmount += pAmount;
                    if (tpAmount > vAmount) {
                        row.cells[1].childNodes[1].value = "";
                        alert('Amounts Do Not Match. Please ReCalculate');
                        return false;
                    }
                }


            }
            return true;
        }

        function isNumberKey(evt) {
            debugger;
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                if (charCode == 46 || charCode == 45) {

                }
                else {
                    return false;
                }

            }
            return true;
        }

        function PaymentSourceChange(dropdown) {

            var selectedValue = dropdown.options[dropdown.selectedIndex].value;
            if (selectedValue == "1" || selectedValue == "0") {
                document.getElementById('<%= dd_banks.ClientID %>').disabled = true;
                document.getElementById('<%= picker_chequeDate.ClientID %>').disabled = true;
                document.getElementById('<%= txt_chequeNo.ClientID %>').disabled = true;
                document.getElementById('<%= txt_dslipNo.ClientID %>').disabled = true;
                document.getElementById('<%= dd_depositSlipBank.ClientID %>').disabled = true;
            }
            else if (selectedValue == "2") {
                document.getElementById('<%= dd_banks.ClientID %>').disabled = false;
                document.getElementById('<%= picker_chequeDate.ClientID %>').disabled = false;
                document.getElementById('<%= txt_chequeNo.ClientID %>').disabled = false;
                document.getElementById('<%= txt_dslipNo.ClientID %>').disabled = false;
                document.getElementById('<%= dd_depositSlipBank.ClientID %>').disabled = false;
            }
            else {
                document.getElementById('<%= dd_banks.ClientID %>').disabled = false;
                document.getElementById('<%= picker_chequeDate.ClientID %>').disabled = false;
                document.getElementById('<%= txt_chequeNo.ClientID %>').disabled = false;
                document.getElementById('<%= txt_dslipNo.ClientID %>').disabled = true;
                document.getElementById('<%= dd_depositSlipBank.ClientID %>').disabled = true;
            }
        }

        function TypeChange(paymentType) {

            if (paymentType.options[paymentType.selectedIndex].value == "1") {
                document.getElementById('<%= cpr.ClientID %>').style.display = 'block';
                document.getElementById('<%= stw.ClientID %>').style.display = 'none';
            }
            else if (paymentType.options[paymentType.selectedIndex].value == "10") {
                document.getElementById('<%= cpr.ClientID %>').style.display = 'none';
                document.getElementById('<%= stw.ClientID %>').style.display = 'block';
            }
            else {
                document.getElementById('<%= cpr.ClientID %>').style.display = 'none';
                document.getElementById('<%= stw.ClientID %>').style.display = 'none';
            }


            if (paymentType.options[paymentType.selectedIndex].value == "4") {
                document.getElementById('<%= dd_EC.ClientID %>').disabled = false;
            }
            else {
                var radioButtons = document.getElementById('<%= rbtn_paymentMode.ClientID %>');
                var selectedValue = "";
                for (var i = 0; i < radioButtons.rows[0].cells.length; i++) {
                    if (radioButtons.rows[0].cells[i].children[0].checked) {
                        selectedValue = radioButtons.rows[0].cells[i].children[0].defaultValue;
                    }
                }
                if (selectedValue == "1") {
                    document.getElementById('<%= dd_EC.ClientID %>').disabled = false;
                }
                else {
                    document.getElementById('<%= dd_EC.ClientID %>').disabled = true;
                }

            }
        }

        function CPRinput(cpr) {
            debugger;

            var cprLength = cpr.value.length;
            if (cprLength < 4) {
                cpr.value = "IT-";
            }

            var arr = cpr.value.split("-");
            if (arr[1].toString().length == 0) {
                cpr.value = "IT-";
            }
            else if (arr[1].toString().length > 0 & arr[1].toString().length <= 7) {
                cpr.value = "IT-" + arr[1].toString();
            }
            else if (arr[1].toString().length == 8) {
                cpr.value = "IT-" + arr[1].toString() + "-";
                if (arr[2].toString().length > 0 & arr[2].toString().length < 4) {
                    cpr.value += arr[2].toString();
                }
                else if (arr[2].toString().length == 4) {
                    cpr.value += arr[2].toString() + "-";
                    if (arr[3].toString().length > 0 && arr[3].toString().length < 7) {
                        cpr.value += arr[3].toString();
                    }
                    else {
                        cpr.value += arr[3].toString();
                    }
                }
            }

        }
    </script>
    <style>
        .input-field.rabi > input {
            width: 10%;
        }
    </style>
    <div runat="server" id="loader" style="background-color: honeydew; float: left; height: 100%; opacity: 0.7; position: absolute; text-align: center; display: none; top: 11%; width: 84% !important; padding-top: 300px;">
        <div class="loader">
            <img src="../images/Loading_Movie-02.gif" style="top: 300px !important;" />
        </div>
    </div>
    <noscript style="background-color: honeydew; float: left; height: 100%; opacity: 1; position: absolute; text-align: center; top: 11%; width: 84% !important; padding-top: 300px;">
        <div>
            You must enable javascript to continue.
        </div>
    </noscript>
    <asp:UpdatePanel ID="panel1111" runat="server">
        <ContentTemplate>
            <div style="text-align: center; font-size: medium; font-weight: bold; width: 100%; padding-left: 20px;">
                <asp:Label ID="Errorid" runat="server"></asp:Label>
            </div>
            <table style="font-family: Calibri; font-size: medium; padding-bottom: 0px !important; padding-top: 10px !important; width: 97%"
                class="input-form">
                <tr style="margin: 0px 0px 0px 0px !important;">
                    <td class="field" style="width: 15% !important;">Payment Mode
                    </td>
                    <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;"></td>
                    <td class="field" style="width: 15% !important;">Form Mode
                    </td>
                    <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;"></td>
                </tr>
                <tr>
                    <td class="input-field" style="width: 15% !important;">
                        <asp:RadioButtonList ID="rbtn_paymentMode" runat="server" RepeatDirection="Horizontal"
                            AutoPostBack="true" RepeatColumns="2" OnSelectedIndexChanged="rbtn_paymentMode_SelectedIndexChanged">
                            <asp:ListItem Selected="True" Value="1">Cash</asp:ListItem>
                            <asp:ListItem Value="2">Credit</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                    <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;"></td>
                    <td class="input-field" style="width: 15% !important;">
                        <asp:RadioButtonList ID="rbtn_formMode" runat="server" RepeatDirection="Horizontal"
                            AutoPostBack="true" RepeatColumns="2" OnSelectedIndexChanged="rbtn_formMode_SelectedIndexChanged">
                            <asp:ListItem Selected="True" Value="1">New</asp:ListItem>
                            <asp:ListItem Value="2">View</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                    <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;"></td>
                    <td class="field" style="width: 7% !important;">Voucher#
                    </td>
                    <td class="input-field" style="width: 15% !important;">
                        <asp:TextBox ID="txt_voucherNo" runat="server" Enabled="false" OnTextChanged="txt_voucherNo_TextChanged"
                            AutoPostBack="true"> </asp:TextBox>
                    </td>
                    <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;"></td>
                    <td class="field" style="width: 5% !important;">Date
                    </td>
                    <td class="input-field" style="width: 15% !important;">
                        <%--<telerik:raddatepicker id="picker_voucherDate" runat="server" dateinput-dateformat="dd/MM/yyyy">
                        </telerik:raddatepicker>--%>
                        <asp:TextBox ID="picker_voucherDate" runat="server"></asp:TextBox>
                        <Ajax1:CalendarExtender ID="calendar1" runat="server" TargetControlID="picker_voucherDate"
                            Format="yyyy-MM-dd"></Ajax1:CalendarExtender>
                    </td>
                    <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;"></td>
                </tr>
            </table>
            <asp:Panel ID="panel1" runat="server">
                <table style="font-family: Calibri; font-size: medium; padding-bottom: 0px !important; padding-top: 0px !important; width: 97%"
                    class="input-form">
                    <tr style="float: none !important;">
                        <td style="float: none !important; font-variant: small-caps !important; width: 200px; padding-bottom: 5px !important; font-size: large; text-align: center;">
                            <b>Information</b>
                        </td>
                    </tr>
                    <tr>
                        <td class="field" style="width: 10% !important; text-align: right !important; padding-right: 5px!important;">Receipt #
                        </td>
                        <td class="input-field" style="width: 15% !important;">
                            <asp:TextBox ID="txt_receiptNo" runat="server" Enabled="false"></asp:TextBox>
                        </td>
                        <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;"></td>
                        <td class="field" style="width: 10% !important; text-align: right !important; padding-right: 5px!important;">Type
                        </td>
                        <td class="input-field" style="width: 15% !important;">
                            <asp:DropDownList ID="dd_type" runat="server" AppendDataBoundItems="true" Width="100%"
                                onchange="TypeChange(this)">
                                <asp:ListItem Value="0">Select Type</asp:ListItem>
                            </asp:DropDownList>
                            <%--<asp:RadioButtonList ID="rbtn_type" runat="server">
                </asp:RadioButtonList>--%>
                        </td>
                        <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;"></td>
                        <td class="field" style="width: 10% !important; text-align: right !important; padding-right: 5px!important;">Ref #
                        </td>
                        <td class="input-field" style="width: 15% !important;">
                            <asp:TextBox ID="txt_refNo" runat="server"></asp:TextBox>
                        </td>
                        <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;"></td>
                    </tr>
                    <tr id="stw" runat="server" style="display: none;">
                        <td class="field" style="width: 10% !important; text-align: right !important;"></td>
                        <td class="input-field rabi" style="width: 15% !important; float: left;"></td>
                        <td class="space" style="width: 12% !important; margin: 0px 0px 0px 0px !important; text-align: right !important;"></td>
                        <td class="input-field rabi" style="width: 15% !important;"></td>
                        <td class="space" style="width: 3% !important; margin: 0px 0px 0px 0px !important;"></td>
                        <td class="field" style="width: 10% !important; text-align: right !important; padding-right: 5px!important;">STW Number
                        </td>
                        <td class="input-field" style="width: 15% !important;">
                            <asp:TextBox ID="txt_stwNo" runat="server" onkeypress="return isNumberKey(event);"></asp:TextBox>
                        </td>
                        <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;"></td>
                    </tr>
                    <tr id="cpr" runat="server" style="display: none;">
                        <td class="field" style="width: 10% !important; text-align: right !important;"></td>
                        <td class="input-field rabi" style="width: 15% !important; float: left;"></td>
                        <td class="space" style="width: 12% !important; margin: 0px 0px 0px 0px !important; text-align: right !important;"></td>
                        <td class="input-field rabi" style="width: 15% !important;"></td>
                        <td class="space" style="width: 3% !important; margin: 0px 0px 0px 0px !important;"></td>
                        <td class="field" style="width: 10% !important; text-align: right !important; padding-right: 5px!important;">CPR Number
                        </td>
                        <td class="input-field" style="width: 15% !important;">
                            <asp:TextBox ID="txt_cprNo" runat="server" MaxLength="25" onkeypress="return isNumberKey(event);"></asp:TextBox>
                            <%--<telerik:RadMaskedTextBox RenderMode="Lightweight" ID="txt_cprNo" runat="server"
                                SelectionOnFocus="SelectAll" LabelWidth="150px" PromptChar="_"
                                Width="100%" Mask="IT-<0..99999999>-<0..9999>-<0..9999999>">
                            </telerik:RadMaskedTextBox>--%><br />
                        </td>
                        <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;"></td>
                    </tr>
                    <tr>
                        <td class="field" style="width: 10% !important; text-align: right !important;"></td>
                        <td class="input-field rabi" style="width: 15% !important; float: left;">
                            <asp:RadioButton ID="rbnt_byExpressCenter" Text="By ExpressCenter" runat="server"
                                AutoPostBack="true" GroupName="Group_By" Checked="true" OnCheckedChanged="rbnt_byEC_CheckedChanged" />
                        </td>
                        <td class="space" style="width: 12% !important; margin: 0px 0px 0px 0px !important; text-align: right !important;">
                            <asp:RadioButton ID="rbtn_byRider" Text="By Rider" runat="server" AutoPostBack="true"
                                GroupName="Group_By" OnCheckedChanged="rbnt_byEC_CheckedChanged" />
                        </td>
                        <td class="input-field rabi" style="width: 15% !important;">
                            <asp:RadioButton ID="rbtn_cod" Text="Cash on Delivery" runat="server" AutoPostBack="true"
                                GroupName="Group_By" OnCheckedChanged="rbnt_byEC_CheckedChanged" />
                        </td>
                        <td class="space" style="width: 3% !important; margin: 0px 0px 0px 0px !important;"></td>
                        <td class="field" style="width: 10% !important; text-align: right !important; padding-right: 5px!important;">Exp Center
                        </td>
                        <td class="input-field" style="width: 15% !important;">
                            <asp:DropDownList ID="dd_EC" runat="server" Width="100%" AppendDataBoundItems="true"
                                OnSelectedIndexChanged="dd_EC_SelectedIndexChanged" AutoPostBack="true">
                                <asp:ListItem Value="0">Select Express Center</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;"></td>
                    </tr>
                    <tr>
                        <td class="field" style="width: 10% !important; text-align: right !important; padding-right: 5px!important;">Rider
                        </td>
                        <td class="input-field" style="width: 15% !important;">
                            <asp:DropDownList ID="dd_rider" runat="server" AppendDataBoundItems="true" Width="100%">
                                <asp:ListItem Value="0">Select Rider</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;"></td>
                        <td class="field" style="width: 10% !important; text-align: right !important; padding-right: 5px!important;">CN#
                        </td>
                        <td class="input-field" style="width: 15% !important;">
                            <asp:TextBox ID="txt_cnNo" runat="server" AutoPostBack="true" OnTextChanged="txt_cnNo_TextChanged"></asp:TextBox>
                        </td>
                        <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;"></td>
                        <td class="field" style="width: 10% !important; text-align: right !important; padding-right: 5px!important;">Sale Amount
                        </td>
                        <td class="input-field" style="width: 15% !important;">
                            <asp:TextBox ID="txt_saleAmt" runat="server"></asp:TextBox>
                        </td>
                        <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;"></td>
                    </tr>
                    <tr>
                        <td class="field" style="width: 10% !important; text-align: right !important; padding-right: 5px!important;">Pay. Source
                        </td>
                        <td class="input-field" style="width: 15% !important;">
                            <asp:DropDownList ID="dd_paySource" runat="server" AppendDataBoundItems="true" onchange="PaymentSourceChange(this)">
                                <asp:ListItem Value="0">Select Pay Source</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;"></td>
                        <td class="field" style="width: 10% !important; text-align: right !important; padding-right: 5px!important;">Client A/C #
                        </td>
                        <td class="input-field" style="width: 15% !important;">
                            <asp:TextBox ID="txt_clientAcNo" runat="server" OnTextChanged="txt_clientAcNo_TextChanged"
                                AutoPostBack="true"></asp:TextBox>
                            <asp:HiddenField ID="creditClientID" runat="server" />
                        </td>
                        <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;"></td>
                        <td class="field" style="width: 10% !important; text-align: right !important; padding-right: 5px!important;">Client Name
                        </td>
                        <td class="input-field" style="width: 15% !important;">
                            <asp:TextBox ID="txt_clientName" runat="server"></asp:TextBox>
                        </td>
                        <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;"></td>
                    </tr>
                    <tr>
                        <td class="field" style="width: 10% !important; text-align: right !important; padding-right: 5px!important;">Client Bank
                        </td>
                        <td class="input-field" style="width: 15% !important;">
                            <asp:DropDownList ID="dd_banks" runat="server" AppendDataBoundItems="true" Width="100%">
                                <asp:ListItem Value="0">Select Bank</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;"></td>
                        <td class="field" style="width: 10% !important; text-align: right !important; padding-right: 5px!important;">Cheque Date
                        </td>
                        <td class="input-field" style="width: 15% !important;">
                            <%--  <telerik:raddatepicker id="picker_chequeDate" runat="server" dateinput-dateformat="dd/MM/yyyy">
                            </telerik:raddatepicker>--%>
                            <asp:TextBox ID="picker_chequeDate" runat="server"></asp:TextBox>
                            <Ajax1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="picker_chequeDate"
                                Format="yyyy-MM-dd"></Ajax1:CalendarExtender>
                        </td>
                        <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;"></td>
                        <td class="field" style="width: 10% !important; text-align: right !important; padding-right: 5px!important;">Cheque #
                        </td>
                        <td class="input-field" style="width: 15% !important;">
                            <asp:TextBox ID="txt_chequeNo" runat="server" MaxLength="15"></asp:TextBox>
                        </td>
                        <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;"></td>
                    </tr>
                    <tr>
                        <td class="field" style="width: 10% !important; text-align: right !important; padding-right: 5px!important;">DSlip Bank
                        </td>
                        <td class="input-field" style="width: 15% !important;">
                            <asp:DropDownList ID="dd_depositSlipBank" runat="server" AppendDataBoundItems="true"
                                Width="100%" Enabled="false">
                                <asp:ListItem Value="0">Select Bank</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;"></td>
                        <td class="field" style="width: 10% !important; text-align: right !important; padding-right: 5px!important;">DSlip No
                        </td>
                        <td class="input-field" style="width: 15% !important;">
                            <asp:TextBox ID="txt_dslipNo" runat="server" Enabled="false"></asp:TextBox>
                        </td>
                        <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;"></td>
                    </tr>
                    <tr>
                        <td class="field" style="width: 10% !important; text-align: right !important; padding-right: 5px!important;">Amount
                        </td>
                        <td class="input-field" style="width: 15% !important;">
                            <asp:TextBox ID="txt_amount" runat="server"></asp:TextBox>
                        </td>
                        <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;"></td>
                        <td class="field" style="width: 10% !important; text-align: right !important; padding-right: 5px!important;"></td>
                        <td class="input-field rabi" style="width: 15% !important;">
                            <asp:CheckBox ID="chk_centralized" runat="server" Text="Centralized Client" OnCheckedChanged="chk_centralized_CheckedChanged"
                                AutoPostBack="true" />
                        </td>
                        <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;"></td>
                        <td class="field" style="width: 10% !important; text-align: right !important; padding-right: 5px!important;">Group
                        </td>
                        <td class="input-field" style="width: 15% !important;">
                            <%--<Ajax1:ComboBox ID="dd_group" runat="server" AutoCompleteMode="SuggestAppend" AppendDataBoundItems="true"
                                Visible="false">
                            </Ajax1:ComboBox>--%>
                            <asp:TextBox ID="txt_clientGroup" runat="server"></asp:TextBox>
                            <a href="SearchRRClientGroups.aspx" target="_blank">Search Group</a>
                            <%--<asp:DropDownList ID="dd_group" runat="server" AppendDataBoundItems="true" Width="100%">
                                <asp:ListItem Value="0">Select Group</asp:ListItem>
                            </asp:DropDownList>--%>
                        </td>
                        <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;"></td>
                    </tr>
                </table>
            </asp:Panel>
            <div class="tbl-large">
                <asp:GridView ID="gv_productWiseAmount" runat="server" AutoGenerateColumns="false"
                    CssClass="mGrid">
                    <Columns>
                        <asp:BoundField HeaderText="Product" DataField="ProductDisplay" />
                        <asp:TemplateField HeaderText="Amount">
                            <ItemTemplate>
                                <asp:TextBox ID="txt_amount" runat="server" onchange="checkAmounts();" onkeypress="return isNumberKey(event);"></asp:TextBox>
                                <asp:HiddenField ID="hd_product" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "Products") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <div style="width: 100%; text-align: center" id="btnDiv" runat="server">
                <asp:Button ID="btn_reset" runat="server" Text="Reset" CssClass="button" OnClick="btn_reset_Click" />
                &nbsp;
                <asp:Button ID="btn_save" runat="server" Text="Save" CssClass="button" CommandName="first"
                    OnClientClick="if(!checkAmountsOnSave()) { return false;}" OnClick="btn_save_Click" />
                &nbsp;
                <asp:Button ID="bt_ReportView" runat="server" Text="ReportView" CssClass="button"
                    OnClick="bt_ReportView_Click" Visible="true" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
