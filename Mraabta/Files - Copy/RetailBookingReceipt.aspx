<%@ Page Language="C#" Title="Booking Slip" AutoEventWireup="true" CodeBehind="RetailBookingReceipt.aspx.cs" Inherits="MRaabta.Files.RetailBookingReceipt" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" moznomarginboxes mozdisallowselectionprint>
<head runat="server">
    <%--<link rel="stylesheet" href="css/print.css" type="text/css" media="print" />--%>
    <style>
        .InnerTables th, td {
            border: 1px black solid;
            border-spacing: 0;
        }

            td.label {
                width: 100px;
            }

            td.label_text {
                text-align: center;
            }
    </style>
    <script>
        function printDiv(divName) {
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
                });
            });
        }

        function printDivFromJS(divName) {
            var originalContents = document.body.innerHTML;

            var toPrint = document.getElementById(divName);
            document.body.innerHTML = toPrint.innerHTML;

            window.print();
            document.body.innerHTML = originalContents;
        }

    </script>
    <style>
        @media print {
            body {
                -webkit-print-color-adjust: exact; /*chrome & webkit browsers*/
                color-adjust: exact; /*firefox & IE */
            }

            @page {
                size: auto;
                /*size: 21cm 29.7cm;*/
                margin: 2mm 7mm 2mm 10mm;
            }
        }

        body {
            font-size: 12px;
            padding: 0;
            margin: 0;
        }

        table, th, td {
            border: 1px solid black;
        }

        table {
            border-collapse: collapse;
            border: 1px solid black;
            width: 100%; /* width: 100%;
            position: absolute;
            top: 0px;
            bottom: 0px;
            margin: auto;
            margin-top: 0px !important;
            border: 1px solid;*/
        }

        table, th, td {
            border: 1px solid black;
        }

            table td b {
                padding: 0 0 0 5px;
            }

        .headerTables, .headerTables td {
            border: 0;
        }

        .mainTables tr {
            height: 20px;
            width: 90%;
        }

        .mainTables td div {
            height: 16px;
        }
    </style>
