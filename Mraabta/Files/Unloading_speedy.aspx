<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="Unloading_speedy.aspx.cs" Inherits="MRaabta.Files.Unloading_speedy" %>

<%@ Register TagPrefix="Ajax1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
    
    <script type="text/javascript" src="../Scripts/jquery-3.5.1.min.js"></script>
    <script type="text/javascript">
        function TableStyleCN(tr) {
            var table = tr.parentElement;
            var receivedCN = document.getElementById('receivedCN');
            var excessCN = document.getElementById('excessCN');
            var shortCN = document.getElementById('shortCN');

            var recCount = 0;
            var excessCount = 0;
            var shortCount = 0;

            var Weight = 0;

            for (var i = 1; i < table.rows.length; i++) {
                table.rows[i].style.backgroundColor = '#FFFFFF';

                var tempWeight = table.rows[i].cells[5].childNodes[0].value;
                if (!isNaN(tempWeight)) {
                    Weight = Weight + parseFloat(tempWeight);
                }

                var temp = table.rows[i].cells[0].getElementsByClassName('hidden')[0].value;
                if (temp == "7") {
                    excessCount = excessCount + 1;
                }
                else if (temp == "5") {
                    recCount = recCount + 1;
                }
                else if (temp == "6") {
                    shortCount = shortCount + 1;
                }

            }

            var totalWeight = document.getElementById('<%= txt_totalLoadWeight.ClientID %>');
            if (!isNaN(totalWeight.value)) {
                totalWeight.value = parseFloat(totalWeight.value) + Weight;
            }

            tr.style.backgroundColor = '#8cf78c';
            receivedCN.value = "Received: " + recCount.toString();
            excessCN.value = "Excess: " + excessCount.toString();
            shortCN.value = "Short: " + shortCount.toString();
        }
        function TableStyleBag(tr) {
            var table = tr.parentElement;
            var receivedBags = document.getElementById('receivedBags');
            var excessBags = document.getElementById('excessBags');
            var shortBags = document.getElementById('shortBags');

            var recCount = 0;
            var excessCount = 0;
            var shortCount = 0;

            var Weight = 0;

            for (var i = 1; i < table.rows.length; i++) {
                table.rows[i].style.backgroundColor = '#FFFFFF';

                var tempWeight = table.rows[i].cells[7].childNodes[0].value;
                if (!isNaN(tempWeight)) {
                    Weight = Weight + parseFloat(tempWeight);
                }

                var temp = table.rows[i].cells[10].getElementsByClassName('hidden')[0].value;
                if (temp == "7") {
                    excessCount = excessCount + 1;
                }
                else if (temp == "5") {
                    recCount = recCount + 1;
                }
                else if (temp == "6") {
                    shortCount = shortCount + 1;
                }

            }

            var totalWeight = document.getElementById('<%= txt_totalLoadWeight.ClientID %>');
            if (!isNaN(totalWeight.value)) {
                totalWeight.value = parseFloat(totalWeight.value) + Weight;
            }

            tr.style.backgroundColor = '#8cf78c';
            receivedBags.value = "Received: " + recCount.toString();
            excessBags.value = "Excess: " + excessCount.toString();
            shortBags.value = "Short: " + shortCount.toString();
        }
        function RemoveBag(obj) {
            var txt_vid = document.getElementById("<%=txt_vid.ClientID %>");
            var tr = obj.parentElement.parentElement;
            var bagNumber = tr.cells[1].innerText;
            var Code = JSON.stringify({ 'bagNumber': bagNumber, 'loadingID': txt_vid.value });
            $.ajax({
                url: 'Unloading_speedy.aspx/RemoveBag',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                data: Code,
                success: function (response) {
                    var a = "";
                    if (response.d == "OK") {
                        alert('Bag Removed');
                        TableStyleBag(tr);
                        tr.parentElement.deleteRow(tr.rowIndex);

                    }
                    else {
                        alert(response.d.toString());
                    }
                },
                failure: function (response) {
                    alert(response.responseText);
                },
                error: function (response) {
                    alert(response.responseText);
                }
            });
        }

        function RemoveBAGRow(tr) {
            var txt_vid = document.getElementById("<%=txt_vid.ClientID %>");
            var txt_Bagno = document.getElementById('txt_Bagno');

            var bagNumber = tr.cells[1].innerText;
            var Code = JSON.stringify({ 'bagNumber': bagNumber, 'loadingID': txt_vid.value });
            $.ajax({
                url: 'Unloading_speedy.aspx/RemoveBag',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                data: Code,
                success: function (response) {
                    var a = "";
                    if (response.d == "OK") {
                        tr.parentElement.deleteRow(tr.rowIndex);
                        txt_Bagno.value = '';
                        txt_Bagno.focus();
                    }
                },
                failure: function (response) {
                    alert(response.responseText);
                },
                error: function (response) {
                    alert(response.responseText);
                }
            });
        }

        function RemoveCN(obj) {
            var txt_vid = document.getElementById("<%=txt_vid.ClientID %>");
            var tr = obj.parentElement.parentElement;
            var CnNumber = tr.cells[1].innerText;
            var Code = JSON.stringify({ 'CnNumber': CnNumber, 'loadingID': txt_vid.value });
            $.ajax({
                url: 'Unloading_speedy.aspx/RemoveCN',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                data: Code,
                success: function (response) {
                    var a = "";
                    if (response.d == "OK") {
                        alert('CN Removed');
                        tr.parentElement.deleteRow(tr.rowIndex);
                    }
                    else {
                        alert(response.d.toString());
                    }
                },
                failure: function (response) {
                    alert(response.responseText);
                },
                error: function (response) {
                    alert(response.responseText);
                }
            });
        }

        function RemoveCNRow(tr) {
            var txt_vid = document.getElementById("<%=txt_vid.ClientID %>");
            var CnNumber = tr.cells[1].innerText;
            var txt_consignmentno = document.getElementById('txt_consignmentno');
            var Code = JSON.stringify({ 'CnNumber': CnNumber, 'loadingID': txt_vid.value });
            $.ajax({
                url: 'Unloading_speedy.aspx/RemoveCN',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                data: Code,
                success: function (response) {
                    var a = "";
                    if (response.d == "OK") {
                        tr.parentElement.deleteRow(tr.rowIndex);
                        txt_consignmentno.value = '';
                        txt_consignmentno.focus();
                    }
                },
                failure: function (response) {
                    alert(response.responseText);
                },
                error: function (response) {
                    alert(response.responseText);
                }
            });
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
    <script language="javascript" type="text/javascript">
        ///////////////////////////////////// SAVE AUTO UNLOADING BAG ////////////////////////////////////////////
        function SaveUnloading_BAG(tr, mode) {
            var jsonObj = { inputsValues: {}, Bag: {} }
            var txt_vid = document.getElementById("<%=txt_vid.ClientID %>");
            var dd_route = document.getElementById("<%=dd_route.ClientID %>");
            var txt_date = document.getElementById("<%=txt_date.ClientID %>");
            var dd_transporttype = document.getElementById("<%=dd_transporttype.ClientID %>");
            var dd_orign = document.getElementById("<%=dd_orign.ClientID %>");
            var dept_date = document.getElementById("<%=dept_date.ClientID %>");
            var Vehicle = document.getElementById("<%=Vehicle.ClientID %>");
            var Rented = document.getElementById("<%=Rented.ClientID %>");
            var dd_destination = document.getElementById("<%=dd_destination.ClientID %>");
            var dd_vehicle = document.getElementById("<%=dd_vehicle.ClientID %>");
            var txt_reg = document.getElementById("<%=txt_reg.ClientID %>");
            var txt_description = document.getElementById("<%=txt_description.ClientID %>");
            var txt_couriername = document.getElementById("<%=txt_couriername.ClientID %>");
            var txt_seal = document.getElementById("<%=txt_seal.ClientID %>");
            var txt_totalLoadWeight = document.getElementById("<%=txt_totalLoadWeight.ClientID %>");
            var divDialogue = document.getElementById("<%=divDialogue.ClientID %>");
            var hd_master = document.getElementById("<%=hd_master.ClientID %>");



            var selected_vehicle = "";
            if (!Rented.checked) {
                selected_vehicle = dd_vehicle.options[dd_vehicle.selectedIndex].value;
            }
            else {
                selected_vehicle = '103';
            }
            var selected_transporttype = dd_transporttype.options[dd_transporttype.selectedIndex].value;
            if (selected_transporttype == "") {
                alert('Select Transport Type');

            }
            var chk_user = false;
            var inputsValues_array = [];

            inputsValues_array[0] = txt_vid.value;
            inputsValues_array[1] = dd_route.options[dd_route.selectedIndex].value;
            inputsValues_array[2] = txt_date.value;
            inputsValues_array[3] = dd_transporttype.options[dd_transporttype.selectedIndex].value;
            inputsValues_array[4] = dd_orign.options[dd_orign.selectedIndex].value;
            if (Vehicle.checked) {
                inputsValues_array[5] = "true";
            }
            else {
                inputsValues_array[5] = "false";
            }
            if (Rented.checked) {
                inputsValues_array[6] = "true";
            }
            else {
                inputsValues_array[6] = "false";
            }

            inputsValues_array[7] = dd_destination.options[dd_destination.selectedIndex].value;
            inputsValues_array[8] = selected_vehicle;
            // dd_vehicle.options[dd_vehicle.selectedIndex].value;

            inputsValues_array[9] = txt_description.value;
            inputsValues_array[10] = txt_couriername.value;
            inputsValues_array[11] = txt_seal.value;
            inputsValues_array[12] = txt_totalLoadWeight.value;
            if (inputsValues_array[6] == "true") {
                inputsValues_array[13] = txt_reg.value;
            }
            else {
                inputsValues_array[13] = "";
            }
            if (inputsValues_array[3] == "197") {
                inputsValues_array[14] = txt_flight.value;
                inputsValues_array[15] = dept_date.value;
            }
            else {
                inputsValues_array[14] = "";
                inputsValues_array[15] = "";
            }
            inputsValues_array[16] = hd_master.value;

            var inputsValues = {
                txt_vid: inputsValues_array[0],
                dd_route: inputsValues_array[1],
                txt_date: inputsValues_array[2],
                dd_transporttype: inputsValues_array[3],
                dd_orign: inputsValues_array[4],
                Vehicle: inputsValues_array[5],
                Rented: inputsValues_array[6],
                dd_destination: inputsValues_array[7],
                dd_vehicle: inputsValues_array[8],
                txt_description: inputsValues_array[9],
                txt_couriername: inputsValues_array[10],
                txt_seal: inputsValues_array[11],
                txt_totalLoadWeight: inputsValues_array[12],
                txt_reg: inputsValues_array[13],
                txt_flight: inputsValues_array[14],
                dept_date: inputsValues_array[15],
                hd_master: inputsValues_array[16]

            }
            jsonObj.inputsValues = inputsValues;

            var tbl_bag_array = [];
            var chk = tr.cells[0].getElementsByClassName('checkbox');
            if (chk[0].checked) {
                tbl_bag_array[0] = "true";
            }
            else {
                tbl_bag_array[0] = "false";
            }
            tbl_bag_array[1] = tr.cells[1].innerHTML;
            tbl_bag_array[2] = tr.cells[2].innerHTML;
            tbl_bag_array[3] = tr.cells[3].innerHTML;
            tbl_bag_array[4] = tr.cells[4].innerHTML;
            tbl_bag_array[5] = tr.cells[5].innerHTML;
            tbl_bag_array[6] = tr.cells[6].getElementsByClassName('textBox')[0].value;
            tbl_bag_array[7] = tr.cells[7].getElementsByClassName('textBox')[0].value;
            tbl_bag_array[8] = tr.cells[8].getElementsByClassName('textBox')[0].value;
            tbl_bag_array[9] = tr.cells[10].getElementsByClassName('hidden')[0].value;
            tbl_bag_array[11] = tr.cells[11].getElementsByClassName('hidden')[0].value;
            tbl_bag_array[12] = tr.cells[12].getElementsByClassName('hidden')[0].value;
            tbl_bag_array[13] = tr.cells[13].getElementsByClassName('hidden')[0].value;
            tbl_bag_array[14] = tr.cells[14].getElementsByClassName('hidden')[0].value;



            var Bag = {
                chk_received: tbl_bag_array[0],
                BagNo: tbl_bag_array[1],
                Description: tbl_bag_array[2],
                Origin: tbl_bag_array[3],
                Destination: tbl_bag_array[4],
                Status: tbl_bag_array[5],
                SealNo: tbl_bag_array[6],
                Weight: tbl_bag_array[7],
                Remarks: tbl_bag_array[8],
                hd_status: tbl_bag_array[9],
                hd_origin: tbl_bag_array[11],
                hd_descStatus: tbl_bag_array[12],
                hd_destination: tbl_bag_array[13],
                bagStatus: tbl_bag_array[14]

            }
            jsonObj.Bag = Bag;

            $.ajax({

                url: 'Unloading_speedy.aspx/save_BAG',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                data: JSON.stringify(jsonObj),
                success: function (result) {
                    var resp = result.d;
                    var err_msg = document.getElementById('<%=err_msg.ClientID%>');

                    if (resp == "Vehicle") {
                        err_msg.value = "Select Vehicle";
                        alert("Select Vehicle");
                    }
                    else if (resp == "Transport") {
                        err_msg.value = "Select Transport Type";
                        alert("Select Transport Type");
                    }
                    else if (resp == "Flight") {
                        err_msg.value = "Provide Flight Departure time";
                        alert("Provide Flight Departure time");
                    }
                    else {
                        var response = resp.split(";");
                        if (response[0] == "Unload Successful.") {
                            txt_Bagno = document.getElementById('txt_Bagno');
                            hd_master.value = response[1];
                            if (mode == "") {
                                txt_Bagno.disabled = false;
                                txt_Bagno.value = '';
                            }
                            focusWorking(txt_Bagno);
                            TableStyleBag(tr);
                            SaveChangedValues();
                        }
                        else {
                            RemoveBAGRow(tr);
                            alert(resp);
                        }
                    }

                },
            });

        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////// SAVE AUTO UNLOADING CN ////////////////////////////////////////////
        function SaveUnloading_CN(tr, mode) {
            var jsonObj = { inputsValues: {}, CN: {} }


            var txt_vid = document.getElementById("<%=txt_vid.ClientID %>");
            var dd_route = document.getElementById("<%=dd_route.ClientID %>");
            var txt_date = document.getElementById("<%=txt_date.ClientID %>");
            var dd_transporttype = document.getElementById("<%=dd_transporttype.ClientID %>");
            var dd_orign = document.getElementById("<%=dd_orign.ClientID %>");
            //  var txt_flight = document.getElementById("<%=txt_flight.ClientID %>");
            var dept_date = document.getElementById("<%=dept_date.ClientID %>");
            var Vehicle = document.getElementById("<%=Vehicle.ClientID %>");
            var Rented = document.getElementById("<%=Rented.ClientID %>");
            var dd_destination = document.getElementById("<%=dd_destination.ClientID %>");
            var dd_vehicle = document.getElementById("<%=dd_vehicle.ClientID %>");
            var txt_reg = document.getElementById("<%=txt_reg.ClientID %>");
            var txt_description = document.getElementById("<%=txt_description.ClientID %>");
            var txt_couriername = document.getElementById("<%=txt_couriername.ClientID %>");
            var txt_seal = document.getElementById("<%=txt_seal.ClientID %>");
            var txt_totalLoadWeight = document.getElementById("<%=txt_totalLoadWeight.ClientID %>");
            var divDialogue = document.getElementById("<%=divDialogue.ClientID %>");
            var hd_master = document.getElementById("<%=hd_master.ClientID %>");
            var selected_vehicle = "";
            if (!Rented.checked) {
                selected_vehicle = dd_vehicle.options[dd_vehicle.selectedIndex].value;
            }
            else {
                selected_vehicle = '103';
            }


            if (selected_vehicle == "") {
                alert('Select Vehicle');

            }
            var selected_transporttype = dd_transporttype.options[dd_transporttype.selectedIndex].value;
            if (selected_transporttype == "") {
                alert('Select Transport Type');

            }
            var chk_user = false;
            var inputsValues_array = [];

            inputsValues_array[0] = txt_vid.value;
            inputsValues_array[1] = dd_route.options[dd_route.selectedIndex].value;
            inputsValues_array[2] = txt_date.value;
            inputsValues_array[3] = dd_transporttype.options[dd_transporttype.selectedIndex].value;
            inputsValues_array[4] = dd_orign.options[dd_orign.selectedIndex].value;
            if (Vehicle.checked) {
                inputsValues_array[5] = "true";
            }
            else {
                inputsValues_array[5] = "false";
            }
            if (Rented.checked) {
                inputsValues_array[6] = "true";
            }
            else {
                inputsValues_array[6] = "false";
            }

            inputsValues_array[7] = dd_destination.options[dd_destination.selectedIndex].value;
            inputsValues_array[8] = selected_vehicle;
            //dd_vehicle.options[dd_vehicle.selectedIndex].value;
            inputsValues_array[9] = txt_description.value;
            inputsValues_array[10] = txt_couriername.value;
            inputsValues_array[11] = txt_seal.value;
            inputsValues_array[12] = txt_totalLoadWeight.value;
            if (inputsValues_array[6] == "true") {
                inputsValues_array[13] = txt_reg.value;
            }
            else {
                inputsValues_array[13] = "";
            }
            if (inputsValues_array[3] == "197") {
                inputsValues_array[14] = txt_flight.value;
                inputsValues_array[15] = dept_date.value;
            }
            else {
                inputsValues_array[14] = "";
                inputsValues_array[15] = "";
            }
            inputsValues_array[16] = hd_master.value;

            var inputsValues = {
                txt_vid: inputsValues_array[0],
                dd_route: inputsValues_array[1],
                txt_date: inputsValues_array[2],
                dd_transporttype: inputsValues_array[3],
                dd_orign: inputsValues_array[4],
                Vehicle: inputsValues_array[5],
                Rented: inputsValues_array[6],
                dd_destination: inputsValues_array[7],
                dd_vehicle: inputsValues_array[8],
                txt_description: inputsValues_array[9],
                txt_couriername: inputsValues_array[10],
                txt_seal: inputsValues_array[11],
                txt_totalLoadWeight: inputsValues_array[12],
                txt_reg: inputsValues_array[13],
                txt_flight: inputsValues_array[14],
                dept_date: inputsValues_array[15],
                hd_master: inputsValues_array[16]

            }
            jsonObj.inputsValues = inputsValues;

            var tbl_cn_array = [];
            var column = tr.getElementsByTagName("td")[0];
            var chk = tr.cells[0].getElementsByClassName('checkbox');
            var hd_descStatus = column.childNodes[1];
            var hd_origin = column.childNodes[2];
            var hd_destination = column.childNodes[3];
            var hd_cnStatus = column.childNodes[4];
            var dropdown = tr.cells[3].getElementsByClassName('dropdown');

            tbl_cn_array[0] = "false";
            if (chk[0].checked) {
                tbl_cn_array[0] = "true";
            }
            tbl_cn_array[1] = hd_descStatus.value;
            tbl_cn_array[2] = hd_origin.value;
            tbl_cn_array[3] = hd_destination.value;
            tbl_cn_array[4] = hd_cnStatus.value;
            tbl_cn_array[5] = tr.cells[1].innerHTML;
            tbl_cn_array[6] = tr.cells[2].innerHTML;
            tbl_cn_array[7] = dropdown.dd_gOrigin.selectedIndex;
            tbl_cn_array[8] = tr.cells[4].innerHTML;
            tbl_cn_array[9] = tr.cells[5].getElementsByClassName('textBox')[0].value;
            tbl_cn_array[10] = tr.cells[6].getElementsByClassName('textBox')[0].value;
            tbl_cn_array[11] = tr.cells[7].getElementsByClassName('textBox')[0].value;



            var CN = {
                chk_received: tbl_cn_array[0],
                hd_descStatus: tbl_cn_array[1],
                hd_origin: tbl_cn_array[2],
                hd_destination: tbl_cn_array[3],
                hd_cnStatus: tbl_cn_array[4],
                ConNumber: tbl_cn_array[5],
                Description: tbl_cn_array[6],
                Origin: tbl_cn_array[7],
                Destination: tbl_cn_array[8],
                Weight: tbl_cn_array[9],
                Pieces: tbl_cn_array[10],
                Remarks: tbl_cn_array[11]

            }
            jsonObj.CN = CN;

            $.ajax({

                url: 'Unloading_speedy.aspx/save_CN',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                data: JSON.stringify(jsonObj),
                success: function (result) {
                    var resp = result.d;
                    var err_msg = document.getElementById('<%=err_msg.ClientID%>');

                    if (resp == "Vehicle") {
                        err_msg.value = "Select Vehicle";
                        alert("Select Vehicle");
                    }
                    else if (resp == "Transport") {
                        err_msg.value = "Select Transport Type";
                        alert("Select Transport Type");
                    }
                    else if (resp == "Flight") {
                        err_msg.value = "Provide Flight Departure time";
                        alert("Provide Flight Departure time");
                    }
                    else {
                        var response = resp.split(";");
                        if (response[0] == "Unload Successful.") {
                            hd_master.value = response[1];
                            txt_consignmentno_ = document.getElementById('txt_consignmentno');
                            if (mode == "") {
                                txt_consignmentno_.disabled = false;
                                txt_consignmentno_.value = '';
                            }
                            TableStyleCN(tr);
                            focusWorking(txt_consignmentno_);
                            SaveChangedValues();
                            //alert(resp);

                        }
                        else {
                            alert(resp);
                        }
                    }

                },
            });
        }


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        function isNumberKey(evt) {


            var count = 1;
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode != 46 && charCode > 31
                && (charCode < 48 || charCode > 57 || charCode == 110 || charCode == 109 || charCode == 45 || charCode == 9 || charCode == 13)) {
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


            var txt_cn = document.getElementById('txt_consignmentno');
            var cn = document.getElementById('txt_consignmentno');

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

            //COD Controls start
            debugger;
            var OutputMessage = '';
            var CODControlCheck = true;
            $.ajax({
                async: false,
                type: 'post',
                url: 'Unloading_speedy.aspx/CheckControls',
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
                return false;
            }

            //COD Controls end

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
            if (!prefixNotFound) {
                chkManChanged_CN();
            }
            return true;
        }
        function Show_Hide_By_Display() {

            var obj = document.getElementById("<%=hd_tblBag.ClientID %>");
            obj.value = document.getElementById('tbl_bagnew');
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
        //////////////////////////// Table BAG /////////////////////////////

        function chkManChanged() {



            var tbl_bagnew = document.getElementById('tbl_bagnew');
            var bag = document.getElementById('txt_Bagno').value;
            var txt_Bagno = document.getElementById('txt_Bagno');

            txt_Bagno.disabled = true;

            var Bag = txt_Bagno.value.trim();
            if (Bag.trim() == "") {
                alert('Enter Bag');
                txt_Bagno.focus();
                txt_Bagno.disabled = false;
                return;
            }

            var isnum = /^\d+$/.test(Bag);
            if (!isnum) {
                alert('Kindly Insert Proper Bag Number');
                txt_Bagno.focus();
                txt_Bagno.value = '';
                txt_Bagno.disabled = false;
                return;
            }
            var tbody = tbl_bagnew.getElementsByTagName("tbody")[0];
            var count = 0;
            var rowCount = 1;
            var row;
            var flag = true;

            for (var i = 1; i < tbl_bagnew.rows.length; i++) {
                row = tbody.getElementsByTagName("tr")[i];
                var column = row.getElementsByTagName("td")[4];
                var RemarksColumn = row.getElementsByTagName("td")[8];
                var chk = row.getElementsByTagName("input");
                var hd_status = row.cells[10].getElementsByTagName("hidden")[0];
                var hd_NewBag = RemarksColumn.childNodes[1];
                var descStatus = row.cells[12].getElementsByTagName("hidden");
                var txt_BagNo = row.cells[1].innerText;
                rowCount = i;

                if (txt_BagNo == bag.trim()) {
                    if (hd_NewBag.value == '0' && row.cells[2].innerText != 'RECEIVED') {
                        chk[0].checked = true;
                        row.cells[2].innerText = 'RECEIVED';
                        hd_status.value = '5';
                        descStatus.value = 'RECEIVED';
                        count++;
                        break;
                    }
                    else {
                        flag = false;
                        alert('Already Bag added.');
                        txt_Bagno.disabled = false;
                        txt_Bagno.value = '';
                        break;
                    }
                }

            }
            if (count > 0) {
                txt_Bagno.disabled = true;
                SaveUnloading_BAG(row, 0);

            } else {
                if (flag) {
                    txt_Bagno.disabled = true;
                    getData();
                }
                //   val.value = "Yes";
            }



        }
        function SaveChangedValues() {
            var tblBags = document.getElementById('tbl_bagnew');
            var tblCn = document.getElementById('tbl_cns');
            var hdMaster = document.getElementById('<%= hd_master.ClientID %>');
            var unloadingID = "0";
            if (hdMaster.value == "0") {
                return;
            }
            else {
                unloadingID = hdMaster.value;
            }
            var jsonObj = { unloadingID: unloadingID, Bags: [], Consignments: [] }

            for (var i = 1; i < tblBags.rows.length; i++) {
                var tr = tbl_bagnew.rows[i];
                var dirtyFlag = tr.cells[0].getElementsByClassName('dirtyFlag')[0];
                if (dirtyFlag.value == "1") {
                    var Bag = {
                        BagNo: tr.cells[1].innerText,
                        SealNo: tr.cells[6].childNodes[0].value,
                        Weight: tr.cells[7].childNodes[0].value,
                        Remarks: tr.cells[8].childNodes[0].value
                    };
                    jsonObj.Bags.push(Bag);
                }
            }

            for (var i = 1; i < tblCn.rows.length; i++) {
                var tr = tblCn.rows[i];
                var dirtyFlag = tr.cells[0].getElementsByClassName('dirtyFlag')[0];
                var origin = "";
                if (dirtyFlag.value == "1") {
                    var dd_origin = tr.cells[3].childNodes[0];
                    origin = dd_origin.options[dd_origin.selectedIndex].value;
                    var Consignment = {
                        ConNumber: tr.cells[1].innerText,
                        hd_origin: origin,
                        Weight: tr.cells[5].childNodes[0].value,
                        Pieces: tr.cells[6].childNodes[0].value,
                        Remarks: tr.cells[7].childNodes[0].value
                    };
                    jsonObj.Consignments.push(Consignment);
                }
            }


            if ((jsonObj.Bags.length > 0 || jsonObj.Consignments.length > 0) && unloadingID != "0") {
                $.ajax({
                    url: 'Unloading_speedy.aspx/SaveChangedValues',
                    type: 'POST',
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    data: JSON.stringify(jsonObj),
                    success: function (response) {
                        var resp = response.d;
                        var Bags = resp.Bags;
                        var Consignments = resp.Consignments;

                        if (resp.Status != "OK") {
                            return;
                        }

                        if (Bags.length > 0) {
                            for (var i = 0; i < Bags.length; i++) {
                                for (var j = 1; j < tblBags.rows.length; j++) {
                                    var tr = tblBags.rows[j];
                                    var bagNo = tr.cells[1].innerText;
                                    if (bagNo == Bags[i].BagNo) {
                                        var dirtyFlag = tr.cells[0].getElementsByClassName('dirtyFlag')[0];
                                        dirtyFlag.value = "0";
                                        break;
                                    }
                                }
                            }
                        }

                        if (Consignments.length > 0) {
                            for (var i = 0; i < Consignments.length; i++) {
                                for (var j = 1; j < tblCn.rows.length; j++) {
                                    var tr = tblCn.rows[j];
                                    var conNumber = tr.cells[1].innerText;
                                    if (conNumber == Consignments[i].ConNumber) {
                                        var dirtyFlag = tr.cells[0].getElementsByClassName('dirtyFlag')[0];
                                        dirtyFlag.value = "0";
                                        break;
                                    }
                                }
                            }
                        }
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    },
                    error: function (response) {
                        alert(response.responseText);
                    }
                });
            }


        }
        function getData() {

            var bagNo = document.getElementById('txt_Bagno').value;
            var originName = "";
            var origin = "";
            var destinationName = "";
            var destination = "";
            var dd_orign = document.getElementById("<%=dd_orign.ClientID %>");
            var dd_destination = document.getElementById("<%=dd_destination.ClientID %>");

            originName = dd_orign.options[dd_orign.options.selectedIndex].text;
            origin = dd_orign.options[dd_orign.options.selectedIndex].value;

            destinationName = dd_destination.options[dd_destination.options.selectedIndex].text;
            destination = dd_destination.options[dd_destination.options.selectedIndex].value;

            var Code = JSON.stringify({ 'bagNo': bagNo, 'origin': origin, 'originName': originName, 'destination': destination, 'destinationName': destinationName });
            $.ajax({
                url: 'Unloading_speedy.aspx/GetBagNo',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                data: Code,
                success: OnSuccess,
                failure: function (response) {
                    alert(response.responseText);
                },
                error: function (response) {
                    alert(response.responseText);
                }
            });

        }

        function OnSuccess(response) {

            var dt = JSON.parse(response.d);
            var count = dt.Table1.length;
            var tbl_bagnew = document.getElementById('tbl_bagnew');

            if (count - 1 > 0) {
                for (var i = 0; i < count - 1; i++) {
                    var newTr = tbl_bagnew.insertRow(i + 1);  //document.createElement('tr');  //consignments.rows[0].cloneNode(true);
                    newTr.className = 'DetailRow';

                    var chk_received = document.createElement('input');
                    chk_received.type = 'checkbox';
                    chk_received.style.marginTop = '2px';
                    chk_received.style.marginBottom = '2px';
                    chk_received.style.width = '80%';
                    chk_received.className = 'checkbox';
                    chk_received.disabled = true;
                    chk_received.checked = true;

                    var txt_gSealNo = document.createElement('input');
                    txt_gSealNo.type = 'text';
                    txt_gSealNo.className = 'textBox';
                    txt_gSealNo.value = dt.Table1[i][6];
                    txt_gSealNo.style.width = '100%';
                    txt_gSealNo.style.textAlign = 'center';

                    var txt_gBagWeight = document.createElement('input');
                    txt_gBagWeight.type = 'text';
                    txt_gBagWeight.className = 'textBox';
                    txt_gBagWeight.value = dt.Table1[i][7];
                    txt_gBagWeight.style.width = '90%';
                    txt_gBagWeight.style.textAlign = 'center';

                    var txt_gBagRemarks = document.createElement('input');
                    txt_gBagRemarks.type = 'text';
                    txt_gBagRemarks.className = 'textBox';
                    txt_gBagRemarks.value = dt.Table1[i][5];
                    txt_gBagRemarks.style.width = '90%';
                    txt_gBagRemarks.style.textAlign = 'center';

                    var hd_status = document.createElement('hidden');
                    hd_status.type = 'hidden';
                    hd_status.className = 'hidden';
                    hd_status.value = "7";
                    hd_status.style.textAlign = 'center';

                    var hd_origin = document.createElement('hidden');
                    hd_origin.type = 'hidden';
                    hd_origin.className = 'hidden';
                    hd_origin.value = dt.Table1[i][3];
                    hd_origin.style.textAlign = 'center';

                    var hd_descStatus = document.createElement('hidden');
                    hd_descStatus.type = 'hidden';
                    hd_descStatus.className = 'hidden';
                    hd_descStatus.value = "Excess Received";
                    hd_descStatus.style.textAlign = 'center';

                    var hd_destination = document.createElement('hidden');
                    hd_destination.type = 'hidden';
                    hd_destination.className = 'hidden';
                    hd_destination.value = dt.Table1[i][4];
                    hd_destination.style.textAlign = 'center';

                    var hd_NewBag = document.createElement('hidden');
                    hd_NewBag.type = 'hidden';
                    hd_NewBag.className = 'hidden';
                    hd_NewBag.value = '1';
                    hd_NewBag.style.textAlign = 'center';
                    hd_NewBag.id = 'hd_NewBag'

                    var hdDirtyFlag = document.createElement('input');
                    hdDirtyFlag.type = "text";
                    hdDirtyFlag.style.display = 'none';
                    hdDirtyFlag.value = "1";
                    hdDirtyFlag.className = "dirtyFlag";

                    var bagStatus = document.createElement('hidden');
                    bagStatus.type = 'hidden';
                    bagStatus.className = 'hidden';
                    bagStatus.value = dt.Table1[i][10];
                    bagStatus.style.textAlign = 'center';

                    var btn_remove = document.createElement('input');
                    btn_remove.type = 'button';
                    btn_remove.className = 'button button1';
                    btn_remove.value = 'Remove';
                    btn_remove.style.marginTop = '2px';
                    btn_remove.style.marginBottom = '2px';
                    btn_remove.style.width = '90%';
                    //   btn_remove.onclick = RemoveConsignment.bind(newTr.cells[0].childNodes[0], newTr.cells[0].childNodes[0]);

                    td = document.createElement('td');
                    td.className = 'gridcell';
                    td.style.fontWeight = 'normal';
                    td.rowSpan = 1;
                    td.style.width = '80px';

                    ///////////Recieved	//////////////
                    col = newTr.insertCell(0);
                    //   td.appendChild(chk_received);
                    newTr.cells[0].appendChild(chk_received);
                    newTr.cells[0].appendChild(hdDirtyFlag);
                    //newTr.cells[0].appendChild(chk_received);
                    //newTr.cells[0].childNodes[0].disabled = true;

                    /////////// Bag No.	//////////////
                    col = newTr.insertCell(1);
                    newTr.cells[1].innerText = dt.Table1[i][0];
                    ///////////Description	//////////////
                    col = newTr.insertCell(2);
                    newTr.cells[2].innerText = "Excess Received";
                    ///////////Origin////////////////////
                    col = newTr.insertCell(3);
                    newTr.cells[3].innerText = dt.Table1[i][1];
                    ///////////Destination////////////////////
                    col = newTr.insertCell(4);
                    newTr.cells[4].innerText = dt.Table1[i][2];
                    ///////////Status////////////////////
                    col = newTr.insertCell(5);
                    newTr.cells[5].innerText = dt.Table1[i][5];
                    ///////////Seal No.////////////////////
                    col = newTr.insertCell(6);
                    //    td.appendChild(txt_gSealNo);
                    newTr.cells[6].appendChild(txt_gSealNo);
                    //newTr.cells[6].appendChild(txt_gSealNo);
                    //newTr.cells[6].childNodes[0].value = dt.Table1[i][8];

                    ///////////Weight////////////////////
                    col = newTr.insertCell(7);
                    //   td.appendChild(txt_gBagWeight);
                    newTr.cells[7].appendChild(txt_gBagWeight);
                    newTr.cells[7].style.textAlign = "center";
                    newTr.cells[7].childNodes[0].onchange = CalculateTotalWeight.bind(newTr.cells[7].childNodes[0]);
                    newTr.cells[7].childNodes[0].maxLength = "5";

                    ///////////Remarks////////////////////
                    col = newTr.insertCell(8);
                    newTr.cells[8].appendChild(txt_gBagRemarks);
                    newTr.cells[8].appendChild(hd_NewBag);
                    ///////////Remove////////////////////
                    col = newTr.insertCell(9);
                    newTr.cells[9].appendChild(btn_remove);
                    newTr.cells[9].onclick = RemoveBag.bind(newTr.cells[9].childNodes[0], newTr.cells[9].childNodes[0]);


                    col = newTr.insertCell(10);
                    newTr.cells[10].appendChild(hd_status);
                    col = newTr.insertCell(11);
                    newTr.cells[11].appendChild(hd_origin);

                    col = newTr.insertCell(12);
                    newTr.cells[12].appendChild(hd_descStatus);

                    col = newTr.insertCell(13);
                    newTr.cells[13].appendChild(hd_destination);

                    col = newTr.insertCell(14);
                    newTr.cells[14].appendChild(bagStatus);

                    SaveUnloading_BAG(newTr, "");

                }
            }

            validate();
        }
        function SetDirtyFlag(cnt) {
            var tr = cnt.parentElement.parentElement;
            var dirtyFlag = tr.cells[0].getElementsByClassName('dirtyFlag')[0];
            dirtyFlag.value = "1";
            CalculateTotalWeight();

        }

        function LoadTable(responseDT, responseDTC, unloaded) {
            debugger;
            if (responseDT != '') {
                var dt = JSON.parse(responseDT);
                var count = dt.Table1.length;
                if ((count - 1) > 0) {
                    getdt(responseDT);
                    if (!unloaded) {
                        document.getElementById('txt_Bagno').disabled = false;
                    }

                }
            }
            if (responseDTC != '') {
                var DTC = JSON.parse(responseDTC);
                count = DTC.Table1.length;
                if ((count - 1) > 0) {
                    getDTC(responseDTC);
                    if (!unloaded) {
                        document.getElementById('txt_consignmentno').disabled = false;
                    }
                }
            }

            var hd_unloaded = document.getElementById('<%= hd_unloaded.ClientID %>');
            if (hd_unloaded.value == "1") {
                document.getElementById('txt_Bagno').disabled = true;
                document.getElementById('txt_consignmentno').disabled = true;
            }
            else {
                document.getElementById('txt_Bagno').disabled = false;
                document.getElementById('txt_consignmentno').disabled = false;
            }

        }
        function getdt(response) {

            var dt = JSON.parse(response);
            var count = dt.Table1.length;
            var tbl_bagnew = document.getElementById('tbl_bagnew');
            var hd_unloaded = document.getElementById('<%= hd_unloaded.ClientID %>');
            var totalWeight = 0;

            if ((count - 1) > 0) {
                for (var i = 0; i < count - 1; i++) {
                    var newTr = tbl_bagnew.insertRow(i + 1);  //document.createElement('tr');  //consignments.rows[0].cloneNode(true);
                    newTr.className = 'DetailRow';

                    var hdDirtyFlag = document.createElement('input');
                    hdDirtyFlag.type = "text";
                    hdDirtyFlag.style.display = 'none';
                    hdDirtyFlag.value = "0";
                    hdDirtyFlag.className = "dirtyFlag";

                    var chk_received = document.createElement('input');
                    chk_received.type = 'checkbox';
                    chk_received.style.marginTop = '2px';
                    chk_received.style.marginBottom = '2px';
                    chk_received.style.width = '80%';
                    chk_received.className = 'checkbox';
                    chk_received.disabled = true;

                    var txt_gSealNo = document.createElement('input');
                    txt_gSealNo.type = 'text';
                    txt_gSealNo.className = 'textBox';
                    txt_gSealNo.value = dt.Table1[i][8];
                    txt_gSealNo.style.width = '100%';
                    txt_gSealNo.style.textAlign = 'center';

                    var tempWeight = 0;
                    if (!isNaN(parseFloat(dt.Table1[i][2]))) {
                        tempWeight = parseFloat(dt.Table1[i][2]);
                        totalWeight = totalWeight + tempWeight;
                    }
                    var txt_gBagWeight = document.createElement('input');
                    txt_gBagWeight.type = 'text';
                    txt_gBagWeight.className = 'textBox';
                    txt_gBagWeight.value = dt.Table1[i][2];
                    txt_gBagWeight.style.width = '90%';
                    txt_gBagWeight.style.textAlign = 'center';

                    var txt_gBagRemarks = document.createElement('input');
                    txt_gBagRemarks.type = 'text';
                    txt_gBagRemarks.className = 'textBox';
                    txt_gBagRemarks.value = dt.Table1[i][9];
                    txt_gBagRemarks.style.width = '90%';
                    txt_gBagRemarks.style.textAlign = 'center';

                    var hd_status = document.createElement('hidden');
                    hd_status.type = 'hidden';
                    hd_status.className = 'hidden';
                    hd_status.value = "6";
                    hd_status.style.textAlign = 'center';

                    var hd_origin = document.createElement('hidden');
                    hd_origin.type = 'hidden';
                    hd_origin.className = 'hidden';
                    hd_origin.value = dt.Table1[i][4];
                    hd_origin.style.textAlign = 'center';

                    var hd_descStatus = document.createElement('hidden');
                    hd_descStatus.type = 'hidden';
                    hd_descStatus.className = 'hidden';
                    hd_descStatus.value = "Short Received";
                    hd_descStatus.style.textAlign = 'center';

                    var hd_destination = document.createElement('hidden');
                    hd_destination.type = 'hidden';
                    hd_destination.className = 'hidden';
                    hd_destination.value = dt.Table1[i][5];
                    hd_destination.style.textAlign = 'center';

                    var hd_NewBag = document.createElement('hidden');
                    hd_NewBag.type = 'hidden';
                    hd_NewBag.className = 'hidden';
                    hd_NewBag.value = '0';
                    hd_NewBag.style.textAlign = 'center';
                    hd_NewBag.id = 'hd_NewBag'

                    var bagStatus = document.createElement('hidden');
                    bagStatus.type = 'hidden';
                    bagStatus.className = 'hidden';
                    bagStatus.value = dt.Table1[i][10];
                    bagStatus.style.textAlign = 'center';

                    var btn_remove = document.createElement('input');
                    btn_remove.type = 'button';
                    btn_remove.className = 'button button1';
                    btn_remove.value = 'Remove';
                    btn_remove.style.marginTop = '2px';
                    btn_remove.style.marginBottom = '2px';
                    btn_remove.style.width = '90%';
                    //   btn_remove.onclick = RemoveConsignment.bind(newTr.cells[0].childNodes[0], newTr.cells[0].childNodes[0]);



                    td = document.createElement('td');
                    td.className = 'gridcell';
                    td.style.fontWeight = 'normal';
                    td.rowSpan = 1;
                    td.style.width = '80px';

                    ///////////Recieved	//////////////
                    col = newTr.insertCell(0);
                    newTr.cells[0].appendChild(chk_received);
                    newTr.cells[0].appendChild(hdDirtyFlag);

                    txt_gSealNo.onchange = SetDirtyFlag.bind(newTr.cells[0].childNodes[0], newTr.cells[0].childNodes[0]);
                    txt_gBagRemarks.onchange = SetDirtyFlag.bind(newTr.cells[0].childNodes[0], newTr.cells[0].childNodes[0]);
                    txt_gBagWeight.onchange = SetDirtyFlag.bind(newTr.cells[0].childNodes[0], newTr.cells[0].childNodes[0]);
                    /////////// Bag No.	//////////////
                    col = newTr.insertCell(1);
                    newTr.cells[1].innerText = dt.Table1[i][0];
                    ///////////Description	//////////////
                    col = newTr.insertCell(2);
                    newTr.cells[2].innerText = "Short Received";
                    ///////////Origin////////////////////
                    col = newTr.insertCell(3);
                    newTr.cells[3].innerText = dt.Table1[i][3];
                    //newTr.cells[3].appendChild(dd_origin);
                    ///////////Destination////////////////////
                    col = newTr.insertCell(4);
                    newTr.cells[4].innerText = dt.Table1[i][6];
                    ///////////Status////////////////////
                    col = newTr.insertCell(5);
                    newTr.cells[5].innerText = dt.Table1[i][7];
                    ///////////Seal No.////////////////////
                    col = newTr.insertCell(6);
                    newTr.cells[6].appendChild(txt_gSealNo);

                    ///////////Weight////////////////////
                    col = newTr.insertCell(7);

                    newTr.cells[7].appendChild(txt_gBagWeight);
                    newTr.cells[7].style.textAlign = "center";
                    newTr.cells[7].childNodes[0].onchange = CalculateTotalWeight.bind(newTr.cells[7].childNodes[0]);
                    newTr.cells[7].childNodes[0].maxLength = "5";

                    ///////////Remarks-HD_NEWBAG////////////////////
                    col = newTr.insertCell(8);
                    newTr.cells[8].appendChild(txt_gBagRemarks);
                    newTr.cells[8].appendChild(hd_NewBag);

                    ///////////Remove////////////////////
                    col = newTr.insertCell(9);

                    col = newTr.insertCell(10);
                    newTr.cells[10].appendChild(hd_status);

                    col = newTr.insertCell(11);
                    newTr.cells[11].appendChild(hd_origin);

                    col = newTr.insertCell(12);
                    newTr.cells[12].appendChild(hd_descStatus);

                    col = newTr.insertCell(13);
                    newTr.cells[13].appendChild(hd_destination);

                    col = newTr.insertCell(14);
                    newTr.cells[14].appendChild(bagStatus);
                }
            }
            validate();



        }

        //////////////////////////// Table CN /////////////////////////////
        function getDTC(response) {

            var dt = JSON.parse(response);
            var count = dt.Table1.length;
            var tbl_bagnew = document.getElementById('tbl_cns');

            var dropdown = document.getElementById('dd_Origin');
            $.ajax({
                url: 'Unloading_speedy.aspx/Cities',
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
                    var totalWeight = document.getElementById('<%= txt_totalLoadWeight.ClientID %>').value;
                    if (isNaN(parseFloat(totalWeight))) {
                        totalWeight = 0;
                    }
                    if (count - 1 > 0) {
                        for (var i = 0; i < count - 1; i++) {
                            var newTr = tbl_bagnew.insertRow(i + 1);  //document.createElement('tr');  //consignments.rows[0].cloneNode(true);
                            newTr.className = 'DetailRow';

                            var hdDirtyFlag = document.createElement('input');
                            hdDirtyFlag.type = "text";
                            hdDirtyFlag.style.display = 'none';
                            hdDirtyFlag.value = "0";
                            hdDirtyFlag.className = "dirtyFlag";

                            var chk_received = document.createElement('input');
                            chk_received.type = 'checkbox';
                            chk_received.style.marginTop = '2px';
                            chk_received.style.marginBottom = '2px';
                            chk_received.style.width = '80%';
                            chk_received.className = 'checkbox';
                            chk_received.disabled = true;

                            var hd_descStatus = document.createElement('hd_descStatus');
                            hd_descStatus.type = 'hidden';
                            hd_descStatus.className = 'hidden';
                            hd_descStatus.value = '6';
                            hd_descStatus.style.textAlign = 'center';
                            hd_descStatus.id = 'hd_descStatus'

                            var hd_origin = document.createElement('hd_origin');
                            hd_origin.type = 'hidden';
                            hd_origin.className = 'hidden';
                            hd_origin.value = dt.Table1[i][5];
                            hd_origin.style.textAlign = 'center';
                            hd_origin.id = 'hd_origin'

                            var hd_destination = document.createElement('hd_destination');
                            hd_destination.type = 'hidden';
                            hd_destination.className = 'hidden';
                            hd_destination.value = dt.Table1[i][6];
                            hd_destination.style.textAlign = 'center';
                            hd_destination.id = 'hd_destination'

                            var hd_cnStatus = document.createElement('hd_cnStatus');
                            hd_cnStatus.type = 'hidden';
                            hd_cnStatus.className = 'hidden';
                            hd_cnStatus.value = dt.Table1[i][9];
                            hd_cnStatus.style.textAlign = 'center';
                            hd_cnStatus.id = 'hd_cnStatus'

                            var hd_NewCN = document.createElement('hidden');
                            hd_NewCN.type = 'hidden';
                            hd_NewCN.className = 'hidden';
                            hd_NewCN.value = '0';
                            hd_NewCN.style.textAlign = 'center';
                            hd_NewCN.id = 'hd_NewCN'

                            var dd_origin = GetBranchDropDown();
                            dd_origin.style.width = '90%'
                            dd_origin.className = 'dropdown';
                            dd_origin.disabled = false;
                            dd_origin.value = dt.Table1[i][5];
                            dd_origin.style.textAlign = 'center';

                            var tempWeight = 0;
                            if (!isNaN(parseFloat(dt.Table1[i][2]))) {
                                tempWeight = parseFloat(dt.Table1[i][2]);
                                totalWeight = parseFloat(totalWeight) + parseFloat(tempWeight);
                            }

                            var txt_gCnWeight = document.createElement('input');
                            txt_gCnWeight.type = 'text';
                            txt_gCnWeight.className = 'textBox';
                            txt_gCnWeight.value = dt.Table1[i][2];
                            txt_gCnWeight.style.width = '90%';
                            txt_gCnWeight.style.textAlign = 'center';

                            var txt_gCnPieces = document.createElement('input');
                            txt_gCnPieces.type = 'text';
                            txt_gCnPieces.className = 'textBox';
                            txt_gCnPieces.value = dt.Table1[i][3];
                            txt_gCnPieces.style.width = '90%';
                            txt_gCnPieces.style.textAlign = 'center';

                            var txt_gCnRemarks = document.createElement('input');
                            txt_gCnRemarks.type = 'text';
                            txt_gCnRemarks.className = 'textBox';
                            txt_gCnRemarks.value = dt.Table1[i][8];
                            txt_gCnRemarks.style.width = '90%';
                            txt_gCnRemarks.style.textAlign = 'center';

                            var btn_remove = document.createElement('input');
                            btn_remove.type = 'button';
                            btn_remove.className = 'button button1';
                            btn_remove.value = 'Remove';
                            btn_remove.style.marginTop = '2px';
                            btn_remove.style.marginBottom = '2px';
                            btn_remove.style.width = '90%';
                            //   btn_remove.onclick = RemoveConsignment.bind(newTr.cells[0].childNodes[0], newTr.cells[0].childNodes[0]);

                            td = document.createElement('td');
                            td.className = 'gridcell';
                            td.style.fontWeight = 'normal';
                            td.rowSpan = 1;
                            td.style.width = '2px';





                            ///////////Recieved	//////////////
                            col = newTr.insertCell(0);
                            newTr.cells[0].appendChild(chk_received);
                            newTr.cells[0].appendChild(hd_descStatus);
                            newTr.cells[0].appendChild(hd_origin);
                            newTr.cells[0].appendChild(hd_destination);
                            newTr.cells[0].appendChild(hd_cnStatus);
                            newTr.cells[0].appendChild(hdDirtyFlag);

                            txt_gCnWeight.onchange = SetDirtyFlag.bind(newTr.cells[0].childNodes[0], newTr.cells[0].childNodes[0]);
                            txt_gCnPieces.onchange = SetDirtyFlag.bind(newTr.cells[0].childNodes[0], newTr.cells[0].childNodes[0]);
                            txt_gCnRemarks.onchange = SetDirtyFlag.bind(newTr.cells[0].childNodes[0], newTr.cells[0].childNodes[0]);

                            /////////// CN Number	//////////////
                            col = newTr.insertCell(1);
                            newTr.cells[1].innerText = dt.Table1[i][0];
                            ///////////Description	//////////////
                            col = newTr.insertCell(2);
                            newTr.cells[2].innerText = "Short Received";
                            ///////////Origin////////////////////
                            col = newTr.insertCell(3);
                            newTr.cells[3].appendChild(dd_origin);
                            ///////////Destination////////////////////
                            col = newTr.insertCell(4);
                            newTr.cells[4].innerText = dt.Table1[i][7];
                            ///////////Weight////////////////////
                            col = newTr.insertCell(5);
                            newTr.cells[5].appendChild(txt_gCnWeight);
                            newTr.cells[5].style.textAlign = "center";
                            newTr.cells[5].childNodes[0].onchange = CalculateTotalWeight.bind(newTr.cells[5].childNodes[0]);
                            newTr.cells[5].childNodes[0].maxLength = "5";
                            ///////////Pieces.////////////////////
                            col = newTr.insertCell(6);
                            newTr.cells[6].appendChild(txt_gCnPieces);
                            newTr.cells[6].appendChild(txt_gCnPieces);
                            newTr.cells[6].childNodes[0].onchange = validate.bind(newTr.cells[6].childNodes[0]);
                            newTr.cells[6].childNodes[0].maxLength = "4";

                            ///////////Remarks////////////////////
                            col = newTr.insertCell(7);
                            newTr.cells[7].appendChild(txt_gCnRemarks);
                            newTr.cells[7].appendChild(hd_NewCN);

                            ///////////Remove////////////////////
                            col = newTr.insertCell(8);
                        }
                        //document.getElementById('<%= txt_totalLoadWeight.ClientID %>').value = totalWeight.toString();
                        validate();
                    }


                },
                error: function () {

                },
                failure: function () {

                }

            })

        }

        function chkManChanged_CN() {

            var txt_consignmentno = document.getElementById('txt_consignmentno').value;
            var txt_consignmentno_ = document.getElementById('txt_consignmentno');
            txt_consignmentno_.disabled = true;

            var cn = txt_consignmentno_.value.trim();
            if (cn.trim() == "") {
                alert('Enter Consignment Number');
                txt_consignmentno_.focus();
                txt_consignmentno_.disabled = false;
                return;
            }

            var isnum = /^\d+$/.test(cn);
            if (!isnum) {
                alert('Kindly Insert Proper Consignment Number');
                focusWorking(txt_consignmentno_);
                txt_consignmentno_.value = '';
                txt_consignmentno_.disabled = false;
                return;
            }

            var tbl_cns = document.getElementById('tbl_cns');

            var tbody = tbl_cns.getElementsByTagName("tbody")[0];
            var count = 0;
            var row;
            var rowCount = 1;
            var flag = true;

            for (var i = 1; i < tbl_cns.rows.length; i++) {
                row = tbody.getElementsByTagName("tr")[i];
                var column = row.getElementsByTagName("td")[0];
                var RemarksColumn = row.getElementsByTagName("td")[7];
                var chk = row.getElementsByTagName("input");
                var txt_pieces = row.cells[6].getElementsByTagName("input");
                var hd_descStatus = column.childNodes[1];
                var hd_NewCN = RemarksColumn.childNodes[1];
                rowCount = i;

                var consignmentno = row.cells[1].innerText;
                if (consignmentno == txt_consignmentno) {
                    if (hd_NewCN.value == '0' && row.cells[2].innerText != 'RECEIVED') {
                        chk[0].checked = true;
                        row.cells[2].innerText = 'RECEIVED';
                        hd_descStatus.value = "5";
                        count++;
                        break;
                    }
                    else {
                        rowCount = i;
                        alert('Already Excess CN added.');
                        txt_consignmentno_.disabled = false;
                        txt_consignmentno_.value = '';
                        flag = false;
                        break;
                    }
                }

            }
            if (count > 0) {
                txt_consignmentno_.disabled = true;
                SaveUnloading_CN(row, 0);
            } else {
                if (flag) {
                    txt_consignmentno_.disabled = true;
                    getData_Consignment();
                }
                //   val.value = "Yes";
            }

        }

        function getData_Consignment() {
            var originName = "";
            var origin = "";
            var destinationName = "";
            var destination = "";
            var dd_orign = document.getElementById("<%=dd_orign.ClientID %>");
            var dd_destination = document.getElementById("<%=dd_destination.ClientID %>");

            originName = dd_orign.options[dd_orign.options.selectedIndex].text;
            origin = dd_orign.options[dd_orign.options.selectedIndex].value;

            destinationName = dd_destination.options[dd_destination.options.selectedIndex].text;
            destination = dd_destination.options[dd_destination.options.selectedIndex].value;

            var consignmentno = document.getElementById('txt_consignmentno').value;
            var Code = JSON.stringify({ 'consignmentno': consignmentno, 'origin': origin, 'originName': originName, 'destination': destination, 'destinationName': destinationName });
            $.ajax({
                url: 'Unloading_speedy.aspx/GetCNNo',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                data: Code,
                success: OnSuccess_CN,
                failure: function (response) {
                    alert(response.responseText);
                },
                error: function (response) {
                    alert(response.responseText);
                }
            });

        }

        function OnSuccess_CN(response) {

            var dt = JSON.parse(response.d);
            var count = dt.Table1.length;
            var tbl_cns = document.getElementById('tbl_cns');

            if (count - 1 > 0) {
                for (var i = 0; i < count - 1; i++) {
                    var newTr = tbl_cns.insertRow(i + 1);  //document.createElement('tr');  //consignments.rows[0].cloneNode(true);
                    newTr.className = 'DetailRow';


                    var chk_received = document.createElement('input');
                    chk_received.type = 'checkbox';
                    chk_received.style.marginTop = '2px';
                    chk_received.style.marginBottom = '2px';
                    chk_received.style.width = '80%';
                    chk_received.className = 'checkbox';
                    chk_received.disabled = true;
                    chk_received.checked = true;


                    var hd_descStatus = document.createElement('hd_descStatus');
                    hd_descStatus.type = 'hidden';
                    hd_descStatus.className = 'hidden';
                    hd_descStatus.value = '7';
                    hd_descStatus.style.textAlign = 'center';
                    hd_descStatus.id = 'hd_descStatus'

                    var hd_origin = document.createElement('hd_origin');
                    hd_origin.type = 'hidden';
                    hd_origin.className = 'hidden';
                    hd_origin.value = dt.Table1[i][2];
                    hd_origin.style.textAlign = 'center';
                    hd_origin.id = 'hd_origin'

                    var hd_destination = document.createElement('hd_destination');
                    hd_destination.type = 'hidden';
                    hd_destination.className = 'hidden';
                    hd_destination.value = dt.Table1[i][3];
                    hd_destination.style.textAlign = 'center';
                    hd_destination.id = 'hd_destination'

                    var hd_cnStatus = document.createElement('hd_cnStatus');
                    hd_cnStatus.type = 'hidden';
                    hd_cnStatus.className = 'hidden';
                    hd_cnStatus.value = 'INSERT';
                    hd_cnStatus.style.textAlign = 'center';
                    hd_cnStatus.id = 'hd_cnStatus'

                    var hd_NewCN = document.createElement('hidden');
                    hd_NewCN.type = 'hidden';
                    hd_NewCN.className = 'hidden';
                    hd_NewCN.value = '1';
                    hd_NewCN.style.textAlign = 'center';
                    hd_NewCN.id = 'hd_NewCN'

                    var hdDirtyFlag = document.createElement('input');
                    hdDirtyFlag.type = "text";
                    hdDirtyFlag.style.display = 'none';
                    hdDirtyFlag.value = "1";
                    hdDirtyFlag.className = "dirtyFlag";

                    var dd_origin = GetBranchDropDown();
                    dd_origin.style.width = '90%'
                    dd_origin.className = 'dropdown';
                    dd_origin.disabled = false;
                    dd_origin.value = dt.Table1[i][2];
                    dd_origin.style.textAlign = 'center';

                    var txt_gCnWeight = document.createElement('input');
                    txt_gCnWeight.type = 'text';
                    txt_gCnWeight.className = 'textBox';
                    txt_gCnWeight.value = dt.Table1[i][6];
                    txt_gCnWeight.style.width = '90%';
                    txt_gCnWeight.style.textAlign = 'center';

                    var txt_gCnPieces = document.createElement('input');
                    txt_gCnPieces.type = 'text';
                    txt_gCnPieces.className = 'textBox';
                    txt_gCnPieces.value = '1';
                    txt_gCnPieces.style.width = '90%';
                    txt_gCnPieces.style.textAlign = 'center';

                    var txt_gCnRemarks = document.createElement('input');
                    txt_gCnRemarks.type = 'text';
                    txt_gCnRemarks.className = 'textBox';
                    txt_gCnRemarks.value = '';
                    txt_gCnRemarks.style.width = '90%';
                    txt_gCnRemarks.style.textAlign = 'center';

                    var btn_remove = document.createElement('input');
                    btn_remove.type = 'button';
                    btn_remove.className = 'button button1';
                    btn_remove.value = 'Remove';
                    btn_remove.style.marginTop = '2px';
                    btn_remove.style.marginBottom = '2px';
                    btn_remove.style.width = '90%';
                    //   btn_remove.onclick = RemoveConsignment.bind(newTr.cells[0].childNodes[0], newTr.cells[0].childNodes[0]);

                    td = document.createElement('td');
                    td.className = 'gridcell';
                    td.style.fontWeight = 'normal';
                    td.rowSpan = 1;
                    td.style.width = '80px';

                    ///////////Recieved	//////////////
                    col = newTr.insertCell(0);
                    newTr.cells[0].appendChild(chk_received);
                    newTr.cells[0].appendChild(hd_descStatus);
                    newTr.cells[0].appendChild(hd_origin);
                    newTr.cells[0].appendChild(hd_destination);
                    newTr.cells[0].appendChild(hd_cnStatus);
                    newTr.cells[0].appendChild(hdDirtyFlag);


                    /////////// CN Number	//////////////
                    col = newTr.insertCell(1);
                    newTr.cells[1].innerText = dt.Table1[i][0];
                    ///////////Description	//////////////
                    col = newTr.insertCell(2);
                    newTr.cells[2].innerText = "Excess Received";
                    ///////////Origin////////////////////
                    col = newTr.insertCell(3);
                    newTr.cells[3].appendChild(dd_origin);
                    ///////////Destination////////////////////
                    col = newTr.insertCell(4);
                    newTr.cells[4].innerText = dt.Table1[i][5];
                    ///////////Weight////////////////////
                    col = newTr.insertCell(5);
                    newTr.cells[5].appendChild(txt_gCnWeight);
                    newTr.cells[5].style.textAlign = "center";
                    newTr.cells[5].childNodes[0].onchange = CalculateTotalWeight.bind(newTr.cells[5].childNodes[0]);
                    newTr.cells[5].childNodes[0].maxLength = "5";
                    ///////////Pieces.////////////////////
                    col = newTr.insertCell(6);
                    newTr.cells[6].appendChild(txt_gCnPieces);
                    newTr.cells[6].childNodes[0].onchange = validate.bind(newTr.cells[6].childNodes[0]);
                    newTr.cells[6].childNodes[0].maxLength = "4";

                    ///////////Remarks////////////////////
                    col = newTr.insertCell(7);
                    newTr.cells[7].appendChild(txt_gCnRemarks);
                    newTr.cells[7].appendChild(hd_NewCN);

                    ///////////Remove////////////////////
                    col = newTr.insertCell(8);
                    newTr.cells[8].appendChild(btn_remove);
                    newTr.cells[8].onclick = RemoveCN.bind(newTr.cells[8].childNodes[0], newTr.cells[8].childNodes[0]);

                    SaveUnloading_CN(newTr, "");
                    var txt_consignmentno = document.getElementById('txt_consignmentno');
                    txt_consignmentno.value = '';


                }
            }
            validate();
        }

        function SetValue(row, index, name, textbox) {
            //Reference the Cell and set the value.
            row.cells[index].innerHTML = textbox.value;

            //Create and add a Hidden Field to send value to server.
            var input = document.createElement("input");
            input.type = "hidden";
            input.name = name;
            input.value = textbox.value;
            row.cells[index].appendChild(input);

            //Clear the TextBox.
            textbox.value = "";
        }

        function CalculateTotalWeight() {


            debugger;
            var bags = document.getElementById('tbl_bagnew');
            var cns = document.getElementById('tbl_cns');
            var btn_save = document.getElementById("<%=btn_save.ClientID %>");
            var txt_consignmentno = document.getElementById('txt_consignmentno');
            var txt_bagno = document.getElementById('txt_Bagno');
            var hd_unloaded = document.getElementById('<%= hd_unloaded.ClientID %>');
            var chk = false;


            var validNumber = new RegExp(/^\d+(\.\d+)?$/);

            for (var i = 1; i < bags.rows.length; i++) {
                if (validNumber.test(bags.rows[i].cells[7].childNodes[0].value) && (bags.rows[i].cells[7].childNodes[0].value != "0")) {
                    bags.rows[i].cells[7].childNodes[0].style.backgroundColor = "";
                }
                else {
                    bags.rows[i].cells[7].childNodes[0].style.backgroundColor = "red";
                    chk = true;
                }
            }

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
                btn_save.disabled = true;
                txt_bagno.disabled = true;
                alert('Kindly insert proper weight');
                return;
            }
            else {
                if (hd_unloaded.value == '0') {
                    txt_consignmentno.disabled = false;
                    txt_bagno.disabled = false;
                    btn_save.disabled = false;
                }
                else if (hd_unloaded.value == '1') {
                    txt_consignmentno.disabled = true;
                    btn_save.disabled = true;
                    txt_bagno.disabled = true;
                }
                //validate();
            }



            var totalWeight = 0;
            var tempCnWeight = 0;
            var tempBagWeight = 0;

            for (var i = 1; i < bags.rows.length; i++) {
                var temp = parseFloat(bags.rows[i].cells[7].childNodes[0].value);
                if (!isNaN(temp)) {
                    tempBagWeight = tempBagWeight + temp;
                }
            }

            for (var i = 1; i < cns.rows.length; i++) {
                var temp = parseFloat(cns.rows[i].cells[5].childNodes[0].value);
                if (!isNaN(temp)) {
                    tempCnWeight = tempCnWeight + temp;
                }
            }

            document.getElementById('<%= txt_totalLoadWeight.ClientID %>').value = (tempCnWeight + tempBagWeight).toString();

        }

        function validate() {
            debugger;
            var bags = document.getElementById('tbl_bagnew');
            var cns = document.getElementById('tbl_cns');
            var btn_save = document.getElementById("<%=btn_save.ClientID %>");
            var txt_consignmentno = document.getElementById('txt_consignmentno');
            var txt_bagno = document.getElementById('txt_Bagno');
            var hd_unloaded = document.getElementById('<%= hd_unloaded.ClientID %>');
            var chk = false;
            var validNumber = new RegExp(/^[1-9][0-9]*$/);
            for (var i = 1; i < cns.rows.length; i++) {
                if (validNumber.test(cns.rows[i].cells[6].childNodes[0].value) && (cns.rows[i].cells[6].childNodes[0].value != "0")) {
                    cns.rows[i].cells[6].childNodes[0].style.backgroundColor = "";
                }
                else {
                    cns.rows[i].cells[6].childNodes[0].style.backgroundColor = "red";
                    chk = true;
                }
            }


            if (chk) {
                txt_consignmentno.disabled = true;
                btn_save.disabled = true;
                txt_bagno.disabled = true;
                alert('Kindly insert proper Piece');
                return;
            }
            else {
                if (hd_unloaded.value == '0') {
                    txt_consignmentno.disabled = false;
                    btn_save.disabled = false;
                    txt_bagno.disabled = false;
                }
                else if (hd_unloaded.value == '1') {
                    txt_consignmentno.disabled = true;
                    btn_save.disabled = true;
                    txt_bagno.disabled = true;
                    CalculateTotalWeight();
                }
            }
        }
    </script>
    <script type="text/javascript">
        function RemoveConsignment(btn) {
            var tr = btn.parentElement.parentElement;
            var table = tr.parentElement;
            table.deleteRow(tr.rowIndex);

        }
    </script>
    <%--Creating DropDowns for Origin and Destination--%>
    <script type="text/javascript">
        function GetBranchDropDown() {
            var sourceDropdown = document.getElementById('dd_Origin');

            var currdd = document.createElement('select');
            currdd.id = 'dd_gOrigin';
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
    <%--Populating Origin--%>
    <script type="text/javascript">

        window.onload = OnWindowLoad;
        function OnWindowLoad() {

            var dropdown = document.getElementById('dd_Origin');

            //   PopulateBranches();

        }
        function PopulateBranches() {
            var dropdown = document.getElementById('dd_Origin');
            $.ajax({
                url: 'Unloading_speedy.aspx/Cities',
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
                },
                error: function () {

                },
                failure: function () {

                }
            })
        }
    </script>
    <%--Saving Unloading--%>
    <script type="text/javascript">
        function onSave_btn_Click() {
            var obj = document.getElementById("<%=hd_tblBag.ClientID %>");
            obj.value = document.getElementById('tbl_bagnew');
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

            var txt_vid = document.getElementById("<%=txt_vid.ClientID %>");
            var dd_route = document.getElementById("<%=dd_route.ClientID %>");
            var txt_date = document.getElementById("<%=txt_date.ClientID %>");
            var dd_transporttype = document.getElementById("<%=dd_transporttype.ClientID %>");
            var dd_orign = document.getElementById("<%=dd_orign.ClientID %>");
            //  var txt_flight = document.getElementById("<%=txt_flight.ClientID %>");
            var dept_date = document.getElementById("<%=dept_date.ClientID %>");
            var Vehicle = document.getElementById("<%=Vehicle.ClientID %>");
            var Rented = document.getElementById("<%=Rented.ClientID %>");
            var dd_destination = document.getElementById("<%=dd_destination.ClientID %>");
            var dd_vehicle = document.getElementById("<%=dd_vehicle.ClientID %>");
            var txt_reg = document.getElementById("<%=txt_reg.ClientID %>");
            var txt_description = document.getElementById("<%=txt_description.ClientID %>");
            var txt_couriername = document.getElementById("<%=txt_couriername.ClientID %>");
            var txt_seal = document.getElementById("<%=txt_seal.ClientID %>");
            var txt_totalLoadWeight = document.getElementById("<%=txt_totalLoadWeight.ClientID %>");
            var divDialogue = document.getElementById("<%=divDialogue.ClientID %>");




            var selected_vehicle = "";
            if (!Rented.checked) {
                selected_vehicle = dd_vehicle.options[dd_vehicle.selectedIndex].value;
            }
            else {
                selected_vehicle = '103';
            }
            var selected_transporttype = dd_transporttype.options[dd_transporttype.selectedIndex].value;
            if (selected_transporttype == "") {
                alert('Select Transport Type');

            }
            var chk_user = false;

            var tbl_bagnew = document.getElementById("tbl_bagnew");
            for (var i = 1, row; row = tbl_bagnew.rows[i]; i++) {
                if (tbl_bagnew.rows[i].cells[2].innerHTML == "Short Received") {
                    // divDialogue.style.display = "block";
                    if (confirm('Some Bags/Out Pieces are short received. Click OK to Continue.?')) {
                        chk_user = true;
                        break;
                    } else {
                        chk_user = false;
                        var div1 = document.getElementById('<%=div2.ClientID%>');
                        var div2 = document.getElementById('<%=rd_div.ClientID%>');
                        if (div1.style.display == "" || div1.style.display == "block") {
                            div1.style.display = "none";
                            div2.style.display = "block";
                        }
                        return false;
                    }

                }
                else {
                    chk_user = true;
                }
            }



            var tbl_cns = document.getElementById("tbl_cns");
            for (var i = 1, row; row = tbl_cns.rows[i]; i++) {
                if (tbl_cns.rows[i].cells[2].innerHTML == "Short Received") {
                    if (confirm('Some Pieces are short received. Click OK to Continue.?')) {
                        chk_user = true;
                        break;
                    } else {
                        chk_user = false;
                        var div1 = document.getElementById('<%=div2.ClientID%>');
                        var div2 = document.getElementById('<%=rd_div.ClientID%>');
                        if (div1.style.display == "" || div1.style.display == "block") {
                            div1.style.display = "none";
                            div2.style.display = "block";
                        }
                        return false;
                    }
                }
            }

            if (chk_user) {
                btn_okDialogue_Click();
            }



        }
        function btn_cancelDialogue_Click() {
            var divDialogue = document.getElementById("<%=divDialogue.ClientID %>");
            divDialogue.style.display = "block";
        }
        function btn_okDialogue_Click() {
            var tblBags = document.getElementById('tbl_bagnew');
            var tblCn = document.getElementById('tbl_cns');
            var hdMaster = document.getElementById('<%= hd_master.ClientID %>');
            var txt_vid = document.getElementById('<%= txt_vid.ClientID %>');
            var unloadingID = "0";
            if (hdMaster.value == "0") {
                return;
            }
            else {
                unloadingID = hdMaster.value;
            }
            var jsonObj = { unloadingID: unloadingID, Bags: [], Consignments: [] }

            for (var i = 1; i < tblBags.rows.length; i++) {
                var tr = tbl_bagnew.rows[i];
                var Bag = {
                    BagNo: tr.cells[1].innerText,
                    SealNo: tr.cells[6].childNodes[0].value,
                    Weight: tr.cells[7].childNodes[0].value,
                    Remarks: tr.cells[8].childNodes[0].value,
                    Origin: tr.cells[11].childNodes[0].value,
                    Destination: tr.cells[13].childNodes[0].value,
                    Description: tr.cells[2].childNodes[0].data, //IN ENGLISH DESCRIPTION
                    hd_status: tr.cells[10].childNodes[0].value, //Status for Bag
                    chk_received: tr.cells[0].childNodes[0].checked //CheckBox for Bag
                };
                jsonObj.Bags.push(Bag);

            }

            for (var i = 1; i < tblCn.rows.length; i++) {
                var tr = tblCn.rows[i];
                var origin = "";
                var dd_origin = tr.cells[3].childNodes[0];
                origin = dd_origin.options[dd_origin.selectedIndex].value;
                var Consignment = {
                    ConNumber: tr.cells[1].innerText,
                    hd_origin: origin,
                    Weight: tr.cells[5].childNodes[0].value,
                    Pieces: tr.cells[6].childNodes[0].value,
                    Remarks: tr.cells[7].childNodes[0].value,
                    hd_origin: tr.cells[0].childNodes[2].value,
                    hd_destination: tr.cells[0].childNodes[3].value,
                    Description: tr.cells[2].childNodes[0].data, //IN ENGLISH DESCRIPTION
                    hd_cnStatus: tr.cells[0].childNodes[4].value, //EXIST OR NEW CN OR INSERT for CN
                    hd_descStatus: tr.cells[0].childNodes[1].value, //Status for CN
                    chk_received: tr.cells[0].childNodes[0].checked//CheckBox for CN
                };
                jsonObj.Consignments.push(Consignment);
            }
            $.ajax({
                url: 'Unloading_speedy.aspx/SaveChangedValues_saveButton',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                data: JSON.stringify(jsonObj),
                success: function (response) {
                    var resp = response.d;
                    var response = resp.split(";");
                    if (response[0] == "Unload Successful.") {
                        txt_vid.disabled = false;
                        alert(resp);
                        window.location = "print_unloading.aspx?Xcode=" + response[1];
                        Reset();

                    }
                    else {
                        alert(resp);
                    }
                }
            });
        }

    </script>
    <script type="text/javascript">
        function Reset() {
            var tbl_bagnew = document.getElementById('tbl_bagnew');
            var i = 1;
            while (tbl_bagnew.rows.length > 0) {
                tbl_bagnew.deleteRow(i);
                i++;
            }
            var tbl_cns = document.getElementById('tbl_cns');
            i = 1;
            while (tbl_cns.rows.length > 0) {
                tbl_cns.deleteRow(i);
                i++;
            }
        }
    </script>
    <%--Hide Div--%>
    <script type="text/javascript">
        function RB_Click(rbtn) {

            var tab_1_Div = document.getElementById('<%=tab_1_Div.ClientID%>');
            var tab_2_Div = document.getElementById('<%=tab_2_Div.ClientID%>');
            //if (rbtn.id == "ContentPlaceHolder2_tab_1") {
            if (rbtn.value == "1") {
                tab_1_Div.style.display = "block";
                tab_2_Div.style.display = "none";
            }
            if (rbtn.value == "2") {
                tab_2_Div.style.display = "block";
                tab_1_Div.style.display = "none";
            }
        }

    </script>
    <style>
        .outer_box {
            background: #444 none repeat scroll 0 0;
            height: 170%;
            left: 0;
            opacity: 0.9;
            position: absolute;
            top: 5%;
            width: 100%;
        }


        .pop_div {
            background: #eee none repeat scroll 0 0;
            border-radius: 10px;
            height: 100px;
            left: 48%;
            position: relative;
            top: 40%;
            width: 235px;
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
            /* position: static;*/
        }

        .content {
            /*  position: relative;*/
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
            left: 20px; /*  position: relative;*/
            width: 70%;
        }

            .tab_radio > span {
                background: #eee none repeat scroll 0 0;
                border: 1px solid #ccc;
                left: 1px;
                margin-left: -1px;
                padding: 10px; /*  position: relative;*/
            }

        [type=radio]:checked {
            background: white;
            border-bottom: 1px solid white;
            z-index: 2;
        }

        .tabs {
            left: 20px;
            margin: 0 0 40px;
            padding: 0; /* position: relative;*/
            width: 97%;
        }

        .input-form.boxbg {
            background: #eee none repeat scroll 0 0;
            margin: 0;
            width: 100%;
        }

        .outer_box img {
            left: 42%;
            position: relative;
            top: 40%;
        }

        .auto-style1 {
            float: left;
            width: 25%;
            height: 26px;
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

        .auto-style2 {
            float: left;
            margin: 0 40px;
            height: 26px;
        }
    </style>

    <div id="div2" runat="server" class="outer_box" style="display: none; background: #FFFFFF none repeat scroll 0 0;">
        <img src="../images/Loading_Movie-02.gif" />
    </div>
    <div id="divDialogue" runat="server" class="outer_box" style="display: none;">
        <div class="pop_div">
            <table style="width: 100% !important;">
                <tr width="100%">
                    <td style="float: left; margin-top: 12px; text-align: center; width: 228px;">
                        <asp:Label ID="lbl_error" runat="server" Text="Some Bags/Out Pieces are short received. Click OK to Continue."></asp:Label>
                    </td>
                </tr>
                <tr width="100%">
                    <td style="float: left; margin-left: 30px; margin-top: 8px; text-align: center !important;"></td>
                    <td style="float: left; margin-top: 8px; text-align: center !important; width: 50% !important;"></td>
                </tr>
            </table>
        </div>
    </div>
    <asp:HiddenField ID="hd_unloaded" runat="server" Value="0" />
    <div style="width: 100%; text-align: center; font-variant: small-caps;">
        <h2>Manage Unloading</h2>
    </div>
    <table class="input-form" style="width: 95%;">
        <tr>
            <td colspan="5">
                <a href="search_unloading.aspx" target="_blank" class="button">Search UnLoading</a>
            </td>
        </tr>
        <tr>
            <td class="field" style="height: 26px">VId:
            </td>
            <td class="auto-style1">
                <asp:TextBox ID="txt_vid" runat="server" CssClass="med-field" MaxLength="15" CausesValidation="true" OnTextChanged="txt_loadingID_TextChanged"
                    AutoPostBack="true"></asp:TextBox>
            </td>
            <td class="field">
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1"
                    ControlToValidate="txt_vid"
                    ValidationExpression="\d+"
                    Display="Static"
                    EnableClientScript="true"
                    ForeColor="Red"
                    ErrorMessage="Please enter Proper Loading ID"
                    runat="server" />
            </td>
            <td class="field" style="height: 26px"></td>
            <td class="auto-style1"></td>
        </tr>
        <tr>
            <td class="field">Route:
            </td>
            <td class="input-field">
                <asp:DropDownList ID="dd_route" runat="server" CssClass="dropdown" AppendDataBoundItems="true"
                    AutoPostBack="true" OnSelectedIndexChanged="Route_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
            <td class="space"></td>
            <td class="field">Date:
            </td>
            <td class="input-field">
                <asp:TextBox ID="txt_date" runat="server" CssClass="med-field" MaxLength="10" disabled="true"></asp:TextBox>
                <Ajax1:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_date" runat="server"
                    Format="yyyy-MM-dd" PopupButtonID="Image1"></Ajax1:CalendarExtender>
            </td>
        </tr>
        <tr>
            <td class="field">Transport type:
            </td>
            <td class="input-field">
                <asp:DropDownList ID="dd_transporttype" runat="server" CssClass="dropdown" AutoPostBack="true"
                    OnSelectedIndexChanged="dd_transporttype_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
            <td class="space"></td>
            <td class="field">Orign:
            </td>
            <td class="input-field">
                <asp:DropDownList ID="dd_orign" runat="server" CssClass="dropdown">
                </asp:DropDownList>
            </td>
        </tr>
        <tr id="flight" runat="server">
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
                <Ajax1:MaskedEditExtender ID="MaskedEditExtender1" TargetControlID="dept_date" Mask="99:99"
                    ClearMaskOnLostFocus="false" MaskType="Time" CultureName="en-us" MessageValidatorTip="true"
                    runat="server"></Ajax1:MaskedEditExtender>
            </td>
        </tr>
        <tr>
            <td class="field">Vehicle Type:
            </td>
            <td class="input-field">
                <div>
                    <asp:RadioButton ID="Vehicle" AutoPostBack="true" GroupName="VehicleType" runat="server"
                        Checked="true" />Vehicle
                    <asp:RadioButton ID="Rented" AutoPostBack="true" GroupName="VehicleType" runat="server" />Rented
                </div>
            </td>
            <td class="space"></td>
            <td class="field">Destination:
            </td>
            <td class="input-field">
                <asp:DropDownList ID="dd_destination" runat="server" CssClass="dropdown" Enabled="false">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="field">
                <div id="vehiclediv1" runat="server">
                    Vehicle No:
                </div>
                <div id="regdiv1" runat="server">
                    Reg No:
                </div>
            </td>
            <td class="input-field">
                <div id="vehiclediv2" runat="server">
                    <asp:DropDownList ID="dd_vehicle" runat="server" CssClass="dropdown">
                    </asp:DropDownList>
                </div>
                <div id="regdiv2" runat="server">
                    <asp:TextBox ID="txt_reg" runat="server"></asp:TextBox>
                </div>
            </td>
            <td class="space"></td>
            <td class="field">Description:
            </td>
            <td class="input-field">
                <asp:TextBox ID="txt_description" runat="server" Columns="8"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="field">Courier Name:
            </td>
            <td class="input-field">
                <asp:TextBox ID="txt_couriername" runat="server"></asp:TextBox>
            </td>
            <td class="space"></td>
            <td class="field">Loading Seal Number:
            </td>
            <td class="input-field">
                <asp:TextBox ID="txt_seal" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="field">Total Weight:
            </td>
            <td class="input-field">
                <asp:TextBox ID="txt_totalLoadWeight" runat="server"></asp:TextBox>
            </td>
            <td class="space">
                <asp:HiddenField ID="hd_master" runat="server" Value="0" />
            </td>
            <td class="field"></td>
            <td class="input-field"></td>
        </tr>
    </table>
    <div id="rd_div" runat="server">
        <div class="tabs">
            <div class="tab">
                <div class="tab_radio">
                    <span>
                        <input id="tab_1" type="radio" runat="server" name="tab_group_1" value="1" onchange="RB_Click(this)"
                            checked="true" />
                        Bag </span><span>
                            <input id="tab_2" type="radio" runat="server" name="tab_group_1" value="2" onchange="RB_Click(this)" />
                            Out Pieces </span>
                </div>
                <div class="content" id="tab_1_Div" runat="server" style="overflow: scroll;">
                    <table class="input-form boxbg">
                        <tr id="bag_div" runat="server">
                            <td class="field" style="height: 26px">Bag No:
                            </td>
                            <td class="auto-style1">
                                <asp:HiddenField ID="hdValue" runat="server" Value="2" />
                                <input type="text" maxlength="12" id="txt_Bagno" onkeypress="return isNumberKey(event);"
                                    onchange="chkManChanged();" disabled="disabled" />
                            </td>
                            <td>
                                <input type="text" id="receivedBags" disabled="disabled" style="border: 0px;" value="" />
                            </td>
                            <td>
                                <input type="text" id="excessBags" disabled="disabled" style="border: 0px;" value="" />
                            </td>
                            <td>
                                <input type="text" id="shortBags" disabled="disabled" style="border: 0px;" value="" />
                            </td>
                            <td style="background-color: #8cf78c">Last Scanned Item
                            </td>
                        </tr>
                    </table>
                    <asp:Label ID="lbl_new" runat="server" CssClass="error_msg"></asp:Label>
                    <div style="width: 100%; overflow: scroll; height: 250px;">
                        <table id="tbl_bagnew" class="tblDetails">
                            <tr>
                                <th style="width: 100px;">Recieved
                                </th>
                                <th style="width: 90px;">Bag No
                                </th>
                                <th style="width: 110px;">Description
                                </th>
                                <th style="width: 90px;">Origin
                                </th>
                                <th style="width: 90px;">Destination
                                </th>
                                <th style="width: 70px;">Status
                                </th>
                                <th style="width: 100px;">Seal No
                                </th>
                                <th style="width: 70px;">Weight
                                </th>
                                <th style="width: 150px;">Remarks
                                </th>
                                <th style="width: 70px;">Remove
                                </th>
                            </tr>
                        </table>
                    </div>
                    <span id="Table_1" class="">
                        <asp:Label ID="err_msg" runat="server" CssClass="error_msg"></asp:Label>
                        <asp:Literal ID="lt_gv_bagnew" runat="server" />
                    </span>
                </div>
            </div>
            <div class="tab">
                <div class="content" id="tab_2_Div" runat="server" style="overflow: scroll;">
                    <table class="input-form boxbg">
                        <tr>
                            <td class="field">Consignment No:
                            </td>
                            <td class="auto-style1">
                                <input type="text" id="txt_consignmentno" maxlength="15" onkeypress="return isNumberKey(event);"
                                    onchange="if ( checkValidations(this) == false ) return;" disabled="disabled" />
                            </td>
                            <td>
                                <input type="text" id="receivedCN" disabled="disabled" style="border: 0px;" value="" />
                            </td>
                            <td>
                                <input type="text" id="excessCN" disabled="disabled" style="border: 0px;" value="" />
                            </td>
                            <td>
                                <input type="text" id="shortCN" disabled="disabled" style="border: 0px;" value="" />
                            </td>
                            <td style="background-color: #8cf78c">Last Scanned Item
                            </td>
                        </tr>
                    </table>
                    <div style="width: 100%; overflow: scroll; height: 250px;">
                        <table id="tbl_cns" class="tblCN">
                            <tr>
                                <th style="width: 5%; text-align: center;">Recieved
                                </th>
                                <th style="width: 10%;">CN Number
                                </th>
                                <th style="width: 10%;">Description
                                </th>
                                <th style="width: 15%;">Origin
                                </th>
                                <th style="width: 5%;">Destination
                                </th>
                                <th style="width: 5%;">Weight
                                </th>
                                <th style="width: 15%;">Pieces
                                </th>
                                <th style="width: 5%;">Remarks
                                </th>
                                <th style="width: 8%;">Remove
                                </th>
                            </tr>
                        </table>
                    </div>
                    <span id="Span3" class="">
                        <asp:Literal ID="lt_gv_cns" runat="server" />
                    </span>
                </div>
            </div>
        </div>
        <div style="display: none;">
            <asp:GridView ID="cnControls" runat="server">
                <Columns>
                    <asp:BoundField HeaderText="Prefix" DataField="Prefix" />
                    <asp:BoundField HeaderText="Length" DataField="Length" />
                </Columns>
            </asp:GridView>
        </div>
        <div style="width: 100%; text-align: center">
            <asp:Button ID="Button1" runat="server" Text="Reset" CssClass="button" OnClick="btn_reset_Click"
                UseSubmitBehavior="false" OnClientClick="Reset();" />
            &nbsp;
            <asp:Button ID="btn_save" runat="server" Text="Save" CssClass="button" UseSubmitBehavior="false"
                OnClientClick="return  onSave_btn_Click();" CommandName="first" />
            &nbsp;
            <asp:Button ID="btn_cancel" runat="server" Text="Cancel" CssClass="button" UseSubmitBehavior="false"
                OnClick="btn_cancel_Click" />
            <asp:HiddenField ID="hd_tblBag" runat="server" />
        </div>
    </div>
    <select id="dd_Origin" style="display: none;">
        <option value="0">.:Select Origin:.</option>
    </select>
</asp:Content>