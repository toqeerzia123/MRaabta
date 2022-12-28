<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="Post_Pickup.aspx.cs" Inherits="MRaabta.Files.Post_Pickup" %>

<%@ Register TagPrefix="Ajax1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<%@ Register Assembly="TimePicker" Namespace="MKB.TimePicker" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        .button {
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
        <legend>Post Pick Up</legend>
        <table width="100%">
            <tr>
                <td colspan="1">
                    <b>Enter Rider Code</b>
                </td>
                <td colspan="1">
                    <%-- <asp:DropDownList ID="dd_riders" runat="server" AutoPostBack="true" OnSelectedIndexChanged="dd_riders_SelectedIndexChanged">
                    </asp:DropDownList>--%>
                    <asp:TextBox ID="dd_riders" runat="server" AutoPostBack="false"></asp:TextBox>
                </td>
                <td colspan="1">
                    <b>Date From</b>
                </td>
                <td colspan="1">
                    <%-- <asp:DropDownList ID="dd_riders" runat="server" AutoPostBack="true" OnSelectedIndexChanged="dd_riders_SelectedIndexChanged">
                    </asp:DropDownList>--%>
                    <asp:TextBox ID="txt_dateFrom" runat="server" AutoPostBack="false"></asp:TextBox>
                    <Ajax1:CalendarExtender ID="calendar" runat="server" TargetControlID="txt_dateFrom" Format="yyyy/MM/dd"></Ajax1:CalendarExtender>
                </td>
                <td colspan="1">
                    <b>Date To</b>
                </td>
                <td colspan="1">
                    <%-- <asp:DropDownList ID="dd_riders" runat="server" AutoPostBack="true" OnSelectedIndexChanged="dd_riders_SelectedIndexChanged">
                    </asp:DropDownList>--%>
                    <asp:TextBox ID="txt_dateTo" runat="server" AutoPostBack="false"></asp:TextBox>
                    <Ajax1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txt_dateTo" Format="yyyy/MM/dd"></Ajax1:CalendarExtender>
                </td>
                <td colspan="1">
                    <asp:Button ID="btn_Search" runat="server" Text="Search" CssClass="button" OnClick="btn_Search_Click" />
                </td>
                <td colspan="1">&nbsp;
                </td>
                <td colspan="1">&nbsp;
                </td>
            </tr>
        </table>
    </fieldset>
    <fieldset style="width: 100%">
        <legend>LIST OF PICK-UP(S)</legend>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div style="height: 500px; overflow: auto;">
                    <asp:Repeater ID="rp_Pickups" runat="server" OnItemCommand="rp_Pickups_ItemCommand">
                        <HeaderTemplate>
                            <table width="100%">
                                <tr>
                                    <td style="background-color: Black; color: White">Rider Code
                                    </td>
                                    <td style="background-color: Black; color: White">Rider Name
                                    </td>
                                    <td style="background-color: Black; color: White">Origin
                                    </td>
                                    <td style="background-color: Black; color: White">Rider Phone
                                    </td>
                                    <td style="background-color: Black; color: White">Account No
                                    </td>
                                    <td style="background-color: Black; color: White">Account Name
                                    </td>
                                    <td style="background-color: Black; color: White">Weight
                                    </td>
                                    <td style="background-color: Black; color: White">Pieces
                                    </td>
                                    <td style="background-color: Black; color: White">Pick Up Date
                                    </td>
                                    <td style="background-color: Black; color: White">Status
                                    </td>
                                    <td style="background-color: Black; color: White">Remarks
                                    </td>
                                </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
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
                                    <%# DataBinder.Eval(Container.DataItem, "riderPhone")%>
                                </td>
                                <td class="grid-cell">
                                    <%# DataBinder.Eval(Container.DataItem, "accountNo")%>
                                </td>
                                <td class="grid-cell">
                                    <%# DataBinder.Eval(Container.DataItem, "name")%>
                                </td>
                                <td class="grid-cell">
                                    <%# DataBinder.Eval(Container.DataItem, "weight")%>
                                </td>
                                <td class="grid-cell">
                                    <%# DataBinder.Eval(Container.DataItem, "pieces")%>
                                    <asp:HiddenField ID="hd_pickup_status" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "pickup_status")%>' />
                                    <asp:HiddenField ID="hd_id" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "id")%>' />
                                    <asp:HiddenField ID="hd_riderCode" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "riderCode")%>' />
                                    <asp:HiddenField ID="hd_riderPhone" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "riderPhone")%>' />
                                </td>
                                <td class="grid-cell">
                                    <%# DataBinder.Eval(Container.DataItem, "pickup_Date")%>
                                </td>
                                <td class="grid-cell">
                                    <asp:DropDownList ID="dd_status" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td class="grid-cell">
                                    <asp:TextBox ID="txt_remarks" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "remarks")%>'></asp:TextBox>
                                </td>
                                <td class="grid-cell">
                                    <asp:Button ID="btn_Update" runat="server" CssClass="button" Text="Update Status"
                                        CommandName="Update" />
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
            </ContentTemplate>
        </asp:UpdatePanel>
    </fieldset>
</asp:Content>