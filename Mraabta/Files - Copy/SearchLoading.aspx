<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="SearchLoading.aspx.cs" Inherits="MRaabta.Files.SearchLoading" %>


<%@ Register TagPrefix="Ajax1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div>
        <table cellpadding="0" cellspacing="0" width='100%' class='mGrid_Table'>
            <tr>
                <td colspan="12" align="center" class="head_column">
                    <h3>Search Loading</h3>
                </td>
            </tr>
        </table>
        <table class="input-form" style="width: 90%">
            <tr>
                <%--<td class="field">Create Date: </td>
                    <td class="input-field">
                        <asp:TextBox ID="dd_start_date" runat="server" CssClass="med-field" MaxLength="10"></asp:TextBox>
                            <Ajax1:CalendarExtender ID="CalendarExtender3" TargetControlID="dd_start_date" runat="server"
                                Format="yyyy-MM-dd" PopupButtonID="Image1">
                            </Ajax1:CalendarExtender>  
                    </td>
                    <td class="space"></td> --%>
                <td class="field" style="width: 10%">Date:
                </td>
                <td class="input-field" style="width: 20%;">
                    <span style="float: left; width: 80%">
                        <asp:TextBox ID="txt_date" runat="server" Style="width: 96%; padding: 4px;"></asp:TextBox>
                        <Ajax1:CalendarExtender ID="CalendarExtender3" TargetControlID="txt_date" runat="server"
                            Format="dd-MM-yyyy" PopupButtonID="Image1"></Ajax1:CalendarExtender>
                    </span>
                </td>
                <td class="field" style="width: 10%">Loading Number:
                </td>
                <td class="input-field" style="width: 20%;">
                    <span style="float: left; width: 80%">
                        <asp:TextBox ID="txt_loading" runat="server" Style="width: 96%; padding: 4px;"></asp:TextBox>
                    </span>
                </td>
                <td class="field" style="width: 10%;">Seal Number
                </td>
                <td class="input-field" style="width: 20%;">
                    <span style="float: left; width: 80%">
                        <asp:TextBox ID="txt_sealNo" runat="server" Style="width: 96%; padding: 4px;"></asp:TextBox>
                    </span>
                </td>
            </tr>
            <tr>
                <%--<td class="field">Create Date: </td>
                    <td class="input-field">
                        <asp:TextBox ID="dd_start_date" runat="server" CssClass="med-field" MaxLength="10"></asp:TextBox>
                            <Ajax1:CalendarExtender ID="CalendarExtender3" TargetControlID="dd_start_date" runat="server"
                                Format="yyyy-MM-dd" PopupButtonID="Image1">
                            </Ajax1:CalendarExtender>  
                    </td>
                    <td class="space"></td> --%>
                <td class="field" style="width: 10%">Destination:
                </td>
                <td class="input-field" style="width: 20%;">
                    <span style="float: left; width: 80%">
                        <asp:DropDownList ID="dd_destination" runat="server" AppendDataBoundItems="true" Style="width: 96%; padding: 4px;">
                            <asp:ListItem Value="0">.::Select Destination ::.</asp:ListItem>
                        </asp:DropDownList>
                    </span>
                </td>
                <td class="field" style="width: 10%">Transport Type
                </td>
                <td class="input-field" style="width: 20%;">
                    <span style="float: left; width: 80%">
                        <asp:DropDownList ID="dd_transportType" runat="server" Style="width: 96%; padding: 4px;"
                            AppendDataBoundItems="true">
                            <asp:ListItem Value="0">.::Select Transport Type::.</asp:ListItem>
                        </asp:DropDownList>
                    </span>
                </td>

            </tr>
            <tr>
                <td class="field">
                    <asp:Button ID="submit" runat="server" Text="Search" CssClass="button1" OnClick="Btn_Save_Click" />
                </td>
            </tr>
        </table>
        <span id="Table_1" class="tbl-large">
            <asp:GridView ID="GridView" runat="server" AutoGenerateColumns="false" CssClass="mGrid"
                AlternatingRowStyle-CssClass="alt" BorderColor="#DEDFDE" BorderStyle="None" AllowPaging="false"
                BorderWidth="1px">
                <Columns>
                    <asp:TemplateField HeaderText="S.No." ItemStyle-Width="2%">
                        <ItemTemplate>
                            <asp:Label ID="lblRowNumber" Text='<%# Container.DataItemIndex + 1 %>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Width="5%" HeaderText="Loading Number" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:HyperLink ID="HyperLink1" CssClass="edit" runat="server" Target="_blank" Text='<%# DataBinder.Eval(Container.DataItem, "id")%>'
                                NavigateUrl='<%# "LoadingPrint.aspx?Xcode=" + Eval("id")%>'></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="date" DataField="date" ItemStyle-HorizontalAlign="Center"
                        ItemStyle-Width="10%"></asp:BoundField>
                    <asp:BoundField HeaderText="Transport Type" DataField="TransportType" ItemStyle-HorizontalAlign="Center"
                        ItemStyle-Width="10%"></asp:BoundField>
                    <asp:BoundField HeaderText="Vehicle Name" DataField="VehicleName" ItemStyle-HorizontalAlign="Center"
                        ItemStyle-Width="10%"></asp:BoundField>
                    <asp:BoundField HeaderText="courier Name" DataField="courierName" ItemStyle-HorizontalAlign="Center"
                        ItemStyle-Width="10%"></asp:BoundField>
                    <asp:BoundField HeaderText="Orign Name" DataField="OrgName" ItemStyle-HorizontalAlign="Center"
                        ItemStyle-Width="10%"></asp:BoundField>
                    <asp:BoundField HeaderText="Destination Name" DataField="DestName" ItemStyle-HorizontalAlign="Center"
                        ItemStyle-Width="10%"></asp:BoundField>
                    <asp:BoundField HeaderText="description" DataField="description" ItemStyle-HorizontalAlign="Center"
                        ItemStyle-Width="10%"></asp:BoundField>
                    <asp:BoundField HeaderText="flight No" DataField="flightNo" ItemStyle-HorizontalAlign="Center"
                        ItemStyle-Width="10%"></asp:BoundField>
                    <asp:BoundField HeaderText="seal no" DataField="sealno" ItemStyle-HorizontalAlign="Center"
                        ItemStyle-Width="10%"></asp:BoundField>
                    <asp:BoundField HeaderText="departure flight Date / Time" DataField="departureflightdate"
                        ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%"></asp:BoundField>
                    <asp:BoundField HeaderText="Total Weight" DataField="TotalWeight"
                        ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%"></asp:BoundField>
                    <asp:BoundField HeaderText="User Name" DataField="CreatedBy"
                        ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%"></asp:BoundField>
                </Columns>
            </asp:GridView>
        </span>
    </div>
</asp:Content>
