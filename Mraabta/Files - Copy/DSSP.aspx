<%@ Page Language="C#" Title="DSSP" AutoEventWireup="true" CodeBehind="DSSP.aspx.cs" Inherits="MRaabta.Files.DSSP" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" moznomarginboxes mozdisallowselectionprint>

<head runat="server">
    <%--<link rel="stylesheet" href="css/print.css" type="text/css" media="print" />--%>
    <script type="text/javascript" src="<%=ResolveUrl("~/Scripts/jquery-3.5.1.min.js") %>"></script>

    <style>
        @media print {
            body {
                -webkit-print-color-adjust: exact; /*chrome & webkit browsers*/
                color-adjust: exact; /*firefox & IE */
            }
        }

        .box2 {
            display: inline-block;
            vertical-align: top;
            text-align: center;
            clear: both;
        }


        .table th {
            border: 0.5px dotted black;
            margin: 0;
            padding: 0;
        }

        .table td {
            border: 0.5px dotted black;
            margin: 0;
            padding: 0;
        }


        @media print {

            @page {
                size: auto;
                margin: 2mm 9mm 2mm 4mm;
            }

            body {
                padding-top: 0px;
            }

            .header, .content {
                clear: both;
            }

            thead.report-header {
            }

            table.content {
                page-break-after: always;
            }

            .header {
                position: fixed;
                top: 0;
            }

            .header-space {
                display: table-header-group;
            }
        }


        /* Center the loader */
        .outer_box {
            background: gray none repeat scroll 0 0;
            height: 100%;
            opacity: 0.9;
            position: fixed;
            top: 0px;
            left: 0px;
            width: 100%;
            z-index: 100000;
            display: flex;
            flex-direction: column;
            justify-content: center;
            align-items: center;
        }

        .loader {
            border: 16px solid #f3f3f3;
            border-radius: 50%;
            border-top: 16px solid #3498db;
            width: 120px;
            height: 120px;
            -webkit-animation: spin 2s linear infinite;
            animation: spin 2s linear infinite;
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

        .large{
            font-size:30px;
        }
    </style>

    <script>
        function printDiv(divName) {
            document.getElementById('AllCheckVoidBtn').style.display = 'none';
            var divsToHide = document.getElementsByClassName("CNCheckbox");
            for (var i = 0; i < divsToHide.length; i++) {
                divsToHide[i].style.display = "none";
            }
            var originalContents = document.body.innerHTML;
            debugger;
            window.addEventListener('load', function () {
                //$(window).on('load', function () {
                debugger;
                var originalContents = document.body.innerHTML;
                var toPrint = document.getElementById('bodyToPrint');
                document.body.innerHTML = toPrint.innerHTML;
                window.print();
                window.addEventListener('afterprint', function () {
                    debugger;
                    document.body.innerHTML = originalContents;

                    var divsToHide = document.getElementsByClassName("CNCheckbox");
                    for (var i = 0; i < divsToHide.length; i++) {
                        divsToHide[i].style.display = "";
                    }
                });
            });
        }

        function showLoader() {
            $("#VoidBtn").attr("disabled", true);
            $('#loaders').show();
        }
        function hideLoader() {
            setTimeout(function () {
                $('#loaders').hide();
                $("#VoidBtn").attr("disabled", false);

            }, 100);
        }
        function printDivFromJS(divName) {
            var originalContents = document.body.innerHTML;
            var toPrint = document.getElementById(divName);
            document.body.innerHTML = toPrint.innerHTML;
            window.print();
            document.body.innerHTML = originalContents;
        }

        function ChangeRow(source) {
            if (source.checked != false) {
                source.parentElement.parentElement.style.backgroundColor = '#f47e24';
            } else {
                source.parentElement.parentElement.style.backgroundColor = 'white';
            }
        }
        function AllCheckVoid()  {
            var Checks = document.getElementsByClassName("CNCheckbox");
                if (document.getElementById('AllCheckVoidBtn').checked) {
                    for (var i = 0; i < Checks.length; i++) 
                        Checks[i].checked = true;
                } else {
                    for (var i = 0; i < Checks.length; i++) 
                        Checks[i].checked = false;
                }
        }
        function VoidCNs() {
            var Checks = document.getElementsByClassName("CNCheckbox");
            var CNsToVoid = [];
            for (var i = 0; i < Checks.length; i++) {
                if (Checks[i].checked) {
                    CNsToVoid.push(Checks[i].value);
                }
            }
            if (CNsToVoid.length==0) {
                return false;
            }
            if (confirm('Press Ok to void these CNs: \r\n\t' + CNsToVoid.join('\r\n\t').toString() + '\r\n')) {
                $.ajax({
                    type: "POST",
                    url: 'DSSP.aspx/VoidCNs',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({ Consignments: CNsToVoid }),
                    beforeSend: function () { showLoader(); },
                    success: (rs) => {
                        hideLoader();
                        alert(rs.d[0][1]);
                        window.location.reload();
                    }, error: function (xhr, ajaxOptions, thrownError) {
                        alert('Error finding consignment');
                    }
                });
            }

            return false;
        }

        $(document).ready(function () {
            document.getElementById('AllCheckVoidBtn').checked = false;
            var Checks = document.getElementsByClassName("CNCheckbox");
            for (var i = 0; i < Checks.length; i++) {
                Checks[i].checked = false;
            }

            let searchParams = new URLSearchParams(window.location.search)
            if (searchParams.has('DSSPNo')) {
                document.getElementById('AllCheckVoidBtn').style.display = 'none';
            }
            
        });

    </script>
</head>
<body>
    <form id="formmm" runat="server" autopostback="false">
        <div id="loaders" runat="server" class="outer_box" style="display: none;">
            <div id="loader" runat="server" class="loader"></div>
        </div>
        <div style="display: inline-flex; width: 100%; float: left">
            <div id="backButtonDiv" style="width: 25%; height: 40px">
                <asp:Button runat="server" ID="backButton" Text="Back" Width="25%" OnClick="Back_btn" Height="30px" />
            </div>
            <div id="buttonsDiv" style="width: 50%; height: 40px">

                <asp:Button runat="server" ID="printBtn" OnClick="Print_Btn" autopostback="false" Visible="false" Text="Print" Width="12.5%" Height="30px" />

                <asp:Button runat="server" ID="generate" OnClick="Generate_Btn" Text="Generate Auto DSSP No." Width="30%" Height="30px" style="height:30px;width:30%;background-color: green;color: white;border: 0;" />


                <asp:Button runat="server" Visible="true" id="VoidBtn" OnClientClick="return VoidCNs()" style="float: right; height: 30px ;background-color: red;color: white;border: 0;"  Text="Void Consignment"   />
            </div>
        </div>


        <div id="bodyToPrint">
            <%--  --%>
            <table class="toptable">
                <thead class="report-header">
                    <tr>
                        <td>
                            <%--  --%>
                            <div class="header-space" style="">


                                <div style="width: 100%">
                                    <div class="LogoArea box2" style="margin-top: 4px">
                                        <asp:Image runat="server" ImageUrl="~/images/mnplogo.png" Width="78px" />
                                    </div>
                                    <div class="MainHeader box2" style="width: 41%; font-size: 13px; margin-top: -12px; margin-left: 12px">
                                        <h2>Daily Sales and Shipment Performa</h2>
                                    </div>
                                    <%--<div class="DateOnTopRight  ">--%>
                                    <div style="margin-left: 22px; width: 10%; border: solid; border-width: thin; text-align: center; font-size: 10px" class="box2">
                                        <p>It contains system generated Transactions</p>
                                    </div>
                                    <div class="DateHeaders box2" style="font-size: 10px; margin-left: 4px; width: 12%; text-align: left">
                                        <label>Sys Date:</label><br />
                                        <label>Print Date:</label><br />
                                        <label>Sales date:</label><br />
                                        <label>Submission date:</label><br />
                                        <label>Auto DSSP No:</label>
                                    </div>
                                    <div class="DateValues box2" style="font-size: 10px; width: 16%; text-align: left">
                                        <asp:Label ID="SysDate" runat="server"></asp:Label><br />
                                        <asp:Label ID="PrintDate" runat="server"></asp:Label><br />
                                        <asp:Label ID="SalesDate" runat="server"></asp:Label><br />
                                        <asp:Label ID="SubmissionDate" runat="server"></asp:Label><br />
                                        <asp:Label ID="AutoDSSPNO" runat="server" ForeColor="Red" Font-Bold="true">No DSSP no. generated</asp:Label>
                                    </div>

                                </div>


                                <div style="border: solid; border-width: thin; border-color: #000000; height: 34px; width: 100%; font-size: 10px">

                                    <%--<legend id="Legend7" visible="true" style="width: auto; font-size: 16px; color: #1f497d;"></legend>--%>
                                    <div class="Values1 box2" style="width: 6%; text-align: left; margin-left: 4px;">

                                        <b>
                                            <label>Zone:</label><br />
                                            <label>Branch:</label><br />
                                            <label>Shift:</label></b>
                                    </div>

                                    <div class="DateValues box2" style="width: 13%; text-align: left">

                                        <asp:Label runat="server" ID="Zone"></asp:Label><br />
                                        <asp:Label runat="server" ID="Branch"></asp:Label><br />
                                        <asp:Label runat="server" ID="Shift"></asp:Label>
                                    </div>

                                    <div class="DateValues box2" style="width: 13%; margin-left: 4px; text-align: left">
                                        <b>
                                            <label>Booking Code:</label><br />
                                            <label>Express C. Code:</label><br />
                                            <label>COT Shipment:</label>
                                        </b>
                                    </div>
                                    <div class="DateValues box2" style="width: 16%; text-align: left">

                                        <asp:Label runat="server" ID="bookingCode"></asp:Label><br />
                                        <asp:Label runat="server" ID="ExpressCenterCode"></asp:Label><br />
                                        <asp:Label runat="server" ID="COTShipmentCount"></asp:Label>
                                    </div>
                                    <div class="DateValues box2" style="width: 19%; margin-left: 4px; text-align: left">
                                        <b>
                                            <label>Staff Name :</label><br />
                                            <label>Express C. Name:</label><br />
                                            <label>Vehicle & Driver Name:</label></b>
                                    </div>

                                    <div class="DateValues box2" style="width: 29%; text-align: left">

                                        <asp:Label runat="server" ID="StaffName"></asp:Label><br />
                                        <asp:Label runat="server" ID="ECName"></asp:Label><br />
                                        <asp:Label runat="server" ID="VehicleNo"></asp:Label>
                                    </div>
                                </div>

                            </div>


                        </td>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td>


                            <div style="width: 100%">
                                <table class="table " cellpadding="0" cellspacing="0" style="display: inline-block; float: left; width: 35%; font-size: 10px">
                                    <tr>
                                        <th style="text-align: center" colspan="5">
                                            <b>Cash sales summary product wise	</b>
                                        </th>
                                    </tr>
                                    <tr style="text-align: center">
                                        <th>Product Summary</th>
                                        <th>Shipment Quantity</th>
                                        <th>Weight (Kg)</th>
                                        <th>Pcs</th>
                                        <th>Charged Amount</th>
                                    </tr>
                                    <asp:Literal ID="CashSalesSummaryProductLiteral" runat="server" />

                                </table>
                                <table class="table" cellpadding="0" cellspacing="0" style="font-size: 10px; margin-left: 8px; display: inline-block; float: left; width: 15%">
                                    <tr>
                                        <th style="text-align: center" colspan="5">
                                            <b>Service Wise Summary	</b>
                                        </th>
                                    </tr>
                                    <tr style="text-align: center">
                                        <th>Service</th>
                                        <th>Quantity</th>
                                    </tr>

                                    <asp:Literal ID="ServiceWiseSummaryLiteral" runat="server" />

                                </table>

                                <table class="table " cellpadding="0" cellspacing="0" style="font-size: 10px; margin-left: 8px; display: inline-block; float: left; width: 28%">
                                    <tr>
                                        <th style="text-align: center" colspan="5">
                                            <b>Summary of International / FedEx</b>
                                        </th>
                                    </tr>
                                    <tr style="text-align: center">
                                        <th>Product Summary</th>
                                        <th>Shipment Quantity   </th>
                                        <th>Amount</th>

                                    </tr>
                                    <asp:Literal ID="summaryOfInternationSalesLiteral" runat="server" />
                                </table>

                                <table class="table " cellpadding="0" cellspacing="0" style="font-size: 10px; margin-left: 8px; display: inline-block; float: left; width: 18%">
                                    <tr>
                                        <th style="text-align: center" colspan="5">
                                            <b>Shipment & Amount Summary</b>
                                        </th>
                                    </tr>
                                    <tr style="text-align: center">
                                        <th>Shipment</th>
                                        <th>Total Amount</th>
                                    </tr>

                                    <asp:Literal ID="ShipmentAmountSummaryLiteral" runat="server" />

                                </table>
                            </div>

                            <div class="content">
                                <table class="table LastTable" cellpadding="0" cellspacing="0" style="width: 100%">
                                    <thead>
                                        <tr>
                                            <th colspan="17" style="background-color: dimgray; font-size: 14px"><b>Consignment wise Detail</b></th>
                                        </tr>


                                        <tr style="background-color: dimgray;">
                                            <th>
                                                
                                                <input type="checkbox" id="AllCheckVoidBtn" onchange="AllCheckVoid()" /></th>
                                            <th class="lastTableTH">
                                                <asp:Label runat="server" Font-Size="10px">S#</asp:Label></th>
                                            <th class="lastTableTH">
                                                <asp:Label runat="server" Font-Size="10px">CN Number</asp:Label></th>
                                            <th class="lastTableTH">
                                                <asp:Label runat="server" Font-Size="10px">Booking<br />Date</asp:Label></th>
                                            <th class="lastTableTH">
                                                <asp:Label runat="server" Font-Size="10px">Service</asp:Label></th>
                                            <th class="lastTableTH">
                                                <asp:Label runat="server" Font-Size="10px">Dstn</asp:Label></th>
                                            <th class="lastTableTH">
                                                <asp:Label runat="server" Font-Size="10px">Weight</asp:Label></th>
                                            <th class="lastTableTH">
                                                <asp:Label runat="server" Font-Size="10px">Pcs</asp:Label></th>
                                            <th class="lastTableTH">
                                                <asp:Label runat="server" Font-Size="10px">Gross<br />Amount</asp:Label></th>
                                            <th class="lastTableTH">
                                                <asp:Label runat="server" Font-Size="10px">Charge<br />Amount</asp:Label></th>
                                            <th class="lastTableTH">
                                                <asp:Label runat="server" Font-Size="10px">Shipment<br />Discount (%)</asp:Label></th>
                                            <th class="lastTableTH">
                                                <asp:Label runat="server" Font-Size="10px">Supp.<br />Service</asp:Label></th>
                                            <th class="lastTableTH">
                                                <asp:Label runat="server" Font-Size="10px">Supp.<br />Charges</asp:Label></th>
                                            <th class="lastTableTH">
                                                <asp:Label runat="server" Font-Size="10px">Franchise<br />Comission</asp:Label></th>
                                            <th class="lastTableTH">
                                                <asp:Label runat="server" Font-Size="10px">Payment<br />Method</asp:Label></th>
                                            <th class="lastTableTH">
                                                <asp:Label runat="server" Font-Size="10px">Calculated<br />Incentive</asp:Label></th>
                                            <th class="lastTableTH">
                                                <asp:Label runat="server" Font-Size="10px">Amount<br />Collect</asp:Label></th>
                                        </tr>
                                    </thead>
                                    <asp:Literal ID="ConsignmentWiseLiteral" runat="server" />
                                </table>

                            </div>
                            <%--  --%>
                        </td>
                    </tr>
                </tbody>
                <tfoot>

                    <tr>
                        <td>
                            <table class="table VoidTable" cellpadding="0" cellspacing="0" style="width: 100%">
                                <thead>
                                    <tr>
                                        <th colspan="17" style="background-color: lightcoral; font-size: 14px"><b>Void Consignments</b></th>
                                    </tr>
                                    <tr style="background-color: lightcoral;">
                                        <th class="lastTableTH" colspan="2">
                                            <asp:Label runat="server" Font-Size="10px">S#</asp:Label></th>
                                        <th class="lastTableTH">
                                            <asp:Label runat="server" Font-Size="10px">CN Number</asp:Label></th>
                                        <th class="lastTableTH">
                                            <asp:Label runat="server" Font-Size="10px">Booking Date</asp:Label></th>
                                        <th class="lastTableTH">
                                            <asp:Label runat="server" Font-Size="10px">Service</asp:Label></th>
                                        <th class="lastTableTH">
                                            <asp:Label runat="server" Font-Size="10px">Dstn</asp:Label></th>
                                        <th class="lastTableTH">
                                            <asp:Label runat="server" Font-Size="10px">Weight</asp:Label></th>
                                        <th class="lastTableTH">
                                            <asp:Label runat="server" Font-Size="10px">Pcs</asp:Label></th>
                                        <th class="lastTableTH">
                                            <asp:Label runat="server" Font-Size="10px">Gross Amount</asp:Label></th>
                                        <th class="lastTableTH">
                                            <asp:Label runat="server" Font-Size="10px">Charge Amount</asp:Label></th>
                                        <th class="lastTableTH">
                                            <asp:Label runat="server" Font-Size="10px">Shipment Discount</asp:Label></th>
                                        <th class="lastTableTH">
                                            <asp:Label runat="server" Font-Size="10px" Width="5%">Supp. Service</asp:Label></th>
                                        <th class="lastTableTH">
                                            <asp:Label runat="server" Font-Size="10px">Supp. Charges</asp:Label></th>
                                        <th class="lastTableTH">
                                            <asp:Label runat="server" Font-Size="10px">Franchise Comission</asp:Label></th>
                                        <th class="lastTableTH">
                                            <asp:Label runat="server" Font-Size="10px">Payment Method</asp:Label></th>
                                        <th class="lastTableTH">
                                            <asp:Label runat="server" Font-Size="10px">Calculated Incentive</asp:Label></th>
                                        <th class="lastTableTH">
                                            <asp:Label runat="server" Font-Size="10px">Amount Collect</asp:Label></th>
                                    </tr>
                                </thead>
                                <asp:Literal ID="VoidTableLiteral" runat="server" />
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="footerDiv row" style="display: flex; margin-top: 10px; width: 100%; font-size: 12px">

                                <div class="BookingSign" style="width: 35%">
                                    Booking Staff Signature:____________________
                                </div>
                                <div class="driverSign" style="width: 32%">
                                    Driver Signature:___________________
                                </div>
                                <div class="CashierSign" style="width: 32%">
                                    Cashier Signature:___________________
                                </div>
                            </div>
                        </td>
                    </tr>
                </tfoot>
            </table>
        </div>
    </form>
</body>
</html>
