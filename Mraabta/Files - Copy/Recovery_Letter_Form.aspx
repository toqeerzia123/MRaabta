<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="Recovery_Letter_Form.aspx.cs" Inherits="MRaabta.Files.Recovery_Letter_Form" %>

<%@ Register TagPrefix="Ajax1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .style1 {
            float: left;
            width: 20%;
            height: 59px;
        }

        .style2 {
            float: left;
            width: 15%;
            height: 59px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <fieldset>
        <table style="font-family: Calibri; font-size: medium; padding-bottom: 0px !important; padding-top: 0px !important; display: none"
            class="input-form" width="100%">
            <tr>
                <td colspan="6" style="width: 100% !important;">Team Information
                </td>
            </tr>
            <tr>
                <td class="field" style="width: 10% !important;">Head of Dept
                </td>
                <td class="input-field" style="width: 15% !important;">
                    <asp:DropDownList ID="dd_Complevel1" runat="server" AppendDataBoundItems="true" CssClass="dropdown"
                        AutoPostBack="true" OnSelectedIndexChanged="dd_Complevel1_SelectedIndexChanged">
                        <asp:ListItem Value="0">Select Head</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td class="field" style="width: 10% !important;">Manager
                </td>
                <td class="input-field" style="width: 15% !important;">
                    <asp:DropDownList ID="dd_Complevel2" runat="server" AppendDataBoundItems="true" CssClass="dropdown"
                        AutoPostBack="true" OnSelectedIndexChanged="dd_Complevel2_SelectedIndexChanged">
                        <asp:ListItem Value="0">Select Manager</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td class="field" style="width: 10% !important;">Officer
                </td>
                <td class="input-field" style="width: 15% !important;">
                    <asp:DropDownList ID="dd_Complevel3" runat="server" AppendDataBoundItems="true" CssClass="dropdown"
                        AutoPostBack="true" OnSelectedIndexChanged="dd_Complevel3_SelectedIndexChanged">
                        <asp:ListItem Value="0">Select Officer</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        <table style="font-family: Calibri; font-size: medium; padding-bottom: 0px !important; padding-top: 0px !important"
            class="input-form" width="100%">
            <tr>
                <td class="field" colspan="4" style="width: 100%;">
                    <asp:RadioButtonList ID="bt_1" runat="server" RepeatDirection="Horizontal" OnSelectedIndexChanged="bt_1_SelectedIndexChanged"
                        AutoPostBack="true">
                        <asp:ListItem Value="0" Selected="True"> By Group </asp:ListItem>
                        <asp:ListItem Value="1"> By Account </asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td class="field" style="width: 10%;">Date
                </td>
                <td class="input-field" style="width: 15% !important;">
                    <asp:TextBox ID="txt_date" runat="server" Width="70%"></asp:TextBox>
                    <Ajax1:CalendarExtender ID="calendarExtender1" runat="server" TargetControlID="txt_date"
                        Format="yyyy-MM-dd"></Ajax1:CalendarExtender>
                </td>
                <td class="field" style="width: 15%;">
                    <asp:Label ID="lb_1" runat="server" Text="Group ID"></asp:Label>
                </td>
                <td class="input-field" style="width: 20% !important;">
                    <asp:TextBox ID="txt_accountNo" runat="server" AutoPostBack="true" Width="76px" Visible="false"></asp:TextBox>
                    <asp:HiddenField ID="creditclientid" runat="server" />
                    <asp:TextBox ID="txt_groupID" runat="server" AutoPostBack="true" Width="50%" MaxLength="6"></asp:TextBox>
                </td>
                <td class="field" style="width: 10%  !important;">Days
                </td>
                <td class="input-field" style="width: 15%  !important;">
                    <asp:DropDownList ID="dd_Days" runat="server">
                        <asp:ListItem Value="0">Select Days</asp:ListItem>
                        <asp:ListItem Value="60">60 Days</asp:ListItem>
                        <asp:ListItem Value="90">90 Days</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="field" style="width: 10%  !important;"></td>
                <td class="input-field" style="width: 15%  !important;"></td>
                <td colspan="2" align="justify">
                    <asp:Button ID="btn_getInformation" runat="server" Text="Show Details" CssClass="button"
                        OnClick="btn_showTariff_Click" Height="26px" />
                </td>
            </tr>
        </table>
        <div style="width: 100%; height: 250px; overflow: scroll;">
            <span id="Table_1" class="tbl-large">
                <asp:Label ID="err_msg" runat="server" CssClass="error_msg"></asp:Label>
                <asp:UpdatePanel ID="up_1" runat="server">
                    <ContentTemplate>
                        <span id="Table_1" class="tbl-large">
                            <asp:GridView ID="gv_tariff" runat="server" AlternatingRowStyle-CssClass="alt" AutoGenerateColumns="false"
                                BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" CssClass="mGrid" EmptyDataText="No Customer Available"
                                OnDataBound="gv_tariff_DataBound" OnRowCommand="gv_tariff_RowCommand" OnRowDataBound="gv_tariff_RowDataBound"
                                OnSelectedIndexChanged="gv_tariff_SelectedIndexChanged">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="cb_Status" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Label ID="lblRowNumber" runat="server" Text="<%# Container.DataItemIndex + 1 %>" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Customer Group Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_ClientName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "GroupName") %>'></asp:Label>
                                            <asp:HiddenField ID="hd_clientid" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "clientGroupID") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Invoice Amount ">
                                        <ItemTemplate>
                                            <asp:Label ID="txt_InvoiceAmount" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "InvoiceAmount") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Outstanding">
                                        <ItemTemplate>
                                            <asp:Label ID="txt_Outstanding" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "outstandingAmount") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Comments">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txt_comments" runat="server" TextMode="MultiLine"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <asp:GridView ID="gv_tariff_" runat="server" AlternatingRowStyle-CssClass="alt" AutoGenerateColumns="false"
                                BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" CssClass="mGrid" EmptyDataText="No Customer Available">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="cb_Status" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Label ID="lblRowNumber" runat="server" Text="<%# Container.DataItemIndex + 1 %>" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Customer Information">
                                        <ItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ClientName") %>'></asp:Label>
                                            -
                                            <asp:Label ID="lbl_ClientName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "AccountNo") %>'></asp:Label>
                                            <asp:HiddenField ID="hd_clientid" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "AccountNo") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Invoice Amount ">
                                        <ItemTemplate>
                                            <asp:Label ID="txt_InvoiceAmount" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "InvoiceAmount") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Outstanding">
                                        <ItemTemplate>
                                            <asp:Label ID="txt_Outstanding" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "outstandingAmount") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Comments">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txt_comments" runat="server" TextMode="MultiLine"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </span>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </span>
        </div>
        <div style="width: 100%; text-align: center">
            <asp:Button ID="btn_save" runat="server" Text="Save" CssClass="button" OnClick="btn_save_Click"
                OnClientClick="return confirm('Are you sure')" />
            &nbsp;
            <asp:Button ID="btn_cancel" runat="server" Text="Cancel" CssClass="button" OnClick="btn_cancel_Click" />
        </div>
    </fieldset>
</asp:Content>
