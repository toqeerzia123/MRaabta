<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="StockIssuance.aspx.cs" Inherits="MRaabta.Files.StockIssuance" %>

<asp:Content ContentPlaceHolderID="head" ID="ContentHead" runat="Server">

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
        .MainTbl {
            border-collapse: collapse;
            width: 100%;
        }

        /*{
            text-align: left;
            padding: 8px;
        }*/

        tr {
            text-align: left;
            padding: 8px;
        }

        /*}*/

        /*tr:nth-child(even){background-color: #f2f2f2}*/

        .MainTbl th {
            background-color: #404040;
            color: white;
        }

        .submitBtn {
            float: right;
            font-size: 250px;
        }

        #btnSubmit {
            width: 120px;
            height: 35px;
        }

        #btndiv {
            font-size: 90px;
        }
    </style>

    <div class="row">
        <div class="col-md-6">
            <table cellpadding="0" cellspacing="0" width='100%' class='mGrid_Table '>
                <tr>
                    <td colspan="12" align="center" class="head_column">
                        <h3 style="text-align: center">Stocks Issuance To Zone
                        </h3>
                    </td>

                </tr>
            </table>
            <div class="shadow rounded">
                <div class="">
                    <table>
                        <tr>

                            <div class="row ml-2 mt-2">
                                <div class="form-group row">
                                    <label for="inputEmail3" class="col-sm-2 col-form-label mt-1" style="font-size: 12px;">Zone</label>
                                    <div class="col-sm-5">
                                        <select class="ddlZonesllll  form-control" id="inputState" style="font-size: 12px;">
                                        </select>
                                    </div>
                                </div>


                            </div>

                        </tr>
                    </table>

                    <table class="MainTbl table table-striped">
                        <thead class="">
                            <tr style="font-size: 12px;">
                                <th style="width: 20%; text-align: center;">Zone</th>
                                <th style="width: 30%; text-align: center;">Request Number</th>
                                <th style="width: 40%; text-align: center;">Total Requested Quantity</th>
                                <th></th>
                            </tr>
                        </thead>
                        <tbody id="reqGrid" style="font-size: 12px;">
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
        <div class="col-md-6">
            <div id="ThirdTable">

                <table cellpadding="0" cellspacing="0" width='100%' class='mGrid_Table'>
                    <tr>
                        <td>
                            <nav>
                                <div class="nav nav-tabs nav-fill " id="nav-tab" role="tablist">
                                    <a class="nav-item nav-link active" id="nav-home-tab" data-toggle="tab" href="#nav-home" role="tab" aria-controls="nav-home" aria-selected="true">Head Office Stock</a>
                                </div>
                        </nav>
                        </td>
                    </tr>
                </table>

                <div class="tab-content" id="nav-tabContent">
                    <div class="tab-pane fade show active" id="nav-home" role="tabpanel" aria-labelledby="nav-home-tab">

                        <div id="mystock" class="col-md-12 mt-1  d-none">
                            <table class="MainTbl table table-striped">
                                <thead>
                                    <tr style="font-size: 12px;">
                                        <th>Zone</th>
                                        <th>Product Type</th>
                                        <th>Product Name</th>
                                        <th>Quantity</th>
                                    </tr>
                                </thead>
                                <tbody id="myStockGrid" style="font-size: 12px;">
                                </tbody>
                            </table>
                        </div>

                    </div>
                </div>
            </div>
        </div>

    </div>

    <div class="modal fade" id="ModalPopUp" role="dialog">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 style="text-align: center;">Request Number: </h4>
                    <h4 style="text-align: center" id="reqId"></h4>
                    <%--<h4 style="text-align: right">Details</h4>--%>
                    <%--<input type="text" name="reqId" id="reqId" value=""/>--%>
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                </div>
                <div class="modal-body">
                    <div id="detailRow" class="row d-none">
                        <div class="col-12">
                            <table class="MainTbl table table-striped">
                                <thead>
                                    <tr style="font-size: 12px;">
                                        <th>
                                            <input class="form-check-input" type="checkbox" id="chk_selectall" style="position: static; margin-top: 0; margin-left: 0;"></th>
                                            <th>Type</th>
                                            <th>Product</th>
                                            <th>Qty</th>
                                            <th>Given Qty</th>
                                            <th>Barcode From</th>
                                            <th>Barcode To</th>
                                    </tr>
                                </thead>
                                <tbody id="reqDetailGrid" style="font-size: 12px;">
                                </tbody>
                            </table>
                           <div id="LoaderDiv" style="display:none"><img id="LoaderImg" src="../assets/images/loader-1.gif" alt="" /></div>

                            <p id="error-msg" style="color: red; font-size: 12px"></p>
                        </div>
                        <div class="text-right" style="margin: 0 auto" id="btnDiv"></div>
                    </div>
                </div>
                <div class="modal-footer">
                    <div id="zonestock" class="col-md-12 d-none">
                        <h3 style="text-align: center" id="zone"></h3>
                        <div class="col-md-12">
                            <table class="MainTbl table table-striped" id="zonestock">
                                <thead>
                                    <tr style="font-size: 12px;">

                                        <th>Product Type</th>
                                        <th>Product</th>
                                        <th>Quantity</th>

                                    </tr>
                                </thead>
                                <tbody id="zoneStockGrid" style="font-size: 12px;">
                                </tbody>
                            </table>
                        </div>

                    </div>
                </div>
            </div>
        </div>
    </div>


    <script>

        // $('body').on('click', '.btn_find', function () {

        //     //var id = $("#ContentPlaceHolder1_ddlZones").val();
        //    // var id = $(".ddlZonesllll").val();
        //        //getRequests(2);
        //        //

        //});
        var getZones = () => {

            $.ajax({
                type: "POST",
                url: 'StockIssuance.aspx/GetZones',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: (rs) => {
                    var kk = '';
                    kk += `<option value="0">All Zones</option>`;
                    var rr = rs.d;
                    for (let y of rr) {
                        kk += `<option value="${y.Value}">${y.Text}</option>`;
                    }
                    $('.ddlZonesllll').html(kk);
                }
            });
        };

        var getRequests = (id) => {


            $.ajax({
                type: 'post',
                url: 'StockIssuance.aspx/GetRequests',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                //processData: false,
                data: JSON.stringify({ id: id }),
                success: (rs) => {

                    var data = rs.d;
                    var rows = '';
                    for (let x of data) {

                        rows += `<tr style="text-align:center;">
                                    <td>${x.ZoneName}</td>
                                    <td>${x.Id}</td>
                                    <td >${x.TotalReqQty_}</td>
                                    <td>
                                        <button type="button"  CssClass="button" class="button btn btn-sm btn-outline-dark btnView" data-name="${x.ZoneName}" data-zone="${x.ZoneCode}" value="${x.Id}" data-isupdated="${x.IsUpdated}" >View</button>
                                    </td>
                                </tr>`;
                    }
                    //__doPostBack('reqGrid', '');
                    $('#reqGrid').html(rows);


                }
            });
        };

        var getMyStock = () => {

            $.ajax({
                type: 'post',
                url: 'StockIssuance.aspx/GetMyStock',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: (rs) => {
                    var data = rs.d;
                    var rows = '';

                    for (let x of data) {
                        if (x.Qty>0) {
                            rows += `<tr>
                                    <td>${x.Zone}</td>
                                    <td>${x.TypeName}</td>
                                    <td>${x.ProductName}</td>
                                    <td>${x.Qty_}</td>
                                </tr>`;
                        }

                    }
                    $('#myStockGrid').html(rows);
                    $('#mystock').removeClass('d-none');
                }
            });
        };

        var getZoneStock = (zone) => {

            $.ajax({
                type: 'post',
                url: 'StockIssuance.aspx/GetZoneStock',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ zone: zone }),
                success: (rs) => {

                    var data = rs.d;
                    var rows = '';
                    for (let x of data) {
                        if (x.Qty > 0) {
                            rows += `<tr>
                                    <td>${x.TypeName}</td>
                                    <td>${x.ProductName}</td>
                                    <td>${x.Qty}</td>
                                  
                                </tr>`;
                        }


                    }

                    $('#zoneStockGrid').html(rows);
                    $('#zonestock').removeClass('d-none');
                }
            });
        };

        var getRequestDetails = (id, isupdated, zone, name) => {

            $.ajax({
                type: 'post',
                url: 'StockIssuance.aspx/GetRequestsDetails',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ id: id }),
                success: (rs) => {
                    var data = rs.d;
                    var rows = '';

                    //$(".modal-header #reqId").val(id);
                    $('#reqId').text(id);
                    name += " Stock"
                    $('#zone').text(name);
                    for (let x of data) {

                        rows += `<tr>
                                    <td>${(isupdated == false && x.BarcodeIssueFrom == null) ? `<input class="form-check-input" type="checkbox" id="gridCheck1" style = "position: static; margin-top: 0; margin-left: 0;" disabled>` : `<input class="form-check-input" type="checkbox" id="gridCheck1" style = "position: static; margin-top: 0; margin-left: 0;">`}</td>
                                    <td>${x.TypeName}</td>
                                    <td><input type="hidden" value="${x.ProductId}">${x.ProductName}</td>
                                    <td><input type="hidden" value="${x.Id}">${x.Qty}</td>
                                    <td style="width:8em;"><input type="hidden" value="${x.QtyIssueFrom}">${(isupdated == false) ? (x.BarcodeIssueFrom == null) ? `<input style="height:2em; font-size:12px;" autopostback="true"  onkeypress="return isNumberKey(event)" type="text" maxlength="6" class="form-control txtissueqty" disabled>` : `<input style="height:2em; font-size:12px;" autopostback="true"  onkeypress="return isNumberKey(event)" type="text" maxlength="6" class="form-control txtissueqty" >` : `${x.IssueQty}`}</td>
                                    <td>${(isupdated == false && x.BarcodeIssueFrom == null) ? `<input type="text" style="height:2em; font-size:12px;" class="form-control txtbarcodefrom " readonly="readonly" disabled>` : `<input type="text" style="height:2em; font-size:12px;" class="form-control txtbarcodefrom" readonly="readonly" value="${x.BarcodeIssueFrom}">`}</td>
                                    <td><input type="text" style="height:2em; font-size:12px;" class="form-control txtbarcodeto" readonly="readonly" value="${x.BarcodeTo}"></td>
                                </tr>`;
                    }

                    $('#reqDetailGrid').html(rows);

                    if (isupdated == false) {
                        $('#btnDiv').html(`<button id="btnSubmit" type="button" class="button btn btn-outline-dark" style="float:right;" data-id="${id}" data-zone="${zone}">Submit</button>`);
                    } else {
                        $('#btnDiv').html('');
                    }

                    $('#detailRow').removeClass('d-none');
                }
            });
        };


        var save = (data) => {

            $.ajax({
                type: 'post',
                url: 'StockIssuance.aspx/SaveIssuanceDetails',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ model: data }),
                success: (rs) => {

                    alert(rs.d);
                    $('#detailRow').addClass('d-none');
                    getRequests(-1);
                    $('#ModalPopUp').modal('hide');
                    $('#error-msg').html("");

                    document.getElementById('btnSubmit').disabled = false;
                    document.getElementById('LoaderDiv').style.display = 'none';

                },
                error: (error) => {
                    alert('Error:' + error.statusText);

                    document.getElementById('btnSubmit').disabled = false;
                    document.getElementById('LoaderDiv').style.display = 'none';
                }
            });
        };

        $(function () {

            getZones();
            getMyStock();
            getRequests(-1);
            // $('body').on('click', '.btn_find', function () {
            //
            //var id = $("#ContentPlaceHolder1_ddlZones").val();
            //getRequests(id);
            //

            // });
            $('#chk_selectall').click(function (e) {
                var table = $(e.target).closest('table');
                $('td input:checkbox', table).not(":disabled").prop('checked', this.checked);
            });

            $('body').on('change', '.ddlZonesllll', function () {

                var id = $(this).val();
                //var kk = foo(id);
                getRequests(id);

                $(this).parent().parent().find('td:eq(1)').find('.productChangeId').html(kk);
            });

            $('body').on('click', '.btnView', function () {
                
                var id = parseInt($(this).val());
                var isupdated = $(this).data('isupdated');
                var zone = $(this).data('zone');
                var name = $(this).data('name');
                getRequestDetails(id, isupdated, zone, name);
                getZoneStock(zone);
                document.getElementById("detailRow").focus();
                //jQuery.noConflict();
                $('#ModalPopUp').modal('show');
                $('#error-msg').html("");

            });

            $('body').on('blur', '.txtissueqty', function () {

                var qty = parseInt($(this).parent().parent().find('td:eq(3)').text());
                var issueqty = parseInt($(this).val());
                var qtyIssueFrom = parseInt($(this).parent().parent().find('td:eq(4)').find('input[type=hidden]').val());
                
                if (issueqty > qty) {
                    $(this).addClass('is-invalid');
                    $(this).focus();
                    return;
                }
                if (qtyIssueFrom == 0) {
                    //var product = parseInt($(this).parent().parent().find('td:eq(3)').find('input[type=hidden]').val());
                    $(this).addClass('is-invalid');
                    //$(this).focus();
                    $('#error-msg').html("Out of Stock");
                    return;
                }
                if (issueqty > qtyIssueFrom) {
                    $(this).addClass('is-invalid');
                    $(this).focus();
                    $('#error-msg').html("You Only Have " + qtyIssueFrom + " barcodes in this series.");
                    return;
                }
                if (issueqty <= 0) {
                    $(this).addClass('is-invalid');
                    $(this).focus();
                    return;
                }
                else {
                    $('#error-msg').html("");
                    var barcodeto = 0;
                    var barcodefrom = parseInt($(this).parent().parent().find('td:eq(5)').find('input[type=text]')[0].value);
                    if (barcodefrom > 0) {

                        //check ocs ana 
                        barcodeto = barcodefrom + issueqty - 1;
                        var len = barcodefrom.toString().length;
                        $(this).parent().parent().find('td:eq(6)').find('input[type=text]').val(barcodeto.toString().padStart(len, '0'));
                        $(this).removeClass('is-invalid');
                    }

                    var ProductToCheck = parseInt($(this).parent().parent().find('td:eq(2)').find('input').val());
                   
                    if (ProductToCheck == 17) {
                        barcodeto = barcodefrom.toString().slice(0, -1);
                        barcodeto = (parseInt(barcodeto) + issueqty - 1);
                        var OCSANA = Math.floor(barcodeto / 7);
                        OCSANA = barcodeto - OCSANA * 7;
                        OCSANA = barcodeto.toString().concat(OCSANA);
                        barcodeto = OCSANA;
                      //  barcodeto = barcodeto + issueqty - 1;
                        var len = barcodeto.toString().length;
                        $(this).parent().parent().find('td:eq(6)').find('input[type=text]').val(barcodeto.toString().padStart(len, '0'));
                    }
                    else {
                        barcodeto = (barcodefrom + qty) - 1;
                    }


                }
            });

            //$('body').on('blur', '.txtbarcodefrom', function () {
            //    
            //    var issueqty = parseInt($(this).parent().parent().find('td:eq(3)').find('input[type=number]').val());
            //    var barcodefrom = parseInt($(this).find('.Barcode From').text()); ;
            //    var barcodeto = barcodefrom + issueqty;
            //    $(this).parent().parent().find('td:eq(5)').find('input[type=number]').val(barcodeto.toString().padStart(12, '0'));
            //});


            $('body').on('click', '#btnSubmit', function () {

                var id = parseInt($(this).data('id'));
                var zone = $(this).data('zone');

                var data = {
                    StockRequestId: id,
                    ZoneCode: zone,
                    StockIssuanceDetails: []
                };
                //rabeea
                var rows = $('#reqDetailGrid').find('tr');

                for (let x of rows) {
                    var row = $(x);
                    var requestDetailid = parseInt(row.find('td:eq(3)').find('input[type=hidden]').val());
                    var requestedQty = parseInt(row.find('td:eq(3)')[0].innerText);

                    var qty = parseInt(row.find('td:eq(4)').find('input[type=text]').val());
                    var barcodefrom = parseInt(row.find('td:eq(5)').find('input[type=text]').val());
                    var barcodeto = parseInt(row.find('td:eq(6)').find('input[type=text]').val());
                    var product = parseInt(row.find('td:eq(2)').find('input[type=hidden]').val());
                    var qtyIssueFrom = parseInt(row.find('td:eq(4)').find('input[type=hidden]').val());
                    if (row.find('td:eq(0)').find('input[type=checkbox]').is(":checked") == true) {
                        if (qty > requestedQty) {
                            row.find('td:eq(4)').find('input[type=text]').addClass('is-invalid');
                            row.find('td:eq(4)').find('input[type=text]').focus();
                            return;
                        } else {
                            row.find('td:eq(4)').find('input[type=text]').removeClass('is-invalid');
                        }

                        if (isNaN(qty) || qty <= 0) {
                            row.find('td:eq(4)').find('input[type=text]').addClass('is-invalid');
                            row.find('td:eq(4)').find('input[type=text]').focus();
                            return;
                        } else {
                            row.find('td:eq(4)').find('input[type=text]').removeClass('is-invalid');
                        }

                        if (isNaN(barcodefrom) || barcodefrom <= 0) {
                            row.find('td:eq(5)').find('input[type=text]').addClass('is-invalid');
                            row.find('td:eq(5)').find('input[type=text]').focus();
                            return;
                        } else {
                            row.find('td:eq(5)').find('input[type=text]').removeClass('is-invalid');
                        }

                        if (qty > qtyIssueFrom) {
                            row.find('td:eq(4)').find('input[type=text]').addClass('is-invalid');
                            row.find('td:eq(4)').find('input[type=text]').focus();
                            return;
                        } else {
                            row.find('td:eq(4)').find('input[type=text]').removeClass('is-invalid');
                        }

                        data.StockIssuanceDetails.push({
                            StockRequestDetailId: requestDetailid,
                            IssueQty: qty,
                            BarcodeFrom: barcodefrom,
                            BarcodeTo: barcodeto,
                            ProductId: product,
                            QtyIssueFrom: qtyIssueFrom
                        });
                    }

                }
                if (data.StockIssuanceDetails.length == 0) {
                    alert("Issue atleast one product");
                }
                else {
                    document.getElementById('btnSubmit').disabled = true;
                    document.getElementById('LoaderDiv').style.display = '';

                    save(data);
                }


            });

        });

    </script>
</asp:Content>
