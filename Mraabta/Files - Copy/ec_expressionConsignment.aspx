<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="ec_expressionConsignment.aspx.cs" Inherits="MRaabta.Files.ec_expressionConsignment" %>

<%@ Register TagPrefix="Ajax1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script language="javascript" type="text/javascript">
        count = 0;
        function isNumberKey(evt) {

            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode != 46 && charCode > 31
            && (charCode < 48 || charCode > 57 || charCode == 110 || charCode == 109 || charCode == 45)) {
                return false;
            }
            else {

                if (charCode == 110) {
                    count++;
                }
                if (count > 1) {
                    return false
                }
            }

            return true;
        }
        function diableSave() {

            var saveBtn = document.getElementById('<%= btn_save.ClientID %>');
            saveBtn.disabled = true;
        }

        function CalculateItemPrice() {

            var qty = document.getElementById('<%= txt_qty.ClientID %>');
            var items = $find('<%= dd_item.ClientID %>');
            var ItemsPrices = document.getElementById('<%= tbl_productPrices.ClientID %>');
            var txt_amount = document.getElementById('<%= txt_amount.ClientID %>');
            var itemID = items.get_value();
            var found = false;
            for (var i = 0; i < ItemsPrices.rows.length; i++) {
                if (ItemsPrices.rows[i].cells[0].innerText == itemID) {
                    txt_amount.value = (ItemsPrices.rows[i].cells[1].innerText.toString()) * qty.value;
                    found = true;
                    break;
                }
            }

            if (!found) {
                alert('Item Not Found. Check the Selected Item.');
                qty.value = "";
            }
        }



        function removeItem(btn) {
            debugger;


        }

        function AddRow() {
            //Reference the GridView.

            var gridView = document.getElementById("<%=gv_items.ClientID %>");


            var qty = document.getElementById('<%= txt_qty.ClientID %>');
            var items = $find('<%= dd_item.ClientID %>');
            var ItemsPrices = document.getElementById('<%= tbl_productPrices.ClientID %>');
            var txt_amount = document.getElementById('<%= txt_amount.ClientID %>');
            var itemID = items.get_value();





            //Reference the TBODY tag.
            var tbody = gridView.getElementsByTagName("tbody")[0];

            //Reference the first row.
            var row = tbody.getElementsByTagName("tr")[1];

            //Check if row is dummy, if yes then remove.
            if (row.getElementsByTagName("td")[0].innerText.trim() == "") {
                tbody.removeChild(row);
            }

            //Clone the reference first row.
            row = row.cloneNode(true);
            row.getElementsByTagName("td")[1].innerText = "";
            row.getElementsByTagName("td")[2].innerText = "";
            row.getElementsByTagName("td")[3].innerText = "";

            //Add the Name value to first cell.

            debugger;
            SetValue(row, 0, "name", itemID);

            //Add the Country value to second cell.


            //Add the row to the GridView.
            tbody.appendChild(row);
            txtName.Text = "";

            return false;
        };

        function SetValue(row, index, name, textbox) {
            //Reference the Cell and set the value.
            row.cells[index].innerHTML = textbox.value;

            //Create and add a Hidden Field to send value to server.
            var input = document.createElement("input");
            input.type = "hidden";
            input.name = name;
            input.value = textbox.value;
            row.cells[index].appendChild(input);

            //Clear the TextBox.
            textbox.value = "";
        }

        function PrintReceipt() {
            var cn = document.getElementById('<%= txt_ConNo.ClientID %>');
            var win = window.open('ExpressionPrint.aspx?Xcode=' + cn.value, '_blank');
            if (win) {
                win.focus();
            }
            return false;
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
        <asp:HiddenField ID="hd_portalCn" runat="server" />
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
                        <asp:TextBox ID="txt_ConNo" runat="server" AutoPostBack="false" onchange="diableSave();"
                            onkeypress="return isNumberKey(event);"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="TextBoxRequiredFieldValidator" runat="server" Display="Dynamic"
                            ControlToValidate="txt_ConNo" ErrorMessage="Consignment Cannot be Empty" ForeColor="Red"></asp:RequiredFieldValidator>
                    </td>
                    <td class="cellNormal">
                        Customer Type
                    </td>
                    <td class="cellTextField">
                        <asp:RadioButtonList ID="rbtn_customerType" runat="server" RepeatColumns="2" RepeatDirection="Horizontal"
                            Enabled="false">
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
                        <asp:TextBox ID="txt_AccNo" MaxLength="8" Width="70px" runat="server" AutoPostBack="false"
                            OnTextChanged="txt_AccNo_TextChanged" Style="height: 17px" onchange="diableSave();"></asp:TextBox>
                        <asp:CheckBox ID="cb_Account" runat="server" Text="Lock" AutoPostBack="false" OnCheckedChanged="cb_Account_CheckedChanged">
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
                            OnSelectedIndexChanged="cb_Destination_ItemSelected" Width="200px" onchange="diableSave();">
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
                        <asp:TextBox ID="txt_consignmentRefNo" runat="server" Width="70px" OnTextChanged="txt_RiderCode_TextChanged"></asp:TextBox>
                        <asp:HiddenField ID="HiddenField1" runat="server" />
                    </td>
                    <td class="cellNormal">
                        RiderCode
                    </td>
                    <td>
                        <%--<telerik:radtextbox id="txt_RiderCode" runat="server" skin="Web20" maxlength="8"
                            width="70px" ontextchanged="txt_RiderCode_TextChanged" autopostback="true">

                        </telerik:radtextbox>--%>
                        <asp:TextBox ID="txt_RiderCode" runat="server" MaxLength="8" Width="70px" onchange="diableSave();"
                            AutoPostBack="false"></asp:TextBox>
                        <asp:CheckBox ID="cb_Rider" runat="server" AutoPostBack="false" Text="Lock" OnCheckedChanged="cb_Rider_CheckedChanged">
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
                        <asp:TextBox ID="booking_date" runat="server" CssClass="med-field" MaxLength="10"
                            Enabled="false" onchange="diableSave();"></asp:TextBox>
                        <Ajax1:CalendarExtender ID="CalendarExtender1" TargetControlID="booking_date" runat="server"
                            Format="yyyy-MM-dd" PopupButtonID="Image1">
                        </Ajax1:CalendarExtender>
                    </td>
                    <td class="cellNormal">
                    </td>
                    <td class="cellTextField">
                    </td>
                    <td class="cellNormal">
                    </td>
                    <td>
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
                                            runat="server" skin="Web20" onchange="diableSave();">
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
                                        <%--   <asp:DropDownList ID="dd_item" runat="server" CssClass="dropdown">
                                        </asp:DropDownList>--%>
                                        <%--<telerik:raddropdownlist id="dd_item" runat="server" skin="Metro" appenddatabounditems="true"
                                            autopostback="true" onitemselected="dd_item_ItemSelected" causesvalidation="false" >
                                            <Items>
                                                <telerik:DropDownListItem Text="Select Item" Value="0" />
                                            </Items>
                                        </telerik:raddropdownlist>--%>
                                        <telerik:radcombobox id="dd_item" runat="server" skin="Metro" appenddatabounditems="true"
                                            autopostback="false" allowcustomtext="true" markfirstmatch="true" visible="true"
                                            causesvalidation="false" onselectedindexchanged="dd_item_ItemSelected" onchange="diableSave();">
                    <Items>
                        <telerik:RadComboBoxItem Text="Select item" Value="0" selected="true"/>
                    </Items>
                </telerik:radcombobox>
                                        <asp:HiddenField ID="HiddenField2" runat="server" />
                                    </td>
                                    <td style="width: 5%; text-align: right;">
                                        QTY
                                    </td>
                                    <td class="cellTextField">
                                        <%--    <asp:TextBox ID="txt_qty" runat="server" CssClass="textBox" Enabled="false"></asp:TextBox>
                                        --%>
                                        <%--<telerik:radtextbox id="txt_qty" runat="server" skin="Web20" maxlength="12" autopostback="True"
                                            labelwidth="64px" resize="None" resolvedrendermode="Classic" width="160px" text="0">
                                        </telerik:radtextbox>--%>
                                        <asp:TextBox ID="txt_qty" runat="server" MaxLength="12" AutoPostBack="true" OnTextChanged="txt_qty_TextChanged">
                                        </asp:TextBox>
                                    </td>
                                    <td style="width: 5%; text-align: right;">
                                        Amount
                                    </td>
                                    <td class="cellTextField">
                                        <%--     <asp:TextBox ID="txt_amount" runat="server" CssClass="textBox" Enabled="false"></asp:TextBox>
                                        --%>
                                        <%--<telerik:radtextbox id="txt_amount" runat="server" skin="Web20" maxlength="12" autopostback="True"
                                            labelwidth="64px" resize="None" resolvedrendermode="Classic" width="160px" text="0"
                                            onkeypress="return isNumberKey(event);" enabled="false">
                                        </telerik:radtextbox>--%>
                                        <asp:TextBox ID="txt_amount" runat="server" MaxLength="12" AutoPostBack="false" onkeypress="return isNumberKey(event);"
                                            Enabled="false"></asp:TextBox>
                                        <asp:HiddenField ID="hd_itemGst" runat="server" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btn_add" runat="server" Text="Add" CssClass="button" UseSubmitBehavior="false"
                                            CausesValidation="false" OnClick="btn_add_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="7">
                                        <div style="height: 250px; overflow: scroll;">
                                            <asp:GridView ID="gv_items" runat="server" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false"
                                                BackColor="Transparent" Width="100%">
                                                <HeaderStyle BackColor="#5f5a8d" HorizontalAlign="Left" ForeColor="White" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Delete" HeaderStyle-Width="5%">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lbtn_delete" Text="Delete" runat="server" CausesValidation="false"
                                                                CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ItemID") %>' CommandName="Remove"
                                                                OnClientClick="diableSave();"></asp:LinkButton>
                                                            <asp:HiddenField ID="hd_id" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "ItemID") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField HeaderText="Item Code" DataField="Description" HeaderStyle-Width="57.5%" />
                                                    <asp:BoundField HeaderText="Qty" DataField="Qty" HeaderStyle-Width="12.5%" />
                                                    <asp:BoundField HeaderText="Amount" DataField="Amount" HeaderStyle-Width="12.5%" />
                                                    <asp:BoundField HeaderText="GST" DataField="Gst" HeaderStyle-Width="12.5%" />
                                                </Columns>
                                            </asp:GridView>
                                            <%--<table id="tbl_items" runat="server" style="width: 95%;">
                                                <tr>
                                                    <th style="display: none;">
                                                    </th>
                                                    <th style="width: 10%; background-color: #F26726;">
                                                    </th>
                                                    <th style="background-color: #F26726;">
                                                        Item Description
                                                    </th>
                                                    <th style="width: 10%; background-color: #F26726;">
                                                        Item Qty
                                                    </th>
                                                    <th style="width: 10%; background-color: #F26726;">
                                                        Item Amount
                                                    </th>
                                                    <th style="width: 10%; background-color: #F26726;">
                                                        Item GST
                                                    </th>
                                                </tr>
                                            </table>--%>
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
                        <%--<telerik:radtextbox id="txt_packageContents" runat="server" skin="Web20" maxlength="150"
                            autopostback="True" labelwidth="64px" resize="None" resolvedrendermode="Classic"
                            width="360px">
                        </telerik:radtextbox>--%>
                        <asp:TextBox ID="txt_packageContents" runat="server" MaxLength="150" AutoPostBack="false"
                            Width="360px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="vertical-align: top;">
                        <fieldset class="fieldsetSmall" style="height: 130px;">
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
                                        <%--<telerik:radmaskedtextbox id="txt_ConsignerCellNo" runat="server" skin="Web20" maxlength="12"
                                            mask="##(###)#######" labelwidth="64px" resize="None" resolvedrendermode="Classic"
                                            width="160px" autopostback="true" causesvalidation="false">
                                        </telerik:radmaskedtextbox>--%>
                                        <asp:TextBox ID="txt_ConsignerCellNo" runat="server" MaxLength="12" Width="160px"></asp:TextBox>
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
                                            labelwidth="64px" resize="None" resolvedrendermode="Classic" width="160px">
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
                                            labelwidth="64px" resize="None" resolvedrendermode="Classic" width="160px">
                                        </telerik:radtextbox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" Display="Dynamic"
                                            ControlToValidate="txt_ShipperAddress" ErrorMessage="Shipper Address cannot be Empty"
                                            ForeColor="Red"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>
                    <td colspan="2" style="vertical-align: top;">
                        <fieldset class="fieldsetSmall" style="height: 130px;">
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
                                            mask="##(###)#######" labelwidth="64px" resize="None" resolvedrendermode="Classic"
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
                    <td colspan="2" style="vertical-align: top;">
                        <fieldset class="fieldsetSmall" style="height: 130px;">
                            <legend>Charges</legend>
                            <table style="height: 100%">
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
                                <tr style="display: none;">
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
                        <asp:Button ID="btn_validate" runat="server" Text="Validate" CssClass="button" UseSubmitBehavior="false"
                            CausesValidation="false" OnClick="btn_validate_Click" />
                        &nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btn_save" runat="server" CssClass="button" Text="APPROVE" OnClick="btn_save_Click"
                            UseSubmitBehavior="false" Enabled="false" />
                        <asp:Button ID="btn_print" runat="server" CssClass="button" Text="PRINT RECEIPT"
                            UseSubmitBehavior="false" OnClientClick="PrintReceipt();" />
                        <%--<asp:Button ID="btn_update" runat="server" CssClass="button" Text="Update" Height="30px"
                            BackColor="#5f5a8d" BorderColor="#9693ba" BorderStyle="Outset" BorderWidth="2px"
                            ForeColor="White" Font-Bold="true" OnClick="btn_update_Click" UseSubmitBehavior="false" />--%>
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
    <table style="display: none;" runat="server" id="tbl_productPrices">
    </table>
</asp:Content>
