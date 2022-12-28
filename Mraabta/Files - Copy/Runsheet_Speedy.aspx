<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="Runsheet_Speedy.aspx.cs" Inherits="MRaabta.Files.Runsheet_Speedy" %>

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
    <script type="text/javascript" src="../Js/jquery-1.7.2.min.js"></script>
    <%--Mode Change--%>
    <script type="text/javascript">
        function ChangeFormMode() {
            var tblDetails = document.getElementById('tblCn');
            for (var i = 1; i < tblDetails.rows.length;) {
                tblDetails.deleteRow(1);
            }
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
            if (selectedValue == "NEW") {
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
    </script>
    <%--Runsheet Number Change--%>
    <script type="text/javascript">
        function RunsheetNumberChange(txt) {
            var tblDetails = document.getElementById('tblCn');
            var runsheetNumber = document.getElementById('<%= txt_runsheetNumber.ClientID %>');
            var date = document.getElementById('<%= txt_date.ClientID %>');
            var type = document.getElementById('<%= dd_runsheetType.ClientID %>');
            var txtRoute = document.getElementById('<%= txt_routeCode.ClientID %>');
            var ddRoute = document.getElementById('<%= dd_route.ClientID %>');
            var txtRider = document.getElementById('<%= txt_riderno.ClientID %>');
            var ddRider = document.getElementById('<%= dd_riders.ClientID %>');
            var txtVehicle = document.getElementById('<%= txt_vehicleNumber.ClientID %>');
            var ddVehicle = document.getElementById('<%= dd_vehicle.ClientID %>');
            var ddVehicleType = document.getElementById('<%= dd_vehicleType.ClientID %>');
            var txtMeterStart = document.getElementById('<%= txt_meterStart.ClientID %>');
            var txtMeterEnd = document.getElementById('<%= txt_meterEnd.ClientID %>');



            if (txt.value.trim() == "") {
                alert('Enter Runsheet Number');
                return;
            }
            else if (txt.value.length < 12) {
                alert('Invalid Runsheet Number');
                return;
            }

            $.ajax({
                url: 'Runsheet_Speedy.aspx/GetRunsheetByRunsheetNumber',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                datatype: 'json',
                data: "{'runsheetNumber':'" + txt.value.trim() + "'}",
                success: function (result) {
                    var resp = result.d;
                    var header = resp.Header;
                    var details = resp.Details;
                    if (header.status == "0") {
                        alert(header.cause);
                        txt.value = "";
                        focusWorking(txt);
                        return;
                    }

                    date.value = header.RunsheetDate;
                    for (var i = 0; i < type.options.length; i++) {
                        var currentType = type.options[i].value.toUpperCase();
                        if (currentType == header.RunsheetType) {
                            type.options[i].selected = true;
                            break;
                        }
                    }

                    txtRoute.value = header.RouteCode;
                    for (var i = 0; i < ddRoute.options.length; i++) {
                        var currentRoute = ddRoute.options[i].value;
                        if (currentRoute == header.RouteCode) {
                            ddRoute.options[i].selected = true;
                            break;
                        }
                    }
                    var vehicleFound = false;
                    for (var i = 0; i < ddVehicle.options.length; i++) {
                        var currentVehicle = ddVehicle.options[i].value;
                        if (currentVehicle.trim().toUpperCase() == header.VehicleNumber.trim().toUpperCase() || currentVehicle.trim().replace('-', '').toUpperCase() == header.VehicleNumber.trim().replace('-', '').toUpperCase()) {
                            ddVehicle.options[i].selected = true;
                            vehicleFound = true;
                            break;
                        }
                    }

                    if (vehicleFound) {
                        txtVehicle.value = ddVehicle.options[ddVehicle.options.selectedIndex].text;
                    }

                    for (var i = 0; i < ddVehicleType.options.length; i++) {
                        var currentVType = ddVehicleType.options[i].value;
                        if (currentVType == header.VehicleType) {
                            ddVehicleType.options[i].selected = true;
                            break;
                        }
                    }

                    txtMeterStart.value = header.MeterStart;
                    txtMeterEnd.value = header.MeterEnd;

                    ddRider.length = 0;
                    var RiderOption = document.createElement('option');
                    RiderOption.value = header.RiderCode;
                    RiderOption.text = header.RiderName;
                    ddRider.appendChild(RiderOption);
                    txtRider.value = header.RiderCode;

                    for (var i = 1; i < tblDetails.rows.length;) {
                        tblDetails.deleteRow(1);
                    }

                    for (var i = 0; i < details.length; i++) {
                        AddConsignment(details[i], "0");
                    }


                },
                error: function () {
                    txt.value = "";
                    alert("Oops Something went wrong. Please contact I.T. Support");
                    focusWorking(txt);
                    return;
                },
                failure: function () {
                    txt.value = "";
                    alert("Oops Something went wrong. Please contact I.T. Support");
                    focusWorking(txt);
                    return;
                }

            })

        }
    </script>
    <%--Miscellenious functions--%>
    <script type="text/javascript">

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



        function FindVehicle(txt) {

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
    <%--Route Change--%>
    <script type="text/javascript">
        function RouteChange(cnt) {
            var txtRoute = document.getElementById('<%= txt_routeCode.ClientID %>');
            var ddRoute = document.getElementById('<%= dd_route.ClientID %>');
            var txtRider = document.getElementById('<%= txt_riderno.ClientID %>');
            var ddRider = document.getElementById('<%= dd_riders.ClientID %>');

            var routeFound = false;
            var riderFound = false;

            if (cnt.tagName.toString().toUpperCase() == "INPUT") {
                for (var i = 0; i < ddRoute.options.length; i++) {
                    var currentRoute = ddRoute.options[i].value;

                    if (currentRoute == cnt.value) {
                        ddRoute.options[i].selected = true;
                        routeFound = true;
                        break;
                    }
                }
            }
            else if (cnt.tagName.toString().toUpperCase() == "SELECT") {
                txtRoute.value = cnt.options[cnt.options.selectedIndex].value;
                routeFound = true;
            }



            if (routeFound) {
                $.ajax({
                    url: 'Runsheet_Speedy.aspx/GetRider',
                    type: 'POST',
                    contentType: 'application/json; charset=utf-8',
                    datatype: 'json',
                    data: "{'routeCode':'" + txtRoute.value + "'}",
                    success: function (result) {
                        var resp = result.d;
                        if (resp[0].toString() == "0") {
                            alert(resp[2].toString());
                            ddRider.length = 0;
                            txtRider.value = "";
                            txtRoute.value = "";
                            ddRoute.options.selectedIndex = 0;
                            focusWorking(cnt);
                            return;
                        }
                        else if (resp[0].toString() == "1") {
                            ddRider.length = 0;
                            var option = document.createElement('option');

                            option.value = resp[1].toString();
                            option.text = resp[2].toString();
                            ddRider.appendChild(option);

                            txtRider.value = resp[1].toString();
                        }
                        else {

                            ddRider.length = 0;
                            txtRider.value = "";
                            txtRoute.value = "";
                            ddRoute.options.selectedIndex = 0;
                            alert("Oops Something went wrong. Please contact I.T. Support");
                            focusWorking(cnt);
                            return;
                        }
                    },
                    error: function () {
                        ddRider.length = 0;
                        txtRider.value = "";
                        txtRoute.value = "";
                        ddRoute.options.selectedIndex = 0;
                        alert("Oops Something went wrong. Please contact I.T. Support");
                        focusWorking(cnt);
                        return;
                    },
                    failure: function () {
                        ddRider.length = 0;
                        txtRider.value = "";
                        txtRoute.value = "";
                        ddRoute.options.selectedIndex = 0;
                        alert("Oops Something went wrong. Please contact I.T. Support");
                        focusWorking(cnt);
                        return;
                    }

                })
            }
            else {
                alert('Invalid Route Code');
                ddRider.length = 0;
                txtRider.value = "";
                txtRoute.value = "";
                ddRoute.options.selectedIndex = 0;
                focusWorking(cnt);
            }



        }
    </script>
    <%--Consignment Change--%>
    <script type="text/javascript">
        function ConsignmentChange(txt) {

            var tblDetails = document.getElementById('tblCn');
            var cn = txt.value.trim();
            if (cn.trim() == "") {
                alert('Enter Consignment Number');
                focusWorking(txt);
                return;
            }
            for (var i = 1; i < tblDetails.rows.length; i++) {
                if (tblDetails.rows[i].cells[1].innerText == cn) {
                    alert('Consignment Already Scanned');
                    txt.value = "";
                    focusWorking(txt);
                    return;
                }
            }


            $.ajax({
                url: 'Runsheet_Speedy.aspx/GetConsignmentDetail',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                datatype: 'json',
                data: "{'ConsignmentNumber':'" + txt.value.trim() + "'}",
                success: function (result) {
                    var resp = result.d;

                    if (resp.status == "0") {
                        alert(resp.cause);
                        focusWorking(txt);
                        txt.value = "";
                        return;
                    }
                    else if (resp.status == "1") {
                        AddConsignment(resp, "1");
                    }
                    else {
                        alert(resp.cause);
                        focusWorking(txt);
                        txt.value = "";
                        return;
                    }


                },
                error: function () {

                    alert("Oops Something went wrong. Please contact I.T. Support");
                    focusWorking(txt);
                    return;
                },
                failure: function () {

                    alert("Oops Something went wrong. Please contact I.T. Support");
                    focusWorking(txt);
                    return;
                }

            })


        }
    </script>
    <%--Adding Consignment in Table--%>
    <script type="text/javascript">
        function AddConsignment(cn, AddMode) {
            var mode = document.getElementById('<%= rbtn_formMode.ClientID %>');
            var inputs = mode.getElementsByTagName('input');
            var branches = document.getElementById('branches');
            var selectedValue = "";
            for (var i = 0; i < inputs.length; i++) {
                if (inputs[i].checked) {
                    selectedValue = inputs[i].value;
                    break;
                }

            }

            var consignments = document.getElementById('tblCn');


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

            var hid = document.createElement('input');
            hid.type = 'hidden';
            hid.id = 'hid';

            var hid2 = document.createElement('input');
            hid2.type = 'hidden';
            hid2.id = 'hid';


            var dd_origin = GetBranchDropDown();
            dd_origin.style.width = '90%'
            dd_origin.className = 'dropdown';




            var DestinationName = "";
            for (var i = 0; i < branches.options.length; i++) {
                var branchValue = branches.options[i].value;
                if (branchValue == cn.Destination) {
                    DestinationName = branches.options[i].text;
                    break;
                }
            }

            var dd_destination = document.createElement('select');
            dd_destination.style.width = '90%'
            dd_destination.className = 'dropdown';
            dd_destination.disabled = true;

            var option = document.createElement('option');
            option.value = cn.Destination;
            option.text = DestinationName;
            dd_destination.appendChild(option);



            col = newTr.insertCell(0);
            newTr.cells[0].style.textAlign = 'center';
            if (selectedValue.toUpperCase() == "NEW" || AddMode == "1") {
                newTr.cells[0].appendChild(btn_remove);
                newTr.cells[0].childNodes[0].onclick = RemoveRow.bind(newTr.cells[0].childNodes[0], newTr.cells[0].childNodes[0]);
                hid2.value = "1";
            }

            col = newTr.insertCell(1);
            newTr.cells[1].innerText = cn.ConsignmentNumber;
            col = newTr.insertCell(2);
            newTr.cells[2].appendChild(dd_origin);
            newTr.cells[2].style.textAlign = 'left';
            //newTr.cells[2].childNodes[0].onchange = CalculateTotalWeight.bind(newTr.cells[2].childNodes[0]);
            col = newTr.insertCell(3);
            newTr.cells[3].appendChild(dd_destination);
            newTr.cells[3].appendChild(hid);
            newTr.cells[3].appendChild(hid2);
            newTr.cells[3].childNodes[1].value = cn.isNew;

            col = newTr.insertCell(4);
            newTr.cells[4].style.textAlign = 'left';
            newTr.cells[4].innerText = cn.ConsignmentTypeName;


            for (var i = 0; i < newTr.cells[2].childNodes[0].options.length; i++) {
                var currentOrigin = cn.Origin;
                var originValue = newTr.cells[2].childNodes[0].options[i].value;
                if (originValue == currentOrigin) {
                    newTr.cells[2].childNodes[0].options[i].selected = true;
                    break;
                }
            }

            var cnnumber = document.getElementById('<%= txt_cnNumber.ClientID %>');
            focusWorking(cnnumber);
            cnnumber.value = "";
            CalculateRows();
        }

    </script>
    <%--Removing Item from Table--%>
    <script type="text/javascript">
        function RemoveRow(btn) {
            var tr = btn.parentElement.parentElement;
            var table = tr.parentElement;

            table.deleteRow(tr.rowIndex);

            CalculateRows();


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
            currdd.disabled = true;
            return currdd;
        }


    </script>
    <%--Populating Branches--%>
    <script type="text/javascript">
        window.onload = OnWindowLoad;
        function OnWindowLoad() {
            var dropdown = document.getElementById('branches');
            var route = document.getElementById('<%= dd_route.ClientID %>');
            PopulateBranches();

        }
        function PopulateBranches() {
            var dropdown = document.getElementById('branches');
            $.ajax({
                url: 'Runsheet_Speedy.aspx/GetBranchesForDropDown',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                datatype: 'json',
                data: '',
                success: function (result) {
                    var a = '';
                    var resp = result.d;
                    var branches = resp.Branches;

                    var BranchCode = resp.BranchCode;
                    var ZoneCode = resp.ZoneCode;
                    var ExpressCenter = resp.ExpressCenter;
                    var CreatedBy = resp.CreatedBy;

                    var hd_BranchCode = document.getElementById('<%= hd_BranchCode.ClientID %>');
                    var hd_ZoneCode = document.getElementById('<%= hd_ZoneCode.ClientID %>');
                    var hd_ExpressCenter = document.getElementById('<%= hd_ExpressCenter.ClientID %>');
                    var hd_CreatedBy = document.getElementById('<%= hd_CreatedBy.ClientID %>');

                    hd_BranchCode.value = BranchCode;
                    hd_ZoneCode.value = ZoneCode;
                    hd_ExpressCenter.value = ExpressCenter;
                    hd_CreatedBy.value = CreatedBy;

                    for (var i = 0; i < branches.length; i++) {
                        var option = document.createElement('option');
                        option.text = branches[i].BranchName;
                        option.value = branches[i].BranchCode;

                        dropdown.add(option);
                    }


                },
                error: function () {

                },
                failure: function () {

                }

            })
        }
    </script>
    <%--Saving Runsheet--%>
    <script type="text/javascript">
        function SaveRunsheet() {
            try {


                loader.style.display = 'block';
                var tblDetails = document.getElementById('tblCn');
                var mode = document.getElementById('<%= rbtn_formMode.ClientID %>');
                var txt_runsheet = document.getElementById('<%= txt_runsheetNumber.ClientID %>');
                var rDate = document.getElementById('<%= div_date.ClientID %>');
                var dDate = document.getElementById('<%= div_dateDisplay.ClientID %>');
                var newDate = document.getElementById('<%= picker1.ClientID %>');
                var updateDate = document.getElementById('<%= txt_date.ClientID %>');
                var rType = document.getElementById('<%= dd_runsheetType.ClientID %>');
                var txt_routeCode = document.getElementById('<%= txt_routeCode.ClientID %>');
                var dd_route = document.getElementById('<%= dd_route.ClientID %>');
                var dd_docType = document.getElementById('<%= dd_docType.ClientID %>');
                var txt_count = document.getElementById('<%= lbl_count.ClientID %>');
                var dd_rider = document.getElementById('<%= dd_riders.ClientID %>');


                var meterEnd = document.getElementById('<%= txt_meterEnd.ClientID %>');
                var meterStart = document.getElementById('<%= txt_meterStart.ClientID %>');
                var ddvehicleType = document.getElementById('<%= dd_vehicleType.ClientID %>');
                var ddVehicle = document.getElementById('<%= dd_vehicle.ClientID %>');
                var txtVehicle = document.getElementById('<%= txt_vehicleNumber.ClientID %>');

                var hd_BranchCode = document.getElementById('<%= hd_BranchCode.ClientID %>');
                var hd_ZoneCode = document.getElementById('<%= hd_ZoneCode.ClientID %>');
                var hd_ExpressCenter = document.getElementById('<%= hd_ExpressCenter.ClientID %>');
                var hd_CreatedBy = document.getElementById('<%= hd_CreatedBy.ClientID %>');


                var inputs = mode.getElementsByTagName('input');
                var selectedValue = "";
                var txt_rider = document.getElementById('<%= txt_riderno.ClientID %>');
                for (var i = 0; i < inputs.length; i++) {
                    if (inputs[i].checked) {
                        selectedValue = inputs[i].value;
                    }
                }

                if (selectedValue.toUpperCase() == "EDIT") {
                    if (txt_runsheet.value.trim() == "") {
                        alert('Enter Runsheet Number');
                        return;
                    }
                }
                //            if (runsheetType.options[runsheetType.options.selectedIndex].value == "0") {
                //                alert('Select Runsheet Type');
                //                return;
                //            }
                if (txt_routeCode.value.trim() == "") {
                    alert('Enter Rider Code or Select Rider');
                    return;
                }
                if (txt_rider.value.trim() == "") {
                    alert('Rider Not Found');
                    return;
                }
                var date = "";
                if (selectedValue.toUpperCase() == "NEW") {
                    date = newDate.value;
                }
                else {
                    date = updateDate.value;
                }
                var runsheetType = rType.options[rType.options.selectedIndex].value;
                var routeCode = dd_route.options[dd_route.options.selectedIndex].value;
                var riderCode = dd_rider.options[dd_rider.options.selectedIndex].value;
                var meterStartValue = meterStart.value;
                var meterEndValue = meterEnd.value;
                var vehicle = txtVehicle.value;
                var vehicleTypeValue = ddvehicleType.options[ddvehicleType.options.selectedIndex].value;
                var jsonObject = { Header: {}, Details: [], Mode: selectedValue }
                var Head = {
                    status: "",
                    cause: "",
                    RunsheetNumber: txt_runsheet.value,
                    BranchCode: hd_BranchCode.value,
                    ZoneCode: hd_ZoneCode.value,
                    ExpressCenterCode: hd_ExpressCenter.value,
                    CreatedBy: hd_CreatedBy.value,
                    RunsheetDate: date,
                    RunsheetType: runsheetType,
                    RouteCode: routeCode,
                    RouteName: "",
                    RiderCode: riderCode,
                    RiderName: "",
                    VehicleNumber: vehicle,
                    VehicleType: vehicleTypeValue,
                    MeterStart: meterStartValue,
                    MeterEnd: meterEndValue
                }

                jsonObject.Header = Head;
                var save = false;
                for (var i = 1; i < tblDetails.rows.length; i++) {
                    var tr = tblDetails.rows[i];
                    var destination = tr.cells[3].childNodes[0];
                    var destinationValue = destination.options[destination.options.selectedIndex].value;
                    var destinationName = destination.options[destination.options.selectedIndex].text;

                    var origin = tr.cells[2].childNodes[0];
                    var originValue = origin.options[origin.options.selectedIndex].value;
                    var originName = origin.options[origin.options.selectedIndex].text;

                    var contype = tr.cells[4].innerText;
                    var isNew = tr.cells[3].childNodes[1].value;
                    var removeable = tr.cells[3].childNodes[2].value;

                    var consignment = {
                        status: "",
                        cause: "",
                        ConsignmentNumber: tr.cells[1].innerText,
                        Origin: originValue,
                        Destination: destinationValue,
                        OriginName: originName,
                        DestinationName: destinationName,
                        ConsignmentType: contype,
                        ConsignmentTypeName: "",
                        SortOrder: "",
                        isNew: isNew,
                        removeable: removeable
                    }

                    jsonObject.Details.push(consignment);
                    save = true;
                    document.getElementById("save_r").disabled = true;

                }

                if (save) {
                    $.ajax({

                        url: 'Runsheet_Speedy.aspx/SaveRunsheet',
                        type: 'POST',
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        data: JSON.stringify(jsonObject),
                        success: function (result) {
                            var resp = result.d;
                            if (resp[0] == "0") {
                                alert(resp[1].toString());
                                loader.style.display = 'none';
                                return;
                            }
                            else if (resp[0] == "1") {
                                loader.style.display = 'none';
                                alert('Runsheet Saved. Runsheet Number: ' + resp[1].toString());
                                window.open('RunsheetInvoice_Plain.aspx?Xcode=' + resp[1] + '&RCode=' + routeCode, '_blank');
                                window.open('Runsheet_Speedy.aspx', '_parent');
                            }
                        },

                        error: function () {
                            alert('Error in Generating Runsheet');
                            loader.style.display = 'none';

                        },
                        failure: function () {
                            alert('Failed to Generate Runsheet');
                            loader.style.display = 'none';


                        }


                    });
                }


            } catch (err) {
                loader.style.display = 'none';
                alert(err.Message);
            }

        }

    </script>
    <script type="text/javascript">
        function CalculateRows() {
            var tbl = document.getElementById('tblCn');
            var cnt = document.getElementById('lbl_cnt');

            cnt.textContent = 'Total Consignments: ' + (tbl.rows.length - 1).toString();
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
        function ResetAll() { }
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
    <style type="text/css">
        .lds-ellipsis {
            display: inline-block;
            position: relative;
            width: 64px;
            height: 64px;
        }

            .lds-ellipsis div {
                position: absolute;
                top: 27px;
                width: 11px;
                height: 11px;
                border-radius: 50%;
                background: #fff;
                animation-timing-function: cubic-bezier(0, 1, 1, 0);
            }

                .lds-ellipsis div:nth-child(1) {
                    left: 6px;
                    animation: lds-ellipsis1 0.6s infinite;
                }

                .lds-ellipsis div:nth-child(2) {
                    left: 6px;
                    animation: lds-ellipsis2 0.6s infinite;
                }

                .lds-ellipsis div:nth-child(3) {
                    left: 26px;
                    animation: lds-ellipsis2 0.6s infinite;
                }

                .lds-ellipsis div:nth-child(4) {
                    left: 45px;
                    animation: lds-ellipsis3 0.6s infinite;
                }

        @keyframes lds-ellipsis1 {
            0% {
                transform: scale(0);
            }

            100% {
                transform: scale(1);
            }
        }

        @keyframes lds-ellipsis3 {
            0% {
                transform: scale(1);
            }

            100% {
                transform: scale(0);
            }
        }

        @keyframes lds-ellipsis2 {
            0% {
                transform: translate(0, 0);
            }

            100% {
                transform: translate(19px, 0);
            }
        }
    </style>
    <div id="loader" style="width: 86%; height: 150%; background-color: #000000; position: absolute; display: none; opacity: 0.5">
        <div style="margin-left: 0%; margin-top: 20%; text-align: center; font-weight: bold; font-size: x-large; color: White;">
            <div class="lds-ellipsis">
                <div>
                </div>
                <div>
                </div>
                <div>
                </div>
                <div>
                </div>
            </div>
        </div>
        <div style="margin-left: 0; text-align: center; font-weight: bold; font-size: x-large; color: White;">
            Please Wait...
        </div>
    </div>
    <div class="search">
        <a href="SearchRunSheet.aspx">Search RunSheet</a>
    </div>
    <asp:Label ID="Errorid" runat="server" Font-Bold="true"></asp:Label>
    <asp:HiddenField ID="hd_BranchCode" runat="server" />
    <asp:HiddenField ID="hd_ZoneCode" runat="server" />
    <asp:HiddenField ID="hd_ExpressCenter" runat="server" />
    <asp:HiddenField ID="hd_CreatedBy" runat="server" />
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
                    onchange="ChangeFormMode()" OnSelectedIndexChanged="rbtn_formMode_SelectedIndexChanged"
                    AutoPostBack="false">
                    <asp:ListItem Value="NEW" Selected="True">NEW</asp:ListItem>
                    <asp:ListItem Value="EDIT">EDIT</asp:ListItem>
                </asp:RadioButtonList>
                <asp:HiddenField ID="hd_modeChanged" runat="server" Value="0" />
                <asp:HiddenField ID="hd_currentMode" runat="server" Value="0" />
            </td>
        </tr>
        <tr>
            <td class="field" style="width: 10% !important;">Runsheet#.
            </td>
            <td class="input-field" style="width: 15% !important;">
                <asp:TextBox ID="txt_runsheetNumber" runat="server" AutoPostBack="false" Enabled="false"
                    OnTextChanged="txt_runsheetNumber_TextChanged" onchange="RunsheetNumberChange(this);"></asp:TextBox>
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
                <asp:TextBox ID="txt_routeCode" runat="server" AutoPostBack="false" onchange="RouteChange(this);"
                    OnTextChanged="txt_routeCode_TextChanged"></asp:TextBox>
            </td>
            <td class="space" style="margin: 0px 0px 0px 0px !important; width: 5% !important;"></td>
            <td class="field" style="width: 10% !important;">Route
            </td>
            <td class="input-field" style="width: 15% !important;">
                <asp:DropDownList ID="dd_route" runat="server" AppendDataBoundItems="true" Width="100%"
                    onchange="RouteChange(this);" OnSelectedIndexChanged="dd_route_SelectedIndexChanged"
                    AutoPostBack="false">
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
                <td style="width: 20%; padding-right: 60px;">
                    <asp:TextBox ID="txt_cnNumber" runat="server" AutoPostBack="false" onkeypress="return isNumberKey(event);"
                        OnTextChanged="txt_cnNumber_TextChanged" MaxLength="20" onchange="ConsignmentChange(this);"></asp:TextBox>
                    <%--Onchange="return validate();"--%>
                </td>
                <td style="text-align: right;">
                    <label id='lbl_cnt'>
                    </label>
                </td>
            </tr>
            <tr>
                <td colspan="3">&nbsp;
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ID="up1" runat="server" Visible="false">
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
        <table id="tblCn" class="tblDetails">
            <tr class="headerRow">
                <td style="width: 7%; text-align: center;"></td>
                <td style="width: 13%;">Consignment Number
                </td>
                <td style="width: 15%;">Origin
                </td>
                <td style="width: 17%;">Destination
                </td>
                <td style="width: 15%;">Consignment Type
                </td>
            </tr>
        </table>
    </fieldset>
    <div style="width: 100%; text-align: center">
        <asp:Button ID="btn_reset" runat="server" Text="Reset" CssClass="button" OnClick="btn_reset_Click"
            UseSubmitBehavior="false" Visible="false" />
        &nbsp;
        <asp:Button ID="btn_save" runat="server" Text="Save" CssClass="button" OnClick="btn_save_Click"
            OnClientClick="loader()" UseSubmitBehavior="false" Visible="false" />
        &nbsp;
        <button type="button" id="save_r" value="Save Runsheet" onclick="SaveRunsheet();"
            class="button">
            SAVE
        </button>
        <button type="button" value="Reset" onclick="ResetAll();" class="button ">
            RESET
        </button>
    </div>
    <div style="display: none">
        <asp:GridView ID="vehicleTypes" runat="server" AutoGenerateColumns="false">
            <Columns>
                <asp:BoundField HeaderText="VehicleCode" DataField="VehicleCode" />
                <asp:BoundField HeaderText="TypeID" DataField="VehicleType_" />
            </Columns>
        </asp:GridView>
    </div>
    <select id="branches" style="display: none;">
        <option value="0">.:Select Branch:.</option>
    </select>
</asp:Content>