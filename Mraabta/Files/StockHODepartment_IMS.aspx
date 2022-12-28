<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="StockHODepartment_IMS.aspx.cs" Inherits="MRaabta.Files.StockHODepartment_IMS" %>

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
                <h3 style="text-align: center">Stocks Request (HO Departments)</h3>
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
                                <th style="width: 20%">Department</th>
                                <th style="width: 20%">Oracle Code</th>
                                <th style="width: 20%">Type</th>
                                <th style="width: 20%">Product</th>
                                <th style="width: 5%">Quantity</th>
                                <th style="width: 5%">Unit</th>
                                <th style="width: 10%">Action</th>
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
            <div id="LoaderDiv" style="display: none">
                <img id="LoaderImg" src="../assets/images/loader-1.gif" alt="" />
            </div>
        </div>
    </div>

    <div class="row">
        <%-- Search --%>
        <div class="col-md-6">
            <div class="FilterArea">
                <div>
                    <table cellpadding="0" cellspacing="0" width='100%' class='mGrid_Table'>
                        <tr>
                            <td colspan="12" align="center" class="head_column">
                                <h3 style="text-align: center; font-size: 16px;" class="ml-2">Request Modification
                                </h3>
                            </td>
                        </tr>
                    </table>

                    <div class="row ml-2">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label for="formGroupExampleInput" style="font-size: 12px;">Start Date</label>
                                <input type="date" name="StartDate" id="StartDate" class="form-control" style="font-size: 12px;" />
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label for="formGroupExampleInput" style="font-size: 12px;">End Date</label>
                                <input type="date" name="EndDate" id="EndDate" class="form-control" style="font-size: 12px;" />
                            </div>
                        </div>
                        <div class="col-md-4" style="margin-top: 1.5em;">
                            <div class="form-group">
                                <button id="btnFilter" type="button" class="button btn btn-lg btn-primary btn-outline-dark" style="font-size: 12px; height: 2.5em; width: 5.7em;">Search</button>
                            </div>
                        </div>
                    </div>
                    <div class="shadow rounded " id="FilterTable" style="display: none;">
                        <div style="width: 99%;">

                            <table class="MainTbl table table-striped ">
                                <thead>
                                    <tr style="font-size: 12px;">
                                        <th>Department</th>
                                        <th>Req Date</th>
                                        <th>Req Id</th>
                                        <th>Total Qty</th>
                                        <th></th>
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

        <%-- Show Stock --%>
        <div class="col-md-6" style="width: 95%; overflow-y: scroll; position: relative; height: 600px;">
            <div class="tab-content" id="myTabContent">
                <div class="tab-pane fade active" id="zone-stock" role="tabpanel" aria-labelledby="zone-tab"></div>
                <div class="tab-pane fade" id="ho-stock" role="tabpanel" aria-labelledby="ho-tab"></div>
            </div>

            <table cellpadding="0" cellspacing="0" width='100%' class='mGrid_Table'>
                <tr>
                    <td>
                        <nav>
                            <div class="nav nav-tabs nav-fill" id="nav-tab" role="tablist">
                                <a class="nav-item nav-link active" id="nav-zone-tab" data-toggle="tab" href="#nav-zone" role="tab" aria-controls="nav-zone" aria-selected="false" style="font-size: 14px;">HO Stock</a>
                            </div>
                        </nav>
                    </td>
                </tr>
            </table>

            <div class="tab-content" id="nav-tabContent">
                <div class="tab-pane fade show active" id="nav-zone" role="tabpanel" aria-labelledby="nav-zone-tab">
                    <table class="MainTbl table table-striped mt-1">
                        <thead>
                            <tr style="font-size: 12px;">
                                <th>Oracle Code</th>
                                <th>Type</th>
                                <th>Product</th>
                                <th>Quantity</th>
                            </tr>
                        </thead>
                        <tbody id="HOTable" style="font-size: 12px;">
                        </tbody>
                    </table>
                </div>
            </div>
        </div>

    </div>

    <%-- Edit Model --%>
    <div class="modal fade" id="ModalPopUp" role="dialog">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                </div>
                <div class="modal-body">
                    <div id="EditTable" style="display: none;">
                        <table cellpadding="0" cellspacing="0" width='100%' class='mGrid_Table'>
                            <tr>
                                <td colspan="12" align="center" class="head_column">
                                    <h3 style="text-align: center">Modify Request
                                    </h3>
                                </td>
                            </tr>
                        </table>
                        <%--<h3 class="bg-dark text-white">Per Stock Details</h3>--%>
                        <div class=" shadow rounded">
                            <div class="">

                                <table class="MainTbl table table-striped" style="width: 100%">
                                    <thead>
                                        <tr style="font-size: 12px;">
                                            <th style="width: 15%">Oracle Code</th>
                                            <th style="width: 15%">Type</th>
                                            <th style="width: 15%">Product</th>
                                            <th style="width: 15%">Quantity</th>
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
                    <%-- <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>--%>
                    <div class="text-right " id="btnDiv"></div>

                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="ModalDetails" role="dialog">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                </div>
                <div class="modal-body">
                    <div id="EditTableDetails" style="display: none;">
                        <table cellpadding="0" cellspacing="0" width='100%' class='mGrid_Table'>
                            <tr>
                                <td colspan="12" align="center" class="head_column">
                                    <h3 style="text-align: center">Details
                                    </h3>
                                </td>
                            </tr>
                        </table>
                        <%--<h3 class="bg-dark text-white">Per Stock Details</h3>--%>

                        <div class=" shadow rounded">
                            <div class="ml-2">

                                <table class="MainTbl table table-striped" style="width: 100%">
                                    <thead>
                                        <tr style="font-size: 12px;">
                                            <th style="width: 15%">Type</th>
                                            <th style="width: 15%">Product</th>
                                            <th style="width: 15%">Quantity</th>
                                        </tr>
                                    </thead>
                                    <tbody id="reqPendingStockDetails" style="font-size: 12px;">
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <%--  <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>              --%>
                </div>
            </div>
        </div>
    </div>
    <link href="../mazen/css/sweetalert.css" rel="stylesheet" />
    <%-- Scripts --%>
    <script type="text/javascript">

