<%@ Page Title="" Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.master" CodeBehind="Demanifest_speedy.aspx.cs" Inherits="MRaabta.Files.Demanifest_speedy" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script src="../Scripts/jquery-3.5.1.min.js"></script>
    <script language="javascript" type="text/javascript">
        var count = 0;
        function isNumberKey(evt) {

            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode != 46 && charCode > 31
            && (charCode < 48 || charCode > 57 || charCode == 110 || charCode == 109 || charCode == 45 || charCode == 9)) {
                return false;
            }
            else {

                return true;
            }


        }




        function PageRedirect() {
            window.open('Search_Demanifest.aspx', '_blank');
        }
    </script>
    <%--working scripts--%>
    <%--Manifest Number Change--%>
    <script type="text/javascript">
        function GetManifestDetails(txt) {
            var manifestNumber = txt.value.trim();
            ResetAll();
            txt.value = manifestNumber;
            if (manifestNumber == "") {
                alert('Enter Manifest Number');
                return;
            }
            $.ajax({
                url: 'Demanifest_speedy.aspx/GetManifestDetails',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                datatype: 'json',
                data: "{'ManifestNumber':'" + manifestNumber.trim() + "'}",
                success: function (result) {
                    var resp = result.d;

                    var date = document.getElementById('<%= txt_date.ClientID %>');
                    var origin = document.getElementById('<%= txt_origin.ClientID %>');
                    var destination = document.getElementById('<%= txt_destination.ClientID %>');
                    var originCode = document.getElementById('<%= hd_originCode.ClientID %>');
                    var destinationCode = document.getElementById('<%= hd_destinationCode.ClientID %>');
                    var manifestType = document.getElementById('<%= hd_manifestType.ClientID %>');
                    var isDemanifested = document.getElementById('<%= hd_isDemanifested.ClientID %>');
                    var table = document.getElementById('tbl_details');

                    if (resp.ServerResponse != "") {
                        alert(resp.ServerResponse);
                        date.value = "";
                        origin.value = "";
                        destination.value = "";
                        originCode.value = "";
                        destinationCode.value = "";
                        txt.value = "";
                        manifestType.value = "";
                        isDemanifested.value = "";
                        return;
                    }

                    date.value = resp.Master.Date;
                    origin.value = resp.Master.Origin;
                    destination.value = resp.Master.Destination;
                    originCode.value = resp.Master.OriginCode;
                    destinationCode.value = resp.Master.DestinationCode;
                    manifestType.value = resp.Master.Type;
                    isDemanifested.value = resp.Master.IsDemanifested;
                    var rbtnMan = document.getElementById('rbtn_manifest');
                    var rbtnWoMan = document.getElementById('rbtn_woManifest');

                    if (resp.Master.WoManifest == "1") {
                        if (confirm('Manifest Does not Exist. Do you want to continue without manifest?')) {
                            tdManifest.style.display = 'none';
                            tdWoManifest.style.display = 'block';
                            rbtnWoMan.checked = true;

                        }
                        else {
                            tdManifest.style.display = 'block';
                            tdWoManifest.style.display = 'none';
                            rbtnMan.checked = true;
                        }
                    }
                    else {
                        tdManifest.style.display = 'block';
                        tdWoManifest.style.display = 'none';
                        rbtnMan.checked = true;
                    }


                    for (var i = 0; i < resp.Details.length; i++) {
                        var cn = resp.Details[i];
                        var tr = table.insertRow(1);
                        tr.className = "DetailRow";

                        var chk = document.createElement('input');
                        chk.className = "chktbl";
                        chk.type = "checkbox";
                        chk.disabled = true;

                        var reason = document.createElement('input');
                        reason.className = 'textBox';
                        reason.value = cn.Reason;
                        reason.style.width = "97%";

                        var hdcOriginCode = document.createElement('input');
                        hdcOriginCode.type = 'hidden';
                        hdcOriginCode.className = "hdcOriginCode";
                        hdcOriginCode.value = cn.OriginCode;

                        var hdcDestinationCode = document.createElement('input');
                        hdcDestinationCode.type = 'hidden';
                        hdcDestinationCode.className = "hdcDestinationCode";
                        hdcDestinationCode.value = cn.DestinationCode;

                        var hdcDemanifestStateID = document.createElement('input');
                        hdcDemanifestStateID.type = 'hidden';
                        hdcDemanifestStateID.className = "hdcDemanifestStateID";
                        hdcDemanifestStateID.value = cn.DemanifestStateID;

                        var dd_status = GetRecStatusDropDown();
                        dd_status.onchange = ChangeRecStatus.bind(dd_status, dd_status);
                        if (rbtnMan.checked) {
                            dd_status.disabled = true;
                        }
                        else if (rbtnWoMan.checked) {
                            dd_status.disabled = false;
                        }
                        for (var j = 0; j < dd_status.options.length; j++) {
                            if (dd_status.options[j].value == hdcDemanifestStateID.value) {
                                dd_status.options[j].selected = true;
                                break;
                            }
                        }
                        if (hdcDemanifestStateID.value == "7" || hdcDemanifestStateID.value == "5") {
                            chk.checked = true;

                        }

                        var td = tr.insertCell(0);
                        td.appendChild(chk);
                        td.appendChild(hdcOriginCode);
                        td.appendChild(hdcDestinationCode);
                        td.appendChild(hdcDemanifestStateID);

                        td = tr.insertCell(1);
                        td.innerHTML = cn.ConsignmentNumber;

                        td = tr.insertCell(2);
                        td.appendChild(dd_status);
                        //td.innerHTML = cn.status;

                        td = tr.insertCell(3);
                        td.appendChild(reason);

                        td = tr.insertCell(4);
                        td.innerHTML = cn.Origin;

                        td = tr.insertCell(5);
                        td.innerHTML = cn.Destination;

                        td = tr.insertCell(6);
                        td.innerHTML = cn.ConsignmentType;

                        td = tr.insertCell(7);
                        td.innerHTML = cn.ServiceType;

                        td = tr.insertCell(8);
                        td.innerHTML = cn.Weight;
                    }
                    CalculateTotalRows();
                    if (isDemanifested.value.toUpperCase() == "1" || isDemanifested.value.toUpperCase() == "TRUE") {
                        alert('Manifest Already Demanifested.');

                        return;
                    }
                    else {

                    }
                },
                error: function (result) { alert(result.statusText); },
                failure: function (result) { }
            });

        }
    </script>
    <%--Consignment Number Change--%>
    <script type="text/javascript">
        function CheckConsignment(txt) {
            var rbtnMan = document.getElementById('rbtn_manifest');
            var rbtnWoMan = document.getElementById('rbtn_woManifest');
            var dd_origin = document.getElementById('<%= dd_origin.ClientID %>');

            var consignmentNumber = txt.value.trim();
            if (consignmentNumber == "") {
                alert('Enter Proper Consignment Number');
                WorkingFocus(txt);
                return;
            }

            if (rbtnWoMan.checked && dd_origin.options.selectedIndex == 0) {
                alert('Select Origin');
                txt.value = "";
                WorkingFocus(txt);
                return;
            }


            var table = document.getElementById('tbl_details');
            var found = false;
            for (var i = 1; i < table.rows.length; i++) {
                var tr = table.rows[i];
                var chk = tr.getElementsByClassName('chktbl')[0];
                var hdcDemanifestStateID = tr.getElementsByClassName('hdcDemanifestStateID')[0];
                if (consignmentNumber == tr.cells[1].innerHTML) {

                    chk.checked = true;
                    hdcDemanifestStateID.value = "5";
                    var dd_status = tr.cells[2].childNodes[0];
                    for (var j = 0; j < dd_status.options.length; j++) {
                        if (dd_status.options[j].value == hdcDemanifestStateID.value) {
                            dd_status.options.selectedIndex = j;
                        }
                    }

                    found = true;
                    break;
                }
            }
            if (found) {
                txt.value = "";
                WorkingFocus(txt);
            }
            else {
                //Excess Received ka scene karna hai idhar


                var message = "";
                var controlGrid = document.getElementById('<%= cnControls.ClientID %>');
                var prefixNotFound = false;
                for (var i = 1; i < controlGrid.rows.length; i++) {
                    var row = controlGrid.rows[i];
                    var prefix = row.cells[0].innerText;
                    var length_ = parseInt(row.cells[1].innerText);
                    if (prefix == "52190") {
                        var a = 0;
                    }
                    if (txt.value.substring(0, prefix.length) == prefix) {
                        if (txt.value.length != length_) {
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
                    txt.value = "";
                    WorkingFocus(txt);
                    return false;
                }



                var originCode = document.getElementById('<%= hd_originCode.ClientID %>');
                var destinationCode = document.getElementById('<%= hd_destinationCode.ClientID %>');
                var manifestType = document.getElementById('<%= hd_manifestType.ClientID %>');
                var origin = document.getElementById('<%= txt_origin.ClientID %>');
                var destination = document.getElementById('<%= txt_destination.ClientID %>');


                var tr = table.insertRow(1);
                tr.className = "DetailRow";

                var chk = document.createElement('input');
                chk.className = "chktbl";
                chk.type = "checkbox";
                chk.disabled = true;
                chk.checked = true;

                var reason = document.createElement('input');
                reason.className = 'textBox';
                reason.value = "";
                reason.style.width = "97%";

                var hdcOriginCode = document.createElement('input');
                hdcOriginCode.type = 'hidden';
                hdcOriginCode.className = "hdcOriginCode";
                hdcOriginCode.value = originCode.value;

                var hdcDestinationCode = document.createElement('input');
                hdcDestinationCode.type = 'hidden';
                hdcDestinationCode.className = "hdcDestinationCode";
                hdcDestinationCode.value = destinationCode.value;

                var hdcDemanifestStateID = document.createElement('input');
                hdcDemanifestStateID.type = 'hidden';
                hdcDemanifestStateID.className = "hdcDemanifestStateID";
                hdcDemanifestStateID.value = "7";

                var dd_status = GetRecStatusDropDown();
                dd_status.onchange = ChangeRecStatus.bind(dd_status, dd_status);

                for (var j = 0; j < dd_status.options.length; j++) {
                    if (dd_status.options[j].value == "7") {
                        dd_status.options.selectedIndex = j;
                    }
                }

                if (rbtnMan.checked) {
                    dd_status.disabled = true;
                }
                else if (rbtnWoMan.checked) {
                    dd_status.disabled = false;
                    dd_status.options.selectedIndex = 0;
                    chk.checked = false;
                }
                var td = tr.insertCell(0);
                td.appendChild(chk);
                td.appendChild(hdcOriginCode);
                td.appendChild(hdcDestinationCode);
                td.appendChild(hdcDemanifestStateID);

                td = tr.insertCell(1);
                td.innerHTML = txt.value.trim();

                td = tr.insertCell(2);
                td.appendChild(dd_status);
                //td.innerHTML = "EXCESS RECEIVED";

                td = tr.insertCell(3);
                td.appendChild(reason);

                td = tr.insertCell(4);
                td.innerHTML = origin.value.split('-')[0];

                td = tr.insertCell(5);
                td.innerHTML = destination.value.split('-')[0];

                td = tr.insertCell(6);
                td.innerHTML = "NORMAL";

                td = tr.insertCell(7);
                td.innerHTML = manifestType.value;

                td = tr.insertCell(8);
                td.innerHTML = "0.5";

                txt.value = "";
                WorkingFocus(txt);
            }
            CalculateTotalRows();
        }
    </script>
    <%--Calculating Number of Rows--%>
    <script type="text/javascript">
        function CalculateTotalRows() {
            var lblCount = document.getElementById('lblCount');
            var tbl_details = document.getElementById('tbl_details');
            lblCount.value = "CN Count: " + (tbl_details.rows.length - 1);
        }
    </script>
    <%--FOCUS--%>
    <script type="text/javascript">
        function WorkingFocus(cnt) {
            var id = '#' + cnt.id.toString();
            $(document).ready(function () {
                setTimeout(function () { $(id).focus(); }, 1);
            });
        }
    </script>
    <%--Saving Demanifest--%>
    <script type="text/javascript">
        function SaveDemanifest() {
            var isDemanifested = document.getElementById('<%= hd_isDemanifested.ClientID %>');
            if (isDemanifested.value.toUpperCase() == "1" || isDemanifested.value.toUpperCase() == "TRUE" || isDemanifested.value.toUpperCase() == "YES" || isDemanifested.value.toUpperCase() == "Y") {
                alert('Manifest Already Demanifested.');
                return;
            }
            var manifest = document.getElementById('<%= txt_manifestNumber.ClientID %>');
            var table = document.getElementById('tbl_details');

            if (manifest.value.trim() == "") {
                alert("Invalid Manifest Number");
                return;
            }

            if (table.rows.length == 1) {
                alert("No Details to Demanifest");
                return;
            }
            var rbtnMan = document.getElementById('rbtn_manifest');
            var rbtnWoMan = document.getElementById('rbtn_woManifest');
            var WoManifest = "";
            if (rbtnMan.checked) {
                WoManifest = "0";
            }
            else if (rbtnWoMan.checked) {
                WoManifest = "1";
            }
            var originCode = document.getElementById('<%= hd_originCode.ClientID %>');
            var destinationCode = document.getElementById('<%= hd_destinationCode.ClientID %>');
            var manifestType = document.getElementById('<%= hd_manifestType.ClientID %>');
            var isDemanifested = document.getElementById('<%= hd_isDemanifested.ClientID %>');
            var manifestDate = document.getElementById('<%= txt_date.ClientID %>');



            if (isDemanifested.value == "1" || isDemanifested.value.toUpperCase() == "TRUE") {
                alert('Manifest Already Demanifested');
                return;
            }

            var jsonObj = { Master: {}, Consignments: [] }
            var Master = {
                Manifest: manifest.value,
                Type: manifestType.value,
                Date: manifestDate.value,
                Origin: "",
                OriginCode: originCode.value,
                Destination: "",
                DestinationCode: destinationCode.value,
                IsDemanifested: "",
                WoManifest: WoManifest
            }
            jsonObj.Master = Master;

            var srFound = false;
            for (var i = 1; i < table.rows.length; i++) {
                var tr = table.rows[i];
                var reason = tr.cells[3].childNodes[0];
                var hdcOriginCode = tr.getElementsByClassName('hdcOriginCode')[0];
                var hdcDestinationCode = tr.getElementsByClassName('hdcDestinationCode')[0];
                var chk = tr.getElementsByClassName('chktbl')[0];
                var dd_status = tr.cells[2].childNodes[0];
                var recStatus = "";
                if (!chk.checked) {
                    srFound = true;
                    for (var j = 0; j < dd_status.options.length; j++) {
                        if (dd_status.options[j].value == "6") {
                            dd_status.options.selectedIndex = j;
                            break;
                        }
                    }
                }
                recStatus = dd_status.options[dd_status.options.selectedIndex].value;
                var Consignment;

                Consignment = {
                    ConsignmentNumber: tr.cells[1].innerHTML,
                    status: recStatus,
                    Reason: reason.value,
                    Origin: tr.cells[4].innerHTML,
                    OriginCode: hdcOriginCode.value,
                    Destination: tr.cells[5].innerHTML,
                    DestinationCode: hdcDestinationCode.value,
                    ConsignmentType: "",
                    ServiceType: tr.cells[7].innerHTML,
                    Weight: tr.cells[8].innerHTML
                }

                jsonObj.Consignments.push(Consignment);
            }
            var proceed = true;
            if (srFound) {
                proceed = confirm('Some Consignments are Short Received. Do you want to Continue');
            }

            if (!proceed) {
                return;
            }

            $.ajax({

                url: 'Demanifest_speedy.aspx/SaveDemanifest',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                data: JSON.stringify(jsonObj),
                success: function (result) {
                    var resp = result.d;
                    if (resp != "OK") {
                        alert(resp);
                    }
                    else {
                        alert("Demanifest Successful");
                        ResetAll();
                    }
                },
                error: function (result) { },
                failure: function (result) { }
            });

        }
    </script>
    <%--Resetting--%>
    <script type="text/javascript">
        function ResetAll() {

            var originCode = document.getElementById('<%= hd_originCode.ClientID %>');
            var destinationCode = document.getElementById('<%= hd_destinationCode.ClientID %>');
            var manifestType = document.getElementById('<%= hd_manifestType.ClientID %>');
            var isDemanifested = document.getElementById('<%= hd_isDemanifested.ClientID %>');
            var manifestNumber = document.getElementById('<%= txt_manifestNumber.ClientID %>');
            var date = document.getElementById('<%= txt_date.ClientID %>');
            var origin = document.getElementById('<%= txt_origin.ClientID %>');
            var destination = document.getElementById('<%= txt_destination.ClientID %>');
            var lblCount = document.getElementById('lblCount');
            var table = document.getElementById('tbl_details');
            var rbtn_woManifest = document.getElementById('rbtn_woManifest');
            var rbtn_manifest = document.getElementById('rbtn_manifest');


            originCode.value = "";
            destinationCode.value = "";
            manifestType.value = "";
            isDemanifested.value = "";
            manifestNumber.value = "";
            date.value = "";
            origin.value = "";
            destination.value = "";
            lblCount.value = "";
            ClearTable(table);

            tdManifest.style.display = 'block';
            tdWoManifest.style.display = 'none';
            rbtn_manifest.checked = true;
        }
        function ClearTable(tbl) {
            for (var i = 1; i < tbl.rows.length; ) {
                tbl.deleteRow(1);
            }
        }
    </script>
    <%--Generating Receiving Status Dropdown--%>
    <script type="text/javascript">
        function GetRecStatusDropDown() {
            var sourceDropdown = document.getElementById('recStatus');

            var currdd = document.createElement('select');
            currdd.id = 'dd_status';
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
    <%--Populating Receiving Status--%>
    <script type="text/javascript">
        window.onload = OnWindowLoad;
        function OnWindowLoad() {
            var dropdown = document.getElementById('recStatus');

            PopulateBranches();

        }
        function PopulateBranches() {
            var dropdown = document.getElementById('recStatus');
            $.ajax({
                url: 'Demanifest_Speedy.aspx/GetRecStatus',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                datatype: 'json',
                data: '',
                success: function (result) {
                    var a = '';
                    for (var i = 0; i < result.d.length; i++) {
                        var option = document.createElement('option');
                        option.text = result.d[i].Name;
                        option.value = result.d[i].Code;

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
    <%--Changing Destination--%>
    <script type="text/javascript">
        function ChangeOrigin(dd) {


            var hdOrigin = document.getElementById('<%= hd_originCode.ClientID %>');
            var txtOrigin = document.getElementById('<%= txt_origin.ClientID %>');

            txtOrigin.value = dd.options[dd.options.selectedIndex].text;
            hdOrigin.value = dd.options[dd.options.selectedIndex].value;
        }    
    </script>
    <%--Changing Receving Status--%>
    <script type="text/javascript">
        function ChangeRecStatus(dd) {
            var a = 0;
            var tr = dd.parentNode.parentNode;
            var chk = tr.getElementsByClassName('chktbl')[0];
            var hdcDemanifestStateID = tr.getElementsByClassName('hdcDemanifestStateID')[0];

            if (dd.options[dd.options.selectedIndex].value == "5" || dd.options[dd.options.selectedIndex].value == "7" || dd.options[dd.options.selectedIndex].value == "8" || dd.options[dd.options.selectedIndex].value == "9") {
                chk.checked = true;
            }
            else {
                chk.checked = false;
            }

            hdcDemanifestStateID.value = dd.options[dd.options.selectedIndex].value;
        }
    </script>
    <style>
        .outer_box
        {
            background: #444 none repeat scroll 0 0;
            height: 101%;
            left: 0;
            opacity: 0.9;
            position: absolute;
            top: -1%;
            width: 100%;
        }
        
        
        .pop_div
        {
            background: #eee none repeat scroll 0 0;
            border-radius: 10px;
            height: 100px;
            left: 48%;
            position: relative;
            top: 40%;
            width: 235px;
        }
        
        .btn_ok
        {
            background: #000 none repeat scroll 0 0;
            border: 0 none;
            color: #fff;
            left: -18px;
            padding: 1px 14px;
            position: relative;
            top: 67%;
        }
        
        .btn_cancel
        {
            background: #000 none repeat scroll 0 0;
            border: 0 none;
            color: #fff;
            left: 22%;
            padding: 1px 14px;
            position: relative;
            top: 42%;
        }
        
        .pop_div > span
        {
            float: left;
            line-height: 40px;
            text-align: center;
            width: 100%;
        }
        .tbl-large div
        {
            position: static;
        }
    </style>
    <style>
        .headerRow td
        {
            font-family: Calibri;
            font-size: small;
            font-weight: bold;
            background-color: #cccccc;
        }
        .DetailRow td
        {
            font-family: Calibri;
            font-size: small;
            font-weight: normal;
        }
        
        .tblDetails
        {
            font-family: calibri;
            font-size: small;
            border: 1px solid black;
            width: 90%;
            margin-left: 5%;
            border-radius: 5px !important;
            padding: 5px;
        }
        
        .tblDetails tr:nth-child(odd)
        {
            background-color: #ededed;
        }
        .button1
        {
            padding-left: 5px !important;
            padding-right: 5px !important;
        }
        .textBox
        {
            border-color: #adadad !important;
        }
        .space
        {
            margin: 0px 10px !important;
        }
    </style>
    <asp:HiddenField ID="hd_originCode" runat="server" />
    <asp:HiddenField ID="hd_destinationCode" runat="server" />
    <asp:HiddenField ID="hd_manifestType" runat="server" />
    <asp:HiddenField ID="hd_isDemanifested" runat="server" />
    <select id="recStatus" style="display: none;">
        <option value="0">.:Select Status:.</option>
    </select>
    <asp:Panel ID="rd_1" runat="server">
        <table style="font-family: Calibri; font-size: medium; padding-bottom: 0px !important;
            padding-top: 0px !important; width: 95%;" class="input-form">
            <tr style="padding: 0px 0px 0px 0px !important;">
                <td colspan="7" style="padding-bottom: 1px !important; padding-top: 1px !important;
                    text-align: center !important; float: left; width: 100%">
                    <h4 style="font-family: Calibri; margin: 0px !important; font-variant: small-caps;
                        text-align: center !important; width: 100%">
                        Manifest Info</h4>
                </td>
            </tr>
            <tr>
                <td colspan="7" style="text-align: right; width: 100%; float: left;">
                    <asp:Button ID="btn_searchDemanifest" runat="server" CssClass="button" UseSubmitBehavior="false"
                        Text="Search Demanifests" OnClientClick="PageRedirect(); return false;"></asp:Button>
                </td>
            </tr>
            <tr>
                <td class="field" style="width: 15% !important;">
                    Manifest Number
                </td>
                <td class="input-field" style="width: 15% !important;">
                    <asp:TextBox ID="txt_manifestNumber" runat="server" onchange="GetManifestDetails(this)"
                        onkeypress="return isNumberKey(event);" onkeydown="{(e) => e.preventDefault()}"></asp:TextBox>
                </td>
                <td class="space">
                </td>
                <td class="field" style="width: 10% !important;">
                    Date
                </td>
                <td class="input-field" style="width: 15% !important;">
                    <asp:TextBox ID="txt_date" runat="server" Enabled="false"></asp:TextBox>
                </td>
                <td class="space">
                </td>
                <td class="field" style="width: 15% !important;">
                    Demanifest Type
                </td>
                <td class="input-field" style="width: 22% !important; font-size: small;">
                    <input type="radio" name="demanType" value="1" disabled="true" style="width: 15px;"
                        checked="true" id='rbtn_manifest' />
                    With Manifest
                    <input type="radio" name="demanType" id="rbtn_woManifest" value="0" disabled="true"
                        style="width: 15px;" />
                    W/O Manifest
                </td>
            </tr>
            <tr>
                <td class="field" style="width: 15% !important;">
                    Origin
                </td>
                <td id="tdManifest"  class="input-field" style="width: 15% !important;">
                    <asp:TextBox ID="txt_origin" runat="server" Enabled="false"></asp:TextBox>
                </td>
                <td id="tdWoManifest" class="input-field" style="width: 15% !important; display: none;">
                    <asp:DropDownList ID="dd_origin" runat="server" AppendDataBoundItems="true"
                        onchange="ChangeOrigin(this);" Width="95%">
                        <asp:ListItem Value="0">Select Origin</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td class="space">
                </td>
                <td class="field" style="width: 10% !important;">
                    Destination
                </td>
                <td class="input-field" style="width: 15% !important;">
                    <asp:TextBox ID="txt_destination" runat="server" Enabled="false"></asp:TextBox>
                </td>
                
            </tr>
        </table>
        <fieldset style="border-radius: 5px !important; font-weight: bold; font-size: medium;
            padding: 10px !important; margin: 18px !important; width: 95%;">
            <legend>Manifest Contents (Consignments)</legend>
            <table style="width: 100%">
                <tr>
                    <td style="width: 13%; font-weight: bold;">
                        CN Number
                    </td>
                    <td style="width: 20%;">
                        <asp:TextBox ID="txt_cnNumber" runat="server" onchange="CheckConsignment(this);"
                            onkeypress="return isNumberKey(event);"></asp:TextBox>
                    </td>
                    <td>
                        <input type="text" style="border: 0px; background-color: #FFFFFF; width: 95%; font-size: medium;
                            font-weight: bold; font-family: Calibri; text-align: right;" disabled="true"
                            id="lblCount" />
                    </td>
                </tr>
            </table>
            <div style="width: 100%; overflow: scroll; height: 250px;">
                <table id="tbl_details" class="tblDetails">
                    <tr class="headerRow">
                        <td style="width: 5%; text-align: center;">
                        </td>
                        <td style="width: 15%;">
                            Consignment #
                        </td>
                        <td style="width: 15%;">
                            Status
                        </td>
                        <td style="width: 25%;">
                            Reason
                        </td>
                        <td style="width: 5%;">
                            ORGN.
                        </td>
                        <td style="width: 5%;">
                            DEST.
                        </td>
                        <td style="width: 10%;">
                            CN. Type
                        </td>
                        <td style="width: 10%;">
                            Service Type
                        </td>
                        <td style="width: 5%;">
                            Weight
                        </td>
                    </tr>
                </table>
            </div>
        </fieldset>
        <div style="width: 100%; text-align: center">
            &nbsp;
            <input type="button" value="Save" class="button" onclick="SaveDemanifest()" id="btnSave" />&nbsp;&nbsp;&nbsp;&nbsp;
            <input type="button" value="Reset" class="button" onclick="ResetAll();" id="btnReset" />
            <%--OnClick="btn_save_Click"--%>
            &nbsp;
        </div>
    </asp:Panel>
    <div style="display: none;">
        <asp:GridView ID="cnControls" runat="server">
            <Columns>
                <asp:BoundField HeaderText="Prefix" DataField="Prefix" />
                <asp:BoundField HeaderText="Length" DataField="Length" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
