<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="RR_Visit_Entry.aspx.cs" Inherits="MRaabta.Files.RR_Visit_Entry" %>

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
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <fieldset>
        <legend>Recovery Visit </legend>
        <table style="font-family: Calibri; font-size: medium; padding-bottom: 0px !important; padding-top: 0px !important"
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
                <td class="field" style="width: 10%;">Visit Date
                </td>
                <td class="input-field" style="width: 15% !important;">
                    <asp:TextBox ID="txt_date" runat="server" Width="70%"></asp:TextBox>
                    <Ajax1:CalendarExtender ID="calendarExtender1" runat="server" TargetControlID="txt_date"
                        Format="yyyy-MM-dd"></Ajax1:CalendarExtender>
                </td>
                <td class="field" style="width: 10%;">Account No
                </td>
                <td class="input-field" style="width: 15% !important;">
                    <asp:TextBox ID="txt_accountNo" runat="server" AutoPostBack="true" Width="76px" MaxLength="7"></asp:TextBox>
                </td>
                <td class="field" style="width: 10%  !important;">group Id
                </td>
                <td class="input-field" style="width: 15%  !important;">
                    <asp:TextBox ID="txt_groupID" runat="server" AutoPostBack="true" Width="50%" MaxLength="6"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="field" style="width: 10%  !important;">Invoice Number
                </td>
                <td class="input-field" style="width: 15%  !important;">
                    <asp:TextBox ID="txt_invoiceNumber" runat="server" AutoPostBack="true" Width="50%"
                        MaxLength="15"></asp:TextBox>
                </td>
                <td class="field" style="width: 10%  !important;">Year
                </td>
                <td class="input-field" style="width: 15%  !important;">
                    <asp:DropDownList ID="dd_Year" runat="server" CssClass="dropdown" Width="45%" Style="float: left;">
                    </asp:DropDownList>
                    <asp:CheckBox ID="chk_year_all" runat="server" AutoPostBack="true" Text="ALL" OnCheckedChanged="year_chk_CheckedChanged" Style="float: left; width: 27px;" />
                </td>
                <td class="field" style="width: 10%  !important;">Month
                </td>
                <td class="input-field" style="width: 15%  !important;">
                    <asp:DropDownList ID="dd_Month" runat="server" CssClass="dropdown" Width="45%" Style="float: left;">
                        <asp:ListItem Value="">Month</asp:ListItem>
                        <asp:ListItem Value="01">Jan</asp:ListItem>
                        <asp:ListItem Value="02">Feb</asp:ListItem>
                        <asp:ListItem Value="03">Mar</asp:ListItem>
                        <asp:ListItem Value="04">Apr</asp:ListItem>
                        <asp:ListItem Value="05">May</asp:ListItem>
                        <asp:ListItem Value="06">Jun</asp:ListItem>
                        <asp:ListItem Value="07">Jul</asp:ListItem>
                        <asp:ListItem Value="08">Aug</asp:ListItem>
                        <asp:ListItem Value="09">Sep</asp:ListItem>
                        <asp:ListItem Value="10">Oct</asp:ListItem>
                        <asp:ListItem Value="11">Nov</asp:ListItem>
                        <asp:ListItem Value="12">Dec</asp:ListItem>
                    </asp:DropDownList>
                    <asp:CheckBox ID="chk_month_all" runat="server" AutoPostBack="true" Text="ALL" OnCheckedChanged="month_chk_CheckedChanged" Style="float: left; width: 27px;" />
                </td>
                <td colspan="2" align="justify">
                    <asp:Button ID="btn_getInformation" runat="server" Text="Show Details" CssClass="button"
                        OnClick="btn_showTariff_Click" Height="26px"
                        OnClientClick="javascript:document.getElementById('ContentPlaceHolder1_loaders').style.display = 'block';javascript:document.getElementById('divDialogue').style.display = 'none';" />

                </td>
            </tr>
        </table>

        <div id="loaders" runat="server" class="outer_box" style="display: none;">
            <div id="loader" runat="server" class="loader">
            </div>
        </div>

        <div style="width: 92%; height: 250px; overflow: scroll;">
            <span id="Table_1" class="tbl-large">
                <asp:Label ID="err_msg" runat="server" CssClass="error_msg"></asp:Label>
                <asp:UpdatePanel ID="up_1" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="gv_tariff" runat="server" AutoGenerateColumns="false" CssClass="mGrid"
                            AlternatingRowStyle-CssClass="alt" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px"
                            OnRowCommand="gv_tariff_RowCommand" EmptyDataText="No Customer Available" OnDataBound="gv_tariff_DataBound"
                            OnRowDataBound="gv_tariff_RowDataBound">
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="cb_Status" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Label ID="lblRowNumber" Text='<%# Container.DataItemIndex + 1 %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Zone">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_Zone" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ZoneName") %>'></asp:Label>
                                        <asp:HiddenField ID="hd_clientid" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "clientid") %>' />
                                        <asp:HiddenField ID="hd_Zoneid" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "zonecode") %>' />
                                        <asp:HiddenField ID="hd_BranchId" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "branchcode") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Branch">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_branch" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "BranchName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Customer Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_ClientName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ClientName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Account #">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_Account" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ClientAccountno") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Billing Month">
                                    <ItemTemplate>
                                        <asp:Label ID="txt_BillMonth" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "BILLDATE") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Invoice Number">
                                    <ItemTemplate>
                                        <asp:Label ID="txt_invoiceNumber" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "invoiceNumber") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Invoice Amount ">
                                    <ItemTemplate>
                                        <asp:Label ID="txt_InvoiceAmount" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "BILLAMOUNT", "{0:N2}") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Outstanding">
                                    <ItemTemplate>
                                        <asp:Label ID="txt_Outstanding" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Outstanding", "{0:N2}") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Major category">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="dd_MajoreCat" runat="server" CssClass="dropdown" AppendDataBoundItems="true">
                                            <asp:ListItem Value="0">Select Major Category</asp:ListItem>
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Comments">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_comments" runat="server" TextMode="MultiLine"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
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
