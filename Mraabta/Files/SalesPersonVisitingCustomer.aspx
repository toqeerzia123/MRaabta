<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="SalesPersonVisitingCustomer.aspx.cs" Inherits="MRaabta.Files.SalesPersonVisitingCustomer" %>

<%@ Register TagPrefix="Ajax1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>

<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <style>
        /*.DataTableCustomer thead {
            background-color: #3f87a6;
        }*/

        .table th {
            border: 1px solid rgb(190, 190, 190);
            padding: 3px 3px;
            color: #fff;
            background-color: #3f87a6;
        }

        .scroll {
            text-align: center;
        }

        .header_1 th {
            text-align: center;
            vertical-align: middle !important;
            width: 216px !important;
        }
    </style>

    <style>
        .view {
            margin: auto;
            width: 1120px;
        }

        .wrapper {
            position: relative;
            overflow: scroll; /* auto;*/
            white-space: nowrap;
            height: 450px;
        }

        .sticky-col {
            position: sticky;
            position: -webkit-sticky;
            background-color: white;
        }

        .first-col {
            width: 45px;
            min-width: 45px;
            max-width: 45px;
            left: 0px;
        }

        .second-col {
            width: 150px;
            min-width: auto;
            max-width: auto;
            left: 45px;
            border: 1px solid rgb(190, 190, 190);
        }

        .table td, .table th {
            border: 0 !important;
        }

        body {
            font-size: small !important;
        }
    </style>

    <link rel="Stylesheet" href="../assets/bootstrap-4.3.1-dist/css/bootstrap.min.css" />
    <script type="text/javascript" src="../Scripts/jquery-3.5.1.min.js"></script>
    <table cellpadding="0" cellspacing="0" width='100%' class='mGrid_Table'>
        <tr>
            <td colspan="12" align="center" class="head_column">
                <h3 style="text-align: center">Sales Visit Scheduler 
                </h3>
            </td>
        </tr>
    </table>
    <table style="margin-left: 5px; font-size: medium; color: black; padding-bottom: 0px; margin-top: 3px">
        <tr class="">
            <td class="field" style="width: 25px !important; text-align: left !important; padding-right: 10px !important;">Year:
            </td>
            <td style="width: 95px; text-align: left; padding-right: 20px !important;">
                <select id="Year_ddl" runat="server" class="form-control" style="font-size: 12px; padding-right: 20px">
                </select>
            </td>
            <td class="field" style="width: 30px !important; text-align: left !important; padding-right: 10px !important;">Month:
            </td>
            <td style="width: 130px; text-align: left; padding-right: 20px !important;">
                <select id="Month_ddl" runat="server" class="form-control" style="font-size: 12px; margin-right: 15px">
                    <option value="01">January</option>
                    <option value="02">February</option>
                    <option value="03">March</option>
                    <option value="04">April</option>
                    <option value="05">May</option>
                    <option value="06">June</option>
                    <option value="07">July</option>
                    <option value="08">August</option>
                    <option value="09">September</option>
                    <option value="10">October</option>
                    <option value="11">November</option>
                    <option value="12">December</option>
                </select>
            </td>

            <td class="field" style="width: 10% !important; text-align: left !important; padding-right: 10px !important;">Sale Person:
            </td>
            <td style="width: 15%; text-align: left; padding-right: 20px !important;">
                <select id="salesPerson_ddl" runat="server" class="form-control" style="font-size: 12px; margin-right: 15px">
                </select>
                <img id="loading-sales" src="../assets/images/loader-2.gif" style="display: none;" />

            </td>


            <td style="width: 15%">
                <button id="btnFilter" type="button" class="btn " style="background-color: #f27031; color: white; font-size: 12px; width: 60px">
                    Search</button>
            </td>
            <td>
                <b>
                    <div runat="server" id="statusMsg" style="color: red; font: bold"></div>
                </b>

            </td>
        </tr>
    </table>
    
    <div id="loadinggif_filter" style="display: none;">
        <img id="loading-image" src="../assets/images/loader-1.gif" alt="../assets/images/loader-2.gif" />
        <b>Loading</b>
    </div>
    <div class="view">
        <div class="wrapper">
            <table id="DataTableCustomer" class="table DataTableCustomer MainTbl table table-striped TdScroll" style="">
            </table>
        </div>
    </div>
    <div style="margin-top: 10px; margin-bottom: 10px; display: none; margin-left: 70px" id="AddCustomerBtn">
        <button id="btnAddCustomer" type="button" class="btn " style="background-color: #f27031; color: white; font-size: 12px; width: 140px;">
            Add Customer</button>
    
        <button id="btnSaveSheet" type="button" class="btn " style="margin-left: 50px;background-color: #f27031; width: 140px; color: white; font-size: 12px;">
            Save Schedule</button>

         <button id="btnCopyNextMonth" type="button" class="btn " style="margin-left: 50px;background-color: #f27031; width: 140px; color: white; font-size: 12px;">
            Copy to next month</button>
    </div>

    <script type="text/javascript">

        function isNumberKey(evt) {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            return true;
        }

        $(document).ready(function () {
            GetYears();
            var d = new Date();
            var YearNumber = d.getFullYear();
            var monthNumber = d.getMonth();
            monthNumber = monthNumber + 1;
            monthNumber = formatted_string('00', monthNumber, 'l');
            document.getElementById('ContentPlaceHolder1_Month_ddl').value = monthNumber;
            document.getElementById('ContentPlaceHolder1_Year_ddl').value = YearNumber;
            GetSalesPerson();
            debugger;

        });

        var GetYears = () => {
            debugger;
            var d = new Date();
            var n = d.getFullYear();
            var kk = '';

            kk += `<option value="${n + 1}">${n + 1}</option>`;

            for (let y = 0; y < 2; y++) {
                kk += `<option value="${n - y}">${n - y}</option>`;
            }
            $('#ContentPlaceHolder1_Year_ddl').html(kk);
        }

        function formatted_string(pad, user_str, pad_pos) {
            if (typeof user_str === 'undefined')
                return pad;
            if (pad_pos == 'l') {
                return (pad + user_str).slice(-pad.length);
            }
            else {
                return (user_str + pad).substring(0, pad.length);
            }
        }

        var GetSalesPerson = () => {
            debugger;
            $.ajax({
                type: "POST",
                url: 'SalesPersonVisitingCustomer.aspx/GetSalesPerson',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                beforeSend: function () {
                    $("#loading-sales").show();
                },
                success: (rs) => {
                    var kk = '';
                    var rr = rs.d;
                    for (let y of rr) {
                        kk += `<option value="${y.Value}">${y.Text}</option>`;
                    }
                    $('#ContentPlaceHolder1_salesPerson_ddl').html(kk);
                    $("#loading-sales").hide();

                }, error: function (jqXHR, textStatus, errorThrown) {
                }

            });
        }

        var FilterData = (data) => {
            debugger;
            $.ajax({
                async: false,
                type: "POST",
                url: 'SalesPersonVisitingCustomer.aspx/FilterCustomer',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ data: data }),
                beforeSend: function () {
                    $("#loadinggif_filter").show();
                },
                success: (rs) => {
                    debugger;
                    if (rs.d.length > 0) {
                        $('#DataTableCustomer').html(rs.d);
                        document.getElementById('ContentPlaceHolder1_statusMsg').innerText = '';
                        $("#AddCustomerBtn").show();
                    } else {
                        $('#DataTableCustomer').html('');
                        document.getElementById('ContentPlaceHolder1_statusMsg').innerText = 'No record found!';
                        $("#AddCustomerBtn").hide();
                    }
                    $("#loadinggif_filter").hide();
                    CountSelectedDatesAfterFilter();
                }
            });
        };

        $('body').on('click', '#btnFilter', function () {
            debugger;
            $("#AddCustomerBtn").hide();
            document.getElementById('ContentPlaceHolder1_statusMsg').innerText = '';
            var Year_ddl = document.getElementById("ContentPlaceHolder1_Year_ddl").value;
            var Month_ddl = document.getElementById("ContentPlaceHolder1_Month_ddl").value;
            var salesPerson_ddl = document.getElementById("ContentPlaceHolder1_salesPerson_ddl").value;
            var data = {
                Year_ddl: Year_ddl,
                Month_ddl: Month_ddl,
                salesPerson_ddl: salesPerson_ddl
            };
            FilterData(data);
        });

        $('body').on('click', '#btnAddCustomer', function () {
            debugger;
            var Year_ddl = document.getElementById("ContentPlaceHolder1_Year_ddl").value;
            var Month_ddl = document.getElementById("ContentPlaceHolder1_Month_ddl").value;
            var salesPerson_ddl = document.getElementById("ContentPlaceHolder1_salesPerson_ddl").value;
            var tableRows = document.getElementById("DataTableCustomer").rows.length; // 5
            var data = {
                Year_ddl: Year_ddl,
                Month_ddl: Month_ddl,
                salesPerson_ddl: salesPerson_ddl,
                tableRows: tableRows
            };
            AddCustomerRow(data);
        });

        var AddCustomerRow = (data) => {
            debugger;
            $.ajax({
                type: "POST",
                url: 'SalesPersonVisitingCustomer.aspx/AddCustomer',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ data: data }),
                success: (rs) => {
                    debugger;
                    var xTablebody = document.getElementById('DataTableCustomer').getElementsByTagName('tbody')[0];
                    var tr = document.createElement('tr');
                    tr.innerHTML = rs.d;
                    xTablebody.insertRow().innerHTML = rs.d
                    //xTablebody.appendChild(tr);
                }
            });
        }

        $('body').on('click', '#btnSaveSheet', function () {
            debugger;
            var CheckBoxes = [];
            var SaleDate = [];
            var Category = [];
            var Customer = [];
            var AccountNo = [];
            var Zones = [];
            var Branches = [];
            var ExpectedRevenue = [];
            var flagCustomer = true;

            $.each($("input[name='SaleDate']:checked"), function () {
                if ($(this).attr('disabled')) {
                } else {
                    CheckBoxes.push($(this).val());
                    rowIndex = this.parentNode.parentNode.rowIndex;
                    rowIndex = rowIndex - 2;

                    var saleDatee = $(this).attr('class');
                    saleDatee = saleDatee.replace(rowIndex + ' ', '');
                    SaleDate.push(saleDatee);
                    var $row = $(this).closest("tr");    // Find the row
                    //if ($row.find(".Category").text() == "1") {
                    Category.push($row.find(".Category").text()); // Find the text
                    //Customer.push($row.find(".Customer").text()); // Find the text
                    //Branches.push($row.find(".Branch").text()); // Find the text
                    //Zones.push($row.find(".Zone").text()); // Find the text
                    var account = $row.find(".AccountNo").val();
                    if (account.length > 1) {
                        account = account.slice(8);
                    }
                    AccountNo.push(account); // Find the text
                    //} else {
                    if ($row.find(".Customer").val() == "") {
                        flagCustomer = false;
                    }
                    Customer.push($row.find(".Customer").val()); // Find the text
                    Branches.push($row.find(".Branch").val()); // Find the text
                    Zones.push($row.find(".Zone").val()); // Find the text
                    ExpectedRevenue.push($row.find(".ExpectedRevenue").val()); // Find the text
                    //}
                }
            });
            if (!flagCustomer) {
                alert('Please provide customer name! ');
                return;
            }
            var salesPerson = document.getElementById("ContentPlaceHolder1_salesPerson_ddl").value;
            if (SaleDate.length > 0) {
                debugger;
                var data = {
                    Zones: Zones,
                    Branches: Branches,
                    AccountNo: AccountNo,
                    Customer: Customer,
                    Category: Category,
                    SaleDate: SaleDate,
                    salesPerson: salesPerson,
                    ExpectedRevenue: ExpectedRevenue
                }

                $.ajax({
                    type: "POST",
                    url: 'SalesPersonVisitingCustomer.aspx/SaveCustomerSheet',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({ data: data }),
                    success: (rs) => {
                        debugger;
                        alert(rs.d);
                        location.reload();
                        //$('#DataTableCustomer').html('');
                        //document.getElementById('btnFilter').click();
                    }
                });
            } else {
                alert('No date selected!');
                return;
            }
        });

        $('body').on('click', '#btnCopyNextMonth', function () {
            debugger;
            $.ajax({
                type: "POST",
                url: 'SalesPersonVisitingCustomer.aspx/CopyToNextMonth',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: (rs) => {
                    alert(rs.d);
                }
            });
        });

        function CountSelectedDatesAfterFilter() {
            var rows = $('#DataTableCustomer > tbody > tr');
            debugger;
            var table = document.getElementById("DataTableCustomer");

            for (let y = 0; y < rows.length; y++) {
                var count2 = 0;
                $('.' + y + ':input:checkbox').each(function () {
                    if (this.checked == true) {
                        count2++;
                    }
                });
                //$row.find(".VisitCount").text(count2);
                var k = y + 2;
                table.rows[k].cells[14].innerHTML = count2;
            }

        }

        function CountSelectedDates(chckDate) {
            //var VisitCount = 0;
            //$.each($("input[name='SaleDate']:checked"), function () {
            //    VisitCount++;
            //});
            var rowIndex = chckDate.parentNode.parentNode.rowIndex;
            rowIndex = rowIndex - 2;
            var $row = $(chckDate).closest("tr");    // Find the row

            //var checkboxessecondway = $("#5 input[type='checkbox']:checked");
            //var checkboxesSelectedd = $("input[id='5']:checked");
            //var countCheckedCheckboxes = checkboxesSelectedd.length;

            //var $checkboxes = $('#5 input[type="checkbox"]');
            //var countCheckedCheckboxes = $checkboxes.filter(':checked').length;


            var count2 = 0;
            $('.' + rowIndex + ':input:checkbox').each(function () {
                if (this.checked == true) {
                    count2++;
                }
            });
            $row.find(".VisitCount").text(count2);

            debugger;
        }

    </script>


</asp:Content>
