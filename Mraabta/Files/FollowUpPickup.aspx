<%@ Page Title="" Language="C#" MasterPageFile="~/BtsMasterPage.Master" AutoEventWireup="true" CodeBehind="FollowUpPickup.aspx.cs" Inherits="MRaabta.Files.FollowUpPickup" %>

<%@ Register TagPrefix="Ajax1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
    <h1>
      Pickups Follow Up
        <%--<small>Control panel</small>--%>
      </h1>
     
    </section>
    <div style="float: left">
        <asp:Label ID="Errorid" runat="server" ForeColor="Red"></asp:Label>
    </div>
    <div class="box box-info">
        <div class="box-body">
            <div class="table-responsive">
                <br />
                <table>
                    <tr>
                        <td style="width: 15%">
                            <b>Date From </b>
                        </td>
                        <td style="width: 15%">
                            <asp:TextBox ID="txt_dateFrom" runat="server"></asp:TextBox>
                            <Ajax1:CalendarExtender ID="extendar1" TargetControlID="txt_dateFrom" runat="server"
                                Format="yyyy-MM-dd">
                            </Ajax1:CalendarExtender>
                        </td>
                        <td></td>
                        <td style="width: 15%">
                            <b>Date To</b>
                        </td>
                        <td style="width: 15%">
                            <asp:TextBox ID="txt_dateTo" runat="server"></asp:TextBox>
                            <Ajax1:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_dateTo" runat="server"
                                Format="yyyy-MM-dd">
                            </Ajax1:CalendarExtender>
                        </td>
                        <td style="width: 15%; text-align: left; padding-left: 10px;">&nbsp;</td>
                        <td style="width: 15%; text-align: left; padding-left: 10px;">&nbsp;
                        </td>
                        <td style="width: 10%;">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 15%">
                            <b>Search criteria:</b>
                        </td>
                        <td style="width: 15%">
                            <asp:RadioButtonList ID="rb_report" runat="server">
                                <asp:ListItem Value="0" Text="Account No" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="1" Text="Rider Code"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td></td>
                        <td style="width: 15%">
                            <b>Search:</b>
                        </td>
                        <td style="width: 15%">
                            <asp:TextBox ID="txt_input" runat="server"></asp:TextBox>
                        </td>
                        <td style="width: 15%; text-align: left; padding-left: 10px;">
                            <asp:Button ID="btn_generate" runat="server" Text="Search" OnClick="btn_generate_Click"
                                CssClass="button" Width="84px" />
                        </td>
                        <td style="width: 15%; text-align: left; padding-left: 10px;"></td>
                        <td style="width: 10%;"></td>
                    </tr>
                </table>
                <fieldset style="width: 100%">
                    <legend>LIST OF PICK-UP(S)</legend>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <div style="height: 500px; overflow: auto;">
                                <asp:Repeater ID="rp_Pickups" runat="server" OnItemDataBound="rp_Pickups_ItemDataBound">
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
                                                <td style="background-color: Black; color: White">Alternate Address
                                                </td>
                                                <td style="background-color: Black; color: White">Remarks
                                                </td>
                                                <td style="background-color: Black; color: White">Entry Time
                                                </td>
                                            </tr>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr id="trID" runat="server">
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
                                                
                                                <asp:Label ID="lblStatusDescription" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "StatusDescription")%>' />
                                            </td>
                                            <td class="grid-cell">
                                                <%# DataBinder.Eval(Container.DataItem, "alternate_Address")%>
                                            </td>
                                            <td class="grid-cell">
                                                <%# DataBinder.Eval(Container.DataItem, "Remarks")%>
                                            </td>
                                            <td class="grid-cell">
                                                <%# DataBinder.Eval(Container.DataItem, "CreatedOn")%>
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
            </div>
        </div>
    </div>
</asp:Content>
