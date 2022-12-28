<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="StockItem.aspx.cs" Inherits="MRaabta.Files.StockItem" %>


<asp:Content ContentPlaceHolderID="head" ID="Contenthead" runat="Server">


    <link href="<%=ResolveUrl("~/Content/bootstrap.min.css") %>" rel="stylesheet" />
    <script type="text/javascript" src="<%=ResolveUrl("~/Scripts/jquery-3.5.1.min.js") %>"></script>
    <script type="text/javascript" src="<%=ResolveUrl("~/Scripts/bootstrap.min.js") %>"></script>

    <script language="javascript" type="text/javascript">
        window.onload = function () {
            noBack();
        }
        function noBack() {
            window.history.forward();
        }
        $(function () {
            $("#txt_qty").bind('keypress', function (e) {
                var charCode = (evt.which) ? evt.which : evt.keyCode;
                if (charCode > 31 && (charCode < 48 || charCode > 57))
                    return false;
                return true;
            });
            $("#txt_qty").bind("paste", function (e) {
                e.preventDefault();
            });

        });
        $(function () {
            $("#txt_barcode_from").bind('keypress', function (e) {
                var charCode = (evt.which) ? evt.which : evt.keyCode;
                if (charCode > 31 && (charCode < 48 || charCode > 57))
                    return false;
                return true;
            });
            $("#txt_barcode_from").bind("paste", function (e) {
                e.preventDefault();
            });

        });

        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }

    </script>
    <style type="text/css">
        .MainTbl th {
            background-color: #404040;
            color: white;
            position: -webkit-sticky;
            position: sticky;
            top: 0;
            z-index: 5;
        }

        #btnFilter {
            width: 120px;
            height: 35px;
        }

        #btnSubmit {
            width: 120px;
            height: 35px;
            float: left;
        }

        #btnModify {
            height: 35px;
            width: 150px;
        }


        .switch {
            position: relative;
            display: inline-block;
            width: 50px;
            height: 24px;
        }

            .switch input {
                opacity: 0;
            }

        .slider {
            position: absolute;
            cursor: pointer;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background-color: #ccc;
            -webkit-transition: .4s;
            transition: .4s;
        }

            .slider:before {
                position: absolute;
                content: "";
                height: 16px;
                width: 16px;
                left: 4px;
                bottom: 4px;
                background-color: white;
                -webkit-transition: .4s;
                transition: .4s;
            }

        input:checked + .slider {
            background-color: #f27031;
        }

        input:focus + .slider {
            box-shadow: 0 0 1px #f27031;
        }

        input:checked + .slider:before {
            -webkit-transform: translateX(26px);
            -ms-transform: translateX(26px);
            transform: translateX(26px);
        }

        /* Rounded sliders */
        .slider.round {
            border-radius: 34px;
        }

            .slider.round:before {
                border-radius: 50%;
            }
    </style>
