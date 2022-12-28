<%@ Page Title="" Language="C#" MasterPageFile="~/BtsMasterPage.Master" AutoEventWireup="true" CodeBehind="BulkConsignment_NewSequences.aspx.cs" 
    Inherits="MRaabta.Files.BulkConsignment_NewSequences" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Namespace="AjaxControlToolkit" TagPrefix="Ajax" Assembly="AjaxControlToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        .bulkCnHeaderTbl {
            border: 1px Solid Black;
            font-size: small;
            font-family: Calibri;
            margin-left: 1%;
            margin-right: 1%;
            margin-top: 1%;
            border-color: #A5A5A5;
            border-radius: 10px !important;
            border-collapse: collapse;
        }

        .bulkCnHeaderTbl1 td {
            font-weight: bold;
            text-align: left;
        }

        .bulkCnHeaderTbl1 tr:nth-child(even) {
            background-color: #ededed;
        }

        .bulkCnAmountTbl {
            border: 1px Solid Black;
            font-size: small;
            font-family: Calibri;
            border-color: #A5A5A5;
            border-radius: 10px !important;
            width: 240px;
            height: 175px;
        }

        .bulkCnHeaderTbl textarea {
            border-color: grey;
        }

        .bulkCnHeaderTbl td {
            margin-right: 5px;
            vertical-align: top;
        }

            .bulkCnHeaderTbl td div {
                margin-top: 2px;
                margin-bottom: 2px;
            }

            .bulkCnHeaderTbl td input {
                border-color: #A5A5A5;
            }

        .bulkCnHeaderTbl .button {
            background-color: #f27031 !important;
            border: 0 none !important;
            border-radius: 5px !important;
            color: White !important;
            font-family: Calibri !important;
            font-size: small !important;
            padding: 3px 16px !important;
            cursor: pointer !important;
        }

        .dropdown {
            padding: 0px 10px 2px 5px;
        }

        .ajax__calendar_container {
            width: 210px !important;
        }

        .ajax__calendar_body {
            width: 210px !important;
        }

        .ajax__calendar_days {
            width: 210px !important;
        }

        .ajax__calendar_months {
            width: 210px !important;
        }

        .ajax__calendar_years {
            width: 210px !important;
        }

        .textBox {
            border-style: solid;
            border-width: 1px;
            border-color: #F1F1F1;
            background-color: White;
            border-radius: 5px;
            -webkit-border-radius: 5px;
            padding-left: 0px !important;
            vertical-align: middle;
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
            width: 314px;
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
    </style>
    <style type="text/css">
        .DimHead td {
            border: 2px Solid Black;
            text-align: center;
        }

        .DimRow td {
            border: 1px Solid Black;
            text-align: center;
            color: White;
        }

        .DivDim {
            width: 630px;
            height: 300px;
            border-radius: 15px;
            text-align: center;
            border: 1px Solid #444444;
            display: none;
            float: left;
            position: absolute;
            top: 33%;
            left: 33%;
            background-color: #444444;
        }
    </style>
    <script type="text/javascript" src="../Scripts/jquery-3.5.1.min.js"></script>
    <%--Locking--%>
    <script type="text/javascript">
        function LockRider(chk) {
            var control = document.getElementById('<%= txt_riderCode.ClientID%>');
            if (chk.checked) {
                if (control.value.trim() == "") {
                    alert('Enter Rider Code');
                    chk.checked = false;
                    return;
                }
                control.disabled = true;
                control.style.backgroundColor = '#EBEBE5';
            }
            else {
                control.disabled = false;
                control.style.backgroundColor = '#FFFFFF';
            }
        }

        function LockAccount(chk) {
            var control = document.getElementById('<%= txt_accountNo.ClientID%>');
            if (chk.checked) {
                if (control.value.trim() == "") {
                    alert('Enter Account Number');
                    chk.checked = false;
                    return;
                }
                control.disabled = true;
                control.style.backgroundColor = '#EBEBE5';
            }
            else {
                control.disabled = false;
                control.style.backgroundColor = '#FFFFFF';
            }
        }

        function LockReportingDate(chk) {
            var control = document.getElementById('<%= txt_reportingDate.ClientID%>');
            if (chk.checked) {
                if (control.value.trim() == "") {
                    alert('Enter Reporting Date');
                    chk.checked = false;
                    return;
                }
                control.disabled = true;
                control.style.backgroundColor = '#EBEBE5';
            }
            else {
                control.disabled = false;
                control.style.backgroundColor = '#FFFFFF';
            }
        }
        function LockBookingDate(chk) {
            var control = document.getElementById('<%= txt_bookingDate.ClientID%>');
            if (chk.checked) {
                control.disabled = true;
                control.style.backgroundColor = '#EBEBE5';
            }
            else {
                control.disabled = false;
                control.style.backgroundColor = '#FFFFFF';
            }
        }
        function LockStatus(chk) {
            var approvalStatus = document.getElementById('<%= dd_approvalStatus.ClientID %>');
            if (chk.checked) {
                approvalStatus.disabled = true;
                approvalStatus.style.backgroundColor = "#EBEBE5";
            }
            else {
                approvalStatus.disabled = false;
                approvalStatus.style.backgroundColor = "#FFFFFF";
            }
        }

        function LockPriceModifier(chk) {

            var modifier = document.getElementById('<%= dd_priceModifier.ClientID %>');
            var modifierBase = document.getElementById('<%= dd_calculationBase.ClientID %>');
            var modifierValue = document.getElementById('<%= txt_priceModifierValue.ClientID %>');
            var declaredValue = document.getElementById('<%= txt_declaredValue.ClientID %>');
            var baseValue = "0";
            if (chk.checked) {

                if (modifier.options.selectedIndex == 0) {
                    chk.checked = false;

                    modifier.disabled = false;
                    modifier.style.backgroundColor = '#FFFFFF';

                    modifierValue.disabled = true;
                    modifierValue.style.backgroundColor = '#EBEBE5';

                    declaredValue.disabled = true;
                    declaredValue.style.backgroundColor = '#EBEBE5';

                    alert('Select Price Modifier First');
                    return;
                }
                else {




                    for (var i = 0; i < modifierBase.rows[0].cells.length; i++) {
                        if (modifierBase.rows[0].cells[i].children[0].children[0].checked) {
                            baseValue = modifierBase.rows[0].cells[i].children[0].children[0].value;
                            break;
                        }
                    }
                    if (modifierValue.value.trim() == "" || modifierValue.value.trim() == "0" || isNaN(parseInt(modifierValue.value))) {

                        chk.checked = false;

                        modifier.disabled = false;
                        modifier.style.backgroundColor = '#FFFFFF';

                        modifierValue.disabled = false;
                        modifierValue.style.backgroundColor = '#FFFFFF';
                        if (baseValue == "3") {
                            declaredValue.disabled = false;
                            declaredValue.style.backgroundColor = '#FFFFFF';
                        }
                        else {
                            declaredValue.disabled = true;
                            declaredValue.style.backgroundColor = '#EBEBE5';
                        }

                        alert('Enter Price Modifier Value');
                        return;

                    }
                    if (baseValue == "3" && (declaredValue.value.trim() == "" || declaredValue.value.trim() == "0" || isNaN(parseInt(declaredValue.value)))) {
                        chk.checked = false;

                        modifier.disabled = false;
                        modifier.style.backgroundColor = '#FFFFFF';

                        if (modifier.options[modifier.options.selectedIndex].value.split('-')[2] == "0" || modifier.options[modifier.options.selectedIndex].value.split('-')[2].trim() == "" || isNaN(parseInt(modifier.options[modifier.options.selectedIndex].value.split('-')[2]))) {
                            modifierValue.disabled = false;
                            modifierValue.style.backgroundColor = '#FFFFFF';
                        }
                        else {
                            modifierValue.disabled = true;
                            modifierValue.style.backgroundColor = '#EBEBE5';
                        }


                        declaredValue.disabled = false;
                        declaredValue.style.backgroundColor = '#FFFFFF';
                        alert('Enter Declared Value');

                        return;
                    }
                    else if (baseValue == "3") {
                        var tempDeclaredValue = parseFloat(declaredValue.value);
                        if (isNaN(tempDeclaredValue)) {
                            alert('Enter Proper Declared Value');
                            return;
                        }
                        else if (tempDeclaredValue < 2000) {
                            alert('Declared Value must be Greater than 2,000');
                            return;
                        }
                        else if (tempDeclaredValue > 200000) {
                            alert('Declared Value must be lesser than 200,000');
                            return;
                        }
                    }
                    modifier.disabled = true;
                    modifier.style.backgroundColor = '#EBEBE5';

                    modifierValue.disabled = true;
                    modifierValue.style.backgroundColor = '#EBEBE5';


                    declaredValue.disabled = true;
                    declaredValue.style.backgroundColor = '#EBEBE5';



                }


            }
            else {
                modifier.disabled = false;
                modifier.style.backgroundColor = '#FFFFFF';
                for (var i = 0; i < modifierBase.rows[0].cells.length; i++) {
                    if (modifierBase.rows[0].cells[i].children[0].children[0].checked) {
                        baseValue = modifierBase.rows[0].cells[i].children[0].children[0].value;
                        break;
                    }
                }
                if (modifier.options[modifier.options.selectedIndex].value.split('-')[2] == "0" || modifier.options[modifier.options.selectedIndex].value.split('-')[2].trim() == "" || isNaN(parseInt(modifier.options[modifier.options.selectedIndex].value.split('-')[2]))) {
                    modifierValue.disabled = false;
                    modifierValue.style.backgroundColor = '#FFFFFF';
                }
                else {
                    modifierValue.disabled = true;
                    modifierValue.style.backgroundColor = '#EBEBE5';
                }
                if (baseValue == "3") {
                    declaredValue.disabled = false;
                    declaredValue.style.backgroundColor = '#FFFFFF';
                }
                else {
                    declaredValue.disabled = true;
                    declaredValue.style.backgroundColor = '#EBEBE5';
                }

            }

        }
        function LockService(chk) {
            var serviceType = document.getElementById('<%= dd_serviceType.ClientID %>');
            if (chk.checked) {
                if (serviceType.options.selectedIndex == 0) {
                    chk.checked = false;
                    alert('Select Service Type');
                    serviceType.disabled = false;
                    serviceType.style.backgroundColor = '#FFFFFF';
                    return;
                }
                else {
                    serviceType.disabled = true;
                    serviceType.style.backgroundColor = '#EBEBE5';
                }
            }
            else {
                serviceType.disabled = false;
                serviceType.style.backgroundColor = '#FFFFFF';
            }
        }
        function CalculateVolWeight(width, breadth, height) {
            if (isNaN(width) || isNaN(breadth) || isNaN(height)) {
                return 0;
            }
            else {
                return ((width * breadth * height) / 5000);
            }
        }

        function LockConsigner(chk) {
            var consigner = document.getElementById('<%= txt_consigner.ClientID %>');
            var consignerPhone = document.getElementById('<%= txt_consignerCell.ClientID %>');

            if (chk.checked) {
                consigner.disabled = true;
                consignerPhone.disabled = true;
                consigner.style.backgroundColor = '#EBEBE5';
                consignerPhone.style.backgroundColor = '#EBEBE5';
            }
            else {
                consigner.disabled = false;
                consignerPhone.disabled = false;
                consigner.style.backgroundColor = '#FFFFFF';
                consignerPhone.style.backgroundColor = '#FFFFFF';
            }

        }
        function LockConsignee(chk) {
            var consignee = document.getElementById('<%= txt_consignee.ClientID %>');
            var consigneePhone = document.getElementById('<%= txt_consigneeCell.ClientID %>');

            if (chk.checked) {
                consignee.disabled = true;
                consigneePhone.disabled = true;
                consignee.style.backgroundColor = '#EBEBE5';
                consigneePhone.style.backgroundColor = '#EBEBE5';
            }
            else {
                consignee.disabled = false;
                consigneePhone.disabled = false;
                consignee.style.backgroundColor = '#FFFFFF';
                consigneePhone.style.backgroundColor = '#FFFFFF';
            }

        }
        function LockAddress(chk) {
            var address = document.getElementById('<%= txt_Address.ClientID %>');
            if (chk.checked) {
                address.disabled = true;
                address.style.backgroundColor = '#EBEBE5';
            }
            else {
                address.disabled = false;
                address.style.backgroundColor = '#FFFFFF';
            }
        }

        function LockCNType(chk) {
            var cnType = document.getElementById('<%= dd_consignmentType.ClientID %>');
            if (chk.checked && cnType.options.selectedIndex == 0) {
                chk.checked = false;
                alert("Select Consignment Type");
                return;
            }
            else if (chk.checked) {
                cnType.disabled = true;
                cnType.style.backgroundColor = '#EBEBE5';
            }
            else {
                cnType.disabled = false;
                cnType.style.backgroundColor = '#FFFFFF';
            }
        }

        function LockEC(chk) {
            var originEC = document.getElementById('<%= txt_originExpressCenter.ClientID %>');
            var accountNumber = document.getElementById('<%= txt_accountNo.ClientID %>');
            if (accountNumber.value.trim() == "" && chk.checked == true) {
                alert('Enter Account Number First');
                chk.checked = false;
                return;
            }
            if (accountNumber.value == "0" && chk.checked == true) {
                if (originEC.value.trim() == "") {
                    alert('Enter Origin Express Center Code');
                    chk.checked = false;
                    return;
                }
                originEC.disabled = true;
                originEC.style.backgroundColor = '#EBEBE5';
            }
            else {
                originEC.disabled = false;
                originEC.style.backgroundColor = '#FFFFFF';
            }
        }
    </script>
    <%--Miscellenous--%>
    <script type="text/javascript">
        function isNumberKey(evt) {

            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if ((charCode > 31 && (charCode < 48 || charCode > 57)) || charCode == 13) {

                return false;
            }
            return true;
        }
        function isNumberKeydouble(evt) {

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

        function isNumber(evt) {

            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            return true;
        }
        function RiderWeightCheck() {

            var weight = document.getElementById('<%= txt_weight.ClientID %>').value;
            var rider = document.getElementById('<%= txt_riderCode.ClientID %>').value;
            var originEC = document.getElementById('<%= dd_originExpressCenter.ClientID %>');
            if (weight == rider) {
                alert('Rider Code and Weight Cannot be equal');
                document.getElementById('<%= txt_weight.ClientID %>').value = "";
                document.getElementById('<%= txt_riderCode.ClientID %>').value = "";
                originEC.options[dropdown.selectedIndex] = 0;
                return false;
            }
            else {
                return true;
            }
        }
        function ChangeMaxLength(dropdown) {

            var serviceType = dropdown.options[dropdown.selectedIndex].value;

            var weight = document.getElementById('<%= txt_weight.ClientID %>');
            //weight.value = "";

            if (serviceType == "Aviation Sale") {
                weight.maxLength = 5;
            }
            else {
                weight.maxLength = 4;
            }

            PageMethods.GetModifiers(serviceType, GetModifiersServerResponse);

        }
        function GetModifiersServerResponse(response, userContext, methodName) {
            var status = response.Status;
            var message = response.Message;

            var chkPm = document.getElementById('<%= chk_pm.ClientID %>');
            var priceModifier = document.getElementById('<%= dd_priceModifier.ClientID %>');
            var currentSelectedPM = priceModifier.options[priceModifier.options.selectedIndex].value;

            priceModifier.options.length = 0;

            var option1 = document.createElement('option');
            option1.value = '0';
            option1.text = 'Select Price Modifier';
            priceModifier.add(option1);

            if (status == "-1") {
                alert(message);
                return;
            }
            if (status == "0") {
                return;
            }
            var selectedIndex = 0;
            for (var i = 0; i < response.PriceModifiers.length; i++) {
                var option = document.createElement('option');
                var modifier = response.PriceModifiers[i];

                option.value = modifier.ID + '-' + modifier.Base + '-' + modifier.Value;
                option.text = modifier.Name;
                priceModifier.add(option);
                if (currentSelectedPM == option.value) {
                    selectedIndex = i + 1;
                }
            }

            priceModifier.options.selectedIndex = selectedIndex;
            PMChange(priceModifier);
            if (selectedIndex == 0) {
                chkPm.checked = false;
                LockPriceModifier(chkPm);
            }
            else {
                ChangeMaxLength(dd_service);
            }

        }

        function VolumeChange() {

            var tblDimensions = document.getElementById('tblDim');

            //            var txtlength = document.getElementById('<%= txt_l.ClientID %>');
            //            var txtwidth = document.getElementById('<%= txt_w.ClientID %>');
            //            var txtheight = document.getElementById('<%= txt_h.ClientID %>');
            var txtvWeight = document.getElementById('<%= txt_vWeight.ClientID %>');
            var txtaWeight = document.getElementById('<%= txt_aWeight.ClientID %>');
            var txtweight = document.getElementById('<%= txt_weight.ClientID %>');
            var txtPieces = document.getElementById('<%= txt_pieces.ClientID %>');
            var totalVWeight = 0;
            for (var i = 1; i < tblDimensions.rows.length; i++) {
                var row = tblDimensions.rows[i];
                var l = parseFloat(row.cells[1].childNodes[0].value);
                if (l.toString() == 'NaN') {
                    l = 0;
                }
                var w = parseFloat(row.cells[2].childNodes[0].value);
                if (w.toString() == 'NaN') {
                    w = 0;
                }

                var h = parseFloat(row.cells[3].childNodes[0].value);
                if (h.toString() == 'NaN') {
                    h = 0;
                }

                var vWeight = (l * w * h) / 5000;
                if (!isNaN(vWeight)) {
                    totalVWeight = totalVWeight + vWeight;
                }
            }


            var aWeight = parseFloat(txtaWeight.value);
            if (aWeight.toString() == 'NaN') {
                aWeight = 0;
            }

            txtvWeight.value = totalVWeight.toString();
            if (totalVWeight > aWeight) {
                txtweight.value = totalVWeight.toString();
            }
            else {
                txtweight.value = aWeight.toString();
            }



        }
        function ClearTable(tbl) {
            for (var i = 1; i < tbl.rows.length;) {
                tbl.deleteRow(1);
            }
        }
    </script>
    <%--Bulk Change--%>
    <script type="text/javascript">
        function bulkChange() {
            var chk = document.getElementById('<%= chk_bulkUpdate.ClientID %>');
            var cnFrom = document.getElementById('<%= txt_CnStart.ClientID %>');
            var numberOfCN = document.getElementById('<%= txt_NumberofCN.ClientID %>');
            var tblConsignments = document.getElementById('tblConsignments');
            if (chk.checked) {
                cnFrom.disabled = false;
                numberOfCN.disabled = false;
                cnFrom.style.backgroundColor = '#FFFFFF';
                numberOfCN.style.backgroundColor = '#FFFFFF';
            }
            else {
                cnFrom.disabled = true;
                numberOfCN.disabled = true;
                cnFrom.style.backgroundColor = '#EBEBE5';
                numberOfCN.style.backgroundColor = '#EBEBE5';

                for (var i = 1; i < tblConsignments.rows.length;) {
                    tblConsignments.deleteRow(1);
                }
            }
        }
    </script>
    <%--Bulk Consignment Change--%>
    <script type="text/javascript">

        function BulkCnChange() {
            document.getElementById('loader').style.display = 'block';
            var cnFrom = document.getElementById('<%= txt_CnStart.ClientID %>');
            var numberOfCN = document.getElementById('<%= txt_NumberofCN.ClientID %>');
            var tblConsignments = document.getElementById('tblConsignments');

            if (isNaN(cnFrom.value.trim())) {
                alert('Invalid Start of Consignment Numbers');
                return;
            }
            if (isNaN(numberOfCN.value.trim())) {
                alert('Invalid Number of CN');
                return;
            }
            if (cnFrom.value.trim() != "" && numberOfCN.value.trim() != "") {
                $.ajax({

                    url: 'BulkConsignment_NewSequences.aspx/GetConsignmentDetails',
                    type: 'POST',
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    data: "{'consignmentStart':'" + cnFrom.value.trim() + "', 'numberOfCN':'" + numberOfCN.value.trim() + "'}",
                    success: function (result) {
                        var resp = result.d.Consignments;
                        ClearTable(tblConsignments);
                        for (var i = 0; i < resp.length; i++) {
                            if (resp[i].ServerResponse != "OK") {
                                alert('Error in ConsignmentNumber: ' + resp[i].CN + '\n Error: ' + resp[i].ServerResponse);
                                continue;
                            }

                            debugger;

                            var btnEdit = document.createElement('input');
                            btnEdit.type = 'button';
                            btnEdit.className = 'button';
                            btnEdit.value = 'Edit';

                            var chkRemove = document.createElement('input');
                            chkRemove.type = 'checkbox';
                            chkRemove.className = 'chkRemove';
                            chkRemove.checked = true;

                            var hdDestination = document.createElement('input');
                            hdDestination.id = 'hdDestination';
                            hdDestination.className = 'hdDestination';
                            hdDestination.type = 'text';
                            hdDestination.style.display = 'none';
                            hdDestination.value = resp[i].Destination;

                            var hdOriginEC = document.createElement('input');
                            hdOriginEC.className = 'hdOriginEc';
                            hdOriginEC.type = 'text';
                            hdOriginEC.style.display = 'none';
                            hdOriginEC.value = resp[i].OriginEC;

                            var hdInsertType = document.createElement('input');
                            hdInsertType.className = 'hdInsertType';
                            hdInsertType.type = 'text';
                            hdInsertType.style.display = 'none';
                            hdInsertType.value = resp[i].InsertType;

                            var hdCreditClientID = document.createElement('input');
                            hdCreditClientID.className = 'hdCreditClientID';
                            hdCreditClientID.type = 'text';
                            hdCreditClientID.style.display = 'none';
                            hdCreditClientID.value = resp[i].CreditClientID;

                            var hdConsignmentType = document.createElement('input');
                            hdConsignmentType.className = 'hdConsignmentType';
                            hdConsignmentType.type = 'text';
                            hdConsignmentType.style.display = 'none';
                            hdConsignmentType.value = resp[i].CNType;

                            var hdPmID = document.createElement('input');
                            hdPmID.className = 'hdPmID';
                            hdPmID.type = 'text';
                            hdPmID.style.display = 'none';
                            hdPmID.value = resp[i].pmID;

                            var hdCalBase = document.createElement('input');
                            hdCalBase.className = 'hdCalBase';
                            hdCalBase.type = 'text';
                            hdCalBase.style.display = 'none';
                            hdCalBase.value = resp[i].calBase;

                            var hdCalValue = document.createElement('input');
                            hdCalValue.className = 'hdCalValue';
                            hdCalValue.type = 'text';
                            hdCalValue.style.display = 'none';
                            hdCalValue.value = resp[i].calValue;

                            var modCalValue = document.createElement('input');
                            modCalValue.className = 'modCalValue';
                            modCalValue.type = 'text';
                            modCalValue.style.display = 'none';
                            modCalValue.value = resp[i].modCalValue;

                            var calGst = document.createElement('input');
                            calGst.className = 'calGst';
                            calGst.type = 'text';
                            calGst.style.display = 'none';
                            calGst.value = resp[i].calGst;

                            var isTaxable = document.createElement('input');
                            isTaxable.className = 'isTaxable';
                            isTaxable.type = 'text';
                            isTaxable.style.display = 'none';
                            isTaxable.value = resp[i].isTaxable;

                            var hdIsApproved = document.createElement('input');
                            hdIsApproved.className = 'isApproved';
                            hdIsApproved.type = 'text';
                            hdIsApproved.style.display = 'none';
                            hdIsApproved.value = resp[i].Approved;

                            var hdIsNew = document.createElement('input');
                            hdIsNew.className = 'isNew';
                            hdIsNew.type = 'text';
                            hdIsNew.style.display = 'none';
                            hdIsNew.value = resp[i].isNew;

                            var isCODCN = document.createElement('input');
                            isCODCN.className = 'isCODCN';
                            isCODCN.type = 'text';
                            isCODCN.style.display = 'none';
                            isCODCN.value = resp[i].isCOD;

                            var tr = tblConsignments.insertRow(1);
                            var td = tr.insertCell(0);
                            td.appendChild(chkRemove);
                            td.appendChild(btnEdit);

                            td.style.fontWeight = 'normal';
                            btnEdit.onclick = EditConsignment.bind(tr.cells[0].childNodes[0], tr);

                            td.appendChild(hdDestination);
                            td.appendChild(hdInsertType);
                            td.appendChild(hdOriginEC);
                            td.appendChild(hdCreditClientID);
                            td.appendChild(hdConsignmentType);
                            td.appendChild(hdPmID);
                            td.appendChild(hdCalBase);
                            td.appendChild(hdCalValue);
                            td.appendChild(modCalValue);
                            td.appendChild(calGst);
                            td.appendChild(isTaxable);
                            td.appendChild(hdIsApproved);
                            td.appendChild(hdIsNew);
                            td.appendChild(isCODCN);

                            td = tr.insertCell(1);
                            td.innerHTML = resp[i].CN;
                            td.style.fontWeight = 'normal';

                            td = tr.insertCell(2);
                            td.innerHTML = resp[i].BKDate;
                            td.style.fontWeight = 'normal';

                            td = tr.insertCell(3);
                            td.innerHTML = resp[i].Acc;
                            td.style.fontWeight = 'normal';

                            td = tr.insertCell(4);
                            td.innerHTML = resp[i].ServiceType;
                            td.style.fontWeight = 'normal';

                            td = tr.insertCell(5);
                            td.innerHTML = resp[i].Consigner;
                            td.style.fontWeight = 'normal';

                            td = tr.insertCell(6);
                            td.innerHTML = resp[i].Consignee;
                            td.style.fontWeight = 'normal';

                            td = tr.insertCell(7);
                            td.innerHTML = resp[i].ConsignerMob;
                            td.style.fontWeight = 'normal';

                            td = tr.insertCell(8);
                            td.innerHTML = resp[i].ConsigneeMob;
                            td.style.fontWeight = 'normal';

                            td = tr.insertCell(9);
                            td.innerHTML = resp[i].DestinationName;
                            td.style.fontWeight = 'normal';

                            td = tr.insertCell(10);
                            td.innerHTML = resp[i].Rider;
                            td.style.fontWeight = 'normal';

                            td = tr.insertCell(11);
                            td.innerHTML = resp[i].Dimensions;
                            td.style.fontWeight = 'normal';

                            td = tr.insertCell(12);
                            td.innerHTML = resp[i].VolWgt;
                            td.style.fontWeight = 'normal';

                            td = tr.insertCell(13);
                            td.innerHTML = resp[i].DnsWgt;
                            td.style.fontWeight = 'normal';

                            td = tr.insertCell(14);
                            td.innerHTML = resp[i].Weight;
                            td.style.fontWeight = 'normal';

                            td = tr.insertCell(15);
                            td.innerHTML = resp[i].Pieces;
                            td.style.fontWeight = 'normal';

                            td = tr.insertCell(16);
                            td.innerHTML = resp[i].Address;
                            td.style.fontWeight = 'normal';

                            td = tr.insertCell(17);
                            td.innerHTML = resp[i].OriginECName;
                            td.style.fontWeight = 'normal';

                            td = tr.insertCell(18);
                            td.innerHTML = resp[i].CNTypeName;
                            td.style.fontWeight = 'normal';

                            td = tr.insertCell(19);
                            td.innerHTML = resp[i].Coupon;
                            td.style.fontWeight = 'normal';

                            td = tr.insertCell(20);
                            td.innerHTML = resp[i].SpecialInstructions;
                            td.style.fontWeight = 'normal';

                            td = tr.insertCell(21);
                            td.innerHTML = resp[i].RPDate;
                            td.style.fontWeight = 'normal';

                            td = tr.insertCell(22);
                            td.innerHTML = resp[i].Approved;
                            td.style.fontWeight = 'normal';

                            td = tr.insertCell(23);
                            td.innerHTML = resp[i].InvStatus;
                            td.style.fontWeight = 'normal';

                            td = tr.insertCell(24);
                            td.innerHTML = resp[i].InvNumber;
                            td.style.fontWeight = 'normal';

                            td = tr.insertCell(25);
                            td.innerHTML = resp[i].CODRef;
                            td.style.fontWeight = 'normal';

                            td = tr.insertCell(26);
                            td.innerHTML = resp[i].CODDesc;
                            td.style.fontWeight = 'normal';

                            td = tr.insertCell(27);
                            td.innerHTML = resp[i].CODAmt;
                            td.style.fontWeight = 'normal';

                            td = tr.insertCell(28);
                            td.innerHTML = resp[i].AddServ;
                            td.style.fontWeight = 'normal';

                            td = tr.insertCell(29);
                            td.innerHTML = resp[i].AltValue;
                            td.style.fontWeight = 'normal';

                            td = tr.insertCell(30);
                            td.innerHTML = resp[i].AddChrg;
                            td.style.fontWeight = 'normal';

                            td = tr.insertCell(31);
                            td.innerHTML = resp[i].ChrgAmt;
                            td.style.fontWeight = 'normal';

                            td = tr.insertCell(32);
                            td.innerHTML = resp[i].TotalAmt;
                            td.style.fontWeight = 'normal';

                            td = tr.insertCell(33);
                            td.innerHTML = resp[i].Gst;
                            td.style.fontWeight = 'normal';

                            td = tr.insertCell(34);
                            td.innerHTML = resp[i].shipperAddress;
                            td.style.fontWeight = 'normal';

                            td = tr.insertCell(35);
                            td.innerHTML = resp[i].PakageContents;
                            td.style.fontWeight = 'normal';


                        }

                        var dimensions = result.d.Dimensions;
                        var tblCnDims = document.getElementById('cnDim');

                        for (var i = 1; i < tblCnDims.rows.length;) {
                            tblCnDims.deleteRow(1);
                        }

                        for (var i = 0; i < dimensions.length; i++) {
                            var cnd = dimensions[i];
                            var tr = tblCnDims.insertRow(tblCnDims.rows.length);

                            tr.insertCell(0);
                            tr.insertCell(1);
                            tr.insertCell(2);
                            tr.insertCell(3);
                            tr.insertCell(4);
                            tr.insertCell(5);
                            tr.insertCell(6);

                            tr.cells[0].innerText = cnd.ConsignmentNumber;
                            tr.cells[1].innerText = cnd.ItemNumber;
                            tr.cells[2].innerText = cnd.Length;
                            tr.cells[3].innerText = cnd.Width;
                            tr.cells[4].innerText = cnd.Height;
                            tr.cells[5].innerText = cnd.Pieces;
                            tr.cells[6].innerText = cnd.VolWeight;
                        }

                        if (tblConsignments.rows.length > 1) {
                            EditConsignment(tblConsignments.rows[1]);
                        }

                        loader.style.display = 'none';

                    },
                    failure: function () { alert('Something Went wrong. Please contact I.T. Support.'); loader.style.display = 'none'; },
                    error: function () { alert('Something Went wrong. Please contact I.T. Support.'); loader.style.display = 'none'; }
                });
            }
            else {
                loader.style.display = 'none';
            }

        }

    </script>
    <%--Editing Consignments--%>
    <script type="text/javascript">
        function EditConsignment(tr) {
            var bookingdate = document.getElementById('<%= txt_bookingDate.ClientID %>');
            var chkBookingDate = document.getElementById('<%= chk_BookingDateFreeze.ClientID %>');
            var cnNumber = document.getElementById('<%= txt_cnNumber.ClientID %>');
            var accountNo = document.getElementById('<%= txt_accountNo.ClientID %>');
            var chkAccountNo = document.getElementById('<%= chk_accountNoFreeze.ClientID %>');
            var serviceType = document.getElementById('<%= dd_serviceType.ClientID %>');
            var chkServiceType = document.getElementById('<%= chk_service.ClientID %>');

            var consigner = document.getElementById('<%= txt_consigner.ClientID %>');
            var consignerCell = document.getElementById('<%= txt_consignerCell.ClientID %>');
            var chk_lockConsigner = document.getElementById('<%= chk_lockConsigner.ClientID %>');

            var consignee = document.getElementById('<%= txt_consignee.ClientID %>');
            var consigneeCell = document.getElementById('<%= txt_consigneeCell.ClientID %>');
            var chk_lockConsignee = document.getElementById('<%= chk_lockConsignee.ClientID %>');

            var address = document.getElementById('<%= txt_Address.ClientID %>');
            var chk_lockAddress = document.getElementById('<%= chk_lockAddress.ClientID %>');

            var destination = document.getElementById('<%= dd_destination.ClientID %>');
            var riderCode = document.getElementById('<%= txt_riderCode.ClientID %>');
            var chkRiderCode = document.getElementById('<%= chk_riderCodeFreeze.ClientID %>');
            var length = document.getElementById('<%= txt_l.ClientID %>');
            var width = document.getElementById('<%= txt_w.ClientID %>');
            var height = document.getElementById('<%= txt_h.ClientID %>');
            var volWeight = document.getElementById('<%= txt_vWeight.ClientID %>');
            var denseWeight = document.getElementById('<%= txt_aWeight.ClientID %>');
            var weight = document.getElementById('<%= txt_weight.ClientID %>');
            var pieces = document.getElementById('<%= txt_pieces.ClientID %>');

            var originEc = document.getElementById('<%= dd_originExpressCenter.ClientID %>');
            var txt_ec = document.getElementById('<%= txt_originExpressCenter.ClientID %>');
            var chk_ec = document.getElementById('<%= chk_lockOriginEC.ClientID %>');

            var cnType = document.getElementById('<%= dd_consignmentType.ClientID %>');
            var chk_cnType = document.getElementById('<%= chk_cnType.ClientID %>');
            var coupon = document.getElementById('<%= txt_coupon.ClientID %>');
            var remarks = document.getElementById('<%= txt_remarks.ClientID %>');
            var reportingDate = document.getElementById('<%= txt_reportingDate.ClientID %>');
            var chkReportingDate = document.getElementById('<%= chk_reportingDateFreeze.ClientID %>');
            var approvalStatus = document.getElementById('<%= dd_approvalStatus.ClientID %>');
            var chkApprovalStatus = document.getElementById('<%= chk_approval.ClientID %>');
            var invoiceStatus = document.getElementById('<%= txt_invoiceStatus.ClientID %>');
            var invoiceNumber = document.getElementById('<%= txt_invoiceNumber.ClientID %>');
            var chargeAmount = document.getElementById('<%= txt_chargedAmount.ClientID %>');
            var totalAmount = document.getElementById('<%= txt_totalAmt.ClientID %>');
            var gst = document.getElementById('<%= txt_gst.ClientID %>');
            var orderRefNo = document.getElementById('<%= txt_orderRefNo.ClientID %>');
            var descriptionCOD = document.getElementById('<%= txt_descriptionCOD.ClientID %>');
            var codAmount = document.getElementById('<%= txt_codAmount.ClientID %>');
            var chkCod = document.getElementById('<%= chk_cod.ClientID %>');
            var priceModifier = document.getElementById('<%= dd_priceModifier.ClientID %>');
            var calBase = document.getElementById('<%= dd_calculationBase.ClientID %>');
            var calValue = document.getElementById('<%= txt_priceModifierValue.ClientID %>');
            var declaredValue = document.getElementById('<%= txt_declaredValue.ClientID %>');
            var chkPm = document.getElementById('<%= chk_pm.ClientID %>');
            var hd_newCN = document.getElementById('<%= hd_newCN.ClientID %>');



            var hdDestination = tr.getElementsByClassName('hdDestination')[0];
            var hdOriginEc = tr.getElementsByClassName('hdOriginEc')[0];
            var hdInsertType = tr.getElementsByClassName('hdInsertType')[0];
            var hdCreditClientID = tr.getElementsByClassName('hdCreditClientID')[0];
            var hdConsignmentType = tr.getElementsByClassName('hdConsignmentType')[0];
            var hdPmID = tr.getElementsByClassName('hdPmID')[0];
            var hdCalBase = tr.getElementsByClassName('hdCalBase')[0];
            var hdCalValue = tr.getElementsByClassName('hdCalValue')[0];
            var modCalValue = tr.getElementsByClassName('modCalValue')[0];
            var calGst = tr.getElementsByClassName('calGst')[0];
            var isTaxable = tr.getElementsByClassName('isTaxable')[0];
            var hdIsApproved = tr.getElementsByClassName('isApproved')[0];
            hd_newCN.value = tr.getElementsByClassName('isNew')[0].value;
            var hdCODCN = tr.getElementsByClassName('isCODCN')[0].value;

            //            if ((approvalStatus.options[approvalStatus.options.selectedIndex].value == "1" || approvalStatus.options[approvalStatus.options.selectedIndex].text.toUpperCase() == "APPROVED") && chkApprovalStatus.checked == false) {
            //                if (hdIsApproved.value == "1") {
            //                    alert("Consignment Already Approved.");
            //                    return;
            //                }
            //            }

            if (invoiceNumber.value.trim() != "") {
                alert('Consignment Already Invoiced. Edit Not Allowed.');
                return;
            }

            var minAllowedDate = document.getElementById('<%= hd_minAllowedDate.ClientID %>');
            var maxAllowedDate = document.getElementById('<%= hd_maxAllowedDate.ClientID %>');

            if (!chkReportingDate.checked || (hdCODCN == "1" || hdCODCN.toUpperCase() == "TRUE")) {


                if (!isNaN(Date.parse(tr.cells[21].innerHTML))) {
                    reportingDate.value = tr.cells[21].innerHTML;
                }
                if (tr.cells[21].innerHTML.trim() == "") {
                    reportingDate.value = maxAllowedDate.value;
                }
                else {
                    reportingDate.value = tr.cells[21].innerHTML;
                }

            }



            if (!chkBookingDate.checked || (hdCODCN == "1" || hdCODCN.toUpperCase() == "TRUE")) {
                if (!isNaN(Date.parse(tr.cells[2].innerHTML))) {
                    bookingdate.value = tr.cells[2].innerHTML;
                }
                if (tr.cells[2].innerHTML.trim() == "") {
                    bookingdate.value = maxAllowedDate.value;
                }
                else {
                    bookingdate.value = tr.cells[2].innerHTML;
                }
            }


            var cnReportingDate = tr.cells[21].innerHTML;
            var cnBookingDate = tr.cells[2].innerHTML;



            var compare = CompareDates(cnReportingDate, minAllowedDate.value);
            if (isNaN(compare)) {

            }
            else if (compare == -1) {
                alert('This Consignment has been Closed.');
                return;
            }
            compare = "";
            compare = CompareDates(cnBookingDate, minAllowedDate.value);
            if (isNaN(compare)) {

            }
            else if (compare == -1) {
                alert('This Consignment has been Closed.');

                return;
            }
            compare = CompareDates(reportingDate.value, minAllowedDate.value);
            if (isNaN(compare)) {
            }
            else if (compare == -1) {
                alert('This Consignment has been Closed.');
                reportingDate.value = maxAllowedDate.value;
                return;
            }

            compare = CompareDates(bookingdate.value, minAllowedDate.value);
            if (isNaN(compare)) {
            }
            else if (compare == -1) {
                alert('This Consignment has been closed');
                bookingdate.value = maxAllowedDate.value;
                return;
            }


            cnNumber.value = tr.cells[1].innerHTML;
            if (!chkAccountNo.checked || (hdCODCN == "1" || hdCODCN.toUpperCase() == "TRUE")) {
                accountNo.value = tr.cells[3].innerHTML;
            }

            if (!chkServiceType.checked || (hdCODCN == "1" || hdCODCN.toUpperCase() == "TRUE")) {
                for (var i = 0; i < serviceType.options.length; i++) {
                    if (serviceType.options[i].value.toUpperCase() == tr.cells[4].innerHTML.toString().toUpperCase()) {
                        serviceType.options[i].selected = true;
                        break;
                    }
                }
            }

            if (!chk_lockConsigner.checked || (hdCODCN == "1" || hdCODCN.toUpperCase() == "TRUE")) {
                consigner.value = tr.cells[5].innerHTML;
                consignerCell.value = tr.cells[7].innerHTML;
            }
            if (!chk_lockConsignee.checked || (hdCODCN == "1" || hdCODCN.toUpperCase() == "TRUE")) {
                consignee.value = tr.cells[6].innerHTML;
                consigneeCell.value = tr.cells[8].innerHTML;
            }




            var combo = $find("<%= dd_destination.ClientID %>");

            var items = combo.get_items();
            var itm = combo.findItemByValue(hdDestination.value);
            if (itm != null) {
                itm.select();
            }

            if (!chkRiderCode.checked || (hdCODCN == "1" || hdCODCN.toUpperCase() == "TRUE")) {
                riderCode.value = tr.cells[10].innerHTML;
            }

            var dimension = tr.cells[11].innerHTML;
            var dimensions = dimension.split('X');
            if (dimensions.length == 3) {
                width.value = dimensions[0];
                length.value = dimensions[1];
                height.value = dimensions[2];
                volWeight.value = tr.cells[12].innerText;
                //volWeight.value = CalculateVolWeight(width.value, length.value, height.value);
            }

            denseWeight.value = tr.cells[13].innerHTML;
            if (isNaN(tr.cells[14].innerHTML)) {
                weight.value = "0";
            }
            else {
                weight.value = tr.cells[14].innerHTML;
            }

            if (isNaN(tr.cells[15].innerHTML)) {
                pieces.value = "0";
            }
            else {
                pieces.value = tr.cells[15].innerHTML;
            }
            if (!chk_lockAddress.checked || (hdCODCN == "1" || hdCODCN.toUpperCase() == "TRUE")) {
                address.value = tr.cells[16].innerHTML;
            }

            if (!chk_cnType.checked || (hdCODCN == "1" || hdCODCN.toUpperCase() == "TRUE")) {
                for (var i = 0; i < cnType.options.length; i++) {

                    if (cnType.options[i].value == hdConsignmentType.value) {
                        cnType.options[i].selected = true;
                        break;
                    }
                }
            }


            coupon.value = tr.cells[19].innerHTML;
            remarks.value = tr.cells[20].innerHTML;

            if (!chkApprovalStatus.checked) {
                if (tr.cells[22].innerHTML.toString().toUpperCase() == "Y" || tr.cells[22].innerHTML.toString().toUpperCase() == "1") {
                    approvalStatus.options.selectedIndex = 1;
                }
                else {
                    approvalStatus.options.selectedIndex = 0;
                }
            }


            invoiceStatus.value = tr.cells[23].innerHTML;
            invoiceNumber.value = tr.cells[24].innerHTML;
            orderRefNo.value = tr.cells[25].innerHTML;
            descriptionCOD.value = tr.cells[26].innerHTML;
            codAmount.value = tr.cells[27].innerHTML;
            var pmDetail = "";
            if (!chkPm.checked) {
                for (var i = 0; i < priceModifier.options.length; i++) {
                    if (priceModifier.options[i].value.split('-')[0] == hdPmID.value) {
                        priceModifier.options[i].selected = true;
                        pmDetail = priceModifier.options[i];
                        break;
                    }
                }

                calValue.value = modCalValue.value;
                for (var i = 0; i < calBase.rows[0].cells.length; i++) {
                    if (calBase.rows[0].cells[i].children[0].children[0].value == hdCalBase.value) {
                        calBase.rows[0].cells[i].children[0].children[0].checked = true;
                        break;
                    }
                }
                declaredValue.value = tr.cells[29].innerHTML;
            }

            chargeAmount.value = tr.cells[31].innerHTML;
            totalAmount.value = tr.cells[32].innerHTML;
            gst.value = tr.cells[33].innerHTML;


            if (hdCODCN == "1" || hdCODCN.toUpperCase() == "TRUE") {
                chkReportingDate.checked = true;
                chkReportingDate.disabled = true;
                reportingDate.disabled = true;

                chkBookingDate.checked = true;
                chkBookingDate.disabled = true;
                bookingdate.disabled = true;

                chkRiderCode.checked = true;
                chkRiderCode.disabled = true;
                riderCode.disabled = true;

                chkAccountNo.checked = true;
                chkAccountNo.disabled = true;
                accountNo.disabled = true;

                chk_lockConsigner.checked = true;
                chk_lockConsigner.disabled = true;
                consigner.disabled = true;
                consignerCell.disabled = true;

                chk_lockConsignee.checked = true;
                chk_lockConsignee.disabled = true;
                consignee.disabled = true;
                consigneeCell.disabled = true;


                chk_lockAddress.checked = true;
                chk_lockAddress.disabled = true;
                address.disabled = true;



                LockReportingDate(chkReportingDate);
                LockBookingDate(chkBookingDate);
                LockRider(chkRiderCode);
                LockAccount(chkAccountNo);
                LockConsigner(chk_lockConsigner);
                LockConsignee(chk_lockConsignee);
                LockAddress(chk_lockAddress);
            }
            else {
                chkReportingDate.disabled = false;
                chkBookingDate.disabled = false;
                chkRiderCode.disabled = false;
                chkAccountNo.disabled = false;
                chk_lockConsigner.disabled = false;
                chk_lockConsignee.disabled = false;
                chk_lockAddress.disabled = false;

                if (chkReportingDate.checked) {
                    reportingDate.disabled = true;
                }
                else {
                    reportingDate.disabled = false;
                }


                if (chkBookingDate.checked) {
                    bookingdate.disabled = true;
                }
                else {
                    bookingdate.disabled = false;
                }


                if (chkRiderCode.checked) {
                    riderCode.disabled = true;
                }
                else {
                    riderCode.disabled = false;
                }

                if (chkAccountNo.checked) {
                    accountNo.disabled = true;
                }
                else {
                    accountNo.disabled = false;
                }

                if (chk_lockConsigner.checked) {
                    consigner.disabled = true;
                    consignerCell.disabled = true;
                }
                else {
                    consigner.disabled = false;
                    consignerCell.disabled = false;
                }


                if (chk_lockConsignee.checked) {
                    consignee.disabled = true;
                    consigneeCell.disabled = true;
                }
                else {
                    consignee.disabled = false;
                    consigneeCell.disabled = false;
                }

                if (chk_lockAddress.checked) {
                    address.disabled = true;

                }
                else {
                    address.disabled = false;

                }

                LockReportingDate(chkReportingDate);
                LockBookingDate(chkBookingDate);
                LockRider(chkRiderCode);
                LockAccount(chkAccountNo);
                LockConsigner(chk_lockConsigner);
                LockConsignee(chk_lockConsignee);
                LockAddress(chk_lockAddress);
            }


            if (!chkBookingDate.checked) {
                focusWorking(bookingdate);
            }
            else if (!chkRiderCode.checked) {
                focusWorking(riderCode);
            }
            else if (!chkAccountNo.checked) {
                focusWorking(accountNo);
            }
            else if (!chk_lockConsigner.checked) {
                focusWorking(consigner);
            }
            else if (!chk_lockConsignee.checked) {
                focusWorking(consignee);
            }
            else if (!chkServiceType.checked) {
                focusWorking(serviceType);
            }
            else {
                focusWorking(chkServiceType);
            }
            if (!chk_ec.checked) {
                txt_ec.value = hdOriginEc.value;
            }

            PopulateDimTableForEdit();

            ValidateAccount();
            ValidateRider();
            if (!chkServiceType.checked) {
                for (var i = 0; i < serviceType.options.length; i++) {
                    if (serviceType.options[i].value.toUpperCase() == tr.cells[4].innerText.toUpperCase()) {
                        serviceType.options[i].selected = true;
                        break;
                    }
                }
            }

        }
    </script>
    <%--Populate Dimension Table for Edit--%>
    <script type="text/javascript">
        function PopulateDimTableForEdit() {
            var cnNumber = document.getElementById('<%= txt_cnNumber.ClientID %>');
            var pieces = document.getElementById('<%= txt_pieces.ClientID %>');
            var volWeight = document.getElementById('<%= txt_vWeight.ClientID %>');
            var txtWeight = document.getElementById('<%= txt_weight.ClientID %>');

            var tblDim = document.getElementById('tblDim');
            var tblCnDim = document.getElementById('cnDim');
            for (var i = 1; i < tblDim.rows.length;) {
                tblDim.deleteRow(1);
            }
            AddDimensionRow();
            var firstRow = true;
            for (var i = 1; i < tblCnDim.rows.length; i++) {
                var currentRow = tblCnDim.rows[i];
                if (currentRow.cells[0].innerText == cnNumber.value) {
                    if (!firstRow) {
                        AddDimensionRow();
                    }
                    var dimRow = tblDim.rows[tblDim.rows.length - 1];
                    dimRow.cells[1].childNodes[0].value = currentRow.cells[2].innerText;
                    dimRow.cells[2].childNodes[0].value = currentRow.cells[3].innerText;
                    dimRow.cells[3].childNodes[0].value = currentRow.cells[4].innerText;
                    dimRow.cells[4].childNodes[0].value = currentRow.cells[5].innerText;
                    VolumeChangePerLine(dimRow.cells[1].childNodes[0]);
                    var temp = txtWeight.value;
                    var temp2 = volWeight.value;
                    firstRow = false;
                }
            }

        }
    </script>
    <%--Update Consignment Changes--%>
    <script type="text/javascript">
        function UpdateCNChanges() {
            var bookingdate = document.getElementById('<%= txt_bookingDate.ClientID %>');
            var chkBookingDate = document.getElementById('<%= chk_BookingDateFreeze.ClientID %>');
            var cnNumber = document.getElementById('<%= txt_cnNumber.ClientID %>');
            var accountNo = document.getElementById('<%= txt_accountNo.ClientID %>');
            var chkAccountNo = document.getElementById('<%= chk_accountNoFreeze.ClientID %>');
            var serviceType = document.getElementById('<%= dd_serviceType.ClientID %>');
            var chkServiceType = document.getElementById('<%= chk_service.ClientID %>');

            var consigner = document.getElementById('<%= txt_consigner.ClientID %>');
            var consignerCell = document.getElementById('<%= txt_consignerCell.ClientID %>');
            var chk_lockConsigner = document.getElementById('<%= chk_lockConsigner.ClientID %>');

            var consignee = document.getElementById('<%= txt_consignee.ClientID %>');
            var consigneeCell = document.getElementById('<%= txt_consigneeCell.ClientID %>');
            var chk_lockConsignee = document.getElementById('<%= chk_lockConsignee.ClientID %>');

            var address = document.getElementById('<%= txt_Address.ClientID %>');
            var chk_lockAddress = document.getElementById('<%= chk_lockAddress.ClientID %>');

            var destination = document.getElementById('<%= dd_destination.ClientID %>');
            var riderCode = document.getElementById('<%= txt_riderCode.ClientID %>');
            var chkRiderCode = document.getElementById('<%= chk_riderCodeFreeze.ClientID %>');
            var length = document.getElementById('<%= txt_l.ClientID %>');
            var width = document.getElementById('<%= txt_w.ClientID %>');
            var height = document.getElementById('<%= txt_h.ClientID %>');
            var volWeight = document.getElementById('<%= txt_vWeight.ClientID %>');
            var denseWeight = document.getElementById('<%= txt_aWeight.ClientID %>');
            var weight = document.getElementById('<%= txt_weight.ClientID %>');
            var pieces = document.getElementById('<%= txt_pieces.ClientID %>');

            var originEc = document.getElementById('<%= dd_originExpressCenter.ClientID %>');
            var txt_ec = document.getElementById('<%= txt_originExpressCenter.ClientID %>');
            var chk_ec = document.getElementById('<%= chk_lockOriginEC.ClientID %>');

            var cnType = document.getElementById('<%= dd_consignmentType.ClientID %>');
            var coupon = document.getElementById('<%= txt_coupon.ClientID %>');
            var remarks = document.getElementById('<%= txt_remarks.ClientID %>');
            var reportingDate = document.getElementById('<%= txt_reportingDate.ClientID %>');
            var chkReportingDate = document.getElementById('<%= chk_reportingDateFreeze.ClientID %>');
            var approvalStatus = document.getElementById('<%= dd_approvalStatus.ClientID %>');
            var invoiceStatus = document.getElementById('<%= txt_invoiceStatus.ClientID %>');
            var invoiceNumber = document.getElementById('<%= txt_invoiceNumber.ClientID %>');
            var chargeAmount = document.getElementById('<%= txt_chargedAmount.ClientID %>');
            var totalAmount = document.getElementById('<%= txt_totalAmt.ClientID %>');
            var gst = document.getElementById('<%= txt_gst.ClientID %>');
            var orderRefNo = document.getElementById('<%= txt_orderRefNo.ClientID %>');
            var descriptionCOD = document.getElementById('<%= txt_descriptionCOD.ClientID %>');
            var codAmount = document.getElementById('<%= txt_codAmount.ClientID %>');
            var chkCod = document.getElementById('<%= chk_cod.ClientID %>');
            var priceModifier = document.getElementById('<%= dd_priceModifier.ClientID %>');
            var calBase = document.getElementById('<%= dd_calculationBase.ClientID %>');
            var calValue = document.getElementById('<%= txt_priceModifierValue.ClientID %>');
            var declaredValue = document.getElementById('<%= txt_declaredValue.ClientID %>');
            var chkPm = document.getElementById('<%= chk_pm.ClientID %>');

            var minAllowedDate = document.getElementById('<%= hd_minAllowedDate.ClientID %>');
            var maxAllowedDate = document.getElementById('<%= hd_maxAllowedDate.ClientID %>');

 	    minAllowedDate.value = minAllowedDate.value.replaceAll("-", "/");	
            maxAllowedDate.value = maxAllowedDate.value.replaceAll("-", "/");


            var table = document.getElementById('tblConsignments');
            var tr;
            var found = false;
            for (var i = 0; i < table.rows.length; i++) {
                tr = table.rows[i];
                if (tr.cells[1].innerHTML.trim() == cnNumber.value.trim()) {
                    break;
                }
            }


            if (bookingdate.value.trim() == "" || reportingDate.value.trim() == "") {
                alert('Invalid Dates Selected');

                reportingDate.value = maxAllowedDate.value;

                if (tr.cells[2].innerHTML.trim() != "") {
                    bookingdate.value = maxAllowedDate.value;
                }
                else {
                    bookingdate.value = tr.cells[2].innerHTML.trim();
                }

                if (tr.cells[21].innerHTML.trim() != "") {
                    reportingDate.value = tr.cells[21].innerHTML;
                }
                return;
            }
            if (invoiceNumber.value.trim() != "") {
                alert('Consignment Already Invoiced. Changes are not allowed.');
                return;
            }
            var bkDate = bookingdate.value.split('/');
            if (bkDate.length == 3) {
                var bkYear = bkDate[2];
                var bkMonth = bkDate[1];
                var bkDay = bkDate[0];


            }
            else {
                alert('Invalid Booking Date');
                focusWorking(bookingdate);
                return;
            }

            var rpDate = reportingDate.value.split('/');
            if (rpDate.length == "3") {

            }
            else {
                alert('Invalid reporting Date');
                focusWorking(reportingDate);
            }

            var compare = CompareDates(reportingDate.value, minAllowedDate.value);
            if (isNaN(compare)) {
                alert('Invalid Reporting Date');
                focusWorking(reportingDate);
                return;
            }
            else if (compare == -1) {
                alert('Reporting Date cannot be less than ' + minAllowedDate.value);
                focusWorking(reportingDate);
                return;
            }

            compare = CompareDates(reportingDate.value, maxAllowedDate.value);
            if (isNaN(compare)) {
                alert('Invalid Reporting Date');
                focusWorking(reportingDate);
                return;
            }
            else if (compare == 1) {
                alert('Reporting Date cannot be greater than ' + maxAllowedDate.value);
                focusWorking(reportingDate);
                return;
            }

            compare = CompareDates(bookingdate.value, minAllowedDate.value);
            if (isNaN(compare)) {
                alert('Invalid Booking Date');
                focusWorking(bookingdate);
                return;
            }
            else if (compare == -1) {
                alert('Booking Date cannot be less than ' + minAllowedDate.value);
                focusWorking(bookingdate);
                return;
            }

            compare = CompareDates(bookingdate.value, maxAllowedDate.value);
            if (isNaN(compare)) {
                alert('Invalid Booking Date');
                focusWorking(bookingdate);
                return;
            }
            else if (compare == 1) {
                alert('Booking Date cannot be greater than ' + maxAllowedDate.value);
                focusWorking(bookingdate);
                return;
            }

            if (accountNo.value.trim() == "") {
                alert('Enter Account Number');
                focusWorking(accountNo);
                return;
            }
            if (serviceType.options.selectedIndex == 0) {
                alert('Select Service Type');
                focusWorking(serviceType);
                return;
            }

            var combo = $find("<%= dd_destination.ClientID %>");
            var items = combo.get_items();
            if ((combo.get_items()) == null) {
                alert('Invalid Destination Selected');
                focusWorking(destination);
                return;
            }
            if (combo.get_selectedItem()._properties._data.value == "0") {
                alert('Select Destination');
                focusWorking(destination);
                return;
            }
            if (riderCode.value.trim() == "") {
                alert('Enter Rider Code');
                focusWorking(riderCode);
                return;
            }

            if (isNaN(weight.value) || weight.value.trim() == "0") {
                weight.value = "";
                alert('Invalid Weight');
                focusWorking(weight);
                return;
            }
            if (parseFloat(weight.value) <= 0) {
                weight.value = "";
                alert('Invalid Weight');
                focusWorking(weight);
                return;
            }

            if (length.value.trim() != "" && width.value.trim() != "" && height.value.trim() != "") {
                if (isNaN(length.value)) {
                    length.value = "0";
                    alert('Invalid length');
                    focusWorking(length);
                    return;
                }
                if (isNaN(width.value)) {
                    width.value = "0";
                    alert('Invalid width');
                    focusWorking(width);
                    return;
                }
                if (isNaN(height.value)) {
                    height.value = "0";
                    alert('Invalid height');
                    focusWorking(height);
                    return;
                }
            }

            if (isNaN(parseInt(pieces.value)) || pieces.value.trim() == "") {
                pieces.value = "0";
                alert('Invalid pieces');
                focusWorking(pieces);
                return;
            }

            if ((isNaN(chargeAmount.value) || chargeAmount.value.trim() == "" || parseFloat(chargeAmount.value) <= 0) && accountNo.value == "0") {
                alert('Invalid Charge Amount');
                focusWorking(chargeAmount);
                chargeAmount.value = "";
                return;
            }

            //if (originEc.options.selectedIndex == "0") {
            if (txt_ec.value.trim() == "") {
                alert('Invalid Express Center');
                focusWorking(txt_ec);
                return;
            }


            if (priceModifier.options.selectedIndex != 0) {

                if (priceModifier.options[priceModifier.options.selectedIndex].value.split('-')[1] == "3" && (declaredValue.value == "0" || declaredValue.value.trim() == "" || isNaN(parseFloat(declaredValue.value)))) {
                    alert('Invalid Declared Value');
                    return;
                }
                else if (priceModifier.options[priceModifier.options.selectedIndex].value.split('-')[1] == "3") {
                    var tempDeclaredValue = parseFloat(declaredValue.value);
                    if (isNaN(tempDeclaredValue)) {
                        alert('Invalid Declared Value');
                        return;
                    }
                    else if (tempDeclaredValue < 2000) {
                        alert('Declared Value must be greater than 2,000');
                        return;
                    }
                    else if (tempDeclaredValue > 200000) {
                        alert('Declared Value must be lesser than 200,000');
                        return;
                    }
                }
                if ((calValue.value == "0" || calValue.value.trim() == "" || isNaN(parseFloat(calValue.value)))) {
                    alert('Invalid Price Modifier Value');
                    return;
                }
            }
            setDirtyFlag(); //Setting flag true so the user cant accidentaly close the window
            var hdDestination = tr.getElementsByClassName('hdDestination')[0];
            var hdOriginEc = tr.getElementsByClassName('hdOriginEc')[0];
            var hdInsertType = tr.getElementsByClassName('hdInsertType')[0];
            var hdCreditClientID = tr.getElementsByClassName('hdCreditClientID')[0];
            var hdConsignmentType = tr.getElementsByClassName('hdConsignmentType')[0];
            var hdPmID = tr.getElementsByClassName('hdPmID')[0];
            var hdCalBase = tr.getElementsByClassName('hdCalBase')[0];
            var hdCalValue = tr.getElementsByClassName('hdCalValue')[0];
            var modCalValue = tr.getElementsByClassName('modCalValue')[0];
            var calGst = tr.getElementsByClassName('calGst')[0];
            var isTaxable = tr.getElementsByClassName('isTaxable')[0];
            var hdIsApproved = tr.getElementsByClassName('isApproved')[0];

            //            if ((hdIsApproved.value == "1" || hdIsApproved.value.toUpperCase() == "APPROVED") && (approvalStatus.options[approvalStatus.options.selectedIndex].value == "1" || approvalStatus.options[approvalStatus.options.selectedIndex].text.toUpperCase() == "APPROVED")) {
            //                alert("Consignment Already Approved");
            //                return;
            //            }

            var crid = document.getElementById('<%= hd_CreditClientID.ClientID %>');



            tr.cells[2].innerHTML = bookingdate.value;
            tr.cells[3].innerHTML = accountNo.value;
            hdCreditClientID.value = crid.value;
            tr.cells[4].innerHTML = serviceType.options[serviceType.options.selectedIndex].text;
            tr.cells[5].innerHTML = consigner.value;
            tr.cells[6].innerHTML = consignee.value;
            tr.cells[7].innerHTML = consignerCell.value;
            tr.cells[8].innerHTML = consigneeCell.value;
            tr.cells[9].innerHTML = combo.get_selectedItem()._text;
            hdDestination.value = combo.get_selectedItem()._properties._data.value;
            tr.cells[10].innerHTML = riderCode.value;
            tr.cells[11].innerHTML = length.value.toString() + "X" + width.value.toString() + "X" + height.value.toString();
            tr.cells[12].innerHTML = volWeight.value;
            tr.cells[13].innerHTML = denseWeight.value;
            tr.cells[14].innerHTML = weight.value;
            tr.cells[15].innerHTML = pieces.value;
            tr.cells[16].innerHTML = address.value;
            tr.cells[17].innerHTML = originEc.options[originEc.options.selectedIndex].text;
            hdOriginEc.value = txt_ec.value.trim();
            tr.cells[18].innerHTML = cnType.options[cnType.options.selectedIndex].text;
            hdConsignmentType.value = cnType.options[cnType.options.selectedIndex].value;
            tr.cells[19].innerHTML = coupon.value;
            tr.cells[20].innerHTML = remarks.value;
            tr.cells[21].innerHTML = reportingDate.value;
            tr.cells[22].innerHTML = approvalStatus.options[approvalStatus.options.selectedIndex].text;
            tr.cells[25].innerHTML = orderRefNo.value;
            tr.cells[26].innerHTML = descriptionCOD.value;
            tr.cells[27].innerHTML = codAmount.value;
            if (priceModifier.options.selectedIndex != 0) {
                tr.cells[28].innerHTML = priceModifier.options[priceModifier.options.selectedIndex].text;
                hdCalBase.value = priceModifier.options[priceModifier.options.selectedIndex].value.split('-')[1];
                hdCalValue.value = calValue.value;
                hdPmID.value = priceModifier.options[priceModifier.options.selectedIndex].value.split('-')[0];
                modCalValue.value = calValue.value;
                calGst.value = "";
                isTaxable.value = "";
            }
            else {
                tr.cells[28].innerHTML = "";
                hdCalBase.value = "";
                hdCalValue.value = "";
                hdPmID.value = "";
                modCalValue.value = "";
                calGst.value = "";
                isTaxable.value = "";
            }


            tr.cells[29].innerHTML = declaredValue.value;
            tr.cells[30].innerHTML = "";
            tr.cells[31].innerHTML = chargeAmount.value;
            tr.cells[32].innerHTML = "";
            tr.cells[33].innerHTML = "";
            ResetUpper();
            ResetDimTable();
            var maxIndex = table.rows.length - 1;
            if (tr.rowIndex != maxIndex) {
                EditConsignment(table.rows[tr.rowIndex + 1]);
            }

            if (!chkBookingDate.checked) {
                focusWorking(bookingdate);
            }
            else if (!chkRiderCode.checked) {
                focusWorking(riderCode);
            }
            else if (!chkAccountNo.checked) {
                focusWorking(accountNo);
            }
            else if (!chk_lockConsigner.checked) {
                focusWorking(consigner);
            }
            else if (!chk_lockConsignee.checked) {
                focusWorking(consignee);
            }
            else if (!chkServiceType.checked) {
                focusWorking(serviceType);
            }
            else {
                focusWorking(chkServiceType);
            }

        }
        function ResetDimTable() {
            var tbl = document.getElementById('tblDim');
            for (var i = 1; i < tbl.rows.length;) {
                tbl.deleteRow(1);
            }
            AddDimensionRow();

        }
    </script>
    <%--Comparing Dates--%>
    <script type="text/javascript">
        function CompareDates(a, b) {
            var aYear = parseInt(a.split('/')[2]);
            var aMonth = parseInt(a.split('/')[1]);
            var aDay = parseInt(a.split('/')[0]);

            var bYear = parseInt(b.split('/')[2]);
            var bMonth = parseInt(b.split('/')[1]);
            var bDay = parseInt(b.split('/')[0]);

            if (isNaN(aYear) || isNaN(aMonth) || isNaN(aDay) || isNaN(bYear) || isNaN(bMonth) || isNaN(bDay)) {
                return NaN;
            }

            if (aYear == bYear) {
                if (aMonth == bMonth) {
                    if (aDay == bDay) {
                        return -1;
                    }
                    else if (aDay > bDay) {
                        return 1;
                    }
                    else {
                        return -1;
                    }
                }
                else if (aMonth > bMonth) {
                    return 1;
                }
                else {
                    return -1;
                }
            }
            else if (aYear > bYear) {
                return 1;
            }
            else {
                return -1;
            }
        }
    </script>
    <%-- Different Validations --%>
    <script type="text/javascript">
        function ValidateRider() {
            loader.style.display = 'block';

            var ridercode = document.getElementById('<%= txt_riderCode.ClientID %>');
            var accNo = document.getElementById('<%= txt_accountNo.ClientID %>');
            var cnFrom = document.getElementById('<%= txt_CnStart.ClientID %>');
            var cnTo = document.getElementById('<%= txt_NumberofCN.ClientID %>');
            var originEC = document.getElementById('<%= txt_originExpressCenter.ClientID %>');
            var chk_ec = document.getElementById('<%= chk_lockOriginEC.ClientID %>');
            PageMethods.ValidateRider(ridercode.value, ValidateRiderServerResponse);
            if (/*(accNo.value.trim() == "0" && cnFrom.value.trim() != "") || */(ridercode.value.trim() != "" && (accNo.value.trim() != "0" && accNo.value.trim() != ""))) {
                PageMethods.GetEC(accNo.value, ridercode.value, cnFrom.value, getEcFromServer);
                originEC.disabled = true;
                chk_ec.disabled = true;
            }
            else {
                chk_ec.disabled = false;
                if (chk_ec.checked) {
                    originEC.disabled = true;
                }
                else {
                    originEC.disabled = false;
                }

            }
            loader.style.display = 'none';

        }
        function getEcFromServer(response, userContext, methodName) {
            var a = '0';

            var accNo = document.getElementById('<%= txt_accountNo.ClientID %>');
            var ridercode = document.getElementById('<%= txt_riderCode.ClientID %>');
            var cnFrom = document.getElementById('<%= txt_CnStart.ClientID %>');
            var cnTo = document.getElementById('<%= txt_NumberofCN.ClientID %>');
            var dd_originExpressCenter = document.getElementById('<%= dd_originExpressCenter.ClientID %>');
            var originEC = document.getElementById('<%= txt_originExpressCenter.ClientID %>');
            var chk_ec = document.getElementById('<%= chk_lockOriginEC.ClientID %>');
            dd_originExpressCenter.disabled = true;

            if (response[0].toString() == "0") {
                alert(response[1].toString() + ' Enter Express Center Code');
                focusWorking(dd_originExpressCenter);
                validEC = false;
                dd_originExpressCenter.options.selectedIndex = 0;
                dd_originExpressCenter.disabled = false;
                originEC.disabled = false;
                return;
            }
            else if (response[0].toString() == "-1") {
                ridercode.value = "";
                focusWorking(ridercode);
                validEC = false;
                dd_originExpressCenter.options.selectedIndex = 0;
                originEC.disabled = false;
                return;
            }
            else {
                validEC = true;
                originEC.disabled = true;
                originEC.value = response[1].toString();
                for (var i = 0; i < dd_originExpressCenter.options.length; i++) {
                    if (dd_originExpressCenter.options[i].value == response[1].toString()) {
                        dd_originExpressCenter.options[i].selected = true;
                    }
                }
            }
            loader.style.display = 'none';
        }
        function ValidateRiderServerResponse(response, userContext, methodName) {
            var txt = document.getElementById('<%= txt_riderCode.ClientID %>');



            if (response.toString().toUpperCase() != 'OK') {
                txt.value = "";
                //txt.focus();
                focusWorking(txt);
                alert(response.toString());
            }
        }
        function ValidateAccount() {

            loader.style.display = 'block';
            var accNo = document.getElementById('<%= txt_accountNo.ClientID %>');
            var ridercode = document.getElementById('<%= txt_riderCode.ClientID %>');
            var cnFrom = document.getElementById('<%= txt_CnStart.ClientID %>');
            var cnTo = document.getElementById('<%= txt_NumberofCN.ClientID %>');
            var chrgAmt = document.getElementById('<%= txt_chargedAmount.ClientID %>');
            var originEC = document.getElementById('<%= txt_originExpressCenter.ClientID %>');
            var chk_ec = document.getElementById('<%= chk_lockOriginEC.ClientID %>');
            if (accNo.value.trim() == "") {
                alert('Enter Account Number');
                loader.style.display = 'none';
                focusWorking(accNo);
                return;
            }

            if (accNo.value.trim() == "0") {
                chk_ec.disabled = false;
                if (chk_ec.checked) {
                    originEC.disabled = true;

                }
                else {
                    originEC.disabled = false;

                }


                chrgAmt.disabled = false;
                chrgAmt.style.backgroundColor = '#FFFFFF';
            }
            else {
                originEC.disabled = true;
                chk_ec.disabled = true;
                chrgAmt.disabled = true;
                chrgAmt.style.backgroundColor = '#EBEBE5';
            }

            PageMethods.ValidateAccount(accNo.value, ValidateAccountServerResponse);

            PageMethods.GetServices(accNo.value, GetServicesServerResponse);

            if (/*(accNo.value.trim() == "0" && cnFrom.value.trim() != "") || */(ridercode.value.trim() != "" && accNo.value.trim() != "0")) {
                loader.style.display = 'block';
                PageMethods.GetEC(accNo.value, ridercode.value, cnFrom.value, getEcFromServer);
            }

        }
        function ValidateAccountServerResponse(response, userContext, methodName) {
            var tblConsignment = document.getElementById('tblConsignments');
            var consignmentNumber = document.getElementById('<%= txt_cnNumber.ClientID %>');
            var txt = document.getElementById('<%= txt_accountNo.ClientID %>');
            var name = document.getElementById('<%= txt_consigner.ClientID %>');
            var phone = document.getElementById('<%= txt_consignerCell.ClientID %>');
            var chk = document.getElementById('<%= Cb_CODAmount.ClientID %>');
            var crId = document.getElementById('<%= hd_CreditClientID.ClientID %>');
            var reportingDate = document.getElementById('<%= txt_reportingDate.ClientID %>');
            var maxAllowedDate = document.getElementById('<%= hd_maxAllowedDate.ClientID %>');
            var chkReportingDate = document.getElementById('<%= chk_reportingDateFreeze.ClientID %>');

            var hd_newCN = document.getElementById('<%= hd_newCN.ClientID %>');

            if (response[0].toString().toUpperCase() != '1') {
                txt.value = "";
                //txt.focus();
                focusWorking(txt);
                name.value = "";
                phone.value = "";
                crId.value = "0";
                chk.checked = false;
                alert(response[1].toString());

                chkReportingDate.disabled = false;
            }
            else {
                if (txt.value.trim() != "0") {
                    phone.value = response[1].toString();
                    name.value = response[2].toString();

                }
                else {


                    LockReportingDate(chkReportingDate);
                }
                crId.value = response[5].toString();
                var isCOD = response[4].split('-')[0];
                var CODType = response[4].split('-')[1];

                if (hd_newCN.value == "Y") {
                    if ((isCOD == "1" || isCOD.toString().toUpperCase() == "TRUE") && CODType == "2") {

                        chk.checked = true;
                    }
                    else if ((isCOD == "1" || isCOD.toString().toUpperCase() == "TRUE") && CODType != "2") {
                        chk.checked = false;
                        alert('New Consignment for COD Accounts other than 2Pay are not Allowed.');
                        txt.value = "";
                        phone.value = "";
                        name.value = "";
                    }
                }
                else {
                    if ((isCOD == "1" || isCOD.toString().toUpperCase() == "TRUE") && CODType == "2") {
                        chk.checked = true;
                    }
                    else if ((isCOD == "1" || isCOD.toString().toUpperCase() == "TRUE") && CODType != "2") {
                        chk.checked = false;
                        for (var i = 1; i < tblConsignment.rows.length; i++) {
                            var row = tblConsignment.rows[i];
                            if (row.cells[1].innerText.trim() == consignmentNumber.value.trim()) {
                                if (row.cells[3].innerText.trim() != txt.value) {
                                    alert('Account Number cannot be changed');
                                    txt.value = row.cells[3].innerText;
                                    crId.value = row.getElementsByClassName('hdCreditClientID')[0].value;
                                    phone.value = row.cells[7].innerText;
                                    name.value = row.cells[5].innerText;
                                    break;
                                }
                            }
                        }
                    }
                    else {

                        for (var i = 1; i < tblConsignment.rows.length; i++) {
                            var row = tblConsignment.rows[i];
                            if (row.cells[1].innerText.trim() == consignmentNumber.value.trim()) {
                                var isCODCN = row.getElementsByClassName('isCODCN')[0];
                                if ((isCODCN.value.toUpperCase() == "TRUE" || isCODCN.value.toUpperCase() == "1") && (row.cells[3].innerText.trim() != txt.value)) {
                                    alert('Account Number cannot be changed');
                                    txt.value = row.cells[3].innerText;
                                    crId.value = row.getElementsByClassName('hdCreditClientID')[0].value;
                                    phone.value = row.cells[7].innerText;
                                    name.value = row.cells[5].innerText;
                                    break;
                                }
                            }
                        }
                        chk.checked = false;
                    }
                }

            }
            loader.style.display = 'none';
        }

        function GetServicesServerResponse(response, userContext, methodName) {
            var servicesFound = response.Status;
            var Message = response.Message;
            var txt = document.getElementById('<%= txt_accountNo.ClientID %>');
            var chkService = document.getElementById('<%= chk_service.ClientID %>');

            var dd_service = document.getElementById('<%= dd_serviceType.ClientID %>');
            var currentSelectedService = dd_service.options[dd_service.options.selectedIndex].value;

            dd_service.options.length = 0;

            var option = document.createElement('option');
            option.value = '0';
            option.text = 'Select Service';
            dd_service.add(option);
            var selectedIndex = 0;
            if (servicesFound == "0") {
                alert(Message);
                focusWorking(txt);
                txt.select();
                return;
            }

            for (var i = 0; i < response.Services.length; i++) {
                var service = document.createElement('option');
                service.value = response.Services[i].ServiceTypeName;
                service.text = response.Services[i].ServiceTypeName;
                if (service.value == currentSelectedService) {
                    selectedIndex = i + 1;
                }
                dd_service.add(service);
            }

            dd_service.selectedIndex = selectedIndex;

            if (selectedIndex == 0) {
                chkService.checked = false;
                LockService(chkService);
            }
            else {
                ChangeMaxLength(dd_service);
            }

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
        function focusSkip() {
            var btn = document.getElementById('btn_add');
            focusWorking(btn);
        }
    </script>
    <%--Posting After Every 5 Minutes to Keep sessions fresh --%>
    <script>

        var myVar = setInterval(myTimer, 150000);

        function myTimer() {
            PageMethods.RefreshTime("", OnSuccess2);
        }
        function OnSuccess2(response, userContext, methodName) {
            //            var bookingDate = document.getElementById('<%= txt_bookingDate.ClientID %>');
            //            bookingDate.value = response.toString();

            var minAllowedDate = document.getElementById('<%= hd_minAllowedDate.ClientID %>');
            var maxAllowedDate = document.getElementById('<%= hd_maxAllowedDate.ClientID %>');
            maxAllowedDate = response.toString();
        }
        function formatNumber(num) {
            return num.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,")
        }
    </script>
    <%--Resetting Methods--%>
    <script type="text/javascript">
        function ResetUpper() {
            var minAllowedDate = document.getElementById('<%= hd_minAllowedDate.ClientID %>');
            var maxAllowedDate = document.getElementById('<%= hd_maxAllowedDate.ClientID %>');

            var bookingdate = document.getElementById('<%= txt_bookingDate.ClientID %>');
            var chkBookingDate = document.getElementById('<%= chk_BookingDateFreeze.ClientID %>');
            if (!chkBookingDate.checked) {
                bookingdate.value = maxAllowedDate.value;
            }

            var cnNumber = document.getElementById('<%= txt_cnNumber.ClientID %>');
            cnNumber.value = "";
            var accountNo = document.getElementById('<%= txt_accountNo.ClientID %>');
            var chkAccountNo = document.getElementById('<%= chk_accountNoFreeze.ClientID %>');
            if (!chkAccountNo.checked) {
                accountNo.value = "";
            }

            var serviceType = document.getElementById('<%= dd_serviceType.ClientID %>');
            var chkServiceType = document.getElementById('<%= chk_service.ClientID %>');
            if (!chkServiceType.checked) {
                serviceType.options.selectedIndex = 0;

            }
            var chk_lockConsigner = document.getElementById('<%= chk_lockConsigner.ClientID %>');
            if (!chk_lockConsigner.checked) {
                var consigner = document.getElementById('<%= txt_consigner.ClientID %>');
                consigner.value = "";
                var consignerCell = document.getElementById('<%= txt_consignerCell.ClientID %>');
                consignerCell.value = "";
            }

            var chk_lockConsignee = document.getElementById('<%= chk_lockConsignee.ClientID %>');
            if (!chk_lockConsignee.checked) {
                var consignee = document.getElementById('<%= txt_consignee.ClientID %>');
                consignee.value = "";
                var consigneeCell = document.getElementById('<%= txt_consigneeCell.ClientID %>');
                consigneeCell.value = "";
            }


            var destination = $find("<%= dd_destination.ClientID %>");

            var items = destination.get_items();
            var itm = destination.findItemByValue('0');
            if (itm != null) {
                itm.select();
            }
            var riderCode = document.getElementById('<%= txt_riderCode.ClientID %>');
            var chkRiderCode = document.getElementById('<%= chk_riderCodeFreeze.ClientID %>');
            if (!chkRiderCode.checked) {
                riderCode.value = "";
            }

            var length = document.getElementById('<%= txt_l.ClientID %>');
            length.value = "";
            var width = document.getElementById('<%= txt_w.ClientID %>');
            width.value = "";
            var height = document.getElementById('<%= txt_h.ClientID %>');
            height.value = "";
            var volWeight = document.getElementById('<%= txt_vWeight.ClientID %>');
            volWeight.value = "";
            var denseWeight = document.getElementById('<%= txt_aWeight.ClientID %>');
            denseWeight.value = "";
            var weight = document.getElementById('<%= txt_weight.ClientID %>');
            weight.value = "";
            var pieces = document.getElementById('<%= txt_pieces.ClientID %>');
            pieces.value = "";
            var chk_lockAddress = document.getElementById('<%= chk_lockAddress.ClientID %>');
            if (!chk_lockAddress.checked) {
                var address = document.getElementById('<%= txt_Address.ClientID %>');
                address.value = "";
            }

            var originEc = document.getElementById('<%= dd_originExpressCenter.ClientID %>');
            originEc.options.selectedIndex = 0;
            var cnType = document.getElementById('<%= dd_consignmentType.ClientID %>');
            var chk_cnType = document.getElementById('<%= chk_cnType.ClientID %>');
            if (!chk_cnType.checked) {
                cnType.options.selectedIndex = 0;
            }

            var coupon = document.getElementById('<%= txt_coupon.ClientID %>');
            coupon.value = "";
            var remarks = document.getElementById('<%= txt_remarks.ClientID %>');
            remarks.value = "";
            var reportingDate = document.getElementById('<%= txt_reportingDate.ClientID %>');
            var chkReportingDate = document.getElementById('<%= chk_reportingDateFreeze.ClientID %>');
            if (!chkReportingDate.checked) {
                reportingDate.value = maxAllowedDate.value;
            }
            var approvalStatus = document.getElementById('<%= dd_approvalStatus.ClientID %>');
            var chkApprovalStatus = document.getElementById('<%= chk_approval.ClientID %>');

            var invoiceStatus = document.getElementById('<%= txt_invoiceStatus.ClientID %>');
            invoiceStatus.value = "";
            var invoiceNumber = document.getElementById('<%= txt_invoiceNumber.ClientID %>');
            invoiceNumber.value = "";
            var chargeAmount = document.getElementById('<%= txt_chargedAmount.ClientID %>');
            chargeAmount.value = "";
            var totalAmount = document.getElementById('<%= txt_totalAmt.ClientID %>');
            totalAmount.value = "";
            var gst = document.getElementById('<%= txt_gst.ClientID %>');
            gst.value = "";
            var orderRefNo = document.getElementById('<%= txt_orderRefNo.ClientID %>');
            orderRefNo.value = "";
            var descriptionCOD = document.getElementById('<%= txt_descriptionCOD.ClientID %>');
            descriptionCOD.value = "";
            var codAmount = document.getElementById('<%= txt_codAmount.ClientID %>');
            codAmount.value = "";
            var chkCod = document.getElementById('<%= chk_cod.ClientID %>');
            var priceModifier = document.getElementById('<%= dd_priceModifier.ClientID %>');
            var calBase = document.getElementById('<%= dd_calculationBase.ClientID %>');
            var calValue = document.getElementById('<%= txt_priceModifierValue.ClientID %>');
            var declaredValue = document.getElementById('<%= txt_declaredValue.ClientID %>');

            var chkPm = document.getElementById('<%= chk_pm.ClientID %>');
            if (!chkPm.checked) {
                priceModifier.options.selectedIndex = 0;
                for (var i = 0; i < calBase.rows[0].cells.length; i++) {
                    calBase.rows[0].cells[i].children[0].children[0].checked = false;
                }
                calValue.value = "";
                declaredValue.value = "";
            }
        }
    </script>
    <%--Saving Consignments--%>
    <script type="text/javascript">
        function SaveToDB() {
            loader.style.display = 'block';
            var table = document.getElementById('tblConsignments');
            var details = { Consignments: [], cnDimensions: [] }

            for (var i = 1; i < table.rows.length; i++) {

                var tr = table.rows[i];
                var chkRemove = tr.getElementsByClassName('chkRemove')[0];
                if (chkRemove.checked) {

                    debugger;
                    var hdDestination = tr.getElementsByClassName('hdDestination')[0];
                    var hdOriginEc = tr.getElementsByClassName('hdOriginEc')[0];
                    var hdInsertType = tr.getElementsByClassName('hdInsertType')[0];
                    var hdCreditClientID = tr.getElementsByClassName('hdCreditClientID')[0];
                    var hdConsignmentType = tr.getElementsByClassName('hdConsignmentType')[0];
                    var hdPmID = tr.getElementsByClassName('hdPmID')[0];
                    var hdCalBase = tr.getElementsByClassName('hdCalBase')[0];
                    var hdCalValue = tr.getElementsByClassName('hdCalValue')[0];
                    var modCalValue = tr.getElementsByClassName('modCalValue')[0];
                    var calGst = tr.getElementsByClassName('calGst')[0];
                    var isTaxable = tr.getElementsByClassName('isTaxable')[0];
                    var hdCod = tr.getElementsByClassName('isCODCN')[0];
                    var cnReportingDate = tr.cells[21].innerHTML;
                    var cnBookingDate = tr.cells[2].innerHTML;

                    var minAllowedDate = document.getElementById('<%= hd_minAllowedDate.ClientID %>');
                    var maxAllowedDate = document.getElementById('<%= hd_maxAllowedDate.ClientID %>');

                    var compare = CompareDates(cnReportingDate, minAllowedDate.value);
                    if (isNaN(compare)) {

                    }
                    else if (compare == -1) {
                        alert('Consignment: ' + tr.cells[1].innerHTML + ' has been Closed.');

                        loader.style.display = 'none';
                        return;
                    }
                    compare = "";
                    compare = CompareDates(cnBookingDate, minAllowedDate.value);
                    if (isNaN(compare)) {

                    }
                    else if (compare == -1) {
                        alert('Consignment: ' + tr.cells[1].innerHTML + ' has been Closed.');

                        loader.style.display = 'none';
                        return;
                    }
                    var consignment = {
                        CN: tr.cells[1].innerText,
                        BKDate: tr.cells[2].innerText,
                        Acc: tr.cells[3].innerText,
                        InsertType: hdInsertType.value,
                        CreditClientID: hdCreditClientID.value,
                        ServiceType: tr.cells[4].innerText,
                        Consigner: tr.cells[5].innerText,
                        Consignee: tr.cells[6].innerText,
                        ConsignerMob: tr.cells[7].innerText,
                        ConsigneeMob: tr.cells[8].innerText,
                        Destination: hdDestination.value,
                        DestinationName: tr.cells[9].innerText,
                        Rider: tr.cells[10].innerText,
                        Dimensions: tr.cells[11].innerText,
                        VolWgt: tr.cells[12].innerText,
                        DnsWgt: tr.cells[13].innerText,
                        Weight: tr.cells[14].innerText,
                        Pieces: tr.cells[15].innerText,
                        Address: tr.cells[16].innerText,
                        OriginEC: hdOriginEc.value,
                        OriginECName: tr.cells[17].innerText,
                        CNType: hdConsignmentType.value,
                        CNTypeName: tr.cells[18].innerText,
                        Coupon: tr.cells[19].innerHTML,
                        SpecialInstructions: tr.cells[20].innerText,
                        RPDate: tr.cells[21].innerText,
                        Approved: tr.cells[22].innerText,
                        InvStatus: tr.cells[23].innerText,
                        InvNumber: tr.cells[24].innerText,
                        CODRef: tr.cells[25].innerText,
                        CODDesc: tr.cells[26].innerText,
                        CODAmt: tr.cells[27].innerText,
                        AddServ: tr.cells[28].innerText,
                        pmID: hdPmID.value,
                        calBase: hdCalBase.value,
                        calValue: hdCalValue.value,
                        modCalValue: modCalValue.value,
                        calGst: calGst.value,
                        isTaxable: isTaxable.value,
                        AltValue: tr.cells[29].innerText,
                        AddChrg: tr.cells[30].innerText,
                        ChrgAmt: tr.cells[31].innerText,
                        TotalAmt: tr.cells[32].innerText,
                        Gst: tr.cells[33].innerText,
                        isCOD: hdCod.value,
                        shipperAddress: tr.cells[34].innerText,
                        PakageContents: tr.cells[35].innerText
                    };
                    details.Consignments.push(consignment);
                }
            }
            var tblcnw = document.getElementById('cnDim');
            for (var j = 1; j < tblcnw.rows.length; j++) {
                var row = tblcnw.rows[j];
                var dimension = {
                    ConsignmentNumber: row.cells[0].innerText,
                    ItemNumber: row.cells[1].innerText,
                    Length: row.cells[2].innerText,
                    Width: row.cells[3].innerText,
                    Height: row.cells[4].innerText,
                    Pieces: row.cells[5].innerText,
                    VolWeight: row.cells[6].innerText
                }

                details.cnDimensions.push(dimension);
            }

            if (details.Consignments.length > 0) {
                $.ajax({

                    url: 'BulkConsignment_NewSequences.aspx/SaveToDb',
                    type: 'POST',
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    data: JSON.stringify(details),
                    success: function (result) {
                        var errorList = { errors: [] };

                        var resp = result.d;
                        var errorFound = false;
                        for (var i = 0; i < resp.length; i++) {
                            var error;
                            if (resp[i].ServerResponse != "OK" && resp[i].ServerResponse != null) {
                                errorFound = true;
                                error = {
                                    ConsignmentNumber: resp[i].CN,
                                    Message: resp[i].ServerResponse
                                };
                                errorList.errors.push(error);
                            }
                        }
                        if (errorList.errors.length > 0) {
                            var errorMessage = "";
                            for (var i = 0; i < errorList.errors.length; i++) {
                                errorMessage = errorMessage + errorList.errors[i].ConsignmentNumber + ": " + errorList.errors[i].Message + "\n";
                            }
                            alert(errorMessage);
                            loader.style.display = 'none';
                            return;
                        }
                        else {
                            var cnFrom = document.getElementById('<%= txt_CnStart.ClientID %>');
                            var numberOfCN = document.getElementById('<%= txt_NumberofCN.ClientID %>');
                            cnFrom.value = "";
                            numberOfCN.value = "";
                            var tblConsignments = document.getElementById('tblConsignments');
                            ClearTable(tblConsignments);
                            ResetUpper();
                            alert('Consignments Saved');
                        }
                        loader.style.display = 'none';
                        releaseDirtyFlag();
                    },
                    failure: function () { alert('Oops! Something Went Wrong. Please Contact I.T. Support') },
                    error: function () { alert('Oops! Something Went Wrong. Please Contact I.T. Support') },
                });
            }
            else {
                alert('No Consignment Selected.');
                loader.style.display = 'none';
            }

        }
    </script>
    <%--Dates Change--%>
    <script type="text/javascript">
        function BookingDateChange(dd) {
            var maxAllowedDate = document.getElementById('<%= hd_maxAllowedDate.ClientID %>');
            var minAllowedDate = document.getElementById('<%= hd_minAllowedDate.ClientID %>');
	    maxAllowedDate.value = maxAllowedDate.value.replaceAll('-', '/');	
            minAllowedDate.value = minAllowedDate.value.replaceAll('-', '/');
            compare = CompareDates(dd.value, minAllowedDate.value);
            if (isNaN(compare)) {
                alert('Invalid Booking Date');
                dd.value = "";
                focusWorking(dd);
                return;
            }
            else if (compare == -1) {
                alert('Booking Date cannot be less than ' + minAllowedDate.value);
                dd.value = "";
                focusWorking(bookingdate);
                return;
            }
            maxAllowedDate.value = maxAllowedDate.value.replaceAll('-', '/');	
            minAllowedDate.value = minAllowedDate.value.replaceAll('-', '/');
            compare = CompareDates(dd.value, maxAllowedDate.value);
            if (isNaN(compare)) {
                alert('Invalid Booking Date');
                dd.value = "";
                focusWorking(dd);
                return;
            }
            else if (compare == 1) {
                alert('Booking Date cannot be greater than ' + maxAllowedDate.value);
                dd.value = "";
                focusWorking(dd);
                return;
            }
        }


        function ReportingDateChange(dd) {

            var maxAllowedDate = document.getElementById('<%= hd_maxAllowedDate.ClientID %>');
            var minAllowedDate = document.getElementById('<%= hd_minAllowedDate.ClientID %>');

            compare = CompareDates(dd.value, minAllowedDate.value);
            if (isNaN(compare)) {
                alert('Invalid Reporting Date');
                dd.value = "";
                focusWorking(dd);
                return;
            }
            else if (compare == -1) {
                alert('Reporting Date cannot be less than ' + minAllowedDate.value);
                dd.value = "";
                focusWorking(bookingdate);
                return;
            }

            compare = CompareDates(dd.value, maxAllowedDate.value);
            if (isNaN(compare)) {
                alert('Invalid Reporting Date');
                focusWorking(dd);
                return;
            }
            else if (compare == 1) {
                alert('Reporting Date cannot be greater than ' + maxAllowedDate.value);
                dd.value = "";
                focusWorking(dd);
                return;
            }
        }
    </script>
    <%--Price Modifiers Related Methods--%>
    <script type="text/javascript">
        function PMChange(dd) {
            if (dd.options.selectedIndex == "0") {
                return;
            }
            var pmID = dd.options[dd.options.selectedIndex].value.split('-')[0];
            var pmName = dd.options[dd.options.selectedIndex].text;
            var pmBase = dd.options[dd.options.selectedIndex].value.split('-')[1];
            var pmValue = dd.options[dd.options.selectedIndex].value.split('-')[2];
            var calBase = document.getElementById('<%= dd_calculationBase.ClientID %>');
            var calValue = document.getElementById('<%= txt_priceModifierValue.ClientID %>');
            var declaredValue = document.getElementById('<%= txt_declaredValue.ClientID %>');

            for (var i = 0; i < calBase.rows[0].cells.length; i++) {
                if (calBase.rows[0].cells[i].children[0].children[0].value == pmBase) {
                    calBase.rows[0].cells[i].children[0].children[0].checked = true;
                    break;
                }
            }

            calValue.value = pmValue;
            if (pmValue.trim() == "0" || pmValue.trim() == "") {
                calValue.disabled = false;
                calValue.style.backgroundColor = '#FFFFFF';
            }
            else {
                calValue.disabled = true;
                calValue.style.backgroundColor = '#EBEBE5';
            }

            if (pmBase == "3") {
                declaredValue.disabled = false;
                declaredValue.style.backgroundColor = '#FFFFFF';
            }
            else {
                declaredValue.disabled = true;
                declaredValue.style.backgroundColor = '#EBEBE5';
            }

        }
    </script>
    <script type="text/javascript">
        var needToConfirm = false;

        function setDirtyFlag() {
            needToConfirm = true; //Call this function if some changes is made to the web page and requires an alert
            // Of-course you could call this is Keypress event of a text box or so...
        }

        function releaseDirtyFlag() {
            needToConfirm = false; //Call this function if dosent requires an alert.
            //this could be called when save button is clicked 
        }


        window.onbeforeunload = confirmExit;
        function confirmExit() {
            if (needToConfirm) {
                return "The Changes you have made will not be Saved. Are you sure you want to Leave this Page";

            }

        }



    </script>
    <script>
        function FocusToUpdateChanges(evt) {
            var a = 0;
        }
    </script>
    <%-- On Loading of Window --%>
    <%-- Open Dimension Div--%>
    <script type="text/javascript">

        var firstTime = true;
        function OpenDimensionsDiv() {
            var tblDim = document.getElementById('tblDim');
            var divInnerConsignee = document.getElementById('divDim');
            var divConsignee = document.getElementById('divConsignee');
            divConsignee.style.display = 'block';
            divInnerConsignee.style.display = 'block';


            focusWorking(tblDim.rows[1].cells[1].childNodes[0]);
            tblDim.rows[1].cells[1].childNodes[0].select();
            return false;
        }
    </script>
    <%-- Creating First Row of Dimensions--%>
    <script type="text/javascript">
        var tabIndexForImageButton = 20000;
        window.onload = AddDimensionRow;
        function AddDimensionRow() {
            var tbl = document.getElementById('tblDim');
            var newRow = tbl.insertRow(tbl.rows.length);
            newRow.className = 'DimRow';

            var img1 = document.createElement('input');
            img1.type = 'image';
            img1.setAttribute("src", "../images/OrangeDelete.png");

            var td0 = newRow.insertCell(0);
            td0.innerText = tbl.rows.length - 1;


            var td1 = newRow.insertCell(1);
            var txt = document.createElement('input');
            txt.className = 'textBox';
            //txt.id = (tbl.rows.length - 1).toString() + 'txt1';
            txt.style.width = '90%';
            txt.onchange = VolumeChangePerLine.bind(txt, txt);
            txt.onkeypress = function () { return isNumberKey(event); };
            txt.value = '0';
            td1.appendChild(txt);

            var td2 = newRow.insertCell(2);
            txt = document.createElement('input');
            //txt.id = 'txt2' + (tbl.rows.length - 1).toString();
            txt.className = 'textBox';
            txt.style.width = '90%';
            txt.onchange = VolumeChangePerLine.bind(txt, txt);
            txt.onkeypress = function () { return isNumberKey(event); };
            txt.value = '0';
            td2.appendChild(txt);

            var td3 = newRow.insertCell(3);
            txt = document.createElement('input');
            //txt.id = 'txt3' + (tbl.rows.length - 1).toString();
            txt.className = 'textBox';
            txt.style.width = '90%';
            txt.onchange = VolumeChangePerLine.bind(txt, txt);
            txt.onkeypress = function () { return isNumberKey(event); };
            txt.value = '0';
            td3.appendChild(txt);

            var td4 = newRow.insertCell(4);
            txt = document.createElement('input');
            //txt.id = 'txt3' + (tbl.rows.length - 1).toString();
            txt.className = 'textBox';
            txt.style.width = '90%';
            txt.onchange = VolumeChangePerLine.bind(txt, txt);
            txt.onkeypress = function () { return isNumberKey(event); };
            txt.value = '1';
            td4.appendChild(txt);

            var td5 = newRow.insertCell(5);

            var td6 = newRow.insertCell(6);
            img1.onclick = function () { return RemoveSingleRow(newRow); }
            img1.tabIndex = tabIndexForImageButton + 1;
            td6.appendChild(img1);

            for (var i = 1; i < tbl.rows.length; i++) {
                tbl.rows[i].cells[1].childNodes[0].id = 'txt' + tbl.rows[i].rowIndex.toString() + '1';
                tbl.rows[i].cells[2].childNodes[0].id = 'txt' + tbl.rows[i].rowIndex.toString() + '2';
                tbl.rows[i].cells[3].childNodes[0].id = 'txt' + tbl.rows[i].rowIndex.toString() + '3';
                tbl.rows[i].cells[4].childNodes[0].id = 'txt' + tbl.rows[i].rowIndex.toString() + '4';
            }

            if (!firstTime) {
                focusWorking(td1.childNodes[0]);
                td1.childNodes[0].select();
            }
            else {
                firstTime = false;
                focusWorking(document.getElementById('<%= txt_CnStart.ClientID %>'));
            }

        }
    </script>
    <%-- Closing Consignee Div--%>
    <script type="text/javascript">
        function CloseConsigneeDetails() {
            var denseWeight = document.getElementById('<%= txt_aWeight.ClientID %>');
            var txtweight = document.getElementById('<%= txt_weight.ClientID %>');
            var txtPieces = document.getElementById('<%= txt_pieces.ClientID %>');
            var tblDim = document.getElementById('tblDim');
            var divInnerConsignee = document.getElementById('divDim');
            var divConsignee = document.getElementById('divConsignee');
            divConsignee.style.display = 'none';
            divInnerConsignee.style.display = 'none';

            var tblCnDims = document.getElementById('cnDim');
            var txt_cn = document.getElementById('<%= txt_cnNumber.ClientID %>');

            for (var i = 1; i < tblCnDims.rows.length; i++) {
                if (cnDim.rows[i].cells[0].innerText == txt_cn.value) {
                    cnDim.deleteRow(i);
                    i--;
                }
            }
            var totalWeight = 0;
            var totalPieces = 0;

            for (var i = 1; i < tblDim.rows.length; i++) {
                var tr = tblCnDims.insertRow(tblCnDims.rows.length);

                tr.insertCell(0);
                tr.insertCell(1);
                tr.insertCell(2);
                tr.insertCell(3);
                tr.insertCell(4);
                tr.insertCell(5);
                tr.insertCell(6);

                tr.cells[0].innerText = txt_cn.value;
                tr.cells[1].innerText = tblDim.rows[i].cells[0].innerText;
                tr.cells[2].innerText = tblDim.rows[i].cells[1].childNodes[0].value;
                tr.cells[3].innerText = tblDim.rows[i].cells[2].childNodes[0].value;
                tr.cells[4].innerText = tblDim.rows[i].cells[3].childNodes[0].value;
                tr.cells[5].innerText = tblDim.rows[i].cells[4].childNodes[0].value;

                var length = parseFloat(tblDim.rows[i].cells[1].childNodes[0].value);
                var width = parseFloat(tblDim.rows[i].cells[2].childNodes[0].value);
                var height = parseFloat(tblDim.rows[i].cells[3].childNodes[0].value);
                var pieces = parseFloat(tblDim.rows[i].cells[4].childNodes[0].value);

                if ((!isNaN(length)) && (!isNaN(width)) && (!isNaN(height)) && (!isNaN(pieces))) {
                    tr.cells[6].innerText = ((length * width * height) / 5000) * pieces;
                    totalWeight = totalWeight + (((length * width * height) / 5000) * pieces);
                    totalPieces = totalPieces + pieces;
                    tr.cells[2].innerText = length.toString();
                    tr.cells[3].innerText = width.toString();
                    tr.cells[4].innerText = height.toString();

                }
                else {
                    tr.cells[5].innerText = '0';
                    tr.cells[2].innerText = '0';
                    tr.cells[3].innerText = '0';
                    tr.cells[4].innerText = '0';
                }


            }


            var totalVolWeight = document.getElementById('<%= txt_vWeight.ClientID %>');
            totalVolWeight.value = totalWeight;
            var dWeight = 0;
            if (isNaN(parseFloat(denseWeight.value))) {
                dWeight = 0;
            }
            else {
                dWeight = parseFloat(denseWeight.value);
            }

            if (dWeight >= totalWeight) {
                txtweight.value = dWeight.toString();
            }
            else {
                txtweight.value = totalWeight.toString();
            }

            txtPieces.value = totalPieces.toString();
            focusWorking(denseWeight);
        }
    </script>
    <%-- Volume Change Per Dimension--%>
    <script type="text/javascript">
        function VolumeChangePerLine(txt) {
            var cell = txt.parentElement;
            var row = cell.parentElement;
            var tbl = row.parentElement;
            var length = parseFloat(row.cells[1].childNodes[0].value);
            var width = parseFloat(row.cells[2].childNodes[0].value);
            var height = parseFloat(row.cells[3].childNodes[0].value);
            var pieces = parseFloat(row.cells[4].childNodes[0].value);
            var volWeight = ((length * width * height) / 5000) * pieces;
            var btn = document.getElementById('btn_AddDim');
            if (!isNaN(volWeight)) {
                row.cells[5].innerText = volWeight.toString();
            }
            else {
                row.cells[5].innerText = '0';
            }

            if (cell.cellIndex == "4") {
                if (row.rowIndex == tbl.rows.length - 1) {
                    focusWorking(btn);
                    return;
                }
                else {
                    var doc = tbl.rows[row.rowIndex + 1].cells[1].childNodes[0];
                    focusWorking(doc);
                    return;
                }

            }

        }
    </script>
    <%-- Remove Single Dimension--%>
    <script type="text/javascript">
        function RemoveSingleRow(row) {
            var tbl = row.parentElement;
            tbl.deleteRow(row.rowIndex);

            if (tbl.rows.length == 1) {
                AddDimensionRow();
            }
            else {
                for (var i = 1; i < tbl.rows.length; i++) {
                    tbl.rows[i].cells[0].innerText = i;
                }
            }

            return false;
        }

        function FocusOutLastControl(txt) {
            var a = 0;

            var cell = txt.parentElement;
            var row = cell.parentElement;
            var tbl = row.parentElement;

            if (cell.cellIndex == "3") {
                if (row.rowIndex == tbl.rows.length - 1) {
                    focusWorking(btn);
                    return;
                }
                else {
                    var doc = tbl.rows[row.rowIndex + 1].cells[1].childNodes[0];
                    focusWorking(doc);
                    return;
                }

            }
        }
    </script>
    <%--Catering for Single Dimension Requirement--%>
    <script type="text/javascript">
        function SingleVolumeChange() {


            var tblDim = document.getElementById('tblDim');


            var txtlength = document.getElementById('<%= txt_l.ClientID %>');
            var txtwidth = document.getElementById('<%= txt_w.ClientID %>');
            var txtheight = document.getElementById('<%= txt_h.ClientID %>');
            var txtvWeight = document.getElementById('<%= txt_vWeight.ClientID %>');
            var txtaWeight = document.getElementById('<%= txt_aWeight.ClientID %>');
            var txtweight = document.getElementById('<%= txt_weight.ClientID %>');
            var txtPieces = document.getElementById('<%= txt_pieces.ClientID %>');

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
            if (tblDim.rows.length == 2) {
                if ((tblDim.rows[1].cells[1].childNodes[0].value == "0" || tblDim.rows[1].cells[1].childNodes[0].value == l.toString()) && (tblDim.rows[1].cells[2].childNodes[0].value == "0" || tblDim.rows[1].cells[2].childNodes[0].value == w.toString()) && (tblDim.rows[1].cells[3].childNodes[0].value == "0" || tblDim.rows[1].cells[3].childNodes[0].value == h.toString()) && (tblDim.rows[1].cells[4].childNodes[0].value == "1" || tblDim.rows[1].cells[4].childNodes[0].value == p.toString())) {
                    tblDim.rows[1].cells[1].childNodes[0].value = l.toString();
                    tblDim.rows[1].cells[2].childNodes[0].value = w.toString();
                    tblDim.rows[1].cells[3].childNodes[0].value = h.toString();
                    tblDim.rows[1].cells[4].childNodes[0].value = p.toString();
                    VolumeChangePerLine(tblDim.rows[1].cells[4].childNodes[0]);

                }
            }
        }
    </script>
    <%--Dense Weight Change--%>
    <script type="text/javascript">
        function DenseWeightChange() {
            var denseWeight = document.getElementById('<%= txt_aWeight.ClientID %>');
            var volWeight = document.getElementById('<%= txt_vWeight.ClientID %>');
            var weight = document.getElementById('<%= txt_weight.ClientID %>');

            var dWeight = parseFloat(denseWeight.value);
            var vWeight = parseFloat(volWeight.value);
            if (isNaN(dWeight)) {
                alert('Invalid Dense Weight');
                denseWeight.value = '0';
                dWeight = 0;
                return;
            }
            if (isNaN(vWeight)) {
                volWeight.value = '0';
                vWeight = 0;
            }

            if (dWeight >= vWeight) {
                weight.value = dWeight.toString();
            }
            else {
                weight.value = vWeight.toString();
            }
        }
    </script>
    <%--Origin EC Change--%>
    <script type="text/javascript">
        function OriginECChange(txt) {
            var ECCode = txt.value.trim();
            var dd_ec = document.getElementById('<%= dd_originExpressCenter.ClientID %>');
            if (ECCode.trim() != "") {
                var validECCode = false;
                for (var i = 0; i < dd_ec.options.length; i++) {
                    if (ECCode == dd_ec.options[i].value) {
                        validECCode = true;
                        break;
                    }
                }

                if (!validECCode) {
                    alert('Invalid Express Center Code');
                    txt.value = "";
                    focusWorking(txt);
                    return;
                }
            }
        }
    </script>
    <%--Locking and Unlocking Controls as per COD Flag--%>
    <script type="text/javascript">
        function LockOnCOD() { }
        function LockControls(tr, lock) {
            var bookingdate = document.getElementById('<%= txt_bookingDate.ClientID %>');
            var chkBookingDate = document.getElementById('<%= chk_BookingDateFreeze.ClientID %>');
            var cnNumber = document.getElementById('<%= txt_cnNumber.ClientID %>');
            var accountNo = document.getElementById('<%= txt_accountNo.ClientID %>');
            var chkAccountNo = document.getElementById('<%= chk_accountNoFreeze.ClientID %>');
            var serviceType = document.getElementById('<%= dd_serviceType.ClientID %>');
            var chkServiceType = document.getElementById('<%= chk_service.ClientID %>');

            var consigner = document.getElementById('<%= txt_consigner.ClientID %>');
            var consignerCell = document.getElementById('<%= txt_consignerCell.ClientID %>');
            var chk_lockConsigner = document.getElementById('<%= chk_lockConsigner.ClientID %>');

            var consignee = document.getElementById('<%= txt_consignee.ClientID %>');
            var consigneeCell = document.getElementById('<%= txt_consigneeCell.ClientID %>');
            var chk_lockConsignee = document.getElementById('<%= chk_lockConsignee.ClientID %>');

            var address = document.getElementById('<%= txt_Address.ClientID %>');
            var chk_lockAddress = document.getElementById('<%= chk_lockAddress.ClientID %>');

            var destination = document.getElementById('<%= dd_destination.ClientID %>');
            var riderCode = document.getElementById('<%= txt_riderCode.ClientID %>');
            var chkRiderCode = document.getElementById('<%= chk_riderCodeFreeze.ClientID %>');
            var length = document.getElementById('<%= txt_l.ClientID %>');
            var width = document.getElementById('<%= txt_w.ClientID %>');
            var height = document.getElementById('<%= txt_h.ClientID %>');
            var volWeight = document.getElementById('<%= txt_vWeight.ClientID %>');
            var denseWeight = document.getElementById('<%= txt_aWeight.ClientID %>');
            var weight = document.getElementById('<%= txt_weight.ClientID %>');
            var pieces = document.getElementById('<%= txt_pieces.ClientID %>');

            var originEc = document.getElementById('<%= dd_originExpressCenter.ClientID %>');
            var txt_ec = document.getElementById('<%= txt_originExpressCenter.ClientID %>');
            var chk_ec = document.getElementById('<%= chk_lockOriginEC.ClientID %>');

            var cnType = document.getElementById('<%= dd_consignmentType.ClientID %>');
            var chk_cnType = document.getElementById('<%= chk_cnType.ClientID %>');
            var coupon = document.getElementById('<%= txt_coupon.ClientID %>');
            var remarks = document.getElementById('<%= txt_remarks.ClientID %>');
            var reportingDate = document.getElementById('<%= txt_reportingDate.ClientID %>');
            var chkReportingDate = document.getElementById('<%= chk_reportingDateFreeze.ClientID %>');
            var approvalStatus = document.getElementById('<%= dd_approvalStatus.ClientID %>');
            var chkApprovalStatus = document.getElementById('<%= chk_approval.ClientID %>');
            var invoiceStatus = document.getElementById('<%= txt_invoiceStatus.ClientID %>');
            var invoiceNumber = document.getElementById('<%= txt_invoiceNumber.ClientID %>');
            var chargeAmount = document.getElementById('<%= txt_chargedAmount.ClientID %>');
            var totalAmount = document.getElementById('<%= txt_totalAmt.ClientID %>');
            var gst = document.getElementById('<%= txt_gst.ClientID %>');
            var orderRefNo = document.getElementById('<%= txt_orderRefNo.ClientID %>');
            var descriptionCOD = document.getElementById('<%= txt_descriptionCOD.ClientID %>');
            var codAmount = document.getElementById('<%= txt_codAmount.ClientID %>');
            var chkCod = document.getElementById('<%= chk_cod.ClientID %>');
            var priceModifier = document.getElementById('<%= dd_priceModifier.ClientID %>');
            var calBase = document.getElementById('<%= dd_calculationBase.ClientID %>');
            var calValue = document.getElementById('<%= txt_priceModifierValue.ClientID %>');
            var declaredValue = document.getElementById('<%= txt_declaredValue.ClientID %>');
            var chkPm = document.getElementById('<%= chk_pm.ClientID %>');
            var hd_newCN = document.getElementById('<%= hd_newCN.ClientID %>');
            var hd_codCN = tr.getElementsByClassName('isCODCN')[0].value;




        }
    </script>
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
    <div id="divConsignee" style="width: 83%; display: none; height: 110%; border: 1px solid black; background-color: #d8d8d8; opacity: 0.5; position: absolute;">
    </div>
    <div id="divDim" class="DivDim">
        <div style="width: 650px; height: 260px; overflow: scroll; overflow-x: hidden;">
            <table id="tblDim" style="width: 630px; padding-right: 5%; padding-left: 5%; margin-top: 2%; border-collapse: collapse; border-radius: 15px;">
                <tr class="DimHead" bgcolor="#f26726">
                    <td style="width: 70px;">
                        <b>Dimension #</b>
                    </td>
                    <td style="width: 100px;">
                        <b>Length</b>
                    </td>
                    <td style="width: 100px;">
                        <b>Width</b>
                    </td>
                    <td style="width: 100px">
                        <b>Height</b>
                    </td>
                    <td style="width: 100px">
                        <b>No. of Pieces</b>
                    </td>
                    <td style="width: 100px">
                        <b>Vol Weight</b>
                    </td>
                    <td style="width: 50px">
                        <b>Remove</b>
                    </td>
                </tr>
            </table>
            <input type="button" id="btn_AddDim" value="Add Item" title="Add Dimension" class="button"
                onclick="AddDimensionRow();" />
        </div>
        <div style="width: 100%; text-align: center;">
            <input type="button" value="Close" title="Close" class="button" onclick="CloseConsigneeDetails();" />
        </div>
    </div>
    <asp:HiddenField ID="hd_maxAllowedDate" runat="server" />
    <asp:HiddenField ID="hd_minAllowedDate" runat="server" />
    <asp:HiddenField ID="hd_CreditClientID" runat="server" />
    <asp:HiddenField ID="hd_customerType" runat="server" />
    <asp:HiddenField ID="hd_COD" runat="server" />
    <asp:HiddenField ID="hd_unApproveable" runat="server" />
    <asp:HiddenField ID="hd_editable" runat="server" />
    <asp:HiddenField ID="hd_portalConsignment" runat="server" />
    <asp:HiddenField ID="hd_BookingDate" runat="server" />
    <asp:HiddenField ID="hd_AccountReceivingDate" runat="server" />
    <asp:HiddenField ID="hd_newCN" runat="server" />
    <asp:HiddenField ID="hd_InsertType" runat="server" />
    <asp:Label ID="Errorid" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
    <asp:Literal ID="lt1" runat="server"></asp:Literal>
    <div class="bulkCnHeaderTbl" style="width: 1110px">
        <table>
            <tr>
                <td colspan="7">
                    <b>Consignment Info.</b>
                </td>
            </tr>
            <tr>
                <td style="width: 160px;">
                    <div>
                        <b>Update Type</b>
                    </div>
                    <div>
                        <asp:CheckBox ID="chk_bulkUpdate" runat="server" Text="Bulk Update" onchange="bulkChange();"
                            Checked="true" Width="155px" />
                    </div>
                </td>
                <td style="width: 160px;">
                    <div>
                        <b>CN Start </b>
                    </div>
                    <div>
                        <asp:TextBox ID="txt_CnStart" Enabled="true" BackColor="#FFFFFF" runat="server" Width="145px"
                            CssClass="textBox" onchange="BulkCnChange();"></asp:TextBox>
                    </div>
                </td>
                <td style="width: 160px;">
                    <div>
                        <b>Number of CNs </b>
                    </div>
                    <div>
                        <asp:TextBox ID="txt_NumberofCN" Enabled="true" runat="server" BackColor="#FFFFFF"
                            Width="145px" CssClass="textBox" onchange="BulkCnChange();"></asp:TextBox>
                    </div>
                </td>
                <td style="width: 160px;">
                    <div>
                        <b>Booking Date</b>
                    </div>
                    <div>
                        <asp:TextBox ID="txt_bookingDate" runat="server" CssClass="textBox" Width="100px"
                            Style="margin-top: -6px;" onchange="BookingDateChange(this);"> </asp:TextBox>
                        <Ajax:CalendarExtender ID="calendar1" runat="server" Format="dd/MM/yyyy" TargetControlID="txt_bookingDate"></Ajax:CalendarExtender>
                        <asp:CheckBox ID="chk_BookingDateFreeze" runat="server" Text="Save" AutoPostBack="false"
                            onclick="LockBookingDate(this);" TextAlign="Right" />
                    </div>
                </td>
                <td style="width: 160px;">
                    <div>
                        <b>Origin </b>
                    </div>
                    <div>
                        <asp:DropDownList ID="dd_origin" runat="server" Enabled="false" Width="145px" CssClass="dropdown"
                            BackColor="#EBEBE5">
                        </asp:DropDownList>
                    </div>
                </td>
                <td style="width: 160px;">
                    <div>
                        <b>Rider Code </b>
                    </div>
                    <div>
                        <asp:TextBox ID="txt_riderCode" runat="server" Enabled="true" CssClass="textBox"
                            onchange="ValidateRider();" Width="100px" AutoPostBack="false" Style="margin-top: -6px"></asp:TextBox>
                        <asp:CheckBox ID="chk_riderCodeFreeze" runat="server" Text="Save" AutoPostBack="false"
                            onclick="LockRider(this);" TextAlign="Right" />
                    </div>
                </td>
                <td style="width: 150px;"></td>
            </tr>
            <tr>
                <td style="width: 160px;">
                    <div>
                        <b>Consignment Number</b>
                    </div>
                    <div>
                        <telerik:RadTextBox ID="txt_cnNumber" runat="server" Skin="Web20" MaxLength="15"
                            Enabled="false" BackColor="#EBEBE5" CssClass="textBox" AutoPostBack="false" onkeypress="return isNumberKey(event);"
                            Resize="None" resolvedrendermode="Classic">
                        </telerik:RadTextBox>
                    </div>
                </td>
                <td style="width: 160px;">
                    <div>
                        <b>Account No.</b>
                    </div>
                    <div>
                        <asp:TextBox ID="txt_accountNo" runat="server" AutoPostBack="false" onchange="ValidateAccount();"
                            CssClass="textBox" Width="100px" Style="margin-top: -6px"></asp:TextBox>
                        <asp:CheckBox ID="chk_accountNoFreeze" runat="server" Text="Save" AutoPostBack="false"
                            onclick="LockAccount(this);" TextAlign="Right" />
                    </div>
                </td>
                <td style="width: 160px;">
                    <div>
                        <b>Consginer </b>
                    </div>
                    <div>
                        <asp:TextBox ID="txt_consigner" runat="server" Width="158px" CssClass="textBox"></asp:TextBox>
                    </div>
                </td>
                <td style="width: 160px;">
                    <div>
                        <b>Consigner Mobile No.</b>
                    </div>
                    <div>
                        <asp:TextBox ID="txt_consignerCell" runat="server" onkeypress="return isNumberKey(event);"
                            CssClass="textBox" Width="100px" Style="margin-top: -6px"></asp:TextBox>
                        <asp:CheckBox ID="chk_lockConsigner" runat="server" Text="Save" AutoPostBack="false"
                            onclick="LockConsigner(this);" TextAlign="Right" />
                    </div>
                </td>
                <td style="width: 160px;">
                    <div>
                        <b>Consignee</b>
                    </div>
                    <div>
                        <asp:TextBox ID="txt_consignee" runat="server" Enabled="true" Width="158px" CssClass="textBox"></asp:TextBox>
                    </div>
                </td>
                <td style="width: 160px;">
                    <div>
                        <b>Consignee Mobile No.</b>
                    </div>
                    <div>
                        <asp:TextBox ID="txt_consigneeCell" runat="server" Enabled="true" onkeypress="return isNumberKey(event);"
                            CssClass="textBox" Width="100px" Style="margin-top: -6px"></asp:TextBox>
                        <asp:CheckBox ID="chk_lockConsignee" runat="server" Text="Save" AutoPostBack="false"
                            onclick="LockConsignee(this);" TextAlign="Right" />
                    </div>
                </td>
                <td style="width: 150px;"></td>
            </tr>
            <tr>
                <td style="width: 160px;">
                    <div>
                        <b>Service Type</b>
                    </div>
                    <div>
                        <asp:DropDownList ID="dd_serviceType" runat="server" AppendDataBoundItems="true"
                            CssClass="dropdown" Width="100px" onchange="ChangeMaxLength(this);">
                            <asp:ListItem Value="0">Select Service Type</asp:ListItem>
                        </asp:DropDownList>
                        <asp:CheckBox ID="chk_service" runat="server" Text="Lock" onclick="LockService(this);" />
                    </div>
                </td>
                <td style="width: 160px;">
                    <div>
                        <b>Destination </b>
                    </div>
                    <div>
                        <telerik:RadComboBox ID="dd_destination" runat="server" Skin="Metro" AppendDataBoundItems="true"
                            AllowCustomText="true" MarkFirstMatch="true" Visible="true" Width="150px">
                            <Items>
                                <telerik:RadComboBoxItem Text="Select Destination" Value="0" Selected="true" />
                            </Items>
                        </telerik:RadComboBox>
                    </div>
                </td>
                <td style="width: 160px;">
                    <div>
                        <b>Dimensions (L X W X H) </b>
                    </div>
                    <div>
                        <asp:TextBox ID="txt_l" onchange="SingleVolumeChange()" runat="server" Width="40px"
                            CssClass="textBox"></asp:TextBox><b> X </b>
                        <asp:TextBox ID="txt_w" onchange="SingleVolumeChange()" runat="server" Width="40px"
                            CssClass="textBox"></asp:TextBox><b> X </b>
                        <asp:TextBox ID="txt_h" onchange="SingleVolumeChange()" runat="server" Width="40px"
                            CssClass="textBox"></asp:TextBox>
                    </div>
                </td>
                <td style="width: 160px;">
                    <div style="width: 70px; float: left; margin-right: 5px;">
                        <b>Vol. Weight </b>
                    </div>
                    <div style="width: 76px; float: left;">
                        <b>Dense Weight </b>
                    </div>
                    <div style="width: 70px; float: left; margin-right: 5px;">
                        <asp:TextBox ID="txt_vWeight" runat="server" Enabled="false" Width="70px" CssClass="textBox"
                            BackColor="#EBEBE5"></asp:TextBox>
                    </div>
                    <div style="width: 76px; float: left;">
                        <asp:TextBox ID="txt_aWeight" runat="server" onchange="DenseWeightChange()" Width="75px"
                            CssClass="textBox" MaxLength="4"></asp:TextBox>
                    </div>
                </td>
                <td style="width: 160px;">
                    <div style="width: 70px; float: left; margin-right: 5px;">
                        <b>Weight </b>
                    </div>
                    <div style="width: 70px; float: left;">
                        <b>Pieces</b>
                    </div>
                    <div style="width: 70px; float: left; margin-right: 5px;">
                        <asp:TextBox ID="txt_weight" runat="server" MaxLength="4" Width="70px" Enabled="false"
                            BackColor="#EBEBE5" CssClass="textBox" onkeypress="return isNumberKeydouble(event);"
                            onchange="if ( RiderWeightCheck() == false ) return;" AutoPostBack="false"></asp:TextBox>
                    </div>
                    <div style="width: 70px; float: left;">
                        <asp:TextBox ID="txt_pieces" runat="server" Enabled="true" onkeypress="return isNumberKey(event);"
                            onchange="SingleVolumeChange()" CssClass="textBox" Width="70px" AutoPostBack="false"></asp:TextBox>
                    </div>
                </td>
                <td style="width: 160px;">
                    <a href="" style="display: block;" onclick="return OpenDimensionsDiv();">Click to Enter
                        Multiple Dimensions</a>
                </td>
                <td style="width: 150px;"></td>
            </tr>
            <tr>
                <td style="width: 160px;" colspan="2" rowspan="2">
                    <div>
                        <b>Address </b>
                    </div>
                    <div>
                        <asp:TextBox ID="txt_Address" runat="server" Width="260px" TextMode="MultiLine" Rows="4"
                            CssClass="textBox" Height="65px"></asp:TextBox>
                        <asp:CheckBox ID="chk_lockAddress" runat="server" Text="Save" AutoPostBack="false"
                            onclick="LockAddress(this);" TextAlign="Right" />
                    </div>
                </td>
                <td style="width: 320px; text-align: right;" colspan="2" rowspan="2">
                    <div>
                        &nbsp;
                    </div>
                    <div>
                        <b>Charged Amount </b>
                        <asp:TextBox ID="txt_chargedAmount" runat="server" Enabled="false" onkeypress="return isNumberKeydouble(event);"
                            BackColor="#EBEBE5" CssClass="textBox" Width="115px" Style="margin-bottom: 4.5px"></asp:TextBox>
                    </div>
                    <div>
                        <b>Total Amount</b>
                        <asp:TextBox ID="txt_totalAmt" runat="server" Enabled="false" Width="115px" CssClass="textBox"
                            BackColor="#EBEBE5" Style="margin-bottom: 4.5px"></asp:TextBox>
                    </div>
                    <div>
                        <b>GST</b>
                        <asp:TextBox ID="txt_gst" runat="server" Enabled="false" Width="115px" CssClass="textBox"
                            BackColor="#EBEBE5"></asp:TextBox>
                    </div>
                </td>
                <td style="width: 160px;">
                    <div>
                        <b>Reporting Date </b>
                    </div>
                    <div>
                        <asp:TextBox ID="txt_reportingDate" runat="server" Width="100px" CssClass="textBox"
                            onchange="ReportingDateChange(this);"></asp:TextBox>
                        <Ajax:CalendarExtender ID="calendar2" runat="server" TargetControlID="txt_reportingDate"
                            Format="dd/MM/yyyy"></Ajax:CalendarExtender>
                        <asp:CheckBox ID="chk_reportingDateFreeze" runat="server" Text="Save" AutoPostBack="false"
                            TextAlign="Right" onclick="LockReportingDate(this);" />
                    </div>
                </td>
                <td style="width: 160px;">
                    <div>
                        <b>Approval Status </b>
                    </div>
                    <div>
                        <asp:DropDownList ID="dd_approvalStatus" runat="server" AppendDataBoundItems="true"
                            Width="100px" BackColor="#FFFFFF" CssClass="dropdown">
                            <asp:ListItem Value="0">Unapproved</asp:ListItem>
                            <asp:ListItem Value="1" Selected="True">Approved</asp:ListItem>
                        </asp:DropDownList>
                        <asp:CheckBox ID="chk_approval" runat="server" Text="Lock" onclick="LockStatus(this);" />
                    </div>
                </td>
                <td style="width: 150px;"></td>
            </tr>
            <tr>
                <td style="width: 160px;">
                    <div>
                        <b>Invoice Status </b>
                    </div>
                    <div>
                        <asp:TextBox ID="txt_invoiceStatus" runat="server" Enabled="false" Width="150px"
                            BackColor="#EBEBE5" CssClass="textBox"></asp:TextBox>
                    </div>
                </td>
                <td style="width: 160px;">
                    <div>
                        <b>Invoice No. </b>
                    </div>
                    <div>
                        <asp:TextBox ID="txt_invoiceNumber" runat="server" Width="150px" Enabled="false"
                            CssClass="textBox" BackColor="#EBEBE5"></asp:TextBox>
                    </div>
                </td>
                <td style="width: 150px;"></td>
            </tr>
            <tr>
                <td style="width: 160px;">
                    <div>
                        <b>Origin Express Center </b>
                    </div>
                    <div>
                        <asp:DropDownList ID="dd_originExpressCenter" runat="server" AppendDataBoundItems="true"
                            CssClass="dropdown" Enabled="false" Width="100%" BackColor="#EBEBE5" Style="display: none;">
                            <asp:ListItem Value="0">--Select EC--</asp:ListItem>
                        </asp:DropDownList>
                        <asp:TextBox ID="txt_originExpressCenter" runat="server" disabled="true" CssClass="textBox"
                            Width="60%" onchange="OriginECChange(this); return false;"></asp:TextBox>
                        <asp:CheckBox ID="chk_lockOriginEC" runat="server" Text="Lock" onclick="LockEC(this);" />
                    </div>
                    <div>
                        <a href="ECList.aspx" target="_blank">List of EC</a>
                    </div>
                </td>
                <td style="width: 160px;">
                    <div>
                        <b>Coupon Number </b>
                    </div>
                    <div>
                        <asp:TextBox ID="txt_coupon" runat="server" Enabled="true" MaxLength="500" ToolTip="Use Comma(,) for multiple Entries"
                            CssClass="textBox" Width="150px"></asp:TextBox>
                    </div>
                </td>
                <td style="width: 320px;" colspan="2">
                    <div>
                        <b>Special Instructions </b>
                    </div>
                    <div>
                        <asp:TextBox ID="txt_remarks" runat="server" Width="300px" CssClass="textBox" onchange="focusSkip(this);"
                            onblur="focusSkip(this);"></asp:TextBox>
                    </div>
                </td>
                <td style="width: 160px;">
                    <div>
                        <b>CN Type</b>
                    </div>
                    <div>
                        <asp:DropDownList ID="dd_consignmentType" runat="server" AppendDataBoundItems="true"
                            CssClass="dropdown" Width="100px">
                            <asp:ListItem Value="0">Select CN Type</asp:ListItem>
                        </asp:DropDownList>
                        <asp:CheckBox ID="chk_cnType" runat="server" Text="Lock" onclick="LockCNType(this);"
                            Checked="false" />
                    </div>
                </td>
                <td style="width: 160px;"></td>
                <td style="width: 150px;"></td>
            </tr>
        </table>
    </div>
    <table class="bulkCnHeaderTbl" style="border: none !important; width: 99%">
        <tr>
            <td style="width: 50%">
                <div class="bulkCnHeaderTbl">
                    <panel id="panel1">
                    <table style="margin-left: 0px !important; width: 99%">
                        <tr>
                            <td colspan="2" style="text-align: center !important; font-size: medium; font-weight: bold;
                                font-variant: small-caps;">
                                COD Info.
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-left: 25px; width: 150px;">
                                <div>
                                    <b>Order Ref. No.</b>
                                </div>
                                <div>
                                    <asp:TextBox ID="txt_orderRefNo" runat="server" CssClass="textBox" Width="145px"></asp:TextBox>
                                </div>
                            </td>
                            <td>
                                <div>
                                    <b>Description</b>
                                </div>
                                <div>
                                    <asp:TextBox ID="txt_descriptionCOD" runat="server" CssClass="textBox" Width="340px"></asp:TextBox>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-left: 25px;">
                                <div>
                                    <b>COD Amount</b>
                                </div>
                                <div>
                                    <asp:TextBox ID="txt_codAmount" runat="server" onkeypress="return isNumberKey(event);"
                                        CssClass="textBox" Width="75px" ></asp:TextBox>
                                </div>
                            </td>
                            <td>
                                <div>
                                    &nbsp;
                                </div>
                                <div>
                                    <asp:CheckBox ID="Cb_CODAmount" runat="server" AutoPostBack="false" Text="Charge COD Amount"
                                        Width="130px" Enabled="false" />
                                    <asp:CheckBox ID="chk_cod" runat="server" Text="Lock" Style="float: right; padding-right: 10px;" />
                                </div>
                            </td>
                        </tr>
                    </table>
                    </panel>
                </div>
            </td>
            <td>
                <div class="bulkCnHeaderTbl">
                    <table style="margin-left: 0px !important; width: 99%">
                        <tr>
                            <td colspan="2" style="text-align: center !important; font-size: medium; font-weight: bold; font-variant: small-caps;">Price Modifier Info.
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-right: 50px; padding-left: 25px;">
                                <div style="display: inline; text-align: left; width: 100%;">
                                    <b>Modifier Name</b>
                                </div>
                                <div>
                                    <asp:DropDownList ID="dd_priceModifier" runat="server" AppendDataBoundItems="true"
                                        onchange="PMChange(this);" CssClass="dropdown" AutoPostBack="false" Width="100%">
                                        <asp:ListItem Value="0">Select Price Modifier</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </td>
                            <td>
                                <div style="display: inline; text-align: left; width: 100%;">
                                    <b>Calculation Base</b>
                                </div>
                                <div>
                                    <asp:RadioButtonList ID="dd_calculationBase" runat="server" RepeatColumns="3" RepeatDirection="Horizontal"
                                        Width="250px" Enabled="false">
                                        <asp:ListItem Value="1">Flat</asp:ListItem>
                                        <asp:ListItem Value="2">Percentage</asp:ListItem>
                                        <asp:ListItem Value="3">Insurance</asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-right: 50px; padding-left: 25px;">
                                <div style="display: inline; text-align: left; width: 100%">
                                    <b>Modifier Value</b>
                                </div>
                                <div>
                                    <asp:TextBox ID="txt_priceModifierValue" runat="server" Enabled="false" CssClass="textBox"
                                        BackColor="#EBEBE5"></asp:TextBox>
                                </div>
                            </td>
                            <td>
                                <div style="display: inline; text-align: left; width: 100%">
                                    <b>Declared Value(Insurance)</b>
                                </div>
                                <div>
                                    <asp:TextBox ID="txt_declaredValue" runat="server" Enabled="false" CssClass="textBox"
                                        BackColor="#EBEBE5"></asp:TextBox>
                                    <asp:CheckBox ID="chk_pm" runat="server" Text="Lock" onclick="LockPriceModifier(this)" />
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
    <div style="width: 100%; text-align: center">
        <input type="button" id="btn_add" value="Update Changes" class="button" onclick="UpdateCNChanges();" />
    </div>
    <div class="bulkCnHeaderTbl bulkCnHeaderTbl1" style="height: 300px; width: 1120px; overflow: scroll;">
        <table id="tblConsignments" style="float: left; width: 3000px;">
            <tr>
                <td style="width: 80px;">&nbsp;
                </td>
                <td style="width: 100px;">
                    <b>CN #</b>
                </td>
                <td style="width: 75px;">
                    <b>BK. Date</b>
                </td>
                <td style="width: 30px;">
                    <b>Acc.#</b>
                </td>
                <td style="width: 100px;">
                    <b>Service Type</b>
                </td>
                <td style="width: 100px;">
                    <b>Consigner</b>
                </td>
                <td style="width: 100px;">
                    <b>Consignee</b>
                </td>
                <td style="width: 105px;">
                    <b>Consigner Mob.#</b>
                </td>
                <td style="width: 105px;">
                    <b>Consignee Mob.#</b>
                </td>
                <td style="width: 100px;">
                    <b>Destination</b>
                </td>
                <td style="width: 30px;">
                    <b>Rider</b>
                </td>
                <td style="width: 100px;">
                    <b>Dimensions</b>
                </td>
                <td style="width: 50px;">
                    <b>Vol Wgt.</b>
                </td>
                <td style="width: 55px;">
                    <b>Dns Wgt.</b>
                </td>
                <td style="width: 30px;">
                    <b>Weight</b>
                </td>
                <td style="width: 30px;">
                    <b>Pieces</b>
                </td>
                <td style="width: 200px;">
                    <b>Address</b>
                </td>
                <td style="width: 150px;">
                    <b>Origin EC</b>
                </td>
                <td style="width: 100px;">
                    <b>CN Type</b>
                </td>
                <td style="width: 80px;">
                    <b>Coupon #</b>
                </td>
                <td style="width: 200px;">
                    <b>Special Instructions</b>
                </td>
                <td style="width: 75px;">
                    <b>RP. Date</b>
                </td>
                <td style="width: 60px;">
                    <b>Approved?</b>
                </td>
                <td style="width: 75px;">
                    <b>Inv. Status</b>
                </td>
                <td style="width: 80px;">
                    <b>Inv. Number</b>
                </td>
                <td style="width: 100px;">
                    <b>COD Ref #</b>
                </td>
                <td style="width: 150px;">
                    <b>COD DESC.</b>
                </td>
                <td style="width: 60px;">
                    <b>COD Amt.</b>
                </td>
                <td style="width: 100px;">
                    <b>Add. Serv.</b>
                </td>
                <td style="width: 80px;">
                    <b>Alt. Value</b>
                </td>
                <td style="width: 80px;">
                    <b>Add. Chrgs.</b>
                </td>
                <td style="width: 60px;">
                    <b>Chrg Amt.</b>
                </td>
                <td style="width: 60px;">
                    <b>Total Amt.</b>
                </td>
                <td style="width: 60px;">
                    <b>Gst</b>
                </td>
                <td style="width: 200px;">
                    <b>shipperAddress</b>
                </td>
                <td style="width: 200px;">
                    <b>PakageContents</b>
                </td>
            </tr>
        </table>
    </div>
    <div style="width: 100%; text-align: center">
        <input type="button" class="button" value="Reset" />
        <input type="button" class="button" value="Save to Database" onclick="SaveToDB()" />
    </div>
    <div style="display: block;">
        <table id='cnDim'>
            <tr>
                <td>ConsignmentNumber
                </td>
                <td>ItemNumber
                </td>
                <td>Length
                </td>
                <td>width
                </td>
                <td>Height
                </td>
                <td>Pieces
                </td>
                <td>VolWeight
                </td>
            </tr>
        </table>
    </div>
</asp:Content>