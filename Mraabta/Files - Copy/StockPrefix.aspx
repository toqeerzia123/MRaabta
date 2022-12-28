<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="StockPrefix.aspx.cs" Inherits="MRaabta.Files.StockPrefix" %>


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
         
    </style>
</asp:Content>
<asp:Content ID="form1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%-- Basic Heading --%>
    <table cellpadding="0" cellspacing="0" width='100%' class='mGrid_Table'>
        <tr>
            <td colspan="12" align="center" class="head_column">
                <h3 style="text-align: center">Product Prefix Registration</h3>
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
                                <th style="width: 10%">Zone</th>
                                <th style="width: 20%">Type</th>
                                <th style="width: 20%">Product</th>
                                <th style="width: 10%">CN Length</th>
                                <th style="width: 10%">Prefix</th>
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
                <h3 style="text-align: center">Existing Prefixes</h3>
            </td>
        </tr>
    </table>


    <%-- Search --%>

    <div class="mb-2 mt-1 ml-4" style="border: 1px solid black; width: 95%">
        <div class="row">
            <div class="col-md-12" >
                <div class="FilterArea">
                    <div>


                        <div class="row ml-2">


                            <div class="col-md-2">
                                <div class="form-group">
                                    <label for="formGroupExampleInput" style="font-size: 12px;">Zone</label>
                                    <select class="ddlZonesllll  form-control" id="ZoneDDL" style="font-size: 12px;">
                                    </select>
                                </div>
                            </div>

                            <div class="col-md-2">
                                <div class="form-group">
                                    <label for="formGroupExampleInput" style="font-size: 12px;">Type</label>
                                    <select class="ddlTypeFilter  form-control" id="TypeDDL" style="font-size: 12px;">
                                    </select>
                                </div>
                            </div>

                            <div class="col-md-2">
                                <div class="form-group">
                                    <label for="formGroupExampleInput" style="font-size: 12px;">Product</label>
                                    <select class="ddlProductFilter  form-control" id="ProductDDL" style="font-size: 12px;">
                                    </select>
                                </div>
                            </div>


                            <div class="col-md-2">
                                <div class="form-group">
                                    <label for="formGroupExampleInput" style="font-size: 12px; ">Prefix</label>
                                    <input id="FilterPrefixTxt" onchange="PrefixSearchTxtOnChange()" maxlength="5" autopostback="true" onkeypress="return isNumberKey(event)" type="text" class="form-control" style="font-size: 12px;" />
                                </div>
                            </div>


                            <div class="col-md-2" style="margin-top: 1.5em;">
                                <div class="form-group">
                                    <button id="btnFilter" type="button" class="button btn btn-lg btn-primary btn-outline-dark" style="font-size: 12px; height: 2.5em; width: 5.7em;">Search</button>

                                </div>
                            </div>
                        </div>
                        <div class="shadow rounded " id="FilterTable" style="display: none; ">
                            <div style="width: 99%; margin-left: 6px;  overflow-y: scroll; position: relative; height: 500px;">

                                <table class="MainTbl table table-striped ">
                                    <thead>
                                        <tr style="font-size: 12px;" class="sticky">
                                            <th style="width: 10%">Zone</th>
                                            <th style="width: 10%">Type</th>
                                            <th style="width: 10%">Product</th>
                                            <th style="width: 10%">CN Length</th>
                                            <th style="width: 10%">Prefix</th>
                                            <th style="width: 10%"></th>
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
            getTypes();
            getZones_01();
        });
        $(function () {

            $('body').on('click', '#btnFilter', function () {
                
                var zone = $('.ddlZonesllll').val();
                var Product = $('.ddlProductFilter').val();
                var Prefix = document.getElementById('FilterPrefixTxt').value;

                document.getElementById("FilterTable").style.display = 'block';
                GetFilteredPrefix(zone, Product, Prefix);
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
                    PrefixDetailList: []
                };

                var rows = $('#details').find('tr');
                var productArr = [];
                for (let x of rows) {
                    var row = $(x);
                    var zoneCode = (row.find('td:eq(0)').find('select').val());
                    var ProductId = parseInt(row.find('td:eq(2)').find('select').val());
                    var PrefixLength = parseInt(row.find('td:eq(3)').find('input[type=text]').val());
                    var PrefixId = parseInt(row.find('td:eq(4)').find('input[type=text]').val());
                    productArr.push(ProductId);
                    if (PrefixLength <= 0 || isNaN(PrefixLength)) {
                        alert('Please provide valid length');
                        return;
                    }
                    if (PrefixId <= 0 || isNaN(PrefixId)) {
                        alert('Please provide valid prefix');
                        return;
                    }
                    data.PrefixDetailList.push({
                        zoneCode: zoneCode,
                        ProductId: ProductId,
                        PrefixLength: PrefixLength,
                        PrefixId: PrefixId
                    });
                }


                $.ajax({
                    type: "POST",
                    url: 'StockPrefix.aspx/SavePrefixEntries',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({ model: data }),
                    success: (rs) => {
                        alert(rs.d);
                        document.getElementById('form1').reset();
                        $('#details').html('');
                    }
                });

            });

            $('body').on('click', '.btnDisable', function () {
                
                if (confirm("Are you sure you want to Disable this prefix")) {
                    var id = $(this).val();
                    var ZoneCode = $(this).data().id;

                    DisablePrefix(id, ZoneCode);

                    var zone = $('.ddlZonesllll').val();
                    var Product = $('.ddlProductFilter').val();

                    document.getElementById("FilterTable").style.display = 'block';
                    var Prefix = document.getElementById('FilterPrefixTxt').value;
                    GetFilteredPrefix(zone, Product, Prefix);
                }
            });


            $('body').on('change', '.ddlTypeFilter', function () {
                

                var id = $(this).val();
                var kk = ProductFromType(id);
                //getProductsSingle(id);

                $('.ddlProductFilter').html(kk);
            });


            $('body').on('change', '.typeId', function () {
                

                var id = $(this).val();
                var kk = ProductFromType(id);
                //getProductsSingle(id);

                $(this).parent().parent().find('td:eq(2)').find('.productChangeId').html(kk);
            });
        });

        function PrefixSearchTxtOnChange() {
            
            var prefixTxt = document.getElementById('FilterPrefixTxt').value;
            if (prefixTxt == '') {
                document.getElementById('ProductDDL').disabled = false;
                document.getElementById('TypeDDL').disabled = false;
                document.getElementById('ZoneDDL').disabled = false;
            } else {
                document.getElementById('ProductDDL').disabled = true;
                document.getElementById('TypeDDL').disabled = true;
                document.getElementById('ZoneDDL').disabled = true;
            }
        }

        var getZones_01 = () => {
            $.ajax({
                async: false,
                type: "POST",
                url: 'StockPrefix.aspx/GetZones',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: (rs) => {
                    var kk = '';
                    kk += `<option value="0">All Zones</option>`;
                    var rr = rs.d;
                    zones = rs.d;
                    for (let y of rr) {
                        kk += `<option value="${y.Value}">${y.Text}</option>`;
                    }
                    $('.ddlZonesllll').html(kk);
                }
            });
        };


        var GetFilteredPrefix = (zone, Product, Prefix) => {
            $.ajax({
                type: 'post',
                url: 'StockPrefix.aspx/GetFilteredPrefix',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ zone: zone, Product: Product, Prefix: Prefix }),
                success: (rs) => {
                    var data = rs.d;
                    var rows = '';
                    for (let x of data) {
                        rows += `<tr>
                                    <td>${x.ZoneName}</td>
                                    <td>${x.TypeName}</td>
                                    <td>${x.ProductName}</td>
                                    <td>${x.PrefixLength}</td>
                                    <td>${x.PrefixId}</td>                                    
                                   <td>
                                        <button type="button"  CssClass="button" class="button btn btn-sm btn-outline-dark btnDisable" value="${x.PrefixId}" data-id="${x.ZoneCode}">Disable</button>
                                    </td>
                                </tr>`;
                    }

                    $('#FilteredInventory').html(rows);
                }
            });
        };

        var DisablePrefix = (Id, ZoneCode) => {
            $.ajax({
                type: 'post',
                url: 'StockPrefix.aspx/DisablePrefix',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ Id: Id, ZoneCode: ZoneCode }),
                success: (rs) => {
                    alert(rs.d);
                }
            });
        };

        var Types = [];
        var products = [];

        var getProducts = () => {
            $.ajax({
                type: "POST",
                url: 'StockPrefix.aspx/GetProducts',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: (rs) => {
                    products = rs.d;
                }
            });
        };

        var getTypes = () => {
            $.ajax({
                type: "POST",
                url: 'StockPrefix.aspx/GetTypeProducts',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: (rs) => {
                    Types = rs.d;
                    var rows = '';
                    for (let y of Types) {
                        rows += `<option value="${y.Value}">${y.Text}</option>`;
                    }
                    $('.ddlTypeFilter').html(rows);
                    var Products = ProductFromType(1);
                    $('.ddlProductFilter').html(Products);
                }
            });
        };

        var selectt = '';
        function ProductFromType(Id) {
            $.ajax({
                async: false,
                type: "POST",
                url: 'StockPrefix.aspx/GetProductsSingle',
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({ Id: Id }),
                dataType: "json",
                success: (rs) => {

                    var productsSingleTpe = rs.d;
                    selectt = '';
                    for (let x of productsSingleTpe) {
                        selectt += `<option value="${x.Value}">${x.Text}</option>`;
                    }
                    return selectt;
                }
            });
            return selectt;
        }

        //important
        var addRow = () => {
            var zone = '';
            var prods = '';
            var types = '';
            for (let y of Types) {
                types += `<option value="${y.Value}">${y.Text}</option>`;
            }
            for (let y of zones) {
                zone += `<option value="${y.Value}">${y.Text}</option>`;
            }
            var gg = ProductFromType(1);

            var html = `<tr>
                            <td>
                                <select class="form-control zoneName" id="zoneName" style="width:10em; font-size:12px;">
                                ${zone}
                                </select>
                            </td>
                            <td style="font-size:12px;">
                                <select class="form-control typeId" style="font-size:12px;">
                                ${types}
                                </select>
                            </td>
                            <td style="font-size:12px;">
                                <select class="form-control productChangeId"  style="font-size:12px;">
                                ${gg}
                                </select>
                            </td>

                            <td style="font-size:12px;"><input maxlength="2" autopostback="true"  onkeypress="return isNumberKey(event)" type="text" class="form-control" style="font-size:12px;" /></td>
                            <td style="font-size:12px;"><input maxlength="5" autopostback="true"  onkeypress="return isNumberKey(event)" type="text" class="form-control" style="font-size:12px;" /></td>
                            <td><button type="button" class="btn btn-sm btn-danger btnRemove" style="font-size:12px;">Remove</button></td>
                        </tr>`;

            $('#details').append(html);
        };


    </script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
</asp:Content>

