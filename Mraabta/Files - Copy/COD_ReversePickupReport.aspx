<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="COD_ReversePickupReport.aspx.cs" Inherits="MRaabta.Files.COD_ReversePickupReport" %>

<%@ Register TagPrefix="AjaxControlToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <link href="<%=ResolveUrl("~/Content/bootstrap.min.css") %>" rel="stylesheet" />
    <script type="text/javascript" src="<%=ResolveUrl("~/Scripts/jquery-3.5.1.min.js") %>"></script>
    <script type="text/javascript" src="<%=ResolveUrl("~/Scripts/bootstrap.min.js") %>"></script>
    <script type="text/javascript" src="<%=ResolveUrl("~/Scripts/sweetalert.min.js") %>"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            if ($("#StatusType").val() == "1") {
                jsFunction(1);
            }
        });
        function BookingStatus(value) {
            if (value == "1") {
                console.log("abcd");
                document.getElementById('ZoneCheckAll').checked = true;
                document.getElementById('ZoneDDL').style.display = 'none';
                document.getElementById('AccountDDLCheckAll').checked = true;
                document.getElementById('AccountDDL').style.display = 'none';
                document.getElementById("DateTo").valueAsDate = new Date();
                document.getElementById("DateFrom").valueAsDate = new Date(new Date().setDate(new Date().getDate() - 7));
                DateTo.max = new Date().toISOString().split("T")[0];
                DateTo.min = new Date().toISOString().split("T")[0];
                DateFrom.min = new Date(new Date().setDate(new Date().getDate() - 7)).toISOString().split("T")[0];
                DateFrom.max = new Date().toISOString().split("T")[0];
            }
            else {
                document.getElementById('ZoneCheckAll').checked = false;
                document.getElementById('ZoneDDL').style.display = '';
                document.getElementById('AccountDDLCheckAll').checked = false;
                document.getElementById('AccountDDL').style.display = '';
                document.getElementById("DateTo").valueAsDate =null;
                document.getElementById("DateFrom").valueAsDate =null;

                DateTo.max = '';
                DateTo.min = '';
                DateFrom.min = '';
                DateFrom.max = '';
                 }
        }
        function jsFunction(value) {
            BookingStatus(value);
        }
        function isNumberKeyWithDecimal(evt) {
            var status = false;
            var charCode = (evt.which) ? evt.which : event.keyCode

            if (charCode > 31 && (charCode < 48 || charCode > 57))
                status = false;
            if (charCode == 46)
                status = true;
            if (charCode > 47 && charCode < 58)
                status = true;
            return status;
        }

        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }


        $(document).ready(function () {
            getZones();
            var ZoneSelected = getSelectValues(document.getElementById('ZoneDDL')).toString();
            GetAccounts(ZoneSelected);
            $('#ZoneDDL').change(function () {

                var ZoneSelected = getSelectValues(document.getElementById('ZoneDDL')).toString();

            });
        });
        function getZones() {
            var pageZone ='<%=ResolveUrl("~/Files/COD_ReversePickupReport.aspx/GetZones")%>';
            var pageAccount = '<%=ResolveUrl("~/Files/COD_ReversePickupReport.aspx/GetAccounts")%>';
            var ZoneDDL = document.getElementById('ZoneDDL');

            $.ajax({

                url: pageZone,
                type: 'POST',
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                success: function (data) {

                    debugger
                    var kk = '';
                    var rr = data.d;
                    for (let y of rr) {
                        kk += `<option value="${y.Value}">${y.Text}</option>`;
                    }
                    ZoneDDL.innerHTML = kk;
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                },
                failure: function () {
                }
            });
        }
        function GetAccounts(ZoneSelected) {
            var pageZone ='<%=ResolveUrl("~/Files/COD_ReversePickupReport.aspx/GetZones")%>';
            var pageAccount = '<%=ResolveUrl("~/Files/COD_ReversePickupReport.aspx/GetAccounts")%>';

            var AccountDDL = document.getElementById('AccountDDL');

            $.ajax({
                url: pageAccount,
                type: 'POST',
                data: JSON.stringify({ Zone: ZoneSelected }),
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                success: function (data) {


                    var kk = '';
                    var rr = data.d;
                    for (let y of rr) {
                        kk += `<option value="${y.Value}">${y.Text}</option>`;
                    }
                    AccountDDL.innerHTML = kk;
                    AccountDDL.style.width = '250px';
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                },
                failure: function () {
                }
            });
        }
        function getSelectValues(select) {
            var result = [];
            var options = select && select.options;
            var opt;

            for (var i = 0, iLen = options.length; i < iLen; i++) {
                opt = options[i];

                if (opt.selected) {
                    result.push("'" + opt.value + "'" || opt.text);
                }
            }
            return result;
        }

        function ZoneCheckAllChange() {


            if (document.getElementById('ZoneCheckAll').checked) {
                document.getElementById('ZoneDDL').style.display = 'none';
                //GetAccounts("all");
            } else {
                document.getElementById('ZoneDDL').style.display = '';
            }
        }

        function AccountCheckAll() {

            if (document.getElementById('AccountDDLCheckAll').checked) {
                document.getElementById('AccountDDL').style.display = 'none';
            } else {
                document.getElementById('AccountDDL').style.display = '';
            }
        }

        function StatusCheckAll() {

            if (document.getElementById('StatusDDLCheckAll').checked) {
                document.getElementById('StatusType').style.display = 'none';
            } else {
                document.getElementById('StatusType').style.display = '';
            }
        }

        function GenerateLoadSheet(ConsignmentNumber) {
            //window.location.href = 'GetAddressLabel_HTML.aspx?con=' + ConsignmentNumber;
            window.open('GetAddressLabel_HTML.aspx?con=' + ConsignmentNumber, '_blank');
        }

        function Search_Click() {

            var PageSearchDataCSV = '<%=ResolveUrl("~/Files/COD_ReversePickupReport.aspx/Search_CSV_Click")%>';
            var PageSearchData = '<%=ResolveUrl("~/Files/COD_ReversePickupReport.aspx/Search_Click")%>';
            var ZoneDDLvalues = getSelectValues(document.getElementById('ZoneDDL')).toString();
            var OutputType = document.getElementById('OutputType').value;
            var AccountDDL = getSelectValues(document.getElementById('AccountDDL')).toString();
            var dateStart = document.getElementById('DateFrom').value;
            var dateEnd = document.getElementById('DateTo').value;
            var Status = document.getElementById('StatusType').value;

            if (document.getElementById('ZoneCheckAll').checked) {
                ZoneDDLvalues = 'All';
            }
            else if (ZoneDDLvalues == '') {
                document.getElementById('StatusLbl').innerText = 'Please provide zone';
                return;
            }

            if (document.getElementById('AccountDDLCheckAll').checked) {
                AccountDDL = 'All';
            }

            if (document.getElementById('StatusDDLCheckAll').checked) {
                Status = 'All';
            }

            if (dateStart == null || dateStart == '') {
                document.getElementById('StatusLbl').innerText = 'Please provide start date';
                return;
            }

            if (dateEnd == null || dateEnd == '') {
                document.getElementById('StatusLbl').innerText = 'Please provide end date';
                return;
            }

            document.getElementById('StatusLbl').innerText = '';

            document.getElementById('loadinggif_filter').style.display = 'block';
            document.getElementById('SearchBtn').style.cssText = 'background-color:grey !important';
            document.getElementById('SearchBtn').disabled = true;

            $('#CODTableBody').html('');


            if (OutputType.toString() == 'CSV') {
               // alert('Loading. Please wait');
                document.getElementById('StatusLbl').innerText = 'Loading!...';
                document.getElementById('loadinggif_filter').style.display = 'block';

                $.ajax({
                    type: "POST",
                    url: PageSearchDataCSV,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({ DateStart: dateStart, DateEnd: dateEnd, Zone: ZoneDDLvalues, Account: AccountDDL, Status: Status, OutputType: OutputType }),
                    success: (rs) => {
                        window.location.href = rs.d;
                    }
                });
            }
            if (OutputType.toString() == 'HTML') {
               // alert('Loading. Please wait');
                document.getElementById('StatusLbl').innerText = 'Loading!...';
                document.getElementById('loadinggif_filter').style.display = 'block';

                $.ajax({

                    url: PageSearchData,
                    type: 'POST',
                    data: JSON.stringify({ DateStart: dateStart, DateEnd: dateEnd, Zone: ZoneDDLvalues, Account: AccountDDL, Status: Status, OutputType: OutputType }),
                    contentType: "application/json; charset=utf-8",
                    dataType: 'json',
                    success: function (data) {
                        if (data.d.status) {

                            if (data.d.CODTable.length > 0) {
                                document.getElementById('StatusLbl').innerText = 'No Record Found';
                            }
                            var rows = '';
                            var table = data.d.CODTable;

                            for (let item in data.d.CODTable) {
                                rows += `<tr >
                                    
                                    <td class="DiscountTypeRadio" >${table[item].serial}</td>
                                    <td >${table[item].consignerAccountNo}</td> 
                                    <td class="allExpressCodeCheckboxTd" >${table[item].consigner}</td>
                                 
                                    <td class="allbranchesExtra" >${table[item].Zone}</td>
                                    <td class="allZonesExtra" >${table[item].branch}</td>
                                    <td class="serviceTypeTd" >${table[item].bookingDate}</td> 
                                    <td  >${table[item].orderRefNo}</td> 
                                    <td class="discountTypeIdTd" >${table[item].consignmentNumber}</td>                            
                                    <td class="discountValueTd" >${table[item].consignee}</td> 
                                    <td class="shortDescriptionTd" >${table[item].consigneePhoneNo}</td> 
                                    <td class="longDescriptionTd" >${table[item].ADDRESS}</td> 

                                    <td class="minShipmentCountTd">${table[item].weight}</td> 
                                    <td class="maxShipmentCountTd">${table[item].pieces}</td> 
                                    <td class="minShipmentWeightTd">${table[item].STATUS}</td> 
                                    <td class="maxShipmentWeightTd">${table[item].BookingDate}</td> 
                                    <td class="maxShipmentWeightTd">${table[item].Arrivaldt}</td> 
                                    <td class="specialDiscountId" >${table[item].deliverydate}</td> 
                                    <td><button type='button' class='button' onclick='GenerateLoadSheet(${table[item].consignmentNumber})'>Generate</td>
                                </tr>`;
                            }
                            document.getElementById('CODStatusTable').style.display = '';
                            $('#CODTableBody').html(rows);
                            document.getElementById('StatusLbl').innerText = 'Total Records: ' + data.d.CODTable.length;
                        }
                        else {
                            document.getElementById('StatusLbl').innerText = 'No Record Found';
                        }
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        document.getElementById('StatusLbl').innerText = 'Error !Please contact IT for support';

                    },
                    failure: function () {
                        document.getElementById('StatusLbl').innerText = 'Error !Please contact IT for support';

                    }
                });
            }

            $("#loadinggif_filter").hide();
            document.getElementById('SearchBtn').style.cssText = 'background-color:#f27031  !important';
            document.getElementById('SearchBtn').disabled = false;
        }


    </script>
    <style>
        .table th {
            border: 1px solid black;
            margin: 0;
            padding: 0;
        }

        .table td {
            border: 1px solid black;
            margin: 0;
            padding: 0;
        }

        tr.spaceUnder > td {
            padding-bottom: 2px;
        }

        .div_header {
            width: 100%;
            height: 23px;
            font-size: 14px;
            margin-bottom: 1px;
        }



        body {
            font-size: 13px;
            font-weight: none;
            line-height: 1.20;
        }

        .form-inline {
            border-bottom: 1px solid #eee;
            padding: 0;
        }

        .MainTbl {
            font-size: 16px;
        }

            .MainTbl th {
                background-color: #404040;
                color: white;
                position: sticky;
                top: 0px;
            }

        .TdScroll {
            overflow-y: scroll;
            height: 30px;
            float: left;
        }

        .TdScrollRight {
            overflow-y: scroll;
            float: right;
        }

        .table thead th {
    vertical-align: middle !important;
    border-bottom: 2px solid #dee2e6;
}

        .table td{
            text-align:center;
    vertical-align: middle !important; 
    border-top: 1px solid #dee2e6;
}
    </style>

    <style>
        /* Center the loader */

        .outer_box {
            background: #444 none repeat scroll 0 0;
            height: 101%;
            left: 0;
            opacity: 0.9;
            position: absolute;
            top: -1%;
            width: 100%;
        }

        .loader {
            border: 16px solid #f3f3f3;
            border-radius: 50%;
            border-top: 16px solid #3498db;
            width: 120px;
            height: 120px;
            -webkit-animation: spin 2s linear infinite;
            animation: spin 2s linear infinite;
            left: 43%;
            position: relative;
            top: 43%
        }

        @-webkit-keyframes spin {
            0% {
                -webkit-transform: rotate(0deg);
            }

            100% {
                -webkit-transform: rotate(360deg);
            }
        }

        @keyframes spin {
            0% {
                transform: rotate(0deg);
            }

            100% {
                transform: rotate(360deg);
            }
        }
    </style>
    <table cellpadding="0" cellspacing="0" width='100%' class='mGrid_Table'>
        <tr>
            <td colspan="12" align="center" class="head_column">
                <h3 style="text-align: center">COD Reverse Pickup Report
                </h3>
            </td>
        </tr>
    </table>
    <fieldset style="border: solid; border-width: thin; height: auto; border-color: #a8a8a8;"
        class="ml-2 mr-2">
        <legend id="Legend6" visible="true" style="width: auto; margin-bottom: 0px; font-size: 16px; font-weight: bold; color: #1f497d;">Filter</legend>

        <table style="margin-left: 5px; font-size: medium; color: black; padding-bottom: 0px; width: 100%; margin-top: 3px">
            <tr class="">

                <td class="field" style="width: 12% !important; text-align: left !important; padding-right: 10px !important;">
                    <b>Date From:</b>
                    <br />

                    <input id="DateFrom" placeholder="Date From" width="180px" type="date" min='1899-01-01' max='2035-13-13'/>
                </td>


                <td class="field" style="width: 12% !important; text-align: left !important; padding-right: 10px !important;">
                    <b>Date To:</b>
                    <br />
                    <input id="DateTo" placeholder="Date To" width="180px" type="date"  min='1899-01-01' max='2035-13-13'/>

                </td>


                <td class="field" style="width: 10% !important; text-align: left !important; padding-right: 10px !important;">
                    <b>Zone:</b>
                    <br />
                    <select id="ZoneDDL" multiple></select>
                    <%--<asp:DropDownList ID="ZoneDDL" runat="server" ></asp:DropDownList>--%>
                    <br />
                    All<input type="checkbox" id="ZoneCheckAll" onclick="ZoneCheckAllChange()" />
                </td>

                <td class="field" style="width: 18% !important; text-align: left !important; padding-right: 10px !important;">
                    <b>Account Name:</b>
                    <br />
                    <select id="AccountDDL" multiple></select>
                    <%--<asp:DropDownList ID="AccountDDL" runat="server"></asp:DropDownList>--%>
                    <br />
                    All<input type="checkbox" id="AccountDDLCheckAll" onclick="AccountCheckAll()" />
                </td>

                <td class="field" style="width: 12% !important; text-align: left !important; padding-right: 10px !important;">
                    <b>Status:</b>
                    <br />
                    <select id="StatusType" onchange="jsFunction(this.value);">
                        <option value="1">Booked</option>
                        <option value="2">Picked</option>
                        <option value="3">Forwarded</option>
                        <option value="4">Delivered</option>
                    </select>
                    All<input type="checkbox" id="StatusDDLCheckAll" onclick="StatusCheckAll()" />
                </td>

                <td class="field" style="width: 12% !important; text-align: left !important; padding-right: 10px !important;">
                    <b>Output Type:</b>
                    <br />
                    <select id="OutputType">
                        <option value="HTML">HTML</option>
                        <option value="CSV">CSV</option>
                    </select>

                </td>
                <td style="width: 8%">
                    <button class="button" id="SearchBtn" type="button" onclick="Search_Click()" style="height: 30px">Search</button>
                </td>
            </tr>

        </table>
    </fieldset>

    <div id="loaders" runat="server" class="outer_box" style="display: none;">
        <div id="loader" runat="server" class="loader"></div>
    </div>
    <fieldset style="border: solid; border-width: thin; height: auto; border-color: #a8a8a8;"
        class="ml-2 mr-2">
        <legend id="Legend6" visible="true" style="width: auto; margin-bottom: 0px; font-size: 16px; font-weight: bold; color: #1f497d;">Report</legend>

        <div id="loadinggif_filter" style="display: none">
            <img id="loading-image" src="../assets/images/loader-1.gif" alt="../assets/images/loader-2.gif" />
            <b>Loading...</b>
        </div>

        <div style="overflow-y: scroll; height: 700px; /*width: 74%; */width: 99.5%; float: left; margin-left: 5px; margin-top: 3px">
            <h5 id="StatusLbl" style="color: red"></h5>
            <table id="CODStatusTable" class="MainTbl table table-striped TdScroll" style="font-size: 14px;display:none;">
                <thead style="text-align:center;vertical-align:central !important;">
                    <tr>
                        <th style="vertical-align:central !important;">S #</th>
                        <th style="vertical-align:middle !important;">Account No</th>
                        <th>Account Name</th>
                        <%-- <th>Customer Address</th>
                        <th>Customer Phone</th>--%>
                        <th>Zone</th>
                        <th>Branch</th>
                        <th>Pickup Req. Date</th>
                        <th>Order Ref #</th>
                        <th>CN #</th>
                        <th>Customer Name</th>
                        <th>Customer Number</th>
                        <th>Pickup Address</th>
                        <th>Weight</th>
                        <th>Pieces</th>
                        <th>Status</th>
                        <th>Created Date</th>
                        <th>Pickup Date</th>
                        <th>Delivery Date</th>
                        <th></th>
                    </tr>
                </thead>
                <tbody id="CODTableBody">
                </tbody>
            </table>

        </div>
        <br />
        <div>
        </div>

    </fieldset>
</asp:Content>
