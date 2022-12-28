<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.master" CodeBehind="Runsheet_Mega.aspx.cs" Inherits="MRaabta.Files.Runsheet_Mega" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="../Scripts/jquery-3.5.1.min.js"></script>
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

    <%--To Focus on any Control--%>
    <script type="text/javascript">
        function focusWorking(cnt) {
            var id = '#' + cnt.id.toString();
            $(document).ready(function () {
                setTimeout(function () { $(id).focus(); }, 1);
            });
        }
        function ResetAll() {
            window.location.href = "Runsheet_Mega.aspx";
        }
    </script>

    <%--////////////////////////////RIDER Change///////////////////////////////////////////////////////////////Shaheer/////////////////////--%>
    <script type="text/javascript">
        function RiderChange(cnt) {
            var txt_riderno = document.getElementById('<%= txt_riderno.ClientID %>');
            var ddl_riders = document.getElementById('<%= ddl_riders.ClientID %>');
            var routeFound = false;
            var riderFound = false;

            if (cnt.tagName.toString().toUpperCase() == "INPUT") {
                if (cnt.value != "") {
                    count = 0;
                    for (var i = 0; i < ddl_riders.options.length; i++) {
                        var currentRider = ddl_riders.options[i].value;

                        if (currentRider == cnt.value) {
                            ddl_riders.options[i].selected = true;
                            routeFound = true;
                            break;
                        }
                        count++;
                    }
                    if (count == ddl_riders.options.length) {
                        if (routeFound == false) {
                            alert("Kindly Select Rider");
                            focusWorking(cnt);
                            cnt.value = '';
                            ddl_riders.options.selectedIndex = 0;
                        }
                    }
                }
                else {
                    alert("Kindly Select Rider");
                    focusWorking(cnt);
                    cnt.value = '';
                    ddl_riders.options.selectedIndex = 0;
                }
            }
        }


    <%--////////////////////////////ROUTE Change///////////////////////////////////////////////////////////////Shaheer/////////////////////--%>
        function RouteChange(cnt) {
            var txt_routeCode = document.getElementById('<%= txt_routeCode.ClientID %>');
            var ddl_route = document.getElementById('<%= ddl_route.ClientID %>');
            var txt_laskMark = document.getElementById('<%= txt_laskMark.ClientID %>');
            var txt_branchCode = document.getElementById('<%= txt_branchCode.ClientID %>');
            var txt_riderno = document.getElementById('<%= txt_riderno.ClientID %>');
            var ddl_riders = document.getElementById('<%= ddl_riders.ClientID %>');

            var routeFound = false;
            var riderFound = false;
            txt_riderno.value = "";
            ddl_riders.options.selectedIndex = 0;

            if (cnt.tagName.toString().toUpperCase() == "INPUT") {
                if (cnt.value != "") {
                    count = 0;
                    for (var i = 0; i < ddl_route.options.length; i++) {
                        var currentRoute = ddl_route.options[i].value;

                        if (currentRoute.toString().toUpperCase() == cnt.value.toString().toUpperCase()) {
                            ddl_route.options[i].selected = true;
                            routeFound = true;
                            break;
                        }
                        count++;
                    }
                    if (count == ddl_route.options.length) {
                        if (routeFound == false) {
                            // alert("Kindly Select route");
                            focusWorking(cnt);
                        }
                    }
                }
                else {
                    alert("Kindly Select route");

                    focusWorking(cnt);
                }
            }
            else if (cnt.tagName.toString().toUpperCase() == "SELECT") {
                txt_routeCode.value = cnt.options[cnt.options.selectedIndex].value;
                routeFound = true;

            }

            else {
                alert("Kindly Select route");
                focusWorking(cnt);
                routeFound = false;
            }
            if (routeFound) {
                $.ajax({
                    url: 'Runsheet_Mega.aspx/GetRider',
                    type: 'POST',
                    contentType: 'application/json; charset=utf-8',
                    datatype: 'json',
                    data: "{'routeCode':'" + ddl_route.options[ddl_route.options.selectedIndex].value + "','branchCode':'" + txt_branchCode.textContent + "'}",
                    success: function (result) {
                        var resp = result.d;
                        if (resp[0].toString() == "1") {
                            txt_riderno.value = resp[3].toString();
                            txt_laskMark.value = resp[2].toString();
                            RiderChange(txt_riderno);
                            return;
                        }
                        else {
                            focusWorking(txt_routeCode);
                            alert('Rider Data is not valid, Contact IT!');
                        }
                    },
                    error: function () {

                        alert("Oops Something went wrong. Please contact I.T. Support");
                        focusWorking(txt_routeCode);
                        return;
                    },
                    failure: function () {

                        alert("Oops Something went wrong. Please contact I.T. Support");
                        focusWorking(txt_routeCode);
                        return;
                    }
                })
            }
            else {
                alert('Invalid Route Code');
                txt_routeCode.value = "";
                ddl_route.options.selectedIndex = 0;
                txt_riderno.value = "";
                ddl_riders.options.selectedIndex = 0;
                focusWorking(cnt);
            }
        }
        ///////////////////////////////////////////////VEHICLE CHANGE MOOD////////////////////////////////////////////////////////////////////Shaheer/////////////////////
        function SetVehicle(dd) {
            var txt_vehicleNumber = document.getElementById('<%= txt_vehicleNumber.ClientID %>');
            if (dd.options[dd.options.selectedIndex].value == "0") {
                alert('Select Vehicle');
                txt_vehicleNumber.value = "";
                var ddl_vehicle = document.getElementById('<%= ddl_vehicle.ClientID %>');
                ddl_vehicle.options[0].selected = true;

            }
            else {
                txt.value = dd.options[dd.options.selectedIndex].text;
                CheckVehicleType(dd);
            }

        }

        function CheckVehicleType(dd) {

            var vehicleGrid = document.getElementById('<%= vehicleTypes.ClientID %>');
            var ddl_vehicleType = document.getElementById('<%= ddl_vehicleType.ClientID %>');
            var selectedVehicle = dd.options[dd.options.selectedIndex].value;

            for (var i = 0; i < vehicleGrid.rows.length; i++) {
                if (selectedVehicle == vehicleGrid.rows[i].cells[0].innerText) {
                    var selectedVehicleType = vehicleGrid.rows[i].cells[1].innerText;
                    for (var j = 0; j < vehicleType.options.length; j++) {
                        if (ddl_vehicleType.options[j].value == selectedVehicleType) {
                            ddl_vehicleType.options[j].selected = true;
                            break;
                        }
                    }
                }
            }

        }
    </script>
    <script type="text/javascript">
        function SaveMasterRunsheet() {
            try {

                var txt_runsheetNumber = document.getElementById('<%= txt_runsheetNumber.ClientID %>');
                var newDate = document.getElementById('<%= picker1.ClientID %>');


                var txt_routeCode = document.getElementById('<%= txt_routeCode.ClientID %>');
                var ddl_route = document.getElementById('<%= ddl_route.ClientID %>');
                var txt_count = document.getElementById('<%= lbl_count.ClientID %>');
                var ddl_riders = document.getElementById('<%= ddl_riders.ClientID %>');
                var txt_riderno = document.getElementById('<%= txt_riderno.ClientID %>');
                var ddl_vehicleType = document.getElementById('<%= ddl_vehicleType.ClientID %>');
                var ddl_vehicle = document.getElementById('<%= ddl_vehicle.ClientID %>');
                var txt_vehicleNumber = document.getElementById('<%= txt_vehicleNumber.ClientID %>');

                if (txt_routeCode.value.trim() == "") {
                    ddl_route.options[ddl_route.options.selectedIndex].value = 0;
                    ddl_riders.options[ddl_riders.options.selectedIndex].value = 0;
                    alert('Enter Route Code or Select Route');
                    return false;
                }
                if (ddl_route.options[ddl_route.options.selectedIndex].value == 0) {
                    txt_routeCode.value = "";
                    txt_riderno.value = "";
                    ddl_riders.options[ddl_riders.options.selectedIndex].value = 0;
                    alert('Enter Route Code or Select Route');
                    return false;
                }
                if (txt_riderno.value.trim() == "") {
                    ddl_riders.options[ddl_riders.options.selectedIndex].value = 0;
                    alert('Rider Not Found');
                    return false;
                }
                if (ddl_riders.options[ddl_riders.options.selectedIndex].value == 0) {
                    txt_riderno.value = "";
                    alert('Rider Not Found');
                    return false;
                }

                if (ddl_vehicle.options[ddl_vehicle.options.selectedIndex].value != 0) {
                    if (txt_vehicleNumber.value == '') {
                        alert('Kindly Insert Vehicle in Textbox');
                        return false;
                    }
                    if (ddl_vehicleType.options[ddl_vehicleType.options.selectedIndex].value == 0) {
                        alert('Kindly Select Vehicle Type');
                        return false;
                    }
                }

                if (txt_vehicleNumber.value != '') {
                    if (ddl_vehicle.options[ddl_vehicle.options.selectedIndex].value == 0) {
                        alert('Kindly Select Vehicle Number Drop Down');
                        return false;
                    }
                    if (ddl_vehicleType.options[ddl_vehicleType.options.selectedIndex].value == 0) {
                        alert('Kindly Select Vehicle Type');
                        return false;
                    }
                }
                return true;
            }
            catch (err) {
                loader.style.display = 'none';
                alert(err.Message);
                return false;
            }

        }

    </script>

    <%--Consignment Change--%>
    <script type="text/javascript">
        function ConsignmentChange(txt_consignment) {
            var txt_runsheetNumber = document.getElementById('<%= txt_runsheetNumber.ClientID %>');
            var txt_routeCode = document.getElementById('<%= txt_routeCode.ClientID %>');
            var txt_riderno = document.getElementById('<%= txt_riderno.ClientID %>');
            var txt_branchCode = document.getElementById('<%= txt_branchCode.ClientID %>');
            var txt_user = document.getElementById('<%= txt_user.ClientID %>');
            var txt_expressCenter = document.getElementById('<%= txt_expressCenter.ClientID %>');
            var tblDetails = document.getElementById('tblDetails');
            var controlGrid = document.getElementById('<%= cnControls.ClientID %>');
            txt_consignment.disabled = true;

            var prefixNotFound = false;
            var message = '';
            for (var i = 1; i < controlGrid.rows.length; i++) {
                var row = controlGrid.rows[i];
                var prefix = row.cells[0].innerText;
                var length_ = parseInt(row.cells[1].innerText);
                if (prefix == "52190") {
                    var a = 0;
                }
                if (txt_consignment.value.substring(0, prefix.length) == prefix) {
                    if (txt_consignment.value.length != length_) {
                        message = "Invalid Length of Mega CN";
                        prefixNotFound = true;
                    }
                    else {

                        prefixNotFound = false;
                        break;
                    }
                }
                else {
                    if (message == "") {
                        message = "Invalid Mega Prefix";
                    }

                    prefixNotFound = true;

                }
            }
            if (prefixNotFound) {

                alert(message);
                focusWorking(txt_consignment);
                txt_consignment.disabled = false;
                txt_consignment.value = '';
                return;
            }
                        

            if (txt_consignment.value.toString().length != "12") {
                alert('Error: CN must have length of 12');
                focusWorking(txt_consignment);
                txt_consignment.disabled = false;
                txt_consignment.value = '';
                return;
            } 


            var rn = txt_runsheetNumber.textContent.trim();
            if (rn.trim() == "") {
                alert('Kindly Save Mega Runsheet First');
                focusWorking(txt_consignment);
                txt_consignment.disabled = false;
                txt_consignment.value = '';
                return;
            }



            var cn = txt_consignment.value.trim();
            if (cn.trim() == "") {
                alert('Enter Mega Consignment Number');
                focusWorking(txt_consignment);
                txt_consignment.disabled = false;
                return;
            }

            var isnum = /^\d+$/.test(cn);
            if (!isnum) {
                alert('Kindly Insert Proper Mega Consignment Number');
                focusWorking(txt_consignment);
                txt_consignment.value = '';
                txt_consignment.disabled = false;
                return;
            }

            for (var i = 1; i < tblDetails.rows.length; i++) {
                if (tblDetails.rows[i].cells[1].innerText.trim() == txt_consignment.value.trim()) {
                    alert('Consignment Already Scanned');
                    txt_consignment.value = "";
                    focusWorking(txt_consignment);
                    txt_consignment.disabled = false;
                    return;
                }
            }


            $.ajax({
                url: 'Runsheet_Mega.aspx/GetConsignmentDetail',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                datatype: 'json',
                data: "{'ConsignmentNumber':'" + txt_consignment.value.trim() + "','BranchCode':'" + txt_branchCode.textContent.trim() + "','expressCenter':'" + txt_expressCenter.textContent.trim() + "','User':'" + txt_user.textContent.trim() + "'}",
                success: function (result) {
                    var resp = result.d;
                    count1 = 0;
                    count2 = 0;


                    if (resp.status == "0") {
                        alert(resp.cause);
                        focusWorking(txt_consignment);
                        txt_consignment.value = "";
                        txt_consignment.disabled = false;
                        return;
                    }
                    else if (resp.status == "1") {

                        AddConsignment(resp, "1", txt_consignment.value.trim());
                        txt_consignment.disabled = false;

                    }
                    else {
                        alert(resp.cause);
                        focusWorking(txt_consignment);
                        txt_consignment.value = "";
                        txt_consignment.disabled = false;
                        return;
                    }


                },
                error: function () {

                    alert("Oops Something went wrong. Please contact I.T. Support");
                    focusWorking(txt_consignment);
                    txt_consignment.disabled = false;
                    return;
                },
                failure: function () {

                    alert("Oops Something went wrong. Please contact I.T. Support");
                    focusWorking(txt_consignment);
                    txt_consignment.disabled = false;
                    return;
                }

            })


        }
    </script>
    <%--Adding Consignment in Table--%>
    <script type="text/javascript">
        function AddConsignment(cn, AddMode, ConsignmentNumber) {
            var mode = document.getElementById('<%= rbtn_Mode.ClientID %>');
            var inputs = mode.getElementsByTagName('input');
            var branches = document.getElementById('branches');
            var selectedValue = "";
            for (var i = 0; i < inputs.length; i++) {
                if (inputs[i].checked) {
                    selectedValue = inputs[i].value;
                    break;
                }

            }

            var consignments = document.getElementById('tblDetails');


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

            var hid3 = document.createElement('input');
            hid3.type = 'hidden';
            hid3.id = 'hid';


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
            newTr.cells[1].innerText = ConsignmentNumber;
            col = newTr.insertCell(2);
            newTr.cells[2].appendChild(dd_origin);
            newTr.cells[2].style.textAlign = 'left';
            //newTr.cells[2].childNodes[0].onchange = CalculateTotalWeight.bind(newTr.cells[2].childNodes[0]);
            col = newTr.insertCell(3);
            newTr.cells[3].appendChild(dd_destination);

            hid.value = cn.isNew;
            hid2.value = cn.isNew;
            hid3.value = cn.isCOD;
            newTr.cells[3].appendChild(hid);
            newTr.cells[3].appendChild(hid2);
            newTr.cells[3].appendChild(hid3);


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

            var cnnumber = document.getElementById('<%= txt_consignment.ClientID %>');
            focusWorking(cnnumber);
            cnnumber.value = "";
            CalculateRows();
            cnnumber.disabled = false;
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

    <script type="text/javascript">
        function CalculateRows() {
            var tbl = document.getElementById('tblDetails');
            var cnt = document.getElementById('lbl_cnt');

            cnt.textContent = 'Total Consignments: ' + (tbl.rows.length - 1).toString();
        }
    </script>
    <%--Saving Runsheet///////////////////////Shaheer/////////////////////////////////--%>
    <script type="text/javascript">
        function SaveRunsheet() {
            try {
                document.getElementById("save_r").disabled = true;
                var tblDetails = document.getElementById('tblDetails');
                var mode = document.getElementById('<%= rbtn_Mode.ClientID %>');
                var txt_runsheet = document.getElementById('<%= txt_runsheetNumber.ClientID %>');
                var div_date = document.getElementById('<%= div_date.ClientID %>');
                var div_dateDisplay = document.getElementById('<%= div_dateDisplay.ClientID %>');
                var newDate = document.getElementById('<%= picker1.ClientID %>');
                var updateDate = document.getElementById('<%= txt_date.ClientID %>');
                var ddl_runsheetType = document.getElementById('<%= ddl_runsheetType.ClientID %>');
                var txt_routeCode = document.getElementById('<%= txt_routeCode.ClientID %>');
                var dd_route = document.getElementById('<%= ddl_route.ClientID %>');
                var txt_count = document.getElementById('<%= lbl_count.ClientID %>');
                var ddl_rider = document.getElementById('<%= ddl_riders.ClientID %>');
                var txt_rider = document.getElementById('<%= txt_riderno.ClientID %>');

                var txt_branchCode = document.getElementById('<%= txt_branchCode.ClientID %>');
                var txt_zoneCode = document.getElementById('<%= txt_zoneCode.ClientID %>');
                var txt_user = document.getElementById('<%= txt_user.ClientID %>');
                var txt_expressCenter = document.getElementById('<%= txt_expressCenter.ClientID %>');

                if (txt_runsheet.textContent == '') {
                    alert('Kindly Save Runsheet Master Data First');
                    return;
                }
                if (tblDetails.rows.length == 1) {
                    alert('Kindly Add CN first!');
                    return;
                }

                var meterEnd = document.getElementById('<%= txt_meterEnd.ClientID %>');
                var meterStart = document.getElementById('<%= txt_meterStart.ClientID %>');
                var ddvehicleType = document.getElementById('<%= ddl_vehicleType.ClientID %>');
                var ddVehicle = document.getElementById('<%= ddl_vehicle.ClientID %>');
                var txtVehicle = document.getElementById('<%= txt_vehicleNumber.ClientID %>');

                var date = newDate.value;

                var runsheetType = ddl_runsheetType.options[ddl_runsheetType.options.selectedIndex].value;
                var RiderName = ddl_rider.options[ddl_rider.options.selectedIndex].text;

                var routeCode = txt_routeCode.value;
                var riderCode = txt_rider.value;
                var meterStartValue = meterStart.value;
                var meterEndValue = meterEnd.value;
                var vehicle = txtVehicle.value;
                var vehicleTypeValue = ddvehicleType.options[ddvehicleType.options.selectedIndex].value;
                var jsonObject = { Header: {}, Details: [] }
                var Head = {
                    status: "",
                    cause: "",
                    RunsheetNumber: txt_runsheet.textContent.trim(),
                    BranchCode: txt_branchCode.textContent,
                    ZoneCode: txt_zoneCode.textContent,
                    ExpressCenterCode: txt_expressCenter.textContent,
                    CreatedBy: txt_user.textContent,
                    RunsheetDate: date,
                    RunsheetType: runsheetType,
                    RouteCode: routeCode,
                    RouteName: "",
                    RiderCode: riderCode,
                    RiderName: RiderName,
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
                    var cod = tr.cells[3].childNodes[3].value;
                    var consignment = {
                        status: "",
                        cause: "",
                        ConsignmentNumber: tr.cells[1].innerText.trim(),
                        Origin: originValue,
                        Destination: destinationValue,
                        OriginName: originName,
                        DestinationName: destinationName,
                        ConsignmentType: contype,
                        ConsignmentTypeName: "",
                        SortOrder: "",
                        isNew: isNew,
                        removeable: removeable,
                        isCOD: cod
                    }

                    jsonObject.Details.push(consignment);
                    save = true;
                }


                if (save) {
                    $.ajax({

                        url: 'Runsheet_Mega.aspx/SaveRunsheet',
                        type: 'POST',
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        data: JSON.stringify(jsonObject),
                        success: function (result) {
                            var resp = result.d;
                            if (resp[0] == "0" || resp[0] == "2") {
                                alert(resp[1].toString());
                                loader.style.display = 'none';
                                document.getElementById('save_r').disabled = false;
                                return;
                            }
                            else if (resp[0] == "1") {
                                loader.style.display = 'none';

                                alert('Runsheet Saved. Runsheet Number: ' + resp[1].toString());
                                window.open('RunsheetInvoice_Plain.aspx?Xcode=' + resp[1] + '&RCode=' + routeCode, '_blank');
                                window.open('Runsheet_Mega.aspx', '_parent');
                            }
                        },

                        error: function () {
                            alert('Error in Generating Runsheet');
                            loader.style.display = 'none';
                            document.getElementById('save_r').disabled = false;
                            return;

                        },
                        failure: function () {
                            alert('Failed to Generate Runsheet');
                            loader.style.display = 'none';
                            document.getElementById('save_r').disabled = false;
                            return;

                        }


                    });
                }


            } catch (err) {
                loader.style.display = 'none';
                alert(err.Message);
                document.getElementById('save_r').disabled = false;
            }

        }

    </script>

    <%--Vehicle Type Change///////////////////////Shaheer/////////////////////////////////--%>
    <script type="text/javascript">
        function SetVehicle(dd) {
            var txt = document.getElementById('<%= txt_vehicleNumber.ClientID %>');
            if (dd.options[dd.options.selectedIndex].value == "0") {
                alert('Select Vehicle');
                txt.value = "";
                var vehicleType = document.getElementById('<%= ddl_vehicleType.ClientID %>');
                vehicleType.options[0].selected = true;

            }
            else {
                txt.value = dd.options[dd.options.selectedIndex].text;
                CheckVehicleType(dd);
            }

        }
        function CheckVehicleType(dd) {

            var vehicleGrid = document.getElementById('<%= vehicleTypes.ClientID %>');
            var vehicleType = document.getElementById('<%= ddl_vehicleType.ClientID %>');
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

        function FindVehicle(txt) {

            var dd_vehicle = document.getElementById('<%= ddl_vehicle.ClientID %>');
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
                if (txt.value.trim() != "") {
                    for (var i = dd_vehicle.options.length - 1; i < dd_vehicle.options.length && i > -1; i--) {
                        var currentValue = dd_vehicle.options[i].text;
                        if (currentValue.trim().toUpperCase() == "RENTED") {
                            dd_vehicle.options[i].selected = true;
                            break;
                        }
                    }

                }
                else {
                    txt.value = "";
                    dd_vehicle.options[0].selected = true;
                    var vehicleType = document.getElementById('<%= ddl_vehicleType.ClientID %>');
                    vehicleType.options[0].selected = true;
                }
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
    <div id="loader" style="width: 86%; height: 150%; background-color: #000000; position: absolute; display: none; opacity: 0.5">
    </div>
    <div class="search">
        <a href="SearchRunSheet.aspx">Search RunSheet</a>
    </div>
    <asp:Label ID="Errorid" runat="server" Font-Bold="true"></asp:Label>

    <table style="font-family: Calibri; font-size: medium; padding-bottom: 0px !important; padding-top: 0px !important"
        class="input-form">
        <tr style="padding: 0px 0px 0px 0px !important; text-align: center;">
            <td colspan="8" style="padding-bottom: 1px !important; padding-top: 1px !important; width: 1% !important;">
                <h3 style="font-family: Calibri; margin: 0px !important; font-variant: small-caps;">Mega Runsheet Info.</h3>
            </td>
        </tr>
        <tr style="padding: 0px 0px 0px 0px !important;">
            <td colspan="8" style="padding-bottom: 1px !important; padding-top: 1px !important;">
                <div>
                    <b>Form Mode</b>
                </div>
                <asp:RadioButtonList ID="rbtn_Mode" runat="server" RepeatColumns="2" RepeatDirection="Horizontal"
                    AutoPostBack="false">
                    <asp:ListItem Value="NEW" Selected="True">NEW</asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <td class="field" style="width: 10% !important;">Mega Runsheet#.
            </td>
            <td class="input-field" style="width: 15% !important;">
                <asp:Label ID="txt_runsheetNumber" runat="server"></asp:Label>
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
                    <asp:TextBox ID="txt_date" runat="server" Enabled="false" Width="80%"></asp:TextBox>
                </div>
            </td>
            <td class="space" style="margin: 0px 0px 0px 0px !important; width: 5% !important;"></td>
            <td class="field" style="width: 10% !important;">Runsheet Type
            </td>
            <td class="input-field" style="width: 15% !important;">
                <asp:DropDownList ID="ddl_runsheetType" runat="server" AppendDataBoundItems="true"
                    Width="100%">
                    <asp:ListItem Value="0">Select Type</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="field" style="width: 10% !important;">Route Code.
            </td>
            <td class="input-field" style="width: 15% !important;">
                <asp:TextBox ID="txt_routeCode" runat="server" AutoPostBack="false" Style="text-transform: uppercase" onchange="RouteChange(this);"></asp:TextBox>
            </td>
            <td class="space" style="margin: 0px 0px 0px 0px !important; width: 5% !important;"></td>
            <td class="field" style="width: 10% !important;">Route
            </td>
            <td class="input-field" style="width: 15% !important;">
                <asp:DropDownList ID="ddl_route" runat="server" AppendDataBoundItems="true" Width="100%"
                    onchange="RouteChange(this);" AutoPostBack="false">
                    <asp:ListItem Value="0">Select Route</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td class="space" style="margin: 0px 0px 0px 0px !important; width: 5% !important;"></td>
        </tr>
        <tr>
            <td class="field" style="width: 10% !important;">Rider No.
            </td>
            <td class="input-field" style="width: 15% !important;">
                <asp:TextBox ID="txt_riderno" runat="server" onchange="RiderChange(this);" Enabled="false"></asp:TextBox>
            </td>
            <td class="space" style="margin: 0px 0px 0px 0px !important; width: 5% !important;"></td>
            <td class="field" style="width: 10% !important;">Rider Name
            </td>
            <td class="input-field" style="width: 15% !important;">
                <asp:DropDownList ID="ddl_riders" runat="server" AppendDataBoundItems="true" Width="100%"
                    onchange="RiderChange(this);" Enabled="false">
                </asp:DropDownList>
            </td>
            <td class="space" style="margin: 0px 0px 0px 0px !important; width: 5% !important;"></td>
            <td class="field" style="width: 10% !important;">LandMark
            </td>
            <td class="input-field" colspan="2" style="width: 24% !important;">
                <asp:TextBox ID="txt_laskMark" TextMode="MultiLine" runat="server" Enabled="false"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="field" style="width: 10% !important;">Vehicle
            </td>
            <td class="input-field" style="width: 15% !important;">
                <asp:TextBox ID="txt_vehicleNumber" runat="server" Width="100%" onchange="FindVehicle(this)" Height="22px"></asp:TextBox>
            </td>
            <td class="space" style="margin: 0px 0px 0px 0px !important; width: 5% !important;"></td>
            <td class="field" style="width: 10% !important;">Vehicle Number</td>
            <td class="input-field" style="width: 16% !important;">
                <asp:DropDownList ID="ddl_vehicle" runat="server" Width="100%" CssClass="dropdown"
                    onchange="SetVehicle(this)" AppendDataBoundItems="true">
                    <asp:ListItem Value="0">Select Vehicle</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td class="space" style="margin: 0px 0px 0px 0px !important; width: 5% !important;"></td>
            <td class="field" style="width: 10% !important;">Vehicle Type
            </td>
            <td class="input-field" style="width: 16% !important;">
                <asp:DropDownList ID="ddl_vehicleType" runat="server" CssClass="dropdown" AppendDataBoundItems="true">
                    <asp:ListItem Value="0">Select Vehicle Type</asp:ListItem>
                </asp:DropDownList>
            </td>
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
            <td class="field" style="width: 10% !important;">User Code:
            </td>
            <td class="input-field" style="width: 15% !important;">
                <asp:Label ID="txt_user" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="field" style="width: 10% !important;">Express Center:
            </td>
            <td class="input-field" style="width: 15% !important;">
                <asp:Label ID="txt_expressCenter" runat="server" />
            </td>
            <td class="space" style="margin: 0px 0px 0px 0px !important; width: 5% !important;"></td>
            <td class="field" style="width: 10% !important;">Branch Code:
            </td>
            <td class="input-field" style="width: 15% !important;">
                <asp:Label ID="txt_branchCode" runat="server" />
            </td>
            <td class="space" style="margin: 0px 0px 0px 0px !important; width: 5% !important;"></td>
            <td class="field" style="width: 10% !important;">Zone Code:
            </td>
            <td class="input-field" style="width: 15% !important;">
                <asp:Label ID="txt_zoneCode" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="field" colspan="8" style="width: 100% !important; text-align: center">
                <asp:Button ID="btn_saveMaster" runat="server" Text="Save Runsheet" CssClass="button" OnClick="btn_saveMaster_Click"
                    OnClientClick="return SaveMasterRunsheet();" />
            </td>
        </tr>
    </table>
    <fieldset style="border-radius: 5px !important; font-weight: bold; font-size: medium; padding: 10px !important; margin: 18px !important; width: 95%;">
        <legend>Mega Consignments</legend>
        <table style="width: 100%">
            <tr>
                <td style="width: 13%; font-weight: bold;">Mega CN Number
                </td>
                <td style="width: 20%; padding-right: 60px;">
                    <asp:TextBox ID="txt_consignment" runat="server" onkeypress="return isNumberKey(event);" MaxLength="20" onchange="ConsignmentChange(this);"></asp:TextBox>
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
                    </span>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <table id="tblDetails" class="tblDetails">
            <tr class="headerRow">
                <td style="width: 7%; text-align: center;"></td>
                <td style="width: 13%;">Mega Consignment Number
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
    <div style="display: none;">
        <asp:GridView ID="cnControls" runat="server">
            <Columns>
                <asp:BoundField HeaderText="Prefix" DataField="Prefix" />
                <asp:BoundField HeaderText="Length" DataField="Length" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
