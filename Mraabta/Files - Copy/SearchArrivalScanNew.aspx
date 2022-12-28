<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="SearchArrivalScanNew.aspx.cs" Inherits="MRaabta.Files.SearchArrivalScanNew" %>

<%@ Register TagPrefix="Ajax1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">   

        <div>
            <table cellpadding="0" cellspacing="0" width='100%' class='mGrid_Table'>
                <tr>
                    <td colspan="12" align="center" class="head_column">
                       <h3> Search Arrival Scan</h3>
                    </td>
                </tr>            
            </table>            

            <table class="input-form" style="width:90%">                     
                <tr>
                    <td class="field">Create Date: </td>
                    <td class="input-field">
                        <asp:TextBox ID="dd_start_date" runat="server" CssClass="med-field" MaxLength="10"></asp:TextBox>
                            <Ajax1:CalendarExtender ID="CalendarExtender3" TargetControlID="dd_start_date" runat="server"
                                Format="yyyy-MM-dd" PopupButtonID="Image1">
                            </Ajax1:CalendarExtender>  
                    </td>
                    <td class="space"></td>   
                    <td class="field">Rider Code: </td>
                    <td class="input-field" style="width: 33%;">
                        <span style="float:left;width:80%">
                            <asp:TextBox ID="txt_ridercode" runat="server" AutoPostBack="true" style="width: 96%; padding: 4px;"></asp:TextBox>
                        </span>
                    </td>
                </tr>
                <tr>
                    <td class="field">
                        <asp:Button ID="submit" runat="server" Text="Search" CssClass="button1" OnClick="Btn_Save_Click" />
                    </td>
                </tr>
            </table>

            <span id="Table_1"  class="tbl-large"> 
                <asp:GridView ID="GridView" runat="server" AutoGenerateColumns="false"
                    CssClass="mGrid" AlternatingRowStyle-CssClass="alt" BorderColor="#DEDFDE" BorderStyle="None"
                    AllowPaging="false" BorderWidth="1px">
                    <Columns>
                        <asp:TemplateField ItemStyle-Width="5%" HeaderText="Id" ItemStyle-HorizontalAlign="Center" >
                            <ItemTemplate>
                                <asp:HyperLink ID="HyperLink1" CssClass="edit" runat="server" Target="_blank" Text='<%# DataBinder.Eval(Container.DataItem, "id")%>' NavigateUrl='<%# "Arrival_print_speedy.aspx?Xcode=" + Eval("ID")%>'></asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:BoundField HeaderText="branch code" DataField="branchcode" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%"></asp:BoundField>  
                        <asp:BoundField HeaderText="express center" DataField="expresscenter" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%"></asp:BoundField>  
                        <asp:BoundField HeaderText="rider code" DataField="ridercode" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%"></asp:BoundField>  
                        <asp:BoundField HeaderText="weight" DataField="weight" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%"></asp:BoundField>  
                        <asp:BoundField HeaderText="create on" DataField="createon" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%"></asp:BoundField>  
                        <asp:BoundField HeaderText="Consignment Count" DataField="count" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%"></asp:BoundField>  
                    </Columns>
                                                      
                </asp:GridView>	                            
            </span>

        </div>
</asp:Content>
