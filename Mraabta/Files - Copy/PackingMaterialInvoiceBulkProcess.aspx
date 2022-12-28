<%@ Page Language="C#" Title="Packing Material Invoice Generate" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="PackingMaterialInvoiceBulkProcess.aspx.cs" Inherits="MRaabta.Files.PackingMaterialInvoiceBulkProcess" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="../Bts_Css/Mazen.css" rel="stylesheet" />
    <asp:UpdatePanel runat="server" ID="panel1">
        <ContentTemplate>
            <div class="row main-body newPanel">
                <fieldset class="fieldsetSmall">
                    <span id="ContentPlaceHolder1_Errorid" style="color: Red; font-weight: bold;"></span>
                    <legend style="font-size: medium;"><b>Packing Material Invoice Bulk Generate</b></legend>
                    <div class="btn_div" style="margin: 15px 0px;">
                        <asp:Button ID="btnRunScheduler" CssClass="btn btn-info"
                            runat="server" OnClick="btnRunScheduler_Click" Text="Genearate Invocie" />
                    </div>
                    <div class="col-lg-12 col-md-12 col-sm-12">
                        <table style="width: 20%">
                            <tr>
                                <td>
                                    <asp:DropDownList ID="ddlStatus" runat="server">
                                        <asp:ListItem Text="Pending Invoice" Selected="True" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="Invoice Saved" Value="4"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Button ID="btnSubmit" OnClick="btnSubmit_Click" CssClass="btn btn-success"
                                        runat="server" Text="Search" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <br />
                    <b>
                        <asp:LinkButton ID="lb_selectAll" runat="server" Text="Select All" OnClick="lb_selectAll_Click"
                            CssClass="btn-link"></asp:LinkButton>
                        <asp:LinkButton ID="lb_clearAll" runat="server" Text="Clear All" OnClick="lb_clearAll_Click"
                            CssClass="btn-link"></asp:LinkButton></b>
                    <br />
                    <asp:GridView ID="grPML" runat="server" AutoGenerateColumns="false" AllowPaging="True"
                        OnPageIndexChanging="grPML_PageIndexChanging" Font-Size="Medium" CellPadding="4" ForeColor="#333333" GridLines="None" Font-Bold="True" Width="100%"
                        EnableTheming="False" BorderColor="#DA7A4D" BorderStyle="Solid" BorderWidth="5px" EmptyDataText="No Request Generated">
                        <Columns>
                            <asp:TemplateField HeaderText="">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chk_chk" runat="server" AutoPostBack="false" Width="20px" />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Packing<br>Request ID" Visible="true">
                                <ItemTemplate>
                                    <asp:LinkButton ID="linkRequesID" runat="server" OnClick="linkRequesID_Click" Text='<%# Bind("PackingRequestID") %>'>  
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <%--<asp:BoundField DataField="PackingRequestID" HeaderText="Packing<br>Request ID" />--%>

                            <%--<asp:BoundField DataField="PackingRequestID" HeaderText="Packing Request ID" ItemStyle-HorizontalAlign="Left"
                                HeaderStyle-HorizontalAlign="Left" SortExpression="PackingRequestID" />--%>

                            <asp:TemplateField HeaderText="Account NO" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblAccountNO" runat="server" Text='<%# Bind("accountNo") %>'>  
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Account Name" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblAccountName" runat="server" Text='<%# Bind("name") %>'>  
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Location Name" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblLocationName" runat="server" Text='<%# Bind("LocationID") %>'>  
                                    <%--<asp:Label ID="lblLocationId" runat="server" Text='<%# Bind("accountNo") %>' Visible="false"> --%> 
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="TotalAmount" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblTotalAmount" runat="server" Text='<%# Bind("TotalAmount") %>'>  
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Request Date" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblRequestDate" runat="server" Text='<%# Bind("RequestDate") %>'>  
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Approve Date" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblApproveDate" runat="server" Text='<%# Bind("ApprovedDate") %>'>  
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Issue Date" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblIssueDate" runat="server" Text='<%# Bind("IssuedDate") %>'>  
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Status" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("AppStatus") %>'>  
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                        </Columns>
                        <EditRowStyle BackColor="#999999" BorderColor="#DA7A4D" BorderStyle="Solid" BorderWidth="1px" />
                        <FooterStyle BackColor="#CC3399" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#DA7A4D" Font-Bold="True" ForeColor="White" BorderColor="White" BorderStyle="Solid" BorderWidth="1px" />
                        <PagerStyle BackColor="#DA7A4D" ForeColor="White" HorizontalAlign="Center" />
                        <RowStyle BackColor="#fbefe9" ForeColor="#333333" Font-Bold="false" HorizontalAlign="Center" BorderColor="#DA7A4D" BorderStyle="Solid" BorderWidth="1px" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="false" ForeColor="#333333" />
                        <SortedAscendingCellStyle BackColor="#E9E7E2" />
                        <SortedAscendingHeaderStyle BackColor="#506C8C" />
                        <SortedDescendingCellStyle BackColor="#FFFDF8" />
                        <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                    </asp:GridView>
                </fieldset>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
