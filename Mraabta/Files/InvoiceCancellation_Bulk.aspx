<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="InvoiceCancellation_Bulk.aspx.cs" Inherits="MRaabta.Files.InvoiceCancellation_Bulk" %>

<%@ Register TagPrefix="Ajax1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script>
        function FormValidation() {
            debugger;
            var branches = document.getElementById('<%= dd_branch.ClientID %>');
            var AccountNumber = document.getElementById('<%= txt_accountNo.ClientID %>');

            var branch = branches.options[branches.selectedIndex].value;
            if (branch == 0) {
                alert('Select Proper Branch');
                return false;
            }
            if (AccountNumber.value == "") {
                alert('Enter Account Number');
                return false;
            }

            return true;
        }
        function BranchChange(ddl) {
            debugger;
            document.getElementById('<%= txt_accountNo.ClientID %>').value = "";
            document.getElementById('<%= lbl_accountName.ClientID %>').value = "";
        }
    </script>
    <table width="100%" style="font-family: Calibri;">
        <tr>
            <td style="text-align: center; font-variant: small-caps; font-size: large;" colspan="7">
                <b>Bulk Invoice Cancelation</b>
            </td>
        </tr>
        <tr>
            <td style="width: 20%">
            </td>
            <td style="width: 10%">
                <b>Year</b>
            </td>
            <td style="width: 15%">
                <asp:DropDownList ID="dd_year" runat="server" AutoPostBack="true" AppendDataBoundItems="true">
                    <asp:ListItem Value="0">Select Year</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td style="width: 10%">
            </td>
            <td style="width: 10%">
                <b>Month</b>
            </td>
            <td style="width: 15%">
                <asp:DropDownList ID="dd_month" runat="server" AutoPostBack="true">
                    <asp:ListItem Value="0">Select Month</asp:ListItem>
                    <asp:ListItem Value="01">January</asp:ListItem>
                    <asp:ListItem Value="02">February</asp:ListItem>
                    <asp:ListItem Value="03">March</asp:ListItem>
                    <asp:ListItem Value="04">April</asp:ListItem>
                    <asp:ListItem Value="05">May</asp:ListItem>
                    <asp:ListItem Value="06">June</asp:ListItem>
                    <asp:ListItem Value="07">July</asp:ListItem>
                    <asp:ListItem Value="08">August</asp:ListItem>
                    <asp:ListItem Value="09">September</asp:ListItem>
                    <asp:ListItem Value="10">October</asp:ListItem>
                    <asp:ListItem Value="11">November</asp:ListItem>
                    <asp:ListItem Value="12">December</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td style="width: 20%">
            </td>
        </tr>
        <tr>
            <td style="width: 20%">
            </td>
            <td style="width: 10%">
                <b>Zone</b>
            </td>
            <td style="width: 15%">
                <asp:DropDownList ID="dd_zone" runat="server" AutoPostBack="true" Width="95%" OnSelectedIndexChanged="dd_zone_SelectedIndexChanged"
                    AppendDataBoundItems="true">
                    <asp:ListItem Value="0">Select Zone</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td style="width: 10%">
            </td>
            <td style="width: 10%">
                <b>Branch</b>
            </td>
            <td style="width: 15%">
                <asp:DropDownList ID="dd_branch" runat="server" Width="95%" AutoPostBack="false"
                    OnSelectedIndexChanged="dd_branch_SelectedIndexChanged" AppendDataBoundItems="true">
                    <asp:ListItem Value="0">Select Branch</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td style="width: 20%">
            </td>
        </tr>
        <tr>
            <td style="width: 20%">
            </td>
            <td style="width: 10%">
                <b>Account No</b>
            </td>
            <td style="width: 15%">
                <asp:TextBox ID="txt_accountNo" runat="server" Width="95%" OnTextChanged="txt_accountNo_TextChanged"
                    AutoPostBack="true"></asp:TextBox>
                <asp:HiddenField ID="hd_creditClientID" runat="server" />
            </td>
            <td style="width: 10%">
            </td>
            <td style="width: 10%">
                <b>Account Name</b>
            </td>
            <td style="width: 15%">
                <asp:Label ID="lbl_accountName" runat="server" onchange="if(!FormValidation()) { return false;}"></asp:Label>
            </td>
            <td style="width: 20%">
            </td>
        </tr>
        <tr>
            <td style="width: 20%">
            </td>
            <td style="width: 10%">
            </td>
            <td style="width: 35%; text-align: center;" colspan="3">
                <asp:Button ID="btn_GetInvoices" runat="server" Text="GET INVOICES" CssClass="button"
                    Width="50%" OnClick="btn_GetInvoices_Click" />
            </td>
            <td style="width: 15%">
            </td>
            <td style="width: 20%">
            </td>
        </tr>
        <tr>
            <td colspan="7" style="text-align: left; font-variant: small-caps; font-size: medium;">
                <b>Legend:</b>
                <asp:Label ID="lbl_legend1" runat="server" BackColor="#DB7093">Redeemed Invoice</asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="7" style="text-align: center; font-variant: small-caps; font-size: large;">
                <br />
                <br />
                <b>List of Invoices</b>
            </td>
        </tr>
        <tr>
            <td colspan="7">
            </td>
        </tr>
    </table>
    <div style="overflow: scroll; height: 300px; width: 100%">
        <div style="width: 100%; margin-top: 20px;" class="tbl-large">
            <asp:GridView ID="gv_invoices" runat="server" AutoGenerateColumns="false" Width="100%"
                CssClass="mGrid" BorderColor="#DEDFDE" OnRowDataBound="gv_invoices_RowDataBound">
                <RowStyle BorderStyle="None" />
                <Columns>
                    <asp:TemplateField ItemStyle-Width="10px">
                        <ItemTemplate>
                            <asp:CheckBox ID="chk_cancel" runat="server" />
                            <asp:HiddenField ID="hd_redeemNumber" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "irNumber") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Invoice Number" DataField="InvoiceNumber" />
                    <asp:BoundField HeaderText="Invoice Date" DataField="invoiceDate" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField HeaderText="Start Date" DataField="startDate" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField HeaderText="End Date" DataField="endDate" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField HeaderText="Total Amount" DataField="totalAmount" DataFormatString="{0:N2}"
                        ItemStyle-HorizontalAlign="Right" />
                    <asp:TemplateField HeaderText="Reason">
                        <ItemTemplate>
                            <asp:DropDownList ID="dd_gReason" runat="server" AppendDataBoundItems="true">
                                <asp:ListItem Value="0">Select Reason</asp:ListItem>
                            </asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="AccountNo" DataField="AccountNO" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField HeaderText="AccountName" DataField="AccountName" />
                    <asp:BoundField HeaderText="Branch" DataField="BranchName" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
    <div style="width: 100%; text-align: center; height: 15px;">
        <asp:Button ID="btn_cancel" runat="server" CssClass="button" Text="Cancel Invoice(s)"
            OnClick="btn_cancel_Click" />
        &nbsp;
        <asp:Button ID="btn_reset" runat="server" Text="Reset" CssClass="button" OnClick="btn_reset_Click" />
    </div>
</asp:Content>