</asp:Content>
<asp:Content ID="form1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%-- Basic Heading --%>
    <table cellpadding="0" cellspacing="0" width='100%' class='mGrid_Table'>
        <tr>
            <td colspan="12" align="center" class="head_column">
                <h3 style="text-align: center">Add New Stock Item</h3>
            </td>
        </tr>
    </table>

    <%-- Adding Request Form --%>
    <div class="mb-2 mt-1 ml-4" style="border: 1px solid black; width: 95%">
        <div class="row">
            <div class="col-12">
                <div class="row mt-1 ml-4">
                    <table class="table table-bordered col-10">
                        <thead>
                            <tr style="font-size: 12px;">
                                <th style="width: 20%">Oracle Code</th>
                                <th style="width: 20%">Type</th>
                                <th style="width: 20%">item Name</th>
                                <th style="width: 15%">is Serialized</th>
                                <th style="width: 20%">Unit</th>
                                <th style="width: 5%"></th>
                            </tr>
                        </thead>
                        <tbody id="details">
                        </tbody>
                    </table>
                    <div class="col-2">
                        <button id="btnAdd" type="button" class="btn btn-sm" style="background-color: #f27031; color: white; height: 3em; font-size: 12px;">Add</button>
                        <button id="btnSave" type="button" class="btn btn-sm" style="background-color: #f27031; color: white; height: 3em; font-size: 12px;">Save</button>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <table cellpadding="0" cellspacing="0" width='100%' class='mGrid_Table'>
        <tr>
            <td colspan="12" align="center" class="head_column">
                <h3 style="text-align: center">Existing Items</h3>
            </td>
        </tr>
    </table>

    <%-- Search --%>
    <div class="mb-2 mt-1 ml-4" style="border: 1px solid black; width: 95%">
        <div class="row">
            <div class="col-md-12">
                <div class="FilterArea">
                    <div>
                        <div class="row ml-2">
                            <div class="col-md-2">
                                <div class="form-group">
                                    <label for="formGroupExampleInput" style="font-size: 12px;">Oracle Code</label>
                                    <input id="FilterOracleCodeTxt" autopostback="true" type="text" class="form-control" style="font-size: 12px;" />
                                </div>
                            </div>
                            <div class="col-md-2" style="margin-top: 1.5em;">
                                <div class="form-group">
                                    <button id="btnFilter" type="button" class="button btn btn-lg btn-primary btn-outline-dark" style="font-size: 12px; height: 2.5em; width: 5.7em;">Search</button>
                                </div>
                            </div>
                        </div>

                        <div class="shadow rounded " id="FilterTable" style="">
                            <div style="width: 99%; margin-left: 6px; overflow-y: scroll; position: relative; height: 500px;">

                                <table class="MainTbl table table-striped ">
                                    <thead>
                                        <tr style="font-size: 12px;" class="sticky">
                                            <th style="width: 20%">Oracle Code</th>
                                            <th style="width: 20%">Type</th>
                                            <th style="width: 20%">item Name</th>
                                            <th style="width: 20%">is Serialized</th>
                                            <th style="width: 20%">Unit</th>
                                        </tr>
                                    </thead>
                                    <tbody id="FilteredInventory" style="font-size: 12px;">
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>


    <%-- Scripts --%>
    <script type="text/javascript" src="../assets/bootstrap-4.3.1-dist/js/bootstrap.min.js"></script>

    <script type="text/javascript">       
        $(document).ready(function () {
            GetTypes();
            GetUnits();
        });
        $(function () {

            $('body').on('click', '#btnFilter', function () {

                var OracleCode = document.getElementById('FilterOracleCodeTxt').value;
                if (OracleCode == "" || OracleCode == null) {
                    alert("Enter Oracle Code");
                }
                else {
                    document.getElementById("FilterTable").style.display = 'block';
                    GetFilteredItems(OracleCode);
                }
            });

            $('#btnAdd').click(function () {

                addRow();
            });

            $('body').on('click', '.btnRemove', function () {
                $(this).parent().parent().remove();
            });

            $('#btnSave').click(function () {

                var rowLen = $('#details').find('tr').length;
                if (rowLen <= 0) {
                    alert('Please Add Details');
                    return;
                }

                var data = {
                    ItemDetailList: []
                };

                var rows = $('#details').find('tr');

                for (let x of rows) {
                    var row = $(x);
                    var oraclecode = row.find('td:eq(0)').find('input').val();
                    var typeid = row.find('td:eq(1)').find('select').val();
                    var itemname = row.find('td:eq(2)').find('input').val();
                    var isSerialised = row.find('td:eq(3)').find('select').val();
                    var unit = row.find('td:eq(4)').find('select').val();

                    if (oraclecode == null || oraclecode == "") {
                        alert('Please enter Oracle Name');
                        return;
                    }
                    if (typeid == null || typeid == "") {
                        alert('Please select Type');
                        return;
                    }
                    if (itemname == null || itemname == "") {
                        alert('Please enter Item');
                        return;
                    }

                    data.ItemDetailList.push({
                        OracleCode: oraclecode,
                        ProductName: itemname,
                        TypeName: typeid,
                        isSerialised: isSerialised,
                        Unit: unit
                    });
                }

                $.ajax({
                    type: "POST",
                    url: 'StockItem.aspx/SaveItemEntries',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({ model: data.ItemDetailList }),
                    success: (rs) => {
                        alert(rs.d);
                        document.getElementById('form1').reset();
                        $('#details').html('');
                    }
                });
            });
        });

        var types = [];
        function GetTypes() {
            $.ajax({
                type: "POST",
                url: 'StockItem.aspx/GetStockTypes',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: (rs) => {
                    types = rs.d;
                }
            });
        };

        var units = [];
        function GetUnits() {
            $.ajax({
                type: "POST",
                url: 'StockItem.aspx/GetStockUnits',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: (rs) => {
                    units = rs.d;
                }
            });
        };

        function GetFilteredItems(OracleCode) {
            $.ajax({
                type: 'post',
                url: 'StockItem.aspx/GetStockItems',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ OracleCode: OracleCode }),
                success: (rs) => {
                    var data = rs.d;
                    var rows = '';
                    for (let x of data) {
                        rows += `<tr>
                                    <td>${x.OracleCode}</td>
                                    <td>${x.ProductName}</td>
                                    <td>${x.TypeName}</td>
                                    <td>${x.isSerialised}</td>
                                    <td>${x.Unit}</td>
                                </tr>`;
                    }
                    $('#FilteredInventory').html(rows);
                    if (rows == "") {
                        alert("No Item found");
                    }
                }
            });
        };

        //important
        var addRow = () => {
            //$(this).parent().parent().attr.getName();
            var typeSelect = '';
            var unitSelect = '';
            for (let y of types) {
                typeSelect += `<option value="${y.Value}">${y.Text}</option>`;
            }
            for (let y of units) {
                unitSelect += `<option value="${y.Value}">${y.Text}</option>`;
            }

            var html = `<tr> 
                            <td style="font-size:12px;">
                                <input class="form-control" onfocusout="CheckOracleCode(this)"  style="font-size:12px;"/>
                            </td>
                            <td>
                                <select class="form-control" style="width:10em; font-size:12px;">
                                ${typeSelect}
                                </select>
                            </td>
                            <td style="font-size:12px;">
                                <input class="form-control"  style="font-size:12px;"/>
                            </td>
                            <td style="font-size:12px;">
                                <select class="form-control" style="font-size:12px;">
                                    <option value=0>No</option>
                                    <option value=1>Yes</option>
                                </select>
                               </td>
                            <td>
                                <select class="form-control" style="width:10em; font-size:12px;">
                                ${unitSelect}
                                </select>
                            </td>
                           <td> <button type="button" class="btn btn-sm btn-danger btnRemove" style="font-size:12px;">Remove</button></td>
                        </tr>`;

            $('#details').append(html);
            document.getElementById('ContentPlaceHolder1_abc').disabled = true;
        }; 

        function CheckOracleCode(txt) {
            var oracle = txt.value;
            $.ajax({
                type: 'post',
                url: 'StockItem.aspx/VerifyOracleCode',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ OracleCode: oracle }),
                success: (rs) => {
                    var data = rs.d;
                    if (rs.d != null) {
                        txt.value = '';
                        alert('Oracle Code Already Exists !!');
                    }
                }
            });

        }
    </script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
</asp:Content>

