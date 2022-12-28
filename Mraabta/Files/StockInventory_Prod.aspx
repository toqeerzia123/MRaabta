<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="StockInventory_Prod.aspx.cs" Inherits="MRaabta.Files.StockInventory_Prod" %>

<asp:Content ContentPlaceHolderID="head" ID="ContentHead" runat="Server">


    <link href="<%=ResolveUrl("~/Content/bootstrap.min.css") %>" rel="stylesheet" />
    <script type="text/javascript" src="<%=ResolveUrl("~/Scripts/jquery-3.5.1.min.js") %>"></script>
    <script type="text/javascript" src="<%=ResolveUrl("~/Scripts/bootstrap.min.js") %>"></script>

    <script language="javascript" type="text/javascript">
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


        //   $('#btnAdd').click(function () {
        //        addRow();
        //    });

        //$("#btn_add").on("click", function (e) {
        //e.preventDefault();
        //$(this).attr("disabled", "disabled");

        //$(this).removeAttr("disabled");
        //});
        // var addRow = () => {
        //    var prods = '';

        //    for (let x of  ViewState["records"]) {
        //        prods += `<option value="${x.ProductName}">${x.ID}</option>`;
        //    }

        //    var html = `<tr>
        //                    <td>
        //                        <select class="form-control">
        //                        ${prods}
        //                        </select>
        //                    </td>
        //                    <td><input type="number" class="form-control" /></td>
        //                    <td><button type="button" class="btn btn-sm btn-danger btnRemove">Remove</button></td>
        //                </tr>`;

        //$('#details').append(html);
        //};
    </script>
