<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.master" CodeBehind="InternationalConsignment.aspx.cs" Inherits="MRaabta.Files.InternationalConsignment" %>

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
            debugger;

            var count = 1;
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode != 46 && charCode > 31
                && (charCode < 48 || charCode > 57 || charCode == 110 || charCode == 109 || charCode == 45 || charCode == 9)) {
                return false;
            }
            else {

                if (charCode == 110 || charCode == 46) {
                    count++;
                }
                if (count > 1) {
                    return false
                }
            }

            return true;
        }
        function isNumberKeydouble(evt) {
            debugger;
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
        function checkValidations(txt) {
            debugger;


            if (txt.value.length > 15 || txt.value.length < 11) {
                alert('Consignment Number must be between 11 and 15 digits');
                txt.value = "";
                txt.focus();
                return false;
            }


            return true;
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
                document.getElementById('<%= txt_TransactionID.ClientID %>').value = "0";
                document.getElementById('<%= txt_TransactionID.ClientID %>').disabled = true;
            }
        }

        function PaymentModeChange_() {
            var saveButton = document.getElementById('<%= dd_PaymentMode.ClientID %>');
            val_ = saveButton.options[saveButton.selectedIndex].value
            if (val_ == "6") {
                document.getElementById('<%= txt_TransactionID.ClientID %>').maxLength = "6";
                document.getElementById('<%= txt_TransactionID.ClientID %>').disabled = false;

            }
            else if (val_ == "7") {
                document.getElementById('<%= txt_TransactionID.ClientID %>').maxLength = "10";
                document.getElementById('<%= txt_TransactionID.ClientID %>').disabled = false;

            }
            else {
                document.getElementById('<%= txt_TransactionID.ClientID %>').value = "0";
                document.getElementById('<%= txt_TransactionID.ClientID %>').disabled = true;
            }


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
                            RepeatDirection="Horizontal" RepeatColumns="2" OnSelectedIndexChanged="rbtn_consignmentSender_SelectedIndexChanged">
                        </asp:RadioButtonList>
                    </td>
                    <td class="cellTextField">
                        <asp:CheckBox ID="chk_riderCode" runat="server" Text="Save Rider Code" OnCheckedChanged="chk_riderCode_CheckedChanged"
                            AutoPostBack="true" />
                    </td>
                </tr>
                <tr>
                    <td class="cellNormal">
                        Consignment No.
                    </td>
                    <td class="cellTextField">
                        <%--<telerik:radmaskedtextbox id="txt_consignmentNo" runat="server" skin="Web20" maxlength="11"
                            autopostback="True" mask="###########" labelwidth="64px" resize="None" resolvedrendermode="Classic"
                            onkeydown="return (event.keyCode!=13);" width="160px" ontextchanged="txt_consignmentNo_TextChanged">
                        </telerik:radmaskedtextbox>--%>
                        <telerik:RadTextBox ID="txt_consignmentNo" runat="server" Skin="Web20" MaxLength="15"
                            AutoPostBack="True" onkeypress="return isNumberKey(event);" LabelWidth="64px"
                            Resize="None" resolvedrendermode="Classic" Width="160px" OnTextChanged="txt_consignmentNo_TextChanged"
                            onchange="if ( checkValidations(this) == false ) return;">
                        </telerik:RadTextBox>
                        <%--<asp:TextBox ID="txt_consignmentNo" CssClass="textBox" runat="server" OnTextChanged="txt_consignmentNo_TextChanged"
                            AutoPostBack="true" onkeydown="return (event.keyCode!=13);" MaxLength="11"></asp:TextBox>--%>
                    </td>
                    <td class="cellNormal">
                        Account No.
                    </td>
                    <td class="cellTextField">
                        <telerik:RadTextBox ID="txt_accountNo" runat="server" Skin="Web20" MaxLength="12"
                            AutoPostBack="True" LabelWidth="64px" Resize="None" resolvedrendermode="Classic"
                            onkeydown="return (event.keyCode!=13);" Width="160px" OnTextChanged="txt_accountNo_TextChanged">
                        </telerik:RadTextBox>
                        <asp:HiddenField ID="hd_customerType" runat="server" />
                        <%--<asp:TextBox ID="txt_accountNo" runat="server" CssClass="textBox" OnTextChanged="txt_accountNo_TextChanged"
                            AutoPostBack="true"></asp:TextBox>--%>
                    </td>
                    <td class="cellNormal">
                        Consigner
                    </td>
                    <td class="cellTextField">
                        <telerik:RadTextBox ID="txt_consigner" runat="server" Skin="Web20" LabelWidth="64px"
                            Resize="None" resolvedrendermode="Classic" onkeydown="return (event.keyCode!=13);"
                            Width="160px">
                        </telerik:RadTextBox>
                        <%--<asp:TextBox ID="txt_consigner" runat="server" CssClass="textBox"></asp:TextBox>--%>
                        <asp:HiddenField ID="hd_creditClientID" runat="server" />
                    </td>
                    <td class="cellNormal">
                        <asp:Label ID="lbl_cupon" runat="server" Text="Cupon No."></asp:Label>
                    </td>
                    <td class="cellTextField">
                        <telerik:RadMaskedTextBox ID="txt_cupon" runat="server" Skin="Web20" MaxLength="40"
                            LabelWidth="64px" resize="None" resolvedrendermode="Classic" onkeydown="return (event.keyCode!=13);"
                            Mask="#####" Width="160px">
                        </telerik:RadMaskedTextBox>
                        <%--<asp:TextBox ID="txt_cupon" runat="server" CssClass="textBox" Text="0" Enabled="false"></asp:TextBox>--%>
                    </td>
                </tr>
                <tr>
                    <td class="cellNormal">
                        Consigner Cell No.
                    </td>
                    <td class="cellTextField">
                        <telerik:RadMaskedTextBox ID="txt_consignerCellNo" runat="server" Skin="Web20" MaxLength="12"
                            Mask="####-###-####" LabelWidth="64px" resize="None" resolvedrendermode="Classic"
                            onkeydown="return (event.keyCode!=13);" Width="160px">
                        </telerik:RadMaskedTextBox>
                        <%--<asp:TextBox ID="txt_consignerCellNo" runat="server" CssClass="textBox"></asp:TextBox>--%>
                    </td>
                    <td class="cellNormal">
                        Consigner CNIC
                    </td>
                    <td class="cellTextField">
                        <telerik:RadMaskedTextBox ID="txt_consignerCNIC" runat="server" Skin="Web20" MaxLength="13"
                            Mask="#####-#######-#" LabelWidth="64px" resize="None" resolvedrendermode="Classic"
                            onkeydown="return (event.keyCode!=13);" Width="160px">
                        </telerik:RadMaskedTextBox>
                        <%-- <asp:TextBox ID="txt_consignerCNIC" runat="server" CssClass="textBox"></asp:TextBox>--%>
                    </td>
                    <td class="cellNormal">
                        Consignee
                    </td>
                    <td class="cellTextField">
                        <telerik:RadTextBox ID="txt_consignee" runat="server" Skin="Web20" MaxLength="40"
                            LabelWidth="64px" Resize="None" resolvedrendermode="Classic" onkeydown="return (event.keyCode!=13);"
                            Width="160px">
                        </telerik:RadTextBox>
                        <%--<asp:TextBox ID="txt_consignee" runat="server" CssClass="textBox"></asp:TextBox>--%>
                    </td>
                    <td class="cellNormal">
                        Cell No.
                    </td>
                    <td class="cellTextField">
                        <telerik:RadMaskedTextBox ID="txt_cellNo" runat="server" Skin="Web20" MaxLength="12"
                            LabelWidth="64px" resize="None" resolvedrendermode="Classic" onkeydown="return (event.keyCode!=13);"
                            Mask="####-###-####" Width="160px">
                        </telerik:RadMaskedTextBox>
                        <%--<asp:TextBox ID="txt_cellNo" runat="server" CssClass="textBox"></asp:TextBox>--%>
                    </td>
                </tr>
                <tr>
                    <td class="cellNormal">
                        Consignee Attn.
                    </td>
                    <td class="cellTextField">
                        <telerik:RadTextBox ID="txt_consigneeAttn" runat="server" Skin="Web20" MaxLength="12"
                            LabelWidth="64px" Resize="None" resolvedrendermode="Classic" onkeydown="return (event.keyCode!=13);"
                            Width="160px">
                        </telerik:RadTextBox>
                        <%--<asp:TextBox ID="txt_consigneeAttn" runat="server" CssClass="textBox"></asp:TextBox>--%>
                    </td>
                    <td class="cellNormal">
                        Con. Type
                    </td>
                    <td class="cellTextField">
                        <asp:DropDownList ID="dd_conType" runat="server" CssClass="dropdown" OnSelectedIndexChanged="dd_conType_SelectedIndexChanged"
                            AutoPostBack="true">
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
                        Destination
                    </td>
                    <td class="cellTextField">
                        <asp:DropDownList ID="dd_destination" runat="server" CssClass="dropdown" OnSelectedIndexChanged="dd_destination_SelectedIndexChanged"
                            AppendDataBoundItems="true" AutoPostBack="true">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="cellNormal">
                        Dest. Country
                    </td>
                    <td class="cellTextField">
                        <asp:DropDownList ID="dd_destCountry" runat="server" CssClass="dropdown" AutoPostBack="true"
                            AppendDataBoundItems="true" OnSelectedIndexChanged="dd_destCountry_SelectedIndexChanged">
                            <asp:ListItem Value="0">Select Dest Country</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td class="cellNormal">
                        Service Type
                    </td>
                    <td class="cellTextField">
                        <asp:DropDownList ID="dd_serviceType" runat="server" CssClass="dropdown" OnSelectedIndexChanged="dd_serviceType_SelectedIndexChanged"
                            AutoPostBack="true" AppendDataBoundItems="true">
                            <asp:ListItem Value="0">Select Service Type</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td class="cellNormal">
                        W
                    </td>
                    <td class="cellTextField">
                        <telerik:RadTextBox ID="txt_w" runat="server" Skin="Web20" MaxLength="12" Width="30px"
                            Resize="None" Enabled="false" resolvedrendermode="Classic" onkeydown="return (event.keyCode!=13);">
                        </telerik:RadTextBox>
                        <%--<asp:TextBox ID="txt_w" runat="server" CssClass="textBox" Width="30px" Style="text-align: right;
                            font-size: small; font-family: Calibri;" Text="0" Enabled="false"></asp:TextBox>--%>
                        &nbsp; H
                        <telerik:RadTextBox ID="txt_h" runat="server" Skin="Web20" MaxLength="12" Width="30px"
                            Resize="None" Enabled="false" resolvedrendermode="Classic" onkeydown="return (event.keyCode!=13);">
                        </telerik:RadTextBox>
                        <%-- <asp:TextBox ID="txt_h" runat="server" CssClass="textBox" Width="30px" Style="text-align: right;
                            font-size: small; font-family: Calibri;" Text="0" Enabled="false"></asp:TextBox>--%>
                        &nbsp; B
                        <telerik:RadTextBox ID="txt_b" runat="server" Skin="Web20" MaxLength="12" Width="30px"
                            Resize="None" Enabled="false" resolvedrendermode="Classic" onkeydown="return (event.keyCode!=13);">
                        </telerik:RadTextBox>
                        <%--<asp:TextBox ID="txt_b" runat="server" CssClass="textBox" Width="30px" Style="text-align: right;
                            font-size: small; font-family: Calibri;" Text="0" Enabled="false"></asp:TextBox>--%>
                    </td>
                    <td class="cellNormal">
                        Weight
                    </td>
                    <td class="cellTextField">
                        <telerik:RadTextBox ID="txt_weight" runat="server" Skin="Web20" Text="0.5" LabelWidth="58px"
                            Resize="None" resolvedrendermode="Classic" onkeypress="return isNumberKeydouble(event);"
                            MaxLength="4" Width="58px">
                            <EnabledStyle HorizontalAlign="Right" />
                        </telerik:RadTextBox>
                        <%--<asp:TextBox ID="txt_weight" runat="server" CssClass="textBox" Width="60px" Text="0"
                            Style="text-align: right;"></asp:TextBox>--%>
                        &nbsp; Disc.
                        <telerik:RadTextBox ID="txt_discount" runat="server" Skin="Web20" MaxLength="12"
                            Enabled="false" LabelWidth="58px" Resize="None" resolvedrendermode="Classic"
                            onkeydown="return (event.keyCode!=13);" Width="58px">
                        </telerik:RadTextBox>
                        <%-- <asp:TextBox ID="txt_discount" runat="server" CssClass="textBox" Width="58px" Text="0"
                            Style="text-align: right;" Enabled="false"></asp:TextBox>--%>
                    </td>
                </tr>
                <tr>
                    <td class="cellNormal">
                        Rider Code
                    </td>
                    <td class="cellTextField">
                        <telerik:RadTextBox ID="txt_riderCode" runat="server" Skin="Web20" MaxLength="12"
                            AutoPostBack="True" LabelWidth="64px" Resize="None" resolvedrendermode="Classic"
                            onkeydown="return (event.keyCode!=13);" Width="160px" OnTextChanged="txt_riderCode_TextChanged">
                        </telerik:RadTextBox>
                        <%--<asp:TextBox ID="txt_riderCode" runat="server" CssClass="textBox" OnTextChanged="txt_riderCode_TextChanged"
                            AutoPostBack="true"></asp:TextBox>--%>
                    </td>
                    <td class="cellNormal">
                        Origin EC
                    </td>
                    <td class="cellTextField">
                        <asp:DropDownList ID="dd_originEC" runat="server" AppendDataBoundItems="true" CssClass="dropdown"
                            Enabled="false">
                            <asp:ListItem Value="0">Select EC</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td class="cellNormal">
                        Pieces
                    </td>
                    <td class="cellTextField">
                        <telerik:RadMaskedTextBox ID="txt_pieces" runat="server" Text="1" Skin="Web20" MaxLength="12"
                            LabelWidth="64px" Mask="###" resize="None" resolvedrendermode="Classic" Width="80px"
                            onkeydown="return (event.keyCode != 109 && event.keyCode != 189 && event.keyCode != 13)">
                            <EnabledStyle HorizontalAlign="Right" />
                        </telerik:RadMaskedTextBox>
                        <%--<asp:TextBox ID="txt_pieces" runat="server" CssClass="textBox" Width="80px" Text="0"
                            Style="text-align: right;"></asp:TextBox>--%>
                        &nbsp; Insurance
                        <asp:CheckBox ID="chk_insurance" runat="server" CssClass="checkBox" Style="float: right"
                            OnCheckedChanged="chk_insurance_CheckedChanged" AutoPostBack="true" />
                    </td>
                    <td class="cellNormal">
                    </td>
                    <td class="cellTextField">
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
                    <td class="cellNormal">
                        Package Content
                    </td>
                    <td class="cellTextField" colspan="2" style="padding-right: 3px;">
                        <telerik:RadTextBox ID="txt_packageContent" runat="server" Skin="Web20" LabelWidth="64px"
                            Resize="None" resolvedrendermode="Classic" Width="98%">
                        </telerik:RadTextBox>
                        <%--<asp:TextBox ID="txt_packageContent" runat="server" CssClass="textBox" Width="98%"></asp:TextBox>--%>
                    </td>
                    <td class="field" style="width: 15% !important; text-align: right !important; padding-right: 5px;" id="div1" runat="server">
                        Payment Mode
                    </td>
                    <td id="div2" runat="server">
                        <asp:DropDownList ID="dd_PaymentMode" runat="server" AppendDataBoundItems="true"
                            onchange="PaymentModeChange();">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="cellNormal" style="width: 155px">
                        Charged Amount
                    </td>
                    <td class="cellTextField" colspan="1">
                        <telerik:RadNumericTextBox ID="txt_chargedamount" runat="server" MaxLength="7" Skin="Web20"
                            CssClass="med-field-right" Enabled="false" onkeypress="return isNumberKeydouble(event);">
                            <EnabledStyle HorizontalAlign="Right" CssClass="RightAligned" />
                        </telerik:RadNumericTextBox>
                    </td>
                    <td class="cellNormal">
                        Total Amount
                    </td>
                    <td class="cellTextField">
                        <asp:TextBox ID="txt_totalAmount" runat="server" Style="text-align: right; width: 60px;
                            margin-right: 25px;" Enabled="false"></asp:TextBox>
                        Gst
                        <asp:TextBox ID="txt_gst" runat="server" Style="text-align: right; width: 60px" Enabled="false"></asp:TextBox>
                    </td>
                    <td>
                    </td>
                    <td class="field" style="width: 15% !important; text-align: right !important; padding-right: 5px;" id="div3" runat="server">
                        Payment Transaction ID
                    </td>
                    <td id="div4" runat="server">
                        <asp:TextBox ID="txt_TransactionID" value="0" Enabled="false" runat="server" Width="100px"
                            onkeypress="return isNumberKey(event);"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="cellNormal">
                        Consignee Address
                    </td>
                    <td class="cellTextField" colspan="2">
                        <telerik:RadTextBox ID="txt_consigneeAddress" runat="server" Skin="Web20" LabelWidth="64px"
                            Resize="None" resolvedrendermode="Classic" Width="100%" TextMode="MultiLine"
                            Rows="3">
                        </telerik:RadTextBox>
                        <%--<asp:TextBox ID="txt_consigneeAddress" runat="server" CssClass="textBox" Width="100%"
                            TextMode="MultiLine" Rows="3"></asp:TextBox>--%>
                    </td>
                    <td class="cellNormal">
                        Consigner Address
                    </td>
                    <td class="cellTextField" colspan="2">
                        <telerik:RadTextBox ID="txt_consignerAddress" runat="server" Skin="Web20" LabelWidth="64px"
                            Resize="None" TextMode="MultiLine" Rows="3" resolvedrendermode="Classic" Width="98%">
                        </telerik:RadTextBox>
                        <%--<asp:TextBox ID="txt_consignerAddress" runat="server" CssClass="textBox" Rows="3"
                            TextMode="MultiLine" Width="98%"></asp:TextBox>--%>
                    </td>
                </tr>
                <tr>
                    <td class="cellNormal">
                        Reporting Date:
                    </td>
                    <td class="cellTextField">
                        <asp:TextBox ID="txt_reporting_date" runat="server" CssClass="med-field" MaxLength="10"></asp:TextBox>
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
            <fieldset class="fieldsetSmall">
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
                                Enabled="false" RepeatColumns="3" Width="220px">
                                <asp:ListItem Value="1">Flat</asp:ListItem>
                                <asp:ListItem Value="2">Percentage</asp:ListItem>
                                <asp:ListItem Value="3">Insurance</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td class="cellNormal" style="width: 60px;">
                            Value
                        </td>
                        <td class="cellTextField" style="width: 65px !important;">
                            <%--<telerik:RadMaskedTextBox ID="txt_value" Mask="#,###,###.##" runat="server" Width="100%"
                                CssClass="textBox" Style="text-align: right;">
                            </telerik:RadMaskedTextBox>--%>
                            <%--<telerik:radnumerictextbox id="txt_value" runat="server" maxlength="7" skin="Web20"
                                cssclass="med-field-right" enabled="false">
                                <EnabledStyle HorizontalAlign="Right" CssClass="RightAligned" />
                            </telerik:radnumerictextbox>--%>
                            <asp:TextBox ID="txt_value" runat="server" Style="text-align: right; width: 60px"
                                Enabled="false"></asp:TextBox>
                            <%--<asp:TextBox ID="txt_value" runat="server" CssClass="textBox" Width="100%" Style="text-align: right;"></asp:TextBox>--%>
                        </td>
                        <td class="cellNormal" style="width: 80px !important;">
                            Declared Value
                        </td>
                        <td class="cellTextField" style="width: 65px !important;">
                            <asp:TextBox ID="txt_declaredValue" runat="server" Style="text-align: right; width: 60px"
                                Enabled="false"></asp:TextBox>
                            <%--<asp:TextBox ID="txt_value" runat="server" CssClass="textBox" Width="100%" Style="text-align: right;"></asp:TextBox>--%>
                        </td>
                    </tr>
                    <tr>
                        <td class="cellNormal">
                            Description
                        </td>
                        <td class="cellTextField" colspan="3">
                            <telerik:RadTextBox ID="txt_description" runat="server" Skin="Web20" LabelWidth="64px"
                                TextMode="MultiLine" Rows="3" Resize="None" resolvedrendermode="Classic" Width="100%">
                            </telerik:RadTextBox>
                            <%--<asp:TextBox ID="txt_description" runat="server" CssClass="textBox" TextMode="MultiLine"
                                Rows="2" Width="100%"></asp:TextBox>--%>
                        </td>
                        <td class="cellNormal" colspan="2">
                            <asp:Button ID="btn_add" runat="server" Text="Add" CssClass="button" Style="float: right;"
                                UseSubmitBehavior="false" OnClick="btn_add_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <asp:GridView ID="gv_surcharges" runat="server" AutoGenerateColumns="false" BackColor="Transparent"
                                Width="100%" OnRowCommand="gv_surcharges_RowCommand" OnRowDataBound="gv_surcharges_RowDataBound">
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
                                            <telerik:RadNumericTextBox ID="txt_value" runat="server" MaxLength="7" Skin="Web20"
                                                CssClass="med-field-right" Enabled="true" DbValue='<%# DataBinder.Eval(Container.DataItem, "PValue") %>'>
                                                <EnabledStyle HorizontalAlign="Right" CssClass="RightAligned" />
                                            </telerik:RadNumericTextBox>
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
                                    <asp:BoundField HeaderText="Delcared Value" DataField="DeclaredValue" />
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
                <asp:Button ID="btn_save" runat="server" CssClass="button" Text="APPROVE" CommandName="Save"
                    UseSubmitBehavior="false" OnClick="btn_save_Click" />
                &nbsp; &nbsp;
                <asp:Button ID="btn_unapprove" runat="server" CssClass="button" Text="UNAPPROVE"
                    UseSubmitBehavior="false" OnClick="btn_unapprove_Click" />
                <asp:HiddenField ID="hd_totalGst" runat="server" />
                <asp:HiddenField ID="hd_totalAmount" runat="server" />
                <asp:HiddenField ID="hd_grandTotalAmount" runat="server" />
            </div>
        </fieldset>
    </div>
    <script language="javascript" type="text/javascript">
     //   PaymentModeChange();
    </script>
</asp:Content>
