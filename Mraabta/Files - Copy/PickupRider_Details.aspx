<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="PickupRider_Details.aspx.cs" Inherits="MRaabta.Files.PickupRider_Details" %>

<%@ Register TagPrefix="Ajax1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link rel="Stylesheet" href="../assets/bootstrap-4.3.1-dist/css/bootstrap.min.css" />
    <script type="text/javascript" src="../assets/js/jquery-1.11.0.min.js"></script>

    <style>
        input {
            padding: 3px;
        }

        tr.spaceUnder > td {
            padding-bottom: 6px;
        }
    </style>
    <script src="../Js/FusionCharts.js" type="text/javascript"></script>
    <div class="m-3" style="border: 1px solid black;">
        <fieldset class="m-2">
            <h1 style="text-align: left">Pickup Rider Details</h1>
            <hr style="background-color: black; height: 1px; margin-left: 30px; margin-right: 30px;" />
            <%--  <legend id="Legend5" visible="true">Pickup Rider Details</legend>--%>

            <asp:Label runat="server" ID="error_msg"> </asp:Label>

            <table class="table" style="margin-left: 10px; font-size: 12px; padding-bottom: 0px; width: 100%;">
                <tr class="spaceUnder mb-2" style="text-align: left;">
                    <td><b>Ticket Number</b></td>
                    <td>
                        <asp:Label ID="lbl_ticketNumber" runat="server"></asp:Label>
                    </td>
                    <td style="width: 15%; padding-left: 20px"><b>Consigner</b></td>
                    <td>
                        <asp:Label ID="Consignor" runat="server"></asp:Label>
                    </td>
                    <td style="width: 15%; padding-left: 20px"><b>Consigner Address</b></td>
                    <td>
                        <asp:Label ID="consignorAddress" runat="server"></asp:Label>
                    </td>
                    <td style=" padding-left: 20px"><b>Consigner Phone</b></td>
                    <td>
                        <asp:Label ID="consignorPhone" runat="server"></asp:Label>
                    </td>
                </tr>

                <tr class="spaceUnder mb-2" style="text-align: left;">

                    <td style="width: 10%;"><b>Consignee</b></td>
                    <td style="width: 10%">
                        <asp:Label ID="consignee" runat="server"></asp:Label>
                    </td>
                    <td style="width: 10%; padding-left: 20px"><b>Consignee Address</b></td>
                    <td style="width: 10%">
                        <asp:Label ID="consigneeAddress" runat="server"></asp:Label>
                    </td>
                    <td style="width: 10%; padding-left: 20px"><b>Consignee Phone</b></td>
                    <td style="width: 10%">
                        <asp:Label ID="ConsigneePhone" runat="server"></asp:Label>
                    </td>
                    <td style="padding-left: 20px"><b>Service Type</b></td>
                    <td>
                        <asp:Label ID="Service" runat="server"></asp:Label>
                    </td>

                </tr>

                <tr class="spaceUnder mb-2" style="text-align: left;">
                    <td><b>Origin</b></td>
                    <td>
                        <asp:Label ID="Origin" runat="server"></asp:Label>
                    </td>
                    <td style="padding-left: 20px"><b>Destination</b></td>
                    <td>
                        <asp:Label ID="Destination" runat="server"></asp:Label>
                    </td>
                    <td style="padding-left: 20px"><b>Pieces</b></td>
                    <td>
                        <asp:Label ID="Pieces" runat="server"></asp:Label>
                    </td>
                    <td style="padding-left: 20px"><b>Weight</b></td>
                    <td>
                        <asp:Label ID="Weight" runat="server"></asp:Label>
                    </td>

                </tr>

                <tr class="spaceUnder mb-2" style="text-align: left;">
                    <td><b>Package Contents</b></td>
                    <td>
                        <asp:Label ID="PackageContent" runat="server"></asp:Label>
                    </td>

                    <td style="padding-left: 20px"><b>GST</b></td>
                    <td>
                        <asp:Label ID="lblgst" runat="server"></asp:Label>
                    </td>
                    <td style="padding-left: 20px"><b>Gross Amount</b></td>
                    <td>
                        <asp:Label ID="lblGross" runat="server"></asp:Label>
                    </td>

                    <td style="padding-left: 20px"><b>Total Amount</b></td>
                    <td>
                        <asp:Label ID="total_Amount" runat="server"></asp:Label>
                    </td>

                </tr>

                <tr class="spaceUnder mb-2" style="text-align: left;">
                    <td ><b>Special Instructions</b></td>
                    <td>
                        <asp:Label ID="specialInstructions" runat="server"></asp:Label>
                    </td>
                    <td style="padding-left: 20px"><b>Rider Assigned</b></td>
                    <td>
                        <asp:Label ID="lblRider" runat="server"></asp:Label>
                    </td>

                </tr>
                <tr>
                    <td colspan="8">
                        <table style="border: 1px solid black">


                            <asp:Literal runat="server" ID="literalModifier"></asp:Literal>


                        </table>
                    </td>
                </tr>

                <tr class="spaceUnder mb-2" style="text-align: left;">

                    <td><b>Reason</b></td>
                    <td>
                        <asp:DropDownList runat="server" ID="ReasonList">
                           
                        </asp:DropDownList>
                    </td>

                    <td style="padding-left: 20px"><b>Remarks</b></td>
                    <td colspan="3">
                        <asp:TextBox ID="rem_txtbox" runat="server" Rows="4" Columns="40" TextMode="multiline" CssClass="form-control"></asp:TextBox>
                    </td>
                </tr>

                <tr class="spaceUnder mb-2" style="text-align: left;">
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td style="float: right;">
                        <asp:Button runat="server" ID="CancelBtn" Text="Cancel" CssClass="button" OnClick="CancelBtn_Click" /></td>
                    <td>
                        <asp:Button runat="server" ID="submit" Text="Submit" CssClass="button" OnClick="submitBtn_Click" /></td>
                </tr>
            </table>
        </fieldset>
    </div>

</asp:Content>
