<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="RR_Mapping.aspx.cs" Inherits="MRaabta.Files.RR_Mapping" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
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
    <fieldset>
        <legend>RR Mapping </legend>
        <table style="font-family: Calibri; font-size: medium; padding-bottom: 0px !important; padding-top: 0px !important"
            class="input-form" width="100%">
            <tr>
                <td class="field" style="width: 10% !important;">Zone
                </td>
                <td class="input-field" style="width: 16% !important; margin-right: 38px;">
                    <asp:DropDownList ID="dd_Zone" runat="server" AppendDataBoundItems="true" CssClass="dropdown"
                        OnSelectedIndexChanged="dd_Zone_SelectedIndexChanged" AutoPostBack="true">
                        <asp:ListItem Value="0">Select Zone</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td class="field" style="width: 10% !important;">Branch
                </td>
                <td class="input-field" style="width: 15% !important; margin-right: 40px;">
                    <asp:DropDownList ID="dd_Branch" runat="server" AppendDataBoundItems="true" CssClass="dropdown">
                        <asp:ListItem Value="0">Select Branch</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td class="field" style="width: 10% !important;">Account No
                </td>
                <td class="input-field" style="width: 15%  !important;">
                    <asp:TextBox ID="txt_accountNo" runat="server" AutoPostBack="true" OnTextChanged="txt_accountNo_TextChanged"
                        Width="76px"></asp:TextBox>
                    <asp:HiddenField ID="creditclientid" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="field" style="width: 10% !important;">Industry
                </td>
                <td class="input-field" style="width: 20% !important;">
                    <telerik:RadComboBox ID="dd_Industry" runat="server" AutoPostBack="true" Skin="Metro"
                        AppendDataBoundItems="true" AllowCustomText="true" MarkFirstMatch="true" Width="150px"
                        CssClass="dropdown">
                        <Items>
                            <telerik:RadComboBoxItem Value="0" Text="Select Industry" />
                        </Items>
                    </telerik:RadComboBox>
                </td>
                <td class="field" style="width: 10%  !important;">group Id
                </td>
                <td class="input-field" style="width: 15%  !important;">
                    <asp:TextBox ID="txt_groupID" runat="server" AutoPostBack="true" Width="50%" MaxLength="6"></asp:TextBox>
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
        <div style="width: 100%; height: 250px; overflow: scroll;">
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
                                        <asp:HiddenField ID="hd_clientid" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "id") %>' />
                                        <asp:HiddenField ID="hd_Zoneid" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "zonecode") %>' />
                                        <asp:HiddenField ID="hd_BranchId" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "branchcode") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Branch">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_branch" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "BranchName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Account #">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_Account" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "accountNo") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Customer Name">
                                    <ItemTemplate>
                                        <asp:Label ID="txt_CustName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Name") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Group Name">
                                    <ItemTemplate>
                                        <asp:Label ID="txt_GroupName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "GroupName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Recovery Officer">
                                    <ItemTemplate>
                                        <asp:Label ID="txt_recoveryofficer" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "RecoveryOfficer") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="industry Name">
                                    <ItemTemplate>
                                        <asp:Label ID="txt_industryName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "industryName") %>'></asp:Label>
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
                OnClientClick="javascript:document.getElementById('ContentPlaceHolder1_Div1').style.display = 'block';javascript:document.getElementById('divDialogue').style.display = 'none';" />

            <%--OnClientClick="return confirm('Are you sure')" />--%>
            &nbsp;
            <asp:Button ID="btn_cancel" runat="server" Text="Cancel" CssClass="button" OnClick="btn_cancel_Click" />
        </div>

        <div id="Div1" runat="server" class="outer_box" style="display: none;">
            <div id="Div2" runat="server" class="loader">
            </div>
        </div>

        <div style="width: 100%; height: 250px; overflow: scroll;">
            <span id="Span1" class="tbl-large">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" CssClass="mGrid"
                            AlternatingRowStyle-CssClass="alt" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px"
                            EmptyDataText="No Customer Available" OnRowCommand="GridView1_RowCommand" OnSelectedIndexChanged="GridView1_SelectedIndexChanged">
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Label ID="lblRowNumber" Text='<%# Container.DataItemIndex + 1 %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Zone">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_Zone" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ZoneName") %>'></asp:Label>
                                        <asp:HiddenField ID="hd_clientid" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "id") %>' />
                                        <asp:HiddenField ID="hd_Zoneid" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "zonecode") %>' />
                                        <asp:HiddenField ID="hd_BranchId" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "branchcode") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Branch">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_branch" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "BranchName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Account #">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_Account" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "accountNo") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Customer Name">
                                    <ItemTemplate>
                                        <asp:Label ID="txt_CustName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Name") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Group Name">
                                    <ItemTemplate>
                                        <asp:Label ID="txt_GroupName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "GroupName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="industry Name">
                                    <ItemTemplate>
                                        <asp:Label ID="txt_industryName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "industryName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Button ID="btn_delete" runat="server" Text="Delete" CommandName="del" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </span>
        </div>
    </fieldset>
</asp:Content>