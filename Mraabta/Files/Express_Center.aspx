<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="False" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="Express_Center.aspx.cs" Inherits="MRaabta.Files.Express_Center" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="Ajax1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        input {
            padding: 3px;
        }
    </style>
    <fieldset>
        <legend style="font-size: medium;">Express Center</legend>
        <table style="font-size: medium; padding-bottom: 0px; width: 100%;">
            <tr>
                <td>
                    <asp:LinkButton ID="LinkButton1" Text="Edit" PostBackUrl="~/Files/update_Express_Center.aspx"
                        CausesValidation="false" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="field" style="width: 12% !important; text-align: right !important; padding-right: 10px !important;">
                    <b>Express Center Code. </b>
                </td>
                <td>
                    <asp:TextBox ID="txt_Eccode" runat="server" MaxLength="13" Width="160px" Enabled="false"> </asp:TextBox>
                </td>
                <td style="width: 12% !important; text-align: right !important; padding-right: 10px !important;">
                    <b>Express Center Name </b>
                </td>
                <td>
                    <asp:TextBox ID="txt_ExpressCenterName" runat="server" MaxLength="100" Width="160px"> </asp:TextBox>
                </td>
                <td></td>
                <td></td>
            </tr>
            <tr>
                <td style="width: 12% !important; text-align: right !important; padding-right: 10px !important;">
                    <b>Address </b>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txt_Description" runat="server" MaxLength="203" AutoPostBack="false"
                        Width="400px" TextMode="MultiLine"> </asp:TextBox>
                </td>
                <td style="width: 12% !important; text-align: right !important; padding-right: 10px !important;">
                    <b>Branch </b>
                </td>
                <td class="input-field" style="width: 15% !important;">
                    <telerik:RadComboBox ID="dd_Branch" runat="server" Skin="Metro" AppendDataBoundItems="true"
                        AllowCustomText="true" MarkFirstMatch="true" Visible="true" OnSelectedIndexChanged="dd_Branch_SelectedIndexChanged">
                        <Items>
                            <telerik:RadComboBoxItem Text="Select Branch" Value="0" Selected="true" />
                        </Items>
                    </telerik:RadComboBox>
                </td>
            </tr>
            <tr>
                <td style="width: 12% !important; text-align: right !important; padding-right: 10px !important;">
                    <b>SName </b>
                </td>
                <td colspan="1">
                    <asp:TextBox ID="txt_Sname" runat="server" MaxLength="3" Width="160px"> </asp:TextBox>
                </td>
                <td style="width: 12% !important; text-align: right !important; padding-right: 10px !important;">
                    <b>Express Center Type </b>
                </td>
                <td colspan="1">
                    <asp:DropDownList ID="ec_type" AutoPostBack="true" runat="server" CssClass="dropdown"
                        OnSelectedIndexChanged="ec_type_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td style="width: 12% !important; text-align: right !important; padding-right: 10px !important;">
                    <b>is distribution Center</b>
                </td>
                <td colspan="1">
                    <asp:CheckBox ID="cb_idc" runat="server" />
                </td>
            </tr>
            <tr>
                <td style="width: 12% !important; text-align: right !important; padding-right: 10px !important;">
                    <b>Franchise Acc</b>
                </td>
                <td colspan="1">
                    <%--      <asp:TextBox ID="txt_Facc" AutoPostBack="true" runat="server" MaxLength="6" Width="160px"
                        Enabled="false" OnTextChanged="txt_Facc_TextChanged"> </asp:TextBox>
                    --%>
                    <telerik:RadComboBox ID="rad_Franchise" runat="server" Skin="Metro" AppendDataBoundItems="true"
                        AllowCustomText="true" MarkFirstMatch="true" Visible="true" Enabled="false">
                        <Items>
                            <telerik:RadComboBoxItem Text="Select Franchise" Value="0" Selected="true" />
                        </Items>
                    </telerik:RadComboBox>
                </td>
                </td>
                <td style="width: 12% !important; text-align: right !important; padding-right: 10px !important;">
                    <b>DayOff </b>
                </td>
                <td colspan="1">
                    <asp:DropDownList ID="dd_Dayoff" runat="server" CssClass="dropdown">
                    </asp:DropDownList>
                </td>
                <td style="width: 12% !important; text-align: right !important; padding-right: 10px !important;">
                    <b>Main EC </b>
                </td>
                <td colspan="1">
                    <asp:CheckBox ID="cb_EC" runat="server" />
                </td>
            </tr>
            <tr>
                <td style="width: 12% !important; text-align: right !important; padding-right: 10px !important;">
                    <b>Client ID </b>
                </td>
                <td colspan="1">
                    <asp:TextBox ID="txt_ClientID" runat="server" MaxLength="20" Width="160px"> </asp:TextBox>
                </td>
                <td style="width: 12% !important; text-align: right !important; padding-right: 10px !important;">
                    <b>Fax No </b>
                </td>
                <td colspan="1">
                    <asp:TextBox ID="txt_FaxNo" runat="server" MaxLength="15" Width="160px"> </asp:TextBox>
                </td>
                <td style="width: 12% !important; text-align: right !important; padding-right: 10px !important;">
                    <b>Phone No </b>
                </td>
                <td colspan="1">
                    <asp:TextBox ID="txt_PhoneNo" runat="server" MaxLength="15" Width="160px"> </asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 12% !important; text-align: right !important; padding-right: 10px !important;">
                    <b>EmailID</b>
                </td>
                <td colspan="1">
                    <asp:TextBox ID="txt_EmailID" runat="server" MaxLength="50" Width="160px"> </asp:TextBox>
                </td>
            </tr>
            <tr>
                <td></td>
                <td></td>
                <td colspan="2">
                    <asp:Button ID="btn_save" runat="server" Text="Save" CssClass="button" OnClick="btn_SaveConsignment1_Click"
                        UseSubmitBehavior="false" />
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    <asp:Button ID="btn_Cancel" runat="server" Text="Cancel" CssClass="button" UseSubmitBehavior="false"
                        OnClick="btn_Cancel_Click" Width="93px" />
                </td>
                <td></td>
                <td></td>
            </tr>
        </table>
    </fieldset>
</asp:Content>