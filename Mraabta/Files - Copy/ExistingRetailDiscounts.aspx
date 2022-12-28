<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="ExistingRetailDiscounts.aspx.cs" Inherits="MRaabta.Files.ExistingRetailDiscounts" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="Ajax1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>



<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <%--
    <link rel="Stylesheet" href="../assets/bootstrap-4.3.1-dist/css/bootstrap.min.css" />
    <script type="text/javascript" src="../assets/js/jquery-1.11.0.min.js"></script>
    <script type="text/javascript" src="../assets/bootstrap-4.3.1-dist/js/bootstrap.min.js"></script>--%>
    <script src="../assets/js/jquery-1.11.0.min.js"></script>

    <%--<script src="../assets/bootstrap-4.3.1-dist/js/bootstrap.min.js"></script>
    <script src="../assets/bootstrap-4.3.1-dist/js/bootstrap.bundle.min.js"></script>
    <link rel="Stylesheet" href="../assets/bootstrap-4.3.1-dist/css/bootstrap.min.css" />--%>

    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/css/bootstrap.min.css">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.4.1/jquery.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/js/bootstrap.min.js"></script>

    <style type="text/css">
        body {
           font-size:13px;
           font-weight:none;
           line-height:1.20;
        }

        
.ModifiedGrid {
    background-color: #fff;
    border-collapse: collapse;
    font-family: Tahoma;
    /*font-size: 11px;*/
    /*margin: 1px 5px 0;*/
    width: 99%;
}

.ModifiedGrid th {
    background: #000 none repeat scroll 0 0;
    /*border-left: 1px solid #525252;*/
    color: #fff;
        padding: 2px 2px 0px 0px;

    font-size: 12px;
   text-align: center;
    text-transform: uppercase !important;
   /*  white-space: nowrap;*/
}
.ModifiedGrid td {
    font-size:12px;
}

