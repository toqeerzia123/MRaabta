<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="StockIssuanceBranchEC_IMS.aspx.cs" Inherits="MRaabta.Files.StockIssuanceBranchEC_IMS" %>

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

            .MainTbl tr {
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
        <div class="col-md-7">
            <table cellpadding="0" cellspacing="0" width='100%' class='mGrid_Table '>
                <tr>
                    <td colspan="12" align="center" class="head_column">
                        <h3 style="text-align: center">Stocks Issuance To Express Center
                        </h3>
                    </td>
                </tr>
            </table>
            <div class="shadow rounded">
                <div class="">
                    <div class="row ml-2 mt-2">
                        <div class="col-md-5">
                            <div class="form-group">
                                <label for="formGroupExampleInput" style="font-size: 12px;">Start Date</label>
                                <input type="date" name="StartDate" id="StartDate" class="form-control" style="font-size: 12px;" />
                            </div>
                        </div>
                        <div class="col-md-5">
                            <div class="form-group">
                                <label for="formGroupExampleInput" style="font-size: 12px;">End Date</label>
                                <input type="date" name="EndDate" id="EndDate" class="form-control" style="font-size: 12px;" />
                            </div>
                        </div>
                    </div>

                    <div class="row ml-2 mt-2 mr-2">
                        <div class="form-group col-md-5">
                            <label for="inputEmail3" class="col-sm-5 col-form-label " style="font-size: 12px;">Express Center</label>
                            <select class="ddlZonesllll  form-control" id="inputState" style="font-size: 12px;">
                            </select>
                        </div>
                        <div class="col-md-5" style="margin-top: 2.1em;">
                            <div class="form-group">
                                <button id="btnFilter" type="button" class="button btn btn-lg btn-primary btn-outline-dark" style="font-size: 12px; height: 2.5em; width: 5.7em;">Search</button>
                            </div>
                        </div>
                    </div>

                    <table class="MainTbl table table-striped">
                        <thead class="">
                            <tr style="font-size: 12px;">
                                <th style="width: 30%; text-align: center;">Express Center</th>
                                <th style="width: 15%; text-align: center;">Request Number</th>
                                <th style="width: 15%; text-align: center;">Requested Date</th>
                                <th style="width: 15%; text-align: center;">Total Requested Quantity</th>
                                <th style="width: 35%; text-align: center;">Action</th>
                            </tr>
                        </thead>
                        <tbody id="reqGrid" style="font-size: 12px;">
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
        <div class="col-md-5">
            <div id="ThirdTable">

                <table cellpadding="0" cellspacing="0" width='100%' class='mGrid_Table'>
                    <tr>
                        <td>
                            <nav>
                                <div class="nav nav-tabs nav-fill " id="nav-tab" role="tablist">
                                    <a class="nav-item nav-link active" id="nav-home-tab" data-toggle="tab" href="#nav-home" role="tab" aria-controls="nav-home" aria-selected="true">Branch Stock</a>
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
                                        <th>Oracle Code</th>
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
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                </div>
                <div class="modal-body">
                    <div id="detailRow" class="row d-none">
                        <div class="col-12">
                            <table class="MainTbl table table-striped">
                                <thead>
                                    <tr style="font-size: 12px;">
                                        <th>
                                            <input class="form-check-input" type="checkbox" id="chk_selectall" style="position: static; margin-top: 0; margin-left: 0;" />
                                        </th>
                                        <th>Oracle Code</th>
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
                            <div id="LoaderDiv" style="display: none">
                                <img id="LoaderImg" src="../assets/images/loader-1.gif" alt="" />
                            </div>
                            <p id="error-msg" style="color: red; font-size: 12px"></p>
                        </div>
                        <div class="text-right" style="margin: 0 auto" id="btnDiv"></div>
                    </div>
                </div>
                <div class="modal-footer">
                    <div id="zonestock" class="col-md-12 d-none">
                        <h3 style="text-align: center" id="zone"></h3>
                        <div class="col-md-12">
                            <table class="MainTbl table table-striped" id="">
                                <thead>
                                    <tr style="font-size: 12px;">
                                        <th>Oracle Code</th>
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
    <link href="../mazen/css/sweetalert.css" rel="stylesheet" /> 
    <script type="text/javascript" src="../mazen/js/sweetalert-dev.js"></script>
    <script type="text/javascript" src="../assets/bootstrap-4.3.1-dist/js/bootstrap.min.js"></script>
    <script type="text/javascript">
        var getZones = () => {

            $.ajax({
                type: "POST",
                url: 'StockIssuanceBranchEC_IMS.aspx/GetExpressCenter',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: (rs) => {
                    var kk = '';
                    kk += `<option value="0">All Express Centers</option>`;
                    var rr = rs.d;
                    for (let y of rr) {
                        kk += `<option value="${y.Value}">${y.Text}</option>`;
                    }
                    $('.ddlZonesllll').html(kk);
                }
            });
        };
        var getRequests = (id, startdate, enddate) => {


            $.ajax({
                type: 'post',
                url: 'StockIssuanceBranchEC_IMS.aspx/GetRequests',
                contentType: "application/json; charset=utf-8",
                dataType: "json", 
                data: JSON.stringify({ EC: id, startdate: startdate, enddate: enddate }),
                success: (rs) => {

                    var data = rs.d;
                    var rows = '';
                    for (let x of data) {

                        rows += `<tr style="text-align:center;">
                                    <td>${x.ECName}</td>                               
                                    <td>${x.Id}</td>
                                    <td>${x.CreatedOn}</td>
                                    <td >${x.TotalReqQty_}</td>
                                    <td>
                                        <button type="button"  CssClass="button" class="button btn btn-sm btn-outline-dark btnDelete" value="${x.Id}" >Delete</button>
                                        <button type="button"  CssClass="button" class="button btn btn-sm btn-outline-dark btnView" data-name="${x.ECName}" data-zone="${x.ECRiderCode}" value="${x.Id}" data-isupdated="${x.IsUpdated}" >View</button>
                                    </td>
                                </tr>`;
                    } 
                    $('#reqGrid').html(rows);  
                }
            });
        };
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
                            url: 'StockIssuanceBranchEC_IMS.aspx/DeleteStockRequest',
                            contentType: "application/json; charset=utf-8",
                            data: JSON.stringify({ RequestId: ReqId }),
                            success: (rs) => {
                                var id = $('#inputState').val(); 
                                getRequests(id, null, null);
                            }
                        });
                    }
                    else {
                        swal("Cancelled", "Cancelled", "error");
                    }
                })

        });
        var getMyStock = () => {

            $.ajax({
                type: 'post',
                url: 'StockIssuanceBranchEC_IMS.aspx/GetMyStock',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: (rs) => {
                    var data = rs.d;
                    var rows = '';

                    for (let x of data) {
                        if (x.Qty > 0) {
                            rows += `<tr>
                                    <!--<td>${x.Zone}</td>-->
                                    <td>${x.OracleCode}</td>
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
                url: 'StockIssuanceBranchEC_IMS.aspx/GetZoneStock',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ EC: zone }),
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

                    $('#zoneStockGrid').html(rows);
                    $('#zonestock').removeClass('d-none');
                }
            });
        };

        var getRequestDetails = (id, isupdated, zone, name) => {

            $.ajax({
                type: 'post',
                url: 'StockIssuanceBranchEC_IMS.aspx/GetRequestsDetails',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ id: id }),
                success: (rs) => {
                    var data = rs.d;
                    var rows = '';

                    $('#reqId').text(id);
                    name += " Stock"
                    $('#zone').text(name);
                    for (let x of data) {
                        rows += `<tr>
                                    <td>${(isupdated == false && x.BarcodeIssueFrom == null) ? `<input class="form-check-input" type="checkbox" id="gridCheck1" style = "position: static; margin-top: 0; margin-left: 0;" disabled>` : `<input class="form-check-input" type="checkbox" id="gridCheck1" style = "position: static; margin-top: 0; margin-left: 0;">`}</td>
                                    <td>${x.OracleCode}</td>
                                    <td>${x.TypeName}</td>
                                    <td><input type="hidden" value="${x.ProductId}">${x.ProductName}</td>
                                    <td><input type="hidden" value="${x.Id}">${x.Qty_}</td>
                                    <td style="width:8em;"><input type="hidden" value="${x.QtyIssueFrom}">${(isupdated == false) ? (x.BarcodeIssueFrom == null) ? `<input style="height:2em; font-size:12px;" autopostback="true"  onkeypress="return isNumberKey(event)" type="text" maxlength="6" class="form-control txtissueqty" disabled>` : `<input style="height:2em; font-size:12px;" autopostback="true"  onkeypress="return isNumberKey(event)" type="text" maxlength="6" class="form-control txtissueqty" >` : `${x.IssueQty}`}</td>
                                    <td>${(isupdated == false && x.BarcodeIssueFrom == null) ? `<input type="text" style="height:2em; font-size:12px;" class="form-control txtbarcodefrom" readonly="readonly">` : `<input type="text" style="height:2em; font-size:12px;" class="form-control txtbarcodefrom" readonly="readonly" value="${x.BarcodeIssueFrom}">`}</td>
                                    <td><input type="text" style="height:2em; font-size:12px;" class="form-control txtbarcodeto" readonly="readonly" value="${x.BarcodeTo}"></td>
                                </tr>`;
                    }

                    $('#reqDetailGrid').html(rows);

                    if (isupdated == false) {
                        $('#btnDiv').html(`<div class="row ml-2 mt-2">                        
                        <div class="col-md-5">
                            <div class="form-group">
                                <label style="font-size: 12px;float:left;">Remarks</label>
                                <input type="text" id="Remarks" style="height:2em; font-size:12px;" type="text" maxlength="200" class="form-control">
                            </div>
                        </div>
                        <div class="col-md-5">
<br/>
                            <div class="form-group">
                                <label style="font-size: 12px;">&nbsp;&nbsp;&nbsp;</label>
                       <button id="btnSubmit" type="button" class="button btn btn-outline-dark" style="float:right;" data-id="${id}" data-zone="${zone}">Submit</button>
                            </div>
                        </div>
                    </div>`);
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
                url: 'StockIssuanceBranchEC_IMS.aspx/SaveIssuanceDetails',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ model: data }),
                success: (rs) => {
                    alert(rs.d);
                    $('#detailRow').addClass('d-none');
                    getRequests("");
                    $('#ModalPopUp').modal('hide');
                    $('#error-msg').html("");
                    document.getElementById('btnSubmit').disabled = false;
                    document.getElementById('LoaderDiv').style.display = 'none';
                    location.reload();
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
            getRequests("", null, null);
             
            $('#chk_selectall').click(function (e) {
                var table = $(e.target).closest('table');
                $('td input:checkbox', table).not(":disabled").prop('checked', this.checked);
            });
            $('#btnFilter').on('click', function () {
                var id = $('#inputState').val();
                var StartDate = document.getElementById('StartDate').value;
                var EndDate = document.getElementById('EndDate').value;
                if (StartDate == '' || EndDate == '') {
                    alert("Select Dates");
                }
                else if (((new Date(EndDate) - new Date(StartDate)) / (1000 * 60 * 60 * 24)) < 0) {
                    alert("Select valid dates");
                }
                else if (((new Date(EndDate) - new Date(StartDate)) / (1000 * 60 * 60 * 24)) > 30) {
                    alert("Maximum days limit is 30");
                }
                else {
                    getRequests(id, StartDate, EndDate);
                } 
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

                var qty = parseInt($(this).parent().parent().find('td:eq(4)').text().replace(/,/g, ''));
                var issueqty = parseInt($(this).val());
                var qtyIssueFrom = parseInt($(this).parent().parent().find('td:eq(5)').find('input[type=hidden]').val());
                if (issueqty > qty) {
                    $(this).addClass('is-invalid');
                    $(this).focus();
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

                    var barcodefrom = parseInt($(this).parent().parent().find('td:eq(6)').find('input[type=text]')[0].value);
                    if (barcodefrom > 0) {
                        var barcodeto = barcodefrom + issueqty - 1;
                        var len = barcodefrom.toString().length;
                        $(this).parent().parent().find('td:eq(7)').find('input[type=text]').val(barcodeto.toString().padStart(len, '0'));
                        $(this).removeClass('is-invalid');

                        var ProductToCheck = parseInt($(this).parent().parent().find('td:eq(3)').find('input').val());

                        if (ProductToCheck == 17) {
                            barcodeto = barcodefrom.toString().slice(0, -1);
                            barcodeto = (parseInt(barcodeto) + issueqty - 1);
                            var OCSANA = Math.floor(barcodeto / 7);
                            OCSANA = barcodeto - OCSANA * 7;
                            OCSANA = barcodeto.toString().concat(OCSANA);
                            barcodeto = OCSANA;
                            //  barcodeto = barcodeto + issueqty - 1;
                            var len = barcodeto.toString().length;
                            $(this).parent().parent().find('td:eq(7)').find('input[type=text]').val(barcodeto.toString().padStart(len, '0'));
                        }
                    }
                    else {
                        $(this).parent().parent().find('td:eq(7)').find('input[type=text]').val(0);
                    }


                }
            }); 
            $('body').on('click', '#btnSubmit', function () {

                var id = parseInt($(this).data('id'));
                var zone = $(this).data('zone');
                var Remarks = $(this).parent('.form-group').parent(".col-md-5").siblings(".col-md-5").find('.form-group').find("#Remarks").val();

                var data = {
                    StockRequestId: id,
                    ZoneCode: zone,
                    Remarks: Remarks,
                    StockIssuanceDetails: []
                };

                var rows = $('#reqDetailGrid').find('tr');

                for (let x of rows) {

                    var row = $(x);
                    var requestDetailid = parseInt(row.find('td:eq(4)').find('input[type=hidden]').val());
                    var requestedQty = parseInt(row.find('td:eq(4)')[0].innerText);

                    var qty = parseInt(row.find('td:eq(5)').find('input[type=text]').val().replace(/,/g, ''));
                    var barcodefrom = parseInt(row.find('td:eq(6)').find('input[type=text]').val());
                    var barcodeto = parseInt(row.find('td:eq(7)').find('input[type=text]').val());
                    var product = parseInt(row.find('td:eq(3)').find('input[type=hidden]').val());
                    var qtyIssueFrom = parseInt(row.find('td:eq(5)').find('input[type=hidden]').val());
                    if (row.find('td:eq(0)').find('input[type=checkbox]').is(":checked") == true) {

                        if (qty > requestedQty) {
                            row.find('td:eq(5)').find('input[type=text]').addClass('is-invalid');
                            row.find('td:eq(5)').find('input[type=text]').focus();
                            return;
                        } else {
                            row.find('td:eq(5)').find('input[type=text]').removeClass('is-invalid');
                        }

                        if (isNaN(qty) || qty <= 0) {
                            row.find('td:eq(5)').find('input[type=text]').addClass('is-invalid');
                            row.find('td:eq(5)').find('input[type=text]').focus();
                            return;
                        } else {
                            row.find('td:eq(5)').find('input[type=text]').removeClass('is-invalid');
                        }

                        if (isNaN(barcodefrom) || barcodefrom < 0) {
                            row.find('td:eq(6)').find('input[type=text]').addClass('is-invalid');
                            row.find('td:eq(6)').find('input[type=text]').focus();
                            return;
                        } else {
                            row.find('td:eq(6)').find('input[type=text]').removeClass('is-invalid');
                        }

                        if (qty > qtyIssueFrom) {
                            row.find('td:eq(5)').find('input[type=text]').addClass('is-invalid');
                            row.find('td:eq(5)').find('input[type=text]').focus();
                            return;
                        } else {
                            row.find('td:eq(5)').find('input[type=text]').removeClass('is-invalid');
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
