<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="Manage_Manifest.aspx.cs" Inherits="MRaabta.Files.Manage_Manifest" %>

<%@ Register TagPrefix="Ajax1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        function isNumberKey(evt) {

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



        function checkValidations(txt) {
            debugger;
            debugger;
            var txt_cn = document.getElementById('<%= txt_consignmentNo.ClientID %>');



            var dest = document.getElementById('<%= dd_destination.ClientID %>');
            if (dest.options[dest.options.selectedIndex].value == "0") {
                alert('Select Destination');
                return false;
            }
            var manifest = document.getElementById('<%= txt_manifestNo.ClientID %>');
            if (manifest.value == "") {
                alert('Enter Manifest Number');
                return false;
            }

            debugger;
            var controlGrid = document.getElementById('<%= cnControls.ClientID %>');
            var prefixNotFound = false;
            var message = "";
            for (var i = 1; i < controlGrid.rows.length; i++) {
                var row = controlGrid.rows[i];
                var prefix = row.cells[0].innerText;
                var length_ = parseInt(row.cells[1].innerText);


                if (txt.value.substring(0, prefix.length) == prefix) {
                    if (txt.value.length != length_) {
                        message = "Invalid Length of CN";
                        prefixNotFound = true;
                    }
                    else {


                        prefixNotFound = false;
                        break;
                    }
                }
                else {
                    if (message == "") {
                        message = "Invalid Prefix";
                    }
                    prefixNotFound = true;

                }
            }
            if (prefixNotFound) {
                alert(message);
                txt_cn.value = "";
                txt_cn.focus();
                return false;
            }

            return true;
        }
    </script>
    <asp:Label ID="ErrorID" runat="server" ForeColor="Red" Font-Bold="true" Font-Size="Medium"></asp:Label>
    <table style="font-family: Calibri; font-size: medium; padding-bottom: 0px !important; padding-top: 0px !important; width: 97% !important;"
        class="input-form">
        <tr style="padding: 0px 0px 0px 0px !important;">
            <td colspan="9" style="padding-bottom: 1px !important; padding-top: 1px !important;">
                <h4 style="font-family: Calibri; margin: 0px !important;">Manifest Info.</h4>
            </td>
        </tr>
        <tr>
            <td class="field" style="width: 10% !important;">Origin
            </td>
            <td class="input-field" style="width: 15% !important;">
                <asp:DropDownList ID="dd_origin" runat="server" AppendDataBoundItems="true" Width="100%">
                </asp:DropDownList>
            </td>
            <td class="space"></td>
            <td class="field" style="width: 10% !important;">Destination
            </td>
            <td class="input-field" style="width: 15% !important;">
                <asp:DropDownList ID="dd_destination" runat="server" AppendDataBoundItems="true"
                    Width="100%">
                    <asp:ListItem Value="0">Select Destination</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td class="space"></td>
            <td class="field" style="width: 10% !important;"></td>
            <td class="input-field" style="width: 20% !important;">
                <asp:RadioButtonList ID="rbtn_search" runat="server" RepeatDirection="Horizontal"
                    AutoPostBack="true" RepeatColumns="3" OnSelectedIndexChanged="rbtn_search_SelectedIndexChanged">
                    <asp:ListItem Value="1" Selected="True">New</asp:ListItem>
                    <asp:ListItem Value="2">View</asp:ListItem>
                    <asp:ListItem Value="3">Edit</asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <td class="field" style="width: 10% !important; text-align: left; vertical-align: top;">Service Type
            </td>
            <td class="input-field" style="width: 15% !important; text-align: left; vertical-align: top;">
                <asp:DropDownList ID="dd_serviceType" runat="server" AppendDataBoundItems="true">
                    <asp:ListItem Value="0">Select Service Type</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td class="space"></td>
            <td class="field" style="width: 10% !important;">Manifest No.
            </td>
            <td class="input-field" style="width: 15% !important;">
                <asp:TextBox ID="txt_manifestNo" runat="server" MaxLength="12" AutoPostBack="true"
                    OnTextChanged="txt_manifestNo_TextChanged"></asp:TextBox>
            </td>
            <td class="space"></td>
            <td class="field" style="width: 10% !important;">Date
            </td>
            <td class="input-field" style="width: 20% !important;">
                <asp:TextBox ID="txt_date" runat="server"></asp:TextBox>
                <Ajax1:CalendarExtender ID="calendarExtender1" runat="server" TargetControlID="txt_date"
                    Format="dd/MM/yyyy"></Ajax1:CalendarExtender>
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <table style="font-family: Calibri; font-size: medium; padding-bottom: 0px !important; padding-top: 0px !important; width: 97% !important;"
                class="input-form">
                <tr style="padding: 0px 0px 0px 0px !important;">
                    <td colspan="9" style="padding-bottom: 1px !important; padding-top: 1px !important;">
                        <h4 style="font-family: Calibri; margin: 0px !important;">Consignment Info.</h4>
                    </td>
                </tr>
                <tr>
                    <td class="field" style="width: 10% !important;">CN Number.
                    </td>
                    <td class="input-field" style="width: 15% !important;">
                        <asp:TextBox ID="txt_consignmentNo" runat="server" AutoPostBack="true" OnTextChanged="txt_consignmentNo_TextChanged"
                            onkeypress="return isNumberKey(event);" onchange="if ( checkValidations(this) == false ) return;"></asp:TextBox>
                        <%--onchange="if ( checkValidations(this) == false ) return;"--%>
                    </td>
                    <td class="space">Count: &nbsp;
                        <asp:Label ID="lbl_count" runat="server"></asp:Label>
                    </td>
                    <%--<td class="field" style="width: 10% !important;">
                Origin
            </td>
            <td class="input-field" style="width: 15% !important;">
                <asp:TextBox ID="txt_origin" runat="server"></asp:TextBox>
            </td>
            <td class="space">
            </td>
            <td class="field" style="width: 10% !important;">
                Destination
            </td>
            <td class="input-field" style="width: 15% !important;">
                <asp:TextBox ID="txt_destination" runat="server"></asp:TextBox>
            </td>--%>
                </tr>
                <%--<tr>
            <td class="field" style="width: 10% !important; text-align: left; vertical-align: top;">
                Service Type
            </td>
            <td class="input-field" style="width: 15% !important; text-align: left; vertical-align: top;">
                <asp:TextBox ID="txt_serviceType" runat="server"></asp:TextBox>
            </td>
            <td class="space">
            </td>
            <td class="field" style="width: 10% !important;">
                Weight
            </td>
            <td class="input-field" style="width: 15% !important;">
                
                <asp:TextBox ID="txt_weight" runat="server" MaxLength="4" Width="50%"></asp:TextBox>
                        
                
                <asp:CheckBox ID="chk_weight" runat="server" Width="40%" Text="Save" 
                    TextAlign="Right" oncheckedchanged="chk_weight_CheckedChanged"  AutoPostBack="true"/>
            </td>
            <td class="space">
            </td>
            <td class="field" style="width: 10% !important;">
                Pieces
            </td>
            <td class="input-field" style="width: 15% !important;">
                <asp:TextBox ID="txt_pieces" runat="server" Width="50%"></asp:TextBox>
                <asp:CheckBox ID="chk_pieces" runat="server" Width="40%" Text="Save" 
                    TextAlign="Right" oncheckedchanged="chk_pieces_CheckedChanged" AutoPostBack="true"/>
            </td>
        </tr>--%>
                <tr>
                    <td colspan="8"></td>
                </tr>
            </table>
            <div style="width: 100%; text-align: center">
                <asp:Button ID="Button1" runat="server" Text="Reset" CssClass="button" OnClick="btn_reset_Click"
                    UseSubmitBehavior="false" />
                &nbsp;
                <asp:Button ID="Button2" runat="server" Text="Save" CssClass="button" OnClick="btn_save_Click"
                    UseSubmitBehavior="false" />
                &nbsp;
                <asp:Button ID="Button3" runat="server" Text="Print Report" CssClass="button" Visible="false"
                    UseSubmitBehavior="false" OnClick="btn_print_Click" />
                &nbsp;
                <asp:Button ID="Button4" runat="server" Text="Cancel" CssClass="button" OnClick="btn_cancel_Click"
                    UseSubmitBehavior="false" />
            </div>
            <div style="width: 100%; height: 250px; overflow: scroll; text-align: center;">
                <span id="Table_1" class="tbl-large">
                    <asp:Label ID="err_msg" runat="server" CssClass="error_msg"></asp:Label>
                    <asp:GridView ID="gv_consignments" runat="server" AutoGenerateColumns="false" CssClass="mGrid"
                        AlternatingRowStyle-CssClass="alt" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px"
                        EmptyDataText="No Data Available" OnRowDataBound="gv_consignments_RowDataBound"
                        OnRowCommand="gv_consignments_RowCommand">
                        <Columns>
                            <asp:TemplateField HeaderStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Button ID="btn_remove" runat="server" Text="Remove" CssClass="button" CommandName="remove"
                                        CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ConsignmentNumber") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="ConsignmentNo" DataField="consignmentNumber" HeaderStyle-Width="10%" />
                            <asp:TemplateField HeaderText="Origin" HeaderStyle-Width="17.5%">
                                <ItemTemplate>
                                    <asp:DropDownList ID="dd_gorigin" runat="server" AppendDataBoundItems="true" AutoPostBack="true" Width="80%"
                                        OnSelectedIndexChanged="dd_gorigin_SelectedIndexChanged">
                                        <asp:ListItem Value="0">Select Origin</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:HiddenField ID="hd_origin" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "OriginName") %>' />
                                    <asp:HiddenField ID="hd_isModified" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "ISMODIFIED") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Destination" HeaderStyle-Width="17.5%">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_destination" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "DestinationName") %>'></asp:Label>
                                    <asp:HiddenField ID="hd_destination" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "Destination") %>' />
                                    <asp:HiddenField ID="hd_order" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "Order") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Con Type" HeaderStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:DropDownList ID="dd_contype" runat="server">
                                    </asp:DropDownList>
                                    <asp:HiddenField ID="hd_conType" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "ConsignmentTypeID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Service Type" HeaderStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:Label ID="hd_serviceType" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "serviceTypeName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Weight" HeaderStyle-Width="5%">
                                <ItemTemplate>
                                    <asp:TextBox ID="txt_gWeight" MaxLength="4" runat="server" Style="overflow: hidden !important; width: 80%;" Text='<%# DataBinder.Eval(Container.DataItem, "Weight") %>'
                                        CssClass="newbox" Width="50px"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Pieces" HeaderStyle-Width="5%">
                                <ItemTemplate>
                                    <asp:TextBox ID="txt_gPieces" runat="server" CssClass="newbox" Style="overflow: hidden !important; width: 80%;" Text='<%# DataBinder.Eval(Container.DataItem, "Pieces") %>'
                                        Width="50px"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-Width="5%" Visible="false">
                                <ItemTemplate>
                                    <asp:Button ID="btn_update" CommandName="Update" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ConsignmentNumber") %>'
                                        CssClass="button" Text="Edit" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </span>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div style="display: none;">
        <asp:GridView ID="cnControls" runat="server">
            <Columns>
                <asp:BoundField HeaderText="Prefix" DataField="Prefix" />
                <asp:BoundField HeaderText="Length" DataField="Length" />
            </Columns>
        </asp:GridView>
    </div>
    <%--<div style="width: 100%; height: 250px; overflow: scroll; text-align: center;">
        <span id="Table_1" class="tbl-large">
            <asp:Label ID="err_msg" runat="server" CssClass="error_msg"></asp:Label>
            <asp:GridView ID="gv_branches" runat="server" AutoGenerateColumns="false" CssClass="mGrid"
                AlternatingRowStyle-CssClass="alt" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px"
                EmptyDataText="No Data Available">
                <Columns>
                    <asp:BoundField HeaderText="Branch Name" DataField="Name" />
                    <asp:BoundField HeaderText="Short Name" DataField="sname" />
                    <asp:BoundField HeaderText="Description" DataField="Description" />
                    <asp:BoundField HeaderText="Email" DataField="Email" />
                    <asp:BoundField HeaderText="Phone No." DataField="PhoneNo" />
                    <asp:BoundField HeaderText="Fax No." DataField="FaxNO" />
                </Columns>
            </asp:GridView>
        </span>
        <asp:Button ID="btn_applyDefault" runat="server" Text="Apply Default Tariff" CssClass="button"
            Visible="false" />
    </div>--%>
    <div style="width: 100%; text-align: center">
        <asp:Button ID="btn_reset" runat="server" Text="Reset" CssClass="button" OnClick="btn_reset_Click"
            UseSubmitBehavior="false" />
        &nbsp;
        <asp:Button ID="btn_save" runat="server" Text="Save" CssClass="button" OnClick="btn_save_Click"
            UseSubmitBehavior="false" />
        &nbsp;
        <asp:Button ID="btn_print" runat="server" Text="Print Report" CssClass="button" Visible="false"
            UseSubmitBehavior="false" OnClick="btn_print_Click" />
        &nbsp;
        <asp:Button ID="btn_cancel" runat="server" Text="Cancel" CssClass="button" OnClick="btn_cancel_Click"
            UseSubmitBehavior="false" />
    </div>
</asp:Content>
