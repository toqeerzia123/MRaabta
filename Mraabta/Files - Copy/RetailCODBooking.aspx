<%@ Page Title="Retail COD Booking Screen" Language="C#" MasterPageFile="~/BtsMasterPage.master"
    AutoEventWireup="true" EnableEventValidation="False" CodeBehind="RetailCODBooking.aspx.cs"
    Inherits="MRaabta.Files.RetailCODBooking" %>

<%@ Register TagPrefix="Ajax1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        .repeater_tbl {
            margin: 0;
            border-radius: 10px;
            box-shadow: 0px 0px 3px #000;
            padding: 10px;
        }

        .blinking {
            animation: blinkingText 0.8s infinite;
            font-weight: bold;
            font-size: 16px;
        }

        @keyframes blinkingText {
            0% {
                color: red;
            }

            49% {
                color: #000;
            }

            50% {
                color: red;
            }

            99% {
                color: red;
            }

            100% {
                color: #000;
            }
        }
    </style>
    <script type="text/javascript">



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
        function isNumberKey(evt) {

            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {

                return false;
            }
            return true;
        }
        function weightChange() {
            debugger;
            var saveButton = document.getElementById('<%= btn_SaveConsignment0.ClientID %>');

            saveButton.disabled = true;
        }
        function VolumeChange() {
            debugger;
            var saveButton = document.getElementById('<%= btn_SaveConsignment0.ClientID %>');
            var txtlength = document.getElementById('<%= txt_l.ClientID %>');
            var txtwidth = document.getElementById('<%= txt_w.ClientID %>');
            var txtheight = document.getElementById('<%= txt_h.ClientID %>');
            var txtvWeight = document.getElementById('<%= txt_vWeight.ClientID %>');
            var txtaWeight = document.getElementById('<%= txt_aWeight.ClientID %>');
            var txtweight = document.getElementById('<%= txt_Weight.ClientID %>');
            var txtPieces = document.getElementById('<%= txt_Piecies.ClientID %>');

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

            var p = parseInt(txtPieces.value);
            if (p.toString() == 'NaN') {
                p = 1;
            }
            var aWeight = parseFloat(txtaWeight.value);
            if (aWeight.toString() == 'NaN') {
                aWeight = 0;
            }




            var vWeight = (l * w * h) / 5000;
            vWeight = vWeight * p;

            txtvWeight.value = vWeight.toString();
            if (vWeight > aWeight) {
                txtweight.value = vWeight.toString();
            }
            else {
                txtweight.value = aWeight.toString();
            }



            saveButton.disabled = true;
        }
        function disabilityCheck() {
            var saveButton = document.getElementById('<%= btn_SaveConsignment0.ClientID %>');
            if (saveButton.disabled == true) {
                alert("Please Press Validation Button First");
            }
        }

        function PaymentModeChange() {
            var saveButton = document.getElementById('<%= dd_PaymentMode.ClientID %>');
            val_ = saveButton.options[saveButton.selectedIndex].value
            if (val_ == "6") {
                document.getElementById('<%= txt_TransactionID.ClientID %>').maxLength = "12";
                document.getElementById('<%= txt_TransactionID.ClientID %>').disabled = false;
                document.getElementById('<%= txt_TransactionID.ClientID %>').value = "0";

            }
            else if (val_ == "7") {
                document.getElementById('<%= txt_TransactionID.ClientID %>').maxLength = "10";
                document.getElementById('<%= txt_TransactionID.ClientID %>').disabled = false;
                document.getElementById('<%= txt_TransactionID.ClientID %>').value = "0";
            }
            else if (val_ == "9") {
                document.getElementById('<%= txt_TransactionID.ClientID %>').maxLength = "12";
                document.getElementById('<%= txt_TransactionID.ClientID %>').disabled = false;
                document.getElementById('<%= txt_TransactionID.ClientID %>').value = "0";
            }
            else if (val_ == "10") {
                document.getElementById('<%= txt_TransactionID.ClientID %>').maxLength = "11";
                document.getElementById('<%= txt_TransactionID.ClientID %>').disabled = false;
                document.getElementById('<%= txt_TransactionID.ClientID %>').value = "0";
            }
            else {
                document.getElementById('<%= txt_TransactionID.ClientID %>').Text = "0";
                document.getElementById('<%= txt_TransactionID.ClientID %>').value = "0";
                document.getElementById('<%= txt_TransactionID.ClientID %>').disabled = true;
            }


        }
    </script>

    <table width="100%">
        <tr>
            <td colspan="12" align="center" class="head_column">
                <h3>Retail COD Booking</h3>
            </td>
        </tr>
    </table>
    <table width="100%">
        <tr>
            <td>
                <asp:Label ID="lbl_Error" runat="server" ForeColor="Red"></asp:Label>
            </td>
        </tr>
        <tr>
            <td rowspan="12" style="width: 24%; vertical-align: top;">
                <fieldset style="height: 398px;">
                    <legend>Consignment Info</legend>
                    <table>
                        <tr style="display:none;">
                            <td>Consignment No.
                            </td>
                            <td>
                                <asp:TextBox ID="txt_ConNo" runat="server" MaxLength="15" onkeypress="return isNumberKey(event);"
                                    onchange="weightChange()" Width="160px"> 
                                </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Account No.
                            </td>
                            <td>
                                <%--<asp:TextBox ID="txt_AccNo" runat="server" MaxLength="8" Width="70px" OnTextChanged="txt_AccNo_Fixed_TextChanged"
                                    AutoPostBack="true">--%>
                                <asp:TextBox ID="txt_AccNo" runat="server" MaxLength="8" Width="70px" AutoPostBack="true"
                                    OnTextChanged="txt_AccNo_Fixed_TextChanged">
                                </asp:TextBox>


                                <asp:CheckBox ID="cb_Account" runat="server" Text="Lock" AutoPostBack="true" Visible="false"
                                    OnCheckedChanged="cb_Account_CheckedChanged"></asp:CheckBox>
                                <asp:HiddenField ID="hd_CreditClientID" runat="server" />
                                <asp:HiddenField ID="hd_CreditClientIDForRetailCOD" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>Rider Code.
                            </td>
                            <td>
                                <asp:TextBox ID="txt_RiderCode" runat="server" MaxLength="8" Width="70px" Enabled="false">
                                </asp:TextBox>
                                <asp:CheckBox ID="cb_Rider" runat="server" Visible="false" Text="Lock"></asp:CheckBox>
                                <asp:HiddenField ID="hd_OriginExpressCenter" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>Con. Type
                            </td>
                            <td>
                                <asp:DropDownList ID="cb_ConType" runat="server" AppendDataBoundItems="true" onchange="weightChange()"
                                    Width="90%">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>City
                            </td>
                            <td>
                                <asp:DropDownList ID="dd_city" runat="server" AppendDataBoundItems="true" onchange="weightChange()"
                                    Width="90%">
                                    <asp:ListItem Value="0">Select City</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>
                                <%--<asp:DropDownList id="cb_Destination" runat="server" skin="Metro" appenddatabounditems="true"
                                        autopostback="true" onitemselected="cb_Destination_ItemSelected" allowcustomtext="true"
                                        markfirstmatch="true" visible="false" onselectedindexchanged="cb_Destination_SelectedIndexChanged">
                                        <Items>
                                            <telerik:RadComboBoxItem Text="Select Destination" Value="0" />
                                        </Items>
                                    </telerik:radcombobox>--%>
                                <asp:HiddenField ID="hd_Destination" runat="server" />
                                <asp:HiddenField ID="hd_Destination_Ec" runat="server" />
                                <asp:HiddenField ID="hd_DestinationZone" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <b>L X W X H</b>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_l" runat="server" Width="40px" onchange="VolumeChange()" onkeypress="return isNumberKeydouble(event);"
                                    ToolTip="in cm"></asp:TextBox><b> X </b>
                                <asp:TextBox ID="txt_w" runat="server" Width="40px" onchange="VolumeChange()" onkeypress="return isNumberKeydouble(event);"
                                    ToolTip="in cm"></asp:TextBox><b> X </b>
                                <asp:TextBox ID="txt_h" runat="server" Width="40px" onchange="VolumeChange()" onkeypress="return isNumberKeydouble(event);"
                                    ToolTip="in cm"></asp:TextBox>
                                <b>in cm</b>
                            </td>
                        </tr>
                        <tr>
                            <td>Volumetric Weight
                            </td>
                            <td>
                                <asp:TextBox ID="txt_vWeight" runat="server" Enabled="false" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Dense Weight
                            </td>
                            <td>
                                <asp:TextBox ID="txt_aWeight" runat="server" Enabled="true" onkeypress="return isNumberKeydouble(event);"
                                    Width="100px" onchange="VolumeChange()"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="1">Weight
                            </td>
                            <td colspan="1">
                                <asp:TextBox ID="txt_Weight" runat="server" Width="100px" MaxLength="4" onkeypress="return isNumberKeydouble(event);"
                                    Enabled="false" onchange="weightChange()">
                                </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Service Type
                            </td>
                            <td>
                                <asp:DropDownList ID="cb_ServiceType" runat="server" AppendDataBoundItems="true"
                                    Width="90%" onchange="weightChange()">
                                    <asp:ListItem Text="Select Service Type" Value="0" />
                                </asp:DropDownList>
                                <%--OnSelectedIndexChanged="cb_ServiceType_SelectedIndexChanged1"--%>
                            </td>
                        </tr>
                        <tr>
                            <td>Discount
                            </td>
                            <td>
                                <asp:TextBox ID="txt_Discount" runat="server" MaxLength="7" Width="100px" Enabled="false"
                                    onkeypress="return isNumberKeydouble(event);">
                                        
                                </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Pieces
                            </td>
                            <td>
                                <asp:TextBox ID="txt_Piecies" value="1" runat="server" Width="100px" onkeydown="return (event.keyCode!=13);"
                                    onchange="VolumeChange()">
                                </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Payment Mode
                            </td>
                            <td>
                                <asp:DropDownList ID="dd_PaymentMode" runat="server" AppendDataBoundItems="true"
                                    onchange="PaymentModeChange();" Width="90%">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>Payment Transaction ID
                            </td>
                            <td>
                                <asp:TextBox ID="txt_TransactionID" value="0" Enabled="false" runat="server" Width="100px"
                                    onkeypress="return isNumberKey(event);"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </td>
            <td rowspan="9" style="width: 50%; vertical-align: top;">
                <fieldset style="height: 398px;">
                    <legend>Consignee & Consigner Info</legend>
                    <table>
                        <tr>
                            <td>Consignee CellNo
                            </td>
                            <td>
                                <asp:TextBox ID="txt_ConsigneeCellno" runat="server" MaxLength="12" Width="160px"
                                    AutoPostBack="true" onkeypress="return isNumberKey(event);" OnTextChanged="txt_ConsigneeCellno_TextChanged">
                                </asp:TextBox>
                            </td>
                            <td colspan="2" rowspan="9" valign="top">
                                <table>
                                    <tr>
                                        <td>Booking Date
                                        </td>
                                        <td>
                                            <asp:TextBox ID="dt_Picker" runat="server"></asp:TextBox>
                                            <Ajax1:CalendarExtender ID="Calendar1" runat="server" TargetControlID="dt_Picker"
                                                Format="dd/MM/yyyy">
                                            </Ajax1:CalendarExtender>
                                            <%--<telerik:raddatepicker id="dt_Picker" runat="server" culture="en-US" resolvedrendermode="Classic"
                                                    enabled="False">
                                                    <Calendar runat="server" UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False"
                                                        EnableWeekends="True" FastNavigationNextText="&amp;lt;&amp;lt;">
                                                    </Calendar>
                                                    <DateInput runat="server" DisplayDateFormat="dd/MM/yyyy" DateFormat="dd/MM/yyyy"
                                                        LabelWidth="40%">
                                                        <EmptyMessageStyle Resize="None"></EmptyMessageStyle>
                                                        <ReadOnlyStyle Resize="None"></ReadOnlyStyle>
                                                        <FocusedStyle Resize="None"></FocusedStyle>
                                                        <DisabledStyle Resize="None"></DisabledStyle>
                                                        <InvalidStyle Resize="None"></InvalidStyle>
                                                        <HoveredStyle Resize="None"></HoveredStyle>
                                                        <EnabledStyle Resize="None"></EnabledStyle>
                                                    </DateInput>
                                                    <DatePopupButton ImageUrl="" HoverImageUrl=""></DatePopupButton>
                                                </telerik:raddatepicker>--%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Origin
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="cb_Origin" runat="server" Enabled="false">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Origin Express Center
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="cb_originExpresscenter" runat="server" Enabled="false" Width="90%">
                                            </asp:DropDownList>
                                            <asp:HiddenField ID="hd_oecCatid" runat="server" />
                                        </td>
                                    </tr>
                                    <%--<tr>
                                        <td>
                                            Insurance
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_insurance" runat="server" MaxLength="7" CssClass="med-field-right"
                                                Enabled="false">
                                                    
                                            </asp:TextBox>
                                        </td>
                                    </tr>--%>
                                    <tr>
                                        <td>Total Charges
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_TotalCharges" runat="server" MaxLength="7" CssClass="med-field-right"
                                                Enabled="false">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Gst Charges
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_Othercharges" runat="server" MaxLength="7" CssClass="med-field-right"
                                                Enabled="false">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Total Amount
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_TotalAmount" runat="server" MaxLength="7" CssClass="med-field-right"
                                                Enabled="false">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="display: none">
                                            <fieldset>
                                                <legend>Day Type</legend>
                                                <asp:RadioButtonList ID="rb_1" runat="server" RepeatDirection="Horizontal">
                                                    <asp:ListItem Text="WeekDay" Selected="True" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="Sunday" Value="2"></asp:ListItem>
                                                    <asp:ListItem Text="Holiday" Value="3"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </fieldset>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>Consignee
                            </td>
                            <td>
                                <asp:TextBox ID="txt_Consignee" runat="server" MaxLength="40" Width="160px">
                                </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Consignee CNIC
                            </td>
                            <td>
                                <asp:TextBox ID="txt_ConsigneeCNIC" runat="server" MaxLength="13" Width="160px" onkeypress="return isNumberKey(event);">
                                </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Consigner CellNo
                            </td>
                            <td>
                                <script src="http://code.jquery.com/jquery-1.11.0.min.js"></script>
                                <script type='text/javascript'>
                                    $(document).ready(function () {
                                        $('#txt_ConsignerCellNo').mask('(999) 999-9999');
                                    });
                                </script>

                                <asp:TextBox ID="txt_ConsignerCellNo" runat="server" MaxLength="12" Width="160px"
                                    AutoPostBack="true" onkeypress="return isNumberKey(event);" OnTextChanged="txt_ConsignerCellNo_TextChanged1">
                                </asp:TextBox>
                            </td>

                        </tr>
                        <tr>
                            <td>Consigner
                            </td>
                            <td>
                                <asp:TextBox ID="txt_Consigner" runat="server" MaxLength="40" Width="160px">
                                </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Consigner CNIC
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_ConsignerCNIC" runat="server" MaxLength="13" Width="160px" AutoPostBack="true" OnTextChanged="txt_ConsignerCellNo_TextChanged1" onkeypress="return isNumberKey(event);">
                                </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_Type" runat="server">Coupon No</asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_Type" runat="server" MaxLength="500" Width="160px">
                                </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="1"></td>
                            <td colspan="1">
                                <script type="text/javascript">
                                    function ShowHideDiv(cb_Insurance) {
                                        debugger;
                                        var dvPassport = document.getElementById("ContentPlaceHolder1_div_InsuranceMsg");
                                        dvPassport.style.display = cb_Insurance.checked ? "none" : "block";
                                    }
                                </script>
                                <asp:CheckBox ID="cb_COD" runat="server" Text="COD" Enabled="False" Visible="false" />
                                <asp:CheckBox ID="cb_Insurance" runat="server" Text="Insurance Offered" Enabled="true"
                                    onclick="ShowHideDiv(this)" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <%--Declared Values--%>
                            </td>
                            <td>
                                <%--<asp:TextBox ID="txt_DeclaredValue" runat="server" MaxLength="7" CssClass="med-field-right"
                                    Enabled="false">--%>
                                </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Package/Handcarry
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txt_Package_Handcarry" runat="server" MaxLength="100" Width="350px">
                                </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Consignee Address
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txt_Address" runat="server" MaxLength="100" Width="350px">
                                </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Shipper Address
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txt_ShipperAddress" runat="server" MaxLength="100" Width="350px">
                                </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Special Instructions
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txt_remarks" runat="server" MaxLength="100" Width="350px">
                                </asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <div id="div_InsuranceMsg" runat="server" style="display: block">
                        <asp:Label ID="lbl_InsuranceMsg" Text="Ask the Customer for Insurance." runat="server"
                            CssClass="blinking"></asp:Label>
                    </div>
                </fieldset>
            </td>
            <td rowspan="9" style="width: 20%; vertical-align: top; display: none;">
                <fieldset>
                    <legend>Config Info</legend>
                    <table>
                        <tr>
                            <td colspan="2">
                                <asp:CheckBox ID="cb_RangeSelection" runat="server" Text="Consignment Ranges" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">First Con. No :
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:TextBox ID="txt_LblFirstCon" runat="server" AutoPostBack="True" Width="160px"
                                    Text="0" Enabled="false" MaxLength="12">

                                </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">Last Con. No :
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:TextBox ID="txt_LblLastConsignment" runat="server" Text="0" Width="160px" Enabled="false"
                                    MaxLength="12">
                                        
                                </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">User Behaviour :
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:RadioButtonList ID="Rb_UserBehaviour" runat="server" RepeatDirection="Horizontal"
                                    Enabled="false">
                                    <asp:ListItem Text="Exp. Cen" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Hub"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" class="style1">Customer Type :
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:RadioButtonList ID="Rb_CustomerType" runat="server" Enabled="false" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="Credit" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Cash"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">Customer Entry
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:RadioButtonList ID="rb_CustomerEntry" runat="server" RepeatDirection="Horizontal"
                                    Enabled="False">
                                    <asp:ListItem Text="Normal" Value="1" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Series" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="Bulk" Value="3"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:CheckBox ID="txt_Preprinted" runat="server" Text="Pre Printed Report"></asp:CheckBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:CheckBox ID="txt_SmsConsignment" runat="server" Text="SMS Consignee"></asp:CheckBox>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </td>
            <td rowspan="9" style="width: 20%; vertical-align: top;">
                <fieldset>
                    <legend>CN Sequence Info</legend>
                    <table>
                        <tr>
                            <td colspan="2">
                                <asp:DropDownList ID="dd_sequence" runat="server">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <fieldset>
                    <legend>Discount Info</legend>
                    <table>
                        <tr>
                            <td>Discount Id:<br />
                                <asp:TextBox ID="txt_discountId" runat="server" onkeypress="return isNumberKey(event);"
                                    AutoPostBack="true" OnTextChanged="discountId_Fixed_TextChanged"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">Discount:<br />
                                <asp:DropDownList ID="dd_discount" runat="server" AppendDataBoundItems="true" Width="90%"
                                    AutoPostBack="true" OnSelectedIndexChanged="discount_SelectedIndexChanged">
                                    <asp:ListItem Value="0">..: Select Discount :..</asp:ListItem>
                                </asp:DropDownList>
                                <asp:Label ID="lbl_discount" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <fieldset id="div_discountcn" runat="server">
                    <legend>Discounted CNs</legend>
                    <div style="overflow-y: scroll; height: 123px;">
                        <asp:Label ID="discountedCN" runat="server"></asp:Label>
                    </div>
                    <asp:Button ID="btn_discount_save" runat="server" Text="Apply Discount" CssClass="button"
                        UseSubmitBehavior="false" Visible="false" OnClick="btn_Discount_Save_Click"></asp:Button>
                </fieldset>
            </td>
        </tr>
    </table>
    <fieldset>
        <legend>COD Info</legend>
        <table width="100%">
            <tr>
                <td width="126px">Order Ref. No.
                </td>
                <td width="450px">
                    <asp:TextBox ID="txt_OrderRefNo" runat="server" MaxLength="20" Enabled="true" Width="397px">
                    </asp:TextBox>
                </td>
                <td style="display: none;">Product Type
                </td>
                <td style="display: none;">
                    <asp:DropDownList ID="dd_ProductType" Enabled="false" runat="server" skin="Metro"
                        AppendDataBoundItems="true">
                        <asp:ListItem Value="0">Select Product</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td style="display: none;">Charge COD Amount.
                </td>
                <td style="display: none;">
                    <asp:CheckBox ID="Cb_CODAmount" runat="server" AutoPostBack="false" Enabled="true" />
                </td>
            </tr>
            <tr>
                <td>Description
                </td>
                <td>
                    <asp:TextBox ID="txt_Description" runat="server" MaxLength="203" AutoPostBack="false"
                        Enabled="true" Width="400px" TextMode="MultiLine">
                    </asp:TextBox>
                </td>
                <td width="126px">COD Amount
                </td>
                <td>
                    <asp:TextBox ID="txt_CodAmount" runat="server" onkeypress="return isNumberKey(event);"
                        onkeyup="word.innerHTML=convertNumberToWords(this.value)"
                        Enabled="true" MaxLength="5" value="0">
                    </asp:TextBox>
                    <div id="word"></div>
                    <script>
                        <%--function amountlimit() {
                            if (document.getElementById('<%= txt_CodAmount.ClientID %>') > 40000) {
                                alert("You have enter exceed amount. Max Amount is 40,000 ");
                            }
                        } onpropertychange="amountlimit()"--%>

                        //$("#txt_CodAmount").change(function () {
                        //    debugger;
                        //    if ($("#txt_CodAmount").text > 40000) {
                        //        alert("You have enter exceed amount. Max Amount is 40,000 ");
                        //    }
                        //});


                        function convertNumberToWords(amount) {
                            var words = new Array();
                            words[0] = '';
                            words[1] = 'One';
                            words[2] = 'Two';
                            words[3] = 'Three';
                            words[4] = 'Four';
                            words[5] = 'Five';
                            words[6] = 'Six';
                            words[7] = 'Seven';
                            words[8] = 'Eight';
                            words[9] = 'Nine';
                            words[10] = 'Ten';
                            words[11] = 'Eleven';
                            words[12] = 'Twelve';
                            words[13] = 'Thirteen';
                            words[14] = 'Fourteen';
                            words[15] = 'Fifteen';
                            words[16] = 'Sixteen';
                            words[17] = 'Seventeen';
                            words[18] = 'Eighteen';
                            words[19] = 'Nineteen';
                            words[20] = 'Twenty';
                            words[30] = 'Thirty';
                            words[40] = 'Forty';
                            words[50] = 'Fifty';
                            words[60] = 'Sixty';
                            words[70] = 'Seventy';
                            words[80] = 'Eighty';
                            words[90] = 'Ninety';
                            amount = amount.toString();
                            var atemp = amount.split(".");
                            var number = atemp[0].split(",").join("");
                            var n_length = number.length;
                            var words_string = "";
                            if (n_length <= 9) {
                                var n_array = new Array(0, 0, 0, 0, 0, 0, 0, 0, 0);
                                var received_n_array = new Array();
                                for (var i = 0; i < n_length; i++) {
                                    received_n_array[i] = number.substr(i, 1);
                                }
                                for (var i = 9 - n_length, j = 0; i < 9; i++, j++) {
                                    n_array[i] = received_n_array[j];
                                }
                                for (var i = 0, j = 1; i < 9; i++, j++) {
                                    if (i == 0 || i == 2 || i == 4 || i == 7) {
                                        if (n_array[i] == 1) {
                                            n_array[j] = 10 + parseInt(n_array[j]);
                                            n_array[i] = 0;
                                        }
                                    }
                                }
                                value = "";
                                for (var i = 0; i < 9; i++) {
                                    if (i == 0 || i == 2 || i == 4 || i == 7) {
                                        value = n_array[i] * 10;
                                    } else {
                                        value = n_array[i];
                                    }
                                    if (value != 0) {
                                        words_string += words[value] + " ";
                                    }
                                    if ((i == 1 && value != 0) || (i == 0 && value != 0 && n_array[i + 1] == 0)) {
                                        words_string += "Crores ";
                                    }
                                    if ((i == 3 && value != 0) || (i == 2 && value != 0 && n_array[i + 1] == 0)) {
                                        words_string += "Lakhs ";
                                    }
                                    if ((i == 5 && value != 0) || (i == 4 && value != 0 && n_array[i + 1] == 0)) {
                                        words_string += "Thousand ";
                                    }
                                    if (i == 6 && value != 0 && (n_array[i + 1] != 0 && n_array[i + 2] != 0)) {
                                        words_string += "Hundred and ";
                                    } else if (i == 6 && value != 0) {
                                        words_string += "Hundred ";
                                    }
                                }
                                words_string = words_string.split("  ").join(" ");
                            }
                            return words_string;
                        }
                    </script>
                </td>
                <td></td>
                <td></td>
            </tr>
        </table>
    </fieldset>
    <fieldset>
        <legend>Price Modifiers</legend>
        <table class="style2" style="width: 85%;">
            <tr>
                <td style="width: 150px;">
                    <div style="display: inline; text-align: left;">
                        <b>Name</b>
                    </div>
                    <div>
                        <asp:DropDownList ID="cb_PriceModifier" runat="server" AppendDataBoundItems="true"
                            AutoPostBack="True" OnSelectedIndexChanged="cb_PriceModifier_SelectedIndexChanged"
                            CausesValidation="false">
                        </asp:DropDownList>
                        <asp:HiddenField ID="hd_priceModifier" runat="server" />
                        <asp:HiddenField ID="hd_TotalAmount" runat="server" />
                    </div>
                </td>
                <td style="width: 10px;">&nbsp;
                </td>
                <td style="width: 220px;">
                    <div style="display: inline; text-align: left;">
                        <b>Calculation Base</b>
                    </div>
                    <div>
                        <asp:RadioButtonList ID="rb_Calculation" runat="server" RepeatDirection="Horizontal"
                            Enabled="false">
                            <asp:ListItem Text="Flat" Selected="True" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Percentage" Value="2"></asp:ListItem>
                            <asp:ListItem Text="Insurance" Value="3"></asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                </td>
                <td style="width: 10px;">&nbsp;
                </td>
                <td style="width: 150px;">
                    <div style="display: inline; text-align: left;">
                        <b>Pieces</b>
                    </div>
                    <div>
                        <asp:TextBox ID="txt_pricequantity" runat="server" MaxLength="7" CssClass="med-field-right"
                            Enabled="false">
                        </asp:TextBox>
                    </div>
                </td>
                <td style="width: 10px;">&nbsp;
                </td>
                <td style="width: 150px;">
                    <div style="display: inline; text-align: left;">
                        <b>Value</b>
                    </div>
                    <div>
                        <asp:TextBox ID="txt_Value" runat="server" MaxLength="7" CssClass="med-field-right"
                            Enabled="false">
                        </asp:TextBox>
                    </div>
                </td>
                <td style="width: 10px;">&nbsp;
                </td>
                <td style="width: 150px;">
                    <div style="display: inline; text-align: left;">
                        <b>Declared Value(Insurance)</b>
                    </div>
                    <div>
                        <asp:TextBox ID="txt_insuranceDeclaredValue" onkeypress="return isNumberKey(event);"
                            runat="server" MaxLength="7" CssClass="med-field-right" Enabled="false">
                        </asp:TextBox>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="6" style="width: 550px;">
                    <div style="display: inline; text-align: left;">
                        <b>Description</b>
                    </div>
                    <div>
                        <asp:TextBox ID="txt_Description2" runat="server" Width="98%">
                        </asp:TextBox>
                    </div>
                </td>
                <td align="center" style="width: 150px;">
                    <asp:Button ID="rd_AddPriceModifier" runat="server" Text="Add" autopostback="true"
                        OnClick="rd_AddPriceModifier_Click" CausesValidation="false" UseSubmitBehavior="false"></asp:Button>
                </td>
            </tr>
            <tr>
                <td colspan="7">
                    <asp:Repeater ID="RadGrid1" runat="server" OnItemCommand="RadGrid1_ItemCommand" OnItemDataBound="RadGrid1_ItemDataBound">
                        <HeaderTemplate>
                            <table width="100%" class="repeater_tbl" cellpadding="0" cellspacing="0">
                                <tr>
                                    <th style="text-align: left; width: 23%;">Price Modifier
                                    </th>
                                    <th style="text-align: left; width: 10%;">Pieces
                                    </th>
                                    <th style="text-align: left; width: 10%;">Modifier Value
                                    </th>
                                    <th style="text-align: left; width: 10%;">Value
                                    </th>
                                    <th style="text-align: left; width: 13%;">Calculation Base
                                    </th>
                                    <th style="text-align: left; width: 23%;">Description
                                    </th>
                                    <th>Declared Value
                                    </th>
                                    <th></th>
                                    <th></th>
                                </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_PriceModifier" runat="server"></asp:Label>
                                    <asp:HiddenField ID="Hd_ID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "priceModifierId")%>' />
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_PricemodifierPieces" runat="server" onkeypress="return isNumberKey(event);"
                                        onkeyup="calc()" Text='<%# DataBinder.Eval(Container.DataItem, "Pieces")%>' Style="width: 55%; text-align: center;"
                                        OnTextChanged="PricemodifierPieces_TextChanged" AutoPostBack="true"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ForeColor="Red"
                                        Font-Size="Small" ErrorMessage="Enter only number" ValidationExpression="^([\S\s]{0,4})$"
                                        ControlToValidate="txt_PricemodifierPieces" runat="server" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_CalPriceModifierValue" runat="server" onkeypress="return isNumberKey(event);"
                                        Text='<%# DataBinder.Eval(Container.DataItem, "ModifiedCalculationValue")%>'
                                        Style="width: 55%; text-align: center;"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" ForeColor="Red"
                                        Font-Size="Small" ErrorMessage="Enter only number" ValidationExpression="^([\S\s]{0,4})$"
                                        ControlToValidate="txt_CalPriceModifierValue" runat="server" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_Value" runat="server" onkeypress="return isNumberKey(event);"
                                        Text='<%# DataBinder.Eval(Container.DataItem, "CalculatedValue")%>' Style="width: 55%; text-align: center;"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" ForeColor="Red"
                                        Font-Size="Small" ErrorMessage="Enter only number" ValidationExpression="^([\S\s]{0,4})$"
                                        ControlToValidate="txt_Value" runat="server" />
                                </td>
                                <%--<td>
                                    <asp:TextBox ID="txt_Value" runat="server" onkeypress="return isNumberKey(event);"
                                        Text='<%# DataBinder.Eval(Container.DataItem, "ModifiedCalculationValue")%>'
                                        Style="width: 55%; text-align: center;"></asp:TextBox>
                                    <asp:RegularExpressionValidator ForeColor="Red" Font-Size="Small" ErrorMessage="Enter only number"
                                        ValidationExpression="^([\S\s]{0,4})$" ControlToValidate="txt_Value" runat="server" />                                    
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_CalPriceModifierValue" runat="server" onkeypress="return isNumberKey(event);"
                                        Text='<%# DataBinder.Eval(Container.DataItem, "CalculatedValue")%>' Style="width: 55%; text-align: center;"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" ForeColor="Red"
                                        Font-Size="Small" ErrorMessage="Enter only number" ValidationExpression="^([\S\s]{0,4})$"
                                        ControlToValidate="txt_CalPriceModifierValue" runat="server" />
                                </td>--%>
                                <td>
                                    <asp:HiddenField ID="hd_base" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "CalculationBase")%>' />
                                    <asp:HiddenField ID="hd_gstExempt" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "isGST") %>' />
                                    <%# DataBinder.Eval(Container.DataItem, "CalculationBase")%>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_Description" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_gDeclaredValue" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "AlternateValue") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:ImageButton ID="Edit_Button" runat="server" ToolTip="Edit Modifier" ImageUrl="~/Images/edit_IMage.png"
                                        CausesValidation="false" Width="20px" Height="20px" CommandName="Edit" />
                                </td>
                                <td>
                                    <asp:ImageButton ID="Delete_Image" runat="server" ToolTip="Delete Modifier" ImageUrl="~/Images/1461581832_delete.png"
                                        CausesValidation="false" Width="20px" Height="20px" CommandName="Delete" />
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </td>
            </tr>
        </table>
    </fieldset>
    <table width="100%">
        <tr>
            <td align="right">
                <asp:Panel ID="pl_1" runat="server">
                    <%--<telerik:RadButton ID="Radbutton1" runat="server" Text="Print" Skin="Metro" OnClick="btn_PrintConsignment_Click"
                        CssClass="button" AutoPostBack="true" UseSubmitBehavior="false" CausesValidation="false">
                    </telerik:RadButton>--%>
                    <asp:Button ID="btn_SaveConsignment1" runat="server" Text="Validate" CssClass="button"
                        UseSubmitBehavior="false" OnClick="btn_SaveConsignment1_Click"></asp:Button>
                    &nbsp;
                    <asp:Button ID="btn_SaveConsignment0" runat="server" CssClass="button" OnClick="btn_SaveConsignment_Click"
                        OnClientClick="disabilityCheck()" Text="Save" UseSubmitBehavior="false" Enabled="false" />
                    &nbsp;
                    <asp:Button ID="btn_reset" runat="server" Text="Reset" CssClass="button" UseSubmitBehavior="false"
                        CausesValidation="false" OnClick="btn_reset_Click"></asp:Button>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="hd_amount" runat="server" />
    <asp:HiddenField ID="hd_gst" runat="server" />
    <asp:HiddenField ID="hd_chargeamount" runat="server" />
    <script language="javascript">
        PaymentModeChange();
    </script>
</asp:Content>
