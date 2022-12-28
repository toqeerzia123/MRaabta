<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReceiptVoucherCODBulk.aspx.cs" Inherits="MRaabta.Files.ReceiptVoucherCODBulk" MasterPageFile="~/BtsMasterPage.master" %>

<%@ Register TagPrefix="Ajax1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script>
        var success = false;
        function isNumberKey(evt) {

            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                if (charCode == 0) {
                    return true;
                }
                return false;
            }
            return true;
        }

        function alphanumeric(evt) {

            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                if (charCode == 0 || ((charCode > 64 && charCode < 91) || (charCode > 96 && charCode < 123) || charCode == 8)) {
                    return true;
                }
                return false;
            }
            return true;
        }

        function checkAmount(txt) {

            //            var div = document.getElementById('<%= loader.ClientID %>');
            //            div.style.display = 'block';
            var index = txt.parentNode.parentNode.rowIndex;

            var gridview = document.getElementById('<%= gv_consignments.ClientID %>');

            var gridviewRow = gridview.rows[index];

            var chk = gridviewRow.cells[0].childNodes[1];

            var strCodAmount = gridviewRow.cells[9].innerHTML;
            var strVoucherAmount = gridviewRow.cells[10].childNodes[1].value;
            var codType = gridviewRow.cells[10].childNodes[13].value;
            var codAmount = parseFloat(strCodAmount);
            var voucherAmount = parseFloat(strVoucherAmount);

            var tempTotalAmount = 0;
            var temp = 0;
            for (var i = 1; i < gridview.rows.length - 1; i++) {
                var tr = gridview.rows[i];
                var td = tr.cells[10];
                var rowAmount = td.childNodes[1];

                temp = parseFloat(rowAmount.value);
                if (!isNaN(temp)) {
                    tempTotalAmount = tempTotalAmount + temp;
                }
                else {
                    rowAmount.value = '';
                }

            }

            gridview.rows[gridview.rows.length - 1].cells[10].innerText = tempTotalAmount.toString(); //.childNodes[0].value = tempTotalAmount.toString();.childNodes[0].value = tempTotalAmount.toString();
            if (txt.parentNode.cellIndex == 10) {

                if (voucherAmount <= 0 || isNaN(voucherAmount)) {
                    alert("Voucher amount cannot be less than or equal to Zero(0)");
                    gridviewRow.bgColor = 'LightPink';
                    success = false;
                    return;
                }
                else if (Math.floor(voucherAmount) != Math.floor(codAmount) && codType != "2") {
                    alert("Voucher Amount must equal COD Amount");
                    gridviewRow.bgColor = '#fc4444';
                    success = false;
                }
                else {
                    success = true;
                    //tempTotalAmount = tempTotalAmount + voucherAmount;
                    //gridview.rows[gridview.rows.length - 1].cells[10].innerText = tempTotalAmount.toString(); //.childNodes[0].value = tempTotalAmount.toString();
                    gridviewRow.bgColor = 'White';
                }
            }

            //return false;

        }

        //function changeamount() {
        //    var cashamount = document.getElementById('ContentPlaceHolder1_gv_consignments_footerTotal').value - document.getElementById('ECAmount');
        //    if (cashamount < 0) {
        //        document.getElementById('CashAmount').value = 0;
        //    }
        //    else {
        //        document.getElementById('CashAmount').value = cashamount;
        //    }
            
        //}

        function checkAmountsOnSave() {
            debugger;
            var loader = document.getElementById('<%= loader.ClientID %>');
            loader.style.display = 'block';
            var grid = document.getElementById('<%= gv_consignments.ClientID %>');
            var count = 0;

            var sessionValue = '<%= Session["U_NAME"] %>';
            var usrName = "affaq.qamar@mulphilog.com";
            if (grid != null) {
                var a = a;
                if (grid.rows.length > 1) {
                    debugger;
                    for (var i = 1; i < grid.rows.length; i++) {
                        var chk = grid.rows[i].cells[0].childNodes[1];
                        if (chk.checked) {
                            count = count + 1;
                            var txt = grid.rows[i].cells[10].childNodes[1];
                            checkAmount(txt);
                            if (sessionValue.toUpperCase() == usrName.toUpperCase() && success == true) {
                                var ddl_payment = grid.rows[i].cells[11].childNodes[1];
                                var value = ddl_payment.value;
                                var txt_refNum = grid.rows[i].cells[12].childNodes[1];
                                var gridviewRow = grid.rows[i];

                                debugger;
                                if ((value == '6' && (txt_refNum.value == '' || txt_refNum.value == null)) || success == false) {
                                    debugger;
                                    if (success) {
                                        alert("Reference Number is Cumpolsory for QR at row: " + i.toString());
                                    }
                                    gridviewRow.bgColor = 'LightPink';
                                    loader.style.display = 'none';
                                    return false;
                                }
                                else {
                                    gridviewRow.bgColor = 'White';
                                }
                            }
                            else {
                                if (success == false) {
                                    loader.style.display = 'none';
                                    return false;
                                }
                            }
                        }
                    }
                    var c = count;
                    debugger;
                    if (count == 0) {
                        debugger;
                        alert('Select Consignment(s) to generate Voucher');
                        loader.style.display = 'none';
                        return false;
                    }
                    else {
                        return true;
                    }

                }
                else {
                    loader.style.display = 'none';
                    return false;
                }
            }
            else {
                loader.style.display = 'none';
                return false;
            }
        }
        function checkAll(chk) {
            var gridview = document.getElementById('<%= gv_consignments.ClientID %>');
            for (var i = 1; i < gridview.rows.length; i++) {
                var checkbox = gridview.rows[i].cells[0].childNodes[1];
                if (chk.checked) {
                    checkbox.checked = true;
                }
                else {
                    checkbox.checked = false;
                }
            }
        }


        function onPaymentSourceChange(ddl) {
            var rowData = ddl.parentNode.parentNode;
            var rowIndex = rowData.rowIndex;
            var GridView = document.getElementById('<%= gv_consignments.ClientID %>');
            var value = ddl.options[ddl.selectedIndex].value;


            var txt_refNum = rowData.cells[12].getElementsByTagName("input")[0];
            var ddl_adjusment = rowData.cells[12].getElementsByTagName("select")[0];
            if (value == '1' || value == '6') {
                txt_refNum.style.display = '';
                ddl_adjusment.style.display = 'none';
            }
            else if (value == '8') {
                txt_refNum.style.display = 'none';
                ddl_adjusment.style.display = '';
            }

            return true;
        }

    </script>
    <style>
        .input-field.rabi > input {
            width: 10%;
        }
    </style>
    <div runat="server" id="loader" style="background-color: honeydew; float: left; height: 100%; opacity: 0.7; position: absolute; text-align: center; display: none; top: 11%; width: 84% !important; padding-top: 300px;">
        <div class="loader">
            <img src="../images/Loading_Movie-02.gif" style="top: 300px !important;" />
        </div>
    </div>
    <noscript style="background-color: honeydew; float: left; height: 100%; opacity: 1; position: absolute; text-align: center; top: 11%; width: 84% !important; padding-top: 300px;">
        <div>
            You must enable javascript to continue.
        </div>
    </noscript>
    <asp:UpdatePanel ID="panel1111" runat="server">
        <ContentTemplate>
            <div style="text-align: center; font-size: medium; font-weight: bold; width: 100%; padding-left: 20px;">
                <asp:Label ID="Errorid" runat="server"></asp:Label>
            </div>
            <asp:Panel ID="panel1" runat="server">
                <table style="font-family: Calibri; font-size: medium; padding-bottom: 0px !important; padding-top: 0px !important; width: 97%"
                    class="input-form">
                    <tr style="float: none !important;">
                        <td style="float: none !important; font-variant: small-caps !important; width: 200px; padding-bottom: 5px !important; font-size: large; text-align: center;">
                            <b>Information</b>
                        </td>
                    </tr>
                    <tr>
                        <td class="field">Payment Mode
                        </td>
                        <td class="input-field" style="text-align: left !important;">
                            <asp:RadioButton ID="rbtn_paymentMode" runat="server" Width="50%" Text="Cash" Checked="true" />
                        </td>
                        <td class="space"></td>
                        <td class="field">Date
                        </td>
                        <td class="input-field">
                            <asp:Label ID="txt_date" runat="server"></asp:Label>
                            <%--<asp:TextBox ID="txt_date" runat="server"></asp:TextBox>
                            <Ajax1:CalendarExtender ID="calendar1" runat="server" TargetControlID="txt_date"
                                Animated="true" Format="yyyy-MM-dd">
                            </Ajax1:CalendarExtender>--%>
                        </td>
                        <td class="space"></td>
                    </tr>
                    <tr>
                        <td class="field">Payment Source
                        </td>
                        <td class="input-field">
                            <asp:DropDownList ID="dd_paymentSource" runat="server" Width="100%">
                                <asp:ListItem Value="5">COD Payment</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="field">Runsheet Number
                        </td>
                        <td class="input-field">
                            <asp:TextBox ID="txt_runsheetNumber" runat="server" Width="95%"></asp:TextBox>
                        </td>

                        <td class="input-field">
                            <div>
                                <asp:TextBox ID="txt_riderCode" runat="server" Width="95%" Visible="false"></asp:TextBox>
                                <asp:ImageButton Width="20px" Height="20px" ImageUrl="~/images/Refresh-512.png" AlternateText="Change Runsheet Number" ID="btn_changeRunsheer" OnClick="btn_changeRunsheer_Click" runat="server" />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="field" style="text-align: center; width: 100%;">
                            <asp:Button ID="btn_search" runat="server" CssClass="button" Width="25%" Text="S E A R C H"
                                OnClick="btn_search_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td style="background-color: #fc4444; width: 10%; float: left;">&nbsp;
                        </td>
                        <td colspan="4" style="width: 70%; float: left;">Voucher Amount Not Equal COD Amount
                        </td>
                    </tr>
                    <tr>
                        <td style="background-color: LightPink; width: 10%; float: left;">&nbsp;
                        </td>
                        <td colspan="4" style="width: 70%; float: left;">Voucher Amount 0
                        </td>
                    </tr>
                </table>
            </asp:Panel>

             <div class="field" style="padding-left: 35px;">
               <br /><br />
                <div class="input-field">
                    <asp:Label runat="server"><b>Amount from EC</b></asp:Label>
                    <asp:TextBox type="number" colspan="4" ID="ECAmount" OnTextChanged="changeamount" onkeydown="return (event.keyCode!=13);" AutoPostBack="true" runat="server" Width="50%"></asp:TextBox>
                </div>
                <div class="input-field">
                    <asp:Label runat="server"><b>Amount from cash</b></asp:Label>
                    <asp:TextBox type="number" colspan="3" ID="CashAmount" disabled="disbaled" runat="server" Width="50%"></asp:TextBox>
                    
                </div>
                 <div>
                         <asp:Button ID="btn_save2" runat="server" Text="Save" CssClass="button" CommandName="first"
                    OnClientClick="if(!checkAmountsOnSave()) { return false;}" OnClick="btn_save_Click" />
                  <br /><br />  </div>
               
            </div>
            <div class="tbl-summary" style="visibility: visible;padding-left: 35px;" >
                <br /><br />
                <asp:GridView ID="gv_summary" runat="server" AutoGenerateColumns="false" CssClass="mGrid">
                    <Columns>
                        <asp:BoundField HeaderText="Rider Name" DataField="RiderName" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="Deposit at EC" DataField="Amount" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="Submit with Cashier" DataField="SubmitwithCashier" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="Difference" DataField="Difference" ItemStyle-HorizontalAlign="Center" />
                    </Columns>
                </asp:GridView>
                <br />
            </div>

           

            <div class="tbl-large">
                <asp:GridView ID="gv_consignments" runat="server" AutoGenerateColumns="false" CssClass="mGrid"
                    OnRowDataBound="gv_consignments_RowDataBound" ShowFooter="true">
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:CheckBox ID="chk_checkAll" runat="server" AutoPostBack="true" OnCheckedChanged="CheckAll" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chk_check" runat="server" AutoPostBack="true" OnCheckedChanged="changeamount" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Consignment No" DataField="CNNo" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="Account" DataField="AccountNumber" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="Account Name" DataField="AccName" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="Delivery Status" DataField="RunsheetReason" />
                        <asp:BoundField HeaderText="VoucherID" DataField="VoucherID" />
                        <asp:BoundField HeaderText="RR Number" DataField="RRNumber" />
                        <asp:BoundField HeaderText="RR Date" DataField="RRDate" />
                        <asp:BoundField HeaderText="RR Entry DateTime" DataField="RREntry" />
                        <asp:BoundField HeaderText="COD Amount" DataField="CodAmount" />
                        <asp:TemplateField HeaderText="Amount" HeaderStyle-Width="50px" ItemStyle-Width="50px"
                            ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:TextBox ID="txt_amount" runat="server" onkeypress="return isNumberKey(event);"
                                    Width="80%" onchange="checkAmount(this);" Style="overflow: hidden" Text='<%# DataBinder.Eval(Container.DataItem, "RRAmount") %>'></asp:TextBox>
                                <asp:HiddenField ID="hd_runsheetNumber" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "RunsheetNumber") %>' />
                                <asp:HiddenField ID="hd_riderCode" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "RiderCode") %>' />
                                <asp:HiddenField ID="hd_expressCenterCode" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "ExpressCenterCode") %>' />
                                <asp:HiddenField ID="hd_creditClientID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "CreditClientID") %>' />
                                <asp:HiddenField ID="hd_voucherExists" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "VoucherExists") %>' />
                                <asp:HiddenField ID="hd_CODType" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "CODType") %>' />
                            </ItemTemplate>
                            <FooterStyle Font-Bold="true" Font-Size="Medium" Font-Names="Calibri" />
                            <FooterTemplate>
                                <%--<asp:TextBox ID="txt_footerTotal" runat="server" ReadOnly="true" Width="80%" Enabled="false"></asp:TextBox>--%>
                                <asp:HyperLink ID="footerTotal" Target="_blank" runat="server" Text='<%# Eval("Keyword") %>' />
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Payment Source" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddl_PaymentSource" runat="server" onchange="onPaymentSourceChange(this);">
                                </asp:DropDownList>
                                <asp:HiddenField ID="hd_PaymentSourceId" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "PaymentSourceId") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Ref Number" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                            <ItemTemplate>
                                <asp:TextBox ID="txt_refNumber" runat="server" onkeypress="return alphanumeric(event);" Visible="true"
                                    Width="80%" Text='<%# DataBinder.Eval(Container.DataItem, "RefNumber") %>' Style="overflow: hidden"></asp:TextBox>
                                <asp:DropDownList ID="ddl_adjustment" runat="server" Style="display: none" Width="90%">
                                    <asp:ListItem Text="Deduction" Value="Deduction" />
                                    <asp:ListItem Text="Write Off" Value="Write Off" />
                                    <asp:ListItem Text="Debit Advice" Value="Debit Advice" />
                                    <asp:ListItem Text="Insurance Claim" Value="Insurance Claim" />
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Remarks" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                            <ItemTemplate>
                                <asp:TextBox ID="txt_Remarks" runat="server" Visible="true"
                                    Width="80%" Style="overflow: hidden" Text='<%# DataBinder.Eval(Container.DataItem, "remarks") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>


            <div style="width: 100%; text-align: center" id="btnDiv" runat="server">
                <asp:Button ID="btn_reset" runat="server" Text="Reset" CssClass="button" OnClick="btn_reset_Click" />
                &nbsp;
                <asp:Button ID="btn_save" runat="server" Text="Save" CssClass="button" CommandName="first"
                    OnClientClick="if(!checkAmountsOnSave()) { return false;}" OnClick="btn_save_Click" />
                &nbsp;
                <asp:Button ID="btn_print" runat="server" Text="Print" CssClass="button" CommandName="first"
                    OnClientClick="if(!checkAmountsOnSave()) { return false;}" OnClick="btn_print_Click" />
                &nbsp;
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