</asp:Content>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" ID="Content1" runat="server" class="mt-5">

    <style type="text/css">
        .MainTbl th {
            background-color: #404040;
            color: white;
        }

        #btnModify {
            height: 35px;
            width: 150px;
        }

        #btnFilter {
            height: 35px;
            width: 150px;
        }
    </style>

    <table cellpadding="0" cellspacing="0" width='100%' class='mGrid_Table'>
        <tr>
            <td colspan="12" align="center" class="head_column">
                <h3 style="text-align: center">Inventory </h3>
            </td>
        </tr>
        </tr>
    </table>
    <div class="container mb-2 mt-1" style="border: 1px solid black; width: 90%">
        <div class="row">
            <div class="col-12">
                <div class="row mt-1">

                    <table class="table table-bordered col-9 ml-2" style="padding: 1em">
                        <thead>
                            <tr style="font-size: 12px;">
                                <th>Zone</th>
                                <th>Type</th>
                                <th>Product</th>
                                <th>Quantity</th>
                                <th>Barcode From</th>
                                <th>Barcode To</th>
                                <th></th>

                            </tr>
                        </thead>
                        <tbody id="details">
                        </tbody>
                    </table>
                    <div class="col-2 ml-2 mb-1">
                        <button id="btnAdd" type="button" class="btn btn-sm" style="background-color: #f27031; color: white; height: 3em; font-size: 12px;">Add</button>
                        <button id="btnSave" type="button" class="btn btn-sm" style="background-color: #f27031; color: white; height: 3em; font-size: 12px;">Save</button>
                    </div>
                </div>
            </div>
            <div id="LoaderDiv" style="display:none"><img id="LoaderImg" src="../assets/images/loader-1.gif" alt="" /></div>
        </div>
    </div>

    <div class="row">
        <div class="col-md-6">
            <div class="FilterArea">
                <div>
                    <table cellpadding="0" cellspacing="0" width='100%' class='mGrid_Table'>
                        <tr>
                            <td colspan="8" align="center" class="head_column">
                                <h3 class="ml-2" style="text-align: center; font-size: 14px; height: 2.4em;">Filter For Inventory Modification
                                </h3>
                            </td>
                        </tr>
                    </table>

                    <%--<h3 class="bg-dark text-white">Filter Date Wise--%>
                    <div class="row ml-3">
                        <div class="col-md-3">
                            <div class="form-group">
                                <label for="formGroupExampleInput" style="font-size: 12px;">Start Date</label>
                                <input type="date" name="StartDate" id="StartDate" class="form-control" style="font-size: 12px;" />
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="form-group">
                                <label for="formGroupExampleInput" style="font-size: 12px;" >End Date</label>
                                <input type="date" name="EndDate" id="EndDate" class="form-control" style="font-size: 12px;" />
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="form-group">
                                <label for="formGroupExampleInput" style="font-size: 12px;">Zone</label>
                                <select class="ddlZonesllll  form-control" id="inputState" style="font-size: 12px;">
                                </select>
                            </div>
                        </div>
                        <div class="col-md-3 mt-4">
                            <button id="btnFilter" type="button" class="button btn btn-lg btn-primary btn-outline-dark" style="font-size: 12px; width: 6em; height: 2.4em;">Search</button>
                        </div>
                    </div>
                </div>

                <div class="shadow rounded ml-2" id="FilterTable" style="display: none;">
                    <div class="">

                        <table class="MainTbl table table-striped">
                            <thead>
                                <tr style="width: 2em; font-size: 12px;">
                                    <th>Zone</th>
                                    <th>Type</th>
                                    <th>Product</th>
                                    <th>Qty</th>
                                    <th>Barcode From</th>
                                    <th>Barcode To</th>
                                    <th></th>
                                </tr>
                            </thead>
                            <tbody id="FilteredInventory">
                            </tbody>

                        </table>
                        <br />

                    </div>
                </div>
            </div>
        </div>
        <div class="col-md-6">
            <div>
                <%--<table cellpadding="0" cellspacing="0" width='100%' class='mGrid_Table ml-1'>
            <tr>
                <td colspan="12" align="center" class="head_column">
                    <h3 style="text-align: center">Stocks Recieved
                    </h3>
                </td>
            </tr>
        </table>--%>
                <div class="tab-content" id="myTabContent">
                    <div class="tab-pane fade show active" id="inventory" role="tabpanel" aria-labelledby="inventory-tab"></div>
                    <%-- <div class="tab-pane fade" id="stock" role="tabpanel" aria-labelledby="stock-tab"></div>--%>
                </div>


                <table cellpadding="0" cellspacing="0" width='100%' class='mGrid_Table'>
                    <tr>
                        <td>
                            <nav>
                                    <div class="nav nav-tabs nav-fill" id="nav-tab" role="tablist">
                                        <a class="nav-item nav-link active" id="nav-inventory-tab" data-toggle="tab" href="#nav-inventory" role="tab" aria-controls="nav-inventory" aria-selected="true" style="font-size:14px; height:2.4em;">Head Office Stock</a>
                                      <%--  <a class="nav-item nav-link" id="nav-stock-tab" data-toggle="tab" href="#nav-stock" role="tab" aria-controls="nav-stock" aria-selected="false">Head Office Stock</a>--%>
                                    </div>
                            </nav>
                        </td>
                    </tr>
                </table>

                <div class="tab-content" id="nav-tabContent">


                    <div class="tab-pane fade show active" id="nav-inventory" role="tabpanel" aria-labelledby="nav-inventory-tab">

                        <table class="MainTbl table table-striped mt-1">
                            <thead>
                                <tr style="font-size: 12px;">

                                    <th>Zone</th>
                                    <th>Type</th>
                                    <th>Product</th>
                                    <th>Quantity</th>
                                </tr>
                            </thead>
                            <tbody id="inventoryHOTable" style="font-size: 12px;">
                            </tbody>

                        </table>
                    </div>
                </div>

            </div>
        </div>
    </div>




    <div class="modal fade" id="ModalPopUp" role="dialog">
        <div class="modal-dialog modal-lg">
            <div class="modal-content" style="width: 60em">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <%--<h4 class="modal-title">Modal Header</h4>--%>
                </div>
                <div class="modal-body">
                    <div id="EditTable" style="display: none;">
                        <table cellpadding="0" cellspacing="0" width='100%' class='mGrid_Table'>
                            <tr>
                                <td colspan="12" align="center" class="head_column">
                                    <h3 style="text-align: center; font-size: 12px;">Modify Request
                                    </h3>
                                </td>
                            </tr>
                        </table>
                        <%--<h3 class="bg-dark text-white">Per Stock Details</h3>--%>
                        <div class="shadow rounded">
                            <div class="">

                                <table class="MainTbl table table-striped" style="width: 100%">
                                    <thead>
                                        <tr style="font-size: 12px;">
                                            <th>Zone</th>
                                            <th>Type</th>
                                            <th>Product</th>
                                            <th>Quantity</th>
                                            <th>Barcode From</th>
                                            <th>barcode To</th>
                                            <th></th>
                                        </tr>
                                    </thead>
                                    <tbody id="reqPerStock" style="font-size: 12px;">
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <div class="text-right " id="btnDiv"></div>

                </div>
            </div>
        </div>
    </div>


    <script type="text/javascript">
        var getZones_01 = () => {

            $.ajax({
                async: false,
                type: "POST",
                url: 'StockInventory.aspx/GetZones',
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
        var TypeProducts = [];

        $(document).ready(function () {
            getTypes();
            getZones_01();

        });
        $(function () {

            GetHOInventory();
            GetHOStock();
            $('body').on('click', '#btnFilter', function () {

                var StartDate = document.getElementById('StartDate').value;
                var EndDate = document.getElementById('EndDate').value;
                var zone = $('.ddlZonesllll').val();

                document.getElementById("FilterTable").style.display = 'block';
                GetFilteredStockZone(StartDate, EndDate, zone);
            });
        });

        var GetHOInventory = () => {
            $.ajax({
                type: 'post',
                url: 'StockInventory.aspx/GetHOInventory',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: (rs) => {
                    var data = rs.d;
                    var rows = '';
                    for (let x of data) {
                        if (x.Qty > 0) {
                            rows += `<tr>   
                                    <td>${x.ZoneCode}</td>
                                    <td >${x.TypeName}</td>
                                    <td>${x.ProductName}</td>
                                    <td >${x.Qty_}</td>   
                             </tr>`;
                        }
                    }

                    $('#inventoryHOTable').html(rows);
                }
            });
        };

        var GetHOStock = () => {
            $.ajax({
                type: 'post',
                url: 'StockInventory.aspx/GetHOStock',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: (rs) => {
                    var data = rs.d;
                    var rows = '';
                    for (let x of data) {
                        rows += `<tr>   
                                    <td>${x.ZoneCode}</td>
                                    <td >${x.TypeName}</td>
                                    <td>${x.ProductName}</td>
                                    <td >${x.Qty}</td>   
                             </tr>`;
                    }

                    $('#StockHOTable').html(rows);
                }
            });
        };

        var GetFilteredStockZone = (StartDate, EndDate, zone) => {
            $.ajax({
                type: 'post',
                url: 'StockInventory.aspx/GetFilteredStockZone',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ StartDate: StartDate, EndDate: EndDate, zone: zone }),
                success: (rs) => {
                    var data = rs.d.Data;
                    var rows = '';
                    if (!(rs.d.status)) {
                        alert(rs.d.statusMessage);
                        $('#FilteredInventory').html('');
                        return;
                    }
                    if (data.length == 0) {
                        alert('No record found!');
                        $('#FilteredInventory').html('');
                        return;
                    }
                    for (let x of data) {

                        if (x.Qty > 0) {
                            if (x.isupdated) {
                                rows += `<tr>   
                                    <td style="width:2em; font-size:12px;"><input type="hidden"  id="ZoneIDModifyTable"  value="${x.ZoneCode}"><label id="ZoneModifyTable">${x.name}</label></td>
                                    <td style="width:2em; font-size:12px;"><input type="hidden"  id="TypeIDModifyTable"  value="${x.TypeId}"><label id="TypeModifyTable">${x.TypeName}</label></td>
                                    <td  style="width:2em; font-size:12px;"><input type="hidden" id="ProductIDModifyTable"  value="${x.ProductId}"><label id="ProductModifyTable">${x.ProductName}</label></td>
                                    <td style="width:2em; font-size:12px;"><label id="QuantityModifyTable">${x.Qty_}</td>                                                                     
                                    <td style="width:2em; font-size:12px;"><label id="BarcodefromModifyTable">${x.BarcodeFrom}</label></td>
                                    <td style="width:2em; font-size:12px;"><label id="BarcodeToModifyTable">${x.BarcodeTo}</label></td>                                                                    
                                    <td>
                                        <button type="button"  CssClass="button" class="button btn btn-sm btn-outline-dark btnView" value="${x.StockID}" data-id="${x.Inv_ID}" disabled>Edit</button>
                                    </td>
                                </tr>`;
                            }
                            else {
                                rows += `<tr>   
                                     <td style="width:2em; font-size:12px;"><input type="hidden"  id="ZoneIDModifyTable"  value="${x.ZoneCode}"><label id="ZoneModifyTable">${x.name}</label></td>
                                    <td style="width:2em; font-size:12px;"><input type="hidden"  id="TypeIDModifyTable"  value="${x.TypeId}"><label id="TypeModifyTable">${x.TypeName}</label></td>
                                    <td  style="width:2em; font-size:12px;"><input type="hidden" id="ProductIDModifyTable"  value="${x.ProductId}"><label id="ProductModifyTable">${x.ProductName}</label></td>
                                    <td style="width:2em; font-size:12px;"><label id="QuantityModifyTable">${x.Qty_}</td>                                                                     
                                    <td style="width:2em; font-size:12px;"><label id="BarcodefromModifyTable">${x.BarcodeFrom}</label></td>
                                    <td style="width:2em; font-size:12px;"><label id="BarcodeToModifyTable">${x.BarcodeTo}</label></td>                                                                   
                                    <td>
                                        <button type="button"  CssClass="button" class="button btn btn-sm btn-outline-dark btnView" value="${x.StockID}" data-id="${x.Inv_ID}" >Edit</button>
                                    </td>
                                </tr>`;
                            }
                        }


                    }

                    $('#FilteredInventory').html(rows);
                }
            });
        };

        var getRecordDetails = (qty, BarcodeFrom, barcodeTo, stock_id, inv_id, zoneID, zonename, productID, productName, typeID, typeName) => {
            
            //$.ajax({
            //type: 'post',
            //url: 'StockInventory.aspx/GetFilteredStockZonePer',
            //contentType: "application/json; charset=utf-8",
            //dataType: "json",
            //async:false,
            //data: JSON.stringify({ qty: qty, zone: zoneID, product: productName, BarcodeFrom: BarcodeFrom, barcodeTo: barcodeTo }),
            //success: (rs) => {

            //var data = rs.d;
            var rows = '';
            var prods = '';
            var types = '';
            var zoness = '';
            //var typeId = data.TypeId;
            //var productId = data.ProductId;
            //var zoneId = data.ZoneCode;

            for (let x of zones) {
                if (x.Value == zoneID) {
                    zoness += `<option selected="selected" value="${x.Value}">${x.Text}</option>`;
                } else {
                    zoness += `<option value="${x.Value}">${x.Text}</option>`;
                }
            }


            for (let x of TypeProducts) {
                if (x.Value == typeID) {
                    types += `<option selected="selected" value="${x.Value}">${x.Text}</option>`;
                } else {
                    types += `<option value="${x.Value}">${x.Text}</option>`;
                }
            }
            var productssList = '';
            $.ajax({
                type: "POST",
                url: 'StockInventory.aspx/GetProductsSingle',
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({ Id: typeID }),
                dataType: "json",
                async: false,
                success: (rs) => {

                    var productsSingleTpe = rs.d;
                    productssList = '';
                    for (let x of productsSingleTpe) {
                        if (productID == x.Value) {
                            productssList += `<option selected="selected" value="${x.Value}">${x.Text}</option>`;
                        } else {
                            productssList += `<option value="${x.Value}">${x.Text}</option>`;
                        }
                    }
                    return productssList;
                }
            });



            rows += `<tr>
                                    <td id="tdZoneModal"><input type="hidden" value="">
                                          <select style="width:10em; font-size:12px;" class="form-control zoneName" id="dropdownZonesModal">
                                         ${zoness}
                                         </select>
                                 </td> <td style="width:12em;" id="dropType">
                                        <select style=" font-size:12px;" class="form-control typeId"  id="dropdownTypeModal" onchange="ModalTypeChange()">
                                         ${types}
                                        </select>
                                    </td>

                                  <td id="tdProductModal">
                                        <select class="form-control productChangeId"  id="dropdownProductModal" style="width:10em; font-size:12px;" >
                                          ${productssList}
                                        </select>
                                        
                                 </td>
                               

                                    <td style="width:10em;" id="Qtyy"><input type="text" style="font-size:12px;" class="form-control " autopostback="true"  onkeypress="return isNumberKey(event)" maxlength="6" value="${qty}" id="QtyModal" name="QtyModal"/></td> 
                                    <td><input type="text" style="font-size:12px;" class="form-control txtbarcodefrom" id="barcodeFromModal" autopostback="true"  onkeypress="return isNumberKey(event)" maxlength="12"  value="${BarcodeFrom}"/></td>
                                    <td><input type="text" style="font-size:12px;" class="form-control txtbarcodeto" readonly="readonly" id="barcodeToModal" value="${barcodeTo}"/></td>
                        </tr>`;
            $('#reqPerStock').html(rows);
            $('#btnDiv').html(`<button id="btnModify" CssClass="submitBtn button"  type="button" class="button btn btn-primary btn-outline-dark btn-lg " data-id="${stock_id}" value="${inv_id}" >Submit</button>`);




            //});
        };
        //var getRecordDetails = (qty, zone, product, BarcodeFrom, barcodeTo) => {
        //    
        //    $.ajax({
        //        type: 'post',
        //        url: 'StockInventory.aspx/GetFilteredStockZonePer',
        //        contentType: "application/json; charset=utf-8",
        //        dataType: "json",
        //        data: JSON.stringify({ qty: qty, zone: zone, product: product, BarcodeFrom: BarcodeFrom, barcodeTo: barcodeTo }),
        //        success: (rs) => {
        //            
        //            var data = rs.d;
        //            var rows = '';
        //            var prods = '';
        //            var types = '';
        //            var zoness = '';


        //            for (let x of TypeProducts) {
        //                types += `<option value="${x.Value}">${x.Text}</option>`;
        //            }
        //            for (let x of zones) {
        //                zoness += `<option value="${x.Value}">${x.Text}</option>`;
        //            }

        //            //var kkk = foo(data.TypeId);
        //            var kkk= foo(1);

        //            rows += `<tr>
        //                            <td id="tdZoneModal"><input type="hidden" value="${data.Id}">
        //                                  <select style="width:8em; font-size:12px;" class="form-control" id="dropdownZonesModal">
        //                                 ${zoness}
        //                                 </select>
        //                         </td> <td style="width:12em;" id="dropType">
        //                                <select style=" font-size:12px;" class="form-control"  id="dropdownTypeModal" onchange="ModalTypeChange()">
        //                                 ${types}
        //                                </select>
        //                            </td>

        //                          <td id="tdProductModal">
        //                                <select class="form-control"  id="dropdownProductModal" style="font-size:12px;" >
        //                                  ${kkk}
        //                                </select>

        //                         </td>


        //                            <td style="width:10em;" id="Qtyy"><input type="text" style="font-size:12px;" class="form-control" autopostback="true"  onkeypress="return isNumberKey(event)" maxlength="6" value="${data.Qty}" id="QtyModal" name="QtyModal"/></td> 
        //                            <td><input type="text" style="font-size:12px;" class="form-control" id="barcodeFromModal" autopostback="true"  onkeypress="return isNumberKey(event)" maxlength="12"  value="${data.BarcodeFrom}"/></td>
        //                            <td><input type="text" style="font-size:12px;" class="form-control" readonly="readonly" id="barcodeToModal" value="${data.BarcodeTo}"/></td>
        //                </tr>`;
        //            $('#reqPerStock').html(rows);
        //            $('#btnDiv').html(`<button id="btnModify" CssClass="submitBtn button"  type="button" class="button btn btn-primary btn-outline-dark btn-lg " data-id="${data.Id}" value="${data.Id}" >Submit</button>`);

        //        }
        //    });
        //};
        var EditPerRecord = (StartDate, EndDate) => {
            $.ajax({
                type: 'post',
                url: 'StockInventory.aspx/EditPerRecord',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ StartDate: StartDate, EndDate: EndDate }),
                success: (rs) => {
                    var data = rs.d;
                    var rows = '';
                }
            });
        };
        var ModifyRequest = (inv_id, stock_id, valueZone, valueType, valueProd, qtyModal, barcodeFrm, barcodeto) => {
            $.ajax({
                type: 'post',
                url: 'StockInventory.aspx/ModifyRequest',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ Inv_id: inv_id, stock_id: stock_id, valueZone: valueZone, valueType: valueType, valueProd: valueProd, qtyModal: qtyModal, barcodeFrm: barcodeFrm, barcodeToo: barcodeto }),
                success: (rs) => {
                    alert(rs.d);
                }
            });
        }
        var products = [];
        var Types = [];
        var zones = [];

        //var getProducts = () => {
        //    $.ajax({
        //        type: "POST",
        //        url: 'StockInventory.aspx/GetProducts',
        //        contentType: "application/json; charset=utf-8",
        //        dataType: "json",
        //        success: (rs) => {
        //            products = rs.d;
        //        }
        //    });
        //};

        var getMyStock = () => {

            $.ajax({
                type: 'post',
                url: 'StockRequestZone.aspx/GetMyStock',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: (rs) => {
                    var data = rs.d;
                    var rows = '';

                    for (let x of data) {
                        rows += `<tr>
                                    <td>${x.ProductName}</td>
                                    <td>${x.BarcodeFrm}</td>
                                    <td>${x.BarcodeTo}</td>
                                    <td>${x.Qty}</td>
                                    <td>${x.Year}</td>
                                    
                                </tr>`;

                    }
                    $('#myStockGrid').html(rows);
                    $('#mystock').removeClass('d-none');
                }
            });
        };

        var productsModal = [];
        var getProductsModal = (Id) => {
            $.ajax({
                type: "POST",
                url: 'StockInventory.aspx/GetProductsSingle',
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({ Id: Id }),
                dataType: "json",
                success: (rs) => {
                    productsModal = rs.d;
                    var selectt = '';
                    for (let x of productsModal) {
                        selectt += `<option value="${x.Value}">${x.Text}</option>`;
                    }
                    var selectTD = `<select class="form-control" id="dropdownProductModal" style="font-size:12px; width:10em;">${selectt} </select>`

                    document.getElementById('tdProductModal').innerHTML = selectTD;

                }
            });
        };

        var selectt = '';
        function foo(Id) {
            $.ajax({
                type: "POST",
                url: 'StockInventory.aspx/GetProductsSingle',
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({ Id: Id }),
                dataType: "json",
                async: false,
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


        var getTypes = () => {
            $.ajax({
                async: false,
                type: "POST",
                url: 'StockInventory.aspx/GetTypes',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: (rs) => {
                    Types = rs.d;
                    TypeProducts = rs.d;
                }
            });
        };

        var getZones = () => {

            $.ajax({
                type: "POST",
                url: 'StockInventory.aspx/GetZones',
                contentType: "application/json; charset=utf-8",
                dataType: "json",

                success: (rs) => {
                    zones = rs.d;
                }
            });
        };

        var getProducts = () => {
            $.ajax({
                type: "POST",
                url: 'StockInventory.aspx/GetProducts',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: (rs) => {
                    products = rs.d;
                }
            });
        };

        var GetTypeProducts = () => {
            $.ajax({
                type: "POST",
                url: 'StockInventory.aspx/GetTypes',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: (rs) => {
                    TypeProducts = rs.d;
                }
            });
        };

        var save = (data) => {
            $.ajax({
                type: "POST",
                url: 'StockInventory.aspx/SaveStockDetails',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ model: data }),
                success: (rs) => {
                    alert(rs.d);
                    document.getElementById('form1').reset();
                    $('#details').html('');
                    document.getElementById('btnSave').disabled = false;
                    document.getElementById('LoaderDiv').style.display = 'none';
                    GetHOInventory();
                    GetHOStock();

                },
                error: (error) => {
                    alert('Error:' + error.statusText);

                    document.getElementById('btnSave').disabled = false;
                    document.getElementById('LoaderDiv').style.display = 'none';
                }
            });
        };

        //Product Type on change on Modal
        function ModalTypeChange() {

            var productsModal = [];
            var Id = document.getElementById('dropdownTypeModal').value;
            var prods = '';

            getProductsModal(Id);
        }

        var validations = (barcodeFrm, barcodeto, data) => {

            $.ajax({
                type: "POST",
                url: 'StockInventory.aspx/CheckSequenceDuplication',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ barcodeFrm: barcodeFrm, barcodeto: barcodeto }),
                success: (rs) => {
                    var chk = rs.d;
                    if (chk) {
                        alert("Barcode Already Exist");
                        document.getElementById('btnSave').disabled = false;
                        document.getElementById('LoaderDiv').style.display = 'none';
                        return
                    }
                    else {
                        save(data);

                    }
                },
                error: (error) => {
                    alert('Error in request ' + error.statusText);
                    document.getElementById('btnSave').disabled = false;
                    document.getElementById('LoaderDiv').style.display = 'none';
                }

            });
        };

        var validationsforModal = (barcodeFrm, barcodeto, valueZone, valueType, valueProd, qtyModal, stock_id, inv_id) => {

            $.ajax({
                type: "POST",
                url: 'StockInventory.aspx/ValidateWithDetailIdInModal',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ barcodeFrm: barcodeFrm, barcodeto: barcodeto, Detailid: inv_id }),
                success: (rs) => {
                    var chk = rs.d;
                    if (chk) {
                        alert("Barcode Already Exist");
                        return
                    }
                    else {
                        //jQuery.noConflict();
                        $('#ModalPopUp').modal('toggle');
                        ModifyRequest(inv_id, stock_id, valueZone, valueType, valueProd, qtyModal, barcodeFrm, barcodeto);
                    }
                }

            });
        };

        var addRow = () => {

            var Type = '';
            var zone = '';
            var prods = '';
            var barcodeto_html = '';

            for (let x of Types) {
                Type += `<option value="${x.Value}">${x.Text}</option>`;
            }
            for (let y of zones) {
                zone += `<option value="${y.Value}">${y.Text}</option>`;
            }

            var gg = foo(1);
            var html = `<tr><td>
                                <select class="form-control zoneName" id="zoneName" style="width:10em; font-size:12px;">
                                ${zone}
                                </select>
                            </td>
                             <td>
                                <select class="form-control typeId" style="width:10em; font-size:12px;">
                                ${Type}
                                </select>
                            </td>   
                            <td>
                                <select class="form-control productChangeId" style="width:10em; font-size:12px;">
                                ${gg}
                                </select>
                            </td>           
                            <td><input maxlength="6" class="form-control txtissueqty" autopostback="true"  onkeypress="return isNumberKey(event)" type="text" id="quantity" style="width:6em; font-size:12px;"/></td>
                            <td><input maxlength="15" class="form-control txtbarcodefrom" autopostback="true"  onkeypress="return isNumberKey(event)" type="text" id="barfrom" style="width:9em; font-size:12px;"/></td>
                            <td><input class="form-control txtbarcodeto" type="text" readonly="readonly" style="width:9em; font-size:12px;" id="barto"/></td>
                            <td><button type="button" class="btn btn-sm btn-danger btnRemove" style="font-size:12px;">Remove</button></td>
                        </tr>`;
            $('#details').append(html);
        };

        var prefix;
        var pre_len;
        function getPrefix(ZoneId, TypeId, ProductId, Barcode) {

            $.ajax({
                type: "POST",
                url: 'StockInventory.aspx/GetPrefixes',
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({ ZoneId: ZoneId, TypeId: TypeId, ProductId: ProductId, Barcode: Barcode }),
                dataType: "json",
                async: false,
                success: (rs) => {

                    if (rs.d.Text != null && rs.d.Value != null) {
                        //alert(rs.d.Text + "-" + rs.d.Value);
                        prefix = rs.d.Text;
                        pre_len = rs.d.Value;
                    }
                    else {
                        prefix = null;
                        pre_len = null;
                    }
                }
            });
        }
        //function getPrefix(TypeId) {
        //    $.ajax({
        //        type: "POST",
        //        url: 'StockInventory.aspx/GetPrefixes',
        //        contentType: "application/json; charset=utf-8",
        //        data: JSON.stringify({ TypeId : TypeId }),
        //        dataType: "json",
        //        async: false,
        //        success: (rs) => {
        //            if (rs != null) {
        //                alert(rs.d.Text + "-" + rs.d.Value);
        //                prefix = rs.d.Text;
        //                    pre_len = rs.d.Value;
        //            }
        //        }
        //    });

        //}
        $(function () {
            //getTypes();
            //GetTypeProducts();
            //getZones();
            //getProducts();

            $('body').on('click', '.btnView', function () {


                var zoneID = $(this).parent().parent().find('td:eq(0)').find('#ZoneIDModifyTable').val();
                var zonename = $(this).parent().parent().find('td:eq(0)').find('#ZoneModifyTable').prevObject[0].innerText;

                var productID = $(this).parent().parent().find('td:eq(2)').find('#ProductIDModifyTable').val();
                var productName = $(this).parent().parent().find('td:eq(2)').find('#ProductModifyTable').prevObject[0].innerText;

                var typeID = $(this).parent().parent().find('td:eq(1)').find('#TypeIDModifyTable').val();
                var typeName = $(this).parent().parent().find('td:eq(1)').find('#TypeModifyTable').prevObject[0].innerText;;

                var stock_id = $(this).val();
                var inv_id = $(this).data().id;

                var BarcodeFrom = $(this).parent().parent().find('td:eq(4)').find('#BarcodefromModifyTable').prevObject[0].innerText;
                var barcodeTo = $(this).parent().parent().find('td:eq(5)').find('#BarcodeToModifyTable').prevObject[0].innerText;
                var qty = $(this).parent().parent().find('td:eq(3)').find('#QuantityModifyTable').prevObject[0].innerText;
                qty = qty.replace(',', '');
                getRecordDetails(qty, BarcodeFrom, barcodeTo, stock_id, inv_id, zoneID, zonename, productID, productName, typeID, typeName);

                var x = document.getElementById("EditTable");
                x.style.display = "block";
                //jQuery.noConflict();
                $('#ModalPopUp').modal('show');

            });

            $('body').on('click', '#btnModify', function () {

                var stock_id = $(this).data().id;
                var inv_id = $(this).val();

                var selectorZone = document.getElementById('dropdownZonesModal');
                var valueZone = selectorZone[selectorZone.selectedIndex].value;
                var selector = document.getElementById('dropdownTypeModal');
                var valueType = parseInt(selector[selector.selectedIndex].value);

                var selectorProd = document.getElementById('dropdownProductModal');
                if (selectorProd == null) {
                    alert('Please select a Product');
                    return;
                }
                var valueProd = parseInt(selectorProd[selectorProd.selectedIndex].value);
                var qtyModal = parseInt(document.getElementById('QtyModal').value);
                var barcodeFrm = parseInt(document.getElementById('barcodeFromModal').value);
                var barcodeTo = parseInt(document.getElementById('barcodeToModal').value);
                if ($("#barcodeFromModal").is(".is-invalid")) {
                    alert('Invalid Prefix or Barcode length.');
                    return;

                }
                if (qtyModal <= 0 || isNaN(qtyModal)) {
                    alert('Please provide valid Quantity');
                    return;
                } if (barcodeFrm <= 0 || isNaN(barcodeFrm)) {
                    alert('Please provide valid barcode starting number');
                    return;
                }

                validationsforModal(barcodeFrm, barcodeTo, valueZone, valueType, valueProd, qtyModal, stock_id, inv_id);
                var StartDate = document.getElementById('StartDate').value;
                var EndDate = document.getElementById('EndDate').value;
                var zone = $('.ddlZonesllll').val();

                document.getElementById("FilterTable").style.display = 'block';
                GetFilteredStockZone(StartDate, EndDate, zone);
            });
            //getTypes();
            //getZones();
            getProducts(1);

            $('body').on('change', '.typeId', function () {
                $(this).parent().parent().find('td:eq(2)').find('.productChangeId').html('');
                //var row = source.closest('tr');

                var id = $(this).val();
                /// Get the Prefix -- rabeea
                //getProductsSingle(id);
                var kk = foo(id);
                var skushtml = '';
                //var p_id = "1";
                $(this).parent().parent().find('td:eq(2)').find('.productChangeId').html(kk);
                getPrefix($(this).parent().parent().find('td:eq(0)').find('select').val(), $(this).parent().parent().find('td:eq(1)').find('select').val(), $(this).parent().parent().find('td:eq(2)').find('select').val(), $(this).parent().parent().find('td:eq(4)').find('input[type=text]').val());

                if (pre_len == null && prefix == null) {
                    $(this).parent().parent().find('td:eq(4)').find('input[type=text]').addClass('is-invalid');
                    $(this).parent().parent().find('td:eq(4)').find('input[type = text]').attr('maxlength', '15');
                }
                else {
                    $(this).parent().parent().find('td:eq(4)').find('input[type = text]').attr('maxlength', pre_len);
                    $(this).parent().parent().find('td:eq(5)').find('.txtbarcodeto').attr('maxlength', pre_len);
                    $(this).parent().parent().find('td:eq(4)').find('input[type=text]').attr('data-prefix', prefix);
                    if ($(this).parent().parent().find('td:eq(4)').find('input[type=text]').attr("data-prefix") == 1) {
                        if ($(this).parent().parent().find('td:eq(4)').find('input[type=text]').val().length != $(this).parent().parent().find('td:eq(4)').find('input[type=text]').attr("maxlength")) {
                            $(this).parent().parent().find('td:eq(4)').find('input[type=text]').addClass('is-invalid');
                        }
                        else {
                            $(this).parent().parent().find('td:eq(4)').find('input[type=text]').removeClass('is-invalid');
                        }
                    }
                    else {
                        if (!($(this).parent().parent().find('td:eq(4)').find('input[type=text]').val().match($(this).parent().parent().find('td:eq(4)').find('input[type=text]').attr("data-prefix")))) {
                            $(this).parent().parent().find('td:eq(4)').find('input[type=text]').addClass('is-invalid');
                        }
                        else {
                            $(this).parent().parent().find('td:eq(4)').find('input[type=text]').removeClass('is-invalid');
                            if ($(this).parent().parent().find('td:eq(4)').find('input[type=text]').val().length != $(this).parent().parent().find('td:eq(4)').find('input[type=text]').attr("maxlength")) {
                                $(this).parent().parent().find('td:eq(4)').find('input[type=text]').addClass('is-invalid');
                            }
                            else {
                                $(this).parent().parent().find('td:eq(4)').find('input[type=text]').removeClass('is-invalid');
                            }
                        }
                    }


                } 
            });

            $('body').on('change', '.productChangeId', function () {
                getPrefix($(this).parent().parent().find('td:eq(0)').find('select').val(), $(this).parent().parent().find('td:eq(1)').find('select').val(), $(this).parent().parent().find('td:eq(2)').find('select').val(), $(this).parent().parent().find('td:eq(4)').find('input[type=text]').val());

                if (pre_len == null && prefix == null) {
                    $(this).parent().parent().find('td:eq(4)').find('input[type=text]').addClass('is-invalid');
                    $(this).parent().parent().find('td:eq(4)').find('input[type = text]').attr('maxlength', '15');
                }
                else {
                    $(this).parent().parent().find('td:eq(4)').find('input[type = text]').attr('maxlength', pre_len);
                    $(this).parent().parent().find('td:eq(5)').find('.txtbarcodeto').attr('maxlength', pre_len);
                    $(this).parent().parent().find('td:eq(4)').find('input[type=text]').attr('data-prefix', prefix);
                    if ($(this).parent().parent().find('td:eq(4)').find('input[type=text]').attr("data-prefix") == 1) {
                        if ($(this).parent().parent().find('td:eq(4)').find('input[type=text]').val().length != $(this).parent().parent().find('td:eq(4)').find('input[type=text]').attr("maxlength")) {
                            $(this).parent().parent().find('td:eq(4)').find('input[type=text]').addClass('is-invalid');
                        }
                        else {
                            $(this).parent().parent().find('td:eq(4)').find('input[type=text]').removeClass('is-invalid');
                        }
                    }
                    else {
                        if (!($(this).parent().parent().find('td:eq(4)').find('input[type=text]').val().match($(this).parent().parent().find('td:eq(4)').find('input[type=text]').attr("data-prefix")))) {
                            $(this).parent().parent().find('td:eq(4)').find('input[type=text]').addClass('is-invalid');
                        }
                        else {
                            $(this).parent().parent().find('td:eq(4)').find('input[type=text]').removeClass('is-invalid');
                            if ($(this).parent().parent().find('td:eq(4)').find('input[type=text]').val().length != $(this).parent().parent().find('td:eq(4)').find('input[type=text]').attr("maxlength")) {
                                $(this).parent().parent().find('td:eq(4)').find('input[type=text]').addClass('is-invalid');
                            }
                            else {
                                $(this).parent().parent().find('td:eq(4)').find('input[type=text]').removeClass('is-invalid');
                            }
                        }

                    }

                }
            });

            $('body').on('change', '.zoneName', function () {
                getPrefix($(this).parent().parent().find('td:eq(0)').find('select').val(), $(this).parent().parent().find('td:eq(1)').find('select').val(), $(this).parent().parent().find('td:eq(2)').find('select').val(), $(this).parent().parent().find('td:eq(4)').find('input[type=text]').val());

                if (pre_len == null && prefix == null) {
                    $(this).parent().parent().find('td:eq(4)').find('input[type=text]').addClass('is-invalid');
                    $(this).parent().parent().find('td:eq(4)').find('input[type = text]').attr('maxlength', '15');
                }
                else {
                    $(this).parent().parent().find('td:eq(4)').find('input[type = text]').attr('maxlength', pre_len);
                    $(this).parent().parent().find('td:eq(5)').find('.txtbarcodeto').attr('maxlength', pre_len);
                    $(this).parent().parent().find('td:eq(4)').find('input[type=text]').attr('data-prefix', prefix);
                    if ($(this).parent().parent().find('td:eq(4)').find('input[type=text]').attr("data-prefix") == 1) {
                        if ($(this).parent().parent().find('td:eq(4)').find('input[type=text]').val().length != $(this).parent().parent().find('td:eq(4)').find('input[type=text]').attr("maxlength")) {
                            $(this).parent().parent().find('td:eq(4)').find('input[type=text]').addClass('is-invalid');
                        }
                        else {
                            $(this).parent().parent().find('td:eq(4)').find('input[type=text]').removeClass('is-invalid');
                        }
                    }
                    else {
                        if (!($(this).parent().parent().find('td:eq(4)').find('input[type=text]').val().match($(this).parent().parent().find('td:eq(4)').find('input[type=text]').attr("data-prefix")))) {
                            $(this).parent().parent().find('td:eq(4)').find('input[type=text]').addClass('is-invalid');
                        }
                        else {
                            $(this).parent().parent().find('td:eq(4)').find('input[type=text]').removeClass('is-invalid');
                            if ($(this).parent().parent().find('td:eq(4)').find('input[type=text]').val().length != $(this).parent().parent().find('td:eq(4)').find('input[type=text]').attr("maxlength")) {
                                $(this).parent().parent().find('td:eq(4)').find('input[type=text]').addClass('is-invalid');
                            }
                            else {
                                $(this).parent().parent().find('td:eq(4)').find('input[type=text]').removeClass('is-invalid');
                            }
                        } 
                    } 
                }
            });
            $('#btnAdd').click(function () {
                addRow();
            });
            $('#barcodeFromModal').click(function () {

                var oldvalue = $(this).val();
                var field = this;
                setTimeout(function () {
                    if (field.value.indexOf(oldvalue) !== 0) {
                        $(field).val(oldvalue);
                    }
                }, 1);
            });
            //barcode_fromChange
            $('body').on('blur', '#barcodeFromModal', function (e) { 
                var bcodeto = '';
                var qty = parseInt($(this).parent().parent().find('td:eq(3)').find('input[type=text]').val());
                var barcodefrom = parseInt($(this).val());
                if (qty == '' || barcodefrom == '') {

                }
                
                var ProductToCheck = parseInt($(this).parent().parent().find('td:eq(2)').find('select').val());
                var barcodeto = 0;
                if (ProductToCheck == 17) {
                    barcodefrom = barcodefrom.toString().slice(0, -1);
                    barcodefrom = (parseInt(barcodefrom) + qty) - 1;
                    var OCSANA = Math.floor(barcodefrom / 7);
                    OCSANA = barcodefrom - OCSANA * 7;
                    OCSANA = barcodefrom.toString().concat(OCSANA);
                    barcodeto = OCSANA;
                } else {
                    barcodeto = (barcodefrom + qty) - 1;
                }
                 
                $('#barcodeToModal').val(barcodeto.toString().padStart('0'));
                 
            });

            //barcode_fromChange
            $('body').on('blur', '#QtyModal', function (e) {
                 
                var bcodeto = '';
                var qty = parseInt($(this).val());
                
                var barcodefrom = parseInt($(this).parent().parent().find('td:eq(4)').find('input[type=text]').val());
                //var barcodefrom = 0;
                if (qty == '' || barcodefrom == '') {

                }
                var ProductToCheck = parseInt($(this).parent().parent().find('td:eq(2)').find('select').val());
                var barcodeto = 0;
                if (ProductToCheck == 17) {
                    barcodefrom = barcodefrom.toString().slice(0, -1);
                    barcodefrom = (parseInt(barcodefrom) + qty) - 1;
                    var OCSANA = Math.floor(barcodefrom / 7);
                    OCSANA = barcodefrom - OCSANA * 7;
                    OCSANA = barcodefrom.toString().concat(OCSANA);
                    barcodeto = OCSANA;
                } else {
                    barcodeto = (barcodefrom + qty) - 1;
                }
                $('#barcodeToModal').val(barcodeto.toString().padStart('0'));
            });

            //qty_Change
            $('body').on('blur', '.txtissueqty', function () {
                var bcodeto = '';
                var qty = parseInt($(this).val());

                var barcodefrom = parseInt($(this).parent().parent().find('td:eq(4)').find('input[type=text]').val());
                //var barcodefrom = 0;
                if (qty == '' || barcodefrom == '') {

                }
                
                var barcodeto = (barcodefrom + qty) - 1;
                 
                var ProductToCheck = parseInt($(this).parent().parent().find('td:eq(2)').find('select').val());
                var barcodeto = 0;
                if (ProductToCheck == 17) {
                    barcodefrom = barcodefrom.toString().slice(0, -1);
                    barcodefrom = (parseInt(barcodefrom) + qty) - 1;
                    var OCSANA = Math.floor(barcodefrom / 7);
                    OCSANA = barcodefrom - OCSANA * 7;
                    OCSANA = barcodefrom.toString().concat(OCSANA);
                    barcodeto = OCSANA;
                } else {
                    barcodeto = (barcodefrom + qty) - 1;
                }
                $(this).parent().parent().find('td:eq(5)').find('#barto').val(barcodeto.toString().padStart(pre_len, '0'));
            });

            //barcode_fromChange -- 
            $('body').on('blur', '.txtbarcodefrom', function (e) {

                getPrefix($(this).parent().parent().find('td:eq(0)').find('select').val(), $(this).parent().parent().find('td:eq(1)').find('select').val(), $(this).parent().parent().find('td:eq(2)').find('select').val(), this.value);
                if (pre_len == null && prefix == null) {
                    $(this).addClass('is-invalid');
                }
                else {
                    $(this).parent().parent().find('td:eq(4)').find('.txtbarcodefrom').attr('maxlength', pre_len);
                    $(this).parent().parent().find('td:eq(5)').find('.txtbarcodeto').attr('maxlength', pre_len);
                    $(this).parent().parent().find('td:eq(4)').find('.txtbarcodefrom').attr('data-prefix', prefix);
                    if ($(this).parent().parent().find('td:eq(4)').find('input[type=text]').attr("data-prefix") == 1) {
                        if ($(this).parent().parent().find('td:eq(4)').find('input[type=text]').val().length != $(this).parent().parent().find('td:eq(4)').find('input[type=text]').attr("maxlength")) {
                            $(this).parent().parent().find('td:eq(4)').find('input[type=text]').addClass('is-invalid');
                        }
                        else {
                            $(this).parent().parent().find('td:eq(4)').find('input[type=text]').removeClass('is-invalid');
                        }
                    }
                    else {
                        if (!(this.value.match($(this).attr("data-prefix")))) {
                            $(this).addClass('is-invalid');
                        }
                        else {
                            $(this).removeClass('is-invalid');
                            if (this.value.length != $(this).attr("maxlength")) {
                                $(this).addClass('is-invalid');
                            }
                            else {
                                $(this).removeClass('is-invalid');
                            }
                        }
                    } 
                }

                var bcodeto = '';
                var qty = parseInt($(this).parent().parent().find('td:eq(3)').find('input[type=text]').val());
                var barcodefrom = parseInt($(this).val());
                if (qty == '' || barcodefrom == '') {

                } 
                var barcodeto = (barcodefrom + qty) - 1; 
                var ProductToCheck = parseInt($(this).parent().parent().find('td:eq(2)').find('select').val());
                var barcodeto = 0;
                if (ProductToCheck == 17) {
                    barcodefrom = barcodefrom.toString().slice(0, -1);
                    barcodefrom = (parseInt(barcodefrom) + qty) - 1;
                    var OCSANA = Math.floor(barcodefrom / 7);
                    OCSANA = barcodefrom - OCSANA * 7;
                    OCSANA = barcodefrom.toString().concat(OCSANA);
                    barcodeto = OCSANA;
                } else {
                    barcodeto = (barcodefrom + qty) - 1;
                }
                $(this).parent().parent().find('td:eq(5)').find('#barto').val(barcodeto.toString().padStart(pre_len, '0'));

            });

            $('body').on('click', '.btnRemove', function () {
                $(this).parent().parent().remove();
            });

            $('#btnSave').click(function () {
                var qty = parseInt($(this).parent().parent().find('.quantity').val()); 

                var rowLen = $('#details').find('tr').length;
                if (rowLen <= 0) {
                    alert('Please Add Details');
                    return;
                }

                var data = {

                    StockRequestDetails: []
                };

                var rows = $('#details').find('tr');
                var productArr = [];
                var zoneCodeArr = [];

                for (let x of rows) {

                    var row = $(x);
                    var zoneCode = (row.find('td:eq(0)').find('select').val());
                    var typeId = parseInt(row.find('td:eq(1)').find('select').val());
                    var productId = parseInt(row.find('td:eq(2)').find('select').val());
                    var qty = parseInt(row.find('td:eq(3)').find('input[type=text]').val());
                    var barcodeFrm = parseInt(row.find('td:eq(4)').find('input[type=text]').val());
                    var barcodeto = parseInt(row.find('td:eq(5)').find('input[type=text]').val());
                    var barcodelen = row.find('td:eq(4)').find('input[type=text]').attr("maxlength");
                    if (row.find('td:eq(4)').find("#barfrom").is(".is-invalid")) {
                        alert('Invalid Prefix or Barcode length.');
                        return;
                    }

                    if (qty <= 0 || isNaN(qty)) {
                        alert('please provide valid quantity');
                        return;
                    }
                    if (barcodeFrm <= 0 || isNaN(barcodeFrm)) {
                        alert('please provide valid barcode starting number');
                        return;
                    }
                    if (barcodeto <= 0 || isNaN(barcodeto)) {
                        alert('please check barcode to field');
                    }
                    if (productId <= 0 || isNaN(productId)) {
                        return;
                    }
                    productArr.push(productId);
                    zoneCodeArr.push(zoneCode);

                    //lala
                    for (var i = 0; i < data.StockRequestDetails.length; i++) {
                        if (barcodeFrm >= data.StockRequestDetails[i].BarcodeFrm && barcodeFrm <= data.StockRequestDetails[i].BarcodeTo) {
                            alert('please check barcode to field');
                            return;
                        }
                        else if (barcodeto <= data.StockRequestDetails[i].BarcodeFrm && barcodeto >= data.StockRequestDetails[i].BarcodeTo) {
                            alert('please check barcode to field');
                            return;
                        }
                    } 

                    data.StockRequestDetails.push({
                        ZoneCode: zoneCode,
                        TypeId: typeId,
                        ProductId: productId,
                        Qty: qty,
                        BarcodeFrm: barcodeFrm,
                        BarcodeTo: barcodeto
                    });
                }

                var statusSameProduct = true;
                for (var k = 0; k <= productArr.length; k++) {
                    for (var j = k + 1; j < productArr.length; j++) {
                        if (productArr[k] == productArr[j]) {
                            if (zoneCodeArr[k] == zoneCodeArr[j]) {
                                statusSameProduct = false;
                            }
                        }
                    }
                }
                if (!statusSameProduct) {
                    alert('Cannot register same product & zone at same time.');
                    return;
                }

                document.getElementById('btnSave').disabled = true;
                document.getElementById('LoaderDiv').style.display = '';
                validations(barcodeFrm, barcodeto, data); 
            });
        });
    </script> 
</asp:Content>