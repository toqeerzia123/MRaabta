<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="SearchConsignment.aspx.cs" Inherits="MRaabta.Files.SearchConsignment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="mainTable">
        <table width="100%">
            <tr>
                <td>
                    <asp:Label ID="lbl_Error" runat="server" ForeColor="Red"></asp:Label>
                </td>
            </tr>
            <tr>
                <td rowspan="12" style="width: 30%">
                    <fieldset>
                        <legend>Consignment Info</legend>
                        <table>
                            <tr>
                                <td>
                                    <b>Consignment No.</b>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_consignmentNumber" runat="server" OnTextChanged="txt_consignmentNumber_TextChanged"
                                        AutoPostBack="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <b>Account No.</b>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_accountNo" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <b>Rider Code.</b>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lbl_riderCode"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <b>Con. Type</b>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_consignmentType" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <b>City</b>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_city" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <b>Destination</b>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_destination" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <b>Weight</b>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_weight" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <b>Service Type</b>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_serviceType" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <b>Discount</b>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_discount" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <b>Pieces</b>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_pieces" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <br />
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </td>
                <td rowspan="9" style="width: 50%">
                    <fieldset>
                        <legend>Consignee & Consigner Info</legend>
                        <table>
                            <tr>
                                <td>
                                    <b>Consignee</b>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_consignee" runat="server"></asp:Label>
                                </td>
                                <td colspan="2" rowspan="9">
                                    <table>
                                        <tr>
                                            <td>
                                                <b>Booking Date</b>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_bookingDate" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <b>Origin</b>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_origin" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <b>Origin Express Center</b>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_originExpressCenter" runat="server">
                                                </asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <b>Insurance</b>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_insurance" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <b>Total Charges</b>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_totalCharges" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <b>Gst Charges</b>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_gstCharges" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <b>Total Amount</b>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_totalAmount" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:Label ID="lbl_dayType" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <b>Consignee CellNo</b>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_consigneeCell" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <b>Consignee CNIC</b>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_consigneeCNIC" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <b>Consigner</b>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_consigner" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <b>Consigner CellNo</b>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_consignerCell" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <b>Consigner CNIC</b>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_consignerCNIC" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_Type" runat="server">Coupon No</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_coupon" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="1">
                                </td>
                                <td colspan="1">
                                    <asp:CheckBox ID="cb_COD" runat="server" Text="COD" Enabled="False" />
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:CheckBox ID="cb_Insurance" runat="server" Text="Insurance" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <b>Declared Values</b>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_declaredValue" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <b>Package/Handcarry</b>
                                </td>
                                <td colspan="3">
                                    <asp:Label ID="lbl_packageContents" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <b>Address</b>
                                </td>
                                <td colspan="3">
                                    <asp:Label ID="lbl_consigneeAddress" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <b>Shipper Address</b>
                                </td>
                                <td colspan="3">
                                    <asp:Label ID="lbl_consignerAddress" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </td>
                <%--<td rowspan="9" style="width: 20%">
                            <fieldset>
                                <legend>Config Info</legend>
                                <table>
                                    <tr>
                                        <td colspan="2">
                                            First Con. No :
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <telerik:radtextbox id="txt_LblFirstCon" runat="server" skin="Web20" maxlength="100"
                                                autopostback="True" labelwidth="64px" resize="None" resolvedrendermode="Classic"
                                                width="160px" text="0" enabled="false">
                                                <EnabledStyle Font-Names="Verdana" HorizontalAlign="Right" />
                                            </telerik:radtextbox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            Last Con. No :
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <telerik:radtextbox id="txt_LblLastConsignment" runat="server" skin="Web20" maxlength="100"
                                                autopostback="True" labelwidth="64px" resize="None" resolvedrendermode="Classic"
                                                text="0" width="160px" enabled="false">
                                                <EnabledStyle Font-Names="Verdana" HorizontalAlign="Right" />
                                            </telerik:radtextbox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            User Behaviour :
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:RadioButtonList ID="Rb_UserBehaviour" runat="server" RepeatDirection="Horizontal"
                                                Enabled="false">
                                                <asp:ListItem Text="Exp. Cen" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="Hub"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="style1">
                                            Customer Type :
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:RadioButtonList ID="Rb_CustomerType" runat="server" Enabled="false" RepeatDirection="Horizontal">
                                                <asp:ListItem Text="Credit" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="Cash"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            Customer Entry
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:RadioButtonList ID="rb_CustomerEntry" runat="server" RepeatDirection="Horizontal"
                                                Enabled="False">
                                                <asp:ListItem Text="Normal" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="Bulk"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:CheckBox ID="txt_Preprinted" runat="server" Text="Pre Printed Report"></asp:CheckBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:CheckBox ID="txt_SmsConsignment" runat="server" Text="SMS Consignment"></asp:CheckBox>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>--%>
            </tr>
        </table>
        <fieldset>
            <legend>COD Info</legend>
            <table width="100%">
                <tr>
                    <td>
                        <b>Order Ref. No.</b>
                    </td>
                    <td>
                        <asp:Label ID="lbl_orderRef" runat="server"></asp:Label>
                    </td>
                    <td>
                        <b>Product Type</b>
                    </td>
                    <td>
                        <asp:Label ID="lbl_productType" runat="server"></asp:Label>
                    </td>
                    <td>
                        <b>Charge COD Amount.</b>
                    </td>
                    <td>
                        <asp:CheckBox ID="Cb_CODAmount" runat="server" AutoPostBack="true" Enabled="false" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>Description</b>
                    </td>
                    <td colspan="3">
                        <asp:Label ID="lbl_descriptionCOD" runat="server"></asp:Label>
                    </td>
                    <td>
                        <b>COD Amount</b>
                    </td>
                    <td>
                        <asp:Label ID="lbl_CODAmount" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
        </fieldset>
        <fieldset>
            <legend>Price Modifiers</legend>
            <table class="style2">
                <tr>
                    <td colspan="6">
                        <asp:Repeater ID="RadGrid1" runat="server">
                            <HeaderTemplate>
                                <table>
                                    <tr>
                                        <th>
                                            <b>Price Modifier</b>
                                        </th>
                                        <th>
                                            <b>Value</b>
                                        </th>
                                        <th>
                                            <b>Calculation Base</b>
                                        </th>
                                        <th>
                                            <b>Description</b>
                                        </th>
                                    </tr>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <%# DataBinder.Eval(Container.DataItem, "name")%>
                                        <asp:HiddenField ID="Hd_ID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "name")%>' />
                                    </td>
                                    <td>
                                        <%# DataBinder.Eval(Container.DataItem, "calculationBase")%>
                                    </td>
                                    <td>
                                        <%# DataBinder.Eval(Container.DataItem, "modifiedCalculationValue")%>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_Description" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "description") %>'></asp:Label>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                </table>
                            </FooterTemplate>
                        </asp:Repeater>
                    </td>
                </tr>
            </table>
        </fieldset>
        <table width="100%">
            <tr>
                <td align="right">
                    <asp:Button ID="btn_print" runat="server" Text="Print" Skin="Metro" OnClick="btn_PrintConsignment_Click"
                        CssClass="button" AutoPostBack="true" UseSubmitBehavior="false" CausesValidation="false">
                    </asp:Button>
                    
                </td>
            </tr>
        </table>
         
    </div>
</asp:Content>
