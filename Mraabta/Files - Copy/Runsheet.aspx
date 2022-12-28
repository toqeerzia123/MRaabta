<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="Runsheet.aspx.cs" Inherits="MRaabta.Files.Runsheet" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        .prt_lbl label {
            float: left;
        }

        .input-field.prt_lbl > input {
            float: left;
            width: 5%;
        }
    </style>
    <style>
        .search {
            float: right;
            width: 10%;
            background: #5f5a8d;
            padding: 3px;
            position: relative;
            right: 68px;
            margin: 0px 0px 15px;
            top: 7px;
            text-align: center;
        }

            .search a {
                color: #fff;
                text-decoration: none;
            }

        .tbl-large > div {
            position: static;
        }
    </style>
    <script type="text/javascript">
        function loader() {
            document.getElementById('<%=loader.ClientID %>').style.display = "";
        }
        function validate() {

            var text = document.getElementById("<%=txt_cnNumber.ClientID %>").value;


            if (text.length < 11) {
                alert("CN Number must be at least 11 characters");
                return false;
                document.getElementById("<%=txt_cnNumber.ClientID %>").focus();


            }
            else {

                return true;
            }

        }
        function isNumberKey(evt) {

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

        function checkValidations(txt) {


            //            if (txt.value.length > 20 || txt.value.length < 11 || true) {
            //                alert('Consignment Number must be between 11 and 20 digits');
            //                txt.value = "";
            //                txt.focus();
            //                return false;
            //            }


            return true;
        }

        function ChangeFormMode() {
            debugger;
            var mode = document.getElementById('<%= rbtn_formMode.ClientID %>');
            var txt_runsheet = document.getElementById('<%= txt_runsheetNumber.ClientID %>');
            var rDate = document.getElementById('<%= div_date.ClientID %>');
            var dDate = document.getElementById('<%= div_dateDisplay.ClientID %>');
            txt_runsheet.value = "";
            var rType = document.getElementById('<%= dd_runsheetType.ClientID %>');
            var txt_routeCode = document.getElementById('<%= txt_routeCode.ClientID %>');
            var dd_route = document.getElementById('<%= dd_route.ClientID %>');
            var dd_docType = document.getElementById('<%= dd_docType.ClientID %>');
            var txt_count = document.getElementById('<%= lbl_count.ClientID %>');
            var dd_rider = document.getElementById('<%= dd_riders.ClientID %>');
            var inputs = mode.getElementsByTagName('input');
            var selectedValue = "";
            var txt_rider = document.getElementById('<%= txt_riderno.ClientID %>');
            for (var i = 0; i < inputs.length; i++) {
                if (inputs[i].checked) {
                    selectedValue = inputs[i].value;
                }
            }

            debugger;
            var hd_modeChanged = document.getElementById('<%= hd_modeChanged.ClientID %>');
            var currentMode = document.getElementById('<%= hd_currentMode.ClientID %>');

            hd_modeChanged.value = '1';
            var grid = document.getElementById('<%= gv_consignments.ClientID %>');
            if (grid != null) {


                for (var i = 1; i < grid.rows.length; i++) {
                    grid.deleteRow(grid.rows[i]);
                    i--;
                }
                grid.deleteRow(0);
                txt_count.value = "Count: 0";
            }
            if (selectedValue == "0") {
                txt_runsheet.disabled = true;
                rType.disabled = false;
                txt_routeCode.disabled = false;
                dd_route.disabled = false;
                dd_docType.disabled = false;
                rDate.style.display = 'block';
                dDate.style.display = 'none';

                txt_runsheet.value = "";
                txt_routeCode.value = "";
                txt_rider.value = "";
                dd_route.options[0].selected = true;
                rType.options[0].selected = true;
                dd_rider.options[0].selected = true;
            }
            else {
                txt_runsheet.disabled = false;
                rType.disabled = true;
                txt_routeCode.disabled = true;
                dd_route.disabled = true;
                dd_docType.disabled = true;

                rDate.style.display = 'none';
                dDate.style.display = 'block';

                txt_runsheet.value = "";
                txt_routeCode.value = "";
                txt_rider.value = "";
                dd_route.options[0].selected = true;
                rType.options[0].selected = true;
                dd_rider.options[0].selected = true;
            }

        }

        function FindVehicle(txt) {
            debugger;
            var dd_vehicle = document.getElementById('<%= dd_vehicle.ClientID %>');
            var txtValue = txt.value.replace(' ', '');
            txtValue = txtValue.replace('-', '');
            var found = false;
            for (var i = 0; i < dd_vehicle.options.length; i++) {
                var currentValue = dd_vehicle.options[i].text;
                if (txtValue.trim().toUpperCase() == currentValue.trim().toUpperCase() || txtValue.trim().replace('-', '').toUpperCase() == currentValue.trim().replace('-', '').toUpperCase()) {
                    dd_vehicle.options[i].selected = true;
                    txt.value = currentValue;
                    found = true;
                    break;
                }
            }
            if (found) {
                CheckVehicleType(dd_vehicle);
            }
            else {
                dd_vehicle.options[0].selected = true;
                var vehicleType = document.getElementById('<%= dd_vehicleType.ClientID %>');
                vehicleType.options[0].selected = true;
            }

        }
        function SetVehicle(dd) {
            var txt = document.getElementById('<%= txt_vehicleNumber.ClientID %>');
            if (dd.options[dd.options.selectedIndex].value == "0") {
                alert('Select Vehicle');
                txt.value = "";
                var vehicleType = document.getElementById('<%= dd_vehicleType.ClientID %>');
                vehicleType.options[0].selected = true;

            }
            else {
                txt.value = dd.options[dd.options.selectedIndex].text;
                CheckVehicleType(dd);
            }

        }
        function CheckVehicleType(dd) {
            debugger;
            var vehicleGrid = document.getElementById('<%= vehicleTypes.ClientID %>');
            var vehicleType = document.getElementById('<%= dd_vehicleType.ClientID %>');
            var selectedVehicle = dd.options[dd.options.selectedIndex].value;

            for (var i = 0; i < vehicleGrid.rows.length; i++) {
                if (selectedVehicle == vehicleGrid.rows[i].cells[0].innerText) {
                    var selectedVehicleType = vehicleGrid.rows[i].cells[1].innerText;
                    for (var j = 0; j < vehicleType.options.length; j++) {
                        if (vehicleType.options[j].value == selectedVehicleType) {
                            vehicleType.options[j].selected = true;
                            break;
                        }
                    }
                }
            }

        }


    </script>
    <div runat="server" id="loader" style="background-color: rgb(238, 238, 238); float: left; height: 585px; opacity: 0.7; position: absolute; text-align: center; display: none; top: 11%; width: 84% !important;">
        <div class="loader">
        </div>
    </div>
    <div class="search">
        <a href="SearchRunSheet.aspx">Search RunSheet</a>
    </div>
    <asp:Label ID="Errorid" runat="server" Font-Bold="true"></asp:Label>
    <table style="font-family: Calibri; font-size: medium; padding-bottom: 0px !important; padding-top: 0px !important"
        class="input-form">
        <tr style="padding: 0px 0px 0px 0px !important; text-align: center;">
            <td colspan="8" style="padding-bottom: 1px !important; padding-top: 1px !important; width: 1% !important;">
                <h3 style="font-family: Calibri; margin: 0px !important; font-variant: small-caps;">Runsheet Info.</h3>
            </td>
        </tr>
        <tr style="padding: 0px 0px 0px 0px !important;">
            <td colspan="8" style="padding-bottom: 1px !important; padding-top: 1px !important;">
                <div>
                    <b>Form Mode</b>
                </div>
                <asp:RadioButtonList ID="rbtn_formMode" runat="server" RepeatColumns="2" RepeatDirection="Horizontal"
                    onchange="ChangeFormMode()" OnSelectedIndexChanged="rbtn_formMode_SelectedIndexChanged" AutoPostBack="true">
                    <asp:ListItem Value="0" Selected="True">NEW</asp:ListItem>
                    <asp:ListItem Value="1">EDIT</asp:ListItem>
                </asp:RadioButtonList>
                <asp:HiddenField ID="hd_modeChanged" runat="server" Value="0" />
                <asp:HiddenField ID="hd_currentMode" runat="server" Value="0" />
            </td>
        </tr>
        <tr>
            <td class="field" style="width: 10% !important;">Runsheet#.
            </td>
            <td class="input-field" style="width: 15% !important;">
                <asp:TextBox ID="txt_runsheetNumber" runat="server" AutoPostBack="true"
                    Enabled="false" OnTextChanged="txt_runsheetNumber_TextChanged"></asp:TextBox>
            </td>
            <td class="space" style="margin: 0px 0px 0px 0px !important; width: 5% !important;"></td>
            <td class="field" style="width: 10% !important;">Date
            </td>
            <td class="input-field" style="width: 15% !important;">
                <div id="div_date" runat="server" style="width: 100%; display: block;">
                    <telerik:RadDatePicker ID="picker1" runat="server" DateInput-DateFormat="yyyy-MM-dd"
                        DateInput-EmptyMessage="Select Date" Enabled="true">
                    </telerik:RadDatePicker>
                </div>
                <div id="div_dateDisplay" runat="server" style="width: 100%; display: none;">
                    <%--<asp:Label ID="lbl_date" runat="server"></asp:Label>--%>
                    <asp:TextBox ID="txt_date" runat="server" Enabled="false" Width="90%"></asp:TextBox>
                </div>
            </td>
            <td class="space" style="margin: 0px 0px 0px 0px !important; width: 5% !important;"></td>
            <td class="field" style="width: 10% !important;">Runsheet Type
            </td>
            <td class="input-field" style="width: 15% !important;">
                <asp:DropDownList ID="dd_runsheetType" runat="server" AppendDataBoundItems="true"
                    Width="100%">
                    <asp:ListItem Value="0">Select Type</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="field" style="width: 10% !important;">Route Code.
            </td>
            <td class="input-field" style="width: 15% !important;">
                <asp:TextBox ID="txt_routeCode" runat="server" AutoPostBack="true" OnTextChanged="txt_routeCode_TextChanged"></asp:TextBox>
            </td>
            <td class="space" style="margin: 0px 0px 0px 0px !important; width: 5% !important;"></td>
            <td class="field" style="width: 10% !important;">Route
            </td>
            <td class="input-field" style="width: 15% !important;">
                <asp:DropDownList ID="dd_route" runat="server" AppendDataBoundItems="true" Width="100%"
                    OnSelectedIndexChanged="dd_route_SelectedIndexChanged" AutoPostBack="true">
                    <asp:ListItem Value="0">Select Route</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td class="space" style="margin: 0px 0px 0px 0px !important; width: 5% !important;"></td>
            <td class="field" style="width: 10% !important;">Document Type
            </td>
            <td class="input-field" style="width: 15% !important;">
                <asp:DropDownList ID="dd_docType" runat="server" AppendDataBoundItems="true" Width="100%"
                    Enabled="false">
                    <asp:ListItem Value="0">Select Doc Type</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="field" style="width: 10% !important;">Rider No.
            </td>
            <td class="input-field" style="width: 15% !important;">
                <asp:TextBox ID="txt_riderno" runat="server" AutoPostBack="true" OnTextChanged="txt_riderno_TextChanged"
                    Enabled="false"></asp:TextBox>
            </td>
            <td class="space" style="margin: 0px 0px 0px 0px !important; width: 5% !important;"></td>
            <td class="field" style="width: 10% !important;">Rider Name
            </td>
            <td class="input-field" style="width: 15% !important;">
                <asp:DropDownList ID="dd_riders" runat="server" AppendDataBoundItems="true" AutoPostBack="true"
                    Enabled="false" Width="100%" OnSelectedIndexChanged="dd_riders_SelectedIndexChanged">
                    <asp:ListItem Value="0">Select Rider</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td class="space" style="margin: 0px 0px 0px 0px !important; width: 5% !important;"></td>
            <td class="field" style="width: 10% !important;"></td>
            <td class="input-field prt_lbl" style="width: 15% !important; text-align: right !important;">
                <asp:CheckBox ID="chk_prePrinted" runat="server" AutoPostBack="true" Text="Pre Printed"
                    TextAlign="Right" />
            </td>
        </tr>
        <tr>
            <td class="field" style="width: 10% !important;">Vehicle
            </td>
            <td class="input-field" style="width: 15% !important;">
                <asp:TextBox ID="txt_vehicleNumber" runat="server" Width="100%" onchange="FindVehicle(this)"></asp:TextBox>

            </td>
            <td class="space" style="margin: 0px 0px 0px 0px !important; width: 5% !important;"></td>
            <td class="field" style="width: 10% !important;"></td>
            <td class="input-field" style="width: 15% !important;">
                <asp:DropDownList ID="dd_vehicle" runat="server" Width="100%" CssClass="dropdown"
                    onchange="SetVehicle(this)" AppendDataBoundItems="true">
                    <asp:ListItem Value="0">Select Vehicle</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td class="space" style="margin: 0px 0px 0px 0px !important; width: 5% !important;"></td>
            <td class="field" style="width: 10% !important;">Vehicle Type
            </td>
            <td class="input-field" style="width: 12% !important;">
                <asp:DropDownList ID="dd_vehicleType" runat="server" CssClass="dropdown" AppendDataBoundItems="true">
                    <asp:ListItem Value="0">Select Vehicle Type</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;"></td>


        </tr>
        <tr>
            <td class="field" style="width: 10% !important;">Meter Start
            </td>
            <td class="input-field" style="width: 15% !important;">
                <asp:TextBox ID="txt_meterStart" runat="server" Width="100%" onkeypress="return isNumberKey(event);"></asp:TextBox>

            </td>
            <td class="space" style="margin: 0px 0px 0px 0px !important; width: 5% !important;"></td>
            <td class="field" style="width: 10% !important;">Meter End
            </td>
            <td class="input-field" style="width: 15% !important;">
                <asp:TextBox ID="txt_meterEnd" runat="server" onkeypress="return isNumberKey(event);"></asp:TextBox>
            </td>
            <td class="space" style="margin: 0px 0px 0px 0px !important; width: 5% !important;"></td>

        </tr>
    </table>
    <fieldset style="border-radius: 5px !important; font-weight: bold; font-size: medium; padding: 10px !important; margin: 18px !important; width: 95%;">
        <legend>Consignments</legend>
        <table style="width: 100%">
            <tr>
                <td style="width: 13%; font-weight: bold;">CN Number
                </td>
                <td style="width: 20%;">
                    <asp:TextBox ID="txt_cnNumber" runat="server" AutoPostBack="true" onkeypress="return isNumberKey(event);"
                        OnTextChanged="txt_cnNumber_TextChanged" MaxLength="20" onchange="if ( checkValidations(this) == false ) return;"></asp:TextBox>
                    <%--Onchange="return validate();"--%>
                </td>
                <td></td>
            </tr>

        </table>
        <asp:UpdatePanel ID="up1" runat="server">
            <ContentTemplate>
                <div style="width: 100%; height: 250px; overflow: scroll; text-align: center;">
                    <span id="Table_1" class="tbl-large" style="width: 100%;">
                        <asp:Label ID="lbl_count" runat="server"></asp:Label>
                        <asp:Label ID="err_msg" runat="server" CssClass="error_msg"></asp:Label>
                        <asp:GridView ID="gv_consignments" runat="server" AutoGenerateColumns="false" CssClass="mGrid"
                            AlternatingRowStyle-CssClass="alt" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px"
                            Width="95%" EmptyDataText="No Data Available" OnRowCommand="gv_consignments_RowCommand"
                            OnRowDataBound="gv_consignments_RowDataBound">
                            <RowStyle Font-Bold="false" />
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Button ID="btn_remove" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ConNo") %>'
                                            CssClass="button" CommandName="Remove" Text="Remove" />
                                        <asp:HiddenField ID="hd_new" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "IsNew") %>' />
                                        <asp:HiddenField ID="hd_removeable" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "removeable") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="ConsignmentNumber" DataField="ConNo" />
                                <%--<asp:BoundField HeaderText="Description" DataField="Description" />--%>
                                <asp:TemplateField HeaderText="Origin">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="dd_gOrigin" runat="server" AppendDataBoundItems="true" OnSelectedIndexChanged="dd_gOrigin_SelectedIndexChanged"
                                            AutoPostBack="true">
                                            <asp:ListItem Value="0">Select Origin</asp:ListItem>
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Destination">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="dd_gDestination" runat="server" AppendDataBoundItems="true">
                                            <asp:ListItem Value="0">Select Destination</asp:ListItem>
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--<asp:BoundField HeaderText="Origin" DataField="ORIGIN" />--%>
                                <%--<asp:BoundField HeaderText="Destination" DataField="NAME" />--%>
                                <asp:BoundField HeaderText="Con Type" DataField="ConType" />
                                <%--<asp:TemplateField>
                            <ItemTemplate>
                                <asp:HiddenField ID="hd_origin" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "ORGIN") %>' />
                                <asp:HiddenField ID="hd_destination" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "Destination") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                            </Columns>
                        </asp:GridView>
                    </span>
                    <asp:Button ID="btn_applyDefault" runat="server" Text="Apply Default Tariff" CssClass="button"
                        Visible="false" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </fieldset>
    <div style="width: 100%; text-align: center">
        <asp:Button ID="btn_reset" runat="server" Text="Reset" CssClass="button" OnClick="btn_reset_Click"
            UseSubmitBehavior="false" />
        &nbsp;
        <asp:Button ID="btn_save" runat="server" Text="Save" CssClass="button" OnClick="btn_save_Click"
            OnClientClick="loader()" UseSubmitBehavior="false" />
        &nbsp;
    </div>

    <div style="display: none">
        <asp:GridView ID="vehicleTypes" runat="server" AutoGenerateColumns="false">
            <Columns>
                <asp:BoundField HeaderText="VehicleCode" DataField="VehicleCode" />
                <asp:BoundField HeaderText="TypeID" DataField="VehicleType_" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
