<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeBriefing_Tracking.aspx.cs" Inherits="MRaabta.Files.DeBriefing_Tracking" MasterPageFile="~/BtsMasterPage.master" %>

<%@ Register TagPrefix="Ajax1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div>
        <table cellpadding="0" cellspacing="0" width='100%' class='mGrid_Table'>
            <tr>
                <td colspan="12" align="center" class="head_column">
                    <h3>
                        Consignment Tracking History
                    </h3>
                </td>
            </tr>
        </table>
        <div style="float: left; position: relative; top: 23px; left: 3%;">
            <asp:ImageButton ID="btn_excel" runat="server" ImageUrl="~/images/ExcelFile.png"
                Width="48" OnClick="ExportToExcel" Visible="false" />
        </div>
        <br />
        <asp:Literal ID="lt_graph" runat="server"></asp:Literal>
        <span>
            <asp:Label ID="error_msg" runat="server" CssClass="error_msg"></asp:Label>
            <div class="report_name" style="float: left;text-align: center;width: 96%;">
                <asp:Label ID="lbl_live_msg" runat="server" CssClass="lbl_rep"></asp:Label><br />
                <asp:Label ID="lbl_report_name" runat="server" CssClass="lbl_rep"></asp:Label>
                <asp:Label ID="lbl_report_version" runat="server"></asp:Label>
            </div>
        </span><span id="Span2" class="tbl-large" style="float:left">
            <asp:GridView ID="GV" runat="server" AutoGenerateColumns="true" CssClass="mGrid floatgrid"
                AlternatingRowStyle-CssClass="alt" BorderColor="#DEDFDE" BorderStyle="None" AllowPaging="true" ShowFooter="true"
                BorderWidth="1px" PageSize="200" OnPageIndexChanging="GridView2_PageIndexChanging">
                <Columns>
                    <asp:TemplateField HeaderText="S.No." ItemStyle-Width="2%">
                        <ItemTemplate>
                            <asp:Label ID="lblRowNumber" Text='<%# Container.DataItemIndex + 1 %>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--<asp:BoundField HeaderText="Transaction Time" DataField="transactionTime" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5%"></asp:BoundField>
                    <asp:BoundField HeaderText="Event" DataField="TrackingStatus" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="10%"></asp:BoundField>
                    <asp:BoundField HeaderText="Location" DataField="currentLocation" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5%"></asp:BoundField>
                    <asp:BoundField HeaderText="Message" DataField="Detail" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="50%"></asp:BoundField>--%>
                </Columns>
            </asp:GridView>
        </span>
    </div>
</asp:Content>
