<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master"  CodeBehind="VoidConsignment_Retail.aspx.cs" Inherits="MRaabta.Files.VoidConsignment_Retail" %>

<%@ Register TagPrefix="Ajax1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <style>
        .DetailTbl tr td {
        }

        .DetailTbl {
            border-spacing: 8px;
            border-collapse: separate;
        }
    </style>
    <table cellpadding="0" cellspacing="0" width='100%' class='mGrid_Table'>
        <tr>
            <td colspan="12" align="center" class="head_column">
                <h3 style="text-align: center">Void Consignment Retail
                </h3>
            </td>
        </tr>
    </table>
    <div>
        <table class="DetailTbl" cellpadding="10" cellspacing="10px">
            <tr>
                <td><b>Consignment Number: </b></td>
                <td>
                    <input type="text"   style="width:135px"   onkeypress=" return isNumberKey(event)" onchange="getConsignmentData()" id="ConsignmentTxtbox" /></td>
                <%-- <td style="width: 30%">
                    <button id="SubmitBtn"  type="button" class="button">Submit </button>
                </td>--%>
            </tr>
        </table>
        <fieldset style="border: solid; border-width: thin; height: auto; border-color: #a8a8a8;"
            class="">

            <legend id="Legend5" visible="true" style="width: auto; font-size: 14px; font-weight: bold; color: #1f497d;">Consignment Details</legend>

            <table class="DetailTbl">

                <tr>
                    <td><b>Booking Date</b></td>
                    <td>

                        <asp:TextBox ID="BookingDateTxtbox2"   style="width:135px" disabled="disabled"  runat="server" placeholder="Booking date" AutoPostBack="false"></asp:TextBox>
                         
                    </td>
                    <td><b>Account No</b></td>
                    <td>
                        <input id="AccountTxtBox" disabled="disabled" style="width:135px"  type="text" disabled="disabled" /></td>

                    <td><b>Consigner</b></td>
                    <td>
                        <input id="ConsignerTxtbox" disabled="disabled" style="width:135px"   type="text" /></td>

                    <td><b>Consignee</b></td>
                    <td>
                        <input id="ConsigneeTxtbox"  disabled="disabled" style="width:135px"   type="text" /></td>

                </tr>
                <tr>
                    <td><b>Origin</b></td>
                    <td>
                        <input id="OriginTxtbox" disabled="disabled"  style="width:135px"   type="text" />
                    </td>

                    <td><b>Destination</b></td>
                    <td>
                        <input id="Destinationtxtbox"  disabled="disabled" style="width:135px"   type="text" />
                    </td>

                    <td><b>Weight</b></td>
                    <td>
                        <input id="WeightTxtbox" disabled="disabled"  style="width:135px"   type="text" /></td>

                    <td><b>Pieces</b></td>
                    <td>
                        <input id="PiecesTxtbox" disabled="disabled"  style="width:135px"   type="text" /></td>

                </tr>
                <tr>
                    <td>

                        <button onclick="UpdateConsignment()" type="button" id="UpdateButton" class="button">Update</button></td>
                    

                </tr>
            </table>
        </fieldset>
    </div>


    <script type="text/javascript" src="../assets/js/jquery-1.11.0.min.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            $('#UpdateButton').attr('disabled', 'disabled');
            $('#UpdateButton').attr('style', 'background:  antiquewhite !important;');

        }); 

      
        function isNumberKeyWithDecimal(evt) {
            var status = false;
            var charCode = (evt.which) ? evt.which : event.keyCode

            if (charCode > 31 && (charCode < 48 || charCode > 57))
                status = false;
            if (charCode == 46)
                status = true;
            if (charCode > 47 && charCode < 58)
                status = true;
            return status;
        }

        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }
        function getConsignmentData() {
            
            var consignmentNum = document.getElementById('ConsignmentTxtbox').value;
            if (consignmentNum != "") {
                $.ajax({
                    async: false,
                    type: "POST",
                    data: JSON.stringify({ consignmentNum: consignmentNum }),
                    url: 'VoidConsignment_Retail.aspx/GetConsignmentData',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",

                    success: (rs) => {
                        if (rs.d.ConsignmentNum != "" && rs.d.ConsignmentNum!=null) {
                            document.getElementById('<%=BookingDateTxtbox2.ClientID %>').value = rs.d.BookingDate;
                            document.getElementById('AccountTxtBox').value = rs.d.AccountNo;
                            document.getElementById('ConsignerTxtbox').value = rs.d.Consigner;
                            document.getElementById('ConsigneeTxtbox').value = rs.d.Consignee;
                            document.getElementById('OriginTxtbox').value = rs.d.Origin;
                            document.getElementById('Destinationtxtbox').value = rs.d.Destination;
                            document.getElementById('WeightTxtbox').value = rs.d.Weight;
                            document.getElementById('PiecesTxtbox').value = rs.d.Pieces;
                     
                            //$("#OriginDDL").val(rs.d.OriginCode);
                            //$("#DestinationDDL").val(rs.d.DestinationCode);

                            $('#UpdateButton').attr('disabled', 'false');
                            $('#UpdateButton').attr('style', 'background:  #f27031   !important;');
               
                        
                            document.getElementById("UpdateButton").disabled = false;
                        } else {
                            $('#UpdateButton').attr('disabled', 'disabled');
                            $('#UpdateButton').attr('style', 'background:  antiquewhite !important;');
                           

                            document.getElementById('<%=BookingDateTxtbox2.ClientID %>').value ='';
                            document.getElementById('AccountTxtBox').value = '';
                            document.getElementById('ConsignerTxtbox').value = '';
                            document.getElementById('ConsigneeTxtbox').value = '';
                            //document.getElementById('OriginTxtbox').value = '';
                            //document.getElementById('Destinationtxtbox').value = '';
                            document.getElementById('WeightTxtbox').value = '';
                            document.getElementById('PiecesTxtbox').value = '';
                        }
                    }
                });
            }
        }
        function UpdateConsignment() {
            var consignmentNum = document.getElementById('ConsignmentTxtbox').value;
            if (consignmentNum != ""  ) {
            if (confirm('Are you sure?')) {
                    $.ajax({
                        async: false,
                        type: "POST",
                        data: JSON.stringify({ consignmentNum: consignmentNum }),
                        url: 'VoidConsignment_Retail.aspx/VoidConsignment',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                            alert(response.d);
                        },
                        error: function (response) {
                            alert("error updating consignment!") // I'm always get this.
                        }
                    });
                }
            }
        }