</script>
    <script type="text/javascript" src="../mazen/js/sweetalert-dev.js"></script>
    <script type="text/javascript" src="../assets/bootstrap-4.3.1-dist/js/bootstrap.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            getTypes();
            GetDepartments();
        });
        $(function () {
            GetHOStock();
            GetIssuedRequest();

            $('body').on('click', '#btnFilter', function () {

                var StartDate = document.getElementById('StartDate').value;
                var EndDate = document.getElementById('EndDate').value;

                document.getElementById("FilterTable").style.display = 'block';
                if (StartDate == '' || EndDate == '') {
                    alert("Select dates");
                }
                else if (((new Date(EndDate) - new Date(StartDate)) / (1000 * 60 * 60 * 24)) < 0) {
                    alert("Select valid dates");
                }
                else if (((new Date(EndDate) - new Date(StartDate)) / (1000 * 60 * 60 * 24)) > 30) {
                    alert("Maximum days limit is 30");
                }
                else {
                    GetFilteredStockZone(StartDate, EndDate);
                }
            });

            getProducts();
            getTypes();
            GetDepartments();

            $('#btnAdd').click(function () {
                addRow();
            });

            $('body').on('click', '.btnRemove', function () {
                $(this).parent().parent().remove();
            });

            $('body').on('click', '.btnDelete', function () {
                var ReqId = parseInt($(this).val());
                swal({
                    title: "CONFIRM:",
                    text: "Are you sure you want to delete this request?",
                    type: "warning",
                    inputType: "submit",
                    showCancelButton: true,
                    closeOnConfirm: true
                },
                    function (isConfirm) {
                        if (isConfirm == true) {

                            $.ajax({
                                type: "POST",
                                url: 'StockBranch_IMS.aspx/DeleteStockRequest',
                                contentType: "application/json; charset=utf-8",
                                data: JSON.stringify({ RequestId: ReqId }),
                                success: (rs) => {
                                    var StartDate = document.getElementById('StartDate').value;
                                    var EndDate = document.getElementById('EndDate').value;

                                    document.getElementById("FilterTable").style.display = 'block';
                                    GetFilteredStockZone(StartDate, EndDate);
                                }
                            });
                        }
                        else {
                            swal("Cancelled", "Cancelled", "error");
                        }
                    })
            });

            $('#btnSave').click(function () {
                var rowLen = $('#details').find('tr').length;
                if (rowLen <= 0) {
                    alert('Please Add Details');
                    return;
                }

                var data = {
                    Year: 2019,
                    Zone: 'Zone',
                    StockRequestDetails: []
                };

                var rows = $('#details').find('tr');
                var productArr = [];
                for (let x of rows) {
                    var row = $(x);
                    var DepartmentId = parseInt(row.find('td:eq(0)').find('select').val());
                    debugger
                    var typeId = parseInt(row.find('td:eq(2)').find('select').val());
                    var productId = parseInt(row.find('td:eq(3)').find('select').val());
                    productArr.push(productId);
                    var qty = parseInt(row.find('td:eq(4)').find('input[type=text]').val());
                    if (qty <= 0 || isNaN(qty)) {
                        alert('Please provide valid quantity');
                        return;
                    }
                    data.StockRequestDetails.push({
                        DepartmentId: DepartmentId,
                        TypeId: typeId,
                        ProductId: productId,
                        Qty: qty
                    });
                }
                var statusSameProduct = true;
                for (var k = 0; k <= productArr.length; k++) {
                    for (var j = k + 1; j < productArr.length; j++) {
                        if (productArr[k] == productArr[j]) {
                            statusSameProduct = false;
                        }
                    }
                }
                if (!statusSameProduct)
                {
                    alert('Cannot send request. No same products should be requested');
                    return;
                }
                document.getElementById('btnSave').disabled = true;
                document.getElementById('LoaderDiv').style.display = '';
                save(data);
            });

            $('body').on('click', '.btnView', function () {
                GetTypeProducts();


                var id = $(this).val();
                var did = parseInt($(this).val());
                var isupdated = $(this).data('isupdated');
                var id = parseInt($(this).data('id'));
                var zone = $(this).data('zone');

                getRecordDetails(id);

                var x = document.getElementById("EditTable");
                x.style.display = "block";
                //jQuery.noConflict();
                $('#ModalPopUp').modal('show');

            });

            $('body').on('click', '.btnDetailView', function () {
                GetTypeProducts();


                var id = $(this).val();
                getPendingReqDetails(id);

                var x = document.getElementById("EditTableDetails");
                x.style.display = "block";
                //jQuery.noConflict();
                $('#ModalDetails').modal('show');

            });
            $('body').on('click', '#btnModify', function () {

                var rowLen = $('#reqPerStock').find('tr').length;
                var Id = parseInt($(this).val());

                var data = {
                    Id: Id,
                    Year: 2019,
                    Zone: 'Zone',
                    StockRequestDetails: []
                };

                var rows = $('#reqPerStock').find('tr');
                var productArr = [];
                for (let x of rows) {
                    var row = $(x);
                    var DetailId = parseInt(row.find('td:eq(0)').find('label').attr("value"));
                    var typeId = parseInt(row.find('td:eq(2)').find('select').val());
                    var productId = parseInt(row.find('td:eq(3)').find('select').val());
                    productArr.push(productId);
                    var qty = parseInt(row.find('td:eq(4)').find('input[type=text]').val());
                    if (qty <= 0 || isNaN(qty)) {
                        alert('Please provide valid quantity');
                        return;
                    }
                    data.StockRequestDetails.push({
                        DetailId: DetailId,
                        TypeId: typeId,
                        ProductId: productId,
                        Qty: qty
                    });
                }
                var statusSameProduct = true;
                for (var k = 0; k <= productArr.length; k++) {
                    for (var j = k + 1; j < productArr.length; j++) {
                        if (productArr[k] == productArr[j]) {
                            statusSameProduct = false;
                        }
                    }
                }
                if (!statusSameProduct) {
                    alert('Cannot send request. No same products should be requested');
                    return;
                }

                //jQuery.noConflict();
                $('#ModalPopUp').modal('toggle');
                ModifyRequest(data);
                var StartDate = document.getElementById('StartDate').value;
                var EndDate = document.getElementById('EndDate').value;

                document.getElementById("FilterTable").style.display = 'block';
                GetFilteredStockZone(StartDate, EndDate);
            });


            $('body').on('change', '.typeId', function () {
                debugger
                var id = $(this).val();
                var kk = foo(id);
                $(this).parent().parent().find('td:eq(3)').find('.productChangeId').html(kk);
            });

            $('body').on('focusout', '.txtoracle', function () {
                debugger
                var OracleCode = $(this).val();

                $.ajax({
                    type: 'post',
                    url: 'StockInventory_IMS.aspx/GetStockItems',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({ OracleCode: OracleCode }),
                    success: (rs) => {
                        var data = rs.d;
                        var rows = '';
                        if (data.length == 0) {
                            alert('No record found!');
                            return;
                        }
                        for (let x of data) {
                            var Type = '';
                            for (let x of Types) {
                                Type += `<option value="${x.Value}">${x.Text}</option>`;
                            }

                            $(this).parent().parent().find('td:eq(1)').find('.txtoracle').val(x.OracleCode);
                            $(this).parent().parent().find('td:eq(2)').find('.typeId').val(x.TypeId).change();
                            $(this).parent().parent().find('td:eq(3)').find('.productChangeId').val(x.ProductId).change();
                            $(this).parent().parent().find('td:eq(5)').find('.txtunit').val(x.Unit);
                        }
                    }
                });
            });

            $('body').on('change', '#dropdownTypeModal', function () {
                var id = $(this).val();
                var kk = foo(id);
                $(this).parent().parent().find('td:eq(2)').find('#dropdownProductModal').html(kk);
            });
        });



        var TypeProducts = [];
        var Departments = [];

        var GetTypeProducts = () => {

            $.ajax({
                type: "POST",
                url: 'StockRequestZone_IMS.aspx/GetTypeProducts',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: (rs) => {
                    TypeProducts = rs.d;
                }
            });
        }
        var GetDepartments = () => {

            $.ajax({
                type: "POST",
                url: 'StockHODepartment_IMS.aspx/GetDepartments',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: (rs) => {
                    Departments = rs.d;
                }
            });
        };
        var GetFilteredStockZone = (StartDate, EndDate) => {
            $.ajax({
                type: 'post',
                url: 'StockHODepartment_IMS.aspx/GetFilteredStockZone',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ StartDate: StartDate, EndDate: EndDate }),
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
                        rows += `<tr>
                                    <td>${x.DepartmentName}</td>
                                    <td>${x.CreatedOn}</td>
                                    <td>${x.ReqId}</td>
                                    <td>${x.Qty_}</td>                                                              
                                    <td>
                                        <button type="button"  CssClass="button" class="button btn btn-sm btn-danger btnDelete" value="${x.ReqId}" data-id="${x.ReqId}">Delete</button>
                                        <button type="button"  CssClass="button" class="button btn btn-sm btn-outline-dark btnView" value="${x.ReqId}" data-id="${x.ReqId}">Edit</button>
                                    </td>
                                </tr>`;
                    }

                    $('#FilteredInventory').html(rows);
                }
            });
        };

        var productsModal = [];
        var getProductsModal = (Id) => {
            $.ajax({
                type: "POST",
                url: 'StockRequestZone_IMS.aspx/GetProductsSingle',
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({ Id: Id }),
                dataType: "json",
                success: (rs) => {
                    productsModal = rs.d;
                    var selectt = '';
                    for (let x of productsModal) {
                        selectt += `<option value="${x.Value}">${x.Text}</option>`;
                    }
                    var selectTD = `<select class="form-control" id="dropdownProductModal">${selectt} </select>`

                    document.getElementById('tdProductModal').innerHTML = selectTD;

                }
            });
        };
        var selectt = '';
        var getProductsSingle = (Id) => {
            $.ajax({
                type: "POST",
                url: 'StockRequestZone_IMS.aspx/GetProductsSingle',
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({ Id: Id }),
                dataType: "json",
                success: (rs) => {

                    var productsSingleTpe = rs.d;
                    selectt = '';
                    for (let x of productsSingleTpe) {
                        selectt += `<option value="${x.Value}">${x.Text}</option>`;
                    }
                }
            });
        };

        var getRecordDetails = (Id) => {
            $.ajax({
                type: 'post',
                url: 'StockBranch_IMS.aspx/GetFilteredStockZonePer',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ Id: Id }),
                success: (rs) => {
                    var data = rs.d;
                    var rows = '';
                    var prods = '';

                    for (let z of data) {
                        var types = '';
                        var typeId = z.TypeId;
                        var productId = z.ProductId;

                        var kkk = foo(typeId);
                        for (let x of TypeProducts) {
                            if (x.Value == typeId) {
                                types += `<option selected="selected" value="${x.Value}">${x.Text}</option>`;
                            } else {
                                types += `<option value="${x.Value}">${x.Text}</option>`;
                            }
                        }
                        var productssList = '';
                        $.ajax({
                            type: "POST",
                            url: 'StockBranch_IMS.aspx/GetProductsSingle',
                            contentType: "application/json; charset=utf-8",
                            data: JSON.stringify({ Id: typeId }),
                            dataType: "json",
                            async: false,
                            success: (rs) => {

                                var productsSingleTpe = rs.d;
                                productssList = '';
                                for (let x of productsSingleTpe) {
                                    if (productId == x.Value) {
                                        productssList += `<option selected="selected" value="${x.Value}">${x.Text}</option>`;
                                    } else {
                                        productssList += `<option value="${x.Value}">${x.Text}</option>`;
                                    }
                                }
                                return productssList;
                            }
                        });

                        rows += `<tr>
                                    <td id="DetailId" style = "display:none" ><label id="DetailLabel" value="${z.DetailId}">${z.DetailId}</label> </td>
                                    <td id="OracleCode"><input type="text" disabled="disabled" value="${z.OracleCode}" class="form-control" name="OracleCode" autopostback="true" style="font-size:12px;"  /></td>
                                    <td id="dropType">
                                        <select class="form-control" disabled="disabled" id="dropdownTypeModal"  style="font-size:12px;">
                                         ${types}
                                        </select>
                                    </td>
                                    <td id="tdProductModal">
                                        <select class="form-control" disabled="disabled"  id="dropdownProductModal" style="font-size:12px;">
                                         ${productssList}
                                        </select>
                                    </td>                                 
                                    <td id="Qtyy" ><input type="text" value="${z.Qty}" class="form-control" name="Qty" maxlength="6" autopostback="true"  onkeypress="return isNumberKey(event)"  style="font-size:12px;"  /></td>                                                               
                                      </tr>`;
                        $('#reqPerStock').html(rows);
                        $('#btnDiv').html(`<button id="btnModify" CssClass="submitBtn button"  type="button" class="button btn btn-lg btn-primary btn-outline-dark  " data-id="${z.ReqId}" value="${z.ReqId}" >Submit</button>`);

                    }
                }
            });
        };
        var ModifyRequest = (data) => {
            $.ajax({
                type: 'post',
                url: 'StockHODepartment_IMS.aspx/ModifyRequest',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ model: data }),
                success: (rs) => {
                    alert(rs.d);
                }
            });
        };

        var Types = [];
        var products = [];
        var Departments = [];

        var getProducts = () => {
            $.ajax({
                type: "POST",
                url: 'StockRequestZone_IMS.aspx/GetProducts',
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
                url: 'StockRequestZone_IMS.aspx/GetTypeProducts',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: (rs) => {
                    Types = rs.d;
                }
            });
        };


        var save = (data) => {
            $.ajax({
                type: "POST",
                url: 'StockHODepartment_IMS.aspx/SaveStockDetails',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ model: data }),
                success: (rs) => {
                    alert(rs.d);
                    document.getElementById('form1').reset();
                    $('#details').html('');

                    document.getElementById('btnSave').disabled = false;
                    document.getElementById('LoaderDiv').style.display = 'none';
                },
                error: (error) => {
                    alert('Error: ' + error.statusText);
                    document.getElementById('btnSave').disabled = false;
                    document.getElementById('LoaderDiv').style.display = 'none';
                }
            });
        };


        var GetBranchStock = () => {

            $.ajax({
                type: 'post',
                url: 'StockBranch_IMS.aspx/GetBranchStock',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: (rs) => {
                    var data = rs.d;
                    var rows = '';

                    for (let x of data) {
                        if (x.Qty > 0) {
                            rows += `<tr>
                              
                                    <td>${x.OracleCode}</td>
                                    <td>${x.TypeName}</td>
                                    <td>${x.ProductName}</td>
                                    <td>${x.Qty_}</td>
                                    
                                </tr>`;
                        }
                    }
                    $('#branchTable').html(rows);
                }
            });
        };

        var GetHOStock = () => {

            $.ajax({
                type: 'post',
                url: 'StockRequestZone_IMS.aspx/GetHOStock',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: (rs) => {
                    var data = rs.d;
                    var rows = '';
                    //  console.log(data);
                    for (let x of data) {
                        if (x.Qty > 0) {
                            rows += `<tr>                         
                                    <td>${x.OracleCode}</td>
                                    <td>${x.TypeName}</td>
                                    <td>${x.ProductName}</td>
                                    <td>${x.Qty_}</td>
                                    
                                </tr>`;
                        }
                    }
                    $('#HOTable').html(rows);
                }
            });
        };

        var getPendingReqDetails = (Id) => {
            $.ajax({
                type: 'post',
                url: 'StockBranch_IMS.aspx/GetPendingRequestDetails',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ Id: Id }),
                success: (rs) => {
                    var data = rs.d;
                    var rows = '';

                    for (let x of data) {
                        rows += `<tr>
                                    <td>${x.TypeName}</td>
                                    <td>${x.ProductName}</td>
                                    <td>${x.Qty}</td>                                    
                                </tr>`;

                    }
                    $('#reqPendingStockDetails').html(rows);
                }
            });
        };

        var GetIssuedRequest = () => {
            $.ajax({
                type: 'post',
                url: 'StockRequestZone_IMS.aspx/GetIssuedRequest',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: (rs) => {
                    var data = rs.d;
                    var rows = '';

                    for (let x of data) {
                        rows += `<tr>
                                    <td>${x.TypeName}</td>
                                    <td>${x.ProductName}</td>
                                    <td>${x.Qty}</td> 
                                </tr>`;
                    }
                    $('#issuedTable').html(rows);
                }
            });
        };

        function ModalTypeChange() {
            var productsModal = [];
            var Id = document.getElementById('dropdownTypeModal').value;
            var prods = '';

            getProductsModal(Id);
        }

        var selectt = '';
        function foo(Id) {
            $.ajax({
                type: "POST",
                url: 'StockInventory_IMS.aspx/GetProductsSingle',
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
        var addRow = () => {

            var prods = '';
            var types = '';
            var department = '';

            for (let y of Types) {
                types += `<option value="${y.Value}">${y.Text}</option>`;
            }

            //  console.log(Departments);
            for (let z of Departments) {
                department += `<option value="${z.Value}">${z.Text}</option>`;
            }

            var gg = foo(1);

            var html = `<tr>
                            <td style="font-size:12px;">
                                <select class="form-control DepartmentId" style="font-size:12px;">
                                ${department}
                                </select>
                            </td>
                           <td>
                            <input class="form-control txtoracle" autopostback="true" type="text" id="oraclecode" style="font-size:12px;"/>
                            </td>
                            <td style="font-size:12px;">
                                <select class="form-control typeId" disabled="disabled" style="font-size:12px;">
                                ${types}
                                </select>
                            </td>
                            <td style="font-size:12px;">
                                <select class="form-control productChangeId"  disabled="disabled"  style="font-size:12px;">
                                ${gg}
                                </select>
                            </td>
                            <td style="font-size:12px;"><input maxlength="6" autopostback="true"  onkeypress="return isNumberKey(event)" type="text" class="form-control" style="font-size:12px;" /></td>
<td><input class="form-control txtunit" disabled="disabled" type="text" id="unit" style="width:3em; font-size:12px;"/></td>

<td><button type="button" class="btn btn-sm btn-danger btnRemove" style="font-size:12px;">Remove</button></td>
                        </tr>`;
            $('#details').append(html);
        };
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
</asp:Content>

