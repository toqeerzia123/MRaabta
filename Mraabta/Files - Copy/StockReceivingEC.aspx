<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="StockReceivingEC.aspx.cs" Inherits="MRaabta.Files.StockReceivingEC" %>

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
        .MainTbl {
            border-collapse: collapse;
            width: 100%;
            th

        {
            text-align: left;
            padding: 8px;
        }

        tr {
            text-align: left;
            padding: 8px;
        }

        }

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
            width: 180px;
            height: 35px;
        }

        #btndiv {
            font-size: 90px;
        }
    </style>

    <div class="row">
        <div class="col-md-6">
            <table cellpadding="0" cellspacing="0" width='100%' class='mGrid_Table'>
                <tr>
                    <td colspan="12" align="center" class="head_column">
                        <h3 style="text-align: center">Stocks Recieved (Express Center)
                     
                        </h3>
                    </td>
                </tr>
            </table>
            <div class=" shadow rounded">
                <div class="">

                    <table class="MainTbl table table-striped">
                        <thead class="">
                            <tr style="font-size: 12px;">
                                <th>ID</th>
                                <th>Date Requested</th>
                                <th style="width: 25%">Total Requested Quantity</th>
                                <th style="width: 25%">Total Issued Quantity</th>
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
            <%--<table cellpadding="0" cellspacing="0" width='100%' class='mGrid_Table'>
                        <tr>
                            <td colspan="12" align="center" class="head_column">
                                <h3 style="text-align: center">Stocks Recieved
                                </h3>
                            </td>
                        </tr>
                    </table>--%>
            <%--<div class="tab-content" id="myTabContent">
                      <div class="tab-pane fade active" id="about" role="tabpanel" aria-labelledby="about-tab"></div> 

                            <div class="tab-pane fade " id="home" role="tabpanel" aria-labelledby="home-tab"></div>
                            <div class="tab-pane fade" id="profile" role="tabpanel" aria-labelledby="profile-tab"></div>
                    <%--  <div class="tab-pane fade" id="contact" role="tabpanel" aria-labelledby="contact-tab"></div>
                    </div>--%>


            <table cellpadding="0" cellspacing="0" width='100%' class='mGrid_Table'>
                <tr>
                    <td>
                        <nav>
                                    <div class="nav nav-tabs nav-fill " id="nav-tab" role="tablist">
                                        <a class="nav-item nav-link active" id="nav-about-tab" data-toggle="tab" href="#nav-about" role="tab" aria-controls="nav-about" aria-selected="true">Express Center Stock</a>

                                        <a class="nav-item nav-link " id="nav-home-tab" data-toggle="tab" href="#nav-home" role="tab" aria-controls="nav-home" aria-selected="false">Branch Stock</a>
                                                           </div>
                            </nav>
                    </td>
                </tr>
            </table>

            <div class="tab-content" id="nav-tabContent">
                <div class="tab-pane fade  " id="nav-home" role="tabpanel" aria-labelledby="nav-home-tab">
                    <table class="MainTbl table table-striped mt-1">
                        <thead>
                            <tr style="font-size: 12px;">
                                <th>Type </th>
                                <th>Product</th>
                                <th>Quantity</th>
                            </tr>
                        </thead>
                        <tbody id="HOStockGrid" style="font-size: 12px;">
                        </tbody>
                    </table>

                    <br />

                </div>

                <div class="tab-pane fade  show active" id="nav-about" role="tabpanel" aria-labelledby="nav-about-tab">

                    <table class="MainTbl table table-striped mt-1">
                        <thead>
                            <tr style="font-size: 12px;">

                                <th>Type </th>
                                <th>Product</th>
                                <th>Quantity</th>
                            </tr>
                        </thead>
                        <tbody id="myStockGrid" style="font-size: 12px;">
                        </tbody>

                    </table>
                    <br />
                </div>
            </div>

        </div>
    </div>
    <div class="modal fade" id="ModalPopUp" role="dialog">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 style="text-align: center">Stock Received Details</h4>
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                </div>
                <div class="modal-body">
                    <div id="detailRow" style="display: none;">
                        <div class="">
                            <table class="MainTbl table table-striped">
                                <thead>
                                    <tr style="font-size: 12px;">
                                        <th>Type</th>
                                        <th>Product</th>
                                        <th>Barcode From</th>
                                        <th>Barcode To</th>
                                        <th>Requested Quantity</th>
                                        <th>Received Quantity</th>

                                    </tr>
                                </thead>
                                <tbody id="receivedRequestDetailGrid" style="font-size: 12px;">
                                </tbody>
                            </table>
                             <div id="LoaderDiv" style="display:none"><img id="LoaderImg" src="../assets/images/loader-1.gif" alt="" /></div>

                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <div id="SubmitBtn"></div>
                </div>
            </div>
        </div>
    </div>


    <script type="text/javascript" src="../assets/bootstrap-4.3.1-dist/js/bootstrap.min.js"></script>

    <script type="text/javascript">
        var getRequests = () => {
            $.ajax({
                type: 'post',
                url: 'StockReceivingEC.aspx/GetIssuances',
                contentType: "application/json; charset=utf-8",
                async: false,
                dataType: "json",
                success: (rs) => {
                    var data = rs.d;
                    var rows = '';
                    for (let x of data) {
                        rows += `<tr style="">
                                    <td>${x.RequestID}</td>
                                    <td>${x.CreatedOn}</td>
                                    <td>${x.TotalReqQty_}</td>
                                    <td>${x.TotalIssueQty_}</td>
                                    <td>
                                        <button type="button"   class="button btn btn-sm btn-outline-dark btnView" data-id="${x.IssuanceID}" value="${x.IssuanceID}" >View</button>
                                    </td>
                                </tr>`;
                    }

                    $('#reqGrid').html(rows);
                }
            });
        };


        var getMyStock = () => {

            $.ajax({
                type: 'post',
                url: 'StockReceivingEC.aspx/GetMyStock',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: (rs) => {
                    var data = rs.d;
                    var rows = '';

                    for (let x of data) {
                        if (x.Qty > 0) {
                            rows += `<tr>
                                    <td>${x.TypeName}</td>
                                    <td>${x.ProductName}</td>
                                    <td>${x.Qty_}</td>
                                    
                                </tr>`;
                        }

                    }
                    $('#myStockGrid').html(rows);
                }
            });
        };
        var getHOStock = () => {

            $.ajax({
                type: 'post',
                url: 'StockReceivingEC.aspx/GetHOStock',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: (rs) => {
                    var data = rs.d;
                    var rows = '';

                    for (let x of data) {
                        if (x.Qty > 0) {
                            rows += `<tr>
                                    <td>${x.TypeName}</td>
                                    <td>${x.ProductName}</td>
                                    <td>${x.Qty_}</td>
                                    
                                </tr>`;
                        }

                    }
                    $('#HOStockGrid').html(rows);
                }
            });
        };
        var getReceivedRequest = () => {

            $.ajax({
                type: 'post',
                url: 'StockReceivingEC.aspx/getReceivedRequest',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: (rs) => {
                    var data = rs.d;
                    var rows = '';

                    for (let x of data) {
                        rows += `<tr>
                                    <td><input type="hidden" value="${x.ID}">${x.ZoneName}</td>
                                    <td>${x.TotalReqQty}</td>
                                    <td>${x.TotalRecieveQty}</td>
                                    <td>
                                        <button type="button"  CssClass="button" class="button btn btn-sm btn-outline-dark btnViewDetail" value="${x.ID}"  >View</button>
                                    </td>
                                    
                                </tr>`;

                    }
                    $('#receivedRequestGrid').html(rows);
                }
            });
        };
        var getReceivedRequestDetails = (id) => {

            $.ajax({
                type: 'post',
                url: 'StockReceivingEC.aspx/GetIssuancesDetails',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                data: JSON.stringify({ ID: id }),
                success: (rs) => {
                    var data = rs.d;
                    var rows = '';

                    var issuanceId = 0;
                    for (let x of data) {
                        rows += `<tr>
                                    <td>${x.TypeName}</td>
                                    <td>${x.ProductName}</td>   
                                    <td>${(x.IssuanceID == 0) ? `-` : x.BarcodeFrom} </td>
                                    <td>${(x.IssuanceID == 0) ? `-` : x.BarcodeTo} </td>
                                    <td>${x.RequestQty}</td>
                                    <td>${(x.IssuanceID == 0) ? `Not Issued` : x.RecievingQty} </td> 
                                </tr>`;
                        if (x.IssuanceID != 0) {
                            issuanceId = x.IssuanceID;
                        }

                    }

                    document.getElementById('SubmitBtn').innerHTML = `<button type="button"  CssClass="button" class="button btn btn-sm btn-outline-dark BtnReceive" value="${issuanceId}" >Receive</button>`;
                    $('#receivedRequestDetailGrid').html(rows);
                }
            });
        };
        var getIssueRequest = () => {

            $.ajax({
                type: 'post',
                url: 'StockReceivingEC.aspx/GetHOStock',
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
                    $('#IssuedRequestGrid').html(rows);
                }
            });
        };

        var getZone = () => {

            $.ajax({
                type: 'post',
                url: 'StockReceivingEC.aspx/GetZone',
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
                    $('#reqGridMyStock').html(rows);
                    $('#mystocks').removeClass('d-none');
                }
            });
        };

       
        var save = (ID) => {
            $.ajax({
                type: 'post',
                url: 'StockReceivingEC.aspx/SaveReceivedPerIssued',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ ID: ID }),
                success: (rs) => {
                    alert(rs.d);
                    // $('#detailRow').addClass('d-none');
                    document.getElementById('LoaderDiv').style.display = 'none';
                    document.getElementsByClassName('BtnReceive').disabled = false;
                    getRequests();
                    getMyStock();
                }
                ,
                error: (error) => {
                    alert('Error:' + error.statusText);
                    document.getElementById('LoaderDiv').style.display = 'none';
                    document.getElementsByClassName('BtnReceive').disabled = false;

                }
            });
        };

        $(function () {

            //getZone();
            getMyStock();
            getRequests();
            getHOStock();
            // getReceivedRequest();
            //getIssueRequest();

            $('body').on('click', '.btnView', function () {


                document.getElementById("detailRow").style.display = 'block';
                var id = parseInt($(this).val());

                getReceivedRequestDetails(id);
                //jQuery.noConflict();
                $('#ModalPopUp').modal('show');

            });
            $('body').on('click', '.btnViewDetail', function () {

                var id = parseInt($(this).val());
                getReceivedRequestDetails(id);

                document.getElementById('detailRow').style.display = "block";

                //jQuery.noConflict();
                $('#ModalPopUp').modal('show');


            });
            $('body').on('blur', '.txtissueqty', function () {
                var qty = parseInt($(this).parent().parent().find('td:eq(1)').text());
                var issueqty = parseInt($(this).val());
                if (issueqty > qty) {
                    $(this).addClass('is-invalid');
                    $(this).focus();
                    return;
                } else {
                    $(this).removeClass('is-invalid');
                }
            });




            $('body').on('click', '.BtnReceive', function () {

                var IssuanceId = $(this).val();
                document.getElementsByClassName('BtnReceive').disabled = true;
                document.getElementById('LoaderDiv').style.display = '';

                save(IssuanceId);

                //jQuery.noConflict();
                $('#ModalPopUp').modal('toggle');

            });

        });

    </script>

</asp:Content>