</head>
<body>
    <form id="formmm" runat="server" autopostback="false">
        <div class="printarea">


            <asp:Repeater ID="RepterDetails" runat="server">
                <ItemTemplate>
                    <div class="customerCopy">
                        <table class="headerTables" cellspacing="0" cellpadding="0" border="0">
                            <tr>
                                <td colspan="2" style="width: 80%;">
                                    <div style="float: left; width: 90px; padding-right: 20px">
                                        <img src="../images/new_logo.png" alt="." style="width: 80px; padding: 10px 0 10px 0px" />

                                    </div>
                                    <div style="float: left; line-height: 50px; font-weight: bold; font-size: 20px; margin-left: 0px">
                                        M&P Express Logistics (Pvt) Ltd. (Sales Tax Invoice) 
                                    </div>
                                </td>
                                <td style="text-align: center;">Customer Copy
                                </td>
                            </tr>
                        </table>
                        <table class="mainTables" cellspacing="0" cellpadding="0" style="border: 0;">
                            <tr>
                                <td style="vertical-align: top; width: 35%; border: 0;">
                                    <table class="InnerTables" cellspacing="0" cellpadding="0">
                                        <tr style="height: 20px;">
                                            <td style="text-align: center; font-weight: bold; background: #eee;">Shipment Information
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table border="0">
                                                    <tr>
                                                        <td style="width: 270px;" colspan="2">
                                                            <b>Tracking / CN #
                                                            <asp:Label ID="Label1" Text='<%#Eval("lbl_cn") %>' runat="server"></asp:Label>
                                                            </b>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>
                                                            <b>Ref No:</b>
                                                            <asp:Label ID="Label2" Text='<%#Eval("lbl_ref") %>' runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <b>Service:</b>
                                                            <asp:Label ID="Label3" Text='<%#Eval("lblService") %>' runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <b>Origin:</b>
                                                            <asp:Label ID="Label4" Text='<%#Eval("lbl_origin") %>' runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <b>DST:</b>
                                                            <asp:Label ID="Label5" Text='<%#Eval("lbl_dst") %>' runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3">
                                                            <b>Booking Date:</b>
                                                            <asp:Label ID="Label6" Text='<%#Eval("lbl_booking") %>' runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3">
                                                            <b>User:</b>
                                                            <asp:Label ID="Label7" Text='<%#Eval("lbl_user") %>' runat="server"></asp:Label>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td colspan="3">
                                                            <b>
                                                                <asp:Label ID="lbl_ntn1" Text='<%#Eval("lbl_ntn1") %>' runat="server"></asp:Label>
                                                            </b>0860540-8

                                                            <b>
                                                                <asp:Label ID="lbl_ridercode1" Text='<%#Eval("lbl_ridercode1") %>' runat="server"></asp:Label>
                                                            </b>
                                                        </td>
                                                    </tr>

                                                </table>


                                            </td>

                                        </tr>

                                        <tr>
                                            <td height="44">
                                                <b>Signature:</b>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td style="vertical-align: top; width: 40%; border: 0;">
                                    <table class="InnerTables" cellspacing="0" cellpadding="0">
                                        <tr style="height: 20px;">
                                            <td style="text-align: center; font-weight: bold; background: #eee;">Client Information
                                            </td>
                                        </tr>
                                        <tr style="height: 70px">
                                            <td style="vertical-align: top;">
                                                <b>To:</b>
                                                <asp:Label ID="Label8" Text='<%#Eval("lbl_consignee") %>' runat="server"></asp:Label><br />
                                                <b>Address:</b><asp:Label ID="Label9" Text='<%#Eval("lbl_consigneeaddress") %>' runat="server"></asp:Label><br />
                                                <b>Contact:</b><asp:Label ID="Label10" Text='<%#Eval("lbl_consigneephone") %>' runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr style="height: 70px">
                                            <td style="vertical-align: top;">
                                                <b>From:</b>
                                                <asp:Label ID="Label11" Text='<%#Eval("lbl_shipper") %>' runat="server"></asp:Label><br />
                                                <b>Address:</b>
                                                <asp:Label ID="Label12" Text='<%#Eval("lbl_shipperaddress") %>' runat="server"></asp:Label><br />
                                                <b>Contact:</b>
                                                <asp:Label ID="Label13" Text='<%#Eval("lbl_shippercontact") %>' runat="server"></asp:Label><br />
                                                <b>CNIC/NTN:</b>
                                                <asp:Label ID="lbl_cnic_ntn1" Text='<%#Eval("lbl_cnic_ntn1") %>' runat="server"></asp:Label>

                                            </td>
                                        </tr>
                                        <tr style="height: 45px;">
                                            <td style="vertical-align: top;">
                                                <b>Supplymentary:</b>
                                                <asp:Label ID="Label14" Text='<%#Eval("lbl_supplementary") %>' runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <b>Special Instructions:</b>
                                                <asp:Label ID="Label15" Text='<%#Eval("lblSpecialInstructions") %>' runat="server"></asp:Label>
                                            </td>
                                        </tr>

                                    </table>
                                </td>
                                <td style="vertical-align: top; width: 25%; border: 0;">
                                    <table class="InnerTables" cellspacing="0" cellpadding="0">
                                        <tr style="height: 20px;">
                                            <td style="text-align: center; font-weight: bold; background: #eee;">Price Information
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table>
                                                    <tr>
                                                        <td class="label">
                                                            <b>Pieces:</b>
                                                        </td>
                                                        <td class="label_text">
                                                            <asp:Label ID="Label16" Text='<%#Eval("lbl_Piece") %>' runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table>
                                                    <tr>
                                                        <td class="label">
                                                            <b>Charging Weight: </b>
                                                        </td>
                                                        <td class="label_text">
                                                            <asp:Label ID="Label17" Text='<%#Eval("lbl_weight") %>' runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="label">
                                                            <b>Dimension: </b>
                                                        </td>
                                                        <td class="label_text">
                                                            <asp:Label ID="Label18" Text='<%#Eval("lbl_w") %>' runat="server"></asp:Label>
                                                            X
                                        <asp:Label ID="Label19" Text='<%#Eval("lbl_b") %>' runat="server"></asp:Label>
                                                            X
                                        <asp:Label ID="Label20" Text='<%#Eval("lbl_h") %>' runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table>
                                                    <tr>
                                                        <td class="label">
                                                            <b>Val. Declared:</b>
                                                        </td>
                                                        <td class="label_text">
                                                            <asp:Label ID="Label21" Text='<%#Eval("lbl_val") %>' runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="label">
                                                            <b>Insurance:</b>
                                                        </td>
                                                        <td class="label_text">
                                                            <asp:Label ID="Label22" Text='<%#Eval("lbl_insurance") %>' runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table>
                                                    <tr>
                                                        <td class="label">
                                                            <b>Amount:</b>
                                                        </td>
                                                        <td class="label_text">
                                                            <asp:Label ID="Label23" Text='<%#Eval("lbl_amount") %>' runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="label">
                                                            <b>Discount:</b>
                                                        </td>
                                                        <td class="label_text">
                                                            <asp:Label ID="Label24" Text='<%#Eval("lbl_discount") %>' runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="label">
                                                            <b>
                                                                <asp:Label ID="lbl_gstpercentage1" Text='<%#Eval("lbl_gstpercentage1") %>' runat="server"></asp:Label></b>
                                                        </td>
                                                        <td class="label_text">
                                                            <asp:Label ID="Label25" Text='<%#Eval("lbl_gst") %>' runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="label">
                                                            <b>Net Amount (Rs.):</b>
                                                        </td>
                                                        <td class="label_text">
                                                            <asp:Label ID="Label26" Text='<%#Eval("lbl_chargedamount") %>' runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <b>Description:</b> ON SHIPPER RISK
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 33px;">
                                                <b>Package Contents:</b>
                                                <asp:Label ID="Label27" Text='<%#Eval("lblPackageContents") %>' runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <div style="font-weight: bold">Note: Please refer to our website for terms and conditions. <span style="padding-left: 200px">UAN:111-202-202 &nbsp;&nbsp; www.mulphilog.com</span></div>

                    </div>


                    <br />
                    <div class="newHr" style="border-top: 1px dashed  black;"></div>

                    <div class="customerCopy">
                        <table class="headerTables" cellspacing="0" cellpadding="0" border="0">
                            <tr>
                                <td colspan="2" style="width: 80%;">
                                    <div style="float: left; width: 90px; padding-right: 20px">
                                        <img src="../images/new_logo.png" alt="." style="width: 80px; padding: 10px 0 10px 0px" />

                                    </div>
                                    <div style="float: left; line-height: 50px; font-weight: bold; font-size: 20px; margin-left: 0px">
                                        M&P Express Logistics (Pvt) Ltd. (Sales Tax Invoice) 
                                    </div>
                                </td>
                                <td style="text-align: center;">Accounts Copy
                                </td>
                            </tr>
                        </table>
                        <table class="mainTables" cellspacing="0" cellpadding="0" style="border: 0;">
                            <tr>
                                <td style="vertical-align: top; width: 35%; border: 0;">
                                    <table class="InnerTables" cellspacing="0" cellpadding="0">
                                        <tr style="height: 20px;">
                                            <td style="text-align: center; font-weight: bold; background: #eee;">Shipment Information
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table border="0">
                                                    <tr>
                                                        <td style="width: 270px;" colspan="2">
                                                            <b>Tracking / CN #
                                                            <asp:Label ID="lbl_cn2" Text='<%#Eval("lbl_cn2") %>' runat="server"></asp:Label>
                                                            </b>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>
                                                            <b>Ref No:</b>
                                                            <asp:Label ID="lbl_ref2" Text='<%#Eval("lbl_ref2") %>' runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <b>Service:</b>
                                                            <asp:Label ID="lblService2" Text='<%#Eval("lblService2") %>' runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <b>Origin:</b>
                                                            <asp:Label ID="lbl_origin2" Text='<%#Eval("lbl_origin2") %>' runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <b>DST:</b>
                                                            <asp:Label ID="lbl_dst2" Text='<%#Eval("lbl_dst2") %>' runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3">
                                                            <b>Booking Date:</b>
                                                            <asp:Label ID="lbl_booking2" Text='<%#Eval("lbl_booking2") %>' runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3">
                                                            <b>User:</b>
                                                            <asp:Label ID="lbl_user2" Text='<%#Eval("lbl_user2") %>' runat="server"></asp:Label>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td colspan="3">
                                                            <b>
                                                                <asp:Label ID="lbl_ntn2" Text='<%#Eval("lbl_ntn2") %>' runat="server"></asp:Label>
                                                            </b>0860540-8
                                                            <b>
                                                                <asp:Label ID="lbl_ridercode2" Text='<%#Eval("lbl_ridercode2") %>' runat="server"></asp:Label>
                                                            </b>
                                                        </td>
                                                    </tr>

                                                </table>


                                            </td>

                                        </tr>

                                        <tr>
                                            <td height="44">
                                                <b>Signature:</b>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td style="vertical-align: top; width: 40%; border: 0;">
                                    <table class="InnerTables" cellspacing="0" cellpadding="0">
                                        <tr style="height: 20px;">
                                            <td style="text-align: center; font-weight: bold; background: #eee;">Client Information
                                            </td>
                                        </tr>
                                        <tr style="height: 70px">
                                            <td style="vertical-align: top;">
                                                <b>To:</b>
                                                <asp:Label ID="lbl_consignee2" Text='<%#Eval("lbl_consignee2") %>' runat="server"></asp:Label><br />
                                                <b>Address:</b><asp:Label ID="lbl_consigneeaddress2" Text='<%#Eval("lbl_consigneeaddress2") %>' runat="server"></asp:Label><br />
                                                <b>Contact:</b><asp:Label ID="lbl_consigneephone2" Text='<%#Eval("lbl_consigneephone2") %>' runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr style="height: 70px">
                                            <td style="vertical-align: top;">
                                                <b>From:</b>
                                                <asp:Label ID="lbl_shipper2" Text='<%#Eval("lbl_shipper2") %>' runat="server"></asp:Label><br />
                                                <b>Address:</b>
                                                <asp:Label ID="lbl_shipperaddress2" Text='<%#Eval("lbl_shipperaddress2") %>' runat="server"></asp:Label><br />
                                                <b>Contact:</b>
                                                <asp:Label ID="lbl_shippercontact2" Text='<%#Eval("lbl_shippercontact2") %>' runat="server"></asp:Label><br />
                                                <b>CNIC/NTN:</b>
                                                <asp:Label ID="lbl_cnic_ntn2" Text='<%#Eval("lbl_cnic_ntn2") %>' runat="server"></asp:Label>

                                            </td>
                                        </tr>
                                        <tr style="height: 45px;">
                                            <td style="vertical-align: top;">
                                                <b>Supplymentary:</b>
                                                <asp:Label ID="lbl_supplementary2" Text='<%#Eval("lbl_supplementary2") %>' runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <b>Special Instructions:</b>
                                                <asp:Label ID="lblSpecialInstructions2" Text='<%#Eval("lblSpecialInstructions2") %>' runat="server"></asp:Label>
                                            </td>
                                        </tr>

                                    </table>
                                </td>
                                <td style="vertical-align: top; width: 25%; border: 0;">
                                    <table class="InnerTables" cellspacing="0" cellpadding="0">
                                        <tr style="height: 20px;">
                                            <td style="text-align: center; font-weight: bold; background: #eee;">Price Information
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table>
                                                    <tr>
                                                        <td class="label">
                                                            <b>Pieces:</b>
                                                        </td>
                                                        <td class="label_text">
                                                            <asp:Label ID="lbl_Piece2" Text='<%#Eval("lbl_Piece2") %>' runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table>
                                                    <tr>
                                                        <td class="label">
                                                            <b>Charging Weight: </b>
                                                        </td>
                                                        <td class="label_text">
                                                            <asp:Label ID="lbl_weight2" Text='<%#Eval("lbl_weight2") %>' runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="label">
                                                            <b>Dimension: </b>
                                                        </td>
                                                        <td class="label_text">
                                                            <asp:Label ID="lbl_w2" Text='<%#Eval("lbl_w2") %>' runat="server"></asp:Label>
                                                            X
                                        <asp:Label ID="lbl_b2" Text='<%#Eval("lbl_b2") %>' runat="server"></asp:Label>
                                                            X
                                        <asp:Label ID="lbl_h2" Text='<%#Eval("lbl_h2") %>' runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table>
                                                    <tr>
                                                        <td class="label">
                                                            <b>Val. Declared:</b>
                                                        </td>
                                                        <td class="label_text">
                                                            <asp:Label ID="lbl_val2" Text='<%#Eval("lbl_val2") %>' runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="label">
                                                            <b>Insurance:</b>
                                                        </td>
                                                        <td class="label_text">
                                                            <asp:Label ID="lbl_insurance2" Text='<%#Eval("lbl_insurance2") %>' runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table>
                                                    <tr>
                                                        <td class="label">
                                                            <b>Amount:</b>
                                                        </td>
                                                        <td class="label_text">
                                                            <asp:Label ID="lbl_amount2" Text='<%#Eval("lbl_amount2") %>' runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="label">
                                                            <b>Discount:</b>
                                                        </td>
                                                        <td class="label_text">
                                                            <asp:Label ID="lbl_discount2" Text='<%#Eval("lbl_discount2") %>' runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="label">
                                                            <b>
                                                                <asp:Label ID="lbl_gstpercentage2" Text='<%#Eval("lbl_gstpercentage2") %>' runat="server"></asp:Label></b>
                                                        </td>
                                                        <td class="label_text">
                                                            <asp:Label ID="lbl_gst2" Text='<%#Eval("lbl_gst2") %>' runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="label">
                                                            <b>Net Amount (Rs.):</b>
                                                        </td>
                                                        <td class="label_text">
                                                            <asp:Label ID="lbl_chargedamount2" Text='<%#Eval("lbl_chargedamount2") %>' runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <b>Description:</b> ON SHIPPER RISK
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 33px;">
                                                <b>Package Contents:</b>
                                                <asp:Label ID="lblPackageContents2" Text='<%#Eval("lblPackageContents2") %>' runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <div style="font-weight: bold">Note: Please refer to our website for terms and conditions. <span style="padding-left: 200px">UAN:111-202-202 &nbsp;&nbsp; www.mulphilog.com</span></div>

                    </div>
                    <br />
                    <div class="" style="border: 1px dashed black;"></div>
                    <div style="margin-bottom: 60px"></div>

                </ItemTemplate>
            </asp:Repeater>
        </div>
    </form>
</body>
</html>
