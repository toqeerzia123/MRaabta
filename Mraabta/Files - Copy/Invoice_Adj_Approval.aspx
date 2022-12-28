<%@ Page Title="" Language="C#" MasterPageFile="~/BtsMasterPage.Master" AutoEventWireup="true" CodeBehind="Invoice_Adj_Approval.aspx.cs" Inherits="MRaabta.Files.Invoice_Adj_Approval" %>

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
        .input-form tr {
            float: none;
            margin: 0 0 10px;
            width: 100%;
        }

        .outer_box {
            background: #444 none repeat scroll 0 0;
            height: 101%;
            left: 0;
            opacity: 0.9;
            position: absolute;
            top: -1%;
            width: 100%;
        }


        .pop_div {
            background: #eee none repeat scroll 0 0;
            border-radius: 10px;
            height: 100px;
            left: 48%;
            position: relative;
            top: 40%;
            width: 235px;
        }

        .btn_ok {
            background: #000 none repeat scroll 0 0;
            border: 0 none;
            color: #fff;
            left: -18px;
            padding: 1px 14px;
            position: relative;
            top: 67%;
        }

        .btn_cancel {
            background: #000 none repeat scroll 0 0;
            border: 0 none;
            color: #fff;
            left: 22%;
            padding: 1px 14px;
            position: relative;
            top: 42%;
        }

        .pop_div > span {
            float: left;
            line-height: 40px;
            text-align: center;
            width: 100%;
        }

        .tbl-large div {
            position: static;
        }

        .outer_box img {
            left: 42%;
            position: relative;
            top: 40%;
        }
    </style>
    <div id="div3" runat="server" style="border: 1px Solid Black; width: 100%; height: 500px; background-color: transparent; text-align: center; vertical-align: middle; font-size: large; font-family: Calibri; display: none;">
        <table style="width: 100%;">
            <tr>
                <td style="width: 100%; text-align: left;">Following Invoices are already Present For this Client.
                </td>
            </tr>
            <tr>
                <td>
                    <asp:PlaceHolder ID="ph1" runat="server"></asp:PlaceHolder>
                </td>
            </tr>
            <tr>
                <td style="text-align: center;">Do you want to continue?
                </td>
            </tr>
            <tr>
                <td style="text-align: center;">
                    <asp:Button ID="btn_proceed" runat="server" Text="YES" CssClass="button" Width="10%" />
                    &nbsp;&nbsp;
                    <asp:Button ID="btn_dontProceed" runat="server" Text="NO" CssClass="button" Width="10%" />
                </td>
            </tr>
        </table>
    </div>
    <div id="div2" runat="server" class="outer_box" style="display: none; background: #FFFFFF none repeat scroll 0 0;">
        <img src="../images/Loading_Movie-02.gif" />
    </div>
    <div style="width: 100% !important; text-align: center;">
        <asp:Label ID="Errorid" runat="server" ForeColor="Red" Font-Bold="true" Font-Size="Large"></asp:Label>
    </div>
    <table style="font-family: Calibri; font-size: medium; padding-bottom: 0px !important; padding-top: 0px !important; width: 97%"
        class="input-form">
        <tr style="float: none !important;">
            <td style="float: none !important; font-variant: small-caps !important; width: 200px; font-size: large; text-align: center;">
                <b>Information</b>
            </td>
        </tr>
        <tr>
            <td class="field" style="width: 10% !important;">Company
            </td>
            <td class="input-field" style="width: 15% !important;">
                <asp:DropDownList ID="dd_company" runat="server">
                </asp:DropDownList>
            </td>
            <td class="space" style="width: 8% !important; margin: 0px 0px 0px 0px !important;"></td>
            <td class="field" style="width: 10% !important;">From Date
            </td>
            <td class="input-field" style="width: 15% !important;">
                <telerik:RadDatePicker ID="pickerStart" runat="server" DateInput-DateFormat="dd/MM/yyyy">
                </telerik:RadDatePicker>
            </td>
            <td class="space" style="width: 8% !important; margin: 0px 0px 0px 0px !important;"></td>
            <td class="field" style="width: 10% !important;">to Date
            </td>
            <td class="input-field" style="width: 15% !important;">
                <telerik:RadDatePicker ID="pickerEndDate" runat="server" DateInput-DateFormat="dd/MM/yyyy">
                </telerik:RadDatePicker>
            </td>
            <td class="space" style="width: 8% !important; margin: 0px 0px 0px 0px !important;"></td>
        </tr>
        <tr>
            <td class="field" style="width: 10% !important;"></td>
            <td class="input-field" style="width: 15% !important;">
                <asp:HiddenField ID="hd_customerClientID" runat="server" />
            </td>
            <td class="space" style="width: 8% !important; margin: 0px 0px 0px 0px !important;"></td>
            <td class="field" style="width: 10% !important;"></td>
            <td class="input-field" style="width: 15% !important;"></td>
            <td class="space" style="width: 8% !important; margin: 0px 0px 0px 0px !important;"></td>
            <td class="field" style="width: 10% !important;"></td>
            <td class="input-field" style="width: 15% !important;">
                <asp:Button ID="btn_getConsignment" runat="server" CssClass="button" Width="100%"
                    Text="Get Notes" OnClick="btn_getConsignment_Click" OnClientClick="loader()" />
            </td>
            <td class="space" style="width: 8% !important; margin: 0px 0px 0px 0px !important;"></td>
        </tr>
    </table>
    <div style="width: 100%; height: 250px; overflow: scroll; text-align: center;">
        <span id="Span1" class="tbl-large">
            <asp:Label ID="Label1" runat="server" CssClass="error_msg"></asp:Label>
            <asp:GridView ID="gv_cns" runat="server" AutoGenerateColumns="false" CssClass="mGrid"
                AlternatingRowStyle-CssClass="alt" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px"
                Width="97%" EmptyDataText="No Data Available" OnRowDataBound="gv_cns_RowDataBound">
                <RowStyle Font-Bold="false" />
                <Columns>
                    <asp:BoundField HeaderText="Invoice Number" DataField="invoiceNumber" />
                    <asp:BoundField HeaderText="companyName" DataField="companyName" />
                    <asp:BoundField HeaderText="ClientName" DataField="ClientName" />
                    <asp:BoundField HeaderText="accountNo" DataField="accountNo" />
                    <asp:BoundField HeaderText="Zonename" DataField="Zonename" />
                    <asp:BoundField HeaderText="OriginName" DataField="OriginName" />
                    <asp:BoundField HeaderText="TotalAmount" DataField="TotalAmount" />
                    <asp:BoundField HeaderText="Gst" DataField="Gst" />
                    <asp:BoundField HeaderText="Discount" DataField="Discount" />
                    <asp:BoundField HeaderText="Fuel" DataField="Fuel" />
                    <asp:BoundField HeaderText="Fuelgst" DataField="Fuelgst" />
                    <asp:BoundField HeaderText="CreatedBy" DataField="CreatedBy" />
                    <asp:BoundField HeaderText="Note_type" DataField="Note_type" />
                    <asp:TemplateField HeaderText="Status">
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_Approve" runat="server" />
                            <asp:HiddenField ID="hd_approve" runat="server" Value='<%# Eval("Approved") %>' />
                            <asp:HiddenField ID="hd_invoiceNumber" runat="server" Value='<%# Eval("invoiceNumber") %>' />
                            <asp:HiddenField ID="hd_Note_type" runat="server" Value='<%# Eval("Note_type") %>' />
                            <asp:HiddenField ID="hd_Note_Number" runat="server" Value='<%# Eval("Note_Number") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:HyperLink ID="lbl_link" Text="view" runat="server"></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </span>
    </div>
    <div style="width: 100%; text-align: center">
        <asp:Button ID="btn_reset" runat="server" Text="Reset" CssClass="button" OnClick="btn_reset_Click" />
        &nbsp;
        <asp:Button ID="btn_save" runat="server" Text="Save" CssClass="button" OnClick="btn_save_Click"
            OnClientClick="loader()" />
    </div>
</asp:Content>
