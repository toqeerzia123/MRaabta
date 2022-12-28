<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.master" CodeBehind="Manage_POD.aspx.cs" Inherits="MRaabta.Files.Manage_POD" %>
<%@ Register TagPrefix="Ajax1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        function ApplyAll() {

            debugger;
            var grid = document.getElementById("<%= gv_consignments.ClientID %>");
            var time = grid.rows[1].cells[5].childNodes[1].value;
            var receivedBy = grid.rows[1].cells[6].childNodes[1].value;
            var Status = grid.rows[1].cells[7].childNodes[1].value;
            var delvDate = grid.rows[1].cells[8].childNodes[1].get_selectedDate();
            var reason = grid.rows[1].cells[9].childNodes[1].value;
            var comments = grid.rows[1].cells[10].childNodes[1].value;
            for (var i = 1; i < grid.rows.length; i++) {

                grid.rows[i].cells[5].childNodes[1].value = time;
                grid.rows[i].cells[6].childNodes[1].value = receivedBy;
                grid.rows[i].cells[7].childNodes[1].value = Status;
                grid.rows[i].cells[8].childNodes[1].value = delvDate;
                grid.rows[i].cells[9].childNodes[1].value = reason;
                grid.rows[i].cells[10].childNodes[1].value = comments;
            }

        }


        function isNumberKey(evt) {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            return true;
        }
        function isNumber(evt) {

            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            return true;
        }
        function ChangeFlag(dd) {

            var grid = document.getElementById('<%= gv_consignments.ClientID %>');
            var tr = dd.parentNode.parentNode;

            var lbl_flag = tr.cells[6].children[1];

            //Existing Values

            var existingTime = tr.cells[6].children[11].innerText;
            var exstingReceivedBy = tr.cells[6].children[12].innerText;
            var existingReason = tr.cells[6].children[2].value;
            var existingCNIC = tr.cells[6].children[13].innerText;
            var existingRelation = tr.cells[6].children[10].value;

            //Changed Values
            var newTime = tr.cells[4].children[0].value;
            var newReceivedBy = tr.cells[5].children[0].value;
            var newReason = tr.cells[6].children[0].options[tr.cells[6].children[0].selectedIndex].value;
            var newCNIC = tr.cells[7].children[0].value;
            var newRelation = tr.cells[8].children[0].options[tr.cells[8].children[0].selectedIndex].value;

            //            var lbl_flag = tr.children[1];
            //            var existingReason = tr.children[2].value;
            //            var existingTime = tr.cells[4].children[0].innerText;


            if (existingTime != newTime || exstingReceivedBy.toUpperCase() != newReceivedBy.toUpperCase() || existingReason != newReason || existingCNIC != newCNIC || existingRelation != newRelation) {
                lbl_flag.value = "1";
            }
            else {
                lbl_flag.value = "0";
            }
        }
        function ChangeFocus(dd) {
            debugger;


            var grid = document.getElementById('<%= gv_consignments.ClientID %>');
            grid.rows[dd.parentNode.parentNode.rowIndex + 1].cells[4].childNodes[1].focus();
        }


        function SetVehicle(dd) {
            var txt = document.getElementById('<%= txt_vehicleNumber.ClientID %>');
            if (dd.options[dd.options.selectedIndex].value == "0") {
                alert('Select Vehicle');
                txt.value = "";
                var vehicleType = document.getElementById('<%= dd_vehicleType.ClientID %>');
                vehicleType.options[0].selected = true;

            }
            else {
                txt.value = dd.options[dd.options.selectedIndex].text;
                CheckVehicleType(dd);
            }

        }
        function CheckVehicleType(dd) {
            debugger;
            var vehicleGrid = document.getElementById('<%= vehicleTypes.ClientID %>');
            var vehicleType = document.getElementById('<%= dd_vehicleType.ClientID %>');
            var selectedVehicle = dd.options[dd.options.selectedIndex].value;

            for (var i = 0; i < vehicleGrid.rows.length; i++) {
                if (selectedVehicle == vehicleGrid.rows[i].cells[0].innerText) {
                    var selectedVehicleType = vehicleGrid.rows[i].cells[1].innerText;
                    for (var j = 0; j < vehicleType.options.length; j++) {
                        if (vehicleType.options[j].value == selectedVehicleType) {
                            vehicleType.options[j].selected = true;
                            break;
                        }
                    }
                }
            }

        }

        function DisplayLoader() {
            var loader = document.getElementById('loader');
            loader.style.display = 'block';
        }
        function HideLoader_(message) {
            debugger;
            var loader = document.getElementById('loader');
            loader.style.display = 'none';
            alert(message);
            return;

        }


    </script>
    <style type="text/css">
        .lds-ellipsis {
            display: inline-block;
            position: relative;
            width: 64px;
            height: 64px;
        }

            .lds-ellipsis div {
                position: absolute;
                top: 27px;
                width: 11px;
                height: 11px;
                border-radius: 50%;
                background: #fff;
                animation-timing-function: cubic-bezier(0, 1, 1, 0);
            }

                .lds-ellipsis div:nth-child(1) {
                    left: 6px;
                    animation: lds-ellipsis1 0.6s infinite;
                }

                .lds-ellipsis div:nth-child(2) {
                    left: 6px;
                    animation: lds-ellipsis2 0.6s infinite;
                }

                .lds-ellipsis div:nth-child(3) {
                    left: 26px;
                    animation: lds-ellipsis2 0.6s infinite;
                }

                .lds-ellipsis div:nth-child(4) {
                    left: 45px;
                    animation: lds-ellipsis3 0.6s infinite;
                }

        @keyframes lds-ellipsis1 {
            0% {
                transform: scale(0);
            }

            100% {
                transform: scale(1);
            }
        }

        @keyframes lds-ellipsis3 {
            0% {
                transform: scale(1);
            }

            100% {
                transform: scale(0);
            }
        }

        @keyframes lds-ellipsis2 {
            0% {
                transform: translate(0, 0);
            }

            100% {
                transform: translate(19px, 0);
            }
        }
    </style>
    <div id="loader" style="width: 86%; height: 150%; background-color: #000000; position: absolute; display: none; opacity: 0.5">
        <div style="margin-left: 0%; margin-top: 20%; text-align: center; font-weight: bold; font-size: x-large; color: White;">
            <div class="lds-ellipsis">
                <div>
                </div>
                <div>
                </div>
                <div>
                </div>
                <div>
                </div>
            </div>
        </div>
        <div style="margin-left: 0; text-align: center; font-weight: bold; font-size: x-large; color: White;">
            Please Wait...
        </div>
    </div>
    <div style="text-align: center; font-variant: small-caps; font-size: medium;">
        <asp:Label ID="Errorid" runat="server" Font-Bold="true"></asp:Label>
    </div>
    <table style="font-family: Calibri; font-size: medium; padding-bottom: 0px !important; padding-top: 0px !important; width: 97%"
        class="input-form">
        <tr style="padding-top: 15px !important;" id="tr_Branch" runat="server">
            <td class="field" style="width: 8% !important; text-align: right !important; padding-right: 5px;">Zone
            </td>
            <td class="input-field" style="width: 12% !important;">
                <asp:DropDownList ID="dd_zone" runat="server" AppendDataBoundItems="true" Width="100%"
                    Enabled="false" Height="100%" Font-Names="Calibri" Font-Size="Medium" AutoPostBack="true"
                    OnSelectedIndexChanged="dd_zone_SelectedIndexChanged">
                    <asp:ListItem Value="0">Select Zone</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;"></td>
            <td class="field" style="width: 10% !important; text-align: right !important; padding-right: 5px;">Branch
            </td>
            <td class="input-field" style="width: 12% !important;">
                <asp:DropDownList ID="dd_branch" runat="server" AppendDataBoundItems="true" Width="100%"
                    Height="100%" Font-Names="Calibri" Font-Size="Medium">
                    <asp:ListItem Value="0">Select Branch</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td class="space" style="width: 1% !important; margin: 0px 0px 0px 0px !important;"></td>
            <td class="field" style="width: 10% !important; text-align: right !important; padding-right: 5px;"></td>
            <td class="input-field" style="width: 12% !important;"></td>
            <td class="space" style="width: 33% !important; margin: 0px 0px 0px 0px !important;"></td>
        </tr>
        <tr style="padding-top: 15px !important;">
            <td class="field" style="width: 8% !important; text-align: right !important; padding-right: 5px;">Sheet No.
            </td>
            <td class="input-field" style="width: 12% !important;">
                <asp:TextBox ID="txt_runsheetNumber" runat="server"></asp:TextBox>
            </td>
            <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;"></td>
            <td class="field" style="width: 10% !important; text-align: right !important; padding-right: 5px;">Rider Code
            </td>
            <td class="input-field" style="width: 12% !important;">
                <asp:TextBox ID="txt_riderCode" runat="server"></asp:TextBox>
                <asp:HiddenField ID="hd_routeCode" runat="server" />
            </td>
            <td class="space" style="width: 1% !important; margin: 0px 0px 0px 0px !important;"></td>
            <td class="field" style="width: 10% !important; text-align: right !important; padding-right: 5px;">Rider Name.
            </td>
            <td class="input-field" style="width: 12% !important;">
                <asp:TextBox ID="txt_riderName" runat="server"></asp:TextBox>
            </td>
            <td class="space" style="width: 33% !important; margin: 0px 0px 0px 0px !important;"></td>
        </tr>
        <tr>
            <td class="field" style="width: 8% !important; text-align: right !important; padding-right: 5px;">Route
            </td>
            <td class="input-field" style="width: 12% !important;">
                <asp:TextBox ID="txt_route" runat="server"></asp:TextBox>
            </td>
            <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;"></td>
            <td class="field" style="width: 10% !important; text-align: right !important; padding-right: 5px;">RunSheet Date
            </td>
            <td class="input-field" style="width: 12% !important;">
                <telerik:RadDatePicker ID="txt_date" runat="server" DateInput-DateFormat="yyyy-MM-dd"
                    DateInput-EmptyMessage="Select Date" Enabled="false">
                </telerik:RadDatePicker>
            </td>
            <td class="space" style="width: 1% !important; margin: 0px 0px 0px 0px !important;"></td>
            <td class="field" style="width: 10% !important; text-align: right !important; padding-right: 5px;">POD Date
            </td>
            <td class="input-field" style="width: 12% !important;">
                <asp:TextBox ID="txt_podDate" runat="server"></asp:TextBox>
            </td>
            <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;"></td>
            <td class="input-field" style="width: 12% !important; text-align: left; padding-left: 0px !important;">
                <asp:CheckBox ID="chk_prePrinted" runat="server" Text="Pre Printed" Width="90%" />
            </td>
  <%--          <td class="input-field" style="width: 10% !important; text-align: right !important;">
                <asp:Label ID="lbl_count" runat="server" Text="Count:0" Width="90%"></asp:Label>
            </td>--%>
            <td class="space" style="width: 9% !important; margin: 0px 0px 0px 0px !important; text-align: right !important;"></td>
        </tr>
        <tr>
            <td class="field" style="width: 8% !important; text-align: right !important; padding-right: 5px;">Vehicle
            </td>
            <td class="input-field" style="width: 12% !important;">
                <asp:TextBox ID="txt_vehicleNumber" runat="server" Width="100%"></asp:TextBox>

            </td>
            <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;"></td>
            <td class="field" style="width: 10% !important; text-align: right !important; padding-right: 5px;"></td>
            <td class="input-field" style="width: 12% !important;">
                <%--<asp:DropDownList ID="dd_vehicle" runat="server" Width="100%" CssClass="dropdown"
                    onchange="SetVehicle(this)" AppendDataBoundItems="true">
                    <asp:ListItem Value="0">Select Vehicle</asp:ListItem>
                </asp:DropDownList>--%>
            </td>
            <td class="space" style="width: 1% !important; margin: 0px 0px 0px 0px !important;"></td>
            <td class="field" style="width: 10% !important; text-align: right !important; padding-right: 5px;">Vehicle Type
            </td>
            <td class="input-field" style="width: 12% !important;">
                <asp:DropDownList ID="dd_vehicleType" runat="server" CssClass="dropdown" AppendDataBoundItems="true">
                    <asp:ListItem Value="0">Select Vehicle Type</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;"></td>
            <td class="input-field" style="width: 12% !important; text-align: left; padding-left: 0px !important;"></td>
            <td class="input-field" style="width: 10% !important; text-align: right !important;"></td>

        </tr>
        <tr>
            <td class="field" style="width: 8% !important; text-align: right !important; padding-right: 5px;">Meter Start
            </td>
            <td class="input-field" style="width: 12% !important;">
                <asp:TextBox ID="txt_meterStart" runat="server" Width="100%"></asp:TextBox>

            </td>
            <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;"></td>
            <td class="field" style="width: 10% !important; text-align: right !important; padding-right: 5px;">Meter End
            </td>
            <td class="input-field" style="width: 12% !important;">
                <asp:TextBox ID="txt_meterEnd" runat="server"></asp:TextBox>
            </td>
            <td class="space" style="width: 1% !important; margin: 0px 0px 0px 0px !important;"></td>
            <td class="field" style="width: 10% !important; text-align: right !important; padding-right: 5px;"></td>
            <td class="input-field" style="width: 12% !important;"></td>
            <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;"></td>
            <td class="input-field" style="width: 12% !important; text-align: left; padding-left: 0px !important;"></td>
            <td class="input-field" style="width: 10% !important; text-align: right !important;"></td>
            <td class="space" style="width: 9% !important; margin: 0px 0px 0px 0px !important; text-align: right !important;"></td>
        </tr>
        <tr>
            <td class="input-field" style="width: 22% !important; text-align: left !important; padding-right: 5px;"
                colspan="3">
                <div style="width: 50px; height: 20px; background-color: #fc7b7b; display: inline; float: left; margin-right: 5px;">
                </div>
                Wrong Consignment
            </td>
            <td class="field" style="width: 10% !important; text-align: right !important; padding-right: 5px;"></td>
            <td class="input-field" style="width: 12% !important;"></td>
            <td class="space" style="width: 1% !important; margin: 0px 0px 0px 0px !important;"></td>
            <td class="field" style="width: 10% !important; text-align: right !important; padding-right: 5px;"></td>
            <td class="input-field" style="width: 12% !important;">
                <asp:Button ID="btn_search" runat="server" Text="Search Runsheet" CssClass="button"
                    OnClick="btn_search_Click" Width="187px" />
            </td>
            <td class="space" style="width: 2% !important; margin: 0px 0px 0px 0px !important;"></td>
            <td class="input-field" style="width: 12% !important; text-align: left; padding-left: 0px !important;"></td>
            <td class="input-field" style="width: 10% !important; text-align: right !important;"></td>

        </tr>
    </table>
    <div style="width: 100%; height: 400px; overflow: scroll; text-align: center;">
        <asp:LinkButton ID="lb_applyAll" runat="server" Text="Apply To All" Visible="false"
            OnClick="lb_applyAll_Click"></asp:LinkButton>
        <div>
            <asp:Label ID="err_msg" runat="server" CssClass="error_msg"></asp:Label>
        </div>
        <span id="Table_1" class="tbl-large" style="width: 100%;">


            <br />
            <asp:GridView ID="gv_consignments" runat="server" AutoGenerateColumns="false" CssClass="mGrid"
                AlternatingRowStyle-CssClass="alt" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px"
                Width="95%" EmptyDataText="No Data Available" OnRowDataBound="gv_consignments_RowDataBound" Style="position: absolute;">
                <RowStyle Font-Bold="false" />
                <Columns>
                    <%--<asp:TemplateField>
                        <ItemTemplate>
                            <asp:Button ID="btn_remove" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ConNo") %>'
                                CssClass="button" CommandName="Remove" Text="Remove" />
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                    <asp:BoundField HeaderText="SR#" DataField="SortOrder" />
                    <asp:BoundField HeaderText="Consignment" DataField="consignmentNumber" />
                    <asp:BoundField HeaderText="Consignee" DataField="consignee" />
                    <asp:BoundField HeaderText="Origin" DataField="ONAME" />
                    <asp:BoundField HeaderText="Pieces" DataField="Pieces" Visible="false" />
                    <asp:TemplateField HeaderText="Time">
                        <ItemTemplate>
                            <%--<asp:TextBox ID="txt_gTime" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "time") %>'></asp:TextBox>--%>
                            <asp:TextBox ID="txt_gTime" runat="server" Width="40px" ValidationGroup="vgrpUpdateTask"
                                onchange="ChangeFlag(this);" Style="overflow: hidden;"></asp:TextBox>
                            <Ajax1:MaskedEditExtender ID="MaskedEditExtender1" TargetControlID="txt_gTime" Mask="99:99"
                                ClearMaskOnLostFocus="false" MaskType="Time" CultureName="en-us" MessageValidatorTip="true"
                                runat="server">
                            </Ajax1:MaskedEditExtender>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Received By">
                        <ItemTemplate>
                            <asp:TextBox ID="txt_gReceivedBy" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "receivedBy") %>'
                                onchange="ChangeFlag(this);" Style="overflow: hidden;"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Status" Visible="false">
                        <ItemTemplate>
                            <asp:DropDownList ID="dd_gStatus" runat="server" runat="server" AppendDataBoundItems="true">
                                <asp:ListItem Value="0">Select Status</asp:ListItem>
                            </asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Delv Date" Visible="false">
                        <ItemTemplate>
                            <telerik:RadDatePicker ID="txt_gDelvDate" runat="server" DateInput-DateFormat="yyyy-MM-dd"
                                Width="100px" DateInput-EmptyMessage="Select Date">
                            </telerik:RadDatePicker>
                            <%--<asp:Label ID="Lbl_1" runat="server" Visible="false"></asp:Label>--%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Reason">
                        <ItemTemplate>
                            <%--<asp:TextBox ID="txt_gReason" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Reason") %>'></asp:TextBox>--%>
                            <asp:DropDownList ID="dd_gReason" runat="server" AppendDataBoundItems="true" Width="95%"
                                onchange="ChangeFlag(this);">
                                <asp:ListItem Value="0">Select Reason</asp:ListItem>
                            </asp:DropDownList>
                            <asp:TextBox ID="lbl_flg" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "flg") %>'
                                Style="display: none;"></asp:TextBox>
                            <asp:HiddenField ID="hd_reason" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "ReasonID") %>' />
                            <asp:HiddenField ID="hd_status" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "statusid") %>' />
                            <asp:HiddenField ID="hd_wrong" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "WrongCN") %>' />
                            <asp:HiddenField ID="hd_origin" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "Orgin") %>' />
                            <asp:HiddenField ID="hd_rtnBranch" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "rtnBranch") %>' />
                            <asp:HiddenField ID="hd_destination" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "destination") %>' />
                            <asp:HiddenField ID="hd_RTO" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "RTO") %>' />
                            <asp:HiddenField ID="hd_consigner" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "Consigner") %>' />
                            <asp:HiddenField ID="hd_consigneeCell" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "ConsigneePhoneNo") %>' />
                            <asp:HiddenField ID="hd_consignerCellNo" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "ConsignerCellNo") %>' />
                            <asp:HiddenField ID="hd_relation" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "RELATION") %>' />

                            <asp:Label ID="lbl_time" runat="server" Style="display: none;"></asp:Label>
                            <asp:Label ID="lbl_ReceivedBy" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "receivedBy") %>'
                                Style="display: none;"></asp:Label>
                            <asp:Label ID="lbl_CNIC" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Receiver_CNIC") %>'
                                Style="display: none;"></asp:Label>
                            <asp:HiddenField ID="hd_isPayable" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "isPayable") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Receiver CNIC">
                        <ItemTemplate>
                            <asp:TextBox ID="txt_receiverCNIC" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Receiver_CNIC") %>'
                                onchange="ChangeFlag(this);" MaxLength="13" onkeypress="return isNumberKey(event);"
                                Style="overflow: hidden;"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Receiver Relation">
                        <ItemTemplate>
                            <asp:DropDownList ID="dd_relation" runat="server" AppendDataBoundItems="true" onchange="ChangeFlag(this);"
                                onblur="return ChangeFocus(this);">
                                <asp:ListItem Value="0">Select Relation</asp:ListItem>
                            </asp:DropDownList>
                            <asp:HiddenField ID="hd_cod" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "COD") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Last Updated" DataField="ModifiedON" Visible="false" />
                    <asp:BoundField HeaderText="Given To Rider" DataField="GivenToRiderName" />
                    <asp:TemplateField HeaderText="Comments">
                        <ItemTemplate>
                            <asp:TextBox ID="txt_gComments" onchange="ChangeFlag(this);" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Comments") %>'
                                Style="overflow: hidden;"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </span>
        <asp:Button ID="btn_applyDefault" runat="server" Text="Apply Default Tariff" CssClass="button"
            Visible="false" />
    </div>
    <div style="width: 100%; text-align: center">
        <asp:Button ID="btn_reset" runat="server" Text="Reset" CssClass="button" OnClick="btn_reset_Click" />
        &nbsp;
        <asp:Button ID="btn_save" runat="server" Text="Save" CssClass="button" OnClick="btn_save_Click" OnClientClick="DisplayLoader();" />
    </div>
    <div style="display: none">
        <asp:GridView ID="vehicleTypes" runat="server" AutoGenerateColumns="false">
            <Columns>
                <asp:BoundField HeaderText="VehicleCode" DataField="VehicleCode" />
                <asp:BoundField HeaderText="TypeID" DataField="VehicleType_" />
            </Columns>
        </asp:GridView>
    </div>

    <script type="text/javascript">
        function HideLoader(message) {
            debugger;
            var loader = document.getElementById('loader');
            loader.style.display = 'none';
            alert(message);
            return;
        }
    </script>
</asp:Content>
