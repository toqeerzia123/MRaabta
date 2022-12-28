<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="ShipperAdvice.aspx.cs" Inherits="MRaabta.Files.ShipperAdvice" %>


<%@ Register TagPrefix="Ajax1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.7.1/jquery.min.js"></script>
    <style>
        .label
        {
            float: left;
            font-weight: bold;
            text-align: right;
            padding: 0 10px 0 0;
            line-height: 22px;
        }
        
        .field
        {
            float: left;
            width: 236px;
        }
        .filter
        {
            margin: 0 0 10px 5px;
            background: #eee;
            padding: 10px;
            width: 1074px;
        }
        .mGrid th
        {
            background: #f27031 !important;
        }
        .boxlabel
        {
            float: left;
            font-weight: bold;
            text-align: left;
            padding: 0 10px 0 0;
            line-height: 22px;
            width: 130px;
        }
        .boxfield
        {
            float: left;
            width: 300px;
            text-align: left;
        }
    </style>
    <style>
        /* Popup box BEGIN */
        .hover_bkgr_fricc
        {
            background: rgba(0,0,0,.4);
            cursor: pointer;
            display: none;
            height: 100%;
            position: fixed;
            text-align: center;
            top: 0;
            width: 100%;
            z-index: 10000;
        }
        .hover_bkgr_fricc .helper
        {
            display: inline-block;
            height: 100%;
            vertical-align: middle;
        }
        .hover_bkgr_fricc > div
        {
            /*background-color: #fff;
            box-shadow: 10px 10px 60px #555;
            display: inline-block;
            height: auto;
            max-width: 551px;
            min-height: 100px;
            vertical-align: middle;
            width: 60%;
            position: relative;
            border-radius: 8px;
            padding: 15px 5%;*/
            background-color: #fff;
            box-shadow: 10px 10px 60px #555;
            display: inline-block;
            height: auto;
            max-width: 67%;
            min-height: 100px;
            vertical-align: middle;
            width: 82%;
            position: relative;
            border-radius: 8px;
            padding: 15px 2%;
        }
        .popupCloseButton
        {
            background-color: #fff;
            border: 3px solid #999;
            border-radius: 50px;
            cursor: pointer;
            display: inline-block;
            font-family: arial;
            font-weight: bold;
            position: absolute;
            top: -20px;
            right: -20px;
            font-size: 25px;
            line-height: 30px;
            width: 30px;
            height: 30px;
            text-align: center;
        }
        .popupCloseButton:hover
        {
            background-color: #ccc;
        }
        .trigger_popup_fricc
        {
            cursor: pointer;
            color: #551A9D; /*font-size: 20px;
            margin: 20px;
            display: inline-block;
            font-weight: bold;*/
        }
        /* Popup box BEGIN */
    </style>
    <script language="javascript">
        function ClickCN(obj) {
            $.ajax({
                url: 'shipperadvice.aspx/GetConsignmentDetails',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                data: "{'consignment':'" + obj.textContent + "'}",
                success: function (result) {
                    debugger;
                    $('.hover_bkgr_fricc').show();

                    document.getElementById('lbl_cn').innerHTML = result.d[0].CN;
                    document.getElementById('lbl_requestid').innerHTML = result.d[0].RequestID;
                    document.getElementById('lbl_destination').innerHTML = result.d[0].Destination;
                    document.getElementById('lbl_consigneename').innerHTML = result.d[0].ConsigneeName;
                    document.getElementById('lbl_consigneeno').innerHTML = result.d[0].ConsigneeNo;
                    document.getElementById('lbl_consigneeaddress').innerHTML = result.d[0].ConsigneeAddress;
                    document.getElementById('lbl_reasonpending').innerHTML = result.d[0].ReasonPending;
                    document.getElementById('lbl_standardnotes').innerHTML = result.d[0].StandardNotes;
                    document.getElementById('lbl_callingstatus').innerHTML = result.d[0].CallingStatus;
                    document.getElementById('lbl_remark').innerHTML = result.d[0].Remark;
                    document.getElementById('lbl_requestdate').innerHTML = result.d[0].RequestDate;
                    document.getElementById('txt_contactno').value = result.d[0].ConsigneeNo;
                    document.getElementById('txt_address').value = result.d[0].ConsigneeAddress;

                    document.getElementById('ShipperName').value = result.d[0].ShipperName;
                }
            });
        }

        function SaveCN(obj) {
            debugger;

            var data_ = {
                RequestID: document.getElementById('lbl_requestid').innerHTML,
                Advice: document.getElementById('ContentPlaceHolder1_dd_advice').value,
                ConsigneeName: document.getElementById('lbl_consigneename').innerHTML,
                ConsigneeNo: document.getElementById('txt_contactno').value,
                ConsigneeAddress:document.getElementById('txt_address').value,
                Remark:document.getElementById('txt_remarks').value,
                CN:document.getElementById('lbl_cn').innerHTML,
                ReAttempt: document.getElementById('ContentPlaceHolder1_dd_reattempt').value
            };

            $.ajax({
                type: "POST",
                url: 'shipperadvice.aspx/SaveToDataBase',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ data_: data_ }),
                success: function (result) {
                    debugger;
                    $('.hover_bkgr_fricc').hide();
                    $("#msg").html("New record addded successfully  :)").css("color", "green");
                }
            });
        }

        function popupCloseButton() {
            $('.hover_bkgr_fricc').hide();
        }

        function isNumberKey(evt) {

            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {

                return false;
            }
            return true;
        }
    
    </script>
    <div>
        <label id="msg" ></label>
        <fieldset style="height: 398px;">
            <legend>Shipper Advice</legend>
            <div class="filter">
                <table>
                    <tr>
                        <td class="label">
                            Date
                        </td>
                        <td class="field">
                            <asp:TextBox ID="datepicker" runat="server" CssClass="med-field" MaxLength="10" Width="100px"></asp:TextBox>
                            <Ajax1:CalendarExtender ID="Calendar1" runat="server" TargetControlID="datepicker"
                                Format="dd/MM/yyyy">
                            </Ajax1:CalendarExtender>
                        </td>
                        <td class="label">
                            Consignment Number
                        </td>
                        <td class="field">
                            <asp:TextBox ID="txt_cn" runat="server" onkeypress="return isNumberKey(event);"></asp:TextBox>
                        </td>
                        <td class="label">
                            <asp:Button ID="btn_save" runat="server" Text="Search" CssClass="button" OnClick="btn_search_Click" />
                        </td>
                    </tr>
                </table>
            </div>
            <asp:Label ID="Errorid" runat="server" Font-Bold="true" Font-Size="Medium" ForeColor="Red"></asp:Label>
            <div>
                <asp:GridView ID="GV_Histroy" runat="server" AutoGenerateColumns="FALSE" CssClass="mGrid"
                    AlternatingRowStyle-CssClass="alt" BorderColor="#DEDFDE" BorderStyle="None" AllowPaging="TRUE"
                    PageSize="200" BorderWidth="1px" OnPageIndexChanging="GridView2_PageIndexChanging">
                    <Columns>
                        <asp:TemplateField HeaderText="S.No." ItemStyle-Width="2%">
                            <ItemTemplate>
                                <asp:Label ID="lblRowNumber" Text='<%# Container.DataItemIndex + 1 %>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="TicketNo" HeaderText="Pending Ref No" />
                        <asp:BoundField DataField="TicketDate" HeaderText="Pending Ref Date" />
                        <asp:TemplateField HeaderText="CN" ItemStyle-Width="10%">
                            <ItemTemplate>
                                <asp:HyperLink ID="lbl_CN" runat="server" CssClass="trigger_popup_fricc" onclick="ClickCN(this);"
                                    Text='<%# Bind("CONSIGNMENTNUMBER", "{0:N0}") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="BookingDate" HeaderText="Booking Date" />
                        <asp:BoundField DataField="DESTINATIONBRANCH" HeaderText="Destination" />
                        <%--<asp:BoundField DataField="Branch" HeaderText="Branch" />--%>
                        <asp:BoundField DataField="PendingReason" HeaderText="Pending Reason" />
                        <asp:BoundField DataField="StandardNote" HeaderText="Standard Note" />
                        <asp:BoundField DataField="CallStatus" HeaderText="Call Status" />
                        <asp:BoundField DataField="KPI" HeaderText="KPI" />
                        <asp:BoundField DataField="AdditionalRemarks" HeaderText="Additional Remarks" />
                    </Columns>
                </asp:GridView>
            </div>
        </fieldset>
    </div>
    <div class="hover_bkgr_fricc">
        <span class="helper"></span>
        <div>
            <div class="popupCloseButton" onclick="popupCloseButton()">
                &times;</div>
            <script type="text/javascript">

                function someFunction(ddl) {
                    if (ddl.value == 3) {
                        document.getElementById('ContentPlaceHolder1_div1').style.display = 'block';
                        document.getElementById('ContentPlaceHolder1_div2').style.display = 'block';
                        document.getElementById('ContentPlaceHolder1_div3').style.display = 'block';
                        document.getElementById('ContentPlaceHolder1_div4').style.display = 'block';
                    }
                    else {
                        document.getElementById('ContentPlaceHolder1_div1').style.display = 'none';
                        document.getElementById('ContentPlaceHolder1_div2').style.display = 'none';
                        document.getElementById('ContentPlaceHolder1_div3').style.display = 'none';
                        document.getElementById('ContentPlaceHolder1_div4').style.display = 'none';
                    }
                }

            </script>
            <table>
                <tr>
                    <td colspan="2">
                        <h2 style="margin: 0;border-bottom: 1px solid #ddd;text-transform: uppercase;;">
                            Track</h2>
                    </td>
                </tr>
                <tr>
                    <td class="boxlabel">
                        Request ID:
                    </td>
                    <td class="boxfield">
                        <label id="lbl_requestid" style="display: block">
                        </label>
                    </td>
                    <td class="boxlabel">
                        RequestDate:
                    </td>
                    <td class="boxfield">
                        <label id="lbl_requestdate" style="display: block">
                        </label>
                    </td>
                </tr>
                <tr>
                    <td class="boxlabel">
                        CN Number:
                    </td>
                    <td class="boxfield">
                        <label id="lbl_cn" style="display: block">
                        </label>
                    </td>
                    <td class="boxlabel">
                        Destination:
                    </td>
                    <td class="boxfield">
                        <label id="lbl_destination" style="display: block">
                        </label>
                    </td>
                </tr>
                <tr>
                    <td class="boxlabel">
                        Consignee Name:
                    </td>
                    <td class="boxfield">
                        <label id="lbl_consigneename" style="display: block">
                        </label>
                    </td>
                    <td class="boxlabel">
                        Consignee No:
                    </td>
                    <td class="boxfield">
                        <label id="lbl_consigneeno" style="display: block">
                        </label>
                    </td>
                </tr>
                <tr>
                    <td class="boxlabel">
                        Consignee Address:
                    </td>
                    <td class="boxfield">
                        <label id="lbl_consigneeaddress" style="display: block">
                        </label>
                    </td>
                </tr>
                <tr>
                    <td class="boxlabel">
                        Reason Pending:
                    </td>
                    <td class="boxfield">
                        <label id="lbl_reasonpending" style="display: block">
                        </label>
                    </td>
                    <td class="boxlabel">
                        Standard Notes:
                    </td>
                    <td class="boxfield">
                        <label id="lbl_standardnotes" style="display: block">
                        </label>
                    </td>
                </tr>
                <tr>
                    <td class="boxlabel">
                        Calling Status:
                    </td>
                    <td class="boxfield">
                        <label id="lbl_callingstatus" style="display: block">
                        </label>
                    </td>
                    <td class="boxlabel">
                        Remark:
                    </td>
                    <td class="boxfield">
                        <label id="lbl_remark" style="display: block">
                        </label>
                        <input id="hd_ShipperName" type="hidden" value="ShipperName" />
                        <input id="hd_AccountNo" type="hidden" />
                        <input id="hd_Origin" type="hidden" />
                        <input id="hd_DestinationCode" type="hidden" />
                        <input id="hd_Reason" type="hidden" />
                        <input id="hd_CallStatus" type="hidden" />
                        <input id="hd_StandardNotesCode" type="hidden" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <h2 style="margin: 0;border-bottom: 1px solid #ddd;text-transform: uppercase;">
                            Advice</h2>
                    </td>
                </tr>
                <tr>
                    <td class="boxlabel">
                        Advice
                    </td>
                    <td class="boxfield">
                        <asp:DropDownList ID="dd_advice" runat="server" AppendDataBoundItems="false" onchange="someFunction(this)">                            
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr id="div1" runat="server" style="display: none;float: left;width: 100%;">
                    <td class="boxlabel">
                        Re-Attempt Reason
                    </td>
                    <td class="boxfield">
                        <asp:DropDownList ID="dd_reattempt" runat="server" AppendDataBoundItems="false">                            
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr id="div2" runat="server" style="display: none;float: left;width: 100%;">
                    <td class="boxlabel">
                        Consignee Contact No
                    </td>
                    <td class="boxfield">
                        <%--<asp:TextBox ID="txt_contactno" runat="server" onkeypress="return isNumberKey(event);"></asp:TextBox>--%>
                        <input id="txt_contactno" type="text" />
                    </td>
                </tr>
                <tr id="div3" runat="server" style="display: none;float: left;width: 100%;">
                    <td class="boxlabel">
                        Consignee Address
                    </td>
                    <td class="boxfield">
                       <%-- <asp:TextBox ID="txt_address" runat="server" Rows="2" Columns="30" TextMode="MultiLine"></asp:TextBox>--%>
                        <textarea id="txt_address" rows="5" cols="50" ></textarea>
                    </td>
                </tr>
                <tr id="div4" runat="server" style="display: none;float: left;width: 100%;">
                    <td class="boxlabel">
                        Remarks
                    </td>
                    <td class="boxfield">
                        <%--<asp:TextBox ID="txt_remarks" runat="server" Rows="2" Columns="30" TextMode="MultiLine"></asp:TextBox>--%>
                        <%--<input id="txt_remarks" type="text" />--%>
                        <textarea id="txt_remarks" rows="5" cols="50"></textarea>
                    </td>
                </tr>
                <tr>
                    <td class="boxlabel">
                       <%-- <asp:Button ID="btn_cnsave" runat="server" Text="Save" CssClass="button"  onclick="SaveCN()" />--%>
                        <input type="button" class="button" value="Save" onclick="SaveCN(this)" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
