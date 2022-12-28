<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="Manage_Invoices_COD.aspx.cs" Inherits="MRaabta.Files.Manage_Invoices_COD" %>

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
    
    <div style="width: 100%; height: 500px;">
        <table class="mGrid">
            <tr>
                <td colspan="2" style="width:30%;">
                    <b>Billing Information</b>
                </td>
                <td style="width:70%;">
                    <b>Customer Information</b>
                </td>
                </tr>
            <tr>
                    <td>
                                    Zone
                    </td>
                <td>
                                    <asp:DropDownList ID="ddl_zoneId" runat="server" Width="100%" Height="30px" AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                    <td rowspan="4" style="height: 420px; vertical-align: top">
                        <div style="overflow: scroll; height: 400px">
                            <asp:CheckBox ID="SelectAll" Text=" Select All" runat="server" AutoPostBack="true"
                                OnCheckedChanged="SelectAll_CheckedChanged" />
                            <hr />
                            <br />
                            <asp:CheckBoxList ID="lb_CustomerList" runat="server" RepeatDirection="Vertical"
                                TextAlign="Right">
                            </asp:CheckBoxList>
                        </div>
                    </td>
                </tr>
                            <tr>
                                <td>
                                    Billing Cycle
                                </td>
                                <td>
                                    <asp:CheckBoxList ID="cbBillingCycle" RepeatDirection="Vertical" runat="server"></asp:CheckBoxList>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <asp:Button ID="btn_getCustomers" runat="server" CssClass="button" Width="100%"
                                        Text="Get Customers" OnClick="btn_getCustomers_Click" OnClientClick="loader()" />
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <asp:Button ID="btn_save" runat="server"  Width="100%" Text="Save" CssClass="button" OnClick="btn_save_Click" />
                                </td>
                            </tr>
        </table>
    </div>
</asp:Content>
