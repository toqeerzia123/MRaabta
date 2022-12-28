<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.master" CodeBehind="Manage_Manifest_temp.aspx.cs" Inherits="MRaabta.Files.Manage_Manifest_temp" %>

<%@ Register TagPrefix="Ajax1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        .headerRow th {
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
    <script type="text/javascript">
        /////////////////////////////////////////////Weight Calculation//////////////////////////////////////////
        function CalculateTotalWeight() {
            debugger;
            var cns = document.getElementById('tbl_consignments');
            var Button2 = document.getElementById('Button2');
            var btn_save_ = document.getElementById('btn_save');
            var txt_consignmentno = document.getElementById('<%= txt_consignmentNo.ClientID %>');

            var chk = false;


            var validNumber = new RegExp(/^\d+(\.\d+)?$/);



            for (var i = 1; i < cns.rows.length; i++) {
                if (validNumber.test(cns.rows[i].cells[5].childNodes[0].value) && (cns.rows[i].cells[5].childNodes[0].value != "0")) {
                    cns.rows[i].cells[5].childNodes[0].style.backgroundColor = "";
                }
                else {
                    cns.rows[i].cells[5].childNodes[0].style.backgroundColor = "red";
                    chk = true;
                }
            }

            if (chk) {
                txt_consignmentno.disabled = true;
                Button2.disabled = true;
                btn_save_.disabled = true;
                alert('Kindly insert proper weight');
                return;
            }
            else {
                txt_consignmentno.disabled = false;
                Button2.disabled = false;
                btn_save_.disabled = false;
                //validate();
            }



            var totalWeight = 0;
            var tempCnWeight = 0;
            var tempBagWeight = 0;


            for (var i = 1; i < cns.rows.length; i++) {
                var temp = parseFloat(cns.rows[i].cells[5].childNodes[0].value);
                if (!isNaN(temp)) {
                    tempCnWeight = tempCnWeight + temp;
                }
            }

            document.getElementById('<%= txt_totalWeight.ClientID %>').value = (tempCnWeight + tempBagWeight).toString();
        }

        /////////////////////////////////////////////Pieces Calculation//////////////////////////////////////////
        function validate() {
            var cns = document.getElementById('tbl_consignments');
            var Button2 = document.getElementById('Button2');
            var btn_save_ = document.getElementById('btn_save');
            var txt_consignmentno = document.getElementById('<%= txt_consignmentNo.ClientID %>');

            var txt_totalPieces = document.getElementById('<%= txt_totalPieces.ClientID %>');

            var chk = false;
            var validNumber = new RegExp(/^[1-9][0-9]*$/);
            for (var i = 1; i < cns.rows.length; i++) {
                if (validNumber.test(cns.rows[i].cells[6].childNodes[0].value)) {
                    cns.rows[i].cells[6].childNodes[0].style.backgroundColor = "";
                }
                else {
                    cns.rows[i].cells[6].childNodes[0].style.backgroundColor = "red";
                    chk = true;
                }
            }

            if (chk) {
                txt_consignmentno.disabled = true;
                Button2.disabled = true;
                btn_save_.disabled = true;
                alert('Kindly insert proper Piece');
                return;
            }
            else {
                txt_consignmentno.disabled = false;
                Button2.disabled = false;
                btn_save_.disabled = false;
                CalculateTotalWeight();

            }


        }
    </script>
    <script type="text/javascript">

        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }
        var Rb_ID = "";
        function Getid(id) {
            Rb_ID = id;
        }

        function ManifestNo_TextChanged() {
            debugger;
            var MainfestNo = document.getElementById('<%= txt_manifestNo.ClientID %>');
            var destination = document.getElementById('<%= dd_destination.ClientID %>');
            var origin = document.getElementById('<%= dd_origin.ClientID %>');
            //  var Selection_List = document.getElementById('<%= rbtn_search.ClientID %>');
            var Selection_List = document.getElementById('ContentPlaceHolder1_rbtn_search');

            Selection = document.getElementById('<%= hd_1.ClientID %>').value;
            if (Selection == "2" || Selection == "3") {

                if (MainfestNo.value == '0' || MainfestNo.value == '' || MainfestNo.length == '0') {
                    alert('Enter ManifestNo');
                    document.getElementById('<%= txt_manifestNo.ClientID %>').focus();
                    return;
                }
                else {
                    Selection = document.getElementById('<%= hd_1.ClientID %>').value;
                    PageMethods.Get_ManifefstInformation(MainfestNo.value, Selection, onSuccess_Manifest, onFailure);
                }
            }
            else {
                if (origin.options[origin.options.selectedIndex].value == "0") {
                    alert('Select Origin');
                    return;
                }

                if (destination.options[destination.options.selectedIndex].value == "0") {
                    alert('Select Destination');
                    document.getElementById('<%= dd_destination.ClientID %>').focus();
                    MainfestNo.disabled = false;
                    MainfestNo.value = '';
                    return;
                }

                if (MainfestNo.value == '0' || MainfestNo.value == '' || MainfestNo.length == '0') {
                    alert('Enter ManifestNo');
                    document.getElementById('<%= txt_manifestNo.ClientID %>').focus();
                    return;
                }
                else {
                    Selection = document.getElementById('<%= hd_1.ClientID %>').value;
                    PageMethods.Get_ManifefstInformation(MainfestNo.value, Selection, onSuccess_Manifest, onFailure);
                }
            }
        }

        function AddConsignment() {
            //  DisableSave();
            var MainfestNo = document.getElementById('<%= txt_manifestNo.ClientID %>');
            var destination = document.getElementById('<%= dd_destination.ClientID %>');
            var origin = document.getElementById('<%= dd_origin.ClientID %>');
            var ServiceType = document.getElementById('<%= dd_serviceType.ClientID %>');
            var Selection_List = document.getElementById('<%= rbtn_search.ClientID %>');
            var cn = document.getElementById('<%= txt_consignmentNo.ClientID %>');
            var consignments = document.getElementById('tbl_consignments');
            cn.disabled = true;


            var Prefix = document.getElementById('<%= Hd_2.ClientID %>').value;
            var PrefixList = Prefix.split(",");

            if (ServiceType.options[ServiceType.options.selectedIndex].value == "0") {
                alert('Select Service Type');
                cn.disabled = false;
                return;
            }

            if (destination.options[destination.options.selectedIndex].value == "0") {
                alert('Select Destination');
                cn.disabled = false;
                MainfestNo.disabled = false;
                MainfestNo.value = '';
                return;
            }

            var isnum = /^\d+$/.test(cn.value);
            if (!isnum) {
                alert('Kindly Insert Proper Consignment Number');
                focusWorking(cn);
                cn.value = '';
                cn.disabled = false;
                return;
            }
            // Prefix Control
            message = "";
            for (var i = 1; i < PrefixList.length; i++) {
                var prefix_ = PrefixList[i].toString();
                var prefix_ = prefix_.split('-');
                var prefix = prefix_[0];
                var length_ = parseInt(prefix_[1]);
                if (prefix == "52190") {
                    var a = 0;
                }
                if (cn.value.substring(0, prefix.length) == prefix) {
                    if (cn.value.length != length_) {
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
                document.getElementById('<%= txt_consignmentNo.ClientID %>').value = "";
                document.getElementById('<%= txt_consignmentNo.ClientID %>').focus();
                cn.disabled = false;
                cn.value = '';
                return false;
            }
            // Now Adding Consignments
            for (var i = 1; i < consignments.rows.length; i++) {

                if (consignments.rows[i].cells[1].innerText.trim() == cn.value.trim()) {
                    alert('Consignment Already Scanned');
                    cn.value = '';
                    cn.focus();
                    cn.disabled = false;
                    return;
                }
            }

            //COD Controls start
            debugger;
            var OutputMessage = '';
            var CODControlCheck = true;

            $.ajax({
                async: false,
                type: 'post',
                url: 'Manage_Manifest_temp.aspx/ControlsCheck',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ cn: cn.value }),
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
                cn.value = '';
                cn.focus();
                cn.disabled = false;
                return;
            }
           
            //COD Controls end


            PageMethods.ChkRunsheet(cn.value, onSuccess, onFailure);

            function onSuccess(result) {

                if (result.toString() == "N/A") {

                    var newTr = consignments.insertRow(1);  //document.createElement('tr');  //consignments.rows[0].cloneNode(true);
                    newTr.className = 'DetailRow';

                    var btn_remove = document.createElement('input');
                    btn_remove.type = 'button';
                    btn_remove.className = 'button button1';
                    btn_remove.value = 'Remove';
                    btn_remove.style.marginTop = '2px';
                    btn_remove.style.marginBottom = '2px';
                    btn_remove.style.width = '80%';
                    //  btn_remove.onclick = "RemoveConsignment(this);";

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

                    var origin_ = document.createElement('input');
                    origin_.type = 'text';
                    origin_.style.display = 'none';
                    origin_.value = origin.options[origin.options.selectedIndex].value;

                    var Destination_ = document.createElement('input');
                    Destination_.type = 'text';
                    Destination_.style.display = 'none';
                    Destination_.value = destination.options[destination.options.selectedIndex].value;


                    var col = newTr.insertCell(0);

                    //cn Value         
                    newTr.cells[0].appendChild(btn_remove);
                    col = newTr.insertCell(1);
                    newTr.cells[0].childNodes[0].onclick = RemoveConsignment.bind(newTr.cells[0].childNodes[0], newTr.cells[0].childNodes[0]);

                    newTr.cells[1].innerText = cn.value.trim();

                    col = newTr.insertCell(2);
                    newTr.cells[2].innerText = origin.options[origin.options.selectedIndex].text;   //;
                    newTr.cells[2].appendChild(origin_);


                    col = newTr.insertCell(3);
                    newTr.cells[3].innerText = destination.options[destination.options.selectedIndex].text;
                    newTr.cells[3].appendChild(Destination_);

                    col = newTr.insertCell(4);
                    newTr.cells[4].innerText = ServiceType.options[ServiceType.options.selectedIndex].value;



                    col = newTr.insertCell(5);
                    newTr.cells[5].innerText = "";
                    newTr.cells[5].appendChild(txtWeight);
                    newTr.cells[5].childNodes[0].onchange = CalculateTotalWeight.bind(newTr.cells[5].childNodes[0]);
                    newTr.cells[5].childNodes[0].maxLength = "5";

                    col = newTr.insertCell(6);
                    newTr.cells[6].innerText = "";
                    newTr.cells[6].appendChild(txtPieces);
                    newTr.cells[6].childNodes[0].onchange = validate.bind(newTr.cells[6].childNodes[0]);
                    newTr.cells[6].childNodes[0].maxLength = "4";

                    newTr.cells[5].style.textAlign = "center";
                    newTr.cells[6].style.textAlign = "center";

                    //consignments.appendChild(newTr);
                    cn.value = '';
                    cn.disabled = false;
                    cn.focus();
                    var count = document.getElementById('lbl_cnCount');
                    count.innerHTML = 'Consignment Count: ' + (consignments.rows.length - 1).toString();
                    document.getElementById('<%= txt_totalPieces.ClientID %>').value = (consignments.rows.length - 1).toString();
                    validate();

                }
                else {
                    var message = result[0][1].toString();
                    alert('This CN ' + cn.value + ' is already Mark as ' + message);
                    cn.disabled = false;
                    cn.value = '';
                    cn.focus();
                    return;
                }
            }

            function onFailure(result) {
                cn.disabled = false;
            }

        }

        function viewConsignment(result) {
            var consignments = document.getElementById('tbl_consignments');

            for (var i = 0; i < result.length; i++) {
                if (result[i].length == 5) {
                    document.getElementById('<%= txt_manifestNo.ClientID %>').value = result[i][0].toString();


                    var destination = document.getElementById('<%= dd_destination.ClientID %>'); //.options[document.getElementById('<%= dd_destination.ClientID %>').options.selectedIndex].value =

                    for (var i1 = 0; i1 < destination.options.length; i1++) {
                        if (destination.options[i1].value == result[i][3].toString()) {
                            destination.options[i1].selected = true;
                            //  return;
                        }
                    }
                    //document.getElementById('<%= dd_serviceType.ClientID %>').options[document.getElementById('<%= dd_serviceType.ClientID %>').options.selectedIndex].value = result[i][1].toString();
                    var dd_serviceType = document.getElementById('<%= dd_serviceType.ClientID %>'); //.options[document.getElementById('<%= dd_destination.ClientID %>').options.selectedIndex].value =

                    for (var i1 = 0; i1 < dd_serviceType.options.length; i1++) {
                        if (dd_serviceType.options[i1].text == result[i][1].toString()) {
                            dd_serviceType.options[i1].selected = true;
                            //return;
                        }
                    }


                    document.getElementById('<%= txt_date.ClientID %>').disabled = false;
                    document.getElementById('<%= txt_date.ClientID %>').value = result[i][4].toString();
                    document.getElementById('<%= txt_date.ClientID %>').disabled = true;

                }
                else {

                    var newTr = consignments.insertRow(1);  //document.createElement('tr');  //consignments.rows[0].cloneNode(true);
                    newTr.className = 'DetailRow';

                    var txtWeight = document.createElement('input');
                    txtWeight.type = 'text';
                    txtWeight.className = 'textBox';
                    txtWeight.value = result[i][10].toString();
                    txtWeight.style.width = '70%';
                    txtWeight.style.textAlign = 'center';
                    txtWeight.disabled = true;

                    var txtPieces = document.createElement('input');
                    txtPieces.type = 'text';
                    txtPieces.className = 'textBox';
                    txtPieces.value = result[i][11].toString();
                    txtPieces.style.width = '70%';
                    txtPieces.style.textAlign = 'center';
                    txtPieces.disabled = true;


                    var origin_ = document.createElement('input');
                    origin_.type = 'text';
                    origin_.style.display = 'none';
                    origin_.value = result[i][4].toString(); // origin.options[origin.options.selectedIndex].value;

                    var Destination_ = document.createElement('input');
                    Destination_.type = 'text';
                    Destination_.style.display = 'none';
                    Destination_.value = result[i][6].toString(); // destination.options[destination.options.selectedIndex].value;


                    var col = newTr.insertCell(0);

                    //cn Value         
                    col = newTr.insertCell(1);
                    newTr.cells[1].innerText = result[i][0].toString(); //cn.value;

                    col = newTr.insertCell(2);
                    newTr.cells[2].innerText = result[i][5].toString(); //  origin.options[origin.options.selectedIndex].text;   //;
                    newTr.cells[2].appendChild(origin_);


                    col = newTr.insertCell(3);
                    newTr.cells[3].innerText = result[i][7].toString(); // destination.options[destination.options.selectedIndex].text;
                    newTr.cells[3].appendChild(Destination_);

                    col = newTr.insertCell(4);
                    newTr.cells[4].innerText = result[i][9].toString(); // ServiceType.options[ServiceType.options.selectedIndex].value;
                    col = newTr.insertCell(5);
                    newTr.cells[5].innerText = "";

                    col = newTr.insertCell(6);
                    newTr.cells[6].innerText = "";

                    newTr.cells[5].appendChild(txtWeight);
                    newTr.cells[6].appendChild(txtPieces);

                    newTr.cells[5].style.textAlign = "center";
                    newTr.cells[6].style.textAlign = "center";
                }
            }
            //consignments.appendChild(newTr);
            var count = document.getElementById('lbl_cnCount');
            count.innerHTML = 'Consignment Count: ' + (consignments.rows.length - 1).toString();
            document.getElementById('<%= txt_totalPieces.ClientID %>').value = (consignments.rows.length - 1).toString();
            validate();
        }

        //Edit Consignment

        function EditConsignment(result) {
            var cn = document.getElementById('<%= txt_consignmentNo.ClientID %>');

            var consignments = document.getElementById('tbl_consignments');
            for (var i = 0; i < result.length; i++) {
                if (result[i].length == 5) {
                    document.getElementById('<%= txt_manifestNo.ClientID %>').value = result[i][0].toString();

                    var destination = document.getElementById('<%= dd_destination.ClientID %>'); //.options[document.getElementById('<%= dd_destination.ClientID %>').options.selectedIndex].value =

                    for (var i1 = 0; i1 < destination.options.length; i1++) {
                        if (destination.options[i1].value == result[i][3].toString()) {
                            destination.options[i1].selected = true;
                            //  return;
                        }
                    }
                    //document.getElementById('<%= dd_serviceType.ClientID %>').options[document.getElementById('<%= dd_serviceType.ClientID %>').options.selectedIndex].value = result[i][1].toString();
                    var dd_serviceType = document.getElementById('<%= dd_serviceType.ClientID %>'); //.options[document.getElementById('<%= dd_destination.ClientID %>').options.selectedIndex].value =

                    for (var i1 = 0; i1 < dd_serviceType.options.length; i1++) {
                        if (dd_serviceType.options[i1].text == result[i][1].toString()) {
                            dd_serviceType.options[i1].selected = true;
                            //    return;
                        }
                    }

                    document.getElementById('<%= txt_date.ClientID %>').disabled = false;
                    document.getElementById('<%= txt_date.ClientID %>').value = result[i][4].toString();
                    document.getElementById('<%= txt_date.ClientID %>').disabled = true;
                }
                else {
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
                    //     btn_remove.class = "delete-row";
                    // btn_remove.onclick. = "RemoveConsignment(this);";



                    var txtWeight = document.createElement('input');
                    txtWeight.type = 'text';
                    txtWeight.className = 'textBox';
                    txtWeight.value = result[i][10].toString();
                    txtWeight.style.width = '70%';
                    txtWeight.style.textAlign = 'center';
                    txtWeight.disabled = true;


                    var txtPieces = document.createElement('input');
                    txtPieces.type = 'text';
                    txtPieces.className = 'textBox';
                    txtPieces.value = result[i][11].toString();
                    txtPieces.style.width = '70%';
                    txtPieces.style.textAlign = 'center';
                    txtWeight.disabled = true;


                    var origin_ = document.createElement('input');
                    origin_.type = 'text';
                    origin_.style.display = 'none';
                    origin_.value = result[i][4].toString(); // origin.options[origin.options.selectedIndex].value;

                    var Destination_ = document.createElement('input');
                    Destination_.type = 'text';
                    Destination_.style.display = 'none';
                    Destination_.value = result[i][6].toString(); // destination.options[destination.options.selectedIndex].value;


                    var col = newTr.insertCell(0);

                    //cn Value         
                    newTr.cells[0].appendChild(btn_remove);
                    col = newTr.insertCell(1);
                    newTr.cells[1].innerText = result[i][0].toString(); //cn.value;
                    newTr.cells[0].childNodes[0].onclick = RemoveConsignment.bind(newTr.cells[0].childNodes[0], newTr.cells[0].childNodes[0]);

                    col = newTr.insertCell(2);
                    newTr.cells[2].innerText = result[i][5].toString(); //  origin.options[origin.options.selectedIndex].text;   //;
                    newTr.cells[2].appendChild(origin_);

                    col = newTr.insertCell(3);
                    newTr.cells[3].innerText = result[i][7].toString(); // destination.options[destination.options.selectedIndex].text;
                    newTr.cells[3].appendChild(Destination_);

                    col = newTr.insertCell(4);
                    newTr.cells[4].innerText = result[i][9].toString(); // ServiceType.options[ServiceType.options.selectedIndex].value;
                    col = newTr.insertCell(5);
                    newTr.cells[5].innerText = "";

                    col = newTr.insertCell(6);
                    newTr.cells[6].innerText = "";

                    newTr.cells[5].appendChild(txtWeight);
                    newTr.cells[5].childNodes[0].onchange = CalculateTotalWeight.bind(newTr.cells[5].childNodes[0]);
                    newTr.cells[5].childNodes[0].maxLength = "5";

                    col = newTr.insertCell(6);
                    newTr.cells[6].innerText = "";
                    newTr.cells[6].appendChild(txtPieces);
                    newTr.cells[6].childNodes[0].onchange = validate.bind(newTr.cells[6].childNodes[0]);
                    newTr.cells[6].childNodes[0].maxLength = "4";


                    newTr.cells[5].style.textAlign = "center";
                    newTr.cells[6].style.textAlign = "center";
                }
            }
            //consignments.appendChild(newTr);
            var count = document.getElementById('lbl_cnCount');
            count.innerHTML = 'Consignment Count: ' + (consignments.rows.length - 1).toString();
            document.getElementById('<%= txt_totalPieces.ClientID %>').value = (consignments.rows.length - 1).toString();
            validate();


            cn.value = "";
            cn.focus()
        }

        function New_check() {
            document.getElementById('<%= txt_manifestNo.ClientID %>').value = "";
            document.getElementById('<%= txt_manifestNo.ClientID %>').focus();
            document.getElementById('<%= dd_destination.ClientID %>').options[document.getElementById('<%= dd_destination.ClientID %>').options.selectedIndex].value = "0";
            document.getElementById('<%= dd_serviceType.ClientID %>').options[document.getElementById('<%= dd_serviceType.ClientID %>').options.selectedIndex].value = "0";

        }

        function Destination_check() {
            document.getElementById('<%= dd_serviceType.ClientID %>').focus();
        }


        function onSuccess_Manifest(result) {
            debugger;
            document.getElementById('<%= txt_manifestNo.ClientID %>').disabled = true;
            if (result.toString() == "N/A") {
                document.getElementById('<%= txt_consignmentNo.ClientID %>').disabled = false;
                document.getElementById('<%= txt_consignmentNo.ClientID %>').focus();

            }
            else {
                Selection = document.getElementById('<%= hd_1.ClientID %>').value;
                if (Selection == "1") {
                    alert("Manifest Already Present");
                    document.getElementById('<%= txt_manifestNo.ClientID %>').value = "";
                    document.getElementById('<%= txt_manifestNo.ClientID %>').focus();
                    document.getElementById('<%= txt_consignmentNo.ClientID %>').disabled = true;
                    document.getElementById('Button2').innerText = "Save";
                    document.getElementById('btn_save').innerText = 'Save';

                    document.getElementById('<%= txt_manifestNo.ClientID %>').disabled = false;

                }
                if (Selection == "2") {
                    //alert("Manifest Already Present");
                    // For Viewing Logic
                    viewConsignment(result);
                    document.getElementById('Button2').disabled = true;
                    document.getElementById('btn_save').disabled = true;
                    document.getElementById('<%= txt_consignmentNo.ClientID %>').disabled = true;
                    document.getElementById('<%= btn_print.ClientID %>').style.display = "";

                }
                if (Selection == "3") {
                    if (result.length != 0) {
                        if (result[0][0] == "Invalid Origin") {
                            document.getElementById('<%= txt_manifestNo.ClientID %>').disabled = false;
                            document.getElementById('<%= txt_manifestNo.ClientID %>').value = "";

                            alert('Cannot edit, Different origin.');
                        }
                        // alert("Manifest Already Present");
                        // For Edit Logic
                        EditConsignment(result);
                        document.getElementById('<%= txt_consignmentNo.ClientID %>').disabled = false;
                        document.getElementById('Button2').innerText = "Update";
                        document.getElementById('btn_save').innerText = 'Update';
                        document.getElementById('<%= btn_print.ClientID %>').style.display = "none";
                    } else {
                        document.getElementById('<%= txt_manifestNo.ClientID %>').disabled = false;
                        document.getElementById('<%= txt_manifestNo.ClientID %>').value = "";

                        alert('Already demanifested. Invalid manifest number!');
                    }

                }
            }
        }

        function onFailure(result) {
            alert("Failed!");
        }

        function RemoveConsignment(btn) {
            var tr = btn.parentElement.parentElement;
            var table = tr.parentElement;
            table.deleteRow(tr.rowIndex);
            var txt_totalPieces = document.getElementById('<%= txt_totalPieces.ClientID %>');
            var pieces = parseInt(txt_totalPieces.value);
            txt_totalPieces.value = (pieces - 1).toString();
            var consignments = document.getElementById('tbl_consignments');
            var count = document.getElementById('lbl_cnCount');
            count.innerHTML = 'Consignment Count: ' + (consignments.rows.length - 1).toString();
            validate();
        }

    </script>
    <script type="text/javascript" src="http://code.jquery.com/jquery-1.7.1.min.js"></script>
    <script type="text/javascript">

        // Button
        // Find and remove selected table rows
        $(document).ready(function () {
            $("#tbl_consignments").on('click', '.delete-row', function () {
                $(this).closest('tr').remove();
            });
            validate();

        });
        // Save consignmnet
        function Reset1() {
            $("#tbl_consignments").find("tr:not(:first)").remove();
            document.getElementById('<%= txt_manifestNo.ClientID %>').disabled = false;
            document.getElementById('<%= txt_manifestNo.ClientID %>').value = "";
            document.getElementById('<%= txt_manifestNo.ClientID %>').focus();
            document.getElementById('<%= txt_totalPieces.ClientID %>').value = '0';
            var count = document.getElementById('lbl_cnCount');
            count.innerHTML = 'Consignment Count: 0';

            validate();
        }


        function Reset2() {
            $("#tbl_consignments").find("tr:not(:first)").remove();
            document.getElementById('<%= txt_manifestNo.ClientID %>').disabled = false;
            document.getElementById('<%= txt_manifestNo.ClientID %>').value = "";
            document.getElementById('<%= txt_totalPieces.ClientID %>').value = '0';
            var count = document.getElementById('lbl_cnCount');
            count.innerHTML = 'Consignment Count: 0';
            validate();
        }

        function SaveConsignments() {

            var MainfestNo = document.getElementById('<%= txt_manifestNo.ClientID %>');
            var destination_ = document.getElementById('<%= dd_destination.ClientID %>');
            var origin_ = document.getElementById('<%= dd_origin.ClientID %>');
            var ServiceType = document.getElementById('<%= dd_serviceType.ClientID %>');
            var cn = document.getElementById('<%= txt_consignmentNo.ClientID %>');
            var txt_totalPieces = document.getElementById('<%= txt_totalPieces.ClientID %>');
            var txt_totalWeight = document.getElementById('<%= txt_totalWeight.ClientID %>');
            var tblconsignments = document.getElementById('tbl_consignments');
            Selection = document.getElementById('<%= hd_1.ClientID %>').value;

            if (MainfestNo.value.trim() == "") {
                alert('Enter ManifestNumber');
                return;
            }

            if (ServiceType.options[ServiceType.options.selectedIndex].value == "0") {
                alert('Select Service Type');
                return;
            }

            if (origin_.options[origin_.options.selectedIndex].value == "0") {
                alert('Select Origin');
                return;
            }

            if (destination_.options[destination_.options.selectedIndex].value == "0") {
                alert('Select Destination');
                MainfestNo.disabled = false;
                MainfestNo.value = '';
                return;
            }

            if (tblconsignments.rows.length > 1) {
                var jsonOBJECT = { consignments: [], manifest: {} };
                var consignment;

                for (var i = 1; i < tblconsignments.rows.length; i++) {
                    var tr = tblconsignments.rows[i];
                    var consignmentNumber = tr.cells[1].innerText;
                    var origin = tr.cells[2].childNodes[1].value;
                    var destination = tr.cells[3].childNodes[1].value;
                    var weight_ = tr.cells[5].childNodes[0].value;
                    var pieces_ = tr.cells[6].childNodes[0].value;

                    //Consignment Information
                    consignment = {
                        ConsignmentNumber: consignmentNumber,
                        ManifestNumber: MainfestNo.value,
                        StatusCode: '06',
                        weight: weight_,
                        pieces: pieces_,


                    };
                    jsonOBJECT.consignments.push(consignment);

                }

                //Manifest Information
                var MasterParam = {
                    ManifestNumber: MainfestNo.value,
                    origin: origin_.options[origin_.options.selectedIndex].value,
                    destination: destination_.options[destination_.options.selectedIndex].value,
                    ServiceType: ServiceType.options[ServiceType.options.selectedIndex].value,
                    type: Selection,
                    Weight: txt_totalWeight.value,
                    Pieces: txt_totalPieces.value
                };
                jsonOBJECT.manifest = MasterParam;

                $.ajax({

                    url: 'Manage_Manifest_temp.aspx/InsertManifest',
                    type: 'POST',
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    data: JSON.stringify(jsonOBJECT),
                    success: function (result) {
                        debugger;
                        if (result.d.toString() == "OK") {
                            $("#tbl_consignments").find("tr:not(:first)").remove();
                            alert("Manifest has been generated");

                            window.open("Manifest_Print_temp.aspx?Xcode=" + MainfestNo.value, "_blank", "");
                            document.getElementById('<%= txt_manifestNo.ClientID %>').disabled = false;
                            document.getElementById('<%= txt_manifestNo.ClientID %>').value = "";
                            document.getElementById('<%= txt_manifestNo.ClientID %>').focus();
                            document.getElementById('<%= txt_totalWeight.ClientID %>').value = "";
                            document.getElementById('<%= txt_totalPieces.ClientID %>').value = "";

                            destination = document.getElementById('<%= dd_destination.ClientID %>'); //.options[document.getElementById('<%= dd_destination.ClientID %>').options.selectedIndex].value =

                            for (var i1 = 0; i1 < destination.options.length; i1++) {
                                if (destination.options[i1].value == "0") {
                                    destination.options[i1].selected = true;
                                    //  return;
                                }
                            }
                            //document.getElementById('<%= dd_serviceType.ClientID %>').options[document.getElementById('<%= dd_serviceType.ClientID %>').options.selectedIndex].value = result[i][1].toString();
                            dd_serviceType = document.getElementById('<%= dd_serviceType.ClientID %>'); //.options[document.getElementById('<%= dd_destination.ClientID %>').options.selectedIndex].value =

                            for (var i1 = 0; i1 < dd_serviceType.options.length; i1++) {
                                if (dd_serviceType.options[i1].text == "0") {
                                    dd_serviceType.options[i1].selected = true;
                                    //    return;
                                }
                            }


                            var count = document.getElementById('lbl_cnCount');
                            count.innerHTML = 'Consignment Count: 0';

                            var List = document.getElementById('ContentPlaceHolder1_rbtn_search_0'); //.options[document.getElementById('<%= dd_destination.ClientID %>').options.selectedIndex].value =
                            var List2 = document.getElementById('ContentPlaceHolder1_rbtn_search_1'); //.options[document.getElementById('<%= dd_destination.ClientID %>').options.selectedIndex].value =
                            var List3 = document.getElementById('ContentPlaceHolder1_rbtn_search_2'); //.options[document.getElementById('<%= dd_destination.ClientID %>').options.selectedIndex].value =

                            List.checked = true;
                            List2.checked = false;
                            List3.checked = false;


                            //                           for (var i1 = 0; i1 < List.options.length; i1++) {
                            //                                if (List.options[i1].text == "1") {
                            //                                    List.options[i1].selected = "1";
                            //                                }
                            //                           }
                        }
                        else {
                            alert("Manifest is not Generated :" + result.d.toString());
                        }
                    }
                });

            }

        }
        function ChangeDestination(dd) {
            var a = dd;
            var manifestNumber = document.getElementById('<%= txt_manifestNo.ClientID %>');
            manifestNumber.focus();
        }

        function destKeyPress(evt) {
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode == 9) {
                var manifestNumber = document.getElementById('<%= txt_manifestNo.ClientID %>');
                focusWorking(manifestNumber);
            }
            return true;
        }

        function focusWorking(cnt) {
            var id = '#' + cnt.id.toString();
            $(document).ready(function () {
                setTimeout(function () { $(id).focus(); }, 1);
            });
        }
    </script>
    <asp:Label ID="ErrorID" runat="server" ForeColor="Red" Font-Bold="true" Font-Size="Medium"></asp:Label>
    <table style="font-family: Calibri; font-size: medium; padding-bottom: 0px !important; padding-top: 0px !important; width: 97% !important;"
        class="input-form">
        <tr style="padding: 0px 0px 0px 0px !important;">
            <td colspan="9" style="padding-bottom: 1px !important; padding-top: 1px !important;">
                <h4 style="font-family: Calibri; margin: 0px !important;">Manifest Info.</h4>
            </td>
        </tr>
        <tr>
            <td class="field" style="width: 10% !important;">Origin
            </td>
            <td class="input-field" style="width: 15% !important;">
                <asp:DropDownList ID="dd_origin" runat="server" AppendDataBoundItems="true" CssClass="dropdown">
                </asp:DropDownList>
            </td>
            <td class="space"></td>
            <td class="field" style="width: 10% !important;">Destination
            </td>
            <td class="input-field" style="width: 15% !important;">
                <asp:DropDownList ID="dd_destination" runat="server" AppendDataBoundItems="true"
                    CssClass="dropdown" onkeypress="destKeyPress(event)">
                    <asp:ListItem Value="0">Select Destination</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td class="space"></td>
            <td class="field" style="width: 10% !important;"></td>
            <td class="input-field" style="width: 20% !important;">
                <asp:UpdatePanel ID="up" runat="server">
                    <ContentTemplate>
                        <asp:RadioButtonList ID="rbtn_search" runat="server" RepeatDirection="Horizontal"
                            RepeatColumns="3" AutoPostBack="true" OnSelectedIndexChanged="rbtn_search_SelectedIndexChanged1">
                            <asp:ListItem Value="1" Selected="True" onclick="New_check();">New</asp:ListItem>
                            <asp:ListItem Value="2" onclick="New_check();">View</asp:ListItem>
                            <asp:ListItem Value="3" onclick="New_check();">Edit</asp:ListItem>
                        </asp:RadioButtonList>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td class="field" style="width: 10% !important; text-align: left; vertical-align: top;">Service Type
            </td>
            <td class="input-field" style="width: 15% !important; text-align: left; vertical-align: top;">
                <asp:DropDownList ID="dd_serviceType" runat="server" AppendDataBoundItems="true"
                    CssClass="dropdown" onchange="Destination_check();">
                    <asp:ListItem Value="0">Select Service Type</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td class="space"></td>
            <td class="field" style="width: 10% !important;">Manifest No.
            </td>
            <td class="input-field" style="width: 15% !important;">
                <asp:TextBox ID="txt_manifestNo" runat="server" MaxLength="12" onchange="ManifestNo_TextChanged();"
                    onkeypress="return isNumberKey(event);"></asp:TextBox>
                <asp:RegularExpressionValidator ID="REV_MANIFEST" runat="server" ControlToValidate="txt_manifestNo"
                    ErrorMessage="Numbers Allowed Only" Font-Size="Small" ForeColor="Red" ValidationExpression="^\d+$"></asp:RegularExpressionValidator>
                <asp:RequiredFieldValidator ID="RequiredFieldValidatorAccount" runat="server" ErrorMessage="*Required Field"
                    ControlToValidate="txt_manifestNo" ForeColor="Red" Font-Size="Small"></asp:RequiredFieldValidator>

            </td>
            <td class="space"></td>
            <td class="field" style="width: 10% !important;">Date
            </td>
            <td class="input-field" style="width: 20% !important;">
                <asp:TextBox ID="txt_date" runat="server"></asp:TextBox>
                <Ajax1:CalendarExtender ID="calendarExtender1" runat="server" TargetControlID="txt_date"
                    Format="dd/MM/yyyy"></Ajax1:CalendarExtender>
            </td>
        </tr>
        <tr>
            <td class="field" style="width: 10% !important; text-align: left; vertical-align: top;">Total Weight
            </td>
            <td class="input-field" style="width: 15% !important; text-align: left; vertical-align: top;">
                <asp:TextBox runat="server" ID="txt_totalWeight" Enabled="false" />
            </td>
            <td class="space"></td>
            <td class="field" style="width: 10% !important;">Total Pieces
            </td>
            <td class="input-field" style="width: 15% !important;">
                <asp:TextBox runat="server" ID="txt_totalPieces" Enabled="false" />

            </td>
            <td class="space"></td>
        </tr>
    </table>
    <contenttemplate>
            <table style="font-family: Calibri; font-size: medium; padding-bottom: 0px !important;
                padding-top: 0px !important; width: 97% !important;" class="input-form">
                <tr style="padding: 0px 0px 0px 0px !important;">
                    <td colspan="9" style="padding-bottom: 1px !important; padding-top: 1px !important;">
                        <h4 style="font-family: Calibri; margin: 0px !important;">
                            Consignment Info.</h4>
                    </td>
                </tr>
                <tr>
                    <td class="field" style="width: 10% !important;">
                        CN Number.
                    </td>
                    <td class="input-field" style="width: 15% !important;">
                        <asp:TextBox ID="txt_consignmentNo" runat="server" onchange="AddConsignment();" onkeypress="return isNumberKey(event);"></asp:TextBox>
                    </td>
                    <td class="space">
                 
                         <label id="lbl_cnCount" style="font-family: Calibri; font-weight: bold; font-size: medium;
                            float: right; margin-right: 5%;">
                    </td>
                    </tr>
                    <tr>
                        <td colspan="8">
                        </td>
                    </tr>
            </table>
            <div style="width: 100%; text-align: center">
       
                      <button id="Button1" class="button" type="button" onclick ="Reset1();">Reset
            </button>
                &nbsp;
                  <button id="Button2" class="button" type="button" onclick="SaveConsignments();" >
            Save</button>&nbsp;&nbsp;&nbsp;&nbsp;
                &nbsp;
                <asp:Button ID="Button3" runat="server" Text="Print Report" CssClass="button" Visible="false"
                    UseSubmitBehavior="false" OnClick="btn_print_Click" />
                &nbsp;
                <asp:Button ID="Button4" runat="server" Text="Cancel" CssClass="button" OnClick="btn_cancel_Click"
                    UseSubmitBehavior="false" />
            </div>
            <div style="width: 100%; height: 250px; overflow: scroll; text-align: center;">
                <span id="Table_1" class="tbl-large">
                    <asp:Label ID="err_msg" runat="server" CssClass="error_msg"></asp:Label>
                    <div class="bulkCnHeaderTbl bulkCnHeaderTbl1" style="height: 300px; overflow: scroll;">
                        <table id="tbl_consignments" class="tblDetails">
                            <tr>
                                <th style="width: 10%; border: 0px !important;">
                                </th>
                                <th style="width: 15%;">
                                    ConsignmentNo
                                </th>
                                <th style="width: 15%;">
                                    Origin
                                </th>
                                <th style="width: 15%;">
                                    Destination
                                </th>
                                <th style="width: 15%;">
                                    Service Type
                                </th>
                                <th style="width: 15%;">
                                    Weight
                                </th>
                                <th style="width: 15%;">
                                    Pieces
                                </th>
                                
                            </tr>
                        </table>
                    </div>
                </span>
            </div>
        </contenttemplate>
    <%--<div style="width: 100%; height: 250px; overflow: scroll; text-align: center;">
        <span id="Table_1" class="tbl-large">
            <asp:Label ID="err_msg" runat="server" CssClass="error_msg"></asp:Label>
            <asp:GridView ID="gv_branches" runat="server" AutoGenerateColumns="false" CssClass="mGrid"
                AlternatingRowStyle-CssClass="alt" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px"
                EmptyDataText="No Data Available">
                <Columns>
                    <asp:BoundField HeaderText="Branch Name" DataField="Name" />
                    <asp:BoundField HeaderText="Short Name" DataField="sname" />
                    <asp:BoundField HeaderText="Description" DataField="Description" />
                    <asp:BoundField HeaderText="Email" DataField="Email" />
                    <asp:BoundField HeaderText="Phone No." DataField="PhoneNo" />
                    <asp:BoundField HeaderText="Fax No." DataField="FaxNO" />
                </Columns>
            </asp:GridView>
        </span>
        <asp:Button ID="btn_applyDefault" runat="server" Text="Apply Default Tariff" CssClass="button"
            Visible="false" />
    </div>--%>
    <div style="width: 100%; text-align: center">
        <%--   <asp:Button ID="btn_reset" runat="server" Text="Reset" CssClass="button" OnClick="btn_reset_Click"
            UseSubmitBehavior="false" />--%>
        <button id="btn_reset" class="button" type="button" onclick="Reset1();">
            Reset
        </button>
        &nbsp;
        <button id="btn_save" class="button" type="button" onclick="SaveConsignments();">
            Save</button>&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="btn_print" runat="server" Text="Print Report" CssClass="button" Visible="false"
            UseSubmitBehavior="false" OnClick="btn_print_Click" />
        &nbsp;
        <asp:Button ID="btn_cancel" runat="server" Text="Cancel" CssClass="button" OnClick="btn_cancel_Click"
            UseSubmitBehavior="false" />
    </div>
    <asp:HiddenField ID="hd_1" runat="server" />
    <asp:HiddenField ID="Hd_2" runat="server" />
</asp:Content>
