<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="Search_Unloading.aspx.cs" Inherits="MRaabta.Files.Search_Unloading" %>

<%@ Register TagPrefix="Ajax1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .style1
        {
            float: left;
            width: 29%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div>
        <table cellpadding="0" cellspacing="0" width='100%' class='mGrid_Table'>
            <tr>
                <td colspan="12" align="center" class="head_column">
                    <h3>
                        Search Unloading</h3>
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
                <td class="field" style="width: 7%">
                    Date:
                </td>
                <td class="style1">
                    <span style="float: left; width: 80%">
                        <asp:TextBox ID="txt_date" runat="server" Style="padding: 4px;" 
                        Width="222px"></asp:TextBox>
                        <Ajax1:CalendarExtender ID="CalendarExtender3" TargetControlID="txt_date" runat="server"
                            Format="dd-MM-yyyy" PopupButtonID="Image1">
                        </Ajax1:CalendarExtender>
                    </span>
                </td>
                <td class="field">
                    <asp:Button ID="submit" runat="server" Text="Search" CssClass="button1" 
                        onclick="submit_Click" Width="84px" />
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
                    <asp:TemplateField ItemStyle-Width="5%" HeaderText="UnLoading Number" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:HyperLink ID="HyperLink1" CssClass="edit" runat="server" Target="_blank" Text='<%# DataBinder.Eval(Container.DataItem, "id")%>'
                                NavigateUrl='<%# "Print_Unloading.aspx?Xcode=" + Eval("id")%>'></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Reference Loading Number" DataField="RefLoadingID" ItemStyle-HorizontalAlign="Center"
                        ItemStyle-Width="10%"></asp:BoundField>
                    <asp:BoundField HeaderText="Origin Name" DataField="origin" ItemStyle-HorizontalAlign="Center"
                        ItemStyle-Width="10%"></asp:BoundField>
                    <asp:BoundField HeaderText="Destination Name" DataField="destination" ItemStyle-HorizontalAlign="Center"
                        ItemStyle-Width="10%"></asp:BoundField>
                    <asp:BoundField HeaderText="Date" DataField="Date" ItemStyle-HorizontalAlign="Center"
                        ItemStyle-Width="10%"></asp:BoundField>
                </Columns>
            </asp:GridView>
        </span>
    </div>
</asp:Content>
