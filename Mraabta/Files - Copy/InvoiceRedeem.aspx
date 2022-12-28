<%@ Page Title="" Language="C#" MasterPageFile="~/BtsMasterPage.Master" AutoEventWireup="true" CodeBehind="InvoiceRedeem.aspx.cs" Inherits="MRaabta.Files.InvoiceRedeem" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="font-family: Calibri; font-size: medium; font-weight: bold;">
        <asp:Label ID="Errorid" runat="server"></asp:Label>
    </div>
    <script type="text/javascript">

        function Update_() {

        }
        function Update() {

            debugger;
            var balAmt = document.getElementById("<%= txt_balAmt.ClientID %>").value;
             var inv = document.getElementById("<%= txt_invoiceNo.ClientID %>").value;
             var redeemAmt = document.getElementById("<%= txt_redeemAmt.ClientID %>").value;
             var grid1 = document.getElementById("<%= gv_invoices.ClientID %>");

             var gv = document.getElementById("<%=gv_invoices.ClientID %>");
             var gvRowCount = gv.rows.length;
             var found = false;
             var rwIndex = 1;
             for (rwIndex; rwIndex <= gvRowCount - 1; rwIndex++) {


                 if (gv.rows[rwIndex].cells[0].childNodes[1].innerHTML == inv) {
                     found = true;
                     //alert(gv.rows[rwIndex].cells[0].childNodes[1].innerHTML);
                     var txtbx = gv.rows[rwIndex].cells[6].children[0];
                     var outstandingAmt = gv.rows[rwIndex].cells[5].innerHTML;
                     //alert(outstandingAmt);
                     debugger
                     if (Number(redeemAmt) > Number(outstandingAmt)) {
                         alert('Redeem Amount Cannot be Greater than OutStanding Amount');
                         document.getElementById("<%= txt_redeemAmt.ClientID %>").value = "";
                    }
                    else {

                        if (Number(balAmt) - Number(redeemAmt) >= 0) {
                            var prevValue = txtbx.value;
                            if (prevValue == "" || prevValue == "0") {
                                prevValue = 0;
                            }
                            txtbx.value = redeemAmt;
                            txtbx.focus();

                            balAmt = balAmt - (redeemAmt - prevValue);

                            document.getElementById("<%= txt_balAmt.ClientID %>").value = balAmt;
                            document.getElementById("<%= txt_invoiceNo.ClientID %>").value = "";
                            document.getElementById("<%= txt_redeemAmt.ClientID %>").value = "";
                            document.getElementById("<%= txt_invoiceNo.ClientID %>").focus();
                            console.write(outstandingAmt);
                        }
                        else {
                            alert('Redeem Amount Cannot be Greater Than Balance Amount');
                        }

                    }

                }
            }
            if (!found) {
                alert('Invoice Not Found');
            }


        }
        function isNumberKey(evt) {

            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode != 46 && charCode > 31
                && (charCode < 48 || charCode > 57 || charCode == 110 || charCode == 109 || charCode == 45 || charCode == 9)) {
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

    </script>
    <script type="text/javascript">

        function preventMultipleSubmissions() {
            $('#<%=btn_save.ClientID %>').prop('disabled', true);
        }

        window.onbeforeunload = preventMultipleSubmissions;

    </script>
    <table style="font-family: Calibri; font-size: medium; padding-bottom: 0px !important;
        padding-top: 0px !important; width: 97%" class="input-form">
        <tr style="float: none !important;">
            <td style="float: none !important; font-variant: small-caps !important; width: 200px;
                padding-bottom: 5px !important; font-size: large; text-align: center;">
                <b>Information</b>
            </td>
        </tr>
        <tr>
            <td class="field" style="width: 10% !important;">
                Pay. VNo.
            </td>
            <td class="input-field" style="width: 15%">
                <asp:TextBox ID="txt_payVno" runat="server" AutoPostBack="true" OnTextChanged="txt_payVno_TextChanged"></asp:TextBox>
            </td>
            <td class="space" style="width: 4% !important; margin: 0px 0px 0px 0px !important;">
            </td>
            <td class="field" style="width: 14% !important;">
                Client A/C #
            </td>
            <td class="input-field" style="width: 15%">
                <asp:TextBox ID="txt_clientAccNo" runat="server" Enabled="false"></asp:TextBox>
            </td>
            <td class="space" style="width: 4% !important; margin: 0px 0px 0px 0px !important;">
            </td>
            <td class="field" style="width: 14% !important;">
                Client Name
            </td>
            <td class="input-field" style="width: 15%">
                <asp:TextBox ID="txt_clientName" runat="server" Enabled="false"></asp:TextBox>
            </td>
            <td class="space" style="width: 5% !important; margin: 0px 0px 0px 0px !important;">
            </td>
        </tr>
        <tr>
            <td class="field" style="width: 10% !important;">
                Cheque #
            </td>
            <td class="input-field" style="width: 15%">
                <asp:TextBox ID="txt_chequeNo" runat="server" Enabled="false"></asp:TextBox>
            </td>
            <td class="space" style="width: 4% !important; margin: 0px 0px 0px 0px !important;">
            </td>
            <td class="field" style="width: 14% !important;">
                Voucher Amount
            </td>
            <td class="input-field" style="width: 15%">
                <asp:TextBox ID="txt_vAmt" runat="server" Text="0" Enabled="false" Style="text-align: right;"></asp:TextBox>
            </td>
            <td class="space" style="width: 4% !important; margin: 0px 0px 0px 0px !important;">
            </td>
            <td class="field" style="width: 14% !important;">
                Balance Amount
            </td>
            <td class="input-field" style="width: 15%">
                <asp:TextBox ID="txt_balAmt" runat="server" Text="0" Enabled="false" Style="text-align: right;"></asp:TextBox>
            </td>
            <td class="space" style="width: 5% !important; margin: 0px 0px 0px 0px !important;">
            </td>
        </tr>
    </table>
    <table style="font-family: Calibri; font-size: medium; padding-bottom: 0px !important;
        padding-top: 0px !important; width: 97%; box-shadow: 0 0 0px #000 !important;
        border-radius: 0px !important;" class="input-form">
        <tr>
            <td class="field" style="width: 10% !important;">
                Invoice #
            </td>
            <td class="input-field" style="width: 15%">
                <asp:TextBox ID="txt_invoiceNo" runat="server" Enabled="true" ></asp:TextBox>
            </td>
            <td class="space" style="width: 4% !important; margin: 0px 0px 0px 0px !important;">
            </td>
            <td class="field" style="width: 14% !important;">
                Redeem Amount
            </td>
            <td class="input-field" style="width: 15%">
                <asp:TextBox ID="txt_redeemAmt" runat="server" Enabled="true" Style="text-align: right;" ></asp:TextBox>
            </td>
            <td class="space" style="width: 4% !important; margin: 0px 0px 0px 0px !important;">
            </td>
            <td class="input-field" style="width: 15%; text-align: center !important;">
                <input type="button" class="button" name="Add" value="Add" onclick="Update()" style="width: 75%" />
                <%--<asp:Button ID="btn_add" runat="server" Text="Add" CssClass="button" Width="75%"
                    OnClick="btn_add_Click" OnClientClick="Update()" />--%>
            </td>
            <td class="space" style="width: 5% !important; margin: 0px 0px 0px 0px !important;">
            </td>
        </tr>
    </table>
    <div style="width: 100%; height: 250px; overflow: scroll; text-align: center;">
        <span id="Span1" class="tbl-large">
            <asp:Label ID="Label1" runat="server" CssClass="error_msg"></asp:Label>
            <asp:GridView ID="gv_invoices" runat="server" AutoGenerateColumns="false" CssClass="mGrid"
                AlternatingRowStyle-CssClass="alt" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px"
                Width="97%" EmptyDataText="No Data Available">
                <RowStyle Font-Bold="false" />
                <Columns>
                    <%--<asp:BoundField HeaderText="Vno" DataField="Vno" />--%>
                    <asp:TemplateField HeaderText="Invoice #">
                        <ItemTemplate>
                            <asp:Label ID="lbl_gInvoiceNo" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "invoiceNumber") %>'
                                CssClass="gridLabel"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--<asp:BoundField HeaderText="Invoice #" DataField="invoiceNumber" />--%>
                    <asp:BoundField HeaderText="Branch" DataField="Branch" />
                    <asp:BoundField HeaderText="Month" DataField="Month" />
                    <asp:BoundField HeaderText="Company" DataField="CompanyName" />
                    <asp:BoundField HeaderText="Total Amt" DataField="Total_Amount" />
                    <asp:BoundField HeaderText="Outstanding Amt" DataField="Oustanding" />
                    <asp:TemplateField HeaderText="Redeem Amt">
                        <ItemTemplate>
                            <asp:TextBox ID="txt_gRedeemAmt" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "RedeemAmt") %>' Enabled="false"
                                CssClass="gridTextBox"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--<asp:BoundField HeaderText="Redeem Amt" DataField="RedeemAmt" />--%>
                </Columns>
            </asp:GridView>
        </span>
    </div>
    <div style="width: 100%; text-align: center">
        <asp:Button ID="btn_reset" runat="server" Text="Reset" CssClass="button" OnClick="btn_reset_Click" UseSubmitBehavior="false" />
        &nbsp;
        <asp:Button ID="btn_save" runat="server" Text="Save" CssClass="button" OnClick="btn_save_Click" OnClientClick="this.disabled=true;"  UseSubmitBehavior="false" />
        &nbsp;
    </div>
</asp:Content>
