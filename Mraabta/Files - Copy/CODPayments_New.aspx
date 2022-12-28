<%@ Page Title="COD Payment" Language="C#" MasterPageFile="~/BtsMasterPage.master" AutoEventWireup="true" CodeBehind="CODPayments_New.aspx.cs" Inherits="MRaabta.Files.CODPayments_New" %>

<%@ Register TagPrefix="Ajax1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" src="<%=ResolveUrl("~/Scripts/jquery-3.5.1.min.js") %>"></script>

    <script type="text/javascript">
        function ShowData() {
            var Zone = document.getElementById('<%= dd_zone.ClientID %>');
            var Customer = document.getElementById('<%= dd_customer.ClientID %>');
            var Condition_amount = document.getElementsByClassName('txt_condition_amount')[0].value

            if (isNaN(Zone.value)) {
                alert('Select Zone');
                return;
            }
            $.ajax({
                type: "POST",
                url: "CODPayments_New.aspx/GetPaymentData",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: "{'zone':'" + Zone.value + "', 'customer':'" + Customer.value + "', 'payable_amount':'" + Condition_amount + "'}",
                beforeSend: function () {
                    document.getElementById('tblpayment').style.display = 'none';
                    document.getElementById('ContentPlaceHolder1_loaders').style.display = 'block';
                },
                success: function (response) {
                    var Table = $('#tblpayment tbody');
                    Table.empty();
                    var Data = response.d;
                    var tbody = '';
                    var serial = 0;
                    for (let y = 0; y < Data.length; y++) {
                        serial++;
                        if (Data[y].NetPayable < 0) {
                            tbody += `<tr style="background: red;color: #fff;">
                                      <td ></td>`;
                        }
                        else {
                            tbody += `<tr>
                                      <td align="center" id="CODPaymentCheck${Data[y].AccountNo}"><input type='checkbox'  name='CODPaymentCheck' class='CODPaymentCheck' value='${Data[y].AccountNo}'/></td>`;
                        }
                        tbody += `<td align="center">${serial}</td>
                            <td align="center">${Data[y].AccountNo}</td>
                            <td align="left">${Data[y].CustomerName}</td>
                            <td align="center">${formatNumber(Data[y].CODPayable)}</td>
                            <td align="center">${formatNumber(Data[y].Oustanding)}</td>
                            <td align="center">${formatNumber(Data[y].NetPayable)}</td>
                            <td align="center" id='Download${Data[y].AccountNo}'></td>
                                </tr >`;
                    }
                    $('#tblpayment tbody').append(tbody);

                },
                complete: function () {

                    document.getElementById('ContentPlaceHolder1_loaders').style.display = 'none';
                    document.getElementById('tblpayment').style.display = 'block';
                    document.getElementById('savebtn').style.display = 'block';
                },
                failure: function (jqXHR, textStatus, errorThrown) {
                    alert("HTTP Status: " + jqXHR.status + "; Error Text: " + jqXHR.responseText); // Display error message  
                }
            });
        }


        function SaveData() {
            var Zone = document.getElementById('<%= dd_zone.ClientID %>');
            var radioButtons = document.getElementById('<%= paidstatus.ClientID %>');

            var selectedValue = "";
            for (var i = 0; i < radioButtons.rows[0].cells.length; i++) {
                if (radioButtons.rows[0].cells[i].children[0].checked) {
                    selectedValue = radioButtons.rows[0].cells[i].children[0].defaultValue;
                }
            }

            //var checkedValue = [];
            //var inputElements = document.getElementsByClassName('CODPaymentCheck');
            //for (var i = 0; inputElements[i]; ++i) {
            //    if (inputElements[i].checked) {
            //        checkedValue[i] = inputElements[i].value;
            //    }
            //}
            var checkboxes = document.getElementsByClassName("CODPaymentCheck");
            var checkedValue = Array.prototype.slice.call(checkboxes).filter(ch => ch.checked == true);

            debugger;


            if (checkedValue.length == 0) {
                alert('Select Account Number Checkbox to process the Payment...');
            }

            for (var j = 0; j < checkedValue.length; j++) {
                debugger;
                $.ajax({
                    type: "POST",
                    url: "CODPayments_New.aspx/SaveCustomerPaymentData",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: "{'zone':'" + Zone.value + "', 'customer':'" + checkedValue[j].defaultValue + "', 'paidstatus':'" + selectedValue + "'}",

                    beforeSend: function () {
                        document.getElementById('savebtn').style.display = 'none';
                        document.getElementById('<%= btn.ClientID %>').style.display = 'none';
                    },
                    success: function (response) {
                        debugger;

                        var Data = response.d.ListPayments;
                        var PaymentId = response.d.PaymentId;
                        var Url = window.location.hostname + `/ Mraabta / Files / CODPayments_New_PDF.aspx ? paymentId = ` + PaymentId + '&Check_Condition1=' + response.d.Check_Condition1;

                        var PDFURL = window.location.hostname + PDFURL + PaymentId;
                        document.getElementById('CODPaymentCheck' + response.d.Account).innerHTML = '';
                        document.getElementById('Download' + response.d.Account).innerHTML = `${PaymentId}`;
                    },
                    complete: function () {
                        document.getElementById('savebtn').style.display = 'block';
                        document.getElementById('<%= btn.ClientID %>').style.display = 'block';
                    },
                    failure: function (jqXHR, textStatus, errorThrown) {
                        alert("HTTP Status: " + jqXHR.status + "; Error Text: " + jqXHR.responseText); // Display error message  
                    }
                });
            }
        }


        function formatNumber(num) {
            return num.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,")
        }

        function toggle(source) {
            checkboxes = document.getElementsByName('CODPaymentCheck');
            for (var i = 0, n = checkboxes.length; i < n; i++) {
                checkboxes[i].checked = source.checked;
            }
        }
    </script>

    <script type="text/javascript">
        function CriteriaChange() {

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
        .outer_box {
            background: #444 none repeat scroll 0 0;
            height: 101%;
            left: 0;
            opacity: 0.9;
            position: absolute;
            top: -1%;
            width: 100%;
        }

        .loader {
            border: 16px solid #f3f3f3;
            border-radius: 50%;
            border-top: 16px solid #3498db;
            width: 120px;
            height: 120px;
            -webkit-animation: spin 2s linear infinite;
            animation: spin 2s linear infinite;
            left: 43%;
            position: relative;
            top: 43%
        }

        @-webkit-keyframes spin {
            0% {
                -webkit-transform: rotate(0deg);
            }

            100% {
                -webkit-transform: rotate(360deg);
            }
        }

        @keyframes spin {
            0% {
                transform: rotate(0deg);
            }

            100% {
                transform: rotate(360deg);
            }
        }

        .detail {
            border-radius: 10px;
            box-shadow: 0 0 1px rgb(0, 0, 0);
            left: 5%;
            margin: 0;
            overflow: hidden;
            padding: 0;
            position: relative;
            width: 90%;
        }

        #tblpayment {
            font-family: Arial, Helvetica, sans-serif;
            border-collapse: collapse;
            width: 70%;
            margin: 0 auto;
        }

            #tblpayment td, #tblpayment th {
                border: 1px solid #ddd;
                padding: 8px;
            }

            #tblpayment tr:nth-child(even) {
                background-color: #f2f2f2;
            }

            #tblpayment tr:hover {
                background-color: #ddd;
            }

            #tblpayment th {
                padding-top: 12px;
                padding-bottom: 12px;
                text-align: left;
                background-color: #eee;
                color: #000;
            }
    </style>
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <div>
                <asp:Label ID="Errorid" runat="server"></asp:Label>
                <table cellpadding="0" cellspacing="0" style="width: 97% !important;" class="input-form">
                    <tr style="margin: 0;">
                        <td class="input-field" style="width: 14% !important">
                            <div>
                                Zone
                            </div>
                            <div>
                                <asp:DropDownList ID="dd_zone" runat="server" SelectionMode="Single" CssClass="dropdown"
                                    AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="branch_SelectedIndexChanged" Style="width: 80%;">
                                    <asp:ListItem Value="0">--- Select Zone---</asp:ListItem>

                                </asp:DropDownList>
                            </div>
                        </td>
                        <td class="input-field" style="width: 20% !important; display: none">
                            <div style="height: 109px">
                                <div>
                                    Branch
                                </div>
                                <div style="width: 215px;">
                                    <asp:ListBox ID="dd_branch" runat="server" SelectionMode="Multiple" CssClass="dropdown"></asp:ListBox>
                                    <br />
                                    <asp:CheckBox ID="branch_chk" runat="server" Text="ALL" AutoPostBack="true" OnCheckedChanged="branch_chk_CheckedChanged" />
                                </div>
                            </div>
                        </td>
                        <td class="input-field" style="width: 17% !important; display: none">
                            <div>
                                Account Number (Optional)
                            </div>
                            <div>
                                <input id="txtAccNum" type="text" onchange="SearchByAccountNumber(this);" />
                            </div>
                        </td>
                        <td class="input-field" style="width: 30% !important">
                            <div>
                                Customer Name
                            </div>
                            <div>
                                <asp:DropDownList ID="dd_customer" runat="server" Width="99%">
                                    <asp:ListItem Value="0">Select Customer Name</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </td>
                        <td class="input-field" style="width: 15% !important; margin: 0 0 0 20px;">
                            <div>Net Amount</div>
                            <div>
                                <asp:TextBox ID="txt_condition_amount" CssClass="txt_condition_amount" runat="server"></asp:TextBox>
                            </div>
                        </td>
                        <td class="input-field" style="width: 20% !important;">
                            <div>
                                Operation Criteria
                            </div>
                            <div>
                                <asp:RadioButtonList ID="paidstatus" runat="server" CssClass="OperationCriteria" RepeatColumns="2" RepeatLayout="Table"
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
                        <td class="input-field" style="width: 15% !important; display: none" colspan="2">
                            <div>
                                Output Type
                            </div>
                            <div style="width: 100% !important;">
                                <asp:DropDownList ID="type" runat="server" Width="90%">
                                    <asp:ListItem Value="">Select Output Type</asp:ListItem>
                                    <asp:ListItem Value="html" Selected="True">HTML</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </td>
                        <td class="input-field" style="width: 10% !important;">
                            <input type="button" class="button" value="Show Data" onclick="ShowData()" />
                        </td>
                    </tr>
                    <tr style="display: none">

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

                <div id="loaders" runat="server" class="outer_box" style="display: none;">
                    <div id="loader" runat="server" class="loader">
                    </div>
                </div>

                <asp:Label ID="error_msg" CssClass="error_msg" runat="server"></asp:Label>

                <span id="Table_1" class="tbl-large" style="float: left; width: 100%; position: relative; top: 44%;">
                    <asp:Button ID="btn" runat="server" Text="Download All Files" class="button" OnClick="DownloadFiles" Style="display: none" />

                    <table id="tblpayment" class="mGrid" style="margin-top: 10px; display: none;">
                        <thead class="bg-danger text-center text-white">
                            <tr>
                                <th width="5%" align="center">
                                    <input type="checkbox" onclick="toggle(this)" /></th>
                                <th width="5%" align="center">ID</th>
                                <th width="10%" align="center">ACCOUNT NO</th>
                                <th width="50%">CUSTOMER NAME</th>
                                <th width="10%" align="center">COD Payable</th>
                                <th width="10%" align="center">Outstanding</th>
                                <th width="10%" align="center">NET PAYABLE</th>
                                <th width="10%" align="center">Payment Id</th>
                            </tr>
                        </thead>
                        <tbody></tbody>
                    </table>

                    <br />
                    <br />
                    <div style="margin: 0 auto; width: 70%">
                        <input type="button" class="button" id="savebtn" value="Save Data" onclick="SaveData()" style="display: none" />
                    </div>
                </span>


                <div id="Tabelabc" class="tbl-large" runat="server" style="float: left; width: 100% !important; height: 200px !important; overflow-y: scroll; display: none">
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
                    <%--<asp:Button ID="btn_save" runat="server" Text="Save Changes" CssClass="button" OnClick="btn_save_new_Click" />&nbsp;
                    <asp:Button ID="btn_reset" runat="server" Text="Reset" CssClass="button" OnClick="btn_reset_Click" />&nbsp;
                    <asp:Button ID="Button4" runat="server" Text="Print" CssClass="button" OnClick="Button4_Click" />--%>
                </div>
                <div id="div2" class="tbl-large" runat="server" style="width: 100%; height: 360px; overflow: scroll;">
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
                    <%--<asp:Button ID="Button2" runat="server" Text="Save Changes" CssClass="button" OnClick="btn_save_new_Click" />&nbsp;
                    <asp:Button ID="Button3" runat="server" Text="Reset" CssClass="button" OnClick="btn_reset_Click" />&nbsp;
                    <asp:Button ID="btn_print" runat="server" Text="Print" CssClass="button" OnClick="Button4_Click" />--%>
                </div>
            </div>
        </ContentTemplate>
        <%-- <Triggers>
            <asp:PostBackTrigger ControlID="Button4" />
            <asp:PostBackTrigger ControlID="btn_print" />
            <asp:PostBackTrigger ControlID="Button2" />
            <asp:PostBackTrigger ControlID="btn_save" />
            <asp:PostBackTrigger ControlID="GridView2" />
            <asp:PostBackTrigger ControlID="Button1" />
        </Triggers>--%>
    </asp:UpdatePanel>
</asp:Content>
