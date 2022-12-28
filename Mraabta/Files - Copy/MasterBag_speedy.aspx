<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.master" CodeBehind="MasterBag_speedy.aspx.cs" Inherits="MRaabta.Files.MasterBag_speedy" %>

<%@ Register TagPrefix="Ajax1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        headerRow th {
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
    </style>
    <script src="../Js/FusionCharts.js" type="text/javascript"></script>
    <script>
        function Loader() {
            debugger;
            document.getElementById('<%=div2.ClientID %>').style.display = "block";
            this.disabled = false;
            this.value = 'Please Wait...';

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


        function Show_Hide_By_Display() {
            debugger;
            var div1 = document.getElementById('<%=div2.ClientID%>');
            var div2 = document.getElementById('<%=rd_div.ClientID%>');
            if (div1.style.display == "" || div1.style.display == "block") {
                div1.style.display = "none";
                div2.style.display = "block";
            }
            else {
                div1.style.display = "block";
                div2.style.display = "none";
            }
            return false;
        }
        function Loader1() {
            debugger;
            $('#<%= div2.ClientID %>').toggle("slow");
        }

        function CheckEmptyManifest(txt) {
            if (txt.value.trim() == "") {
                alert("Enter Valid Manifest Number");
                return false;
            }
            else {
                return true;
            }
        }
    </script>
    <script type="text/javascript">
        /////////////////////////////////////////////Weight Calculation//////////////////////////////////////////
        function CalculateTotalWeight() {
            debugger;
            var tbl_consignments = document.getElementById('tbl_consignments');
            var Tbl_Manifest = document.getElementById('Tbl_Manifest');

            var Button2 = document.getElementById('Button2');
            var Button1 = document.getElementById('Button1');

            var txt_consignmentno = document.getElementById('<%= txt_consignmentno.ClientID %>');
            var txt_manifestno = document.getElementById('<%= txt_manifestno.ClientID %>');

            var chk = false;


            var validNumber = new RegExp(/^\d+(\.\d+)?$/);

            for (var i = 1; i < tbl_consignments.rows.length; i++) {
                if (validNumber.test(tbl_consignments.rows[i].cells[5].childNodes[0].value) && (tbl_consignments.rows[i].cells[5].childNodes[0].value != "0")) {
                    tbl_consignments.rows[i].cells[5].childNodes[0].style.backgroundColor = "";
                }
                else {
                    tbl_consignments.rows[i].cells[5].childNodes[0].style.backgroundColor = "red";
                    chk = true;
                }
            }

            for (var i = 1; i < Tbl_Manifest.rows.length; i++) {
                if (validNumber.test(Tbl_Manifest.rows[i].cells[5].childNodes[0].value) && (Tbl_Manifest.rows[i].cells[5].childNodes[0].value != "0")) {
                    Tbl_Manifest.rows[i].cells[5].childNodes[0].style.backgroundColor = "";
                }
                else {
                    Tbl_Manifest.rows[i].cells[5].childNodes[0].style.backgroundColor = "red";
                    chk = true;
                }
            }

            if (chk) {
                txt_consignmentno.disabled = true;
                txt_manifestno.disabled = true;
                Button2.disabled = true;
                Button1.disabled = true;
                alert('Kindly insert proper weight');
                return;
            }
            else {
                txt_consignmentno.disabled = false;
                txt_manifestno.disabled = false;
                Button2.disabled = false;
                Button1.disabled = false;
                //validate();
            }


            var totalWeight = 0;
            var tempCnWeight = 0;
            var tempBagWeight = 0;


            for (var i = 1; i < tbl_consignments.rows.length; i++) {
                var temp = parseFloat(tbl_consignments.rows[i].cells[5].childNodes[0].value);
                if (!isNaN(temp)) {
                    tempCnWeight = tempCnWeight + temp;
                }
            }

            for (var i = 1; i < Tbl_Manifest.rows.length; i++) {
                var temp = parseFloat(Tbl_Manifest.rows[i].cells[5].childNodes[0].value);
                if (!isNaN(temp)) {
                    tempCnWeight = tempCnWeight + temp;
                }
            }
            document.getElementById('<%= txt_weight.ClientID %>').value = (tempCnWeight).toString();
        }

        /////////////////////////////////////////////Pieces Calculation//////////////////////////////////////////
        function validate() {
            var tbl_consignments = document.getElementById('tbl_consignments');
            var Tbl_Manifest = document.getElementById('Tbl_Manifest');

            var Button2 = document.getElementById('Button2');
            var Button1 = document.getElementById('Button1');

            var txt_consignmentno = document.getElementById('<%= txt_consignmentno.ClientID %>');
            var txt_manifestno = document.getElementById('<%= txt_manifestno.ClientID %>');


            var chk = false;
            var validNumber = new RegExp(/^[1-9][0-9]*$/);
            for (var i = 1; i < tbl_consignments.rows.length; i++) {
                if (validNumber.test(tbl_consignments.rows[i].cells[6].childNodes[0].value)) {
                    tbl_consignments.rows[i].cells[6].childNodes[0].style.backgroundColor = "";
                }
                else {
                    tbl_consignments.rows[i].cells[6].childNodes[0].style.backgroundColor = "red";
                    chk = true;
                }
            }

            for (var i = 1; i < Tbl_Manifest.rows.length; i++) {
                if (validNumber.test(Tbl_Manifest.rows[i].cells[6].childNodes[0].value)) {
                    Tbl_Manifest.rows[i].cells[6].childNodes[0].style.backgroundColor = "";
                }
                else {
                    Tbl_Manifest.rows[i].cells[6].childNodes[0].style.backgroundColor = "red";
                    chk = true;
                }
            }

            if (chk) {
                txt_consignmentno.disabled = true;
                txt_manifestno.disabled = true;
                Button2.disabled = true;
                Button1.disabled = true;
                alert('Kindly insert proper Piece');
                return;
            }
            else {
                txt_consignmentno.disabled = false;
                txt_manifestno.disabled = false;
                Button2.disabled = false;
                Button1.disabled = false;
                CalculateTotalWeight();

            }


        }
    </script>
    <script type="text/javascript">

        function BagNo_TextChanged() {
            var MainfestNo = document.getElementById('<%= txt_bagno.ClientID %>');
            var destination = document.getElementById('<%= dd_destination.ClientID %>');
            var origin = document.getElementById('<%= dd_origin.ClientID %>');
            var Selection_List = document.getElementById('ContentPlaceHolder1_rbtn_search');


            if (origin.options[origin.options.selectedIndex].value == "0") {
                alert('Select Origin');
                return;
            }

            if (destination.options[destination.options.selectedIndex].value == "0") {
                alert('Select Destination');
                return;
            }

            if (MainfestNo.value == '0' || MainfestNo.value == '') {
                alert('Enter BagNo');
                return;
            }
            else {
                Selection = document.getElementById('<%= hd_1.ClientID %>').value;
                PageMethods.Get_BagInformation(MainfestNo.value, Selection, onSuccess_Bag, onFailure);
            }
        }

        function AddConsignment() {
            //  DisableSave();
            var MainfestNo = document.getElementById('<%= txt_bagno.ClientID %>');
            var destination = document.getElementById('<%= dd_destination.ClientID %>');
            var origin = document.getElementById('<%= dd_origin.ClientID %>');
            var cn = document.getElementById('<%= txt_consignmentno.ClientID %>');
            var consignments = document.getElementById('tbl_consignments');
            var SealNo = document.getElementById('<%= txt_seal.ClientID %>').value; //= "";

            var Prefix = document.getElementById('<%= Hd_2.ClientID %>').value;
            var PrefixList = Prefix.split(",");

            cn.disabled = true;

            if (cn.value == '0' || cn.value == '') {
                alert('Enter CN');
                cn.disabled = false;
                cn.focus();
                return;
            }


            if (destination.options[destination.options.selectedIndex].value == "0" || destination.options[destination.options.selectedIndex].value == "") {
                alert('Select Destination');
                destination.focus();
                cn.disabled = false;
                return;

            }

            if (SealNo == "") {
                alert('Select Seal No');
                document.getElementById('<%= txt_seal.ClientID %>').focus();
                cn.disabled = false;
                return;
            }


            var isnum = /^\d+$/.test(cn.value);
            if (!isnum) {
                alert('Kindly Insert Proper Consignment Number');
                cn.disabled = false;
                cn.value = '';
                cn.focus();
                return;
            }

            // Prefix Control
            message = "";
            for (var i = 0; i < PrefixList.length; i++) {
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
                document.getElementById('<%= txt_consignmentno.ClientID %>').disabled = false;
                document.getElementById('<%= txt_consignmentno.ClientID %>').value = "";
                document.getElementById('<%= txt_consignmentno.ClientID %>').focus();
                return false;
            }
            // Now Adding Consignments
            for (var i = 1; i < consignments.rows.length; i++) {

                if (consignments.rows[i].cells[1].innerText.trim() == cn.value.trim()) {
                    alert('Consignment Already Scanned');
                    cn.disabled = false;
                    cn.value = "";
                    cn.focus();
                    return;
                }
            }



            //COD Controls start
            var OutputMessage = '';
            var CODControlCheck = true;

            $.ajax({
                async: false,
                type: 'post',
                url: 'MasterBag_speedy.aspx/CheckControls',
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

            PageMethods.ChkRunsheet(cn.value.trim(), onSuccess, onFailure);

            function onSuccess(result) {
                if (result.toString() == "N/A") {
                    var cn = document.getElementById('<%= txt_consignmentno.ClientID %>');

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
                    newTr.cells[2].innerText = "overnight" //ServiceType.options[ServiceType.options.selectedIndex].value;

                    col = newTr.insertCell(3);
                    newTr.cells[3].innerText = origin.options[origin.options.selectedIndex].text;   //;
                    newTr.cells[3].appendChild(origin_);

                    col = newTr.insertCell(4);
                    newTr.cells[4].innerText = destination.options[destination.options.selectedIndex].text;
                    newTr.cells[4].appendChild(Destination_);

                    col = newTr.insertCell(5);
                    newTr.cells[5].innerText = "";

                    col = newTr.insertCell(6);
                    newTr.cells[6].innerText = "";

                    newTr.cells[5].appendChild(txtWeight);
                    newTr.cells[5].childNodes[0].onchange = CalculateTotalWeight.bind(newTr.cells[5].childNodes[0]);


                    newTr.cells[6].appendChild(txtPieces);
                    newTr.cells[6].childNodes[0].onchange = validate.bind(newTr.cells[6].childNodes[0]);

                    newTr.cells[5].style.textAlign = "center";
                    newTr.cells[6].style.textAlign = "center";

                    //consignments.appendChild(newTr);

                    var count = document.getElementById('lbl_cnCount');
                    count.innerHTML = 'Consignment Count: ' + (consignments.rows.length - 1).toString();

                    CalculateTotalWeight();
                    cn.disabled = false;
                    cn.focus();
                    cn.value = '';
                }
                else {
                    var cn = document.getElementById('<%= txt_consignmentno.ClientID %>');
                    var message = result[0][1].toString();
                    alert('This CN ' + cn.value + ' is already Mark as ' + message);
                    cn.disabled = false;
                    cn.focus();
                    cn.value = '';
                    return;
                }
            }

            function onFailure(result) {

                var cn = document.getElementById('<%= txt_consignmentno.ClientID %>');
                cn.disabled = false;
                cn.focus();
                cn.value = '';
                alert('Kindly Scan Again!');
            }

        }



        //Add Manifest
        function AddManifest(Manifest_) {
            //  DisableSave();
            var txt_bagno = document.getElementById('<%= txt_bagno.ClientID %>');
                var destination = document.getElementById('<%= dd_destination.ClientID %>');
                var origin = document.getElementById('<%= dd_origin.ClientID %>');
                var txt_manifestno = document.getElementById('<%= txt_manifestno.ClientID %>');
                var Tbl_Manifest = document.getElementById('Tbl_Manifest');
                var SealNo = document.getElementById('<%= txt_seal.ClientID %>').value; //= "";

                if (txt_bagno.value == '0' || txt_bagno.value == '') {
                    alert('Enter BagNo');
                    txt_manifestno.disabled = false;
                    return;
                }


                if (destination.options[destination.options.selectedIndex].value == "0") {
                    alert('Select Destination');
                    destination.focus();
                    txt_manifestno.disabled = false;
                    return;

                }

                if (SealNo == "") {
                    alert('Select Seal No');
                    document.getElementById('<%= txt_seal.ClientID %>').focus();
                    txt_manifestno.disabled = false;
                    return;
                }

                for (var i = 1; i < Tbl_Manifest.rows.length; i++) {

                    if (Tbl_Manifest.rows[i].cells[1].innerText.trim() == txt_manifestno.value.trim()) {
                        alert('Manifest Already Scanned');
                        txt_manifestno.disabled = false;
                        txt_manifestno.value = "";
                        txt_manifestno.focus();
                        return;
                    }
                }
                var newTr = Tbl_Manifest.insertRow(1);  //document.createElement('tr');  //consignments.rows[0].cloneNode(true);
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
                txtWeight.value = Manifest_[0][5].toString().trim();
                txtWeight.style.width = '70%';
                txtWeight.style.textAlign = 'center';

                var txtPieces = document.createElement('input');
                txtPieces.type = 'text';
                txtPieces.className = 'textBox';
                txtPieces.value = Manifest_[0][6].toString().trim();
                txtPieces.style.width = '70%';
                txtPieces.style.textAlign = 'center';

                var Origin_ = document.createElement('input');
                Origin_.type = 'text';
                Origin_.style.display = 'none';
                Origin_.value = Manifest_[0][2].toString(); //origin.options[origin.options.selectedIndex].value;

                var Destination_ = document.createElement('input');
                Destination_.type = 'text';
                Destination_.style.display = 'none';
                Destination_.value = Manifest_[0][3].toString(); //destination.options[destination.options.selectedIndex].value;

                var destination = document.getElementById('<%= dd_destination.ClientID %>'); //.options[document.getElementById('<%= dd_destination.ClientID %>').options.selectedIndex].value =
            var destination_ = "";
            var origin_ = "";

            for (var i1 = 0; i1 < destination.options.length; i1++) {
                if (destination.options[i1].value == Manifest_[0][2].toString()) {
                    //   destination.options[i1].selected = true;
                    origin_ = destination.options[i1].text;
                    //  return;
                }
            }

            for (var i1 = 0; i1 < destination.options.length; i1++) {
                if (destination.options[i1].value == Manifest_[0][3].toString()) {
                    //   destination.options[i1].selected = true;
                    destination_ = destination.options[i1].text;
                    //  return;
                }
            }

            var col = newTr.insertCell(0);

            //cn Value         
            newTr.cells[0].appendChild(btn_remove);
            col = newTr.insertCell(1);
            newTr.cells[0].childNodes[0].onclick = RemoveConsignment.bind(newTr.cells[0].childNodes[0], newTr.cells[0].childNodes[0]);

            newTr.cells[1].innerText = Manifest_[0][0].toString().trim();

            col = newTr.insertCell(2);
            newTr.cells[2].innerText = Manifest_[0][1].toString(); //origin.options[origin.options.selectedIndex].text;   //;

            col = newTr.insertCell(3);
            newTr.cells[3].innerText = origin_//origin.options[origin.options.selectedIndex].text;   //;
            newTr.cells[3].appendChild(Origin_);

            col = newTr.insertCell(4);
            newTr.cells[4].innerText = destination_ //destination.options[destination.options.selectedIndex].text;
            newTr.cells[4].appendChild(Destination_);

            ////////////Shaheer CODE /////////////////

            col = newTr.insertCell(5);
            newTr.cells[5].innerText = "";

            col = newTr.insertCell(6);
            newTr.cells[6].innerText = "";

            newTr.cells[5].appendChild(txtWeight);
            newTr.cells[5].childNodes[0].onchange = CalculateTotalWeight.bind(newTr.cells[5].childNodes[0]);

            newTr.cells[6].appendChild(txtPieces);
            newTr.cells[6].childNodes[0].onchange = validate.bind(newTr.cells[6].childNodes[0]);

            var count = document.getElementById('lb_Mfcount');
            count.innerHTML = 'Manifest Count: ' + (Tbl_Manifest.rows.length - 1).toString();

            txt_manifestno.disabled = false;
            txt_manifestno.value = "";
            txt_manifestno.focus();

            validate();
        }


        //Edit Consignment

        function EditConsignment(result) {
            debugger;
            if (result[0][0].toString() == "DEBAGGED") {
                alert('Already Debagged');
                document.getElementById('<%= txt_bagno.ClientID %>').disabled = false;

                document.getElementById('<%= txt_bagno.ClientID %>').value = "";
                document.getElementById('<%= txt_bagno.ClientID %>').focus();
                document.getElementById('<%= txt_consignmentno.ClientID %>').value = "";
                document.getElementById('<%= txt_manifestno.ClientID %>').value = "";
                return;
            }

            if (result[0][0].toString() == "Invalid Origin") {
                alert('Cannot edit, Different origin.');

                document.getElementById('<%= txt_bagno.ClientID %>').disabled = false;

                document.getElementById('<%= txt_bagno.ClientID %>').value = "";
                document.getElementById('<%= txt_bagno.ClientID %>').focus();
                document.getElementById('<%= txt_consignmentno.ClientID %>').value = "";
                document.getElementById('<%= txt_manifestno.ClientID %>').value = "";
                return;
            }
            var cn = document.getElementById('<%= txt_consignmentno.ClientID %>');

            for (var i = 0; i < result.length; i++) {

                if (result[i].length == 5) {
                    //Baging
                    document.getElementById('<%= txt_bagno.ClientID %>').value = result[i][0];
                        var destination = document.getElementById('<%= dd_destination.ClientID %>'); //.options[document.getElementById('<%= dd_destination.ClientID %>').options.selectedIndex].value =

                        for (var i1 = 0; i1 < destination.options.length; i1++) {
                            if (destination.options[i1].value == result[i][3].toString()) {
                                destination.options[i1].selected = true;
                                //  return;
                            }
                        }

                        document.getElementById('<%= txt_weight.ClientID %>').value = result[i][4].toString();
                        document.getElementById('<%= txt_seal.ClientID %>').value = result[i][1].toString()



                }
                if (result[i].length == 10) {
                    //Manifest
                    var Manifest = document.getElementById('Tbl_Manifest');
                    var newTr = Manifest.insertRow(1);  //document.createElement('tr');  //consignments.rows[0].cloneNode(true);
                    newTr.className = 'DetailRow';

                    var btn_remove = document.createElement('input');
                    btn_remove.type = 'button';
                    btn_remove.className = 'button button1';
                    btn_remove.value = 'Remove';
                    btn_remove.style.marginTop = '2px';
                    btn_remove.style.marginBottom = '2px';
                    btn_remove.style.width = '80%';
                    btn_remove.style.display = 'none';

                    var origin_ = document.createElement('input');
                    origin_.type = 'text';
                    origin_.style.display = 'none';
                    origin_.value = result[i][2].toString(); // origin.options[origin.options.selectedIndex].value;
                    origin_.disabled = true;

                    var Destination_ = document.createElement('input');
                    Destination_.type = 'text';
                    Destination_.style.display = 'none';
                    Destination_.value = result[i][3].toString(); // destination.options[destination.options.selectedIndex].value;
                    Destination_.disabled = true;

                    ////////////Shaheer CODE /////////////////
                    var txtWeight = document.createElement('input');
                    txtWeight.type = 'text';
                    txtWeight.className = 'textBox';
                    txtWeight.value = result[i][9].toString().trim();
                    txtWeight.style.width = '70%';
                    txtWeight.style.textAlign = 'center';
                    txtWeight.disabled = true;

                    var txtPieces = document.createElement('input');
                    txtPieces.type = 'text';
                    txtPieces.className = 'textBox';
                    txtPieces.value = result[i][7].toString().trim();
                    txtPieces.style.width = '70%';
                    txtPieces.style.textAlign = 'center';
                    txtPieces.disabled = true;


                    var col = newTr.insertCell(0);

                    //cn Value         
                    newTr.cells[0].appendChild(btn_remove);
                    newTr.cells[0].childNodes[0].onclick = RemoveConsignment.bind(newTr.cells[0].childNodes[0], newTr.cells[0].childNodes[0]);

                    col = newTr.insertCell(1);
                    newTr.cells[1].innerText = result[i][0].toString(); //cn.value;

                    col = newTr.insertCell(2);
                    newTr.cells[2].innerText = result[i][1].toString(); //  origin.options[origin.options.selectedIndex].text;   //;

                    col = newTr.insertCell(3);
                    newTr.cells[3].innerText = result[i][4].toString(); //  origin.options[origin.options.selectedIndex].text;   //;
                    newTr.cells[3].appendChild(origin_);

                    col = newTr.insertCell(4);
                    newTr.cells[4].innerText = result[i][5].toString(); // destination.options[destination.options.selectedIndex].text;
                    newTr.cells[4].appendChild(Destination_);

                    col = newTr.insertCell(5);
                    newTr.cells[5].innerText = "";

                    col = newTr.insertCell(6);
                    newTr.cells[6].innerText = "";

                    newTr.cells[5].appendChild(txtWeight);
                    newTr.cells[5].childNodes[0].onchange = CalculateTotalWeight.bind(newTr.cells[5].childNodes[0]);

                    newTr.cells[6].appendChild(txtPieces);
                    newTr.cells[6].childNodes[0].onchange = validate.bind(newTr.cells[6].childNodes[0]);

                }
                if (result[i].length == 13) {
                    //consignment
                    var consignments = document.getElementById('tbl_consignments');

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
                    txtWeight.value = '0.5';
                    txtWeight.style.width = '70%';
                    txtWeight.style.textAlign = 'center';
                    txtWeight.disabled = true;

                    txtWeight.value = result[i][8].toString();

                    var txtPieces = document.createElement('input');
                    txtPieces.type = 'text';
                    txtPieces.className = 'textBox';
                    txtPieces.value = '1';
                    txtPieces.style.width = '70%';
                    txtPieces.style.textAlign = 'center';
                    txtPieces.disabled = true;

                    txtPieces.value = result[i][11].toString();


                    var origin_ = document.createElement('input');
                    origin_.type = 'text';
                    origin_.style.display = 'none';
                    origin_.value = result[i][4].toString(); // origin.options[origin.options.selectedIndex].value;
                    origin_.disabled = true;

                    var Destination_ = document.createElement('input');
                    Destination_.type = 'text';
                    Destination_.style.display = 'none';
                    Destination_.value = result[i][5].toString(); // destination.options[destination.options.selectedIndex].value;
                    Destination_.disabled = true;

                    var col = newTr.insertCell(0);

                    //cn Value         
                    newTr.cells[0].appendChild(btn_remove);
                    newTr.cells[0].childNodes[0].onclick = RemoveConsignment.bind(newTr.cells[0].childNodes[0], newTr.cells[0].childNodes[0]);

                    col = newTr.insertCell(1);
                    newTr.cells[1].innerText = result[i][0].toString(); //cn.value;

                    col = newTr.insertCell(2);
                    newTr.cells[2].innerText = result[i][1].toString(); //  origin.options[origin.options.selectedIndex].text;   //;

                    col = newTr.insertCell(3);
                    newTr.cells[3].innerText = result[i][6].toString(); //  origin.options[origin.options.selectedIndex].text;   //;
                    newTr.cells[3].appendChild(origin_);


                    col = newTr.insertCell(4);
                    newTr.cells[4].innerText = result[i][7].toString(); // destination.options[destination.options.selectedIndex].text;
                    newTr.cells[4].appendChild(Destination_);

                    //col = newTr.insertCell(4);
                    //newTr.cells[4].innerText = result[i][8].toString(); // ServiceType.options[ServiceType.options.selectedIndex].value;
                    col = newTr.insertCell(5);
                    //newTr.cells[5].innerText = result[i][8].toString(); ;

                    col = newTr.insertCell(6);
                    //  newTr.cells[6].innerText = result[i][11].toString(); ;

                    newTr.cells[5].appendChild(txtWeight);
                    newTr.cells[6].appendChild(txtPieces);

                    newTr.cells[5].style.textAlign = "center";
                    newTr.cells[6].style.textAlign = "center";


                }
            }     //consignments.appendChild(newTr);

            validate();
            consignments = document.getElementById('tbl_consignments');
            count = document.getElementById('lbl_cnCount');
            count.innerHTML = 'Consignment Count: ' + (consignments.rows.length - 1).toString();

            consignments = document.getElementById('Tbl_Manifest');

            count = document.getElementById('lb_Mfcount');
            count.innerHTML = 'Manifest Count: ' + (consignments.rows.length - 1).toString();

        }

        // For Mainfest Checking from Database

        function ManifestNo_TextChanged() {
            var MainfestNo = document.getElementById('<%= txt_manifestno.ClientID %>');
                if (MainfestNo.value == '0' || MainfestNo.value == '' || MainfestNo.length == '0') {
                    alert('Enter ManifestNo');
                    document.getElementById('<%= txt_manifestno.ClientID %>').focus();
                return;
            }
            else {

                var isnum = /^\d+$/.test(MainfestNo.value);
                if (!isnum) {
                    alert('Kindly Insert Proper Manifest Number');
                    MainfestNo.disabled = false;
                    MainfestNo.focus();
                    MainfestNo.value = '';
                    return;
                }
                MainfestNo.disabled = true;
                //   Selection = document.getElementById('<%= hd_1.ClientID %>').value;
                PageMethods.Get_ManifefstInformation(MainfestNo.value.trim(), onSuccess_Manifest, onFailure);
            }
        }



        function onSuccess_Manifest(result) {

            if (result.toString() == "N/A") {
                alert("Mainfest Not Available");
                document.getElementById('<%= txt_manifestno.ClientID %>').disabled = false;
                document.getElementById('<%= txt_manifestno.ClientID %>').value = "";
                document.getElementById('<%= txt_manifestno.ClientID %>').focus();
                return;
            }
            else {
                var mainfest = result;
                AddManifest(mainfest);
            }
        }


        function onSuccess_Bag(result) {

            if (result.toString() == "N/A") {
                document.getElementById('<%= txt_manifestno.ClientID %>').value = "";
                document.getElementById('<%= dd_destination.ClientID %>').focus();
                document.getElementById('<%= txt_consignmentno.ClientID %>').value = "";

            }
            else {

                Selection = document.getElementById('<%= hd_1.ClientID %>').value;
                if (Selection == "1") {
                    alert("Bag Already Present");
                    document.getElementById('<%= txt_bagno.ClientID %>').value = "";
                    document.getElementById('<%= txt_bagno.ClientID %>').focus();
                    document.getElementById('<%= txt_consignmentno.ClientID %>').value = "";
                    document.getElementById('<%= txt_manifestno.ClientID %>').value = "";
                }
                if (Selection == "2") {
                    //alert("Bag Already Present");
                    // For Edit Logic
                    document.getElementById('<%= txt_manifestno.ClientID %>').value = "";
                    document.getElementById('<%= txt_consignmentno.ClientID %>').value = "";
                    document.getElementById('<%= txt_bagno.ClientID %>').disabled = true;
                    document.getElementById('<%= txt_seal.ClientID %>').disabled = true;
                    EditConsignment(result);

                }
            }
        }

        function onFailure(result) {
            var manifestno = document.getElementById('<%= txt_manifestno.ClientID %>');
            manifestno.disabled = false;
            manifestno.focus();
            manifestno.value = '';
            alert('Kindly Scan Again!');
        }

        function RemoveConsignment(btn) {
            var tr = btn.parentElement.parentElement;
            var table = tr.parentElement;
            table.deleteRow(tr.rowIndex);

            var consignments = document.getElementById('tbl_consignments');
            var count = document.getElementById('lbl_cnCount');
            count.innerHTML = 'Consignment Count: ' + (consignments.rows.length - 1).toString();

            consignments = document.getElementById('Tbl_Manifest');

            count = document.getElementById('lb_Mfcount');
            count.innerHTML = 'Manifest Count: ' + (consignments.rows.length - 1).toString();
            CalculateTotalWeight();

        }

        function Editbag() {

            document.getElementById('<%= txt_bagno.ClientID %>').value = "";
            document.getElementById('<%= txt_seal.ClientID %>').value = "";
            document.getElementById('<%= dd_destination.ClientID %>').disabled = false;
            document.getElementById('<%= dd_origin.ClientID %>').disabled = false;

        }

        function Check_New() {
            document.getElementById('Button2').innerText = "Save";
            document.getElementById('Button1_save').innerText = 'Save';
            document.getElementById('<%= hd_1.ClientID %>').value = "1";
            document.getElementById('<%= txt_bagno.ClientID %>').focus();


            //  BagNo_TextChanged();
        }
        function Check_Edit() {
            document.getElementById('<%= hd_1.ClientID %>').value = "2";
            document.getElementById('Button2').innerText = "Update";
            document.getElementById('Button1_save').innerText = 'Update';
            document.getElementById('<%= txt_bagno.ClientID %>').focus();

            //BagNo_TextChanged();
        }


        function Check1() {
            if (document.getElementById('R1').checked == true) {
                document.getElementById('Div1').style.display = "";
                document.getElementById('Div2').style.display = "none";
                document.getElementById('<%= txt_bagno.ClientID %>').focus();
            }
        }
        function Check2() {
            if (document.getElementById('R2').checked == true) {
                document.getElementById('Div2').style.display = "";
                document.getElementById('Div1').style.display = "none";

            }

        }

        function sealchange() {
            document.getElementById('<%= txt_manifestno.ClientID %>').focus();
        }

    </script>

    <script type="text/javascript" src="http://code.jquery.com/jquery-1.7.1.min.js"></script>
    <script type="text/javascript">
        $('input[name=tabPick]').click(function () {
            $('#tabs').tabs('select', $(this).val());
        });

        function Reset1() {
            $("#tbl_consignments").find("tr:not(:first)").remove();
            document.getElementById('<%= txt_manifestno.ClientID %>').value = "";
            var count = document.getElementById('lbl_cnCount');
            count.innerHTML = 'Consignment Count: 0';
            CalculateTotalWeight();

        }


        function Reset2() {
            $("#tbl_consignments").find("tr:not(:first)").remove();
            document.getElementById('<%= txt_manifestno.ClientID %>').value = "";
            var count = document.getElementById('lbl_cnCount');
            count.innerHTML = 'Consignment Count: 0';
            CalculateTotalWeight();
        }

        function Reset1() {
            $("#tbl_consignments").find("tr:not(:first)").remove();
            $("#Tbl_Manifest").find("tr:not(:first)").remove();
            document.getElementById('<%= txt_bagno.ClientID %>').value = "";
            document.getElementById('<%= txt_seal.ClientID %>').value = "";


            var count = document.getElementById('lbl_cnCount');
            count.innerHTML = 'Consignment Count: 0';

            count = document.getElementById('lb_Mfcount');
            count.innerHTML = 'Manifest Count: 0';
        }



        function SaveConsignments() {
            debugger;
            var bagno = document.getElementById('<%= txt_bagno.ClientID %>');
            var Sealno = document.getElementById('<%= txt_seal.ClientID %>');
            var TotalWeight = document.getElementById('<%= txt_weight.ClientID %>');
            var bagdate = document.getElementById('<%= dd_start_date.ClientID %>');

            var destination_ = document.getElementById('<%= dd_destination.ClientID %>');
            var origin_ = document.getElementById('<%= dd_origin.ClientID %>');
            var cn = document.getElementById('<%= txt_consignmentno.ClientID %>');
            var mf = document.getElementById('<%= txt_manifestno.ClientID %>');

            var tblconsignments = document.getElementById('tbl_consignments');
            var tblmanifest = document.getElementById('Tbl_Manifest');

            Selection = document.getElementById('<%= hd_1.ClientID %>').value;

            if (bagno.value.trim() == "") {
                alert('Enter BagNo');
                return;
            }
            if (Sealno.value.trim() == "") {
                alert('Enter SealNo');
                return;
            }

            if (TotalWeight.value.trim() == "") {
                alert('Enter Total Weight');
                return;
            }



            if (origin_.options[origin_.options.selectedIndex].value == "0") {
                alert('Select Origin');
                return;
            }

            if (destination_.options[destination_.options.selectedIndex].value == "0") {
                alert('Select Destination');
                return;
            }
            var jsonOBJECT = { manifests: [], consignments: [], bag: {} };
            if (tblmanifest.rows.length > 1) {
                for (var i = 1; i < tblmanifest.rows.length; i++) {
                    var tr = tblmanifest.rows[i];

                    var Manifestnumber = tr.cells[1].innerText.trim();
                    var weight_ = tr.cells[5].childNodes[0].value;
                    var pieces_ = tr.cells[6].childNodes[0].value;
                    manifests =
                    {
                        Manifestnumber: Manifestnumber,
                        BagNumber: bagno.value,
                        weight: weight_,
                        pieces: pieces_

                    }
                    jsonOBJECT.manifests.push(manifests);

                }


            }

            if (tblconsignments.rows.length > 1) {


                debugger;
                var consignment;

                for (var i = 1; i < tblconsignments.rows.length; i++) {
                    var tr = tblconsignments.rows[i];
                    var consignmentNumber = tr.cells[1].innerText.trim();
                    var origin = tr.cells[3].childNodes[1].value;
                    var destination = tr.cells[4].childNodes[1].value;
                    var weight_ = tr.cells[5].childNodes[0].value;
                    var pieces_ = tr.cells[6].childNodes[0].value;

                    //Consignment Information
                    consignment = {
                        ConsignmentNumber: consignmentNumber,
                        BagNumber: bagno.value,
                        ServiceType: 'overnight',
                        weight: weight_,
                        pieces: pieces_

                    };
                    jsonOBJECT.consignments.push(consignment);

                }

            }


            var MasterParam = {
                BagNumber: bagno.value,
                TotalWeight: TotalWeight.value,
                Seal: Sealno.value,
                bagDate: bagdate.value,
                origin: origin_.options[origin_.options.selectedIndex].value,
                destination: destination_.options[destination_.options.selectedIndex].value,
                type: Selection
            };
            jsonOBJECT.bag = MasterParam;

            // Now Post function
            $.ajax({

                url: 'MasterBag_speedy.aspx/InsertBag',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                data: JSON.stringify(jsonOBJECT),
                success: function (result) {
                    debugger;
                    if (result.d.toString() == "OK") {
                        $("#tbl_consignments").find("tr:not(:first)").remove();
                        $("#Tbl_Manifest").find("tr:not(:first)").remove();

                        alert("bag has been generated");
                        window.open("Bag_speedy_Print.aspx?Xcode=" + bagno.value, "_blank", "");
                        document.getElementById('<%= txt_bagno.ClientID %>').value = "";
                        document.getElementById('<%= txt_seal.ClientID %>').value = "";
                        document.getElementById('<%= txt_weight.ClientID %>').value = "";

                        destination = document.getElementById('<%= dd_destination.ClientID %>'); //.options[document.getElementById('<%= dd_destination.ClientID %>').options.selectedIndex].value =
                        document.getElementById('R1').checked = true
                        for (var i1 = 0; i1 < destination.options.length; i1++) {
                            if (destination.options[i1].value == "0") {
                                destination.options[i1].selected = true;
                                //  return;
                            }
                        }

                        document.getElementById('<%= txt_bagno.ClientID %>').focus();

                        var count = document.getElementById('lbl_cnCount');
                        count.innerHTML = 'Consignment Count: 0';

                        count = document.getElementById('lb_Mfcount');
                        count.innerHTML = 'Manifest Count: 0';
                        document.getElementById('ctl00_ContentPlaceHolder1_New').checked = true;
                        document.getElementById('<%= txt_bagno.ClientID %>').disabled = false;
                        document.getElementById('<%= txt_seal.ClientID %>').disabled = false;



                        Check1();

                    }
                    else {
                        alert("Bag is not Generated :" + result.d.toString());
                    }
                }
            });



        }
    </script>
    <script type="text/javascript" src="http://code.jquery.com/jquery-1.7.1.min.js"></script>
    <body>
        <div>
            <table cellpadding="0" cellspacing="0" width='100%' class='mGrid_Table'>
                <tr>
                    <td colspan="12" align="center" class="head_column">
                        <h3>Master Bagging
                        </h3>
                    </td>
                </tr>
            </table>
            <div id="div2" runat="server" class="outer_box" style="display: none; background: #FFFFFF none repeat scroll 0 0;">
                <img src="../images/Loading_Movie-02.gif" />
            </div>
            <div class="search">
                <a href="SearchBags.aspx">Search Bags</a>
            </div>
            <table class="input-form" style="width: 95%;">
                <tr>
                    <td style="text-align: right">
                        <div>
                            <asp:RadioButton ID="New" GroupName="VehicleGroup" runat="server" onclick="Check_New();"
                                Checked="true" />New
                            <asp:RadioButton ID="Edit" GroupName="VehicleGroup" runat="server" onclick="Check_Edit();" />Edit
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="field">Bag No:
                    </td>
                    <td class="input-field">
                        <asp:TextBox ID="txt_bagno" MaxLength="12" runat="server" onchange="BagNo_TextChanged();"
                            onkeypress="return isNumberKey(event);"></asp:TextBox>
                    </td>
                    <td class="space"></td>
                    <td class="field">Orign:
                    </td>
                    <td class="input-field">
                        <asp:DropDownList ID="dd_origin" runat="server" CssClass="dropdown" Enabled="false">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="field">Date:
                    </td>
                    <td class="input-field">
                        <asp:TextBox ID="dd_start_date" runat="server" CssClass="med-field" MaxLength="10"
                            Enabled="false"></asp:TextBox>
                        <Ajax1:CalendarExtender ID="CalendarExtender3" TargetControlID="dd_start_date" runat="server"
                            Format="yyyy-MM-dd" PopupButtonID="Image1">
                        </Ajax1:CalendarExtender>
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
                    <td class="field">Total Weight:
                    </td>
                    <td class="input-field">
                        <asp:TextBox ID="txt_weight" runat="server" Columns="8" Text="0" Enabled="true" onkeypress="return isNumberKey(event);"></asp:TextBox>
                    </td>
                    <td class="space"></td>
                    <td class="field">Seal No:
                    </td>
                    <td class="input-field">
                        <asp:TextBox ID="txt_seal" runat="server" onchange="sealchange();"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <div style="position: relative; width: 100%; left: 15px; margin: 0 0 10px; text-align: center">
                <button id="Button2" class="button" type="button" onclick="SaveConsignments();">
                    Save</button>&nbsp;&nbsp;&nbsp;&nbsp;
                <button id="Button1" class="button" type="button" onclick="Reset1();">
                    Reset
                </button>
            </div>
            <asp:Label ID="error_msg" runat="server" CssClass="error_msg"></asp:Label>
            <div style="font-weight: bold; width: 10%; position: relative; right: 40px; text-align: right; float: right; top: 10px;">
                <asp:Literal ID="lbl_count" runat="server"></asp:Literal>
            </div>
            <div id="rd_div" runat="server">
                <div class="tab_radio" style="position: relative important;">
                    <span>
                        <input type="radio" id="R1" name="tabPick" value="tab1" checked="checked" onclick="Check1();" />
                        Handle Manifest
                        <input type="radio" id="R2" name="tabPick" value="tab2" onclick="Check2();" />
                        Handle Out Pieces </span>
                </div>
                <br />
                <div class="tab-frame">
                    <div class="tabPick" id="Div1" style="display: none;">
                        <table class="input-form boxbg">
                            <tr>
                                <td class="field">Manifest No:
                                </td>
                                <td class="input-field">
                                    <asp:TextBox ID="txt_manifestno" MaxLength="12" runat="server" onchange="ManifestNo_TextChanged();"
                                        onkeypress="return isNumberKey(event);"></asp:TextBox>
                                </td>
                                <td>
                                    <label id="lb_Mfcount" style="font-family: Calibri; font-weight: bold; font-size: medium; float: right; margin-right: 5%;">
                                </td>
                            </tr>
                        </table>
                        <span id="Table_1">
                            <table id="Tbl_Manifest" class="tblDetails" width="80%">
                                <tr style="background-color: Gray; font-weight: bold">
                                    <th style="width: 15px; border: 0px !important;" width="80%"></th>
                                    <th style="width: 100px;">manifestNumber
                                    </th>
                                    <th style="width: 70px;">Service Type
                                    </th>
                                    <th style="width: 70px;">Origin
                                    </th>
                                    <th style="width: 70px;">Destination
                                    </th>
                                    <th style="width: 70px;">Weight
                                    </th>
                                    <th style="width: 70px;">Pieces
                                    </th>
                                </tr>
                            </table>
                        </span>
                    </div>
                    <div class="tabPick" id="Div2" style="display: none;">
                        <table class="input-form boxbg">
                            <tr>
                                <td class="field">Consignment No:
                                </td>
                                <td class="input-field">
                                    <asp:TextBox ID="txt_consignmentno" runat="server" onkeypress="return isNumberKey(event);"
                                        onchange="AddConsignment();"></asp:TextBox>
                                </td>
                                <td>
                                    <label id="lbl_cnCount" style="font-family: Calibri; font-weight: bold; font-size: medium; float: right; margin-right: 5%;">
                                </td>
                            </tr>
                        </table>
                        <span id="Span1">
                            <table id="tbl_consignments" class="tblDetails">
                                <tr style="background-color: Gray; font-weight: bold">
                                    <th style="width: 15px; border: 0px !important;"></th>
                                    <th style="width: 100px;">ConsignmentNo
                                    </th>
                                    <th style="width: 70px;">Service Type
                                    </th>
                                    <th style="width: 70px;">Origin
                                    </th>
                                    <th style="width: 70px;">Destination
                                    </th>
                                    <th style="width: 70px;">Weight
                                    </th>
                                    <th style="width: 70px;">Pieces
                                    </th>
                                </tr>
                            </table>
                        </span>
                    </div>
                </div>
            </div>
            <br />
            <br />
            <div style="position: relative; width: 100%; left: 15px; margin: 0 0 10px; text-align: center">
                <button id="Button1_save" class="button" type="button" onclick="SaveConsignments();">
                    Save</button>&nbsp;&nbsp;&nbsp;&nbsp;
                <button id="Button3" class="button" type="button" onclick="Reset1();">
                    Reset
                </button>
            </div>
            <asp:HiddenField ID="hd_1" runat="server" />
            <asp:HiddenField ID="Hd_2" runat="server" />
        </div>
    </body>
    <script language="javascript" type="text/javascript">
        Check1();


    </script>
</asp:Content>
