<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="MnP_ConsignmentPrint.aspx.cs" Inherits="MRaabta.Files.MnP_ConsignmentPrint" %>

<%@ Register TagPrefix="Ajax1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script>
        function RiderChange() {
            var riderCode = document.getElementById('<%= txt_riderCode.ClientID %>');
            gridview = document.getElementById('<%= gv_consignments.ClientID %>');
            debugger;
            for (var i = 1; i < gridview.rows.length; i++) {
                row = gridview.rows[i];
                var gRider = row.cells[9].childNodes[1];
                var rider = gRider.value;
                var chk = row.cells[0].childNodes[1];
                if (chk.checked) {
                    gRider.value = riderCode.value;
                }

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

        function isNumberKeydouble(evt) {

            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                if (charCode == 46) {
                    return true;
                }
                return false;
            }
            return true;
        }
        function CheckAll(chk) {
            var grid = document.getElementById('<%= gv_consignments.ClientID %>');
            var check = chk.childNodes[0].checked;
            for (var i = 1; i < grid.rows.length; i++) {
                debugger;
                grid.rows[i].cells[0].childNodes[1].checked = check;
            }
        }
    </script>
    <asp:Label ID="Errorid" runat="server"></asp:Label>
    <table class="input-form" style="width: 98%;">
        <tr>
            <td class="field" style="width: 8% !important; text-align: right; padding-right: 10px;">
                Booking Date
            </td>
            <td class="input-field" style="width: 13% !important;">
                <asp:TextBox ID="txt_bookingDate" runat="server"></asp:TextBox>
                <Ajax1:CalendarExtender ID="calendar1" runat="server" Format="yyyy-MM-dd" TargetControlID="txt_bookingDate">
                </Ajax1:CalendarExtender>
            </td>
            <td class="space" style="margin: 0px !important; width:1% !important;">
            </td>
            <td class="field" style="width: 8% !important; text-align: right; padding-right: 10px;">
                Account No.
            </td>
            <td class="input-field" style="width: 13% !important;">
                <asp:TextBox ID="txt_accountNumber" runat="server"></asp:TextBox>
            </td>
            <td class="space" style="margin: 0px !important; width:1% !important;">
            </td>
            <td class="field" style="width: 8% !important; text-align: right; padding-right: 10px;">
                Gatepass #
            </td>
            <td class="input-field" style="width: 13% !important;">
                <asp:TextBox ID="txt_gatepass" runat="server"></asp:TextBox>
            </td>
            <td class="space" style="margin: 0px !important; width:1% !important;">
            </td>
            <td class="input-field" style="width: 25% !important; text-align:center;">
                <asp:Button ID="btn_search" runat="server" CssClass="button" Text="S E A R C H" Width="50%"
                    OnClick="btn_search_Click" />
            </td>
        </tr>
        <tr>
            <td class="field" style="width: 8% !important; text-align: right; padding-right: 10px;">
                Rider Code
            </td>
            <td class="input-field" style="width: 13% !important;">
                <asp:TextBox ID="txt_riderCode" runat="server" onchange="RiderChange()" onkeypress="return isNumberKey(event);"></asp:TextBox>
            </td>
            <td class="space" style="margin: 0px !important; width:5% !important;">
            </td>
            <td class="field" style="width: 10% !important; text-align: right; padding-right: 10px;">
            </td>
            <td class="input-field" style="width: 10% !important;">
            </td>
            <td class="space" style="margin: 0px !important; width:5% !important;">
            </td>
            <td class="field" style="width: 10% !important; text-align: right; padding-right: 10px;">
            </td>
            
        </tr>
        
    </table>
    <div style="width: 100%; height: 400px; overflow: scroll; text-align: center;">
        <span id="Table_1" class="tbl-large" style="width: 100%;">
            <asp:Label ID="err_msg" runat="server" CssClass="error_msg"></asp:Label>
            <asp:GridView ID="gv_consignments" runat="server" AutoGenerateColumns="false" CssClass="mGrid"
                AlternatingRowStyle-CssClass="alt" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px"
                Width="95%" EmptyDataText="No Data Available">
                <RowStyle Font-Bold="false" />
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:CheckBox ID="chk_print" runat="server" />
                        </ItemTemplate>
                        <HeaderTemplate>
                            <asp:CheckBox ID="chk_checkAll" runat="server" onchange="CheckAll(this);" />
                        </HeaderTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Consignment" DataField="consignmentNumber" />
                    <asp:BoundField HeaderText="Consigner" DataField="consigner" HeaderStyle-Width="15%" />
                    <asp:BoundField HeaderText="Consignee" DataField="consignee" HeaderStyle-Width="15%" />
                    <asp:BoundField HeaderText="Booking Date" DataField="bookingDate" />
                    <asp:BoundField HeaderText="Service Type" DataField="serviceTypeName" />
                    <asp:BoundField HeaderText="Origin" DataField="ORIGIN" />
                    <asp:BoundField HeaderText="Destination" DataField="Destination" />
                    <asp:BoundField HeaderText="TI" DataField="CouponNumber" />
                    <asp:TemplateField HeaderText="Rider" HeaderStyle-Width="5%">
                        <ItemTemplate>
                            <asp:TextBox ID="txt_gRider" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "riderCode") %>'
                                Enabled="false" Width="80%"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Pieces" HeaderStyle-Width="5%">
                        <ItemTemplate>
                            <asp:TextBox ID="txt_gPieces" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "pieces") %>'
                                Width="80%" onkeypress="return isNumberKey(event);"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Weight" HeaderStyle-Width="5%">
                        <ItemTemplate>
                            <asp:TextBox ID="txt_gWeight" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "weight") %>'
                                onkeypress="return isNumberKeydouble(event);" Width="80%"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </span>
    </div>
    <div style="width: 100%; text-align: center">
        <asp:Button ID="btn_reset" runat="server" Text="Reset" CssClass="button" OnClick="btn_reset_Click" />
        &nbsp;
        <asp:Button ID="btn_save" runat="server" CssClass="button" OnClick="btn_save_Click"
            Text="PRINT LABELS" />&nbsp;
        <asp:Button ID="btn_print" runat="server" CssClass="button" OnClick="btn_print_Click"
            Visible="false" Text="PRINT" />&nbsp;
        <asp:Button ID="sssss" runat="server" CssClass="button" Text="Generate LoadSheet"
            PostBackUrl="~/Files/LoadSheetGenerator.aspx" Visible="false" OnClick="sssss_Click" />
        <asp:Button ID="btn_printLoadSheet" runat="server" CssClass="button" Text="PRINT LOADSHEET"
            OnClick="btn_printLoadSheet_Click" />

      <asp:Button ID="Button1" runat="server" CssClass="button" Text="HTML Print"
            OnClick="btn_HTML_Click"  />
        <asp:Button ID="btn_ExportToCSV" runat="server" CssClass="button" Text="EXPORT CSV"
            OnClick="btn_ExportToCSV_Click" style="display:none"  /> 
     
        <%--OnClick="btn_printReconciliation_Click"--%>
    </div>
</asp:Content>
