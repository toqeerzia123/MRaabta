<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="ExpressionConsignmentApproval.aspx.cs" Inherits="MRaabta.Files.ExpressionConsignmentApproval" %>

<%@ Register TagPrefix="Ajax1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <telerik:radajaxmanager id="RadAjaxManager1" runat="server">
        <ajaxsettings>
            <telerik:AjaxSetting AjaxControlID="gv_items">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gv_items" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </ajaxsettings>
    </telerik:radajaxmanager>
    <telerik:radajaxloadingpanel id="RadAjaxLoadingPanel1" runat="server" />
    <script language="javascript" type="text/javascript">
        
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

        function checkValidations(txt) {
            debugger;


            if (txt.value.length > 15 || txt.value.length < 11) {
                alert('Consignment Number must be between 11 and 15 digits');
                txt.value = "";
                txt.focus();
                return false;
            }

           
            return true;
        }
    </script>
    <style>
        /*.dd_dropdown*/
        .rddlSlide
        {
            /*
            overflow:auto;
            height:200px;
            */
            max-height: 300px;
            overflow-y: scroll;
        }
    </style>
    <div class="">
        <fieldset class="fieldsetSmall">
            <asp:Label ID="Errorid" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
            <legend style="font-size: medium;"><b>Expressions Consignment Form</b></legend>
            <table width="90%">
                <tr>
                    <td class="cellNormal">
                        Consignment No.
                    </td>
                    <td>
                        <%--<telerik:radtextbox id="txt_ConNo" runat="server" skin="Web20" maxlength="12" autopostback="True"
                            labelwidth="64px" resize="None" resolvedrendermode="Classic" width="160px" ontextchanged="txt_ConNo_TextChanged"
                            onkeypress="return isNumberKey(event);">
                        </telerik:radtextbox>--%>
                        <asp:TextBox ID="txt_ConNo" runat="server" AutoPostBack="true" OnTextChanged="txt_ConNo_TextChanged"
                            onkeypress="return isNumberKey(event);"   onchange="if ( checkValidations(this) == false ) return;"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="TextBoxRequiredFieldValidator" runat="server" Display="Dynamic"
                            ControlToValidate="txt_ConNo" ErrorMessage="Consignment Cannot be Empty" ForeColor="Red"></asp:RequiredFieldValidator>
                    </td>
                    <td class="cellNormal">
                        Customer Type
                    </td>
                    <td class="cellTextField">
                        <asp:RadioButtonList ID="rbtn_customerType" runat="server" RepeatColumns="2" RepeatDirection="Horizontal">
                            <asp:ListItem Value="1">Cash</asp:ListItem>
                            <asp:ListItem Value="2">Credit</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                    <td class="cellNormal">
                        Account No.
                    </td>
                    <td>
                        <%-- <telerik:radtextbox id="txt_AccNo" runat="server" skin="Web20" maxlength="8" width="70px"
                            autopostback="true" ontextchanged="txt_AccNo_TextChanged" style="height: 17px">
                        </telerik:radtextbox>--%>
                        <asp:TextBox ID="txt_AccNo" MaxLength="8" Width="70px" runat="server" AutoPostBack="true"
                            OnTextChanged="txt_AccNo_TextChanged" Style="height: 17px"></asp:TextBox>
                        <asp:CheckBox ID="cb_Account" runat="server" Text="Lock" AutoPostBack="true" OnCheckedChanged="cb_Account_CheckedChanged">
                        </asp:CheckBox>
                        <asp:HiddenField ID="hd_CreditClientID" runat="server" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Display="Dynamic"
                            ControlToValidate="txt_AccNo" ErrorMessage="Account No Cannot be Empty" ForeColor="Red"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="cellNormal">
                        Destination
                    </td>
                    <td class="cellTextField">
                        <%--<telerik:raddropdownlist id="cb_Destination" runat="server" skin="Metro" appenddatabounditems="true"
                             autopostback="true" onitemselected="cb_Destination_ItemSelected"
                            >
                            <Items>
                                                <telerik:DropDownListItem Text="Select Destination" Value="0" />
                                            </Items>
                        </telerik:raddropdownlist>--%>
                        <asp:DropDownList ID="cb_Destination" runat="server" AppendDataBoundItems="true"
                            OnSelectedIndexChanged="cb_Destination_ItemSelected" Width="200px">
                            <asp:ListItem Value="0">Select Destination</asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator10" ControlToValidate="cb_Destination"
                            Display="Dynamic" ErrorMessage="Destination Missing!" InitialValue="Select Destination"
                            CssClass="validator" ForeColor="Red" />
                        <asp:HiddenField ID="hd_Destination" runat="server" />
                    </td>
                    <td class="cellNormal">
                        Consignment Ref No.
                    </td>
                    <td>
                        <%--<telerik:radtextbox id="txt_consignmentRefNo" runat="server" skin="Web20" maxlength="8"
                            width="70px" ontextchanged="txt_RiderCode_TextChanged" autopostback="true">
                        </telerik:radtextbox>--%>
                        <asp:TextBox ID="txt_consignmentRefNo" runat="server" Width="70px" OnTextChanged="txt_RiderCode_TextChanged"
                            AutoPostBack="true"></asp:TextBox>
                        <asp:HiddenField ID="HiddenField1" runat="server" />
                    </td>
                    <td class="cellNormal">
                        RiderCode
                    </td>
                    <td>
                        <%--<telerik:radtextbox id="txt_RiderCode" runat="server" skin="Web20" maxlength="8"
                            width="70px" ontextchanged="txt_RiderCode_TextChanged" autopostback="true">

                        </telerik:radtextbox>--%>
                        <asp:TextBox ID="txt_RiderCode" runat="server" MaxLength="8" Width="70px" OnTextChanged="txt_RiderCode_TextChanged"
                            AutoPostBack="true"></asp:TextBox>
                        <asp:CheckBox ID="cb_Rider" runat="server" AutoPostBack="true" Text="Lock" OnCheckedChanged="cb_Rider_CheckedChanged">
                        </asp:CheckBox>
                        <asp:HiddenField ID="hd_OriginExpressCenter" runat="server" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" Display="Dynamic"
                            ControlToValidate="txt_RiderCode" ErrorMessage="Rider Code Cannot be Empty" ForeColor="Red"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="cellNormal">
                        Booking Date
                    </td>
                    <td class="cellTextField">
                        <%--<telerik:RadDatePicker RenderMode="Lightweight" ID="booking_date" Width="200px"
                                runat="server" Skin="Web20" >
                            </telerik:RadDatePicker>--%>
                        <asp:TextBox ID="booking_date" runat="server" CssClass="med-field" MaxLength="10"></asp:TextBox>
                        <Ajax1:CalendarExtender ID="CalendarExtender1" TargetControlID="booking_date" runat="server"
                            Format="yyyy-MM-dd" PopupButtonID="Image1">
                        </Ajax1:CalendarExtender>
                    </td>
                    <td class="cellNormal">
                        Acc Receving Date
                    </td>
                    <td class="cellTextField">
                        <%--<telerik:RadDatePicker RenderMode="Lightweight" ID="booking_date" Width="200px"
                                runat="server" Skin="Web20" >
                            </telerik:RadDatePicker>--%>
                        <asp:TextBox ID="txt_reportingDate" runat="server" CssClass="med-field" MaxLength="10"></asp:TextBox>
                        <Ajax1:CalendarExtender ID="CalendarExtender2" TargetControlID="txt_reportingDate"
                            runat="server" Format="yyyy-MM-dd" PopupButtonID="Image1">
                        </Ajax1:CalendarExtender>
                    </td>
                    <td class="cellNormal">
                        Invoice Number
                    </td>
                    <td class="cellTextField">
                        <asp:TextBox ID="txt_invoiceNumber" runat="server" Enabled="false"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="cellNormal">
                        Approve Status
                    </td>
                    <td class="cellTextField">
                        <asp:TextBox ID="txt_approveStatus" runat="server" Enabled="false"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <fieldset class="fieldsetSmall">
                            <legend><b>Expressions</b></legend>
                            <table style="width: 100%">
                                <tr>
                                    <td class="cellNormal">
                                        Delivery Date Time
                                    </td>
                                    <td class="cellTextField">
                                        <telerik:raddatetimepicker rendermode="Lightweight" id="dt_DeliveryDatetime" width="200px"
                                            runat="server" skin="Web20">
                                        </telerik:raddatetimepicker>
                                    </td>
                                    <td>
                                    </td>
                                    <td class="cellTextField">
                                        <asp:CheckBox ID="chk_greetingCard" runat="server" Text="Greeting Card" />
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="cellNormal">
                                        Item
                                    </td>
                                    <td class="cellTextField">
                                        <telerik:radcombobox id="dd_item" runat="server" skin="Metro" autopostback="true"
                                            appenddatabounditems="true" allowcustomtext="true" markfirstmatch="true" visible="true"
                                            onselectedindexchanged="dd_item_SelectedIndexChanged" ontextchanged="dd_item_TextChanged"
                                            causesvalidation="false">
                                            <items>
                                                <telerik:RadComboBoxItem Text="Select Item" Value="0" Selected="true" />
                                            </items>
                                        </telerik:radcombobox>
                                        <asp:HiddenField ID="HiddenField2" runat="server" />
                                    </td>
                                    <td style="width: 5%; text-align: right;">
                                        QTY
                                    </td>
                                    <td class="cellTextField">
                                        <%--    <asp:TextBox ID="txt_qty" runat="server" CssClass="textBox" Enabled="false"></asp:TextBox>
                                        --%>
                                        <telerik:radtextbox id="txt_qty" runat="server" skin="Web20" maxlength="12" autopostback="True"
                                            labelwidth="64px" resize="None" resolvedrendermode="Classic" width="160px" text="0">
                                        </telerik:radtextbox>
                                    </td>
                                    <td style="width: 5%; text-align: right;">
                                        Amount
                                    </td>
                                    <td class="cellTextField">
                                        <%--     <asp:TextBox ID="txt_amount" runat="server" CssClass="textBox" Enabled="false"></asp:TextBox>
                                        --%>
                                        <telerik:radtextbox id="txt_amount" runat="server" skin="Web20" maxlength="12" autopostback="True"
                                            labelwidth="64px" resize="None" resolvedrendermode="Classic" width="160px" text="0"
                                            onkeypress="return isNumberKey(event);" enabled="false">
                                        </telerik:radtextbox>
                                    </td>
                                    <td>
                                        <asp:Button ID="btn_add" runat="server" Text="Add" CssClass="button" OnClick="btn_add_Click"
                                            UseSubmitBehavior="false" CausesValidation="false" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="7">
                                        <div style="height: 250px; overflow: scroll;">
                                            <%--                                            <asp:GridView ID="gv_items" runat="server" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false"
                                                BackColor="Transparent" Width="100%">
                                                <HeaderStyle BackColor="#5f5a8d" HorizontalAlign="Left" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Delete" HeaderStyle-Width="5%">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lbtn_delete" Text="Delete" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Code") %>'
                                                                CommandName="Del"></asp:LinkButton>
                                                            <asp:HiddenField ID="hd_id" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "Code") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField HeaderText="Item Code" DataField="Description" HeaderStyle-Width="70%" />
                                                    <asp:BoundField HeaderText="Qty" DataField="Qty" HeaderStyle-Width="12.5%" />
                                                    <asp:BoundField HeaderText="Amount" DataField="Amount" HeaderStyle-Width="12.5%" />
                                                </Columns>
                                            </asp:GridView>--%>
                                            <asp:Repeater ID="gv_items" runat="server" OnItemCommand="RadGrid1_ItemCommand" OnItemDataBound="RadGrid1_ItemDataBound">
                                                <HeaderTemplate>
                                                    <table width="100%" class="TBL_REPETER">
                                                        <tr>
                                                            <th align="center">
                                                                Item Code
                                                            </th>
                                                            <th align="center">
                                                                Qty
                                                            </th>
                                                            <th align="center">
                                                                Amount
                                                            </th>
                                                            <th>
                                                            </th>
                                                        </tr>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lbl_ItemDescription" runat="server" ForeColor="Black"></asp:Label>
                                                            <asp:HiddenField ID="Hd_ID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "itemid")%>' />
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lbl_qty" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "itemQty")%>'></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lbl_Amount" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "amount")%>'></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:ImageButton ID="Delete_Image" runat="server" ToolTip="Delete SKU" ImageUrl="~/Images/1461581832_delete.png"
                                                                CausesValidation="false" Width="20px" Height="20px" CommandName="Delete" />
                                                            <asp:HiddenField ID="hd_status" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "status") %>' />
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    </table>
                                                </FooterTemplate>
                                            </asp:Repeater>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="cellNormal">
                                        Message:
                                    </td>
                                    <td class="cellTextField" colspan="6">
                                        <asp:TextBox ID="txt_message" runat="server" CssClass="textBox" Width="100%"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>
                </tr>
                <tr>
                    <td class="cellNormal">
                        Pieces
                    </td>
                    <td class="cellTextField">
                        <asp:TextBox ID="txt_pieces" Text="1" runat="server" Enabled="false" CssClass="textBox"></asp:TextBox>
                    </td>
                    <td class="cellNormal">
                        Other Charges
                    </td>
                    <td class="cellTextField">
                        <asp:TextBox ID="txt_otherCharges" runat="server" Enabled="false" CssClass="textBox"></asp:TextBox>
                    </td>
                    <td colspan="2">
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td class="cellNormal">
                        <asp:CheckBox ID="chk_cash" runat="server" Style="float: left" Text="Cash Sent" />
                    </td>
                    <td class="cellNormal">
                        Package Contents
                    </td>
                    <td colspan="3" class="cellTextField">
                        <telerik:radtextbox id="txt_packageContents" runat="server" skin="Web20" maxlength="150"
                            autopostback="True" labelwidth="64px" resize="None" resolvedrendermode="Classic"
                            width="360px">
                        </telerik:radtextbox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <fieldset class="fieldsetSmall">
                            <legend>Consigner Details</legend>
                            <table>
                                <tr>
                                    <td style="text-align: right">
                                        Cell No.
                                    </td>
                                    <td>
                                        <%--      <telerik:RadTextBox ID="txt_ConsignerCellNo" runat="server" Skin="Web20" MaxLength="12"
                                            AutoPostBack="True" LabelWidth="64px" Resize="None" ResolvedRenderMode="Classic"
                                            Width="160px">
                                        </telerik:RadTextBox>--%>
                                        <telerik:radmaskedtextbox id="txt_ConsignerCellNo" runat="server" skin="Web20" 
                                            mask="##(###)#######" labelwidth="64px" resize="None" resolvedrendermode="Classic"
                                            width="160px" causesvalidation="false">
                                        </telerik:radmaskedtextbox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" Display="Dynamic"
                                            ControlToValidate="txt_ConsignerCellNo" ErrorMessage="Consigner Cell No Cannot be empty"
                                            ForeColor="Red"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        CNIC No.
                                    </td>
                                    <td>
                                        <telerik:radmaskedtextbox id="Txt_ConsignerCNIC" runat="server" mask="#####-#######-#"
                                            labelwidth="64px" resolvedrendermode="Classic" width="160px" skin="Web20" cssclass="med-field-right"
                                            displayformatposition="Right" roundnumericranges="False">
                                        </telerik:radmaskedtextbox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" Display="Dynamic"
                                            ControlToValidate="Txt_ConsignerCNIC" ErrorMessage="Consigner CNIC Cannot be empty"
                                            ForeColor="Red"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        Consigner
                                    </td>
                                    <td>
                                        <telerik:radtextbox id="txt_Consigner" runat="server" skin="Web20" maxlength="40"
                                             labelwidth="64px" resize="None" resolvedrendermode="Classic"
                                            width="160px">
                                        </telerik:radtextbox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" Display="Dynamic"
                                            ControlToValidate="txt_Consigner" ErrorMessage="Consigner Cannot be empty" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        Address
                                    </td>
                                    <td>
                                        <telerik:radtextbox id="txt_ShipperAddress" runat="server" skin="Web20" maxlength="100"
                                             labelwidth="64px" resize="None" resolvedrendermode="Classic"
                                            width="160px">
                                        </telerik:radtextbox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" Display="Dynamic"
                                            ControlToValidate="txt_ShipperAddress" ErrorMessage="Shipper Address cannot be Empty"
                                            ForeColor="Red"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>
                    <td colspan="2">
                        <fieldset class="fieldsetSmall">
                            <legend>Consignee Details</legend>
                            <table>
                                <tr>
                                    <td>
                                        Consignee
                                    </td>
                                    <td>
                                        <telerik:radtextbox id="txt_Consignee" runat="server" skin="Web20" maxlength="40"
                                            labelwidth="64px" resize="None" resolvedrendermode="Classic" width="160px">
                                        </telerik:radtextbox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" Display="Dynamic"
                                            ControlToValidate="txt_Consignee" ErrorMessage="Consignee cannot be Empty" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        Address
                                    </td>
                                    <td>
                                        <telerik:radtextbox id="txt_Address" runat="server" skin="Web20" maxlength="100"
                                            labelwidth="64px" resize="None" resolvedrendermode="Classic" width="160px">
                                        </telerik:radtextbox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" Display="Dynamic"
                                            ControlToValidate="txt_Address" ErrorMessage="Consignee Address cannot be Empty"
                                            ForeColor="Red"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        Area
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="dd_consigneeArea" runat="server" CssClass="dropdown">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Phone No
                                    </td>
                                    <td>
                                        <%--    <telerik:RadTextBox ID="txt_ConsigneeCellno" runat="server" Skin="Web20" MaxLength="12"
                                            AutoPostBack="True" LabelWidth="64px" Resize="None" ResolvedRenderMode="Classic"
                                            Width="160px" CausesValidation="false">
                                        </telerik:RadTextBox>--%>
                                        <telerik:radmaskedtextbox id="txt_ConsigneeCellno" runat="server" skin="Web20" maxlength="12"
                                            mask="##(###)#######" autopostback="True" labelwidth="64px" resize="None" resolvedrendermode="Classic"
                                            width="160px" causesvalidation="false">
                                        </telerik:radmaskedtextbox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" Display="Dynamic"
                                            ControlToValidate="txt_ConsigneeCellno" ErrorMessage="Consignee Phone No cannot be Empty"
                                            ForeColor="Red"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>
                    <td colspan="2">
                        <fieldset class="fieldsetSmall">
                            <legend>Charges</legend>
                            <table>
                                <tr>
                                    <td style="text-align: right; width: 40%">
                                        Charged Rate
                                    </td>
                                    <td style="width: 60%; text-align: center;">
                                        <telerik:radnumerictextbox id="txt_chargedRate" runat="server" maxlength="7" skin="Web20"
                                            enabled="false">
                                        </telerik:radnumerictextbox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        Service Charges
                                    </td>
                                    <td>
                                        <telerik:radnumerictextbox id="txt_chargesWalaOtherCharges" runat="server" maxlength="7"
                                            skin="Web20" enabled="false">
                                        </telerik:radnumerictextbox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        GST
                                    </td>
                                    <td>
                                        <telerik:radnumerictextbox id="txt_gst" runat="server" maxlength="7" skin="Web20"
                                            enabled="false">
                                        </telerik:radnumerictextbox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        Total Amount
                                    </td>
                                    <td>
                                        <telerik:radnumerictextbox id="txt_totalAmount" runat="server" maxlength="7" skin="Web20"
                                            enabled="false">
                                        </telerik:radnumerictextbox>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>
                </tr>
                <tr>
                    <td colspan="6" style="text-align: right;">
                        <asp:Button ID="btn_save" runat="server" CssClass="button" Text="Save" Height="30px"
                            BackColor="#5f5a8d" BorderColor="#9693ba" BorderStyle="Outset" BorderWidth="2px"
                            ForeColor="White" Font-Bold="true" OnClick="btn_save_Click" UseSubmitBehavior="false" />
                        <asp:Button ID="btn_update" runat="server" CssClass="button" Text="Update" Height="30px"
                            BackColor="#5f5a8d" BorderColor="#9693ba" BorderStyle="Outset" BorderWidth="2px"
                            ForeColor="White" Font-Bold="true" OnClick="btn_update_Click" UseSubmitBehavior="false" />
                        <asp:Button ID="btn_unapprove1" runat="server" CssClass="button" Height="30px"
                            BackColor="#5f5a8d" BorderColor="#9693ba" BorderStyle="Outset" BorderWidth="2px"
                            ForeColor="White" Font-Bold="true"
                            Text="UnApprove" onclick="btn_unapprove_Click" CausesValidation="false"/>
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
    </table>
</asp:Content>