<%--        function UpdateConsignment() {
            var consignmentNum = document.getElementById('ConsignmentTxtbox').value;
            if (consignmentNum != "") {
            if (confirm('Are you sure?')) {
             
                    if ($('#<%=BookingDateTxtbox2.ClientID %>').val() == "") {
                        alert('Please provide booking date!');
                        return;
                    }
                    if ($("#AccountTxtBox").val() == "") {
                        alert('Please provide account!');
                        return;
                    }
                    if ($("#ConsignerTxtbox").val() == "") {
                        alert('Please provide consigner !');
                        return;
                    }
                    if ($("#ConsigneeTxtbox").val() == "") {
                        alert('Please provide consignee!');
                        return;
                    }
                    if ($("#OriginDDL").val() == "") {
                        alert('Please provide origin!');
                        return;
                    }
                    if ($("#DestinationDDL").val() == "") {
                        alert('Please provide destination!');
                        return;
                    }
                    if ($("#WeightTxtbox").val() == "") {
                        alert('Please provide weight!');
                        return;
                    }
                    if ($("#PiecesTxtbox").val() == "") {
                        alert('Please provide pieces!');
                        return;
                    }

                    
                    var Data = {
                        ConsignmentNum: $("#ConsignmentTxtbox").val(),
                        BookingDate: $('#<%=BookingDateTxtbox2.ClientID %>').val(),
                        AccountNo: $("#AccountTxtBox").val(),
                        Consigner: $("#ConsignerTxtbox").val(),
                        Consignee: $("#ConsigneeTxtbox").val(),
                        Origin: $("#OriginDDL").val(),
                        Destination: $("#DestinationDDL").val(),
                        Weight: $("#WeightTxtbox").val(),
                        Pieces: $("#PiecesTxtbox").val()
                    }
                    $.ajax({
                        async: false,
                        type: "POST",
                        data: JSON.stringify({ Data: Data }),
                        url: 'VoidConsignment_Retail.aspx/UpdateConsignment',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                            alert(response.d);
                        },
                        error: function (response) {
                            alert("error updating consignment!"); // I'm always get this.
                        }
                    });
                }
            }
        }--%>

        //$('#ConsignmentTxtbox').focus(function () {
        //    getConsignmentData();
        //});

    </script>

</asp:Content>