<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="SearchBags.aspx.cs" Inherits="MRaabta.Files.SearchBags" %>

<%@ Register TagPrefix="Ajax1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div>
        <table cellpadding="0" cellspacing="0" width='100%' class='mGrid_Table'>
            <tr>
                <td colspan="12" align="center" class="head_column">
                    <h3>
                        Search Bags</h3>
                </td>
            </tr>
        </table>
        <table class="input-form" style="width: 90%">
            <tr>
                <td class="field">
                    Create Date:
                </td>
                <td class="input-field">
                    <asp:TextBox ID="dd_start_date" runat="server" CssClass="med-field" MaxLength="10"></asp:TextBox>
                    
                    <Ajax1:CalendarExtender ID="CalendarExtender3" TargetControlID="dd_start_date" runat="server"
                        Format="yyyy-MM-dd" PopupButtonID="Image1">
                    </Ajax1:CalendarExtender>
                </td>
                <td class="space">
                </td>
                <td class="field">
                    <b>Search Criteria</b>
                </td>
                <td>
                    <asp:DropDownList ID="ddl_critera" runat="server" Width="200px" hieght="50px">
                        <asp:ListItem Value="1">Bag Number</asp:ListItem>
                        <asp:ListItem Value="2">Seal Number</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td style="width: 15%">
                </td>
            </tr>
            <tr>
                <td class="field">
                    
                </td>
                <td class="input-field">
                    
                </td>
                <td class="space">
                </td>
                <td class="field" id="num" runat="server">
                    Number:
                </td>
                <td>
                    <span style="float: left; width: 100%">
                        <asp:TextBox ID="txt_bag" runat="server" AutoPostBack="true" Style="width: 96%; padding: 4px;"></asp:TextBox>
                    </span>
                </td>
                <td style="width: 15%">
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
                    <asp:TemplateField ItemStyle-Width="5%" HeaderText="bag Number" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:HyperLink ID="HyperLink1" CssClass="edit" runat="server" Target="_blank" Text='<%# DataBinder.Eval(Container.DataItem, "bagNumber")%>'
                                NavigateUrl='<%# "Bag_speedy_Print.aspx?Xcode=" + Eval("bagNumber")%>'></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="total weight" DataField="totalweight" ItemStyle-HorizontalAlign="Center"
                        ItemStyle-Width="10%"></asp:BoundField>
                    <asp:BoundField HeaderText="orign" DataField="orign" ItemStyle-HorizontalAlign="Center"
                        ItemStyle-Width="10%"></asp:BoundField>
                    <asp:BoundField HeaderText="destination" DataField="destination" ItemStyle-HorizontalAlign="Center"
                        ItemStyle-Width="10%"></asp:BoundField>
                    <asp:BoundField HeaderText="seal no" DataField="sealno" ItemStyle-HorizontalAlign="Center"
                        ItemStyle-Width="10%"></asp:BoundField>
                    <asp:BoundField HeaderText="create on" DataField="createon" ItemStyle-HorizontalAlign="Center"
                        ItemStyle-Width="10%"></asp:BoundField>
                    <%--<asp:BoundField HeaderText="Manifest Count" DataField="manifestcount" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%"></asp:BoundField>  
                        <asp:BoundField HeaderText="Consignment Count" DataField="cncount" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%"></asp:BoundField>  --%>
                </Columns>
            </asp:GridView>
        </span>
    </div>
</asp:Content>
