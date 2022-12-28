<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="Rider_Attendence.aspx.cs" Inherits="MRaabta.Files.Rider_Attendence" %>

<%@ Register Assembly="TimePicker" Namespace="MKB.TimePicker" TagPrefix="cc1" %>
<%@ Register TagPrefix="Ajax1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        .button
        {
            background-color: #5f5a8d !important;
            border: 0 none !important;
            border-radius: 5px !important;
            color: White !important;
            font-family: Calibri !important;
            font-size: small !important;
            padding: 3px 20px !important;
            cursor: pointer !important;
        }
    </style>
    <div style="float: left">
        <asp:Label ID="Errorid" runat="server" ForeColor="Red"></asp:Label>
    </div>
    <br />
    <fieldset style="width: 100%">
        <legend>Mark Rider Attendence</legend>
        <table width="100%">
            <tr>
                <td colspan="1">
                    <b>Select Date for Attendence</b>
                </td>
                <td colspan="1">
                    <%-- <asp:DropDownList ID="dd_riders" runat="server" AutoPostBack="true" OnSelectedIndexChanged="dd_riders_SelectedIndexChanged">
                    </asp:DropDownList>--%>
                    <asp:TextBox ID="dd_riders" runat="server" AutoPostBack="false"></asp:TextBox>
                    <Ajax1:CalendarExtender ID="extendar1" TargetControlID="dd_riders" runat="server"
                        Format="yyyy-MM-dd">
                    </Ajax1:CalendarExtender>
                </td>
                <td colspan="1">
                    <b>Select Origin of Riders</b>
                </td>
                <td colspan="1">
                    <asp:DropDownList ID="dd_origin" runat="server">
                    </asp:DropDownList>
                </td>
                <td colspan="1">
                    <asp:Button ID="btn_Search" runat="server" Text="Get Riders" CssClass="button" OnClick="btn_Search_Click" />
                </td>
            </tr>
        </table>
    </fieldset>
    <fieldset style="width: 100%">
        <legend>LIST OF RIDER(S)</legend>
        <div style="height: 500px; overflow: auto;">
            <asp:Repeater ID="rp_Pickups" runat="server">
                <HeaderTemplate>
                    <table width="100%">
                        <tr>
                            <td style="background-color: Black; color: White">
                                Select
                            </td>
                            <td style="background-color: Black; color: White">
                                Rider Code
                            </td>
                            <td style="background-color: Black; color: White">
                                Rider Name
                            </td>
                            <td style="background-color: Black; color: White">
                                Origin
                            </td>
                            <td style="background-color: Black; color: White">
                                Rider Contact
                            </td>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td class="grid-cell">
                            <asp:CheckBox ID="chk_absent" runat="server" Checked="false" AutoPostBack="false" />
                        </td>
                        <td class="grid-cell">
                            <%# DataBinder.Eval(Container.DataItem, "riderCode")%>
                        </td>
                        <td class="grid-cell">
                            <%# DataBinder.Eval(Container.DataItem, "RiderName")%>
                        </td>
                        <td class="grid-cell">
                            <%# DataBinder.Eval(Container.DataItem, "Origin")%>
                        </td>
                        <td class="grid-cell">
                            <%# DataBinder.Eval(Container.DataItem, "ContactNo")%>
                            <asp:HiddenField ID="hd_riderCode" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "riderCode")%>' />
                            <asp:HiddenField ID="hd_riderPhone" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "ContactNo")%>' />
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
    </fieldset>
    <asp:Button ID="btn_Mark" runat="server" Text="Mark Absent for Selected Date" CssClass="button"
        OnClick="btn_Mark_Click" />
</asp:Content>