/*.tbl-large > div > .ModifiedGrid th {
    background: #eee none repeat scroll 0 0 !important;
    border: 0 none !important;
    color: #000 !important;
}
.ModifiedGrid th {
    background: #000 none repeat scroll 0 0;
    border-left: 1px solid #525252;
    color: #fff;
    font-size: 12px;
    padding: 10px 8px;
    text-align: center;
    text-transform: uppercase !important;
    white-space: nowrap;
}

.ModifiedGrid td {
    border: 1px solid #c1c1c1;
    /*padding: 5px;
    white-space: nowrap;
}*/
    </style>
    <script type="text/javascript">

        $(document).ready(function () {
            //$.ajax({
            //    type: "GET",
            //    url: 'ExistingRetailDiscounts.aspx/GetActiveDiscounts',
            //    contentType: "application/json; charset=utf-8",
            //    dataType: "json",
            //    //   data: JSON.stringify({ groupId: groupId }),
            //    success: (rs) => {
            //        var rows = '';

            //        for (let y of rs.d) {
            //            rows += `<tr >
            //                        <td >${y.zone}</td> 
            //                        <td >${y.branch}</td> 

            //                        <td >${y.expressCenter}</td> 

            //                        <td class="fromDateTd">${y.fromDate}</td> 
            //                        <td class="toDateTd">${y.toDate}</td> 
            //                        <td  >${y.serviceType}</td> 

            //                        <td class="serviceTypeTd" >${y.shortDescription}</td> 
            //                        <td  >${y.longDescription}</td> 
            //                        <td  >${y.minShipment}</td> 
            //                        <td  >${y.maxShipment}</td> 
            //                        <td  >${y.minShipmentWeight}</td> 
            //                        <td  >${y.maxShipmentWeight}</td> 
            //                        <td  >${y.discountType}</td> 
            //                        <td  >${y.discountValue}</td> 
            //                        <td  >${y.parentGroupId}</td> 
            //                        <td  >${y.specialDiscountId}</td> 
            //             </tr>`;
            //        }
            //        $('#DiscountsActive').html(rows);

            //    }
            //});

            //$.ajax({
            //    type: "GET",
            //    url: 'ExistingRetailDiscounts.aspx/GetPendingDiscounts',
            //    contentType: "application/json; charset=utf-8",
            //    dataType: "json",
            //    //   data: JSON.stringify({ groupId: groupId }),
            //    success: (rs) => {
            //        var rows = '';

            //        for (let y of rs.d) {
            //            rows += `<tr >
            //                        <td >${y.zone}</td> 
            //                        <td >${y.branch}</td> 

            //                        <td >${y.expressCenter}</td> 

            //                        <td class="fromDateTd">${y.fromDate}</td> 
            //                        <td class="toDateTd">${y.toDate}</td> 
            //                        <td >${y.serviceType}</td> 

            //                        <td class="serviceTypeTd" >${y.shortDescription}</td> 
            //                        <td  >${y.longDescription}</td> 
            //                        <td  >${y.minShipment}</td> 
            //                        <td  >${y.maxShipment}</td> 
            //                        <td  >${y.minShipmentWeight}</td> 
            //                        <td  >${y.maxShipmentWeight}</td> 
            //                        <td  >${y.discountType}</td> 
            //                        <td  >${y.discountValue}</td> 
            //                        <td  >${y.parentGroupId}</td> 
            //                        <td  >${y.specialDiscountId}</td> 
            //             </tr>`;
            //        }
            //        $('#DiscountsPending').html(rows);

            //    }
            //});

            //debugger;   
            //$.ajax({
            //    type: "GET",
            //    url: 'ExistingRetailDiscounts.aspx/GetInactiveDiscounts',
            //    contentType: "application/json; charset=utf-8",
            //    dataType: "json",
            //    async: false,
            //    //   data: JSON.stringify({ groupId: groupId }),
            //    success: (rs) => {
            //        debugger;
            //        var rows = '';

            //        debugger;

            //        for (let y of rs.d) {
            //            rows += `<tr >
            //                        <td >${y.zone}</td> 
            //                        <td >${y.branch}</td> 

            //                        <td >${y.expressCenter}</td> 

            //                        <td class="fromDateTd">${y.fromDate}</td> 
            //                        <td class="toDateTd">${y.toDate}</td> 
            //                        <td  >${y.serviceType}</td> 

            //                        <td class="serviceTypeTd" >${y.shortDescription}</td> 
            //                        <td  >${y.longDescription}</td> 
            //                        <td  >${y.minShipment}</td> 
            //                        <td  >${y.maxShipment}</td> 
            //                        <td  >${y.minShipmentWeight}</td> 
            //                        <td  >${y.maxShipmentWeight}</td> 
            //                        <td  >${y.discountType}</td> 
            //                        <td  >${y.discountValue}</td> 
            //                        <td  >${y.parentGroupId}</td> 
            //                        <td  >${y.specialDiscountId}</td> 
            //             </tr>`;
            //        }
            //        debugger;
            //        $('#DiscountsInactive').html(rows);

            //    }, error: function (xhr, status, error) {
            //        var err = eval("(" + xhr.responseText + ")");
            //        alert(err.Message);
            //    }

            //});


            $(".nav-tabs a").click(function () {
                $(this).tab('show');
            });
        });

    </script>
     <table cellpadding="0" cellspacing="0" width='100%' class='mGrid_Table'>
        <tr>
            <td colspan="12" align="center" class="head_column">
                <h3 style="text-align: center">Existing Retail Discounts
                </h3>
            </td>
        </tr>
    </table>



    <ul class="nav nav-tabs">
        <li class="active"><a href="#home" style="font-size:12px">Active</a></li>
        <li><a href="#menuPending" style="font-size:12px">Pending</a></li>
        <li><a href="#menuInactive" style="font-size:12px">Approved (Inactive)</a></li>
    </ul>

    <div class="tab-content">
        <div id="home" class="tab-pane fade in active">
            <table class="MainTbl table table-striped TdScroll" style="width: 100%"
                id="DiscountTable">
             <%--<table class="table " cellpadding="0" cellspacing="0" style="display: inline-block; float: left; width: 35%; font-size: 14px">--%>

                <thead style="background-color: #444;">
                    <tr style="font-size: 12px; color: white">
                        <th style="color: white">S.#</th>

                        <th style="color: white">Group Id</th>
                        <th style="color: white">Zone</th>
                        <th style="color: white">Branch</th>
                        <th style="color: white">Express Center</th>
                        <th style="color: white">Valid From</th>
                        <th style="color: white">Valid To</th>
                        <th style="color: white">Service Type</th>
                        <th style="color: white">Short Description</th>
                        <th style="color: white">Long Description</th>
                        <th style="color: white">Min. Shipment</th>
                        <th style="color: white">Max. Shipment</th>
                        <th style="color: white">Min. Shipment Weight</th>
                        <th style="color: white">Max. Shipment Weight</th>
                        <th style="color: white">Discount Type</th>
                        <th style="color: white">Discount Value</th>
                        <th style="color: white">Special Discount Id</th>
                    </tr>
                </thead>
               <%-- <tbody id="DiscountsActive" style="font-size: 12px;">
                </tbody>--%>
                
              <asp:Literal ID="ActiveLiteral" runat="server"  />

            </table>

            </div>                        

        <div id="menuPending" class="tab-pane fade out">
            <table class="MainTbl table table-striped TdScroll" style="width: 100%"
                id="">
                <thead style="background-color: #444;">
                    <tr style="font-size: 12px; color: white">
                        <th style="color: white">S.#</th>

                        
                        <th style="color: white">Group Id</th>
                        <th style="color: white">Zone</th>
                        <th style="color: white">Branch</th>
                        <th style="color: white">Express Center</th>
                        <th style="color: white">Valid From</th>
                        <th style="color: white">Valid To</th>
                        <th style="color: white">Service Type</th>
                        <th style="color: white">Short Description</th>
                        <th style="color: white">Long Description</th>
                        <th style="color: white">Min. Shipment</th>
                        <th style="color: white">Max. Shipment</th>
                        <th style="color: white">Min. Shipment Weight</th>
                        <th style="color: white">Max. Shipment Weight</th>
                        <th style="color: white">Discount Type</th>
                        <th style="color: white">Discount Value</th>
                        <th style="color: white">Special Discount Id</th>
                    </tr>
                </thead>
                <tbody id="DiscountsPending" style="font-size: 12px;">
                </tbody>
              <asp:Literal ID="PendingLiteral" runat="server"  />

            </table>
        </div>
        <div id="menuInactive" class="tab-pane fade out">
           <table class="MainTbl table table-striped TdScroll" style="width: 100%"
                id="">
                <thead style="background-color: #444;">
                    <tr style="font-size: 12px; color: white">
                        <th style="color: white">S.#</th>
                      
                        <th style="color: white">Group Id</th>
                        <th style="color: white">Zone</th>
                        <th style="color: white">Branch</th>
                        <th style="color: white">Express Center</th>
                        <th style="color: white">Valid From</th>
                        <th style="color: white">Valid To</th>
                        <th style="color: white">Service Type</th>
                        <th style="color: white">Short Description</th>
                        <th style="color: white">Long Description</th>
                        <th style="color: white">Min. Shipment</th>
                        <th style="color: white">Max. Shipment</th>
                        <th style="color: white">Min. Shipment Weight</th>
                        <th style="color: white">Max. Shipment Weight</th>
                        <th style="color: white">Discount Type</th>
                        <th style="color: white">Discount Value</th>
                        <th style="color: white">Special Discount Id</th>
                    </tr>
                </thead>
                <tbody id="DiscountsInactive" style="font-size: 12px;">
                </tbody>
              <asp:Literal ID="InactiveLiteral" runat="server"  />

               </table>
            <div >

<%--            <asp:GridView id="InactiveGrid" Width="100%"  runat="server" HeaderStyle-BackColor="#444" BorderColor="#DEDFDE" BorderStyle="None" AutoGenerateColumns="true" CssClass="ModifiedGrid" >
                <Columns>
                    <asp:TemplateField HeaderText="S.No." ItemStyle-Width="2%">
                        <ItemTemplate>
                            <asp:Label ID="lblRowNumber" Text='<%# Container.DataItemIndex + 1 %>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                   
                    </Columns>
            </asp:GridView>--%>
                </div>
        </div>
      

    </div>




    <%--    <fieldset style="border: solid; border-width: thin; height: auto; border-color: #a8a8a8;"
        class="ml-2 mr-2">

        <legend id="Legend5" visible="true" style="width: auto; font-size: 16px; font-weight: bold; color: #1f497d;"></legend>

        <table style="margin-left: 10px; font-size: medium; padding-bottom: 0px; width: 100%;">
        </table>
    </fieldset>
    <br />--%>
</asp:Content>
