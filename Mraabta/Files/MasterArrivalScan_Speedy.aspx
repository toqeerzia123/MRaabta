<%@ Page Language="C#" Title="" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.master" CodeBehind="MasterArrivalScan_Speedy.aspx.cs" Inherits="MRaabta.Files.MasterArrivalScan_Speedy" %>


<%@ Register TagPrefix="Ajax1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="http://code.jquery.com/jquery-1.7.1.min.js"></script>
    <style>
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
    <script language="javascript" type="text/javascript">

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
    <%-- ChangeMode --%>
    <script type="text/javascript">
        function ChangeMode(rb) {

            var mode = document.getElementById('<%= rbtn_mode.ClientID %>');
            var arrivalID = document.getElementById('<%= txt_arrivalID.ClientID %>');
            var riderCode = document.getElementById('<%= txt_ridercode.ClientID %>');
            var riderName = document.getElementById('<%= txt_ridername.ClientID %>');
            var weight = document.getElementById('<%= txt_weight.ClientID %>');
            var btn_refresh = document.getElementById('btn_refresh');
            var serviceType = document.getElementById('<%= dd_servicetype.ClientID %>');
            var conType = document.getElementById('<%= dd_contype.ClientID %>');
            var cn = document.getElementById('txt_consignment');
            var consignments = document.getElementById('tbl_details');
            var currentMode = document.getElementById('<%= hd_currentMode.ClientID %>');
            var cn = document.getElementById('txt_consignment');
            var bt_1 = document.getElementById('bt_1');
            var btn_reset1 = document.getElementById('<%= btn_reset1.ClientID %>');
            //var btn_reset1 = document.getElementById('btn_reset1');
            var bt_2 = document.getElementById('bt_2');
            var btn_reset2 = document.getElementById('<%= btn_reset2.ClientID %>');
            //var btn_reset2 = document.getElementById('btn_reset2');
            var lbl_message = document.getElementById('<%= lbl_message.ClientID %>');
            var btn_saveArrival = document.getElementById('<%= btn_saveArrival.ClientID %>');

            var inputs = mode.getElementsByTagName('input');

            var selectedValue = "";
            for (var i = 0; i < inputs.length; i++) {
                if (inputs[i].checked) {
                    selectedValue = inputs[i].value;
                    break;
                }

            }

            if (consignments.rows.length > 1) {
                var confirmation = confirm('This will erase all the current data on the page. Do you want to Continue?');
                if (confirmation == true) {
                    currentMode.value = selectedValue; //setting new Current Mode
                    if (selectedValue == "0") {
                        arrivalID.disabled = true;
                        arrivalID.value = "";
                    }
                    else {
                        arrivalID.disabled = false;
                        arrivalID.value = "";
                    }
                    for (var i = 1; i < consignments.rows.length;) {
                        consignments.deleteRow(1);
                    }
                    riderCode.value = "";
                    riderName.value = "";
                    weight.value = "";
                }
                else {
                    //setting mode back to current mode
                    for (var i = 0; i < inputs.length; i++) {
                        if (inputs[i].value == currentMode.value) {
                            inputs[i].checked = true;
                            break;
                        }

                    }
                }
            }
            else {
                if (selectedValue == "0") {
                    arrivalID.disabled = true;
                    arrivalID.value = "";
                    cn.disabled = true;
                    bt_1.disabled = true;
                    bt_2.disabled = true;
                    btn_reset1.disabled = true;
                    btn_reset2.disabled = true;
                    riderCode.disabled = false;
                    btn_refresh.style.visibility = "hidden";
                    lbl_message.style.visibility = "visible";
                    btn_saveArrival.style.visibility = "visible";
                }
                else {
                    arrivalID.disabled = false;
                    arrivalID.value = "";
                    cn.disabled = false;
                    bt_1.disabled = false;
                    bt_2.disabled = false;
                    btn_reset1.disabled = false;
                    btn_reset2.disabled = false;
                    riderCode.value = "";
                    riderName.value = "";
                    riderCode.disabled = true;
                    riderName.disabled = true;
                    btn_refresh.style.visibility = "visible";
                    lbl_message.style.visibility = "hidden";
                    btn_saveArrival.style.visibility = "hidden";

                }
            }


        }
    </script>
    <%--Calculate Total Weight--%>
    <script type="text/javascript">
        function CalculateTotalWeight() {
            var weight = document.getElementById('<%= txt_weight.ClientID %>');
            var tbl_consignments = document.getElementById('tbl_details');
            var cn = document.getElementById('txt_consignment');
            var bt_1 = document.getElementById('bt_1');
            var bt_2 = document.getElementById('bt_2');

            var chk = false;
            var validNumber = new RegExp(/^\d+(\.\d+)?$/);

            for (var i = 1; i < tbl_consignments.rows.length; i++) {
                var tr = tbl_consignments.rows[i];
                if (validNumber.test(tbl_consignments.rows[i].cells[4].childNodes[0].value) && (tbl_consignments.rows[i].cells[4].childNodes[0].value != "0")) {
                    tbl_consignments.rows[i].cells[5].childNodes[0].style.backgroundColor = "";
                    tr.style.backgroundColor = '';
                }
                else {
                    tbl_consignments.rows[i].cells[5].childNodes[0].style.backgroundColor = "red";
                    tr.style.backgroundColor = '#ffb7c1';
                    chk = true;
                }
            }

            if (chk) {
                cn.disabled = true;
                bt_1.disabled = true;
                bt_2.disabled = true;
                alert('Kindly insert proper weight');
                return;
            }
            else {
                cn.disabled = false;
                bt_1.disabled = false;
                bt_2.disabled = false;
                //validate();
            }

            var totalWeight = 0;
            var tempCnWeight = 0;
            var tempBagWeight = 0;


            for (var i = 1; i < tbl_consignments.rows.length; i++) {
                var temp = parseFloat(tbl_consignments.rows[i].cells[4].childNodes[0].value);
                if (!isNaN(temp)) {
                    tempCnWeight = tempCnWeight + temp;
                }
            }

            weight.value = tempCnWeight;
        }

        /////////////////////////////////////////////Pieces Calculation//////////////////////////////////////////
        function validate() {
            var tbl_consignments = document.getElementById('tbl_details');

            var tbl_consignments = document.getElementById('tbl_details');
            var cn = document.getElementById('txt_consignment');
            var bt_1 = document.getElementById('bt_1');
            var bt_2 = document.getElementById('bt_2');


            var chk = false;
            var validNumber = new RegExp(/^[1-9][0-9]*$/);
            for (var i = 1; i < tbl_consignments.rows.length; i++) {
                if (validNumber.test(tbl_consignments.rows[i].cells[5].childNodes[0].value)) {
                    tbl_consignments.rows[i].cells[5].childNodes[0].style.backgroundColor = "";
                }
                else {
                    tbl_consignments.rows[i].cells[5].childNodes[0].style.backgroundColor = "red";
                    chk = true;
                }
            }
            if (chk) {
                cn.disabled = true;
                bt_1.disabled = true;
                bt_2.disabled = true;
                alert('Kindly insert proper Piece');
                return;
            }
            else {
                cn.disabled = false;
                bt_1.disabled = false;
                bt_2.disabled = false;
                CalculateTotalWeight();
            }
        }
    </script>
    <%--Getting Arrival Details--%>
    <script type="text/javascript">

        function GetArrivalDetails(txt) {
            var hd_BranchCode = document.getElementById('<%= hd_BranchCode.ClientID %>');
            var arrivalID = document.getElementById('<%= txt_arrivalID.ClientID %>');
            var consignments = document.getElementById('tbl_details');
            $.ajax({

                url: 'MasterArrivalScan_speedy.aspx/GetArrivalDetails',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                data: "{'arrivalID':'" + txt.value + "','BranchCode':'" + hd_BranchCode.value + "'}",
                success: function (result) {
                    if (result.d.status == "1") {
                        alert('Arrival Details Found');


                        var riderCode = document.getElementById('<%= txt_ridercode.ClientID %>');
                        var riderName = document.getElementById('<%= txt_ridername.ClientID %>');
                        var weight = document.getElementById('<%= txt_weight.ClientID %>');
                        var serviceType = document.getElementById('<%= dd_servicetype.ClientID %>');
                        var conType = document.getElementById('<%= dd_contype.ClientID %>');
                        var cn = document.getElementById('txt_consignment');
                        var currentMode = document.getElementById('<%= hd_currentMode.ClientID %>');

                        riderCode.value = result.d.masterParameters.RiderCode.toString();
                        riderName.value = result.d.masterParameters.RiderName.toString();

                        var tempWeight = 0;
                        var totalWeight = 0;

                        //Binding the Table
                        for (var i = 0; i < result.d.consignments.length; i++) {
                            var newTr = consignments.insertRow(1);  //document.createElement('tr');  //consignments.rows[0].cloneNode(true);
                            newTr.className = 'DetailRow';

                            var btn_remove = document.createElement('input');
                            btn_remove.type = 'button';
                            btn_remove.className = 'button button1';
                            btn_remove.value = 'Remove';
                            btn_remove.style.marginTop = '2px';
                            btn_remove.style.marginBottom = '2px';
                            btn_remove.style.width = '80%';
                            btn_remove.style.display = 'none';
                            var txtWeight = document.createElement('input');
                            txtWeight.type = 'text';
                            txtWeight.className = 'textBox';
                            txtWeight.value = '0.5';
                            txtWeight.style.width = '70%';
                            txtWeight.style.textAlign = 'center';
                            txtWeight.disabled = true;

                            var txtPieces = document.createElement('input');
                            txtPieces.type = 'text';
                            txtPieces.className = 'textBox';
                            txtPieces.value = '1';
                            txtPieces.style.width = '70%';
                            txtPieces.style.textAlign = 'center';
                            txtPieces.disabled = true;
                            var conTypeID = document.createElement('input');
                            conTypeID.type = 'text';
                            conTypeID.style.display = 'none';
                            conTypeID.value = result.d.masterParameters.ConType;
                            var col = newTr.insertCell(0);


                            newTr.cells[0].appendChild(btn_remove);
                            newTr.cells[0].childNodes[0].onclick = RemoveConsignment.bind(newTr.cells[0].childNodes[0], newTr.cells[0].childNodes[0]);
                            col = newTr.insertCell(1);
                            newTr.cells[1].innerText = result.d.consignments[i].ConsignmentNumber;
                            col = newTr.insertCell(2);
                            newTr.cells[2].innerText = result.d.consignments[i].ServiceType;
                            col = newTr.insertCell(3);
                            newTr.cells[3].innerText = result.d.consignments[i].ConTypeName;
                            newTr.cells[3].appendChild(conTypeID);
                            col = newTr.insertCell(4);
                            newTr.cells[4].innerText = "";
                            newTr.cells[4].appendChild(txtWeight);
                            newTr.cells[4].childNodes[0].onchange = CalculateTotalWeight.bind(newTr.cells[0].childNodes[0]);
                            newTr.cells[4].childNodes[0].value = result.d.consignments[i].Weight;
                            col = newTr.insertCell(5);
                            newTr.cells[5].innerText = "";
                            newTr.cells[5].appendChild(txtPieces);
                            newTr.cells[5].childNodes[0].onchange = validate.bind(newTr.cells[0].childNodes[0]);


                            tempWeight = parseFloat(result.d.consignments[i].Weight);
                            if (!isNaN(tempWeight)) {
                                totalWeight = totalWeight + tempWeight;
                            }

                            newTr.cells[5].childNodes[0].value = result.d.consignments[i].Pieces;

                            newTr.cells[4].style.textAlign = "center";
                            newTr.cells[5].style.textAlign = "center";
                            col = newTr.insertCell(6);
                            newTr.cells[6].innerText = riderCode.value;
                            col = newTr.insertCell(7);
                            newTr.cells[7].innerText = riderName.value;
                        }
                        var count = document.getElementById('lbl_cnCount');
                        count.innerHTML = 'Consignment Count: ' + (consignments.rows.length - 1).toString();
                        arrivalID.disabled = true;
                        validate();
                        //   weight.value = totalWeight.toString();
                    }
                    else {
                        alert("Arrival Details Not Found. Error: " + result.d.reason);
                        ClearTable(consignments);
                        arrivalID.disabled = false;
                        arrivalID.value = "";
                        arrivalID.focus();
                    }
                },

                error: function () {
                    alert('Error in Generating Arrival');
                    arrivalID.disabled = false;
                    arrivalID.value = "";
                    arrivalID.focus();

                },
                failure: function () {
                    alert('Failed to Generate Arrival');
                    arrivalID.disabled = false;
                    arrivalID.value = "";
                    arrivalID.focus();
                }
            });
        }
    </script>
    <%--Resetting Table--%>
    <script type="text/javascript">
        function ClearTable(tbl_consignments) {
            var rowCount = tbl_consignments.rows.length;
            for (var i = rowCount - 1; i > 0; i--) {
                tbl_consignments.deleteRow(i);
            }
        }
    </script>
    <%--Adding Consignment--%>
    <script type="text/javascript">
        function AddConsignment() {
            var mode = document.getElementById('<%= rbtn_mode.ClientID %>');
            var arrivalID = document.getElementById('<%= txt_arrivalID.ClientID %>');
            var riderCode = document.getElementById('<%= txt_ridercode.ClientID %>');
            var riderName = document.getElementById('<%= txt_ridername.ClientID %>');
            var weight = document.getElementById('<%= txt_weight.ClientID %>');
            var serviceType = document.getElementById('<%= dd_servicetype.ClientID %>');
            var conType = document.getElementById('<%= dd_contype.ClientID %>');
            var cn = document.getElementById('txt_consignment');
            var consignments = document.getElementById('tbl_details');


            cn.disabled = true;


            var inputs = mode.getElementsByTagName('input');

            var selectedValue = "";
            for (var i = 0; i < inputs.length; i++) {
                if (inputs[i].checked) {
                    selectedValue = inputs[i].value;
                    break;
                }

            }
            if (selectedValue == "1") {
                if (arrivalID.value.trim() == "") {
                    alert('Enter Arrival ID');
                    cn.disabled = false;
                    cn.value = "";
                    cn.focus();
                    return;
                }
            }

            var isnum = /^\d+$/.test(cn.value.trim());
            if (!isnum) {
                alert('Kindly Insert Proper Consignment Number');
                cn.disabled = false;
                cn.value = "";
                cn.focus();

                return;
            }


            if (riderCode.value == '' || riderName.value == '') {
                alert('Enter Rider');
                cn.disabled = false;
                return false;
            }
            if (serviceType.options[serviceType.options.selectedIndex].valu == '0') {
                alert('Select Service Type');
                cn.disabled = false;
                return false;
            }
            if (conType.options[conType.options.selectedIndex].value == '0') {
                alert('Select Consignment Type');
                cn.disabled = false;
                return false;
            }


            var controlGrid = document.getElementById('<%= cnControls.ClientID %>');
            var message = "";
            for (var i = 1; i < controlGrid.rows.length; i++) {
                var row = controlGrid.rows[i];
                var prefix = row.cells[0].innerText;
                var length_ = parseInt(row.cells[1].innerText);
                if (prefix == "52190") {
                    var a = 0;
                }
                cn_ = cn.value.trim()
                if (cn_.substring(0, prefix.length) == prefix) {
                    if (cn_.length != length_) {
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
                cn.disabled = false;
                cn.value = '';
                cn.focus();
                return false;
            }

            for (var i = 1; i < consignments.rows.length; i++) {

                if (consignments.rows[i].cells[1].innerText.trim() == cn.value.trim()) {
                    alert('Consignment Already Scanned');
                    cn.disabled = false;
                    cn.value = '';
                    cn.focus();
                    return;
                }
            }

            PageMethods.ChkRunsheet(cn.value.trim(), onSuccess, onFailure);

            function onSuccess(result) {

                if (result.toString() == "N/A") {
                    if (weight.value == "") {
                        weight.value = "0";
                    }
                    var tempWeight = parseFloat(weight.value);
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

                    var conTypeID = document.createElement('input');
                    conTypeID.type = 'text';
                    conTypeID.style.display = 'none';
                    conTypeID.value = conType.options[conType.options.selectedIndex].value;
                    var col = newTr.insertCell(0);


                    newTr.cells[0].appendChild(btn_remove);
                    newTr.cells[0].childNodes[0].onclick = RemoveConsignment.bind(newTr.cells[0].childNodes[0], newTr.cells[0].childNodes[0]);
                    col = newTr.insertCell(1);
                    newTr.cells[1].innerText = cn.value.trim();
                    col = newTr.insertCell(2);
                    newTr.cells[2].innerText = serviceType.options[serviceType.options.selectedIndex].value;
                    col = newTr.insertCell(3);
                    newTr.cells[3].innerText = conType.options[conType.options.selectedIndex].text;
                    newTr.cells[3].appendChild(conTypeID);
                    col = newTr.insertCell(4);
                    newTr.cells[4].innerText = "";
                    col = newTr.insertCell(5);
                    newTr.cells[5].innerText = "";
                    newTr.cells[4].appendChild(txtWeight);
                    newTr.cells[4].childNodes[0].onchange = CalculateTotalWeight.bind(newTr.cells[0].childNodes[0]);
                    newTr.cells[5].appendChild(txtPieces);
                    newTr.cells[5].childNodes[0].onchange = validate.bind(newTr.cells[0].childNodes[0]);
                    newTr.cells[4].style.textAlign = "center";
                    newTr.cells[5].style.textAlign = "center";
                    col = newTr.insertCell(6);
                    newTr.cells[6].innerText = riderCode.value;
                    col = newTr.insertCell(7);
                    newTr.cells[7].innerText = riderName.value;
                    weight.value = parseFloat(tempWeight) + 0.5;

                    col = newTr.insertCell(8); // Cod Amount Column
                    newTr.cells[8].innerText = '-';


                    cn.disabled = false;
                    cn.focus();
                    cn.value = '';
                    var count = document.getElementById('lbl_cnCount');
                    count.innerHTML = 'Consignment Count: ' + (consignments.rows.length - 1).toString();
                    CalculateTotalWeight();

                }

                else if (result[0][1].toString() == "RS-Return to Shipper" || result[0][1].toString() == "D-DELIVERED" || result[0][1].toString() == "Arrived" /*|| result[0][1].toString() == "Void"*/) {
                    var message = result[0][1].toString();
                    alert('This CN ' + cn.value + ' is already Mark as ' + message);
                    cn.disabled = false;
                    cn.value = '';
                    cn.focus();
                    return;
                }
                else {


                    var tempWeight = weight.value;
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
                    txtWeight.value = result[0][2].toString();
                    txtWeight.style.width = '70%';
                    txtWeight.style.textAlign = 'center';

                    var txtPieces = document.createElement('input');
                    txtPieces.type = 'text';
                    txtPieces.className = 'textBox';
                    txtPieces.value = result[0][3].toString();
                    txtPieces.style.width = '70%';
                    txtPieces.style.textAlign = 'center';

                    var conTypeID = document.createElement('input');
                    conTypeID.type = 'text';
                    conTypeID.style.display = 'none';
                    conTypeID.value = conType.options[conType.options.selectedIndex].value;
                    var col = newTr.insertCell(0);


                    newTr.cells[0].appendChild(btn_remove);
                    newTr.cells[0].childNodes[0].onclick = RemoveConsignment.bind(newTr.cells[0].childNodes[0], newTr.cells[0].childNodes[0]);
                    col = newTr.insertCell(1);
                    newTr.cells[1].innerText = cn.value;
                    col = newTr.insertCell(2);
                    newTr.cells[2].innerText = serviceType.options[serviceType.options.selectedIndex].value;
                    col = newTr.insertCell(3);
                    newTr.cells[3].innerText = conType.options[conType.options.selectedIndex].text;
                    newTr.cells[3].appendChild(conTypeID);
                    col = newTr.insertCell(4);
                    newTr.cells[4].innerText = "";
                    col = newTr.insertCell(5);
                    newTr.cells[5].innerText = "";
                    newTr.cells[4].appendChild(txtWeight);
                    newTr.cells[4].childNodes[0].onchange = CalculateTotalWeight.bind(newTr.cells[0].childNodes[0]);
                    //newTr.cells[4].childNodes[0].onchange = CalculateTotalWeight.bind(newTr.cells[0].childNodes[0]);
                    newTr.cells[5].appendChild(txtPieces);
                    newTr.cells[5].childNodes[0].onchange = validate.bind(newTr.cells[0].childNodes[0]);
                    newTr.cells[4].style.textAlign = "center";
                    newTr.cells[5].style.textAlign = "center";
                    col = newTr.insertCell(6);
                    newTr.cells[6].innerText = riderCode.value;
                    col = newTr.insertCell(7);
                    newTr.cells[7].innerText = riderName.value;
                    weight.value = parseFloat(tempWeight) + parseFloat(result[0][2].toString());
                    col = newTr.insertCell(8);
                    newTr.cells[8].innerText = result[0][4].toString();

                    cn.disabled = false;
                    cn.focus();
                    cn.value = '';
                    var count = document.getElementById('lbl_cnCount');
                    count.innerHTML = 'Consignment Count: ' + (consignments.rows.length - 1).toString();
                    CalculateTotalWeight();
                }
            }

            function onFailure(result) {
                cn.disabled = false;
            }






        }
    </script>
    <%--Validating Riders--%>
    <script type="text/javascript">
        function ValidateRider(txt) {
            PageMethods.ValidateRider(txt.value.trim(), onSuccess);
        }
        function onSuccess(response, userContext, methodName) {
            var riderCode = document.getElementById('<%= txt_ridercode.ClientID %>');
            var riderName = document.getElementById('<%= txt_ridername.ClientID %>');
            var orgEc = document.getElementById('<%= hd_ExpressCenter.ClientID %>');

            var consignments = document.getElementById('tbl_details');
            if (response[2].toString() == "0") {
                alert('Invalid Rider Code');
                riderCode.focus();
                riderCode.value = "";
                riderName.value = "";
                return;
            }
            else {
                riderName.value = response[0].toString();
                orgEc.value = response[1].toString();
                for (var i = 1; i < consignments.rows.length; i++) {
                    var tr = consignments.rows[i];
                    tr.cells[6].innerText = riderCode.value;
                    tr.cells[7].innerText = riderName.value;

                }
            }
        }
    </script>
    <%--Saving Arrival--%>
    <script type="text/javascript">
        function SaveArrival() {

            bt_2.disable = true;
            bt_1.disable = true;

            var mode = document.getElementById('<%= rbtn_mode.ClientID %>');
            var arrivalID = document.getElementById('<%= txt_arrivalID.ClientID %>');
            var riderCode = document.getElementById('<%= txt_ridercode.ClientID %>');
            var riderName = document.getElementById('<%= txt_ridername.ClientID %>');
            var weight = document.getElementById('<%= txt_weight.ClientID %>');
            var serviceType = document.getElementById('<%= dd_servicetype.ClientID %>');
            var conType = document.getElementById('<%= dd_contype.ClientID %>');
            var cn = document.getElementById('txt_consignment');
            var consignments = document.getElementById('tbl_details');
            var orgEc = document.getElementById('<%= hd_ExpressCenter.ClientID %>');

            var hd_ExpressCenter_Session = document.getElementById('<%= hd_ExpressCenter_Session.ClientID %>');
            var hd_BranchCode = document.getElementById('<%= hd_BranchCode.ClientID %>');
            var hd_zoneCode = document.getElementById('<%= hd_zoneCode.ClientID %>');
            var hd_U_ID = document.getElementById('<%= hd_U_ID.ClientID %>');
            var hd_LocationID = document.getElementById('<%= hd_LocationID.ClientID %>');
            var hd_LocationName = document.getElementById('<%= hd_LocationName.ClientID %>');

            if (riderCode.value == "") {
                alert('Rider Code Missinig');
                riderCode.value = "";
                riderName.value = "";
                riderCode.focus();
                return;
            }

            var jsonObject = { consignments: [], MasterParameters: {} };
            if (consignments.rows.length <= 1) {
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

            for (var i = 1; i < consignments.rows.length; i++) {
                var consignment;
                var tr = consignments.rows[i];
                consignment =
                {
                    ConsignmentNumber: tr.cells[1].innerText.toString(),
                    Weight: tr.cells[4].childNodes[0].value.toString(),
                    Pieces: tr.cells[5].childNodes[0].value.toString()
                }
                jsonObject.consignments.push(consignment);
            }
            if (arrivalID.value == '' || arrivalID.value == null) {
                alert('Kindly Enter Arrival Information first. <|> .براہ مہربانی سب سے پہلے آمد کی معلومات درج کریں');
            }
            var MasterParam =
            {
                ArrivalID: arrivalID.value,
                RiderCode: riderCode.value,
                ServiceType: serviceType.options[serviceType.options.selectedIndex].value,
                ConType: conType.options[conType.options.selectedIndex].value,
                OriginExpressCenterCode: orgEc.value,
                TotalWeight: weight.value,
                Mode: selectedValue,
                hd_ExpressCenter_Session: hd_ExpressCenter_Session.value,
                hd_BranchCode: hd_BranchCode.value,
                hd_zoneCode: hd_zoneCode.value,
                hd_U_ID: hd_U_ID.value,
                hd_LocationID: hd_LocationID.value,
                hd_LocationName: hd_LocationName.value
            }
            jsonObject.MasterParameters = MasterParam;

            $.ajax({

                url: 'MasterArrivalScan_speedy.aspx/InsertArrival',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                data: JSON.stringify(jsonObject),
                success: function (result) {
                    var resp = result.d;
                    var errorFound = false;
                    var errorMessage = "";
                    for (var i = 1; i < consignments.rows.length; i++) {
                        var cn = consignments.rows[i].cells[1].innerText;
                        for (var j = resp.length - 1; j >= 0; j--) {
                            if (resp[j][1] == cn) {
                                if (resp[j][0] == "0") {
                                    errorFound = true;
                                    errorMessage = resp[j][2];
                                }
                                break;
                            }
                        }
                    }

                    if (errorFound) {
                        alert('Could Not Create Arrival. Error: ' + errorMessage);
                    }
                    else {
                        alert('Arrival Generated');
                        //Object.assign(document.createElement('a'), { target: '_blank', href: 'Arrival_print.aspx?XCode=' + resp[0][2].toString() }).click();
                        window.open('Arrival_print_speedy.aspx?XCode=' + resp[0][2].toString(), '_blank');
                        riderCode.focus();
                        riderCode.value = '';
                        ResetAll();
                    }
                },

                error: function () {
                    alert('Error in Generating Arrival');

                },
                failure: function () {
                    alert('Failed to Generate Arrival');


                }


            });
        }
    </script>
    <%--Resetting--%>
    <script type="text/javascript">
        function ResetAll() {
            location.reload();

        }
    </script>
    <%--Removing Consignment--%>
    <script type="text/javascript">
        function RemoveConsignment(btn) {
            var tr = btn.parentElement.parentElement;
            var table = tr.parentElement;
            table.deleteRow(tr.rowIndex);

            var count = document.getElementById('lbl_cnCount');
            count.innerHTML = 'Consignment Count: ' + (table.rows.length - 1).toString();
        }


    </script>
    <%--Saving Master Arrival--%>
    <script type="text/javascript">
        function SaveArrivalMaster(Success, Message) {
            var riderCode = document.getElementById('<%= txt_ridercode.ClientID %>');
            document.getElementById('<%=loader.ClientID %>').style.display = "none";
            var cn = document.getElementById('txt_consignment');
            var bt_1 = document.getElementById('bt_1');

            var btn_reset1 = document.getElementById('<%= btn_reset1.ClientID %>');
            //var btn_reset1 = document.getElementById('btn_reset1');
            var bt_2 = document.getElementById('bt_2');
            var btn_reset2 = document.getElementById('<%= btn_reset2.ClientID %>');
            //var btn_reset2 = document.getElementById('btn_reset2');
            if (Success == 'True') {
                cn.disabled = false;
                bt_1.disabled = false;
                bt_2.disabled = false;
                btn_reset1.disabled = false;
                btn_reset2.disabled = false;
                riderCode.disabled = true;

                alert('Please Enter Shipment numbers now. -|- براہ کرم شپمنٹ نمبر درج کریں.');
            }
            else {
                cn.disabled = true;
                bt_1.disabled = true;
                bt_2.disabled = true;
                btn_reset1.disabled = true;
                btn_reset2.disabled = true;
                alert(Message);
            }
        }
    </script>
    <%--Validate RiderCode--%>
    <script type="text/javascript">
        function ValidateSaveMaster() {

            var riderCode = document.getElementById('<%= txt_ridercode.ClientID %>');
            ValidateRider(riderCode);
            var txt_ridername = document.getElementById('<%= txt_ridername.ClientID %>');
            if (riderCode.value == '' || riderCode.value == null) {
                alert('Kindly Enter Rider Code.!');
                return false;
            }
            else if (txt_ridername.value == '' || txt_ridername.value == null) {
                alert('Kindly Wait Let System Fetch the Rider Name.!');
                ValidateRider(riderCode);
                return false;
            }
            else {
                // loader();
                return true
            }

        }


    </script>

    <%--Validate RiderCode--%>
    <script type="text/javascript">
        function RefreshID() {
            var arrivalID = document.getElementById('<%= txt_arrivalID.ClientID %>');
            var riderCode = document.getElementById('<%= txt_ridercode.ClientID %>');
            var riderName = document.getElementById('<%= txt_ridername.ClientID %>');
            var weight = document.getElementById('<%= txt_weight.ClientID %>');
            var serviceType = document.getElementById('<%= dd_servicetype.ClientID %>');
            var conType = document.getElementById('<%= dd_contype.ClientID %>');
            var cn = document.getElementById('txt_consignment');
            var consignments = document.getElementById('tbl_details');
            var currentMode = document.getElementById('<%= hd_currentMode.ClientID %>');
            var count = document.getElementById('lbl_cnCount');
            var btn_reset1 = document.getElementById('<%= btn_reset1.ClientID %>');
            var btn_reset2 = document.getElementById('<%= btn_reset2.ClientID %>');
            var btn_refresh = document.getElementById('btn_refresh');
            var btn_saveArrival = document.getElementById('<%= btn_saveArrival.ClientID %>');
            var lbl_message = document.getElementById('<%= lbl_message.ClientID %>');

            ClearTable(consignments);
            arrivalID.disabled = false;
            arrivalID.focus();
            count.innerHTML = "";
            arrivalID.value = "";
            riderCode.value = "";
            riderName.value = "";
            cn.value = "";
            cn.disabled = false;
            bt_1.disabled = false;
            bt_2.disabled = false;
            btn_reset1.disabled = false;
            btn_reset2.disabled = false;
            riderCode.disabled = true;
            riderName.disabled = true;
            btn_refresh.style.visibility = "visible";
            lbl_message.style.visibility = "hidden";
            btn_saveArrival.style.visibility = "hidden";

        }


    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
    <div>
        <div runat="server" id="loader" style="float: left; opacity: 0.7; position: absolute; text-align: center; display: none; top: 50%; width: 84% !important;">
            <div class="loader">
                <img src="../images/Loading_Movie-02.gif" />
            </div>
        </div>
        <table cellpadding="0" cellspacing="0" width='100%' class='mGrid_Table'>
            <tr>
                <td colspan="12" align="center" class="head_column">
                    <h3>Master Arrival Scan
                    </h3>
                </td>
            </tr>
        </table>

        <div class="search">
            <a href="SearchArrivalScanNew.aspx">Search Arrival Scan</a>
        </div>
        <asp:UpdatePanel ID="up_1" runat="server">
            <ContentTemplate>
                <table class="input-form" style="width: 90%">

                    <tr>
                        <td class="field">Arrival Mode
                        </td>
                        <td class="input-field" style="width: 33%;">
                            <span style="float: left; width: 80%">
                                <asp:RadioButtonList ID="rbtn_mode" runat="server" RepeatColumns="2" RepeatDirection="Horizontal"
                                    onclick="ChangeMode(this)">
                                    <asp:ListItem Value="0" Selected="True">New</asp:ListItem>
                                    <asp:ListItem Value="1">Edit</asp:ListItem>
                                </asp:RadioButtonList>
                                <asp:HiddenField ID="hd_currentMode" runat="server" Value="0" />
                            </span>
                        </td>
                        <td class="space"></td>
                        <td class="field">Arrival ID
                        </td>
                        <td class="input-field">
                            <asp:TextBox ID="txt_arrivalID" runat="server" Enabled="false" AutoPostBack="false"
                                onchange="GetArrivalDetails(this);"></asp:TextBox>

                            <input type="button" id="btn_refresh" style="visibility: hidden; background: none; border: none; color: blue; font-size: smaller; background-size: inherit" value="Click here to Change Arrival ID" onclick="RefreshID()" />
                        </td>
                    </tr>
                    <tr>
                        <td class="field">Rider Code:
                        </td>
                        <td class="input-field" style="width: 33%;">
                            <span style="float: left; width: 80%">
                                <asp:TextBox ID="txt_ridercode" runat="server"
                                    onchange="ValidateRider(this)" AutoPostBack="false" Style="width: 96%; padding: 4px;"></asp:TextBox>
                            </span>
                        </td>
                        <td class="space"></td>
                        <td class="field">Rider Name:
                        </td>
                        <td class="input-field">
                            <asp:TextBox ID="txt_ridername" runat="server" Columns="8"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="field">Total Weight:
                        </td>
                        <td class="input-field" style="width: 33%;">
                            <span style="float: left; width: 80%">
                                <asp:TextBox ID="txt_weight" runat="server" Enabled="false" Style="width: 96%; padding: 4px;"></asp:TextBox>
                                <%--ontextchanged="txt_weight_TextChanged1"--%>
                            </span>
                        </td>
                        <td class="space"></td>
                        <td class="field">Service Type:
                        </td>
                        <td class="input-field" style="width: 30%;">
                            <span style="float: left; width: 85%">
                                <asp:DropDownList ID="dd_servicetype" runat="server" CssClass="dropdown">
                                </asp:DropDownList>
                                <asp:HiddenField ID="hd_ExpressCenter" runat="server" />
                            </span>
                        </td>
                    </tr>
                    <tr>
                        <td class="field">Consignment Type:
                        </td>
                        <td class="input-field" style="width: 33%;">
                            <span style="float: left; width: 80%">
                                <asp:DropDownList ID="dd_contype" runat="server" CssClass="dropdown">
                                </asp:DropDownList>
                            </span>
                        </td>
                        <td class="space"></td>
                        <td class="field">Consignment No:
                        </td>
                        <td class="input-field">

                            <input type="text" id="txt_consignment" onchange="AddConsignment()" style="width: 230px" disabled
                                onkeypress="return isNumberKey(event);" />
                        </td>
                    </tr>
                </table>
                <br />
                <div style="text-align: center;">
                    <asp:Label ID="lbl_message" Text="Kindly Enter Arrival Information first. -|- .براہ مہربانی سب سے پہلے آمد کی معلومات درج کریں." runat="server" ForeColor="Red" />
                </div>

                <div style="text-align: center;">


                    <asp:Button ID="btn_saveArrival" runat="server" Text="Save Arrival" CssClass="button1" CausesValidation="False"
                        OnClick="btn_saveArrival_Click" OnClientClick="return ValidateSaveMaster()" />



                </div>
            </ContentTemplate>

        </asp:UpdatePanel>
        <br />
        <br />
        <div style="text-align: center;">
            <%--<asp:Button ID="Button1" runat="server" Text="Save" CssClass="button1" OnClick="Btn_Save_Click"
                            OnClientClick="loader();" CausesValidation="false" UseSubmitBehavior="false" />--%>
            <input type="button" class="button" id="bt_2" value="Save" onclick="SaveArrival()" disabled />
            &nbsp;&nbsp;&nbsp;&nbsp;
                      <%--  <input type="button" class="button" id="btn_reset2" value="Reset" onclick="ResetAll()" disabled />--%>
            <asp:Button ID="btn_reset2" runat="server" Text="Reset" CssClass="button1" OnClick="btn_reset2_Click" />
        </div>
        <asp:Label ID="error_msg" runat="server" CssClass="error_msg"></asp:Label>
        <div style="width: 100%; overflow: scroll; height: 250px;">
            <label id="lbl_cnCount" style="font-family: Calibri; font-weight: bold; font-size: medium; float: right; margin-right: 5%;">
                Consignment Count: 0
            </label>
            <table id="tbl_details" class="tblDetails">
                <tr class="headerRow">
                    <td style="width: 10%; text-align: center;"></td>
                    <td style="width: 15%;">Consignment #
                    </td>
                    <td style="width: 15%;">Service Type
                    </td>
                    <td style="width: 15%;">Consignment Type
                    </td>
                    <td style="width: 5%;">Weight
                    </td>
                    <td style="width: 5%;">Pieces
                    </td>
                    <td style="width: 8%;">RiderCode
                    </td>
                    <td style="width: 22%;">RiderName
                    </td>
                    <td style="width: 5%;">COD Amount
                    </td>
                </tr>
            </table>
        </div>
        <br />
        <div style="text-align: center;">
            <%--<asp:Button ID="Button1" runat="server" Text="Save" CssClass="button1" OnClick="Btn_Save_Click"
                            OnClientClick="loader();" CausesValidation="false" UseSubmitBehavior="false" />--%>
            <input type="button" class="button" id="bt_1" value="Save" onclick="SaveArrival()" disabled />
            &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btn_reset1" runat="server" Text="Reset" CssClass="button1" OnClick="btn_reset2_Click" />

        </div>
        <div style="display: none;">
            <asp:GridView ID="cnControls" runat="server">
                <Columns>
                    <asp:BoundField HeaderText="Prefix" DataField="Prefix" />
                    <asp:BoundField HeaderText="Length" DataField="Length" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hd_U_ID" />
    <asp:HiddenField runat="server" ID="hd_BranchCode" />
    <asp:HiddenField runat="server" ID="hd_zoneCode" />
    <asp:HiddenField runat="server" ID="hd_ExpressCenter_Session" />
    <asp:HiddenField runat="server" ID="hd_LocationID" />
    <asp:HiddenField runat="server" ID="hd_LocationName" />
</asp:Content>