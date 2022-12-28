<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CODPayments.aspx.cs" Inherits="MRaabta.Files.CODPayments" MasterPageFile="~/BtsMasterPage.Master" %>

<%@ Register TagPrefix="Ajax1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script>
        function CriteriaChange() {
            debugger;
            var errorid = document.getElementById('<%= Errorid.ClientID %>');
            errorid.value = "";
            DeleteGridView();
            var cnnumber = document.getElementById('<%= cnNumber.ClientID %>');
            var ChequeCriteria = document.getElementById('<%= ChequeCriteria.ClientID %>');
            var ChequeNumber = document.getElementById('<%= ChequeNo.ClientID %>');


            var radioButtons = document.getElementById('<%= paidstatus.ClientID %>');
            var selectedValue = "";
            for (var i = 0; i < radioButtons.rows[0].cells.length; i++) {
                if (radioButtons.rows[0].cells[i].children[0].checked) {
                    selectedValue = radioButtons.rows[0].cells[i].children[0].defaultValue;
                }
            }


            if (selectedValue.toUpperCase() == "PAID") {
                cnnumber.style.display = 'none';
                ChequeNumber.style.display = 'none';
                ChequeCriteria.style.display = 'none';
            }
            else if (selectedValue.toUpperCase() == "PRINT") {
                cnnumber.style.display = 'none';
                ChequeCriteria.style.display = 'none';
                ChequeNumber.style.display = 'block';
                ChequeCriteriaChange();
            }
        }
        function ChequeCriteriaChange() {
            debugger;
            var radioButtons = document.getElementById('<%= paidstatus.ClientID %>');

            var ChequeNumber = document.getElementById('<%= ChequeNo.ClientID %>');
            var rbtnC = document.getElementById('<%= rbtn_ChequeCriteria.ClientID %>');
            if (rbtnC.rows[0].cells[0].children[0].checked) {
                if (radioButtons.rows[0].cells[1].children[0].checked) { }
                else {
                    /* ChequeNumber.style.display = 'none';*/
                    document.getElementById('<%= txt_chequeNo.ClientID %>').value = "";
                }
            }
            else {

                ChequeNumber.style.display = 'block';
            }
        }

        function DeleteGridView() {
            var grid = document.getElementById('<%= GridView2.ClientID %>');
            if (grid != null) {
                var count = grid.rows.length;
                for (var i = 0; i < count; i++) {
                    grid.deleteRow(0);
                }
            }

            return false;
        }

        function CheckAll(chk) {
            debugger;
            var gridview = document.getElementById('<%= GridView2.ClientID %>');

            var headerCheck = gridview.rows[0].cells[11].childNodes[1].children[0].checked;
            for (var i = 1; i < gridview.rows.length; i++) {
                var rowChk = gridview.rows[i].cells[11].children[0];
                if (headerCheck == true) {
                    rowChk.checked = true;
                }
                else {
                    rowChk.checked = false;
                }

            }


        }

        function ChNoChange(txt) {
            debugger;


            var change = false;
            var radioButtons = document.getElementById('<%= paidstatus.ClientID %>');
            var gridview = document.getElementById('<%= GridView2.ClientID %>');
            var selectedValue = "";
            for (var i = 0; i < radioButtons.rows[0].cells.length; i++) {
                if (radioButtons.rows[0].cells[i].children[0].checked) {
                    selectedValue = radioButtons.rows[0].cells[i].children[0].defaultValue;
                }
            }
            var count = gridview.rows.length;
            if (selectedValue.toUpperCase() == "PAID") {
                if (gridview != null) {
                    for (var i = 1; i < count; i++) {
                        var rowChk = gridview.rows[i].cells[11].children[0].childNodes[0];
                        if (rowChk.checked) {
                            gridview.rows[i].cells[10].childNodes[1].value = txt.value;
                            change = true;
                        }
                    }
                }


                if (!change) {
                    alert('Select Consignment(s) First.');
                }
            }
            else if (selectedValue.toUpperCase() == "EDIT") {
                if (gridview != null) {
                    for (var i = 1; i < count; i++) {
                        var rowChk = gridview.rows[i].cells[11].children[0];
                        if (rowChk.checked) {
                            gridview.rows[i].cells[10].childNodes[1].value = txt.value;
                            change = true;
                        }
                    }
                }
                if (!change) {
                    alert('Select Consignment(s) First.');
                }
            }


        }




        function btnApplyClick() {
            debugger;
            var txt = document.getElementById('<%= txt_chequeNo.ClientID %>');
            var change = false;
            var radioButtons = document.getElementById('<%= paidstatus.ClientID %>');
            var gridview = document.getElementById('<%= GridView2.ClientID %>');
            var selectedValue = "";
            for (var i = 0; i < radioButtons.rows[0].cells.length; i++) {
                if (radioButtons.rows[0].cells[i].children[0].checked) {
                    selectedValue = radioButtons.rows[0].cells[i].children[0].defaultValue;
                }
            }
            var count = gridview.rows.length;
            if (selectedValue.toUpperCase() == "PAID") {
                if (gridview != null) {
                    for (var i = 1; i < count; i++) {
                        var rowChk = gridview.rows[i].cells[11].children[0].childNodes[0];
                        if (rowChk.checked) {
                            gridview.rows[i].cells[10].childNodes[1].value = txt.value;
                            change = true;
                        }
                    }
                }


                if (!change) {
                    alert('Select Consignment(s) First.');
                }
            }
            else if (selectedValue.toUpperCase() == "EDIT") {
                if (gridview != null) {
                    for (var i = 1; i < count; i++) {
                        var rowChk = gridview.rows[i].cells[11].children[0];
                        if (rowChk.checked) {
                            gridview.rows[i].cells[10].childNodes[1].value = txt.value;
                            change = true;
                        }
                    }
                }
                if (!change) {
                    alert('Select Consignment(s) First.');
                }
            }
        }
        function SearchByAccountNumber(txt) {
            debugger;
            var dropdown = document.getElementById('<%= dd_customer.ClientID %>');
            var accountNumber = txt.value;
            var found = false;
            for (var i = 0; i < dropdown.length; i++) {
                var dropdownCurrentValue = dropdown.options[i].value.replace('_NEW', '');
                dropdownCurrentValue = dropdownCurrentValue.replace('_OLD', '');
                if (dropdownCurrentValue.toUpperCase() == accountNumber.toUpperCase()) {
                    dropdown.options[i].selected = true;
                    found = true;
                    break;
                }
            }

            if (!found) {
                alert('Account Not Found');
            }
         }
    </script>
    <style>
        .detail
        {
            border-radius: 10px;
            box-shadow: 0 0 1px rgb(0, 0, 0);
            left: 5%;
            margin: 0;
            overflow: hidden;
            padding: 0;
            position: relative;
            width: 90%;
        }
    </style>
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <div>
                <asp:Label ID="Errorid" runat="server"></asp:Label>
                <table cellpadding="0" cellspacing="0" style="width: 97% !important;" class="input-form">
                    <tr>
                        <td class="input-field" style="width: 20% !important; display: none;">
                            <div>
                                Start Date</div>
                            <div>
                                <asp:TextBox ID="dd_start_date" runat="server" CssClass="med-field" MaxLength="10"
                                    Width="95%"></asp:TextBox>
                                <Ajax1:CalendarExtender ID="CalendarExtender3" TargetControlID="dd_start_date" runat="server"
                                    Format="yyyy-MM-dd" PopupButtonID="Image1">
                                </Ajax1:CalendarExtender>
                            </div>
                        </td>
                        <td class="input-field" style="width: 20% !important; display: none;">
                            <div>
                                End Date</div>
                            <div>
                                <asp:TextBox ID="dd_end_date" runat="server" CssClass="med-field" MaxLength="10"
                                    Width="95%"></asp:TextBox>
                                <Ajax1:CalendarExtender ID="CalendarExtender1" TargetControlID="dd_end_date" runat="server"
                                    Format="yyyy-MM-dd" PopupButtonID="Image1">
                                </Ajax1:CalendarExtender>
                            </div>
                        </td>
                        <td class="input-field" style="width: 20% !important">
                            <div>
                                Account Number</div>
                            <div>
                                <input id="txtAccNum" type="text" onchange="SearchByAccountNumber(this);" />
                            </div>
                        </td>
                        <td class="input-field" style="width: 25% !important;">
                            <div>
                                Customer Name</div>
                            <div>
                                <asp:DropDownList ID="dd_customer" runat="server" Width="99%">
                                    <asp:ListItem Value="">Select Customer Name</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </td>
                        <%--<td class="input-field" style="width: 12% !important; margin-left: 2% !important;">
                    <div>
                        Paid Status</div>
                    <div>
                        <asp:DropDownList ID="paidstatus" runat="server">
                            <asp:ListItem Value="PAID" Text="PAID"></asp:ListItem>
                            <asp:ListItem Value="UNPAID" Text="UNPAID"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:CheckBox ID="paidstatus_chk" runat="server" AutoPostBack="true" Text="ALL" OnCheckedChanged="paidstatus_chk_CheckedChanged" />
                    </div>
                </td>--%>
                        <td class="input-field" style="width: 15% !important;" colspan="2">
                            <div>
                                Output Type</div>
                            <div style="width: 100% !important;">
                                <asp:DropDownList ID="type" runat="server" Width="90%">
                                    <asp:ListItem Value="">Select Output Type</asp:ListItem>
                                    <asp:ListItem Value="html" Selected="True">HTML</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </td>
                        <td class="input-field" style="width: 10% !important;">
                            <asp:Button ID="Button1" runat="server" Text="Show Data" CssClass="button" OnClick="Btn_Search_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td class="input-field" style="width: 20% !important;">
                            <div>
                                Operation Criteria
                            </div>
                            <div>
                                <asp:RadioButtonList ID="paidstatus" runat="server" RepeatColumns="2" RepeatLayout="Table"
                                    onchange="CriteriaChange();" Width="100%">
                                    <asp:ListItem Value="PAID" Text="Mark Paid" Selected="True"></asp:ListItem>
                                    <asp:ListItem Value="PRINT" Text="PRINT"></asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                        </td>
                        <td id="ChequeNo" runat="server" style="display: none; float: left;">
                            <div>
                                Payment ID
                            </div>
                            <div>
                                <asp:TextBox ID="txt_chequeNo" runat="server" Width="95%" onchange="ChNoChange(this);"></asp:TextBox>
                            </div>
                        </td>
                        <td id="cnNumber" runat="server" style="display: none; float: left;">
                            <div>
                                Consignment Number
                            </div>
                            <div>
                                <asp:TextBox ID="txt_cnNumber" runat="server" Width="95%"></asp:TextBox>
                            </div>
                        </td>
                        <td id="ChequeCriteria" runat="server" style="display: none; float: left;">
                            <div>
                                &nbsp;
                            </div>
                            <asp:RadioButtonList ID="rbtn_ChequeCriteria" runat="server" RepeatColumns="3" RepeatLayout="Table"
                                onchange="ChequeCriteriaChange();">
                                <asp:ListItem Value="0" Text="W/O Cheque" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="1" Text="With Cheque"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td style="display: block; float: right; text-align: left; margin-right: 100px;">
                            <div>
                                &nbsp;
                            </div>
                            <div>
                                <%--<asp:Button ID="btn_apply" runat="server" Width="95%" OnClientClick="btnApplyClick();"
                            Text="Apply Changes" CssClass="button"></asp:Button>--%>
                                <button id="btn_apply" style="width: 100%; display: none;" class="button" onclick="btnApplyClick();">
                                    Apply Changes
                                </button>
                            </div>
                        </td>
                    </tr>
                </table>
                <asp:Label ID="error_msg" CssClass="error_msg" runat="server"></asp:Label>
                <div id="Tabelabc" class="tbl-large" runat="server" style="float: left; width: 100% !important;
                    height: 200px !important; overflow-y: scroll;">
                    <div style="width: 100%; padding-left: 20%; padding-right: 20%; text-align: center;">
                        <table width="60%">
                            <tr>
                                <td style="text-align: right;">
                                    <b>Account No: </b>
                                </td>
                                <td style="text-align: left;">
                                    <asp:Label ID="lbl_accountNumber" runat="server"></asp:Label>
                                </td>
                                <td style="text-align: right;">
                                    <b>Account Name: </b>
                                </td>
                                <td style="text-align: left;">
                                    <asp:Label ID="lbl_accountName" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right;">
                                    <b>Branch: </b>
                                </td>
                                <td style="text-align: left;">
                                    <asp:Label ID="lbl_branchName" runat="server"></asp:Label>
                                </td>
                                <td style="text-align: right;">
                                    <b>Date: </b>
                                </td>
                                <td style="text-align: left;">
                                    <asp:Label ID="lbl_date" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="width: 100%; padding-left: 10%; padding-right: 10%;" class="tbl-large">
                        <asp:GridView ID="gv_invoiceDetail" runat="server" AutoGenerateColumns="false" Width="80%"
                            ShowHeaderWhenEmpty="true" EmptyDataText="No Invoices Avaiable" CssClass="mGrid"
                            ShowFooter="true">
                            <Columns>
                                <asp:TemplateField HeaderText="InvoiceNumber" HeaderStyle-Width="20%">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_invoiceNumber" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "InvoiceNumber") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Month" HeaderStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_month" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Month") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Branch" HeaderStyle-Width="15%">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_branch" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Branch") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Company Name" HeaderStyle-Width="15%" FooterStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_company" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "CompanyNAme") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <b>Total Invoice Amount(A):</b><br />
                                        <b>Total COD Payable(B):</b><br />
                                        <b>Balance (B-A):</b>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Outstanding" HeaderStyle-Width="25%" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_outstanding" runat="server" Text='<%# String.Format("{0:N0}",DataBinder.Eval(Container.DataItem, "Oustanding")) %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <b>
                                            <asp:Label ID="lbl_fTotalInvAmount" runat="server"></asp:Label>
                                        </b>
                                        <br />
                                        <b>
                                            <asp:Label ID="lbl_fTotalCODPayable" runat="server"></asp:Label>
                                        </b>
                                        <br />
                                        <b>
                                            <asp:Label ID="lbl_fBalance" runat="server"></asp:Label>
                                        </b>
                                        <br />
                                    </FooterTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
                <br />
                <div style="width: 100%; text-align: center;">
                    <asp:Button ID="btn_save" runat="server" Text="Save Changes" CssClass="button" OnClick="btn_save_Click" />&nbsp;
                    <asp:Button ID="btn_reset" runat="server" Text="Reset" CssClass="button" OnClick="btn_reset_Click" />&nbsp;
                    <asp:Button ID="Button4" runat="server" Text="Print" CssClass="button" OnClick="Button4_Click" />
                </div>
                <div id="div2" class="tbl-large" runat="server" style="width: 100%; height: 360px;
                    overflow: scroll;">
                    <asp:GridView ID="GridView2" runat="server" CssClass="mGrid" AutoGenerateColumns="false"
                        OnRowDataBound="GridView2_RowDataBound">
                        <HeaderStyle />
                        <Columns>
                            <asp:BoundField DataField="Sr" HeaderText="Sr" ControlStyle-Width="5px" />
                            <asp:BoundField DataField="CONSIGNMENTNUMBER" HeaderText="CN Number" />
                            <asp:BoundField DataField="BOOKINGDATE" HeaderText="B-Date" />
                            <asp:BoundField DataField="RRSTATUS" HeaderText="Status" />
                            <asp:BoundField DataField="PaymentVoucherID" HeaderText="RR Number" />
                            <asp:BoundField DataField="VOUCHERDATE" HeaderText="Voucher Date" />
                            <asp:BoundField DataField="CollectionBr" HeaderText="Collection Br." />
                            <asp:BoundField DataField="CODAMOUNT" HeaderText="COD AMT" />
                            <asp:BoundField DataField="AvailableAmount" HeaderText="AvailableAmt" />
                            <asp:BoundField DataField="PAIDSTATUS" HeaderText="PAID STATUS" />
                            <%--<asp:BoundField DataField="ChequeNo" HeaderText="CHEQUE #" />--%>
                            <asp:TemplateField HeaderText="CHEQUE #">
                                <ItemTemplate>
                                    <asp:TextBox ID="lbl_cheque" runat="server" Enabled="false" Text='<%# DataBinder.Eval(Container.DataItem, "ChequeNo") %>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="PAID/UNPAID">
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chk_paidAll" runat="server" onchange="CheckAll(this);" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chk_paid" runat="server" Text="" />
                                    <asp:HiddenField ID="hd_voucherID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "VoucherID") %>' />
                                    <asp:HiddenField ID="hd_DeliveryStatus" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "DeliveryStatus") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <asp:GridView ID="GridView1" runat="server" CssClass="mGrid" AutoGenerateColumns="false">
                    <HeaderStyle />
                    <Columns>
                        <asp:BoundField DataField="Sr" HeaderText="SR." />
                        <asp:BoundField DataField="CONSIGNMENTNUMBER" HeaderText="CN Number" />
                        <asp:BoundField DataField="orderRefNo" HeaderText="CustRef#" />
                        <asp:BoundField DataField="BOOKINGDATE" HeaderText="B-Date" />
                        <asp:BoundField DataField="RRDate" HeaderText="Receipt Date" />
                        <asp:BoundField DataField="Consignee" HeaderText="Consignee" />
                        <asp:BoundField DataField="Origin" HeaderText="ORG." />
                        <asp:BoundField DataField="Destination" HeaderText="DST." />
                        <asp:BoundField DataField="Weight" HeaderText="Weight" />
                        <asp:BoundField DataField="CNSTATUS" HeaderText="Status" />
                        <asp:BoundField DataField="CODAmount" HeaderText="COD Amount(PKR)" />
                        <asp:BoundField DataField="ShippingCharges" HeaderText="Shipping CHG(PKR)" />
                    </Columns>
                </asp:GridView>
                <br />
                <div style="width: 100%; text-align: center;">
                    <asp:Button ID="Button2" runat="server" Text="Save Changes" CssClass="button" OnClick="btn_save_Click" />&nbsp;
                    <asp:Button ID="Button3" runat="server" Text="Reset" CssClass="button" OnClick="btn_reset_Click" />&nbsp;
                    <asp:Button ID="btn_print" runat="server" Text="Print" CssClass="button" OnClick="Button4_Click" />
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="Button4" />
            <asp:PostBackTrigger ControlID="btn_print" />
            <asp:PostBackTrigger ControlID="Button2" />
            <asp:PostBackTrigger ControlID="btn_save" />
            <asp:PostBackTrigger ControlID="GridView2" />
            <asp:PostBackTrigger ControlID="Button1" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
