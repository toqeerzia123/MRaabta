<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InvoicePDF_2.aspx.cs" Inherits="MRaabta.Files.InvoicePDF_2" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <p>
        <span style="color: rgb(0, 0, 0); font-family: verdana; font-size: 12px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: 400; letter-spacing: normal; orphans: 2; text-align: center; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; text-decoration-style: initial; text-decoration-color: initial; display: inline !important; float: none;">On Shipper&#39;s Risk</span>
    </p>
    <telerik:radscriptmanager id="rad1" runat="server">
                    </telerik:radscriptmanager>
    <telerik:radbarcode runat="server" id="RadBarcode1" type="Code11" height="50px" text="9780615159591"
        onprerender="RadBarcode1_PreRender">
    </telerik:radbarcode>
    <telerik:radbarcode runat="server" id="RadBarcode2" type="Code11" height="50px" text="9780615159591"
        onprerender="RadBarcode1_PreRender">
    </telerik:radbarcode>
    <asp:Panel ID="tbl_invoice" runat="server">
        <div style="vertical-align: top; position: relative; width: 100%; font-size: 12px; font-family: verdana; left: 0%;">
            <div class="left" style="float: left; height: 985px; position: relative; top: 14px; width: 35%; overflow: hidden">
                <div style="clear: both; text-align: right; float: left; width: 82%; position: relative; top: 10px;">
                    <asp:Label ID="prt_acc" runat="server"></asp:Label>
                </div>
                <div style="clear: both; text-align: right; float: left; width: 82%; position: relative; top: 29px;">
                    <asp:Label ID="prt_reff" runat="server"></asp:Label>
                </div>
                <div class="consigner" style="position: relative; top: 40px; height: 188px;">
                    <div style="clear: both; text-align: left; float: left; position: relative; top: 19px; padding: 0px 0px 10px; overflow: hidden; height: 9px;">
                        <asp:Label ID="prt_consigner" runat="server"></asp:Label>
                    </div>
                    <div style="clear: both; text-align: left; float: left; position: relative; top: 19px; padding: 0px 0px 10px; height: 57px; overflow: hidden;">
                        <asp:Label ID="prt_consigner_add" runat="server"></asp:Label>
                    </div>
                    <div style="clear: both; text-align: left; float: left; position: relative; top: 19px; padding: 0px 0px 10px;">
                        <asp:Label ID="prt_consigner_ph" runat="server"></asp:Label>
                    </div>
                    <div style="clear: both; text-align: left; float: left; position: relative; top: 19px;">
                        <asp:Label ID="prt_consigner_nic" runat="server"></asp:Label>
                    </div>
                </div>
                <div class="consignee" style="position: relative; top: 30px; height: 200px;">
                    <div style="clear: both; text-align: left; float: left; position: relative; top: 19px; padding: 0px 0px 10px; overflow: hidden; height: 9px;">
                        <asp:Label ID="prt_consignee" runat="server"></asp:Label>
                    </div>
                    <div style="clear: both; text-align: left; float: left; position: relative; top: 19px; padding: 0px 0px 10px; height: 57px; overflow: hidden;">
                        <asp:Label ID="prt_consignee_add" runat="server"></asp:Label>
                    </div>
                    <div style="clear: both; text-align: left; float: left; width: 82%; position: relative; top: 19px; padding: 0px 0px 10px;">
                        <asp:Label ID="prt_consignee_ph" runat="server"></asp:Label>
                    </div>
                    <div style="clear: both; text-align: left; float: left; width: 82%; position: relative; top: 19px;">
                        &nbsp;
                    </div>
                </div>
                <div class="con_acc" style="padding: 130px 0px 0px;">
                    <div style="clear: both; text-align: right; float: left; width: 82%; position: relative; top: 17px;">
                        <asp:Label ID="Label1" runat="server"></asp:Label>
                    </div>
                    <div style="clear: both; text-align: right; float: left; width: 82%; position: relative; top: 35px;">
                        <asp:Label ID="Label2" runat="server"></asp:Label>
                    </div>
                </div>
                <div class="cons_consigner" style="position: relative; top: 50px; height: 200px;">
                    <div style="clear: both; text-align: left; float: left; position: relative; top: 19px; padding: 0px 0px 10px; overflow: hidden; height: 9px;">
                        <asp:Label ID="Label3" runat="server"></asp:Label>
                    </div>
                    <div style="clear: both; text-align: left; float: left; position: relative; top: 19px; padding: 0px 0px 10px; height: 57px; overflow: hidden;">
                        <asp:Label ID="Label4" runat="server"></asp:Label>
                    </div>
                    <div style="clear: both; text-align: left; float: left; position: relative; top: 19px; padding: 0px 0px 10px;">
                        <asp:Label ID="Label5" runat="server"></asp:Label>
                    </div>
                    <div style="clear: both; text-align: left; float: left; position: relative; top: 19px;">
                        <asp:Label ID="Label6" runat="server"></asp:Label>
                    </div>
                </div>
                <div class="cons_consignee" style="position: relative; top: 35px; height: 200px;">
                    <div style="clear: both; text-align: left; float: left; position: relative; top: 19px; padding: 0px 0px 10px; overflow: hidden; height: 9px;">
                        <asp:Label ID="Label9" runat="server"></asp:Label>
                    </div>
                    <div style="clear: both; text-align: left; float: left; position: relative; top: 19px; padding: 0px 0px 10px; height: 57px; overflow: hidden;">
                        <asp:Label ID="Label8" runat="server"></asp:Label>
                    </div>
                    <div style="clear: both; text-align: left; float: left; position: relative; top: 19px; padding: 0px 0px 10px;">
                        <asp:Label ID="Label7" runat="server"></asp:Label>
                    </div>
                    <div style="clear: both; text-align: left; float: left; position: relative; top: 19px;">
                        &nbsp;
                    </div>
                </div>
            </div>
            <div class="mid" style="float: left; height: 985px; position: relative; width: 35%; top: 14px; overflow: hidden">
                <div class="barcode" style="height: 91px;">
                    <div style="float: left; width: 100%; text-align: center; height: 50px;">
                        <img id="img_1" runat="server" alt="" />
                    </div>
                    <div style="float: left; width: 100%; text-align: center;">
                        <asp:Label ID="prt_consignmentno" runat="server" Style="text-align: center;"></asp:Label>
                    </div>
                </div>
                <div class="package" style="height: 125px; overflow: hidden;">
                    <div style="float: left; width: 100%; text-align: center; position: relative; top: 29px;">
                        <asp:Label ID="prt_package" runat="server"></asp:Label>
                    </div>
                </div>
                <div class="null" style="height: 125px;">
                    <div style="float: left; width: 100%; text-align: center;">
                        <asp:Label ID="lbl_specialInstructions2" runat="server" Style="word-wrap: break-word"></asp:Label>
                    </div>
                    <div style="float: left; width: 100%; text-align: center;">
                        <asp:Label ID="lbl_insuranceMessage" runat="server"></asp:Label>
                    </div>
                </div>
                <div class="shipper" style="height: 100px;">
                    <div style="float: left; width: 100%; text-align: center; height: 50px;">
                        <asp:Label ID="lbl_shipperRisk" runat="server"></asp:Label>
                        <asp:Label ID="lbl_hps" runat="server" style="font-size: 10px;"></asp:Label>
                    </div>
                </div>
                <div class="booking_detail">
                    <div style="position: relative; left: 50px; width: 215px; top: -15px; font-size: 10px;">
                        <asp:Label ID="lbl_ecName" runat="server"></asp:Label>
                    </div>
                    <div style="position: relative; left: 100px; width: 130px; top: -40px; font-size: 10px;">
                        <asp:Label ID="prt_bookingdate" runat="server" Style="text-align: center;"></asp:Label>
                    </div>
                    <div style="position: relative; width: 100px; left: 50px; top: -75px; font-size: 10px;">
                        <asp:Label ID="prt_username" runat="server" Style="text-align: center;"></asp:Label>
                    </div>
                    <div style="position: relative; top: -91px; left: 150px; width: 140px; font-size: 10px;">
                        <asp:Label ID="ridercode" runat="server" Style="text-align: center;"></asp:Label>
                    </div>
                </div>
                <div class="con_barcode" style="height: 91px; padding: 80px 0px 0px;">
                    <div style="float: left; width: 100%; text-align: center; height: 50px; position: relative; top: -35px;">
                        <img id="Image1" runat="server" alt="" />
                    </div>
                    <div style="float: left; width: 100%; text-align: center; position: relative; top: -45px;">
                        <asp:Label ID="Label10" runat="server" Style="text-align: center;"></asp:Label>
                    </div>
                </div>
                <div class="con_package" style="height: 125px; overflow: hidden;">
                    <div style="float: left; width: 100%; text-align: center; position: relative; top: 29px;">
                        <asp:Label ID="Label11" runat="server"></asp:Label>
                    </div>
                </div>
                <div class="con_null" style="height: 85px;">
                    <div style="float: left; width: 100%; text-align: center;">
                        <asp:Label ID="lbl_specialInstructions" runat="server" Style="word-wrap: break-word"></asp:Label>
                    </div>
                    <div style="float: left; width: 100%; text-align: center;">
                        <asp:Label ID="lbl_insuranceMessage2" runat="server"></asp:Label>
                    </div>
                </div>
                <div class="con_shipper" style="height: 110px;">
                    <div style="float: left; width: 100%; text-align: center; height: 50px;">
                        <asp:Label ID="lbl_shipperRisk2" runat="server"></asp:Label>

                        <asp:Label ID="lbl_hps1" runat="server" style="font-size: 10px;"></asp:Label>
                    </div>
                </div>
                <div class="con_booking_detail" style="height: 100px;">
                    <div style="position: relative; left: 50px; width: 215px; top: -15px; font-size: 10px;">
                        <asp:Label ID="lbl_ecName2" runat="server"></asp:Label>
                    </div>
                    <div style="position: relative; left: 100px; width: 130px; top: -40px; font-size: 10px;">
                        <asp:Label ID="shi_bookingdate" runat="server" Style="text-align: center;"></asp:Label>
                    </div>
                    <div style="position: relative; width: 100px; left: 50px; top: -75px; font-size: 10px;">
                        <asp:Label ID="shi_username" runat="server" Style="text-align: center;"></asp:Label>
                    </div>
                    <div style="position: relative; top: -91px; left: 150px; width: 140px; font-size: 10px;">
                        <asp:Label ID="shi_ridername" runat="server" Style="text-align: center;"></asp:Label>
                    </div>
                </div>
            </div>
            <div class="right" style="float: left; height: 985px; position: relative; width: 25%; left: 30px; top: 14px; overflow: hidden; padding: 0px 0px 0px;">
                <div class="consigeer_right" style="float: left; height: 512px;">
                    <div class="city" style="float: left; width: 100%; position: relative; top: 45px;">
                        <div style="float: left; width: 50%; text-align: center;">
                            <asp:Label ID="prt_zone" runat="server"></asp:Label>
                        </div>
                        <div style="float: left; width: 50%; text-align: center;">
                            <asp:Label ID="prt_branch" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="pieces" style="float: left; width: 100%; position: relative; top: 70px;">
                        <div style="float: left; width: 50%; text-align: center;">
                            <asp:Label ID="prt_piecies" runat="server"></asp:Label>
                        </div>
                        <div style="float: left; width: 50%; text-align: center;">
                            <asp:Label ID="prt_weight" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="dimension" style="float: left; width: 100%; position: relative; top: 93px;">
                        <div style="float: left; width: 100%; text-align: center;">
                            0.00 X 0.00 X 0.00
                        </div>
                    </div>
                    <div class="servicetype" style="float: left; width: 100%; position: relative; top: 108px;">
                        <div style="float: left; width: 100%; text-align: center;">
                            <asp:Label ID="prt_servicetype" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="payment" style="float: left; width: 100%; position: relative; top: 125px;">
                        <div style="float: left; width: 100%; text-align: center;">
                            <asp:Label ID="prt_paymentmode" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="charges" style="float: left; width: 100%; position: relative; top: 160px;">
                        <div style="float: left; width: 50%; text-align: center;">
                            Service
                        </div>
                        <div style="float: left; width: 50%; text-align: center;">
                            <asp:Label ID="prt_servicecharge" runat="server"></asp:Label>
                        </div>
                        <div id="extraDiv" runat="server">
                        </div>
                        <div style="float: left; width: 50%; text-align: center;">
                            Discount
                        </div>
                        <div style="float: left; width: 50%; text-align: center;">
                            <asp:Label ID="prt_discountamount" runat="server"></asp:Label>
                        </div>
                        <div style="float: left; width: 50%; text-align: center;">
                            GST
                        </div>
                        <div style="float: left; width: 50%; text-align: center;">
                            <asp:Label ID="prt_gstamount" runat="server"></asp:Label>
                        </div>
                        <div style="float: left; width: 50%; text-align: center;">
                        </div>
                        <div style="float: left; width: 50%; text-align: center;">
                        </div>
                    </div>
                    <div class="totalamount" style="float: left; width: 100%; position: relative; top: 255px;">
                        <div style="float: left; width: 50%; height: 10px; text-align: center;">
                        </div>
                        <div style="float: left; width: 50%; height: 10px; text-align: center;">
                            <asp:Label ID="prt_totalamount" runat="server"></asp:Label>
                        </div>
                    </div>
                </div>
                <div class="consigee_right" style="padding: 20px 0 0; float: left;">
                    <div class="city" style="float: left; width: 100%; position: relative; top: 40px;">
                        <div style="float: left; width: 50%; text-align: center;">
                            <asp:Label ID="Label12" runat="server"></asp:Label>
                        </div>
                        <div style="float: left; width: 50%; text-align: center;">
                            <asp:Label ID="Label13" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="pieces" style="float: left; width: 100%; position: relative; top: 67px;">
                        <div style="float: left; width: 50%; text-align: center;">
                            <asp:Label ID="Label14" runat="server"></asp:Label>
                        </div>
                        <div style="float: left; width: 50%; text-align: center;">
                            <asp:Label ID="Label15" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="dimension" style="float: left; width: 100%; position: relative; top: 85px;">
                        <div style="float: left; width: 100%; text-align: center;">
                            0.00 X 0.00 X 0.00
                        </div>
                    </div>
                    <div class="servicetype" style="float: left; width: 100%; position: relative; top: 103px;">
                        <div style="float: left; width: 100%; text-align: center;">
                            <asp:Label ID="Label16" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="payment" style="float: left; width: 100%; position: relative; top: 122px;">
                        <div style="float: left; width: 100%; text-align: center;">
                            <asp:Label ID="Label17" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="charges" style="float: left; width: 100%; position: relative; top: 160px;">
                        <div style="float: left; width: 50%; text-align: center;">
                            Service
                        </div>
                        <div style="float: left; width: 50%; text-align: center;">
                            <asp:Label ID="Label18" runat="server"></asp:Label>
                        </div>
                        <div id="extraDiv1" runat="server">
                        </div>
                        <div style="float: left; width: 50%; text-align: center;">
                            Discount
                        </div>
                        <div style="float: left; width: 50%; text-align: center;">
                            <asp:Label ID="Label19" runat="server"></asp:Label>
                        </div>
                        <div style="float: left; width: 50%; text-align: center;">
                            GST
                        </div>
                        <div style="float: left; width: 50%; text-align: center;">
                            <asp:Label ID="Label20" runat="server"></asp:Label>
                        </div>
                        <div style="float: left; width: 50%; text-align: center;">
                        </div>
                        <div style="float: left; width: 50%; text-align: center;">
                        </div>
                    </div>
                    <div class="totalamount" style="float: left; width: 100%; position: relative; top: 250px;">
                        <div style="float: left; width: 50%; height: 10px; text-align: center;">
                        </div>
                        <div style="float: left; width: 50%; height: 10px; text-align: center;">
                            <asp:Label ID="Label21" runat="server"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
    <%--
    <asp:Panel ID="tbl_invoice" runat="server">

    <div style="vertical-align: top; position: relative; width: 100%; font-size: 12px; font-family: verdana; left: 0%;">

        <div class="left" style="float: left; height: 985px; position: relative; top: 14px; width: 35%; overflow:hidden">
        
            <div style="clear: both; text-align: right; float: left; width: 82%; position: relative; top: 10px;"><asp:Label ID="prt_acc" runat="server"></asp:Label></div>
            <div style="clear: both; text-align: right; float: left; width: 82%; position: relative; top: 29px;"><asp:Label ID="prt_reff" runat="server"></asp:Label></div>

            <div class="consigner" style="position: relative; top: 40px; height: 188px;">
                <div style="clear: both; text-align: left; float: left; position: relative; top: 19px; padding: 0px 0px 10px; overflow: hidden; height: 9px;"><asp:Label ID="prt_consigner" runat="server"></asp:Label></div>
                <div style="clear: both; text-align: left; float: left; position: relative; top: 19px; padding: 0px 0px 10px; height: 57px; overflow: hidden;"><asp:Label ID="prt_consigner_add" runat="server" ></asp:Label></div>
                <div style="clear: both; text-align: left; float: left; position: relative; top: 19px; padding: 0px 0px 10px;"><asp:Label ID="prt_consigner_ph" runat="server"></asp:Label></div>
                <div style="clear: both; text-align: left; float: left; position: relative; top: 19px;"><asp:Label ID="prt_consigner_nic" runat="server"></asp:Label></div>
            </div>

            <div class="consignee" style="position: relative; top: 30px; height: 200px;">
                <div style="clear: both; text-align: left; float: left; position: relative; top: 19px; padding: 0px 0px 10px; overflow: hidden; height: 9px;"><asp:Label ID="prt_consignee" runat="server"></asp:Label></div>
                <div style="clear: both; text-align: left; float: left; position: relative; top: 19px; padding: 0px 0px 10px; height: 57px; overflow: hidden;"><asp:Label ID="prt_consignee_add" runat="server"></asp:Label></div>
                <div style="clear: both; text-align: left; float: left; width: 82%; position: relative; top: 19px; padding: 0px 0px 10px;"><asp:Label ID="prt_consignee_ph" runat="server"></asp:Label></div>
                <div style="clear: both; text-align: left; float: left; width: 82%; position: relative; top: 19px;">&nbsp;</div>
            </div>

            <div class="con_acc" style="padding: 130px 0px 0px;">
                <div style="clear: both; text-align: right; float: left; width: 82%; position: relative; top: 17px;"><asp:Label ID="Label1" runat="server"></asp:Label></div>
                <div style="clear: both; text-align: right; float: left; width: 82%; position: relative; top: 35px;"><asp:Label ID="Label2" runat="server"></asp:Label></div>
            </div>

            <div class="cons_consigner" style="position: relative; top: 50px; height: 200px;">
                <div style="clear: both; text-align: left; float: left; position: relative; top: 19px; padding: 0px 0px 10px; overflow: hidden; height: 9px;"><asp:Label ID="Label3" runat="server"></asp:Label></div>
                <div style="clear: both; text-align: left; float: left; position: relative; top: 19px; padding: 0px 0px 10px; height: 57px; overflow: hidden;"><asp:Label ID="Label4" runat="server"></asp:Label> </div>
                <div style="clear: both; text-align: left; float: left; position: relative; top: 19px; padding: 0px 0px 10px;"><asp:Label ID="Label5" runat="server"></asp:Label></div>
                <div style="clear: both; text-align: left; float: left; position: relative; top: 19px;"><asp:Label ID="Label6" runat="server"></asp:Label></div>
            </div>            

            <div class="cons_consignee" style="position: relative; top: 35px; height: 200px;">
                <div style="clear: both; text-align: left; float: left; position: relative; top: 19px; padding: 0px 0px 10px; overflow: hidden; height: 9px;"><asp:Label ID="Label9" runat="server"></asp:Label></div>
                <div style="clear: both; text-align: left; float: left; position: relative; top: 19px; padding: 0px 0px 10px; height: 57px; overflow: hidden;"><asp:Label ID="Label8" runat="server"></asp:Label></div>
                <div style="clear: both; text-align: left; float: left; position: relative; top: 19px; padding: 0px 0px 10px;"><asp:Label ID="Label7" runat="server"></asp:Label></div>
                <div style="clear: both; text-align: left; float: left; position: relative; top: 19px;">&nbsp;</div>
            </div>
        
        </div>

        <div class="mid" style="float: left; height: 985px; position: relative; width: 35%; top: 14px; overflow:hidden">
            <div class="barcode" style="height: 91px;">
                <div style="float: left; width: 100%; text-align: center; height: 50px;"><img id="img_1" runat="server" alt="" /></div>
                <div style="float: left; width: 100%; text-align: center;"><asp:Label ID="prt_consignmentno" runat="server" Style="text-align: center;"></asp:Label></div>
            </div>

            <div class="package" style="height: 125px; overflow:hidden;">
                <div style="float: left; width: 100%; text-align: center; position: relative; top: 29px;"><asp:Label ID="prt_package" runat="server"></asp:Label></div>
            </div>

            <div class="null" style="height: 125px;">
                <div style="float: left; width: 100%; text-align: center;"></div>
            </div>

            <div class="shipper" style="height: 100px;">
                <div style="float: left; width: 100%; text-align: center; height: 50px;">On Shipper's Risk </div>
            </div>

            <div class="booking_detail">
                <div style="position: relative; left: 100px; width: 130px; top: -40px; font-size:10px;"><asp:Label ID="prt_bookingdate" runat="server" Style="text-align: center;"></asp:Label></div>
                <div style="position: relative; width: 100px; left: 50px; top: -75px; font-size:10px;"><asp:Label ID="prt_username" runat="server" Style="text-align: center;"></asp:Label> </div>
                <div style="position: relative; top: -91px; left: 150px; width: 140px; font-size:10px;"><asp:Label ID="ridercode" runat="server" Style="text-align: center;"></asp:Label> </div>
            </div>

            
            <div class="con_barcode" style="height: 91px; padding: 80px 0px 0px;">
                <div style="float: left; width: 100%; text-align: center; height: 50px;"><img id="Image1" runat="server" alt="" /></div>
                <div style="float: left; width: 100%; text-align: center;"><asp:Label ID="Label10" runat="server" Style="text-align: center;"></asp:Label></div>
            </div>

            <div class="con_package" style="height: 125px;overflow:hidden;">
                <div style="float: left; width: 100%; text-align: center; position: relative; top: 29px;"><asp:Label ID="Label11" runat="server"></asp:Label></div>
            </div>

            <div class="con_null" style="height: 125px;">
                <div style="float: left; width: 100%; text-align: center;"></div>
            </div>

            <div class="con_shipper" style="height: 100px;">
                <div style="float: left; width: 100%; text-align: center; height: 50px;">On Shipper's Risk </div>
            </div>

            <div class="con_booking_detail" style="height: 100px;">
                <div style="position: relative; left: 100px; width: 130px; top: -40px;font-size:10px;"><asp:Label ID="shi_bookingdate" runat="server" Style="text-align: center;"></asp:Label> </div>
                <div style="position: relative; width: 100px; left: 50px; top: -75px;font-size:10px;"><asp:Label ID="shi_username" runat="server" Style="text-align: center;"></asp:Label> </div>
                <div style="position: relative; top: -91px; left: 150px; width: 140px;font-size:10px;"><asp:Label ID="shi_ridername" runat="server" Style="text-align: center;"></asp:Label> </div>
            </div>
        
        
        </div>        

        <div class="right" style="float: left; height: 985px; position: relative; width: 25%; left: 30px; top: 14px; overflow: hidden; padding: 0px 0px 0px;">

            <div class="consigeer_right" style="float: left; height: 512px;">

                <div class="city" style="float: left; width: 100%; position: relative; top: 45px;">
                    <div style="float: left; width: 50%; text-align: center;"><asp:Label ID="prt_zone" runat="server"></asp:Label></div>
                    <div style="float: left; width: 50%; text-align: center;"><asp:Label ID="prt_branch" runat="server"></asp:Label></div>
                </div>

                <div class="pieces" style="float: left; width: 100%; position: relative; top: 70px;">
                    <div style="float: left; width: 50%; text-align: center;"><asp:Label ID="prt_piecies" runat="server"></asp:Label></div>
                    <div style="float: left; width: 50%; text-align: center;"><asp:Label ID="prt_weight" runat="server"></asp:Label></div>
                </div>

                <div class="dimension" style="float: left; width: 100%; position: relative; top: 93px;">
                    <div style="float: left; width: 100%; text-align: center;">0.00 X 0.00 X 0.00</div>
                </div>

                <div class="servicetype" style="float: left; width: 100%; position: relative; top: 108px;">
                    <div style="float: left; width: 100%; text-align: center;"><asp:Label ID="prt_servicetype" runat="server"></asp:Label></div>
                </div>

                <div class="payment" style="float: left; width: 100%; position: relative; top: 125px;">
                    <div style="float: left; width: 100%; text-align: center;"><asp:Label ID="prt_paymentmode" runat="server"></asp:Label></div>
                </div>

                <div class="charges" style="float: left; width: 100%; position: relative; top: 160px;">
                    <div style="float: left; width: 50%; text-align: center;">Service</div>
                    <div style="float: left; width: 50%; text-align: center;"><asp:Label ID="prt_servicecharge" runat="server"></asp:Label></div>
                    <div style="float: left; width: 50%; text-align: center;">Discount</div>
                    <div style="float: left; width: 50%; text-align: center;"><asp:Label ID="prt_discountamount" runat="server"></asp:Label></div>
                    <div style="float: left; width: 50%; text-align: center;">GST</div>
                    <div style="float: left; width: 50%; text-align: center;"><asp:Label ID="prt_gstamount" runat="server"></asp:Label></div>
                </div>

                <div class="totalamount" style="float: left; width: 100%; position: relative; top: 255px;">
                    <div style="float: left; width: 50%; height:10px; text-align: center;"></div>
                    <div style="float: left; width: 50%; height:10px; text-align: center;"><asp:Label ID="prt_totalamount" runat="server"></asp:Label></div>
                </div>

            </div>

            <div class="consigee_right" style="padding:20px 0 0; float:left; ">
            
                <div class="city" style="float: left; width: 100%; position: relative; top: 40px;">
                    <div style="float: left; width: 50%; text-align: center;"><asp:Label ID="Label12" runat="server"></asp:Label></div>
                    <div style="float: left; width: 50%; text-align: center;"><asp:Label ID="Label13" runat="server"></asp:Label></div>
                </div>

                <div class="pieces" style="float: left; width: 100%; position: relative; top: 67px;">
                    <div style="float: left; width: 50%; text-align: center;"><asp:Label ID="Label14" runat="server"></asp:Label></div>
                    <div style="float: left; width: 50%; text-align: center;"><asp:Label ID="Label15" runat="server"></asp:Label></div>
                </div>

                <div class="dimension" style="float: left; width: 100%; position: relative; top: 85px;">
                    <div style="float: left; width: 100%; text-align: center;">0.00 X 0.00 X 0.00</div>
                </div>

                <div class="servicetype" style="float: left; width: 100%; position: relative; top: 103px;">
                    <div style="float: left; width: 100%; text-align: center;"><asp:Label ID="Label16" runat="server"></asp:Label></div>
                </div>

                <div class="payment" style="float: left; width: 100%; position: relative; top: 122px;">
                    <div style="float: left; width: 100%; text-align: center;"><asp:Label ID="Label17" runat="server"></asp:Label></div>
                </div>

                <div class="charges" style="float: left; width: 100%; position: relative; top: 160px;">
                    <div style="float: left; width: 50%; text-align: center;">Service</div>
                    <div style="float: left; width: 50%; text-align: center;"><asp:Label ID="Label18" runat="server"></asp:Label></div>
                    <div style="float: left; width: 50%; text-align: center;">Discount</div>
                    <div style="float: left; width: 50%; text-align: center;"><asp:Label ID="Label19" runat="server"></asp:Label></div>
                    <div style="float: left; width: 50%; text-align: center;">GST</div>
                    <div style="float: left; width: 50%; text-align: center;"><asp:Label ID="Label20" runat="server"></asp:Label></div>
                </div>

                <div class="totalamount" style="float: left; width: 100%; position: relative; top: 250px;">
                    <div style="float: left; width: 50%; height:10px; text-align: center;"></div>
                    <div style="float: left; width: 50%; height:10px; text-align: center;"><asp:Label ID="Label21" runat="server"></asp:Label></div>
                </div>
            </div>
        
        </div>    

    </div>

    </asp:Panel>
    --%>
</body>
</html>
