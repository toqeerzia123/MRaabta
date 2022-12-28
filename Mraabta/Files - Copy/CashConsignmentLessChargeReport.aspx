<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.Master" CodeBehind="CashConsignmentLessChargeReport.aspx.cs" Inherits="MRaabta.Files.CashConsignmentLessChargeReport" %>

<%@ Register TagPrefix="Ajax1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <title>POD Daily Status</title>
    <script src="../Js/FusionCharts.js" type="text/javascript"></script>
    <body>
        <div>
            <table cellpadding="0" cellspacing="0" width='100%' class='mGrid_Table'>
                <tr>
                    <td colspan="12" align="center" class="head_column">
                        <h3>Cash Consignment Less Charge Report</h3>
                    </td>
                </tr>
                <tr>
                    <td class="head_column_panel" width="15%">
                        <div>Select Year</div>
                        <div>
                            <asp:DropDownList ID="dd_year" runat="server" CssClass="dropdown">
                                <asp:ListItem Value="">Select Year</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </td>
                    <td class="head_column_panel" width="15%">
                        <div>Select Month</div>
                        <div>
                            <asp:DropDownList ID="dd_Month" runat="server" CssClass="dropdown">
                                <asp:ListItem Value="">Select Month</asp:ListItem>
                                <asp:ListItem Value="01">Jan</asp:ListItem>
                                <asp:ListItem Value="02">Feb</asp:ListItem>
                                <asp:ListItem Value="03">Mar</asp:ListItem>
                                <asp:ListItem Value="04">Apr</asp:ListItem>
                                <asp:ListItem Value="05">May</asp:ListItem>
                                <asp:ListItem Value="06">Jun</asp:ListItem>
                                <asp:ListItem Value="07">Jul</asp:ListItem>
                                <asp:ListItem Value="08">Aug</asp:ListItem>
                                <asp:ListItem Value="09">Sep</asp:ListItem>
                                <asp:ListItem Value="10">Oct</asp:ListItem>
                                <asp:ListItem Value="11">Nov</asp:ListItem>
                                <asp:ListItem Value="12">Dec</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </td>

                    <td class="head_column_panel" width="15%">
                        <div>Zone</div>
                        <div>
                            <asp:ListBox ID="dd_zone" runat="server" SelectionMode="Multiple" CssClass="dropdown" AppendDataBoundItems="true"
                                AutoPostBack="true" OnSelectedIndexChanged="branch_SelectedIndexChanged"></asp:ListBox>
                            <asp:CheckBox ID="zone_chk" runat="server" AutoPostBack="true"
                                OnCheckedChanged="zone_chk_CheckedChanged" />
                            ALL
                        </div>
                    </td>

                    <td class="head_column_panel" width="15%">
                        <div>Branch</div>
                        <div>
                            <asp:ListBox ID="dd_branch" runat="server" SelectionMode="Multiple" CssClass="dropdown"></asp:ListBox>
                            <asp:CheckBox ID="branch_chk" runat="server"
                                AutoPostBack="true" OnCheckedChanged="branch_chk_CheckedChanged" />
                            ALL
                        </div>
                    </td>

                    <td class="head_column_panel" width="15%">
                        <div>Output Type</div>
                        <div>
                            <asp:DropDownList ID="type" runat="server" CssClass="dropdown">
                                <asp:ListItem Value="">Select Output Type</asp:ListItem>
                                <asp:ListItem Value="html" Selected="True">HTML</asp:ListItem>
                                <asp:ListItem Value="excel">Excel</asp:ListItem>
                                <asp:ListItem Value="pdf">PDF</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </td>
                    <td class="head_column_panel">
                        <asp:Button ID="Button1" runat="server" Text="Show Data" CssClass="button" OnClick="Btn_Search_Click" />
                    </td>
                </tr>
            </table>
            <br />
            <span id="Table_1" class="tbl-large">
                <div class="report_name">
                    <asp:Label ID="lbl_report_name" runat="server" CssClass="lbl_rep"></asp:Label>
                    <br />
                    <asp:Label ID="lbl_total_record" runat="server" CssClass="lbl_rep"></asp:Label>
                    <br />
                    <asp:Label ID="lbl_report_version" runat="server"></asp:Label>
                </div>

                <asp:Label ID="error_msg" runat="server" CssClass="error_msg"></asp:Label>

                <asp:GridView ID="GridView" runat="server" AutoGenerateColumns="true"
                    CssClass="mGrid" AlternatingRowStyle-CssClass="alt" BorderColor="#DEDFDE" BorderStyle="None"
                    AllowPaging="true" BorderWidth="1px" PageSize="200" OnPageIndexChanging="GridView2_PageIndexChanging" ShowFooter="true">
                    <Columns>
                        <asp:TemplateField HeaderText="S.No." ItemStyle-Width="2%">
                            <ItemTemplate>
                                <asp:Label ID="lblRowNumber" Text='<%# Container.DataItemIndex + 1 %>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%-- <asp:BoundField HeaderText="CONSIGNMENT NUMBER" DataField="CONSIGNMENTNUMBER" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%"></asp:BoundField>
                            <asp:BoundField HeaderText="BOOKING DATE" DataField="BOOKINGDATE" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20%"></asp:BoundField>
                            <asp:BoundField HeaderText="ORIGN BRANCH" DataField="ORIGNBRANCH" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20%"></asp:BoundField>
                            <asp:BoundField HeaderText="RIDER CODE" DataField="RIDERCODE" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20%"></asp:BoundField>
                            <asp:BoundField HeaderText="RIDER NAME" DataField="RIDERNAME" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20%"></asp:BoundField>
                            <asp:BoundField HeaderText="DESTINATION BRANCH" DataField="DESTINATIONBRANCH" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20%"></asp:BoundField>
                            <asp:BoundField HeaderText="WEIGHT" DataField="WEIGHT" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20%"></asp:BoundField>
                            <asp:BoundField HeaderText="TOTAL AMOUNT" DataField="TAMOUNT" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20%"></asp:BoundField>    --%>
                    </Columns>
                </asp:GridView>


                <asp:GridView ID="excelgg" runat="server" AutoGenerateColumns="true"
                    CssClass="mGrid" AlternatingRowStyle-CssClass="alt" BorderColor="#DEDFDE" BorderStyle="None"
                    BorderWidth="1px" ShowFooter="true">
                    <Columns>
                        <asp:TemplateField HeaderText="S.No." ItemStyle-Width="2%">
                            <ItemTemplate>
                                <asp:Label ID="lblRowNumber" Text='<%# Container.DataItemIndex + 1 %>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--<asp:BoundField HeaderText="CONSIGNMENT NUMBER" DataField="CONSIGNMENTNUMBER" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%"></asp:BoundField>
                            <asp:BoundField HeaderText="BOOKING DATE" DataField="BOOKINGDATE" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20%"></asp:BoundField>
                            <asp:BoundField HeaderText="ORIGN BRANCH" DataField="ORIGNBRANCH" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20%"></asp:BoundField>
                            <asp:BoundField HeaderText="RIDER CODE" DataField="RIDERCODE" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20%"></asp:BoundField>
                            <asp:BoundField HeaderText="RIDER NAME" DataField="RIDERNAME" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20%"></asp:BoundField>
                            <asp:BoundField HeaderText="DESTINATION BRANCH" DataField="DESTINATIONBRANCH" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20%"></asp:BoundField>
                            <asp:BoundField HeaderText="WEIGHT" DataField="WEIGHT" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20%"></asp:BoundField>
                            <asp:BoundField HeaderText="TOTAL AMOUNT" DataField="TAMOUNT" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20%"></asp:BoundField> --%>
                    </Columns>
                </asp:GridView>
            </span>
        </div>
</asp:Content>
