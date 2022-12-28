<%@ Page Title="" Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.master" CodeBehind="MasterAirportLoadingSpeedy_New.aspx.cs" Inherits="MRaabta.Files.MasterAirportLoadingSpeedy_New" %>

<%@ Register TagPrefix="Ajax1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script src="../Scripts/jquery-3.5.1.min.js"></script>
    <style>
        #ContentPlaceHolder1_CalendarExtender2_daysBody td {
            padding: 0 !important;
        }

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
    </style>
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
    <script>

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
        function isNumberKeyDecimal(evt, txt) {
            var count = 0;
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode != 46 && charCode > 31
                && (charCode < 48 || charCode > 57 || charCode == 110 || charCode == 109 || charCode == 45 || charCode == 9)) {
                return false;
            }
            else {

                if (charCode == 110 || charCode == 46) {
                    count++;
                    if (txt.value.includes('.')) {
                        return false;
                    }

                }
                if (count > 1) {
                    return false
                }
            }

            if (txt.value.includes('.')) {
                txt.maxLength = 5;
            }
            else {
                txt.maxLength = 4;
            }

            return true;
        }
        function ValidNumber(txt) {
            var num = txt.value;
            if (isNaN(num)) {
                alert('Invalid Bag Number');
                txt.value = "";
                return false;
            }
            return true;
        }

        function checkValidations(txt) {


            var txt_cn = document.getElementById('<%= txt_consignmentno.ClientID %>');


            var route = document.getElementById('<%= dd_route.ClientID %>');
            if (route.options[route.options.selectedIndex].value == "") {
                alert('Select Route');
                txt.value = "";
                route.focus();
                return false;
            }

            var transporttype = document.getElementById('<%= dd_transporttype.ClientID %>');
            if (transporttype.options[transporttype.options.selectedIndex].value == "") {
                alert('Select Transport Type');
                txt.value = "";
                transporttype.focus();
                return false;
            }


            var controlGrid = document.getElementById('<%= cnControls.ClientID %>');
            var prefixNotFound = false;

            var message = "";

            for (var i = 1; i < controlGrid.rows.length; i++) {
                var row = controlGrid.rows[i];
                var prefix = row.cells[0].innerText;
                var length_ = parseInt(row.cells[1].innerText);

                if (txt_cn.value.substring(0, prefix.length) == prefix) {
                    if (txt_cn.value.length != length_) {
                        message = "Invalid Length of CN";

                        prefixNotFound = true;
                    }
                    else {
                        prefixNotFound = false;
                        break;
                    }
                }
                else {
                    if (message == "") {
                        message = "Invalid Prefix";
                    }

                    prefixNotFound = true;

                }
            }
            if (prefixNotFound) {
                alert(message);
                txt_cn.value = "";
                txt_cn.focus();
                return false;
            }

            return true;
        }

        function read() {
        }

        function Show_Hide_By_Display() {

            var field = document.getElementById('<%= hd.ClientID %>');
            if (field.value == "1") {
                alert("Please wait while Loading is being Processed");
                return false;
            }
            field.value = "1";
            return true;
        }

        function CheckVehicleType(dd) {

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
    <style>
        .headerRow td {
            font-family: Calibri;
            font-size: small;
            font-weight: bold;
            background-color: #cccccc;
        }

        .DetailRow td {
            font-family: Calibri;
            font-size: small;
            font-weight: normal;
        }

        .tblDetails {
            font-family: calibri;
            font-size: small;
            border: 1px solid black;
            width: 90%;
            margin-left: 5%;
            border-radius: 5px !important;
            padding: 5px;
        }

            .tblDetails tr:nth-child(odd) {
                background-color: #ededed;
            }

        .button1 {
            padding-left: 5px !important;
            padding-right: 5px !important;
        }

        .textBox {
            border-color: #adadad !important;
        }
    </style>
    <body>
        <asp:HiddenField ID="hd" runat="server" Value="0" />
        <asp:HiddenField ID="hd_IDChk" runat="server" Value="0" />
        <asp:HiddenField ID="hd_branchCode" runat="server" />
        <asp:HiddenField ID="hd_zoneCode" runat="server" />
        <asp:HiddenField ID="hd_expressCenterCode" runat="server" />
        <asp:HiddenField ID="hd_U_ID" runat="server" />
        <asp:HiddenField ID="hd_LocationName" runat="server" />
        <asp:HiddenField ID="hd_LocationID" runat="server" />


        <div id="div2" class="outer_box" style="display: none; background: #FFFFFF none repeat scroll 0 0;">
            <img src="../images/Loading_Movie-02.gif" />
        </div>
        <div>
            <table cellpadding="0" cellspacing="0" width='100%' class='mGrid_Table'>
                <tr>
                    <td colspan="12" align="center" class="head_column">
                        <h3>Master Airport Loading Material
                        </h3>
                    </td>
                </tr>
            </table>
            <div>
                <asp:Label ID="hd_loadingID" runat="server" ForeColor="Transparent" />
            </div>
            <div class="search">
                <a href="SearchLoadingNew.aspx">Search Loading</a>
            </div>
            <table class="input-form" style="width: 95%;">
                <tr>
                    <td style="text-align: right">
                        <div>
                            <asp:RadioButtonList ID="rbtn_mode" runat="server" RepeatDirection="Horizontal"
                                RepeatColumns="2" onclick="ModeChange(this);">
                                <asp:ListItem Value="NEW" Selected="True">New</asp:ListItem>
                                <asp:ListItem Value="UPDATE">Update</asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="field">Route:
                    </td>
                    <td class="input-field">
                        <asp:DropDownList ID="dd_route" runat="server" CssClass="dropdown" onchange="RouteChange(this);">
                        </asp:DropDownList>
                    </td>
                    <td class="space"></td>
                    <td class="field">Date:
                    </td>
                    <td class="input-field">
                        <asp:TextBox ID="dd_start_date" runat="server" CssClass="med-field" MaxLength="10"></asp:TextBox>
                        <Ajax1:CalendarExtender ID="CalendarExtender1" TargetControlID="dd_start_date" runat="server"
                            Format="yyyy-MM-dd" PopupButtonID="Image1">
                        </Ajax1:CalendarExtender>
                    </td>
                </tr>
                <tr>
                    <td class="field">Touch Point:
                    </td>
                    <td class="input-field">
                        <asp:DropDownList ID="dd_touchpoint" runat="server" CssClass="dropdown">
                        </asp:DropDownList>
                    </td>
                    <td class="space"></td>
                    <td class="field">VId:
                    </td>
                    <td class="input-field">
                        <asp:TextBox ID="txt_vid" runat="server" CssClass="med-field" MaxLength="15" AutoPostBack="false"
                            onchange="VidChange(this);"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="field">Transport type:
                    </td>
                    <td class="input-field">
                        <asp:DropDownList ID="dd_transporttype" runat="server" CssClass="dropdown" AutoPostBack="false"
                            onchange="TransportTypeChange(this)">
                        </asp:DropDownList>
                    </td>
                    <td class="space"></td>
                    <td class="field">Origin:
                    </td>
                    <td class="input-field">
                        <asp:DropDownList ID="dd_orign" runat="server" CssClass="dropdown" disabled="true">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr id="flight" style="display: none;">
                    <td class="field">Flight Number:
                    </td>
                    <td class="input-field">
                        <asp:TextBox ID="txt_flight" runat="server"></asp:TextBox>
                    </td>
                    <td class="space"></td>
                    <td class="field">Departure Flight Date
                    </td>
                    <td class="input-field">
                        <asp:TextBox ID="dept_date" runat="server" CssClass="med-field" MaxLength="10"></asp:TextBox>
                        <%--<Ajax1:CalendarExtender ID="CalendarExtender2" TargetControlID="dept_date" runat="server"
                                Format="yyyy-MM-dd" PopupButtonID="Image1">
                            </Ajax1:CalendarExtender>  --%>
                        <Ajax1:MaskedEditExtender ID="MaskedEditExtender1" TargetControlID="dept_date" Mask="99:99"
                            ClearMaskOnLostFocus="false" MaskType="Time" CultureName="en-us" MessageValidatorTip="true"
                            runat="server">
                        </Ajax1:MaskedEditExtender>
                    </td>
                </tr>
                <tr>
                    <td class="field">Vehicle Mode:
                    </td>
                    <td class="input-field">
                        <div>
                            <input id="Vehicle" type="radio" name="VehicleType" onchange="VehicleModeChange(this)"
                                checked="true" />
                            Vehicle
                            <input type="radio" id="Rented" name="VehicleType" onchange="VehicleModeChange(this)" />
                            Rented
                        </div>
                    </td>
                    <td class="space"></td>
                    <td class="field">Destination:
                    </td>
                    <td class="input-field">
                        <asp:DropDownList ID="dd_destination" runat="server" CssClass="dropdown">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="field">
                        <div id="vehiclediv1" style="display: block;">
                            Vehicle No:
                        </div>
                        <div id="regdiv1" style="display: none;">
                            Reg No:
                        </div>
                    </td>
                    <td class="input-field">
                        <div id="vehiclediv2" style="display: block;">
                            <asp:DropDownList ID="dd_vehicle" runat="server" CssClass="dropdown" AppendDataBoundItems="true"
                                onchange="CheckVehicleType(this);">
                                <asp:ListItem Value="0">Select Vehicle</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div id="regdiv2" style="display: none;">
                            <asp:TextBox ID="txt_reg" runat="server"></asp:TextBox>
                        </div>
                    </td>
                    <td class="space"></td>
                    <td class="field">Vehicle Type
                    </td>
                    <td class="input-field">
                        <asp:DropDownList ID="dd_vehicleType" Width="100%" CssClass="dropdown" runat="server"
                            AppendDataBoundItems="true">
                            <asp:ListItem Value="0">Select Vehicle Type</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td class="space"></td>
                </tr>
                <tr>
                    <td class="field">Description:
                    </td>
                    <td class="input-field">
                        <asp:TextBox ID="txt_description" runat="server" Columns="8"></asp:TextBox>
                    </td>
                    <td class="space"></td>
                    <td class="field">Courier Name:
                    </td>
                    <td class="input-field">
                        <asp:TextBox ID="txt_couriername" runat="server"></asp:TextBox>
                    </td>
                    <td class="space"></td>
                </tr>
                <tr>
                    <td class="field">Loading Seal Number:
                    </td>
                    <td class="input-field">
                        <asp:TextBox ID="txt_seal" runat="server" onchange="focusToBag()" onkeydown="focusAfterSeal(event);"></asp:TextBox>
                    </td>
                    <td class="space"></td>
                    <td class="field">Total Weight
                    </td>
                    <td class="input-field">
                        <asp:TextBox ID="txt_totalLoadWeight" runat="server" Text="0" Enabled="false"></asp:TextBox>
                    </td>
                    <td class="space"></td>
                </tr>
            </table>
            <style>
                .content {
                    position: relative;
                    top: 10px;
                    left: 0;
                    background: white;
                    right: 0;
                    bottom: 0;
                    padding: 20px;
                    border: 1px solid #ccc;
                    min-height: 415px;
                    overflow: hidden;
                }


                .tab_radio {
                    left: 20px;
                    position: relative;
                    width: 70%;
                }

                    .tab_radio > span {
                        background: #eee none repeat scroll 0 0;
                        border: 1px solid #ccc;
                        left: 1px;
                        margin-left: -1px;
                        padding: 10px;
                        position: relative;
                    }

                [type=radio]:checked {
                    background: white;
                    border-bottom: 1px solid white;
                    z-index: 2;
                }

                .tabs {
                    left: 20px;
                    margin: 0 0 40px;
                    padding: 0;
                    position: relative;
                    width: 97%;
                }

                .input-form.boxbg {
                    background: #eee none repeat scroll 0 0;
                    margin: 0;
                    width: 100%;
                }

                .ajax__scroll_none{
                    overflow-y:scroll !important;
                }
            </style>
            <asp:Label ID="error_msg" runat="server" CssClass="error_msg"></asp:Label>
            <div style="font-weight: bold; width: 21%; position: relative; right: 40px; float: right; top: 25px; margin-bottom: 5px">
                <label id="lbl_count" style="font-family: Calibri; font-size: medium; font-weight: bold;">
                </label>
            </div>
            <div style="width: 100%; text-align: center; float: left;">
                <input id="btn_save_" type="button" class="button" onclick="SaveLoading()" value="Save" />&nbsp;
                &nbsp; &nbsp; &nbsp;
                <%--<input type="button" class="button" onclick="ResetAll()" value="Reset" />--%>
            </div>
            <div>
                <Ajax1:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" Width="100%" Height="300px"
                    Style="margin-left: 2% !important; margin-right: 2%">
                    <Ajax1:TabPanel runat="server" HeaderText="Manifest" ID="TabPanel1" Width="100%">
                        <HeaderTemplate>
                            Bags and Consignment Information
                        </HeaderTemplate>
                        <ContentTemplate>
                            <table style="width: 100% !important;">
                                <tr>
                                    <td valign="top" style="width: 20%; font-weight: bold; margin-left: 20px;">Bag /CN No:
                                    </td>
                                    <td valign="top" style="width: 60%">
                                        <asp:TextBox ID="txt_bagno" MaxLength="20" runat="server"
                                            AutoPostBack="false" onkeypress="return isNumberKey(event);" onchange="bag_TextChanged()"></asp:TextBox>
                                        &nbsp;
                                        <input type="checkbox" id="outPiece_" name="outPiece" value="0">
                                        <b>For OutPieces</b>
                                    </td>
                                    <td valign="top" style="width: 20%">
                                        <label id="lbl_bagCount" style="font-family: Calibri; font-weight: bold; font-size: small;">
                                        </label>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <table id="tbl_detailsBag" class="tblDetails">
                                <tr class="headerRow">
                                    <td style="width: 7%; text-align: center;">
                                        <input id="hdOrigin" type="hidden" />
                                        <input id="hdDestination" type="hidden" />
                                    </td>
                                    <td style="width: 13%;">Bag Number/ Outpiece Number
                                    </td>
                                    <td style="width: 15%;">Origin
                                    </td>
                                    <td style="width: 17%;">Destination
                                    </td>
                                    <td style="width: 8%;">Weight
                                    </td>
                                    <td style="width: 15%;">Seal # / Pieces
                                    </td>
                                    <td style="width: 25%;">Remarks
                                    </td>
                                    <td style="width: 25%;">Type
                                    </td>
                                </tr>
                            </table>
                            <br />
                        </ContentTemplate>
                    </Ajax1:TabPanel>
                    <Ajax1:TabPanel ID="TabPanel2" runat="server" HeaderText="Consignments" Visible="false" Width="100%">
                        <ContentTemplate>
                            <table style="width: 100% !important;">
                                <tr>
                                    <td valign="top" style="width: 20%; font-weight: bold; margin-left: 20px;">Consignment No:
                                    </td>
                                    <td valign="top" style="width: 60%">
                                        <asp:TextBox ID="txt_consignmentno" MaxLength="15" runat="server"
                                            AutoPostBack="false" onkeypress="return isNumberKey(event);"></asp:TextBox>
                                    </td>
                                    <td valign="top" style="width: 20%">
                                        <label id="lbl_cnCount" style="font-family: Calibri; font-weight: bold; font-size: small;">
                                        </label>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <table id="tbl_detailsCN" class="tblDetails">
                                <tr class="headerRow">
                                    <td style="width: 7%; text-align: center;"></td>
                                    <td style="width: 15%;">Consignment Number
                                    </td>
                                    <td style="width: 12%;">Service Type
                                    </td>
                                    <td style="width: 12%;">Consignment Type
                                    </td>
                                    <td style="width: 17%;">Destination
                                    </td>
                                    <td style="width: 7%;">Weight
                                    </td>
                                    <td style="width: 7%;">Pieces
                                    </td>
                                    <td style="width: 23%;">Remarks
                                    </td>
                                </tr>
                            </table>
                            <br />
                        </ContentTemplate>
                    </Ajax1:TabPanel>
                </Ajax1:TabContainer>
            </div>
            <div style="width: 100%; text-align: center; float: left;">
                <input id="btn_save" type="button" class="button" onclick="SaveLoading()" value="Save" />&nbsp;
                &nbsp; &nbsp; &nbsp;
                <%--<input type="button" class="button" onclick="ResetAll()" value="Reset" />--%>
            </div>
        </div>
        <div style="display: none;">
            <asp:GridView ID="cnControls" runat="server">
                <Columns>
                    <asp:BoundField HeaderText="Prefix" DataField="Prefix" />
                    <asp:BoundField HeaderText="Length" DataField="Length" />
                </Columns>
            </asp:GridView>
            <asp:GridView ID="vehicleTypes" runat="server" AutoGenerateColumns="false">
                <Columns>
                    <asp:BoundField HeaderText="VehicleCode" DataField="VehicleCode" />
                    <asp:BoundField HeaderText="TypeID" DataField="VehicleType_" />
                </Columns>
            </asp:GridView>
        </div>
        <select id="branches" style="display: none">
            <option value="0">.:Select Branch:.</option>
        </select>
        <%--Mode Change--%>
        <script type="text/javascript">
            function ModeChange(rbt) {

                var route = document.getElementById('<%= dd_route.ClientID %>');
                var startDate = document.getElementById('<%= dd_start_date.ClientID %>');
                var touchPoint = document.getElementById('<%= dd_touchpoint.ClientID %>');
                var vid = document.getElementById('<%= txt_vid.ClientID %>');
                var transportType = document.getElementById('<%= dd_transporttype.ClientID %>');
                var origin = document.getElementById('<%= dd_orign.ClientID %>');
                var flight = document.getElementById('<%= txt_flight.ClientID %>');
                var departureDate = document.getElementById('<%= dept_date.ClientID %>');
                var rVehicle = document.getElementById('Vehicle');
                var rRented = document.getElementById('Rented');
                var destination = document.getElementById('<%= dd_destination.ClientID %>');
                var vehicle = document.getElementById('<%= dd_vehicle.ClientID %>');
                var regNo = document.getElementById('<%= txt_reg.ClientID %>');
                var vehicleType = document.getElementById('<%= dd_vehicleType.ClientID %>');
                var description = document.getElementById('<%= txt_description.ClientID %>');
                var courierName = document.getElementById('<%= txt_couriername.ClientID %>');
                var seal = document.getElementById('<%= txt_seal.ClientID %>');
                var hd_loadingID = document.getElementById('<%= hd_loadingID.ClientID %>');
                var hd_IDChk = document.getElementById('<%= hd_IDChk.ClientID %>');

                var consignments = document.getElementById('tbl_detailsCN');
                var bags = document.getElementById('tbl_detailsBag');

                var selectedValue = "";
                var inputs = rbt.getElementsByTagName('input');
                for (var i = 0; i < inputs.length; i++) {
                    if (inputs[i].checked) {
                        selectedValue = inputs[i].value;
                        break;
                    }

                }

                vid.value = "";
                route.options.selectedIndex = 0;
                touchPoint.length = 0;
                destination.length = 0;
                transportType.options.selectedIndex = 0;
                TransportTypeChange(transportType);
                vehicle.options.selectedIndex = 0;
                regNo.value = "";
                description.value = "";
                courierName.value = "";
                vehicleType.options.selectedIndex = 0;
                for (var i = 1; i < bags.rows.length;) {
                    bags.deleteRow(1);
                }

                if (selectedValue.toString().toUpperCase() == "UPDATE") {
                    vid.disabled = false;
                    destination.disabled = true;
                    startDate.disabled = true;
                    hd_IDChk.value = '1';
                    hd_loadingID.innerHTML = '';
                    ' <% Session["Loading_check"] = "2";  %>';
                }
                else if (selectedValue.toString().toUpperCase() == "NEW") {
                    vid.disabled = true;
                    destination.disabled = true;
                    startDate.disabled = true;
                    ' <% Session["Loading_check"] = "1";  %>';
                }
                CalculateItems();

            }
        </script>
        <%--Populating Branches--%>
        <script type="text/javascript">
            var Br = "";
            window.onload = OnWindowLoad;
            function OnWindowLoad() {
                var dropdown = document.getElementById('branches');
                var route = document.getElementById('<%= dd_route.ClientID %>');
                PopulateBranches();

            }
            function PopulateBranches() {
                var dropdown = document.getElementById('branches');
                $.ajax({
                    url: 'MasterAirportLoadingSpeedy_new.aspx/GetBranchesForDropDown',
                    type: 'POST',
                    contentType: 'application/json; charset=utf-8',
                    datatype: 'json',
                    data: '',
                    success: function (result) {
                        var a = '';
                        for (var i = 0; i < result.d.length; i++) {
                            var option = document.createElement('option');
                            option.text = result.d[i].BranchName;
                            option.value = result.d[i].BranchCode;

                            dropdown.add(option);
                        }

                        Br = dropdown;
                    },
                    error: function () {

                    },
                    failure: function () {

                    }

                })
            }


        </script>
        <%--Route Change--%>
        <script type="text/javascript">
            function RouteChange(dd) {
                var routeCode = dd.options[dd.options.selectedIndex].value;
                $.ajax({
                    url: 'MasterAirportLoadingSpeedy_new.aspx/RouteChange',
                    type: 'POST',
                    contentType: 'application/json; charset=utf-8',
                    datatype: 'json',
                    data: "{'route':'" + routeCode + "'}",
                    success: function (result) {
                        var touchPoint = document.getElementById('<%= dd_touchpoint.ClientID %>');
                        var destination = document.getElementById('<%= dd_destination.ClientID %>');
                        touchPoint.options.length = 0;
                        destination.options.length = 0;

                        for (var i = 0; i < result.d.length; i++) {
                            var option = document.createElement('option');
                            option.text = result.d[i].Text;
                            option.value = result.d[i].Value;
                            if (result.d[i].id == "1") {
                                touchPoint.add(option);
                            }
                            else if (result.d[i].id == "2") {
                                destination.add(option);
                            }
                        }
                    },
                    error: function () {

                    },
                    failure: function () {

                    }

                })

            }
        </script>
        <%--Transport Type Change--%>
        <script type="text/javascript">
            function TransportTypeChange(dd) {
                var tr = document.getElementById('flight');
                if (dd.options[dd.options.selectedIndex].value == "197") {
                    flight.style.display = 'block';
                }
                else {
                    flight.style.display = 'none';
                }
            }
        </script>
        <%--Vehicle Mode Change--%>
        <script type="text/javascript">
            function VehicleModeChange(rbtn) {
                if (rbtn.id == "Vehicle") {
                    vehiclediv1.style.display = 'block';
                    vehiclediv2.style.display = 'block';
                    regdiv1.style.display = 'none';
                    regdiv2.style.display = 'none';
                }
                else {
                    regdiv1.style.display = 'block';
                    regdiv2.style.display = 'block';
                    vehiclediv1.style.display = 'none';
                    vehiclediv2.style.display = 'none';
                }
            }
        </script>
        <%--Adding Bag--%>
        <script type="text/javascript">

            function bag_TextChanged() {
                var consignment = document.getElementById('<%= txt_bagno.ClientID %>');
                var chk = document.getElementById("outPiece_");
                var rVehicle = document.getElementById('Vehicle');
                var rRented = document.getElementById('Rented');
                var vehicle = document.getElementById('<%= dd_vehicle.ClientID %>');
                var vehicleType = document.getElementById('<%= dd_vehicleType.ClientID %>');


                if (vehicleType.options[vehicleType.options.selectedIndex].value == '0') {
                    alert('Select Vehicle type');
                    consignment.disabled = false;
                    return;
                }

                var vehicleMode = 0;
                if (rRented.checked) {
                    vehicleMode = 'r';
                }
                else if (rVehicle.checked) {
                    vehicleMode = 'v';
                }
                if (vehicleMode == 'v') {

                    if (vehicle.options.selectedIndex == "0") {
                        alert('Select vehicle');
                        focusWorking(consignment);
                        consignment.value = "";
                        consignment.disabled = false;
                        return;
                    }

                }
                if (consignment.value == '0' || consignment.value == '' || consignment.length == '0') {
                    if (chk.checked == true) {
                        alert('Enter Outpiece No');
                        document.getElementById('<%= txt_bagno.ClientID %>').focus();
                        return;
                    }
                    else {
                        alert('Enter Bag No');
                        document.getElementById('<%= txt_bagno.ClientID %>').focus();
                        return;
                    }
                }
                else {
                    consignment.disabled = true;
                    var isnum = /^\d+$/.test(consignment.value);
                    if (!isnum) {
                        if (chk.checked == true) {
                            alert('Kindly Insert Proper Consignment Number.');
                        }
                        else {
                            alert('Kindly Insert Proper Bag No.');
                        }
                        focusWorking(consignment);
                        consignment.value = '';
                        consignment.disabled = false;
                        return;
                    }

                    if (chk.checked == true) {
                        consignment_TextChanged();
                    }
                    else {
                        PageMethods.Get_bagInformation(consignment.value, onSuccess_bag, onFailure);
                    }
                }
            }

            function onSuccess_bag(result) {
                var consignment = document.getElementById('<%= txt_bagno.ClientID %>');
                if (result.toString() == "N/A") {
                    AddBag();

                }
                else {
                    var mainfest = result;
                    AddBag_(result);
                }
            }

            function AddBag_(Bagging) {

                var mode = document.getElementById('<%= rbtn_mode.ClientID %>');
                var route = document.getElementById('<%= dd_route.ClientID %>');
                var startDate = document.getElementById('<%= dd_start_date.ClientID %>');
                var touchPoint = document.getElementById('<%= dd_touchpoint.ClientID %>');
                var vid = document.getElementById('<%= txt_vid.ClientID %>');
                var transportType = document.getElementById('<%= dd_transporttype.ClientID %>');
                var origin = document.getElementById('<%= dd_orign.ClientID %>');
                var flight = document.getElementById('<%= txt_flight.ClientID %>');
                var departureDate = document.getElementById('<%= dept_date.ClientID %>');
                var rVehicle = document.getElementById('Vehicle');
                var rRented = document.getElementById('Rented');
                var destination = document.getElementById('<%= dd_destination.ClientID %>');
                var vehicle = document.getElementById('<%= dd_vehicle.ClientID %>');
                var regNo = document.getElementById('<%= txt_reg.ClientID %>');
                var vehicleType = document.getElementById('<%= dd_vehicleType.ClientID %>');
                var description = document.getElementById('<%= txt_description.ClientID %>');
                var courierName = document.getElementById('<%= txt_couriername.ClientID %>');
                var seal = document.getElementById('<%= txt_seal.ClientID %>');
                var weight = document.getElementById('<%= txt_totalLoadWeight.ClientID %>');

                <%--var cnNo = document.getElementById('<%= txt_consignmentno.ClientID %>');--%>
                var bagNo = document.getElementById('<%= txt_bagno.ClientID %>');
                var bags = document.getElementById('tbl_detailsBag');
                //var consignments = document.getElementById('tbl_detailsCN');

                if (destination.options.selectedIndex == -1) {
                    alert('Select Route');

                    focusWorking(route);
                    bagNo.value = "";
                    bagNo.disabled = false;
                    return;
                }

                if (bagNo.value.trim() == "") {
                    alert('Enter a Valid Bag Number');
                    bagNo.disabled = false;
                    return;
                }


                if (vehicleType.options[vehicleType.options.selectedIndex].value.toString() == "") {
                    alert('Select Vehicle type');
                    bagNo.disabled = false;
                    return;

                }
                var vehicleMode = 0;
                if (rRented.checked) {
                    vehicleMode = 'r';
                }
                else if (rVehicle.checked) {
                    vehicleMode = 'v';
                }
                if (vehicleMode == 'v') {

                    if (vehicle.options.selectedIndex == "0") {
                        alert('Select vehicle');
                        focusWorking(vehicle);
                        bagNo.value = "";
                        bagNo.disabled = false;
                        return;
                    }

                }

                var inputs = mode.getElementsByTagName('input');

                var selectedValue = "";
                for (var i = 0; i < inputs.length; i++) {
                    if (inputs[i].checked) {
                        selectedValue = inputs[i].value;
                        break;
                    }

                }
                if (selectedValue.toUpperCase() == "UPDATE") {
                    if (vid.value.trim() == "") {
                        alert('Enter Loading ID');
                        bagNo.value = "";
                        focusWorking(bagNo);
                        bagNo.disabled = false;
                        return;
                    }
                }

                var message = "";

                for (var i = 1; i < bags.rows.length; i++) {

                    if (bags.rows[i].cells[1].innerText.trim() == bagNo.value.trim()) {
                        alert('Bag Already Scanned');
                        bagNo.value = "";
                        focusWorking(bagNo);
                        bagNo.disabled = false;
                        return;
                    }
                }
                if (weight.value == "") {
                    weight.value = "0";
                }

                var jsonObj = { Master: {}, Bag: {}, Bag_linked: [], Consignment_linked: [] }
                /////////////New JASON OBJ////////////////////
                var tempWeight = parseFloat(Bagging[0][3].toString());
                var routeValue = route.options[route.options.selectedIndex].value.toString();
                var transportTypeValue = transportType.options[transportType.options.selectedIndex].value.toString();
                var destinationValue = destination.options[destination.options.selectedIndex].value;
                var vehicleNoValue = vehicle.options[vehicle.options.selectedIndex].value.toString();
                var vehicleTypeValue = vehicleType.options[vehicleType.options.selectedIndex].value.toString();
                var originValue = Bagging[0][1].toString();
                var originText = Bagging[0][5].toString();
                var hd_loadingID = document.getElementById('<%= hd_loadingID.ClientID %>');
                var hd_IDChk = document.getElementById('<%= hd_IDChk.ClientID %>');
                var hd_U_ID = document.getElementById('<%=hd_U_ID.ClientID %>');
                var hd_zoneCode = document.getElementById('<%=hd_zoneCode.ClientID %>');
                var hd_branchCode = document.getElementById('<%=hd_branchCode.ClientID %>');
                var hd_expressCenterCode = document.getElementById('<%=hd_expressCenterCode.ClientID %>');
                var hd_LocationName = document.getElementById('<%=hd_LocationName.ClientID %>');
                var hd_LocationID = document.getElementById('<%=hd_LocationID.ClientID %>');
                var MasterParameters;
                MasterParameters = {
                    Mode: selectedValue,
                    Date: startDate.value,
                    Route: routeValue,
                    TransportType: transportTypeValue,
                    LoadingID: vid.value.toString(),
                    VehicleMode: vehicleMode,
                    Destination: destinationValue,
                    VehicleNo: vehicleNoValue,
                    RegNo: regNo.value.toString(),
                    FlightNo: flight.value.toString(),
                    FlightDeparture: departureDate.value.toString(),
                    VehicleType: vehicleTypeValue,
                    Description: description.value.toString(),
                    CourierName: courierName.value.toString(),
                    SealNo: seal.value.toString(),
                    TotalWeight: weight.value.toString(),
                    lbl_loadingID: hd_loadingID.innerHTML.toString(),
                    hd_IDChk: hd_IDChk.value.toString(),
                    hd_U_ID: hd_U_ID.value.toString(),
                    hd_zoneCode: hd_zoneCode.value.toString(),
                    hd_branchCode: hd_branchCode.value.toString(),
                    hd_expressCenterCode: hd_expressCenterCode.value.toString(),
                    hd_LocationName: hd_LocationName.value.toString(),
                    hd_LocationID: hd_LocationID.value.toString()

                }

                var Bag = {
                    BagNo: bagNo.value.trim(),
                    Weight: Bagging[0][3].toString(),
                    Destination: Bagging[0][2].toString(),
                    Origin: Bagging[0][1].toString(),
                    SealNo: Bagging[0][4].toString(),
                    Remarks: "",
                    SortOrder: bags.rows.length.toString(),
                    OriginName: originText
                }

                jsonObj.Bag = Bag;
                jsonObj.Master = MasterParameters;


                for (var i = 1; i < bags.rows.length; i++) {
                    var Bag_linked;
                    var tr = bags.rows[i];
                    var destinationValue = tr.cells[2].childNodes[0].options[tr.cells[2].childNodes[0].options.selectedIndex].value.toString();
                    if (tr.cells[7].childNodes[0].value.toString() != "CN") {
                        Bag_linked = {
                            BagNo: tr.cells[1].innerText.trim(),
                            Weight: tr.cells[4].childNodes[0].value.toString(),
                            Origin: tr.cells[2].childNodes[0].value.toString(),
                            Destination: tr.cells[3].childNodes[0].value.toString(),
                            SealNo: tr.cells[5].childNodes[0].value.toString(),
                            Remarks: tr.cells[6].childNodes[0].value.toString()
                        }

                        jsonObj.Bag_linked.push(Bag_linked);
                    }
                    else {
                        var destinationValue = tr.cells[2].childNodes[0].options[tr.cells[2].childNodes[0].options.selectedIndex].value;
                        var consignment_lindked = {
                            ConsignmentNumber: tr.cells[1].innerText.trim(),
                            ServiceType: 'Overnight',
                            ConsignmentType: 'Normal',
                            Destination: tr.cells[3].childNodes[0].value.toString(),
                            Weight: tr.cells[4].childNodes[0].value.toString(),
                            Pieces: tr.cells[5].childNodes[0].value.toString(),
                            Remarks: tr.cells[6].childNodes[0].value.toString()
                        }

                        jsonObj.Consignment_linked.push(consignment_lindked);
                    }
                }

                //for (var i = 1; i < consignments.rows.length; i++) {
                //    var tr = consignments.rows[i];
                //    var consignment;
                //    var destinationValue = tr.cells[4].childNodes[0].options[tr.cells[4].childNodes[0].options.selectedIndex].value;
                //    consignment = {
                //        ConsignmentNumber: tr.cells[1].innerText,
                //        ServiceType: tr.cells[2].innerText,
                //        ConsignmentType: tr.cells[3].innerText,
                //        Destination: destinationValue,
                //        Weight: tr.cells[5].childNodes[0].value.toString(),
                //        Pieces: tr.cells[6].childNodes[0].value.toString(),
                //        Remarks: tr.cells[7].childNodes[0].value.toString()
                //    }
                //    jsonObj.Consignment_linked.push(consignment);
                //}
                var saved = false;
                if (bags.rows.length > 0) {

                    $.ajax({
                        url: 'MasterAirportLoadingSpeedy_new.aspx/InsertBag',
                        type: 'POST',
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        data: JSON.stringify(jsonObj),
                        success: function (result) {
                            var txt_bagno = document.getElementById('<%= txt_bagno.ClientID %>');
                            var resp = result.d;
                            if (resp.Status.toString() == "0") {
                                alert(resp.Cause.toString());
                                txt_bagno.disabled = false;
                                return;
                            }
                            else if (resp.Status.toString() == "ID0") {
                                alert('Kindly Refresh Page!.Press Okay. Thank You :)');
                                window.location.href = "MasterAirportLoadingSpeedy_new.aspx";
                                return;
                            }
                            else {
                                var hdLoadingID = document.getElementById('<%= hd_loadingID.ClientID %>');
                                //vid.value = resp.Cause.toString();
                                if (resp.Cause.toString() != hdLoadingID.innerText) {
                                    alert('Merging issue inserting addBag__,Contact IT or refresh');
                                    txt_bagno.disabled = false;
                                }
                                vid.disabled = true;
                                saved = true;
                                if (resp.Status.toString() == "1") {
                                    //r1 = SaveLoading_Linked("0");
                                    //if (r1 == "1") {
                                    hd_IDChk.value = '1';
                                    AddBagEdit(resp.Bag, "1");
                                    txt_bagno.disabled = false;
                                    // }    
                                }
                                else if (resp.Status.toString() == "2") {
                                    alert('Error in Saving: ' + resp[1].toString());
                                    txt_bagno.disabled = false;
                                }
                            }
                        },
                        error: function () { },
                        failure: function () { }

                    });

                    return;
                    if (saved == false) {
                        bagNo.value = "";
                        focusWorking(bagNo);
                        txt_bagno.disabled = false;
                        return;
                    }
                }


                if (r1 == "1") {
                    var newTr = bags.insertRow(1);  //document.createElement('tr');  //consignments.rows[0].cloneNode(true);
                    newTr.className = 'DetailRow';

                    var btn_remove = document.createElement('input');
                    btn_remove.type = 'button';
                    btn_remove.className = 'button button1';
                    btn_remove.value = 'Remove';
                    btn_remove.style.marginTop = '2px';
                    btn_remove.style.marginBottom = '2px';
                    btn_remove.style.width = '80%';

                    var txtWeight = document.createElement('input');
                    txtWeight.type = 'text';
                    txtWeight.className = 'textBox';
                    txtWeight.value = Bagging;
                    txtWeight.style.width = '70%';
                    txtWeight.style.textAlign = 'center';

                    var txtseal = document.createElement('input');
                    txtseal.type = 'text';
                    txtseal.className = 'textBox';
                    txtseal.value = Bagging[0][2].toString();
                    txtseal.style.width = '90%';
                    txtseal.style.textAlign = 'center';

                    var txtremarks = document.createElement('input');
                    txtremarks.type = 'text';
                    txtremarks.className = 'textBox';
                    txtremarks.value = '';
                    txtremarks.style.width = '90%';
                    txtremarks.style.textAlign = 'center';

                    var dd_dest = GetBranchDropDown_(Br, "dd_dest");
                    dd_dest.style.width = '90%'
                    dd_dest.className = 'dropdown';
                    dd_dest.style.display = "";
                    //dd_dest.disabled = true;

                    var dd_ori = GetBranchDropDown_(Br, "dd_ori");
                    dd_ori.style.width = '90%'
                    dd_ori.className = 'dropdown';
                    dd_ori.style.display = "";
                    //dd_ori.disabled = true;

                    var txt_hiddenfield = document.createElement('input');
                    txt_hiddenfield.type = 'label';
                    txt_hiddenfield.className = 'textBox';
                    txt_hiddenfield.value = 'bag';
                    txt_hiddenfield.style.width = '90%';
                    txt_hiddenfield.style.textAlign = 'center';

                    col = newTr.insertCell(0);
                    newTr.cells[0].appendChild(btn_remove);
                    newTr.cells[0].childNodes[0].onclick = RemoveRow.bind(newTr.cells[0].childNodes[0], newTr.cells[0].childNodes[0]);
                    col = newTr.insertCell(1);
                    newTr.cells[1].innerText = bagNo.value;


                    col = newTr.insertCell(4);
                    newTr.cells[4].appendChild(txtWeight);
                    newTr.cells[4].style.textAlign = 'center';
                    newTr.cells[4].childNodes[0].onchange = CalculateTotalWeight.bind(newTr.cells[4].childNodes[0]);
                    newTr.cells[4].childNodes[0].maxLength = "5";

                    col = newTr.insertCell(2);
                    newTr.cells[2].style.textAlign = 'center';
                    newTr.cells[2].appendChild(dd_ori);
                    for (var i = 0; i < newTr.cells[2].childNodes[0].options.length; i++) {
                        var currentDestination = origin.options[origin.options.selectedIndex].value;
                        var dd_destValue = newTr.cells[2].childNodes[0].options[i].value;
                        if (dd_destValue == currentDestination) {
                            newTr.cells[2].childNodes[0].options[i].selected = true;
                            break;
                        }
                    }

                    col = newTr.insertCell(3);
                    newTr.cells[3].style.textAlign = 'center';
                    newTr.cells[3].appendChild(dd_dest);
                    for (var i = 0; i < newTr.cells[3].childNodes[0].options.length; i++) {
                        var currentDestination = Bag[0][2].toString();
                        var dd_destValue = newTr.cells[3].childNodes[0].options[i].value;
                        if (dd_destValue == currentDestination) {
                            newTr.cells[3].childNodes[0].options[i].selected = true;
                            break;
                        }
                    }

                    col = newTr.insertCell(5);
                    newTr.cells[5].appendChild(txtseal);
                    newTr.cells[4].style.textAlign = "center";
                    newTr.cells[5].style.textAlign = "center";

                    col = newTr.insertCell(6);
                    newTr.cells[6].style.textAlign = "center";
                    newTr.cells[6].appendChild(txtremarks);

                    col = newTr.insertCell(7);
                    newTr.cells[7].style.textAlign = "center";
                    newTr.cells[7].appendChild(txt_hiddenfield);

                    //consignments.appendChild(newTr);

                    bagNo.value = '';
                    focusWorking(bagNo);

                    CalculateItems();
                    CalculateTotalWeight();
                }
            }

            function AddBag() {


                var mode = document.getElementById('<%= rbtn_mode.ClientID %>');
                var route = document.getElementById('<%= dd_route.ClientID %>');
                var startDate = document.getElementById('<%= dd_start_date.ClientID %>');
                var touchPoint = document.getElementById('<%= dd_touchpoint.ClientID %>');
                var vid = document.getElementById('<%= txt_vid.ClientID %>');
                var transportType = document.getElementById('<%= dd_transporttype.ClientID %>');
                var origin = document.getElementById('<%= dd_orign.ClientID %>');
                var flight = document.getElementById('<%= txt_flight.ClientID %>');
                var departureDate = document.getElementById('<%= dept_date.ClientID %>');
                var rVehicle = document.getElementById('Vehicle');
                var rRented = document.getElementById('Rented');
                var destination = document.getElementById('<%= dd_destination.ClientID %>');
                var vehicle = document.getElementById('<%= dd_vehicle.ClientID %>');
                var regNo = document.getElementById('<%= txt_reg.ClientID %>');
                var vehicleType = document.getElementById('<%= dd_vehicleType.ClientID %>');
                var description = document.getElementById('<%= txt_description.ClientID %>');
                var courierName = document.getElementById('<%= txt_couriername.ClientID %>');
                var seal = document.getElementById('<%= txt_seal.ClientID %>');
                var weight = document.getElementById('<%= txt_totalLoadWeight.ClientID %>');

            <%--    var cnNo = document.getElementById('<%= txt_consignmentno.ClientID %>');--%>
                var bagNo = document.getElementById('<%= txt_bagno.ClientID %>');

                //var consignments = document.getElementById('tbl_detailsCN');
                var bags = document.getElementById('tbl_detailsBag');

                if (destination.options.selectedIndex == -1) {
                    alert('Select Route');
                    focusWorking(route);
                    bagNo.value = "";
                    bagNo.disabled = false;
                    return;
                }

                var vehicleMode = 0;
                if (rRented.checked) {
                    vehicleMode = 'r';
                }
                else if (rVehicle.checked) {
                    vehicleMode = 'v';
                }
                if (vehicleMode == 'v') {

                    if (vehicle.options.selectedIndex == "0") {
                        alert('Select vehicle');
                        focusWorking(vehicle);
                        bagNo.value = "";
                        bagNo.disabled = false;
                        return;
                    }

                }


                if (bagNo.value.trim() == "") {
                    alert('Enter a Valid Bag Number');
                    bagNo.disabled = false;
                    return;
                }
                if (vehicleType.options[vehicleType.options.selectedIndex].value.toString() == "") {
                    alert('Select Vehicle type');
                    bagNo.disabled = false;
                    return;

                }
                var inputs = mode.getElementsByTagName('input');

                var selectedValue = "";
                for (var i = 0; i < inputs.length; i++) {
                    if (inputs[i].checked) {
                        selectedValue = inputs[i].value;
                        break;
                    }

                }
                if (selectedValue.toUpperCase() == "UPDATE") {
                    if (vid.value.trim() == "") {
                        alert('Enter Loading ID');
                        bagNo.value = "";
                        focusWorking(bagNo);
                        bagNo.disabled = false;
                        return;
                    }
                }






                var message = "";

                for (var i = 1; i < bags.rows.length; i++) {

                    if (bags.rows[i].cells[1].innerText.trim() == bagNo.value.trim()) {
                        alert('Bag Already Scanned');
                        bagNo.value = "";
                        focusWorking(bagNo);
                        bagNo.disabled = false;
                        return;
                    }
                }
                if (weight.value == "") {
                    weight.value = "0";
                }


                var jsonObj = { Master: {}, Bag: {}, Bag_linked: [], Consignment_linked: [] }
                /////////////New JASON OBJ////////////////////

                var tempWeight = parseFloat(weight.value);
                var routeValue = route.options[route.options.selectedIndex].value.toString();
                var transportTypeValue = transportType.options[transportType.options.selectedIndex].value.toString();
                var destinationValue = destination.options[destination.options.selectedIndex].value.toString()
                var vehicleNoValue = vehicle.options[vehicle.options.selectedIndex].value.toString();
                var vehicleTypeValue = vehicleType.options[vehicleType.options.selectedIndex].value.toString();
                var originValue = origin.options[origin.options.selectedIndex].value;
                var originText = origin.options[origin.options.selectedIndex].text;
                var hd_loadingID = document.getElementById('<%= hd_loadingID.ClientID %>');
                var hd_IDChk = document.getElementById('<%= hd_IDChk.ClientID %>');
                var hd_U_ID = document.getElementById('<%=hd_U_ID.ClientID %>');
                var hd_zoneCode = document.getElementById('<%=hd_zoneCode.ClientID %>');
                var hd_branchCode = document.getElementById('<%=hd_branchCode.ClientID %>');
                var hd_expressCenterCode = document.getElementById('<%=hd_expressCenterCode.ClientID %>');
                var hd_LocationName = document.getElementById('<%=hd_LocationName.ClientID %>');
                var hd_LocationID = document.getElementById('<%=hd_LocationID.ClientID %>');
                var MasterParameters;
                MasterParameters = {
                    Mode: selectedValue,
                    Date: startDate.value,
                    Route: routeValue,
                    TransportType: transportTypeValue,
                    LoadingID: vid.value.toString(),
                    VehicleMode: vehicleMode,
                    Destination: destinationValue,
                    VehicleNo: vehicleNoValue,
                    RegNo: regNo.value.toString(),
                    FlightNo: flight.value.toString(),
                    FlightDeparture: departureDate.value.toString(),
                    VehicleType: vehicleTypeValue,
                    Description: description.value.toString(),
                    CourierName: courierName.value.toString(),
                    SealNo: seal.value.toString(),
                    TotalWeight: weight.value.toString(),
                    lbl_loadingID: hd_loadingID.innerHTML.toString(),
                    hd_IDChk: hd_IDChk.value.toString(),
                    hd_U_ID: hd_U_ID.value.toString(),
                    hd_zoneCode: hd_zoneCode.value.toString(),
                    hd_branchCode: hd_branchCode.value.toString(),
                    hd_expressCenterCode: hd_expressCenterCode.value.toString(),
                    hd_LocationName: hd_LocationName.value.toString(),
                    hd_LocationID: hd_LocationID.value.toString()
                }

                var Bag = {
                    BagNo: bagNo.value.trim(),
                    Weight: "1",
                    Destination: destinationValue,
                    Origin: originValue,
                    SealNo: "",
                    Remarks: "",
                    SortOrder: bags.rows.length.toString(),
                    OriginName: originText
                }

                jsonObj.Bag = Bag;
                jsonObj.Master = MasterParameters;

                for (var i = 1; i < bags.rows.length; i++) {
                    var Bag_linked;
                    var tr = bags.rows[i];
                    var destinationValue = tr.cells[2].childNodes[0].options[tr.cells[2].childNodes[0].options.selectedIndex].value.toString();
                    if (tr.cells[7].childNodes[0].value.toString() != "CN") {
                        Bag_linked = {
                            BagNo: tr.cells[1].innerText.trim(),
                            Weight: tr.cells[4].childNodes[0].value.toString(),
                            Origin: tr.cells[2].childNodes[0].value.toString(),
                            Destination: tr.cells[3].childNodes[0].value.toString(),
                            SealNo: tr.cells[5].childNodes[0].value.toString(),
                            Remarks: tr.cells[6].childNodes[0].value.toString()
                        }

                        jsonObj.Bag_linked.push(Bag_linked);
                    }
                    else {
                        var destinationValue = tr.cells[2].childNodes[0].options[tr.cells[2].childNodes[0].options.selectedIndex].value;
                        var consignment_lindked = {
                            ConsignmentNumber: tr.cells[1].innerText.trim(),
                            ServiceType: 'Overnight',
                            ConsignmentType: 'Normal',
                            Destination: tr.cells[3].childNodes[0].value.toString(),
                            Weight: tr.cells[4].childNodes[0].value.toString(),
                            Pieces: tr.cells[5].childNodes[0].value.toString(),
                            Remarks: tr.cells[6].childNodes[0].value.toString()
                        }

                        jsonObj.Consignment_linked.push(consignment_lindked);
                    }
                }

                var saved = false;
                if (bags.rows.length > 0) {
                    $.ajax({
                        url: 'MasterAirportLoadingSpeedy_new.aspx/InsertBag',
                        type: 'POST',
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        data: JSON.stringify(jsonObj),
                        success: function (result) {
                            var txt_bagno = document.getElementById('<%= txt_bagno.ClientID %>');
                            var resp = result.d;
                            if (resp.Status.toString() == "0") {
                                alert(resp.Cause.toString());
                                txt_bagno.disabled = false;
                                return;
                            }
                            else if (resp.Status.toString() == "ID0") {
                                alert('Kindly Refresh Page!.Press Okay. Thank You :)');
                                window.location.href = "MasterAirportLoadingSpeedy_new.aspx";
                                return;
                            }
                            else {
                                //vid.value = resp.Cause.toString();
                                var hdLoadingID = document.getElementById('<%= hd_loadingID.ClientID %>');
                                if (resp.Cause.toString() != hdLoadingID.innerText) {
                                    alert('Merging issue inserting addBag,Contact IT or refresh');
                                    txt_bagno.disabled = false;
                                }
                                vid.disabled = true;
                                saved = true;
                                if (resp.Status.toString() == "1") {
                                    //r1 = SaveLoading_Linked("0");
                                    //if (r1 == "1") {
                                    hd_IDChk.value = '1';
                                    AddBagEdit(resp.Bag, "1");
                                    txt_bagno.disabled = false;
                                    // }    
                                }
                                else if (resp.Status.toString() == "2") {
                                    alert('Error in Saving: ' + resp[1].toString());
                                    txt_bagno.disabled = false;
                                }
                                //else {
                                //    //vid.value = resp.Cause.toString();
                                //    vid.disabled = true;
                                //    saved = true;
                                //    r1 = SaveLoading_Linked("0");
                                //    r1 = "1";
                                //    if (r1 == "1") {
                                //        AddBagEdit(resp.Bag, "1");
                                //    }
                            }
                        },
                        error: function () { },
                        failure: function () { }

                    });

                    return;
                    if (saved == false) {
                        bagNo.value = "";
                        focusWorking(bagNo);
                        txt_bagno.disabled = false;
                        return;
                    }

                }
                if (r1 == "1") {

                    var newTr = bags.insertRow(1);  //document.createElement('tr');  //consignments.rows[0].cloneNode(true);
                    newTr.className = 'DetailRow';

                    var btn_remove = document.createElement('input');
                    btn_remove.type = 'button';
                    btn_remove.className = 'button button1';
                    btn_remove.value = 'Remove';
                    btn_remove.style.marginTop = '2px';
                    btn_remove.style.marginBottom = '2px';
                    btn_remove.style.width = '80%';

                    var txtWeight = document.createElement('input');
                    txtWeight.type = 'text';
                    txtWeight.className = 'textBox';
                    txtWeight.value = '1';
                    txtWeight.style.width = '70%';
                    txtWeight.style.textAlign = 'center';

                    var txtseal = document.createElement('input');
                    txtseal.type = 'text';
                    txtseal.className = 'textBox';
                    txtseal.value = '';
                    txtseal.style.width = '90%';
                    txtseal.style.textAlign = 'center';

                    var txtremarks = document.createElement('input');
                    txtremarks.type = 'text';
                    txtremarks.className = 'textBox';
                    txtremarks.value = '';
                    txtremarks.style.width = '90%';
                    txtremarks.style.textAlign = 'center';

                    var dd_dest = GetBranchDropDown_(Br, "dd_dest");
                    dd_dest.style.width = '90%'
                    dd_dest.className = 'dropdown';
                    dd_dest.style.display = "";
                    //dd_dest.disabled = true;

                    var dd_ori = GetBranchDropDown_(Br, "dd_ori");
                    dd_ori.style.width = '90%'
                    dd_ori.className = 'dropdown';
                    dd_ori.style.display = "";
                    //dd_ori.disabled = true;

                    var txt_hiddenfield = document.createElement('input');
                    txt_hiddenfield.type = 'label';
                    txt_hiddenfield.className = 'textBox';
                    txt_hiddenfield.value = 'bag';
                    txt_hiddenfield.style.width = '90%';
                    txt_hiddenfield.style.textAlign = 'center';

                    col = newTr.insertCell(0);
                    newTr.cells[0].appendChild(btn_remove);
                    newTr.cells[0].childNodes[0].onclick = RemoveRow.bind(newTr.cells[0].childNodes[0], newTr.cells[0].childNodes[0]);
                    col = newTr.insertCell(1);
                    newTr.cells[1].innerText = bagNo.value;


                    col = newTr.insertCell(4);
                    newTr.cells[4].appendChild(txtWeight);
                    newTr.cells[4].style.textAlign = 'center';
                    newTr.cells[4].childNodes[0].onchange = CalculateTotalWeight.bind(newTr.cells[4].childNodes[0]);
                    newTr.cells[4].childNodes[0].maxLength = "5";

                    col = newTr.insertCell(2);
                    newTr.cells[2].style.textAlign = 'center';
                    newTr.cells[2].appendChild(dd_ori);
                    for (var i = 0; i < newTr.cells[2].childNodes[0].options.length; i++) {
                        var currentDestination = origin.options[origin.options.selectedIndex].value;
                        var dd_destValue = newTr.cells[2].childNodes[0].options[i].value;
                        if (dd_destValue == currentDestination) {
                            newTr.cells[2].childNodes[0].options[i].selected = true;
                            break;
                        }
                    }

                    col = newTr.insertCell(3);
                    newTr.cells[3].style.textAlign = 'center';
                    newTr.cells[3].appendChild(dd_dest);
                    for (var i = 0; i < newTr.cells[3].childNodes[0].options.length; i++) {
                        var currentDestination = destination.options[destination.options.selectedIndex].value;
                        var dd_destValue = newTr.cells[3].childNodes[0].options[i].value;
                        if (dd_destValue == currentDestination) {
                            newTr.cells[3].childNodes[0].options[i].selected = true;
                            break;
                        }
                    }

                    col = newTr.insertCell(5);
                    newTr.cells[5].appendChild(txtseal);
                    newTr.cells[4].style.textAlign = "center";
                    newTr.cells[5].style.textAlign = "center";

                    col = newTr.insertCell(6);
                    newTr.cells[6].style.textAlign = "center";
                    newTr.cells[6].appendChild(txtremarks);

                    col = newTr.insertCell(7);
                    newTr.cells[7].style.textAlign = "center";
                    newTr.cells[7].appendChild(txt_hiddenfield);

                    bagNo.value = '';
                    focusWorking(bagNo);

                    CalculateItems();
                    CalculateTotalWeight();
                }
            }

            function AddBagEdit(SingleBag, AddMode) {


                var mode = document.getElementById('<%= rbtn_mode.ClientID %>');
                var route = document.getElementById('<%= dd_route.ClientID %>');
                var startDate = document.getElementById('<%= dd_start_date.ClientID %>');
                var touchPoint = document.getElementById('<%= dd_touchpoint.ClientID %>');
                var vid = document.getElementById('<%= txt_vid.ClientID %>');
                var transportType = document.getElementById('<%= dd_transporttype.ClientID %>');
                var origin = document.getElementById('<%= dd_orign.ClientID %>');
                var flight = document.getElementById('<%= txt_flight.ClientID %>');
                var departureDate = document.getElementById('<%= dept_date.ClientID %>');
                var rVehicle = document.getElementById('Vehicle');
                var rRented = document.getElementById('Rented');
                var destination = document.getElementById('<%= dd_destination.ClientID %>');
                var vehicle = document.getElementById('<%= dd_vehicle.ClientID %>');
                var regNo = document.getElementById('<%= txt_reg.ClientID %>');
                var vehicleType = document.getElementById('<%= dd_vehicleType.ClientID %>');
                var description = document.getElementById('<%= txt_description.ClientID %>');
                var courierName = document.getElementById('<%= txt_couriername.ClientID %>');
                var seal = document.getElementById('<%= txt_seal.ClientID %>');
                var weight = document.getElementById('<%= txt_totalLoadWeight.ClientID %>');

                var bagNo = document.getElementById('<%= txt_bagno.ClientID %>');
                var bags = document.getElementById('tbl_detailsBag');

                if (destination.options.selectedIndex == -1) {
                    alert('Select Route');

                    focusWorking(route);
                    bagNo.value = "";
                    return;
                }

                if (bagNo.value.trim() == "") {
                    alert('Enter a Valid Bag Number');
                    return;
                }

                if (vehicleType.options[vehicleType.options.selectedIndex].value.toString() == "") {
                    alert('Select Vehicle type');
                    return;

                }

                var inputs = mode.getElementsByTagName('input');

                var selectedValue = "";
                for (var i = 0; i < inputs.length; i++) {
                    if (inputs[i].checked) {
                        selectedValue = inputs[i].value;
                        break;
                    }

                }
                if (selectedValue.toUpperCase() == "UPDATE") {
                    if (vid.value.trim() == "") {
                        alert('Enter Loading ID');
                        focusWorking(route);
                        bagNo.value = "";
                        return;
                    }
                }

                var message = "";

                for (var i = 1; i < bags.rows.length; i++) {

                    if (bags.rows[i].cells[1].innerText.trim() == bagNo.value.trim()) {
                        alert('Bag Already Scanned');
                        focusWorking(route);
                        bagNo.value = "";
                        bagNo.disabled = false;
                        return;
                    }
                }
                if (weight.value == "") {
                    weight.value = "0";
                }
                var tempWeight = parseFloat(weight.value);

                for (var i = 0; i < destination.options.length; i++) {

                }

                var newTr = bags.insertRow(1);  //document.createElement('tr');  //consignments.rows[0].cloneNode(true);
                newTr.className = 'DetailRow';
                newTr.id = "Bag-" + bags.rows.length.toString();

                newTr.className = 'DetailRow';

                var btn_remove = document.createElement('input');
                btn_remove.type = 'button';
                btn_remove.className = 'button button1';
                btn_remove.value = 'Remove';
                btn_remove.style.marginTop = '2px';
                btn_remove.style.marginBottom = '2px';
                btn_remove.style.width = '80%';

                var txtWeight = document.createElement('input');
                txtWeight.type = 'text';
                txtWeight.className = 'textBox';
                txtWeight.value = '1';
                txtWeight.style.width = '70%';
                txtWeight.style.textAlign = 'center';

                var txtseal = document.createElement('input');
                txtseal.type = 'text';
                txtseal.className = 'textBox';
                txtseal.value = '';
                txtseal.style.width = '90%';
                txtseal.style.textAlign = 'center';

                var txtremarks = document.createElement('input');
                txtremarks.type = 'text';
                txtremarks.className = 'textBox';
                txtremarks.value = SingleBag.Remarks;
                txtremarks.style.width = '90%';
                txtremarks.style.textAlign = 'center';

                var dd_dest = GetBranchDropDown_(Br, "dd_dest");
                dd_dest.style.width = '90%'
                dd_dest.className = 'dropdown';
                dd_dest.style.display = "";
                //dd_dest.disabled = true;

                var dd_ori = GetBranchDropDown_(Br, "dd_ori");
                dd_ori.style.width = '90%'
                dd_ori.className = 'dropdown';
                dd_ori.style.display = "";
                //dd_ori.disabled = true;

                var txt_hiddenfield = document.createElement('input');
                txt_hiddenfield.type = 'label';
                txt_hiddenfield.className = 'textBox';
                txt_hiddenfield.value = 'bag';
                txt_hiddenfield.style.width = '90%';
                txt_hiddenfield.style.textAlign = 'center';

                col = newTr.insertCell(0);
                if (selectedValue.toUpperCase() == "NEW" || AddMode == "1") {
                    newTr.cells[0].appendChild(btn_remove);
                    newTr.cells[0].childNodes[0].onclick = RemoveRow.bind(newTr.cells[0].childNodes[0], newTr.cells[0].childNodes[0]);
                }


                col = newTr.insertCell(1);
                newTr.cells[1].innerText = bagNo.value.trim();



                col = newTr.insertCell(2);
                newTr.cells[2].style.textAlign = 'center';
                newTr.cells[2].appendChild(dd_ori);
                for (var i = 0; i < newTr.cells[2].childNodes[0].options.length; i++) {
                    var currentDestination = SingleBag.Origin; //origin.options[origin.options.selectedIndex].value;
                    var dd_destValue = newTr.cells[2].childNodes[0].options[i].value;
                    if (dd_destValue == currentDestination) {
                        newTr.cells[2].childNodes[0].options[i].selected = true;
                        break;
                    }
                }

                col = newTr.insertCell(3);
                newTr.cells[3].style.textAlign = 'center';
                newTr.cells[3].appendChild(dd_dest);
                for (var i = 0; i < newTr.cells[3].childNodes[0].options.length; i++) {
                    var currentDestination = SingleBag.Destination; //destination.options[destination.options.selectedIndex].value;
                    var dd_destValue = newTr.cells[3].childNodes[0].options[i].value;
                    if (dd_destValue == currentDestination) {
                        newTr.cells[3].childNodes[0].options[i].selected = true;
                        break;
                    }
                }

                col = newTr.insertCell(4);
                newTr.cells[4].appendChild(txtWeight);
                txtWeight.value = SingleBag.Weight;
                newTr.cells[4].style.textAlign = 'center';
                newTr.cells[4].childNodes[0].onchange = CalculateTotalWeight.bind(newTr.cells[4].childNodes[0]);
                newTr.cells[4].childNodes[0].maxLength = "5";


                col = newTr.insertCell(5);
                newTr.cells[5].appendChild(txtseal);
                newTr.cells[5].style.textAlign = "center";
                newTr.cells[5].style.textAlign = "center";
                txtseal.value = SingleBag.SealNo;

                col = newTr.insertCell(6);
                newTr.cells[6].style.textAlign = "center";
                newTr.cells[6].appendChild(txtremarks);


                col = newTr.insertCell(7);
                newTr.cells[7].style.textAlign = "center";
                newTr.cells[7].appendChild(txt_hiddenfield);


                //consignments.appendChild(newTr);

                bagNo.value = '';
                focusWorking(bagNo);

                CalculateItems();
                CalculateTotalWeight();
            }

        </script>
        <%--Adding Consignment--%>
        <script type="text/javascript">

            // Out piece information
            function consignment_TextChanged() {
                var consignment = document.getElementById('<%= txt_bagno.ClientID %>');
                if (consignment.value == '0' || consignment.value == '' || consignment.length == '0') {
                    alert('Enter Outpiece No');
                    document.getElementById('<%= txt_bagno.ClientID %>').focus();
                    consignment.disabled = false;
                    return;
                }
                else {

                    //COD Controls start
                    debugger;
                    var OutputMessage = '';
                    var CODControlCheck = true;
                    $.ajax({
                        async: false,
                        type: 'post',
                        url: 'MasterLoadingSpeedy_New.aspx/CheckControls',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        data: JSON.stringify({ cn: consignment.value }),
                        success: (result) => {
                            if (result.d[0][0].toString() == "false") {
                                CODControlCheck = false;
                                OutputMessage = result.d[0][1].toString();
                            }
                        }, error: function (XMLHttpRequest, textStatus, errorThrown) {
                            CODControlCheck = false;
                            OutputMessage = "Error finding CN record!";
                        }
                    });
                    if (!CODControlCheck) {
                        alert(OutputMessage);
                        consignment.value = '';
                        document.getElementById('<%= txt_bagno.ClientID %>').focus();
                        consignment.disabled = false;
                        return;
                    }

                    PageMethods.Get_ConsignmentInformation(consignment.value, onSuccess_Manifest, onFailure);
                }
            }

            function onSuccess_Manifest(result) {
                var consignment = document.getElementById('<%= txt_bagno.ClientID %>');
                if (result.toString() == "N/A") {
                    AddConsignment();
                }
                else {
                    var mainfest = result;
                    AddConsignment_(mainfest);
                }
            }


            function onFailure(result) {
                consignment.disabled = false;
                alert("Failed!");
            }


            function AddConsignment() {
                var mode = document.getElementById('<%= rbtn_mode.ClientID %>');
                var route = document.getElementById('<%= dd_route.ClientID %>');
                var startDate = document.getElementById('<%= dd_start_date.ClientID %>');
                var touchPoint = document.getElementById('<%= dd_touchpoint.ClientID %>');
                var vid = document.getElementById('<%= txt_vid.ClientID %>');
                var transportType = document.getElementById('<%= dd_transporttype.ClientID %>');
                var origin = document.getElementById('<%= dd_orign.ClientID %>');
                var flight = document.getElementById('<%= txt_flight.ClientID %>');
                var departureDate = document.getElementById('<%= dept_date.ClientID %>');
                var rVehicle = document.getElementById('Vehicle');
                var rRented = document.getElementById('Rented');
                var destination = document.getElementById('<%= dd_destination.ClientID %>');
                var vehicle = document.getElementById('<%= dd_vehicle.ClientID %>');
                var regNo = document.getElementById('<%= txt_reg.ClientID %>');
                var vehicleType = document.getElementById('<%= dd_vehicleType.ClientID %>');
                var description = document.getElementById('<%= txt_description.ClientID %>');
                var courierName = document.getElementById('<%= txt_couriername.ClientID %>');
                var seal = document.getElementById('<%= txt_seal.ClientID %>');
                var weight = document.getElementById('<%= txt_totalLoadWeight.ClientID %>');

                var cnNo = document.getElementById('<%= txt_bagno.ClientID %>');
                <%--var bagNo = document.getElementById('<%= txt_bagno.ClientID %>');--%>

                //var consignments = document.getElementById('tbl_detailsCN');
                var consignments = document.getElementById('tbl_detailsBag');

                if (destination.options.selectedIndex == -1) {
                    alert('Select Route');

                    focusWorking(route);
                    cnNo.value = "";
                    cnNo.disabled = false;
                    return;
                }

                if (cnNo.value.trim() == "") {
                    alert('Enter a Valid Consignment Number');
                    cnNo.disabled = false;
                    return;
                }

                if (vehicleType.options[vehicleType.options.selectedIndex] == "0") {
                    alert('Select Vehicle type');
                    cnNo.disabled = false;
                    return;

                }

                var vehicleMode = 0;
                if (rRented.checked) {
                    vehicleMode = 'r';
                }
                else if (rVehicle.checked) {
                    vehicleMode = 'v';
                }
                if (vehicleMode == 'v') {

                    if (vehicle.options.selectedIndex == "0") {
                        alert('Select vehicle');
                        focusWorking(vehicle);
                        cnNo.value = "";
                        cnNo.disabled = false;
                        return;
                    }

                }

                var inputs = mode.getElementsByTagName('input');

                var selectedValue = "";
                for (var i = 0; i < inputs.length; i++) {
                    if (inputs[i].checked) {
                        selectedValue = inputs[i].value;
                        break;
                    }

                }
                if (selectedValue.toUpperCase() == "UPDATE") {
                    if (vid.value.trim() == "") {
                        alert('Enter Loading ID');
                        cnNo.value = "";
                        cnNo.focus();
                        cnNo.disabled = false;
                        return;
                    }
                }

                var message = "";

                var controlGrid = document.getElementById('<%= cnControls.ClientID %>');
                var message = "";
                for (var i = 1; i < controlGrid.rows.length; i++) {
                    var row = controlGrid.rows[i];
                    var prefix = row.cells[0].innerText;
                    var length_ = parseInt(row.cells[1].innerText);
                    if (prefix == "52190") {
                        var a = 0;
                    }
                    if (cnNo.value.substring(0, prefix.length) == prefix) {
                        if (cnNo.value.length != length_) {
                            message = "Invalid Length of CN";

                            prefixNotFound = true;
                        }
                        else {

                            prefixNotFound = false;
                            break;
                        }
                    }
                    else {
                        if (message == "") {
                            message = "Invalid Prefix";
                        }

                        prefixNotFound = true;

                    }
                }
                if (prefixNotFound) {
                    alert(message);
                    cnNo.value = "";
                    focusWorking(cnNo);
                    cnNo.disabled = false;
                    return false;
                }


                for (var i = 1; i < consignments.rows.length; i++) {
                    var tr = consignments.rows[i];
                    if (consignments.rows[i].cells[1].innerText.trim() == cnNo.value.trim() && tr.cells[7].childNodes[0].value.toString() == "CN") {
                        alert('Consignment Already Scanned');
                        cnNo.value = "";
                        focusWorking(cnNo);
                        cnNo.disabled = false;
                        return;
                    }
                }
                if (weight.value == "") {
                    weight.value = "0";
                }

                var jsonObj = { Master: {}, Consignment: {}, Bag_linked: [], Consignment_linked: [] }
                /////////////New JASON OBJ////////////////////
                var tempWeight = parseFloat(weight.value);
                var routeValue = route.options[route.options.selectedIndex].value.toString();
                var transportTypeValue = transportType.options[transportType.options.selectedIndex].value.toString();
                var destinationValue = destination.options[destination.options.selectedIndex].value.toString();
                var vehicleNoValue = vehicle.options[vehicle.options.selectedIndex].value.toString();
                var vehicleTypeValue = vehicleType.options[vehicleType.options.selectedIndex].value.toString();
                var originText = origin.options[origin.options.selectedIndex].text;
                var hd_loadingID = document.getElementById('<%= hd_loadingID.ClientID %>');
                var hd_IDChk = document.getElementById('<%= hd_IDChk.ClientID %>');
                var hd_U_ID = document.getElementById('<%=hd_U_ID.ClientID %>');
                var hd_zoneCode = document.getElementById('<%=hd_zoneCode.ClientID %>');
                var hd_branchCode = document.getElementById('<%=hd_branchCode.ClientID %>');
                var hd_expressCenterCode = document.getElementById('<%=hd_expressCenterCode.ClientID %>');
                var hd_LocationName = document.getElementById('<%=hd_LocationName.ClientID %>');
                var hd_LocationID = document.getElementById('<%=hd_LocationID.ClientID %>');
                var MasterParameters;
                MasterParameters = {
                    Mode: selectedValue,
                    Date: startDate.value,
                    Route: routeValue,
                    TransportType: transportTypeValue,
                    LoadingID: vid.value.toString(),
                    VehicleMode: vehicleMode,
                    Destination: destinationValue,
                    VehicleNo: vehicleNoValue,
                    RegNo: regNo.value.toString(),
                    FlightNo: flight.value.toString(),
                    FlightDeparture: departureDate.value.toString(),
                    VehicleType: vehicleTypeValue,
                    Description: description.value.toString(),
                    CourierName: courierName.value.toString(),
                    SealNo: seal.value.toString(),
                    TotalWeight: weight.value.toString(),
                    lbl_loadingID: hd_loadingID.innerHTML.toString(),
                    hd_IDChk: hd_IDChk.value.toString(),
                    hd_U_ID: hd_U_ID.value.toString(),
                    hd_zoneCode: hd_zoneCode.value.toString(),
                    hd_branchCode: hd_branchCode.value.toString(),
                    hd_expressCenterCode: hd_expressCenterCode.value.toString(),
                    hd_LocationName: hd_LocationName.value.toString(),
                    hd_LocationID: hd_LocationID.value.toString()
                }

                var Consignment = {
                    ConsignmentNumber: cnNo.value.trim(),
                    ServiceType: "Overnight",
                    ConsignmentType: "12",
                    Destination: destinationValue,
                    Weight: "0.5",
                    Pieces: "1",
                    Remarks: "",
                    SortOrder: consignments.rows.length.toString()
                }

                jsonObj.Consignment = Consignment;
                jsonObj.Master = MasterParameters;

                for (var i = 1; i < consignments.rows.length; i++) {
                    var Bag_linked;
                    var tr = consignments.rows[i];
                    var destinationValue = tr.cells[2].childNodes[0].options[tr.cells[2].childNodes[0].options.selectedIndex].value.toString();
                    if (tr.cells[7].childNodes[0].value.toString() != "CN") {
                        Bag_linked = {
                            BagNo: tr.cells[1].innerText.trim(),
                            Weight: tr.cells[4].childNodes[0].value.toString(),
                            Origin: tr.cells[2].childNodes[0].value.toString(),
                            Destination: tr.cells[3].childNodes[0].value.toString(),
                            SealNo: tr.cells[5].childNodes[0].value.toString(),
                            Remarks: tr.cells[6].childNodes[0].value.toString()
                        }

                        jsonObj.Bag_linked.push(Bag_linked);
                    }
                    else {
                        var destinationValue = tr.cells[2].childNodes[0].options[tr.cells[2].childNodes[0].options.selectedIndex].value;
                        var consignment_lindked = {
                            ConsignmentNumber: tr.cells[1].innerText.trim(),
                            ServiceType: 'Overnight',
                            ConsignmentType: 'Normal',
                            Destination: tr.cells[3].childNodes[0].value.toString(),
                            Weight: tr.cells[4].childNodes[0].value.toString(),
                            Pieces: tr.cells[5].childNodes[0].value.toString(),
                            Remarks: tr.cells[6].childNodes[0].value.toString()
                        }

                        jsonObj.Consignment_linked.push(consignment_lindked);
                    }
                }

                if (consignments.rows.length > 0) {


                    var saved = false;
                    $.ajax({
                        url: 'MasterAirportLoadingSpeedy_new.aspx/InsertConsignment',
                        type: 'POST',
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        data: JSON.stringify(jsonObj),
                        success: function (result) {
                            var txt_consignmentno = document.getElementById('<%= txt_bagno.ClientID %>');
                            var resp = result.d;
                            if (resp[0].toString() == "0") {
                                alert(resp[1].toString());
                                txt_consignmentno.disabled = false;

                            }
                            else if (resp[0].toString() == "ID0") {
                                alert('Kindly Refresh Page!.Press Okay. Thank You :)');
                                window.location.href = "MasterAirportLoadingSpeedy_new.aspx";
                                return;
                            }
                            else {
                                //vid.value = resp[1].toString();
                                var hdLoadingID = document.getElementById('<%= hd_loadingID.ClientID %>');
                                if (resp[1].toString() != hdLoadingID.innerText) {
                                    alert('Merging issue inserting addConsignment,Contact IT or refresh');
                                    txt_consignmentno.disabled = false;
                                }
                                vid.disabled = true;
                                saved = true;
                                if (resp[0].toString() == "1") {
                                    hd_IDChk.value = '1';
                                    AddConsignmentEdit(Consignment, "1");
                                    txt_consignmentno.disabled = false;
                                }
                                else if (resp[0].toString() == "2") {
                                    alert('Error in Saving: ' + resp[1].toString());
                                    txt_consignmentno.disabled = false;
                                }
                                //r1 = SaveLoading_Linked("0");
                                //if (r1 == "1") {
                                //    AddConsignmentEdit(Consignment, "1");
                                //}
                            }

                            //else {
                            //    //vid.value = resp[1].toString();
                            //    vid.disabled = true;
                            //    saved = true;
                            //    r1 = SaveLoading_Linked("0");
                            //    r1 = "1";
                            //    if (r1 == "1") {
                            //        AddConsignmentEdit(Consignment, "1");
                            //    }
                            //}


                        },
                        error: function () { alert('Error in Saving Consignment'); },
                        failure: function () { alert('Error Connecting to Server'); }

                    });
                    return;
                    var a = '0';
                    if (saved == false) {
                        cnNo.value = "";
                        focusWorking(cnNo);
                        txt_consignmentno.disabled = false;
                        return;
                    }

                }


                if (r1 == "1") {

                    var newTr = consignments.insertRow(1);  //document.createElement('tr');  //consignments.rows[0].cloneNode(true);
                    newTr.className = 'DetailRow';

                    var btn_remove = document.createElement('input');
                    btn_remove.type = 'button';
                    btn_remove.className = 'button button1';
                    btn_remove.value = 'Remove';
                    btn_remove.style.marginTop = '2px';
                    btn_remove.style.marginBottom = '2px';
                    btn_remove.style.width = '80%';

                    var txtWeight = document.createElement('input');
                    txtWeight.type = 'text';
                    txtWeight.className = 'textBox';
                    txtWeight.value = '0.5';
                    txtWeight.style.width = '70%';
                    txtWeight.style.textAlign = 'center';

                    var txtPieces = document.createElement('input');
                    txtPieces.type = 'text';
                    txtPieces.className = 'textBox';
                    txtPieces.value = '1';
                    txtPieces.style.width = '70%';
                    txtPieces.style.textAlign = 'center';

                    var txtremarks = document.createElement('input');
                    txtremarks.type = 'text';
                    txtremarks.className = 'textBox';
                    txtremarks.value = '';
                    txtremarks.style.width = '90%';
                    txtremarks.style.textAlign = 'center';

                    var dd_dest = GetBranchDropDown_(Br, "dd_dest");
                    dd_dest.style.width = '90%'
                    dd_dest.className = 'dropdown';
                    dd_dest.style.display = "";
                    //dd_dest.disabled = true;


                    var dd_ori = GetBranchDropDown_(Br, "dd_ori");
                    dd_ori.style.width = '90%'
                    dd_ori.className = 'dropdown';
                    dd_ori.style.display = "";
                    //dd_ori.disabled = true;

                    var txt_hiddenfield = document.createElement('input');
                    txt_hiddenfield.type = 'label';
                    txt_hiddenfield.className = 'textBox';
                    txt_hiddenfield.value = 'CN';
                    txt_hiddenfield.style.width = '90%';
                    txt_hiddenfield.style.textAlign = 'center';


                    col = newTr.insertCell(0);
                    newTr.cells[0].appendChild(btn_remove);
                    newTr.cells[0].childNodes[0].onclick = RemoveRow.bind(newTr.cells[0].childNodes[0], newTr.cells[0].childNodes[0]);
                    col = newTr.insertCell(1);

                    newTr.cells[1].innerText = cnNo.value;

                    col = newTr.insertCell(2);
                    newTr.cells[2].style.textAlign = 'center';
                    newTr.cells[2].appendChild(dd_ori);

                    for (var i = 0; i < newTr.cells[2].childNodes[0].options.length; i++) {
                        var currentDestination = origin.options[origin.options.selectedIndex].value;
                        var dd_destValue = newTr.cells[2].childNodes[0].options[i].value;
                        if (dd_destValue == currentDestination) {
                            newTr.cells[2].childNodes[0].options[i].selected = true;
                            break;
                        }
                    }

                    col = newTr.insertCell(3);
                    newTr.cells[3].style.textAlign = 'center';
                    newTr.cells[3].appendChild(dd_dest);
                    for (var i = 0; i < newTr.cells[3].childNodes[0].options.length; i++) {
                        var currentDestination = destination.options[destination.options.selectedIndex].value;
                        var dd_destValue = newTr.cells[3].childNodes[0].options[i].value;
                        if (dd_destValue == currentDestination) {
                            newTr.cells[3].childNodes[0].options[i].selected = true;
                            break;
                        }
                    }


                    col = newTr.insertCell(4);
                    newTr.cells[4].appendChild(txtWeight);
                    newTr.cells[4].style.textAlign = 'center';

                    col = newTr.insertCell(5);
                    newTr.cells[5].appendChild(txtPieces);
                    newTr.cells[5].style.textAlign = "center";

                    col = newTr.insertCell(6);
                    newTr.cells[6].style.textAlign = "center";
                    newTr.cells[6].appendChild(txtremarks);

                    col = newTr.insertCell(7);
                    newTr.cells[7].style.textAlign = "center";
                    newTr.cells[7].appendChild(txt_hiddenfield);

                    cnNo.value = '';
                    focusWorking(cnNo);

                    CalculateItems();
                    CalculateTotalWeight();
                }
            }



            function AddConsignment_(consignmentno) {
                var mode = document.getElementById('<%= rbtn_mode.ClientID %>');
                var route = document.getElementById('<%= dd_route.ClientID %>');
                var startDate = document.getElementById('<%= dd_start_date.ClientID %>');
                var touchPoint = document.getElementById('<%= dd_touchpoint.ClientID %>');
                var vid = document.getElementById('<%= txt_vid.ClientID %>');
                var transportType = document.getElementById('<%= dd_transporttype.ClientID %>');
                var origin = document.getElementById('<%= dd_orign.ClientID %>');
                var flight = document.getElementById('<%= txt_flight.ClientID %>');
                var departureDate = document.getElementById('<%= dept_date.ClientID %>');
                var rVehicle = document.getElementById('Vehicle');
                var rRented = document.getElementById('Rented');
                var destination = document.getElementById('<%= dd_destination.ClientID %>');
                var vehicle = document.getElementById('<%= dd_vehicle.ClientID %>');
                var regNo = document.getElementById('<%= txt_reg.ClientID %>');
                var vehicleType = document.getElementById('<%= dd_vehicleType.ClientID %>');
                var description = document.getElementById('<%= txt_description.ClientID %>');
            var courierName = document.getElementById('<%= txt_couriername.ClientID %>');
            var seal = document.getElementById('<%= txt_seal.ClientID %>');
            var weight = document.getElementById('<%= txt_totalLoadWeight.ClientID %>');

            var cnNo = document.getElementById('<%= txt_bagno.ClientID %>');
            var consignments = document.getElementById('tbl_detailsBag');

            if (destination.options.selectedIndex == -1) {
                alert('Select Route');
                focusWorking(route);
                cnNo.value = "";
                cnNo.disabled = false;
                return;
            }

            var vehicleMode = 0;
            if (rRented.checked) {
                vehicleMode = 'r';
            }
            else if (rVehicle.checked) {
                vehicleMode = 'v';
            }
            if (vehicleMode == 'v') {

                if (vehicle.options.selectedIndex == "0") {
                    alert('Select vehicle');
                    focusWorking(vehicle);
                    cnNo.value = "";
                    cnNo.disabled = false;
                    return;
                }

            }

            if (vehicleType.options[vehicleType.options.selectedIndex].value.toString() == "") {
                alert('Select Vehicle type');
                cnNo.disabled = false;
                return;

            }


            if (cnNo.value.trim() == "") {
                alert('Enter a Valid Consignment Number');
                cnNo.disabled = false;
                return;
            }

            var inputs = mode.getElementsByTagName('input');

            var selectedValue = "";
            for (var i = 0; i < inputs.length; i++) {
                if (inputs[i].checked) {
                    selectedValue = inputs[i].value;
                    break;
                }

            }
            if (selectedValue.toUpperCase() == "UPDATE") {
                if (vid.value.trim() == "") {
                    alert('Enter Loading ID');
                    cnNo.value = "";
                    cnNo.focus();
                    cnNo.disabled = false;
                    return;
                }
            }

            var message = "";

            var controlGrid = document.getElementById('<%= cnControls.ClientID %>');
            var message = "";
            for (var i = 1; i < controlGrid.rows.length; i++) {
                var row = controlGrid.rows[i];
                var prefix = row.cells[0].innerText;
                var length_ = parseInt(row.cells[1].innerText);
                if (prefix == "52190") {
                    var a = 0;
                }
                if (cnNo.value.substring(0, prefix.length) == prefix) {
                    if (cnNo.value.length != length_) {
                        message = "Invalid Length of CN";

                        prefixNotFound = true;
                    }
                    else {

                        prefixNotFound = false;
                        break;
                    }
                }
                else {
                    if (message == "") {
                        message = "Invalid Prefix";
                    }

                    prefixNotFound = true;

                }
            }
            if (prefixNotFound) {
                alert(message);
                cnNo.value = "";
                focusWorking(cnNo);
                cnNo.disabled = false;
                return false;
            }


            for (var i = 1; i < consignments.rows.length; i++) {
                var tr = consignments.rows[i];
                if (consignments.rows[i].cells[1].innerText.trim() == cnNo.value.trim() && tr.cells[7].childNodes[0].value.toString() == "CN") {
                    alert('Consignment Already Scanned');
                    cnNo.value = "";
                    focusWorking(cnNo);
                    cnNo.disabled = false;
                    return;
                }
            }
            if (weight.value == "") {
                weight.value = consignmentno[0][3].toString();
            }




            var vehicleMode = 0;
            if (rRented.checked) {
                vehicleMode = 'r';
            }
            else if (rVehicle.checked) {
                vehicleMode = 'v';
            }
            var jsonObj = { Master: {}, Consignment: {}, Bag_linked: [], Consignment_linked: [] }
            /////////////New JASON OBJ////////////////////                
            var tempWeight = weight.value; //parseFloat(weight.value);
            var routeValue = route.options[route.options.selectedIndex].value;
            var transportTypeValue = transportType.options[transportType.options.selectedIndex].value;
            var destinationValue = destination.options[destination.options.selectedIndex].value;
            var vehicleNoValue = vehicle.options[vehicle.options.selectedIndex].value;
            var vehicleTypeValue = vehicleType.options[vehicleType.options.selectedIndex].value;
            var originText = origin.options[origin.options.selectedIndex].text;
            var hd_loadingID = document.getElementById('<%= hd_loadingID.ClientID %>');
            var hd_IDChk = document.getElementById('<%= hd_IDChk.ClientID %>');
            var hd_U_ID = document.getElementById('<%=hd_U_ID.ClientID %>');
            var hd_zoneCode = document.getElementById('<%=hd_zoneCode.ClientID %>');
            var hd_branchCode = document.getElementById('<%=hd_branchCode.ClientID %>');
            var hd_expressCenterCode = document.getElementById('<%=hd_expressCenterCode.ClientID %>');
            var hd_LocationName = document.getElementById('<%=hd_LocationName.ClientID %>');
            var hd_LocationID = document.getElementById('<%=hd_LocationID.ClientID %>');
            var MasterParameters;
            MasterParameters = {
                Mode: selectedValue,
                Date: startDate.value,
                Route: routeValue,
                TransportType: transportTypeValue,
                LoadingID: vid.value.toString(),
                VehicleMode: vehicleMode,
                Destination: destinationValue,
                VehicleNo: vehicleNoValue,
                RegNo: regNo.value.toString(),
                FlightNo: flight.value.toString(),
                FlightDeparture: departureDate.value.toString(),
                VehicleType: vehicleTypeValue,
                Description: description.value.toString(),
                CourierName: courierName.value.toString(),
                SealNo: seal.value.toString(),
                TotalWeight: weight.value.toString(),
                lbl_loadingID: hd_loadingID.innerHTML.toString(),
                hd_IDChk: hd_IDChk.value.toString(),
                hd_U_ID: hd_U_ID.value.toString(),
                hd_zoneCode: hd_zoneCode.value.toString(),
                hd_branchCode: hd_branchCode.value.toString(),
                hd_expressCenterCode: hd_expressCenterCode.value.toString(),
                hd_LocationName: hd_LocationName.value.toString(),
                hd_LocationID: hd_LocationID.value.toString()
            }

            var Consignment = {
                ConsignmentNumber: cnNo.value.trim(),
                ServiceType: "Overnight",
                ConsignmentType: "12",
                Destination: consignmentno[0][2].toString(),
                Weight: consignmentno[0][3].toString(),
                Pieces: consignmentno[0][4].toString(),
                Remarks: "",
                SortOrder: consignments.rows.length.toString()
            }

            jsonObj.Consignment = Consignment;
            jsonObj.Master = MasterParameters;

            for (var i = 1; i < consignments.rows.length; i++) {
                var Bag_linked;
                var tr = consignments.rows[i];
                var destinationValue = tr.cells[2].childNodes[0].options[tr.cells[2].childNodes[0].options.selectedIndex].value.toString();
                if (tr.cells[7].childNodes[0].value.toString() != "CN") {
                    Bag_linked = {
                        BagNo: tr.cells[1].innerText.trim(),
                        Weight: tr.cells[4].childNodes[0].value.toString(),
                        Origin: tr.cells[2].childNodes[0].value.toString(),
                        Destination: tr.cells[3].childNodes[0].value.toString(),
                        SealNo: tr.cells[5].childNodes[0].value.toString(),
                        Remarks: tr.cells[6].childNodes[0].value.toString()
                    }

                    jsonObj.Bag_linked.push(Bag_linked);
                }
                else {
                    var destinationValue = tr.cells[2].childNodes[0].options[tr.cells[2].childNodes[0].options.selectedIndex].value;
                    var consignment_lindked = {
                        ConsignmentNumber: tr.cells[1].innerText.trim(),
                        ServiceType: 'Overnight',
                        ConsignmentType: 'Normal',
                        Destination: tr.cells[3].childNodes[0].value.toString(),
                        Weight: tr.cells[4].childNodes[0].value.toString(),
                        Pieces: tr.cells[5].childNodes[0].value.toString(),
                        Remarks: tr.cells[6].childNodes[0].value.toString()
                    }

                    jsonObj.Consignment_linked.push(consignment_lindked);
                }
            }
            if (consignments.rows.length > 0) {
                var saved = false;
                $.ajax({
                    url: 'MasterAirportLoadingSpeedy_new.aspx/InsertConsignment',
                    type: 'POST',
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    data: JSON.stringify(jsonObj),
                    success: function (result) {
                        var txt_consignmentno = document.getElementById('<%= txt_bagno.ClientID %>');
                        var resp = result.d;
                        if (resp[0].toString() == "0") {
                            alert(resp[1].toString());
                            txt_consignmentno.disabled = false;

                        }
                        else if (resp[0].toString() == "ID0") {
                            alert('Kindly Refresh Page!.Press Okay. Thank You :)');
                            window.location.href = "MasterAirportLoadingSpeedy_new.aspx";
                            return;
                        }
                        else {
                            var hdLoadingID = document.getElementById('<%= hd_loadingID.ClientID %>');
                                if (resp[1].toString() != hdLoadingID.innerText) {
                                    alert('check Loading ID');
                                    txt_consignmentno.disabled = false;
                                }
                                vid.disabled = true;
                                saved = true;
                                if (resp[0].toString() == "1") {
                                    hd_IDChk.value = '1';
                                    AddConsignmentEdit(Consignment, "1");
                                    txt_consignmentno.disabled = false;
                                }
                                else if (resp[0].toString() == "2") {
                                    alert('Error in Saving: ' + resp[1].toString());
                                    txt_consignmentno.disabled = false;
                                }
                            //r1 = SaveLoading_Linked("0");
                            //if (r1 == "1") {
                            //    AddConsignmentEdit(Consignment, "1");
                            //}
                            }
                        //else {
                        //    //vid.value = resp[1].toString();
                        //    vid.disabled = true;
                        //    saved = true;
                        //    r1 = SaveLoading_Linked("0");
                        //    r1 = "1";
                        //    if (r1 == "1") {
                        //        AddConsignmentEdit(Consignment, "1");
                        //    }
                        //}
                    },
                    error: function () { alert('Error in Saving Consignment'); },
                    failure: function () { alert('Error Connecting to Server'); }

                });
                return;
                var a = '0';
                if (saved == false) {
                    cnNo.value = "";
                    focusWorking(cnNo);
                    txt_consignmentno.disabled = false;
                    return;
                }

            }


            if (r1 == "1") {

                var newTr = consignments.insertRow(1);  //document.createElement('tr');  //consignments.rows[0].cloneNode(true);
                newTr.className = 'DetailRow';

                var btn_remove = document.createElement('input');
                btn_remove.type = 'button';
                btn_remove.className = 'button button1';
                btn_remove.value = 'Remove';
                btn_remove.style.marginTop = '2px';
                btn_remove.style.marginBottom = '2px';
                btn_remove.style.width = '80%';

                var txtWeight = document.createElement('input');
                txtWeight.type = 'text';
                txtWeight.className = 'textBox';
                txtWeight.value = '0.5';
                txtWeight.style.width = '70%';
                txtWeight.style.textAlign = 'center';

                var txtPieces = document.createElement('input');
                txtPieces.type = 'text';
                txtPieces.className = 'textBox';
                txtPieces.value = '1';
                txtPieces.style.width = '70%';
                txtPieces.style.textAlign = 'center';

                var txtremarks = document.createElement('input');
                txtremarks.type = 'text';
                txtremarks.className = 'textBox';
                txtremarks.value = '';
                txtremarks.style.width = '90%';
                txtremarks.style.textAlign = 'center';

                var dd_dest = GetBranchDropDown_(Br, "dd_dest");
                dd_dest.style.width = '90%'
                dd_dest.className = 'dropdown';
                dd_dest.style.display = "";
                //dd_dest.disabled = true;


                var dd_ori = GetBranchDropDown_(Br, "dd_ori");
                dd_ori.style.width = '90%'
                dd_ori.className = 'dropdown';
                dd_ori.style.display = "";
                //dd_ori.disabled = true;

                var txt_hiddenfield = document.createElement('input');
                txt_hiddenfield.type = 'label';
                txt_hiddenfield.value = 'CN';
                txt_hiddenfield.style.width = '100%';
                txt_hiddenfield.style.textAlign = 'center';


                col = newTr.insertCell(0);
                newTr.cells[0].appendChild(btn_remove);
                newTr.cells[0].childNodes[0].onclick = RemoveRow.bind(newTr.cells[0].childNodes[0], newTr.cells[0].childNodes[0]);
                col = newTr.insertCell(1);

                newTr.cells[1].innerText = cnNo.value;

                col = newTr.insertCell(2);
                newTr.cells[2].style.textAlign = 'center';
                newTr.cells[2].appendChild(dd_ori);

                for (var i = 0; i < newTr.cells[2].childNodes[0].options.length; i++) {
                    var currentDestination = consignmentno[0][1].toString(); //origin.options[origin.options.selectedIndex].value;
                    var dd_destValue = newTr.cells[2].childNodes[0].options[i].value;
                    if (dd_destValue == currentDestination) {
                        newTr.cells[2].childNodes[0].options[i].selected = true;
                        break;
                    }
                }

                col = newTr.insertCell(3);
                newTr.cells[3].style.textAlign = 'center';
                newTr.cells[3].appendChild(dd_dest);
                for (var i = 0; i < newTr.cells[3].childNodes[0].options.length; i++) {
                    var currentDestination = consignmentno[0][2].toString(); //destination.options[destination.options.selectedIndex].value;
                    var dd_destValue = newTr.cells[3].childNodes[0].options[i].value;
                    if (dd_destValue == currentDestination) {
                        newTr.cells[3].childNodes[0].options[i].selected = true;
                        break;
                    }
                }

                col = newTr.insertCell(4);
                newTr.cells[4].appendChild(txtWeight);
                newTr.cells[4].style.textAlign = 'center';
                newTr.cells[4].childNodes[0].onchange = CalculateTotalWeight.bind(newTr.cells[4].childNodes[0]);
                newTr.cells[4].childNodes[0].maxLength = "5";

                col = newTr.insertCell(5);
                newTr.cells[5].appendChild(txtseal);
                newTr.cells[5].style.textAlign = "center";

                col = newTr.insertCell(6);
                newTr.cells[6].appendChild(txtremarks);

                col = newTr.insertCell(7);
                newTr.cells[7].style.textAlign = "center";
                newTr.cells[7].appendChild(txt_hiddenfield);

                cnNo.value = '';
                focusWorking(cnNo);

                CalculateItems();
                CalculateTotalWeight();
            }
        }

        function AddConsignmentEdit(SingleCN, AddMode) {
            var mode = document.getElementById('<%= rbtn_mode.ClientID %>');
                var route = document.getElementById('<%= dd_route.ClientID %>');
                var startDate = document.getElementById('<%= dd_start_date.ClientID %>');
                var touchPoint = document.getElementById('<%= dd_touchpoint.ClientID %>');
                var vid = document.getElementById('<%= txt_vid.ClientID %>');
                var transportType = document.getElementById('<%= dd_transporttype.ClientID %>');
                var origin = document.getElementById('<%= dd_orign.ClientID %>');
                var flight = document.getElementById('<%= txt_flight.ClientID %>');
                var departureDate = document.getElementById('<%= dept_date.ClientID %>');
                var rVehicle = document.getElementById('Vehicle');
                var rRented = document.getElementById('Rented');
                var destination = document.getElementById('<%= dd_destination.ClientID %>');
                var vehicle = document.getElementById('<%= dd_vehicle.ClientID %>');
                var regNo = document.getElementById('<%= txt_reg.ClientID %>');
                var vehicleType = document.getElementById('<%= dd_vehicleType.ClientID %>');
                var description = document.getElementById('<%= txt_description.ClientID %>');
                var courierName = document.getElementById('<%= txt_couriername.ClientID %>');
                var seal = document.getElementById('<%= txt_seal.ClientID %>');
                var weight = document.getElementById('<%= txt_totalLoadWeight.ClientID %>');

                var cnNo = document.getElementById('<%= txt_bagno.ClientID %>');
                var consignments = document.getElementById('tbl_detailsBag');

                if (destination.options.selectedIndex == -1) {
                    alert('Select Route');

                    focusWorking(route);
                    cnNo.value = "";
                    return;
                }

                if (rVehicle.checked) {
                    if (vehicle.options.selectedIndex == "0" && regNo.value == "") {
                        alert('Select vehicle');
                        focusWorking(vehicle);
                        cnNo.value = "";
                        return;
                    }
                }

                if (vehicleType.options[vehicleType.options.selectedIndex].value.toString() == "") {
                    alert('Select Vehicle type');
                    return;

                }
                cnNo.value = SingleCN.ConsignmentNumber;
                if (cnNo.value.trim() == "") {
                    alert('Enter a Valid Consignment Number');
                    return;
                }

                var inputs = mode.getElementsByTagName('input');

                var selectedValue = "";
                for (var i = 0; i < inputs.length; i++) {
                    if (inputs[i].checked) {
                        selectedValue = inputs[i].value;
                        break;
                    }

                }
                if (selectedValue.toUpperCase() == "UPDATE") {
                    if (vid.value.trim() == "") {
                        alert('Enter Loading ID');
                        cn.value = "";
                        cn.focus();
                        return;
                    }
                }

                var message = "";
                for (var i = 1; i < consignments.rows.length; i++) {

                    if (consignments.rows[i].cells[1].innerText.trim() == cnNo.value.trim()) {
                        alert('Consignment Already Scanned');
                        cnNo.value = "";
                        focusWorking(cnNo);
                        cnNo.disabled = false;
                        return;
                    }
                }
                if (weight.value == "") {
                    weight.value = "0";
                }
                var tempWeight = parseFloat(weight.value);
                var newTr = consignments.insertRow(1);  //document.createElement('tr');  //consignments.rows[0].cloneNode(true);
                newTr.className = 'DetailRow';
                newTr.id = "CN-" + consignments.rows.length;

                var btn_remove = document.createElement('input');
                btn_remove.type = 'button';
                btn_remove.className = 'button button1';
                btn_remove.value = 'Remove';
                btn_remove.style.marginTop = '2px';
                btn_remove.style.marginBottom = '2px';
                btn_remove.style.width = '80%';

                var txtWeight = document.createElement('input');
                txtWeight.type = 'text';
                txtWeight.className = 'textBox';
                txtWeight.value = '0.5';
                txtWeight.style.width = '70%';
                txtWeight.style.textAlign = 'center';

                var txtPieces = document.createElement('input');
                txtPieces.type = 'text';
                txtPieces.className = 'textBox';
                txtPieces.value = '1';
                txtPieces.style.width = '70%';
                txtPieces.style.textAlign = 'center';

                var txtremarks = document.createElement('input');
                txtremarks.type = 'text';
                txtremarks.className = 'textBox';
                txtremarks.value = '';
                txtremarks.style.width = '90%';
                txtremarks.style.textAlign = 'center';

                var txt_hiddenfield = document.createElement('input');
                txt_hiddenfield.type = 'label';
                txt_hiddenfield.className = 'textBox';
                txt_hiddenfield.value = 'CN';
                txt_hiddenfield.style.width = '90%';
                txt_hiddenfield.style.textAlign = 'center';

                var dd_dest = GetBranchDropDown_(Br, "dd_dest");
                dd_dest.style.width = '90%'
                dd_dest.className = 'dropdown';
                dd_dest.style.display = "";
                //dd_dest.disabled = true;


                var dd_ori = GetBranchDropDown_(Br, "dd_ori");
                dd_ori.style.width = '90%'
                dd_ori.className = 'dropdown';
                dd_ori.style.display = "";
                //dd_ori.disabled = true;

                var txt_hiddenfield = document.createElement('input');
                txt_hiddenfield.type = 'label';
                txt_hiddenfield.value = 'CN';
                txt_hiddenfield.style.width = '100%';
                txt_hiddenfield.style.textAlign = 'center';

                col = newTr.insertCell(0);
                if (selectedValue.toUpperCase() == "NEW" || AddMode == "1") {
                    newTr.cells[0].appendChild(btn_remove);
                    newTr.cells[0].childNodes[0].onclick = RemoveRow.bind(newTr.cells[0].childNodes[0], newTr.cells[0].childNodes[0]);
                }



                col = newTr.insertCell(1);
                newTr.cells[1].innerText = cnNo.value.trim();

                col = newTr.insertCell(2);
                newTr.cells[2].style.textAlign = 'center';
                newTr.cells[2].appendChild(dd_ori);

                for (var i = 0; i < newTr.cells[2].childNodes[0].options.length; i++) {
                    var currentDestination = origin.options[origin.options.selectedIndex].value;
                    var dd_destValue = newTr.cells[2].childNodes[0].options[i].value;
                    if (dd_destValue == currentDestination) {
                        newTr.cells[2].childNodes[0].options[i].selected = true;
                        break;
                    }
                }

                col = newTr.insertCell(3);
                newTr.cells[3].style.textAlign = 'center';
                newTr.cells[3].appendChild(dd_dest);
                for (var i = 0; i < newTr.cells[3].childNodes[0].options.length; i++) {
                    var currentDestination = SingleCN.Destination; //destination.options[destination.options.selectedIndex].value;
                    var dd_destValue = newTr.cells[3].childNodes[0].options[i].value;
                    if (dd_destValue == currentDestination) {
                        newTr.cells[3].childNodes[0].options[i].selected = true;
                        break;
                    }
                }

                col = newTr.insertCell(4);
                newTr.cells[4].appendChild(txtWeight);
                newTr.cells[4].style.textAlign = 'center';
                newTr.cells[4].childNodes[0].onchange = CalculateTotalWeight.bind(newTr.cells[4].childNodes[0]);
                newTr.cells[4].childNodes[0].maxLength = "5";
                txtWeight.value = SingleCN.Weight;

                col = newTr.insertCell(5);
                newTr.cells[5].appendChild(txtPieces);
                newTr.cells[5].style.textAlign = "center";
                txtPieces.value = SingleCN.Pieces;

                col = newTr.insertCell(6);
                newTr.cells[6].appendChild(txtremarks);
                txtremarks.value = SingleCN.Remarks;

                col = newTr.insertCell(7);
                newTr.cells[7].style.textAlign = "center";
                newTr.cells[7].appendChild(txt_hiddenfield);
                cnNo.value = '';
                focusWorking(cnNo);

                CalculateItems();
                CalculateTotalWeight();
            }
        </script>
        <%--Removing Item from Table--%>
        <script type="text/javascript">
            function RemoveRow(btn) {
                var tr = btn.parentElement.parentElement;
                var hd_loadingID = document.getElementById('<%=  hd_loadingID.ClientID %>');
                var LoadingID = hd_loadingID.innerText;
                var txt_vid = document.getElementById('<%=  txt_vid.ClientID %>');
                var BagNumber = "";
                var ConsignmentNumber = "";
                var Removethis = "";
                if (LoadingID.trim() != "") {

                    if (tr.id.toString().split('-')[0].toUpperCase() == "BAG") {
                        Removethis = "BAG"
                        ConsignmentNumber = "";
                        BagNumber = tr.cells[1].innerText;
                    }
                    else if (tr.id.toString().split('-')[0].toUpperCase() == "CN") {
                        Removethis = "CN";
                        BagNumber = "";
                        ConsignmentNumber = tr.cells[1].innerText;
                    }

                    var cn = tr.cells[1].innerText;

                    var table = tr.parentElement;

                    $.ajax({
                        url: 'MasterAirportLoadingSpeedy_new.aspx/RemoveRow',
                        type: 'POST',
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        data: "{'lbl_loadingID' : '" + LoadingID + "', 'BagNumber' : '" + BagNumber + "', 'ConsignmentNumber' : '" + ConsignmentNumber + "', 'Remove' : '" + Removethis + "', 'LoadingID_Issue' : '" + txt_vid.value + "'}",
                        success: function (result) {
                            if (result.d.toString() != "OK") {
                                alert('Could Not Remove ' + Removethis);
                            }
                            else {
                                table.deleteRow(tr.rowIndex);
                                CalculateItems();
                                CalculateTotalWeight();
                            }

                        },
                        error: function () { alert(Removethis + 'Could not be deleted'); },
                        failure: function () { alert(Removethis + 'Could not be deleted'); }
                    });
                }
                else {
                    table.deleteRow(tr.rowIndex);
                    CalculateItems();
                    CalculateTotalWeight();
                }


            }

        </script>
        <%--Calculating Total Weight--%>
        <script type="text/javascript">
            function CalculateTotalWeight() {
                var bags = document.getElementById('tbl_detailsBag');
                var btn_save = document.getElementById('btn_save');
                var btn_save_ = document.getElementById('btn_save_');
                var txt_bagno = document.getElementById('<%= txt_bagno.ClientID %>');
                var chk = false;


                var validNumber = new RegExp(/^\d+(\.\d+)?$/);

                for (var i = 1; i < bags.rows.length; i++) {
                    if (validNumber.test(bags.rows[i].cells[4].childNodes[0].value) && (bags.rows[i].cells[4].childNodes[0].value != "0")) {
                        bags.rows[i].cells[4].childNodes[0].style.backgroundColor = "";
                    }
                    else {
                        bags.rows[i].cells[4].childNodes[0].style.backgroundColor = "red";
                        chk = true;
                    }
                }

                if (chk) {
                    btn_save.disabled = true;
                    txt_bagno.disabled = true;
                    btn_save_.disabled = true;
                    alert('Kindly insert proper weight');
                    return;
                }
                else {
                    btn_save.disabled = false;
                    txt_bagno.disabled = false;
                    btn_save_.disabled = false;
                }



                var totalWeight = 0;
                var tempCnWeight = 0;
                var tempBagWeight = 0;

                for (var i = 1; i < bags.rows.length; i++) {
                    var temp = parseFloat(bags.rows[i].cells[4].childNodes[0].value);
                    if (!isNaN(temp)) {
                        tempBagWeight = tempBagWeight + temp;
                    }
                }



                document.getElementById('<%= txt_totalLoadWeight.ClientID %>').value = (tempCnWeight + tempBagWeight).toString();
            }
        </script>
        <%--Calculting Total Items--%>
        <script type="text/javascript">
            function CalculateItems() {
                var bags = document.getElementById('tbl_detailsBag');
                var cns = document.getElementById('tbl_detailsCN');
                var totalCount = document.getElementById('lbl_count');
                var cnCount = document.getElementById('lbl_cnCount');
                var bagCount = document.getElementById('lbl_bagCount');

                // bagCount.innerText = 'Total : ' + (bags.rows.length - 1).toString();

                totalCount.innerText = 'Total Scanned Items: ' + ((bags.rows.length - 1)).toString();

            }
        </script>
        <%--Creating DropDowns for Origin and Destination--%>
        <script type="text/javascript">
            function GetBranchDropDown() {
                var sourceDropdown = document.getElementById('branches');

                var currdd = document.createElement('select');
                currdd.id = 'dd_dest';
                for (var i = 0; i < sourceDropdown.options.length; i++) {
                    var option = document.createElement('option');
                    option.text = sourceDropdown.options[i].text;
                    option.value = sourceDropdown.options[i].value;

                    currdd.add(option);
                }
                return currdd;
            }

            function GetBranchDropDown_(a, b) {
                var sourceDropdown = a; //document.getElementById('branches');

                var currdd = document.createElement('select');
                currdd.id = b;
                for (var i = 0; i < sourceDropdown.options.length; i++) {
                    var option = document.createElement('option');
                    option.text = sourceDropdown.options[i].text;
                    option.value = sourceDropdown.options[i].value;

                    currdd.add(option);
                }
                return currdd;
            }


        </script>
        <%--To Focus on any Control--%>
        <script type="text/javascript">
            function focusWorking(cnt) {
                var id = '#' + cnt.id.toString();
                $(document).ready(function () {
                    setTimeout(function () { $(id).focus(); }, 1);
                });
            }
        </script>
        <%--Saving Loading--%>
        <script type="text/javascript">
            function SaveLoading() {
                var mode = document.getElementById('<%= rbtn_mode.ClientID %>');
                var route = document.getElementById('<%= dd_route.ClientID %>');
                var startDate = document.getElementById('<%= dd_start_date.ClientID %>');
                var touchPoint = document.getElementById('<%= dd_touchpoint.ClientID %>');
                var vid = document.getElementById('<%= txt_vid.ClientID %>');
                var transportType = document.getElementById('<%= dd_transporttype.ClientID %>');
                var origin = document.getElementById('<%= dd_orign.ClientID %>');
                var flight = document.getElementById('<%= txt_flight.ClientID %>');
                var departureDate = document.getElementById('<%= dept_date.ClientID %>');
                var rVehicle = document.getElementById('Vehicle');
                var rRented = document.getElementById('Rented');
                var destination = document.getElementById('<%= dd_destination.ClientID %>');
                var vehicle = document.getElementById('<%= dd_vehicle.ClientID %>');
                var regNo = document.getElementById('<%= txt_reg.ClientID %>');
                var vehicleType = document.getElementById('<%= dd_vehicleType.ClientID %>');
                var description = document.getElementById('<%= txt_description.ClientID %>');
                var courierName = document.getElementById('<%= txt_couriername.ClientID %>');
                var seal = document.getElementById('<%= txt_seal.ClientID %>');
                var weight = document.getElementById('<%= txt_totalLoadWeight.ClientID %>');
                var hd_IDChk = document.getElementById('<%= hd_IDChk.ClientID %>');
                var bagNo = document.getElementById('<%= txt_bagno.ClientID %>');
                var dd_destination = document.getElementById('<%= dd_destination.ClientID %>');
                var dd_route = document.getElementById('<%= dd_route.ClientID %>');

                var bags = document.getElementById('tbl_detailsBag');

                if ((bags.rows.length - 1) == 0) {
                    alert("Cannot Save Loading");
                    return;
                }
                if (hd_IDChk.value.toString() != '1') {
                    alert('Kindly insert Details First');
                    return;
                }
                if (dd_route.selectedIndex == 0) {
                    alert('Select Route');
                    focusWorking(route);
                    return;
                }
                if (dd_destination.options[dd_destination.selectedIndex].value == -1) {
                    alert('Select Route');
                    focusWorking(route);
                    return;
                }

                var inputs = mode.getElementsByTagName('input');

                var selectedValue = "";
                for (var i = 0; i < inputs.length; i++) {
                    if (inputs[i].checked) {
                        selectedValue = inputs[i].value;
                        break;
                    }

                }
                var vehicleMode = 0;
                if (rRented.checked) {
                    vehicleMode = 'r';
                }
                else if (rVehicle.checked) {
                    vehicleMode = 'v';
                }
                var jsonObj = { Master: {}, Bags: [], Consignments: [] }
                var routeValue = route.options[route.options.selectedIndex].value.toString();
                var transportTypeValue = transportType.options[transportType.options.selectedIndex].value.toString();
                var destinationValue = destination.options[destination.options.selectedIndex].value.toString()
                var vehicleNoValue = vehicle.options[vehicle.options.selectedIndex].value.toString();
                var vehicleTypeValue = vehicleType.options[vehicleType.options.selectedIndex].value.toString();
                var hd_loadingID = document.getElementById('<%=hd_loadingID.ClientID %>');
                var hd_IDChk = document.getElementById('<%= hd_IDChk.ClientID %>');
                var hd_U_ID = document.getElementById('<%=hd_U_ID.ClientID %>');
                var hd_zoneCode = document.getElementById('<%=hd_zoneCode.ClientID %>');
                var hd_branchCode = document.getElementById('<%=hd_branchCode.ClientID %>');
                var hd_expressCenterCode = document.getElementById('<%=hd_expressCenterCode.ClientID %>');
                var hd_LocationName = document.getElementById('<%=hd_LocationName.ClientID %>');
                var hd_LocationID = document.getElementById('<%=hd_LocationID.ClientID %>');
                var MasterParameters;
                MasterParameters = {
                    Mode: selectedValue,
                    Date: startDate.value,
                    Route: routeValue,
                    TransportType: transportTypeValue,
                    LoadingID: vid.value.toString(),
                    VehicleMode: vehicleMode,
                    Destination: destinationValue,
                    VehicleNo: vehicleNoValue,
                    RegNo: regNo.value.toString(),
                    FlightNo: flight.value.toString(),
                    FlightDeparture: departureDate.value.toString(),
                    VehicleType: vehicleTypeValue,
                    Description: description.value.toString(),
                    CourierName: courierName.value.toString(),
                    SealNo: seal.value.toString(),
                    TotalWeight: weight.value.toString(),
                    lbl_loadingID: hd_loadingID.innerHTML.toString(),
                    hd_U_ID: hd_U_ID.value.toString(),
                    hd_zoneCode: hd_zoneCode.value.toString(),
                    hd_branchCode: hd_branchCode.value.toString(),
                    hd_expressCenterCode: hd_expressCenterCode.value.toString(),
                    hd_LocationName: hd_LocationName.value.toString(),
                    hd_LocationID: hd_LocationID.value.toString()
                }
                jsonObj.Master = MasterParameters;

                for (var i = 1; i < bags.rows.length; i++) {
                    var bag;
                    var tr = bags.rows[i];
                    var destinationValue = tr.cells[2].childNodes[0].options[tr.cells[2].childNodes[0].options.selectedIndex].value.toString();
                    if (tr.cells[7].childNodes[0].value.toString() != "CN") {
                        bag = {
                            BagNo: tr.cells[1].innerText.trim(),
                            Weight: tr.cells[4].childNodes[0].value.toString(),
                            Origin: tr.cells[2].childNodes[0].value.toString(),
                            Destination: tr.cells[3].childNodes[0].value.toString(),
                            SealNo: tr.cells[5].childNodes[0].value.toString(),
                            Remarks: tr.cells[6].childNodes[0].value.toString()
                        }

                        jsonObj.Bags.push(bag);
                    }
                    else {
                        var destinationValue = tr.cells[2].childNodes[0].options[tr.cells[2].childNodes[0].options.selectedIndex].value;
                        consignment = {
                            ConsignmentNumber: tr.cells[1].innerText.trim(),
                            ServiceType: 'Overnight',
                            ConsignmentType: 'Normal',
                            Destination: tr.cells[3].childNodes[0].value.toString(),
                            Weight: tr.cells[4].childNodes[0].value.toString(),
                            Pieces: tr.cells[5].childNodes[0].value.toString(),
                            Remarks: tr.cells[6].childNodes[0].value.toString()
                        }

                        jsonObj.Consignments.push(consignment);



                    }
                }



                $.ajax({

                    url: 'MasterAirportLoadingSpeedy_new.aspx/InsertLoading',
                    type: 'POST',
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    data: JSON.stringify(jsonObj),
                    success: function (result) {
                        var resp = result.d;
                        if (resp[0] == "1") {
                            //window.open("LoadingPrint.aspx?Xcode=" + result[1].toString(), '_blank');
                            window.open('LoadingPrintNew.aspx?XCode=' + resp[1].toString(), '_blank');
                            ResetAll();
                            window.open('MasterAirportLoadingSpeedy_New.aspx', '_self');
                        }
                        else {
                            alert('Error in Loading: ' + resp[1].toString());
                        }
                    },

                    error: function () {
                        alert('Error in Generating AirportArrival');

                    },
                    failure: function () {
                        alert('Failed to Generate AirportArrival');


                    }


                });
            }

            function SaveLoading_Linked(curr) {
                var mode = document.getElementById('<%= rbtn_mode.ClientID %>');
                var route = document.getElementById('<%= dd_route.ClientID %>');
                var startDate = document.getElementById('<%= dd_start_date.ClientID %>');
                var touchPoint = document.getElementById('<%= dd_touchpoint.ClientID %>');
                var vid = document.getElementById('<%= txt_vid.ClientID %>');
                var transportType = document.getElementById('<%= dd_transporttype.ClientID %>');
                var origin = document.getElementById('<%= dd_orign.ClientID %>');
                var flight = document.getElementById('<%= txt_flight.ClientID %>');
                var departureDate = document.getElementById('<%= dept_date.ClientID %>');
                var rVehicle = document.getElementById('Vehicle');
                var rRented = document.getElementById('Rented');
                var destination = document.getElementById('<%= dd_destination.ClientID %>');
                var vehicle = document.getElementById('<%= dd_vehicle.ClientID %>');
                var regNo = document.getElementById('<%= txt_reg.ClientID %>');
                var vehicleType = document.getElementById('<%= dd_vehicleType.ClientID %>');
                var description = document.getElementById('<%= txt_description.ClientID %>');
                var courierName = document.getElementById('<%= txt_couriername.ClientID %>');
                var seal = document.getElementById('<%= txt_seal.ClientID %>');
                var weight = document.getElementById('<%= txt_totalLoadWeight.ClientID %>');
                var hd_IDChk = document.getElementById('<%= hd_IDChk.ClientID %>');
                var bagNo = document.getElementById('<%= txt_bagno.ClientID %>');
                var dd_destination = document.getElementById('<%= dd_destination.ClientID %>');
                var dd_route = document.getElementById('<%= dd_route.ClientID %>');
                var bags = document.getElementById('tbl_detailsBag');

                //                if ((bags.rows.length - 1) == 0) {
                //                    alert("Cannot Save Loading");
                //                    return;
                //                }

                var inputs = mode.getElementsByTagName('input');

                var selectedValue = "";
                for (var i = 0; i < inputs.length; i++) {
                    if (inputs[i].checked) {
                        selectedValue = inputs[i].value;
                        break;
                    }

                }
                var vehicleMode = 0;
                if (rRented.checked) {
                    vehicleMode = 'r';
                }
                else if (rVehicle.checked) {
                    vehicleMode = 'v';
                }
                var jsonObj = { Master: {}, Bags: [], Consignments: [] }
                var routeValue = route.options[route.options.selectedIndex].value.toString();
                var transportTypeValue = transportType.options[transportType.options.selectedIndex].value.toString();
                var destinationValue = destination.options[destination.options.selectedIndex].value.toString()
                var vehicleNoValue = vehicle.options[vehicle.options.selectedIndex].value.toString();
                var vehicleTypeValue = vehicleType.options[vehicleType.options.selectedIndex].value.toString();
                var hd_IDChk = document.getElementById('<%= hd_IDChk.ClientID %>');
                var hd_U_ID = document.getElementById('<%=hd_U_ID.ClientID %>');
                var hd_zoneCode = document.getElementById('<%=hd_zoneCode.ClientID %>');
                var hd_branchCode = document.getElementById('<%=hd_branchCode.ClientID %>');
                var hd_expressCenterCode = document.getElementById('<%=hd_expressCenterCode.ClientID %>');
                var hd_LocationName = document.getElementById('<%=hd_LocationName.ClientID %>');
                var hd_LocationID = document.getElementById('<%=hd_LocationID.ClientID %>');
                var MasterParameters;
                MasterParameters = {
                    Mode: selectedValue,
                    Date: startDate.value,
                    Route: routeValue,
                    TransportType: transportTypeValue,
                    LoadingID: vid.value.toString(),
                    VehicleMode: vehicleMode,
                    Destination: destinationValue,
                    VehicleNo: vehicleNoValue,
                    RegNo: regNo.value.toString(),
                    FlightNo: flight.value.toString(),
                    FlightDeparture: departureDate.value.toString(),
                    VehicleType: vehicleTypeValue,
                    Description: description.value.toString(),
                    CourierName: courierName.value.toString(),
                    SealNo: seal.value.toString(),
                    TotalWeight: weight.value.toString(),
                    lbl_loadingID: hd_loadingID.innerHTML.toString(),
                    hd_IDChk: hd_IDChk.value.toString(),
                    hd_U_ID: hd_U_ID.value.toString(),
                    hd_zoneCode: hd_zoneCode.value.toString(),
                    hd_branchCode: hd_branchCode.value.toString(),
                    hd_expressCenterCode: hd_expressCenterCode.value.toString(),
                    hd_LocationName: hd_LocationName.value.toString(),
                    hd_LocationID: hd_LocationID.value.toString()
                }
                jsonObj.Master = MasterParameters;

                for (var i = 1; i < bags.rows.length; i++) {
                    var bag;
                    var tr = bags.rows[i];
                    var destinationValue = tr.cells[2].childNodes[0].options[tr.cells[2].childNodes[0].options.selectedIndex].value.toString();
                    if (tr.cells[7].childNodes[0].value.toString() != "CN") {
                        bag = {
                            BagNo: tr.cells[1].innerText,
                            Weight: tr.cells[4].childNodes[0].value.toString(),
                            Origin: tr.cells[2].childNodes[0].value.toString(),
                            Destination: tr.cells[3].childNodes[0].value.toString(),
                            SealNo: tr.cells[5].childNodes[0].value.toString(),
                            Remarks: tr.cells[6].childNodes[0].value.toString()
                        }

                        jsonObj.Bags.push(bag);
                    }
                    else {
                        var destinationValue = tr.cells[2].childNodes[0].options[tr.cells[2].childNodes[0].options.selectedIndex].value;
                        consignment = {
                            ConsignmentNumber: tr.cells[1].innerText,
                            ServiceType: 'Overnight',
                            ConsignmentType: 'Normal',
                            Destination: tr.cells[3].childNodes[0].value.toString(),
                            Weight: tr.cells[4].childNodes[0].value.toString(),
                            Pieces: tr.cells[5].childNodes[0].value.toString(),
                            Remarks: tr.cells[6].childNodes[0].value.toString()
                        }

                        jsonObj.Consignments.push(consignment);



                    }
                }



                $.ajax({

                    url: 'MasterAirportLoadingSpeedy_New.aspx/InsertLoading',
                    type: 'POST',
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    data: JSON.stringify(jsonObj),
                    success: function (response) {
                        var RESP_ = response.d;
                        if (RESP_[0] == "1") {

                            curr = "1";
                        }
                        else {
                            curr = "2"
                            alert('Error in Saving: ' + RESP_[1].toString());
                        }
                    },

                    error: function () {
                        alert('Error in Generating AirportArrival');

                    },
                    failure: function () {
                        alert('Failed to Generate AirportArrival');


                    }


                });
                return curr;
            }
        </script>
        <%--Loading ID Text Change--%>
        <script type="text/javascript">
            function VidChange(txt) {

                var route = document.getElementById('<%= dd_route.ClientID %>');
                var startDate = document.getElementById('<%= dd_start_date.ClientID %>');
                var touchPoint = document.getElementById('<%= dd_touchpoint.ClientID %>');
                var vid = document.getElementById('<%= txt_vid.ClientID %>');
                document.getElementById('<%= txt_vid.ClientID %>').disabled = true;
                var transportType = document.getElementById('<%= dd_transporttype.ClientID %>');
                var origin = document.getElementById('<%= dd_orign.ClientID %>');
                var flight = document.getElementById('<%= txt_flight.ClientID %>');
                var departureDate = document.getElementById('<%= dept_date.ClientID %>');
                var rVehicle = document.getElementById('Vehicle');
                var rRented = document.getElementById('Rented');
                var destination = document.getElementById('<%= dd_destination.ClientID %>');
                var vehicle = document.getElementById('<%= dd_vehicle.ClientID %>');
                var regNo = document.getElementById('<%= txt_reg.ClientID %>');
                var vehicleType = document.getElementById('<%= dd_vehicleType.ClientID %>');
                var description = document.getElementById('<%= txt_description.ClientID %>');
                var courierName = document.getElementById('<%= txt_couriername.ClientID %>');
                var seal = document.getElementById('<%= txt_seal.ClientID %>');
                var weight = document.getElementById('<%= txt_totalLoadWeight.ClientID %>');
                var hd_loadingID = document.getElementById('<%= hd_loadingID.ClientID %>');

                var cnNo = document.getElementById('<%= txt_consignmentno.ClientID %>');
                var bagNo = document.getElementById('<%= txt_bagno.ClientID %>');
                var hd_branchCode = document.getElementById('<%= hd_branchCode.ClientID %>');
                var bags = document.getElementById('tbl_detailsBag');

                var laodingID = txt.value;



                $.ajax({
                    url: 'MasterAirportLoadingSpeedy_new.aspx/GetLoadingData',
                    type: 'POST',
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    data: "{'laodingID':'" + laodingID + "','hd_branchCode':'" + hd_branchCode.value + "'}",
                    success: function (result) {
                        var resp = result.d;
                        var Master = resp.Master;
                        if (resp.status == "1") {


                            if (hd_loadingID.innerHTML == '') {
                                hd_loadingID.innerHTML = txt.value;
                                hd_loadingID.value = txt.value;
                            }

                            startDate.value = Master.Date;
                            description.value = Master.Description;
                            courierName.value = Master.CourierName;
                            seal.value = Master.SealNo;
                            weight.value = Master.TotalWeight;

                            for (var i = 0; i < route.options.length; i++) {

                                var currentRoute = route.options[i].value;
                                if (Master.Route == currentRoute) {
                                    route.options.selectedIndex = i;
                                    break;
                                }
                            }

                            for (var i = 0; i < transportType.options.length; i++) {

                                var currenttransportType = transportType.options[i].value;
                                if (Master.TransportType == currenttransportType) {
                                    transportType.options.selectedIndex = i;
                                    break;
                                }
                            }

                            TransportTypeChange(transportType);
                            if (Master.TransportType == "197") {
                                flight.value = Master.FlightNo;
                                //departureDate.value = Master.FlightDeparture;
                            }
                            touchPoint.length = 0;
                            for (var i = 0; i < resp.TouchPoint.length; i++) {
                                var option = document.createElement('option');
                                option.value = resp.TouchPoint[i].Value;
                                option.text = resp.TouchPoint[i].Text;
                                touchPoint.add(option);
                            }

                            destination.length = 0;
                            for (var i = 0; i < resp.Destination.length; i++) {
                                var option = document.createElement('option');
                                option.value = resp.Destination[i].Value;
                                option.text = resp.Destination[i].Text;
                                destination.add(option);
                            }

                            if (Master.VehicleNo == "103") {
                                rRented.checked = true;
                                rVehicle.checked = false;
                                VehicleModeChange(rRented);
                                regNo.value = Master.RegNo;
                            }
                            else {
                                rVehicle.checked = true;
                                rRented.checked = false;
                                VehicleModeChange(rVehicle);
                                regNo.value = "";
                                for (var i = 0; i < vehicle.options.length; i++) {

                                    var currentVehicle = vehicle.options[i].value;
                                    if (Master.VehicleNo == currentVehicle) {
                                        vehicle.options.selectedIndex = i;
                                        break;
                                    }
                                }
                            }

                            for (var i = 0; i < vehicleType.options.length; i++) {
                                var currentVehicleType = vehicleType.options[i].value;
                                if (currentVehicleType == Master.VehicleType) {
                                    vehicleType.options.selectedIndex = i;
                                    break;
                                }
                            }

                            for (var i = 1; i < bags.rows.length;) {
                                bags.deleteRow(1);
                            }
                            for (var i = 0; i < resp.Bags.length; i++) {
                                bagNo.value = resp.Bags[i].BagNo;
                                AddBagEdit(resp.Bags[i], "0");
                            }

                            for (var i = 0; i < resp.Consignments.length; i++) {
                                //  cnNo.value = resp.Consignments[i].ConsignmentNumber;
                                AddConsignmentEdit(resp.Consignments[i], "0");
                            }
                        }
                        else {
                            alert("Loading Not Found. Error: " + resp.reason);
                            document.getElementById('<%= txt_vid.ClientID %>').value = '';

                            hd_loadingID.innerHTML == '';

                        }

                    },
                    error: function () { alert('Loading Not Found'); txt.value = ""; },
                    failure: function () { alert('Loading Not Found'); txt.value = ""; }
                });
            }
        </script>
        <%--Reset All--%>
        <script type="text/javascript">
            function ResetAll() {
                var mode = document.getElementById('<%= rbtn_mode.ClientID %>');
                var route = document.getElementById('<%= dd_route.ClientID %>');
                var startDate = document.getElementById('<%= dd_start_date.ClientID %>');
                var touchPoint = document.getElementById('<%= dd_touchpoint.ClientID %>');
                var vid = document.getElementById('<%= txt_vid.ClientID %>');
                var transportType = document.getElementById('<%= dd_transporttype.ClientID %>');
                var origin = document.getElementById('<%= dd_orign.ClientID %>');
                var flight = document.getElementById('<%= txt_flight.ClientID %>');
                var departureDate = document.getElementById('<%= dept_date.ClientID %>');
                var rVehicle = document.getElementById('Vehicle');
                var rRented = document.getElementById('Rented');
                var destination = document.getElementById('<%= dd_destination.ClientID %>');
                var vehicle = document.getElementById('<%= dd_vehicle.ClientID %>');
                var regNo = document.getElementById('<%= txt_reg.ClientID %>');
                var vehicleType = document.getElementById('<%= dd_vehicleType.ClientID %>');
                var description = document.getElementById('<%= txt_description.ClientID %>');
                var courierName = document.getElementById('<%= txt_couriername.ClientID %>');
                var seal = document.getElementById('<%= txt_seal.ClientID %>');
                var weight = document.getElementById('<%= txt_totalLoadWeight.ClientID %>');

                var cnNo = document.getElementById('<%= txt_consignmentno.ClientID %>');
                var bagNo = document.getElementById('<%= txt_bagno.ClientID %>');

                var bags = document.getElementById('tbl_detailsBag');
                var totalCount = document.getElementById('lbl_count'); //Total Scanned Items:

                totalCount.value = "Total Item Scanned #:";
                var d = new Date();
                startDate.value = "";
                startDate.value = formatDate(d);
                route.options.selectedIndex = 0;

                touchPoint.length = 0;
                vid.value = "";
                transportType.selectedIndex = 0;
                TransportTypeChange(transportType);
                flight.value = "";
                departureDate.value = "";
                rVehicle.checked = true;
                VehicleModeChange(rVehicle);
                destination.length = 0;
                vehicle.options.selectedIndex = 0;
                regNo.value = "";
                vehicleType.options.selectedIndex = 0;
                description.value = "";
                courierName.value = "";
                seal.value = "";
                weight.value = "";

                for (var i = 1; i < bags.rows.length;) {
                    bags.deleteRow(1);
                }


                CalculateItems();
                CalculateTotalWeight();
            }
            function formatDate(date) {
                var d = new Date(date),
                month = '' + (d.getMonth() + 1),
                day = '' + d.getDate(),
                year = d.getFullYear();

                if (month.length < 2) month = '0' + month;
                if (day.length < 2) day = '0' + day;

                return [year, month, day].join('-');
            }
        </script>
        <script type="text/javascript">
            function ajaxcall(url, data, callback) {
                $.ajax({
                    url: url, // server url
                    type: 'POST', //POST or GET 
                    data: data, // data to send in ajax format or querystring format
                    datatype: 'json',
                    beforeSend: function () {
                        alert('sending data');
                        // do some loading options
                    },
                    success: function (data) {
                        callback(data); // return data in callback
                    },

                    complete: function () {
                        alert('ajax call complete');
                        // success alerts
                    },

                    error: function (xhr, status, error) {
                        alert(xhr.responseText); // error occur 
                    }

                });
            }
        </script>
        <%--Posting After Every 5 Minutes to Keep sessions fresh --%>
        <script type="text/javascript">

            var myVar = setInterval(myTimer, 150000);

            function myTimer() {
                PageMethods.RefreshTime("", OnSuccess2);
            }
            function OnSuccess2(response, userContext, methodName) {

            }
            function formatNumber(num) {
                return num.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,")
            }
        </script>
        <%--focus to bag--%>
        <script type="text/javascript">
            function focusToBag() {
                var pan1 = document.getElementById('<%= TabPanel1.ClientID %>');
                var pan2 = document.getElementById('<%= TabPanel2.ClientID %>');
                var cn = document.getElementById('<%= txt_consignmentno.ClientID %>');
                var bag = document.getElementById('<%= txt_bagno.ClientID %>');
                focusWorking(bag);

            }
            function focusAfterSeal(evt) {
                var charCode = (evt.which) ? evt.which : event.keyCode

                var pan1 = document.getElementById('<%= TabPanel1.ClientID %>');
                var pan2 = document.getElementById('<%= TabPanel2.ClientID %>');
                var cn = document.getElementById('<%= txt_consignmentno.ClientID %>');
                var bag = document.getElementById('<%= txt_bagno.ClientID %>');


                // focusWorking(bag);





            }
        </script>
        <asp:HiddenField ID="hd_Branches" runat="server" />
</asp:Content>