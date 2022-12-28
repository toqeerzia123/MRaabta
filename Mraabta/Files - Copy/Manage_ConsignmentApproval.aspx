<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="Manage_ConsignmentApproval.aspx.cs" Inherits="MRaabta.Files.Manage_ConsignmentApproval" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
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

        function isNumber(evt) {

            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            return true;
        }
        function RiderWeightCheck() {
            debugger;
            var weight = document.getElementById('<%= txt_weight.ClientID %>').value;
            var rider = document.getElementById('<%= txt_riderCode.ClientID %>').value;
            var originEC = document.getElementById('<%= dd_originExpressCenter.ClientID %>');
            if (weight == rider) {
                alert('Rider Code and Weight Cannot be equal');
                document.getElementById('<%= txt_weight.ClientID %>').value = "";
                document.getElementById('<%= txt_riderCode.ClientID %>').value = "";
                originEC.options[dropdown.selectedIndex] = 0;
                return false;
            }
            else {
                return true;
            }
        }
        function ChangeMaxLength(dropdown) {
            debugger;
            var serviceType = dropdown.options[dropdown.selectedIndex].value;

            var weight = document.getElementById('<%= txt_weight.ClientID %>');
            weight.value = "";
            if (serviceType == "Aviation Sale") {
                weight.maxLength = 5;
            }
            else {
                weight.maxLength = 4;
            }
        }


        function VolumeChange() {
            debugger;

            var txtlength = document.getElementById('<%= txt_l.ClientID %>');
            var txtwidth = document.getElementById('<%= txt_w.ClientID %>');
            var txtheight = document.getElementById('<%= txt_h.ClientID %>');
            var txtvWeight = document.getElementById('<%= txt_vWeight.ClientID %>');
            var txtaWeight = document.getElementById('<%= txt_aWeight.ClientID %>');
            var txtweight = document.getElementById('<%= txt_weight.ClientID %>');


            var l = parseFloat(txtlength.value);
            if (l.toString() == 'NaN') {
                l = 0;
            }

            var w = parseFloat(txtwidth.value);
            if (w.toString() == 'NaN') {
                w = 0;
            }

            var h = parseFloat(txtheight.value);
            if (h.toString() == 'NaN') {
                h = 0;
            }
            var aWeight = parseFloat(txtaWeight.value);
            if (aWeight.toString() == 'NaN') {
                aWeight = 0;
            }
            var vWeight = (l * w * h) / 5000;
            txtvWeight.value = vWeight.toString();
            if (vWeight > aWeight) {
                txtweight.value = vWeight.toString();
            }
            else {
                txtweight.value = aWeight.toString();
            }



        }
    </script>
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
            width: 314px;
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
    </style>
    <div id="divDialogue" runat="server" class="outer_box" style="display: none;">
        <div class="pop_div">
            <table style="width: 100% !important;">
                <tr width="100%">
                    <td style="float: left; margin-top: 8px; text-align: center !important; width: 49%">
                        <asp:TextBox ID="txt_rangeFrom" runat="server" PlaceHolder="Range From"></asp:TextBox>
                    </td>
                    <td style="float: left; margin-top: 8px; text-align: center !important; width: 49%">
                        <asp:TextBox ID="txt_rangeTo" runat="server" PlaceHolder="RangeTo"></asp:TextBox>
                    </td>
                </tr>
                <tr width="100%">
                    <td style="float: left; margin-top: 8px; text-align: center !important; width: 49%">
                        <asp:Button ID="btn_cancelDialogue" runat="server" Text="Cancel" CssClass="button"
                            OnClick="btn_cancelDialogue_Click" />
                    </td>
                    <td style="float: left; margin-top: 8px; text-align: center !important; width: 49%">
                        <asp:Button ID="btn_okDialogue" runat="server" Text="OK" CssClass="button" OnClick="btn_okDialogue_Click" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <asp:HiddenField ID="hd_CreditClientID" runat="server" />
    <asp:HiddenField ID="hd_customerType" runat="server" />
    <asp:HiddenField ID="hd_COD" runat="server" />
    <asp:HiddenField ID="hd_unApproveable" runat="server" />
    <asp:HiddenField ID="hd_editable" runat="server" />
    <asp:HiddenField ID="hd_portalConsignment" runat="server" />
    <asp:HiddenField ID="hd_BookingDate" runat="server" />
    <asp:HiddenField ID="hd_AccountReceivingDate" runat="server" />
    <asp:Label ID="Errorid" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
    <asp:Literal ID="lt1" runat="server"></asp:Literal>
    <table style="font-family: Calibri; font-size: medium; padding-bottom: 0px !important; padding-top: 0px !important; width: 97%"
        class="input-form">
        <tr style="float: none !important;">
            <td colspan="9" style="float: none !important; font-variant: small-caps !important; font-size: large; text-align: center;">
                <b>Consignment Info.</b>
            </td>
        </tr>
        <tr style="margin: 20px 0px 0px !important; padding: 10px 0px 0px 0px !important;">
            <td class="field" style="width: 15% !important; text-align: right !important; padding-right: 5px;">Update Type
            </td>
            <td class="input-field" style="width: 15% !important; float: left;">
                <asp:CheckBox ID="chk_bulkUpdate" runat="server" Text="Bulk Update" AutoPostBack="true"
                    OnCheckedChanged="chk_bulkUpdate_CheckedChanged" Width="80%" />
            </td>
            <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;"></td>
            <td class="field" style="width: 15% !important; text-align: right !important; padding-right: 5px;">Booking Date
            </td>
            <td class="input-field" style="width: 15% !important;">
                <telerik:RadDatePicker ID="picker_bookingDate" runat="server" AutoPostBack="true"
                    Width="60%" DateInput-DateFormat="dd/MM/yyyy" OnSelectedDateChanged="picker_bookingDate_SelectedDateChanged">
                </telerik:RadDatePicker>
                <asp:CheckBox ID="chk_BookingDateFreeze" runat="server" Text="Save" AutoPostBack="true"
                    OnCheckedChanged="chk_BookingDateFreeze_CheckedChanged" TextAlign="Right" Width="35%" />
            </td>
            <td class="space" style="width: 1% !important; margin: 0px 0px 0px 0px !important;"></td>
            <td class="field" style="width: 10% !important; text-align: right !important; padding-right: 5px;"></td>
            <td class="input-field" style="width: 12% !important;"></td>
            <td class="space" style="width: 15% !important; margin: 0px 0px 0px 0px !important;"></td>
        </tr>
        <tr style="margin: 20px 0px 0px !important; padding: 10px 0px 0px 0px !important;">
            <td class="field" style="width: 15% !important; text-align: right !important; padding-right: 5px;">Consignment Number
            </td>
            <td class="input-field" style="width: 15% !important;">
                <telerik:RadTextBox ID="txt_cnNumber" runat="server" Skin="Web20" MaxLength="15"
                    AutoPostBack="True" onkeypress="return isNumberKey(event);" LabelWidth="64px"
                    Resize="None" resolvedrendermode="Classic" Width="160px" OnTextChanged="txt_cnNumber_TextChanged">
                </telerik:RadTextBox>
                <%--     <telerik:RadTextBox RenderMode="Lightweight" ID="txt_cnNumber" runat="server" resolvedrendermode="Classic"
                    Width="100%" Skin="Web20" MaxLength="20" 
                    OnTextChanged="txt_cnNumber_TextChanged" InputType="Number">
                </telerik:RadTextBox>--%>
            </td>
            <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;"></td>
            <td class="field" style="width: 15% !important; text-align: right !important; padding-right: 5px;">Account No.
            </td>
            <td class="input-field" style="width: 15% !important;">
                <asp:TextBox ID="txt_accountNo" runat="server" AutoPostBack="true" OnTextChanged="txt_accountNo_TextChanged"
                    Width="55%"></asp:TextBox>
                <asp:CheckBox ID="chk_accountNoFreeze" runat="server" Text="Save" AutoPostBack="true"
                    TextAlign="Right" Width="35%" OnCheckedChanged="chk_accountNoFreeze_CheckedChanged" />
            </td>
            <td class="space" style="width: 1% !important; margin: 0px 0px 0px 0px !important;"></td>
            <td class="field" style="width: 15% !important; text-align: right !important; padding-right: 5px;">Origin
            </td>
            <td class="input-field" style="width: 12% !important;">
                <asp:DropDownList ID="dd_origin" runat="server" Enabled="false" Width="100%">
                </asp:DropDownList>
            </td>
            <td class="space" style="width: 10% !important; margin: 0px 0px 0px 0px !important;"></td>
        </tr>
        <tr style="margin: 0px 0px 0px 0px !important;">
            <td class="field" style="width: 15% !important; text-align: right !important; padding-right: 5px;">Service Type
            </td>
            <td class="input-field" style="width: 15% !important;">
                <asp:DropDownList ID="dd_serviceType" runat="server" Width="100%" AppendDataBoundItems="true"
                    onchange="ChangeMaxLength(this);">
                    <asp:ListItem Value="0">Select Service Type</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;"></td>
            <td class="field" style="width: 15% !important; text-align: right !important; padding-right: 5px;">Consginer
            </td>
            <td class="input-field" style="width: 15% !important;">
                <asp:TextBox ID="txt_consigner" runat="server"></asp:TextBox>
            </td>
            <td class="space" style="width: 1% !important; margin: 0px 0px 0px 0px !important;"></td>
            <td class="field" style="width: 15% !important; text-align: right !important; padding-right: 5px;">Consignee
            </td>
            <td class="input-field" style="width: 12% !important;">
                <asp:TextBox ID="txt_consignee" runat="server" Enabled="true"></asp:TextBox>
            </td>
            <td class="space" style="width: 10% !important; margin: 0px 0px 0px 0px !important;"></td>
        </tr>
        <tr style="margin: 0px 0px 0px 0px !important;">
            <td class="field" style="width: 15% !important; text-align: right !important; padding-right: 5px;"></td>
            <td class="input-field" style="width: 15% !important;"></td>
            <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;"></td>
            <td class="field" style="width: 15% !important; text-align: right !important; padding-right: 5px;">Consigner Mobile No.
            </td>
            <td class="input-field" style="width: 15% !important;">
                <asp:TextBox ID="txt_consignerCell" runat="server" onkeypress="return isNumberKey(event);"></asp:TextBox>
            </td>
            <td class="space" style="width: 1% !important; margin: 0px 0px 0px 0px !important;"></td>
            <td class="field" style="width: 15% !important; text-align: right !important; padding-right: 5px;">Consignee Mobile No.
            </td>
            <td class="input-field" style="width: 12% !important;">
                <asp:TextBox ID="txt_consigneeCell" runat="server" Enabled="true" onkeypress="return isNumberKey(event);"></asp:TextBox>
            </td>
            <td class="space" style="width: 10% !important; margin: 0px 0px 0px 0px !important;"></td>
        </tr>
        <tr style="margin: 0px 0px 0px 0px !important;">
            <td class="field" style="width: 15% !important; text-align: right !important; padding-right: 5px;">Destination
            </td>
            <td class="input-field" style="width: 15% !important;">
                <telerik:RadComboBox ID="dd_destination" runat="server" Skin="Metro" AppendDataBoundItems="true"
                    AllowCustomText="true" MarkFirstMatch="true" Visible="true">
                    <Items>
                        <telerik:RadComboBoxItem Text="Select Destination" Value="0" Selected="true" />
                    </Items>
                </telerik:RadComboBox>
            </td>
            <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;"></td>
            <td class="field" style="width: 15% !important; text-align: right !important; padding-right: 5px;"></td>
            <td class="input-field" style="width: 15% !important;"></td>
            <td class="space" style="width: 1% !important; margin: 0px 0px 0px 0px !important;"></td>
            <td class="field" style="width: 15% !important; text-align: right !important; padding-right: 5px;">Rider Code
            </td>
            <td class="input-field" style="width: 12% !important;">
                <asp:TextBox ID="txt_riderCode" runat="server" Enabled="true" OnTextChanged="txt_riderCode_TextChanged"
                    onchange="if ( RiderWeightCheck() == false ) return;" Width="40%" AutoPostBack="true"></asp:TextBox>
                <asp:CheckBox ID="chk_riderCodeFreeze" runat="server" Text="Save" AutoPostBack="true"
                    TextAlign="Right" Width="40%" OnCheckedChanged="chk_riderCodeFreeze_CheckedChanged" />
            </td>
            <td class="space" style="width: 10% !important; margin: 0px 0px 0px 0px !important;"></td>
        </tr>
        <tr style="margin: 0px 0px 0px 0px !important;">
            <td class="field" style="width: 15% !important; text-align: right !important; padding-right: 5px;">Dimensions (L X W X H)
            </td>
            <td class="input-field" style="width: 15% !important;">
                <asp:TextBox ID="txt_l" onchange="VolumeChange()" runat="server" Width="15%"></asp:TextBox><b>
                    X </b>
                <asp:TextBox ID="txt_w" onchange="VolumeChange()" runat="server" Width="15%"></asp:TextBox><b>
                    X </b>
                <asp:TextBox ID="txt_h" onchange="VolumeChange()" runat="server" Width="15%"></asp:TextBox>
            </td>
            <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;"></td>
            <td class="field" style="width: 15% !important; text-align: right !important; padding-right: 5px;">Volumetric Weight
            </td>
            <td class="input-field" style="width: 15% !important;">
                <asp:TextBox ID="txt_vWeight" runat="server" Enabled="false" Width="40%"></asp:TextBox>
            </td>
            <td class="space" style="width: 1% !important; margin: 0px 0px 0px 0px !important;"></td>
            <td class="field" style="width: 15% !important; text-align: right !important; padding-right: 5px;">Dense Weight
            </td>
            <td class="input-field" style="width: 12% !important;">
                <asp:TextBox ID="txt_aWeight" runat="server" onchange="VolumeChange()" Width="40%"
                    MaxLength="4"></asp:TextBox>
            </td>
            <td class="space" style="width: 10% !important; margin: 0px 0px 0px 0px !important;"></td>
        </tr>
        <tr style="margin: 0px 0px 0px 0px !important;">
            <td class="field" style="width: 15% !important; text-align: right !important; padding-right: 5px;"></td>
            <td class="input-field" style="width: 15% !important;"></td>
            <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;"></td>
            <td class="field" style="width: 15% !important; text-align: right !important; padding-right: 5px;"></td>
            <td class="input-field" style="width: 15% !important;"></td>
            <td class="space" style="width: 1% !important; margin: 0px 0px 0px 0px !important;"></td>
            <td class="field" style="width: 15% !important; text-align: right !important; padding-right: 5px;">Weight
            </td>
            <td class="input-field" style="width: 12% !important;">
                <asp:TextBox ID="txt_weight" runat="server" MaxLength="4" Width="40%" Enabled="false"
                    OnTextChanged="txt_weight_TextChanged" onkeypress="return isNumberKeydouble(event);"
                    onchange="if ( RiderWeightCheck() == false ) return;" AutoPostBack="true"></asp:TextBox>
            </td>
            <td class="space" style="width: 10% !important; margin: 0px 0px 0px 0px !important;"></td>
        </tr>
        <tr style="margin: 0px 0px 0px 0px !important;">
            <td class="field" style="width: 15% !important; text-align: right !important; padding-right: 5px;">Address
            </td>
            <td class="input-field" style="width: 48% !important;" colspan="4">
                <asp:TextBox ID="txt_Address" runat="server" Width="100%"></asp:TextBox>
            </td>
            <td class="space" style="width: 1% !important; margin: 0px 0px 0px 0px !important;"></td>
            <td class="field" style="width: 15% !important; text-align: right !important; padding-right: 5px;">Pieces
            </td>
            <td class="input-field" style="width: 12% !important;">
                <asp:TextBox ID="txt_pieces" runat="server" Enabled="true" onkeypress="return isNumberKey(event);"
                    Width="40%" AutoPostBack="false"></asp:TextBox>
            </td>
            <td class="space" style="width: 10% !important; margin: 0px 0px 0px 0px !important;"></td>
        </tr>
        <tr style="margin: 0px 0px 0px 0px !important;">
            <td class="field" style="width: 15% !important; text-align: right !important; padding-right: 5px;">Origin Express Center
            </td>
            <td class="input-field" style="width: 15% !important;">
                <asp:DropDownList ID="dd_originExpressCenter" runat="server" AppendDataBoundItems="true"
                    Enabled="true" Width="100%">
                    <asp:ListItem Value="0">--Select EC--</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;"></td>
            <td class="field" style="width: 15% !important; text-align: right !important; padding-right: 5px;">CN Type
            </td>
            <td class="input-field" style="width: 15% !important;">
                <asp:DropDownList ID="dd_consignmentType" runat="server" AppendDataBoundItems="true">
                    <asp:ListItem Value="0">Select CN Type</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td class="space" style="width: 1% !important; margin: 0px 0px 0px 0px !important;"></td>
            <td class="field" style="width: 15% !important; text-align: right !important; padding-right: 5px;">Coupon Number
            </td>
            <td class="input-field" style="width: 12% !important;">
                <asp:TextBox ID="txt_coupon" runat="server" Enabled="true" MaxLength="500" ToolTip="Use Comma(,) for multiple Entries"></asp:TextBox>
            </td>
            <td class="space" style="width: 10% !important; margin: 0px 0px 0px 0px !important;"></td>
        </tr>
        <tr style="margin: 0px 0px 0px 0px !important;">
            <td class="field" style="width: 15% !important; text-align: right !important; padding-right: 5px;">Special Instructions
            </td>
            <td class="input-field" style="width: 32% !important; margin-right: 8px;" colspan="3">
                <asp:TextBox ID="txt_remarks" runat="server" Width="100%"></asp:TextBox>
            </td>
            <td class="input-field" style="width: 15% !important;"></td>
            <td class="space" style="width: 1% !important; margin: 0px 0px 0px 0px !important;"></td>
            <td class="field" style="width: 15% !important; text-align: right !important; padding-right: 5px;">Charged Amount
            </td>
            <td class="input-field" style="width: 12% !important;">
                <asp:TextBox ID="txt_chargedAmount" runat="server" Enabled="false" onkeypress="return isNumberKeydouble(event);"></asp:TextBox>
            </td>
            <td class="space" style="width: 10% !important; margin: 0px 0px 0px 0px !important;"></td>
        </tr>
        <tr style="margin: 0px 0px 0px 0px !important;">
            <td class="field" style="width: 15% !important; text-align: right !important; padding-right: 5px;"></td>
            <td class="input-field" style="width: 15% !important;"></td>
            <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;"></td>
            <td class="field" style="width: 15% !important; text-align: right !important; padding-right: 5px;"></td>
            <td class="input-field" style="width: 15% !important;"></td>
            <td class="space" style="width: 1% !important; margin: 0px 0px 0px 0px !important;"></td>
            <td class="field" style="width: 15% !important; text-align: right !important; padding-right: 5px;">Total Amount
            </td>
            <td class="input-field" style="width: 12% !important;">
                <asp:TextBox ID="txt_totalAmt" runat="server" Enabled="false"></asp:TextBox>
            </td>
            <td class="space" style="width: 10% !important; margin: 0px 0px 0px 0px !important;"></td>
        </tr>
        <tr style="margin: 0px 0px 0px 0px !important;">
            <td class="field" style="width: 15% !important; text-align: right !important; padding-right: 5px;"></td>
            <td class="input-field" style="width: 15% !important;"></td>
            <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;"></td>
            <td class="field" style="width: 15% !important; text-align: right !important; padding-right: 5px;"></td>
            <td class="input-field" style="width: 15% !important;"></td>
            <td class="space" style="width: 1% !important; margin: 0px 0px 0px 0px !important;"></td>
            <td class="field" style="width: 15% !important; text-align: right !important; padding-right: 5px;">GST
            </td>
            <td class="input-field" style="width: 12% !important;">
                <asp:TextBox ID="txt_gst" runat="server" Enabled="false"></asp:TextBox>
            </td>
            <td class="space" style="width: 10% !important; margin: 0px 0px 0px 0px !important;"></td>
        </tr>
        <tr style="margin: 0px 0px 0px 0px !important;">
            <td class="field" style="width: 15% !important; text-align: right !important; padding-right: 5px;">Reporting Date
            </td>
            <td class="input-field" style="width: 15% !important;">
                <telerik:RadDatePicker ID="picker_reportingDate" runat="server" DateInput-DateFormat="dd/MM/yyyy"
                    Width="60%">
                </telerik:RadDatePicker>
                <asp:CheckBox ID="chk_reportingDateFreeze" runat="server" Text="Save" AutoPostBack="true"
                    TextAlign="Right" Width="35%" OnCheckedChanged="chk_reportingDateFreeze_CheckedChanged" />
            </td>
            <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;"></td>
            <td class="field" style="width: 15% !important; text-align: right !important; padding-right: 5px;">Approval Status
            </td>
            <td class="input-field" style="width: 15% !important;">
                <asp:DropDownList ID="dd_approvalStatus" runat="server" AppendDataBoundItems="true"
                    Enabled="false">
                    <asp:ListItem Value="0">Unapproved</asp:ListItem>
                    <asp:ListItem Value="1">Approved</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td class="space" style="width: 1% !important; margin: 0px 0px 0px 0px !important;"></td>
            <td class="field" style="width: 15% !important; text-align: right !important; padding-right: 5px;">Invoice Status
            </td>
            <td class="input-field" style="width: 12% !important;">
                <asp:TextBox ID="txt_invoiceStatus" runat="server" Enabled="false"></asp:TextBox>
            </td>
            <td class="space" style="width: 10% !important; margin: 0px 0px 0px 0px !important;"></td>
        </tr>
        <tr style="margin: 0px 0px 0px 0px !important;">
            <td class="field" style="width: 15% !important; text-align: right !important; padding-right: 5px;">Invoice No.
            </td>
            <td class="input-field" style="width: 15% !important;">
                <asp:TextBox ID="txt_invoiceNumber" runat="server" Enabled="false"></asp:TextBox>
            </td>
            <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;"></td>
            <td class="field" style="width: 15% !important; text-align: right !important; padding-right: 5px;"></td>
            <td class="input-field" style="width: 15% !important;"></td>
            <td class="space" style="width: 1% !important; margin: 0px 0px 0px 0px !important;"></td>
            <td class="field" style="width: 15% !important; text-align: right !important; padding-right: 5px;"></td>
            <td class="input-field" style="width: 12% !important;"></td>
            <td class="space" style="width: 10% !important; margin: 0px 0px 0px 0px !important;"></td>
        </tr>
    </table>
    <asp:Panel ID="codTable" runat="server">
        <table style="font-family: Calibri; font-size: medium; padding-bottom: 0px !important; padding-top: 0px !important; width: 97%"
            class="input-form">
            <tr style="float: none !important;">
                <td colspan="9" style="float: none !important; font-variant: small-caps !important; font-size: large; text-align: center;">
                    <b>COD Info</b>
                </td>
            </tr>
            <tr style="margin: 20px 0px 0px !important; padding: 10px 0px 0px 0px !important;">
                <td class="field" style="width: 15% !important; text-align: right !important; padding-right: 5px;">Order Ref. No.
                </td>
                <td class="input-field" style="width: 15% !important;">
                    <asp:TextBox ID="txt_orderRefNo" runat="server"></asp:TextBox>
                </td>
                <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;"></td>
                <td class="field" style="width: 15% !important; text-align: right !important; padding-right: 5px;">Product Type
                </td>
                <td class="input-field" style="width: 15% !important; vertical-align: middle !important;">
                    <asp:DropDownList ID="dd_productType" runat="server">
                    </asp:DropDownList>
                </td>
                <td class="space" style="width: 1% !important; margin: 0px 0px 0px 0px !important;"></td>
                <td class="field" style="width: 15% !important; text-align: right !important; padding-right: 5px;">Charge COD Amount.
                </td>
                <td class="input-field" style="width: 12% !important;">
                    <asp:CheckBox ID="Cb_CODAmount" runat="server" AutoPostBack="true" Enabled="false" />
                </td>
                <td class="space" style="width: 10% !important; margin: 0px 0px 0px 0px !important;"></td>
            </tr>
            <tr style="margin: 20px 0px 0px !important; padding: 10px 0px 0px 0px !important;">
                <td class="field" style="width: 15% !important; text-align: right !important; padding-right: 5px;">Description
                </td>
                <td class="input-field" style="width: 15% !important;">
                    <asp:TextBox ID="txt_descriptionCOD" runat="server"></asp:TextBox>
                </td>
                <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;"></td>
                <td class="field" style="width: 15% !important; text-align: right !important; padding-right: 5px;">COD Amount
                </td>
                <td class="input-field" style="width: 15% !important; vertical-align: middle !important;">
                    <asp:TextBox ID="txt_codAmount" runat="server" onkeypress="return isNumberKey(event);"></asp:TextBox>
                </td>
                <td class="space" style="width: 1% !important; margin: 0px 0px 0px 0px !important;"></td>
                <td class="field" style="width: 15% !important; text-align: right !important; padding-right: 5px;"></td>
                <td class="input-field" style="width: 12% !important;"></td>
                <td class="space" style="width: 10% !important; margin: 0px 0px 0px 0px !important;"></td>
        </table>
    </asp:Panel>
    <table style="font-family: Calibri; font-size: medium; padding-bottom: 0px !important; padding-top: 0px !important; width: 97%"
        class="input-form">
        <tr style="float: none !important;">
            <td colspan="9" style="float: none !important; font-variant: small-caps !important; font-size: large; text-align: center;">
                <b>Price Modifiers</b>
            </td>
        </tr>
        <tr style="margin: 20px 0px 0px !important; padding: 10px 0px 0px 0px !important;">
            <td class="field" style="width: 15% !important; padding-right: 5px;">
                <div style="display: inline; text-align: left; width: 100%">
                    Modifier Name
                </div>
                <div>
                    <asp:DropDownList ID="dd_priceModifier" runat="server" AppendDataBoundItems="true"
                        AutoPostBack="true" Width="100%" OnSelectedIndexChanged="dd_priceModifier_SelectedIndexChanged">
                        <asp:ListItem Value="0">Select Price Modifier</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </td>
            <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;"></td>
            <td class="field" style="width: 25% !important; font-weight: normal !important; padding-right: 5px;">
                <div style="display: inline; text-align: left; width: 100%">
                    <b>Calculation Base</b>
                </div>
                <div>
                    <asp:RadioButtonList ID="dd_calculationBase" runat="server" RepeatColumns="3" RepeatDirection="Horizontal" Enabled="false">
                        <asp:ListItem Value="1">Flat</asp:ListItem>
                        <asp:ListItem Value="2">Percentage</asp:ListItem>
                        <asp:ListItem Value="3">Insurance</asp:ListItem>
                    </asp:RadioButtonList>
                </div>
            </td>
            <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;"></td>
            <td class="field" style="width: 15% !important; font-weight: normal !important; padding-right: 5px;">
                <div style="display: inline; text-align: left; width: 100%">
                    <b>Modifier Value</b>
                </div>
                <div>
                    <asp:TextBox ID="txt_priceModifierValue" runat="server" Enabled="false"></asp:TextBox>
                </div>
            </td>
            <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;"></td>
            <td class="field" style="width: 20% !important; font-weight: normal !important; padding-right: 5px;">
                <div style="display: inline; text-align: left; width: 100%">
                    <b>Declared Value(Insurance)</b>
                </div>
                <div>
                    <asp:TextBox ID="txt_declaredValue" runat="server" Enabled="false"></asp:TextBox>
                </div>
            </td>
            <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;"></td>
            <td class="field" style="width: 10% !important; text-align: left !important;">
                <div style="display: inline; text-align: left; width: 100%">
                    &nbsp;
                </div>
                <div>
                    <asp:Button ID="btn_add" runat="server" CssClass="button" Text="Add" Width="75%"
                        OnClick="btn_add_Click" />
            </td>
        </tr>
        <%--<tr style="margin: 0px 0px 0px !important; padding: 0px 0px 0px 0px !important;">
            <td class="field" style="width: 15% !important; text-align: right !important; padding-right: 5px;">
            </td>
            <td class="input-field" style="width: 15% !important;">
            </td>
            <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;">
            </td>
            <td class="field" style="width: 15% !important; text-align: right !important; padding-right: 5px;">
            </td>
            <td class="input-field" style="width: 15% !important; vertical-align: middle !important;">
            </td>
            <td class="space" style="width: 1% !important; margin: 0px 0px 0px 0px !important;">
            </td>
            <td class="field" style="width: 15% !important; text-align: right !important; padding-right: 5px;">
            </td>
            <td class="input-field" style="width: 12% !important; text-align: right !important;">
                <asp:Button ID="btn_add" runat="server" CssClass="button" Text="Add" Width="75%"
                    OnClick="btn_add_Click" />
            </td>
            <td class="space" style="width: 10% !important; margin: 0px 0px 0px 0px !important;">
            </td>
        </tr>--%>
        <tr style="float: none !important;">
            <td colspan="9" style="float: none !important;">
                <div style="width: 100%; height: 150px; overflow: scroll; text-align: center;">
                    <span id="Span1" class="tbl-large" style="width: 100%;">
                        <asp:Label ID="Label1" runat="server" CssClass="error_msg"></asp:Label>
                        <asp:GridView ID="gv_CNModifiers" runat="server" AutoGenerateColumns="false" CssClass="mGrid"
                            AlternatingRowStyle-CssClass="alt" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px"
                            Width="95%" EmptyDataText="No Data Available" OnRowCommand="gv_CNModifiers_RowCommand"
                            OnRowDataBound="gv_CNModifiers_RowDataBound">
                            <RowStyle Font-Bold="false" />
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Button ID="btn_gRemove" runat="server" CommandName="Remove" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "priceModifierId") %>'
                                            Text="Remove" CssClass="button" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Name" DataField="priceModifierName" />
                                <asp:TemplateField HeaderText="Value">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_gValue" Style="overflow: hidden;" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "calculatedValue") %>'
                                            Enabled="true"></asp:TextBox>
                                        <asp:HiddenField ID="hd_new" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "NEW") %>' />
                                        <asp:HiddenField ID="hd_sortOrder" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "SortOrder") %>' />
                                        <asp:HiddenField ID="hd_value" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "ModifiedCalculatedValue") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--<asp:BoundField HeaderText="Value" DataField="calculatedValue" />--%>
                                <asp:BoundField HeaderText="Calculation Base" DataField="calculationBase" />
                                <asp:BoundField HeaderText="Taxable" DataField="isTaxable" />
                                <asp:BoundField HeaderText="Description" DataField="description" />
                                <asp:TemplateField HeaderText="DeclaredValue">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_gDeclaredValue" Style="overflow: hidden;" Enabled="false" runat="server"
                                            Text='<%# DataBinder.Eval(Container.DataItem, "AlternateValue") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--<asp:BoundField HeaderText="New" DataField="NEW" Visible="false" />
                                <asp:BoundField HeaderText="SortOrder" DataField="SortOrder" Visible="false" />--%>
                            </Columns>
                        </asp:GridView>
                    </span>
                </div>
            </td>
        </tr>
    </table>
    <div style="width: 100%; text-align: center">
        <asp:Button ID="btn_reset" runat="server" Text="Reset" CssClass="button" OnClick="btn_reset_Click" />
        &nbsp;
        <asp:Button ID="btn_save" runat="server" Text="Approve" CssClass="button" OnClick="btn_SaveConsignment1_Click" />
        &nbsp;
        <asp:Button ID="btn_unapprove" runat="server" Text="Unapprove" CssClass="button"
            OnClick="btn_unapprove_Click" />
    </div>
</asp:Content>
