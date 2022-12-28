<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.master" CodeBehind="CardConsignmentWithoutData.aspx.cs" Inherits="MRaabta.Files.CardConsignmentWithoutData" %>

<%@ Register TagPrefix="Ajax1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        .tbl-large > div {
            height: 400px;
            overflow-y: scroll;
            width: 90%;
        }
    </style>
    <script type="text/javascript">
        function isNumberKey(evt) {
            debugger;

            var count = 1;
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode != 46 && charCode > 31
                && (charCode < 48 || charCode > 57 || charCode == 110 || charCode == 109 || charCode == 45 || charCode == 9)) {
                return false;
            }
            else {

                if (charCode == 110 || charCode == 46) {
                    count++;
                }
                if (count > 1) {
                    return false
                }
            }

            return true;
        }

        function checkRider(txt) {
            debugger;
            var rider = document.getElementById('<%= txt_riderCode.ClientID %>');
            if (rider.value.trim() == "") {
                alert('Enter Rider Code');
                txt.value = "";
                rider.focus();
                return false;

            }
            var txt_ = document.getElementById('<%= txt_consignment.ClientID %>');
            if (txt_.value.length > 20 || txt_.value.length < 11) {
                alert('Consignment Number must be between 11 and 20 digits');
                txt.value = "";
                txt.focus();
                return false;
            }
            return true;
        }
    </script>
    <script type="text/javascript" language="javascript">
        function checkQuote() {
            if (event.keyCode == 39) {
                event.keyCode = 0;
                return false;
            }
        }
    </script>
    <table cellpadding="0" cellspacing="0" width='100%' class='mGrid_Table'>
        <tr>
            <td colspan="12" align="center" class="head_column">
                <h3>Card Consignment Without Data
                </h3>
            </td>
        </tr>
    </table>
    <table style="font-family: Calibri; font-size: medium; padding-bottom: 0px !important; width: 1090px !important; padding-top: 15px !important"
        class="input-form">
        <tr>
            <td class="field" style="width: 10% !important; text-align: right !important; padding-right: 10px !important;">Destination
            </td>
            <td class="input-field" style="width: 15% !important; text-align: left !important;">
                <%--<asp:DropDownList ID="dd_destination" runat="server" Width="100%" AppendDataBoundItems="true">
                    <asp:ListItem Value="0">Select Destination</asp:ListItem>
                </asp:DropDownList>--%>
                <telerik:RadComboBox ID="dd_city" runat="server" Skin="Metro" OnSelectedIndexChanged="dd_cities_selectedIndexChanged"
                    AppendDataBoundItems="true" AllowCustomText="true" MarkFirstMatch="true">
                    <Items>
                        <telerik:RadComboBoxItem Value="0" Text="Select City" />
                    </Items>
                </telerik:RadComboBox>
                <asp:HiddenField ID="hd_Destination" runat="server" />
                <asp:HiddenField ID="hd_Destination_Ec" runat="server" />
                <asp:HiddenField ID="hd_DestinationZone" runat="server" />
            </td>
            <td class="space" style="margin: 0px 0px 0px 0px !important; width: 3.33% !important"></td>
            <td class="field" style="text-align: right ! important; padding-right: 10px ! important; width: 11% ! important;">Service Type
            </td>
            <td class="input-field" style="width: 15% !important; text-align: left !important;">
                <asp:DropDownList ID="dd_serviceType" runat="server" Width="100%">
                </asp:DropDownList>
            </td>
            <td class="space" style="margin: 0px 0px 0px 0px !important; width: 3.33% !important"></td>
            <td class="field" style="width: 12% !important; text-align: right !important; padding-right: 10px !important;">Date
            </td>
            <td class="input-field" style="width: 15% !important; text-align: left !important;">
                <asp:TextBox ID="dd_start_date" runat="server" CssClass="med-field" MaxLength="10"></asp:TextBox>
                <Ajax1:CalendarExtender ID="CalendarExtender3" TargetControlID="dd_start_date" runat="server"
                    Format="yyyy-MM-dd" PopupButtonID="Image1">
                </Ajax1:CalendarExtender>
            </td>
            <td class="space" style="margin: 0px 0px 0px 0px !important; width: 16.33% !important"></td>
        </tr>
        <tr>
            <td class="field" style="width: 10% !important; text-align: right !important; padding-right: 10px !important;">Account No
            </td>
            <td class="input-field" style="width: 15% !important; text-align: left !important;">
                <asp:TextBox ID="txt_accountNo" runat="server" Width="94%" AutoPostBack="true" OnTextChanged="txt_accountNo_TextChanged"></asp:TextBox>
            </td>
            <td class="space" style="margin: 0px 0px 0px 0px !important; width: 3.33% !important"></td>
            <td class="field" style="text-align: right ! important; padding-right: 10px ! important; width: 11% ! important;">Rider Code
            </td>
            <td class="input-field" style="width: 15% !important; text-align: left !important;">
                <asp:TextBox ID="txt_riderCode" runat="server" Width="50%" AutoPostBack="true" OnTextChanged="txt_riderCode_TextChanged"></asp:TextBox>
                <asp:HiddenField ID="hd_originEC" runat="server" />
            </td>
            <td class="space" style="margin: 0px 0px 0px 0px !important; width: 3.33% !important"></td>
            <td class="field" style="text-align: right ! important; padding-right: 10px ! important; width: 12% ! important;">Total
                Weight
            </td>
            <td class="input-field" style="width: 15% !important; text-align: left !important;">
                <asp:TextBox ID="txt_weight" Text="0.5" runat="server" Enabled="false"></asp:TextBox>
            </td>
            <td class="space" style="margin: 0px 0px 0px 0px !important; width: 16.33% !important"></td>
        </tr>
        <tr>
            <td class="field" style="width: 10% !important; text-align: right !important; padding-right: 10px !important;">Manifest No
            </td>
            <td class="input-field" style="width: 15% !important; text-align: left !important;">
                <asp:TextBox ID="txt_manifest" runat="server" Width="94%" AutoPostBack="true" MaxLength="50"
                    OnTextChanged="txt_manifest_TextChanged" onkeypress="return isNumberKey(event);"></asp:TextBox>
            </td>
            <td class="field" style="text-align: right ! important; padding-right: 10px ! important; width: 14.5% ! important;">Consignment No
            </td>
            <td class="input-field" style="width: 15% !important; text-align: left !important;">
                <asp:TextBox ID="txt_consignment" runat="server" Width="94%" MaxLength="50" AutoPostBack="true"
                    OnTextChanged="txt_consignment_TextChanged" onkeypress="return isNumberKey(event);"
                    onchange="if ( checkRider(this) == false ) return;"></asp:TextBox>
            </td>
            <td class="field" style="text-align: right ! important; padding-right: 10px ! important; width: 15.5% ! important;">Reference No
            </td>
            <td class="input-field" style="width: 16% !important; text-align: left !important;">
                <asp:TextBox ID="txt_referenceNo" runat="server" Width="64%" MaxLength="50" OnTextChanged="txt_referenceNo_TextChanged"
                    onchange="if ( checkRider(this) == false ) return;"></asp:TextBox>
                <asp:CheckBox ID="chk_refFreeze" Width="20%" runat="server" Text="Save" AutoPostBack="true"
                    OnCheckedChanged="chk_refFreeze_CheckedChanged" />
            </td>
            <td class="input-field" style="width: 15% !important; text-align: right !important;"></td>
            <td class="space" style="margin: 0px 0px 0px 0px !important; width: 16.33% !important"></td>
        </tr>
    </table>
    <table style="font-family: Calibri; font-size: medium; padding-bottom: 0px !important; width: 1090px !important; padding-top: 15px !important"
        class="input-form">
        <tr>
            <td class="field" style="text-align: right ! important; padding-right: 10px ! important; width: 14.5% ! important;">Consignee
            </td>
            <td class="input-field" style="width: 15% !important; text-align: left !important;">
                <asp:TextBox ID="txt_consignee" runat="server" Width="94%"></asp:TextBox>
            </td>
            <td class="field" style="text-align: right ! important; padding-right: 10px ! important; width: 14.5% ! important;">Address
            </td>
            <td class="input-field" style="width: 15% !important; text-align: left !important;">
                <asp:TextBox ID="txt_address" runat="server" Width="94%" AutoPostBack="true" OnTextChanged="txt_address_TextChanged" onkeypress="return checkQuote();"
                    onchange="if ( checkRider(this) == false ) return;"></asp:TextBox>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="hd_creditClientID" runat="server" />
    <asp:HiddenField ID="hd_Consigner" runat="server" />
    <%--</fieldset>--%>
    <div style="font-weight: bold; width: 30%; position: relative; float: left; top: -7px; text-align: left; left: 45px;">
        <asp:Literal ID="lbl_count" runat="server"></asp:Literal>
    </div>
    <span id="Span1" class="tbl-large">
        <asp:GridView ID="GridView" runat="server" AutoGenerateColumns="false" CssClass="mGrid"
            AlternatingRowStyle-CssClass="alt" BorderColor="#DEDFDE" BorderStyle="None" AllowPaging="false"
            BorderWidth="1px">
            <Columns>
                <asp:TemplateField HeaderText="S.No." ItemStyle-Width="2%">
                    <ItemTemplate>
                        <asp:Label ID="lblRowNumber" Text='<%# Container.DataItemIndex + 1 %>' runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="Manifest No" DataField="ManifestNo" ItemStyle-HorizontalAlign="Center"
                    ItemStyle-Width="10%"></asp:BoundField>
                <asp:BoundField HeaderText="Consignment No" DataField="ConsignmentNo" ItemStyle-HorizontalAlign="Center"
                    ItemStyle-Width="10%"></asp:BoundField>
                <asp:BoundField HeaderText="Reference No" DataField="referenceNo" ItemStyle-HorizontalAlign="Center"
                    ItemStyle-Width="10%"></asp:BoundField>
                <asp:BoundField HeaderText="Account No" DataField="AccountNo" ItemStyle-HorizontalAlign="Center"
                    ItemStyle-Width="10%"></asp:BoundField>
                <asp:BoundField HeaderText="Consigner" DataField="Consigner" ItemStyle-HorizontalAlign="Center"
                    ItemStyle-Width="10%"></asp:BoundField>
                <asp:BoundField HeaderText="Consignee" DataField="Consignee" ItemStyle-HorizontalAlign="Center"
                    ItemStyle-Width="10%"></asp:BoundField>
                <asp:BoundField HeaderText="Address" DataField="Address" ItemStyle-HorizontalAlign="Center"
                    ItemStyle-Width="10%"></asp:BoundField>
                <%-- <asp:BoundField HeaderText="Credit Client ID" DataField="CreditClientID" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%"></asp:BoundField>
                <asp:BoundField HeaderText="Origin" DataField="Origin" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%"></asp:BoundField>
                <asp:BoundField HeaderText="Destination" DataField="Destination" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%"></asp:BoundField>--%>
                <asp:BoundField HeaderText="Service Type" DataField="ServiceType" ItemStyle-HorizontalAlign="Center"
                    ItemStyle-Width="10%"></asp:BoundField>
                <asp:BoundField HeaderText="Rider Code" DataField="RiderCode" ItemStyle-HorizontalAlign="Center"
                    ItemStyle-Width="10%"></asp:BoundField>
                <asp:BoundField HeaderText="Weight" DataField="Weight" ItemStyle-HorizontalAlign="Center"
                    ItemStyle-Width="10%"></asp:BoundField>
                <asp:TemplateField HeaderText="" ItemStyle-Width="2%">
                    <ItemTemplate>
                        <asp:Button ID="btn_removeConsignment" Text="Remove" OnClick="btn_removeConsignment_Click" runat="server" CssClass="button1" BackColor="#f27031" />
                        <asp:HiddenField Value='<%# Eval("ConsignmentNo") %>' runat="server" ID="hd_ConsignmentNo" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </span>
    <div style="position: relative; width: 50px; left: 15px; margin: 0 0 10px;">
        <asp:Button ID="Button1" runat="server" Text="Save" CssClass="button1" OnClick="Btn_Save_Click"
            UseSubmitBehavior="false" />
    </div>
</asp:Content>
