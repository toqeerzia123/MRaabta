<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="Manage_Invoices.aspx.cs" Inherits="MRaabta.Files.Manage_Invoices" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        function loader() {
            document.getElementById('<%=div2.ClientID %>').style.display = "";
        }
    </script>
    <style>
        .input-form tr
        {
            float: none;
            margin: 0 0 10px;
            width: 100%;
        }
        .outer_box
        {
            background: #444 none repeat scroll 0 0;
            height: 101%;
            left: 0;
            opacity: 0.9;
            position: absolute;
            top: -1%;
            width: 100%;
        }
        
        
        .pop_div
        {
            background: #eee none repeat scroll 0 0;
            border-radius: 10px;
            height: 100px;
            left: 48%;
            position: relative;
            top: 40%;
            width: 235px;
        }
        
        .btn_ok
        {
            background: #000 none repeat scroll 0 0;
            border: 0 none;
            color: #fff;
            left: -18px;
            padding: 1px 14px;
            position: relative;
            top: 67%;
        }
        
        .btn_cancel
        {
            background: #000 none repeat scroll 0 0;
            border: 0 none;
            color: #fff;
            left: 22%;
            padding: 1px 14px;
            position: relative;
            top: 42%;
        }
        
        .pop_div > span
        {
            float: left;
            line-height: 40px;
            text-align: center;
            width: 100%;
        }
        .tbl-large div
        {
            position: static;
        }
        
        .outer_box img
        {
            left: 42%;
            position: relative;
            top: 40%;
        }
    </style>
    <div id="div2" runat="server" class="outer_box" style="display: none; background: #FFFFFF none repeat scroll 0 0;">
        <img src="../images/Loading_Movie-02.gif" />
    </div>
    <div style="width: 100% !important; text-align: center;">
        <asp:Label ID="Errorid" runat="server" ForeColor="Red" Font-Bold="true" Font-Size="Large"></asp:Label>
    </div>
    <table style="font-family: Calibri; font-size: medium; padding-bottom: 0px !important;
        padding-top: 0px !important; width: 97%" class="input-form">
        <tr style="float: none !important;">
            <td class="field" style="width: 10% !important;">
                Sending Option
            </td>
            <td class="input-field" style="width: 15% !important;">
                <asp:CheckBoxList ID="chk_SendingOption" runat="server" RepeatColumns="2" RepeatDirection="Horizontal">
                    <asp:ListItem Value="0" Selected="True">Email</asp:ListItem>
                    <asp:ListItem Value="1" Enabled="false">Fax</asp:ListItem>
                </asp:CheckBoxList>
            </td>
            <td class="space" style="width: 8% !important; margin: 0px 0px 0px 0px !important;">
            </td>
            <td class="field" style="width: 10% !important;">
                Form Format
            </td>
            <td class="input-field" style="width: 15% !important;">
                <asp:RadioButtonList ID="rbtn_formMode" runat="server" RepeatColumns="2" RepeatDirection="Horizontal"
                    Style="text-align: center;" AutoPostBack="true" OnSelectedIndexChanged="rbtn_formMode_SelectedIndexChanged1">
                    <asp:ListItem Value="0" Selected="True">Add New</asp:ListItem>
                    <asp:ListItem Value="1">View</asp:ListItem>
                </asp:RadioButtonList>
            </td>
            <td class="space" style="width: 8% !important; margin: 0px 0px 0px 0px !important;">
            </td>
        </tr>
        <%--<table style="font-family: Calibri; font-size: medium; padding-bottom: 0px !important;
        padding-top: 0px !important; width: 97%" class="input-form">--%>
        <tr style="float: none !important;">
            <td style="float: none !important; font-variant: small-caps !important; width: 200px;
                font-size: large; text-align: center;">
                <b>Information</b>
            </td>
        </tr>
        <tr>
            <td class="field" style="width: 10% !important;">
                Invoice#
            </td>
            <td class="input-field" style="width: 15% !important;">
                <asp:TextBox ID="txt_invoiceNo" runat="server" Enabled="false" AutoPostBack="true"
                    OnTextChanged="txt_invoiceNo_TextChanged"></asp:TextBox>
            </td>
            <td class="space" style="width: 8% !important; margin: 0px 0px 0px 0px !important;">
            </td>
            <td class="field" style="width: 10% !important;">
                Client A/C #
            </td>
            <td class="input-field" style="width: 15% !important;">
                <asp:TextBox ID="txt_clientAccNo" runat="server" OnTextChanged="txt_clientAccNo_TextChanged"
                    AutoPostBack="true"></asp:TextBox>
            </td>
            <td class="space" style="width: 8% !important; margin: 0px 0px 0px 0px !important;">
            </td>
            <td class="field" style="width: 10% !important;">
                Client Name
            </td>
            <td class="input-field" style="width: 15% !important;">
                <asp:TextBox ID="txt_clientName" runat="server" Enabled="false"></asp:TextBox>
            </td>
            <td class="space" style="width: 8% !important; margin: 0px 0px 0px 0px !important;">
            </td>
        </tr>
        <tr>
            <td class="field" style="width: 10% !important;">
                Company
            </td>
            <td class="input-field" style="width: 15% !important;">
                <asp:DropDownList ID="dd_company" runat="server">
                </asp:DropDownList>
            </td>
            <td class="space" style="width: 8% !important; margin: 0px 0px 0px 0px !important;">
            </td>
            <td class="field" style="width: 10% !important;">
                Start Date
            </td>
            <td class="input-field" style="width: 15% !important;">
                <telerik:RadDatePicker ID="pickerStart" runat="server" DateInput-DateFormat="dd/MM/yyyy">
                </telerik:RadDatePicker>
            </td>
            <td class="space" style="width: 8% !important; margin: 0px 0px 0px 0px !important;">
            </td>
            <td class="field" style="width: 10% !important;">
                End Date
            </td>
            <td class="input-field" style="width: 15% !important;">
                <telerik:RadDatePicker ID="pickerEndDate" runat="server" DateInput-DateFormat="dd/MM/yyyy">
                </telerik:RadDatePicker>
            </td>
            <td class="space" style="width: 8% !important; margin: 0px 0px 0px 0px !important;">
            </td>
        </tr>
        <tr>
            <td class="field" style="width: 10% !important;">
            </td>
            <td class="input-field" style="width: 15% !important;">
                <asp:HiddenField ID="hd_customerClientID" runat="server" />
            </td>
            <td class="space" style="width: 8% !important; margin: 0px 0px 0px 0px !important;">
            </td>
            <td class="field" style="width: 10% !important;">
                Invoice Date
            </td>
            <td class="input-field" style="width: 15% !important;">
                <telerik:RadDatePicker ID="pickerInvoiceDate" runat="server" DateInput-DateFormat="dd/MM/yyyy">
                </telerik:RadDatePicker>
            </td>
            <td class="space" style="width: 8% !important; margin: 0px 0px 0px 0px !important;">
            </td>
            <td class="field" style="width: 10% !important;">
            </td>
            <td class="input-field" style="width: 15% !important;">
                <asp:Button ID="btn_getConsignment" runat="server" CssClass="button" Width="100%"
                    Text="Get Consignments" OnClick="btn_getConsignment_Click" OnClientClick="loader()" />
            </td>
            <td class="space" style="width: 8% !important; margin: 0px 0px 0px 0px !important;">
            </td>
        </tr>
    </table>
    <%-- <table style="font-family: Calibri; font-size: medium; padding-bottom: 0px !important;
        padding-top: 0px !important; width: 97%" class="input-form">
        <tr style="float: none !important;">
            <td colspan="9" style="float: none !important;">
                
                   
                </div>
            </td>
        </tr>
    </table>--%>
    <div style="width: 100%; height: 250px; overflow: scroll; text-align: center;">
        <div style="width: 100%; text-align: center; font-variant: small-caps; font-weight: bold;">
            CNs to Invoiced :
            <asp:Label ID="lbl_Count1" runat="server"></asp:Label>
        </div>
        <span id="Span1" class="tbl-large">
            <asp:Label ID="Label1" runat="server" CssClass="error_msg"></asp:Label>
            <asp:GridView ID="gv_cns" runat="server" AutoGenerateColumns="false" CssClass="mGrid"
                AlternatingRowStyle-CssClass="alt" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px"
                Width="97%" EmptyDataText="No Data Available">
                <RowStyle Font-Bold="false" />
                <Columns>
                    <asp:TemplateField HeaderText="Sr No" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Date" DataField="bookingDate" />
                    <asp:BoundField HeaderText="Consignment #" DataField="ConsignmentNumber" />
                    <asp:BoundField HeaderText="Pieces" DataField="pieces" />
                    <asp:BoundField HeaderText="Service Type" DataField="ServiceTypeName" />
                    <asp:BoundField HeaderText="Destination" DataField="Destination" />
                    <asp:BoundField HeaderText="Weight" DataField="Weight" />
                    <asp:BoundField HeaderText="Amount" DataField="totalAmount" />
                </Columns>
            </asp:GridView>
        </span>
    </div>
    <div style="width: 100%; overflow: scroll; text-align: center;">
        <div style="width: 100%; text-align: center; font-variant: small-caps; font-weight: bold;">
            Non Computed CNs
        </div>
        <span id="Span2" class="tbl-large">
            <asp:Label ID="Label2" runat="server" CssClass="error_msg"></asp:Label>
            <asp:GridView ID="gv_notComputedCNS" runat="server" AutoGenerateColumns="false" CssClass="mGrid"
                AlternatingRowStyle-CssClass="alt" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px"
                Width="97%" EmptyDataText="No Data Available">
                <RowStyle Font-Bold="false" />
                <Columns>
                    <asp:TemplateField HeaderText="Sr No" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Date" DataField="bookingDate" />
                    <asp:BoundField HeaderText="Consignment #" DataField="ConsignmentNumber" />
                    <asp:BoundField HeaderText="Pieces" DataField="pieces" />
                    <asp:BoundField HeaderText="Service Type" DataField="ServiceTypeName" />
                    <asp:BoundField HeaderText="Destination" DataField="Destination" />
                    <asp:BoundField HeaderText="Weight" DataField="Weight" />
                    <asp:BoundField HeaderText="Amount" DataField="totalAmount" />
                </Columns>
            </asp:GridView>
        </span>
    </div>
    <div>
        <asp:LinkButton ID="lk_summary" runat="server" Text="Invoice Summary" OnClick="lk_summary_Click"
            Visible="false"></asp:LinkButton>
        <asp:LinkButton ID="lk_detail" runat="server" Text="Invoice Detail" OnClick="lk_detail_Click"
            Visible="false"></asp:LinkButton>
    </div>
    <div style="width: 100%; text-align: center">
        <asp:Button ID="btn_reset" runat="server" Text="Reset" CssClass="button" OnClick="btn_reset_Click" />
        &nbsp;
        <asp:Button ID="btn_save" runat="server" Text="Save" CssClass="button" OnClick="btn_save_Click"
            OnClientClick="loader()" />
        &nbsp;
        <asp:Button ID="btn_print" runat="server" Text="Print Invoice" CssClass="button"
            Visible="false" OnClientClick="loader()" OnClick="btn_print_Click" />
    </div>
</asp:Content>
