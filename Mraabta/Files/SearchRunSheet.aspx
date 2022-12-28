<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SearchRunSheet.aspx.cs" Inherits="MRaabta.Files.SearchRunSheet" MasterPageFile="~/BtsMasterPage.master" %>

<%@ Register TagPrefix="Ajax1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">   
<style>

.print
{
    background: #f27031 none repeat scroll 0 0;
    color: #fff;
    margin: 0 10px;
    padding: 5px;
    text-decoration: none;
    border-radius: 5px;
}

.print:hover
{
    text-shadow: 1px 1px 1px #000;
}
.mGrid td {
    padding: 8px 5px;
}
</style>
        <div>
            <table cellpadding="0" cellspacing="0" width='100%' class='mGrid_Table'>
                <tr>
                    <td colspan="12" align="center" class="head_column">
                       <h3> Search RunSheet</h3>
                    </td>
                </tr>            
            </table> 
            <div>
            <asp:Label ID="Errorid" runat="server" ForeColor="Red"></asp:Label>
            </div>
            <table class="input-form" style="width:90%">                     
                <tr>                                      
                    <td class="field">RunSheet Date: </td>
                    <td class="input-field">
                        <asp:TextBox ID="dd_start_date" runat="server" CssClass="med-field" MaxLength="10"></asp:TextBox>
                            <Ajax1:CalendarExtender ID="CalendarExtender3" TargetControlID="dd_start_date" runat="server"
                                Format="yyyy-MM-dd" PopupButtonID="Image1">
                            </Ajax1:CalendarExtender>  
                    </td>
                    <td class="space"></td>
                                                          
                    <td class="field">Route: </td>
                    <td class="input-field">
                        <asp:TextBox ID="txt_route" runat="server" CssClass="med-field" ></asp:TextBox>
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
                    AllowPaging="false" BorderWidth="1px" style="margin: 0 0 0 10px;">
                    <Columns>
                        <asp:TemplateField HeaderText = "S.No." ItemStyle-Width="2%">
                            <ItemTemplate>
                                <asp:Label ID="lblRowNumber" Text='<%# Container.DataItemIndex + 1 %>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
<%--
                        <asp:TemplateField ItemStyle-Width="5%" HeaderText="Runsheet No" ItemStyle-HorizontalAlign="Center" >
                            <ItemTemplate>
                                <asp:HyperLink ID="HyperLink1" CssClass="edit" runat="server" Target="_blank" Text='<%# DataBinder.Eval(Container.DataItem, "runsheetNumber")%>' NavigateUrl='<%# "RunsheetInvoice.aspx?Xcode=" + Eval("runsheetNumber") + "&RCode="+ Eval("routeCode")%>'></asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateField>  
--%>
                        <asp:BoundField HeaderText="runsheet Number" DataField="runsheetNumber" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%"></asp:BoundField>  
                        <asp:BoundField HeaderText="runsheet Date" DataField="runsheetDate" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%"></asp:BoundField>  
                        <asp:BoundField HeaderText="route Code" DataField="routeCode" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%"></asp:BoundField>  
                        <asp:BoundField HeaderText="Consignment Count" DataField="cncount" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%"></asp:BoundField>  

                        <asp:TemplateField ItemStyle-Width="5%" HeaderText="Print Option" ItemStyle-HorizontalAlign="Center" >
                            <ItemTemplate>
                                <asp:HyperLink ID="HyperLink1" CssClass="print" runat="server" Target="_blank" Text='Pre Printed' NavigateUrl='<%# "RunsheetInvoice.aspx?Xcode=" + Eval("runsheetNumber") + "&RCode="+ Eval("routeCode")%>'></asp:HyperLink>
                                <asp:HyperLink ID="HyperLink2" CssClass="print" runat="server" Target="_blank" Text='Printed on Plain Paper' NavigateUrl='<%# "RunsheetInvoice_Plain.aspx?Xcode=" + Eval("runsheetNumber") + "&RCode="+ Eval("routeCode")%>'></asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateField> 
                         

                    </Columns>
                                                      
                </asp:GridView>	                            
            </span>

        </div>
</asp:Content>
